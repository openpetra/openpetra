//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
    /// Class for GUI control verifications.
    /// </summary>
    /// <remarks>None of the data verifications in here must access the database
    /// since the Client doesn't have access to the database!
    /// </remarks>
    public class TGuiChecks : System.Object
    {
        #region Resourcestrings

        private static readonly string StrItemNeedsToBeChecked = Catalog.GetString("At least one item needs to be checked.");
        private static readonly string StrNoItemSelected = Catalog.GetString("No item has been selected");
        private static readonly string StrNonExistingItem = Catalog.GetString("You have typed in a non-existing item.");

        #endregion


        /// <summary>
        /// Checks if a CheckedListBox has at least one checked item.
        /// </summary>
        /// <param name="ACheckedListBox">LisBbox Control that should be verified.</param>
        /// <returns>Null if if validation succeeded, otherwise a <see cref="TVerificationResult" />
        /// is returned that contains details about the problem.
        /// </returns>
        public static TVerificationResult ValidateCheckedListBox(System.Windows.Forms.CheckedListBox ACheckedListBox)
        {
            TVerificationResult ReturnValue = null;

            if (ACheckedListBox.CheckedItems.Count == 0)
            {
                ReturnValue = new TVerificationResult(ACheckedListBox.Name, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INFORMATIONMISSING, StrItemNeedsToBeChecked));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if a versatile CheckedListBox has at least one checked item.
        /// </summary>
        /// <param name="AClbVersatile">VersatileListBox Control that should be verified.</param>
        /// <returns>Null if if validation succeeded, otherwise a <see cref="TVerificationResult" />
        /// is returned that contains details about the problem.
        /// </returns>
        public static TVerificationResult ValidateCheckedListBoxVersatile(TClbVersatile AClbVersatile)
        {
            TVerificationResult ReturnValue = null;

            if (AClbVersatile.CheckedItemsCount == 0)
            {
                ReturnValue = new TVerificationResult(AClbVersatile.Name, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INFORMATIONMISSING, StrItemNeedsToBeChecked));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if a Combobox Text is equal the selected item.
        /// </summary>
        /// <param name="AComboBox">ComboBox Control that should be verified.</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateStringComboBox(System.Windows.Forms.ComboBox AComboBox)
        {
            TVerificationResult ReturnValue = null;

            if (AComboBox.SelectedItem == null)
            {
                ReturnValue = new TVerificationResult(AComboBox.Name, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INFORMATIONMISSING, StrNoItemSelected));
            }

            if (AComboBox.FindString(AComboBox.Text) != AComboBox.SelectedIndex)
            {
                ReturnValue = new TVerificationResult(AComboBox.Name, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INFORMATIONMISSING, StrNonExistingItem));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if at least one of a pair of checkboxes is checked.
        /// </summary>
        /// <param name="ACheckBox1">First CheckBox Control that should be verified.</param>
        /// <param name="ACheckBox2">Second CheckBox Control that should be verified.</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateCheckBoxPairIsChecked(System.Windows.Forms.CheckBox ACheckBox1,
            System.Windows.Forms.CheckBox ACheckBox2)
        {
            TVerificationResult ReturnValue = null;

            if (!ACheckBox1.Checked && !ACheckBox2.Checked)
            {
                ReturnValue = new TVerificationResult(ACheckBox1.Name, ErrorCodes.GetErrorInfo(
                        CommonErrorCodes.ERR_INFORMATIONMISSING, StrItemNeedsToBeChecked));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if the specified record is a duplicate of an existing one
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation occurs.</param>
        /// <param name="AConstraintExceptionOccurred">Set to True if a constraint exception occurred when saving the data or False otherwise</param>
        /// <param name="AResultText">The text that will be displayed as help that explains the data entered that is a duplicate</param>
        /// <param name="APrimaryKeyColumn">The data column that will be used to identify the error (usually the first of the primary key columns)</param>
        /// <param name="APrimaryKeyControl">The control corresponding to the Primary Key column</param>
        /// <returns>TVerificationResult Nil if validation succeeded, otherwise it contains
        /// details about the problem.
        /// </returns>
        public static TVerificationResult ValidateNonDuplicateRecord(object AContext, bool AConstraintExceptionOccurred, string AResultText,
            System.Data.DataColumn APrimaryKeyColumn, System.Windows.Forms.Control APrimaryKeyControl)
        {
            TVerificationResult ReturnValue = null;

            if (AConstraintExceptionOccurred)
            {
                // create a new screen verification result
                ReturnValue = new TScreenVerificationResult(AContext,
                    APrimaryKeyColumn,
                    AResultText,
                    CommonErrorCodes.ERR_DUPLICATE_RECORD,
                    APrimaryKeyControl,
                    TResultSeverity.Resv_Critical);
                ReturnValue.OverrideResultTextCaption(Catalog.GetString("Duplicate record"));
            }

            return ReturnValue;
        }
    }
}