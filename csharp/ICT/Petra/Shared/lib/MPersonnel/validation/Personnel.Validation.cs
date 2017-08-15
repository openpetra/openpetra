//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Shared.MPersonnel.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPersonnel Personnel DataTables.
    /// </summary>
    public static partial class TSharedPersonnelValidation_Personnel
    {
        /// <summary>
        /// Validates the Commitment data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateCommitmentManual(object AContext, PmStaffDataRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Receiving Field' must be a Partner of Class 'UNIT' and must not be 0
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnReceivingFieldId];

            if (true)
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                    ARow.ReceivingField, false, THelper.NiceValueDescription(
                        String.Empty) + " must be set correctly.",
                    AContext, ValidationColumn);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Home Office' must be a Partner of Class 'UNIT'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnHomeOfficeId];

            if (true)
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(ARow.HomeOffice, false,
                    THelper.NiceValueDescription(String.Empty) + " must be set correctly.",
                    AContext, ValidationColumn);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Recruiting Office' must be a Partner of Class 'UNIT'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnOfficeRecruitedById];

            if (true)
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(ARow.OfficeRecruitedBy, false,
                    THelper.NiceValueDescription(String.Empty) + " must be set correctly.",
                    AContext, ValidationColumn);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'End of Commitment' must be later than 'Start of Commitment'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnEndOfCommitmentId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.EndOfCommitment, ARow.StartOfCommitment,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Status' must have a value
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnStatusCodeId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.StatusCode,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the Job Assignment data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateJobAssignmentManual(object AContext, PmJobAssignmentRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'From Date' must be defined
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnFromDateId];

            if (true)
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.FromDate,
                    String.Empty, AVerificationResultCollection, true,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'To Date' must be later than 'From Date', must not be null and must not be more than 2 years from now
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnToDateId];

            if (true)
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.ToDate,
                    String.Empty, AVerificationResultCollection, true,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);

                if (VerificationResult == null)
                {
                    VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.ToDate, ARow.FromDate,
                        String.Empty, String.Empty,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);

                    if (VerificationResult == null)
                    {
                        VerificationResult = TDateChecks.IsDateBetweenDates(ARow.ToDate, ARow.FromDate, DateTime.Today.AddYears(2),
                            String.Empty,
                            TDateBetweenDatesCheckType.dbdctUnspecific, TDateBetweenDatesCheckType.dbdctUnspecific,
                            AContext, ValidationColumn);

                        // Handle addition to/removal from TVerificationResultCollection
                        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                    }
                }
            }

            // 'Unit' must be a Partner of Class 'UNIT' and must not be 0
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnUnitKeyId];

            if (true)
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                    ARow.UnitKey, false, THelper.NiceValueDescription(
                        String.Empty) + " must be set correctly.",
                    AContext, ValidationColumn);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Assignment Type' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnAssignmentTypeCodeId];

            if (true)
            {
                PtAssignmentTypeTable TypeTable;
                PtAssignmentTypeRow TypeRow;

                VerificationResult = null;

                if ((!ARow.IsAssignmentTypeCodeNull())
                    && (ARow.AssignmentTypeCode != String.Empty))
                {
                    TypeTable = (PtAssignmentTypeTable)TSharedDataCache.TMPersonnel.GetCacheableUnitsTable(
                        TCacheableUnitTablesEnum.JobAssignmentTypeList);
                    TypeRow = (PtAssignmentTypeRow)TypeTable.Rows.Find(ARow.AssignmentTypeCode);

                    // 'Assignment Type' must not be unassignable
                    if ((TypeRow != null)
                        && TypeRow.UnassignableFlag
                        && (TypeRow.IsUnassignableDateNull()
                            || (TypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Assignment Type' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmJobAssignmentTable.GetAssignmentTypeCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.AssignmentTypeCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Position' must be not be null and not unassignable
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnPositionNameId];

            if (true)
            {
                PtPositionTable PositionTable;
                PtPositionRow PositionRow;

                VerificationResult = null;

                if ((!ARow.IsPositionNameNull())
                    && (ARow.PositionName != String.Empty))
                {
                    PositionTable = (PtPositionTable)TSharedDataCache.TMPersonnel.GetCacheableUnitsTable(
                        TCacheableUnitTablesEnum.PositionList);
                    PositionRow = (PtPositionRow)PositionTable.Rows.Find(new object[] { ARow.PositionName, ARow.PositionScope });

                    // 'Position' must not be unassignable
                    if ((PositionRow != null)
                        && PositionRow.UnassignableFlag
                        && (PositionRow.IsUnassignableDateNull()
                            || (PositionRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Position' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmJobAssignmentTable.GetPositionNameDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.PositionName })),
                                ValidationColumn);
                        }
                    }
                }
                else
                {
                    // Position name must not be null
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.PositionName,
                        String.Empty,
                        AContext, ValidationColumn);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the Passport data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidatePassportManual(object AContext, PmPassportDetailsRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Passport Number' must have a value
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnPassportNumberId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.PassportNumber,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Passport Name' must contain an opening and a closing paraenthesis
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnFullPassportNameId];

            if (true)
            {
                if ((!ARow.FullPassportName.Contains("("))
                    || (!ARow.FullPassportName.Contains(")")))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INDIV_DATA_PASSPORT_NAME_MISSING_PARAS,
                                new string[] { String.Empty, ARow.FullPassportName })),
                        ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }

            // 'Expiry Date' must be later than 'Issue Date'
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnDateOfExpirationId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.DateOfExpiration, ARow.DateOfIssue,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Passport Type' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnPassportDetailsTypeId];

            if (true)
            {
                PtPassportTypeTable TypeTable;
                PtPassportTypeRow TypeRow;

                VerificationResult = null;

                if ((!ARow.IsPassportDetailsTypeNull())
                    && (ARow.PassportDetailsType != String.Empty))
                {
                    TypeTable = (PtPassportTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                        TCacheablePersonTablesEnum.PassportTypeList);
                    TypeRow = (PtPassportTypeRow)TypeTable.Rows.Find(ARow.PassportDetailsType);

                    // 'Passport Type' must not be unassignable
                    if ((TypeRow != null)
                        && TypeRow.UnassignableFlag
                        && (TypeRow.IsUnassignableDateNull()
                            || (TypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Passport Type' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmPassportDetailsTable.GetPassportDetailsTypeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.PassportDetailsType })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the personal document data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidatePersonalDocumentManual(object AContext, PmDocumentRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;
            PmDocumentTypeTable DocTypeTable;
            PmDocumentTypeRow DocTypeRow = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDocCodeId];

            if (true)
            {
                VerificationResult = null;

                if ((!ARow.IsDocCodeNull())
                    && (ARow.DocCode != String.Empty))
                {
                    DocTypeTable = (PmDocumentTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                        TCacheablePersonTablesEnum.DocumentTypeList);
                    DocTypeRow = (PmDocumentTypeRow)DocTypeTable.Rows.Find(ARow.DocCode);

                    // 'Document Type' must not be unassignable
                    if ((DocTypeRow != null)
                        && DocTypeRow.UnassignableFlag
                        && (DocTypeRow.IsUnassignableDateNull()
                            || (DocTypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Document Type' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmDocumentTable.GetDocCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.DocCode })),
                                ValidationColumn);
                        }
                    }
                }
                else
                {
                    // 'Document Code' must have a value
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.DocCode,
                        String.Empty,
                        AContext, ValidationColumn);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Document Id' must have a value
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDocumentIdId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.DocumentId,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Issue Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDateOfIssueId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.DateOfIssue, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Expiry Date' must be later or equal 'Start Date'
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDateOfExpirationId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.DateOfExpiration, ARow.DateOfStart,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the personal language data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidatePersonalLanguageManual(object AContext, PmPersonLanguageRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;
            PtLanguageLevelTable LanguageLevelTable;
            PtLanguageLevelRow LanguageLevelRow = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Language Level' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonLanguageTable.ColumnLanguageLevelId];

            if (true)
            {
                VerificationResult = null;

                if (!ARow.IsLanguageLevelNull())
                {
                    LanguageLevelTable = (PtLanguageLevelTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.LanguageLevelList);
                    LanguageLevelRow = (PtLanguageLevelRow)LanguageLevelTable.Rows.Find(ARow.LanguageLevel);

                    // 'Language Level' must not be unassignable
                    if ((LanguageLevelRow != null)
                        && LanguageLevelRow.UnassignableFlag
                        && (LanguageLevelRow.IsUnassignableDateNull()
                            || (LanguageLevelRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Language Level' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmPersonLanguageTable.GetLanguageLevelDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.LanguageLevel.ToString() })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the skill data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateSkillManual(object AContext, PmPersonSkillRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Skill Category' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonSkillTable.ColumnSkillCategoryCodeId];

            if (true)
            {
                PtSkillCategoryTable CategoryTable;
                PtSkillCategoryRow CategoryRow = null;

                VerificationResult = null;

                if (!ARow.IsSkillCategoryCodeNull())
                {
                    CategoryTable = (PtSkillCategoryTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.SkillCategoryList);
                    CategoryRow = (PtSkillCategoryRow)CategoryTable.Rows.Find(ARow.SkillCategoryCode);

                    // 'Skill Category' must not be unassignable
                    if ((CategoryRow != null)
                        && CategoryRow.UnassignableFlag
                        && (CategoryRow.IsUnassignableDateNull()
                            || (CategoryRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Skill Category' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmPersonSkillTable.GetSkillCategoryCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.SkillCategoryCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Skill Level' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonSkillTable.ColumnSkillLevelId];

            if (true)
            {
                PtSkillLevelTable LevelTable;
                PtSkillLevelRow LevelRow = null;

                VerificationResult = null;

                if (!ARow.IsSkillLevelNull())
                {
                    LevelTable = (PtSkillLevelTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.SkillLevelList);
                    LevelRow = (PtSkillLevelRow)LevelTable.Rows.Find(ARow.SkillLevel);

                    // 'Skill Level' must not be unassignable
                    if ((LevelRow != null)
                        && LevelRow.UnassignableFlag
                        && (LevelRow.IsUnassignableDateNull()
                            || (LevelRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Skill Level' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmPersonSkillTable.GetSkillLevelDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.SkillLevel.ToString() })),
                                ValidationColumn);
                        }
                    }
                }
                else
                {
                    // skill level must have a value
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUE_NOT_ENTERED)),
                        ValidationColumn);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the previous experience data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidatePreviousExperienceManual(object AContext, PmPastExperienceRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Start Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnStartDateId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.StartDate, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'End Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnEndDateId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.EndDate, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'End Date' must be later or equal 'Start Date'
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnEndDateId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.EndDate, ARow.StartDate,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the progress report (evaluation) data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateProgressReportManual(object AContext, PmPersonEvaluationRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Evaluator' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluatorId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Evaluator,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Evaluation Date' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluationDateId];

            if (true)
            {
                VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.EvaluationDate,
                    String.Empty, AVerificationResultCollection, true,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Next Evaluation Date' must have a value if evaluation type is not set to "Leaving"
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnNextEvaluationDateId];

            if (true)
            {
                VerificationResult = null;

                if (ARow.EvaluationType != "Leaving")
                {
                    VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.NextEvaluationDate,
                        String.Empty, AVerificationResultCollection, true,
                        AContext, ValidationColumn);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Next Evaluation Date' must be later than 'Evaluation Date'
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnNextEvaluationDateId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.NextEvaluationDate, ARow.EvaluationDate,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Report Type' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluationTypeId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.EvaluationType,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the personal (miscellaneous) data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidatePersonalDataManual(object AContext, PmPersonalDataRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Believer since year' must have a sensible value (must not be below 1850 and must not lie in the future)
            ValidationColumn = ARow.Table.Columns[PmPersonalDataTable.ColumnBelieverSinceYearId];

            if (true)
            {
                if (!ARow.IsBelieverSinceYearNull() && (ARow.BelieverSinceYear != 0))
                {
                    VerificationResult = TDateChecks.IsDateBetweenDates(
                        new DateTime(ARow.BelieverSinceYear, 12, 31), new DateTime(1850, 1, 1), new DateTime(DateTime.Today.Year, 12, 31),
                        String.Empty,
                        TDateBetweenDatesCheckType.dbdctUnrealisticDate, TDateBetweenDatesCheckType.dbdctNoFutureDate,
                        AContext, ValidationColumn);

                    // Handle addition to/removal from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }
        }

        /// <summary>
        /// Validates the general application record of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AEventApplication">true if application for event, false if application for field.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateGeneralApplicationManual(object AContext, PmGeneralApplicationRow ARow, bool AEventApplication,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Application Type' must have a value and must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnAppTypeNameId];

            if (true)
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.AppTypeName,
                    String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);

                PtApplicationTypeTable AppTypeTable;
                PtApplicationTypeRow AppTypeRow = null;

                VerificationResult = null;

                if (!ARow.IsAppTypeNameNull())
                {
                    AppTypeTable = (PtApplicationTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.ApplicationTypeList);
                    AppTypeRow = (PtApplicationTypeRow)AppTypeTable.Rows.Find(ARow.AppTypeName);

                    // 'Application Type' must not be unassignable
                    if ((AppTypeRow != null)
                        && AppTypeRow.UnassignableFlag
                        && (AppTypeRow.IsUnassignableDateNull()
                            || (AppTypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Application Type' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmGeneralApplicationTable.GetAppTypeNameDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.AppTypeName })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Application Status' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenApplicationStatusId];

            if (true)
            {
                PtApplicantStatusTable AppStatusTable;
                PtApplicantStatusRow AppStatusRow = null;

                VerificationResult = null;

                if (!ARow.IsGenApplicationStatusNull())
                {
                    AppStatusTable = (PtApplicantStatusTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.ApplicantStatusList);
                    AppStatusRow = (PtApplicantStatusRow)AppStatusTable.Rows.Find(ARow.GenApplicationStatus);

                    // 'Application Status' must not be unassignable
                    if ((AppStatusRow != null)
                        && AppStatusRow.UnassignableFlag
                        && (AppStatusRow.IsUnassignableDateNull()
                            || (AppStatusRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Application Status' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmGeneralApplicationTable.GetGenApplicationStatusDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.GenApplicationStatus })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // following validation only relevant for event applications
            if (AEventApplication)
            {
                // 'Organization Contact 1' must not be unassignable
                ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenContact1Id];

                if (true)
                {
                    PtContactTable ContactTable;
                    PtContactRow ContactRow;

                    VerificationResult = null;

                    if ((!ARow.IsGenContact1Null())
                        && (ARow.GenContact1 != String.Empty))
                    {
                        ContactTable = (PtContactTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                            TCacheablePersonTablesEnum.ContactList);
                        ContactRow = (PtContactRow)ContactTable.Rows.Find(ARow.GenContact1);

                        // 'Contact' must not be unassignable
                        if ((ContactRow != null)
                            && ContactRow.UnassignableFlag
                            && (ContactRow.IsUnassignableDateNull()
                                || (ContactRow.UnassignableDate <= DateTime.Today)))
                        {
                            // if 'Contact' is unassignable then check if the value has been changed or if it is a new record
                            if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmGeneralApplicationTable.GetGenContact1DBName()))
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                            new string[] { String.Empty, ARow.GenContact1 })),
                                    ValidationColumn);
                            }
                        }
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }

                // 'Organization Contact 2' must not be unassignable
                ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenContact2Id];

                if (true)
                {
                    PtContactTable ContactTable;
                    PtContactRow ContactRow;

                    VerificationResult = null;

                    if ((!ARow.IsGenContact2Null())
                        && (ARow.GenContact2 != String.Empty))
                    {
                        ContactTable = (PtContactTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                            TCacheablePersonTablesEnum.ContactList);
                        ContactRow = (PtContactRow)ContactTable.Rows.Find(ARow.GenContact2);

                        // 'Contact' must not be unassignable
                        if ((ContactRow != null)
                            && ContactRow.UnassignableFlag
                            && (ContactRow.IsUnassignableDateNull()
                                || (ContactRow.UnassignableDate <= DateTime.Today)))
                        {
                            // if 'Contact' is unassignable then check if the value has been changed or if it is a new record
                            if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmGeneralApplicationTable.GetGenContact2DBName()))
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                            new string[] { String.Empty, ARow.GenContact2 })),
                                    ValidationColumn);
                            }
                        }
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
                }
            }

            // following validation only relevant for field applications
            if (!AEventApplication)
            {
                // Field Application: 'Field' must be a Partner of Class 'UNIT' and must not be 0 and not be null
                ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenAppPossSrvUnitKeyId];

                if (true)
                {
                    if (ARow.IsGenAppPossSrvUnitKeyNull())
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOTNULL,
                                    new string[] { String.Empty })),
                            ValidationColumn);
                    }
                    else
                    {
                        VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                            ARow.GenAppPossSrvUnitKey, false, THelper.NiceValueDescription(
                                String.Empty) + " must be set correctly.",
                            AContext, ValidationColumn);
                    }

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }
            }

            // 'Cancellation date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenAppCancelledId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.GenAppCancelled, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Accepted by sending field date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenAppSendFldAcceptDateId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.GenAppSendFldAcceptDate, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Accepted by receiving field date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmGeneralApplicationTable.ColumnGenAppRecvgFldAcceptId];

            if (true)
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.IsCurrentOrPastDate(ARow.GenAppRecvgFldAccept, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }

        /// <summary>
        /// Validates the event (short term) application record of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateEventApplicationManual(object AContext, PmShortTermApplicationRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Event' must be a Partner of Class 'UNIT' and must not be 0
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnStConfirmedOptionId];

            if (true)
            {
                if (ARow.IsStConfirmedOptionNull())
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PARTNERKEY_INVALID_NOTNULL,
                                new string[] { String.Empty })),
                        ValidationColumn);
                }
                else
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                        ARow.StConfirmedOption, false, THelper.NiceValueDescription(
                            String.Empty) + " must be set correctly.",
                        AContext, ValidationColumn);
                }

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Charged Field' must be a Partner of Class 'UNIT'
            //
            // HOWEVER, 'null' is a perfectly valid value for 'Charged Field' (according to WolfgangB).
            // If it is null then we must not call TSharedPartnerValidation_Partner.IsValidUNITPartner
            // as the attempt to retrieve 'ARow.StFieldCharged' would result in
            // 'System.Data.StrongTypingException("Error: DB null", null)'!!!
            if (!ARow.IsStFieldChargedNull())
            {
                ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnStFieldChargedId];

                if (true)
                {
                    VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                        ARow.StFieldCharged, true, THelper.NiceValueDescription(
                            String.Empty) + " must be set correctly.",
                        AContext, ValidationColumn);

                    // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                    // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                    // ResultText!
                    AVerificationResultCollection.Remove(ValidationColumn);
                    AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
                }
            }

            // 'Arrival Method' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnTravelTypeToCongCodeId];

            if (true)
            {
                PtTravelTypeTable TravelTypeTable;
                PtTravelTypeRow TravelTypeRow = null;

                VerificationResult = null;

                if (!ARow.IsTravelTypeToCongCodeNull())
                {
                    TravelTypeTable = (PtTravelTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.TransportTypeList);
                    TravelTypeRow = (PtTravelTypeRow)TravelTypeTable.Rows.Find(ARow.TravelTypeToCongCode);

                    // 'Arrival Method' must not be unassignable
                    if ((TravelTypeRow != null)
                        && TravelTypeRow.UnassignableFlag
                        && (TravelTypeRow.IsUnassignableDateNull()
                            || (TravelTypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Arrival Method' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmShortTermApplicationTable.GetTravelTypeToCongCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.TravelTypeToCongCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Departure Method' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnTravelTypeFromCongCodeId];

            if (true)
            {
                PtTravelTypeTable TravelTypeTable;
                PtTravelTypeRow TravelTypeRow = null;

                VerificationResult = null;

                if (!ARow.IsTravelTypeFromCongCodeNull())
                {
                    TravelTypeTable = (PtTravelTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate(
                        TCacheablePersonTablesEnum.TransportTypeList);
                    TravelTypeRow = (PtTravelTypeRow)TravelTypeTable.Rows.Find(ARow.TravelTypeFromCongCode);

                    // 'Departure Method' must not be unassignable
                    if ((TravelTypeRow != null)
                        && TravelTypeRow.UnassignableFlag
                        && (TravelTypeRow.IsUnassignableDateNull()
                            || (TravelTypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Departure Method' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmShortTermApplicationTable.GetTravelTypeFromCongCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.TravelTypeFromCongCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Departure Date' must be later than 'Arrival Date'
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnDepartureId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.Departure, ARow.Arrival,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Arrival Hour' must be between 0 and 24
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnArrivalHourId];

            if (true)
            {
                VerificationResult = TNumericalChecks.IsInRange(ARow.ArrivalHour, 0, 24,
                    Catalog.GetString("Arrival Hour"),
                    AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Arrival Minute' must be between 0 and 59
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnArrivalMinuteId];

            if (true)
            {
                VerificationResult = TNumericalChecks.IsInRange(ARow.ArrivalMinute, 0, 59,
                    Catalog.GetString("Arrival Minute"),
                    AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Departure Hour' must be between 0 and 24
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnDepartureHourId];

            if (true)
            {
                VerificationResult = TNumericalChecks.IsInRange(ARow.DepartureHour, 0, 24,
                    Catalog.GetString("Departure Hour"),
                    AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Departure Minute' must be between 0 and 59
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnDepartureMinuteId];

            if (true)
            {
                VerificationResult = TNumericalChecks.IsInRange(ARow.DepartureMinute, 0, 59,
                    Catalog.GetString("Departure Minute"),
                    AContext, ValidationColumn);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Arrival Point' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnArrivalPointCodeId];

            if (true)
            {
                PtArrivalPointTable ArrivalPointTable;
                PtArrivalPointRow ArrivalPointRow;

                VerificationResult = null;

                if ((!ARow.IsArrivalPointCodeNull())
                    && (ARow.ArrivalPointCode != String.Empty))
                {
                    ArrivalPointTable = (PtArrivalPointTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                        TCacheablePersonTablesEnum.ArrivalDeparturePointList);
                    ArrivalPointRow = (PtArrivalPointRow)ArrivalPointTable.Rows.Find(ARow.ArrivalPointCode);

                    // 'Arrival Point' must not be unassignable
                    if ((ArrivalPointRow != null)
                        && ArrivalPointRow.UnassignableFlag
                        && (ArrivalPointRow.IsUnassignableDateNull()
                            || (ArrivalPointRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Contact' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmShortTermApplicationTable.GetArrivalPointCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.ArrivalPointCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            // 'Departure Point' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmShortTermApplicationTable.ColumnDeparturePointCodeId];

            if (true)
            {
                PtArrivalPointTable ArrivalPointTable;
                PtArrivalPointRow ArrivalPointRow;

                VerificationResult = null;

                if ((!ARow.IsDeparturePointCodeNull())
                    && (ARow.DeparturePointCode != String.Empty))
                {
                    ArrivalPointTable = (PtArrivalPointTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                        TCacheablePersonTablesEnum.ArrivalDeparturePointList);
                    ArrivalPointRow = (PtArrivalPointRow)ArrivalPointTable.Rows.Find(ARow.DeparturePointCode);

                    // 'Arrival Point' must not be unassignable
                    if ((ArrivalPointRow != null)
                        && ArrivalPointRow.UnassignableFlag
                        && (ArrivalPointRow.IsUnassignableDateNull()
                            || (ArrivalPointRow.UnassignableDate <= DateTime.Today)))
                    {
                        // if 'Arrival Point' is unassignable then check if the value has been changed or if it is a new record
                        if (TSharedValidationHelper.IsRowAddedOrFieldModified(ARow, PmShortTermApplicationTable.GetDeparturePointCodeDBName()))
                        {
                            VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,
                                        new string[] { String.Empty, ARow.DeparturePointCode })),
                                ValidationColumn);
                        }
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }

            //TODO: if arrival   hour == 24 then arrival   minute must be 0
            //TODO: if departure hour == 24 then departure minute must be 0

            //TODO: make sure that no other application already exists for this event and this person
        }

        /// <summary>
        /// Validates the field (long term) application record of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <returns>void</returns>
        public static void ValidateFieldApplicationManual(object AContext, PmYearProgramApplicationRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'Available to' must be later than 'Available from' date
            ValidationColumn = ARow.Table.Columns[PmYearProgramApplicationTable.ColumnEndOfCommitmentId];

            if (true)
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.EndOfCommitment, ARow.StartOfCommitment,
                    String.Empty, String.Empty,
                    AContext, ValidationColumn);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }
    }
}
