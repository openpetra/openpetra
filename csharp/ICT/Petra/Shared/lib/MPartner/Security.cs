//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Contains security-related functions for Partners that that can be used by any Class.
    /// </summary>
    /// <remarks>There are other security-related Methods to be found on the server side. Class: Ict.Petra.Server.MParter, Security.cs, Class TSecurity.</remarks>
    public static class TSecurity
    {
        /// <summary>Partner is restricted to a Security Group</summary>
        public const byte PARTNER_RESTRICTED_TO_GROUP = 1;

        /// <summary>Partner is restricted to a User</summary>
        public const byte PARTNER_RESTRICTED_TO_USER = 2;

        /// <summary>
        /// Tests whether the current user has access to a particular Partner.
        /// </summary>
        /// <remarks>
        /// <para>Corresponds to Progress 4GL Method 'CanAccessPartner' in
        /// common/sp_partn.p</para>
        /// <para>A server-side implementation of this Method exists that has only the
        /// <paramref name="APartnerRow" />parameter as an Argument. It
        /// looks up the Foundation Row on its own if this is needed.</para>
        /// </remarks>
        /// <param name="APartnerRow">Partner for which access should be checked for.</param>
        /// <param name="AIsFoundation">Set to true if Partner is a Foundation.</param>
        /// <param name="AFoundationRow">Foundation Row needs to be passed in
        /// if Partner is a Foundation.</param>
        /// <returns><see cref="TPartnerAccessLevelEnum.palGranted" /> if access
        /// to the Partner is granted, otherwise a different
        /// <see cref="TPartnerAccessLevelEnum" /> value.</returns>
        public static TPartnerAccessLevelEnum CanAccessPartner(PPartnerRow APartnerRow, bool AIsFoundation,
            PFoundationRow AFoundationRow)
        {
            if ((APartnerRow.Restricted == PARTNER_RESTRICTED_TO_USER)
                && !((APartnerRow.UserId == UserInfo.GUserInfo.UserID) || UserInfo.GUserInfo.IsInModule("SYSMAN")))
            {
                TLogging.LogAtLevel(6,
                    "CanAccessPartner: Access DENIED - Partner " + APartnerRow.PartnerKey.ToString() + " is restriced to User " +
                    APartnerRow.UserId + "!");
                return TPartnerAccessLevelEnum.palRestrictedToUser;
            }
            else if ((APartnerRow.Restricted == PARTNER_RESTRICTED_TO_GROUP)
                     && !((UserInfo.GUserInfo.IsInGroup(APartnerRow.GroupId)) || UserInfo.GUserInfo.IsInModule("SYSMAN")))
            {
                TLogging.LogAtLevel(6,
                    "CanAccessPartner: Access DENIED - Partner " + APartnerRow.PartnerKey.ToString() + " is restriced to Group " +
                    APartnerRow.GroupId +
                    "!");
                return TPartnerAccessLevelEnum.palRestrictedToGroup;
            }

            if (APartnerRow.PartnerClass == SharedTypes.PartnerClassEnumToString(TPartnerClass.ORGANISATION))
            {
                if (AIsFoundation)
                {
                    if (AFoundationRow != null)
                    {
                        if (!CheckFoundationSecurity(AFoundationRow))
                        {
                            TLogging.LogAtLevel(6,
                                "CanAccessPartner: Access DENIED - Partner " + APartnerRow.PartnerKey.ToString() +
                                " is restriced by Foundation Ownership!");
                            return TPartnerAccessLevelEnum.palRestrictedByFoundationOwnership;
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("AFoundationRow must not be null if AIsFoundation is true");
                    }
                }
            }

            TLogging.LogAtLevel(6, "CanAccessPartner: Access to Partner " + APartnerRow.PartnerKey.ToString() + " is GRANTED!");
            return TPartnerAccessLevelEnum.palGranted;
        }

        /// <summary>
        /// Tests whether the current user has access to a particular Partner.
        /// </summary>
        /// <remarks>This Method throws an <see cref="ESecurityPartnerAccessDeniedException" />
        /// if access to the Partner is not granted, thereby ensuring that a denied access
        /// doesn't go unnoticed.</remarks>
        /// <param name="APartnerRow">Partner for which access should be checked for.</param>
        /// <param name="AIsFoundation">Set to true if Partner is a Foundation.</param>
        /// <param name="AFoundationRow">Foundation Row needs to be passed in
        /// if Partner is a Foundation.</param>
        /// <returns>void</returns>
        /// <exception cref="ESecurityPartnerAccessDeniedException">Thrown if access is not granted.</exception>
        public static void CanAccessPartnerExc(PPartnerRow APartnerRow, bool AIsFoundation,
            PFoundationRow AFoundationRow)
        {
            TPartnerAccessLevelEnum AccessLevel;

            AccessLevel = CanAccessPartner(APartnerRow, AIsFoundation, AFoundationRow);


            AccessLevelExceptionEvaluatorAndThrower(APartnerRow, AccessLevel);
        }

        /// <summary>
        /// Evaluates the passed in AccessLevel and throws
        /// <see cref="ESecurityPartnerAccessDeniedException" /> if the AccessLevel
        /// isn't <see cref="TPartnerAccessLevelEnum.palGranted" />.
        /// </summary>
        /// <param name="APartnerRow">Partner for which access should be checked for.</param>
        /// <param name="AAccessLevel">AccessLevel as determined by caller.</param>
        public static void AccessLevelExceptionEvaluatorAndThrower(
            PPartnerRow APartnerRow, TPartnerAccessLevelEnum AAccessLevel)
        {
            switch (AAccessLevel)
            {
                case TPartnerAccessLevelEnum.palGranted:
                    return;

                default:
                    throw new ESecurityPartnerAccessDeniedException("",
                    APartnerRow.PartnerKey, APartnerRow.PartnerShortName, AAccessLevel);
            }
        }

        /// <summary>
        /// Tests whether the current user has access to a particular Foundation.
        /// </summary>
        /// <remarks>Corresponds to Progress 4GL Method 'CheckFoundationSecurity' in
        /// common/sp_partn.p</remarks>
        /// <param name="AFoundationRow">Foundation row to check for.</param>
        /// <returns>True if the current user has access to the passed in Foundation,
        /// otherwise false.</returns>
        public static bool CheckFoundationSecurity(PFoundationRow AFoundationRow)
        {
            return CheckFoundationSecurity(AFoundationRow.Owner1Key, AFoundationRow.Owner2Key);
        }

        /// <summary>
        /// Tests whether the current user has access to a particular Foundation.
        /// </summary>
        /// <remarks>Corresponds to Progress 4GL Method 'CheckFoundationSecurity' in
        /// common/sp_partn.p</remarks>
        /// <param name="AFoundationOwner1Key">PartnerKey of the first owner of the Foundation.
        /// Pass in 0 if there is no first owner.</param>
        /// <param name="AFoundationOwner2Key">PartnerKey of the second owner of the Foundation
        /// Pass in 0 if there is no second owner.</param>
        /// <returns>True if the current user has access to the passed in Foundation,
        /// otherwise false.</returns>
        public static bool CheckFoundationSecurity(Int64 AFoundationOwner1Key, Int64 AFoundationOwner2Key)
        {
            Boolean ReturnValue;

            ReturnValue = false;

            if ((AFoundationOwner1Key == 0) && (AFoundationOwner2Key == 0))
            {
                TLogging.Log("CheckFoundationSecurity: None of the Owners is set.");

                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVUSER)
                    || (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN)))
                {
                    TLogging.Log("CheckFoundationSecurity: User is member of DEVUSER or DEVADMIN Module");
                    ReturnValue = true;
                }
            }
            else
            {
                TLogging.Log("CheckFoundationSecurity: One of the Owners is set!");

                if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_DEVADMIN))
                {
                    TLogging.Log("CheckFoundationSecurity: User is member of DEVADMIN Module");
                    ReturnValue = true;
                }
                else
                {
                    TLogging.Log("CheckFoundationSecurity: User is NOT member of DEVADMIN Module");

                    if ((UserInfo.GUserInfo.PetraIdentity.PartnerKey == AFoundationOwner1Key)
                        || (UserInfo.GUserInfo.PetraIdentity.PartnerKey == AFoundationOwner2Key))
                    {
                        TLogging.Log("CheckFoundationSecurity: User is Owner1 or Owner2");
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }
    }
}