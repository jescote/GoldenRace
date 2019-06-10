using System;

namespace DownloadFiles.Excepciones
{
	public class DirectorioToDisKException : Exception
	{
		public DirectorioToDisKException()
		{
		}

		public DirectorioToDisKException(string message)
			: base(message)
		{
		}

		public DirectorioToDisKException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
