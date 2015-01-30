//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using System.Windows.Forms;

using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Common;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains functions for the validation of MFinance Maintain Table screens.
    /// </summary>
    public static partial class TSharedFinanceValidation_Setup
    {
        /// <summary>
        /// Check that Foreign Currency Accounts are using a valid currency
        /// </summary>
        /// <param name="AContext">Context that describes what I'm validating.</param>
        /// <param name="ARow">DataRow with the the data I'm validating</param>
        /// <param name="AVerificationResultCollection">Will be filled with TVerificationResult items if data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateAccountDetailManual(object AContext, GLSetupTDSAAccountRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            TValidationControlsData ValidationControlsData;

            if (ARow.ForeignCurrencyFlag)
            {
                if (ARow.AccountType != MFinanceConstants.ACCOUNT_TYPE_ASSET && ARow.AccountType != MFinanceConstants.ACCOUNT_TYPE_LIABILITY)
                {
                    DataColumn ValidationColumn = ARow.Table.Columns[AAccountTable.ColumnAccountTypeId];

                    Control targetControl = null;

                    if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                    {
                        targetControl = ValidationControlsData.ValidationControl;
                    }

                    TScreenVerificationResult VerificationResult = new TScreenVerificationResult(
                        AContext,
                        ValidationColumn,
                        string.Format(Catalog.GetString("A foreign currency account's Account Type must be either '{0}' or '{1}'."), 
                            MFinanceConstants.ACCOUNT_TYPE_ASSET, MFinanceConstants.ACCOUNT_TYPE_LIABILITY),
                        targetControl,
                        TResultSeverity.Resv_Critical);
                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                if (!ARow.PostingStatus)
                {
                    DataColumn ValidationColumn = ARow.Table.Columns[AAccountTable.ColumnPostingStatusId];

                    Control targetControl = null;

                    if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                    {
                        targetControl = ValidationControlsData.ValidationControl;
                    }

                    TScreenVerificationResult VerificationResult = new TScreenVerificationResult(
                        AContext,
                        ValidationColumn,
                        Catalog.GetString("A foreign currency account must be a posting account; it cannot be a summary account."),
                        targetControl,
                        TResultSeverity.Resv_Critical);
                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // If this account is foreign, its currency must be assigned!
                if (ARow.ForeignCurrencyCode == "")
                {
                    DataColumn ValidationColumn = ARow.Table.Columns[AAccountTable.ColumnForeignCurrencyCodeId];

                    Control targetControl = null;

                    if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                    {
                        targetControl = ValidationControlsData.ValidationControl;
                    }

                    TScreenVerificationResult VerificationResult = new TScreenVerificationResult(
                        AContext,
                        ValidationColumn,
                        Catalog.GetString("Currency Code must be specified for foreign accounts."),
                        targetControl,
                        TResultSeverity.Resv_Critical);
                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
            else // If the Account is not foreign, I have nothing at all to say about the contents of the currency field.
            {
                AVerificationResultCollection.AddOrRemove(null, ARow.Table.Columns[AAccountTable.ColumnForeignCurrencyCodeId]);
            }
        }
    }
}