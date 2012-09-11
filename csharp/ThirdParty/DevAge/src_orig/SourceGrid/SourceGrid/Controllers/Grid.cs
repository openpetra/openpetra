using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Controllers
{
	/// <summary>
	/// Grid controller for standard events.
	/// </summary>
	public class Grid : GridBase
	{
		public static Grid Default = new Grid();

		protected override void OnAttach(GridVirtual grid)
		{
			grid.MouseDown += new GridMouseEventHandler(grid_MouseDown);
			grid.MouseUp += new GridMouseEventHandler(grid_MouseUp);
			grid.MouseMove += new GridMouseEventHandler(grid_MouseMove);
			grid.MouseWheel += new GridMouseEventHandler(grid_MouseWheel);
			grid.MouseLeave += new GridEventHandler(grid_MouseLeave);
			grid.DragDrop += new GridDragEventHandler(grid_DragDrop);
			grid.DragEnter += new GridDragEventHandler(grid_DragEnter);
			grid.DragLeave += new GridEventHandler(grid_DragLeave);
			grid.DragOver += new GridDragEventHandler(grid_DragOver);
			grid.GiveFeedback += new GridGiveFeedbackEventHandler(grid_GiveFeedback);
			grid.Click += new GridEventHandler(grid_Click);
			grid.DoubleClick += new GridEventHandler(grid_DoubleClick);
			grid.KeyDown += new GridKeyEventHandler(grid_KeyDown);
			grid.KeyUp += new GridKeyEventHandler(grid_KeyUp);
			grid.KeyPress += new GridKeyPressEventHandler(grid_KeyPress);
		}

		protected override void OnDetach(GridVirtual grid)
		{
			grid.MouseDown -= new GridMouseEventHandler(grid_MouseDown);
			grid.MouseUp -= new GridMouseEventHandler(grid_MouseUp);
			grid.MouseMove -= new GridMouseEventHandler(grid_MouseMove);
			grid.MouseWheel -= new GridMouseEventHandler(grid_MouseWheel);
			grid.MouseLeave -= new GridEventHandler(grid_MouseLeave);
			grid.DragDrop -= new GridDragEventHandler(grid_DragDrop);
			grid.DragEnter -= new GridDragEventHandler(grid_DragEnter);
			grid.DragLeave -= new GridEventHandler(grid_DragLeave);
			grid.DragOver -= new GridDragEventHandler(grid_DragOver);
			grid.GiveFeedback -= new GridGiveFeedbackEventHandler(grid_GiveFeedback);
			grid.Click -= new GridEventHandler(grid_Click);
			grid.DoubleClick -= new GridEventHandler(grid_DoubleClick);
			grid.KeyDown -= new GridKeyEventHandler(grid_KeyDown);
			grid.KeyUp -= new GridKeyEventHandler(grid_KeyUp);
			grid.KeyPress -= new GridKeyPressEventHandler(grid_KeyPress);
		}


		protected virtual void grid_MouseDown(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			//verifico che l'eventuale edit sia terminato altrimenti esco
			if (sender.Selection.ActivePosition.IsEmpty() == false)
			{
				CellContext focusCell = new CellContext(sender, sender.Selection.ActivePosition);
				if (focusCell.Cell != null && focusCell.IsEditing())
				{
					if (focusCell.EndEdit(false) == false)
						return;
				}
			}

			//scateno eventi di MouseDown
			Position position = sender.PositionAtPoint( new Point(e.X, e.Y) );
			if (position.IsEmpty() == false)
			{
				Cells.ICellVirtual cellMouseDown = sender.GetCell(position);
				if (cellMouseDown != null)
				{
					sender.ChangeMouseDownCell(position, position);

					//Cell.OnMouseDown
					CellContext cellContext = new CellContext(sender, position, cellMouseDown);
					sender.Controller.OnMouseDown(cellContext, e);
				}
			}
			else
				sender.ChangeMouseDownCell(Position.Empty, Position.Empty);
		}

		protected virtual void grid_MouseUp(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (sender.MouseDownPosition.IsEmpty() == false)
			{
				Cells.ICellVirtual l_MouseDownCell = sender.GetCell(sender.MouseDownPosition);
				if (l_MouseDownCell!=null)
					sender.Controller.OnMouseUp(new CellContext(sender, sender.MouseDownPosition, l_MouseDownCell), e );

				sender.ChangeMouseDownCell(Position.Empty, sender.PositionAtPoint(new Point(e.X, e.Y)));
			}
		}

		protected virtual void grid_MouseMove(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
			Position l_PointPosition = sender.PositionAtPoint(new Point(e.X, e.Y));
			Cells.ICellVirtual l_CellPosition = sender.GetCell(l_PointPosition);

			//Call MouseMove on the cell that receive tha MouseDown event
			if (sender.MouseDownPosition.IsEmpty() == false)
			{
				Cells.ICellVirtual l_MouseDownCell = sender.GetCell(sender.MouseDownPosition);
				if (l_MouseDownCell!=null)
				{
					sender.Controller.OnMouseMove(new CellContext(sender, sender.MouseDownPosition, l_MouseDownCell), e);
				}
			}
			else //se non ho nessuna cella attualmente che ha ricevuto un mousedown, l'evento di MouseMove viene segnalato sulla cella correntemente sotto il Mouse
			{
				// se non c'è nessuna cella MouseDown cambio la cella corrente sotto il Mouse
#if !MINI
				sender.ChangeMouseCell(l_PointPosition);//in ogni caso cambio la cella corrente
#endif
				if (l_PointPosition.IsEmpty() == false && l_CellPosition != null)
				{
					// I call MouseMove on the current cell only if there aren't any cells under the mouse
					sender.Controller.OnMouseMove(new CellContext(sender, l_PointPosition, l_CellPosition), e);
				}
			}
		}

		protected virtual void grid_MouseWheel(GridVirtual sender, System.Windows.Forms.MouseEventArgs e)
		{
            sender.CustomScrollWheel(e.Delta);
		}

		protected virtual void grid_MouseLeave(GridVirtual sender, EventArgs e)
		{
			sender.ChangeMouseCell(Position.Empty);
		}

		protected virtual void grid_DragDrop(GridVirtual sender, System.Windows.Forms.DragEventArgs e)
		{
            Position pointPosition = sender.PositionAtPoint(sender.PointToClient(new Point(e.X, e.Y)));
            Cells.ICellVirtual cellPosition = sender.GetCell(pointPosition);

			sender.Controller.OnDragDrop(new CellContext(sender, pointPosition, cellPosition), e);
		}
		protected virtual void grid_DragEnter(GridVirtual sender, System.Windows.Forms.DragEventArgs e)
		{

			Position pointPosition = sender.PositionAtPoint( sender.PointToClient( new Point(e.X, e.Y) ));
			Cells.ICellVirtual cellPosition = sender.GetCell(pointPosition);

			sender.ChangeDragCell(new CellContext(sender, pointPosition, cellPosition), e);
		}
		protected virtual void grid_DragLeave(GridVirtual sender, EventArgs e)
		{
			sender.ChangeDragCell(new CellContext(sender, Position.Empty, null), null);
		}
		protected virtual void grid_DragOver(GridVirtual sender, System.Windows.Forms.DragEventArgs e)
		{
			Position pointPosition = sender.PositionAtPoint(sender.PointToClient( new Point(e.X, e.Y) ));
			Cells.ICellVirtual cellPosition = sender.GetCell(pointPosition);

			CellContext cellContext = new CellContext(sender, pointPosition, cellPosition);
			sender.ChangeDragCell(cellContext, e);
			sender.Controller.OnDragOver(cellContext, e);
		}

		protected virtual void grid_GiveFeedback(GridVirtual sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			Position dragPosition = sender.DragCellPosition;
			Cells.ICellVirtual cellPosition = sender.GetCell(dragPosition);

			sender.Controller.OnGiveFeedback(new CellContext(sender, dragPosition, cellPosition), e);
		}
		protected virtual void grid_Click(GridVirtual sender, EventArgs e)
		{
			Position clickPosition = sender.PositionAtPoint(sender.PointToClient(Control.MousePosition));
			Position clickStartPosition = sender.PositionToStartPosition(clickPosition);

			//Se ho precedentemente scatenato un MouseDown su una cella 
			// e se questa corrisponde alla cella sotto il puntatore del mouse (non posso usare MouseCellPosition perchè questa viene aggiornata solo quando non si ha una cella come MouseDownPosition
			if (sender.MouseDownPosition.IsEmpty() == false && 
				sender.MouseDownPosition == clickStartPosition /* MouseCellPosition && 
				m_MouseDownCell.Focused == true //tolto altrimenti non funzionava per le celle Selectable==false*/)
			{
				Cells.ICellVirtual mouseDownCell = sender.GetCell(sender.MouseDownPosition);
				if (mouseDownCell != null)
				{
					sender.Controller.OnClick(new CellContext(sender, sender.MouseDownPosition, mouseDownCell), EventArgs.Empty);
				}
			}		
		}
		protected virtual void grid_DoubleClick(GridVirtual sender, EventArgs e)
		{

			if (sender.MouseDownPosition.IsEmpty() == false)
			{
				Cells.ICellVirtual l_MouseDownCell = sender.GetCell(sender.MouseDownPosition);
				if (l_MouseDownCell!=null)
				{
					sender.Controller.OnDoubleClick(new CellContext(sender, sender.MouseDownPosition, l_MouseDownCell), EventArgs.Empty);
				}
			}
		}

		protected virtual void grid_KeyDown(GridVirtual sender, System.Windows.Forms.KeyEventArgs e)
		{

			if (sender.Selection.ActivePosition.IsEmpty() == false)
			{
				Cells.ICellVirtual l_FocusCell = sender.GetCell(sender.Selection.ActivePosition);
				if (l_FocusCell!=null)
					sender.Controller.OnKeyDown( new CellContext(sender, sender.Selection.ActivePosition, l_FocusCell), e );
			}

			if (e.Handled == false)
				sender.ProcessSpecialGridKey(e);
		}
		protected virtual void grid_KeyUp(GridVirtual sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (sender.Selection.ActivePosition.IsEmpty() == false)
			{
				Cells.ICellVirtual l_FocusCell = sender.GetCell(sender.Selection.ActivePosition);
				if (l_FocusCell!=null)
					sender.Controller.OnKeyUp(new CellContext(sender, sender.Selection.ActivePosition, l_FocusCell), e );
			}
		}

		private void grid_KeyPress(GridVirtual sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			//solo se diverso da tab e da a capo ( e non è un comando di copia/incolla)
			if (sender.Selection.ActivePosition.IsEmpty() || e.KeyChar == '\t' || e.KeyChar == 13 ||
				e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24)
			{
			}
			else
			{
				Cells.ICellVirtual l_FocusCell = sender.GetCell(sender.Selection.ActivePosition);
				if (l_FocusCell != null)
					sender.Controller.OnKeyPress( new CellContext(sender, sender.Selection.ActivePosition, l_FocusCell), e );
			}
		}
	}
}
