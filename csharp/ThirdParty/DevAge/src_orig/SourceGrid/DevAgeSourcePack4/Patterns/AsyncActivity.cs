using System;

namespace DevAge.Patterns
{
	/// <summary>
	/// Interface for asyncronuos activity. Extend the IActivity interface
	/// </summary>
	public interface IAsyncActivity : IActivity
	{
	}

	/// <summary>
	/// Base activity class. Override the OnBeginWork and OnEndWork method to customize the activity.
	/// This class support an asyncronous activity.
	/// </summary>
	public abstract class AsyncActivityBase : ActivityBase
	{
		private IAsyncResult mAsyncResult = null;

		protected override void ResetRunningStatus()
		{
			base.ResetRunningStatus ();

			mAsyncResult = null;
		}

		/// <summary>
		/// Begind working method. Called to start the asyncronous activity. Abstract.
		/// </summary>
		protected abstract void OnBeginWork(AsyncCallback callback);

		/// <summary>
		/// End working method. Called when the asyncronous operation is finished
		/// </summary>
		/// <param name="asyncResult">AsyncResult</param>
		protected abstract void OnEndWork(IAsyncResult asyncResult);

		private void CurrentAsyncCallback(IAsyncResult asyncResult)
		{
			//Note: I can't assign directly the return IAsyncResult to the member variable mAsyncResult in the StartActivity, because there can be situations when the CurrentAsyncCallback is called before the variable receive the value, for this reason I assign the member variable directly in the CurrentAsyncCallback callback
			mAsyncResult = asyncResult;
			if (mAsyncResult == null)
				throw new DevAgeApplicationException("Invalid async activity, IAsyncResult is null");

			//Do the internal work
			DoWork();
		}

		/// <summary>
		/// Working method.
		/// </summary>
		protected override void OnWork()
		{
			//Terminate the asyncronous operation. Note: If an exception is throwed inside the asyncronous operation is catched with this line.
			OnEndWork(mAsyncResult);
		}

		/// <summary>
		/// Start the activity. NOTE: Usually don't override this method but override the OnBeginWork method to implement the specific activity work.
		/// </summary>
		protected override void StartActivity()
		{
			//Note: Don't call the base class because this class start an async operation.
			//base.StartActivity();

			//Note: I can't assign directly the return IAsyncResult to the member variable mAsyncResult, because there can be situations when the CurrentAsyncCallback is called before the variable receive the value, for this reason I assign the member variable directly in the CurrentAsyncCallback callback
			OnBeginWork(new AsyncCallback(CurrentAsyncCallback));
		}
	}
}
