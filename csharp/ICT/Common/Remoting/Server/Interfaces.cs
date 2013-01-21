//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// an interface for the user authentication
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// add a new user
        /// </summary>
        bool AddUser(string AUserID);

        /// <summary>
        /// authenticate a user
        /// </summary>
        IPrincipal PerformUserAuthentication(string AUserName, string APassword,
            out Int32 AProcessID,
            out Boolean ASystemEnabled);
    }

    /// <summary>
    /// for saving and loading the database
    /// </summary>
    public interface IImportExportManager
    {
        /// <summary>
        /// BackupDatabaseToYmlGZ
        /// </summary>
        string BackupDatabaseToYmlGZ();

        /// <summary>
        /// RestoreDatabaseFromYmlGZ
        /// </summary>
        bool RestoreDatabaseFromYmlGZ(string AYmlGzData);
    }

    /// <summary>
    /// an interface for logging to the database
    /// </summary>
    public interface IErrorLog
    {
        /// <summary>
        /// add an error log
        /// </summary>
        Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            out TVerificationResultCollection AVerificationResult);

        /// <summary>
        /// add an error log
        /// </summary>
        Boolean AddErrorLogEntry(String AErrorCode,
            String AContext,
            String AMessageLine1,
            String AMessageLine2,
            String AMessageLine3,
            String AUserID,
            Int32 AProcessID,
            out TVerificationResultCollection AVerificationResult);
    }

    /// <summary>
    /// an interface for retrieving a welcome message from the databse
    /// </summary>
    public interface IMaintenanceLogonMessage
    {
        /// <summary>
        /// get a welcome message
        /// </summary>
        string GetLogonMessage(IPrincipal UserInfo, Boolean AReturnEnglishIfNotFound);
    }

    /// <summary>
    /// an interface for an implementation for an appdomain
    /// </summary>
    public interface IClientAppDomainConnection
    {
        /// <summary>
        /// Executes the ClientTaskAdd procedure on the TClientDomainManager Object.
        ///
        /// </summary>
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
        /// <returns>TaskID
        /// </returns>
        Int32 ClientTaskAdd(String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            Int16 ATaskPriority);

        /// <summary>
        /// get the last time something happened in this appdomain
        /// </summary>
        DateTime LastActionTime
        {
            get;
        }

        /// <summary>
        /// a string identifier of the appdomain
        /// </summary>
        string AppDomainName
        {
            get;
        }

        /// <summary>
        /// Creates a new AppDomain for a Client and remotes an instance of TRemoteLoader
        /// into it.
        /// </summary>
        /// <param name="AClientName"></param>
        IClientAppDomainConnection CreateAppDomain(string AClientName);

        /// <summary>
        /// Loads the ClientDomain DLL into the Client's AppDomain, instantiates the
        /// main Class (TClientDomainManager) and initialises the AppDomain by calling
        /// several functions of that Class.
        ///
        /// </summary>
        /// <param name="AClientID">ClientID as assigned by the ClientManager</param>
        /// <param name="AClientServerConnectionType">Tells in which way the Client connected
        /// to the PetraServer</param>
        /// <param name="AClientManagerRef">A reference to the ClientManager object
        /// (Note: .NET Remoting will be working behind the scenes since calls to
        /// this Object will cross AppDomains!)</param>
        /// <param name="ASystemDefaultsCacheRef">A reference to the SystemDefaultsCache object
        /// (Note: .NET Remoting will be working behind the scenes since calls to
        /// this Object will cross AppDomains!)</param>
        /// <param name="ACacheableTablesManagerRef"></param>
        /// <param name="AUserInfo">An instantiated PetraPrincipal Object, containing User
        /// information</param>
        /// <param name="ARemotingURLPollClientTasks">he .NET Remoting URL of the
        /// TPollClientTasks Class which the Client needs to calls to retrieve
        /// ClientTasks.</param>
        /// <returns>void</returns>
        void LoadDomainManagerAssembly(Int32 AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            object ASystemDefaultsCacheRef,
            object ACacheableTablesManagerRef,
            IPrincipal AUserInfo,
            out String ARemotingURLPollClientTasks);

        /// <summary>
        /// load the assemblies for the modules
        /// </summary>
        void LoadAssemblies(string AClientID, IPrincipal AUserInfo, ref Hashtable ARemotingURLs);

        /// <summary>
        /// close the database connection
        /// </summary>
        void CloseDBConnection();

        /// <summary>
        /// stop the appdomain of the client
        /// </summary>
        void StopClientAppDomain();

        /// <summary>
        /// Unloads a Client's AppDomain.
        /// </summary>
        void Unload();
    }

    /// an interface for system defaults cache
    public interface ISystemDefaultsCache
    {
        /// <summary>
        /// get boolean default value
        /// </summary>
        bool GetBooleanDefault(String AKey, bool ADefault);

        /// <summary>
        /// get int default
        /// </summary>
        System.Int64 GetInt64Default(String AKey);
    }
}