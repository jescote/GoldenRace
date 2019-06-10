using DownloadFiles.Excepciones;
using DownloadFiles.Interfaces;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace DownloadFiles.Clases
{
	public class ObtenerJsonFromRestService : IObtenerJson
	{
		private readonly ILog _logger;
		public ObtenerJsonFromRestService(ILogCreator logCreator)
		{
			_logger = logCreator.GetTipoLogger<ObtenerJsonFromRestService>();
		}

		public JObject GetJson(string ruta)
		{
			try
			{
				return JObject.Parse(new WebClient().DownloadString(new Uri(ruta, UriKind.Absolute)));
			}
			catch (Exception ex) when (ex is ArgumentException || ex is UriFormatException || ex is ArgumentNullException)
			{
				throw new ObtenerJsonFromRestServiceException($"La url indicada no es una url correcta: {ruta}", ex);
			}
			catch (WebException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
