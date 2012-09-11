using System;

namespace SourceGrid.Utils
{
	public interface IPerformanceCounter : IDisposable
	{
		double GetSeconds();
		double GetMilisec();
	}
}
