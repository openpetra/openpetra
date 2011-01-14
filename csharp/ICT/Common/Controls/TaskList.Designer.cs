/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 11:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Ict.Common.Controls
{
	partial class TaskList
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.pnlModule = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// pnlModule
			// 
			this.pnlModule.AccessibleName = "pnlModule";
			this.pnlModule.Location = new System.Drawing.Point(3, 3);
			this.pnlModule.Name = "pnlModule";
			this.pnlModule.Size = new System.Drawing.Size(114, 144);
			this.pnlModule.TabIndex = 0;
			// 
			// TaskList
			// 
			this.AccessibleName = "pnlModule";
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlModule);
			this.Name = "TaskList";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Panel pnlModule;
	}
}
