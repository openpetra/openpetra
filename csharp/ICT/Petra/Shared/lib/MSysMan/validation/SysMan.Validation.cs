//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2013 by OM International
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Shared.MSysMan.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPartner Partner DataTables.
    /// </summary>
    public static partial class TSharedSysManValidation
    {
        /// <summary>
        /// Validates SUser Details
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateSUserDetails(object AContext, SUserRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            ValidationColumn = ARow.Table.Columns[SUserTable.ColumnPasswordHashId];
            AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData);

            // PasswordHash must not be empty.
            if ((ARow.RowState != DataRowState.Unchanged) && string.IsNullOrEmpty(ARow.PasswordHash))
            {
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_MISSING_PASSWORD, new string[] { ARow.UserId })),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // If this is a first password (no salt) check that the password is valid.
            if ((ARow.RowState != DataRowState.Unchanged) && string.IsNullOrEmpty(ARow.PasswordSalt) && !string.IsNullOrEmpty(ARow.PasswordHash))
            {
                VerificationResult = null;

                if (!CheckPasswordQuality(ARow.PasswordHash, out VerificationResult))
                {
                    VerificationResult = new TScreenVerificationResult(VerificationResult, ValidationColumn, ValidationControlsData.ValidationControl);
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }

        /// <summary>
        /// this will do some simple checks and return false if the password is not strong enough
        /// </summary>
        /// <returns></returns>
        public static bool CheckPasswordQuality(string APassword, out TVerificationResult VerificationResult)
        {
            // at least 8 characters, at least one digit, at least one letter
            string passwordPattern = @"^.*(?=.{8,})(?=.*\d)((?=.*[a-z])|(?=.*[A-Z])).*$";
            Regex regex = new Regex(passwordPattern);

            VerificationResult = null;

            if (regex.Match(APassword).Success == false)
            {
                VerificationResult = new TVerificationResult("Password Quality Check",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALID_PASSWORD, new string[] { "8" }));

                return false;
            }

            // TODO: could do some lexical check?
            return true;
        }
    }
}