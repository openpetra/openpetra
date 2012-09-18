/* */

using System;
using NUnit.Framework;
using SourceGrid;
using SourceGrid.Cells;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGird_ColumnSpan
	{
		[Test, ExpectedExceptionAttribute(typeof(OverlappingCellException))]
		public void ModifyColumnSpan_ToCauseOverlapping()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 6);
			
			// this cell should span both to the right, and to the left
			grid1[0, 4] = new SourceGrid.Cells.Cell("cell to span", typeof(string));
			grid1[0, 5] = new SourceGrid.Cells.Cell("cell to overlap", typeof(string));
			
			// this should throw exception
			grid1[0, 4].ColumnSpan = 2;
		}
		
		[Test, ExpectedExceptionAttribute(typeof(OverlappingCellException))]
		public void ModifyRowSpan_ToCauseOverlapping()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(6, 2);
			
			// this cell should span both to the right, and to the left
			grid1[4, 0] = new SourceGrid.Cells.Cell("cell to span", typeof(string));
			grid1[5, 0] = new SourceGrid.Cells.Cell("cell to overlap", typeof(string));
			
			// this should throw exception
			grid1[4, 0].RowSpan = 2;
		}
		
		[Test, ExpectedExceptionAttribute(typeof(OverlappingCellException))]
		public void InsertOverlappingCell()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 2);
			
			// this cell should span both to the right, and to the left
			grid1[0, 0] = new SourceGrid.Cells.Cell("Text Span", typeof(string));;
			grid1[0, 0].ColumnSpan = 2;
			
			grid1[0, 1] = new SourceGrid.Cells.Cell("This should throw OverlappingCellException");
		}
		
		[Test]
		public void GetCell_NonExistent()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 2);
			
			Assert.AreEqual(null, grid1.GetCell(0, 0));
		}
		
		[Test]
		public void GetCell_OutOfBounds()
		{
			Grid grid = new Grid();
			grid.Redim(5, 5);
			
			Assert.AreEqual(null, grid.GetCell(new Position(10, 10)));
		}
		
		[Test]
		public void GetCell()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 2);
			
			Cell visibleCell = new SourceGrid.Cells.Cell("Text Span", typeof(string));
			
			// this cell should span both to the right, and to the left
			grid1[0, 0] = visibleCell;
			grid1[0, 0].RowSpan = 2;
			grid1[0, 0].ColumnSpan = 2;
			
			
			Assert.AreEqual(visibleCell, grid1.GetCell(0, 0));
			Assert.AreEqual(visibleCell, grid1.GetCell(0, 1));
			Assert.AreEqual(visibleCell, grid1.GetCell(1, 0));
			Assert.AreEqual(visibleCell, grid1.GetCell(1, 1));
		}
		
		[Test]
		public void InsertNullCell()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 2);
			
			// this should not throw exception
			grid1[0, 0] = null;
		}
		
		[Test]
		public void AreSpannedCells_CorrectlyRemoved()
		{
			SourceGrid.Grid grid1 = new Grid();
			grid1.Redim(2, 2);
			
			grid1[0, 0] = new Cell();
			grid1[0, 0].ColumnSpan = 2;
			grid1[0, 0] = null;
			
			// this should be true. But is not, at the moment of writing
			Assert.AreEqual(null, grid1.GetCell(0, 1));
			// this should not throw exception
			grid1[0, 1] = new Cell("my new cell");
		}
	}
}
