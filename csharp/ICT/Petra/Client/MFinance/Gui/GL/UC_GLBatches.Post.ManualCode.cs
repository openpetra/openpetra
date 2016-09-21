//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// A business logic class that handles posting of batches
    /// </summary>
    public class TUC_GLBatches_Post
    {
        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GLBatchTDS FMainDS = null;
        private GLSetupTDS FCacheDS = null;
        private ACostCentreTable FCostCentreTable = null;
        private AAccountTable FAccountTable = null;
        private IUC_GLBatches FMyUserControl = null;
        private TFrmGLBatch FMyForm = null;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GLBatches_Post(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GLBatchTDS AMainDS, IUC_GLBatches AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FMyUserControl = AUserControl;

            FMyForm = (TFrmGLBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Main Public methods

        /// <summary>
        /// Posts a batch
        /// </summary>
        /// <param name="ACurrentBatchRow">The data row corresponding to the batch to post</param>
        /// <param name="AEffectiveDate">The effective date for the batch</param>
        /// <param name="AStartDateCurrentPeriod">The earliest postable date</param>
        /// <param name="AEndDateLastForwardingPeriod">The latest postable date</param>
        /// <returns>
        /// True if the batch was successfully posted
        /// </returns>
        public bool PostBatch(ABatchRow ACurrentBatchRow,
            DateTime AEffectiveDate,
            DateTime AStartDateCurrentPeriod,
            DateTime AEndDateLastForwardingPeriod)
        {
            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                return false;
            }

            int CurrentBatchNumber = ACurrentBatchRow.BatchNumber;

            //Make sure that all control data is in dataset
            FMyForm.GetLatestControlData();

            if (FPetraUtilsObject.HasChanges)
            {
                //Keep this conditional check separate so that it only gets called when necessary
                // and doesn't result in the executon of the next else if which calls same method
                if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction))
                {
                    return false;
                }
            }
            //This has to be called here as if there are no changes then the DataSavingValidating method
            // which calls the method below, will not run.
            else if (!FMyForm.GetTransactionsControl().AllowInactiveFieldValues(FLedgerNumber,
                         CurrentBatchNumber, FMyForm.FCurrentGLBatchAction))
            {
                return false;
            }

            //Load all Batch data
            FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndRelatedTables(FLedgerNumber, CurrentBatchNumber));

            if (FCacheDS == null)
            {
                FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber, false);
            }

            if (FAccountTable == null)
            {
                SetAccountCostCentreTableVariables();
            }

            if ((AEffectiveDate.Date < AStartDateCurrentPeriod) || (AEffectiveDate.Date > AEndDateLastForwardingPeriod))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The Date Effective is outside the periods available for posting. Enter a date between {0:d} and {1:d}."),
                        AStartDateCurrentPeriod,
                        AEndDateLastForwardingPeriod));

                return false;
            }

            // check that a corportate exchange rate exists
            FMyForm.WarnAboutMissingIntlExchangeRate = true;

            if (FMyForm.GetInternationalCurrencyExchangeRate() == 0)
            {
                return false;
            }

            if ((MessageBox.Show(String.Format(Catalog.GetString("Are you sure you want to post GL batch {0}?"),
                         CurrentBatchNumber),
                     Catalog.GetString("Question"),
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != System.Windows.Forms.DialogResult.Yes))
            {
                return true;
            }

            TVerificationResultCollection Verifications = new TVerificationResultCollection();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Thread postingThread = new Thread(() => PostGLBatch(CurrentBatchNumber, out Verifications));

                using (TProgressDialog dialog = new TProgressDialog(postingThread))
                {
                    dialog.ShowDialog();
                }

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(Verifications))
                {
                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

                    StringBuilder errorMessages = new StringBuilder();
                    int counter = 0;

                    errorMessages.AppendLine(Catalog.GetString("________________________GL Posting Errors________________________"));
                    errorMessages.AppendLine();

                    foreach (TVerificationResult verif in Verifications)
                    {
                        counter++;
                        errorMessages.AppendLine(counter.ToString("000") + " - " + verif.ResultText);
                        errorMessages.AppendLine();
                    }

                    extendedMessageBox.ShowDialog(errorMessages.ToString(),
                        Catalog.GetString("Post Batch Error"),
                        string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);
                }
                else
                {
                    //I don't need to call this directly, because the server calls it:
                    //TFrmGLBatch.PrintPostingRegister(FLedgerNumber, CurrentBatchNumber);

                    // TODO: print reports on successfully posted batch
                    MessageBox.Show(Catalog.GetString("The batch has been posted successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // refresh the grid, to reflect that the batch has been posted
                    FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndRelatedTables(FLedgerNumber, CurrentBatchNumber));

                    // make sure that the current dataset is clean,
                    // otherwise the next save would try to modify the posted batch, even though no values have been changed
                    FMainDS.AcceptChanges();

                    // Ensure these tabs will ask the server for updates
                    FMyForm.GetTransactionsControl().ClearCurrentSelection();
                    FMyForm.GetJournalsControl().ClearCurrentSelection();

                    FMyUserControl.UpdateDisplay();
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            return true;
        }

        /// <summary>
        /// executed by progress dialog thread
        /// </summary>
        private void PostGLBatch(int ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            TRemote.MFinance.GL.WebConnectors.PostGLBatch(FLedgerNumber, ABatchNumber, out AVerifications);
        }

        private void SetAccountCostCentreTableVariables()
        {
            //Populate CostCentreList variable
            DataTable CostCentreListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref CostCentreListTable, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)CostCentreListTable;

            //Populate AccountList variable
            DataTable AccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref AccountListTable, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)AccountListTable;
        }

        private void ShowMessages(TVerificationResultCollection AMessages)
        {
            if (AMessages.Count == 0)
            {
                return;
            }
        }

        /// <summary>
        /// Runs a test on posting a batch
        /// </summary>
        /// <param name="ACurrentBatchRow">The data row corresponding to the batch to post</param>
        public void TestPostBatch(ABatchRow ACurrentBatchRow)
        {
            int CurrentBatchNumber = ACurrentBatchRow.BatchNumber;

            if (FPetraUtilsObject.HasChanges)
            {
                //Keep this conditional check separate so that it only gets called when necessary
                // and doesn't result in the executon of the next else if which calls same method
                if (!FMyForm.SaveChangesManual(FMyForm.FCurrentGLBatchAction))
                {
                    return;
                }
            }
            else if (!FMyForm.GetTransactionsControl().AllowInactiveFieldValues(FLedgerNumber,
                         CurrentBatchNumber, FMyForm.FCurrentGLBatchAction))
            {
                return;
            }

            TVerificationResultCollection Verifications;
            TFrmExtendedMessageBox ExtendedMessageBox = new TFrmExtendedMessageBox(FMyForm);

            FMyForm.Cursor = Cursors.WaitCursor;

            List <TVariant>Result = TRemote.MFinance.GL.WebConnectors.TestPostGLBatch(FLedgerNumber, CurrentBatchNumber, out Verifications);

            try
            {
                if ((Verifications != null) && (Verifications.Count > 0))
                {
                    string ErrorMessages = string.Empty;

                    foreach (TVerificationResult verif in Verifications)
                    {
                        ErrorMessages += "[" + verif.ResultContext + "] " +
                                         verif.ResultTextCaption + ": " +
                                         verif.ResultText + Environment.NewLine;
                    }

                    ExtendedMessageBox.ShowDialog(ErrorMessages,
                        Catalog.GetString("Test Post Failed"), string.Empty,
                        TFrmExtendedMessageBox.TButtons.embbOK,
                        TFrmExtendedMessageBox.TIcon.embiWarning);
                }
                else
                {
                    string header = string.Empty;
                    string message = string.Empty;

                    foreach (TVariant value in Result)
                    {
                        ArrayList compValues = value.ToComposite();

                        message += String.Format(Catalog.GetString("--{1}/{0} ({3}/{2}) is: {4} and would be: {5}{6}"),
                            ((TVariant)compValues[0]).ToString(),
                            ((TVariant)compValues[2]).ToString(),
                            ((TVariant)compValues[1]).ToString(),
                            ((TVariant)compValues[3]).ToString(),
                            StringHelper.FormatCurrency((TVariant)compValues[4], "currency"),
                            StringHelper.FormatCurrency((TVariant)compValues[5], "currency"),
                            Environment.NewLine);
                    }

                    if (Result.Count > 25)
                    {
                        string line = new String('_', 70);
                        header =
                            String.Format(Catalog.GetString(
                                    "{0}{1}{1}{2} results listed below. Do you want to export this list to a CSV file?{1}{0}{1}{1}"),
                                line,
                                Environment.NewLine,
                                Result.Count);

                        if (ExtendedMessageBox.ShowDialog((header + message),
                                Catalog.GetString("Result of Test Posting"), string.Empty,
                                TFrmExtendedMessageBox.TButtons.embbYesNo,
                                TFrmExtendedMessageBox.TIcon.embiQuestion) == TFrmExtendedMessageBox.TResult.embrYes)
                        {
                            // store to CSV file
                            string cSVForExport = string.Empty;
                            string messageExport = string.Empty;

                            foreach (TVariant value in Result)
                            {
                                ArrayList compValues = value.ToComposite();

                                string[] columns = new string[] {
                                    ((TVariant)compValues[0]).ToString(),
                                    ((TVariant)compValues[1]).ToString(),
                                    ((TVariant)compValues[2]).ToString(),
                                    ((TVariant)compValues[3]).ToString(),
                                    StringHelper.FormatCurrency((TVariant)compValues[4], "CurrencyCSV"),
                                    StringHelper.FormatCurrency((TVariant)compValues[5], "CurrencyCSV")
                                };

                                cSVForExport += StringHelper.StrMerge(columns,
                                    Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator[0]) +
                                                Environment.NewLine;
                            }

                            try
                            {
                                string CSVFilePath = TClientSettings.PathLog + Path.DirectorySeparatorChar + "Batch" +
                                                     CurrentBatchNumber.ToString() +
                                                     "_TestPosting.csv";

                                StreamWriter sw = new StreamWriter(CSVFilePath, false, System.Text.Encoding.UTF8);
                                sw.Write(cSVForExport);
                                sw.Close();

                                messageExport = String.Format(Catalog.GetString("Please see the results in the file:{1}{1}'{0}'{1}{1}"),
                                    CSVFilePath,
                                    Environment.NewLine);
                                messageExport += "Do you want to open the file for viewing now?";

                                if (MessageBox.Show(messageExport,
                                        Catalog.GetString("Result of Test Posting"),
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        ProcessStartInfo si = new ProcessStartInfo(CSVFilePath);
                                        si.UseShellExecute = true;
                                        si.Verb = "open";

                                        Process p = new Process();
                                        p.StartInfo = si;
                                        p.Start();
                                    }
                                    catch
                                    {
                                        MessageBox.Show(Catalog.GetString(
                                                "Unable to launch the default application to open: '") + CSVFilePath + "'!", Catalog.GetString(
                                                "Result of Test Posting"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                                        //If windows start Windows File Explorer
                                        TExecutingOSEnum osVersion = Utilities.DetermineExecutingOS();

                                        if ((osVersion >= TExecutingOSEnum.eosWinXP)
                                            && (osVersion < TExecutingOSEnum.oesUnsupportedPlatform))
                                        {
                                            try
                                            {
                                                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", CSVFilePath));
                                            }
                                            catch
                                            {
                                                MessageBox.Show(Catalog.GetString(
                                                        "Unable to launch Windows File Explorer to open: '") + CSVFilePath + "'!", Catalog.GetString(
                                                        "Result of Test Posting"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error trying to export test posting results to CSV file: " + ex.Message,
                                    "Test Posting Results Export",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        ExtendedMessageBox.ShowDialog(message,
                            Catalog.GetString("Result of Test Posting"), string.Empty,
                            TFrmExtendedMessageBox.TButtons.embbOK,
                            TFrmExtendedMessageBox.TIcon.embiInformation);
                    }
                }
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Helper methods


        #endregion
    }
}
