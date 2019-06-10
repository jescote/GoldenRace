using DownloadFiles.Interfaces;
using log4net;
using System;
using System.Collections.Generic;

namespace DownloadFiles.Clases
{
	public class LogCreator : ILogCreator
	{
		private static readonly IDictionary<Type, ILog> loggers = new Dictionary<Type, ILog>();
		private static readonly object objLock = new object();

		public ILog GetTipoLogger<T>() where T : class
		{
			var loggerType = typeof(T);
			if (loggers.ContainsKey(loggerType))
			{
				return loggers[typeof(T)];
			}

			lock (objLock)
			{
				if (loggers.ContainsKey(loggerType))
				{
					return loggers[typeof(T)];
				}
				var logger = LogManager.GetLogger(loggerType);
				loggers[loggerType] = logger;
				return logger;
			}

		}
	}
}
