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
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner.Validation
{
    /// <summary>
    /// Contains functions for the validation of MPartner Partner DataTables.
    /// </summary>
    public static partial class TSharedPartnerValidation_Partner
    {
        /// <summary>BIC/SWIFT Code entered.</summary>
        [ErrCodeAttribute("BIC/SWIFT Code entered.")]
        public const String ERR_BRANCHCODELIKEBIC = "PARTN.00005V";

        /// <summary>todoComment</summary>
        private static readonly string StrBICSwiftCodeInvalid = Catalog.GetString(
            "The BIC / Swift code you entered for this bank is invalid!" + "\r\n" + "\r\n" +
            "  Here is the format of a valid BIC: 'BANKCCLL' or 'BANKCCLLBBB'." + "\r\n" +
            "    BANK = Bank Code. This code identifies the bank world wide." + "\r\n" +
            "    CC   = Country Code. This is the ISO country code of the country the bank is in.\r\n" +
            "    LL   = Location Code. This code gives the town where the bank is located." + "\r\n" +
            "    BBB  = Branch Code. This code denotes the branch of the bank." + "\r\n" +
            "  BICs have either 8 or 11 characters." + "\r\n");

        /// <summary>todoComment</summary>
        private static readonly string StrBranchCodeLikeBIC = "The {0} you entered seems to be a BIC/SWIFT Code!\r\n\r\n" +
                                                              "Make sure that you have entered the BIC/SWIFT Code in the BIC/SWIFT Code field\r\n" +
                                                              "and that the information you entered in the {1} field is actually\r\nthe {2}";

        /// <summary>todoComment</summary>
        private static readonly string StrBranchCodeLikeBICTitle = " seems to be a BIC/SWIFT Code";
        
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
        public static void ValidatePartnerBankDetailsManual(object AContext, PBankRow ARow, 
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
        {            
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult;
            
            // 'BIC' (Bank Identifier Code) must be valid
            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBicId];
            
            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (CommonRoutines.CheckBIC(ARow.Bic) == false)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult("",
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid)), 
                        ValidationColumn, ValidationControlsData.ValidationControl);

//                    VerificationResult = new TVerificationResult("",
//                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid));
                }
                else
                {
                    VerificationResult = null;
                }
                
                
//                VerificationResult = TNumericalChecks.IsPositiveOrZeroInteger(ARow.InternatTelephoneCode,
//                    ValidationControlsData.ValidationControlLabel,
//                    AContext, ValidationColumn, ValidationControlsData.ValidationControl);
                
                // Handle addition/removal to/from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
            

//            // 'Branch Code' must not have the format of a BIC
//            ValidationColumn = ARow.Table.Columns[PBankTable.ColumnBranchCodeId];
//            
//            if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
//            {   
//                if (CommonRoutines.CheckBIC(ARow.BranchCode) == false)
//                {
////                    AVerificationResult = new TVerificationResult("",
////                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid));
//
////                    TMessages.MsgGeneralWarning(String.Format(StrBranchCodeLikeBIC, BranchCodeLocal, BranchCodeLocal, BranchCodeLocal) + "!",
////                        BranchCodeLocal + StrBranchCodeLikeBICTitle, ERR_BRANCHCODELIKEBIC, null);
//
//                }
//                else
//                {
//                    AVerificationResult = null;
//                }                
////                VerificationResult = TNumericalChecks.FirstLesserOrEqualThanSecondDecimal(
////                        ARow.TimeZoneMinimum, ARow.TimeZoneMaximum,
////                        ValidationControlsData.ValidationControlLabel, ValidationControlsData.SecondValidationControlLabel,
////                        AContext, ValidationColumn, ValidationControlsData.ValidationControl);
//                
//                // Handle addition to/removal from TVerificationResultCollection
//                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
//            }
        }
    }    
}
