//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2016 by OM International
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
using System.IO;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// manual methods for the generated window
    public partial class TFrmImportPartnerTaxCodesDialog : System.Windows.Forms.Form
    {
        private const string PARTNER_IMPORT_TAX_CODE_OPTIONS = "PartnerImportTaxCode_Options";
        private const string PARTNER_IMPORT_TAX_CODE_OUTFILE = "PartnerImportTaxCode_Outfile";
        private const string PARTNER_IMPORT_TAX_CODE_INFILE = "PartnerImportTaxCode_Infile";

        private string FTaxGovIdLabel = string.Empty;
        private string FTaxGovIdKeyName = string.Empty;

        private void InitializeManualCode()
        {
            FTaxGovIdLabel =
                TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_GOVID_LABEL, string.Empty);

            FTaxGovIdKeyName =
                TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_GOVID_DB_KEY_NAME, string.Empty);

            if (FTaxGovIdLabel != string.Empty)
            {
                this.Text = string.Format("{0} ({1})", this.Text, FTaxGovIdLabel);
            }

            nudPartnerKeyColumn.Minimum = 1;
            nudPartnerKeyColumn.Maximum = 25;
            nudPartnerKeyColumn.Increment = 1;
            nudTaxCodeColumn.Minimum = 1;
            nudTaxCodeColumn.Maximum = 25;
            nudTaxCodeColumn.Increment = 1;
        }

        private void RunOnceOnActivationManual()
        {
            // Load the settings from user preferences
            string[] options = TUserDefaults.GetStringDefault(PARTNER_IMPORT_TAX_CODE_OPTIONS, string.Empty).Split(';');

            if (options.Length >= 11)
            {
                rbtFromFile.Checked = Convert.ToInt16(options[0]) != 0;
                nudPartnerKeyColumn.Value = Convert.ToInt16(options[1]);
                nudTaxCodeColumn.Value = Convert.ToInt16(options[2]);
                chkFirstRowIsHeader.Checked = Convert.ToInt16(options[3]) != 0;
                int taxCodeOption = Convert.ToInt16(options[4]);
                rbtSkipEmptyTaxCode.Checked = taxCodeOption == 1;
                rbtDeleteEmptyTaxCode.Checked = taxCodeOption == 2;
                chkFailIfNotPerson.Checked = Convert.ToInt16(options[5]) != 0;
                chkFailInvalidPartner.Checked = Convert.ToInt16(options[6]) != 0;
                chkOverwriteExistingTaxCode.Checked = Convert.ToInt16(options[7]) != 0;
                chkCreateExtract.Checked = Convert.ToInt16(options[8]) != 0;
                chkCreateOutFile.Checked = Convert.ToInt16(options[9]) != 0;
                chkIncludePartnerDetails.Checked = Convert.ToInt16(options[10]) != 0;
            }
            else
            {
                chkFailIfNotPerson.Checked = true;
                chkFailInvalidPartner.Checked = true;
                chkCreateOutFile.Checked = true;
                chkIncludePartnerDetails.Checked = true;
                nudTaxCodeColumn.Value = 2;
            }

            txtFileName.Text = TUserDefaults.GetStringDefault(PARTNER_IMPORT_TAX_CODE_INFILE, string.Empty);
            txtOutputFileName.Text = TUserDefaults.GetStringDefault(PARTNER_IMPORT_TAX_CODE_OUTFILE, string.Empty);

            // Force the updates on the enabled states
            OnDataSourceChange(null, null);
            chkCreateExtract_CheckedChanged(null, null);
            chkCreateOutFile_CheckedChanged(null, null);

            if (rbtFromFile.Checked)
            {
                rbtFromFile.Focus();
            }
        }

        private void OnDataSourceChange(object sender, EventArgs e)
        {
            txtFileName.Enabled = rbtFromFile.Checked;
        }

        private void chkCreateExtract_CheckedChanged(object sender, EventArgs e)
        {
            bool bEnable = chkCreateExtract.Checked;

            txtExtractName.Enabled = bEnable;
            txtExtractDescription.Enabled = bEnable;
        }

        private void chkCreateOutFile_CheckedChanged(object sender, EventArgs e)
        {
            chkIncludePartnerDetails.Enabled = chkCreateOutFile.Checked;
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            Close();
        }

        private bool ValidateInputs()
        {
            if (rbtFromClipboard.Checked)
            {
                // Any text on the clipboard
                string s = Clipboard.GetText(TextDataFormat.UnicodeText);

                if ((s == null) || (s.Length == 0))
                {
                    MessageBox.Show(Catalog.GetString(
                            "There is no text on the clipboard!"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                // Do we have a valid file?
                if ((txtFileName.Text.Length == 0) || (File.Exists(txtFileName.Text) == false))
                {
                    MessageBox.Show(Catalog.GetString("The input file does not exist!"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            if (nudPartnerKeyColumn.Value == nudTaxCodeColumn.Value)
            {
                MessageBox.Show(Catalog.GetString("The Partner Key and Tax Code column numbers cannot be the same!"),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (chkCreateOutFile.Checked)
            {
                // Do we have an output file to write to?
                if (txtOutputFileName.Text.Length == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "Please select a file for the output information."), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (Directory.Exists(Path.GetDirectoryName(txtOutputFileName.Text)) == false)
                {
                    MessageBox.Show(Catalog.GetString(
                            "The folder for the file for the output information does not exist!"), this.Text, MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            if (chkCreateExtract.Checked)
            {
                // Has the user specified an extract name?
                if (txtExtractName.Text.Trim().Length == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "Please provide a name for the extract."), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                // Is the extract name ok (at the moment!)
                if (TRemote.MPartner.Partner.WebConnectors.ExtractExists(txtExtractName.Text))
                {
                    MessageBox.Show(Catalog.GetString("The selected extract name has already been used.  Please choose another name."),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            return true;
        }

        private void btnBrowse_Clicked(Object Sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Put the filename in the text box and ensure that the FromFile button is selected
                // This will enable the text box automatically
                txtFileName.Text = dialog.FileName;
                rbtFromFile.Checked = true;
            }
        }

        private void btnOutputBrowse_Clicked(Object Sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Log Files (*.log)|*.log|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Put the filename in the text box and ensure that the chkCreateOutFile checkbox is selected
                // This will enable the text box automatically
                txtOutputFileName.Text = dialog.FileName;
                chkCreateOutFile.Checked = true;
            }
        }

        /// <summary>
        /// This is the main routine to import tax data from clipboard or file
        /// </summary>
        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            TDlgSelectCSVSeparator dialog = new TDlgSelectCSVSeparator(chkFirstRowIsHeader.Checked);

            if (rbtFromClipboard.Checked)
            {
                dialog.CSVData = Clipboard.GetText(TextDataFormat.UnicodeText);
                dialog.SelectedSeparator = "\t";
            }
            else
            {
                if (dialog.OpenCsvFile(txtFileName.Text) == false)
                {
                    MessageBox.Show(Catalog.GetString("Could not open the file you have chosen.  Maybe it is already open somewhere else."),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            // work out what the separator is...
            String impOptions = TUserDefaults.GetStringDefault("Imp Options", ";" + TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN);

            string separator = StringHelper.GetCSVSeparator(dialog.FileContent) ??
                               ((impOptions.Length > 0) ? impOptions.Substring(0, 1) : ";");
            string numberFormat = (impOptions.Length > 1) ? impOptions.Substring(1) : TDlgSelectCSVSeparator.NUMBERFORMAT_AMERICAN;

            // Now we need to convert the multi-column file/clipboard data to a simple two column list
            string twoColumnImport;

            if (ConvertInputTextToTwoColumns(dialog.FileContent, separator, Convert.ToInt16(nudPartnerKeyColumn.Value - 1),
                    Convert.ToInt16(nudTaxCodeColumn.Value - 1), out twoColumnImport) == false)
            {
                // We got an error
                return;
            }

            dialog.CSVData = twoColumnImport;

            dialog.SelectedSeparator = separator;
            dialog.DateFormat = "";         // This will make the combo box empty

            // Show the Preview dialog
            DialogResult dialogResult = dialog.ShowDialog();

            // Save the settings whether the result was OK or cancel
            TUserDefaults.SetDefault("Imp Options", dialog.SelectedSeparator + numberFormat);
            TUserDefaults.SaveChangedUserDefaults();

            if (dialogResult != DialogResult.OK)
            {
                // It was cancelled
                return;
            }

            // Set up the inputs for the call to the server to do the actual import
            string importString = dialog.FileContent;
            string selectedSeparator = dialog.SelectedSeparator;
            int emptyCodeAction = rbtFailEmptyTaxCode.Checked ? 0 : rbtSkipEmptyTaxCode.Checked ? 1 : rbtDeleteEmptyTaxCode.Checked ? 2 : -1;

            Hashtable requestParams = new Hashtable();

            requestParams.Add("Delimiter", dialog.SelectedSeparator);
            requestParams.Add("FirstRowIsHeader", chkFirstRowIsHeader.Checked);
            requestParams.Add("FailIfNotPerson", chkFailIfNotPerson.Checked);
            requestParams.Add("FailIfInvalidPartner", chkFailInvalidPartner.Checked);
            requestParams.Add("OverwriteExistingTaxCode", chkOverwriteExistingTaxCode.Checked);
            requestParams.Add("CreateExtract", chkCreateExtract.Checked);
            requestParams.Add("ExtractName", txtExtractName.Text);
            requestParams.Add("ExtractDescription", txtExtractDescription.Text);
            requestParams.Add("CreateOutFile", chkCreateOutFile.Checked);
            requestParams.Add("EmptyTaxCode", emptyCodeAction);
            requestParams.Add("TaxCodeType", FTaxGovIdKeyName);
            // we include partner details if the user does not want a output file because we will write a sneaky one in the logs folder
            requestParams.Add("IncludePartnerDetails",
                (chkCreateOutFile.Checked && chkIncludePartnerDetails.Checked) || (chkCreateOutFile.Checked == false));

            // Get the server to parse the file and return our results
            bool success = false;
            TVerificationResultCollection errorMessages = null;
            List <string>outputLines = null;
            bool newExtractCreated = false;
            int newExtractId = -1;
            int newExtractKeyCount = -1;
            int taxCodesImported = -1;
            int taxCodesDeleted = -1;
            int taxCodeMismatchCount = -1;

            // Do the import on the server
            Thread ImportThread = new Thread(() => ImportPartnerTaxCodes(
                    requestParams, importString, out success, out errorMessages, out outputLines, out newExtractCreated,
                    out newExtractId, out newExtractKeyCount, out taxCodesImported, out taxCodesDeleted, out taxCodeMismatchCount));

            // Show the progress dialog so that the user can cancel
            using (TProgressDialog ImportDialog = new TProgressDialog(ImportThread))
            {
                ImportDialog.ShowDialog();
            }

            if (success)
            {
                // Import was successful
                string msg = Catalog.GetString("The Import was successful.  ");

                msg += string.Format(Catalog.GetPluralString("{0} tax code was imported.  ",
                        "{0} tax codes were imported.  ",
                        taxCodesImported, true), taxCodesImported);
                msg += string.Format(Catalog.GetPluralString("{0} tax code was deleted.  ",
                        "{0} tax codes were deleted.  ",
                        taxCodesDeleted, true), taxCodesDeleted);

                if (taxCodeMismatchCount > 0)
                {
                    msg +=
                        string.Format(Catalog.GetPluralString(
                                "{0} tax code was not imported because it did not match the existing code for the Partner.  ",
                                "{0} tax codes were not imported because they did not match the existing code for the Partner.  ",
                                taxCodeMismatchCount, true), taxCodeMismatchCount);
                }

                if (chkCreateOutFile.Checked)
                {
                    //msg += "  ";
                    msg += Catalog.GetString("You can see full details in the output file.");
                }

                if (chkCreateExtract.Checked)
                {
                    msg += Environment.NewLine + Environment.NewLine;

                    if (newExtractCreated)
                    {
                        msg += string.Format(Catalog.GetString("In addition an extract was created containing {0} keys."), newExtractKeyCount);
                    }
                    else
                    {
                        msg += "WARNING! The creation of a new extract failed.  Maybe the name was already in use.";
                    }
                }

                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK);
            }
            else
            {
                // Import failed
                if (errorMessages.HasCriticalErrors)
                {
                    // A failed import should contain some critical errors.
                    // Concatenate them and show them in an extended message box with scroll bar
                    string msg = Catalog.GetString("The import failed") + Environment.NewLine + Environment.NewLine;

                    for (int i = 0; i < errorMessages.Count; i++)
                    {
                        msg += string.Format("[{0}] - {1}", errorMessages[i].ResultContext, errorMessages[i].ResultText);
                        msg += Environment.NewLine;
                    }

                    msg += Catalog.GetString("No data was imported into the database.");

                    TFrmExtendedMessageBox msgBox = new TFrmExtendedMessageBox(this);
                    msgBox.ShowDialog(msg, this.Text, "", TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiInformation);
                }
                else
                {
                    // Should not end up wit a failed import and no error messages
                    MessageBox.Show("Import failed", this.Text, MessageBoxButtons.OK);
                }
            }

            string pathToOutFile = null;

            if (chkCreateOutFile.Checked)
            {
                pathToOutFile = txtOutputFileName.Text;
            }
            else
            {
                // we try and write a log file anyway in the log folder
                string logPath = TAppSettingsManager.GetValue("OpenPetra.PathLog", "");

                if (logPath.Length > 0)
                {
                    pathToOutFile = logPath + Path.DirectorySeparatorChar + "ImportPartnerTaxCodes.log";
                }
            }

            if (pathToOutFile != null)
            {
                // Write the output file
                using (StreamWriter sw = new StreamWriter(pathToOutFile))
                {
                    for (int i = 0; i < outputLines.Count; i++)
                    {
                        sw.WriteLine(outputLines[i]);
                    }

                    sw.Close();
                }
            }

            if (chkCreateExtract.Checked)
            {
                // Tell the client about the new extract
                // refresh extract master screen if it is open
                TFormsMessage BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcExtractCreated);

                BroadcastMessage.SetMessageDataName(txtExtractName.Text);
                TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);
            }

            // Save the GUI settings
            SaveGUISettings();
        }

        /// <summary>
        /// This method runs in a separate thread to keep the progress dialog responsive
        /// </summary>
        private void ImportPartnerTaxCodes(Hashtable ARequestParams,
            string AImportString,
            out bool ASuccess,
            out TVerificationResultCollection AErrorMessages,
            out List <string>AOutputLines,
            out bool AExtractCreated,
            out int ANewExtractId,
            out int AExtractKeyCount,
            out int ATaxCodesImportedCount,
            out int ATaxCodesDeletedCount,
            out int ATaxCodeMismatchCount)
        {
            ASuccess = TRemote.MPartner.ImportExport.WebConnectors.ImportPartnerTaxCodes(ARequestParams, AImportString, out AErrorMessages,
                out AOutputLines, out AExtractCreated, out ANewExtractId, out AExtractKeyCount, out ATaxCodesImportedCount,
                out ATaxCodesDeletedCount, out ATaxCodeMismatchCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AImportText">The original text from the file/clipboard</param>
        /// <param name="ADelimiter">The delimiter</param>
        /// <param name="APartnerKeyColumn">Zero-based index of the partner key column</param>
        /// <param name="ATaxCodeColumn">Zero-based index of the tax code column</param>
        /// <param name="ATwoColumnText">The resulting two column text</param>
        /// <returns>True if the conversion was successful.  False if an error such as 'not enough columns' occurred</returns>
        private bool ConvertInputTextToTwoColumns(string AImportText,
            string ADelimiter,
            short APartnerKeyColumn,
            short ATaxCodeColumn,
            out string ATwoColumnText)
        {
            StringBuilder sb = new StringBuilder();
            StringReader sr = new StringReader(AImportText);

            int rowNumber = 0;

            ATwoColumnText = string.Empty;

            string importLine = sr.ReadLine();

            while (importLine != null)
            {
                rowNumber++;

                // skip empty lines and commented lines
                if ((importLine.Trim().Length == 0) || importLine.StartsWith("/*") || importLine.StartsWith("#"))
                {
                    sb.AppendLine(importLine);
                    importLine = sr.ReadLine();
                    continue;
                }

                int numberOfElements = StringHelper.GetCSVList(importLine, ADelimiter).Count;

                if ((numberOfElements < APartnerKeyColumn) || (numberOfElements < ATaxCodeColumn))
                {
                    MessageBox.Show(string.Format(Catalog.GetString("Not enough columns in row {0}"), rowNumber),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                // Now we need to convert this line
                string copyLine = importLine;
                string[] separators = new string[] {
                    ADelimiter
                };

                for (int i = 0; i < APartnerKeyColumn; i++)
                {
                    StringHelper.GetNextCSV(ref copyLine, separators);
                }

                // This is the column we need
                string partnerKey = StringHelper.GetNextCSV(ref copyLine, separators);

                // Get the original line back and start again from the beginning
                copyLine = importLine;

                for (int i = 0; i < ATaxCodeColumn; i++)
                {
                    StringHelper.GetNextCSV(ref copyLine, separators);
                }

                string taxCode = StringHelper.GetNextCSV(ref copyLine, separators);

                // Create a line containing two columns separated by our chosen delimiter
                sb.AppendLine(string.Format("{0}{1}{2}", partnerKey, ADelimiter, taxCode));

                // Read the next line and go round the loop again
                importLine = sr.ReadLine();
            }

            ATwoColumnText = sb.ToString();
            return true;
        }

        private void SaveGUISettings()
        {
            string options = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                rbtFromFile.Checked ? 1 : 0,
                (int)nudPartnerKeyColumn.Value,
                (int)nudTaxCodeColumn.Value,
                chkFirstRowIsHeader.Checked ? 1 : 0,
                rbtSkipEmptyTaxCode.Checked ? 1 : rbtDeleteEmptyTaxCode.Checked ? 2 : 0,
                chkFailIfNotPerson.Checked ? 1 : 0,
                chkFailInvalidPartner.Checked ? 1 : 0,
                chkOverwriteExistingTaxCode.Checked ? 1 : 0,
                chkCreateExtract.Checked ? 1 : 0,
                chkCreateOutFile.Checked ? 1 : 0,
                chkIncludePartnerDetails.Checked ? 1 : 0);

            TUserDefaults.SetDefault(PARTNER_IMPORT_TAX_CODE_OPTIONS, options);

            if (txtFileName.Text.Length > 0)
            {
                TUserDefaults.SetDefault(PARTNER_IMPORT_TAX_CODE_INFILE, txtFileName.Text);
            }

            if (txtOutputFileName.Text.Length > 0)
            {
                TUserDefaults.SetDefault(PARTNER_IMPORT_TAX_CODE_OUTFILE, txtOutputFileName.Text);
            }
        }
    }
}