
using System;
using SourceGrid.Examples.Threading;

namespace WindowsFormsSample.GridSamples.PingGrids
{
	public class RowInsertCounter
	{
		private DateTime? started = null;
		ProgressBarForm form = null;
		
		public RowInsertCounter(ProgressBarForm form)
		{
			this.form = form;
		}
		
		private int TimeDifference
		{
			get
			{
				var now = DateTime.Now;
				TimeSpan diff = now - started.Value;
				var seconds = diff.Seconds + diff.Minutes * 60 + diff.Hours * 60 * 60;
				return seconds;
			}
		}
		
		
		public void Add(object s, IUpdateStatus status)
		{
			if (started == null)
				started = DateTime.Now;
			
			if (TimeDifference < 1)
			{
				this.form.Label.Text = "Estimating...";
				return;
			}
			
			var ratio = status.Current / TimeDifference;
			
			this.form.Label.Text = string.Format("Inserted {0} / {1} rows. Inserting at {2} per second", 
			                                     status.Current, status.MaxValue,
			                                    ratio);
			
		}
	}
}
