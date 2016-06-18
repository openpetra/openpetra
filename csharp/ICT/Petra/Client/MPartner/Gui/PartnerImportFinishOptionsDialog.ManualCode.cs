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


namespace Ict.Petra.Client.MPartner.Gui
{
    /// Manual methods for the generated window
    public partial class TFrmPartnerImportFinishOptionsDialog
    {
        private void InitializeManualCode()
        {
            chkIncludeImportID.Checked = true;
        }

        private void chkWriteOutputFile_CheckedChanged(object sender, EventArgs e)
        {
            chkIncludeFamilies.Enabled = chkWriteOutputFile.Checked;
            chkIncludeImportID.Enabled = chkWriteOutputFile.Checked;
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
            out bool AIncludeFamilies,
            out bool AIncludeImportIDs,
            out string AOutCSVPath,
            out bool ADoCreateExtract,
            out string AExtractName,
            out string AExtractDescription)
        {
            ADoCreateCSVFile = chkWriteOutputFile.Checked;
            AIncludeFamilies = ADoCreateCSVFile && chkIncludeFamilies.Checked;
            AIncludeImportIDs = ADoCreateCSVFile && chkIncludeImportID.Checked;
            AOutCSVPath = txtOutputFileName.Text;

            ADoCreateExtract = chkCreateExtract.Checked;
            AExtractName = ADoCreateExtract ? txtExtractName.Text : string.Empty;
            AExtractDescription = ADoCreateExtract ? txtExtractDescription.Text : string.Empty;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.InitialDirectory = Path.GetDirectoryName(txtOutputFileName.Text);
            dialog.FileName = Path.GetFileNameWithoutExtension(txtOutputFileName.Text);
            dialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            dialog.CheckPathExists = true;

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