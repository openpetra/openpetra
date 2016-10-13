//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Globalization;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.Setup.WebConnectors
{
    /// <summary>
    /// setup the Corporate Exchange Rates
    /// </summary>
    public partial class TCorporateExchangeRatesSetupWebConnector
    {
        /// <summary>
        /// Saves the corporate exchange setup TDS.
        /// </summary>
        /// <param name="AInspectDS">Containing all ACorporateExchangeRate records to be saved.</param>
        /// <param name="ATransactionsChanged">Number of transaction that were updated with the new exchange rate</param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveCorporateExchangeSetupTDS(ref CorporateExchangeSetupTDS AInspectDS,
            out int ATransactionsChanged, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            ATransactionsChanged = -1;
            int TransactionsChanged = -1;

            AInspectDS = AInspectDS.GetChangesTyped(true);

            if (AInspectDS == null)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Save Corportate Exchange Rates"),
                        Catalog.GetString("No changes - nothing to do"),
                        TResultSeverity.Resv_Info));
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            TDBTransaction Transaction = null;
            bool SubmissionOK = true;
            CorporateExchangeSetupTDS InspectDS = AInspectDS;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable,
                ref Transaction, ref SubmissionOK,
                delegate
                {
                    foreach (ACorporateExchangeRateRow xchangeRateRow in InspectDS.ACorporateExchangeRate.Rows)
                    {
                        if ((xchangeRateRow.RowState == DataRowState.Modified) || (xchangeRateRow.RowState == DataRowState.Added))
                        {
                            // should only be -1 if no exchange rates were modified or created
                            if (TransactionsChanged == -1)
                            {
                                TransactionsChanged = 0;
                            }

                            String rateOfExchangeStr = xchangeRateRow.RateOfExchange.ToString(
                                CultureInfo.InvariantCulture);
                            // update international amounts for all gl transaction using modified or new exchange rate
                            string Query = "UPDATE a_transaction SET a_amount_in_intl_currency_n = " +
                                           "ROUND (a_amount_in_base_currency_n / " + rateOfExchangeStr + ", 2)" +
                                           " FROM a_ledger" +
                                           " WHERE EXTRACT(MONTH FROM a_transaction.a_transaction_date_d) = " +
                                           xchangeRateRow.DateEffectiveFrom.Month +
                                           " AND EXTRACT(YEAR FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Year +
                                           " AND a_ledger.a_ledger_number_i = a_transaction.a_ledger_number_i" +
                                           " AND a_ledger.a_base_currency_c = '" + xchangeRateRow.FromCurrencyCode + "'" +
                                           " AND a_ledger.a_intl_currency_c = '" + xchangeRateRow.ToCurrencyCode + "'";

                            TransactionsChanged += DBAccess.GDBAccessObj.ExecuteNonQuery(Query, Transaction);
                        }

                        if (TransactionsChanged > 0)
                        {
                            //
                            // I also need to correct entries in GLM and GLMP after modifying these transactions:
                            DataTable ledgerTbl = DBAccess.GDBAccessObj.SelectDT(
                                "SELECT * FROM a_ledger WHERE " +
                                " a_ledger.a_base_currency_c = '" + xchangeRateRow.FromCurrencyCode + "'" +
                                " AND a_ledger.a_intl_currency_c = '" + xchangeRateRow.ToCurrencyCode + "'",
                                "a_ledger", Transaction);

                            foreach (DataRow ledgerRow in ledgerTbl.Rows)
                            {
                                Int32 ledgerNumber = Convert.ToInt32(ledgerRow["a_ledger_number_i"]);
                                DataTable tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                    "SELECT DISTINCT a_batch.a_batch_period_i, a_batch.a_batch_year_i FROM" +
                                    " a_batch, a_transaction, a_ledger WHERE" +
                                    " EXTRACT(MONTH FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Month +
                                    " AND EXTRACT(YEAR FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Year +
                                    " AND a_transaction.a_ledger_number_i =" + ledgerNumber +
                                    " AND a_batch.a_batch_number_i = a_transaction.a_batch_number_i " +
                                    " ORDER BY a_batch_period_i",
                                    "temp", Transaction);

                                if ((tempTbl == null) || (tempTbl.Rows.Count == 0))
                                {
                                    continue;
                                }

                                Int32 transactionPeriod = Convert.ToInt32(tempTbl.Rows[0]["a_batch_period_i"]);
                                Int32 transactionYear = Convert.ToInt32(tempTbl.Rows[0]["a_batch_year_i"]);

                                DataTable glmTbl = DBAccess.GDBAccessObj.SelectDT(
                                    "SELECT * from a_general_ledger_master" +
                                    " WHERE a_ledger_number_i = " + ledgerNumber +
                                    " AND a_year_i = " + transactionYear,
                                    "temp", Transaction);
                                Boolean seemsToWorkOk = true;

//                              TLogging.Log("GLM correction: ");
                                Int32 glmSequence = 0;
                                String accountCode = "";
                                String costCentreCode = "";
                                String problem = "";

                                foreach (DataRow glmRow in glmTbl.Rows)
                                {
                                    glmSequence = Convert.ToInt32(glmRow["a_glm_sequence_i"]);
                                    accountCode = glmRow["a_account_code_c"].ToString();
                                    costCentreCode = glmRow["a_cost_centre_code_c"].ToString();

                                    tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                        "SELECT sum(a_amount_in_intl_currency_n) AS debit_total FROM a_transaction WHERE " +
                                        " EXTRACT(MONTH FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Month +
                                        " AND EXTRACT(YEAR FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Year +
                                        " AND a_account_code_c = '" + accountCode + "'" +
                                        " AND a_cost_centre_code_c = '" + costCentreCode + "'" +
                                        " AND a_debit_credit_indicator_l", "temp", Transaction);

                                    Boolean hasDebitTransactions = (
                                        tempTbl != null
                                        && tempTbl.Rows[0]["debit_total"].GetType() != typeof(System.DBNull)
                                        );
                                    Decimal debitTotal = 0;

                                    if (hasDebitTransactions)
                                    {
                                        seemsToWorkOk = (tempTbl.Rows.Count == 1);

                                        if (!seemsToWorkOk)
                                        {
                                            problem = "DebitTotal";
                                            break;
                                        }

                                        debitTotal = Convert.ToDecimal(tempTbl.Rows[0]["debit_total"]);
                                    }

                                    tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                        "SELECT sum(a_amount_in_intl_currency_n) AS credit_total FROM a_transaction WHERE " +
                                        " EXTRACT(MONTH FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Month +
                                        " AND EXTRACT(YEAR FROM a_transaction.a_transaction_date_d) = " + xchangeRateRow.DateEffectiveFrom.Year +
                                        " AND a_account_code_c = '" + accountCode + "'" +
                                        " AND a_cost_centre_code_c = '" + costCentreCode + "'" +
                                        " AND NOT a_debit_credit_indicator_l", "temp", Transaction);

                                    Boolean hasCreditTransactions = (
                                        tempTbl != null
                                        && tempTbl.Rows[0]["credit_total"].GetType() != typeof(System.DBNull)
                                        );
                                    Decimal creditTotal = 0;

                                    if (hasCreditTransactions)
                                    {
                                        seemsToWorkOk = (tempTbl.Rows.Count == 1);

                                        if (!seemsToWorkOk)
                                        {
                                            problem = "CreditTotal";
                                            break;
                                        }

                                        creditTotal = Convert.ToDecimal(tempTbl.Rows[0]["credit_total"]);
                                    }

                                    if (!hasDebitTransactions && !hasCreditTransactions)
                                    {
//                                      TLogging.Log("CostCentre " + costCentreCode + " Account " + accountCode + " - no transactions.");
                                        continue;
                                    }

                                    Decimal lastMonthBalance = 0;

                                    if (transactionPeriod > 1)
                                    {
                                        tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                            "SELECT a_actual_intl_n as last_month_balance " +
                                            " FROM a_general_ledger_master_period WHERE " +
                                            " a_glm_sequence_i = " + glmSequence + " AND a_period_number_i = " + (transactionPeriod - 1),
                                            "temp", Transaction);
                                        seemsToWorkOk = (tempTbl.Rows.Count == 1);

                                        if (!seemsToWorkOk)
                                        {
                                            problem = "lastMonthBalance";
                                            break;
                                        }

                                        lastMonthBalance = Convert.ToDecimal(tempTbl.Rows[0]["last_month_balance"]);
                                    }
                                    else
                                    {
                                        lastMonthBalance = Convert.ToInt32(glmRow["a_start_balance_intl_n"]);
                                    }

                                    tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                        "SELECT a_actual_intl_n as this_month_balance " +
                                        " FROM  a_general_ledger_master_period WHERE " +
                                        " a_glm_sequence_i = " + glmSequence + " AND a_period_number_i = " + transactionPeriod,
                                        "temp", Transaction);

                                    seemsToWorkOk = (tempTbl.Rows.Count == 1);

                                    if (!seemsToWorkOk)
                                    {
                                        problem = "thisMonthBalance";
                                        break;
                                    }

                                    Decimal thisMonthBalance = Convert.ToDecimal(tempTbl.Rows[0]["this_month_balance"]);

                                    tempTbl = DBAccess.GDBAccessObj.SelectDT(
                                        "SELECT a_debit_credit_indicator_l AS debit_indicator FROM " +
                                        " a_account WHERE a_account_code_c = '" + accountCode + "'" +
                                        " AND a_ledger_number_i = " + ledgerNumber,
                                        "temp", Transaction);
                                    seemsToWorkOk = (tempTbl.Rows.Count == 1);

                                    if (!seemsToWorkOk)
                                    {
                                        problem = "debitCreditIndicator";
                                        break;
                                    }

                                    Boolean debitCreditIndicator = Convert.ToBoolean(tempTbl.Rows[0]["debit_indicator"]);

                                    Decimal newPeriodBalance = (debitCreditIndicator) ?
                                                               lastMonthBalance + debitTotal - creditTotal
                                                               :
                                                               lastMonthBalance - debitTotal + creditTotal;

                                    Decimal discrepency = newPeriodBalance - thisMonthBalance;

                                    DBAccess.GDBAccessObj.ExecuteNonQuery(
                                        "UPDATE a_general_ledger_master_period SET " +
                                        " a_actual_intl_n = a_actual_intl_n + " + discrepency.ToString(CultureInfo.InvariantCulture) +
                                        " WHERE a_glm_sequence_i = " + glmSequence +
                                        " AND a_period_number_i >= " + transactionPeriod, Transaction);

                                    DBAccess.GDBAccessObj.ExecuteNonQuery(
                                        "UPDATE a_general_ledger_master SET " +
                                        " a_ytd_actual_intl_n = a_ytd_actual_intl_n + " + discrepency.ToString(CultureInfo.InvariantCulture) +
                                        " WHERE a_glm_sequence_i = " + glmSequence, Transaction);

//                                  TLogging.Log("Discrepency for CostCentre " + costCentreCode + " Account " + accountCode + " is " + discrepency);
                                } // foreach glmRow

                                if (!seemsToWorkOk)
                                {
                                    TLogging.Log("SaveCorporateExchangeSetupTDS: unable to read " + problem + " for CostCentre " + costCentreCode +
                                        " Account " + accountCode);
                                    SubmissionOK = false;
                                }
                            } // foreach ledgerRow

                        } // if TransactionsChanged

                    }

                    // save changes to exchange rates
                    ACorporateExchangeRateAccess.SubmitChanges(InspectDS.ACorporateExchangeRate, Transaction);
                }); // GetNewOrExistingAutoTransaction

            TSubmitChangesResult SubmissionResult;

            if (SubmissionOK)
            {
                SubmissionResult = TSubmitChangesResult.scrOK;
            }
            else
            {
                SubmissionResult = TSubmitChangesResult.scrError;
            }

            AInspectDS = InspectDS;
            ATransactionsChanged = TransactionsChanged;

            return SubmissionResult;
        }

        /// <summary>
        /// Returns true if a Corporate Exchange Rate can be deleted.
        /// Cannot be deleted if it is effective for a period in the current year which has at least one batch.
        /// </summary>
        /// <param name="ADateEffectiveFrom">Corporate Exchange Rate's Date Effective From</param>
        /// <param name="AIntlCurrency"></param>
        /// <param name="ATransactionCurrency"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CanDeleteCorporateExchangeRate(DateTime ADateEffectiveFrom, string AIntlCurrency, string ATransactionCurrency)
        {
            bool ReturnValue = true;
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    // get accounting period for when the exchange rate is effective (if it exists)
                    string Query = "SELECT * FROM a_accounting_period" +
                                   " WHERE a_accounting_period.a_period_end_date_d >= '" + DataUtilities.DateToSQLString(ADateEffectiveFrom) + "'" +
                                   " AND a_accounting_period.a_period_start_date_d <= '" + DataUtilities.DateToSQLString(ADateEffectiveFrom) + "'";

                    AAccountingPeriodTable AccountingPeriodTable = new AAccountingPeriodTable();
                    DBAccess.GDBAccessObj.SelectDT(AccountingPeriodTable, Query, ReadTransaction);

                    // no accounting period if effective in a year other that the current year
                    if ((AccountingPeriodTable == null) || (AccountingPeriodTable.Rows.Count == 0))
                    {
                        return;
                    }

                    AAccountingPeriodRow AccountingPeriodRow = AccountingPeriodTable[0];

                    // search for batches for the found accounting period
                    string Query2 = "SELECT CASE WHEN EXISTS (" +
                                    "SELECT * FROM a_batch, a_journal, a_ledger" +
                                    " WHERE a_batch.a_date_effective_d <= '" + DataUtilities.DateToSQLString(
                        AccountingPeriodRow.PeriodEndDate) + "'" +
                                    " AND a_batch.a_date_effective_d >= '" + DataUtilities.DateToSQLString(
                        AccountingPeriodRow.PeriodStartDate) + "'" +
                                    " AND a_journal.a_ledger_number_i = a_batch.a_ledger_number_i" +
                                    " AND a_journal.a_batch_number_i = a_batch.a_batch_number_i" +
                                    " AND a_ledger.a_ledger_number_i = a_batch.a_ledger_number_i" +
                                    " AND ((a_journal.a_transaction_currency_c = '" + ATransactionCurrency + "'" +
                                    " AND a_ledger.a_intl_currency_c = '" + AIntlCurrency + "')" +
                                    " OR (a_journal.a_transaction_currency_c = '" + AIntlCurrency + "'" +
                                    " AND a_ledger.a_intl_currency_c = '" + ATransactionCurrency + "'))" +
                                    ") THEN 'TRUE'" +
                                    " ELSE 'FALSE' END";

                    DataTable DT = DBAccess.GDBAccessObj.SelectDT(Query2, "temp", ReadTransaction);

                    // a batch has been found
                    if ((DT != null) && (DT.Rows.Count > 0) && (DT.Rows[0][0].ToString() == "TRUE"))
                    {
                        ReturnValue = false;
                        return;
                    }
                });

            return ReturnValue;
        }
    }
}