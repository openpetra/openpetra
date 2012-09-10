using System;
using System.Drawing;
using System.Windows.Forms;

namespace SourceGrid.Examples.Threading
{
	public partial class ProgressBarForm : Form, IProgressBarForm
	{
		public event EventHandler Cancelled;
		
		protected virtual void OnCancelled(EventArgs e)
		{
			if (Cancelled != null) {
				Cancelled(this, e);
			}
		}
		
		public Form Form 
		{
			get 
			{
				return this;
			}
		}
		
		/// <summary>
		/// Set custom text for the progress bar form
		/// </summary>
		public System.Windows.Forms.Label Label {
			get { return label; }
		}
		
		public System.Windows.Forms.ProgressBar ProgressBar {
			get { return progressBar1; }
		}
		
		public ProgressBarForm(string caption)
			:this()
		{
			this.Text = caption;
		}
		
		public ProgressBarForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.progressBar1.Value = 0;
			this.label.Text = string.Empty;
			
			SetUpCancellHandler();
		}
		
		
		
		private void SetUpCancellHandler()
		{
			this.buttonCancel.Click += delegate 
			{  
				OnCancelled(EventArgs.Empty);
			};
		}
	}
}
