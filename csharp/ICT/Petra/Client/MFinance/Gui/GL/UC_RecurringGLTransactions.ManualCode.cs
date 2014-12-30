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
        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        private GLSetupTDS FCacheDS;
        private GLBatchTDSARecurringJournalRow FJournalRow = null;
        private ARecurringTransAnalAttribRow FPSAttributesRow = null;

        private SourceGrid.Cells.Editors.ComboBox FcmbAnalAttribValues;

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
            bool DifferentBatchSelected = false;
            bool HadChangesFromTheStart = FPetraUtilsObject.HasChanges;

            FLoadCompleted = false;
            FBatchRow = GetBatchRow();
            FJournalRow = GetJournalRow();

            if (FLedgerNumber == -1)
            {
                InitialiseControls();
            }

            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionCurrency == ACurrencyCode) && (FMainDS.ARecurringTransaction.DefaultView.Count > 0))
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
            }
            else
            {
                // Different Transactions
                DifferentBatchSelected = true;
                bool requireControlSetup = (FLedgerNumber == -1) || (FTransactionCurrency != ACurrencyCode);

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

                //Load from server if necessary
                if (FMainDS.ARecurringTransaction.DefaultView.Count == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadARecurringTransactionARecurringTransAnalAttrib(ALedgerNumber, ABatchNumber,
                            AJournalNumber));
                }

                // We need to call this because we have not called ShowData(), which would have set it.  This differs from the Gift screen.
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransaction.DefaultView);

                // Now we set the full filter
                FFilterAndFindObject.ApplyFilter();

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

                    grdAnalAttributes.Columns[0].Width = 100;
                }

                SetTransAnalAttributeDefaultView();
                FMainDS.ARecurringTransAnalAttrib.DefaultView.AllowNew = false;
                grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransAnalAttrib.DefaultView);

                //Always show active and inactive values
                if (requireControlSetup)
                {
                    //Load all analysis attribute values
                    if (FCacheDS == null)
                    {
                        FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, FActiveOnly);
                    }

                    SetupExtraGridFunctionality();

                    TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                        true, false, FActiveOnly, false, ACurrencyCode, true);
                    TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, FActiveOnly, false);
                }

                // This modifies the content of the text boxes that display the totals.  This is not a user change.
                bool prev = FPetraUtilsObject.SuppressChangeDetection;
                FPetraUtilsObject.SuppressChangeDetection = true;
                UpdateTransactionTotals();
                FPetraUtilsObject.SuppressChangeDetection = prev;

                grdDetails.ResumeLayout();
                FLoadCompleted = true;
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

            if (FPetraUtilsObject.HasChanges && !HadChangesFromTheStart)
            {
                // This means we updated the transaction totals.  We will save them behind the scenes since there were no other changes when we started
                ((TFrmRecurringGLBatch)ParentForm).SaveChanges();
                FMainDS.ARecurringTransaction.AcceptChanges();
            }

            ShowData();
            SelectRowInGrid(1);
            ShowDetails(); //Needed because of how currency is handled

            UpdateChangeableStatus();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();

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

        private void SetTransAnalAttributeDefaultView(Int32 ATransactionNumber = 0, String AAnalysisCodeFilterValues = "")
        {
            if (FBatchNumber != -1)
            {
                if (ATransactionNumber > 0)
                {
                    FMainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        FJournalNumber,
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        ATransactionNumber);
                }
                else
                {
                    FMainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} And {2}={3}",
                        ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                        FBatchNumber,
                        ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                        FJournalNumber);
                }

                FMainDS.ARecurringTransAnalAttrib.DefaultView.Sort = String.Format("{0} ASC, {1} ASC",
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName()
                    );
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

        private ARecurringTransAnalAttribRow GetSelectedAttributeRow()
        {
            DataRowView[] SelectedGridRow = grdAnalAttributes.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (ARecurringTransAnalAttribRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            this.CreateNewARecurringTransaction();

            ValidateAllData(true, false);

            pnlTransAnalysisAttributes.Enabled = true;
            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);

            //Needs to be called at end of addition process to process Analysis Attributes
            AccountCodeDetailChanged(null, null);
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

                    cmbDetailAccountCode.SetSelectedString(SelectedAccount);

                    FTransactionCurrency = TransactionCurrency;
                }

                // Needs to be called to process Analysis Attributes
                // AlanP: Not sure we need this? It already gets called.
                //AccountCodeDetailChanged(null, null);
            }
        }

        private void ShowDetailsManual(ARecurringTransactionRow ARow)
        {
            txtJournalNumber.Text = FJournalNumber.ToString();
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

            if (FPetraUtilsObject.HasChanges)
            {
                UpdateTransactionTotals();
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

            SetTransAnalAttributeDefaultView(FTransactionNumber);

            grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ARecurringTransAnalAttrib.DefaultView);

            if (grdAnalAttributes.Rows.Count > 1)
            {
                grdAnalAttributes.SelectRowWithoutFocus(1);
                AnalysisAttributesGrid_RowSelected(null, null);
            }
        }

        private bool AccountAnalysisAttributeCountIsCorrect()
        {
            bool RetVal = true;

            if (FPreviouslySelectedDetailRow == null)
            {
                return RetVal;
            }

            int TransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            string AccountCode = FPreviouslySelectedDetailRow.AccountCode;
            int NumberOfAttributes = 0;

            TRemote.MFinance.Setup.WebConnectors.AccountHasAnalysisAttributes(FLedgerNumber, AccountCode, out NumberOfAttributes, FActiveOnly);

            if (NumberOfAttributes == 0)
            {
                return RetVal;
            }

            DataView analAttrib = new DataView(FMainDS.ARecurringTransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                TransactionNumber,
                ARecurringTransAnalAttribTable.GetAccountCodeDBName(),
                AccountCode);

            RetVal = (analAttrib.Count == NumberOfAttributes);

            return RetVal;
        }

        private bool AccountAnalysisAttributesValuesExist(out String ValueRequiredForType)
        {
            ValueRequiredForType = "";

            if ((FPreviouslySelectedDetailRow == null) || (FMainDS.ARecurringTransAnalAttrib.DefaultView.Count == 0))
            {
                return true;
            }

            int TransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            string AccountCode = FPreviouslySelectedDetailRow.AccountCode;

            StringCollection RequiredAnalAttrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                AccountCode, FActiveOnly);

            string AnalysisCodeFilterValues = ConvertStringCollectionToCSV(RequiredAnalAttrCodes, "'");

            DataView analAttrib = new DataView(FMainDS.ARecurringTransAnalAttrib);

            analAttrib.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6} IN ({7})",
                ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                TransactionNumber,
                ARecurringTransAnalAttribTable.GetAnalysisTypeCodeDBName(),
                AnalysisCodeFilterValues);

            foreach (DataRowView drv in analAttrib)
            {
                ARecurringTransAnalAttribRow rw = (ARecurringTransAnalAttribRow)drv.Row;

                string analysisCode = rw.AnalysisTypeCode;

                if (TRemote.MFinance.Setup.WebConnectors.AccountAnalysisAttributeRequiresValues(FLedgerNumber, analysisCode, FActiveOnly))
                {
                    if (rw.IsAnalysisAttributeValueNull() || (rw.AnalysisAttributeValue == string.Empty))
                    {
                        ValueRequiredForType = rw.AnalysisTypeCode;
                        return false;
                    }
                }
            }

            return true;
        }

        private void AnalysisAttributesGrid_RowSelected(System.Object sender, RangeRegionChangedEventArgs e)
        {
            if (grdAnalAttributes.Selection.ActivePosition.IsEmpty() || (grdAnalAttributes.Selection.ActivePosition.Column == 0))
            {
                return;
            }

            if ((GetSelectedAttributeRow() == null) || (FPSAttributesRow == GetSelectedAttributeRow()))
            {
                return;
            }

            FPSAttributesRow = GetSelectedAttributeRow();

            string currentAnalTypeCode = FPSAttributesRow.AnalysisTypeCode;

            FCacheDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                currentAnalTypeCode);

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

            if (txtDebitAmount.Text.Length == 0)
            {
                txtDebitAmount.NumberValueDecimal = 0;
            }

            if (txtCreditAmount.Text.Length == 0)
            {
                txtCreditAmount.NumberValueDecimal = 0;
            }

            ARow.DebitCreditIndicator = (txtDebitAmount.NumberValueDecimal.Value > 0);

            if (ARow.DebitCreditIndicator)
            {
                ARow.TransactionAmount = Math.Abs(txtDebitAmount.NumberValueDecimal.Value);

                if (txtCreditAmount.NumberValueDecimal.Value != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }
            else
            {
                ARow.TransactionAmount = Math.Abs(txtCreditAmount.NumberValueDecimal.Value);
            }

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

        private void UpdateTransactionTotals()
        {
            if ((FJournalNumber == -1)
                || (FBatchRow == null)
                || (FJournalRow == null))
            {
                return;
            }

            bool AmountsChanged = false;

            if (FPreviouslySelectedDetailRow != null)
            {
                if (FPreviouslySelectedDetailRow.DebitCreditIndicator)
                {
                    if (txtCreditAmount.NumberValueDecimal != 0)
                    {
                        txtCreditAmount.NumberValueDecimal = 0;
                    }

                    if (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtDebitAmount.NumberValueDecimal))
                    {
                        FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtDebitAmount.NumberValueDecimal);
                        AmountsChanged = true;
                    }
                }
                else
                {
                    if (txtDebitAmount.NumberValueDecimal != 0)
                    {
                        txtDebitAmount.NumberValueDecimal = 0;
                    }

                    if (FPreviouslySelectedDetailRow.TransactionAmount != Convert.ToDecimal(txtCreditAmount.NumberValueDecimal))
                    {
                        FPreviouslySelectedDetailRow.TransactionAmount = Convert.ToDecimal(txtCreditAmount.NumberValueDecimal);
                        AmountsChanged = true;
                    }
                }
            }
            else
            {
                if (txtDebitAmount.NumberValueDecimal != 0)
                {
                    txtDebitAmount.NumberValueDecimal = 0;
                }

                if (txtCreditAmount.NumberValueDecimal != 0)
                {
                    txtCreditAmount.NumberValueDecimal = 0;
                }
            }

            if (AmountsChanged)
            {
                GLRoutines.UpdateTotalsOfRecurringBatch(ref FMainDS, FBatchRow);
                FPetraUtilsObject.SetChangedFlag();
            }

            txtCreditTotalAmount.NumberValueDecimal = FJournalRow.JournalCreditTotal;
            txtDebitTotalAmount.NumberValueDecimal = FJournalRow.JournalDebitTotal;

            // refresh the currency symbols
            ShowDataManual();
        }

        private void SetupExtraGridFunctionality()
        {
            //Populate CostCentreList variable
            DataTable costCentreList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref costCentreList, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)costCentreList;

            //Populate AccountList variable
            DataTable accountList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref accountList, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)accountList;
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
                return !AnalysisCodeIsActive(analysisCode);
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
                    return !AnalysisAttributeValueIsActive(analysisCode, analysisAttributeValue);
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

        private bool AnalysisCodeIsActive(String AAnalysisCode = "")
        {
            bool retVal = true;
            string accountCode = string.Empty;

            accountCode = cmbDetailAccountCode.GetSelectedString();

            if ((AAnalysisCode == string.Empty) || (accountCode == string.Empty))
            {
                return retVal;
            }

            DataView dv = new DataView(FCacheDS.AAnalysisAttribute);

            dv.RowFilter = String.Format("{0}={1} AND {2}='{3}' AND {4}='{5}' AND {6}=true",
                AAnalysisAttributeTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                accountCode,
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

            //Make sure the grid combobox has right font else it will adopt strikeout
            // for all items in the list.
            FcmbAnalAttribValues.Control.Font = new Font(FontFamily.GenericSansSerif, 8);

            return retVal;
        }

        private bool AccountIsActive(string AAccountCode = "")
        {
            bool retVal = true;

            AAccountRow currentAccountRow = null;

            //If empty, read value from combo
            if (AAccountCode == string.Empty)
            {
                if ((FAccountTable != null) && (cmbDetailAccountCode.SelectedIndex != -1) && (cmbDetailAccountCode.Count > 0)
                    && (cmbDetailAccountCode.GetSelectedString() != null))
                {
                    AAccountCode = cmbDetailAccountCode.GetSelectedString();
                }
            }

            if (FAccountTable != null)
            {
                currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });
            }

            if (currentAccountRow != null)
            {
                retVal = currentAccountRow.AccountActiveFlag;
            }

            return retVal;
        }

        private bool CostCentreIsActive(string ACostCentreCode = "")
        {
            bool retVal = true;

            ACostCentreRow currentCostCentreRow = null;

            //If empty, read value from combo
            if (ACostCentreCode == string.Empty)
            {
                if ((FCostCentreTable != null) && (cmbDetailCostCentreCode.SelectedIndex != -1) && (cmbDetailCostCentreCode.Count > 0)
                    && (cmbDetailCostCentreCode.GetSelectedString() != null))
                {
                    ACostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
                }
            }

            if (FCostCentreTable != null)
            {
                currentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });
            }

            if (currentCostCentreRow != null)
            {
                retVal = currentCostCentreRow.CostCentreActiveFlag;
            }

            return retVal;
        }

        private void ControlHasChanged(System.Object sender, EventArgs e)
        {
            int counter = FPetraUtilsObject.VerificationResultCollection.Count;

            if (sender.GetType() == typeof(TTxtCurrencyTextBox))
            {
                CheckAmounts((TTxtCurrencyTextBox)sender);
            }

            ControlValidatedHandler(sender, e);

            //If no errors
            if (FPetraUtilsObject.VerificationResultCollection.Count == counter)
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
        }

        private void DeleteAllTrans(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all transactions in this recurring Journal ({0}).\n\nDo you really want to continue?"),
                         FJournalNumber),
                     Catalog.GetString("Confirm Deletion"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
            {
                //Backup the Dataset for reversion purposes
                GLBatchTDS FTempDS = (GLBatchTDS)FMainDS.Copy();

                try
                {
                    //Unbind any transactions currently being editied in the Transaction Tab
                    ((TFrmRecurringGLBatch)ParentForm).GetTransactionsControl().ClearCurrentSelection();

                    //Delete transactions
                    SetTransAnalAttributeDefaultView();
                    SetTransactionDefaultView();

                    for (int i = FMainDS.ARecurringTransAnalAttrib.DefaultView.Count - 1; i >= 0; i--)
                    {
                        FMainDS.ARecurringTransAnalAttrib.DefaultView.Delete(i);
                    }

                    for (int i = FMainDS.ARecurringTransaction.DefaultView.Count - 1; i >= 0; i--)
                    {
                        FMainDS.ARecurringTransaction.DefaultView.Delete(i);
                    }

                    UpdateTransactionTotals();

                    // Be sure to set the last transaction number in the parent table before saving all the changes
                    SetJournalLastTransNumber();

                    FPetraUtilsObject.SetChangedFlag();

                    //Need to call save
                    if (((TFrmRecurringGLBatch)ParentForm).SaveChanges())
                    {
                        MessageBox.Show(Catalog.GetString("The recurring journal has been cleared successfully!"),
                            Catalog.GetString("Success"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // saving failed, therefore do not try to post
                        MessageBox.Show(Catalog.GetString(
                                "The journal has been cleared but there were problems during saving; ") + Environment.NewLine +
                            Catalog.GetString("Please try and save immediately."),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    FMainDS = (GLBatchTDS)FTempDS.Copy();
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
                SetJournalLastTransNumber();

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
            bool deletionSuccessful = false;

            ACompletionMessage = string.Empty;

            if (ARowToDelete == null)
            {
                return deletionSuccessful;
            }

            bool newRecord = (ARowToDelete.RowState == DataRowState.Added);

            if (!newRecord && !((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
            {
                MessageBox.Show("Error in trying to save prior to deleting current recurring transaction!");
                return deletionSuccessful;
            }

            //Backup the Dataset for reversion purposes
            GLBatchTDS FTempDS = (GLBatchTDS)FMainDS.Copy();

            int transactionNumberToDelete = ARowToDelete.TransactionNumber;
            int lastTransactionNumber = FJournalRow.LastTransactionNumber;

            try
            {
                // Delete on client side data through views that is already loaded. Data that is not
                // loaded yet will be deleted with cascading delete on server side so we don't have
                // to worry about this here.

                SetTransAnalAttributeDefaultView(transactionNumberToDelete);
                DataView attrView = FMainDS.ARecurringTransAnalAttrib.DefaultView;

                if (attrView.Count > 0)
                {
                    //Iterate through attributes and delete
                    ARecurringTransAnalAttribRow attrRowCurrent = null;

                    foreach (DataRowView gv in attrView)
                    {
                        attrRowCurrent = (ARecurringTransAnalAttribRow)gv.Row;

                        attrRowCurrent.Delete();
                    }
                }

                //Reduce those with higher transaction number by one
                attrView.RowFilter = String.Format("{0} = {1} AND {2} = {3} AND {4} > {5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    FJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    transactionNumberToDelete);

                // Delete the associated transaction analysis attributes
                //  if attributes do exist, and renumber those above
                if (attrView.Count > 0)
                {
                    //Iterate through higher number attributes and transaction numbers and reduce by one
                    ARecurringTransAnalAttribRow attrRowCurrent = null;

                    foreach (DataRowView gv in attrView)
                    {
                        attrRowCurrent = (ARecurringTransAnalAttribRow)gv.Row;

                        attrRowCurrent.TransactionNumber--;
                    }
                }

                //Bubble the transaction to delete to the top
                DataView transView = new DataView(FMainDS.ARecurringTransaction);
                transView.RowFilter = String.Format("{0}={1} And {2}={3}",
                    ARecurringTransactionTable.GetBatchNumberDBName(),
                    FBatchNumber,
                    ARecurringTransactionTable.GetJournalNumberDBName(),
                    FJournalNumber);

                transView.Sort = String.Format("{0} ASC",
                    ARecurringTransactionTable.GetTransactionNumberDBName());

                ARecurringTransactionRow transRowToReceive = null;
                ARecurringTransactionRow transRowToCopyDown = null;
                ARecurringTransactionRow transRowCurrent = null;

                int currentTransNo = 0;

                foreach (DataRowView gv in transView)
                {
                    transRowCurrent = (ARecurringTransactionRow)gv.Row;

                    currentTransNo = transRowCurrent.TransactionNumber;

                    if (currentTransNo > transactionNumberToDelete)
                    {
                        transRowToCopyDown = transRowCurrent;

                        //Copy column values down
                        for (int j = 4; j < transRowToCopyDown.Table.Columns.Count; j++)
                        {
                            //Update all columns except the pk fields that remain the same
                            if (!transRowToCopyDown.Table.Columns[j].ColumnName.EndsWith("_text"))
                            {
                                // Don't include the columns that the filter uses for numeric textual comparison
                                transRowToReceive[j] = transRowToCopyDown[j];
                            }
                        }
                    }

                    if (currentTransNo == transView.Count)                         //Last row which is the row to be deleted
                    {
                        //Mark last record for deletion
                        transRowCurrent.SubType = MFinanceConstants.MARKED_FOR_DELETION;
                    }

                    //transRowToReceive will become previous row for next recursion
                    transRowToReceive = transRowCurrent;
                }

                if (newRecord && (transRowCurrent.SubType == MFinanceConstants.MARKED_FOR_DELETION))
                {
                    transRowCurrent.Delete();
                }

                FPreviouslySelectedDetailRow = null;

                FPetraUtilsObject.SetChangedFlag();

                //Try to save changes
                if (!newRecord)
                {
                    if (!((TFrmRecurringGLBatch) this.ParentForm).SaveChanges())
                    {
                        throw new Exception("Unable to save after deleting a recurring transaction!");
                    }
                }

                ACompletionMessage = String.Format(Catalog.GetString("Recurring transaction no.: {0} deleted successfully."),
                    transactionNumberToDelete);

                deletionSuccessful = true;
            }
            catch (Exception ex)
            {
                ACompletionMessage = ex.Message;
                MessageBox.Show(ex.Message,
                    "Deletion Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                //Revert to previous state
                FMainDS = (GLBatchTDS)FTempDS.Copy();
            }
            finally
            {
                SetTransactionDefaultView();
                FFilterAndFindObject.ApplyFilter();
            }

            return deletionSuccessful;
        }

        private void SetJournalLastTransNumber()
        {
            string rowFilter = String.Format("{0}={1} And {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                FBatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                FJournalNumber);
            string sort = String.Format("{0} {1}", ARecurringTransactionTable.GetTransactionNumberDBName(), "DESC");
            DataView dv = new DataView(FMainDS.ARecurringTransaction, rowFilter, sort, DataViewRowState.CurrentRows);

            if (dv.Count > 0)
            {
                ARecurringTransactionRow transRow = (ARecurringTransactionRow)dv[0].Row;
                FJournalRow.LastTransactionNumber = transRow.TransactionNumber;
            }
            else
            {
                FJournalRow.LastTransactionNumber = 0;
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
            txtDebitAmount.NumberValueDecimal = 0;
            txtCreditAmount.NumberValueDecimal = 0;
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
                || (cmbDetailCostCentreCode.GetSelectedString() == String.Empty) || (cmbDetailCostCentreCode.SelectedIndex == -1))
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

            if ((FPreviouslySelectedDetailRow.TransactionNumber == FTransactionNumber) && (FTransactionNumber != -1))
            {
                ReconcileTransAnalysisAttributes();
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
            Int64 RecipientKey;

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

        // Need to ensure that the Analysis Attributes grid has all the entries
        // that are required for the selected account.
        // There may or may not already be attribute assignments for this transaction.
        private void ReconcileTransAnalysisAttributes()
        {
            string currentAccountCode = cmbDetailAccountCode.GetSelectedString();

            if ((FPreviouslySelectedDetailRow == null) || (currentAccountCode == null)
                || (currentAccountCode == string.Empty))
            {
                return;
            }

            StringCollection RequiredAnalattrCodes = TRemote.MFinance.Setup.WebConnectors.RequiredAnalysisAttributesForAccount(FLedgerNumber,
                currentAccountCode, false);
            Int32 currentTransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;

            SetTransAnalAttributeDefaultView(currentTransactionNumber);

            // If the AnalysisType list I'm currently using is the same as the list of required types, I can keep it (with any existing values).
            Boolean existingListIsOk = (RequiredAnalattrCodes.Count == FMainDS.ARecurringTransAnalAttrib.DefaultView.Count);

            if (existingListIsOk)
            {
                foreach (DataRowView rv in FMainDS.ARecurringTransAnalAttrib.DefaultView)
                {
                    ARecurringTransAnalAttribRow row = (ARecurringTransAnalAttribRow)rv.Row;

                    if (!RequiredAnalattrCodes.Contains(row.AnalysisTypeCode))
                    {
                        existingListIsOk = false;
                        break;
                    }
                }
            }

            if (existingListIsOk)
            {
                return;
            }

            // Delete any existing Analysis Type records and re-create the list (Removing any prior selections by the user).
            foreach (DataRowView rv in FMainDS.ARecurringTransAnalAttrib.DefaultView)
            {
                ARecurringTransAnalAttribRow attrRowCurrent = (ARecurringTransAnalAttribRow)rv.Row;
                attrRowCurrent.Delete();
            }

            foreach (String analysisTypeCode in RequiredAnalattrCodes)
            {
                ARecurringTransAnalAttribRow newRow = FMainDS.ARecurringTransAnalAttrib.NewRowTyped(true);
                newRow.LedgerNumber = FLedgerNumber;
                newRow.BatchNumber = FBatchNumber;
                newRow.JournalNumber = FJournalNumber;
                newRow.TransactionNumber = currentTransactionNumber;
                newRow.AnalysisTypeCode = analysisTypeCode;
                newRow.AccountCode = currentAccountCode;

                FMainDS.ARecurringTransAnalAttrib.Rows.Add(newRow);
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
            if (((txtDebitAmount.NumberValueDecimal.Value == 0) && (txtCreditAmount.NumberValueDecimal.Value == 0))
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

            if ((controlToPass == null) || (controlToPass == txtDetailReference))
            {
                //This is needed because the above runs many times during setting up the form
                VerificationResultCollection.Clear();
            }

            TSharedFinanceValidation_GL.ValidateRecurringGLDetailManual(this, FBatchRow, ARow, controlToPass, ref VerificationResultCollection,
                FValidationControlsDict);

            if (!AccountAnalysisAttributeCountIsCorrect())
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
                        "A value must be entered for 'Analysis Attributes' for Account Code {0} in Transaction {1}.{2}{2}" +
                        "CLICK THE DOWN ARROW NEXT TO THE ACCOUNT CODE BOX TO OPEN THE LIST AND THEN RESELECT ACCOUNT CODE {0}",
                        ARow.AccountCode,
                        ARow.TransactionNumber,
                        Environment.NewLine));
                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }

            String ValueRequiredForType;

            if (!AccountAnalysisAttributesValuesExist(out ValueRequiredForType))
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
            if ((grdDetails != null) && grdDetails.Enabled && grdDetails.TabStop)
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

        private string ConvertStringCollectionToCSV(StringCollection AStringCollection, string AWrapString = "")
        {
            string csvRetVal = string.Empty;

            int sizeCollection = AStringCollection.Count;

            if (sizeCollection > 0)
            {
                string[] allStrings = new string[sizeCollection];
                AStringCollection.CopyTo(allStrings, 0);

                csvRetVal = AWrapString + String.Join(AWrapString + ", " + AWrapString, allStrings) + AWrapString;
            }

            return csvRetVal;
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

            btnDeleteAll.Enabled = btnDelete.Enabled && (FFilterAndFindObject.IsActiveFilterEqualToBase);
        }
    }
}