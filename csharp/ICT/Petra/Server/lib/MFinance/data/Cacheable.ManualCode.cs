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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core;

using System.Collections.Generic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.DataAggregates;
using Ict.Petra.Shared.MFinance.Account.Validation;
using Ict.Petra.Shared.MFinance.Gift.Validation;
using Ict.Petra.Shared.MFinance.AP.Validation;

namespace Ict.Petra.Server.MFinance.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MFinance sub-namespace
    /// that can be cached on the Client side.
    /// </summary>
    public partial class TCacheable
    {
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Wrapper for other GetCacheableTable method
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <returns>DataTable</returns>
        public DataTable GetCacheableTable(TCacheableFinanceTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
        }

        private DataTable GetAccountingPeriodListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            StringCollection FieldList = new StringCollection();

            FieldList.Add(AAccountingPeriodTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodNumberDBName());
            FieldList.Add(AAccountingPeriodTable.GetAccountingPeriodDescDBName());
            FieldList.Add(AAccountingPeriodTable.GetPeriodStartDateDBName());
            FieldList.Add(AAccountingPeriodTable.GetPeriodEndDateDBName());
            return AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
        }

        private DataTable GetLedgerNameListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            DataTable LedgerTable;

            LedgerTable = TALedgerNameAggregate.GetData(ATableName, AReadTransaction);
            LedgerTable.PrimaryKey = new DataColumn[] {
                LedgerTable.Columns[0]
            };
            return LedgerTable;
        }

        private DataTable GetLedgerDetailsTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
//            StringCollection FieldList = new StringCollection();
//            FieldList.Add(ALedgerTable.GetLedgerNumberDBName());
//            FieldList.Add(ALedgerTable.GetNumberFwdPostingPeriodsDBName());
//            FieldList.Add(ALedgerTable.GetNumberOfAccountingPeriodsDBName());
//            FieldList.Add(ALedgerTable.GetCurrentPeriodDBName());
//            FieldList.Add(ALedgerTable.GetCurrentFinancialYearDBName());
//            FieldList.Add(ALedgerTable.GetBranchProcessingDBName());
//            FieldList.Add(ALedgerTable.GetBaseCurrencyDBName());
//            FieldList.Add(ALedgerTable.GetIntlCurrencyDBName());
            return ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, AReadTransaction);
        }

        private DataTable GetCostCentreListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            StringCollection FieldList = new StringCollection();

            FieldList.Add(ACostCentreTable.GetLedgerNumberDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreCodeDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreNameDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreToReportToDBName());
            FieldList.Add(ACostCentreTable.GetPostingCostCentreFlagDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreActiveFlagDBName());
            FieldList.Add(ACostCentreTable.GetCostCentreTypeDBName());
            return ACostCentreAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
        }

        private DataTable GetCostCentresLinkedToPartnerListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            DataTable CostCentreTable;

            CostCentreTable = TCostCentresLinkedToPartner.GetData(ATableName, ALedgerNumber, AReadTransaction);
            CostCentreTable.PrimaryKey = new DataColumn[] {
                CostCentreTable.Columns[0]
            };
            return CostCentreTable;
        }

        private DataTable GetICHStewardshipListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            StringCollection FieldList = new StringCollection();

            FieldList.Add(AIchStewardshipTable.GetLedgerNumberDBName());
            FieldList.Add(AIchStewardshipTable.GetCostCentreCodeDBName());
            FieldList.Add(AIchStewardshipTable.GetPeriodNumberDBName());
            FieldList.Add(AIchStewardshipTable.GetIchNumberDBName());
            FieldList.Add(AIchStewardshipTable.GetDateProcessedDBName());
            return AIchStewardshipAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
        }

        private DataTable GetAccountListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            StringCollection FieldList = new StringCollection();

            FieldList.Add(AAccountTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountTable.GetAccountCodeDBName());
            FieldList.Add(AAccountTable.GetAccountCodeShortDescDBName());
            FieldList.Add(AAccountTable.GetAccountActiveFlagDBName());
            FieldList.Add(AAccountTable.GetPostingStatusDBName());
            FieldList.Add(AAccountTable.GetForeignCurrencyFlagDBName());
            FieldList.Add(AAccountTable.GetForeignCurrencyCodeDBName());
            GLSetupTDS TempDS = new GLSetupTDS();
            AAccountAccess.LoadViaALedger(TempDS, ALedgerNumber, FieldList, AReadTransaction);

            // load AAccountProperty and set the BankAccountFlag
            AAccountPropertyAccess.LoadViaALedger(TempDS, ALedgerNumber, AReadTransaction);

            foreach (AAccountPropertyRow accProp in TempDS.AAccountProperty.Rows)
            {
                if ((accProp.PropertyCode == MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT) && (accProp.PropertyValue == "true"))
                {
                    TempDS.AAccount.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        AAccountTable.GetAccountCodeDBName(),
                        accProp.AccountCode);
                    GLSetupTDSAAccountRow acc = (GLSetupTDSAAccountRow)TempDS.AAccount.DefaultView[0].Row;
                    acc.BankAccountFlag = true;
                    TempDS.AAccount.DefaultView.RowFilter = "";
                }
            }

            // load AAccountHierarchyDetails and check if this account reports to the CASH account
            AAccountHierarchyDetailAccess.LoadViaAAccountHierarchy(TempDS,
                ALedgerNumber,
                MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD,
                AReadTransaction);

            TLedgerInfo ledgerInfo = new TLedgerInfo(ALedgerNumber);
            TGetAccountHierarchyDetailInfo accountHierarchyTools = new TGetAccountHierarchyDetailInfo(ledgerInfo);
            List <string>children = accountHierarchyTools.GetChildren(MFinanceConstants.CASH_ACCT);

            foreach (GLSetupTDSAAccountRow account in TempDS.AAccount.Rows)
            {
                if (children.Contains(account.AccountCode))
                {
                    account.CashAccountFlag = true;
                }
            }

            return TempDS.AAccount;
        }

        private DataTable GetAccountHierarchyListTable(TDBTransaction AReadTransaction, System.Int32 ALedgerNumber, string ATableName)
        {
            StringCollection FieldList = new StringCollection();

            FieldList.Add(AAccountHierarchyTable.GetLedgerNumberDBName());
            FieldList.Add(AAccountHierarchyTable.GetAccountHierarchyCodeDBName());
            return AAccountHierarchyAccess.LoadViaALedger(ALedgerNumber, FieldList, AReadTransaction);
        }
    }
}