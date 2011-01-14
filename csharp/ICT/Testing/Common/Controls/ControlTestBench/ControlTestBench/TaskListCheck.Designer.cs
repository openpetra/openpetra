/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 14:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
 using System.Xml;
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
		private void InitializeComponent(XmlNode node, Ict.Common.Controls.TVisualStylesEnum Style)
		{
			this.taskList1 = new Ict.Common.Controls.TTaskList(node,Style);
			//this.taskList1.VisualStyle = new Ict.Common.Controls.TVisualStyles(Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel);
			this.container = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// taskList1
			// 
			this.taskList1.BackColor = System.Drawing.SystemColors.AppWorkspace;
//			this.taskList1.Location = new System.Drawing.Point(83, 53);
			this.taskList1.Name = "taskList1";
//			this.taskList1.Size = new System.Drawing.Size(150, 150);
			this.taskList1.TabIndex = 0;
			// 
			// taskList1
			// 
			
			this.container.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.container.Location = new System.Drawing.Point(83, 53);
			this.container.Name = "taskListContainer";
			this.container.AutoSize = true;
			this.container.Size = new System.Drawing.Size(150, 150);
//			this.container.AutoScroll = true;
			this.container.TabIndex = 0;
			
			// 
			// TaskListCheck
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.container.Controls.Add(this.taskList1);
			this.Controls.Add(this.container);
			this.Name = "TaskListCheck";
			this.Text = "TaskListCheck";
			this.ResumeLayout(false);
		}
		private Ict.Common.Controls.TTaskList taskList1;
		private System.Windows.Forms.Panel container;
	}
}
