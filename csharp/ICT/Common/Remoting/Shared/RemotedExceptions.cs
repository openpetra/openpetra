//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Runtime.Serialization;

/*
 * Contains Exceptions that can be passed from the Server to the Client via
 * .NET Remoting.
 *
 * These Exceptions are Petra-specific, but not specific to a certain
 * Petra Module (Partner, Finance, etc).
 *
 * @Comment Put remotable Exceptions which are specific to a certain Petra Module
 *          into shared Petra Module DLLs - eg Ict_Petra_Shared_MPartner,
 *          Ict_Petra_Shared_MFinance...
 *
 */
namespace Ict.Common.Remoting.Shared
{
    #region OutOfRangeException

    /// <summary>
    /// Thrown if a value is out of range
    /// </summary>
    [Serializable()]
    public class EOutOfRangeException : ApplicationException
    {
        private String FCaption;

        /// <summary>
        /// Caption
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
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EOutOfRangeException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EOutOfRangeException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="caption"></param>
        public EOutOfRangeException(String msg, String caption) : base(msg)
        {
            FCaption = caption;
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region PagedTableNoRecordsException

    /// <summary>
    /// Thrown by TPagedDataSet class if no records where found by the query
    /// </summary>
    [Serializable()]
    public class EPagedTableNoRecordsException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EPagedTableNoRecordsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EPagedTableNoRecordsException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EPagedTableNoRecordsException() : base()
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region PagedTableNoSuchPageException

    /// <summary>
    /// Thrown by TPagedDataSet class if a page was requested that does not exist
    /// </summary>
    [Serializable()]
    public class EPagedTableNoSuchPageException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EPagedTableNoSuchPageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public EPagedTableNoSuchPageException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EPagedTableNoSuchPageException() : base()
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ECachedDataTableNotImplementedException

    /// <summary>
    /// Thrown by a Cache Instantiator class if a Cached DataTable was requested that does not exist
    /// </summary>
    [Serializable()]
    public class ECachedDataTableNotImplementedException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ECachedDataTableNotImplementedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ECachedDataTableNotImplementedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ECachedDataTableNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ECachedDataTableReturnedNoDataException

    /// <summary>
    /// Thrown by a Cache Instantiator class if a Cached DataTable that was requested returned no DataTable object
    /// </summary>
    [Serializable()]
    public class ECachedDataTableReturnedNothingException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ECachedDataTableReturnedNothingException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ECachedDataTableReturnedNothingException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ECachedDataTableReturnedNothingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ECachedDataTableTableNameMismatchException

    /// <summary>
    /// Thrown by a Cache Instantiator class if a Cached DataTable that was requested is named differently than the Enum
    /// </summary>
    [Serializable()]
    public class ECachedDataTableTableNameMismatchException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ECachedDataTableTableNameMismatchException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ECachedDataTableTableNameMismatchException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ECachedDataTableTableNameMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ELoginFailedServerTooBusyException

    /// <summary>
    /// exception is thrown if login was not possible, because the server was too busy
    /// </summary>
    [Serializable()]
    public class ELoginFailedServerTooBusyException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ELoginFailedServerTooBusyException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ELoginFailedServerTooBusyException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ELoginFailedServerTooBusyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ESecurityAccessDeniedException

    /// <summary>
    /// Base class for all Security Exceptions
    /// </summary>
    [Serializable()]
    public class ESecurityAccessDeniedException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ESecurityAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ESecurityDBTableAccessDeniedException

    /// <summary>
    /// Thrown by TDataBasePetra class if the user doesn't have enough rights to execute the query
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
        /// constructor
        /// </summary>
        public ESecurityDBTableAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityDBTableAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityDBTableAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FDBTable = info.GetString("DBTable");
            FAccessRight = info.GetString("AccessRight");
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="AAccessRight"></param>
        /// <param name="ADBTable"></param>
        public ESecurityDBTableAccessDeniedException(String msg, String AAccessRight, String ADBTable) : base(msg)
        {
            FAccessRight = AAccessRight;
            FDBTable = ADBTable;
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DBTable", FDBTable);
            info.AddValue("AccessRight", FAccessRight);
        }
    }
    #endregion

    #region ESecurityScreenAccessDeniedException

    /// <summary>
    /// Thrown if a Petra screen (WinForm) cannot be opened for security reasons
    /// </summary>
    [Serializable()]
    public class ESecurityScreenAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ESecurityScreenAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityScreenAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityScreenAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ESecurityModuleAccessDeniedException

    /// <summary>
    /// Thrown if a Petra User has no rights for a certain Security Module
    /// </summary>
    [Serializable()]
    public class ESecurityModuleAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ESecurityModuleAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityModuleAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityModuleAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ESecurityGroupAccessDeniedException

    /// <summary>
    /// Thrown if a Petra User has no rights for a certain Security Group
    /// </summary>
    [Serializable()]
    public class ESecurityGroupAccessDeniedException : ESecurityAccessDeniedException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ESecurityGroupAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="msg"></param>
        public ESecurityGroupAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ESecurityGroupAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for remoting, serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
}