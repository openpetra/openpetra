// auto generated with nant generateWinforms from UC_CostCentreSettings.yaml
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    partial class TFrmUC_CostCentreSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_CostCentreSettings));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrCostCentreOptions = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtSelectedCostCentres = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.clbCostCentres = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllCostCentres = new System.Windows.Forms.Button();
            this.rbtAllCostCentres = new System.Windows.Forms.RadioButton();
            this.rbtAllActiveCostCentres = new System.Windows.Forms.RadioButton();
            this.rbtAccountLevel = new System.Windows.Forms.RadioButton();
            this.chkExcludeInactiveCostCentres = new System.Windows.Forms.CheckBox();
            this.chkCostCentreBreakdown = new System.Windows.Forms.CheckBox();
            this.rgrDepth = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtDetail = new System.Windows.Forms.RadioButton();
            this.rbtStandard = new System.Windows.Forms.RadioButton();
            this.rbtSummary = new System.Windows.Forms.RadioButton();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrCostCentreOptions.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.rgrDepth.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();

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
            // rgrCostCentreOptions
            //
            this.rgrCostCentreOptions.Location = new System.Drawing.Point(2,2);
            this.rgrCostCentreOptions.Name = "rgrCostCentreOptions";
            this.rgrCostCentreOptions.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrCostCentreOptions.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtSelectedCostCentres
            //
            this.rbtSelectedCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtSelectedCostCentres.Name = "rbtSelectedCostCentres";
            this.rbtSelectedCostCentres.AutoSize = true;
            this.rbtSelectedCostCentres.Text = "Selected Cost Centres";
            this.rbtSelectedCostCentres.Checked = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            //
            // clbCostCentres
            //
            this.clbCostCentres.Name = "clbCostCentres";
            this.clbCostCentres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbCostCentres.Size = new System.Drawing.Size(365, 100);
            this.clbCostCentres.FixedRows = 1;
            //
            // btnUnselectAllCostCentres
            //
            this.btnUnselectAllCostCentres.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllCostCentres.Name = "btnUnselectAllCostCentres";
            this.btnUnselectAllCostCentres.AutoSize = true;
            this.btnUnselectAllCostCentres.Click += new System.EventHandler(this.UnselectAll);
            this.btnUnselectAllCostCentres.Text = "Unselect All";
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.clbCostCentres, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnUnselectAllCostCentres, 1, 0);
            this.rbtSelectedCostCentres.CheckedChanged += new System.EventHandler(this.rbtSelectedCostCentresCheckedChanged);
            //
            // rbtAllCostCentres
            //
            this.rbtAllCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtAllCostCentres.Name = "rbtAllCostCentres";
            this.rbtAllCostCentres.AutoSize = true;
            this.rbtAllCostCentres.Text = "All Cost Centres";
            //
            // rbtAllActiveCostCentres
            //
            this.rbtAllActiveCostCentres.Location = new System.Drawing.Point(2,2);
            this.rbtAllActiveCostCentres.Name = "rbtAllActiveCostCentres";
            this.rbtAllActiveCostCentres.AutoSize = true;
            this.rbtAllActiveCostCentres.Text = "All Active Cost Centres";
            //
            // rbtAccountLevel
            //
            this.rbtAccountLevel.Location = new System.Drawing.Point(2,2);
            this.rbtAccountLevel.Name = "rbtAccountLevel";
            this.rbtAccountLevel.AutoSize = true;
            this.rbtAccountLevel.Text = "Account Level";
            //
            // chkExcludeInactiveCostCentres
            //
            this.chkExcludeInactiveCostCentres.Location = new System.Drawing.Point(2,2);
            this.chkExcludeInactiveCostCentres.Name = "chkExcludeInactiveCostCentres";
            this.chkExcludeInactiveCostCentres.AutoSize = true;
            this.chkExcludeInactiveCostCentres.CheckedChanged += new System.EventHandler(this.chkExcludeCostCentresChanged);
            this.chkExcludeInactiveCostCentres.Text = "Exclude inactive Cost Centres";
            this.chkExcludeInactiveCostCentres.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkExcludeInactiveCostCentres.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtSelectedCostCentres, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.rbtAllCostCentres, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtAllCostCentres, 0, 1);
            this.tableLayoutPanel2.SetColumnSpan(this.rbtAllActiveCostCentres, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtAllActiveCostCentres, 0, 2);
            this.tableLayoutPanel2.SetColumnSpan(this.rbtAccountLevel, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtAccountLevel, 0, 3);
            this.tableLayoutPanel2.SetColumnSpan(this.chkExcludeInactiveCostCentres, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkExcludeInactiveCostCentres, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.rgrCostCentreOptions.Text = "Cost Centre Options";
            //
            // chkCostCentreBreakdown
            //
            this.chkCostCentreBreakdown.Location = new System.Drawing.Point(2,2);
            this.chkCostCentreBreakdown.Name = "chkCostCentreBreakdown";
            this.chkCostCentreBreakdown.AutoSize = true;
            this.chkCostCentreBreakdown.Text = "Cost Centre Breakdown";
            this.chkCostCentreBreakdown.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkCostCentreBreakdown.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            //
            // rgrDepth
            //
            this.rgrDepth.Name = "rgrDepth";
            this.rgrDepth.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrDepth.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.rgrDepth.Controls.Add(this.tableLayoutPanel4);
            //
            // rbtDetail
            //
            this.rbtDetail.Location = new System.Drawing.Point(2,2);
            this.rbtDetail.Name = "rbtDetail";
            this.rbtDetail.AutoSize = true;
            this.rbtDetail.Text = "detail";
            this.rbtDetail.Checked = true;
            //
            // rbtStandard
            //
            this.rbtStandard.Location = new System.Drawing.Point(2,2);
            this.rbtStandard.Name = "rbtStandard";
            this.rbtStandard.AutoSize = true;
            this.rbtStandard.Text = "standard";
            //
            // rbtSummary
            //
            this.rbtSummary.Location = new System.Drawing.Point(2,2);
            this.rbtSummary.Name = "rbtSummary";
            this.rbtSummary.AutoSize = true;
            this.rbtSummary.Text = "summary";
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.rbtDetail, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtStandard, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.rbtSummary, 0, 2);
            this.rgrDepth.Text = "Depth";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.rgrCostCentreOptions, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkCostCentreBreakdown, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rgrDepth, 0, 2);

            //
            // TFrmUC_CostCentreSettings
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_CostCentreSettings";
            this.Text = "";

            this.tableLayoutPanel4.ResumeLayout(false);
            this.rgrDepth.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrCostCentreOptions.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox rgrCostCentreOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtSelectedCostCentres;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Common.Controls.TClbVersatile clbCostCentres;
        private System.Windows.Forms.Button btnUnselectAllCostCentres;
        private System.Windows.Forms.RadioButton rbtAllCostCentres;
        private System.Windows.Forms.RadioButton rbtAllActiveCostCentres;
        private System.Windows.Forms.RadioButton rbtAccountLevel;
        private System.Windows.Forms.CheckBox chkExcludeInactiveCostCentres;
        private System.Windows.Forms.CheckBox chkCostCentreBreakdown;
        private System.Windows.Forms.GroupBox rgrDepth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton rbtDetail;
        private System.Windows.Forms.RadioButton rbtStandard;
        private System.Windows.Forms.RadioButton rbtSummary;
    }
}
