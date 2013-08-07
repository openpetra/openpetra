//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       joachimm, timop
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MPartner.Partner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_FinanceDetails
    {
        /// <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>used for passing through the Clientside Proxy for the UIConnector</summary>
        public IPartnerUIConnectorsPartnerEdit PartnerEditUIConnector
        {
            get
            {
                return FPartnerEditUIConnector;
            }

            set
            {
                FPartnerEditUIConnector = value;
            }
        }

        /// <summary>an event that will reload the grid after saving</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>
        /// load the data for this control
        /// </summary>
        public void PreInitUserControl(PartnerEditTDS AMainDS)
        {
            FMainDS = AMainDS;

            LoadDataOnDemand();

            if (grdDetails.Rows.Count <= 1)
            {
                btnSetMainAccount.Enabled = false;
                pnlDetails.Visible = false;
            }

            // if partner is of class FAMILY or class UNIT, enable grpRecipientGiftReceipting
            grpRecipientGiftReceipting.Enabled = (FMainDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY
                                                  || FMainDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewPBankingDetails();
        }

        private void NewRowManual(ref PartnerEditTDSPBankingDetailsRow ARow)
        {
            // TODO provide a dialog like Petra 2.x to enable the user to reuse an existing account
            ARow.BankingDetailsKey = (FMainDS.PBankingDetails.Rows.Count + 1) * -1;
            ARow.BankingType = MPartnerConstants.BANKINGTYPE_BANKACCOUNT;
            ARow.BankKey = 0;
            ARow.MainAccount = (FMainDS.PBankingDetails.Rows.Count == 0);

            PPartnerBankingDetailsRow partnerBankingDetails = FMainDS.PPartnerBankingDetails.NewRowTyped();
            partnerBankingDetails.BankingDetailsKey = ARow.BankingDetailsKey;
            partnerBankingDetails.PartnerKey = FMainDS.PPartner[0].PartnerKey;
            FMainDS.PPartnerBankingDetails.Rows.Add(partnerBankingDetails);

            btnSetMainAccount.Enabled = true;
        }

        private bool PreDeleteManual(PartnerEditTDSPBankingDetailsRow ARowToDelete, ref String ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2})",
                Environment.NewLine,
                lblAccountName.Text,
                txtAccountName.Text);
            return true;
        }

        private bool DeleteRowManual(PartnerEditTDSPBankingDetailsRow ARowToDelete, ref String ACompletionMessage)
        {
            ACompletionMessage = String.Empty;

            // TODO what if several people are using the same bank account?
            FMainDS.PPartnerBankingDetails.DefaultView.Sort = PPartnerBankingDetailsTable.GetBankingDetailsKeyDBName();
            FMainDS.PPartnerBankingDetails.DefaultView.FindRows(ARowToDelete.BankingDetailsKey)[0].Row.Delete();

            ARowToDelete.Delete();

            return true;
        }

        private void PostDeleteManual(PartnerEditTDSPBankingDetailsRow ARowToDelete,
            Boolean AAllowDeletion,
            Boolean ADeletionPerformed,
            String ACompletionMessage)
        {
            if (grdDetails.Rows.Count <= 1)
            {
                // disable buttons if no record in grid (first row for headings)
                btnSetMainAccount.Enabled = false;
            }

            if (ADeletionPerformed)
            {
                DoRecalculateScreenParts();
            }
        }

        private void DoRecalculateScreenParts()
        {
            OnRecalculateScreenParts(new TRecalculateScreenPartsEventArgs() {
                    ScreenPart = TScreenPartEnum.spCounters
                });
        }

        private void ShowDetailsManual(PBankingDetailsRow ARow)
        {
            if (ARow != null)
            {
                btnDelete.Enabled = true;
                pnlDetails.Visible = true;
            }

            // In theory, the next Method call could be done in Methods NewRowManual; however, NewRowManual runs before
            // the Row is actually added and this would result in the Count to be one too less, so we do the Method call here, short
            // of a non-existing 'AfterNewRowManual' Method....
            DoRecalculateScreenParts();
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        /// Loads PBankingDetails Data from Petra Server into FMainDS, if not already loaded.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        private Boolean LoadDataOnDemand()
        {
            // Make sure that Typed DataTables are already there at Client side
            if (FMainDS.PBankingDetails == null)
            {
                FMainDS.Tables.Add(new PartnerEditTDSPBankingDetailsTable());
                FMainDS.Tables.Add(new PPartnerBankingDetailsTable());
                FMainDS.InitVars();
            }

            if (TClientSettings.DelayedDataLoading
                && ((FMainDS.PBankingDetails == null) || (FMainDS.PBankingDetails.Rows.Count == 0)))
            {
                FMainDS.Merge(FPartnerEditUIConnector.GetBankingDetails());

                // Make DataRows unchanged
                if (FMainDS.PBankingDetails.Rows.Count > 0)
                {
                    if (FMainDS.PBankingDetails.Rows[0].RowState != DataRowState.Added)
                    {
                        FMainDS.PBankingDetails.AcceptChanges();
                    }
                }
            }

            return FMainDS.PBankingDetails.Rows.Count != 0;
        }

        /// <summary>
        /// Performs necessary actions after the Merging of rows that were changed on
        /// the Server side into the Client-side DataSet.
        /// New rows with negative id numbers in the primary key have been removed, and replaced with the saved rows.
        /// </summary>
        public void RefreshRecordsAfterMerge()
        {
            FPreviouslySelectedDetailRow = null;
            grdDetails.Selection.ResetSelection(false);
            ShowData();
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        private void ValidateDataDetailsManual(PBankingDetailsRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            // check if bankkey refers to valid BANK partner
            TSharedPartnerValidation_Partner.ValidateBankingDetails(this,
                ARow,
                FMainDS.PBankingDetails,
                ref VerificationResultCollection,
                FValidationControlsDict);

            // GetDataFromControls for PPartner table
            FMainDS.PPartner[0].ReceiptLetterFrequency = cmbReceiptLetterFrequency.GetSelectedString();
            FMainDS.PPartner[0].ReceiptEachGift = chkReceiptEachGift.Checked;
            FMainDS.PPartner[0].AnonymousDonor = chkAnonymousDonor.Checked;
            FMainDS.PPartner[0].EmailGiftStatement = chkEmailGiftStatement.Checked;
            FMainDS.PPartner[0].FinanceComment = txtFinanceComment.Text;
        }

        // set the main account flag, remove that flag from the other accounts (p_banking_details_usage)
        private void SetMainAccount(System.Object Sender, EventArgs e)
        {
            foreach (PartnerEditTDSPBankingDetailsRow r in FMainDS.PBankingDetails.Rows)
            {
                if ((r.RowState != DataRowState.Deleted) && (r != FPreviouslySelectedDetailRow) && r.MainAccount)
                {
                    r.MainAccount = false;
                    FPetraUtilsObject.SetChangedFlag();
                }
            }

            if (!FPreviouslySelectedDetailRow.MainAccount)
            {
                FPreviouslySelectedDetailRow.MainAccount = true;
                FPetraUtilsObject.SetChangedFlag();
            }

            // MainAccount PBankingDetailsUsage is processed on the server side!!!
        }
    }
}