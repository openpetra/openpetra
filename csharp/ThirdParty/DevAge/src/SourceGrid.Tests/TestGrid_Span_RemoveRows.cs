/* */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGrid_Span_RemoveColumns
	{
		[Test]
		public void ForceSpannedColumnsToMoveUp()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell
			grid1[0, 1] = new SourceGrid.Cells.Cell();
			grid1[0, 1].ColumnSpan= 2;
			
			// add single cell
			grid1[0, 4] = new SourceGrid.Cells.Cell();
			grid1[0, 4].ColumnSpan = 2;
	
			// shorten the row span
			grid1.Columns.Remove(3);
			
			// spanned cell must be reduced
			Assert.AreEqual(2, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			var ranges = grid1.SpannedCellReferences.SpannedRangesCollection.ToArray();
			Assert.AreEqual(new Range(0, 1, 0, 2), ranges[1]);
			Assert.AreEqual(new Range(0, 3, 0, 4), ranges[0]);
			
		}
	}
	
	[TestFixture]
	public class TestGrid_Span_RemoveRows
	{
		[Test]
		public void RedimShouldRemove_SpannedCellReferences()
		{
			Grid grid1 = new Grid();
			grid1.Redim(2, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].ColumnSpan = 2;
	
			// remove row at index 1
			grid1.Redim(1, 5);
			
			// increase grid
			grid1.Redim(2, 5);
	
			// add again cell at row 1
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].ColumnSpan = 2;
		}
		
		
		[Test]
		public void RemoveRowsShouldRmoveCellReferences()
		{
			// when we delete row, sppanned cells on that row should be also deleted
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].ColumnSpan = 2;
	
			// remove row
			grid1.Rows.Remove(1);
			
			// add again cell at row 1
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			// this will throw exception
			grid1[1, 0].ColumnSpan = 2;
		}
		
		[Test]
		public void RemoveRowsAllow_CellSpanningShouldBeShortened()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
	
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 2, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
			// shorten the row span
			grid1.Rows.Remove(2);
			
			// spanned cell must be reduced
			Assert.AreEqual(0, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			
		}
		
		[Test]
		public void RemoveRows_1()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 4;
	
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 4, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
			// shorten the row span
			grid1.Rows.Remove(2);
			
			// spanned cell must be reduced
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 3, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
			
		}
		
		
		[Test]
		public void ForceSpannedRowsToMoveUp()
		{
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			// add single cell
			grid1[4, 0] = new SourceGrid.Cells.Cell();
			grid1[4, 0].RowSpan = 2;
	
			// shorten the row span
			grid1.Rows.Remove(3);
			
			// spanned cell must be reduced
			Assert.AreEqual(2, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			var ranges = grid1.SpannedCellReferences.SpannedRangesCollection.ToArray();
			Assert.AreEqual(new Range(1, 0, 2, 0), ranges[1]);
			Assert.AreEqual(new Range(3, 0, 4, 0), ranges[0]);
			
		}
		
		[Test]
		public void RemoveSpannedCell_ShouldRemove_SpannedCellReference()
		{
			
			// when we delete row, if the given row
			// is in the range of some cell spann,
			// then throw an exception
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 3;
	
			// shorten the row span
			grid1.Rows.Remove(1);
			
			// spanned cell must be reduced
			Assert.AreEqual(0, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			
		}
	}
}
