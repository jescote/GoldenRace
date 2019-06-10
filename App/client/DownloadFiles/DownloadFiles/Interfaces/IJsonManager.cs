using DownloadFiles.Datos;
using Newtonsoft.Json.Linq;

namespace DownloadFiles.Interfaces
{
	public interface IJsonManager
	{
		JObject GetJson(string urlJson);
		EstructuraFicheros ParseJson(JObject json);
	}
}
