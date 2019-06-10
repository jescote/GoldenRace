using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadFiles.Inyeccion
{
	public class SimpleInyectorEngine
	{
		private static IDictionary<Type, Type> _mapeos = new Dictionary<Type, Type>();

		public static void Clear()
		{
			_mapeos.Clear();
		}

		public static void Map<T, V>() where V : T
		{
			_mapeos.Add(typeof(T), typeof(V));
		}

		public static T Get<T>()
		{
			var type = typeof(T);
			return (T)Get(type);
		}

		private static Type ResolveType(Type t)
		{
			if (_mapeos.ContainsKey(t))
			{
				return _mapeos[t];
			}
			return t;
		}

		private static object Get(Type t)
		{
			var tipo = ResolveType(t);
			var constructor = tipo.GetConstructors()[0];
			var parametros_tipo = constructor.GetParameters();

			IList<object> parametros = new List<object>();

			foreach (var item in parametros_tipo)
			{
				parametros.Add(Get(item.ParameterType));
			}

			return constructor.Invoke(parametros.ToArray());
		}

	}
}
