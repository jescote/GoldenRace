using DownloadFiles.Clases;
using DownloadFiles.Interfaces;

namespace DownloadFiles.Inyeccion
{
	public class ConfiguracionInyector
	{
		public static void Configura()
		{
			// Configuración de mapeos de tipos para la inyeccion de dependencias

			SimpleInyectorEngine.Clear();
			SimpleInyectorEngine.Map<ILogCreator, LogCreator>();
			SimpleInyectorEngine.Map<IDownloadManager, DownloadManager>();
			SimpleInyectorEngine.Map<IObtenerJson, ObtenerJsonFromRestService>();
			SimpleInyectorEngine.Map<IJsonManager, JsonManager>();
			SimpleInyectorEngine.Map<IDirectorio, DirectorioToDisK>();
			SimpleInyectorEngine.Map<ISaveFiles, SaveFilesFromRestService>();
			SimpleInyectorEngine.Map<ISerializacion, Serializacion>();
		}
	}
}
