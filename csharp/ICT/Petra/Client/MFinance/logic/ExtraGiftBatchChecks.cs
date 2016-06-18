//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// Common logic for extra checks to be carried out during saving/posting/submitting in Gift Batch and Recurring Gift Batch
    /// </summary>
    public static class TExtraGiftBatchChecks
    {
        /// <summary>
        ///
        /// </summary>
        public enum GiftBatchAction
        {
            /// <summary>GiftBatch and RecurringGiftBatch</summary>
            SAVING,
            /// <summary>GiftBatch and RecurringGiftBatch</summary>
            NEWBATCH,
            /// <summary>GiftBatch</summary>
            POSTING,
            /// <summary>GiftBatch</summary>
            CANCELLING,
            /// <summary>RecurringGiftBatch</summary>
            SUBMITTING,
            /// <summary>RecurringGiftBatch</summary>
            DELETING
        };


        /// <summary>
        /// Checks the entire gift batch for inactive values and informs the user
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAction"></param>
        /// <param name="AMainDS"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="AIsRecurringGift"></param>
        /// <returns></returns>
        public static bool CheckForInactiveFieldValues(Int32 ALedgerNumber,
            GiftBatchAction AAction,
            GiftBatchTDS AMainDS,
            TFrmPetraEditUtils APetraUtilsObject,
            Boolean AIsRecurringGift = false)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Gift Batch dataset is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AMainDS.AGiftBatch.Count == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The Gift Batch table contains no batches to validate!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //TODO: Need to rewrite...

            bool RetVal = false;

            bool ChangesAtGiftBatchLevel = false;
            bool ChangesAtGiftLevel = false;
            bool ChangesAtGiftDetailLevel = false;

            string IndentifyRecurring = (AIsRecurringGift ? "ARecurring" : "A");
            Type GiftBatchTableType = Type.GetType(IndentifyRecurring + "GiftBatchTable");


            bool WarnOfInactiveValuesOnPosting = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_WARN_OF_INACTIVE_VALUES_ON_POSTING, true);

            if ((AAction == GiftBatchAction.POSTING) && !WarnOfInactiveValuesOnPosting)
            {
                return true;
            }

            if (APetraUtilsObject.HasChanges)
            {
                GiftBatchTDS Changes = new GiftBatchTDS();

                //Batch level
                if (!AIsRecurringGift && (AMainDS.AGiftBatch.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.AGiftBatch.GetChangesTyped());
                    ChangesAtGiftBatchLevel = true;
                }
                else if (AIsRecurringGift && (AMainDS.ARecurringGiftBatch.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.ARecurringGiftBatch.GetChangesTyped());
                    ChangesAtGiftBatchLevel = true;
                }

                //Gift level
                if (!AIsRecurringGift && (AMainDS.AGift.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.AGift.GetChangesTyped());
                    ChangesAtGiftLevel = true;
                }
                else if (AIsRecurringGift && (AMainDS.ARecurringGift.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.ARecurringGift.GetChangesTyped());
                    ChangesAtGiftLevel = true;
                }

                //Detail level
                if (!AIsRecurringGift && (AMainDS.AGiftDetail.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.AGiftDetail.GetChangesTyped());
                    ChangesAtGiftDetailLevel = true;
                }
                else if (AIsRecurringGift && (AMainDS.ARecurringGiftDetail.GetChangesTyped() != null))
                {
                    Changes.Merge(AMainDS.ARecurringGiftDetail.GetChangesTyped());
                    ChangesAtGiftDetailLevel = true;
                }

                //Process changes
                if (ChangesAtGiftBatchLevel)
                {
                    //***Batch level
                    //TODO: Check for inactive Bank Cost Centre
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Bank Account
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Currency Code
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Method of Payment
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }
                }

                if (ChangesAtGiftLevel)
                {
                    //**Gift-level
                    //TODO: Check for inactive Donor
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Method of Payment
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Method of Giving
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }
                }

                if (ChangesAtGiftDetailLevel)
                {
                    //*Detail-level
                    //TODO: Check for inactive Recipient
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Key Ministry
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Cost Centre
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Account
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Tax Account
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive Motivation Group or Detail
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }

                    //TODO: Check for inactive MailingCode
                    if (!AIsRecurringGift)
                    {
                    }
                    else
                    {
                        //Recurring gift batch changes
                    }
                }
            }

            return RetVal;
        }

        private static bool CheckForInactiveKeyMinistries(GiftBatchTDS AChangesDS, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool RetVal = false;

            //int CurrentBatchNumber = 0;

            //Check for inactive KeyMinistries
            DataTable GiftsWithInactiveKeyMinistries;
            bool ModifiedDetails = false;

            if (TRemote.MFinance.Gift.WebConnectors.InactiveKeyMinistriesFoundInBatch(ALedgerNumber, ABatchNumber,
                    out GiftsWithInactiveKeyMinistries, false))
            {
                int numInactiveValues = GiftsWithInactiveKeyMinistries.Rows.Count;

                string messageNonModifiedBatch =
                    String.Format(Catalog.GetString("Gift Batch {0} contains {1} inactive key ministries. Please fix before saving!{2}{2}"),
                        ABatchNumber,
                        numInactiveValues,
                        Environment.NewLine);
                string messageModifiedBatch =
                    String.Format(Catalog.GetString(
                            "Reversal/Adjustment Gift Batch {0} contains {1} inactive key ministries. Do you still want to save?{2}{2}"),
                        ABatchNumber,
                        numInactiveValues,
                        Environment.NewLine);

                string listOfOffendingRows = string.Empty;

                listOfOffendingRows += "Gift      Detail   Recipient          KeyMinistry" + Environment.NewLine;
                listOfOffendingRows += "-------------------------------------------------------------------------------";

                foreach (DataRow dr in GiftsWithInactiveKeyMinistries.Rows)
                {
                    listOfOffendingRows += String.Format("{0}{1:0000}    {2:00}        {3:00000000000}    {4}",
                        Environment.NewLine,
                        dr[0],
                        dr[1],
                        dr[2],
                        dr[3]);

                    bool isModified = Convert.ToBoolean(dr[4]);

                    if (isModified)
                    {
                        ModifiedDetails = true;
                    }
                }

                //TODO: work on how to relate to form
                TFrmExtendedMessageBox extendedMessageBox = null;  // new TFrmExtendedMessageBox(FMyForm);

                if (ModifiedDetails)
                {
                    if (extendedMessageBox.ShowDialog((messageModifiedBatch + listOfOffendingRows),
                            Catalog.GetString("Post Batch"), string.Empty,
                            TFrmExtendedMessageBox.TButtons.embbYesNo,
                            TFrmExtendedMessageBox.TIcon.embiWarning) == TFrmExtendedMessageBox.TResult.embrYes)
                    {
                        //TODO: Check this
                        //APostingAlreadyConfirmed = true;
                    }
                }
                else
                {
                    extendedMessageBox.ShowDialog((messageNonModifiedBatch + listOfOffendingRows),
                        Catalog.GetString("Post Batch Error"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Checks the entire gift batch for inactive values and informs the user
        /// </summary>
        /// <param name="AAction"></param>
        /// <param name="AMainDS"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="AIsRecurringGift"></param>
        /// <returns></returns>
        public static bool CheckForConsistentFieldValues(GiftBatchAction AAction,
            GiftBatchTDS AMainDS,
            TFrmPetraEditUtils APetraUtilsObject,
            Boolean AIsRecurringGift = false)
        {
            bool RetVal = false;

            //TODO: check for field value consistency before saving

            //TODO: Check Batch, Gift and Detail key field consistency
            //  e.g. consecutive numbers, last number values etc
            if (!AIsRecurringGift)
            {
            }
            else
            {
            }

            //TODO: Check Method of Payment between Batch and Gift level
            if (!AIsRecurringGift)
            {
            }
            else
            {
            }

            //TODO: Check for Tax value correctness at Gift Detail level
            if (!AIsRecurringGift)
            {
            }

            //Nothing to do for recurring

            return RetVal;
        }

        /// <summary>
        /// Looks for gifts where the recipient is an ExWorker and asks the user if they want to continue.
        /// (Make sure GetDataFromControls is called before this method so that AMainDS is up-to-date.)
        /// </summary>
        /// <param name="AAction">Why this method is being called</param>
        /// <param name="AMainDS"></param>
        /// <param name="APetraUtilsObject"></param>
        /// <param name="APostingGiftDetails">Only used when being called in order to carry out a batch posting</param>
        /// <returns>Returns true if saving/posting can continue</returns>
        public static bool CanContinueWithAnyExWorkers(GiftBatchAction AAction,
            GiftBatchTDS AMainDS,
            TFrmPetraEditUtils APetraUtilsObject,
            DataTable APostingGiftDetails = null)
        {
            DataTable ExWorkers = null;
            string Msg = string.Empty;
            int BatchNumber = -1;
            int ExWorkerGifts = 0;

            string ExWorkerSpecialType = TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_EXWORKERSPECIALTYPE, "EX-WORKER");

            // first check for Ex-Workers in the batch that is being posted/submitted (if a batch is being posted/submitted)
            if ((APostingGiftDetails != null) && (APostingGiftDetails.Rows.Count > 0))
            {
                ExWorkers = TRemote.MFinance.Gift.WebConnectors.FindGiftRecipientExWorker(APostingGiftDetails, BatchNumber);
                ExWorkerGifts += ExWorkers.Rows.Count;

                Msg = GetExWorkersString(AAction, ExWorkerSpecialType, ExWorkers);

                if (ExWorkers.Rows.Count > 0)
                {
                    BatchNumber = (int)APostingGiftDetails.Rows[0][GiftBatchTDSAGiftDetailTable.GetBatchNumberDBName()];
                }
            }

            // check for Ex-Workers in all added and modified data
            if (APetraUtilsObject.HasChanges)
            {
                DataTable Changes = new DataTable();

                if (AMainDS.AGiftDetail.GetChangesTyped() != null)
                {
                    Changes.Merge(AMainDS.AGiftDetail.GetChangesTyped());
                }
                else if (AMainDS.ARecurringGiftDetail.GetChangesTyped() != null)
                {
                    Changes.Merge(AMainDS.ARecurringGiftDetail.GetChangesTyped());
                }

                if ((Changes != null) && (Changes.Rows.Count > 0))
                {
                    ExWorkers = TRemote.MFinance.Gift.WebConnectors.FindGiftRecipientExWorker(Changes, BatchNumber);
                    ExWorkerGifts += ExWorkers.Rows.Count;

                    Msg += GetExWorkersString(null, ExWorkerSpecialType, ExWorkers);
                }
            }

            // alert the user to any recipients who are Ex-Workers
            if (Msg != string.Empty)
            {
                if (AAction == GiftBatchAction.SAVING)
                {
                    Msg += Environment.NewLine + Environment.NewLine;
                    Msg += Catalog.GetString("Do you want to continue with saving anyway?");
                }
                else
                {
                    Msg += Catalog.GetString("Changed gift(s) will need to be saved before ");

                    if (AAction == GiftBatchAction.NEWBATCH)
                    {
                        Msg += Catalog.GetString("a new batch can be created.");
                    }
                    else //POSTING, CANCELLING, SUBMITTING, DELETING
                    {
                        Msg += Catalog.GetString("this batch continues with " + AAction.ToString().ToLower());
                    }

                    Msg += Environment.NewLine + Environment.NewLine;
                    Msg += Catalog.GetString("Do you want to continue?");
                }

                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(APetraUtilsObject.GetForm());

                if (extendedMessageBox.ShowDialog(Msg,
                        Catalog.GetString("Ex-Workers Found"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbYesNo,
                        TFrmExtendedMessageBox.TIcon.embiWarning)
                    == TFrmExtendedMessageBox.TResult.embrNo)
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetExWorkersString(GiftBatchAction? AAction, string AExWorkerSpecialType, DataTable AExWorkers)
        {
            string ReturnValue = string.Empty;

            if (AExWorkers.Rows.Count == 0)
            {
                return ReturnValue;
            }

            DataView dv = AExWorkers.DefaultView;
            dv.Sort = GiftBatchTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " ASC";
            DataTable sortedDT = dv.ToTable();

            if ((AAction == GiftBatchAction.POSTING) || (AAction == GiftBatchAction.SUBMITTING))
            {
                if (AExWorkers.Rows.Count == 1)
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The gift listed below has a recipient who has a Special Type beginning with {0}:"), AExWorkerSpecialType);
                }
                else
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The gifts listed below have recipients who have a Special Type beginning with {0}:"),
                        AExWorkerSpecialType);
                }
            }
            else
            {
                if (AExWorkers.Rows.Count == 1)
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The unsaved gift listed below is for a recipient who has Special Type beginning with {0}:"), AExWorkerSpecialType);
                }
                else
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The unsaved gifts listed below are for recipients who have Special Type beginning with {0}:"), AExWorkerSpecialType);
                }
            }

            ReturnValue += "\n\n";

            foreach (DataRow Row in sortedDT.Rows)
            {
                ReturnValue += Catalog.GetString("Batch: ") + Row[GiftBatchTDSAGiftDetailTable.GetBatchNumberDBName()] + "; " +
                               Catalog.GetString("Gift: ") + Row[GiftBatchTDSAGiftDetailTable.GetGiftTransactionNumberDBName()] + "; " +
                               Catalog.GetString("Recipient: ") + Row[GiftBatchTDSAGiftDetailTable.GetRecipientDescriptionDBName()] + " (" +
                               Row[GiftBatchTDSAGiftDetailTable.GetRecipientKeyDBName()] + ")\n";
            }

            return ReturnValue += "\n";
        }

        /// <summary>
        /// Looks for gifts where the donor is anoymous but the gift is not marked as confidential and asks the user if they want to continue.
        /// (Make sure GetDataFromControls is called before this method so that AMainDS is up-to-date.)
        /// </summary>
        /// <param name="AMainDS"></param>
        public static bool CanContinueWithAnyAnonymousDonors(GiftBatchTDS AMainDS)
        {
            GiftBatchTDSAGiftDetailTable UnConfidentialGiftsWithAnonymousDonors = new GiftBatchTDSAGiftDetailTable();

            foreach (GiftBatchTDSAGiftDetailRow Row in AMainDS.AGiftDetail.Rows)
            {
                if (!Row.ConfidentialGiftFlag)
                {
                    PPartnerRow PartnerRow = (PPartnerRow)AMainDS.DonorPartners.Rows.Find(Row.DonorKey);

                    if ((PartnerRow != null) && PartnerRow.AnonymousDonor)
                    {
                        UnConfidentialGiftsWithAnonymousDonors.Rows.Add((object[])Row.ItemArray.Clone());
                    }
                }
            }

            if (UnConfidentialGiftsWithAnonymousDonors.Rows.Count > 0)
            {
                string Message = string.Empty;

                DataView dv = UnConfidentialGiftsWithAnonymousDonors.DefaultView;
                dv.Sort = GiftBatchTDSAGiftDetailTable.GetGiftTransactionNumberDBName() + " ASC";
                DataTable sortedDT = dv.ToTable();

                if (UnConfidentialGiftsWithAnonymousDonors.Rows.Count == 1)
                {
                    Message = Catalog.GetString(
                        "The gift listed below in this batch is not marked as confidential but the donor has asked to remain anonymous.");
                }
                else
                {
                    Message = Catalog.GetString(
                        "The gifts listed below in this batch are not marked as confidential but the donors have asked to remain anonymous.");
                }

                Message += "\n\n";

                foreach (DataRow UnConfidentialGifts in sortedDT.Rows)
                {
                    Message += Catalog.GetString("Batch: ") + UnConfidentialGifts[GiftBatchTDSAGiftDetailTable.GetBatchNumberDBName()] + "; " +
                               Catalog.GetString("Gift: ") + UnConfidentialGifts[GiftBatchTDSAGiftDetailTable.GetGiftTransactionNumberDBName()] +
                               "; " +
                               Catalog.GetString("Donor: ") + UnConfidentialGifts[GiftBatchTDSAGiftDetailTable.GetDonorNameDBName()] + " (" +
                               UnConfidentialGifts[GiftBatchTDSAGiftDetailTable.GetDonorKeyDBName()] + ")\n";
                }

                Message += "\n" + Catalog.GetString("Do you want to continue with posting anyway?");

                if (MessageBox.Show(
                        Message, Catalog.GetString("Anonymous Donor Warning"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }
    }
}