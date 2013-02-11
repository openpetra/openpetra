//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, dougm
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
using System.IO;
using System.Windows.Forms;
using System.Threading;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftBatches
    {
        private TDlgSelectCSVSeparator FdlgSeparator;
        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches(System.Object sender, System.EventArgs e)
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
                FdlgSeparator.CSVFileName = dialog.FileName;

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

                    String importString = File.ReadAllText(dialog.FileName);
                    TVerificationResultCollection AMessages = new TVerificationResultCollection();

                    Thread ImportThread = new Thread(() => ImportGiftBatches(
                            requestParams,
                            importString,
                            out AMessages,
                            out ok));

                    using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
                    {
                        ImportDialog.ShowDialog();
                    }

                    ShowMessages(AMessages);
                }

                if (ok)
                {
                    MessageBox.Show(Catalog.GetString("Your data was imported successfully!"),
                        Catalog.GetString("Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    SaveUserDefaults(dialog, impOptions);
                    LoadBatches(FLedgerNumber);
                    FPetraUtilsObject.DisableSaveButton();
                }
            }
        }

        /// <summary>
        /// Wrapper method to handle returned bool value from remoting call to ImportGiftBatches
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="AMessages"></param>
        /// <param name="ok"></param>
        private void ImportGiftBatches(Hashtable ARequestParams, string AImportString,
            out TVerificationResultCollection AMessages, out bool ok)
        {
            TVerificationResultCollection AResultMessages;
            bool ImportIsSuccessful;

            ImportIsSuccessful = TRemote.MFinance.Gift.WebConnectors.ImportGiftBatches(
                ARequestParams,
                AImportString,
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
            string ErrorMessages = String.Empty;

            if (AMessages.Count > 0)
            {
                foreach (TVerificationResult message in AMessages)
                {
                    ErrorMessages += "[" + message.ResultContext + "] " + message.ResultTextCaption + ": " + message.ResultText + Environment.NewLine;
                }
            }

            if (ErrorMessages.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Warning"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}