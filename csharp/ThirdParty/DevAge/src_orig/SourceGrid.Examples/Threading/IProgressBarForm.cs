
using System;
using System.Windows.Forms;

namespace SourceGrid.Examples.Threading
{
	public interface IProgressBarForm
	{
		ProgressBar ProgressBar {get;}
		
		/// <summary>
		/// This 
		/// </summary>
		event EventHandler Cancelled;
		
		Form Form {get;}
	}
}
