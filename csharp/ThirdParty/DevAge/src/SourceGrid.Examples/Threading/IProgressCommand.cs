
using System;

namespace SourceGrid.Examples.Threading
{
	

	public delegate void OnUpdateHandler(object sender, IUpdateStatus status);
	
	public interface IProgressCommand
	{
		event OnUpdateHandler Progress;
		void Start();
		
		/// <summary>
		/// Attempt to cancel the current operation.  This returns
		/// immediately to the caller.  No guarantee is made as to
		/// whether the operation will be successfully cancelled.  All
		/// that can be known is that at some point, one of the
		/// three events Completed, Cancelled, or Failed will be raised
		/// at some point.
		/// </summary>
		void Cancel();
		
		/// <summary>
		/// Attempt to cancel the current operation and block until either
		/// the cancellation succeeds or the operation completes.
		/// </summary>
		/// <returns>true if the operation was successfully cancelled
		/// or it failed, false if it ran to completion.</returns>
		bool CancelAndWait();
		
		/// <summary>
		/// Blocks until the operation has either run to completion, or has
		/// been successfully cancelled, or has failed with an internal
		/// exception.
		/// </summary>
		/// <returns>true if the operation completed, false if it was
		/// cancelled before completion or failed with an internal
		/// exception.</returns>
		bool WaitUntilDone();
		
		/// <summary>
		/// Returns false if the operation is still in progress, or true if
		/// it has either completed successfully, been cancelled
		///  successfully, or failed with an internal exception.
		/// </summary>
		bool IsDone {get;}
		
		/// <summary>
		/// This event will be fired if the operation runs to completion
		/// without being cancelled.  This event will be raised through the
		/// ISynchronizeTarget supplied at construction time.  Note that
		/// this event may still be received after a cancellation request
		/// has been issued.  (This would happen if the operation completed
		/// at about the same time that cancellation was requested.)  But
		/// the event is not raised if the operation is cancelled
		/// successfully.
		/// </summary>
		event EventHandler Completed;
		
		
		/// <summary>
		/// This event will be fired when the operation is successfully
		/// stoped through cancellation.  This event will be raised through
		/// the ISynchronizeTarget supplied at construction time.
		/// </summary>
		event EventHandler Cancelled;
		
		
		/// <summary>
		/// This event will be fired if the operation throws an exception.
		/// This event will be raised through the ISynchronizeTarget
		/// supplied at construction time.
		/// </summary>
		event System.Threading.ThreadExceptionEventHandler Failed;
		
		
		
	}
}
