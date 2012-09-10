using System;

namespace DevAge.Patterns
{
	/// <summary>
	/// Interface to receive the callback for the activity events.
	/// </summary>
	public interface IActivityEvents
	{
		/// <summary>
		/// Fired when the activity is started
		/// </summary>
		/// <param name="sender">Activity that have sended the event</param>
		void ActivityStarted(IActivity sender);

		/// <summary>
		/// Fired when the activity is completed
		/// </summary>
		/// <param name="sender">Activity that have sended the event</param>
		void ActivityCompleted(IActivity sender);

		/// <summary>
		/// Fired when the activity or one of its children throws an exception
		/// </summary>
		/// <param name="sender">Activity that have sended the event</param>
		/// <param name="exception"></param>
		void ActivityException(IActivity sender, Exception exception);
	}

	public class ActivityEventArgs : EventArgs
	{
		private IActivity mActivity;
		public ActivityEventArgs(IActivity activity)
		{
			mActivity = activity;
		}

		public IActivity Activity
		{
			get{return mActivity;}
			set{mActivity = value;}
		}
	}

	public delegate void ActivityEventHandler(object sender, ActivityEventArgs e);

	public class ActivityExceptionEventArgs : ActivityEventArgs
	{
		private Exception mException;
		public ActivityExceptionEventArgs(IActivity activity, Exception exception):base(activity)
		{
			mException = exception;
		}

		public Exception Exception
		{
			get{return mException;}
			set{mException = value;}
		}
	}

	public delegate void ActivityExceptionEventHandler(object sender, ActivityExceptionEventArgs e);
}
