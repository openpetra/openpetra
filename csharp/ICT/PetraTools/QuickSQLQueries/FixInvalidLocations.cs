/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.AR.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.AR.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;

namespace QuickSQLTests
{
/// <summary>
/// there is a location that was used as location 0 but has a different location key
/// </summary>
public class FixInvalidLocations
{
    /// <summary>
    /// run the statements
    /// </summary>
    /// <param name="transaction"></param>
    public static void Run(TDBTransaction ATransaction)
    {
        Int32 WrongLocationKey = 39005;

        WrongLocationKey = 51381;
        WrongLocationKey = 46283;

        // get all NOT ACTIVE partners that are linked to the location key that should be 0
        string sqlcommand =
            "select pub_p_partner.p_partner_key_n " +
            "from pub_p_partner, pub_p_partner_location " +
            "where pub_p_partner.p_status_code_c <> 'ACTIVE' " +
            "AND pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n " +
            "AND pub_p_partner_location.p_location_key_i = ?";

        PPartnerTable partners = new PPartnerTable();

        List <OdbcParameter>parameters = new List <OdbcParameter>();
        OdbcParameter param = new OdbcParameter("locationkey", OdbcType.Int);
        param.Value = WrongLocationKey;
        parameters.Add(param);

        DBAccess.GDBAccessObj.SelectDT((DataTable)partners, sqlcommand, ATransaction, parameters.ToArray(), -1, -1);

        DBAccess.GDBAccessObj.RollbackTransaction();

        ATransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

        Int32 counterFixed = 0;

        // reset p_partner_location record to location 0
        foreach (PPartnerRow partner in partners.Rows)
        {
            string updatestmt = "UPDATE pub_p_partner_location SET p_location_key_i = 0 WHERE p_partner_key_n = ? AND p_location_key_i = ?";

            parameters = new List <OdbcParameter>();
            param = new OdbcParameter("partnerkey", OdbcType.BigInt);
            param.Value = partner.PartnerKey;
            parameters.Add(param);
            param = new OdbcParameter("locationkey", OdbcType.Int);
            param.Value = WrongLocationKey;
            parameters.Add(param);

            Console.WriteLine("Fixing address for partner " + partner.PartnerKey.ToString());
            DBAccess.GDBAccessObj.ExecuteNonQuery(updatestmt, ATransaction, false, parameters.ToArray());

            counterFixed++;
        }

        DBAccess.GDBAccessObj.CommitTransaction();

        Console.WriteLine("Fixed " + counterFixed.ToString() + " partners");

        ATransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
    }
}
}