using System;

namespace SourceGrid.Cells.Controllers
{
	public class ControllerContainer : IController
	{
		private ControllerList m_ControllerList = null;
		/// <summary>
		/// A collection of elements of type IController. Only one instance of the same controller is allowed.
		/// </summary>
		public class ControllerList : DevAge.Collections.ListByType<IController>
		{
		}


		/// <summary>
		/// Returns null if not exist
		/// </summary>
		/// <param name="modelType"></param>
		/// <returns></returns>
		public virtual IController FindController(Type modelType)
		{
			if (m_ControllerList == null)
				m_ControllerList = new ControllerList();

			return m_ControllerList.GetByType(modelType);
		}


		public virtual void AddController(IController model)
		{
			if (m_ControllerList == null)
				m_ControllerList = new ControllerList();

			m_ControllerList.Add(model);
		}

		public virtual void RemoveController(IController model)
		{
			if (m_ControllerList != null)
				m_ControllerList.Remove(model);
		}

		#region IController Members
		public void OnMouseDown(CellContext sender, System.Windows.Forms.MouseEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnMouseDown(sender, e);
		}

		public void OnMouseUp(CellContext sender, System.Windows.Forms.MouseEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnMouseUp(sender, e);
		}

		public void OnMouseMove(CellContext sender, System.Windows.Forms.MouseEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnMouseMove(sender, e);
		}

		public void OnMouseEnter(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnMouseEnter(sender, e);
		}

		public void OnMouseLeave(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnMouseLeave(sender, e);
		}

		public void OnKeyUp(CellContext sender, System.Windows.Forms.KeyEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnKeyUp(sender, e);
		}

		public void OnKeyDown(CellContext sender, System.Windows.Forms.KeyEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnKeyDown(sender, e);
		}

		public void OnKeyPress(CellContext sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnKeyPress(sender, e);
		}

		public void OnDoubleClick(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnDoubleClick(sender, e);
		}

		public void OnClick(CellContext sender, EventArgs e)
		{
			for (int Counter = 0; Counter < m_ControllerList.Count; Counter++)
			{
				m_ControllerList[Counter].OnClick(sender, e);
			}
		}

		public void OnFocusLeaving(CellContext sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnFocusLeaving(sender, e);
		}

		public void OnFocusLeft(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnFocusLeft(sender, e);
		}

		public void OnFocusEntering(CellContext sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnFocusEntering(sender, e);
		}

		public void OnFocusEntered(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnFocusEntered(sender, e);
		}

		/// <summary>
		/// Fired before the value of the cell is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnValueChanging(CellContext sender, ValueChangeEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnValueChanging(sender, e);
		}

		/// <summary>
		/// Fired after the value of the cell is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnValueChanged(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnValueChanged(sender, e);
		}

		/// <summary>
		/// Fired when the StartEdit is called and before the cell start the edit operation. You can set the Cancel = true to stop editing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnEditStarting(CellContext sender, System.ComponentModel.CancelEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnEditStarting(sender, e);
		}
		/// <summary>
		/// Fired when the StartEdit is sucesfully called.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnEditStarted(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnEditStarted(sender, e);
		}
		/// <summary>
		/// Fired when the EndEdit is called. You can read the Cancel property to determine if the edit is completed. If you change the cancel property there is no effect.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnEditEnded(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnEditEnded(sender, e);
		}

		public bool CanReceiveFocus(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
			{
				if (controller.CanReceiveFocus(sender, e) == false)
					return false;
			}

			return true;
		}

		public void OnDragDrop(CellContext sender, System.Windows.Forms.DragEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnDragDrop(sender, e);
		}

		public void OnDragEnter(CellContext sender, System.Windows.Forms.DragEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnDragEnter(sender, e);
		}

		public void OnDragLeave(CellContext sender, EventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnDragLeave(sender, e);
		}

		public void OnDragOver(CellContext sender, System.Windows.Forms.DragEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnDragOver(sender, e);
		}

		public void OnGiveFeedback(CellContext sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			foreach (IController controller in m_ControllerList)
				controller.OnGiveFeedback(sender, e);
		}
		#endregion
	}
}
