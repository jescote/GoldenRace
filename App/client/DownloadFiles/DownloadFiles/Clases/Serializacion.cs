using DownloadFiles.Interfaces;
using log4net;
using System;
using System.IO;

namespace DownloadFiles.Clases
{
	public class Serializacion : ISerializacion
	{
		private ILog _logger;
		private string _file;
		public Serializacion(ILogCreator logCreator)
		{
			_logger = logCreator.GetTipoLogger<Serializacion>();
			_file = System.AppDomain.CurrentDomain.BaseDirectory + @"\ErrorDescarga.bin";
		}

		public void Serializa<T>(T t)
		{
			try
			{
				using (Stream st = File.Open(_file, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					var forma_bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					forma_bin.Serialize(st, t);
				}
			}
			catch (Exception ex)
			{
				_logger.Error("Error: no se ha podido serializar el objeto", ex);
				throw ex;
			}
		}

		public T Deserializa<T>()
		{
			T ret = default(T);
			try
			{
				if (File.Exists(_file))
				{
					using (Stream st = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.None))
					{
						var forma_bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
						ret = (T)forma_bin.Deserialize(st);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.Error("Error: no se ha podido desserializar el objeto", ex);
				throw ex;
			}
			return ret;
		}
	}
}
