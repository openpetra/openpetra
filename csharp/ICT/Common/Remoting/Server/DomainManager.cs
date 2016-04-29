//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Web;
using System.Collections.Generic;
using System.Threading;
using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Session;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// holds some global variables, depending on the current session or thread
    /// </summary>
    public class DomainManager
    {
        private static Func <Int64>FGetSiteKeyFromSystemDefaultsCacheDelegate;

        /// <summary>Used internally for accessing SiteKey Information (for convenience).</summary>
        /// <remarks>The SiteKey in OpenPetra is part of the data that is held in the System Defaults and gets returned from
        /// OpenPetra's server-side System Defaults Cache.
        /// <para><em>Important:</em>The SiteKey can get changed by a user with the necessary priviledges while being logged
        /// in to OpenPetra and any further inquiry of the GSiteKey Property reflects any such change!!!</para></remarks>
        public static Int64 GSiteKey
        {
            get
            {
                if (FGetSiteKeyFromSystemDefaultsCacheDelegate != null)
                {
                    return FGetSiteKeyFromSystemDefaultsCacheDelegate();
                }
                else
                {
                    throw new Exception("GetSiteKeyFromSystemDefaultsCacheDelegate must be set up, but it isn't!");
                }
            }
        }

        /// <summary>
        /// Delegate that allows <see cref="DomainManagerBase" /> to access the System Defaults Cache of OpenPetra.
        /// </summary>
        public static Func <Int64>GetSiteKeyFromSystemDefaultsCacheDelegate
        {
            get
            {
                return FGetSiteKeyFromSystemDefaultsCacheDelegate;
            }

            set
            {
                FGetSiteKeyFromSystemDefaultsCacheDelegate = value;
            }
        }

        /// <summary>
        /// get the ClientID of the current session
        /// </summary>
        public static Int32 GClientID
        {
            get
            {
                if (TSession.HasVariable("ClientID"))
                {
                    string ClientID = TSession.GetVariable("ClientID").ToString();
                    return Convert.ToInt32(ClientID);
                }

                throw new EOPDBInvalidSessionException("Session is invalid - it does not have a Client ID");
            }

            set
            {
                TSession.SetVariable("ClientID", value);
            }
        }

        /// <summary>
        /// the current client in this session
        /// </summary>
        public static TConnectedClient CurrentClient
        {
            get
            {
                return (TConnectedClient)TSession.GetVariable("ConnectedClient");
            }

            set
            {
                TSession.SetVariable("ConnectedClient", value);
            }
        }
    }
}
