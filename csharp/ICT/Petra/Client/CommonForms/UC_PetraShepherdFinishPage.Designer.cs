// auto generated with nant generateWinforms from UC_PetraShepherdFinishPage.yaml
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
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.CommonForms
{
    partial class TUC_PetraShepherdFinishPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PetraShepherdFinishPage));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpSummary = new System.Windows.Forms.GroupBox();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSummaryText1 = new System.Windows.Forms.Label();
            this.lblSummaryText2 = new System.Windows.Forms.Label();
            this.chkFurtherActionOnFinish = new System.Windows.Forms.CheckBox();

            this.pnlContent.SuspendLayout();
            this.grpSummary.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpSummary);
            this.pnlContent.Controls.Add(this.chkFurtherActionOnFinish);
            //
            // grpSummary
            //
            this.grpSummary.Name = "grpSummary";
            this.grpSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSummary.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
            this.grpSummary.AutoSize = true;
            this.grpSummary.Controls.Add(this.pnlSummary);
            //
            // pnlSummary
            //
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSummary.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlSummary.BackColor = System.Drawing.Color.White;
            this.pnlSummary.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlSummary.Controls.Add(this.tableLayoutPanel1);
            //
            // lblSummaryText1
            //
            this.lblSummaryText1.Location = new System.Drawing.Point(2,2);
            this.lblSummaryText1.Name = "lblSummaryText1";
            this.lblSummaryText1.AutoSize = true;
            this.lblSummaryText1.Text = "Summary Text1:";
            this.lblSummaryText1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // lblSummaryText2
            //
            this.lblSummaryText2.Location = new System.Drawing.Point(2,2);
            this.lblSummaryText2.Name = "lblSummaryText2";
            this.lblSummaryText2.AutoSize = true;
            this.lblSummaryText2.Text = "Summary Text2:";
            this.lblSummaryText2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblSummaryText1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSummaryText2, 0, 1);
            this.grpSummary.Text = "Summary";
            //
            // chkFurtherActionOnFinish
            //
            this.chkFurtherActionOnFinish.Name = "chkFurtherActionOnFinish";
            this.chkFurtherActionOnFinish.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkFurtherActionOnFinish.Visible = false;
            this.chkFurtherActionOnFinish.Padding = new System.Windows.Forms.Padding(5, 8, 5, 3);
            this.chkFurtherActionOnFinish.AutoSize = true;
            this.chkFurtherActionOnFinish.Text = "Further action on finish";
            this.chkFurtherActionOnFinish.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkFurtherActionOnFinish.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);

            //
            // TUC_PetraShepherdFinishPage
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_PetraShepherdFinishPage";
            this.Text = "";

            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlSummary.ResumeLayout(false);
            this.grpSummary.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSummaryText1;
        private System.Windows.Forms.Label lblSummaryText2;
        private System.Windows.Forms.CheckBox chkFurtherActionOnFinish;
    }
}
