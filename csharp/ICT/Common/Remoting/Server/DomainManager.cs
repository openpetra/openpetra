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
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using System.Security.Principal;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Sinks.Encryption;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// collection of static functions and variables for the appdomain management
    /// </summary>
    public class DomainManagerBase
    {
        /// <summary>used internally to store the ClientID for which this AppDomain was created</summary>
        public static Int32 GClientID;

        /// <summary>used internally to hold SiteKey Information (for convenience)</summary>
        public static Int64 GSiteKey;

        /// <summary>used internally to hold a proxy reference to the ClientManager in the Server's Default AppDomain</summary>
        public static TClientManagerCallForwarder UClientManagerCallForwarderRef;

        /// <summary>tells when the last remoteable object was marshaled (remoted).</summary>
        public static DateTime ULastObjectRemotingAction;

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAdd(String ATaskGroup, String ATaskCode, Int16 ATaskPriority)
        {
            return ClientTaskAdd(ATaskGroup, ATaskCode, null, null, null, null, ATaskPriority);
        }

        /// <summary>
        /// add a task for the client
        /// </summary>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1"></param>
        /// <param name="ATaskParameter2"></param>
        /// <param name="ATaskParameter3"></param>
        /// <param name="ATaskParameter4"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAdd(String ATaskGroup,
            String ATaskCode,
            System.Object ATaskParameter1,
            System.Object ATaskParameter2,
            System.Object ATaskParameter3,
            System.Object ATaskParameter4,
            Int16 ATaskPriority)
        {
            return UClientManagerCallForwarderRef.QueueClientTaskFromClient(GClientID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                -1);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAddToOtherClient(Int16 AClientID, String ATaskGroup, String ATaskCode, Int16 ATaskPriority)
        {
            return ClientTaskAddToOtherClient(AClientID, ATaskGroup, ATaskCode, null, null, null, null, ATaskPriority);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1"></param>
        /// <param name="ATaskParameter2"></param>
        /// <param name="ATaskParameter3"></param>
        /// <param name="ATaskParameter4"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAddToOtherClient(Int16 AClientID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            Int16 ATaskPriority)
        {
            return UClientManagerCallForwarderRef.QueueClientTaskFromClient(AClientID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                GClientID);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAddToOtherClient(String AUserID, String ATaskGroup, String ATaskCode, Int16 ATaskPriority)
        {
            return ClientTaskAddToOtherClient(AUserID, ATaskGroup, ATaskCode, null, null, null, null, ATaskPriority);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserID"></param>
        /// <param name="ATaskGroup"></param>
        /// <param name="ATaskCode"></param>
        /// <param name="ATaskParameter1"></param>
        /// <param name="ATaskParameter2"></param>
        /// <param name="ATaskParameter3"></param>
        /// <param name="ATaskParameter4"></param>
        /// <param name="ATaskPriority"></param>
        /// <returns></returns>
        public static Int32 ClientTaskAddToOtherClient(String AUserID,
            String ATaskGroup,
            String ATaskCode,
            object ATaskParameter1,
            object ATaskParameter2,
            object ATaskParameter3,
            object ATaskParameter4,
            Int16 ATaskPriority)
        {
            return UClientManagerCallForwarderRef.QueueClientTaskFromClient(AUserID,
                ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority,
                GClientID);
        }
    }

    /// <summary>
    /// Sets up .NET Remoting for this AppDomain, remotes a simple object which
    /// can be called to from outside, sets up and remotes the Server-side .NET
    /// Remoting Sponsor and opens a DB connection for the AppDomain.
    ///
    /// It also monitors the Server-side .NET Remoting Sponsor and tears down the
    /// AppDomain if the Sponsor is disconnected from .NET Remoting, which only
    /// happens if the Client fails to call the Sponsor's KeepAlive method
    /// regularily (that means that either the Client crashed or the connection
    /// between the Client and the Server has broken). In either case the AppDomain
    /// and all objects that are instantiated in it are of no use anymore - the
    /// Client can only connect anew, and then a new AppDomain is created.
    ///
    /// @comment This class gets instantiated from TClientAppDomainConnection.
    ///
    /// @comment WARNING: The name of the class and the names of many functions and
    /// procedures must not be changed, because they get instantiated/invoked via
    /// .NET Reflection, that is 'late-bound'. If you need to rename them, you also
    /// need to change the Strings with their names in the .NET Reflection calls in
    /// TClientAppDomainConnection!
    ///
    /// </summary>
    public class TClientDomainManagerBase : MarshalByRefObject
    {
        /// <summary>UserID for which this AppDomain was created</summary>
        private String FUserID;

        /// <summary>ClientServer connection type</summary>
        private TClientServerConnectionType FClientServerConnectionType;

        /// <summary>holds reference to an instance of the ClientTasksManager</summary>
        private TClientTasksManager FClientTasksManager;

        private ICrossDomainService FRemotedPollClientTaskObject;

        /// <summary>Random Security Token (to prevent unauthorised AppDomain shutdown)</summary>
        private String FRandomAppDomainTearDownToken;
        private System.Object FTearDownAppDomainMonitor;

        /// <summary>Tells when the last Client Action occured (the last time when a remoteable object was marshaled (remoted)).
        /// Can be overloaded by a server with database access to see when the last DB action occured</summary>
        public virtual DateTime LastActionTime
        {
            get
            {
                return DomainManagerBase.ULastObjectRemotingAction;
            }
        }


        /// <summary>
        /// Inserts a ClientTask into this Clients Task queue.
        ///
        /// @comment WARNING: If you need to rename this function or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
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
            return FClientTasksManager.ClientTaskAdd(ATaskGroup,
                ATaskCode,
                ATaskParameter1,
                ATaskParameter2,
                ATaskParameter3,
                ATaskParameter4,
                ATaskPriority);
        }

        #region TClientDomainManager

        /// <summary>
        /// Sets up .NET Remoting Lifetime Services and TCP Channel for this AppDomain.
        ///
        /// @comment WARNING: If you need to change the parameters of the Constructor,
        /// you also need to change the parameters in the .NET Reflection call in
        /// TClientAppDomainConnection!
        ///
        /// </summary>
        /// <param name="AClientID">ClientID as assigned by the ClientManager</param>
        /// <param name="AClientServerConnectionType">Tells in which way the Client connected
        /// to the PetraServer</param>
        /// <param name="AClientManagerRef">A reference to the ClientManager object
        /// (Note: .NET Remoting will be working behind the scenes since calls to
        /// this Object will cross AppDomains!)</param>
        /// <param name="AUserID"></param>
        /// <returns>void</returns>
        public TClientDomainManagerBase(String AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            string AUserID)
        {
            new TAppSettingsManager(false);

            FUserID = AUserID;

            // Console.WriteLine('TClientDomainManager.Create in AppDomain: ' + Thread.GetDomain().FriendlyName);
            DomainManagerBase.GClientID = Convert.ToInt16(AClientID);
            DomainManagerBase.UClientManagerCallForwarderRef = AClientManagerRef;
            FClientServerConnectionType = AClientServerConnectionType;
            FClientTasksManager = new TClientTasksManager();
            FTearDownAppDomainMonitor = new System.Object();
            Random random = new Random();
            FRandomAppDomainTearDownToken = random.Next(Int32.MinValue, Int32.MaxValue).ToString();

            //
            // Set up .NET Remoting Lifetime Services and TCP Channel for this AppDomain.
            // Note: .NET Remoting needs to be set up separately for each AppDomain, and settings in
            // the .NET (Remoting) Configuration File are valid only for the Default AppDomain.
            //
            try
            {
                // The following settings equal to a config file's
                // <lifetime leaseTime="10MS" renewOnCallTime="10MS" leaseManagerPollTime = "5MS" />
                // setting.
                // Please note that this breaks remoting on mono, but these settings help on MS .NET
                // to find any remoted objects whose lifetime was inadvertedly not configured, and
                // therefore live only very short and calls to them break...
                // LifetimeServices.LeaseTime := TimeSpan.FromMilliseconds(10);
                // LifetimeServices.RenewOnCallTime := TimeSpan.FromMilliseconds(10);
                // LifetimeServices.LeaseManagerPollTime := TimeSpan.FromMilliseconds(5);
                // More sensible settings for Lifetime Services
                // TODO 1 ochristiank cRemoting : .NET Remoting LifetimeSettings should be flexible instead hardcoded in the future!
                try
                {
                    LifetimeServices.LeaseTime = TimeSpan.FromSeconds(60);
                    LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(60);
                    LifetimeServices.LeaseManagerPollTime = TimeSpan.FromSeconds(5);
                }
                catch (RemotingException)
                {
                    // ignore System.Runtime.Remoting.RemotingException : 'LeaseTime' can only be set once within an AppDomain.
                    // this happens in the Server NUnit test, when running several tests, therefore reconnecting with the same AppDomain.
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                throw;
            }

            if (TLogging.DL >= 4)
            {
                Console.WriteLine("Application domain: " + Thread.GetDomain().FriendlyName);
                Console.WriteLine("  for User: " + FUserID);
            }
        }

        /// <summary>
        /// make sure that the TClientDomainManager object exists until this AppDomain is unloaded!
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            // make sure that the TClientDomainManager object exists until this AppDomain is unloaded!
            return null;
        }

        /// <summary>
        /// stop the appdomain of the client
        /// </summary>
        public void StopClientAppDomain()
        {
            if (TLogging.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.StopClientAppDomain: calling StopClientStillAliveCheckThread...",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            ClientStillAliveCheck.TClientStillAliveCheck.StopClientStillAliveCheckThread();

            // this can get the server to a halt and prevent people to logon again. see email from Moray "Server Crash" 22/08/2007
            // if TLogging.DL >= 5 then TLogging.Log('TClientDomainManager.StopClientAppDomain: before ChannelServices.UnregisterChannel(FTcpChannel)', [ToConsole, ToLogfile]);
            // ChannelServices.UnregisterChannel(FTcpChannel);
            // if TLogging.DL >= 5 then TLogging.Log('TClientDomainManager.StopClientAppDomain: after ChannelServices.UnregisterChannel(FTcpChannel)', [ToConsole, ToLogfile]);
            DomainManagerBase.UClientManagerCallForwarderRef = null;

            if (TLogging.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.StopClientAppDomain: after UClientManagerCallForwarderRef := nil",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
        }

        /// <summary>
        /// Creates a new static instance of TSrvSetting for this AppDomain and takes
        /// over all settings from the static TSrvSetting object in the Default
        /// AppDomain.
        ///
        /// @comment WARNING: If you need to rename this procedure or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// @comment The static TSrvSetting object in the Default AppDomain is
        /// inaccessible in this AppDomain!
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitAppDomain(TSrvSetting ASettings)
        {
            // Console.WriteLine('TClientDomainManager.InitAppDomain in AppDomain: ' + Thread.GetDomain().FriendlyName);

            new TSrvSetting(ASettings);
            new TAppSettingsManager(TSrvSetting.ConfigurationFile);

            TLogging.DebugLevel = TAppSettingsManager.GetInt16("Server.DebugLevel", 0);
        }

        /// <summary>
        /// Calls a method in ClientManager to tear down the AppDomain where this Object
        /// is residing.
        ///
        /// @comment This procedure is designed to be executed through a callback from
        /// TClientStillAliveCheck.
        ///
        /// </summary>
        /// <param name="AToken">Security token (which is passed in the constructor of
        /// TClientStillAliveCheck) to prevent malicious calling of this function</param>
        /// <param name="AReason">String telling the reason why the AppDomain is being teared down
        /// </param>
        /// <returns>void</returns>
        public void TearDownAppDomain(String AToken, String AReason)
        {
            if (TSrvSetting.ClientAppDomainShutdownAfterKeepAliveTimeout)
            {
                /*
                 * Use a Monitor to prevent the following code beeing executed simultaneusly!
                 * (for some odd reason this would happen on .NET/Windows, but not on
                 * mono/Linux).
                 */
                if (Monitor.TryEnter(FTearDownAppDomainMonitor))
                {
                    Monitor.Enter(FTearDownAppDomainMonitor);

                    // prevent unauthorised tearing down of AppDomain by comparing security token
                    if (AToken == FRandomAppDomainTearDownToken)
                    {
                        if (TLogging.DL >= 9)
                        {
                            Console.WriteLine("TearDownAppDomain: Tearing down ClientDomain!!! Reason: " + AReason);
                        }

                        // The AppDomain and all the objects that are instantiated in it cease
                        // to exist after the following call!!!
                        // > No further code can be executed in the AppDomain after that!
                        DomainManagerBase.UClientManagerCallForwarderRef.DisconnectClient((short)DomainManagerBase.GClientID, AReason);
                        Monitor.Exit(FTearDownAppDomainMonitor);
                    }
                }
            }
        }

        /// <summary>
        /// Parameterises and remotes a TPollClientTasks Object that will be used by the
        /// Client to poll for ClientTasks.
        ///
        /// @comment WARNING: If you need to rename this function or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// @comment The Client needs to make calls to the TPollClientTasks Object
        /// in regular intervals. If the calls don't come anymore, the Client's
        /// AppDomain will be unloaded by a thread of TClientStillAliveCheck!
        ///
        /// </summary>
        /// <returns>The URL at which the remoted TPollClientTasks Object can be reached.
        /// </returns>
        public String GetPollClientTasksURL()
        {
            // Set Parameters for TPollClientTasks Class
            new TPollClientTasksParameters(FClientTasksManager);

            FRemotedPollClientTaskObject = new TPollClientTasks();

            // Start ClientStillAliveCheck Thread
            new ClientStillAliveCheck.TClientStillAliveCheck(FClientServerConnectionType, new TDelegateTearDownAppDomain(
                    TearDownAppDomain), FRandomAppDomainTearDownToken);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("TClientDomainManager.GetPollClientTasksURL: created TClientStillAliveCheck.");
            }

            string ReturnValue = TConfigurableMBRObject.BuildRandomURI("PollClientTasks");

            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TClientDomainManager.GetPollClientTasksURL: remote at: " + ReturnValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the object for remoting
        /// </summary>
        public ICrossDomainService GetRemotedPollClientTasksObject()
        {
            return FRemotedPollClientTaskObject;
        }

        /// <summary>
        /// Returns the current Status of a Task in the Clients Task queue.
        ///
        /// </summary>
        /// <param name="ATaskID">Task ID
        /// </param>
        /// <returns>void</returns>
        public String ClientTaskStatus(System.Int32 ATaskID)
        {
            return FClientTasksManager.ClientTaskStatus(ATaskID);
        }

        #endregion
    }
}