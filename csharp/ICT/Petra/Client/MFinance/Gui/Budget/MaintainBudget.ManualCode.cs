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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

//using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    /// <summary>
    /// Interface used by logic objects in order to access selected public methods in the MaintainBudget class
    /// </summary>
    public interface IMaintainBudget
    {
        /// <summary></summary>
        void SetBudgetDefaultView(BudgetTDS AMainDS);

        /// <summary></summary>
        /// <param name="ARowIndex"></param>
        void SelectRowInGrid(int ARowIndex);
    }

    public partial class TFrmMaintainBudget : IMaintainBudget, IBoundImageEvaluator
    {
        private Int32 FLedgerNumber;
        private Int32 FCurrentFinancialYear;
        private Int32 FNextFinancialYear;
        private Int32 FSelectedBudgetYear;

        private Int32 FBudgetSequence = -1;

        //This may be 13 so allow for it
        private Int32 FNumberOfPeriods;
        private bool FHas13Periods;
        private bool FHas14Periods;

        private bool FLoadCompleted = false;
        private bool FRejectYearChange = false;

        private String FCurrencyCode = "";

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        private MaintainBudget_Import FImportLogicObject = null;

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                LoadBudgets();
                FImportLogicObject = new MaintainBudget_Import(FPetraUtilsObject, FLedgerNumber, FNumberOfPeriods, this);
            }
        }

        private void LoadBudgets()
        {
            Console.WriteLine("LoadBudgets() ...");
            DateTime DtStart = DateTime.Now;

            Console.WriteLine("Budgets loaded -- {0} ms", (DateTime.Now - DtStart).TotalMilliseconds);

            //Prepare form for correct number of periods
            FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadLedgerInfo(FLedgerNumber));

            ALedgerRow LedgerRow = (ALedgerRow)FMainDS.ALedger.Rows[0];
            FNumberOfPeriods = LedgerRow.NumberOfAccountingPeriods;
            FCurrencyCode = LedgerRow.BaseCurrency;
            FCurrentFinancialYear = LedgerRow.CurrentFinancialYear;
            FNextFinancialYear = FCurrentFinancialYear + 1;

            FHas13Periods = (FNumberOfPeriods == 13);
            FHas14Periods = (FNumberOfPeriods == 14);

            // to get an empty ABudgetFee table, instead of null reference
            FMainDS.Merge(new BudgetTDS());

            //Setup form and controls
            this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            InitialiseControls();

            //Always auto-load budgets for current and next financial year
            LoadBudgetsForNextYear();
            FSelectedBudgetYear = FCurrentFinancialYear; // TFinanceControls.GetLedgerCurrentFinancialYear(FLedgerNumber);
            cmbSelectBudgetYear.SetSelectedInt32(FSelectedBudgetYear);
            LoadBudgetsForSelectedYear();

            SelectRowInGrid(1);

            RefreshComboLabels();
            UpdateRecordNumberDisplay();

            Console.WriteLine("Load complete  {0} ms", (DateTime.Now - DtStart).TotalMilliseconds);
            FLoadCompleted = true;
        }

        private void LoadBudgetsForSelectedYear()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (FMainDS.ABudget.Select(String.Format("{0}={1}", ABudgetTable.GetYearDBName(), FSelectedBudgetYear)).Length == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.Budget.WebConnectors.LoadBudgetsForYear(FLedgerNumber, FSelectedBudgetYear));
                }

                SetBudgetDefaultView(FMainDS);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        //Load budgets into dataset for next financial year
        // This is required for import process and is called
        //  from first load of budgets for current year
        private void LoadBudgetsForNextYear()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (FMainDS.ABudget.Select(String.Format("{0}={1}", ABudgetTable.GetYearDBName(), FNextFinancialYear)).Length == 0)
                {
                    FMainDS.Merge(TRemote.MFinance.Budget.WebConnectors.LoadBudgetsForYear(FLedgerNumber, FNextFinancialYear));
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void SetBudgetDefaultView(BudgetTDS AMainDS)
        {
            DataView MyDataView = AMainDS.ABudget.DefaultView;

            MyDataView.AllowNew = false;

            string RowFilter = String.Format("{0} = {1}",
                ABudgetTable.GetYearDBName(),
                FSelectedBudgetYear);

            MyDataView.Sort = String.Format("{0} ASC, {1} ASC",
                ABudgetTable.GetCostCentreCodeDBName(),
                ABudgetTable.GetAccountCodeDBName());

            MyDataView.RowFilter = RowFilter;

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(RowFilter, true);
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();
        }

        private void RefreshComboLabels()
        {
            //Force a refresh of the combo labels if the first highlighted row contains inactive codes
            // as the colour does not show up for a first row
            if (!AccountIsActive(cmbDetailAccountCode.GetSelectedString())
                && (cmbDetailAccountCode.AttachedLabel.BackColor != System.Drawing.Color.PaleVioletRed))
            {
                cmbDetailAccountCode.AttachedLabel.BackColor = System.Drawing.Color.PaleVioletRed;
            }

            if (!CostCentreIsActive(cmbDetailCostCentreCode.GetSelectedString())
                && (cmbDetailCostCentreCode.AttachedLabel.BackColor != System.Drawing.Color.PaleVioletRed))
            {
                cmbDetailCostCentreCode.AttachedLabel.BackColor = System.Drawing.Color.PaleVioletRed;
            }
        }

        private void SetupExtraGridFunctionality()
        {
            //Populate CostCentreList variable
            DataTable CostCentreList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable TmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(TmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref CostCentreList, FMainDS.Tables[TmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(TmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)CostCentreList;

            //Populate AccountList variable
            DataTable AccountList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable TmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(TmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref AccountList, FMainDS.Tables[TmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(TmpAccountTable.TableName);

            FAccountTable = (AAccountTable)AccountList;

            int IndexOfCostCentreCodeDataColumn = 0;
            int IndexOfAccountCodeDataColumn = 1;

            // Add red triangle to inactive accounts
            grdDetails.AddAnnotationImage(this, IndexOfCostCentreCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.CostCentreCode, BoundGridImage.DisplayImageEnum.Inactive);
            grdDetails.AddAnnotationImage(this, IndexOfAccountCodeDataColumn,
                BoundGridImage.AnnotationContextEnum.AccountCode, BoundGridImage.DisplayImageEnum.Inactive);
        }

        private void InitialiseControls()
        {
            SetupExtraGridFunctionality();

            // Deal with labels on toolbar to get required effect
            lblBlank.ForeColor = Color.Transparent;
            lblBlank.BackColor = Color.Transparent;
            lblBlank.Text = new String(' ', 4);

            lblYearEnding.BackColor = Color.Transparent;
            lblYearEnding.TextAlign = ContentAlignment.TopRight;
            lblYearEnding.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);

            //Get Available GL Years showing year end dates
            TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbSelectBudgetYear, FLedgerNumber, true, true);
            cmbSelectBudgetYear.ComboBoxWidth = 100;

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false, true);

            // Do not include summary cost centres: we want to use one cost centre for each Motivation Details. Local and Foreign included
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, false, true);

            bool IfMoreThan12Periods = (FNumberOfPeriods > 12);
            bool IfMoreThan13Periods = (FNumberOfPeriods > 13);

            txtPeriod13Amount.Visible = IfMoreThan12Periods;
            lblPeriod13Amount.Visible = IfMoreThan12Periods;
            txtPeriod13Index.Visible = IfMoreThan12Periods;
            lblPeriod13Index.Visible = IfMoreThan12Periods;

            txtPeriod14Amount.Visible = IfMoreThan13Periods;
            lblPeriod14Amount.Visible = IfMoreThan13Periods;
            txtPeriod14Index.Visible = IfMoreThan13Periods;
            lblPeriod14Index.Visible = IfMoreThan13Periods;

            lblPerPeriodAmount.Text = String.Format("Amount for periods 1 to {0}:", (FNumberOfPeriods - 1));
            lblLastPeriodAmount.Text = String.Format("Amount for period {0}:", FNumberOfPeriods);

            grdDetails.Columns[grdDetails.Columns.Count - 1].Visible = IfMoreThan13Periods;
            grdDetails.Columns[grdDetails.Columns.Count - 2].Visible = IfMoreThan12Periods;
        }

        private void NewRowManual(ref BudgetTDSABudgetRow ARow)
        {
            if (!cmbDetailAccountCode.Enabled)
            {
                EnableBudgetEntry(true);
            }

            ARow.BudgetSequence = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_budget));
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, FSelectedBudgetYear);
            ARow.Year = FSelectedBudgetYear;

            //Add the budget period values
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                ABudgetPeriodRow budgetPeriodRow = FMainDS.ABudgetPeriod.NewRowTyped();
                budgetPeriodRow.BudgetSequence = ARow.BudgetSequence;
                budgetPeriodRow.PeriodNumber = i;
                budgetPeriodRow.BudgetBase = 0;
                FMainDS.ABudgetPeriod.Rows.Add(budgetPeriodRow);
                budgetPeriodRow = null;
            }
        }

        private void EnableBudgetEntry(bool AAllowEntry)
        {
            pnlDetails.Enabled = AAllowEntry;
            rgrBudgetTypeCode.Enabled = AAllowEntry;
            cmbDetailCostCentreCode.Enabled = AAllowEntry;
            cmbDetailAccountCode.Enabled = AAllowEntry;

            if (AAllowEntry)
            {
                pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
                pnlBudgetTypeSame.Visible = rbtSame.Checked;
                pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
                pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
                pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;
            }
            else
            {
                pnlBudgetTypeAdhoc.Visible = false;
                pnlBudgetTypeSame.Visible = false;
                pnlBudgetTypeSplit.Visible = false;
                pnlBudgetTypeInflateN.Visible = false;
                pnlBudgetTypeInflateBase.Visible = false;
            }
        }

        private void UpdatePeriodAmountsFromControls(BudgetTDSABudgetRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            //Write to Budget custom fields
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                ABudgetPeriodRow budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(
                    new object[] { ARow.BudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    string customColumn = "Period" + i.ToString("00") + "Amount";

                    if ((decimal)ARow[customColumn] != budgetPeriodRow.BudgetBase)
                    {
                        ARow[customColumn] = budgetPeriodRow.BudgetBase;
                    }
                }

                budgetPeriodRow = null;
            }
        }

        private void SelectBudgetYear(Object sender, EventArgs e)
        {
            if (FLoadCompleted)
            {
                if (FRejectYearChange)
                {
                    return;
                }
                else if (FPetraUtilsObject.HasChanges)
                {
                    FRejectYearChange = true;
                    MessageBox.Show(Catalog.GetString("Please save changes before attempting to change year."));

                    cmbSelectBudgetYear.SetSelectedInt32(FSelectedBudgetYear);
                    return;
                }

                FSelectedBudgetYear = cmbSelectBudgetYear.GetSelectedInt32();
                LoadBudgetsForSelectedYear();
                SelectRowInGrid(1);

                if (FPetraUtilsObject.HasChanges)
                {
                    //Change of year clears boxes in some circumstances so need to save
                    SaveChanges();
                }
            }
        }

        private TSubmitChangesResult StoreManualCode(ref BudgetTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TSubmitChangesResult TSCR = TRemote.MFinance.Budget.WebConnectors.SaveBudget(ref ASubmitChanges);

            //Reset this flag if the save was successful
            FRejectYearChange = !(TSCR == TSubmitChangesResult.scrOK);

            return TSCR;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            //Make sure valid year selected
            if (cmbSelectBudgetYear.GetSelectedInt32() < FCurrentFinancialYear)
            {
                cmbSelectBudgetYear.SetSelectedInt32(FCurrentFinancialYear);
            }

            //Change to valid year failed so user needs to save changes
            if (cmbSelectBudgetYear.GetSelectedInt32() < FCurrentFinancialYear)
            {
                return;
            }

            CreateNewABudget();
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        ///  row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(ABudgetRow ARowToDelete, ref string ADeletionQuestion)
        {
            if (FSelectedBudgetYear < FCurrentFinancialYear)
            {
                MessageBox.Show("You cannot delete a budget from a previous year!",
                    "Delete Budget",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }

            ADeletionQuestion = String.Format(Catalog.GetString(
                    "You have chosen to delete the current budget for:{0}{0}" +
                    "    Year Ending: {1}, Cost Centre: {2}, Account: {3}, Type: {4}.{0}{0}" +
                    "Do you really want to delete it?"),
                Environment.NewLine,
                cmbSelectBudgetYear.GetSelectedDescription(),
                ARowToDelete.CostCentreCode,
                ARowToDelete.AccountCode,
                ARowToDelete.BudgetTypeCode);

            return true;

            //TODO: When budget revisioning is added:
            //"You have chosen to delete Budget: {0}    Year Ending: {1}, Cost Centre: {2}, Account: {3}, Type: {4}, Revision: {5}.{0}{0}Do you really want to delete it?"),
            //                ARowToDelete.Revision,
        }

        /// <summary>
        /// Deletes the current row and optionally populates a completion message
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to delete</param>
        /// <param name="ACompletionMessage">if specified, is the deletion completion message</param>
        /// <returns>true if row deletion is successful</returns>
        private bool DeleteRowManual(ABudgetRow ARowToDelete, ref string ACompletionMessage)
        {
            ACompletionMessage = String.Empty;

            DeleteBudgetPeriodData(ARowToDelete.BudgetSequence);
            ARowToDelete.Delete();
            DeleteBudgetRevisionData();

            return true;
        }

        private void PostDeleteManual(ABudgetRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            //Disable the controls if no records found
            if (FPreviouslySelectedDetailRow == null)
            {
                EnableBudgetEntry(false);
            }
        }

        private void ImportBudget(System.Object sender, System.EventArgs e)
        {
            if (FSelectedBudgetYear < FCurrentFinancialYear)
            {
                MessageBox.Show(Catalog.GetString("You can only import budget data when the current or next financial year is selected."),
                    "Budget Import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Import and refresh grid
            FImportLogicObject.ImportBudget(FCurrentFinancialYear, ref FMainDS);

            grdDetails.DataSource = null;
            SetBudgetDefaultView(FMainDS);
            SelectRowInGrid(1);
        }

        // This is not used (and imcomplete...)
        private void ExportBudget(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // Without save the server does not have the current changes, so forbid it.
                MessageBox.Show(Catalog.GetString("Please save changed Data before the Export!"),
                    Catalog.GetString("Export Error"));
                return;
            }

            //TODO: Complete the budget export code
            MessageBox.Show(Catalog.GetString("Not yet implemented."));
            //exportForm = new TFrmGiftBatchExport(FPetraUtilsObject.GetForm());
            //exportForm.LedgerNumber = FLedgerNumber;
            //exportForm.Show();
        }

        private void DeleteBudgetPeriodData(int ABudgetSequence)
        {
            DataView MyDataView = new DataView(FMainDS.ABudgetPeriod);

            MyDataView.RowFilter = String.Format("{0}={1}",
                ABudgetPeriodTable.GetBudgetSequenceDBName(),
                ABudgetSequence);

            foreach (DataRowView drv in MyDataView)
            {
                ABudgetPeriodRow bpr = (ABudgetPeriodRow)drv.Row;
                bpr.Delete();
            }
        }

        private void DeleteBudgetRevisionData()
        {
            //Check if any budgets exist for selected year and revision
            DataView BudgetDataView = new DataView(FMainDS.ABudget);

            //TODO: update this when budget revisioning is introduced
            BudgetDataView.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                ABudgetTable.GetLedgerNumberDBName(),
                FLedgerNumber,
                ABudgetTable.GetYearDBName(),
                FSelectedBudgetYear,
                ABudgetTable.GetRevisionDBName(),
                0);

            //If all budgets deleted for selected year then remove row in revision table
            if (BudgetDataView.Count == 0)
            {
                //Deleted any unwanted revision row
                DataView budgetRevisionDataView = new DataView(FMainDS.ABudgetRevision);

                //TODO: update this when budget revisioning is introduced
                budgetRevisionDataView.RowFilter = String.Format("{0}={1} And {2}={3} And {4}={5}",
                    ABudgetRevisionTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ABudgetRevisionTable.GetYearDBName(),
                    FSelectedBudgetYear,
                    ABudgetRevisionTable.GetRevisionDBName(),
                    0);

                foreach (DataRowView drv in budgetRevisionDataView)
                {
                    ABudgetRevisionRow bpr = (ABudgetRevisionRow)drv.Row;
                    bpr.Delete();
                }
            }
        }

        private void BudgetTypeChanged(System.Object sender, EventArgs e)
        {
            ClearBudgetTypeTextboxesExcept("None");

            pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
            pnlBudgetTypeSame.Visible = rbtSame.Checked;
            pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
            pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
            pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;

            if (FLoadCompleted && !FPetraUtilsObject.HasChanges)
            {
                if (rbtAdHoc.Checked)
                {
                    DisplayBudgetTypeAdhoc();
                }
                else if (rbtSame.Checked)
                {
                    DisplayBudgetTypeSame();
                }
                else if (rbtSplit.Checked)
                {
                    DisplayBudgetTypeSplit();
                }
                else if (rbtInflateN.Checked)
                {
                    DisplayBudgetTypeInflateN();
                }
                else      //rbtInflateBase.Checked
                {
                    DisplayBudgetTypeInflateBase();
                }
            }

            UpdateCurrencyCode();
        }

        private void ProcessBudgetTypeAdhoc(System.Object sender, EventArgs e)
        {
            decimal TotalAmount = 0;

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            decimal[] PeriodAmounts = new decimal[FNumberOfPeriods];
            PeriodAmounts[0] = Convert.ToDecimal(txtPeriod01Amount.NumberValueDecimal);
            PeriodAmounts[1] = Convert.ToDecimal(txtPeriod02Amount.NumberValueDecimal);
            PeriodAmounts[2] = Convert.ToDecimal(txtPeriod03Amount.NumberValueDecimal);
            PeriodAmounts[3] = Convert.ToDecimal(txtPeriod04Amount.NumberValueDecimal);
            PeriodAmounts[4] = Convert.ToDecimal(txtPeriod05Amount.NumberValueDecimal);
            PeriodAmounts[5] = Convert.ToDecimal(txtPeriod06Amount.NumberValueDecimal);
            PeriodAmounts[6] = Convert.ToDecimal(txtPeriod07Amount.NumberValueDecimal);
            PeriodAmounts[7] = Convert.ToDecimal(txtPeriod08Amount.NumberValueDecimal);
            PeriodAmounts[8] = Convert.ToDecimal(txtPeriod09Amount.NumberValueDecimal);
            PeriodAmounts[9] = Convert.ToDecimal(txtPeriod10Amount.NumberValueDecimal);
            PeriodAmounts[10] = Convert.ToDecimal(txtPeriod11Amount.NumberValueDecimal);
            PeriodAmounts[11] = Convert.ToDecimal(txtPeriod12Amount.NumberValueDecimal);

            if (FHas13Periods || FHas14Periods)
            {
                PeriodAmounts[12] = Convert.ToDecimal(txtPeriod13Amount.NumberValueDecimal);
            }

            if (FHas14Periods)
            {
                PeriodAmounts[13] = Convert.ToDecimal(txtPeriod14Amount.NumberValueDecimal);
            }

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                TotalAmount += PeriodAmounts[i - 1];

                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();
                    BudgetPeriodRow.BudgetBase = PeriodAmounts[i - 1];
                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                BudgetPeriodRow = null;
            }

            txtTotalAdhocAmount.NumberValueDecimal = TotalAmount;

            UpdatePeriodAmountsFromControls(FPreviouslySelectedDetailRow);
        }

        private void ProcessBudgetTypeSame(System.Object sender, EventArgs e)
        {
            decimal PeriodAmount = Convert.ToDecimal(txtAmount.NumberValueDecimal);
            decimal AnnualAmount = PeriodAmount * FNumberOfPeriods;

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();
                    BudgetPeriodRow.BudgetBase = PeriodAmount;
                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                BudgetPeriodRow = null;
            }

            txtSameTotalAmount.NumberValueDecimal = AnnualAmount;

            UpdatePeriodAmountsFromControls(FPreviouslySelectedDetailRow);
        }

        private void ProcessBudgetTypeSplit(System.Object sender, EventArgs e)
        {
            decimal AnnualAmount = Convert.ToDecimal(txtTotalSplitAmount.NumberValueDecimal);
            decimal PerPeriodAmount = Math.Truncate(AnnualAmount / FNumberOfPeriods);
            decimal LastPeriodAmount = AnnualAmount - PerPeriodAmount * (FNumberOfPeriods - 1);

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();

                    if (i < FNumberOfPeriods)
                    {
                        BudgetPeriodRow.BudgetBase = PerPeriodAmount;
                    }
                    else
                    {
                        BudgetPeriodRow.BudgetBase = LastPeriodAmount;
                    }

                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                BudgetPeriodRow = null;
            }

            txtPerPeriodAmount.NumberValueDecimal = PerPeriodAmount;
            txtLastPeriodAmount.NumberValueDecimal = LastPeriodAmount;

            UpdatePeriodAmountsFromControls(FPreviouslySelectedDetailRow);
        }

        private void ProcessBudgetTypeInflateN(System.Object sender, EventArgs e)
        {
            decimal TotalAmount = 0;
            decimal FirstPeriodAmount = Convert.ToDecimal(txtFirstPeriodAmount.NumberValueDecimal);
            int InflateAfterPeriod = Convert.ToInt16(txtInflateAfterPeriod.NumberValueInt);
            decimal InflationRate = Convert.ToDecimal(txtInflationRate.NumberValueDecimal) / 100;
            decimal SubsequentPeriodsAmount = FirstPeriodAmount * (1 + InflationRate);

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Control the inflate after period number
            if (InflateAfterPeriod < 0)
            {
                txtInflateAfterPeriod.NumberValueInt = 0;
                InflateAfterPeriod = 0;
            }
            else if (InflateAfterPeriod >= FNumberOfPeriods)
            {
                txtInflateAfterPeriod.NumberValueInt = (FNumberOfPeriods - 1);
                InflateAfterPeriod = (FNumberOfPeriods - 1);
            }

            TotalAmount = FirstPeriodAmount * InflateAfterPeriod + SubsequentPeriodsAmount * (FNumberOfPeriods - InflateAfterPeriod);

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();

                    if (i <= InflateAfterPeriod)
                    {
                        BudgetPeriodRow.BudgetBase = FirstPeriodAmount;
                    }
                    else
                    {
                        BudgetPeriodRow.BudgetBase = SubsequentPeriodsAmount;
                    }

                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                BudgetPeriodRow = null;
            }

            txtInflateNTotalAmount.NumberValueDecimal = TotalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);

            UpdatePeriodAmountsFromControls(FPreviouslySelectedDetailRow);
        }

        private void ProcessBudgetTypeInflateBase(System.Object sender, EventArgs e)
        {
            decimal TotalAmount = 0;

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            decimal[] PeriodAmounts = new decimal[FNumberOfPeriods];
            PeriodAmounts[0] = Convert.ToDecimal(txtPeriod1Amount.NumberValueDecimal);
            PeriodAmounts[1] = PeriodAmounts[0] * (1 + (Convert.ToDecimal(txtPeriod02Index.NumberValueDecimal) / 100));
            PeriodAmounts[2] = PeriodAmounts[1] * (1 + (Convert.ToDecimal(txtPeriod03Index.NumberValueDecimal) / 100));
            PeriodAmounts[3] = PeriodAmounts[2] * (1 + (Convert.ToDecimal(txtPeriod04Index.NumberValueDecimal) / 100));
            PeriodAmounts[4] = PeriodAmounts[3] * (1 + (Convert.ToDecimal(txtPeriod05Index.NumberValueDecimal) / 100));
            PeriodAmounts[5] = PeriodAmounts[4] * (1 + (Convert.ToDecimal(txtPeriod06Index.NumberValueDecimal) / 100));
            PeriodAmounts[6] = PeriodAmounts[5] * (1 + (Convert.ToDecimal(txtPeriod07Index.NumberValueDecimal) / 100));
            PeriodAmounts[7] = PeriodAmounts[6] * (1 + (Convert.ToDecimal(txtPeriod08Index.NumberValueDecimal) / 100));
            PeriodAmounts[8] = PeriodAmounts[7] * (1 + (Convert.ToDecimal(txtPeriod09Index.NumberValueDecimal) / 100));
            PeriodAmounts[9] = PeriodAmounts[8] * (1 + (Convert.ToDecimal(txtPeriod10Index.NumberValueDecimal) / 100));
            PeriodAmounts[10] = PeriodAmounts[9] * (1 + (Convert.ToDecimal(txtPeriod11Index.NumberValueDecimal) / 100));
            PeriodAmounts[11] = PeriodAmounts[10] * (1 + (Convert.ToDecimal(txtPeriod12Index.NumberValueDecimal) / 100));

            if (FHas13Periods || FHas14Periods)
            {
                PeriodAmounts[12] = PeriodAmounts[11] * (1 + (Convert.ToDecimal(txtPeriod13Index.NumberValueDecimal) / 100));
            }

            if (FHas14Periods)
            {
                PeriodAmounts[13] = PeriodAmounts[12] * (1 + (Convert.ToDecimal(txtPeriod14Index.NumberValueDecimal) / 100));
            }

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                TotalAmount += PeriodAmounts[i - 1];

                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();
                    BudgetPeriodRow.BudgetBase = PeriodAmounts[i - 1];
                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                BudgetPeriodRow = null;
            }

            txtInflateBaseTotalAmount.NumberValueDecimal = TotalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);

            UpdatePeriodAmountsFromControls(FPreviouslySelectedDetailRow);
        }

        private void DisplayBudgetTypeAdhoc()
        {
            decimal TotalAmount = 0;
            decimal CurrentPeriodAmount = 0;
            string TextboxName;

            ABudgetPeriodRow BudgetPeriodRow = null;
            TTxtCurrencyTextBox CurrencyTextbox = null;

            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                TextboxName = "txtPeriod" + i.ToString("00") + "Amount";

                foreach (Control ctrl in pnlBudgetTypeAdhoc.Controls)
                {
                    if (ctrl is TTxtCurrencyTextBox && (ctrl.Name == TextboxName))
                    {
                        CurrencyTextbox = (TTxtCurrencyTextBox)ctrl;
                        break;
                    }
                }

                if (BudgetPeriodRow != null)
                {
                    CurrentPeriodAmount = BudgetPeriodRow.BudgetBase;
                    CurrencyTextbox.NumberValueDecimal = CurrentPeriodAmount;
                    TotalAmount += CurrentPeriodAmount;
                }
                else
                {
                    CurrencyTextbox.NumberValueDecimal = 0;
                }

                BudgetPeriodRow = null;
                CurrencyTextbox = null;
            }

            txtTotalAdhocAmount.NumberValueDecimal = TotalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSame()
        {
            decimal TotalAmount = 0;
            decimal FirstPeriodAmount = 0;

            //Get the first period amount
            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(
                new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (BudgetPeriodRow != null)
            {
                FirstPeriodAmount = BudgetPeriodRow.BudgetBase;
                TotalAmount = FirstPeriodAmount * FNumberOfPeriods;
            }

            txtAmount.NumberValueDecimal = FirstPeriodAmount;
            txtSameTotalAmount.NumberValueDecimal = TotalAmount; //StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSplit()
        {
            decimal PerPeriodAmount = 0;
            decimal EndPeriodAmount = 0;

            //Find periods 1-(total periods-1) amount
            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(
                new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (BudgetPeriodRow != null)
            {
                PerPeriodAmount = BudgetPeriodRow.BudgetBase;
                BudgetPeriodRow = null;

                //Find period FNumberOfPeriods amount
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence,
                                                                                                   FNumberOfPeriods });

                if (BudgetPeriodRow != null)
                {
                    EndPeriodAmount = BudgetPeriodRow.BudgetBase;
                }
            }

            //Calculate the total amount
            txtPerPeriodAmount.NumberValueDecimal = PerPeriodAmount;
            txtLastPeriodAmount.NumberValueDecimal = EndPeriodAmount;
            txtTotalSplitAmount.NumberValueDecimal = PerPeriodAmount * (FNumberOfPeriods - 1) + EndPeriodAmount;
        }

        private void DisplayBudgetTypeInflateN()
        {
            decimal FirstPeriodAmount = 0;
            int InflateAfterPeriod = 0;
            decimal InflationRate = 0;
            decimal CurrentPeriodAmount;
            decimal TotalAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow = null;

            try
            {
                for (int i = 1; i <= FNumberOfPeriods; i++)
                {
                    BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                    if (BudgetPeriodRow != null)
                    {
                        CurrentPeriodAmount = BudgetPeriodRow.BudgetBase;

                        if (i == 1)
                        {
                            FirstPeriodAmount = CurrentPeriodAmount;
                        }
                        else
                        {
                            if (CurrentPeriodAmount != FirstPeriodAmount)
                            {
                                InflateAfterPeriod = i - 1;
                                InflationRate = (CurrentPeriodAmount - FirstPeriodAmount) / FirstPeriodAmount * 100;
                                TotalAmount = FirstPeriodAmount * InflateAfterPeriod + CurrentPeriodAmount * (FNumberOfPeriods - InflateAfterPeriod);
                                break;
                            }
                            else if (i == FNumberOfPeriods)     // and by implication CurrentPeriodAmount == FirstPeriodAmount
                            {
                                //This is an odd case that the user should never implement, but still needs to be covered.
                                //  It is equivalent to using BUDGET TYPE: SAME
                                InflateAfterPeriod = 0;
                                InflationRate = 0;
                                TotalAmount = CurrentPeriodAmount * FNumberOfPeriods;
                            }
                        }
                    }

                    BudgetPeriodRow = null;
                }

                txtFirstPeriodAmount.NumberValueDecimal = FirstPeriodAmount;
                txtInflateAfterPeriod.NumberValueInt = InflateAfterPeriod;
                txtInflationRate.NumberValueDecimal = InflationRate;
                txtInflateNTotalAmount.NumberValueDecimal = TotalAmount;     //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Catalog.GetString("Error in displaying budget type: InflateN: " + ex.Message));
            }
        }

        private void DisplayBudgetTypeInflateBase()
        {
            decimal TotalAmount = 0;

            decimal[] PeriodValues = new decimal[FNumberOfPeriods];
            decimal PriorPeriodAmount = 0;
            decimal CurrentPeriodAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow = null;

            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    CurrentPeriodAmount = BudgetPeriodRow.BudgetBase;

                    if (i == 1)
                    {
                        PeriodValues[0] = CurrentPeriodAmount;
                    }
                    else
                    {
                        if (PriorPeriodAmount == 0)
                        {
                            PeriodValues[i - 1] = 0;
                        }
                        else
                        {
                            PeriodValues[i - 1] = (CurrentPeriodAmount - PriorPeriodAmount) / PriorPeriodAmount * 100;
                        }
                    }

                    PriorPeriodAmount = CurrentPeriodAmount;
                    TotalAmount += CurrentPeriodAmount;
                }

                BudgetPeriodRow = null;
            }

            txtPeriod1Amount.NumberValueDecimal = PeriodValues[0];
            txtPeriod02Index.NumberValueDecimal = PeriodValues[1];
            txtPeriod03Index.NumberValueDecimal = PeriodValues[2];
            txtPeriod04Index.NumberValueDecimal = PeriodValues[3];
            txtPeriod05Index.NumberValueDecimal = PeriodValues[4];
            txtPeriod06Index.NumberValueDecimal = PeriodValues[5];
            txtPeriod07Index.NumberValueDecimal = PeriodValues[6];
            txtPeriod08Index.NumberValueDecimal = PeriodValues[7];
            txtPeriod09Index.NumberValueDecimal = PeriodValues[8];
            txtPeriod10Index.NumberValueDecimal = PeriodValues[9];
            txtPeriod11Index.NumberValueDecimal = PeriodValues[10];
            txtPeriod12Index.NumberValueDecimal = PeriodValues[11];

            if (FHas13Periods || FHas14Periods)
            {
                txtPeriod13Index.NumberValueDecimal = PeriodValues[12];
            }

            if (FHas14Periods)
            {
                txtPeriod14Index.NumberValueDecimal = PeriodValues[13];
            }

            txtInflateBaseTotalAmount.NumberValueDecimal = TotalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private string CurrencyCodeToUse()
        {
            string RetVal = string.Empty;

            AAccountRow CurrentAccountRow = null;

            if ((FAccountTable != null) && (cmbDetailAccountCode.SelectedIndex != -1) && (cmbDetailAccountCode.Count > 0)
                && (cmbDetailAccountCode.GetSelectedString() != null))
            {
                CurrentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, cmbDetailAccountCode.GetSelectedString() });

                if ((CurrentAccountRow != null) && CurrentAccountRow.ForeignCurrencyFlag)
                {
                    grpBudgetDetails.Text = "Budget Details (Foreign Account)";
                    RetVal = CurrentAccountRow.ForeignCurrencyCode;
                }
            }

            if (RetVal == string.Empty)
            {
                grpBudgetDetails.Text = "Budget Details";
                RetVal = FCurrencyCode;
            }

            return RetVal;
        }

        private void ClearBudgetTypeTextboxesExcept(string AExcludeType = "")
        {
            if (AExcludeType != MFinanceConstants.BUDGET_ADHOC)
            {
                //Adhoc controls
                txtPeriod01Amount.NumberValueDecimal = 0;
                txtPeriod02Amount.NumberValueDecimal = 0;
                txtPeriod03Amount.NumberValueDecimal = 0;
                txtPeriod04Amount.NumberValueDecimal = 0;
                txtPeriod05Amount.NumberValueDecimal = 0;
                txtPeriod06Amount.NumberValueDecimal = 0;
                txtPeriod07Amount.NumberValueDecimal = 0;
                txtPeriod08Amount.NumberValueDecimal = 0;
                txtPeriod09Amount.NumberValueDecimal = 0;
                txtPeriod10Amount.NumberValueDecimal = 0;
                txtPeriod11Amount.NumberValueDecimal = 0;
                txtPeriod12Amount.NumberValueDecimal = 0;
                txtPeriod13Amount.NumberValueDecimal = 0;
                txtPeriod14Amount.NumberValueDecimal = 0;
                txtTotalAdhocAmount.NumberValueDecimal = 0;
            }

            if (AExcludeType != MFinanceConstants.BUDGET_SAME)
            {
                //Same controls
                txtAmount.NumberValueDecimal = 0;
                txtSameTotalAmount.NumberValueDecimal = 0;
            }

            if (AExcludeType != MFinanceConstants.BUDGET_SPLIT)
            {
                //Split controls
                txtTotalSplitAmount.NumberValueDecimal = 0;
                txtPerPeriodAmount.NumberValueDecimal = 0;
                txtLastPeriodAmount.NumberValueDecimal = 0;
            }

            if (AExcludeType != MFinanceConstants.BUDGET_INFLATE_N)
            {
                //Inflate N controls
                txtFirstPeriodAmount.NumberValueDecimal = 0;
                txtInflateAfterPeriod.NumberValueInt = 0;
                txtInflationRate.NumberValueDecimal = 0;
                txtInflateNTotalAmount.NumberValueDecimal = 0;
            }

            if (AExcludeType != MFinanceConstants.BUDGET_INFLATE_BASE)
            {
                //Inflate Base controls
                txtPeriod1Amount.NumberValueDecimal = 0;
                txtPeriod02Index.NumberValueDecimal = 0;
                txtPeriod03Index.NumberValueDecimal = 0;
                txtPeriod04Index.NumberValueDecimal = 0;
                txtPeriod05Index.NumberValueDecimal = 0;
                txtPeriod06Index.NumberValueDecimal = 0;
                txtPeriod07Index.NumberValueDecimal = 0;
                txtPeriod08Index.NumberValueDecimal = 0;
                txtPeriod09Index.NumberValueDecimal = 0;
                txtPeriod10Index.NumberValueDecimal = 0;
                txtPeriod11Index.NumberValueDecimal = 0;
                txtPeriod12Index.NumberValueDecimal = 0;
                txtPeriod13Index.NumberValueDecimal = 0;
                txtPeriod14Index.NumberValueDecimal = 0;
                txtInflateBaseTotalAmount.NumberValueDecimal = 0;
            }
        }

        private void UpdateCurrencyCode()
        {
            string CurrencyCode = CurrencyCodeToUse();

            // Adhoc
            txtPeriod01Amount.CurrencyCode = CurrencyCode;
            txtPeriod02Amount.CurrencyCode = CurrencyCode;
            txtPeriod03Amount.CurrencyCode = CurrencyCode;
            txtPeriod04Amount.CurrencyCode = CurrencyCode;
            txtPeriod05Amount.CurrencyCode = CurrencyCode;
            txtPeriod06Amount.CurrencyCode = CurrencyCode;
            txtPeriod07Amount.CurrencyCode = CurrencyCode;
            txtPeriod08Amount.CurrencyCode = CurrencyCode;
            txtPeriod09Amount.CurrencyCode = CurrencyCode;
            txtPeriod10Amount.CurrencyCode = CurrencyCode;
            txtPeriod11Amount.CurrencyCode = CurrencyCode;
            txtPeriod12Amount.CurrencyCode = CurrencyCode;
            txtPeriod13Amount.CurrencyCode = CurrencyCode;
            txtPeriod14Amount.CurrencyCode = CurrencyCode;
            txtTotalAdhocAmount.CurrencyCode = CurrencyCode;

            // Same
            txtAmount.CurrencyCode = CurrencyCode;
            txtSameTotalAmount.CurrencyCode = CurrencyCode;

            // Split
            txtPerPeriodAmount.CurrencyCode = CurrencyCode;
            txtLastPeriodAmount.CurrencyCode = CurrencyCode;
            txtTotalSplitAmount.CurrencyCode = CurrencyCode;

            // Inflate N
            txtFirstPeriodAmount.CurrencyCode = CurrencyCode;
            txtInflateNTotalAmount.CurrencyCode = CurrencyCode;

            // Inflate Base
            txtPeriod1Amount.CurrencyCode = CurrencyCode;
            txtInflateBaseTotalAmount.CurrencyCode = CurrencyCode;
        }

        private void ShowDetailsManual(BudgetTDSABudgetRow ARow)
        {
            ClearBudgetTypeTextboxesExcept("None");
            UpdateCurrencyCode();

            if ((ARow == null) || ((grdDetails.Rows.Count < 2) && rgrBudgetTypeCode.Enabled))
            {
                FBudgetSequence = -1;
                EnableBudgetEntry(false);
                return;
            }
            else if (rgrBudgetTypeCode.Enabled == false)
            {
                EnableBudgetEntry(true);
            }

            if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_SPLIT)
            {
                rbtSplit.Checked = true;
                DisplayBudgetTypeSplit();
            }
            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_ADHOC)
            {
                rbtAdHoc.Checked = true;
                DisplayBudgetTypeAdhoc();
            }
            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_SAME)
            {
                rbtSame.Checked = true;
                DisplayBudgetTypeSame();
            }
            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_INFLATE_BASE)
            {
                rbtInflateBase.Checked = true;
                DisplayBudgetTypeInflateBase();
            }
            else          //ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_N
            {
                rbtInflateN.Checked = true;
                DisplayBudgetTypeInflateN();
            }

            FBudgetSequence = ARow.BudgetSequence;

            pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
            pnlBudgetTypeSame.Visible = rbtSame.Checked;
            pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
            pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
            pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;
        }

        private bool GetDetailDataFromControlsManual(BudgetTDSABudgetRow ARow)
        {
            if (ARow != null)
            {
                ARow.BeginEdit();

                if (rbtAdHoc.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeAdhoc(null, null);
                        ClearBudgetTypeTextboxesExcept(MFinanceConstants.BUDGET_ADHOC);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_ADHOC;
                }
                else if (rbtSame.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeSame(null, null);
                        ClearBudgetTypeTextboxesExcept(MFinanceConstants.BUDGET_SAME);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SAME;
                }
                else if (rbtSplit.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeSplit(null, null);
                        ClearBudgetTypeTextboxesExcept(MFinanceConstants.BUDGET_SPLIT);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SPLIT;
                }
                else if (rbtInflateN.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeInflateN(null, null);
                        ClearBudgetTypeTextboxesExcept(MFinanceConstants.BUDGET_INFLATE_N);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_N;
                }
                else      //rbtInflateBase.Checked
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeInflateBase(null, null);
                        ClearBudgetTypeTextboxesExcept(MFinanceConstants.BUDGET_INFLATE_BASE);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_BASE;
                }

                //Write to Budget custom fields
                UpdatePeriodAmountsFromControls(ARow);

                //ARow.Year & ARow.Revision are never set here, but on record creation

                ARow.EndEdit();
            }

            return true;
        }

        private int CreateBudgetRevisionRow(int ALedgerNumber, int AYear)
        {
            //Always to be zero for now
            int NewRevision = 0;

            if (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, NewRevision }) == null)
            {
                ABudgetRevisionRow BudgetRevisionRow = FMainDS.ABudgetRevision.NewRowTyped();

                BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                BudgetRevisionRow.Year = AYear;

                BudgetRevisionRow.Revision = NewRevision;
                FMainDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
            }

            return NewRevision;
        }

        private void CostCentreCodeDetailChanged(object sender, EventArgs e)
        {
            string CurrentCostCentre = string.Empty;
            bool CostCentreActive = true;

            if ((FLoadCompleted == false) || (FPreviouslySelectedDetailRow == null)
                || (cmbDetailCostCentreCode.GetSelectedString() == String.Empty) || (cmbDetailCostCentreCode.SelectedIndex == -1))
            {
                return;
            }

            CurrentCostCentre = cmbDetailCostCentreCode.GetSelectedString();
            CostCentreActive = CostCentreIsActive();

            //If change from combo action as opposed to moving rows
            if (FPreviouslySelectedDetailRow.BudgetSequence == FBudgetSequence)
            {
                if ((cmbDetailAccountCode.SelectedIndex != -1) && !CostCentreAccountCombinationIsUnique())
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The Cost Centre ({0})/Account Code ({1}) combination is already used in this budget."),
                            CurrentCostCentre,
                            cmbDetailAccountCode.GetSelectedString()));
                    cmbDetailCostCentreCode.SelectedIndex = -1;
                }
                else if (!CostCentreActive)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString("Cost Centre Code {0} is set to Inactive. Do you want to select it?"),
                                CurrentCostCentre),
                            Catalog.GetString("Confirm Cost Centre"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                    {
                        cmbDetailCostCentreCode.SelectedIndex = -1;
                    }
                }
            }
        }

        /// <summary>
        /// if the account code changes, analysis types/attributes  have to be updated
        /// </summary>
        private void AccountCodeDetailChanged(object sender, EventArgs e)
        {
            string CurrentAccount = string.Empty;
            bool AccountActive = true;

            if ((FLoadCompleted == false) || (FPreviouslySelectedDetailRow == null) || (cmbDetailAccountCode.GetSelectedString() == String.Empty)
                || (cmbDetailAccountCode.SelectedIndex == -1))
            {
                return;
            }

            CurrentAccount = cmbDetailAccountCode.GetSelectedString();
            AccountActive = AccountIsActive(CurrentAccount);

            //If change from combo action as opposed to moving rows
            if (FPreviouslySelectedDetailRow.BudgetSequence == FBudgetSequence)
            {
                if ((cmbDetailCostCentreCode.SelectedIndex != -1) && !CostCentreAccountCombinationIsUnique())
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The Cost Centre ({0})/Account Code ({1}) combination is already used in this budget."),
                            cmbDetailCostCentreCode.GetSelectedString(),
                            cmbDetailAccountCode.GetSelectedString()));
                    cmbDetailAccountCode.SelectedIndex = -1;
                }
                else if (!AccountActive)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString("Account Code {0} is set to Inactive. Do you want to select it?"),
                                CurrentAccount),
                            Catalog.GetString("Confirm Account"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
                    {
                        cmbDetailAccountCode.SelectedIndex = -1;
                    }
                }
            }

            UpdateCurrencyCode();
        }

        private bool CostCentreAccountCombinationIsUnique()
        {
            DataRow[] FoundRows = FMainDS.ABudget.Select(String.Format("{0}={1} And {2}={3} And {4}=0 And {5}='{6}' And {7}='{8}' And {9}<>{10}",
                    ABudgetTable.GetLedgerNumberDBName(),
                    FLedgerNumber,
                    ABudgetTable.GetYearDBName(),
                    cmbSelectBudgetYear.GetSelectedString(),
                    ABudgetTable.GetRevisionDBName(),
                    ABudgetTable.GetCostCentreCodeDBName(),
                    cmbDetailCostCentreCode.GetSelectedString(),
                    ABudgetTable.GetAccountCodeDBName(),
                    cmbDetailAccountCode.GetSelectedString(),
                    ABudgetTable.GetBudgetSequenceDBName(),
                    FPreviouslySelectedDetailRow.BudgetSequence));

            return FoundRows.Length == 0;
        }

        private bool AccountIsActive(string AAccountCode = "")
        {
            bool RetVal = true;

            AAccountRow CurrentAccountRow = null;

            //If empty, read value from combo
            if (AAccountCode == string.Empty)
            {
                if ((FAccountTable != null) && (cmbDetailAccountCode.SelectedIndex != -1) && (cmbDetailAccountCode.Count > 0)
                    && (cmbDetailAccountCode.GetSelectedString() != null))
                {
                    AAccountCode = cmbDetailAccountCode.GetSelectedString();
                }
            }

            CurrentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });

            if (CurrentAccountRow != null)
            {
                RetVal = CurrentAccountRow.AccountActiveFlag;
            }

            return RetVal;
        }

        private bool CostCentreIsActive(string ACostCentreCode = "")
        {
            bool RetVal = true;

            ACostCentreRow CurrentCostCentreRow = null;

            //If empty, read value from combo
            if (ACostCentreCode == string.Empty)
            {
                if ((FCostCentreTable != null) && (cmbDetailCostCentreCode.SelectedIndex != -1) && (cmbDetailCostCentreCode.Count > 0)
                    && (cmbDetailCostCentreCode.GetSelectedString() != null))
                {
                    ACostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
                }
            }

            CurrentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });

            if (CurrentCostCentreRow != null)
            {
                RetVal = CurrentCostCentreRow.CostCentreActiveFlag;
            }

            return RetVal;
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            // We need to deal with the spaces in the combo box text
            AFilterString = AFilterString.Replace("Ad hoc", MFinanceConstants.BUDGET_ADHOC);
            AFilterString = AFilterString.Replace("Inflate base", MFinanceConstants.BUDGET_INFLATE_BASE);
            AFilterString = AFilterString.Replace("Inflate n", MFinanceConstants.BUDGET_INFLATE_N);
        }

        private void RunOnceOnActivationManual()
        {
            cmbDetailAccountCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_ACCOUNT;
            cmbDetailCostCentreCode.AttachedLabel.Text = TFinanceControls.SELECT_VALID_COST_CENTRE;
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
            ABudgetRow row = (ABudgetRow)ADataRowView.Row;

            switch (AContext)
            {
                case BoundGridImage.AnnotationContextEnum.AccountCode:
                    return !AccountIsActive(row.AccountCode);

                case BoundGridImage.AnnotationContextEnum.CostCentreCode:
                    return !CostCentreIsActive(row.CostCentreCode);
            }

            return false;
        }

        #endregion
    }
}