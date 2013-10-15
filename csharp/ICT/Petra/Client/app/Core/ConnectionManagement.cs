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
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MPersonnel;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Reflection;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Manages the connection to the PetraServer.
    /// </summary>
    public class TConnectionManagement : TConnectionManagementBase
    {
        private String FRemotingURL_MConference;
        private String FRemotingURL_MPartner;
        private String FRemotingURL_MPersonnel;
        private String FRemotingURL_MCommon;
        private String FRemotingURL_MFinance;
        private String FRemotingURL_MReporting;
        private String FRemotingURL_MSysMan;
        private IMCommonNamespace FRemoteCommonObjects;
        private IMConferenceNamespace FRemoteConferenceObjects;
        private IMPartnerNamespace FRemotePartnerObjects;
        private IMPersonnelNamespace FRemotePersonnelObjects;
        private IMFinanceNamespace FRemoteFinanceObjects;
        private IMReportingNamespace FRemoteReportingObjects;
        private IMSysManNamespace FRemoteSysManObjects;

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <returns></returns>
        public bool ConnectToServer(String AUserName,
            String APassword,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError)
        {
            IPrincipal LocalUserInfo;

            if (!ConnectToServer(AUserName, APassword, out AProcessID, out AWelcomeMessage, out ASystemEnabled, out AError, out LocalUserInfo))
            {
                return false;
            }

            Ict.Petra.Shared.UserInfo.GUserInfo = (TPetraPrincipal)LocalUserInfo;

            FRemoteConferenceObjects = (IMConferenceNamespace)FConnector.GetRemoteObject(FRemotingURL_MConference, typeof(IMConferenceNamespace));
            FRemotePersonnelObjects = (IMPersonnelNamespace)FConnector.GetRemoteObject(FRemotingURL_MPersonnel, typeof(IMPersonnelNamespace));
            FRemoteCommonObjects = (IMCommonNamespace)FConnector.GetRemoteObject(FRemotingURL_MCommon, typeof(IMCommonNamespace));
            FRemotePartnerObjects = (IMPartnerNamespace)FConnector.GetRemoteObject(FRemotingURL_MPartner, typeof(IMPartnerNamespace));
            FRemoteFinanceObjects = (IMFinanceNamespace)FConnector.GetRemoteObject(FRemotingURL_MFinance, typeof(IMFinanceNamespace));
            FRemoteReportingObjects = (IMReportingNamespace)FConnector.GetRemoteObject(FRemotingURL_MReporting, typeof(IMReportingNamespace));
            FRemoteSysManObjects = (IMSysManNamespace)FConnector.GetRemoteObject(FRemotingURL_MSysMan, typeof(IMSysManNamespace));

            //
            // initialise object that holds references to all our remote object .NET Remoting Proxies
            //
            FRemote = new TRemote(FClientManager,
                FRemoteCommonObjects,
                FRemoteConferenceObjects,
                FRemotePartnerObjects,
                FRemotePersonnelObjects,
                FRemoteFinanceObjects,
                FRemoteReportingObjects,
                FRemoteSysManObjects);

            return true;
        }

        /// <summary>
        /// specific things for connecting all the services
        /// </summary>
        /// <param name="AUserName"></param>
        /// <param name="APassword"></param>
        /// <param name="AClientManager"></param>
        /// <param name="AProcessID"></param>
        /// <param name="AWelcomeMessage"></param>
        /// <param name="ASystemEnabled"></param>
        /// <param name="AError"></param>
        /// <param name="AUserInfo"></param>
        /// <returns></returns>
        protected override bool ConnectClient(String AUserName,
            String APassword,
            IClientManagerInterface AClientManager,
            out Int32 AProcessID,
            out String AWelcomeMessage,
            out Boolean ASystemEnabled,
            out String AError,
            out IPrincipal AUserInfo)
        {
            try
            {
                if (!base.ConnectClient(AUserName,
                        APassword,
                        AClientManager,
                        out AProcessID,
                        out AWelcomeMessage,
                        out ASystemEnabled,
                        out AError,
                        out AUserInfo))
                {
                    return false;
                }

                Ict.Petra.Shared.UserInfo.GUserInfo = (TPetraPrincipal)AUserInfo;

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN))
                {
                    FRemotingURL_MSysMan = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MSYSMAN];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON))
                {
                    FRemotingURL_MCommon = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MCOMMON];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE))
                {
                    FRemotingURL_MConference = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MCONFERENCE];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER))
                {
                    FRemotingURL_MPartner = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MPARTNER];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL))
                {
                    FRemotingURL_MPersonnel = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MPERSONNEL];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE))
                {
                    FRemotingURL_MFinance = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MFINANCE];
                }

                if (FRemotingURLs.ContainsKey(SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING))
                {
                    FRemotingURL_MReporting = (String)FRemotingURLs[SharedConstants.REMOTINGURL_IDENTIFIER_MREPORTING];
                }

                return true;
            }
            catch (ELoginFailedServerTooBusyException)
            {
                throw;
            }
            catch (Exception exp)
            {
                TLogging.Log(exp.ToString() + Environment.NewLine + exp.StackTrace, TLoggingType.ToLogfile);
                throw;
            }
        }
    }
}