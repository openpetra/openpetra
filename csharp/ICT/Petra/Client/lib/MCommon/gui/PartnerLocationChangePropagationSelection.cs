//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using SourceGrid;
using Ict.Petra.Shared.MPartner.Partner.Data;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;

TODO: split into PartnerAddressChangePropagationSelection.cs and PartnerAddressChangePropagationSelection.Designer.cs
TODO: move SetStatusBarText calls to constructor; make sure that Catalog.GetString is used

namespace Ict.Petra.Client.MCommon
{
    /// Partner Location Change Propagation Selection Dialog.
    /// Called from Partner Edit screen/Address tab (UC_PartnerAddresses).
    public class TPartnerLocationChangePropagationSelectionWinForm : System.Windows.Forms.Form
    {
        // TODO: TFrmPetraDialog
        public const String StrDefaultFormTitle = "Partner-specific Address Data Change - Family Member Promotion";
        public const String StrDefaultExplanationText = "The list below shows all addresses of members " + "of the family";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblAdvise;
        private TSgrdDataGrid grdPersonsLocations;
        private System.Windows.Forms.CheckBox chkChangeAllPartners;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblAddressLines;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label1;
        private TSgrdDataGrid grdChangedDetails;
        private TPartnerLocationChangePropagationSelectionLogic FLogic;
        private System.Data.DataView FPartnerSharingLocationDV;
        private PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow FAddressAddedOrChangedPromotionDR;
        private PLocationRow FLocationRow;
        private String FUserAnswer;
        private DataTable FChangedColumnsDT;
        private DataView FChangedDetailsDV;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerLocationChangePropagationSelectionWinForm));
            this.lblAdvise = new System.Windows.Forms.Label();
            this.grdPersonsLocations = new Ict.Common.Controls.TSgrdDataGrid();
            this.chkChangeAllPartners = new System.Windows.Forms.CheckBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblAddressLines = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.grdChangedDetails = new Ict.Common.Controls.TSgrdDataGrid();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 20);
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept selection and continue.");
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 464);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(562, 34);

            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(88, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 20);
            this.sbtForm.SetStatusBarText(this.btnCancel, "Ignore selection; don't ch" + "ange any data of the listed Partners.");

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(482, 8);
            this.btnHelp.Name = "btnHelp";
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");
            this.btnHelp.TabIndex = 996;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 498);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(562, 22);

            //
            // stpInfo
            //
            this.stpInfo.Width = 562;

            //
            // lblAdvise
            //
            this.lblAdvise.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdvise.Location = new System.Drawing.Point(16, 238);
            this.lblAdvise.Name = "lblAdvise";
            this.lblAdvise.Size = new System.Drawing.Size(536, 16);
            this.lblAdvise.TabIndex = 0;
            this.lblAdvise.Text = "Select any addresses to which these changes should" + " be applied to.";

            //
            // grdPersonsLocations
            //
            this.grdPersonsLocations.AlternatingBackgroundColour = System.Drawing.Color.White;
            this.grdPersonsLocations.BackColor = System.Drawing.SystemColors.Control;
            this.grdPersonsLocations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdPersonsLocations.DeleteQuestionMessage = "You have chosen to dele" +
                                                             "te this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdPersonsLocations.FixedColumns = 2;
            this.grdPersonsLocations.FixedRows = 1;
            this.grdPersonsLocations.Location = new System.Drawing.Point(8, 278);
            this.grdPersonsLocations.MinimumHeight = 19;
            this.grdPersonsLocations.Name = "grdPersonsLocations";
            this.grdPersonsLocations.Size = new System.Drawing.Size(546, 188);
            this.grdPersonsLocations.SpecialKeys =
                (SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                  SourceGrid.GridSpecialKeys.PageDownUp) |
                                                 SourceGrid.GridSpecialKeys.Enter) |
                                                SourceGrid.GridSpecialKeys.Escape) |
                                               SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift));
            this.sbtForm.SetStatusBarText(this.grdPersonsLocations, "Select addresses " + "(use CTRL key+Mouse click for multiple selection).");
            this.grdPersonsLocations.TabIndex = 1;
            this.grdPersonsLocations.TabStop = true;

            //
            // chkChangeAllPartners
            //
            this.chkChangeAllPartners.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.chkChangeAllPartners.Location = new System.Drawing.Point(28, 254);
            this.chkChangeAllPartners.Name = "chkChangeAllPartners";
            this.chkChangeAllPartners.Size = new System.Drawing.Size(524, 24);
            this.sbtForm.SetStatusBarText(this.chkChangeAllPartners, "Tick this to tak" + "e the changes over to all listed addresses.");
            this.chkChangeAllPartners.TabIndex = 0;
            this.chkChangeAllPartners.Text = "Apply changes to all addresses";

            //
            // lblExplanation
            //
            this.lblExplanation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExplanation.Location = new System.Drawing.Point(16, 222);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(536, 16);
            this.lblExplanation.TabIndex = 998;
            this.lblExplanation.Text = "The list below shows all addresses of members" + " of the family.  (VARIABLE TEXT)";

            //
            // lblAddressLines
            //
            this.lblAddressLines.Location = new System.Drawing.Point(28, 30);
            this.lblAddressLines.Name = "lblAddressLines";
            this.lblAddressLines.Size = new System.Drawing.Size(400, 82);
            this.lblAddressLines.TabIndex = 999;
            this.lblAddressLines.Text = "Address lines go here  (VARIABLE TEXT)";

            //
            // Label2
            //
            this.Label2.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.Location = new System.Drawing.Point(16, 10);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(536, 16);
            this.Label2.TabIndex = 998;
            this.Label2.Text = "You have changed the following address:";

            //
            // Label1
            //
            this.Label1.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.Location = new System.Drawing.Point(16, 116);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(536, 16);
            this.Label1.TabIndex = 1000;
            this.Label1.Text = "Changed Partner-specific details:";

            //
            // grdChangedDetails
            //
            this.grdChangedDetails.AlternatingBackgroundColour = System.Drawing.Color.White;
            this.grdChangedDetails.BackColor = System.Drawing.SystemColors.Control;
            this.grdChangedDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdChangedDetails.DeleteQuestionMessage = "You have chosen to delete" +
                                                           " this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdChangedDetails.FixedRows = 1;
            this.grdChangedDetails.Location = new System.Drawing.Point(24, 132);
            this.grdChangedDetails.MinimumHeight = 19;
            this.grdChangedDetails.Name = "grdChangedDetails";
            this.grdChangedDetails.Size = new System.Drawing.Size(400, 80);
            this.grdChangedDetails.SpecialKeys =
                (SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                  SourceGrid.GridSpecialKeys.PageDownUp) |
                                                 SourceGrid.GridSpecialKeys.Enter) |
                                                SourceGrid.GridSpecialKeys.Escape) |
                                               SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift));
            this.sbtForm.SetStatusBarText(this.grdChangedDetails, "Changes to Partner-" + "specific details");
            this.grdChangedDetails.TabIndex = 4;
            this.grdChangedDetails.TabStop = true;

            //
            // TPartnerLocationChangePropagationSelectionWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(562, 520);
            this.Controls.Add(this.grdPersonsLocations);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblAddressLines);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.chkChangeAllPartners);
            this.Controls.Add(this.lblAdvise);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.grdChangedDetails);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerLocationChangePropagationSelectionWinForm";
            this.Text = "Partner-specific Address Data Change - Family Member Promoti" + "on  (VARIABLE TEXT)";
            this.Load += new System.EventHandler(this.TPartnerLocationChangePropagationSelectionWinForm_Load);
            this.Controls.SetChildIndex(this.grdChangedDetails, 0);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.lblAdvise, 0);
            this.Controls.SetChildIndex(this.chkChangeAllPartners, 0);
            this.Controls.SetChildIndex(this.lblExplanation, 0);
            this.Controls.SetChildIndex(this.lblAddressLines, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.grdPersonsLocations, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal
        public TPartnerLocationChangePropagationSelectionWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FFormSetupFinished = false;
        }

        private void BuildChangedColumnsDT()
        {
            StringCollection ChangedFieldsColl;
            Int32 Counter;
            DataColumn ChangedColumnsDC;
            DataRow ChangedColumnsDR;

            FChangedColumnsDT = new DataTable("ChangedColumns");
            ChangedColumnsDC = new DataColumn();
            ChangedColumnsDC.DataType = System.Type.GetType("System.String");
            ChangedColumnsDC.ColumnName = "DBName";
            FChangedColumnsDT.Columns.Add(ChangedColumnsDC);
            ChangedColumnsDC = new DataColumn();
            ChangedColumnsDC.DataType = System.Type.GetType("System.String");
            ChangedColumnsDC.ColumnName = "DBLabel";
            FChangedColumnsDT.Columns.Add(ChangedColumnsDC);
            ChangedColumnsDC = new DataColumn();
            ChangedColumnsDC.DataType = System.Type.GetType("System.String");
            ChangedColumnsDC.ColumnName = "OriginalValue";
            FChangedColumnsDT.Columns.Add(ChangedColumnsDC);
            ChangedColumnsDC = new DataColumn();
            ChangedColumnsDC.DataType = System.Type.GetType("System.String");
            ChangedColumnsDC.ColumnName = "CurrentValue";
            FChangedColumnsDT.Columns.Add(ChangedColumnsDC);

            // Create a Collection by splitting String 'ChangedFields'
            // The String contains: DBName, Label, OriginalValue, CurrentValue for each changed DataColumn
            ChangedFieldsColl = StringHelper.StrSplit(FAddressAddedOrChangedPromotionDR.ChangedFields, "|");
            Counter = 0;

            while (Counter < ChangedFieldsColl.Count - 1)
            {
                ChangedColumnsDR = FChangedColumnsDT.NewRow();
                ChangedColumnsDR[0] = ChangedFieldsColl[Counter];
                ChangedColumnsDR[1] = ChangedFieldsColl[Counter + 1];
                ChangedColumnsDR[2] = ChangedFieldsColl[Counter + 2];
                ChangedColumnsDR[3] = ChangedFieldsColl[Counter + 3];

                // MessageBox.Show('Counter: ' + Counter.ToString + "\r\n" +
                // 'Column 0:' + ChangedColumnsDR[0].ToString + "\r\n" +
                // 'Column 1:' + ChangedColumnsDR[1].ToString + "\r\n" +
                // 'Column 2:' + ChangedColumnsDR[2].ToString + "\r\n" +
                // 'Column 3:' + ChangedColumnsDR[3].ToString + "\r\n" +  #10#13 +
                // 'ChangedFieldsColl.Count: ' + ChangedFieldsColl.Count.ToString);
                FChangedColumnsDT.Rows.Add(ChangedColumnsDR);

                // position Counter to next DB Field name
                Counter = Counter + 4;
            }
        }

//        procedure grdPersonsLocations_DoubleClickCell(Sender: System.Object; e: SourceGrid.CellContextEventArgs);
//        procedure grdPersonsLocations_EnterKeyPressed(Sender: System.Object; e: SourceGrid.RowEventArgs);
//        procedure grdPersonsLocations_SpaceKeyPressed(Sender: System.Object; e: SourceGrid.RowEventArgs);
//        procedure grdFamilyMembers_DataGrid_FocusRowEntered(Sender: System.Object; e: SourceGrid.RowEventArgs);

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="Disposing"></param>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        #endregion

        #region Form Parameters

        /// <summary>
        /// procedure OnPartnerSharingLocationColumnChanging(sender: System.Object; e: DataColumnChangeEventArgs);
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow AAddressAddedOrChangedPromotionDR,
            DataView APartnerSharingLocationDV,
            PLocationRow ALocationRow,
            String AOtherFormTitle,
            String AOtherExplanation)
        {
            FAddressAddedOrChangedPromotionDR = AAddressAddedOrChangedPromotionDR;
            FPartnerSharingLocationDV = APartnerSharingLocationDV;

            // MessageBox.Show('FPartnerSharingLocationDV.Count: ' + FPartnerSharingLocationDV.Count.ToString);
            FLocationRow = ALocationRow;
            ApplyText(AOtherFormTitle, AOtherExplanation, FLocationRow);
        }

        public Boolean GetReturnedParameters(out String AUserAnswer)
        {
            Boolean ReturnValue;

            AUserAnswer = "";

            if (FFormSetupFinished)
            {
                AUserAnswer = FUserAnswer;
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        #endregion

        #region Event Handlers

//        procedure TPartnerLocationChangePropagationSelectionWinForm.OnPartnerSharingLocationColumnChanging(sender: System.Object; e: DataColumnChangeEventArgs);
//        begin  if (this.FFormSetupFinished = true) then  begin    FLogic.CheckedStateForRowChanging(e);  end;end;
//        procedure TPartnerLocationChangePropagationSelectionWinForm.grdPersonsLocations_DoubleClickCell(Sender: System.Object; e: SourceGrid3.CellContextEventArgs);
//        var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow((e.CellContext.Position.Row 1), mChanged);/  messagebox.Show('grdFamilyMembers_DoubleClickCell');end;
//        procedure TPartnerLocationChangePropagationSelectionWinForm.grdPersonsLocations_SpaceKeyPressed(Sender: System.Object; e: SourceGrid3.RowEventArgs);
//        var  mChanged: System.Boolean;begin  grdPersonsLocations.Selection.SelectRow(e.Row, not grdPersonsLocations.Selection.ContainsRow(e.Row));/  messagebox.Show('grdFamilyMembers_SpaceKeyPressed');end;
//        procedure TPartnerLocationChangePropagationSelectionWinForm.grdPersonsLocations_EnterKeyPressed(Sender: System.Object; e: SourceGrid3.RowEventArgs);
//        var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow(e.Row 1, mChanged);/  messagebox.Show('grdFamilyMembers_EnterKeyPressed');end;
//        procedure TPartnerLocationChangePropagationSelectionWinForm.grdFamilyMembers_DataGrid_FocusRowEntered(Sender: System.Object; e: SourceGrid3.RowEventArgs);
//        var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow(e.Row 1, mChanged);end;

        private void ChkChangeAllPartners_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            // if chkChangeAllPartners.Checked then
            // begin
            // grdPersonsLocations.Enabled := false;
            // end
            // else
            // begin
            // grdPersonsLocations.Enabled := true;
            // end;
            // FLogic.ChangeCheckedStateForAllRows(this.chkChangeAllPartners.Checked);
            // messagebox.show('chkAddToAllFamilyMembers_CheckedChanged');
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            DataRowView[] SelectedRows;
            int Counter;
            SelectedRows = grdPersonsLocations.SelectedDataRowsAsDataRowView;

            if (chkChangeAllPartners.Checked)
            {
                FUserAnswer = "CHANGE-ALL";
            }
            else if (SelectedRows.Length == 0)
            {
                FUserAnswer = "CHANGE-NONE";
            }
            else if (SelectedRows.Length == FPartnerSharingLocationDV.Count)
            {
                FUserAnswer = "CHANGE-ALL";
            }
            else
            {
                FUserAnswer = "CHANGE-SOME" + ':';

                // Build list of those PartnerLocations that are selected
                // It's format is 'CHANGESOME:PartnerKey1,SiteKey1,LocationKey1;PartnerKey2,SiteKey2,LocationKey2;PartnerKeyN,SiteKeyN,LocationKeyN'
                for (Counter = 0; Counter <= SelectedRows.Length - 1; Counter += 1)
                {
                    FUserAnswer = FUserAnswer +
                                  ((PartnerAddressAggregateTDSChangePromotionParametersRow)SelectedRows[Counter].Row).PartnerKey.ToString() + ',';
                    FUserAnswer = FUserAnswer +
                                  ((PartnerAddressAggregateTDSChangePromotionParametersRow)SelectedRows[Counter].Row).SiteKey.ToString() + ',';
                    FUserAnswer = FUserAnswer +
                                  ((PartnerAddressAggregateTDSChangePromotionParametersRow)SelectedRows[Counter].Row).LocationKey.ToString() + ';';
                }

                // Remove last ';'
                FUserAnswer = FUserAnswer.Substring(0, FUserAnswer.Length - 1);
            }

//          MessageBox.Show("FUserAnswer: " + FUserAnswer);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void TPartnerLocationChangePropagationSelectionWinForm_Load(System.Object sender, System.EventArgs e)
        {
            FLogic = new TPartnerLocationChangePropagationSelectionLogic();
            BuildChangedColumnsDT();
            FLogic.CreateColumnsPersonsLocations(grdPersonsLocations, FPartnerSharingLocationDV.Table);
            FLogic.CreateColumnsChangedDetails(grdChangedDetails, FChangedColumnsDT);

            // DataBindingrelated stuff
            SetupDataGridDataBindings();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearances();

            // messagebox.show('Rows: ' + this.grdFamilyMembers.Rows.Count.ToString);
            // DataSetup and the rest of the initialisation bit
            FLogic.PartnerSharingLocationDV = FPartnerSharingLocationDV;
            FLogic.InitialisePartnerTypeFamilyMembers((PartnerAddressAggregateTDSChangePromotionParametersTable) this.FPartnerSharingLocationDV.Table);
            FFormSetupFinished = true;
        }

        #endregion

        #region Helper functions
        protected void ApplyText(String AOtherFormTitle, String AOtherExplanation, PLocationRow ALocationRow)
        {
            PLocationTable LocationDT;

            if (AOtherExplanation != "")
            {
                lblExplanation.Text = AOtherExplanation;
            }
            else
            {
                lblExplanation.Text = StrDefaultExplanationText;
            }

            if (AOtherFormTitle != "")
            {
                this.Text = AOtherFormTitle;
            }
            else
            {
                this.Text = StrDefaultFormTitle;
            }

            // Set up address lines display
            LocationDT = (PLocationTable)ALocationRow.Table;
            lblAddressLines.Text =
                TSaveConvert.StringColumnToString(LocationDT.ColumnLocality, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnStreetName,
                    ALocationRow) + "\r\n" +
                TSaveConvert.StringColumnToString(LocationDT.ColumnAddress3, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnCity,
                    ALocationRow) + ' ' +
                TSaveConvert.StringColumnToString(LocationDT.ColumnPostalCode, ALocationRow) + "\r\n" + TSaveConvert.StringColumnToString(
                    LocationDT.ColumnCounty,
                    ALocationRow) + ' ' + TSaveConvert.StringColumnToString(LocationDT.ColumnCountryCode, ALocationRow);
        }

        #endregion

        #region Setup SourceDataGrid
        private void SetupDataGridDataBindings()
        {
            FPartnerSharingLocationDV.AllowNew = false;
            FPartnerSharingLocationDV.AllowDelete = false;
            FPartnerSharingLocationDV.AllowEdit = false;
            FChangedDetailsDV = FChangedColumnsDT.DefaultView;
            FChangedDetailsDV.AllowNew = false;
            FChangedDetailsDV.AllowDelete = false;
            FChangedDetailsDV.AllowEdit = false;

            // DataBind the DataGrids
            grdPersonsLocations.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSharingLocationDV);
            grdChangedDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FChangedDetailsDV);
        }

        private void SetupDataGridVisualAppearances()
        {
            grdPersonsLocations.AutoSizeCells();
            grdPersonsLocations.Width = 546; /// it is necessary to reassign the width because the columns don't take up the maximum width
            grdChangedDetails.AutoSizeCells();
            grdChangedDetails.Width = 380; /// it is necessary to reassign the width because the columns don't take up the maximum width

            // Enable selection of multiple Grid rows with the following statement.
            // Needs different row processing (using the grdPersonsLocations.SelectedDataRows property) in the following Event Handlers: DoubleClickCell, SpaceKeyPressed, EnterKeyPressed!
            grdPersonsLocations.Selection.EnableMultiSelection = true;
        }

        #endregion
    }
}