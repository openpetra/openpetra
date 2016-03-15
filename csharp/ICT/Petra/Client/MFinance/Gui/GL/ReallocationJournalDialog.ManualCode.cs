//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using SourceGrid;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmReallocationJournalDialog
    {
        private Int32 FLedgerNumber = -1;
        private GLBatchTDSAJournalRow FJournal = null;
        private int FNextTransactionNumber = -1;
        private DataTable FAccountsCustomTable = null;
        private DataRow FPreviouslySelectedAccountsRow = null;
        private int FCurrentTransactionNumber = -1;

        // for analysis atributes
        private SourceGrid.Cells.Editors.ComboBox FcmbFromAnalAttribValues;
        private SourceGrid.Cells.Editors.ComboBox FcmbToAnalAttribValues;
        private ATransAnalAttribRow FPSAttributesRow = null;
        private GLSetupTDS FCacheDS = null;
        private GLBatchTDS FTempFromDS = new GLBatchTDS();
        private TAnalysisAttributes FAnalysisAttributesLogic;

        /// <summary>
        /// The Journal that this Reallocation will be added to.
        /// </summary>
        public GLBatchTDSAJournalRow Journal
        {
            set
            {
                FJournal = value;

                // set currency codes
                txtFromTransactionAmount.CurrencyCode = FJournal.TransactionCurrency;
                txtDetailTransactionAmount.CurrencyCode = FJournal.TransactionCurrency;
                txtFromTransactionAmount.NumberValueDecimal = 0.0m;

                if (FLedgerNumber != FJournal.LedgerNumber)
                {
                    FLedgerNumber = FJournal.LedgerNumber;
                    txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FJournal.LedgerNumber);

                    //Load all analysis attribute values
                    if (FCacheDS == null)
                    {
                        FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, true);
                    }

                    SetupGrdAccounts();

                    SetupComboboxes();
                }

                txtBatchNumber.Text = FJournal.BatchNumber.ToString();

                // LastTransactionNumber + 1 is reserved for 'from' Reallocation
                FNextTransactionNumber = FJournal.LastTransactionNumber + 2;

                FAnalysisAttributesLogic = new TAnalysisAttributes(FLedgerNumber, FJournal.BatchNumber, FJournal.JournalNumber);

                SetupAnalysisAttributeGrid(grdFromAnalAttributes, ref FcmbFromAnalAttribValues);
                SetupAnalysisAttributeGrid(grdToAnalAttributes, ref FcmbToAnalAttribValues);
            }
        }

        /// <summary>
        /// </summary>
        public GLBatchTDS MainDS
        {
            get
            {
                return FMainDS;
            }
        }

        #region Setup

        private void InitializeManualCode()
        {
            rbtFromPercentageOption.Checked = true;
            rbtPercentageOption.Checked = true;

            // disallow negative numbers
            txtFromTransactionAmount.NegativeValueAllowed = false;
            txtDetailTransactionAmount.NegativeValueAllowed = false;
            txtFromPercentage.NegativeValueAllowed = false;
            txtDetailPercentage.NegativeValueAllowed = false;

            // ok button disabled until at least one Reallocation is added
            btnOK.Enabled = false;
        }

        private void RunOnceOnActivationManual()
        {
            // Stop changes from ever being detected. We do not want to save the data on this screen.
            FPetraUtilsObject.DisableSaveButton();
            FPetraUtilsObject.UnhookControl(this, true);

            cmbDetailCostCentreCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_COST_CENTRE;
            cmbDetailAccountCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_ACCOUNT;
        }

        private void SetupGrdAccounts()
        {
            // get data from database
            FAccountsCustomTable = TRemote.MFinance.GL.WebConnectors.GetAccountsForReallocationJournal(FLedgerNumber, FJournal.JournalPeriod);

            if (FAccountsCustomTable.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("No current accounts have been found. This screen will now be closed."),
                    Catalog.GetString("Reallocation Journal"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            grdAccounts.Columns.Clear();
            grdAccounts.AddCurrencyColumn(
                Catalog.GetString("Current Balance"), FAccountsCustomTable.Columns[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()]);
            grdAccounts.AddTextColumn(
                Catalog.GetString("Cost Centre"), FAccountsCustomTable.Columns[AGeneralLedgerMasterTable.GetCostCentreCodeDBName()]);
            grdAccounts.AddTextColumn(
                Catalog.GetString("Account"), FAccountsCustomTable.Columns[AGeneralLedgerMasterTable.GetAccountCodeDBName()]);
            grdAccounts.AddTextColumn(
                Catalog.GetString("Description"), FAccountsCustomTable.Columns[AAccountTable.GetAccountCodeShortDescDBName()]);

            DataView myDataView = FAccountsCustomTable.DefaultView;
            myDataView.AllowNew = false;
            myDataView.Sort = AGeneralLedgerMasterTable.GetCostCentreCodeDBName() + " ASC";
            grdAccounts.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            grdAccounts.Selection.SelectionChanged += new SourceGrid.RangeRegionChangedEventHandler(grdAccounts_RowSelected);

            grdAccounts.SelectRowInGrid(0, true);
        }

        private void SetupComboboxes()
        {
            if (FLedgerNumber != -1)
            {
                FPetraUtilsObject.SuppressChangeDetection = true;

                // populate combo boxes
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, true, false);
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber,
                    true, false, true, false, FJournal.TransactionCurrency, true);
            }
        }

        private void SetupAnalysisAttributeGrid(TSgrdDataGrid AGrid, ref SourceGrid.Cells.Editors.ComboBox AGridCombo)
        {
            AGrid.DataSource = null;
            GLBatchTDS DS = null;
            SourceGrid.Cells.Editors.ComboBox ATempCombo = null;

            if (AGrid.Name == grdFromAnalAttributes.Name)
            {
                FTempFromDS = (GLBatchTDS)FMainDS.Clone();
                DS = FTempFromDS;
            }
            else
            {
                DS = FMainDS;
            }

            if (AGrid.Columns.Count == 0)
            {
                ATempCombo = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                ATempCombo.EnableEdit = true;
                ATempCombo.EditableMode = EditableMode.Focus;
                AGrid.Columns.Clear();
                AGrid.AddTextColumn(Catalog.GetString("Type"), FMainDS.ATransAnalAttrib.ColumnAnalysisTypeCode, 99);
                AGrid.AddTextColumn(Catalog.GetString("Value"),
                    DS.ATransAnalAttrib.Columns[ATransAnalAttribTable.GetAnalysisAttributeValueDBName()], 150,
                    ATempCombo);
            }

            FAnalysisAttributesLogic.SetTransAnalAttributeDefaultView(DS);
            DS.ATransAnalAttrib.DefaultView.AllowNew = false;
            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(DS.ATransAnalAttrib.DefaultView);
            AGrid.SetHeaderTooltip(0, Catalog.GetString("Type"));
            AGrid.SetHeaderTooltip(1, Catalog.GetString("Value"));

            AGrid.Selection.SelectionChanged += AnalysisAttributesGrid_RowSelected;

            //Prepare Analysis attributes grid to highlight inactive analysis codes
            // Create a cell view for special conditions
            SourceGrid.Cells.Views.Cell strikeoutCell2 = new SourceGrid.Cells.Views.Cell();
            strikeoutCell2.Font = new System.Drawing.Font(AGrid.Font, FontStyle.Strikeout);

            // Create a condition, apply the view when true, and assign a delegate to handle it
            SourceGrid.Conditions.ConditionView conditionAnalysisCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell2);
            conditionAnalysisCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column2, int gridRow2, object itemRow2)
            {
                DataRowView row2 = (DataRowView)itemRow2;
                string analysisCode = row2[ATransAnalAttribTable.ColumnAnalysisTypeCodeId].ToString();

                if (AGrid.Name == grdFromAnalAttributes.Name)
                {
                    return !FAnalysisAttributesLogic.AnalysisCodeIsActive(
                        FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString(), FCacheDS.AAnalysisAttribute,
                        analysisCode);
                }

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
                    return !TAnalysisAttributes.AnalysisAttributeValueIsActive(ref ATempCombo,
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

            AGrid.Columns[indexOfAnalysisCodeColumn].Conditions.Add(conditionAnalysisCodeActive);
            AGrid.Columns[indexOfAnalysisAttributeValueColumn].Conditions.Add(conditionAnalysisAttributeValueActive);

            AGridCombo = ATempCombo;
        }

        #endregion

        private void NewRowManual(ref GLBatchTDSATransactionRow ANewRow)
        {
            ANewRow.LedgerNumber = FJournal.LedgerNumber;
            ANewRow.BatchNumber = FJournal.BatchNumber;
            ANewRow.JournalNumber = FJournal.JournalNumber;
            ANewRow.TransactionNumber = FNextTransactionNumber;
            ANewRow.TransactionDate = FJournal.DateEffective;
            ANewRow.CostCentreCode = System.DBNull.Value.ToString();
            ANewRow.AccountCode = System.DBNull.Value.ToString();
            ANewRow.Percentage = 0;
            FNextTransactionNumber++;
        }

        private void ShowDetailsManual(GLBatchTDSATransactionRow ARow)
        {
            btnDeleteAll.Enabled = pnlDetails.Enabled;

            if (ARow != null)
            {
                FCurrentTransactionNumber = ARow.TransactionNumber;
            }

            if (FLedgerNumber != -1)
            {
                RefreshAnalysisAttributesGrid(cmbDetailAccountCode, FMainDS);
            }
        }

        #region Events

        bool FValidateEverything = false;

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            FMainDS.ATransAnalAttrib.Merge(FTempFromDS.ATransAnalAttrib);

            // enables extra validation (i.e. data not in grid)
            FValidateEverything = true;

            if (ValidateAllData(false, TErrorProcessingMode.Epm_All))
            {
                // Create the transaction to take the given amount OUT of the "allocate from" account & cost centre.
                GLBatchTDSATransactionRow NewRow = FMainDS.ATransaction.NewRowTyped(true);
                NewRow.LedgerNumber = FJournal.LedgerNumber;
                NewRow.BatchNumber = FJournal.BatchNumber;
                NewRow.JournalNumber = FJournal.JournalNumber;
                NewRow.TransactionNumber = FJournal.LastTransactionNumber + 1;
                NewRow.TransactionDate = FJournal.DateEffective;
                NewRow.CostCentreCode = FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetCostCentreCodeDBName()].ToString();
                NewRow.AccountCode = FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString();
                NewRow.DebitCreditIndicator = (bool)FPreviouslySelectedAccountsRow[AAccountTable.GetDebitCreditIndicatorDBName()];
                NewRow.TransactionAmount = Convert.ToDecimal(txtFromTransactionAmount.Text);
                NewRow.Reference = txtFromReference.Text;

                // If the source account has a postive balance, reverse the debit/credit indicator: we are moving money out of this account.
                if ((decimal)FPreviouslySelectedAccountsRow[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()] >= 0)
                {
                    NewRow.DebitCreditIndicator = !NewRow.DebitCreditIndicator;
                }

                // automatic narritive if none supplied by user
                if (string.IsNullOrEmpty(txtFromNarrative.Text))
                {
                    NewRow.Narrative = Catalog.GetString("Reallocation from ") +
                                       FPreviouslySelectedAccountsRow[AAccountTable.GetAccountCodeShortDescDBName()];
                }
                else
                {
                    NewRow.Narrative = txtFromNarrative.Text;
                }

                // add DebitCreditIndicator, Narrative and Reference to each row in grid
                DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

                for (int i = dv.Count - 1; i >= 0; i--)
                {
                    GLBatchTDSATransactionRow Row = (GLBatchTDSATransactionRow)dv[i].Row;
                    Row.DebitCreditIndicator = !NewRow.DebitCreditIndicator;
                    Row.Reference = txtFromReference.Text;

                    // automatic narritive if none supplied by user
                    if (string.IsNullOrEmpty(txtFromNarrative.Text))
                    {
                        Row.Narrative = Catalog.GetString("Reallocation to ") + NewRow.CostCentreCode + "-" + NewRow.AccountCode;
                    }
                    else
                    {
                        Row.Narrative = txtFromNarrative.Text;
                    }
                }

                FMainDS.ATransaction.Rows.Add(NewRow);

                this.DialogResult = DialogResult.OK;
                FPetraUtilsObject.DisableSaveButton();
                Close();
            }

            // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
            FPetraUtilsObject.VerificationResultCollection.Clear();

            FValidateEverything = false;
        }

        // This does nothing. We do not actually want to save any data here.
        private TSubmitChangesResult StoreManualCode(ref GLBatchTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            return TSubmitChangesResult.scrOK;
        }

        private void NewRow(Object Sender, EventArgs e)
        {
            if (CreateNewATransaction())
            {
                txtDetailTransactionAmount.NumberValueDecimal = 0;
                txtDetailPercentage.NumberValueDecimal = 0;

                // ok button is only enable when at least one row has been added
                if (grdDetails.Rows.Count > 1)
                {
                    btnOK.Enabled = true;
                }
            }

            FPetraUtilsObject.DisableSaveButton();
        }

        // update Reallocation percentages or amounts when the total 'from' amount is changed
        private void TotalAmountChanged(Object Sender, EventArgs e)
        {
            if (txtDetailTransactionAmount.Enabled)
            {
                UpdatePercentages(grdDetails.Rows.Count <= 2);
            }
            else
            {
                UpdateAmounts(grdDetails.Rows.Count <= 2);
            }
        }

        // radio selection has changed
        private void FromAmountPercentageChanged(Object Sender, EventArgs e)
        {
            txtFromTransactionAmount.Enabled = rbtFromAmountOption.Checked;
            txtFromPercentage.Enabled = rbtFromPercentageOption.Checked;
        }

        // radio selection has changed
        private void ToAmountPercentageChanged(Object Sender, EventArgs e)
        {
            txtDetailTransactionAmount.Enabled = rbtAmountOption.Checked;
            txtDetailPercentage.Enabled = rbtPercentageOption.Checked;
            TotalAmountChanged(Sender, e);
        }

        private void ToAmountChanged(Object Sender, EventArgs e)
        {
            // update percentage for either current row or all rows
            UpdatePercentages(GetAmountTotal() != Convert.ToDecimal(txtFromTransactionAmount.Text));
        }

        private void ToPercentageChanged(Object Sender, EventArgs e)
        {
            // update amount for either current row or all rows
            UpdateAmounts(GetPercentageTotal() != 100);
        }

        private void FromAmountChanged(Object Sender, EventArgs e)
        {
            if (FPreviouslySelectedAccountsRow != null)
            {
                txtFromPercentage.TextChanged -= new System.EventHandler(this.FromPercentageChanged);

                // update the percentage
                if ((decimal)FPreviouslySelectedAccountsRow[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()] == 0)
                {
                    txtFromPercentage.NumberValueDecimal = 0;
                }
                else
                {
                    txtFromPercentage.NumberValueDecimal =
                        (txtFromTransactionAmount.NumberValueDecimal /
                         Math.Abs((decimal)FPreviouslySelectedAccountsRow[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()])) * 100;

                    txtFromPercentage.TextChanged += new System.EventHandler(this.FromPercentageChanged);
                }

                if (FPreviouslySelectedDetailRow != null)
                {
                    TotalAmountChanged(this, null);
                }
            }
        }

        private void FromPercentageChanged(Object Sender, EventArgs e)
        {
            if (FPreviouslySelectedAccountsRow != null)
            {
                txtFromTransactionAmount.TextChanged -= new System.EventHandler(this.FromAmountChanged);

                // update the amount
                txtFromTransactionAmount.NumberValueDecimal =
                    (txtFromPercentage.NumberValueDecimal / 100) *
                    Math.Abs((decimal)FPreviouslySelectedAccountsRow[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()]);

                txtFromTransactionAmount.TextChanged += new System.EventHandler(this.FromAmountChanged);

                if (FPreviouslySelectedDetailRow != null)
                {
                    TotalAmountChanged(this, null);
                }
            }
        }

        // delete highlighted row/s
        private void DeleteRecord(Object Sender, EventArgs e)
        {
            ATransactionRow RowToDelete = FMainDS.ATransaction.NewRowTyped();

            RowToDelete.ItemArray = (object[])FPreviouslySelectedDetailRow.ItemArray.Clone();

            this.DeleteATransaction();

            if (grdDetails.Rows.Count <= 2)
            {
                btnOK.Enabled = false;
            }

            ATransAnalAttribTable TempTable = new ATransAnalAttribTable();
            TempTable.Merge(FMainDS.ATransAnalAttrib);

            foreach (ATransAnalAttribRow Row in TempTable.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted)
                    && (Row.LedgerNumber == RowToDelete.LedgerNumber)
                    && (Row.BatchNumber == RowToDelete.BatchNumber)
                    && (Row.JournalNumber == RowToDelete.JournalNumber))
                {
                    if (Row.TransactionNumber == RowToDelete.TransactionNumber)
                    {
                        // delete row as no longer used
                        FMainDS.ATransAnalAttrib.Rows.Find(
                            new object[] { Row.LedgerNumber, Row.BatchNumber, Row.JournalNumber, Row.TransactionNumber,
                                           Row.AnalysisTypeCode }).Delete();
                    }
                    else if (Row.TransactionNumber > RowToDelete.TransactionNumber)
                    {
                        // transaction number needs updated
                        ((ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows.Find(
                             new object[] { Row.LedgerNumber, Row.BatchNumber, Row.JournalNumber, Row.TransactionNumber,
                                            Row.AnalysisTypeCode })).TransactionNumber -= 1;
                    }
                }
            }

            foreach (ATransactionRow Row in FMainDS.ATransaction.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted)
                    && (Row.LedgerNumber == RowToDelete.LedgerNumber)
                    && (Row.BatchNumber == RowToDelete.BatchNumber)
                    && (Row.JournalNumber == RowToDelete.JournalNumber)
                    && (Row.TransactionNumber > RowToDelete.TransactionNumber))
                {
                    // transaction number needs updated
                    Row.TransactionNumber -= 1;
                }
            }

            FPetraUtilsObject.DisableSaveButton();
        }

        // delete all rows
        private void DeleteAllReallocations(Object Sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString(
                             "You have chosen to delete all Reallocations.\n\nDo you really want to continue?")),
                     Catalog.GetString("Confirm Deletion"),
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question,
                     MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes))
            {
                DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;
                DataView TransAttribDV = new DataView(FMainDS.ATransAnalAttrib);

                for (int i = TransAttribDV.Count - 1; i >= 0; i--)
                {
                    TransAttribDV.Delete(i);
                }

                for (int i = dv.Count - 1; i >= 0; i--)
                {
                    dv[i].Delete();
                }

                SelectRowInGrid(1);

                btnOK.Enabled = false;
            }
        }

        /// <summary>
        /// New account is selected in grdAccounts
        /// </summary>
        private void grdAccounts_RowSelected(object sender, RangeRegionChangedEventArgs e)
        {
            DataRowView rowView = (DataRowView)grdAccounts.Rows.IndexToDataSourceRow(grdAccounts.Selection.ActivePosition.Row);

            if (rowView != null)
            {
                FPreviouslySelectedAccountsRow = rowView.Row;

                if (rbtFromAmountOption.Checked)
                {
                    // update percentage
                    FromAmountChanged(sender, null);
                }
                else
                {
                    //update amount
                    FromPercentageChanged(sender, null);
                }
            }
        }

        private void AnalysisAttributesGrid_RowSelected(System.Object sender, RangeRegionChangedEventArgs e)
        {
            TSgrdDataGridPaged Grid = sender as TSgrdDataGridPaged;

            if (Grid == null)
            {
                Grid = (TSgrdDataGridPaged)((SourceGrid.Selection.RowSelection)sender).Grid;
            }

            if (Grid.Selection.ActivePosition.IsEmpty() || (Grid.Selection.ActivePosition.Column == 0))
            {
                return;
            }

            if ((TAnalysisAttributes.GetSelectedAttributeRow(Grid) == null) || (FPSAttributesRow == TAnalysisAttributes.GetSelectedAttributeRow(Grid)))
            {
                return;
            }

            FPSAttributesRow = TAnalysisAttributes.GetSelectedAttributeRow(Grid);

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
            if (Grid.Name == grdFromAnalAttributes.Name)
            {
                FcmbFromAnalAttribValues.StandardValuesExclusive = true;
                FcmbFromAnalAttribValues.StandardValues = analTypeValues;
            }
            else
            {
                FcmbToAnalAttribValues.StandardValuesExclusive = true;
                FcmbToAnalAttribValues.StandardValues = analTypeValues;
            }
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            GLBatchTDS DS = null;
            int TransactionNumber = 0;
            string AccountCode = "";

            if ((sender as SourceGrid.Selection.RowSelection) != null)
            {
                DS = FTempFromDS;
                TransactionNumber = FJournal.LastTransactionNumber + 1;
                AccountCode = FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString();
            }
            else
            {
                if (FPreviouslySelectedDetailRow == null)
                {
                    return;
                }

                DS = FMainDS;
                TransactionNumber = FCurrentTransactionNumber;
                AccountCode = ((TCmbAutoPopulated)sender).GetSelectedString();
            }

            if (FPreviouslySelectedDetailRow != null)
            {
                FCurrentTransactionNumber = FPreviouslySelectedDetailRow.TransactionNumber;
            }

            FAnalysisAttributesLogic.TransAnalAttrRequiredUpdating(DS, null, AccountCode, TransactionNumber, false);
            RefreshAnalysisAttributesGrid(sender, DS);
        }

        #endregion

        #region Helper Methods

        private void UpdatePercentages(bool ACurrentRowOnly)
        {
            this.txtDetailPercentage.TextChanged -= new System.EventHandler(this.ToPercentageChanged);

            if (txtFromTransactionAmount.NumberValueDecimal != 0)
            {
                txtDetailPercentage.NumberValueDecimal =
                    (txtDetailTransactionAmount.NumberValueDecimal / txtFromTransactionAmount.NumberValueDecimal) * 100;
            }
            else
            {
                txtDetailPercentage.NumberValueDecimal = 0;
            }

            // only currently selected row needs updating
            if (ACurrentRowOnly)
            {
                return;
            }
            // all rows need updating
            else
            {
                List <GLBatchTDSATransactionRow>ReallocationList = new List <GLBatchTDSATransactionRow>();

                foreach (GLBatchTDSATransactionRow Row in MainDS.ATransaction.Rows)
                {
                    if (txtFromTransactionAmount.NumberValueDecimal != 0)
                    {
                        Row.Percentage = decimal.Round((Row.TransactionAmount / ((decimal)txtFromTransactionAmount.NumberValueDecimal)) * 100, 2);
                    }
                    else
                    {
                        Row.Percentage = 0;
                    }

                    ReallocationList.Add(Row);
                }

                // fix rounding error
                if ((GetAmountTotal() == txtFromTransactionAmount.NumberValueDecimal) && (GetPercentageTotal() != 100)
                    && (txtFromTransactionAmount.NumberValueDecimal != 0))
                {
                    decimal Difference = 100 - GetPercentageTotal();

                    // sort list by amount sizes
                    ReallocationList = ReallocationList.OrderByDescending(o => o.Percentage).ToList();

                    if (Difference < 0)
                    {
                        int Index = 0;

                        while (Difference != 0)
                        {
                            ReallocationList[Index].Percentage -= (decimal)0.01;
                            Difference += (decimal)0.01;
                            Index++;
                        }
                    }
                    else if (Difference > 0)
                    {
                        int Index = 0;

                        while (Difference != 0)
                        {
                            ReallocationList[Index].Percentage += (decimal)0.01;
                            Difference -= (decimal)0.01;
                            Index++;
                        }
                    }
                }
            }

            this.txtDetailPercentage.TextChanged += new System.EventHandler(this.ToPercentageChanged);
        }

        private void UpdateAmounts(bool ACurrentRowOnly)
        {
            this.txtDetailTransactionAmount.TextChanged -= new System.EventHandler(this.ToAmountChanged);

            txtDetailTransactionAmount.NumberValueDecimal =
                (txtDetailPercentage.NumberValueDecimal / 100) * txtFromTransactionAmount.NumberValueDecimal;

            // only currently selected row needs updating
            if (ACurrentRowOnly)
            {
                return;
            }
            // all rows need updating
            else
            {
                List <GLBatchTDSATransactionRow>ReallocationList = new List <GLBatchTDSATransactionRow>();

                foreach (GLBatchTDSATransactionRow Row in MainDS.ATransaction.Rows)
                {
                    Row.TransactionAmount = decimal.Round((Row.Percentage / 100) * ((decimal)txtFromTransactionAmount.NumberValueDecimal), 2);
                    ReallocationList.Add(Row);
                }

                // fix rounding error
                if ((GetPercentageTotal() == 100) && (GetAmountTotal() != txtFromTransactionAmount.NumberValueDecimal)
                    && (txtFromTransactionAmount.NumberValueDecimal != 0))
                {
                    decimal Difference = (decimal)txtFromTransactionAmount.NumberValueDecimal - GetAmountTotal();

                    // sort list by amount sizes
                    ReallocationList = ReallocationList.OrderByDescending(o => o.TransactionAmount).ToList();

                    if (Difference < 0)
                    {
                        int Index = 0;

                        while (Difference != 0)
                        {
                            ReallocationList[Index].TransactionAmount -= (decimal)0.01;
                            Difference += (decimal)0.01;
                            Index++;
                        }
                    }
                    else if (Difference > 0)
                    {
                        int Index = 0;

                        while (Difference != 0)
                        {
                            ReallocationList[Index].TransactionAmount += (decimal)0.01;
                            Difference -= (decimal)0.01;
                            Index++;
                        }
                    }
                }
            }

            this.txtDetailTransactionAmount.TextChanged += new System.EventHandler(this.ToAmountChanged);
        }

        // calculates total amount in rows
        private decimal GetAmountTotal()
        {
            decimal TotalAmountInReallocations = 0;

            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = dv.Count - 1; i >= 0; i--)
            {
                TotalAmountInReallocations += ((GLBatchTDSATransactionRow)dv[i].Row).TransactionAmount;
            }

            return TotalAmountInReallocations;
        }

        // calculates total percentage in rows
        private decimal GetPercentageTotal()
        {
            decimal TotalPercentageInReallocations = 0;

            DataView dv = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = dv.Count - 1; i >= 0; i--)
            {
                TotalPercentageInReallocations += ((GLBatchTDSATransactionRow)dv[i].Row).Percentage;
            }

            return TotalPercentageInReallocations;
        }

        private bool CanCloseManual()
        {
            // if 'Cancel' button has been clicked then ask the user if they really want to close the screen.
            if (FMainDS.HasChanges()
                && (this.DialogResult != DialogResult.OK)
                && (MessageBox.Show(Catalog.GetString("Are you sure you want to cancel this Reallocation?"),
                        Catalog.GetString("Reallocation Journal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                    == DialogResult.No))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Analysis Attributes

        private void RefreshAnalysisAttributesGrid(object sender, GLBatchTDS ADS)
        {
            TSgrdDataGrid Grid = null;
            int TransactionNumber = 0;
            string AccountCode = "";

            if ((sender as SourceGrid.Selection.RowSelection) != null)
            {
                Grid = grdFromAnalAttributes;
                TransactionNumber = FJournal.LastTransactionNumber + 1;
                AccountCode = FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString();
            }
            else
            {
                Grid = grdToAnalAttributes;
                TransactionNumber = FCurrentTransactionNumber;
                AccountCode = cmbDetailAccountCode.GetSelectedString();
            }

            //Empty the grid
            ADS.ATransAnalAttrib.DefaultView.RowFilter = "1=2";
            FPSAttributesRow = null;

            if (!TRemote.MFinance.Setup.WebConnectors.AccountHasAnalysisAttributes(FLedgerNumber, AccountCode,
                    true))
            {
                if (Grid.Enabled)
                {
                    Grid.Enabled = false;
                }

                return;
            }
            else
            {
                if (!Grid.Enabled)
                {
                    Grid.Enabled = true;
                }
            }

            FAnalysisAttributesLogic.SetTransAnalAttributeDefaultView(ADS, TransactionNumber);

            Grid.DataSource = new DevAge.ComponentModel.BoundDataView(ADS.ATransAnalAttrib.DefaultView);

            if (Grid.Rows.Count > 1)
            {
                Grid.SelectRowWithoutFocus(1);
                AnalysisAttributesGrid_RowSelected(Grid, null);
            }
        }

        #endregion

        #region Validation

        private void ValidateDataDetailsManual(GLBatchTDSATransactionRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GL.ValidateReallocationJournalDialog(this,
                ARow,
                rbtAmountOption.Checked,
                txtFromTransactionAmount.NumberValueDecimal,
                ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            if (!FAnalysisAttributesLogic.AccountAnalysisAttributeCountIsCorrect(ARow.TransactionNumber, ARow.AccountCode, FMainDS))
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
                VerificationResult.OverrideResultText(String.Format(Catalog.GetString(
                            "A value must be entered for the 'Analysis Attribute' for the 'From Allocation's' Account Code {0}."),
                        ARow.AccountCode));

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(grdToAnalAttributes, VerificationResult, ValidationColumn, true);
            }

            String ValueRequiredForType;

            if (!FAnalysisAttributesLogic.AccountAnalysisAttributesValuesExist(ARow.TransactionNumber, ARow.AccountCode, FMainDS,
                    out ValueRequiredForType))
            {
                DataColumn ValidationColumn;
                TVerificationResult VerificationResult = null;
                object ValidationContext;

                ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnAccountCodeId];
                ValidationContext = String.Format("Analysis code {0} for Account Code {1}",
                    ValueRequiredForType,
                    ARow.AccountCode);

                // This code is only running because of failure, so cause an error to occur.
                VerificationResult = TStringChecks.StringMustNotBeEmpty("",
                    ValidationContext.ToString(),
                    this, ValidationColumn, null);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn, true);
            }

            if ((VerificationResultCollection.Count == 0) && FValidateEverything)
            {
                ValidateEverything();
            }
        }

        /// validate all data not in a DataRow
        private void ValidateEverything()
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            TScreenVerificationResult VerificationResult;

            // Validate Reference
            if (!string.IsNullOrEmpty(txtFromReference.Text) && (txtFromReference.Text.Length > 100))
            {
                // 'Reference' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtFromReference.Text, 100,
                        "Reference", txtFromReference), null, txtFromReference);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromReference, VerificationResult, null);
            }
            else if (string.IsNullOrEmpty(txtFromReference.Text))
            {
                // 'Reference' must not be empty
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringMustNotBeEmpty(txtFromReference.Text,
                        "Reference", txtFromReference), null, txtFromReference);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromReference, VerificationResult, null);
            }

            // Validate Narrative
            if (!string.IsNullOrEmpty(txtFromNarrative.Text) && (txtFromNarrative.Text.Length > 500))
            {
                // 'Narrative' must not contain more than 100 characters
                VerificationResult = new TScreenVerificationResult(TStringChecks.StringLengthLesserOrEqual(txtFromNarrative.Text, 500,
                        "Narrative", txtFromNarrative), null, txtFromNarrative);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromNarrative, VerificationResult, null);
            }

            if (rbtFromAmountOption.Checked)
            {
                // Validate txtFromTransactionAmount
                if (string.IsNullOrEmpty(txtFromTransactionAmount.Text) || (Convert.ToDecimal(txtFromTransactionAmount.Text) == 0)
                    || (Convert.ToDecimal(txtFromTransactionAmount.Text) >
                        Math.Abs((decimal)FPreviouslySelectedAccountsRow[AGeneralLedgerMasterPeriodTable.GetActualBaseDBName()])))
                {
                    if (string.IsNullOrEmpty(txtFromTransactionAmount.Text))
                    {
                        txtFromTransactionAmount.NumberValueDecimal = 0;
                    }

                    // From Amount must not be 0 or greater than |account balance|
                    VerificationResult = new TScreenVerificationResult(this, null,
                        string.Format(Catalog.GetString(
                                "The amount for the Re-allocation must be between 0 and the positive total for this period.{0}" +
                                "Please re-enter the amount."), "\n\n"),
                        txtFromTransactionAmount, TResultSeverity.Resv_Critical);

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromTransactionAmount, VerificationResult, null);
                }
            }
            else if (rbtFromPercentageOption.Checked)
            {
                // Validate txtFromPercentage
                if ((Convert.ToDecimal(txtFromPercentage.NumberValueDecimal) == 0)
                    || (Convert.ToDecimal(txtFromPercentage.NumberValueDecimal) > 100))
                {
                    if (string.IsNullOrEmpty(txtFromPercentage.Text))
                    {
                        txtFromPercentage.NumberValueDecimal = 0;
                    }

                    // Percentage must not be 0 or greater than 100
                    VerificationResult = new TScreenVerificationResult(this, null,
                        string.Format(Catalog.GetString(
                                "The percentage for the Re-allocation must be between 1% and 100%.{0}Please re-enter the percentage."), "\n\n"),
                        txtFromPercentage, TResultSeverity.Resv_Critical);

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtFromPercentage, VerificationResult, null);
                }
            }

            // Validate To Allocations' amounts
            if (rbtAmountOption.Checked)
            {
                if (GetAmountTotal() != Convert.ToDecimal(txtFromTransactionAmount.Text))
                {
                    VerificationResult = new TScreenVerificationResult(this, null,
                        Catalog.GetString(
                            "The 'To' amounts entered do not match the total amount of the Allocation. Please check the amounts entered."),
                        txtDetailTransactionAmount, TResultSeverity.Resv_Critical);

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtDetailTransactionAmount, VerificationResult, null);
                }
            }
            // Validate Allocations' percentages
            else
            {
                if (GetPercentageTotal() != 100)
                {
                    VerificationResult = new TScreenVerificationResult(this, null,
                        Catalog.GetString("The 'To' percentages entered must add up to 100%."),
                        txtDetailPercentage, TResultSeverity.Resv_Critical);

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(txtDetailPercentage, VerificationResult, null);
                }
            }

            if (grdDetails.Rows.Count <= 1)
            {
                VerificationResult = new TScreenVerificationResult(this, null,
                    Catalog.GetString("You must include at least 1 destination allocation."),
                    btnNew, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(btnNew, VerificationResult, null);
            }
            else if (grdDetails.Rows.Count > 11)
            {
                VerificationResult = new TScreenVerificationResult(this, null,
                    Catalog.GetString("You must include no more than 10 destination allocations."),
                    btnDeleteReallocation, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(btnDeleteReallocation, VerificationResult, null);
            }

            if (!FAnalysisAttributesLogic.AccountAnalysisAttributeCountIsCorrect(FJournal.LastTransactionNumber + 1,
                    FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString(), FTempFromDS))
            {
                VerificationResult = new TScreenVerificationResult(this, null,
                    String.Format(Catalog.GetString(
                            "A value must be entered for the 'Analysis Attribute' for the 'From Allocation's' Account Code {0}."),
                        FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString()),
                    grdFromAnalAttributes, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(grdFromAnalAttributes, VerificationResult, null);
            }

            String ValueRequiredForType;

            if (!FAnalysisAttributesLogic.AccountAnalysisAttributesValuesExist(
                    FJournal.LastTransactionNumber + 1, FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString(),
                    FTempFromDS, out ValueRequiredForType))
            {
                VerificationResult = new TScreenVerificationResult(this, null,
                    String.Format(Catalog.GetString(
                            "A value must be entered for the 'Analysis code {0} for Account Code {1}."),
                        ValueRequiredForType, FPreviouslySelectedAccountsRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString()),
                    grdFromAnalAttributes, TResultSeverity.Resv_Critical);

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(grdFromAnalAttributes, VerificationResult, null);
            }
        }

        #endregion
    }
}