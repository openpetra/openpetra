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
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;

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
            bool RetVal = false;

            if (!SaveBatchForPosting())
            {
                return RetVal;
            }

            // TODO: display progress of posting
            TVerificationResultCollection Verifications;

            int CurrentBatchNumber = ACurrentBatchRow.BatchNumber;

            if ((AEffectiveDate.Date < AStartDateCurrentPeriod) || (AEffectiveDate.Date > AEndDateLastForwardingPeriod))
            {
                MessageBox.Show(String.Format(Catalog.GetString(
                            "The Date Effective is outside the periods available for posting. Enter a date between {0:d} and {1:d}."),
                        AStartDateCurrentPeriod,
                        AEndDateLastForwardingPeriod));

                return RetVal;
            }

            // check that a corportate exchange rate exists
            FMyForm.WarnAboutMissingIntlExchangeRate = true;

            if (FMyForm.GetInternationalCurrencyExchangeRate() == 0)
            {
                return false;
            }

            if (MessageBox.Show(String.Format(Catalog.GetString("Are you sure you want to post batch {0}?"),
                        CurrentBatchNumber),
                    Catalog.GetString("Question"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (!TRemote.MFinance.GL.WebConnectors.PostGLBatch(FLedgerNumber, CurrentBatchNumber, out Verifications))
                    {
                        string ErrorMessages = String.Empty;

                        foreach (TVerificationResult verif in Verifications)
                        {
                            ErrorMessages += "[" + verif.ResultContext + "] " +
                                             verif.ResultTextCaption + ": " +
                                             verif.ResultText + Environment.NewLine;
                        }

                        System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
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
                        FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadABatchAndContent(FLedgerNumber, CurrentBatchNumber));

                        // make sure that the current dataset is clean,
                        // otherwise the next save would try to modify the posted batch, even though no values have been changed
                        FMainDS.AcceptChanges();

                        // Ensure these tabs will ask the server for updates
                        FMyForm.GetJournalsControl().ClearCurrentSelection();
                        FMyForm.GetTransactionsControl().ClearCurrentSelection();

                        FMyUserControl.UpdateDisplay();

                        RetVal = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = (String.Format(Catalog.GetString("Unexpected error occurred during the posting of a GL Batch!{0}{1}{2}{1}    {3}"),
                                      Utilities.GetMethodSignature(),
                                      Environment.NewLine,
                                      ex.Message,
                                      ex.InnerException!=null?ex.InnerException.Message:String.Empty));

                    TLogging.Log(msg);
                    MessageBox.Show(msg, "Post GL Batch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Runs a test on posting a batch
        /// </summary>
        /// <param name="ACurrentBatchRow">The data row corresponding to the batch to post</param>
        public void TestPostBatch(ABatchRow ACurrentBatchRow)
        {
            if (!SaveBatchForPosting())
            {
                return;
            }

            TVerificationResultCollection Verifications;

            FMyForm.Cursor = Cursors.WaitCursor;

            List <TVariant>Result = TRemote.MFinance.GL.WebConnectors.TestPostGLBatch(FLedgerNumber, ACurrentBatchRow.BatchNumber, out Verifications);

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

                    System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else if (Result.Count < 25)
                {
                    string message = string.Empty;

                    foreach (TVariant value in Result)
                    {
                        ArrayList compValues = value.ToComposite();

                        message +=
                            string.Format(
                                Catalog.GetString("{1}/{0} ({3}/{2}) is: {4} and would be: {5}"),
                                ((TVariant)compValues[0]).ToString(),
                                ((TVariant)compValues[2]).ToString(),
                                ((TVariant)compValues[1]).ToString(),
                                ((TVariant)compValues[3]).ToString(),
                                StringHelper.FormatCurrency((TVariant)compValues[4], "currency"),
                                StringHelper.FormatCurrency((TVariant)compValues[5], "currency")) +
                            Environment.NewLine;
                    }

                    MessageBox.Show(message, Catalog.GetString("Result of Test Posting"));
                }
                else
                {
                    // store to CSV file
                    string message = string.Empty;

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

                        message += StringHelper.StrMerge(columns,
                            Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator[0]) +
                                   Environment.NewLine;
                    }

                    string CSVFilePath = TClientSettings.PathLog + Path.DirectorySeparatorChar + "Batch" + ACurrentBatchRow.BatchNumber.ToString() +
                                         "_TestPosting.csv";
                    StreamWriter sw = new StreamWriter(CSVFilePath, false, System.Text.Encoding.UTF8);
                    sw.Write(message);
                    sw.Close();

                    MessageBox.Show(
                        String.Format(Catalog.GetString("Please see file {0} for the result of the test posting"), CSVFilePath),
                        Catalog.GetString("Result of Test Posting"));
                }
            }
            finally
            {
                FMyForm.Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Helper methods

        private bool SaveBatchForPosting()
        {
            // save first, then post
            if (!FMyForm.SaveChanges())
            {
                // saving failed, therefore do not try to post
                MessageBox.Show(Catalog.GetString("The batch was not posted due to problems during saving; ") + Environment.NewLine +
                    Catalog.GetString("Please first save the batch, and then post it!"),
                    Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
