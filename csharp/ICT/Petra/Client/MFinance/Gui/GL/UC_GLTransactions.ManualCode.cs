//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
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
    public partial class TUC_GLTransactions
    {
        private bool FLoadCompleted = false;
        private Int32 FLedgerNumber = -1;
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private Int32 FTransactionNumber = -1;
        private bool FActiveOnly = true;
        private string FTransactionCurrency = string.Empty;

        private decimal FDebitAmount = 0;
        private decimal FCreditAmount = 0;

        private ABatchRow FBatchRow = null;
        private AAccountTable FAccountList;
        private ACostCentreTable FCostCentreList;

        private GLSetupTDS FCacheDS = null;
        private GLBatchTDSAJournalRow FJournalRow = null;
        private ATransAnalAttribRow FPSAttributesRow = null;
        private TAnalysisAttributes FAnalysisAttributesLogic;

        private SourceGrid.Cells.Editors.ComboBox FcmbAnalAttribValues;

        private bool FShowStatusDialogOnLoad = true;

        private bool FIsUnposted = true;
        private string FBatchStatus = string.Empty;
        private string FJournalStatus = string.Empty;
        private bool FDoneComboInitialise = false;
        private bool FContainsSystemGenerated = false;

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
            dtpDetailTransactionDate.Validated += new EventHandler(ControlValidatedHandler);

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
        /// <param name="ABatchStatus"></param>
        /// <param name="AJournalStatus"></param>
        /// <returns>True if new GL transactions were loaded, false if transactions had been loaded already.</returns>
        public bool LoadTransactions(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            string ACurrencyCode,
            bool AFromBatchTab = false,
            string ABatchStatus = MFinanceConstants.BATCH_UNPOSTED,
            string AJournalStatus = MFinanceConstants.BATCH_UNPOSTED)
        {
            TFrmStatusDialog dlgStatus = null;
            bool DifferentBatchSelected = false;

            FLoadCompleted = false;
            FBatchRow = GetBatchRow();
            FJournalRow = GetJournalRow();
            FIsUnposted = (FBatchRow.BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

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
                    && (FMainDS.ATransaction.DefaultView.Count > 0)
                    && (FBatchStatus == ABatchStatus)
                    && (FJournalStatus == AJournalStatus))
                {
                    //Same as previously selected
                    if (FIsUnposted && (GetSelectedRowIndex() > 0))
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

                // A new ledger/batch
                DifferentBatchSelected = true;
                bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != ACurrencyCode);

                dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());

                if (FShowStatusDialogOnLoad == true)
                {
                    dlgStatus.Show();
                    FShowStatusDialogOnLoad = false;
                    dlgStatus.Heading = String.Format(Catalog.GetString("Batch {0}, Journal {1}"), ABatchNumber, AJournalNumber);
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

                FBatchStatus = ABatchStatus;
                FJournalStatus = AJournalStatus;

                // This sets the main part of the filter but excluding the additional items set by the user GUI
                // It gets the right sort order
                SetTransactionDefaultView();

                //Set the Analysis attributes filter as well
                FAnalysisAttributesLogic = new TAnalysisAttributes(FLedgerNumber, FBatchNumber, FJournalNumber);
                FAnalysisAttributesLogic.SetTransAnalAttributeDefaultView(FMainDS);
                FMainDS.ATransAnalAttrib.DefaultView.AllowNew = false;

                //Load from server if necessary
                if (FMainDS.ATransaction.DefaultView.Count == 0)
                {
                    dlgStatus.CurrentStatus = Catalog.GetString("Requesting transactions from server...");
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionATransAnalAttrib(ALedgerNumber, ABatchNumber, AJournalNumber));
                }
                else if (FMainDS.ATransAnalAttrib.DefaultView.Count == 0) // just in case transactions have been loaded in a separate process without analysis attributes
                {
                    dlgStatus.CurrentStatus = Catalog.GetString("Requesting analysis attributes from server...");
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttribForJournal(ALedgerNumber, ABatchNumber, AJournalNumber));
                }

                FContainsSystemGenerated = false;

                // check if any of the rows are system generated (i.e. reversals)
                foreach (DataRowView rv in FMainDS.ATransaction.DefaultView)
                {
                    if (((ATransactionRow)rv.Row).SystemGenerated)
                    {
                        FContainsSystemGenerated = true;
                        break;
                    }
                }

                // We need to call this because we have not called ShowData(), which would have set it.  This differs from the Gift screen.
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);

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
                    grdAnalAttributes.AddTextColumn(Catalog.GetString("Value"),
                        FMainDS.ATransAnalAttrib.Columns[ATransAnalAttribTable.GetAnalysisAttributeValueDBName()], 150,
                        FcmbAnalAttribValues);
                    FcmbAnalAttribValues.Control.SelectedValueChanged += new EventHandler(this.AnalysisAttributeValueChanged);

                    grdAnalAttributes.Columns[0].Width = 99;
                }

                grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransAnalAttrib.DefaultView);
                grdAnalAttributes.SetHeaderTooltip(0, Catalog.GetString("Type"));
                grdAnalAttributes.SetHeaderTooltip(1, Catalog.GetString("Value"));

                // if this form is readonly or batch is posted, then we need all account and cost centre codes, because old codes might have been used
                bool ActiveOnly = (this.Enabled && FIsUnposted);

                if (requireControlSetup || (FActiveOnly != ActiveOnly))
                {
                    FActiveOnly = ActiveOnly;

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
                        true, false, ActiveOnly, false, ACurrencyCode, true);
                    TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, ActiveOnly, false);
                    FPetraUtilsObject.SuppressChangeDetection = prevSuppressChangeDetection;
                }

                UpdateTransactionTotals();
                grdDetails.ResumeLayout();
                FLoadCompleted = true;

                ShowData();
                SelectRowInGrid(1);
                ShowDetails(); //Needed because of how currency is handled

                UpdateRecordNumberDisplay();
                FFilterAndFindObject.SetRecordNumberDisplayProperties();

                //Check for missing analysis attributes and their values
                if (FIsUnposted && (grdDetails.Rows.Count > 1))
                {
                    string updatedTransactions = string.Empty;

                    dlgStatus.CurrentStatus = Catalog.GetString("Checking analysis attributes ...");

                    FAnalysisAttributesLogic.ReconcileTransAnalysisAttributes(FMainDS, FCacheDS, out updatedTransactions);

                    if (updatedTransactions.Length > 0)
                    {
                        //Remove trailing comma
                        updatedTransactions = updatedTransactions.Remove(updatedTransactions.Length - 2);
                        MessageBox.Show(String.Format(Catalog.GetString(
                                    "Analysis Attributes have been updated in transaction(s): {0}.{1}{1}Remeber to set their values before posting!"),
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
                string rowFilter = String.Format("{0}={1} AND {2}={3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    FJournalNumber);

                FMainDS.ATransaction.DefaultView.RowFilter = rowFilter;
                FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
                FFilterAndFindObject.CurrentActiveFilter = rowFilter;
                // We don't apply the filter yet!

                FMainDS.ATransaction.DefaultView.Sort = String.Format("{0} " + sort,
                    ATransactionTable.GetTransactionNumberDBName());
            }
        }

        private ABatchRow GetBatchRow()
        {
            return ((TFrmGLBatch)ParentForm).GetBatchControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private GLBatchTDSAJournalRow GetJournalRow()
        {
            return ((TFrmGLBatch)ParentForm).GetJournalsControl().GetSelectedDetailRow();
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (CreateNewATransaction())
            {
                pnlTransAnalysisAttributes.Enabled = true;
                btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase) && !FContainsSystemGenerated;

                //Needs to be called at end of addition process to process Analysis Attributes
                AccountCodeDetailChanged(null, null);
            }
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefJournalRow">this can be null; otherwise this is the journal that the transaction should belong to</param>
        public void NewRowManual(ref GLBatchTDSATransactionRow ANewRow, AJournalRow ARefJournalRow)
        {
            if (ARefJournalRow == null)
            {
                ARefJournalRow = FJournalRow;
            }

            ANewRow.LedgerNumber = ARefJournalRow.LedgerNumber;
            ANewRow.BatchNumber = ARefJournalRow.BatchNumber;
            ANewRow.JournalNumber = ARefJournalRow.JournalNumber;
            ANewRow.TransactionNumber = ++ARefJournalRow.LastTransactionNumber;
            ANewRow.TransactionDate = GetBatchRow().DateEffective;

            if (FPreviouslySelectedDetailRow != null)
            {
                ANewRow.CostCentreCode = FPreviouslySelectedDetailRow.CostCentreCode;

                // don't want these copied over if previous transaction was a reversal
                if (!FPreviouslySelectedDetailRow.SystemGenerated)
                {
                    ANewRow.Narrative = FPreviouslySelectedDetailRow.Narrative;
                    ANewRow.Reference = FPreviouslySelectedDetailRow.Reference;
                }
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
                txtBatchNumber.Text = FBatchRow.BatchNumber.ToString();
                txtJournalNumber.Text = FJournalNumber.ToString();

                lblTransactionCurrency.Text = String.Format(Catalog.GetString("{0} (Transaction Currency)"), TransactionCurrency);
                txtDebitAmount.CurrencyCode = TransactionCurrency;
                txtCreditAmount.CurrencyCode = TransactionCurrency;
                txtCreditTotalAmount.CurrencyCode = TransactionCurrency;
                txtDebitTotalAmount.CurrencyCode = TransactionCurrency;

                lblBaseCurrency.Text = String.Format(Catalog.GetString("{0} (Base Currency)"), BaseCurrency);
                txtDebitAmountBase.CurrencyCode = BaseCurrency;
                txtCreditAmountBase.CurrencyCode = BaseCurrency;
                txtCreditTotalAmountBase.CurrencyCode = BaseCurrency;
                txtDebitTotalAmountBase.CurrencyCode = BaseCurrency;

                // foreign currency accounts only get transactions in that currency
                if (FTransactionCurrency != TransactionCurrency)
                {
                    string SelectedAccount = cmbDetailAccountCode.GetSelectedString();

                    // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
                    bool ActiveOnly = this.Enabled;

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, ActiveOnly, false, TransactionCurrency);

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount);

                    FTransactionCurrency = TransactionCurrency;

                    if (!FIsUnposted && FPetraUtilsObject.HasChanges)
                    {
                        FPetraUtilsObject.DisableSaveButton();
                    }
                }
            }
        }

        private void ShowDetailsManual(ATransactionRow ARow)
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
                txtDebitAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
                txtCreditAmountBase.NumberValueDecimal = 0;
            }
            else
            {
                txtDebitAmount.NumberValueDecimal = 0;
                txtCreditAmount.NumberValueDecimal = ARow.TransactionAmount;
                txtDebitAmountBase.NumberValueDecimal = 0;
                txtCreditAmountBase.NumberValueDecimal = ARow.AmountInBaseCurrency;
            }

            if (FPetraUtilsObject.HasChanges && !FIsUnposted)
            {
                FPetraUtilsObject.DisableSaveButton();
            }

            RefreshAnalysisAttributesGrid();
            UpdateChangeableStatus();
        }

        private void RefreshAnalysisAttributesGrid()
        {
            //Empty the grid
            FMainDS.ATransAnalAttrib.DefaultView.RowFilter = "1=2";
            FPSAttributesRow = null;

            if ((FPreviouslySelectedDetailRow == null)
                || (!pnlTransAnalysisAttributes.Enabled && FIsUnposted)
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

            FAnalysisAttributesLogic.SetTransAnalAttributeDefaultView(FMainDS, FTransactionNumber);

            grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransAnalAttrib.DefaultView);

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

            if ((TAnalysisAttributes.GetSelectedAttributeRow(grdAnalAttributes) == null)
                || (FPSAttributesRow == TAnalysisAttributes.GetSelectedAttributeRow(grdAnalAttributes)))
            {
                return;
            }

            FPSAttributesRow = TAnalysisAttributes.GetSelectedAttributeRow(grdAnalAttributes);

            string currentAnalTypeCode = FPSAttributesRow.AnalysisTypeCode;

            if (FIsUnposted)
            {
                FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0}='{1}' AND {2}=true",
                    AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                    currentAnalTypeCode,
                    AFreeformAnalysisTable.GetActiveDBName());
            }
            else
            {
                FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                    currentAnalTypeCode);
            }

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
            if (!FIsUnposted)
            {
                return;
            }

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

        private void GetDetailDataFromControlsManual(ATransactionRow ARow)
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

        private void GetDataForAmountFields(ATransactionRow ARow)
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

            DataView TransDV = new DataView(FMainDS.ATransaction);

            if (AUpdateCurrentTransOnly)
            {
                AJournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    AJournalTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    AJournalTable.GetJournalNumberDBName(),
                    AJournalNumber);

                RetVal = true;
            }
            else if (AJournalNumber == 0)
            {
                AJournalDV.RowFilter = String.Format("{0}={1} And {2}='{3}'",
                    AJournalTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    AJournalTable.GetJournalStatusDBName(),
                    MFinanceConstants.BATCH_UNPOSTED);

                if (AJournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournalATransaction(ALedgerNumber, ABatchNumber));

                    if (AJournalDV.Count == 0)
                    {
                        return false;
                    }
                }

                TransDV.RowFilter = String.Format("{0}={1}",
                    ATransactionTable.GetBatchNumberDBName(),
                    ABatchNumber);

                if (TransDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransactionForBatch(ALedgerNumber, ABatchNumber));
                }

                //As long as transactions exist, return true
                RetVal = true;
            }
            else
            {
                AJournalDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    AJournalTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    AJournalTable.GetJournalNumberDBName(),
                    AJournalNumber);

                if (AJournalDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadAJournal(ALedgerNumber, ABatchNumber, AJournalNumber));

                    if (AJournalDV.Count == 0)
                    {
                        return false;
                    }
                }

                TransDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    AJournalNumber);

                if (TransDV.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransaction(ALedgerNumber, ABatchNumber, AJournalNumber));
                }

                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        ///  update amount in other currencies (optional) and recalculate all totals for current batch and journal
        /// </summary>
        /// <param name="AUpdateLevel"></param>
        /// <param name="AUpdateTransDates"></param>
        public void UpdateTransactionTotals(TFrmGLBatch.eGLLevel AUpdateLevel = TFrmGLBatch.eGLLevel.Transaction, bool AUpdateTransDates = false)
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
            decimal AmtInIntlCurrency = 0.0M;

            string LedgerBaseCurrency = string.Empty;
            string LedgerIntlCurrency = string.Empty;
            decimal IntlRateToBaseCurrency = 0;
            bool IsTransactionInIntlCurrency = false;
            int LedgerNumber = 0;
            int CurrentBatchNumber = 0;
            int CurrentJournalNumber = 0;

            DataView JournalsToUpdateDV = null;
            DataView TransactionsToUpdateDV = null;

            bool BatchLevel = (AUpdateLevel == TFrmGLBatch.eGLLevel.Batch);
            bool JournalLevel = (AUpdateLevel == TFrmGLBatch.eGLLevel.Journal);
            bool TransLevel = (AUpdateLevel == TFrmGLBatch.eGLLevel.Transaction);

            if (AUpdateLevel == TFrmGLBatch.eGLLevel.Analysis)
            {
                TLogging.Log(String.Format("{0} called with wrong first argument!", Utilities.GetMethodSignature()));
                return;
            }

            ABatchRow CurrentBatchRow = GetBatchRow();
            AJournalRow CurrentJournalRow = null;

            if ((CurrentBatchRow == null)
                || (CurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return;
            }

            //Set inital values after confirming not null
            OriginalSaveButtonState = FPetraUtilsObject.HasChanges;
            LedgerBaseCurrency = FMainDS.ALedger[0].BaseCurrency;
            LedgerIntlCurrency = FMainDS.ALedger[0].IntlCurrency;
            LedgerNumber = CurrentBatchRow.LedgerNumber;
            CurrentBatchNumber = CurrentBatchRow.BatchNumber;

            JournalsToUpdateDV = new DataView(FMainDS.AJournal);
            TransactionsToUpdateDV = new DataView(FMainDS.ATransaction);

            //If called at the batch level, clear the current selections
            if (BatchLevel)
            {
                CurrentJournalNumber = 0;

                FPetraUtilsObject.SuppressChangeDetection = true;
                ClearCurrentSelection();
                ((TFrmGLBatch) this.ParentForm).GetJournalsControl().ClearCurrentSelection();
                FPetraUtilsObject.SuppressChangeDetection = false;
                //Ensure that when the Journal and Trans tab is opened, the data is reloaded.
                FBatchNumber = -1;
                ((TFrmGLBatch) this.ParentForm).GetJournalsControl().FBatchNumber = -1;
            }
            else
            {
                CurrentJournalRow = GetJournalRow();
                CurrentJournalNumber = CurrentJournalRow.JournalNumber;
            }

            if (JournalLevel)
            {
                ClearCurrentSelection();
                //Ensure that when the Trans tab is opened, the data is reloaded.
                FBatchNumber = -1;
            }
            else if (TransLevel && (FPreviouslySelectedDetailRow != null))
            {
                TransactionRowActive = true;

                CurrentTransBatchNumber = FPreviouslySelectedDetailRow.BatchNumber;
                CurrentTransJournalNumber = FPreviouslySelectedDetailRow.JournalNumber;
                CurrentTransNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            }

            //Get the corporate exchange rate
            ((TFrmGLBatch) this.ParentForm).WarnAboutMissingIntlExchangeRate = false;
            IntlRateToBaseCurrency = ((TFrmGLBatch) this.ParentForm).GetInternationalCurrencyExchangeRate();

            if (!EnsureGLDataPresent(LedgerNumber, CurrentBatchNumber, CurrentJournalNumber, ref JournalsToUpdateDV, TransactionRowActive))
            {
                //No transactions exist to process or corporate exchange rate not found
                return;
            }

            //Iterate through journals
            foreach (DataRowView drv in JournalsToUpdateDV)
            {
                GLBatchTDSAJournalRow jr = (GLBatchTDSAJournalRow)drv.Row;

                IsTransactionInIntlCurrency = (jr.TransactionCurrency == LedgerIntlCurrency);

                if (BatchLevel)
                {
                    //Journal row is active
                    if (jr.DateEffective != CurrentBatchRow.DateEffective)
                    {
                        jr.DateEffective = CurrentBatchRow.DateEffective;
                    }
                }

                TransactionsToUpdateDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    jr.BatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    jr.JournalNumber);

                //If all rows deleted
                if (TransactionsToUpdateDV.Count == 0)
                {
                    if ((txtCreditAmountBase.NumberValueDecimal != 0)
                        || (txtCreditAmount.NumberValueDecimal != 0)
                        || (txtDebitAmountBase.NumberValueDecimal != 0)
                        || (txtDebitAmount.NumberValueDecimal != 0)
                        || (txtCreditTotalAmount.NumberValueDecimal != 0)
                        || (txtDebitTotalAmount.NumberValueDecimal != 0)
                        || (txtCreditTotalAmountBase.NumberValueDecimal != 0)
                        || (txtDebitTotalAmountBase.NumberValueDecimal != 0))
                    {
                        txtCreditAmountBase.NumberValueDecimal = 0;
                        txtCreditAmount.NumberValueDecimal = 0;
                        txtDebitAmountBase.NumberValueDecimal = 0;
                        txtDebitAmount.NumberValueDecimal = 0;
                        txtCreditTotalAmount.NumberValueDecimal = 0;
                        txtDebitTotalAmount.NumberValueDecimal = 0;
                        txtCreditTotalAmountBase.NumberValueDecimal = 0;
                        txtDebitTotalAmountBase.NumberValueDecimal = 0;
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
                                                    && Convert.ToInt32(drWork[ATransactionTable.ColumnTransactionNumberId]) == CurrentTransNumber
                                                    && Convert.ToInt32(drWork[ATransactionTable.ColumnBatchNumberId]) == CurrentTransBatchNumber
                                                    && Convert.ToInt32(drWork[ATransactionTable.ColumnJournalNumberId]) == CurrentTransJournalNumber);

                    if (AUpdateTransDates
                        && (Convert.ToDateTime(drWork[ATransactionTable.ColumnTransactionDateId]) != CurrentBatchRow.DateEffective))
                    {
                        //TODO: add if
                        drWork[ATransactionTable.ColumnTransactionDateId] = CurrentBatchRow.DateEffective;

                        TransactionDataChanged = true;
                    }

                    if (IsCurrentActiveTransRow)
                    {
                        if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                        {
                            if ((txtCreditAmountBase.NumberValueDecimal != 0)
                                || (txtCreditAmount.NumberValueDecimal != 0)
                                || (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtDebitAmount.NumberValueDecimal)))
                            {
                                txtCreditAmountBase.NumberValueDecimal = 0;
                                txtCreditAmount.NumberValueDecimal = 0;
                                FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtDebitAmount.NumberValueDecimal);
                            }
                        }
                        else
                        {
                            if ((txtDebitAmountBase.NumberValueDecimal != 0)
                                || (txtDebitAmount.NumberValueDecimal != 0)
                                || (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtCreditAmount.NumberValueDecimal)))
                            {
                                txtDebitAmountBase.NumberValueDecimal = 0;
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
                                drWork[ATransactionTable.ColumnTransactionAmountId]), jr.ExchangeRateToBase);

                        if (AmtInBaseCurrency != Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]))
                        {
                            //TODO: add if
                            drWork[ATransactionTable.ColumnAmountInBaseCurrencyId] = AmtInBaseCurrency;

                            TransactionDataChanged = true;
                        }

                        if (IsTransactionInIntlCurrency)
                        {
                            if (Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]) !=
                                Convert.ToDecimal(drWork[ATransactionTable.ColumnTransactionAmountId]))
                            {
                                drWork[ATransactionTable.ColumnAmountInIntlCurrencyId] = drWork[ATransactionTable.ColumnTransactionAmountId];

                                TransactionDataChanged = true;
                            }
                        }
                        else
                        {
                            // TODO: Instead of hard coding the number of decimals to 2 (for US cent) it should come from the database.
                            AmtInIntlCurrency =
                                (IntlRateToBaseCurrency ==
                                 0) ? 0 : GLRoutines.Divide(Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]),
                                    IntlRateToBaseCurrency,
                                    2);

                            if (Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]) != AmtInIntlCurrency)
                            {
                                drWork[ATransactionTable.ColumnAmountInIntlCurrencyId] = AmtInIntlCurrency;

                                TransactionDataChanged = true;
                            }
                        }
                    }

                    if (Convert.ToBoolean(drWork[ATransactionTable.ColumnDebitCreditIndicatorId]) == true)
                    {
                        AmtDebitTotal += Convert.ToDecimal(drWork[ATransactionTable.ColumnTransactionAmountId]);
                        AmtDebitTotalBase += Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);

                        if (IsCurrentActiveTransRow)
                        {
                            if ((FPreviouslySelectedDetailRow.AmountInBaseCurrency !=
                                 Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]))
                                || (FPreviouslySelectedDetailRow.AmountInIntlCurrency !=
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]))
                                || (txtDebitAmountBase.NumberValueDecimal != Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]))
                                || (txtCreditAmountBase.NumberValueDecimal != 0))
                            {
                                FPreviouslySelectedDetailRow.AmountInBaseCurrency =
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);
                                FPreviouslySelectedDetailRow.AmountInIntlCurrency =
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]);
                                txtDebitAmountBase.NumberValueDecimal = Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);
                                txtCreditAmountBase.NumberValueDecimal = 0;
                            }
                        }
                    }
                    else
                    {
                        AmtCreditTotal += Convert.ToDecimal(drWork[ATransactionTable.ColumnTransactionAmountId]);
                        AmtCreditTotalBase += Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);

                        if (IsCurrentActiveTransRow)
                        {
                            if ((FPreviouslySelectedDetailRow.AmountInBaseCurrency !=
                                 Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]))
                                || (FPreviouslySelectedDetailRow.AmountInIntlCurrency !=
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]))
                                || (txtCreditAmountBase.NumberValueDecimal !=
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]))
                                || (txtDebitAmountBase.NumberValueDecimal != 0))
                            {
                                FPreviouslySelectedDetailRow.AmountInBaseCurrency =
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);
                                FPreviouslySelectedDetailRow.AmountInIntlCurrency =
                                    Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInIntlCurrencyId]);
                                txtCreditAmountBase.NumberValueDecimal = Convert.ToDecimal(drWork[ATransactionTable.ColumnAmountInBaseCurrencyId]);
                                txtDebitAmountBase.NumberValueDecimal = 0;
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
                    FMainDS.ATransaction.Merge(dtWork);
                    JournalDataChanged = true;
                }

                if (TransactionRowActive
                    && (jr.BatchNumber == CurrentTransBatchNumber)
                    && (jr.JournalNumber == CurrentTransJournalNumber)
                    && ((txtCreditTotalAmount.NumberValueDecimal != AmtCreditTotal)
                        || (txtDebitTotalAmount.NumberValueDecimal != AmtDebitTotal)
                        || (txtCreditTotalAmountBase.NumberValueDecimal != AmtCreditTotalBase)
                        || (txtDebitTotalAmountBase.NumberValueDecimal != AmtDebitTotalBase)))
                {
                    txtCreditTotalAmount.NumberValueDecimal = AmtCreditTotal;
                    txtDebitTotalAmount.NumberValueDecimal = AmtDebitTotal;
                    txtCreditTotalAmountBase.NumberValueDecimal = AmtCreditTotalBase;
                    txtDebitTotalAmountBase.NumberValueDecimal = AmtDebitTotalBase;
                }
            }   // next journal

            //Update totals of Batch
            if (FLoadCompleted)
            {
                GLRoutines.UpdateTotalsOfBatch(ref FMainDS, CurrentBatchRow);
            }
            else
            {
                //In trans loading
                txtCreditTotalAmount.NumberValueDecimal = AmtCreditTotal;
                txtDebitTotalAmount.NumberValueDecimal = AmtDebitTotal;
                txtCreditTotalAmountBase.NumberValueDecimal = AmtCreditTotalBase;
                txtDebitTotalAmountBase.NumberValueDecimal = AmtDebitTotalBase;
            }

            // refresh the currency symbols
            if (TransactionRowActive)
            {
                ShowDataManual();
            }

            if (!JournalDataChanged && (OriginalSaveButtonState != FPetraUtilsObject.HasChanges))
            {
                ((TFrmGLBatch)ParentForm).SaveChanges();
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
                string accountCode = row[ATransactionTable.ColumnAccountCodeId].ToString();
                return !AccountIsActive(accountCode);
            };

            SourceGrid.Conditions.ConditionView conditionCostCentreCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionCostCentreCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string costCentreCode = row[ATransactionTable.ColumnCostCentreCodeId].ToString();
                return !CostCentreIsActive(costCentreCode);
            };

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 2;
            int indexOfAccountCodeDataColumn = 3;

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
                string analysisCode = row2[ATransAnalAttribTable.ColumnAnalysisTypeCodeId].ToString();
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
                    string analysisCode = row2[ATransAnalAttribTable.ColumnAnalysisTypeCodeId].ToString();
                    string analysisAttributeValue = row2[ATransAnalAttribTable.ColumnAnalysisAttributeValueId].ToString();
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

            if (!AccountExists && (AAccountCode.Length > 0))
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

            if (!CostCentreExists && (ACostCentreCode.Length > 0))
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
            bool changeable = !FPetraUtilsObject.DetailProtectedMode
                              && (GetBatchRow() != null)
                              && (FIsUnposted)
                              && (FJournalRow.JournalStatus == MFinanceConstants.BATCH_UNPOSTED);
            bool canDeleteAll = (FFilterAndFindObject.IsActiveFilterEqualToBase && !FContainsSystemGenerated);
            bool rowsInGrid = (grdDetails.Rows.Count > 1);

            // pnlDetailsProtected must be changed first: when the enabled property of the control is changed, the focus changes, which triggers validation
            pnlDetailsProtected = !changeable;
            pnlDetails.Enabled = (changeable && rowsInGrid);
            btnDeleteAll.Enabled = (changeable && canDeleteAll && rowsInGrid);
            pnlTransAnalysisAttributes.Enabled = changeable && (FPreviouslySelectedDetailRow == null || !FPreviouslySelectedDetailRow.SystemGenerated);
            //lblAnalAttributes.Enabled = (changeable && rowsInGrid);
            btnNew.Enabled = changeable;

            // If transaction is a reversal then we want to only have the Transaction date control enabled.
            // Only run this code if the journal contains at least one reversal transaction
            // or there are no reversals but controls are disabled (from viewing previous batch) and need enabled
            if (FContainsSystemGenerated || !cmbDetailCostCentreCode.Enabled)
            {
                foreach (Control cont in pnlDetails.Controls)
                {
                    if ((cont.Name != dtpDetailTransactionDate.Name) && (cont.Name != lblDetailTransactionDate.Name))
                    {
                        cont.Enabled = (FPreviouslySelectedDetailRow != null && !FPreviouslySelectedDetailRow.SystemGenerated)
                                       || (FPreviouslySelectedDetailRow == null && changeable);
                    }
                    else
                    {
                        cont.Enabled = changeable;
                    }
                }
            }
        }

        private void DeleteAllTrans(System.Object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow == null) || !FIsUnposted)
            {
                return;
            }
            else if (!FFilterAndFindObject.IsActiveFilterEqualToBase)
            {
                MessageBox.Show(Catalog.GetString("Please remove the filter before attempting to delete all transactions in this journal."),
                    Catalog.GetString("Delete All Transactions"));

                return;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all transactions in this Journal ({0}).\n\nDo you really want to continue?"),
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
                    DataView TransDV = new DataView(FMainDS.ATransaction);
                    DataView TransAttribDV = new DataView(FMainDS.ATransAnalAttrib);

                    TransDV.AllowDelete = true;
                    TransAttribDV.AllowDelete = true;

                    TransDV.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        FJournalNumber);

                    TransDV.Sort = String.Format("{0} ASC",
                        ATransactionTable.GetTransactionNumberDBName());

                    TransAttribDV.RowFilter = String.Format("{0}={1} AND {2}={3}",
                        ATransactionTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ATransactionTable.GetJournalNumberDBName(),
                        FJournalNumber);

                    TransAttribDV.Sort = String.Format("{0} ASC, {1} ASC",
                        ATransAnalAttribTable.GetTransactionNumberDBName(),
                        ATransAnalAttribTable.GetAnalysisTypeCodeDBName());

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
                    if (((TFrmGLBatch)ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The journal has been cleared successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UpdateTransactionTotals();
                        ((TFrmGLBatch)ParentForm).SaveChanges();
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

        private bool PreDeleteManual(ATransactionRow ARowToDelete, ref string ADeletionQuestion)
        {
            bool allowDeletion = true;

            if (FPreviouslySelectedDetailRow != null)
            {
                if (ARowToDelete.SystemGenerated)
                {
                    MessageBox.Show(string.Format(Catalog.GetString(
                                "Transaction {0} cannot be deleted as it is a system generated transaction"), ARowToDelete.TransactionNumber),
                        Catalog.GetString("Delete Transaction"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                ADeletionQuestion = String.Format(Catalog.GetString("Are you sure you want to delete transaction no. {0} from Journal {1}?"),
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
        private void PostDeleteManual(ATransactionRow ARowToDelete,
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

                ((TFrmGLBatch) this.ParentForm).SaveChanges();

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
        private bool DeleteRowManual(ATransactionRow ARowToDelete, ref string ACompletionMessage)
        {
            //Assign a default values
            bool DeletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return DeletionSuccessful;
            }

            bool RowToDeleteIsNew = (ARowToDelete.RowState == DataRowState.Added);

            if (!RowToDeleteIsNew && !((TFrmGLBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current transaction!");
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
                    FMainDS.ATransAnalAttrib.Clear();
                    FMainDS.ATransaction.Clear();
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.ProcessTransAndAttributesForDeletion(TempDS, FLedgerNumber, FBatchNumber,
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
                DataView attributesDV = new DataView(FMainDS.ATransAnalAttrib);
                attributesDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumber);

                foreach (DataRowView attrDRV in attributesDV)
                {
                    ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                    attrRow.Delete();
                }

                DataView transactionsDV = new DataView(FMainDS.ATransaction);
                transactionsDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ATransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ATransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumber);

                foreach (DataRowView transDRV in transactionsDV)
                {
                    ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
                    tranRow.Delete();
                }

                //Renumber the transactions and attributes in TempDS
                DataView attributesDV2 = new DataView(FMainDS.ATransAnalAttrib);
                attributesDV2.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>{5}",
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    ATransactionNumber);
                attributesDV2.Sort = String.Format("{0} ASC", ATransAnalAttribTable.GetTransactionNumberDBName());

                foreach (DataRowView attrDRV in attributesDV2)
                {
                    ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                    attrRow.TransactionNumber--;
                }

                DataView transactionsDV2 = new DataView(FMainDS.ATransaction);
                transactionsDV2.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>{5}",
                    ATransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ATransactionTable.GetTransactionNumberDBName(),
                    ATransactionNumber);
                transactionsDV2.Sort = String.Format("{0} ASC", ATransactionTable.GetTransactionNumberDBName());

                foreach (DataRowView transDRV in transactionsDV2)
                {
                    ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
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
            txtDebitAmountBase.NumberValueDecimal = 0M;
            txtCreditAmountBase.NumberValueDecimal = 0M;
            //Refresh grids
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
                FAnalysisAttributesLogic.TransAnalAttrRequiredUpdating(FMainDS, FCacheDS,
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

        private void ValidateDataDetailsManual(ATransactionRow ARow)
        {
            if ((ARow == null) || (GetBatchRow() == null) || !FIsUnposted)
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

            TSharedFinanceValidation_GL.ValidateGLDetailManual(this, FBatchRow, ARow, controlToPass, ref VerificationResultCollection,
                FValidationControlsDict);

            if ((FPreviouslySelectedDetailRow != null)
                && !FAnalysisAttributesLogic.AccountAnalysisAttributeCountIsCorrect(
                    FPreviouslySelectedDetailRow.TransactionNumber,
                    FPreviouslySelectedDetailRow.AccountCode,
                    FMainDS,
                    FIsUnposted))
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnAccountCodeId];
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
                && !FAnalysisAttributesLogic.AccountAnalysisAttributesValuesExist(
                    FPreviouslySelectedDetailRow.TransactionNumber,
                    FPreviouslySelectedDetailRow.AccountCode,
                    FMainDS,
                    out ValueRequiredForType,
                    FIsUnposted))
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnAccountCodeId];
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
            grdDetails.DataSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(DataSource_ListChanged);
        }

        private void DataSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (grdDetails.CanFocus && (grdDetails.Rows.Count > 1))
            {
                grdDetails.AutoResizeGrid();
            }

            // If the grid list changes we might need to disable the Delete All button
            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase) && !FContainsSystemGenerated;
        }

        private void TransDateChanged(object sender, EventArgs e)
        {
            if ((FPetraUtilsObject == null) || FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            try
            {
                DateTime dateValue;

                string aDate = dtpDetailTransactionDate.Date.ToString();

                if (!DateTime.TryParse(aDate, out dateValue))
                {
                    dtpDetailTransactionDate.Date = GetBatchRow().DateEffective;
                }
            }
            catch
            {
                //Do nothing
            }
        }

        /// <summary>
        /// Cancel any changes made to this form
        /// </summary>
        public void CancelChangesToFixedBatches()
        {
            if ((GetBatchRow() != null) && (GetBatchRow().BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                DataView transDV = new DataView(FMainDS.ATransaction);
                transDV.RowFilter = string.Format("{0}={1}",
                    ATransactionTable.GetBatchNumberDBName(),
                    GetBatchRow().BatchNumber);

                foreach (DataRowView drv in transDV)
                {
                    ATransactionRow tr = (ATransactionRow)drv.Row;
                    tr.RejectChanges();
                }
            }
        }

        private void ImportTransactionsFromFile(object sender, EventArgs e)
        {
            if (ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                ((TFrmGLBatch)ParentForm).GetBatchControl().ImportTransactions(TUC_GLBatches_Import.TImportDataSourceEnum.FromFile);
                // The import method refreshes the screen if the import is successful
            }
        }

        private void ImportTransactionsFromClipboard(object sender, EventArgs e)
        {
            if (ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                ((TFrmGLBatch)ParentForm).GetBatchControl().ImportTransactions(TUC_GLBatches_Import.TImportDataSourceEnum.FromClipboard);
                // The import method refreshes the screen if the import is successful
            }
        }

        /// <summary>
        /// Select a row in the grid
        ///   Called from outside
        /// </summary>
        /// <param name="ARowNumber"></param>
        public void SelectRow(int ARowNumber)
        {
            SelectRowInGrid(ARowNumber);
            UpdateRecordNumberDisplay();
        }

        private void FilterToggledManual(bool AFilterIsOff)
        {
            // The first time the filter is toggled on we need to set up the cost centre and account comboBoxes
            // This means showing inactive values in red
            // We achieve this by using our own owner draw mode event
            // Also the data source for the combos will be wrong because they have been cloned from items that may not have shown inactive values
            if ((AFilterIsOff == false) && !FDoneComboInitialise)
            {
                InitFilterFindComboBox((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbDetailAccountCode"),
                    TCacheableFinanceTablesEnum.AccountList);
                InitFilterFindComboBox((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbDetailCostCentreCode"),
                    TCacheableFinanceTablesEnum.CostCentreList);
                InitFilterFindComboBox((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbDetailAccountCode"),
                    TCacheableFinanceTablesEnum.AccountList);
                InitFilterFindComboBox((TCmbAutoComplete)FFilterAndFindObject.FindPanelControls.FindControlByName("cmbDetailCostCentreCode"),
                    TCacheableFinanceTablesEnum.CostCentreList);

                FDoneComboInitialise = true;
            }
        }

        /// <summary>
        /// Helper method that we can call to initialise each of the filter/find comboBoxes
        /// </summary>
        private void InitFilterFindComboBox(TCmbAutoComplete AFFInstance, TCacheableFinanceTablesEnum AListTable)
        {
            AFFInstance.DataSource = TDataCache.TMFinance.GetCacheableFinanceTable(AListTable, FLedgerNumber).DefaultView;
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
                brush = CostCentreIsActive(content) ? Brushes.Black : Brushes.Red;
            }
            else if (cmb.Name.StartsWith("cmbDetailAccount"))
            {
                brush = AccountIsActive(content) ? Brushes.Black : Brushes.Red;
            }
            else
            {
                throw new ArgumentException("Unexpected caller of DrawComboBoxItem event");
            }

            Font font = new Font(((Control)sender).Font, (brush == Brushes.Red) ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(content, font, brush, new PointF(e.Bounds.X, e.Bounds.Y));
        }

        /// <summary>
        /// Select a special transaction number from outside
        /// </summary>
        /// <param name="ATransactionNumber"></param>
        /// <returns>True if the record is displayed in the grid, False otherwise</returns>
        public void SelectTransactionNumber(Int32 ATransactionNumber)
        {
            DataView myView = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView;

            for (int counter = 0; (counter < myView.Count); counter++)
            {
                int myViewTransactionNumber = (int)myView[counter]["a_transaction_number_i"];

                if (myViewTransactionNumber == ATransactionNumber)
                {
                    SelectRowInGrid(counter + 1);
                    break;
                }
            }
        }
    }
}