//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// general methods for use in partner module
    /// </summary>
    public class TPartnerWebConnector
    {
        /// <summary>
        /// adds partner to list of recently used partners for given use scenario
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ANewPartner"></param>
        /// <param name="ALastPartnerUse"></param>
        /// <returns>true if action was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool AddRecentlyUsedPartner(Int64 APartnerKey, TPartnerClass APartnerClass,
            Boolean ANewPartner, TLastPartnerUse ALastPartnerUse)
        {
            bool ResultValue = false;

            ResultValue = Server.MPartner.Partner.TRecentPartnersHandling.AddRecentlyUsedPartner
                              (APartnerKey, APartnerClass, ANewPartner, ALastPartnerUse);

            return ResultValue;
        }

        /// <summary>
        /// return the location key and site key for the best address for that partner
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static TLocationPK DetermineBestAddress(Int64 APartnerKey)
        {
            return ServerCalculations.DetermineBestAddress(APartnerKey);
        }

        /// <summary>
        /// performs database changes to move person from current (old) family to new family record
        /// </summary>
        /// <param name="APersonKey"></param>
        /// <param name="AOldFamilyKey"></param>
        /// <param name="ANewFamilyKey"></param>
        /// <param name="AProblemMessage"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if change of family completed successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool ChangeFamily(Int64 APersonKey, Int64 AOldFamilyKey, Int64 ANewFamilyKey,
            out String AProblemMessage, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = false;

            ResultValue = Server.MPartner.Partner.TFamilyHandling.ChangeFamily(APersonKey,
                AOldFamilyKey,
                ANewFamilyKey,
                out AProblemMessage,
                out AVerificationResult);

            return ResultValue;
        }

        /// <summary>
        /// get the correct bank partner, according to the sortcode/branchcode.
        /// if it does not exist yet, create a new bank partner with empty location
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 GetBankBySortCode(string ABranchCode)
        {
            string sqlFindBankBySortCode =
                String.Format("SELECT * FROM PUB_{0} WHERE {1}=?",
                    PBankTable.GetTableDBName(),
                    PBankTable.GetBranchCodeDBName());

            OdbcParameter param = new OdbcParameter("branchcode", OdbcType.VarChar);

            param.Value = ABranchCode;
            PBankTable bank = new PBankTable();
            DBAccess.GDBAccessObj.SelectDT(bank, sqlFindBankBySortCode, null, new OdbcParameter[] {
                    param
                }, -1, -1);

            if (bank.Count > 0)
            {
                return bank[0].PartnerKey;
            }
            else
            {
                // create new bank partner, with empty location
                PartnerEditTDS MainDS = new PartnerEditTDS();

                PPartnerRow newPartner = MainDS.PPartner.NewRowTyped();
                Int64 BankPartnerKey = TNewPartnerKey.GetNewPartnerKey(DomainManager.GSiteKey);
                TNewPartnerKey.SubmitNewPartnerKey(DomainManager.GSiteKey, BankPartnerKey, ref BankPartnerKey);
                newPartner.PartnerKey = BankPartnerKey;
                newPartner.PartnerShortName = "Bank " + ABranchCode;
                newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_BANK;
                MainDS.PPartner.Rows.Add(newPartner);

                PBankRow newBank = MainDS.PBank.NewRowTyped(true);
                newBank.PartnerKey = newPartner.PartnerKey;
                newBank.BranchCode = ABranchCode;
                newBank.BranchName = newPartner.PartnerShortName;
                MainDS.PBank.Rows.Add(newBank);

                PPartnerLocationRow partnerlocation = MainDS.PPartnerLocation.NewRowTyped(true);
                partnerlocation.SiteKey = DomainManager.GSiteKey;
                partnerlocation.PartnerKey = newPartner.PartnerKey;
                partnerlocation.DateEffective = DateTime.Now;
                partnerlocation.LocationType = "HOME";
                partnerlocation.SendMail = false;
                MainDS.PPartnerLocation.Rows.Add(partnerlocation);

                TVerificationResultCollection VerificationResult;

                if (PartnerEditTDSAccess.SubmitChanges(MainDS, out VerificationResult) == TSubmitChangesResult.scrOK)
                {
                    return newPartner.PartnerKey;
                }
            }

            throw new Exception("problem for GetBankBySortCode, cannot find or create bank");
        }
    }
}