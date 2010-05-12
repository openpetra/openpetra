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

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains resourcetexts that are used across several Petra Modules.
    /// </summary>
    public class CommonResourcestrings
    {
        /// <summary>todoComment</summary>
        public const String StrGenericInfo = "Information";

        /// <summary>todoComment</summary>
        public const String StrGenericWarning = "Warning";

        /// <summary>todoComment</summary>
        public const String StrGenericError = "Error";

        /// <summary>todoComment</summary>
        public const String StrGenericReady = "Ready";

        /// <summary>todoComment</summary>
        public const String StrGenericInactiveCode = " (inactive)";

        /// <summary>todoComment</summary>
        public const String StrGenericFunctionalityNotAvailable = "Functionality not available";

        /// <summary>todoComment</summary>
        public const String StrFormHasUnsavedChanges = "This window has changes that have not been saved.";

        /// <summary>todoComment</summary>
        public const String StrFormHasUnsavedChangesQuestion = "Save changes before closing?";

        /// <summary>todoComment</summary>
        public const String StrSavingDataCancelled = "Saving of data cancelled by user!";

        /// <summary>todoComment</summary>
        public const String StrSavingDataNothingToSave = "There was nothing to be saved.";

        /// <summary>todoComment</summary>
        public const String StrSavingDataErrorOccured = "An error occured during the saving of the data!";

        /// <summary>todoComment</summary>
        public const String StrPetraServerTooBusy = "The Petra Server is currently too busy to {0}." + "\r\n" + "\r\n" +
                                                    "Please wait a few seconds and press 'Retry' then to retry, or 'Cancel' to abort.";

        /// <summary>todoComment</summary>
        public const String StrPetraServerTooBusyTitle = "Petra Server Too Busy";

        /// <summary>todoComment</summary>
        public const String StrOpeningCancelledByUser = "Opening of {0} screen got cancelled by user.";

        /// <summary>todoComment</summary>
        public const String StrOpeningCancelledByUserTitle = "Screen opening cancelled";

        /// <summary>todoComment</summary>
        public const String StrPartnerClass = "Partner Class";

        /// <summary>todoComment</summary>
        public const String StrPartnerKey = "Partner Key";

        /// <summary>todoComment</summary>
        public const String StrErrorOnlyForPerson = "This only works for Partners of Partner Class PERSON";

        /// <summary>todoComment</summary>
        public const String StrErrorOnlyForFamilyOrPerson = "This only works for Partners of Partner Classes FAMILY or PERSON";

        /// <summary>todoComment</summary>
        public const String StrErrorOnlyForPersonOrUnit = "This only works for Partners of Partner Classes PERSON or UNIT";

        /// <summary>todoComment</summary>
        public const String StrErrorTheCodeIsNoLongerActive1 = "The code";

        /// <summary>todoComment</summary>
        public const String StrErrorTheCodeIsNoLongerActive2 = "is no longer active.";

        /// <summary>todoComment</summary>
        public const String StrErrorTheCodeIsNoLongerActive3 = "Do you still want to use it?";

        /// <summary>todoComment</summary>
        public const String StrErrorNoInstalledSites = "No Installed Sites!";

        /// <summary>todoComment</summary>
        public const String StrBtnTextNew = "&New";

        /// <summary>todoComment</summary>
        public const String StrBtnTextEdit = "Edi&t";

        /// <summary>todoComment</summary>
        public const String StrBtnTextDelete = "&Delete";

        /// <summary>todoComment</summary>
        public const String StrBtnTextCancel = "&Cancel";

        /// <summary>todoComment</summary>
        public const String StrBtnTextDone = "D&one";

        /// <summary>todoComment</summary>
        public const String StrPartnerStatusChange = "The Partner's Status is currently" + " '{0}'." + "\r\n" +
                                                     "Petra will change it automatically to '{1}'";

        /// <summary>todoComment</summary>
        public const String StrPartnerReActivationTitle = "Partner Gets Re-activated!";
    }
}