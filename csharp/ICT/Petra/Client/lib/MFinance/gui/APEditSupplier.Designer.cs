/* auto generated with nant generateWinforms from APEditSupplier.yaml
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

namespace Ict.Petra.Client.MFinance.Gui
{
    partial class TFrmAccountsPayableEditSupplier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAccountsPayableEditSupplier));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPartnerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPartnerKey = new System.Windows.Forms.TextBox();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.btnEditPartner = new System.Windows.Forms.Button();
            this.pnlPartnerInfo2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPartnerName = new System.Windows.Forms.TextBox();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.pnlDefaults = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.grpGeneralInformation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCurrency = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.cmbSupplierType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSupplierType = new System.Windows.Forms.Label();
            this.grpMiscDefaults = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.nudInvoiceAging = new System.Windows.Forms.NumericUpDown();
            this.lblInvoiceAging = new System.Windows.Forms.Label();
            this.nudCreditTerms = new System.Windows.Forms.NumericUpDown();
            this.lblCreditTerms = new System.Windows.Forms.Label();
            this.cmbDefaultPaymentType = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDefaultPaymentType = new System.Windows.Forms.Label();
            this.nudDiscountDays = new System.Windows.Forms.NumericUpDown();
            this.lblDiscountDays = new System.Windows.Forms.Label();
            this.txtDiscountValue = new System.Windows.Forms.TextBox();
            this.lblDiscountValue = new System.Windows.Forms.Label();
            this.grpAccountInformation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbAPAccount = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblAPAccount = new System.Windows.Forms.Label();
            this.cmbDefaultBankAccount = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDefaultBankAccount = new System.Windows.Forms.Label();
            this.cmbCostCentre = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCostCentre = new System.Windows.Forms.Label();
            this.cmbExpenseAccount = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblExpenseAccount = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
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
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlPartnerInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlPartnerInfo2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlDefaults.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.grpGeneralInformation.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.grpMiscDefaults.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.grpAccountInformation.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

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
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlPartnerInfo
            //
            this.pnlPartnerInfo.Name = "pnlPartnerInfo";
            this.pnlPartnerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPartnerInfo.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlPartnerInfo.Controls.Add(this.tableLayoutPanel2);
            //
            // txtPartnerKey
            //
            this.txtPartnerKey.Location = new System.Drawing.Point(2,2);
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.Size = new System.Drawing.Size(150, 28);
            this.txtPartnerKey.ReadOnly = true;
            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(2,2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.AutoSize = true;
            this.lblPartnerKey.Text = "Partner Key:";
            this.lblPartnerKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPartnerKey, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtPartnerKey, 1, 0);
            //
            // btnEditPartner
            //
            this.btnEditPartner.Location = new System.Drawing.Point(2,2);
            this.btnEditPartner.Name = "btnEditPartner";
            this.btnEditPartner.AutoSize = true;
            this.btnEditPartner.Click += new System.EventHandler(this.btnEditPartnerClick);
            this.btnEditPartner.Text = "&Edit Partner info of Supplier";
            this.tableLayoutPanel2.Controls.Add(this.btnEditPartner, 2, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.btnEditPartner, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlPartnerInfo, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlPartnerInfo, 2);
            //
            // pnlPartnerInfo2
            //
            this.pnlPartnerInfo2.Name = "pnlPartnerInfo2";
            this.pnlPartnerInfo2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPartnerInfo2.AutoSize = true;
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
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlPartnerInfo2.Controls.Add(this.tableLayoutPanel3);
            //
            // txtPartnerName
            //
            this.txtPartnerName.Location = new System.Drawing.Point(2,2);
            this.txtPartnerName.Name = "txtPartnerName";
            this.txtPartnerName.Size = new System.Drawing.Size(150, 28);
            this.txtPartnerName.ReadOnly = true;
            //
            // lblPartnerName
            //
            this.lblPartnerName.Location = new System.Drawing.Point(2,2);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.AutoSize = true;
            this.lblPartnerName.Text = "Partner Name:";
            this.lblPartnerName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblPartnerName, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtPartnerName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlPartnerInfo2, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlPartnerInfo2, 2);
            //
            // pnlDefaults
            //
            this.pnlDefaults.Name = "pnlDefaults";
            this.pnlDefaults.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefaults.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlDefaults.Controls.Add(this.tableLayoutPanel4);
            //
            // grpGeneralInformation
            //
            this.grpGeneralInformation.Location = new System.Drawing.Point(2,2);
            this.grpGeneralInformation.Name = "grpGeneralInformation";
            this.grpGeneralInformation.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.grpGeneralInformation.Controls.Add(this.tableLayoutPanel5);
            //
            // cmbCurrency
            //
            this.cmbCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(150, 28);
            this.cmbCurrency.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            //
            // lblCurrency
            //
            this.lblCurrency.Location = new System.Drawing.Point(2,2);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Text = "&Currency:";
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblCurrency, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmbCurrency, 1, 0);
            //
            // cmbSupplierType
            //
            this.cmbSupplierType.Location = new System.Drawing.Point(2,2);
            this.cmbSupplierType.Name = "cmbSupplierType";
            this.cmbSupplierType.Size = new System.Drawing.Size(150, 28);
            this.cmbSupplierType.Items.AddRange(new object[] {"NORMAL","CREDIT CARD"});;
            //
            // lblSupplierType
            //
            this.lblSupplierType.Location = new System.Drawing.Point(2,2);
            this.lblSupplierType.Name = "lblSupplierType";
            this.lblSupplierType.AutoSize = true;
            this.lblSupplierType.Text = "Supplier &Type:";
            this.lblSupplierType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblSupplierType, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.cmbSupplierType, 1, 1);
            this.grpGeneralInformation.Text = "General Information";
            this.tableLayoutPanel4.Controls.Add(this.grpGeneralInformation, 0, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.grpGeneralInformation, 2);
            //
            // grpMiscDefaults
            //
            this.grpMiscDefaults.Location = new System.Drawing.Point(2,2);
            this.grpMiscDefaults.Name = "grpMiscDefaults";
            this.grpMiscDefaults.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.grpMiscDefaults.Controls.Add(this.tableLayoutPanel6);
            //
            // nudInvoiceAging
            //
            this.nudInvoiceAging.Location = new System.Drawing.Point(2,2);
            this.nudInvoiceAging.Name = "nudInvoiceAging";
            this.nudInvoiceAging.Size = new System.Drawing.Size(150, 28);
            //
            // lblInvoiceAging
            //
            this.lblInvoiceAging.Location = new System.Drawing.Point(2,2);
            this.lblInvoiceAging.Name = "lblInvoiceAging";
            this.lblInvoiceAging.AutoSize = true;
            this.lblInvoiceAging.Text = "Invoice A&ging (in months):";
            this.lblInvoiceAging.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblInvoiceAging, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.nudInvoiceAging, 1, 0);
            //
            // nudCreditTerms
            //
            this.nudCreditTerms.Location = new System.Drawing.Point(2,2);
            this.nudCreditTerms.Name = "nudCreditTerms";
            this.nudCreditTerms.Size = new System.Drawing.Size(150, 28);
            //
            // lblCreditTerms
            //
            this.lblCreditTerms.Location = new System.Drawing.Point(2,2);
            this.lblCreditTerms.Name = "lblCreditTerms";
            this.lblCreditTerms.AutoSize = true;
            this.lblCreditTerms.Text = "C&redit Terms (in days):";
            this.lblCreditTerms.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblCreditTerms, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.nudCreditTerms, 1, 1);
            //
            // cmbDefaultPaymentType
            //
            this.cmbDefaultPaymentType.Location = new System.Drawing.Point(2,2);
            this.cmbDefaultPaymentType.Name = "cmbDefaultPaymentType";
            this.cmbDefaultPaymentType.Size = new System.Drawing.Size(150, 28);
            this.cmbDefaultPaymentType.Items.AddRange(new object[] {"CASH","CHEQUE","TRANSFER"});;
            //
            // lblDefaultPaymentType
            //
            this.lblDefaultPaymentType.Location = new System.Drawing.Point(2,2);
            this.lblDefaultPaymentType.Name = "lblDefaultPaymentType";
            this.lblDefaultPaymentType.AutoSize = true;
            this.lblDefaultPaymentType.Text = "Default &Payment Type:";
            this.lblDefaultPaymentType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblDefaultPaymentType, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.cmbDefaultPaymentType, 1, 2);
            //
            // nudDiscountDays
            //
            this.nudDiscountDays.Location = new System.Drawing.Point(2,2);
            this.nudDiscountDays.Name = "nudDiscountDays";
            this.nudDiscountDays.Size = new System.Drawing.Size(150, 28);
            //
            // lblDiscountDays
            //
            this.lblDiscountDays.Location = new System.Drawing.Point(2,2);
            this.lblDiscountDays.Name = "lblDiscountDays";
            this.lblDiscountDays.AutoSize = true;
            this.lblDiscountDays.Text = "Number of Days for &Discount (0 for none):";
            this.lblDiscountDays.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblDiscountDays, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.nudDiscountDays, 1, 3);
            //
            // txtDiscountValue
            //
            this.txtDiscountValue.Location = new System.Drawing.Point(2,2);
            this.txtDiscountValue.Name = "txtDiscountValue";
            this.txtDiscountValue.Size = new System.Drawing.Size(150, 28);
            //
            // lblDiscountValue
            //
            this.lblDiscountValue.Location = new System.Drawing.Point(2,2);
            this.lblDiscountValue.Name = "lblDiscountValue";
            this.lblDiscountValue.AutoSize = true;
            this.lblDiscountValue.Text = "Discount &Value (%):";
            this.lblDiscountValue.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.lblDiscountValue, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.txtDiscountValue, 1, 4);
            this.grpMiscDefaults.Text = "Misc Defaults";
            this.tableLayoutPanel4.Controls.Add(this.grpMiscDefaults, 2, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.grpMiscDefaults, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlDefaults, 0, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlDefaults, 2);
            //
            // grpAccountInformation
            //
            this.grpAccountInformation.Location = new System.Drawing.Point(2,2);
            this.grpAccountInformation.Name = "grpAccountInformation";
            this.grpAccountInformation.AutoSize = true;
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.AutoSize = true;
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 4;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.grpAccountInformation.Controls.Add(this.tableLayoutPanel7);
            //
            // cmbAPAccount
            //
            this.cmbAPAccount.Location = new System.Drawing.Point(2,2);
            this.cmbAPAccount.Name = "cmbAPAccount";
            this.cmbAPAccount.Size = new System.Drawing.Size(150, 28);
            //
            // lblAPAccount
            //
            this.lblAPAccount.Location = new System.Drawing.Point(2,2);
            this.lblAPAccount.Name = "lblAPAccount";
            this.lblAPAccount.AutoSize = true;
            this.lblAPAccount.Text = "&AP Account:";
            this.lblAPAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblAPAccount, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.cmbAPAccount, 1, 0);
            //
            // cmbDefaultBankAccount
            //
            this.cmbDefaultBankAccount.Location = new System.Drawing.Point(2,2);
            this.cmbDefaultBankAccount.Name = "cmbDefaultBankAccount";
            this.cmbDefaultBankAccount.Size = new System.Drawing.Size(150, 28);
            //
            // lblDefaultBankAccount
            //
            this.lblDefaultBankAccount.Location = new System.Drawing.Point(2,2);
            this.lblDefaultBankAccount.Name = "lblDefaultBankAccount";
            this.lblDefaultBankAccount.AutoSize = true;
            this.lblDefaultBankAccount.Text = "Default &Bank Account:";
            this.lblDefaultBankAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblDefaultBankAccount, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.cmbDefaultBankAccount, 1, 1);
            //
            // cmbCostCentre
            //
            this.cmbCostCentre.Location = new System.Drawing.Point(2,2);
            this.cmbCostCentre.Name = "cmbCostCentre";
            this.cmbCostCentre.Size = new System.Drawing.Size(150, 28);
            //
            // lblCostCentre
            //
            this.lblCostCentre.Location = new System.Drawing.Point(2,2);
            this.lblCostCentre.Name = "lblCostCentre";
            this.lblCostCentre.AutoSize = true;
            this.lblCostCentre.Text = "Default C&ost Centre:";
            this.lblCostCentre.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblCostCentre, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.cmbCostCentre, 1, 2);
            //
            // cmbExpenseAccount
            //
            this.cmbExpenseAccount.Location = new System.Drawing.Point(2,2);
            this.cmbExpenseAccount.Name = "cmbExpenseAccount";
            this.cmbExpenseAccount.Size = new System.Drawing.Size(150, 28);
            //
            // lblExpenseAccount
            //
            this.lblExpenseAccount.Location = new System.Drawing.Point(2,2);
            this.lblExpenseAccount.Name = "lblExpenseAccount";
            this.lblExpenseAccount.AutoSize = true;
            this.lblExpenseAccount.Text = "Default &Expense Account:";
            this.lblExpenseAccount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.lblExpenseAccount, 0, 3);
            this.tableLayoutPanel7.Controls.Add(this.cmbExpenseAccount, 1, 3);
            this.grpAccountInformation.Text = "Account Information";
            this.tableLayoutPanel1.Controls.Add(this.grpAccountInformation, 0, 3);
            this.tableLayoutPanel1.SetColumnSpan(this.grpAccountInformation, 2);
            //
            // tbbSave
            //
            this.tbbSave.Name = "tbbSave";
            this.tbbSave.AutoSize = true;
            this.tbbSave.Click += new System.EventHandler(this.tbbSaveClick);
            this.tbbSave.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSave.Glyph"));
            this.tbbSave.ToolTipText = "Saves changed data";
            this.tbbSave.Text = "&Save";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave});
            //
            // mniFileSave
            //
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.AutoSize = true;
            this.mniFileSave.Click += new System.EventHandler(this.mniFileSaveClick);
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
            this.mniClose.Click += new System.EventHandler(this.mniCloseClick);
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
            // TFrmAccountsPayableEditSupplier
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmAccountsPayableEditSupplier";
            this.Text = "AP Supplier Edit";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.grpAccountInformation.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.grpMiscDefaults.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.grpGeneralInformation.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlDefaults.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlPartnerInfo2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlPartnerInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlPartnerInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Button btnEditPartner;
        private System.Windows.Forms.Panel pnlPartnerInfo2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtPartnerName;
        private System.Windows.Forms.Label lblPartnerName;
        private System.Windows.Forms.Panel pnlDefaults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox grpGeneralInformation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private Ict.Common.Controls.TCmbAutoComplete cmbSupplierType;
        private System.Windows.Forms.Label lblSupplierType;
        private System.Windows.Forms.GroupBox grpMiscDefaults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.NumericUpDown nudInvoiceAging;
        private System.Windows.Forms.Label lblInvoiceAging;
        private System.Windows.Forms.NumericUpDown nudCreditTerms;
        private System.Windows.Forms.Label lblCreditTerms;
        private Ict.Common.Controls.TCmbAutoComplete cmbDefaultPaymentType;
        private System.Windows.Forms.Label lblDefaultPaymentType;
        private System.Windows.Forms.NumericUpDown nudDiscountDays;
        private System.Windows.Forms.Label lblDiscountDays;
        private System.Windows.Forms.TextBox txtDiscountValue;
        private System.Windows.Forms.Label lblDiscountValue;
        private System.Windows.Forms.GroupBox grpAccountInformation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private Ict.Common.Controls.TCmbAutoComplete cmbAPAccount;
        private System.Windows.Forms.Label lblAPAccount;
        private Ict.Common.Controls.TCmbAutoComplete cmbDefaultBankAccount;
        private System.Windows.Forms.Label lblDefaultBankAccount;
        private Ict.Common.Controls.TCmbAutoComplete cmbCostCentre;
        private System.Windows.Forms.Label lblCostCentre;
        private Ict.Common.Controls.TCmbAutoComplete cmbExpenseAccount;
        private System.Windows.Forms.Label lblExpenseAccount;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
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
