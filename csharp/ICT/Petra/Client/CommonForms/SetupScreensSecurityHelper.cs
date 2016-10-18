//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// Helper Class for security aspects of 'Setup Screens'.
    /// </summary>
    public static class TSetupScreensSecurityHelper
    {
        /// <summary>
        /// Shows an information message to the user if the screen is read-only and the user has admin rights
        /// for the respective Module (see if-statement in this Method): that the user will need different
        /// admin rights (PTNRADMIN) for the screen passed in with <paramref name="AParentForm"/> than for
        /// other Setup screens in the respective Module.  The user can choose to not get that message shown
        /// again (handled inside the called Method) - this choice is stored in a UserDefault.
        /// </summary>
        /// <param name="AParentForm">Reference to the calling Form.</param>
        /// <param name="AScreenIsReadOnly">Set to true if the screen is in read-only mode, to false if
        /// this is not the case.</param>
        /// <param name="AContext">Context in which the calling Form gets called (to be passed in from the
        /// Main Menu.)</param>
        /// <param name="AModuleThatDataIsAssociatedWith">Module that the data that this Form allows to edit
        /// is associated with.</param>
        /// <param name="AAdminModulePermission">The Module Permission required for editing data in the screen
        /// that differs from the Module Permission that is required for editing data of Setup screens in the
        /// respective Module.</param>
        /// <param name="AUserDefaultNameForNotShowingTheMessageBoxAgain">Name of the UserDefault. Gets prefixed with
        /// <see cref="TUserDefaults.NamedDefaults.SUPPRESS_MESSAGE_PREFIX"/>!</param>
        public static void ShowMsgUserWillNeedToHaveDifferentAdminModulePermissionForEditing(
            Form AParentForm, bool AScreenIsReadOnly, string AContext,
            string AModuleThatDataIsAssociatedWith,
            string AAdminModulePermission,
            string AUserDefaultNameForNotShowingTheMessageBoxAgain)
        {
            string FinalUserDefaultName = AUserDefaultNameForNotShowingTheMessageBoxAgain + AContext;
            string UserWillNeedToHaveAdminModulePermission = String.Empty;

            if (AContext == "Finance")
            {
                UserWillNeedToHaveAdminModulePermission = SharedConstants.PETRAMODULE_FINANCE3;
            }
            else if (AContext == "Personnel")
            {
                UserWillNeedToHaveAdminModulePermission = SharedConstants.PETRAMODULE_PERSADMIN;
            }

            // If the screen is read-only and the user has admin rights for the respective Module (see
            // if-statement just above) then show a message to the user that (s)he will need different
            // admin rights (PTNRADMIN) for *this* screen than for other Setup screens in the respective
            // Module.  The user can choose to not get that message shown again (handled inside the
            // called Method).
            if (AScreenIsReadOnly
                && (UserInfo.GUserInfo.IsInModule(UserWillNeedToHaveAdminModulePermission)))
            {
                TExtendedMessageBoxHelper.MsgUserWillNeedToHaveDifferentAdminModulePermissionForEditing(
                    AParentForm,
                    (AContext == "Finance" ? Catalog.GetString("Finance") : Catalog.GetString("Personnel")),
                    AModuleThatDataIsAssociatedWith, AAdminModulePermission, FinalUserDefaultName);
            }
        }
    }
}