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
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MPersonnel;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.Interfaces.MReporting;

namespace Ict.Petra.Client.App.Core.RemoteObjects
{
    /// <summary>
    /// Holds all references to instantiated Serverside objects that are remoted by the Server.
    /// These objects can get remoted either statically (the same Remoting URL all
    /// the time) or dynamically (on-the-fly generation of Remoting URL).
    ///
    /// The TRemote class is used by the Client for all communication with the Server
    /// after the initial communication at Client start-up is done.
    ///
    /// The properties MPartner, MFinance, etc. represent the top-most level of the
    /// Petra Partner, Finance, etc. Petra Module Namespaces in the PetraServer.
    /// </summary>
    public class TRemote : TRemoteBase
    {
        /// <summary>Reference to the topmost level of the Petra Common Module Namespace</summary>
        public static IMCommonNamespace MCommon
        {
            get
            {
                return UCommonObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Partner Module Namespace</summary>
        public static IMConferenceNamespace MConference
        {
            get
            {
                return UConferenceObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Partner Module Namespace</summary>
        public static IMPartnerNamespace MPartner
        {
            get
            {
                return UPartnerObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Personnel Module Namespace</summary>
        public static IMPersonnelNamespace MPersonnel
        {
            get
            {
                return UPersonnelObject;
            }
        }

        /// <summary>Reference to  the topmost level of the Petra Finance Module Namespace</summary>
        public static IMFinanceNamespace MFinance
        {
            get
            {
                return UFinanceObject;
            }
        }

        /// <summary>Reference to  the topmost level of the Petra Reporting Module Namespace</summary>
        public static IMReportingNamespace MReporting
        {
            get
            {
                return UReportingObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra System Manager Module Namespace</summary>
        public static IMSysManNamespace MSysMan
        {
            get
            {
                return USysManObject;
            }
        }

        private static IMCommonNamespace UCommonObject;
        private static IMConferenceNamespace UConferenceObject;
        private static IMPartnerNamespace UPartnerObject;
        private static IMPersonnelNamespace UPersonnelObject;
        private static IMFinanceNamespace UFinanceObject;
        private static IMReportingNamespace UReportingObject;
        private static IMSysManNamespace USysManObject;

        /// <summary>
        /// References to the rest of the topmost level of the other Petra Module Namespaces will go here
        /// </summary>
        /// <returns>void</returns>
        public TRemote(IClientManagerInterface AClientManager,
            IMCommonNamespace ACommonObject,
            IMConferenceNamespace AConferenceObject,
            IMPartnerNamespace APartnerObject,
            IMPersonnelNamespace APersonnelObject,
            IMFinanceNamespace AFinanceObject,
            IMReportingNamespace AReportingObject,
            IMSysManNamespace ASysManObject)
            : base(AClientManager)
        {
            USysManObject = ASysManObject;
            UConferenceObject = AConferenceObject;
            UPartnerObject = APartnerObject;
            UCommonObject = ACommonObject;
            UPersonnelObject = APersonnelObject;
            UFinanceObject = AFinanceObject;
            UReportingObject = AReportingObject;

            FRemoteObjects.Add((MarshalByRefObject)USysManObject);
            FRemoteObjects.Add((MarshalByRefObject)UConferenceObject);
            FRemoteObjects.Add((MarshalByRefObject)UPartnerObject);
            FRemoteObjects.Add((MarshalByRefObject)UCommonObject);
            FRemoteObjects.Add((MarshalByRefObject)UPersonnelObject);
            FRemoteObjects.Add((MarshalByRefObject)UFinanceObject);
            FRemoteObjects.Add((MarshalByRefObject)UReportingObject);
        }
    }
}