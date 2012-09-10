using System;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Summary description for FullColumnSelection.
	/// </summary>
	public class ColumnSelector : ControllerBase
	{
		/// <summary>
		/// Default controller to select all the column
		/// </summary>
		public readonly static ColumnSelector Default = new ColumnSelector();

		public ColumnSelector()
		{
		}

		public override void OnClick(CellContext sender, EventArgs e)
		{
			base.OnClick (sender, e);

			sender.Grid.Selection.SelectColumn(sender.Position.Column, true);
		}
	}
}
