/* */

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestDataGrid_Selection
	{
		/// <summary>
		/// This test fails at the moment, 
		/// selection code should be upgraded
		/// </summary>
		[Test, IgnoreAttribute("Not yet implemented")]
		public void TrySelectHeaderRow()
		{
			// HeaderRow should not be selectable
			// databind a list with one item
			
			DataGrid grid1 = new DataGrid();
			List<int> list = new List<int>();
			list.Add(1);
			grid1.DataSource = new DevAge.ComponentModel.BoundList<int>(list);
			grid1.DataSource.AllowNew  = false;
			
			// assert no rows are selected
			Assert.AreEqual(false, grid1.Selection.IsSelectedRow(0));
			Assert.AreEqual(false, grid1.Selection.IsSelectedRow(0));
			
			// select two rows, including header
			grid1.Selection.SelectRange(new Range(0, 0, 1, 1), true);
			
			// assert header row is not selected
			Assert.AreEqual(false, grid1.Selection.IsSelectedRow(0));
			// assert first row is selected
			Assert.AreEqual(true, grid1.Selection.IsSelectedRow(0));
		}
	}
	
	[TestFixture]
	public class TestDataGrid_PositionToCellRange
	{
		[Test]
		public void PositionToCellRange_OutOfBounds()
		{
			DataGrid grid1 = new DataGrid();
			List<int> list = new List<int>();
			grid1.DataSource = new DevAge.ComponentModel.BoundList<int>(list);
			Assert.AreEqual(Range.Empty, grid1.PositionToCellRange(new Position(5, 5)));
		}
	}
}
