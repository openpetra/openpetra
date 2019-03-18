//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using System.Data.Odbc;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;

namespace Ict.Common.DB
{
    /// <summary>
    /// A generic Class for managing all kinds of ADO.NET Database Transactions -
    /// to be used instead of concrete ADO.NET Transaction objects, eg. <see cref="OdbcTransaction" />
    /// or NpgsqlTransaction, etc. Effectively wraps ADO.NET Transaction objects.
    /// </summary>
    /// <remarks>
    /// <em>IMPORTANT:</em> This Transaction Class does not have Commit or
    /// Rollback methods! This is so that the programmers are forced to use the
    /// CommitTransaction and RollbackTransaction methods of the <see cref="TDataBase" /> Class.
    /// <para>
    /// The reasons for this:
    /// <list type="bullet">
    /// <item><see cref="TDataBase" /> can know whether a Transaction is
    /// running (unbelievably, there is no way to find this out through ADO.NET!)</item>
    /// <item><see cref="TDataBase" /> can log Commits and Rollbacks. Another benefit of using this
    /// Class instead of a concrete implementation of ADO.NET Transaction Classes
    /// (eg. <see cref="OdbcTransaction" />) is that it is not tied to a specific ADO.NET
    /// provider, therefore making it easier to use a different ADO.NET provider than ODBC.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public class TDBTransaction : object, IDisposable
    {
        /// <summary>Holds the DbTransaction that we are wrapping inside this class.</summary>
        private DbTransaction FWrappedTransaction;
        private TDataBase FTDataBaseInstanceThatTransactionBelongsTo;
        private bool FReused = false;
        private Thread FThreadThatTransactionWasStartedOn;
        private AppDomain FAppDomainThatTransactionWasStartedIn;
        private StackTrace FStackTraceAtPointOfTransactionStart;
        private System.Guid FTransactionIdentifier;
        private System.Guid FConnectionIdentifier;
        private string FTransactionName;

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies a DB Transaction once it got created.
        /// It is used for internal 'sanity checks'. It also gets logged and hence can aid debugging (also useful for
        /// Unit Testing).</summary>
        public System.Guid TransactionIdentifier
        {
            get
            {
                return FTransactionIdentifier;
            }
        }

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies the DB Connection that this
        /// DB Transaction is running against. It is used for internal 'sanity checks'. It also gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</summary>
        public System.Guid ConnectionIdentifier
        {
            get
            {
                return FConnectionIdentifier;
            }
        }

        /// <summary>
        /// Instance of <see cref="TDataBase"/> that this Transaction belongs to.
        /// <para>
        /// Can be used e.g. in Static Methods to get the <see cref="TDataBase"/> instance that a <see cref="TDBTransaction"/>
        /// object was instantiated on. This <see cref="TDataBase"/> instance can then be used to execute calls on the proper
        /// <see cref="TDataBase"/> instance without needing to have this instance readily in a given context
        /// (thinking of static Methods in the 'Typed DataStore' here)!
        /// </para>
        /// </summary>
        public TDataBase DataBaseObj
        {
            get
            {
                return FTDataBaseInstanceThatTransactionBelongsTo;
            }
        }

        /// <summary>
        /// Name of the DB Transaction (optional). It gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        public string TransactionName
        {
            get
            {
                return FTransactionName;
            }
        }


        /// <summary>
        /// Database connection to which the Transaction belongs.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return FWrappedTransaction.Connection;
            }
        }

        /// <summary>
        /// <see cref="IsolationLevel" /> of the Transaction.
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                return FWrappedTransaction.IsolationLevel;
            }
        }

        /// <summary>
        /// DbTransaction that is wrapped in an instance of this class, i.e. which the instance of this class represents.
        /// <para><em><b>WARNING:</b> Do not do anything
        /// with this Object other than inspecting it; the correct
        /// working of Transactions in the <see cref="TDataBase" />
        /// Object relies on the fact that <see cref="TDataBase" /> manages <em>everything</em> about
        /// Transactions!!!</em>
        /// </para>
        /// </summary>
        public DbTransaction WrappedTransaction
        {
            get
            {
                return FWrappedTransaction;
            }
        }

        /// <summary>
        /// True if the Transaction has been re-used at least once by way of calling one of the
        /// 'TDataBase.GetNewOrExistingTransaction' or 'TDataBase.GetNewOrExistingAutoTransaction' Methods.
        /// </summary>
        public bool Reused
        {
            get
            {
                return FReused;
            }
        }

        /// <summary>
        /// True if the Transaction hasn't been Committed or Rolled Back, otherwise false.
        /// </summary>
        public bool Valid
        {
            get
            {
                if (FWrappedTransaction != null)
                {
                    return FWrappedTransaction.Connection != null;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Name of the AppDomain that the DB Transaction was started in. It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).
        /// </summary>
        public string AppDomainNameThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn.FriendlyName;
            }
        }

        /// <summary>
        /// AppDomain that the DB Transaction was started in.
        /// </summary>
        internal AppDomain AppDomainThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn;
            }
        }

        /// <summary>
        /// Thread that the DB Transaction was started on. It is used for internal 'sanity checks'. It also gets logged and
        /// hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        internal Thread ThreadThatTransactionWasStartedOn
        {
            get
            {
                return FThreadThatTransactionWasStartedOn;
            }
        }

        /// <summary>
        /// StackTrace at the point of the starting of the DB Transaction. This is kept so it can get logged and hence it
        /// can aid debugging.
        /// </summary>
        internal StackTrace StackTraceAtPointOfTransactionStart
        {
            get
            {
                return FStackTraceAtPointOfTransactionStart;
            }
        }

        /// <summary>
        /// Constructor for a <see cref="TDBTransaction" /> Object.
        /// </summary>
        /// <param name="ATransaction">The concrete <see cref="DbTransaction"/> object that <see cref="TDBTransaction" />
        /// should represent.</param>
        /// <param name="AConnectionIdentifier">ConnectionIdentifier of the DB Connection that this DB Transaction
        /// is running against.</param>
        /// <param name="ATDataBaseInstanceThatTransactionBelongsTo">Instance of <see cref="TDataBase"/> that
        /// owns this <see cref="TDBTransaction"/> instance.</param>
        /// <param name="AReused">Set to true to make the new instance return 'true' right away when its
        /// <see cref="Reused"/> Property gets inquired (default=false).</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        public TDBTransaction(DbTransaction ATransaction,
            System.Guid AConnectionIdentifier,
            TDataBase ATDataBaseInstanceThatTransactionBelongsTo,
            bool AReused = false,
            string ATransactionName = "")
        {
            FTransactionIdentifier = System.Guid.NewGuid();
            FTransactionName = ATransactionName;

            FWrappedTransaction = ATransaction;
            FReused = AReused;

            FTDataBaseInstanceThatTransactionBelongsTo = ATDataBaseInstanceThatTransactionBelongsTo;
            FAppDomainThatTransactionWasStartedIn = AppDomain.CurrentDomain;
            FThreadThatTransactionWasStartedOn = Thread.CurrentThread;
            FStackTraceAtPointOfTransactionStart = new StackTrace(true);
            FConnectionIdentifier = AConnectionIdentifier;
        }

        /// <summary>
        /// This Method must only get called from one of the 'TDataBase.GetNewOrExistingTransaction' Methods, or their extension DBAccess.SimpleGetTransaction!
        /// </summary>
        internal void SetTransactionToReused()
        {
            FReused = true;
        }

        /// <summary>
        /// Returns a string containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too. Gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        /// <returns>String containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too.
        /// </returns>
        public string GetDBTransactionIdentifier()
        {
            return " (Trans.Identifier: " + FTransactionIdentifier + ")" +
                   (FTransactionName != String.Empty ? String.Format(" (Transaction Name: {0})", FTransactionName) : "");
        }

        #region Dispose pattern

        /// <summary>
        /// Releases all resources used by the <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and <see cref="Connection" />).
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the  <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and optionally releases the managed resources of that object.
        /// </summary>
        /// <param name="ADisposing">True to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool ADisposing)
        {
            if (ADisposing)
            {
                if (FWrappedTransaction != null)
                {
                    FWrappedTransaction.Dispose();
                    FWrappedTransaction = null;
                }
            }
        }

        #endregion
    }
}
