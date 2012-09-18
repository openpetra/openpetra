using System;
using System.Drawing;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Summary description for FullRowSelection.
	/// </summary>
	public class FullRowSelection : ControllerBase
	{
		/// <summary>
		/// Default controller to select all the row
		/// </summary>
		public readonly static FullRowSelection Default = new FullRowSelection();

		public FullRowSelection()
		{
		}

		public override void OnSelectionAdding(CellContext sender, RangeRegionChangingEventArgs e)
		{
			base.OnSelectionAdding (sender, e);

			if (sender.Grid.Controller.FindController(typeof(FullColumnSelection)) != null)
				return;

			Range rangeToAdd = new Range(sender.Position.Row, 0, sender.Position.Row, sender.Grid.Columns.Count - 1);

			e.RegionToInclude.Add(rangeToAdd);
		}
		public override void OnSelectionRemoving(CellContext sender, RangeRegionChangingEventArgs e)
		{
			base.OnSelectionRemoving (sender, e);

			if (sender.Grid.Controller.FindController(typeof(FullColumnSelection)) != null)
				return;

			Range rangeToRemove = new Range(sender.Position.Row, 0, sender.Position.Row, sender.Grid.Columns.Count - 1);

			e.RegionToInclude.Add(rangeToRemove);
		}



//		public override void OnClick(PositionEventArgs e)
//		{
//			base.OnClick (e);
//
//			if (sender.Grid.Controller.FindController(typeof(FullColumnSelection)) != null)
//				return;
//
//			Range rangeToAdd = sender.Grid.Rows[sender.Position.Row].Range;
//
//			sender.Grid.Selection.Add(rangeToAdd);
//		}

	}
}
