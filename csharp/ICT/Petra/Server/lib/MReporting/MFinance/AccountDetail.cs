//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Remoting.Server;
using System.IO;

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// calculate the Account Detail report
    public class AccountDetail
    {
        /// calculate the report
        public static string Calculate(
            string AHTMLReportDefinition,
            TParameterList parameterlist,
            out TResultList resultlist)
        {
            resultlist = new TResultList();

            HTMLTemplateProcessor templateProcessor = new HTMLTemplateProcessor(AHTMLReportDefinition, parameterlist);

            bool NewTransaction;
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                out NewTransaction,
                "AccountDetailRead");

            // get all the transactions
            string sql = templateProcessor.GetSQLQuery("SelectTransactions", parameterlist);
            TLogging.Log(sql);
            DataTable transactions = DBAccess.GDBAccessObj.SelectDT(sql, "transactions", ReadTransaction);

            // get all the balances
            sql = templateProcessor.GetSQLQuery("SelectBalances", parameterlist);
            TLogging.Log(sql);
            DataTable balances = DBAccess.GDBAccessObj.SelectDT(sql, "balances", ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // generate the data version for the Excel export
            resultlist = PrepareResultList(balances, transactions, parameterlist);

            // render the report from the HTML template

            return templateProcessor.GetHTML();
        }

        private static TResultList PrepareResultList(DataTable balances, DataTable transactions, TParameterList parameters)
        {
            int MaxDisplayColumns = 5; //parameters.Get("MaxDisplayColumns");
            int MasterRow = 0;
            int ChildRow = 1;
            int Level = 0;

            TResultList result = new TResultList();
            parameters.Add("MaxDisplayColumns", MaxDisplayColumns);

            foreach (DataRow balance in balances.Rows)
            {

                TVariant[] Header = new TVariant[MaxDisplayColumns];
                TVariant[] Description =
                {
                    new TVariant("desc1test"), new TVariant("desc2test")
                };
                TVariant[] Columns = new TVariant[MaxDisplayColumns];

                for (int Counter = 0; Counter < MaxDisplayColumns; ++Counter)
                {
                    Columns[Counter] = new TVariant(" ");
                    Header[Counter] = new TVariant();
                }

                // TODO use parameters for column arrangement
                Columns[0] = new TVariant(balance["a_account_code_c"]);
                Columns[1] = new TVariant(balance["a_account_code_short_desc_c"]);
                Columns[2] = new TVariant(balance["a_cost_centre_code_c"]);
                Columns[3] = new TVariant(balance["a_cost_centre_name_c"]);
                Columns[4] = new TVariant(balance["a_actual_base_n"]);

                result.AddRow(MasterRow, ChildRow, true, Level,
                              balance["a_account_code_c"].ToString() + "-" + balance["a_cost_centre_code_c"].ToString(),
                              "", 
                              (bool)balance["a_debit_credit_indicator_l"],
                              Header, Description, Columns);
                ChildRow++;
            }

            return result;
        }
    }
}
