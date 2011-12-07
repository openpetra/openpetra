//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, wolfgangu
//
// Copyright 2004-2011 by OM International
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

namespace Ict.Petra.Shared.MFinance
{
    /// <summary>
    /// some constants used in the finance module
    /// </summary>
    public class MFinanceConstants
    {
        /// <summary>GL Batch</summary>
        public const String BATCH_UNPOSTED = "Unposted";

        /// <summary>GL Batch</summary>
        public const String BATCH_POSTED = "Posted";

        /// <summary>GL Batch</summary>
        public const String BATCH_CANCELLED = "Cancelled";

        /// <summary>Subsystem and Transaction Types</summary>
        public const String GENERAL_LEDGER = "GL";

        /// <summary>Subsystem and Transaction Types</summary>
        public const String STANDARD_JOURNAL = "STD";

        /// <summary>Subsystem and Transaction Types</summary>
        public const String CASHBOOK_RECEIPTING = "CBR";

        /// <summary>Subsystem and Transaction Types</summary>
        public const String CASHBOOK_PAYMENT = "CBP";

        /// <summary>Subsystem and Transaction Types</summary>
        public const String GIFT_RECEIPTING = "GR";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_TYPE_ASSET = "Asset";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_TYPE_LIABILITY = "Liability";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_TYPE_INCOME = "Income";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_TYPE_EXPENSE = "Expense";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_TYPE_EQUITY = "Equity";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_HIERARCHY_STANDARD = "STANDARD";

        /// <summary>General Ledger</summary>
        public const String ACCOUNT_HIERARCHY_CODE = "STANDARD";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_OPEN = "OPEN";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_APPROVED = "APPROVED";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_POSTED = "POSTED";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_PARTIALLY_PAID = "PARTPAID";

        /// <summary>Accounts Payable</summary>
        public const String AP_DOCUMENT_PAID = "PAID";

        /// <summary>Account Property</summary>
        public const String ACCOUNT_PROPERTY_BANK_ACCOUNT = "BANK ACCOUNT";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED = "MATCHED";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_GIFT = "MATCHED-GIFT";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_GL = "MATCHED-GL";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_MATCHED_AP = "MATCHED-AP";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_NO_MATCHING = "DONT-MATCH";

        /// <summary>Bank statements</summary>
        public const String BANK_STMT_STATUS_UNMATCHED = "UNMATCHED";

        /// <summary>Sub Systems, General Ledger</summary>
        public const String SUB_SYSTEM_GL = "GL";

        /// <summary>Sub Systems, Accounts Payable</summary>
        public const String SUB_SYSTEM_AP = "AP";

        /// <summary>Sub Systems, Accounts Receivable</summary>
        public const String SUB_SYSTEM_AR = "AR";

        /// <summary>Sub Systems, Gifts receivable</summary>
        public const String SUB_SYSTEM_GR = "GR";

        /// <summary>Unit-Type is key-min</summary>
        public const String GROUP_DETAIL_KEY_MIN = "KEYMIN";

        /// <summary>Used in Admin Grants Payable and Receivable</summary>
        public const string ADMIN_FEE_INCOME_ACCT = "3400";

        /// <summary>Used in Admin Grants Payable and Receivable</summary>
        public const string ADMIN_FEE_EXPENSE_ACCT = "4900";

        /// <summary>Used in Admin Fee Charge Options</summary>
        public const string ADMIN_CHARGE_OPTION_MAX = "MAXIMUM";

        /// <summary>Used in Admin Fee Charge Options</summary>
        public const string ADMIN_CHARGE_OPTION_MIN = "MINIMUM";

        /// <summary>Used in Admin Fee Charge Options</summary>
        public const string ADMIN_CHARGE_OPTION_FIXED = "FIXED";

        /// <summary>Used in Admin Fee Charge Options</summary>
        public const string ADMIN_CHARGE_OPTION_PERCENT = "PERCENTAGE";

        /// <summary>Sets the transaction to a debit transaction</summary>
        public const bool IS_DEBIT = true;

        /// <summary>Sets the transaction to a credit transaction</summary>
        public const bool IS_CREDIT = false;

        /// Logical values for debit and credit transactions.
        public const bool POSTED = true;

        /// standard account for earnings bf
        public const string EARNINGS_BF_ACCT = "9700";


        //System account/summary names //
        /// standard account summary name
        public const string GIFT_HEADING = "GIFT";

        /// standard account summary name
        public const string INTER_LEDGER_HEADING = "ILT";

        /// standard account summary name
        public const string BANK_HEADING = "CASH";

        /// standard account summary name
        public const string BALANCE_SHEET_HEADING = "BAL SHT";

        /// standard account summary name
        public const string PROFIT_AND_LOSS_HEADING = "PL";

        /// standard account summary name
        public const string INCOME_HEADING = "INC";

        /// standard account summary name
        public const string EXPENSE_HEADING = "EXP";

        /// standard account summary name
        public const string DEBTOR_HEADING = "DRS";

        /// standard account summary name
        public const string CREDITOR_HEADING = "CRS";

        /// standard account summary name
        public const string TOTAL_ASSET_HEADING = "ASSETS";

        /// standard account summary name
        public const string TOTAL_LIABILITY_HEADING = "LIABS";

        /// standard account summary name
        public const string EQUITY_HEADING = "RET EARN";

        /// standard account summary name
        public const string DIRECT_XFER_ACCT = "5501";

        /// standard account
        public const string ICH_SETTLEMENT_ACCT = "5601";

        /// standard account
        public const string ICH_ACCT = "8500";

        /// standard account
        public const string INTERNAL_XFER_ACCT = "9800";

        /// Cost Centres
        public const string FOREIGN_CC_TYPE = "FOREIGN";

        /// admin fee accounts.
        public const string FUND_TRANSFER_INCOME_ACC = "3300";

        /// admin fee accounts.
        public const string FUND_TRANSFER_EXPENSE_ACCT = "4800";

        /// standard account
        public const string CASH_ACCT = "CASH";

        /// Allocation Journal values
        public const int MAX_AC_CC_SPLIT_INTO = 10;

        /// Allocation Journal values
        public const int MAX_ALLOCATION_DESTINATIONS = 10;

        /// Number of fees which can be assigned to each motivation det. code
        public const int MAX_FEE_CODES = 5;

        /// Finance User Levels
        public const string HIGH_FIN_USER_LEVEL = "FINANCE-3";

        /// Budget Types
        public const string BUDGET_SPLIT = "Split";

        /// Budget Types
        public const string BUDGET_ADHOC = "Adhoc";

        /// Budget Types
        public const string BUDGET_SAME = "Same";

        /// Budget Types
        public const string BUDGET_INFLATE_BASE = "Inf.Base";

        /// Budget Types
        public const string BUDGET_INFLATE_N = "Inf. n";
    }

    /// <summary>
    /// Some E-Nums for the TCommonAccountingTool i.E. for the transaction property
    /// Sub-System.
    /// (The enum.toString() is used for the database entry so you must not change the
    /// values if you do not want to change the entries.)
    /// </summary>
    public enum CommonAccountingSubSystemsEnum
    {
        /// <summary>
        /// Sub Systems, General Ledger
        /// </summary>
        GL,

        /// <summary>
        /// Sub Systems, Accounts Payable
        /// </summary>
        AP,

        /// <summary>
        /// Sub Systems, Accounts Receivable
        /// </summary>
        AR,

        /// <summary>
        /// Sub Systems, Gifts receivable
        /// </summary>
        GR
    }

    /// <summary>
    /// Some E-Nums for the TCommonAccountingTool i.E. for the transaction property
    /// Transaction Type.
    /// (The enum.toString() is used for the database entry so you must not change the
    /// values if you do not want to change the entries.)
    /// </summary>
    public enum CommonAccountingTransactionTypesEnum
    {
        /// <summary>
        /// GL-Batch-Standard
        /// </summary>
        STD,

        /// <summary>
        /// ...
        /// </summary>
            ALLOC,

        /// <summary>
        /// ...
        /// </summary>
            GR,

        /// <summary>
        /// ...
        /// </summary>
            INV,

        /// <summary>
        /// Reallloc
        /// </summary>
            REALLOC,

        /// <summary>
        /// Used in a revaluation only ...
        /// </summary>
            REVAL
    }

    /// <summary>
    /// enum for several runtime environments
    /// </summary>
    public enum TLedgerInitialisationArrayEnum
    {
        /// <summary>
        /// Tax = 1
        /// </summary>
        liaTax,

        /// <summary>
        /// Currency = 2
        /// </summary>
            liaCurrency,

        /// <summary>
        /// AcctPeriods = 3
        /// </summary>
            liaAcctPeriods,

        /// <summary>
        /// DataRetain = 4
        /// </summary>
            liaDataRetain,

        /// <summary>
        /// PL = 5
        /// </summary>
            liaPL,

        /// <summary>
        /// ILT = 6
        /// </summary>
            liaILT,

        /// <summary>
        /// Forex = 7
        /// </summary>
            liaForex,

        /// <summary>
        /// SysInt = 8
        /// </summary>
            liaSysInt,

        /// <summary>
        /// SuspAcct = 9
        /// </summary>
            liaSuspAcct,

        /// <summary>
        /// Cal = 10
        /// </summary>
            liaCal,

        /// <summary>
        /// Budget = 11
        /// </summary>
            liaBudget,

        /// <summary>
        /// FwdPosting = 12
        /// </summary>
            liaFwdPosting,

        /// <summary>
        /// CurrentPeriod = 13
        /// </summary>
            liaCurrentPeriod,

        /// <summary>
        /// RevaluationRun = 14
        /// </summary>
            liaRevaluationRun
    }


    /// <summary>
    /// enum for several runtime environments
    /// </summary>
    public enum TMOPTypeEnum
    {
        /// <summary>
        /// UseExtraFields = 1
        /// </summary>
        moptUseExtraFields,

        /// <summary>
        /// RecurringOnly = 2
        /// </summary>
            moptRecurringOnly,

        /// <summary>
        /// EntireBatchOnly = 3
        /// </summary>
            moptEntireBatchOnly,

        /// <summary>
        /// NeedBankingDetailKey = 4
        /// </summary>
            moptNeedBankingDetailKey,

        /// <summary>
        /// BankingTypesAllowed = 5
        /// </summary>
            moptBankingTypesAllowed
    }
}