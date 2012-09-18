using System;
using System.ComponentModel;

using SourceGrid.Examples.Threading;
using SourceGrid.Extensions.PingGrids;
using SourceGrid.PingGrid.Backends.DSet;
using WindowsFormsSample.GridSamples.PingGrids;

namespace WindowsFormsSample
{
	public class InsertRowsOperation : AsynchroniousOperation, IProgressCommand
	{
		public event OnUpdateHandler Progress;
		private int from = 0;
		private int to = 0;
		public SessionManager SessionManager{get;set;}
		public TransactionManager TransactionManager{get;set;}
		private Random rand = new Random();
		
		protected override void DoWork()
		{
			Update();
			// When a cancel occurs, the recursive DoSearch drops back
			// here asap, so we'd better acknowledge cancellation.
			if (CancelRequested)
			{
				AcknowledgeCancel();
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="updateItemsList">This must be list containing
		/// only IUpdateItem instances</param>
		/// <param name="target">a target upon which to syncrhonize this command,
		/// usually this is a Fform</param>
		/// <param name="systemFunctionRepository">a repository containing
		/// system functions to execute</param>
		/// <param name="updateItemGenericType">a concrete type of UpdateItem of T </param>
		/// <param name="undoState">commit to databse, or join with undo system?</param>
		public InsertRowsOperation(
			ISynchronizeInvoke target,int from, int to)
			:base(target)
		{
			this.from = from;
			this.to = to;
			
			ServiceFactory.Init(this);
		}
		
		private void OnProgress(IUpdateStatus status)
		{
			lock(this)
			{
				FireAsync(this.Progress, this, status);
			}
		}
		
		
		public void Update()
		{
			
			using (var session = SessionManager.GetSession())
			{
				using (var transaction = TransactionManager.BeginTransaction())
				{
					for (var i = from; i < to; i++)
					{
						var deleted = rand.Next(0, 2) == 0? false: true;
						// create a couple of Stores each with some Products and Employees
						var track = new Track{ Name = "SuperMart" + i, Composer = "random composer"  +i,
						IsDeleted = deleted};
						track.DateCreated = DateTime.Now;
						track.Price = (decimal)(i * Math.PI);
						
						
						session.Save(track);
						if (i % 100 == 0)
						{
							OnProgress(new UpdateStatus(i - from, to - from));
							session.Flush();
							session.Clear();
						}
					}
					transaction.Commit();
	
				}
			}
		}
	}
}
