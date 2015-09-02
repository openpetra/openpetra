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
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
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

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;

using SourceGrid;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_RecurringGLTransactions
    {
        private bool FLoadCompleted = false;
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private Int32 FTransactionNumber = -1;
        private bool FActiveOnly = false; //opposite of GL Transactions form
        private string FTransactionCurrency = string.Empty;

        private decimal FDebitAmount = 0;
        private decimal FCreditAmount = 0;

        private ARecurringBatchRow FBatchRow = null;
        private AAccountTable FAccountList;
        private ACostCentreTable FCostCentreList;

        private GLSetupTDS FCacheDS;
        private GLBatchTDSARecurringJournalRow FJournalRow = null;
        private ARecurringTransAnalAttribRow FPSAttributesRow = null;
        private TAnalysisAttributes FAnalysisAttributesLogic;

        private SourceGrid.Cells.Editors.ComboBox FcmbAnalAttribValues;

        private bool FShowStatusDialogOnLoad = true;

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

                // Different batch
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

                // We need to call this because we have not called ShowData(), which would have set it.  This differs from the Gift screen.
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
                    TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                    FPetraUtilsObject.SuppressChangeDetection = prevSuppressChangeDetection;

                    cmbDetailAccountCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_ACCOUNT;
                    cmbDetailCostCentreCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_COST_CENTRE;
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
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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

            if (grdAnalAttributes.Rows.Count > 1)
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
            else if (valueType.Items[selectedValueIndex].ToString() != FPSAttributesRow.AnalysisAttributeValue.ToString())
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
                UpdateTransactionTotals();
            }

            // If combobox to set analysis attribute value has focus when save button is pressed then currently
            // displayed value is not stored in database.
            // --> move focus to different field so that grid accepts value for storing in database
            if (FcmbAnalAttribValues.Control.Focused)
            {
                cmbDetailCostCentreCode.Focus();
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
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionForBatch(ALedgerNumber, ABatchNumber));
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

        private void UpdateTransactionTotals()
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
                            }
                        }
                        else
                        {
                            if ((txtDebitAmount.NumberValueDecimal != 0)
                                || (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtCreditAmount.NumberValueDecimal)))
                            {
                                txtDebitAmount.NumberValueDecimal = 0;
                                FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtCreditAmount.NumberValueDecimal);
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

            //Update totals of Batch
            if (FLoadCompleted)
            {
                GLRoutines.UpdateTotalsOfRecurringBatch(ref FMainDS, CurrentBatchRow);
            }
            else
            {
                //In trans loading
                txtCreditTotalAmount.NumberValueDecimal = AmtCreditTotal;
                txtDebitTotalAmount.NumberValueDecimal = AmtDebitTotal;
            }

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
            DataTable TempTbl1 = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, FLedgerNumber);

            if ((TempTbl1 == null) || (TempTbl1.Rows.Count == 0))
            {
                FCostCentreList = null;
            }
            else
            {
                FCostCentreList = (ACostCentreTable)TempTbl1;
            }

            DataTable TempTbl2 = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            if ((TempTbl2 == null) || (TempTbl2.Rows.Count == 0))
            {
                FAccountList = null;
            }
            else
            {
                FAccountList = (AAccountTable)TempTbl2;
            }

            //Prepare grid to highlight inactive accounts/cost centres
            // Create a cell view for special conditions
            SourceGrid.Cells.Views.Cell strikeoutCell = new SourceGrid.Cells.Views.Cell();
            strikeoutCell.Font = new System.Drawing.Font(grdDetails.Font, FontStyle.Strikeout);
            //strikeoutCell.ForeColor = Color.Crimson;

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView conditionAccountCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionAccountCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string accountCode = row[ARecurringTransactionTable.ColumnAccountCodeId].ToString();
                return !AccountIsActive(accountCode);
            };

            SourceGrid.Conditions.ConditionView conditionCostCentreCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionCostCentreCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string costCentreCode = row[ARecurringTransactionTable.ColumnCostCentreCodeId].ToString();
                return !CostCentreIsActive(costCentreCode);
            };

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 1;
            int indexOfAccountCodeDataColumn = 2;

            grdDetails.Columns[indexOfCostCentreCodeDataColumn].Conditions.Add(conditionCostCentreCodeActive);
            grdDetails.Columns[indexOfAccountCodeDataColumn].Conditions.Add(conditionAccountCodeActive);

            //Prepare Analysis attributes grid to highlight inactive analysis codes
            // Create a cell view for special conditions
            SourceGrid.Cells.Views.Cell strikeoutCell2 = new SourceGrid.Cells.Views.Cell();
            strikeoutCell2.Font = new System.Drawing.Font(grdAnalAttributes.Font, FontStyle.Strikeout);
            //strikeoutCell.ForeColor = Color.Crimson;

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView conditionAnalysisCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell2);
            conditionAnalysisCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column2, int gridRow2, object itemRow2)
            {
                DataRowView row2 = (DataRowView)itemRow2;
                string analysisCode = row2[ARecurringTransAnalAttribTable.ColumnAnalysisTypeCodeId].ToString();
                return !FAnalysisAttributesLogic.AnalysisCodeIsActive(
                    cmbDetailAccountCode.GetSelectedString(), FCacheDS.AAnalysisAttribute, analysisCode);
            };

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView conditionAnalysisAttributeValueActive = new SourceGrid.Conditions.ConditionView(strikeoutCell2);
            conditionAnalysisAttributeValueActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column2, int gridRow2, object itemRow2)
            {
                if (itemRow2 != null)
                {
                    DataRowView row2 = (DataRowView)itemRow2;
                    string analysisCode = row2[ARecurringTransAnalAttribTable.ColumnAnalysisTypeCodeId].ToString();
                    string analysisAttributeValue = row2[ARecurringTransAnalAttribTable.ColumnAnalysisAttributeValueId].ToString();
                    return !TAnalysisAttributes.AnalysisAttributeValueIsActive(ref FcmbAnalAttribValues,
                        FCacheDS.AFreeformAnalysis,
                        analysisCode,
                        analysisAttributeValue);
                }
                else
                {
                    return false;
                }
            };

            //Add conditions to columns
            int indexOfAnalysisCodeColumn = 0;
            int indexOfAnalysisAttributeValueColumn = 1;

            grdAnalAttributes.Columns[indexOfAnalysisCodeColumn].Conditions.Add(conditionAnalysisCodeActive);
            grdAnalAttributes.Columns[indexOfAnalysisAttributeValueColumn].Conditions.Add(conditionAnalysisAttributeValueActive);
        }

        private bool AccountIsActive(string AAccountCode = "")
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

            AccountActive = TFinanceControls.AccountIsActive(FLedgerNumber, AAccountCode, FAccountList, out AccountExists);

            if (!AccountExists)
            {
                string errorMessage = String.Format(Catalog.GetString("Account {0} does not exist in Ledger {1}!"),
                    AAccountCode,
                    FLedgerNumber);
                TLogging.Log(errorMessage);
            }

            return AccountActive;
        }

        private bool CostCentreIsActive(string ACostCentreCode = "")
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

            CostCentreActive = TFinanceControls.CostCentreIsActive(FLedgerNumber, ACostCentreCode, FCostCentreList, out CostCentreExists);

            if (!CostCentreExists)
            {
                string errorMessage = String.Format(Catalog.GetString("Cost Centre {0} does not exist in Ledger {1}!"),
                    ACostCentreCode,
                    FLedgerNumber);
                TLogging.Log(errorMessage);
            }

            return CostCentreActive;
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
                     MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
            {
                //Backup the Dataset for reversion purposes
                GLBatchTDS FTempDS = (GLBatchTDS)FMainDS.Copy();
                FTempDS.Merge(FMainDS);

                try
                {
                    //Unbind any transactions currently being editied in the Transaction Tab
                    ClearCurrentSelection();

                    //Delete transactions
                    DataView TransDV = new DataView(FMainDS.ARecurringTransaction);
                    DataView TransAttribDV = new DataView(FMainDS.ARecurringTransAnalAttrib);

                    TransDV.AllowDelete = true;
                    TransAttribDV.AllowDelete = true;

                    TransDV.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ARecurringTransactionTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransactionTable.GetJournalNumberDBName(),
                        FJournalNumber);

                    TransDV.Sort = String.Format("{0} ASC",
                        ARecurringTransactionTable.GetTransactionNumberDBName());

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

                    for (int i = TransDV.Count - 1; i >= 0; i--)
                    {
                        TransDV.Delete(i);
                    }

                    //Set last journal number
                    GetJournalRow().LastTransactionNumber = 0;

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (((TFrmRecurringGLBatch)ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The recurring journal has been cleared successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UpdateTransactionTotals();
                        ((TFrmRecurringGLBatch)ParentForm).SaveChanges();
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The journal has been cleared but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        SelectRowInGrid(1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    FMainDS.Merge(FTempDS);
                }

                //If some row(s) still exist after deletion
                if (grdDetails.Rows.Count < 2)
                {
                    UpdateChangeableStatus();
                    ClearControls();
                }
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
            if (ADeletionPerformed)
            {
                UpdateChangeableStatus();

                if (!pnlDetails.Enabled)
                {
                    ClearControls();
                }

                UpdateTransactionTotals();

                ((TFrmRecurringGLBatch) this.ParentForm).SaveChanges();

                //message to user
                MessageBox.Show(ACompletionMessage,
                    "Deletion Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ARecurringTransactionRow ARowToDelete, ref string ACompletionMessage)
        {
            //Assign a default values
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current recurring transaction!");
                return DeletionSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS BackupMainDS = (GLBatchTDS)FMainDS.Copy();
            BackupMainDS.Merge(FMainDS);

            //Pass copy to delete method.
            GLBatchTDS TempDS = (GLBatchTDS)FMainDS.Copy();
            TempDS.Merge(FMainDS);

            int TransactionNumberToDelete = ARowToDelete.TransactionNumber;
            int TopMostTransNo = FJournalRow.LastTransactionNumber;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (RowToDeleteIsNew)
                {
                    ProcessNewlyAddedTransactionRowForDeletion(TopMostTransNo, TransactionNumberToDelete);
                }
                else
                {
                    TempDS.AcceptChanges();

                    //Clear the transactions and load newly saved dataset
                    FMainDS.ARecurringTransAnalAttrib.Clear();
                    FMainDS.ARecurringTransaction.Clear();
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
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS.Merge(BackupMainDS);
            }
            finally
            {
                SetTransactionDefaultView();
                FFilterAndFindObject.ApplyFilter();
                this.Cursor = Cursors.Default;
            }

            return DeletionSuccessful;
        }

        private void ProcessNewlyAddedTransactionRowForDeletion(Int32 AHighestTransactionNumber, Int32 ATransactionNumber)
        {
            GLBatchTDS SubmitDS = (GLBatchTDS)FMainDS.Copy();

            SubmitDS.Merge(FMainDS);

            try
            {
                //Remove unaffected attributes and transactions
                DataView attributesDV = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumber);

                foreach (DataRowView attrDRV in attributesDV)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.Delete();
                }

                DataView transactionsDV = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumber);

                foreach (DataRowView transDRV in transactionsDV)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.Delete();
                }

                //Renumber the transactions and attributes in TempDS
                DataView attributesDV2 = new DataView(FMainDS.ARecurringTransAnalAttrib);
                attributesDV2.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>{5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumber);
                attributesDV2.Sort = String.Format("{0} ASC", ARecurringTransAnalAttribTable.GetTransactionNumberDBName());

                foreach (DataRowView attrDRV in attributesDV2)
                {
                    ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                    attrRow.TransactionNumber--;
                }

                DataView transactionsDV2 = new DataView(FMainDS.ARecurringTransaction);
                transactionsDV2.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>{5}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumber);
                transactionsDV2.Sort = String.Format("{0} ASC", ARecurringTransactionTable.GetTransactionNumberDBName());

                foreach (DataRowView transDRV in transactionsDV2)
                {
                    ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                    tranRow.TransactionNumber--;
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        private void ClearControls()
        {
            //Stop data change detection
            FPetraUtilsObject.DisableDataChangedEvent();

            //Clear combos
            cmbDetailAccountCode.SelectedIndex = -1;
            cmbDetailCostCentreCode.SelectedIndex = -1;
            cmbDetailKeyMinistryKey.SelectedIndex = -1;
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
            if ((ARow == null) || (GetBatchRow() == null))
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
            else if (TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_GLREFMANDATORY, "no") == "yes")
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

            if ((FPreviouslySelectedDetailRow != null)
                && !FAnalysisAttributesLogic.AccountRecurringAnalysisAttributeCountIsCorrect(
                    FPreviouslySelectedDetailRow.TransactionNumber,
                    FPreviouslySelectedDetailRow.AccountCode,
                    FMainDS))
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnAccountCodeId];
                ValidationContext = "unused because of OverrideResultText";

                // This code is only running because of failure, so cause an error to occur.
                VerificationResult = TStringChecks.StringMustNotBeEmpty("",
                    ValidationContext.ToString(),
                    this, ValidationColumn, null);
                VerificationResult.OverrideResultText(String.Format(
                        "A value must be entered for 'Analysis Attributes' for Account Code {0} in Transaction {1}.",
                        ARow.AccountCode,
                        ARow.TransactionNumber));
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
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnAccountCodeId];
                ValidationContext = String.Format("Analysis code {0} for Account Code {1} in Transaction {2}",
                    ValueRequiredForType,
                    ARow.AccountCode,
                    ARow.TransactionNumber);

                // This code is only running because of failure, so cause an error to occur.
                VerificationResult = TStringChecks.StringMustNotBeEmpty("",
                    ValidationContext.ToString(),
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }
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
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }

            // If the grid list changes we might need to disable the Delete All button
            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
        }
    }
}