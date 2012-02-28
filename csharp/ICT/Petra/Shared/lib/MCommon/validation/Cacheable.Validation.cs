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

using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;

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
    }
}
