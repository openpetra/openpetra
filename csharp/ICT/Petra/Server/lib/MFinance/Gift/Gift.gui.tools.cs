//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// Description of Class1.
    /// </summary>
    public class TGuiTools
    {
        /// <summary>
        /// The user can insert a recipient key manualy and so each key has to be checked for
        /// usability. Parallel to this request you can check the values of MotivationGroup and
        /// MotivationDetail. In normal cases the global defaul values shall be used.
        /// But in some cases the "PartnerKey"-specific defaults shall be used.
        ///
        /// This Routine
        /// 1. Checks if PartnerKey is available in p_partner
        /// 2. If PartnerKey is not available the result PartnerKeyIsValid = false and the other
        ///    parameter are not changed.
        /// 3. If PartnerKey is available then PartnerKeyIsValid = false and:
        /// 4. The Table-Entry p_partner_class_c is checked for the value "unit"
        /// 5. If p_partner_class_c does not hold the value unit, then the routine is done, the other
        ///    parameters are not changed
        /// 6. If p_partner_class_c holds the value "unit" then the table p_unit shall be checked.
        /// 7. If p_unit.p_partner_class_c holds the value "KEYMIN" then the value of MotivationDetail
        ///    shall be changed to KEY-MIN.
        /// 8. If p_unit.p_partner_class_c does not hold the value "KEYMIN" the routine is done.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AMotivationGroup">Output: Always set to GIFT</param>
        /// <param name="AMotivationDetail">Input: default value; unlikely to be used!
        ///                               Output: value depending on APartnerKey. </param>
        /// <returns>true if parther key is valid</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetMotivationGroupAndDetail(Int64 APartnerKey,
            ref String AMotivationGroup,
            ref String AMotivationDetail)
        {
            Boolean PartnerKeyIsValid = false;

            if (APartnerKey != 0)
            {
                string MotivationGroup = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                string MotivationDetail = AMotivationDetail;

                TDBTransaction readTransaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref readTransaction,
                    delegate
                    {
                        PPartnerTable myPPartnerTable = null;

                        myPPartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, readTransaction);

                        if (myPPartnerTable.Rows.Count == 1)
                        {
                            // Entry for partnerKey is valid
                            PartnerKeyIsValid = true;
                            PPartnerRow partnerRow = null;

                            partnerRow = (PPartnerRow)myPPartnerTable.Rows[0];

                            // Change motivationDetail if ColumnPartnerClass is UNIT
                            if (partnerRow.PartnerClass.Equals(MPartnerConstants.PARTNERCLASS_UNIT))
                            {
                                // AND KEY-MIN

                                bool KeyMinFound = false;

                                // first check if a motivation detail is linked to this potential key min
                                AMotivationDetailTable MotivationDetailTable = null;

                                MotivationDetailTable = AMotivationDetailAccess.LoadViaPPartner(APartnerKey, readTransaction);

                                if ((MotivationDetailTable != null) && (MotivationDetailTable.Rows.Count > 0))
                                {
                                    foreach (AMotivationDetailRow Row in MotivationDetailTable.Rows)
                                    {
                                        if (Row.MotivationStatus)
                                        {
                                            MotivationGroup = MotivationDetailTable[0].MotivationGroupCode;
                                            MotivationDetail = MotivationDetailTable[0].MotivationDetailCode;

                                            KeyMinFound = true;
                                            break;
                                        }
                                    }
                                }

                                // second check to see if this is a key min
                                if (!KeyMinFound)
                                {
                                    PUnitTable pUnitTable = null;

                                    pUnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, readTransaction);

                                    if (pUnitTable.Rows.Count == 1)
                                    {
                                        PUnitRow unitRow = null;

                                        unitRow = (PUnitRow)pUnitTable.Rows[0];

                                        if (unitRow.UnitTypeCode.Equals(MPartnerConstants.UNIT_TYPE_KEYMIN))
                                        {
                                            MotivationDetail = MFinanceConstants.GROUP_DETAIL_KEY_MIN;
                                        }
                                        else
                                        {
                                            MotivationDetail =
                                                TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_DEFAULTFIELDMOTIVATION,
                                                    MFinanceConstants.GROUP_DETAIL_FIELD);

                                            // if system default is empty then set to FIELD
                                            if (string.IsNullOrEmpty(MotivationDetail))
                                            {
                                                MotivationDetail = MFinanceConstants.GROUP_DETAIL_FIELD;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MotivationDetail = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                            }
                        }
                    });

                AMotivationGroup = MotivationGroup;
                AMotivationDetail = MotivationDetail;
            }

            return PartnerKeyIsValid;
        }
    }
}