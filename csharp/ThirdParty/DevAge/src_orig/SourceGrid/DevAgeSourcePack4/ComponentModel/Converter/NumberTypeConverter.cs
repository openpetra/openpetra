using System;

namespace DevAge.ComponentModel.Converter
{
	/// <summary>
	/// A TypeConverter that support string conversion from and to string with a custom format number.
	/// Support Conversion for Float, Double and Decimal, Int
	/// </summary>
	public class NumberTypeConverter : 
#if !MINI
		System.ComponentModel.TypeConverter
#else
		DevAge.ComponentModel.TypeConverter
#endif

	{
		#region Constructors
		public NumberTypeConverter(Type p_BaseType)
		{
			BaseType = p_BaseType;
		}

		public NumberTypeConverter(Type p_BaseType,
			string p_Format):this(p_BaseType)
		{
			Format = p_Format;
		}
		#endregion

		#region Member Variables

#if !MINI
		private System.ComponentModel.TypeConverter m_BaseTypeConverter;
		public System.ComponentModel.TypeConverter BaseTypeConverter
#else
		private DevAge.ComponentModel.TypeConverter m_BaseTypeConverter;
		public DevAge.ComponentModel.TypeConverter BaseTypeConverter
#endif
		{
			get{return m_BaseTypeConverter;}
			set{m_BaseTypeConverter = value;}
		}

		private Type m_BaseType;
		public Type BaseType
		{
			get{return m_BaseType;}
			set
			{
				if (value != typeof(double) &&
					value != typeof(float) &&
					value != typeof(decimal) &&
					value != typeof(int))
					throw new ArgumentException("Type not supported", "BaseType");

#if !MINI
				m_BaseTypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(value);
#else
				m_BaseTypeConverter = DevAge.ComponentModel.TypeDescriptor.GetConverter(value);
#endif

				m_BaseType = value;
			}
		}

		private string m_Format = "G";
		public string Format
		{
			get{return m_Format;}
			set{m_Format = value;}
		}

//		private System.IFormatProvider m_FormatProvider = null;
//		public IFormatProvider FormatProvider
//		{
//			get{return m_FormatProvider;}
//			set{m_FormatProvider = value;}
//		}
//
		private System.Globalization.NumberStyles m_NumberStyles = System.Globalization.NumberStyles.Number;
		public System.Globalization.NumberStyles NumberStyles
		{
			get{return m_NumberStyles;}
			set{m_NumberStyles = value;}
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
				if (BaseType == typeof(double))
					return StringToDouble((string)value,NumberStyles,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(decimal))
					return StringToDecimal((string)value,NumberStyles,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(float))
					return StringToFloat((string)value,NumberStyles,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(int))
					return StringToInt((string)value,NumberStyles,GetCulture(culture).NumberFormat);
				else
					throw new ArgumentException("Not supported type");
			}
			else
				return m_BaseTypeConverter.ConvertFrom(context,culture,value);
		}
		
		public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value != null)
			{
				if (BaseType == typeof(double))
					return DoubleToString((double)value,Format,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(decimal))
					return DecimalToString((decimal)value,Format,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(float))
					return FloatToString((float)value,Format,GetCulture(culture).NumberFormat);
				else if (BaseType == typeof(int))
					return IntToString((int)value,Format,GetCulture(culture).NumberFormat);
				else
					return m_BaseTypeConverter.ConvertTo(context,culture,value,destinationType);
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


		#region Conversion String Methods
		public static double StringToDouble(string p_strVal,
											System.Globalization.NumberStyles style , 
											System.IFormatProvider provider)
		{
			return double.Parse(p_strVal,style,provider);
		}
		public static float StringToFloat(string p_strVal,
			System.Globalization.NumberStyles style , 
			System.IFormatProvider provider)
		{
			return float.Parse(p_strVal,style,provider);
		}
		public static decimal StringToDecimal(string p_strVal,
			System.Globalization.NumberStyles style , 
			System.IFormatProvider provider )
		{
			return decimal.Parse(p_strVal,style,provider);
		}
		public static int StringToInt(string p_strVal,
			System.Globalization.NumberStyles style , 
			System.IFormatProvider provider )
		{
			return int.Parse(p_strVal,style,provider);
		}

		public static string DoubleToString(double p_Val, System.String format , System.IFormatProvider provider  )
		{
			return p_Val.ToString(format,provider);
		}
		public static string FloatToString(float p_Val, System.String format , System.IFormatProvider provider  )
		{
			return p_Val.ToString(format,provider);
		}
		public static string DecimalToString(decimal p_Val, System.String format , System.IFormatProvider provider  )
		{
			return p_Val.ToString(format,provider);
		}
		public static string IntToString(int p_Val, System.String format , System.IFormatProvider provider  )
		{
			return p_Val.ToString(format,provider);
		}
		#endregion
	}
}
