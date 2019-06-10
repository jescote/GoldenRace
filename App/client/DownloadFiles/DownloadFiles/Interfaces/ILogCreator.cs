using log4net;

namespace DownloadFiles.Interfaces
{
	public interface ILogCreator
	{
		ILog GetTipoLogger<T>() where T : class;
	}
}
