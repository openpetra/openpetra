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
                    AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                        LedgerTable[0].CurrentPeriod,
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
            Boolean ReturnValue;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ReturnValue = (ASuspenseAccountAccess.CountViaALedger(ALedgerNumber, Transaction) > 0);

            DBAccess.GDBAccessObj.RollbackTransaction();

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

            APartnerKey = 0;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ACostCentreTable CostCentreTable;
            CostCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentreCode, Transaction);

            if (CostCentreTable.Count > 0)
            {
                ACostCentreRow CostCentreRow = (ACostCentreRow)CostCentreTable.Rows[0];

                if (CostCentreRow.CostCentreType == MFinanceConstants.FOREIGN_CC_TYPE)
                {
                    AValidLedgerNumberTable ValidLedgerNumberTable;
                    AValidLedgerNumberRow ValidLedgerNumberRow;
                    ValidLedgerNumberTable = AValidLedgerNumberAccess.LoadViaACostCentre(ALedgerNumber, ACostCentreCode, Transaction);

                    if (ValidLedgerNumberTable.Count > 0)
                    {
                        ValidLedgerNumberRow = (AValidLedgerNumberRow)ValidLedgerNumberTable.Rows[0];
                        APartnerKey = ValidLedgerNumberRow.PartnerKey;
                        ReturnValue = true;
                    }
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

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
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref Transaction,
                delegate
                {
                    ReturnValue = ((ALedgerRow)ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction).Rows[0]).BaseCurrency;
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
                            ALedgerNumber, AYear, ForeignCurrencyAccountRow[AAccountTable.GetAccountCodeDBName()].ToString(), CostCentreCode, Transaction);

                        if (Table != null && Table.Rows.Count > 0)
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

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    ACurrencyLanguageTable CurrencyLanguageTable = ACurrencyLanguageAccess.LoadByPrimaryKey(ACurrencyCode, Language, Transaction);

                    if (CurrencyLanguageTable != null && CurrencyLanguageTable.Rows.Count > 0)
                    {
                        ReturnValue = CurrencyLanguageTable[0];
                    }
                });

            return ReturnValue;
        }
    }
}