using SourceGrid.Utils;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuadTreeLib;
using SourceGrid.Tests.Selection;

namespace SourceGrid.Tests.Performance
{
	public class TestRowSelection_Performance
	{
		[Test]
		public void GetSelectionRegion_GetRows_Index_Performance()
		{
			// select range:
			// * from row 1 to 2000
			// Print out how long in seconds that function takes to execute
			
			// The tests take seconds to execute
			// 6 seconds, 2008-12-11, AMD Athlon 64 X2 dual machine
			// 0 seconds, 2008-12-11, AMD Athlon 64 X2 dual machine
			
			Grid grid = new TestRowSelectionHelper().CreateGridWithRows(2000);

			grid.Selection.SelectRange(new Range(0, 0, 1999, 1), true);
			
			using (IPerformanceCounter counter = new PerformanceCounter())
			{
				grid.Selection.GetSelectionRegion().GetRowsIndex();
				
				Console.WriteLine(string.Format(
					"Test executed in {0} seconds", counter.GetSeconds()));
			}
		}
	}
}
