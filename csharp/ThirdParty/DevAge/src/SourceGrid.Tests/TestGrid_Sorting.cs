/* */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGrid_Sorting
	{
		[Test]
		public void Bug_3424()
		{
			var grid1 = new Grid();
			grid1.ColumnsCount = 3;
			grid1.FixedRows = 1;
			grid1.Rows.Insert(0);
	
			SourceGrid.Cells.ColumnHeader header1 = new SourceGrid.Cells.ColumnHeader("String");
	
			// here you can se the other column to sort when the current column is equal
			header1.SortComparer = new SourceGrid.MultiColumnsComparer(1, 2);
	
			var sorter = new SourceGrid.Cells.ColumnHeader("CheckBox");
			grid1[0, 0] = header1;
			grid1[0, 1] = new SourceGrid.Cells.ColumnHeader("DateTime");
			grid1[0, 2] = sorter;
			for (int r = 1; r < 10; r++)
			{
				grid1.Rows.Insert(r);
				grid1[r, 0] = new SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
				grid1[r, 1] = new SourceGrid.Cells.Cell(DateTime.Today.AddDays(7 * r), typeof(DateTime));
				grid1[r, 2] = new SourceGrid.Cells.CheckBox("", true);
			}
			
			grid1[9, 2] = null;
			grid1[9, 1].ColumnSpan = 2;
			
			sorter.Sort(true);
		}
	}
}
