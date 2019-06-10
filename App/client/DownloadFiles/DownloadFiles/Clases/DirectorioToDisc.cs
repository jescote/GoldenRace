using DownloadFiles.Datos;
using DownloadFiles.Excepciones;
using DownloadFiles.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadFiles.Clases
{
	public class DirectorioToDisK : IDirectorio
	{
		private readonly ILog _logger;
		private DirectoryInfo _dir_root;

		public DirectorioToDisK(ILogCreator logCreator)
		{
			_logger = logCreator.GetTipoLogger<DirectorioToDisK>();
		}

		public bool CrearEstructuraDirectorios(EstructuraFicheros estructura)
		{
			bool ret = true;
			string ruta = string.Empty;

			try
			{
				ruta = estructura.Root;
				_dir_root = new DirectoryInfo(ruta);
				if (!_dir_root.Exists) _dir_root.Create();

				foreach (KeyValuePair<string, IList<IDictionary<string, string>>> dir in estructura.GetEstructura())
				{
					ruta = _dir_root.FullName + @"\" + dir.Key;
					if (!Directory.Exists(ruta)) _dir_root.CreateSubdirectory(dir.Key);
				}
			}

			catch (Exception ex) when (ex is IOException)
			{
				throw new DirectorioToDisKException($"no se ha podido crear la ruta {ruta}", ex);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return ret;
		}



	}
}
