//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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

namespace Ict.Common.Remoting.Shared
{
    #region EPetraSecurityException

    /// <summary>
    /// security violation (eg. access permissions etc)
    /// </summary>
    public class EPetraSecurityException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EPetraSecurityException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EPetraSecurityException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EPetraSecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region EUserNotExistantException

    /// <summary>
    /// user does not exist
    /// </summary>
    [Serializable()]
    public class EUserNotExistantException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EUserNotExistantException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EUserNotExistantException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EUserNotExistantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EPasswordWrongException

    /// <summary>
    /// password was wrong
    /// </summary>
    [Serializable()]
    public class EPasswordWrongException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EPasswordWrongException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EPasswordWrongException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EPasswordWrongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EUserRetiredException

    /// <summary>
    /// user is not active anymore (either retired on purpose, or too many login attempts with the wrong password
    /// </summary>
    [Serializable()]
    public class EUserRetiredException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EUserRetiredException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EUserRetiredException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EUserRetiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EAccessDeniedException

    /// <summary>
    /// access denied to a certain screen or information
    /// </summary>
    [Serializable()]
    public class EAccessDeniedException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EAccessDeniedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EAccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EUserRecordLockedException

    /// <summary>
    /// user record has been locked and is not available
    /// </summary>
    [Serializable()]
    public class EUserRecordLockedException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EUserRecordLockedException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EUserRecordLockedException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EUserRecordLockedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region ESystemDisabledException

    /// <summary>
    /// the whole Petra System is down for maintenance
    /// </summary>
    [Serializable()]
    public class ESystemDisabledException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public ESystemDisabledException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public ESystemDisabledException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public ESystemDisabledException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EInvalidSiteKeyException

    /// <summary>
    /// the client is using an invalid sitekey, which the server does not know about
    /// </summary>
    [Serializable()]
    public class EInvalidSiteKeyException : EPetraSecurityException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EInvalidSiteKeyException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EInvalidSiteKeyException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EInvalidSiteKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion

    #region EClientVersionMismatchException

    /// <summary>
    /// the binaries of the client don't match the version that the server is using
    /// </summary>
    [Serializable()]
    public class EClientVersionMismatchException : ApplicationException
    {
        /// <summary>
        /// constructor
        /// </summary>
        public EClientVersionMismatchException() : base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public EClientVersionMismatchException(String msg) : base(msg)
        {
        }

        /// <summary>
        /// constructor for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public EClientVersionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// needed for serialization
        /// </summary>
        /// <param name="info">needed for serialization</param>
        /// <param name="context">needed for serialization</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
    #endregion
}