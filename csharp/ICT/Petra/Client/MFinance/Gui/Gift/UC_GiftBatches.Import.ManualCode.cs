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
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles importing of batches
    /// </summary>
    public class TUC_GiftBatches_Import
    {
        /// <summary>
        /// A enumeration of available import data sources
        /// </summary>
        public enum TGiftImportDataSourceEnum
        {
            /// <summary>
            /// Import from a CSV file
            /// </summary>
            FromFile,

            /// <summary>
            /// Import from the clipboard in csv format
            /// </summary>
            FromClipboard
        };

        private TFrmGiftBatch FMyForm = null;
        private TDlgSelectCSVSeparator FdlgSeparator;

        private TFrmPetraEditUtils FPetraUtilsObject = null;
        private Int32 FLedgerNumber = 0;
        private IUC_GiftBatches FMyUserControl = null;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_Import(TFrmPetraEditUtils APetraUtilsObject, Int32 ALedgerNumber, IUC_GiftBatches AUserControl)
        {
            FPetraUtilsObject = APetraUtilsObject;
            FLedgerNumber = ALedgerNumber;
            FMyUserControl = AUserControl;
            FMyForm = (TFrmGiftBatch)FPetraUtilsObject.GetForm();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        public void ImportBatches(TGiftImportDataSourceEnum AImportSource, GiftBatchTDS AMainDS)
        {
            bool ImportOK = false;
            OpenFileDialog OFileDialog = null;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save before calling this function!"), Catalog.GetString(
                        "Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ALedgerRow LedgerRow = (ALedgerRow)AMainDS.ALedger.Rows[0];
            int CurrentTopBatchNumber = LedgerRow.LastGiftBatchNumber;

            try
            {
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.IMPORTING;

                FdlgSeparator = new TDlgSelectCSVSeparator(false);

                if (AImportSource == TGiftImportDataSourceEnum.FromClipboard)
                {
                    string ImportString = Clipboard.GetText(TextDataFormat.UnicodeText);

                    if ((ImportString == null) || (ImportString.Length == 0))
                    {
                        MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                            Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FdlgSeparator.CSVData = ImportString;
                }
                else if (AImportSource == TGiftImportDataSourceEnum.FromFile)
                {
                    OFileDialog = new OpenFileDialog();

                    string exportPath = TClientSettings.GetExportPath();
                    string fullPath = TUserDefaults.GetStringDefault("Imp Filename",
                        exportPath + Path.DirectorySeparatorChar + "import.csv");
                    TImportExportDialogs.SetOpenFileDialogFilePathAndName(OFileDialog, fullPath, exportPath);

                    OFileDialog.Title = Catalog.GetString("Import Batches from CSV File");
                    OFileDialog.Filter = Catalog.GetString("Gift Batch Files(*.csv)|*.csv|Text Files(*.txt)|*.txt");

                    // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
                    TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(fullPath));

                    if (OFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(OFileDialog.FileName);

                        if (!fileCanOpen)
                        {
                            MessageBox.Show(Catalog.GetString("Unable to open file."),
                                Catalog.GetString("Gift Import"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    // unknown source!!
                    return;
                }

                String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);
                String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");

                FdlgSeparator.DateFormat = dateFormatString;
                FdlgSeparator.NumberFormat = (impOptions.Length > 1) ? impOptions.Substring(1) : TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
                FdlgSeparator.SelectedSeparator = StringHelper.GetCSVSeparator(FdlgSeparator.FileContent) ??
                                                  ((impOptions.Length > 0) ? impOptions.Substring(0, 1) : ";");

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

                        TVerificationResultCollection AMessages = new TVerificationResultCollection();
                        GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();

                        Thread ImportThread = new Thread(() => ImportGiftBatches(
                                requestParams,
                                FdlgSeparator.FileContent,
                                out AMessages,
                                out ImportOK,
                                out NeedRecipientLedgerNumber));

                        using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                        {
                            ImportDialog.ShowDialog();
                        }

                        // If NeedRecipientLedgerNumber contains data then AMessages will only ever contain
                        // one message alerting the user that no data has been imported.
                        // We do not want to show this as we will be displaying another more detailed message.
                        if (NeedRecipientLedgerNumber.Rows.Count == 0)
                        {
                            ShowMessages(AMessages);
                        }

                        // if the import contains gifts with Motivation Group 'GIFT' and that have a Family recipient with no Gift Destination
                        // then the import will have failed and we need to alert the user
                        if (NeedRecipientLedgerNumber.Rows.Count > 0)
                        {
                            bool OfferToRunImportAgain = true;
                            bool DoNotShowMessageBoxEverytime = false;
                            TFrmExtendedMessageBox.TResult Result = TFrmExtendedMessageBox.TResult.embrUndefined;
                            int count = 1;

                            // for each gift in which the recipient needs a Git Destination
                            foreach (GiftBatchTDSAGiftDetailRow Row in NeedRecipientLedgerNumber.Rows)
                            {
                                if (!DoNotShowMessageBoxEverytime)
                                {
                                    string CheckboxText = string.Empty;

                                    // only show checkbox if there is at least one more occurrence of this error
                                    if (NeedRecipientLedgerNumber.Rows.Count - count > 0)
                                    {
                                        CheckboxText = string.Format(
                                            Catalog.GetString(
                                                "Do this for all further occurrences ({0})?"), NeedRecipientLedgerNumber.Rows.Count - count);
                                    }

                                    TFrmExtendedMessageBox extendedMessageBox = new TFrmExtendedMessageBox(FPetraUtilsObject.GetForm());

                                    extendedMessageBox.ShowDialog(string.Format(
                                            Catalog.GetString(
                                                "Gift Import has been cancelled as the recipient '{0}' ({1}) has no Gift Destination assigned."),
                                            Row.RecipientDescription, Row.RecipientKey) +
                                        "\n\r\n\r\n\r" +
                                        Catalog.GetString("Do you want to assign a Gift Destination to this partner now?"),
                                        Catalog.GetString("Import Errors"),
                                        CheckboxText,
                                        TFrmExtendedMessageBox.TButtons.embbYesNo, TFrmExtendedMessageBox.TIcon.embiWarning);
                                    Result = extendedMessageBox.GetResult(out DoNotShowMessageBoxEverytime);
                                }

                                if (Result == TFrmExtendedMessageBox.TResult.embrYes)
                                {
                                    // allow the user to assign a Gift Destingation
                                    TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(FPetraUtilsObject.GetForm(), Row.RecipientKey);
                                    GiftDestinationForm.ShowDialog();
                                }
                                else
                                {
                                    OfferToRunImportAgain = false;

                                    if (DoNotShowMessageBoxEverytime)
                                    {
                                        break;
                                    }
                                }

                                count++;
                            }

                            // if the user has clicked yes to assigning Gift Destinations then offer to restart the import
                            if (OfferToRunImportAgain
                                && (MessageBox.Show(Catalog.GetString("Would you like to import this Gift Batch again?"),
                                        Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2)
                                    == DialogResult.Yes))
                            {
                                Repeat = true;
                            }
                        }
                    }
                }

                // We save the defaults even if ok is false - because the client will probably want to try and import
                //   the same file again after correcting any errors
                SaveUserDefaults(OFileDialog);

                if (ImportOK)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Gift Import"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    FMyUserControl.LoadBatchesForCurrentYear();
                    FMyForm.GetBatchControl().SelectRowInBatchGrid(1);

                    DataView allNewBatches = new DataView(AMainDS.AGiftBatch);

                    allNewBatches.RowFilter = String.Format("{0} > {1}",
                        AGiftBatchTable.GetBatchNumberDBName(),
                        CurrentTopBatchNumber);

                    foreach (DataRowView drv in allNewBatches)
                    {
                        drv.Row.SetModified();
                    }

                    FPetraUtilsObject.SetChangedFlag();
                    //Force initial inactive values check
                    FMyForm.SaveChangesManual(FMyForm.FCurrentGiftBatchAction);
                }
            }
            finally
            {
                FMyForm.FCurrentGiftBatchAction = Logic.TExtraGiftBatchChecks.GiftBatchAction.NONE;
            }
        }

        /// <summary>
        /// Import a transactions file or a clipboard equivalent
        /// </summary>
        /// <param name="ACurrentBatchRow">The batch to import to</param>
        /// <param name="AImportSource">The import source - eg File or Clipboard</param>
        /// <returns>True if the import was successful</returns>
        public bool ImportTransactions(AGiftBatchRow ACurrentBatchRow, TGiftImportDataSourceEnum AImportSource)
        {
            bool ok = false;
            OpenFileDialog dialog = null;
            Boolean IsPlainText = false;

            if (FPetraUtilsObject.HasChanges)
            {
                // saving failed, therefore do not try to import
                MessageBox.Show(Catalog.GetString("Please save any changes before calling this function!"), Catalog.GetString(
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

            FdlgSeparator = new TDlgSelectCSVSeparator(false);

            if (AImportSource == TGiftImportDataSourceEnum.FromClipboard)
            {
                string importString = Clipboard.GetText(TextDataFormat.UnicodeText);

                if ((importString == null) || (importString.Length == 0))
                {
                    MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                        Catalog.GetString("Failure"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                FdlgSeparator.CSVData = importString;
            }
            else if (AImportSource == TGiftImportDataSourceEnum.FromFile)
            {
                dialog = new OpenFileDialog();

                string exportPath = TClientSettings.GetExportPath();
                string fullPath = TUserDefaults.GetStringDefault("Imp Filename",
                    exportPath + Path.DirectorySeparatorChar + "import.csv");
                TImportExportDialogs.SetOpenFileDialogFilePathAndName(dialog, fullPath, exportPath);

                dialog.Title = Catalog.GetString("Import Transactions from CSV File");
                dialog.Filter = Catalog.GetString("Gift Transactions files (*.csv)|*.csv|Text Files (*.txt)|*.txt");

                // This call fixes Windows7 Open File Dialogs.  It must be the line before ShowDialog()
                TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(fullPath));

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Boolean fileCanOpen = FdlgSeparator.OpenCsvFile(dialog.FileName);

                    if (!fileCanOpen)
                    {
                        MessageBox.Show(Catalog.GetString("Unable to open file."),
                            Catalog.GetString("Gift Import"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        return false;
                    }

                    IsPlainText = (Path.GetExtension(dialog.FileName).ToLower() == ".txt");
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // unknown source!!  The following need a value...
                return false;
            }

            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);
            String dateFormatString = TUserDefaults.GetStringDefault("Imp Date", "MDY");

            FdlgSeparator.DateFormat = dateFormatString;
            FdlgSeparator.NumberFormat = (impOptions.Length > 1) ? impOptions.Substring(1) : TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;
            FdlgSeparator.SelectedSeparator = StringHelper.GetCSVSeparator(FdlgSeparator.FileContent) ??
                                              ((impOptions.Length > 0) ? impOptions.Substring(0, 1) : ";");

            if (IsPlainText || (FdlgSeparator.ShowDialog() == DialogResult.OK))
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

                    TVerificationResultCollection AMessages = new TVerificationResultCollection();
                    GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();

                    Thread ImportThread = new Thread(() => ImportGiftTransactions(
                            requestParams,
                            FdlgSeparator.FileContent,
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
                    int numberOfMissingGiftDestinations = NeedRecipientLedgerNumber.Rows.Count;

                    if (numberOfMissingGiftDestinations > 0)
                    {
                        bool offerToRunImportAgain = true;
                        int currentMissingGiftDestinationNo = 1;

                        // for each gift in which the recipient needs a Git Destination
                        foreach (GiftBatchTDSAGiftDetailRow Row in NeedRecipientLedgerNumber.Rows)
                        {
                            //Lookup the partner shortname
                            string partnerShortName = string.Empty;
                            TPartnerClass partnerClass;

                            if (TServerLookup.TMPartner.GetPartnerShortName(Row.RecipientKey, out partnerShortName, out partnerClass))
                            {
                                Row.RecipientDescription = partnerShortName;
                            }

                            if (MessageBox.Show(string.Format(
                                        Catalog.GetString(
                                            "Error: {0:0000} of {1:0000} - Recipient '{2}' ({3}) has no Gift Destination assigned."),
                                        currentMissingGiftDestinationNo, numberOfMissingGiftDestinations, Row.RecipientDescription,
                                        Row.RecipientKey) +
                                    "\n\n" +
                                    Catalog.GetString("Do you want to assign a Gift Destination to this partner now?"),
                                    Catalog.GetString("Gift Import Cancelled"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                                == DialogResult.Yes)
                            {
                                // allow the user to assign a Gift Destingation
                                TFrmGiftDestination GiftDestinationForm = new TFrmGiftDestination(FPetraUtilsObject.GetForm(), Row.RecipientKey);
                                GiftDestinationForm.ShowDialog();
                            }
                            else
                            {
                                offerToRunImportAgain = false;
                            }

                            currentMissingGiftDestinationNo++;
                        }

                        // if the user has clicked yes to assigning Gift Destinations then offer to restart the import
                        if (offerToRunImportAgain
                            && (MessageBox.Show(Catalog.GetString("Would you like to import these Gift Transactions again?"),
                                    Catalog.GetString("Gift Import"), MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button1)
                                == DialogResult.Yes))
                        {
                            Repeat = true;
                        }
                    }
                }
            }

            // We save the defaults even if ok is false - because the client will probably want to try and import
            //   the same file again after correcting any errors
            if (!IsPlainText)
            {
                SaveUserDefaults(dialog);
            }

            if (ok)
            {
                MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                    Catalog.GetString("Gift Import"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                FMyUserControl.LoadBatchesForCurrentYear();
                FPetraUtilsObject.DisableSaveButton();
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

        private void SaveUserDefaults(OpenFileDialog dialog)
        {
            if (dialog != null)
            {
                TUserDefaults.SetDefault("Imp Filename", dialog.FileName);
            }

            string impOptions = FdlgSeparator.SelectedSeparator;
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