using QuadTreeLib;
using System;
using System.Collections.Generic;

namespace SourceGrid
{
	public class SpannedCellRangesController : ISpannedCellRangesController
	{
		Grid m_grid = null;
		
		public ISpannedRangesCollection SpannedRangesCollection {get; private set;}
		
		public Grid Grid {
			get { return m_grid; }
		}
		
		/// <summary>
		/// Adds or updates given range.
		/// Updates range only when existing range with given start position is found
		/// </summary>
		/// <param name="newRange"></param>
		public void UpdateOrAdd(Range newRange)
		{
			if (newRange.Equals(Range.Empty))
				throw new ArgumentException("Range can not be empty");
			Range? index = this.SpannedRangesCollection.FindRangeWithStart(newRange.Start);
			if (index == null)
				this.SpannedRangesCollection.Add(newRange);
			else
				this.SpannedRangesCollection.Update(Range.FromPosition(newRange.Start), newRange);
		}
		
		/// <summary>
		/// Updates range whose start position matches.
		/// If no matches found, an exception is thrown
		/// </summary>
		/// <param name="newRange"></param>
		public void Update(Range newRange)
		{
			Range? index = this.SpannedRangesCollection.FindRangeWithStart(newRange.Start);
			if (index == null)
				throw new ArgumentException(string.Format(
					"Could not find a spanned cell range with the same starting point as {0}",
					newRange.Start));
			this.SpannedRangesCollection.Update(index.Value, newRange);
		}
		
		
		public SpannedCellRangesController(Grid grid, ISpannedRangesCollection spannedRangeCollection)
			:this(grid)
		{
			SpannedRangesCollection = spannedRangeCollection;
		}
		
		public SpannedCellRangesController(Grid grid)
		{
			this.m_grid = grid;
			SpannedRangesCollection = new SpannedRangesList();
		}
		
		public void MoveLeftSpannedRanges(int startIndex, int moveCount)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if (range.Start.Column <= startIndex)
					continue;
				var newRange = new Range(range.Start.Row, range.Start.Column - moveCount,
				                         range.End.Row, range.End.Column - moveCount);
				this.SpannedRangesCollection.Update(range, newRange);
			}
		}
		
		public void MoveUpSpannedRanges(int startIndex, int moveCount)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if (range.Start.Row <= startIndex)
					continue;
				var newRange = new Range(range.Start.Row - moveCount, range.Start.Column,
				                         range.End.Row - moveCount, range.End.Column);
				this.SpannedRangesCollection.Update(range, newRange);
			}
		}
		
		public void Swap(int rowIndex1, int rowIndex2)
		{
			var wholeGrid = new Range(rowIndex1, 0, rowIndex1, int.MaxValue);
			var wholeGrid2 = new Range(rowIndex2, 0, rowIndex2, int.MaxValue);
			
			var firstRanes = this.SpannedRangesCollection.GetRanges(wholeGrid);
			var secondRanges = this.SpannedRangesCollection.GetRanges(wholeGrid2);
			
			foreach (var rangineInFirst in firstRanes)
			{
				if (rangineInFirst.RowsCount > 1)
					throw new SourceGridException("Can not swap rows if they contain spanned ranged which extend more than one row");
				var newRange = new Range(rowIndex2, rangineInFirst.Start.Column, rowIndex2, rangineInFirst.End.Column);
				this.SpannedRangesCollection.Update(rangineInFirst, newRange);
			}
			
			foreach (var rangesInSecond in secondRanges)
			{
				if (rangesInSecond.RowsCount > 1)
					throw new SourceGridException("Can not swap rows if they contain spanned ranged which extend more than one row");
				var newRange = new Range(rowIndex1, rangesInSecond.Start.Column, rowIndex1, rangesInSecond.End.Column);
				this.SpannedRangesCollection.Update(rangesInSecond, newRange);
			}
		}
		 
		
		public void MoveDownSpannedRanges(int startIndex, int moveCount)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if (range.Start.Row < startIndex)
					continue;
				var newRange = new Range(range.Start.Row + moveCount, range.Start.Column,
				                         range.End.Row + moveCount, range.End.Column);
				this.SpannedRangesCollection.Update(range, newRange);
			}
		}
		
		public void MoveRightSpannedRanges(int startIndex, int moveCount)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if (range.Start.Column < startIndex)
					continue;
				var newRange = new Range(range.Start.Row, range.Start.Column + moveCount,
				                         range.End.Row, range.End.Column + moveCount);
				this.SpannedRangesCollection.Update(range, newRange);
			}
		}
		
		
		public void RemoveSpannedCellReferencesInRows(int startIndex, int count)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if ((range.Start.Row >= startIndex) &&
				    (range.Start.Row < startIndex + count))
				{
					this.SpannedRangesCollection.Remove(range);
				}
			}
		}
		
		public void RemoveSpannedCellReferencesInColumns(int startIndex, int count)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				if ((range.Start.Column >= startIndex) &&
				    (range.Start.Column < startIndex + count))
				{
					this.SpannedRangesCollection.Remove(range);
				}
			}
		}
		
		public void ExpandSpannedColumns(int startIndex, int count)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				bool isInsideStart = (range.Start.Column >= startIndex) && (range.Start.Column <= startIndex + count - 1);
				bool isInsideEnd = (range.End.Column >= startIndex) && (range.End.Column <= startIndex + count - 1);
				bool isInside = isInsideStart || isInsideEnd;
				if (isInside)
				{
					var cell = m_grid[range.Start];
					cell.ColumnSpan += count;
				}
			}
		}
		
		public void ExpandSpannedRows(int startIndex, int count)
		{
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				bool isInsideStart = (range.Start.Row >= startIndex) && (range.Start.Row <= startIndex + count - 1);
				bool isInsideEnd = (range.End.Row >= startIndex) && (range.End.Row <= startIndex + count - 1);
				bool isInside = isInsideStart || isInsideEnd;
				if (isInside)
				{
					var cell = m_grid[range.Start];
					cell.RowSpan += count;
				}
			}
		}
		
		public  void ShrinkOrRemoveSpannedRows(int startIndex, int count)
		{
			const int startCol = 0;
			const int endCol = 1000;
			var removeRange = new Range(startIndex, startCol, startIndex + count - 1, endCol);
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				var intersection = range.Intersect(removeRange);
				if (intersection.IsEmpty() == false)
				{
					var cell = m_grid[range.Start];
					var span = cell.RowSpan - count;
					if (span <= 1 && cell.ColumnSpan <= 1)
					{
						cell.RowSpan = 1;
						this.SpannedRangesCollection.Remove(range);
					} else
						cell.RowSpan = span;
				}
			}
		}
		
		public  void ShrinkOrRemoveSpannedColumns(int startIndex, int count)
		{
			const int startRow = 0;
			const int endRow = 1000;
			var removeRange = new Range(startRow, startIndex, endRow, startIndex + count - 1);
			foreach (var range in this.SpannedRangesCollection.ToArray())
			{
				var intersection = range.Intersect(removeRange);
				if (intersection.IsEmpty() == false)
				{
					var cell = m_grid[range.Start];
					var span = cell.ColumnSpan - count;
					if (span <= 1 && cell.ColumnSpan <= 1)
					{
						cell.ColumnSpan = 1;
						this.SpannedRangesCollection.Remove(range);
					} else
						cell.ColumnSpan = span;
				}
			}
		}
	}
}
