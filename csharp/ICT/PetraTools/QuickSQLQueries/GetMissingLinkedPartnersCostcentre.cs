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
using System.IO;
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
/// check which partners have a costcentre but noone from the family is linked to it, so that donations
/// would go to that local costcentre
/// </summary>
public class GetMissingLinkedPartnersCosCentre
{
    /// <summary>
    /// run the statements
    /// </summary>
    /// <param name="transaction"></param>
    public static void Run(TDBTransaction ATransaction)
    {
        // get all partners that have a cost centre, but the cost centre is not linked to a family
        string sqlcommand =
            "select pub_p_person.p_partner_key_n, pub_p_person.p_family_key_n, pub_p_person.p_first_name_c, pub_p_person.p_family_name_c " +
            "from pub_p_person, pub_a_cost_centre " +
            "where pub_p_person.p_partner_key_n = pub_a_cost_centre.a_cost_centre_code_c " +
            "and not exists (SELECT * from pub_a_valid_ledger_number where pub_a_valid_ledger_number.a_cost_centre_code_c = pub_a_cost_centre.a_cost_centre_code_c)";

        PPersonTable persons = new PPersonTable();

        DBAccess.GDBAccessObj.SelectDT((DataTable)persons, sqlcommand, ATransaction, null, -1, -1);

        // now check if any member at all of that family is linked to a cost centre
        foreach (PPersonRow person in persons.Rows)
        {
            PPersonRow templateRow = persons.NewRowTyped(false);

            if (AValidLedgerNumberAccess.CountViaPPartnerPartnerKey(person.FamilyKey, ATransaction) == 0)
            {
                Console.WriteLine("Problem with partner " + person.PartnerKey.ToString() + " " + person.FirstName + " " + person.FamilyName);
            }
        }
    }
}
}