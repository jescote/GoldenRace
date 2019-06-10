using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DownloadFiles.Clases
{
	public class IniciaApp : IIniciaApp
	{
		private ILog _logger;

		public IniciaApp(ILogCreator logCreator)
		{
			_logger = logCreator.GetTipoLogger<IniciaApp>();
		}

		public void Iniciar(IList<string> parametrosApp)
		{
			string mensaje = string.Empty;
			var window = SimpleInyectorEngine.Get<frmDescargas>();

			if (parametrosApp.Count == 2)
			{
				window.txtUrlJson.Text = parametrosApp[0];
				window.txtDestino.Text = parametrosApp[1];
				window.DescargaAutomatica = true;
			}
			else if (parametrosApp.Count == 1)
			{
				mensaje = "No se han especificado todos los parámetros: falta la ruta del Json o la ruta de la descarga";
			}
			else if (parametrosApp.Count > 2)
			{
				mensaje = "Demasiados parámetros";
			}

			if (!string.IsNullOrEmpty(mensaje))
			{
				MessageBox.Show(mensaje);
				throw new Exception("Error en los parametros");
			}
			else
			{
				window.Show();
			}

		}

	}
}
