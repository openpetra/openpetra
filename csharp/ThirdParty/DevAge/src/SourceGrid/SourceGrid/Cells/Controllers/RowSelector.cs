using System;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Summary description for FullColumnSelection.
	/// </summary>
	public class RowSelector : ControllerBase
	{
		/// <summary>
		/// Default controller to select all the column
		/// </summary>
		public readonly static RowSelector Default = new RowSelector();

		public RowSelector()
		{
		}

		public override void OnClick(CellContext sender, EventArgs e)
		{
			base.OnClick (sender, e);

			sender.Grid.Selection.SelectRow(sender.Position.Row, true );
		}
	}
}
