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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance;
//using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    public partial class TFrmMaintainBudget
    {
        private Int32 FLedgerNumber;

        private Int32 FCurrentBudgetYear;

        private Int32 FBudgetSequence = -1;

        //This may be 13 so allow for it
        private Int32 FNumberOfPeriods;
        private bool FHas13Periods;
        private bool FHas14Periods;

        private bool FLoadCompleted = false;
        private bool FRejectYearChange = false;

        private TDlgSelectCSVSeparator FdlgSeparator;
        private String FCurrencyCode = "";

        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                LoadBudgets();

                //UpdateRecordNumberDisplay();
            }
        }

        private void LoadBudgets()
        {
            Console.WriteLine("LoadBudgets() ...");
            DateTime dtStart = DateTime.Now;

            FMainDS = TRemote.MFinance.Budget.WebConnectors.LoadBudget(FLedgerNumber);

            Console.WriteLine("Budgets loaded -- {0} ms", (DateTime.Now - dtStart).TotalMilliseconds);

            //Prepare form for correct number of periods
            //FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadLedgerInfo(FLedgerNumber));

            ALedgerRow ledgerRow = (ALedgerRow)FMainDS.ALedger.Rows[0];
            FNumberOfPeriods = ledgerRow.NumberOfAccountingPeriods;
            FCurrencyCode = ledgerRow.BaseCurrency;

            FHas13Periods = (FNumberOfPeriods == 13);
            FHas14Periods = (FNumberOfPeriods == 14);

            // to get an empty ABudgetFee table, instead of null reference
            FMainDS.Merge(new BudgetTDS());

            //Setup form and controls
            this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";
            InitialiseControls();

            if (!int.TryParse(cmbSelectBudgetYear.GetSelectedString(), out FCurrentBudgetYear))
            {
                FCurrentBudgetYear = TFinanceControls.GetLedgerCurrentFinancialYear(FLedgerNumber);
            }

            SetBudgetDefaultView();
            //grdDetails.AutoSizeCells();

            SelectRowInGrid(1);

            RefreshComboLabels();

            Console.WriteLine("Load complete  {0} ms", (DateTime.Now - dtStart).TotalMilliseconds);
            FLoadCompleted = true;
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
                string accountCode = row[ABudgetTable.ColumnAccountCodeId].ToString();
                return !AccountIsActive(accountCode);
            };

            SourceGrid.Conditions.ConditionView conditionCostCentreCodeActive = new SourceGrid.Conditions.ConditionView(strikeoutCell);
            conditionCostCentreCodeActive.EvaluateFunction = delegate(SourceGrid.DataGridColumn column, int gridRow, object itemRow)
            {
                DataRowView row = (DataRowView)itemRow;
                string costCentreCode = row[ABudgetTable.ColumnCostCentreCodeId].ToString();
                return !CostCentreIsActive(costCentreCode);
            };

            //Add conditions to columns
            int indexOfCostCentreCodeDataColumn = 0;
            int indexOfAccountCodeDataColumn = 1;

            grdDetails.Columns[indexOfCostCentreCodeDataColumn].Conditions.Add(conditionCostCentreCodeActive);
            grdDetails.Columns[indexOfAccountCodeDataColumn].Conditions.Add(conditionAccountCodeActive);
        }

        private void SetBudgetDefaultView()
        {
            DataView myDataView = FMainDS.ABudget.DefaultView;

            myDataView.AllowNew = false;
            myDataView.Sort = String.Format("{0} ASC, {1} ASC", ABudgetTable.GetCostCentreCodeDBName(), ABudgetTable.GetAccountCodeDBName());
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            string rowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), FCurrentBudgetYear);
            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, true);
            FFilterAndFindObject.ApplyFilter();
            UpdateRecordNumberDisplay();
            FFilterAndFindObject.SetRecordNumberDisplayProperties();
        }

        private void InitialiseControls()
        {
            SetupExtraGridFunctionality();

            TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbSelectBudgetYear, FLedgerNumber, true);

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false, true);

            // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true, true);

            bool bMoreThan12 = (FNumberOfPeriods > 12);
            bool bMoreThan13 = (FNumberOfPeriods > 13);

            txtPeriod13Amount.Visible = bMoreThan12;
            lblPeriod13Amount.Visible = bMoreThan12;
            txtPeriod13Index.Visible = bMoreThan12;
            lblPeriod13Index.Visible = bMoreThan12;

            txtPeriod14Amount.Visible = bMoreThan13;
            lblPeriod14Amount.Visible = bMoreThan13;
            txtPeriod14Index.Visible = bMoreThan13;
            lblPeriod14Index.Visible = bMoreThan13;

            lblPerPeriodAmount.Text = "Amount for periods 1 to " + (FNumberOfPeriods - 1).ToString() + ":";
            lblLastPeriodAmount.Text = "Amount for period " + FNumberOfPeriods.ToString() + ":";
        }

        private void NewRowManual(ref ABudgetRow ARow)
        {
            if (!cmbDetailAccountCode.Enabled)
            {
                EnableBudgetEntry(true);
            }

            ARow.BudgetSequence = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_budget));
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, FCurrentBudgetYear);
            ARow.Year = FCurrentBudgetYear;

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

        private void SelectBudgetYear(Object sender, EventArgs e)
        {
            if (FLoadCompleted)
            {
                if (FRejectYearChange)
                {
                    return;
                }

                if (FPetraUtilsObject.HasChanges)
                {
                    FRejectYearChange = true;
                    MessageBox.Show(Catalog.GetString("Please save changes before attempting to change year."));
                    cmbSelectBudgetYear.SetSelectedInt32(FCurrentBudgetYear);
                    return;
                }

                if (int.TryParse(cmbSelectBudgetYear.GetSelectedString(), out FCurrentBudgetYear))
                {
                    SetBudgetDefaultView();

                    SelectRowInGrid(1);

                    if (FPetraUtilsObject.HasChanges)
                    {
                        //Change of year clears boxes in some circumstances so need to save
                        SaveChanges();
                    }
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
            ADeletionQuestion = String.Format(Catalog.GetString(
                    "You have chosen to delete this budget (Cost Centre: {0}, Account: {1}, Type: {2}, Revision: {3}).{4}{4}Do you really want to delete it?"),
                ARowToDelete.CostCentreCode,
                ARowToDelete.AccountCode,
                ARowToDelete.BudgetTypeCode,
                ARowToDelete.Revision,
                Environment.NewLine);
            return true;
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

            int budgetSequence = ARowToDelete.BudgetSequence;
            DeleteBudgetPeriodData(budgetSequence);
            ARowToDelete.Delete();

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

        private void ImportBudget(System.Object sender, EventArgs e)
        {
            decimal numRecsImported = 0;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog dialog = new OpenFileDialog();

            string exportPath = TClientSettings.GetExportPath();
            string fullPath = TUserDefaults.GetStringDefault("Imp Filename",
                exportPath + Path.DirectorySeparatorChar + "import.csv");
            TImportExportDialogs.SetOpenFileDialogFilePathAndName(dialog, fullPath, exportPath);

            dialog.Title = Catalog.GetString("Import budget(s) from csv file");
            dialog.Filter = Catalog.GetString("Budget files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
            TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(fullPath));

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);

                try
                {
                    Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                    if (!fileCanOpen)
                    {
                        throw new Exception(String.Format(Catalog.GetString("File {0} Cannot be opened."), dialog.FileName));
                    }

                    FdlgSeparator.DateFormat = dateFormatString;

                    if (impOptions.Length > 1)
                    {
                        FdlgSeparator.NumberFormat = impOptions.Substring(1);
                    }

                    FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                    if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                    {
                        TVerificationResultCollection AMessages;

                        string[] FdlgSeparatorVal = new string[] {
                            FdlgSeparator.SelectedSeparator, FdlgSeparator.DateFormat, FdlgSeparator.NumberFormat
                        };

                        //TODO return the budget from the year, and -99 for fail
                        numRecsImported = TRemote.MFinance.Budget.WebConnectors.ImportBudgets(FLedgerNumber,
                            FCurrentBudgetYear,
                            dialog.FileName,
                            FdlgSeparatorVal,
                            ref FMainDS,
                            out AMessages);
                    }
                }
                catch (Exception ex)
                {
                    numRecsImported = -2;
                    MessageBox.Show(ex.Message, Catalog.GetString("Budget Import"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (numRecsImported > 0)
                {
                    Int32 totalImported = Convert.ToInt32(Math.Truncate(numRecsImported));
                    Int32 totalUpdated = Convert.ToInt32((numRecsImported - totalImported) * 10000);

                    string msg = String.Format(Catalog.GetString("{0} budget records imported successfully!"), totalImported);

                    if (totalUpdated > 0)
                    {
                        msg += Environment.NewLine + Environment.NewLine + String.Format(Catalog.GetString(
                                "({0} of which updated existing budgets)"), totalImported);
                    }

                    MessageBox.Show(msg,
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SetBudgetDefaultView();

                    SelectRowInGrid(1);

                    FPetraUtilsObject.SetChangedFlag();
                }
                else if (numRecsImported == -1)
                {
                    MessageBox.Show(Catalog.GetString("The year contained in the import file is different to the current selected year."));

                    SelectRowInGrid(1);
                }
                else if (numRecsImported == -2)
                {
                    SelectRowInGrid(1);
                }
                else //0
                {
                    MessageBox.Show(Catalog.GetString("No records found to import"));
                }
            }
        }

        // This is not used (and imcomplete...)
        private void ExportBudget(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                // without save the server does not have the current changes, so forbid it.
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
            ABudgetPeriodRow budgetPeriodRow = null;

            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.Delete();
                }

                budgetPeriodRow = null;
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
            decimal totalAmount = 0;

            decimal[] periodAmounts = new decimal[FNumberOfPeriods];
            periodAmounts[0] = Convert.ToDecimal(txtPeriod01Amount.NumberValueDecimal);
            periodAmounts[1] = Convert.ToDecimal(txtPeriod02Amount.NumberValueDecimal);
            periodAmounts[2] = Convert.ToDecimal(txtPeriod03Amount.NumberValueDecimal);
            periodAmounts[3] = Convert.ToDecimal(txtPeriod04Amount.NumberValueDecimal);
            periodAmounts[4] = Convert.ToDecimal(txtPeriod05Amount.NumberValueDecimal);
            periodAmounts[5] = Convert.ToDecimal(txtPeriod06Amount.NumberValueDecimal);
            periodAmounts[6] = Convert.ToDecimal(txtPeriod07Amount.NumberValueDecimal);
            periodAmounts[7] = Convert.ToDecimal(txtPeriod08Amount.NumberValueDecimal);
            periodAmounts[8] = Convert.ToDecimal(txtPeriod09Amount.NumberValueDecimal);
            periodAmounts[9] = Convert.ToDecimal(txtPeriod10Amount.NumberValueDecimal);
            periodAmounts[10] = Convert.ToDecimal(txtPeriod11Amount.NumberValueDecimal);
            periodAmounts[11] = Convert.ToDecimal(txtPeriod12Amount.NumberValueDecimal);

            if (FHas13Periods || FHas14Periods)
            {
                periodAmounts[12] = Convert.ToDecimal(txtPeriod13Amount.NumberValueDecimal);
            }

            if (FHas14Periods)
            {
                periodAmounts[13] = Convert.ToDecimal(txtPeriod14Amount.NumberValueDecimal);
            }

            int budgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow budgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                totalAmount += periodAmounts[i - 1];

                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { budgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.BeginEdit();
                    budgetPeriodRow.BudgetBase = periodAmounts[i - 1];
                    budgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                budgetPeriodRow = null;
            }

            txtTotalAdhocAmount.NumberValueDecimal = totalAmount;
        }

        private void ProcessBudgetTypeSame(System.Object sender, EventArgs e)
        {
            decimal periodAmount = Convert.ToDecimal(txtAmount.NumberValueDecimal);
            decimal annualAmount = periodAmount * FNumberOfPeriods;

            int budgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow budgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { budgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.BeginEdit();
                    budgetPeriodRow.BudgetBase = periodAmount;
                    budgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                budgetPeriodRow = null;
            }

            txtSameTotalAmount.NumberValueDecimal = annualAmount;
        }

        private void ProcessBudgetTypeSplit(System.Object sender, EventArgs e)
        {
            decimal annualAmount = 0;
            decimal perPeriodAmount = 0;
            decimal lastPeriodAmount = 0;

            annualAmount = Convert.ToDecimal(txtTotalSplitAmount.NumberValueDecimal);
            perPeriodAmount = Math.Truncate(annualAmount / FNumberOfPeriods);
            lastPeriodAmount = annualAmount - perPeriodAmount * (FNumberOfPeriods - 1);

            int budgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow budgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { budgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.BeginEdit();

                    if (i < FNumberOfPeriods)
                    {
                        budgetPeriodRow.BudgetBase = perPeriodAmount;
                    }
                    else
                    {
                        budgetPeriodRow.BudgetBase = lastPeriodAmount;
                    }

                    budgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                budgetPeriodRow = null;
            }

            txtPerPeriodAmount.NumberValueDecimal = perPeriodAmount;
            txtLastPeriodAmount.NumberValueDecimal = lastPeriodAmount;
        }

        private void ProcessBudgetTypeInflateN(System.Object sender, EventArgs e)
        {
            decimal totalAmount = 0;
            decimal firstPeriodAmount = Convert.ToDecimal(txtFirstPeriodAmount.NumberValueDecimal);
            int inflateAfterPeriod = Convert.ToInt16(txtInflateAfterPeriod.NumberValueInt);
            decimal inflationRate = Convert.ToDecimal(txtInflationRate.NumberValueDecimal) / 100;
            decimal subsequentPeriodsAmount = firstPeriodAmount * (1 + inflationRate);

            int budgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow budgetPeriodRow = null;

            //Control the inflate after period number
            if (inflateAfterPeriod < 0)
            {
                txtInflateAfterPeriod.NumberValueInt = 0;
                inflateAfterPeriod = 0;
            }
            else if (inflateAfterPeriod >= FNumberOfPeriods)
            {
                txtInflateAfterPeriod.NumberValueInt = (FNumberOfPeriods - 1);
                inflateAfterPeriod = (FNumberOfPeriods - 1);
            }

            totalAmount = firstPeriodAmount * inflateAfterPeriod + subsequentPeriodsAmount * (FNumberOfPeriods - inflateAfterPeriod);

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { budgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.BeginEdit();

                    if (i <= inflateAfterPeriod)
                    {
                        budgetPeriodRow.BudgetBase = firstPeriodAmount;
                    }
                    else
                    {
                        budgetPeriodRow.BudgetBase = subsequentPeriodsAmount;
                    }

                    budgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                budgetPeriodRow = null;
            }

            txtInflateNTotalAmount.NumberValueDecimal = totalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void ProcessBudgetTypeInflateBase(System.Object sender, EventArgs e)
        {
            decimal totalAmount = 0;

            decimal[] periodAmounts = new decimal[FNumberOfPeriods];
            periodAmounts[0] = Convert.ToDecimal(txtPeriod1Amount.NumberValueDecimal);
            periodAmounts[1] = periodAmounts[0] * (1 + (Convert.ToDecimal(txtPeriod02Index.NumberValueDecimal) / 100));
            periodAmounts[2] = periodAmounts[1] * (1 + (Convert.ToDecimal(txtPeriod03Index.NumberValueDecimal) / 100));
            periodAmounts[3] = periodAmounts[2] * (1 + (Convert.ToDecimal(txtPeriod04Index.NumberValueDecimal) / 100));
            periodAmounts[4] = periodAmounts[3] * (1 + (Convert.ToDecimal(txtPeriod05Index.NumberValueDecimal) / 100));
            periodAmounts[5] = periodAmounts[4] * (1 + (Convert.ToDecimal(txtPeriod06Index.NumberValueDecimal) / 100));
            periodAmounts[6] = periodAmounts[5] * (1 + (Convert.ToDecimal(txtPeriod07Index.NumberValueDecimal) / 100));
            periodAmounts[7] = periodAmounts[6] * (1 + (Convert.ToDecimal(txtPeriod08Index.NumberValueDecimal) / 100));
            periodAmounts[8] = periodAmounts[7] * (1 + (Convert.ToDecimal(txtPeriod09Index.NumberValueDecimal) / 100));
            periodAmounts[9] = periodAmounts[8] * (1 + (Convert.ToDecimal(txtPeriod10Index.NumberValueDecimal) / 100));
            periodAmounts[10] = periodAmounts[9] * (1 + (Convert.ToDecimal(txtPeriod11Index.NumberValueDecimal) / 100));
            periodAmounts[11] = periodAmounts[10] * (1 + (Convert.ToDecimal(txtPeriod12Index.NumberValueDecimal) / 100));

            if (FHas13Periods || FHas14Periods)
            {
                periodAmounts[12] = periodAmounts[11] * (1 + (Convert.ToDecimal(txtPeriod13Index.NumberValueDecimal) / 100));
            }

            if (FHas14Periods)
            {
                periodAmounts[13] = periodAmounts[12] * (1 + (Convert.ToDecimal(txtPeriod14Index.NumberValueDecimal) / 100));
            }

            int budgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow budgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                totalAmount += periodAmounts[i - 1];

                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { budgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    budgetPeriodRow.BeginEdit();
                    budgetPeriodRow.BudgetBase = periodAmounts[i - 1];
                    budgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show(Catalog.GetString("Error trying to write BudgetPeriod values"));
                }

                budgetPeriodRow = null;
            }

            txtInflateBaseTotalAmount.NumberValueDecimal = totalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeAdhoc()
        {
            decimal totalAmount = 0;
            decimal currentPeriodAmount = 0;
            string textboxName;

            ABudgetPeriodRow budgetPeriodRow;
            TTxtCurrencyTextBox txt = null;

            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                textboxName = "txtPeriod" + i.ToString("00") + "Amount";

                foreach (Control ctrl in pnlBudgetTypeAdhoc.Controls)
                {
                    if (ctrl is TTxtCurrencyTextBox && (ctrl.Name == textboxName))
                    {
                        txt = (TTxtCurrencyTextBox)ctrl;
                        break;
                    }
                }

                if (budgetPeriodRow != null)
                {
                    currentPeriodAmount = budgetPeriodRow.BudgetBase;
                    txt.NumberValueDecimal = currentPeriodAmount;
                    totalAmount += currentPeriodAmount;
                }
                else
                {
                    txt.NumberValueDecimal = 0;
                }

                budgetPeriodRow = null;
                txt = null;
            }

            txtTotalAdhocAmount.NumberValueDecimal = totalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSame()
        {
            decimal totalAmount = 0;
            decimal firstPeriodAmount = 0;

            ABudgetPeriodRow budgetPeriodRow;

            //Get the first period amount
            budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (budgetPeriodRow != null)
            {
                firstPeriodAmount = budgetPeriodRow.BudgetBase;
                totalAmount = firstPeriodAmount * FNumberOfPeriods;
            }

            txtAmount.NumberValueDecimal = firstPeriodAmount;
            txtSameTotalAmount.NumberValueDecimal = totalAmount; //StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSplit()
        {
            decimal perPeriodAmount = 0;
            decimal endPeriodAmount = 0;

            ABudgetPeriodRow budgetPeriodRow;

            //Find periods 1-(total periods-1) amount
            budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (budgetPeriodRow != null)
            {
                perPeriodAmount = budgetPeriodRow.BudgetBase;
                budgetPeriodRow = null;

                //Find period FNumberOfPeriods amount
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence,
                                                                                                   FNumberOfPeriods });

                if (budgetPeriodRow != null)
                {
                    endPeriodAmount = budgetPeriodRow.BudgetBase;
                }
            }

            //Calculate the total amount
            txtPerPeriodAmount.NumberValueDecimal = perPeriodAmount;
            txtLastPeriodAmount.NumberValueDecimal = endPeriodAmount;
            txtTotalSplitAmount.NumberValueDecimal = perPeriodAmount * (FNumberOfPeriods - 1) + endPeriodAmount;
        }

        private void DisplayBudgetTypeInflateN()
        {
            decimal firstPeriodAmount = 0;
            int inflateAfterPeriod = 0;
            decimal inflationRate = 0;
            decimal currentPeriodAmount;
            decimal totalAmount = 0;

            ABudgetPeriodRow budgetPeriodRow;

            try
            {
                for (int i = 1; i <= FNumberOfPeriods; i++)
                {
                    budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                    if (budgetPeriodRow != null)
                    {
                        currentPeriodAmount = budgetPeriodRow.BudgetBase;

                        if (i == 1)
                        {
                            firstPeriodAmount = currentPeriodAmount;
                        }
                        else
                        {
                            if (currentPeriodAmount != firstPeriodAmount)
                            {
                                inflateAfterPeriod = i - 1;
                                inflationRate = (currentPeriodAmount - firstPeriodAmount) / firstPeriodAmount * 100;
                                totalAmount = firstPeriodAmount * inflateAfterPeriod + currentPeriodAmount * (FNumberOfPeriods - inflateAfterPeriod);
                                break;
                            }
                            else if (i == FNumberOfPeriods)     // and by implication CurrentPeriodAmount == FirstPeriodAmount
                            {
                                //This is an odd case that the user should never implement, but still needs to be covered.
                                //  It is equivalent to using BUDGET TYPE: SAME
                                inflateAfterPeriod = 0;
                                inflationRate = 0;
                                totalAmount = currentPeriodAmount * FNumberOfPeriods;
                            }
                        }
                    }

                    budgetPeriodRow = null;
                }

                txtFirstPeriodAmount.NumberValueDecimal = firstPeriodAmount;
                txtInflateAfterPeriod.NumberValueInt = inflateAfterPeriod;
                txtInflationRate.NumberValueDecimal = inflationRate;
                txtInflateNTotalAmount.NumberValueDecimal = totalAmount;     //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Catalog.GetString("Error in displaying budget type: InflateN: " + ex.Message));
            }
        }

        private void DisplayBudgetTypeInflateBase()
        {
            decimal totalAmount = 0;

            decimal[] periodValues = new decimal[FNumberOfPeriods];
            decimal priorPeriodAmount = 0;
            decimal currentPeriodAmount = 0;

            ABudgetPeriodRow budgetPeriodRow;

            for (int i = 1; i <= FNumberOfPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    currentPeriodAmount = budgetPeriodRow.BudgetBase;

                    if (i == 1)
                    {
                        periodValues[0] = currentPeriodAmount;
                    }
                    else
                    {
                        if (priorPeriodAmount == 0)
                        {
                            periodValues[i - 1] = 0;
                        }
                        else
                        {
                            periodValues[i - 1] = (currentPeriodAmount - priorPeriodAmount) / priorPeriodAmount * 100;
                        }
                    }

                    priorPeriodAmount = currentPeriodAmount;
                    totalAmount += currentPeriodAmount;
                }

                budgetPeriodRow = null;
            }

            txtPeriod1Amount.NumberValueDecimal = periodValues[0];
            txtPeriod02Index.NumberValueDecimal = periodValues[1];
            txtPeriod03Index.NumberValueDecimal = periodValues[2];
            txtPeriod04Index.NumberValueDecimal = periodValues[3];
            txtPeriod05Index.NumberValueDecimal = periodValues[4];
            txtPeriod06Index.NumberValueDecimal = periodValues[5];
            txtPeriod07Index.NumberValueDecimal = periodValues[6];
            txtPeriod08Index.NumberValueDecimal = periodValues[7];
            txtPeriod09Index.NumberValueDecimal = periodValues[8];
            txtPeriod10Index.NumberValueDecimal = periodValues[9];
            txtPeriod11Index.NumberValueDecimal = periodValues[10];
            txtPeriod12Index.NumberValueDecimal = periodValues[11];

            if (FHas13Periods || FHas14Periods)
            {
                txtPeriod13Index.NumberValueDecimal = periodValues[12];
            }

            if (FHas14Periods)
            {
                txtPeriod14Index.NumberValueDecimal = periodValues[13];
            }

            txtInflateBaseTotalAmount.NumberValueDecimal = totalAmount; //.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private string CurrencyCodeToUse()
        {
            string retVal = string.Empty;

            AAccountRow currentAccountRow = null;

            if ((FAccountTable != null) && (cmbDetailAccountCode.SelectedIndex != -1) && (cmbDetailAccountCode.Count > 0)
                && (cmbDetailAccountCode.GetSelectedString() != null))
            {
                currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, cmbDetailAccountCode.GetSelectedString() });

                if ((currentAccountRow != null) && currentAccountRow.ForeignCurrencyFlag)
                {
                    grpBudgetDetails.Text = "Budget Details (Foreign Account)";
                    retVal = currentAccountRow.ForeignCurrencyCode;
                }
            }

            if (retVal == string.Empty)
            {
                grpBudgetDetails.Text = "Budget Details";
                retVal = FCurrencyCode;
            }

            return retVal;
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
            string currencyCode = CurrencyCodeToUse();

            // Adhoc
            txtPeriod01Amount.CurrencyCode = currencyCode;
            txtPeriod02Amount.CurrencyCode = currencyCode;
            txtPeriod03Amount.CurrencyCode = currencyCode;
            txtPeriod04Amount.CurrencyCode = currencyCode;
            txtPeriod05Amount.CurrencyCode = currencyCode;
            txtPeriod06Amount.CurrencyCode = currencyCode;
            txtPeriod07Amount.CurrencyCode = currencyCode;
            txtPeriod08Amount.CurrencyCode = currencyCode;
            txtPeriod09Amount.CurrencyCode = currencyCode;
            txtPeriod10Amount.CurrencyCode = currencyCode;
            txtPeriod11Amount.CurrencyCode = currencyCode;
            txtPeriod12Amount.CurrencyCode = currencyCode;
            txtPeriod13Amount.CurrencyCode = currencyCode;
            txtPeriod14Amount.CurrencyCode = currencyCode;
            txtTotalAdhocAmount.CurrencyCode = currencyCode;

            // Same
            txtAmount.CurrencyCode = currencyCode;
            txtSameTotalAmount.CurrencyCode = currencyCode;

            // Split
            txtPerPeriodAmount.CurrencyCode = currencyCode;
            txtLastPeriodAmount.CurrencyCode = currencyCode;
            txtTotalSplitAmount.CurrencyCode = currencyCode;

            // Inflate N
            txtFirstPeriodAmount.CurrencyCode = currencyCode;
            txtInflateNTotalAmount.CurrencyCode = currencyCode;

            // Inflate Base
            txtPeriod1Amount.CurrencyCode = currencyCode;
            txtInflateBaseTotalAmount.CurrencyCode = currencyCode;
        }

        private void ShowDetailsManual(ABudgetRow ARow)
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

        private bool GetDetailDataFromControlsManual(ABudgetRow ARow)
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

                //TODO switch to using Ledger financial year
                ARow.Year = Convert.ToInt16(cmbSelectBudgetYear.GetSelectedString());
                ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, ARow.Year);
                ARow.EndEdit();
            }

            return true;
        }

        private int CreateBudgetRevisionRow(int ALedgerNumber, int AYear)
        {
            //Always to be zero
            int newRevision = 0;

            if (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, newRevision }) == null)
            {
                ABudgetRevisionRow BudgetRevisionRow = FMainDS.ABudgetRevision.NewRowTyped();

                BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                BudgetRevisionRow.Year = AYear;

                BudgetRevisionRow.Revision = newRevision;
                FMainDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
            }

            return newRevision;
        }

        private void CostCentreCodeDetailChanged(object sender, EventArgs e)
        {
            string currentCostCentre;
            bool costCentreActive = true;

            if ((FLoadCompleted == false) || (FPreviouslySelectedDetailRow == null)
                || (cmbDetailCostCentreCode.GetSelectedString() == String.Empty) || (cmbDetailCostCentreCode.SelectedIndex == -1))
            {
                return;
            }

            currentCostCentre = cmbDetailCostCentreCode.GetSelectedString();
            costCentreActive = CostCentreIsActive();

            //If change from combo action as opposed to moving rows
            if (FPreviouslySelectedDetailRow.BudgetSequence == FBudgetSequence)
            {
                if ((cmbDetailAccountCode.SelectedIndex != -1) && !CostCentreAccountCombinationIsUnique())
                {
                    MessageBox.Show(String.Format(Catalog.GetString(
                                "The Cost Centre ({0})/Account Code ({1}) combination is already used in this budget."),
                            currentCostCentre,
                            cmbDetailAccountCode.GetSelectedString()));
                    cmbDetailCostCentreCode.SelectedIndex = -1;
                }
                else if (!costCentreActive)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString("Cost Centre Code {0} is set to Inactive. Do you want to select it?"),
                                currentCostCentre),
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
            string currentAccount;
            bool accountActive = true;

            if ((FLoadCompleted == false) || (FPreviouslySelectedDetailRow == null) || (cmbDetailAccountCode.GetSelectedString() == String.Empty)
                || (cmbDetailAccountCode.SelectedIndex == -1))
            {
                return;
            }

            currentAccount = cmbDetailAccountCode.GetSelectedString();
            accountActive = AccountIsActive(currentAccount);

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
                else if (!accountActive)
                {
                    if (MessageBox.Show(String.Format(Catalog.GetString("Account Code {0} is set to Inactive. Do you want to select it?"),
                                currentAccount),
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
            DataRow[] foundRows;
            foundRows = FMainDS.ABudget.Select(String.Format("{0}={1} And {2}={3} And {4}=0 And {5}='{6}' And {7}='{8}' And {9}<>{10}",
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

            return foundRows.Length == 0;
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

            currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });

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

            currentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });

            if (currentCostCentreRow != null)
            {
                retVal = currentCostCentreRow.CostCentreActiveFlag;
            }

            return retVal;
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
    }
}