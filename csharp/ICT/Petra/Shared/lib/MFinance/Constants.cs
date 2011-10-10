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
        public const String ACCOUNT_PROPERTY_BANK_ACCOUNT = "Bank Account";

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
        public const int ADMIN_FEE_INCOME_ACCT = 3400;

        /// <summary>Used in Admin Grants Payable and Receivable</summary>
        public const int ADMIN_FEE_EXPENSE_ACCT = 4900;

        /// <summary>Sets the transaction to a debit transaction</summary>
        public const bool IS_DEBIT = true;

        /// <summary>Sets the transaction to a credit transaction</summary>
        public const bool IS_CREDIT = false;

        //TODO: Cleanup

        /* 0001 Logical values for debit & credit transactions. */
        public const bool DEBIT = true;
        public const bool CREDIT = false;
        public const bool POSTED = true;

        /* 0001 Logical values for payment & receipt cash books. */
        public const bool RECEIPT = true;
        public const bool PAYMENT = false;

        /* 0001 Logical values for transactions - system generated or not. */
        public const bool SYSTEM_GENERATED = true;
        public const bool NOT_SYSTEM_GENERATED = false;

        /* Logical account & cost centre flags */
        public const bool ACTIVE = true;
        public const bool POSTING = true;

        /* Standard accounts. */

        /* 0004 public const EARNINGS-BF-ACCT RET B/F */
        public const int EARNINGS_BF_ACCT = 9700;

        /* public const DIRECT-XFER-ACCT 5600 */
        public const int DIRECT_XFER_ACCT = 5501;
        public const int ICH_SETTLEMENT_ACCT = 5601;
        public const int ICH_ACCT = 8500;
        public const int INTERNAL_XFER_ACCT = 9800;

        /* 0005 Admin fee accounts. */
        public const int FUND_TRANSFER_INCOME_ACC = 3300;
        public const int FUND_TRANSFER_EXPENSE_ACCT = 4800;

        //* Cost Centres */
        //public const string FOREIGN-CC-TYPE Foreign

        //* Allocation Journal values */
        public const int MAX_AC_CC_SPLIT_INTO = 10;
        public const int MAX_ALLOCATION_DESTINATIONS = 10;
        public const string ALLOC_AMOUNT_FORMAT = "#,##0.00";
        public const string PERCENTAGE = "no";
        public const string AMOUNT = "yes";

        //* Subsystem & Transaction Types */
        public const string GENERAL_LEDGER = "GL";
        public const string STANDARD_JOURNAL = "STD";
        public const string CASHBOOK_RECEIPTING = "CBR";
        public const string CASHBOOK_PAYMENT = "CBP";
        public const string GIFT_RECEIPTING = "GR";
        //* 0011 */
        public const string AP_SUBSYS = "AP";
        public const string AP_TRAN_TYPE = "INV";


        //* 0002 System account/summary names */
        public const string GIFT_HEADING = "GIFT";
        public const string INTER_LEDGER_HEADING = "ILT";
        //* &GLOBAL-DEFINE BANK-HEADING BANK */
        public const string BANK_HEADING = "CASH";
        public const string BALANCE_SHEET_HEADING = "BAL SHT";
        public const string PROFIT_AND_LOSS_HEADING = "PL";
        public const string INCOME_HEADING = "INC";
        public const string EXPENSE_HEADING = "EXP";
        public const string DEBTOR_HEADING = "DRS";
        public const string CREDITOR_HEADING = "CRS";
        public const string TOTAL_ASSET_HEADING = "ASSETS";
        public const string TOTAL_LIABILITY_HEADING = "LIABS";
        public const string EQUITY_HEADING = "RET EARN";

        //* 0002 Cash Book/Gift Receipt Status */
        public const string REVERSED = "Reversed";
        public const bool TRANSACTION_POSTED = true;

        //* Motivation/Fee information */
        //* OMSS Designation code to Motiv Detail (in Motiv Group: GIFT) */
        public const string OMSS_DESIGNATION_1 = "SUPPORT";
        public const string OMSS_DESIGNATION_2 = "PERSONAL";
        public const string OMSS_DESIGNATION_3 = "FIELD";
        public const string OMSS_DESIGNATION_4 = "FIELD";
        //* Standard motivation group code for giving. */
        public const string GIFT_MOTIVATION = "GIFT";
        //* Number of fees which can be assigned to each motivation det. code */
        public const int MAX_FEE_CODES = 5;

        //* Finance User Levels */
        public const string HIGH_FIN_USER_LEVEL = "FINANCE-3";

        //* Income & expense type 0007 */
        public const string INCOME_TYPE = "INCOME";
        public const string EXPENSE_TYPE = "EXPENSE";

        //* Form Design Preprocessors 0009 */
        public const string FORM_RECEIPT = "Receipt";
        public const string STD_RECEIPT = "Standard";
        public const string YEARLY_RECEIPT = "Annual";
        //* 0011 */
        public const string FORM_REMIT = "Remittance";
        public const string STD_REMIT = "Standard";
        //* M012 */
        public const string FORM_CHEQUE = "Cheque";
        public const string STD_CHEQUE = "Standard";

        //* Euro Preprocessors 0010 */
        public const string EURO_CURRENCY = "EUR";
        public const int EURO_INTERMEDIATE_DP = 4;

        //* accounts Payable 0011 */
        public const string AP_UNPOSTED = "OPEN";
        public const string AP_APPROVED = "APPROVED";
        public const string AP_CANCELLED = "CANCELLED";
        public const string AP_POSTED = "POSTED";
        public const string AP_PAID = "PAID";
        public const string AP_PARTIALLY_PAID = "PARTIALLY PAID";

        //*defines for some system account properties*/
        public const string ACC_PROP_BANK_ACCOUNT = "Bank Account";
        public const string ACC_PROP_METHOD_OF_PAYMENT = "MOP";

        //*defines for using MOP Type for special processing for direct debit and credit card gifts. */
        public const char MOP_TYPE_INDICATOR_CHAR = ':';

        //* Constants relating to carrying forward of Equity */
        //* Account property code to indicate that the specified CC is to be carried forward to this account */
        public const string CARRY_FORWARD_CC = "CARRYFORWARDCC";
        //* Values embedded in a_account_property.a_property_value_c to indicate whether the specified CC should
        //   carry forward into the same CC or the standard CC */
        public const string CARRY_FORWARD_IN_SAME_CC = "SAMECC";
        public const string CARRY_FORWARD_IN_STANDARD_CC = "STANDARDCC";
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
    };


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