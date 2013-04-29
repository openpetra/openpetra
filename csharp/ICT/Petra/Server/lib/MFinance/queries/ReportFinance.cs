//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Server.MFinance.queries
{
    /// <summary>
    /// contains a method that calculates the gifts for a HOSA
    /// </summary>
    public class QueryFinanceReport
    {
        /// <summary>
        /// get all gifts for the current costcentre and account
        /// </summary>
        public static DataTable HosaCalculateGifts(TParameterList AParameters, TResultList AResults)
        {
            SortedList <string, string>Defines = new SortedList <string, string>();
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
                if (AParameters.Get("param_filter_cost_centres").ToString() == "PersonalCostcentres")
                {
                    Defines.Add("PERSONALHOSA", "true");
                }

                SqlParameterList.Add(new OdbcParameter("ledgernumber", OdbcType.Decimal)
                    {
                        Value = AParameters.Get("param_ledger_number_i").ToDecimal()
                    });
                SqlParameterList.Add(new OdbcParameter("costcentre", OdbcType.VarChar)
                    {
                        Value = AParameters.Get("line_a_cost_centre_code_c")
                    });

                if (AParameters.Get("param_ich_number").ToInt32() == 0)
                {
                    Defines.Add("NOT_LIMITED_TO_ICHNUMBER", "true");
                }
                else
                {
                    SqlParameterList.Add(new OdbcParameter("ichnumber", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_ich_number").ToInt32()
                        });
                }

                SqlParameterList.Add(new OdbcParameter("batchstatus", OdbcType.VarChar)
                    {
                        Value = MFinanceConstants.BATCH_POSTED
                    });

                if (AParameters.Get("param_period").ToBool() == true)
                {
                    Defines.Add("BYPERIOD", "true");
                    SqlParameterList.Add(new OdbcParameter("batchyear", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_year_i").ToInt32()
                        });
                    SqlParameterList.Add(new OdbcParameter("batchperiod_start", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_start_period_i").ToInt32()
                        });
                    SqlParameterList.Add(new OdbcParameter("batchperiod_start", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_end_period_i").ToInt32()
                        });
                }
                else
                {
                    SqlParameterList.Add(new OdbcParameter("param_start_date", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_start_date").ToInt32()
                        });
                    SqlParameterList.Add(new OdbcParameter("param_end_date", OdbcType.Int)
                        {
                            Value = AParameters.Get("param_end_date").ToInt32()
                        });
                }

                SqlParameterList.Add(new OdbcParameter("accountcode", OdbcType.VarChar)
                    {
                        Value = AParameters.Get("line_a_account_code_c")
                    });
            }
            catch (Exception e)
            {
                TLogging.Log("problem while preparing sql statement for HOSA report: " + e.ToString());
                return null;
            }

            string SqlStmt = TDataBase.ReadSqlFile("ICH.HOSAReportGiftSummary.sql", Defines);
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                // now run the database query
                DataTable resultTable = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "result", Transaction,
                    SqlParameterList.ToArray());

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return null;
                }

                resultTable.Columns.Add("a_transaction_amount_n", typeof(Decimal));
                resultTable.Columns.Add("a_amount_in_base_currency_n", typeof(Decimal));
                resultTable.Columns.Add("a_amount_in_intl_currency_n", typeof(Decimal));
                resultTable.Columns.Add("a_reference_c", typeof(string));
                resultTable.Columns.Add("a_narrative_c", typeof(string));

                foreach (DataRow r in resultTable.Rows)
                {
                    r["a_transaction_amount_n"] = Convert.ToDecimal(r["GiftTransactionAmount"]);
                    r["a_amount_in_base_currency_n"] = Convert.ToDecimal(r["GiftBaseAmount"]);
                    // TODO convert to international currency, etc
                    // r["a_amount_in_intl_currency_n"] = Convert.ToDecimal(r["GiftBaseAmount"]) * GetExchangeRate;

                    r["a_reference_c"] = StringHelper.PartnerKeyToStr(Convert.ToInt64(r["RecipientKey"]));
                    r["a_narrative_c"] = r["RecipientShortname"].ToString();
                }

                return resultTable;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return null;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Find all the gifts for a specific month, returning "worker", "field" and "total" results.
        /// </summary>
        public static DataTable TotalGiftsThroughFieldMonth(TParameterList AParameters, TResultList AResults)
        {
            Int32 LedgerNum = AParameters.Get("param_ledger_number_i").ToInt32();
            Int32 Year = AParameters.Get("param_YearBlock").ToInt32();
            string YearStart = String.Format("#{0:0000}-01-01#", Year);
            string YearEnd = String.Format("#{0:0000}-12-31#", Year);

            string SqlQuery = "SELECT batch.a_gl_effective_date_d as Date, motive.a_report_column_c AS ReportColumn, ";

            if (AParameters.Get("param_currency").ToString() == "Base")
            {
                SqlQuery += "detail.a_gift_amount_n AS Amount";
            }
            else
            {
                SqlQuery += "detail.a_gift_amount_intl_n AS Amount";
            }

            SqlQuery += (" FROM PUB_a_gift as gift, PUB_a_gift_detail as detail, PUB_a_gift_batch as batch, PUB_a_motivation_detail AS motive"

                         + " WHERE detail.a_ledger_number_i = " + LedgerNum +
                         " AND batch.a_batch_status_c = 'Posted'" +
                         " AND batch.a_batch_number_i = gift.a_batch_number_i" +
                         " AND batch.a_ledger_number_i = " + LedgerNum +
                         " AND batch.a_gl_effective_date_d >= " + YearStart +
                         " AND batch.a_gl_effective_date_d <= " + YearEnd

                         + " AND gift.a_ledger_number_i = " + LedgerNum +
                         " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                         " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i"

                         + " AND motive.a_ledger_number_i = " + LedgerNum +
                         " AND motive.a_motivation_group_code_c = detail.a_motivation_group_code_c" +
                         " AND motive.a_motivation_detail_code_c = detail.a_motivation_detail_code_c" +
                         " AND motive.a_receipt_l = true");
            Boolean newTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out newTransaction);
            DataTable tempTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "result", Transaction);

            if (newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("MonthName", typeof(String));               //
            resultTable.Columns.Add("MonthWorker", typeof(Decimal));            // These are the names of the variables
            resultTable.Columns.Add("MonthWorkerCount", typeof(Int32));         // returned by this calculation.
            resultTable.Columns.Add("MonthField", typeof(Decimal));             //
            resultTable.Columns.Add("MonthFieldCount", typeof(Int32));          //
            resultTable.Columns.Add("MonthTotal", typeof(Decimal));             //
            resultTable.Columns.Add("MonthTotalCount", typeof(Int32));          //

            for (int mnth = 1; mnth <= 12; mnth++)
            {
                string monthStart = String.Format("#{0:0000}-{1:00}-01#", Year, mnth);
                string nextMonthStart = String.Format("#{0:0000}-{1:00}-01#", Year, mnth + 1);

                if (mnth == 12)
                {
                    nextMonthStart = String.Format("#{0:0000}-12-31#", Year);
                }

                tempTbl.DefaultView.RowFilter = "Date >= " + monthStart + " AND Date < " + nextMonthStart;

                Decimal WorkerTotal = 0;
                Decimal FieldTotal = 0;
                Int32 WorkerCount = 0;
                Int32 FieldCount = 0;
                Int32 TotalCount = tempTbl.DefaultView.Count;

                for (int i = 0; i < TotalCount; i++)
                {
                    DataRow Row = tempTbl.DefaultView[i].Row;

                    if (Row["ReportColumn"].ToString() == "Worker")
                    {
                        WorkerCount++;
                        WorkerTotal += Convert.ToDecimal(Row["Amount"]);
                    }
                    else
                    {
                        FieldCount++;
                        FieldTotal += Convert.ToDecimal(Row["Amount"]);
                    }
                }

                DataRow resultRow = resultTable.NewRow();

                resultRow["MonthName"] = StringHelper.GetLongMonthName(mnth);
                resultRow["MonthWorker"] = WorkerTotal;
                resultRow["MonthWorkerCount"] = WorkerCount;
                resultRow["MonthField"] = FieldTotal;
                resultRow["MonthFieldCount"] = FieldCount;
                resultRow["MonthTotal"] = WorkerTotal + FieldTotal;
                resultRow["MonthTotalCount"] = TotalCount;
                resultTable.Rows.Add(resultRow);
            }

            return resultTable;
        }

        /// <summary>
        /// Find all the gifts for a year, returning "worker", "field" and "total" results.
        /// </summary>
        public static DataTable TotalGiftsThroughFieldYear(TParameterList AParameters, TResultList AResults)
        {
            Int32 LedgerNum = AParameters.Get("param_ledger_number_i").ToInt32();
            string SqlQuery = "SELECT batch.a_gl_effective_date_d as Date, motive.a_report_column_c AS ReportColumn, ";

            if (AParameters.Get("param_currency").ToString() == "Base")
            {
                SqlQuery += "detail.a_gift_amount_n AS Amount";
            }
            else
            {
                SqlQuery += "detail.a_gift_amount_intl_n AS Amount";
            }

            SqlQuery += (" FROM PUB_a_gift as gift, PUB_a_gift_detail as detail, PUB_a_gift_batch as batch, PUB_a_motivation_detail AS motive"

                         + " WHERE detail.a_ledger_number_i = " + LedgerNum +
                         " AND batch.a_batch_status_c = 'Posted'" +
                         " AND batch.a_batch_number_i = gift.a_batch_number_i" +
                         " AND batch.a_ledger_number_i = " + LedgerNum

                         + " AND gift.a_ledger_number_i = " + LedgerNum +
                         " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                         " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i"

                         + " AND motive.a_ledger_number_i = " + LedgerNum +
                         " AND motive.a_motivation_group_code_c = detail.a_motivation_group_code_c" +
                         " AND motive.a_motivation_detail_code_c = detail.a_motivation_detail_code_c" +
                         " AND motive.a_receipt_l = true");
            Boolean newTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out newTransaction);
            DataTable tempTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "result", Transaction);

            if (newTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("SummaryYear", typeof(Int32));              //
            resultTable.Columns.Add("YearWorker", typeof(Decimal));             // These are the names of the variables
            resultTable.Columns.Add("YearWorkerCount", typeof(Int32));          // returned by this calculation.
            resultTable.Columns.Add("YearField", typeof(Decimal));              //
            resultTable.Columns.Add("YearFieldCount", typeof(Int32));           //
            resultTable.Columns.Add("YearTotal", typeof(Decimal));              //
            resultTable.Columns.Add("YearTotalCount", typeof(Int32));           //

            Int32 Year = DateTime.Now.Year;

            for (Int32 YearIdx = 0; YearIdx < 10; YearIdx++)
            {
                string yearStart = String.Format("#{0:0000}-01-01#", Year - YearIdx);
                string yearEnd = String.Format("#{0:0000}-12-31#", Year - YearIdx);

                tempTbl.DefaultView.RowFilter = "Date >= " + yearStart + " AND Date < " + yearEnd;

                Decimal WorkerTotal = 0;
                Decimal FieldTotal = 0;
                Int32 WorkerCount = 0;
                Int32 FieldCount = 0;
                Int32 TotalCount = tempTbl.DefaultView.Count;

                for (int i = 0; i < TotalCount; i++)
                {
                    DataRow Row = tempTbl.DefaultView[i].Row;

                    if (Row["ReportColumn"].ToString() == "Worker")
                    {
                        WorkerCount++;
                        WorkerTotal += Convert.ToDecimal(Row["Amount"]);
                    }
                    else
                    {
                        FieldCount++;
                        FieldTotal += Convert.ToDecimal(Row["Amount"]);
                    }
                }

                DataRow resultRow = resultTable.NewRow();

                resultRow["SummaryYear"] = Year - YearIdx;
                resultRow["YearWorker"] = WorkerTotal;
                resultRow["YearWorkerCount"] = WorkerCount;
                resultRow["YearField"] = FieldTotal;
                resultRow["YearFieldCount"] = FieldCount;
                resultRow["YearTotal"] = WorkerTotal + FieldTotal;
                resultRow["YearTotalCount"] = TotalCount;
                resultTable.Rows.Add(resultRow);

                if (TotalCount == 0)  // The summary runs backwards in time until it has reported one row of zeroes.
                {
                    break;
                }
            }

            return resultTable;
        }
    }
}