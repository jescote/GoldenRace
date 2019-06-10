using DownloadFiles.Datos;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DownloadFiles.Clases.SaveFilesFromRestService;

namespace DownloadFiles.Interfaces
{
	public interface ISaveFiles
	{
		Task<Resultado> Save(IList<string> filesToDownload, EstructuraFicheros estructura, bool conErrores);
		event FicheroProcesadoHandler FicheroProcesadoEvent;
	}
}
