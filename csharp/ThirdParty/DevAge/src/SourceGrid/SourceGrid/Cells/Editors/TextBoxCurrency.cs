using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// An editor to support Currency data type
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class TextBoxCurrency : TextBoxNumeric
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public TextBoxCurrency(Type p_Type):base(p_Type)
		{
			TypeConverter = new DevAge.ComponentModel.Converter.CurrencyTypeConverter(p_Type);
		}
		#endregion

	}
}