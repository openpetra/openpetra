// auto generated with nant generateWinforms from UC_GeneralSettings.yaml
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
    partial class TFrmUC_GeneralSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_GeneralSettings));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpLedger = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedger = new System.Windows.Forms.TextBox();
            this.lblLedger = new System.Windows.Forms.Label();
            this.cmbAccountHierarchy = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAccountHierarchy = new System.Windows.Forms.Label();
            this.grpCurrency = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCurrency = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.grpPeriodRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPeriod = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtStartPeriod = new System.Windows.Forms.TextBox();
            this.lblStartPeriod = new System.Windows.Forms.Label();
            this.txtEndPeriod = new System.Windows.Forms.TextBox();
            this.lblEndPeriod = new System.Windows.Forms.Label();
            this.cmbPeriodYear = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPeriodYear = new System.Windows.Forms.Label();
            this.rbtQuarter = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtQuarter = new System.Windows.Forms.TextBox();
            this.lblQuarter = new System.Windows.Forms.Label();
            this.cmbQuarterYear = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblQuarterYear = new System.Windows.Forms.Label();
            this.rbtDate = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpStartDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblEndDate = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpLedger.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpCurrency.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpPeriodRange.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Location = new System.Drawing.Point(2,2);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // grpLedger
            //
            this.grpLedger.Name = "grpLedger";
            this.grpLedger.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLedger.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpLedger.Controls.Add(this.tableLayoutPanel2);
            //
            // txtLedger
            //
            this.txtLedger.Location = new System.Drawing.Point(2,2);
            this.txtLedger.Name = "txtLedger";
            this.txtLedger.Size = new System.Drawing.Size(150, 28);
            this.txtLedger.ReadOnly = true;
            this.txtLedger.TabStop = false;
            //
            // lblLedger
            //
            this.lblLedger.Location = new System.Drawing.Point(2,2);
            this.lblLedger.Name = "lblLedger";
            this.lblLedger.AutoSize = true;
            this.lblLedger.Text = "Ledger:";
            this.lblLedger.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLedger.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLedger.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbAccountHierarchy
            //
            this.cmbAccountHierarchy.Location = new System.Drawing.Point(2,2);
            this.cmbAccountHierarchy.Name = "cmbAccountHierarchy";
            this.cmbAccountHierarchy.Size = new System.Drawing.Size(300, 28);
            this.cmbAccountHierarchy.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblAccountHierarchy
            //
            this.lblAccountHierarchy.Location = new System.Drawing.Point(2,2);
            this.lblAccountHierarchy.Name = "lblAccountHierarchy";
            this.lblAccountHierarchy.AutoSize = true;
            this.lblAccountHierarchy.Text = "Account Hierarchy:";
            this.lblAccountHierarchy.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAccountHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAccountHierarchy.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblLedger, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLedger, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAccountHierarchy, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbAccountHierarchy, 3, 0);
            this.grpLedger.Text = "Ledger Details";
            //
            // grpCurrency
            //
            this.grpCurrency.Name = "grpCurrency";
            this.grpCurrency.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCurrency.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpCurrency.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbCurrency
            //
            this.cmbCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(150, 28);
            this.cmbCurrency.Items.AddRange(new object[] {"Base","International","Transaction"});
            //
            // lblCurrency
            //
            this.lblCurrency.Location = new System.Drawing.Point(2,2);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Text = "Currency:";
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblCurrency, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbCurrency, 1, 0);
            this.grpCurrency.Text = "Currency";
            //
            // grpPeriodRange
            //
            this.grpPeriodRange.Location = new System.Drawing.Point(2,2);
            this.grpPeriodRange.Name = "grpPeriodRange";
            this.grpPeriodRange.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.grpPeriodRange.Controls.Add(this.tableLayoutPanel4);
            //
            // rbtPeriod
            //
            this.rbtPeriod.Location = new System.Drawing.Point(2,2);
            this.rbtPeriod.Name = "rbtPeriod";
            this.rbtPeriod.AutoSize = true;
            this.rbtPeriod.Text = "Period";
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            //
            // txtStartPeriod
            //
            this.txtStartPeriod.Location = new System.Drawing.Point(2,2);
            this.txtStartPeriod.Name = "txtStartPeriod";
            this.txtStartPeriod.Size = new System.Drawing.Size(30, 28);
            //
            // lblStartPeriod
            //
            this.lblStartPeriod.Location = new System.Drawing.Point(2,2);
            this.lblStartPeriod.Name = "lblStartPeriod";
            this.lblStartPeriod.AutoSize = true;
            this.lblStartPeriod.Text = "from:";
            this.lblStartPeriod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStartPeriod.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStartPeriod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtEndPeriod
            //
            this.txtEndPeriod.Location = new System.Drawing.Point(2,2);
            this.txtEndPeriod.Name = "txtEndPeriod";
            this.txtEndPeriod.Size = new System.Drawing.Size(30, 28);
            //
            // lblEndPeriod
            //
            this.lblEndPeriod.Location = new System.Drawing.Point(2,2);
            this.lblEndPeriod.Name = "lblEndPeriod";
            this.lblEndPeriod.AutoSize = true;
            this.lblEndPeriod.Text = "to:";
            this.lblEndPeriod.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblEndPeriod.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblEndPeriod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbPeriodYear
            //
            this.cmbPeriodYear.Location = new System.Drawing.Point(2,2);
            this.cmbPeriodYear.Name = "cmbPeriodYear";
            this.cmbPeriodYear.Size = new System.Drawing.Size(300, 28);
            this.cmbPeriodYear.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblPeriodYear
            //
            this.lblPeriodYear.Location = new System.Drawing.Point(2,2);
            this.lblPeriodYear.Name = "lblPeriodYear";
            this.lblPeriodYear.AutoSize = true;
            this.lblPeriodYear.Text = "Year:";
            this.lblPeriodYear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPeriodYear.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPeriodYear.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel5.ColumnCount = 6;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblStartPeriod, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtStartPeriod, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblEndPeriod, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtEndPeriod, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblPeriodYear, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmbPeriodYear, 5, 0);
            this.rbtPeriod.CheckedChanged += new System.EventHandler(this.rbtPeriodCheckedChanged);
            //
            // rbtQuarter
            //
            this.rbtQuarter.Location = new System.Drawing.Point(2,2);
            this.rbtQuarter.Name = "rbtQuarter";
            this.rbtQuarter.AutoSize = true;
            this.rbtQuarter.Text = "Quarter";
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            //
            // txtQuarter
            //
            this.txtQuarter.Location = new System.Drawing.Point(2,2);
            this.txtQuarter.Name = "txtQuarter";
            this.txtQuarter.Size = new System.Drawing.Size(30, 28);
            //
            // lblQuarter
            //
            this.lblQuarter.Location = new System.Drawing.Point(2,2);
            this.lblQuarter.Name = "lblQuarter";
            this.lblQuarter.AutoSize = true;
            this.lblQuarter.Text = "from:";
            this.lblQuarter.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblQuarter.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblQuarter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbQuarterYear
            //
            this.cmbQuarterYear.Location = new System.Drawing.Point(2,2);
            this.cmbQuarterYear.Name = "cmbQuarterYear";
            this.cmbQuarterYear.Size = new System.Drawing.Size(300, 28);
            this.cmbQuarterYear.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblQuarterYear
            //
            this.lblQuarterYear.Location = new System.Drawing.Point(2,2);
            this.lblQuarterYear.Name = "lblQuarterYear";
            this.lblQuarterYear.AutoSize = true;
            this.lblQuarterYear.Text = "Year:";
            this.lblQuarterYear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblQuarterYear.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblQuarterYear.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblQuarter, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.txtQuarter, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblQuarterYear, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.cmbQuarterYear, 3, 0);
            this.rbtQuarter.CheckedChanged += new System.EventHandler(this.rbtQuarterCheckedChanged);
            //
            // rbtDate
            //
            this.rbtDate.Location = new System.Drawing.Point(2,2);
            this.rbtDate.Name = "rbtDate";
            this.rbtDate.AutoSize = true;
            this.rbtDate.Text = "Date";
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            //
            // dtpStartDate
            //
            this.dtpStartDate.Location = new System.Drawing.Point(2,2);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblStartDate
            //
            this.lblStartDate.Location = new System.Drawing.Point(2,2);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Text = "from:";
            this.lblStartDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStartDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpEndDate
            //
            this.dtpEndDate.Location = new System.Drawing.Point(2,2);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(94, 28);
            //
            // lblEndDate
            //
            this.lblEndDate.Location = new System.Drawing.Point(2,2);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Text = "to:";
            this.lblEndDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblEndDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.lblStartDate, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.dtpStartDate, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblEndDate, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.dtpEndDate, 3, 0);
            this.rbtDate.CheckedChanged += new System.EventHandler(this.rbtDateCheckedChanged);
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.rbtPeriod, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.rbtQuarter, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.rbtDate, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 1, 2);
            this.grpPeriodRange.Text = "Period Range";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.grpLedger, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpCurrency, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpPeriodRange, 0, 2);

            //
            // TFrmUC_GeneralSettings
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_GeneralSettings";
            this.Text = "";

            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.grpPeriodRange.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpCurrency.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpLedger.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpLedger;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtLedger;
        private System.Windows.Forms.Label lblLedger;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountHierarchy;
        private System.Windows.Forms.Label lblAccountHierarchy;
        private System.Windows.Forms.GroupBox grpCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Common.Controls.TCmbAutoComplete cmbCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.GroupBox grpPeriodRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton rbtPeriod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtStartPeriod;
        private System.Windows.Forms.Label lblStartPeriod;
        private System.Windows.Forms.TextBox txtEndPeriod;
        private System.Windows.Forms.Label lblEndPeriod;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPeriodYear;
        private System.Windows.Forms.Label lblPeriodYear;
        private System.Windows.Forms.RadioButton rbtQuarter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TextBox txtQuarter;
        private System.Windows.Forms.Label lblQuarter;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbQuarterYear;
        private System.Windows.Forms.Label lblQuarterYear;
        private System.Windows.Forms.RadioButton rbtDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
    }
}
