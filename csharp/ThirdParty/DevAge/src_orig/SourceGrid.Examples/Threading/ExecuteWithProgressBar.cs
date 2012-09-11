
using System;
using System.ComponentModel;
using System.Threading;

namespace SourceGrid.Examples.Threading
{
	public class DelegateAsynchronousOperation: AsynchroniousOperation, IProgressCommand
	{
		public event OnUpdateHandler Progress;
		private AsyncrhonousOperationHandler m_handler;
		public void OnProgress(IUpdateStatus status)
		{
			lock(this)
			{
				FireAsync(this.Progress, this, status);
			}
		}
		
		public DelegateAsynchronousOperation(
			ISynchronizeInvoke target,
			AsyncrhonousOperationHandler handler)
			:base(target)
		{
			this.m_handler = handler;
		}
		
		public delegate void AsyncrhonousOperationHandler(DelegateAsynchronousOperation operation);
		
		protected override void DoWork()
		{
			m_handler.Invoke(this);
		}
	}
	
	
	
	public class ExecuteWithProgressBar
	{
		private IProgressCommand m_progressCommand = null;
		private IProgressBarForm m_form = null;
		
		public ExecuteWithProgressBar(IProgressCommand progressCommand,
		                              IProgressBarForm form)
		{
			this.m_progressCommand = progressCommand;
			m_form = form;
		}
		
		public void Run()
		{
			m_progressCommand.Failed += delegate(object sender, ThreadExceptionEventArgs e)
			{
				throw new Exception(string.Format(
					"Error executing asynchronious command with progress bar"), e.Exception);
			};
			
			m_progressCommand.Progress += delegate (object sender, IUpdateStatus status)
			{
				m_form.ProgressBar.Maximum = status.MaxValue;
				m_form.ProgressBar.Value = status.Current;
			};
			
			m_progressCommand.Completed += delegate
			{
				CloseForm();
			};
			
			m_form.Cancelled += delegate
			{
				m_progressCommand.CancelAndWait();
				CloseForm();
			};
			
			m_progressCommand.Start();
			m_form.Form.ShowDialog();
		}
		
		private void CloseForm()
		{
			m_form.Form.Close();
		}
	}
}
