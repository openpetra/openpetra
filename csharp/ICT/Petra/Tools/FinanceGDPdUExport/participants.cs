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
using System.Collections.Generic;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Testing.NUnitPetraServer;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExportParticipants
    {
        /// <summary>
        /// export all posted invoices for conference and seminar participants in this year
        /// </summary>
        public static void Export(string AOutputPath,
            char ACSVSeparator,
            string ANewLine,
            Int32 ALedgerNumber,
            Int32 AFinancialYear,
            string ACostCentres)
        {
            string filename = Path.GetFullPath(Path.Combine(AOutputPath, "participants.csv"));

            Console.WriteLine("Writing file: " + filename);

            StringBuilder sb = new StringBuilder();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // all gift details towards a costcentre that needs to be exported
            string sql =
                String.Format("SELECT DISTINCT D.* " +
                    "FROM PUB_{0} AS B, PUB_{1} AS G, PUB_{2} AS D " +
                    "WHERE B.{3} = {4} AND B.{5} = {6} AND B.{7}='{8}' " +
                    "AND G.{3} = B.{3} AND G.{9} = B.{9} " +
                    "AND D.{3} = G.{3} AND D.{9} = G.{9} AND D.{10} = G.{10} " +
                    "AND D.{11} IN ({12}) " +
                    "AND NOT D.{13} = '{14}'",
                    AGiftBatchTable.GetTableDBName(),
                    AGiftTable.GetTableDBName(),
                    AGiftDetailTable.GetTableDBName(),
                    AGiftBatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AGiftBatchTable.GetBatchYearDBName(),
                    AFinancialYear,
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_POSTED,
                    AGiftBatchTable.GetBatchNumberDBName(),
                    AGiftTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetCostCentreCodeDBName(),
                    "'" + ACostCentres.Replace(",", "','") + "'",
                    AGiftDetailTable.GetMotivationGroupCodeDBName(),
                    "GIFT");

            AGiftDetailTable giftdetails = new AGiftDetailTable();
            DBAccess.GDBAccessObj.SelectDT(giftdetails, sql, Transaction, null, 0, 0);

            sql = sql.Replace("SELECT DISTINCT D.*", "SELECT DISTINCT G.*");

            AGiftTable gifts = new AGiftTable();
            DBAccess.GDBAccessObj.SelectDT(gifts, sql, Transaction, null, 0, 0);

            gifts.DefaultView.Sort =
                AGiftTable.GetBatchNumberDBName() + "," +
                AGiftTable.GetGiftTransactionNumberDBName();

            sql = sql.Replace("SELECT DISTINCT G.*", "SELECT DISTINCT B.*");

            AGiftBatchTable batches = new AGiftBatchTable();
            DBAccess.GDBAccessObj.SelectDT(batches, sql, Transaction, null, 0, 0);
            batches.DefaultView.Sort = AGiftTable.GetBatchNumberDBName();

            sql =
                String.Format("SELECT DISTINCT P.* " +
                    "FROM PUB_{0} AS B, PUB_{1} AS G, PUB_{2} AS D, PUB.{15} AS P " +
                    "WHERE B.{3} = {4} AND B.{5} = {6} AND B.{7}='{8}' " +
                    "AND G.{3} = B.{3} AND G.{9} = B.{9} " +
                    "AND D.{3} = G.{3} AND D.{9} = G.{9} AND D.{10} = G.{10} " +
                    "AND D.{11} IN ({12}) " +
                    "AND NOT D.{13} = '{14}' " +
                    "AND P.{16} = G.{17}",
                    AGiftBatchTable.GetTableDBName(),
                    AGiftTable.GetTableDBName(),
                    AGiftDetailTable.GetTableDBName(),
                    AGiftBatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AGiftBatchTable.GetBatchYearDBName(),
                    AFinancialYear,
                    AGiftBatchTable.GetBatchStatusDBName(),
                    MFinanceConstants.BATCH_POSTED,
                    AGiftBatchTable.GetBatchNumberDBName(),
                    AGiftTable.GetGiftTransactionNumberDBName(),
                    AGiftDetailTable.GetCostCentreCodeDBName(),
                    "'" + ACostCentres.Replace(",", "','") + "'",
                    AGiftDetailTable.GetMotivationGroupCodeDBName(),
                    "GIFT",
                    PPersonTable.GetTableDBName(),
                    PPersonTable.GetPartnerKeyDBName(),
                    AGiftTable.GetDonorKeyDBName());

            PPersonTable persons = new PPersonTable();
            DBAccess.GDBAccessObj.SelectDT(persons, sql, Transaction, null, 0, 0);
            persons.DefaultView.Sort = PPersonTable.GetPartnerKeyDBName();

            DBAccess.GDBAccessObj.RollbackTransaction();

            foreach (AGiftDetailRow detail in giftdetails.Rows)
            {
                AGiftRow gift = (AGiftRow)gifts.DefaultView.FindRows(new object[] { detail.BatchNumber, detail.GiftTransactionNumber })[0].Row;
                AGiftBatchRow batch = (AGiftBatchRow)batches.DefaultView.FindRows(detail.BatchNumber)[0].Row;

                DataRowView[] personList = persons.DefaultView.FindRows(gift.DonorKey);
                PPersonRow person = (personList.Length > 0 ? (PPersonRow)personList[0].Row : null);

                sb.Append(StringHelper.StrMerge(
                        new string[] {
                            detail.BatchNumber.ToString(),
                            detail.GiftTransactionNumber.ToString(),
                            String.Format("{0:N}", detail.GiftTransactionAmount),
                            batch.GlEffectiveDate.ToString("yyyyMMdd"),
                            gift.DonorKey.ToString(),
                            person !=
                            null ? (person.DateOfBirth.HasValue ? person.DateOfBirth.Value.ToString("yyyyMMdd") : string.Empty) : string.Empty,
                            detail.CostCentreCode,
                            batch.BatchDescription,
                            detail.GiftCommentOne,
                            detail.GiftCommentTwo
                        }, ACSVSeparator));
                sb.Append(ANewLine);
            }

            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(1252));
            sw.Write(sb.ToString());
            sw.Close();
        }
    }
}