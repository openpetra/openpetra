//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Threading;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Session;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Delegates;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Testing.NUnitTools;

namespace Ict.Testing.NUnitPetraServer
{
    /// This is a dll for NUnit test programs.
    /// The dll can be used by calling TPetraServerConnector.Connect and TPetraServerConnector.Disconnect
    /// from TFixture.SetUp and TFixture.TearDown
    ///
    /// required parameters (in the config file or on the command line):
    /// AutoLogin
    /// AutoLoginPasswd
    public static class TPetraServerConnector
    {
        /// <summary>
        /// Initialize the Petra server and connect to the database.
        /// this overload looks for the config file itself
        /// </summary>
        public static TServerManager Connect()
        {
            CommonNUnitFunctions.InitRootPath();

            string strNameConfig = CommonNUnitFunctions.rootPath + "etc/TestServer.config";

            return Connect(strNameConfig);
        }

        /// <summary>
        /// Initialize the Petra server and connect to the database
        /// </summary>
        /// <param name="AConfigName">just provide the server config file, plus AutoLogin and AutoLoginPasswd</param>
        public static TServerManager Connect(string AConfigName)
        {
            TDBTransaction LoginTransaction;
            bool CommitLoginTransaction = false;
            bool SystemEnabled;
            string WelcomeMessage;
            Int32 ClientID;
            Int64 SiteKey;

            if (File.Exists(AConfigName))
            {
                new TAppSettingsManager(AConfigName);
            }
            else if (AConfigName.Length > 0)
            {
                TLogging.Log("cannot find config file " + Path.GetFullPath(AConfigName));
                Environment.Exit(-1);
            }
            else
            {
                new TAppSettingsManager();
            }

            new TLogging(TAppSettingsManager.GetValue("Server.LogFile"));

            TSession.InitThread();

            CommonNUnitFunctions.InitRootPath();

            Catalog.Init();
            TServerManager.TheServerManager = new TServerManager();

            ErrorCodeInventory.Init();

            // initialise the cached tables and the delegates
            TSetupDelegates.Init();

            TDataBase db = DBAccess.Connect(
                "Ict.Testing.NUnitPetraServer.TPetraServerConnector.Connect DB Connection");

            // we need a serializable transaction, to store the session
            LoginTransaction = db.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                TClientManager.PerformLoginChecks(TAppSettingsManager.GetValue("AutoLogin").ToUpper(),
                    TAppSettingsManager.GetValue("AutoLoginPasswd"),
                    "NUNITTEST", "127.0.0.1", out SystemEnabled, LoginTransaction);

                CommitLoginTransaction = true;
            }
            catch (EPetraSecurityException)
            {
                // We need to set this flag to true here to get the failed login to be stored in the DB!!!
                CommitLoginTransaction = true;
            }
            finally
            {
                if (CommitLoginTransaction)
                {
                    LoginTransaction.Commit();
                }
                else
                {
                    LoginTransaction.Rollback();
                }
            }

            TConnectedClient CurrentClient = TClientManager.ConnectClient(
                TAppSettingsManager.GetValue("AutoLogin").ToUpper(),
                TAppSettingsManager.GetValue("AutoLoginPasswd"),
                "NUNITTEST", "127.0.0.1",
                TFileVersionInfo.GetApplicationVersion().ToVersion(),
                TClientServerConnectionType.csctLocal,
                out ClientID,
                out WelcomeMessage,
                out SystemEnabled,
                out SiteKey,
                db);

            // the following values are stored in the session object
            DomainManager.GClientID = ClientID;
            DomainManager.CurrentClient = CurrentClient;
            DomainManager.GSiteKey = SiteKey;

            TSetupDelegates.Init();

            db.CloseDBConnection();

            return (TServerManager)TServerManager.TheServerManager;
        }

        /// <summary>
        /// shutdown the server
        /// </summary>
        public static void Disconnect()
        {
            DomainManager.CurrentClient.EndSession();
        }
    }
}
