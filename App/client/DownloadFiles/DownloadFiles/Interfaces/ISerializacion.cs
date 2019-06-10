namespace DownloadFiles.Interfaces
{
	public interface ISerializacion
	{
		void Serializa<T>(T t);
		T Deserializa<T>();
	}
}
