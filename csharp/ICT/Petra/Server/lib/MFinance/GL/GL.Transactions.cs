//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash
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
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MCommon.Data.Cascading;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance GL screens
    ///</summary>
    public class TGLTransactionWebConnector
    {
        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            return TGLPosting.CreateABatch(ALedgerNumber, true);
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACommitTransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber, Boolean ACommitTransaction)
        {
            return TGLPosting.CreateABatch(ALedgerNumber, ACommitTransaction);
        }

        /// <summary>
        /// create a new recurring batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS CreateARecurringBatch(Int32 ALedgerNumber)
        {
            return TGLPosting.CreateARecurringBatch(ALedgerNumber);
        }

        /// <summary>
        /// loads a list of batches for the given ledger;
        /// also get the ledger for the base currency etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFilterBatchStatus"></param>
        /// <param name="AYear">if -1, the year will be ignored</param>
        /// <param name="APeriod">if AYear is -1 or period is -1, the period will be ignored.
        /// if APeriod is 0 and the current year is selected, then the current and the forwarding periods are used</param>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatch(Int32 ALedgerNumber, TFinanceBatchFilterEnum AFilterBatchStatus, int AYear, int APeriod)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            string FilterByPeriod = string.Empty;

            if (AYear != -1)
            {
                FilterByPeriod = String.Format(" AND PUB_{0}.{1} = {2}",
                    ABatchTable.GetTableDBName(),
                    ABatchTable.GetBatchYearDBName(),
                    AYear);

                if ((APeriod == 0) && (AYear == MainDS.ALedger[0].CurrentFinancialYear))
                {
                    FilterByPeriod += String.Format(" AND PUB_{0}.{1} >= {2}",
                        ABatchTable.GetTableDBName(),
                        ABatchTable.GetBatchPeriodDBName(),
                        MainDS.ALedger[0].CurrentPeriod);
                }
                else if (APeriod != -1)
                {
                    FilterByPeriod += String.Format(" AND PUB_{0}.{1} = {2}",
                        ABatchTable.GetTableDBName(),
                        ABatchTable.GetBatchPeriodDBName(),
                        APeriod);
                }
            }

            string SelectClause =
                String.Format("SELECT * FROM PUB_{0} WHERE {1} = {2}",
                    ABatchTable.GetTableDBName(),
                    ABatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber);

            string FilterByBatchStatus = string.Empty;

            if (AFilterBatchStatus == TFinanceBatchFilterEnum.fbfAll)
            {
                // FilterByBatchStatus is empty
            }
            else if ((AFilterBatchStatus & TFinanceBatchFilterEnum.fbfEditing) != 0)
            {
                FilterByBatchStatus =
                    string.Format(" AND {0} = '{1}'",
                        ABatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED);
            }

            DBAccess.GDBAccessObj.Select(MainDS, SelectClause + FilterByBatchStatus + FilterByPeriod,
                MainDS.ABatch.TableName, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified batch.
        /// this method is called after a batch has been posted.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified batch and its journals and transactions and attributes.
        /// this method is called after a batch has been posted.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified batch and its journals and transactions.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAJournalATransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified batch and its journals.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAJournal(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified batch and its journals.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournal(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// Loads a list of journals for the given ledger, batch and journal
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournalATransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// load the specified journal with its transactions.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournalATransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
            ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch and journal
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// Loads a list of transactions for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransactionForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch and journal with analysis attributes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransactionATransAnalAttrib(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            string strAnalAttr = string.Empty;

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            ATransAnalAttribAccess.LoadViaAJournal(MainDS,
                ALedgerNumber,
                ABatchNumber,
                AJournalNumber,
                Transaction);

            foreach (GLBatchTDSATransactionRow transRow in MainDS.ATransaction.Rows)
            {
                MainDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    transRow.TransactionNumber);

                foreach (DataRowView rv in MainDS.ATransAnalAttrib.DefaultView)
                {
                    ATransAnalAttribRow Row = (ATransAnalAttribRow)rv.Row;

                    if (strAnalAttr.Length > 0)
                    {
                        strAnalAttr += ", ";
                    }

                    strAnalAttr += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                }

                transRow.AnalysisAttributes = strAnalAttr;

                //reset the attributes string
                strAnalAttr = string.Empty;
            }

            MainDS.ATransAnalAttrib.DefaultView.RowFilter = string.Empty;

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given transaction (identified by ledger,batch,journal and transaction number)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransAnalAttrib(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransAnalAttribAccess.LoadViaATransaction(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given Batch Journal (identified by ledger,batch, journal)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransAnalAttribForJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransAnalAttribAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given Batch (identified by ledger,batch)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransAnalAttribForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads some necessary analysis attributes tables for the given ledger number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns>GLSetupTDS</returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadAAnalysisAttributes(Int32 ALedgerNumber)
        {
            GLSetupTDS CacheDS = new GLSetupTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AAnalysisTypeAccess.LoadAll(CacheDS, Transaction);
            AFreeformAnalysisAccess.LoadViaALedger(CacheDS, ALedgerNumber, Transaction);
            AAnalysisAttributeAccess.LoadViaALedger(CacheDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return CacheDS;
        }

        /// <summary>
        /// loads a list of batches for the given ledger;
        /// also get the ledger for the base currency etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFilterBatchStatus"></param>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringBatch(Int32 ALedgerNumber, TFinanceBatchFilterEnum AFilterBatchStatus)
        {
            Boolean NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GLBatchTDS MainDS = new GLBatchTDS();

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            string SelectClause =
                String.Format("SELECT * FROM PUB_{0} WHERE {1} = {2}",
                    ARecurringBatchTable.GetTableDBName(),
                    ARecurringBatchTable.GetLedgerNumberDBName(),
                    ALedgerNumber);

            string FilterByBatchStatus = string.Empty;

            if (AFilterBatchStatus == TFinanceBatchFilterEnum.fbfAll)
            {
                // FilterByBatchStatus is empty
            }
            else if ((AFilterBatchStatus & TFinanceBatchFilterEnum.fbfEditing) != 0)
            {
                FilterByBatchStatus =
                    string.Format(" AND {0} = '{1}'",
                        ARecurringBatchTable.GetBatchStatusDBName(),
                        MFinanceConstants.BATCH_UNPOSTED);
            }

            DBAccess.GDBAccessObj.Select(MainDS, SelectClause + FilterByBatchStatus,
                MainDS.ARecurringBatch.TableName, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// load the specified recurring batch and its journals and transactions and attributes.
        /// this method is called after a batch has been posted.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringBatchAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            Boolean NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);


            GLBatchTDS MainDS = new GLBatchTDS();

            ARecurringBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            ARecurringJournalAccess.LoadViaARecurringBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            ARecurringTransactionTable TransactionTable = new ARecurringTransactionTable();
            ARecurringTransactionRow TemplateTransactionRow = TransactionTable.NewRowTyped(false);
            TemplateTransactionRow.LedgerNumber = ALedgerNumber;
            TemplateTransactionRow.BatchNumber = ABatchNumber;
            ARecurringTransactionAccess.LoadUsingTemplate(MainDS, TemplateTransactionRow, Transaction);

            ARecurringTransAnalAttribTable TransAnalAttribTable = new ARecurringTransAnalAttribTable();
            ARecurringTransAnalAttribRow TemplateTransAnalAttribRow = TransAnalAttribTable.NewRowTyped(false);
            TemplateTransAnalAttribRow.LedgerNumber = ALedgerNumber;
            TemplateTransAnalAttribRow.BatchNumber = ABatchNumber;
            ARecurringTransAnalAttribAccess.LoadUsingTemplate(MainDS, TemplateTransAnalAttribRow, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringJournal(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            Boolean NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GLBatchTDS MainDS = new GLBatchTDS();

            ARecurringJournalAccess.LoadViaARecurringBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch and journal with analysis attributes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransactionWithAttributes(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            string strAnalAttr = string.Empty;
            Boolean NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GLBatchTDS MainDS = new GLBatchTDS();

            ARecurringTransactionAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            ARecurringTransAnalAttribAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            foreach (GLBatchTDSARecurringTransactionRow transRow in MainDS.ARecurringTransaction.Rows)
            {
                MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    transRow.TransactionNumber);

                foreach (DataRowView rv in MainDS.ARecurringTransAnalAttrib.DefaultView)
                {
                    ARecurringTransAnalAttribRow Row = (ARecurringTransAnalAttribRow)rv.Row;

                    if (strAnalAttr.Length > 0)
                    {
                        strAnalAttr += ", ";
                    }

                    strAnalAttr += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                }

                transRow.AnalysisAttributes = strAnalAttr;

                //clear the attributes string and table
                strAnalAttr = string.Empty;
            }

            MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = string.Empty;

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of Recurring attributes for the given transaction (identified by ledger,batch,journal and transaction number)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransAnalAttrib(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            Int32 ATransactionNumber)
        {
            Boolean NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GLBatchTDS MainDS = new GLBatchTDS();

            ARecurringTransAnalAttribAccess.LoadViaARecurringTransaction(MainDS,
                ALedgerNumber,
                ABatchNumber,
                AJournalNumber,
                ATransactionNumber,
                Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given recurring Batch Journal (identified by ledger,batch, journal)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransAnalAttribForJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ARecurringTransAnalAttribAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// this will store all new and modified batches, journals, transactions
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveGLBatchTDS(ref GLBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            bool batchTableInDataSet = (AInspectDS.ABatch != null);
            bool journalTableInDataSet = (AInspectDS.AJournal != null);
            bool transTableInDataSet = (AInspectDS.ATransaction != null);
            bool attrTableInDataSet = (AInspectDS.ATransAnalAttrib != null);
            bool recurrBatchTableInDataSet = (AInspectDS.ARecurringBatch != null);
            bool recurrJournalTableInDataSet = (AInspectDS.ARecurringJournal != null);
            bool recurrTransTableInDataSet = (AInspectDS.ARecurringTransaction != null);

            // calculate debit and credit sums for journal and batch? but careful: we only have the changed parts!
            // no, we calculate the debit and credit sums before the posting, with GLRoutines.UpdateTotalsOfBatch

            // check added and modified and deleted rows: are they related to a posted or cancelled batch? we must not save adjusted posted batches!
            List <Int32>BatchNumbersInvolved = new List <int>();
            Int32 LedgerNumber = -1;

            //Check if saving recurring tables
            if (recurrBatchTableInDataSet
                || recurrJournalTableInDataSet
                || recurrTransTableInDataSet)
            {
                return SaveRecurringGLBatchTDS(ref AInspectDS, out AVerificationResult);
            }

            if (batchTableInDataSet)
            {
                LedgerNumber = ((ABatchRow)AInspectDS.ABatch.Rows[0]).LedgerNumber;

                bool NewTransaction = false;

                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                foreach (ABatchRow batch in AInspectDS.ABatch.Rows)
                {
                    if (batch.RowState != DataRowState.Added)
                    {
                        Int32 BatchNumber;

                        try
                        {
                            BatchNumber = batch.BatchNumber;
                        }
                        catch (Exception)
                        {
                            // for deleted batches
                            BatchNumber = (Int32)batch[ABatchTable.ColumnBatchNumberId, DataRowVersion.Original];
                        }

                        if (!BatchNumbersInvolved.Contains(BatchNumber))
                        {
                            BatchNumbersInvolved.Add(BatchNumber);
                        }
                    }

                    int PeriodNumber, YearNr;

                    if (TFinancialYear.IsValidPostingPeriod(LedgerNumber,
                            batch.DateEffective,
                            out PeriodNumber,
                            out YearNr,
                            Transaction))
                    {
                        batch.BatchYear = YearNr;
                        batch.BatchPeriod = PeriodNumber;
                    }
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            if (journalTableInDataSet)
            {
                if (LedgerNumber == -1)
                {
                    LedgerNumber = ((AJournalRow)AInspectDS.AJournal.Rows[0]).LedgerNumber;
                }

                foreach (GLBatchTDSAJournalRow journal in AInspectDS.AJournal.Rows)
                {
                    Int32 BatchNumber;

                    try
                    {
                        BatchNumber = journal.BatchNumber;
                        LedgerNumber = journal.LedgerNumber;
                    }
                    catch (Exception)
                    {
                        // for deleted journals
                        BatchNumber = (Int32)journal[AJournalTable.ColumnBatchNumberId, DataRowVersion.Original];
                        LedgerNumber = (Int32)journal[AJournalTable.ColumnLedgerNumberId, DataRowVersion.Original];
                    }

                    if (!BatchNumbersInvolved.Contains(BatchNumber))
                    {
                        BatchNumbersInvolved.Add(BatchNumber);
                    }
                }
            }

            if (transTableInDataSet)
            {
                if (LedgerNumber == -1)
                {
                    LedgerNumber = ((ATransactionRow)AInspectDS.ATransaction.Rows[0]).LedgerNumber;
                }

                GLPostingTDS TestAccountsAndCostCentres = new GLPostingTDS();

                foreach (ATransactionRow transaction in AInspectDS.ATransaction.Rows)
                {
                    Int32 BatchNumber;

                    try
                    {
                        BatchNumber = transaction.BatchNumber;
                        LedgerNumber = transaction.LedgerNumber;

                        if (TestAccountsAndCostCentres.AAccount.Count == 0)
                        {
                            AAccountAccess.LoadViaALedger(TestAccountsAndCostCentres, LedgerNumber, null);
                            ACostCentreAccess.LoadViaALedger(TestAccountsAndCostCentres, LedgerNumber, null);
                        }

                        // TODO could check for active accounts and cost centres?

                        // check for valid accounts and cost centres
                        if (TestAccountsAndCostCentres.AAccount.Rows.Find(new object[] { LedgerNumber, transaction.AccountCode }) == null)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    Catalog.GetString("Cannot save transaction"),
                                    String.Format(Catalog.GetString("Invalid account code {0} in batch {1}, journal {2}, transaction {3}"),
                                        transaction.AccountCode,
                                        transaction.BatchNumber,
                                        transaction.JournalNumber,
                                        transaction.TransactionNumber),
                                    TResultSeverity.Resv_Critical));
                        }

                        if (TestAccountsAndCostCentres.ACostCentre.Rows.Find(new object[] { LedgerNumber, transaction.CostCentreCode }) == null)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    Catalog.GetString("Cannot save transaction"),
                                    String.Format(Catalog.GetString("Invalid cost centre code {0} in batch {1}, journal {2}, transaction {3}"),
                                        transaction.CostCentreCode,
                                        transaction.BatchNumber,
                                        transaction.JournalNumber,
                                        transaction.TransactionNumber),
                                    TResultSeverity.Resv_Critical));
                        }
                    }
                    catch (Exception)
                    {
                        // for deleted transactions
                        BatchNumber = (Int32)transaction[ATransactionTable.ColumnBatchNumberId, DataRowVersion.Original];
                        LedgerNumber = (Int32)transaction[ATransactionTable.ColumnLedgerNumberId, DataRowVersion.Original];
                    }

                    if (!BatchNumbersInvolved.Contains(BatchNumber))
                    {
                        BatchNumbersInvolved.Add(BatchNumber);
                    }
                }
            }

            if (attrTableInDataSet)
            {
                foreach (ATransAnalAttribRow transAnalAttrib in AInspectDS.ATransAnalAttrib.Rows)
                {
                    Int32 BatchNumber;

                    BatchNumber = transAnalAttrib.BatchNumber;

                    if (!BatchNumbersInvolved.Contains(BatchNumber))
                    {
                        BatchNumbersInvolved.Add(BatchNumber);
                    }
                }
            }

            // load previously stored batches and check for posted status
            if (BatchNumbersInvolved.Count == 0)
            {
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Saving Batch"),
                        Catalog.GetString("Cannot save an empty Batch!"),
                        TResultSeverity.Resv_Critical));
            }
            else
            {
                string ListOfBatchNumbers = string.Empty;

                foreach (Int32 BatchNumber in BatchNumbersInvolved)
                {
                    ListOfBatchNumbers = StringHelper.AddCSV(ListOfBatchNumbers, BatchNumber.ToString());
                }

                string SQLStatement = "SELECT * " +
                                      " FROM PUB_" + ABatchTable.GetTableDBName() + " WHERE " + ABatchTable.GetLedgerNumberDBName() + " = " +
                                      LedgerNumber.ToString() +
                                      " AND " + ABatchTable.GetBatchNumberDBName() + " IN (" + ListOfBatchNumbers + ")";

                GLBatchTDS BatchDS = new GLBatchTDS();

                bool tempTransaction; // If I create a transaction here, then I need to rollback when I'm done.
                TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out tempTransaction);

                try
                {
                    DBAccess.GDBAccessObj.Select(BatchDS, SQLStatement, BatchDS.ABatch.TableName, Transaction);
                }
                finally
                {
                    if (tempTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }

                foreach (ABatchRow batch in BatchDS.ABatch.Rows)
                {
                    if ((batch.BatchStatus == MFinanceConstants.BATCH_POSTED)
                        || (batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED))
                    {
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Saving Batch"),
                                String.Format(Catalog.GetString("Cannot modify Batch {0} because it is {1}"),
                                    batch.BatchNumber, batch.BatchStatus),
                                TResultSeverity.Resv_Critical));
                    }
                }
            }

            if (AVerificationResult.HasCriticalErrors)
            {
                return TSubmitChangesResult.scrError;
            }

            TSubmitChangesResult SubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

            //Process transactions and their analysis attributes
            if (transTableInDataSet)
            {
                AInspectDS.ATransaction.AcceptChanges();
            }

            if (attrTableInDataSet)
            {
                AInspectDS.ATransAnalAttrib.AcceptChanges();
            }

            if ((SubmissionResult == TSubmitChangesResult.scrOK)
                && (transTableInDataSet) && (AInspectDS.ATransaction.Rows.Count > 0))
            {
                ATransactionRow tranR = (ATransactionRow)AInspectDS.ATransaction.Rows[0];

                Int32 currentLedger = tranR.LedgerNumber;
                Int32 currentBatch = tranR.BatchNumber;
                Int32 currentJournal = tranR.JournalNumber;
                Int32 transToDelete = 0;

                try
                {
                    //Check if a transaction has been deleted
                    //Accept the deletion of the single details row
                    //AInspectDS.ATransaction.AcceptChanges();

                    if (!attrTableInDataSet)
                    {
                        AInspectDS.Tables.Add(new ATransAnalAttribTable("ATransAnalAttrib"));
                        AInspectDS.Merge(LoadATransAnalAttribForJournal(currentLedger, currentBatch, currentJournal));
                        attrTableInDataSet = true;
                    }

                    //Check if any records have been marked for deletion
                    DataRow[] foundTransactionForDeletion = AInspectDS.ATransaction.Select(String.Format("{0} = '{1}'",
                            ATransactionTable.GetSubTypeDBName(),
                            MFinanceConstants.MARKED_FOR_DELETION));

                    if (foundTransactionForDeletion.Length > 0)
                    {
                        ATransactionRow transRowClient = null;

                        for (int i = 0; i < foundTransactionForDeletion.Length; i++)
                        {
                            transRowClient = (ATransactionRow)foundTransactionForDeletion[i];

                            transToDelete = transRowClient.TransactionNumber;
                            TLogging.Log(String.Format("Transaction to Delete: {0} from Journal: {1} in Batch: {2}",
                                    transToDelete,
                                    currentJournal,
                                    currentBatch));

                            transRowClient.Delete();
                        }

                        //save changes
                        SubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

                        //Accept the deletion of the single detail row
                        AInspectDS.ATransAnalAttrib.AcceptChanges();
                        AInspectDS.ATransaction.AcceptChanges();
                    }

                    //Check that all analysis attributes exist
                    CheckTransAnalysisAttributes(ref AInspectDS,
                        currentLedger,
                        currentBatch,
                        currentJournal,
                        ref SubmissionResult,
                        ref AVerificationResult);
                }
                catch (Exception)
                {
                    TLogging.Log(String.Format("Error trying to delete transaction: {0} in Journal: {1}, Batch: {2}",
                            transToDelete,
                            currentJournal,
                            currentBatch
                            ));
                }


                // Problem: unchanged rows will not arrive here? check after committing, and update the gift batch again
                // TODO: calculate hash of saved batch or batch of saved gift
            }

            return SubmissionResult;
        }

        private static void UpdateTotalsOfBatchesAndJournals(ref GLBatchTDS AInspectDS,
            Int32 ALedgerNumber,
            int[] ABatchNumbers,
            bool ABatchTableInDataSet,
            bool AJournalTableInDataSet)
        {
            Int32 currJournalNumber = -1;

            ABatchRow currentBatchRow = null;

            foreach (int currBatchNumber in ABatchNumbers)
            {
                if (!ABatchTableInDataSet && !AJournalTableInDataSet)
                {
                    if (AInspectDS.ATransaction.Count > 0)
                    {
                        AInspectDS.ATransaction.DefaultView.RowFilter = String.Format("{0} = {1}",
                            ATransactionTable.GetBatchNumberDBName(),
                            currBatchNumber);

                        List <int>journalNums = new List <int>();

                        foreach (DataRowView dr in AInspectDS.ATransaction.DefaultView)
                        {
                            ATransactionRow tr = (ATransactionRow)dr.Row;

                            currJournalNumber = tr.JournalNumber;

                            if (!journalNums.Contains(currJournalNumber))
                            {
                                journalNums.Add(currJournalNumber);
                            }
                        }

                        if (journalNums.Count == 1)
                        {
                            AInspectDS.Merge(LoadABatch(ALedgerNumber, currBatchNumber), true);
                            AInspectDS.Merge(LoadAJournalATransaction(ALedgerNumber, currBatchNumber, currJournalNumber), true);
                        }
                        else
                        {
                            //Multiple journals
                            AInspectDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, currBatchNumber), true);
                        }
                    }
                    else
                    {
                        AInspectDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, currBatchNumber), true);
                    }
                }
                else if (!ABatchTableInDataSet)
                {
                    AInspectDS.AJournal.DefaultView.RowFilter = String.Format("{0} = {1}",
                        AJournalTable.GetBatchNumberDBName(),
                        currBatchNumber);

                    if (AInspectDS.AJournal.DefaultView.Count == 1)
                    {
                        currJournalNumber = ((AJournalRow)AInspectDS.AJournal.DefaultView[0].Row).JournalNumber;
                        AInspectDS.Merge(LoadABatch(ALedgerNumber, currBatchNumber), true);
                        AInspectDS.Merge(LoadAJournalATransaction(ALedgerNumber, currBatchNumber, currJournalNumber), true);
                    }
                    else
                    {
                        AInspectDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, currBatchNumber), true);
                    }
                }
                else if (!AJournalTableInDataSet)
                {
                    if (AInspectDS.ATransaction.Count > 0)
                    {
                        AInspectDS.ATransaction.DefaultView.RowFilter = String.Format("{0} = {1}",
                            ATransactionTable.GetBatchNumberDBName(),
                            currBatchNumber);

                        List <int>journalNums = new List <int>();

                        foreach (DataRowView dr in AInspectDS.ATransaction.DefaultView)
                        {
                            ATransactionRow tr = (ATransactionRow)dr.Row;

                            currJournalNumber = tr.JournalNumber;

                            if (!journalNums.Contains(currJournalNumber))
                            {
                                journalNums.Add(currJournalNumber);
                            }
                        }

                        if (journalNums.Count == 1)
                        {
                            AInspectDS.Merge(LoadAJournalATransaction(ALedgerNumber, currBatchNumber, currJournalNumber), true);
                        }
                        else
                        {
                            //Multiple journals
                            AInspectDS.Merge(LoadAJournalATransaction(ALedgerNumber, currBatchNumber), true);
                        }
                    }
                    else
                    {
                        AInspectDS.Merge(LoadAJournalATransaction(ALedgerNumber, currBatchNumber), true);
                    }
                }
                else
                {
                    AInspectDS.AJournal.DefaultView.RowFilter = String.Format("{0} = {1}",
                        AJournalTable.GetBatchNumberDBName(),
                        currBatchNumber);

                    if (AInspectDS.AJournal.DefaultView.Count == 1)
                    {
                        currJournalNumber = ((AJournalRow)AInspectDS.AJournal.DefaultView[0].Row).JournalNumber;
                        AInspectDS.Merge(LoadATransaction(ALedgerNumber, currBatchNumber, currJournalNumber), true);
                    }
                    else
                    {
                        //Multiple journals
                        AInspectDS.Merge(LoadATransactionForBatch(ALedgerNumber, currBatchNumber), true);
                    }
                }

                //Read current batch and update totals
                currentBatchRow = (ABatchRow)AInspectDS.ABatch.Rows.Find(new object[] { ALedgerNumber, currBatchNumber });

                UpdateTotalsOfBatch(ref AInspectDS, currentBatchRow);
            }
        }

        private static void CheckTransAnalysisAttributes(ref GLBatchTDS AInspectDS,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            ref TSubmitChangesResult ASubmissionResult,
            ref TVerificationResultCollection AVerificationResult)
        {
            //check if the necessary rows for the given account are there, automatically add/update account
            GLSetupTDS glSetupCacheDS = LoadAAnalysisAttributes(ALedgerNumber);

            //Account Number for AnalysisTable lookup
            int currentTransactionNumber = 0;
            string currentTransAccountCode = String.Empty;

            if (glSetupCacheDS == null)
            {
                return;
            }

            //Reference all transactions in dataset
            DataView allTransView = AInspectDS.ATransaction.DefaultView;
            DataView transAnalAttrView = AInspectDS.ATransAnalAttrib.DefaultView;

            transAnalAttrView.RowFilter = string.Empty;

            allTransView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                AJournalNumber);

            foreach (DataRowView transRowView in allTransView)
            {
                ATransactionRow currentTransactionRow = (ATransactionRow)transRowView.Row;

                currentTransactionNumber = currentTransactionRow.TransactionNumber;
                currentTransAccountCode = currentTransactionRow.AccountCode;

                //If this account code is used need to delete it from TransAnal table.
                //Delete any existing rows with old code
                transAnalAttrView.RowFilter = String.Format("{0} = {1} AND {2} = {3} AND {4} = {5} AND {6} <> '{7}'",
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    currentTransactionNumber,
                    ATransAnalAttribTable.GetAccountCodeDBName(),
                    currentTransAccountCode);

                foreach (DataRowView dv in transAnalAttrView)
                {
                    ATransAnalAttribRow tr = (ATransAnalAttribRow)dv.Row;

                    tr.Delete();
                }

                transAnalAttrView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ATransAnalAttribTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    currentTransactionNumber);

                //Retrieve the analysis attributes for the supplied account
                DataView analAttrView = glSetupCacheDS.AAnalysisAttribute.DefaultView;
                analAttrView.RowFilter = String.Format("{0} = '{1}'",
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    currentTransAccountCode);

                if (analAttrView.Count > 0)
                {
                    for (int i = 0; i < analAttrView.Count; i++)
                    {
                        //Read the Type Code for each attribute row
                        AAnalysisAttributeRow analAttrRow = (AAnalysisAttributeRow)analAttrView[i].Row;
                        string analysisTypeCode = analAttrRow.AnalysisTypeCode;

                        //Check if the attribute type code exists in the Transaction Analysis Attributes table
                        ATransAnalAttribRow transAnalAttrRow =
                            (ATransAnalAttribRow)AInspectDS.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber,
                                                                                                      currentTransactionNumber,
                                                                                                      analysisTypeCode });

                        if (transAnalAttrRow == null)
                        {
                            //Create a new TypeCode for this account
                            ATransAnalAttribRow newRow = AInspectDS.ATransAnalAttrib.NewRowTyped(true);
                            newRow.LedgerNumber = ALedgerNumber;
                            newRow.BatchNumber = ABatchNumber;
                            newRow.JournalNumber = AJournalNumber;
                            newRow.TransactionNumber = currentTransactionNumber;
                            newRow.AnalysisTypeCode = analysisTypeCode;
                            newRow.AccountCode = currentTransAccountCode;

                            AInspectDS.ATransAnalAttrib.Rows.Add(newRow);
                        }
                        else if (transAnalAttrRow.AccountCode != currentTransAccountCode)
                        {
                            //Check account code is correct
                            transAnalAttrRow.AccountCode = currentTransAccountCode;
                        }
                    }
                }

                ASubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
                AInspectDS.ATransAnalAttrib.AcceptChanges();
            }

            transAnalAttrView.RowFilter = string.Empty;
        }

        private static void CheckRecurringTransAnalysisAttributes(ref GLBatchTDS AInspectDS,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            ref TSubmitChangesResult ASubmissionResult,
            ref TVerificationResultCollection AVerificationResult)
        {
            //check if the necessary rows for the given account are there, automatically add/update account
            GLSetupTDS glSetupCacheDS = LoadAAnalysisAttributes(ALedgerNumber);

            //Account Number for AnalysisTable lookup
            int currentTransactionNumber = 0;
            string currentTransAccountCode = String.Empty;

            if (glSetupCacheDS == null)
            {
                return;
            }

            //Reference all transactions in dataset
            DataView allTransView = AInspectDS.ARecurringTransaction.DefaultView;
            DataView transAnalAttrView = AInspectDS.ARecurringTransAnalAttrib.DefaultView;

            transAnalAttrView.RowFilter = string.Empty;

            allTransView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ARecurringTransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ARecurringTransactionTable.GetJournalNumberDBName(),
                AJournalNumber);

            foreach (DataRowView transRowView in allTransView)
            {
                ARecurringTransactionRow currentTransactionRow = (ARecurringTransactionRow)transRowView.Row;

                currentTransactionNumber = currentTransactionRow.TransactionNumber;
                currentTransAccountCode = currentTransactionRow.AccountCode;

                //If this account code is used need to delete it from TransAnal table.
                //Delete any existing rows with old code
                transAnalAttrView.RowFilter = String.Format("{0} = {1} AND {2} = {3} AND {4} = {5} AND {6} <> '{7}'",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    currentTransactionNumber,
                    ARecurringTransAnalAttribTable.GetAccountCodeDBName(),
                    currentTransAccountCode);

                foreach (DataRowView dv in transAnalAttrView)
                {
                    ARecurringTransAnalAttribRow tr = (ARecurringTransAnalAttribRow)dv.Row;

                    tr.Delete();
                }

                transAnalAttrView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                    ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                    AJournalNumber,
                    ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                    currentTransactionNumber);

                //Retrieve the analysis attributes for the supplied account
                DataView analAttrView = glSetupCacheDS.AAnalysisAttribute.DefaultView;
                analAttrView.RowFilter = String.Format("{0} = '{1}'",
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    currentTransAccountCode);

                if (analAttrView.Count > 0)
                {
                    for (int i = 0; i < analAttrView.Count; i++)
                    {
                        //Read the Type Code for each attribute row
                        AAnalysisAttributeRow analAttrRow = (AAnalysisAttributeRow)analAttrView[i].Row;
                        string analysisTypeCode = analAttrRow.AnalysisTypeCode;

                        //Check if the attribute type code exists in the Transaction Analysis Attributes table
                        ARecurringTransAnalAttribRow transAnalAttrRow =
                            (ARecurringTransAnalAttribRow)AInspectDS.ARecurringTransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber,
                                                                                                                        AJournalNumber,
                                                                                                                        currentTransactionNumber,
                                                                                                                        analysisTypeCode });

                        if (transAnalAttrRow == null)
                        {
                            //Create a new TypeCode for this account
                            ARecurringTransAnalAttribRow newRow = AInspectDS.ARecurringTransAnalAttrib.NewRowTyped(true);
                            newRow.LedgerNumber = ALedgerNumber;
                            newRow.BatchNumber = ABatchNumber;
                            newRow.JournalNumber = AJournalNumber;
                            newRow.TransactionNumber = currentTransactionNumber;
                            newRow.AnalysisTypeCode = analysisTypeCode;
                            newRow.AccountCode = currentTransAccountCode;

                            AInspectDS.ARecurringTransAnalAttrib.Rows.Add(newRow);
                        }
                        else if (transAnalAttrRow.AccountCode != currentTransAccountCode)
                        {
                            //Check account code is correct
                            transAnalAttrRow.AccountCode = currentTransAccountCode;
                        }
                    }
                }

                ASubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
                AInspectDS.ARecurringTransAnalAttrib.AcceptChanges();
            }

            transAnalAttrView.RowFilter = string.Empty;
        }

        /// <summary>
        /// this will store all new and modified recurring batches, journals, transactions
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        private static TSubmitChangesResult SaveRecurringGLBatchTDS(ref GLBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            bool NewTransaction = false;
            bool transactionCommitted = false;
            Int32 LedgerNumber;
            Int32 BatchNumber;
            Int32 JournalNumber;
            Int32 TransactionNumber;
            Int32 Counter;

            bool recurrBatchTableInDataSet = (AInspectDS.ARecurringBatch != null);
            bool recurrJournalTableInDataSet = (AInspectDS.ARecurringJournal != null);
            bool recurrTransactionTableInDataSet = (AInspectDS.ARecurringTransaction != null);
            bool recurrTransAnalTableInDataSet = (AInspectDS.ARecurringTransAnalAttrib != null);

            TSubmitChangesResult SubmissionResult = new TSubmitChangesResult();

            //Error handling
            string ErrorContext = "Save a recurring batch";
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            AVerificationResult = new TVerificationResultCollection();

            ARecurringJournalTable JournalTable = new ARecurringJournalTable();
            ARecurringJournalRow TemplateJournalRow = JournalTable.NewRowTyped(false);

            ARecurringTransactionTable TransactionTable = new ARecurringTransactionTable();
            ARecurringTransactionRow TemplateTransactionRow = TransactionTable.NewRowTyped(false);

            ARecurringTransAnalAttribTable TransAnalAttribTable = new ARecurringTransAnalAttribTable();
            ARecurringTransAnalAttribRow TemplateTransAnalAttribRow = TransAnalAttribTable.NewRowTyped(false);

            GLBatchTDS DeletedDS = new GLBatchTDS();
            ARecurringTransAnalAttribTable DeletedTransAnalAttribTable;

            // in this method we also think about deleted batches, journals, transactions where subsequent information
            // had not been loaded yet: a type of cascading delete is being used (real cascading delete currently does
            // not go down the number of levels needed).

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                if (recurrBatchTableInDataSet)
                {
                    foreach (ARecurringBatchRow batch in AInspectDS.ARecurringBatch.Rows)
                    {
                        if (batch.RowState == DataRowState.Deleted)
                        {
                            // need to use this way of retrieving data from deleted rows
                            LedgerNumber = (Int32)batch[ARecurringBatchTable.ColumnLedgerNumberId, DataRowVersion.Original];
                            BatchNumber = (Int32)batch[ARecurringBatchTable.ColumnBatchNumberId, DataRowVersion.Original];

                            // load all depending journals, transactions and attributes and make sure they are also deleted via the dataset
                            TemplateTransAnalAttribRow.LedgerNumber = LedgerNumber;
                            TemplateTransAnalAttribRow.BatchNumber = BatchNumber;
                            DeletedTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                            for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedTransAnalAttribTable.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedTransAnalAttribTable);

                            TemplateTransactionRow.LedgerNumber = LedgerNumber;
                            TemplateTransactionRow.BatchNumber = BatchNumber;
                            ARecurringTransactionAccess.LoadUsingTemplate(DeletedDS, TemplateTransactionRow, Transaction);

                            for (Counter = DeletedDS.ARecurringTransaction.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedDS.ARecurringTransaction.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedDS.ARecurringTransaction);

                            TemplateJournalRow.LedgerNumber = LedgerNumber;
                            TemplateJournalRow.BatchNumber = BatchNumber;
                            ARecurringJournalAccess.LoadUsingTemplate(DeletedDS, TemplateJournalRow, Transaction);

                            for (Counter = DeletedDS.ARecurringJournal.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedDS.ARecurringJournal.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedDS.ARecurringJournal);
                        }
                    }
                }

                if (recurrJournalTableInDataSet)
                {
                    foreach (ARecurringJournalRow journal in AInspectDS.ARecurringJournal.Rows)
                    {
                        if (journal.RowState == DataRowState.Deleted)
                        {
                            // need to use this way of retrieving data from deleted rows
                            LedgerNumber = (Int32)journal[ARecurringJournalTable.ColumnLedgerNumberId, DataRowVersion.Original];
                            BatchNumber = (Int32)journal[ARecurringJournalTable.ColumnBatchNumberId, DataRowVersion.Original];
                            JournalNumber = (Int32)journal[ARecurringJournalTable.ColumnJournalNumberId, DataRowVersion.Original];

                            // load all depending transactions and attributes and make sure they are also deleted via the dataset
                            TemplateTransAnalAttribRow.LedgerNumber = LedgerNumber;
                            TemplateTransAnalAttribRow.BatchNumber = BatchNumber;
                            TemplateTransAnalAttribRow.JournalNumber = JournalNumber;
                            DeletedTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                            for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedTransAnalAttribTable.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedTransAnalAttribTable);

                            TemplateTransactionRow.LedgerNumber = LedgerNumber;
                            TemplateTransactionRow.BatchNumber = BatchNumber;
                            TemplateTransactionRow.JournalNumber = JournalNumber;
                            ARecurringTransactionAccess.LoadUsingTemplate(DeletedDS, TemplateTransactionRow, Transaction);

                            for (Counter = DeletedDS.ARecurringTransaction.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedDS.ARecurringTransaction.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedDS.ARecurringTransaction);
                        }
                    }
                }

                if (recurrTransactionTableInDataSet)
                {
                    foreach (ARecurringTransactionRow transaction in AInspectDS.ARecurringTransaction.Rows)
                    {
                        if (transaction.RowState == DataRowState.Deleted)
                        {
                            // need to use this way of retrieving data from deleted rows
                            LedgerNumber = (Int32)transaction[ARecurringTransactionTable.ColumnLedgerNumberId, DataRowVersion.Original];
                            BatchNumber = (Int32)transaction[ARecurringTransactionTable.ColumnBatchNumberId, DataRowVersion.Original];
                            JournalNumber = (Int32)transaction[ARecurringTransactionTable.ColumnJournalNumberId, DataRowVersion.Original];
                            TransactionNumber = (Int32)transaction[ARecurringTransactionTable.ColumnTransactionNumberId, DataRowVersion.Original];

                            // load all depending transactions and attributes and make sure they are also deleted via the dataset
                            TemplateTransAnalAttribRow.LedgerNumber = LedgerNumber;
                            TemplateTransAnalAttribRow.BatchNumber = BatchNumber;
                            TemplateTransAnalAttribRow.JournalNumber = JournalNumber;
                            TemplateTransAnalAttribRow.TransactionNumber = TransactionNumber;
                            DeletedTransAnalAttribTable = ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                            for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                            {
                                DeletedTransAnalAttribTable.Rows[Counter].Delete();
                            }

                            AInspectDS.Merge(DeletedTransAnalAttribTable);
                        }
                    }
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    transactionCommitted = true;
                }

                // now submit the actual dataset
                SubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

                //Process transactions and their analysis attributes
                if (recurrTransactionTableInDataSet)
                {
                    AInspectDS.ARecurringTransaction.AcceptChanges();
                }

                if (recurrTransAnalTableInDataSet)
                {
                    AInspectDS.ARecurringTransAnalAttrib.AcceptChanges();
                }

                if ((SubmissionResult == TSubmitChangesResult.scrOK)
                    && (recurrTransactionTableInDataSet) && (AInspectDS.ARecurringTransaction.Rows.Count > 0))
                {
                    ARecurringTransactionRow tranR = (ARecurringTransactionRow)AInspectDS.ARecurringTransaction.Rows[0];

                    Int32 currentLedger = tranR.LedgerNumber;
                    Int32 currentBatch = tranR.BatchNumber;
                    Int32 currentJournal = tranR.JournalNumber;
                    Int32 transToDelete = 0;

                    try
                    {
                        //Check if a transaction has been deleted
                        //Accept the deletion of the single details row
                        //AInspectDS.ATransaction.AcceptChanges();

                        if (!recurrTransAnalTableInDataSet)
                        {
                            AInspectDS.Tables.Add(new ARecurringTransAnalAttribTable("ARecurringTransAnalAttrib"));
                            AInspectDS.Merge(LoadARecurringTransAnalAttribForJournal(currentLedger, currentBatch, currentJournal));
                            recurrTransAnalTableInDataSet = true;
                        }

                        //Check if any records have been marked for deletion
                        DataRow[] foundTransactionForDeletion = AInspectDS.ARecurringTransaction.Select(String.Format("{0} = '{1}'",
                                ARecurringTransactionTable.GetSubTypeDBName(),
                                MFinanceConstants.MARKED_FOR_DELETION));

                        if (foundTransactionForDeletion.Length > 0)
                        {
                            ARecurringTransactionRow transRowClient = null;

                            for (int i = 0; i < foundTransactionForDeletion.Length; i++)
                            {
                                transRowClient = (ARecurringTransactionRow)foundTransactionForDeletion[i];

                                transToDelete = transRowClient.TransactionNumber;
                                TLogging.Log(String.Format("Transaction to Delete: {0} from Journal: {1} in Batch: {2}",
                                        transToDelete,
                                        currentJournal,
                                        currentBatch));

                                transRowClient.Delete();
                            }

                            //save changes
                            SubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

                            //Accept the deletion of the single detail row
                            AInspectDS.ARecurringTransAnalAttrib.AcceptChanges();
                            AInspectDS.ARecurringTransaction.AcceptChanges();
                        }

                        //Check that all analysis attributes exist
                        CheckRecurringTransAnalysisAttributes(ref AInspectDS,
                            currentLedger,
                            currentBatch,
                            currentJournal,
                            ref SubmissionResult,
                            ref AVerificationResult);
                    }
                    catch (Exception)
                    {
                        TLogging.Log(String.Format(
                                "Error trying to delete recurring transaction: {0} in recurring Journal: {1}, recurring Batch: {2}",
                                transToDelete,
                                currentJournal,
                                currentBatch
                                ));
                    }


                    // Problem: unchanged rows will not arrive here? check after committing, and update the gift batch again
                    // TODO: calculate hash of saved batch or batch of saved gift
                }
            }
            catch (Exception ex)
            {
                if (NewTransaction && !transactionCommitted)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                ErrorMessage = Catalog.GetString("Unknown error while saving a recurring batch." +
                    Environment.NewLine + Environment.NewLine + ex.ToString());
                ErrorType = TResultSeverity.Resv_Critical;
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
                DBAccess.GDBAccessObj.RollbackTransaction();
                SubmissionResult = TSubmitChangesResult.scrError;
            }

            return SubmissionResult;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            return TGLPosting.PostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// <summary>
        /// return a string that shows the balances of the accounts involved, if the GL Batch was posted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-1")]
        public static List <TVariant>TestPostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            GLPostingTDS MainDS;
            int BatchPeriod = -1;

            DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            bool success = TGLPosting.TestPostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications, out MainDS, ref BatchPeriod);

            // we do not want to actually post the batch
            DBAccess.GDBAccessObj.RollbackTransaction();

            List <TVariant>Result = new List <TVariant>();

            if (success)
            {
                MainDS.AGeneralLedgerMaster.DefaultView.RowFilter = string.Empty;
                MainDS.AAccount.DefaultView.RowFilter = string.Empty;
                MainDS.ACostCentre.DefaultView.RowFilter = string.Empty;
                MainDS.AGeneralLedgerMaster.DefaultView.Sort = AGeneralLedgerMasterTable.GetGlmSequenceDBName();
                MainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
                MainDS.AAccount.DefaultView.Sort = AAccountTable.GetAccountCodeDBName();

                foreach (AGeneralLedgerMasterPeriodRow glmpRow in MainDS.AGeneralLedgerMasterPeriod.Rows)
                {
                    if ((glmpRow.PeriodNumber == BatchPeriod) && (glmpRow.RowState != DataRowState.Unchanged))
                    {
                        AGeneralLedgerMasterRow masterRow =
                            (AGeneralLedgerMasterRow)MainDS.AGeneralLedgerMaster.Rows.Find(glmpRow.GlmSequence);

                        ACostCentreRow ccRow = (ACostCentreRow)MainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, masterRow.CostCentreCode });

                        // only consider the posting cost centres
                        // TODO or consider only the top cost centre?
                        if (ccRow.PostingCostCentreFlag)
                        {
                            AAccountRow accRow =
                                (AAccountRow)
                                MainDS.AAccount.DefaultView[MainDS.AAccount.DefaultView.Find(masterRow.AccountCode)].Row;

                            // only modified accounts have been loaded to the dataset, therefore report on all accounts available
                            if (accRow.PostingStatus)
                            {
                                decimal CurrentValue = 0.0m;

                                if (glmpRow.RowState == DataRowState.Modified)
                                {
                                    CurrentValue = (decimal)glmpRow[AGeneralLedgerMasterPeriodTable.ColumnActualBaseId, DataRowVersion.Original];
                                }

                                decimal DebitCredit = 1.0m;

                                if (accRow.DebitCreditIndicator
                                    && (accRow.AccountType != MFinanceConstants.ACCOUNT_TYPE_ASSET)
                                    && (accRow.AccountType != MFinanceConstants.ACCOUNT_TYPE_EXPENSE))
                                {
                                    DebitCredit = -1.0m;
                                }

                                // only return values, the client compiles the message, with Catalog.GetString
                                TVariant values = new TVariant(accRow.AccountCode);
                                values.Add(new TVariant(accRow.AccountCodeShortDesc), "", false);
                                values.Add(new TVariant(ccRow.CostCentreCode), "", false);
                                values.Add(new TVariant(ccRow.CostCentreName), "", false);
                                values.Add(new TVariant(CurrentValue * DebitCredit), "", false);
                                values.Add(new TVariant(glmpRow.ActualBase * DebitCredit), "", false);

                                Result.Add(values);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// create a GL batch from a recurring GL batch
        /// including journals, transactions and attributes
        /// </summary>
        /// <param name="requestParams">HashTable with many parameters</param>
        /// <param name="AMessages">Output structure for user error messages</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean SubmitRecurringGLBatch(Hashtable requestParams, out TVerificationResultCollection AMessages)
        {
            Boolean NewTransaction = false;
            Boolean success = false;

            AMessages = new TVerificationResultCollection();
            GLBatchTDS GLMainDS = new GLBatchTDS();
            ABatchRow BatchRow;
            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Int32 ABatchNumber = (Int32)requestParams["ABatchNumber"];
            DateTime AEffectiveDate = (DateTime)requestParams["AEffectiveDate"];
            Decimal AExchangeRateToBase;
            int PeriodNumber, YearNr;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                // make sure that recurring GL batch is fully loaded, including journals, transactions and attributes
                GLBatchTDS RGLMainDS = LoadARecurringBatchAndContent(ALedgerNumber, ABatchNumber);

                // Assuming all relevant data is loaded in FMainDS
                foreach (ARecurringBatchRow recBatch  in RGLMainDS.ARecurringBatch.Rows)
                {
                    if ((recBatch.BatchNumber == ABatchNumber) && (recBatch.LedgerNumber == ALedgerNumber))
                    {
                        GLMainDS = CreateABatch(ALedgerNumber);

                        BatchRow = (ABatchRow)GLMainDS.ABatch.Rows[0];
                        BatchRow.DateEffective = AEffectiveDate;
                        BatchRow.BatchDescription = recBatch.BatchDescription;
                        BatchRow.BatchControlTotal = recBatch.BatchControlTotal;
                        BatchRow.BatchRunningTotal = recBatch.BatchRunningTotal;
                        BatchRow.BatchCreditTotal = recBatch.BatchCreditTotal;
                        BatchRow.BatchDebitTotal = recBatch.BatchDebitTotal;

                        if (TFinancialYear.IsValidPostingPeriod(ALedgerNumber,
                                AEffectiveDate,
                                out PeriodNumber,
                                out YearNr,
                                Transaction))
                        {
                            BatchRow.BatchYear = YearNr;
                            BatchRow.BatchPeriod = PeriodNumber;
                        }

                        foreach (ARecurringJournalRow recJournal in RGLMainDS.ARecurringJournal.Rows)
                        {
                            if ((recJournal.BatchNumber == ABatchNumber) && (recJournal.LedgerNumber == ALedgerNumber))
                            {
                                // create the journal from recJournal
                                AJournalRow JournalRow = GLMainDS.AJournal.NewRowTyped();
                                JournalRow.LedgerNumber = BatchRow.LedgerNumber;
                                JournalRow.BatchNumber = BatchRow.BatchNumber;
                                JournalRow.JournalNumber = recJournal.JournalNumber;
                                JournalRow.JournalDescription = recJournal.JournalDescription;
                                JournalRow.SubSystemCode = recJournal.SubSystemCode;
                                JournalRow.TransactionTypeCode = recJournal.TransactionTypeCode;
                                JournalRow.TransactionCurrency = recJournal.TransactionCurrency;
                                JournalRow.JournalCreditTotal = recJournal.JournalCreditTotal;
                                JournalRow.JournalDebitTotal = recJournal.JournalDebitTotal;

                                AExchangeRateToBase = (Decimal)requestParams["AExchangeRateToBaseForJournal" + recJournal.JournalNumber.ToString()];
                                JournalRow.ExchangeRateToBase = AExchangeRateToBase;

                                JournalRow.DateEffective = AEffectiveDate;
                                JournalRow.JournalPeriod = recJournal.JournalPeriod;

                                GLMainDS.AJournal.Rows.Add(JournalRow);

                                if (JournalRow.JournalNumber > BatchRow.LastJournal)
                                {
                                    BatchRow.LastJournal = JournalRow.JournalNumber;
                                }

                                //TODO (not here, but in the client or while posting) Check for expired key ministry (while Posting)

                                foreach (ARecurringTransactionRow recTransaction in RGLMainDS.ARecurringTransaction.Rows)
                                {
                                    if ((recTransaction.JournalNumber == recJournal.JournalNumber)
                                        && (recTransaction.BatchNumber == ABatchNumber)
                                        && (recTransaction.LedgerNumber == ALedgerNumber))
                                    {
                                        ATransactionRow TransactionRow = GLMainDS.ATransaction.NewRowTyped();
                                        TransactionRow.LedgerNumber = JournalRow.LedgerNumber;
                                        TransactionRow.BatchNumber = JournalRow.BatchNumber;
                                        TransactionRow.JournalNumber = JournalRow.JournalNumber;
                                        TransactionRow.TransactionNumber = recTransaction.TransactionNumber;

                                        if (TransactionRow.TransactionNumber > JournalRow.LastTransactionNumber)
                                        {
                                            JournalRow.LastTransactionNumber = TransactionRow.TransactionNumber;
                                        }

                                        TransactionRow.Narrative = recTransaction.Narrative;
                                        TransactionRow.AccountCode = recTransaction.AccountCode;
                                        TransactionRow.CostCentreCode = recTransaction.CostCentreCode;
                                        TransactionRow.TransactionAmount = recTransaction.TransactionAmount;
                                        TransactionRow.AmountInBaseCurrency = recTransaction.TransactionAmount * AExchangeRateToBase;
                                        TransactionRow.TransactionDate = AEffectiveDate;
                                        TransactionRow.DebitCreditIndicator = recTransaction.DebitCreditIndicator;
                                        TransactionRow.HeaderNumber = recTransaction.HeaderNumber;
                                        TransactionRow.DetailNumber = recTransaction.DetailNumber;
                                        TransactionRow.SubType = recTransaction.SubType;
                                        TransactionRow.Reference = recTransaction.Reference;

                                        GLMainDS.ATransaction.Rows.Add(TransactionRow);
                                    }
                                }
                            }
                        }
                    }
                }

                if (ABatchAccess.SubmitChanges(GLMainDS.ABatch, Transaction, out AMessages))
                {
                    if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out AMessages))
                    {
                        if (AJournalAccess.SubmitChanges(GLMainDS.AJournal, Transaction, out AMessages))
                        {
                            if (ATransactionAccess.SubmitChanges(GLMainDS.ATransaction, Transaction, out AMessages))
                            {
                                if (ATransAnalAttribAccess.SubmitChanges(GLMainDS.ATransAnalAttrib, Transaction, out AMessages))
                                {
                                    success = true;
                                }
                            }
                        }
                    }
                }

                if (success)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    GLMainDS.AcceptChanges();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    GLMainDS.RejectChanges();
                }
            }
            catch (Exception ex)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw new Exception("Error in SubmitRecurringGLBatch", ex);
            }
            return success;
        }

        /// <summary>
        /// return the name of the standard costcentre for the given ledger;
        /// this supports up to 4 digit ledgers
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetStandardCostCentre(Int32 ALedgerNumber)
        {
            return TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
        }

        /// <summary>
        /// get daily exchange rate for the given currencies and date;
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static decimal GetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective)
        {
            return TExchangeRateTools.GetDailyExchangeRate(ACurrencyFrom, ACurrencyTo, ADateEffective);
        }

        /// <summary>
        /// get corporate exchange rate for the given currencies and date;
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static decimal GetCorporateExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime AStartDate, DateTime AEndDate)
        {
            return TExchangeRateTools.GetCorporateExchangeRate(ACurrencyFrom, ACurrencyTo, AStartDate, AEndDate);
        }

        /// <summary>
        /// Tell me whether this Batch can be cancelled
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool GLBatchCanBeCancelled(out GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            return TGLPosting.GLBatchCanBeCancelled(out AMainDS, ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches"></param>
        /// <param name="requestParams"></param>
        /// <param name="exportString"></param>
        /// <returns>false if batch does not exist at all</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ExportAllGLBatchData(ref ArrayList batches, Hashtable requestParams, out String exportString)
        {
            TGLExporting exporting = new TGLExporting();

            return exporting.ExportAllGLBatchData(ref batches, requestParams, out exportString);
        }

        /// <summary>
        /// Import GL batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">Big parts of the import file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportGLBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            TGLImporting importing = new TGLImporting();

            return importing.ImportGLBatches(requestParams, importString, out AMessages);
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        private static void UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch)
        {
            string origTransactionFilter = AMainDS.ATransaction.DefaultView.RowFilter;
            string origJournalFilter = AMainDS.AJournal.DefaultView.RowFilter;

            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            AMainDS.AJournal.DefaultView.RowFilter =
                AJournalTable.GetBatchNumberDBName() + " = " + ACurrentBatch.BatchNumber.ToString();

            foreach (DataRowView journalview in AMainDS.AJournal.DefaultView)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                AMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0} = {1} And {2} = {3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    journalrow.BatchNumber.ToString(),
                    ATransactionTable.GetJournalNumberDBName(),
                    journalrow.JournalNumber.ToString());

                UpdateTotalsOfJournal(ref AMainDS, journalrow);

                sumDebits += journalrow.JournalDebitTotal;
                sumCredits += journalrow.JournalCreditTotal;
            }

            ACurrentBatch.BatchDebitTotal = sumDebits;
            ACurrentBatch.BatchCreditTotal = sumCredits;
            ACurrentBatch.BatchRunningTotal = Math.Round(sumDebits - sumCredits, 2);

            AMainDS.ATransaction.DefaultView.RowFilter = origTransactionFilter;
            AMainDS.AJournal.DefaultView.RowFilter = origJournalFilter;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        private static GLBatchTDS UpdateTotalsOfBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            //TVerificationResultCollection AVerificationResult = new TVerificationResultCollection();
            GLBatchTDS glDS = new GLBatchTDS();

            decimal sumDebits = 0.0M;
            decimal sumCredits = 0.0M;

            //Load Batch, Journal and Transaction records
            glDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, ABatchNumber));

            ABatchRow currentBatch = (ABatchRow)glDS.ABatch.Rows[0];

            glDS.AJournal.DefaultView.RowFilter = string.Empty;

            foreach (DataRowView journalview in glDS.AJournal.DefaultView)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                glDS.ATransaction.DefaultView.RowFilter = String.Format("{0} = {1} And {2} = {3}",
                    ATransactionTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    ATransactionTable.GetJournalNumberDBName(),
                    journalrow.JournalNumber);

                UpdateTotalsOfJournal(ref glDS, journalrow);

                sumDebits += journalrow.JournalDebitTotal;
                sumCredits += journalrow.JournalCreditTotal;
            }

            currentBatch.BatchDebitTotal = sumDebits;
            currentBatch.BatchCreditTotal = sumCredits;
            currentBatch.BatchRunningTotal = Math.Round(sumDebits - sumCredits, 2);

            return glDS;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        /// NOTE this no longer calculates AmountInBaseCurrency
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        private static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            GLBatchTDSAJournalRow ACurrentJournal)
        {
            if (ACurrentJournal == null)
            {
                return;
            }

            if ((ACurrentJournal.ExchangeRateToBase == 0.0m)
                && (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString()))
            {
                throw new Exception(String.Format("Batch {0} Journal {1} has invalid exchange rate to base",
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
            }

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            AMainDS.ATransaction.DefaultView.RowFilter = String.Format("{0} = {1} And {2} = {3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView v in AMainDS.ATransaction.DefaultView)
            {
                ATransactionRow r = (ATransactionRow)v.Row;

                // recalculate the amount in base currency

                if (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                {
                    r.AmountInBaseCurrency = r.TransactionAmount / ACurrentJournal.ExchangeRateToBase;
                }

                if (r.DebitCreditIndicator)
                {
                    ACurrentJournal.JournalDebitTotal += r.TransactionAmount;
                    ACurrentJournal.JournalDebitTotalBase += r.AmountInBaseCurrency;
                }
                else
                {
                    ACurrentJournal.JournalCreditTotal += r.TransactionAmount;
                    ACurrentJournal.JournalCreditTotalBase += r.AmountInBaseCurrency;
                }
            }
        }
    }
}