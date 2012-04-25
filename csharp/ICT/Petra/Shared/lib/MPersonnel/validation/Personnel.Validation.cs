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
            	PtAssignmentTypeRow   TypeRow;
            		
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
            	PtPositionRow   PositionRow;
            		
                VerificationResult = null;

                if ((!ARow.IsPositionNameNull())
                    && (ARow.PositionName != String.Empty))
                {
                    PositionTable = (PtPositionTable)TSharedDataCache.TMPersonnel.GetCacheableUnitsTable(
                        TCacheableUnitTablesEnum.PositionList);
                	PositionRow = (PtPositionRow)PositionTable.Rows.Find(new object[] {ARow.PositionName, ARow.PositionScope});

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
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEEMPTY_ERROR, new string[] {  PmJobAssignmentTable.GetLabel(PmJobAssignmentTable.TableId, PmJobAssignmentTable.ColumnPositionNameId) })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
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
            TScreenVerificationResult VerificationResult;
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

                // Handle addition/removal to/from TVerificationResultCollection
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
            TScreenVerificationResult VerificationResult;
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
    }
}