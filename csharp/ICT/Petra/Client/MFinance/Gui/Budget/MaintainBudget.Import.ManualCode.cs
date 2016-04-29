//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, peters
//
// Copyright 2004-2014 by OM International
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
using System.IO;
using System.Text;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    /// <summary>
    /// A business logic class that handles importing of budgets
    /// </summary>
    public class MaintainBudget_Import
    {
        private TDlgSelectCSVSeparator FdlgSeparator;
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private Int32 FNumberOfPeriods;
        private IMaintainBudget FUserControl = null;
        private Form FParentForm = null;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MaintainBudget_Import(TFrmPetraEditUtils APetraUtilsObject,
            Int32 ALedgerNumber,
            Int32 ANumberOfPeriods,
            TFrmMaintainBudget AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FNumberOfPeriods = ANumberOfPeriods;
            FUserControl = AUserControl;
            FParentForm = AUserControl;
        }

        #endregion

        /// <summary>
        /// Imports budgets from a file
        /// </summary>
        /// <param name="ASelectedBudgetYear"></param>
        /// <param name="AMainDS"></param>
        public void ImportBudget(int ASelectedBudgetYear, BudgetTDS AMainDS)
        {
            TVerificationResultCollection Messages = new TVerificationResultCollection();

            int NumBudgetsImported = 0;
            int NumRecsUpdated = 0;
            int NumRowsFailed = 0;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("Please save before trying to import!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            String DateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog OFDialog = new OpenFileDialog();

            string ExportPath = TClientSettings.GetExportPath();
            string FullPath = TUserDefaults.GetStringDefault("Imp Filename",
                ExportPath + Path.DirectorySeparatorChar + "import.csv");
            TImportExportDialogs.SetOpenFileDialogFilePathAndName(OFDialog, FullPath, ExportPath);

            OFDialog.Title = Catalog.GetString("Import budget(s) from csv file");
            OFDialog.Filter = Catalog.GetString("Budget files (*.csv)|*.csv");
            String ImportOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

            // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
            TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(FullPath));

            if (OFDialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);

                try
                {
                    FParentForm.UseWaitCursor = true;

                    Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(OFDialog.FileName);

                    if (!fileCanOpen)
                    {
                        throw new Exception(String.Format(Catalog.GetString("File {0} cannot be opened."), OFDialog.FileName));
                    }

                    FdlgSeparator.DateFormat = DateFormatString;

                    if (ImportOptions.Length > 1)
                    {
                        FdlgSeparator.NumberFormat = ImportOptions.Substring(1);
                    }

                    FdlgSeparator.SelectedSeparator = ImportOptions.Substring(0, 1);

                    if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                    {
                        string[] FdlgSeparatorVal = new string[] {
                            FdlgSeparator.SelectedSeparator, FdlgSeparator.DateFormat, FdlgSeparator.NumberFormat
                        };

                        // read contents of file
                        string ImportString = File.ReadAllText(OFDialog.FileName);

                        //TODO return the budget from the year, and -99 for fail
                        NumBudgetsImported = TRemote.MFinance.Budget.WebConnectors.ImportBudgets(FLedgerNumber,
                            ASelectedBudgetYear,
                            ImportString,
                            OFDialog.FileName,
                            FdlgSeparatorVal,
                            AMainDS,
                            out NumRecsUpdated,
                            out NumRowsFailed,
                            out Messages);

                        ShowMessages(Messages, NumBudgetsImported, NumRecsUpdated, NumRowsFailed);
                    }
                }
                catch (Exception ex)
                {
                    NumBudgetsImported = -1;
                    MessageBox.Show(ex.Message, Catalog.GetString("Budget Import"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // We save the defaults even if ok is false - because the client will probably want to try and import
                    //   the same file again after correcting any errors
                    SaveUserDefaults(OFDialog, ImportOptions);

                    FParentForm.UseWaitCursor = false;
                }

                // update grid
                if ((NumBudgetsImported - NumRowsFailed) > 0)
                {
                    try
                    {
                        FParentForm.UseWaitCursor = true;

                        UpdateABudgetPeriodAmounts(AMainDS, ASelectedBudgetYear);
                        FUserControl.SetBudgetDefaultView();
                    }
                    finally
                    {
                        FParentForm.UseWaitCursor = false;
                    }

                    FPetraUtilsObject.SetChangedFlag();
                }

                FUserControl.SelectRowInGrid(1);
            }
        }

        private void SaveUserDefaults(OpenFileDialog dialog, String impOptions)
        {
            if (dialog != null)
            {
                TUserDefaults.SetDefault("Imp Filename", dialog.FileName);
            }

            impOptions = FdlgSeparator.SelectedSeparator;
            impOptions += FdlgSeparator.NumberFormat;
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", FdlgSeparator.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }

        private void ShowMessages(TVerificationResultCollection AMessages,
            int ANumBudgetsToImport, int ANumRecsUpdated, int ANumRowsFailed)
        {
            StringBuilder ErrorMessages = new StringBuilder();

            if (ANumBudgetsToImport != 0)
            {
                // if there were budgets to import
                if (ANumBudgetsToImport > 0)
                {
                    ErrorMessages.AppendFormat
                        (Catalog.GetPluralString("{0} budget row was found in the file ", "{0} budget rows were found in the file ",
                            ANumBudgetsToImport, true),
                        ANumBudgetsToImport);

                    ErrorMessages.AppendFormat
                        (Catalog.GetPluralString("and {0} was successfully imported!", "and {0} were successfully imported!",
                            ANumBudgetsToImport - ANumRowsFailed, true),
                        (ANumBudgetsToImport - ANumRowsFailed));

                    if (ANumRecsUpdated > 0)
                    {
                        ErrorMessages.AppendFormat(Catalog.GetPluralString("{0}({1} of which updated an existing budget row.)",
                                "{0}({1} of which updated existing budget rows.)", ANumRecsUpdated),
                            Environment.NewLine,
                            ANumRecsUpdated);
                    }

                    ErrorMessages.Append(Environment.NewLine + Environment.NewLine);
                }

                //Check for import errors
                if (AMessages.Count > 0)
                {
                    ErrorMessages.AppendFormat(
                        Catalog.GetPluralString("{0} row failed to import:{1}", "{0} rows failed to import:{1}",
                            ANumRowsFailed, true),
                        ANumRowsFailed, Environment.NewLine);

                    foreach (TVerificationResult message in AMessages)
                    {
                        ErrorMessages.AppendFormat("{0}[{1}] {2}: {3}",
                            Environment.NewLine,
                            message.ResultContext, message.ResultTextCaption, message.ResultText);
                    }
                }

                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FParentForm);

                if ((ANumRowsFailed > 0) || (ANumBudgetsToImport == -1))
                {
                    extendedMessageBox.ShowDialog(ErrorMessages.ToString(),
                        Catalog.GetString("Budget Import"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiError);
                }
                else
                {
                    extendedMessageBox.ShowDialog(ErrorMessages.ToString(),
                        Catalog.GetString("Budget Import"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiInformation);
                }
            }
            else //0
            {
                MessageBox.Show(Catalog.GetString("No records found to import"), Catalog.GetString(
                        "Budget Import"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Update Budget Period Amounts for each record that was created or modified during an import
        /// </summary>
        private void UpdateABudgetPeriodAmounts(BudgetTDS AMainDS, int ASelectedBudgetYear)
        {
            foreach (BudgetTDSABudgetRow budgetRow in AMainDS.ABudget.Rows)
            {
                if (budgetRow.RowState == DataRowState.Unchanged)
                {
                    continue;
                }

                int budgetSeq = budgetRow.BudgetSequence;

                DataView budgetPeriodsDV = new DataView(AMainDS.ABudgetPeriod);

                budgetPeriodsDV.RowFilter = String.Format("{0}={1}",
                    ABudgetPeriodTable.GetBudgetSequenceDBName(),
                    budgetRow.BudgetSequence);
                budgetPeriodsDV.Sort = String.Format("{0} ASC",
                    ABudgetPeriodTable.GetPeriodNumberDBName());

                foreach (DataRowView drv2 in budgetPeriodsDV)
                {
                    ABudgetPeriodRow budgetPeriodRow = (ABudgetPeriodRow)drv2.Row;

                    int period = budgetPeriodRow.PeriodNumber;
                    string periodAmountColumn = string.Empty;

                    if (period <= FNumberOfPeriods)
                    {
                        periodAmountColumn = "Period" + period.ToString("00") + "Amount";
                        budgetRow[periodAmountColumn] = budgetPeriodRow.BudgetBase;
                    }
                    else
                    {
                        //TODO After data migration, this should not happen so add an error message.
                        // In old Petra, budget periods always go up to 20, but are only populated
                        //   up to number of financial periods
                    }
                }
            }

            //Attempts using LINQ
            //DataTable BudgetPeriodAmounts = new DataTable();
            //BudgetPeriodAmounts.Columns.Add("BudgetSequence", typeof(int));
            //BudgetPeriodAmounts.Columns.Add("PeriodNumber", typeof(int));
            //BudgetPeriodAmounts.Columns.Add("Amount", typeof(decimal));
            //BudgetPeriodAmounts.PrimaryKey = new DataColumn[] {BudgetPeriodAmounts.Columns["BudgetSequence"],
            //                             BudgetPeriodAmounts.Columns["PeriodNumber"]};

            //var varBudgetPeriodAmounts =
            //    from BudgetTDSABudgetRow budgetRow in FMainDS.ABudget.Rows
            //                 where budgetRow.Year == ASpecificYear
            //                 join ABudgetPeriodRow budgetPeriodRow in FMainDS.ABudgetPeriod.Rows on budgetRow.BudgetSequence equals budgetPeriodRow.BudgetSequence
            //                 select new
            //                 {
            //                     BudgetSequence = budgetRow.BudgetSequence,
            //                     PeriodNumber = budgetPeriodRow.PeriodNumber,
            //                     Amount = budgetPeriodRow.BudgetBase
            //                 }; //produces flat sequence

            //foreach (var rowObj in varBudgetPeriodAmounts)
            //{
            //    DataRow row = BudgetPeriodAmounts.NewRow();
            //    BudgetPeriodAmounts.Rows.Add(rowObj.BudgetSequence, rowObj.PeriodNumber, rowObj.Amount);
            //}

            //DataView BudgetsPeriodAmountsForYearDV = new DataView(BudgetPeriodAmounts);
            //BudgetsPeriodAmountsForYearDV.Sort = "BudgetSequence ASC, PeriodNumber ASC";


            //for (int i = 0; i < BudgetsForYear.Count; i++)
            //{
            //    BudgetTDSABudgetRow budgetRow = (BudgetTDSABudgetRow)BudgetsForYear[i].Row;

            //    for (int j = 1; j <= FNumberOfPeriods; j++)
            //    {
            //        DataRow budgetsPeriodAmounts = BudgetsPeriodAmountsForYearDV[(FNumberOfPeriods * i) + j - 1].Row;

            //        string columnName = "Period" + j.ToString("00") + "Amount";

            //        if (budgetRow.BudgetSequence == (int)budgetsPeriodAmounts["BudgetSequence"])
            //        {
            //            budgetRow[columnName] = (decimal)budgetsPeriodAmounts["Amount"];
            //        }
            //    }
            //}
        }
    }
}