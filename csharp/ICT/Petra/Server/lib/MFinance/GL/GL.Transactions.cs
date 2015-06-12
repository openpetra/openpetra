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
using System.Data;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Common.Exceptions;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadABatchAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadAJournalAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadARecurringJournalAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadARecurringTransactionAndContent(Int32 ALedgerNumber, Int32 ABatchNumber)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadATransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        public static GLBatchTDS LoadATransactionATransAnalAttrib(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
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
                string analAttrList = string.Empty;

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

                foreach (GLBatchTDSATransactionRow transRow in MainDS.ATransaction.Rows)
                {
                    MainDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                        ATransAnalAttribTable.GetTransactionNumberDBName(),
                        transRow.TransactionNumber);

                    foreach (DataRowView rv in MainDS.ATransAnalAttrib.DefaultView)
                    {
                        ATransAnalAttribRow Row = (ATransAnalAttribRow)rv.Row;

                        if (analAttrList.Length > 0)
                        {
                            analAttrList += ", ";
                        }

                        analAttrList += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                    }

                    if (transRow.AnalysisAttributes != analAttrList)
                    {
                        transRow.AnalysisAttributes = analAttrList;
                    }

                    //reset the attributes string
                    analAttrList = string.Empty;
                }

                MainDS.ATransAnalAttrib.DefaultView.RowFilter = string.Empty;

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                        AAnalysisTypeAccess.LoadAll(MainDS, Transaction);

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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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

            string AnalAttrList = string.Empty;

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

                foreach (GLBatchTDSARecurringTransactionRow transRow in MainDS.ARecurringTransaction.Rows)
                {
                    MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = String.Format("{0} = {1}",
                        ARecurringTransAnalAttribTable.GetTransactionNumberDBName(),
                        transRow.TransactionNumber);

                    foreach (DataRowView rv in MainDS.ARecurringTransAnalAttrib.DefaultView)
                    {
                        ARecurringTransAnalAttribRow Row = (ARecurringTransAnalAttribRow)rv.Row;

                        if (AnalAttrList.Length > 0)
                        {
                            AnalAttrList += ", ";
                        }

                        AnalAttrList += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                    }

                    transRow.AnalysisAttributes = AnalAttrList;

                    //clear the attributes string and table
                    AnalAttrList = string.Empty;
                }

                MainDS.ARecurringTransAnalAttrib.DefaultView.RowFilter = string.Empty;

                MainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
            if (RecurrGLBatchTableInDataSet || RecurrGLJournalTableInDataSet || RecurrGLTransTableInDataSet)
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            AVerificationResult = VerificationResult;

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

            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrOK;

            if (AllValidationsOK)
            {
                //Need to save changes before deleting any transactions
                GLBatchTDSAccess.SubmitChanges(AInspectDS);

                if (GLTransTableInDataSet && (AInspectDS.ATransaction.Rows.Count > 0))
                {
                    //Accept deletion of Attributes to allow deletion of transactions
                    if (GLTransAttrTableInDataSet)
                    {
                        AInspectDS.ATransAnalAttrib.AcceptChanges();
                    }

                    AInspectDS.ATransaction.AcceptChanges();

                    if (AInspectDS.ATransaction.Count > 0)
                    {
                        ATransactionRow tranR = (ATransactionRow)AInspectDS.ATransaction.Rows[0];

                        Int32 currentLedger = tranR.LedgerNumber;
                        Int32 currentBatch = tranR.BatchNumber;
                        Int32 currentJournal = tranR.JournalNumber;
                        Int32 transToDelete = 0;

                        try
                        {
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

                                //Submit all changes
                                GLBatchTDSAccess.SubmitChanges(AInspectDS);

                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                        }
                        catch (Exception ex)
                        {
                            TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                                    Utilities.GetMethodSignature(),
                                    Environment.NewLine,
                                    ex.Message));
                            throw ex;
                        }
                    }
                }
            }

            return SubmissionResult;
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
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
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
        /// create a GL batch from a recurring GL batch
        /// including journals, transactions and attributes
        /// </summary>
        /// <param name="requestParams">HashTable with many parameters</param>
        [RequireModulePermission("FINANCE-1")]
        public static void SubmitRecurringGLBatch(Hashtable requestParams)
        {
            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Int32 ABatchNumber = (Int32)requestParams["ABatchNumber"];
            DateTime AEffectiveDate = (DateTime)requestParams["AEffectiveDate"];
            Decimal AExchangeRateToBase = (Decimal)requestParams["AExchangeRateToBase"];;

            Decimal AExchangeRateIntlToBase = (Decimal)requestParams["AExchangeRateIntlToBase"];

            #region Validate Hashtable Argument

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
            else if (AExchangeRateToBase <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The Exchange Rate from Journal currency to Base currency must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AExchangeRateIntlToBase <= 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString(
                            "Function:{0} - The Exchange Rate from International currency to Base currency must be greater than 0!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Hashtable Argument

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

                        #region Validate Data

                        if ((LedgerTable == null) || (LedgerTable.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        // make sure that recurring GL batch is fully loaded, including journals, transactions and attributes
                        GLBatchTDS RGLMainDS = LoadARecurringBatchAndContent(ALedgerNumber, ABatchNumber);

                        // Assuming all relevant data is loaded in FMainDS
                        foreach (ARecurringBatchRow recBatch in RGLMainDS.ARecurringBatch.Rows)
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
                                        JournalRow.ExchangeRateToBase = AExchangeRateToBase;
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
                                                    GLRoutines.Divide(recTransaction.TransactionAmount, AExchangeRateToBase);

                                                if (!TransactionInIntlCurrency)
                                                {
                                                    TransactionRow.AmountInIntlCurrency =
                                                        GLRoutines.Divide((decimal)TransactionRow.AmountInBaseCurrency,
                                                            AExchangeRateIntlToBase);
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

                        ABatchAccess.SubmitChanges(GLMainDS.ABatch, Transaction);
                        ALedgerAccess.SubmitChanges(LedgerTable, Transaction);
                        AJournalAccess.SubmitChanges(GLMainDS.AJournal, Transaction);
                        ATransactionAccess.SubmitChanges(GLMainDS.ATransaction, Transaction);
                        ATransAnalAttribAccess.SubmitChanges(GLMainDS.ATransAnalAttrib, Transaction);

                        SubmissionOK = true;
                    });

                GLMainDS.AcceptChanges();
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }
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
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        ///   Assumes that the dataset has all batch, journal and transaction data loaded.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ACurrentBatch"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void UpdateTotalsOfBatch(ref GLBatchTDS AMainDS,
            ABatchRow ACurrentBatch)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentBatch == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The current GL Batch row does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            decimal SumDebits = 0.0M;
            decimal SumCredits = 0.0M;

            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ACurrentBatch.BatchNumber);

            foreach (DataRowView journalview in JournalDV)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                UpdateTotalsOfJournal(ref AMainDS, journalrow);

                SumDebits += journalrow.JournalDebitTotal;
                SumCredits += journalrow.JournalCreditTotal;
            }

            ACurrentBatch.BatchDebitTotal = SumDebits;
            ACurrentBatch.BatchCreditTotal = SumCredits;
            ACurrentBatch.BatchRunningTotal = Math.Round(SumDebits - SumCredits, 2);
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        ///   Assumes that the dataset has all batch, and journal data loaded.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void UpdateTotalsOfBatchFromJournals(ref GLBatchTDS AMainDS,
            int ABatchNumber, int AJournalNumber = 0)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((AMainDS.ABatch == null) || (AMainDS.ABatch.Count == 0))
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The GL Batch table is null or empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ABatchNumber <= 0)
            {
                int ledgerNumber = ((ABatchRow)AMainDS.ABatch.Rows[0]).LedgerNumber;

                throw new EFinanceSystemInvalidBatchNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Batch number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ledgerNumber, ABatchNumber);
            }
            else if (AJournalNumber < 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Journal number cannot be a negative number!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

            int CurrentLedger = ((ABatchRow)AMainDS.ABatch.Rows[0]).LedgerNumber;

            decimal SumDebits = 0.0M;
            decimal SumCredits = 0.0M;

            //Locate the current Batch
            ABatchRow BatchRow = (ABatchRow)AMainDS.ABatch.Rows.Find(new object[] { CurrentLedger, ABatchNumber });

            if (BatchRow == null)
            {
                return;
            }

            //If journal number is given, then update the specified journal according to the transaction values in the dataset
            if (AJournalNumber > 0)
            {
                GLBatchTDSAJournalRow journalRow = (GLBatchTDSAJournalRow)AMainDS.AJournal.Rows.Find(new object[] { CurrentLedger, ABatchNumber,
                                                                                                                    AJournalNumber });

                if (journalRow != null)
                {
                    //Update the totals of the current journal from the transactions.
                    UpdateTotalsOfJournal(ref AMainDS, journalRow);
                }
            }

            //Iterate through all journals in the current Batch
            DataView JournalDV = new DataView(AMainDS.AJournal);

            JournalDV.RowFilter = String.Format("{0}={1}",
                AJournalTable.GetBatchNumberDBName(),
                ABatchNumber);

            //Recalculate the Batch totals
            foreach (DataRowView journalview in JournalDV)
            {
                GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                SumDebits += journalrow.JournalDebitTotal;
                SumCredits += journalrow.JournalCreditTotal;
            }

            BatchRow.BatchDebitTotal = SumDebits;
            BatchRow.BatchCreditTotal = SumCredits;
            BatchRow.BatchRunningTotal = Math.Round(SumDebits - SumCredits, 2);
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the journals and the current batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void UpdateTotalsOfBatch(Int32 ALedgerNumber, Int32 ABatchNumber)
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

            //TVerificationResultCollection AVerificationResult = new TVerificationResultCollection();
            GLBatchTDS BatchDS = new GLBatchTDS();

            decimal SumDebits = 0.0M;
            decimal SumCredits = 0.0M;

            //Load all Batch, Journal and Transaction records
            BatchDS.Merge(LoadABatchAJournalATransaction(ALedgerNumber, ABatchNumber));

            try
            {
                ABatchRow currentBatch = (ABatchRow)BatchDS.ABatch.Rows[0];

                BatchDS.AJournal.DefaultView.RowFilter = string.Empty;

                foreach (DataRowView journalview in BatchDS.AJournal.DefaultView)
                {
                    GLBatchTDSAJournalRow journalrow = (GLBatchTDSAJournalRow)journalview.Row;

                    UpdateTotalsOfJournal(ref BatchDS, journalrow);

                    SumDebits += journalrow.JournalDebitTotal;
                    SumCredits += journalrow.JournalCreditTotal;
                }

                currentBatch.BatchDebitTotal = SumDebits;
                currentBatch.BatchCreditTotal = SumCredits;
                currentBatch.BatchRunningTotal = Math.Round(SumDebits - SumCredits, 2);

                GLBatchTDSAccess.SubmitChanges(BatchDS);
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Calculate the base amount for the transactions, and update the totals for the current journal
        ///   Assumes that transactions are already loaded into the Dataset
        /// </summary>
        /// <param name="AMainDS">ATransactions are filtered on current journal</param>
        /// <param name="ACurrentJournal"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void UpdateTotalsOfJournal(ref GLBatchTDS AMainDS,
            GLBatchTDSAJournalRow ACurrentJournal)
        {
            #region Validate Arguments

            if (AMainDS == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString("Function:{0} - The GL Batch dataset is null!"),
                        Utilities.GetMethodName(true)));
            }
            else if (ACurrentJournal == null)
            {
                throw new EFinanceSystemDataObjectNullOrEmptyException(String.Format(Catalog.GetString(
                            "Function:{0} - The current GL Journal row does not exist or is empty!"),
                        Utilities.GetMethodName(true)));
            }
            else if ((ACurrentJournal.ExchangeRateToBase == 0.0m)
                     && (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString()))
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - Batch {1} Journal {2} has invalid exchange rate to base!"),
                        Utilities.GetMethodName(true),
                        ACurrentJournal.BatchNumber,
                        ACurrentJournal.JournalNumber));
            }

            #endregion Validate Arguments

            ACurrentJournal.JournalDebitTotal = 0.0M;
            ACurrentJournal.JournalDebitTotalBase = 0.0M;
            ACurrentJournal.JournalCreditTotal = 0.0M;
            ACurrentJournal.JournalCreditTotalBase = 0.0M;

            DataView TransactionDV = new DataView(AMainDS.ATransaction);

            TransactionDV.RowFilter = String.Format("{0} = {1} And {2} = {3}",
                ATransactionTable.GetBatchNumberDBName(),
                ACurrentJournal.BatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                ACurrentJournal.JournalNumber);

            // transactions are filtered for this journal; add up the total amounts
            foreach (DataRowView tdrv in TransactionDV)
            {
                ATransactionRow trn = (ATransactionRow)tdrv.Row;

                // recalculate the amount in base currency

                if (ACurrentJournal.TransactionTypeCode != CommonAccountingTransactionTypesEnum.REVAL.ToString())
                {
                    trn.AmountInBaseCurrency = GLRoutines.Divide(trn.TransactionAmount, ACurrentJournal.ExchangeRateToBase);
                }

                if (trn.DebitCreditIndicator)
                {
                    ACurrentJournal.JournalDebitTotal += trn.TransactionAmount;
                    ACurrentJournal.JournalDebitTotalBase += trn.AmountInBaseCurrency;
                }
                else
                {
                    ACurrentJournal.JournalCreditTotal += trn.TransactionAmount;
                    ACurrentJournal.JournalCreditTotalBase += trn.AmountInBaseCurrency;
                }
            }
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

        //Currently not used
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

        //Currently not used
        private static void CheckTransAnalysisAttributes(ref GLBatchTDS AInspectDS,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            ref TSubmitChangesResult ASubmissionResult)
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

                GLBatchTDSAccess.SubmitChanges(AInspectDS);

                ASubmissionResult = TSubmitChangesResult.scrOK;

                AInspectDS.ATransAnalAttrib.AcceptChanges();
            }

            transAnalAttrView.RowFilter = string.Empty;
        }

        //Currently not used
        private static void CheckRecurringTransAnalysisAttributes(ref GLBatchTDS AInspectDS,
            int ALedgerNumber,
            int ABatchNumber,
            int AJournalNumber,
            ref TSubmitChangesResult ASubmissionResult)
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

                GLBatchTDSAccess.SubmitChanges(AInspectDS);

                ASubmissionResult = TSubmitChangesResult.scrOK;

                AInspectDS.ARecurringTransAnalAttrib.AcceptChanges();
            }

            transAnalAttrView.RowFilter = string.Empty;
        }
    }
}