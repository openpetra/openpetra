/* auto generated with nant generateWinforms from PartnerEdit2.yaml
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

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TFrmPartnerEdit2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmPartnerEdit2));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpCollapsible = new System.Windows.Forms.GroupBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlPartnerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPartnerKey = new System.Windows.Forms.TextBox();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblEmpty2 = new System.Windows.Forms.Label();
            this.txtPartnerClass = new System.Windows.Forms.TextBox();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.pnlFamily = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtFamilyName = new System.Windows.Forms.TextBox();
            this.lblEmpty = new System.Windows.Forms.Label();
            this.cmbAddresseeTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAddresseeTypeCode = new System.Windows.Forms.Label();
            this.chkNoSolicitations = new System.Windows.Forms.CheckBox();
            this.pnlAdditionalInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLastGift = new System.Windows.Forms.TextBox();
            this.lblLastGift = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnWorkerField = new System.Windows.Forms.Button();
            this.txtWorkerField = new System.Windows.Forms.TextBox();
            this.cmbPartnerStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPartnerStatus = new System.Windows.Forms.Label();
            this.txtStatusUpdated = new System.Windows.Forms.TextBox();
            this.lblStatusUpdated = new System.Windows.Forms.Label();
            this.txtLastContact = new System.Windows.Forms.TextBox();
            this.lblLastContact = new System.Windows.Forms.Label();
            this.tabPartners = new Ict.Common.Controls.TTabVersatile();
            this.tpgAddresses = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTest = new System.Windows.Forms.Label();
            this.tpgDetails = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.ucoPartnerDetails = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetailsPerson();
            this.tpgSubscriptions = new System.Windows.Forms.TabPage();
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
            this.grpCollapsible.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlPartnerInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlFamily.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlAdditionalInfo.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabPartners.SuspendLayout();
            this.tpgAddresses.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tpgDetails.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tpgSubscriptions.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.stbMain.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.tabPartners);
            this.pnlContent.Controls.Add(this.grpCollapsible);
            //
            // grpCollapsible
            //
            this.grpCollapsible.Name = "grpCollapsible";
            this.grpCollapsible.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCollapsible.AutoSize = true;
            this.grpCollapsible.Controls.Add(this.pnlLeft);
            this.grpCollapsible.Controls.Add(this.pnlRight);
            //
            // pnlLeft
            //
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.AutoSize = true;
            this.pnlLeft.Controls.Add(this.pnlAdditionalInfo);
            this.pnlLeft.Controls.Add(this.pnlFamily);
            this.pnlLeft.Controls.Add(this.pnlPartnerInfo);
            //
            // pnlPartnerInfo
            //
            this.pnlPartnerInfo.Name = "pnlPartnerInfo";
            this.pnlPartnerInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPartnerInfo.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlPartnerInfo.Controls.Add(this.tableLayoutPanel1);
            //
            // txtPartnerKey
            //
            this.txtPartnerKey.Location = new System.Drawing.Point(2,2);
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.Size = new System.Drawing.Size(90, 28);
            this.txtPartnerKey.ReadOnly = true;
            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(2,2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.AutoSize = true;
            this.lblPartnerKey.Text = "Key:";
            this.lblPartnerKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // lblEmpty2
            //
            this.lblEmpty2.Location = new System.Drawing.Point(2,2);
            this.lblEmpty2.Name = "lblEmpty2";
            this.lblEmpty2.AutoSize = true;
            this.lblEmpty2.Text = "Empty2:";
            this.lblEmpty2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtPartnerClass
            //
            this.txtPartnerClass.Location = new System.Drawing.Point(2,2);
            this.txtPartnerClass.Name = "txtPartnerClass";
            this.txtPartnerClass.Size = new System.Drawing.Size(150, 28);
            this.txtPartnerClass.ReadOnly = true;
            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(2,2);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.AutoSize = true;
            this.lblPartnerClass.Text = "Class:";
            this.lblPartnerClass.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblPartnerKey, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartnerKey, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblEmpty2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPartnerClass, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartnerClass, 4, 0);
            //
            // pnlFamily
            //
            this.pnlFamily.Name = "pnlFamily";
            this.pnlFamily.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFamily.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlFamily.Controls.Add(this.tableLayoutPanel2);
            //
            // txtTitle
            //
            this.txtTitle.Location = new System.Drawing.Point(2,2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(90, 28);
            //
            // lblTitle
            //
            this.lblTitle.Location = new System.Drawing.Point(2,2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.AutoSize = true;
            this.lblTitle.Text = "Title/Na&me:";
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtFirstName
            //
            this.txtFirstName.Location = new System.Drawing.Point(2,2);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(150, 28);
            //
            // txtFamilyName
            //
            this.txtFamilyName.Location = new System.Drawing.Point(2,2);
            this.txtFamilyName.Name = "txtFamilyName";
            this.txtFamilyName.Size = new System.Drawing.Size(150, 28);
            //
            // lblEmpty
            //
            this.lblEmpty.Location = new System.Drawing.Point(2,2);
            this.lblEmpty.Name = "lblEmpty";
            this.lblEmpty.Size = new System.Drawing.Size(90, 28);
            this.lblEmpty.Text = "Empty:";
            this.lblEmpty.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbAddresseeTypeCode
            //
            this.cmbAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.cmbAddresseeTypeCode.Name = "cmbAddresseeTypeCode";
            this.cmbAddresseeTypeCode.Size = new System.Drawing.Size(105, 28);
            this.cmbAddresseeTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.AddresseeTypeList;
            //
            // lblAddresseeTypeCode
            //
            this.lblAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblAddresseeTypeCode.Name = "lblAddresseeTypeCode";
            this.lblAddresseeTypeCode.AutoSize = true;
            this.lblAddresseeTypeCode.Text = "&Addressee Type:";
            this.lblAddresseeTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // chkNoSolicitations
            //
            this.chkNoSolicitations.Location = new System.Drawing.Point(2,2);
            this.chkNoSolicitations.Name = "chkNoSolicitations";
            this.chkNoSolicitations.AutoSize = true;
            this.chkNoSolicitations.Text = "No Solicitations";
            this.chkNoSolicitations.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.lblEmpty, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblEmpty, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtTitle, 1, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.txtFirstName, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtFirstName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAddresseeTypeCode, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbAddresseeTypeCode, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtFamilyName, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkNoSolicitations, 4, 1);
            //
            // pnlAdditionalInfo
            //
            this.pnlAdditionalInfo.Name = "pnlAdditionalInfo";
            this.pnlAdditionalInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAdditionalInfo.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlAdditionalInfo.Controls.Add(this.tableLayoutPanel3);
            //
            // txtLastGift
            //
            this.txtLastGift.Location = new System.Drawing.Point(2,2);
            this.txtLastGift.Name = "txtLastGift";
            this.txtLastGift.Size = new System.Drawing.Size(420, 28);
            this.txtLastGift.ReadOnly = true;
            //
            // lblLastGift
            //
            this.lblLastGift.Location = new System.Drawing.Point(2,2);
            this.lblLastGift.Name = "lblLastGift";
            this.lblLastGift.AutoSize = true;
            this.lblLastGift.Text = "Last Gift:";
            this.lblLastGift.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblLastGift, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtLastGift, 1, 0);
            //
            // pnlRight
            //
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlRight.Controls.Add(this.tableLayoutPanel4);
            //
            // btnWorkerField
            //
            this.btnWorkerField.Location = new System.Drawing.Point(2,2);
            this.btnWorkerField.Name = "btnWorkerField";
            this.btnWorkerField.AutoSize = true;
            this.btnWorkerField.Text = "&OMer Field...";
            //
            // txtWorkerField
            //
            this.txtWorkerField.Location = new System.Drawing.Point(2,2);
            this.txtWorkerField.Name = "txtWorkerField";
            this.txtWorkerField.Size = new System.Drawing.Size(115, 28);
            this.txtWorkerField.ReadOnly = true;
            //
            // cmbPartnerStatus
            //
            this.cmbPartnerStatus.Location = new System.Drawing.Point(2,2);
            this.cmbPartnerStatus.Name = "cmbPartnerStatus";
            this.cmbPartnerStatus.Size = new System.Drawing.Size(100, 28);
            this.cmbPartnerStatus.ListTable = TCmbAutoPopulated.TListTableEnum.PartnerStatusList;
            //
            // lblPartnerStatus
            //
            this.lblPartnerStatus.Location = new System.Drawing.Point(2,2);
            this.lblPartnerStatus.Name = "lblPartnerStatus";
            this.lblPartnerStatus.AutoSize = true;
            this.lblPartnerStatus.Text = "Partner &Status:";
            this.lblPartnerStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtStatusUpdated
            //
            this.txtStatusUpdated.Location = new System.Drawing.Point(2,2);
            this.txtStatusUpdated.Name = "txtStatusUpdated";
            this.txtStatusUpdated.Size = new System.Drawing.Size(115, 28);
            this.txtStatusUpdated.ReadOnly = true;
            //
            // lblStatusUpdated
            //
            this.lblStatusUpdated.Location = new System.Drawing.Point(2,2);
            this.lblStatusUpdated.Name = "lblStatusUpdated";
            this.lblStatusUpdated.AutoSize = true;
            this.lblStatusUpdated.Text = "Status Updated:";
            this.lblStatusUpdated.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtLastContact
            //
            this.txtLastContact.Location = new System.Drawing.Point(2,2);
            this.txtLastContact.Name = "txtLastContact";
            this.txtLastContact.Size = new System.Drawing.Size(115, 28);
            this.txtLastContact.ReadOnly = true;
            //
            // lblLastContact
            //
            this.lblLastContact.Location = new System.Drawing.Point(2,2);
            this.lblLastContact.Name = "lblLastContact";
            this.lblLastContact.AutoSize = true;
            this.lblLastContact.Text = "Last Contact:";
            this.lblLastContact.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.SetColumnSpan(this.btnWorkerField, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnWorkerField, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPartnerStatus, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblStatusUpdated, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblLastContact, 0, 3);
            this.tableLayoutPanel4.SetColumnSpan(this.cmbPartnerStatus, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbPartnerStatus, 1, 1);
            this.tableLayoutPanel4.SetColumnSpan(this.txtStatusUpdated, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtStatusUpdated, 1, 2);
            this.tableLayoutPanel4.SetColumnSpan(this.txtLastContact, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtLastContact, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtWorkerField, 2, 0);
            this.grpCollapsible.Text = "Partner";
            //
            // tpgAddresses
            //
            this.tpgAddresses.Location = new System.Drawing.Point(2,2);
            this.tpgAddresses.Name = "tpgAddresses";
            this.tpgAddresses.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.tpgAddresses.Controls.Add(this.tableLayoutPanel5);
            //
            // lblTest
            //
            this.lblTest.Location = new System.Drawing.Point(2,2);
            this.lblTest.Name = "lblTest";
            this.lblTest.AutoSize = true;
            this.lblTest.Text = "Test only:";
            this.lblTest.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblTest, 0, 0);
            this.tpgAddresses.Text = "Addresses ({0})";
            this.tpgAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgDetails
            //
            this.tpgDetails.Location = new System.Drawing.Point(2,2);
            this.tpgDetails.Name = "tpgDetails";
            this.tpgDetails.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.tpgDetails.Controls.Add(this.tableLayoutPanel6);
            //
            // ucoPartnerDetails
            //
            this.ucoPartnerDetails.Location = new System.Drawing.Point(2,2);
            this.ucoPartnerDetails.Name = "ucoPartnerDetails";
            this.ucoPartnerDetails.Size = new System.Drawing.Size(650, 386);
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.ucoPartnerDetails, 0, 0);
            this.tpgDetails.Text = "Partner Details";
            this.tpgDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgSubscriptions
            //
            this.tpgSubscriptions.Location = new System.Drawing.Point(2,2);
            this.tpgSubscriptions.Name = "tpgSubscriptions";
            this.tpgSubscriptions.AutoSize = true;
            this.tpgSubscriptions.Text = "Subscriptions ({0})";
            this.tpgSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabPartners
            //
            this.tabPartners.Name = "tabPartners";
            this.tabPartners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPartners.Controls.Add(this.tpgAddresses);
            this.tabPartners.Controls.Add(this.tpgDetails);
            this.tabPartners.Controls.Add(this.tpgSubscriptions);
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
            // TFrmPartnerEdit2
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 700);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmPartnerEdit2";
            this.Text = "Partner Edit";

	        this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
	        this.Load += new System.EventHandler(this.TFrmPetra_Load);
	        this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
	        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
	        this.Closed += new System.EventHandler(this.TFrmPetra_Closed);
	
            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tpgSubscriptions.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tpgDetails.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tpgAddresses.ResumeLayout(false);
            this.tabPartners.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlAdditionalInfo.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlFamily.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlPartnerInfo.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.grpCollapsible.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpCollapsible;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlPartnerInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblEmpty2;
        private System.Windows.Forms.TextBox txtPartnerClass;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.Panel pnlFamily;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtFamilyName;
        private System.Windows.Forms.Label lblEmpty;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAddresseeTypeCode;
        private System.Windows.Forms.Label lblAddresseeTypeCode;
        private System.Windows.Forms.CheckBox chkNoSolicitations;
        private System.Windows.Forms.Panel pnlAdditionalInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtLastGift;
        private System.Windows.Forms.Label lblLastGift;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnWorkerField;
        private System.Windows.Forms.TextBox txtWorkerField;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPartnerStatus;
        private System.Windows.Forms.Label lblPartnerStatus;
        private System.Windows.Forms.TextBox txtStatusUpdated;
        private System.Windows.Forms.Label lblStatusUpdated;
        private System.Windows.Forms.TextBox txtLastContact;
        private System.Windows.Forms.Label lblLastContact;
        private Ict.Common.Controls.TTabVersatile tabPartners;
        private System.Windows.Forms.TabPage tpgAddresses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.TabPage tpgDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetailsPerson ucoPartnerDetails;
        private System.Windows.Forms.TabPage tpgSubscriptions;
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
