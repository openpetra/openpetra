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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MPersonnel;
using Ict.Petra.Shared.Interfaces.MSysMan;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ClientManager and to retrieve Remoting Proxy objects for
    /// the Server-side .NET Remoting Sponsor and several other remoted objects from
    /// the PetraServer.
    /// </summary>
    public class TConnector : TConnectorBase
    {
        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MCommon namespace
        ///
        /// @comment The MCommon Namespace holds client-instantiable objects for the
        /// Petra Common Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MCommon namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MCommon namespace object</param>
        public void GetRemoteMCommonObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMCommonNamespace ARemote)
        {
            string strTCP;
            string strServer = null;

            ARemote = null;

            if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
            {
                TLogging.Log("Entering GetRemoteMCommonObject()...", TLoggingType.ToLogfile);
            }

            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                }

                ARemote = (IMCommonNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMCommonNamespace)).GetTransparentProxy();

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMCommonObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteMCommonObject: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMCommonObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MConference namespace
        ///
        /// @comment The MConferenceNamespace holds client-instantiable objects for the
        /// Petra Conference Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MConference namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MConference namespace object</param>
        public void GetRemoteMConferenceObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMConferenceNamespace ARemote)
        {
            string strTCP;
            string strServer = null;

            ARemote = null;

            if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
            {
                TLogging.Log("Entering GetRemoteMConferenceObject()...", TLoggingType.ToLogfile);
            }

            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                }

                ARemote = (IMConferenceNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMConferenceNamespace)).GetTransparentProxy();

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMConferenceObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteMConferenceObject: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMConferenceObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MPartner namespace
        ///
        /// @comment The MPartner Namespace holds client-instantiable objects for the
        /// Petra Partner Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MPartner namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MPartner namespace object</param>
        public void GetRemoteMPartnerObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMPartnerNamespace ARemote)
        {
            string strTCP;
            string strServer = null;

            ARemote = null;

            if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
            {
                TLogging.Log("Entering GetRemoteMPartnerObject()...", TLoggingType.ToLogfile);
            }

            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                }

                ARemote = (IMPartnerNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMPartnerNamespace)).GetTransparentProxy();

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMPartnerObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteMPartnerObject: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMPartnerObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MPersonnel namespace
        ///
        /// @comment The MPersonnel Namespace holds client-instantiable objects for the
        /// Petra Personnel Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MPersonnel namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MPersonnel namespace object</param>
        public void GetRemoteMPersonnelObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMPersonnelNamespace ARemote)
        {
            string strTCP;
            string strServer = null;

            ARemote = null;

            if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
            {
                TLogging.Log("Entering GetRemoteMPersonnelObject()...", TLoggingType.ToLogfile);
            }

            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                }

                ARemote = (IMPersonnelNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMPersonnelNamespace)).GetTransparentProxy();

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMPersonnelObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteMPersonnelObject: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMPersonnelObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MFinance namespace
        ///
        /// @comment The MFinance Namespace holds client-instantiable objects for the
        /// Petra Finance Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MFinance namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MFinance namespace object</param>
        public void GetRemoteMFinanceObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMFinanceNamespace ARemote)
        {
            ARemote = null;
            string strTCP;
            string strServer = null;

            if (RemotingURL != "")
            {
                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Entering GetRemoteMFinanceObject()...", TLoggingType.ToLogfile);
                }

                try
                {
                    strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                    strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                    }

                    ARemote = (IMFinanceNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMFinanceNamespace)).GetTransparentProxy();

                    if (ARemote == null)
                    {
                        TLogging.Log("GetRemoteMFinanceObject: Connection failed!", TLoggingType.ToLogfile);
                    }
                    else
                    {
                        if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                        {
                            TLogging.Log("GetRemoteMFinanceObject: connected.", TLoggingType.ToLogfile);
                        }
                    }
                }
                catch (Exception exp)
                {
                    TLogging.Log("Error in GetRemoteMFinanceObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MReporting namespace
        ///
        /// @comment The MReporting Namespace holds client-instantiable objects for the
        /// Petra Reporting Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MReporting namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MReporting namespace object</param>
        public void GetRemoteMReportingObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMReportingNamespace ARemote)
        {
            ARemote = null;
            string strTCP;
            string strServer = null;

            if (RemotingURL != "")
            {
                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Entering GetRemoteMReportingObject()...", TLoggingType.ToLogfile);
                }

                try
                {
                    strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                    strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                    }

                    ARemote = (IMReportingNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMReportingNamespace)).GetTransparentProxy();

                    if (ARemote == null)
                    {
                        TLogging.Log("GetRemoteMReportingObject: Connection failed!", TLoggingType.ToLogfile);
                    }
                    else
                    {
                        if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                        {
                            TLogging.Log("GetRemoteMReportingObject: connected.", TLoggingType.ToLogfile);
                        }
                    }
                }
                catch (Exception exp)
                {
                    TLogging.Log("Error in GetRemoteMReportingObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves a Remoting Proxy object for the Server-side MSysMan namespace
        ///
        /// @comment The MSysMan Namespace holds client-instantiable objects for the
        /// Petra System Manager Module.
        ///
        /// </summary>
        /// <param name="CrossDomainURL"></param>
        /// <param name="RemotingURL">The Server-assigned URL for the MSysMan namespace object</param>
        /// <param name="ClientID"></param>
        /// <param name="ARemote">.NET Remoting Proxy object for the MSysMan namespace object</param>
        public void GetRemoteMSysManObject(string CrossDomainURL, string RemotingURL, string ClientID, out IMSysManNamespace ARemote)
        {
            string strTCP;
            string strServer = null;

            ARemote = null;

            if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
            {
                TLogging.Log("Entering GetRemoteMSysManObject()...", TLoggingType.ToLogfile);
            }

            try
            {
                strServer = DetermineServerIPAddress() + ':' + ServerIPPort.ToString();
                strTCP = (("tcp://" + strServer) + '/' + CrossDomainURL);

                if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                {
                    TLogging.Log("Connecting to: " + strTCP, TLoggingType.ToLogfile);
                }

                ARemote = (IMSysManNamespace) new CustomProxy(strTCP, RemotingURL, ClientID, typeof(IMSysManNamespace)).GetTransparentProxy();

                if (ARemote == null)
                {
                    TLogging.Log("GetRemoteMSysManObject: Connection failed!", TLoggingType.ToLogfile);
                }
                else
                {
                    if (TLogging.DebugLevel >= CONNECTOR_LOGGING)
                    {
                        TLogging.Log("GetRemoteMSysManObject: connected.", TLoggingType.ToLogfile);
                    }
                }
            }
            catch (Exception exp)
            {
                TLogging.Log("Error in GetRemoteMSysManObject(), Possible reasons :-" + exp.ToString(), TLoggingType.ToLogfile);
                throw;
            }
        }
    }
}