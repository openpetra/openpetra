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
    /// Contains verification logic for the UC_PartnerEdit_CollapsiblePart UserControl.
    /// </summary>
    public class TPartnerVerification : System.Object
    {
        #region Resourcetexts

        private static readonly string StrFundnameChange = Catalog.GetString(
            "This partner is a ledger.\r\n" +
            "Fund names can be changed only with the approval from the IFC.\r\n\r\n" +
            "Do you have the approval to change the fund name?");

        private static readonly string StrFundNameChangeTitle = Catalog.GetString("Fund Name Change Authorisation");

        private static readonly string StrFundNameChangeUndone = Catalog.GetString("Fund Name change has been undone.");

        /// <summary>Resourcetext: 'Missing Approval'</summary>
        public static readonly string StrFundNameChangeUndoneTitle = Catalog.GetString("Missing Approval");

        #endregion

        #region TPartnerVerification

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
                    if (AMainDS.PPartnerType != null)
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
                                    PetraErrorCodes.ERR_UNITNAMECHANGEUNDONE,
                                    TResultSeverity.Resv_Noncritical);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}