using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// Editor for a ComboBox (using DevAgeComboBox control)
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class ComboBox : EditorControlBase
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public ComboBox(Type p_Type):base(p_Type)
		{
		}

		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		/// <param name="p_StandardValues"></param>
		/// <param name="p_StandardValueExclusive">True to not allow custom value, only the values specified in the standardvalues collection are allowed.</param>
		public ComboBox(Type p_Type, ICollection p_StandardValues, bool p_StandardValueExclusive):base(p_Type)
		{
			StandardValues = p_StandardValues;
			StandardValuesExclusive = p_StandardValueExclusive;
		}
		#endregion

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
            DevAge.Windows.Forms.DevAgeComboBox editor = new DevAge.Windows.Forms.DevAgeComboBox();
			//editor.FlatStyle = FlatStyle.System;
			editor.Validator = this;

            //NOTE: I have changed a little the ArrangeLinkedControls to support ComboBox control

			return editor;
		}

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new DevAge.Windows.Forms.DevAgeComboBox Control
		{
			get
			{
                return (DevAge.Windows.Forms.DevAgeComboBox)base.Control;
			}
		}
		#endregion

		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public override void SetEditValue(object editValue)
		{
			if (editValue is string && IsStringConversionSupported() &&
                    Control.DropDownStyle == ComboBoxStyle.DropDown)
			{
                Control.SelectedIndex = -1;
                Control.Text = (string)editValue;
                Control.SelectionLength = 0;
				if (Control.Text != null)
					Control.SelectionStart = Control.Text.Length;
				else
					Control.SelectionStart = 0;
			}
			else
			{
                Control.SelectedIndex = -1;
                Control.Value = editValue;
				Control.SelectAll();
			}
		}

		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override object GetEditedValue()
		{
			return Control.Value;
		}

        protected override void OnSendCharToEditor(char key)
        {
            if (Control.DropDownStyle == ComboBoxStyle.DropDown)
            {
                Control.Text = key.ToString();
                if (Control.Text != null)
                    Control.SelectionStart = Control.Text.Length;
            }
        }
	}
}

