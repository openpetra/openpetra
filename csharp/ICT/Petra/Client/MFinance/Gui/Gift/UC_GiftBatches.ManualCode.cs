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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
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
    public partial class TUC_GiftBatches
    {
        private Int32 FLedgerNumber;
        private Int32 FSelectedBatchNumber;
        private DateTime FDateEffective;
        private string FBatchDescription = Catalog.GetString("Please enter batch description");
        private string FStatusFilter = "1 = 1";
        private string FPeriodFilter = "1 = 1";
        private bool FBatchLoaded = false;

        /// <summary>
        /// Refresh the data in the grid and the details after the database content was changed on the server
        /// </summary>
        public void RefreshAll()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            LoadBatches(FLedgerNumber);
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// load the batches into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void LoadBatches(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FDateEffective = DateTime.Today;

            ((TFrmGiftBatch)ParentForm).ClearCurrentSelections();

            if (ViewMode)
            {
                FMainDS.Merge(ViewModeTDS);
                rbtAll.Checked = true;
                cmbYear.Enabled = false;
                cmbPeriod.Enabled = false;
            }
            else
            {
                FPetraUtilsObject.DisableDataChangedEvent();
                TFinanceControls.InitialiseAvailableGiftYearsList(ref cmbYear, FLedgerNumber);
                FPetraUtilsObject.EnableDataChangedEvent();

                // only refresh once, seems we are doing too many loads from the db otherwise
                RefreshFilter(null, null);
            }

            // Load Motivation detail in this central place; it will be used by UC_GiftTransactions
            AMotivationDetailTable motivationDetail = (AMotivationDetailTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.MotivationList,
                FLedgerNumber);
            motivationDetail.TableName = FMainDS.AMotivationDetail.TableName;
            FMainDS.Merge(motivationDetail);

            FMainDS.AcceptChanges();

            // if this form is readonly, then we need all codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailBankAccountCode, FLedgerNumber, true, false, ActiveOnly, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailBankCostCentre, FLedgerNumber, true, false, ActiveOnly, true);
            cmbDetailMethodOfPaymentCode.AddNotSetRow("", "");
            TFinanceControls.InitialiseMethodOfPaymentCodeList(ref cmbDetailMethodOfPaymentCode, ActiveOnly);

            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;
            DateTime DefaultDate;
            TLedgerSelection.GetCurrentPostingRangeDates(ALedgerNumber, out StartDateCurrentPeriod, out EndDateLastForwardingPeriod, out DefaultDate);
            lblValidDateRange.Text = String.Format(Catalog.GetString("Valid between {0} and {1}"),
                StartDateCurrentPeriod.ToShortDateString(), EndDateLastForwardingPeriod.ToShortDateString());
            dtpDetailGlEffectiveDate.Date = DefaultDate;

            if (grdDetails.Rows.Count > 1)
            {
                ((TFrmGiftBatch) this.ParentForm).EnableTransactions();
            }
            else
            {
                ((TFrmGiftBatch) this.ParentForm).DisableTransactions();
            }

            ShowData();

            FBatchLoaded = true;
        }

        void RefreshPeriods(Object sender, EventArgs e)
        {
            TFinanceControls.InitialiseAvailableFinancialPeriodsList(ref cmbPeriod, FLedgerNumber, cmbYear.GetSelectedInt32());
            cmbPeriod.SelectedIndex = 0;
        }

        void RefreshFilter(Object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }
            else if (!((TFrmGiftBatch)ParentForm).SaveChanges())
            {
                return;
            }

            ClearCurrentSelection();

            Int32 SelectedYear = cmbYear.GetSelectedInt32();
            Int32 SelectedPeriod = cmbPeriod.GetSelectedInt32();

            FPeriodFilter = String.Format(
                "{0} = {1} AND ",
                AGiftBatchTable.GetBatchYearDBName(), SelectedYear);

            if (SelectedPeriod == 0)
            {
                ALedgerRow Ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                FPeriodFilter += String.Format(
                    "{0} >= {1}",
                    AGiftBatchTable.GetBatchPeriodDBName(), Ledger.CurrentPeriod);
            }
            else
            {
                FPeriodFilter += String.Format(
                    "{0} = {1}",
                    AGiftBatchTable.GetBatchPeriodDBName(), SelectedPeriod);
            }

            if (rbtEditing.Checked)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, MFinanceConstants.BATCH_UNPOSTED, SelectedYear,
                        SelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);
            }
            else if (rbtPosted.Checked)
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, MFinanceConstants.BATCH_POSTED, SelectedYear,
                        SelectedPeriod));
                FStatusFilter = String.Format("{0} = '{1}'",
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_POSTED);
            }
            else
            {
                FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.LoadAGiftBatch(FLedgerNumber, string.Empty, SelectedYear, SelectedPeriod));
                FStatusFilter = "1 = 1";
            }

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftBatch.DefaultView);

            FMainDS.AGiftBatch.DefaultView.RowFilter =
                String.Format("({0}) AND ({1})", FPeriodFilter, FStatusFilter);

            FGridFilterChanged = true;

            if (grdDetails.Rows.Count < 2)
            {
                ClearControls();
                ((TFrmGiftBatch) this.ParentForm).DisableTransactions();
            }
            else if (FBatchLoaded == true)
            {
                grdDetails.SelectRowInGrid(1, TSgrdDataGrid.TInvokeGridFocusEventEnum.NoFocusEvent);
                //FCurrentRow = 0; //necessary to force code execution in FocusRowChanged event
                InvokeFocusedRowChanged(1);
            }

            UpdateChangeableStatus();
        }

        /// reset the control
        public void ClearCurrentSelection()
        {
            GetDataFromControls();
            this.FPreviouslySelectedDetailRow = null;
            ShowData();
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

        private void ShowDetailsManual(AGiftBatchRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            FLedgerNumber = ARow.LedgerNumber;
            FSelectedBatchNumber = ARow.BatchNumber;

            FPetraUtilsObject.DetailProtectedMode =
                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED) || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED)) || ViewMode;

            ((TFrmGiftBatch)ParentForm).EnableTransactions();

            UpdateChangeableStatus();

//            FPetraUtilsObject.DetailProtectedMode =
//                (ARow.BatchStatus.Equals(MFinanceConstants.BATCH_POSTED) || ARow.BatchStatus.Equals(MFinanceConstants.BATCH_CANCELLED)) || ViewMode;
//            ((TFrmGiftBatch)ParentForm).LoadTransactions(
//                ARow.LedgerNumber,
//                ARow.BatchNumber);
        }

        private Boolean ViewMode
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewMode;
            }
        }
        private GiftBatchTDS ViewModeTDS
        {
            get
            {
                return ((TFrmGiftBatch)ParentForm).ViewModeTDS;
            }
        }

        private void ShowTransactionTab(Object sender, EventArgs e)
        {
            ((TFrmGiftBatch)ParentForm).SelectTab(TFrmGiftBatch.eGiftTabs.Transactions, false);
        }

        /// <summary>
        /// add a new batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            //If viewing posted batches only, show list of editing batches
            //  instead before adding a new batch
            if (!rbtEditing.Checked)
            {
                rbtEditing.Checked = true;
            }

            pnlDetails.Enabled = true;
            this.CreateNewAGiftBatch();
            txtDetailBatchDescription.Focus();

            dtpDetailGlEffectiveDate.Date = DateTime.Today;
            ((TFrmGiftBatch)ParentForm).SaveChanges();
        }

        /// <summary>
        /// cancel a batch (there is no deletion of batches)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelRow(System.Object sender, EventArgs e)
        {
            this.DeleteAGiftBatch();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(ref AGiftBatchRow ARowToDelete, ref string ADeletionQuestion)
        {
            if ((grdDetails.SelectedRowIndex() == -1) || (FPreviouslySelectedDetailRow == null))
            {
                MessageBox.Show(Catalog.GetString("No Gift Batch is selected to delete."),
                    Catalog.GetString("Cancelling of Gift Batch"));
                return false;
            }
            else
            {
                // ask if the user really wants to cancel the batch
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to cancel Gift Batch no: {0} ?"),
                    ARowToDelete.BatchNumber);
                return true;
            }
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ref AGiftBatchRow ARowToDelete, out string ACompletionMessage)
        {
            bool deletionSuccessful = false;

            try
            {
                //Normally need to set the message parameters before the delete is performed if requiring any of the row values
                ACompletionMessage = String.Format(Catalog.GetString("Batch no.: {0} cancelled successfully."),
                    ARowToDelete.BatchNumber);

                //Batch is only cancelled and never deleted
                ARowToDelete.BatchStatus = MFinanceConstants.BATCH_CANCELLED;

                // save first, then post
                if (!((TFrmGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The cancelled batch cannot be saved!") + Environment.NewLine +
                        Catalog.GetString("Please click Save to confirm the deletion."));

                    deletionSuccessful = false;
                }
                else
                {
                    deletionSuccessful = true;
                }
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
        private void PostDeleteManual(ref AGiftBatchRow ARowToDelete, bool AAllowDeletion, bool ADeletionPerformed, string ACompletionMessage)
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
                    ClearControls();
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
                ((TFrmGiftBatch)ParentForm).EnableTransactions();
            }
            else
            {
                ((TFrmGiftBatch)ParentForm).DisableTransactions();
            }
        }

        private void ClearControls()
        {
            txtDetailBatchDescription.Clear();
            txtDetailHashTotal.NumberValueDecimal = 0;
            dtpDetailGlEffectiveDate.Clear();
            cmbDetailBankCostCentre.SelectedIndex = -1;
            cmbDetailBankAccountCode.SelectedIndex = -1;
            cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
        }

        private void PostBatch(System.Object sender, EventArgs e)
        {
            // TODO: show VerificationResult
            // TODO: display progress of posting
            TVerificationResultCollection Verifications;

            if (FPetraUtilsObject.HasChanges)
            {
                // save first, then post
                if (!((TFrmGiftBatch)ParentForm).SaveChanges())
                {
                    // saving failed, therefore do not try to post
                    MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                        Catalog.GetString("Please first save the batch, and then post it!"));
                    return;
                }
            }

            // ask if the user really wants to post the batch
            if (MessageBox.Show(Catalog.GetString("Do you really want to post this gift batch?"), Catalog.GetString("Confirm posting of Gift Batch"),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }

            //Read current rows position ready to reposition after removal of posted row from grid
            int newCurrentRowPos = grdDetails.SelectedRowIndex();

            if (!TRemote.MFinance.Gift.WebConnectors.PostGiftBatch(FLedgerNumber, FSelectedBatchNumber, out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"));
            }
            else
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"));

                AGiftBatchRow giftBatchRow = (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, FSelectedBatchNumber });
                giftBatchRow.BatchStatus = MFinanceConstants.BATCH_POSTED;
                giftBatchRow.AcceptChanges();

                // make sure that the gift batch is not touched again, by GetDetailsFromControls
                FSelectedBatchNumber = -1;
                FPreviouslySelectedDetailRow = null;

                // make sure that gift transactions and details are cleared as well
                ((TFrmGiftBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();
                FMainDS.AGiftDetail.Rows.Clear();
                FMainDS.AGift.Rows.Clear();

                ((TFrmGiftBatch)ParentForm).ClearCurrentSelections();

                //Select unposted batch row in same index position as batch just posted
	            grdDetails.DataSource = null;
	            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftBatch.DefaultView);

	            if (grdDetails.Rows.Count > 1)
                {
		            //Needed because posting process forces grid events which sets FDetailGridRowsCountPrevious = FDetailGridRowsCountCurrent
		            // such that a removal of a row is not detected
	            	FDetailGridRowsCountPrevious++;
	            	InvokeFocusedRowChanged(newCurrentRowPos);
				}
				else 
				{
                    ClearControls();
                    ((TFrmGiftBatch)this.ParentForm).DisableTransactions();
                }
            }
        }

        private void ExportBatches(System.Object sender, System.EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // without save the server does not have the current changes, so forbid it.
                MessageBox.Show(Catalog.GetString("Please save changed Data before the Export!"),
                    Catalog.GetString("Export Error"));
                return;
            }

            TFrmGiftBatchExport exportForm = new TFrmGiftBatchExport(FPetraUtilsObject.GetForm());
            exportForm.LedgerNumber = FLedgerNumber;
            exportForm.Show();
        }

        private void ReverseGiftBatch(System.Object sender, System.EventArgs e)
        {
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().ShowRevertAdjustForm("ReverseGiftBatch");
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        public void UpdateChangeableStatus()
        {
            Boolean changeable = (FPreviouslySelectedDetailRow != null) && (!ViewMode)
                                 && (FPreviouslySelectedDetailRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            this.btnDelete.Enabled = changeable;
            this.btnPostBatch.Enabled = changeable;
            pnlDetails.Enabled = changeable;
            mniBatch.Enabled = !ViewMode;
            mniPost.Enabled = !ViewMode;
            tbbExportBatches.Enabled = !ViewMode;
            tbbImportBatches.Enabled = !ViewMode;
            tbbPostBatch.Enabled = !ViewMode;
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
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateControlsProtection();
        }

        private void CurrencyChanged(object sender, EventArgs e)
        {
            String ACurrencyCode = cmbDetailCurrencyCode.GetSelectedString();

            txtDetailHashTotal.CurrencySymbol = ACurrencyCode;
            ((TFrmGiftBatch)ParentForm).GetTransactionsControl().UpdateCurrencySymbols(ACurrencyCode);
        }

        private void HashTotalChanged(object sender, EventArgs e)
        {
            Decimal HashTotal = Convert.ToDecimal(txtDetailHashTotal.NumberValueDecimal);
            Form p = ParentForm;

            if (p != null)
            {
                TUC_GiftTransactions t = ((TFrmGiftBatch)ParentForm).GetTransactionsControl();

                if (t != null)
                {
                    t.UpdateHashTotal(HashTotal);
                }
            }
        }

        /// Select a special batch number from outside
        public void SelectBatchNumber(Int32 ABatchNumber)
        {
            for (int i = 0; (i < FMainDS.AGiftBatch.Rows.Count); i++)
            {
                if (FMainDS.AGiftBatch[i].BatchNumber == ABatchNumber)
                {
                    SelectDetailRowByDataTableIndex(i);
                    break;
                }
            }
        }

        private void ValidateDataDetailsManual(AGiftBatchRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_Gift.ValidateGiftBatchManual(this, ARow, ref VerificationResultCollection,
                FValidationControlsDict);
        }
    }
}