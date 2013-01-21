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
using Ict.Petra.Client.App.Core;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MCommon;
using Ict.Testing.NUnitTools;

namespace Ict.Testing.NUnitPetraClient
{
/// This is a dll for NUnit test programs.
/// The dll can be used by calling TPetraConnector.Connect and TPetraConnector.Disconnect
/// from TFixture.SetUp and TFixture.TearDown
///
/// required parameters (in the config file or on the command line):
/// AutoLogin
/// AutoLoginPasswd
    public class TPetraConnector
    {
        /// connect to the server
        public static void Connect(string AConfigName)
        {
            TUnhandledThreadExceptionHandler UnhandledThreadExceptionHandler;

            // Set up Handlers for 'UnhandledException'
            // Note: BOTH handlers are needed for a WinForms Application!!!
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandling.UnhandledExceptionHandler);
            UnhandledThreadExceptionHandler = new TUnhandledThreadExceptionHandler();

            Application.ThreadException += new ThreadExceptionEventHandler(UnhandledThreadExceptionHandler.OnThreadException);

            new TAppSettingsManager(AConfigName);

            CommonNUnitFunctions.InitRootPath();

            Catalog.Init();
            TClientTasksQueue.ClientTasksInstanceType = typeof(TClientTaskInstance);
            TConnectionManagementBase.ConnectorType = typeof(TConnector);
            TConnectionManagementBase.GConnectionManagement = new TConnectionManagement();

            new TClientSettings();
            TClientInfo.InitializeUnit();
            TCacheableTablesManager.InitializeUnit();

            // Set up Data Validation Delegates
            TSharedValidationHelper.SharedGetDataDelegate = @TServerLookup.TMCommon.GetData;
            TSharedPartnerValidationHelper.VerifyPartnerDelegate = @TServerLookup.TMPartner.VerifyPartner;
            TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TServerLookup.TMFinance.GetCurrentPostingRangeDates;
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TServerLookup.TMFinance.GetCurrentPeriodDates;

            Connect(TAppSettingsManager.GetValue("AutoLogin"), TAppSettingsManager.GetValue("AutoLoginPasswd"),
                TAppSettingsManager.GetInt64("SiteKey"));
        }

        private static void Connect(String AUserName, String APassword, Int64 ASiteKey)
        {
            bool ConnectionResult;
            String WelcomeMessage;
            String LoginError;
            Int32 ProcessID;
            Boolean SystemEnabled;

            TLogging.Log("connecting UserId: " + AUserName + " to Server...");
            try
            {
                ConnectionResult = ((TConnectionManagement)TConnectionManagement.GConnectionManagement).ConnectToServer(
                    AUserName.ToUpper(), APassword,
                    out ProcessID,
                    out WelcomeMessage,
                    out SystemEnabled,
                    out LoginError);

                if (!ConnectionResult)
                {
                    TLogging.Log("Connection to PetraServer failed! ConnectionResult: " + ConnectionResult + " Error: " + LoginError);
                    return;
                }
            }
            catch (EServerConnectionServerNotReachableException)
            {
                throw;
            }
            catch (ELoginFailedServerTooBusyException)
            {
                TLogging.Log("Login failed because server was too busy.");
                throw;
            }
            catch (EDBConnectionNotEstablishedException exp)
            {
                if (exp.Message.IndexOf("Exceeding permissible number of connections") != -1)
                {
                    throw new Exception("Login failed because too many users are logged in.");
                }
                else
                {
                    throw;
                }
            }
            catch (EServerConnectionGeneralException)
            {
                throw;
            }
            TUserDefaults.InitUserDefaults();
            new TServerInfo(Utilities.DetermineExecutingOS());
            TLogging.Log(
                "client is connected ClientID: " + TConnectionManagement.GConnectionManagement.ClientID.ToString() + " UserId: " + AUserName +
                " to Server...");
        }

        /// disconnect from the server
        public static void Disconnect()
        {
            String CantDisconnectReason;

            try
            {
                if (!TConnectionManagement.GConnectionManagement.DisconnectFromServer(out CantDisconnectReason))
                {
                    throw new Exception("Error on Client Disconnection: " + CantDisconnectReason);
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                /* Don't show a warning here since we are normally about to close the application... */
                return;
            }
            catch (System.Runtime.Remoting.RemotingException)
            {
                /* Don't show a warning here since we are normally about to close the application... */
                return;
            }
        }
    }
}