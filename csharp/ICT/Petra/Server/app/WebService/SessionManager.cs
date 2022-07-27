//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

using Newtonsoft.Json;

using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Session;
using Ict.Common.Printing;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.Security;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Delegates;
using Ict.Petra.Server.App.Core.ServerAdmin.WebConnectors;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Server.MSysMan.WebConnectors;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;
using Ict.Petra.Server.app.JSClient;

namespace Ict.Petra.Server.App.WebService
{
/// <summary>
/// this publishes the SOAP web services of OpenPetra.org
/// </summary>
    [WebService(Namespace = "http://www.openpetra.org/webservices/SessionManager")]
    [ScriptService]
    public class TOpenPetraOrgSessionManager : System.Web.Services.WebService
    {
        /// <summary>
        /// constructor, which is called for each http request
        /// </summary>
        public TOpenPetraOrgSessionManager() : base()
        {
            try
            {
                Init();
            }
            catch (Exception e)
            {
                if (TLogWriter.GetLogFileName() == String.Empty)
                {
                    throw new Exception("Exception in TOpenPetraOrgSessionManager.Init(): " + e.ToString());
                }

                TLogging.Log("Exception in TOpenPetraOrgSessionManager.Init()");
                TLogging.Log(e.ToString());
                throw new Exception("Exception in TOpenPetraOrgSessionManager.Init()");
            }
        }

        /// <summary>
        /// initialise the server for each Web Request
        /// </summary>
        private static bool Init()
        {
            // make sure the correct config file is used
            string Instance = HttpContext.Current.Request.Url.ToString().Replace("http://", "").Replace("https://", "");
            Instance = Instance.Substring(0, Instance.IndexOf(".")).Replace("openpetra", "OPENPETRA").Replace("op_","op").Replace("op", "op_").ToLower();

            // for demo etc
            if (!Instance.StartsWith("op_"))
            {
                Instance = "op_" + Instance;
            }

            string ConfigFileName = "/home/" + Instance + "/etc/PetraServerConsole.config";

            if (!File.Exists(ConfigFileName))
            {
                // multi tenant on shared hosting
                ConfigFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "instances") + "/" + Instance + "/etc/PetraServerConsole.config";
            }

            if (!File.Exists(ConfigFileName))
            {
                // single tenant on shared hosting
                ConfigFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Instance) + "/etc/PetraServerConsole.config";
            }

            if (File.Exists(ConfigFileName))
            {
                // we are in a multi tenant hosting scenario
            }
            else if (Environment.CommandLine.Contains("/appconfigfile="))
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

            TTypedDataTable.ResetStaticVariables();
            TPdfPrinter.ResetStaticVariables();
            THTTPUtils.ResetStaticVariables();
            TServerManagerBase.ResetStaticVariables();
            TClientManager.ResetStaticVariables();

            TSession.InitThread("TOpenPetraOrgSessionManager.Init", ConfigFileName);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Server.ScriptTimeout = Convert.ToInt32(
                    TimeSpan.FromMinutes(TAppSettingsManager.GetInt32("WebRequestTimeOutInMinutes", 15)).
                    TotalSeconds);
            }

            // if the Login Method is called: reset cookie, ignore any old session
            if ((HttpContext.Current != null) && (HttpContext.Current.Request.PathInfo == "/Login"))
            {
                TSession.CloseSession();
                TSession.InitThread("TOpenPetraOrgSessionManager.Init Reset", ConfigFileName);
            }

            Catalog.Init();

            ErrorCodeInventory.Init();
            ErrorCodeInventory.BuildErrorCodeInventory(typeof(Ict.Petra.Shared.PetraErrorCodes));
            ErrorCodeInventory.BuildErrorCodeInventory(typeof(Ict.Common.Verification.TStringChecks));

            TServerManager.TheServerManager = new TServerManager();

            // initialise the cached tables and the delegates
            TSetupDelegates.Init();

            TLogging.LogAtLevel(4, "Server has been initialised");

            return true;
        }

        private eLoginEnum LoginInternal(string username, string password, Version AClientVersion,
            out Int32 AClientID,
            out string AWelcomeMessage,
            out Boolean ASystemEnabled,
            out Boolean AMustChangePassword)
        {
            ASystemEnabled = true;
            AWelcomeMessage = string.Empty;
            AClientID = -1;
            AMustChangePassword = false;
            Int64 SiteKey;

            try
            {
                if (username.ToUpper() == "SELFSERVICE")
                {
                    throw new Exception("Login with user SELFSERVICE is not permitted");
                }

                TConnectedClient CurrentClient = TClientManager.ConnectClient(
                    username.ToUpper(), password.Trim(),
                    HttpContext.Current.Request.UserHostName,
                    HttpContext.Current.Request.UserHostAddress,
                    AClientVersion,
                    TClientServerConnectionType.csctRemote,
                    out AClientID,
                    out AWelcomeMessage,
                    out ASystemEnabled,
                    out SiteKey);
                TSession.SetVariable("LoggedIn", true);

                // the following values are stored in the session object
                DomainManager.GClientID = AClientID;
                DomainManager.CurrentClient = CurrentClient;
                DomainManager.GSiteKey = SiteKey;

                AMustChangePassword = (UserInfo.GetUserInfo().LoginMessage == SharedConstants.LOGINMUSTCHANGEPASSWORD);

                return eLoginEnum.eLoginSucceeded;
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);
                TLogging.Log(e.StackTrace);
                TSession.SetVariable("LoggedIn", false);

                TSession.CloseSession();
                return TClientManager.LoginErrorFromException(e);
            }
        }

        /// <summary>Login a user</summary>
        [WebMethod(EnableSession = true)]
        public string Login(string username, string password)
        {
            string WelcomeMessage;
            bool SystemEnabled;
            Int32 ClientID;
            bool MustChangePassword;

            eLoginEnum resultCode =  LoginInternal(username, password, TFileVersionInfo.GetApplicationVersion().ToVersion(),
                out ClientID, out WelcomeMessage, out SystemEnabled, out MustChangePassword);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("resultcode", resultCode.ToString());
            result.Add("mustchangepassword", MustChangePassword);

            if (resultCode == eLoginEnum.eLoginSucceeded)
            {
                result.Add("ModulePermissions", UserInfo.GetUserInfo().GetPermissions());
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>get the current version of OpenPetra</summary>
        [WebMethod(EnableSession = true)]
        public string GetVersion()
        {
            object loggedIn = TSession.GetVariable("LoggedIn");

            if ((null != loggedIn) && ((bool)loggedIn == true))
            {
                return TFileVersionInfo.GetApplicationVersion().ToVersion().ToString();
            }

            return "0.0.0.0";
        }

        /// <summary>check if the user has logged in successfully</summary>
        [WebMethod(EnableSession = true)]
        public string IsUserLoggedIn()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            object loggedIn = TSession.GetVariable("LoggedIn");

            if ((null != loggedIn) && ((bool)loggedIn == true))
            {
                result.Add("resultcode", "success");
            }
            else
            {
                result.Add("selfsignupEnabled", TMaintenanceWebConnector.SignUpSelfServiceEnabled()?"true":"false");
                result.Add("resultcode", "error");
            }

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>set the initial email address for user SYSADMIN</summary>
        [WebMethod(EnableSession = true)]
        public bool SetInitialSysadminEmail(string AEmailAddress, string AFirstName, string ALastName, string ALanguageCode, string AAuthToken)
        {
            bool result = true;

            string requiredToken = TAppSettingsManager.GetValue("AuthTokenForInitialisation");
            if ((AAuthToken != requiredToken) || (requiredToken == String.Empty) )
            {
                return false;
            }

            string UserEmailAddress = String.Empty;
            string UserID = String.Empty;

            UserInfo.SetUserInfo(new TPetraPrincipal("SYSADMIN"));

            if (TMaintenanceWebConnector.SetInitialSysadminEmail(AEmailAddress, AFirstName, ALastName, ALanguageCode))
            {
                // create unprivileged user as well
                if (AEmailAddress.Contains("+sysadmin@"))
                {
                    string InitialModulePermissions;
                    Int64 SiteKey;
                    string InitialPassword;
                    string FirstName;
                    string LastName;
                    string LanguageCode;

                    result = TSettingsWebConnector.GetDefaultsForFirstSetup(
                        ALanguageCode,
                        out UserID,
                        out FirstName,
                        out LastName,
                        out LanguageCode,
                        out UserEmailAddress,
                        out InitialModulePermissions,
                        out InitialPassword,
                        out SiteKey);

                    if (result)
                    {
                        TVerificationResultCollection VerificationResult;
                        result = TSettingsWebConnector.RunFirstSetup(
                            UserID,
                            FirstName,
                            LastName,
                            LanguageCode,
                            UserEmailAddress,
                            InitialModulePermissions.Split(',').ToList(),
                            "",
                            SiteKey,
                            false,
                            out VerificationResult);
                    }
                }

                if (result)
                {
                    return TMaintenanceWebConnector.SendWelcomeEmail(AEmailAddress, UserEmailAddress, UserID, AFirstName, ALastName, ALanguageCode);
                }
            }

            return false;
        }

        /// <summary>send out an e-mail for setting a new password</summary>
        [WebMethod(EnableSession = true)]
        public bool RequestNewPassword(string AEmailAddress)
        {
            return TMaintenanceWebConnector.RequestNewPassword(AEmailAddress);
        }

        /// <summary>send out an e-mail for creating a self-service account</summary>
        [WebMethod(EnableSession = true)]
        public string SignUpSelfService(string AEmailAddress, string AFirstName, string ALastName, string APassword, string ALanguageCode, out TVerificationResultCollection AVerification)
        {
            AVerification = new TVerificationResultCollection();

            try
            {
                TServerAdminWebConnector.LoginServerAdmin("SELFSERVICE");
                bool Result = TMaintenanceWebConnector.SignUpSelfService(AEmailAddress, AFirstName, ALastName, APassword, ALanguageCode, out AVerification);
                Logout();
                return "{" + "\"AVerification\": " + THttpBinarySerializer.SerializeObject(AVerification)+ "," + "\"result\": "+THttpBinarySerializer.SerializeObject(Result)+ "}";
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during SignUpSelfService:" + Environment.NewLine +
                    Exc.ToString());

                throw;
            }
        }

        /// <summary>confirm the e-mail address for the self service account</summary>
        [WebMethod(EnableSession = true)]
        public bool SignUpSelfServiceConfirm(string AUserID, string AToken)
        {
            try
            {
                TServerAdminWebConnector.LoginServerAdmin("SELFSERVICE");
                bool Result = TMaintenanceWebConnector.SignUpSelfServiceConfirm(AUserID, AToken);
                Logout();
                return Result;
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during SignUpSelfServiceConfirm:" + Environment.NewLine +
                    Exc.ToString());

                throw;
            }
        }

        /// <summary>set a new password with a token that was sent via e-mail</summary>
        [WebMethod(EnableSession = true)]
        public string SetNewPassword(string AUserID, string AToken, string ANewPassword)
        {
            // make sure we are logged out. especially SYSADMIN could be logged in when a new user is created.
            Logout();

            TSession.InitThread("SetNewPassword", TAppSettingsManager.ConfigFileName);

            TVerificationResultCollection VerificationResult;
            bool Result = TMaintenanceWebConnector.SetNewPassword(AUserID, AToken, ANewPassword, out VerificationResult);
            return "{" + "\"AVerificationResult\": " + THttpBinarySerializer.SerializeObject(VerificationResult)+ "," + "\"result\": "+THttpBinarySerializer.SerializeObject(Result)+ "}";
        }

        /// <summary>log the user out</summary>
        [WebMethod(EnableSession = true)]
        public string Logout()
        {
            string clientName = "unknown";

            if (DomainManager.CurrentClient != null)
            {
                clientName = DomainManager.CurrentClient.ClientName;
            }

            TLogging.Log("Logout from session: ClientName=" + clientName, TLoggingType.ToLogfile | TLoggingType.ToConsole);

            if (DomainManager.CurrentClient == null)
            {
                TSession.CloseSession();
            }
            else
            {
                DomainManager.CurrentClient.EndSession();
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("resultcode", "success");
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>get the navigation menu</summary>
        [WebMethod(EnableSession = true)]
        public string GetNavigationMenu()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (UserInfo.GetUserInfo() == null)
            {
                result.Add("resultcode", "error");
                result.Add("error", "invalid user");
                return JsonConvert.SerializeObject(result);
            }

            result.Add("resultcode", "success");
            result.Add("navigation", new TUINavigation().LoadNavigationUI());

            string assistant = String.Empty;

            if (assistant == String.Empty)
            {
                assistant = TSettingsWebConnector.GetPasswordNeedsChange();
            }

            if (assistant == String.Empty)
            {
                assistant = TSettingsWebConnector.GetSetupAssistant();
            }

            if (assistant == String.Empty)
            {
                assistant = TMaintenanceWebConnector.GetSelfServiceAssistant();
            }

            if (assistant == String.Empty)
            {
                assistant = TGLSetupWebConnector.GetLedgerSetupAssistant();
            }

            result.Add("assistant", assistant);

            return JsonConvert.SerializeObject(result);
        }

#if notused
        /// <summary>load a specific navigation page</summary>
        [WebMethod(EnableSession = true)]
        public string LoadNavigationPage(string ANavigationPage)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (UserInfo.GetUserInfo() == null)
            {
                result.Add("resultcode", "error");
                result.Add("error", "invalid user");
                return JsonConvert.SerializeObject(result);
            }

            string htmlcode = new TUINavigation().LoadNavigationPage(ANavigationPage);

            if (htmlcode.StartsWith("error:"))
            {
                result.Add("resultcode", "error");
                result.Add("message", htmlcode);
            } else {
                result.Add("resultcode", "success");
                result.Add("htmlpage", htmlcode);
            }
                
            return JsonConvert.SerializeObject(result);
        }
#endif

        /// <summary>client gets tasks, and lets the server know that it is still connected</summary>
        [WebMethod(EnableSession = true)]
        public string PollClientTasks()
        {
            try
            {
                if (UserInfo.GetUserInfo() == null)
                {
                    TLogging.Log("PollClientTasks: GUserInfo == null!");
                    return THttpBinarySerializer.SerializeObject(false);
                }
                
                if (DomainManager.CurrentClient == null)
                {
                    TLogging.Log("PollClientTasks: CurrentClient == null!");
                    return THttpBinarySerializer.SerializeObject(false);
                }

                return THttpBinarySerializer.SerializeObject(DomainManager.CurrentClient.FPollClientTasks.PollClientTasks());
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
