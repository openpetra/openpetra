using System;

namespace DevAge.Patterns
{
	/// <summary>
	/// An interface to represents a basic activity.
	/// </summary>
	public interface IActivity
	{
		/// <summary>
		/// Start the activity and all sub activities.
		/// </summary>
		/// <param name="events">Interface class that receive the events, can be null if no event class is needed.</param>
		void Start(IActivityEvents events);
		/// <summary>
		/// Cancel the current activity and all sub activities throwing a ActivityCanceledException.
		/// </summary>
		void Cancel();
		
		/// <summary>
		/// Subordinated activities. Are executed after the current activity. If one of these activity throws an exception is propagated to parent activity and the operation is stopped.
		///  Note that the sub activities can be async and so can be executed in a parellel mode.
		/// </summary>
		ActivityCollection SubActivities
		{
			get;
		}

		/// <summary>
		/// Activity status
		/// </summary>
		ActivityStatus Status
		{
			get;
		}

		/// <summary>
		/// Name of the activity used to describe the class.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the WaitHandle class used to wait for the completition of the activity.
		/// </summary>
		System.Threading.WaitHandle WaitHandle
		{
			get;
		}

		/// <summary>
		/// Gets the exception throwed when the activity fail. Null if no exception.
		/// </summary>
		Exception Exception
		{
			get;
		}

		/// <summary>
		/// Gets or sets the Activity parent. Null when it is a root activity.
		/// Do not set manually the parent activity, but simply add the activity to the SubActivities collection.
		/// </summary>
		IActivity Parent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the activity full name of the activity, composed by the full name of the parent activity separated with a \ character
		/// </summary>
		string FullName
		{
			get;
		}
	}

	public enum ActivityStatus
	{
		Pending,
		Running,
		Completed,
		Exception
	}


	public enum SubActivityWaitMode
	{
		/// <summary>
		/// Don't wait the sub activities to finish (parallel processing, asyncronous)
		/// </summary>
		DoNotWait,
		/// <summary>
		/// Wait for each sub activities (syncronous)
		/// </summary>
		WaitOnEach,
		/// <summary>
		/// Wait all the sub activities at the end (parallel processing but syncronized with the parent)
		/// </summary>
		WaitAtTheEnd
	}

	/// <summary>
	/// Base activity class. Override the OnWork method to customize the activity.
	/// </summary>
	public abstract class ActivityBase : IActivity
	{
		private ActivityCollection mSubActivities;
		private bool mIsCanceled = false;
		private ActivityStatus mStatus = ActivityStatus.Pending;
		private IActivityEvents mEvents;
		private string mName;
		private int mSubActivitiesTimeOut = System.Threading.Timeout.Infinite;
		/// <summary>
		/// An activity count used only to generate a seguential name
		/// </summary>
		private static int mActivityCount = 0;
		private SubActivityWaitMode mSubActivityWaitMode = SubActivityWaitMode.WaitOnEach;
		private bool mPropagateException = true;
		private Exception mException;

		private IActivity mParent;

		/// <summary>
		/// Initially set to signaled=true=completed, nonsignaled=false=notcompleted,
		/// </summary>
		private System.Threading.ManualResetEvent mWaitHandle = new System.Threading.ManualResetEvent(true);

		/// <summary>
		/// Constructor
		/// </summary>
		public ActivityBase()
		{
			mActivityCount++;
			Name = "Activity " + mActivityCount.ToString();

			mSubActivities = new ActivityCollection(this);
		}


		/// <summary>
		/// Gets or sets the time to wait for the sub activities. If the operation is still executing then a TimeOutException is fired. Default is System.Threading.Timeout.Infinite. Default is true.
		/// </summary>
		public int SubActivitiesTimeOut
		{
			get{return mSubActivitiesTimeOut;}
			set{mSubActivitiesTimeOut = value;}
		}

		/// <summary>
		/// Gets or sets if propagate an exception from sub activities to the current activities. Default is true.
		/// Only valid SubActivitiesTimeOut is WaitOnEach or WaitAtTheEnd
		/// </summary>
		public bool PropagateException
		{
			get{return mPropagateException;}
			set{mPropagateException = value;}
		}

		/// <summary>
		/// Gets or sets how the current activity wait the completition of the sub activities.
		/// </summary>
		public SubActivityWaitMode SubActivityWaitMode
		{
			get{return mSubActivityWaitMode;}
			set{mSubActivityWaitMode = value;}
		}

		/// <summary>
		/// Reset the status property to the original values. If the activity is still running an exception is throw.
		/// </summary>
		protected virtual void ResetRunningStatus()
		{
			if (mStatus == ActivityStatus.Running)
				throw new ActivityStatusNotValidException();

			mIsCanceled = false;
			mStatus = ActivityStatus.Pending;
			mException = null;
			mWaitHandle.Set(); //signaled
		}

		/// <summary>
		/// Working method. Abstract. Override this method to provide a specific work for the activity.
		/// </summary>
		protected abstract void OnWork();

		/// <summary>
		/// Internal work method. Call the OnWork method and Start the SubActivities.
		/// </summary>
		protected void DoWork()
		{
			bool success = false;
			try
			{
				//Call the abstract specific work method
				OnWork();

				//Start the SubActivities
				for (int i = 0; i < SubActivities.Count; i++)
				{
					if (mIsCanceled)
						throw new ActivityCanceledException();

					SubActivities[i].Start(mEvents);

					//Wait the activity
					if (SubActivityWaitMode == SubActivityWaitMode.WaitOnEach)
					{
						WaitActivity(SubActivities[i], SubActivitiesTimeOut);
						CheckActivityException(SubActivities[i]);
					}
				}

				if (SubActivityWaitMode == SubActivityWaitMode.WaitAtTheEnd)
				{
					WaitActivities(SubActivities, SubActivitiesTimeOut);
					if (PropagateException)
						CheckActivitiesException(SubActivities);
				}

				success = true;
			}
			catch(Exception e)
			{
				OnException(e);
			}
			if (success)
				OnCompleted();
		}

		/// <summary>
		/// Wait until or SubActivities are completed. Throw an exception on timeout.
		/// </summary>
		/// <param name="activities"></param>
		/// <param name="timeout"></param>
		public static void WaitActivities(ActivityCollection activities, int timeout)
		{
			for (int i = 0; i < activities.Count; i++)
			{
				WaitActivity(activities[i], timeout);
			}
		}

		/// <summary>
		/// Wait until or SubActivities are completed. Throw an exception on timeout.
		/// </summary>
		/// <param name="activity"></param>
		/// <param name="timeout"></param>
		public static void WaitActivity(IActivity activity, int timeout)
		{
			if (activity.Status == ActivityStatus.Pending)
				throw new DevAgeApplicationException("Activity not started");
			else if (activity.Status == ActivityStatus.Running)
			{
				bool terminated = activity.WaitHandle.WaitOne(timeout, false);
				if (terminated == false)
					throw new TimeOutActivityException();
			}
		}

		/// <summary>
		/// Throw an exception if one of the activities has an exception.
		/// </summary>
		/// <param name="activities"></param>
		public static void CheckActivitiesException(ActivityCollection activities)
		{
			for (int i = 0; i < activities.Count; i++)
			{
				CheckActivityException(activities[i]);
			}
		}
		/// <summary>
		/// Throw an exception if one of the activities has an exception.
		/// </summary>
		/// <param name="activity"></param>
		public static void CheckActivityException(IActivity activity)
		{
			if (activity.Status == ActivityStatus.Exception)
				throw new SubActivityException(activity.Name, activity.Exception);
		}

		/// <summary>
		/// Start the activity. NOTE: Usually don't override this method but override the OnWork method to implement the specific activity work.
		/// </summary>
		protected virtual void StartActivity()
		{
			//Do the internal work
			DoWork();
		}

		protected virtual void OnStarted()
		{
			mWaitHandle.Reset();
			mStatus = ActivityStatus.Running;

			if (mEvents != null)
				mEvents.ActivityStarted(this);
		}
		protected virtual void OnCompleted()
		{
			mStatus = ActivityStatus.Completed;
			mWaitHandle.Set();

			if (mEvents != null)
				mEvents.ActivityCompleted(this);
		}
		protected virtual void OnException(Exception e)
		{
			mStatus = ActivityStatus.Exception;
			mException = e;
			mWaitHandle.Set();

			if (mEvents != null)
				mEvents.ActivityException(this, e);
		}

		#region IActivity
		/// <summary>
		/// Start the activity. If the activity is already running an exception is throw.
		/// To override the common working code use the OnWork abstract method.
		/// </summary>
		/// <param name="events">Interface class that receive the events, can be null if no event class is needed.</param>
		public void Start(IActivityEvents events)
		{
			ResetRunningStatus();

			if (mStatus != ActivityStatus.Pending)
				throw new ActivityStatusNotValidException();

			mEvents = events;

			try
			{
				OnStarted();
				StartActivity();
			}
			catch(Exception e)
			{
				OnException(e);
			}
		}

		/// <summary>
		/// Cancel the current activity and all sub activities throwing a ActivityCanceledException.
		/// </summary>
		public void Cancel()
		{
			mIsCanceled = true;
		}
		
		/// <summary>
		/// Subordinated activities. Are executed after the current activity. If one of these activity throws an exception is propagated to parent activity and the operation is stopped.
		///  Note that the sub activities can be async and so can be executed in a parellel mode.
		/// </summary>
		public ActivityCollection SubActivities
		{
			get{return mSubActivities;}
		}

		/// <summary>
		/// Activity status
		/// </summary>
		public ActivityStatus Status
		{
			get{return mStatus;}
		}

		/// <summary>
		/// Name of the activity used to describe the class.
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}
		/// <summary>
		/// Gets the WaitHandle class used to wait for the completition of the activity.
		/// </summary>
		public virtual System.Threading.WaitHandle WaitHandle
		{
			get{return mWaitHandle;}
		}
		/// <summary>
		/// Gets the exception throwed when the activity fail. Null if no exception.
		/// </summary>
		public Exception Exception
		{
			get{return mException;}
		}
		/// <summary>
		/// Gets or sets the Activity parent. Null when it is a root activity.
		/// Do not set manually the parent activity, but simply add the activity to the SubActivities collection.
		/// </summary>
		public IActivity Parent
		{
			get{return mParent;}
			set
			{
				if (mParent != null && value != null)
					throw new DevAgeApplicationException("Activity already has a parent");

				mParent = value;
			}
		}

		/// <summary>
		/// Gets the activity full name of the activity, composed by the full name of the parent activity separated with a \ character
		/// </summary>
		public string FullName
		{
			get
			{
				if (Parent == null)
					return Name;
				else
					return Parent.Name + "\\" + Name;
			}
		}
		#endregion
	}


	/// <summary>
	/// An activity used as a container for other activities using a syncronized code.
	/// </summary>
	public class Activity : ActivityBase
	{
		protected override void OnWork()
		{
			//Do nothing
		}
	}

	/// <summary>
	/// An activity used as a container for other activities using an asynchronous code.
	/// Override the OnAsyncWork method for custom asynchronous work.
	/// </summary>
	public class AsynchronousActivity : AsyncActivityBase
	{
		private EmptyDelegate asyncDelegate;
		public AsynchronousActivity()
		{
			asyncDelegate = new EmptyDelegate(OnAsyncWork);
		}

		protected override void OnBeginWork(AsyncCallback callback)
		{
			//Chiamo in maniera asincrona proprio la funzione di callback,
			// perchè logicamente questa funzione deve solo chiamare su un altro thread la funzione di callback che a sua volta chiama la complete e le chiamate correlate
			// devo fare un giro su un altro delegate perchè altrimenti creo un deadlock, perchè aspetto sullo stesso thread che deve finire
			asyncDelegate.BeginInvoke(callback, new Object());
		}

		private delegate void EmptyDelegate();
		protected virtual void OnAsyncWork()
		{
			//Do nothing
		}

		protected override void OnEndWork(IAsyncResult asyncResult)
		{
			asyncDelegate.EndInvoke(asyncResult);
		}
	}
}
