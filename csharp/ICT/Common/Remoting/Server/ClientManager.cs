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
using System.Collections;
using System.Data;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using GNU.Gettext;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Main class for Client connection and disconnection and other Client actions.
    ///
    /// TClientManager is also used by TServerManager to perform actions on connected
    /// Clients and to request information about Clients.
    ///
    /// </summary>
    public class TClientManager
    {
        const string CLIENTINITIATED_DISCONNECTION = "Disconnection requested by Client";

        #region Resourcestrings

        private static readonly string StrClientServerExeProgramVersionMismatchMessage = Catalog.GetString(
            "The Program Version of your OpenPetra Client ({0}) does not match the Program Version of the OpenPetra " +
            "Server ({1}).\r\nAn OpenPetra Client cannot connect to an OpenPetra " +
            "Server unless the Program Versions match. You need to install an OpenPetra " +
            "Client with the correct Program Version.");

        #endregion

        private static IUserManager UUserManager = null; // STATIC_OK: will be set for each request
        private static IErrorLog UErrorLog = null; // STATIC_OK: will be set for each request
        private static ILoginLog ULoginLog = null; // STATIC_OK: will be set for each request
        private static IMaintenanceLogonMessage UMaintenanceLogonMessage = null; // STATIC_OK: will be set for each request

        /// <summary>
        /// Called by TClientManager to request the number of Clients that are currently
        /// connected to the Petra Server.
        ///
        /// </summary>
        public static System.Int32 ClientsConnected
        {
            get
            {
                // TODO: calculate the currently open sessions from the s_session table???
                return -1;
            }
        }

        /// <summary>
        /// Called by TClientManager to request the total number of Clients that
        /// connected to the Petra Server since the start of the Petra Server.
        ///
        /// </summary>
        public static System.Int32 ClientsConnectedTotal
        {
            get
            {
                // TODO: calculate the sessions within the last 24 hours from the s_session table???
                return -1;
            }
        }

        /// make sure that not a null value is returned, but an empty string
        private static string ValueOrEmpty(string s)
        {
            if (s == null)
            {
                return "";
            }

            return s;
        }

        /// <summary>
        /// Formats the client list array for output in a fixed-width font (eg. to the
        /// Console)
        ///
        /// </summary>
        /// <returns>Formatted client list.
        /// </returns>
        public static String FormatClientList(Boolean AListDisconnectedClients)
        {
            String ClientLines;
            ArrayList ClientsArrayList;

            ClientsArrayList = TClientManager.ClientList(AListDisconnectedClients);

            if (ClientsArrayList.Count > 0)
            {
                ClientLines =
                    Catalog.GetString("  ID | Client          | Status           | Computer    | IP Address      | Type") +
                    Environment.NewLine +
                    "----+-----------------+------------------+-------------+-----------------+-----" + Environment.NewLine +
                    Catalog.GetString("     | Connected since | Last activity    |             |                 |") +
                    Environment.NewLine;

                if (AListDisconnectedClients)
                {
                    // 'Last activity' column becomes 'Disconnected at' column
                    ClientLines = ClientLines.Replace("Last activity  ", "Disconnected at");
                }

                int ClientLine = 1;

                foreach (string[] currentClient in ClientsArrayList)
                {
                    ClientLines = ClientLines +
                                  ValueOrEmpty(currentClient[0]).PadLeft(4) + " | " +
                                  ValueOrEmpty(currentClient[1]).PadRight(15) + " | " +
                                  ValueOrEmpty(currentClient[2]).PadRight(16) + " | " +
                                  ValueOrEmpty(currentClient[5]).PadRight(11) + " | " +
                                  ValueOrEmpty(currentClient[6]).PadRight(15) + " | " +
                                  ValueOrEmpty(currentClient[7]) + Environment.NewLine + "     | " +
                                  ValueOrEmpty(currentClient[3]).PadRight(15) + " | " +
                                  ValueOrEmpty(currentClient[4]).PadRight(16) + " | " +
                                  Environment.NewLine;

                    ClientLine++;
                }
            }
            else
            {
                if (!AListDisconnectedClients)
                {
                    ClientLines = Catalog.GetString(" * no connected Clients *") + Environment.NewLine;
                }
                else
                {
                    ClientLines = Catalog.GetString(" * no disconnected Clients *") + Environment.NewLine;
                }
            }

            ClientLines = ClientLines +
                          String.Format(Catalog.GetString("  (Currently connected Clients: {0}; client connections since Server start: {1})"),
                ClientsConnected, ClientsConnectedTotal);
            return ClientLines;
        }

        /// <summary>
        /// Formats the client list array for output in the sysadm dialog for selection of a client id
        ///
        /// </summary>
        /// <returns>Formatted client list for sysadm dialog.
        /// </returns>
        public static String FormatClientListSysadm(Boolean AListDisconnectedClients)
        {
            String ReturnValue;

            //System.Int16 ClientLine;
            ArrayList ClientsArrayList;

            ClientsArrayList = TClientManager.ClientList(AListDisconnectedClients);
            ReturnValue = "";

            if (ClientsArrayList.Count > 0)
            {
                // the format is "22" "TIMOP" "23" "CHRISTIANK" (ClientId and UserName/Description)
                foreach (string[] currentClient in ClientsArrayList)
                {
                    ReturnValue = ReturnValue + " \"" +
                                  (currentClient[0]) + "\" \"" + // clientid
                                  (currentClient[1]).PadRight(15) + // username
                                  (currentClient[5]).PadRight(15) + "\""; // computer
                }
            }

            return ReturnValue;
        }

        /// reset the static variables for each Web Request call.
        public static void ResetStaticVariables()
        {
            UUserManager = null;
            UErrorLog = null;
            ULoginLog = null;
            UMaintenanceLogonMessage = null;
        }

        /// <summary>
        /// initialize variables that are initialized from classes specific to the server, eg. with access to OpenPetra database
        /// </summary>
        public static void InitializeStaticVariables(
            IUserManager AUserManager,
            IErrorLog AErrorLog,
            ILoginLog ALoginLog,
            IMaintenanceLogonMessage AMaintenanceLogonMessage)
        {
            UUserManager = AUserManager;
            UErrorLog = AErrorLog;
            ULoginLog = ALoginLog;
            UMaintenanceLogonMessage = AMaintenanceLogonMessage;
        }

        /// <summary>
        /// public for stateless (webservice) authentication
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AClientComputerName"></param>
        /// <param name="AClientIPAddress"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="ATransaction">Instantiated DB Transaction.</param>
        /// <returns></returns>
        static public bool PerformLoginChecks(String AUserName,
            String APassword,
            String AClientComputerName,
            String AClientIPAddress,
            out Boolean ASystemEnabled,
            TDBTransaction ATransaction)
        {
            bool ReturnValue;

            if (UUserManager == null)
            {
                throw new Exception("TClientManager.PerformLoginChecks Configuration error: no valid IUserManager has been installed for the server!");
            }

            try
            {
                // This function call will throw various Exceptions if the User doesn't exist or cannot be authenticated!
                ReturnValue = UUserManager.PerformUserAuthentication(AUserName,
                    APassword, AClientComputerName, AClientIPAddress,
                    out ASystemEnabled,
                    ATransaction);
            }
            catch (EUserNotExistantException)
            {
                LogFailedUserAuthentication(AUserName, "User does not exist in the OpenPetra Database!",
                    AClientComputerName, AClientIPAddress);

                // Simulate that time is spent on 'authenticating' a user (although the user doesn't exist)...! Reason for that: see Method
                // SimulatePasswordAuthenticationForNonExistingUser!
                UUserManager.SimulatePasswordAuthenticationForNonExistingUser();

                // for security reasons we don't distinguish between a nonexisting user and a wrong password when informing the Client!
                throw;
            }
            catch (EPasswordWrongException PasswordWrongException)
            {
                LogFailedUserAuthentication(AUserName, "The Password that the user supplied is wrong!",
                    AClientComputerName, AClientIPAddress);

                // for security reasons we don't distinguish between a nonexisting user and a wrong password when informing the Client!
                throw new EUserNotExistantException(PasswordWrongException.Message);
            }
            catch (EUserAccountGotLockedException UserAccountGotLockedException)
            {
                LogFailedUserAuthentication(AUserName,
                    "User tried to log in, but the password that the user supplied is wrong and now the account got locked " +
                    "after too many failed log-ins.",
                    AClientComputerName, AClientIPAddress);

                // for security reasons we don't distinguish between a user account that got Locked and a wrong username/password combination when informing the Client!
                throw new EUserNotExistantException(UserAccountGotLockedException.Message);
            }
            catch (EUserAccountLockedException UserAccountLockedException)
            {
                LogFailedUserAuthentication(AUserName,
                    "User tried to log in, but that user account is locked.",
                    AClientComputerName, AClientIPAddress);

                // for security reasons we don't distinguish between user account that is Locked and a wrong username/password combination when informing the Client!
                throw new EUserNotExistantException(UserAccountLockedException.Message);
            }
            catch (EUserRetiredException UserRetiredException)
            {
                LogFailedUserAuthentication(AUserName,
                    "User tried to log in, but that user is 'retired'.",
                    AClientComputerName, AClientIPAddress);

                // for security reasons we don't distinguish between user that is retired and a wrong username/password combination when informing the Client!
                throw new EUserNotExistantException(UserRetiredException.Message);
            }
            catch (ESystemDisabledException)
            {
                LogFailedUserAuthentication(AUserName, "The System is currently Disabled",
                    AClientComputerName, AClientIPAddress);

                throw;
            }
            catch (ELicenseExpiredException)
            {
                LogFailedUserAuthentication(AUserName, "The System is not licensed",
                    AClientComputerName, AClientIPAddress);

                throw;
            }
            catch (EDBConnectionBrokenException)
            {
                LogFailedUserAuthentication(AUserName, "The database connection is currently not available",
                    AClientComputerName, AClientIPAddress);

                throw;
            }
            catch (Exception exp)
            {
                LogFailedUserAuthentication(AUserName, "Exception occured: " + exp.ToString(),
                    AClientComputerName, AClientIPAddress);

                throw;
            }

            return ReturnValue;
        }

        private static void LogFailedUserAuthentication(string AUserName, string AReason,
            string AClientComputerName, string AClientIPAddress)
        {
            const String AUTHENTICATION_FAILED = "Authentication for User '{0}' failed! " +
                                                 "Reason: {1}  ";

            TLogging.Log(String.Format(AUTHENTICATION_FAILED,
                    new String[] { AUserName, AReason }) +
                String.Format(ResourceTexts.StrRequestCallerInfo, AClientComputerName, AClientIPAddress));
        }

        /// <summary>
        /// Called by TClientManager to request the disconnection of a certain Client
        /// from the Petra Server.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="AReason"></param>
        /// <param name="ACantDisconnectReason">In case the function returns false, this
        /// contains the reason why the disconnection cannot take place.</param>
        /// <returns>true if disconnection will take place, otherwise false.
        /// </returns>
        public static bool ServerDisconnectClient(System.Int32 AClientID, String AReason, out String ACantDisconnectReason)
        {
            // TODO: perhaps allow the closing of a session, writing to s_session table?
            ACantDisconnectReason = "not implemented";
            return false;
        }

        /// <summary>
        /// Called by TClientManager to queue a ClientTask for a certain Client.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client; use -1 to queue the
        /// ClientTask to all Clients</param>
        /// <param name="ATaskGroup">Group of the Task</param>
        /// <param name="ATaskCode">Code of the Task (depending on the TaskGroup this can be
        /// left empty)</param>
        /// <param name="ATaskParameter1">Parameter #1 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter2">Parameter #2 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter3">Parameter #3 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskParameter4">Parameter #4 for the Task (depending on the TaskGroup
        /// this can be left empty)</param>
        /// <param name="ATaskPriority">Priority of the Task</param>
        /// <param name="AExceptClientID">Pass in a Server-assigned ID of the Client that should
        /// not get the ClientTask in its queue. Makes sense only if -1 is used for the
        /// AClientID parameter. Default is -1, which means no Client is excepted.</param>
        /// <returns>The ClientID for which the ClientTask was queued (the last ClientID
        /// in the list of Clients if -1 is submitted for AClientID). Error values
        /// are: -1 if AClientID is identical with AExceptClientID, -2 if the
        /// Client specified with AClientID is not in the list of Clients or
        /// AClientID &lt;&gt; -1
        /// </returns>
        public static Int32 QueueClientTask(System.Int32 AClientID,
            String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID = -1)
        {
            // Currently not implemented
            return -1;
        }

        /// <summary>
        /// add client task to queue
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1"></param>
        /// <param name="ATaskParameter2"></param>
        /// <param name="ATaskParameter3"></param>
        /// <param name="ATaskParameter4"></param>
        /// <param name="ATaskPriority"></param>
        /// <param name="AExceptClientID"></param>
        /// <returns></returns>
        public static Int32 QueueClientTask(String AUserID,
            String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            System.Int16 ATaskPriority,
            System.Int32 AExceptClientID)
        {
            // Currently not implemented
            return -1;
        }

        /// <summary>
        /// add error to log, using IErrorLog
        /// </summary>
        public static void AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            Int32 AProcessID,
            String AUserID)
        {
            if (UErrorLog != null)
            {
                UErrorLog.AddErrorLogEntry(AErrorCode,
                    AContext,
                    AMessageLine1,
                    AMessageLine2,
                    AMessageLine3,
                    AUserID,
                    AProcessID);
            }
        }

        /// <summary>
        /// Records the logging-out (=disconnection) of a Client, using ILoginLog
        /// </summary>
        /// <param name="AUserID">UserID of the User for which a logout should be recorded.</param>
        /// <param name="AProcessID">ProcessID of the User for which a logout should be recorded.
        /// This will need to be the number that got returned from an earlier call to
        /// AddLoginLogEntry(string, bool, string, bool, out int, TDBTransaction)!</param>
        public void RecordUserLogout(String AUserID, Int32 AProcessID)
        {
            if (ULoginLog != null)
            {
                ULoginLog.RecordUserLogout(AUserID,
                    AProcessID, null);
            }
        }

        /// <summary>
        /// Builds an array that contains information about the Clients that are currently
        /// connected to the Petra Server or were connected to the Petra Server at some
        /// time in the past.
        ///
        /// </summary>
        /// <param name="AListDisconnectedClients">Lists only connected Clients if false and
        /// only disconnected Clients if true.</param>
        /// <returns>A two-dimensional String Array containing the
        /// </returns>
        public static ArrayList BuildClientList(Boolean AListDisconnectedClients)
        {
            // TODO: use information from s_session table???
            ArrayList ClientList = new ArrayList();
            return ClientList;
        }

        /// <summary>
        /// Called by TServerManager to request an array that contains information about
        /// the Clients that are currently connected to the Petra Server.
        ///
        /// </summary>
        /// <param name="AListDisconnectedClients">Lists only connected Clients if false and
        /// only disconnected Clients if true.
        /// </param>
        /// <returns>void</returns>
        public static ArrayList ClientList(Boolean AListDisconnectedClients)
        {
            return BuildClientList(AListDisconnectedClients);
        }

        private static string GetUserIDFromEmail(string AUserEmail)
        {
            TDataBase db = DBAccess.Connect("GetUserIDFromEmail");
            TDBTransaction Transaction = new TDBTransaction();
            string UserID = AUserEmail;

            DBConnectionObj.ReadTransaction(ref Transaction,
                delegate
                {
                    string sql = "SELECT s_user_id_c FROM PUB_s_user WHERE UPPER(s_email_address_c) = ?";

                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("EmailAddress", OdbcType.VarChar);
                    parameters[0].Value = AUserEmail.ToUpper();

                    DataTable result = db.SelectDT(sql, "user", Transaction, parameters);

                    if (result.Rows.Count == 1)
                    {
                        UserID = result.Rows[0][0].ToString();
                    }
                    else
                    {
                        TLogging.Log("Login with E-Mail address failed for " + AUserEmail + ". " +
                            "We found " + result.Rows.Count.ToString() + " matching rows for this address.");
                        throw new Exception("multiple users are matching this email address");
                    }
                });

            db.CloseDBConnection();
            
            return UserID;
        }

        /// <summary>
        /// Called by a Client to request connection to the Petra Server.
        ///
        /// Authenticate the user and create a sesssion for the user.
        ///
        /// </summary>
        /// <param name="AUserName">Username with which the Client connects</param>
        /// <param name="APassword">Password with which the Client connects</param>
        /// <param name="AClientComputerName">Computer name of the Client</param>
        /// <param name="AClientExeVersion"></param>
        /// <param name="AClientIPAddress">IP Address of the Client</param>
        /// <param name="AClientServerConnectionType">Type of the connection (eg. LAN, Remote)</param>
        /// <param name="AClientID">Server-assigned ID of the Client</param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="ASiteKey"></param>
        /// <param name="ADataBase"></param>
        public static TConnectedClient ConnectClient(String AUserName,
            String APassword,
            String AClientComputerName,
            String AClientIPAddress,
            System.Version AClientExeVersion,
            TClientServerConnectionType AClientServerConnectionType,
            out System.Int32 AClientID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out System.Int64 ASiteKey,
            TDataBase ADataBase = null)
        {
            TDataBase DBConnectionObj = null;
            TDBTransaction ReadWriteTransaction = new TDBTransaction();
            bool SystemEnabled = true;
            string WelcomeMessage = String.Empty;
            Int64 SiteKey = -1;

            TConnectedClient ConnectedClient = null;

            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Loaded Assemblies in AppDomain " + Thread.GetDomain().FriendlyName + " (at call of ConnectClient):", TLoggingType.ToConsole |
                    TLoggingType.ToLogfile);

                foreach (Assembly tmpAssembly in Thread.GetDomain().GetAssemblies())
                {
                    TLogging.Log(tmpAssembly.FullName, TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }

            /*
             * Every Client Connection request is coming in in a separate Thread
             * (.NET Remoting does that for us and this is good!). However, the next block
             * of code must be executed only by exactly ONE thread at the same time to
             * preserve the integrity of Client tracking!
             */
            try
            {
                // TODORemoting if (Monitor.TryEnter(UConnectClientMonitor, TSrvSetting.ClientConnectionTimeoutAfterXSeconds * 1000))
                {
                    if (Thread.CurrentThread.Name == String.Empty)
                    {
                        Thread.CurrentThread.Name = "Client_" + AUserName + "__CLIENTCONNECTION_THREAD";
                    }

                    #region Logging

                    if (TLogging.DL >= 4)
                    {
                        Console.WriteLine(FormatClientList(false));
                        Console.WriteLine(FormatClientList(true));
                    }

                    if (TLogging.DL >= 4)
                    {
                        TLogging.Log("Client '" + AUserName + "' is connecting...", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                    }
                    else
                    {
                        TLogging.Log("Client '" + AUserName + "' is connecting...", TLoggingType.ToLogfile);
                    }

                    #endregion

                    // check for username, if it is an email address
                    if (AUserName.Contains('@'))
                    {
                        AUserName = GetUserIDFromEmail(AUserName);
                    }

                    #region Variable assignments
                    // we are not really using the ClientID anymore, but the session ID!
                    AClientID = (short)0;
                    string ClientName = AUserName.ToUpper() + "_" + AClientID.ToString();
                    #endregion

                    ConnectedClient = new TConnectedClient(AClientID, AUserName.ToUpper(), ClientName, AClientComputerName, AClientIPAddress,
                        AClientServerConnectionType, ClientName);

                    #region Client Version vs. Server Version check

                    if (TLogging.DL >= 9)
                    {
                        Console.WriteLine(
                            "Client EXE Program Version: " + AClientExeVersion.ToString() + "; Server EXE Program Version: " +
                            TSrvSetting.ApplicationVersion.ToString());
                    }

                    if (TSrvSetting.ApplicationVersion.Compare(new TFileVersionInfo(AClientExeVersion)) != 0)
                    {
                        ConnectedClient.SessionStatus = TSessionStatus.adsStopped;
                        #region Logging

                        if (TLogging.DL >= 4)
                        {
                            TLogging.Log(
                                "Client '" + AUserName + "' tried to connect, but its Program Version (" + AClientExeVersion.ToString() +
                                ") doesn't match! Aborting Client Connection!", TLoggingType.ToConsole | TLoggingType.ToLogfile);
                        }
                        else
                        {
                            TLogging.Log(
                                "Client '" + AUserName + "' tried to connect, but its Program Version (" + AClientExeVersion.ToString() +
                                ") doesn't match! Aborting Client Connection!", TLoggingType.ToLogfile);
                        }

                        #endregion
                        throw new EClientVersionMismatchException(String.Format(StrClientServerExeProgramVersionMismatchMessage,
                                AClientExeVersion.ToString(), TSrvSetting.ApplicationVersion.ToString()));
                    }

                    #endregion

                    #region Login request verification (incl. User authentication)
                    DBConnectionObj = DBAccess.Connect("ConnectClient (User Login)", ADataBase);
                    bool SubmitOK = false;

                    DBConnectionObj.WriteTransaction(ref ReadWriteTransaction,
                        ref SubmitOK,
                        delegate
                        {
                            // Perform login checks such as User authentication and Site Key check
                            try
                            {
                                PerformLoginChecks(AUserName,
                                    APassword,
                                    AClientComputerName,
                                    AClientIPAddress,
                                    out SystemEnabled,
                                    ReadWriteTransaction);
                            }
                            #region Exception handling
                            catch (EPetraSecurityException)
                            {
                                #region Logging

                                if (TLogging.DL >= 4)
                                {
                                    TLogging.Log(
                                        "Client '" + AUserName + "' tried to connect, but it failed the Login Checks. Aborting Client Connection!",
                                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                                }
                                else
                                {
                                    TLogging.Log(
                                        "Client '" + AUserName + "' tried to connect, but it failed the Login Checks. Aborting Client Connection!",
                                        TLoggingType.ToLogfile);
                                }

                                #endregion

                                ConnectedClient.SessionStatus = TSessionStatus.adsStopped;

                                // We need to set this flag to true here to get the failed login to be stored in the DB!!!
                                SubmitOK = true;

                                throw;
                            }
                            catch (Exception)
                            {
                                ConnectedClient.SessionStatus = TSessionStatus.adsStopped;
                                throw;
                            }
                            #endregion

                        // Login Checks were successful!
                        ConnectedClient.SessionStatus = TSessionStatus.adsConnectingLoginOK;

                        // Retrieve Welcome message and SiteKey
                        try
                        {
                            if (UMaintenanceLogonMessage != null)
                            {
                                WelcomeMessage = UMaintenanceLogonMessage.GetLogonMessage(AUserName, true, ReadWriteTransaction);
                            }
                            else
                            {
                                WelcomeMessage = "Welcome";
                            }

                            // we could do this directly, or via an interface, similar to LogonMessage, see above
                            string sql = "SELECT s_default_value_c FROM s_system_defaults WHERE s_default_code_c = 'SiteKey'";

                            try
                            {
                                SiteKey = Convert.ToInt64(DBConnectionObj.ExecuteScalar(sql, ReadWriteTransaction));
                            }
                            catch (EOPDBException)
                            {
                                // there is no site key defined yet.
                                SiteKey = -1;
                            }
                        }
                        catch (Exception)
                        {
                            ConnectedClient.SessionStatus = TSessionStatus.adsStopped;
                            throw;
                        }

                        SubmitOK = true;
                    });
                    #endregion

                    /*
                     * Uncomment the following statement to be able to better test how the
                     * Client reacts when it tries to connect and receives a
                     * ELoginFailedServerTooBusyException.
                     */

                    // Thread.Sleep(7000);

                    /*
                     * Notify all waiting Clients (that have not timed out yet) that they can
                     * now try to connect...
                     */
                    // TODORemoting Monitor.PulseAll(UConnectClientMonitor);
                }
// TODORemoting               else
                {
                    /*
                     * Throw Exception to tell any timed-out connecting Client that the Server
                     * is too busy to accept connect requests at the moment.
                     */
// TODORemoting                   throw new ELoginFailedServerTooBusyException();
                }
            }
            finally
            {
// TODORemoting               Monitor.Exit(UConnectClientMonitor);
            }

            ConnectedClient.StartSession();

            #region Logging

            //
            // Assemblies successfully loaded into Client AppDomain
            //
            if (TLogging.DL >= 4)
            {
                TLogging.Log(
                    "Client '" + AUserName + "' successfully connected. ClientID: " + AClientID.ToString(),
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
            else
            {
                TLogging.Log("Client '" + AUserName + "' successfully connected. ClientID: " + AClientID.ToString(), TLoggingType.ToLogfile);
            }

            #endregion

            ASystemEnabled = SystemEnabled;
            AWelcomeMessage = WelcomeMessage;
            ASiteKey = SiteKey;

            return ConnectedClient;
        }

        /// <summary>
        /// convert exception to error code
        /// </summary>
        public static eLoginEnum LoginErrorFromException(Exception e)
        {
            if (e is EUserNotExistantException || e is EAccessDeniedException)
            {
                return eLoginEnum.eLoginAuthenticationFailed;
            }
            else if (e is EUserRetiredException)
            {
                return eLoginEnum.eLoginUserIsRetired;
            }
            else if (e is ESystemDisabledException)
            {
                return eLoginEnum.eLoginSystemDisabled;
            }
            else if (e is ELicenseExpiredException)
            {
                return eLoginEnum.eLoginLicenseExpired;
            }
            else if (e is EClientVersionMismatchException)
            {
                return eLoginEnum.eLoginVersionMismatch;
            }
            else if (e is ELoginFailedServerTooBusyException)
            {
                return eLoginEnum.eLoginServerTooBusy;
            }
            else if (e is EDBConnectionNotEstablishedException)
            {
                return eLoginEnum.eLoginServerTooBusy;
            }

            TLogging.Log("Unspecified Error: " + e.ToString());

            return eLoginEnum.eLoginFailedForUnspecifiedError;
        }

        /// <summary>
        /// Called by a Client to request disconnection from the Petra Server.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client that should be disconnected</param>
        /// <param name="ACantDisconnectReason">In case the function returns false, this
        /// contains the reason why the disconnection cannot take place.</param>
        /// <returns>true if disconnection will take place, otherwise false.
        /// </returns>
        public static Boolean DisconnectClient(System.Int32 AClientID, out String ACantDisconnectReason)
        {
            return DisconnectClient(AClientID, CLIENTINITIATED_DISCONNECTION, out ACantDisconnectReason);
        }

        /// <summary>
        /// Called by a Client to request disconnection from the Petra Server.
        ///
        /// </summary>
        /// <param name="AClientID">Server-assigned ID of the Client that should be disconnected</param>
        /// <param name="AReason"></param>
        /// <param name="ACantDisconnectReason">In case the function returns false, this
        /// contains the reason why the disconnection cannot take place.</param>
        /// <returns>true if disconnection will take place, otherwise false.
        /// </returns>
        public static Boolean DisconnectClient(System.Int32 AClientID, String AReason, out String ACantDisconnectReason)
        {
            // TODO disconnect client by dropping the session from s_session table???
            ACantDisconnectReason = "NotImplemented";
            return false;
        }

        /// <summary>
        /// Can be called by a Client to get memory information from the
        /// GarbageCollection on the Server.
        ///
        /// @comment For debugging/memory tracking purposes only.
        ///
        /// </summary>
        /// <returns>Result of a call to GC.GetTotalMemory.
        /// </returns>
        public static System.Int32 GCGetApproxMemory()
        {
            System.Int32 ReturnValue;
            ReturnValue = (int)GC.GetTotalMemory(false);
            Console.WriteLine("TClientManager.GCGetApproxMemory: Approx. memory in use: " + ReturnValue.ToString());
            return ReturnValue;
        }

        /// <summary>
        /// Can be called by a Client to request information about the GarbageCollection
        /// Generation of a certain remoted object.
        ///
        /// @comment For debugging/memory tracking purposes only.
        ///
        /// </summary>
        /// <param name="AObject">Remoted Object</param>
        /// <returns>GC Generation (or -1 if the object no longer exists)
        /// </returns>
        public static System.Int32 GCGetGCGeneration(object AObject)
        {
            System.Int32 ReturnValue;
            try
            {
                ReturnValue = GC.GetGeneration(AObject);
            }
            catch (Exception)
            {
                ReturnValue = -1;
                Console.WriteLine("TClientManager.GetGCGeneration: Object no longer in memory!");
            }
            return ReturnValue;
        }

        /// <summary>
        /// Can be called by a Client to perform a GarbageCollection on the Server.
        ///
        /// @comment For debugging/memory tracking purposes only.
        ///
        /// </summary>
        /// <returns>Result of a call to GC.GetTotalMemory after the GC was performed.
        /// </returns>
        public static System.Int32 GCPerformGC()
        {
            GC.Collect();
            Console.WriteLine("TClientManager.PerformGC: GC performed");
            return (int)GC.GetTotalMemory(false);
        }
    }
}
