//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, dougm, alanP
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
using System.Collections;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles importing of batches
    /// </summary>
    public class TUC_GiftBatches_Import
    {
        private TDlgSelectCSVSeparator FdlgSeparator;

        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private IUC_GiftBatches FMyUserControl = null;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_Import(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, GiftBatchTDS AMainDS, IUC_GiftBatches AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;
            FMyUserControl = AUserControl;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        public void ImportBatches()
        {
            bool ok = false;

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

            dialog.Title = Catalog.GetString("Import batches from spreadsheet file");
            dialog.Filter = Catalog.GetString("Gift Batches files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                if (!fileCanOpen)
                {
                    MessageBox.Show(Catalog.GetString("Unable to open file."),
                        Catalog.GetString("Gift Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }

                FdlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                    requestParams.Add("NewLine", Environment.NewLine);

                    bool Repeat = true;

                    while (Repeat)
                    {
                        Repeat = false;

                        String importString = File.ReadAllText(dialog.FileName);
                        TVerificationResultCollection AMessages = new TVerificationResultCollection();
                        GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();

                        Thread ImportThread = new Thread(() => ImportGiftBatches(
                                requestParams,
                                importString,
                                out AMessages,
                                out ok,
                                out NeedRecipientLedgerNumber));

                        using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                        {
                            ImportDialog.ShowDialog();
                        }

                        ShowMessages(AMessages);

                        // if the import contains gifts with Motivation Group 'GIFT' and that have a Family recipient with no Gift Destination
                        // then the import will have failed and we need to alert the user
                        if (NeedRecipientLedgerNumber.Rows.Count > 0)
                        {
                            bool OfferToRunImportAgain = true;

                            // for each gift in which the recipient needs a Git Destination
                            foreach (GiftBatchTDSAGiftDetailRow Row in NeedRecipientLedgerNumber.Rows)
                            {
                                if (MessageBox.Show(string.Format(
                                            Catalog.GetString(
                                                "Gift Import has been cancelled as the recipient '{0}' ({1}) has no Gift Destination assigned."),
                                            Row.RecipientDescription, Row.RecipientKey) +
                                        "\n\n" +
                                        Catalog.GetString("Do you want to assign a Gift Destination to this partner now?"),
                                        Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == DialogResult.Yes)
                                {
                                    // allow the user to assign a Gift Destingation
                                    TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(FPetraUtilsObject.GetForm(), Row.RecipientKey);
                                    GiftDestinationForm.ShowDialog();
                                }
                                else
                                {
                                    OfferToRunImportAgain = false;
                                }
                            }

                            // if the user has clicked yes to assigning Gift Destinations then offer to restart the import
                            if (OfferToRunImportAgain
                                && (MessageBox.Show(Catalog.GetString("Would you like to import this Gift Batch again?"),
                                        Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.Yes))
                            {
                                Repeat = true;
                            }
                        }
                    }
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Gift Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SaveUserDefaults(dialog, impOptions);
                    FMyUserControl.LoadBatchesForCurrentYear();
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// Import a transactions file
        /// </summary>
        /// <param name="ACurrentBatchRow">The batch to import to</param>
        /// <returns>True if the import was successful</returns>
        public bool ImportTransactions(AGiftBatchRow ACurrentBatchRow)
        {
            bool ok = false;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Gift Import"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if ((ACurrentBatchRow == null) || (ACurrentBatchRow.BatchStatus != MFinanceConstants.BATCH_UNPOSTED))
            {
                MessageBox.Show(Catalog.GetString("Please select an unposted batch to import transactions."), Catalog.GetString(
                        "Gift Import"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ACurrentBatchRow.LastGiftNumber > 0)
            {
                if (MessageBox.Show(Catalog.GetString(
                            "The current batch already contains some gift transactions.  Do you really want to add more transactions to this batch?"),
                        Catalog.GetString("Gift Transaction Import"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }
            }

            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.FileName = TUserDefaults.GetStringDefault("Imp Filename",
                TClientSettings.GetExportPath() + Path.DirectorySeparatorChar + "import.csv");

            dialog.Title = Catalog.GetString("Import transactions from spreadsheet file");
            dialog.Filter = Catalog.GetString("Gift Transactions files (*.csv)|*.csv");
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FdlgSeparator = new TDlgSelectCSVSeparator(false);
                Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                if (!fileCanOpen)
                {
                    MessageBox.Show(Catalog.GetString("Unable to open file."),
                        Catalog.GetString("Gift Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return false;
                }

                FdlgSeparator.DateFormat = dateFormatString;

                if (impOptions.Length > 1)
                {
                    FdlgSeparator.NumberFormat = impOptions.Substring(1);
                }

                FdlgSeparator.SelectedSeparator = impOptions.Substring(0, 1);

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    requestParams.Add("NumberFormat", FdlgSeparator.NumberFormat);
                    requestParams.Add("NewLine", Environment.NewLine);

                    bool Repeat = true;

                    while (Repeat)
                    {
                        Repeat = false;

                        String importString = File.ReadAllText(dialog.FileName);
                        TVerificationResultCollection AMessages = new TVerificationResultCollection();
                        GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();

                        Thread ImportThread = new Thread(() => ImportGiftTransactions(
                                requestParams,
                                importString,
                                ACurrentBatchRow.BatchNumber,
                                out AMessages,
                                out ok,
                                out NeedRecipientLedgerNumber));

                        using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                        {
                            ImportDialog.ShowDialog();
                        }

                        ShowMessages(AMessages);

                        // if the import contains gifts with Motivation Group 'GIFT' and that have a Family recipient with no Gift Destination
                        // then the import will have failed and we need to alert the user
                        if (NeedRecipientLedgerNumber.Rows.Count > 0)
                        {
                            bool OfferToRunImportAgain = true;

                            // for each gift in which the recipient needs a Git Destination
                            foreach (GiftBatchTDSAGiftDetailRow Row in NeedRecipientLedgerNumber.Rows)
                            {
                                if (MessageBox.Show(string.Format(
                                            Catalog.GetString(
                                                "Gift Import has been cancelled as the recipient '{0}' ({1}) has no Gift Destination assigned."),
                                            Row.RecipientDescription, Row.RecipientKey) +
                                        "\n\n" +
                                        Catalog.GetString("Do you want to assign a Gift Destination to this partner now?"),
                                        Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                    == DialogResult.Yes)
                                {
                                    // allow the user to assign a Gift Destingation
                                    TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(FPetraUtilsObject.GetForm(), Row.RecipientKey);
                                    GiftDestinationForm.ShowDialog();
                                }
                                else
                                {
                                    OfferToRunImportAgain = false;
                                }
                            }

                            // if the user has clicked yes to assigning Gift Destinations then offer to restart the import
                            if (OfferToRunImportAgain
                                && (MessageBox.Show(Catalog.GetString("Would you like to import these Gift Transactions again?"),
                                        Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    == DialogResult.Yes))
                            {
                                Repeat = true;
                            }
                        }
                    }
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Gift Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SaveUserDefaults(dialog, impOptions);
                    //FMyUserControl.LoadBatchesForCurrentYear();
                    FPetraUtilsObject.DisableSaveButton();
                }
            }

            return ok;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ImportGiftBatches
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="AMessages"></param>
        /// <param name="ok"></param>
        /// <param name="ANeedRecipientLedgerNumber"></param>
        private void ImportGiftBatches(Hashtable ARequestParams, string AImportString,
            out TVerificationResultCollection AMessages, out bool ok, out GiftBatchTDSAGiftDetailTable ANeedRecipientLedgerNumber)
        {
            TVerificationResultCollection AResultMessages;
            bool ImportIsSuccessful;


            ImportIsSuccessful = TRemote.MFinance.Gift.WebConnectors.ImportGiftBatches(
                ARequestParams,
                AImportString,
                out ANeedRecipientLedgerNumber,
                out AResultMessages);

            ok = ImportIsSuccessful;
            AMessages = AResultMessages;
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ImportGiftTransactions
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AMessages"></param>
        /// <param name="ok"></param>
        /// <param name="ANeedRecipientLedgerNumber"></param>
        private void ImportGiftTransactions(Hashtable ARequestParams, string AImportString, Int32 ABatchNumber,
            out TVerificationResultCollection AMessages, out bool ok, out GiftBatchTDSAGiftDetailTable ANeedRecipientLedgerNumber)
        {
            TVerificationResultCollection AResultMessages;
            bool ImportIsSuccessful;

            ImportIsSuccessful = TRemote.MFinance.Gift.WebConnectors.ImportGiftTransactions(
                ARequestParams,
                AImportString,
                ABatchNumber,
                out ANeedRecipientLedgerNumber,
                out AResultMessages);

            ok = ImportIsSuccessful;
            AMessages = AResultMessages;
        }

        private void SaveUserDefaults(OpenFileDialog dialog, String impOptions)
        {
            TUserDefaults.SetDefault("Imp Filename", dialog.FileName);
            impOptions = FdlgSeparator.SelectedSeparator;
            impOptions += FdlgSeparator.NumberFormat;
            TUserDefaults.SetDefault("Imp Options", impOptions);
            TUserDefaults.SetDefault("Imp Date", FdlgSeparator.DateFormat);
            TUserDefaults.SaveChangedUserDefaults();
        }

        private void ShowMessages(TVerificationResultCollection AMessages)
        {
            StringBuilder ErrorMessages = new StringBuilder();

            if (AMessages.Count > 0)
            {
                foreach (TVerificationResult message in AMessages)
                {
                    ErrorMessages.AppendFormat("[{0}] {1}: {2}{3}", message.ResultContext, message.ResultTextCaption,
                        message.ResultText.Replace(Environment.NewLine, " "), Environment.NewLine);
                }
            }

            if (ErrorMessages.Length > 0)
            {
                TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FPetraUtilsObject.GetForm());
                extendedMessageBox.ShowDialog(ErrorMessages.ToString(), Catalog.GetString("Import Errors"), String.Empty,
                    TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiError);
            }
        }

        #endregion
    }
}