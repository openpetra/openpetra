//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2011 by OM International
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
using System.Security.Principal;
using System.Collections;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Acts as a .NET Remoting Proxy that allows loading of DLL's and instantiation
    /// of Objects in a Client's AppDomain. This is needed to prevent .NET from
    /// loading Assemblies (in which these Objects are defined) into the Default
    /// AppDomain (those Assemblies got loaded into the Client's AppDomain).
    ///
    /// @comment Gets instantiated through TClientAppDomainConnection in the Default
    /// AppDomain and is then remoted by it into the Client's AppDomain to perform
    /// the desired actions there.
    ///
    /// @comment All functions and procedures must be virtual to allow them to be
    /// executed in the Client's AppDomain (due to a limitation of mono)!!!
    /// Explanation for this: mono can't cope with method calls into different
    /// AppDomains if these methods are not marked virtual (see answer of Lluis
    /// Sanchez for the filed bug #76752 in mono's bugzilla). Apparently,
    /// C# code automatically marks such methods virtual when it is JITted, but
    /// Delphi.NET code doesn't do this.
    ///
    /// </summary>
    public class TRemoteLoader : MarshalByRefObject
    {
        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String CLIENTDOMAIN_DLLNAME = "Ict.Petra.Server.app.Core";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String CLIENTDOMAIN_CLASSNAME = "Ict.Petra.Server.App.Core.TClientDomainManager";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MCOMMON_DLLNAME = "Ict.Petra.Server.lib.MCommon";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MCOMMON_CLASSNAME = "Ict.Petra.Server.MCommon.Instantiator.TMCommonNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MCONFERENCE_DLLNAME = "Ict.Petra.Server.lib.MConference";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MCONFERENCE_CLASSNAME = "Ict.Petra.Server.MConference.Instantiator.TMConferenceNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MSYSMAN_DLLNAME = "Ict.Petra.Server.lib.MSysMan";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MSYSMAN_CLASSNAME = "Ict.Petra.Server.MSysMan.Instantiator.TMSysManNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MPARTNER_DLLNAME = "Ict.Petra.Server.lib.MPartner.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MPARTNER_CLASSNAME = "Ict.Petra.Server.MPartner.Instantiator.TMPartnerNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MPERSONNEL_DLLNAME = "Ict.Petra.Server.lib.MPersonnel";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MPERSONNEL_CLASSNAME = "Ict.Petra.Server.MPersonnel.Instantiator.TMPersonnelNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MFINANCE_DLLNAME = "Ict.Petra.Server.lib.MFinance.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MFINANCE_CLASSNAME = "Ict.Petra.Server.MFinance.Instantiator.TMFinanceNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MREPORTING_DLLNAME = "Ict.Petra.Server.lib.MReporting.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MREPORTING_CLASSNAME = "Ict.Petra.Server.MReporting.Instantiator.TMReportingNamespaceLoader";


        /// <summary>Holds a reference to the TClientDomainManager Class</summary>
        private System.Type FRemoteClientDomainManagerClass;

        /// <summary>Holds a reference to an instance of the TClientDomainManager Object</summary>
        private object FRemoteClientDomainManagerObject;

        /// <summary>Returns the LastActionTime property value from TClientDomainManager</summary>
        public DateTime LastActionTime
        {
            get
            {
                return Convert.ToDateTime(
                    FRemoteClientDomainManagerClass.InvokeMember("LastActionTime",
                        (BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty),
                        null,
                        FRemoteClientDomainManagerObject,
                        null, null));
            }
        }


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
        public Int32 ClientTaskAdd(String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            Int16 ATaskPriority)
        {
            // $IFDEF DEBUGMODE Console.WriteLine('Invoking Member ''ClientTaskAdd'' in AppDomain: ' + AppDomain.CurrentDomain.ToString); $ENDIF
            return Convert.ToInt32(
                FRemoteClientDomainManagerClass.InvokeMember("ClientTaskAdd",
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                    null,
                    FRemoteClientDomainManagerObject,
                    new Object[] { ATaskGroup, ATaskCode, ATaskParameter1, ATaskParameter2, ATaskParameter3, ATaskParameter4,
                                   (System.Object)ATaskPriority },
                    null));

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''ClientTaskAdd'' in Client''s AppDomain! Returned value: ' + Result.ToString); $ENDIF
        }

        /// <summary>
        /// needed for remoting
        ///
        /// </summary>
        public override object InitializeLifetimeService()
        {
            // make sure that the TRemoteLoader object exists until this AppDomain is unloaded!
            return null;
        }

        /// <summary>
        /// Loads the ClientDomain DLL into the Client's AppDomain, instantiates the
        /// main Class (TClientDomainManager) and initialises the AppDomain by calling
        /// several functions of that Class.
        ///
        /// </summary>
        /// <param name="AClientID">ClientID as assigned by the ClientManager</param>
        /// <param name="ARemotingPort">IP Port on which the .NET Remoting TCP Channel should
        /// be set up</param>
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
        /// <param name="AServerSettings">A copy of the ServerSettings</param>
        /// <param name="ARemotingURLPollClientTasks">The .NET Remoting URL of the
        /// TPollClientTasks Class which the Client needs to calls to retrieve
        /// ClientTasks.</param>
        /// <returns>void</returns>
        public void LoadDomainManagerAssembly(Int32 AClientID,
            Int16 ARemotingPort,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            object ASystemDefaultsCacheRef,
            object ACacheableTablesManagerRef,
            IPrincipal AUserInfo,
            TSrvSetting AServerSettings,
            out String ARemotingURLPollClientTasks)
        {
            // Console.WriteLine('TRemoteLoader.LoadDomainManagerAssembly in AppDomain: ' + AppDomain.CurrentDomain.ToString);
            #region Load ClientDomain DLL into AppDomain of Client, create instance of main Object

            // $IFDEF DEBUGMODE Console.WriteLine('Trying to load ' + CLIENTDOMAIN_DLLNAME + '.dll into Client''s AppDomain...'); $ENDIF
            Assembly LoadedAssembly = Assembly.Load(CLIENTDOMAIN_DLLNAME);

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully loaded ' + CLIENTDOMAIN_DLLNAME + '.dll into Client''s AppDomain.'); $ENDIF
            FRemoteClientDomainManagerClass = LoadedAssembly.GetType(CLIENTDOMAIN_CLASSNAME);

            // $IFDEF DEBUGMODE
            // Console.WriteLine('Loaded Assemblies in AppDomain ' + Thread.GetDomain.FriendlyName + ' (after DLL loading into new AppDomain):');
            // foreach Assembly tmpAssembly in Thread.GetDomain.GetAssemblies() do
            // begin
            // Console.WriteLine(tmpAssembly.FullName);
            // end;
            // $ENDIF
            // $IFDEF DEBUGMODE Console.WriteLine('Creating Instance of ' + CLIENTDOMAIN_CLASSNAME + ' in Client''s AppDomain...'); $ENDIF
            FRemoteClientDomainManagerObject = Activator.CreateInstance(FRemoteClientDomainManagerClass,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance), null,
                new Object[] { AClientID.ToString(),
                               ARemotingPort.ToString(),
                               AClientServerConnectionType,
                               AClientManagerRef,
                               ASystemDefaultsCacheRef,
                               ACacheableTablesManagerRef, AUserInfo },
                null);

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully created an instance of ' + CLIENTDOMAIN_CLASSNAME + ' in Client''s AppDomain.'); $ENDIF
            #endregion
            #region Initialise AppDomain for Client

            // $IFDEF DEBUGMODE Console.WriteLine('Invoking Member ''InitAppDomain'' in Client''s AppDomain...'); $ENDIF
            FRemoteClientDomainManagerClass.InvokeMember("InitAppDomain",
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                FRemoteClientDomainManagerObject,
                new Object[] { AServerSettings });

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''InitAppDomain'' in Client''s AppDomain...'); $ENDIF
            // Create and remote the TPollClientTasks Class
            // $IFDEF DEBUGMODE Console.WriteLine('Invoking Member ''GetPollClientTasksURL'' in Client''s AppDomain...'); $ENDIF
            ARemotingURLPollClientTasks =
                Convert.ToString(FRemoteClientDomainManagerClass.InvokeMember("GetPollClientTasksURL",
                        (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null));

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''GetPollClientTasksURL'' in Client''s AppDomain! Returned value: ' + ARemotingURLPollClientTasks); $ENDIF
            // Establish (separate) DataBase connection for the AppDomain
            // $IFDEF DEBUGMODE Console.WriteLine('Invoking Member ''EstablishDBConnection'' in Client''s AppDomain...'); $ENDIF
            Convert.ToString(FRemoteClientDomainManagerClass.InvokeMember("EstablishDBConnection",
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null));

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''EstablishDBConnection'' in Client''s AppDomain!'); $ENDIF
            // /    LoadClientDomainManager.ClientTaskAdd('USERMESSAGE','This is just for testing purposes!', 'blabla_1', 'blabla_2', 'blabla_3', 'blabla_4', 1);
            #endregion
        }

        /// <summary>
        /// Loads a specified Petra Module Instantiator DLL into a Client's AppDomain,
        /// initialises the main Class (an Instantiator) and remotes the resulting
        /// Instantiator Object.
        ///
        /// </summary>
        /// <param name="APetraModule">The Petra Module for which the Instantiator should be
        /// loaded. Needs to be the value of a REMOTINGURL_IDENTIFIER_M... constant.</param>
        /// <param name="APetraModuleInstantiatorRemotingURL">The .NET Remoting URL which the
        /// Client needs to make calls to the Instantiator Object.
        /// </param>
        /// <returns>void</returns>
        public void LoadPetraModuleAssembly(String APetraModule, out String APetraModuleInstantiatorRemotingURL)
        {
            String AssemblyDLLName = "";
            String RemoteType = "";
            Assembly LoadedAssembly;

            // tmpAssembly: Assembly;
            System.Type RemoteClass;
            object RemoteObject;

            // Console.WriteLine('TRemoteLoader.LoadPetraModuleAssembly in AppDomain: ' + AppDomain.CurrentDomain.ToString);
            if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN)
            {
                AssemblyDLLName = MSYSMAN_DLLNAME;
                RemoteType = MSYSMAN_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON)
            {
                AssemblyDLLName = MCOMMON_DLLNAME;
                RemoteType = MCOMMON_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE)
            {
                AssemblyDLLName = MCONFERENCE_DLLNAME;
                RemoteType = MCONFERENCE_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER)
            {
                AssemblyDLLName = MPARTNER_DLLNAME;
                RemoteType = MPARTNER_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING)
            {
                AssemblyDLLName = MREPORTING_DLLNAME;
                RemoteType = MREPORTING_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL)
            {
                AssemblyDLLName = MPERSONNEL_DLLNAME;
                RemoteType = MPERSONNEL_CLASSNAME;
            }
            else if (APetraModule == SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE)
            {
                AssemblyDLLName = MFINANCE_DLLNAME;
                RemoteType = MFINANCE_CLASSNAME;
            }

            #region Load Petra Module DLL into AppDomain of Client, create instance of Instantiator Object

//			#if DEBUGMODE
//			Console.WriteLine("Trying to load " + AssemblyDLLName + ".dll into Client's AppDomain...");
//			#endif

            LoadedAssembly = Assembly.Load(AssemblyDLLName);

//			#if DEBUGMODE
//			Console.WriteLine("Successfully loaded " + AssemblyDLLName + ".dll into Client's AppDomain.");
//			#endif
            RemoteClass = LoadedAssembly.GetType(RemoteType);

            // $IFDEF DEBUGMODE
            // Console.WriteLine('Loaded Assemblies in AppDomain ' + Thread.GetDomain.FriendlyName + ' (after DLL loading into Client''s AppDomain):');
            // for tmpAssembly in Thread.GetDomain.GetAssemblies() do
            // begin
            // Console.WriteLine(tmpAssembly.FullName);
            // end;
            // $ENDIF

//			#if DEBUGMODE
//			Console.WriteLine("Creating Instance of " + RemoteType + " in Client's AppDomain...");
//			#endif

            RemoteObject = Activator.CreateInstance(RemoteClass,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                null,
                null);

//			#if DEBUGMODE
//			Console.WriteLine("Successfully created an instance of " + RemoteType + " in Client's  AppDomain.");
//			#endif
            #endregion

            // Remote the Petra Module Instantiator from the AppDomain

//			#if DEBUGMODE
//			Console.WriteLine("Invoking Member 'GetRemotingURL' in Client's AppDomain...");
//			#endif
            APetraModuleInstantiatorRemotingURL =
                Convert.ToString(RemoteClass.InvokeMember("GetRemotingURL", (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                        null, RemoteObject, null, null));

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''GetRemotingURL'' in Client''s AppDomain! Returned value: ' + APetraModuleInstantiatorRemotingURL); $ENDIF
        }

        /// <summary>
        /// stop the appdomain of the client
        /// </summary>
        public void StopClientAppDomain()
        {
            // Stop Client's AppDomain
            // $IFDEF DEBUGMODE Console.WriteLine('Invoking Member ''StopClientAppDomain'' in AppDomain: ' + AppDomain.CurrentDomain.ToString); $ENDIF
            FRemoteClientDomainManagerClass.InvokeMember("StopClientAppDomain",
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null);

            // $IFDEF DEBUGMODE Console.WriteLine('Successfully invoked Member ''StopClientAppDomain'' in the Client''s AppDomain!'); $ENDIF
        }

        /// <summary>
        /// Executes the CloseDBConnection procedure on the TClientDomainManager Object.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            // Close Database connection of the Client's AppDomain
            // TODO 1 oChristianK cLogging (Console) : Put the following debug messages in a DEBUGMODE conditional compilation directive; these logging statements were inserted to trace problems in on live installations!
            if (TLogging.DL >= 5)
            {
                TLogging.Log("TRemoteLoader.CloseDBConnection: Invoking Member 'CloseDBConnection' in AppDomain: " + AppDomain.CurrentDomain.ToString());
            }

            FRemoteClientDomainManagerClass.InvokeMember("CloseDBConnection",
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null);

            if (TLogging.DL >= 5)
            {
                TLogging.Log("TRemoteLoader.CloseDBConnection: Successfully invoked Member 'CloseDBConnection' in the Client's AppDomain!");
            }
        }
    }

    /// <summary>
    /// Allows creation of and connection to a Client's AppDomain without causing
    /// the Assemblies which are loaded in the Client's AppDomain to be loaded into
    /// the Default AppDomain.
    ///
    /// @comment This class is used by TClientManager to create AppDomains for Clients
    /// and to communicate with them, using a TRemoteLoader object.
    ///
    /// </summary>
    public class TClientAppDomainConnection : IClientAppDomainConnection
    {
        /// <summary>Holds a reference to the Client's AppDomain</summary>
        private AppDomain FAppDomain;

        /// <summary>Holds a reference to the instance of TRemoteLoader for this Client's AppDomain</summary>
        private TRemoteLoader FRemoteLoader;

        /// <summary>Name of the Client's AppDomain</summary>
        public String AppDomainName
        {
            get
            {
                return FAppDomain.FriendlyName;
            }
        }

        /// <summary>Returns the LastActionTime property value from TClientDomainManager</summary>
        public DateTime LastActionTime
        {
            get
            {
                return FRemoteLoader.LastActionTime;
            }
        }


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
        public Int32 ClientTaskAdd(String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            Int16 ATaskPriority)
        {
            return FRemoteLoader.ClientTaskAdd(ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority);
        }

        /// <summary>
        /// Creates a new AppDomain for a Client and remotes an instance of TRemoteLoader
        /// into it.
        ///
        /// </summary>
        /// <returns>void</returns>
        public IClientAppDomainConnection CreateAppDomain(String AClientName)
        {
            // Set ApplicationBase to the application directory
            AppDomainSetup Setup = new AppDomainSetup();

            Setup.ApplicationBase = "file:///" + TAppSettingsManager.ApplicationDirectory;
            String LoadInAppDomainName = AClientName + "_Domain";

#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                Console.WriteLine("Creating new AppDomain for Client '" + AClientName + "'...");
            }
#endif


            TClientAppDomainConnection NewAppDomainConnection = new TClientAppDomainConnection();
            NewAppDomainConnection.FAppDomain = AppDomain.CreateDomain(LoadInAppDomainName, null, Setup);

#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Loaded Assemblies in AppDomain " + Thread.GetDomain().FriendlyName + " (just after AppDomain creation):",
                    TLoggingType.ToConsole |
                    TLoggingType.ToLogfile);

                foreach (Assembly tmpAssembly in Thread.GetDomain().GetAssemblies())
                {
                    TLogging.Log(tmpAssembly.FullName, TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }
#endif
#if DEBUGMODE
            //
            // IMPORTANT: If the following code is uncommented, the ClientDomain DLL that is loaded only in the Client's AppDomain might get loaded into the Default AppDomain  that's what we don't want!!!
            // Use this therefore only to find out what DLL's are loaded in the Client's AppDomain!!!!!!
            //
            // if TLogging.DL >= 10 then
            // begin
            // TLogging.Log('Loaded Assemblies in AppDomain ' + FAppDomain.FriendlyName + ': (just after AppDomain creation)', [TLoggingType.ToConsole, TLoggingType.ToLogfile]);
            // for tmpAssembly in FAppDomain.GetAssemblies() do
            // begin
            // TLogging.Log(tmpAssembly.FullName, [TLoggingType.ToConsole, TLoggingType.ToLogfile]);
            // end;
            // end;
            // Console.ReadLine;
#endif
#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                TLogging.Log("AppDomain sucessfully created.", TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
#endif
#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                TLogging.Log("Trying to create an instance of TRemoteLoader in Client's AppDomain...",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
#endif
            NewAppDomainConnection.FRemoteLoader =
                (TRemoteLoader)(NewAppDomainConnection.FAppDomain.CreateInstanceFromAndUnwrap("Ict.Petra.Server.app.Core.dll",
                                    "Ict.Petra.Server.App.Core.TRemoteLoader"));
#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                TLogging.Log("Successfully created an instance of TRemoteLoader in Client's AppDomain '" + FAppDomain.FriendlyName,
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
#endif

            return NewAppDomainConnection;
        }

        /// <summary>
        /// Loads the ClientDomain DLL into the Client's AppDomain, instantiates the
        /// main Class (TClientDomainManager) and initialises the AppDomain by calling
        /// several functions of that Class.
        ///
        /// </summary>
        /// <param name="AClientID">ClientID as assigned by the ClientManager</param>
        /// <param name="ARemotingPort">IP Port on which the .NET Remoting TCP Channel should
        /// be set up</param>
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
        public void LoadDomainManagerAssembly(Int32 AClientID,
            Int16 ARemotingPort,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            object ASystemDefaultsCacheRef,
            object ACacheableTablesManagerRef,
            IPrincipal AUserInfo,
            out String ARemotingURLPollClientTasks)
        {
            FRemoteLoader.LoadDomainManagerAssembly(AClientID,
                ARemotingPort,
                AClientServerConnectionType,
                AClientManagerRef,
                ASystemDefaultsCacheRef,
                ACacheableTablesManagerRef,
                AUserInfo,
                TSrvSetting.ServerSettings,
                out ARemotingURLPollClientTasks);
#if DEBUGMODE
            //
            // IMPORTANT: If the following code is uncommented, the ClientDomain DLL that is loaded only in the Client's AppDomain might get loaded into the Default AppDomain  that's what we don't want!!!
            // Use this therefore only to find out what DLL's are loaded in the Client's AppDomain!!!!!!
            //
            // if TLogging.DL >= 10 then
            // begin
            // TLogging.Log('Loaded Assemblies in AppDomain ' + FAppDomain.FriendlyName + ': (after instantiation of TClientDomainManager)', [TLoggingType.ToConsole, TLoggingType.ToLogfile]);
            // for tmpAssembly in FAppDomain.GetAssemblies() do
            // begin
            // TLogging.Log(tmpAssembly.FullName, [TLoggingType.ToConsole, TLoggingType.ToLogfile]);
            // end;
            // end;
            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Loaded Assemblies in AppDomain " + AppDomain.CurrentDomain.FriendlyName + ": (after instantiation of TClientDomainManager)",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);

                foreach (Assembly tmpAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    TLogging.Log(tmpAssembly.FullName, TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }
            // Console.ReadLine;
#endif
        }

        /// Load Petra Module DLLs into Clients AppDomain, initialise them and remote an Instantiator Object
        public void LoadAssemblies(IPrincipal AUserInfo, ref Hashtable ARemotingURLs)
        {
            String RemotingURL_MCommon;
            String RemotingURL_MConference;
            String RemotingURL_MSysMan;
            String RemotingURL_MPartner;
            String RemotingURL_MPersonnel;
            String RemotingURL_MFinance;
            String RemotingURL_MReporting;

            TPetraPrincipal UserInfo = (TPetraPrincipal)AUserInfo;

            // Load SYSMAN Module assembly (always loaded)
            LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN, out RemotingURL_MSysMan);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN, RemotingURL_MSysMan);
#if DEBUGMODE
            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMSysMan instantiated. Remoting URL: " + RemotingURL_MSysMan);
            }
#endif

            // Load COMMON Module assembly (always loaded)
            LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON, out RemotingURL_MCommon);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON, RemotingURL_MCommon);
#if DEBUGMODE
            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMCommon instantiated. Remoting URL: " + RemotingURL_MCommon);
            }
#endif

            // Load CONFERENCE Module assembly (always loaded)
            LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE, out RemotingURL_MConference);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE, RemotingURL_MConference);
#if DEBUGMODE
            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMConference instantiated. Remoting URL: " + RemotingURL_MConference);
            }
#endif

            // Load PARTNER Module assembly (always loaded)
            LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER, out RemotingURL_MPartner);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER, RemotingURL_MPartner);
#if DEBUGMODE
            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMPartner instantiated. Remoting URL: " + RemotingURL_MPartner);
            }
#endif

            // Load REPORTING Module assembly (always loaded)
            LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING, out RemotingURL_MReporting);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING, RemotingURL_MReporting);
#if DEBUGMODE
            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMReporting instantiated. Remoting URL: " + RemotingURL_MReporting);
            }
#endif

            // Load PERSONNEL Module assembly (loaded only for users that have personnel privileges)
            if (UserInfo.IsInModule(SharedConstants.PETRAMODULE_PERSONNEL))
            {
                LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL, out RemotingURL_MPersonnel);
                ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL, RemotingURL_MPersonnel);
#if DEBUGMODE
                if (TLogging.DL >= 5)
                {
                    Console.WriteLine("  TMPersonnel instantiated. Remoting URL: " + RemotingURL_MPersonnel);
                }
#endif
            }

            // Load FINANCE Module assembly (loaded only for users that have finance privileges)
            if ((UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)) || (UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE2))
                || (UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3)))
            {
                LoadPetraModuleAssembly(SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE, out RemotingURL_MFinance);
                ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE, RemotingURL_MFinance);
#if DEBUGMODE
                if (TLogging.DL >= 5)
                {
                    Console.WriteLine("  TMFinance instantiated. Remoting URL: " + RemotingURL_MFinance);
                }
#endif
            }
        }

        /// <summary>
        /// Loads a specified Petra Module Instantiator DLL into a Client's AppDomain,
        /// initialises the main Class (an Instantiator) and remotes the resulting
        /// Instantiator Object.
        ///
        /// </summary>
        /// <param name="APetraModule">The Petra Module for which the Instantiator should be
        /// loaded. Needs to be the value of a REMOTINGURL_IDENTIFIER_M... constant.</param>
        /// <param name="APetraModuleInstantiatorRemotingURL">The .NET Remoting URL which the
        /// Client needs to make calls to the Instantiator Object.
        /// </param>
        /// <returns>void</returns>
        public void LoadPetraModuleAssembly(String APetraModule, out String APetraModuleInstantiatorRemotingURL)
        {
            FRemoteLoader.LoadPetraModuleAssembly(APetraModule, out APetraModuleInstantiatorRemotingURL);
#if DEBUGMODE
            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Loaded Assemblies in AppDomain " + AppDomain.CurrentDomain.FriendlyName + ": (after instantiation of " + APetraModule +
                    " Module Instantiator assembly)",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);

                foreach (Assembly tmpAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    TLogging.Log(tmpAssembly.FullName, TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }
            // Console.ReadLine;
#endif
        }

        /// <summary>
        /// stop the appdomain of the client
        /// </summary>
        public void StopClientAppDomain()
        {
            FRemoteLoader.StopClientAppDomain();
        }

        /// <summary>
        /// Executes the CloseDBConnection procedure on the TClientDomainManager Object.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            // TODO 1 oChristianK cLogging (Console) : Put the following debug messages in a DEBUGMODE conditional compilation directive and raise the DL to >=9; these logging statements were inserted to trace problems in on live installations!
            if (TLogging.DL >= 5)
            {
                TLogging.Log("TClientAppDomainConnection.CloseDBConnection: before calling FRemoteLoader.CloseDBConnection",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            FRemoteLoader.CloseDBConnection();

            if (TLogging.DL >= 5)
            {
                TLogging.Log("TClientAppDomainConnection.CloseDBConnection: after calling FRemoteLoader.CloseDBConnection",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
        }

        /// <summary>
        /// Unloads a Client's AppDomain.
        ///
        /// This sends a Thread.Abort message to all Threads that are running in the
        /// Client's AppDomains context, destroys all objects that were ever instantiated
        /// in the Client's AppDomain (executing Finalizers where existing) and releases
        /// all memory allocated to the Client.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Unload()
        {
            AppDomain.Unload(this.FAppDomain);
            this.FAppDomain = null;
        }
    }
}