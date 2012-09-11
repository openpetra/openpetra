using System;

namespace DevAge
{
    /// <summary>
    /// Generic DevAge Exception
    /// </summary>
	[Serializable]
	public class DevAgeApplicationException : ApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_strErrDescription"></param>
		public DevAgeApplicationException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_strErrDescription"></param>
        /// <param name="p_InnerException"></param>
		public DevAgeApplicationException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_Info"></param>
        /// <param name="p_StreamingContext"></param>
        protected DevAgeApplicationException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
	}

    /// <summary>
    /// The type specified it is not supported in the current contest
    /// </summary>
	[Serializable]
	public class TypeNotSupportedException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pType"></param>
		public TypeNotSupportedException(Type pType):
			base("Type " + pType.ToString() + " not supported exception")
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="p_InnerException"></param>
		public TypeNotSupportedException(Type pType, Exception p_InnerException):
			base("Type " + pType.ToString() + " not supported exception", p_InnerException)
		{
		}
	}

    /// <summary>
    /// Command line not valid exception
    /// </summary>
	[Serializable]
	public class UnrecognizedCommandLineParametersException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameter"></param>
		public UnrecognizedCommandLineParametersException(string parameter):
			base("Unrecognized command line parameter " + parameter + ".")
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="p_InnerException"></param>
		public UnrecognizedCommandLineParametersException(string parameter, Exception p_InnerException):
			base("Unrecognized command line parameter " + parameter + ".", p_InnerException)
		{
		}
	}

    /// <summary>
    /// Conversion exception
    /// </summary>
	[Serializable]
	public class ConversionErrorException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="value"></param>
        /// <param name="extendedMessage"></param>
		public ConversionErrorException(string destinationType, string value, string extendedMessage):
			base(extendedMessage + ", cannot convert " + value + " to " + destinationType  + ".")
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="value"></param>
		public ConversionErrorException(string destinationType, string value):
			base("Cannot convert " + value + " to " + destinationType  + ".")
		{
		}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="value"></param>
        /// <param name="p_InnerException"></param>
		public ConversionErrorException(string destinationType, string value, Exception p_InnerException):
			base("Cannot convert " + value + " to " + destinationType  + ".", p_InnerException)
		{
		}
	}

	/// <summary>
	/// Common EventArgs class used to store and raise events with an Exception associated
	/// </summary>
	public class ExceptionEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ex"></param>
		public ExceptionEventArgs(Exception ex)
		{
			mException = ex;
		}

		private Exception mException;
        /// <summary>
        /// Exception
        /// </summary>
		public Exception Exception
		{
			get{return mException;}
		}
	}
	/// <summary>
	/// Common EventHandler class used to raise events with an Exception associated
	/// </summary>
	public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);
}
