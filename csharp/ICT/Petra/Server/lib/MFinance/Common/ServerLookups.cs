//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared;
//using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;

namespace Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MFinance.Common.ServerLookups
    /// sub-namespace.
    ///
    /// </summary>
    public class TFinanceServerLookups
    {
        /// <summary>
        /// Returns starting and ending posting dates for the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            DateTime StartDateCurrentPeriod = new DateTime();
            DateTime EndDateLastForwardingPeriod = new DateTime();
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                    int FirstPostingPeriod = -1;

                    // If final month end has been run but year end has not yet been run
                    // then we cannot post to the current period as it is actually closed.
                    if (LedgerTable[0].ProvisionalYearEndFlag)
                    {
                        FirstPostingPeriod = LedgerTable[0].CurrentPeriod + 1;
                    }
                    else
                    {
                        FirstPostingPeriod = LedgerTable[0].CurrentPeriod;
                    }

                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        FirstPostingPeriod,
                        Transaction);

                    StartDateCurrentPeriod = AccountingPeriodTable[0].PeriodStartDate;

                    AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        LedgerTable[0].CurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods,
                        Transaction);
                    EndDateLastForwardingPeriod = AccountingPeriodTable[0].PeriodEndDate;
                });

            AStartDateCurrentPeriod = StartDateCurrentPeriod;
            AEndDateLastForwardingPeriod = EndDateLastForwardingPeriod;

            return true;
        }

        /// <summary>
        /// return information if ledger with given number has suspense accounts set up
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean HasSuspenseAccounts(Int32 ALedgerNumber)
        {
            Boolean ReturnValue = false;
            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    ReturnValue = (ASuspenseAccountAccess.CountViaALedger(ALedgerNumber, transaction) > 0);
                });
            return ReturnValue;
        }

        /// <summary>
        /// return partner key associated with cost centre code in a_valid_ledger_number table
        /// returns false if cost centre type is not "Foreign" or if cost centre cannot be found in a_valid_ledger_number table
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetPartnerKeyForForeignCostCentreCode(Int32 ALedgerNumber, String ACostCentreCode, out Int64 APartnerKey)
        {
            Boolean ReturnValue = false;

            Int64 PartnerKey = 0;

            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    ACostCentreTable CostCentreTable;
                    CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentreCode, transaction);

                    if (CostCentreTable.Count > 0)
                    {
                        ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];

                        if (CostCentreRow.CostCentreType == MFinanceConstants.FOREIGN_CC_TYPE)
                        {
                            AValidLedgerNumberTable ValidLedgerNumberTable;
                            AValidLedgerNumberRow ValidLedgerNumberRow;
                            ValidLedgerNumberTable = AValidLedgerNumberAccess.LoadViaACostCentre(ALedgerNumber, ACostCentreCode, transaction);

                            if (ValidLedgerNumberTable.Count > 0)
                            {
                                ValidLedgerNumberRow = (AValidLedgerNumberRow)ValidLedgerNumberTable.Rows[0];
                                PartnerKey = ValidLedgerNumberRow.PartnerKey;
                                ReturnValue = true;
                            }
                        }
                    }
                });

            APartnerKey = PartnerKey;
            return ReturnValue;
        }

        /// <summary>
        /// return ledger's base currency
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetLedgerBaseCurrency(Int32 ALedgerNumber)
        {
            string ReturnValue = "";
            TDBTransaction ReadTransaction = null;

            // Need to use 'GetNewOrExistingAutoReadTransaction' rather than 'BeginAutoReadTransaction' to allow
            // opening of the 'GL Transaction Find' screen while a Report is calculating (Bug #3879).
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    ReturnValue = ((ALedgerRow)ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ReadTransaction).Rows[0]).BaseCurrency;
                });

            return ReturnValue;
        }

        /// <summary>
        /// Get Foreign Currency Accounts' YTD Actuals
        /// </summary>
        /// <param name="AForeignCurrencyAccounts">DataTable containing rows of Foreign Currency Accounts</param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void GetForeignCurrencyAccountActuals(ref DataTable AForeignCurrencyAccounts, Int32 ALedgerNumber, Int32 AYear)
        {
            //string ReturnValue = "";
            DataTable ForeignCurrencyAccounts = AForeignCurrencyAccounts.Clone();

            ForeignCurrencyAccounts.Merge(AForeignCurrencyAccounts);
            string CostCentreCode = "[" + ALedgerNumber + "]";
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref Transaction,
                delegate
                {
                    foreach (DataRow ForeignCurrencyAccountRow in ForeignCurrencyAccounts.Rows)
                    {
                        AGeneralLedgerMasterTable Table = AGeneralLedgerMasterAccess.LoadByUniqueKey(
                            ALedgerNumber, AYear, ForeignCurrencyAccountRow[AAccountTable.GetAccountCodeDBName()].ToString(), CostCentreCode,
                            Transaction);

                        if ((Table != null) && (Table.Rows.Count > 0))
                        {
                            AGeneralLedgerMasterRow Row = Table[0];

                            if (Row.IsYtdActualForeignNull())
                            {
                                ForeignCurrencyAccountRow[AGeneralLedgerMasterTable.GetYtdActualForeignDBName()] = 0;
                            }
                            else
                            {
                                ForeignCurrencyAccountRow[AGeneralLedgerMasterTable.GetYtdActualForeignDBName()] = Row.YtdActualForeign;
                            }

                            ForeignCurrencyAccountRow[AGeneralLedgerMasterTable.GetYtdActualBaseDBName()] = Row.YtdActualBase;
                        }
                        else
                        {
                            ForeignCurrencyAccountRow[AGeneralLedgerMasterTable.GetYtdActualForeignDBName()] = 0;
                            ForeignCurrencyAccountRow[AGeneralLedgerMasterTable.GetYtdActualBaseDBName()] = 0;
                        }
                    }
                });

            AForeignCurrencyAccounts = ForeignCurrencyAccounts;
        }

        /// <summary>
        /// Returns CurrencyLanguageRow for a corresponding currency code
        /// </summary>
        /// <param name="ACurrencyCode">Currency Code</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static ACurrencyLanguageRow GetCurrencyLanguage(string ACurrencyCode)
        {
            ACurrencyLanguageRow ReturnValue = null;
            string Language = TUserDefaults.GetStringDefault(MSysManConstants.USERDEFAULT_UILANGUAGE);

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ACurrencyLanguageTable CurrencyLanguageTable = ACurrencyLanguageAccess.LoadByPrimaryKey(ACurrencyCode, Language, Transaction);

                    if ((CurrencyLanguageTable != null) && (CurrencyLanguageTable.Rows.Count > 0))
                    {
                        ReturnValue = CurrencyLanguageTable[0];
                    }
                });

            return ReturnValue;
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
                                   " WHERE a_accounting_period.a_period_end_date_d >= '" + ADateEffectiveFrom + "'" +
                                   " AND a_accounting_period.a_period_start_date_d <= '" + ADateEffectiveFrom + "'";

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
                                    " WHERE a_batch.a_date_effective_d <= '" + AccountingPeriodRow.PeriodEndDate + "'" +
                                    " AND a_batch.a_date_effective_d >= '" + AccountingPeriodRow.PeriodStartDate + "'" +
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

        /// <summary>
        /// return table with forms for given form code and form type code
        /// </summary>
        /// <param name="AFormCode">Form Code Filter</param>
        /// <param name="AFormTypeCode">Form Type Code Filter, ignore this filter if empty string</param>
        /// <returns>Result Form Table</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AFormTable GetForms(TFinanceFormCodeEnum AFormCode, String AFormTypeCode)
        {
            AFormTable ResultTable = new AFormTable();
            AFormRow TemplateRow = ResultTable.NewRowTyped(false);

            switch (AFormCode)
            {
                case TFinanceFormCodeEnum.ffcReceipt:
                    TemplateRow.FormCode = MFinanceConstants.FORM_CODE_RECEIPT;
                    break;

                case TFinanceFormCodeEnum.ffcCheque:
                    TemplateRow.FormCode = MFinanceConstants.FORM_CODE_CHEQUE;
                    break;

                case TFinanceFormCodeEnum.ffcRemittance:
                    TemplateRow.FormCode = MFinanceConstants.FORM_CODE_REMITTANCE;
                    break;

                default:
                    break;
            }

            if (AFormTypeCode != "")
            {
                TemplateRow.FormTypeCode = AFormTypeCode;
            }

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ResultTable = AFormAccess.LoadUsingTemplate(TemplateRow, Transaction);
                });

            return ResultTable;
        }
    }
}