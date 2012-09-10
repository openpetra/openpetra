using System;

namespace DevAge.ComponentModel.Converter
{
	/// <summary>
	/// Summary description for DateTimeTypeConverter.
	/// </summary>
	public class DateTimeTypeConverter : 
#if !MINI
		System.ComponentModel.TypeConverter
#else
		DevAge.ComponentModel.TypeConverter
#endif
	{
		#region Constructors
		public DateTimeTypeConverter()
		{
		}

		public DateTimeTypeConverter(string p_ToStringFormat)
		{
			m_Format = p_ToStringFormat;
		}

		public DateTimeTypeConverter(string p_ToStringFormat, string[] p_ParseFormats)
		{
			m_ParseFormats = p_ParseFormats;
			m_Format = p_ToStringFormat;
		}

		public DateTimeTypeConverter(string p_ToStringFormat, string[] p_ParseFormats, System.Globalization.DateTimeStyles p_DateTimeStyles)
		{
			m_ParseFormats = p_ParseFormats;
			m_Format = p_ToStringFormat;
			m_DateTimeStyles = p_DateTimeStyles;
		}
		#endregion

		#region Member Variables

#if !MINI
		private System.ComponentModel.TypeConverter m_BaseTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(DateTime));
		public System.ComponentModel.TypeConverter BaseTypeConverter
#else
		private DevAge.ComponentModel.TypeConverter m_BaseTypeConverter = DevAge.ComponentModel.TypeDescriptor.GetConverter(typeof(DateTime));
		public DevAge.ComponentModel.TypeConverter BaseTypeConverter
#endif
		{
			get{return m_BaseTypeConverter;}
			set{m_BaseTypeConverter = value;}
		}

		private System.Globalization.DateTimeStyles m_DateTimeStyles = System.Globalization.DateTimeStyles.AllowInnerWhite|System.Globalization.DateTimeStyles.AllowLeadingWhite|System.Globalization.DateTimeStyles.AllowTrailingWhite|System.Globalization.DateTimeStyles.AllowWhiteSpaces;
		/// <summary>
		/// DateTimeStyle for Parse operations. DefaultValue: AllowInnerWhite|AllowLeadingWhite|AllowTrailingWhite|AllowWhiteSpaces
		/// </summary>
		public System.Globalization.DateTimeStyles DateTimeStyles
		{
			get{return m_DateTimeStyles;}
			set{m_DateTimeStyles = value;}
		}

		private string m_Format = "G";
		/// <summary>
		/// Format of the Date. Example: G, g, d, D. Default value : G
		/// </summary>
		public string Format
		{
			get{return m_Format;}
			set{m_Format = value;}
		}

		private string[] m_ParseFormats = null;
		/// <summary>
		/// Formats to check when parse the string. If null call with no format the parse method. Default value: null
		/// </summary>
		public string[] ParseFormats
		{
			get{return m_ParseFormats;}
			set{m_ParseFormats = value;}
		}
		#endregion

		#region Implementation TypeConverter
		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, 
			Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			else
				return m_BaseTypeConverter.CanConvertFrom(context,sourceType);
		}

		public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, 
			Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;
			else
				return m_BaseTypeConverter.CanConvertTo(context,destinationType);
		}

		public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			return m_BaseTypeConverter.CreateInstance(context,propertyValues);
		}

		public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value != null && value.GetType() == typeof(string))
			{
				if (m_ParseFormats!=null)
					return DateTime.ParseExact((string)value, m_ParseFormats, GetCulture(culture).DateTimeFormat, m_DateTimeStyles);
				else
					return DateTime.Parse((string)value, GetCulture(culture).DateTimeFormat, m_DateTimeStyles);
			}
			else
				return m_BaseTypeConverter.ConvertFrom(context,culture,value);
		}
		
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value != null)
			{
				return ((DateTime)value).ToString(m_Format, GetCulture(culture).DateTimeFormat);
			}
			else
				return m_BaseTypeConverter.ConvertTo(context,culture,value,destinationType);
		}


		public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return m_BaseTypeConverter.GetCreateInstanceSupported (context);
		}
		public override System.ComponentModel.PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return m_BaseTypeConverter.GetProperties (context, value, attributes);
		}
		public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return m_BaseTypeConverter.GetPropertiesSupported (context);
		}
		public override StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			return m_BaseTypeConverter.GetStandardValues (context);
		}
		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context)
		{
			return m_BaseTypeConverter.GetStandardValuesExclusive (context);
		}
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return m_BaseTypeConverter.GetStandardValuesSupported (context);
		}
		public override bool IsValid(System.ComponentModel.ITypeDescriptorContext context, object value)
		{
			if (value!=null && value.GetType() == typeof(string))
			{
				//I try to convert it
				try
				{
					ConvertFrom(context,System.Globalization.CultureInfo.CurrentCulture,value);
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}
			else
				return m_BaseTypeConverter.IsValid (context, value);
		}

		#endregion	

		#region Member Utility Function
		private System.Globalization.CultureInfo GetCulture(System.Globalization.CultureInfo p_RequestedCulture)
		{
			if (p_RequestedCulture==null)
				return System.Globalization.CultureInfo.CurrentCulture;
			else
				return p_RequestedCulture;
		}
		#endregion
	}
}
