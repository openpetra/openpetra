using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Button controller is used to executed a specific action when the user click on a cell or when the user press the Enter or Space key (using the OnKeyDown event).
	/// Is normally used with the Link or Button Cell.
	/// Override the OnExecuted to add your code or use the Executed event.
	/// </summary>
	public class Button : ControllerBase
	{
        /// <summary>
        /// I mantain the last mouse button pressed here to simulate exactly the behavior of the standard system button.
        /// 
        /// Here are the events executed on a system button:
        /// 
        /// [status checked = false]
        /// MouseDown [status checked = false]
        /// CheckedChanged [status checked = true]
        /// Click [status checked = true]
        /// MouseUp [status checked = true]
        /// 
        /// Consider that I can use this member varialbes because also if you have multiple grid or multiple threads there is only one mouse that can fire the events.
        /// Consider also that I cannot use the Click event because in that event I don't have informations about the button pressed.
        /// </summary>
        private MouseButtons mLastButton = MouseButtons.None;

        public override void OnMouseDown(CellContext sender, MouseEventArgs e)
        {
            base.OnMouseDown(sender, e);

            mLastButton = e.Button;
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);

            if (mLastButton == MouseButtons.Left)
                OnExecuted(sender, e);
        }

        public override void OnKeyDown(CellContext sender, KeyEventArgs e)
        {
            base.OnKeyDown(sender, e);

            if (e.Handled)
                return;

            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                OnExecuted(sender, e);
                e.Handled = true;
            }
        }

		public event EventHandler Executed;
		public virtual void OnExecuted(CellContext sender, EventArgs e)
		{
			if (Executed != null)
				Executed(sender, e);
		}
	}
}
