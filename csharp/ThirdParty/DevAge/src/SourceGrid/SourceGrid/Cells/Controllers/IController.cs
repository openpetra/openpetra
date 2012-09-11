using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Controllers
{
	/// <summary>
	/// Represents a behavior of a cell.
	/// </summary>
	public interface IController
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseDown(CellContext sender, MouseEventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseUp(CellContext sender, MouseEventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseMove(CellContext sender, MouseEventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseEnter(CellContext sender, EventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseLeave(CellContext sender, EventArgs e);

        /// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnKeyUp (CellContext sender, KeyEventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnKeyDown (CellContext sender, KeyEventArgs e);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnKeyPress (CellContext sender, KeyPressEventArgs e);

		/// <summary>
		/// Process MouseDoubleClick event. See
		/// http://msdn.microsoft.com/lt-lt/library/system.windows.forms.control.mousedoubleclick(en-us,VS.80).aspx
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnDoubleClick (CellContext sender, EventArgs e);

        /// <summary>
		/// Handles OnMouseClick event. See
		/// http://msdn.microsoft.com/lt-lt/library/system.windows.forms.control.mouseclick(en-us,VS.80).aspx
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnClick (CellContext sender, EventArgs e);

		/// <summary>
		/// Fired before the cell leave the focus, you can put the e.Cancel = true to cancel the leave operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnFocusLeaving(CellContext sender, System.ComponentModel.CancelEventArgs e);
		/// <summary>
		/// Fired when the cell has left the focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnFocusLeft(CellContext sender, EventArgs e);
		/// <summary>
		/// Fired when the focus is entering in the specified cell. You can put the e.Cancel = true to cancel the focus operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnFocusEntering(CellContext sender, System.ComponentModel.CancelEventArgs e);
		/// <summary>
		/// Fired when the focus enter in the specified cell.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnFocusEntered(CellContext sender, EventArgs e);

		/// <summary>
		/// Fired before the value of the cell is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnValueChanging(CellContext sender, ValueChangeEventArgs e);

		/// <summary>
		/// Fired after the value of the cell is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnValueChanged(CellContext sender, EventArgs e);

		/// <summary>
		/// Fired when the StartEdit is called and before the cell start the edit operation. You can set the Cancel = true to stop editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnEditStarting(CellContext sender, System.ComponentModel.CancelEventArgs e);
		/// <summary>
		/// Fired when the StartEdit is sucesfully called.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnEditStarted(CellContext sender, EventArgs e);
		/// <summary>
		/// Fired when the EndEdit is called. You can read the Cancel property to determine if the edit is completed. If you change the cancel property there is no effect.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnEditEnded(CellContext sender, EventArgs e);

		/// <summary>
		/// Returns true if the current cell can receive the focus. If only one behavior return false the return value is false.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		bool CanReceiveFocus(CellContext sender, EventArgs e);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnDragDrop(CellContext sender, DragEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnDragEnter(CellContext sender, DragEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnDragLeave(CellContext sender, EventArgs e);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnDragOver(CellContext sender, DragEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnGiveFeedback(CellContext sender, GiveFeedbackEventArgs e);
	}
}
