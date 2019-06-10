using System;

namespace DownloadFiles.Excepciones
{
	public class ObtenerJsonFromRestServiceException : Exception
	{
		public ObtenerJsonFromRestServiceException()
		{
		}

		public ObtenerJsonFromRestServiceException(string message)
			: base(message)
		{
		}

		public ObtenerJsonFromRestServiceException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
