// auto generated with nant generateWinforms from TotalGivingForRecipients.yaml
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

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    partial class TFrmTotalGivingForRecipients
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmTotalGivingForRecipients));

            this.tabReportSettings = new Ict.Common.Controls.TTabVersatile();
            this.tpgGeneralSettings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLedger = new System.Windows.Forms.Label();
            this.rgrDonorSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPartner = new System.Windows.Forms.RadioButton();
            this.txtRecipient = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtExtract = new System.Windows.Forms.RadioButton();
            this.txtExtract = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtAllRecipients = new System.Windows.Forms.RadioButton();
            this.grpCurrencySelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCurrency = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.tpgAdditionalSettings = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrFormatCurrency = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtCurrencyComplete = new System.Windows.Forms.RadioButton();
            this.rbtCurrencyWithoutDecimals = new System.Windows.Forms.RadioButton();
            this.rbtCurrencyThousands = new System.Windows.Forms.RadioButton();
            this.tpgFields = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrFields = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAllFields = new System.Windows.Forms.RadioButton();
            this.rbtSelectedFields = new System.Windows.Forms.RadioButton();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.clbFields = new Ict.Common.Controls.TClbVersatile();
            this.pnlFieldButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSelectAllFields = new System.Windows.Forms.Button();
            this.btnUnselectAllFields = new System.Windows.Forms.Button();
            this.tpgTypes = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.rgrTypes = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAllTypes = new System.Windows.Forms.RadioButton();
            this.rbtSelectedTypes = new System.Windows.Forms.RadioButton();
            this.pnlTypes = new System.Windows.Forms.Panel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.clbTypes = new Ict.Common.Controls.TClbVersatile();
            this.pnlTypeButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSelectAllTypes = new System.Windows.Forms.Button();
            this.btnUnselectAllTypes = new System.Windows.Forms.Button();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbGenerateReport = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettings = new System.Windows.Forms.ToolStripButton();
            this.tbbSaveSettingsAs = new System.Windows.Forms.ToolStripButton();
            this.tbbLoadSettingsDialog = new System.Windows.Forms.ToolStripButton();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettingsDialog = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.mniLoadSettings1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniLoadSettings5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSaveSettingsAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mniMaintainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mniWrapColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniGenerateReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpPetraHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpBugReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniHelpAboutPetra = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpDevelopmentTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.tabReportSettings.SuspendLayout();
            this.tpgGeneralSettings.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.rgrDonorSelection.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpCurrencySelection.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tpgAdditionalSettings.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.rgrFormatCurrency.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tpgFields.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.rgrFields.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.pnlFields.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.pnlFieldButtons.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tpgTypes.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.rgrTypes.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.pnlTypes.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.pnlTypeButtons.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // tpgGeneralSettings
            //
            this.tpgGeneralSettings.Location = new System.Drawing.Point(2,2);
            this.tpgGeneralSettings.Name = "tpgGeneralSettings";
            this.tpgGeneralSettings.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.tpgGeneralSettings.Controls.Add(this.tableLayoutPanel1);
            //
            // lblLedger
            //
            this.lblLedger.Location = new System.Drawing.Point(2,2);
            this.lblLedger.Name = "lblLedger";
            this.lblLedger.AutoSize = true;
            this.lblLedger.Text = "Ledger:";
            this.lblLedger.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // rgrDonorSelection
            //
            this.rgrDonorSelection.Name = "rgrDonorSelection";
            this.rgrDonorSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgrDonorSelection.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.rgrDonorSelection.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtPartner
            //
            this.rbtPartner.Location = new System.Drawing.Point(2,2);
            this.rbtPartner.Name = "rbtPartner";
            this.rbtPartner.AutoSize = true;
            this.rbtPartner.Text = "One Recipient";
            this.rbtPartner.Checked = true;
            //
            // txtRecipient
            //
            this.txtRecipient.Location = new System.Drawing.Point(2,2);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.Size = new System.Drawing.Size(400, 28);
            this.txtRecipient.ASpecialSetting = true;
            this.txtRecipient.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtRecipient.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtRecipient.PartnerClass = "";
            this.txtRecipient.MaxLength = 32767;
            this.txtRecipient.Tag = "CustomDisableAlthoughInvisible";
            this.txtRecipient.TextBoxWidth = 80;
            this.txtRecipient.ButtonWidth = 40;
            this.txtRecipient.ReadOnly = false;
            this.txtRecipient.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtRecipient.ButtonText = "Find";
            this.rbtPartner.CheckedChanged += new System.EventHandler(this.rbtPartnerCheckedChanged);
            //
            // rbtExtract
            //
            this.rbtExtract.Location = new System.Drawing.Point(2,2);
            this.rbtExtract.Name = "rbtExtract";
            this.rbtExtract.AutoSize = true;
            this.rbtExtract.Text = "Recipients from Extract";
            //
            // txtExtract
            //
            this.txtExtract.Location = new System.Drawing.Point(2,2);
            this.txtExtract.Name = "txtExtract";
            this.txtExtract.Size = new System.Drawing.Size(400, 28);
            this.txtExtract.ASpecialSetting = true;
            this.txtExtract.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtExtract.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.Extract;
            this.txtExtract.PartnerClass = "";
            this.txtExtract.MaxLength = 32767;
            this.txtExtract.Tag = "CustomDisableAlthoughInvisible";
            this.txtExtract.TextBoxWidth = 80;
            this.txtExtract.ButtonWidth = 40;
            this.txtExtract.ReadOnly = false;
            this.txtExtract.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtExtract.ButtonText = "Find";
            this.rbtExtract.CheckedChanged += new System.EventHandler(this.rbtExtractCheckedChanged);
            //
            // rbtAllRecipients
            //
            this.rbtAllRecipients.Location = new System.Drawing.Point(2,2);
            this.rbtAllRecipients.Name = "rbtAllRecipients";
            this.rbtAllRecipients.AutoSize = true;
            this.rbtAllRecipients.Text = "All Recipients";
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtPartner, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtExtract, 0, 1);
            this.tableLayoutPanel2.SetColumnSpan(this.rbtAllRecipients, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtAllRecipients, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtRecipient, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtExtract, 1, 1);
            this.rgrDonorSelection.Text = "Select Donors";
            //
            // grpCurrencySelection
            //
            this.grpCurrencySelection.Name = "grpCurrencySelection";
            this.grpCurrencySelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCurrencySelection.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpCurrencySelection.Controls.Add(this.tableLayoutPanel3);
            //
            // cmbCurrency
            //
            this.cmbCurrency.Location = new System.Drawing.Point(2,2);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(150, 28);
            this.cmbCurrency.Items.AddRange(new object[] {"Base","International"});
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
            this.grpCurrencySelection.Text = "Gift Limit";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblLedger, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rgrDonorSelection, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpCurrencySelection, 0, 2);
            this.tpgGeneralSettings.Text = "General Settings";
            this.tpgGeneralSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgAdditionalSettings
            //
            this.tpgAdditionalSettings.Location = new System.Drawing.Point(2,2);
            this.tpgAdditionalSettings.Name = "tpgAdditionalSettings";
            this.tpgAdditionalSettings.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.tpgAdditionalSettings.Controls.Add(this.tableLayoutPanel4);
            //
            // rgrFormatCurrency
            //
            this.rgrFormatCurrency.Location = new System.Drawing.Point(2,2);
            this.rgrFormatCurrency.Name = "rgrFormatCurrency";
            this.rgrFormatCurrency.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.rgrFormatCurrency.Controls.Add(this.tableLayoutPanel5);
            //
            // rbtCurrencyComplete
            //
            this.rbtCurrencyComplete.Location = new System.Drawing.Point(2,2);
            this.rbtCurrencyComplete.Name = "rbtCurrencyComplete";
            this.rbtCurrencyComplete.AutoSize = true;
            this.rbtCurrencyComplete.Text = "Complete";
            this.rbtCurrencyComplete.Checked = true;
            //
            // rbtCurrencyWithoutDecimals
            //
            this.rbtCurrencyWithoutDecimals.Location = new System.Drawing.Point(2,2);
            this.rbtCurrencyWithoutDecimals.Name = "rbtCurrencyWithoutDecimals";
            this.rbtCurrencyWithoutDecimals.AutoSize = true;
            this.rbtCurrencyWithoutDecimals.Text = "Without decimals";
            //
            // rbtCurrencyThousands
            //
            this.rbtCurrencyThousands.Location = new System.Drawing.Point(2,2);
            this.rbtCurrencyThousands.Name = "rbtCurrencyThousands";
            this.rbtCurrencyThousands.AutoSize = true;
            this.rbtCurrencyThousands.Text = "Only Thousands";
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.rbtCurrencyComplete, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.rbtCurrencyWithoutDecimals, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.rbtCurrencyThousands, 0, 2);
            this.rgrFormatCurrency.Text = "Format currency numbers:";
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.rgrFormatCurrency, 0, 0);
            this.tpgAdditionalSettings.Text = "Additional Settings";
            this.tpgAdditionalSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgFields
            //
            this.tpgFields.Location = new System.Drawing.Point(2,2);
            this.tpgFields.Name = "tpgFields";
            this.tpgFields.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.tpgFields.Controls.Add(this.tableLayoutPanel6);
            //
            // rgrFields
            //
            this.rgrFields.Location = new System.Drawing.Point(2,2);
            this.rgrFields.Name = "rgrFields";
            this.rgrFields.AutoSize = true;
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            this.rgrFields.Controls.Add(this.tableLayoutPanel7);
            //
            // rbtAllFields
            //
            this.rbtAllFields.Location = new System.Drawing.Point(2,2);
            this.rbtAllFields.Name = "rbtAllFields";
            this.rbtAllFields.AutoSize = true;
            this.rbtAllFields.Text = "All Fields";
            this.rbtAllFields.Checked = true;
            //
            // rbtSelectedFields
            //
            this.rbtSelectedFields.Location = new System.Drawing.Point(2,2);
            this.rbtSelectedFields.Name = "rbtSelectedFields";
            this.rbtSelectedFields.AutoSize = true;
            this.rbtSelectedFields.Text = "Selected Fields";
            //
            // pnlFields
            //
            this.pnlFields.Location = new System.Drawing.Point(2,2);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.AutoSize = true;
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.AutoSize = true;
            this.pnlFields.Controls.Add(this.tableLayoutPanel8);
            //
            // clbFields
            //
            this.clbFields.Name = "clbFields";
            this.clbFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbFields.Size = new System.Drawing.Size(365, 300);
            this.clbFields.FixedRows = 1;
            //
            // pnlFieldButtons
            //
            this.pnlFieldButtons.Location = new System.Drawing.Point(2,2);
            this.pnlFieldButtons.Name = "pnlFieldButtons";
            this.pnlFieldButtons.AutoSize = true;
            //
            // tableLayoutPanel9
            //
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.AutoSize = true;
            this.pnlFieldButtons.Controls.Add(this.tableLayoutPanel9);
            //
            // btnSelectAllFields
            //
            this.btnSelectAllFields.Location = new System.Drawing.Point(2,2);
            this.btnSelectAllFields.Name = "btnSelectAllFields";
            this.btnSelectAllFields.AutoSize = true;
            this.btnSelectAllFields.Click += new System.EventHandler(this.SelectAllFields);
            this.btnSelectAllFields.Text = "Select All";
            //
            // btnUnselectAllFields
            //
            this.btnUnselectAllFields.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllFields.Name = "btnUnselectAllFields";
            this.btnUnselectAllFields.AutoSize = true;
            this.btnUnselectAllFields.Click += new System.EventHandler(this.UnselectAllFields);
            this.btnUnselectAllFields.Text = "Unselect All";
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Controls.Add(this.btnSelectAllFields, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.btnUnselectAllFields, 1, 0);
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 2;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Controls.Add(this.clbFields, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.pnlFieldButtons, 0, 1);
            this.rbtSelectedFields.CheckedChanged += new System.EventHandler(this.rbtSelectedFieldsCheckedChanged);
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.SetColumnSpan(this.rbtAllFields, 2);
            this.tableLayoutPanel7.Controls.Add(this.rbtAllFields, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.rbtSelectedFields, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.pnlFields, 1, 1);
            this.rgrFields.Text = "Select Receiving Fields";
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.rgrFields, 0, 0);
            this.tpgFields.Text = "Fields";
            this.tpgFields.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgTypes
            //
            this.tpgTypes.Location = new System.Drawing.Point(2,2);
            this.tpgTypes.Name = "tpgTypes";
            this.tpgTypes.AutoSize = true;
            //
            // tableLayoutPanel10
            //
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.AutoSize = true;
            this.tpgTypes.Controls.Add(this.tableLayoutPanel10);
            //
            // rgrTypes
            //
            this.rgrTypes.Location = new System.Drawing.Point(2,2);
            this.rgrTypes.Name = "rgrTypes";
            this.rgrTypes.AutoSize = true;
            //
            // tableLayoutPanel11
            //
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.AutoSize = true;
            this.rgrTypes.Controls.Add(this.tableLayoutPanel11);
            //
            // rbtAllTypes
            //
            this.rbtAllTypes.Location = new System.Drawing.Point(2,2);
            this.rbtAllTypes.Name = "rbtAllTypes";
            this.rbtAllTypes.AutoSize = true;
            this.rbtAllTypes.Text = "All Types";
            this.rbtAllTypes.Checked = true;
            //
            // rbtSelectedTypes
            //
            this.rbtSelectedTypes.Location = new System.Drawing.Point(2,2);
            this.rbtSelectedTypes.Name = "rbtSelectedTypes";
            this.rbtSelectedTypes.AutoSize = true;
            this.rbtSelectedTypes.Text = "Selected Types";
            //
            // pnlTypes
            //
            this.pnlTypes.Location = new System.Drawing.Point(2,2);
            this.pnlTypes.Name = "pnlTypes";
            this.pnlTypes.AutoSize = true;
            //
            // tableLayoutPanel12
            //
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.AutoSize = true;
            this.pnlTypes.Controls.Add(this.tableLayoutPanel12);
            //
            // clbTypes
            //
            this.clbTypes.Name = "clbTypes";
            this.clbTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbTypes.Size = new System.Drawing.Size(365, 300);
            this.clbTypes.FixedRows = 1;
            //
            // pnlTypeButtons
            //
            this.pnlTypeButtons.Location = new System.Drawing.Point(2,2);
            this.pnlTypeButtons.Name = "pnlTypeButtons";
            this.pnlTypeButtons.AutoSize = true;
            //
            // tableLayoutPanel13
            //
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.AutoSize = true;
            this.pnlTypeButtons.Controls.Add(this.tableLayoutPanel13);
            //
            // btnSelectAllTypes
            //
            this.btnSelectAllTypes.Location = new System.Drawing.Point(2,2);
            this.btnSelectAllTypes.Name = "btnSelectAllTypes";
            this.btnSelectAllTypes.AutoSize = true;
            this.btnSelectAllTypes.Click += new System.EventHandler(this.SelectAllTypes);
            this.btnSelectAllTypes.Text = "Select All";
            //
            // btnUnselectAllTypes
            //
            this.btnUnselectAllTypes.Location = new System.Drawing.Point(2,2);
            this.btnUnselectAllTypes.Name = "btnUnselectAllTypes";
            this.btnUnselectAllTypes.AutoSize = true;
            this.btnUnselectAllTypes.Click += new System.EventHandler(this.UnselectAllTypes);
            this.btnUnselectAllTypes.Text = "Unselect All";
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Controls.Add(this.btnSelectAllTypes, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.btnUnselectAllTypes, 1, 0);
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.Controls.Add(this.clbTypes, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.pnlTypeButtons, 0, 1);
            this.rbtSelectedTypes.CheckedChanged += new System.EventHandler(this.rbtSelectedTypesCheckedChanged);
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.SetColumnSpan(this.rbtAllTypes, 2);
            this.tableLayoutPanel11.Controls.Add(this.rbtAllTypes, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.rbtSelectedTypes, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.pnlTypes, 1, 1);
            this.rgrTypes.Text = "Select Types";
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.Controls.Add(this.rgrTypes, 0, 0);
            this.tpgTypes.Text = "Types";
            this.tpgTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabReportSettings
            //
            this.tabReportSettings.Name = "tabReportSettings";
            this.tabReportSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReportSettings.Controls.Add(this.tpgGeneralSettings);
            this.tabReportSettings.Controls.Add(this.tpgAdditionalSettings);
            this.tabReportSettings.Controls.Add(this.tpgFields);
            this.tabReportSettings.Controls.Add(this.tpgTypes);
            this.tabReportSettings.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            //
            // tbbGenerateReport
            //
            this.tbbGenerateReport.Name = "tbbGenerateReport";
            this.tbbGenerateReport.AutoSize = true;
            this.tbbGenerateReport.Click += new System.EventHandler(this.actGenerateReport);
            this.tbbGenerateReport.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbGenerateReport.Glyph"));
            this.tbbGenerateReport.ToolTipText = "Generate the report";
            this.tbbGenerateReport.Text = "&Generate";
            //
            // tbbSaveSettings
            //
            this.tbbSaveSettings.Name = "tbbSaveSettings";
            this.tbbSaveSettings.AutoSize = true;
            this.tbbSaveSettings.Click += new System.EventHandler(this.actSaveSettings);
            this.tbbSaveSettings.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSaveSettings.Glyph"));
            this.tbbSaveSettings.Text = "&Save Settings";
            //
            // tbbSaveSettingsAs
            //
            this.tbbSaveSettingsAs.Name = "tbbSaveSettingsAs";
            this.tbbSaveSettingsAs.AutoSize = true;
            this.tbbSaveSettingsAs.Click += new System.EventHandler(this.actSaveSettingsAs);
            this.tbbSaveSettingsAs.Image = ((System.Drawing.Bitmap)resources.GetObject("tbbSaveSettingsAs.Glyph"));
            this.tbbSaveSettingsAs.Text = "Save Settings &As...";
            //
            // tbbLoadSettingsDialog
            //
            this.tbbLoadSettingsDialog.Name = "tbbLoadSettingsDialog";
            this.tbbLoadSettingsDialog.AutoSize = true;
            this.tbbLoadSettingsDialog.Click += new System.EventHandler(this.actLoadSettingsDialog);
            this.tbbLoadSettingsDialog.Text = "&Open...";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbGenerateReport,
                        tbbSaveSettings,
                        tbbSaveSettingsAs,
                        tbbLoadSettingsDialog});
            //
            // mniLoadSettingsDialog
            //
            this.mniLoadSettingsDialog.Name = "mniLoadSettingsDialog";
            this.mniLoadSettingsDialog.AutoSize = true;
            this.mniLoadSettingsDialog.Click += new System.EventHandler(this.actLoadSettingsDialog);
            this.mniLoadSettingsDialog.Text = "&Open...";
            //
            // mniSeparator0
            //
            this.mniSeparator0.Name = "mniSeparator0";
            this.mniSeparator0.AutoSize = true;
            this.mniSeparator0.Text = "-";
            //
            // mniLoadSettings1
            //
            this.mniLoadSettings1.Name = "mniLoadSettings1";
            this.mniLoadSettings1.AutoSize = true;
            this.mniLoadSettings1.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings1.Text = "RecentSettings";
            //
            // mniLoadSettings2
            //
            this.mniLoadSettings2.Name = "mniLoadSettings2";
            this.mniLoadSettings2.AutoSize = true;
            this.mniLoadSettings2.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings2.Text = "RecentSettings";
            //
            // mniLoadSettings3
            //
            this.mniLoadSettings3.Name = "mniLoadSettings3";
            this.mniLoadSettings3.AutoSize = true;
            this.mniLoadSettings3.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings3.Text = "RecentSettings";
            //
            // mniLoadSettings4
            //
            this.mniLoadSettings4.Name = "mniLoadSettings4";
            this.mniLoadSettings4.AutoSize = true;
            this.mniLoadSettings4.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings4.Text = "RecentSettings";
            //
            // mniLoadSettings5
            //
            this.mniLoadSettings5.Name = "mniLoadSettings5";
            this.mniLoadSettings5.AutoSize = true;
            this.mniLoadSettings5.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings5.Text = "RecentSettings";
            //
            // mniLoadSettings
            //
            this.mniLoadSettings.Name = "mniLoadSettings";
            this.mniLoadSettings.AutoSize = true;
            this.mniLoadSettings.Click += new System.EventHandler(this.actLoadSettings);
            this.mniLoadSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           mniLoadSettingsDialog,
                        mniSeparator0,
                        mniLoadSettings1,
                        mniLoadSettings2,
                        mniLoadSettings3,
                        mniLoadSettings4,
                        mniLoadSettings5});
            this.mniLoadSettings.Text = "&Load Settings";
            //
            // mniSaveSettings
            //
            this.mniSaveSettings.Name = "mniSaveSettings";
            this.mniSaveSettings.AutoSize = true;
            this.mniSaveSettings.Click += new System.EventHandler(this.actSaveSettings);
            this.mniSaveSettings.Image = ((System.Drawing.Bitmap)resources.GetObject("mniSaveSettings.Glyph"));
            this.mniSaveSettings.Text = "&Save Settings";
            //
            // mniSaveSettingsAs
            //
            this.mniSaveSettingsAs.Name = "mniSaveSettingsAs";
            this.mniSaveSettingsAs.AutoSize = true;
            this.mniSaveSettingsAs.Click += new System.EventHandler(this.actSaveSettingsAs);
            this.mniSaveSettingsAs.Image = ((System.Drawing.Bitmap)resources.GetObject("mniSaveSettingsAs.Glyph"));
            this.mniSaveSettingsAs.Text = "Save Settings &As...";
            //
            // mniMaintainSettings
            //
            this.mniMaintainSettings.Name = "mniMaintainSettings";
            this.mniMaintainSettings.AutoSize = true;
            this.mniMaintainSettings.Click += new System.EventHandler(this.actMaintainSettings);
            this.mniMaintainSettings.Text = "&Maintain Settings...";
            //
            // mniSeparator1
            //
            this.mniSeparator1.Name = "mniSeparator1";
            this.mniSeparator1.AutoSize = true;
            this.mniSeparator1.Text = "-";
            //
            // mniWrapColumn
            //
            this.mniWrapColumn.Name = "mniWrapColumn";
            this.mniWrapColumn.AutoSize = true;
            this.mniWrapColumn.Click += new System.EventHandler(this.actWrapColumn);
            this.mniWrapColumn.Text = "&Wrap Columns";
            //
            // mniSeparator2
            //
            this.mniSeparator2.Name = "mniSeparator2";
            this.mniSeparator2.AutoSize = true;
            this.mniSeparator2.Text = "-";
            //
            // mniGenerateReport
            //
            this.mniGenerateReport.Name = "mniGenerateReport";
            this.mniGenerateReport.AutoSize = true;
            this.mniGenerateReport.Click += new System.EventHandler(this.actGenerateReport);
            this.mniGenerateReport.Image = ((System.Drawing.Bitmap)resources.GetObject("mniGenerateReport.Glyph"));
            this.mniGenerateReport.ToolTipText = "Generate the report";
            this.mniGenerateReport.Text = "&Generate";
            //
            // mniSeparator3
            //
            this.mniSeparator3.Name = "mniSeparator3";
            this.mniSeparator3.AutoSize = true;
            this.mniSeparator3.Text = "-";
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
                           mniLoadSettings,
                        mniSaveSettings,
                        mniSaveSettingsAs,
                        mniMaintainSettings,
                        mniSeparator1,
                        mniWrapColumn,
                        mniSeparator2,
                        mniGenerateReport,
                        mniSeparator3,
                        mniClose});
            this.mniFile.Text = "&File";
            //
            // mniHelpPetraHelp
            //
            this.mniHelpPetraHelp.Name = "mniHelpPetraHelp";
            this.mniHelpPetraHelp.AutoSize = true;
            this.mniHelpPetraHelp.Text = "&Petra Help";
            //
            // mniSeparator4
            //
            this.mniSeparator4.Name = "mniSeparator4";
            this.mniSeparator4.AutoSize = true;
            this.mniSeparator4.Text = "-";
            //
            // mniHelpBugReport
            //
            this.mniHelpBugReport.Name = "mniHelpBugReport";
            this.mniHelpBugReport.AutoSize = true;
            this.mniHelpBugReport.Text = "Bug &Report";
            //
            // mniSeparator5
            //
            this.mniSeparator5.Name = "mniSeparator5";
            this.mniSeparator5.AutoSize = true;
            this.mniSeparator5.Text = "-";
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
                        mniSeparator4,
                        mniHelpBugReport,
                        mniSeparator5,
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
                        mniHelp});
            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;

            //
            // TFrmTotalGivingForRecipients
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(680, 480);

            this.Controls.Add(this.tabReportSettings);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmTotalGivingForRecipients";
            this.Text = "Total giving for selected recipients";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.pnlTypeButtons.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.pnlTypes.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.rgrTypes.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tpgTypes.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.pnlFieldButtons.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.pnlFields.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.rgrFields.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tpgFields.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.rgrFormatCurrency.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tpgAdditionalSettings.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpCurrencySelection.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rgrDonorSelection.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpgGeneralSettings.ResumeLayout(false);
            this.tabReportSettings.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Ict.Common.Controls.TTabVersatile tabReportSettings;
        private System.Windows.Forms.TabPage tpgGeneralSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblLedger;
        private System.Windows.Forms.GroupBox rgrDonorSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtPartner;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtRecipient;
        private System.Windows.Forms.RadioButton rbtExtract;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtExtract;
        private System.Windows.Forms.RadioButton rbtAllRecipients;
        private System.Windows.Forms.GroupBox grpCurrencySelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Ict.Common.Controls.TCmbAutoComplete cmbCurrency;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.TabPage tpgAdditionalSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox rgrFormatCurrency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton rbtCurrencyComplete;
        private System.Windows.Forms.RadioButton rbtCurrencyWithoutDecimals;
        private System.Windows.Forms.RadioButton rbtCurrencyThousands;
        private System.Windows.Forms.TabPage tpgFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.GroupBox rgrFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.RadioButton rbtAllFields;
        private System.Windows.Forms.RadioButton rbtSelectedFields;
        private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private Ict.Common.Controls.TClbVersatile clbFields;
        private System.Windows.Forms.Panel pnlFieldButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Button btnSelectAllFields;
        private System.Windows.Forms.Button btnUnselectAllFields;
        private System.Windows.Forms.TabPage tpgTypes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.GroupBox rgrTypes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.RadioButton rbtAllTypes;
        private System.Windows.Forms.RadioButton rbtSelectedTypes;
        private System.Windows.Forms.Panel pnlTypes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private Ict.Common.Controls.TClbVersatile clbTypes;
        private System.Windows.Forms.Panel pnlTypeButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Button btnSelectAllTypes;
        private System.Windows.Forms.Button btnUnselectAllTypes;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbGenerateReport;
        private System.Windows.Forms.ToolStripButton tbbSaveSettings;
        private System.Windows.Forms.ToolStripButton tbbSaveSettingsAs;
        private System.Windows.Forms.ToolStripButton tbbLoadSettingsDialog;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettingsDialog;
        private System.Windows.Forms.ToolStripSeparator mniSeparator0;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings1;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings2;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings3;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings4;
        private System.Windows.Forms.ToolStripMenuItem mniLoadSettings5;
        private System.Windows.Forms.ToolStripMenuItem mniSaveSettings;
        private System.Windows.Forms.ToolStripMenuItem mniSaveSettingsAs;
        private System.Windows.Forms.ToolStripMenuItem mniMaintainSettings;
        private System.Windows.Forms.ToolStripSeparator mniSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mniWrapColumn;
        private System.Windows.Forms.ToolStripSeparator mniSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniGenerateReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniClose;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
        private System.Windows.Forms.ToolStripSeparator mniSeparator4;
        private System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
        private System.Windows.Forms.ToolStripSeparator mniSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
        private System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}
