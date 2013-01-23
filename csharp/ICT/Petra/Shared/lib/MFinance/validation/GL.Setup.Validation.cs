//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Windows.Forms;

using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains functions for the validation of MFinance GL DataTables.
    /// </summary>
    public static partial class TSharedFinanceValidation_GLSetup
    {
        /// <summary>
        /// Validates the Daily Exchange Rates screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AMinDateTime">The earliest allowable date.</param>
        /// <param name="AMaxDateTime">The latest allowable date.</param>
        public static void ValidateDailyExchangeRate(object AContext, ADailyExchangeRateRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict, DateTime AMinDateTime, DateTime AMaxDateTime)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // RateOfExchange must be positive (definitely not zero because we can invert it)
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnRateOfExchangeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.RateOfExchange,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // Date must not be empty and must be in range
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnDateEffectiveFromId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.DateEffectiveFrom,
                    ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, true,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);

                if (VerificationResult == null)
                {
                    if (AMinDateTime > DateTime.MinValue && AMaxDateTime < DateTime.MaxValue)
                    {
                        // Check that the date is in range
                        VerificationResult = TDateChecks.IsDateBetweenDates(ARow.DateEffectiveFrom, AMinDateTime, AMaxDateTime,
                            ValidationControlsData.ValidationControlLabel, TDateBetweenDatesCheckType.dbdctUnspecific, TDateBetweenDatesCheckType.dbdctUnspecific,
                            AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                    else if (AMaxDateTime < DateTime.MaxValue)
                    {
                        VerificationResult = TDateChecks.FirstLesserThanSecondDate(ARow.DateEffectiveFrom, AMaxDateTime, 
                            ValidationControlsData.ValidationControlLabel, Ict.Common.StringHelper.DateToLocalizedString(AMaxDateTime),
                            AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                    else if (AMinDateTime > DateTime.MinValue)
                    {
                        VerificationResult = TDateChecks.FirstGreaterThanSecondDate(ARow.DateEffectiveFrom, AMinDateTime,
                            ValidationControlsData.ValidationControlLabel, Ict.Common.StringHelper.DateToLocalizedString(AMinDateTime),
                            AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
            }

            // Time must not be negative (indicating an error)
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnTimeEffectiveFromId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TTimeChecks.IsValidIntegerTime(ARow.TimeEffectiveFrom,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Corporate Exchange Rates screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateCorporateExchangeRate(object AContext, ACorporateExchangeRateRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // RateOfExchange must be positive (definitely not zero because we can invert it)
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnRateOfExchangeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.RateOfExchange,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // Date must not be empty
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnDateEffectiveFromId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.DateEffectiveFrom,
                    ValidationControlsData.ValidationControlLabel,
                    true, AContext, ValidationColumn, ValidationControlsData.ValidationControl);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AValidationControlsDict"></param>
        public static void ValidateAdminGrantPayable(object AContext, AFeesPayableRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            // ChargeOption = { "Minimum", "Maximum", "Fixed", "Percentage" }

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            if (ARow.ChargeOption == "Percentage")
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesPayableTable.ColumnChargePercentageId];
                TValidationControlsData ValidationControlsData;

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.ChargePercentage,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
            else // the ChargeOption relates to an amount
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesPayableTable.ColumnChargeAmountId];
                TValidationControlsData ValidationControlsData;

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.ChargeAmount,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AValidationControlsDict"></param>
        public static void ValidateAdminGrantReceivable(object AContext, AFeesReceivableRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            // ChargeOption = { "Minimum", "Maximum", "Fixed", "Percentage" }

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            if (ARow.ChargeOption == "Percentage")
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesReceivableTable.ColumnChargePercentageId];
                TValidationControlsData ValidationControlsData;

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.ChargePercentage,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
            else // the ChargeOption relates to an amount
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesReceivableTable.ColumnChargeAmountId];
                TValidationControlsData ValidationControlsData;

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.ChargeAmount,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }
    }
}