using System;

namespace DevAge.Configuration
{
	[Serializable]
	public class ConfigurationException : DevAgeApplicationException
	{
		public ConfigurationException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public ConfigurationException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
#if !MINI
		protected ConfigurationException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext): 
			base(p_Info, p_StreamingContext)
		{
		}
#endif
	}
}
