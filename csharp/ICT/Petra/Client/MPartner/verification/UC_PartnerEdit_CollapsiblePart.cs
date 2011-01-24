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
using Ict.Common.Data; // Implicit reference
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MPartner.Verification
{
    /// <summary>
    /// Contains verification logic for the UC_PartnerAddress UserControl.
    /// </summary>
    public class TPartnerVerification : System.Object
    {
        /// <summary>todoComment</summary>
        public const String StrPartnerStatusNotMerged = "The Partner Status cannot be set to 'MERGED'" + " by the user -" + "\r\n" +
                                                        "this Partner Status is set only by the " + "Partner Merge function" + "\r\n" +
                                                        "for Partners that have been merged " +
                                                        "into another Partner!";

        /// <summary>todoComment</summary>
        public const String StrFundnameChange = "This partner is a ledger." + "\r\n" +
                                                "Fund names can be changed only with the approval from the IFC." + "\r\n" + "\r\n" +
                                                "Do you have the approval to change the fund name?";

        /// <summary>todoComment</summary>
        public const String StrFundNameChangeTitle = "Fund Name Change Authorisation";

        /// <summary>todoComment</summary>
        public const String StrFundNameChangeUndone = "Fund Name change has been undone.";

        /// <summary>todoComment</summary>
        public const String StrFundNameChangeUndoneTitle = "Missing Approval";

        #region TPartnerVerification

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean VerifyPartnerData(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            Boolean ReturnValue;

            AVerificationResult = null;

            if (e.Column.ColumnName == PPartnerTable.GetStatusCodeDBName())
            {
                VerifyPartnerStatus(e, out AVerificationResult);
            }

            // any verification errors?
            if (AVerificationResult == null)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;

                // MessageBox.Show('VerifyPartnerData: There was an error!');
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AMainDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static Boolean VerifyUnitData(DataColumnChangeEventArgs e, PartnerEditTDS AMainDS, out TVerificationResult AVerificationResult)
        {
            Boolean ReturnValue;

            AVerificationResult = null;

            if (e.Column.ColumnName == PUnitTable.GetUnitNameDBName())
            {
                VerifyUnitNameChange(e, AMainDS, out AVerificationResult);
            }

            // any verification errors?
            if (AVerificationResult == null)
            {
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;

                // MessageBox.Show('VerifyUnitData: There was an error!');
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AMainDS"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyUnitNameChange(DataColumnChangeEventArgs e, PartnerEditTDS AMainDS, out TVerificationResult AVerificationResult)
        {
            AVerificationResult = null;
            System.Windows.Forms.DialogResult ApprovalFromIFC;
            try
            {
                /*
                 * Check for an *edited* Partner of PartnerClass UNIT whether it has a
                 * Special Type 'LEDGER'
                 */
                if ((SharedTypes.PartnerClassStringToEnum(AMainDS.PPartner[0].PartnerClass) == TPartnerClass.UNIT)
                    && (AMainDS.PPartner[0].HasVersion(DataRowVersion.Original)))
                {
                    if (AMainDS.PPartnerType.Rows.Find(new Object[] { AMainDS.PPartner[0].PartnerKey, "LEDGER" }) != null)
                    {
                        ApprovalFromIFC = MessageBox.Show(StrFundnameChange,
                            StrFundNameChangeTitle,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);

                        if (ApprovalFromIFC == System.Windows.Forms.DialogResult.No)
                        {
                            AVerificationResult = new TVerificationResult("",
                                StrFundNameChangeUndone,
                                StrFundNameChangeUndoneTitle,
                                ErrorCodes.PETRAERRORCODE_UNITNAMECHANGEUNDONE,
                                TResultSeverity.Resv_Critical);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        /// <param name="AVerificationResult"></param>
        public static void VerifyPartnerStatus(DataColumnChangeEventArgs e, out TVerificationResult AVerificationResult)
        {
            if (e.ProposedValue.ToString() == SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED))
            {
                AVerificationResult = new TVerificationResult("",
                    StrPartnerStatusNotMerged,
                    Catalog.GetString("Invalid Data"),
                    ErrorCodes.PETRAERRORCODE_PARTNERSTATUSMERGEDCHANGEUNDONE,
                    TResultSeverity.Resv_Critical);
            }
            else
            {
                AVerificationResult = null;
            }
        }

        #endregion
    }
}