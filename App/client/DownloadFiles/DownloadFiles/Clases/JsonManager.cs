using DownloadFiles.Datos;
using DownloadFiles.Interfaces;
using DownloadFiles.Inyeccion;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DownloadFiles.Clases
{
	public class JsonManager : IJsonManager
	{
		private ILog _logger;
		private IObtenerJson _obtenerJson;
		private EstructuraFicheros _estructura;

		private IList<IDictionary<string, string>> _ficheros = new List<IDictionary<string, string>>();
		private IDictionary<string, string> _propiedades = null;
		private string _path = string.Empty;
		private string _path_actual = string.Empty;

		public JsonManager(ILogCreator logCreator, IObtenerJson obtenerJson)
		{
			_logger = _logger = logCreator.GetTipoLogger<JsonManager>();
			_obtenerJson = obtenerJson;
		}

		public JObject GetJson(string urlJson)
		{
			return _obtenerJson.GetJson(urlJson.Trim());
		}

		public EstructuraFicheros ParseJson(JObject json)
		{
			try
			{
				_estructura = SimpleInyectorEngine.Get<EstructuraFicheros>();
				Parsea(json, n =>
				{
					if (n.First != null && n.First.Type == JTokenType.Property && ((JProperty)n.First).Name.Equals("md5"))
					{
						_path_actual = Regex.Replace(n.Path, @" ?\[.*?\]", string.Empty).Replace(".", @"\");

						if (!_path_actual.Equals(_path) && !string.IsNullOrEmpty(_path))
						{
							_estructura.AddFicheros(_path, _ficheros);
							_ficheros = new List<IDictionary<string, string>>();
						}
						_path = _path_actual;
						_propiedades = new Dictionary<string, string>();
						foreach (JProperty prop in n.Children())
						{
							_propiedades.Add(prop.Name, (string)prop.Value);
						}
						_ficheros.Add(_propiedades);
					}
					else if (n.First.Last.Type == JTokenType.Array && !n.First.Last.HasValues)
					{
						string path_vacio = Regex.Replace(n.First.Last.Path, @" ?\[.*?\]", string.Empty).Replace(".", @"\");
						_estructura.AddFicheros(path_vacio, null);
					}
				});
				if (!_estructura.GetEstructura().ContainsKey(_path)) _estructura.AddFicheros(_path, _ficheros);
			}
			catch (Exception ex)
			{
				_estructura = null;
				_logger.Error($"Error al parsear el json\n{json.ToString()}");
				throw ex;
			}

			return _estructura;
		}

		private void Parsea(JToken node, Action<JObject> action)
		{
			if (node.Type == JTokenType.Object)
			{
				action((JObject)node);
				foreach (JProperty child in node.Children<JProperty>())
				{
					Parsea(child.Value, action);
				}
			}
			else if (node.Type == JTokenType.Array)
			{
				foreach (JToken child in node.Children())
				{
					Parsea(child, action);
				}
			}
		}


	}


}
