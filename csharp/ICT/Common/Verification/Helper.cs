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

using GNU.Gettext;

namespace Ict.Common.Verification
{
    /// <summary>
    /// Contains Helper Methods for data verification.
    /// </summary>
    public static class THelper
    {
        /// <summary>
        /// Returns a description for a value surrounded by single quotes, or the
        /// translated equivalent of the text 'Value' in case
        /// <paramref name="AValueDescription" /> is an empty string or null.
        /// </summary>
        /// <param name="AValueDescription">Description of a value.</param>
        /// <returns><paramref name="AValueDescription" /> surrounded by single
        /// quotes, or the translated equivalent of the text 'Value' in case
        /// <paramref name="AValueDescription" /> is an empty string.
        /// </returns>
        public static string NiceValueDescription(string AValueDescription)
        {
            string ReturnValue;

            if (AValueDescription != String.Empty)
            {
                if (AValueDescription.EndsWith(":"))
                {
                    // Remove trailing colon (':') which would come from a Control's Label .Text Property
                    ReturnValue = "'" + AValueDescription.Substring(0, AValueDescription.Length - 1) + "'";
                }
                else
                {
                    ReturnValue = "'" + AValueDescription + "'";
                }

                // Remove any ampersands ('&', signalling the keyboard shortcut in a Control's Label .Text Property)
                // and any double-occurrence of a single-quote (which could happen after we prepended and appended
                // single quotes)
                ReturnValue = ReturnValue.Replace("&", String.Empty).Replace("''", "'");;
            }
            else
            {
                ReturnValue = Catalog.GetString("Value");
            }

            return ReturnValue;
        }
    }
}