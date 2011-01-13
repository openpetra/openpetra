/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 14:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ControlTestBench
{
	partial class TaskListCheck
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.taskList1 = new Ict.Common.Controls.TaskList();
			this.SuspendLayout();
			// 
			// taskList1
			// 
			this.taskList1.BackColor = System.Drawing.SystemColors.Desktop;
			this.taskList1.Location = new System.Drawing.Point(83, 53);
			this.taskList1.Name = "taskList1";
			this.taskList1.Size = new System.Drawing.Size(150, 150);
			this.taskList1.TabIndex = 0;
			// 
			// TaskListCheck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.taskList1);
			this.Name = "TaskListCheck";
			this.Text = "TaskListCheck";
			this.ResumeLayout(false);
		}
		private Ict.Common.Controls.TaskList taskList1;
	}
}
