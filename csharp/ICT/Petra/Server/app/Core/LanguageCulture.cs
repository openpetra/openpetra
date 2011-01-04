//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Threading;
using System.Globalization;
using Ict.Common;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// allow the user to set the language and culture settings.
    /// it seems between several calls via .net remoting, the Thread.CurrentThread object is always different, and certainly does not remember the CurrentCulture
    /// </summary>
    public class TLanguageCulture
    {
        private static string FLanguageCode = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
        private static string FCultureCode = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;

        /// <summary>
        /// initialise the translation and culture settings
        /// </summary>
        public static void Init()
        {
            // TODO load culture from config file etc;
            // cannot load from user defaults from Ict.Petra.Server.App.Core.
            // Loading from user defaults is done from the client, calling TMaintainLanguageSettingsWebConnector.LoadLanguageAndCultureFromUserDefaults

            SetLanguageAndCulture(Thread.CurrentThread.CurrentCulture.IetfLanguageTag,
                Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
        }

        /// <summary>
        /// this will set the UI language and the culture for the current user
        /// </summary>
        /// <param name="ALanguageCode"></param>
        /// <param name="ACultureCode"></param>
        /// <returns>true if the language file was available</returns>
        public static void SetLanguageAndCulture(string ALanguageCode, string ACultureCode)
        {
            FLanguageCode = ALanguageCode;
            FCultureCode = ACultureCode;
            LoadLanguageAndCulture();
        }

        /// <summary>
        /// for the current thread, make sure the correct culture and language settings are applied.
        /// should this be called for every call to a web connector?
        /// </summary>
        public static void LoadLanguageAndCulture()
        {
            // TODO: how to support organisation specific language files?
            // TODO another Catalog.Init("org", "./locale") for organisation specific words?

            Catalog.Init(FLanguageCode, FCultureCode);
        }
    }
}