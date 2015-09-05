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
using System.Data;
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmAddressLayoutSetup
    {
        // Simple boolean which is true if the action is Copy and false if the action is New
        private bool FCreateAsCopy;

        // An additional button that was hard to position using YAML
        private Button btnInsert = new Button();

        // A table we can use in validation
        private PAddressBlockElementTable FAddressBlockElements;

        #region Initialisation

        private void InitializeManualCode()
        {
            // Set up the appearance of the combo boxes
            cmbDetailAddressLayoutCode.AppearanceSetup(new int[] { -1, 150 }, -1);
            cmbAddressBlockElement.AppearanceSetup(new int[] { 190, 390 }, -1);
            cmbAddressBlockElement.AttachedLabel.Left += 100;

            // set up the appearance of the text box
            txtDetailAddressBlockText.HideSelection = false;

            // set up the properties of the additional button
            btnInsert.Name = "btnInsert";
            btnInsert.Text = "&Insert";
            btnInsert.Size = new System.Drawing.Size(80, 23);
            btnInsert.Left = 10;
            btnInsert.Top = txtDetailAddressBlockText.Bottom - btnInsert.Height;
            btnInsert.TabIndex = txtDetailAddressBlockText.TabIndex + 3;
            btnInsert.Click += new System.EventHandler(InsertElement);
            pnlDetails.Controls.Add(btnInsert);

            FPetraUtilsObject.SetStatusBarText(btnInsert,
                Catalog.GetString("Click to enter the selected placeholder at the cursor position in the Address Layout"));

            FAddressBlockElements = (PAddressBlockElementTable)TDataCache.GetCacheableDataTableFromCache("AddressBlockElementList");
        }

        private void RunOnceOnActivationManual()
        {
            // Set the initial index of the elements combo
            if (cmbAddressBlockElement.Count > 0)
            {
                cmbAddressBlockElement.SelectedIndex = 0;
            }

            // Modify the description for country code 99 - usually it says 'BAD COUNTRY CODE'.
            ((DataView)cmbDetailCountryCode.cmbCombobox.DataSource)[0][1] = "Default Country Code";
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Create a new record
        /// </summary>
        private void NewRecord(Object sender, EventArgs e)
        {
            FCreateAsCopy = false;
            CreateNewPAddressBlock();
        }

        /// <summary>
        /// Copy an existing record
        /// </summary>
        private void CopyRecord(Object sender, EventArgs e)
        {
            FCreateAsCopy = true;
            CreateNewPAddressBlock();
        }

        /// <summary>
        /// Insert the selected placeholder at the start of the next line
        /// </summary>
        private void InsertElementAndLine(Object sender, EventArgs e)
        {
            InsertElement(true);
        }

        /// <summary>
        /// Insert the selected placeholder at the current cursor position
        /// </summary>
        private void InsertElement(Object sender, EventArgs e)
        {
            InsertElement(false);
        }

        /// <summary>
        /// Insert the currently selected placeholder from the ComboBox into the text
        /// </summary>
        /// <param name="AStartNewLine">If true the placeholder is inserted at the start of the following line</param>
        private void InsertElement(bool AStartNewLine)
        {
            // Get the text prior to thew cursor position
            string s = txtDetailAddressBlockText.Text.Substring(0, txtDetailAddressBlockText.SelectionStart);

            if (AStartNewLine && (s.Length > 0))
            {
                // Insert placeholder at start of next line
                if (s.EndsWith("[["))
                {
                    // remove the trailing [[ from the end of the previous line
                    s = s.Substring(0, s.Length - 2);
                }

                // add a new line
                s += Environment.NewLine;
            }

            // If the text at the cursor position does not end with [[ we add it
            if (!s.EndsWith("[["))
            {
                s += "[[";
            }

            // Add the placeholder
            s += cmbAddressBlockElement.GetSelectedString();

            // Get the tail
            string sTail = txtDetailAddressBlockText.Text.Substring(
                txtDetailAddressBlockText.SelectionStart + txtDetailAddressBlockText.SelectionLength);

            // Add ]] if the tail does not start with it
            if (!sTail.StartsWith("]]"))
            {
                s += "]]";
            }

            // Remember this position - it will be the new cursor position
            int newPos = s.Length;
            s += sTail;

            // Set the new text and set the cursor position
            txtDetailAddressBlockText.Text = s;
            txtDetailAddressBlockText.SelectionStart = newPos;
        }

        #endregion

        #region Manual method extensions

        private void NewRowManual(ref PAddressBlockRow ARow)
        {
            // We don't guess at new codes - we force the user to select them from the lists
            if (FCreateAsCopy)
            {
                ARow.AddressBlockText = FPreviouslySelectedDetailRow.AddressBlockText;
                ARow.AddressLayoutCode = FPreviouslySelectedDetailRow.AddressLayoutCode;
            }
            else
            {
                ARow.AddressBlockText = String.Empty;
                ARow.AddressLayoutCode = String.Empty;
            }

            ARow.CountryCode = String.Empty;
        }

        private void ShowDetailsManual(PAddressBlockRow ARow)
        {
            btnCopy.Enabled = (ARow != null);
            btnInsert.Enabled = (cmbAddressBlockElement.Count > 0);
            btnInsertLine.Enabled = btnInsert.Enabled;
        }

        private void ValidateDataDetailsManual(PAddressBlockRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateAddressBlockSetup(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, FAddressBlockElements);
        }

        #endregion
    }
}