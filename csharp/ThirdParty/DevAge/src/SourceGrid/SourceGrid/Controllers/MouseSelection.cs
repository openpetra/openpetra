using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Controllers
{
	/// <summary>
	/// A Grid controllers used to handle mouse selection and multi selection with Shift and Ctrl.
	/// </summary>
	public class MouseSelection : GridBase
	{
		public static MouseSelection Default = new MouseSelection();

		protected override void OnAttach(GridVirtual grid)
		{
			grid.MouseDown += new GridMouseEventHandler(grid_MouseDown);
			grid.MouseUp += new GridMouseEventHandler(grid_MouseUp);
			grid.MouseMove += new GridMouseEventHandler(grid_MouseMove);
			grid.MouseLeave += new GridEventHandler(grid_MouseLeave);
		}

		protected override void OnDetach(GridVirtual grid)
		{
			grid.MouseDown -= new GridMouseEventHandler(grid_MouseDown);
			grid.MouseUp -= new GridMouseEventHandler(grid_MouseUp);
			grid.MouseMove -= new GridMouseEventHandler(grid_MouseMove);
			grid.MouseLeave -= new GridEventHandler(grid_MouseLeave);
		}


		protected virtual void grid_MouseDown(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			//Check if the cell is valid
			if (sender.Selection.ActivePosition.IsEmpty() == false)
			{
				//If the cell is still active exit
				if (sender.MouseDownPosition == sender.Selection.ActivePosition)
					return;

				//If the cell is still in editing then exit and stop the multi selection
				CellContext focusCell = new CellContext(sender, sender.Selection.ActivePosition);
				if (focusCell.Cell != null && focusCell.IsEditing())
					return;
			}

#warning Da fare
            ////Select the cell
            //if (sender.MouseDownPosition.IsEmpty() == false)
            //{
            //    Cells.ICellVirtual cellMouseDown = sender.GetCell(sender.MouseDownPosition);
            //    if (cellMouseDown != null)
            //    {
            //        //Only select the cell if click inside or ourside the selection area (if click inside the border is not considered)
            //        float distance;
            //        DevAge.Drawing.RectanglePartType partType = sender.Selection.Border.GetPointPartType(sender.Selection.GetDrawingRectangle(), 
            //            new System.Drawing.Point( e.X, e.Y) , out distance);
            //        if (partType == DevAge.Drawing.RectanglePartType.ContentArea || 
            //            partType == DevAge.Drawing.RectanglePartType.None)
            //        {
            //            bool l_bShiftPress = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift &&
            //                (sender.SpecialKeys & GridSpecialKeys.Shift) == GridSpecialKeys.Shift);
				
            //            if (l_bShiftPress == false || 
            //                sender.Selection.EnableMultiSelection == false || 
            //                sender.Selection.ActivePosition.IsEmpty() )
            //            {
            //                //Standard focus on the cell on MouseDown
            //                if (sender.Selection.Contains(sender.MouseDownPosition) == false || e.Button == MouseButtons.Left) //solo se non è stata ancora selezionata
            //                    sender.Selection.Focus(sender.MouseDownPosition);
            //            }
            //            else //handle shift key
            //            {
            //                sender.Selection.Clear();
            //                Range rangeToSelect = new Range(sender.Selection.ActivePosition, sender.MouseDownPosition);
            //                sender.Selection.Add(rangeToSelect);
            //            }
            //        }
            //    }
            //}
		}

		protected virtual void grid_MouseUp(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			//questo è per assicurarsi che la selezione precedentemente fatta tramite mouse venga effettivamente deselezionata
			sender.MouseSelectionFinish();
		}

		protected virtual void grid_MouseMove(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && sender.Selection.EnableMultiSelection)
			{
                //Scroll if necesary
                sender.ScrollOnPoint(e.Location);



                Position pointPosition = sender.PositionAtPoint(e.Location);
                Cells.ICellVirtual cellPosition = sender.GetCell(pointPosition);

				//Only if there is a FocusCell
				CellContext focusCellContext = new CellContext(sender, sender.Selection.ActivePosition);
				if (focusCellContext.Cell != null && focusCellContext.IsEditing() ==false)
				{
                    Position selCornerPos = pointPosition;
                    Cells.ICellVirtual selCorner = cellPosition;

                    ////If the current Focus Cell is a scrollable cell then search the current cell (under the mouse)only in scrollable cells
                    //// see PositionAtPoint with false parameter
                    //if (sender.GetPositionType(sender.Selection.ActivePosition) == CellPositionType.Scrollable)
                    //{
                    //    selCornerPos = sender.PositionAtPoint(new Point(e.X, e.Y));
                    //    selCorner = sender.GetCell(pointPosition);
                    //}

                    if (selCornerPos.IsEmpty() == false && selCorner != null)
					{
						//Only if the user start the selection with a cell (m_MouseDownCell!=null)
						if (sender.MouseDownPosition.IsEmpty() == false && sender.Selection.IsSelectedCell(sender.MouseDownPosition))
						{
                            sender.ChangeMouseSelectionCorner(selCornerPos);
							//sender.ShowCell(l_SelCornerPos);
						}
					}
				}
			}
		}

		protected virtual void grid_MouseLeave(GridVirtual sender, EventArgs e)
		{
			//questo è per assicurarsi che la selezione del mouse venga effettivamente deselezionata
			sender.MouseSelectionFinish();
		}
	}
}
