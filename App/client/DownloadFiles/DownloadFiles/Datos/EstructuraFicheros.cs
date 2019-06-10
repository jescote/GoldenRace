using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadFiles.Datos
{
	[Serializable]
	public class EstructuraFicheros
	{
		private IDictionary<string, IList<IDictionary<string, string>>> _estructura = new Dictionary<string, IList<IDictionary<string, string>>>();
		private string _urlJson;
		private string _root;
		public EstructuraFicheros()
		{
		}

		public string UrlJson
		{
			get { return _urlJson; }
			set { _urlJson = value; }
		}

		public string Root
		{
			get { return _root; }
			set { _root = value; }
		}

		public IDictionary<string, IList<IDictionary<string, string>>> GetEstructura() { return _estructura; }
		public void SetEstructura(IDictionary<string, IList<IDictionary<string, string>>> estructura) { _estructura = estructura; }
		public void AddFicheros(string path, IList<IDictionary<string, string>> ficheros) { _estructura.Add(path, ficheros); }

		public void LimpiaEstructura(IList<string> ficheros)
		{
			foreach (string f in ficheros)
			{
				string ruta_fichero = f.Substring(0, f.LastIndexOf(@"\"));
				string name_fichero = f.Substring(f.LastIndexOf(@"\") + 1);
				bool eliminado = false;

				foreach (KeyValuePair<string, IList<IDictionary<string, string>>> ruta in _estructura)
				{
					string path_estructura = ruta.Key;
					IList<IDictionary<string, string>> ficheros_estructura = ruta.Value;

					if (ficheros_estructura != null)
					{
						int i = 0;
						for (i = 0; i < ficheros_estructura.Count; i++)
						{
							string fichero_estructura = ficheros_estructura[i].Values.FirstOrDefault(x => x == name_fichero);
							if (ficheros_estructura[i].Values.FirstOrDefault(x => x == name_fichero) == name_fichero && ruta_fichero.Equals(path_estructura))
							{
								ficheros_estructura.RemoveAt(i);
								eliminado = true;
								break;
							}
						}

					}
					if (eliminado) break;
				}
			}

			var keys_a_borrar = _estructura.Where(v => v.Value == null || v.Value.Count == 0).ToList();
			foreach (KeyValuePair<string, IList<IDictionary<string, string>>> ruta_a_borrar in keys_a_borrar)
			{
				_estructura.Remove(ruta_a_borrar.Key);
			}
		}
	}
}
