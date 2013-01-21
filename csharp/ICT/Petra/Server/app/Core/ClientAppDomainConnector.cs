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
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// Allows creation of and connection to a Client's AppDomain without causing
    /// the Assemblies which are loaded in the Client's AppDomain to be loaded into
    /// the Default AppDomain.
    ///
    /// @comment This class is used by TClientManager to create AppDomains for Clients
    /// and to communicate with them, using a TRemoteLoader object.
    ///
    /// </summary>
    public class TClientAppDomainConnection : TClientAppDomainConnectionBase
    {
        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MCOMMON_DLLNAME = "Ict.Petra.Server.lib.MCommon.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MCOMMON_CLASSNAME = "Ict.Petra.Server.MCommon.Instantiator.TMCommonNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MCONFERENCE_DLLNAME = "Ict.Petra.Server.lib.MConference.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MCONFERENCE_CLASSNAME = "Ict.Petra.Server.MConference.Instantiator.TMConferenceNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MSYSMAN_DLLNAME = "Ict.Petra.Server.lib.MSysMan.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MSYSMAN_CLASSNAME = "Ict.Petra.Server.MSysMan.Instantiator.TMSysManNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MPARTNER_DLLNAME = "Ict.Petra.Server.lib.MPartner.connect";

        /// <summary>need to leave out the last part of the Namespace so that .NET can find the Class!</summary>
        public const String MPARTNER_CLASSNAME = "Ict.Petra.Server.MPartner.Instantiator.TMPartnerNamespaceLoader";

        /// <summary>need to leave out '.dll' suffix so that .NET can find the Assembly!</summary>
        public const String MPERSONNEL_DLLNAME = "Ict.Petra.Server.lib.MPersonnel.connect";

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

        /// Load Petra Module DLLs into Clients AppDomain, initialise them and remote an Instantiator Object
        public override void LoadAssemblies(string AClientID, IPrincipal AUserInfo, ref Hashtable ARemotingURLs)
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
            LoadPetraModuleAssembly(AClientID, MSYSMAN_DLLNAME, MSYSMAN_CLASSNAME, out RemotingURL_MSysMan);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN, RemotingURL_MSysMan);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMSysMan instantiated. Remoting URL: " + RemotingURL_MSysMan);
            }

            // Load COMMON Module assembly (always loaded)
            LoadPetraModuleAssembly(AClientID, MCOMMON_DLLNAME, MCOMMON_CLASSNAME, out RemotingURL_MCommon);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON, RemotingURL_MCommon);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMCommon instantiated. Remoting URL: " + RemotingURL_MCommon);
            }

            // Load CONFERENCE Module assembly (always loaded)
            LoadPetraModuleAssembly(AClientID, MCONFERENCE_DLLNAME, MCONFERENCE_CLASSNAME, out RemotingURL_MConference);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE, RemotingURL_MConference);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMConference instantiated. Remoting URL: " + RemotingURL_MConference);
            }

            // Load PARTNER Module assembly (always loaded)
            LoadPetraModuleAssembly(AClientID, MPARTNER_DLLNAME, MPARTNER_CLASSNAME, out RemotingURL_MPartner);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER, RemotingURL_MPartner);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMPartner instantiated. Remoting URL: " + RemotingURL_MPartner);
            }

            // Load REPORTING Module assembly (always loaded)
            LoadPetraModuleAssembly(AClientID, MREPORTING_DLLNAME, MREPORTING_CLASSNAME, out RemotingURL_MReporting);
            ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING, RemotingURL_MReporting);

            if (TLogging.DL >= 5)
            {
                Console.WriteLine("  TMReporting instantiated. Remoting URL: " + RemotingURL_MReporting);
            }

            // Load PERSONNEL Module assembly (loaded only for users that have personnel privileges)
            if (UserInfo.IsInModule(SharedConstants.PETRAMODULE_PERSONNEL))
            {
                LoadPetraModuleAssembly(AClientID, MPERSONNEL_DLLNAME, MPERSONNEL_CLASSNAME, out RemotingURL_MPersonnel);
                ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL, RemotingURL_MPersonnel);

                if (TLogging.DL >= 5)
                {
                    Console.WriteLine("  TMPersonnel instantiated. Remoting URL: " + RemotingURL_MPersonnel);
                }
            }

            // Load FINANCE Module assembly (loaded only for users that have finance privileges)
            if ((UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1)) || (UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE2))
                || (UserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3)))
            {
                LoadPetraModuleAssembly(AClientID, MFINANCE_DLLNAME, MFINANCE_CLASSNAME, out RemotingURL_MFinance);
                ARemotingURLs.Add(SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE, RemotingURL_MFinance);

                if (TLogging.DL >= 5)
                {
                    Console.WriteLine("  TMFinance instantiated. Remoting URL: " + RemotingURL_MFinance);
                }
            }
        }
    }
}