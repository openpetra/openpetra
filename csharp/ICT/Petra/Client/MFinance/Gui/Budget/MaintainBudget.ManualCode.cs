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

        private Int32 CurrentBudgetYear;

        private bool LoadCompleted = false;
        private bool RejectYearChange = false;

        private TDlgSelectCSVSeparator FdlgSeparator;
        private String FCurrencyCode = "";

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                FMainDS = TRemote.MFinance.Budget.WebConnectors.LoadBudget(FLedgerNumber);

                // to get an empty ABudgetFee table, instead of null reference
                FMainDS.Merge(new BudgetTDS());

                TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbSelectBudgetYear, FLedgerNumber, true);

                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);

                if (!int.TryParse(cmbSelectBudgetYear.GetSelectedString(), out CurrentBudgetYear))
                {
                    CurrentBudgetYear = TFinanceControls.GetLedgerCurrentFinancialYear(FLedgerNumber);
                }

                DataView myDataView = FMainDS.ABudget.DefaultView;
                myDataView.AllowNew = false;
                myDataView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdDetails.AutoSizeCells();

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";

                ClearBudgetTextboxCurrencyFormat();
                FCurrencyCode = FMainDS.ALedger[0].BaseCurrency;

//                EnableBudgetEntry(false);
//
//                if (grdDetails.Rows.Count > 0)
//                {
//                    EnableBudgetEntry(true);
//                }

                LoadCompleted = true;
            }
        }

        private void NewRowManual(ref ABudgetRow ARow)
        {
            if (!cmbDetailAccountCode.Enabled)
            {
                EnableBudgetEntry(true);
            }

            ARow.BudgetSequence = Convert.ToInt32(TRemote.MCommon.WebConnectors.GetNextSequence(TSequenceNames.seq_budget));;
            ARow.LedgerNumber = FLedgerNumber;
            ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, CurrentBudgetYear);
            ARow.Year = CurrentBudgetYear;

            //Add the budget period values
            for (int i = 1; i <= 12; i++)
            {
                ABudgetPeriodRow BudgetPeriodRow = FMainDS.ABudgetPeriod.NewRowTyped();
                BudgetPeriodRow.BudgetSequence = ARow.BudgetSequence;
                BudgetPeriodRow.PeriodNumber = i;
                BudgetPeriodRow.BudgetBase = 0;
                FMainDS.ABudgetPeriod.Rows.Add(BudgetPeriodRow);
                BudgetPeriodRow = null;
            }
        }

        private void EnableBudgetEntry(bool AAllowEntry)
        {
            rgrSelectBudgetType.Enabled = AAllowEntry;
            cmbDetailCostCentreCode.Enabled = AAllowEntry;
            cmbDetailAccountCode.Enabled = AAllowEntry;

            if (!AAllowEntry)
            {
                pnlBudgetTypeAdhoc.Visible = false;
                pnlBudgetTypeSame.Visible = false;
                pnlBudgetTypeSplit.Visible = false;
                pnlBudgetTypeInflateN.Visible = false;
                pnlBudgetTypeInflateBase.Visible = false;
            }
            else
            {
                pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
                pnlBudgetTypeSame.Visible = rbtSame.Checked;
                pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
                pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
                pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;
            }
        }

        private void SelectBudgetYear(Object sender, EventArgs e)
        {
            if (LoadCompleted)
            {
                //MessageBox.Show(RejectYearChange.ToString());
                if (RejectYearChange)
                {
                    return;
                }

                //MessageBox.Show(FMainDS.ABudget.Rows.Count.ToString());
                if (FPetraUtilsObject.HasChanges)
                {
                    RejectYearChange = true;
                    MessageBox.Show("Please save changes before attempting to change year.");
                    cmbSelectBudgetYear.SetSelectedInt32(CurrentBudgetYear);
                    return;
                }

                if (int.TryParse(cmbSelectBudgetYear.GetSelectedString(), out CurrentBudgetYear))
                {
                    //MessageBox.Show(cmbSelectBudgetYear.GetSelectedString() + " - " + CurrentBudgetYear.ToString());
                    DataView myDataView = FMainDS.ABudget.DefaultView;
                    myDataView.AllowNew = false;
                    myDataView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);
                    grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

                    if (grdDetails.Rows.Count > 0)
                    {
                        SelectByIndex(0);
                    }

//                      FMainDS.ABudget.DefaultView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);
//                    //grdDetails.Refresh();
//                    SelectByIndex(0);

                    //FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                    if (FMainDS.ABudget.DefaultView.Count == 0)
                    {
                        EnableBudgetEntry(false);
                    }
                    else
                    {
                        EnableBudgetEntry(true);

                        // display the details of the currently selected row
                        if (FPreviouslySelectedDetailRow != null)
                        {
                            ShowDetails(FPreviouslySelectedDetailRow);
                        }

                        pnlDetails.Enabled = true;
                    }
                }
            }
        }

        private TSubmitChangesResult StoreManualCode(ref BudgetTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            //Reset this flag
            RejectYearChange = false;

            TSubmitChangesResult TSCR = TRemote.MFinance.Budget.WebConnectors.SaveBudget(ref ASubmitChanges, out AVerificationResult);

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
                FPreviouslySelectedDetailRow.CostCentreCode,
                FPreviouslySelectedDetailRow.AccountCode,
                FPreviouslySelectedDetailRow.BudgetTypeCode,
                FPreviouslySelectedDetailRow.Revision,
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

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            DeleteBudgetPeriodData(BudgetSequence);
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
            int NumRecsImported = 0;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.FileName = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "import.csv");

            dialog.Title = Catalog.GetString("Import budget(s) from csv file");
            dialog.Filter = Catalog.GetString("Budget files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";American");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                FdlgSeparator.CSVFileName = dialog.FileName;

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

                    //MessageBox.Show(FMainDS.ABudget.Rows.Count.ToString());
                    //MessageBox.Show(importString);
                    //TODO return the budget from the year, and -99 for fail
                    NumRecsImported = TRemote.MFinance.Budget.WebConnectors.ImportBudgets(FLedgerNumber,
                        CurrentBudgetYear,
                        dialog.FileName,
                        FdlgSeparatorVal,
                        ref FMainDS,
                        out AMessages);
                    //ShowMessages(AMessages);
                    //MessageBox.Show(FMainDS.ABudget.Rows.Count.ToString());
                }

                if (NumRecsImported > 0)
                {
                    MessageBox.Show(String.Format(Catalog.GetString("{0} budget records imported successfully!"), NumRecsImported),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //DataView myView = FMainDS.ABudget.DefaultView;
                    //myView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);

                    //FMainDS.ABudget.DefaultView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);
                    ////grdDetails.Refresh();
                    //MessageBox.Show(String.Format("Current Year is {0}", CurrentBudgetYear));
                    DataView myDataView = FMainDS.ABudget.DefaultView;
                    myDataView.AllowNew = false;
                    myDataView.RowFilter = String.Format("{0} = {1}", ABudgetTable.GetYearDBName(), CurrentBudgetYear);
                    grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

                    if (grdDetails.Rows.Count > 0)
                    {
                        SelectByIndex(0);
                    }

                    //SelectDetailRowByDataTableIndex(FMainDS.ABudget.Rows.Count - 1);
                    //SaveUserDefaults(dialog, impOptions);
                    //FLoadedData = TFinanceBatchFilterEnum.fbfNone;
                    //LoadBatches(FLedgerNumber);
                    FPetraUtilsObject.SetChangedFlag();
                }
                else if (NumRecsImported == -1)
                {
                    MessageBox.Show("The year contained in the import file is different to the current selected year.");

                    ////grdDetails.Refresh();
                    if (grdDetails.Rows.Count > 0)
                    {
                        SelectByIndex(0);
                    }
                }
                else
                {
                    MessageBox.Show("No records found to import");
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
            MessageBox.Show("Not yet implemented.");
            //exportForm = new TFrmGiftBatchExport(FPetraUtilsObject.GetForm());
            //exportForm.LedgerNumber = FLedgerNumber;
            //exportForm.Show();
        }

        private void DeleteBudgetPeriodData(int ABudgetSequence)
        {
            ABudgetPeriodRow BudgetPeriodRow = null;

            for (int i = 1; i <= 12; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.Delete();
                }

                BudgetPeriodRow = null;
            }
        }

        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void NewBudgetType(System.Object sender, EventArgs e)
        {
            //ClearBudgetPeriodTextboxes();

            pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
            pnlBudgetTypeSame.Visible = rbtSame.Checked;
            pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
            pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
            pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;

            if (LoadCompleted && !FPetraUtilsObject.HasChanges)
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

            //grdDetails.Refresh();
        }

        private void ProcessBudgetTypeAdhoc(System.Object sender, EventArgs e)
        {
            decimal TotalAmount = 0;

            decimal[] PeriodAmounts = new decimal[12];
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

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= 12; i++)
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
                    MessageBox.Show("Error trying to write BudgetPeriod values");
                }

                BudgetPeriodRow = null;
            }

            txtTotalAdhocAmount.NumberValueDecimal = TotalAmount;
            //grdDetails.Refresh();
        }

        private void ProcessBudgetTypeSame(System.Object sender, EventArgs e)
        {
            decimal PeriodAmount = Convert.ToDecimal(txtAmount.NumberValueDecimal);
            decimal AnnualAmount = PeriodAmount * 12;

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= 12; i++)
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
                    MessageBox.Show("Error trying to write BudgetPeriod values");
                }

                BudgetPeriodRow = null;
            }

            txtSameTotalAmount.NumberValueDecimal = AnnualAmount;
            //grdDetails.Refresh();
        }

        private void ProcessBudgetTypeSplit(System.Object sender, EventArgs e)
        {
            decimal AnnualAmount = 0;
            decimal PerPeriodAmount = 0;
            decimal Period12Amount = 0;

            AnnualAmount = Convert.ToDecimal(txtTotalSplitAmount.NumberValueDecimal);
            PerPeriodAmount = Math.Truncate(AnnualAmount / 12);
            Period12Amount = AnnualAmount - PerPeriodAmount * 11;

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= 12; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { BudgetSequence, i });

                if (BudgetPeriodRow != null)
                {
                    BudgetPeriodRow.BeginEdit();

                    if (i < 12)
                    {
                        BudgetPeriodRow.BudgetBase = PerPeriodAmount;
                    }
                    else
                    {
                        BudgetPeriodRow.BudgetBase = Period12Amount;
                    }

                    BudgetPeriodRow.EndEdit();
                }
                else
                {
                    //TODO: add error handling
                    MessageBox.Show("Error trying to write BudgetPeriod values");
                }

                BudgetPeriodRow = null;
            }

            txtPerPeriodAmount.NumberValueDecimal = PerPeriodAmount;
            txtPeriod12AmountPlus.NumberValueDecimal = Period12Amount;
            //grdDetails.Refresh();
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

            TotalAmount = FirstPeriodAmount * InflateAfterPeriod + SubsequentPeriodsAmount * (12 - InflateAfterPeriod);

            //Write to Budget rows
            for (int i = 1; i <= 12; i++)
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
                    MessageBox.Show("Error trying to write BudgetPeriod values");
                }

                BudgetPeriodRow = null;
            }

            lblInflateNTotalAmount.Text = "    Total: " + StringHelper.FormatUsingCurrencyCode(TotalAmount, FCurrencyCode);
            grdDetails.Refresh();
        }

        private void ProcessBudgetTypeInflateBase(System.Object sender, EventArgs e)
        {
            decimal TotalAmount = 0;

            decimal[] PeriodAmounts = new decimal[12];
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

            int BudgetSequence = FPreviouslySelectedDetailRow.BudgetSequence;
            ABudgetPeriodRow BudgetPeriodRow = null;

            //Write to Budget rows
            for (int i = 1; i <= 12; i++)
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
                    MessageBox.Show("Error trying to write BudgetPeriod values");
                }

                BudgetPeriodRow = null;
            }

            lblInflateBaseTotalAmount.Text = "    Total: " + StringHelper.FormatUsingCurrencyCode(TotalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeAdhoc()
        {
            decimal totalAmount = 0;
            decimal CurrentPeriodAmount = 0;
            string textboxName;

            ABudgetPeriodRow BudgetPeriodRow;
            TTxtCurrencyTextBox txt = null;

            for (int i = 1; i <= 12; i++)
            {
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i });

                textboxName = "txtPeriod" + i.ToString("00") + "Amount";

                foreach (Control ctrl in pnlBudgetTypeAdhoc.Controls)
                {
                    if (ctrl is TTxtCurrencyTextBox && (ctrl.Name == textboxName))
                    {
                        txt = (TTxtCurrencyTextBox)ctrl;
                        break;
                    }
                }

                if (BudgetPeriodRow != null)
                {
                    CurrentPeriodAmount = BudgetPeriodRow.BudgetBase;
                    txt.NumberValueDecimal = CurrentPeriodAmount;
                    totalAmount += CurrentPeriodAmount;
                }
                else
                {
                    txt.NumberValueDecimal = 0;
                }

                BudgetPeriodRow = null;
                txt = null;
            }

            txtTotalAdhocAmount.Text = StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSame()
        {
            decimal totalAmount = 0;
            decimal FirstPeriodAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow;

            //Get the first period amount
            BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (BudgetPeriodRow != null)
            {
                FirstPeriodAmount = BudgetPeriodRow.BudgetBase;
                totalAmount = FirstPeriodAmount * 12;
            }

            txtAmount.NumberValueDecimal = FirstPeriodAmount;
            lblSameTotalAmount.Text = "    Total: " + StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeSplit()
        {
            decimal PerPeriodAmount = 0;
            decimal EndPeriodAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow;

            //Find periods 1-11 amount
            BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 1 });

            if (BudgetPeriodRow != null)
            {
                PerPeriodAmount = BudgetPeriodRow.BudgetBase;
                BudgetPeriodRow = null;

                //Find period 12 amount
                BudgetPeriodRow = (ABudgetPeriodRow)FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, 12 });

                if (BudgetPeriodRow != null)
                {
                    EndPeriodAmount = BudgetPeriodRow.BudgetBase;
                }
            }

            //Calculate the total amount
            txtPerPeriodAmount.NumberValueDecimal = PerPeriodAmount;
            txtPeriod12AmountPlus.NumberValueDecimal = EndPeriodAmount;
            txtTotalSplitAmount.NumberValueDecimal = PerPeriodAmount * 11 + EndPeriodAmount;
        }

        private void DisplayBudgetTypeInflateN()
        {
            decimal FirstPeriodAmount = 0;
            int InflateAfterPeriod = 0;
            decimal InflationRate = 0;
            decimal CurrentPeriodAmount;
            decimal TotalAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow;

            for (int i = 1; i <= 12; i++)
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
                            TotalAmount = FirstPeriodAmount * InflateAfterPeriod + CurrentPeriodAmount * (12 - InflateAfterPeriod);
                            break;
                        }
                        else if (i == 12) // and by implication CurrentPeriodAmount == FirstPeriodAmount
                        {
                            //This is an odd case that the user should never implement, but still needs to be covered.
                            //  It is equivalent to using BUDGET TYPE: SAME
                            InflateAfterPeriod = 0;
                            InflationRate = 0;
                            TotalAmount = CurrentPeriodAmount * 12;
                        }
                    }
                }

                BudgetPeriodRow = null;
            }

            txtFirstPeriodAmount.NumberValueDecimal = FirstPeriodAmount;
            txtInflateAfterPeriod.NumberValueInt = InflateAfterPeriod;
            txtInflationRate.NumberValueDecimal = InflationRate;
            lblInflateNTotalAmount.Text = "    Total: " + StringHelper.FormatUsingCurrencyCode(TotalAmount, FCurrencyCode);
        }

        private void DisplayBudgetTypeInflateBase()
        {
            decimal totalAmount = 0;

            decimal[] PeriodValues = new decimal[12];
            decimal PriorPeriodAmount = 0;
            decimal CurrentPeriodAmount = 0;

            ABudgetPeriodRow BudgetPeriodRow;

            for (int i = 1; i <= 12; i++)
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
                        PeriodValues[i - 1] = (CurrentPeriodAmount - PriorPeriodAmount) / PriorPeriodAmount * 100;
                    }

                    PriorPeriodAmount = CurrentPeriodAmount;
                    totalAmount += CurrentPeriodAmount;
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

            lblInflateBaseTotalAmount.Text = "    Total: " + StringHelper.FormatUsingCurrencyCode(totalAmount, FCurrencyCode);
        }

        private void ClearBudgetPeriodTextboxes(string AExcludeType)
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
                txtPeriod12AmountPlus.NumberValueDecimal = 0;
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
                txtInflateBaseTotalAmount.NumberValueDecimal = 0;
            }
        }

        private void ClearBudgetTextboxCurrencyFormat()
        {
            //Adhoc controls
            txtPeriod01Amount.CurrencySymbol = String.Empty;
            txtPeriod02Amount.CurrencySymbol = String.Empty;
            txtPeriod03Amount.CurrencySymbol = String.Empty;
            txtPeriod04Amount.CurrencySymbol = String.Empty;
            txtPeriod05Amount.CurrencySymbol = String.Empty;
            txtPeriod06Amount.CurrencySymbol = String.Empty;
            txtPeriod07Amount.CurrencySymbol = String.Empty;
            txtPeriod08Amount.CurrencySymbol = String.Empty;
            txtPeriod09Amount.CurrencySymbol = String.Empty;
            txtPeriod10Amount.CurrencySymbol = String.Empty;
            txtPeriod11Amount.CurrencySymbol = String.Empty;
            txtPeriod12Amount.CurrencySymbol = String.Empty;
            //Same controls
            txtAmount.CurrencySymbol = String.Empty;
            //Split controls
            txtTotalSplitAmount.CurrencySymbol = String.Empty;
            txtPerPeriodAmount.CurrencySymbol = String.Empty;
            txtPeriod12AmountPlus.CurrencySymbol = String.Empty;
            //Inflate N controls
            txtFirstPeriodAmount.CurrencySymbol = String.Empty;
            //Inflate Base controls
            txtPeriod1Amount.CurrencySymbol = String.Empty;
        }

        private void ShowDetailsManual(ABudgetRow ARow)
        {
            ClearBudgetPeriodTextboxes("All");

            if ((grdDetails.Rows.Count == 0) && rgrSelectBudgetType.Enabled)
            {
                EnableBudgetEntry(false);
                return;
            }
            else if (rgrSelectBudgetType.Enabled == false)
            {
                EnableBudgetEntry(true);
            }

            //
            // ARow can be null...
            if (ARow != null)
            {
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
            }

            pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
            pnlBudgetTypeSame.Visible = rbtSame.Checked;
            pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
            pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
            pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;

            LoadCompleted = true;
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
                        ClearBudgetPeriodTextboxes(MFinanceConstants.BUDGET_ADHOC);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_ADHOC;
                }
                else if (rbtSame.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeSame(null, null);
                        ClearBudgetPeriodTextboxes(MFinanceConstants.BUDGET_SAME);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SAME;
                }
                else if (rbtSplit.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeSplit(null, null);
                        ClearBudgetPeriodTextboxes(MFinanceConstants.BUDGET_SPLIT);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SPLIT;
                }
                else if (rbtInflateN.Checked)
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeInflateN(null, null);
                        ClearBudgetPeriodTextboxes(MFinanceConstants.BUDGET_INFLATE_N);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_N;
                }
                else      //rbtInflateBase.Checked
                {
                    if (FPetraUtilsObject.HasChanges)
                    {
                        ProcessBudgetTypeInflateBase(null, null);
                        ClearBudgetPeriodTextboxes(MFinanceConstants.BUDGET_INFLATE_BASE);
                    }

                    ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_BASE;
                }

                //TODO switch to using Ledger financial year
                ARow.Year = Convert.ToInt16(cmbSelectBudgetYear.GetSelectedString());
                ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, ARow.Year);
                ARow.EndEdit();
            }

            //grdDetails.Refresh();
            return true;
        }

        private int CreateBudgetRevisionRow(int ALedgerNumber, int AYear)
        {
            int newRevision = 0;

            if (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, newRevision }) == null)
            {
                ABudgetRevisionRow BudgetRevisionRow = FMainDS.ABudgetRevision.NewRowTyped();

                BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                BudgetRevisionRow.Year = AYear;

                BudgetRevisionRow.Revision = newRevision;
                FMainDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);

                //TODO check with Rob about budget versioning
                //              while (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, newRevision }) != null)
                //                {
                //                    newRevision++;
                //                }
            }

            return newRevision;
        }
    }
}