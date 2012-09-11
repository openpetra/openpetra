using System;
using System.Windows.Forms;
using NUnit.Framework;
using SourceGrid.Extensions.PingGrids;

namespace SourceGrid.Tests.Rows
{
	[TestFixture]
	public class TestStandardHiddenRowCoordinator
	{
		StandardHiddenRowCoordinator coord = null;
		GridRows rowsBase = null;
		Grid grid = null;
		
		private void CreateGrid()
		{
			grid = new Grid();
			grid.Redim(100, 10);
			grid.FixedRows = 0;
			rowsBase = grid.Rows;
			coord = new StandardHiddenRowCoordinator(grid.Rows);
		}
			
		[Test]
		public void NoInvisible()
		{
			CreateGrid();
			
			Assert.AreEqual(0, rowsBase.FirstVisibleScrollableRow);
			Assert.AreEqual(0, coord.ConvertScrollbarValueToRowIndex(0));
			Assert.AreEqual(1, coord.ConvertScrollbarValueToRowIndex(1));
		}
		
		[Test]
		public void Invisible_FirstRowInvisible()
		{
			CreateGrid();
			
			rowsBase.ShowRow(0, false);
			
			Assert.AreEqual(1, coord.ConvertScrollbarValueToRowIndex(0));
			Assert.AreEqual(2, coord.ConvertScrollbarValueToRowIndex(1));
			Assert.AreEqual(1, rowsBase.FirstVisibleScrollableRow);
		}
		
		[Test]
		public void Invisible_FirstRowVisible_SecondInvisible()
		{
			CreateGrid();
			
			rowsBase.ShowRow(1, false);
			
			Assert.AreEqual(0, coord.ConvertScrollbarValueToRowIndex(0));
			Assert.AreEqual(2, coord.ConvertScrollbarValueToRowIndex(1));
			Assert.AreEqual(3, coord.ConvertScrollbarValueToRowIndex(2));
			Assert.AreEqual(0, rowsBase.FirstVisibleScrollableRow);
		}
	}
}
