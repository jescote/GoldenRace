using Newtonsoft.Json.Linq;

namespace DownloadFiles.Interfaces
{
	public interface IObtenerJson
	{
		JObject GetJson(string ruta);
	}
}
