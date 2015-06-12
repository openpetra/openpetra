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
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Shared.MFinance.Validation
{
    /// <summary>
    /// Contains functions for the validation of MFinance GL DataTables.
    /// </summary>
    public static partial class TSharedFinanceValidation_GL
    {
        /// <summary>
        /// Validates the GL Batch data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AStartDateCurrentPeriod">If the caller knows this value it can be supplied. Otherwise the server will supply the value for the ledger.</param>
        /// <param name="AEndDateLastForwardingPeriod">If the caller knows this value it can be supplied. Otherwise the server will supply the value for the ledger.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGLBatchManual(object AContext, ABatchRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            DateTime? AStartDateCurrentPeriod = null, DateTime? AEndDateLastForwardingPeriod = null)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;
            object ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted or posted DataRows
            if ((ARow.RowState == DataRowState.Deleted) || (ARow.BatchStatus == MFinanceConstants.BATCH_POSTED)
                || (ARow.BatchStatus == MFinanceConstants.BATCH_CANCELLED))
            {
                return true;
            }

            bool isImporting = AContext.ToString().Contains("Importing");

            // 'Effective From Date' must be valid
            ValidationColumn = ARow.Table.Columns[ABatchTable.ColumnDateEffectiveId];
            ValidationContext = ARow.BatchNumber;

            DateTime StartDateCurrentPeriod;
            DateTime EndDateLastForwardingPeriod;

            if ((AStartDateCurrentPeriod == null) || (AEndDateLastForwardingPeriod == null))
            {
                TSharedFinanceValidationHelper.GetValidPostingDateRange(ARow.LedgerNumber,
                    out StartDateCurrentPeriod,
                    out EndDateLastForwardingPeriod);
            }
            else
            {
                StartDateCurrentPeriod = AStartDateCurrentPeriod.Value;
                EndDateLastForwardingPeriod = AEndDateLastForwardingPeriod.Value;
            }

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TDateChecks.IsDateBetweenDates(ARow.DateEffective,
                    StartDateCurrentPeriod,
                    EndDateLastForwardingPeriod,
                    ValidationControlsData.ValidationControlLabel + (isImporting ? String.Empty : " of Batch Number " + ValidationContext.ToString()),
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
        /// Validates the GL Batch Date dialog.
        /// </summary>
        /// <param name="ABatchDate">The Data being validated</param>
        /// <param name="ADescription">Description of control</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AStartDateCurrentPeriod">If the caller knows this value it can be supplied. Otherwise the server will supply the value for the ledger.</param>
        /// <param name="AEndDateLastForwardingPeriod">If the caller knows this value it can be supplied. Otherwise the server will supply the value for the ledger.</param>
        /// <param name="AControl"></param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGLBatchDateManual(DateTime? ABatchDate, string ADescription,
            ref TVerificationResultCollection AVerificationResultCollection,
            DateTime AStartDateCurrentPeriod, DateTime AEndDateLastForwardingPeriod, Control AControl)
        {
            TScreenVerificationResult VerificationResult = null;
            int VerifResultCollAddedCount = 0;

            // 'Reversal Date' must be a valid date
            TVerificationResult Result = TSharedValidationControlHelper.IsNotInvalidDate(ABatchDate,
                ADescription, AVerificationResultCollection, true,
                AControl, null, AControl);

            if (Result != null)
            {
                VerificationResult = new TScreenVerificationResult(Result, null, AControl);
            }

            // Handle addition/removal to/from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AControl, VerificationResult, null, true))
            {
                VerifResultCollAddedCount++;
            }
            else
            {
                // 'Reversal Date' must lie within the required date range
                Result = TDateChecks.IsDateBetweenDates(ABatchDate,
                    AStartDateCurrentPeriod,
                    AEndDateLastForwardingPeriod,
                    ADescription,
                    TDateBetweenDatesCheckType.dbdctUnspecific,
                    TDateBetweenDatesCheckType.dbdctUnspecific,
                    AControl,
                    null,
                    AControl);

                if (Result != null)
                {
                    VerificationResult = new TScreenVerificationResult(Result, null, AControl);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AControl, VerificationResult, null, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the Recurring GL Batch data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGLBatchManual(object AContext, ARecurringBatchRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            int VerifResultCollAddedCount = 0;

            //TODO

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the GL Journal data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AGLSetupDSRef">A GLSetupTDS reference with a populated ATransactionTypeTable.  A reference to this DataSet is REQUIRED when importing - optional otherwise</param>
        /// <param name="ACurrencyTableRef">A reference to the Currency table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ACorporateExchangeTableRef">Corporate exchange rate table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ABaseCurrency">Ledger base currency.  Required when importing</param>
        /// <param name="AIntlCurrency">Ledger international currency.  Required when importing</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGLJournalManual(object AContext, AJournalRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            GLSetupTDS AGLSetupDSRef = null,
            ACurrencyTable ACurrencyTableRef = null,
            ACorporateExchangeRateTable ACorporateExchangeTableRef = null,
            String ABaseCurrency = null,
            String AIntlCurrency = null)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;
            string ValidationContext;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted or posted DataRows
            if ((ARow.RowState == DataRowState.Deleted) || (ARow.JournalStatus == MFinanceConstants.BATCH_POSTED))
            {
                return true;
            }

            bool isImporting = AContext.ToString().Contains("Importing");

            // 'Exchange Rate' must be greater than 0
            ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnExchangeRateToBaseId];
            ValidationContext = ARow.JournalNumber.ToString() + " of Batch Number: " + ARow.BatchNumber.ToString();

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TNumericalChecks.IsPositiveDecimal(ARow.ExchangeRateToBase,
                    ValidationControlsData.ValidationControlLabel + (isImporting ? String.Empty : " of Journal Number: " + ValidationContext.ToString()),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }

                // Exchange rate must be 1.00 if the currency is the the base ledger currency
                if ((ABaseCurrency != null)
                    && (!ARow.IsExchangeRateToBaseNull())
                    && (ARow.TransactionCurrency == ABaseCurrency)
                    && (ARow.ExchangeRateToBase != 1.00m))
                {
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext,
                            new TVerificationResult(ValidationContext,
                                Catalog.GetString("A journal in the ledger base currency must have exchange rate of 1.00."),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Transaction currency must be valid
            bool isValidTransactionCurrency = true;

            if (isImporting && (ACurrencyTableRef != null))
            {
                ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnTransactionCurrencyId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    ACurrencyRow foundRow = (ACurrencyRow)ACurrencyTableRef.Rows.Find(ARow.TransactionCurrency);
                    isValidTransactionCurrency = (foundRow != null);

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("'{0}' is not a valid currency."), ARow.TransactionCurrency),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            if ((ACorporateExchangeTableRef != null) && isValidTransactionCurrency && (ABaseCurrency != null) && (AIntlCurrency != null)
                && !ARow.IsDateEffectiveNull() && (ABaseCurrency != AIntlCurrency))
            {
                // For ledgers where the base currency and intl currency differ there must be a corporate exchange rate to the international currency
                ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnTransactionCurrencyId];
                ValidationContext = ARow.JournalNumber.ToString() + " of Batch Number: " + ARow.BatchNumber.ToString();

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    DateTime firstOfMonth;

                    if (TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriod(ARow.LedgerNumber,
                            ARow.DateEffective, out firstOfMonth))
                    {
                        ACorporateExchangeRateRow foundRow = (ACorporateExchangeRateRow)ACorporateExchangeTableRef.Rows.Find(
                            new object[] { ABaseCurrency, AIntlCurrency, firstOfMonth });

                        if ((foundRow == null)
                            && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString(
                                            "There is no Corporate Exchange Rate defined for '{0}' to '{1}' for the month starting on '{2}'."),
                                        ABaseCurrency, AIntlCurrency,
                                        StringHelper.DateToLocalizedString(firstOfMonth)),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            // Sub-system code must exist in the transaction type table
            if (isImporting && (AGLSetupDSRef.ATransactionType != null))
            {
                ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnSubSystemCodeId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    ATransactionTypeRow foundRow = (ATransactionTypeRow)AGLSetupDSRef.ATransactionType.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.SubSystemCode, ARow.TransactionTypeCode });

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString(
                                        "The combination of Transaction Type of '{0}' and Sub-system Code of '{1}' is not valid for journals in Ledger {2}."),
                                    ARow.TransactionTypeCode, ARow.SubSystemCode, ARow.LedgerNumber),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Journal description must not be null
            if (isImporting)
            {
                ValidationColumn = ARow.Table.Columns[AJournalTable.ColumnJournalDescriptionId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if ((ARow.JournalDescription == null) || (ARow.JournalDescription.Length == 0))
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    Catalog.GetString("The journal description must not be empty."),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the recurring GL Journal data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGLJournalManual(object AContext, ARecurringJournalRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            int VerifResultCollAddedCount = 0;

            //TODO - nothing to add

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the GL Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ABatchRow">Manually added to bring over some GL Batch fields</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AControl"></param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AValidationCostCentreTable">REQUIRED for importing.  A reference to a cost centre table so that inputs can be validated.</param>
        /// <param name="AvalidationAccountTable">REQUIRED for importing.  A reference to an account table so that inputs can be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGLDetailManual(object AContext, ABatchRow ABatchRow, ATransactionRow ARow, Control AControl,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            ACostCentreTable AValidationCostCentreTable = null, AAccountTable AvalidationAccountTable = null)
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

            bool isImporting = AContext.ToString().Contains("Importing");

            // When used by the GUI TransactionAmount is not in the dictionary so had to pass the control directly
            // But when importing we do have a dictionary entry
            if (isImporting)
            {
                // 'GL amount must be non-zero and positive
                ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnTransactionAmountId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.TransactionAmount,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, AControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }
            else
            {
                if ((AControl != null) && AControl.Name.EndsWith("Amount"))
                {
                    // 'GL amount must be non-zero and positive
                    ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnTransactionAmountId];
                    ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                        ARow.TransactionNumber,
                        ARow.BatchNumber,
                        ARow.JournalNumber);

                    VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.TransactionAmount,
                        "Amount of " + ValidationContext,
                        AContext, ValidationColumn, AControl);

                    if (VerificationResult != null)
                    {
                        VerificationResult.SuppressValidationToolTip = true;
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }

                    return VerifResultCollAddedCount == 0;
                }
            }

            // 'Narrative must not be empty
            ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnNarrativeId];
            ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                ARow.TransactionNumber,
                ARow.BatchNumber,
                ARow.JournalNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Narrative,
                    (isImporting) ? ValidationControlsData.ValidationControlLabel : "Narrative of " + ValidationContext,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // 'Entered From Date' must be valid
            ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnTransactionDateId];
            ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                ARow.TransactionNumber,
                ARow.BatchNumber,
                ARow.JournalNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                DateTime StartDatePeriod;
                DateTime EndDatePeriod;
                TSharedFinanceValidationHelper.GetValidPeriodDates(ARow.LedgerNumber, ABatchRow.BatchYear, 0, ABatchRow.BatchPeriod,
                    out StartDatePeriod,
                    out EndDatePeriod);

                VerificationResult = (TScreenVerificationResult)TDateChecks.IsDateBetweenDates(ARow.TransactionDate,
                    StartDatePeriod,
                    EndDatePeriod,
                    (isImporting) ? ValidationControlsData.ValidationControlLabel : "Transaction Date for " + ValidationContext.ToString(),
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

            if ((AControl != null) && AControl.Name.EndsWith("Reference"))
            {
                // if "Reference" is mandatory then make sure it is set
                ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnReferenceId];
                ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                    ARow.TransactionNumber,
                    ARow.BatchNumber,
                    ARow.JournalNumber);

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Reference,
                        (isImporting) ? ValidationControlsData.ValidationControlLabel : "Reference of " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // 'CostCentre' must be valid
            ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnCostCentreCodeId];
            ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                ARow.TransactionNumber,
                ARow.BatchNumber,
                ARow.JournalNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((AValidationCostCentreTable != null) && !ARow.IsCostCentreCodeNull())
                {
                    // Code must exist in the cost centre table
                    ACostCentreRow foundRow = (ACostCentreRow)AValidationCostCentreTable.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.CostCentreCode });

                    if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            ValidationContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Cost centre code '{0}' does not exist."), ARow.CostCentreCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }

                    // cost centre must be a posting cost centre
                    if ((foundRow != null) && !foundRow.PostingCostCentreFlag)
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(ValidationContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Cost centre code '{0}' is not a posting cost centre."), ARow.CostCentreCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }

                    // cost centre must not be inactive
                    if ((foundRow != null) && !foundRow.CostCentreActiveFlag)
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(ValidationContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Cost centre code '{0}' is an inactive cost centre."), ARow.CostCentreCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            // 'Account code' must be valid
            ValidationColumn = ARow.Table.Columns[ATransactionTable.ColumnAccountCodeId];
            ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                ARow.TransactionNumber,
                ARow.BatchNumber,
                ARow.JournalNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((AvalidationAccountTable != null) && !ARow.IsAccountCodeNull())
                {
                    // Code must exist in the account table
                    AAccountRow foundRow = (AAccountRow)AvalidationAccountTable.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.AccountCode });

                    if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            ValidationContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Account code '{0}' does not exist."), ARow.AccountCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }

                    // Account code must be a posting Account code
                    if ((foundRow != null) && !foundRow.PostingStatus)
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(ValidationContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Account code '{0}' is not a posting account."), ARow.AccountCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }

                    // Account code must not be inactive
                    if ((foundRow != null) && !foundRow.AccountActiveFlag)
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(ValidationContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Account code '{0}' is an inactive account."), ARow.AccountCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the recurring GL Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ABatchRow">Manually added to bring over some GL Batch fields</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AControl"></param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGLDetailManual(object AContext,
            ARecurringBatchRow ABatchRow,
            ARecurringTransactionRow ARow,
            Control AControl,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict)
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

            //TransactionAmount is not in the dictionary so had to pass the control directly
            //  also needed to handle reference
            if ((AControl != null) && AControl.Name.EndsWith("Amount"))
            {
                // 'GL amount must be non-zero
                ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnTransactionAmountId];
                ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                    ARow.TransactionNumber,
                    ARow.BatchNumber,
                    ARow.JournalNumber);

                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.TransactionAmount,
                    "Amount of " + ValidationContext,
                    AContext, ValidationColumn, AControl);

                if (VerificationResult != null)
                {
                    VerificationResult.SuppressValidationToolTip = true;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }

                return VerifResultCollAddedCount == 0;
            }

            // Narrative must not be empty
            ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnNarrativeId];
            ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                ARow.TransactionNumber,
                ARow.BatchNumber,
                ARow.JournalNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Narrative,
                    "Narrative of " + ValidationContext,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            if ((AControl != null) && AControl.Name.EndsWith("Reference"))
            {
                ValidationColumn = ARow.Table.Columns[ARecurringTransactionTable.ColumnReferenceId];
                ValidationContext = String.Format("Transaction number {0} (batch:{1} journal:{2})",
                    ARow.TransactionNumber,
                    ARow.BatchNumber,
                    ARow.JournalNumber);

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Reference,
                        "Reference of " + ValidationContext,
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
        /// Validates the AllocationJournalDialog.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AAmountEnabled">True if txtDetailAmount is enabled (rather than txtDetailPercentage).</param>
        /// <param name="ATotalAmount">The total amount for the allocation.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateAllocationJournalDialog(object AContext,
            GLBatchTDSATransactionRow ARow,
            bool AAmountEnabled,
            decimal? ATotalAmount,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult = null;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            ValidationColumn = ARow.Table.Columns[GLBatchTDSATransactionTable.ColumnTransactionAmountId];

            // an individual amount cannot be great than total amount
            if (AAmountEnabled && (ARow.TransactionAmount > ATotalAmount))
            {
                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_AMOUNT_TOO_LARGE, new string[] { ARow.TransactionAmount.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
            }

            // Handle addition to/removal from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn))
            {
                VerifResultCollAddedCount++;
            }

            VerificationResult = null;
            ValidationColumn = ARow.Table.Columns[GLBatchTDSATransactionTable.ColumnPercentageId];

            // a percentage cannot be greater than 100%
            if (!AAmountEnabled && (ARow.Percentage > 100))
            {
                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PERCENTAGE_TOO_LARGE)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
            }

            // Handle addition to/removal from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn))
            {
                VerifResultCollAddedCount++;
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validates the ReallocationJournal Dialog.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AAmountEnabled">True if txtDetailAmount is enabled (rather than txtDetailPercentage).</param>
        /// <param name="ATotalAmount">The total amount for the allocation.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateReallocationJournalDialog(object AContext,
            GLBatchTDSATransactionRow ARow,
            bool AAmountEnabled,
            decimal? ATotalAmount,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult = null;
            int VerifResultCollAddedCount = 0;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return true;
            }

            ValidationColumn = ARow.Table.Columns[GLBatchTDSATransactionTable.ColumnTransactionAmountId];

            // an individual amount cannot be great than total amount
            if (AAmountEnabled && (ARow.TransactionAmount > ATotalAmount))
            {
                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_AMOUNT_TOO_LARGE, new string[] { ARow.TransactionAmount.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
            }

            // Handle addition to/removal from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn))
            {
                VerifResultCollAddedCount++;
            }

            VerificationResult = null;
            ValidationColumn = ARow.Table.Columns[GLBatchTDSATransactionTable.ColumnPercentageId];

            // a percentage cannot be greater than 100%
            if (!AAmountEnabled && (ARow.Percentage > 100))
            {
                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PERCENTAGE_TOO_LARGE)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
            }

            // Handle addition to/removal from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn))
            {
                VerifResultCollAddedCount++;
            }

            return VerifResultCollAddedCount == 0;
        }
    }
}