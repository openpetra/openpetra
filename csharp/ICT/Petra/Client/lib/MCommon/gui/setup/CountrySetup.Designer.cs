// auto generated with nant generateWinforms from CountrySetup.yaml
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

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    partial class TFrmCountrySetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmCountrySetup));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailCountryCode = new System.Windows.Forms.TextBox();
            this.lblDetailCountryCode = new System.Windows.Forms.Label();
            this.txtDetailCountryName = new System.Windows.Forms.TextBox();
            this.lblDetailCountryName = new System.Windows.Forms.Label();
            this.txtDetailCountryNameLocal = new System.Windows.Forms.TextBox();
            this.lblDetailCountryNameLocal = new System.Windows.Forms.Label();
            this.pnlInternat = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chkDetailUndercover = new System.Windows.Forms.CheckBox();
            this.lblDetailUndercover = new System.Windows.Forms.Label();
            this.txtDetailInternatAccessCode = new System.Windows.Forms.TextBox();
            this.lblDetailInternatAccessCode = new System.Windows.Forms.Label();
            this.txtDetailInternatTelephoneCode = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailInternatTelephoneCode = new System.Windows.Forms.Label();
            this.cmbDetailAddressOrder = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailAddressOrder = new System.Windows.Forms.Label();
            this.cmbDetailInternatPostalTypeCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailInternatPostalTypeCode = new System.Windows.Forms.Label();
            this.pnlTimeZone = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailTimeZoneMinimum = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailTimeZoneMinimum = new System.Windows.Forms.Label();
            this.txtDetailTimeZoneMaximum = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailTimeZoneMaximum = new System.Windows.Forms.Label();
            this.chkDetailDeletable = new System.Windows.Forms.CheckBox();
            this.lblDetailDeletable = new System.Windows.Forms.Label();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbbSave = new System.Windows.Forms.ToolStripButton();
            this.tbbNew = new System.Windows.Forms.ToolStripButton();
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
            this.pnlGrid.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlInternat.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlTimeZone.SuspendLayout();
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
            this.pnlContent.Controls.Add(this.pnlGrid);
            this.pnlContent.Controls.Add(this.pnlDetails);
            //
            // pnlGrid
            //
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.AutoSize = true;
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
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlButtons.Controls.Add(this.tableLayoutPanel1);
            //
            // btnNew
            //
            this.btnNew.Location = new System.Drawing.Point(2,2);
            this.btnNew.Name = "btnNew";
            this.btnNew.AutoSize = true;
            this.btnNew.Click += new System.EventHandler(this.NewRecord);
            this.btnNew.Text = "New";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnNew, 0, 0);
            //
            // pnlDetails
            //
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetails.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlDetails.Controls.Add(this.tableLayoutPanel2);
            //
            // txtDetailCountryCode
            //
            this.txtDetailCountryCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailCountryCode.Name = "txtDetailCountryCode";
            this.txtDetailCountryCode.Size = new System.Drawing.Size(100, 28);
            this.txtDetailCountryCode.CharacterCasing = CharacterCasing.Upper;
            //
            // lblDetailCountryCode
            //
            this.lblDetailCountryCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailCountryCode.Name = "lblDetailCountryCode";
            this.lblDetailCountryCode.AutoSize = true;
            this.lblDetailCountryCode.Text = "Country Code:";
            this.lblDetailCountryCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCountryCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCountryCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailCountryName
            //
            this.txtDetailCountryName.Location = new System.Drawing.Point(2,2);
            this.txtDetailCountryName.Name = "txtDetailCountryName";
            this.txtDetailCountryName.Size = new System.Drawing.Size(247, 28);
            this.txtDetailCountryName.Leave += new System.EventHandler(this.UpdateCountryNameLocal);
            //
            // lblDetailCountryName
            //
            this.lblDetailCountryName.Location = new System.Drawing.Point(2,2);
            this.lblDetailCountryName.Name = "lblDetailCountryName";
            this.lblDetailCountryName.AutoSize = true;
            this.lblDetailCountryName.Text = "Country Name:";
            this.lblDetailCountryName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCountryName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCountryName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailCountryNameLocal
            //
            this.txtDetailCountryNameLocal.Location = new System.Drawing.Point(2,2);
            this.txtDetailCountryNameLocal.Name = "txtDetailCountryNameLocal";
            this.txtDetailCountryNameLocal.Size = new System.Drawing.Size(247, 28);
            //
            // lblDetailCountryNameLocal
            //
            this.lblDetailCountryNameLocal.Location = new System.Drawing.Point(2,2);
            this.lblDetailCountryNameLocal.Name = "lblDetailCountryNameLocal";
            this.lblDetailCountryNameLocal.AutoSize = true;
            this.lblDetailCountryNameLocal.Text = "Country Name Local:";
            this.lblDetailCountryNameLocal.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailCountryNameLocal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailCountryNameLocal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlInternat
            //
            this.pnlInternat.Location = new System.Drawing.Point(2,2);
            this.pnlInternat.Name = "pnlInternat";
            this.pnlInternat.Margin = new System.Windows.Forms.Padding(63,0,0,5);
            this.pnlInternat.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlInternat.Controls.Add(this.tableLayoutPanel3);
            //
            // chkDetailUndercover
            //
            this.chkDetailUndercover.Location = new System.Drawing.Point(2,2);
            this.chkDetailUndercover.Name = "chkDetailUndercover";
            this.chkDetailUndercover.Size = new System.Drawing.Size(30, 28);
            this.chkDetailUndercover.Text = "";
            this.chkDetailUndercover.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailUndercover
            //
            this.lblDetailUndercover.Location = new System.Drawing.Point(2,2);
            this.lblDetailUndercover.Name = "lblDetailUndercover";
            this.lblDetailUndercover.AutoSize = true;
            this.lblDetailUndercover.Text = "Undercover:";
            this.lblDetailUndercover.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailUndercover.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailUndercover.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailInternatAccessCode
            //
            this.txtDetailInternatAccessCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailInternatAccessCode.Name = "txtDetailInternatAccessCode";
            this.txtDetailInternatAccessCode.Size = new System.Drawing.Size(30, 28);
            //
            // lblDetailInternatAccessCode
            //
            this.lblDetailInternatAccessCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailInternatAccessCode.Name = "lblDetailInternatAccessCode";
            this.lblDetailInternatAccessCode.AutoSize = true;
            this.lblDetailInternatAccessCode.Text = "Int'l Access Code:";
            this.lblDetailInternatAccessCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailInternatAccessCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailInternatAccessCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailInternatTelephoneCode
            //
            this.txtDetailInternatTelephoneCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailInternatTelephoneCode.Name = "txtDetailInternatTelephoneCode";
            this.txtDetailInternatTelephoneCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailInternatTelephoneCode.Size = new System.Drawing.Size(30, 28);
            this.txtDetailInternatTelephoneCode.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Integer;
            this.txtDetailInternatTelephoneCode.DecimalPlaces = 2;
            this.txtDetailInternatTelephoneCode.NullValueAllowed = true;
            //
            // lblDetailInternatTelephoneCode
            //
            this.lblDetailInternatTelephoneCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailInternatTelephoneCode.Name = "lblDetailInternatTelephoneCode";
            this.lblDetailInternatTelephoneCode.AutoSize = true;
            this.lblDetailInternatTelephoneCode.Text = "Int'l Dialling Code:";
            this.lblDetailInternatTelephoneCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailInternatTelephoneCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailInternatTelephoneCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 144));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 180));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblDetailUndercover, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkDetailUndercover, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailInternatAccessCode, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailInternatAccessCode, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailInternatTelephoneCode, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtDetailInternatTelephoneCode, 5, 0);
            //
            // cmbDetailAddressOrder
            //
            this.cmbDetailAddressOrder.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAddressOrder.Name = "cmbDetailAddressOrder";
            this.cmbDetailAddressOrder.Size = new System.Drawing.Size(145, 28);
            this.cmbDetailAddressOrder.ListTable = TCmbAutoPopulated.TListTableEnum.AddressDisplayOrderList;
            //
            // lblDetailAddressOrder
            //
            this.lblDetailAddressOrder.Location = new System.Drawing.Point(2,2);
            this.lblDetailAddressOrder.Name = "lblDetailAddressOrder";
            this.lblDetailAddressOrder.AutoSize = true;
            this.lblDetailAddressOrder.Text = "Address Display Order:";
            this.lblDetailAddressOrder.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAddressOrder.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAddressOrder.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailInternatPostalTypeCode
            //
            this.cmbDetailInternatPostalTypeCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailInternatPostalTypeCode.Name = "cmbDetailInternatPostalTypeCode";
            this.cmbDetailInternatPostalTypeCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailInternatPostalTypeCode.ListTable = TCmbAutoPopulated.TListTableEnum.InternationalPostalTypeList;
            //
            // lblDetailInternatPostalTypeCode
            //
            this.lblDetailInternatPostalTypeCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailInternatPostalTypeCode.Name = "lblDetailInternatPostalTypeCode";
            this.lblDetailInternatPostalTypeCode.AutoSize = true;
            this.lblDetailInternatPostalTypeCode.Text = "Int'l Postal Type:";
            this.lblDetailInternatPostalTypeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailInternatPostalTypeCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailInternatPostalTypeCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // pnlTimeZone
            //
            this.pnlTimeZone.Location = new System.Drawing.Point(2,2);
            this.pnlTimeZone.Name = "pnlTimeZone";
            this.pnlTimeZone.Margin = new System.Windows.Forms.Padding(35,0,0,5);
            this.pnlTimeZone.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlTimeZone.Controls.Add(this.tableLayoutPanel4);
            //
            // txtDetailTimeZoneMinimum
            //
            this.txtDetailTimeZoneMinimum.Location = new System.Drawing.Point(2,2);
            this.txtDetailTimeZoneMinimum.Name = "txtDetailTimeZoneMinimum";
            this.txtDetailTimeZoneMinimum.Size = new System.Drawing.Size(45, 28);
            this.txtDetailTimeZoneMinimum.Leave += new System.EventHandler(this.UpdateTimeZoneMaximum);
            this.txtDetailTimeZoneMinimum.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            this.txtDetailTimeZoneMinimum.DecimalPlaces = 2;
            this.txtDetailTimeZoneMinimum.NullValueAllowed = true;
            //
            // lblDetailTimeZoneMinimum
            //
            this.lblDetailTimeZoneMinimum.Location = new System.Drawing.Point(2,2);
            this.lblDetailTimeZoneMinimum.Name = "lblDetailTimeZoneMinimum";
            this.lblDetailTimeZoneMinimum.AutoSize = true;
            this.lblDetailTimeZoneMinimum.Text = "Time Zone From:";
            this.lblDetailTimeZoneMinimum.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailTimeZoneMinimum.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailTimeZoneMinimum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailTimeZoneMaximum
            //
            this.txtDetailTimeZoneMaximum.Location = new System.Drawing.Point(2,2);
            this.txtDetailTimeZoneMaximum.Name = "txtDetailTimeZoneMaximum";
            this.txtDetailTimeZoneMaximum.Size = new System.Drawing.Size(45, 28);
            this.txtDetailTimeZoneMaximum.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            this.txtDetailTimeZoneMaximum.DecimalPlaces = 2;
            this.txtDetailTimeZoneMaximum.NullValueAllowed = true;
            //
            // lblDetailTimeZoneMaximum
            //
            this.lblDetailTimeZoneMaximum.Location = new System.Drawing.Point(2,2);
            this.lblDetailTimeZoneMaximum.Name = "lblDetailTimeZoneMaximum";
            this.lblDetailTimeZoneMaximum.AutoSize = true;
            this.lblDetailTimeZoneMaximum.Text = "To:";
            this.lblDetailTimeZoneMaximum.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailTimeZoneMaximum.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailTimeZoneMaximum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // chkDetailDeletable
            //
            this.chkDetailDeletable.Location = new System.Drawing.Point(2,2);
            this.chkDetailDeletable.Name = "chkDetailDeletable";
            this.chkDetailDeletable.Size = new System.Drawing.Size(30, 28);
            this.chkDetailDeletable.Text = "";
            this.chkDetailDeletable.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            //
            // lblDetailDeletable
            //
            this.lblDetailDeletable.Location = new System.Drawing.Point(2,2);
            this.lblDetailDeletable.Name = "lblDetailDeletable";
            this.lblDetailDeletable.AutoSize = true;
            this.lblDetailDeletable.Text = "Deletable:";
            this.lblDetailDeletable.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailDeletable.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailDeletable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 60));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 97));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.lblDetailTimeZoneMinimum, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailTimeZoneMinimum, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailTimeZoneMaximum, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtDetailTimeZoneMaximum, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblDetailDeletable, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkDetailDeletable, 5, 0);
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 127));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDetailCountryCode, 0, 0);
            this.tableLayoutPanel2.SetColumnSpan(this.pnlInternat, 4);
            this.tableLayoutPanel2.Controls.Add(this.pnlInternat, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailAddressOrder, 0, 3);
            this.tableLayoutPanel2.SetColumnSpan(this.pnlTimeZone, 4);
            this.tableLayoutPanel2.Controls.Add(this.pnlTimeZone, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailCountryCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbDetailAddressOrder, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailCountryName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailCountryNameLocal, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailInternatPostalTypeCode, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailCountryName, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailCountryNameLocal, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbDetailInternatPostalTypeCode, 3, 3);
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
            // tbbNew
            //
            this.tbbNew.Name = "tbbNew";
            this.tbbNew.AutoSize = true;
            this.tbbNew.Click += new System.EventHandler(this.NewRecord);
            this.tbbNew.Text = "New Country";
            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbrMain.AutoSize = true;
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                           tbbSave,
                        tbbNew});
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
            // TFrmCountrySetup
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(740, 700);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmCountrySetup";
            this.Text = "Maintain Countries";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlTimeZone.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlInternat.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlGrid;
        private Ict.Common.Controls.TSgrdDataGridPaged grdDetails;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtDetailCountryCode;
        private System.Windows.Forms.Label lblDetailCountryCode;
        private System.Windows.Forms.TextBox txtDetailCountryName;
        private System.Windows.Forms.Label lblDetailCountryName;
        private System.Windows.Forms.TextBox txtDetailCountryNameLocal;
        private System.Windows.Forms.Label lblDetailCountryNameLocal;
        private System.Windows.Forms.Panel pnlInternat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox chkDetailUndercover;
        private System.Windows.Forms.Label lblDetailUndercover;
        private System.Windows.Forms.TextBox txtDetailInternatAccessCode;
        private System.Windows.Forms.Label lblDetailInternatAccessCode;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailInternatTelephoneCode;
        private System.Windows.Forms.Label lblDetailInternatTelephoneCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailAddressOrder;
        private System.Windows.Forms.Label lblDetailAddressOrder;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailInternatPostalTypeCode;
        private System.Windows.Forms.Label lblDetailInternatPostalTypeCode;
        private System.Windows.Forms.Panel pnlTimeZone;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailTimeZoneMinimum;
        private System.Windows.Forms.Label lblDetailTimeZoneMinimum;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailTimeZoneMaximum;
        private System.Windows.Forms.Label lblDetailTimeZoneMaximum;
        private System.Windows.Forms.CheckBox chkDetailDeletable;
        private System.Windows.Forms.Label lblDetailDeletable;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbbSave;
        private System.Windows.Forms.ToolStripButton tbbNew;
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
