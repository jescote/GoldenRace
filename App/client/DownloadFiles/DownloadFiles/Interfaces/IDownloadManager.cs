using DownloadFiles.Datos;
using System.Threading.Tasks;

namespace DownloadFiles.Interfaces
{
	public interface IDownloadManager
	{
		Task<Resultado> DownloadManagerStart(string urlJson, string rutaDestino, ControlesProgeso controlesProgeso, bool conErrores);
		Task<Resultado> RecuperaErrores(ControlesProgeso controlesProgeso, bool conErrores);
		bool HayErrores();
		bool BorraFicheroRecuperacion();
	}
}
