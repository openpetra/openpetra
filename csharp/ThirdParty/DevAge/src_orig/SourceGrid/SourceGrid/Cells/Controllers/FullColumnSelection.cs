using System;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Summary description for FullColumnSelection.
	/// </summary>
	public class FullColumnSelection : ControllerBase
	{
		/// <summary>
		/// Default controller to select all the column
		/// </summary>
		public readonly static FullColumnSelection Default = new FullColumnSelection();

		public FullColumnSelection()
		{
		}

		public override void OnSelectionAdding(CellContext sender, RangeRegionChangingEventArgs e)
		{
			base.OnSelectionAdding (sender, e);

			if (sender.Grid.Controller.FindController(typeof(FullRowSelection)) != null)
				return;

			Range rangeToAdd = new Range(0, sender.Position.Column, sender.Grid.Rows.Count - 1, sender.Position.Column);

			e.RegionToInclude.Add(rangeToAdd);
		}
		public override void OnSelectionRemoving(CellContext sender, RangeRegionChangingEventArgs e)
		{
			base.OnSelectionRemoving (sender, e);

			if (sender.Grid.Controller.FindController(typeof(FullRowSelection)) != null)
				return;

			Range rangeToRemove = new Range(0, sender.Position.Column, sender.Grid.Rows.Count - 1, sender.Position.Column);

			e.RegionToInclude.Add(rangeToRemove);
		}

//		public override void OnClick(PositionEventArgs e)
//		{
//			base.OnClick (e);
//
//			if (sender.Grid.Controller.FindController(typeof(FullRowSelection)) != null)
//				return;
//
//			Range rangeToAdd = sender.Grid.Columns[sender.Position.Column].Range;
//
//			sender.Grid.Selection.Add(rangeToAdd);
//		}

	}
}
