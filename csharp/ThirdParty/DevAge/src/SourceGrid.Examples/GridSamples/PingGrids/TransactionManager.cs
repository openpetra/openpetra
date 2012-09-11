
using System;
using NHibernate;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class TransactionManager
	{
		public SessionManager SessionManager{get;set;}
		
		public ITransaction BeginTransaction()
		{
			var sess = SessionManager.GetSession();
			
			if (sess.Transaction.IsActive == false)
				return sess.BeginTransaction();
			return sess.Transaction;
		}
	}
}
