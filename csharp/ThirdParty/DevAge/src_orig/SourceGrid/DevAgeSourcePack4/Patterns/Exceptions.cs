using System;

namespace DevAge.Patterns
{
	/// <summary>
	/// Exception fired when canceling an activity with the Cancel method.
	/// </summary>
	[Serializable]
	public class ActivityCanceledException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public ActivityCanceledException():
			base("Activity canceled.")
		{
		}
	}


	/// <summary>
	/// Exception fired when canceling an activity with the Cancel method.
	/// </summary>
	[Serializable]
	public class ActivityStatusNotValidException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public ActivityStatusNotValidException():
			base("Activity status not valid.")
		{
		}
	}

	/// <summary>
	/// Exception fired when a time out is encountered.
	/// </summary>
	[Serializable]
	public class TimeOutActivityException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public TimeOutActivityException():
			base("Activity timeout.")
		{
		}
	}


	/// <summary>
	/// Exception fired when a time out is encountered.
	/// </summary>
	[Serializable]
	public class SubActivityException : DevAgeApplicationException
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityName"></param>
        /// <param name="innerException"></param>
		public SubActivityException(string activityName, Exception innerException):
			base("The activity " + activityName + " throwed an exception, " + innerException.Message, innerException)
		{
		}
	}
}
