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
using System.Data.Odbc;
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
/// check if the commitment of the recipient reflects the recipient ledger number on the gift detail
/// </summary>
public class CheckGiftRecipientLedgerNumber
{
    /// <summary>
    /// run the statements
    /// </summary>
    /// <param name="transaction"></param>
    public static void Run(TDBTransaction ATransaction)
    {
        // get all partners that have a cost centre
        string sqlcommand =
            "select pub_p_person.p_partner_key_n, pub_p_person.p_family_key_n, pub_p_person.p_first_name_c, pub_p_person.p_family_name_c " +
            "from pub_p_person, pub_a_cost_centre " +
            "where pub_p_person.p_partner_key_n = pub_a_cost_centre.a_cost_centre_code_c";

        PPersonTable persons = new PPersonTable();

        DBAccess.GDBAccessObj.SelectDT((DataTable)persons, sqlcommand, ATransaction, null, -1, -1);

        // now check all gifts for that person in the given time period
        foreach (PPersonRow person in persons.Rows)
        {
            string sql2 = "SELECT DISTINCT pub_a_gift_detail.a_recipient_ledger_number_n " +
                          "from pub_a_gift_detail, pub_a_gift_batch " +
                          "where pub_a_gift_batch.a_ledger_number_i = 27 " +
                          "and pub_a_gift_batch.a_batch_status_c = 'Posted' " +
                          "and pub_a_gift_detail.a_ledger_number_i = 27 " +
                          "and pub_a_gift_detail.a_batch_number_i = pub_a_gift_batch.a_batch_number_i " +
                          "and pub_a_gift_detail.a_modified_detail_l = 0 " +
                          "and pub_a_gift_batch.a_gl_effective_date_d between ? and ? " +
                          "and pub_a_gift_detail.p_recipient_key_n = " + person.FamilyKey.ToString();

            List <OdbcParameter>parameters = new List <OdbcParameter>();
            OdbcParameter param = new OdbcParameter("datefrom", OdbcType.DateTime);
            param.Value = new DateTime(2010, 1, 1);
            parameters.Add(param);
            param = new OdbcParameter("dateto", OdbcType.DateTime);
            param.Value = new DateTime(2010, 1, 31);
            parameters.Add(param);

            DataTable ledgerNumbers = DBAccess.GDBAccessObj.SelectDT(sql2, "test", ATransaction, parameters.ToArray());

            if (ledgerNumbers.Rows.Count > 1)
            {
                foreach (DataRow row in ledgerNumbers.Rows)
                {
                    Console.WriteLine(person.FirstName + " " + person.FamilyName + " " + person.PartnerKey + " " + row[0].ToString());
                }
            }
            else if (ledgerNumbers.Rows.Count > 0)
            {
                // compare with current commitment
                string sqlCurrentCommitment =
                    "SELECT PUB_pm_staff_data.pm_target_field_n " +
                    "FROM PUB_pm_staff_data " +
                    "WHERE PUB_pm_staff_data.p_partner_key_n = " + person.PartnerKey.ToString() + " " +
                    "   AND (? BETWEEN PUB_pm_staff_data.pm_start_of_commitment_d AND PUB_pm_staff_data.pm_end_of_commitment_d " +
                    " OR (PUB_pm_staff_data.pm_start_of_commitment_d <= ? AND PUB_pm_staff_data.pm_end_of_commitment_d IS NULL))";

                parameters = new List <OdbcParameter>();
                param = new OdbcParameter("date1", OdbcType.DateTime);
                param.Value = new DateTime(2010, 1, 31);
                parameters.Add(param);
                param = new OdbcParameter("date2", OdbcType.DateTime);
                param.Value = new DateTime(2010, 1, 31);
                parameters.Add(param);

                DataTable commitments = DBAccess.GDBAccessObj.SelectDT(sqlCurrentCommitment, "test", ATransaction, parameters.ToArray());

                bool foundCommitment = false;

                foreach (DataRow row in commitments.Rows)
                {
                    if (row[0].ToString() == ledgerNumbers.Rows[0].ItemArray[0].ToString())
                    {
                        foundCommitment = true;
                    }
                }

                if (!foundCommitment)
                {
                    Console.WriteLine(
                        person.FirstName + " " + person.FamilyName + " " + person.PartnerKey + " " + ledgerNumbers.Rows[0].ItemArray[0].ToString());

                    foreach (DataRow row in commitments.Rows)
                    {
                        Console.WriteLine("    " + row[0].ToString());
                    }
                }
            }
        }
    }
}
}