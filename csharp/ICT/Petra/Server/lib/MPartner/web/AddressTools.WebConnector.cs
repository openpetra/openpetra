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
            out string ACountryNameLocal)
        {
            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            bool ResultValue = false;

            try
            {
                ResultValue = TAddressTools.GetBestAddress(APartnerKey,
                    out AAddress,
                    out ACountryNameLocal,
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
    }
}