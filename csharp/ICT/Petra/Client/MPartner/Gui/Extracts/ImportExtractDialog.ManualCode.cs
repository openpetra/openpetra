//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;


namespace Ict.Petra.Client.MPartner.Gui.Extracts
{
    /// manual methods for the generated window
    public partial class TFrmImportExtractDialog
    {
        private void InitializeManualCode()
        {
            btnBrowse.Height = txtFileName.Height;

            // Set default values
            rbtClipboard.Checked = true;
            chkExcludeInactive.Checked = true;
            chkExcludeNonMailing.Checked = true;

            this.Resize += TFrmImportExtractDialog_Resize;
        }

        private void RunOnceOnActivationManual()
        {
            TFrmImportExtractDialog_Resize(null, null);
        }

        private void TFrmImportExtractDialog_Resize(object sender, EventArgs e)
        {
            // Keep the browse button in its right place
            btnBrowse.Left = txtFileName.Right + 8;
        }

        private void SelectFile(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import Extract from File");
            dialog.Filter = Catalog.GetString("Extract files (*.csv; *.txt)|*.csv;*.txt|All Files (*.*)|*.*");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = dialog.FileName;
                rbtFile.Checked = true;
            }
        }

        private void ShowHelp(object sender, EventArgs e)
        {
            string msg = Catalog.GetString(
                "This screen will create an extract from a list of partner keys.  The list can come from a file or by copying data from a spreadsheet to the clipboard.");

            msg += Environment.NewLine + Environment.NewLine;
            msg += Catalog.GetString(
                "In either case the partner keys must be in the first 'column'.  Additional text or data in other columns will be ignored.  Partner keys are numbers with a maximum of 10 digits.");

            MessageBox.Show(msg, this.Text + Catalog.GetString(" Help"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// This is the main routine that does the work
        /// </summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            // Enforce some validation rules
            if (txtExtractName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Please enter a name for the extract."),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TRemote.MPartner.Partner.WebConnectors.ExtractExists(txtExtractName.Text))
            {
                MessageBox.Show(Catalog.GetString("An extract with this name already exists. Please enter a new name."),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Import the text
            string importString = null;

            if (rbtClipboard.Checked)
            {
                // Import from the clipboard
                importString = Clipboard.GetText(TextDataFormat.UnicodeText);

                if ((importString == null) || (importString.Length == 0))
                {
                    MessageBox.Show(Catalog.GetString("Please first copy data from your spreadsheet application!"),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                // Import from the specified file
                string pathToFile = txtFileName.Text;

                if (!File.Exists(pathToFile))
                {
                    MessageBox.Show(Catalog.GetString(
                            "Cannot find the file to import from."), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string extension = Path.GetExtension(pathToFile);

                if (".txt.csv.".Contains(extension + ".") == false)
                {
                    MessageBox.Show(Catalog.GetString("You must choose either a text or CSV file with a file extension of .txt or .csv"),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                System.Text.Encoding FileEncoding = TTextFile.GetFileEncoding(pathToFile);

                //
                // If it failed to open the file, GetFileEncoding returned null.
                if (FileEncoding == null)
                {
                    MessageBox.Show(Catalog.GetString("Could not open the file."),
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                StreamReader reader = new StreamReader(pathToFile, FileEncoding, false);

                while (!reader.EndOfStream)
                {
                    importString = reader.ReadToEnd();
                }

                reader.Close();

                if (importString.Length == 0)
                {
                    MessageBox.Show(Catalog.GetString("The file was empty!"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // So we know we have some text rather than an empty string
            // The partner keys must be the first column of a row
            List <Int64>partnerKeyList = new List <Int64>();
            string[] rows = importString.Split('\n');

            for (int i = 0; i < rows.Length; i++)
            {
                string row = rows[i].TrimEnd('\r');

                if ((row.Length == 0) || row.StartsWith("#") || row.StartsWith("/*") || row.StartsWith(";"))
                {
                    // Empty line or a comment
                    continue;
                }

                // The row has some length.  Columns can be separated by tab, comma or semicolon
                // Partner key must be in the first column
                string[] cols = row.Split(new char[] { '\t', ',', ';' });
                Int64 partnerKey;

                // See if it is a number (remove any quotes around the string first)
                if (Int64.TryParse(cols[0].Replace("\"", ""), out partnerKey) && (partnerKey != 0))
                {
                    partnerKeyList.Add(partnerKey);
                }
            }

            if (MessageBox.Show(string.Format(Catalog.GetString("Found {0} partners to import.  Continue?"),
                        partnerKeyList.Count), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // Convert our list to a datatable
            DataTable table = new DataTable();
            table.Columns.Add("Key", typeof(System.Int64));

            foreach (Int64 item in partnerKeyList)
            {
                DataRow dr = table.NewRow();
                dr[0] = item;
                table.Rows.Add(dr);
            }

            // Call the main method.  If it succeeds we can close
            bool bSuccess = false;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                bSuccess = TPartnerExtractsMain.CreateNewExtractFromPartnerKeys(txtExtractName.Text, txtExtractDescription.Text, table, 0,
                    chkExcludeInactive.Checked, chkExcludeNonMailing.Checked, chkExcludeNoSolicitations.Checked, this);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (bSuccess)
            {
                Close();
            }
        }
    }
}