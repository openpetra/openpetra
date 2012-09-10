using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;

namespace SourceGrid.Cells.Editors
{
	/// <summary>
	///  A model that use a TextBoxButton for Image editing, allowing to select a source image file. Returns null as DisplayString. Write and read byte[] values.
	/// </summary>
	[System.ComponentModel.ToolboxItem(false)]
	public class ImagePicker : EditorControlBase
	{
		public readonly static ImagePicker Default = new ImagePicker();
		
		#region Constructor
		/// <summary>
		/// Construct an Editor of type ImagePicker.
		/// </summary>
		public ImagePicker():base(typeof(byte[]))
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
			editor.Validator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
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

		public override object GetEditedValue()
		{
			object val = Control.Value;
			if (val == null)
				return null;
			else if (val is System.Drawing.Image)
			{
				DevAge.ComponentModel.Validator.ValidatorTypeConverter imageValidator = new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(System.Drawing.Image));
				return imageValidator.ValueToObject(val, typeof(byte[]));

				//Stranamente questo codice in caso di ico va in eccezione!
//				System.Drawing.Image img = (System.Drawing.Image)val;
//				using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
//				{
//					img.Save(memStream, System.Drawing.Imaging.ImageCodecInfo.);
//
//					return memStream.ToArray();
//				}
			}
			else if (val is byte[])
				return val;
			else
				throw new SourceGridException("Invalid edited value, expected byte[] or Image");
		}

		public override void SetEditValue(object editValue)
		{
			Control.Value = editValue;
			Control.TextBox.SelectAll();
		}

		/// <summary>
		/// Used to returns the display string for a given value. In this case return null.
		/// </summary>
		/// <param name="p_Value"></param>
		/// <returns></returns>
		public override string ValueToDisplayString(object p_Value)
		{
			return null;
		}

		protected override void OnSendCharToEditor(char key)
		{
			//No implementation
		}
	}
}
