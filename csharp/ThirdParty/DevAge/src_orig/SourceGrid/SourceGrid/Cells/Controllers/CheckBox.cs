using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Controllers
{
	//TODO I can now use the VisualElement GetElementsAtPoint method to check the checkbox only when clicking on the check and not when clicking on the header. This can also be used with the HeaderCheckBox cell (to enable the sort when clicking outside the check and to select all when clicking the check box.

	/// <summary>
	/// Summary description for BehaviorModelCheckBox. This behavior can be shared between multiple cells.
	/// </summary>
	public class CheckBox : ControllerBase
	{
		/// <summary>
		/// Default behavior checkbox
		/// </summary>
		public readonly static CheckBox Default = new CheckBox();
		/// <summary>
		/// An event similar to Windows.Forms.CheckBox.CheckedChanged.
		/// Fired when value changed
		/// </summary>
		public event EventHandler CheckedChanged;
		
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (CheckedChanged != null) {
				CheckedChanged(this, e);
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CheckBox()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_bAutoChangeValueOfSelectedCells">Indicates if this cells when checked or uncheck must change also the value of the selected cells of type CellCheckBox.</param>
		public CheckBox(bool p_bAutoChangeValueOfSelectedCells)
		{
			m_bAutoChangeValueOfSelectedCells = p_bAutoChangeValueOfSelectedCells;
		}

		#region Controller Events
		public override void OnKeyPress(CellContext sender, KeyPressEventArgs e)
		{
			base.OnKeyPress(sender, e);

			if (e.KeyChar == ' ')
				UIChangeChecked(sender, e);
		}

		/// <summary>
		/// I mantain the last mouse button pressed here to simulate exactly the behavior of the standard system CheckBox.
		/// 
		/// Here are the events executed on a system checkbox:
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
				UIChangeChecked(sender, e);
		}

		#endregion

		private bool m_bAutoChangeValueOfSelectedCells = false;
		/// <summary>
		/// Indicates if this cells when checked or uncheck must change also the value of the selected cells of type CellCheckBox. Default is false
		/// </summary>
		public bool AutoChangeValueOfSelectedCells
		{
			get{return m_bAutoChangeValueOfSelectedCells;}
		}

		/// <summary>
		/// Toggle the value of the current cell and if AutoChangeValueOfSelectedCells is true of all the selected cells.
		/// Simulate an edit operation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UIChangeChecked(CellContext sender, EventArgs e)
		{
			Models.ICheckBox checkModel = (Models.ICheckBox)sender.Cell.Model.FindModel(typeof(Models.ICheckBox));;
			if (checkModel == null)
				throw new SourceGrid.SourceGridException("Models.ICheckBox not found");

			Models.CheckBoxStatus checkStatus = checkModel.GetCheckBoxStatus(sender);
			if (checkStatus.CheckEnable)
			{
				bool newVal = true;
				if (checkStatus.Checked != null)
					newVal = !checkStatus.Checked.Value;

				sender.StartEdit();
				try
				{
					checkModel.SetCheckedValue(sender, newVal);
					sender.EndEdit(false);
					OnCheckedChanged(EventArgs.Empty);
				}
				catch(Exception ex)
				{
					sender.EndEdit(true);
					throw new Exception(string.Empty, ex);
				}

				//change the status of all selected control
				if (AutoChangeValueOfSelectedCells)
				{
					AutoChangeValues(sender, newVal);
				}
			}
		}
		
		private void AutoChangeValues(CellContext sender, bool newVal)
		{
			foreach(Position pos in sender.Grid.Selection.GetSelectionRegion().GetCellsPositions())
			{
				Cells.ICellVirtual c = sender.Grid.GetCell(pos);
				Models.ICheckBox check;
				if (c != this && c != null &&
				    (check = (Models.ICheckBox)c.Model.FindModel(typeof(Models.ICheckBox))) != null )
				{
					CellContext context = new CellContext(sender.Grid, pos, c);
					context.StartEdit();
					try
					{
						check.SetCheckedValue(context, newVal);
						context.EndEdit(false);
					}
					catch(Exception)
					{
						context.EndEdit(true);
						throw;
					}
				}
			}
		}

	}
}
