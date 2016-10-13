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

using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MCommon;

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// Helper Class for using the <see cref="TFrmExtendedMessageBox"/>.
    /// </summary>
    public class TExtendedMessageBoxHelper
    {
        /// <summary>
        /// Shows a message that the user will need to have different Module permissions for the editing of data
        /// in *this* screen. Call this Method if the screen is read-only and the user has admin rights for the
        /// Module in which the screen is listed in the Main Menu.
        /// The user can choose to not get that message shown again (handled inside this called Method)  - this
        /// choice is stored in a UserDefault.
        /// </summary>
        /// <param name="AParentForm">Reference to the calling Form.</param>
        /// <param name="AShownOnMenuForConvenience">The Module in which the screen is listed in the
        /// Main Menu.</param>
        /// <param name="AModuleThatDataIsAssociatedWith">Module that the data that this Form allows to edit
        /// is associated with.</param>
        /// <param name="AAdminModulePermission">The Module Permission required for editing data in the screen
        /// that differs from the Module Permission that is required for editing data of Setup screens in the
        /// respective Module.</param>
        /// <param name="AUserDefaultNameForNotShowingTheMessageBoxAgain">Name of the UserDefault.
        /// Gets prefixed with <see cref="TUserDefaults.NamedDefaults.SUPPRESS_MESSAGE_PREFIX"/>!</param>
        public static void MsgUserWillNeedToHaveDifferentAdminModulePermissionForEditing(Form AParentForm,
            string AShownOnMenuForConvenience, string AModuleThatDataIsAssociatedWith,
            string AAdminModulePermission, string AUserDefaultNameForNotShowingTheMessageBoxAgain)
        {
            TFrmExtendedMessageBox ExtendedMessageBox = new TFrmExtendedMessageBox(AParentForm);
            bool DoNotShowMessageBoxAgain;
            string FinalUserDefaultName = TUserDefaults.NamedDefaults.SUPPRESS_MESSAGE_PREFIX +
                                          AUserDefaultNameForNotShowingTheMessageBoxAgain;

            if (!TUserDefaults.GetBooleanDefault(FinalUserDefaultName, false))
            {
                ExtendedMessageBox.ShowDialog(
                    String.Format(MCommonResourcestrings.StrDiffentPermissionRequiredForEditingData,
                        AAdminModulePermission) + Environment.NewLine +
                    String.Format(
                        MCommonResourcestrings.StrDiffentPermissionRequiredForEditingDataMenuHint,
                        AShownOnMenuForConvenience, AModuleThatDataIsAssociatedWith),
                    MCommonResourcestrings.StrReadOnlyInformationTitle,
                    MCommonResourcestrings.StrDontShowThisMessageAgain,
                    TFrmExtendedMessageBox.TButtons.embbOK, TFrmExtendedMessageBox.TIcon.embiInformation);

                // We don't care about the return value because there is only an 'OK' Button shown...
                ExtendedMessageBox.GetResult(out DoNotShowMessageBoxAgain);

                if (DoNotShowMessageBoxAgain)
                {
                    TUserDefaults.SetDefault(FinalUserDefaultName,
                        DoNotShowMessageBoxAgain);
                }
            }
        }
    }
}