using SourceGrid.Utils;
using System;
using NUnit.Framework;

namespace SourceGrid.Tests.Performance
{
	[TestFixture]
	public class TestRangeRegion_Performance
	{
		[Test]
		public void GetRowsIndex_Performance()
		{
			// this test, sadly, tests nothing
			// remove it
			RangeRegion region = new RangeRegion(new Range(0, 0, 10000, 5000));
			
			using (IPerformanceCounter counter = new PerformanceCounter())
			{
				region.GetRowsIndex();
				Console.WriteLine(string.Format(
					"Test executed in {0} ms", counter.GetMilisec()));
			}
		}
	}
}
