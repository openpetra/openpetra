using System;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for ControlCursor.
	/// </summary>
	public class ControlCursor
	{
		private System.Windows.Forms.Cursor mCursor;
		private System.Windows.Forms.Cursor mOldCursor;
		private System.Windows.Forms.Control mControl;
		public ControlCursor(System.Windows.Forms.Cursor pCursor)
		{
			mCursor = pCursor;
		}

		public void ApplyCursor(System.Windows.Forms.Control control)
		{
			if (mControl == null)
			{
				mControl = control;
				mOldCursor = mControl.Cursor;
				mControl.Cursor = mCursor;
			}
		}

		public void ResetCursor()
		{
			if (mControl != null && mControl.Cursor == mCursor)
			{
				mControl.Cursor = mOldCursor;
				mControl = null;
			}
		}
	}
}
