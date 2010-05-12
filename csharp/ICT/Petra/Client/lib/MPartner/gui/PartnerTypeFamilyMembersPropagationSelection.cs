//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, markusm
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
using Ict.Petra.Client.CommonForms;
using SourceGrid;
using Ict.Common.Controls;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// PartnerType Add/Remove Family Members Propagation Selection Dialog.
    /// Called from Partner Edit screen/Partner Types tab (UC_PartnerTypes).
    public class TPartnerTypeFamilyMembersPropagationSelectionWinForm : TFrmPetraDialog
    {
        public const String StrWindowTitleGeneral = "Change Partner Type:  ";
        public const String StrWindowTitleAdd = "Add to Family Members";
        public const String StrWindowTitleRemove = "Remove from Family Members";
        public const String StrSelectFamilyMembersInstructionsGeneral = "Tick the family members that ";
        public const String StrSelectFamilyMembersInstructionsAdd = " should be added to:";
        public const String StrSelectFamilyMembersInstructionsDelete = " should be removed from:";
        public const String StrAllFamilyMembersAdd = "Add to &all family members";
        public const String StrAllFamilyMembersDelete = "Remove from &all family members";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblSitesAvailable;
        private TsgrdDataGrid grdFamilyMembers;
        private System.Windows.Forms.CheckBox chkAddToAllFamilyMembers;
        private System.Windows.Forms.Label lblExplanation;
        private TPartnerTypeFamilyMembersPropagationSelectionLogic FLogic;
        private System.Data.DataView FFamilyMembersDV;

        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;
        private PartnerEditTDSFamilyMembersTable FFamilyMembersDT;
        private String FPartnerTypeCode;
        private String FAction;
        private Int64 FFamilyPartnerKey;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerTypeFamilyMembersPropagationSelectionWinForm));
            this.lblSitesAvailable = new System.Windows.Forms.Label();
            this.grdFamilyMembers = new Ict.Common.Controls.TsgrdDataGrid();
            this.chkAddToAllFamilyMembers = new System.Windows.Forms.CheckBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnOK.Location = new System.Drawing.Point(440, 8);
            this.btnOK.Name = "btnOK";
todo: move statusbar things to constructor
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept selection and continue.");
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 256);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(604, 34);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCancel.Location = new System.Drawing.Point(522, 8);
            this.btnCancel.Name = "btnCancel";
            this.sbtForm.SetStatusBarText(this.btnCancel, "Ignore selection; don't ma" + "ke this change to any family member.");

            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnHelp.Location = new System.Drawing.Point(8, 10);
            this.btnHelp.Name = "btnHelp";
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");
            this.btnHelp.TabIndex = 996;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 290);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(604, 22);

            //
            // stpInfo
            //
            this.stpInfo.Width = 604;

            //
            // lblSitesAvailable
            //
            this.lblSitesAvailable.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSitesAvailable.Location = new System.Drawing.Point(14, 32);
            this.lblSitesAvailable.Name = "lblSitesAvailable";
            this.lblSitesAvailable.Size = new System.Drawing.Size(582, 16);
            this.lblSitesAvailable.TabIndex = 0;
            this.lblSitesAvailable.Text = "Tick the family members that XXX should be" + " added to:  (VARIABLE TEXT)";

            //
            // grdFamilyMembers
            //
            this.grdFamilyMembers.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(230, 230, 230);
            this.grdFamilyMembers.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFamilyMembers.BackColor = System.Drawing.SystemColors.Control;
            this.grdFamilyMembers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdFamilyMembers.DeleteQuestionMessage = "You have chosen to delete " +
                                                          "this record.'#13#10#13#10'Dou you really want to delete it?";
            this.grdFamilyMembers.FixedRows = 1;
            this.grdFamilyMembers.Location = new System.Drawing.Point(14, 50);
            this.grdFamilyMembers.MinimumHeight = 19;
            this.grdFamilyMembers.Name = "grdFamilyMembers";
            this.grdFamilyMembers.Size = new System.Drawing.Size(582, 176);
            this.grdFamilyMembers.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.sbtForm.SetStatusBarText(this.grdFamilyMembers, "Place or remove a ch" + "eck mark in the first column.");
            this.grdFamilyMembers.TabIndex = 0;
            this.grdFamilyMembers.TabStop = true;
            this.grdFamilyMembers.Validating += new System.ComponentModel.CancelEventHandler(this.GrdFamilyMembers_Validating);
            this.grdFamilyMembers.DoubleClickCell += new Ict.Common.Controls.TDoubleClickCellEventHandler(this.GrdFamilyMembers_DoubleClickCell);
            this.grdFamilyMembers.SpaceKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.GrdFamilyMembers_SpaceKeyPressed);
            this.grdFamilyMembers.EnterKeyPressed += new Ict.Common.Controls.TKeyPressedEventHandler(this.GrdFamilyMembers_EnterKeyPressed);

            //
            // chkAddToAllFamilyMembers
            //
            this.chkAddToAllFamilyMembers.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAddToAllFamilyMembers.Location = new System.Drawing.Point(14, 226);
            this.chkAddToAllFamilyMembers.Name = "chkAddToAllFamilyMembers";
            this.chkAddToAllFamilyMembers.Size = new System.Drawing.Size(582, 24);
            this.sbtForm.SetStatusBarText(this.chkAddToAllFamilyMembers, "Tick this to" + " make the change to all family members.");
            this.chkAddToAllFamilyMembers.TabIndex = 1;
            this.chkAddToAllFamilyMembers.Text = "Add to &all family members  (VARIAB" + "LE TEXT)";
            this.chkAddToAllFamilyMembers.CheckedChanged += new System.EventHandler(this.ChkAddToAllFamilyMembers_CheckedChanged);

            //
            // lblExplanation
            //
            this.lblExplanation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExplanation.Location = new System.Drawing.Point(14, 10);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(582, 16);
            this.lblExplanation.TabIndex = 998;
            this.lblExplanation.Text = "You can make this change to family members as" + " well.";

            //
            // TPartnerTypeFamilyMembersPropagationSelectionWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(604, 312);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.chkAddToAllFamilyMembers);
            this.Controls.Add(this.grdFamilyMembers);
            this.Controls.Add(this.lblSitesAvailable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerTypeFamilyMembersPropagationSelectionWinForm";
            this.Text = "Change Special Type: Add to Family Members  (VARIABLE TEXT)";
            this.Load += new System.EventHandler(this.PartnerTypeFamilyMembersPropagationSelectionWinForm_Load);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.lblSitesAvailable, 0);
            this.Controls.SetChildIndex(this.grdFamilyMembers, 0);
            this.Controls.SetChildIndex(this.chkAddToAllFamilyMembers, 0);
            this.Controls.SetChildIndex(this.lblExplanation, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Creation and Disposal
        public TPartnerTypeFamilyMembersPropagationSelectionWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            FFormSetupFinished = false;
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
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
        /// procedure OnPartnerDataColumnChanging(sender: System.Object; e: DataColumnChangeEventArgs);
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(Int64 AFamilyPartnerKey,
            IPartnerUIConnectorsPartnerEdit APartnerEditUIConnector,
            String APartnerTypeCode,
            String AAction)
        {
            FFamilyPartnerKey = AFamilyPartnerKey;
            FPartnerEditUIConnector = APartnerEditUIConnector;
            FPartnerTypeCode = APartnerTypeCode;
            FAction = AAction;
            ApplyText(AAction, APartnerTypeCode);
        }

        public Boolean GetReturnedParameters(out PartnerEditTDSPartnerTypeChangeFamilyMembersPromotionTable AFamilyMembersPromotionTable)
        {
            Boolean ReturnValue;

            AFamilyMembersPromotionTable = null;

            if (FFormSetupFinished)
            {
                AFamilyMembersPromotionTable = FLogic.GetResultTable();
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
        private void OnFamilyMembersColumnChanging(System.Object sender, DataColumnChangeEventArgs e)
        {
            if (this.FFormSetupFinished == true)
            {
                FLogic.CheckedStateForRowChanging(ref e);
            }
        }

        /// <summary>
        /// procedure grdFamilyMembers_Click(Sender: GridVirtual; e: EventArgs);
        /// </summary>
        /// <returns>void</returns>
        private void GrdFamilyMembers_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            bool mChanged;

            FLogic.ChangeCheckedStateForRow((e.CellContext.Position.Row - 1), out mChanged);

            // messagebox.Show('grdFamilyMembers_DoubleClickCell');
        }

        private void GrdFamilyMembers_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            bool mChanged;

            FLogic.ChangeCheckedStateForRow(e.Row - 1, out mChanged);

            // messagebox.Show('grdFamilyMembers_SpaceKeyPressed');
        }

        private void GrdFamilyMembers_Validating(System.Object Sender, CancelEventArgs e)
        {
            // Messagebox.show('grdFamilyMembers_Validating');
        }

        private void GrdFamilyMembers_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            bool mChanged;

            FLogic.ChangeCheckedStateForRow(e.Row - 1, out mChanged);

            // messagebox.Show('grdFamilyMembers_EnterKeyPressed');
        }

        private void ChkAddToAllFamilyMembers_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkAddToAllFamilyMembers.Checked)
            {
                grdFamilyMembers.Enabled = false;
            }
            else
            {
                grdFamilyMembers.Enabled = true;
            }

            FLogic.ChangeCheckedStateForAllRows(this.chkAddToAllFamilyMembers.Checked);

            // messagebox.show('chkAddToAllFamilyMembers_CheckedChanged');
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void PartnerTypeFamilyMembersPropagationSelectionWinForm_Load(System.Object sender, System.EventArgs e)
        {
            FLogic = new TPartnerTypeFamilyMembersPropagationSelectionLogic();
            FFamilyMembersDT = FPartnerEditUIConnector.GetDataFamilyMembers(FFamilyPartnerKey, FPartnerTypeCode);
            FLogic.CreateColumns(grdFamilyMembers, FFamilyMembersDT, FAction);

            // DataBindingrelated stuff
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

            // messagebox.show('Rows: ' + this.grdFamilyMembers.Rows.Count.ToString);
            // DataSetup and the rest of the initialisation bit
            FLogic.PartnerEditUIConnector = FPartnerEditUIConnector;
            FLogic.FamilyMembersDV = FFamilyMembersDV;
            FLogic.InitialisePartnerTypeFamilyMembers(this.FFamilyMembersDT);
            FLogic.TypeCode = this.FPartnerTypeCode;
            FFormSetupFinished = true;
        }

        #endregion

        #region Helper functions
        protected void ApplyText(String AAction, String APartnerTypeCode)
        {
            String mLabelString;
            String mTitleString;
            String mCheckString;

            if (AAction.Equals("ADD"))
            {
                mLabelString = StrSelectFamilyMembersInstructionsGeneral + "'" + APartnerTypeCode + "'" + StrSelectFamilyMembersInstructionsAdd;
                mTitleString = StrWindowTitleGeneral + StrWindowTitleAdd;
                mCheckString = StrAllFamilyMembersAdd;
            }
            else
            {
                mLabelString = StrSelectFamilyMembersInstructionsGeneral + "'" + APartnerTypeCode + "'" + StrSelectFamilyMembersInstructionsDelete;
                mTitleString = StrWindowTitleGeneral + StrWindowTitleRemove;
                mCheckString = StrAllFamilyMembersDelete;
            }

            this.lblSitesAvailable.Text = mLabelString;
            this.Text = mTitleString;
            this.chkAddToAllFamilyMembers.Text = mCheckString;
        }

        #endregion

        #region Setup SourceDataGrid
        private void SetupDataGridDataBinding()
        {
            FFamilyMembersDV = FFamilyMembersDT.DefaultView;
            FFamilyMembersDV.AllowNew = false;
            FFamilyMembersDV.AllowDelete = false;
            FFamilyMembersDV.AllowEdit = true;
            FFamilyMembersDV.Sort = PartnerEditTDSFamilyMembersTable.GetOldOmssFamilyIdDBName() + " ASC";

            // DataBind the DataGrid
            grdFamilyMembers.DataSource = new DevAge.ComponentModel.BoundDataView(FFamilyMembersDV);
            FFamilyMembersDT.ColumnChanging += new DataColumnChangeEventHandler(this.OnFamilyMembersColumnChanging);
            grdFamilyMembers.Selection.SelectRow(1, true);
        }

        private void SetupDataGridVisualAppearance()
        {
            grdFamilyMembers.AutoSizeCells();
            grdFamilyMembers.Width = 582;             /// it is necessary to reassign the width in case the columns don't take up the maximum width

            /* TODO 2 oMarkusM cGrid Selection : Enable selection of multiple Grid Rows with the following statement. Needs different Row processing (using the grdFamilyMembers.SelectedDataRows property) in the following Event Handlers:
             *DoubleClickCell, SpaceKeyPressed, EnterKeyPressed! */

            // grdFamilyMembers.Selection.EnableMultiSelection := true;
        }

        #endregion
    }
}