//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Drawing;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmOutreachOptions class
    /// </summary>
    public partial class TFrmOutreachOptions
    {
        /// <summary>Marker if the last character typed in the year field was a number</summary>
        private bool FNonNumberEntered;

        private void InitUserControlsManually()
        {
            this.txtOutreachCode.MaxLength = 2;
            this.txtYear.MaxLength = 4;

            this.txtYear.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtYearKeyDown);
            this.txtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtYearKeyPress);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // Matching Pattern of the outreach code for sql search.
            // The first two characters refer to the "prefix"
            // The 3. and 4. character refer to the year
            string MatchingPatern = "";

            switch (txtOutreachCode.Text.Length)
            {
                case 1:
                    MatchingPatern = txtOutreachCode.Text + "_";
                    break;

                case 2:
                    MatchingPatern = txtOutreachCode.Text;
                    break;

                default:
                    break;
            }

            if (txtYear.Text.Length > 0)
            {
                CompleteYearBox();
                int year = Convert.ToInt32(txtYear.Text);

                if ((year < 1900)
                    || (year > 3000))
                {
                    TVerificationResult VerificationResult = new TVerificationResult("Please insert a reasonable year or leave the year box empty.",
                        "For example correct year value is 2012",
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
                else
                {
                    if (MatchingPatern.Length == 0)
                    {
                        MatchingPatern = "__";
                    }

                    MatchingPatern = MatchingPatern + txtYear.Text.Substring(2, 2);
                }
            }

            if (MatchingPatern.Length > 0)
            {
                MatchingPatern = MatchingPatern + "%";
            }

            ACalc.AddParameter("param_matching_pattern", MatchingPatern);
        }

        /// <summary>
        /// Insert a 4 digit year in the year box if there are only 1 or 2 digits
        /// </summary>
        private void CompleteYearBox()
        {
            int Year = Convert.ToInt32(txtYear.Text);

            switch (txtYear.Text.Length)
            {
                case 1:
                    txtYear.Text = "200" + txtYear.Text;
                    break;

                case 2:

                    if (Year > 80)
                    {
                        txtYear.Text = "19" + txtYear.Text;
                    }
                    else
                    {
                        txtYear.Text = "20" + txtYear.Text;
                    }

                    break;

                default:
                    break;
            }
        }

        private void TxtYearKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            FNonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if ((e.KeyCode < Keys.D0) || (e.KeyCode > Keys.D9))
            {
                // Determine whether the keystroke is a number from the keypad.
                if ((e.KeyCode < Keys.NumPad0) || (e.KeyCode > Keys.NumPad9))
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        FNonNumberEntered = true;
                    }
                }
            }

            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                FNonNumberEntered = true;
            }
        }

        private void TxtYearKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (FNonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }
    }
}