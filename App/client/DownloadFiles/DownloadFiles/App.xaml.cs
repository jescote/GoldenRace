using DownloadFiles.Clases;
using DownloadFiles.Datos;
using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadFiles
{
	/// <summary>
	/// Lógica de interacción para App.xaml
	/// </summary>
	public partial class App : Application
	{
		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ILog _logger;

		public App()
		{
			ILogCreator logCreator = new LogCreator();
			_logger = logCreator.GetTipoLogger<App>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			try
			{
				ConfiguracionInyector.Configura();
				_logger.Debug("Se inicia la aplicación");

				//throw new Exception("Su aplicación ha sufrido un fallo catastrófico, le tengo que pedir que abandone la fragua");
				var inicio = SimpleInyectorEngine.Get<IniciaApp>();
				inicio.Iniciar(e.Args.ToList());
			}
			catch (Exception ex)
			{
				_logger.Fatal("Error en la carga de la aplicación\n" + ex.Message + "\n" + ex.StackTrace);
				Application.Current.Shutdown();
			}
		}
	}
}
