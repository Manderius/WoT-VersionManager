using System;
using System.Collections.Generic;
using System.Reflection;

namespace VSUI.Utils
{
    class PageCache
    {
		private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

		public void AddInstance<T>(T instance)
		{
			_instances[typeof(T)] = instance;
		}

		private void CreateInner(Type t)
		{
			if (t.IsPrimitive || t == typeof(string))
			{
				throw new InvalidOperationException("Cannot create primitives");
			}
			if (_instances.ContainsKey(t))
			{
				return;
			}

			ConstructorInfo[] constr = t.GetConstructors();
			foreach (var ci in constr)
			{
				try
				{
					List<object> param = new List<object>();
					foreach (var pi in ci.GetParameters())
					{
						Type paramType = pi.ParameterType;
						CreateInner(paramType);
						param.Add(_instances[paramType]);
					}
					_instances[t] = Activator.CreateInstance(t, param.ToArray());
					return;
				}
				catch (Exception) { }
			}

			_instances[t] = Activator.CreateInstance(t);
		}

		public T Get<T>()
		{
			CreateInner(typeof(T));
			return (T)_instances[typeof(T)];
		}
	}
}
