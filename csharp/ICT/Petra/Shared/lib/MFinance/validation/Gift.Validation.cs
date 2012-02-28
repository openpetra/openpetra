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
            TVerificationResult VerificationResult;
            object ValidationContext;            
            int VerifResultCollAddedCount = 0;
            
            // 'Batch Description' must not be empty
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnBatchDescriptionId];
			ValidationContext = ARow.BatchNumber;
            
            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.BatchDescription,
                    ValidationControlsData.ValidationControlLabel + " of Batch Number " + ValidationContext.ToString(),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                
                // Handle addition/removal to/from TVerificationResultCollection
                if(AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }
			
            
			// 'Exchange Rate' must be greater than 0
            ValidationColumn = ARow.Table.Columns[AGiftBatchTable.ColumnExchangeRateToBaseId];            
			ValidationContext = ARow.BatchNumber;

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {			
                VerificationResult = TNumericalChecks.IsPositiveDecimal(ARow.ExchangeRateToBase,
                    ValidationControlsData.ValidationControlLabel + " of Batch Number " + ValidationContext.ToString(),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                
                // Handle addition/removal to/from TVerificationResultCollection
                if(AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
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
            TVerificationResult VerificationResult;
            object ValidationContext;           
            int VerifResultCollAddedCount = 0;           

            // 'Gift Comment One' must not be empty
            ValidationColumn = ARow.Table.Columns[AGiftDetailTable.ColumnGiftCommentOneId];
			ValidationContext = ARow.BatchNumber.ToString() + ";" + ARow.GiftTransactionNumber.ToString();
            
            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {			
                VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.GiftCommentOne,
                    String.Format("{0} of Batch Number {1}, Gift Transaction Number {2}, Gift Detail {3}",
                    ValidationControlsData.ValidationControlLabel,
                    ARow.BatchNumber, ARow.GiftTransactionNumber, ARow.DetailNumber),
                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                
                // Handle addition/removal to/from TVerificationResultCollection
                if(AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn, true))
                {
                    VerifResultCollAddedCount++;
                }
            }
            
            return VerifResultCollAddedCount == 0;
        }        
    }    
}
