// auto generated with nant generateWinforms from UC_AccountCostCentreSettings.yaml
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
    partial class TFrmUC_AccountCostCentreSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_AccountCostCentreSettings));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrAccountCodes = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAccountRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbFromAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblFromAccountCode = new System.Windows.Forms.Label();
            this.cmbToAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblToAccountCode = new System.Windows.Forms.Label();
            this.rbtAccountFromList = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.clbAccountCodes = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllAccountCodes = new System.Windows.Forms.Button();
            this.rgrCostCentre = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtCostCentreRange = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbFromCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblFromCostCentre = new System.Windows.Forms.Label();
            this.cmbToCostCentre = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblToCostCentre = new System.Windows.Forms.Label();
            this.rbtCostCentreFromList = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.clbCostCentres = new Ict.Common.Controls.TClbVersatile();
            this.btnUnselectAllCostCentres = new System.Windows.Forms.Button();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrAccountCodes.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.rgrCostCentre.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();

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
            // rgrAccountCodes
            //
            this.rgrAccountCodes.Location = new System.Drawing.Point(2,2);
            this.rgrAccountCodes.Name = "rgrAccountCodes";
            this.rgrAccountCodes.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrAccountCodes.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtAccountRange
            //
            this.rbtAccountRange.Location = new System.Drawing.Point(2,2);
            this.rbtAccountRange.Name = "rbtAccountRange";
            this.rbtAccountRange.AutoSize = true;
            this.rbtAccountRange.Text = "Select Range";
            this.rbtAccountRange.Checked = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            //
            // cmbFromAccountCode
            //
            this.cmbFromAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbFromAccountCode.Name = "cmbFromAccountCode";
            this.cmbFromAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbFromAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblFromAccountCode
            //
            this.lblFromAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblFromAccountCode.Name = "lblFromAccountCode";
            this.lblFromAccountCode.AutoSize = true;
            this.lblFromAccountCode.Text = "From:";
            this.lblFromAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFromAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFromAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbToAccountCode
            //
            this.cmbToAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbToAccountCode.Name = "cmbToAccountCode";
            this.cmbToAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbToAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblToAccountCode
            //
            this.lblToAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblToAccountCode.Name = "lblToAccountCode";
            this.lblToAccountCode.AutoSize = true;
            this.lblToAccountCode.Text = "To:";
            this.lblToAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblToAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblToAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblFromAccountCode, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblToAccountCode, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbFromAccountCode, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbToAccountCode, 1, 1);
            this.rbtAccountRange.CheckedChanged += new System.EventHandler(this.rbtAccountRangeCheckedChanged);
            //
            // rbtAccountFromList
            //
            this.rbtAccountFromList.Location = new System.Drawing.Point(2,2);
            this.rbtAccountFromList.Name = "rbtAccountFromList";
            this.rbtAccountFromList.AutoSize = true;
            this.rbtAccountFromList.Text = "From List";
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            //
            // clbAccountCodes
            //
            this.clbAccountCodes.Name = "clbAccountCodes";
            this.clbAccountCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbAccountCodes.Size = new System.Drawing.Size(365, 100);
            this.clbAccountCodes.FixedRows = 1;
            //
            // btnUnselectAllAccountCodes
            //
            this.btnUnselectAllAccountCodes.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllAccountCodes.Name = "btnUnselectAllAccountCodes";
            this.btnUnselectAllAccountCodes.AutoSize = true;
            this.btnUnselectAllAccountCodes.Click += new System.EventHandler(this.UnselectAllAccountCodes);
            this.btnUnselectAllAccountCodes.Text = "Unselect All";
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.clbAccountCodes, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnUnselectAllAccountCodes, 1, 0);
            this.rbtAccountFromList.CheckedChanged += new System.EventHandler(this.rbtAccountFromListCheckedChanged);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtAccountRange, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtAccountFromList, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.rgrAccountCodes.Text = "Select Account Codes";
            //
            // rgrCostCentre
            //
            this.rgrCostCentre.Location = new System.Drawing.Point(2,2);
            this.rgrCostCentre.Name = "rgrCostCentre";
            this.rgrCostCentre.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.rgrCostCentre.Controls.Add(this.tableLayoutPanel5);
            //
            // rbtCostCentreRange
            //
            this.rbtCostCentreRange.Location = new System.Drawing.Point(2,2);
            this.rbtCostCentreRange.Name = "rbtCostCentreRange";
            this.rbtCostCentreRange.AutoSize = true;
            this.rbtCostCentreRange.Text = "Select Range";
            this.rbtCostCentreRange.Checked = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            //
            // cmbFromCostCentre
            //
            this.cmbFromCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbFromCostCentre.Name = "cmbFromCostCentre";
            this.cmbFromCostCentre.Size = new System.Drawing.Size(300, 28);
            this.cmbFromCostCentre.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblFromCostCentre
            //
            this.lblFromCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblFromCostCentre.Name = "lblFromCostCentre";
            this.lblFromCostCentre.AutoSize = true;
            this.lblFromCostCentre.Text = "From:";
            this.lblFromCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFromCostCentre.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFromCostCentre.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbToCostCentre
            //
            this.cmbToCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbToCostCentre.Name = "cmbToCostCentre";
            this.cmbToCostCentre.Size = new System.Drawing.Size(300, 28);
            this.cmbToCostCentre.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblToCostCentre
            //
            this.lblToCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblToCostCentre.Name = "lblToCostCentre";
            this.lblToCostCentre.AutoSize = true;
            this.lblToCostCentre.Text = "To:";
            this.lblToCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblToCostCentre.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblToCostCentre.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblFromCostCentre, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblToCostCentre, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.cmbFromCostCentre, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.cmbToCostCentre, 1, 1);
            this.rbtCostCentreRange.CheckedChanged += new System.EventHandler(this.rbtCostCentreRangeCheckedChanged);
            //
            // rbtCostCentreFromList
            //
            this.rbtCostCentreFromList.Location = new System.Drawing.Point(2,2);
            this.rbtCostCentreFromList.Name = "rbtCostCentreFromList";
            this.rbtCostCentreFromList.AutoSize = true;
            this.rbtCostCentreFromList.Text = "From List";
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
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
            this.btnUnselectAllCostCentres.Click += new System.EventHandler(this.UnselectAllCostCentres);
            this.btnUnselectAllCostCentres.Text = "Unselect All";
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.clbCostCentres, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnUnselectAllCostCentres, 1, 0);
            this.rbtCostCentreFromList.CheckedChanged += new System.EventHandler(this.rbtCostCentreFromListCheckedChanged);
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.rbtCostCentreRange, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.rbtCostCentreFromList, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 1, 1);
            this.rgrCostCentre.Text = "Select Cost Centre Codes";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.rgrAccountCodes, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rgrCostCentre, 0, 1);

            //
            // TFrmUC_AccountCostCentreSettings
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_AccountCostCentreSettings";
            this.Text = "";

            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.rgrCostCentre.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrAccountCodes.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox rgrAccountCodes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtAccountRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbFromAccountCode;
        private System.Windows.Forms.Label lblFromAccountCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbToAccountCode;
        private System.Windows.Forms.Label lblToAccountCode;
        private System.Windows.Forms.RadioButton rbtAccountFromList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Ict.Common.Controls.TClbVersatile clbAccountCodes;
        private System.Windows.Forms.Button btnUnselectAllAccountCodes;
        private System.Windows.Forms.GroupBox rgrCostCentre;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton rbtCostCentreRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbFromCostCentre;
        private System.Windows.Forms.Label lblFromCostCentre;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbToCostCentre;
        private System.Windows.Forms.Label lblToCostCentre;
        private System.Windows.Forms.RadioButton rbtCostCentreFromList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Ict.Common.Controls.TClbVersatile clbCostCentres;
        private System.Windows.Forms.Button btnUnselectAllCostCentres;
    }
}
