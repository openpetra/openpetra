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
using System.Security.Principal;
using System.Collections;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace Ict.Common.Remoting.Server
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
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AClientDomainDllName"></param>
        /// <param name="AClientDomainClassname"></param>
        public TRemoteLoader(string AClientDomainDllName, string AClientDomainClassname)
        {
            CLIENTDOMAIN_DLLNAME = AClientDomainDllName;
            CLIENTDOMAIN_CLASSNAME = AClientDomainClassname;
        }

        /// <summary>
        /// to be set during server startup
        /// </summary>
        public static string CLIENTDOMAIN_DLLNAME = string.Empty;

        /// <summary>
        /// to be set during server startup
        /// </summary>
        public static string CLIENTDOMAIN_CLASSNAME = string.Empty;

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
            return Convert.ToInt32(
                FRemoteClientDomainManagerClass.InvokeMember("ClientTaskAdd",
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                    null,
                    FRemoteClientDomainManagerObject,
                    new Object[] { ATaskGroup, ATaskCode, ATaskParameter1, ATaskParameter2, ATaskParameter3, ATaskParameter4,
                                   (System.Object)ATaskPriority },
                    null));
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
        /// <param name="ARemotedPollClientTasksObject"></param>
        public void LoadDomainManagerAssembly(Int32 AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            object ASystemDefaultsCacheRef,
            object ACacheableTablesManagerRef,
            IPrincipal AUserInfo,
            TSrvSetting AServerSettings,
            out String ARemotingURLPollClientTasks,
            out ICrossDomainService ARemotedPollClientTasksObject)
        {
            // Console.WriteLine('TRemoteLoader.LoadDomainManagerAssembly in AppDomain: ' + AppDomain.CurrentDomain.ToString);
            Assembly LoadedAssembly = Assembly.Load(CLIENTDOMAIN_DLLNAME);

            FRemoteClientDomainManagerClass = LoadedAssembly.GetType(CLIENTDOMAIN_CLASSNAME);

            FRemoteClientDomainManagerObject = Activator.CreateInstance(FRemoteClientDomainManagerClass,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance), null,
                new Object[] { AClientID.ToString(),
                               AClientServerConnectionType,
                               AClientManagerRef,
                               ASystemDefaultsCacheRef,
                               ACacheableTablesManagerRef, AUserInfo },
                null);

            FRemoteClientDomainManagerClass.InvokeMember("InitAppDomain",
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                FRemoteClientDomainManagerObject,
                new Object[] { AServerSettings });

            // Create and remote the TPollClientTasks Class
            ARemotingURLPollClientTasks =
                Convert.ToString(FRemoteClientDomainManagerClass.InvokeMember("GetPollClientTasksURL",
                        (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null));

            ARemotedPollClientTasksObject =
                (ICrossDomainService)FRemoteClientDomainManagerClass.InvokeMember("GetRemotedPollClientTasksObject",
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null);

            // Establish (separate) DataBase connection for the AppDomain
            Convert.ToString(FRemoteClientDomainManagerClass.InvokeMember("EstablishDBConnection",
                    (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null));

            //  LoadClientDomainManager.ClientTaskAdd('USERMESSAGE','This is just for testing purposes!', 'blabla_1', 'blabla_2', 'blabla_3', 'blabla_4', 1);
        }

        /// <summary>
        /// Loads a specified Petra Module Instantiator DLL into a Client's AppDomain,
        /// initialises the main Class (an Instantiator) and remotes the resulting
        /// Instantiator Object.
        ///
        /// </summary>
        /// <param name="AAssemblyDLLName">name of the dll that contains ARemoteType</param>
        /// <param name="ARemoteType">name of the class that should be loaded</param>
        /// <param name="APetraModuleInstantiatorRemotingURL">The .NET Remoting URL which the
        /// Client needs to make calls to the Instantiator Object.
        /// </param>
        /// <param name="ARemoteObject">the remote object</param>
        /// <returns>void</returns>
        public void LoadPetraModuleAssembly(
            string AAssemblyDLLName,
            String ARemoteType,
            out String APetraModuleInstantiatorRemotingURL,
            out ICrossDomainService ARemoteObject)
        {
            // Console.WriteLine('TRemoteLoader.LoadPetraModuleAssembly in AppDomain: ' + AppDomain.CurrentDomain.ToString);
            #region Load Petra Module DLL into AppDomain of Client, create instance of Instantiator Object

            Assembly LoadedAssembly = Assembly.Load(AAssemblyDLLName);

            Type RemoteClass = LoadedAssembly.GetType(ARemoteType);

            if (RemoteClass == null)
            {
                string msg = "cannot find type " + ARemoteType + " in " + AAssemblyDLLName;
                TLogging.Log(msg);
                throw new Exception(msg);
            }

            object Instantiator = Activator.CreateInstance(RemoteClass,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null,
                null,
                null);

            #endregion

            // Remote the Petra Module Instantiator from the AppDomain
            APetraModuleInstantiatorRemotingURL =
                Convert.ToString(RemoteClass.InvokeMember("GetRemotingURL", (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                        null, Instantiator, null, null));

            ARemoteObject = (ICrossDomainService)
                            RemoteClass.InvokeMember("GetRemotedObject", (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod),
                null, Instantiator, null, null);
        }

        /// <summary>
        /// Loads the CallForwinding DLL into the Client's AppDomain, instantiates the
        /// main Class (TCallForwarding) and by that sets up Delegates that allow arbitrary code
        /// to be called in various server-side methods.
        /// </summary>
        /// <returns>void</returns>
        public void LoadCallForwardingAssembly()
        {
            const string CALLFORWARDING_DLLNAME = "Ict.Petra.Server.lib.CallForwarding";
            const string CALLFORWARDING_CLASSNAME = "Ict.Petra.Server.CallForwarding.TCallForwarding";
            Type CallforwardingClass;

//Console.WriteLine("TRemoteLoader.LoadCallForwardingAssembly in AppDomain: " + AppDomain.CurrentDomain.ToString());

//Console.WriteLine("Trying to load " + CALLFORWARDING_DLLNAME + ".dll into Client''s AppDomain...");
            Assembly LoadedAssembly = Assembly.Load(CALLFORWARDING_DLLNAME);

//Console.WriteLine("Successfully loaded " + CALLFORWARDING_DLLNAME + ".dll into Client's AppDomain.");
            CallforwardingClass = LoadedAssembly.GetType(CALLFORWARDING_CLASSNAME);

//Console.WriteLine("Creating Instance of " + CALLFORWARDING_CLASSNAME + " in Client''s AppDomain...");
            Activator.CreateInstance(CallforwardingClass,
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance), null,
                new Object[] { }, null);

//Console.WriteLine("Successfully created an instance of " + CALLFORWARDING_CLASSNAME + " in Client''s AppDomain.");
        }

        /// <summary>
        /// stop the appdomain of the client
        /// </summary>
        public void StopClientAppDomain()
        {
            // Stop Client's AppDomain
            FRemoteClientDomainManagerClass.InvokeMember("StopClientAppDomain",
                (BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod), null, FRemoteClientDomainManagerObject, null, null);
        }

        /// <summary>
        /// Executes the CloseDBConnection procedure on the TClientDomainManager Object.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            // Close Database connection of the Client's AppDomain
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
    public class TClientAppDomainConnectionBase : IClientAppDomainConnection
    {
        /// <summary>
        /// set to typeof(TClientAppDomainConnection)
        /// </summary>
        public static Type ClientAppDomainConnectionType = typeof(TClientAppDomainConnectionBase);

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

            if (TLogging.DL >= 10)
            {
                Console.WriteLine("Creating new AppDomain for Client '" + AClientName + "'...");
            }

            TClientAppDomainConnectionBase NewAppDomainConnection = (TClientAppDomainConnectionBase)Activator.CreateInstance(
                ClientAppDomainConnectionType);
            NewAppDomainConnection.FAppDomain = AppDomain.CreateDomain(LoadInAppDomainName, null, Setup);

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

            if (TLogging.DL >= 10)
            {
                TLogging.Log("AppDomain sucessfully created.", TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            if (TLogging.DL >= 10)
            {
                TLogging.Log("Trying to create an instance of TRemoteLoader in Client's AppDomain...",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            NewAppDomainConnection.FRemoteLoader =
                (TRemoteLoader)(NewAppDomainConnection.FAppDomain.CreateInstanceFromAndUnwrap("Ict.Common.Remoting.Server.dll",
                                    "Ict.Common.Remoting.Server.TRemoteLoader",
                                    false,
                                    BindingFlags.CreateInstance,
                                    null,
                                    new object[] { TRemoteLoader.CLIENTDOMAIN_DLLNAME, TRemoteLoader.CLIENTDOMAIN_CLASSNAME },
                                    Thread.CurrentThread.CurrentCulture,
                                    null));

            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Successfully created an instance of TRemoteLoader in Client's AppDomain '" + NewAppDomainConnection.FAppDomain.FriendlyName,
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            return NewAppDomainConnection;
        }

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
        /// <param name="ARemotingURLPollClientTasks">the .NET Remoting URL of the
        /// TPollClientTasks Class which the Client needs to calls to retrieve
        /// ClientTasks.</param>
        /// <returns>void</returns>
        public void LoadDomainManagerAssembly(Int32 AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            object ASystemDefaultsCacheRef,
            object ACacheableTablesManagerRef,
            IPrincipal AUserInfo,
            out String ARemotingURLPollClientTasks)
        {
            ICrossDomainService RemoteObject;

            FRemoteLoader.LoadDomainManagerAssembly(AClientID,
                AClientServerConnectionType,
                AClientManagerRef,
                ASystemDefaultsCacheRef,
                ACacheableTablesManagerRef,
                AUserInfo,
                TSrvSetting.ServerSettings,
                out ARemotingURLPollClientTasks,
                out RemoteObject);

            // register the remote url at the CrossDomainMarshaller
            TCrossDomainMarshaller.AddService(AClientID.ToString(), ARemotingURLPollClientTasks, RemoteObject);

            // Load the CallForwinding DLL into the Client's AppDomain
            FRemoteLoader.LoadCallForwardingAssembly();

            // IMPORTANT: If the following code is uncommented, the ClientDomain DLL that is loaded only in the Client's AppDomain might get loaded into the Default AppDomain  that's what we don't want!!!
            // Use this therefore only to find out what DLL's are loaded in the Client's AppDomain!!!!!!
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
        }

        /// Load Petra Module DLLs into Clients AppDomain, initialise them and remote an Instantiator Object
        public virtual void LoadAssemblies(string AClientID, IPrincipal AUserInfo, ref Hashtable ARemotingURLs)
        {
        }

        /// <summary>
        /// Loads a specified Petra Module Instantiator DLL into a Client's AppDomain,
        /// initialises the main Class (an Instantiator) and remotes the resulting
        /// Instantiator Object.
        ///
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="AAssemblyDLLName">name of the dll that contains ARemoteType</param>
        /// <param name="ARemoteType">name of the class that should be loaded</param>
        /// <param name="APetraModuleInstantiatorRemotingURL">The .NET Remoting URL which the
        /// Client needs to make calls to the Instantiator Object.
        /// </param>
        public void LoadPetraModuleAssembly(String AClientID,
            String AAssemblyDLLName,
            string ARemoteType,
            out String APetraModuleInstantiatorRemotingURL)
        {
            ICrossDomainService RemoteObject;

            FRemoteLoader.LoadPetraModuleAssembly(
                AAssemblyDLLName,
                ARemoteType,
                out APetraModuleInstantiatorRemotingURL,
                out RemoteObject);

            // register the remote url at the CrossDomainMarshaller
            TCrossDomainMarshaller.AddService(AClientID, APetraModuleInstantiatorRemotingURL, RemoteObject);

            if (TLogging.DL >= 10)
            {
                TLogging.Log(
                    "Loaded Assemblies in AppDomain " + AppDomain.CurrentDomain.FriendlyName +
                    ": (after instantiation of " + ARemoteType + ")",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);

                foreach (Assembly tmpAssembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    TLogging.Log(tmpAssembly.FullName, TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }
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