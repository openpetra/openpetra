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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Gui;

namespace Ict.Petra.Client.MPartner.Verification
{
    /// <summary>
    /// Contains verification logic for the UC_PartnerAddress UserControl.
    /// </summary>
    public class TPartnerDetailsBankVerification
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

        #region TPartnerDetailsBankVerification

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean VerifyBankDetailsData(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            Boolean ReturnValue;

            AVerificationResult = null;

            if (e.Column.ColumnName == PBankTable.GetBicDBName())
            {
                VerifyBICSwiftCode(e, out AVerificationResult);
            }

            if (e.Column.ColumnName == PBankTable.GetBranchCodeDBName())
            {
                CheckBranchCodeIsNotBICSwiftCode(e);
            }

            // any verification errors?
            if (AVerificationResult == null)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;

                // MessageBox.Show('VerifyBankDetailsData: There was an error!');
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyBICSwiftCode(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            if (CommonRoutines.CheckBIC(e.ProposedValue.ToString()) == false)
            {
                AVerificationResult = new TVerificationResult("",
                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_BANKBICSWIFTCODEINVALID, StrBICSwiftCodeInvalid));
            }
            else
            {
                AVerificationResult = null;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        public static void CheckBranchCodeIsNotBICSwiftCode(DataColumnChangeEventArgs e)
        {
            String Dummy;
            String Dummy2;
            String BranchCodeLocal;

            if (e.ProposedValue.ToString() != String.Empty)
            {
                if (CommonRoutines.CheckBIC(e.ProposedValue.ToString()) == true)
                {
                    LocalisedStrings.GetLocStrBankBranchCode(out Dummy, out Dummy2, out BranchCodeLocal);

                    TMessages.MsgGeneralWarning(String.Format(StrBranchCodeLikeBIC, BranchCodeLocal, BranchCodeLocal, BranchCodeLocal) + "!",
                        BranchCodeLocal + StrBranchCodeLikeBICTitle, ERR_BRANCHCODELIKEBIC, null);
                }
            }
        }

        #endregion
    }
}