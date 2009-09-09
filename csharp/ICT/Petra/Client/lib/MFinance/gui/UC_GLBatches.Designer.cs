/* auto generated with nant generateWinforms from UC_GLBatches.yaml
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
    partial class TUC_GLBatches
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_GLBatches));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLedgerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLedgerNumber = new System.Windows.Forms.TextBox();
            this.lblLedgerNumber = new System.Windows.Forms.Label();
            this.rgrShowBatches = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPosting = new System.Windows.Forms.RadioButton();
            this.rbtEditing = new System.Windows.Forms.RadioButton();
            this.rbtAll = new System.Windows.Forms.RadioButton();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPostBatch = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailBatchDescription = new System.Windows.Forms.TextBox();
            this.lblDetailBatchDescription = new System.Windows.Forms.Label();
            this.txtDetailBatchControlTotal = new System.Windows.Forms.TextBox();
            this.lblDetailBatchControlTotal = new System.Windows.Forms.Label();
            this.dtpDetailDateEffective = new System.Windows.Forms.DateTimePicker();
            this.lblDetailDateEffective = new System.Windows.Forms.Label();
            this.dtpDateCantBeBeyond = new System.Windows.Forms.DateTimePicker();
            this.lblDateCantBeBeyond = new System.Windows.Forms.Label();
            this.tbrTabPage = new System.Windows.Forms.ToolStrip();
            this.tbbPostBatch = new System.Windows.Forms.ToolStripButton();
            this.mnuTabPage = new System.Windows.Forms.MenuStrip();
            this.mniBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.mniPost = new System.Windows.Forms.ToolStripMenuItem();

            this.pnlContent.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlLedgerInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrShowBatches.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tbrTabPage.SuspendLayout();
            this.mnuTabPage.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Controls.Add(this.pnlDetailGrid);
            this.pnlContent.Controls.Add(this.pnlDetails);
            this.pnlContent.Controls.Add(this.pnlInfo);
            //
            // pnlInfo
            //
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlLedgerInfo
            //
            this.pnlLedgerInfo.Location = new System.Drawing.Point(2,2);
            this.pnlLedgerInfo.Name = "pnlLedgerInfo";
            this.pnlLedgerInfo.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLedgerInfo.Controls.Add(this.tableLayoutPanel2);
            //
            // txtLedgerNumber
            //
            this.txtLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.txtLedgerNumber.Name = "txtLedgerNumber";
            this.txtLedgerNumber.Size = new System.Drawing.Size(150, 28);
            this.txtLedgerNumber.ReadOnly = true;
            //
            // lblLedgerNumber
            //
            this.lblLedgerNumber.Location = new System.Drawing.Point(2,2);
            this.lblLedgerNumber.Name = "lblLedgerNumber";
            this.lblLedgerNumber.AutoSize = true;
            this.lblLedgerNumber.Text = "Ledger:";
            this.lblLedgerNumber.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblLedgerNumber, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLedgerNumber, 1, 0);
            //
            // rgrShowBatches
            //
            this.rgrShowBatches.Location = new System.Drawing.Point(2,2);
            this.rgrShowBatches.Name = "rgrShowBatches";
            this.rgrShowBatches.AutoSize = true;
            this.rgrShowBatches.Tag = "SuppressChangeDetection";
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgrShowBatches.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtPosting
            //
            this.rbtPosting.Location = new System.Drawing.Point(2,2);
            this.rbtPosting.Name = "rbtPosting";
            this.rbtPosting.AutoSize = true;
            this.rbtPosting.Tag = "SuppressChangeDetection";
            this.rbtPosting.Text = "Posting";
            this.rbtPosting.Checked = true;
            //
            // rbtEditing
            //
            this.rbtEditing.Location = new System.Drawing.Point(2,2);
            this.rbtEditing.Name = "rbtEditing";
            this.rbtEditing.AutoSize = true;
            this.rbtEditing.Tag = "SuppressChangeDetection";
            this.rbtEditing.Text = "Editing";
            //
            // rbtAll
            //
            this.rbtAll.Location = new System.Drawing.Point(2,2);
            this.rbtAll.Name = "rbtAll";
            this.rbtAll.AutoSize = true;
            this.rbtAll.Tag = "SuppressChangeDetection";
            this.rbtAll.Text = "All";
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtPosting, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtEditing, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtAll, 2, 0);
            this.rgrShowBatches.Text = "Show batches available for";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlLedgerInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rgrShowBatches, 0, 1);
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
            this.grdDetails.DoubleClick += new System.EventHandler(this.ShowJournalTab);
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            //
            // pnlDetailButtons
            //
            this.pnlDetailButtons.Name = "pnlDetailButtons";
            this.pnlDetailButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetailButtons.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailButtons.Controls.Add(this.tableLayoutPanel4);
            //
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRow);
            this.btnNew.Text = "&Add";
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(2,2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.AutoSize = true;
            this.btnDelete.Click += new System.EventHandler(this.CancelRow);
            this.btnDelete.Text = "&Cancel";
            //
            // btnPostBatch
            //
            this.btnPostBatch.Location = new System.Drawing.Point(2,2);
            this.btnPostBatch.Name = "btnPostBatch";
            this.btnPostBatch.AutoSize = true;
            this.btnPostBatch.Click += new System.EventHandler(this.PostBatch);
            this.btnPostBatch.Text = "&Post Batch";
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnNew, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnDelete, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnPostBatch, 0, 2);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel5);
            //
            // txtDetailBatchDescription
            //
            this.txtDetailBatchDescription.Location = new System.Drawing.Point(2,2);
            this.txtDetailBatchDescription.Name = "txtDetailBatchDescription";
            this.txtDetailBatchDescription.Size = new System.Drawing.Size(350, 28);
            //
            // lblDetailBatchDescription
            //
            this.lblDetailBatchDescription.Location = new System.Drawing.Point(2,2);
            this.lblDetailBatchDescription.Name = "lblDetailBatchDescription";
            this.lblDetailBatchDescription.AutoSize = true;
            this.lblDetailBatchDescription.Text = "Batch Description:";
            this.lblDetailBatchDescription.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtDetailBatchControlTotal
            //
            this.txtDetailBatchControlTotal.Location = new System.Drawing.Point(2,2);
            this.txtDetailBatchControlTotal.Name = "txtDetailBatchControlTotal";
            this.txtDetailBatchControlTotal.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailBatchControlTotal
            //
            this.lblDetailBatchControlTotal.Location = new System.Drawing.Point(2,2);
            this.lblDetailBatchControlTotal.Name = "lblDetailBatchControlTotal";
            this.lblDetailBatchControlTotal.AutoSize = true;
            this.lblDetailBatchControlTotal.Text = "Batch Hash Total:";
            this.lblDetailBatchControlTotal.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpDetailDateEffective
            //
            this.dtpDetailDateEffective.Location = new System.Drawing.Point(2,2);
            this.dtpDetailDateEffective.Name = "dtpDetailDateEffective";
            this.dtpDetailDateEffective.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailDateEffective
            //
            this.lblDetailDateEffective.Location = new System.Drawing.Point(2,2);
            this.lblDetailDateEffective.Name = "lblDetailDateEffective";
            this.lblDetailDateEffective.AutoSize = true;
            this.lblDetailDateEffective.Text = "Effective Date:";
            this.lblDetailDateEffective.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // dtpDateCantBeBeyond
            //
            this.dtpDateCantBeBeyond.Location = new System.Drawing.Point(2,2);
            this.dtpDateCantBeBeyond.Name = "dtpDateCantBeBeyond";
            this.dtpDateCantBeBeyond.Size = new System.Drawing.Size(150, 28);
            this.dtpDateCantBeBeyond.Enabled = false;
            //
            // lblDateCantBeBeyond
            //
            this.lblDateCantBeBeyond.Location = new System.Drawing.Point(2,2);
            this.lblDateCantBeBeyond.Name = "lblDateCantBeBeyond";
            this.lblDateCantBeBeyond.AutoSize = true;
            this.lblDateCantBeBeyond.Text = "Date can't be beyond:";
            this.lblDateCantBeBeyond.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblDetailBatchDescription, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailBatchControlTotal, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblDetailDateEffective, 0, 2);
            this.tableLayoutPanel5.SetColumnSpan(this.txtDetailBatchDescription, 2);
            this.tableLayoutPanel5.Controls.Add(this.txtDetailBatchDescription, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtDetailBatchControlTotal, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.dtpDetailDateEffective, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.lblDateCantBeBeyond, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.dtpDateCantBeBeyond, 3, 2);
            //
            // tbbPostBatch
            //
            this.tbbPostBatch.Name = "tbbPostBatch";
            this.tbbPostBatch.AutoSize = true;
            this.tbbPostBatch.Click += new System.EventHandler(this.PostBatch);
            this.tbbPostBatch.Text = "&Post Batch";
            //
            // tbrTabPage
            //
            this.tbrTabPage.Name = "tbrTabPage";
            this.tbrTabPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrTabPage.AutoSize = true;
            this.tbrTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbPostBatch});
            //
            // mniPost
            //
            this.mniPost.Name = "mniPost";
            this.mniPost.AutoSize = true;
            this.mniPost.Click += new System.EventHandler(this.PostBatch);
            this.mniPost.Text = "&Post Batch";
            //
            // mniBatch
            //
            this.mniBatch.Name = "mniBatch";
            this.mniBatch.AutoSize = true;
            this.mniBatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniPost});
            this.mniBatch.Text = "&Batch";
            //
            // mnuTabPage
            //
            this.mnuTabPage.Name = "mnuTabPage";
            this.mnuTabPage.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuTabPage.AutoSize = true;
            this.mnuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniBatch});

            //
            // TUC_GLBatches
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrTabPage);
            this.Controls.Add(this.mnuTabPage);
            this.Name = "TUC_GLBatches";
            this.Text = "";

	
            this.mnuTabPage.ResumeLayout(false);
            this.tbrTabPage.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrShowBatches.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlLedgerInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlLedgerInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtLedgerNumber;
        private System.Windows.Forms.Label lblLedgerNumber;
        private System.Windows.Forms.GroupBox rgrShowBatches;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtPosting;
        private System.Windows.Forms.RadioButton rbtEditing;
        private System.Windows.Forms.RadioButton rbtAll;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPostBatch;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtDetailBatchDescription;
        private System.Windows.Forms.Label lblDetailBatchDescription;
        private System.Windows.Forms.TextBox txtDetailBatchControlTotal;
        private System.Windows.Forms.Label lblDetailBatchControlTotal;
        private System.Windows.Forms.DateTimePicker dtpDetailDateEffective;
        private System.Windows.Forms.Label lblDetailDateEffective;
        private System.Windows.Forms.DateTimePicker dtpDateCantBeBeyond;
        private System.Windows.Forms.Label lblDateCantBeBeyond;
        private System.Windows.Forms.ToolStrip tbrTabPage;
        private System.Windows.Forms.ToolStripButton tbbPostBatch;
        private System.Windows.Forms.MenuStrip mnuTabPage;
        private System.Windows.Forms.ToolStripMenuItem mniBatch;
        private System.Windows.Forms.ToolStripMenuItem mniPost;
    }
}
