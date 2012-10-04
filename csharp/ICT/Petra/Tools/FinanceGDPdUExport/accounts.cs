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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the accounts and costcentres involved
    public class TGDPdUExportAccountsAndCostCentres
    {
        /// <summary>
        /// Export the cost centres
        /// </summary>
        public static void ExportCostCentres(string AOutputPath,
                                             char ACSVSeparator,
                                             string ANewLine,
                                             Int32 ALedgerNumber, 
                                             string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "costcentre.csv"));
            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();
            
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            
            string sql = 
                String.Format("SELECT {0}, {1} from PUB_{2} WHERE {4} = {5} AND {0} IN ({3}) ORDER BY {0}", 
                              ACostCentreTable.GetCostCentreCodeDBName(),
                              ACostCentreTable.GetCostCentreNameDBName(),
                              ACostCentreTable.GetTableDBName(),
                              "'" + ACostCentres.Replace(",", "','") + "'",
                              ACostCentreTable.GetLedgerNumberDBName(),
                             ALedgerNumber);
            
            DataTable costcentres = DBAccess.GDBAccessObj.SelectDT(sql, "costcentres", Transaction);
            
            DBAccess.GDBAccessObj.RollbackTransaction();
            
            foreach (DataRow row in costcentres.Rows)
            {
                sb.Append(StringHelper.StrMerge(new string[]{row[0].ToString(), row[1].ToString()}, ACSVSeparator));
                sb.Append(ANewLine);
            }
            
            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }

        /// <summary>
        /// Export the accounts
        /// </summary>
        public static void ExportAccounts(string AOutputPath,
                                          char ACSVSeparator,
                                          string ANewLine,
                                          Int32 ALedgerNumber,
                                          string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "account.csv"));
            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();
            
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            
            // only export accounts that are actually used with these cost centres
            string sql = 
                String.Format("SELECT {0}, {1}, {8} from PUB_{2} AS A WHERE {3} = {4} AND " + 
                              " {8}=true AND " +
                              "EXISTS (SELECT * FROM PUB_{5} AS GLM WHERE GLM.{0} = A.{0} AND GLM.{6} IN ({7})) ORDER BY {0}",
                              AAccountTable.GetAccountCodeDBName(),
                              AAccountTable.GetAccountCodeLongDescDBName(),
                              AAccountTable.GetTableDBName(),
                              AAccountTable.GetLedgerNumberDBName(),
                              ALedgerNumber,
                              AGeneralLedgerMasterTable.GetTableDBName(),
                              AGeneralLedgerMasterTable.GetCostCentreCodeDBName(),
                              "'" + ACostCentres.Replace(",", "','") + "'",
                              AAccountTable.GetPostingStatusDBName(),
                              AAccountTable.GetDebitCreditIndicatorDBName());
            
            DataTable accounts = DBAccess.GDBAccessObj.SelectDT(sql, "accounts", Transaction);
            
            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (DataRow row in accounts.Rows)
            {
                sb.Append(StringHelper.StrMerge(new string[]{row[0].ToString(), row[1].ToString(),
                                                    Convert.ToBoolean(row[2])?"Debit":"Credit"}, ACSVSeparator));
                sb.Append(ANewLine);
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}