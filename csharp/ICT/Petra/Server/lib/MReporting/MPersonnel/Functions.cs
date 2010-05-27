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
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Common;
using System.Data.Odbc;
using System.Data;

namespace Ict.Petra.Server.MReporting.MPersonnel
{
    /// <summary>
    /// These are the specific functions for the Personnel module,
    /// that are needed for report generation.
    /// </summary>
    public class TRptUserFunctionsPersonnel : TRptUserFunctions
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsPersonnel() : base()
        {
        }

        /// <summary>
        /// functions need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            if (StringHelper.IsSame(f, "GetSiteName"))
            {
                value = new TVariant(GetSiteName());
                return true;
            }

            if (StringHelper.IsSame(f, "GetCurrentCommitmentPeriod"))
            {
                value = new TVariant(GetCurrentCommitmentPeriod(ops[1].ToInt64(), ops[2].ToDate()));
                return true;
            }

            /*
             * if (isSame(f, 'doSomething')) then
             * begin
             * value := new TVariant();
             * doSomething(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString());
             * exit;
             * end;
             */
            value = new TVariant();
            return false;
        }

        /// <summary>
        /// returns the site name of the current site key,
        /// which is stored in s_system_parameter
        ///
        /// </summary>
        /// <returns>void</returns>
        private String GetSiteName()
        {
            String ReturnValue = "";
            string strSql;
            DataTable tab;
            long SiteKey = -1;
            PPartnerTable PartnerTable;

            strSql = "SELECT PUB_s_system_defaults.s_default_value_c " + "FROM PUB_s_system_defaults " +
                     "WHERE PUB_s_system_defaults.s_default_code_c = 'SiteKey'";

            tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                String SiteKeyString = Convert.ToString(tab.Rows[0]["s_default_value_c"]);
                try
                {
                    SiteKey = Convert.ToInt64(SiteKeyString);
                }
                catch (Exception)
                {
                    SiteKey = -1;
                }
            }

            PartnerTable = PPartnerAccess.LoadByPrimaryKey(SiteKey, situation.GetDatabaseConnection().Transaction);

            if (PartnerTable.Rows.Count > 0)
            {
                ReturnValue = (String)PartnerTable.Rows[0][PPartnerTable.GetPartnerShortNameDBName()];
            }

            return ReturnValue;
        }

        /// <summary>
        /// This functions finds the commitment period of a given partner, at a given time
        /// The result is stored in the variables CommitmentStart
        /// and CommitmentEnd. The end date might be empty, even if the start date is set
        ///
        /// It will find the most recent commitment,
        /// that starts on or before the given date and
        /// lasts till or beyond the given date (also open ended).
        /// If no such commitment exists, the most recent commitment of all will be returned.
        ///
        /// </summary>
        /// <returns>s true if a current commitment period was found
        /// </returns>
        private bool GetCurrentCommitmentPeriod(Int64 APartnerKey, DateTime AGivenDate)
        {
            bool ReturnValue;
            string strSql;
            DataTable tab;
            TRptFormatQuery formatQuery;

            System.Object StartDate = DateTime.MinValue;
            System.Object EndDate = DateTime.MinValue;
            ReturnValue = false;
            strSql = "SELECT pm_start_of_commitment_d, pm_end_of_commitment_d " + "FROM PUB_pm_staff_data " +
                     "WHERE PUB_pm_staff_data.p_partner_key_n = " + APartnerKey.ToString() + ' ' + "AND pm_start_of_commitment_d <= {#" +
                     StringHelper.DateToStr(AGivenDate, "dd/MM/yyyy") + "#} " + "AND (pm_end_of_commitment_d >= {#" + StringHelper.DateToStr(
                AGivenDate,
                "dd/MM/yyyy") + "#} " + "     OR pm_end_of_commitment_d IS NULL) " + "ORDER BY pm_start_of_commitment_d ASC";
            formatQuery = new TRptFormatQuery(null, -1, -1);
            strSql = formatQuery.ReplaceVariables(strSql).ToString();
            formatQuery = null;
            tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                // take the last row, the most recent start date
                ReturnValue = true;
                StartDate = tab.Rows[tab.Rows.Count - 1]["pm_start_of_commitment_d"];
                EndDate = tab.Rows[tab.Rows.Count - 1]["pm_end_of_commitment_d"];
            }
            else
            {
                // no commitment period for the given date was found, so find the most recent commitment
                strSql = "SELECT pm_start_of_commitment_d, pm_end_of_commitment_d " + "FROM PUB_pm_staff_data " +
                         "WHERE PUB_pm_staff_data.p_partner_key_n = " + APartnerKey.ToString() + ' ' + "ORDER BY pm_start_of_commitment_d ASC";
                tab = situation.GetDatabaseConnection().SelectDT(strSql, "", situation.GetDatabaseConnection().Transaction);

                if (tab.Rows.Count > 0)
                {
                    // take the last row, the most recent start date
                    ReturnValue = true;
                    StartDate = tab.Rows[tab.Rows.Count - 1]["pm_start_of_commitment_d"];
                    EndDate = tab.Rows[tab.Rows.Count - 1]["pm_end_of_commitment_d"];
                }
            }

            if (!ReturnValue)
            {
                situation.GetParameters().RemoveVariable("CommitmentStart", -1, -1, eParameterFit.eExact);
                situation.GetParameters().RemoveVariable("CommitmentEnd", -1, -1, eParameterFit.eExact);
            }
            else
            {
                situation.GetParameters().Add("CommitmentStart", new TVariant(StartDate), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                situation.GetParameters().Add("CommitmentEnd", new TVariant(EndDate), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }

            return ReturnValue;
        }
    }
}