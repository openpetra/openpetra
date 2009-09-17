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

            this.dsbContent = new Ict.Common.Controls.TDashboard();
            this.lstFolders = new Ict.Common.Controls.TLstFolderNavigation();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();
            {#CONTROLCREATION}

            this.stbMain.SuspendLayout();
            this.dsbContent.SuspendLayout();
            this.lstFolders.SuspendLayout();
            {#SUSPENDLAYOUT}
        	this.SuspendLayout();

            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;
        	// 
        	// lstFolders
        	// 
        	this.lstFolders.Dock = System.Windows.Forms.DockStyle.Left;
        	this.lstFolders.Location = new System.Drawing.Point(0, 0);
        	this.lstFolders.Name = "lstFolders";
        	this.lstFolders.Size = new System.Drawing.Size(200, 466);
        	this.lstFolders.TabIndex = 0;
        	// 
        	// dsbContent
        	// 
        	this.dsbContent.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.dsbContent.Name = "dsbContent";
            {#CONTROLINITIALISATION}

            //
            // {#CLASSNAME} 
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size({#FORMSIZE});
            this.Controls.Add(dsbContent);
        	this.Controls.Add(this.lstFolders);
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "{#CLASSNAME}";
            this.Text = "{#FORMTITLE}";

            this.stbMain.ResumeLayout(false);
        	this.lstFolders.ResumeLayout(false);
            this.dsbContent.ResumeLayout(false);
            {#RESUMELAYOUT}
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Ict.Common.Controls.TLstFolderNavigation lstFolders;
        private Ict.Common.Controls.TDashboard dsbContent;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
        {#CONTROLDECLARATION}
    }
}