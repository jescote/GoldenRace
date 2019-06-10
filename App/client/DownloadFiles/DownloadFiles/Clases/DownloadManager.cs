using DownloadFiles.Datos;
using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DownloadFiles.Clases
{
	public class DownloadManager : IDownloadManager
	{
		private ILog _logger;
		private IJsonManager _jsonManager;
		private IDirectorio _directorio;
		private ISaveFiles _saveFiles;
		private ISerializacion _serializacion;
		private ControlesProgeso _controlesProgeso;
		private bool _conErrores;

		public DownloadManager(ILogCreator logCreator, IJsonManager jsonManager, IDirectorio directorio, ISaveFiles saveFiles, ISerializacion serializacion)
		{
			_logger = logCreator.GetTipoLogger<frmDescargas>();
			_jsonManager = jsonManager;
			_directorio = directorio;
			_saveFiles = saveFiles;
			_serializacion = serializacion;
		}

		public async Task<Resultado> DownloadManagerStart(string urlJson, string rutaDestino, ControlesProgeso controlesProgeso, bool conErrores)
		{
			_controlesProgeso = controlesProgeso;
			_conErrores = conErrores;
			if (!rutaDestino.EndsWith(@"\")) rutaDestino += @"\";
			JObject json = _jsonManager.GetJson(urlJson);
			if (json != null)
			{
				EstructuraFicheros estructura = _jsonManager.ParseJson(json);
				estructura.UrlJson = urlJson;
				estructura.Root = rutaDestino;
				return await Descarga(estructura);
			}
			else
			{
				return SimpleInyectorEngine.Get<Resultado>();
			}
		}
		public async Task<Resultado> RecuperaErrores(ControlesProgeso controlesProgeso, bool conErrores)
		{
			_controlesProgeso = controlesProgeso;
			_conErrores = conErrores;
			EstructuraFicheros estructura = _serializacion.Deserializa<EstructuraFicheros>();
			if (!estructura.Root.EndsWith(@"\")) estructura.Root += @"\";
			return await Descarga(estructura);
		}

		public bool HayErrores()
		{
			return File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\ErrorDescarga.bin");
		}

		public bool BorraFicheroRecuperacion()
		{
			string fichero = string.Empty;
			try
			{
				fichero = System.AppDomain.CurrentDomain.BaseDirectory + @"\ErrorDescarga.bin";
				File.Delete(fichero);
				return true;
			}
			catch (Exception ex)
			{
				_logger.Error($"Error al eliminar el fichero de recuperacion: {fichero}", ex);
				return false;
			}

		}

		private async Task<Resultado> Descarga(EstructuraFicheros estructura)
		{
			Resultado ret = SimpleInyectorEngine.Get<Resultado>();
			IList<string> filesToDownload = GetFilesToDownload(estructura);

			if (filesToDownload != null)
			{
				_controlesProgeso.pbarProgeso.Minimum = 0;
				_controlesProgeso.pbarProgeso.Maximum = filesToDownload.Count();
				_controlesProgeso.pbarProgeso.BeginAnimation(RangeBase.ValueProperty, null);
				_saveFiles.FicheroProcesadoEvent += saveFiles_FicheroProcesadoEvent;
				if (estructura != null)
				{
					if (_directorio.CrearEstructuraDirectorios(estructura))
					{
						ret = await _saveFiles.Save(filesToDownload, estructura, _conErrores);
						if (!ret.ProcesoCorrecto)
						{
							_serializacion.Serializa(estructura);
						}
					}
				}
				_saveFiles.FicheroProcesadoEvent -= saveFiles_FicheroProcesadoEvent;
			}
			return ret;
		}

		private void saveFiles_FicheroProcesadoEvent(FicheroProcesado ficheroProcesado)
		{
			_controlesProgeso.pbarProgeso.Value = ficheroProcesado.Indice;
			_controlesProgeso.txtProcesando.Text = ficheroProcesado.Fichero;
		}

		private IList<string> GetFilesToDownload(EstructuraFicheros estructura)
		{
			IList<string> files_to_download;

			Uri aux_uri = new Uri(estructura.UrlJson, UriKind.Absolute);
			string url = aux_uri.AbsoluteUri.Replace(aux_uri.AbsolutePath, "") + "/" + "getfile?file=";

			files_to_download = new List<string>();

			foreach (KeyValuePair<string, IList<IDictionary<string, string>>> ruta in estructura.GetEstructura())
			{
				string path_estructura = ruta.Key;
				IList<IDictionary<string, string>> ficheros_estructura = ruta.Value;

				if (ficheros_estructura != null)
				{
					foreach (IDictionary<string, string> propiedades_fichero in ficheros_estructura)
					{

						if (propiedades_fichero["required"].ToLower().Equals("true"))
						{
							string file_to_download = estructura.Root + path_estructura + @"\" + propiedades_fichero["name"];

							if (!File.Exists(file_to_download))
							{
								files_to_download.Add(path_estructura + @"\" + propiedades_fichero["name"]);
							}
							else
							{
								using (var md5 = MD5.Create())
								{
									using (var stream = File.OpenRead(file_to_download))
									{
										string hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty);

										if (!hash.ToLower().SequenceEqual(propiedades_fichero["md5"].ToLower()))
										{
											files_to_download.Add(path_estructura + @"\" + propiedades_fichero["name"]);
										}
									}
								}
							}
						}

					}
				}
			}
			return files_to_download;
		}
	}
}
