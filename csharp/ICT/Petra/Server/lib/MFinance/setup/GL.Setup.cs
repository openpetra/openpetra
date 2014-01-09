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
using System.Globalization;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.AR.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MCommon.Data.Cascading;
using System.Collections.Generic;

namespace Ict.Petra.Server.MFinance.Setup.WebConnectors
{
    /// <summary>
    /// setup the account hierarchy, cost centre hierarchy, and other data relevant for a General Ledger
    /// </summary>
    public partial class TGLSetupWebConnector
    {
        /// <summary>
        /// returns general ledger information
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadLedgerInfo(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, null);
            AAccountingSystemParameterAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
            AAccountingPeriodAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// returns general ledger settings
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACalendarStartDate"></param>
        /// <param name="ACurrencyChangeAllowed"></param>
        /// <param name="ACalendarChangeAllowed"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadLedgerSettings(Int32 ALedgerNumber, out DateTime ACalendarStartDate,
            out bool ACurrencyChangeAllowed, out bool ACalendarChangeAllowed)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, null);
            AAccountingSystemParameterAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            // retrieve calendar start date (start date of financial year)
            AAccountingPeriodTable CalendarTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, 1, Transaction);
            ACalendarStartDate = DateTime.MinValue;

            if (CalendarTable.Count > 0)
            {
                ACalendarStartDate = ((AAccountingPeriodRow)CalendarTable.Rows[0]).PeriodStartDate;
            }

            // now check if currency change would be allowed
            ACurrencyChangeAllowed = true;

            if ((AJournalAccess.CountViaALedger(ALedgerNumber, Transaction) > 0)
                || (AGiftBatchAccess.CountViaALedger(ALedgerNumber, Transaction) > 0))
            {
                // don't allow currency change if journals or gift batches exist
                ACurrencyChangeAllowed = false;
            }

            if (AGiftBatchAccess.CountViaALedger(ALedgerNumber, Transaction) > 0)
            {
                // don't allow currency change if journals exist
                ACurrencyChangeAllowed = false;
            }

            if (ACurrencyChangeAllowed)
            {
                // don't allow currency change if there are foreign currency accounts for this ledger
                AAccountTable TemplateTable;
                AAccountRow TemplateRow;
                StringCollection TemplateOperators;

                TemplateTable = new AAccountTable();
                TemplateRow = TemplateTable.NewRowTyped(false);
                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.ForeignCurrencyFlag = true;
                TemplateOperators = new StringCollection();
                TemplateOperators.Add("=");

                if (AAccountAccess.CountUsingTemplate(TemplateRow, TemplateOperators, Transaction) > 0)
                {
                    ACurrencyChangeAllowed = false;
                }
            }

            // now check if calendar change would be allowed
            ACalendarChangeAllowed = IsCalendarChangeAllowed(ALedgerNumber);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// returns true if calendar change is allowed for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool IsCalendarChangeAllowed(Int32 ALedgerNumber)
        {
            Boolean NewTransaction;
            Boolean CalendarChangeAllowed = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            if ((ABatchAccess.CountViaALedger(ALedgerNumber, Transaction) > 0)
                || (AGiftBatchAccess.CountViaALedger(ALedgerNumber, Transaction) > 0))
            {
                // don't allow calendar change if any batch for this ledger exists
                CalendarChangeAllowed = false;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return CalendarChangeAllowed;
        }

        /// <summary>
        /// returns number of accounting periods for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static int NumberOfAccountingPeriods(Int32 ALedgerNumber)
        {
            Boolean NewTransaction;
            int NumberOfAccountingPeriods = 0;
            ALedgerTable LedgerTable;
            ALedgerRow LedgerRow;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);


            LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            if (LedgerTable.Count > 0)
            {
                LedgerRow = (ALedgerRow)LedgerTable.Rows[0];
                NumberOfAccountingPeriods = LedgerRow.NumberOfAccountingPeriods;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return NumberOfAccountingPeriods;
        }

        /// <summary>
        /// returns true if subsystem is activated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ASubsystemCode"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        private static bool IsSubsystemActivated(Int32 ALedgerNumber, String ASubsystemCode)
        {
            Boolean NewTransaction;
            Boolean Activated = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ASystemInterfaceTable TemplateTable;
            ASystemInterfaceRow TemplateRow;
            StringCollection TemplateOperators;

            TemplateTable = new ASystemInterfaceTable();
            TemplateRow = TemplateTable.NewRowTyped(false);
            TemplateRow.LedgerNumber = ALedgerNumber;
            TemplateRow.SubSystemCode = ASubsystemCode;
            TemplateRow.SetUpComplete = true;
            TemplateOperators = new StringCollection();
            TemplateOperators.Add("=");

            if (ASystemInterfaceAccess.CountUsingTemplate(TemplateRow, TemplateOperators, Transaction) > 0)
            {
                Activated = true;
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Activated;
        }

        /// <summary>
        /// returns true if gift receipting subsystem is activated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool IsGiftReceiptingSubsystemActivated(Int32 ALedgerNumber)
        {
            return IsSubsystemActivated(ALedgerNumber, CommonAccountingSubSystemsEnum.GR.ToString());
        }

        /// <summary>
        /// returns true if accounts payable subsystem is activated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool IsAccountsPayableSubsystemActivated(Int32 ALedgerNumber)
        {
            return IsSubsystemActivated(ALedgerNumber, CommonAccountingSubSystemsEnum.AP.ToString());
        }

        /// <summary>
        /// activate subsystem for gift receipting for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartingReceiptNumber"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult ActivateGiftReceiptingSubsystem(Int32 ALedgerNumber,
            Int32 AStartingReceiptNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrOK;
            Boolean NewTransaction;

            AVerificationResult = new TVerificationResultCollection();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            // if subsystem already active then no need to go further
            if (!IsGiftReceiptingSubsystemActivated(ALedgerNumber))
            {
                // create or update account for Creditor's Control

                // make sure transaction type exists for gift receipting subsystem
                ATransactionTypeTable TemplateTransactionTypeTable;
                ATransactionTypeRow TemplateTransactionTypeRow;
                StringCollection TemplateTransactionTypeOperators;

                TemplateTransactionTypeTable = new ATransactionTypeTable();
                TemplateTransactionTypeRow = TemplateTransactionTypeTable.NewRowTyped(false);
                TemplateTransactionTypeRow.LedgerNumber = ALedgerNumber;
                TemplateTransactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
                TemplateTransactionTypeOperators = new StringCollection();
                TemplateTransactionTypeOperators.Add("=");

                if (ATransactionTypeAccess.CountUsingTemplate(TemplateTransactionTypeRow, TemplateTransactionTypeOperators, Transaction) == 0)
                {
                    ATransactionTypeTable TransactionTypeTable;
                    ATransactionTypeRow TransactionTypeRow;

                    TransactionTypeTable = new ATransactionTypeTable();
                    TransactionTypeRow = TransactionTypeTable.NewRowTyped();
                    TransactionTypeRow.LedgerNumber = ALedgerNumber;
                    TransactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
                    TransactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.GR.ToString();
                    TransactionTypeRow.DebitAccountCode = "CASH";
                    TransactionTypeRow.CreditAccountCode = "GIFT";
                    TransactionTypeRow.TransactionTypeDescription = "Gift Receipting";
                    TransactionTypeRow.SpecialTransactionType = true;
                    TransactionTypeTable.Rows.Add(TransactionTypeRow);

                    if (!ATransactionTypeAccess.SubmitChanges(TransactionTypeTable, Transaction, out AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }

                if (ReturnValue == TSubmitChangesResult.scrOK)
                {
                    ASystemInterfaceTable SystemInterfaceTable;
                    ASystemInterfaceRow SystemInterfaceRow;
                    SystemInterfaceTable = ASystemInterfaceAccess.LoadByPrimaryKey(ALedgerNumber,
                        CommonAccountingSubSystemsEnum.GR.ToString(),
                        Transaction);

                    if (SystemInterfaceTable.Count == 0)
                    {
                        SystemInterfaceRow = SystemInterfaceTable.NewRowTyped();
                        SystemInterfaceRow.LedgerNumber = ALedgerNumber;
                        SystemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
                        SystemInterfaceRow.SetUpComplete = true;
                        SystemInterfaceTable.Rows.Add(SystemInterfaceRow);
                    }
                    else
                    {
                        SystemInterfaceRow = (ASystemInterfaceRow)SystemInterfaceTable.Rows[0];
                        SystemInterfaceRow.SetUpComplete = true;
                    }

                    if (!ASystemInterfaceAccess.SubmitChanges(SystemInterfaceTable, Transaction, out AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }

                // now set the starting receipt number
                if (ReturnValue == TSubmitChangesResult.scrOK)
                {
                    ALedgerTable LedgerTable;
                    ALedgerRow LedgerRow;

                    LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                    LedgerRow = (ALedgerRow)LedgerTable.Rows[0];
                    LedgerRow.LastHeaderRNumber = AStartingReceiptNumber;

                    if (!ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }
            }

            if ((ReturnValue == TSubmitChangesResult.scrOK)
                && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ReturnValue;
        }

        /// <summary>
        /// activate subsystem for accounts payable for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult ActivateAccountsPayableSubsystem(Int32 ALedgerNumber,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrOK;
            Boolean NewTransaction;

            AVerificationResult = new TVerificationResultCollection();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            // if subsystem already active then no need to go further
            if (!IsAccountsPayableSubsystemActivated(ALedgerNumber))
            {
                // make sure transaction type exists for accounts payable subsystem
                ATransactionTypeTable TemplateTransactionTypeTable;
                ATransactionTypeRow TemplateTransactionTypeRow;
                StringCollection TemplateTransactionTypeOperators;

                TemplateTransactionTypeTable = new ATransactionTypeTable();
                TemplateTransactionTypeRow = TemplateTransactionTypeTable.NewRowTyped(false);
                TemplateTransactionTypeRow.LedgerNumber = ALedgerNumber;
                TemplateTransactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                TemplateTransactionTypeOperators = new StringCollection();
                TemplateTransactionTypeOperators.Add("=");

                if (ATransactionTypeAccess.CountUsingTemplate(TemplateTransactionTypeRow, TemplateTransactionTypeOperators, Transaction) == 0)
                {
                    ATransactionTypeTable TransactionTypeTable;
                    ATransactionTypeRow TransactionTypeRow;

                    TransactionTypeTable = new ATransactionTypeTable();
                    TransactionTypeRow = TransactionTypeTable.NewRowTyped();
                    TransactionTypeRow.LedgerNumber = ALedgerNumber;
                    TransactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                    TransactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
                    TransactionTypeRow.DebitAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
                    TransactionTypeRow.CreditAccountCode = MFinanceConstants.ACCOUNT_CREDITORS;
                    TransactionTypeRow.TransactionTypeDescription = "Input Creditor's Invoice";
                    TransactionTypeRow.SpecialTransactionType = true;
                    TransactionTypeTable.Rows.Add(TransactionTypeRow);

                    if (!ATransactionTypeAccess.SubmitChanges(TransactionTypeTable, Transaction, out AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }

                // create or update system interface record for accounts payable
                ASystemInterfaceTable SystemInterfaceTable;
                ASystemInterfaceRow SystemInterfaceRow;
                SystemInterfaceTable = ASystemInterfaceAccess.LoadByPrimaryKey(ALedgerNumber,
                    CommonAccountingSubSystemsEnum.AP.ToString(),
                    Transaction);

                if (SystemInterfaceTable.Count == 0)
                {
                    SystemInterfaceRow = SystemInterfaceTable.NewRowTyped();
                    SystemInterfaceRow.LedgerNumber = ALedgerNumber;
                    SystemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                    SystemInterfaceRow.SetUpComplete = true;
                    SystemInterfaceTable.Rows.Add(SystemInterfaceRow);
                }
                else
                {
                    SystemInterfaceRow = (ASystemInterfaceRow)SystemInterfaceTable.Rows[0];
                    SystemInterfaceRow.SetUpComplete = true;
                }

                if (!ASystemInterfaceAccess.SubmitChanges(SystemInterfaceTable, Transaction, out AVerificationResult))
                {
                    ReturnValue = TSubmitChangesResult.scrError;
                }
            }

            if ((ReturnValue == TSubmitChangesResult.scrOK)
                && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns true if subsystem can be deactivated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ASubsystemCode"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        private static bool CanSubsystemBeDeactivated(Int32 ALedgerNumber, String ASubsystemCode)
        {
            Boolean NewTransaction;
            Boolean Result = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            if (ASubsystemCode == CommonAccountingSubSystemsEnum.GR.ToString())
            {
                // for gift receipting don't allow to deactivate if 'Posted' or 'Unposted' gift batches exist
                AGiftBatchTable TemplateGiftBatchTable;
                AGiftBatchRow TemplateGiftBatchRow;
                StringCollection TemplateGiftBatchOperators;

                TemplateGiftBatchTable = new AGiftBatchTable();
                TemplateGiftBatchRow = TemplateGiftBatchTable.NewRowTyped(false);
                TemplateGiftBatchRow.LedgerNumber = ALedgerNumber;
                TemplateGiftBatchRow.BatchStatus = MFinanceConstants.BATCH_POSTED;
                TemplateGiftBatchOperators = new StringCollection();
                TemplateGiftBatchOperators.Add("=");

                if (AGiftBatchAccess.CountUsingTemplate(TemplateGiftBatchRow, TemplateGiftBatchOperators, Transaction) == 0)
                {
                    Result = true;
                }

                if (!Result)
                {
                    TemplateGiftBatchRow.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

                    if (AGiftBatchAccess.CountUsingTemplate(TemplateGiftBatchRow, TemplateGiftBatchOperators, Transaction) == 0)
                    {
                        Result = true;
                    }
                }
            }

            if (!Result)
            {
                AJournalTable TemplateJournalTable;
                AJournalRow TemplateJournalRow;
                StringCollection TemplateJournalOperators;

                TemplateJournalTable = new AJournalTable();
                TemplateJournalRow = TemplateJournalTable.NewRowTyped(false);
                TemplateJournalRow.LedgerNumber = ALedgerNumber;
                TemplateJournalRow.SubSystemCode = ASubsystemCode;
                TemplateJournalOperators = new StringCollection();
                TemplateJournalOperators.Add("=");

                ARecurringJournalTable TemplateRJournalTable;
                ARecurringJournalRow TemplateRJournalRow;
                StringCollection TemplateRJournalOperators;

                TemplateRJournalTable = new ARecurringJournalTable();
                TemplateRJournalRow = TemplateRJournalTable.NewRowTyped(false);
                TemplateRJournalRow.LedgerNumber = ALedgerNumber;
                TemplateRJournalRow.SubSystemCode = ASubsystemCode;
                TemplateRJournalOperators = new StringCollection();
                TemplateRJournalOperators.Add("=");

                // do not allow to deactivate subsystem if journals already exist
                if ((AJournalAccess.CountUsingTemplate(TemplateJournalRow, TemplateJournalOperators, Transaction) == 0)
                    && (ARecurringJournalAccess.CountUsingTemplate(TemplateRJournalRow, TemplateRJournalOperators, Transaction) == 0))
                {
                    Result = true;
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Result;
        }

        /// <summary>
        /// returns true if gift receipting subsystem can be deactivated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CanGiftReceiptingSubsystemBeDeactivated(Int32 ALedgerNumber)
        {
            return CanSubsystemBeDeactivated(ALedgerNumber, CommonAccountingSubSystemsEnum.GR.ToString());
        }

        /// <summary>
        /// returns true if accounts payable subsystem can be deactivated for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CanAccountsPayableSubsystemBeDeactivated(Int32 ALedgerNumber)
        {
            return CanSubsystemBeDeactivated(ALedgerNumber, CommonAccountingSubSystemsEnum.AP.ToString());
        }

        /// <summary>
        /// deactivate given subsystem for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="SubsystemCode"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        private static bool DeactivateSubsystem(Int32 ALedgerNumber, String SubsystemCode)
        {
            Boolean NewTransaction;
            bool Deactivated = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ASystemInterfaceAccess.DeleteByPrimaryKey(ALedgerNumber, SubsystemCode, Transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                Deactivated = true;
            }
            finally
            {
                if (!Deactivated)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return Deactivated;
        }

        /// <summary>
        /// deactivate subsystem for gift receipting for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool DeactivateGiftReceiptingSubsystem(Int32 ALedgerNumber)
        {
            return DeactivateSubsystem(ALedgerNumber, CommonAccountingSubSystemsEnum.GR.ToString());
        }

        /// <summary>
        /// deactivate subsystem for accounts payable for given ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool DeactivateAccountsPayableSubsystem(Int32 ALedgerNumber)
        {
            return DeactivateSubsystem(ALedgerNumber, CommonAccountingSubSystemsEnum.AP.ToString());
        }

        /// <summary>
        /// returns all account hierarchies available for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadAccountHierarchies(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, null);
            AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountHierarchyDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountPropertyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAnalysisTypeAccess.LoadAll(MainDS, null);
            AAnalysisAttributeAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AFreeformAnalysisAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // set Account BankAccountFlag if there exists a property
            foreach (AAccountPropertyRow accProp in MainDS.AAccountProperty.Rows)
            {
                if ((accProp.PropertyCode == MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT) && (accProp.PropertyValue == "true"))
                {
                    MainDS.AAccount.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        AAccountTable.GetAccountCodeDBName(),
                        accProp.AccountCode);
                    GLSetupTDSAAccountRow acc = (GLSetupTDSAAccountRow)MainDS.AAccount.DefaultView[0].Row;
                    acc.BankAccountFlag = true;
                    MainDS.AAccount.DefaultView.RowFilter = "";
                }
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// returns cost centre hierarchy and cost centre details for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadCostCentreHierarchy(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AValidLedgerNumberAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary></summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable LoadLocalCostCentres(Int32 ALedgerNumber)
        {
            String SqlQuery = "SELECT a_cost_centre_code_c AS CostCentreCode, " +
                              "a_cost_centre_name_c AS CostCentreName, " +
                              "a_posting_cost_centre_flag_l AS Posting, " +
                              "a_cost_centre_to_report_to_c AS ReportsTo" +
                              " FROM PUB_a_cost_centre" +
                              " WHERE a_ledger_number_i = " + ALedgerNumber +
                              " AND a_cost_centre_type_c = 'Local';";
            DataTable ParentCostCentreTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "ParentCostCentre", null);

            return ParentCostCentreTbl;
        }

        /// <summary></summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable LoadCostCentrePartnerLinks(Int32 ALedgerNumber)
        {
            //
            // Load Partners where PartnerType includes "COSTCENTRE":
            String SqlQuery = "SELECT p_partner_short_name_c as ShortName, " +
                              "PUB_p_partner.p_partner_key_n as PartnerKey, " +
                              "'0' as IsLinked, " +
                              "'0' as ReportsTo" +
                              " FROM PUB_p_partner, PUB_p_partner_type" +
                              " WHERE PUB_p_partner_type.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                              " AND PUB_p_partner_type.p_type_code_c = 'COSTCENTRE';";

            DataTable PartnerCostCentreTbl = DBAccess.GDBAccessObj.SelectDT(SqlQuery, "PartnerCostCentre", null);

            PartnerCostCentreTbl.DefaultView.Sort = ("PartnerKey");
            AValidLedgerNumberTable LinksTbl = AValidLedgerNumberAccess.LoadViaALedger(ALedgerNumber, null);

            foreach (AValidLedgerNumberRow Row in LinksTbl.Rows)
            {
                Int32 RowIdx = PartnerCostCentreTbl.DefaultView.Find(Row.PartnerKey);

                if (RowIdx >= 0)
                {
                    PartnerCostCentreTbl.DefaultView[RowIdx].Row["IsLinked"] = Row.CostCentreCode;
                    ACostCentreTable CCTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, Row.CostCentreCode, null);

                    if (CCTbl.Rows.Count > 0)
                    {
                        PartnerCostCentreTbl.DefaultView[RowIdx].Row["ReportsTo"] = CCTbl[0].CostCentreToReportTo;
                    }
                }
            }

            return PartnerCostCentreTbl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="PartnerCostCentreTbl"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveCostCentrePartnerLinks(
            Int32 ALedgerNumber, DataTable PartnerCostCentreTbl, out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;

            try
            {
                AValidLedgerNumberTable LinksTbl = AValidLedgerNumberAccess.LoadViaALedger(ALedgerNumber, Transaction);
                LinksTbl.DefaultView.Sort = "p_partner_key_n";

                ACostCentreTable CostCentreTbl = ACostCentreAccess.LoadViaALedger(ALedgerNumber, Transaction);
                CostCentreTbl.DefaultView.Sort = "a_cost_centre_code_c";

                foreach (DataRow Row in PartnerCostCentreTbl.Rows)
                {
                    String RowCCCode = Convert.ToString(Row["IsLinked"]);

                    if (RowCCCode != "0")   // This should be in the LinksTbl - if it's not, I'll add it.
                    {                       // { AND I probably need to create a CostCentre Row too! }
                        Int32 CostCentreRowIdx = CostCentreTbl.DefaultView.Find(RowCCCode);

                        if (CostCentreRowIdx < 0)       // There's no such Cost Centre - I need to create it now.
                        {
                            ACostCentreRow NewCostCentreRow = CostCentreTbl.NewRowTyped();
                            NewCostCentreRow.LedgerNumber = ALedgerNumber;
                            NewCostCentreRow.CostCentreCode = RowCCCode;
                            NewCostCentreRow.CostCentreToReportTo = Convert.ToString(Row["ReportsTo"]);
                            NewCostCentreRow.CostCentreName = Convert.ToString(Row["ShortName"]);
                            NewCostCentreRow.PostingCostCentreFlag = true;
                            NewCostCentreRow.CostCentreActiveFlag = true;
                            CostCentreTbl.Rows.Add(NewCostCentreRow);
                        }
                        else    // The cost Centre was found, but the match above was case-insensitive.
                        {       // So I'm going to use the actual name from the table, otherwise it might break the DB Constraint.
                            RowCCCode = CostCentreTbl.DefaultView[CostCentreRowIdx].Row["a_cost_centre_code_c"].ToString();
                        }

                        Int32 RowIdx = LinksTbl.DefaultView.Find(Row["PartnerKey"]);

                        if (RowIdx < 0)
                        {
                            AValidLedgerNumberRow LinksRow = LinksTbl.NewRowTyped();
                            LinksRow.LedgerNumber = ALedgerNumber;
                            LinksRow.PartnerKey = Convert.ToInt64(Row["PartnerKey"]);
                            LinksRow.IltProcessingCentre = 4000000; // This is the ICH ledger number, but apparently anyone cares about it!
                            LinksRow.CostCentreCode = RowCCCode;
                            LinksTbl.Rows.Add(LinksRow);
                        }
                        else    // If this partner is already linked to a cost centre, it's possible the user has changed the code!
                        {
                            AValidLedgerNumberRow LinksRow = (AValidLedgerNumberRow)LinksTbl.DefaultView[RowIdx].Row;
                            LinksRow.CostCentreCode = RowCCCode;
                        }
                    }
                    else                // This should not be in the LinksTbl - if it is, I'll delete it.
                    {
                        Int32 RowIdx = LinksTbl.DefaultView.Find(Row["PartnerKey"]);

                        if (RowIdx >= 0)
                        {
                            AValidLedgerNumberRow LinksRow = (AValidLedgerNumberRow)LinksTbl.DefaultView[RowIdx].Row;
                            LinksRow.Delete();
                        }
                    }
                }

                if (ACostCentreAccess.SubmitChanges(CostCentreTbl, Transaction, out AVerificationResult))
                {
                    if (AValidLedgerNumberAccess.SubmitChanges(LinksTbl, Transaction, out AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrOK;
                    }
                }
            }
            finally
            {
                if (ReturnValue == TSubmitChangesResult.scrOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();

                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableFinanceTablesEnum.CostCentresLinkedToPartnerList.ToString());
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return ReturnValue;
        }

        private static void DropAccountProperties(
            ref GLSetupTDS AInspectDS,
            Int32 ALedgerNumber,
            String AAccountCode)
        {
            if (AInspectDS.AAccountProperty != null)
            {
                AInspectDS.AAccountProperty.DefaultView.RowFilter = String.Format("{0}={1} and {2}='{3}'",
                    AAccountPropertyTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    AAccountPropertyTable.GetAccountCodeDBName(),
                    AAccountCode);

                foreach (DataRowView rv in AInspectDS.AAccountProperty.DefaultView)
                {
                    AAccountPropertyRow accountPropertyRow = (AAccountPropertyRow)rv.Row;
                    accountPropertyRow.Delete();
                }

                AInspectDS.AAccountProperty.DefaultView.RowFilter = "";
            }
        }

        private static void AddOrRemoveLedgerInitFlag(Int32 ALedgerNumber, String AInitFlagName,
            Boolean AAdd, TDBTransaction ATransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            if (AAdd)
            {
                // add flag if not there yet
                if (!ALedgerInitFlagAccess.Exists(ALedgerNumber, AInitFlagName, ATransaction))
                {
                    ALedgerInitFlagTable InitFlagTable = new ALedgerInitFlagTable();
                    ALedgerInitFlagRow InitFlagRow = InitFlagTable.NewRowTyped();
                    InitFlagRow.LedgerNumber = ALedgerNumber;
                    InitFlagRow.InitOptionName = AInitFlagName;
                    InitFlagTable.Rows.Add(InitFlagRow);
                    ALedgerInitFlagAccess.SubmitChanges(InitFlagTable, ATransaction, out AVerificationResult);
                }
            }
            else
            {
                // remove flag from table if it exists
                if (ALedgerInitFlagAccess.Exists(ALedgerNumber, AInitFlagName, ATransaction))
                {
                    ALedgerInitFlagAccess.DeleteByPrimaryKey(ALedgerNumber, AInitFlagName, ATransaction);
                }
            }
        }

        /// <summary>
        /// save general ledger settings
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACalendarStartDate"></param>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveLedgerSettings(
            Int32 ALedgerNumber,
            DateTime ACalendarStartDate,
            ref GLSetupTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrError;
            Boolean NewTransaction;
            ALedgerTable LedgerTable;
            ALedgerRow LedgerRow;
            AAccountingPeriodTable AccountingPeriodTable;
            AAccountingPeriodRow AccountingPeriodRow;
            AAccountingPeriodTable NewAccountingPeriodTable;
            AAccountingPeriodRow NewAccountingPeriodRow;
            AGeneralLedgerMasterTable GLMTable;
            AGeneralLedgerMasterRow GLMRow;
            AGeneralLedgerMasterPeriodTable GLMPeriodTable;
            AGeneralLedgerMasterPeriodTable TempGLMPeriodTable;
            AGeneralLedgerMasterPeriodTable NewGLMPeriodTable;
            AGeneralLedgerMasterPeriodRow GLMPeriodRow;
            AGeneralLedgerMasterPeriodRow TempGLMPeriodRow;
            AGeneralLedgerMasterPeriodRow NewGLMPeriodRow;

            int CurrentNumberPeriods;
            int NewNumberPeriods;
            int CurrentNumberFwdPostingPeriods;
            int NewNumberFwdPostingPeriods;
            int CurrentLastFwdPeriod;
            int NewLastFwdPeriod;
            int Period;
            Boolean ExtendFwdPeriods = false;
            DateTime PeriodStartDate;
            DateTime CurrentCalendarStartDate;
            Boolean CreateCalendar = false;

            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                // load ledger row currently saved in database so it can be used for comparison with modified data
                LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

                // initialize variables for accounting periods and forward periods
                CurrentNumberPeriods = LedgerRow.NumberOfAccountingPeriods;
                NewNumberPeriods = ((ALedgerRow)(AInspectDS.ALedger.Rows[0])).NumberOfAccountingPeriods;

                CurrentNumberFwdPostingPeriods = LedgerRow.NumberFwdPostingPeriods;
                NewNumberFwdPostingPeriods = ((ALedgerRow)(AInspectDS.ALedger.Rows[0])).NumberFwdPostingPeriods;

                // retrieve currently saved calendar start date (start date of financial year)
                AAccountingPeriodTable CalendarTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, 1, Transaction);
                CurrentCalendarStartDate = DateTime.MinValue;

                if (CalendarTable.Count > 0)
                {
                    CurrentCalendarStartDate = ((AAccountingPeriodRow)CalendarTable.Rows[0]).PeriodStartDate;
                }

                // update accounting periods (calendar):
                // this only needs to be done if the calendar mode is changed
                // or if calendar mode is monthly and the start date has changed
                // or if not monthly and number of periods has changed
                if (((ALedgerRow)(AInspectDS.ALedger.Rows[0])).CalendarMode != LedgerRow.CalendarMode)
                {
                    CreateCalendar = true;
                }
                else if (((ALedgerRow)(AInspectDS.ALedger.Rows[0])).CalendarMode
                         && (ACalendarStartDate != CurrentCalendarStartDate))
                {
                    CreateCalendar = true;
                }
                else if (!((ALedgerRow)(AInspectDS.ALedger.Rows[0])).CalendarMode
                         && (NewNumberPeriods != CurrentNumberPeriods))
                {
                    CreateCalendar = true;
                }

                if (!CreateCalendar
                    && (NewNumberFwdPostingPeriods < CurrentNumberFwdPostingPeriods))
                {
                    CreateCalendar = true;
                }

                if (!CreateCalendar
                    && (NewNumberFwdPostingPeriods > CurrentNumberFwdPostingPeriods))
                {
                    // in this case only extend the periods (as there may already be existing transactions)
                    ExtendFwdPeriods = true;
                }

                // now perform the actual update of accounting periods (calendar)
                if (CreateCalendar)
                {
                    // first make sure all accounting period records are deleted
                    if (AAccountingPeriodAccess.CountViaALedger(ALedgerNumber, Transaction) > 0)
                    {
                        AAccountingPeriodTable TemplateTable = new AAccountingPeriodTable();
                        AAccountingPeriodRow TemplateRow = TemplateTable.NewRowTyped(false);
                        TemplateRow.LedgerNumber = ALedgerNumber;
                        AAccountingPeriodAccess.DeleteUsingTemplate(TemplateRow, null, Transaction);
                    }

                    // now create all accounting period records according to monthly calendar mode
                    // (at the same time create forwarding periods. If number of forwarding periods also
                    // changes with this saving method then this will be dealt with further down in the code)
                    NewAccountingPeriodTable = new AAccountingPeriodTable();

                    PeriodStartDate = ACalendarStartDate;

                    for (Period = 1; Period <= NewNumberPeriods; Period++)
                    {
                        NewAccountingPeriodRow = NewAccountingPeriodTable.NewRowTyped();
                        NewAccountingPeriodRow.LedgerNumber = ALedgerNumber;
                        NewAccountingPeriodRow.AccountingPeriodNumber = Period;
                        NewAccountingPeriodRow.PeriodStartDate = PeriodStartDate;

                        if ((((ALedgerRow)(AInspectDS.ALedger.Rows[0])).NumberOfAccountingPeriods == 13)
                            && (Period == 12))
                        {
                            // in case of 12 periods the second last period represents the last month except for the very last day
                            NewAccountingPeriodRow.PeriodEndDate = PeriodStartDate.AddMonths(1).AddDays(-2);
                        }
                        else if ((((ALedgerRow)(AInspectDS.ALedger.Rows[0])).NumberOfAccountingPeriods == 13)
                                 && (Period == 13))
                        {
                            // in case of 13 periods the last period just represents the very last day of the financial year
                            NewAccountingPeriodRow.PeriodEndDate = PeriodStartDate;
                        }
                        else
                        {
                            NewAccountingPeriodRow.PeriodEndDate = PeriodStartDate.AddMonths(1).AddDays(-1);
                        }

                        NewAccountingPeriodRow.AccountingPeriodDesc = PeriodStartDate.ToString("MMMM");
                        NewAccountingPeriodTable.Rows.Add(NewAccountingPeriodRow);
                        PeriodStartDate = NewAccountingPeriodRow.PeriodEndDate.AddDays(1);
                    }

                    AAccountingPeriodAccess.SubmitChanges(NewAccountingPeriodTable, Transaction, out AVerificationResult);

                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());

                    CurrentNumberPeriods = NewNumberPeriods;
                }

                // check if any new forwarding periods need to be created
                if (CreateCalendar || ExtendFwdPeriods)
                {
                    // now create new forwarding posting periods (if at all needed)
                    NewAccountingPeriodTable = new AAccountingPeriodTable();

                    // if calendar was created then there are no forward periods yet
                    if (CreateCalendar)
                    {
                        Period = CurrentNumberPeriods + 1;
                    }
                    else
                    {
                        Period = CurrentNumberPeriods + CurrentNumberFwdPostingPeriods + 1;
                    }

                    while (Period <= NewNumberPeriods + NewNumberFwdPostingPeriods)
                    {
                        AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                            Period - CurrentNumberPeriods,
                            Transaction);
                        AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                        NewAccountingPeriodRow = NewAccountingPeriodTable.NewRowTyped();
                        NewAccountingPeriodRow.LedgerNumber = ALedgerNumber;
                        NewAccountingPeriodRow.AccountingPeriodNumber = Period;
                        NewAccountingPeriodRow.AccountingPeriodDesc = AccountingPeriodRow.AccountingPeriodDesc;
                        NewAccountingPeriodRow.PeriodStartDate = AccountingPeriodRow.PeriodStartDate.AddYears(1);
                        NewAccountingPeriodRow.PeriodEndDate = AccountingPeriodRow.PeriodEndDate.AddYears(1);

                        NewAccountingPeriodTable.Rows.Add(NewAccountingPeriodRow);

                        Period++;
                    }

                    AAccountingPeriodAccess.SubmitChanges(NewAccountingPeriodTable, Transaction, out AVerificationResult);

                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());

                    // also create new general ledger master periods with balances
                    CurrentLastFwdPeriod = LedgerRow.NumberOfAccountingPeriods + CurrentNumberFwdPostingPeriods;
                    NewLastFwdPeriod = LedgerRow.NumberOfAccountingPeriods + NewNumberFwdPostingPeriods;
                    // TODO: the following 2 lines would need to replace the 2 lines above if not all possible forward periods are created initially
                    //CurrentLastFwdPeriod = LedgerRow.CurrentPeriod + CurrentNumberFwdPostingPeriods;
                    //NewLastFwdPeriod = LedgerRow.CurrentPeriod + NewNumberFwdPostingPeriods;

                    GLMTable = new AGeneralLedgerMasterTable();
                    AGeneralLedgerMasterRow template = GLMTable.NewRowTyped(false);

                    template.LedgerNumber = ALedgerNumber;
                    template.Year = LedgerRow.CurrentFinancialYear;

                    // find all general ledger master records of the current financial year for given ledger
                    GLMTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(template, Transaction);

                    NewGLMPeriodTable = new AGeneralLedgerMasterPeriodTable();

                    foreach (DataRow Row in GLMTable.Rows)
                    {
                        // for each of the general ledger master records of the current financial year set the
                        // new, extended forwarding glm period records (most likely they will not exist yet
                        // but if they do then update values)
                        GLMRow = (AGeneralLedgerMasterRow)Row;
                        GLMPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(GLMRow.GlmSequence, CurrentLastFwdPeriod, Transaction);

                        if (GLMPeriodTable.Count > 0)
                        {
                            GLMPeriodRow = (AGeneralLedgerMasterPeriodRow)GLMPeriodTable.Rows[0];

                            for (Period = CurrentLastFwdPeriod + 1; Period <= NewLastFwdPeriod; Period++)
                            {
                                if (AGeneralLedgerMasterPeriodAccess.Exists(GLMPeriodRow.GlmSequence, Period, Transaction))
                                {
                                    // if the record already exists then just change values
                                    TempGLMPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(GLMPeriodRow.GlmSequence,
                                        Period,
                                        Transaction);
                                    TempGLMPeriodRow = (AGeneralLedgerMasterPeriodRow)TempGLMPeriodTable.Rows[0];
                                    TempGLMPeriodRow.ActualBase = GLMPeriodRow.ActualBase;
                                    TempGLMPeriodRow.ActualIntl = GLMPeriodRow.ActualIntl;

                                    if (!GLMPeriodRow.IsActualForeignNull())
                                    {
                                        TempGLMPeriodRow.ActualForeign = GLMPeriodRow.ActualForeign;
                                    }
                                    else
                                    {
                                        TempGLMPeriodRow.SetActualForeignNull();
                                    }

                                    NewGLMPeriodTable.Merge(TempGLMPeriodTable, true);
                                }
                                else
                                {
                                    // add new row since it does not exist yet
                                    NewGLMPeriodRow = NewGLMPeriodTable.NewRowTyped();
                                    NewGLMPeriodRow.GlmSequence = GLMPeriodRow.GlmSequence;
                                    NewGLMPeriodRow.PeriodNumber = Period;
                                    NewGLMPeriodRow.ActualBase = GLMPeriodRow.ActualBase;
                                    NewGLMPeriodRow.ActualIntl = GLMPeriodRow.ActualIntl;

                                    if (!GLMPeriodRow.IsActualForeignNull())
                                    {
                                        NewGLMPeriodRow.ActualForeign = GLMPeriodRow.ActualForeign;
                                    }
                                    else
                                    {
                                        NewGLMPeriodRow.SetActualForeignNull();
                                    }

                                    NewGLMPeriodTable.Rows.Add(NewGLMPeriodRow);
                                }
                            }

                            // remove periods if the number of periods + forwarding periods has been reduced
                            int NumberOfExistingPeriods = LedgerRow.NumberOfAccountingPeriods + LedgerRow.NumberFwdPostingPeriods;

                            while ((NewNumberPeriods + NewNumberFwdPostingPeriods) < NumberOfExistingPeriods)
                            {
                                AGeneralLedgerMasterPeriodAccess.DeleteByPrimaryKey(GLMPeriodRow.GlmSequence, NumberOfExistingPeriods, Transaction);

                                NumberOfExistingPeriods--;
                            }
                        }
                    }

                    // just one SubmitChanges for all records needed
                    AGeneralLedgerMasterPeriodAccess.SubmitChanges(NewGLMPeriodTable, Transaction, out AVerificationResult);
                }

                // update a_ledger_init_flag records for:
                // suspense account flag: "SUSP-ACCT"
                // budget flag: "BUDGET"
                // branch processing: "BRANCH-PROCESS" (this is a new flag for OpenPetra)
                // base currency: "CURRENCY"
                // international currency: "INTL-CURRENCY" (this is a new flag for OpenPetra)
                // current period (start of ledger date): CURRENT-PERIOD
                // calendar settings: CAL
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "SUSP-ACCT", LedgerRow.SuspenseAccountFlag, Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "BUDGET", LedgerRow.BudgetControlFlag, Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "BRANCH-PROCESS", LedgerRow.BranchProcessing, Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "CURRENCY", !LedgerRow.IsBaseCurrencyNull(), Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "INTL-CURRENCY", !LedgerRow.IsIntlCurrencyNull(), Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "CURRENT-PERIOD", !LedgerRow.IsCurrentPeriodNull(), Transaction, ref AVerificationResult);
                AddOrRemoveLedgerInitFlag(ALedgerNumber, "CAL", !LedgerRow.IsNumberOfAccountingPeriodsNull(), Transaction, ref AVerificationResult);

                ReturnValue = GLSetupTDSAccess.SubmitChanges(AInspectDS);

                if (AVerificationResult.Count > 0)
                {
                    // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                    // Serialisation (needed for .NET Remoting).
                    TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
                }
            }
            finally
            {
                if ((ReturnValue == TSubmitChangesResult.scrOK)
                    && NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// save modified account hierarchy etc; does not support moving accounts;
        /// also used for saving cost centre hierarchy and cost centre details
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveGLSetupTDS(
            Int32 ALedgerNumber,
            ref GLSetupTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult ReturnValue = TSubmitChangesResult.scrOK;
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            if ((AInspectDS.ACostCentre != null) && (AInspectDS.AValidLedgerNumber != null))
            {
                // check for removed cost centres, and also delete the AValidLedgerNumber row if there is one for the removed cost centre
                foreach (ACostCentreRow cc in AInspectDS.ACostCentre.Rows)
                {
                    if (cc.RowState == DataRowState.Deleted)
                    {
                        string CostCentreCodeToDelete = cc[ACostCentreTable.ColumnCostCentreCodeId, DataRowVersion.Original].ToString();
                        AInspectDS.AValidLedgerNumber.DefaultView.RowFilter =
                            String.Format("{0}='{1}'",
                                AValidLedgerNumberTable.GetCostCentreCodeDBName(),
                                CostCentreCodeToDelete);

                        foreach (DataRowView rv in AInspectDS.AValidLedgerNumber.DefaultView)
                        {
                            AValidLedgerNumberRow ValidLedgerNumberRow = (AValidLedgerNumberRow)rv.Row;

                            ValidLedgerNumberRow.Delete();
                        }
                    }
                }

                AInspectDS.AValidLedgerNumber.DefaultView.RowFilter = "";
            }

            if (AInspectDS.AAccount != null)
            {
                // check AAccount, if BankAccountFlag is not null, then create AAccountProperty or delete it
                foreach (GLSetupTDSAAccountRow acc in AInspectDS.AAccount.Rows)
                {
                    // special treatment of deleted accounts
                    if (acc.RowState == DataRowState.Deleted)
                    {
                        // delete all account properties as well
                        string AccountCodeToDelete = acc[GLSetupTDSAAccountTable.ColumnAccountCodeId, DataRowVersion.Original].ToString();

                        DropAccountProperties(ref AInspectDS, ALedgerNumber, AccountCodeToDelete);

                        continue;
                    }

                    // if the flag has been changed by the client, it will not be null
                    if (!acc.IsBankAccountFlagNull())
                    {
                        if (AInspectDS.AAccountProperty == null)
                        {
                            // because AccountProperty has not been changed on the client, GetChangesTyped will have removed the table
                            // so we need to reload the table from the database
                            AInspectDS.Merge(new AAccountPropertyTable());
                            AAccountPropertyAccess.LoadViaALedger(AInspectDS, ALedgerNumber, null);
                        }

                        AInspectDS.AAccountProperty.DefaultView.RowFilter =
                            String.Format("{0}='{1}' and {2}='{3}'",
                                AAccountPropertyTable.GetAccountCodeDBName(),
                                acc.AccountCode,
                                AAccountPropertyTable.GetPropertyCodeDBName(),
                                MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT);

                        if ((AInspectDS.AAccountProperty.DefaultView.Count == 0) && acc.BankAccountFlag)
                        {
                            AAccountPropertyRow accProp = AInspectDS.AAccountProperty.NewRowTyped(true);
                            accProp.LedgerNumber = acc.LedgerNumber;
                            accProp.AccountCode = acc.AccountCode;
                            accProp.PropertyCode = MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT;
                            accProp.PropertyValue = "true";
                            AInspectDS.AAccountProperty.Rows.Add(accProp);
                        }
                        else if (AInspectDS.AAccountProperty.DefaultView.Count == 1)
                        {
                            AAccountPropertyRow accProp = (AAccountPropertyRow)AInspectDS.AAccountProperty.DefaultView[0].Row;

                            if (!acc.BankAccountFlag)
                            {
                                accProp.Delete();
                            }
                            else
                            {
                                accProp.PropertyValue = "true";
                            }
                        }

                        AInspectDS.AAccountProperty.DefaultView.RowFilter = "";
                    }
                }
            }

            if (AInspectDS.AAnalysisType != null)
            {
                if (AInspectDS.AAnalysisType.Rows.Count > 0)
                {
                    ValidateAAnalysisType(ValidationControlsDict, ref AVerificationResult, AInspectDS.AAnalysisType);
                    ValidateAAnalysisTypeManual(ValidationControlsDict, ref AVerificationResult, AInspectDS.AAnalysisType);

                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                    {
                        ReturnValue = TSubmitChangesResult.scrError;
                    }
                }
            }

            if (ReturnValue != TSubmitChangesResult.scrError)
            {
                ReturnValue = GLSetupTDSAccess.SubmitChanges(AInspectDS);
            }

            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AccountList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AnalysisTypeList.ToString());
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.CostCentreList.ToString());

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return ReturnValue;
        }

        private static bool AccountCodeHasBeenUsed(Int32 ALedgerNumber, string AAccountCode, TDBTransaction Transaction)
        {
            // TODO: enhance sql statement to check for more references to a_account

            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_transaction WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_account_code_c = '" + AAccountCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
            bool IsInUse = (Convert.ToInt32(SqlResult) > 0);

            if (!IsInUse)
            {
                QuerySql =
                    "SELECT COUNT (*) FROM PUB_a_ap_document_detail WHERE " +
                    "a_ledger_number_i=" + ALedgerNumber + " AND " +
                    "a_account_code_c = '" + AAccountCode + "';";
                SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
                IsInUse = (Convert.ToInt32(SqlResult) > 0);
            }

            return IsInUse;
        }

        private static bool AccountHasChildren(Int32 ALedgerNumber, string AAccountCode, TDBTransaction Transaction)
        {
            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_account_hierarchy_detail WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_account_code_to_report_to_c = '" + AAccountCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);

            return Convert.ToInt32(SqlResult) > 0;
        }

        /// <summary>I can add child accounts to this account if it's a summary account,
        ///          or if there have never been transactions posted to it.
        ///
        ///          (If children are added to this account, it will be promoted to a summary account.)
        ///
        ///          I can delete this account if it has no transactions posted as above,
        ///          AND it has no children.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACanBeParent"></param>
        /// <param name="ACanDelete"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetAccountCodeAttributes(Int32 ALedgerNumber, String AAccountCode, out bool ACanBeParent, out bool ACanDelete)
        {
//        public static Boolean AccountCodeCanHaveChildren(Int32 ALedgerNumber, String AAccountCode)
            ACanBeParent = true;
            ACanDelete = true;
            bool DbSuccess = true;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            AAccountTable AccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AAccountCode, Transaction);

            if (AccountTbl.Rows.Count < 1)  // This shouldn't happen..
            {
                DbSuccess = false;
            }
            else
            {
                bool IsParent = AccountHasChildren(ALedgerNumber, AAccountCode, Transaction);
                AAccountRow AccountRow = AccountTbl[0];
                ACanBeParent = IsParent; // If it's a summary account, it's OK (This shouldn't happen either, because the client shouldn't ask me!)
                ACanDelete = !IsParent;

                if (!ACanBeParent || ACanDelete)
                {
                    bool IsInUse = AccountCodeHasBeenUsed(ALedgerNumber, AAccountCode, Transaction);
                    ACanBeParent = !IsInUse;    // For posting accounts, I can still add children (and upgrade the account) if there's nothing posted to it yet.
                    ACanDelete = !IsInUse;      // Once it has transactions posted, I can't delete it, ever.
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return DbSuccess;
        }

        #region Data Validation

        static partial void ValidateAAnalysisType(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateAAnalysisTypeManual(TValidationControlsDict ValidationControlsDict,
            ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation

        /// <summary>
        /// Find out whether I can detach this Analysis Type Code from this account
        /// If it's been used in any transactions, I'm stuck with it.
        /// Cascading checks AApAnalAttrib, ARecurringTransAnalAttrib and ATransAnalAttrib.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="AAnalysisTypeCode"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean CanDetachAnalysisType(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode)
        {
            List <TRowReferenceInfo>References;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            Int32 RefCount = AAnalysisAttributeCascading.CountByPrimaryKey(ALedgerNumber,
                AAnalysisTypeCode,
                AAccountCode,
                Transaction,
                true,
                out References);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return RefCount == 0;
        }

        /// <summary>
        /// helper function for ExportAccountHierarchy
        /// </summary>
        private static void InsertNodeIntoXmlDocument(GLSetupTDS AMainDS,
            XmlDocument ADoc,
            XmlNode AParentNode,
            AAccountHierarchyDetailRow ADetailRow)
        {
            AAccountRow account = (AAccountRow)AMainDS.AAccount.Rows.Find(new object[] { ADetailRow.LedgerNumber, ADetailRow.ReportingAccountCode });
            XmlElement accountNode = ADoc.CreateElement(TYml2Xml.XMLELEMENT);

            // AccountCodeToReportTo and ReportOrder are encoded implicitly
            accountNode.SetAttribute("name", ADetailRow.ReportingAccountCode);
            accountNode.SetAttribute("active", account.AccountActiveFlag ? "True" : "False");
            accountNode.SetAttribute("type", account.AccountType.ToString());
            accountNode.SetAttribute("debitcredit", account.DebitCreditIndicator ? "debit" : "credit");
            accountNode.SetAttribute("validcc", account.ValidCcCombo);
            accountNode.SetAttribute("shortdesc", account.EngAccountCodeShortDesc);

            if (account.EngAccountCodeLongDesc != account.EngAccountCodeShortDesc)
            {
                accountNode.SetAttribute("longdesc", account.EngAccountCodeLongDesc);
            }

            if (account.EngAccountCodeShortDesc != account.AccountCodeShortDesc)
            {
                accountNode.SetAttribute("localdesc", account.AccountCodeShortDesc);
            }

            if (account.EngAccountCodeLongDesc != account.AccountCodeLongDesc)
            {
                accountNode.SetAttribute("locallongdesc", account.AccountCodeLongDesc);
            }

            if (AMainDS.AAccountProperty.Rows.Find(new object[] { account.LedgerNumber, account.AccountCode,
                                                                  MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT, "true" }) != null)
            {
                accountNode.SetAttribute("bankaccount", "true");
            }

            if (account.ForeignCurrencyFlag)
            {
                accountNode.SetAttribute("currency", account.ForeignCurrencyCode);
            }

            AParentNode.AppendChild(accountNode);

            AMainDS.AAccountHierarchyDetail.DefaultView.Sort = AAccountHierarchyDetailTable.GetReportOrderDBName();
            AMainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + ADetailRow.AccountHierarchyCode + "' AND " +
                AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + ADetailRow.ReportingAccountCode + "'";

            foreach (DataRowView rowView in AMainDS.AAccountHierarchyDetail.DefaultView)
            {
                AAccountHierarchyDetailRow accountDetail = (AAccountHierarchyDetailRow)rowView.Row;
                InsertNodeIntoXmlDocument(AMainDS, ADoc, accountNode, accountDetail);
            }
        }

        /// <summary>
        /// return a simple XMLDocument (encoded into a string) with the account hierarchy and account details;
        /// root account can be calculated (find which account is reporting nowhere)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountHierarchyName"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string ExportAccountHierarchy(Int32 ALedgerNumber, string AAccountHierarchyName)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            AAccountHierarchyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountHierarchyDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            AAccountPropertyAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            AAccountHierarchyRow accountHierarchy = (AAccountHierarchyRow)MainDS.AAccountHierarchy.Rows.Find(new object[] { ALedgerNumber,
                                                                                                                            AAccountHierarchyName });

            if (accountHierarchy != null)
            {
                // find the BALSHT account that is reporting to the root account
                MainDS.AAccountHierarchyDetail.DefaultView.RowFilter =
                    AAccountHierarchyDetailTable.GetAccountHierarchyCodeDBName() + " = '" + AAccountHierarchyName + "' AND " +
                    AAccountHierarchyDetailTable.GetAccountCodeToReportToDBName() + " = '" + accountHierarchy.RootAccountCode + "'";

                InsertNodeIntoXmlDocument(MainDS, doc, doc.DocumentElement,
                    (AAccountHierarchyDetailRow)MainDS.AAccountHierarchyDetail.DefaultView[0].Row);
            }

            // XmlDocument is not serializable, therefore print it to string and return the string
            return TXMLParser.XmlToString(doc);
        }

        /// <summary>
        /// helper function for ExportCostCentreHierarchy
        /// </summary>
        private static void InsertNodeIntoXmlDocument(GLSetupTDS AMainDS,
            XmlDocument ADoc,
            XmlNode AParentNode,
            ACostCentreRow ADetailRow)
        {
            XmlElement costCentreNode = ADoc.CreateElement(TYml2Xml.XMLELEMENT);

            // CostCentreToReportTo is encoded implicitly
            costCentreNode.SetAttribute("name", ADetailRow.CostCentreCode);
            costCentreNode.SetAttribute("descr", ADetailRow.CostCentreName);
            costCentreNode.SetAttribute("active", ADetailRow.CostCentreActiveFlag ? "True" : "False");
            costCentreNode.SetAttribute("type", ADetailRow.CostCentreType.ToString());
            AParentNode.AppendChild(costCentreNode);

            AMainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
            AMainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " = '" + ADetailRow.CostCentreCode + "'";

            foreach (DataRowView rowView in AMainDS.ACostCentre.DefaultView)
            {
                InsertNodeIntoXmlDocument(AMainDS, ADoc, costCentreNode, (ACostCentreRow)rowView.Row);
            }
        }

        /// <summary>
        /// return a simple XMLDocument (encoded into a string) with the cost centre hierarchy and cost centre details;
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string ExportCostCentreHierarchy(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            XmlDocument doc = TYml2Xml.CreateXmlDocument();

            MainDS.ACostCentre.DefaultView.RowFilter =
                ACostCentreTable.GetCostCentreToReportToDBName() + " IS NULL";

            InsertNodeIntoXmlDocument(MainDS, doc, doc.DocumentElement,
                (ACostCentreRow)MainDS.ACostCentre.DefaultView[0].Row);

            // XmlDocument is not serializable, therefore print it to string and return the string
            return TXMLParser.XmlToString(doc);
        }

        private static void CreateAccountHierarchyRecursively(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            ref StringCollection AImportedAccountNames,
            XmlNode ACurrentNode,
            string AParentAccountCode)
        {
            AAccountRow newAccount = null;

            string AccountCode = TYml2Xml.GetElementName(ACurrentNode);

            AImportedAccountNames.Add(AccountCode);

            // does this account already exist?
            bool newRow = false;
            DataRow existingAccount = AMainDS.AAccount.Rows.Find(new object[] { ALedgerNumber, AccountCode });

            if (existingAccount != null)
            {
                newAccount = (AAccountRow)existingAccount;
                DropAccountProperties(ref AMainDS, ALedgerNumber, AccountCode);
                ((GLSetupTDSAAccountRow)newAccount).BankAccountFlag = false;
            }
            else
            {
                newRow = true;
                newAccount = AMainDS.AAccount.NewRowTyped();
            }

            newAccount.LedgerNumber = AMainDS.AAccountHierarchy[0].LedgerNumber;
            newAccount.AccountCode = AccountCode;
            newAccount.AccountActiveFlag = TYml2Xml.GetAttributeRecursive(ACurrentNode, "active").ToLower() == "true";
            newAccount.AccountType = TYml2Xml.GetAttributeRecursive(ACurrentNode, "type");
            newAccount.DebitCreditIndicator = TYml2Xml.GetAttributeRecursive(ACurrentNode, "debitcredit") == "debit";
            newAccount.ValidCcCombo = TYml2Xml.GetAttributeRecursive(ACurrentNode, "validcc");
            newAccount.EngAccountCodeShortDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "shortdesc");

            if (TXMLParser.HasAttribute(ACurrentNode, "shortdesc"))
            {
                newAccount.EngAccountCodeLongDesc = TYml2Xml.GetAttribute(ACurrentNode, "longdesc");
                newAccount.AccountCodeShortDesc = TYml2Xml.GetAttribute(ACurrentNode, "localdesc");
                newAccount.AccountCodeLongDesc = TYml2Xml.GetAttribute(ACurrentNode, "locallongdesc");
            }
            else
            {
                newAccount.EngAccountCodeLongDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "longdesc");
                newAccount.AccountCodeShortDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "localdesc");
                newAccount.AccountCodeLongDesc = TYml2Xml.GetAttributeRecursive(ACurrentNode, "locallongdesc");
            }

            if (newAccount.EngAccountCodeLongDesc.Length == 0)
            {
                newAccount.EngAccountCodeLongDesc = newAccount.EngAccountCodeShortDesc;
            }

            if (newAccount.AccountCodeShortDesc.Length == 0)
            {
                newAccount.AccountCodeShortDesc = newAccount.EngAccountCodeShortDesc;
            }

            if (newAccount.AccountCodeLongDesc.Length == 0)
            {
                newAccount.AccountCodeLongDesc = newAccount.AccountCodeShortDesc;
            }

            if (newRow)
            {
                AMainDS.AAccount.Rows.Add(newAccount);
            }

            if (TYml2Xml.GetAttributeRecursive(ACurrentNode, "bankaccount") == "true")
            {
                AAccountPropertyRow accProp = AMainDS.AAccountProperty.NewRowTyped(true);
                accProp.LedgerNumber = newAccount.LedgerNumber;
                accProp.AccountCode = newAccount.AccountCode;
                accProp.PropertyCode = MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT;
                accProp.PropertyValue = "true";
                AMainDS.AAccountProperty.Rows.Add(accProp);
                ((GLSetupTDSAAccountRow)newAccount).BankAccountFlag = true;
            }

            if (TYml2Xml.HasAttributeRecursive(ACurrentNode, "currency"))
            {
                string currency = TYml2Xml.GetAttributeRecursive(ACurrentNode, "currency");

                if (currency != AMainDS.ALedger[0].BaseCurrency)
                {
                    newAccount.ForeignCurrencyCode = currency;
                    newAccount.ForeignCurrencyFlag = true;
                }
            }

            // account hierarchy has been deleted, so always add
            AAccountHierarchyDetailRow newAccountHDetail = AMainDS.AAccountHierarchyDetail.NewRowTyped();
            newAccountHDetail.LedgerNumber = AMainDS.AAccountHierarchy[0].LedgerNumber;
            newAccountHDetail.AccountHierarchyCode = AMainDS.AAccountHierarchy[0].AccountHierarchyCode;
            newAccountHDetail.AccountCodeToReportTo = AParentAccountCode;
            newAccountHDetail.ReportingAccountCode = AccountCode;
            newAccountHDetail.ReportOrder = AMainDS.AAccountHierarchyDetail.Rows.Count;

            AMainDS.AAccountHierarchyDetail.Rows.Add(newAccountHDetail);

            newAccount.PostingStatus = !ACurrentNode.HasChildNodes;

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateAccountHierarchyRecursively(ref AMainDS, ALedgerNumber, ref AImportedAccountNames, child, newAccount.AccountCode);
            }
        }

        /// <summary>
        /// only works if there are no balances/transactions yet for the accounts that are deleted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AHierarchyName"></param>
        /// <param name="AXmlAccountHierarchy"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool ImportAccountHierarchy(Int32 ALedgerNumber, string AHierarchyName, string AXmlAccountHierarchy)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(AXmlAccountHierarchy);
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    AXmlAccountHierarchy);
            }

            GLSetupTDS MainDS = LoadAccountHierarchies(ALedgerNumber);
            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedAccountNames = new StringCollection();

            ImportedAccountNames.Add(ALedgerNumber.ToString());

            // delete all account hierarchy details of this hierarchy
            foreach (AAccountHierarchyDetailRow accounthdetail in MainDS.AAccountHierarchyDetail.Rows)
            {
                if (accounthdetail.AccountHierarchyCode == AHierarchyName)
                {
                    accounthdetail.Delete();
                }
            }

            CreateAccountHierarchyRecursively(ref MainDS, ALedgerNumber, ref ImportedAccountNames, root, ALedgerNumber.ToString());

            foreach (AAccountRow accountRow in MainDS.AAccount.Rows)
            {
                if ((accountRow.RowState != DataRowState.Deleted) && !ImportedAccountNames.Contains(accountRow.AccountCode))
                {
                    // if there are any existing posted transactions that reference this account, it can't be deleted.
                    ATransactionTable TransTbl = ATransactionAccess.LoadViaAAccount(ALedgerNumber, accountRow.AccountCode, null);

                    if (TransTbl.Rows.Count == 0) // No-one's used this account, so I can delete it.
                    {
                        //
                        // If the deleted account included Analysis types I need to unlink them from the Account first.

                        foreach (AAnalysisAttributeRow Row in MainDS.AAnalysisAttribute.Rows)
                        {
                            if ((Row.LedgerNumber == ALedgerNumber) && (Row.AccountCode == accountRow.AccountCode))
                            {
                                Row.Delete();
                            }
                        }

                        accountRow.Delete();
                    }
                }
            }

            TVerificationResultCollection VerificationResult;
            return SaveGLSetupTDS(ALedgerNumber, ref MainDS, out VerificationResult) == TSubmitChangesResult.scrOK;
        }

        private static void CreateCostCentresRecursively(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            ref StringCollection AImportedCostCentreCodes,
            XmlNode ACurrentNode,
            string AParentCostCentreCode)
        {
            ACostCentreRow newCostCentre = null;

            string CostCentreCode = TYml2Xml.GetElementName(ACurrentNode);

            AImportedCostCentreCodes.Add(CostCentreCode);

            // does this costcentre already exist?
            bool newRow = false;
            DataRow existingCostCentre = AMainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, CostCentreCode });

            if (existingCostCentre != null)
            {
                newCostCentre = (ACostCentreRow)existingCostCentre;
            }
            else
            {
                newRow = true;
                newCostCentre = AMainDS.ACostCentre.NewRowTyped();
            }

            newCostCentre.LedgerNumber = ALedgerNumber;
            newCostCentre.CostCentreCode = CostCentreCode;
            newCostCentre.CostCentreName = TYml2Xml.GetAttribute(ACurrentNode, "descr");
            newCostCentre.CostCentreActiveFlag = TYml2Xml.GetAttributeRecursive(ACurrentNode, "active").ToLower() == "true";
            newCostCentre.SystemCostCentreFlag = true;
            newCostCentre.CostCentreType = TYml2Xml.GetAttributeRecursive(ACurrentNode, "type");
            newCostCentre.PostingCostCentreFlag = (ACurrentNode.ChildNodes.Count == 0);

            if ((AParentCostCentreCode != null) && (AParentCostCentreCode.Length != 0))
            {
                newCostCentre.CostCentreToReportTo = AParentCostCentreCode;
            }

            if (newRow)
            {
                AMainDS.ACostCentre.Rows.Add(newCostCentre);
            }

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateCostCentresRecursively(ref AMainDS, ALedgerNumber, ref AImportedCostCentreCodes, child, newCostCentre.CostCentreCode);
            }
        }

        /// <summary>
        /// only works if there are no balances/transactions yet for the cost centres that are deleted
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool ImportCostCentreHierarchy(Int32 ALedgerNumber, string AXmlHierarchy)
        {
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlHierarchy);

            GLSetupTDS MainDS = LoadCostCentreHierarchy(ALedgerNumber);
            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedCostCentreNames = new StringCollection();

            CreateCostCentresRecursively(ref MainDS, ALedgerNumber, ref ImportedCostCentreNames, root, null);

            foreach (ACostCentreRow costCentreRow in MainDS.ACostCentre.Rows)
            {
                if ((costCentreRow.RowState != DataRowState.Deleted) && !ImportedCostCentreNames.Contains(costCentreRow.CostCentreCode))
                {
                    // TODO: delete costcentres that don't exist anymore in the new hierarchy, or deactivate them?
                    // (check if their balance is empty and no transactions exist, or catch database constraint violation)
                    // TODO: what about system cost centres? probably alright to ignore here

                    costCentreRow.Delete();
                }
            }

            TVerificationResultCollection VerificationResult;
            return SaveGLSetupTDS(ALedgerNumber, ref MainDS, out VerificationResult) == TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// import basic data for new ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportNewLedger(Int32 ALedgerNumber,
            string AXmlLedgerDetails,
            string AXmlAccountHierarchy,
            string AXmlCostCentreHierarchy,
            string AXmlMotivationDetails
            )
        {
            // TODO ImportNewLedger

            // if this ledger already exists, delete all tables first?
            // Or try to reuse existing balances etc?

            // first create/modify ledger
            // set ForexGainsLossesAccount; there is no foreign key, so no problem

            // create the calendar for the ledger, automatically calculating the dates of the forwarding periods

            // create the partner with special type LEDGER from the ledger number, with 6 trailing zeros

            // create/modify accounts (might need to drop motivation details)

            // create/modify costcentres (might need to drop motivation details)

            // create/modify motivation details

            return false;
        }

        /// import a new Account hierarchy into an empty new ledger
        private static void ImportDefaultAccountHierarchy(ref GLSetupTDS AMainDS, Int32 ALedgerNumber)
        {
            XmlDocument doc;
            TYml2Xml ymlFile;
            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultAccountHierarchy.yml";

            try
            {
                ymlFile = new TYml2Xml(Filename);
                doc = ymlFile.ParseYML2XML();
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    Filename);
            }

            // create the root account
            AAccountHierarchyRow accountHierarchyRow = AMainDS.AAccountHierarchy.NewRowTyped();
            accountHierarchyRow.LedgerNumber = ALedgerNumber;
            accountHierarchyRow.AccountHierarchyCode = "STANDARD";
            accountHierarchyRow.RootAccountCode = ALedgerNumber.ToString();
            AMainDS.AAccountHierarchy.Rows.Add(accountHierarchyRow);

            AAccountRow accountRow = AMainDS.AAccount.NewRowTyped();
            accountRow.LedgerNumber = ALedgerNumber;
            accountRow.AccountCode = ALedgerNumber.ToString();
            accountRow.PostingStatus = false;
            AMainDS.AAccount.Rows.Add(accountRow);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedAccountNames = new StringCollection();

            CreateAccountHierarchyRecursively(ref AMainDS, ALedgerNumber, ref ImportedAccountNames, root, ALedgerNumber.ToString());
        }

        private static void ImportDefaultCostCentreHierarchy(ref GLSetupTDS AMainDS, Int32 ALedgerNumber, string ALedgerName)
        {
            if (ALedgerName.Length == 0)
            {
                throw new Exception("We need a name for the ledger, otherwise the yml will be invalid");
            }

            // load XmlCostCentreHierarchy from a default file

            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultCostCentreHierarchy.yml";
            TextReader reader = new StreamReader(Filename, TTextFile.GetFileEncoding(Filename), false);
            string XmlCostCentreHierarchy = reader.ReadToEnd();

            reader.Close();

            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNUMBER}", ALedgerNumber.ToString());
            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNUMBERWITHLEADINGZEROS}", ALedgerNumber.ToString("00"));

            XmlCostCentreHierarchy = XmlCostCentreHierarchy.Replace("{#LEDGERNAME}", ALedgerName);

            string[] lines = XmlCostCentreHierarchy.Replace("\r", "").Split(new char[] { '\n' });
            TYml2Xml ymlFile = new TYml2Xml(lines);
            XmlDocument doc = ymlFile.ParseYML2XML();

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            StringCollection ImportedCostCentreNames = new StringCollection();

            CreateCostCentresRecursively(ref AMainDS, ALedgerNumber, ref ImportedCostCentreNames, root, null);
        }

        private static void CreateMotivationDetailFee(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            XmlNode ACurrentNode,
            string AMotivationGroupCode,
            string AMotivationDetailCode)
        {
            AMotivationDetailFeeRow newMotivationDetailFee = null;

            string MotivationDetailFeeCode = TYml2Xml.GetElementName(ACurrentNode);

            // does this motivation detail fee already exist?
            DataRow existingMotivationDetailFee =
                AMainDS.AMotivationDetailFee.Rows.Find(new object[] { ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode,
                                                                      MotivationDetailFeeCode });

            if (existingMotivationDetailFee == null)
            {
                newMotivationDetailFee = AMainDS.AMotivationDetailFee.NewRowTyped();
                newMotivationDetailFee.LedgerNumber = ALedgerNumber;
                newMotivationDetailFee.MotivationGroupCode = AMotivationGroupCode;
                newMotivationDetailFee.MotivationDetailCode = AMotivationDetailCode;
                newMotivationDetailFee.FeeCode = MotivationDetailFeeCode;
                AMainDS.AMotivationDetailFee.Rows.Add(newMotivationDetailFee);
            }
        }

        private static void CreateMotivationDetail(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            XmlNode ACurrentNode,
            string AMotivationGroupCode)
        {
            AMotivationDetailRow newMotivationDetail = null;

            string MotivationDetailCode = TYml2Xml.GetElementName(ACurrentNode);

            // does this motivation already exist?
            bool newRow = false;
            DataRow existingMotivationDetail = AMainDS.AMotivationDetail.Rows.Find(new object[] { ALedgerNumber, AMotivationGroupCode,
                                                                                                  MotivationDetailCode });

            if (existingMotivationDetail != null)
            {
                newMotivationDetail = (AMotivationDetailRow)existingMotivationDetail;
            }
            else
            {
                newRow = true;
                newMotivationDetail = AMainDS.AMotivationDetail.NewRowTyped();
            }

            newMotivationDetail.LedgerNumber = ALedgerNumber;
            newMotivationDetail.MotivationGroupCode = AMotivationGroupCode;
            newMotivationDetail.MotivationDetailCode = MotivationDetailCode;

            if (TYml2Xml.HasAttribute(ACurrentNode, "accountcode"))
            {
                newMotivationDetail.AccountCode = TYml2Xml.GetAttribute(ACurrentNode, "accountcode");
            }

            newMotivationDetail.CostCentreCode = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);

            if (TYml2Xml.HasAttribute(ACurrentNode, "description"))
            {
                newMotivationDetail.MotivationDetailDesc = TYml2Xml.GetAttribute(ACurrentNode, "description");
                newMotivationDetail.MotivationDetailDescLocal = newMotivationDetail.MotivationDetailDesc;
            }

            if (newRow)
            {
                AMainDS.AMotivationDetail.Rows.Add(newMotivationDetail);
            }

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateMotivationDetailFee(ref AMainDS, ALedgerNumber, child, newMotivationDetail.MotivationGroupCode,
                    newMotivationDetail.MotivationDetailCode);
            }
        }

        private static void CreateMotivationGroup(ref GLSetupTDS AMainDS,
            Int32 ALedgerNumber,
            XmlNode ACurrentNode)
        {
            AMotivationGroupRow newMotivationGroup = null;

            string MotivationGroupCode = TYml2Xml.GetElementName(ACurrentNode);

            // does this motivation already exist?
            bool newRow = false;
            DataRow existingMotivationGroup = AMainDS.AMotivationGroup.Rows.Find(new object[] { ALedgerNumber, MotivationGroupCode });

            if (existingMotivationGroup != null)
            {
                newMotivationGroup = (AMotivationGroupRow)existingMotivationGroup;
            }
            else
            {
                newRow = true;
                newMotivationGroup = AMainDS.AMotivationGroup.NewRowTyped();
            }

            newMotivationGroup.LedgerNumber = ALedgerNumber;
            newMotivationGroup.MotivationGroupCode = MotivationGroupCode;

            if (TYml2Xml.HasAttribute(ACurrentNode, "desclocal"))
            {
                newMotivationGroup.MotivationGroupDescLocal = TYml2Xml.GetAttribute(ACurrentNode, "desclocal");
            }

            if (TYml2Xml.HasAttribute(ACurrentNode, "description"))
            {
                newMotivationGroup.MotivationGroupDescription = TYml2Xml.GetAttribute(ACurrentNode, "description");
            }

            if (newMotivationGroup.MotivationGroupDescription.Length == 0)
            {
                newMotivationGroup.MotivationGroupDescription = newMotivationGroup.MotivationGroupDescLocal;
            }

            if (newRow)
            {
                AMainDS.AMotivationGroup.Rows.Add(newMotivationGroup);
            }

            foreach (XmlNode child in ACurrentNode.ChildNodes)
            {
                CreateMotivationDetail(ref AMainDS, ALedgerNumber, child, newMotivationGroup.MotivationGroupCode);
            }
        }

        /// import motivation groups, details into an empty new ledger
        private static void ImportDefaultMotivations(ref GLSetupTDS AMainDS, Int32 ALedgerNumber)
        {
            XmlDocument doc;
            TYml2Xml ymlFile;
            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultMotivations.yml";

            try
            {
                ymlFile = new TYml2Xml(Filename);
                doc = ymlFile.ParseYML2XML();
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    Filename);
            }

            XmlNode root = doc.FirstChild.NextSibling;

            foreach (XmlNode child in root)
            {
                CreateMotivationGroup(ref AMainDS, ALedgerNumber, child);
            }
        }

        /// import records for fees payable or receivable into an empty new ledger
        private static void ImportDefaultAdminGrantsPayableReceivable(ref GLSetupTDS AMainDS, Int32 ALedgerNumber)
        {
            AFeesPayableRow newFeesPayableRow = null;
            AFeesReceivableRow newFeesReceivableRow = null;
            bool newRow;
            DataRow existingRow;

            bool IsFeesPayable;
            string FeeCode;

            XmlDocument doc;
            TYml2Xml ymlFile;
            string Filename = TAppSettingsManager.GetValue("SqlFiles.Path", ".") +
                              Path.DirectorySeparatorChar +
                              "DefaultAdminGrantsPayableReceivable.yml";

            try
            {
                ymlFile = new TYml2Xml(Filename);
                doc = ymlFile.ParseYML2XML();
            }
            catch (XmlException exp)
            {
                throw new Exception(
                    Catalog.GetString("There was a problem with the syntax of the file.") +
                    Environment.NewLine +
                    exp.Message +
                    Environment.NewLine +
                    Filename);
            }

            XmlNode root = doc.FirstChild.NextSibling;

            foreach (XmlNode child in root)
            {
                FeeCode = TYml2Xml.GetElementName(child);

                IsFeesPayable = (TYml2Xml.GetAttribute(child, "feespayable") == "yes"
                                 || TYml2Xml.GetAttribute(child, "feespayable") == "true");

                if (IsFeesPayable)
                {
                    // does this fee already exist?
                    newRow = false;
                    existingRow = AMainDS.AFeesPayable.Rows.Find(new object[] { ALedgerNumber, FeeCode });

                    if (existingRow != null)
                    {
                        newFeesPayableRow = (AFeesPayableRow)existingRow;
                    }
                    else
                    {
                        newRow = true;
                        newFeesPayableRow = AMainDS.AFeesPayable.NewRowTyped();
                    }

                    newFeesPayableRow.LedgerNumber = ALedgerNumber;
                    newFeesPayableRow.FeeCode = FeeCode;
                    newFeesPayableRow.ChargeOption = TYml2Xml.GetAttribute(child, "chargeoption");

                    if (TYml2Xml.HasAttribute(child, "percentage"))
                    {
                        newFeesPayableRow.ChargePercentage = Convert.ToInt32(TYml2Xml.GetAttribute(child, "percentage"));
                    }

                    newFeesPayableRow.CostCentreCode = TYml2Xml.GetAttribute(child, "costcentrecode");
                    newFeesPayableRow.AccountCode = TYml2Xml.GetAttribute(child, "accountcode");
                    newFeesPayableRow.DrAccountCode = TYml2Xml.GetAttribute(child, "draccountcode");

                    if (TYml2Xml.HasAttribute(child, "description"))
                    {
                        newFeesPayableRow.FeeDescription = TYml2Xml.GetAttribute(child, "description");
                    }

                    if (newRow)
                    {
                        AMainDS.AFeesPayable.Rows.Add(newFeesPayableRow);
                    }
                }
                else
                {
                    // does this fee already exist?
                    newRow = false;
                    existingRow = AMainDS.AFeesReceivable.Rows.Find(new object[] { ALedgerNumber, FeeCode });

                    if (existingRow != null)
                    {
                        newFeesReceivableRow = (AFeesReceivableRow)existingRow;
                    }
                    else
                    {
                        newRow = true;
                        newFeesReceivableRow = AMainDS.AFeesReceivable.NewRowTyped();
                    }

                    newFeesReceivableRow.LedgerNumber = ALedgerNumber;
                    newFeesReceivableRow.FeeCode = FeeCode;
                    newFeesReceivableRow.ChargeOption = TYml2Xml.GetAttribute(child, "chargeoption");

                    if (TYml2Xml.HasAttribute(child, "percentage"))
                    {
                        newFeesReceivableRow.ChargePercentage = Convert.ToInt32(TYml2Xml.GetAttribute(child, "percentage"));
                    }

                    newFeesReceivableRow.CostCentreCode = TYml2Xml.GetAttribute(child, "costcentrecode");
                    newFeesReceivableRow.AccountCode = TYml2Xml.GetAttribute(child, "accountcode");
                    newFeesReceivableRow.DrAccountCode = TYml2Xml.GetAttribute(child, "draccountcode");

                    if (TYml2Xml.HasAttribute(child, "description"))
                    {
                        newFeesReceivableRow.FeeDescription = TYml2Xml.GetAttribute(child, "description");
                    }

                    if (newRow)
                    {
                        AMainDS.AFeesReceivable.Rows.Add(newFeesReceivableRow);
                    }
                }
            }
        }

        /// <summary>
        /// create a new ledger and do the initial setup
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool CreateNewLedger(Int32 ANewLedgerNumber,
            String ALedgerName,
            String ACountryCode,
            String ABaseCurrency,
            String AIntlCurrency,
            DateTime ACalendarStartDate,
            Int32 ANumberOfPeriods,
            Int32 ACurrentPeriod,
            Int32 ANumberOfFwdPostingPeriods,
            bool AActivateGiftReceipting,
            Int32 AStartingReceiptNumber,
            bool AActivateAccountsPayable,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            // check if such a ledger already exists
            ALedgerTable tempLedger = ALedgerAccess.LoadByPrimaryKey(ANewLedgerNumber, Transaction);

            if (tempLedger.Count > 0)
            {
                AVerificationResult = new TVerificationResultCollection();
                string msg = String.Format(Catalog.GetString(
                        "There is already a ledger with number {0}. Please choose another number."), ANewLedgerNumber);
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Ledger"), msg, TResultSeverity.Resv_Critical));
                return false;
            }

            if ((ANewLedgerNumber <= 1) || (ANewLedgerNumber > 9999))
            {
                // ledger number 1 does not work, because the root unit has partner key 1000000.
                AVerificationResult = new TVerificationResultCollection();
                string msg = String.Format(Catalog.GetString(
                        "Invalid number {0} for a ledger. Please choose a number between 2 and 9999."), ANewLedgerNumber);
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Ledger"), msg, TResultSeverity.Resv_Critical));
                return false;
            }

            Int64 PartnerKey = Convert.ToInt64(ANewLedgerNumber) * 1000000L;
            GLSetupTDS MainDS = new GLSetupTDS();

            ALedgerRow ledgerRow = MainDS.ALedger.NewRowTyped();
            ledgerRow.LedgerNumber = ANewLedgerNumber;
            ledgerRow.LedgerName = ALedgerName;
            ledgerRow.CurrentPeriod = ACurrentPeriod;
            ledgerRow.NumberOfAccountingPeriods = ANumberOfPeriods;
            ledgerRow.NumberFwdPostingPeriods = ANumberOfFwdPostingPeriods;
            ledgerRow.BaseCurrency = ABaseCurrency;
            ledgerRow.IntlCurrency = AIntlCurrency;
            ledgerRow.ActualsDataRetention = 11;
            ledgerRow.GiftDataRetention = 11;
            ledgerRow.BudgetDataRetention = 2;
            ledgerRow.CountryCode = ACountryCode;
            ledgerRow.ForexGainsLossesAccount = "5003";
            ledgerRow.PartnerKey = PartnerKey;

            if (ANumberOfPeriods == 12)
            {
                ledgerRow.CalendarMode = true;
            }
            else
            {
                ledgerRow.CalendarMode = false;
            }

            MainDS.ALedger.Rows.Add(ledgerRow);

            PPartnerRow partnerRow;

            if (!PPartnerAccess.Exists(PartnerKey, Transaction))
            {
                partnerRow = MainDS.PPartner.NewRowTyped();
                ledgerRow.PartnerKey = PartnerKey;
                partnerRow.PartnerKey = PartnerKey;
                partnerRow.PartnerShortName = ALedgerName;
                partnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
                partnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
                MainDS.PPartner.Rows.Add(partnerRow);

                // create or use addresses (only if partner record is created here as
                // otherwise we assume that Partner has address already)
                PLocationRow locationRow;
                PLocationTable LocTemplateTable;
                PLocationTable LocResultTable;
                PLocationRow LocTemplateRow;
                StringCollection LocTemplateOperators;

                // find address with country set
                LocTemplateTable = new PLocationTable();
                LocTemplateRow = LocTemplateTable.NewRowTyped(false);
                LocTemplateRow.SiteKey = 0;
                LocTemplateRow.StreetName = Catalog.GetString("No valid address on file");
                LocTemplateRow.CountryCode = ACountryCode;
                LocTemplateOperators = new StringCollection();

                LocResultTable = PLocationAccess.LoadUsingTemplate(LocTemplateRow, LocTemplateOperators, Transaction);

                if (LocResultTable.Count > 0)
                {
                    locationRow = (PLocationRow)LocResultTable.Rows[0];
                }
                else
                {
                    // no location record exists yet: create new one
                    locationRow = MainDS.PLocation.NewRowTyped();
                    locationRow.SiteKey = 0;
                    locationRow.LocationKey = (int)DBAccess.GDBAccessObj.GetNextSequenceValue(
                        TSequenceNames.seq_location_number.ToString(), Transaction);
                    locationRow.StreetName = Catalog.GetString("No valid address on file");
                    locationRow.CountryCode = ACountryCode;
                    MainDS.PLocation.Rows.Add(locationRow);
                }

                // now create partner location record
                PPartnerLocationRow partnerLocationRow = MainDS.PPartnerLocation.NewRowTyped();
                partnerLocationRow.SiteKey = locationRow.SiteKey;
                partnerLocationRow.PartnerKey = PartnerKey;
                partnerLocationRow.LocationKey = locationRow.LocationKey;
                partnerLocationRow.DateEffective = DateTime.Today;
                MainDS.PPartnerLocation.Rows.Add(partnerLocationRow);
            }
            else
            {
                // partner record already exists in database -> update ledger name
                PPartnerAccess.LoadByPrimaryKey(MainDS, PartnerKey, Transaction);
                partnerRow = (PPartnerRow)MainDS.PPartner.Rows[0];
                partnerRow.PartnerShortName = ALedgerName;
            }

            PPartnerTypeAccess.LoadViaPPartner(MainDS, PartnerKey, Transaction);
            PPartnerTypeRow partnerTypeRow;

            // only create special type "LEDGER" if it does not exist yet
            if (MainDS.PPartnerType.Rows.Find(new object[] { PartnerKey, MPartnerConstants.PARTNERTYPE_LEDGER }) == null)
            {
                partnerTypeRow = MainDS.PPartnerType.NewRowTyped();
                partnerTypeRow.PartnerKey = PartnerKey;
                partnerTypeRow.TypeCode = MPartnerConstants.PARTNERTYPE_LEDGER;
                MainDS.PPartnerType.Rows.Add(partnerTypeRow);
            }

            if (!PUnitAccess.Exists(PartnerKey, Transaction))
            {
                PUnitRow unitRow = MainDS.PUnit.NewRowTyped();
                unitRow.PartnerKey = PartnerKey;
                unitRow.UnitName = ALedgerName;
                MainDS.PUnit.Rows.Add(unitRow);
            }

            if (!PPartnerLedgerAccess.Exists(PartnerKey, Transaction))
            {
                PPartnerLedgerRow partnerLedgerRow = MainDS.PPartnerLedger.NewRowTyped();
                partnerLedgerRow.PartnerKey = PartnerKey;

                // calculate last partner id, from older uses of this ledger number
                object MaxExistingPartnerKeyObj = DBAccess.GDBAccessObj.ExecuteScalar(
                    String.Format("SELECT MAX(p_partner_key_n) FROM PUB_p_partner " +
                        "WHERE p_partner_key_n > {0} AND p_partner_key_n < {1}",
                        PartnerKey,
                        PartnerKey + 500000), Transaction);

                if (MaxExistingPartnerKeyObj.GetType() != typeof(DBNull))
                {
                    partnerLedgerRow.LastPartnerId = Convert.ToInt32(Convert.ToInt64(MaxExistingPartnerKeyObj) - PartnerKey);
                }
                else
                {
                    partnerLedgerRow.LastPartnerId = 5000;
                }

                MainDS.PPartnerLedger.Rows.Add(partnerLedgerRow);
            }

            String ModuleId = "LEDGER" + ANewLedgerNumber.ToString("0000");

            if (!SModuleAccess.Exists(ModuleId, Transaction))
            {
                SModuleRow moduleRow = MainDS.SModule.NewRowTyped();
                moduleRow.ModuleId = ModuleId;
                moduleRow.ModuleName = moduleRow.ModuleId;
                MainDS.SModule.Rows.Add(moduleRow);
            }

            // if this is the first ledger, make it the default site
            SSystemDefaultsTable systemDefaults = SSystemDefaultsAccess.LoadByPrimaryKey("SiteKey", Transaction);

            if (systemDefaults.Rows.Count == 0)
            {
                SSystemDefaultsRow systemDefaultsRow = MainDS.SSystemDefaults.NewRowTyped();
                systemDefaultsRow.DefaultCode = SharedConstants.SYSDEFAULT_SITEKEY;
                systemDefaultsRow.DefaultDescription = "there has to be one site key for the database";
                systemDefaultsRow.DefaultValue = PartnerKey.ToString("0000000000");
                MainDS.SSystemDefaults.Rows.Add(systemDefaultsRow);
            }

            // create calendar
            // at the moment we only support financial years that start on the first day of a month
            // and currently only 12 or 13 periods are allowed and a maximum of 8 forward periods
            DateTime periodStartDate = ACalendarStartDate;

            for (Int32 periodNumber = 1; periodNumber <= ANumberOfPeriods + ANumberOfFwdPostingPeriods; periodNumber++)
            {
                AAccountingPeriodRow accountingPeriodRow = MainDS.AAccountingPeriod.NewRowTyped();
                accountingPeriodRow.LedgerNumber = ANewLedgerNumber;
                accountingPeriodRow.AccountingPeriodNumber = periodNumber;
                accountingPeriodRow.PeriodStartDate = periodStartDate;

                if ((ANumberOfPeriods == 13)
                    && (periodNumber == 12))
                {
                    // in case of 12 periods the second last period represents the last month except for the very last day
                    accountingPeriodRow.PeriodEndDate = periodStartDate.AddMonths(1).AddDays(-2);
                }
                else if ((ANumberOfPeriods == 13)
                         && (periodNumber == 13))
                {
                    // in case of 13 periods the last period just represents the very last day of the financial year
                    accountingPeriodRow.PeriodEndDate = periodStartDate;
                }
                else
                {
                    accountingPeriodRow.PeriodEndDate = periodStartDate.AddMonths(1).AddDays(-1);
                }

                accountingPeriodRow.AccountingPeriodDesc = periodStartDate.ToString("MMMM");
                MainDS.AAccountingPeriod.Rows.Add(accountingPeriodRow);
                periodStartDate = accountingPeriodRow.PeriodEndDate.AddDays(1);
            }

            // mark cached table for accounting periods to be refreshed
            TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                TCacheableFinanceTablesEnum.AccountingPeriodList.ToString());

            AAccountingSystemParameterRow accountingSystemParameterRow = MainDS.AAccountingSystemParameter.NewRowTyped();
            accountingSystemParameterRow.LedgerNumber = ANewLedgerNumber;
            accountingSystemParameterRow.ActualsDataRetention = ledgerRow.ActualsDataRetention;
            accountingSystemParameterRow.GiftDataRetention = ledgerRow.GiftDataRetention;
            accountingSystemParameterRow.NumberFwdPostingPeriods = ledgerRow.NumberFwdPostingPeriods;
            accountingSystemParameterRow.NumberOfAccountingPeriods = ledgerRow.NumberOfAccountingPeriods;
            accountingSystemParameterRow.BudgetDataRetention = ledgerRow.BudgetDataRetention;
            MainDS.AAccountingSystemParameter.Rows.Add(accountingSystemParameterRow);

            // activate GL subsystem (this is always active)
            ASystemInterfaceRow systemInterfaceRow = MainDS.ASystemInterface.NewRowTyped();
            systemInterfaceRow.LedgerNumber = ANewLedgerNumber;

            systemInterfaceRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            systemInterfaceRow.SetUpComplete = true;
            MainDS.ASystemInterface.Rows.Add(systemInterfaceRow);


            ATransactionTypeRow transactionTypeRow;

            // TODO: this might be different for other account or costcentre names
            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.ALLOC.ToString();
            transactionTypeRow.DebitAccountCode = "BAL SHT";
            transactionTypeRow.CreditAccountCode = "BAL SHT";
            transactionTypeRow.TransactionTypeDescription = "Allocation Journal";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);

            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REALLOC.ToString();
            transactionTypeRow.DebitAccountCode = "BAL SHT";
            transactionTypeRow.CreditAccountCode = "BAL SHT";
            transactionTypeRow.TransactionTypeDescription = "Reallocation Journal";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);

            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
            transactionTypeRow.DebitAccountCode = "5003";
            transactionTypeRow.CreditAccountCode = "5003";
            transactionTypeRow.TransactionTypeDescription = "Foreign Exchange Revaluation";
            transactionTypeRow.SpecialTransactionType = true;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);

            transactionTypeRow = MainDS.ATransactionType.NewRowTyped();
            transactionTypeRow.LedgerNumber = ANewLedgerNumber;
            transactionTypeRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            transactionTypeRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
            transactionTypeRow.DebitAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
            transactionTypeRow.CreditAccountCode = MFinanceConstants.ACCOUNT_BAL_SHT;
            transactionTypeRow.TransactionTypeDescription = "Standard Journal";
            transactionTypeRow.SpecialTransactionType = false;
            MainDS.ATransactionType.Rows.Add(transactionTypeRow);


            AValidLedgerNumberTable validLedgerNumberTable = AValidLedgerNumberAccess.LoadByPrimaryKey(ANewLedgerNumber, PartnerKey, Transaction);

            if (validLedgerNumberTable.Rows.Count == 0)
            {
                AValidLedgerNumberRow validLedgerNumberRow = MainDS.AValidLedgerNumber.NewRowTyped();
                validLedgerNumberRow.PartnerKey = PartnerKey;
                validLedgerNumberRow.LedgerNumber = ANewLedgerNumber;

                // TODO can we assume that ledger 4 is used for international clearing?
                // but in the empty database, that ledger and therefore p_partner with key 4000000 does not exist
                // validLedgerNumberRow.IltProcessingCentre = 4000000;

                validLedgerNumberRow.CostCentreCode = (ANewLedgerNumber * 100).ToString("0000");
                MainDS.AValidLedgerNumber.Rows.Add(validLedgerNumberRow);
            }

            ACostCentreTypesRow costCentreTypesRow = MainDS.ACostCentreTypes.NewRowTyped();
            costCentreTypesRow.LedgerNumber = ANewLedgerNumber;
            costCentreTypesRow.CostCentreType = "Local";
            costCentreTypesRow.Deletable = false;
            MainDS.ACostCentreTypes.Rows.Add(costCentreTypesRow);
            costCentreTypesRow = MainDS.ACostCentreTypes.NewRowTyped();
            costCentreTypesRow.LedgerNumber = ANewLedgerNumber;
            costCentreTypesRow.CostCentreType = "Foreign";
            costCentreTypesRow.Deletable = false;
            MainDS.ACostCentreTypes.Rows.Add(costCentreTypesRow);


            ImportDefaultAccountHierarchy(ref MainDS, ANewLedgerNumber);
            ImportDefaultCostCentreHierarchy(ref MainDS, ANewLedgerNumber, ALedgerName);
            ImportDefaultMotivations(ref MainDS, ANewLedgerNumber);
            ImportDefaultAdminGrantsPayableReceivable(ref MainDS, ANewLedgerNumber);


            // TODO: modify UI navigation yml file etc?
            // TODO: permissions for which users?

            TSubmitChangesResult result = GLSetupTDSAccess.SubmitChanges(MainDS);

            // activate gift receipting subsystem
            if (AActivateGiftReceipting)
            {
                ActivateGiftReceiptingSubsystem(ANewLedgerNumber, AStartingReceiptNumber, out AVerificationResult);
            }

            // activate accounts payable subsystem
            if (AActivateAccountsPayable)
            {
                ActivateAccountsPayableSubsystem(ANewLedgerNumber, out AVerificationResult);
            }

            if (result == TSubmitChangesResult.scrOK)
            {
                // give the current user access permissions to this new ledger
                SUserModuleAccessPermissionTable moduleAccessPermissionTable = new SUserModuleAccessPermissionTable();

                SUserModuleAccessPermissionRow moduleAccessPermissionRow = moduleAccessPermissionTable.NewRowTyped();
                moduleAccessPermissionRow.UserId = UserInfo.GUserInfo.UserID;
                moduleAccessPermissionRow.ModuleId = "LEDGER" + ANewLedgerNumber.ToString("0000");
                moduleAccessPermissionRow.CanAccess = true;
                moduleAccessPermissionTable.Rows.Add(moduleAccessPermissionRow);

                try
                {
                    if (!SUserModuleAccessPermissionAccess.SubmitChanges(moduleAccessPermissionTable, Transaction, out AVerificationResult))
                    {
                        return false;
                    }

                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                finally
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return result == TSubmitChangesResult.scrOK;
        }

        /// <summary>
        /// return true if the ledger contains any transactions
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool ContainsTransactions(Int32 ALedgerNumber)
        {
            bool Result = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            Result = (ATransactionAccess.CountViaALedger(ALedgerNumber, Transaction) > 0);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return Result;
        }

        /// <summary>
        /// deletes the complete ledger, with all finance data. useful for testing purposes
        /// </summary>
        [RequireModulePermission("FINANCE-3")]
        public static bool DeleteLedger(Int32 ALedgerNumber, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting ledger"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Deleting ledger"),
                20);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                OdbcParameter[] ledgerparameter = new OdbcParameter[] {
                    new OdbcParameter("ledgernumber", OdbcType.Int)
                };
                ledgerparameter[0].Value = ALedgerNumber;

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format("DELETE FROM PUB_{0} WHERE {1} = 'LEDGER{2:0000}'",
                        SUserModuleAccessPermissionTable.GetTableDBName(),
                        SUserModuleAccessPermissionTable.GetModuleIdDBName(),
                        ALedgerNumber),
                    Transaction);

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format("DELETE FROM PUB_{0} WHERE {1} = 'LEDGER{2:0000}'",
                        SModuleTable.GetTableDBName(),
                        SModuleTable.GetModuleIdDBName(),
                        ALedgerNumber),
                    Transaction);

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format(
                        "DELETE FROM PUB_{0} WHERE EXISTS (SELECT * FROM PUB_{1} WHERE {2}.{3} = {4}.{5} AND {6}.{7} = ?)",
                        AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                        AGeneralLedgerMasterTable.GetTableDBName(),
                        AGeneralLedgerMasterTable.GetTableDBName(),
                        AGeneralLedgerMasterTable.GetGlmSequenceDBName(),
                        AGeneralLedgerMasterPeriodTable.GetTableDBName(),
                        AGeneralLedgerMasterPeriodTable.GetGlmSequenceDBName(),
                        AGeneralLedgerMasterTable.GetTableDBName(),
                        AGeneralLedgerMasterTable.GetLedgerNumberDBName()),
                    Transaction, ledgerparameter);

                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format(
                        "DELETE FROM PUB_{0} WHERE EXISTS (SELECT * FROM PUB_{1} WHERE {2}.{3} = {4}.{5} AND {6}.{7} = ?)",
                        ABudgetPeriodTable.GetTableDBName(),
                        ABudgetTable.GetTableDBName(),
                        ABudgetTable.GetTableDBName(),
                        ABudgetTable.GetBudgetSequenceDBName(),
                        ABudgetPeriodTable.GetTableDBName(),
                        ABudgetPeriodTable.GetBudgetSequenceDBName(),
                        ABudgetTable.GetTableDBName(),
                        ABudgetTable.GetLedgerNumberDBName()),
                    Transaction, ledgerparameter);

                // the following tables are not deleted at the moment as they are not in use
                //      PFoundationProposalDetailTable.GetTableDBName(),
                //      AEpTransactionTable.GetTableDBName(),
                //      AEpStatementTable.GetTableDBName(),
                //      AEpMatchTable.GetTableDBName(),
                // also: tables referring to ATaxTableTable are not deleted now as they are not yet in use
                //      (those are tables needed in the accounts receivable module that does not exist yet)


                string[] tablenames = new string[] {
                    AValidLedgerNumberTable.GetTableDBName(),
                         AProcessedFeeTable.GetTableDBName(),
                         AGeneralLedgerMasterTable.GetTableDBName(),
                         AMotivationDetailFeeTable.GetTableDBName(),

                         ABudgetTable.GetTableDBName(),
                         ABudgetRevisionTable.GetTableDBName(),

                         ARecurringGiftDetailTable.GetTableDBName(),
                         ARecurringGiftTable.GetTableDBName(),
                         ARecurringGiftBatchTable.GetTableDBName(),

                         AGiftDetailTable.GetTableDBName(),
                         AGiftTable.GetTableDBName(),
                         AGiftBatchTable.GetTableDBName(),

                         ATransAnalAttribTable.GetTableDBName(),
                         ATransactionTable.GetTableDBName(),
                         AJournalTable.GetTableDBName(),
                         ABatchTable.GetTableDBName(),

                         ARecurringTransAnalAttribTable.GetTableDBName(),
                         ARecurringTransactionTable.GetTableDBName(),
                         ARecurringJournalTable.GetTableDBName(),
                         ARecurringBatchTable.GetTableDBName(),

                         AEpDocumentPaymentTable.GetTableDBName(),
                         AEpPaymentTable.GetTableDBName(),

                         AApAnalAttribTable.GetTableDBName(),
                         AApDocumentPaymentTable.GetTableDBName(),
                         AApPaymentTable.GetTableDBName(),
                         ACrdtNoteInvoiceLinkTable.GetTableDBName(),
                         AApDocumentDetailTable.GetTableDBName(),
                         AApDocumentTable.GetTableDBName(),

                         AFreeformAnalysisTable.GetTableDBName(),

                         AEpAccountTable.GetTableDBName(),
                         ASuspenseAccountTable.GetTableDBName(),
                         SGroupMotivationTable.GetTableDBName(),
                         AIchStewardshipTable.GetTableDBName(),
                         SGroupCostCentreTable.GetTableDBName(),
                         AAnalysisAttributeTable.GetTableDBName(),

                         AMotivationDetailTable.GetTableDBName(),
                         AMotivationGroupTable.GetTableDBName(),
                         AFeesReceivableTable.GetTableDBName(),
                         AFeesPayableTable.GetTableDBName(),
                         ACostCentreTable.GetTableDBName(),
                         ATransactionTypeTable.GetTableDBName(),
                         AAccountPropertyTable.GetTableDBName(),
                         AAccountHierarchyDetailTable.GetTableDBName(),
                         AAccountHierarchyTable.GetTableDBName(),
                         AAccountTable.GetTableDBName(),
                         ASystemInterfaceTable.GetTableDBName(),
                         AAccountingSystemParameterTable.GetTableDBName(),
                         ACostCentreTypesTable.GetTableDBName(),

                         ALedgerInitFlagTable.GetTableDBName(),
                         ATaxTableTable.GetTableDBName(),

                         AAccountingPeriodTable.GetTableDBName(),

                         SGroupLedgerTable.GetTableDBName()
                };

                foreach (string table in tablenames)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery(
                        String.Format("DELETE FROM PUB_{0} WHERE a_ledger_number_i = ?", table),
                        Transaction, ledgerparameter);
                }

                ALedgerAccess.DeleteByPrimaryKey(ALedgerNumber, Transaction);

                // remove from the list of sites when creating new partners
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    String.Format("DELETE FROM PUB_{0} WHERE p_partner_key_n = {1}",
                        PPartnerLedgerTable.GetTableDBName(),
                        Convert.ToInt64(ALedgerNumber) * 1000000),
                    Transaction);

                if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                {
                    throw new Exception("Deletion of Ledger was cancelled by the user");
                }

                DBAccess.GDBAccessObj.CommitTransaction();

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());

                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult(
                        "Problems deleting ledger " + ALedgerNumber.ToString(),
                        e.Message,
                        "Cannot delete ledger",
                        string.Empty,
                        TResultSeverity.Resv_Critical,
                        Guid.Empty));
                DBAccess.GDBAccessObj.RollbackTransaction();
                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// get the ledger numbers that are available for the current user
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static ALedgerTable GetAvailableLedgers()
        {
            // TODO check for permissions of the current user
            StringCollection Fields = new StringCollection();

            Fields.Add(ALedgerTable.GetLedgerNameDBName());
            Fields.Add(ALedgerTable.GetLedgerNumberDBName());
            Fields.Add(ALedgerTable.GetBaseCurrencyDBName());
            Fields.Add(ALedgerTable.GetLedgerStatusDBName());
            return ALedgerAccess.LoadAll(Fields, null, null, 0, 0);
        }

        /// <summary>
        /// Load  the table AFREEFORMANALSYSIS
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AFreeformAnalysisTable LoadAFreeformAnalysis(Int32 ALedgerNumber)
        {
            GLSetupTDS MainDS = new GLSetupTDS();

            AFreeformAnalysisAccess.LoadViaALedger(MainDS, ALedgerNumber, null);

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();
            AFreeformAnalysisTable myAT = MainDS.AFreeformAnalysis;
            return myAT;
        }

        /// <summary>
        /// Check if a value in  AFREEFORMANALSYSIS cand be deleted (count the references in ATRansANALATTRIB)
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static int CheckDeleteAFreeformAnalysis(Int32 ALedgerNumber, String ATypeCode, String AAnalysisValue)
        {
            return ATransAnalAttribAccess.CountViaAFreeformAnalysis(ALedgerNumber, ATypeCode, AAnalysisValue, null);
        }

        /// <summary>
        /// Check if a TypeCode in  AnalysisType can be deleted (count the references in ATRansAnalysisAtrributes)
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static int CheckDeleteAAnalysisType(String ATypeCode)
        {
            return AAnalysisAttributeAccess.CountViaAAnalysisType(ATypeCode, null);
        }

        /// <summary>
        /// Get a list of Analysis Attributes that must be used with this account.
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static StringCollection RequiredAnalysisAttributesForAccount(Int32 ALedgerNumber, String AAccountCode, Boolean AActiveOnly = false)
        {
            StringCollection Ret = new StringCollection();
            AAnalysisAttributeTable tbl = AAnalysisAttributeAccess.LoadViaAAccount(ALedgerNumber, AAccountCode, null);

            foreach (AAnalysisAttributeRow Row in tbl.Rows)
            {
                if (!AActiveOnly || Row.Active)
                {
                    Ret.Add(Row.AnalysisTypeCode);
                }
            }

            return Ret;
        }

        /// <summary>
        /// Check if this account code for Ledger ALedgerNumber requires one or more analysis attributes
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool HasAccountSetupAnalysisAttributes(Int32 ALedgerNumber, String AAccountCode)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            bool HasAccountAnalysisAttributes = false;

            HasAccountAnalysisAttributes = (AAnalysisAttributeAccess.CountViaAAccount(ALedgerNumber, AAccountCode, Transaction) > 0);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return HasAccountAnalysisAttributes;
        }

        //
        //    Rename Account: to rename an AccountCode or a CostCentreCode, we need to update lots of values all over the database:

        private static void UpdateAccountField(String ATblName,
            String AFldName,
            String AOldName,
            String ANewName,
            Int32 ALedgerNumber,
            TDBTransaction ATransaction,
            ref String AttemptedOperation)
        {
            AttemptedOperation = "Rename " + AFldName + " in " + ATblName;
            String QuerySql =
                "UPDATE PUB_" + ATblName +
                " SET " + AFldName + "='" + ANewName +
                "' WHERE " + AFldName + "='" + AOldName + "'";

            if (ALedgerNumber >= 0)
            {
                QuerySql += (" AND a_ledger_number_i=" + ALedgerNumber);
            }

            DBAccess.GDBAccessObj.ExecuteNonQuery(QuerySql, ATransaction);
        }

        /// <summary>
        /// Use this new account code instead of that old one.
        /// THIS RENAMES THE FIELD IN LOTS OF PLACES!
        /// </summary>
        /// <param name="AOldCode"></param>
        /// <param name="ANewCode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="VerificationResults"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool RenameAccountCode(String AOldCode,
            String ANewCode,
            Int32 ALedgerNumber,
            out TVerificationResultCollection VerificationResults)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            Boolean RenameComplete = false;
            String VerificationContext = "Rename Account Code";
            String AttemptedOperation = "";

            VerificationResults = new TVerificationResultCollection();
            try
            {
                //
                // First check whether this new code is available for use!
                //
                AAccountTable TempAccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, ANewCode, Transaction);

                if (TempAccountTbl.Rows.Count > 0)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Target name is already present",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                TempAccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AOldCode, Transaction);

                if (TempAccountTbl.Rows.Count != 1)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Existing name not accessible",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                AAccountRow PrevAccountRow = TempAccountTbl[0];
                AAccountRow NewAccountRow = TempAccountTbl.NewRowTyped();
                DataUtilities.CopyAllColumnValues(PrevAccountRow, NewAccountRow);
                NewAccountRow.AccountCode = ANewCode;
                TempAccountTbl.Rows.Add(NewAccountRow);

                if (!AAccountAccess.SubmitChanges(TempAccountTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                TempAccountTbl.AcceptChanges();

                UpdateAccountField("a_ledger", "a_creditor_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_debtor_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_fa_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_ilt_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_po_accrual_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_profit_loss_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_purchase_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_sales_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_so_accrual_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_stock_adj_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_stock_gl_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_tax_input_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_tax_output_gl_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_cost_of_sales_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger",
                    "a_forex_gains_losses_account_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_ret_earnings_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ledger", "a_stock_accrual_gl_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction",
                    "a_primary_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

/*
 *              UpdateAccountField ("a_this_year_old_transaction","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_this_year_old_transaction","a_primary_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_previous_year_transaction","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 *              UpdateAccountField ("a_previous_year_transaction","a_primary_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
 */
                UpdateAccountField("a_fees_receivable", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_receivable", "a_dr_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_payable", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_fees_payable", "a_dr_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_balancing_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_credit_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_transaction_type",
                    "a_debit_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                AAnalysisAttributeTable TempAnalAttrTbl = AAnalysisAttributeAccess.LoadViaAAccount(ALedgerNumber, AOldCode, Transaction);

                for (Int32 Idx = TempAnalAttrTbl.Rows.Count - 1; Idx >= 0; Idx--)
                {
                    AAnalysisAttributeRow OldAnalAttribRow = (AAnalysisAttributeRow)TempAnalAttrTbl.Rows[Idx];
                    // "a_analysis_attribute"  is the referrent in foreign keys, so I can't just go changing it - I need to make a copy?
                    AAnalysisAttributeRow NewAnalAttribRow = TempAnalAttrTbl.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAnalAttribRow, NewAnalAttribRow);
                    NewAnalAttribRow.AccountCode = ANewCode;
                    TempAnalAttrTbl.Rows.Add(NewAnalAttribRow);

                    if (!AAnalysisAttributeAccess.SubmitChanges(TempAnalAttrTbl, Transaction, out VerificationResults))
                    {
                        return false;
                    }

                    TempAnalAttrTbl.AcceptChanges();

                    UpdateAccountField("a_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                    UpdateAccountField("a_recurring_trans_anal_attrib",
                        "a_account_code_c",
                        AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                    UpdateAccountField("a_ap_anal_attrib", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                    OldAnalAttribRow.Delete();

                    if (!AAnalysisAttributeAccess.SubmitChanges(TempAnalAttrTbl, Transaction, out VerificationResults))
                    {
                        return false;
                    }
                }

                UpdateAccountField("a_suspense_account",
                    "a_suspense_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_motivation_detail", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_transaction",
                    "a_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_gift_batch", "a_bank_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_gift_batch",
                    "a_bank_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ap_document_detail", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ap_document", "a_ap_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ap_payment", "a_bank_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_ep_payment", "a_bank_account_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                UpdateAccountField("a_ap_supplier", "a_default_ap_account_c", AOldCode, ANewCode, -1, Transaction, ref AttemptedOperation); // There's no Ledger associated with this field.

                UpdateAccountField("a_budget", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_general_ledger_master",
                    "a_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy_detail",
                    "a_reporting_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy_detail",
                    "a_account_code_to_report_to_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_account_hierarchy",
                    "a_root_account_code_c",
                    AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_account_property", "a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
//              UpdateAccountField("a_fin_statement_group","a_account_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);

                PrevAccountRow.Delete();

                if (!AAccountAccess.SubmitChanges(TempAccountTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                RenameComplete = true;
            }
            //
            // There's no "catch" - if any of the calls above fails (with an SQL problem),
            // the server task will fail, and cause a descriptive exception on the client.
            // (And the VerificationResults might also contain a useful string because of "finally" below.)
            //
            finally
            {
                if (RenameComplete)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    if (AttemptedOperation != "")
                    {
                        VerificationResults.Add(new TVerificationResult(VerificationContext, "Problem " + AttemptedOperation,
                                TResultSeverity.Resv_Critical));
                    }

                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return RenameComplete;
        }

        private static bool CostCentreHasChildren(Int32 ALedgerNumber, string ACostCentreCode, TDBTransaction Transaction)
        {
            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_cost_centre WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_cost_centre_to_report_to_c = '" + ACostCentreCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);

            return Convert.ToInt32(SqlResult) > 0;
        }

        private static bool CostCentreCodeHasBeenUsed(Int32 ALedgerNumber, string ACostCentreCode, TDBTransaction Transaction)
        {
            // TODO: enhance sql statement to check for more references to a_cost_centre
            // TODO:? This method is *almost exactly like* the equivalent AccountCode function.
            //        With an extra parameter the two could be combined.

            String QuerySql =
                "SELECT COUNT (*) FROM PUB_a_transaction WHERE " +
                "a_ledger_number_i=" + ALedgerNumber + " AND " +
                "a_cost_centre_code_c = '" + ACostCentreCode + "';";
            object SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
            bool IsInUse = (Convert.ToInt32(SqlResult) > 0);

            if (!IsInUse)
            {
                QuerySql =
                    "SELECT COUNT (*) FROM PUB_a_ap_document_detail WHERE " +
                    "a_ledger_number_i=" + ALedgerNumber + " AND " +
                    "a_cost_centre_code_c = '" + ACostCentreCode + "';";
                SqlResult = DBAccess.GDBAccessObj.ExecuteScalar(QuerySql, Transaction);
                IsInUse = (Convert.ToInt32(SqlResult) > 0);
            }

            return IsInUse;
        }

        /// <summary>I can add child accounts to this account if it's a summary account,
        ///          or if there have never been transactions posted to it,
        ///          or if it's linked to a partner.
        ///
        ///          (If children are added to this account, it will be promoted to a summary account.)
        ///
        ///          I can delete this account if it has no transactions posted as above,
        ///          AND it has no children.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="ACanBeParent"></param>
        /// <param name="ACanDelete"></param>
        /// <param name="AMsg"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean GetCostCentreAttributes(Int32 ALedgerNumber,
            String ACostCentreCode,
            out bool ACanBeParent,
            out bool ACanDelete,
            out String AMsg)
        {
            ACanBeParent = true;
            ACanDelete = true;
            AMsg = "";
            bool DbSuccess = true;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            ACostCentreTable TempTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCentreCode, Transaction);

            if (TempTbl.Rows.Count < 1)  // This shouldn't happen..
            {
                DbSuccess = false;
                AMsg = Catalog.GetString("Not Found!");
            }
            else
            {
                bool IsParent = CostCentreHasChildren(ALedgerNumber, ACostCentreCode, Transaction);
                ACostCentreRow AccountRow = TempTbl[0];
                ACanBeParent = IsParent; // If it's a summary account, it's OK (This shouldn't happen either, because the client shouldn't ask me!)

                if (IsParent)
                {
                    ACanDelete = false;
                    AMsg = Catalog.GetString("Cost Centre has children.");
                }

                if (!ACanBeParent || ACanDelete)
                {
                    bool IsInUse = CostCentreCodeHasBeenUsed(ALedgerNumber, ACostCentreCode, Transaction);

                    if (IsInUse)
                    {
                        ACanBeParent = false;    // For posting accounts, I can still add children (and change the account to summary) if there's nothing posted to it yet.
                        ACanDelete = false;      // Once it has transactions posted, I can't delete it, ever.
                        AMsg = Catalog.GetString("Cost Centre is referenced in transactions.");
                    }
                }

                if (ACanBeParent || ACanDelete)     // I need to check whether the Cost Centre has been linked to a partner (but never used).
                {                                   // If it has, the link must be deleted first.
                    AValidLedgerNumberTable VlnTbl = AValidLedgerNumberAccess.LoadViaACostCentre(ALedgerNumber, ACostCentreCode, Transaction);

                    if (VlnTbl.Rows.Count > 0)      // There's a link to a partner!
                    {
                        ACanBeParent = false;
                        ACanDelete = false;
                        AMsg = String.Format(Catalog.GetString("Cost Centre is linked to partner {0}."), VlnTbl[0].PartnerKey);
                    }
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return DbSuccess;
        }

        /// <summary>
        /// Use this new Cost Centre code instead of that old one.
        /// THIS RENAMES THE FIELD IN LOTS OF PLACES!
        /// </summary>
        /// <param name="AOldCode"></param>
        /// <param name="ANewCode"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="VerificationResults"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool RenameCostCentreCode(String AOldCode,
            String ANewCode,
            Int32 ALedgerNumber,
            out TVerificationResultCollection VerificationResults)
        {
            bool RenameComplete = false;
            String VerificationContext = "Rename Cost Centre Code";
            String AttemptedOperation = "";

            VerificationResults = new TVerificationResultCollection();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                //
                // First check whether this new code is available for use!
                // (Check that the old name exists, and the new name doesn't!)
                //
                ACostCentreTable TempTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ANewCode, Transaction);

                if (TempTbl.Rows.Count > 0)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Target name is already present",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                TempTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, AOldCode, Transaction);

                if (TempTbl.Rows.Count != 1)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext, "Existing name not accessible",
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                ACostCentreRow PrevRow = TempTbl[0];

                if (PrevRow.SystemCostCentreFlag)
                {
                    VerificationResults.Add(new TVerificationResult(VerificationContext,
                            String.Format("Cannot rename System Cost Centre {0}.", AOldCode),
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                // I can't just rename this,
                // because lots of tables rely on this entry and I'll break their foreign constraints.
                // I need to create a new row, point everyone to that, then delete the current row.
                //
                ACostCentreRow NewRow = TempTbl.NewRowTyped();
                DataUtilities.CopyAllColumnValues(PrevRow, NewRow);
                NewRow.CostCentreCode = ANewCode;
                TempTbl.Rows.Add(NewRow);

                if (!ACostCentreAccess.SubmitChanges(TempTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                TempTbl.AcceptChanges();

                UpdateAccountField("a_cost_centre",
                    "a_cost_centre_to_report_to_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_transaction", "a_cost_centre_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_transaction",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_valid_ledger_number",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_motivation_detail",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_fees_receivable",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_fees_payable", "a_cost_centre_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_gift_batch", "a_bank_cost_centre_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_gift_detail", "a_cost_centre_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_recurring_gift_batch",
                    "a_bank_cost_centre_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_ap_document_detail",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);
                UpdateAccountField("a_processed_fee", "a_cost_centre_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_budget", "a_cost_centre_code_c", AOldCode, ANewCode, ALedgerNumber, Transaction, ref AttemptedOperation);
                UpdateAccountField("a_general_ledger_master",
                    "a_cost_centre_code_c",
                    AOldCode,
                    ANewCode,
                    ALedgerNumber,
                    Transaction,
                    ref AttemptedOperation);


/*
 * These tables were previously checked in the 4GL, but they don't exist in Open Petra:
 *
 * "a_previous_year_transaction"
 * "a_ich_stewardship"
 */

                PrevRow.Delete();

                if (!ACostCentreAccess.SubmitChanges(TempTbl, Transaction, out VerificationResults))
                {
                    return false;
                }

                RenameComplete = true;
            } // try
              //
              // There's no "catch" - if any of the calls above fails (with an SQL problem),
              // the server task will fail, and cause a descriptive exception on the client.
              // (And the VerificationResults might also contain a useful string because of "finally" below.)
              //
            finally
            {
                if (RenameComplete)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    if (AttemptedOperation != "")
                    {
                        VerificationResults.Add(new TVerificationResult(VerificationContext, "Problem " + AttemptedOperation,
                                TResultSeverity.Resv_Critical));
                    }

                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return RenameComplete;
        } // RenameCostCentreCode
    } // TGLSetupWebConnector
} // namespace