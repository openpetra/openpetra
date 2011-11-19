/*
 * >>>> Describe the functionality of this file. <<<<
 *
 * Comment: >>>> Optional comment. <<<<
 *
 * Author:  >>>> Put your full name here <<<<
 *
 * Version: $Revision: 1.3 $ / $Date: 2008/11/27 11:47:02 $
 */

namespace ControlTestBench
{
    partial class ShepherdTest
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
        	this.tPnlCollapsible1 = new Ict.Common.Controls.TPnlCollapsible();
        	this.SuspendLayout();
        	// 
        	// tPnlCollapsible1
        	// 
        	this.tPnlCollapsible1.AutoSize = true;
        	this.tPnlCollapsible1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        	this.tPnlCollapsible1.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
        	this.tPnlCollapsible1.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        	this.tPnlCollapsible1.IsCollapsed = false;
        	this.tPnlCollapsible1.Location = new System.Drawing.Point(9, 18);
        	this.tPnlCollapsible1.Margin = new System.Windows.Forms.Padding(0);
        	this.tPnlCollapsible1.Name = "tPnlCollapsible1";
        	this.tPnlCollapsible1.Size = new System.Drawing.Size(290, 332);
        	this.tPnlCollapsible1.TabIndex = 0;
        	this.tPnlCollapsible1.TaskListInstance = null;
        	this.tPnlCollapsible1.TaskListNode = null;
        	this.tPnlCollapsible1.UserControlClass = "";
        	this.tPnlCollapsible1.UserControlNamespace = "";
        	this.tPnlCollapsible1.VisualStyle = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
        	// 
        	// ShepherdTest
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(511, 359);
        	this.Controls.Add(this.tPnlCollapsible1);
        	this.Name = "ShepherdTest";
        	this.Text = "ShepherdTest";
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private Ict.Common.Controls.TPnlCollapsible tPnlCollapsible1;
    }
}
