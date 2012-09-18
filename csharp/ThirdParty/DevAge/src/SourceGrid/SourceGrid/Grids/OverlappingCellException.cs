using System;

namespace SourceGrid
{
	public class OverlappingCellException : SourceGridException
	{
		public OverlappingCellException(string p_strErrDescription):
			base(p_strErrDescription)
		{
		}
		public OverlappingCellException(string p_strErrDescription, Exception p_InnerException):
			base(p_strErrDescription, p_InnerException)
		{
		}
	}
}
