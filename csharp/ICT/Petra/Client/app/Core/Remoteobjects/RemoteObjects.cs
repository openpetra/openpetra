//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Client.App.Core.RemoteObjects
{
    /// <summary>
    /// Holds all references to objects that communicate with the server
    ///
    /// The TRemote class is used by the Client for all communication with the Server
    /// after the initial communication at Client start-up is done.
    ///
    /// The properties MPartner, MFinance, etc. represent the top-most level of the
    /// Petra Partner, Finance, etc. Petra Module Namespaces in the PetraServer.
    /// </summary>
    public class TRemote
    {
        /// <summary>Reference to the topmost level of the Petra Common Module Namespace</summary>
        public static TMCommonNamespace MCommon
        {
            get
            {
                return UCommonObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Partner Module Namespace</summary>
        public static TMConferenceNamespace MConference
        {
            get
            {
                return UConferenceObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Partner Module Namespace</summary>
        public static TMPartnerNamespace MPartner
        {
            get
            {
                return UPartnerObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra Personnel Module Namespace</summary>
        public static TMPersonnelNamespace MPersonnel
        {
            get
            {
                return UPersonnelObject;
            }
        }

        /// <summary>Reference to  the topmost level of the Petra Finance Module Namespace</summary>
        public static TMFinanceNamespace MFinance
        {
            get
            {
                return UFinanceObject;
            }
        }

        /// <summary>Reference to  the topmost level of the Petra Reporting Module Namespace</summary>
        public static TMReportingNamespace MReporting
        {
            get
            {
                return UReportingObject;
            }
        }

        /// <summary>Reference to the topmost level of the Petra System Manager Module Namespace</summary>
        public static TMSysManNamespace MSysMan
        {
            get
            {
                return USysManObject;
            }
        }

        private static TMCommonNamespace UCommonObject;
        private static TMConferenceNamespace UConferenceObject;
        private static TMPartnerNamespace UPartnerObject;
        private static TMPersonnelNamespace UPersonnelObject;
        private static TMFinanceNamespace UFinanceObject;
        private static TMReportingNamespace UReportingObject;
        private static TMSysManNamespace USysManObject;

        /// <summary>
        /// initialize the references
        /// </summary>
        public TRemote()
        {
            USysManObject = new TMSysManNamespace();
            UConferenceObject = new TMConferenceNamespace();
            UPartnerObject = new TMPartnerNamespace();
            UCommonObject = new TMCommonNamespace();
            UPersonnelObject = new TMPersonnelNamespace();
            UFinanceObject = new TMFinanceNamespace();
            UReportingObject = new TMReportingNamespace();
        }
    }
}