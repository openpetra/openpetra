using System;
using Castle.Windsor;
using WindowsFormsSample.GridSamples.PingGrids;

namespace WindowsFormsSample
{
	public class ServiceIntialization
	{
		private static WindsorContainer m_container = null;
		
		private static void AddService<T>()
		{
			m_container.AddComponent<T>();
		}
		
		public static void Iinit(WindsorContainer container)
		{
			m_container = container;
			ServiceFactory.Container = container;
			
			
			AddService<FirebirdEmbeddedPreparer>();
			AddService<EmptyFirebirdDatabasePreparer>();
			AddService<SessionFactoryManager>();
			AddService<SessionManager>();
			AddService<TransactionManager>();
		}
	}
}
