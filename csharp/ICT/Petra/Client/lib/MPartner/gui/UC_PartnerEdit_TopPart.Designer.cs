/* auto generated with nant generateWinforms from UC_PartnerEdit_TopPart.yaml
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
    partial class TUC_PartnerEdit_TopPart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_TopPart));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpCollapsible = new System.Windows.Forms.GroupBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlPartnerInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPartnerKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblPartnerKey = new System.Windows.Forms.Label();
            this.lblFamilyEmpty2 = new System.Windows.Forms.Label();
            this.txtPartnerClass = new System.Windows.Forms.TextBox();
            this.lblPartnerClass = new System.Windows.Forms.Label();
            this.pnlFamily = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtFamilyTitle = new System.Windows.Forms.TextBox();
            this.lblFamilyTitle = new System.Windows.Forms.Label();
            this.txtFamilyFirstName = new System.Windows.Forms.TextBox();
            this.txtFamilyFamilyName = new System.Windows.Forms.TextBox();
            this.lblFamilyEmpty = new System.Windows.Forms.Label();
            this.cmbFamilyAddresseeTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblFamilyAddresseeTypeCode = new System.Windows.Forms.Label();
            this.chkFamilyNoSolicitations = new System.Windows.Forms.CheckBox();
            this.pnlPerson = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPersonTitle = new System.Windows.Forms.TextBox();
            this.lblPersonTitle = new System.Windows.Forms.Label();
            this.txtPersonFirstName = new System.Windows.Forms.TextBox();
            this.txtPersonMiddleName = new System.Windows.Forms.TextBox();
            this.txtPersonFamilyName = new System.Windows.Forms.TextBox();
            this.pnlPerson2ndLine = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbPersonGender = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPersonGender = new System.Windows.Forms.Label();
            this.cmbPersonAddresseeTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPersonAddresseeTypeCode = new System.Windows.Forms.Label();
            this.chkPersonNoSolicitations = new System.Windows.Forms.CheckBox();
            this.pnlChurch = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txtChurchName = new System.Windows.Forms.TextBox();
            this.lblChurchName = new System.Windows.Forms.Label();
            this.pnlOrganisation = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtOrganisationName = new System.Windows.Forms.TextBox();
            this.lblOrganisationName = new System.Windows.Forms.Label();
            this.pnlUnit = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtUnitName = new System.Windows.Forms.TextBox();
            this.lblUnitName = new System.Windows.Forms.Label();
            this.pnlBank = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.txtBankName = new System.Windows.Forms.TextBox();
            this.lblBankName = new System.Windows.Forms.Label();
            this.pnlVenue = new System.Windows.Forms.Panel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.txtVenueName = new System.Windows.Forms.TextBox();
            this.lblVenueName = new System.Windows.Forms.Label();
            this.pnlOther = new System.Windows.Forms.Panel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.lblOtherEmpty = new System.Windows.Forms.Label();
            this.cmbOtherAddresseeTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblOtherAddresseeTypeCode = new System.Windows.Forms.Label();
            this.chkOtherNoSolicitations = new System.Windows.Forms.CheckBox();
            this.pnlAdditionalInfo = new System.Windows.Forms.Panel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLastGiftDetailsDate = new System.Windows.Forms.TextBox();
            this.lblLastGiftDetailsDate = new System.Windows.Forms.Label();
            this.txtLastGiftDetails = new System.Windows.Forms.TextBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlWorkerField = new System.Windows.Forms.Panel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.btnWorkerField = new System.Windows.Forms.Button();
            this.txtWorkerField = new System.Windows.Forms.TextBox();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.lblEmptySpacer = new System.Windows.Forms.Label();
            this.cmbPartnerStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblPartnerStatus = new System.Windows.Forms.Label();
            this.txtStatusUpdated = new System.Windows.Forms.TextBox();
            this.lblStatusUpdated = new System.Windows.Forms.Label();
            this.txtLastContact = new System.Windows.Forms.TextBox();
            this.lblLastContact = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.grpCollapsible.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlPartnerInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlFamily.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlPerson.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlPerson2ndLine.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlChurch.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlOrganisation.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.pnlUnit.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.pnlBank.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.pnlVenue.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.pnlOther.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.pnlAdditionalInfo.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.pnlWorkerField.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.pnlSpacer.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpCollapsible);
            //
            // grpCollapsible
            //
            this.grpCollapsible.Name = "grpCollapsible";
            this.grpCollapsible.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCollapsible.Padding = new System.Windows.Forms.Padding(3,0,3,4);
            this.grpCollapsible.AutoSize = true;
            this.grpCollapsible.Controls.Add(this.pnlLeft);
            this.grpCollapsible.Controls.Add(this.pnlRight);
            //
            // pnlLeft
            //
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.AutoSize = true;
            this.pnlLeft.Controls.Add(this.pnlOther);
            this.pnlLeft.Controls.Add(this.pnlAdditionalInfo);
            this.pnlLeft.Controls.Add(this.pnlVenue);
            this.pnlLeft.Controls.Add(this.pnlBank);
            this.pnlLeft.Controls.Add(this.pnlUnit);
            this.pnlLeft.Controls.Add(this.pnlOrganisation);
            this.pnlLeft.Controls.Add(this.pnlChurch);
            this.pnlLeft.Controls.Add(this.pnlPerson);
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
            this.txtPartnerKey.TabStop = false;
            this.txtPartnerKey.Size = new System.Drawing.Size(80, 28);
            this.txtPartnerKey.ReadOnly = true;
            this.txtPartnerKey.TabStop = false;
            this.txtPartnerKey.ShowLabel = false;
            this.txtPartnerKey.ASpecialSetting = true;
            this.txtPartnerKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPartnerKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtPartnerKey.PartnerClass = "";
            this.txtPartnerKey.MaxLength = 32767;
            this.txtPartnerKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtPartnerKey.TextBoxWidth = 80;
            this.txtPartnerKey.ButtonWidth = 0;
            this.txtPartnerKey.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerKey.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            //
            // lblPartnerKey
            //
            this.lblPartnerKey.Location = new System.Drawing.Point(2,2);
            this.lblPartnerKey.Name = "lblPartnerKey";
            this.lblPartnerKey.AutoSize = true;
            this.lblPartnerKey.Text = "Key:";
            this.lblPartnerKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPartnerKey.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // lblFamilyEmpty2
            //
            this.lblFamilyEmpty2.Location = new System.Drawing.Point(2,2);
            this.lblFamilyEmpty2.Name = "lblFamilyEmpty2";
            this.lblFamilyEmpty2.AutoSize = true;
            this.lblFamilyEmpty2.Text = "Family Empty2:";
            this.lblFamilyEmpty2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtPartnerClass
            //
            this.txtPartnerClass.Location = new System.Drawing.Point(2,2);
            this.txtPartnerClass.Name = "txtPartnerClass";
            this.txtPartnerClass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPartnerClass.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtPartnerClass.Size = new System.Drawing.Size(150, 28);
            this.txtPartnerClass.ReadOnly = true;
            this.txtPartnerClass.TabStop = false;
            //
            // lblPartnerClass
            //
            this.lblPartnerClass.Location = new System.Drawing.Point(2,2);
            this.lblPartnerClass.Name = "lblPartnerClass";
            this.lblPartnerClass.AutoSize = true;
            this.lblPartnerClass.Text = "Class:";
            this.lblPartnerClass.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPartnerClass.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 80));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 25));
            this.tableLayoutPanel1.Controls.Add(this.lblPartnerKey, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartnerKey, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFamilyEmpty2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPartnerClass, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPartnerClass, 4, 0);
            //
            // pnlFamily
            //
            this.pnlFamily.Name = "pnlFamily";
            this.pnlFamily.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFamily.Visible = false;
            this.pnlFamily.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlFamily.Controls.Add(this.tableLayoutPanel2);
            //
            // txtFamilyTitle
            //
            this.txtFamilyTitle.Location = new System.Drawing.Point(2,2);
            this.txtFamilyTitle.Name = "txtFamilyTitle";
            this.txtFamilyTitle.Size = new System.Drawing.Size(90, 28);
            //
            // lblFamilyTitle
            //
            this.lblFamilyTitle.Location = new System.Drawing.Point(2,2);
            this.lblFamilyTitle.Name = "lblFamilyTitle";
            this.lblFamilyTitle.AutoSize = true;
            this.lblFamilyTitle.Text = "Title/Na&me:";
            this.lblFamilyTitle.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFamilyTitle.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtFamilyFirstName
            //
            this.txtFamilyFirstName.Name = "txtFamilyFirstName";
            this.txtFamilyFirstName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFamilyFirstName.AutoSize = true;
            //
            // txtFamilyFamilyName
            //
            this.txtFamilyFamilyName.Location = new System.Drawing.Point(2,2);
            this.txtFamilyFamilyName.Name = "txtFamilyFamilyName";
            this.txtFamilyFamilyName.Size = new System.Drawing.Size(150, 28);
            //
            // lblFamilyEmpty
            //
            this.lblFamilyEmpty.Location = new System.Drawing.Point(2,2);
            this.lblFamilyEmpty.Name = "lblFamilyEmpty";
            this.lblFamilyEmpty.Size = new System.Drawing.Size(90, 28);
            this.lblFamilyEmpty.Text = "Family Empty:";
            this.lblFamilyEmpty.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbFamilyAddresseeTypeCode
            //
            this.cmbFamilyAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.cmbFamilyAddresseeTypeCode.Name = "cmbFamilyAddresseeTypeCode";
            this.cmbFamilyAddresseeTypeCode.Size = new System.Drawing.Size(105, 28);
            this.cmbFamilyAddresseeTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.AddresseeTypeList;
            //
            // lblFamilyAddresseeTypeCode
            //
            this.lblFamilyAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblFamilyAddresseeTypeCode.Name = "lblFamilyAddresseeTypeCode";
            this.lblFamilyAddresseeTypeCode.AutoSize = true;
            this.lblFamilyAddresseeTypeCode.Text = "&Addressee Type:";
            this.lblFamilyAddresseeTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblFamilyAddresseeTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // chkFamilyNoSolicitations
            //
            this.chkFamilyNoSolicitations.Location = new System.Drawing.Point(2,2);
            this.chkFamilyNoSolicitations.Name = "chkFamilyNoSolicitations";
            this.chkFamilyNoSolicitations.AutoSize = true;
            this.chkFamilyNoSolicitations.CheckedChanged += new System.EventHandler(this.UpdateNoSolicitationsColouring);
            this.chkFamilyNoSolicitations.Text = "No Solicitations";
            this.chkFamilyNoSolicitations.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblFamilyTitle, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.lblFamilyEmpty, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblFamilyEmpty, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtFamilyTitle, 1, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.txtFamilyFirstName, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtFamilyFirstName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblFamilyAddresseeTypeCode, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbFamilyAddresseeTypeCode, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtFamilyFamilyName, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkFamilyNoSolicitations, 4, 1);
            //
            // pnlPerson
            //
            this.pnlPerson.Name = "pnlPerson";
            this.pnlPerson.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPerson.Visible = false;
            this.pnlPerson.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlPerson.Controls.Add(this.tableLayoutPanel3);
            //
            // txtPersonTitle
            //
            this.txtPersonTitle.Location = new System.Drawing.Point(2,2);
            this.txtPersonTitle.Name = "txtPersonTitle";
            this.txtPersonTitle.Size = new System.Drawing.Size(88, 28);
            //
            // lblPersonTitle
            //
            this.lblPersonTitle.Location = new System.Drawing.Point(2,2);
            this.lblPersonTitle.Name = "lblPersonTitle";
            this.lblPersonTitle.AutoSize = true;
            this.lblPersonTitle.Text = "Title/Na&me:";
            this.lblPersonTitle.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPersonTitle.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtPersonFirstName
            //
            this.txtPersonFirstName.Name = "txtPersonFirstName";
            this.txtPersonFirstName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPersonFirstName.Size = new System.Drawing.Size(104, 28);
            //
            // txtPersonMiddleName
            //
            this.txtPersonMiddleName.Location = new System.Drawing.Point(2,2);
            this.txtPersonMiddleName.Name = "txtPersonMiddleName";
            this.txtPersonMiddleName.Size = new System.Drawing.Size(30, 28);
            //
            // txtPersonFamilyName
            //
            this.txtPersonFamilyName.Name = "txtPersonFamilyName";
            this.txtPersonFamilyName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPersonFamilyName.AutoSize = true;
            //
            // pnlPerson2ndLine
            //
            this.pnlPerson2ndLine.Name = "pnlPerson2ndLine";
            this.pnlPerson2ndLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPerson2ndLine.Margin = new System.Windows.Forms.Padding(0,0,0,0);
            this.pnlPerson2ndLine.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlPerson2ndLine.Controls.Add(this.tableLayoutPanel4);
            //
            // cmbPersonGender
            //
            this.cmbPersonGender.Location = new System.Drawing.Point(2,2);
            this.cmbPersonGender.Name = "cmbPersonGender";
            this.cmbPersonGender.Size = new System.Drawing.Size(88, 28);
            this.cmbPersonGender.ListTable = TCmbAutoPopulated.TListTableEnum.GenderList;
            //
            // lblPersonGender
            //
            this.lblPersonGender.Location = new System.Drawing.Point(2,2);
            this.lblPersonGender.Name = "lblPersonGender";
            this.lblPersonGender.AutoSize = true;
            this.lblPersonGender.Text = "&Gender:";
            this.lblPersonGender.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPersonGender.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbPersonAddresseeTypeCode
            //
            this.cmbPersonAddresseeTypeCode.Name = "cmbPersonAddresseeTypeCode";
            this.cmbPersonAddresseeTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.cmbPersonAddresseeTypeCode.Width = 105;
            this.cmbPersonAddresseeTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.AddresseeTypeList;
            //
            // lblPersonAddresseeTypeCode
            //
            this.lblPersonAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblPersonAddresseeTypeCode.Name = "lblPersonAddresseeTypeCode";
            this.lblPersonAddresseeTypeCode.AutoSize = true;
            this.lblPersonAddresseeTypeCode.Text = "&Addressee Type:";
            this.lblPersonAddresseeTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPersonAddresseeTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // chkPersonNoSolicitations
            //
            this.chkPersonNoSolicitations.Location = new System.Drawing.Point(2,2);
            this.chkPersonNoSolicitations.Name = "chkPersonNoSolicitations";
            this.chkPersonNoSolicitations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPersonNoSolicitations.AutoSize = true;
            this.chkPersonNoSolicitations.CheckedChanged += new System.EventHandler(this.UpdateNoSolicitationsColouring);
            this.chkPersonNoSolicitations.Text = "No Solicitations";
            this.chkPersonNoSolicitations.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 80));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 28));
            this.tableLayoutPanel4.Controls.Add(this.lblPersonGender, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbPersonGender, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblPersonAddresseeTypeCode, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbPersonAddresseeTypeCode, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkPersonNoSolicitations, 4, 0);
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 80));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 25));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblPersonTitle, 0, 0);
            this.tableLayoutPanel3.SetColumnSpan(this.pnlPerson2ndLine, 5);
            this.tableLayoutPanel3.Controls.Add(this.pnlPerson2ndLine, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtPersonTitle, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtPersonFirstName, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtPersonMiddleName, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtPersonFamilyName, 4, 0);
            //
            // pnlChurch
            //
            this.pnlChurch.Name = "pnlChurch";
            this.pnlChurch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlChurch.Visible = false;
            this.pnlChurch.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.pnlChurch.Controls.Add(this.tableLayoutPanel5);
            //
            // txtChurchName
            //
            this.txtChurchName.Location = new System.Drawing.Point(2,2);
            this.txtChurchName.Name = "txtChurchName";
            this.txtChurchName.Size = new System.Drawing.Size(428, 28);
            //
            // lblChurchName
            //
            this.lblChurchName.Location = new System.Drawing.Point(2,2);
            this.lblChurchName.Name = "lblChurchName";
            this.lblChurchName.AutoSize = true;
            this.lblChurchName.Text = "Title/Na&me:";
            this.lblChurchName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblChurchName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblChurchName, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtChurchName, 1, 0);
            //
            // pnlOrganisation
            //
            this.pnlOrganisation.Name = "pnlOrganisation";
            this.pnlOrganisation.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOrganisation.Visible = false;
            this.pnlOrganisation.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.pnlOrganisation.Controls.Add(this.tableLayoutPanel6);
            //
            // txtOrganisationName
            //
            this.txtOrganisationName.Location = new System.Drawing.Point(2,2);
            this.txtOrganisationName.Name = "txtOrganisationName";
            this.txtOrganisationName.Size = new System.Drawing.Size(428, 28);
            //
            // lblOrganisationName
            //
            this.lblOrganisationName.Location = new System.Drawing.Point(2,2);
            this.lblOrganisationName.Name = "lblOrganisationName";
            this.lblOrganisationName.AutoSize = true;
            this.lblOrganisationName.Text = "Title/Na&me:";
            this.lblOrganisationName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblOrganisationName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.lblOrganisationName, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.txtOrganisationName, 1, 0);
            //
            // pnlUnit
            //
            this.pnlUnit.Name = "pnlUnit";
            this.pnlUnit.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUnit.Visible = false;
            this.pnlUnit.AutoSize = true;
            //
            // tableLayoutPanel7
            //
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.AutoSize = true;
            this.pnlUnit.Controls.Add(this.tableLayoutPanel7);
            //
            // txtUnitName
            //
            this.txtUnitName.Location = new System.Drawing.Point(2,2);
            this.txtUnitName.Name = "txtUnitName";
            this.txtUnitName.Size = new System.Drawing.Size(428, 28);
            //
            // lblUnitName
            //
            this.lblUnitName.Location = new System.Drawing.Point(2,2);
            this.lblUnitName.Name = "lblUnitName";
            this.lblUnitName.AutoSize = true;
            this.lblUnitName.Text = "Title/Na&me:";
            this.lblUnitName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblUnitName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel7.Controls.Add(this.lblUnitName, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtUnitName, 1, 0);
            //
            // pnlBank
            //
            this.pnlBank.Name = "pnlBank";
            this.pnlBank.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBank.Visible = false;
            this.pnlBank.AutoSize = true;
            //
            // tableLayoutPanel8
            //
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.AutoSize = true;
            this.pnlBank.Controls.Add(this.tableLayoutPanel8);
            //
            // txtBankName
            //
            this.txtBankName.Location = new System.Drawing.Point(2,2);
            this.txtBankName.Name = "txtBankName";
            this.txtBankName.Size = new System.Drawing.Size(428, 28);
            //
            // lblBankName
            //
            this.lblBankName.Location = new System.Drawing.Point(2,2);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.AutoSize = true;
            this.lblBankName.Text = "Title/Na&me:";
            this.lblBankName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBankName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Controls.Add(this.lblBankName, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.txtBankName, 1, 0);
            //
            // pnlVenue
            //
            this.pnlVenue.Name = "pnlVenue";
            this.pnlVenue.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlVenue.Visible = false;
            this.pnlVenue.AutoSize = true;
            //
            // tableLayoutPanel9
            //
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.AutoSize = true;
            this.pnlVenue.Controls.Add(this.tableLayoutPanel9);
            //
            // txtVenueName
            //
            this.txtVenueName.Location = new System.Drawing.Point(2,2);
            this.txtVenueName.Name = "txtVenueName";
            this.txtVenueName.Size = new System.Drawing.Size(428, 28);
            //
            // lblVenueName
            //
            this.lblVenueName.Location = new System.Drawing.Point(2,2);
            this.lblVenueName.Name = "lblVenueName";
            this.lblVenueName.AutoSize = true;
            this.lblVenueName.Text = "Title/Na&me:";
            this.lblVenueName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblVenueName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Controls.Add(this.lblVenueName, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.txtVenueName, 1, 0);
            //
            // pnlOther
            //
            this.pnlOther.Name = "pnlOther";
            this.pnlOther.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOther.Visible = false;
            this.pnlOther.AutoSize = true;
            //
            // tableLayoutPanel10
            //
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.AutoSize = true;
            this.pnlOther.Controls.Add(this.tableLayoutPanel10);
            //
            // lblOtherEmpty
            //
            this.lblOtherEmpty.Location = new System.Drawing.Point(2,2);
            this.lblOtherEmpty.Name = "lblOtherEmpty";
            this.lblOtherEmpty.Size = new System.Drawing.Size(90, 28);
            this.lblOtherEmpty.Text = "Other Empty:";
            this.lblOtherEmpty.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // cmbOtherAddresseeTypeCode
            //
            this.cmbOtherAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.cmbOtherAddresseeTypeCode.Name = "cmbOtherAddresseeTypeCode";
            this.cmbOtherAddresseeTypeCode.Size = new System.Drawing.Size(105, 28);
            this.cmbOtherAddresseeTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.AddresseeTypeList;
            //
            // lblOtherAddresseeTypeCode
            //
            this.lblOtherAddresseeTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblOtherAddresseeTypeCode.Name = "lblOtherAddresseeTypeCode";
            this.lblOtherAddresseeTypeCode.AutoSize = true;
            this.lblOtherAddresseeTypeCode.Text = "&Addressee Type:";
            this.lblOtherAddresseeTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblOtherAddresseeTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // chkOtherNoSolicitations
            //
            this.chkOtherNoSolicitations.Location = new System.Drawing.Point(2,2);
            this.chkOtherNoSolicitations.Name = "chkOtherNoSolicitations";
            this.chkOtherNoSolicitations.AutoSize = true;
            this.chkOtherNoSolicitations.CheckedChanged += new System.EventHandler(this.UpdateNoSolicitationsColouring);
            this.chkOtherNoSolicitations.Text = "No Solicitations";
            this.chkOtherNoSolicitations.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel10.ColumnCount = 4;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.Controls.Add(this.lblOtherEmpty, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.lblOtherAddresseeTypeCode, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.cmbOtherAddresseeTypeCode, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.chkOtherNoSolicitations, 3, 0);
            //
            // pnlAdditionalInfo
            //
            this.pnlAdditionalInfo.Name = "pnlAdditionalInfo";
            this.pnlAdditionalInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAdditionalInfo.AutoSize = true;
            //
            // tableLayoutPanel11
            //
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.AutoSize = true;
            this.pnlAdditionalInfo.Controls.Add(this.tableLayoutPanel11);
            //
            // txtLastGiftDetailsDate
            //
            this.txtLastGiftDetailsDate.Location = new System.Drawing.Point(2,2);
            this.txtLastGiftDetailsDate.Name = "txtLastGiftDetailsDate";
            this.txtLastGiftDetailsDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastGiftDetailsDate.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtLastGiftDetailsDate.Size = new System.Drawing.Size(88, 28);
            this.txtLastGiftDetailsDate.ReadOnly = true;
            this.txtLastGiftDetailsDate.TabStop = false;
            //
            // lblLastGiftDetailsDate
            //
            this.lblLastGiftDetailsDate.Location = new System.Drawing.Point(2,2);
            this.lblLastGiftDetailsDate.Name = "lblLastGiftDetailsDate";
            this.lblLastGiftDetailsDate.AutoSize = true;
            this.lblLastGiftDetailsDate.Text = "Last Gift:";
            this.lblLastGiftDetailsDate.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLastGiftDetailsDate.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtLastGiftDetails
            //
            this.txtLastGiftDetails.Name = "txtLastGiftDetails";
            this.txtLastGiftDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLastGiftDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastGiftDetails.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtLastGiftDetails.Size = new System.Drawing.Size(328, 28);
            this.txtLastGiftDetails.ReadOnly = true;
            this.txtLastGiftDetails.TabStop = false;
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 80));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 20));
            this.tableLayoutPanel11.Controls.Add(this.lblLastGiftDetailsDate, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.txtLastGiftDetailsDate, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.txtLastGiftDetails, 2, 0);
            //
            // pnlRight
            //
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.AutoSize = true;
            //
            // tableLayoutPanel12
            //
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.AutoSize = true;
            this.pnlRight.Controls.Add(this.tableLayoutPanel12);
            //
            // pnlWorkerField
            //
            this.pnlWorkerField.Name = "pnlWorkerField";
            this.pnlWorkerField.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWorkerField.Visible = false;
            this.pnlWorkerField.Padding = new System.Windows.Forms.Padding(0,0,5,0);
            this.pnlWorkerField.Margin = new System.Windows.Forms.Padding(0,0,0,0);
            this.pnlWorkerField.AutoSize = true;
            //
            // tableLayoutPanel13
            //
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.AutoSize = true;
            this.pnlWorkerField.Controls.Add(this.tableLayoutPanel13);
            //
            // btnWorkerField
            //
            this.btnWorkerField.Location = new System.Drawing.Point(2,2);
            this.btnWorkerField.Name = "btnWorkerField";
            this.btnWorkerField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWorkerField.Size = new System.Drawing.Size(100, 28);
            this.btnWorkerField.Click += new System.EventHandler(this.MaintainWorkerField);
            this.btnWorkerField.Image = ((System.Drawing.Bitmap)resources.GetObject("btnWorkerField.Glyph"));
            this.btnWorkerField.Text = "&Worker Field...";
            //
            // txtWorkerField
            //
            this.txtWorkerField.Name = "txtWorkerField";
            this.txtWorkerField.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWorkerField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWorkerField.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtWorkerField.AutoSize = true;
            this.txtWorkerField.ReadOnly = true;
            this.txtWorkerField.TabStop = false;
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 27));
            this.tableLayoutPanel13.Controls.Add(this.btnWorkerField, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.txtWorkerField, 1, 0);
            //
            // pnlSpacer
            //
            this.pnlSpacer.Location = new System.Drawing.Point(2,2);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.AutoSize = true;
            //
            // tableLayoutPanel14
            //
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.AutoSize = true;
            this.pnlSpacer.Controls.Add(this.tableLayoutPanel14);
            //
            // lblEmptySpacer
            //
            this.lblEmptySpacer.Location = new System.Drawing.Point(2,2);
            this.lblEmptySpacer.Name = "lblEmptySpacer";
            this.lblEmptySpacer.AutoSize = true;
            this.lblEmptySpacer.Text = "Empty Spacer:";
            this.lblEmptySpacer.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel14.Controls.Add(this.lblEmptySpacer, 0, 0);
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
            this.lblPartnerStatus.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtStatusUpdated
            //
            this.txtStatusUpdated.Location = new System.Drawing.Point(2,2);
            this.txtStatusUpdated.Name = "txtStatusUpdated";
            this.txtStatusUpdated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatusUpdated.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtStatusUpdated.Size = new System.Drawing.Size(100, 28);
            this.txtStatusUpdated.ReadOnly = true;
            this.txtStatusUpdated.TabStop = false;
            //
            // lblStatusUpdated
            //
            this.lblStatusUpdated.Location = new System.Drawing.Point(2,2);
            this.lblStatusUpdated.Name = "lblStatusUpdated";
            this.lblStatusUpdated.AutoSize = true;
            this.lblStatusUpdated.Text = "Status Updated:";
            this.lblStatusUpdated.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblStatusUpdated.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // txtLastContact
            //
            this.txtLastContact.Location = new System.Drawing.Point(2,2);
            this.txtLastContact.Name = "txtLastContact";
            this.txtLastContact.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastContact.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.txtLastContact.Size = new System.Drawing.Size(100, 28);
            this.txtLastContact.ReadOnly = true;
            this.txtLastContact.TabStop = false;
            //
            // lblLastContact
            //
            this.lblLastContact.Location = new System.Drawing.Point(2,2);
            this.lblLastContact.Name = "lblLastContact";
            this.lblLastContact.AutoSize = true;
            this.lblLastContact.Text = "Last Contact:";
            this.lblLastContact.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLastContact.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.RowCount = 5;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 28));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, 24));
            this.tableLayoutPanel12.SetColumnSpan(this.pnlWorkerField, 2);
            this.tableLayoutPanel12.Controls.Add(this.pnlWorkerField, 0, 0);
            this.tableLayoutPanel12.SetColumnSpan(this.pnlSpacer, 2);
            this.tableLayoutPanel12.Controls.Add(this.pnlSpacer, 0, 1);
            this.tableLayoutPanel12.Controls.Add(this.lblPartnerStatus, 0, 2);
            this.tableLayoutPanel12.Controls.Add(this.lblStatusUpdated, 0, 3);
            this.tableLayoutPanel12.Controls.Add(this.lblLastContact, 0, 4);
            this.tableLayoutPanel12.Controls.Add(this.cmbPartnerStatus, 1, 2);
            this.tableLayoutPanel12.Controls.Add(this.txtStatusUpdated, 1, 3);
            this.tableLayoutPanel12.Controls.Add(this.txtLastContact, 1, 4);
            this.grpCollapsible.Text = "Key Partner Data";

            //
            // TUC_PartnerEdit_TopPart
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Name = "TUC_PartnerEdit_TopPart";
            this.Text = "";

	
            this.tableLayoutPanel14.ResumeLayout(false);
            this.pnlSpacer.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.pnlWorkerField.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.pnlAdditionalInfo.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.pnlOther.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.pnlVenue.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.pnlBank.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.pnlUnit.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlOrganisation.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlChurch.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlPerson2ndLine.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlPerson.ResumeLayout(false);
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
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtPartnerKey;
        private System.Windows.Forms.Label lblPartnerKey;
        private System.Windows.Forms.Label lblFamilyEmpty2;
        private System.Windows.Forms.TextBox txtPartnerClass;
        private System.Windows.Forms.Label lblPartnerClass;
        private System.Windows.Forms.Panel pnlFamily;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtFamilyTitle;
        private System.Windows.Forms.Label lblFamilyTitle;
        private System.Windows.Forms.TextBox txtFamilyFirstName;
        private System.Windows.Forms.TextBox txtFamilyFamilyName;
        private System.Windows.Forms.Label lblFamilyEmpty;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbFamilyAddresseeTypeCode;
        private System.Windows.Forms.Label lblFamilyAddresseeTypeCode;
        private System.Windows.Forms.CheckBox chkFamilyNoSolicitations;
        private System.Windows.Forms.Panel pnlPerson;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtPersonTitle;
        private System.Windows.Forms.Label lblPersonTitle;
        private System.Windows.Forms.TextBox txtPersonFirstName;
        private System.Windows.Forms.TextBox txtPersonMiddleName;
        private System.Windows.Forms.TextBox txtPersonFamilyName;
        private System.Windows.Forms.Panel pnlPerson2ndLine;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPersonGender;
        private System.Windows.Forms.Label lblPersonGender;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPersonAddresseeTypeCode;
        private System.Windows.Forms.Label lblPersonAddresseeTypeCode;
        private System.Windows.Forms.CheckBox chkPersonNoSolicitations;
        private System.Windows.Forms.Panel pnlChurch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox txtChurchName;
        private System.Windows.Forms.Label lblChurchName;
        private System.Windows.Forms.Panel pnlOrganisation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TextBox txtOrganisationName;
        private System.Windows.Forms.Label lblOrganisationName;
        private System.Windows.Forms.Panel pnlUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.TextBox txtUnitName;
        private System.Windows.Forms.Label lblUnitName;
        private System.Windows.Forms.Panel pnlBank;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TextBox txtBankName;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.Panel pnlVenue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TextBox txtVenueName;
        private System.Windows.Forms.Label lblVenueName;
        private System.Windows.Forms.Panel pnlOther;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label lblOtherEmpty;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbOtherAddresseeTypeCode;
        private System.Windows.Forms.Label lblOtherAddresseeTypeCode;
        private System.Windows.Forms.CheckBox chkOtherNoSolicitations;
        private System.Windows.Forms.Panel pnlAdditionalInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.TextBox txtLastGiftDetailsDate;
        private System.Windows.Forms.Label lblLastGiftDetailsDate;
        private System.Windows.Forms.TextBox txtLastGiftDetails;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Panel pnlWorkerField;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Button btnWorkerField;
        private System.Windows.Forms.TextBox txtWorkerField;
        private System.Windows.Forms.Panel pnlSpacer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label lblEmptySpacer;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbPartnerStatus;
        private System.Windows.Forms.Label lblPartnerStatus;
        private System.Windows.Forms.TextBox txtStatusUpdated;
        private System.Windows.Forms.Label lblStatusUpdated;
        private System.Windows.Forms.TextBox txtLastContact;
        private System.Windows.Forms.Label lblLastContact;
    }
}
