using System;

namespace DevAge.Text.FixedLength
{
	/// <summary>
	/// Required attribute to specify the field position and length
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Property)]
	public class FieldAttribute : System.Attribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fieldIndex">Index of the field, 0 based. Each field must have a unique progressive index</param>
		/// <param name="length">Lenght of the field when readed and writed to the string.</param>
		public FieldAttribute(int fieldIndex, int length)
		{
			mFieldIndex = fieldIndex;
			mLength = length;
		}

		private int mFieldIndex;
		public int FieldIndex
		{
			get{return mFieldIndex;}
		}
		private int mLength;
		public int Length
		{
			get{return mLength;}
		}
	}

	/// <summary>
	/// Attribute used to specify additional parse options
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Property)]
	public class ParseFormatAttribute : System.Attribute
	{
		/// <summary>
        /// Constructor. Use one of these properties to customize the format: CultureInfo, DateTimeFormat, NumberFormat, TrimBeforeParse.
		/// Default is Invariant culture format.
		/// </summary>
		public ParseFormatAttribute()
		{
			System.Globalization.CultureInfo invariant = System.Globalization.CultureInfo.InvariantCulture;

			DateTimeFormat = invariant.DateTimeFormat.ShortDatePattern;
            NumberFormat = "+00000000.0000;-00000000.0000";
			TrimBeforeParse = true;
			CultureInfo = invariant;
		}

		private string mDateTimeFormat;
        private string mNumberFormat;
		private bool mTrimBeforeParse;
		private System.Globalization.CultureInfo mCultureInfo;

		public System.Globalization.CultureInfo CultureInfo
		{
			get{return mCultureInfo;}
			set{mCultureInfo = value;}
		}
		public string DateTimeFormat
		{
			get{return mDateTimeFormat;}
			set{mDateTimeFormat = value;}
		}
		public string NumberFormat
		{
            get { return mNumberFormat; }
            set { mNumberFormat = value; }
		}
		public bool TrimBeforeParse
		{
			get{return mTrimBeforeParse;}
			set{mTrimBeforeParse = value;}
		}
	}

	
	/// <summary>
	/// Attribute used to convert a specific value to another value
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
	public class ValueMappingAttribute : System.Attribute
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="stringValue">String value</param>
		/// <param name="fieldValue">Field typed value</param>
		public ValueMappingAttribute(string stringValue, object fieldValue)
		{
			StringValue = stringValue;
			FieldValue = fieldValue;
		}

		private string mStringValue;
		public string StringValue
		{
			get{return mStringValue;}
			set{mStringValue = value;}
		}

		private object mFieldValue;
		public object FieldValue
		{
			get{return mFieldValue;}
			set{mFieldValue = value;}
		}
	}

	/// <summary>
	/// Attribute used to specify the standard value (mandatory value) for a specific field.
    /// You can use this attribute for example when you want a particular field to only accept one or more standard values.
	/// </summary>
	[System.AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
	public class StandardValueAttribute : System.Attribute
	{
		/// <summary>
		/// Construcotr
		/// </summary>
		/// <param name="standardValue">Required value</param>
		public StandardValueAttribute(object standardValue)
		{
			mStandardValue = standardValue;
		}

		private object mStandardValue;
		public object StandardValue
		{
			get{return mStandardValue;}
			set{mStandardValue = value;}
		}
	}
}
