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
using System.Threading;
using System.Globalization;
using GNU.Gettext;
using System.IO;

namespace Ict.Common
{
    /// <summary>
    /// a helper class for Gnu gettext
    /// </summary>
    public class Catalog
    {
        static GettextResourceManager catalog = null;

        /// <summary>
        /// initialise the resource manager with the default language
        /// </summary>
        public static void Init()
        {
            // TODO load culture from config file etc

            Init(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag, Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
        }

        /// <summary>
        /// initialise the resource manager with the given language
        /// </summary>
        public static void Init(string ALanguageCode, string ACultureCode)
        {
            if (ALanguageCode.ToLower() == "en-en")
            {
                ALanguageCode = "en-GB";
            }

            if (ACultureCode.ToLower() == "en-en")
            {
                ACultureCode = "en-GB";
            }

            // modify current locale for the given language
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ALanguageCode);

            string ResourceDllFname = TAppSettingsManager.ApplicationDirectory +
                                      "\\" + Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName + "\\OpenPetra.resources.dll";

            if (File.Exists(ResourceDllFname))
            {
                catalog = new GettextResourceManager("OpenPetra");
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(ACultureCode);
        }

        /// <summary>
        /// set the given culture
        /// </summary>
        /// <param name="ACulture"></param>
        /// <returns>the previously set culture</returns>
        public static CultureInfo SetCulture(CultureInfo ACulture)
        {
            CultureInfo OrigCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = ACulture;

            return OrigCulture;
        }

        /// <summary>
        /// set the given culture
        /// </summary>
        /// <param name="ACulture"></param>
        /// <returns>the previously set culture</returns>
        public static CultureInfo SetCulture(string ACulture)
        {
            CultureInfo OrigCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(ACulture);
            }
            catch (Exception)
            {
                TLogging.Log("Ict.Common Catalog.SetCulture: invalid culture name: " + ACulture);
            }

            return OrigCulture;
        }

        /// <summary>
        /// set the new language
        /// </summary>
        public static void SetLanguage(string ALanguageCode)
        {
            // modify current locale for the given language
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ALanguageCode);
        }

        /// <summary>
        /// get the translated string
        /// </summary>
        public static string GetString(string AEnglishMessage)
        {
            if (catalog == null)
            {
                return AEnglishMessage;
            }

            string result = AEnglishMessage;

            try
            {
                result = catalog.GetString(AEnglishMessage);
            }
            catch (Exception e)
            {
                TLogging.Log("GetText: Catalog.GetString: problem for getting text for \"" + AEnglishMessage + "\"");
                TLogging.Log(e.ToString());
            }

            return result;
        }

        /// <summary>
        /// Returns the translation of <paramref name="msgid"/> and
        /// <paramref name="msgidPlural"/>, choosing the right plural form
        /// depending on the number <paramref name="n"/>.
        /// </summary>
        /// <param name="msgid">the key string to be translated, an ASCII
        ///                     string</param>
        /// <param name="msgidPlural">the English plural of <paramref name="msgid"/>,
        ///                           an ASCII string</param>
        /// <param name="n">the number, should be &gt;= 0</param>
        /// <returns>the translation, or <c>null</c> if none is found</returns>
        public static string GetPluralString(String msgid, String msgidPlural, long n)
        {
            if (catalog == null)
            {
                if (n > 1)
                {
                    return msgidPlural;
                }
                else
                {
                    return msgid;
                }
            }

            return catalog.GetPluralString(msgid, msgidPlural, n);
        }
    }
}