//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Data;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Session;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.Security;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Delegates;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;

namespace Ict.Petra.Server.App.WebService
{
/// <summary>
/// this publishes the SOAP web services of OpenPetra.org
/// </summary>
    [WebService(Namespace = "http://www.openpetra.org/webservices/SessionManager")]
    [ScriptService]
    public class TOpenPetraOrgSessionManager : System.Web.Services.WebService
    {
        private static string ConfigFileName = string.Empty;

        /// <summary>
        /// constructor, which is called for each http request
        /// </summary>
        public TOpenPetraOrgSessionManager() : base()
        {
            if (TLogging.DebugLevel >= 4)
            {
                TLogging.Log(HttpContext.Current.Request.PathInfo);
            }

            Init();
        }

        /// <summary>
        /// initialise the server once for everyone
        /// </summary>
        public static bool Init()
        {
            if (ConfigFileName == string.Empty)
            {
                // make sure the correct config file is used
                if (Environment.CommandLine.Contains("/appconfigfile="))
                {
                    // this happens when we use fastcgi-mono-server4
                    ConfigFileName = Environment.CommandLine.Substring(
                        Environment.CommandLine.IndexOf("/appconfigfile=") + "/appconfigfile=".Length);

                    if (ConfigFileName.IndexOf(" ") != -1)
                    {
                        ConfigFileName = ConfigFileName.Substring(0, ConfigFileName.IndexOf(" "));
                    }
                }
                else
                {
                    // this is the normal behaviour when running with local http server
                    ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config";
                }
            }

            new TAppSettingsManager(ConfigFileName);
            new TSrvSetting();
            new TLogging(TSrvSetting.ServerLogFile);
            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(
                    TimeSpan.FromMinutes(TAppSettingsManager.GetInt32("WebRequestTimeOutInMinutes", 15)).
                    TotalSeconds);
            }

            // if the Login Method is called: reset cookie, ignore any old session
            if ((HttpContext.Current != null) && (HttpContext.Current.Request.PathInfo == "/Login"))
            {
                TSession.Clear();
            }

            if (TServerManager.TheServerManager == null)
            {
                Catalog.Init();

                TServerManager.TheServerManager = new TServerManager();

                try
                {
                    TServerManager.TheCastedServerManager.EstablishDBConnection();

                    TSystemDefaultsCache.GSystemDefaultsCache = new TSystemDefaultsCache();
                    DomainManager.GSiteKey = TSystemDefaultsCache.GSystemDefaultsCache.GetInt64Default(
                        Ict.Petra.Shared.SharedConstants.SYSDEFAULT_SITEKEY);

                    TLanguageCulture.Init();

                    // initialise the cached tables
                    TSetupDelegates.Init();

                    TUserDefaults.InitializeUnit();
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                    throw;
                }

                TLogging.Log("Server has been initialised");

                return true;
            }

            if (DomainManager.CurrentClient != null)
            {
                if (DomainManager.CurrentClient.FAppDomainStatus == TSessionStatus.adsStopped)
                {
                    TLogging.Log("There is an attempt to reconnect to stopped session: " + DomainManager.CurrentClient.ClientName);

                    HttpContext.Current.Response.Status = "404 " + THTTPUtils.SESSION_ALREADY_CLOSED;
                    HttpContext.Current.Response.End();
                }

//                TLogging.Log("Init(): WebService Method name that got called: " + HttpContext.Current.Request.PathInfo);

                if (HttpContext.Current.Request.PathInfo != "/PollClientTasks")
                {
                    DomainManager.CurrentClient.UpdateLastAccessTime();
                }
            }

            return false;
        }

        private eLoginEnum LoginInternal(string username, string password, Version AClientVersion,
            out Int32 AClientID,
            out string AWelcomeMessage,
            out Boolean ASystemEnabled,
            out IPrincipal AUserInfo)
        {
            AUserInfo = null;
            ASystemEnabled = true;
            AWelcomeMessage = string.Empty;
            AClientID = -1;

            if (DBAccess.GDBAccessObj == null)
            {
                TServerManager.TheCastedServerManager.EstablishDBConnection();
            }

            try
            {
                TConnectedClient CurrentClient = TClientManager.ConnectClient(
                    username.ToUpper(), password.Trim(),
                    HttpContext.Current.Request.UserHostName,
                    HttpContext.Current.Request.UserHostAddress,
                    AClientVersion,
                    TClientServerConnectionType.csctRemote,
                    out AClientID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out AUserInfo);
                TSession.SetVariable("LoggedIn", true);

                // the following values are stored in the session object
                DomainManager.GClientID = AClientID;
                DomainManager.CurrentClient = CurrentClient;
                UserInfo.GUserInfo = (TPetraPrincipal)AUserInfo;

                DBAccess.GDBAccessObj.UserID = username.ToUpper();

                TServerManager.TheCastedServerManager.AddDBConnection(DBAccess.GDBAccessObj);

                return eLoginEnum.eLoginSucceeded;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                TSession.SetVariable("LoggedIn", false);

                if (DBAccess.GDBAccessObj != null)
                {
                    DBAccess.GDBAccessObj.CloseDBConnection();
                }

                TSession.Clear();
                return TClientManager.LoginErrorFromException(e);
            }
        }

        /// <summary>Login a user</summary>
        [WebMethod(EnableSession = true)]
        public eLoginEnum Login(string username, string password)
        {
            string WelcomeMessage;
            bool SystemEnabled;
            IPrincipal UserInfo;
            Int32 ClientID;

            return LoginInternal(username, password, TFileVersionInfo.GetApplicationVersion().ToVersion(),
                out ClientID, out WelcomeMessage, out SystemEnabled, out UserInfo);
        }

        /// <summary>Login a user</summary>
        [WebMethod(EnableSession = true)]
        public string LoginClient(string username, string password, string version)
        {
            Version ClientVersion;

            try
            {
                ClientVersion = Version.Parse(version);
            }
            catch (Exception)
            {
                TLogging.Log("LoginClient: invalid version, cannot be parsed: " + version);
                return THttpBinarySerializer.SerializeObjectWithType(eLoginEnum.eLoginVersionMismatch);
            }

            string WelcomeMessage;
            bool SystemEnabled;
            IPrincipal UserInfo;
            Int32 ClientID;
            eLoginEnum Result = LoginInternal(username, password, ClientVersion,
                out ClientID, out WelcomeMessage, out SystemEnabled, out UserInfo);

            if (Result != eLoginEnum.eLoginSucceeded)
            {
                return THttpBinarySerializer.SerializeObjectWithType(Result);
            }

            return THttpBinarySerializer.SerializeObjectWithType(Result) + "," +
                   THttpBinarySerializer.SerializeObjectWithType(ClientID) + "," +
                   THttpBinarySerializer.SerializeObjectWithType(WelcomeMessage) + "," +
                   THttpBinarySerializer.SerializeObjectWithType(SystemEnabled) + "," +
                   THttpBinarySerializer.SerializeObjectWithType(UserInfo);
        }

        /// <summary>
        /// TODO: we should only use one database object per request, and not have global variables for database connections
        /// </summary>
        private static SortedList <string, TDataBase>FDatabaseObjects = new SortedList <string, TDataBase>();

        static private TDataBase GetDatabaseFromSession(bool AOpenConnection = true)
        {
            // if another thread gets called, then the session object is null
            if (HttpContext.Current == null)
            {
                if (Thread.CurrentThread.Name == null)
                {
                    throw new Exception(
                        "TOpenPetraOrgSessionManager.GetDatabaseFromSession: we do need a name for the thread for managing the database connection");
                }

                if (!FDatabaseObjects.ContainsKey(Thread.CurrentThread.Name))
                {
                    TDataBase db = new TDataBase();

                    if (AOpenConnection)
                    {
                        db.EstablishDBConnection(TSrvSetting.RDMBSType,
                            TSrvSetting.PostgreSQLServer,
                            TSrvSetting.PostgreSQLServerPort,
                            TSrvSetting.PostgreSQLDatabaseName,
                            TSrvSetting.DBUsername,
                            TSrvSetting.DBPassword,
                            "");
                    }

                    FDatabaseObjects.Add(Thread.CurrentThread.Name, db);
                }

                return FDatabaseObjects[Thread.CurrentThread.Name];
            }

            if (TSession.GetVariable("DBAccessObj") == null)
            {
                if (TServerManager.TheServerManager == null)
                {
                    TLogging.Log("GetDatabaseFromSession : TheServerManager is null");
                }
                else
                {
                    // disconnect web user after 2 minutes of inactivity. should disconnect itself already earlier
                    TServerManager.TheCastedServerManager.DisconnectTimedoutDatabaseConnections(2 * 60, "ANONYMOUS");

                    // disconnect normal users after 3 hours of inactivity
                    TServerManager.TheCastedServerManager.DisconnectTimedoutDatabaseConnections(3 * 60 * 60, "");

                    if (AOpenConnection)
                    {
                        TServerManager.TheCastedServerManager.EstablishDBConnection();
                    }
                }
            }

            return (TDataBase)TSession.GetVariable("DBAccessObj");
        }

        static private void SetDatabaseForSession(TDataBase database)
        {
            if (Thread.CurrentThread.Name == null)
            {
                // TLogging.Log("there is a new thread for session " + HttpContext.Current.Session.SessionID);
                System.Threading.Thread.CurrentThread.Name = "MainThread" + Guid.NewGuid().ToString();;
            }

            TSession.SetVariable("DBAccessObj", database);
        }

        static private TPetraPrincipal GetUserInfoFromSession()
        {
            return (TPetraPrincipal)TSession.GetVariable("UserInfo");
        }

        static private void SetUserInfoForSession(TPetraPrincipal userinfo)
        {
            TSession.SetVariable("UserInfo", userinfo);
        }

        /// <summary>check if the user has logged in successfully</summary>
        [WebMethod(EnableSession = true)]
        public bool IsUserLoggedIn()
        {
            object loggedIn = TSession.GetVariable("LoggedIn");

            if ((null != loggedIn) && ((bool)loggedIn == true))
            {
                return true;
            }

            return false;
        }

        /// <summary>log the user out</summary>
        [WebMethod(EnableSession = true)]
        public bool Logout()
        {
            TLogging.Log("Logout from session: ClientName=" + DomainManager.CurrentClient.ClientName, TLoggingType.ToLogfile | TLoggingType.ToConsole);

            if (DomainManager.CurrentClient == null)
            {
                TSession.Clear();
            }
            else
            {
                DomainManager.CurrentClient.EndSession();
            }

            return true;
        }

        /// <summary>get user information</summary>
        [WebMethod(EnableSession = true)]
        public string GetUserInfo()
        {
            if (UserInfo.GUserInfo == null)
            {
                return THttpBinarySerializer.SerializeObject(false);
            }

            return THttpBinarySerializer.SerializeObject(UserInfo.GUserInfo, true);
        }

        /// <summary>client gets tasks, and lets the server know that it is still connected</summary>
        [WebMethod(EnableSession = true)]
        public string PollClientTasks()
        {
            try
            {
                if (UserInfo.GUserInfo == null)
                {
                    TLogging.Log("PollClientTasks: GUserInfo == null!");
                    return THttpBinarySerializer.SerializeObject(false);
                }

                return THttpBinarySerializer.SerializeObject(DomainManager.CurrentClient.FPollClientTasks.PollClientTasks(), true);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return string.Empty;
            }
        }

#if TODORemoting

        /**
         * Calling DisconnectClient tears down the Client's AppDomain and therefore
         * kills all remoted objects for the Client that were not released before.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Boolean DisconnectClient(System.Int32 AClientID, out String ACantDisconnectReason);


        /**
         * Calling DisconnectClient tears down the Client's AppDomain and therefore
         * kills all remoted objects for the Client that were not released before.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Boolean DisconnectClient(System.Int32 AClientID, String AReason, out String ACantDisconnectReason);

        /**
         * Can be called to queue a ClientTask for a certain Client.
         *
         * See implementation of this class for more detailed description!
         *
         */
        Int32 QueueClientTaskFromClient(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID = -1,
            object ATaskParameter1 = null,
            object ATaskParameter2 = null,
            object ATaskParameter3 = null,
            object ATaskParameter4 = null);

        /// <summary>
        /// add error to the log
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        /// <param name="AUserID"></param>
        /// <param name="AProcessID"></param>
        void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID);


        /**
         * The following functions are only for development purposes (note that these
         * functions can also be invoked directly from the Server's menu)
         *
         */
        System.Int32 GCGetGCGeneration(object AInspectObject);

        /// <summary>
        /// perform garbage collection
        /// </summary>
        /// <returns></returns>
        System.Int32 GCPerformGC();

        /// <summary>
        /// see how much memory is available
        /// </summary>
        /// <returns></returns>
        System.Int32 GCGetApproxMemory();
#endif
    }
}