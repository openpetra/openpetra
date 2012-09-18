using System;

namespace DevAge.Text.FixedLength
{
	[Serializable]
	public class InvalidFieldLengthException : DevAgeApplicationException
	{
		public InvalidFieldLengthException(int length):
			base("Invalid field length " + length.ToString() + " must be a positive number.")
		{
		}

#if !MINI
		protected InvalidFieldLengthException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class ValueNotValidLengthException : DevAgeApplicationException
	{
		public ValueNotValidLengthException(string value, int expectedLength):
			base("Value " + value + " not valid, length must be " + expectedLength.ToString())
		{
		}

#if !MINI
		protected ValueNotValidLengthException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class ValueNotSupportedException : DevAgeApplicationException
	{
		public ValueNotSupportedException(string value, Type type):
			base("Value " + value + " not supported, type is " + type.Name)
		{
		}

#if !MINI
		protected ValueNotSupportedException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class TypeNotSupportedException : DevAgeApplicationException
	{
		public TypeNotSupportedException(Type type):
			base("Type " + type.ToString() + " not supported")
		{
		}

#if !MINI
		protected TypeNotSupportedException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class RegExException : DevAgeApplicationException
	{
		public RegExException(string group):
			base("Regular expression group " + group + " not valid")
		{
		}

#if !MINI
		protected RegExException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class FieldParseException : DevAgeApplicationException
	{
		public FieldParseException(string name, string valToParse, Exception innerException):
			base("Failed to parse field " + name + " '" + valToParse + "' - " + innerException.Message, innerException)
		{
		}

#if !MINI
		protected FieldParseException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class FieldStringConvertException : DevAgeApplicationException
	{
		public FieldStringConvertException(string name, object value, Exception innerException):
			base("Failed to convert to string field " + name + " '" + FieldStringConvertException.ObjectToStringForError(value) + "' - " + innerException.Message, innerException)
		{
		}

#if !MINI
		protected FieldStringConvertException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
		/// <summary>
		/// Returns a string used for error description for a specified object. Usually used when printing the object for the error message when there is a conversion error.
		/// </summary>
		/// <param name="val"></param>
		private static string ObjectToStringForError(object val)
		{
			try
			{
				if (val == null)
					return "<null>";
				else
					return val.ToString();
			}
			catch(Exception)
			{
				return "<object>";
			}
		}
	}


	[Serializable]
	public class FieldNotDefinedException : DevAgeApplicationException
	{
		public FieldNotDefinedException(int fieldIndex):
			base("Field " + fieldIndex.ToString() + " not defined.")
		{
		}

#if !MINI
		protected FieldNotDefinedException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}

	[Serializable]
	public class FailedPropertySetFieldException : DevAgeApplicationException
	{
		public FailedPropertySetFieldException(string field, Exception innerException):
			base("Failed to set property for field " + field + " - " + innerException.Message, innerException)
		{
		}

#if !MINI
		protected FailedPropertySetFieldException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}
	[Serializable]
	public class FailedPropertyGetFieldException : DevAgeApplicationException
	{
		public FailedPropertyGetFieldException(string field, Exception innerException):
			base("Failed to get property for field " + field + " - " + innerException.Message, innerException)
		{
		}

#if !MINI
		protected FailedPropertyGetFieldException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}
}
