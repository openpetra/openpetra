/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using SourceGrid;
using Ict.Petra.Shared.MPartner.Partner.Data;
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
    /// Partner Address Change Propagation Selection Dialog.
    /// Called from Partner Edit screen/Address tab (UC_PartnerAddresses).
    public class TPartnerAddressChangePropagationSelectionWinForm : System.Windows.Forms.Form
    {
        // TODO: TFrmPetraDialog
        public const String StrDefaultFormTitle = "Change Location, Partner Selection";
        public const String StrDefaultExplanationText = "The following Partners also use this Partner's ad" + "dress.";
        public const String StrSecurityViolationExplanation = "Due to the selection that you have made" + " a new Address would need" + "\r\n" +
                                                              "to be created. However, you do not have permission to do this." + "\r\n" + "\r\n" +
                                                              "Either select 'Change all' to change all addresses (if this is appropriate)," +
                                                              "\r\n" + "or choose 'Cancel' to abort the Save operation.";
        public const String StrSecurityViolationExplanationTitle = "Security Violation - Explanation";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblAdvise;
        private TSgrdDataGrid grdPartnersSharingLocation;
        private System.Windows.Forms.CheckBox chkChangeAllPartners;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblAddressLines;
        private System.Windows.Forms.Label Label2;
        private TPartnerAddressChangePropagationSelectionLogic FLogic;
        private System.Data.DataView FPartnerSharingLocationDV;
        private PartnerAddressAggregateTDSAddressAddedOrChangedPromotionRow FAddressAddedOrChangedPromotionDR;
        private PLocationRow FLocationRow;
        private String FUserAnswer;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerAddressChangePropagationSelectionWinForm));
            this.lblAdvise = new System.Windows.Forms.Label();
            this.grdPartnersSharingLocation = new Ict.Common.Controls.TSgrdDataGrid();
            this.chkChangeAllPartners = new System.Windows.Forms.CheckBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblAddressLines = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 20);
            this.sbtForm.SetStatusBarText(this.btnOK,
                Catalog.GetString("" Accept selection and continue.
                    "));
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 367);
            this.pnlBtnOKCancelHelpLayout.Name = "
                    pnlBtnOKCancelHelpLayout
                    ";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(392, 34);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCancel.Location = new System.Drawing.Point(88, 8);
            this.btnCancel.Name = "
                    btnCancel
                    ";
            this.btnCancel.Size = new System.Drawing.Size(76, 20);
            this.sbtForm.SetStatusBarText(this.btnCancel, Catalog.GetString("
                    "Ignore selection; don't ch" + "ange any Location of the listed Partners."));

            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnHelp.Location = new System.Drawing.Point(312, 8);
            this.btnHelp.Name = "btnHelp";
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");
            this.btnHelp.TabIndex = 996;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 401);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(392, 22);
            this.stbMain.Visible = false;

            //
            // stpInfo
            //
            this.stpInfo.Width = 392;

            //
            // lblAdvise
            //
            this.lblAdvise.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdvise.Location = new System.Drawing.Point(16, 148);
            this.lblAdvise.Name = "lblAdvise";
            this.lblAdvise.Size = new System.Drawing.Size(350, 16);
            this.lblAdvise.TabIndex = 0;
            this.lblAdvise.Text = "Select from the list any which need to change.";

            //
            // grdPartnersSharingLocation
            //
            this.grdPartnersSharingLocation.AlternatingBackgroundColour = System.Drawing.Color.White;
            this.grdPartnersSharingLocation.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPartnersSharingLocation.BackColor = System.Drawing.SystemColors.Control;
            this.grdPartnersSharingLocation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdPartnersSharingLocation.DeleteQuestionMessage = "You have chosen " +
                                                                    "to delete this record.'#13#10#13#10'Do you really want to delete it?";
            this.grdPartnersSharingLocation.FixedRows = 1;
            this.grdPartnersSharingLocation.Location = new System.Drawing.Point(8, 188);
            this.grdPartnersSharingLocation.MinimumHeight = 19;
            this.grdPartnersSharingLocation.Name = "grdPartnersSharingLocation";
            this.grdPartnersSharingLocation.Size = new System.Drawing.Size(370, 192);
            this.grdPartnersSharingLocation.SpecialKeys =
                (SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                  SourceGrid.GridSpecialKeys.PageDownUp) |
                                                 SourceGrid.GridSpecialKeys.Enter) |
                                                SourceGrid.GridSpecialKeys.Escape) |
                                               SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift));
            this.sbtForm.SetStatusBarText(this.grdPartnersSharingLocation, "Place or r" + "emove a check mark in the first column.");
            this.grdPartnersSharingLocation.TabIndex = 1;
            this.grdPartnersSharingLocation.TabStop = true;

            //
            // chkChangeAllPartners
            //
            this.chkChangeAllPartners.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.chkChangeAllPartners.Location = new System.Drawing.Point(28, 164);
            this.chkChangeAllPartners.Name = "chkChangeAllPartners";
            this.chkChangeAllPartners.Size = new System.Drawing.Size(340, 24);
            this.sbtForm.SetStatusBarText(this.chkChangeAllPartners, "Tick this to mak" + "e the change to all listed Partners.");
            this.chkChangeAllPartners.TabIndex = 0;
            this.chkChangeAllPartners.Text = "Change all";

            //
            // lblExplanation
            //
            this.lblExplanation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExplanation.Location = new System.Drawing.Point(16, 130);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(350, 16);
            this.lblExplanation.TabIndex = 998;
            this.lblExplanation.Text = "The following Partners also use this Partner'" + "s address.  (VARIABLE TEXT)";

            //
            // lblAddressLines
            //
            this.lblAddressLines.Location = new System.Drawing.Point(28, 30);
            this.lblAddressLines.Name = "lblAddressLines";
            this.lblAddressLines.Size = new System.Drawing.Size(400, 92);
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
            this.Label2.Size = new System.Drawing.Size(350, 16);
            this.Label2.TabIndex = 998;
            this.Label2.Text = "You have changed the following address:";

            //
            // TPartnerAddressChangePropagationSelectionWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(392, 423);
            this.Controls.Add(this.lblAddressLines);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.chkChangeAllPartners);
            this.Controls.Add(this.grdPartnersSharingLocation);
            this.Controls.Add(this.lblAdvise);
            this.Controls.Add(this.Label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerAddressChangePropagationSelectionWinForm";
            this.Text = "Change Location, Partner Selection  (VARIABLE TEXT)";
            this.Load += new System.EventHandler(this.TPartnerAddressChangePropagationSelectionWinForm_Load);
            this.Controls.SetChildIndex(this.Label2, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.lblAdvise, 0);
            this.Controls.SetChildIndex(this.grdPartnersSharingLocation, 0);
            this.Controls.SetChildIndex(this.chkChangeAllPartners, 0);
            this.Controls.SetChildIndex(this.lblExplanation, 0);
            this.Controls.SetChildIndex(this.lblAddressLines, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal
        public TPartnerAddressChangePropagationSelectionWinForm() : base ()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FFormSetupFinished = false;
        }

//        procedure grdPartnersSharingLocation_DoubleClickCell(Sender: System.Object; e: SourceGrid.CellContextEventArgs);
//        procedure grdPartnersSharingLocation_EnterKeyPressed(Sender: System.Object; e: SourceGrid.RowEventArgs);
//        procedure grdPartnersSharingLocation_SpaceKeyPressed(Sender: System.Object; e: SourceGrid.RowEventArgs);
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

//		 procedure TPartnerAddressChangePropagationSelectionWinForm.OnPartnerSharingLocationColumnChanging(sender: System.Object; e: DataColumnChangeEventArgs);
//		 begin  if (this.FFormSetupFinished = true) then  begin    FLogic.CheckedStateForRowChanging(e);  end;end;procedure TPartnerAddressChangePropagationSelectionWinForm.grdPartnersSharingLocation_DoubleClickCell(Sender: System.Object; e:
        // SourceGrid3.CellContextEventArgs);var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow((e.CellContext.Position.Row 1), mChanged);/  messagebox.Show('grdFamilyMembers_DoubleClickCell');end;procedure
        // TPartnerAddressChangePropagationSelectionWinForm.grdPartnersSharingLocation_SpaceKeyPressed(Sender: System.Object; e: SourceGrid3.RowEventArgs);var  mChanged: System.Boolean;begin  grdPartnersSharingLocation.Selection.SelectRow(e.Row, not
        // grdPartnersSharingLocation.Selection.ContainsRow(e.Row));/  messagebox.Show('grdFamilyMembers_SpaceKeyPressed');end;procedure TPartnerAddressChangePropagationSelectionWinForm.grdPartnersSharingLocation_EnterKeyPressed(Sender:
        // System.Object; e:
        // SourceGrid3.RowEventArgs);var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow(e.Row 1, mChanged);/  messagebox.Show('grdFamilyMembers_EnterKeyPressed');end;procedure
        // TPartnerAddressChangePropagationSelectionWinForm.grdFamilyMembers_DataGrid_FocusRowEntered(Sender: System.Object; e: SourceGrid3.RowEventArgs);var  mChanged: System.Boolean;begin  FLogic.ChangeCheckedStateForRow(e.Row 1, mChanged);end;

        private void ChkChangeAllPartners_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            // if chkChangeAllPartners.Checked then
            // begin
            // grdPartnersSharingLocation.Enabled := false;
            // end
            // else
            // begin
            // grdPartnersSharingLocation.Enabled := true;
            // end;
            // FLogic.ChangeCheckedStateForAllRows(this.chkChangeAllPartners.Checked);
            // messagebox.show('chkAddToAllFamilyMembers_CheckedChanged');
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            DataRowView[] SelectedRows;
            int Counter;

            SelectedRows = grdPartnersSharingLocation.SelectedDataRowsAsDataRowView;

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

                // Build list of PartnerKeys of those Partners that are selected
                for (Counter = 0; Counter <= SelectedRows.Length - 1; Counter += 1)
                {
                    FUserAnswer = FUserAnswer +
                                  ((PartnerAddressAggregateTDSChangePromotionParametersRow)SelectedRows[Counter].Row).PartnerKey.ToString() + ';';
                }

                // Remove last ';'
                FUserAnswer = FUserAnswer.Substring(0, FUserAnswer.Length - 1);
            }

            //MessageBox.Show("FUserAnswer: " + FUserAnswer);

            if ((FUserAnswer == "CHANGE-NONE") || (FUserAnswer.StartsWith("CHANGE-SOME")))
            {
                // Check whether user has CREATE right on p_location table
                if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapCREATE, PLocationTable.GetTableDBName()))
                {
                    TMessages.MsgSecurityException(new ESecurityDBTableAccessDeniedException("", "create",
                            PLocationTable.GetTableDBName()), this.GetType());
                    MessageBox.Show(StrSecurityViolationExplanation,
                        StrSecurityViolationExplanationTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            Close();
        }

        private void TPartnerAddressChangePropagationSelectionWinForm_Load(System.Object sender, System.EventArgs e)
        {
            FLogic = new TPartnerAddressChangePropagationSelectionLogic();
            FLogic.CreateColumns(grdPartnersSharingLocation, FPartnerSharingLocationDV.Table);

            // DataBindingrelated stuff
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

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
        private void SetupDataGridDataBinding()
        {
            FPartnerSharingLocationDV.AllowNew = false;
            FPartnerSharingLocationDV.AllowDelete = false;
            FPartnerSharingLocationDV.AllowEdit = false;

            // DataBind the DataGrid
            grdPartnersSharingLocation.DataSource = new DevAge.ComponentModel.BoundDataView(FPartnerSharingLocationDV);

            // Include(FPartnerSharingLocationDT.ColumnChanging, OnPartnerSharingLocationColumnChanging);
            // grdPartnersSharingLocation.Selection.SelectRow(1, true);
        }

        private void SetupDataGridVisualAppearance()
        {
            grdPartnersSharingLocation.AutoSizeCells();
            grdPartnersSharingLocation.Width = 376;             /// it is necessary to reassign the width in case the columns don't take up the maximum width

            // Enable selection of multiple Grid rows with the following statement.
            // Needs different row processing (using the grdPartnersSharingLocation.SelectedDataRows property) in the following Event Handlers: DoubleClickCell, SpaceKeyPressed, EnterKeyPressed!
            grdPartnersSharingLocation.Selection.EnableMultiSelection = true;
        }

        #endregion
    }
}