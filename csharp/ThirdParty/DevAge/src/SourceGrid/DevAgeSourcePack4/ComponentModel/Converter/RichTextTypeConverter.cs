using System;

namespace DevAge.ComponentModel.Converter
{
    /// <summary>
    /// A TypeConverter that support rich text conversion from and to string.
    /// </summary>
    public class RichTextTypeConverter :
#if !MINI
 System.ComponentModel.TypeConverter
#else
		DevAge.ComponentModel.TypeConverter
#endif

    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public RichTextTypeConverter()
        {
        }

        #endregion

        #region Implementation TypeConverter

        /// <summary>
        /// String and RichText can be converted from
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context,
                                        Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            if (sourceType == typeof(DevAge.Windows.Forms.RichText))
                return true;
            if (sourceType == typeof(int))
                return true;

            return false;
        }

        /// <summary>
        /// String and RichText can be converted to
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context,
                                        Type destinationType)
        {
            if (destinationType == typeof(DevAge.Windows.Forms.RichText))
                return true;
            if (destinationType == typeof(string))
                return true;

            return false;
        }

        /// <summary>
        /// Convert String to RichText
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value != null && (value.GetType() == typeof(string) || value.GetType() == typeof(int)))
            {
                return DevAge.Windows.Forms.RichTextConversion.StringToRichText(value.ToString());
            }
            else if (value != null && value.GetType() == typeof(DevAge.Windows.Forms.RichText))
            {
                return value;
            }
            else
            {
                throw new ArgumentException("Not supported type");
            }
        }

        /// <summary>
        /// Convert RichText to String
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null && value.GetType() == typeof(DevAge.Windows.Forms.RichText))
            {
                return DevAge.Windows.Forms.RichTextConversion.RichTextToString(value as DevAge.Windows.Forms.RichText);
            }
            else if (destinationType == typeof(DevAge.Windows.Forms.RichText) && IsValid(value))
            {
                return new DevAge.Windows.Forms.RichText(value as string);
            }
            else if (destinationType == typeof(DevAge.Windows.Forms.RichText) && value != null && value.GetType() == typeof(string))
            {
                return DevAge.Windows.Forms.RichTextConversion.StringToRichText(value as string);
            }
            else if(value != null && destinationType == value.GetType())
            {
                return value;
            }
            else
            {
                throw new ArgumentException("Not supported type");
            }
        }

        /// <summary>
        /// Check if rich text string is valid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
        {
            if (value != null && value.GetType() == typeof(string))
            {
                // try to convert it
                try
                {
                    ConvertFrom(context, System.Globalization.CultureInfo.CurrentCulture, value);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}
