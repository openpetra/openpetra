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
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportTransactions
    {
        /// <summary>
        /// export all GL Transactions in the given year, towards the specified cost centres
        /// </summary>
        public static void ExportGLTransactions(string AOutputPath,
                                          char ACSVSeparator,
                                          string ANewLine,
                                          Int32 ALedgerNumber,
                                          Int32 AFinancialYear,
                                          string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "transaction.csv"));
            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();
            
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            
            string sql = 
                String.Format("SELECT T.{13}, T.{17}, T.{0}, T.{1}, T.{2}, T.{3}, B.{4}, T.{5}, T.{6}, T.{18} "
                              + "FROM PUB_{8} AS B, PUB_{7} AS T " +
                              "WHERE B.{9} = {10} AND B.{15} = {16} AND B.{11}='{12}' " +
                              "AND T.{9} = B.{9} AND T.{0} = B.{0} " +
                              "AND T.{13} IN ({14}) ORDER BY {0}, {1}, {2}",
                              ATransactionTable.GetBatchNumberDBName(),
                              ATransactionTable.GetJournalNumberDBName(),
                              ATransactionTable.GetTransactionNumberDBName(),
                              ATransactionTable.GetTransactionAmountDBName(),
                              ABatchTable.GetDateEffectiveDBName(),
                              ATransactionTable.GetNarrativeDBName(),
                              ATransactionTable.GetReferenceDBName(),
                              ATransactionTable.GetTableDBName(),
                              ABatchTable.GetTableDBName(),
                              ATransactionTable.GetLedgerNumberDBName(),
                              ALedgerNumber,
                              ABatchTable.GetBatchStatusDBName(),
                              MFinanceConstants.BATCH_POSTED,
                              ATransactionTable.GetCostCentreCodeDBName(),
                              "'" + ACostCentres.Replace(",", "','") + "'",
                              ABatchTable.GetBatchYearDBName(),
                              AFinancialYear,
                              ATransactionTable.GetAccountCodeDBName(),
                              ATransactionTable.GetDebitCreditIndicatorDBName());
            
            DataTable transactions = DBAccess.GDBAccessObj.SelectDT(sql, "transactions", Transaction);

            // get the analysis attributes
            sql = 
                String.Format("SELECT A.* from PUB_{1} AS B, PUB_{13} AS T, PUB_{0} AS A " +
                              "WHERE B.{2} = {3} AND B.{4} = {5} AND B.{6}='{7}' " +
                              "AND T.{2} = B.{2} AND T.{8} = B.{8} " +
                              "AND T.{9} IN ({10}) " + 
                              "AND A.{2} = T.{2} AND A.{8} = T.{8} AND A.{11} = T.{11} AND A.{12} = T.{12}",
                              ATransAnalAttribTable.GetTableDBName(),
                              ABatchTable.GetTableDBName(),
                              ABatchTable.GetLedgerNumberDBName(),
                              ALedgerNumber,
                              ABatchTable.GetBatchYearDBName(),
                              AFinancialYear,
                              ABatchTable.GetBatchStatusDBName(),
                              MFinanceConstants.BATCH_POSTED,
                              ATransactionTable.GetBatchNumberDBName(),
                              ATransactionTable.GetCostCentreCodeDBName(),
                              "'" + ACostCentres.Replace(",", "','") + "'",
                              ATransactionTable.GetJournalNumberDBName(),
                              ATransactionTable.GetTransactionNumberDBName(),
                              ATransactionTable.GetTableDBName(),
                              ABatchTable.GetBatchYearDBName());

            ATransAnalAttribTable TransAnalAttrib = new ATransAnalAttribTable();
            DBAccess.GDBAccessObj.SelectDT(TransAnalAttrib, sql, Transaction, null, 0, 0);
            
            TransAnalAttrib.DefaultView.Sort = 
                ATransAnalAttribTable.GetBatchNumberDBName() + "," +
                ATransAnalAttribTable.GetJournalNumberDBName() + "," +
                ATransAnalAttribTable.GetTransactionNumberDBName();
            
            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (DataRow row in transactions.Rows)
            {
                StringBuilder attributes = new StringBuilder();

                DataRowView[] attribs = TransAnalAttrib.DefaultView.FindRows(new object[]{row[2], row[3], row[4]});
                foreach (DataRowView rv in attribs)
                {
                    ATransAnalAttribRow attribRow = (ATransAnalAttribRow)rv.Row;
                    
                    // also export attribRow.AnalysisTypeCode?
                    attributes.Append(attribRow.AnalysisAttributeValue);
                }

                sb.Append(StringHelper.StrMerge(
                    new string[]{
                        row[0].ToString(),
                        row[1].ToString(),
                        "B" + row[2].ToString() + "_J" + row[3].ToString() + "_T" + row[4].ToString(),
                        String.Format("{0:N}", Convert.ToDecimal(row[5])),
                        Convert.ToBoolean(row[9])?"Debit":"Credit",
                        Convert.ToDateTime(row[6]).ToString("yyyyMMdd"),
                        row[7].ToString(),
                        row[8].ToString(),
                        attributes.ToString()}, ACSVSeparator));

                sb.Append(ANewLine);
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}