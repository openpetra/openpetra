// auto generated with nant generateWinforms from {#XAMLSRCFILE}
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

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
        {#IFDEF CUSTOMDISPOSING}

        
           /*
            * The following Dispose calls are necessary to get dynamically created Controls out
            * of memory if they were created in the Constructor (e.g. because of running on
            * 'Large Fonts (120 DPI)'. 
            */        
            {#CUSTOMDISPOSING}
        {#ENDIF CUSTOMDISPOSING}
        
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            {#IFDEF RESOURCES}
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof({#CLASSNAME}));

            {#ENDIF RESOURCES}
            {#CONTROLCREATION}

            {#SUSPENDLAYOUT}

            {#CONTROLINITIALISATION}

            //
            // {#CLASSNAME} 
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size({#FORMSIZE});

            {#ADDMAINCONTROLS}

            this.Name = "{#CLASSNAME}";
            this.Text = "{#FORMTITLE}";

	    {#CLASSEVENTHANDLER}
	    
            {#RESUMELAYOUT}

            this.ResumeLayout(false);
            this.PerformLayout();
        }
        
        {#CONTROLDECLARATION}
    }
}