//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Testing.NUnitTools;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.CallForwarding;

namespace Ict.Testing.NUnitPetraServer
{
    /// This is a dll for NUnit test programs.
    /// The dll can be used by calling TPetraServerConnector.Connect and TPetraServerConnector.Disconnect
    /// from TFixture.SetUp and TFixture.TearDown
    ///
    /// required parameters (in the config file or on the command line):
    /// AutoLogin
    /// AutoLoginPasswd
    public class TPetraServerConnector
    {
        private static TClientDomainManager FDomain = null;

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
            TServerManager ServerManager = new TServerManager();

            DBAccess.GDBAccessObj = new TDataBase();
            DBAccess.GDBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                TSrvSetting.PostgreSQLServer, TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername, TSrvSetting.DBPassword, "");

            bool SystemEnabled;
            int ProcessID;
            TPetraPrincipal UserInfo = (TPetraPrincipal)TClientManager.PerformLoginChecks(TAppSettingsManager.GetValue("AutoLogin").ToUpper(),
                TAppSettingsManager.GetValue("AutoLoginPasswd"),
                "NUNITTEST", "127.0.0.1", out ProcessID, out SystemEnabled);

            if (FDomain != null)
            {
                FDomain.StopClientAppDomain();
            }

            TClientManager ClientManager = new TClientManager();
            DomainManager.UClientManagerCallForwarderRef = new TClientManagerCallForwarder(ClientManager);

            // do the same as in Ict.Petra.Server.App.Main.TRemoteLoader.LoadDomainManagerAssembly
            FDomain = new TClientDomainManager("0",
                TClientServerConnectionType.csctLocal,
                DomainManager.UClientManagerCallForwarderRef,
                new TSystemDefaultsCache(),
                new TCacheableTablesManager(null),
                UserInfo);
            FDomain.InitAppDomain(TSrvSetting.ServerSettings);

            new TCallForwarding();

            // we don't need to establish the database connection anymore
            // FDomain.EstablishDBConnection();

            return ServerManager;
        }

        /// <summary>
        /// shutdown the server
        /// </summary>
        public static void Disconnect()
        {
            FDomain.CloseDBConnection();
            FDomain.StopClientAppDomain();
            FDomain = null;
        }
    }
}