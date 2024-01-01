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

// This Namespace contains Exceptions that can be passed from the Server to the Client
// via .NET Remoting.
//
// These Exceptions are OpenPetra-specific, but not specific to a certain
// OpenPetra Module (Partner, Finance, etc).
//
// Comment:
// Put remotable Exceptions which are specific to a certain Petra Module
// into shared Petra Module DLLs - eg Ict.Petra.Shared.MPartner, Ict.Petra.Shared.MFinance...

namespace Ict.Common.Exceptions
{
    #region EOPException

    /// <summary>
    /// Base Class for OpenPetra-specific Exceptions.
    /// </summary>
    /// <remarks>
    /// *** *** *** *** ALL CUSTOM EXCEPTIONS THAT ARE THROWN BY ANY OF OpenPetra's CLASSES
    /// MUST DERIVE FROM THIS EXCEPTION!!! *** *** *** *** *** *** *** *** *** *** *** ***
    /// </remarks>
    [Serializable()]
    public class EOPException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EOPAppException

    /// <summary>
    /// Base Class for OpenPetra-specific application-level Exceptions.
    /// </summary>
    [Serializable()]
    public class EOPAppException : EOPException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPAppException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPAppException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPAppException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EOPDBException

    /// <summary>
    /// Base Class for OpenPetra-specific data access-level and database-level Exceptions.
    /// </summary>
    [Serializable()]
    public class EOPDBException : EOPException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPDBException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPDBException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a reference to the inner <see cref="Exception" />
        /// that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBException(Exception AInnerException) : base("Database Access Exception occurred", AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EOPDBSessionException

    /// <summary>
    /// Base Class for OpenPetra-specific Session Exceptions.
    /// </summary>
    [Serializable()]
    public class EOPDBSessionException : EOPException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPDBSessionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPDBSessionException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a reference to the inner <see cref="Exception" />
        /// that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBSessionException(Exception AInnerException) : base("Session Exception occured", AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBSessionException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EOPDBInvalidSessionException

    /// <summary>
    /// Thrown if a Session that is asked for is invalid.
    /// </summary>
    [Serializable()]
    public class EOPDBInvalidSessionException : EOPDBSessionException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOPDBInvalidSessionException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOPDBInvalidSessionException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOPDBInvalidSessionException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EOutOfRangeException

    /// <summary>
    /// Thrown if a value is out of range.
    /// </summary>
    [Serializable()]
    public class EOutOfRangeException : EOPAppException
    {
        private String FCaption;

        /// <summary>
        /// Caption.
        /// </summary>
        public String Caption
        {
            get
            {
                return FCaption;
            }

            set
            {
                FCaption = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EOutOfRangeException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EOutOfRangeException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified  error message and <see cref="Caption" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ACaption">Caption.</param>
        public EOutOfRangeException(String AMessage, String ACaption) : base(AMessage)
        {
            FCaption = ACaption;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EOutOfRangeException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EPagedTableNoRecordsException

    /// <summary>
    /// Thrown by the TPagedDataSet Class if no records where found by the query.
    /// </summary>
    [Serializable()]
    public class EPagedTableNoRecordsException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EPagedTableNoRecordsException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EPagedTableNoRecordsException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EPagedTableNoRecordsException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region PagedTableNoSuchPageException

    /// <summary>
    /// Thrown by the TPagedDataSet Class if a page was requested that does not exist.
    /// </summary>
    [Serializable()]
    public class EPagedTableNoSuchPageException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EPagedTableNoSuchPageException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EPagedTableNoSuchPageException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EPagedTableNoSuchPageException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ECachedDataTableNotImplementedException

    /// <summary>
    /// Thrown by a Cache Instantiator Class if a Cached DataTable was requested that does not exist.
    /// </summary>
    [Serializable()]
    public class ECachedDataTableNotImplementedException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECachedDataTableNotImplementedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECachedDataTableNotImplementedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECachedDataTableNotImplementedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ECachedDataTableReturnedNoDataException

    /// <summary>
    /// Thrown by a Cache Instantiator Class if a Cached DataTable that was requested returned no DataTable object.
    /// </summary>
    [Serializable()]
    public class ECachedDataTableReturnedNothingException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECachedDataTableReturnedNothingException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECachedDataTableReturnedNothingException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECachedDataTableReturnedNothingException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ECachedDataTableLoadingRetryGotCancelledException

    /// <summary>
    /// Thrown by the Cache Manager Class if the loading of a Cached DataTable got cancelled by the user when the attemps to
    /// retry the loading were exhausted.
    /// </summary>
    [Serializable()]
    public class ECachedDataTableLoadingRetryGotCancelledException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECachedDataTableLoadingRetryGotCancelledException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECachedDataTableLoadingRetryGotCancelledException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECachedDataTableLoadingRetryGotCancelledException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion


    #region ECachedDataTableTableNameMismatchException

    /// <summary>
    /// Thrown by a Cache Instantiator Class if a Cached DataTable that was requested is named differently than the Enum.
    /// </summary>
    [Serializable()]
    public class ECachedDataTableTableNameMismatchException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ECachedDataTableTableNameMismatchException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ECachedDataTableTableNameMismatchException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ECachedDataTableTableNameMismatchException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ELoginFailedServerTooBusyException

    /// <summary>
    /// Thrown if login was not possible because the Server was too busy.
    /// </summary>
    [Serializable()]
    public class ELoginFailedServerTooBusyException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ELoginFailedServerTooBusyException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ELoginFailedServerTooBusyException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ELoginFailedServerTooBusyException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ESecurityAccessDeniedException

    /// <summary>
    /// Base Class for all Security Exceptions.
    /// </summary>
    [Serializable()]
    public class ESecurityAccessDeniedException : EOPAppException
    {
        /// <summary>Context in which the access was denied.</summary>
        private String FContext = String.Empty;

        /// <summary>Context in which the access was denied.</summary>
        public String Context
        {
            get
            {
                return FContext;
            }

            set
            {
                FContext = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, OpenPetra Module and OpenPetra User.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AContext">Context in which the access was denied.</param>
        public ESecurityAccessDeniedException(String AMessage, String AContext) : base(AMessage)
        {
            FContext = AContext;
        }
    }

    #endregion

    #region ESecurityDBTableAccessDeniedException

    /// <summary>
    /// Thrown by the TDataBase Class if the user doesn't have enough rights to execute the query.
    /// </summary>
    [Serializable()]
    public class ESecurityDBTableAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>'create', 'modify', 'delete' or 'inquire'</summary>
        private String FAccessRight;

        /// <summary>eg. 'p_partner'</summary>
        private String FDBTable;

        /// <summary>'create', 'modify', 'delete' or 'inquire'</summary>
        public String AccessRight
        {
            get
            {
                return FAccessRight;
            }

            set
            {
                FAccessRight = value;
            }
        }

        /// <summary>table name that cannot be accessed</summary>
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
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityDBTableAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityDBTableAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityDBTableAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, access right and Database Table.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AAccessRight">Access right.</param>
        /// <param name="ADBTable">Database Table.</param>
        public ESecurityDBTableAccessDeniedException(String AMessage, String AAccessRight, String ADBTable) : base(AMessage)
        {
            FAccessRight = AAccessRight;
            FDBTable = ADBTable;
        }
    }

    #endregion

    #region ESecurityScreenAccessDeniedException

    /// <summary>
    /// Thrown if an OpenPetra screen (WinForm) cannot be opened for security reasons.
    /// </summary>
    [Serializable()]
    public class ESecurityScreenAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityScreenAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityScreenAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityScreenAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region ESecurityModuleAccessDeniedException

    /// <summary>
    /// Thrown if an OpenPetra User has no rights for a certain Security Module.
    /// </summary>
    [Serializable()]
    public class ESecurityModuleAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>OpenPetra Module that cannot be accessed.</summary>
        private String FModule;

        /// <summary>OpenPetra User.</summary>
        private String FUserName;

        /// <summary>OpenPetra Module that cannot be accessed.</summary>
        public String Module
        {
            get
            {
                return FModule;
            }

            set
            {
                FModule = value;
            }
        }

        /// <summary>OpenPetra User.</summary>
        public String UserName
        {
            get
            {
                return FUserName;
            }

            set
            {
                FUserName = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityModuleAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityModuleAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityModuleAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, OpenPetra Module and OpenPetra User.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AUserName">OpenPetra User.</param>
        /// <param name="AModule">OpenPetra Module that cannot be accessed.</param>
        public ESecurityModuleAccessDeniedException(String AMessage, String AUserName, String AModule) : base(AMessage)
        {
            FModule = AModule;
            FUserName = AUserName;
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message, context, OpenPetra Module and OpenPetra User.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AUserName">OpenPetra User.</param>
        /// <param name="AModule">OpenPetra Module that cannot be accessed.</param>
        /// <param name="AExceptionContext">The context for this exception</param>
        public ESecurityModuleAccessDeniedException(String AMessage, String AUserName, String AModule, String AExceptionContext) : base(AMessage,
                                                                                                                                       AExceptionContext)
        {
            FModule = AModule;
            FUserName = AUserName;
        }
    }

    #endregion

    #region ESecurityGroupAccessDeniedException

    /// <summary>
    /// Thrown if an OpenPetra User has no rights for a certain Security Group.
    /// </summary>
    [Serializable()]
    public class ESecurityGroupAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESecurityGroupAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESecurityGroupAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESecurityGroupAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EProblemConstructingHyperlinkException

    /// <summary>
    /// Thrown if the the attempt to construct a Hyperlink from a Value and a Hyperlink Format fails.
    /// </summary>
    [Serializable()]
    public class EProblemConstructingHyperlinkException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EProblemConstructingHyperlinkException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EProblemConstructingHyperlinkException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EProblemConstructingHyperlinkException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemUnexpectedStateException

    /// <summary>
    /// Base Class for OpenPetra-finance-system-specific application-level Exceptions.
    /// </summary>
    [Serializable()]
    public class EFinanceSystemUnexpectedStateException : EOPException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemUnexpectedStateException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemUnexpectedStateException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemUnexpectedStateException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemInvalidLedgerNumberException

    /// <summary>
    /// Thrown if a Ledger number is invalid.
    /// </summary>
    [Serializable()]
    public class EFinanceSystemInvalidLedgerNumberException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>Ledger number</summary>
        private int FLedgerNumber;

        /// <summary>Ledger number</summary>
        public int LedgerNumber
        {
            get
            {
                return FLedgerNumber;
            }

            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemInvalidLedgerNumberException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemInvalidLedgerNumberException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemInvalidLedgerNumberException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and ledger number.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ALedgerNumber">Ledger number.</param>
        public EFinanceSystemInvalidLedgerNumberException(String AMessage, int ALedgerNumber)
            : base(AMessage)
        {
            FLedgerNumber = ALedgerNumber;
        }
    }

    #endregion

    #region EFinanceSystemInvalidBatchNumberException

    /// <summary>
    /// Thrown if a Batch number in a given Ledger number is invalid.
    /// </summary>
    [Serializable()]
    public class EFinanceSystemInvalidBatchNumberException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>Ledger number</summary>
        private int FLedgerNumber;

        /// <summary>Batch number</summary>
        private int FBatchNumber;

        /// <summary>Ledger number</summary>
        public int LedgerNumber
        {
            get
            {
                return FLedgerNumber;
            }

            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>Ledger number</summary>
        public int BatchNumber
        {
            get
            {
                return FBatchNumber;
            }

            set
            {
                FBatchNumber = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemInvalidBatchNumberException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemInvalidBatchNumberException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemInvalidBatchNumberException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and ledger number.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="ALedgerNumber">Ledger number.</param>
        /// <param name="ABatchNumber">Batch number.</param>
        public EFinanceSystemInvalidBatchNumberException(String AMessage, int ALedgerNumber, int ABatchNumber)
            : base(AMessage)
        {
            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
        }
    }

    #endregion

    #region EFinanceSystemDBTransactionNullException

    /// <summary>
    /// Thrown if a given DB Transaction is null
    /// </summary>
    [Serializable()]
    public class EFinanceSystemDBTransactionNullException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemDBTransactionNullException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemDBTransactionNullException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemDBTransactionNullException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemDataObjectNullException

    /// <summary>
    /// Thrown if a given data object is null
    /// </summary>
    [Serializable()]
    public class EFinanceSystemDataObjectNullOrEmptyException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemDataObjectNullOrEmptyException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemDataObjectNullOrEmptyException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemDataObjectNullOrEmptyException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemDataTableReturnedNoDataException

    /// <summary>
    /// Thrown if a given data table object is null or empty
    /// </summary>
    [Serializable()]
    public class EFinanceSystemDataTableReturnedNoDataException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemDataTableReturnedNoDataException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemDataTableReturnedNoDataException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemDataTableReturnedNoDataException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemCacheableTableReturnedNoDataException

    /// <summary>
    /// Thrown if a given cacheable data table object is null or empty
    /// </summary>
    [Serializable()]
    public class EFinanceSystemCacheableTableReturnedNoDataException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemCacheableTableReturnedNoDataException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemCacheableTableReturnedNoDataException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemCacheableTableReturnedNoDataException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion

    #region EFinanceSystemDataTableAccessFailedException

    /// <summary>
    /// Thrown if a given data table object is unable to access (and load the contents of) a specified DB table
    /// </summary>
    [Serializable()]
    public class EFinanceSystemDataTableAccessFailedException : EFinanceSystemUnexpectedStateException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EFinanceSystemDataTableAccessFailedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EFinanceSystemDataTableAccessFailedException(String AMessage)
            : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EFinanceSystemDataTableAccessFailedException(string AMessage, Exception AInnerException)
            : base(AMessage, AInnerException)
        {
        }
    }

    #endregion
}
