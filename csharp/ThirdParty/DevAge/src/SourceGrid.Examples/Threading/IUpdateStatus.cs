
using System;

namespace SourceGrid.Examples.Threading
{
	public class UpdateStatus : IUpdateStatus
	{
		private int m_maxValue = 100;
		private int m_current = 0;
		
		public UpdateStatus()
		{
			
		}
		
		public UpdateStatus(int current, int maxValue)
		{
			this.m_maxValue = maxValue;
			this.m_current = current;
		}
		
		public int MaxValue {
			get { return m_maxValue; }
			set { m_maxValue = value; }
		}
		
		public int Current {
			get { return m_current; }
			set { m_current = value; }
		}
	}
	
	public interface IUpdateStatus
	{
		int MaxValue {get;}
		int Current {get;}
	}
}
