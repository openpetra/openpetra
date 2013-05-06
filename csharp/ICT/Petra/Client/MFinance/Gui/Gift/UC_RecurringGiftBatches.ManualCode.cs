//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2012 by OM International
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_RecurringGiftBatches
    {
        private Int32 FLedgerNumber;
//        private Int32 FSelectedBatchNumber;

        /// <summary>
        /// Stores the current batch's method of payment
        /// </summary>//
        public string FSelectedBatchMethodOfPayment = String.Empty;

        /// <summary>
        /// Flags whether all the gift batch rows for this form have finished loading
        /// </summary>
        public bool FBatchLoaded = false;

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;

            ((TFrmRecurringGiftBatch)ParentForm).ClearCurrentSelections();

            // TODO: more criteria: state of batch, period, etc
            FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadARecurringGiftBatch(ALedgerNumber));

            // Load Motivation detail in this central place; it will be used by UC_GiftTransactions
            AMotivationDetailTable motivationDetail = (AMotivationDetailTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.MotivationList,
                FLedgerNumber);
            motivationDetail.TableName = FMainDS.AMotivationDetail.TableName;
            FMainDS.Merge(motivationDetail);

            FMainDS.AcceptChanges();

            FMainDS.ARecurringGiftBatch.DefaultView.Sort = String.Format("{0}, {1} DESC",
                AGiftBatchTable.GetLedgerNumberDBName(),
                AGiftBatchTable.GetBatchNumberDBName()
                );

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, ActiveOnly, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, ActiveOnly, true);
            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGiftBatch) this.ParentForm).EnableTransactionsTab();
            }
            else
            {
                ShowDetails(null);
                ((TFrmRecurringGiftBatch) this.ParentForm).DisableTransactionsTab();
            }

            ShowData();

            FBatchLoaded = true;

            ShowDetails(GetCurrentRecurringBatchRow());
        }

        /// <summary>
        /// get the row of the current batch
        /// </summary>
        /// <returns>AGiftBatchRow</returns>
        public ARecurringGiftBatchRow GetCurrentRecurringBatchRow()
        {
            if (FBatchLoaded && (FPreviouslySelectedDetailRow != null))
            {
                return (ARecurringGiftBatchRow)FMainDS.ARecurringGiftBatch.Rows.Find(new object[] { FLedgerNumber,
                                                                                                    FPreviouslySelectedDetailRow.BatchNumber });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// </summary>
        public void RefreshAll()
        {
            try
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                LoadBatches(FLedgerNumber);
            }
            finally
            {
                FPetraUtilsObject.EnableDataChangedEvent();
            }
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// </summary>
        public void AdjustAfterResizing()
        {
            // TODO Adjustment of SourceGrid's column widhts needs to be done like in Petra 2.3 ('SetupDataGridVisualAppearance' Methods)
        }

        /// <summary>
        /// show ledger number
        /// </summary>
        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
        }

        private void ShowDetailsManual(ARecurringGiftBatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            FLedgerNumber = ARow.LedgerNumber;
//            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode = false;

            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactionsTab();

            UpdateChangeableStatus();

//            FPetraUtilsObject.DetailProtectedMode = false;
//            ((TFrmRecurringGiftBatch)ParentForm).EnableTransactionsTab();
//            UpdateChangeableStatus();
//            FPetraUtilsObject.DetailProtectedMode = false;
//            ((TFrmRecurringGiftBatch)ParentForm).LoadTransactions(
//                ARow.LedgerNumber,
//                ARow.BatchNumber);

            // FSelectedBatchNumber = ARow.BatchNumber;
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmRecurringGiftBatch)ParentForm).SelectTab(TFrmRecurringGiftBatch.eGiftTabs.Transactions, false);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges())
            {
                this.CreateNewARecurringGiftBatch();
                txtDetailBatchDescription.Focus();

                //Save the new record
                ((TFrmRecurringGiftBatch) this.ParentForm).SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringGiftBatchRow ARowToDelete, ref string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            int batchNumber = ARowToDelete.BatchNumber;

            try
            {
                ACompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} deleted successfully."),
                    batchNumber);


                //Load the gift details first before deleting them
                FMainDS.ARecurringGiftDetail.DefaultView.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringGiftDetailTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringGiftDetailTable.GetBatchNumberDBName(),
                    batchNumber);

                // only load from server if there are no transactions loaded yet for this batch
                // otherwise we would overwrite transactions that have already been modified
                if (FMainDS.ARecurringGiftDetail.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadRecurringTransactions(FLedgerNumber, batchNumber));
                }

                // Delete the associated recurring gift detail rows.
                DataView viewGiftDetail = new DataView(FMainDS.ARecurringGiftDetail);
                viewGiftDetail.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringGiftTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    batchNumber);

                foreach (DataRowView row in viewGiftDetail)
                {
                    row.Delete();
                }

                // Delete the associated recurring gift rows.
                DataView viewGift = new DataView(FMainDS.ARecurringGift);
                viewGift.RowFilter = String.Format("{0} = {1} AND {2} = {3}",
                    ARecurringGiftTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ARecurringGiftTable.GetBatchNumberDBName(),
                    batchNumber);

                foreach (DataRowView row in viewGift)
                {
                    row.Delete();
                }

                // Delete the recurring batch row.
                ARowToDelete.Delete();

                FPreviouslySelectedDetailRow = null;

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return deletionSuccessful;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ARecurringGiftBatchRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            /*Code to execute after the delete has occurred*/
            if (ADeletionPerformed && (ACompletionMessage.Length > 0))
            {
                MessageBox.Show(ACompletionMessage,
                    "Deletion Completed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (!pnlDetails.Enabled)         //set by FocusedRowChanged if grdDetails.Rows.Count < 2
                {
                    ShowDetails(null);
                }
            }
            else if (!AAllowDeletion)
            {
                //message to user
            }
            else if (!ADeletionPerformed)
            {
                //message to user
            }

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmRecurringGiftBatch)ParentForm).EnableTransactionsTab();
            }
            else
            {
                ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                ((TFrmRecurringGiftBatch)ParentForm).DisableTransactionsTab();
            }
        }

        private void Submit(System.Object sender, System.EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please select a Batch before submitting."));
                return;
            }

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmRecurringGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The batch was not submitted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then submit it!"));
                    return;
                }
            }

            if ((FPreviouslySelectedDetailRow.HashTotal != 0) && (FPreviouslySelectedDetailRow.BatchTotal != FPreviouslySelectedDetailRow.HashTotal))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The recurring gift batch total ({0}) for batch {1} does not equal the hash total ({2})."),
                        FPreviouslySelectedDetailRow.BatchTotal.ToString("C"),
                        FPreviouslySelectedDetailRow.BatchNumber,
                        FPreviouslySelectedDetailRow.HashTotal.ToString("C")), "Submit Recurring Gift Batch");

                txtDetailHashTotal.Focus();
                txtDetailHashTotal.SelectAll();
                return;
            }

            TFrmRecurringGiftBatchSubmit submitForm = new TFrmRecurringGiftBatchSubmit(FPetraUtilsObject.GetForm());
            try
            {
                ParentForm.ShowInTaskbar = false;
                submitForm.MainDS = FMainDS;
                submitForm.BatchRow = FPreviouslySelectedDetailRow;
                submitForm.ShowDialog();
            }
            finally
            {
                submitForm.Dispose();
                ParentForm.ShowInTaskbar = true;
            }
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null);

            this.btnDelete.Enabled = changeable;
            pnlDetails.Enabled = changeable;
        }

        /// <summary>
        /// return the method of Payment for the transaction tab
        /// </summary>

        public String MethodOfPaymentCode {
            get
            {
                return cmbDetailMethodOfPaymentCode.GetSelectedString();
            }
        }
        private void MethodOfPaymentChanged(object sender, EventArgs e)
        {
            FSelectedBatchMethodOfPayment = cmbDetailMethodOfPaymentCode.GetSelectedString();

            if ((FSelectedBatchMethodOfPayment != null) && (FSelectedBatchMethodOfPayment.Length > 0))
            {
                ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateMethodOfPayment(false);
            }
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            String ACurrencyCode = cmbDetailCurrencyCode.GetSelectedString();

            txtDetailHashTotal.CurrencySymbol = ACurrencyCode;
            ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(ACurrencyCode);
        }

        private void HashTotalChanged(object sender, EventArgs e)
        {
            Decimal HashTotal = Convert.ToDecimal(txtDetailHashTotal.NumberValueDecimal);
            Form p = ParentForm;

            if (p != null)
            {
                TUC_RecurringGiftTransactions t = ((TFrmRecurringGiftBatch)ParentForm).GetTransactionsControl();

                if (t != null)
                {
                    t.UpdateHashTotal(HashTotal);
                }
            }
        }

        private void ValidateDataDetailsManual(ARecurringGiftBatchRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            if (ARow == null)
            {
                return;
            }

            //Hash total special case in view of the textbox handling
            ParseHashTotal(ARow);

            TSharedFinanceValidation_Gift.ValidateRecurringGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ParseHashTotal(ARecurringGiftBatchRow ARow)
        {
            decimal correctHashValue;
            string hashTotal = txtDetailHashTotal.Text.Trim();
            string hashNumericPart = string.Empty;
            decimal hashDecimalVal;
            Int32 hashTotalIndexOfLastNumeric = -1;
            bool isNumericVal;

            if (!txtDetailHashTotal.NumberValueDecimal.HasValue)
            {
                correctHashValue = 0m;
            }
            else if (hashTotal.Contains(" "))
            {
                hashNumericPart = hashTotal.Substring(0, hashTotal.IndexOf(' '));

                if (!Decimal.TryParse(hashNumericPart, out hashDecimalVal))
                {
                    correctHashValue = 0m;
                }
                else
                {
                    correctHashValue = hashDecimalVal;
                }
            }
            else
            {
                hashTotalIndexOfLastNumeric = hashTotal.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

                if (hashTotalIndexOfLastNumeric > -1)
                {
                    hashNumericPart = hashTotal.Substring(0, hashTotalIndexOfLastNumeric + 1);
                    isNumericVal = Decimal.TryParse(hashNumericPart, out hashDecimalVal);

                    if (!isNumericVal)
                    {
                        correctHashValue = 0m;
                    }
                    else
                    {
                        //hashTotal = hashTotal.Insert(hashNumericPart.Length, " ");
                        correctHashValue = hashDecimalVal;
                    }
                }
                else
                {
                    correctHashValue = 0m;
                }
            }

            if (txtDetailHashTotal.NumberValueDecimal != correctHashValue)
            {
                txtDetailHashTotal.NumberValueDecimal = correctHashValue;
            }

            if (ARow.HashTotal != correctHashValue)
            {
                ARow.HashTotal = correctHashValue;
            }
        }

        /// <summary>
        /// Focus on grid
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
            {
                grdDetails.Focus();
            }
        }
    }
}