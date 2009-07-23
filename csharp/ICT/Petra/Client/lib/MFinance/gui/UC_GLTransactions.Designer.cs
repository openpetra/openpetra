/* auto generated with nant generateWinforms from UC_GLTransactions.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    partial class TUC_GLTransactions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GLTransactions));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLedgerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.pnlBatches = new System.Windows.Forms.Panel();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailNarrative = new System.Windows.Forms.TextBox();
            this.lblDetailNarrative = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlLedgerInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlBatches.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlLedgerInfo
            //
            this.pnlLedgerInfo.Location = new System.Drawing.Point(2,2);
            this.pnlLedgerInfo.Name = "pnlLedgerInfo";
            this.pnlLedgerInfo.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLedgerInfo.Controls.Add(this.tableLayoutPanel2);
            //
            // txtLedgerNumber
            //
            this.txtLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.txtLedgerNumber.Name = "txtLedgerNumber";
            this.txtLedgerNumber.Size = new System.Drawing.Size(150, 28);
            //
            // lblLedgerNumber
            //
            this.lblLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.lblLedgerNumber.Name = "lblLedgerNumber";
            this.lblLedgerNumber.AutoSize = true;
            this.lblLedgerNumber.Text = "LedgerNumber:";
            this.lblLedgerNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLedgerNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlLedgerInfo, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlLedgerInfo, 2);
            //
            // pnlBatches
            //
            this.pnlBatches.Name = "pnlBatches";
            this.pnlBatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBatches.Controls.Add(this.pnlDetailGrid);
            this.pnlBatches.Controls.Add(this.pnlDetails);
            //
            // pnlDetailGrid
            //
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.Controls.Add(this.grdDetails);
            this.pnlDetailGrid.Controls.Add(this.pnlDetailButtons);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // pnlDetailButtons
            //
            this.pnlDetailButtons.Name = "pnlDetailButtons";
            this.pnlDetailButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetailButtons.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlDetailButtons.Controls.Add(this.tableLayoutPanel3);
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(2,2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.AutoSize = true;
            this.btnAdd.Text = "Add";
            this.tableLayoutPanel3.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.btnAdd, 2);
            //
            // btnRemove
            //
            this.btnRemove.Location = new System.Drawing.Point(2,2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.AutoSize = true;
            this.btnRemove.Text = "Remove";
            this.tableLayoutPanel3.Controls.Add(this.btnRemove, 0, 1);
            this.tableLayoutPanel3.SetColumnSpan(this.btnRemove, 2);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlDetails.Controls.Add(this.tableLayoutPanel4);
            //
            // txtDetailNarrative
            //
            this.txtDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.txtDetailNarrative.Name = "txtDetailNarrative";
            this.txtDetailNarrative.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailNarrative
            //
            this.lblDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.lblDetailNarrative.Name = "lblDetailNarrative";
            this.lblDetailNarrative.AutoSize = true;
            this.lblDetailNarrative.Text = "DetailNarrative:";
            this.lblDetailNarrative.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailNarrative, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailNarrative, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlBatches, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlBatches, 2);

            //
            // TUC_GLTransactions
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Name = "TUC_GLTransactions";
            this.Text = "";

	
	
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.pnlBatches.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlLedgerInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlLedgerInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtLedgerNumber;
        private System.Windows.Forms.Label lblLedgerNumber;
        private System.Windows.Forms.Panel pnlBatches;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtDetailNarrative;
        private System.Windows.Forms.Label lblDetailNarrative;
    }
}
