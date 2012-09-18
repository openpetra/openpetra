using System;
using Castle.Windsor;

namespace WindowsFormsSample
{
	public class ServiceFactory
	{
		private static WindsorContainer m_container = null;
		
		public static WindsorContainer Container {
			get { return m_container; }
			set { m_container = value; }
		}
		
		/// <summary>
		/// Auto assign services for specific instance. 
		/// Searches public properties, and if required type is not in the namespace "System",
		/// tries to get it from windsor
		/// </summary>
		/// <param name="instance"></param>
		public static void Init(object instance)
		{
			var type = instance.GetType();
			foreach (var pi in type.GetProperties())
			{
				if (pi.CanRead == false)
					continue;
				if (pi.CanWrite == false)
					continue;
				var serviceType = pi.PropertyType;
				if (serviceType.FullName.ToLower().StartsWith("system"))
					continue;
				
				var service = GetService(serviceType);
				pi.SetValue(instance, service, null);
			}
		}
		
		public static object GetService(Type serviceType)
		{
			return m_container.Resolve(serviceType);
		}
		
		public static T GetService<T>()
		{
			return m_container.Resolve<T>();
		}
		
	}
}
