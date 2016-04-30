//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Petra.Client.App.Gui;


namespace Ict.Petra.Client.MPartner.Gui
{
    /// Manual methods for the generated window
    public partial class TFrmEmergencyContactsDialog
    {
        private Int64 FPrimaryContactKey = 0;
        private Int64 FSecondaryContactKey = 0;

        /// <summary>
        /// Set parameters before opening the screen
        /// </summary>
        /// <param name="ATitleText">The title text for the dialog screen - should include the contact name that this screen is about</param>
        /// <param name="APrimaryText">The text for the primary emergency contact</param>
        /// <param name="ASecondaryText">The text for the secondary emergency contact</param>
        /// <param name="APrimaryContactKey">The partner key for the primary contact</param>
        /// <param name="ASecondaryContactKey">The partner key for the secondary contact</param>
        public void SetParameters(string ATitleText, string APrimaryText, string ASecondaryText, Int64 APrimaryContactKey, Int64 ASecondaryContactKey)
        {
            this.Text = ATitleText;
            txtContact1.Text = APrimaryText;
            txtContact2.Text = ASecondaryText;

            FPrimaryContactKey = APrimaryContactKey;
            FSecondaryContactKey = ASecondaryContactKey;

            btnView1.Enabled = FPrimaryContactKey != 0;
            btnView2.Enabled = FSecondaryContactKey != 0;
        }

        private void InitializeManualCode()
        {
            // Position the two buttons
            btnView1.Left = txtContact1.Left;
            btnView2.Left = txtContact2.Left;

            this.Resize += TFrmEmergencyContactsDialog_Resize;
        }

        private void TFrmEmergencyContactsDialog_Resize(object sender, EventArgs e)
        {
            // Don't go smaller than 500
            if (Width < 500)
            {
                Width = 500;
            }

            // Resize the text boxes so that they stay centralised
            int halfWidth = (Width / 2) - (2 * txtContact1.Left);
            txtContact2.Left = halfWidth + txtContact1.Left;
            txtContact1.Width = halfWidth - (2 * txtContact1.Left);
            txtContact2.Width = txtContact1.Width;

            btnView2.Left = txtContact2.Left;
        }

        private void ViewPrimaryContact(Object Sender, EventArgs e)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, FPrimaryContactKey);
            frm.Show();
        }

        private void ViewSecondaryContact(Object Sender, EventArgs e)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, FSecondaryContactKey);
            frm.Show();
        }
    }
}