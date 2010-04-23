/* auto generated with nant generateWinforms from APEditDocument.yaml
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

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    partial class TFrmAPEditDocument
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAPEditDocument));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSupplierInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSupplierName = new System.Windows.Forms.TextBox();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.txtSupplierCurrency = new System.Windows.Forms.TextBox();
            this.lblSupplierCurrency = new System.Windows.Forms.Label();
            this.grpDocumentInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDocumentCode = new System.Windows.Forms.TextBox();
            this.lblDocumentCode = new System.Windows.Forms.Label();
            this.cmbDocumentType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDocumentType = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.dtpDateIssued = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateIssued = new System.Windows.Forms.Label();
            this.dtpDateDue = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateDue = new System.Windows.Forms.Label();
            this.nudCreditTerms = new System.Windows.Forms.NumericUpDown();
            this.lblCreditTerms = new System.Windows.Forms.Label();
            this.nudDiscountDays = new System.Windows.Forms.NumericUpDown();
            this.lblDiscountDays = new System.Windows.Forms.Label();
            this.txtDiscountPercentage = new System.Windows.Forms.TextBox();
            this.lblDiscountPercentage = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtExchangeRateToBase = new System.Windows.Forms.TextBox();
            this.lblExchangeRateToBase = new System.Windows.Forms.Label();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlDetailButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddDetail = new System.Windows.Forms.Button();
            this.btnRemoveDetail = new System.Windows.Forms.Button();
            this.btnAnalysisAttributes = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailNarrative = new System.Windows.Forms.TextBox();
            this.lblDetailNarrative = new System.Windows.Forms.Label();
            this.txtDetailItemRef = new System.Windows.Forms.TextBox();
            this.lblDetailItemRef = new System.Windows.Forms.Label();
            this.txtDetailAmount = new System.Windows.Forms.TextBox();
            this.lblDetailAmount = new System.Windows.Forms.Label();
            this.cmbDetailCostCentreCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailCostCentreCode = new System.Windows.Forms.Label();
            this.btnUseTaxAccountCostCentre = new System.Windows.Forms.Button();
            this.txtDetailBaseAmount = new System.Windows.Forms.TextBox();
            this.lblDetailBaseAmount = new System.Windows.Forms.Label();
            this.cmbDetailAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailAccountCode = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbPostDocument = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoCurrentField = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditUndoScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.pnlSupplierInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpDocumentInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpDetails.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            this.pnlDetailButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpDetails);
            this.pnlContent.Controls.Add(this.grpDocumentInfo);
            this.pnlContent.Controls.Add(this.pnlSupplierInfo);
            //
            // pnlSupplierInfo
            //
            this.pnlSupplierInfo.Name = "pnlSupplierInfo";
            this.pnlSupplierInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSupplierInfo.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlSupplierInfo.Controls.Add(this.tableLayoutPanel1);
            //
            // txtSupplierName
            //
            this.txtSupplierName.Location = new System.Drawing.Point(2,2);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.Size = new System.Drawing.Size(150, 28);
            this.txtSupplierName.ReadOnly = true;
            this.txtSupplierName.TabStop = false;
            //
            // lblSupplierName
            //
            this.lblSupplierName.Location = new System.Drawing.Point(2,2);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Text = "Current Supplier:";
            this.lblSupplierName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSupplierName.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtSupplierCurrency
            //
            this.txtSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.txtSupplierCurrency.Name = "txtSupplierCurrency";
            this.txtSupplierCurrency.Size = new System.Drawing.Size(150, 28);
            this.txtSupplierCurrency.ReadOnly = true;
            this.txtSupplierCurrency.TabStop = false;
            //
            // lblSupplierCurrency
            //
            this.lblSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.lblSupplierCurrency.Name = "lblSupplierCurrency";
            this.lblSupplierCurrency.AutoSize = true;
            this.lblSupplierCurrency.Text = "Currency:";
            this.lblSupplierCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSupplierCurrency.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblSupplierName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSupplierName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSupplierCurrency, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSupplierCurrency, 3, 0);
            //
            // grpDocumentInfo
            //
            this.grpDocumentInfo.Name = "grpDocumentInfo";
            this.grpDocumentInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDocumentInfo.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpDocumentInfo.Controls.Add(this.tableLayoutPanel2);
            //
            // txtDocumentCode
            //
            this.txtDocumentCode.Location = new System.Drawing.Point(2,2);
            this.txtDocumentCode.Name = "txtDocumentCode";
            this.txtDocumentCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblDocumentCode
            //
            this.lblDocumentCode.Location = new System.Drawing.Point(2,2);
            this.lblDocumentCode.Name = "lblDocumentCode";
            this.lblDocumentCode.AutoSize = true;
            this.lblDocumentCode.Text = "Invoice &Number:";
            this.lblDocumentCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDocumentCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbDocumentType
            //
            this.cmbDocumentType.Location = new System.Drawing.Point(2,2);
            this.cmbDocumentType.Name = "cmbDocumentType";
            this.cmbDocumentType.Size = new System.Drawing.Size(150, 28);
            this.cmbDocumentType.Items.AddRange(new object[] {"Invoice","Credit Note"});
            this.cmbDocumentType.Text = "Invoice";
            //
            // lblDocumentType
            //
            this.lblDocumentType.Location = new System.Drawing.Point(2,2);
            this.lblDocumentType.Name = "lblDocumentType";
            this.lblDocumentType.AutoSize = true;
            this.lblDocumentType.Text = "T&ype:";
            this.lblDocumentType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDocumentType.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtReference
            //
            this.txtReference.Location = new System.Drawing.Point(2,2);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(150, 28);
            //
            // lblReference
            //
            this.lblReference.Location = new System.Drawing.Point(2,2);
            this.lblReference.Name = "lblReference";
            this.lblReference.AutoSize = true;
            this.lblReference.Text = "&Reference:";
            this.lblReference.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReference.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // dtpDateIssued
            //
            this.dtpDateIssued.Location = new System.Drawing.Point(2,2);
            this.dtpDateIssued.Name = "dtpDateIssued";
            this.dtpDateIssued.Size = new System.Drawing.Size(150, 28);
            this.dtpDateIssued.DateChanged += new TPetraDateChangedEventHandler(this.UpdateCreditTerms);
            //
            // lblDateIssued
            //
            this.lblDateIssued.Location = new System.Drawing.Point(2,2);
            this.lblDateIssued.Name = "lblDateIssued";
            this.lblDateIssued.AutoSize = true;
            this.lblDateIssued.Text = "&Date Issued:";
            this.lblDateIssued.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateIssued.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // dtpDateDue
            //
            this.dtpDateDue.Location = new System.Drawing.Point(2,2);
            this.dtpDateDue.Name = "dtpDateDue";
            this.dtpDateDue.Size = new System.Drawing.Size(150, 28);
            this.dtpDateDue.DateChanged += new TPetraDateChangedEventHandler(this.UpdateCreditTerms);
            //
            // lblDateDue
            //
            this.lblDateDue.Location = new System.Drawing.Point(2,2);
            this.lblDateDue.Name = "lblDateDue";
            this.lblDateDue.AutoSize = true;
            this.lblDateDue.Text = "Date D&ue:";
            this.lblDateDue.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateDue.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // nudCreditTerms
            //
            this.nudCreditTerms.Location = new System.Drawing.Point(2,2);
            this.nudCreditTerms.Name = "nudCreditTerms";
            this.nudCreditTerms.Size = new System.Drawing.Size(50, 28);
            this.nudCreditTerms.ValueChanged += new System.EventHandler(this.UpdateCreditTermsOverload);
            //
            // lblCreditTerms
            //
            this.lblCreditTerms.Location = new System.Drawing.Point(2,2);
            this.lblCreditTerms.Name = "lblCreditTerms";
            this.lblCreditTerms.AutoSize = true;
            this.lblCreditTerms.Text = "Credit &Terms:";
            this.lblCreditTerms.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCreditTerms.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // nudDiscountDays
            //
            this.nudDiscountDays.Location = new System.Drawing.Point(2,2);
            this.nudDiscountDays.Name = "nudDiscountDays";
            this.nudDiscountDays.Size = new System.Drawing.Size(150, 28);
            this.nudDiscountDays.ValueChanged += new System.EventHandler(this.nudDiscountDaysValueChanged);
            //
            // lblDiscountDays
            //
            this.lblDiscountDays.Location = new System.Drawing.Point(2,2);
            this.lblDiscountDays.Name = "lblDiscountDays";
            this.lblDiscountDays.AutoSize = true;
            this.lblDiscountDays.Text = "Discount &Days:";
            this.lblDiscountDays.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDiscountDays.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtDiscountPercentage
            //
            this.txtDiscountPercentage.Location = new System.Drawing.Point(2,2);
            this.txtDiscountPercentage.Name = "txtDiscountPercentage";
            this.txtDiscountPercentage.Size = new System.Drawing.Size(150, 28);
            //
            // lblDiscountPercentage
            //
            this.lblDiscountPercentage.Location = new System.Drawing.Point(2,2);
            this.lblDiscountPercentage.Name = "lblDiscountPercentage";
            this.lblDiscountPercentage.AutoSize = true;
            this.lblDiscountPercentage.Text = "Discount &Value (%):";
            this.lblDiscountPercentage.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDiscountPercentage.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtTotalAmount
            //
            this.txtTotalAmount.Location = new System.Drawing.Point(2,2);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(150, 28);
            //
            // lblTotalAmount
            //
            this.lblTotalAmount.Location = new System.Drawing.Point(2,2);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Text = "&Amount:";
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblTotalAmount.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtExchangeRateToBase
            //
            this.txtExchangeRateToBase.Location = new System.Drawing.Point(2,2);
            this.txtExchangeRateToBase.Name = "txtExchangeRateToBase";
            this.txtExchangeRateToBase.Size = new System.Drawing.Size(150, 28);
            //
            // lblExchangeRateToBase
            //
            this.lblExchangeRateToBase.Location = new System.Drawing.Point(2,2);
            this.lblExchangeRateToBase.Name = "lblExchangeRateToBase";
            this.lblExchangeRateToBase.AutoSize = true;
            this.lblExchangeRateToBase.Text = "E&xchange Rate:";
            this.lblExchangeRateToBase.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblExchangeRateToBase.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDocumentCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblReference, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDateIssued, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDiscountDays, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblTotalAmount, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtDocumentCode, 1, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.txtReference, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtReference, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpDateIssued, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.nudDiscountDays, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtTotalAmount, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblDocumentType, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDateDue, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDiscountPercentage, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblExchangeRateToBase, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmbDocumentType, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpDateDue, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtDiscountPercentage, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtExchangeRateToBase, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblCreditTerms, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.nudCreditTerms, 5, 2);
            this.grpDocumentInfo.Text = "Document Information";
            //
            // grpDetails
            //
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetails.AutoSize = true;
            this.grpDetails.Controls.Add(this.pnlDetailGrid);
            this.grpDetails.Controls.Add(this.pnlDetails);
            //
            // pnlDetailGrid
            //
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.AutoSize = true;
            this.pnlDetailGrid.Controls.Add(this.grdDetails);
            this.pnlDetailGrid.Controls.Add(this.pnlDetailButtons);
            //
            // grdDetails
            //
            this.grdDetails.Name = "grdDetails";
            this.grdDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDetails.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(this.FocusedRowChanged);
            //
            // pnlDetailButtons
            //
            this.pnlDetailButtons.Name = "pnlDetailButtons";
            this.pnlDetailButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetailButtons.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlDetailButtons.Controls.Add(this.tableLayoutPanel3);
            //
            // btnAddDetail
            //
            this.btnAddDetail.Location = new System.Drawing.Point(2,2);
            this.btnAddDetail.Name = "btnAddDetail";
            this.btnAddDetail.Size = new System.Drawing.Size(100, 28);
            this.btnAddDetail.Click += new System.EventHandler(this.NewDetail);
            this.btnAddDetail.Text = "Add De&tail";
            //
            // btnRemoveDetail
            //
            this.btnRemoveDetail.Location = new System.Drawing.Point(2,2);
            this.btnRemoveDetail.Name = "btnRemoveDetail";
            this.btnRemoveDetail.Size = new System.Drawing.Size(100, 28);
            this.btnRemoveDetail.Text = "&Remove Detail";
            //
            // btnAnalysisAttributes
            //
            this.btnAnalysisAttributes.Location = new System.Drawing.Point(2,2);
            this.btnAnalysisAttributes.Name = "btnAnalysisAttributes";
            this.btnAnalysisAttributes.Size = new System.Drawing.Size(100, 28);
            this.btnAnalysisAttributes.Text = "Analysis Attri&b.";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnAddDetail, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnRemoveDetail, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnAnalysisAttributes, 0, 2);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel4);
            //
            // txtDetailNarrative
            //
            this.txtDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.txtDetailNarrative.Name = "txtDetailNarrative";
            this.txtDetailNarrative.Size = new System.Drawing.Size(350, 28);
            //
            // lblDetailNarrative
            //
            this.lblDetailNarrative.Location = new System.Drawing.Point(2,2);
            this.lblDetailNarrative.Name = "lblDetailNarrative";
            this.lblDetailNarrative.AutoSize = true;
            this.lblDetailNarrative.Text = "Narrati&ve:";
            this.lblDetailNarrative.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailNarrative.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtDetailItemRef
            //
            this.txtDetailItemRef.Location = new System.Drawing.Point(2,2);
            this.txtDetailItemRef.Name = "txtDetailItemRef";
            this.txtDetailItemRef.Size = new System.Drawing.Size(350, 28);
            //
            // lblDetailItemRef
            //
            this.lblDetailItemRef.Location = new System.Drawing.Point(2,2);
            this.lblDetailItemRef.Name = "lblDetailItemRef";
            this.lblDetailItemRef.AutoSize = true;
            this.lblDetailItemRef.Text = "Detail &Ref:";
            this.lblDetailItemRef.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailItemRef.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtDetailAmount
            //
            this.txtDetailAmount.Location = new System.Drawing.Point(2,2);
            this.txtDetailAmount.Name = "txtDetailAmount";
            this.txtDetailAmount.Size = new System.Drawing.Size(150, 28);
            //
            // lblDetailAmount
            //
            this.lblDetailAmount.Location = new System.Drawing.Point(2,2);
            this.lblDetailAmount.Name = "lblDetailAmount";
            this.lblDetailAmount.AutoSize = true;
            this.lblDetailAmount.Text = "A&mount:";
            this.lblDetailAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAmount.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbDetailCostCentreCode
            //
            this.cmbDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCostCentreCode.Name = "cmbDetailCostCentreCode";
            this.cmbDetailCostCentreCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailCostCentreCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailCostCentreCode
            //
            this.lblDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCostCentreCode.Name = "lblDetailCostCentreCode";
            this.lblDetailCostCentreCode.AutoSize = true;
            this.lblDetailCostCentreCode.Text = "C&ost Centre:";
            this.lblDetailCostCentreCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCostCentreCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // btnUseTaxAccountCostCentre
            //
            this.btnUseTaxAccountCostCentre.Location = new System.Drawing.Point(2,2);
            this.btnUseTaxAccountCostCentre.Name = "btnUseTaxAccountCostCentre";
            this.btnUseTaxAccountCostCentre.AutoSize = true;
            this.btnUseTaxAccountCostCentre.Text = "Use Ta&x Acct+CC";
            //
            // txtDetailBaseAmount
            //
            this.txtDetailBaseAmount.Location = new System.Drawing.Point(2,2);
            this.txtDetailBaseAmount.Name = "txtDetailBaseAmount";
            this.txtDetailBaseAmount.Size = new System.Drawing.Size(150, 28);
            this.txtDetailBaseAmount.ReadOnly = true;
            this.txtDetailBaseAmount.TabStop = false;
            //
            // lblDetailBaseAmount
            //
            this.lblDetailBaseAmount.Location = new System.Drawing.Point(2,2);
            this.lblDetailBaseAmount.Name = "lblDetailBaseAmount";
            this.lblDetailBaseAmount.AutoSize = true;
            this.lblDetailBaseAmount.Text = "Base:";
            this.lblDetailBaseAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailBaseAmount.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbDetailAccountCode
            //
            this.cmbDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAccountCode.Name = "cmbDetailAccountCode";
            this.cmbDetailAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailAccountCode
            //
            this.lblDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCode.Name = "lblDetailAccountCode";
            this.lblDetailAccountCode.AutoSize = true;
            this.lblDetailAccountCode.Text = "Accou&nt:";
            this.lblDetailAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblDetailNarrative, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailItemRef, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailAmount, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailBaseAmount, 0, 3);
            this.tableLayoutPanel4.SetColumnSpan(this.txtDetailNarrative, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailNarrative, 1, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.txtDetailItemRef, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailItemRef, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailAmount, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailBaseAmount, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailCostCentreCode, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailAccountCode, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.cmbDetailCostCentreCode, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbDetailAccountCode, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.btnUseTaxAccountCostCentre, 4, 2);
            this.grpDetails.Text = "Details";
            //
            // tbbSave
            //
            this.tbbSave.Name = "tbbSave";
            this.tbbSave.AutoSize = true;
            this.tbbSave.Click += new System.EventHandler(this.FileSave);
            this.tbbSave.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSave.Glyph"));
            this.tbbSave.ToolTipText = "Saves changed data";
            this.tbbSave.Text = "&Save";
            //
            // tbbPostDocument
            //
            this.tbbPostDocument.Name = "tbbPostDocument";
            this.tbbPostDocument.AutoSize = true;
            this.tbbPostDocument.Click += new System.EventHandler(this.PostDocument);
            this.tbbPostDocument.Text = "Post Document";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbPostDocument});
            //
            // mniFileSave
            //
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.AutoSize = true;
            this.mniFileSave.Click += new System.EventHandler(this.FileSave);
            this.mniFileSave.Image = ((System.Drawing.Bitmap)resources.GetObject("mniFileSave.Glyph"));
            this.mniFileSave.ToolTipText = "Saves changed data";
            this.mniFileSave.Text = "&Save";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniFilePrint
            //
            this.mniFilePrint.Name = "mniFilePrint";
            this.mniFilePrint.AutoSize = true;
            this.mniFilePrint.Text = "&Print...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniClose
            //
            this.mniClose.Name = "mniClose";
            this.mniClose.AutoSize = true;
            this.mniClose.Click += new System.EventHandler(this.actClose);
            this.mniClose.Image = ((System.Drawing.Bitmap)resources.GetObject("mniClose.Glyph"));
            this.mniClose.ToolTipText = "Closes this window";
            this.mniClose.Text = "&Close";
            //
            // mniFile
            //
            this.mniFile.Name = "mniFile";
            this.mniFile.AutoSize = true;
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFileSave,
                        mniSeparator0,
                        mniFilePrint,
                        mniSeparator1,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniEditUndoCurrentField
            //
            this.mniEditUndoCurrentField.Name = "mniEditUndoCurrentField";
            this.mniEditUndoCurrentField.AutoSize = true;
            this.mniEditUndoCurrentField.Text = "Undo &Current Field";
            //
            // mniEditUndoScreen
            //
            this.mniEditUndoScreen.Name = "mniEditUndoScreen";
            this.mniEditUndoScreen.AutoSize = true;
            this.mniEditUndoScreen.Text = "&Undo Screen";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniEditFind
            //
            this.mniEditFind.Name = "mniEditFind";
            this.mniEditFind.AutoSize = true;
            this.mniEditFind.Text = "&Find...";
            //
            // mniEdit
            //
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.AutoSize = true;
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniEditUndoCurrentField,
                        mniEditUndoScreen,
                        mniSeparator2,
                        mniEditFind});
            this.mniEdit.Text = "&Edit";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniHelpAboutPetra
            //
            this.mniHelpAboutPetra.Name = "mniHelpAboutPetra";
            this.mniHelpAboutPetra.AutoSize = true;
            this.mniHelpAboutPetra.Text = "&About Petra";
            //
            // mniHelpDevelopmentTeam
            //
            this.mniHelpDevelopmentTeam.Name = "mniHelpDevelopmentTeam";
            this.mniHelpDevelopmentTeam.AutoSize = true;
            this.mniHelpDevelopmentTeam.Text = "&The Development Team...";
            //
            // mniHelp
            //
            this.mniHelp.Name = "mniHelp";
            this.mniHelp.AutoSize = true;
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniHelpPetraHelp,
                        mniSeparator3,
                        mniHelpBugReport,
                        mniSeparator4,
                        mniHelpAboutPetra,
                        mniHelpDevelopmentTeam});
            this.mniHelp.Text = "&Help";
            //
            // mnuMain
            //
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.mnuMain.AutoSize = true;
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniFile,
                        mniEdit,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmAPEditDocument
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 600);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmAPEditDocument";
            this.Text = "AP Document Edit";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlDetailButtons.ResumeLayout(false);
            this.pnlDetailGrid.ResumeLayout(false);
            this.grpDetails.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpDocumentInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlSupplierInfo.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlSupplierInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtSupplierName;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.TextBox txtSupplierCurrency;
        private System.Windows.Forms.Label lblSupplierCurrency;
        private System.Windows.Forms.GroupBox grpDocumentInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtDocumentCode;
        private System.Windows.Forms.Label lblDocumentCode;
        private Ict.Common.Controls.TCmbAutoComplete cmbDocumentType;
        private System.Windows.Forms.Label lblDocumentType;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.Label lblReference;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateIssued;
        private System.Windows.Forms.Label lblDateIssued;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateDue;
        private System.Windows.Forms.Label lblDateDue;
        private System.Windows.Forms.NumericUpDown nudCreditTerms;
        private System.Windows.Forms.Label lblCreditTerms;
        private System.Windows.Forms.NumericUpDown nudDiscountDays;
        private System.Windows.Forms.Label lblDiscountDays;
        private System.Windows.Forms.TextBox txtDiscountPercentage;
        private System.Windows.Forms.Label lblDiscountPercentage;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtExchangeRateToBase;
        private System.Windows.Forms.Label lblExchangeRateToBase;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Panel pnlDetailGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlDetailButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnAddDetail;
        private System.Windows.Forms.Button btnRemoveDetail;
        private System.Windows.Forms.Button btnAnalysisAttributes;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtDetailNarrative;
        private System.Windows.Forms.Label lblDetailNarrative;
        private System.Windows.Forms.TextBox txtDetailItemRef;
        private System.Windows.Forms.Label lblDetailItemRef;
        private System.Windows.Forms.TextBox txtDetailAmount;
        private System.Windows.Forms.Label lblDetailAmount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailCostCentreCode;
        private System.Windows.Forms.Label lblDetailCostCentreCode;
        private System.Windows.Forms.Button btnUseTaxAccountCostCentre;
        private System.Windows.Forms.TextBox txtDetailBaseAmount;
        private System.Windows.Forms.Label lblDetailBaseAmount;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailAccountCode;
        private System.Windows.Forms.Label lblDetailAccountCode;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbPostDocument;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniFilePrint;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
        private System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniEditFind;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
