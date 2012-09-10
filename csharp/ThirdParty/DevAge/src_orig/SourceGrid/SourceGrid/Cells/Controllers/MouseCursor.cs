using System;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Allow to customize the cursor of a cell. The cell must implement ICellCursor. This behavior can be shared between multiple cells.
	/// </summary>
	public class MouseCursor : ControllerBase
	{
		public readonly static MouseCursor Default = new MouseCursor(System.Windows.Forms.Cursors.Default, true);
		public readonly static MouseCursor Hand = new MouseCursor(System.Windows.Forms.Cursors.Hand, true);

		#region Constructor
		public MouseCursor(System.Windows.Forms.Cursor cursor, bool applyOnMouseEnter)
		{
			mApplyOnMouseEnter = applyOnMouseEnter;
			mCursor = cursor;
		}
		#endregion

		#region IBehaviorModel Members

		public override void OnMouseEnter(CellContext sender, EventArgs e)
		{
			base.OnMouseEnter(sender, e);

			if (mApplyOnMouseEnter)
				ApplyCursor(sender, e);
		}

		public override void OnMouseLeave(CellContext sender, EventArgs e)
		{
			base.OnMouseLeave(sender, e);

			if (mApplyOnMouseEnter)
				ResetCursor(sender, e);
		}
		#endregion

		/// <summary>
		/// Change the cursor with the cursor of the cell
		/// </summary>
		public virtual void ApplyCursor(CellContext sender, EventArgs e)
		{
            if (Cursor != null)
                sender.Grid.Cursor = Cursor;
		}

		/// <summary>
		/// Reset the original cursor
		/// </summary>
		public virtual void ResetCursor(CellContext sender, EventArgs e)
		{
            if (Cursor != null)
            {
                if (sender.Grid.Cursor == Cursor)
                    sender.Grid.Cursor = null;
            }
		}

		private bool mApplyOnMouseEnter = false;
		private System.Windows.Forms.Cursor mCursor = null;

		public System.Windows.Forms.Cursor Cursor
		{
			get{return mCursor;}
			set{mCursor = value;}
		}
	}
}
