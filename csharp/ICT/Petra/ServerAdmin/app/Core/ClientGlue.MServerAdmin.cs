//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2004-2018 by OM International
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
using System.Threading;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;

namespace Ict.Petra.ServerAdmin.App.Core.RemoteObjects
{
    /// the top level namespace for the module ServerAdmin
    public class TMServerAdminNamespace
    {
        private TServerAdminWebConnectorsNamespace FServerAdminWebConnectorsNamespace = new TServerAdminWebConnectorsNamespace();

        /// <summary>The 'WebConnectors' subnamespace contains further subnamespaces.</summary>
        public TServerAdminWebConnectorsNamespace WebConnectors
        {
            get
            {
                return FServerAdminWebConnectorsNamespace;
            }
        }
        /// <summary> namespace definition </summary>
        public class TServerAdminWebConnectorsNamespace
        {
            /// forward the method call
            public System.Boolean LoginServerAdmin()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AServerAdminSecurityToken", THttpConnector.ServerAdminSecurityToken);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.LoginServerAdmin", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public System.Int32 GetClientsConnectedTotal()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetClientsConnectedTotal", ActualParameters, "System.Int32");
                return (System.Int32) Result[0];
            }
            /// forward the method call
            public System.Int32 GetClientsConnected()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetClientsConnected", ActualParameters, "System.Int32");
                return (System.Int32) Result[0];
            }
            /// forward the method call
            public System.Int64 GetSiteKey()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetSiteKey", ActualParameters, "System.Int64");
                return (System.Int64) Result[0];
            }
            /// forward the method call
            public System.Int64 GetDBReconnectionAttemptsCounter()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetDBReconnectionAttemptsCounter", ActualParameters, "System.Int64");
                return (System.Int64) Result[0];
            }
            /// forward the method call
            public System.Int64 GetDBConnectionCheckInterval()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetDBConnectionCheckInterval", ActualParameters, "System.Int64");
                return (System.Int64) Result[0];
            }
            /// forward the method call
            public ArrayList GetClientList()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetClientList", ActualParameters, "binary");
                return (ArrayList) Result[0];
            }
            /// forward the method call
            public System.String GetServerInfoVersion()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetServerInfoVersion", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
            /// forward the method call
            public System.String GetServerInfoState()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetServerInfoState", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
            /// forward the method call
            public System.Int64 GetServerInfoMemory()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetServerInfoMemory", ActualParameters, "System.Int64");
                return (System.Int64) Result[0];
            }
            /// forward the method call
            public System.Boolean StopServerControlled(System.Boolean AForceAutomaticClosing)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AForceAutomaticClosing", AForceAutomaticClosing);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.StopServerControlled", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public void StopServer()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.StopServer", ActualParameters, "void");
            }
            /// forward the method call
            public System.Int64 PerformGC()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.PerformGC", ActualParameters, "System.Int64");
                return (System.Int64) Result[0];
            }
            /// forward the method call
            public System.String FormatClientList(Boolean AListDisconnectedClients)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AListDisconnectedClients", AListDisconnectedClients);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.FormatClientList", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
            /// forward the method call
            public System.String FormatClientListSysadm(Boolean AListDisconnectedClients)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AListDisconnectedClients", AListDisconnectedClients);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.FormatClientListSysadm", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
            /// forward the method call
            public System.Boolean QueueClientTask(System.Int16 AClientID,
                                                           String ATaskGroup,
                                                           String ATaskCode,
                                                           System.Object ATaskParameter1,
                                                           System.Object ATaskParameter2,
                                                           System.Object ATaskParameter3,
                                                           System.Object ATaskParameter4,
                                                           System.Int16 ATaskPriority)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AClientID", AClientID);
                ActualParameters.Add("ATaskGroup", ATaskGroup);
                ActualParameters.Add("ATaskCode", ATaskCode);
                ActualParameters.Add("ATaskParameter1", ATaskParameter1);
                ActualParameters.Add("ATaskParameter2", ATaskParameter2);
                ActualParameters.Add("ATaskParameter3", ATaskParameter3);
                ActualParameters.Add("ATaskParameter4", ATaskParameter4);
                ActualParameters.Add("ATaskPriority", ATaskPriority);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.QueueClientTask", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public System.Boolean DisconnectClient(System.Int16 AClientID,
                                                            out String ACantDisconnectReason)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AClientID", AClientID);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.DisconnectClient", ActualParameters, "list");
                ACantDisconnectReason = (String) Result[0];
                return (System.Boolean) Result[1];
            }
            /// forward the method call
            public System.Boolean UpgradeDatabase()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.UpgradeDatabase", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public System.String BackupDatabaseToYmlGZ()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.BackupDatabaseToYmlGZ", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
            /// forward the method call
            public System.Boolean RestoreDatabaseFromYmlGZ(System.String AYmlGzData)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AYmlGzData", AYmlGzData);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.RestoreDatabaseFromYmlGZ", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public void RefreshAllCachedTables()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.RefreshAllCachedTables", ActualParameters, "void");
            }
            /// forward the method call
            public System.Int32 ClearConnectionPoolAndGetNumberOfDBConnections()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.ClearConnectionPoolAndGetNumberOfDBConnections", ActualParameters, "System.Int32");
                return (System.Int32) Result[0];
            }
            /// forward the method call
            public System.Boolean AddUser(System.String AUserID)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AUserID", AUserID);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.AddUser", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public System.Boolean AddUser(System.String AUserID,
                                                   System.String APassword)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AUserID", AUserID);
                ActualParameters.Add("APassword", APassword);
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.AddUser2", ActualParameters, "System.Boolean");
                return (System.Boolean) Result[0];
            }
            /// forward the method call
            public System.Int32 ListGpgKeys(out System.String List)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.ListGpgKeys", ActualParameters, "list");
                List = (System.String) Result[0];
                return (System.Int32) Result[1];
            }
            /// forward the method call
            public System.Int32 ImportGpgKeys(out System.String List)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.ImportGpgKeys", ActualParameters, "list");
                List = (System.String) Result[0];
                return (System.Int32) Result[1];
            }
            /// forward the method call
            public void PerformTimedProcessingNow(System.String AProcessName)
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                ActualParameters.Add("AProcessName", AProcessName);
                THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.PerformTimedProcessingNow", ActualParameters, "void");
            }
            /// forward the method call
            public System.String GetSmtpHost()
            {
                SortedList<string, object> ActualParameters = new SortedList<string, object>();
                List<object> Result = THttpConnector.CallWebConnector("MServerAdmin", "TServerAdminWebConnector.GetSmtpHost", ActualParameters, "System.String");
                return (System.String) Result[0];
            }
        }
    }
}
