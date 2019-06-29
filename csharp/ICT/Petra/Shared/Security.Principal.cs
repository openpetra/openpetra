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
using System.Runtime.Serialization;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan.Data;

using Newtonsoft.Json.Linq;

namespace Ict.Petra.Shared.Security
{
    /// The TPetraPrincipal class is a .NET Principal-derived representation of a
    /// User in the Petra DB with its Groups and Roles.
    [Serializable()]
    public class TPetraPrincipal
    {
        private String[] FGroups;
        // TODO: this will not work with json sessions yet
        // private SUserTableAccessPermissionTable FUserTableAccessPermissionDT;
        private String[] FModuleAccess;
        private String[] FRoles;
        private String[] FFunctions;
        private String FLoginMessage;
        private Int32 FProcessID;
        private String FUserID;
        private Int64 FPartnerKey;

        /// <summary>the current user id</summary>
        public String UserID
        {
            get
            {
                return FUserID;
            }
            set
            {
                FUserID = value;
            }
        }

        /// <summary>the partner key of this user</summary>
        public Int64 PartnerKey
        {
            get
            {
                return FPartnerKey;
            }
            set
            {
                FPartnerKey = value;
            }
        }

        /// <summary>
        /// login message can give system information to the user during login
        /// </summary>
        public String LoginMessage
        {
            get
            {
                return FLoginMessage;
            }

            set
            {
                if (FLoginMessage == null)
                {
                    FLoginMessage = value;
                }
                else
                {
                    throw new ELoginMessageAlreadySetException();
                }
            }
        }

        /// <summary>
        /// process id of the client domain on the server???
        /// todoComment
        /// </summary>
        public Int32 ProcessID
        {
            get
            {
                return FProcessID;
            }

            set
            {
                if (FProcessID == 0)
                {
                    FProcessID = value;
                }
                else
                {
                    throw new EProcessIDAlreadySetException();
                }
            }
        }

        /// we need this for JSON serialization for the session
        public string[] ModuleAccess
        {
            get
            {
                return FModuleAccess;
            }
            set
            {
                // only allow setting by json deserialization
                if (FModuleAccess == null || FModuleAccess.Length == 0)
                {
                    FModuleAccess = value;
                }
            }
        }

        /// we need this for JSON serialization for the session
        public string[] Groups
        {
            get
            {
                return FGroups;
            }
            set
            {
                // only allow setting by json deserialization
                if (FGroups == null || FGroups.Length == 0)
                {
                    FGroups = value;
                }
            }
        }

        #region TPetraPrincipal

        /// <summary>
        /// default constructor for JsonConvert
        /// </summary>
        public TPetraPrincipal()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="AGroups"></param>
        /// <param name="AModuleAccess"></param>
        /// <param name="AFunctions"></param>
        /// <param name="ARoles"></param>
        public TPetraPrincipal(string AUserID,
            string[] AGroups = null,
            String[] AModuleAccess = null,
            String[] AFunctions = null,
            String[] ARoles = null) : base()
        {
            FUserID = AUserID;
            FGroups = AGroups;
            //FUserTableAccessPermissionDT = AUserTableAccessPermissions;
            FModuleAccess = AModuleAccess;
            FFunctions = AFunctions;
            FRoles = ARoles;

            // Prepare Arrays for fast BinarySearch
            if (FGroups != null)
            {
                System.Array.Sort(FGroups);
            }

            if (FModuleAccess != null)
            {
                System.Array.Sort(FModuleAccess);
            }

            if (FRoles != null)
            {
                System.Array.Sort(FRoles);
            }

            if (FFunctions != null)
            {
                System.Array.Sort(FFunctions);
            }

            // Default LoginMessage is not defined
            FLoginMessage = null;
        }

        /// <summary>
        /// tells if the user is part of the given group
        /// </summary>
        /// <param name="AGroupName"></param>
        /// <returns></returns>
        public Boolean IsInGroup(string AGroupName)
        {
            return (FGroups != null) && (System.Array.BinarySearch(FGroups, AGroupName) >= 0);
        }

        /// <summary>
        /// tells if the user has the given role
        /// </summary>
        /// <param name="ARoleName"></param>
        /// <returns></returns>
        public Boolean IsInRole(string ARoleName)
        {
            return (FRoles != null) && (System.Array.BinarySearch(FRoles, ARoleName) >= 0);
        }

#if disabled
        /// <summary>
        /// tells if the user has the given permission to the given table
        /// </summary>
        /// <param name="APermission"></param>
        /// <param name="ADBTable"></param>
        /// <returns></returns>
        public Boolean IsTableAccessOK(TTableAccessPermission APermission, String ADBTable)
        {
            Boolean ReturnValue;
            SUserTableAccessPermissionRow FoundTableRow;

            DataRow[] FoundDataRows = FUserTableAccessPermissionDT.Select(
                SUserTableAccessPermissionTable.GetTableNameDBName() + " = '" + ADBTable + "'");

            if (FoundDataRows.Length != 0)
            {
                ReturnValue = true;
                FoundTableRow = (SUserTableAccessPermissionRow)FoundDataRows[0];

                switch (APermission)
                {
                    case TTableAccessPermission.tapINQUIRE:

                        if (!FoundTableRow.CanInquire)
                        {
                            ReturnValue = false;
                        }

                        break;

                    case TTableAccessPermission.tapMODIFY:

                        if (!FoundTableRow.CanModify)
                        {
                            ReturnValue = false;
                        }

                        break;

                    case TTableAccessPermission.tapCREATE:

                        if (!FoundTableRow.CanCreate)
                        {
                            ReturnValue = false;
                        }

                        break;

                    case TTableAccessPermission.tapDELETE:

                        if (!FoundTableRow.CanDelete)
                        {
                            ReturnValue = false;
                        }

                        break;
                }
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }
#endif

        /// <summary>
        /// check if user has access to the given module
        /// </summary>
        /// <param name="AModuleName"></param>
        /// <returns></returns>
        public Boolean IsInModule(string AModuleName)
        {
            return (FModuleAccess != null) && (System.Array.BinarySearch(FModuleAccess, AModuleName) >= 0);
        }

        /// <summary>
        /// Check if user can access this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public Boolean IsInLedger(Int32 ALedgerNumber)
        {
            return (FModuleAccess != null) && (System.Array.BinarySearch(FModuleAccess, "LEDGER" + ALedgerNumber.ToString("0000")) >= 0);
        }

        /// <summary>
        /// diagnostic string to show which modules a user has been given access to (separated by newlines)
        /// </summary>
        /// <returns>
        /// string permissions
        /// </returns>
        public string GetPermissions()
        {
            string permissions = string.Join(Environment.NewLine, FModuleAccess);

            return permissions;
        }

        #endregion
    }

    #region ELoginMessageAlreadySetException

    /// <summary>
    /// Thrown by TPetraPrincipal class if the LoginMessage property is written to althought it has already got a value
    /// </summary>
    public class ELoginMessageAlreadySetException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public ELoginMessageAlreadySetException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public ELoginMessageAlreadySetException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public ELoginMessageAlreadySetException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public ELoginMessageAlreadySetException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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

    #region EProcessIDAlreadySetException

    /// <summary>
    /// Thrown by TPetraPrincipal class if the ProcessID property is written to althought it has already got a value
    /// </summary>
    public class EProcessIDAlreadySetException : EOPAppException
    {
        /// <summary>
        /// Initializes a new instance of this Exception Class.
        /// </summary>
        public EProcessIDAlreadySetException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        public EProcessIDAlreadySetException(String AMessage) : base(AMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of this Exception Class with a specified error message and a reference to the inner <see cref="Exception" /> that is the cause of this <see cref="Exception" />.
        /// </summary>
        /// <param name="AMessage">The error message that explains the reason for the <see cref="Exception" />.</param>
        /// <param name="AInnerException">The <see cref="Exception" /> that is the cause of the current <see cref="Exception" />, or a null reference if no inner <see cref="Exception" /> is specified.</param>
        public EProcessIDAlreadySetException(string AMessage, Exception AInnerException) : base(AMessage, AInnerException)
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
        public EProcessIDAlreadySetException(SerializationInfo AInfo, StreamingContext AContext) : base(AInfo, AContext)
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
}
