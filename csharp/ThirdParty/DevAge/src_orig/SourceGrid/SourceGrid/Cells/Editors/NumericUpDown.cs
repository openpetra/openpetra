using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// EditorNumericUpDown editor class.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class NumericUpDown : EditorControlBase
	{
		/// <summary>
		/// Create a model of type Decimal
		/// </summary>
		public NumericUpDown():base(typeof(decimal))
		{
		}
		
		public NumericUpDown(Type p_CellType, decimal p_Maximum, decimal p_Minimum, decimal p_Increment):base(p_CellType)
		{
			if (p_CellType==null || p_CellType == typeof(int) ||
				p_CellType == typeof(long) || p_CellType == typeof(decimal))
			{
                Control.Maximum = p_Maximum;
                Control.Minimum = p_Minimum;
                Control.Increment = p_Increment;
			}
			else
				throw new SourceGridException("Invalid CellType expected long, int or decimal");
		}

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
			System.Windows.Forms.NumericUpDown l_Control = new System.Windows.Forms.NumericUpDown();
			l_Control.BorderStyle = System.Windows.Forms.BorderStyle.None;
			return l_Control;
		}

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new System.Windows.Forms.NumericUpDown Control
		{
			get
			{
				return (System.Windows.Forms.NumericUpDown)base.Control;
			}
		}
		#endregion

		/// <summary>
		/// Set the specified value in the current editor control.
		/// </summary>
		/// <param name="editValue"></param>
		public override void SetEditValue(object editValue)
		{
			decimal dec;
			if (editValue is decimal)
				dec = (decimal)editValue;
			else if (editValue is long)
				dec = (decimal)((long)editValue);
			else if (editValue is int)
				dec = (decimal)((int)editValue);
			else if (editValue == null)
				dec = Control.Minimum;
			else
				throw new SourceGridException("Invalid value, expected Decimal, Int or Long");

			//.NET BUG:  First I must get the value, otherwise seems that the control don't work properly (when I hit the Escape so I never getthe value so the control use always the previous value also if I manually set the value)
			decimal oldValue = Control.Value;

			Control.Value = dec;
		}

		/// <summary>
		/// Returns the value inserted with the current editor control
		/// </summary>
		/// <returns></returns>
		public override object GetEditedValue()
		{
			if (ValueType == null)
				return Control.Value;
			if (ValueType == typeof(decimal))
				return Control.Value;
			if (ValueType == typeof(int))
				return (int)Control.Value;
			if (ValueType == typeof(long))
				return (long)Control.Value;

			throw new SourceGridException("Invalid type of the cell expected decimal, long or int");
		}

        protected override void OnSendCharToEditor(char key)
        {
            //No implementation
        }
	}
}
