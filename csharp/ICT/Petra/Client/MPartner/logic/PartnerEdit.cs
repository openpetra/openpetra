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

using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MPartner.Logic
{
    /// <summary>
    /// Contains logic for the Partner Edit Screen.
    /// </summary>
    public class TPartnerEditScreenLogic
    {
        #region Enums

        /// <summary>
        /// Used for identification of the TabGroups of the Partner Edit screen according to the
        /// Petra Module.
        /// </summary>
        public enum TModuleTabGroupEnum
        {
            /// <summary>No module</summary>
            mtgNone,

            /// <summary>Partner Module</summary>
            mtgPartner,

            /// <summary>Personnel Module</summary>
            mtgPersonnel,

            /// <summary>Finance Module</summary>
            mtgFinance
        }

        #endregion

        /// <summary>
        /// Determines the TabGroup that a particular TabPage is part of.
        /// </summary>
        /// <param name="ATabPage">Tab.</param>
        /// <returns>TabGroup that the TabPage specified with <paramref name="ATabPage" /> is part of.</returns>
        public static TModuleTabGroupEnum DetermineTabGroup(TPartnerEditTabPageEnum ATabPage)
        {
            switch (ATabPage)
            {
                case TPartnerEditTabPageEnum.petpDefault:
                case TPartnerEditTabPageEnum.petpAddresses:
                case TPartnerEditTabPageEnum.petpDetails:
                case TPartnerEditTabPageEnum.petpSubscriptions:
                case TPartnerEditTabPageEnum.petpPartnerTypes:
                case TPartnerEditTabPageEnum.petpFamilyMembers:
                case TPartnerEditTabPageEnum.petpPartnerRelationships:
                case TPartnerEditTabPageEnum.petpNotes:
                case TPartnerEditTabPageEnum.petpOfficeSpecific:
                case TPartnerEditTabPageEnum.petpFoundationDetails:
                case TPartnerEditTabPageEnum.petpContacts:
                case TPartnerEditTabPageEnum.petpReminders:
                case TPartnerEditTabPageEnum.petpInterests:
                    return TModuleTabGroupEnum.mtgPartner;

                case TPartnerEditTabPageEnum.petpPersonnelIndividualData:
                case TPartnerEditTabPageEnum.petpPersonnelApplications:
                    return TModuleTabGroupEnum.mtgPersonnel;

                default:
                    // Fallback
                    return TModuleTabGroupEnum.mtgPartner;
            }
        }
    }
}