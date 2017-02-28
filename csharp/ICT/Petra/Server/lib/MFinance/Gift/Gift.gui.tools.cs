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
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
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
        /// MotivationDetail. In normal cases the global default values will be used.
        /// But in some cases "PartnerKey"-specific defaults are used instead.
        ///
        /// This Routine
        /// 1. Checks if PartnerKey is available in p_partner
        /// 2. If PartnerKey is not available the result PartnerKeyIsValid = false
        ///   and the other parameters are not changed.
        /// 3. If PartnerKey is available then PartnerKeyIsValid = true and:
        /// 4. The Table-Entry p_partner_class_c is checked for the value "unit"
        /// 5. If p_partner_class_c does not hold the value unit, then the routine is done;
        ///   the other parameters are not changed
        /// 6. If p_partner_class_c holds the value "unit" then the table p_unit is checked.
        /// 7. If p_unit.p_partner_class_c holds the value "KEYMIN" then MotivationDetail
        ///    is changed to KEY-MIN.
        /// 8. If p_unit.p_partner_class_c does not hold the value "KEYMIN" the routine is done.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AMotivationGroup">Output: Probably set to GIFT</param>
        /// <param name="AMotivationDetail">Input: default value; unlikely to be used!
        ///                               Output: value depending on APartnerKey. </param>
        /// <returns>true if parther key is valid</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetMotivationGroupAndDetailForPartner(Int64 APartnerKey,
            ref String AMotivationGroup,
            ref String AMotivationDetail)
        {
            Boolean PartnerKeyIsValid = false;

            if (APartnerKey != 0)
            {
                string motivationGroup = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                string motivationDetail = AMotivationDetail;

                TDBTransaction readTransaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref readTransaction,
                    delegate
                    {
                        PPartnerTable myPPartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, readTransaction);

                        if (myPPartnerTable.Rows.Count == 1)
                        {
                            // partnerKey is valid
                            PartnerKeyIsValid = true;
                            PPartnerRow partnerRow = myPPartnerTable[0];

                            // Change motivationDetail if PartnerClass is UNIT
                            if (partnerRow.PartnerClass.Equals(MPartnerConstants.PARTNERCLASS_UNIT))
                            {
                                // AND KEY-MIN
                                bool KeyMinFound = false;

                                // first check if a specific motivation detail is linked to this partner
                                AMotivationDetailTable MotivationDetailTable
                                    = AMotivationDetailAccess.LoadViaPPartner(APartnerKey, readTransaction);

                                if ((MotivationDetailTable != null) && (MotivationDetailTable.Rows.Count > 0))
                                {
                                    foreach (AMotivationDetailRow Row in MotivationDetailTable.Rows)
                                    {
                                        if (Row.MotivationStatus)
                                        {
                                            motivationGroup = MotivationDetailTable[0].MotivationGroupCode;
                                            motivationDetail = MotivationDetailTable[0].MotivationDetailCode;

                                            KeyMinFound = true;
                                            break; // Go with the first entry found.
                                        }
                                    }
                                }

                                if (!KeyMinFound)
                                {
                                    // Is this is a key min, or a field?
                                    PUnitTable pUnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, readTransaction);

                                    if (pUnitTable.Rows.Count == 1)
                                    {
                                        PUnitRow unitRow = pUnitTable[0];

                                        switch (unitRow.UnitTypeCode)
                                        {
                                            case MPartnerConstants.UNIT_TYPE_AREA:
                                            case MPartnerConstants.UNIT_TYPE_FUND:
                                            case MPartnerConstants.UNIT_TYPE_FIELD:
                                                motivationDetail = MFinanceConstants.GROUP_DETAIL_FIELD;
                                                break;

                                            case MPartnerConstants.UNIT_TYPE_KEYMIN:
                                                motivationDetail = MFinanceConstants.GROUP_DETAIL_KEY_MIN;
                                                break;

                                            case MPartnerConstants.UNIT_TYPE_COUNTRY:
                                            case MPartnerConstants.UNIT_TYPE_CONFERENCE:
                                            case MPartnerConstants.UNIT_TYPE_OTHER:
                                            case MPartnerConstants.UNIT_TYPE_ROOT:
                                            case MPartnerConstants.UNIT_TYPE_TEAM:
                                            case MPartnerConstants.UNIT_TYPE_WORKING_GROUP:
                                            default:
                                                motivationDetail = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                motivationDetail = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                            }
                        }
                    });

                AMotivationGroup = motivationGroup;
                AMotivationDetail = motivationDetail;
            }

            return PartnerKeyIsValid;
        }
    }
}