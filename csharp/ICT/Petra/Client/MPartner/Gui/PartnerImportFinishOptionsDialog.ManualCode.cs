//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.IO;


namespace Ict.Petra.Client.MPartner.Gui
{
    /// Manual methods for the generated window
    public partial class TFrmPartnerImportFinishOptionsDialog
    {
        private void InitializeManualCode()
        {
            pnlExtract.TabIndex = pnlOutputFile.TabIndex + 10;
            chkWriteOutputFile.Checked = true;
        }

        private void chkWriteOutputFile_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = chkWriteOutputFile.Checked;

            chkIncludeFamiliesInCSV.Enabled = enable;
            txtOutputFileName.Enabled = enable;
            btnBrowse.Enabled = enable;
        }

        private void chkCreateExtract_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = chkCreateExtract.Checked;

            chkIncludeFamiliesInExtract.Enabled = enable;
            txtExtractName.Enabled = enable;
            txtExtractDescription.Enabled = enable;
        }

        /// <summary>
        /// Sets the initial dialog control values
        /// </summary>
        /// <param name="AExtractName">Name for extract</param>
        /// <param name="AExtractDescription">Description of extract</param>
        /// <param name="AInputCSVPath">Path to the source CSV file</param>
        public void SetParameters(string AExtractName, string AExtractDescription, string AInputCSVPath)
        {
            txtExtractName.Text = AExtractName;
            txtExtractDescription.Text = AExtractDescription;
            txtOutputFileName.Text = AInputCSVPath;
        }

        /// <summary>
        /// Gets the values of the controls in the dialog
        /// </summary>
        public void GetResults(out bool ADoCreateCSVFile,
            out bool AIncludeFamiliesInCSV,
            out string AOutCSVPath,
            out bool ADoCreateExtract,
            out string AExtractName,
            out string AExtractDescription,
            out bool AIncludeFamiliesInExtract)
        {
            ADoCreateCSVFile = chkWriteOutputFile.Checked;
            AIncludeFamiliesInCSV = ADoCreateCSVFile && chkIncludeFamiliesInCSV.Checked;
            AOutCSVPath = txtOutputFileName.Text;

            ADoCreateExtract = chkCreateExtract.Checked;
            AExtractName = ADoCreateExtract ? txtExtractName.Text : string.Empty;
            AExtractDescription = ADoCreateExtract ? txtExtractDescription.Text : string.Empty;
            AIncludeFamiliesInExtract = ADoCreateExtract && chkIncludeFamiliesInExtract.Checked;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.InitialDirectory = Path.GetDirectoryName(txtOutputFileName.Text);
            dialog.FileName = Path.GetFileName(txtOutputFileName.Text);
            dialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.CheckPathExists = true;

            TWin7FileOpenSaveDialog.PrepareDialog(Path.GetFileName(txtOutputFileName.Text));

            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            txtOutputFileName.Text = dialog.FileName;
            chkWriteOutputFile.Checked = true;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string msgTitle = Catalog.GetString("Import Options");

            if (chkWriteOutputFile.Checked)
            {
                if (txtOutputFileName.Text.Length == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "Please choose a name for the CSV output file"), msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (chkCreateExtract.Checked)
            {
                // Validation...
                if (txtExtractName.Text.Length == 0)
                {
                    MessageBox.Show(Catalog.GetString(
                            "Please enter a name for the Extract"), msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Check the name
                if (((TFrmPartnerImport)FPetraUtilsObject.GetCallerForm()).ValidateExtractName(txtExtractName.Text) == false)
                {
                    MessageBox.Show(Catalog.GetString("The extract name has already been used.  Please choose a different name"),
                        msgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}