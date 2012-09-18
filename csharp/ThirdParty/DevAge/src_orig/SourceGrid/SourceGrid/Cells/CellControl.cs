using System;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace SourceGrid.Cells
{
	/// <summary>
	/// The CellControl class is used to create a cell with a Windows Forms Control inside.
	/// The CellControl class requires a new Windows Forms control for each cell. Unfortunately Winwods Forms control requires a lot of system resources and with many cells this can cause system fault or out of memory conditions.
	/// Basically I suggest to use CellControl with no more than 50 cells and only if necessary, usually it is better to use standard cells.
    /// Another problem with the CellControl class is that it is not integrated well with the rest of the grid (control borders, cell navigation, ...)
    /// Finally a cell of type CellControl cannot be moved, for example you cannot use sort, move the columns, ... when using a CellControl.
	/// </summary>
    [Obsolete("I will soon remove this class. If you need to add a user control to the grid I suggest to manually add or remove it using the Grid.LinkedControls collection.")]
	public class CellControl : Cell
	{
		private Control mControl;
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="control">Control to insert inside the grid</param>
		public CellControl(Control control):base(null)
		{
			mControl = control;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="control">Control to insert inside the grid</param>
		/// <param name="scrollMode"></param>
		/// <param name="useCellBorder"></param>
		public CellControl(Control control, LinkedControlScrollMode scrollMode, bool useCellBorder):this(control)
		{
			mControl = control;
			mScrollMode = scrollMode;
			mUseCellBorder = useCellBorder;
		}

		/// <summary>
		/// Gets the control associated with this cell.
		/// </summary>
		public Control Control
		{
			get{return mControl;}
		}

		private LinkedControlScrollMode mScrollMode = LinkedControlScrollMode.BasedOnPosition;
		private bool mUseCellBorder = true;

        public override void BindToGrid(Grid p_grid, Position p_Position)
        {
            base.BindToGrid(p_grid, p_Position);

            LinkedControlValue linkedValue = new LinkedControlValue(mControl, Range.Start);
            linkedValue.ScrollMode = mScrollMode;
            linkedValue.UseCellBorder = mUseCellBorder;
            Grid.LinkedControls.Add(linkedValue);

            Grid.ArrangeLinkedControls();
        }

        public override void UnBindToGrid()
        {
            if (Grid.LinkedControls.GetByControl(mControl) != null)
                Grid.LinkedControls.Remove(Grid.LinkedControls.GetByControl(mControl));

            base.UnBindToGrid();
        }
	}
}