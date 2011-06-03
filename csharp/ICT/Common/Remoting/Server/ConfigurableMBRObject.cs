//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Configuration;
using System.Runtime.Remoting.Lifetime;
using Ict.Common;

namespace Ict.Common.Remoting.Server
{
    /// <summary>
    /// Allows remotable objects that are derived from it (instead of MarshalByRefObject) to make their lifetime configurable via the application's config file.
    /// see also the book 'Advanced .NET Remoting' (Chapter 7, page 193)
    /// </summary>
    /// <example>
    /// &lt;add key="Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector_Lifetime" value="4000" /&gt;
    /// This sets the lifetime of this object to four seconds.
    /// </example>
    public class TConfigurableMBRObject : MarshalByRefObject
    {
        /// <summary>
        /// Overrides the InitializeLifetimeService function of objects that would normally derive from MarshalByRefObject (such objects must not have a InitializeLifetimeService function then)
        /// </summary>
        /// <returns>void</returns>
        public override System.Object InitializeLifetimeService()
        {
            ILease tmp;
            string myName = this.GetType().FullName;

            // if value is not in the config file, assuming default values, 60 seconds
            string lifetime = TAppSettingsManager.GetValue(myName + "_Lifetime", "60000");
            string renewoncall = TAppSettingsManager.GetValue(myName + "_RenewOnCallTime", "60000");
            string sponsorshiptimeout = TAppSettingsManager.GetValue(myName + "_SponsorShipTimeout", "60000");

            if (lifetime == "infinity")
            {
                return null;
            }
            else
            {
                tmp = (ILease) base.InitializeLifetimeService();

                if (tmp.CurrentState == LeaseState.Initial)
                {
                    if (lifetime != null)
                    {
                        tmp.InitialLeaseTime = TimeSpan.FromMilliseconds(double.Parse(lifetime));
                    }

                    if (renewoncall != null)
                    {
                        tmp.RenewOnCallTime = TimeSpan.FromMilliseconds(double.Parse(renewoncall));
                    }

                    if (sponsorshiptimeout != null)
                    {
                        tmp.SponsorshipTimeout = TimeSpan.FromMilliseconds(double.Parse(sponsorshiptimeout));
                    }
                }

                return tmp;
            }
        }
    }
}