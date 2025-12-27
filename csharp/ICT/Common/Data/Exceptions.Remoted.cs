//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2024 by OM International
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
using System.Runtime.Serialization;
using Ict.Common.Exceptions;

// This Namespace contains Exceptions that can be passed from the Server to the Client
// via .NET Remoting.
//
// These Exceptions are OpenPetra-Data-specific, but not specific to a certain
// OpenPetra Module (Partner, Finance, etc).
//
// Comment:
// Put remotable Exceptions which are specific to a certain Petra Module
// into shared Petra Module DLLs - eg Ict.Petra.Shared.MPartner, Ict.Petra.Shared.MFinance...

namespace Ict.Common.Data.Exceptions
{
    #region EOPDBTypedDataAccessException

    /// <summary>
    /// Base Class for OpenPetra-specific data access-level and database-level Exceptions
    /// that are thrown by the Typed Data Access class, <see cref="TTypedDataAccess" />.
    /// </summary>
    [Serializable()]
    public class EOPDBTypedDataAccessException : EOPDBException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPDBTypedDataAccessException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPDBTypedDataAccessException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBTypedDataAccessException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EDBConcurrencyException

    /// <summary>
    /// Thrown by classes in the DataStore if the record that is beeing written to the DB has
    /// modifications by another user.
    /// </summary>
    [Serializable()]
    public class EDBConcurrencyException : EOPDBTypedDataAccessException
    {
        /// <summary>Database Operation: either 'write', 'update' or 'delete'.</summary>
        private String FDBOperation;

        /// <summary>eg. 'p_partner'</summary>
        private String FDBTable;

        /// <summary>eg. 'SYSADMIN'</summary>
        private String FLastModificationUser;
        private DateTime FLastModification;

        /// <summary>
        /// Database Operation: either 'write', 'update' or 'delete'.
        /// </summary>
        public String DBOperation
        {
            get
            {
                return FDBOperation;
            }

            set
            {
                FDBOperation = value;
            }
        }

        /// <summary>
        /// Database Table where the problem was encountered.
        /// </summary>
        public String DBTable
        {
            get
            {
                return FDBTable;
            }

            set
            {
                FDBTable = value;
            }
        }

        /// <summary>
        /// Which user has changed the record in question last?
        /// </summary>
        public String LastModificationUser
        {
            get
            {
                return FLastModificationUser;
            }

            set
            {
                FLastModificationUser = value;
            }
        }

        /// <summary>
        /// When was the record in question changed last?
        /// </summary>
        public DateTime LastModification
        {
            get
            {
                return FLastModification;
            }

            set
            {
                FLastModification = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConcurrencyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConcurrencyException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConcurrencyException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ADBOperation">Database Operation: either 'write', 'update' or 'delete'.</param>
        /// <param name="ADBTable">Database Table where the problem was encountered.</param>
        /// <param name="ALastModificationUser">The user which has changed the record in question last.</param>
        /// <param name="ALastModification">DateTime when the record in question was changed last.</param>
        public EDBConcurrencyException(String AMessage, String ADBOperation, String ADBTable, String ALastModificationUser,
            DateTime ALastModification) : base(AMessage)
        {
            FDBOperation = ADBOperation;
            FDBTable = ADBTable;
            FLastModificationUser = ALastModificationUser;
            FLastModification = ALastModification;
        }
    }

    #endregion

    #region EDBConcurrencyRowDeletedException

    /// <summary>
    /// Specialisation of EDBConcurrencyException - this is thrown if a record should
    /// be updated, but it isn't in the DB!
    /// </summary>
    [Serializable()]
    public class EDBConcurrencyNoRowToUpdateException : EDBConcurrencyException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBConcurrencyNoRowToUpdateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBConcurrencyNoRowToUpdateException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBConcurrencyNoRowToUpdateException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ADBTable">Database Table where the problem was encountered.</param>
        /// <param name="ALastModificationUser">The user which has changed the record in question last.</param>
        /// <param name="ALastModification">DateTime when the record in question was changed last.</param>
        public EDBConcurrencyNoRowToUpdateException(String AMessage,
            String ADBTable,
            String ALastModificationUser,
            DateTime ALastModification) : base(AMessage, "update", ADBTable, ALastModificationUser, ALastModification)
        {
        }
    }

    #endregion

    #region EDBSubmitException

    /// <summary>
    /// Raised when a Database INSERT, UPDATE or DELETE query that was issued by the DataStore failed,
    /// or if it is to be issued and the DataStore determines that it cannot be issued for some reason.
    /// </summary>
    [Serializable()]
    public class EDBSubmitException : EOPDBTypedDataAccessException
    {
        private TTypedDataAccess.eSubmitChangesOperations FSubmitOperation;

        /// <summary>
        /// Gets or sets the Submit Operation that was attempted to be executed.
        /// </summary>
        public TTypedDataAccess.eSubmitChangesOperations SubmitOperation
        {
            get
            {
                return FSubmitOperation;
            }

            set
            {
                FSubmitOperation = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EDBSubmitException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EDBSubmitException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and Submit Operation.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ASubmitOperation">The Submit Operation that was attempted to be executed.</param>
        public EDBSubmitException(string AMessage, TTypedDataAccess.eSubmitChangesOperations ASubmitOperation) : base(AMessage)
        {
            FSubmitOperation = ASubmitOperation;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EDBSubmitException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" /> and the Submit Operation.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        /// <param name="ASubmitOperation">The Submit Operation that was attempted to be executed.</param>
        public EDBSubmitException(string AMessage, Exception AInnerException,
            TTypedDataAccess.eSubmitChangesOperations ASubmitOperation) : base(AMessage, AInnerException)
        {
            FSubmitOperation = ASubmitOperation;
        }
    }

    #endregion
}
