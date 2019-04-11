//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using HtmlAgilityPack;

namespace Ict.Petra.Server.MReporting.MFinance
{
    /// calculate the Account Detail report
    public class AccountDetail
    {
        /// calculate the report
        public static HtmlDocument Calculate(
            string AHTMLReportDefinition,
            TParameterList parameterlist,
            TDBTransaction ATransaction)
        {
            HTMLTemplateProcessor templateProcessor = new HTMLTemplateProcessor(AHTMLReportDefinition, parameterlist);

            // get all the transactions
            string sql = templateProcessor.GetSQLQuery("SelectTransactions");
            DataTable transactions = ATransaction.DataBaseObj.SelectDT(sql, "transactions", ATransaction);

            // get all the balances
            sql = templateProcessor.GetSQLQuery("SelectBalances");
            DataTable balances = ATransaction.DataBaseObj.SelectDT(sql, "balances", ATransaction);

            // generate the report from the HTML template
            HtmlDocument html = templateProcessor.GetHTML();
            CalculateData(ref html, balances, transactions, templateProcessor);

            return html;
        }


        private static void CalculateData(ref HtmlDocument html, DataTable balances, DataTable transactions, HTMLTemplateProcessor templateProcessor)
        {
            var balanceTemplate = HTMLTemplateProcessor.SelectSingleNode(html.DocumentNode, "//div[@id='costcentreaccount_template']");
            var balanceParentNode = balanceTemplate.ParentNode;

            int countBalanceRow = 0;
            int countTransactionRow = 0;
            foreach (DataRow balance in balances.Rows)
            {
                // skip account/costcentre combination if there are no transations and the balance is 0
                if ((Decimal)balance["end_balance"] == 0.0m)
                {
                    bool transactionExists = false;
                    foreach (DataRow transaction in transactions.Rows)
                    {
                        if ((transaction["a_account_code_c"].ToString() == balance["a_account_code_c"].ToString()) &&
                              (transaction["a_cost_centre_code_c"].ToString() == balance["a_cost_centre_code_c"].ToString()))
                        {
                            transactionExists = true;
                            break;
                        }
                    }

                    if (!transactionExists)
                    {
                        continue;
                    }
                }

                templateProcessor.AddParametersFromRow(balance);

                var newBalanceRow = balanceTemplate.Clone();
                string balanceId = "acccc" + countBalanceRow.ToString();
                newBalanceRow.SetAttributeValue("id", balanceId);
                balanceParentNode.AppendChild(newBalanceRow);
                var header = HTMLTemplateProcessor.SelectSingleNode(newBalanceRow, ".//div[contains(@class,'header')]");
                header.InnerHtml = templateProcessor.InsertParameters("{", "}", header.InnerHtml,
                    HTMLTemplateProcessor.ReplaceOptions.NoQuotes);
                countBalanceRow++;

                var transactionTemplate = HTMLTemplateProcessor.SelectSingleNode(newBalanceRow, ".//div[@id='transaction_template']");
                var FooterDiv = HTMLTemplateProcessor.SelectSingleNode(newBalanceRow, ".//div[@class='footer row']");

                Decimal TotalCredit = 0.0m;
                Decimal TotalDebit = 0.0m;
                foreach (DataRow transaction in transactions.Rows)
                {
                    if (!((transaction["a_account_code_c"].ToString() == balance["a_account_code_c"].ToString()) &&
                          (transaction["a_cost_centre_code_c"].ToString() == balance["a_cost_centre_code_c"].ToString())))
                    {
                        continue;
                    }

                    TVariant amount = new TVariant(transaction["a_transaction_amount_n"]);
                    if ((bool)transaction["a_debit_credit_indicator_l"])
                    {
                        templateProcessor.SetParameter("debit", amount);
                        templateProcessor.SetParameter("credit", new TVariant());
                        TotalDebit += amount.ToDecimal();
                    }
                    else
                    {
                        templateProcessor.SetParameter("credit", amount);
                        templateProcessor.SetParameter("debit", new TVariant());
                        TotalCredit += amount.ToDecimal();
                    }

                    templateProcessor.AddParametersFromRow(transaction);

                    var newTransactionRow = transactionTemplate.Clone();
                    string transactionID = "trans" + countTransactionRow.ToString();
                    newTransactionRow.SetAttributeValue("id", transactionID);
                    newBalanceRow.InsertBefore(newTransactionRow, FooterDiv);
                    newTransactionRow.InnerHtml = templateProcessor.InsertParameters("{", "}", newTransactionRow.InnerHtml,
                        HTMLTemplateProcessor.ReplaceOptions.NoQuotes);
                    countTransactionRow++;
                }

                templateProcessor.SetParameter("total_debit", new TVariant(TotalDebit));
                templateProcessor.SetParameter("total_credit", new TVariant(TotalCredit));
                FooterDiv.InnerHtml = templateProcessor.InsertParameters("{", "}", FooterDiv.InnerHtml,
                            HTMLTemplateProcessor.ReplaceOptions.NoQuotes);

                transactionTemplate.Remove();
            }

            balanceTemplate.Remove();
        }
    }
}
