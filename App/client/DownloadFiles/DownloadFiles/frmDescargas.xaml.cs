using DownloadFiles.Datos;
using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using System;
using System.Windows;

namespace DownloadFiles
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class frmDescargas : Window
	{
		private ILog _logger;
		private IDownloadManager _downloadManager;
		private bool _descargaAutomatica;
		public bool DescargaAutomatica { set { _descargaAutomatica = value; } }

		public frmDescargas(ILogCreator logCreator, IDownloadManager downloadManager)
		{
			InitializeComponent();

			_logger = logCreator.GetTipoLogger<frmDescargas>();
			_downloadManager = downloadManager;
		}

		private void frmDescargas_Loaded(object sender, RoutedEventArgs e)
		{
			string mensaje = string.Empty;
			try
			{
				if (_downloadManager.HayErrores())
				{
					btnRecuperar.IsEnabled = true;
					btnInicio.IsEnabled = false;
					MessageBoxResult result = MessageBox.Show("Hay un fichero de recuperación de errores, ¿ desea recuperar los errores anteriores ?", "", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.Yes)
					{
						RecuperarDescarga();
					}
					else
					{
						result = MessageBox.Show("Hay un fichero de recuperación de errores, ¿ desea eliminar el fichero de recuperación de errores ?", "", MessageBoxButton.YesNo);
						if (result == MessageBoxResult.Yes)
						{
							_downloadManager.BorraFicheroRecuperacion();
							btnRecuperar.IsEnabled = false;
							btnInicio.IsEnabled = true;
						}
					}
				}
				else
				{
					if (_descargaAutomatica)
					{
						Descarga();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
				MessageBox.Show("Error al tratar el fichero de recuperacion de errores");
			}
		}

		private void BtnInicio_Click(object sender, RoutedEventArgs e)
		{
			Descarga();
		}

		private async void Descarga()
		{
			string mensaje = string.Empty;
			Resultado resultado = null;
			MessageBoxButton botones = MessageBoxButton.OK;
			try
			{
				ControlesProgeso controles = SimpleInyectorEngine.Get<ControlesProgeso>();
				controles.pbarProgeso = pbarProgeso;
				controles.txtProcesando = txtProcesando;

				btnInicio.IsEnabled = false;
				resultado = await _downloadManager.DownloadManagerStart(txtUrlJson.Text.Trim(), txtDestino.Text.Trim(), controles, chkErrores.IsChecked.Value);
				mensaje = $"Estado de la descarga:\nTotal de ficheros a descargar: {resultado.Procesados}\nFicheros descargados: {resultado.Correctos}\nFicheros con error: {resultado.Fallidos}\nEstado: {(resultado.ProcesoCorrecto ? "Descarga correcta" : "Descarga con errorres")}";
				if (!resultado.ProcesoCorrecto)
				{
					btnRecuperar.IsEnabled = true;
					botones = MessageBoxButton.YesNo;
					mensaje += $"\n¿ Recuperar la descarga ?";
				}

			}
			catch (Exception ex) when (ex.GetType() != typeof(Exception))
			{
				_logger.Error(ex.Message, ex);
				mensaje = ex.Message;
				btnInicio.IsEnabled = true;
			}
			catch (Exception ex)
			{
				_logger.Error($"Error no controlado", ex);
				mensaje = ex.Message;
			}
			finally
			{
				if (!string.IsNullOrEmpty(mensaje))
				{
					MessageBoxResult result = MessageBox.Show(mensaje, "", botones);
					if (result == MessageBoxResult.Yes)
					{
						RecuperarDescarga();
					}
				}
			}
		}

		private void btnRecuperar_Click(object sender, RoutedEventArgs e)
		{
			RecuperarDescarga();
		}

		private async void RecuperarDescarga()
		{
			try
			{
				string mensaje = string.Empty;

				ControlesProgeso controles = SimpleInyectorEngine.Get<ControlesProgeso>();
				controles.pbarProgeso = pbarProgeso;
				controles.txtProcesando = txtProcesando;

				txtDestino.Text = string.Empty;
				txtUrlJson.Text = string.Empty;

				Resultado resultado = await _downloadManager.RecuperaErrores(controles, chkErrores.IsChecked.Value);
				mensaje = $"Estado de la recuparacion:\nTotal de ficheros a recuperar: {resultado.Procesados}\nFicheros recuperados: {resultado.Correctos}\nFicheros con error: {resultado.Fallidos}\nEstado: {(resultado.ProcesoCorrecto ? "Recuparacion correcta" : "Recuperación con errorres")}";
				MessageBox.Show(mensaje);
				if (resultado.ProcesoCorrecto)
				{
					_downloadManager.BorraFicheroRecuperacion();
					btnRecuperar.IsEnabled = false;
				}
				else
				{
					btnRecuperar.IsEnabled = true;
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Error no controlado al recuperar la descarga", ex);
			}
		}
	}
}
