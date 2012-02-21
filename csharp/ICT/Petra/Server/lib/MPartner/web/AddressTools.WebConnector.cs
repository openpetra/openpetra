//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Extracts;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Common;

namespace Ict.Petra.Server.MPartner.Mailing.WebConnectors
{
    ///<summary>
    /// useful functions for the address of a partner
    ///</summary>
    public class TAddressWebConnector
    {
        /// find the current best address for the partner
        [RequireModulePermission("PTNRUSER")]
        public static bool GetBestAddress(Int64 APartnerKey,
            out PLocationTable AAddress,
            out PPartnerLocationTable APartnerLocation,
            out string ACountryNameLocal,
            out string AEmailAddress)
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            bool ResultValue = false;

            try
            {
                ResultValue = TAddressTools.GetBestAddress(APartnerKey,
                    out AAddress,
                    out APartnerLocation,
                    out ACountryNameLocal,
                    out AEmailAddress,
                    Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return ResultValue;
        }

        /// <summary>
        /// get the best postal address of the partners and return in the result table;
        /// you have to check the ValidAddress flag on the result table
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static BestAddressTDSLocationTable AddPostalAddress(ref DataTable APartnerTable,
            DataColumn APartnerKeyColumn,
            bool AIgnoreForeignAddresses)
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);
            BestAddressTDSLocationTable ResultTable = null;

            try
            {
                ResultTable = TAddressTools.AddPostalAddress(APartnerTable,
                    APartnerKeyColumn,
                    AIgnoreForeignAddresses,
                    false,
                    Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return ResultTable;
        }

        /// <summary>
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AExtractAlreadyExists">True if there is already an extract with
        /// the given name, otherwise false.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="ABestAddressTable"></param>
        /// <param name="AIncludeNonValidAddresses">you might want to include invalid addresses if an email was sent</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool CreateExtractFromBestAddressTable(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out Boolean AExtractAlreadyExists,
            out TVerificationResultCollection AVerificationResults,
            BestAddressTDSLocationTable ABestAddressTable,
            bool AIncludeNonValidAddresses)
        {
            return TExtractsHandling.CreateExtractFromBestAddressTable(
                AExtractName,
                AExtractDescription,
                out ANewExtractId,
                out AExtractAlreadyExists,
                out AVerificationResults,
                ABestAddressTable,
                AIncludeNonValidAddresses);
        }
    }
}