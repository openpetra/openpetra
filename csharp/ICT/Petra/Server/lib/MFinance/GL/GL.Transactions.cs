//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash
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
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Data;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;
//using System.Text;

using Ict.Common;
//using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;

//using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL.Data.Access;

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
        /// <param name="AYear">if -1, the year will be ignored</param>
        /// <param name="APeriod">if AYear is -1 or period is -1, the period will be ignored.
        /// if APeriod is 0 and the current year is selected, then the current and the forwarding periods are used</param>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatch(Int32 ALedgerNumber, int AYear, int APeriod)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(
                                    Catalog.GetString("Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        string FilterByPeriod = string.Empty;

                        if (AYear > -1)
                        {
                            FilterByPeriod = String.Format(" AND PUB_{0}.{1} = {2}",
                                ABatchTable.GetTableDBName(),
                                ABatchTable.GetBatchYearDBName(),
                                AYear);

                            if ((APeriod == 0) && (AYear == MainDS.ALedger[0].CurrentFinancialYear))
                            {
                                //Return current and forwarding periods
                                FilterByPeriod += String.Format(" AND PUB_{0}.{1} >= {2}",
                                    ABatchTable.GetTableDBName(),
                                    ABatchTable.GetBatchPeriodDBName(),
                                    MainDS.ALedger[0].CurrentPeriod);
                            }
                            else if (APeriod > 0)
                            {
                                //Return only specified period
                                FilterByPeriod += String.Format(" AND PUB_{0}.{1} = {2}",
                                    ABatchTable.GetTableDBName(),
                                    ABatchTable.GetBatchPeriodDBName(),
                                    APeriod);
                            }

                            //else
                            //{
                            //    //Nothing to add, returns all periods
                            //}
                        }

                        string SelectClause =
                            String.Format("SELECT * FROM PUB_{0} WHERE {1} = {2}",
                                ABatchTable.GetTableDBName(),
                                ABatchTable.GetLedgerNumberDBName(),
                                ALedgerNumber);

                        DBAccess.GDBAccessObj.Select(MainDS, SelectClause + FilterByPeriod,
                            MainDS.ABatch.TableName, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ABatch == null) || (MainDS.ABatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
        public static GLBatchTDS LoadABatchAndRelatedTables(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            return LoadABatchAndRelatedTables(DBAccess.GDBAccessObj, ALedgerNumber, ABatchNumber);
        }

        /// <summary>
        /// load the specified batch and its journals and transactions and attributes.
        /// this method is called after a batch has been posted.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAndRelatedTablesUsingPrivateDb(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            TDataBase dbConnection = TReportingDbAdapter.EstablishDBConnection(true, "LoadABatchAndRelatedTables");

            GLBatchTDS tempTDS = LoadABatchAndRelatedTables(dbConnection, ALedgerNumber, ABatchNumber);

            dbConnection.CloseDBConnection();
            return tempTDS;
        }

        private static GLBatchTDS LoadABatchAndRelatedTables(TDataBase ADbConnection, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                ADbConnection.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
                        ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }
                        else if ((MainDS.ABatch == null) || (MainDS.ABatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger number {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if ((MainDS.AJournal.Count == 0) && (MainDS.ATransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if (((MainDS.AJournal.Count == 0) || (MainDS.ATransaction.Count == 0)) && (MainDS.ATransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ABatch == null) || (MainDS.ABatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger number {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if ((MainDS.AJournal.Count == 0) && (MainDS.ATransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// load the specified batch and its journals and transactions.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringBatchARecurJournalARecurTransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ARecurringJournalAccess.LoadViaARecurringBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        ARecurringTransactionTable TransactionTable = new ARecurringTransactionTable();
                        ARecurringTransactionRow TemplateTransactionRow = TransactionTable.NewRowTyped(false);
                        TemplateTransactionRow.LedgerNumber = ALedgerNumber;
                        TemplateTransactionRow.BatchNumber = ABatchNumber;
                        ARecurringTransactionAccess.LoadUsingTemplate(MainDS, TemplateTransactionRow, Transaction);

                        #region Validate Data

                        if ((MainDS.ARecurringBatch == null) || (MainDS.ARecurringBatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger number {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if ((MainDS.ARecurringJournal.Count == 0) && (MainDS.ARecurringTransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ABatch == null) || (MainDS.ABatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger number {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// load the specified batch and specified journal.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatchAJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ABatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ABatch == null) || (MainDS.ABatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Batch data for Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if ((MainDS.AJournal == null) || (MainDS.AJournal.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Journal data for Journal {1} Batch {2} in Ledger {3} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    AJournalNumber,
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// Loads a journal for the given ledger, batch and journal number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.AJournal == null) || (MainDS.AJournal.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Journal data for Journal {1} Batch {2} in Ledger {3} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    AJournalNumber,
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of journals and transactions for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournalATransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.AJournal.Count == 0) && (MainDS.ATransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournalAndRelatedTablesForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.AJournal.Count == 0) && (MainDS.ATransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if (((MainDS.AJournal.Count == 0) || (MainDS.ATransaction.Count == 0)) && (MainDS.ATransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournalAndRelatedTables(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                        ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                        ATransAnalAttribAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.AJournal.Count == 0) && (MainDS.ATransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if (((MainDS.AJournal.Count == 0) || (MainDS.ATransaction.Count == 0)) && (MainDS.ATransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring journals for the given ledger and recurring batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringJournalAndRelatedTables(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
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

                        #region Validate Data

                        if ((MainDS.ARecurringJournal.Count == 0) && (MainDS.ARecurringTransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if (((MainDS.ARecurringJournal.Count == 0)
                                  || (MainDS.ARecurringTransaction.Count == 0)) && (MainDS.ARecurringTransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring transactions for the given ledger and recurring batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransactionAndRelatedTables(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
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

                        #region Validate Data

                        if ((MainDS.ARecurringTransaction.Count == 0) && (MainDS.ARecurringTransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.AJournal == null) || (MainDS.AJournal.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - GL Journal data for Journal {1} Batch {2} in Ledger {3} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    AJournalNumber,
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// load the specified journal with its transactions.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringJournalARecurringTransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringJournalAccess.LoadViaARecurringBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        ARecurringTransactionTable TransactionTable = new ARecurringTransactionTable();
                        ARecurringTransactionRow TemplateTransactionRow = TransactionTable.NewRowTyped(false);
                        TemplateTransactionRow.LedgerNumber = ALedgerNumber;
                        TemplateTransactionRow.BatchNumber = ABatchNumber;
                        ARecurringTransactionAccess.LoadUsingTemplate(MainDS, TemplateTransactionRow, Transaction);

                        #region Validate Data

                        if ((MainDS.ARecurringJournal.Count == 0) && (MainDS.ARecurringTransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned transactions exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
        public static GLBatchTDS LoadARecurringJournalARecurringTransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Recurring Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ARecurringJournal == null) || (MainDS.ARecurringJournal.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Recurring GL Journal data for Journal {1} Batch {2} in Ledger {3} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    AJournalNumber,
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        ARecurringTransactionAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
        public static GLBatchTDS LoadATransactionForJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// Load the transactions for the specified journal.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Recurring Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringTransactionAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// Loads a list of transactions for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringTransaction(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringTransactionTable TransactionTable = new ARecurringTransactionTable();
                        ARecurringTransactionRow TemplateTransactionRow = TransactionTable.NewRowTyped(false);
                        TemplateTransactionRow.LedgerNumber = ALedgerNumber;
                        TemplateTransactionRow.BatchNumber = ABatchNumber;
                        ARecurringTransactionAccess.LoadUsingTemplate(MainDS, TemplateTransactionRow, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch with analysis attributes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransactionAndRelatedTablesForBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            string AnalysisAttrList = string.Empty;

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransactionAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                        ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ATransaction.Count == 0) && (MainDS.ATransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                CreateCombinedAnalAttribListPerTransaction(MainDS);

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
        public static GLBatchTDS LoadATransactionAndRelatedTablesForJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                        ATransAnalAttribAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ATransaction.Count == 0) && (MainDS.ATransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in GL Journal {1} in Batch {2} in Ledger {3}!"),
                                    Utilities.GetMethodName(true),
                                    AJournalNumber,
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                CreateCombinedAnalAttribListPerTransaction(MainDS);

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        private static void CreateCombinedAnalAttribListPerTransaction(GLBatchTDS AMainDS)
        {
            if ((AMainDS == null)
                || (AMainDS.ATransaction == null)
                || (AMainDS.ATransAnalAttrib == null)
                || (AMainDS.ATransaction.Count == 0))
            {
                return;
            }

            string AnalysisAttrList = string.Empty;

            //Create the combined analysis attributes list per transaction row
            foreach (GLBatchTDSATransactionRow transRow in AMainDS.ATransaction.Rows)
            {
                AMainDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                    ATransAnalAttribTable.GetTransactionNumberDBName(),
                    transRow.TransactionNumber);

                foreach (DataRowView drv in AMainDS.ATransAnalAttrib.DefaultView)
                {
                    ATransAnalAttribRow transAnalAttrRow = (ATransAnalAttribRow)drv.Row;

                    if (AnalysisAttrList.Length > 0)
                    {
                        AnalysisAttrList += ", ";
                    }

                    AnalysisAttrList += (transAnalAttrRow.AnalysisTypeCode + "=" + transAnalAttrRow.AnalysisAttributeValue);
                }

                if (transRow.AnalysisAttributes != AnalysisAttrList)
                {
                    transRow.AnalysisAttributes = AnalysisAttrList;
                }

                //reset the attributes string
                AnalysisAttrList = string.Empty;
            }

            AMainDS.ATransAnalAttrib.DefaultView.RowFilter = string.Empty;
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
        public static GLBatchTDS LoadATransAnalAttribForTransaction(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            Int32 ATransactionNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }
            else if (ATransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        ATransactionNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransAnalAttribAccess.LoadViaATransaction(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber,
                            Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransAnalAttribAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given Batch (identified by ledger,batch)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransAnalAttribForJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ATransAnalAttribAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads some necessary analysis attributes tables for the given ledger number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AActiveOnly"></param>
        /// <returns>GLSetupTDS</returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadAAnalysisAttributes(Int32 ALedgerNumber, Boolean AActiveOnly = false)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLSetupTDS MainDS = new GLSetupTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        AAnalysisTypeAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

                        if (!AActiveOnly)
                        {
                            AFreeformAnalysisAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                            AAnalysisAttributeAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                        }
                        else
                        {
                            AFreeformAnalysisTable FFTable = new AFreeformAnalysisTable();
                            AFreeformAnalysisRow TemplateFFRow = FFTable.NewRowTyped(false);
                            TemplateFFRow.LedgerNumber = ALedgerNumber;
                            TemplateFFRow.Active = true;
                            AFreeformAnalysisAccess.LoadUsingTemplate(MainDS, TemplateFFRow, Transaction);

                            AAnalysisAttributeTable AATable = new AAnalysisAttributeTable();
                            AAnalysisAttributeRow TemplateAARow = AATable.NewRowTyped(false);
                            TemplateAARow.LedgerNumber = ALedgerNumber;
                            TemplateAARow.Active = true;
                            AAnalysisAttributeAccess.LoadUsingTemplate(MainDS, TemplateAARow, Transaction);
                        }
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// returns ledger table for specified ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadALedgerTable(Int32 ALedgerNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                // Remove all Tables that were not filled with data before remoting them.
                MainDS.RemoveEmptyTables();

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        string SelectClause =
                            String.Format("SELECT * FROM PUB_{0} WHERE {1}={2}",
                                ARecurringBatchTable.GetTableDBName(),
                                ARecurringBatchTable.GetLedgerNumberDBName(),
                                ALedgerNumber);

                        string FilterByBatchStatus = string.Empty;

                        if ((AFilterBatchStatus & TFinanceBatchFilterEnum.fbfEditing) != 0)
                        {
                            FilterByBatchStatus =
                                string.Format(" AND {0} = '{1}'",
                                    ARecurringBatchTable.GetBatchStatusDBName(),
                                    MFinanceConstants.BATCH_UNPOSTED);
                        }

                        //else if (AFilterBatchStatus == TFinanceBatchFilterEnum.fbfAll)
                        //{
                        //    // FilterByBatchStatus is empty
                        //}

                        DBAccess.GDBAccessObj.Select(MainDS, SelectClause + FilterByBatchStatus,
                            MainDS.ARecurringBatch.TableName, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
        public static GLBatchTDS LoadARecurringBatchAndRelatedTables(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
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

                        #region Validate Data

                        if ((MainDS.ARecurringBatch == null) || (MainDS.ARecurringBatch.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Batch data for Recurring GL Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if ((MainDS.ARecurringJournal.Count == 0) && (MainDS.ARecurringTransaction.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transactions exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }
                        else if (((MainDS.ARecurringJournal.Count == 0)
                                  || (MainDS.ARecurringTransaction.Count == 0)) && (MainDS.ARecurringTransAnalAttrib.Count > 0))
                        {
                            throw new ApplicationException(String.Format(Catalog.GetString(
                                        "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in Recurring GL Batch {1} in Ledger {2}!"),
                                    Utilities.GetMethodName(true),
                                    ABatchNumber,
                                    ALedgerNumber));
                        }

                        #endregion Validate Data
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringJournalAccess.LoadViaARecurringBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadARecurringJournal(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringJournalAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
        public static GLBatchTDS LoadARecurringTransactionARecurringTransAnalAttrib(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            string AnalysisAttrList = string.Empty;

            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringTransactionAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                        ARecurringTransAnalAttribAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                #region Validate Data

                if ((MainDS.ARecurringTransaction.Count == 0) && (MainDS.ARecurringTransAnalAttrib.Count > 0))
                {
                    throw new ApplicationException(String.Format(Catalog.GetString(
                                "Function:{0} - Orphaned GL Transaction Analysis Attributes exist in Recurring GL Batch {1} in Ledger {2}!"),
                            Utilities.GetMethodName(true),
                            ABatchNumber,
                            ALedgerNumber));
                }

                #endregion Validate Data

                foreach (GLBatchTDSARecurringTransactionRow transRow in MainDS.ARecurringTransaction.Rows)
                {
                    MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        transRow.TransactionNumber);

                    foreach (DataRowView drv in MainDS.ARecurringTransAnalAttrib.DefaultView)
                    {
                        ARecurringTransAnalAttribRow recurrTransAnalAttrRow = (ARecurringTransAnalAttribRow)drv.Row;

                        if (AnalysisAttrList.Length > 0)
                        {
                            AnalysisAttrList += ", ";
                        }

                        AnalysisAttrList += (recurrTransAnalAttrRow.AnalysisTypeCode + "=" + recurrTransAnalAttrRow.AnalysisAttributeValue);
                    }

                    if (transRow.AnalysisAttributes != AnalysisAttrList)
                    {
                        transRow.AnalysisAttributes = AnalysisAttrList;
                    }

                    //clear the attributes string and table
                    AnalysisAttrList = string.Empty;
                }

                MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = string.Empty;

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }
            else if (ATransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Transaction number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringTransAnalAttribAccess.LoadViaARecurringTransaction(MainDS,
                            ALedgerNumber,
                            ABatchNumber,
                            AJournalNumber,
                            ATransactionNumber,
                            Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true),
                        AJournalNumber));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDS = new GLBatchTDS();

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ARecurringTransAnalAttribAccess.LoadViaARecurringJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
                    });

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

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
            TVerificationResultCollection VerificationResult = AVerificationResult;

            // make sure that empty tables are removed. This can return NULL!!
            AInspectDS = AInspectDS.GetChangesTyped(true);

            if (AInspectDS == null)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Save GL Batch"),
                        Catalog.GetString("No changes - nothing to do"),
                        TResultSeverity.Resv_Info));
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            bool AllValidationsOK = true;

            bool GLBatchTableInDataSet = (AInspectDS.ABatch != null && AInspectDS.ABatch.Count > 0);
            bool GLJournalTableInDataSet = (AInspectDS.AJournal != null && AInspectDS.AJournal.Count > 0);
            bool GLTransTableInDataSet = (AInspectDS.ATransaction != null && AInspectDS.ATransaction.Count > 0);
            bool GLTransAttrTableInDataSet = (AInspectDS.ATransAnalAttrib != null && AInspectDS.ATransAnalAttrib.Count > 0);

            bool RecurrGLBatchTableInDataSet = (AInspectDS.ARecurringBatch != null && AInspectDS.ARecurringBatch.Count > 0);
            bool RecurrGLJournalTableInDataSet = (AInspectDS.ARecurringJournal != null && AInspectDS.ARecurringJournal.Count > 0);
            bool RecurrGLTransTableInDataSet = (AInspectDS.ARecurringTransaction != null && AInspectDS.ARecurringTransaction.Count > 0);
            bool RecurrGLAttrTableInDataSet = (AInspectDS.ARecurringTransAnalAttrib != null && AInspectDS.ARecurringTransAnalAttrib.Count > 0);

            //Check if saving recurring tables
            if (RecurrGLBatchTableInDataSet || RecurrGLJournalTableInDataSet || RecurrGLTransTableInDataSet || RecurrGLAttrTableInDataSet)
            {
                if (GLBatchTableInDataSet || GLJournalTableInDataSet || GLTransTableInDataSet || GLTransAttrTableInDataSet)
                {
                    throw new Exception(String.Format("Function:{0} - Recurring and normal GL data found in same changes batch!",
                            Utilities.GetMethodName(true)));
                }

                return SaveRecurringGLBatchTDS(ref AInspectDS,
                    ref AVerificationResult,
                    RecurrGLBatchTableInDataSet,
                    RecurrGLJournalTableInDataSet,
                    RecurrGLTransTableInDataSet,
                    RecurrGLAttrTableInDataSet);
            }
            else
            {
                if (!(GLBatchTableInDataSet || GLJournalTableInDataSet || GLTransTableInDataSet || GLTransAttrTableInDataSet))
                {
                    throw new Exception(String.Format("Function:{0} - No GL data changes to save!", Utilities.GetMethodName(true)));
                }
            }

            GLBatchTDS InspectDS = AInspectDS;

            List <Int32>ListAllGLBatchesToProcess = new List <int>();
            Int32 LedgerNumber = -1;

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable,
                    ref Transaction,
                    delegate
                    {
                        if (GLBatchTableInDataSet)
                        {
                            DataView AllBatchesToProcess = new DataView(InspectDS.ABatch);
                            AllBatchesToProcess.RowStateFilter = DataViewRowState.OriginalRows | DataViewRowState.Added;

                            foreach (DataRowView drv in AllBatchesToProcess)
                            {
                                ABatchRow glbr = (ABatchRow)drv.Row;
                                int batchNumber;

                                if (glbr.RowState != DataRowState.Deleted)
                                {
                                    LedgerNumber = glbr.LedgerNumber;
                                    batchNumber = glbr.BatchNumber;
                                }
                                else
                                {
                                    LedgerNumber = (Int32)glbr[ABatchTable.ColumnLedgerNumberId, DataRowVersion.Original];
                                    batchNumber = (Int32)glbr[ABatchTable.ColumnBatchNumberId, DataRowVersion.Original];
                                }

                                if (!ListAllGLBatchesToProcess.Contains(batchNumber))
                                {
                                    ListAllGLBatchesToProcess.Add(batchNumber);
                                }

                                int periodNumber, yearNumber;

                                if (TFinancialYear.IsValidPostingPeriod(LedgerNumber,
                                        glbr.DateEffective,
                                        out periodNumber,
                                        out yearNumber,
                                        Transaction))
                                {
                                    glbr.BatchYear = yearNumber;
                                    glbr.BatchPeriod = periodNumber;
                                }
                            }

                            //TODO add validation as with gift
                            //ValidateGiftDetail(ref AVerificationResult, AInspectDS.AGiftDetail);
                            //ValidateGiftDetailManual(ref AVerificationResult, AInspectDS.AGiftDetail);

                            if (!TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult))
                            {
                                AllValidationsOK = false;
                            }
                        }

                        if (GLJournalTableInDataSet)
                        {
                            DataView AllBatchesToProcess = new DataView(InspectDS.AJournal);
                            AllBatchesToProcess.RowStateFilter = DataViewRowState.OriginalRows | DataViewRowState.Added;

                            foreach (DataRowView drv in AllBatchesToProcess)
                            {
                                GLBatchTDSAJournalRow gljr = (GLBatchTDSAJournalRow)drv.Row;
                                int batchNumber;

                                if (gljr.RowState != DataRowState.Deleted)
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = gljr.LedgerNumber;
                                    }

                                    batchNumber = gljr.BatchNumber;
                                }
                                else
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = (Int32)gljr[AJournalTable.ColumnLedgerNumberId, DataRowVersion.Original];
                                    }

                                    batchNumber = (Int32)gljr[AJournalTable.ColumnBatchNumberId, DataRowVersion.Original];
                                }

                                if (!ListAllGLBatchesToProcess.Contains(batchNumber))
                                {
                                    ListAllGLBatchesToProcess.Add(batchNumber);
                                }
                            }

                            //TODO add validation as with gift
                            //ValidateGiftDetail(ref AVerificationResult, AInspectDS.AGiftDetail);
                            //ValidateGiftDetailManual(ref AVerificationResult, AInspectDS.AGiftDetail);

                            if (!TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult))
                            {
                                AllValidationsOK = false;
                            }
                        }

                        if (GLTransTableInDataSet)
                        {
                            DataView AllBatchesToProcess = new DataView(InspectDS.ATransaction);
                            AllBatchesToProcess.RowStateFilter = DataViewRowState.OriginalRows | DataViewRowState.Added;

                            GLPostingTDS accountsAndCostCentresDS = new GLPostingTDS();

                            foreach (DataRowView drv in AllBatchesToProcess)
                            {
                                ATransactionRow gltr = (ATransactionRow)drv.Row;
                                int batchNumber;

                                if (gltr.RowState != DataRowState.Deleted)
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = gltr.LedgerNumber;
                                    }

                                    batchNumber = gltr.BatchNumber;

                                    //Prepare to test for valid account and cost centre code
                                    if ((accountsAndCostCentresDS.AAccount == null) || (accountsAndCostCentresDS.AAccount.Count == 0))
                                    {
                                        AAccountAccess.LoadViaALedger(accountsAndCostCentresDS, LedgerNumber, Transaction);
                                        ACostCentreAccess.LoadViaALedger(accountsAndCostCentresDS, LedgerNumber, Transaction);

                                        #region Validate Data

                                        if ((accountsAndCostCentresDS.AAccount == null) || (accountsAndCostCentresDS.AAccount.Count == 0))
                                        {
                                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                                        "Function:{0} - Account data for Ledger number {1} does not exist or could not be accessed!"),
                                                    Utilities.GetMethodName(true),
                                                    LedgerNumber));
                                        }
                                        else if ((accountsAndCostCentresDS.ACostCentre == null) || (accountsAndCostCentresDS.ACostCentre.Count == 0))
                                        {
                                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                                        "Function:{0} - Cost Centre data for Ledger number {1} does not exist or could not be accessed!"),
                                                    Utilities.GetMethodName(true),
                                                    LedgerNumber));
                                        }

                                        #endregion Validate Data
                                    }

                                    CheckTransactionAccountAndCostCentre(LedgerNumber, ref accountsAndCostCentresDS, ref gltr, ref VerificationResult);
                                }
                                else
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = (Int32)gltr[ATransactionTable.ColumnLedgerNumberId, DataRowVersion.Original];
                                    }

                                    batchNumber = (Int32)gltr[ATransactionTable.ColumnBatchNumberId, DataRowVersion.Original];
                                }

                                if (!ListAllGLBatchesToProcess.Contains(batchNumber))
                                {
                                    ListAllGLBatchesToProcess.Add(batchNumber);
                                }
                            }

                            //TODO add validation as with gift
                            //ValidateGiftDetail(ref AVerificationResult, AInspectDS.AGiftDetail);
                            //ValidateGiftDetailManual(ref AVerificationResult, AInspectDS.AGiftDetail);

                            if (!TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult))
                            {
                                AllValidationsOK = false;
                            }
                        }

                        if (GLTransAttrTableInDataSet)
                        {
                            DataView AllBatchesToProcess = new DataView(InspectDS.ATransAnalAttrib);
                            AllBatchesToProcess.RowStateFilter = DataViewRowState.OriginalRows | DataViewRowState.Added;

                            foreach (DataRowView drv in AllBatchesToProcess)
                            {
                                ATransAnalAttribRow glta = (ATransAnalAttribRow)drv.Row;
                                int batchNumber;

                                if (glta.RowState != DataRowState.Deleted)
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = glta.LedgerNumber;
                                    }

                                    batchNumber = glta.BatchNumber;
                                }
                                else
                                {
                                    if (LedgerNumber == -1)
                                    {
                                        LedgerNumber = (Int32)glta[ATransAnalAttribTable.ColumnLedgerNumberId, DataRowVersion.Original];
                                    }

                                    batchNumber = (Int32)glta[ATransAnalAttribTable.ColumnBatchNumberId, DataRowVersion.Original];
                                }

                                if (!ListAllGLBatchesToProcess.Contains(batchNumber))
                                {
                                    ListAllGLBatchesToProcess.Add(batchNumber);
                                }
                            }

                            //TODO add validation as with gift
                            //ValidateGiftDetail(ref AVerificationResult, AInspectDS.AGiftDetail);
                            //ValidateGiftDetailManual(ref AVerificationResult, AInspectDS.AGiftDetail);

                            if (!TVerificationHelper.IsNullOrOnlyNonCritical(VerificationResult))
                            {
                                AllValidationsOK = false;
                            }
                        }

                        // load previously stored batches and check for posted status
                        if (ListAllGLBatchesToProcess.Count == 0)
                        {
                            VerificationResult.Add(new TVerificationResult(Catalog.GetString("Saving Batch"),
                                    Catalog.GetString("Cannot save an empty Batch!"),
                                    TResultSeverity.Resv_Critical));
                        }
                        else
                        {
                            string listOfBatchNumbers = string.Empty;

                            foreach (Int32 batchNumber in ListAllGLBatchesToProcess)
                            {
                                listOfBatchNumbers = StringHelper.AddCSV(listOfBatchNumbers, batchNumber.ToString());
                            }

                            string SQLStatement = "SELECT * " +
                                                  " FROM PUB_" + ABatchTable.GetTableDBName() + " WHERE " + ABatchTable.GetLedgerNumberDBName() +
                                                  " = " +
                                                  LedgerNumber.ToString() +
                                                  " AND " + ABatchTable.GetBatchNumberDBName() + " IN (" + listOfBatchNumbers + ")";

                            GLBatchTDS BatchDS = new GLBatchTDS();

                            DBAccess.GDBAccessObj.Select(BatchDS, SQLStatement, BatchDS.ABatch.TableName, Transaction);

                            foreach (ABatchRow batch in BatchDS.ABatch.Rows)
                            {
                                if ((batch.BatchStatus == MFinanceConstants.BATCH_POSTED)
                                    || (batch.BatchStatus == MFinanceConstants.BATCH_CANCELLED))
                                {
                                    VerificationResult.Add(new TVerificationResult(Catalog.GetString("Saving Batch"),
                                            String.Format(Catalog.GetString("Cannot modify Batch {0} because it is {1}"),
                                                batch.BatchNumber, batch.BatchStatus),
                                            TResultSeverity.Resv_Critical));
                                }
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
            {
                return TSubmitChangesResult.scrError;
            }

            TSubmitChangesResult SubmissionResult;

            if (AllValidationsOK)
            {
                //Need to save changes before deleting any transactions
                GLBatchTDSAccess.SubmitChanges(AInspectDS);

                SubmissionResult = TSubmitChangesResult.scrOK;
            }
            else
            {
                SubmissionResult = TSubmitChangesResult.scrError;
            }

            return SubmissionResult;
        }

        /// <summary>
        /// Delete transactions and attributes and renumber accordingly
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AHighestTransactionNumber"></param>
        /// <param name="ATransactionToDelete"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS ProcessTransAndAttributesForDeletion(GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            Int32 AHighestTransactionNumber,
            Int32 ATransactionToDelete)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AHighestTransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The highest Transaction number in the Journal must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransactionToDelete <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The number of the Transaction to delete must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDSCopy = (GLBatchTDS)AMainDS.Copy();
            MainDSCopy.Merge(AMainDS);
            MainDSCopy.AcceptChanges();

            GLBatchTDS SubmitDS = (GLBatchTDS)AMainDS.Copy();
            SubmitDS.Merge(AMainDS);
            SubmitDS.AcceptChanges();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        //Delete current row+ (attributes first).
                        DataView attributesDV = new DataView(MainDSCopy.ATransAnalAttrib);
                        attributesDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV)
                        {
                            ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV = new DataView(MainDSCopy.ATransaction);
                        transactionsDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ATransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV)
                        {
                            ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        //Need to save changes before deleting any transactions
                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);


                        //Remove unaffected attributes and transactions from SubmitDS
                        DataView attributesDV1 = new DataView(SubmitDS.ATransAnalAttrib);
                        attributesDV1.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}<={5}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV1)
                        {
                            ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV1 = new DataView(SubmitDS.ATransaction);
                        transactionsDV1.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}<={5}",
                            ATransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV1)
                        {
                            ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        //GLBatchTDSAccess.SubmitChanges(MainDS);
                        SubmitDS.AcceptChanges();


                        //Renumber the transactions and attributes in SubmitDS
                        DataView attributesDV2 = new DataView(SubmitDS.ATransAnalAttrib);
                        attributesDV2.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber);
                        attributesDV2.Sort = String.Format("{0} ASC", ATransAnalAttribTable.GetTransactionNumberDBName());

                        foreach (DataRowView attrDRV in attributesDV2)
                        {
                            ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                            attrRow.TransactionNumber--;
                        }

                        DataView transactionsDV2 = new DataView(SubmitDS.ATransaction);
                        transactionsDV2.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ATransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransactionTable.GetJournalNumberDBName(),
                            AJournalNumber);
                        transactionsDV2.Sort = String.Format("{0} ASC", ATransactionTable.GetTransactionNumberDBName());

                        foreach (DataRowView transDRV in transactionsDV2)
                        {
                            ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
                            tranRow.TransactionNumber--;
                        }

                        SubmitDS.AcceptChanges();


                        //Set RowStates to added to ensure changes get detected
                        MainDSCopy.Merge(SubmitDS.ATransaction);
                        MainDSCopy.AcceptChanges();

                        DataView transactionsDV3 = new DataView(MainDSCopy.ATransaction);
                        transactionsDV3.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ATransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV3)
                        {
                            ATransactionRow tranRow = (ATransactionRow)transDRV.Row;
                            tranRow.SetAdded();
                        }

                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);


                        MainDSCopy.Merge(SubmitDS.ATransAnalAttrib);
                        MainDSCopy.AcceptChanges();

                        //Set RowState to added to ensure changes get detected
                        DataView attributesDV3 = new DataView(MainDSCopy.ATransAnalAttrib);
                        attributesDV3.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ATransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ATransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ATransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV3)
                        {
                            ATransAnalAttribRow attrRow = (ATransAnalAttribRow)attrDRV.Row;
                            attrRow.SetAdded();
                        }

                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);
                        MainDSCopy.AcceptChanges();

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDSCopy;
        }

        /// <summary>
        /// Delete recurring journals and transactions and attributes and renumber accordingly
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AHighestJournalNumber"></param>
        /// <param name="AJournalToDelete"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS ProcessRecurrJrnlTransAttribForDeletion(GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AHighestJournalNumber,
            Int32 AJournalToDelete)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AHighestJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The highest Transaction number in the Recurring Journal must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AJournalToDelete <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The number of the Recurring Journal to delete must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDSCopy = (GLBatchTDS)AMainDS.Copy();
            MainDSCopy.Merge(AMainDS);
            MainDSCopy.AcceptChanges();

            GLBatchTDS SubmitDS = (GLBatchTDS)AMainDS.Copy();
            SubmitDS.Merge(AMainDS);
            SubmitDS.AcceptChanges();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        //Delete current journal (and higher journals) data (attributes first).
                        DataView attributesDV = new DataView(MainDSCopy.ARecurringTransAnalAttrib);
                        attributesDV.RowFilter = String.Format("{0}={1} And {2}>={3}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView attrDRV in attributesDV)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV = new DataView(MainDSCopy.ARecurringTransaction);
                        transactionsDV.RowFilter = String.Format("{0}={1} And {2}>={3}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView transDRV in transactionsDV)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        DataView journalDV = new DataView(MainDSCopy.ARecurringJournal);
                        journalDV.RowFilter = String.Format("{0}={1} And {2}>={3}",
                            ARecurringJournalTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringJournalTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView jrnlDRV in journalDV)
                        {
                            ARecurringJournalRow jrnlRow = (ARecurringJournalRow)jrnlDRV.Row;
                            jrnlRow.Delete();
                        }

                        //Need to save changes before deleting any transactions
                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);


                        //Remove unaffected attributes and transactions and journals from SubmitDS
                        DataView attributesDV1 = new DataView(SubmitDS.ARecurringTransAnalAttrib);
                        attributesDV1.RowFilter = String.Format("{0}={1} AND {2}<={3}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView attrDRV in attributesDV1)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV1 = new DataView(SubmitDS.ARecurringTransaction);
                        transactionsDV1.RowFilter = String.Format("{0}={1} AND {2}<={3}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView transDRV in transactionsDV1)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        DataView jrnlDV1 = new DataView(SubmitDS.ARecurringJournal);
                        jrnlDV1.RowFilter = String.Format("{0}={1} AND {2}<={3}",
                            ARecurringJournalTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringJournalTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView jrnlDRV in jrnlDV1)
                        {
                            ARecurringJournalRow jrnlRow = (ARecurringJournalRow)jrnlDRV.Row;
                            jrnlRow.Delete();
                        }

                        SubmitDS.AcceptChanges();


                        //Renumber the transactions and attributes in SubmitDS
                        DataView attributesDV2 = new DataView(SubmitDS.ARecurringTransAnalAttrib);
                        attributesDV2.RowFilter = String.Format("{0}={1}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber);
                        attributesDV2.Sort = String.Format("{0} ASC", ARecurringTransAnalAttribTable.GetJournalNumberDBName());

                        foreach (DataRowView attrDRV in attributesDV2)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.JournalNumber--;
                        }

                        DataView transactionsDV2 = new DataView(SubmitDS.ARecurringTransaction);
                        transactionsDV2.RowFilter = String.Format("{0}={1}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber);
                        transactionsDV2.Sort = String.Format("{0} ASC", ARecurringTransactionTable.GetJournalNumberDBName());

                        foreach (DataRowView transDRV in transactionsDV2)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.JournalNumber--;
                        }

                        DataView jrnlDV2 = new DataView(SubmitDS.ARecurringJournal);
                        jrnlDV2.RowFilter = String.Format("{0}={1}",
                            ARecurringJournalTable.GetBatchNumberDBName(),
                            ABatchNumber);
                        jrnlDV2.Sort = String.Format("{0} ASC", ARecurringJournalTable.GetJournalNumberDBName());

                        foreach (DataRowView jrnlDRV in jrnlDV2)
                        {
                            ARecurringJournalRow jrnlRow = (ARecurringJournalRow)jrnlDRV.Row;
                            jrnlRow.JournalNumber--;
                        }

                        SubmitDS.AcceptChanges();


                        //Set RowStates to added to ensure changes get detected
                        MainDSCopy.Merge(SubmitDS.ARecurringJournal);
                        MainDSCopy.AcceptChanges();


                        //Set RowState to added to ensure changes get detected
                        DataView JrnlDV3 = new DataView(MainDSCopy.ARecurringJournal);
                        JrnlDV3.RowFilter = String.Format("{0}={1} AND {2}>={3}",
                            ARecurringJournalTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringJournalTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView jrnlDRV in JrnlDV3)
                        {
                            ARecurringJournalRow jrnlRow = (ARecurringJournalRow)jrnlDRV.Row;
                            jrnlRow.SetAdded();
                        }

                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);


                        MainDSCopy.Merge(SubmitDS.ARecurringTransaction);
                        MainDSCopy.AcceptChanges();

                        DataView transactionsDV3 = new DataView(MainDSCopy.ARecurringTransaction);
                        transactionsDV3.RowFilter = String.Format("{0}={1} AND {2}>={3}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView transDRV in transactionsDV3)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.SetAdded();
                        }

                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);


                        MainDSCopy.Merge(SubmitDS.ARecurringTransAnalAttrib);
                        MainDSCopy.AcceptChanges();

                        DataView attributesDV3 = new DataView(MainDSCopy.ARecurringTransAnalAttrib);
                        attributesDV3.RowFilter = String.Format("{0}={1} AND {2}>={3}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalToDelete);

                        foreach (DataRowView attrDRV in attributesDV3)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.SetAdded();
                        }

                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);
                        MainDSCopy.AcceptChanges();

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDSCopy;
        }

        /// <summary>
        /// Delete transactions and attributes and renumber accordingly
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AHighestTransactionNumber"></param>
        /// <param name="ATransactionToDelete"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS ProcessRecurringTransAndAttributesForDeletion(GLBatchTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            Int32 AHighestTransactionNumber,
            Int32 ATransactionToDelete)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (AJournalNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Recurring Journal number must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AHighestTransactionNumber <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The highest Transaction number in the Recurring Journal must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ATransactionToDelete <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The number of the Transaction to delete must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            GLBatchTDS MainDSCopy = (GLBatchTDS)AMainDS.Copy();
            MainDSCopy.Merge(AMainDS);
            MainDSCopy.AcceptChanges();

            GLBatchTDS SubmitDS = (GLBatchTDS)AMainDS.Copy();
            SubmitDS.Merge(AMainDS);
            SubmitDS.AcceptChanges();

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        //Delete current row+ (attributes first).
                        DataView attributesDV = new DataView(MainDSCopy.ARecurringTransAnalAttrib);
                        attributesDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV = new DataView(MainDSCopy.ARecurringTransaction);
                        transactionsDV.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        //Need to save changes before deleting any transactions
                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);

                        //Remove unaffected attributes and transactions from SubmitDS
                        DataView attributesDV1 = new DataView(SubmitDS.ARecurringTransAnalAttrib);
                        attributesDV1.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}<={5}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV1)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.Delete();
                        }

                        DataView transactionsDV1 = new DataView(SubmitDS.ARecurringTransaction);
                        transactionsDV1.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}<={5}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV1)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.Delete();
                        }

                        //GLBatchTDSAccess.SubmitChanges(MainDS);
                        SubmitDS.AcceptChanges();

                        //Renumber the transactions and attributes in SubmitDS
                        DataView attributesDV2 = new DataView(SubmitDS.ARecurringTransAnalAttrib);
                        attributesDV2.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber);
                        attributesDV2.Sort = String.Format("{0} ASC", ARecurringTransAnalAttribTable.GetTransactionNumberDBName());

                        foreach (DataRowView attrDRV in attributesDV2)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.TransactionNumber--;
                        }

                        DataView transactionsDV2 = new DataView(SubmitDS.ARecurringTransaction);
                        transactionsDV2.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalNumber);
                        transactionsDV2.Sort = String.Format("{0} ASC", ARecurringTransactionTable.GetTransactionNumberDBName());

                        foreach (DataRowView transDRV in transactionsDV2)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.TransactionNumber--;
                        }

                        SubmitDS.AcceptChanges();

                        MainDSCopy.Merge(SubmitDS.ARecurringTransaction);
                        MainDSCopy.AcceptChanges();

                        DataView transactionsDV3 = new DataView(MainDSCopy.ARecurringTransaction);
                        transactionsDV3.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ARecurringTransactionTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransactionTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransactionTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView transDRV in transactionsDV3)
                        {
                            ARecurringTransactionRow tranRow = (ARecurringTransactionRow)transDRV.Row;
                            tranRow.SetAdded();
                        }

                        //Need to save changes before deleting any transactions
                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);

                        MainDSCopy.Merge(SubmitDS.ARecurringTransAnalAttrib);
                        MainDSCopy.AcceptChanges();

                        DataView attributesDV3 = new DataView(MainDSCopy.ARecurringTransAnalAttrib);
                        attributesDV3.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}>={5}",
                            ARecurringTransAnalAttribTable.GetBatchNumberDBName(),
                            ABatchNumber,
                            ARecurringTransAnalAttribTable.GetJournalNumberDBName(),
                            AJournalNumber,
                            ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                            ATransactionToDelete);

                        foreach (DataRowView attrDRV in attributesDV3)
                        {
                            ARecurringTransAnalAttribRow attrRow = (ARecurringTransAnalAttribRow)attrDRV.Row;
                            attrRow.SetAdded();
                        }

                        //Need to save changes before deleting any transactions
                        GLBatchTDSAccess.SubmitChanges(MainDSCopy);
                        MainDSCopy.AcceptChanges();

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDSCopy;
        }

        private static void CheckTransactionAccountAndCostCentre(Int32 ALedgerNumber,
            ref GLPostingTDS AAccountsAndCostCentresDS,
            ref ATransactionRow ATransRow,
            ref TVerificationResultCollection AVerificationResult)
        {
            // check for valid accounts and cost centres
            if (AAccountsAndCostCentresDS.AAccount.Rows.Find(new object[] { ALedgerNumber, ATransRow.AccountCode }) == null)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Cannot save transaction"),
                        String.Format(Catalog.GetString("Invalid account code {0} in batch {1}, journal {2}, transaction {3}"),
                            ATransRow.AccountCode,
                            ATransRow.BatchNumber,
                            ATransRow.JournalNumber,
                            ATransRow.TransactionNumber),
                        TResultSeverity.Resv_Critical));
            }

            if (AAccountsAndCostCentresDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, ATransRow.CostCentreCode }) == null)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Cannot save transaction"),
                        String.Format(Catalog.GetString("Invalid cost centre code {0} in batch {1}, journal {2}, transaction {3}"),
                            ATransRow.CostCentreCode,
                            ATransRow.BatchNumber,
                            ATransRow.JournalNumber,
                            ATransRow.TransactionNumber),
                        TResultSeverity.Resv_Critical));
            }

            // AmountInBaseCurrency must be greater than 0.
            // Transaction amount can be 0 if ForexGain.
            if (ATransRow.AmountInBaseCurrency <= 0)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Cannot save transaction"),
                        String.Format(Catalog.GetString("Invalid amount in batch {0}, journal {1}, transaction {2}. " +
                                "Either the debit amount or the credit amount needs to be greater than 0."),
                            ATransRow.BatchNumber,
                            ATransRow.JournalNumber,
                            ATransRow.TransactionNumber),
                        TResultSeverity.Resv_Critical));
            }
        }

        /// This will store all new and modified recurring batches, journals, transactions
        [RequireModulePermission("FINANCE-1")]
        private static TSubmitChangesResult SaveRecurringGLBatchTDS(ref GLBatchTDS AInspectDS,
            ref TVerificationResultCollection AVerificationResult, bool ARecurringBatchTableInDataSet,
            bool ARecurringJournalTableInDataSet, bool ARecurringTransTableInDataSet,
            bool ARecurringTransAnalTableInDataSet)
        {
            Int32 LedgerNumber;
            Int32 BatchNumber;
            Int32 JournalNumber;
            Int32 TransactionNumber;
            Int32 Counter;

            TSubmitChangesResult SubmissionResult = new TSubmitChangesResult();

            //bool AllValidationsOK = true;

            //Not needed as yet
            // int RecurrBatchCount = ARecurringBatchTableInDataSet ? AInspectDS.ARecurringBatch.Count : 0;
            // int RecurrJournalCount = ARecurringJournalTableInDataSet ? AInspectDS.AJournal.Count : 0;
            // int RecurrTransCount = ARecurringTransTableInDataSet ? AInspectDS.ARecurringTransaction.Count : 0;
            // int RecurrTransAnalCount = ARecurringTransAnalTableInDataSet ? AInspectDS.ARecurringTransAnalAttrib.Count : 0;

            //Get a list of all batches involved
            // List <Int32>ListAllGiftBatchesToProcess = new List <int>();

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

            GLBatchTDS InspectDS = AInspectDS;
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable,
                ref Transaction,
                delegate
                {
                    if (ARecurringBatchTableInDataSet)
                    {
                        foreach (ARecurringBatchRow batch in InspectDS.ARecurringBatch.Rows)
                        {
                            if (batch.RowState == DataRowState.Deleted)
                            {
                                // need to use this way of retrieving data from deleted rows
                                LedgerNumber = (Int32)batch[ARecurringBatchTable.ColumnLedgerNumberId, DataRowVersion.Original];
                                BatchNumber = (Int32)batch[ARecurringBatchTable.ColumnBatchNumberId, DataRowVersion.Original];

                                // load all depending journals, transactions and attributes and make sure they are also deleted via the dataset
                                TemplateTransAnalAttribRow.LedgerNumber = LedgerNumber;
                                TemplateTransAnalAttribRow.BatchNumber = BatchNumber;
                                DeletedTransAnalAttribTable =
                                    ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                                for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedTransAnalAttribTable.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedTransAnalAttribTable);

                                TemplateTransactionRow.LedgerNumber = LedgerNumber;
                                TemplateTransactionRow.BatchNumber = BatchNumber;
                                ARecurringTransactionAccess.LoadUsingTemplate(DeletedDS, TemplateTransactionRow, Transaction);

                                for (Counter = DeletedDS.ARecurringTransaction.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedDS.ARecurringTransaction.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedDS.ARecurringTransaction);

                                TemplateJournalRow.LedgerNumber = LedgerNumber;
                                TemplateJournalRow.BatchNumber = BatchNumber;
                                ARecurringJournalAccess.LoadUsingTemplate(DeletedDS, TemplateJournalRow, Transaction);

                                for (Counter = DeletedDS.ARecurringJournal.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedDS.ARecurringJournal.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedDS.ARecurringJournal);
                            }
                        }
                    }

                    if (ARecurringJournalTableInDataSet)
                    {
                        foreach (ARecurringJournalRow journal in InspectDS.ARecurringJournal.Rows)
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
                                DeletedTransAnalAttribTable =
                                    ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                                for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedTransAnalAttribTable.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedTransAnalAttribTable);

                                TemplateTransactionRow.LedgerNumber = LedgerNumber;
                                TemplateTransactionRow.BatchNumber = BatchNumber;
                                TemplateTransactionRow.JournalNumber = JournalNumber;
                                ARecurringTransactionAccess.LoadUsingTemplate(DeletedDS, TemplateTransactionRow, Transaction);

                                for (Counter = DeletedDS.ARecurringTransaction.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedDS.ARecurringTransaction.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedDS.ARecurringTransaction);
                            }
                        }
                    }

                    if (ARecurringTransTableInDataSet)
                    {
                        foreach (ARecurringTransactionRow transaction in InspectDS.ARecurringTransaction.Rows)
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
                                DeletedTransAnalAttribTable =
                                    ARecurringTransAnalAttribAccess.LoadUsingTemplate(TemplateTransAnalAttribRow, Transaction);

                                for (Counter = DeletedTransAnalAttribTable.Count - 1; Counter >= 0; Counter--)
                                {
                                    DeletedTransAnalAttribTable.Rows[Counter].Delete();
                                }

                                InspectDS.Merge(DeletedTransAnalAttribTable);
                            }
                        }
                    }
                });

            // now submit the changes
            GLBatchTDSAccess.SubmitChanges(AInspectDS);

            SubmissionResult = TSubmitChangesResult.scrOK;

            if (ARecurringTransTableInDataSet)
            {
                //Accept deletion of Attributes to allow deletion of transactions
                if (ARecurringTransAnalTableInDataSet)
                {
                    AInspectDS.ARecurringTransAnalAttrib.AcceptChanges();
                }

                AInspectDS.ARecurringTransaction.AcceptChanges();

                if (AInspectDS.ARecurringTransaction.Count > 0)
                {
                    ARecurringTransactionRow tranR = (ARecurringTransactionRow)AInspectDS.ARecurringTransaction.Rows[0];

                    Int32 currentLedger = tranR.LedgerNumber;
                    Int32 currentBatch = tranR.BatchNumber;
                    Int32 currentJournal = tranR.JournalNumber;
                    Int32 transToDelete = 0;

                    try
                    {
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
                                TLogging.Log(String.Format("Recurring transaction to Delete: {0} from Journal: {1} in Batch: {2}",
                                        transToDelete,
                                        currentJournal,
                                        currentBatch));

                                transRowClient.Delete();
                            }

                            //save changes
                            GLBatchTDSAccess.SubmitChanges(AInspectDS);

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                    }
                    catch (Exception ex)
                    {
                        TLogging.Log("Saving DataSet: " + ex.Message);

                        TLogging.Log(String.Format("Error trying to save transaction: {0} in Journal: {1}, Batch: {2}",
                                transToDelete,
                                currentJournal,
                                currentBatch
                                ));

                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// reverse a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumberToReverse"></param>
        /// <param name="ADateForReversal"></param>
        /// <param name="AReversalBatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// <param name="AAutoPostReverseBatch"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool ReverseBatch(Int32 ALedgerNumber, Int32 ABatchNumberToReverse,
            DateTime ADateForReversal,
            out Int32 AReversalBatchNumber,
            out TVerificationResultCollection AVerifications,
            bool AAutoPostReverseBatch)
        {
            return TGLPosting.ReverseBatch(ALedgerNumber, ABatchNumberToReverse,
                ADateForReversal, out AReversalBatchNumber, out AVerifications, AAutoPostReverseBatch);
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        /// Returns true if it seems to be OK.
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            GLPostingTDS MainDS = null;
            TVerificationResultCollection Verifications = null;

            int BatchPeriod = -1;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;
            bool Success = false;

            try
            {
                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        Success = TGLPosting.TestPostGLBatch(ALedgerNumber,
                            ABatchNumber,
                            Transaction,
                            out Verifications,
                            out MainDS,
                            ref BatchPeriod);
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            AVerifications = Verifications;

            List <TVariant>Result = new List <TVariant>();

            if (Success)
            {
                MainDS.AGeneralLedgerMaster.DefaultView.RowFilter = string.Empty;
                MainDS.AGeneralLedgerMaster.DefaultView.Sort = AGeneralLedgerMasterTable.GetGlmSequenceDBName();
                MainDS.AAccount.DefaultView.RowFilter = string.Empty;
                MainDS.AAccount.DefaultView.Sort = AAccountTable.GetAccountCodeDBName();
                MainDS.ACostCentre.DefaultView.RowFilter = string.Empty;
                MainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();

                foreach (AGeneralLedgerMasterPeriodRow glmpRow in MainDS.AGeneralLedgerMasterPeriod.Rows)
                {
                    if ((glmpRow.PeriodNumber == BatchPeriod) && (glmpRow.RowState != DataRowState.Unchanged))
                    {
                        AGeneralLedgerMasterRow masterRow =
                            (AGeneralLedgerMasterRow)MainDS.AGeneralLedgerMaster.Rows.Find(glmpRow.GlmSequence);

                        #region Validate Data 1

                        if ((MainDS.AGeneralLedgerMaster == null) || (MainDS.AGeneralLedgerMaster.Count == 0) || (masterRow == null))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - General Ledger Master data for GLM sequence {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    glmpRow.GlmSequence));
                        }

                        #endregion Validate Data 1

                        ACostCentreRow ccRow = (ACostCentreRow)MainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, masterRow.CostCentreCode });

                        #region Validate Data 2

                        if ((MainDS.ACostCentre == null) || (MainDS.ACostCentre.Count == 0) || (ccRow == null))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Cost Centre data for Cost Centre {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    masterRow.CostCentreCode));
                        }

                        #endregion Validate Data 2

                        // only consider the posting cost centres
                        // TODO or consider only the top cost centre?
                        if (ccRow.PostingCostCentreFlag)
                        {
                            AAccountRow accRow =
                                (AAccountRow)
                                MainDS.AAccount.DefaultView[MainDS.AAccount.DefaultView.Find(masterRow.AccountCode)].Row;

                            #region Validate Data 3

                            if ((MainDS.AAccount == null) || (MainDS.AAccount.Count == 0) || (accRow == null))
                            {
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - Account data for Account {1} does not exist or could not be accessed!"),
                                        Utilities.GetMethodName(true),
                                        masterRow.AccountCode));
                            }

                            #endregion Validate Data 3

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
        ///
        /// </summary>
        /// <param name="AGLMainDS"></param>
        /// <param name="ARequestParams"></param>
        /// <param name="AExchangeRatesDictionary"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 SubmitRecurringGLBatch(ref GLBatchTDS AGLMainDS,
            Hashtable ARequestParams,
            ref Dictionary <string, decimal>AExchangeRatesDictionary,
            ref TVerificationResultCollection AVerifications)
        {
            Int32 NewGLBatchNumber = 0;

            //Copy parameters for use in Delegate
            GLBatchTDS RGLMainDS = AGLMainDS;

            Dictionary <string, decimal>ExchangeRatesDictionary = AExchangeRatesDictionary;

            TVerificationResultCollection Verifications = AVerifications;

            Int32 ALedgerNumber = (Int32)ARequestParams["ALedgerNumber"];
            Int32 ABatchNumber = (Int32)ARequestParams["ABatchNumber"];
            DateTime AEffectiveDate = (DateTime)ARequestParams["AEffectiveDate"];
            Decimal AExchangeRateIntlToBase = (Decimal)ARequestParams["AExchangeRateIntlToBase"];

            ALedgerRow LedgerRow = (ALedgerRow)AGLMainDS.ALedger[0];
            ARecurringBatchRow RBatchRow = (ARecurringBatchRow)AGLMainDS.ARecurringBatch[0];

            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }
            else if (RBatchRow == null)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The data for Ledger:{1} and Batch:{2} does not exist!"),
                        Utilities.GetMethodName(true),
                        ALedgerNumber,
                        ABatchNumber));
            }
            else if (AExchangeRateIntlToBase < 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The Exchange Rate from International currency to Base currency cannot be a negative number!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AExchangeRatesDictionary.Count == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - There are no exchange rates present for the Journals!"),
                        Utilities.GetMethodName(true)));
            }
            //Check the validity of the Journal and transaction numbering
            // This will also correct invalid LastJournal and LastTransaction numbers
            else if (!ValidateRecurringGLBatchJournalNumbering(ref AGLMainDS, ref RBatchRow, ref Verifications)
                     || !ValidateRecurringGLJournalTransactionNumbering(ref AGLMainDS, ref RBatchRow, ref Verifications))
            {
                return 0;
            }
            else if (AGLMainDS.ARecurringJournal.Count > 0)
            {
                string journalCurrencyCode = string.Empty;
                decimal journalCurrencyExchangeRateToBase;

                DataView rJournalDV = new DataView(AGLMainDS.ARecurringJournal);
                rJournalDV.RowFilter = string.Format("{0}={1} And {2}={3}",
                    ARecurringJournalTable.GetLedgerNumberDBName(),
                    ALedgerNumber,
                    ARecurringJournalTable.GetBatchNumberDBName(),
                    ABatchNumber);

                foreach (DataRowView drv in rJournalDV)
                {
                    ARecurringJournalRow rjr = (ARecurringJournalRow)drv.Row;

                    journalCurrencyCode = rjr.TransactionCurrency;
                    journalCurrencyExchangeRateToBase = 0;

                    if (!AExchangeRatesDictionary.TryGetValue(journalCurrencyCode, out journalCurrencyExchangeRateToBase)
                        || (rjr.ExchangeRateToBase <= 0))
                    {
                        Verifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot submit Recurring GL Batch: {0} in Ledger {1}"), ABatchNumber,
                                    ALedgerNumber),
                                String.Format(Catalog.GetString(
                                        "The Exchange Rate in Journal:{0} (with currency: {1}) to Base currency ({2}) does not exist or is 0!"),
                                    journalCurrencyCode, journalCurrencyCode, LedgerRow.BaseCurrency),
                                TResultSeverity.Resv_Critical));

                        break;
                    }
                }

                if (Verifications.Count > 0)
                {
                    return 0;
                }
            }

            #endregion Validate Arguments

            GLBatchTDS GLMainDS = new GLBatchTDS();
            ABatchRow BatchRow;

            int PeriodNumber, YearNr;
            bool TransactionInIntlCurrency = false;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum,
                    ref Transaction, ref SubmissionOK,
                    delegate
                    {
                        ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                        #region Validate Data 1

                        if ((LedgerTable == null) || (LedgerTable.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data 1

                        // Assuming all relevant data is loaded in RGLMainDS
                        foreach (ARecurringBatchRow recBatch in RGLMainDS.ARecurringBatch.Rows)
                        {
                            if ((recBatch.BatchNumber == ABatchNumber) && (recBatch.LedgerNumber == ALedgerNumber))
                            {
                                GLMainDS = CreateABatch(ALedgerNumber);

                                BatchRow = (ABatchRow)GLMainDS.ABatch.Rows[0];

                                //The new Batch number to be returned
                                NewGLBatchNumber = BatchRow.BatchNumber;

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
                                        string journalCurrencyCode = recJournal.TransactionCurrency;
                                        decimal journalCurrencyExchangeRateToBase = 0;
                                        ExchangeRatesDictionary.TryGetValue(journalCurrencyCode, out journalCurrencyExchangeRateToBase);

                                        // create the journal from recJournal
                                        AJournalRow JournalRow = GLMainDS.AJournal.NewRowTyped();
                                        JournalRow.LedgerNumber = BatchRow.LedgerNumber;
                                        JournalRow.BatchNumber = BatchRow.BatchNumber;
                                        JournalRow.JournalNumber = recJournal.JournalNumber;
                                        JournalRow.JournalDescription = recJournal.JournalDescription;
                                        JournalRow.SubSystemCode = recJournal.SubSystemCode;
                                        JournalRow.TransactionTypeCode = recJournal.TransactionTypeCode;
                                        JournalRow.TransactionCurrency = journalCurrencyCode;
                                        JournalRow.JournalCreditTotal = recJournal.JournalCreditTotal;
                                        JournalRow.JournalDebitTotal = recJournal.JournalDebitTotal;
                                        JournalRow.ExchangeRateToBase = journalCurrencyExchangeRateToBase;
                                        JournalRow.DateEffective = AEffectiveDate;
                                        JournalRow.JournalPeriod = recJournal.JournalPeriod;

                                        GLMainDS.AJournal.Rows.Add(JournalRow);

                                        if (JournalRow.JournalNumber > BatchRow.LastJournal)
                                        {
                                            BatchRow.LastJournal = JournalRow.JournalNumber;
                                        }

                                        TransactionInIntlCurrency = (JournalRow.TransactionCurrency == LedgerTable[0].IntlCurrency);
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
                                                TransactionRow.AmountInBaseCurrency =
                                                    GLRoutines.Divide(recTransaction.TransactionAmount, journalCurrencyExchangeRateToBase);

                                                if (!TransactionInIntlCurrency)
                                                {
                                                    TransactionRow.AmountInIntlCurrency = ((AExchangeRateIntlToBase == 0) ? 0 :
                                                                                           GLRoutines.Divide((decimal)TransactionRow.
                                                                                               AmountInBaseCurrency,
                                                                                               AExchangeRateIntlToBase));
                                                }
                                                else
                                                {
                                                    TransactionRow.AmountInIntlCurrency = TransactionRow.TransactionAmount;
                                                }

                                                TransactionRow.TransactionDate = AEffectiveDate;
                                                TransactionRow.DebitCreditIndicator = recTransaction.DebitCreditIndicator;
                                                TransactionRow.HeaderNumber = recTransaction.HeaderNumber;
                                                TransactionRow.DetailNumber = recTransaction.DetailNumber;
                                                TransactionRow.SubType = recTransaction.SubType;
                                                TransactionRow.Reference = recTransaction.Reference;

                                                GLMainDS.ATransaction.Rows.Add(TransactionRow);

                                                foreach (ARecurringTransAnalAttribRow recAnalAttrib in RGLMainDS.ARecurringTransAnalAttrib.Rows)
                                                {
                                                    if ((recAnalAttrib.TransactionNumber == recTransaction.TransactionNumber)
                                                        && (recTransaction.JournalNumber == recJournal.JournalNumber)
                                                        && (recTransaction.BatchNumber == ABatchNumber)
                                                        && (recTransaction.LedgerNumber == ALedgerNumber))
                                                    {
                                                        ATransAnalAttribRow TransAnalAttribRow = GLMainDS.ATransAnalAttrib.NewRowTyped();

                                                        TransAnalAttribRow.LedgerNumber = JournalRow.LedgerNumber;
                                                        TransAnalAttribRow.BatchNumber = JournalRow.BatchNumber;
                                                        TransAnalAttribRow.JournalNumber = JournalRow.JournalNumber;
                                                        TransAnalAttribRow.TransactionNumber = recTransaction.TransactionNumber;
                                                        TransAnalAttribRow.AnalysisTypeCode = recAnalAttrib.AnalysisTypeCode;

                                                        TransAnalAttribRow.AccountCode = recAnalAttrib.AccountCode;
                                                        TransAnalAttribRow.CostCentreCode = recAnalAttrib.CostCentreCode;
                                                        TransAnalAttribRow.AnalysisAttributeValue = recAnalAttrib.AnalysisAttributeValue;

                                                        GLMainDS.ATransAnalAttrib.Rows.Add(TransAnalAttribRow);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        ALedgerAccess.SubmitChanges(LedgerTable, Transaction);
                        ABatchAccess.SubmitChanges(GLMainDS.ABatch, Transaction);
                        AJournalAccess.SubmitChanges(GLMainDS.AJournal, Transaction);
                        ATransactionAccess.SubmitChanges(GLMainDS.ATransaction, Transaction);
                        ATransAnalAttribAccess.SubmitChanges(GLMainDS.ATransAnalAttrib, Transaction);

                        SubmissionOK = true;
                    });

                GLMainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            AVerifications = Verifications;

            return NewGLBatchNumber;
        }

        private static bool ValidateRecurringGLBatchJournalNumbering(ref GLBatchTDS AGLBatch,
            ref ARecurringBatchRow ARecurringBatchToSubmit,
            ref TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if ((AGLBatch.ARecurringBatch == null) || (AGLBatch.ARecurringBatch.Count == 0) || (ARecurringBatchToSubmit == null))
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - No Recurring GL Batch data is present!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Default to most likely outcome
            bool NumberingIsValid = true;

            string SQLStatement = string.Empty;
            string TempTableName = "TempCheckForConsecutiveRecurringJournals";

            //Parameters for SQL as strings
            string prmLedgerNumber = ARecurringBatchToSubmit.LedgerNumber.ToString();
            string prmBatchNumber = ARecurringBatchToSubmit.BatchNumber.ToString();

            //Tables with alias
            string BatchTableAlias = "b";
            string bBatchTable = ARecurringBatchTable.GetTableDBName() + " " + BatchTableAlias;
            string JournalTableAlias = "j";
            string jJournalTable = ARecurringJournalTable.GetTableDBName() + " " + JournalTableAlias;

            //Table: ABudgetTable and fields
            string bLedgerNumber = BatchTableAlias + "." + ARecurringBatchTable.GetLedgerNumberDBName();
            string bBatchNumber = BatchTableAlias + "." + ARecurringBatchTable.GetBatchNumberDBName();
            string bBatchNumberAlias = "BatchNumber";
            string bBatchLastJournal = BatchTableAlias + "." + ARecurringBatchTable.GetLastJournalDBName();
            string bBatchLastJournalAlias = "BatchLastJournal";
            string jLedgerNumber = JournalTableAlias + "." + ARecurringJournalTable.GetLedgerNumberDBName();
            string jBatchNumber = JournalTableAlias + "." + ARecurringJournalTable.GetBatchNumberDBName();
            string jJournalNumber = JournalTableAlias + "." + ARecurringJournalTable.GetJournalNumberDBName();
            string jFirstJournalAlias = "FirstJournal";
            string jLastJournalAlias = "LastJournal";
            string jCountJournalAlias = "CountJournal";

            try
            {
                DataTable tempTable = AGLBatch.Tables.Add(TempTableName);
                tempTable.Columns.Add(bBatchNumberAlias, typeof(Int32));
                tempTable.Columns.Add(bBatchLastJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jFirstJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jLastJournalAlias, typeof(Int32));
                tempTable.Columns.Add(jCountJournalAlias, typeof(Int32));

                SQLStatement = "SELECT " + bBatchNumber + " " + bBatchNumberAlias + "," +
                               "      MIN(" + bBatchLastJournal + ") " + bBatchLastJournalAlias + "," +
                               "      COALESCE(MIN(" + jJournalNumber + "), 0) " + jFirstJournalAlias + "," +
                               "      COALESCE(MAX(" + jJournalNumber + "), 0) " + jLastJournalAlias + "," +
                               "      Count(" + jJournalNumber + ") " + jCountJournalAlias +
                               " FROM " + bBatchTable + " LEFT OUTER JOIN " + jJournalTable +
                               "        ON " + bLedgerNumber + " = " + jLedgerNumber +
                               "         AND " + bBatchNumber + " = " + jBatchNumber +
                               " WHERE " + bLedgerNumber + " = " + prmLedgerNumber +
                               "   AND " + bBatchNumber + " = " + prmBatchNumber +
                               " GROUP BY " + bBatchNumber + ";";

                GLBatchTDS GLBatch = AGLBatch;
                TDBTransaction transaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                    delegate
                    {
                        DBAccess.GDBAccessObj.Select(GLBatch, SQLStatement, TempTableName, transaction);
                    });

                //As long as Batches exist, rows will be returned
                int numTempRows = AGLBatch.Tables[TempTableName].Rows.Count;

                DataView tempDV = new DataView(AGLBatch.Tables[TempTableName]);

                //Confirm are all equal and correct - the most common
                tempDV.RowFilter = string.Format("{0}={1} And {1}={2} And {0}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                //If all records are correct, nothing to do
                if (tempDV.Count == numTempRows)
                {
                    return NumberingIsValid;
                }

                //!!Reaching this point means there are issues that need addressing.

                //Confirm that no negative numbers exist
                tempDV.RowFilter = string.Format("{0} < 0 Or {1} < 0",
                    bBatchLastJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Recurring Batches have a negative LastJournalNumber or have Journals with a negative JournalNumber!";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1}",
                            Environment.NewLine,
                            drv[bBatchNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot submit Batch {0} in Ledger {1}"), ARecurringBatchToSubmit.BatchNumber,
                                ARecurringBatchToSubmit.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //Display non-sequential journals
                tempDV.RowFilter = string.Format("{2}>0 And ({3}<>1 Or {1}<>{2})",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Recurring Batches have gaps in their Journal numbering! You will need to cancel the Batch(es) and recreate:";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1}",
                            Environment.NewLine,
                            drv[bBatchNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot submit Recurring Batch {0} in Ledger {1}"), ARecurringBatchToSubmit.BatchNumber,
                                ARecurringBatchToSubmit.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //The next most likely, is where the BatchLastJournal needs updating
                //Display mismatched journal last number
                tempDV.RowFilter = string.Format("{0}<>{1} And {1}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    bBatchLastJournalAlias,
                    jLastJournalAlias,
                    jCountJournalAlias,
                    jFirstJournalAlias);

                if (tempDV.Count > 0)
                {
                    ARecurringBatchToSubmit.LastJournal = Convert.ToInt32(tempDV[0][jLastJournalAlias]);
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (AGLBatch.Tables.Contains(TempTableName))
                {
                    AGLBatch.Tables.Remove(TempTableName);
                }
            }

            return NumberingIsValid;
        }

        private static bool ValidateRecurringGLJournalTransactionNumbering(ref GLBatchTDS AGLBatch,
            ref ARecurringBatchRow ARecurringBatchToSubmit,
            ref TVerificationResultCollection AVerifications)
        {
            #region Validate Arguments

            if (AGLBatch.ARecurringJournal == null)
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - No Recurring GL Journal data is present!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            //Default to most likely outcome
            bool NumberingIsValid = true;

            string SQLStatement = string.Empty;
            string TempTableName = "TempCheckForConsecutiveRecurringTransactions";

            //Parameters for SQL as strings
            string prmLedgerNumber = ARecurringBatchToSubmit.LedgerNumber.ToString();
            string prmBatchNumber = ARecurringBatchToSubmit.BatchNumber.ToString();

            //Tables with alias
            string JournalTableAlias = "j";
            string jJournalTable = ARecurringJournalTable.GetTableDBName() + " " + JournalTableAlias;

            string TransactionTableAlias = "t";
            string tTransactionTable = ARecurringTransactionTable.GetTableDBName() + " " + TransactionTableAlias;

            //Fields and Aliases
            string jLedgerNumber = JournalTableAlias + "." + ARecurringJournalTable.GetLedgerNumberDBName();
            string jBatchNumber = JournalTableAlias + "." + ARecurringJournalTable.GetBatchNumberDBName();
            string jBatchNumberAlias = "BatchNumber";
            string jJournalNumber = JournalTableAlias + "." + ARecurringJournalTable.GetJournalNumberDBName();
            string jJournalNumberAlias = "JournalNumber";
            string jJournalLastTransaction = JournalTableAlias + "." + ARecurringJournalTable.GetLastTransactionNumberDBName();
            string jJournalLastTransactionAlias = "JournalLastTransaction";

            string tLedgerNumber = TransactionTableAlias + "." + ARecurringTransactionTable.GetLedgerNumberDBName();
            string tBatchNumber = TransactionTableAlias + "." + ARecurringTransactionTable.GetBatchNumberDBName();
            string tJournalNumber = TransactionTableAlias + "." + ARecurringTransactionTable.GetJournalNumberDBName();
            string tTransactionNumber = TransactionTableAlias + "." + ARecurringTransactionTable.GetTransactionNumberDBName();
            string tFirstTransactionAlias = "FirstTransaction";
            string tLastTransactionAlias = "LastTransaction";
            string tCountTransactionAlias = "CountTransaction";

            try
            {
                DataTable tempTable = AGLBatch.Tables.Add(TempTableName);
                tempTable.Columns.Add(jBatchNumberAlias, typeof(Int32));
                tempTable.Columns.Add(jJournalNumberAlias, typeof(Int32));
                tempTable.Columns.Add(jJournalLastTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tFirstTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tLastTransactionAlias, typeof(Int32));
                tempTable.Columns.Add(tCountTransactionAlias, typeof(Int32));

                SQLStatement = "SELECT " + jBatchNumber + " " + jBatchNumberAlias + ", " + jJournalNumber + " " + jJournalNumberAlias + "," +
                               "      MIN(" + jJournalLastTransaction + ") " + jJournalLastTransactionAlias + "," +
                               "      COALESCE(MIN(" + tTransactionNumber + "), 0) " + tFirstTransactionAlias + "," +
                               "      COALESCE(MAX(" + tTransactionNumber + "), 0) " + tLastTransactionAlias + "," +
                               "      Count(" + tTransactionNumber + ") " + tCountTransactionAlias +
                               " FROM " + jJournalTable + " LEFT OUTER JOIN " + tTransactionTable +
                               "        ON " + jLedgerNumber + " = " + tLedgerNumber +
                               "         AND " + jBatchNumber + " = " + tBatchNumber +
                               "         AND " + jJournalNumber + " = " + tJournalNumber +
                               " WHERE " + jLedgerNumber + " = " + prmLedgerNumber +
                               "   AND " + jBatchNumber + " = " + prmBatchNumber +
                               " GROUP BY " + jBatchNumber + ", " + jJournalNumber + ";";

                GLBatchTDS GLBatch = AGLBatch;
                TDBTransaction transaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref transaction,
                    delegate
                    {
                        DBAccess.GDBAccessObj.Select(GLBatch, SQLStatement, TempTableName, transaction);
                    });


                //As long as Batches exist, rows will be returned
                int numTempRows = AGLBatch.Tables[TempTableName].Rows.Count;

                DataView tempDV = new DataView(AGLBatch.Tables[TempTableName]);

                //Confirm are all equal and correct - the most common
                tempDV.RowFilter = string.Format("{0}={1} And {1}={2} And {0}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                //If all records are correct, nothing to do
                if (tempDV.Count == numTempRows)
                {
                    return NumberingIsValid;
                }

                //!!Reaching this point means there are issues that need addressing.

                //Confirm that no negative numbers exist
                tempDV.RowFilter = string.Format("{0} < 0 Or {1} < 0",
                    jJournalLastTransactionAlias,
                    tFirstTransactionAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Recurring Journals have a negative LastTransactionNumber or have Transactions with a negative TransactionNumber!";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1} Journal:{2}",
                            Environment.NewLine,
                            drv[jBatchNumberAlias],
                            drv[jJournalNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot submit Recurring Batch {0} in Ledger {1}"), ARecurringBatchToSubmit.BatchNumber,
                                ARecurringBatchToSubmit.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //Display non-sequential transactions
                tempDV.RowFilter = string.Format("{2}>0 And ({3}<>1 Or {1}<>{2})",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                if (tempDV.Count > 0)
                {
                    string errMessage =
                        "The following Recurring Journals have gaps in their Transaction numbering! You will need to cancel the Journal(s) and recreate:";

                    foreach (DataRowView drv in tempDV)
                    {
                        errMessage += string.Format("{0}Batch:{1} Journal:{2}",
                            Environment.NewLine,
                            drv[jBatchNumberAlias],
                            drv[jJournalNumberAlias]);
                    }

                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot submit Recurring Batch {0} in Ledger {1}"), ARecurringBatchToSubmit.BatchNumber,
                                ARecurringBatchToSubmit.LedgerNumber),
                            errMessage,
                            TResultSeverity.Resv_Critical));

                    return false;
                }

                //The next most likely, is where the JournalLastTransaction needs updating
                //Display mismatched journal last number
                tempDV.RowFilter = string.Format("{0}<>{1} And {1}={2} And (({2}>0 And {3}=1) Or ({2}=0 And {3}=0))",
                    jJournalLastTransactionAlias,
                    tLastTransactionAlias,
                    tCountTransactionAlias,
                    tFirstTransactionAlias);

                DataView journalsDV = new DataView(AGLBatch.ARecurringJournal);

                if (tempDV.Count > 0)
                {
                    //This means the LastTransactionNumber field needs to be updated and is incorrect
                    foreach (DataRowView drv in tempDV)
                    {
                        journalsDV.RowFilter = String.Format("{0}={1} And {2}={3}",
                            ARecurringJournalTable.GetBatchNumberDBName(),
                            drv[jBatchNumberAlias],
                            ARecurringJournalTable.GetJournalNumberDBName(),
                            drv[jJournalNumberAlias]);

                        foreach (DataRowView journals in journalsDV)
                        {
                            ARecurringJournalRow journalRow = (ARecurringJournalRow)journals.Row;
                            journalRow.LastTransactionNumber = Convert.ToInt32(drv[tLastTransactionAlias]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }
            finally
            {
                if (AGLBatch.Tables.Contains(TempTableName))
                {
                    AGLBatch.Tables.Remove(TempTableName);
                }
            }

            return NumberingIsValid;
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
        /// Gets daily exchange rate for the given currencies and date.  There is no limit on how 'old' the rate can be.
        /// If more than one rate exists on or before the specified date the latest one is returned.  This might be old or it might
        /// be one of several on the same day.
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
        /// Gets daily exchange rate for the given currencies and date. The APriorDaysAllwed parameter limits how 'old' the rate can be.
        /// The unique rate parameter can ensure that a rate is only returned if there is only one to choose from.
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="APriorDaysAllowed">Sets a limit on how many days prior to ADateEffective to search.  Use -1 for no limit,
        /// 0 to imply that the rate must match for the specified date, 1 for the date and the day before and so on.</param>
        /// <param name="AEnforceUniqueRate">If true the method will only return a value if there is one unique rate in the date range.
        /// Otherwise it returns the latest rate.</param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static decimal GetDailyExchangeRate(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime ADateEffective,
            int APriorDaysAllowed,
            Boolean AEnforceUniqueRate)
        {
            return TExchangeRateTools.GetDailyExchangeRate(ACurrencyFrom, ACurrencyTo, ADateEffective, APriorDaysAllowed, AEnforceUniqueRate);
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
        public static bool ExportAllGLBatchData(ArrayList batches, Hashtable requestParams, out String exportString)
        {
            TGLExporting Exporting = new TGLExporting();

            return Exporting.ExportAllGLBatchData(batches, requestParams, out exportString);
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
            TGLImporting Importing = new TGLImporting();

            return Importing.ImportGLBatches(requestParams, importString, out AMessages);
        }

        /// <summary>
        /// Import GL transaction data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AMessages"></param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportGLTransactions(
            Hashtable ARequestParams,
            String AImportString,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            out TVerificationResultCollection AMessages
            )
        {
            TGLImporting Importing = new TGLImporting();

            return Importing.ImportGLTransactions(ARequestParams, AImportString, ALedgerNumber, ABatchNumber, AJournalNumber, out AMessages);
        }

        /// <summary>
        /// Get current accounts and their current balances for use in the Reallocation Journal dialog
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAccountsForReallocationJournal(int ALedgerNumber, int APeriodNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

            TDBTransaction Transaction = null;
            DataTable NewTable = new DataTable("NewTable");

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable,
                ref Transaction,
                delegate
                {
                    string sqlQuery =
                        "SELECT PUB_a_general_ledger_master.a_cost_centre_code_c, PUB_a_general_ledger_master.a_account_code_c, " +
                        "PUB_a_general_ledger_master_period.a_actual_base_n, " +
                        "PUB_a_account.a_account_code_short_desc_c, PUB_a_account.a_debit_credit_indicator_l, " +
                        "PUB_a_cost_centre.a_cost_centre_name_c " +

                        "FROM PUB_a_general_ledger_master, PUB_a_general_ledger_master_period, PUB_a_account, PUB_a_cost_centre " +

                        "WHERE PUB_a_general_ledger_master.a_ledger_number_i = " + ALedgerNumber + " AND " +

                        "PUB_a_account.a_account_code_c = PUB_a_general_ledger_master.a_account_code_c AND " +
                        "PUB_a_account.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i AND " +
                        "PUB_a_account.a_posting_status_l = 1 AND " +
                        "PUB_a_account.a_account_active_flag_l = 1 AND " +

                        "PUB_a_cost_centre.a_cost_centre_code_c = PUB_a_general_ledger_master.a_cost_centre_code_c AND " +
                        "PUB_a_cost_centre.a_ledger_number_i = PUB_a_general_ledger_master.a_ledger_number_i AND " +
                        "PUB_a_cost_centre.a_posting_cost_centre_flag_l = 1 AND " +
                        "PUB_a_cost_centre.a_cost_centre_active_flag_l = 1 AND " +

                        "PUB_a_general_ledger_master_period.a_glm_sequence_i = PUB_a_general_ledger_master.a_glm_sequence_i AND " +
                        "PUB_a_general_ledger_master_period.a_period_number_i = " + APeriodNumber;

                    NewTable = DBAccess.GDBAccessObj.SelectDT(sqlQuery, "NewTable", Transaction);
                });

            // create a new description
            foreach (DataRow Row in NewTable.Rows)
            {
                Row["a_account_code_short_desc_c"] += ", " + Row["a_cost_centre_name_c"];
            }

            return NewTable;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool UpdateBatchTotalsWithLoad(ref GLBatchTDS AMainDS,
            Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            bool AmountsUpdated = false;

            //Take a copy of the dataset but with specified batch only
            GLBatchTDS SingleBatchDS = GLRoutines.SingleBatchOnlyDataSet(ref AMainDS, ALedgerNumber, ABatchNumber);

            #region Validate Data

            if (SingleBatchDS.ABatch.Count == 0)
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - GL Batch data for Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true),
                        ABatchNumber,
                        ALedgerNumber));
            }
            else if (SingleBatchDS.ABatch[0].BatchStatus != MFinanceConstants.BATCH_UNPOSTED)
            {
                TLogging.Log(String.Format("Function:{0} - Tried to update totals for non-Unposted Batch:{1}",
                        Utilities.GetMethodName(true),
                        SingleBatchDS.ABatch[0].BatchNumber));
                return false;
            }

            #endregion Validate Data

            //Assign current Batch Row
            ABatchRow CurrentBatchRow = (ABatchRow)SingleBatchDS.ABatch[0];

            if (SingleBatchDS.AJournal.Count == 0)
            {
                //Try to load all data
                SingleBatchDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, ABatchNumber));
            }
            else if (SingleBatchDS.ATransaction.Count == 0)
            {
                //Try to load all data
                SingleBatchDS.Merge(LoadATransactionForBatch(ALedgerNumber, ABatchNumber));
            }

            SingleBatchDS.AcceptChanges();

            AmountsUpdated = GLRoutines.UpdateBatchTotals(ref SingleBatchDS, ref CurrentBatchRow);

            if (AmountsUpdated)
            {
                SingleBatchDS.AcceptChanges();
                AMainDS.Merge(SingleBatchDS);
            }

            return AmountsUpdated;
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool UpdateRecurringBatchTotalsWithLoad(ref GLBatchTDS AMainDS,
            Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The Recurring GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABatchNumber <= 0)
            {
                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber, ABatchNumber);
            }

            #endregion Validate Arguments

            bool AmountsUpdated = false;

            //Take a copy of the dataset but with specified batch only
            GLBatchTDS SingleBatchDS = GLRoutines.SingleBatchOnlyDataSet(ref AMainDS, ALedgerNumber, ABatchNumber);

            #region Validate Data

            if (SingleBatchDS.ARecurringBatch.Count == 0)
            {
                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                            "Function:{0} - GL Batch data for Batch {1} in Ledger {2} does not exist or could not be accessed!"),
                        Utilities.GetMethodName(true),
                        ABatchNumber,
                        ALedgerNumber));
            }

            #endregion Validate Data

            //Assign
            ARecurringBatchRow CurrentRecurringBatchRow = SingleBatchDS.ARecurringBatch[0];

            if (SingleBatchDS.ARecurringJournal.Count == 0)
            {
                //Try to load all data
                SingleBatchDS.Merge(LoadARecurringBatchARecurJournalARecurTransaction(ALedgerNumber, ABatchNumber));
            }
            else if (SingleBatchDS.ARecurringTransaction.Count == 0)
            {
                //Try to load all data
                SingleBatchDS.Merge(LoadARecurringTransaction(ALedgerNumber, ABatchNumber));
            }

            SingleBatchDS.AcceptChanges();

            AmountsUpdated = GLRoutines.UpdateRecurringBatchTotals(ref SingleBatchDS, ref CurrentRecurringBatchRow);

            if (AmountsUpdated)
            {
                SingleBatchDS.AcceptChanges();
                AMainDS.Merge(SingleBatchDS);
            }

            return AmountsUpdated;
        }
    }
}