//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2016 by OM International
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
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Windows.Forms;

using Npgsql;

namespace Ict.Common.Exceptions
{
    #region EPetraSecurityException

    /// <summary>
    /// Security violation (eg. access permissions etc).
    /// </summary>
    [Serializable()]
    public class EPetraSecurityException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EPetraSecurityException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EPetraSecurityException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EPetraSecurityException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EPetraSecurityException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EUserNotExistantException

    /// <summary>
    /// User does not exist.
    /// </summary>
    [Serializable()]
    public class EUserNotExistantException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EUserNotExistantException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EUserNotExistantException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EUserNotExistantException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EUserNotExistantException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EPasswordWrongException

    /// <summary>
    /// Password was wrong.
    /// </summary>
    [Serializable()]
    public class EPasswordWrongException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EPasswordWrongException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EPasswordWrongException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EPasswordWrongException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EPasswordWrongException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EUserRetiredException

    /// <summary>
    /// User is not active anymore (Retired on purpose).
    /// </summary>
    [Serializable()]
    public class EUserRetiredException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EUserRetiredException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EUserRetiredException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EUserRetiredException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EUserRetiredException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EUserAccountLockedException

    /// <summary>
    /// User Account is locked (a User Account gets locked automatically after too many login attempts with the wrong password).
    /// </summary>
    [Serializable()]
    public class EUserAccountLockedException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EUserAccountLockedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EUserAccountLockedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EUserAccountLockedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EUserAccountLockedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EUserAccountGotLockedException

    /// <summary>
    /// User Account got locked (happens automatically after too many login attempts with the wrong password).
    /// </summary>
    [Serializable()]
    public class EUserAccountGotLockedException : EUserAccountLockedException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EUserAccountGotLockedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EUserAccountGotLockedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EUserAccountGotLockedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EUserAccountGotLockedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EAccessDeniedException

    /// <summary>
    /// Access denied to a certain screen or information.
    /// </summary>
    [Serializable()]
    public class EAccessDeniedException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EAccessDeniedException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EAccessDeniedException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EAccessDeniedException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EAccessDeniedException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region ESystemDisabledException

    /// <summary>
    /// Thrown if the whole OpenPetra System is down for maintenance.
    /// </summary>
    [Serializable()]
    public class ESystemDisabledException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ESystemDisabledException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ESystemDisabledException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ESystemDisabledException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public ESystemDisabledException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EInvalidSiteKeyException

    /// <summary>
    /// The Client is using an invalid SiteKey which the Server does not know about.
    /// </summary>
    [Serializable()]
    public class EInvalidSiteKeyException : EPetraSecurityException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EInvalidSiteKeyException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EInvalidSiteKeyException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EInvalidSiteKeyException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EInvalidSiteKeyException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EClientVersionMismatchException

    /// <summary>
    /// The binaries of the Client don't match the Version that the Server uses.
    /// The Client is prevented from connecting to the Server in such a circumstance.
    /// </summary>
    [Serializable()]
    public class EClientVersionMismatchException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EClientVersionMismatchException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EClientVersionMismatchException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EClientVersionMismatchException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EClientVersionMismatchException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    #region EPartnerKeyOutOfRangeException

    /// <summary>
    /// This Exception is raised when a the PartnerKey number does not fit into a string
    /// of Length 10.
    /// </summary>
    [Serializable()]
    public class EPartnerKeyOutOfRangeException : System.FormatException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EPartnerKeyOutOfRangeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EPartnerKeyOutOfRangeException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EPartnerKeyOutOfRangeException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
        {
        }

        #region Remoting and serialization

        /// <summary>
        /// Initializes a new instance of this Exception Class with serialized data. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public EPartnerKeyOutOfRangeException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
        {
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo" /> with information about this Exception. Needed for Remoting and general serialization.
        /// </summary>
        /// <remarks>
        /// Only to be used by the .NET Serialization system (eg within .NET Remoting).
        /// </remarks>
        /// <param name="AInfo">The <see cref="SerializationInfo" /> that holds the serialized object data about the <see cref="Exception" /> being thrown.</param>
        /// <param name="AContext">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            if (AInfo == null)
            {
                throw new ArgumentNullException("AInfo");
            }

            // We must call through to the base class to let it save its own state!
            base.GetObjectData(AInfo, AContext);
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// Helper Class for handling Exceptions.
    /// </summary>
    public class TExceptionHelper
    {
        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBroken = Catalog.GetString(
            "The OpenPetra server has encountered a problem while accessing the OpenPetra database.\r\n\r\n" +
            "We are sorry about this!  Automatic attempts at restoring the database connection are underway.\r\n\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRestored = Catalog.GetString(
            "The OpenPetra server has successfully recovered from a database access problem!\r\n\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenAdvice = Catalog.GetString(
            "If you have seen an error message just before you got to see this message then you should definitely close " +
            "the OpenPetra Client, if you haven't then you are still advised to close the OpenPetra Client because it might " +
            "not be working stable anymore.\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggingIn = Catalog.GetString(
            "Unfortunately a login to OpenPetra is not possible while this is being addressed. Please retry the login in " +
            "a minute or so to see if is already possible again. ");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggedIn1 = Catalog.GetString(
            "Please do not attempt to do any work with OpenPetra until you are told that the problem got rectified!!! ");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggedIn2 = Catalog.GetString(
            "   In case you have entered data that has not been saved then you can copy data that you see on " +
            "screen(s) and paste it in a text document so that you can have it to hand later, or take a screenshot to preserve data " +
            "that you still can see.\r\n\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggedIn3 = Catalog.GetString(
            "   In case you have entered data that has not been saved yet do try to save it now. Should this not be possible " +
            "then you can copy data that you see on " +
            "screen(s) and paste it in a text document so that you can have it to hand later, or take a screenshot to preserve data " +
            "that you still can see.\r\n\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggedInFollowUpAction1 = Catalog.GetString(
            "Once you have closed and restarted the OpenPetra Client you can attempt to log in to OpenPetra again. " +
            "At that point you will find out if the login is already possible. ");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRemedyWhenLoggedInFollowUpAction2 = Catalog.GetString(
            "Once you have closed and restarted the OpenPetra Client you can log in to OpenPetra again.\r\n");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenTitle = Catalog.GetString(
            "Temporary Service Outage (OpenPetra Database Problem)");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenRecoveredTitlePrefix = Catalog.GetString(
            "RECOVERED: ");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionBrokenContactITSupport = Catalog.GetString(
            "In case the problem should persist for longer than a few minutes please contact your IT support staff.");

        /// <summary>todoComment</summary>
        public static readonly string StrDBConnectionIssueDateTimeFooter = Catalog.GetString(
            "\r\nSorry for any inconvenience caused.   (Message issued on {0:F})");

        /// <summary>Used as a way of marking any Exception as being caused by the DB becoming unavilable.</summary>
        public const string EXCEPTION_DATA_DBUNAVAILABLE = "DB_UNAVAILABLE";

        /// <summary>
        /// Checks if a given Exception is caused by the fact that the DB Connection of the PetraServer isn't available.
        /// </summary>
        /// <remarks>There is a Method with nearly the same name, <see cref="IsExceptionCausedByUnavailableDBConnectionServerSide"/>
        /// which evaluates Exceptions that get thrown on the server side. That Method checks if a given Exception is
        /// caused by the fact that the DB Connection isn't available and if so adds a specific Item to the Exception's
        /// Data Property. This Item is checked for by <see cref="IsExceptionCausedByUnavailableDBConnectionClientSide"/>.</remarks>
        /// <param name="AException">Exception to check.</param>
        /// <returns>True if the Exception is caused by the fact that the DB Connection of the PetraServer isn't available,
        /// false if not.</returns>
        public static bool IsExceptionCausedByUnavailableDBConnectionClientSide(Exception AException)
        {
            bool ExceptionHierachyFullyTraversed = false;
            IDictionary ExceptionData;
            bool DBUnavailable = false;

            while (!ExceptionHierachyFullyTraversed)
            {
                ExceptionData = AException.Data;

                if ((ExceptionData != null)
                    && (ExceptionData.Contains(EXCEPTION_DATA_DBUNAVAILABLE)))
                {
                    DBUnavailable = true;

                    break;
                }

                if (AException.InnerException != null)
                {
                    AException = AException.InnerException;
                }
                else
                {
                    ExceptionHierachyFullyTraversed = true;
                }
            }

            return DBUnavailable;
        }

        /// <summary>
        /// Checks if a given Exception is caused by the fact that the DB Connection isn't available and if so
        /// adds a specific Item to the Exception's Data Property.
        /// </summary>
        /// <remarks>There is a Method with nearly the same name, <see cref="IsExceptionCausedByUnavailableDBConnectionClientSide"/>
        /// which evaluates Exceptions that get thrown from the server side to the client side. That Method checks for
        /// the presence of the <see cref="TExceptionHelper.EXCEPTION_DATA_DBUNAVAILABLE"/> entry in the Exception's
        /// Data Property, which the Method here <see cref="IsExceptionCausedByUnavailableDBConnectionServerSide"/> adds to
        /// Exceptions that are caused by the fact that the DB Connection isn't available.</remarks>
        /// <param name="AException">Exception to check.</param>
        /// <returns>True if the Exception is caused by the fact that the DB Connection isn't available,
        /// false if not.</returns>
        public static bool IsExceptionCausedByUnavailableDBConnectionServerSide(Exception AException)
        {
            bool ProcessNpgsqlException = true;

            if (AException is NpgsqlException)
            {
                if (AException.Message.StartsWith("Failed to establish a connection to")  // Unfortnately there's no Code available for this Message so we have to check for the string...
                    || (AException.Message.StartsWith("A timeout has occured. If you were establishing a connection, "))  // Unfortnately there's no Code available for this Message so we have to check for the string...
                    || (((NpgsqlException)AException).Code == "57P03"))  // Message: 'the database system is starting up'
                {
                    if (AException.Message.StartsWith("A timeout has occured. If you were establishing a connection, "))
                    {
                        if (!AException.StackTrace.Contains("IsDBConnectionOK"))
                        {
                            ProcessNpgsqlException = false;
                        }
                    }

                    if (ProcessNpgsqlException)
                    {
                        if (TLogging.DebugLevel >= 1)
                        {
                            TLogging.Log(String.Format("NpgsqlException with Message {0} raised by Npgql in {1}: {2}",
                                    ((NpgsqlException)AException).Code == "57P03" ? "'The database system is starting up...'" :
                                    "'" + AException.Message + "'",
                                    AppDomain.CurrentDomain.FriendlyName, AException.Message));
                            TLogging.LogStackTrace(TLoggingType.ToLogfile);
                        }

                        AddDBUnavailableItemToDataPropertyOfException(AException);

                        return true;
                    }
                }
            }
            else if ((AException is SocketException)
                     || (AException is IOException))
            {
                if (AException.StackTrace.Contains("Npgsql"))
                {
                    if (TLogging.DebugLevel >= 1)
                    {
                        TLogging.Log(String.Format("SocketException or IOException raised by Npgql in {0}: {1}",
                                AppDomain.CurrentDomain.FriendlyName, AException.Message));
                        TLogging.LogStackTrace(TLoggingType.ToLogfile);
                    }

                    AddDBUnavailableItemToDataPropertyOfException(AException);

                    return true;
                }
            }
            else if ((AException.GetType().ToString() == "Ict.Common.DB.Exceptions.EDBConnectionBrokenException")
                     || (AException.GetType().ToString() == "Ict.Common.DB.Exceptions.EDBConnectionNotEstablishedException"))
            {
                if (TLogging.DebugLevel >= 1)
                {
                    TLogging.Log(String.Format("{0} got raised in {1}: {2}",
                            AException.GetType().ToString(), AppDomain.CurrentDomain.FriendlyName, AException.Message));
                    TLogging.LogStackTrace(TLoggingType.ToLogfile);
                }

                AddDBUnavailableItemToDataPropertyOfException(AException);

                return true;
            }

            return false;
        }

        private static void AddDBUnavailableItemToDataPropertyOfException(Exception AException)
        {
            if ((AException.Data != null)
                && !AException.Data.Contains(TExceptionHelper.EXCEPTION_DATA_DBUNAVAILABLE))
            {
                try
                {
                    AException.Data.Add(TExceptionHelper.EXCEPTION_DATA_DBUNAVAILABLE, true);
                }
                catch (System.ArgumentException)
                {
                    // Swallow this as it will get thrown in case the Dictionary Entry was already
                    // present (I know I am checking for this just above, but somehow it still
                    // can happen because the caller of this Method is not 'reentrancy-safe'...
                }
            }
        }

        /// <summary>
        /// Shows a MessageBox to the user that explains to the user that the DB Connection is broken. Advice is given
        /// on how to proceed, depending on the value of <paramref name="AClientLogin"/>. The same message gets logged, too.
        /// </summary>
        /// <param name="AClientLogin">Set this to true if the Exception was encountered at Client login time, otherwise
        /// to false.</param>
        public static void ShowExceptionCausedByUnavailableDBConnectionMessage(bool AClientLogin)
        {
            string Message = StrDBConnectionBroken + (AClientLogin ?
                                                      StrDBConnectionBrokenRemedyWhenLoggingIn + StrDBConnectionBrokenContactITSupport :
                                                      StrDBConnectionBrokenAdvice + StrDBConnectionBrokenRemedyWhenLoggedIn2 +
                                                      StrDBConnectionBrokenRemedyWhenLoggedInFollowUpAction1 +
                                                      StrDBConnectionBrokenContactITSupport);

            TLogging.Log(Message);

            MessageBox.Show(Message + Environment.NewLine + Environment.NewLine + Catalog.GetString(String.Format(
                        TExceptionHelper.StrDBConnectionIssueDateTimeFooter, DateTime.Now)),
                StrDBConnectionBrokenTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a MessageBox to the user that explains to the user that OpenPetra can't process that much data.
        /// The same message gets logged, too.
        /// </summary>
        /// <param name="AReportWasBeingGenerated">Set this to true if the Exception was encountered while a Report was generated
        /// otherwise to false.</param>
        public static void ShowExceptionCausedByOutOfMemoryMessage(bool AReportWasBeingGenerated)
        {
            string Message = AReportWasBeingGenerated ?
                             Catalog.GetString("Your request cannot be handled by OpenPetra because the Report Criteria that you have " +
                "specified causes too much data to be processed, and OpenPetra cannot handle this (yet).") :
                             Catalog.GetString("Your request cannot be handled by OpenPetra because too much data would need to be " +
                "processed in order to handle it.");

            Message += Environment.NewLine + Environment.NewLine + Catalog.GetString(
                "If you believe this is a genuine error or if you can't do your work by requesting less data to be processed " +
                "then please contact OpenPetra Support.");

            TLogging.Log(Message.ToString(), TLoggingType.ToLogfile);

            MessageBox.Show(Message, Catalog.GetString("Request Cannot be Handled"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}