
using System;
using NHibernate;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class SessionManager
	{
		public SessionFactoryManager SessionFactoryManager{get;set;}
		
		private ISession m_sesion = null;
		
		public ISession GetSession()
		{
			if (m_sesion == null)
				m_sesion = SessionFactoryManager.SessionFactory.OpenSession();
			if (m_sesion.IsOpen == false)
				m_sesion = SessionFactoryManager.SessionFactory.OpenSession();
			return m_sesion;
		}
	}
}
