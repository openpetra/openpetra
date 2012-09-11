using System;
using System.Collections.Generic;
using SourceGrid.Selection;

namespace SourceGrid
{
	public class StandardHiddenRowCoordinator : IHiddenRowCoordinator
	{
		protected RowsBase m_rows = null;
		protected int m_totalHiddenRows = 0;
		/// <summary>
		/// This will help us track which rows are hidden, and which are not
		/// </summary>
		protected RangeMergerByRows m_rowMerger = new RangeMergerByRows();
		
		public RowsBase Rows {
			get { return m_rows; }
		}
		
		public int GetTotalHiddenRows()
		{
			return m_totalHiddenRows;
		}
		
		public StandardHiddenRowCoordinator(RowsBase rows)
		{
			this.m_rows = rows;
			
			rows.RowVisibilityChanged += delegate(int rowIndex, bool becameVisible)
			{
				var range = new Range(rowIndex, 0, rowIndex, 1);
				if (becameVisible)
					m_rowMerger.RemoveRange(range);
				else
					m_rowMerger.AddRange(range);
			};
			
			rows.RowVisibilityChanged += delegate(int rowIndex, bool becameVisible)
			{
				if (becameVisible == true)
					m_totalHiddenRows -= 1;
				else
					m_totalHiddenRows += 1;
				if (m_totalHiddenRows < 0)
					throw new SourceGridException("Total hidden rows becamse less than 0. This indicates a bug");
			};
		}
		
		private int? GetNextVisibleRow(int startFrom)
		{
			var i = startFrom + 1;
			while (i < Rows.Count)
			{
				if (Rows.IsRowVisible(i) == true)
					return i;
				i++;
			}
			return null;
		}
		
		public int ConvertScrollbarValueToRowIndex(int scrollBarValue)
		{
			//return scrollBarValue;
			var processedAllRows = 0;
			var processedVisibleRows = 0;
			foreach (var range in m_rowMerger.LoopAllRanges())
			{
				var deltaAllRows = range.End.Row - processedAllRows + 1;
				var deltaVisibleRows = range.Start.Row - processedAllRows;
				
				// break if we have enough
				if (processedVisibleRows > scrollBarValue)
					break;
				
				// cap current range, if we are near our needed range
				if (processedVisibleRows + deltaVisibleRows > scrollBarValue)
				{
					deltaAllRows = deltaVisibleRows - (processedVisibleRows + deltaVisibleRows - scrollBarValue);
				}
				
				processedAllRows += deltaAllRows;
				processedVisibleRows += deltaVisibleRows;
			}
			if (processedVisibleRows < scrollBarValue)
			{
				processedAllRows += scrollBarValue - processedVisibleRows;
			}
			return processedAllRows;
		}
		
		/// <summary>
		/// Returns a sequence of row indexes which are visible only.
		/// Correctly handles invisible rows.
		/// </summary>
		/// <param name="scrollBarValue">The value of the vertical scroll bar. Note that
		/// this does not directly relate to row number. If there are no hidden rows at all,
		/// then scroll bar value directly relates to row index number. However,
		/// if some rows are hidden, then this value is different.
		/// Basically, it says how many visible rows must be scrolled down</param>
		/// <param name="numberOfRowsToProduce">How many visible rows to return</param>
		/// <returns>Can return less rows than requested. This might occur
		/// if you request to return visible rows in the end of the grid,
		/// and all the rows would be hidden. In that case no indexes would be returned
		/// at all, even though specific amount of rows was requested</returns>
		public IEnumerable<int> LoopVisibleRows(int scrollBarValue, int numberOfRowsToProduce)
		{
			var producedRows = 0;
			var currentRow = scrollBarValue;
			if (Rows.IsRowVisible(scrollBarValue) == false)
			{
				var next = GetNextVisibleRow(currentRow);
				if (next == null)
					yield break;
				currentRow = next.Value;
			}
			
			
			// produce speicifc number of visible row indexes
			while (producedRows <= numberOfRowsToProduce)
			{
				yield return currentRow;
				producedRows++;
				
				var nextRow = GetNextVisibleRow(currentRow);
				if (nextRow == null)
					break;
				currentRow = nextRow.Value;
				
			}
			
		}
		
	}
}
