// auto generated with nant generateWinforms from UC_AccountAnalysisAttributes.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    partial class TUC_AccountAnalysisAttributes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_AccountAnalysisAttributes));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbDetailAnalysisTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailAnalysisTypeCode = new System.Windows.Forms.Label();
            this.chkDetailActive = new System.Windows.Forms.CheckBox();
            this.lblDetailActive = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();

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
            // pnlGrid
            //
            this.pnlGrid.Location = new System.Drawing.Point(2,2);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(300, 250);
            this.pnlGrid.Controls.Add(this.grdDetails);
            this.pnlGrid.Controls.Add(this.pnlButtons);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            //
            // pnlButtons
            //
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel2);
            //
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRow);
            this.btnNew.Text = "&New";
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(2,2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.AutoSize = true;
            this.btnDelete.Click += new System.EventHandler(this.DeleteRow);
            this.btnDelete.Text = "&Delete";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnNew, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 0, 1);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbDetailAnalysisTypeCode
            //
            this.cmbDetailAnalysisTypeCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAnalysisTypeCode.Name = "cmbDetailAnalysisTypeCode";
            this.cmbDetailAnalysisTypeCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailAnalysisTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.AnalysisTypeList;
            //
            // lblDetailAnalysisTypeCode
            //
            this.lblDetailAnalysisTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailAnalysisTypeCode.Name = "lblDetailAnalysisTypeCode";
            this.lblDetailAnalysisTypeCode.AutoSize = true;
            this.lblDetailAnalysisTypeCode.Text = "Analysis Type Code:";
            this.lblDetailAnalysisTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAnalysisTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAnalysisTypeCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailActive
            //
            this.chkDetailActive.Location = new System.Drawing.Point(2,2);
            this.chkDetailActive.Name = "chkDetailActive";
            this.chkDetailActive.Size = new System.Drawing.Size(30, 28);
            this.chkDetailActive.Text = "";
            this.chkDetailActive.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailActive
            //
            this.lblDetailActive.Location = new System.Drawing.Point(2,2);
            this.lblDetailActive.Name = "lblDetailActive";
            this.lblDetailActive.AutoSize = true;
            this.lblDetailActive.Text = "Active:";
            this.lblDetailActive.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailActive.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailActive.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailAnalysisTypeCode, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailActive, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailAnalysisTypeCode, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkDetailActive, 1, 1);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlDetails, 0, 1);

            //
            // TUC_AccountAnalysisAttributes
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_AccountAnalysisAttributes";
            this.Text = "";

            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailAnalysisTypeCode;
        private System.Windows.Forms.Label lblDetailAnalysisTypeCode;
        private System.Windows.Forms.CheckBox chkDetailActive;
        private System.Windows.Forms.Label lblDetailActive;
    }
}
