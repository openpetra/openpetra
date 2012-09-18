using System;

namespace SourceGrid.Selection
{
	public interface IGridSelection
	{
		/// <summary>
		/// Fired after when the selection change (added or removed).
		/// If you need more control over the selection I suggest to create a custom Selection class.
		/// </summary>
		event RangeRegionChangedEventHandler SelectionChanged;
		
		/// <summary>
		/// Returns the selected region.
		/// </summary>
		RangeRegion GetSelectionRegion();
		
		/// <summary>
		/// Check if the row is selected. Returns true if one or more column of the row is selected.
		/// </summary>
		bool IsSelectedRow(int row);
		
		/// <summary>
		/// Returns true if the specified position can receive the focus.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		bool CanReceiveFocus(Position position);
		
		/// <summary>
		/// Set the focus on the first available cells starting from the not fixed cells.
		/// If there is an active selection set the focus on the first selected cells.
		/// </summary>
		/// <param name="pResetSelection"></param>
		/// <returns></returns>
		bool FocusFirstCell(bool pResetSelection);
		
		/// <summary>
		/// Link the cell at the specified grid.
		/// For internal use only.
		/// </summary>
		/// <param name="p_grid"></param>
		void BindToGrid(GridVirtual p_grid);
	
		/// <summary>
		/// Remove the link of the cell from the grid.
		/// For internal use only.
		/// </summary>
		void UnBindToGrid();
		
		
		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified.
		/// Try to set the focus using the first shift, if failed try to use the second shift (rowShift2, colShift2).
		/// If rowShift2 or colShift2 is int.MaxValue the next start position is the maximum row or column, if is int.MinValue 0 is used, otherwise the current position is shifted using the specified value.
		/// This method is usually used for the Tab navigation using this code : MoveActiveCell(0,1,1,0);
		/// Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <returns></returns>
		bool MoveActiveCell(int rowShift1, int colShift1, int rowShift2, int colShift2);
		
		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified. Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// Does reset selection
		/// </summary>
		/// <returns></returns>
		bool MoveActiveCell(int rowShift, int colShift);
		
		/// <summary>
		/// Move the active cell (focus), moving the row and column as specified. Returns true if the focus can be moved.
		/// Returns false if there aren't any cell to move.
		/// </summary>
		/// <param name="resetSelection">if false, selection is not reset</param>
		/// <param name="rowShift"></param>
		/// <param name="colShift"></param>
		/// <returns></returns>
		bool MoveActiveCell(int rowShift, int colShift, bool resetSelection);
		
		/// <summary>
		/// Returns true if the selection is empty
		/// </summary>
		/// <returns></returns>
		bool IsEmpty();
		
		/// <summary>
		/// Invalidate all the selected cells
		/// </summary>
		void Invalidate();
		
		/// <summary>
		/// Select or unselect the specified range
		/// </summary>
		/// <param name="range"></param>
		/// <param name="select"></param>
		void SelectRange(Range range, bool select);
	
		
		/// <summary>
		/// Select or unselect the specified cell
		/// </summary>
		/// <param name="position"></param>
		/// <param name="select"></param>
		void SelectCell(Position position, bool select);
		
		/// <summary>
		/// Check if the cell is selected.
		/// </summary>
		bool IsSelectedCell(Position position);
		
		/// <summary>
		/// Gets or sets if enable multi selection using Ctrl key or Shift Key or with mouse. Default is true.
		/// </summary>
		bool EnableMultiSelection {get;set;}
		
		/// <summary>
		/// Reset the selection
		/// </summary>
		void ResetSelection(bool mantainFocus);
		
		/// <summary>
		/// Select or unselect the specified row
		/// </summary>
		void SelectRow(int row, bool select);
		
		/// <summary>
		/// Select or unselect the specified column
		/// </summary>
		/// <param name="column"></param>
		/// <param name="select"></param>
		void SelectColumn(int column, bool select);
	
		
		/// <summary>
		/// Move the Focus to the first cell that can receive the focus of the current row otherwise put the focus to null.
		/// </summary>
		/// <returns></returns>
		bool FocusRow(int row);
	
		/// <summary>
		/// Move the Focus to the first cell that can receive the focus of the current column otherwise put the focus to null.
		/// </summary>
		/// <returns></returns>
		bool FocusColumn(int column);
		
		/// <summary>
		/// Change the ActivePosition (focus) of the grid.
		/// </summary>
		/// <param name="pCellToActivate"></param>
		/// <param name="pResetSelection">True to deselect the previous selected cells</param>
		/// <returns></returns>
		bool Focus(Position pCellToActivate, bool pResetSelection);
		
		/// <summary>
		/// Gets the active cell position. The cell with the focus.
		/// Returns Position.Empty if there isn't an active cell.
		/// </summary>
		Position ActivePosition{get;}
		
		/// <summary>
		/// Gets or sets the behavior of the focus and selection. Default is FocusStyle.Default.
		/// </summary>
		FocusStyle FocusStyle {get;set;}
		
		
		#region Focus Events
		/// <summary>
		/// Fired before a cell receive the focus (FocusCell is populated after this event, use e.Cell to read the cell that will receive the focus)
		/// </summary>
		event ChangeActivePositionEventHandler CellGotFocus;
	
		/// <summary>
		/// Fired before a cell lost the focus
		/// </summary>
		event ChangeActivePositionEventHandler CellLostFocus;
	
		/// <summary>
		/// Fired before a row lost the focus
		/// </summary>
		event RowCancelEventHandler FocusRowLeaving;
		/// <summary>
		/// Fired after a row receive the focus
		/// </summary>
		event RowEventHandler FocusRowEntered;
		/// <summary>
		/// Fired before a column lost the focus
		/// </summary>
		event ColumnCancelEventHandler FocusColumnLeaving;
		/// <summary>
		/// Fired after a column receive the focus
		/// </summary>
		event ColumnEventHandler FocusColumnEntered;
		#endregion
	}
}
