//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains functions for the validation of MFinance Gift DataTables.
    /// </summary>
    public static partial class TSharedFinanceValidation_Gift
    {
        /// <summary>
        /// Validates the Gift Batch data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGiftBatchManual(object AContext, AGiftBatchRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;
            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted or posted DataRows
            if ((ARow.RowState == DataRowState.Deleted) || (ARow.BatchStatus == MFinanceConstants.BATCH_POSTED))
            {
                return true;
            }

            // 'Exchange Rate' must be greater than 0
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnExchangeRateToBaseId];
            ValidationContext = ARow.BatchNumber;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TNumericalChecks.IsPositiveDecimal(ARow.ExchangeRateToBase,
                    ValidationControlsData.ValidationControlLabel + " of Batch Number " + ValidationContext.ToString(),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // 'Effective From Date' must be valid
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnGlEffectiveDateId];
            ValidationContext = ARow.BatchNumber;

            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;
            TSharedFinanceValidationHelper.GetValidPostingDateRange(ARow.LedgerNumber,
                out StartDateCurrentPeriod,
                out EndDateLastForwardingPeriod);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TDateChecks.IsDateBetweenDates(ARow.GlEffectiveDate,
                    StartDateCurrentPeriod,
                    EndDateLastForwardingPeriod,
                    ValidationControlsData.ValidationControlLabel + " of Batch Number " + ValidationContext.ToString(),
                    TDateBetweenDatesCheckType.dbdctUnspecific,
                    TDateBetweenDatesCheckType.dbdctUnspecific,
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the Gift Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGiftDetailManual(object AContext, AGiftDetailRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;
            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            // 'Gift amount must be non-zero
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnGiftTransactionAmountId];
            ValidationContext = String.Format("Batch Number {0} (transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsNonZeroDecimal(ARow.GiftTransactionAmount,
                    ValidationControlsData.ValidationControlLabel + " of " + ValidationContext,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Detail comments type 1 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnCommentOneTypeId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentOneNull() && (ARow.GiftCommentOne != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentOneType,
                        "Comment 1 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Detail comments type 2 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnCommentTwoTypeId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentTwoNull() && (ARow.GiftCommentTwo != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentTwoType,
                        "Comment 2 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Detail comments type 3 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnCommentThreeTypeId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentThreeNull() && (ARow.GiftCommentThree != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentThreeType,
                        "Comment 3 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validation for Gift table
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AYear"></param>
        /// <param name="APeriod"></param>
        /// <param name="AControl">Need to pass the validation control because it is not a bound control</param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AValidationControlsDict"></param>
        /// <returns></returns>
        public static bool ValidateGiftManual(object AContext, AGiftRow ARow, Int32 AYear, Int32 APeriod, Control AControl,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            //TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;
            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            // 'Entered From Date' must be valid
            ValidationColumn = ARow.Table.Columns[AGiftTable.ColumnDateEnteredId];
            ValidationContext = String.Format("Gift No.: {0}", ARow.GiftTransactionNumber);

            DateTime StartDatePeriod;
            DateTime EndDatePeriod;
            TSharedFinanceValidationHelper.GetValidPeriodDates(ARow.LedgerNumber, AYear, 0, APeriod,
                out StartDatePeriod,
                out EndDatePeriod);

            VerificationResult = (TScreenVerificationResult)TDateChecks.IsDateBetweenDates(ARow.DateEntered,
                StartDatePeriod,
                EndDatePeriod,
                "Gift Date for " + ValidationContext.ToString(),
                TDateBetweenDatesCheckType.dbdctUnspecific,
                TDateBetweenDatesCheckType.dbdctUnspecific,
                AContext,
                ValidationColumn,
                AControl);
            //ValidationControlsData.ValidationControl);

            // Handle addition/removal to/from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
            {
                VerifResultCollAddedCount++;
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the Recurring Gift Batch data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGiftBatchManual(object AContext, ARecurringGiftBatchRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
//            DataColumn ValidationColumn;
//            TValidationControlsData ValidationControlsData;
//            TVerificationResult VerificationResult;
//            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            //No checks as yet

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the Recurring Gift Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGiftDetailManual(object AContext, ARecurringGiftDetailRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;
            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            // 'Gift amount must be non-zero
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnGiftAmountId];
            ValidationContext = String.Format("Batch Number {0} (transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsNonZeroDecimal(ARow.GiftAmount,
                    ValidationControlsData.ValidationControlLabel + " of " + ValidationContext,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Detail comments type 1 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentOneTypeId];
            ValidationContext = String.Format("(recurring batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentOneNull() && (ARow.GiftCommentOne != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentOneType,
                        "Comment 1 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Detail comments type 2 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentTwoTypeId];
            ValidationContext = String.Format("(recurring batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentTwoNull() && (ARow.GiftCommentTwo != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentTwoType,
                        "Comment 2 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Detail comments type 3 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentThreeTypeId];
            ValidationContext = String.Format("(recurring batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftCommentThreeNull() && (ARow.GiftCommentThree != String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.CommentThreeType,
                        "Comment 3 type " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            return VerifResultCollAddedCount == 0;
        }
    }
}