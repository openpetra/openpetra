//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;

using SourceGrid;

#region changelog

/*
 * Fix incorrect checking when submitting batch containing analysis attributes: https://tracker.openpetra.org/view.php?id=5562 - Moray
 */
#endregion

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLTransactions : IBoundImageEvaluator
    {
        /// <summary>
        /// Currently selected Batch number
        /// </summary>
        public Int32 FBatchNumber = -1;
        /// <summary>
        /// Currently selected journal number
        /// </summary>
        public Int32 FJournalNumber = -1;

        private GLSetupTDS FCacheDS = null;
        private bool FLoadCompleted = false;
        private Int32 FLedgerNumber = -1;
        private Int32 FTransactionNumber = -1;
        private bool FActiveOnly = false; //opposite of GL Transactions form
        private string FTransactionCurrency = string.Empty;

        private decimal FDebitAmount = 0;
        private decimal FCreditAmount = 0;

        private ARecurringBatchRow FBatchRow = null;
        private AAccountTable FAccountList;
        private ACostCentreTable FCostCentreList;

        private GLBatchTDSARecurringJournalRow FJournalRow = null;
        private ARecurringTransAnalAttribRow FPSAttributesRow = null;
        private TAnalysisAttributes FAnalysisAttributesLogic;

        private SourceGrid.Cells.Editors.ComboBox FcmbAnalAttribValues;

        private bool FShowStatusDialogOnLoad = true;
        private bool FDoneComboInitialise = false;
        private bool FSuppressListChanged = false;

        /// <summary>
        /// Sets a flag to show the status dialog when transactions are loaded
        /// </summary>
        public Boolean ShowStatusDialogOnLoad
        {
            set
            {
                FShowStatusDialogOnLoad = value;
            }
        }

        private void InitialiseControls()
        {
            cmbDetailKeyMinistryKey.ComboBoxWidth = txtDetailNarrative.Width;
        }

        /// <summary>
        /// WorkAroundInitialization
        /// </summary>
        public void WorkAroundInitialization()
        {
            txtCreditAmount.Validated += new EventHandler(ControlHasChanged);
            txtDebitAmount.Validated += new EventHandler(ControlHasChanged);
            cmbDetailCostCentreCode.Validated += new EventHandler(ControlValidatedHandler);
            cmbDetailAccountCode.Validated += new EventHandler(ControlValidatedHandler);
            cmbDetailKeyMinistryKey.Validated += new EventHandler(ControlValidatedHandler);
            txtDetailNarrative.Validated += new EventHandler(ControlValidatedHandler);
            txtDetailReference.Validated += new EventHandler(ControlValidatedHandler);
            grdAnalAttributes.Selection.SelectionChanged += new RangeRegionChangedEventHandler(AnalysisAttributesGrid_RowSelected);

            //Disallow the entry of the minus sign as no negative amounts allowed.
            //Instead, the user is expected to follow accounting riles and apply a positive amount
            //  to debit or credit accordingly to achieve the same effect
            txtDebitAmount.NegativeValueAllowed = false;
            txtCreditAmount.NegativeValueAllowed = false;
        }

        /// <summary>
        /// Get current transaction row
        /// </summary>
        /// <returns></returns>
        public GLBatchTDSARecurringTransactionRow GetCurrentTransactionRow()
        {
            return (GLBatchTDSARecurringTransactionRow) this.GetSelectedDetailRow();
        }

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ACurrencyCode"></param>
        /// <param name="AFromBatchTab"></param>
        /// <returns>True if new GL transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string ACurrencyCode,
            bool AFromBatchTab = false)
        {
            TFrmStatusDialog dlgStatus = null;
            bool DifferentBatchSelected = false;

            FLoadCompleted = false;

            FBatchRow = GetBatchRow();
            FJournalRow = GetJournalRow();

            //FBatchNumber and FJournalNumber may have already been set outside
            //  so need to reset to previous value
            if (txtBatchNumber.Text.Length > 0)
            {
                FBatchNumber = Int32.Parse(txtBatchNumber.Text);
            }

            if (txtJournalNumber.Text.Length > 0)
            {
                FJournalNumber = Int32.Parse(txtJournalNumber.Text);
            }

            if (FLedgerNumber == -1)
            {
                InitialiseControls();
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                //Check if the same batch is selected, so no need to apply filter
                if ((FLedgerNumber == ALedgerNumber)
                    && (FBatchNumber == ABatchNumber)
                    && (FJournalNumber == AJournalNumber)
                    && (FTransactionCurrency == ACurrencyCode)
                    && (FMainDS.ARecurringTransaction.DefaultView.Count > 0))
                {
                    //Same as previously selected

                    //Need to reconnect FPrev in some circumstances
                    if (FPreviouslySelectedDetailRow == null)
                    {
                        DataRowView rowView = (DataRowView)grdDetails.Rows.IndexToDataSourceRow(FPrevRowChangedRow);

                        if (rowView != null)
                        {
                            FPreviouslySelectedDetailRow = (GLBatchTDSARecurringTransactionRow)(rowView.Row);
                        }
                    }

                    if (GetSelectedRowIndex() > 0)
                    {
                        if (AFromBatchTab)
                        {
                            SelectRowInGrid(GetSelectedRowIndex());
                        }
                        else
                        {
                            GetDetailsFromControls(GetSelectedDetailRow());
                        }
                    }

                    FLoadCompleted = true;

                    return false;
                }

                // Different batch selected
                DifferentBatchSelected = true;
                bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != ACurrencyCode);

                //Handle dialog
                dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());

                if (FShowStatusDialogOnLoad)
                {
                    dlgStatus.Show();
                    FShowStatusDialogOnLoad = false;
                    dlgStatus.Heading = String.Format(Catalog.GetString("Recurring Batch {0}, Journal {1}"), ABatchNumber, AJournalNumber);
                    dlgStatus.CurrentStatus = Catalog.GetString("Loading transactions ...");
                }

                FLedgerNumber = ALedgerNumber;
                FBatchNumber = ABatchNumber;
                FJournalNumber = AJournalNumber;
                FTransactionNumber = -1;
                FTransactionCurrency = ACurrencyCode;

                FPreviouslySelectedDetailRow = null;
                grdDetails.SuspendLayout();
                //Empty grids before filling them
                grdDetails.DataSource = null;
                grdAnalAttributes.DataSource = null;
                FSuppressListChanged = false;

                // This sets the main part of the filter but excluding the additional items set by the user GUI
                // It gets the right sort order
                SetTransactionDefaultView();

                //Set the Analysis attributes filter as well
                FAnalysisAttributesLogic = new TAnalysisAttributes(FLedgerNumber, FBatchNumber, FJournalNumber);
                FAnalysisAttributesLogic.SetRecurringTransAnalAttributeDefaultView(FMainDS);
                FMainDS.ARecurringTransAnalAttrib.DefaultView.AllowNew = false;

                //Load from server if necessary
                if (FMainDS.ARecurringTransaction.DefaultView.Count == 0)
                {
                    dlgStatus.CurrentStatus = Catalog.GetString("Requesting transactions from server...");
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionARecurringTransAnalAttrib(ALedgerNumber, ABatchNumber,
                            AJournalNumber));
                }
                else if (FMainDS.ARecurringTransAnalAttrib.DefaultView.Count == 0) // just in case transactions have been loaded in a separate process without analysis attributes
                {
                    dlgStatus.CurrentStatus = Catalog.GetString("Requesting analysis attributes from server...");
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransAnalAttribForJournal(ALedgerNumber, ABatchNumber,
                            AJournalNumber));
                }

                //We need to call this because we have not called ShowData(), which would have set it.
                // This differs from the Gift screen.
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransaction.DefaultView);

                // Now we set the full filter
                dlgStatus.CurrentStatus = Catalog.GetString("Selecting the records...");
                FFilterAndFindObject.ApplyFilter();

                dlgStatus.CurrentStatus = Catalog.GetString("Configuring analysis attributes ...");

                if (grdAnalAttributes.Columns.Count == 1)
                {
                    grdAnalAttributes.SpecialKeys = GridSpecialKeys.Default | GridSpecialKeys.Tab;

                    FcmbAnalAttribValues = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                    FcmbAnalAttribValues.EnableEdit = true;
                    FcmbAnalAttribValues.EditableMode = EditableMode.Focus;
                    grdAnalAttributes.AddTextColumn("Value",
                        FMainDS.ARecurringTransAnalAttrib.Columns[ARecurringTransAnalAttribTable.GetAnalysisAttributeValueDBName()], 100,
                        FcmbAnalAttribValues);
                    FcmbAnalAttribValues.Control.SelectedValueChanged += new EventHandler(this.AnalysisAttributeValueChanged);

                    grdAnalAttributes.Columns[0].Width = 99;
                }

                grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransAnalAttrib.DefaultView);
                grdAnalAttributes.SetHeaderTooltip(0, Catalog.GetString("Type"));
                grdAnalAttributes.SetHeaderTooltip(1, Catalog.GetString("Value"));

                //Always show active and inactive values
                if (requireControlSetup)
                {
                    //Load all analysis attribute values
                    if (FCacheDS == null)
                    {
                        dlgStatus.CurrentStatus = Catalog.GetString("Loading analysis attributes ...");
                        FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, FActiveOnly);
                    }

                    SetupExtraGridFunctionality();

                    dlgStatus.CurrentStatus = Catalog.GetString("Initialising accounts and cost centres ...");

                    // We suppress change detection because these are the correct values
                    // Then initialise our combo boxes for the correct account codes and cost centres
                    bool prevSuppressChangeDetection = FPetraUtilsObject.SuppressChangeDetection;
                    FPetraUtilsObject.SuppressChangeDetection = true;
                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, FActiveOnly, false, ACurrencyCode, true);
                    TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false, true);
                    FPetraUtilsObject.SuppressChangeDetection = prevSuppressChangeDetection;

                    cmbDetailAccountCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_ACCOUNT;
                    cmbDetailCostCentreCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_COST_CENTRE;
                }

                //Old data may not have correct LastTransactionNumber
                if (FJournalRow.LastTransactionNumber != FMainDS.ARecurringTransaction.DefaultView.Count)
                {
                    if (GLRoutines.UpdateRecurringJournalLastTransaction(ref FMainDS, ref FJournalRow))
                    {
                        FPetraUtilsObject.SetChangedFlag();
                    }
                }

                //Check for incorrect Exchange rate to base (mainly for existing Petra data)
                foreach (DataRowView drv in FMainDS.ARecurringTransaction.DefaultView)
                {
                    ARecurringTransactionRow rtr = (ARecurringTransactionRow)drv.Row;

                    if (rtr.ExchangeRateToBase == 0)
                    {
                        rtr.ExchangeRateToBase = 1;
                        FPetraUtilsObject.HasChanges = true;
                    }
                }

                UpdateTransactionTotals();
                grdDetails.ResumeLayout();
                FLoadCompleted = true;

                ShowData();
                SelectRowInGrid(1);
                ShowDetails(); //Needed because of how currency is handled
                UpdateChangeableStatus();

                UpdateRecordNumberDisplay();
                FFilterAndFindObject.SetRecordNumberDisplayProperties();

                //Check for missing analysis attributes and their values
                if (grdDetails.Rows.Count > 1)
                {
                    string updatedTransactions = string.Empty;

                    dlgStatus.CurrentStatus = Catalog.GetString("Checking analysis attributes ...");
                    FAnalysisAttributesLogic.ReconcileRecurringTransAnalysisAttributes(FMainDS, out updatedTransactions);

                    if (updatedTransactions.Length > 0)
                    {
                        //Remove trailing comma
                        updatedTransactions = updatedTransactions.Remove(updatedTransactions.Length - 2);
                        MessageBox.Show(String.Format(Catalog.GetString(
                                    "Analysis Attributes have been updated in transaction(s): {0}.{1}{1}Remember to check their values."),
                                updatedTransactions,
                                Environment.NewLine),
                            "Analysis Attributes",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        FPetraUtilsObject.SetChangedFlag();
                    }
                }

                RefreshAnalysisAttributesGrid();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (dlgStatus != null)
                {
                    dlgStatus.Close();
                }

                this.Cursor = Cursors.Default;
            }

            return DifferentBatchSelected;
        }

        private void SetTransactionDefaultView(bool AAscendingOrder = true)
        {
            string sort = AAscendingOrder ? "ASC" : "DESC";

            if (FBatchNumber != -1)
            {
                string rowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber);

                FMainDS.ARecurringTransaction.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.ARecurringTransaction.DefaultView.Sort = String.Format("{0} " + sort,
                    ARecurringTransactionTable.GetTransactionNumberDBName());
            }
        }

        private ARecurringBatchRow GetBatchRow()
        {
            return ((TFrmRecurringGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private GLBatchTDSARecurringJournalRow GetJournalRow()
        {
            return ((TFrmRecurringGLBatch)ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            //TODO check if needed
            //FPetraUtilsObject.VerificationResultCollection.Clear();

            if (CreateNewARecurringTransaction())
            {
                pnlTransAnalysisAttributes.Enabled = true;
                btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);

                //Needs to be called at end of addition process to process Analysis Attributes
                AccountCodeDetailChanged(null, null);
            }
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref GLBatchTDSARecurringTransactionRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefJournalRow">this can be null; otherwise this is the journal that the transaction should belong to</param>
        public void NewRowManual(ref GLBatchTDSARecurringTransactionRow ANewRow, ARecurringJournalRow ARefJournalRow)
        {
            if (ARefJournalRow == null)
            {
                ARefJournalRow = FJournalRow;
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ++ARefJournalRow.LastTransactionNumber;
            ANewRow.TransactionDate = FBatchRow.DateEffective;
            ANewRow.ExchangeRateToBase = FJournalRow.ExchangeRateToBase;

            if (FPreviouslySelectedDetailRow != null)
            {
                ANewRow.CostCentreCode = FPreviouslySelectedDetailRow.CostCentreCode;
                ANewRow.Narrative = FPreviouslySelectedDetailRow.Narrative;
                ANewRow.Reference = FPreviouslySelectedDetailRow.Reference;
            }
        }

        /// <summary>
        /// show ledger, batch and journal number
        /// </summary>
        private void ShowDataManual()
        {
            if (FLedgerNumber != -1)
            {
                string TransactionCurrency = FJournalRow.TransactionCurrency;
                string BaseCurrency = FMainDS.ALedger[0].BaseCurrency;

                txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
                txtBatchNumber.Text = FBatchNumber.ToString();
                txtJournalNumber.Text = FJournalNumber.ToString();

                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), TransactionCurrency);
                txtDebitAmount.CurrencyCode = TransactionCurrency;
                txtCreditAmount.CurrencyCode = TransactionCurrency;
                txtCreditTotalAmount.CurrencyCode = TransactionCurrency;
                txtDebitTotalAmount.CurrencyCode = TransactionCurrency;

                // foreign currency accounts only get transactions in that currency
                if (FTransactionCurrency != TransactionCurrency)
                {
                    string SelectedAccount = cmbDetailAccountCode.GetSelectedString();

                    // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
                    bool ActiveOnly = this.Enabled;

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, FActiveOnly, false, TransactionCurrency);

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount, -1);

                    FTransactionCurrency = TransactionCurrency;
                }
            }
        }

        private void ShowDetailsManual(ARecurringTransactionRow ARow)
        {
            grdDetails.TabStop = (ARow != null);
            grdAnalAttributes.Enabled = (ARow != null);

            if (ARow == null)
            {
                FTransactionNumber = -1;
                ClearControls();
                btnNew.Focus();
                return;
            }

            FTransactionNumber = ARow.TransactionNumber;

            if (ARow.DebitCreditIndicator)
            {
                txtDebitAmount.NumberValueDecimal = ARow.TransactionAmount;
                txtCreditAmount.NumberValueDecimal = 0;
            }
            else
            {
                txtDebitAmount.NumberValueDecimal = 0;
                txtCreditAmount.NumberValueDecimal = ARow.TransactionAmount;
            }

            RefreshAnalysisAttributesGrid();
        }

        private void RefreshAnalysisAttributesGrid()
        {
            // We can be called when the user has clicked on the current transaction during an attribute value edit.
            // If the attributes are stuck in an edit we must not fiddle with the grid but wait until the edit is complete.
            if (grdAnalAttributes.IsEditorEditing)
            {
                return;
            }

            //Empty the grid
            FMainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = "1=2";
            FPSAttributesRow = null;

            if ((FPreviouslySelectedDetailRow == null)
                || !pnlTransAnalysisAttributes.Enabled
                || !TRemote.MFinance.Setup.WebConnectors.AccountHasAnalysisAttributes(FLedgerNumber, cmbDetailAccountCode.GetSelectedString(),
                    FActiveOnly))
            {
                if (grdAnalAttributes.Enabled)
                {
                    grdAnalAttributes.Enabled = false;
                    lblAnalAttributesHelp.Enabled = false;
                }

                return;
            }
            else
            {
                if (!grdAnalAttributes.Enabled)
                {
                    grdAnalAttributes.Enabled = true;
                }
            }

            FAnalysisAttributesLogic.SetRecurringTransAnalAttributeDefaultView(FMainDS, FTransactionNumber);

            grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransAnalAttrib.DefaultView);

            bool gotRows = (grdAnalAttributes.Rows.Count > 1);
            lblAnalAttributesHelp.Enabled = gotRows;

            if (gotRows)
            {
                grdAnalAttributes.SelectRowWithoutFocus(1);
                AnalysisAttributesGrid_RowSelected(null, null);
            }
        }

        private void AnalysisAttributesGrid_RowSelected(System.Object sender, RangeRegionChangedEventArgs e)
        {
            if (grdAnalAttributes.Selection.ActivePosition.IsEmpty() || (grdAnalAttributes.Selection.ActivePosition.Column == 0))
            {
                return;
            }

            if ((TAnalysisAttributes.GetSelectedRecurringAttributeRow(grdAnalAttributes) == null)
                || (FPSAttributesRow == TAnalysisAttributes.GetSelectedRecurringAttributeRow(grdAnalAttributes)))
            {
                return;
            }

            FPSAttributesRow = TAnalysisAttributes.GetSelectedRecurringAttributeRow(grdAnalAttributes);

            string currentAnalTypeCode = FPSAttributesRow.AnalysisTypeCode;

            FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0}='{1}' AND {2}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                currentAnalTypeCode,
                AFreeformAnalysisTable.GetActiveDBName());

            int analTypeCodeValuesCount = FCacheDS.AFreeformAnalysis.DefaultView.Count;

            if (analTypeCodeValuesCount == 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "No attribute values are defined!"), currentAnalTypeCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string[] analTypeValues = new string[analTypeCodeValuesCount];

            FCacheDS.AFreeformAnalysis.DefaultView.Sort = AFreeformAnalysisTable.GetAnalysisValueDBName();
            int counter = 0;

            foreach (DataRowView dvr in FCacheDS.AFreeformAnalysis.DefaultView)
            {
                AFreeformAnalysisRow faRow = (AFreeformAnalysisRow)dvr.Row;
                analTypeValues[counter] = faRow.AnalysisValue;

                counter++;
            }

            //Refresh the combo values
            FcmbAnalAttribValues.StandardValuesExclusive = true;
            FcmbAnalAttribValues.StandardValues = analTypeValues;
        }

        private void AnalysisAttributeValueChanged(System.Object sender, EventArgs e)
        {
            DevAge.Windows.Forms.DevAgeComboBox valueType = (DevAge.Windows.Forms.DevAgeComboBox)sender;

            int selectedValueIndex = valueType.SelectedIndex;

            if (selectedValueIndex < 0)
            {
                return;
            }
            else if ((FPSAttributesRow != null)
                     && (valueType.Items[selectedValueIndex].ToString() != FPSAttributesRow.AnalysisAttributeValue.ToString()))
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void GetDetailDataFromControlsManual(ARecurringTransactionRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            Decimal OldTransactionAmount = ARow.TransactionAmount;
            bool OldDebitCreditIndicator = ARow.DebitCreditIndicator;

            GetDataForAmountFields(ARow);

            if ((OldTransactionAmount != Convert.ToDecimal(ARow.TransactionAmount))
                || (OldDebitCreditIndicator != ARow.DebitCreditIndicator))
            {
                UpdateTransactionTotals(true);
            }
        }

        private void GetDataForAmountFields(ARecurringTransactionRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            bool DebitCreditIndicator;
            decimal TransactionAmount;

            if ((txtDebitAmount.Text.Length == 0) && (txtDebitAmount.NumberValueDecimal.Value != 0))
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }

            if ((txtCreditAmount.Text.Length == 0) && (txtCreditAmount.NumberValueDecimal.Value != 0))
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator != DebitCreditIndicator)
            {
                ARow.DebitCreditIndicator = DebitCreditIndicator;
            }

            if (ARow.DebitCreditIndicator)
            {
                TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);

                if (txtCreditAmount.NumberValueDecimal.Value != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
            else
            {
                TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

            if (ARow.TransactionAmount != TransactionAmount)
            {
                ARow.TransactionAmount = TransactionAmount;
            }
        }

        private Boolean EnsureGLDataPresent(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            ref DataView AJournalDV,
            bool AUpdateCurrentTransOnly)
        {
            bool RetVal = false;

            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);

            if (AUpdateCurrentTransOnly)
            {
                AJournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringJournalTable.GetJournalNumberDBName(),
                    AJournalNumber);

                RetVal = true;
            }
            else if (AJournalNumber == 0)
            {
                AJournalDV.RowFilter = String.Format("{0}={1}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    ABatchNumber);

                if (AJournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalARecurringTransaction(ALedgerNumber, ABatchNumber));

                    if (AJournalDV.Count == 0)
                    {
                        return false;
                    }
                }

                TransDV.RowFilter = String.Format("{0}={1}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    ABatchNumber);

                if (TransDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransaction(ALedgerNumber, ABatchNumber));
                }

                //As long as transactions exist, return true
                RetVal = true;
            }
            else
            {
                AJournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringJournalTable.GetJournalNumberDBName(),
                    AJournalNumber);

                if (AJournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournal(ALedgerNumber, ABatchNumber, AJournalNumber));

                    if (AJournalDV.Count == 0)
                    {
                        return false;
                    }
                }

                TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    AJournalNumber);

                if (TransDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransaction(ALedgerNumber, ABatchNumber, AJournalNumber));
                }

                RetVal = true;
            }

            return RetVal;
        }

        private void UpdateTransactionTotals(bool AIsActionInsideRowEdit = false)
        {
            bool OriginalSaveButtonState = false;
            bool TransactionRowActive = false;
            bool TransactionDataChanged = false;
            bool JournalDataChanged = false;

            int CurrentTransBatchNumber = 0;
            int CurrentTransJournalNumber = 0;
            int CurrentTransNumber = 0;

            decimal AmtCreditTotal = 0.0M;
            decimal AmtDebitTotal = 0.0M;
            decimal AmtCreditTotalBase = 0.0M;
            decimal AmtDebitTotalBase = 0.0M;
            decimal AmtInBaseCurrency = 0.0M;

            string LedgerBaseCurrency = string.Empty;
            int LedgerNumber = 0;
            int CurrentBatchNumber = 0;
            int CurrentJournalNumber = 0;

            DataView JournalsToUpdateDV = null;
            DataView TransactionsToUpdateDV = null;

            ARecurringBatchRow CurrentBatchRow = GetBatchRow();
            ARecurringJournalRow CurrentJournalRow = GetJournalRow();

            if ((CurrentBatchRow == null) || (CurrentJournalRow == null))
            {
                return;
            }

            //Set inital values after confirming not null
            OriginalSaveButtonState = FPetraUtilsObject.HasChanges;
            LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            LedgerNumber = CurrentBatchRow.LedgerNumber;
            CurrentBatchNumber = CurrentBatchRow.BatchNumber;
            CurrentJournalNumber = CurrentJournalRow.JournalNumber;

            JournalsToUpdateDV = new DataView(FMainDS.ARecurringJournal);
            TransactionsToUpdateDV = new DataView(FMainDS.ARecurringTransaction);

            if (FPreviouslySelectedDetailRow != null)
            {
                TransactionRowActive = true;

                CurrentTransBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
                CurrentTransJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
                CurrentTransNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            }

            if (!EnsureGLDataPresent(LedgerNumber, CurrentBatchNumber, CurrentJournalNumber, ref JournalsToUpdateDV, TransactionRowActive))
            {
                //No transactions exist to process or corporate exchange rate not found
                return;
            }

            //Iterate through journals
            if ((FPreviouslySelectedDetailRow != null) && !AIsActionInsideRowEdit)
            {
                FPreviouslySelectedDetailRow.BeginEdit();
            }

            bool currentRowEdited = false;

            foreach (DataRowView drv in JournalsToUpdateDV)
            {
                GLBatchTDSARecurringJournalRow jr = (GLBatchTDSARecurringJournalRow)drv.Row;

                TransactionsToUpdateDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    jr.BatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    jr.JournalNumber);

                //If all rows deleted
                if (TransactionsToUpdateDV.Count == 0)
                {
                    if ((txtCreditAmount.NumberValueDecimal != 0)
                        || (txtDebitAmount.NumberValueDecimal != 0)
                        || (txtCreditTotalAmount.NumberValueDecimal != 0)
                        || (txtDebitTotalAmount.NumberValueDecimal != 0))
                    {
                        txtCreditAmount.NumberValueDecimal = 0;
                        txtDebitAmount.NumberValueDecimal = 0;
                        txtCreditTotalAmount.NumberValueDecimal = 0;
                        txtDebitTotalAmount.NumberValueDecimal = 0;
                    }
                }

                // NOTE: AlanP changed this code in Feb 2015.  Before that the code used the DataView directly.
                // We did a foreach on the DataView and modified the international currency amounts in the rows of the DataRowView.
                // Amazingly the effect of this was that each iteration of the loop took longer and longer.  We had a set of 70 transactions
                // that we worked with and the first row took 50ms and then the times increased linearly until row 70 took 410ms!
                // Overall the 70 rows CONSISTENTLY took just over 15 seconds.  But the scary thing was that if we had, say, 150 rows (which
                // would easily be possible), we would be looking at more than 1 minute to execute this loop.
                // So the code now converts the view to a Working Table, then operates on the data in that table and finally merges the
                // working table back into FMainDS.  By doing this the time for the 70 rows goes from 15 seconds to 300ms.

                DataTable dtWork = TransactionsToUpdateDV.ToTable();

                //Iterate through all transactions in Journal
                for (int i = dtWork.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow drWork = dtWork.Rows[i];
                    TransactionDataChanged = false;
                    drWork.BeginEdit();

                    bool IsCurrentActiveTransRow = (TransactionRowActive
                                                    && Convert.ToInt32(drWork[ARecurringTransactionTable.ColumnTransactionNumberId]) ==
                                                    CurrentTransNumber
                                                    && Convert.ToInt32(drWork[ARecurringTransactionTable.ColumnBatchNumberId]) ==
                                                    CurrentTransBatchNumber
                                                    && Convert.ToInt32(drWork[ARecurringTransactionTable.ColumnJournalNumberId]) ==
                                                    CurrentTransJournalNumber);

                    if (IsCurrentActiveTransRow)
                    {
                        if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                        {
                            if ((txtCreditAmount.NumberValueDecimal != 0)
                                || (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtDebitAmount.NumberValueDecimal)))
                            {
                                txtCreditAmount.NumberValueDecimal = 0;
                                FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtDebitAmount.NumberValueDecimal);
                                currentRowEdited = true;
                            }
                        }
                        else
                        {
                            if ((txtDebitAmount.NumberValueDecimal != 0)
                                || (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtCreditAmount.NumberValueDecimal)))
                            {
                                txtDebitAmount.NumberValueDecimal = 0;
                                FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtCreditAmount.NumberValueDecimal);
                                currentRowEdited = true;
                            }
                        }
                    }

                    // recalculate the amount in base currency
                    if (jr.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                    {
                        //TODO: add if
                        AmtInBaseCurrency = GLRoutines.Divide(Convert.ToDecimal(
                                drWork[ARecurringTransactionTable.ColumnTransactionAmountId]), jr.ExchangeRateToBase);

                        if (AmtInBaseCurrency != Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId]))
                        {
                            //TODO: add if
                            drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId] = AmtInBaseCurrency;

                            TransactionDataChanged = true;
                        }
                    }

                    if (Convert.ToBoolean(drWork[ARecurringTransactionTable.ColumnDebitCreditIndicatorId]) == true)
                    {
                        AmtDebitTotal += Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnTransactionAmountId]);
                        AmtDebitTotalBase += Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId]);

                        if (IsCurrentActiveTransRow)
                        {
                            if ((FPreviouslySelectedDetailRow.AmountInBaseCurrency !=
                                 Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId])))
                            {
                                FPreviouslySelectedDetailRow.AmountInBaseCurrency =
                                    Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId]);
                                currentRowEdited = true;
                            }
                        }
                    }
                    else
                    {
                        AmtCreditTotal += Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnTransactionAmountId]);
                        AmtCreditTotalBase += Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId]);

                        if (IsCurrentActiveTransRow)
                        {
                            if ((FPreviouslySelectedDetailRow.AmountInBaseCurrency !=
                                 Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId])))
                            {
                                FPreviouslySelectedDetailRow.AmountInBaseCurrency =
                                    Convert.ToDecimal(drWork[ARecurringTransactionTable.ColumnAmountInBaseCurrencyId]);
                                currentRowEdited = true;
                            }
                        }
                    }

                    if (TransactionDataChanged == true)
                    {
                        drWork.EndEdit();
                    }
                    else
                    {
                        drWork.CancelEdit();
                        drWork.Delete();
                    }
                }   // Next transaction

                if (dtWork.Rows.Count > 0)
                {
                    FMainDS.ARecurringTransaction.Merge(dtWork);
                    JournalDataChanged = true;
                }

                if (TransactionRowActive
                    && (jr.BatchNumber == CurrentTransBatchNumber)
                    && (jr.JournalNumber == CurrentTransJournalNumber)
                    && ((txtCreditTotalAmount.NumberValueDecimal != AmtCreditTotal)
                        || (txtDebitTotalAmount.NumberValueDecimal != AmtDebitTotal)))
                {
                    txtCreditTotalAmount.NumberValueDecimal = AmtCreditTotal;
                    txtDebitTotalAmount.NumberValueDecimal = AmtDebitTotal;
                }
            }   // next journal

            if (currentRowEdited)
            {
                FPreviouslySelectedDetailRow.EndEdit();
            }
            else if ((FPreviouslySelectedDetailRow != null) && !AIsActionInsideRowEdit)
            {
                FPreviouslySelectedDetailRow.CancelEdit();
            }

            //Update totals of Batch
            GLRoutines.UpdateRecurringBatchTotals(ref FMainDS, ref CurrentBatchRow, CurrentJournalNumber);

            //In trans loading
            txtCreditTotalAmount.NumberValueDecimal = AmtCreditTotal;
            txtDebitTotalAmount.NumberValueDecimal = AmtDebitTotal;

            // refresh the currency symbols
            if (TransactionRowActive)
            {
                ShowDataManual();
            }

            if (!JournalDataChanged && (OriginalSaveButtonState != FPetraUtilsObject.HasChanges))
            {
                ((TFrmRecurringGLBatch)ParentForm).SaveChanges();
            }
            else if (JournalDataChanged)
            {
                // Automatically save the changes to Totals??
                // For now we will just enable the save button which will give the user a surprise!
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void SetupExtraGridFunctionality()
        {
            DataTable CostCentreListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref CostCentreListTable, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            if ((CostCentreListTable == null) || (CostCentreListTable.Rows.Count == 0))
            {
                FCostCentreList = null;
            }
            else
            {
                FCostCentreList = (ACostCentreTable)CostCentreListTable;
            }

            DataTable AccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref AccountListTable, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            if ((AccountListTable == null) || (AccountListTable.Rows.Count == 0))
            {
                FAccountList = null;
            }
            else
            {
                FAccountList = (AAccountTable)AccountListTable;
            }

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 1;
            int indexOfAccountCodeDataColumn = 2;

            // Add red triangle to inactive accounts
            grdDetails.AddAnnotationImage(this, indexOfCostCentreCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.CostCentreCode, BoundGridImage.DisplayImageEnum.Inactive);
            grdDetails.AddAnnotationImage(this, indexOfAccountCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.AccountCode, BoundGridImage.DisplayImageEnum.Inactive);

            //Add conditions to columns of Analysis Attributes grid
            int indexOfAnalysisCodeColumn = 0;
            int indexOfAnalysisAttributeValueColumn = 1;

            // Add red triangle to inactive analysis type codes and their values
            grdAnalAttributes.AddAnnotationImage(this, indexOfAnalysisCodeColumn,
                BoundGridImage.AnnotationContextEnum.AnalysisTypeCode, BoundGridImage.DisplayImageEnum.Inactive);
            grdAnalAttributes.AddAnnotationImage(this, indexOfAnalysisAttributeValueColumn,
                BoundGridImage.AnnotationContextEnum.AnalysisAttributeValue, BoundGridImage.DisplayImageEnum.Inactive);
        }

        private bool AccountIsActive(int ALedgerNumber, string AAccountCode = "")
        {
            bool AccountActive = false;
            bool AccountExists = true;

            //If empty, read value from combo
            if (AAccountCode.Length == 0)
            {
                if ((cmbDetailAccountCode.SelectedIndex != -1)
                    && (cmbDetailAccountCode.Count > 0)
                    && (cmbDetailAccountCode.GetSelectedString() != null))
                {
                    AAccountCode = cmbDetailAccountCode.GetSelectedString();
                }
                else
                {
                    return false;
                }
            }

            if (FAccountList == null)
            {
                SetupAccountCostCentreVariables(ALedgerNumber);
            }

            AccountActive = TFinanceControls.AccountIsActive(ALedgerNumber, AAccountCode, FAccountList, out AccountExists);

            if (!AccountExists && (AAccountCode.Length > 0))
            {
                string errorMessage = String.Format(Catalog.GetString("Account {0} does not exist in Ledger {1}!"),
                    AAccountCode,
                    ALedgerNumber);
                TLogging.Log(errorMessage);
            }

            return AccountActive;
        }

        private bool CostCentreIsActive(int ALedgerNumber, string ACostCentreCode = "")
        {
            bool CostCentreActive = false;
            bool CostCentreExists = true;

            //If empty, read value from combo
            if (ACostCentreCode.Length == 0)
            {
                if ((cmbDetailCostCentreCode.SelectedIndex != -1)
                    && (cmbDetailCostCentreCode.Count > 0)
                    && (cmbDetailCostCentreCode.GetSelectedString() != null))
                {
                    ACostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
                }
                else
                {
                    return false;
                }
            }

            if (FCostCentreList == null)
            {
                SetupAccountCostCentreVariables(ALedgerNumber);
            }

            CostCentreActive = TFinanceControls.CostCentreIsActive(ALedgerNumber, ACostCentreCode, FCostCentreList, out CostCentreExists);

            if (!CostCentreExists && (ACostCentreCode.Length > 0))
            {
                string errorMessage = String.Format(Catalog.GetString("Cost Centre {0} does not exist in Ledger {1}!"),
                    ACostCentreCode,
                    ALedgerNumber);
                TLogging.Log(errorMessage);
            }

            return CostCentreActive;
        }

        private void SetupAccountCostCentreVariables(int ALedgerNumber)
        {
            DataTable CostCentreListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, ALedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref CostCentreListTable, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            if ((CostCentreListTable == null) || (CostCentreListTable.Rows.Count == 0))
            {
                FCostCentreList = null;
            }
            else
            {
                FCostCentreList = (ACostCentreTable)CostCentreListTable;
            }

            DataTable AccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, ALedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref AccountListTable, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            if ((AccountListTable == null) || (AccountListTable.Rows.Count == 0))
            {
                FAccountList = null;
            }
            else
            {
                FAccountList = (AAccountTable)AccountListTable;
            }
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            bool NumericAmountChange = false;
            int ErrorCounter = FPetraUtilsObject.VerificationResultCollection.Count;

            if (sender.GetType() == typeof(TTxtCurrencyTextBox))
            {
                NumericAmountChange = true;
                CheckAmounts((TTxtCurrencyTextBox)sender);
            }

            ControlValidatedHandler(sender, e);

            //If no errors and amount has changed then update totals
            if (NumericAmountChange && (FPetraUtilsObject.VerificationResultCollection.Count == ErrorCounter))
            {
                UpdateTransactionTotals();
            }
        }

        private void CheckAmounts(TTxtCurrencyTextBox ATxtCurrencyTextBox)
        {
            bool debitChanged = (ATxtCurrencyTextBox.Name == "txtDebitAmount");

            if (!debitChanged && (ATxtCurrencyTextBox.Name != "txtCreditAmount"))
            {
                return;
            }
            else if ((ATxtCurrencyTextBox.NumberValueDecimal == null) || !ATxtCurrencyTextBox.NumberValueDecimal.HasValue)
            {
                ATxtCurrencyTextBox.NumberValueDecimal = 0;
            }

            decimal valDebit = txtDebitAmount.NumberValueDecimal.Value;
            decimal valCredit = txtCreditAmount.NumberValueDecimal.Value;

            //If no changes then proceed no further
            if (debitChanged && (FDebitAmount == valDebit))
            {
                return;
            }
            else if (!debitChanged && (FCreditAmount == valCredit))
            {
                return;
            }

            if (debitChanged && ((valDebit > 0) && (valCredit > 0)))
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }
            else if (!debitChanged && ((valDebit > 0) && (valCredit > 0)))
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }
            else if (valDebit < 0)
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }
            else if (valCredit < 0)
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            //Reset class variables
            FDebitAmount = txtDebitAmount.NumberValueDecimal.Value;
            FCreditAmount = txtCreditAmount.NumberValueDecimal.Value;
        }

        /// <summary>
        /// enable or disable the buttons
        /// </summary>
        private void UpdateChangeableStatus()
        {
            Boolean changeable = !FPetraUtilsObject.DetailProtectedMode
                                 && (GetBatchRow() != null);
            Boolean canDeleteAll = (FFilterAndFindObject.IsActiveFilterEqualToBase);
            bool rowsInGrid = (grdDetails.Rows.Count > 1);

            // pnlDetailsProtected must be changed first: when the enabled property of the control is changed, the focus changes, which triggers validation
            pnlDetailsProtected = !changeable;
            pnlDetails.Enabled = (changeable && rowsInGrid);
            btnDelete.Enabled = (changeable && rowsInGrid);
            btnDeleteAll.Enabled = (changeable && canDeleteAll && rowsInGrid);
            pnlTransAnalysisAttributes.Enabled = changeable;
            //lblAnalAttributes.Enabled = (changeable && rowsInGrid);
            btnNew.Enabled = changeable;
        }

        /// <summary>
        /// Delete transaction data from current recurring GL Batch
        /// </summary>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        public void DeleteRecurringTransactionData(Int32 ABatchNumber, Int32 AJournalNumber = 0)
        {
            DataView TransAnalAttribDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            TransAnalAttribDV.RowFilter = String.Format("{0}={1} And " + (AJournalNumber > 0 ? "{2}={3}" : "{2}>{3}"),
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                AJournalNumber);

            TransAnalAttribDV.Sort = String.Format("{0} DESC, {1} DESC, {2} DESC",
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

            foreach (DataRowView dr in TransAnalAttribDV)
            {
                dr.Delete();
            }

            DataView TransactionDV = new DataView(FMainDS.ARecurringTransaction);

            TransactionDV.RowFilter = String.Format("{0}={1} And " + (AJournalNumber > 0 ? "{2}={3}" : "{2}>{3}"),
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalNumber);

            TransactionDV.Sort = String.Format("{0} DESC, {1} DESC",
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ARecurringTransactionTable.GetTransactionNumberDBName());

            foreach (DataRowView dr in TransactionDV)
            {
                dr.Delete();
            }
        }

        private void DeleteAllTrans(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            else if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
            {
                MessageBox.Show(Catalog.GetString("Please remove the filter before attempting to delete all transactions in this recurring journal."),
                    Catalog.GetString("Delete All Transactions"));

                return;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all transactions in this recurring Journal ({0}).\n\nDo you really want to continue?"),
                         FJournalNumber),
                     Catalog.GetString("Confirm Deletion"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
            {
                return;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = null;

            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Specify current action
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.DELETINGALLTRANS;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GLBatchTDS)FMainDS.GetChangesTyped(false);

                //Unbind any transactions currently being editied in the Transaction Tab
                // but do not reset FBatchNumber to -1
                ClearCurrentSelection(0, 0, false);

                //Delete transactions
                DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
                DataView TransAttribDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

                TransDV.AllowDelete = true;
                TransAttribDV.AllowDelete = true;

                TransAttribDV.RowFilter = String.Format("{0}={1} AND {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber);

                TransAttribDV.Sort = String.Format("{0} ASC, {1} ASC",
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                for (int i = TransAttribDV.Count - 1; i >= 0; i--)
                {
                    TransAttribDV.Delete(i);
                }

                TransDV.RowFilter = String.Format("{0}={1} AND {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber);

                TransDV.Sort = String.Format("{0} ASC",
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                for (int i = TransDV.Count - 1; i >= 0; i--)
                {
                    TransDV.Delete(i);
                }

                //Set last journal number
                GetJournalRow().LastTransactionNumber = 0;

                FPetraUtilsObject.SetChangedFlag();

                //Need to call save
                if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction, true, false))
                {
                    FMyForm.GetBatchControl().UpdateRecurringBatchDictionary();

                    MessageBox.Show(Catalog.GetString("The transactions have been deleted but the changes are not saved!"),
                        Catalog.GetString("Deletion Warning"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    SelectRowInGrid(1);
                }
                else
                {
                    UpdateChangeableStatus();
                    ClearControls();

                    MessageBox.Show(Catalog.GetString("All transactions have been deleted successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Update transaction totals and save the new figures
                    UpdateTransactionTotals();
                    FMyForm.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS, 1);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
                this.Cursor = Cursors.Default;
            }
        }

        private bool PreDeleteManual(ARecurringTransactionRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            if (FPreviouslySelectedDetailRow != null)
            {
                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete transaction no. {0} from recurring Journal {1}?"),
                    ARowToDelete.TransactionNumber,
                    ARowToDelete.JournalNumber);
            }

            return allowDeletion;
        }

        /// <summary>
        /// Code to be run after the deletion process
        /// </summary>
        /// <param name="ARowToDelete">the row that was/was to be deleted</param>
        /// <param name="AAllowDeletion">whether or not the user was permitted to delete</param>
        /// <param name="ADeletionPerformed">whether or not the deletion was performed successfully</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        private void PostDeleteManual(ARecurringTransactionRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                if (ADeletionPerformed)
                {
                    UpdateChangeableStatus();

                    if (!pnlDetails.Enabled)
                    {
                        ClearControls();
                    }

                    //Always update LastTransactionNumber first before updating totals
                    GLRoutines.UpdateRecurringJournalLastTransaction(ref FMainDS, ref FJournalRow);
                    UpdateTransactionTotals();

                    if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction))
                    {
                        FMyForm.GetBatchControl().UpdateRecurringBatchDictionary();

                        MessageBox.Show(Catalog.GetString("The transaction has been deleted but the changes are not saved!"),
                            Catalog.GetString("Deletion Warning"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //message to user
                        MessageBox.Show(ACompletionMessage,
                            "Deletion Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else if (!AAllowDeletion && (ACompletionMessage.Length > 0))
                {
                    //message to user
                    MessageBox.Show(ACompletionMessage,
                        "Deletion not allowed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (!ADeletionPerformed && (ACompletionMessage.Length > 0))
                {
                    //message to user
                    MessageBox.Show(ACompletionMessage,
                        "Deletion failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            finally
            {
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;
            }
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(GLBatchTDSARecurringTransactionRow ARowToDelete, ref string ACompletionMessage)
        {
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            //Check if row to delete is on server or not
            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            //Take a backup of FMainDS
            GLBatchTDS BackupMainDS = null;

            int TransactionNumberToDelete = ARowToDelete.TransactionNumber;
            int TopMostTransNo = FJournalRow.LastTransactionNumber;

            TFrmRecurringGLBatch FMyForm = (TFrmRecurringGLBatch) this.ParentForm;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Specify current action
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.DELETINGTRANS;

                //Backup the Dataset for reversion purposes
                BackupMainDS = (GLBatchTDS)FMainDS.GetChangesTyped(false);

                if (RowToDeleteIsNew)
                {
                    ProcessNewlyAddedTransactionRowForDeletion(TransactionNumberToDelete);
                }
                else
                {
                    //Return modified row to last saved state to avoid validation failures
                    ARowToDelete.RejectChanges();
                    ShowDetails(ARowToDelete);

                    //Accept changes for other newly added rows, which by definition would have passed validation
                    if (OtherUncommittedRowsExist(FBatchNumber, FJournalNumber, TransactionNumberToDelete)
                        && !FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction))
                    {
                        FMyForm.GetBatchControl().UpdateRecurringBatchDictionary();

                        MessageBox.Show(Catalog.GetString("The transaction has not been deleted!"),
                            Catalog.GetString("Deletion Warning"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }

                    GLBatchTDS TempDS = CopyTransDataToNewDataset(ARowToDelete.BatchNumber, ARowToDelete.JournalNumber);

                    //Clear the transactions and load newly saved dataset
                    RemoveTransDataFromFMainDS(ARowToDelete.BatchNumber);
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.ProcessRecurringTransAndAttributesForDeletion(TempDS, FLedgerNumber, FBatchNumber,
                            FJournalNumber, TopMostTransNo, TransactionNumberToDelete));
                }

                FPreviouslySelectedDetailRow = null;
                FPetraUtilsObject.SetChangedFlag();

                ACompletionMessage = String.Format(Catalog.GetString("Transaction no.: {0} deleted successfully."),
                    TransactionNumberToDelete);

                DeletionSuccessful = true;
            }
            catch (Exception ex)
            {
                //Normally set in PostDeleteManual
                FMyForm.FCurrentGLBatchAction = TGLBatchEnums.GLBatchAction.NONE;

                //Revert to previous state
                RevertDataSet(FMainDS, BackupMainDS);

                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                SetTransactionDefaultView();
                FFilterAndFindObject.ApplyFilter();
                this.Cursor = Cursors.Default;
            }

            return DeletionSuccessful;
        }

        private bool OtherUncommittedRowsExist(int ABatchNumber, int AJournalNumber, int ATransactionNumber)
        {
            bool UncommittedRowsExist = false;

            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
            DataView TransAnalDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            TransDV.RowFilter = String.Format("{0}={1} And {2}={3} And {4}<>{5}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalNumber,
                ARecurringTransactionTable.GetTransactionNumberDBName(),
                ATransactionNumber);

            foreach (DataRowView drv in TransDV)
            {
                DataRow dr = (DataRow)drv.Row;

                if (dr.RowState == DataRowState.Added)
                {
                    UncommittedRowsExist = true;
                    break;
                }
            }

            if (!UncommittedRowsExist)
            {
                TransAnalDV.RowFilter = String.Format("{0}={1} And {2}={3} And {4}<>{5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumber);

                foreach (DataRowView drv in TransAnalDV)
                {
                    DataRow dr = (DataRow)drv.Row;

                    if (dr.RowState == DataRowState.Added)
                    {
                        UncommittedRowsExist = true;
                        break;
                    }
                }
            }

            return UncommittedRowsExist;
        }

        private void RemoveTransDataFromFMainDS(int ABatchNumber)
        {
            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
            DataRowCollection TransRowsCollection = FMainDS.ARecurringTransaction.Rows;
            DataView TransAnalDV = new DataView(FMainDS.ARecurringTransAnalAttrib);
            DataRowCollection TransAnalRowsCollection = FMainDS.ARecurringTransAnalAttrib.Rows;

            //In reverse order REMOVE rows from tables ready for delete process
            TransAnalDV.RowFilter = String.Format("{0}={1}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView drv in TransAnalDV)
            {
                DataRow dr = (DataRow)drv.Row;
                TransAnalRowsCollection.Remove(dr);
            }

            TransDV.RowFilter = String.Format("{0}={1}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber);

            foreach (DataRowView drv in TransDV)
            {
                DataRow dr = (DataRow)drv.Row;
                TransRowsCollection.Remove(dr);
            }
        }

        private GLBatchTDS CopyTransDataToNewDataset(int ABatchNumber, int AJournalNumber)
        {
            GLBatchTDS TempDS = (GLBatchTDS)FMainDS.Copy();

            TempDS.Merge(FMainDS);

            DataView TransDV = new DataView(TempDS.ARecurringTransaction);
            DataView TransAnalDV = new DataView(TempDS.ARecurringTransAnalAttrib);

            //In reverse order
            TransAnalDV.RowFilter = String.Format("{0}<>{1} Or {2}<>{3}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                AJournalNumber);

            foreach (DataRowView drv in TransAnalDV)
            {
                drv.Delete();
            }

            TransDV.RowFilter = String.Format("{0}<>{1} Or {2}<>{3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalNumber);

            foreach (DataRowView drv in TransDV)
            {
                drv.Delete();
            }

            TempDS.AcceptChanges();

            return TempDS;
        }

        private void RevertDataSet(GLBatchTDS AMainDS, GLBatchTDS ABackupDS, int ASelectRowInGrid = 0)
        {
            if ((ABackupDS != null) && (AMainDS != null))
            {
                AMainDS.RejectChanges();
                AMainDS.Merge(ABackupDS);

                if (ASelectRowInGrid > 0)
                {
                    SelectRowInGrid(ASelectRowInGrid);
                }
            }
        }

        private void ProcessNewlyAddedTransactionRowForDeletion(Int32 ATransactionNumberToDelete)
        {
            try
            {
                // Delete the associated recurring transaction analysis attributes
                DataView attributesDV = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumberToDelete);

                foreach (DataRowView attrDRV in attributesDV)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.Delete();
                }

                //Delete the recurring transaction
                DataView transactionsDV = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumberToDelete);

                foreach (DataRowView transDRV in transactionsDV)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.Delete();
                }

                //Renumber the transactions and attributes
                DataView attributesDV2 = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV2.RowFilter = String.Format("{0}={1} And {2}={3} And {4}>{5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumberToDelete);
                attributesDV2.Sort = String.Format("{0} ASC", ARecurringTransAnalAttribTable.GetTransactionNumberDBName());

                foreach (DataRowView attrDRV in attributesDV2)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.TransactionNumber--;
                }

                DataView transactionsDV2 = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV2.RowFilter = String.Format("{0}={1} And {2}={3} And {4}>{5}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumberToDelete);
                transactionsDV2.Sort = String.Format("{0} ASC", ARecurringTransactionTable.GetTransactionNumberDBName());

                foreach (DataRowView transDRV in transactionsDV2)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.TransactionNumber--;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
        }

        private void ClearControls()
        {
            //Stop data change detection
            FPetraUtilsObject.DisableDataChangedEvent();

            //Clear combos
            cmbDetailAccountCode.SelectedIndex = -1;
            cmbDetailAccountCode.Text = string.Empty;
            cmbDetailCostCentreCode.SelectedIndex = -1;
            cmbDetailCostCentreCode.Text = string.Empty;
            cmbDetailKeyMinistryKey.SelectedIndex = -1;
            cmbDetailKeyMinistryKey.Text = string.Empty;
            //Clear Textboxes
            txtDetailNarrative.Clear();
            txtDetailReference.Clear();
            //Clear Numeric Textboxes
            txtDebitAmount.NumberValueDecimal = 0M;
            txtCreditAmount.NumberValueDecimal = 0M;
            //Clear grids
            RefreshAnalysisAttributesGrid();
            //Enable data change detection
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// if the cost centre code changes
        /// </summary>
        private void CostCentreCodeDetailChanged(object sender, EventArgs e)
        {
            if ((FLoadCompleted == false) || (FPreviouslySelectedDetailRow == null)
                || (cmbDetailCostCentreCode.GetSelectedString() == String.Empty)
                || (cmbDetailCostCentreCode.SelectedIndex == -1))
            {
                return;
            }

            // update key ministry combobox depending on account code and cost centre
            UpdateCmbDetailKeyMinistryKey();
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((FPreviouslySelectedDetailRow.TransactionNumber == FTransactionNumber)
                && (FTransactionNumber != -1))
            {
                FAnalysisAttributesLogic.RecurringTransAnalAttrRequiredUpdating(FMainDS,
                    cmbDetailAccountCode.GetSelectedString(), FTransactionNumber);
                RefreshAnalysisAttributesGrid();
            }

            // update key ministry combobox depending on account code and cost centre
            UpdateCmbDetailKeyMinistryKey();
        }

        /// <summary>
        /// if the cost centre code changes
        /// </summary>
        private void UpdateCmbDetailKeyMinistryKey()
        {
            Int64 RecipientKey = 0;

            // update key ministry combobox depending on account code and cost centre
            if ((cmbDetailAccountCode.GetSelectedString() == MFinanceConstants.FUND_TRANSFER_INCOME_ACC)
                && (cmbDetailCostCentreCode.GetSelectedString() != ""))
            {
                cmbDetailKeyMinistryKey.Enabled = true;
                TRemote.MFinance.Common.ServerLookups.WebConnectors.GetPartnerKeyForForeignCostCentreCode(FLedgerNumber,
                    cmbDetailCostCentreCode.GetSelectedString(),
                    out RecipientKey);
                TFinanceControls.GetRecipientData(ref cmbDetailKeyMinistryKey, RecipientKey);
                cmbDetailKeyMinistryKey.ComboBoxWidth = txtDetailNarrative.Width;
            }
            else
            {
                cmbDetailKeyMinistryKey.SetSelectedString("", -1);
                cmbDetailKeyMinistryKey.Enabled = false;
            }
        }

        private void ValidateDataDetailsManual(ARecurringTransactionRow ARow)
        {
            //Can be called from outside, so need to update fields
            FBatchRow = GetBatchRow();

            if (FBatchRow == null)
            {
                return;
            }

            FJournalRow = GetJournalRow();

            if (FJournalRow != null)
            {
                FJournalNumber = FJournalRow.JournalNumber;
            }

            if ((ARow == null) || (FBatchRow.BatchNumber != ARow.BatchNumber))
            {
                return;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            Control controlToPass = null;

            //Local validation
            if (((txtDebitAmount.NumberValueDecimal.Value == 0)
                 && (txtCreditAmount.NumberValueDecimal.Value == 0))
                || (txtDebitAmount.NumberValueDecimal.Value < 0))
            {
                controlToPass = txtDebitAmount;
            }
            else if (txtCreditAmount.NumberValueDecimal.Value < 0)
            {
                controlToPass = txtCreditAmount;
            }
            else if (TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_GLREFMANDATORY, "no") == "yes")
            {
                controlToPass = txtDetailReference;
            }
            else if ((VerificationResultCollection.Count == 1)
                     && (VerificationResultCollection[0].ResultCode == CommonErrorCodes.ERR_INVALIDNUMBER))
            {
                //The amount controls register as invalid even when value is correct. Need to reset
                //  Verifications accordingly.
                FPetraUtilsObject.VerificationResultCollection.Clear();
            }

            TSharedFinanceValidation_GL.ValidateRecurringGLDetailManual(this, FBatchRow, ARow, controlToPass, ref VerificationResultCollection,
                FValidationControlsDict);

            DataColumn ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnAccountCodeId];
            TScreenVerificationResult VerificationResult = null;

            if (!grdAnalAttributes.EndEdit(false))
            {
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALID_ANALYSIS_ATTRIBUTE_VALUE)),
                    ValidationColumn, grdAnalAttributes);
            }

            // Handle addition/removal to/from TVerificationResultCollection
            VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);

            if ((FPreviouslySelectedDetailRow != null)
                && !FAnalysisAttributesLogic.AccountRecurringAnalysisAttributeCountIsCorrect(
                    FPreviouslySelectedDetailRow.TransactionNumber,
                    FPreviouslySelectedDetailRow.AccountCode,
                    FMainDS))
            {
                // This code is only running because of failure, so cause an error to occur.
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INCORRECT_ANALYSIS_ATTRIBUTE_COUNT,
                            new string[] { ARow.AccountCode, ARow.TransactionNumber.ToString() })),
                    ValidationColumn, grdAnalAttributes);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }

            String ValueRequiredForType;

            if ((FPreviouslySelectedDetailRow != null)
                && !FAnalysisAttributesLogic.AccountRecurringAnalysisAttributesValuesExist(
                    FPreviouslySelectedDetailRow.TransactionNumber,
                    FPreviouslySelectedDetailRow.AccountCode,
                    FMainDS,
                    out ValueRequiredForType))
            {
                // This code is only running because of failure, so cause an error to occur.
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_MISSING_ANALYSIS_ATTRIBUTE_VALUE,
                            new string[] { ValueRequiredForType, ARow.AccountCode, ARow.TransactionNumber.ToString() })),
                    ValidationColumn, grdAnalAttributes);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        /// <param name="ABatchToClear"></param>
        /// <param name="AJournalToClear"></param>
        /// <param name="AResetFBatchNumber"></param>
        public void ClearCurrentSelection(int ABatchToClear = 0, int AJournalToClear = 0, bool AResetFBatchNumber = true)
        {
            if (this.FPreviouslySelectedDetailRow == null)
            {
                return;
            }
            else if ((ABatchToClear > 0) && (AJournalToClear == 0)
                     && (FPreviouslySelectedDetailRow.BatchNumber != ABatchToClear))
            {
                return;
            }
            else if ((ABatchToClear > 0) && (AJournalToClear > 0)
                     && !((FPreviouslySelectedDetailRow.BatchNumber == ABatchToClear)
                          && (FPreviouslySelectedDetailRow.JournalNumber == AJournalToClear)))
            {
                return;
            }
            else if ((ABatchToClear == 0) && (AJournalToClear > 0))
            {
                return;
            }

            //Set selection to null
            this.FPreviouslySelectedDetailRow = null;

            if (AResetFBatchNumber)
            {
                FBatchNumber = -1;
                FJournalNumber = -1;
            }
        }

        /// <summary>
        /// Confirm with the user concerning the presence of inactive fields
        ///  before saving changes to all changed batches
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AAction"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <returns></returns>
        public bool AllowInactiveFieldValues(int ALedgerNumber,
            int ABatchNumber,
            TGLBatchEnums.GLBatchAction AAction,
            int AJournalNumber = 0,
            int ATransactionNumber = 0)
        {
            if (AAction == TGLBatchEnums.GLBatchAction.NONE)
            {
                AAction = TGLBatchEnums.GLBatchAction.SAVING;
            }

            TUC_RecurringGLBatches MainForm = ((TFrmRecurringGLBatch)ParentForm).GetBatchControl();

            bool InSaving = (AAction == TGLBatchEnums.GLBatchAction.SAVING);
            bool InSubmitting = false;
            bool InDeletingBatch = false;
            bool InDeletingJournal = false;
            bool InDeletingAllTrans = false;
            bool InDeletingTrans = false;

            if (!InSaving)
            {
                switch (AAction)
                {
                    case TGLBatchEnums.GLBatchAction.SUBMITTING:
                        InSubmitting = true;
                        break;

                    case TGLBatchEnums.GLBatchAction.DELETING:
                        InDeletingBatch = true;
                        break;

                    case TGLBatchEnums.GLBatchAction.DELETINGJOURNAL:
                        InDeletingJournal = true;
                        break;

                    case TGLBatchEnums.GLBatchAction.DELETINGALLTRANS:
                        InDeletingAllTrans = true;
                        break;

                    case TGLBatchEnums.GLBatchAction.DELETINGTRANS:
                        InDeletingTrans = true;
                        break;
                }
            }

            bool InDeletingData = (InDeletingJournal || InDeletingAllTrans || InDeletingTrans);

            bool WarnOfInactiveForSubmittingCurrentBatch = InSubmitting && MainForm.FInactiveValuesWarningOnGLSubmitting;

            //Variables for building warning message
            string WarningMessage = string.Empty;
            string WarningHeader = string.Empty;
            StringBuilder WarningList = new StringBuilder();

            //Find batches that have changed
            List <ARecurringBatchRow>BatchesToCheck = GetUnsavedBatchRowsList(ABatchNumber);
            List <int>BatchesWithInactiveValues = new List <int>();

            if (BatchesToCheck.Count > 0)
            {
                int currentBatchListNo;
                string batchNoList = string.Empty;

                int numInactiveFieldsPresent = 0;
                int numInactiveAccounts = 0;
                int numInactiveCostCentres = 0;
                int numInactiveAccountTypes = 0;
                int numInactiveAccountValues = 0;

                // AllowInactiveFieldValues can be called without LoadTransactions() being called in this class (e.g. select a Batch and Submit it
                // without visiting the Transactions tab. In that case, FLedgerNumber and FCacheDS have not been initialized. We need them later for
                // AnalysisCodeIsActive(), but we don't want to leave our copies lying around, because LoadTransactions() may not replace them if necessary.
                // Bug #5562
                bool InvalidateAnalysisCacheAfterUse = false;

                if (FCacheDS == null)
                {
                    InvalidateAnalysisCacheAfterUse = true;
                    FLedgerNumber = ALedgerNumber;
                    FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, true);
                }

                foreach (ARecurringBatchRow gBR in BatchesToCheck)
                {
                    currentBatchListNo = gBR.BatchNumber;

                    bool checkingCurrentBatch = (currentBatchListNo == ABatchNumber);

                    //in a deleting process
                    bool noNeedToLoadDataForThisBatch = (InDeletingData && checkingCurrentBatch && (AJournalNumber > 0));

                    bool batchVerified = false;
                    bool batchExistsInDict = MainForm.FRecurringBatchesVerifiedOnSavingDict.TryGetValue(currentBatchListNo, out batchVerified);

                    if (batchExistsInDict)
                    {
                        if (batchVerified && !(InSubmitting && checkingCurrentBatch && WarnOfInactiveForSubmittingCurrentBatch))
                        {
                            continue;
                        }
                    }
                    else if (!(InDeletingBatch && checkingCurrentBatch))
                    {
                        MainForm.FRecurringBatchesVerifiedOnSavingDict.Add(currentBatchListNo, false);
                    }

                    //If processing batch about to be submitted, only warn according to user preferences
                    if ((InSubmitting && checkingCurrentBatch && !WarnOfInactiveForSubmittingCurrentBatch)
                        || (InDeletingBatch && checkingCurrentBatch))
                    {
                        continue;
                    }

                    DataView journalDV = new DataView(FMainDS.ARecurringJournal);
                    DataView transDV = new DataView(FMainDS.ARecurringTransaction);
                    DataView attribDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

                    //Make sure that journal and transaction data etc. is loaded for the current batch
                    journalDV.RowFilter = String.Format("{0}={1}",
                        ARecurringJournalTable.GetBatchNumberDBName(),
                        currentBatchListNo);

                    transDV.RowFilter = String.Format("{0}={1}",
                        ARecurringTransactionTable.GetBatchNumberDBName(),
                        currentBatchListNo);

                    attribDV.RowFilter = String.Format("{0}={1}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        currentBatchListNo);

                    if (!noNeedToLoadDataForThisBatch)
                    {
                        if (journalDV.Count == 0)
                        {
                            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringJournalAndRelatedTablesForBatch(ALedgerNumber,
                                    currentBatchListNo));

                            if (journalDV.Count == 0)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (transDV.Count == 0)
                            {
                                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionAndRelatedTablesForBatch(ALedgerNumber,
                                        currentBatchListNo));

                                if (transDV.Count == 0)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (attribDV.Count == 0)
                                {
                                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransAnalAttribForBatch(ALedgerNumber,
                                            currentBatchListNo));
                                }
                            }
                        }
                    }

                    //Check for inactive account or cost centre codes
                    transDV.Sort = String.Format("{0} ASC, {1} ASC",
                        ARecurringTransactionTable.GetJournalNumberDBName(),
                        ARecurringTransactionTable.GetTransactionNumberDBName());

                    foreach (DataRowView drv in transDV)
                    {
                        ARecurringTransactionRow transRow = (ARecurringTransactionRow)drv.Row;

                        //No need to record inactive values in transactions about to be deleted
                        if (checkingCurrentBatch
                            && ((InDeletingJournal && (AJournalNumber > 0) && (transRow.JournalNumber == AJournalNumber))
                                || (InDeletingTrans && (ATransactionNumber > 0) && (transRow.TransactionNumber == ATransactionNumber))))
                        {
                            continue;
                        }

                        if (!AccountIsActive(ALedgerNumber, transRow.AccountCode))
                        {
                            WarningList.AppendFormat(" Batch:{1} Journal:{2} Transaction:{3:00} has Account '{0}'{4}",
                                transRow.AccountCode,
                                transRow.BatchNumber,
                                transRow.JournalNumber,
                                transRow.TransactionNumber,
                                Environment.NewLine);

                            numInactiveAccounts++;

                            if (!BatchesWithInactiveValues.Contains(transRow.BatchNumber))
                            {
                                BatchesWithInactiveValues.Add(transRow.BatchNumber);
                            }
                        }

                        if (!CostCentreIsActive(ALedgerNumber, transRow.CostCentreCode))
                        {
                            WarningList.AppendFormat(" Batch:{1} Journal:{2} Transaction:{3:00} has Cost Centre '{0}'{4}",
                                transRow.CostCentreCode,
                                transRow.BatchNumber,
                                transRow.JournalNumber,
                                transRow.TransactionNumber,
                                Environment.NewLine);

                            numInactiveCostCentres++;

                            if (!BatchesWithInactiveValues.Contains(transRow.BatchNumber))
                            {
                                BatchesWithInactiveValues.Add(transRow.BatchNumber);
                            }
                        }
                    }

                    //Check anlysis attributes
                    attribDV.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

                    foreach (DataRowView drv2 in attribDV)
                    {
                        ARecurringTransAnalAttribRow analAttribRow = (ARecurringTransAnalAttribRow)drv2.Row;

                        //No need to record inactive values in transactions about to be deleted
                        if (checkingCurrentBatch
                            && ((InDeletingJournal && (AJournalNumber > 0) && (analAttribRow.JournalNumber == AJournalNumber))
                                || (InDeletingTrans && (ATransactionNumber > 0) && (analAttribRow.TransactionNumber == ATransactionNumber))))
                        {
                            continue;
                        }

                        if (!AnalysisCodeIsActive(analAttribRow.AccountCode, analAttribRow.AnalysisTypeCode))
                        {
                            WarningList.AppendFormat(" Batch:{1} Journal:{2} Transaction:{3:00} has Analysis Code '{0}'{4}",
                                analAttribRow.AnalysisTypeCode,
                                analAttribRow.BatchNumber,
                                analAttribRow.JournalNumber,
                                analAttribRow.TransactionNumber,
                                Environment.NewLine);

                            numInactiveAccountTypes++;

                            if (!BatchesWithInactiveValues.Contains(analAttribRow.BatchNumber))
                            {
                                BatchesWithInactiveValues.Add(analAttribRow.BatchNumber);
                            }
                        }

                        if (!AnalysisAttributeValueIsActive(analAttribRow.AnalysisTypeCode, analAttribRow.AnalysisAttributeValue))
                        {
                            WarningList.AppendFormat(" Batch:{1} Journal:{2} Transaction:{3:00} has Analysis Value '{0}'{4}",
                                analAttribRow.AnalysisAttributeValue,
                                analAttribRow.BatchNumber,
                                analAttribRow.JournalNumber,
                                analAttribRow.TransactionNumber,
                                Environment.NewLine);

                            numInactiveAccountValues++;

                            if (!BatchesWithInactiveValues.Contains(analAttribRow.BatchNumber))
                            {
                                BatchesWithInactiveValues.Add(analAttribRow.BatchNumber);
                            }
                        }
                    }
                }

                if (InvalidateAnalysisCacheAfterUse)
                {
                    FLedgerNumber = -1;
                    FCacheDS = null;
                }

                numInactiveFieldsPresent = (numInactiveAccounts + numInactiveCostCentres + numInactiveAccountTypes + numInactiveAccountValues);

                if (numInactiveFieldsPresent > 0)
                {
                    string batchList = string.Empty;
                    string otherChangedBatches = string.Empty;

                    BatchesWithInactiveValues.Sort();

                    //Update the dictionary
                    foreach (int batch in BatchesWithInactiveValues)
                    {
                        if (batch == ABatchNumber)
                        {
                            if ((!InSubmitting && (MainForm.FRecurringBatchesVerifiedOnSavingDict[batch] == false))
                                || (InSubmitting && WarnOfInactiveForSubmittingCurrentBatch))
                            {
                                MainForm.FRecurringBatchesVerifiedOnSavingDict[batch] = true;
                                batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            }
                        }
                        else if (MainForm.FRecurringBatchesVerifiedOnSavingDict[batch] == false)
                        {
                            MainForm.FRecurringBatchesVerifiedOnSavingDict[batch] = true;
                            batchList += (string.IsNullOrEmpty(batchList) ? "" : ", ") + batch.ToString();
                            //Build a list of all batches except current batch
                            otherChangedBatches += (string.IsNullOrEmpty(otherChangedBatches) ? "" : ", ") + batch.ToString();
                        }
                    }

                    //Create header message
                    WarningHeader = "{0} inactive value(s) found in recurring batch{1}{4}{4}Do you still want to continue with ";

                    if (InDeletingJournal)
                    {
                        WarningHeader += String.Format("deleting journal {0} and saving changes to", AJournalNumber);
                    }
                    else if (InDeletingAllTrans)
                    {
                        WarningHeader += String.Format("deleting all transactions from journal {0} and saving changes to", AJournalNumber);
                    }
                    else if (InDeletingTrans)
                    {
                        WarningHeader += String.Format("deleting transaction {0} and saving changes to", ATransactionNumber);
                    }
                    else
                    {
                        WarningHeader += AAction.ToString().ToLower();
                    }

                    WarningHeader += " batch: {2}" + (otherChangedBatches.Length > 0 ? " and with saving: {3}" : "") + " ?{4}";

                    if (!InSubmitting || (otherChangedBatches.Length > 0))
                    {
                        WarningHeader += "{4}(You will only be warned once about inactive values when saving any batch!){4}";
                    }

                    //Handle plural
                    batchList = (otherChangedBatches.Length > 0 ? "es: " : ": ") + batchList;

                    WarningMessage = String.Format(Catalog.GetString(WarningHeader + "{4}Inactive values:{4}{5}{4}{6}{5}"),
                        numInactiveFieldsPresent,
                        batchList,
                        ABatchNumber,
                        otherChangedBatches,
                        Environment.NewLine,
                        new String('-', 80),
                        WarningList);

                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox((TFrmRecurringGLBatch)ParentForm);

                    string header = string.Empty;

                    switch (AAction)
                    {
                        case TGLBatchEnums.GLBatchAction.SUBMITTING:
                            header = "Submit";
                            break;

                        case TGLBatchEnums.GLBatchAction.DELETING:
                            header = "Delete";
                            break;

                        case TGLBatchEnums.GLBatchAction.DELETINGJOURNAL:
                            header = "Delete Journal In";
                            break;

                        case TGLBatchEnums.GLBatchAction.DELETINGALLTRANS:
                            header = "Delete All Transaction Detail From";
                            break;

                        case TGLBatchEnums.GLBatchAction.DELETINGTRANS:
                            header = "Delete Transaction Detail From";
                            break;

                        default:
                            header = "Save";
                            break;
                    }

                    return extendedMessageBox.ShowDialog(WarningMessage,
                        Catalog.GetString(header + " Recurring GL Batch"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbYesNo,
                        TFrmExtendedMessageBox.TIcon.embiQuestion) == TFrmExtendedMessageBox.TResult.embrYes;
                }
            }

            return true;
        }

        private List <ARecurringBatchRow>GetUnsavedBatchRowsList(int ABatchToInclude = 0)
        {
            List <ARecurringBatchRow>RetVal = new List <ARecurringBatchRow>();
            List <int>BatchesWithChangesList = new List <int>();
            string BatchesWithChangesString = string.Empty;

            DataView BatchesDV = new DataView(FMainDS.ARecurringBatch);
            BatchesDV.Sort = ARecurringBatchTable.GetBatchNumberDBName() + " ASC";

            DataView JournalDV = new DataView(FMainDS.ARecurringJournal);
            DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
            DataView AttribDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

            //Make sure that journal and transaction data etc. is loaded for the current batch
            JournalDV.Sort = String.Format("{0} ASC, {1} ASC",
                ARecurringJournalTable.GetBatchNumberDBName(),
                ARecurringJournalTable.GetJournalNumberDBName());

            TransDV.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ARecurringTransactionTable.GetJournalNumberDBName(),
                ARecurringTransactionTable.GetTransactionNumberDBName());

            AttribDV.Sort = String.Format("{0} ASC, {1} ASC, {2} ASC, {3} ASC",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName());

            //Add the batch number(s) of changed journals
            foreach (DataRowView dRV in JournalDV)
            {
                ARecurringJournalRow jR = (ARecurringJournalRow)dRV.Row;

                if (!BatchesWithChangesList.Contains(jR.BatchNumber)
                    && (jR.RowState != DataRowState.Unchanged))
                {
                    BatchesWithChangesList.Add(jR.BatchNumber);
                }
            }

            //Generate string of all batches found with changes
            if (BatchesWithChangesList.Count > 0)
            {
                BatchesWithChangesString = String.Join(",", BatchesWithChangesList);

                //Add any other batch number(s) of changed transactions
                TransDV.RowFilter = String.Format("{0} NOT IN ({1})",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    BatchesWithChangesString);
            }

            foreach (DataRowView dRV in TransDV)
            {
                ARecurringTransactionRow tR = (ARecurringTransactionRow)dRV.Row;

                if (!BatchesWithChangesList.Contains(tR.BatchNumber)
                    && (tR.RowState != DataRowState.Unchanged))
                {
                    BatchesWithChangesList.Add(tR.BatchNumber);
                }
            }

            //Generate string of all batches found with changes
            if (BatchesWithChangesList.Count > 0)
            {
                BatchesWithChangesString = String.Join(",", BatchesWithChangesList);

                //Add any other batch number(s) of changed analysis attributes
                AttribDV.RowFilter = String.Format("{0} NOT IN ({1})",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    BatchesWithChangesString);
            }

            foreach (DataRowView dRV in AttribDV)
            {
                ARecurringTransAnalAttribRow aR = (ARecurringTransAnalAttribRow)dRV.Row;

                if (!BatchesWithChangesList.Contains(aR.BatchNumber)
                    && (aR.RowState != DataRowState.Unchanged))
                {
                    BatchesWithChangesList.Add(aR.BatchNumber);
                }
            }

            BatchesWithChangesList.Sort();

            foreach (DataRowView dRV in BatchesDV)
            {
                ARecurringBatchRow batchRow = (ARecurringBatchRow)dRV.Row;

                if ((batchRow.BatchNumber == ABatchToInclude)
                    || BatchesWithChangesList.Contains(batchRow.BatchNumber)
                    || (batchRow.RowState != DataRowState.Unchanged))
                {
                    RetVal.Add(batchRow);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Set focus to the gid controltab
        /// </summary>
        public void FocusGrid()
        {
            if ((grdDetails != null) && grdDetails.CanFocus)
            {
                grdDetails.Focus();
            }
        }

        private void DebitAmountChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if ((txtDebitAmount.NumberValueDecimal != 0) && (txtCreditAmount.NumberValueDecimal != 0))
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
        }

        private void CreditAmountChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if ((txtCreditAmount.NumberValueDecimal != 0) && (txtDebitAmount.NumberValueDecimal != 0))
                {
                    txtDebitAmount.NumberValueDecimal = 0;
                }
            }
        }

        /// <summary>
        /// Shows the Filter/Find UserControl and switches to the Find Tab.
        /// </summary>
        public void ShowFindPanel()
        {
            if (FFilterAndFindObject.FilterFindPanel == null)
            {
                FFilterAndFindObject.ToggleFilter();
            }

            FFilterAndFindObject.FilterFindPanel.DisplayFindTab();
        }

        private void RunOnceOnParentActivationManual()
        {
            grdDetails.DataSource.ListChanged += new ListChangedEventHandler(DataSource_ListChanged);
        }

        private void DataSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && !FSuppressListChanged && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();

                // Once we have auto-sized once and there are more than 8 rows we don't auto-size any more (unless we load data again)
                FSuppressListChanged = (grdDetails.Rows.Count > 8);
            }

            // If the grid list changes we might need to disable the Delete All button
            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
        }

        private void FilterToggledManual(bool AFilterIsOff)
        {
            // The first time the filter is toggled on we need to set up the cost centre and account comboBoxes
            // This means showing inactive values in red
            // We achieve this by using our own owner draw mode event
            // Also the data source for the combos will be wrong because they have been cloned from items that may not have shown inactive values
            if ((AFilterIsOff == false) && !FDoneComboInitialise)
            {
                InitFilterFindAccountCodeComboBox((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbDetailAccountCode"),
                    TCacheableFinanceTablesEnum.AccountList);
                InitFilterFindCostCentreComboBox((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName(
                        "cmbDetailCostCentreCode"),
                    TCacheableFinanceTablesEnum.CostCentreList);
                InitFilterFindAccountCodeComboBox((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbDetailAccountCode"),
                    TCacheableFinanceTablesEnum.AccountList);
                InitFilterFindCostCentreComboBox((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbDetailCostCentreCode"),
                    TCacheableFinanceTablesEnum.CostCentreList);

                FDoneComboInitialise = true;
            }

            int prevMaxRows = grdDetails.MaxAutoSizeRows;
            grdDetails.MaxAutoSizeRows = 20;
            grdDetails.AutoResizeGrid();
            grdDetails.MaxAutoSizeRows = prevMaxRows;
        }

        /// <summary>
        /// Helper method that we can call to initialise each of the filter/find comboBoxes
        /// </summary>
        private void InitFilterFindAccountCodeComboBox(TCmbAutoComplete AFFInstance, TCacheableFinanceTablesEnum AListTable)
        {
            DataView dv = new DataView(TDataCache.TMFinance.GetCacheableFinanceTable(AListTable, FLedgerNumber));

            dv.RowFilter = TFinanceControls.PrepareAccountFilter(true, false, false, false, "");
            dv.Sort = String.Format("{0}", AAccountTable.GetAccountCodeDBName());
            AFFInstance.DataSource = dv;
            AFFInstance.DrawMode = DrawMode.OwnerDrawFixed;
            AFFInstance.DrawItem += new DrawItemEventHandler(DrawComboBoxItem);
        }

        /// <summary>
        /// Helper method that we can call to initialise each of the filter/find comboBoxes
        /// </summary>
        private void InitFilterFindCostCentreComboBox(TCmbAutoComplete AFFInstance, TCacheableFinanceTablesEnum AListTable)
        {
            DataView dv = new DataView(TDataCache.TMFinance.GetCacheableFinanceTable(AListTable, FLedgerNumber));

            dv.RowFilter = TFinanceControls.PrepareCostCentreFilter(true, false, false, false);
            dv.Sort = String.Format("{0}", ACostCentreTable.GetCostCentreCodeDBName());
            AFFInstance.DataSource = dv;
            AFFInstance.DrawMode = DrawMode.OwnerDrawFixed;
            AFFInstance.DrawItem += new DrawItemEventHandler(DrawComboBoxItem);
        }

        /// <summary>
        /// This method is called when the system wants to draw a comboBox item in the list.
        /// We choose the colour and weight for the font, showing inactive codes in bold red text
        /// </summary>
        private void DrawComboBoxItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            TCmbAutoComplete cmb = (TCmbAutoComplete)sender;
            DataRowView drv = (DataRowView)cmb.Items[e.Index];
            string content = drv[1].ToString();
            Brush brush;

            if (cmb.Name.StartsWith("cmbDetailCostCentre"))
            {
                brush = CostCentreIsActive(FLedgerNumber, content) ? Brushes.Black : Brushes.Red;
            }
            else if (cmb.Name.StartsWith("cmbDetailAccount"))
            {
                brush = AccountIsActive(FLedgerNumber, content) ? Brushes.Black : Brushes.Red;
            }
            else
            {
                throw new ArgumentException("Unexpected caller of DrawComboBoxItem event");
            }

            Font font = new Font(((Control)sender).Font, (brush == Brushes.Red) ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(content, font, brush, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private bool AnalysisCodeIsActive(String AAccountCode, String AAnalysisCode = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAccountCode == string.Empty))
            {
                return retVal;
            }

            DataView dv = new DataView(FCacheDS.AAnalysisAttribute);

            dv.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}=true",
                AAnalysisAttributeTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                AAccountCode,
                AAnalysisAttributeTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AAnalysisAttributeTable.GetActiveDBName());

            retVal = (dv.Count > 0);

            return retVal;
        }

        private bool AnalysisAttributeValueIsActive(String AAnalysisCode = "", String AAnalysisAttributeValue = "")
        {
            bool retVal = true;

            if ((AAnalysisCode == string.Empty) || (AAnalysisAttributeValue == string.Empty))
            {
                return retVal;
            }

            DataView dv = new DataView(FCacheDS.AFreeformAnalysis);

            dv.RowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                AAnalysisCode,
                AFreeformAnalysisTable.GetAnalysisValueDBName(),
                AAnalysisAttributeValue,
                AFreeformAnalysisTable.GetActiveDBName());

            retVal = (dv.Count > 0);

            return retVal;
        }

        #region BoundImage interface implementation

        /// <summary>
        /// Implementation of the interface member
        /// </summary>
        /// <param name="AContext">The context that identifies the column for which an image is to be evaluated</param>
        /// <param name="ADataRowView">The data containing the column of interest.  You will evaluate whether this column contains data that should have the image or not.</param>
        /// <returns>True if the image should be displayed in the current context</returns>
        public bool EvaluateBoundImage(BoundGridImage.AnnotationContextEnum AContext, DataRowView ADataRowView)
        {
            switch (AContext)
            {
                case BoundGridImage.AnnotationContextEnum.AccountCode:
                    ARecurringTransactionRow row = (ARecurringTransactionRow)ADataRowView.Row;
                    return !AccountIsActive(FLedgerNumber, row.AccountCode);

                case BoundGridImage.AnnotationContextEnum.CostCentreCode:
                    ARecurringTransactionRow row2 = (ARecurringTransactionRow)ADataRowView.Row;
                    return !CostCentreIsActive(FLedgerNumber, row2.CostCentreCode);

                case BoundGridImage.AnnotationContextEnum.AnalysisTypeCode:
                    ARecurringTransAnalAttribRow row3 = (ARecurringTransAnalAttribRow)ADataRowView.Row;
                    return !FAnalysisAttributesLogic.AnalysisCodeIsActive(cmbDetailAccountCode.GetSelectedString(),
                    FCacheDS.AAnalysisAttribute,
                    row3.AnalysisTypeCode);

                case BoundGridImage.AnnotationContextEnum.AnalysisAttributeValue:
                    ARecurringTransAnalAttribRow row4 = (ARecurringTransAnalAttribRow)ADataRowView.Row;
                    return !TAnalysisAttributes.AnalysisAttributeValueIsActive(ref FcmbAnalAttribValues,
                    FCacheDS.AFreeformAnalysis,
                    row4.AnalysisTypeCode,
                    row4.AnalysisAttributeValue);
            }

            return false;
        }

        #endregion
    }
}