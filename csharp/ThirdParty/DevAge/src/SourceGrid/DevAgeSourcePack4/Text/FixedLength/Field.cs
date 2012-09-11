using System;

namespace DevAge.Text.FixedLength
{
	/// <summary>
	/// Interface for defining a Field in the FixedLength string
	/// </summary>
	public interface IField
	{
		string RegularExpressionPattern
		{
			get;
		}

		/// <summary>
		/// Index of the field. 0 based.
		/// </summary>
		int Index
		{
			get;
		}

		/// <summary>
		/// Name of the field, used for retriving the field by its name.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Convert the specified value to a string value valid for this field
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		string ValueToString(object val);

		/// <summary>
		/// Convert the specified string value to a value based on the field format.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		object StringToValue(string str);
	}

	/// <summary>
	/// Class for define a field, implements IField interface.
	/// </summary>
	public class Field : IField
	{
		public Field(int index, string name, int length, Type type):this(index, name, length, new ComponentModel.Validator.ValidatorTypeConverter(type))
		{
		}

		public Field(int index, string name, int length, ComponentModel.Validator.IValidator validator)
		{
			mIndex = index;
			mName = name;
			Length = length;
			Validator = validator;
		}

		private int mIndex;
		public int Index
		{
			get{return mIndex;}
		}
		private string mName;
		public string Name
		{
			get{return mName;}
		}

		private int mLength;
		public int Length
		{
			get{return mLength;}
			set
			{
				if (value <= 0)
					throw new InvalidFieldLengthException(value);
				mLength = value;
			}
		}

		private bool mTrimBeforeParse = true;
		public bool TrimBeforeParse
		{
			get{return mTrimBeforeParse;}
			set{mTrimBeforeParse = value;}
		}
		private ComponentModel.Validator.IValidator mValidator;
		public ComponentModel.Validator.IValidator Validator
		{
			get{return mValidator;}
			set
			{
				if (value == null)
					throw new ArgumentNullException("Validator");
				mValidator = value;
			}
		}


		public virtual string RegularExpressionPattern
		{
			get
			{
				string pattern = "(?<{0}>.{{{1}}})"; //sample:  (?<NOME>.{LENGTH})
				return string.Format(pattern, Name, Length.ToString());
			}
		}

		public virtual string ValueToString(object val)
		{
			try
			{
				string tmp = Validator.ValueToString(val);
				if (tmp == null)
					tmp = string.Empty;

				if (tmp.Length > Length)
					throw new ValueNotValidLengthException(tmp, Length);
				else if (tmp.Length < Length)
				{
                    if (Validator.AllowNull)
                    {
                        tmp = tmp.PadRight(Length, ' ');
                    }
                    else
                    {
                        if (Validator.ValueType == typeof(string))
                            tmp = tmp.PadRight(Length, ' ');
                        else if (Validator.ValueType == typeof(int))
                            tmp = tmp.PadLeft(Length, '0'); //N.B. Questo sarebbe da cambiare nel caso in cui l'int abbia il segno!!
                        else if (Validator.ValueType == typeof(double))
                            throw new ValueNotValidLengthException(tmp, Length);
                        else if (Validator.ValueType == typeof(decimal))
                            throw new ValueNotValidLengthException(tmp, Length);
                        else
                            throw new ValueNotSupportedException(tmp, Validator.ValueType);
                    }
				}

				return tmp;
			}
			catch(Exception ex)
			{
				throw new FieldStringConvertException(Name, val, ex);
			}
		}

		public virtual object StringToValue(string str)
		{
			try
			{
				if (TrimBeforeParse && str != null)
					str = str.Trim();

				return Validator.StringToValue(str);
			}
			catch(Exception ex)
			{
				throw new FieldParseException(Name, str, ex);
			}
		}
	}

//	public class FieldFiller : Field
//	{
//		public FieldFiller(int index, string name, int length):base(index, name, length, typeof(string))
//		{
//		}
//
//		public override string RegularExpressionPattern
//		{
//			get
//			{
//				string pattern = "(?<{0}>.{{1}}){2}";
//				return string.Format(pattern, Name, "*", Utilities.ValidateRegExpSeparator(Separator));
//			}
//		}
//	}
}
