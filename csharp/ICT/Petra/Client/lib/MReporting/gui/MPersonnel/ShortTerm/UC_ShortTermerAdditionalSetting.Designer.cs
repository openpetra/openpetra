// auto generated with nant generateWinforms from UC_ShortTermerAdditionalSetting.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
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
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    partial class TFrmUC_ShortTermerAdditionalSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_ShortTermerAdditionalSetting));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpAdditionalSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkHideEmptyLines = new System.Windows.Forms.CheckBox();
            this.chkPrintTwoLines = new System.Windows.Forms.CheckBox();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpAdditionalSettings.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // grpAdditionalSettings
            //
            this.grpAdditionalSettings.Location = new System.Drawing.Point(2,2);
            this.grpAdditionalSettings.Name = "grpAdditionalSettings";
            this.grpAdditionalSettings.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpAdditionalSettings.Controls.Add(this.tableLayoutPanel2);
            //
            // chkHideEmptyLines
            //
            this.chkHideEmptyLines.Location = new System.Drawing.Point(2,2);
            this.chkHideEmptyLines.Name = "chkHideEmptyLines";
            this.chkHideEmptyLines.AutoSize = true;
            this.chkHideEmptyLines.Text = "Hide empty lines";
            this.chkHideEmptyLines.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkHideEmptyLines.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // chkPrintTwoLines
            //
            this.chkPrintTwoLines.Location = new System.Drawing.Point(2,2);
            this.chkPrintTwoLines.Name = "chkPrintTwoLines";
            this.chkPrintTwoLines.AutoSize = true;
            this.chkPrintTwoLines.Text = "Print result on two lines (Sort by Partner Name)";
            this.chkPrintTwoLines.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkPrintTwoLines.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.chkHideEmptyLines, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkPrintTwoLines, 0, 1);
            this.grpAdditionalSettings.Text = "Additional Settings";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.grpAdditionalSettings, 0, 0);

            //
            // TFrmUC_ShortTermerAdditionalSetting
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_ShortTermerAdditionalSetting";
            this.Text = "";

            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpAdditionalSettings.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpAdditionalSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox chkHideEmptyLines;
        private System.Windows.Forms.CheckBox chkPrintTwoLines;
    }
}
