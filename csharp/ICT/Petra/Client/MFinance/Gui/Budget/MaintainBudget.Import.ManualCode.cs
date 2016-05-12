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
using Ict.Petra.Client.CommonDialogs;
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
        /// <param name="ACurrentBudgetYear"></param>
        /// <param name="AMainDS"></param>
        public void ImportBudget(int ACurrentBudgetYear, ref BudgetTDS AMainDS)
        {
            TVerificationResultCollection Messages = new TVerificationResultCollection();

            int BudgetsImported = 0;
            int BudgetsAdded = 0;
            int BudgetsUpdated = 0;
            int BudgetsFailed = 0;

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
                TFrmStatusDialog dlgStatus = new TFrmStatusDialog(FPetraUtilsObject.GetForm());

                FdlgSeparator = new TDlgSelectCSVSeparator(false);

                try
                {
                    string fileTitle = OFDialog.SafeFileName;
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

                        Application.UseWaitCursor = true;

                        //New set of budgets to be loaded
                        dlgStatus.Show();
                        dlgStatus.Heading = Catalog.GetString("Budget Import");
                        dlgStatus.CurrentStatus = Catalog.GetString("Importing budgets from '" + fileTitle + "' ...");

                        // read contents of file
                        string ImportString = File.ReadAllText(OFDialog.FileName);

                        //TODO return the budget from the year, and -99 for fail
                        BudgetsImported = TRemote.MFinance.Budget.WebConnectors.ImportBudgets(FLedgerNumber,
                            ImportString,
                            OFDialog.FileName,
                            FdlgSeparatorVal,
                            ref AMainDS,
                            out BudgetsAdded,
                            out BudgetsUpdated,
                            out BudgetsFailed,
                            out Messages);

                        dlgStatus.Visible = false;

                        Application.UseWaitCursor = false;
                        ShowMessages(Messages, BudgetsImported, BudgetsAdded, BudgetsUpdated, BudgetsFailed);
                    }

                    // We save the defaults even if ok is false - because the client will probably want to try and import
                    //   the same file again after correcting any errors
                    SaveUserDefaults(OFDialog, ImportOptions);
                }
                catch (Exception ex)
                {
                    dlgStatus.Close();
                    TLogging.LogException(ex, Utilities.GetMethodSignature());
                    throw;
                }
                finally
                {
                    Application.UseWaitCursor = false;
                }

                // update grid
                if ((BudgetsAdded + BudgetsUpdated) > 0)
                {
                    try
                    {
                        dlgStatus.CurrentStatus = Catalog.GetString("Updating budget period data...");
                        dlgStatus.Visible = true;

                        Application.UseWaitCursor = true;
                        UpdateABudgetPeriodAmounts(AMainDS, ACurrentBudgetYear);
                        FUserControl.SetBudgetDefaultView(AMainDS);
                    }
                    finally
                    {
                        Application.UseWaitCursor = false;
                        dlgStatus.Close();
                    }

                    FPetraUtilsObject.SetChangedFlag();
                }
                else
                {
                    dlgStatus.Close();
                }
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
            int ABudgetsImported,
            int ABudgetsAdded,
            int ABudgetsUpdated,
            int ABudgetsFailed)
        {
            StringBuilder ErrorMessages = new StringBuilder();

            int BudgetsSuccessful = ABudgetsImported - ABudgetsFailed;
            int BudgetsDuplicate = BudgetsSuccessful - (ABudgetsAdded + ABudgetsUpdated);

            if (ABudgetsImported != 0)
            {
                // if there were budgets to import
                if (ABudgetsImported > 0)
                {
                    ErrorMessages.AppendFormat
                        (Catalog.GetPluralString("{0} budget row was found in the file ", "{0} budget rows were found in the file ",
                            ABudgetsImported, true),
                        ABudgetsImported);

                    ErrorMessages.AppendFormat
                        (Catalog.GetPluralString("and {0} was successfully imported!", "and {0} were successfully imported!",
                            BudgetsSuccessful, true),
                        (BudgetsSuccessful));

                    if (ABudgetsUpdated > 0)
                    {
                        ErrorMessages.AppendFormat(Catalog.GetPluralString("{0} - {1} existing budget row was updated",
                                "{0} - {1} existing budget rows were updated", ABudgetsUpdated, true),
                            Environment.NewLine,
                            ABudgetsUpdated);
                    }

                    if (ABudgetsAdded > 0)
                    {
                        ErrorMessages.AppendFormat(Catalog.GetPluralString("{0} - {1} new budget row was added",
                                "{0} - {1} new budget rows were added", ABudgetsAdded, true),
                            Environment.NewLine,
                            ABudgetsAdded);
                    }

                    if (BudgetsDuplicate > 0)
                    {
                        ErrorMessages.AppendFormat(Catalog.GetPluralString("{0} - {1} identical budget row was not needed",
                                "{0} - {1} identical budget rows were not needed", BudgetsDuplicate, true),
                            Environment.NewLine,
                            BudgetsDuplicate);
                    }

                    ErrorMessages.Append(Environment.NewLine + Environment.NewLine);
                }

                //Check for import errors
                if (AMessages.Count > 0)
                {
                    ErrorMessages.AppendFormat(
                        Catalog.GetPluralString("{0} budget row failed on import:", "{0} budget rows failed on import:",
                            ABudgetsFailed, true),
                        ABudgetsFailed);

                    foreach (TVerificationResult message in AMessages)
                    {
                        ErrorMessages.AppendFormat("{0}[{1}] {2}: {3}",
                            Environment.NewLine,
                            message.ResultContext, message.ResultTextCaption, message.ResultText);
                    }
                }

                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FParentForm);

                if ((ABudgetsFailed > 0) || (ABudgetsImported == -1))
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
        private void UpdateABudgetPeriodAmounts(BudgetTDS AMainDS, int ACurrentBudgetYear)
        {
            DataView BudgetDV = new DataView(AMainDS.ABudget);

            BudgetDV.RowFilter = String.Format("Isnull({0},'') <> '' And ({0}='Added' Or {0}='Updated')",
                ABudgetTable.GetCommentDBName());

            BudgetDV.Sort = String.Format("{0} ASC",
                ABudgetTable.GetYearDBName());

            foreach (DataRowView drv in BudgetDV)
            {
                BudgetTDSABudgetRow budgetRow = (BudgetTDSABudgetRow)drv.Row;

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

            //Remove import related comment
            // - done separately to loop above to avoid confusion as DataView filtering is on Comment field
            foreach (DataRowView drv in BudgetDV)
            {
                BudgetTDSABudgetRow budgetRow = (BudgetTDSABudgetRow)drv.Row;
                budgetRow.Comment = string.Empty;
            }
        }
    }
}