//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.Configuration;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExportIncomeTax
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportWorkers
    {
        /// <summary>
        /// export all details of workers in this year
        /// </summary>
        public static void Export(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int64 SiteKey,
            Int32 AYear)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "angestellte.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();
			PmStaffDataTable staffdata = new PmStaffDataTable();
			PPartnerTable partners = new PPartnerTable();
			partners.Constraints.Clear();
			PPersonTable persons = new PPersonTable();
			persons.Constraints.Clear();

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    // get all partners with a commitment period for this date range
                    // ignore non-native workers. field must be home office, or receiving field
                    string sql =
                        String.Format(
                            "SELECT * FROM PUB_pm_staff_data s, PUB_p_partner p, PUB_p_person per " +
                            "WHERE p.p_partner_key_n = s.p_partner_key_n " +
                            "AND (s.pm_home_office_n = ? OR pm_receiving_field_office_n = ? OR pm_receiving_field_n = ?) " +
                            "AND per.p_partner_key_n = s.p_partner_key_n " +
                            // start of commitment during this year
                            "AND ((pm_start_of_commitment_d BETWEEN ? AND ?) " +
                            // start of commitment before this year, end of commitment null or during/after this year
                            "  OR (pm_start_of_commitment_d < ? AND (pm_start_of_commitment_d IS NULL OR pm_start_of_commitment_d >= ?)))");

                    List <OdbcParameter>Parameters = new List <OdbcParameter>();
                    OdbcParameter param;

                    param = new OdbcParameter("field", OdbcType.Numeric);
                    param.Value = SiteKey;
                    Parameters.Add(param);

                    param = new OdbcParameter("field", OdbcType.Numeric);
                    param.Value = SiteKey;
                    Parameters.Add(param);

                    param = new OdbcParameter("field", OdbcType.Numeric);
                    param.Value = SiteKey;
                    Parameters.Add(param);

                    param = new OdbcParameter("startdate", OdbcType.DateTime);
                    param.Value = new DateTime(AYear, 1, 1);
                    Parameters.Add(param);

                    param = new OdbcParameter("enddate", OdbcType.DateTime);
                    param.Value = new DateTime(AYear, 12, 31);
                    Parameters.Add(param);

					param = new OdbcParameter("startdate", OdbcType.DateTime);
                    param.Value = new DateTime(AYear, 1, 1);
                    Parameters.Add(param);

					param = new OdbcParameter("startdate", OdbcType.DateTime);
                    param.Value = new DateTime(AYear, 1, 1);
                    Parameters.Add(param);                    

                    DBAccess.GDBAccessObj.SelectDT(staffdata, sql.Replace("SELECT *", "SELECT s.*"), Transaction, Parameters.ToArray(), 0, 0);
                    DBAccess.GDBAccessObj.SelectDT(partners, sql.Replace("SELECT *", "SELECT p.*"), Transaction, Parameters.ToArray(), 0, 0);
                    DBAccess.GDBAccessObj.SelectDT(persons, sql.Replace("SELECT *", "SELECT per.*"), Transaction, Parameters.ToArray(), 0, 0);

                });

				foreach (PmStaffDataRow staff in staffdata.Rows)
				{
					partners.DefaultView.Sort = "p_partner_key_n";
					persons.DefaultView.Sort = "p_partner_key_n";
					PPartnerRow partner = (PPartnerRow)partners.DefaultView.FindRows(new object[] { staff.PartnerKey })[0].Row;
					PPersonRow person = (PPersonRow)persons.DefaultView.FindRows(new object[] { staff.PartnerKey })[0].Row;

					sb.Append(StringHelper.StrMerge(
							new string[] {
								partner.PartnerKey.ToString(),
								partner.PartnerShortName.ToString(),
								person.DateOfBirth.HasValue?person.DateOfBirth.Value.ToString("yyyyMMdd"):String.Empty,
								partner.PartnerKey.ToString()
							}, ACSVSeparator));
					sb.Append(ANewLine);
				}

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}
