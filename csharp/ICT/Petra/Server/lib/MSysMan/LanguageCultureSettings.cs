//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.Verification;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;

namespace Ict.Petra.Server.MSysMan.Maintenance.WebConnectors
{
    /// <summary>
    /// allow the user to set the language and culture settings
    /// </summary>
    public class TMaintainLanguageSettingsWebConnector
    {
        /// <summary>
        /// this will set the UI language and the culture for the current user
        /// </summary>
        /// <param name="ALanguageCode"></param>
        /// <param name="ACultureCode"></param>
        [RequireModulePermission("NONE")]
        public static bool SetLanguageAndCulture(string ALanguageCode, string ACultureCode)
        {
            TLanguageCulture.SetLanguageAndCulture(ALanguageCode, ACultureCode);

            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_UILANGUAGE, ALanguageCode, false);
            TUserDefaults.SetDefault(MSysManConstants.USERDEFAULT_UICULTURE, ACultureCode, false);

            TVerificationResultCollection VerificationResult;
            TUserDefaults.SaveUserDefaultsFromServerSide(out VerificationResult);
            return true;
        }

        /// <summary>
        /// load the language and culture settings from the user defaults
        /// </summary>
        /// <returns>false if user has not selected any defaults</returns>
        [RequireModulePermission("NONE")]
        public static bool LoadLanguageAndCultureFromUserDefaults()
        {
            if (TUserDefaults.HasDefault(MSysManConstants.USERDEFAULT_UILANGUAGE))
            {
                string LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.USERDEFAULT_UILANGUAGE);
                string CultureCode = TUserDefaults.GetStringDefault(MSysManConstants.USERDEFAULT_UICULTURE);
                TLanguageCulture.SetLanguageAndCulture(LanguageCode, CultureCode);
                return true;
            }

            return false;
        }

        /// <summary>
        /// this will return the UI language and the culture for the current user
        /// </summary>
        /// <param name="ALanguageCode"></param>
        /// <param name="ACultureCode"></param>
        [RequireModulePermission("NONE")]
        public static bool GetLanguageAndCulture(ref string ALanguageCode, ref string ACultureCode)
        {
            ALanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.USERDEFAULT_UILANGUAGE, ALanguageCode);
            ACultureCode = TUserDefaults.GetStringDefault(MSysManConstants.USERDEFAULT_UICULTURE, ACultureCode);

            return true;
        }
    }
}