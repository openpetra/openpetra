using SourceGrid.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SourceGrid.Cells;

namespace SourceGrid
{
	/// <summary>
	/// A abstract Grid control to support large virtual data. You must override: GetCell, CreateRowsObject, CreateColumnsObject
	/// </summary>
	[System.ComponentModel.ToolboxItem(false)]
	public abstract class GridVirtual : CustomScrollControl
	{
		#region Constructor
		/// <summary>
		/// Grid constructor
		/// </summary>
		public GridVirtual()
		{
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ContainerControl, true);

			TabStop = true;
			EnableSort = true;

			m_Rows = CreateRowsObject();
			m_Columns = CreateColumnsObject();

			SelectionMode = GridSelectionMode.Cell;

			//Create the Controller list for the Cells (the order is important because is the same order that the events will be dispatched)
			Controller.AddController(Cells.Controllers.StandardBehavior.Default);
			Controller.AddController(Cells.Controllers.MouseSelection.Default);
			Controller.AddController(Cells.Controllers.CellEventDispatcher.Default);

			m_LinkedControls = new LinkedControlsList(this);

			//ToolTip
			toolTip = new System.Windows.Forms.ToolTip();
			ToolTipText = "";
		}

		/// <summary>
		/// Abstract method used to create the rows object.
		/// </summary>
		protected abstract RowsBase CreateRowsObject();

		/// <summary>
		/// Abstract method used to create the columns object.
		/// </summary>
		protected abstract ColumnsBase CreateColumnsObject();

		#endregion

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Default Cell Width/Height
		private int mDefaultHeight = 20;
		/// <summary>
		/// Indicates the default height of new row
		/// </summary>
		[DefaultValue(20)]
		public int DefaultHeight
		{
			get{return mDefaultHeight;}
			set{mDefaultHeight = value;}
		}

		private int mDefaultWidth = 50;
		/// <summary>
		/// Indicates the default width of new column
		/// </summary>
		[DefaultValue(50)]
		public int DefaultWidth
		{
			get{return mDefaultWidth;}
			set{mDefaultWidth = value;}
		}

		private int mMinimumHeight = 0;
		/// <summary>
		/// Indicates the minimum height of the rows
		/// </summary>
		[DefaultValue(0)]
		public int MinimumHeight
		{
			get{return mMinimumHeight;}
			set{mMinimumHeight = value;}
		}

		private int mMinimumWidth = 0;
		/// <summary>
		/// Indicates the minimum width of the columns
		/// </summary>
		[DefaultValue(0)]
		public int MinimumWidth
		{
			get{return mMinimumWidth;}
			set{mMinimumWidth = value;}
		}
		#endregion

		#region AutoSize
		/// <summary>
		/// Auto size the columns and the rows speified
		/// </summary>
		/// <param name="p_RangeToAutoSize"></param>
		public virtual void AutoSizeCells(Range p_RangeToAutoSize)
		{
			SuspendLayout();
			if (p_RangeToAutoSize.IsEmpty() == false)
			{
				Rows.SuspendLayout();
				Columns.SuspendLayout();
				try
				{
					for (int c = p_RangeToAutoSize.End.Column; c >= p_RangeToAutoSize.Start.Column ; c--)
						Columns.AutoSizeColumn(c,false, p_RangeToAutoSize.Start.Row, p_RangeToAutoSize.End.Row);
					for (int r = p_RangeToAutoSize.End.Row; r >= p_RangeToAutoSize.Start.Row ; r--)
						Rows.AutoSizeRow(r, false, p_RangeToAutoSize.Start.Column, p_RangeToAutoSize.End.Column);
				}
				finally
				{
					//aggiorno top e left
					Rows.ResumeLayout();
					Columns.ResumeLayout();
				}

				//Call this method after calculated Bottom and Right
				if (AutoStretchColumnsToFitWidth)
					Columns.StretchToFit();
				if (AutoStretchRowsToFitHeight)
					Rows.StretchToFit();
			}
			ResumeLayout(false);
		}

		/// <summary>
		/// Auto size all the columns and all the rows with the required width and height
		/// </summary>
		public virtual void AutoSizeCells()
		{
			AutoSizeCells(CompleteRange);
		}

		private bool m_bAutoStretchColumnsToFitWidth = false;
		/// <summary>
		/// True to auto stretch the columns width to always fit the available space, also when the contents of the cell is smaller.
		/// False to leave the original width of the columns
		/// </summary>
		[DefaultValue(false)]
		public bool AutoStretchColumnsToFitWidth
		{
			get{return m_bAutoStretchColumnsToFitWidth;}
			set{m_bAutoStretchColumnsToFitWidth = value;}
		}
		private bool m_bAutoStretchRowsToFitHeight = false;
		/// <summary>
		/// True to auto stretch the rows height to always fit the available space, also when the contents of the cell is smaller.
		/// False to leave the original height of the rows
		/// </summary>
		[DefaultValue(false)]
		public bool AutoStretchRowsToFitHeight
		{
			get{return m_bAutoStretchRowsToFitHeight;}
			set{m_bAutoStretchRowsToFitHeight = value;}
		}
		#endregion

		#region CheckPositions
		/// <summary>
		/// Check if the positions saved are still valid, for example if all the selected cells are still valid positions, if not the selection are removed without calling any other methods.
		/// </summary>
		public virtual void CheckPositions()
		{
			Range complete = CompleteRange;

			if (m_MouseCellPosition.IsEmpty() == false &&
			    CompleteRange.Contains(m_MouseCellPosition) == false)
				m_MouseCellPosition = Position.Empty;

			if (m_MouseDownPosition.IsEmpty() == false &&
			    CompleteRange.Contains(m_MouseDownPosition) == false)
				m_MouseDownPosition = Position.Empty;

			if (mDragCellPosition.IsEmpty() == false &&
			    CompleteRange.Contains(mDragCellPosition) == false)
				mDragCellPosition = Position.Empty;

			//If the selection contains some invalid cells reset the selection state.
			RangeRegion completeRegion = new RangeRegion(complete);
			if (
				(Selection.ActivePosition.IsEmpty() == false && complete.Contains(Selection.ActivePosition) == false) ||
				(Selection.IsEmpty() == false && completeRegion.Contains(Selection.GetSelectionRegion()) == false)
			)
				Selection.ResetSelection(false);
		}
		#endregion

		#region Position methods
		/// <summary>
		/// Get the rectangle of the cell respect to the client area visible, the grid DisplayRectangle.
		/// Returns Rectangle.Empty if the Position is empty or if is not valid.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Rectangle PositionToRectangle(Position position)
		{
			return RangeToRectangle(PositionToCellRange(position));
		}

		/// <summary>
		/// Returns the cell at the specified grid view relative point (the point must be relative to the grid display region), SearchInFixedCells = true. Return Position.Empty if no valid cells are found.
		/// </summary>
		/// <param name="point">Point relative to the DisplayRectangle area.</param>
		/// <returns></returns>
		public virtual Position PositionAtPoint(Point point)
		{
			int? row = Rows.RowAtPoint(point.Y);
			if (row == null)
				return Position.Empty;

			int? col = Columns.ColumnAtPoint(point.X);
			if (col == null)
				return Position.Empty;

			Position realPos = new Position(row.Value, col.Value);

			//Returns the logical position (using row/col span if present)
			return PositionToStartPosition(realPos);
		}

		#endregion

		#region Range methods

		public Size RangeToSize(Range range)
		{
			if (range.IsEmpty())
				return Size.Empty;

			int width = 0;
			for (int c = range.Start.Column; c <= range.End.Column; c++)
				width += Columns.GetWidth(c);

			int height = 0;
			for (int r = range.Start.Row; r <= range.End.Row; r++)
				height += Rows.GetHeight(r);

			return new Size(width, height);
		}
		

		/// <summary>
		/// Returns the relative rectangle to the current scrollable area of the specified Range.
		/// Returns a Rectangle.Empty if the Range is not valid.
		/// </summary>
		public Rectangle RangeToRectangle(Range range)
		{
			if (range.IsEmpty())
				return Rectangle.Empty;

			if (range.Start.Column < 0)
				throw new ArgumentOutOfRangeException(string.Format("range.Start.Column was less than zero: {0}", range.Start.Column));
			int x = Columns.GetLeft(range.Start.Column);
			if (range.Start.Row < 0)
				throw new ArgumentOutOfRangeException(string.Format("range.Start.Row was less than zero: {0}", range.Start.Row));
			int y = Rows.GetTop(range.Start.Row);

			Size size = RangeToSize(range);
			if (size.IsEmpty)
				return Rectangle.Empty;

			return new Rectangle(new Point(x, y), size);
		}

		/// <summary>
		/// Get the range of cells at the specified dispaly area.
		/// This method consider only the visible cells using the current scroll position.
		/// Returns a single Range for the specified grid area (scrollable, fixedtop, fixedleft, fixedtopleft).
		/// Returns Range.Empty if there isn't a valid range in the specified area.
		/// </summary>
		public Range RangeAtArea(CellPositionType areaType)
		{
			if (areaType == CellPositionType.FixedTopLeft)
			{
				if (FixedRows > 0 && Rows.Count >= FixedRows &&
				    FixedColumns > 0 && Columns.Count >= FixedColumns)
					return new Range(0, 0, FixedRows - 1, FixedColumns - 1);
				else
					return Range.Empty;
			}
			else if (areaType == CellPositionType.FixedLeft)
			{
				int actualFixed = FixedColumns;
				if (actualFixed > Columns.Count)
					actualFixed = Columns.Count;

				if (actualFixed <= 0)
					return Range.Empty;

				int? firstRow = Rows.FirstVisibleScrollableRow;
				int? lastRow = Rows.LastVisibleScrollableRow;

				if (firstRow == null || lastRow == null)
					return Range.Empty;

				return new Range(firstRow.Value, 0, lastRow.Value, actualFixed - 1);
			}
			else if (areaType == CellPositionType.FixedTop)
			{
				int actualFixed = FixedRows;
				if (actualFixed > Rows.Count)
					actualFixed = Rows.Count;

				if (actualFixed <= 0)
					return Range.Empty;

				int? firstCol = Columns.FirstVisibleScrollableColumn;
				int? lastCol = Columns.LastVisibleScrollableColumn;

				if (firstCol == null || lastCol == null)
					return Range.Empty;

				return new Range(0, firstCol.Value, actualFixed - 1, lastCol.Value);
			}
			else if (areaType == CellPositionType.Scrollable)
			{
				int? firstRow = Rows.FirstVisibleScrollableRow;
				int? lastRow = Rows.LastVisibleScrollableRow;

				int? firstCol = Columns.FirstVisibleScrollableColumn;
				int? lastCol = Columns.LastVisibleScrollableColumn;

				if (firstRow == null || firstCol == null ||
				    lastRow == null || lastCol == null)
					return Range.Empty;

				return new Range(firstRow.Value, firstCol.Value,
				                 lastRow.Value, lastCol.Value);
			}
			else
				throw new SourceGridException("Invalid areaType");
		}

		///// <summary>
		///// Get the range for the specific area type and rectangle
		///// </summary>
		//public Range RangeAtRectangle(CellPositionType areaType, Rectangle clipRectangle)
		//{
		//    if (areaType == CellPositionType.FixedTopLeft)
		//    {
		//    }
		//    else if (areaType == CellPositionType.FixedLeft)
		//    {
		//    }
		//    else if (areaType == CellPositionType.FixedTop)
		//    {
		//    }
		//    else if (areaType == CellPositionType.Scrollable)
		//    {
		//    }
		//    else
		//        throw new SourceGridException("Invalid areaType");
		//}

		/// <summary>
		/// Get the visible ranges. Returns a list of Range, one for each area.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<Range> GetVisibleRegion()
		{
			Range rng;

			rng = RangeAtArea(CellPositionType.FixedTopLeft);
			if (rng.IsEmpty() == false)
				yield return rng;
			rng = RangeAtArea(CellPositionType.FixedTop);
			if (rng.IsEmpty() == false)
				yield return rng;
			rng = RangeAtArea(CellPositionType.FixedLeft);
			if (rng.IsEmpty() == false)
				yield return rng;
			rng = RangeAtArea(CellPositionType.Scrollable);
			if (rng.IsEmpty() == false)
				yield return rng;
		}

		#endregion

		#region Rectangle, range and scrollable areas methods

		public List<int> GetVisibleRows(bool returnsPartial)
		{
			return GetVisibleRows(DisplayRectangle, returnsPartial);
		}

		public List<int> GetVisibleRows(Rectangle displayRectangle, bool returnsPartial)
		{
			var allRows = Rows.RowsInsideRegion(displayRectangle.Y, displayRectangle.Height, returnsPartial, true);
			var onlyVisible = new List<int>(allRows.Count);
			foreach (var index in allRows)
			{
				if (Rows.IsRowVisible(index) == true)
					onlyVisible.Add(index);
			}
			return onlyVisible;
		}

		public List<int> GetVisibleColumns(bool returnsPartial)
		{
			return GetVisibleColumns(DisplayRectangle, returnsPartial);
		}

		public List<int> GetVisibleColumns(Rectangle displayRectangle, bool returnsPartial)
		{
			return Columns.ColumnsInsideRegion(displayRectangle.X, displayRectangle.Width, returnsPartial, true);
		}

		///// <summary>
		///// Returns the logical scroll size (usually Rows and Columns) for the specified display area.
		///// </summary>
		///// <param name="displayRectangle"></param>
		///// <returns></returns>
		//protected override Size GetVisibleScrollArea(Rectangle displayRectangle)
		//{
		//    List<int> rows = GetVisibleRows(displayRectangle, false);
		//    List<int> columns = GetVisibleColumns(displayRectangle, false);

		//    return new Size(columns.Count, rows.Count);
		//}

		///// <summary>
		///// Returns true if the vertical scrollbar is required
		///// </summary>
		///// <returns></returns>
		//protected override bool RequireVerticalScroll(Size displaySize)
		//{
		//    int currentHeight = 0;

		//    for (int r = 0; r < Rows.Count; r++)
		//    {
		//        currentHeight += Rows.GetHeight(r);

		//        if (currentHeight > displaySize.Height)
		//            return true;
		//    }

		//    return false;
		//}

		///// <summary>
		///// Returns true if the horizontal scrollbar is required
		///// </summary>
		///// <returns></returns>
		//protected override bool RequireHorizontalScroll(Size displaySize)
		//{
		//    int currentWidth = 0;

		//    for (int c = 0; c < Columns.Count; c++)
		//    {
		//        currentWidth += Columns.GetWidth(c);

		//        if (currentWidth > displaySize.Width)
		//            return true;
		//    }

		//    return false;
		//}

		/// <summary>
		/// Calculate the number of rows to scroll. 0 to disable the scrollbar.
		/// The returned value is independent from the current scrolling position, must be a fixed value
		/// calculated based on the total number of rows and the available area.
		/// </summary>
		protected override int GetScrollRows(int displayHeight)
		{
			if (displayHeight < 0)
				return 0;
			
			int currentHeight = 0;
			int scrollRows = 0;

			//Remove the fixed rows from the scrollable area
			for (int f = 0; f < ActualFixedRows; f++)
				displayHeight -= Rows.GetHeight(f);

			//Calculate the rows to be scrolled
			for (int r = Rows.Count - 1; r >= ActualFixedRows; r--)
			{
				if (Rows.IsRowVisible(r) == false)
					continue;
				currentHeight += Rows.GetHeight(r);

				if (currentHeight > displayHeight)
					return Rows.Count - scrollRows - GetTotalHiddenRows(r);

				scrollRows++;
			}

			return 0;
		}
		
		private int GetTotalHiddenRows(int from)
		{
			return Rows.HiddenRowsCoordinator.GetTotalHiddenRows();
			/*var hidden = 0;
			for (int x = from; x >= ActualFixedRows; x--)
			{
				if (Rows.IsRowVisible(x) == false)
					hidden ++;
			}
			return hidden;*/
		}
		
		
		//MICK(13)
		protected override int GetActualFixedRows()
		{
			return ActualFixedRows;
		}

		/*
        //MICK(14)
        protected int GetHiddenRows()
        {
            int HiddenRows=0;

            for (int r = 0; r < Rows.Count; r++)
            {
                //IsRowVisible - this function name is confusing - is it visible on the screen? - no.
                //Also FirstVisibleScrollableRow - is it a bug - it does not consider hidden rows?
                if (!Rows.IsRowVisible(r)) HiddenRows++;
            }

            return HiddenRows;
        }
		 */

		/// <summary>
		/// Calculate the number of columns to scroll. 0 to disable the scrollbar.
		/// The returned value is independent from the current scrolling position, must be a fixed value
		/// calculated based on the total number of columns and the available area.
		/// </summary>
		protected override int GetScrollColumns(int displayWidth)
		{
			if (displayWidth < 0)
				return 0;
			int currentWidth = 0;
			int scrollCols = 0;

			//Remove the fixed columns from the scrollable area
			for (int f = 0; f < ActualFixedColumns; f++)
				displayWidth -= Columns.GetWidth(f);

			//Calculate the columns to be scrolled
			for (int c = Columns.Count - 1; c >= ActualFixedColumns; c--)
			{
				currentWidth += Columns.GetWidth(c);

				if (currentWidth > displayWidth)
					return Columns.Count - scrollCols;

				scrollCols++;
			}

			return 0;
		}

		/// <summary>
		/// Indicates if the specified cell is visible.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="partial">True to returns also partial visible cells</param>
		/// <returns></returns>
		public bool IsCellVisible(Position position, bool partial)
		{
			Point scrollPosition;
			return !(GetScrollPositionToShowCell(position, partial, out scrollPosition));
		}

		///// <summary>
		///// Indicates if the specified range is visible
		///// </summary>
		///// <param name="range"></param>
		///// <param name="partial">True to return also partial visible cells</param>
		///// <returns></returns>
		//public bool IsRangeVisible(Range range, bool partial)
		//{
		//    List<int> rows = GetVisibleRows(partial);
		//    List<int> columns = GetVisibleColumns(partial);

		//    if (rows.Count == 0 || columns.Count == 0)
		//        return false;

		//    //All the fixed rows are considered to be visible
		//    bool isRowVisible = false;
		//    if (range.Start.Row < FixedRows)
		//        isRowVisible = true;
		//    else if (range.Start.Row <= rows[rows.Count - 1] &&
		//                range.End.Row >= rows[0])
		//        isRowVisible = true;

		//    bool isColVisible = false;
		//    if (range.Start.Column < FixedColumns)
		//        isColVisible = true;
		//    else if (range.Start.Column <= columns[columns.Count - 1] &&
		//                range.End.Column >= columns[0])
		//        isColVisible = true;

		//    return isColVisible && isRowVisible;
		//}

		/// <summary>
		/// Return the scroll position that must be set to show a specific cell.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="partial">True to consider also partial visible cells</param>
		/// <param name="newScrollPosition"></param>
		/// <returns>Return false if the cell is already visible, return true is the cell is not currently visible.</returns>
		protected virtual bool GetScrollPositionToShowCell(Position position, bool partial, out Point newScrollPosition)
		{
			Rectangle displayRectangle = DisplayRectangle;

			List<int> rows = GetVisibleRows(partial);
			List<int> columns = GetVisibleColumns(partial);

			if (rows.Contains(position.Row) && columns.Contains(position.Column))
			{
				newScrollPosition = CustomScrollPosition;
				return false;
			}
			else
			{
				CellPositionType posType = GetPositionType(position);
				bool isFixedTop = false;
				if (posType == CellPositionType.FixedTop || posType == CellPositionType.FixedTopLeft)
					isFixedTop = true;
				bool isFixedLeft = false;
				if (posType == CellPositionType.FixedLeft || posType == CellPositionType.FixedTopLeft)
					isFixedLeft = true;

				int x;
				if (columns.Contains(position.Column)) //Is x visible
				{
					x = CustomScrollPosition.X;
				}
				else
				{
					if (isFixedLeft)
						x = 0;
					else
						x = position.Column - FixedColumns;

					//Check if the scrollable positioin if not outside the valid area
					int maxX = GetScrollColumns(displayRectangle.Width);
					if (x > maxX)
						x = maxX;
				}

				int y;
				if (rows.Contains(position.Row)) //Is y visible
				{
					y = CustomScrollPosition.Y;
					y = ExtendTopRowByHiddenRows(y);
				}
				else
				{
					//MICK(15): add direction dependent procedure of finding y (and x should be too).
					//It means that when you go from bottom to top, the selected item will be at top,
					//              (this already happens, so this part is not changed)
					//              when you go from top to bottom, the selected item will be at bottom,
					// y (and x) value is adjusted accordingly
					if (CustomScrollPosition.Y + ActualFixedRows > position.Row)
					{
						//MICK(16): direction bottom to top
						if (isFixedTop)
							y = 0;
						else
							y = position.Row - FixedRows;

						//Check if the scrollable positioin if not outside the valid area
						int maxY = GetScrollRows(displayRectangle.Height);
						if (y > maxY)
							y = maxY;
						//MICK(21)
						y=ExtendTopRowByHiddenRows(y);

					}
					else
					{
						//MICK(17): direction from top to bottom
						y = GetTopVisibleRowFromBottomRow(position.Row) - ActualFixedRows;
						y = ExtendTopRowByHiddenRows(y);
					}
				}

				newScrollPosition = new Point(x, y);

				return true;
			}
		}

		/// <summary>
		/// Scroll the view to show the cell passed. Ensure that if the cell if invisible or partial visible it will be totally visible
		/// </summary>
		/// <param name="p_Position"></param>
		/// <param name="ignorePartial">true to ignore and consider already visible partial visible cells</param>
		/// <returns>Returns true if the Cell passed was already visible, otherwise false</returns>
		public bool ShowCell(Position p_Position, bool ignorePartial)
		{
			Point l_newCustomScrollPosition;
			if (GetScrollPositionToShowCell(p_Position, ignorePartial, out l_newCustomScrollPosition))
			{
				CustomScrollPosition = l_newCustomScrollPosition;
				//il problema di refresh si verifica solo in caso di FixedRows e ColumnsCount maggiori di 0
				if (FixedRows > 0 || FixedColumns > 0)
					Invalidate();

				return false;
			}
			return true;
		}


		/// <summary>
		/// Force a cell to redraw.
		/// </summary>
		/// <param name="position"></param>
		public virtual void InvalidateCell(Position position)
		{
			InvalidateRange(new Range(position));
		}

		/// <summary>
		/// Force a range of cells to redraw.
		/// </summary>
		/// <param name="range"></param>
		public void InvalidateRange(Range range)
		{
			if (range.IsEmpty())
				return;
			CellPositionType[] types = new CellPositionType[]{
				CellPositionType.FixedLeft,
				CellPositionType.FixedTop,
				CellPositionType.FixedTopLeft,
				CellPositionType.Scrollable
			};
			
			// invalidate eaach position type individually
			// cliping against given range
			foreach (CellPositionType type in types)
			{
				Range visibleRange = RangeAtArea(type);
				// clip our range with visible range
				// so that we wount loop through thousands of rows
				Range clippedRange = Range.Intersect(range, visibleRange);
				if (clippedRange.IsEmpty() == false)
				{
					Rectangle gridRectangle = RangeToRectangle(clippedRange);
					if (gridRectangle.IsEmpty == false)
						Invalidate(gridRectangle, true);
				}
			}
		}

		/// <summary>
		/// Returns a visible area range, which is a bit
		/// expanded to the left and top
		/// </summary>
		/// <param name="areaType"></param>
		/// <returns></returns>
		public Range RangeAtAreaExpanded(CellPositionType areaType)
		{
			Range range = RangeAtArea(areaType);
			// decrease visible row by 1, so that selection top row would not be drawn
			// if selection start somewhere outside the screen
			if (range.IsEmpty() == false)
			{
				int startRow = range.Start.Row > 0 ? range.Start.Row -1 : 0;
				int startCol = range.Start.Column > 0 ? range.Start.Column - 1 : 0;
				range = new Range(
					new Position(startRow, startCol),
					range.End);
			}
			return range;
		}
		
		/// <summary>
		/// Move the scrollbars to the direction specified by the point specified.
		/// Method used by the Mouse multi selection (MouseSelection.cs).
		/// Scroll the grid only if the specified location is outside the visible area.
		/// </summary>
		/// <param name="mousePoint"></param>
		public void ScrollOnPoint(Point mousePoint)
		{
			//Scroll if necesary
			Rectangle scrollRect = GetScrollableArea();

			int? last = this.Columns.LastVisibleScrollableColumn;
			if (mousePoint.X > scrollRect.Right && (! last.HasValue ||
			                                        last.Value < this.Columns.Count - 1 || this.Columns.GetRight(last.Value) > scrollRect.Right) )
				CustomScrollLineRight();
			last = this.Rows.LastVisibleScrollableRow;
			if (mousePoint.Y > scrollRect.Bottom && (! last.HasValue ||
			                                         last.Value < this.Rows.Count - 1 || this.Rows.GetBottom(last.Value) > scrollRect.Bottom) )
				CustomScrollLineDown();
			if (mousePoint.X < scrollRect.Left)
				CustomScrollLineLeft();
			if (mousePoint.Y < scrollRect.Top)
				CustomScrollLineUp();
		}

		/// <summary>
		/// Invalidate the cells
		/// </summary>
		protected override void InvalidateScrollableArea()
		{
			Invalidate(true);
		}


		public Rectangle GetScrollableArea()
		{
			Rectangle rect = DisplayRectangle;

			int fixedRows = ActualFixedRows;

			int fixedColumns = ActualFixedColumns;

			if (fixedRows > 0)
				rect.Y = Rows.GetAbsoluteBottom(fixedRows - 1);
			else
				rect.Y = 0;
			rect.Height -= rect.Y;

			if (fixedColumns > 0)
				rect.X = Columns.GetAbsoluteRight(fixedColumns - 1);
			else
				rect.X = 0;
			rect.Width -= rect.X;

			return rect;
		}
		public Rectangle GetFixedTopLeftArea()
		{
			Rectangle rect = GetScrollableArea();

			return new Rectangle(0, 0, rect.Left, rect.Top);
		}

		public Rectangle GetFixedTopArea()
		{
			Rectangle rect = GetScrollableArea();

			return new Rectangle(rect.Left, 0, rect.Width, rect.Top);
		}
		public Rectangle GetFixedLeftArea()
		{
			Rectangle rect = GetScrollableArea();

			return new Rectangle(0, rect.Top, rect.Left, rect.Height);
		}
		#endregion

		#region Row/Column Span
		
		
		
		/// <summary>
		/// This method converts a Position to the real range of the cell. This is usefull when RowSpan or ColumnSpan is greater than 1.
		/// For example suppose to have at grid[0,0] a cell with ColumnSpan equal to 2. If you call this method with the position 0,0 returns 0,0-0,1 and if you call this method with 0,1 return again 0,0-0,1.
		/// </summary>
		/// <returns></returns>
		public virtual Range RangeToCellRange(Range range)
		{
			return new Range(range.Start, range.End);
		}
		
		/// <summary>
		/// This method converts a Position to the real start position of cell. This is usefull when RowSpan or ColumnSPan is greater than 1.
		/// For example suppose to have at grid[0,0] a cell with ColumnSpan equal to 2. If you call this method with the position 0,0 returns 0,0 and if you call this method with 0,1 return again 0,0.
		/// Get the real position for the specified position. For example when p_Position is a merged cell this method returns the starting position of the merged cells.
		/// Usually this method returns the same cell specified as parameter. This method is used for processing arrow keys, to find a valid cell when the focus is in a merged cell.
		/// For this class returns always p_Position.
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public Position PositionToStartPosition(Position p_Position)
		{
			return PositionToCellRange(p_Position).Start;
		}

		/// <summary>
		/// This method converts a Position to the real range of the cell. This is usefull when RowSpan or ColumnSpan is greater than 1.
		/// For example suppose to have at grid[0,0] a cell with ColumnSpan equal to 2. If you call this method with the position 0,0 returns 0,0-0,1 and if you call this method with 0,1 return again 0,0-0,1.
		/// </summary>
		/// <param name="pPosition"></param>
		/// <returns></returns>
		public virtual Range PositionToCellRange(Position pPosition)
		{
			if (pPosition.IsEmpty())
				return Range.Empty;

			ICellVirtual l_Cell = this.GetCell(pPosition.Row, pPosition.Column);
			if (l_Cell == null)
				return Range.Empty;
			else
				return new Range(pPosition);
			//return new Range(pPosition);
		}
		#endregion

		#region Drag Fields
		/// <summary>
		/// Indica la cella che ha subito l'ultimo evento di DragEnter
		/// </summary>
		private Position mDragCellPosition = Position.Empty;

		/// <summary>
		/// The last cell that has received a DragEnter event.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Position DragCellPosition
		{
			get{return mDragCellPosition;}
		}

		/// <summary>
		/// Fired when the cell in the drag events change. For internal use only.
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="pDragEventArgs"></param>
		public virtual void ChangeDragCell(CellContext cell, DragEventArgs pDragEventArgs)
		{
			if (cell.Position != mDragCellPosition)
			{
				if (mDragCellPosition.IsEmpty() == false)
					Controller.OnDragLeave(new CellContext(this, mDragCellPosition, GetCell(mDragCellPosition)), pDragEventArgs);
				
				if (cell.Position.IsEmpty() == false)
					Controller.OnDragEnter(cell, pDragEventArgs);

				mDragCellPosition = cell.Position;
			}
		}
		#endregion

		#region Selection
		/// <summary>
		/// Virtual factory method used to create the SelectionBase derived object.
		/// The base method create a different object based on the value of SelectionMode property.
		/// </summary>
		/// <returns></returns>
		protected virtual Selection.SelectionBase CreateSelectionObject()
		{
			switch (SelectionMode)
			{
				case GridSelectionMode.Cell:
					return new Selection.FreeSelection();
				case GridSelectionMode.Column:
					return new Selection.ColumnSelection();
				case GridSelectionMode.Row:
					return new Selection.RowSelection();
				default:
					throw new ArgumentException("SelectionMode not valid", "SelectionMode");
			}
		}

		private IGridSelection mSelection;
		/// <summary>
		/// Gets or sets the Selection object that represents the selected cells and the active cell.
		/// Use the SelectionMode property to set the type of selection (free, by row, by column).
		/// Override the CreateSelectionObject method to create a custom Selection class for special needs.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IGridSelection Selection
		{
			get{return mSelection;}
			protected set
			{
				if (mSelection != null)
					mSelection.UnBindToGrid();

				mSelection = value;

				if (mSelection != null)
					mSelection.BindToGrid(this);
			}
		}

		private GridSelectionMode mSelectionMode;
		/// <summary>
		/// Gets or sets the selection mode. Changing this property cause the Selection object to be recreated, for this reason remember to set this property at the beginning of your code.
		/// </summary>
		public GridSelectionMode SelectionMode
		{
			get { return mSelectionMode; }
			set
			{
				mSelectionMode = value;
				Selection = CreateSelectionObject();
				Invalidate(true);
			}
		}
		#endregion

		#region Mouse cell positions and mouse selection
		/// <summary>
		/// Represents the cell that receive the mouse down event
		/// </summary>
		protected Position m_MouseDownPosition = Position.Empty;

		/// <summary>
		/// Represents the cell that have received the MouseDown event. You can use this cell for contextmenu logic. Can be null.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Position MouseDownPosition
		{
			get { return m_MouseDownPosition; }
		}

		protected Position m_MouseCellPosition = Position.Empty;
		/// <summary>
		/// The cell position currently under the mouse cursor (row, col).
		/// If you MouseDown on a cell this cell is the MouseCellPosition until an MouseUp is fired
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Position MouseCellPosition
		{
			get{return m_MouseCellPosition;}
		}

		/// <summary>
		/// Fired when the cell under the mouse change. For internal use only.
		/// </summary>
		/// <param name="p_Cell"></param>
		public virtual void ChangeMouseCell(Position p_Cell)
		{
			if (m_MouseCellPosition != p_Cell)
			{
				//se la cella che sta perdento il mouse è anche quella che ha ricevuto un eventuale evento di MouseDown non scateno il MouseLeave (che invece verrà scatenato dopo il MouseUp)
				if (m_MouseCellPosition.IsEmpty() == false &&
				    m_MouseCellPosition != m_MouseDownPosition)
				{
					Controller.OnMouseLeave(new CellContext(this, m_MouseCellPosition), EventArgs.Empty);
				}

				m_MouseCellPosition = p_Cell;
				if (m_MouseCellPosition.IsEmpty() == false)
				{
					Controller.OnMouseEnter(new CellContext(this, m_MouseCellPosition), EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Change the cell currently under the mouse
		/// </summary>
		/// <param name="p_MouseDownCell"></param>
		/// <param name="p_MouseCell"></param>
		public virtual void ChangeMouseDownCell(Position p_MouseDownCell, Position p_MouseCell)
		{
			m_MouseDownPosition = p_MouseDownCell;

			ChangeMouseCell(p_MouseCell);
		}

		/// <summary>
		/// Fired when the selection eith the mouse is finished
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseSelectionFinish(RangeEventArgs e)
		{
			m_OldMouseSelectionRange = Range.Empty;
		}

		private Range m_OldMouseSelectionRange = Range.Empty;
		private Range m_MouseSelectionRange = Range.Empty;
		/// <summary>
		/// Returns the cells that are selected with the mouse. Range.Empty if no cells are selected. Consider that this method returns valid cells only during the mouse down operations, when release the mouse the cells are selected and you can read them using Grid.Selection object.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Range MouseSelectionRange
		{
			get{return m_MouseSelectionRange;}
		}

		/// <summary>
		/// Fired when the mouse selection must be canceled. See also MouseSelectionRange.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnUndoMouseSelection(RangeEventArgs e)
		{
			Selection.SelectRange(e.Range, false);
		}

		/// <summary>
		/// Fired when the mouse selection is succesfully finished. See also MouseSelectionRange.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnApplyMouseSelection(RangeEventArgs e)
		{
			Selection.SelectRange(e.Range, true);
		}

		/// <summary>
		/// Fired when the mouse selection change. See also MouseSelectionRange.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseSelectionChange(EventArgs e)
		{
			Range l_MouseRange = MouseSelectionRange;

			OnUndoMouseSelection(new RangeEventArgs(m_OldMouseSelectionRange));

			OnApplyMouseSelection(new RangeEventArgs(l_MouseRange));

			m_OldMouseSelectionRange = l_MouseRange;
		}

		/// <summary>
		/// Fired when the mouse selection finish. See also MouseSelectionRange.
		/// </summary>
		public void MouseSelectionFinish()
		{
			if (m_MouseSelectionRange != Range.Empty)
				OnMouseSelectionFinish(new RangeEventArgs(m_OldMouseSelectionRange));

			m_MouseSelectionRange = Range.Empty;
		}

		/// <summary>
		/// Fired when the corner of the mouse selection change. For internal use only.
		/// </summary>
		/// <param name="p_Corner"></param>
		public virtual void ChangeMouseSelectionCorner(Position p_Corner)
		{
			Range newMouseSelection = new Range(Selection.ActivePosition, p_Corner);

			bool l_bChange = false;
			if (m_MouseSelectionRange != newMouseSelection)
			{
				m_MouseSelectionRange = newMouseSelection;
				l_bChange = true;
			}

			if (l_bChange)
				OnMouseSelectionChange(EventArgs.Empty);
		}
		#endregion

		#region Special Keys and command keys
		/// <summary>
		/// Stores the active position when user presses shift
		/// </summary>
 		private Position m_firstCellShiftSelected;

		/// <summary>
		/// Processes a command key.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(
			ref Message msg,
			Keys keyData
		)
		{
			// if shift pressed, remember which cell is selected
            if (keyData == (Keys.Shift | Keys.ShiftKey))
			{
                m_firstCellShiftSelected = Selection.ActivePosition;
			}

			if ((keyData == Keys.Enter ||
			     keyData == Keys.Escape ||
			     keyData == Keys.Tab ||
			     keyData == (Keys.Tab | Keys.Shift)) &&
			    OverrideCommonCmdKey)
			{
				KeyEventArgs args = new KeyEventArgs(keyData);
				OnKeyDown(args);
				if (args.Handled)
					return true;
				else
					return base.ProcessCmdKey(ref msg, keyData);
			}
			else
				return base.ProcessCmdKey(ref msg, keyData);
		}

		private bool m_OverrideCommonCmdKey = true;
		/// <summary>
		/// True to override with the ProcessCmdKey the common Command Key: Enter, Escape, Tab
		/// </summary>
		[DefaultValue(true)]
		public bool OverrideCommonCmdKey
		{
			get { return m_OverrideCommonCmdKey; }
			set { m_OverrideCommonCmdKey = value; }
		}

		private GridSpecialKeys m_GridSpecialKeys = GridSpecialKeys.Default;

		/// <summary>
		/// Special keys that the grid can handle. You can change this enum to block or allow some special keys function. For example to disable Ctrl+C Copy operation remove from this enum the GridSpecialKeys.Ctrl_C.
		/// </summary>
		[DefaultValue(GridSpecialKeys.Default)]
		public GridSpecialKeys SpecialKeys
		{
			get{return m_GridSpecialKeys;}
			set{m_GridSpecialKeys = value;}
		}

		private bool mAcceptsInputChar = true;
		/// <summary>
		/// True accepts input char when the grid has the focus. Used for example to start the edit operation without processing the char.
		/// If you set this property to false when the character is sended to the windows forms handler and can be used for example to execute directly some access key (shortcut keys).
		/// Default is true.
		/// </summary>
		[DefaultValue(true)]
		public bool AcceptsInputChar
		{
			get{return mAcceptsInputChar;}
			set{mAcceptsInputChar = value;}
		}

		/// <summary>
		/// Process Delete, Ctrl+C, Ctrl+V, Up, Down, Left, Right, Tab keys
		/// </summary>
		/// <param name="e"></param>
		public virtual void ProcessSpecialGridKey(KeyEventArgs e)
		{
			if (e.Handled)
				return;

			bool enableArrows,enableTab,enablePageDownUp;
			enableArrows = enableTab = enablePageDownUp = false;

			if ( (SpecialKeys & GridSpecialKeys.Arrows) == GridSpecialKeys.Arrows)
				enableArrows = true;
			if ( (SpecialKeys & GridSpecialKeys.PageDownUp) == GridSpecialKeys.PageDownUp)
				enablePageDownUp = true;
			if ( (SpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab)
				enableTab = true;

			bool enableEscape = false;
			if ( (SpecialKeys & GridSpecialKeys.Escape) == GridSpecialKeys.Escape)
				enableEscape = true;
			bool enableEnter = false;
			if ( (SpecialKeys & GridSpecialKeys.Enter) == GridSpecialKeys.Enter)
				enableEnter = true;

			#region Processing keys
			//Escape
			if (e.KeyCode == Keys.Escape && enableEscape)
			{
				CellContext focusCellContext = new CellContext(this, Selection.ActivePosition);
				if (focusCellContext.Cell != null && focusCellContext.IsEditing())
				{
					if (focusCellContext.EndEdit(true))
						e.Handled = true;
				}
			}

			//Enter
			if (e.KeyCode == Keys.Enter && enableEnter)
			{
				CellContext focusCellContext = new CellContext(this, Selection.ActivePosition);
				if (focusCellContext.Cell != null && focusCellContext.IsEditing())
				{
					focusCellContext.EndEdit(false);

					e.Handled = true;
				}
			}

			//Tab
			if (e.KeyCode == Keys.Tab && enableTab)
			{
				CellContext focusCellContext = new CellContext(this, Selection.ActivePosition);
				if (focusCellContext.Cell != null && focusCellContext.IsEditing())
				{
					//se l'editing non riesce considero il tasto processato
					// altrimenti no, in questo modo il tab ha effetto anche per lo spostamento
					if (focusCellContext.EndEdit(false) == false)
					{
						e.Handled = true;
						return;
					}
				}
			}
			#endregion

			#region Navigate keys: arrows, tab and PgDown/Up
			var shiftPressed = e.Modifiers == Keys.Shift;
			var resetSelection = shiftPressed == false;
			if (e.KeyCode == Keys.Down && enableArrows)
			{
				Selection.MoveActiveCell(1, 0, resetSelection);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Up && enableArrows)
			{
				Selection.MoveActiveCell(-1, 0, resetSelection);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Right && enableArrows)
			{
				Selection.MoveActiveCell(0, 1, resetSelection);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Left && enableArrows)
			{
				Selection.MoveActiveCell(0, -1, resetSelection);
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Tab && enableTab)
			{
				//If the tab failed I automatically select the next control in the form (SelectNextControl)

				if (e.Modifiers == Keys.Shift) // backward
				{
					if (Selection.MoveActiveCell(0, -1, -1, int.MaxValue) == false)
						FindForm().SelectNextControl(this, false, true, true, true);
					e.Handled = true;
				}
				else //forward
				{
					if (Selection.MoveActiveCell(0, 1, 1, int.MinValue) == false)
						FindForm().SelectNextControl(this, true, true, true, true);
					e.Handled = true;
				}
			}
			else if ( (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown)
			         && enablePageDownUp)
			{
				//MICK(18): I commented out this part - focusing is done within my CustomScrollPageDown() and CustomScrollPageUp()
				//Point focusPoint = PositionToRectangle(Selection.ActivePosition).Location;
				//focusPoint.Offset(1, 1); //in modo da entrare nella cella

				if (e.KeyCode == Keys.PageDown)
					CustomScrollPageDown();
				else if (e.KeyCode == Keys.PageUp)
					CustomScrollPageUp();

				//MICK(18): I commented out this part - focusing is done within my CustomScrollPageDown() and CustomScrollPageUp()
				//Position newPosition = PositionAtPoint(focusPoint);
				//if (Selection.CanReceiveFocus(newPosition))
				//    Selection.Focus(newPosition, true);
				
				e.Handled = true;
			}

            //ALAN - July 2013.  I added '&& e.handled' to the if clause
            // This fixed several buggy behaviours
            // Pressing SHIFT on its own caused a cascade of Focus events and selection_changed events - the knock-on effects were amazing!
            //  1. Just pressing SHIFT would cause multiple highlighted rows to become just one highlighted row
            //  2. SHIFT+mouse click did not work.
            if (shiftPressed && e.Handled)
            {
                Selection.ResetSelection(true);
                Selection.SelectRange(new Range(m_firstCellShiftSelected, Selection.ActivePosition), true);
            }

			#endregion

			#region Clipboard
			RangeRegion selRegion = (ClipboardUseOnlyActivePosition ? new RangeRegion(Selection.ActivePosition) : Selection.GetSelectionRegion());

			//Paste
			if (e.Control && e.KeyCode == Keys.V)
			{
				PerformPaste(selRegion);
				e.Handled = true;
			}
			//Copy
			else if (e.Control && e.KeyCode == Keys.C)
			{
				PerformCopy(selRegion);
				e.Handled = true;
			}
			//Cut
			else if (e.Control && e.KeyCode == Keys.X)
			{
				PerformCut(selRegion);
				e.Handled = true;
			}
			//Delete
			else if (e.KeyCode == Keys.Delete)
			{
				PerformDelete(selRegion);
				e.Handled = true;
			}
			#endregion
		}

		/// <summary>
		/// Checks if cut mode is enabled, and copies
		/// data into clipaborad
		/// </summary>
		/// <param name="selRegion"></param>
		public void PerformCut(RangeRegion selRegion)
		{
			bool cutEnabled = (ClipboardMode & ClipboardMode.Cut) == ClipboardMode.Cut;
			if (cutEnabled == false || selRegion.IsEmpty() == true)
				return;
			Range rng = selRegion[0];

			RangeData data = RangeData.LoadData(this, rng, CutMode.CutImmediately);
			RangeData.ClipboardSetData(data);
		}
		
		/// <summary>
		/// Checks if paste mode is enabled and writes
		/// data into grid from clipboard
		/// </summary>
		/// <param name="selRegion"></param>
		public void PerformPaste(RangeRegion selRegion)
		{
			bool pasteEnabled = (ClipboardMode & ClipboardMode.Paste) == ClipboardMode.Paste;
			if (pasteEnabled == false || selRegion.IsEmpty() == true)
				return;
			RangeData rngData = RangeData.ClipboardGetData();

			if (rngData == null)
				return;
			Range rng = selRegion[0];
			Range data = rngData.SourceRange;
			Range destinationRange = new Range(
				new Position(rng.Start.Row, rng.Start.Column),
				new Position(rng.Start.Row + (data.End.Row - data.Start.Row),
				             rng.Start.Column + (data.End.Column - data.Start.Column)));
			
			rngData.WriteData(this, rng.Start);
			
			Selection.ResetSelection(true);
			Selection.SelectRange(destinationRange, true);
		}
		
		/// <summary>
		/// Checks if copy mode is enabled and copies data into clipboard
		/// </summary>
		/// <param name="selRegion"></param>
		public void PerformCopy(RangeRegion selRegion)
		{
			bool copyEnabled = (ClipboardMode & ClipboardMode.Copy) == ClipboardMode.Copy;
			if (copyEnabled == false || selRegion.IsEmpty() == true)
				return;
			Range rng = selRegion[0];

			RangeData data = RangeData.LoadData(this, rng, CutMode.None);
			RangeData.ClipboardSetData(data);
		}
		
		/// <summary>
		/// Checks if delete is enabled, and clears values from
		/// given destination by invoking ClearValues(selRegion)
		/// </summary>
		/// <param name="selRegion"></param>
		public void PerformDelete(RangeRegion selRegion)
		{
			bool deleteEnabled = (ClipboardMode & ClipboardMode.Delete) == ClipboardMode.Delete;
			if (deleteEnabled == false || selRegion.IsEmpty() == true)
				return;
			ClearValues(selRegion);
		}
		/// <summary>
		/// Allow the grid to handle specials keys like Arrows and Tab. See also Grid.SpecialKeys
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool IsInputKey(Keys keyData)
		{
			//Handle arrows and tab keys only if OverrideCommonCmdKey
			if (OverrideCommonCmdKey)
			{
				if ((SpecialKeys & GridSpecialKeys.Arrows) == GridSpecialKeys.Arrows)
				{
					switch (keyData)
					{
						case Keys.Up:
						case Keys.Down:
						case Keys.Left:
						case Keys.Right:
							//altrimenti venivano (per qualche strana ragione) selezionate le scrollbars
						case (Keys.Up | Keys.Shift):
						case (Keys.Down | Keys.Shift):
						case (Keys.Left | Keys.Shift):
						case (Keys.Right | Keys.Shift):
							return true;
					}
				}

				if ((SpecialKeys & GridSpecialKeys.Tab) == GridSpecialKeys.Tab)
				{
					switch (keyData)
					{
						case Keys.Tab:
						case (Keys.Tab | Keys.Shift):
							return true;
					}
				}
			}

			return base.IsInputKey(keyData);
		}

		/// <summary>
		/// IsInputChar method.
		/// </summary>
		/// <param name="charCode"></param>
		/// <returns></returns>
		protected override bool IsInputChar(char charCode)
		{
			return AcceptsInputChar;
		}
		#endregion

		#region Paint

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			//NOTE: For now I draw all the visible cells (not only the invalidated cells).
			using (DevAge.Drawing.GraphicsCache grCache = new DevAge.Drawing.GraphicsCache(e.Graphics, e.ClipRectangle))
			{
				foreach (Range rng in GetVisibleRegion())
				{
					OnRangePaint(new RangePaintEventArgs(this, grCache, rng));
				}
			}
		}
		
		public event RangePaintEventHandler RangePaint;
		protected virtual void OnRangePaint(RangePaintEventArgs e)
		{
			Rectangle drawRectangle = RangeToRectangle(e.DrawingRange);

			System.Drawing.Drawing2D.GraphicsState state = e.GraphicsCache.Graphics.Save();

			try
			{
				e.GraphicsCache.Graphics.SetClip(drawRectangle);

				int top = drawRectangle.Top;
				int width;
				int height;
				foreach (var r in m_Rows.HiddenRowsCoordinator.LoopVisibleRows(e.DrawingRange.Start.Row, e.DrawingRange.End.Row - e.DrawingRange.Start.Row))
					//for (int r = e.DrawingRange.Start.Row; r <= e.DrawingRange.End.Row; r++)
				{
					if (Rows.IsRowVisible(r) == false)
						throw new SourceGridException("Incorrect internal state. This rows must have been visible");
					height = Rows.GetHeight(r);

					int left = drawRectangle.Left;
					for (int c = e.DrawingRange.Start.Column; c <= e.DrawingRange.End.Column; c++)
					{
						width = Columns.GetWidth(c);
						if (Columns.IsColumnVisible(c) == false)
							continue;

						Position position = new Position(r, c);

						Cells.ICellVirtual cell = GetCell(position);
						if (cell != null)
						{
							Rectangle drawRect = new Rectangle(left, top, width, height);

							//Draw the cell only if inside the ClipRectangle
							if (e.GraphicsCache.ClipRectangle.IsEmpty ||
							    e.GraphicsCache.ClipRectangle.IntersectsWith(drawRect))
							{
								CellContext cellContext = new CellContext(this, position, cell);
								PaintCell(e.GraphicsCache, cellContext, drawRect);
							}
						}

						left += width;
					}

					top += height;
				}

				//Draw the decorators
				foreach (SourceGrid.Decorators.DecoratorBase dec in Decorators)
				{
					if (dec.IntersectWith(e.DrawingRange))
						dec.Draw(e);
				}

				if (RangePaint != null)
					RangePaint(this, e);
			}
			finally
			{
				e.GraphicsCache.Graphics.Restore(state);
			}
		}

		/// <summary>
		/// Draw the specified Cell
		/// </summary>
		protected virtual void PaintCell(DevAge.Drawing.GraphicsCache graphics,
		                                 CellContext cellContext,
		                                 RectangleF drawRectangle)
		{
			if (drawRectangle.Width > 0 && drawRectangle.Height > 0 &&
			    cellContext.CanBeDrawn() )
			{
				cellContext.Cell.View.DrawCell(cellContext, graphics, drawRectangle);
			}
		}
		#endregion

		#region Mouse
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Position mousePosition = PositionAtPoint(new Point(e.X, e.Y));
			Cells.ICellVirtual mouseCell = GetCell(mousePosition);

			//Call MouseMove on the cell that receive tha MouseDown event
			if (MouseDownPosition.IsEmpty() == false)
			{
				Cells.ICellVirtual mouseDownCell = GetCell(MouseDownPosition);
				if (mouseDownCell != null)
					Controller.OnMouseMove(new CellContext(this, MouseDownPosition, mouseDownCell), e);
			}
			else //se non ho nessuna cella attualmente che ha ricevuto un mousedown, l'evento di MouseMove viene segnalato sulla cella correntemente sotto il Mouse
			{
				// se non c'è nessuna cella MouseDown cambio la cella corrente sotto il Mouse
				ChangeMouseCell(mousePosition);//in ogni caso cambio la cella corrente

				if (mousePosition.IsEmpty() == false && mouseCell != null)
				{
					// I call MouseMove on the current cell only if there aren't any cells under the mouse
					Controller.OnMouseMove(new CellContext(this, mousePosition, mouseCell), e);
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			ChangeMouseCell(Position.Empty);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			//verifico che l'eventuale edit sia terminato altrimenti esco
			if (Selection.ActivePosition.IsEmpty() == false)
			{
				CellContext focusCell = new CellContext(this, Selection.ActivePosition);
				if (focusCell.Cell != null && focusCell.IsEditing())
				{
					if (focusCell.EndEdit(false) == false)
						return;
				}
			}

			//scateno eventi di MouseDown
			Position position = PositionAtPoint(new Point(e.X, e.Y));
			if (position.IsEmpty() == false)
			{
				Cells.ICellVirtual cellMouseDown = GetCell(position);
				if (cellMouseDown != null)
				{
					ChangeMouseDownCell(position, position);

					//Cell.OnMouseDown
					CellContext cellContext = new CellContext(this, position, cellMouseDown);
					Controller.OnMouseDown(cellContext, e);
				}
			}
			else
				ChangeMouseDownCell(Position.Empty, Position.Empty);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (MouseDownPosition.IsEmpty() == false)
			{
				Cells.ICellVirtual mouseDownCell = GetCell(MouseDownPosition);
				if (mouseDownCell != null)
					Controller.OnMouseUp(new CellContext(this, MouseDownPosition, mouseDownCell), e);

				ChangeMouseDownCell(Position.Empty, PositionAtPoint(new Point(e.X, e.Y)));
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			Position clickPosition = PositionAtPoint(PointToClient(Control.MousePosition));

			//Se ho precedentemente scatenato un MouseDown su una cella
			// e se questa corrisponde alla cella sotto il puntatore del mouse (non posso usare MouseCellPosition perchè questa viene aggiornata solo quando non si ha una cella come MouseDownPosition
			if (MouseDownPosition.IsEmpty() == false &&
			    MouseDownPosition == clickPosition)
			{
				Cells.ICellVirtual mouseDownCell = GetCell(MouseDownPosition);
				if (mouseDownCell != null)
					Controller.OnClick(new CellContext(this, MouseDownPosition, mouseDownCell), e);
			}
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			// just pass up this event.
			// we do not have a method for this at the moment.
			// And it is different from MouseDoubleClick.
			base.OnDoubleClick(e);
		}
		
		private void HandleMouseDoubleClick(Position clickPosition, EventArgs e)
		{
			if (clickPosition.IsEmpty() == true)
				return;
			Cells.ICellVirtual mouseDownCell = GetCell(clickPosition);
			if (mouseDownCell != null)
				Controller.OnDoubleClick(new CellContext(this, clickPosition, mouseDownCell), e);
		}
		
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Position clickPosition = PositionAtPoint(PointToClient(Control.MousePosition));
			HandleMouseDoubleClick(clickPosition, e);
			base.OnMouseDoubleClick(e);
		}

		/// <summary>
		/// Fired when a user scroll with the mouse wheel
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);

			CustomScrollWheel(e.Delta);
		}

		#endregion

		#region Keyboard

		/// <summary>
		/// Remember the active position, when the key was pressed.
		/// Events are called in this order:
		/// KeyDown
		/// KeyPress
		/// KeyUp
		/// 
		/// During these three events, the same active position must be kept,
		/// regardless of whether it is changed in any of these events
		/// 
		/// http://msdn.microsoft.com/en-us/library/system.windows.forms.control.keydown(VS.80).aspx
		/// </summary>
		private Position m_keybordActivePosition;
		
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			m_keybordActivePosition = Selection.ActivePosition;

			if (m_keybordActivePosition.IsEmpty() == false)
			{
				Cells.ICellVirtual focusCell = GetCell(m_keybordActivePosition);
				if (focusCell != null)
					Controller.OnKeyDown(new CellContext(this, m_keybordActivePosition, focusCell), e);
			}

			if (e.Handled == false)
				ProcessSpecialGridKey(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (m_keybordActivePosition.IsEmpty() == false)
			{
				Cells.ICellVirtual focusCell = GetCell(m_keybordActivePosition);
				if (focusCell != null)
					Controller.OnKeyUp(new CellContext(this, m_keybordActivePosition, focusCell), e);
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			//Che if is not Tab, Enter or Copy/Paste
			if (m_keybordActivePosition.IsEmpty() || e.KeyChar == '\t' || e.KeyChar == 13 ||
			    e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24)
			{
			}
			else
			{
				Cells.ICellVirtual focusCell = GetCell(m_keybordActivePosition);
				if (focusCell != null)
					Controller.OnKeyPress(new CellContext(this, m_keybordActivePosition, focusCell), e);
			}
		}

		#endregion

		#region Focus
		private bool mAutoFocusFirstCellOnEnter = true;
		/// <summary>
		/// Set the Focus on the grid with an optional behaviour.
		/// If selectFirstCell is true and the FocusStyle is equal to FocusStyle.FocusFirstCellOnEnter then
		/// if there isn't a cell selected the first cell of the grid receive became the active cell.
		/// Otherwise the focus is not changed.
		/// The Default is true.
		/// </summary>
		/// <param name="selectFirstCell"></param>
		public virtual bool Focus(bool selectFirstCell)
		{
			bool ret;
			try
			{
				mAutoFocusFirstCellOnEnter = selectFirstCell;
				ret = Focus();
			}
			finally
			{
				mAutoFocusFirstCellOnEnter = true;
			}

			return ret;
		}
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			//Move the focus on the first cell if there isn't an active cell and the FocusStyle FocusFirstCellOnEnter is on.
			if ((Selection.FocusStyle & FocusStyle.FocusFirstCellOnEnter) == FocusStyle.FocusFirstCellOnEnter &&
			    mAutoFocusFirstCellOnEnter && Selection.ActivePosition.IsEmpty())
			{
				Selection.FocusFirstCell(false);
			}
		}
		protected override void OnValidated(EventArgs e)
		{
			base.OnValidated(e);

			//NOTE: I use OnValidated and not OnLostFocus because is not called when the focus is on another child control (for example an editor control) or OnLeave because before Validating event and so the validation can still be stopped

			if ((Selection.FocusStyle & FocusStyle.RemoveFocusCellOnLeave) == FocusStyle.RemoveFocusCellOnLeave)
			{
				Selection.Focus(Position.Empty, false);
			}

			if ((Selection.FocusStyle & FocusStyle.RemoveSelectionOnLeave) == FocusStyle.RemoveSelectionOnLeave)
			{
				Selection.ResetSelection(true);
			}
		}

		#endregion

		#region Controls linked
		private LinkedControlsList m_LinkedControls;

		/// <summary>
		/// List of controls that are linked to a specific cell position.
		/// For example is used for editors controls. Key=Control, Value=Position.
		/// The controls are automatically removed from the list when they are removed from the Grid.Controls
		/// collection
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LinkedControlsList LinkedControls
		{
			get{return m_LinkedControls;}
		}

		/// <summary>
		/// OnHScrollPositionChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHScrollPositionChanged(ScrollPositionChangedEventArgs e)
		{
			base.OnHScrollPositionChanged (e);

			ArrangeLinkedControls();
		}

		/// <summary>
		/// OnVScrollPositionChanged
		/// </summary>
		/// <param name="e"></param>
		protected override void OnVScrollPositionChanged(ScrollPositionChangedEventArgs e)
		{
			base.OnVScrollPositionChanged (e);

			ArrangeLinkedControls();
		}

		/// <summary>
		/// Refresh the linked controls bounds
		/// </summary>
		public virtual void ArrangeLinkedControls()
		{
			SuspendLayout();
			foreach (LinkedControlValue linked in m_LinkedControls)
			{
				if (linked.Position.IsEmpty())
					continue;

				Control control = linked.Control;
				Cells.ICellVirtual cell = GetCell(linked.Position);

				Rectangle rect = PositionToRectangle(linked.Position);

				if (cell != null && linked.UseCellBorder)
					rect = Rectangle.Round(cell.View.Border.GetContentRectangle(rect));

				control.Bounds = rect;
				
				// Hide all linked controls which are not in the fixed area
				// and fall outside the scrollable area
				if ( ( linked.Position.Row >= FixedRows ) && ( linked.Position.Column >= FixedColumns ) )
				{
					Range scrollableRange = RangeAtArea(CellPositionType.Scrollable );
					Rectangle scrollableRectangle = RangeToRectangle(scrollableRange);

					control.Visible = !Rectangle.Intersect( rect, scrollableRectangle ).IsEmpty;
				}
			}
			ResumeLayout(false);
		}

		#endregion

		#region Layout
		/// <summary>
		/// Recalculate the scrollbar position and value based on the current cells, scroll client area, linked controls and more. If redraw == false this method has not effect.
		/// </summary>
		private void PerformStretch()
		{
			if (AutoStretchColumnsToFitWidth || AutoStretchRowsToFitHeight)
			{
				if (AutoStretchColumnsToFitWidth && AutoStretchRowsToFitHeight)
				{
					Rows.AutoSize(false);
					Columns.AutoSize(false);
					Columns.StretchToFit();
					Rows.StretchToFit();
				}
				else if (AutoStretchColumnsToFitWidth)
				{
					Columns.AutoSize(true);
					Columns.StretchToFit();
				}
				else if (AutoStretchRowsToFitHeight)
				{
					Rows.AutoSize(true);
					Rows.StretchToFit();
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			SuspendLayout();
			SetScrollArea();
			PerformStretch();
			ResumeLayout(true);
		}


		/// <summary>
		/// Force to recalculate scrollbars and panels location. Used usually after changing width and height of the columns / rows, or after adding or removing rows and columns.
		/// </summary>
		public virtual void OnCellsAreaChanged()
		{
			SuspendLayout();
			SetScrollArea();
			CheckPositions();
			ArrangeLinkedControls();
			ResumeLayout(true);
		}

		private void SetScrollArea()
		{
			//MICK(19): original version is obviously a bug - when vertPage is calculated in this manner
			//the thumb height is not right
			//and scrolling is not continuous (not all items are displayed when scrolling)
			//ORIG: int vertPage = Math.Max(Rows.Count / 10, 1);
			//ORIG: int horPage = Math.Max(Columns.Count / 10, 1);

			//MICK(20)
			int vertPage = Math.Max(GetVisibleRows(false).Count - ActualFixedRows, 1);
			int horPage = Math.Max(GetVisibleColumns(false).Count - ActualFixedColumns, 1);


			LoadScrollArea(vertPage, horPage);
		}
		#endregion

		#region Sort Range
		
		/// <summary>
		/// Tells if grid has sorting enabled.
		/// Note that you can still call manually functions to sort data,
		/// such as SortRangeRows, or even sort the data
		/// in your own data source. This property is for UI only
		/// </summary>
		public abstract bool EnableSort {get;set;}
		
		/// <summary>
		/// Sort a range of the grid
		/// </summary>
		/// <param name="p_RangeToSort">Range to sort</param>
		/// <param name="keyColumn">Index of the column relative to the grid to use as sort keys, must be between start and end col of the range</param>
		/// <param name="p_bAsc">Ascending true, Descending false</param>
		/// <param name="p_CellComparer">CellComparer, if null the default comparer will be used</param>
		public void SortRangeRows(IRangeLoader p_RangeToSort,
		                          int keyColumn,
		                          bool p_bAsc,
		                          IComparer p_CellComparer)
		{
			Range l_Range = p_RangeToSort.GetRange(this);
			SortRangeRows(l_Range, keyColumn, p_bAsc, p_CellComparer);
		}

		/// <summary>
		/// Sort a range of the grid.
		/// </summary>
		/// <param name="p_Range"></param>
		/// <param name="keyColumn">Index of the column relative to the grid to use as sort keys, must be between start and end col</param>
		/// <param name="p_bAscending">Ascending true, Descending false</param>
		/// <param name="p_CellComparer">CellComparer, if null the default ValueCellComparer comparer will be used</param>
		public void SortRangeRows(Range p_Range,
		                          int keyColumn,
		                          bool p_bAscending,
		                          IComparer p_CellComparer)
		{
			SortRangeRowsEventArgs eventArgs = new SortRangeRowsEventArgs(p_Range, keyColumn, p_bAscending, p_CellComparer);

			if (SortingRangeRows!=null)
				SortingRangeRows(this, eventArgs);

			OnSortingRangeRows(eventArgs);

			if (SortedRangeRows!=null)
				SortedRangeRows(this, eventArgs);

			OnSortedRangeRows(eventArgs);
		}

		/// <summary>
		/// Fired when calling SortRangeRows method
		/// </summary>
		[Browsable(true)]
		public event SortRangeRowsEventHandler SortingRangeRows;

		/// <summary>
		/// Fired after calling SortRangeRows method
		/// </summary>
		[Browsable(true)]
		public event SortRangeRowsEventHandler SortedRangeRows;

		/// <summary>
		/// Fired when calling SortRangeRows method
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSortingRangeRows(SortRangeRowsEventArgs e)
		{
		}
		/// <summary>
		/// Fired after calling SortRangeRows method
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSortedRangeRows(SortRangeRowsEventArgs e)
		{
		}
		#endregion

		#region GetCell methods

		/// <summary>
		/// Return the Cell at the specified Row and Col position. Abstract, must be implemented in the derived class.
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <returns></returns>
		public abstract Cells.ICellVirtual GetCell(int p_iRow, int p_iCol);

		/// <summary>
		/// Return the Cell at the specified Row and Col position. This method is called for sort operations and for Move operations. If position is Empty return null. This method calls GetCell(int p_iRow, int p_iCol)
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public Cells.ICellVirtual GetCell(Position p_Position)
		{
			if (p_Position.IsEmpty())
				return null;
			else
				return GetCell(p_Position.Row, p_Position.Column);
		}

		/// <summary>
		/// Returns all the cells at specified row position
		/// </summary>
		/// <param name="p_RowIndex"></param>
		/// <returns></returns>
		public virtual Cells.ICellVirtual[] GetCellsAtRow(int p_RowIndex)
		{
			Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[Columns.Count];
			for (int c = 0; c < Columns.Count; c++)
				l_Cells[c] = GetCell(p_RowIndex, c);

			return l_Cells;
		}

		/// <summary>
		/// Returns all the cells at specified column position
		/// </summary>
		/// <param name="p_ColumnIndex"></param>
		/// <returns></returns>
		public virtual Cells.ICellVirtual[] GetCellsAtColumn(int p_ColumnIndex)
		{
			Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[Rows.Count];
			for (int r = 0; r < Rows.Count; r++)
				l_Cells[r] = GetCell(r, p_ColumnIndex);

			return l_Cells;
		}

		#endregion

		#region Rows, Columns
		private int m_FixedRows = 0;
		/// <summary>
		/// Gets or Sets how many rows are not scrollable
		/// </summary>
		[DefaultValue(0)]
		public int FixedRows
		{
			get{return m_FixedRows;}
			set
			{
				if (m_FixedRows != value)
				{
					m_FixedRows = value;
					OnCellsAreaChanged();
				}
			}
		}
		public int ActualFixedRows
		{
			get
			{
				if (FixedRows > Rows.Count)
					return Rows.Count;
				else
					return FixedRows;
			}
		}

		private int m_FixedCols = 0;
		/// <summary>
		/// Gets or Sets how many cols are not scrollable
		/// </summary>
		[DefaultValue(0)]
		public int FixedColumns
		{
			get{return m_FixedCols;}
			set
			{
				if (m_FixedCols !=  value)
				{
					m_FixedCols = value;
					OnCellsAreaChanged();
				}
			}
		}
		public int ActualFixedColumns
		{
			get
			{
				if (FixedColumns > Columns.Count)
					return Columns.Count;
				else
					return FixedColumns;
			}
		}

		
		private RowsBase m_Rows;

		/// <summary>
		/// RowsCount informations
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RowsBase Rows
		{
			get{return m_Rows;}
		}

		private ColumnsBase m_Columns;

		/// <summary>
		/// Columns informations
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ColumnsBase Columns
		{
			get{return m_Columns;}
		}

		/// <summary>
		/// Returns the type of a cell position
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public CellPositionType GetPositionType(Position position)
		{
			if (position.IsEmpty())
				return CellPositionType.Empty;
			else if (position.Row < FixedRows && position.Column < FixedColumns)
				return CellPositionType.FixedTopLeft;
			else if (position.Row < FixedRows)
				return CellPositionType.FixedTop;
			else if (position.Column < FixedColumns)
				return CellPositionType.FixedLeft;
			else
				return CellPositionType.Scrollable;
		}

		/// <summary>
		/// Returns a Range that represents the complete cells of the grid
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Range CompleteRange
		{
			get
			{
				if (Rows.Count > 0 && Columns.Count > 0)
					return new Range(0, 0, Rows.Count - 1, Columns.Count - 1);
				else
					return Range.Empty;
			}
		}
		#endregion

		#region Exception
		/// <summary>
		/// Event fired when an exception is throw in some method that require a notification to the user.
		/// </summary>
		public event ExceptionEventHandler UserException;

		/// <summary>
		/// Event fired when an exception is throw in some method that require a notification to the user.
		/// If not handled by the user (Handled property = false) a MessageBox is used to display the exception.
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnUserException(ExceptionEventArgs e)
		{
			#if DEBUG
			System.Diagnostics.Debug.WriteLine("Exception on editing cell: " + e.Exception.ToString());
			#endif

			if (UserException!=null)
				UserException(this, e);

			if (e.Handled == false)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this, e.Exception, "Error");
				e.Handled = true;
			}
		}
		#endregion

		#region Right To Left Support
		//With Visual Studio 2005 probably I can use the new RightToLeftLayout and TextRenderer class
		// se also
//		http://www.microsoft.com/middleeast/msdn/visualstudio2005.aspx
//		http://www.microsoft.com/middleeast/msdn/mirror.aspx
		//      http://msdn2.microsoft.com/en-us/library/fh376txk.aspx
		//      http://msdn2.microsoft.com/en-us/library/7d3337xw.aspx
		//      "How to: Create Mirrored Windows Forms and Controls"  http://msdn2.microsoft.com/en-us/library/xwbz5ws0.aspx

		const int WS_EX_LAYOUTRTL = 0x400000;
		const int WS_EX_NOINHERITLAYOUT = 0x100000;
		private bool m_Mirrored = false;

		[Description("Change the right-to-left layout."), DefaultValue(false),
		 Localizable(true), Category("Appearance"), Browsable(true)]
		public bool Mirrored
		{
			get
			{
				return m_Mirrored;
			}
			set
			{
				if (m_Mirrored != value)
				{
					m_Mirrored = value;
					base.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams CP;
				CP = base.CreateParams;
				if (Mirrored)
					CP.ExStyle = CP.ExStyle | WS_EX_LAYOUTRTL;
				return CP;
			}
		}

		/// <summary>
		/// Hide the RightToLeft property and returns always No.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(RightToLeft.No)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override RightToLeft RightToLeft
		{
			get { return RightToLeft.No; }
			set { }
		}
		#endregion

		#region Clear
		/// <summary>
		/// Clear all the selected cells with a valid Model.
		/// </summary>
		public virtual void ClearValues(RangeRegion region)
		{
			PositionCollection positions = region.GetCellsPositions();
			foreach (Position pos in positions)
			{
				CellContext cellContext = new CellContext(this, pos);
				if (cellContext.Cell != null)
				{
					if (cellContext.Cell.Editor != null)
						cellContext.Cell.Editor.ClearCell(cellContext);
				}
			}
		}
		#endregion

		#region ToolTip
		private System.Windows.Forms.ToolTip toolTip;
		/// <summary>
		/// ToolTip text
		/// </summary>
		public string ToolTipText
		{
			get { return toolTip.GetToolTip(this); }
			set { toolTip.SetToolTip(this, value); }
		}

		/// <summary>
		/// Gets the tooltip control used when showing tooltip text.
		/// </summary>
		public ToolTip ToolTip
		{
			get { return toolTip; }
		}
		#endregion

		#region Decorator List
		private Decorators.DecoratorList mDecorators = new Decorators.DecoratorList();
		/// <summary>
		/// Grid decorators, used to draw additiona contents on the drawing surface
		/// </summary>
		public Decorators.DecoratorList Decorators
		{
			get { return mDecorators; }
		}
		#endregion

		#region Controllers
		private Cells.Controllers.ControllerContainer mController = new SourceGrid.Cells.Controllers.ControllerContainer();
		public Cells.Controllers.ControllerContainer Controller
		{
			get { return mController; }
		}
		#endregion

		#region Clipboard
		private ClipboardMode mClipboardMode = ClipboardMode.None;

		/// <summary>
		/// Gets or sets the clipboard operation mode. Default is none
		/// </summary>
		[DefaultValue(ClipboardMode.None)]
		public ClipboardMode ClipboardMode
		{
			get { return mClipboardMode; }
			set { mClipboardMode = value; }
		}

		private bool mClipboardUseOnlyActivePosition = false;

		/// <summary>
		/// Gets or sets if only active cell should be used for clipboard operation. Default is whole selection
		/// </summary>
		[DefaultValue(false)]
		public bool ClipboardUseOnlyActivePosition
		{
			get { return mClipboardUseOnlyActivePosition; }
			set { mClipboardUseOnlyActivePosition = value; }
		}
		#endregion

		#region MICK(1)
		#region Scroll PageDown/Up/Right/Left/LineUp/Down/Right/Left

		#region NewScrollDown
		public override void CustomScrollPageDown()
		{
			bool NeedActualScrolling=false;
			int NewProposedTopRow=0;
			int NewProposedFocusedRow=0;

			Range rngFocusedCell = PositionToCellRange(Selection.ActivePosition);
			
			//if there is no selection here (ActiveRow is undefined),
			//put selection in the first possible cell
			//confusing NAMING: Rows.IsRowVisible -> Rows.NotHidden - because it is not hidden,
			//                  may be confused with visible on screen within visible page

			if (rngFocusedCell.IsEmpty())
			{
				Selection.MoveActiveCell(1, 0);
				return;
			}

			List<int> rows = GetVisibleRows(false);

			if (rows.Count <= ActualFixedRows + 1)
			{
				Selection.MoveActiveCell(1, 0);
				return;
			};
			
			int firstVisibleRow = rows[ActualFixedRows];
			int lastVisibleRow = rows[rows.Count - 1];



			if (IsThisRowVisibleOnScreen(rngFocusedCell.Start.Row))
			{   //focus is on the screen

				int BottommostFocusableRow = GetBottommostFocusableRowFromRange(firstVisibleRow, lastVisibleRow);
				if (rngFocusedCell.ContainsRow(BottommostFocusableRow))
				{   //need actual scrolling, ie. displaying new page
					int FindFirstFocusableRow = FindFirstFocusableRowInDownDirection(lastVisibleRow);
					if (FindFirstFocusableRow != -1)
					{
						NeedActualScrolling = true;
						NewProposedTopRow = lastVisibleRow;
						int NewProposedBottomRow = GetBottomVisibleRowFromTopRow(NewProposedTopRow);
						NewProposedFocusedRow = GetBottommostFocusableRowFromRange(NewProposedTopRow, NewProposedBottomRow);

						if (NewProposedFocusedRow == -1)
						{//There is no Focusable Row in the proposed page, so there need to be new page and focus found
							NewProposedFocusedRow = FindFirstFocusableRowInDownDirection(NewProposedBottomRow);
							NewProposedTopRow = GetTopVisibleRowFromBottomRow(NewProposedFocusedRow);
						}
					}
					else
					{   //no focusable cells below -
						//in this case we might scroll down page, leaving focus unchanged
						//or move focus to the side like in arrow down handler
						
						//this is only temporary
						//MICK(23)
						Selection.MoveActiveCell(1, 0);
						return;
					}
				}
				else
				{   //this time only move focus, wihout actual scrolling

					Position newPosition = new Position(BottommostFocusableRow, Selection.ActivePosition.Column);
					Selection.Focus(newPosition, true);
				}

			}
			else
			{// focus is not on the screen, so actual scrolling is done always
				NeedActualScrolling = true;
				NewProposedTopRow = rngFocusedCell.Start.Row;
				int NewProposedBottomRow = GetBottomVisibleRowFromTopRow(NewProposedTopRow);
				NewProposedFocusedRow = GetBottommostFocusableRowFromRange(NewProposedTopRow, NewProposedBottomRow);
			}

			if (NeedActualScrolling)
			{
				int NewTopRow=0;
				int NewFocusedRow=0;

				GetValidScrollDown(NewProposedTopRow, NewProposedFocusedRow, out NewTopRow, out NewFocusedRow);
				NewTopRow = ExtendTopRowByHiddenRows(NewTopRow);

				base.CustomScrollPageToLine(NewTopRow - ActualFixedRows);

				if (rngFocusedCell.ContainsRow(NewFocusedRow) == false)
				{
					Position newPosition = new Position(NewFocusedRow, Selection.ActivePosition.Column);
					Selection.Focus(newPosition, true);
				}

			}

		}

		int ExtendTopRowByHiddenRows(int TopRow)
		{//Simple (though not the best) way to handle hidden rows

			if (TopRow == 0) return 0;
			for (int k = TopRow; k >= ActualFixedRows; k--)
			{
				if (Rows.IsRowVisible(k)) return k;
			}
			return ActualFixedRows;
		}
		int FindFirstFocusableRowInDownDirection(int TopRow)
		{
			for (int k = TopRow; k < Rows.Count; k++)
			{
				Position newPosition = new Position(k, Selection.ActivePosition.Column);
				if (Selection.CanReceiveFocus(newPosition)) return k;
			}
			return -1;
		}
		int GetBottommostFocusableRowFromRange(int firstRow, int lastRow)
		{
			for (int k = lastRow; k >= firstRow; k--)
			{
				Position newPosition = new Position(k, Selection.ActivePosition.Column);
				if (Selection.CanReceiveFocus(newPosition)) return k;
			}
			return -1;
		}
		bool IsThisRowVisibleOnScreen(int Row)
		{
			List<int> rows = GetVisibleRows(false);
			int firstVisibleRow = rows[ActualFixedRows];
			int lastVisibleRow = rows[rows.Count - 1];
			if ((Row <= lastVisibleRow) && (Row >= firstVisibleRow)) return true;
			else return false;
		}
		int GetTopVisibleRowFromBottomRow(int BottomRow)
		{
			int displayHeight = DisplayRectangle.Height;
			int currentHeight = 0;
			//int CurrentRows = 0;

			//Remove the fixed rows from the scrollable area
			for (int f = 0; f < ActualFixedRows; f++)
				displayHeight -= Rows.GetHeight(f);

			int r = 0;
			for (r = BottomRow; r >= ActualFixedRows; r--)
			{
				currentHeight += Rows.GetHeight(r);

				if (currentHeight == displayHeight)
				{
					return r;
				}
				if (currentHeight > displayHeight)
				{
					if (r == BottomRow) return r; else return r + 1;
				}

			}
			return 0;
		}
		int GetBottomVisibleRowFromTopRow(int TopRow)
		{
			int displayHeight = DisplayRectangle.Height;
			int currentHeight = 0;
			//int CurrentRows = 0;

			//Remove the fixed rows from the scrollable area
			for (int f = 0; f < ActualFixedRows; f++)
				displayHeight -= Rows.GetHeight(f);

			int r = 0;
			for (r = TopRow; r < Rows.Count; r++)
			{
				currentHeight += Rows.GetHeight(r);

				if (currentHeight == displayHeight)
				{
					return r;
				}
				if (currentHeight > displayHeight)
				{
					if (r == TopRow) return r; else return r - 1;
				}

			}
			return Rows.Count - 1;
		}
		void GetValidScrollDown(int NewProposedTopRow, int NewProposedFocusedRow, out int NewTopRow, out int NewFocusedRow)
		{   //if scrool down is too far down, it needs to be corrected
			NewTopRow=NewProposedTopRow;
			NewFocusedRow = NewProposedFocusedRow;

			int ValidLastTopRow = GetTopVisibleRowFromBottomRow(Rows.Count - 1);

			if (NewProposedTopRow > ValidLastTopRow)
				NewTopRow = ValidLastTopRow;

			if (NewProposedFocusedRow > Rows.Count - 1)
				NewFocusedRow = Rows.Count - 1;
		}
		#endregion




		#region NewScrollUp
		public override void CustomScrollPageUp()
		{
			bool NeedActualScrolling=false;
			int NewProposedTopRow=0;
			int NewProposedFocusedRow=0;

			//confusing NAMING: ActivePosition -> FocusedPosition - it would be better
			int FocusedRow = Selection.ActivePosition.Row;
			//TODO: if there is no selection here (ActiveRow is undefined),
			//put selection in the first possible cell
			//confusing NAMING: Rows.IsRowVisible -> Rows.NotHidden - because it is not hidden,
			//                  may be confused with visible on screen within visible page

			
			if (FocusedRow == -1)
			{
				Selection.MoveActiveCell(-1, 0);
				return;
			}


			List<int> rows = GetVisibleRows(false);

			//TODO: if Rows.height<displayheight
			if (rows.Count <= ActualFixedRows + 1)
			{
				Selection.MoveActiveCell(-1, 0);
				return;
			};
			
			int firstVisibleRow = rows[ActualFixedRows];
			int lastVisibleRow = rows[rows.Count - 1];

			//another workaround: if (focusedrow==firstVisibleRow)
			//if the row above has height larger than displayheight it is also like one row scrolling-
			//so workaround is needed like the one above
			if (FocusedRow == firstVisibleRow)
				if (FocusedRow>ActualFixedRows)
			{
				int displayHeight = DisplayRectangle.Height;

				//Remove the fixed rows from the scrollable area
				for (int f = 0; f < ActualFixedRows; f++)
					displayHeight -= Rows.GetHeight(f);

				if (displayHeight <= Rows.GetHeight(FocusedRow - 1))
				{
					Selection.MoveActiveCell(-1, 0);
					return;
				}
			}



			if (IsThisRowVisibleOnScreen(FocusedRow))
			{   //focus is on the screen

				int TopmostFocusableRow = GetTopmostFocusableRowFromRange(firstVisibleRow, lastVisibleRow);
				if (TopmostFocusableRow == FocusedRow)
				{   //need actual scrolling, ie. displaying new page
					int FindFirstFocusableRow = FindFirstFocusableRowInUpDirection(firstVisibleRow);
					if (FindFirstFocusableRow != -1)
					{
						NeedActualScrolling = true;
						int NewProposedBottomRow = firstVisibleRow;
						NewProposedTopRow = GetTopVisibleRowFromBottomRow(NewProposedBottomRow);
						NewProposedFocusedRow = GetTopmostFocusableRowFromRange(NewProposedTopRow, NewProposedBottomRow);

						if (NewProposedFocusedRow == -1)
						{//There is no Focusable Row in the proposed page, so there need to be new page and focus found
							NewProposedFocusedRow = FindFirstFocusableRowInUpDirection(NewProposedTopRow);
							NewProposedTopRow = NewProposedFocusedRow;
						}
					}
					else
					{   //no focusable cells below -
						//in this case we might scroll down page, leaving focus unchanged
						//or move focus to the side like in arrow down handler
						
						//this is only temporary
						//MICK(24)
						Selection.MoveActiveCell(-1, 0);
						return;
					}
				}
				else
				{   //this time only move focus, wihout actual scrolling

					Position newPosition = new Position(TopmostFocusableRow, Selection.ActivePosition.Column);
					Selection.Focus(newPosition, true);
				}

			}
			else
			{// focus is not on the screen, so actual scrolling is done always
				NeedActualScrolling = true;

				int NewProposedBottomRow = FocusedRow;
				NewProposedTopRow = GetTopVisibleRowFromBottomRow(NewProposedBottomRow);
				NewProposedFocusedRow = GetTopmostFocusableRowFromRange(NewProposedTopRow, NewProposedBottomRow);

				//NewProposedTopRow = FocusedRow;
				//NewProposedFocusedRow = GetBottomVisibleRowFromTopRow(NewProposedTopRow);
			}

			if (NeedActualScrolling)
			{
				int NewTopRow=0;
				int NewFocusedRow=0;

				GetValidScrollUp(NewProposedTopRow, NewProposedFocusedRow, out NewTopRow, out NewFocusedRow);
				NewTopRow = ExtendTopRowByHiddenRows(NewTopRow);

				base.CustomScrollPageToLine(NewTopRow - ActualFixedRows);

				if (NewFocusedRow != FocusedRow)
				{
					Position newPosition = new Position(NewFocusedRow, Selection.ActivePosition.Column);
					Selection.Focus(newPosition, true);
				}

			}

		}

		int FindFirstFocusableRowInUpDirection(int BottomRow)
		{
			for (int k = BottomRow; k > ActualFixedRows; k--)
			{
				Position newPosition = new Position(k, Selection.ActivePosition.Column);
				if (Selection.CanReceiveFocus(newPosition)) return k;
			}
			return -1;
		}
		int GetTopmostFocusableRowFromRange(int firstRow, int lastRow)
		{
			for (int k = firstRow; k <= lastRow; k++)
			{
				Position newPosition = new Position(k, Selection.ActivePosition.Column);
				if (Selection.CanReceiveFocus(newPosition)) return k;
			}
			return -1;
		}
		void GetValidScrollUp(int NewProposedTopRow, int NewProposedFocusedRow, out int NewTopRow, out int NewFocusedRow)
		{   //if scrool down is too far down, it needs to be corrected
			NewTopRow = NewProposedTopRow;
			NewFocusedRow = NewProposedFocusedRow;

			int ValidFirstTopRow = ActualFixedRows;

			if (NewProposedTopRow < ValidFirstTopRow)
				NewTopRow = ValidFirstTopRow;

			if (NewProposedFocusedRow < ValidFirstTopRow)
				NewFocusedRow = ValidFirstTopRow;
		}


		#endregion
		
		#endregion

		#endregion
	}
}
