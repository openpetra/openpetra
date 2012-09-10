using System;
using System.Drawing;

namespace SourceGrid.Selection
{
	/// <summary>
	/// Base selection class
	/// </summary>
	public abstract class SelectionBase : IGridSelection
	{
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public SelectionBase()
		{
		}
		#endregion

		#region Grid
		/// <summary>
		/// Link the cell at the specified grid.
		/// For internal use only.
		/// </summary>
		/// <param name="p_grid"></param>
		public virtual void BindToGrid(GridVirtual p_grid)
		{
			mGrid = p_grid;
		}

		/// <summary>
		/// Remove the link of the cell from the grid.
		/// For internal use only.
		/// </summary>
		public virtual void UnBindToGrid()
		{
			mGrid = null;
		}

		private GridVirtual mGrid;
		/// <summary>
		/// The Grid object
		/// </summary>
		public GridVirtual Grid
		{
			get { return mGrid; }
		}
		#endregion

		#region Focus
		private Position m_ActivePosition = Position.Empty;

		/// <summary>
		/// Gets the active cell position. The cell with the focus.
		/// Returns Position.Empty if there isn't an active cell.
		/// </summary>
		public Position ActivePosition
		{
			get
			{
				return m_ActivePosition;
			}
			protected set 
			{
				m_ActivePosition = value;
			}
		}

		private bool IsActivePositionValid()
		{
			return ActivePosition.IsEmpty() == false &&
				Grid != null &&
				Grid.CompleteRange.Contains(ActivePosition);
		}

		/// <summary>
		/// Change the ActivePosition (focus) of the grid.
		/// </summary>
		/// <param name="pCellToActivate"></param>
		/// <param name="pResetSelection">True to deselect the previous selected cells</param>
		/// <returns></returns>
		public bool Focus(Position pCellToActivate, bool pResetSelection)
		{
			//If control key is pressed, enableMultiSelection is true and the cell that will receive the focus is not empty leave the cell selected otherwise deselect other cells
			bool deselectOtherCells = false;
			if (pCellToActivate.IsEmpty() == false && pResetSelection)
				deselectOtherCells = true;

			//pCellToActivate = Grid.PositionToStartPosition(pCellToActivate);

			//Check to see if the value is changed (note that I use the internal variable to see the actual stored value)
			if (pCellToActivate != ActivePosition)
			{
				//GotFocus Event Arguments
				Cells.ICellVirtual newCellToFocus = Grid.GetCell(pCellToActivate);
				CellContext newCellContext = new CellContext(Grid, pCellToActivate, newCellToFocus);
				ChangeActivePositionEventArgs gotFocusEventArgs = new ChangeActivePositionEventArgs(ActivePosition, pCellToActivate);

				if (newCellToFocus != null)
				{
					//Cell Focus Entering
					Grid.Controller.OnFocusEntering(newCellContext, gotFocusEventArgs);
					if (gotFocusEventArgs.Cancel)
						return false;

					//If the cell can't receive the focus stop the focus operation
					if (Grid.Controller.CanReceiveFocus(newCellContext, gotFocusEventArgs) == false)
						return false;
				}

				//If the new cell is valid I check if I can move the focus inside the grid
				if (newCellToFocus != null)
				{
					//This method cause any cell editor to leave the focus if the validation is ok, otherwise returns false.
					// This is useful for 2 reason:
					//	-To validate the editor
					//	-To check if I can move the focus on another cell
					bool canFocus = Grid.Focus(false);
					if (canFocus == false)
						return false;
				}

				RangeRegion oldFocusRegion = null;

				//If there is a cell with the focus fire the focus leave events
				if (IsActivePositionValid())
				{
					oldFocusRegion = new RangeRegion(ActivePosition);

					//LostFocus Event Arguments
					Cells.ICellVirtual oldCellFocus = Grid.GetCell(ActivePosition);
					CellContext oldCellContext = new CellContext(Grid, ActivePosition, oldCellFocus);
					ChangeActivePositionEventArgs lostFocusEventArgs = new ChangeActivePositionEventArgs(ActivePosition, pCellToActivate);

					//Cell Focus Leaving
					Grid.Controller.OnFocusLeaving(oldCellContext, lostFocusEventArgs);
					if (lostFocusEventArgs.Cancel)
						return false;

					//Cell Lost Focus
					OnCellLostFocus(lostFocusEventArgs);
					if (lostFocusEventArgs.Cancel)
						return false;
				}
				else
				{
					//Reset anyway the actual value. This can happen when there is an ActivePosition but it is not more valid (outside the valid range maybe when removing some cells)
					// NOTE: in this case the focus event are not executed
					m_ActivePosition = Position.Empty;
				}

				//Deselect previous selected cells
				if (deselectOtherCells)
					ResetSelection(false);

				bool success;
				if (newCellToFocus != null)
				{
					//Cell Got Focus
					OnCellGotFocus(gotFocusEventArgs);

					success = (!gotFocusEventArgs.Cancel);
				}
				else
				{
					success = true;
				}

				//Fire a change event
				RangeRegion newFocusRegion = new RangeRegion(pCellToActivate);
				OnSelectionChanged(new RangeRegionChangedEventArgs(newFocusRegion, oldFocusRegion));

				return success;
			}
			else
			{
				if (pCellToActivate.IsEmpty() == false)
				{
					DeselectOldRangeAndSelectActiveCell();
					
					//I check if the grid still has the focus, otherwise I force it
					if (Grid.ContainsFocus)
						return true;
					else
						return Grid.Focus();
				}
				else
					return true;
			}
		}
		
		private void DeselectOldRangeAndSelectActiveCell()
		{
			//Deselect previous selected cells
			//ResetSelection(true);
			//Select the cell
			SelectCell(m_ActivePosition, true);

			//Invalidate the selection
			Invalidate();
		}
		

		/// <summary>
		/// Set the focus on the first available cells starting from the not fixed cells.
		/// If there is an active selection set the focus on the first selected cells.
		/// </summary>
		/// <param name="pResetSelection"></param>
		/// <returns></returns>
		public virtual bool FocusFirstCell(bool pResetSelection)
		{
			Position focusPos;

			PositionCollection selectedPos = GetSelectionRegion().GetCellsPositions();
			//Check if there is a valid selection
			focusPos = SearchForValidFocusPosition(selectedPos);
			if (focusPos.IsEmpty())
				focusPos = SearchForValidFocusPosition();

			if (focusPos.IsEmpty() == false)
				return Focus(focusPos, pResetSelection);
			else
				return false;
		}

		/// <summary>
		/// Move the Focus to the first cell that can receive the focus of the current column otherwise put the focus to null.
		/// </summary>
		/// <returns></returns>
		public bool FocusColumn(int column)
		{
			if (Grid.Columns.Count > column)
			{
				for (int r = 0; r < Grid.Rows.Count; r++)
				{
					Position newFocus = new Position(r, column);

					if (Grid.Controller.CanReceiveFocus(new CellContext(Grid, newFocus), EventArgs.Empty))
						return Focus(newFocus, true);
				}

				return Focus(Position.Empty, true);
			}
			else
				return Focus(Position.Empty, true);
		}

		/// <summary>
		/// Move the Focus to the first cell that can receive the focus of the current row otherwise put the focus to null.
		/// </summary>
		/// <returns></returns>
		public bool FocusRow(int row)
		{
			if (Grid.Rows.Count > row)
			{
				for (int c = 0; c < Grid.Columns.Count; c++)
				{
					Position newFocus = new Position(row, c);

					if (Grid.Controller.CanReceiveFocus(new CellContext(Grid, newFocus), EventArgs.Empty))
						return Focus(newFocus, true);
				}

				return Focus(Position.Empty, true);
			}
			else
				return Focus(Position.Empty, true);
		}

		private Position SearchForValidFocusPosition(PositionCollection positions)
		{
			foreach (Position ps in positions)
			{
				if (CanReceiveFocus(ps))
					return ps;
			}

			return Position.Empty;
		}

		private Position SearchForValidFocusPosition()
		{
			for (int r = Grid.FixedRows; r < Grid.Rows.Count; r++)
			{
				for (int c = Grid.FixedColumns; c < Grid.Columns.Count; c++)
				{
					Position startPosition = new Position(r, c);
					if (CanReceiveFocus(startPosition))
						return startPosition;
				}
			}

			return Position.Empty;
		}

		private Color m_FocusBackColor = Color.Transparent;
		/// <summary>
		/// Gets or sets the backColor of the cell with the Focus. Default is Color.Transparent.
		/// </summary>
		public Color FocusBackColor
		{
			get { return m_FocusBackColor; }
			set { m_FocusBackColor = value; Invalidate(); }
		}

		private FocusStyle m_FocusStyle = FocusStyle.Default;

		/// <summary>
		/// Gets or sets the behavior of the focus and selection. Default is FocusStyle.Default.
		/// </summary>
		public FocusStyle FocusStyle
		{
			get { return m_FocusStyle; }
			set { m_FocusStyle = value; }
		}

		/// <summary>
		/// Returns true if the specified position can receive the focus.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public bool CanReceiveFocus(Position position)
		{
			if (Grid.CompleteRange.Contains(position))
			{
				Cells.ICellVirtual cell = Grid.GetCell(position);
				if (cell != null)
				{
					CellContext context = new CellContext(Grid, position, cell);
					if (Grid.Controller.CanReceiveFocus(context, EventArgs.Empty))
						return true;
					else
						return false;
				}
				else
					return false;
			}
			else
				return false;
		}

		#region Focus Events
		/// <summary>
		/// Fired before a cell receive the focus (FocusCell is populated after this event, use e.Cell to read the cell that will receive the focus)
		/// </summary>
		public event ChangeActivePositionEventHandler CellGotFocus;

		/// <summary>
		/// Fired before a cell lost the focus
		/// </summary>
		public event ChangeActivePositionEventHandler CellLostFocus;

		/// <summary>
		/// Fired before a row lost the focus
		/// </summary>
		public event RowCancelEventHandler FocusRowLeaving;
		/// <summary>
		/// Fired after a row receive the focus
		/// </summary>
		public event RowEventHandler FocusRowEntered;
		/// <summary>
		/// Fired before a column lost the focus
		/// </summary>
		public event ColumnCancelEventHandler FocusColumnLeaving;
		/// <summary>
		/// Fired after a column receive the focus
		/// </summary>
		public event ColumnEventHandler FocusColumnEntered;

		/// <summary>
		/// Fired before a row lost the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFocusRowLeaving(RowCancelEventArgs e)
		{
			if (FocusRowLeaving != null)
				FocusRowLeaving(this, e);
		}
		/// <summary>
		/// Fired after a row receive the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFocusRowEntered(RowEventArgs e)
		{
			if (FocusRowEntered != null)
				FocusRowEntered(this, e);
		}
		/// <summary>
		/// Fired before a column lost the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFocusColumnLeaving(ColumnCancelEventArgs e)
		{
			if (FocusColumnLeaving != null)
				FocusColumnLeaving(this, e);
		}
		/// <summary>
		/// Fired after a column receive the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFocusColumnEntered(ColumnEventArgs e)
		{
			if (FocusColumnEntered != null)
				FocusColumnEntered(this, e);
		}

		/// <summary>
		/// Fired when a cell receive the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCellGotFocus(ChangeActivePositionEventArgs e)
		{
			//Check if the position is valid (this is an useful check because there are cases when the rows are changed inside the leaving events, for example on the DataGrid extension when adding new rows), so I check if the row is still valid
			if (Grid.CompleteRange.Contains(e.NewFocusPosition) == false)
				e.Cancel = true;

			if (e.Cancel)
				return;

			//Evento Got Focus
			if (CellGotFocus != null)
				CellGotFocus(this, e);
			if (e.Cancel)
				return;

			//N.B. E' importante impostare prima la variabile m_FocusCell e dopo chiamare l'evento OnEnter, altrimenti nel caso in cui la cella sia impostata in edit sul focus, l'eseguzione va in loop (cerca di fare l'edit ma per far questo è necessario avere il focus ...)
			m_ActivePosition = e.NewFocusPosition; //Set the focus on the cell

			//Select the cell
			SelectCell(m_ActivePosition, true);

			//Invalidate the selection
			Invalidate();

			////Recalculate the rectangle border
			//RecalcBorderRange();

			//Cell Focus Entered
			Grid.Controller.OnFocusEntered(new CellContext(Grid, e.NewFocusPosition), EventArgs.Empty);

			//Column/Row Focus Enter
			//If the row is different from the previous row, fire a row focus entered
			if (e.NewFocusPosition.Row != e.OldFocusPosition.Row)
				OnFocusRowEntered(new RowEventArgs(ActivePosition.Row));
			//If the column is different from the previous column, fire a column focus entered
			if (e.NewFocusPosition.Column != e.OldFocusPosition.Column)
				OnFocusColumnEntered(new ColumnEventArgs(ActivePosition.Column));
		}

		/// <summary>
		/// Fired when a cell lost the focus
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCellLostFocus(ChangeActivePositionEventArgs e)
		{
			if (e.Cancel)
				return;

			//This code is not necessary because when the cell receive a focus I check
			// if the grid can receive the focus using the SetFocusOnCells method.
			// The SetFocusOnCells method cause any editor to automatically close itself.
			// If I leave this code there are problem when the cell lost the focus because the entire grid lost the focus,
			// in this case the EndEdit cause the grid to receive again the focus. (this problem is expecially visible when using the grid inside a tab and you click on the second tab after set an invalid cell value inside the first tab)
			//CellContext cellLostContext = new CellContext(Grid, e.OldFocusPosition);
			////Stop the Edit operation
			//if (cellLostContext.EndEdit(false) == false)
			//    e.Cancel = true;
			//if (e.Cancel)
			//    return;

			//evento Lost Focus
			if (CellLostFocus != null)
				CellLostFocus(this, e);
			if (e.Cancel)
				return;

			//Row/Column leaving
			//If the new Row is different from the current focus row calls a Row Leaving event
			int focusRow = ActivePosition.Row;
			if (ActivePosition.IsEmpty() == false && focusRow != e.NewFocusPosition.Row)
			{
				RowCancelEventArgs rowArgs = new RowCancelEventArgs(focusRow, e.NewFocusPosition.Row);
				OnFocusRowLeaving(rowArgs);
				if (rowArgs.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}
			//If the new Row is different from the current focus row calls a Row Leaving event
			int focusColumn = ActivePosition.Column;
			if (ActivePosition.IsEmpty() == false && focusColumn != e.NewFocusPosition.Column)
			{
				ColumnCancelEventArgs columnArgs = new ColumnCancelEventArgs(focusColumn, e.NewFocusPosition.Column);
				OnFocusColumnLeaving(columnArgs);
				if (columnArgs.Cancel)
				{
					e.Cancel = true;
					return;
				}
			}

			//Change the focus cell to Empty
			m_ActivePosition = Position.Empty; //from now the cell doesn't have the focus

			//Cell Focus Left
			Grid.Controller.OnFocusLeft(new CellContext(Grid, e.OldFocusPosition), EventArgs.Empty);
		}
		#endregion

		#endregion

		#region Cell navigation

		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified. Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <param name="rowShift"></param>
		/// <param name="colShift"></param>
		/// <returns></returns>
		public bool MoveActiveCell(int rowShift, int colShift)
		{
			return MoveActiveCell(ActivePosition, rowShift, colShift);
		}
		
		public bool MoveActiveCell(int rowShift, int colShift, bool resetSelection)
		{
			return MoveActiveCell(ActivePosition, rowShift, colShift, resetSelection);
		}

		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified. Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		public bool MoveActiveCell(Position start, int rowShift, int colShift)
		{
			return MoveActiveCell(start, rowShift, colShift, true);
		}
		
		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified. Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <returns></returns>
		public bool MoveActiveCell(Position start, int rowShift, int colShift, bool resetSelection)
		{
			Position newPosition = Position.Empty;

			//If there isn't a current active cell I try to put the focus on the 0, 0 cell.
			if (start.IsEmpty())
			{
				newPosition = new Position(0, 0);
				if (CanReceiveFocus(newPosition))
					return Focus(newPosition, true);
				else
				{
					start = newPosition;
					newPosition = Position.Empty;
				}
			}

			int currentRow = start.Row;
			int currentCol = start.Column;

			currentRow += rowShift;
			currentCol += colShift;

			while (newPosition.IsEmpty() && currentRow < Grid.Rows.Count && currentCol < Grid.Columns.Count &&
			       currentRow >= 0 && currentCol >= 0)
			{
				newPosition = new Position(currentRow, currentCol);

				//verifico che la posizione di partenza non coincida con quella di focus, altrimenti significa che ci stiamo spostando sulla stessa cella perchè usa un RowSpan/ColSpan
				if (Grid.PositionToStartPosition(newPosition) == start)
					newPosition = Position.Empty;
				else
				{
					if (CanReceiveFocus(newPosition) == false)
						newPosition = Position.Empty;
				}

				currentRow += rowShift;
				currentCol += colShift;
			}

			if (newPosition.IsEmpty() == false)
				return Focus(newPosition, resetSelection);
			else
				return false;
		}

		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified.
		/// Try to set the focus using the first shift, if failed try to use the second shift (rowShift2, colShift2).
		/// If rowShift2 or colShift2 is int.MaxValue the next start position is the maximum row or column, if is int.MinValue 0 is used, otherwise the current position is shifted using the specified value.
		/// This method is usually used for the Tab navigation using this code : MoveActiveCell(0,1,1,0);
		/// Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <returns></returns>
		public bool MoveActiveCell(int rowShift1, int colShift1, int rowShift2, int colShift2)
		{
			return MoveActiveCell(ActivePosition, rowShift1, colShift1, rowShift2, colShift2);
		}

		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified.
		/// Try to set the focus using the first shift, if failed try to use the second shift (rowShift2, colShift2).
		/// If rowShift2 or colShift2 is int.MaxValue the next start position is the maximum row or column, if is int.MinValue 0 is used, otherwise the current position is shifted using the specified value.
		/// This method is usually used for the Tab navigation using this code : MoveActiveCell(0,1,1,0);
		/// Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="rowShift1"></param>
		/// <param name="colShift1"></param>
		/// <param name="rowShift2"></param>
		/// <param name="colShift2"></param>
		/// <returns></returns>
		public bool MoveActiveCell(Position start, int rowShift1, int colShift1, int rowShift2, int colShift2)
		{
			bool ret = MoveActiveCell(start, rowShift1, colShift1);

			if (ret)
				return true;
			else
			{
				Position newPosition = Position.Empty;

				//If there isn't a current active cell I try to put the focus on the 0, 0 cell.
				if (start.IsEmpty())
				{
					newPosition = new Position(0, 0);
					if (CanReceiveFocus(newPosition))
						return Focus(newPosition, true);
					else
						start = newPosition;
				}

				int row;
				if (rowShift2 == int.MinValue)
					row = 0;
				else if (rowShift2 == int.MaxValue)
					row = Grid.Rows.Count - 1;
				else
					row = start.Row + rowShift2;

				int column;
				if (colShift2 == int.MinValue)
					column = 0;
				else if (colShift2 == int.MaxValue)
					column = Grid.Columns.Count - 1;
				else
					column = start.Column + colShift2;

				newPosition = new Position(row, column);

				if (newPosition == start || Grid.CompleteRange.Contains(newPosition) == false)
					return false;

				if (CanReceiveFocus(newPosition))
					return Focus(newPosition, true);
				else
					start = newPosition;
				return MoveActiveCell(start, rowShift1, colShift1, rowShift2, colShift2);
			}
		}

		#endregion

		#region Drawing
		/// <summary>
		/// Invalidate all the selected cells
		/// </summary>
		public virtual void Invalidate()
		{
			foreach (Range rng in GetSelectionRegion())
				Grid.InvalidateRange(rng);
		}
		#endregion

		#region Selection
		private bool mEnableMultiSelection = true;
		/// <summary>
		/// Gets or sets if enable multi selection using Ctrl key or Shift Key or with mouse. Default is true.
		/// </summary>
		public bool EnableMultiSelection
		{
			get { return mEnableMultiSelection; }
			set { mEnableMultiSelection = value; }
		}

		private System.Drawing.Color mBackColor = System.Drawing.Color.FromArgb(75, System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Highlight));
		/// <summary>
		/// Gets or set the highlight backcolor.
		/// Usually is a color with a transparent value so you can see the color of the cell. Default is: Color.FromArgb(75, Color.FromKnownColor(KnownColor.Highlight))
		/// </summary>
		public System.Drawing.Color BackColor
		{
			get { return mBackColor; }
			set { mBackColor = value; Invalidate(); }
		}

		private DevAge.Drawing.RectangleBorder mBorder = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.Black, 2));
		/// <summary>
		/// The Border used to highlight the range
		/// </summary>
		public DevAge.Drawing.RectangleBorder Border
		{
			get { return mBorder; }
			set { mBorder = value; Invalidate(); }
		}

		/// <summary>
		/// Check if the column is selected. Returns true if one or more row of the column is selected.
		/// </summary>
		public abstract bool IsSelectedColumn(int column);
		/// <summary>
		/// Select or unselect the specified column
		/// </summary>
		/// <param name="column"></param>
		/// <param name="select"></param>
		public abstract void SelectColumn(int column, bool select);

		/// <summary>
		/// Check if the row is selected. Returns true if one or more column of the row is selected.
		/// </summary>
		public abstract bool IsSelectedRow(int row);
		/// <summary>
		/// Select or unselect the specified row
		/// </summary>
		/// <param name="row"></param>
		/// <param name="select"></param>
		public abstract void SelectRow(int row, bool select);

		/// <summary>
		/// Check if the cell is selected.
		/// </summary>
		public abstract bool IsSelectedCell(Position position);
		/// <summary>
		/// Select or unselect the specified cell
		/// </summary>
		/// <param name="position"></param>
		/// <param name="select"></param>
		public abstract void SelectCell(Position position, bool select);

		/// <summary>
		/// Check if the range is selected.
		/// </summary>
		public abstract bool IsSelectedRange(Range range);
		/// <summary>
		/// Select or unselect the specified range
		/// </summary>
		/// <param name="range"></param>
		/// <param name="select"></param>
		public abstract void SelectRange(Range range, bool select);

		/// <summary>
		/// Reset the selection
		/// </summary>
		protected abstract void OnResetSelection();

		/// <summary>
		/// Reset the selection
		/// </summary>
		public void ResetSelection(bool mantainFocus)
		{
			//Reset also the focus is mantainFocus == false
			if (mantainFocus == false && ActivePosition.IsEmpty() == false)
				Focus(Position.Empty, false);

			OnResetSelection();

			//If mantainFocus == true leave the selection for the focus
			if (mantainFocus && ActivePosition.IsEmpty() == false)
				SelectCell(ActivePosition, true);
		}

		/// <summary>
		/// Returns true if the selection is empty
		/// </summary>
		/// <returns></returns>
		public abstract bool IsEmpty();

		/// <summary>
		/// Returns the selected region.
		/// </summary>
		/// <returns></returns>
		public abstract RangeRegion GetSelectionRegion();

		/// <summary>
		/// Returns true if the specified selection intersect with the range
		/// </summary>
		/// <param name="rng"></param>
		/// <returns></returns>
		public abstract bool IntersectsWith(Range rng);

		/// <summary>
		/// Check if the range can be selected
		/// </summary>
		/// <param name="rng"></param>
		/// <returns></returns>
		protected Range ValidateRange(Range rng)
		{
			//Position start = rng.Start;
			//Position end = rng.End;

			//if (rng.Start.Row < Grid.FixedRows)
			//    start = new Position(Grid.FixedRows, start.Column);

			//if (rng.Start.Column < Grid.FixedColumns)
			//    start = new Position(start.Row, Grid.FixedColumns);

			//if (rng.End.Row < Grid.FixedRows)
			//    end = new Position(Grid.FixedRows, end.Column);

			//if (rng.End.Column < Grid.FixedColumns)
			//    end = new Position(end.Row, Grid.FixedColumns);

			//return new Range(start, end);

			return Grid.CompleteRange.Intersect(rng);
		}
		#endregion

		#region Selection Events
		/// <summary>
		/// Fired after when the selection change (added or removed).
		/// If you need more control over the selection I suggest to create a custom Selection class.
		/// </summary>
		public event RangeRegionChangedEventHandler SelectionChanged;
		protected virtual void OnSelectionChanged(RangeRegionChangedEventArgs e)
		{
			if (SelectionChanged != null)
				SelectionChanged(this, e);

			//Invalidate the new selection and the previous (removed) selection

			if (e.AddedRange != null)
			{
				foreach (Range r in e.AddedRange)
					Grid.InvalidateRange(r);
			}

			if (e.RemovedRange != null)
			{
				foreach (Range r in e.RemovedRange)
					Grid.InvalidateRange(r);
			}
		}
		#endregion
	}
}
