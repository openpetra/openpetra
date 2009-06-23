/* auto generated with nant generateWinforms from APMain.yaml
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
//using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MFinance.Gui
{
    partial class TFrmAccountsPayableMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmAccountsPayableMain));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSearchCriteria = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbSupplierCode = new System.Windows.Forms.ComboBox();
            this.lblSupplierCode = new System.Windows.Forms.Label();
            this.rgrPartnerKeyOrName = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPartnerKey = new System.Windows.Forms.RadioButton();
            this.rbtName = new System.Windows.Forms.RadioButton();
            this.pnlSearchFilter = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.chkDueToday = new System.Windows.Forms.CheckBox();
            this.chkOverdue = new System.Windows.Forms.CheckBox();
            this.chkDueFuture = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.nudNumberTimeUnits = new System.Windows.Forms.NumericUpDown();
            this.cmbTimeUnit = new System.Windows.Forms.ComboBox();
            this.pnlSearchButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tabSearchResult = new System.Windows.Forms.TabControl();
            this.tpgSuppliers = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSupplierOptions = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.chkShowOutstandingAmounts = new System.Windows.Forms.CheckBox();
            this.chkHideInactiveSuppliers = new System.Windows.Forms.CheckBox();
            this.cmbSupplierCurrency = new System.Windows.Forms.ComboBox();
            this.lblSupplierCurrency = new System.Windows.Forms.Label();
            this.grdSupplierResult = new System.Windows.Forms.DataGridView();
            this.tpgOutstandingInvoices = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTaggingOptions = new System.Windows.Forms.Panel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTagAllApprovable = new System.Windows.Forms.Button();
            this.btnTagAllPostable = new System.Windows.Forms.Button();
            this.btnTagAllPayable = new System.Windows.Forms.Button();
            this.btnUntagAll = new System.Windows.Forms.Button();
            this.txtSumTagged = new System.Windows.Forms.TextBox();
            this.lblSumTagged = new System.Windows.Forms.Label();
            this.grdInvoiceResult = new System.Windows.Forms.DataGridView();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbTransactions = new System.Windows.Forms.ToolStripButton();
            this.tbbEditSupplier = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbNewSupplier = new System.Windows.Forms.ToolStripButton();
            this.tbbSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbCreateInvoice = new System.Windows.Forms.ToolStripButton();
            this.tbbCreateCreditNote = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTodo1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSupplier = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTodo2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFind = new System.Windows.Forms.ToolStripMenuItem();
            this.mniTodo3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlSearchCriteria.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rgrPartnerKeyOrName.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlSearchFilter.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlSearchButtons.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tabSearchResult.SuspendLayout();
            this.tpgSuppliers.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.pnlSupplierOptions.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tpgOutstandingInvoices.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.pnlTaggingOptions.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
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
            // pnlSearchCriteria
            //
            this.pnlSearchCriteria.Name = "pnlSearchCriteria";
            this.pnlSearchCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchCriteria.AutoSize = true;
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
            this.pnlSearchCriteria.Controls.Add(this.tableLayoutPanel2);
            //
            // cmbSupplierCode
            //
            this.cmbSupplierCode.Location = new System.Drawing.Point(2,2);
            this.cmbSupplierCode.Name = "cmbSupplierCode";
            this.cmbSupplierCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblSupplierCode
            //
            this.lblSupplierCode.Location = new System.Drawing.Point(2,2);
            this.lblSupplierCode.Name = "lblSupplierCode";
            this.lblSupplierCode.AutoSize = true;
            this.lblSupplierCode.Text = "S&earch Supplier:";
            this.lblSupplierCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSupplierCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbSupplierCode, 1, 0);
            //
            // rgrPartnerKeyOrName
            //
            this.rgrPartnerKeyOrName.Location = new System.Drawing.Point(2,2);
            this.rgrPartnerKeyOrName.Name = "rgrPartnerKeyOrName";
            this.rgrPartnerKeyOrName.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.rgrPartnerKeyOrName.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtPartnerKey
            //
            this.rbtPartnerKey.Location = new System.Drawing.Point(2,2);
            this.rbtPartnerKey.Name = "rbtPartnerKey";
            this.rbtPartnerKey.AutoSize = true;
            this.rbtPartnerKey.Text = "&Partner Key";
            this.rbtPartnerKey.Checked = true;
            this.tableLayoutPanel3.Controls.Add(this.rbtPartnerKey, 0, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.rbtPartnerKey, 2);
            //
            // rbtName
            //
            this.rbtName.Location = new System.Drawing.Point(2,2);
            this.rbtName.Name = "rbtName";
            this.rbtName.AutoSize = true;
            this.rbtName.Text = "N&ame";
            this.tableLayoutPanel3.Controls.Add(this.rbtName, 2, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.rbtName, 2);
            this.rgrPartnerKeyOrName.Text = "PartnerKeyOrName";
            this.tableLayoutPanel2.Controls.Add(this.rgrPartnerKeyOrName, 2, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.rgrPartnerKeyOrName, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlSearchCriteria, 0, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlSearchCriteria, 2);
            //
            // pnlSearchFilter
            //
            this.pnlSearchFilter.Name = "pnlSearchFilter";
            this.pnlSearchFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchFilter.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSearchFilter.Controls.Add(this.tableLayoutPanel4);
            //
            // chkDueToday
            //
            this.chkDueToday.Location = new System.Drawing.Point(2,2);
            this.chkDueToday.Name = "chkDueToday";
            this.chkDueToday.AutoSize = true;
            this.chkDueToday.Text = "Due &Today";
            this.chkDueToday.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkDueToday, 0, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.chkDueToday, 2);
            //
            // chkOverdue
            //
            this.chkOverdue.Location = new System.Drawing.Point(2,2);
            this.chkOverdue.Name = "chkOverdue";
            this.chkOverdue.AutoSize = true;
            this.chkOverdue.Text = "&Overdue";
            this.chkOverdue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkOverdue, 2, 0);
            this.tableLayoutPanel4.SetColumnSpan(this.chkOverdue, 2);
            //
            // chkDueFuture
            //
            this.chkDueFuture.Location = new System.Drawing.Point(2,2);
            this.chkDueFuture.Name = "chkDueFuture";
            this.chkDueFuture.AutoSize = true;
            this.chkDueFuture.Text = "Due &Within Future";
            this.chkDueFuture.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkDueFuture, 4, 0);
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            //
            // nudNumberTimeUnits
            //
            this.nudNumberTimeUnits.Location = new System.Drawing.Point(2,2);
            this.nudNumberTimeUnits.Name = "nudNumberTimeUnits";
            this.nudNumberTimeUnits.Size = new System.Drawing.Size(150, 28);
            this.tableLayoutPanel5.Controls.Add(this.nudNumberTimeUnits, 0, 0);
            this.tableLayoutPanel5.SetColumnSpan(this.nudNumberTimeUnits, 2);
            //
            // cmbTimeUnit
            //
            this.cmbTimeUnit.Location = new System.Drawing.Point(2,2);
            this.cmbTimeUnit.Name = "cmbTimeUnit";
            this.cmbTimeUnit.Size = new System.Drawing.Size(150, 28);
            this.tableLayoutPanel5.Controls.Add(this.cmbTimeUnit, 2, 0);
            this.tableLayoutPanel5.SetColumnSpan(this.cmbTimeUnit, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 5, 0);
            this.chkDueFuture.CheckedChanged += new System.EventHandler(this.chkDueFutureCheckedChanged);
            this.tableLayoutPanel1.Controls.Add(this.pnlSearchFilter, 0, 1);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlSearchFilter, 2);
            //
            // pnlSearchButtons
            //
            this.pnlSearchButtons.Location = new System.Drawing.Point(2,2);
            this.pnlSearchButtons.Name = "pnlSearchButtons";
            this.pnlSearchButtons.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSearchButtons.Controls.Add(this.tableLayoutPanel6);
            //
            // btnSearch
            //
            this.btnSearch.Location = new System.Drawing.Point(2,2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.AutoSize = true;
            this.btnSearch.Text = "&Search";
            this.tableLayoutPanel6.Controls.Add(this.btnSearch, 0, 0);
            this.tableLayoutPanel6.SetColumnSpan(this.btnSearch, 2);
            //
            // btnReset
            //
            this.btnReset.Location = new System.Drawing.Point(2,2);
            this.btnReset.Name = "btnReset";
            this.btnReset.AutoSize = true;
            this.btnReset.Text = "&Reset Criteria";
            this.tableLayoutPanel6.Controls.Add(this.btnReset, 2, 0);
            this.tableLayoutPanel6.SetColumnSpan(this.btnReset, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlSearchButtons, 0, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.pnlSearchButtons, 2);
            //
            // tpgSuppliers
            //
            this.tpgSuppliers.Location = new System.Drawing.Point(2,2);
            this.tpgSuppliers.Name = "tpgSuppliers";
            this.tpgSuppliers.AutoSize = true;
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
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpgSuppliers.Controls.Add(this.tableLayoutPanel7);
            //
            // pnlSupplierOptions
            //
            this.pnlSupplierOptions.Location = new System.Drawing.Point(2,2);
            this.pnlSupplierOptions.Name = "pnlSupplierOptions";
            this.pnlSupplierOptions.AutoSize = true;
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.AutoSize = true;
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.ColumnCount = 6;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlSupplierOptions.Controls.Add(this.tableLayoutPanel8);
            //
            // chkShowOutstandingAmounts
            //
            this.chkShowOutstandingAmounts.Location = new System.Drawing.Point(2,2);
            this.chkShowOutstandingAmounts.Name = "chkShowOutstandingAmounts";
            this.chkShowOutstandingAmounts.AutoSize = true;
            this.chkShowOutstandingAmounts.Text = "Show Outstanding &Amounts";
            this.chkShowOutstandingAmounts.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.chkShowOutstandingAmounts, 0, 0);
            this.tableLayoutPanel8.SetColumnSpan(this.chkShowOutstandingAmounts, 2);
            //
            // chkHideInactiveSuppliers
            //
            this.chkHideInactiveSuppliers.Location = new System.Drawing.Point(2,2);
            this.chkHideInactiveSuppliers.Name = "chkHideInactiveSuppliers";
            this.chkHideInactiveSuppliers.AutoSize = true;
            this.chkHideInactiveSuppliers.Text = "Hide &Inactive Suppliers";
            this.chkHideInactiveSuppliers.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.chkHideInactiveSuppliers, 2, 0);
            this.tableLayoutPanel8.SetColumnSpan(this.chkHideInactiveSuppliers, 2);
            //
            // cmbSupplierCurrency
            //
            this.cmbSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbSupplierCurrency.Name = "cmbSupplierCurrency";
            this.cmbSupplierCurrency.Size = new System.Drawing.Size(150, 28);
            //
            // lblSupplierCurrency
            //
            this.lblSupplierCurrency.Location = new System.Drawing.Point(2,2);
            this.lblSupplierCurrency.Name = "lblSupplierCurrency";
            this.lblSupplierCurrency.AutoSize = true;
            this.lblSupplierCurrency.Text = "C&urrency:";
            this.lblSupplierCurrency.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.lblSupplierCurrency, 4, 0);
            this.tableLayoutPanel8.Controls.Add(this.cmbSupplierCurrency, 5, 0);
            this.tableLayoutPanel7.Controls.Add(this.pnlSupplierOptions, 0, 0);
            this.tableLayoutPanel7.SetColumnSpan(this.pnlSupplierOptions, 2);
            //
            // grdSupplierResult
            //
            this.grdSupplierResult.Name = "grdSupplierResult";
            this.grdSupplierResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Controls.Add(this.grdSupplierResult, 0, 1);
            this.tableLayoutPanel7.SetColumnSpan(this.grdSupplierResult, 2);
            this.tpgSuppliers.Text = "Suppliers";
            //
            // tpgOutstandingInvoices
            //
            this.tpgOutstandingInvoices.Location = new System.Drawing.Point(2,2);
            this.tpgOutstandingInvoices.Name = "tpgOutstandingInvoices";
            this.tpgOutstandingInvoices.AutoSize = true;
            //
            // tableLayoutPanel9
            //
            this.tableLayoutPanel9.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.AutoSize = true;
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tpgOutstandingInvoices.Controls.Add(this.tableLayoutPanel9);
            //
            // pnlTaggingOptions
            //
            this.pnlTaggingOptions.Location = new System.Drawing.Point(2,2);
            this.pnlTaggingOptions.Name = "pnlTaggingOptions";
            this.pnlTaggingOptions.AutoSize = true;
            //
            // tableLayoutPanel10
            //
            this.tableLayoutPanel10.Location = new System.Drawing.Point(2,2);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.ColumnCount = 10;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlTaggingOptions.Controls.Add(this.tableLayoutPanel10);
            //
            // btnTagAllApprovable
            //
            this.btnTagAllApprovable.Location = new System.Drawing.Point(2,2);
            this.btnTagAllApprovable.Name = "btnTagAllApprovable";
            this.btnTagAllApprovable.AutoSize = true;
            this.btnTagAllApprovable.Text = "Tag all Appro&vable";
            this.tableLayoutPanel10.Controls.Add(this.btnTagAllApprovable, 0, 0);
            this.tableLayoutPanel10.SetColumnSpan(this.btnTagAllApprovable, 2);
            //
            // btnTagAllPostable
            //
            this.btnTagAllPostable.Location = new System.Drawing.Point(2,2);
            this.btnTagAllPostable.Name = "btnTagAllPostable";
            this.btnTagAllPostable.AutoSize = true;
            this.btnTagAllPostable.Text = "Tag a&ll Postable";
            this.tableLayoutPanel10.Controls.Add(this.btnTagAllPostable, 2, 0);
            this.tableLayoutPanel10.SetColumnSpan(this.btnTagAllPostable, 2);
            //
            // btnTagAllPayable
            //
            this.btnTagAllPayable.Location = new System.Drawing.Point(2,2);
            this.btnTagAllPayable.Name = "btnTagAllPayable";
            this.btnTagAllPayable.AutoSize = true;
            this.btnTagAllPayable.Text = "Tag all Paya&ble";
            this.tableLayoutPanel10.Controls.Add(this.btnTagAllPayable, 4, 0);
            this.tableLayoutPanel10.SetColumnSpan(this.btnTagAllPayable, 2);
            //
            // btnUntagAll
            //
            this.btnUntagAll.Location = new System.Drawing.Point(2,2);
            this.btnUntagAll.Name = "btnUntagAll";
            this.btnUntagAll.AutoSize = true;
            this.btnUntagAll.Text = "&Untag all";
            this.tableLayoutPanel10.Controls.Add(this.btnUntagAll, 6, 0);
            this.tableLayoutPanel10.SetColumnSpan(this.btnUntagAll, 2);
            //
            // txtSumTagged
            //
            this.txtSumTagged.Location = new System.Drawing.Point(2,2);
            this.txtSumTagged.Name = "txtSumTagged";
            this.txtSumTagged.Size = new System.Drawing.Size(150, 28);
            this.txtSumTagged.ReadOnly = true;
            //
            // lblSumTagged
            //
            this.lblSumTagged.Location = new System.Drawing.Point(2,2);
            this.lblSumTagged.Name = "lblSumTagged";
            this.lblSumTagged.AutoSize = true;
            this.lblSumTagged.Text = "Sum of Tagged:";
            this.lblSumTagged.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel10.Controls.Add(this.lblSumTagged, 8, 0);
            this.tableLayoutPanel10.Controls.Add(this.txtSumTagged, 9, 0);
            this.tableLayoutPanel9.Controls.Add(this.pnlTaggingOptions, 0, 0);
            this.tableLayoutPanel9.SetColumnSpan(this.pnlTaggingOptions, 2);
            //
            // grdInvoiceResult
            //
            this.grdInvoiceResult.Name = "grdInvoiceResult";
            this.grdInvoiceResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Controls.Add(this.grdInvoiceResult, 0, 1);
            this.tableLayoutPanel9.SetColumnSpan(this.grdInvoiceResult, 2);
            this.tpgOutstandingInvoices.Text = "OutstandingInvoices";
            //
            // tabSearchResult
            //
            this.tabSearchResult.Name = "tabSearchResult";
            this.tabSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSearchResult.Controls.Add(this.tpgSuppliers);
            this.tabSearchResult.Controls.Add(this.tpgOutstandingInvoices);
            this.tabSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(this.tabSearchResult, 0, 3);
            this.tableLayoutPanel1.SetColumnSpan(this.tabSearchResult, 2);
            //
            // tbbTransactions
            //
            this.tbbTransactions.Name = "tbbTransactions";
            this.tbbTransactions.AutoSize = true;
            this.tbbTransactions.Click += new System.EventHandler(this.tbbTransactionsClick);
            this.tbbTransactions.ToolTipText = "Open the transactions of the supplier";
            this.tbbTransactions.Text = "Open Transactions";
            //
            // tbbEditSupplier
            //
            this.tbbEditSupplier.Name = "tbbEditSupplier";
            this.tbbEditSupplier.AutoSize = true;
            this.tbbEditSupplier.Text = "Edit Supplier";
            //
            // tbbSeparator0
            //
            this.tbbSeparator0.Name = "tbbSeparator0";
            this.tbbSeparator0.AutoSize = true;
            this.tbbSeparator0.Text = "-";
            //
            // tbbNewSupplier
            //
            this.tbbNewSupplier.Name = "tbbNewSupplier";
            this.tbbNewSupplier.AutoSize = true;
            this.tbbNewSupplier.Text = "New Supplier";
            //
            // tbbSeparator1
            //
            this.tbbSeparator1.Name = "tbbSeparator1";
            this.tbbSeparator1.AutoSize = true;
            this.tbbSeparator1.Text = "-";
            //
            // tbbCreateInvoice
            //
            this.tbbCreateInvoice.Name = "tbbCreateInvoice";
            this.tbbCreateInvoice.AutoSize = true;
            this.tbbCreateInvoice.Text = "Create Invoice";
            //
            // tbbCreateCreditNote
            //
            this.tbbCreateCreditNote.Name = "tbbCreateCreditNote";
            this.tbbCreateCreditNote.AutoSize = true;
            this.tbbCreateCreditNote.Text = "Create Credit Note";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbTransactions,
                        tbbEditSupplier,
                        tbbSeparator0,
                        tbbNewSupplier,
                        tbbSeparator1,
                        tbbCreateInvoice,
                        tbbCreateCreditNote});
            //
            // mniTodo1
            //
            this.mniTodo1.Name = "mniTodo1";
            this.mniTodo1.AutoSize = true;
            this.mniTodo1.Text = "todo";
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
                           mniTodo1,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniTodo2
            //
            this.mniTodo2.Name = "mniTodo2";
            this.mniTodo2.AutoSize = true;
            this.mniTodo2.Text = "todo";
            //
            // mniSupplier
            //
            this.mniSupplier.Name = "mniSupplier";
            this.mniSupplier.AutoSize = true;
            this.mniSupplier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniTodo2});
            this.mniSupplier.Text = "Supplier";
            //
            // mniTodo3
            //
            this.mniTodo3.Name = "mniTodo3";
            this.mniTodo3.AutoSize = true;
            this.mniTodo3.Text = "todo";
            //
            // mniFind
            //
            this.mniFind.Name = "mniFind";
            this.mniFind.AutoSize = true;
            this.mniFind.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniTodo3});
            this.mniFind.Text = "Find";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
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
                        mniSeparator0,
                        mniHelpBugReport,
                        mniSeparator1,
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
                        mniSupplier,
                        mniFind,
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmAccountsPayableMain
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 623);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmAccountsPayableMain";
            this.Text = "Accounts Payable";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.pnlTaggingOptions.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tpgOutstandingInvoices.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.pnlSupplierOptions.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tpgSuppliers.ResumeLayout(false);
            this.tabSearchResult.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlSearchButtons.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlSearchFilter.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.rgrPartnerKeyOrName.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlSearchCriteria.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlSearchCriteria;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cmbSupplierCode;
        private System.Windows.Forms.Label lblSupplierCode;
        private System.Windows.Forms.Panel rgrPartnerKeyOrName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtPartnerKey;
        private System.Windows.Forms.RadioButton rbtName;
        private System.Windows.Forms.Panel pnlSearchFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox chkDueToday;
        private System.Windows.Forms.CheckBox chkOverdue;
        private System.Windows.Forms.CheckBox chkDueFuture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.NumericUpDown nudNumberTimeUnits;
        private System.Windows.Forms.ComboBox cmbTimeUnit;
        private System.Windows.Forms.Panel pnlSearchButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabControl tabSearchResult;
        private System.Windows.Forms.TabPage tpgSuppliers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel pnlSupplierOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.CheckBox chkShowOutstandingAmounts;
        private System.Windows.Forms.CheckBox chkHideInactiveSuppliers;
        private System.Windows.Forms.ComboBox cmbSupplierCurrency;
        private System.Windows.Forms.Label lblSupplierCurrency;
        private System.Windows.Forms.DataGridView grdSupplierResult;
        private System.Windows.Forms.TabPage tpgOutstandingInvoices;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Panel pnlTaggingOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Button btnTagAllApprovable;
        private System.Windows.Forms.Button btnTagAllPostable;
        private System.Windows.Forms.Button btnTagAllPayable;
        private System.Windows.Forms.Button btnUntagAll;
        private System.Windows.Forms.TextBox txtSumTagged;
        private System.Windows.Forms.Label lblSumTagged;
        private System.Windows.Forms.DataGridView grdInvoiceResult;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbTransactions;
        private System.Windows.Forms.ToolStripButton tbbEditSupplier;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator0;
        private System.Windows.Forms.ToolStripButton tbbNewSupplier;
        private System.Windows.Forms.ToolStripSeparator tbbSeparator1;
        private System.Windows.Forms.ToolStripButton tbbCreateInvoice;
        private System.Windows.Forms.ToolStripButton tbbCreateCreditNote;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniTodo1;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniSupplier;
        private System.Windows.Forms.ToolStripMenuItem mniTodo2;
        private System.Windows.Forms.ToolStripMenuItem mniFind;
        private System.Windows.Forms.ToolStripMenuItem mniTodo3;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
