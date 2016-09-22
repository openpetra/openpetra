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
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common.IO;


namespace Ict.Petra.Client.MPartner.Gui
{
    /// Manual methods for the generated window
    public partial class TFrmExtFilePreviewDialog
    {
        private Encoding FCurrentEncoding = null;
        private string FFileContent = null;
        private byte[] FRawBytes = null;

        /// <summary> Returns true if the preview TextBox contains examples of ambiguous text.  False if there are no examples for this file. </summary>
        public bool HasContent
        {
            get
            {
                if (FFileContent == null)
                {
                    throw new Exception("You must call 'SetParameters' before accessing this property");
                }

                // There is content that the user needs to do something about if there are some chars above 0x7F AND there is an Encoding choice
                return (txtPreview.Text.Length > 0) && cmbTextEncoding.Enabled;
            }
        }

        /// <summary>
        /// Sets the initial dialog control values
        /// </summary>
        /// <param name="AFileContent"></param>
        /// <param name="AFileEncoding"></param>
        /// <param name="AHasBOM"></param>
        /// <param name="AIsAmbiguousUTF"></param>
        /// <param name="ARawBytes"></param>
        public void SetParameters(string AFileContent, Encoding AFileEncoding, bool AHasBOM, bool AIsAmbiguousUTF, byte[] ARawBytes)
        {
            // Store local copies
            FFileContent = AFileContent;
            FCurrentEncoding = AFileEncoding;
            FRawBytes = ARawBytes;

            // Set up the Combo Box Content
            TTextFileEncoding e = new TTextFileEncoding();
            cmbTextEncoding.ValueMember = TTextFileEncoding.ColumnCodeDbName;
            cmbTextEncoding.DisplayMember = TTextFileEncoding.ColumnDescriptionDbName;
            cmbTextEncoding.DataSource = e.DefaultView;
            TTextFileEncoding.SetComboBoxProperties(cmbTextEncoding, AHasBOM, AIsAmbiguousUTF, AFileEncoding);

            // Put the examples of ambiguous content in the preview window
            txtPreview.Text = ParsedContent();
        }

        private void RunOnceOnActivationManual()
        {
            // Set the initial combo box index
            DataView dv = (DataView)cmbTextEncoding.DataSource;

            for (int i = 0; i < dv.Count; i++)
            {
                if (Convert.ToInt32(dv[i].Row[TTextFileEncoding.ColumnCodeDbName]) == FCurrentEncoding.CodePage)
                {
                    cmbTextEncoding.SelectedIndex = i;
                    break;
                }
            }

            // And activate the index changed event
            cmbTextEncoding.SelectedIndexChanged += CmbTextEncoding_SelectedIndexChanged;
        }

        /// <summary>
        /// Gets the values of the controls in the dialog
        /// </summary>
        public void GetResults(out Encoding AEncoding)
        {
            AEncoding = FCurrentEncoding;
        }

        private void CmbTextEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change to the new encoding
            FCurrentEncoding = Encoding.GetEncoding(
                Convert.ToInt32(((DataView)cmbTextEncoding.DataSource)[cmbTextEncoding.SelectedIndex].Row[TTextFileEncoding.ColumnCodeDbName]));
            FFileContent = FCurrentEncoding.GetString(FRawBytes);

            // Update the preview text
            txtPreview.Text = ParsedContent();
        }

        private string ParsedContent()
        {
            // We only want to display lines in the text that have a character above 0x7F.  We can ignore all the others.
            StringBuilder content = new StringBuilder();

            using (StringReader sr = new StringReader(FFileContent))
            {
                string aLine = sr.ReadLine();

                while (aLine != null)
                {
                    foreach (char c in aLine.ToCharArray())
                    {
                        if (c > 0x7F)
                        {
                            content.AppendLine(aLine);
                            break;
                        }
                    }

                    aLine = sr.ReadLine();
                }
            }

            return content.ToString();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // We are done!
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}