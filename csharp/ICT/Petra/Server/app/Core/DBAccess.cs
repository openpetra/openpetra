//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.DBCaching;
using Ict.Petra.Shared;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Shared.Security;
using GNU.Gettext;
using System.Data;

namespace Ict.Petra.Server.App.Core.Security
{
    /// <summary>
    /// </summary>
    public delegate void TDelegateAddErrorLogEntry(string AErrorCode, string AContext, string AMessageLine1,
        string AMessageLine2, string AMessageLine3);

    /// <summary>
    /// This class provides protection for the database.
    /// It is specific to the Petra Database.
    /// It uses s_user_table_access_permission to get the permissions for the current user.
    /// The database object needs to be created with the constructor from this class,
    /// not with TDataBase.Create()
    /// </summary>
    public class TDataBasePetra : TDataBase
    {
        /// <summary>
        /// Log entry - do not translate!
        /// </summary>
        private static readonly string StrAccessDeniedLogPrefix = "DB ACCESS DENIED: ";

        /// <summary>set by EstablishDBConnection; used for access security</summary>
        private String FUsername;
        private TSQLCache FCache;
        private bool FRetrievingTablePermissions;

        // private TDelegateAddErrorLogEntry FAddErrorLogEntryDelegate;
        /// <summary>Returns the name of the current petra user, without the sql in the beginning or the end</summary>
        public String PetraUserName
        {
            get
            {
                return FUsername;
            }
        }

        /// <summary>
        /// </summary>
        public event TDelegateAddErrorLogEntry AddErrorLogEntryCallback;

        /// <summary>
        /// </summary>
        public TDataBasePetra() : base()
        {
            FCache = new TSQLCache();
            FRetrievingTablePermissions = false;
        }

        /// <summary>
        /// open a connection to an RDBMS
        /// </summary>
        /// <param name="ADataBaseType"></param>
        /// <param name="ADsn"></param>
        /// <param name="ADBPort"></param>
        /// <param name="ADatabaseName"></param>
        /// <param name="AUsername"></param>
        /// <param name="APassword"></param>
        /// <param name="AConnectionString"></param>
        /// <param name="APetraUserName"></param>
        public void EstablishDBConnection(TDBType ADataBaseType,
            String ADsn,
            String ADBPort,
            String ADatabaseName,
            String AUsername,
            String APassword,
            String AConnectionString,
            String APetraUserName)
        {
            FUsername = APetraUserName;
            FCache.Invalidate();

            // inherited
            EstablishDBConnection(ADataBaseType, ADsn, ADBPort, ADatabaseName, AUsername, APassword, AConnectionString);
        }

        /// <summary>
        /// This function checks if the current user has enough access rights to execute that query.
        /// </summary>
        /// <returns>true if the user has access, false if access is denied
        /// </returns>
        public new bool HasAccess(string ASQLStatement)
        {
            bool ReturnValue;

            System.Data.DataTable tab;
            String SQLStatement;
            System.Int32 Counter;
            System.Int32 EndOfNamePos;
            String TableName;
            char[] WhiteChar;
            DataRow[] FoundRows;
            String RequiredAccessPermission;
            String RequiredAccessPermission4GLName;
            String SQLTablePrecedingKeyword;
            String ErrorMessage;
            ReturnValue = false;
            try
            {
                // inherited
                if (HasAccess(ASQLStatement) == true)
                {
                    if (FRetrievingTablePermissions == true)
                    {
                        return true;
                    }

                    SQLStatement = ASQLStatement.Trim().ToUpper();
#if DEBUGMODE
                    if (TSrvSetting.DL >= 10)
                    {
                        Console.WriteLine("TDataBasePetra.HasAccess: SQLStatement: " + SQLStatement);
                    }
#endif

                    // get all the access rights to the tables of the user
                    // TODO 2 oChristianK cThread safety : This is currently not threadsave and probably not the most efficient way to use cached data. Change this.
                    FRetrievingTablePermissions = true;


                    tab = FCache.GetDataTable(
                        this,
                        "SELECT s_can_create_l, s_can_modify_l, s_can_delete_l, s_can_inquire_l, s_table_name_c FROM PUB_s_user_table_access_permission WHERE s_user_id_c = '"
                        +
                        FUsername + "'");

                    FRetrievingTablePermissions = false;
                    RequiredAccessPermission = "";

                    if (SQLStatement.IndexOf("SELECT") == 0)
                    {
                        RequiredAccessPermission = "s_can_inquire_l";
                        RequiredAccessPermission4GLName = "INQUIRE";
                        SQLTablePrecedingKeyword = "FROM";
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("TDataBasePetra.HasAccess: Access permission: " + RequiredAccessPermission4GLName);
                        }
#endif
                    }
                    else if (SQLStatement.IndexOf("UPDATE") == 0)
                    {
                        RequiredAccessPermission = "s_can_modify_l";
                        RequiredAccessPermission4GLName = "MODIFY";
                        SQLTablePrecedingKeyword = " ";
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("TDataBasePetra.HasAccess: Access permission: " + RequiredAccessPermission4GLName);
                        }
#endif
                    }
                    else if (SQLStatement.IndexOf("INSERT") == 0)
                    {
                        RequiredAccessPermission = "s_can_create_l";
                        RequiredAccessPermission4GLName = "CREATE";
                        SQLTablePrecedingKeyword = "INTO";
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("TDataBasePetra.HasAccess: Access permission: " + RequiredAccessPermission4GLName);
                        }
#endif
                    }
                    else if (SQLStatement.IndexOf("DELETE") == 0)
                    {
                        RequiredAccessPermission = "s_can_delete_l";
                        RequiredAccessPermission4GLName = "DELETE";
                        SQLTablePrecedingKeyword = "FROM";
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("TDataBasePetra.HasAccess: Access permission: " + RequiredAccessPermission4GLName);
                        }
#endif
                    }
                    else
                    {
                        TLogging.Log("DBAccessSecurity: SQL query could not be recognised. Starting with: " + SQLStatement.Substring(0, 10));
                        throw new Exception("DBAccessSecurity: SQL query could not be recognised.");
                    }

                    if (RequiredAccessPermission.Length != 0)
                    {
#if DEBUGMODE
                        if (TSrvSetting.DL >= 10)
                        {
                            Console.WriteLine("TDataBasePetra.HasAccess: RequiredAccessPermission.Length <> 0");
                        }
#endif

                        WhiteChar = new char[] {
                            ',', ')', '.', ' '
                        };

                        Counter = SQLStatement.IndexOf(SQLTablePrecedingKeyword);

                        if (Counter == -1)
                        {
                            TLogging.Log(
                                "DBAccessSecurity: SQL query could not be recognised. Keyword that should precede the DB table name (" +
                                SQLTablePrecedingKeyword + ") was not found!");
                            throw new Exception("DBAccessSecurity: SQL query could not be recognised.");
                        }

                        ReturnValue = true;

                        while (Counter != -1)
                        {
                            Counter = SQLStatement.IndexOf("PUB_", Counter);

                            if (Counter != -1)
                            {
                                EndOfNamePos = SQLStatement.IndexOfAny(WhiteChar, Counter);

                                if (EndOfNamePos == -1)
                                {
                                    EndOfNamePos = SQLStatement.Length;
                                }

                                TableName = SQLStatement.Substring(Counter + 4, EndOfNamePos - Counter - 4).Trim();
#if DEBUGMODE
                                if (TSrvSetting.DL >= 10)
                                {
                                    Console.WriteLine("TDataBasePetra.HasAccess: Table name: " + TableName);
                                }
#endif
                                Counter = Counter + TableName.Length;

                                if (TableName == "S_USER_DEFAULTS")
                                {
                                    // always allow access to the s_user_defaults
                                    // strangely enough, that is not in the table s_user_table_access_permission
                                }
                                else if ((RequiredAccessPermission == "s_can_inquire_l")
                                         && (TableName == "S_USER_MODULE_ACCESS_PERMISSION"))
                                {
                                    // always allow INQUIRE access to the
                                    // s_user_module_access_permission table
                                }
                                else
                                {
                                    // test for DB table
                                    FoundRows = tab.Select("s_table_name_c = '" + TableName + "'");

                                    if (FoundRows.Length == 0)
                                    {
                                        ErrorMessage = String.Format(Catalog.GetString(
                                                "You do not have permission to access {0}."), TableName.ToLower());
                                        TLogging.Log(StrAccessDeniedLogPrefix + ErrorMessage);
                                        LogInPetraErrorLog(ErrorMessage);
#if DEBUGMODE
                                        if (TSrvSetting.DL >= 10)
                                        {
                                            Console.WriteLine("TDataBasePetra.HasAccess: logged access error in DB Log Table.");
                                        }
#endif
                                        throw new ESecurityDBTableAccessDeniedException(String.Format(ErrorMessage,
                                                RequiredAccessPermission4GLName.ToLower(), TableName.ToLower()),
                                            RequiredAccessPermission4GLName.ToLower(), TableName.ToLower());
                                    }

                                    // test for access permission
                                    if (Convert.ToBoolean(FoundRows[0][RequiredAccessPermission]) == false)
                                    {
                                        ErrorMessage = String.Format(Catalog.GetString("You do not have permission to {0} {1} records."),
                                            RequiredAccessPermission4GLName.ToLower(),
                                            TableName.ToLower());
                                        TLogging.Log(StrAccessDeniedLogPrefix + ErrorMessage);
                                        LogInPetraErrorLog(ErrorMessage);
#if DEBUGMODE
                                        if (TSrvSetting.DL >= 10)
                                        {
                                            Console.WriteLine("TDataBasePetra.HasAccess: logged access error in DB Log Table.");
                                        }
#endif
                                        throw new ESecurityDBTableAccessDeniedException(ErrorMessage,
                                            RequiredAccessPermission4GLName.ToLower(), TableName.ToLower());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return ReturnValue;
        }

        void LogInPetraErrorLog(String AErrorMessage)
        {
            String MessageLine1 = "";
            String MessageLine2 = "";
            int LineBreakPosition;
            string newline = Environment.NewLine;

            if (AddErrorLogEntryCallback != null)
            {
                LineBreakPosition = AErrorMessage.IndexOf(newline);

                if (LineBreakPosition > 0)
                {
                    MessageLine1 = AErrorMessage.Substring(0, LineBreakPosition - 1);
                    MessageLine2 = AErrorMessage.Substring(LineBreakPosition + 2); // #10#13.Length
                }
                else
                {
                    MessageLine1 = AErrorMessage;
                }

                AddErrorLogEntryCallback(PetraErrorCodes.ERR_NOPERMISSIONTOACCESSTABLE,
                    "", MessageLine1, MessageLine2, "");
            }
        }
    }
}