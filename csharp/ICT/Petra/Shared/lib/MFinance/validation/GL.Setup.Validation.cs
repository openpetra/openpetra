//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

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
        /// <param name="AMinDateTime">The earliest allowable date.</param>
        /// <param name="AMaxDateTime">The latest allowable date.</param>
        /// <param name="AIgnoreZeroRateCheck">If true a zero rate will be allowed.  This will be the case when the Daily Exchange Rate screen is modal.</param>
        /// <param name="ALedgerTableRef">A ledger table containg the available ledgers and their base currencies</param>
        /// <param name="AEarliestAccountingPeriodStartDate">The earliest accounting period start date in all the active ledgers</param>
        /// <param name="ALatestAccountingPeriodEndDate">The latest accounting period end date in all the active ledgers</param>
        public static void ValidateDailyExchangeRate(object AContext,
            ADailyExchangeRateRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection,
            DateTime AMinDateTime,
            DateTime AMaxDateTime,
            bool AIgnoreZeroRateCheck,
            ALedgerTable ALedgerTableRef,
            DateTime AEarliestAccountingPeriodStartDate,
            DateTime ALatestAccountingPeriodEndDate)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // RateOfExchange must be positive (definitely not zero unless in modal mode)
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnRateOfExchangeId];

            if (true)
            {
                if (AIgnoreZeroRateCheck)
                {
                    VerificationResult = TNumericalChecks.IsPositiveOrZeroDecimal(ARow.RateOfExchange,
                        String.Empty,
                        AContext, ValidationColumn);
                }
                else
                {
                    VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.RateOfExchange,
                        String.Empty,
                        AContext, ValidationColumn);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // Date must not be empty and must be in range
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnDateEffectiveFromId];

            if (true)
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.DateEffectiveFrom,
                    String.Empty, AVerificationResultCollection, true,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);

                if (VerificationResult == null)
                {
                    if ((AMinDateTime > DateTime.MinValue) && (AMaxDateTime < DateTime.MaxValue))
                    {
                        // Check that the date is in range
                        VerificationResult = TDateChecks.IsDateBetweenDates(ARow.DateEffectiveFrom,
                            AMinDateTime,
                            AMaxDateTime,
                            String.Empty,
                            TDateBetweenDatesCheckType.dbdctUnspecific,
                            TDateBetweenDatesCheckType.dbdctUnspecific,
                            AContext,
                            ValidationColumn);
                    }
                    else if (AMaxDateTime < DateTime.MaxValue)
                    {
                        VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate(ARow.DateEffectiveFrom, AMaxDateTime,
                            String.Empty, Ict.Common.StringHelper.DateToLocalizedString(AMaxDateTime),
                            AContext, ValidationColumn);

                        if ((VerificationResult == null) && (ARow.RowState == DataRowState.Added))
                        {
                            // even without a specific minimum date it should not be too far back
                            if (ARow.DateEffectiveFrom < AEarliestAccountingPeriodStartDate)
                            {
                                VerificationResult = new TScreenVerificationResult(AContext, ValidationColumn,
                                    Catalog.GetString(
                                        "The date is before the start of the earliest current accounting period of any active ledger."),
                                    TResultSeverity.Resv_Noncritical);
                            }
                        }
                    }
                    else if (AMinDateTime > DateTime.MinValue)
                    {
                        VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.DateEffectiveFrom, AMinDateTime,
                            String.Empty, Ict.Common.StringHelper.DateToLocalizedString(AMinDateTime),
                            AContext, ValidationColumn);

                        if ((VerificationResult == null) && (ARow.RowState == DataRowState.Added))
                        {
                            // even without a specific maximum date it should not be too far ahead
                            if (ARow.DateEffectiveFrom > ALatestAccountingPeriodEndDate)
                            {
                                VerificationResult = new TScreenVerificationResult(AContext, ValidationColumn,
                                    Catalog.GetString(
                                        "The date is after the end of the latest forwarding period of any active ledger."),
                                    TResultSeverity.Resv_Noncritical);
                            }
                        }
                    }
                    else if ((AMinDateTime == DateTime.MinValue) && (AMaxDateTime == DateTime.MaxValue)
                             && (ARow.RowState == DataRowState.Added))
                    {
                        // even without a specific maximum date it should not be too far ahead
                        if (ARow.DateEffectiveFrom > ALatestAccountingPeriodEndDate)
                        {
                            VerificationResult = new TScreenVerificationResult(AContext, ValidationColumn,
                                Catalog.GetString(
                                    "The date is after the end of the latest forwarding period of any active ledger."),
                                TResultSeverity.Resv_Noncritical);
                        }
                        // even without a specific minimum date it should not be too far back
                        else if (ARow.DateEffectiveFrom < AEarliestAccountingPeriodStartDate)
                        {
                            VerificationResult = new TScreenVerificationResult(AContext, ValidationColumn,
                                Catalog.GetString(
                                    "The date is before the start of the earliest current accounting period of any active ledger."),
                                TResultSeverity.Resv_Noncritical);
                        }
                    }

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }

            // Time must not be negative (indicating an error)
            ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnTimeEffectiveFromId];

            if (true)
            {
                VerificationResult = TTimeChecks.IsValidIntegerTime(ARow.TimeEffectiveFrom,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            if (true)
            {
                // These tests are for the GUI only
                ValidationColumn = ARow.Table.Columns[ADailyExchangeRateTable.ColumnToCurrencyCodeId];

                if (true)
                {
                    // One of the currencies should be the base currency of one of the ledgers
                    if (ARow.RowState == DataRowState.Added)
                    {
                        // Only do this test if the To Currency ComboBox is enabled
                        TScreenVerificationResult vr = null;
                        DataView fromView = new DataView(ALedgerTableRef, String.Format("{0}='{1}'",
                                ALedgerTable.GetBaseCurrencyDBName(), ARow.FromCurrencyCode), String.Empty, DataViewRowState.CurrentRows);

                        if (fromView.Count == 0)
                        {
                            DataView toView = new DataView(ALedgerTableRef, String.Format("{0}='{1}'",
                                    ALedgerTable.GetBaseCurrencyDBName(), ARow.ToCurrencyCode), String.Empty, DataViewRowState.CurrentRows);

                            if (toView.Count == 0)
                            {
                                vr = new TScreenVerificationResult(AContext, ValidationColumn,
                                    "One of the currencies should normally be a base currency for one of the Ledgers",
                                    TResultSeverity.Resv_Noncritical);
                            }
                        }

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, vr);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the Corporate Exchange Rates screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="ALedgerTableRef">A reference to a ledger table that has contains the ledgers that a client has access to</param>
        /// <param name="AAlternativeFirstDayOfPeriod">An alternative day (apart from 1) that is the start of an accounting period
        /// for at least one of the availbale ledgers</param>
        public static void ValidateCorporateExchangeRate(object AContext, ACorporateExchangeRateRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection,
            ALedgerTable ALedgerTableRef, int AAlternativeFirstDayOfPeriod)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // RateOfExchange must be positive (definitely not zero because we can invert it)
            ValidationColumn = ARow.Table.Columns[ACorporateExchangeRateTable.ColumnRateOfExchangeId];

            if (true)
            {
                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.RateOfExchange,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // Date must not be empty
            ValidationColumn = ARow.Table.Columns[ACorporateExchangeRateTable.ColumnDateEffectiveFromId];

            if (true)
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.DateEffectiveFrom,
                    String.Empty,
                    true, AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // Date must be first of month or first day in accounting period of a ledger
            ValidationColumn = ARow.Table.Columns[ACorporateExchangeRateTable.ColumnDateEffectiveFromId];

            if (true)
            {
                VerificationResult = null;

                if (AAlternativeFirstDayOfPeriod != 0)
                {
                    // day must be either 1 or AAlternativeFirstDayOfPeriod
                    VerificationResult = TDateChecks.IsNotCorporateDateTime(ARow.DateEffectiveFrom,
                        String.Empty,
                        AContext, ValidationColumn, AAlternativeFirstDayOfPeriod);
                }
                else
                {
                    // when the value is 0 we cannot do validation because there are too many alternatives!
                    // How complicated is this set of ledgers???
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            if (true)
            {
                // These tests are for the GUI only
                ValidationColumn = ARow.Table.Columns[ACorporateExchangeRateTable.ColumnToCurrencyCodeId];

                if (true)
                {
                    // One of the currencies should be the base currency of one of the ledgers
                    if ((ARow.RowState == DataRowState.Added) && (ALedgerTableRef != null))
                    {
                        // Only do this test on new rows
                        TScreenVerificationResult vr = null;
                        DataView fromView = new DataView(ALedgerTableRef, String.Format("{0}='{1}'",
                                ALedgerTable.GetBaseCurrencyDBName(), ARow.FromCurrencyCode), String.Empty, DataViewRowState.CurrentRows);

                        if (fromView.Count == 0)
                        {
                            DataView toView = new DataView(ALedgerTableRef, String.Format("{0}='{1}'",
                                    ALedgerTable.GetBaseCurrencyDBName(), ARow.ToCurrencyCode), String.Empty, DataViewRowState.CurrentRows);

                            if (toView.Count == 0)
                            {
                                vr = new TScreenVerificationResult(AContext, ValidationColumn,
                                    "One of the currencies should normally be a base currency for one of the Ledgers",
                                    TResultSeverity.Resv_Noncritical);
                            }
                        }

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, vr);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        public static void ValidateAdminGrantPayable(object AContext, AFeesPayableRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
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

                if (true)
                {
                    decimal enteredValue = (ARow.IsChargePercentageNull() ? -1 : ARow.ChargePercentage); // If the user has cleared the value in the control, I'll treat it as -1.

                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(enteredValue,
                        String.Empty,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }
            else // the ChargeOption relates to an amount
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesPayableTable.ColumnChargeAmountId];

                if (true)
                {
                    decimal enteredValue = (ARow.IsChargeAmountNull() ? -1 : ARow.ChargeAmount); // If the user has cleared the value in the control, I'll treat it as -1.

                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(enteredValue,
                        String.Empty,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        public static void ValidateAdminGrantReceivable(object AContext, AFeesReceivableRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
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

                if (true)
                {
                    decimal enteredValue = (ARow.IsChargePercentageNull() ? -1 : ARow.ChargePercentage); // If the user has cleared the value in the control, I'll treat it as -1.

                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(enteredValue,
                        String.Empty,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }
            else // the ChargeOption relates to an amount
            {
                DataColumn ValidationColumn = ARow.Table.Columns[AFeesReceivableTable.ColumnChargeAmountId];

                if (true)
                {
                    decimal enteredValue = (ARow.IsChargeAmountNull() ? -1 : ARow.ChargeAmount); // If the user has cleared the value in the control, I'll treat it as -1.

                    TVerificationResult VerificationResult = TNumericalChecks.IsPositiveDecimal(enteredValue,
                        String.Empty,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        public static void ValidateAccountingPeriod(object AContext, AAccountingPeriodRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // 'Period End Date' must be later than 'Period Start Date'
            ValidationColumn = ARow.Table.Columns[AAccountingPeriodTable.ColumnPeriodEndDateId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.PeriodEndDate, ARow.PeriodStartDate,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }
    }
}
