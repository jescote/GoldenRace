namespace DownloadFiles.Datos
{
	public class Resultado
	{
		public bool ProcesoCorrecto { get; set; }
		public int Procesados { get; set; }
		public int Correctos { get; set; }
		public int Fallidos { get; set; }
	}
}
