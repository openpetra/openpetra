/* auto generated with nant generateWinforms from {#XAMLSRCFILE} 
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
{#GPLFILEHEADER}
using System;
using System.Windows.Forms;

namespace {#NAMESPACE}
{
    partial class {#CLASSNAME}
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
            if (disposing) 
            {
                if (components != null) 
                {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof({#CLASSNAME}));

//        	this.imageListButtons = new System.Windows.Forms.ImageList(this.components);
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.sptNavigation = new System.Windows.Forms.SplitContainer();
        	this.pnlMoreButtons = new System.Windows.Forms.Panel();
        	this.pnlNavigationCaption = new System.Windows.Forms.Panel();
        	this.lblNavigationCaption = new System.Windows.Forms.Label();
        	this.btnCollapseNavigation = new System.Windows.Forms.Button();
            {#CONTROLCREATION}

        	this.pnlNavigation.SuspendLayout();
        	this.sptNavigation.Panel1.SuspendLayout();
        	this.sptNavigation.Panel2.SuspendLayout();
        	this.sptNavigation.SuspendLayout();
            {#SUSPENDLAYOUT}
        	this.SuspendLayout();

        	// 
        	// lblNavigationCaption
        	// 
        	this.lblNavigationCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.lblNavigationCaption.ForeColor = System.Drawing.Color.Blue;
        	this.lblNavigationCaption.Location = new System.Drawing.Point(3, 9);
        	this.lblNavigationCaption.Name = "lblNavigationCaption";
        	this.lblNavigationCaption.Size = new System.Drawing.Size(153, 23);
        	this.lblNavigationCaption.TabIndex = 0;
        	this.lblNavigationCaption.Text = "Caption";
        	// 
        	// btnCollapseNavigation
        	// 
        	this.btnCollapseNavigation.Dock = System.Windows.Forms.DockStyle.Right;
        	this.btnCollapseNavigation.Location = new System.Drawing.Point(154, 0);
        	this.btnCollapseNavigation.Name = "btnCollapseNavigation";
        	this.btnCollapseNavigation.Size = new System.Drawing.Size(46, 42);
        	this.btnCollapseNavigation.TabIndex = 1;
        	this.btnCollapseNavigation.Text = "<=";
        	this.btnCollapseNavigation.UseVisualStyleBackColor = true;
        	// 
        	// pnlMoreButtons
        	// 
        	this.pnlMoreButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.pnlMoreButtons.Location = new System.Drawing.Point(0, 438);
        	this.pnlMoreButtons.Name = "pnlMoreButtons";
        	this.pnlMoreButtons.Size = new System.Drawing.Size(200, 28);
        	this.pnlMoreButtons.TabIndex = 2;
        	// 
        	// pnlNavigationCaption
        	// 
        	this.pnlNavigationCaption.Dock = System.Windows.Forms.DockStyle.Top;
        	this.pnlNavigationCaption.Location = new System.Drawing.Point(0, 0);
        	this.pnlNavigationCaption.Name = "pnlNavigationCaption";
        	this.pnlNavigationCaption.Size = new System.Drawing.Size(200, 42);
        	this.pnlNavigationCaption.TabIndex = 7;
        	this.pnlNavigationCaption.Controls.Add(this.btnCollapseNavigation);
        	this.pnlNavigationCaption.Controls.Add(this.lblNavigationCaption);
        	// 
        	// pnlNavigation
        	// 
        	this.pnlNavigation.Controls.Add(this.sptNavigation);
        	this.pnlNavigation.Controls.Add(this.pnlMoreButtons);
        	this.pnlNavigation.Controls.Add(this.pnlNavigationCaption);
        	this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
        	this.pnlNavigation.Location = new System.Drawing.Point(0, 0);
        	this.pnlNavigation.Name = "pnlNavigation";
        	this.pnlNavigation.Size = new System.Drawing.Size(200, 466);
        	this.pnlNavigation.TabIndex = 0;
        	// 
        	// sptNavigation
        	// 
        	this.sptNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.sptNavigation.Location = new System.Drawing.Point(0, 42);
        	this.sptNavigation.Name = "sptNavigation";
        	this.sptNavigation.Orientation = System.Windows.Forms.Orientation.Horizontal;
        	// 
        	// sptNavigation.Panel1
        	// 
        	this.sptNavigation.Panel1.AutoScroll = true;
            {#ADDNAVIGATIONPANELS}
        	// 
        	// sptNavigation.Panel2
        	// 
            {#ADDNAVIGATIONBUTTONS}
        	this.sptNavigation.Size = new System.Drawing.Size(200, 396);
        	this.sptNavigation.SplitterDistance = 210;
        	this.sptNavigation.TabIndex = 6;
        	// 
        	// imageListButtons
        	// 
//        	this.imageListButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListButtons.ImageStream")));
//        	this.imageListButtons.TransparentColor = System.Drawing.Color.Transparent;
            {#IMAGEBUTTONSSETKEYNAME}
            {#CONTROLINITIALISATION}

            //
            // {#CLASSNAME} 
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size({#FORMSIZE});
        	this.Controls.Add(this.pnlNavigation);
            this.Name = "{#CLASSNAME}";
            this.Text = "{#FORMTITLE}";

        	this.pnlNavigation.ResumeLayout(false);
        	this.sptNavigation.Panel1.ResumeLayout(false);
        	this.sptNavigation.Panel2.ResumeLayout(false);
        	this.sptNavigation.ResumeLayout(false);
            {#RESUMELAYOUT}
            this.ResumeLayout(false);
            this.PerformLayout();
        }
//        private System.Windows.Forms.ImageList imageListButtons;
        private System.Windows.Forms.Label lblNavigationCaption;
        private System.Windows.Forms.Button btnCollapseNavigation;
        private System.Windows.Forms.SplitContainer sptNavigation;
        private System.Windows.Forms.Panel pnlNavigationCaption;
        private System.Windows.Forms.Panel pnlMoreButtons;
        private System.Windows.Forms.Panel pnlNavigation;
        {#CONTROLDECLARATION}
    }
}