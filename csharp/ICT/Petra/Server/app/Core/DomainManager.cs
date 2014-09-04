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
using System.Data;
using System.Security.Principal;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Exceptions;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// collection of static functions and variables for the appdomain management
    /// </summary>
    public class DomainManager : DomainManagerBase
    {
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
    public class TClientDomainManager : TClientDomainManagerBase
    {
        /// <summary>Tells when the last Client Action occured (either the last time when a remoteable object was marshaled (remoted) or when the last DB action occured).</summary>
        public override DateTime LastActionTime
        {
            get
            {
                if (DBAccess.GDBAccessObj.LastDBAction > base.LastActionTime)
                {
                    return DBAccess.GDBAccessObj.LastDBAction;
                }
                else
                {
                    return base.LastActionTime;
                }
            }
        }

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
        /// <param name="ASystemDefaultsCacheRef">A reference to the SystemDefaultsCache object
        /// (Note: .NET Remoting will be working behind the scenes since calls to
        /// this Object will cross AppDomains!)</param>
        /// <param name="ACacheableTablesManagerRef"></param>
        /// <param name="AUserInfo">An instantiated PetraPrincipal Object, containing User
        /// information
        /// </param>
        public TClientDomainManager(String AClientID,
            TClientServerConnectionType AClientServerConnectionType,
            TClientManagerCallForwarder AClientManagerRef,
            TSystemDefaultsCache ASystemDefaultsCacheRef,
            TCacheableTablesManager ACacheableTablesManagerRef,
            IPrincipal AUserInfo) :
            base(AClientID,
                AClientServerConnectionType, AClientManagerRef,
                ((TPetraPrincipal)AUserInfo).UserID)
        {
            TCacheableTablesManager.GCacheableTablesManager = ACacheableTablesManagerRef;
            TSystemDefaultsCache.GSystemDefaultsCache = ASystemDefaultsCacheRef;

            TLanguageCulture.Init();

            UserInfo.GUserInfo = (TPetraPrincipal)AUserInfo;
            DomainManager.GSiteKey = TSystemDefaultsCache.GSystemDefaultsCache.GetInt64Default(SharedConstants.SYSDEFAULT_SITEKEY);

            if (DomainManager.GSiteKey <= 0)
            {
                // this is for connecting to legacy database format
                // we cannot add SiteKey to SystemDefaults, because Petra 2.3 would have a conflict since it adds it on startup already to the in-memory defaults, but not to the database
                // see also https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=114
                DomainManager.GSiteKey = TSystemDefaultsCache.GSystemDefaultsCache.GetInt64Default("SiteKeyPetra2");
            }

            if (DomainManager.GSiteKey <= 0)
            {
                // this can happen either with a legacy Petra 2.x database or with a fresh OpenPetra database without any ledger yet
                Console.WriteLine("there is no SiteKey or SiteKeyPetra2 record in s_system_defaults");
                DomainManager.GSiteKey = 99000000;
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
            new TLogging(TSrvSetting.ServerLogFile);

            TLogging.LogAtLevel(9, "Connecting to Database...");
            DBAccess.GDBAccessObj = new TDataBasePetra();
            TLogging.LogAtLevel(9, "DBAccessObj object created.");

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
                TLogging.LogAtLevel(9, "Connected to Database.");
            }
            catch (Exception /* exp */)
            {
                // TLogging.Log('Exception occured while establishing connection to Database Server: ' + exp.ToString);
                throw;
            }
        }

        /// <summary>
        /// Closes the Database connection for this AppDomain.
        ///
        /// @comment WARNING: If you need to rename this method or change its parameters,
        /// you also need to change the String with its name and the parameters in the
        /// .NET Reflection call in TClientAppDomainConnection!
        ///
        /// </summary>
        /// <returns>void</returns>
        public void CloseDBConnection()
        {
            // Console.WriteLine('TClientDomainManager.CloseDBConnection in AppDomain: ' + Thread.GetDomain().FriendlyName);
            TLogging.LogAtLevel(9, "TClientDomainManager.CloseDBConnection: before calling GDBAccessObj.CloseDBConnection");

            try
            {
                DBAccess.GDBAccessObj.CloseDBConnection();
            }
            catch (EDBConnectionNotAvailableException)
            {
                // The DB connection was never opened. Since this is no problem here, ignore this Exception.
                TLogging.LogAtLevel(9, "TClientDomainManager.CloseDBConnection: Info: DB Connection was never opened, so can't close.");
            }
            catch (Exception)
            {
                throw;
            }

            TLogging.LogAtLevel(9, "TClientDomainManager.CloseDBConnection: after calling GDBAccessObj.CloseDBConnection");
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
        /// Called after the ClientDomain is up and running - add any initialisation in here.
        /// </summary>
        public void PostAppDomainSetupInitialisation()
        {
            TDBTransaction ReadTransaction = null;
            
            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
            delegate
            {                        
                StringHelper.CurrencyFormatTable = ACurrencyAccess.LoadAll(ReadTransaction);
            });
        }
    }
}