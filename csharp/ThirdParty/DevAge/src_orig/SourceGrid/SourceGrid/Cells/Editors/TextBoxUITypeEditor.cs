using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

namespace SourceGrid.Cells.Editors
{
	/// <summary>
	///  An editor that use a UITypeEditor to edit the cell.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class TextBoxUITypeEditor : TextBoxButton
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public TextBoxUITypeEditor(Type p_Type):base(p_Type)
		{
		}
		#endregion

		#region Edit Control
		/// <summary>
		/// Create the editor control
		/// </summary>
		/// <returns></returns>
		protected override Control CreateControl()
		{
			DevAge.Windows.Forms.TextBoxUITypeEditor editor = new DevAge.Windows.Forms.TextBoxUITypeEditor();
			editor.BorderStyle = DevAge.Drawing.BorderStyle.None;
			editor.Validator = this;

			object objEditor = System.ComponentModel.TypeDescriptor.GetEditor(base.ValueType,typeof(System.Drawing.Design.UITypeEditor));
			UITypeEditor uiEditor = null;
			if (editor != null)
				uiEditor = (System.Drawing.Design.UITypeEditor)objEditor;

			editor.UITypeEditor = uiEditor;
			return editor;
		}

		/// <summary>
		/// Gets the control used for editing the cell.
		/// </summary>
		public new DevAge.Windows.Forms.TextBoxUITypeEditor Control
		{
			get
			{
				return (DevAge.Windows.Forms.TextBoxUITypeEditor)base.Control;
			}
		}
		#endregion
	}
}
