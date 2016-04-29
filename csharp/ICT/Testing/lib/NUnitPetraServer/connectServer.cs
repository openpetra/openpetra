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
using System.Data;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Testing.NUnitTools;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Delegates;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;

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
            if (File.Exists(AConfigName))
            {
                new TAppSettingsManager(AConfigName);
            }
            else
            {
                new TAppSettingsManager();
            }

            new TLogging(TAppSettingsManager.GetValue("Server.LogFile"));

            CommonNUnitFunctions.InitRootPath();

            Catalog.Init();
            TServerManager.TheServerManager = new TServerManager();

            DBAccess.GDBAccessObj = new TDataBase();
            DBAccess.GDBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                TSrvSetting.PostgreSQLServer, TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername, TSrvSetting.DBPassword, "", "Ict.Testing.NUnitPetraServer.TPetraServerConnector.Connect DB Connection");

            bool SystemEnabled;
            string WelcomeMessage;
            IPrincipal ThisUserInfo;
            Int32 ClientID;

            TConnectedClient CurrentClient = TClientManager.ConnectClient(
                TAppSettingsManager.GetValue("AutoLogin").ToUpper(),
                TAppSettingsManager.GetValue("AutoLoginPasswd"),
                "NUNITTEST", "127.0.0.1",
                TFileVersionInfo.GetApplicationVersion().ToVersion(),
                TClientServerConnectionType.csctLocal,
                out ClientID,
                out WelcomeMessage,
                out SystemEnabled,
                out ThisUserInfo);

            // the following values are stored in the session object
            DomainManager.GClientID = ClientID;
            DomainManager.CurrentClient = CurrentClient;
            UserInfo.GUserInfo = (TPetraPrincipal)ThisUserInfo;

            TSetupDelegates.Init();
            TSystemDefaultsCache.GSystemDefaultsCache = new TSystemDefaultsCache();
            DomainManager.GetSiteKeyFromSystemDefaultsCacheDelegate = 
                @TSystemDefaultsCache.GSystemDefaultsCache.GetSiteKeyDefault;

            TUserDefaults.InitializeUnit();

            StringHelper.CurrencyFormatTable = DBAccess.GDBAccessObj.SelectDT("SELECT * FROM PUB_a_currency", "a_currency", null);

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