using DownloadFiles.Datos;
using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadFiles.Clases
{
	public class SaveFilesFromRestService : ISaveFiles
	{
		private ILog _logger;
		private Resultado _resultado;

		public delegate void FicheroProcesadoHandler(FicheroProcesado ficheroProcesado);
		public event FicheroProcesadoHandler FicheroProcesadoEvent;

		public SaveFilesFromRestService(ILogCreator logCreator)
		{
			_logger = _logger = logCreator.GetTipoLogger<SaveFilesFromRestService>();
		}

		public async Task<Resultado> Save(IList<string> filesToDownload, EstructuraFicheros estructura, bool conErrores)
		{
			_resultado = SimpleInyectorEngine.Get<Resultado>();

			Uri aux_uri = new Uri(estructura.UrlJson, UriKind.Absolute);
			string url = aux_uri.AbsoluteUri.Replace(aux_uri.AbsolutePath, "") + "/" + "getfile?file=";

			//pbar.Maximum = filesToDownload.Count();
			//pbar.Minimum = 0;
			//pbar.BeginAnimation(RangeBase.ValueProperty, null);

			IList<string> ficheros_ok = await GetFicherosFromApi(estructura.Root, url, filesToDownload, conErrores);
			estructura.LimpiaEstructura(ficheros_ok);

			_resultado.Procesados = filesToDownload.Count();
			_resultado.Correctos = _resultado.Procesados - _resultado.Fallidos;
			_resultado.ProcesoCorrecto = _resultado.Correctos == _resultado.Procesados;

			return _resultado;
		}

		private async Task<IList<string>> GetFicherosFromApi(string root, string url, IList<string> filesToDownload, bool conErrores)
		{
			IList<string> ret = new List<string>();
			HttpClient client_http = new HttpClient();

			int i = 0;
			foreach (string fichero in filesToDownload)
			{
				//await Task.Delay(TimeSpan.FromMilliseconds(200));				
				try
				{
					++i;
					FicheroProcesadoEvent(new FicheroProcesado
					{
						Indice = i,
						Fichero = fichero
					});
					var response = await client_http.GetAsync(url + fichero);

					if (conErrores && i % 10 == 0) throw new Exception("Probando errores");

					if (response.IsSuccessStatusCode)
					{
						using (var stream = await response.Content.ReadAsStreamAsync())
						{
							var file = new FileInfo(root + fichero);
							using (var file_stream = file.OpenWrite())
							{
								await stream.CopyToAsync(file_stream);
							}
						}
					}
					ret.Add(fichero);
				}
				catch (Exception ex)
				{
					_resultado.Fallidos++;
					_logger.Info($"Error al procesar el fichero: {root + fichero}", ex);
				}
			}
			return ret;
		}
	}
}
