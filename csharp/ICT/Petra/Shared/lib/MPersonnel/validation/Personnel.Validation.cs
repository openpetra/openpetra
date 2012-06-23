//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2010 by OM International
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
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateCommitmentManual(object AContext, PmStaffDataRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Receiving Field' must be a Partner of Class 'UNIT' and must not be 0
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnReceivingFieldId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                    ARow.ReceivingField, false, THelper.NiceValueDescription(
                        ValidationControlsData.ValidationControlLabel) + " must be set correctly.",
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Home Office' must be a Partner of Class 'UNIT'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnHomeOfficeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(ARow.HomeOffice, false,
                    THelper.NiceValueDescription(ValidationControlsData.ValidationControlLabel) + " must be set correctly.",
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Recruiting Office' must be a Partner of Class 'UNIT'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnOfficeRecruitedById];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(ARow.OfficeRecruitedBy, false,
                    THelper.NiceValueDescription(ValidationControlsData.ValidationControlLabel) + " must be set correctly.",
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Start of Commitment' must be defined
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnStartOfCommitmentId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.StartOfCommitment,
                    ValidationControlsData.ValidationControlLabel, true, AContext, ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'End of Commitment' must be later than 'Start of Commitment'
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnEndOfCommitmentId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.EndOfCommitment, ARow.StartOfCommitment,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Status' must have a value
            ValidationColumn = ARow.Table.Columns[PmStaffDataTable.ColumnStatusCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.StatusCode,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Job Assignment data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateJobAssignmentManual(object AContext, PmJobAssignmentRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'From Date' must be defined
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnFromDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.FromDate,
                    ValidationControlsData.ValidationControlLabel, true, AContext, ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'To Date' must be later than 'From Date'
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnToDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.ToDate, ARow.FromDate,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Unit' must be a Partner of Class 'UNIT' and must not be 0
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnUnitKeyId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedPartnerValidation_Partner.IsValidUNITPartner(
                    ARow.UnitKey, false, THelper.NiceValueDescription(
                        ValidationControlsData.ValidationControlLabel) + " must be set correctly.",
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Since the validation can result in different ResultTexts we need to remove any validation result manually as a call to
                // AVerificationResultCollection.AddOrRemove wouldn't remove a previous validation result with a different
                // ResultText!
                AVerificationResultCollection.Remove(ValidationColumn);
                AVerificationResultCollection.AddAndIgnoreNullValue(VerificationResult);
            }

            // 'Assignment Type' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnAssignmentTypeCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.AssignmentTypeCode })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Position' must be not be null and not unassignable
            ValidationColumn = ARow.Table.Columns[PmJobAssignmentTable.ColumnPositionNameId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.PositionName })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }
                else
                {
                    // Position name must not be null
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.PositionName,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Passport data of a Partner.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePassportManual(object AContext, PmPassportDetailsRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Passport Number' must have a value
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnPassportNumberId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.PassportNumber,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Expiry Date' must be later than 'Issue Date'
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnDateOfExpirationId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.DateOfExpiration, ARow.DateOfIssue,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Passport Type' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPassportDetailsTable.ColumnPassportDetailsTypeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.PassportDetailsType })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the personal document data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePersonalDocumentManual(object AContext, PmDocumentRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;
            PmDocumentTypeTable DocTypeTable;
            PmDocumentTypeRow DocTypeRow = null;

            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDocCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if ((!ARow.IsDocCodeNull())
                    && (ARow.DocCode != String.Empty))
                {
                    DocTypeTable = (PmDocumentTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(
                        TCacheablePersonTablesEnum.DocumentTypeList);
                    DocTypeRow = (PmDocumentTypeRow)DocTypeTable.Rows.Find(ARow.DocCode);

                    // 'Document Code' must not be unassignable
                    if ((DocTypeRow != null)
                        && DocTypeRow.UnassignableFlag
                        && (DocTypeRow.IsUnassignableDateNull()
                            || (DocTypeRow.UnassignableDate <= DateTime.Today)))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.DocCode })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }
                else
                {
                    // 'Document Code' must have a value
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.DocCode,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Document Id' must have a value
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDocumentIdId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.DocumentId,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Issue Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDateOfStartId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if (!ARow.IsDateOfIssueNull()
                    && (ARow.DateOfIssue > DateTime.Today))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_FUTUREDATE_ERROR,
                                new string[] { ValidationControlsData.ValidationControlLabel })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Expiry Date' must be later or equal 'Start Date'
            ValidationColumn = ARow.Table.Columns[PmDocumentTable.ColumnDateOfExpirationId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.DateOfExpiration, ARow.DateOfStart,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the personal language data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePersonalLanguageManual(object AContext, PmPersonLanguageRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;
            PtLanguageLevelTable LanguageLevelTable;
            PtLanguageLevelRow LanguageLevelRow = null;

            // 'Language Level' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonLanguageTable.ColumnLanguageLevelId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.LanguageLevel.ToString() })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the skill data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateSkillManual(object AContext, PmPersonSkillRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Skill Category' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonSkillTable.ColumnSkillCategoryCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.SkillCategoryCode })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Skill Level' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonSkillTable.ColumnSkillLevelId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
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
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING, new string[] { ARow.SkillLevel.ToString() })),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                }

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the previous experience data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidatePreviousExperienceManual(object AContext, PmPastExperienceRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Start Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnStartDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if (!ARow.IsStartDateNull()
                    && (ARow.StartDate > DateTime.Today))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_FUTUREDATE_ERROR,
                                new string[] { ValidationControlsData.ValidationControlLabel })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'End Date' must not be a future date
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnEndDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if (!ARow.IsEndDateNull()
                    && (ARow.EndDate > DateTime.Today))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_FUTUREDATE_ERROR,
                                new string[] { ValidationControlsData.ValidationControlLabel })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'End Date' must be later or equal 'Start Date'
            ValidationColumn = ARow.Table.Columns[PmPastExperienceTable.ColumnEndDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                VerificationResult = TDateChecks.FirstGreaterOrEqualThanSecondDate(ARow.EndDate, ARow.StartDate,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the progress report (evaluation) data of a Person.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <returns>void</returns>
        public static void ValidateProgressReportManual(object AContext, PmPersonEvaluationRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Evaluator' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluatorId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Evaluator,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Evaluation Date' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluationDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.EvaluationDate,
                    ValidationControlsData.ValidationControlLabel, true, AContext, ValidationColumn,
                    ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Next Evaluation Date' must have a value if evaluation type is not set to "Leaving"
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnNextEvaluationDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = null;

                if (ARow.EvaluationType != "Leaving")
                {
                    VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.NextEvaluationDate,
                        ValidationControlsData.ValidationControlLabel, true, AContext, ValidationColumn,
                        ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Report Type' must have a value
            ValidationColumn = ARow.Table.Columns[PmPersonEvaluationTable.ColumnEvaluationTypeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.EvaluationType,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }
    }
}