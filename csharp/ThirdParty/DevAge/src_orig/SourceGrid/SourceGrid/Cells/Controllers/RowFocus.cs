using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// RowFocus controller overrides the OnFocusEntering method and set the Focus on the Row. This controller is usually used on the Row Header. This controller also add an arror Cursor when the mouse is over the cell.
	/// </summary>
	public class RowFocus : ControllerBase
	{
		/// <summary>
		/// Default controller to select all the row
		/// </summary>
		public readonly static RowFocus Default = new RowFocus();

		public RowFocus()
		{
		}

		/// <summary>
		/// Border used to calculate the region to enable the row selection
		/// </summary>
		public DevAge.Drawing.RectangleBorder LogicalBorder = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(System.Drawing.Color.Black, 4), new DevAge.Drawing.BorderLine(System.Drawing.Color.Black, 4) );
		private MouseCursor mSelectionCursor = new MouseCursor(DevAge.Windows.Forms.Resources.CursorRightArrow, false);

		//Non uso questo evento altrimenti non verrebbero applicate le regole di selezione del tasto Shift
//		public override void OnClick(PositionEventArgs e)
//		{
//			base.OnClick (e);
//
//			sender.Grid.Rows[sender.Position.Row].Focus();
//		}

		public override void OnMouseMove(CellContext sender, MouseEventArgs e)
		{
			base.OnMouseMove (sender, e);

			Rectangle l_CellRect = sender.Grid.PositionToRectangle(sender.Position);
			Point l_MousePoint = new Point(e.X, e.Y);

			float distance;
			DevAge.Drawing.RectanglePartType partType = LogicalBorder.GetPointPartType(l_CellRect, l_MousePoint, out distance);

			if ( partType == DevAge.Drawing.RectanglePartType.ContentArea)
				mSelectionCursor.ApplyCursor(sender, e);
		}

		public override void OnMouseLeave(CellContext sender, EventArgs e)
		{
			base.OnMouseLeave(sender, e);

			mSelectionCursor.ResetCursor(sender, e);
		}

		public override void OnFocusEntering(CellContext sender, System.ComponentModel.CancelEventArgs e)
		{
			base.OnFocusEntering (sender, e);

			sender.Grid.Selection.FocusRow(sender.Position.Row);
		}

	}
}
