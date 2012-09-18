/* */

using System;
using NUnit.Framework;

namespace SourceGrid.Tests
{
	[TestFixture]
	public class TestGrid_Span_AddColumns
	{
		[Test]
		public void InsertCell_ForceSpannedColumnsToMoveRight()
		{
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[0, 1] = new SourceGrid.Cells.Cell();
			grid1[0, 1].ColumnSpan = 2;
			
			// increase grid
			grid1.Columns.Insert(0);
	
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(0, 2, 0, 3), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
		}
		
	}
	
	[TestFixture]
	public class TestGrid_Span_AddRows
	{
		[Test]
		public void InsertCell_ForceSpannedRowsToMoveDown()
		{
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			// increase grid
			grid1.Rows.Insert(0);
	
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(2, 0, 3, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
		}
		
		[Test]
		public void InsertRowAtSpannedCell_ShouldMoveSpannedRowDown()
		{
			var grid1 = new Grid();
			grid1.Redim(4, 12);
			grid1.FixedRows = 2;
			
			var cell = new SourceGrid.Cells.Cell();
			grid1[3, 0] = cell;
			grid1[3, 0].ColumnSpan = 2;
			grid1.Rows.Insert(3);
			
			Assert.AreEqual(null, grid1[3, 0]);
			Assert.AreEqual(null, grid1[3, 1]);
			Assert.AreEqual(cell, grid1[4, 0]);
			Assert.AreEqual(cell, grid1[4, 1]);
		}
		
		[Test]
		public void InsertCell_ForceSpannedRowsToExpand()
		{
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			// increase grid
			grid1.Rows.Insert(2);
	
			Assert.AreEqual(1, grid1.SpannedCellReferences.SpannedRangesCollection.Count);
			Assert.AreEqual(new Range(1, 0, 3, 0), grid1.SpannedCellReferences.SpannedRangesCollection.ToArray()[0]);
		}
		
		[Test]
		public void InsertCell_ForceSpannedCellToExpand()
		{
			Grid grid1 = new Grid();
			grid1.Redim(5, 5);
	
			// add single cell at row 1, with cell span 2
			grid1[1, 0] = new SourceGrid.Cells.Cell();
			grid1[1, 0].RowSpan = 2;
			
			// increase grid
			grid1.Rows.Insert(2);
	
			Assert.AreEqual(3, grid1[1, 0].RowSpan);
		}
		
	}
}
