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
using System.Configuration;
using System.IO;
using System.Text;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportBalances
    {
        /// <summary>
        /// export all GL Balances in the given year, towards the specified cost centres
        /// </summary>
        public static void ExportGLBalances(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            Int32 AFinancialYear,
            string ACostCentres,
            string AIgnoreAccounts)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "balance.csv"));

            Console.WriteLine("Writing file: " + filename);

            string sql =
                String.Format("SELECT GLM.{1} AS CostCentre, GLM.{0} AS Account, {2} AS StartBalance, GLMP.{3} AS EndBalance " +
                    "FROM PUB_{4} AS GLM, PUB_{10} AS A, PUB_{13} AS GLMP " +
                    "WHERE GLM.{5} = {6} AND {7} = {8} AND GLM.{1} IN ({9}) " +
                    "AND GLMP.{14} = GLM.{14} AND GLMP.{15} = 12 " +
                    "AND A.{5} = GLM.{5} AND A.{0} = GLM.{0} AND " +
                    "A.{11}=true AND NOT GLM.{0} IN ({12})" +
                    "ORDER BY GLM.{1}, GLM.{0}",
                    AGeneralLedgerMasterTable.GetAccountCodeDBName(),
                    AGeneralLedgerMasterTable.GetCostCentreCodeDBName(),
                    AGeneralLedgerMasterTable.GetStartBalanceBaseDBName(),
                    AGeneralLedgerMasterPeriodTable.GetActualBaseDBName(),
                    AGeneralLedgerMasterTable.GetTableDBName(),
                    AGeneralLedgerMasterTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AGeneralLedgerMasterTable.GetYearDBName(),
                    AFinancialYear,
                    "'" + ACostCentres.Replace(",", "','") + "'",
                    AAccountTable.GetTableDBName(),
                    AAccountTable.GetPostingStatusDBName(),
                    "'" + AIgnoreAccounts.Replace(",", "','") + "'",
                    AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                    AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName(),
                    AGeneralLedgerMasterPeriodTable.GetPeriodNumberDBName());

            TDBTransaction Transaction = null;
            DataTable balances = null;
            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    balances = DBAccess.GDBAccessObj.SelectDT(sql, "balances", Transaction);
                });

            StringBuilder sb = new StringBuilder();

            if (balances != null)
            {
                foreach (DataRow row in balances.Rows)
                {
                    sb.Append(StringHelper.StrMerge(
                            new string[] {
                            row["CostCentre"].ToString(),
                            row["Account"].ToString(),
                            String.Format("{0:N}", Convert.ToDecimal(row["StartBalance"])),
                            String.Format("{0:N}", Convert.ToDecimal(row["EndBalance"]))
                        }, ACSVSeparator));

                    sb.Append(ANewLine);
                }
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}