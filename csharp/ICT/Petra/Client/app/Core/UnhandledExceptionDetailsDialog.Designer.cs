// auto generated with nant generateWinforms from UnhandledExceptionDetailsDialog.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Windows.Forms;
using Ict.Common.Controls;

namespace Ict.Petra.Client.App.Core
{
    partial class TFrmUnhandledExceptionDetailsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUnhandledExceptionDetailsDialog));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlErrorDetails = new System.Windows.Forms.Panel();
            this.txtErrorDetails = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.pnlErrorDetails.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.pnlErrorDetails);
            this.pnlContent.Controls.Add(this.pnlButtons);
            //
            // pnlErrorDetails
            //
            this.pnlErrorDetails.Name = "pnlErrorDetails";
            this.pnlErrorDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlErrorDetails.AutoSize = true;
            this.pnlErrorDetails.Controls.Add(this.txtErrorDetails);
            //
            // txtErrorDetails
            //
            this.txtErrorDetails.Name = "txtErrorDetails";
            this.txtErrorDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrorDetails.Size = new System.Drawing.Size(540, 374);
            this.txtErrorDetails.ReadOnly = true;
            this.txtErrorDetails.TabStop = false;
            this.txtErrorDetails.Multiline = true;
            this.txtErrorDetails.ScrollBars = ScrollBars.Vertical;
            //
            // pnlButtons
            //
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel1);
            //
            // btnCopyToClipboard
            //
            this.btnCopyToClipboard.Location = new System.Drawing.Point(2,2);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.AutoSize = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            this.btnCopyToClipboard.Text = "Copy To Clipboard";
            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(2,2);
            this.btnOK.Name = "btnOK";
            this.btnOK.AutoSize = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnOK.Text = "OK";
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 25));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 33));
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnCopyToClipboard, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 0);
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmUnhandledExceptionDetailsDialog
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(600, 430);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmUnhandledExceptionDetailsDialog";
            this.Text = "Error Details - OpenPetra";

            this.Load += new System.EventHandler(this.Form_Load);

            this.stbMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlErrorDetails.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlErrorDetails;
        private System.Windows.Forms.TextBox txtErrorDetails;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.Button btnOK;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
