//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Threading;
using System.Globalization;
using GNU.Gettext;

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
            // modify current locale for the given language
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ALanguageCode);

            catalog = new GettextResourceManager("OpenPetra");

            Thread.CurrentThread.CurrentCulture = new CultureInfo(ACultureCode);
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
//            return catalog.GetString(AEnglishMessage); @TODO Fix this (Catalog is coming up null)
            return AEnglishMessage;
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
            return catalog.GetPluralString(msgid, msgidPlural, n);
        }
    }
}