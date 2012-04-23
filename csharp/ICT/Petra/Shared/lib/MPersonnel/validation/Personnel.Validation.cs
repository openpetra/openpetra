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

namespace Ict.Petra.Shared.MPersonnel.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPersonnel Personnel DataTables.
    /// </summary>
    public static partial class TSharedPersonnelValidation_Personnel
    {
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
                
            	if (   (!ARow.IsDocCodeNull())
                    && (ARow.DocCode != String.Empty))
                {
		            DocTypeTable = (PmDocumentTypeTable)TSharedDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.DocumentTypeList);
                    DocTypeRow = (PmDocumentTypeRow)DocTypeTable.Rows.Find(ARow.DocCode);
                    
		            // 'Document Code' must not be unassignable
				    if (   DocTypeRow != null
                        && DocTypeRow.UnassignableFlag)
                    {
	                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
							ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,new string[] {ARow.DocCode})),
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

            // 'Language Code' must not be unassignable
            ValidationColumn = ARow.Table.Columns[PmPersonLanguageTable.ColumnLanguageCodeId];

            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
            	if ((   (!ARow.IsLanguageCodeNull())
                     && (ARow.LanguageCode != String.Empty))
                     && (ARow.LanguageCode == "IN"))
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext,
						ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_VALUEUNASSIGNABLE_WARNING,new string[] {"IN"})),
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
}