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

        /// <summary>GL Batch</summary>
        public const String BATCH_HAS_TRANSACTIONS = "HasTransactions";

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

        /// <summary>Sets the transaction to a debit transaction</summary>
        public const bool IS_DEBIT = true;

        /// <summary>Sets the transaction to a credit transaction</summary>
        public const bool IS_CREDIT = false;
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
        /// ALLOC
        /// </summary>

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
}