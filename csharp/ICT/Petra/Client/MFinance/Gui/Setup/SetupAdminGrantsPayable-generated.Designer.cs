// auto generated with nant generateWinforms from SetupAdminGrantsPayable.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
    partial class TFrmSetupAdminGrantsPayable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmSetupAdminGrantsPayable));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.grdDetails = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDetailFeeCode = new System.Windows.Forms.TextBox();
            this.lblDetailFeeCode = new System.Windows.Forms.Label();
            this.txtDetailFeeDescription = new System.Windows.Forms.TextBox();
            this.lblDetailFeeDescription = new System.Windows.Forms.Label();
            this.cmbDetailChargeOption = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblDetailChargeOption = new System.Windows.Forms.Label();
            this.txtDetailChargePercentage = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailChargePercentage = new System.Windows.Forms.Label();
            this.txtDetailChargeAmount = new Ict.Common.Controls.TTxtNumericTextBox();
            this.lblDetailChargeAmount = new System.Windows.Forms.Label();
            this.grpAssignment = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblToBeDebited = new System.Windows.Forms.Label();
            this.lblToBeCredited = new System.Windows.Forms.Label();
            this.txtReceivingFund = new System.Windows.Forms.TextBox();
            this.lblReceivingFund = new System.Windows.Forms.Label();
            this.cmbDetailCostCentreCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbDetailAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblDetailAccountCode = new System.Windows.Forms.Label();
            this.cmbDetailDrAccountCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
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
            this.grpAssignment.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
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
            this.btnNew.Click += new System.EventHandler(this.NewRow);
            this.btnNew.Text = "&New";
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
            // txtDetailFeeCode
            //
            this.txtDetailFeeCode.Location = new System.Drawing.Point(2,2);
            this.txtDetailFeeCode.Name = "txtDetailFeeCode";
            this.txtDetailFeeCode.Size = new System.Drawing.Size(150, 28);
            this.txtDetailFeeCode.CharacterCasing = CharacterCasing.Upper;
            //
            // lblDetailFeeCode
            //
            this.lblDetailFeeCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailFeeCode.Name = "lblDetailFeeCode";
            this.lblDetailFeeCode.AutoSize = true;
            this.lblDetailFeeCode.Text = "Fee Code:";
            this.lblDetailFeeCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailFeeCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailFeeCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailFeeDescription
            //
            this.txtDetailFeeDescription.Location = new System.Drawing.Point(2,2);
            this.txtDetailFeeDescription.Name = "txtDetailFeeDescription";
            this.txtDetailFeeDescription.Size = new System.Drawing.Size(290, 28);
            //
            // lblDetailFeeDescription
            //
            this.lblDetailFeeDescription.Location = new System.Drawing.Point(2,2);
            this.lblDetailFeeDescription.Name = "lblDetailFeeDescription";
            this.lblDetailFeeDescription.AutoSize = true;
            this.lblDetailFeeDescription.Text = "Description:";
            this.lblDetailFeeDescription.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailFeeDescription.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailFeeDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailChargeOption
            //
            this.cmbDetailChargeOption.Location = new System.Drawing.Point(2,2);
            this.cmbDetailChargeOption.Name = "cmbDetailChargeOption";
            this.cmbDetailChargeOption.Size = new System.Drawing.Size(150, 28);
            this.cmbDetailChargeOption.SelectedValueChanged += new System.EventHandler(this.ChargeOptionChanged);
            this.cmbDetailChargeOption.Items.AddRange(new object[] {"Minimum","Maximum","Fixed","Percentage"});
            //
            // lblDetailChargeOption
            //
            this.lblDetailChargeOption.Location = new System.Drawing.Point(2,2);
            this.lblDetailChargeOption.Name = "lblDetailChargeOption";
            this.lblDetailChargeOption.AutoSize = true;
            this.lblDetailChargeOption.Text = "Charge Option:";
            this.lblDetailChargeOption.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailChargeOption.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailChargeOption.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailChargePercentage
            //
            this.txtDetailChargePercentage.Location = new System.Drawing.Point(2,2);
            this.txtDetailChargePercentage.Name = "txtDetailChargePercentage";
            this.txtDetailChargePercentage.Size = new System.Drawing.Size(80, 28);
            this.txtDetailChargePercentage.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            this.txtDetailChargePercentage.DecimalPlaces = 2;
            this.txtDetailChargePercentage.NullValueAllowed = true;
            //
            // lblDetailChargePercentage
            //
            this.lblDetailChargePercentage.Location = new System.Drawing.Point(2,2);
            this.lblDetailChargePercentage.Name = "lblDetailChargePercentage";
            this.lblDetailChargePercentage.AutoSize = true;
            this.lblDetailChargePercentage.Text = "Charge Percentage:";
            this.lblDetailChargePercentage.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailChargePercentage.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailChargePercentage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDetailChargeAmount
            //
            this.txtDetailChargeAmount.Location = new System.Drawing.Point(2,2);
            this.txtDetailChargeAmount.Name = "txtDetailChargeAmount";
            this.txtDetailChargeAmount.Size = new System.Drawing.Size(80, 28);
            this.txtDetailChargeAmount.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            this.txtDetailChargeAmount.DecimalPlaces = 2;
            this.txtDetailChargeAmount.NullValueAllowed = true;
            //
            // lblDetailChargeAmount
            //
            this.lblDetailChargeAmount.Location = new System.Drawing.Point(2,2);
            this.lblDetailChargeAmount.Name = "lblDetailChargeAmount";
            this.lblDetailChargeAmount.AutoSize = true;
            this.lblDetailChargeAmount.Text = "Charge Amount:";
            this.lblDetailChargeAmount.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailChargeAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailChargeAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // grpAssignment
            //
            this.grpAssignment.Location = new System.Drawing.Point(2,2);
            this.grpAssignment.Name = "grpAssignment";
            this.grpAssignment.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpAssignment.Controls.Add(this.tableLayoutPanel3);
            //
            // lblToBeDebited
            //
            this.lblToBeDebited.Location = new System.Drawing.Point(2,2);
            this.lblToBeDebited.Name = "lblToBeDebited";
            this.lblToBeDebited.Padding = new System.Windows.Forms.Padding(96,0,0,0);
            this.lblToBeDebited.AutoSize = true;
            this.lblToBeDebited.Text = "To Be Debited:";
            this.lblToBeDebited.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // lblToBeCredited
            //
            this.lblToBeCredited.Location = new System.Drawing.Point(2,2);
            this.lblToBeCredited.Name = "lblToBeCredited";
            this.lblToBeCredited.AutoSize = true;
            this.lblToBeCredited.Text = "To Be Credited:";
            this.lblToBeCredited.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // txtReceivingFund
            //
            this.txtReceivingFund.Location = new System.Drawing.Point(2,2);
            this.txtReceivingFund.Name = "txtReceivingFund";
            this.txtReceivingFund.Enabled = false;
            this.txtReceivingFund.Size = new System.Drawing.Size(200, 28);
            //
            // lblReceivingFund
            //
            this.lblReceivingFund.Location = new System.Drawing.Point(2,2);
            this.lblReceivingFund.Name = "lblReceivingFund";
            this.lblReceivingFund.AutoSize = true;
            this.lblReceivingFund.Text = "Cost Centre:";
            this.lblReceivingFund.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblReceivingFund.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblReceivingFund.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailCostCentreCode
            //
            this.cmbDetailCostCentreCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailCostCentreCode.Name = "cmbDetailCostCentreCode";
            this.cmbDetailCostCentreCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailCostCentreCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // cmbDetailAccountCode
            //
            this.cmbDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailAccountCode.Name = "cmbDetailAccountCode";
            this.cmbDetailAccountCode.Size = new System.Drawing.Size(250, 28);
            this.cmbDetailAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            //
            // lblDetailAccountCode
            //
            this.lblDetailAccountCode.Location = new System.Drawing.Point(2,2);
            this.lblDetailAccountCode.Name = "lblDetailAccountCode";
            this.lblDetailAccountCode.AutoSize = true;
            this.lblDetailAccountCode.Text = "Account:";
            this.lblDetailAccountCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDetailAccountCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDetailAccountCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbDetailDrAccountCode
            //
            this.cmbDetailDrAccountCode.Location = new System.Drawing.Point(2,2);
            this.cmbDetailDrAccountCode.Name = "cmbDetailDrAccountCode";
            this.cmbDetailDrAccountCode.Size = new System.Drawing.Size(300, 28);
            this.cmbDetailDrAccountCode.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 96));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 284));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 289));
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.SetColumnSpan(this.lblToBeDebited, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblToBeDebited, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblReceivingFund, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDetailAccountCode, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtReceivingFund, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailAccountCode, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblToBeCredited, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailCostCentreCode, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbDetailDrAccountCode, 2, 2);
            this.grpAssignment.Text = "Assignment";
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 123));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDetailFeeCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailChargeOption, 0, 1);
            this.tableLayoutPanel2.SetColumnSpan(this.grpAssignment, 6);
            this.tableLayoutPanel2.Controls.Add(this.grpAssignment, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailFeeCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbDetailChargeOption, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailFeeDescription, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailChargePercentage, 2, 1);
            this.tableLayoutPanel2.SetColumnSpan(this.txtDetailFeeDescription, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailFeeDescription, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailChargePercentage, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblDetailChargeAmount, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtDetailChargeAmount, 5, 1);
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
            this.tbbNew.Click += new System.EventHandler(this.NewRow);
            this.tbbNew.Text = "New Admin. Grant Payable";
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
            // TFrmSetupAdminGrantsPayable
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(760, 500);

            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = mnuMain;
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "TFrmSetupAdminGrantsPayable";
            this.Text = "Maintain Admin. Grants Payable";

            this.Activated += new System.EventHandler(this.TFrmPetra_Activated);
            this.Load += new System.EventHandler(this.TFrmPetra_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TFrmPetra_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.Closed += new System.EventHandler(this.TFrmPetra_Closed);

            this.stbMain.ResumeLayout(false);
            this.mnuMain.ResumeLayout(false);
            this.tbrMain.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpAssignment.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtDetailFeeCode;
        private System.Windows.Forms.Label lblDetailFeeCode;
        private System.Windows.Forms.TextBox txtDetailFeeDescription;
        private System.Windows.Forms.Label lblDetailFeeDescription;
        private Ict.Common.Controls.TCmbAutoComplete cmbDetailChargeOption;
        private System.Windows.Forms.Label lblDetailChargeOption;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailChargePercentage;
        private System.Windows.Forms.Label lblDetailChargePercentage;
        private Ict.Common.Controls.TTxtNumericTextBox txtDetailChargeAmount;
        private System.Windows.Forms.Label lblDetailChargeAmount;
        private System.Windows.Forms.GroupBox grpAssignment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblToBeDebited;
        private System.Windows.Forms.Label lblToBeCredited;
        private System.Windows.Forms.TextBox txtReceivingFund;
        private System.Windows.Forms.Label lblReceivingFund;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailCostCentreCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailAccountCode;
        private System.Windows.Forms.Label lblDetailAccountCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbDetailDrAccountCode;
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
