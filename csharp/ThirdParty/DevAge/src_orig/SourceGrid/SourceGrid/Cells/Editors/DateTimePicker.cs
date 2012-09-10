using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// Create an Editor that use a DateTimePicker as control for date editing.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class DateTimePicker : EditorControlBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DateTimePicker():base(typeof(System.DateTime))
		{
		}

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
			System.Windows.Forms.DateTimePicker dtPicker = new System.Windows.Forms.DateTimePicker();
			dtPicker.Format = DateTimePickerFormat.Short;
            dtPicker.ShowCheckBox = AllowNull;
			return dtPicker;
		}

        protected override void OnChanged(EventArgs e)
        {
            base.OnChanged(e);

            //if the control is null the editor is not yet created
            if (Control != null)
            {
                Control.ShowCheckBox = AllowNull;
            }
        }

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new System.Windows.Forms.DateTimePicker Control
		{
			get
			{
				return (System.Windows.Forms.DateTimePicker)base.Control;
			}
		}
		#endregion

		/// <summary>
		/// This method is called just before the edit start. You can use this method to customize the editor with the cell informations.
		/// </summary>
		/// <param name="cellContext"></param>
		/// <param name="editorControl"></param>
		protected override void OnStartingEdit(CellContext cellContext, Control editorControl)
		{
			base.OnStartingEdit(cellContext, editorControl);

			System.Windows.Forms.DateTimePicker dtPicker = (System.Windows.Forms.DateTimePicker)editorControl;
			dtPicker.Font = cellContext.Cell.View.Font;
		}
		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public override void SetEditValue(object editValue)
		{
			if (editValue is DateTime)
				Control.Value = (DateTime)editValue;
            else if (editValue == null)
                Control.Checked = false;
            else
                throw new SourceGridException("Invalid edit value, expected DateTime");
		}
		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override object GetEditedValue()
		{
            if (Control.Checked)
                return Control.Value;
            else
                return null;
		}

        protected override void OnSendCharToEditor(char key)
        {
            //No implementation
        }
	}
}

