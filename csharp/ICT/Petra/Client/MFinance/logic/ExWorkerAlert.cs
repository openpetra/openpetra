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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// Common logic for Ex-Worker check in Gift Batch and Recurring Gift Batch
    /// </summary>
    public static class TExWorkerAlert
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

            string ExWorkerSpecialType = TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_EXWORKERSPECIALTYPE, "EX-WORKER");

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
                    Msg += Catalog.GetString("Do you want to continue with saving anyway?");
                }
                else
                {
                    // singular
                    if (ExWorkerGifts == 1)
                    {
                        if (AAction == GiftBatchAction.POSTING)
                        {
                            Msg += Catalog.GetString("This gift will need to be saved before this batch can be posted.");
                        }
                        else if (AAction == GiftBatchAction.NEWBATCH)
                        {
                            Msg += Catalog.GetString("This gift will need to be saved before a new batch can be created.");
                        }
                        else if (AAction == GiftBatchAction.CANCELLING)
                        {
                            Msg += Catalog.GetString("This gift will need to be saved before this batch can be cancelled.");
                        }
                        else if (AAction == GiftBatchAction.SUBMITTING)
                        {
                            Msg += Catalog.GetString("This gift will need to be saved before this batch can be submitted.");
                        }
                        else if (AAction == GiftBatchAction.DELETING)
                        {
                            Msg += Catalog.GetString("This gift will need to be saved before this batch can be deleted.");
                        }
                    }
                    // plural
                    else
                    {
                        if (AAction == GiftBatchAction.POSTING)
                        {
                            Msg += Catalog.GetString("These gifts will need to be saved before this batch can be posted.");
                        }
                        else if (AAction == GiftBatchAction.NEWBATCH)
                        {
                            Msg += Catalog.GetString("These gifts will need to be saved before a new batch can be created.");
                        }
                        else if (AAction == GiftBatchAction.CANCELLING)
                        {
                            Msg += Catalog.GetString("These gifts will need to be saved before this batch can be cancelled.");
                        }
                        else if (AAction == GiftBatchAction.SUBMITTING)
                        {
                            Msg += Catalog.GetString("These gifts will need to be saved before this batch can be submitted.");
                        }
                        else if (AAction == GiftBatchAction.DELETING)
                        {
                            Msg += Catalog.GetString("These gifts will need to be saved before this batch can be deleted.");
                        }
                    }

                    Msg += " " + Catalog.GetString("Do you want to continue?");
                }

                if (MessageBox.Show(
                        Msg, string.Format(Catalog.GetString("{0} Warning"), ExWorkerSpecialType), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.No)
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

            if ((AAction == GiftBatchAction.POSTING) || (AAction == GiftBatchAction.SUBMITTING))
            {
                if (AExWorkers.Rows.Count == 1)
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The gift listed below in this batch is for a recipient who has Special Type beginning with {0}:"), AExWorkerSpecialType);
                }
                else
                {
                    ReturnValue = string.Format(Catalog.GetString(
                            "The gifts listed below in this batch are for recipients who have Special Type beginning with {0}:"),
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

            foreach (DataRow Row in AExWorkers.Rows)
            {
                ReturnValue += Catalog.GetString("Batch: ") + Row[GiftBatchTDSAGiftDetailTable.GetBatchNumberDBName()] + "; " +
                               Catalog.GetString("Gift: ") + Row[GiftBatchTDSAGiftDetailTable.GetGiftTransactionNumberDBName()] + "; " +
                               Catalog.GetString("Recipient: ") + Row[GiftBatchTDSAGiftDetailTable.GetRecipientDescriptionDBName()] + " (" +
                               Row[GiftBatchTDSAGiftDetailTable.GetRecipientKeyDBName()] + ")\n";
            }

            return ReturnValue += "\n";
        }
    }
}