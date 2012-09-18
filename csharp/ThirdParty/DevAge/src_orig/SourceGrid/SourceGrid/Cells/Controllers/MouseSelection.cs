using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// A cell controller used to handle mouse selection
	/// </summary>
	public class MouseSelection : ControllerBase
	{
		public static MouseSelection Default = new MouseSelection();

		/// <summary>
		/// Controls which mouse buttons invoke mouse selection.
		/// Default is MouseButtons.Left
		/// </summary>
		public MouseButtons MouseButtons {get;set;}
		
		public MouseSelection()
		{
			MouseButtons = MouseButtons.Left;
		}
		
		
		public override void OnMouseDown(CellContext sender, System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseDown(sender, e);

			var pressed = e.Button & MouseButtons;
			if (pressed == MouseButtons.None)
				return;
			
			GridVirtual grid = sender.Grid;

			//Check the control and shift key status
			bool controlPress = ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
			                     (grid.SpecialKeys & GridSpecialKeys.Control) == GridSpecialKeys.Control);

			bool shiftPress = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift &&
			                   (grid.SpecialKeys & GridSpecialKeys.Shift) == GridSpecialKeys.Shift);

			//Default click handler
			if (shiftPress == false ||
			    grid.Selection.EnableMultiSelection == false)
			{
				//Handle Control key
				bool mantainSelection = grid.Selection.EnableMultiSelection && controlPress;

				//If the cell is already selected and the user has the ctrl key pressed then deselect the cell
				if (controlPress && grid.Selection.IsSelectedCell(sender.Position) && grid.Selection.ActivePosition != sender.Position)
					grid.Selection.SelectCell(sender.Position, false);
				else
					grid.Selection.Focus(sender.Position, !mantainSelection);
			}
			else //handle shift key
			{
				grid.Selection.ResetSelection(true);

				Range rangeToSelect = new Range(grid.Selection.ActivePosition, sender.Position);
				grid.Selection.SelectRange(rangeToSelect, true);
			}

			// begin scroll tracking only if mouse was clicked
			// in scrollable area
			Rectangle scrollRect = grid.GetScrollableArea();
			if (scrollRect.Contains(e.Location))
				BeginScrollTracking(grid);
			
		}

		public override void OnMouseUp(CellContext sender, MouseEventArgs e)
		{
			base.OnMouseUp(sender, e);

			if (e.Button != MouseButtons.Left)
				return;
			
			sender.Grid.MouseSelectionFinish();

			EndScrollTracking();
		}

		/// <summary>
		/// Used for mouse multi selection and mouse scrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnMouseMove(CellContext sender, MouseEventArgs e)
		{
			if ( mCapturedGrid != null && mCapturedMouseMoveEventArgs != null )
			{
				mCapturedMouseMoveSender = CellContext.Empty;
				mCapturedMouseMoveEventArgs = null;
			}

			base.OnMouseMove(sender, e);

			//First check if the multi selection is enabled and the active position is valid
			if (sender.Grid.Selection.EnableMultiSelection == false ||
			    sender.Grid.MouseDownPosition.IsEmpty() ||
			    sender.Grid.MouseDownPosition != sender.Grid.Selection.ActivePosition)
				return;

			// preprare helper variables
			int? lastVisibleRow = sender.Grid.Rows.LastVisibleScrollableRow;
			int? lastVisibleColumn = sender.Grid.Columns.LastVisibleScrollableColumn;
			int? firstVisibleRow = sender.Grid.Rows.FirstVisibleScrollableRow;
			int? firstVisibleColumn = sender.Grid.Columns.FirstVisibleScrollableColumn;
			
			//Check if the mouse position is valid and try to fix it if it's not
			int? row = sender.Grid.Rows.RowAtPoint(e.Y);
			int? col = sender.Grid.Columns.ColumnAtPoint(e.X);
			if ( !row.HasValue )
			{
				if ( e.Y < 0 )
					row = firstVisibleRow;
				else
					row = lastVisibleRow;
			}
			if ( !col.HasValue )
			{
				if ( e.X < 0 )
					col = firstVisibleColumn;
				else
					col = lastVisibleColumn;
			}

			if ( ! col.HasValue || ! row.HasValue )
				return;

			// Fix mouse position so it does not go out of visible range
			if ( lastVisibleRow.HasValue && row.Value > lastVisibleRow.Value )
				row = lastVisibleRow;
			if ( lastVisibleColumn.HasValue && col.Value > lastVisibleColumn.Value )
				col = lastVisibleColumn;
			if ( firstVisibleRow.HasValue && row < firstVisibleRow.Value )
				row = firstVisibleRow;
			if ( firstVisibleColumn.HasValue && col < firstVisibleColumn.Value )
				col = firstVisibleColumn;
			
			Position mousePosition = new Position(row.Value, col.Value);


			//If the position type is different I don't continue
			// bacause this can cause problem for example when selection the fixed rows when the scroll is on a position > 0
			// that cause all the rows to be selected
			if (sender.Grid.GetPositionType(mousePosition) !=
			    sender.Grid.GetPositionType(sender.Grid.Selection.ActivePosition))
				return;

			sender.Grid.ChangeMouseSelectionCorner(mousePosition);
			if ( mCapturedGrid != null )
			{
				mCapturedMouseMoveSender = sender;
				mCapturedMouseMoveEventArgs = e;
			}
		}

		/// <summary>
		/// Ends scroll tracking on double click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnDoubleClick(CellContext sender, EventArgs e)
		{
			base.OnDoubleClick(sender, e);
			
			EndScrollTracking();
		}
		
		private Timer mScrollTimer;
		private GridVirtual mCapturedGrid;
		private CellContext mCapturedMouseMoveSender = CellContext.Empty;
		private MouseEventArgs mCapturedMouseMoveEventArgs = null;

		/// <summary>
		/// Start the timer to scroll the visible area
		/// </summary>
		/// <param name="grid"></param>
		private void BeginScrollTracking(GridVirtual grid)
		{
			//grid.Capture = true;
			mCapturedGrid = grid;

			if (mScrollTimer == null)
			{
				mScrollTimer = new Timer();
				mScrollTimer.Interval = 100;
				mScrollTimer.Tick += this.mScrollTimer_Tick;
			}
			mScrollTimer.Enabled = true;
		}
		/// <summary>
		/// Stop the timer
		/// </summary>
		private void EndScrollTracking()
		{
			//grid.Capture = false;
			if (mScrollTimer != null)
				mScrollTimer.Enabled = false;
			mCapturedGrid = null;
			mCapturedMouseMoveEventArgs = null;
			mCapturedMouseMoveSender = CellContext.Empty;
		}

		private void mScrollTimer_Tick(object sender, EventArgs e)
		{
			if (mCapturedGrid == null)
				return;
			if (mCapturedGrid.IsDisposed == true)
			{
				EndScrollTracking();
				return;
			}
			
			//If grid has lost focus end scroll tracking
			if (!mCapturedGrid.Focused)
			{
				EndScrollTracking();
				return;
			}

			
			//Scroll the view if required
			Point mousePoint = mCapturedGrid.PointToClient(Control.MousePosition);
			mCapturedGrid.ScrollOnPoint(mousePoint);
			// If we are scrolling view fire mouse move event to change also selection
			if ( mCapturedMouseMoveEventArgs != null )
			{
				this.OnMouseMove(mCapturedMouseMoveSender,
				                 new MouseEventArgs(mCapturedMouseMoveEventArgs.Button, mCapturedMouseMoveEventArgs.Clicks,
				                                    mousePoint.X, mousePoint.Y, mCapturedMouseMoveEventArgs.Delta));
			}
		}
	}
}
