/*
 * Created by SharpDevelop.
 * User: darius.damalakas
 * Date: 2008-12-10
 * Time: 17:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using SourceGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using SourceGrid.Selection;

namespace SourceGrid.Tests.Selection
{
	[TestFixture]
	public class TestFreeSelection
	{
		[Test]
		public void TestTabMovement_DoesLoopCorrectly_ThroughSpannedRowCells()
		{
			using (var form = new Form())
			{
				
				Grid grid1 = new Grid();
				grid1.Redim(40,3);
				grid1.FixedColumns = 1;
				grid1.FixedRows = 1;

				Random rnd = new Random();
				for (int r = 0; r < grid1.RowsCount/2; r++)
				{
					for (int c = 0; c < grid1.ColumnsCount; c++)
					{
						grid1[r * 2, c] = new SourceGrid.Cells.Cell(r*c);
						grid1[r * 2, c].RowSpan = 2;
					}
				}
				
				form.Controls.Add(grid1);
				form.Show();
				
				Assert.AreEqual(true, grid1.Selection.Focus(new Position(0, 0), true));
				Assert.AreEqual(new Position(0, 0), grid1.Selection.ActivePosition);
				
				Assert.AreEqual(true, grid1.Selection.Focus(new Position(1, 0), true));
				Assert.AreEqual(new Position(1, 0), grid1.Selection.ActivePosition);
				
				form.Close();
			}
		}
		
		[Test]
		public void SelectSpannedCells_SingleCell()
		{
			// just select a single cell. Should select whole spanned cell
			Grid grid1 = new Grid();
			grid1.Redim(6, 6);

			grid1[0, 0] = new SourceGrid.Cells.Cell();
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].RowSpan = 3;

			grid1[0, 3] = new SourceGrid.Cells.Cell();
			grid1[0, 3].ColumnSpan = 3;
			grid1[0, 3].RowSpan = 3;
			
			grid1.Selection.SelectCell(new Position(0, 0), true);
			RangeRegion region = grid1.Selection.GetSelectionRegion();
			Assert.AreEqual(1, region.Count);
			Assert.AreEqual(new Range(0, 0, 2, 2), region[0]);
		}
		
		[Test]
		public void SelectSpannedCells_SingleCell_DoubleSelection()
		{
			// select at two positions the same cell
			Grid grid1 = new Grid();
			grid1.Redim(6, 6);

			grid1[0, 0] = new SourceGrid.Cells.Cell();
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].RowSpan = 3;

			grid1[0, 3] = new SourceGrid.Cells.Cell();
			grid1[0, 3].ColumnSpan = 3;
			grid1[0, 3].RowSpan = 3;
			
			grid1.Selection.SelectRange(new Range(0, 0, 0, 1), true);
			
			RangeRegion region = grid1.Selection.GetSelectionRegion();
			Assert.AreEqual(1, region.Count);
			Assert.AreEqual(new Range(0, 0, 2, 2), region[0]);
		}
		
		
		[Test]
		public void SelectSpannedCells_TwoSpannedCells()
		{
			// select at two positions the same cell
			Grid grid1 = new Grid();
			grid1.Redim(6, 6);

			grid1[0, 0] = new SourceGrid.Cells.Cell();
			grid1[0, 0].ColumnSpan = 3;
			grid1[0, 0].RowSpan = 3;

			grid1[0, 3] = new SourceGrid.Cells.Cell();
			grid1[0, 3].ColumnSpan = 3;
			grid1[0, 3].RowSpan = 3;
			
			grid1.Selection.SelectRange(new Range(0, 0, 0, 3), true);
			RangeRegion region = grid1.Selection.GetSelectionRegion();
			Assert.AreEqual(1, region.Count);
			Assert.AreEqual(new Range(0, 0, 2, 5), region[0]);
		}
	}

	

	
	
	[TestFixture]
	public class TestRangeMergerByRows
	{
		[Test]
		public void RemoveRange_ResizeFromBottom()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 2, 1))
				.RemoveRange(new Range(1, 1, 1, 1));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions(0, 1).Count);
			Assert.AreEqual(new Range(2, 0, 2, 1), list.GetSelectedRowRegions(0, 1)[0]);
		}
		
		[Test]
		public void RemoveRange_SplitIntoTwo()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 3, 1))
				.RemoveRange(new Range(2, 1, 2, 1));
			
			Assert.AreEqual(2, list.GetSelectedRowRegions(0, 1).Count);
			Assert.AreEqual(new Range(1, 0, 1, 1), list.GetSelectedRowRegions(0, 1)[0]);
			Assert.AreEqual(new Range(3, 0, 3, 1), list.GetSelectedRowRegions(0, 1)[1]);
		}
		
		[Test]
		public void RemoveRange_RemoveFromTopAndBottom()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 2, 1))
				.AddRange(new Range(4, 1, 5, 1))
				.RemoveRange(new Range(2, 1, 4, 1));
			
			List<Range> ranges = list.GetSelectedRowRegions(0, 1);
			Assert.AreEqual(2, ranges.Count);
			Assert.AreEqual(new Range(1, 0, 1, 1), ranges[0]);
			Assert.AreEqual(new Range(5, 0, 5, 1), ranges[1]);
		}
		
		[Test]
		public void AddRange_Merge_Adjanced()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 1, 1))
				.AddRange(new Range(2, 2, 2, 2));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions(0, 1).Count);
			Assert.AreEqual(new Range(1, 0, 2, 1), list.GetSelectedRowRegions(0, 1)[0]);
		}
		
		[Test]
		public void AddRange_Merge_Overlaping()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 5, 1))
				.AddRange(new Range(3, 1, 8, 1));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions(0, 1).Count);
			Assert.AreEqual(new Range(1, 0, 8, 1), list.GetSelectedRowRegions(0, 1)[0]);
		}
		
		[Test]
		public void AddRange_NotMerge()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 1, 1))
				.AddRange(new Range(3, 3, 3, 3));
			
			Assert.AreEqual(2, list.GetSelectedRowRegions(0, 1).Count);
		}
		
		[Test]
		public void AddRange_Merge_WhenOverlapped()
		{
			// single addition causes to range many rows at once
			
			// add separate ranges
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 1, 1))
				.AddRange(new Range(3, 3, 3, 3))
				.AddRange(new Range(5, 5, 5, 5));
			
			Assert.AreEqual(3, list.GetSelectedRowRegions(0, 1).Count);
			
			// cause them all to merge
			list.AddRange(new Range(1, 1, 5, 1));
			Assert.AreEqual(1, list.GetSelectedRowRegions(0, 1).Count);
			
		}
		
		[Test]
		public void AddRange_Merge_Intersecting()
		{
			RangeMergerByRows list = new RangeMergerByRows()
				.AddRange(new Range(1, 1, 1, 1))
				.AddRange(new Range(1, 1, 2, 2));
			
			Assert.AreEqual(1, list.GetSelectedRowRegions(0, 1).Count);
			Assert.AreEqual(new Range(1, 0, 2, 1), list.GetSelectedRowRegions(0, 1)[0]);
		}
		
		/*[Test]
		public void Merge()
		{
			RangeMergerByRows list = new RangeMergerByRows(0, 1);
			Assert.AreEqual(false, list.Merge(new Range(1, 1, 1, 1)));
			Assert.AreEqual(tru, list.Merge(new Range(1, 1, 1, 1)));
				
		}*/

	}
}