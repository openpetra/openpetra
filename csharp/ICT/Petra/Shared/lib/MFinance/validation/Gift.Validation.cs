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
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Validation;

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
        /// <param name="AAccountTableRef">Account Table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ACostCentreTableRef">Cost centre table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="AAccountPropertyTableRef">Account Property Table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="AAccountingPeriodTableRef">Accounting Period Table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ACorporateExchangeTableRef">Corporate exchange rate table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ACurrencyTableRef">Currency table.  A reference to this table is REQUIRED when importing - optional otherwise</param>
        /// <param name="ABaseCurrency">Ledger base currency.  Required when importing</param>
        /// <param name="AInternationalCurrency">Ledger international currency.  Required when importing</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGiftBatchManual(object AContext,
            AGiftBatchRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict,
            AAccountTable AAccountTableRef = null,
            ACostCentreTable ACostCentreTableRef = null,
            AAccountPropertyTable AAccountPropertyTableRef = null,
            AAccountingPeriodTable AAccountingPeriodTableRef = null,
            ACorporateExchangeRateTable ACorporateExchangeTableRef = null,
            ACurrencyTable ACurrencyTableRef = null,
            string ABaseCurrency = null,
            string AInternationalCurrency = null)
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

            bool isImporting = AContext.ToString().Contains("Importing");

            // Bank Account Code must be active
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnBankAccountCodeId];
            ValidationContext = ARow.BankAccountCode;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsBankAccountCodeNull() && (AAccountTableRef != null))
                {
                    // We even need to check that the code exists!
                    AAccountRow foundRow = (AAccountRow)AAccountTableRef.Rows.Find(new object[] { ARow.LedgerNumber, ARow.BankAccountCode });

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Unknown bank account code '{0}'."), ARow.BankAccountCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }

                    // If it does exist and the account is a foreign currency account then the batch currency must match
                    if ((foundRow != null) && foundRow.ForeignCurrencyFlag)
                    {
                        if ((foundRow.ForeignCurrencyCode != ARow.CurrencyCode) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString(
                                            "The bank account code '{0}' is a foreign currency account so the currency code for the batch must be '{1}'."),
                                        ARow.BankAccountCode, foundRow.ForeignCurrencyCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }

                    // If it does exist it must be a posting account
                    if (foundRow != null)
                    {
                        if (!foundRow.PostingStatus && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString(
                                            "The bank account code '{0}' is not a posting account."),
                                        ARow.BankAccountCode),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }

                    if ((foundRow != null) && (ARow.GiftType == MFinanceConstants.GIFT_TYPE_GIFT))
                    {
                        // The account must be a bank account as defined in the AccountProperty table
                        if (AAccountPropertyTableRef != null)
                        {
                            AAccountPropertyRow foundRow2 = (AAccountPropertyRow)AAccountPropertyTableRef.Rows.Find(
                                new object[] { ARow.LedgerNumber, ARow.BankAccountCode, "BANK ACCOUNT", "true" });

                            if ((foundRow2 == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                    AContext,
                                    new TVerificationResult(ValidationContext,
                                        String.Format(Catalog.GetString(
                                                "The bank account code '{0}' must be associated with a real 'Bank Account' when the gift type is a 'Gift'."),
                                            ARow.BankAccountCode),
                                        TResultSeverity.Resv_Critical),
                                    ValidationColumn))
                            {
                                VerifResultCollAddedCount++;
                            }
                        }
                    }
                }

                VerificationResult = (TScreenVerificationResult)TStringChecks.ValidateValueIsActive(ARow.LedgerNumber,
                    AAccountTableRef,
                    ValidationContext.ToString(),
                    AAccountTable.GetAccountActiveFlagDBName(),
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if ((VerificationResult != null)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Bank Cost Centre Code validation
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnBankCostCentreId];
            ValidationContext = ARow.BankCostCentre;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsBankCostCentreNull() && (ACostCentreTableRef != null))
                {
                    // We even need to check that the code exists!
                    ACostCentreRow foundRow = (ACostCentreRow)ACostCentreTableRef.Rows.Find(new object[] { ARow.LedgerNumber, ARow.BankCostCentre });

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Unknown cost centre code '{0}'."), ARow.BankCostCentre),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }

                    // Even if the cost centre exists it must be a 'posting' cost centre
                    if (foundRow != null)
                    {
                        if (!foundRow.PostingCostCentreFlag && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("The cost centre '{0}' is not a Posting Cost Centre."), ARow.BankCostCentre),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }

                // Bank Cost Centre Code must be active
                VerificationResult = (TScreenVerificationResult)TStringChecks.ValidateValueIsActive(ARow.LedgerNumber,
                    ACostCentreTableRef,
                    ValidationContext.ToString(),
                    ACostCentreTable.GetCostCentreActiveFlagDBName(),
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if ((VerificationResult != null)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Currency Code validation
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnCurrencyCodeId];
            ValidationContext = ARow.BatchNumber;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsCurrencyCodeNull() && (ACurrencyTableRef != null))
                {
                    // Currency code must exist in the currency table
                    ACurrencyRow foundRow = (ACurrencyRow)ACurrencyTableRef.Rows.Find(ARow.CurrencyCode);

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Unknown currency code '{0}'."), ARow.CurrencyCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // 'Exchange Rate' must be greater than 0
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnExchangeRateToBaseId];
            ValidationContext = ARow.BatchNumber;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsExchangeRateToBaseNull())
                {
                    VerificationResult = (TScreenVerificationResult)TNumericalChecks.IsPositiveDecimal(ARow.ExchangeRateToBase,
                        ValidationControlsData.ValidationControlLabel +
                        (isImporting ? String.Empty : " of Batch Number " + ValidationContext.ToString()),
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }

                    // Exchange rate must be 1.00 if the currency is the the base ledger currency
                    if ((ABaseCurrency != null) && !ARow.IsCurrencyCodeNull() && (ARow.CurrencyCode == ABaseCurrency)
                        && (ARow.ExchangeRateToBase != 1.00m)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                Catalog.GetString("A batch in the ledger base currency must have exchange rate of 1.00."),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
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

                // If the GL date was good we need to have a corporate exchange rate for base currency to Intl for the first day of the period
                if ((VerificationResult == null) && (ACorporateExchangeTableRef != null) && !ARow.IsGlEffectiveDateNull())
                {
                    DateTime firstOfMonth;

                    if (TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriod(ARow.LedgerNumber, ARow.GlEffectiveDate, out firstOfMonth))
                    {
                        ACorporateExchangeRateRow foundRow = (ACorporateExchangeRateRow)ACorporateExchangeTableRef.Rows.Find(
                            new object[] { ABaseCurrency, AInternationalCurrency, firstOfMonth });

                        if ((foundRow == null)
                            && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString(
                                            "International currency: there is no Corporate Exchange Rate defined for '{0}' to '{1}' for the month starting on '{2}'."),
                                        ABaseCurrency, AInternationalCurrency,
                                        StringHelper.DateToLocalizedString(firstOfMonth)),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            // Gift Type must be one of our predefined constants
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnGiftTypeId];
            ValidationContext = ARow.BatchNumber;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsGiftTypeNull())
                {
                    // Ensure the gift type is correct and that it matches one of the allowable options (applies when importing)
                    if ((ARow.GiftType != MFinanceConstants.GIFT_TYPE_GIFT)
                        && (ARow.GiftType != MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND)
                        && (ARow.GiftType != MFinanceConstants.GIFT_TYPE_OTHER))
                    {
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Unknown gift type '{0}'. Expected one of '{1}', '{2}' or '{3}'"),
                                        ARow.GiftType,
                                        MFinanceConstants.GIFT_TYPE_GIFT, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND, MFinanceConstants.GIFT_TYPE_OTHER),
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
        /// Validates the Gift Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="ACostCentres">Optional - a CostCentres table.  Is required for import validation. </param>
        /// <param name="AMotivationGroups">Optional - a MotivationGroups table.  Is required for import validation. </param>
        /// <param name="AMotivationDetails">Optional - a MotivationDetails table.  Is required for import validation. </param>
        /// <param name="ARecipientField">Optional The recipient field for the gift.  Is required for import validation. </param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateGiftDetailManual(object AContext,
            GiftBatchTDSAGiftDetailRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict,
            ACostCentreTable ACostCentres = null,
            AMotivationGroupTable AMotivationGroups = null,
            AMotivationDetailTable AMotivationDetails = null,
            Int64 ARecipientField = -1)
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

            // Check if valid recipient
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnRecipientKeyId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}, detail no. {2}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                ARow.RecipientKey, new TPartnerClass[] { TPartnerClass.FAMILY, TPartnerClass.UNIT }, true,
                isImporting ? Catalog.GetString("Recipient key") :
                "Recipient of " + THelper.NiceValueDescription(ValidationContext.ToString()),
                ValidationContext, ValidationColumn, null);

            if (VerificationResult != null)
            {
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
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
                    ValidationControlsData.ValidationControlLabel + (isImporting ? String.Empty : " of " + ValidationContext),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Motivation Group validation
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnMotivationGroupCodeId];
            ValidationContext = String.Format("batch:{0} transaction:{1} detail:{2}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // Motivation Group type Gift must have non-zero Recipient field
                if (!ARow.IsMotivationGroupCodeNull() && (ARow.MotivationGroupCode == MFinanceConstants.MOTIVATION_GROUP_GIFT)
                    && (ARecipientField == 0))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidRecipientFieldForMotivationGroup(ARow.RecipientKey,
                        ARecipientField,
                        MFinanceConstants.MOTIVATION_GROUP_GIFT,
                        isImporting ? ValidationControlsData.ValidationControlLabel : "Recipient of " +
                        THelper.NiceValueDescription(ValidationContext.ToString()) + Environment.NewLine,
                        AContext,
                        ValidationColumn,
                        null);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }

                // Motivation Group code must exist
                if (!ARow.IsMotivationGroupCodeNull() && (AMotivationGroups != null))
                {
                    AMotivationGroupRow foundRow = (AMotivationGroupRow)AMotivationGroups.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.MotivationGroupCode });

                    if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Unknown motivation group code '{0}'."),
                                    ARow.MotivationGroupCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            if (!isImporting)
            {
                // NOTE AlanP Oct 2014.  This gets checked by standard validation so may no longer be necessary?
                //  (There was a bug in standard validation where NULL and empty string checks did not quite work as they should ...
                //   so maybe this was necessary before.  Anyway I am leaving it in for now.  I know that importing works fine,
                //   but maybe it is necessary in other circumstances?)

                // Motivation Detail must not be null
                ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnMotivationDetailCodeId];
                ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                    ARow.BatchNumber,
                    ARow.GiftTransactionNumber,
                    ARow.DetailNumber);

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if (ARow.IsMotivationDetailCodeNull() || (ARow.MotivationDetailCode == String.Empty))
                    {
                        VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.MotivationDetailCode,
                            (isImporting ? ValidationControlsData.ValidationControlLabel : "Motivation Detail code " + ValidationContext),
                            AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition/removal to/from TVerificationResultCollection
                        if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                        {
                            VerifResultCollAddedCount++;
                        }
                    }
                }
            }

            // Motivation Detail must be valid
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnMotivationDetailCodeId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (!ARow.IsMotivationDetailCodeNull() && (AMotivationDetails != null))
            {
                AMotivationDetailRow foundRow = (AMotivationDetailRow)AMotivationDetails.Rows.Find(
                    new object[] { ARow.LedgerNumber, ARow.MotivationGroupCode, ARow.MotivationDetailCode });

                if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                        AContext,
                        new TVerificationResult(ValidationContext,
                            String.Format(Catalog.GetString("Unknown motivation detail code '{0}' for group '{1}'."),
                                ARow.MotivationDetailCode, ARow.MotivationGroupCode),
                            TResultSeverity.Resv_Critical),
                        ValidationColumn))
                {
                    VerifResultCollAddedCount++;
                }

                if ((foundRow != null) && (foundRow.MotivationStatus == false) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                        AContext,
                        new TVerificationResult(ValidationContext,
                            String.Format(Catalog.GetString("Motivation detail code '{0}' is no longer in use."),
                                ARow.MotivationDetailCode),
                            TResultSeverity.Resv_Critical),
                        ValidationColumn))
                {
                    VerifResultCollAddedCount++;
                }

                if ((foundRow != null) && (foundRow.RecipientKey != 0) && (ARow.RecipientKey != 0) && (foundRow.RecipientKey != ARow.RecipientKey)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                        AContext,
                        new TVerificationResult(ValidationContext,
                            String.Format(Catalog.GetString(
                                    "The recipient partner key for motivation detail code '{0}' does not match the recipient partner key in the import line."),
                                ARow.MotivationDetailCode),
                            TResultSeverity.Resv_Critical),
                        ValidationColumn))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Cost Centre Code must exist and be active.  Only required for importing because the GUI does this for us otherwise.
            if (isImporting && (ACostCentres != null) && !ARow.IsCostCentreCodeNull())
            {
                ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnCostCentreCodeId];
                ValidationContext = ARow.CostCentreCode;

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    // We even need to check that the code exists!
                    DataRow foundRow = ACostCentres.Rows.Find(new object[] { ARow.LedgerNumber, ARow.CostCentreCode });

                    if ((foundRow == null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                            AContext,
                            new TVerificationResult(ValidationContext,
                                String.Format(Catalog.GetString("Unknown cost centre code '{0}'."), ARow.CostCentreCode),
                                TResultSeverity.Resv_Critical),
                            ValidationColumn))
                    {
                        VerifResultCollAddedCount++;
                    }

                    VerificationResult = (TScreenVerificationResult)TStringChecks.ValidateValueIsActive(ARow.LedgerNumber,
                        ACostCentres,
                        ValidationContext.ToString(),
                        ACostCentreTable.GetCostCentreActiveFlagDBName(),
                        AContext,
                        ValidationColumn,
                        ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if ((VerificationResult != null)
                        && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
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
                        (isImporting ? ValidationControlsData.ValidationControlLabel : "Comment 1 type " + ValidationContext),
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }

                    if (VerificationResult == null)
                    {
                        // There is a comment type for the comment - but it needs to be one of the valid types
                        if ((ARow.CommentOneType != MFinanceConstants.GIFT_COMMENT_TYPE_DONOR)
                            && (ARow.CommentOneType != MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT)
                            && (ARow.CommentOneType != MFinanceConstants.GIFT_COMMENT_TYPE_BOTH)
                            && (ARow.CommentOneType != MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE)
                            && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Comment type must be one of '{0}', '{1}', '{2}' or '{3}'."),
                                        MFinanceConstants.GIFT_COMMENT_TYPE_DONOR,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_BOTH,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
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
                        (isImporting ? ValidationControlsData.ValidationControlLabel : "Comment 2 type " + ValidationContext),
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }

                    if (VerificationResult == null)
                    {
                        // There is a comment type for the comment - but it needs to be one of the valid types
                        if ((ARow.CommentTwoType != MFinanceConstants.GIFT_COMMENT_TYPE_DONOR)
                            && (ARow.CommentTwoType != MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT)
                            && (ARow.CommentTwoType != MFinanceConstants.GIFT_COMMENT_TYPE_BOTH)
                            && (ARow.CommentTwoType != MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE)
                            && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Comment type must be one of '{0}', '{1}', '{2}' or '{3}'."),
                                        MFinanceConstants.GIFT_COMMENT_TYPE_DONOR,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_BOTH,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE),
                                    TResultSeverity.Resv_Critical),
                                ValidationColumn))
                        {
                            VerifResultCollAddedCount++;
                        }
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
                        (isImporting ? ValidationControlsData.ValidationControlLabel : "Comment 3 type " + ValidationContext),
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }

                    if (VerificationResult == null)
                    {
                        // There is a comment type for the comment - but it needs to be one of the valid types
                        if ((ARow.CommentThreeType != MFinanceConstants.GIFT_COMMENT_TYPE_DONOR)
                            && (ARow.CommentThreeType != MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT)
                            && (ARow.CommentThreeType != MFinanceConstants.GIFT_COMMENT_TYPE_BOTH)
                            && (ARow.CommentThreeType != MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE)
                            && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                                AContext,
                                new TVerificationResult(ValidationContext,
                                    String.Format(Catalog.GetString("Comment type must be one of '{0}', '{1}', '{2}' or '{3}'."),
                                        MFinanceConstants.GIFT_COMMENT_TYPE_DONOR,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_BOTH,
                                        MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE),
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
        /// Validates the Gift Detail data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateTaxDeductiblePct(object AContext,
            GiftBatchTDSAGiftDetailRow ARow,
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

            // Tax deductible account code must not be null or empty string if there is a percent specified
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnTaxDeductiblePctId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // it should be impossible for this to ever happen
                if (ARow.TaxDeductiblePct != 0)
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.TaxDeductibleAccountCode,
                        "Tax-Deductible Account " + ValidationContext,
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
        /// <param name="AMethodOfGivingRef">Required for import validation</param>
        /// <param name="AMethodOfPaymentRef">Required for import validation</param>
        /// <returns></returns>
        public static bool ValidateGiftManual(object AContext, AGiftRow ARow, Int32 AYear, Int32 APeriod, Control AControl,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            AMethodOfGivingTable AMethodOfGivingRef = null,
            AMethodOfPaymentTable AMethodOfPaymentRef = null)
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

            bool isImporting = AContext.ToString().Contains("Importing");

            // Check if valid donor
            ValidationColumn = ARow.Table.Columns[AGiftTable.ColumnDonorKeyId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber);

            VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                ARow.DonorKey, new TPartnerClass[] { }, true,
                (isImporting) ? String.Empty : "Donor of " + THelper.NiceValueDescription(ValidationContext.ToString()),
                ValidationContext, ValidationColumn, null);

            if (VerificationResult != null)
            {
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Entered From Date' must be valid
            // But we do not test for this when importing because the date is tested for the batch rather than the individual gift(s)
            if (!isImporting)
            {
                ValidationColumn = ARow.Table.Columns[AGiftTable.ColumnDateEnteredId];
                ValidationContext = String.Format("Gift No.: {0}", ARow.GiftTransactionNumber);

                DateTime StartDateCurrentPeriod;
                DateTime EndDateCurrentPeriod;
                TSharedFinanceValidationHelper.GetValidPeriodDates(ARow.LedgerNumber, AYear, 0, APeriod,
                    out StartDateCurrentPeriod,
                    out EndDateCurrentPeriod);

                VerificationResult = (TScreenVerificationResult)TDateChecks.IsDateBetweenDates(ARow.DateEntered,
                    StartDateCurrentPeriod,
                    EndDateCurrentPeriod,
                    (isImporting) ? String.Empty : "Gift Date for " + ValidationContext.ToString(),
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
            }

            // A method of giving must be valid
            ValidationColumn = ARow.Table.Columns[AGiftTable.ColumnMethodOfGivingCodeId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber);

            if (!ARow.IsMethodOfGivingCodeNull() && (AMethodOfGivingRef != null))
            {
                AMethodOfGivingRow foundRow = (AMethodOfGivingRow)AMethodOfGivingRef.Rows.Find(ARow.MethodOfGivingCode);

                if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                        AContext,
                        new TVerificationResult(ValidationContext,
                            String.Format(Catalog.GetString("Unknown method of giving code '{0}'."),
                                ARow.MethodOfGivingCode),
                            TResultSeverity.Resv_Critical),
                        ValidationColumn))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // A method of payment must be valid
            ValidationColumn = ARow.Table.Columns[AGiftTable.ColumnMethodOfPaymentCodeId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber);

            if (!ARow.IsMethodOfPaymentCodeNull() && (AMethodOfPaymentRef != null))
            {
                AMethodOfGivingRow foundRow = (AMethodOfGivingRow)AMethodOfPaymentRef.Rows.Find(ARow.MethodOfPaymentCode);

                if ((foundRow == null) && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(
                        AContext,
                        new TVerificationResult(ValidationContext,
                            String.Format(Catalog.GetString("Unknown method of payment code '{0}'."),
                                ARow.MethodOfPaymentCode),
                            TResultSeverity.Resv_Critical),
                        ValidationColumn))
                {
                    VerifResultCollAddedCount++;
                }
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

            // Description cannot be empty
            ValidationColumn = ARow.Table.Columns[ARecurringGiftBatchTable.ColumnBatchDescriptionId];
            ValidationContext = String.Format("Description in Recurring Batch no.: {0}", ARow.BatchNumber); //ARow.BankAccountCode;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TStringChecks.StringMustNotBeEmpty(ARow.BatchDescription,
                    ValidationContext.ToString(),
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if ((VerificationResult != null)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // A Bank Account Code must be selected
            ValidationColumn = ARow.Table.Columns[ARecurringGiftBatchTable.ColumnBankAccountCodeId];
            ValidationContext = String.Format("Bank Account in Recurring Batch no.: {0}", ARow.BatchNumber); //ARow.BankAccountCode;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TStringChecks.StringMustNotBeEmpty(ARow.BankAccountCode,
                    ValidationContext.ToString(),
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if ((VerificationResult != null)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // A Bank Cost Centre Code must be selected
            ValidationColumn = ARow.Table.Columns[ARecurringGiftBatchTable.ColumnBankCostCentreId];
            ValidationContext = String.Format("Cost Centre in Recurring Batch no.: {0}", ARow.BatchNumber); //ARow.BankCostCentre;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = (TScreenVerificationResult)TStringChecks.StringMustNotBeEmpty(ARow.BankCostCentre,
                    ValidationContext.ToString(),
                    AContext,
                    ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if ((VerificationResult != null)
                    && AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            return VerifResultCollAddedCount == 0;
        }

        /// <summary>
        /// Validation for Recurring Gift table
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="AVerificationResultCollection"></param>
        /// <param name="AValidationControlsDict"></param>
        /// <returns></returns>
        public static bool ValidateRecurringGiftManual(object AContext, ARecurringGiftRow ARow,
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

            // Check if valid donor
            ValidationColumn = ARow.Table.Columns[ARecurringGiftTable.ColumnDonorKeyId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber);

            VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                ARow.DonorKey, new TPartnerClass[] { }, true,
                "Donor of " + THelper.NiceValueDescription(ValidationContext.ToString()), ValidationContext, ValidationColumn, null);

            if (VerificationResult != null)
            {
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // Handle addition/removal to/from TVerificationResultCollection
            if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
            {
                VerifResultCollAddedCount++;
            }

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
        /// <param name="ARecipientField">Optional</param>
        /// <returns>True if the validation found no data validation errors, otherwise false.</returns>
        public static bool ValidateRecurringGiftDetailManual(object AContext,
            ARecurringGiftDetailRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection,
            TValidationControlsDict AValidationControlsDict,
            Int64 ARecipientField = -1)
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

            // Check if valid donor
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnRecipientKeyId];
            ValidationContext = String.Format("Batch no. {0}, gift no. {1}, detail no. {2}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                ARow.RecipientKey, new TPartnerClass[] { TPartnerClass.FAMILY, TPartnerClass.UNIT }, true,
                "Recipient of " + THelper.NiceValueDescription(ValidationContext.ToString()), ValidationContext, ValidationColumn, null);

            if (VerificationResult != null)
            {
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Gift amount must be non-zero
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnGiftAmountId];
            ValidationContext = String.Format("Batch Number {0} (transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.GiftAmount,
                    ValidationControlsData.ValidationControlLabel + " of " + ValidationContext,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }

            // Motivation Group type Gift must have non-zero Recipient field
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnMotivationGroupCodeId];
            ValidationContext = String.Format("batch:{0} transaction:{1} detail:{2}",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((ARow.MotivationGroupCode == MFinanceConstants.MOTIVATION_GROUP_GIFT) && (ARecipientField == 0))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidRecipientFieldForMotivationGroup(ARow.RecipientKey,
                        ARecipientField,
                        MFinanceConstants.MOTIVATION_GROUP_GIFT,
                        "Recipient of " + THelper.NiceValueDescription(ValidationContext.ToString()) + Environment.NewLine,
                        AContext,
                        ValidationColumn,
                        null);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Motivation Detail must not be null
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnMotivationDetailCodeId];
            ValidationContext = String.Format("(batch:{0} transaction:{1} detail:{2})",
                ARow.BatchNumber,
                ARow.GiftTransactionNumber,
                ARow.DetailNumber);

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.IsMotivationDetailCodeNull() || (ARow.MotivationDetailCode == String.Empty))
                {
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.MotivationDetailCode,
                        "Motivation Detail code " + ValidationContext,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition/removal to/from TVerificationResultCollection
                    if (AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                    {
                        VerifResultCollAddedCount++;
                    }
                }
            }

            // Detail comments type 1 must not be null if associated comment is not null
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentOneTypeId];
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
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentTwoTypeId];
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
            ValidationColumn = ARow.Table.Columns[ARecurringGiftDetailTable.ColumnCommentThreeTypeId];
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
        /// Validates the Gift Motivation Setup.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="ATaxDeductiblePercentageEnabled">True if Tax Deductible Percentage is enabled</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateGiftMotivationSetupManual(object AContext, AMotivationDetailRow ARow, bool ATaxDeductiblePercentageEnabled,
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

            // 'Motivation Group' must not be unassignable
            ValidationColumn = ARow.Table.Columns[AMotivationDetailTable.ColumnMotivationGroupCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                AMotivationGroupTable MotivationGroupTable;
                AMotivationGroupRow MotivationGroupPointRow;

                VerificationResult = null;

                if ((!ARow.IsMotivationGroupCodeNull())
                    && (ARow.MotivationGroupCode != String.Empty))
                {
                    MotivationGroupTable = (AMotivationGroupTable)TSharedDataCache.TMFinance.GetCacheableFinanceTable(
                        TCacheableFinanceTablesEnum.MotivationGroupList);
                    MotivationGroupPointRow = (AMotivationGroupRow)MotivationGroupTable.Rows.Find(
                        new object[] { ARow.LedgerNumber, ARow.MotivationGroupCode });

                    // 'Motivation Group' must not be unassignable
                    if ((MotivationGroupPointRow != null)
                        && !MotivationGroupPointRow.GroupStatus)
                    {
                        // if 'Motivation Group' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, AMotivationDetailTable.GetMotivationGroupCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { ValidationControlsData.ValidationControlLabel, ARow.MotivationGroupCode })),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            if (ATaxDeductiblePercentageEnabled)
            {
                // 'TaxDeductibleAccount' must have a value (NOT NULL constraint)
                ValidationColumn = ARow.Table.Columns[AMotivationDetailTable.ColumnTaxDeductibleAccountId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.TaxDeductibleAccount,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }
    }
}