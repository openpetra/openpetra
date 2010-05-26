//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data.Odbc;
using Mono.Unix;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MPartner.Address.WebConnectors
{
    ///<summary>
    /// useful functions for the address of a partner
    ///</summary>
    public class TAddressWebConnector
    {
        /// find the current best address for the partner
        public static bool GetBestAddress(Int64 APartnerKey,
            out PLocationTable AAddress,
            out string ACountryNameLocal,
            out string AEmailAddress,
            TDBTransaction ATransaction)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            bool ResultValue = false;

            try
            {
                ResultValue = TAddressTools.GetBestAddress(APartnerKey,
                    out AAddress,
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
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            return ResultValue;
        }

        /// <summary>
        /// get the best postal address of the partners and return in the result table;
        /// you have to check the ValidAddress flag on the result table
        /// </summary>
        public static BestAddressTDSLocationTable AddPostalAddress(ref DataTable APartnerTable,
            DataColumn APartnerKeyColumn,
            bool AIgnoreForeignAddresses)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            BestAddressTDSLocationTable ResultTable = null;

            try
            {
                ResultTable = TAddressTools.AddPostalAddress(APartnerTable,
                    APartnerKeyColumn,
                    AIgnoreForeignAddresses,
                    Transaction);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultTable;
        }
    }
}