using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace SourceGrid
{
	/// <summary>
	/// Abstract base class for manage columns informations.
	/// </summary>
	public abstract class ColumnsBase
	{
		private GridVirtual mGrid;
		
		public ColumnsBase(GridVirtual grid)
		{
			mGrid = grid;
		}
		
		public GridVirtual Grid
		{
			get{return mGrid;}
		}
		
		#region Abstract methods
		public abstract int Count
		{
			get;
		}
		
		/// <summary>
		/// Gets the width of the specified column.
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public abstract int GetWidth(int column);
		/// <summary>
		/// Sets the width of the specified column.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="width"></param>
		public abstract void SetWidth(int column, int width);
		
		public abstract AutoSizeMode GetAutoSizeMode(int column);
		#endregion
		
		/// <summary>
		/// Autosize column using default auto size mode
		/// </summary>
		/// <param name="column"></param>
		public void AutoSizeColumn(int column)
		{
			AutoSizeColumn(column, true);
		}
		
		/// <summary>
		/// Autosize column using default auto size mode
		/// </summary>
		public void AutoSizeColumn(int column, bool useRowHeight)
		{
			int minRow = 0;
			int maxRow = Grid.Rows.Count - 1;
			
			if ((GetAutoSizeMode(column) & AutoSizeMode.EnableAutoSizeView) == AutoSizeMode.EnableAutoSizeView)
			{
				bool isColumnVisible = this.Grid.GetVisibleColumns(true).Contains(column);
				if (isColumnVisible == false)
					return;
				List<int> visibleRows = Grid.GetVisibleRows(true);
				visibleRows.Sort();
				if (visibleRows.Count == 0)
					return;
				minRow = visibleRows[0];
				maxRow = visibleRows[visibleRows.Count - 1];
			}
			AutoSizeColumn(column, useRowHeight, minRow, maxRow);
		}
		public void AutoSizeColumn(int column, bool useRowHeight, int StartRow, int EndRow)
		{
			if ((GetAutoSizeMode(column) & AutoSizeMode.EnableAutoSize) == AutoSizeMode.EnableAutoSize &&
			    IsColumnVisible(column) )
				SetWidth(column, MeasureColumnWidth(column, useRowHeight, StartRow, EndRow) );
		}
		/// <summary>
		/// Measures the current column when drawn with the specified cells.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="useRowHeight">True to fix the row height when measure the column width.</param>
		/// <param name="StartRow">Start row to measure</param>
		/// <param name="EndRow">End row to measure</param>
		/// <returns>Returns the required width</returns>
		public int MeasureColumnWidth(int column, bool useRowHeight, int StartRow, int EndRow)
		{
			int min = Grid.MinimumWidth;
			
			if ((GetAutoSizeMode(column) & AutoSizeMode.MinimumSize) == AutoSizeMode.MinimumSize)
				return min;
			
			for (int r = StartRow; r <= EndRow; r++)
			{
				Cells.ICellVirtual cell = Grid.GetCell(r, column);
				if (cell != null)
				{
					Position cellPosition = new Position(r, column);
					
					Size maxLayout = Size.Empty;
					//Use the width of the actual cell (considering spanned cells)
					if (useRowHeight)
						maxLayout.Height = Grid.RangeToSize(Grid.PositionToCellRange(cellPosition)).Height;
					
					CellContext cellContext = new CellContext(Grid, cellPosition, cell);
					Size cellSize = cellContext.Measure(maxLayout);
					if (cellSize.Width > min)
						min = cellSize.Width;
				}
			}
			return min;
		}
		
		public void AutoSize(bool useRowHeight)
		{
			SuspendLayout();
			for (int i = 0; i < Count; i++)
			{
				AutoSizeColumn(i, useRowHeight);
			}
			ResumeLayout();
		}
		
		/// <summary>
		/// Auto size all the columns with the max required width of all cells.
		/// </summary>
		/// <param name="useRowHeight">True to fix the row height when measure the column width.</param>
		/// <param name="StartRow">Start row to measure</param>
		/// <param name="EndRow">End row to measure</param>
		public void AutoSize(bool useRowHeight, int StartRow, int EndRow)
		{
			SuspendLayout();
			for (int i = 0; i < Count; i++)
			{
				AutoSizeColumn(i, useRowHeight, StartRow, EndRow);
			}
			ResumeLayout();
		}
		
		/// <summary>
		/// stretch the columns width to always fit the available space when the contents of the cell is smaller.
		/// </summary>
		public virtual void StretchToFit()
		{
			SuspendLayout();
			
			Rectangle displayRect = Grid.DisplayRectangle;
			
			if (Count > 0 && displayRect.Width > 0)
			{
				List<int> visibleIndex = ColumnsInsideRegion(displayRect.X, displayRect.Width);
				
				//Continue only if the columns are all visible, otherwise this method cannot shirnk the columns
				if (visibleIndex.Count >= Count)
				{
					int? current = GetRight(Count - 1);
					if (current != null && displayRect.Width > current.Value)
					{
						//Calculate the columns to stretch
						int countToStretch = 0;
						for (int i = 0; i < Count; i++)
						{
							if ((GetAutoSizeMode(i) & AutoSizeMode.EnableStretch) == AutoSizeMode.EnableStretch &&
							    IsColumnVisible(i) )
								countToStretch++;
						}
						
						if (countToStretch > 0)
						{
							int deltaPerColumn = (displayRect.Width - current.Value) / countToStretch;
							for (int i = 0; i < Count; i++)
							{
								if ((GetAutoSizeMode(i) & AutoSizeMode.EnableStretch) == AutoSizeMode.EnableStretch &&
								    IsColumnVisible(i) )
									SetWidth(i, GetWidth(i) + deltaPerColumn);
							}
						}
					}
				}
			}
			
			ResumeLayout();
		}
		
		/// <summary>
		/// Gets the columns index inside the specified display area.
		/// </summary>
		/// <returns></returns>
		public List<int> ColumnsInsideRegion(int x, int width)
		{
			return ColumnsInsideRegion(x, width, true, true);
		}
		
		/// <summary>
		/// Gets the columns index inside the specified display area.
		/// The list returned is ordered by the index.
		/// Note that this method returns also invisible rows.
		/// </summary>
		/// <param name="returnsPartial">True to returns also partial columns</param>
		/// <param name="x"></param>
		/// <param name="width"></param>
		/// <param name="returnsFixedColumns"></param>
		/// <returns></returns>
		public List<int> ColumnsInsideRegion(int x, int width, bool returnsPartial, bool returnsFixedColumns)
		{
			int right = x + width;
			
			List<int> list = new List<int>();
			
			//Add the fixed columns
			// Loop until the currentHeight is smaller then the requested displayRect
			for (int fr = 0; fr < Grid.FixedColumns && fr < Count; fr++)
			{
				int leftDisplay = GetLeft(fr);
				int rightDisplay = leftDisplay + GetWidth(fr);
				
				//If the column is inside the view
				if (right >= leftDisplay && x <= rightDisplay &&
				    (returnsPartial || (rightDisplay <= right && leftDisplay >= x)))
				{
					if (returnsFixedColumns)
						list.Add(fr);
				}
				
				if (rightDisplay > right)
					break;
			}
			
			int? relativeCol = FirstVisibleScrollableColumn;
			
			if (relativeCol != null)
			{
				//Add the standard columns
				for (int r = relativeCol.Value; r < Count; r++)
				{
					int leftDisplay = GetLeft(r);
					int rightDisplay = leftDisplay + GetWidth(r);
					
					//If the column is inside the view
					if (right >= leftDisplay && x <= rightDisplay &&
					    (returnsPartial || (rightDisplay <= right && leftDisplay >= x)) )
					{
						list.Add(r);
					}
					
					if (rightDisplay > right)
						break;
				}
			}
			
			return list;
		}
		
		/// <summary>
		/// Calculate the Column that have the Left value smaller or equal than the point p_X, or -1 if not found found.
		/// </summary>
		/// <param name="x">X Coordinate to search for a column</param>
		/// <returns></returns>
		public int? ColumnAtPoint(int x)
		{
			List<int> list = ColumnsInsideRegion(x, 1);
			if (list.Count == 0)
				return null;
			else
				return list[0];
		}
		
		/// <summary>
		/// Returns the first visible scrollable column.
		/// Return null if there isn't a visible column.
		/// </summary>
		/// <returns></returns>
		public int? FirstVisibleScrollableColumn
		{
			get
			{
				int firstVisible = Grid.CustomScrollPosition.X + Grid.FixedColumns;
				
				if (firstVisible >= Count)
					return null;
				else
					return firstVisible;
			}
		}
		
		/// <summary>
		/// Returns the last visible scrollable column.
		/// Return null if there isn't a visible column.
		/// </summary>
		/// <returns></returns>
		public int? LastVisibleScrollableColumn
		{
			get
			{
				int? first = FirstVisibleScrollableColumn;
				if (first == null)
					return null;
				
				Rectangle scrollableArea = Grid.GetScrollableArea();
				
				int right = GetLeft(first.Value);
				int c = first.Value;
				for (; c < Count; c++)
				{
					right += GetWidth(c);
					
					if (right >= scrollableArea.Right)
						return c;
				}
				
				return c - 1;
			}
		}
		
		public Range GetRange(int column)
		{
			return new Range(0, column, Grid.Rows.Count-1, column);
		}
		
		#region Layout
		private int mSuspendedCount = 0;
		public void SuspendLayout()
		{
			mSuspendedCount++;
		}
		public void ResumeLayout()
		{
			if (mSuspendedCount > 0)
				mSuspendedCount--;
			
			PerformLayout();
		}
		public void PerformLayout()
		{
			if (mSuspendedCount == 0)
				OnLayout();
		}
		protected virtual void OnLayout()
		{
			Grid.OnCellsAreaChanged();
		}
		#endregion
		
		/// <summary>
		/// Fired when the numbes of columns changed.
		/// </summary>
		public void ColumnsChanged()
		{
			PerformLayout();
		}
		
		#region Left/Right
		public int GetAbsoluteLeft(int column)
		{
			if (column < 0)
				throw new ArgumentException("Must be a valid index");
			
			int left = 0;
			
			int index = 0;
			while (index < column)
			{
				left += GetWidth(index);
				
				index++;
			}
			
			return left;
		}
		public int GetAbsoluteRight(int column)
		{
			int left = GetAbsoluteLeft(column);
			return left + GetWidth(column);
		}
		
		/// <summary>
		/// Gets the column left position.
		/// The Left is relative to the specified start position.
		/// Calculate the left using also the FixedColumn if present.
		/// </summary>
		public int GetLeft(int column)
		{
			int actualFixedColumns = Math.Min(Grid.FixedColumns, Count);
			
			int left = 0;
			
			//Calculate fixed left cells
			for (int i = 0; i < actualFixedColumns; i++)
			{
				if (i == column)
					return left;
				
				left += GetWidth(i);
			}
			
			int? relativeColumn = FirstVisibleScrollableColumn;
			if (relativeColumn == null)
				relativeColumn = Count;
			
			if (relativeColumn == column)
				return left;
			else if (relativeColumn < column)
			{
				for (int i = relativeColumn.Value; i < Count; i++)
				{
					if (i == column)
						return left;
					
					left += GetWidth(i);
				}
			}
			else if (relativeColumn > column)
			{
				for (int i = relativeColumn.Value - 1; i >= 0; i--)
				{
					left -= GetWidth(i);
					
					if (i == column)
						return left;
				}
			}
			
			throw new IndexOutOfRangeException();
		}
		
		/// <summary>
		/// Gets the column right position. GetLeft + GetWidth.
		/// </summary>
		public int GetRight(int column)
		{
			int left = GetLeft(column);
			return left + GetWidth(column);
		}
		#endregion
		
		/// <summary>
		/// Show a column (set the width to default width)
		/// </summary>
		/// <param name="column"></param>
		public abstract void ShowColumn(int column);

		/// <summary>
		/// Hide the specified column (set the width to 0)
		/// </summary>
		/// <param name="column"></param>
		public abstract void HideColumn(int column);

		/// <summary>
		/// Returns true if the specified column is visible
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public abstract bool IsColumnVisible(int column);
	}
}
