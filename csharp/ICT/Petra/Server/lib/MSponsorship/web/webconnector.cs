//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MSponsorship.Data;

namespace Ict.Petra.Server.MSponsorship.WebConnectors
{

    /// <summary>
    /// webconnector for the sponsorship module
    /// </summary>
    public class TSponsorshipWebConnector
    {
        private const string TYPE_SPONSOREDCHILD = "SPONSOREDCHILD";

        /// <summary>
        /// find children using filters
        /// </summary>
        [RequireModulePermission("OR(SPONSORVIEW,SPONSORADMIN)")]
        public static SponsorshipFindTDSSearchResultTable FindChildren(
            string AFirstName,
            string AFamilyName,
            string APartnerStatus,
            string ASponsorshipStatus,
            string ASponsorAdmin)
        {
            TDBTransaction t = new TDBTransaction();
            TDataBase db = DBAccess.Connect("FindChildren");
            string sql = "SELECT p.p_partner_short_name_c, p.p_status_code_c, p.p_partner_key_n, p.p_user_id_c, " +
                "f.p_first_name_c, f.p_family_name_c " +
                "FROM PUB_p_partner p, PUB_p_family f, PUB_p_partner_type t " +
                "WHERE p.p_partner_key_n = f.p_partner_key_n " +
                "AND p.p_partner_key_n = t.p_partner_key_n " +
                "AND t.p_type_code_c = '" + TYPE_SPONSOREDCHILD + "'";

            int CountParameters = 0;
            CountParameters += (AFirstName != String.Empty ? 1: 0);
            OdbcParameter[] parameters = new OdbcParameter[CountParameters];

            if (AFirstName != String.Empty)
            {
                sql += " AND f.p_first_name_c LIKE ?";
                parameters[0] = new OdbcParameter("FirstName", OdbcType.VarChar);
                parameters[0].Value = AFirstName;
            }

            SponsorshipFindTDSSearchResultTable result = new SponsorshipFindTDSSearchResultTable();

            db.ReadTransaction(ref t,
                delegate
                {
                    db.SelectDT(result, sql, t, parameters);
                });

            db.CloseDBConnection();

            return result;
        }
    }
}

