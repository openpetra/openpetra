//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Shared.MPartner.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPartner Partner DataTables.
    /// </summary>
    public static partial class TSharedPartnerValidation_Partner
    {
        /// <summary>todoComment</summary>
        private static readonly string StrBICSwiftCodeInvalid = Catalog.GetString(
            "The BIC / Swift code you entered for this bank is invalid!" + "\r\n" + "\r\n" +
            "  Here is the format of a valid BIC: 'BANKCCLL' or 'BANKCCLLBBB'." + "\r\n" +
            "    BANK = Bank Code. This code identifies the bank world wide." + "\r\n" +
            "    CC   = Country Code. This is the ISO country code of the country the bank is in.\r\n" +
            "    LL   = Location Code. This code gives the town where the bank is located." + "\r\n" +
            "    BBB  = Branch Code. This code denotes the branch of the bank." + "\r\n" +
            "  BICs have either 8 or 11 characters." + "\r\n");

        /// <summary>
        /// Validates the Partner Detail data of a Partner of PartnerClass BANK.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerBankManual(object AContext, PBankRow ARow,
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

            // 'BIC' (Bank Identifier Code) must be valid
            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBicId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (CommonRoutines.CheckBIC(ARow.Bic) == false)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // For information only: 'Branch Code' format matches the format of a BIC
            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBranchCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((ARow.BranchCode != null)
                    && (ARow.BranchCode != String.Empty))
                {
                    if (CommonRoutines.CheckBIC(ARow.BranchCode) == true)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BRANCHCODELIKEBIC, String.Empty,
                                    new String[] {
                                        ValidationControlsData.ValidationControlLabel,
                                        ValidationControlsData.SecondValidationControlLabel,
                                        ValidationControlsData.ValidationControlLabel,
                                        ValidationControlsData.ValidationControlLabel
                                    },
                                    new String[] { ValidationControlsData.ValidationControlLabel })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                    else
                    {
                        VerificationResult = null;
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }
        }

        /// <summary>
        /// Validates the Partner Detail data of a Partner of PartnerClass PERSON.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="ACacheRetriever">Delegate that returns the specified DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerPersonManual(object AContext, PPersonRow ARow, TGetCacheableDataTableFromCache ACacheRetriever,
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

            // 'Date of Birth' must have a sensible value (must not be below 1850 and must not lie in the future)
            ValidationColumn = ARow.Table.Columns[PPersonTable.ColumnDateOfBirthId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsDateOfBirthNull())
                {
                    VerificationResult = TDateChecks.IsDateBetweenDates(
                        ARow.DateOfBirth, new DateTime(1850, 1, 1), DateTime.Today,
                        ValidationControlsData.ValidationControlLabel,
                        TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctNoFutureDate,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // 'Marital Status' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PPersonTable.ColumnMaritalStatusId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PtMaritalStatusTable TypeTable;
                PtMaritalStatusRow TypeRow;

                VerificationResult = null;

                if ((!ARow.IsMaritalStatusNull())
                    && (ARow.MaritalStatus != String.Empty))
                {
                    TypeTable = (PtMaritalStatusTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.MaritalStatusList);
                    TypeRow = (PtMaritalStatusRow)TypeTable.Rows.Find(ARow.MaritalStatus);

                    // 'Marital Status' must not be unassignable
                    if ((TypeRow != null)
                        && !TypeRow.AssignableFlag
                        && (TypeRow.IsAssignableDateNull()
                            || (TypeRow.AssignableDate <= DateTime.Today)))
                    {
                        // if 'Marital Status' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PPersonTable.GetMaritalStatusDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { ValidationControlsData.ValidationControlLabel, ARow.MaritalStatus })),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'MaritalStatusSince' must be valid
            ValidationColumn = ARow.Table.Columns[PPersonTable.ColumnMaritalStatusSinceId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.MaritalStatusSince,
                    ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, false,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'OccupationCode' must be valid
            ValidationColumn = ARow.Table.Columns[PPersonTable.ColumnOccupationCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!string.IsNullOrEmpty(ARow.OccupationCode))
                {
                    Type tmp;
                    DataTable CachedDT = ACacheRetriever(Enum.GetName(typeof(TCacheablePartnerTablesEnum),
                            TCacheablePartnerTablesEnum.OccupationList), out tmp);
                    DataRow FoundDR = CachedDT.Rows.Find(new object[] { ARow.OccupationCode });

                    if (FoundDR == null)
                    {
                        VerificationResult = new TVerificationResult(ValidationControlsData.ValidationControl, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_OCCUPATIONCODE_INVALID, new string[] { ARow.OccupationCode }));
                        VerificationResult = new TScreenVerificationResult(VerificationResult,
                            ValidationColumn,
                            ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the Partner Detail data of a Partner of PartnerClass FAMILY.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerFamilyManual(object AContext, PFamilyRow ARow,
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

            // 'Marital Status' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PFamilyTable.ColumnMaritalStatusId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PtMaritalStatusTable TypeTable;
                PtMaritalStatusRow TypeRow;

                VerificationResult = null;

                if ((!ARow.IsMaritalStatusNull())
                    && (ARow.MaritalStatus != String.Empty))
                {
                    TypeTable = (PtMaritalStatusTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.MaritalStatusList);
                    TypeRow = (PtMaritalStatusRow)TypeTable.Rows.Find(ARow.MaritalStatus);

                    // 'Marital Status' must not be unassignable
                    if ((TypeRow != null)
                        && !TypeRow.AssignableFlag
                        && (TypeRow.IsAssignableDateNull()
                            || (TypeRow.AssignableDate <= DateTime.Today)))
                    {
                        // if 'Marital Status' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PFamilyTable.GetMaritalStatusDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { ValidationControlsData.ValidationControlLabel, ARow.MaritalStatus })),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'MaritalStatusSince' must be valid
            ValidationColumn = ARow.Table.Columns[PFamilyTable.ColumnMaritalStatusSinceId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.MaritalStatusSince,
                    ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, false,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner Detail data of a Partner of PartnerClass CHURCH.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="ADenominationCacheableDT">The contents of the Cacheable DataTable 'DenominationList'.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerChurchManual(object AContext, PChurchRow ARow, DataTable ADenominationCacheableDT,
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

            // Special check: 'Denominations' must exist and must not be unassignable!
            ValidationColumn = ARow.Table.Columns[PChurchTable.ColumnDenominationCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ADenominationCacheableDT != null)
                {
                    if (ADenominationCacheableDT.Rows.Count == 0)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NO_DENOMINATIONS_SET_UP,
                                    String.Empty)),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);

                // 'Denomination' must be valid
                PDenominationTable DenominationTable;
                PDenominationRow DenominationRow = null;

                VerificationResult = null;

                if (!ARow.IsDenominationCodeNull())
                {
                    DenominationTable = (PDenominationTable)TSharedDataCache.TMPartner.GetCacheablePartnerTableDelegate(
                        TCacheablePartnerTablesEnum.DenominationList);
                    DenominationRow = (PDenominationRow)DenominationTable.Rows.Find(ARow.DenominationCode);

                    // 'Denomination' must be valid
                    if ((DenominationRow != null)
                        && !DenominationRow.ValidDenomination)
                    {
                        // if 'Denomination' is invalid then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PChurchTable.GetDenominationCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { ValidationControlsData.ValidationControlLabel, ARow.DenominationCode })),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePartnerManual(object AContext, PPartnerRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'PartnerStatus' must not be set to MERGED
            ValidationColumn = ARow.Table.Columns[PPartnerTable.ColumnStatusCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.StatusCode == SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PARTNERSTATUSMERGEDCHANGEUNDONE)),
                        ValidationColumn, ValidationControlsData.ValidationControl);

                    // Note: The error code 'ERR_PARTNERSTATUSMERGEDCHANGEUNDONE' sets VerificationResult.ControlValueUndoRequested = true!
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Subscription data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateSubscriptionManual(object AContext, PSubscriptionRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            DataColumn ValidationColumn2;
            TValidationControlsData ValidationControlsData;
            TValidationControlsData ValidationControlsData2;
            TScreenVerificationResult ScreenVerificationResult;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'SubscriptionStatus' must not be null or empty
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnSubscriptionStatusId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (((!ARow.IsSubscriptionStatusNull())
                     && (ARow.SubscriptionStatus == String.Empty))
                    || (ARow.IsSubscriptionStatusNull()))
                {
                    ScreenVerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_STATUSMANDATORY)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    ScreenVerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, ScreenVerificationResult, ValidationColumn);
            }

            // perform checks that include 'Start Date' ----------------------------------------------------------------
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnStartDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // 'Start Date' must not be later than 'Expiry Date'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnExpiryDateId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.ExpiryDate,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Start Date' must not be later than 'Renewal Date'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnSubscriptionRenewalDateId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.SubscriptionRenewalDate,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Start Date' must not be later than 'End Date'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnDateCancelledId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.DateCancelled,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Start Date' must not be later than 'Notice Sent'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnDateNoticeSentId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.DateNoticeSent,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Start Date' must not be later than 'First Sent'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnFirstIssueId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.FirstIssue,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Start Date' must not be later than 'Last Date'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnLastIssueId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.LastIssue,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // perform checks that include 'Date Renewed' ----------------------------------------------------------------
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnSubscriptionRenewalDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // 'Date Renewed' must not be later than today
                VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                         (ARow.SubscriptionRenewalDate, DateTime.Today,
                                         ValidationControlsData.ValidationControlLabel, Catalog.GetString("Today's Date"),
                                         AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);


                // 'Date Renewed' must not be later than 'Date Expired'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnExpiryDateId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.SubscriptionRenewalDate, ARow.ExpiryDate,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }

                // 'Date Renewed' must not be later than 'Date Notice Sent'
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnDateNoticeSentId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.SubscriptionRenewalDate, ARow.DateNoticeSent,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // 'Date Cancelled' must not be before today
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnDateCancelledId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                         (ARow.DateCancelled, DateTime.Today,
                                         ValidationControlsData.ValidationControlLabel, Catalog.GetString("Today's Date"),
                                         AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'First Sent' must not be later than 'Last Sent'
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnFirstIssueId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnLastIssueId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.FirstIssue, ARow.LastIssue,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // 'First Sent' must not be later than today
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnFirstIssueId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                         (ARow.FirstIssue, DateTime.Today,
                                         ValidationControlsData.ValidationControlLabel, Catalog.GetString("Today's Date"),
                                         AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Date Started' must not be later than 'First Sent'
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnStartDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                ValidationColumn2 = ARow.Table.Columns[PSubscriptionTable.ColumnFirstIssueId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                             (ARow.StartDate, ARow.FirstIssue,
                                             ValidationControlsData.ValidationControlLabel, ValidationControlsData2.ValidationControlLabel,
                                             AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // 'Last Sent' must not be later than today
            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnLastIssueId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstLesserOrEqualThanSecondDate
                                         (ARow.LastIssue, DateTime.Today,
                                         ValidationControlsData.ValidationControlLabel, Catalog.GetString("Today's Date"),
                                         AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            ValidationColumn = ARow.Table.Columns[PSubscriptionTable.ColumnSubscriptionStatusId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if ((!ARow.IsSubscriptionStatusNull())
                    && ((ARow.SubscriptionStatus == "CANCELLED")
                        || (ARow.SubscriptionStatus == "EXPIRED")))
                {
                    // When status is CANCELLED or EXPIRED then make sure that Reason ended and End date are set
                    if (ARow.IsReasonSubsCancelledCodeNull()
                        || (ARow.ReasonSubsCancelledCode == String.Empty))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_REASONENDEDMANDATORY_WHEN_EXPIRED)),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition/removal to/from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }

                    if (ARow.IsDateCancelledNull())
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_DATEENDEDMANDATORY_WHEN_EXPIRED)),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition/removal to/from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
                else
                {
                    // When Reason ended or End date are set then status must be CANCELLED or EXPIRED
                    if ((!ARow.IsReasonSubsCancelledCodeNull())
                        && (ARow.ReasonSubsCancelledCode != String.Empty))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_REASONENDEDSET_WHEN_ACTIVE)),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition/removal to/from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }

                    if (!ARow.IsDateCancelledNull())
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_SUBSCRIPTION_DATEENDEDSET_WHEN_ACTIVE)),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition/removal to/from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the Relationship data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AValidateForNewPartner">true if validation is run for a new partner record</param>
        /// <param name="APartnerKey">main partner key this validation is run for</param>
        /// <returns>void</returns>
        public static void ValidateRelationshipManual(object AContext, PPartnerRelationshipRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            bool AValidateForNewPartner = false, Int64 APartnerKey = 0)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Partner' must have a valid partner key and must not be 0
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnPartnerKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                // don't complain if this is done for a new partner record (partner key not yet saved in db)
                if (!(AValidateForNewPartner
                      && (APartnerKey == ARow.PartnerKey)))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                        ARow.PartnerKey, new TPartnerClass[] { }, false, "",
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }

                // 'Partner Key' and 'Another Partner Key'must not be the same
                // (Partner Key 0 will be dealt with by other checks)
                if ((ARow.PartnerKey != 0)
                    && (ARow.PartnerKey == ARow.RelationKey))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUESIDENTICAL_ERROR,
                                new string[] { ARow.PartnerKey.ToString(), ARow.RelationKey.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                //if (AVerificationResultCollection.Contains(ValidationColumn))
                //{xxx
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                //}
            }

            // 'Another Partner' must have a valid partner key and must not be 0
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnRelationKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // don't complain if this is done for a new partner record (partner key not yet saved in db)
                if (!(AValidateForNewPartner
                      && (APartnerKey == ARow.RelationKey)))
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidPartner(
                        ARow.RelationKey, new TPartnerClass[] { }, false, "",
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }
            }

            // 'Relation' must be valid and have a value
            ValidationColumn = ARow.Table.Columns[PPartnerRelationshipTable.ColumnRelationNameId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PRelationTable RelationTable;
                PRelationRow RelationRow;

                VerificationResult = null;

                if ((!ARow.IsRelationNameNull())
                    && (ARow.RelationName != String.Empty))
                {
                    RelationTable = (PRelationTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.RelationList);
                    RelationRow = (PRelationRow)RelationTable.Rows.Find(ARow.RelationName);

                    // 'Relation' must be valid
                    if ((RelationRow != null)
                        && !RelationRow.ValidRelation)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                    new string[] { ValidationControlsData.ValidationControlLabel, ARow.RelationName })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }
                else
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.RelationName,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Checks whether a Partner with a certain PartnerKey and a range of valid PartnerClasses exists.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey.</param>
        /// <param name="AValidPartnerClasses">An array of PartnerClasses. If the Partner exists, but its
        /// PartnerClass isn't in the array, a TVerificationResult is still returned.</param>
        /// <param name="AZeroPartnerKeyIsValid">Set to true if <paramref name="APartnerKey" /> 0 should be considered
        /// as valid (Default: false)</param>
        /// <param name="AErrorMessageText">Text that should be prepended to the ResultText. (Default: empty string)</param>
        /// <param name="AResultContext">ResultContext (Default: null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null). (Default: null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null). (Default: null).</param>
        /// <returns>Null if the Partner exists and its PartnerClass is in the <paramref name="AValidPartnerClasses" />
        /// array. If the Partner exists, but its PartnerClass isn't in the array, a TVerificationResult
        /// with details about the error is returned. This is also the case if the Partner doesn't exist at all
        /// or got merged into another Partner, or if <paramref name="APartnerKey" /> is 0 and <paramref name="AZeroPartnerKeyIsValid" />
        /// is false.
        /// </returns>
        public static TVerificationResult IsValidPartner(Int64 APartnerKey, TPartnerClass[] AValidPartnerClasses,
            bool AZeroPartnerKeyIsValid = false, string AErrorMessageText = "", object AResultContext = null,
            System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;
            string ShortName;
            TPartnerClass PartnerClass;
            bool PartnerExists;
            bool IsMergedPartner;
            string ValidPartnerClassesStr = String.Empty;
            bool PartnerClassValid = false;
            string PartnerClassInvalidMessageStr = Catalog.GetString("The Partner Class of the Partner needs to be '{0}', but it is '{1}'.");
            string PartnerClassConcatStr = Catalog.GetString(" or ");

            if ((AZeroPartnerKeyIsValid)
                && (APartnerKey == 0))
            {
                return null;
            }
            else if ((!AZeroPartnerKeyIsValid)
                     && (APartnerKey == 0))
            {
                if (AErrorMessageText == String.Empty)
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOZERO, new string[] { APartnerKey.ToString() }));
                }
                else
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOZERO));
                    ReturnValue.OverrideResultText(AErrorMessageText + Environment.NewLine + ReturnValue.ResultText);
                }
            }
            else
            {
                bool VerificationOK = TSharedPartnerValidationHelper.VerifyPartner(APartnerKey, AValidPartnerClasses, out PartnerExists,
                    out ShortName, out PartnerClass, out IsMergedPartner);

                if ((!VerificationOK)
                    || (IsMergedPartner))
                {
                    if (AErrorMessageText == String.Empty)
                    {
                        ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] { APartnerKey.ToString() }));
                    }
                    else
                    {
                        ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                                PetraErrorCodes.ERR_PARTNERKEY_INVALID, new string[] { APartnerKey.ToString() }));
                        ReturnValue.OverrideResultText(AErrorMessageText + Environment.NewLine + ReturnValue.ResultText);
                    }

                    if ((PartnerExists)
                        && (!IsMergedPartner))
                    {
                        if ((AValidPartnerClasses.Length == 1)
                            && (AValidPartnerClasses[0] != PartnerClass))
                        {
                            ReturnValue.OverrideResultText(ReturnValue.ResultText + " " +
                                String.Format(PartnerClassInvalidMessageStr, AValidPartnerClasses[0], PartnerClass));
                        }
                        else if (AValidPartnerClasses.Length > 1)
                        {
                            for (int Counter = 0; Counter < AValidPartnerClasses.Length; Counter++)
                            {
                                ValidPartnerClassesStr += "'" + AValidPartnerClasses[Counter] + "'" + PartnerClassConcatStr;

                                if (AValidPartnerClasses[Counter] == PartnerClass)
                                {
                                    PartnerClassValid = true;
                                }
                            }

                            if (!PartnerClassValid)
                            {
                                ValidPartnerClassesStr = ValidPartnerClassesStr.Substring(0,
                                    ValidPartnerClassesStr.Length - PartnerClassConcatStr.Length - 1);      // strip off "' or "
                                ReturnValue.OverrideResultText(ReturnValue.ResultText + " " +
                                    String.Format(PartnerClassInvalidMessageStr, ValidPartnerClassesStr, PartnerClass));
                            }
                        }
                    }
                }
            }

            if ((ReturnValue != null)
                && (AResultColumn != null))
            {
                ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether a Partner with Field 0 has a non-gift Motivation Group code.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey.</param>
        /// <param name="APartnerField">The field associated with the partner key</param>
        /// <param name="AMotivationGroup">The current motivation group</param>
        /// <param name="AErrorMessageText">Text that should be prepended to the ResultText. (Default: empty string)</param>
        /// <param name="AResultContext">ResultContext (Default: null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null). (Default: null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null). (Default: null).</param>
        /// <returns>Null if the Partner Field is non-zero or Motivation Group code is not Gift.
        ///   If the Partner Field is zero and Motivation Group is Gift, a TVerificationResult
        ///   with details about the error is returned.
        /// </returns>
        public static TVerificationResult IsValidRecipientFieldForMotivationGroup(Int64 APartnerKey, Int64 APartnerField,
            string AMotivationGroup, string AErrorMessageText = "", object AResultContext = null,
            System.Data.DataColumn AResultColumn = null, System.Windows.Forms.Control AResultControl = null)
        {
            TVerificationResult ReturnValue = null;

            if ((APartnerField == 0) && (AMotivationGroup == MFinanceConstants.MOTIVATION_GROUP_GIFT))
            {
                if (AErrorMessageText == String.Empty)
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_RECIPIENT_FIELD_MOTIVATION_GROUP, new string[] { APartnerKey.ToString() }));
                }
                else
                {
                    ReturnValue = new TVerificationResult(AResultContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_RECIPIENT_FIELD_MOTIVATION_GROUP));
                    ReturnValue.OverrideResultText(AErrorMessageText + Environment.NewLine + ReturnValue.ResultText);
                }
            }
            else
            {
                return null;
            }

            if ((ReturnValue != null)
                && (AResultColumn != null))
            {
                ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn, AResultControl);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks that a Partner with a certain PartnerKey exists and is a Partner of PartnerClass UNIT.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey.</param>
        /// <param name="AZeroPartnerKeyIsValid">Set to true if <paramref name="APartnerKey" /> 0 should be considered
        /// as valid (Default: false)</param>
        /// <param name="AErrorMessageText">Text that should be prepended to the ResultText. (Default: empty string)</param>
        /// <param name="AResultContext">ResultContext (optional).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null). (Default: null).</param>
        /// <param name="AResultControl">Which <see cref="System.Windows.Forms.Control" /> is involved (can be null). (Default: null).</param>
        /// <returns>Null if the Partner exists and its PartnerClass is UNIT. If the Partner exists,
        /// but its PartnerClass isn't UNIT, a TVerificationResult with details about the error is
        /// returned. This is also the case if the Partner doesn't exist at all or got merged
        /// into another Partner, or if <paramref name="APartnerKey" /> is 0 and <paramref name="AZeroPartnerKeyIsValid" />
        /// is false.
        /// </returns>
        public static TVerificationResult IsValidUNITPartner(Int64 APartnerKey, bool AZeroPartnerKeyIsValid = false,
            string AErrorMessageText = "", object AResultContext = null, System.Data.DataColumn AResultColumn = null,
            System.Windows.Forms.Control AResultControl = null)
        {
            return IsValidPartner(APartnerKey, new TPartnerClass[] { TPartnerClass.UNIT }, AZeroPartnerKeyIsValid,
                AErrorMessageText, AResultContext, AResultColumn, AResultControl);
        }

        /// <summary>
        /// Validates the MPartner Mailing Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateMailingSetup(object AContext, PMailingRow ARow,
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

            // 'MailingDate' must not be empty
            ValidationColumn = ARow.Table.Columns[PMailingTable.ColumnMailingDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.MailingDate,
                    ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, true,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MPartner Address Layout Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AAddressElementTable">A table of all available Address Block Elements</param>
        public static void ValidateAddressBlockSetup(object AContext, PAddressBlockRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            PAddressBlockElementTable AAddressElementTable)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // Do validation on the address block text
            ValidationColumn = ARow.Table.Columns[PAddressBlockTable.ColumnAddressBlockTextId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                // Address Block Text must not be empty
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.AddressBlockText, ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                if (VerificationResult == null)
                {
                    // Text must contain at least one replaceable parameter and parameter must exist
                    // Start by parsing the text
                    string s = ARow.AddressBlockText;
                    List <string>allElements = new List <string>();
                    int posStart = 0;
                    int posEnd = -2;

                    while (posStart >= 0)
                    {
                        posStart = s.IndexOf("[[", posEnd + 2);

                        if (posStart != -1)
                        {
                            posEnd = s.IndexOf("]]", posStart);

                            if (posEnd > posStart)
                            {
                                // get the placeholder text
                                string item = s.Substring(posStart + 2, posEnd - posStart - 2);

                                if ((item == "CapsOn") && allElements.Contains("CapsOff"))
                                {
                                    allElements.Remove("CapsOff");
                                }

                                if (!allElements.Contains(item))
                                {
                                    allElements.Add(item);
                                }
                            }
                            else
                            {
                                // No matching tag
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ADDRESS_BLOCK_HAS_MISMATCHED_TAGS)),
                                    ValidationColumn, ValidationControlsData.ValidationControl);
                                break;
                            }
                        }
                    }

                    if (VerificationResult == null)
                    {
                        // Check there is at least one data element
                        if (allElements.Count == 0)
                        {
                            // No elements
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ADDRESS_BLOCK_HAS_NO_DATA_PLACEHOLDERS)),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                        else
                        {
                            // Check that the elements exist and that at least one is not a directive
                            bool bFoundNonDirective = false;

                            foreach (string e in allElements)
                            {
                                PAddressBlockElementRow row = (PAddressBlockElementRow)AAddressElementTable.Rows.Find(e);

                                if (row == null)
                                {
                                    // Unknown element
                                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ADDRESS_BLOCK_HAS_UNKNOWN_PLACEHOLDER)),
                                        ValidationColumn, ValidationControlsData.ValidationControl);

                                    break;
                                }
                                else if (row.IsDirective == false)
                                {
                                    bFoundNonDirective = true;
                                }
                            }

                            if ((VerificationResult == null) && (bFoundNonDirective == false))
                            {
                                // We got elements but they were all directives
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ADDRESS_BLOCK_ONLY_HAS_DIRECTIVE_PLACEHOLDERS)),
                                    ValidationColumn, ValidationControlsData.ValidationControl);
                            }

                            if (VerificationResult == null)
                            {
                                // All good so far.  If there is a CapsOn there must be a CapsOff
                                if (allElements.Contains("CapsOn") && !allElements.Contains("CapsOff"))
                                {
                                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ADDRESS_BLOCK_HAS_NO_MATCHING_CAPS_OFF)),
                                        ValidationColumn, ValidationControlsData.ValidationControl);
                                }
                            }
                        }
                    }
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MPartner Formality Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateFormalitySetup(object AContext, PFormalityRow ARow,
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

            // 'FormalityLevel' must be between 1 and 6
            ValidationColumn = ARow.Table.Columns[PFormalityTable.ColumnFormalityLevelId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsInRange(ARow.FormalityLevel, 1, 6,
                    Catalog.GetString("Formality Level"),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            //Greeting string must be well formed
            // It starts with <N and must be followed by none or more of TPIFA and a closing <
            ValidationColumn = ARow.Table.Columns[PFormalityTable.ColumnSalutationTextId];
            VerificationResult = null;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                int startPos = ARow.SalutationText.IndexOf("<N");

                if (startPos >= 0)
                {
                    int endPos = ARow.SalutationText.IndexOf('<', startPos + 1);

                    if (endPos == -1)
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_MISSING_CLOSE_TAG_IN_SALUTATION)),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }

                    if (VerificationResult == null)
                    {
                        string additionalChars = ARow.SalutationText.Substring(startPos + 2, endPos - startPos - 2);

                        // Make sure there are no duplicates
                        for (int i = 0; i < additionalChars.Length - 1; i++)
                        {
                            if (additionalChars.IndexOf(additionalChars[i], i + 1) != -1)
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_DUPLICATE_MODIFIER_TAG_IN_SALUTATION)),
                                    ValidationColumn, ValidationControlsData.ValidationControl);
                                break;
                            }
                        }

                        if (VerificationResult == null)
                        {
                            string allowedChars = "TPIFA";

                            // Make sure that only these chars are used...
                            for (int i = 0; i < additionalChars.Length; i++)
                            {
                                if (allowedChars.IndexOf(additionalChars[i]) == -1)
                                {
                                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_UNKNOWN_MODIFIER_TAG_IN_SALUTATION)),
                                        ValidationColumn, ValidationControlsData.ValidationControl);
                                    break;
                                }
                            }
                        }
                    }
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MPartner Relationship Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateRelationshipSetupManual(object AContext, PRelationRow ARow,
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

            // 'Relationship Category' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PRelationTable.ColumnRelationCategoryId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PRelationCategoryTable RelationCategoryTable;
                PRelationCategoryRow RelationCategoryRow;

                VerificationResult = null;

                if ((!ARow.IsRelationCategoryNull())
                    && (ARow.RelationCategory != String.Empty))
                {
                    RelationCategoryTable = (PRelationCategoryTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                        TCacheablePartnerTablesEnum.RelationCategoryList);
                    RelationCategoryRow = (PRelationCategoryRow)RelationCategoryTable.Rows.Find(ARow.RelationCategory);

                    // 'Relationship Category' must not be unassignable
                    if ((RelationCategoryRow != null)
                        && RelationCategoryRow.UnassignableFlag
                        && (RelationCategoryRow.IsUnassignableDateNull()
                            || (RelationCategoryRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Relationship Category' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PRelationTable.GetRelationCategoryDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { ValidationControlsData.ValidationControlLabel, ARow.RelationCategory })),
                                ValidationColumn, ValidationControlsData.ValidationControl);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Banking Details screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="ABankingDetails">test if there is only one main account</param>
        /// <param name="ACountryCode">Country Code for ARow's corresponding Bank's country</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateBankingDetails(object AContext, PBankingDetailsRow ARow,
            PBankingDetailsTable ABankingDetails, string ACountryCode,
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

            // 'BankKey' must be a valid BANK partner
            ValidationColumn = ARow.Table.Columns[PBankingDetailsTable.ColumnBankKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.BankKey != 0)
                {
                    VerificationResult = IsValidPartner(
                        ARow.BankKey, new TPartnerClass[] { TPartnerClass.BANK }, false, string.Empty,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl
                        );
                }

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!

                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // validate that there are not multiple main accounts
            ValidationColumn = ARow.Table.Columns[PartnerEditTDSPBankingDetailsTable.ColumnMainAccountId];
            int countMainAccount = 0;

            foreach (PartnerEditTDSPBankingDetailsRow bdrow in ABankingDetails.Rows)
            {
                if (bdrow.RowState != DataRowState.Deleted)
                {
                    if (bdrow.MainAccount)
                    {
                        countMainAccount++;
                    }
                }
            }

            if (countMainAccount > 1)
            {
                // will we ever get here?
                AVerificationResultCollection.Add(
                    new TScreenVerificationResult(
                        new TVerificationResult(
                            AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKINGDETAILS_ONLYONEMAINACCOUNT)),
                        ((PartnerEditTDSPBankingDetailsTable)ARow.Table).ColumnMainAccount,
                        ValidationControlsData.ValidationControl
                        ));
            }

            VerificationResult = null;

            // validate the account number (if validation exists for bank's country)
            ValidationColumn = ARow.Table.Columns[PBankingDetailsTable.ColumnBankAccountNumberId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                CommonRoutines Routines = new CommonRoutines();

                if (!string.IsNullOrEmpty(ARow.BankAccountNumber) && (Routines.CheckAccountNumber(ARow.BankAccountNumber, ACountryCode) <= 0))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(
                            AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_ACCOUNTNUMBER_INVALID)),
                        ((PartnerEditTDSPBankingDetailsTable)ARow.Table).ColumnBankAccountNumber,
                        ValidationControlsData.ValidationControl);
                }

                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            VerificationResult = null;

            // validate the IBAN (if it exists)
            ValidationColumn = ARow.Table.Columns[PBankingDetailsTable.ColumnIbanId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                AVerificationResultCollection.Remove(ValidationColumn);

                if (!string.IsNullOrEmpty(ARow.Iban) && (CommonRoutines.CheckIBAN(ARow.Iban, out VerificationResult) == false))
                {
                    VerificationResult = new TScreenVerificationResult(
                        new TVerificationResult(AContext, VerificationResult.ResultText, VerificationResult.ResultCode,
                            VerificationResult.ResultSeverity),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Extra Validatation for the Banking Details screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateBankingDetailsExtra(object AContext, PBankingDetailsRow ARow,
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

            // 'BankKey' must be included
            ValidationColumn = ARow.Table.Columns[PBankingDetailsTable.ColumnBankKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.BankKey == 0)
                {
                    VerificationResult = new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_BANKINGDETAILS_NO_BANK_SELECTED, new string[] { ARow.BankKey.ToString() }));

                    VerificationResult = new TScreenVerificationResult(VerificationResult, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            VerificationResult = null;

            // Account Number and IBAN cannot both be empty
            ValidationColumn = ARow.Table.Columns[PBankingDetailsTable.ColumnBankAccountNumberId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (string.IsNullOrEmpty(ARow.BankAccountNumber) && string.IsNullOrEmpty(ARow.Iban))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKINGDETAILS_MISSING_ACCOUNTNUMBERORIBAN)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner Interest screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AInterestCategory">The chosen interest category.</param>
        public static void ValidatePartnerInterestManual(object AContext, PPartnerInterestRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            string AInterestCategory)
        {
            DataColumn ValidationColumn;
            DataColumn ValidationColumn2;
            DataColumn ValidationColumn3;
            TValidationControlsData ValidationControlsData;
            TValidationControlsData ValidationControlsData2;
            TValidationControlsData ValidationControlsData3;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // remove possible previous columns from result collection
            ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnLevelId];
            AVerificationResultCollection.Remove(ValidationColumn);
            ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnInterestId];
            AVerificationResultCollection.Remove(ValidationColumn);

            // check that level is entered within valid range (depending on interest category)
            ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnLevelId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                PInterestCategoryTable CategoryTable;
                PInterestCategoryRow CategoryRow;
                int LevelRangeLow;
                int LevelRangeHigh;

                // check if level is within valid range (retrieve valid range from cached tables)
                CategoryTable = (PInterestCategoryTable)TSharedDataCache.TMPartner.GetCacheablePartnerTable(
                    TCacheablePartnerTablesEnum.InterestCategoryList);
                CategoryRow = (PInterestCategoryRow)CategoryTable.Rows.Find(new object[] { AInterestCategory });

                if ((CategoryRow != null)
                    && !ARow.IsLevelNull())
                {
                    LevelRangeLow = 0;
                    LevelRangeHigh = 0;

                    if (!CategoryRow.IsLevelRangeLowNull())
                    {
                        LevelRangeLow = CategoryRow.LevelRangeLow;
                    }

                    if (!CategoryRow.IsLevelRangeHighNull())
                    {
                        LevelRangeHigh = CategoryRow.LevelRangeHigh;
                    }

                    if ((!CategoryRow.IsLevelRangeLowNull()
                         && (ARow.Level < CategoryRow.LevelRangeLow))
                        || (!CategoryRow.IsLevelRangeHighNull()
                            && (ARow.Level > CategoryRow.LevelRangeHigh)))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUE_OUTSIDE_OF_RANGE,
                                    new string[] { ValidationControlsData.ValidationControlLabel, LevelRangeLow.ToString(), LevelRangeHigh.ToString() })),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
            }

            // check that at least one of interest, country or field is filled
            ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnInterestId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                ValidationColumn2 = ARow.Table.Columns[PPartnerInterestTable.ColumnCountryId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn2, out ValidationControlsData2))
                {
                    ValidationColumn3 = ARow.Table.Columns[PPartnerInterestTable.ColumnFieldKeyId];

                    if (AValidationControlsDict.TryGetValue(ValidationColumn3, out ValidationControlsData3))
                    {
                        if ((ARow.IsInterestNull() || (ARow.Interest == String.Empty))
                            && (ARow.IsCountryNull() || (ARow.Country == String.Empty))
                            && (ARow.IsFieldKeyNull() || (ARow.FieldKey == 0)))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INTEREST_NO_DATA_SET_AT_ALL, new string[] { })),
                                ValidationColumn, ValidationControlsData.ValidationControl);

                            // Handle addition to/removal from TVerificationResultCollection
                            AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                        }
                    }
                }
            }

            // check that interest is filled if a category is set
            if (AInterestCategory != "")
            {
                ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnInterestId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if (ARow.IsInterestNull()
                        || (ARow.Interest == String.Empty))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INTEREST_NOT_SET, new string[] { AInterestCategory })),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                    }
                }
            }

            // 'Field' must be a valid UNIT partner (if set at all)
            ValidationColumn = ARow.Table.Columns[PPartnerInterestTable.ColumnFieldKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.IsFieldKeyNull())
                {
                    VerificationResult = IsValidPartner(
                        ARow.FieldKey, new TPartnerClass[] { TPartnerClass.UNIT }, true, string.Empty,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl
                        );
                }

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates a Gift Destination record
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateGiftDestinationRowManual(object AContext, PPartnerGiftDestinationRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Field Key' must be a Partner associated with a Cost Centre
            DataColumn ValidationColumn = ARow.Table.Columns[PPartnerGiftDestinationTable.ColumnFieldKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!TSharedPartnerValidationHelper.PartnerIsLinkedToCC(ARow.FieldKey))
                {
                    VerificationResult = new TScreenVerificationResult(AContext, ValidationColumn,
                        String.Format(
                            Catalog.GetString("Gift Destination ({0}) must be a Partner linked to a Cost Centre."),
                            ARow.FieldKey),
                        ValidationControlsData.ValidationControl,
                        TResultSeverity.Resv_Critical);
                }

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // Date Effective must not be after Date Expired (it can be equal)
            ValidationColumn = ARow.Table.Columns[PPartnerGiftDestinationTable.ColumnDateEffectiveId];

            if (ARow.DateEffective > ARow.DateExpires)
            {
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALID_DATES)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates whole Gift Destination data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ATable">The <see cref="DataTable" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateGiftDestinationManual(object AContext, PPartnerGiftDestinationTable ATable,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;
            DataColumn ValidationColumn = ATable.Columns[PPartnerGiftDestinationTable.ColumnDateExpiresId];

            bool MoreThanOneOpenGiftDestination = false;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                foreach (PPartnerGiftDestinationRow Row in ATable.Rows)
                {
                    foreach (PPartnerGiftDestinationRow CompareToRow in ATable.Rows)
                    {
                        if (Row != CompareToRow)
                        {
                            // make sure there is no more than one open ended record
                            if (Row.IsDateExpiresNull() && CompareToRow.IsDateExpiresNull())
                            {
                                MoreThanOneOpenGiftDestination = true;
                            }

                            // Make sure no records overlap
                            if ((CompareToRow.DateEffective != CompareToRow.DateExpires) && (Row.DateEffective != Row.DateExpires)
                                && (((Row.DateEffective < CompareToRow.DateEffective)
                                     && ((Row.DateExpires >= CompareToRow.DateEffective)
                                         || (Row.IsDateExpiresNull() && !CompareToRow.IsDateExpiresNull())))
                                    || (Row.DateEffective == CompareToRow.DateEffective)))
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_DATES_OVERLAP)),
                                    ValidationColumn, ValidationControlsData.ValidationControl);

                                // Handle addition to/removal from TVerificationResultCollection
                                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                            }
                        }
                    }
                }
            }

            if (MoreThanOneOpenGiftDestination)
            {
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_MORETHANONE_OPEN_GIFTDESTINATION)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner Edit screens' Contact Details Tab data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AValueKind">The PPartnerAttributeType.AttributeValueKind of the DataRow passed in in <paramref name="ARow"/>.</param>
        public static void ValidateContactDetailsManual(object AContext, PPartnerAttributeRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            TPartnerAttributeTypeValueKind AValueKind)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Value' must not be null
            ValidationColumn = ARow.Table.Columns[PPartnerAttributeTable.ColumnValueId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.Value,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // If this record is about an E-Mail Contact Detail...
            if (AValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS)
            {
                // ...then the E-mail Address must be in a correct format
                ValidationColumn = ARow.Table.Columns[PPartnerAttributeTable.ColumnValueId];

                if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    VerificationResult = TStringChecks.ValidateEmail(ARow.Value, true,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
            }

            // 'No Longer Current From Date' must not be a future date if the 'Current' Flag is set to false
            ValidationColumn = ARow.Table.Columns[PPartnerAttributeTable.ColumnNoLongerCurrentFromId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if (!ARow.Current)
                {
                    VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.NoLongerCurrentFrom,
                        ValidationControlsData.ValidationControlLabel, AContext, ValidationColumn,
                        ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner Interest Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateInterestSetupManual(object AContext, PInterestRow ARow,
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

            // 'Category' must not be null
            ValidationColumn = ARow.Table.Columns[PInterestTable.ColumnCategoryId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.Category, Catalog.GetString("Category"),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Partner Contact Types Setup usercontrol data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateContactTypesSetupManual(object AContext, PPartnerAttributeTypeRow ARow,
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

            // 'HyperLink Format' must be correct if ARow.AttributeTypeValueKind is "CONTACTDETAIL_HYPERLINK_WITHVALUE"
            ValidationColumn = ARow.Table.Columns[PPartnerAttributeTypeTable.ColumnHyperlinkFormatId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.AttributeTypeValueKind == "CONTACTDETAIL_HYPERLINK_WITHVALUE")
                {
                    // Remove any Data Validation errors that might have been recorded
                    AVerificationResultCollection.Remove(ValidationColumn);

                    // 'HyperLink Format' must not be empty string
                    VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.HyperlinkFormat, Catalog.GetString("Link Format"),
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                    if ((VerificationResult == null)
                        && (ARow.HyperlinkFormat == THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER))
                    {
                        // 'HyperLink Format' must contain more than just THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALID_HYPERLINK_WITH_VALUE_JUST_CONTAINING_PLACEHOLDER,
                                    new string[] { THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }

                    if ((VerificationResult == null)
                        && (ARow.HyperlinkFormat.IndexOf(THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER,
                                StringComparison.InvariantCulture) == -1))
                    {
                        // 'HyperLink Format' must contain THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER,
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALID_HYPERLINK_WITH_VALUE_NOT_CONTAINING_PLACEHOLDER,
                                    new string[] { THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
                }
                else
                {
                    // Remove any Data Validation errors that might have been recorded
                    AVerificationResultCollection.Remove(ValidationColumn);
                }
            }

            VerificationResult = null;

            // 'Unssignable Date' must not be empty if the flag is set
            ValidationColumn = ARow.Table.Columns[PPartnerAttributeTypeTable.ColumnUnassignableDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.Unassignable)
                {
                    VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.UnassignableDate,
                        ValidationControlsData.ValidationControlLabel, AVerificationResultCollection, true,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AContext"></param>
        /// <param name="ARow"></param>
        /// <param name="VerificationResultCollection"></param>
        /// <param name="FValidationControlsDict"></param>
        public static void ValidateContactLogManual(object AContext,
            PContactLogRow ARow,
            ref TVerificationResultCollection VerificationResultCollection,
            TValidationControlsDict FValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            ValidationColumn = ARow.Table.Columns[PContactLogTable.ColumnContactCodeId];
            VerificationResult = TGeneralChecks.ValueMustNotBeNullOrEmptyString(ARow.ContactCode, Catalog.GetString("Contact Code"),
                AContext, ValidationColumn);

            // Handle addition to/removal from TVerificationResultCollection
            VerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
        }
    }
}