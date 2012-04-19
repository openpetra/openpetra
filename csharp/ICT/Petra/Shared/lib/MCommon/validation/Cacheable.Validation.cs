//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Shared.MCommon.Validation
{
    /// <summary>
    /// Contains functions for the validation of Cacheable DataTables.
    /// </summary>
    public static partial class TSharedValidation_CacheableDataTables
    {
        /// <summary>
        /// Validates the Setup Countries screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateCountrySetupManual(object AContext, PCountryRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'International Telephone Code' must be positive or 0
            ValidationColumn = ARow.Table.Columns[PCountryTable.ColumnInternatTelephoneCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.IsPositiveOrZeroInteger(ARow.InternatTelephoneCode,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'Time Zone From' must be <= 'Time Zone To'
            ValidationColumn = ARow.Table.Columns[PCountryTable.ColumnTimeZoneMinimumId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal(
                    ARow.TimeZoneMinimum, ARow.TimeZoneMaximum,
                    ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // 'International Postal Type' must be in 'p_international_postal_type' DB Table (this DB Table is not a Cacheable DataTable)
            ValidationColumn = ARow.Table.Columns[PCountryTable.ColumnInternatPostalTypeCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TSharedCommonValidation.IsValidInternationalPostalCode(ARow.InternatPostalTypeCode,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the Setup International Postal Type screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateInternationalPostalTypeSetup(object AContext, PInternationalPostalTypeRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;

            // 'Description' must have a value
            ValidationColumn = ARow.Table.Columns[PInternationalPostalTypeTable.ColumnDescriptionId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.Description,
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MPartner Marital Status screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateMaritalStatus(object AContext, PtMaritalStatusRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // 'AssignableDate' must not be empty if the flag is set
            ValidationColumn = ARow.Table.Columns[PtMaritalStatusTable.ColumnAssignableDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.AssignableFlag)
                {
                    VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.AssignableDate,
                        ValidationControlsData.ValidationControlLabel,
                        true, AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MPartner Relation Category screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateRelationCategory(object AContext, PRelationCategoryRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // 'UnssignableDate' must not be empty if the flag is set
            ValidationColumn = ARow.Table.Columns[PtMaritalStatusTable.ColumnAssignableDateId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.UnassignableFlag)
                {
                    VerificationResult = TDateChecks.IsNotUndefinedDateTime(ARow.UnassignableDate,
                        ValidationControlsData.ValidationControlLabel,
                        true, AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MCommon Local Data Field Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        public static void ValidateLocalDataFieldSetup(object AContext, PDataLabelRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // The added column at the end of the table, which is a concatenated string of checkedListBox entries, must not be empty
            ValidationColumn = ARow.Table.Columns[ARow.Table.Columns.Count - 1];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow[ARow.Table.Columns.Count - 1].ToString(),
                    ValidationControlsData.ValidationControlLabel,
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection.  In this case we ignore ResultText because it will have been over-ridden by the form
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, false, true);
            }

            // If the 'DataType' is 'lookup' then categoryCode cannot be empty string (which would indicate no entries in the DataLabelCategory DB table)
            VerificationResult = null;
            ValidationColumn = ARow.Table.Columns[PDataLabelTable.ColumnLookupCategoryCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (String.Compare(ARow.DataType, "lookup", true) == 0)
                {
                    VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.LookupCategoryCode,
                        ValidationControlsData.ValidationControlLabel,
                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                    VerificationResult.OverrideResultText(Catalog.GetString(
                        "You cannot use the option list until you have defined at least one option using the 'Local Data Option List Names' main menu selection"));
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }
    }
}