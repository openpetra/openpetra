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
    public class QueryHOSAReport
    {
        /// <summary>
        /// get all gifts for the current costcentre and account
        /// </summary>
        public static DataTable CalculateGifts(TParameterList AParameters, TResultList AResults)
        {
            SortedList <string, string>Defines = new SortedList <string, string>();
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
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

                resultTable.Columns.Add(new DataColumn("a_transaction_amount_n", typeof(Decimal)));
                resultTable.Columns.Add(new DataColumn("a_amount_in_base_currency_n", typeof(Decimal)));
                resultTable.Columns.Add(new DataColumn("a_amount_in_intl_currency_n", typeof(Decimal)));
                resultTable.Columns.Add(new DataColumn("a_reference_c", typeof(string)));
                resultTable.Columns.Add(new DataColumn("a_narrative_c", typeof(string)));

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
    }
}