using System;
using System.Windows.Forms;

namespace SourceGrid.Controllers
{
	/// <summary>
	/// Controller to enable the delete of the current selection content when pressing the Delete (Canc) Key.
	/// </summary>
	public class SelectionDelete : GridBase
	{
		/// <summary>
		/// Create a selection drag controller for cut operations.
		/// </summary>
		public readonly static SelectionDelete Default = new SelectionDelete();

		public SelectionDelete()
		{
		}

		protected override void OnAttach(GridVirtual grid)
		{
			grid.KeyDown += new GridKeyEventHandler(grid_KeyDown);
		}

		protected override void OnDetach(GridVirtual grid)
		{
			grid.KeyDown -= new GridKeyEventHandler(grid_KeyDown);
		}

		private void grid_KeyDown(GridVirtual sender, KeyEventArgs e)
		{
			if (e.Handled)
				return;

			if (sender.Selection.IsEmpty())
				return;

			if (e.KeyCode == Keys.Delete)
			{
				sender.ClearValues(sender.Selection.GetSelectionRegion());
				e.Handled = true;
			}
		}
	}
}
