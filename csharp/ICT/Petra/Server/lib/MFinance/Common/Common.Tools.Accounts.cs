//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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

using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// This object handles the table AccountHierarchyDetailInfo and provides some standard
    /// procedures.
    /// </summary>
    public class TGetAccountHierarchyDetailInfo
    {
        /// <summary>
        /// A AChildLevel value which defines to search the children and all subchildren of a
        /// given parent.
        /// </summary>
        public const int GET_ALL_LEVELS = -1;

        private const string STANDARD = "STANDARD";
        AAccountHierarchyDetailTable accountTable;
        AAccountHierarchyDetailRow accountRow = null;
        TLedgerInfo ledgerInfo;


        /// <summary>
        /// ...
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public TGetAccountHierarchyDetailInfo(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;

            bool NewTransaction = false;

            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                accountTable = AAccountHierarchyDetailAccess.LoadViaALedger(
                    ledgerInfo.LedgerNumber, transaction);
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
        /// The idea of this routine is given by x_clist.i
        /// of course the data are read only one time (Constructor) and the results are put
        /// into a list for a more specific use.
        /// All children and sub children are listed ...
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public List <String>GetChildren(string AAccountCode)
        {
            List <String>help = new List <String>();
            GetChildrenIntern(help, AAccountCode, GET_ALL_LEVELS);
            return help;
        }

        /// <summary>
        /// See ChildList definition without AChildLevel, here you can define your own
        /// ChildLevel.
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <param name="AChildLevel">Level counting starts with 1</param>
        /// <returns></returns>
        public List <String>GetChildren(string AAccountCode, int AChildLevel)
        {
            List <String>help = new List <String>();
            GetChildrenIntern(help, AAccountCode, --AChildLevel);
            return help;
        }

        private void GetChildrenIntern(IList <String>help, string AAccountCode, int AChildLevel)
        {
            if (accountTable.Rows.Count > 0)
            {
                accountTable.DefaultView.Sort =
                    AAccountHierarchyDetailTable.GetReportOrderDBName() + ", " +
                    AAccountHierarchyDetailTable.GetReportingAccountCodeDBName();

                foreach (DataRowView rv in accountTable.DefaultView)
                {
                    accountRow = (AAccountHierarchyDetailRow)rv.Row;

                    if (accountRow.AccountCodeToReportTo.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            help.Add(accountRow.ReportingAccountCode);

                            if (AChildLevel != 0)
                            {
                                GetChildrenIntern(help, accountRow.ReportingAccountCode, --AChildLevel);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If you only want to know, that the account code has no children, you can avoid the
        /// handling of the list.
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public bool HasNoChildren(string AAccountCode)
        {
            if (accountTable.Rows.Count > 0)
            {
                for (int i = 0; i < accountTable.Rows.Count; ++i)
                {
                    accountRow = (AAccountHierarchyDetailRow)accountTable.Rows[i];

                    if (accountRow.AccountCodeToReportTo.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the parent account of an inserted account ...
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public string GetParentAccount(string AAccountCode)
        {
            if (accountTable.Rows.Count > 0)
            {
                for (int i = 0; i < accountTable.Rows.Count; ++i)
                {
                    accountRow = (AAccountHierarchyDetailRow)accountTable.Rows[i];

                    if (accountRow.ReportingAccountCode.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            return accountRow.AccountCodeToReportTo;
                        }
                    }
                }
            }

            return String.Empty;
        }
    }

    /// <summary>
    /// A Enum-list of some special account codes ...
    /// https://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_Conversion_from_Petra_to_Openpetra
    /// </summary>
    public enum TAccountPropertyEnum
    {
        /// <summary>
        ///
        /// </summary>
        GIFT_HEADING,               // GIFT

        /// <summary>
        ///
        /// </summary>
        INTER_LEDGER_HEADING,       // ILT

        /// <summary>
        ///
        /// </summary>
        BANK_HEADING,               // CASH

        /// <summary>
        ///
        /// </summary>
        BALANCE_SHEET_HEADING,      // BAL

        //PROFIT_AND_LOSS_HEADING,    // PL
        //INCOME_HEADING,             // INC
        //EXPENSE_HEADING,            // EXP

        /// <summary>
        ///
        /// </summary>
        DEBTOR_HEADING,             // DRS


        /// <summary>
        ///
        /// </summary>
        CREDITOR_HEADING,           // CRS

        /// <summary>
        ///
        /// </summary>
        TOTAL_ASSET_HEADING,        // ASSETS

        /// <summary>
        ///
        /// </summary>
        TOTAL_LIABILITY_HEADING,    // LIABS

        /// <summary>
        ///
        /// </summary>
        EQUITY_HEADING,             // RET EARN


        /// <summary>
        ///
        /// </summary>
        EARNINGS_BF_ACCT,           // 9700

        /// <summary>
        ///
        /// </summary>
        DIRECT_XFER_ACCT,           // 5501

        /// <summary>
        ///
        /// </summary>
        ICH_SETTLEMENT_ACCT,        // 5601

        /// <summary>
        ///
        /// </summary>
        ICH_ACCT,                   // 8500

        /// <summary>
        ///
        /// </summary>
        INTERNAL_XFER_ACCT,         // 9800

        /// <summary>
        ///
        /// </summary>
        ADMIN_FEE_INCOME_ACCT,      // 3400

        /// <summary>
        ///
        /// </summary>
        ADMIN_FEE_EXPENSE_ACCT,     // 4900

        /// <summary>
        ///
        /// </summary>
        FUND_TRANSFER_INCOME_ACCT,  // 3300

        /// <summary>
        ///
        /// </summary>
        FUND_TRANSFER_EXPENSE_ACCT, // 4800
    }

    /// <summary>
    /// A handler to the special accounts in TAccountPropertyEnum
    /// </summary>
    public class THandleAccountPropertyInfo
    {
        AAccountPropertyTable propertyCodeTable;
        /// <summary>
        /// The constructor needs a lederinfo (for the ledger number)
        /// </summary>
        /// <param name="ledgerInfo"></param>
        public THandleAccountPropertyInfo(TLedgerInfo ledgerInfo)
        {
            try
            {
                bool NewTransaction;
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                propertyCodeTable = AAccountPropertyAccess.LoadViaALedger(
                    ledgerInfo.LedgerNumber, transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Get access on a special account ...
        /// </summary>
        /// <param name="AEnum"></param>
        /// <returns></returns>
        public string GetAccountCode(TAccountPropertyEnum AEnum)
        {
            try
            {
                AAccountPropertyRow row;

                for (int i = 0; i < propertyCodeTable.Rows.Count; ++i)
                {
                    row = (AAccountPropertyRow)propertyCodeTable[i];

                    if (row.PropertyCode.Equals("Is_Special_Account"))
                    {
                        if (row.PropertyValue.Equals(AEnum.ToString()))
                        {
                            return row.AccountCode;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="APropertyCode"></param>
        /// <returns></returns>
        public string GetAccountCode(string APropertyCode)
        {
            try
            {
                AAccountPropertyRow row;

                for (int i = 0; i < propertyCodeTable.Rows.Count; ++i)
                {
                    row = (AAccountPropertyRow)propertyCodeTable[i];

                    if (row.PropertyCode.Equals(APropertyCode))
                    {
                        return row.PropertyValue;
                    }
                }
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }
    }


    /// <summary>
    /// TAccountInfo uses a TLedgerInfo a primilary references the LedgerNumber.
    /// All Accounts are load in both contructors. You can define an inital account code in the
    /// second constructor or you can set the value later (or change) by using SetAccountRowTo.
    /// Then you can read the values for the selected Account.
    /// </summary>
    public class TAccountInfo
    {
        AAccountTable accountTable;
        AAccountRow accountRow = null;
        TLedgerInfo ledgerInfo;
        THandleAccountPropertyInfo tAccountPropertyHandler = null;

        int iPtr;

        /// <summary>
        /// This mininmal constructor defines the result collection for the error messages and
        /// Ledger Info to select the ledger ...
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public TAccountInfo(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
            LoadData();
        }

        /// <summary>
        /// The Constructor defines a first value of a specific accounting code too.
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        /// <param name="AAccountCode"></param>
        public TAccountInfo(TLedgerInfo ALedgerInfo, string AAccountCode)
        {
            ledgerInfo = ALedgerInfo;
            LoadData();
            AccountCode = AAccountCode;
        }

        private void LoadData()
        {
            try
            {
                bool NewTransaction;
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                accountTable = AAccountAccess.LoadViaALedger(
                    ledgerInfo.LedgerNumber, transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }
            accountRow = null;
        }

        /// <summary>
        /// TAccountPropertyEnum defines a set of special accounts and here one of them
        /// can be selected. ..
        /// </summary>
        /// <param name="AENum"></param>
        public void SetSpecialAccountCode(TAccountPropertyEnum AENum)
        {
            accountRow = null;

            if (tAccountPropertyHandler == null)
            {
                tAccountPropertyHandler = new THandleAccountPropertyInfo(ledgerInfo);
            }

            string account = tAccountPropertyHandler.GetAccountCode(AENum);

            if (!account.Equals(string.Empty))
            {
                AccountCode = account;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <returns></returns>
        public string SetCarryForwardAccount()
        {
            accountRow = null;

            if (tAccountPropertyHandler == null)
            {
                tAccountPropertyHandler = new THandleAccountPropertyInfo(ledgerInfo);
            }

            string result = tAccountPropertyHandler.GetAccountCode("CARRYFORWARDCC");

            if (!result.Equals(string.Empty))
            {
                string[] arrStrHelp = result.Split(new Char[] { ',' });
                AccountCode = arrStrHelp[0];
                return arrStrHelp[1];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// The Account code can be read and the result is the account code of the row
        /// which was selected before. <br />
        /// The Account can be written and this will change the selected row without any
        /// database request. The result may be invalid.
        /// </summary>
        public string AccountCode
        {
            get
            {
                if (accountRow == null)
                {
                    return String.Empty;
                }
                else
                {
                    return accountRow.AccountCode;
                }
            }
            set
            {
                if (value.Equals(String.Empty))
                {
                    accountRow = null;
                }
                else
                {
                    accountRow = null;

                    for (int i = 0; i < accountTable.Rows.Count; ++i)
                    {
                        if (value.Equals(((AAccountRow)accountTable[i]).AccountCode))
                        {
                            accountRow = (AAccountRow)accountTable[i];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public string AccountType
        {
            get
            {
                return accountRow.AccountType;
            }
        }

        /// <summary>
        /// Reset the row list
        /// </summary>
        public void Reset()
        {
            iPtr = -1;
        }

        /// <summary>
        /// Move to the next row ...
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            ++iPtr;

            if (iPtr < accountTable.Rows.Count)
            {
                accountRow = (AAccountRow)accountTable[iPtr];
                return true;
            }
            else
            {
                accountRow = null;
                return false;
            }
        }

        /// <summary>
        /// Informs that a row selection is valid and the row properties will not produce
        /// an exception.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !(accountRow == null);
            }
        }

        /// <summary>
        /// Standard property of an AAccountRow
        /// </summary>
        public bool ForeignCurrencyFlag
        {
            get
            {
                return accountRow.ForeignCurrencyFlag;
            }
        }

        /// <summary>
        /// Standard property of an AAccountRow
        /// </summary>
        public string ForeignCurrencyCode
        {
            get
            {
                return accountRow.ForeignCurrencyCode;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool PostingStatus
        {
            get
            {
                return accountRow.PostingStatus;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool DebitCreditIndicator
        {
            get
            {
                return accountRow.DebitCreditIndicator;
            }
        }
    }

    /// <summary>
    /// Gets the specific date informations of an accounting intervall. This routine is either used by
    /// GL.PeriodEnd.Month and GL.Revaluation but in different senses. On time the dataset holds exact
    /// one row (Contructor with two parameters) and on time it holds a set of rows (Constructor with
    /// one parameter.
    /// </summary>
    public class TAccountPeriodInfo
    {
        private int intLedgerNumber = 0;
        private AAccountingPeriodTable periodTable = null;
        private AAccountingPeriodRow periodRow = null;


        /// <summary>
        ///
        /// </summary>
        protected void LoadTableData(int ALedgerNumber)
        {
            intLedgerNumber = ALedgerNumber;
            LoadData();
        }

        /// <summary>
        /// Constructor needs a valid ledger number.
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        public TAccountPeriodInfo(int ALedgerNumber)
        {
            intLedgerNumber = ALedgerNumber;
            LoadData();
        }

        /// <summary>
        /// Constructor to adress a record by its primary key
        /// </summary>
        /// <param name="ALedgerNumber">the ledger number</param>
        /// <param name="ACurrentPeriod">the current accounting period</param>

        public TAccountPeriodInfo(int ALedgerNumber, int ACurrentPeriod)
        {
            intLedgerNumber = ALedgerNumber;
            LoadData();
            AccountingPeriodNumber = ACurrentPeriod;
        }

        private void LoadData()
        {
            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                periodTable = AAccountingPeriodAccess.LoadViaALedger(intLedgerNumber, transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int AccountingPeriodNumber
        {
            set
            {
                periodRow = null;
                AAccountingPeriodRow periodRowH;

                for (int i = 0; i < periodTable.Rows.Count; ++i)
                {
                    periodRowH = periodTable[i];

                    if (periodRowH.AccountingPeriodNumber == value)
                    {
                        periodRow = periodRowH;
                    }
                }
            }
        }


        /// <summary>
        ///
        /// </summary>
        public DateTime PeriodEndDate
        {
            get
            {
                return periodRow.PeriodEndDate;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public DateTime PeriodStartDate
        {
            get
            {
                return periodRow.PeriodStartDate;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public DateTime EffectiveDate
        {
            get
            {
                return periodRow.EffectiveDate;
            }
        }


        /// <summary>
        /// Returns the number of accounting periods in the table
        /// </summary>
        public int Rows
        {
            get
            {
                return periodTable.Rows.Count;
            }
        }
    }


    /// <summary>
    /// Different Account Types ...
    /// </summary>
    public enum TAccountTypeEnum
    {
        /// <summary>
        ///
        /// </summary>
        Income,

        /// <summary>
        ///
        /// </summary>
        Expense,

        /// <summary>
        ///
        /// </summary>
        Asset,

        /// <summary>
        ///
        /// </summary>
        Equity,

        /// <summary>
        ///
        /// </summary>
        Liability
    }
}