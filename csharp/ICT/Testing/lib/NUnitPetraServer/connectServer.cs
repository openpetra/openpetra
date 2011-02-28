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
using System.Data;
using System.Configuration;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Main;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Shared.Interfaces.ServerAdminInterface;
using Ict.Petra.Server.App.Core;

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
        /// Initialize the Petra server and connect to the database
        /// </summary>
        /// <param name="AConfigName">just provide the server config file, plus AutoLogin and AutoLoginPasswd</param>
        public static void Connect(string AConfigName)
        {
            TAppSettingsManager Config = new TAppSettingsManager(AConfigName);

            Catalog.Init();
            TServerManager ServerManager = new TServerManager();

            DBAccess.GDBAccessObj = new TDataBase();
            DBAccess.GDBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                TSrvSetting.PostgreSQLServer, TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername, TSrvSetting.DBPassword, "");

            bool SystemEnabled;
            int ProcessID;
            TPetraPrincipal UserInfo = TClientManager.PerformLoginChecks(Config.GetValue("AutoLogin").ToUpper(),
                Config.GetValue("AutoLoginPasswd"),
                "NUNITTEST", "127.0.0.1", out ProcessID, out SystemEnabled);

            if (FDomain != null)
            {
                FDomain.StopClientAppDomain();
            }

            // do the same as in Ict.Petra.Server.App.Main.TRemoteLoader.LoadDomainManagerAssembly
            FDomain = new TClientDomainManager("0",
                "-1",
                TClientServerConnectionType.csctLocal,
                DomainManager.UClientManagerCallForwarderRef,
                TClientManager.SystemDefaultsCache,
                TClientManager.UCacheableTablesManager,
                UserInfo);
            FDomain.TakeoverServerSettings(TSrvSetting.ApplicationName,
                TSrvSetting.ConfigurationFile,
                TSrvSetting.ApplicationVersion,
                TSrvSetting.ExecutingOS,
                TSrvSetting.RDMBSType,
                TSrvSetting.ODBCDsn,
                TSrvSetting.PostgreSQLServer, TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername, TSrvSetting.DBPassword,
                TSrvSetting.BaseIPAddress,
                TSrvSetting.DebugLevel, TSrvSetting.ServerLogFile, TSrvSetting.HostName, TSrvSetting.HostIPAddresses,
                TSrvSetting.ClientIdleStatusAfterXMinutes, TSrvSetting.ClientKeepAliveCheckIntervalInSeconds,
                TSrvSetting.ClientKeepAliveTimeoutAfterXSecondsLAN,
                TSrvSetting.ClientKeepAliveTimeoutAfterXSecondsRemote,
                TSrvSetting.ClientConnectionTimeoutAfterXSeconds,
                TSrvSetting.ClientAppDomainShutdownAfterKeepAliveTimeout,
                TSrvSetting.SMTPServer,
                TSrvSetting.AutomaticIntranetExportEnabled,
                TSrvSetting.RunAsStandalone,
                TSrvSetting.IntranetDataDestinationEmail,
                TSrvSetting.IntranetDataSenderEmail);

            // we don't need to establish the database connection anymore
            // FDomain.EstablishDBConnection();
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