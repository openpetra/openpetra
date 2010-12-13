//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.App.Core;


namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftBatches
    {
        private String FImportMessage;
        private String FImportLine;
        private TDlgSelectCSVSeparator FdlgSeparator = null;
        GiftBatchTDS FMergeDS = null;
        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches(System.Object sender, System.EventArgs e)
        {
            bool ok = false;
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import batches from spreadsheet file");
            dialog.Filter = Catalog.GetString("GL Batches files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TDlgSelectCSVSeparator FdlgSeparator = new TDlgSelectCSVSeparator(false);
                FdlgSeparator.CSVFileName = dialog.FileName;

                if (FdlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    Hashtable requestParams = new Hashtable();

                    requestParams.Add("ALedgerNumber", FLedgerNumber);
                    requestParams.Add("Delimiter", FdlgSeparator.SelectedSeparator);
                    requestParams.Add("DateFormatString", FdlgSeparator.DateFormat);
                    //requestParams.Add("NumberFormat", FdlgSeparator.N);


                    String importString;
                    TVerificationResultCollection AMessages;


                    importString = File.ReadAllText(dialog.FileName);
                    string ErrorMessages = String.Empty;

                    ok = TRemote.MFinance.Gift.WebConnectors.ImportGiftBatchData(
                        requestParams,
                        importString,
                        out AMessages,
                        out FMergeDS);

                    if (AMessages.Count > 0)
                    {
                        foreach (TVerificationResult message in AMessages)
                        {
                            ErrorMessages += "[" + message.ResultContext + "] " +
                                             message.ResultTextCaption + ": " +
                                             message.ResultText + Environment.NewLine;
                        }
                    }

                    if (ErrorMessages.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Warning"),

                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }

                if (ok)
                {
                    FMainDS.Merge(FMergeDS);
                    //TODO refresh the gui
                }
            }
        }

        private String ImportString(String message)
        {
            FImportMessage = Catalog.GetString("Parsing the " + message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
            return sReturn;
        }

        private Boolean ImportBoolean(String message)
        {
            FImportMessage = Catalog.GetString("Parsing the " + message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
            return sReturn.ToLower().Equals("yes");
        }

        private Int64 ImportInt64(String message)
        {
            FImportMessage = Catalog.GetString("Parsing the " + message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
            return Convert.ToInt64(sReturn);
        }

        private Int32 ImportInt32(String message)
        {
            FImportMessage = Catalog.GetString("Parsing the " + message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
            return Convert.ToInt32(sReturn);
        }

        private Double ImportDouble(String message)
        {
            FImportMessage = Catalog.GetString("Parsing the " + message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FdlgSeparator.SelectedSeparator);
            return Convert.ToDouble(sReturn);
        }
    }
}