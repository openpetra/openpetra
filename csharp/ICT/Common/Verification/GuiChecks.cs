//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Windows.Forms;
using SourceGrid;
using Ict.Common.Controls;
using Ict.Common.Verification;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Class for GUI control verifications
    ///
    /// </summary>
    /// <remark> None of the data verifications in here must access the database
    ///   since the Client doesn't have access to the database!
    /// </remark>
    public class TGuiChecks : System.Object
    {
        /// <summary>
        /// Validates if a CheckedListBox has at least one checked item
        ///
        /// </summary>
        /// <param name="ACheckedListBox">listbox control that should be verified</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateCheckedListBox(System.Windows.Forms.CheckedListBox ACheckedListBox)
        {
            TVerificationResult ReturnValue = null;

            if (ACheckedListBox.CheckedItems.Count == 0)
            {
                ReturnValue = new TVerificationResult(ACheckedListBox.Name,
                    "At least one item needs to be checked.",
                    "Information missing",
                    "X_00??",
                    TResultSeverity.Resv_Critical);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Validates if a versatile CheckedListBox has at least one checked item
        ///
        /// </summary>
        /// <param name="AClbVersatile">versatile listbox control that should be verified</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateCheckedListBoxVersatile(TClbVersatile AClbVersatile)
        {
            TVerificationResult ReturnValue = null;

            if (AClbVersatile.CheckedItemsCount == 0)
            {
                ReturnValue = new TVerificationResult(AClbVersatile.Name,
                    "At least one item needs to be checked.",
                    "Information missing",
                    "X_00??",
                    TResultSeverity.Resv_Critical);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Validates if a Combobox Text is equal the selected item
        ///
        /// </summary>
        /// <param name="AComboBox">combobox control that should be verified</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateStringComboBox(System.Windows.Forms.ComboBox AComboBox)
        {
            TVerificationResult ReturnValue = null;

            if (AComboBox.SelectedItem == null)
            {
                return new TVerificationResult(AComboBox.Name,
                    "No item has been selected",
                    "Information missing",
                    "X_00??",
                    TResultSeverity.Resv_Critical);
            }

            if (AComboBox.FindString(AComboBox.Text) != AComboBox.SelectedIndex)
            {
                return new TVerificationResult(AComboBox.Name,
                    "You have typed in a non existing item.",
                    "Information missing",
                    "X_00??",
                    TResultSeverity.Resv_Critical);
            }

            return ReturnValue;
        }
    }
}