//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces;
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.RemotingSinks.Encryption;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.ClientDomain;

namespace Ict.Petra.Server.App.ClientDomain
{
    /// <summary>
    /// collection of static functions and variables for the appdomain management
    /// </summary>
    public class DomainManager
    {
        /// <summary>used internally to store the ClientID for which this AppDomain was created</summary>
        public static Int32 GClientID;

        /// <summary>used internally to hold a proxy reference to the SystemDefaultsCache in the Server's Default AppDomain</summary>
        public static TSystemDefaultsCache GSystemDefaultsCache;

        /// <summary>used internally to hold a proxy reference to the CacheableTablesManager in the Server's Default AppDomain</summary>
        public static TCacheableTablesManager GCacheableTablesManager;

        /// <summary>used internally to hold SiteKey Information (for convenience)</summary>
        public static Int64 GSiteKey;

        /// <summary>used internally for logging purposes</summary>
        public static TLogging ULogger;

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
            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('ClientTaskAddToOtherClient: calling UClientManagerRef.QueueClientTaskFromClient...'); $ENDIF
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
            // $IFDEF DEBUGMODE if TSrvSetting.DL >= 7 then Console.WriteLine('ClientTaskAddToOtherClient: calling UClientManagerRef.QueueClientTaskFromClient...'); $ENDIF
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
    public class TClientDomainManager : MarshalByRefObject
    {
        /// <summary>A copy of the TSrvSetting class (including property values) from the Server's Default AppDomain</summary>
        private TSrvSetting FServerSettings;

        /// <summary>Remoting proxy reference to TRemoteFactory object</summary>
        private ObjRef FRemotedObject;

        /// <summary>URL of the remoted TRemoteFactory object</summary>
        private String FRemotingURL;

        /// <summary>UserID for which this AppDomain was created</summary>
        private String FUserID;

        /// <summary>ClientServer connection type</summary>
        private TClientServerConnectionType FClientServerConnectionType;

        /// <summary>holds reference to an instance of the ClientTasksManager</summary>
        private TClientTasksManager FClientTasksManager;

        /// <summary>Random Security Token (to prevent unauthorised AppDomain shutdown)</summary>
        private String FRandomAppDomainTearDownToken;
        private System.Object FTearDownAppDomainMonitor;
        private TcpChannel FTcpChannel;

        /// <summary>Tells when the last Client Action occured (either the last time when a remoteable object was marshaled (remoted) or when the last DB action occured).</summary>
        public DateTime LastActionTime
        {
            get
            {
                DateTime ReturnValue;

#if  TESTMODE_WITHOUT_ODBC
#else
                if (DBAccess.GDBAccessObj.LastDBAction > DomainManager.ULastObjectRemotingAction)
                {
                    ReturnValue = DBAccess.GDBAccessObj.LastDBAction;
                }
                else
#endif
                {
                    ReturnValue = DomainManager.ULastObjectRemotingAction;
                }

                return ReturnValue;
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
        /// information
        /// </param>
        /// <returns>void</returns>
        public TClientDomainManager(String AClientID,
            String ARemotingPort,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            TSystemDefaultsCache ASystemDefaultsCacheRef,
            TCacheableTablesManager ACacheableTablesManagerRef,
            TPetraPrincipal AUserInfo)
        {
            System.Int16 RemotingPortInt;
            Hashtable ChannelProperties;

            // Console.WriteLine('TClientDomainManager.Create in AppDomain: ' + Thread.GetDomain().FriendlyName);
            DomainManager.GClientID = Convert.ToInt16(AClientID);
            DomainManager.GCacheableTablesManager = ACacheableTablesManagerRef;
            DomainManager.UClientManagerCallForwarderRef = AClientManagerRef;
            FUserID = AUserInfo.UserID;
            FClientServerConnectionType = AClientServerConnectionType;
            FRemotingURL = "NOT YET SET!!!"; // as default
            FClientTasksManager = new TClientTasksManager();
            FTearDownAppDomainMonitor = new System.Object();
            Random random = new Random();
            FRandomAppDomainTearDownToken = random.Next(Int32.MinValue, Int32.MaxValue).ToString();

            //
            // Set up .NET Remoting Lifetime Services and TCP Channel for this AppDomain.
            // Note: .NET Remoting needs to be set up separately for each AppDomain, and settings in
            // the .NET (Remoting) Configuration File are valid only for the Default AppDomain.
            //
            RemotingPortInt = Convert.ToInt16(ARemotingPort);
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
#if DEBUGMODE
                    LifetimeServices.LeaseTime = TimeSpan.FromSeconds(20);
                    LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(20);
                    LifetimeServices.LeaseManagerPollTime = TimeSpan.FromSeconds(1);
#else
                    LifetimeServices.LeaseTime = TimeSpan.FromSeconds(60);
                    LifetimeServices.RenewOnCallTime = TimeSpan.FromSeconds(60);
                    LifetimeServices.LeaseManagerPollTime = TimeSpan.FromSeconds(5);
#endif
                }
                catch (RemotingException)
                {
                    // ignore System.Runtime.Remoting.RemotingException : 'LeaseTime' can only be set once within an AppDomain.
                    // this happens in the Server NUnit test, when running several tests, therefore reconnecting with the same AppDomain.
                }

                BinaryServerFormatterSinkProvider TCPSink = new BinaryServerFormatterSinkProvider();
                TCPSink.TypeFilterLevel = TypeFilterLevel.Low;
                IServerChannelSinkProvider EncryptionSink = TCPSink;

                if (TAppSettingsManager.GetValueStatic("Server.ChannelEncryption.PrivateKeyfile", "", false).Length > 0)
                {
                    EncryptionSink = new EncryptionServerSinkProvider();
                    EncryptionSink.Next = TCPSink;
                }

                ChannelProperties = new Hashtable();
                ChannelProperties.Add("port", RemotingPortInt.ToString());

                string SpecificIPAddress = TAppSettingsManager.GetValueStatic("ListenOnIPAddress", "", false);

                if (SpecificIPAddress.Length > 0)
                {
                    ChannelProperties.Add("machineName", SpecificIPAddress);
                }

                FTcpChannel = new TcpChannel(ChannelProperties, null, EncryptionSink);
                ChannelServices.RegisterChannel(FTcpChannel, false);
            }
            catch (Exception)
            {
                throw;
            }
            UserInfo.GUserInfo = AUserInfo;
            DomainManager.GSystemDefaultsCache = ASystemDefaultsCacheRef;
            DomainManager.GSiteKey = DomainManager.GSystemDefaultsCache.GetInt64Default(SharedConstants.SYSDEFAULT_SITEKEY);

            if (DomainManager.GSiteKey <= 0)
            {
                // this is for connecting to legacy database format
                // we cannot add SiteKey to SystemDefaults, because Petra 2.3 would have a conflict since it adds it on startup already to the in-memory defaults, but not to the database
                // see also https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=114
                DomainManager.GSiteKey = DomainManager.GSystemDefaultsCache.GetInt64Default("SiteKeyPetra2");
            }

            if (DomainManager.GSiteKey <= 0)
            {
                // this can happen either with a legacy Petra 2.x database or with a fresh OpenPetra database without any ledger yet
                Console.WriteLine("there is no SiteKey or SiteKeyPetra2 record in s_system_defaults");
                DomainManager.GSiteKey = 99000000;
            }

#if DEBUGMODE
            if (TSrvSetting.DL >= 4)
            {
                Console.WriteLine("Application domain: " + Thread.GetDomain().FriendlyName + " @ Port " + ARemotingPort);
                Console.WriteLine("  for User: " + FUserID);
            }
#endif
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
            // TODO 1 oChristianK cLogging (Console) : Put the following debug messages again in a DEBUGMODE conditional compilation directive and raise the DL to >=9; this was removed to trace problems in on live installations!
            if (TSrvSetting.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.StopClientAppDomain: calling StopClientStillAliveCheckThread...",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            ClientStillAliveCheck.TClientStillAliveCheck.StopClientStillAliveCheckThread();

            // this can get the server to a halt and prevent people to logon again. see email from Moray "Server Crash" 22/08/2007
            // if TSrvSetting.DL >= 5 then TLogging.Log('TClientDomainManager.StopClientAppDomain: before ChannelServices.UnregisterChannel(FTcpChannel)', [ToConsole, ToLogfile]);
            // ChannelServices.UnregisterChannel(FTcpChannel);
            // if TSrvSetting.DL >= 5 then TLogging.Log('TClientDomainManager.StopClientAppDomain: after ChannelServices.UnregisterChannel(FTcpChannel)', [ToConsole, ToLogfile]);
            DomainManager.UClientManagerCallForwarderRef = null;

            if (TSrvSetting.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.StopClientAppDomain: after UClientManagerCallForwarderRef := nil",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            ChannelServices.UnregisterChannel(FTcpChannel);
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
        /// <param name="AApplicationName">ApplicationName setting</param>
        /// <param name="AApplicationVersion"></param>
        /// <param name="AConfigurationFile">ConfigurationFile setting</param>
        /// <param name="AExecutingOS">ExecutingOS setting</param>
        /// <param name="ARDMBSType">RDMBSType setting</param>
        /// <param name="AODBCDsn">ODBC Dsn setting</param>
        /// <param name="APostgreSQLServer"></param>
        /// <param name="APostgreSQLServerPort"></param>
        /// <param name="APostgreSQLDatabaseName"></param>
        /// <param name="ADBUsername">DB Username</param>
        /// <param name="ADBPassword">DB Password</param>
        /// <param name="AIPBasePort">IPBasePort setting</param>
        /// <param name="ADebugLevel">DebugLevel setting</param>
        /// <param name="AServerLogFile">complete path for log file to write logging to</param>
        /// <param name="AHostName">HostName setting</param>
        /// <param name="AHostIPAddresses">HostIPAddresses setting</param>
        /// <param name="AClientIdleStatusAfterXMinutes">ClientIdleStatusAfterXMinutes setting</param>
        /// <param name="AClientKeepAliveCheckIntervalInSeconds"></param>
        /// <param name="AClientKeepAliveTimeoutAfterXSecondsLAN">ClientKeepAliveTimeoutAfterXSecondsLAN
        /// setting</param>
        /// <param name="AClientKeepAliveTimeoutAfterXSecondsRemote">ClientKeepAliveTimeoutAfterXSecondsRemote
        /// setting</param>
        /// <param name="AClientConnectionTimeoutAfterXSeconds"></param>
        /// <param name="AClientAppDomainShutdownAfterKeepAliveTimeout">ClientAppDomainShutdownAfterKeepAliveTimeout
        /// setting</param>
        /// <param name="ASMTPServer">ASMTPServer setting</param>
        /// <param name="AAutomaticIntranetExportEnabled">AAutomaticIntranetExportEnabled setting</param>
        /// <param name="ARunAsStandalone">ARunAsStandalone setting</param>
        /// <param name="AIntranetDataDestinationEmail">AIntranetDataDestinationEmail setting</param>
        /// <param name="AIntranetDataSenderEmail">AIntranetDataSenderEmail setting</param>
        /// <returns>void</returns>
        public void TakeoverServerSettings(String AApplicationName,
            String AConfigurationFile,
            System.Version AApplicationVersion,
            TExecutingOSEnum AExecutingOS,
            TDBType ARDMBSType,
            String AODBCDsn,
            String APostgreSQLServer,
            String APostgreSQLServerPort,
            String APostgreSQLDatabaseName,
            String ADBUsername,
            String ADBPassword,
            System.Int16 AIPBasePort,
            System.Int16 ADebugLevel,
            String AServerLogFile,
            String AHostName,
            String AHostIPAddresses,
            System.Int16 AClientIdleStatusAfterXMinutes,
            System.Int16 AClientKeepAliveCheckIntervalInSeconds,
            System.Int16 AClientKeepAliveTimeoutAfterXSecondsLAN,
            System.Int16 AClientKeepAliveTimeoutAfterXSecondsRemote,
            System.Int16 AClientConnectionTimeoutAfterXSeconds,
            bool AClientAppDomainShutdownAfterKeepAliveTimeout,
            string ASMTPServer,
            bool AAutomaticIntranetExportEnabled,
            bool ARunAsStandalone,
            string AIntranetDataDestinationEmail,
            string AIntranetDataSenderEmail)
        {
            // Console.WriteLine('TClientDomainManager.TakeoverServerSettings in AppDomain: ' + Thread.GetDomain().FriendlyName);
            FServerSettings = new TSrvSetting(AApplicationName,
                AConfigurationFile,
                AApplicationVersion,
                AExecutingOS,
                ARDMBSType,
                AODBCDsn,
                APostgreSQLServer,
                APostgreSQLServerPort,
                APostgreSQLDatabaseName,
                ADBUsername,
                ADBPassword,
                AIPBasePort,
                ADebugLevel,
                AServerLogFile,
                AHostName,
                AHostIPAddresses,
                AClientIdleStatusAfterXMinutes,
                AClientKeepAliveCheckIntervalInSeconds,
                AClientKeepAliveTimeoutAfterXSecondsLAN,
                AClientKeepAliveTimeoutAfterXSecondsRemote,
                AClientConnectionTimeoutAfterXSeconds,
                AClientAppDomainShutdownAfterKeepAliveTimeout,
                ASMTPServer,
                AAutomaticIntranetExportEnabled,
                ARunAsStandalone,
                AIntranetDataDestinationEmail,
                AIntranetDataSenderEmail);
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
#if DEBUGMODE
                        if (TSrvSetting.DL >= 9)
                        {
                            Console.WriteLine("TearDownAppDomain: Tearing down ClientDomain!!! Reason: " + AReason);
                        }
#endif

                        // The AppDomain and all the objects that are instantiated in it cease
                        // to exist after the following call!!!
                        // > No further code can be executed in the AppDomain after that!
                        DomainManager.UClientManagerCallForwarderRef.DisconnectClient((short)DomainManager.GClientID, AReason);
                        Monitor.Exit(FTearDownAppDomainMonitor);
                    }
                }
            }
        }

        /// <summary>
        /// Establishes a new Database connection for this AppDomain.
        /// For every Client AppDomain a separate DB Connection needs to be opened.
        ///
        /// @comment WARNING: If you need to rename this procedure or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// @comment The global Ict.Common.DB.DBAccess.DBAccessObj object in the Default
        /// AppDomain is inaccessible in this AppDomain!
        ///
        /// </summary>
        /// <returns>void</returns>
        public void EstablishDBConnection()
        {
            DomainManager.ULogger = new TLogging(TSrvSetting.ServerLogFile);

            TLanguageCulture.Init();

#if  TESTMODE_WITHOUT_ODBC
#else
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("  Connecting to Database...");
            }
#endif
            DBAccess.GDBAccessObj = new TDataBasePetra();
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("DBAccessObj object created.");
            }
#endif
            DBAccess.GDBAccessObj.DebugLevel = TSrvSetting.DebugLevel;
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("DebugLevel set.");
            }
#endif
            ((TDataBasePetra)DBAccess.GDBAccessObj).AddErrorLogEntryCallback += new TDelegateAddErrorLogEntry(this.AddErrorLogEntry);
            try
            {
                ((TDataBasePetra)DBAccess.GDBAccessObj).EstablishDBConnection(TSrvSetting.RDMBSType,
                    TSrvSetting.PostgreSQLServer,
                    TSrvSetting.PostgreSQLServerPort,
                    TSrvSetting.PostgreSQLDatabaseName,
                    TSrvSetting.DBUsername,
                    TSrvSetting.DBPassword,
                    "",
                    UserInfo.GUserInfo.UserID);
#if DEBUGMODE
                if (TSrvSetting.DL >= 9)
                {
                    Console.WriteLine("  Connected to Database.");
                }
#endif

                // $IFDEF DEBUGMODE Console.WriteLine('SystemDefault "CalebEmail": ' + GSystemDefaultsCache.GetSystemDefault('CalebEmail'));$ENDIF
            }
            catch (Exception)
            {
                // TLogging.Log('Exception occured while establishing connection to Database Server: ' + exp.ToString);
                throw;
            }
#endif
        }

        /// <summary>
        /// Closes the Database connection for this AppDomain.
        ///
        /// @comment WARNING: If you need to rename this procedure or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            // Console.WriteLine('TClientDomainManager.CloseDBConnection in AppDomain: ' + Thread.GetDomain().FriendlyName);
#if  TESTMODE_WITHOUT_ODBC
#else
            // TODO 1 oChristianK cLogging (Console) : Put the following debug messages in a DEBUGMODE conditional compilation directive and raise the DL to >=9; these logging statements were inserted to trace problems in on live installations!
            if (TSrvSetting.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.CloseDBConnection: before calling GDBAccessObj.CloseDBConnection",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }

            try
            {
                DBAccess.GDBAccessObj.CloseDBConnection();
            }
            catch (EDBConnectionNotAvailableException)
            {
                // The DB connection was never opened  since this is no problem here, ignore this Exception.
                if (TSrvSetting.DL >= 5)
                {
                    TLogging.Log("TClientDomainManager.CloseDBConnection: Info: DB Connection was never opened, therefore no need to close it.",
                        TLoggingType.ToConsole | TLoggingType.ToLogfile);
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (TSrvSetting.DL >= 5)
            {
                TLogging.Log("TClientDomainManager.CloseDBConnection: after calling GDBAccessObj.CloseDBConnection",
                    TLoggingType.ToConsole | TLoggingType.ToLogfile);
            }
#endif
        }

        /// <summary>
        /// Creates and remotes a simple test object out of this AppDomain which can be
        /// called to from outside (also from the Client!) through .NET Remoting.
        ///
        /// @comment WARNING: If you need to rename this function or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// </summary>
        /// <returns>The URL at which the remoted test object can be reached.
        /// </returns>
        public String GetRemotedObjectRemotingURL()
        {
            // TMyRemotedObject
            TRemoteFactory RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;

            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte[] rndbytes = new Byte[4];
            Byte rndbytespos;
            String RandomString;

            // Console.WriteLine('TClientDomainManager.GetRemotedObjectRemotingURL in AppDomain: ' + Thread.GetDomain().FriendlyName);
            RandomString = "";
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rnd.GetBytes(rndbytes);

            for (rndbytespos = 0; rndbytespos <= 3; rndbytespos += 1)
            {
                RandomString = RandomString + rndbytes[rndbytespos].ToString();
            }

            RemotingTime = DateTime.Now;
            RemotedObject = new TRemoteFactory(); // TMyRemotedObject
            RemoteAtURI = FUserID + '_' + (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IRemoteFactory));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TRemoteFactory.URI: " + FRemotedObject.URI);
            }
#endif
            return FRemotingURL;
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
            String ReturnValue;
            DateTime RemotingTime;
            String RemoteAtURI;

            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte[] RandomRemotingBytes = new Byte[4];
            Byte rndbytespos;
            String RandomRemotingString;
            RandomRemotingString = "";

            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rnd.GetBytes(RandomRemotingBytes);

            for (rndbytespos = 0; rndbytespos <= 3; rndbytespos += 1)
            {
                RandomRemotingString = RandomRemotingString + RandomRemotingBytes[rndbytespos].ToString();
            }

            RemotingTime = DateTime.Now;
            RemoteAtURI = FUserID + '_' + (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomRemotingString.ToString();

            // Set Parameters for TPollClientTasks Class
            new TPollClientTasksParameters(FClientTasksManager);

            // Register TPollClientTasks Class as 'SingleCall' remoted Object
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TPollClientTasks), RemoteAtURI, WellKnownObjectMode.SingleCall);

            // Start ClientStillAliveCheck Thread
            new ClientStillAliveCheck.TClientStillAliveCheck(FClientServerConnectionType, new TDelegateTearDownAppDomain(
                    TearDownAppDomain), FRandomAppDomainTearDownToken);
#if DEBUGMODE
            if (TSrvSetting.DL >= 5)
            {
                Console.WriteLine("TClientDomainManager.GetPollClientTasksURL: created TClientStillAliveCheck.");
            }
#endif
            ReturnValue = RemoteAtURI;
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TClientDomainManager.GetPollClientTasksURL: RemoteAtURI: " + RemoteAtURI);
            }
#endif
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AErrorCode"></param>
        /// <param name="AContext"></param>
        /// <param name="AMessageLine1"></param>
        /// <param name="AMessageLine2"></param>
        /// <param name="AMessageLine3"></param>
        public void AddErrorLogEntry(String AErrorCode, String AContext, String AMessageLine1, String AMessageLine2, String AMessageLine3)
        {
            DomainManager.UClientManagerCallForwarderRef.AddErrorLogEntry(AErrorCode,
                AContext,
                AMessageLine1,
                AMessageLine2,
                AMessageLine3,
                UserInfo.GUserInfo.UserID,
                UserInfo.GUserInfo.ProcessID);
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

    /// <summary>
    /// For Experimenting with Sponsors only!
    /// Still in use by the experimental GUI Client.
    ///
    /// </summary>
    public class TRemoteFactory : MarshalByRefObject, IRemoteFactory
    {
        #region TRemoteFactory

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public IClientInstanceInterface CreateInstance()
        {
            return new TMyVanishingRemotedObject();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public IClientInstanceInterface2 CreateInstance2()
        {
            return new TMyVanishingRemotedObject2();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TRemoteFactory object exists until this AppDomain is unloaded!
        }

        #endregion
    }

    /// <summary>
    /// For Experimenting with Sponsors only!
    /// Still in use by the experimental GUI Client.
    ///
    /// </summary>
    public class TMyVanishingRemotedObject : TConfigurableMBRObject, IClientInstanceInterface
    {
        private DateTime FStartTime;

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Int32 InstanceHash
        {
            get
            {
                return this.GetHashCode();
            }
        }


        #region TMyVanishingRemotedObject

        /// <summary>
        /// todoComment
        /// </summary>
        public TMyVanishingRemotedObject()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Created without constructor. Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
        }

#if DEBUGMODE
        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        ~TMyVanishingRemotedObject()
        {
            const Int32 MaxIterations = 100000;

            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
            MyObject = new System.Object();

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject: Getting collected after " + (new TimeSpan(
                                                                                               DateTime.Now.Ticks -
                                                                                               FStartTime.Ticks)).ToString() + " seconds.");
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject: Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MaxIterations; LoopCounter += 1)
            {
                MyObject2 = new System.Object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject: Finalizer has run.");
            }
        }
#endif



        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            Console.WriteLine("TMyVanishingRemotedObject: Time requested by a client.");
            return DateTime.Now;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AHelloString"></param>
        public void Hello(out String AHelloString)
        {
            AHelloString = "HELLO from TMyVanishingRemotedObject!" + Environment.NewLine + "HashCode: " + this.GetHashCode().ToString() +
                           Environment.NewLine + "In the application domain: " + Thread.GetDomain().FriendlyName;
        }

        #endregion
    }

    /// <summary>
    /// For Experimenting with Sponsors only!
    /// Still in use by the experimental GUI Client.
    ///
    /// </summary>
    public class TMyVanishingRemotedObject2 : TConfigurableMBRObject, IClientInstanceInterface2
    {
        private DateTime FStartTime;

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Int32 InstanceHash
        {
            get
            {
                return this.GetHashCode();
            }
        }


        #region TMyVanishingRemotedObject2

        /// <summary>
        /// todoComment
        /// </summary>
        public TMyVanishingRemotedObject2() : base()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Created without constructor. Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
        }

#if DEBUGMODE
        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        ~TMyVanishingRemotedObject2()
        {
            const Int32 MaxIterations = 100000;

            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
            MyObject = new object();

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject2: Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject2: Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MaxIterations; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyVanishingRemotedObject2: Finalizer has run.");
            }
        }
#endif



        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            Console.WriteLine("TMyVanishingRemotedObject2: Time requested by a client.");
            return DateTime.Now;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AHelloString"></param>
        public void Hello2(out String AHelloString)
        {
            AHelloString = "HELLO from TMyVanishingRemotedObject2!" + Environment.NewLine + "HashCode: " + this.GetHashCode().ToString() +
                           Environment.NewLine + "In the application domain: " + Thread.GetDomain().FriendlyName;
        }

        #endregion
    }

    /// <summary>
    /// For Experimenting with Sponsors only!
    ///
    /// Left over from early tests and is no longer in use, still kept in here for
    /// historic and learning purposes.
    ///
    /// </summary>
    public class TMyRemotedObject : MarshalByRefObject, IClientInstanceInterface
    {
        private DateTime FStartTime;

        /// <summary>
        /// todoComment
        /// </summary>
        public System.Int32 InstanceHash
        {
            get
            {
                return this.GetHashCode();
            }
        }


        #region TMyRemotedObject

        /// <summary>
        /// todoComment
        /// </summary>
        public TMyRemotedObject() : base()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " Created without constructor. Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
        }

#if DEBUGMODE
        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        ~TMyRemotedObject()
        {
            const Int32 MaxIterations = 100000;

            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
            MyObject = new System.Object();

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyRemotedObject: Getting collected after " + (new TimeSpan(
                                                                                      DateTime.Now.Ticks -
                                                                                      FStartTime.Ticks)).ToString() + " seconds.");
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyRemotedObject: Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MaxIterations; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMyRemotedObject: Finalizer has run.");
            }
        }
#endif



        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            // var
            // baseLeaseLifetimeService: ILease;

            /*
             * baseLeaseLifetimeService := ILease(inherited InitializeLifetimeService());
             *
             * if baseLeaseLifetimeService.CurrentState = LeaseState.Initial then
             * begin
             * baseLeaseLifetimeService.InitialLeaseTime := Timespan.FromSeconds(25);
             * baseLeaseLifetimeService.RenewOnCallTime := Timespan.FromSeconds(15);
             * end;
             *
             * Result := baseLeaseLifetimeService;
             */
            return null;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            Console.WriteLine("TMyRemotedObject: Time requested by a client.");
            return DateTime.Now;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AHelloString"></param>
        public void Hello(out String AHelloString)
        {
            AHelloString = "HELLO!" + Environment.NewLine + "HashCode: " + this.GetHashCode().ToString() + Environment.NewLine +
                           "In the application domain: " + Thread.GetDomain().FriendlyName;
        }

        #endregion
    }
}