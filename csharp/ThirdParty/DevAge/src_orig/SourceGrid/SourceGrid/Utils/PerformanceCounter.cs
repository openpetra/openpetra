using System;

namespace SourceGrid.Utils
{
	public class PerformanceCounter : IDisposable, IPerformanceCounter
	{
		private DateTime m_start = DateTime.MinValue;
		
		public PerformanceCounter()
		{
			this.m_start = DateTime.Now;
		}
		
		public double GetSeconds()
		{
			TimeSpan span = DateTime.Now - m_start;
			return span.TotalSeconds;
		}
		
		public double GetMilisec()
		{
			TimeSpan span = DateTime.Now - m_start;
			return span.TotalMilliseconds;
		}
		
		public void Dispose()
		{
		}
	}
}
