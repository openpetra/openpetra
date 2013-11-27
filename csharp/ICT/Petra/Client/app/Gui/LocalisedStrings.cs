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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Contains functions and procedures that return Localised Strings for the User Interface.
    /// </summary>
    public class LocalisedStrings
    {
        #region Resourcestrings

        /// <summary>This resourcestring needs to be public as it is referenced from outside this Class as well.</summary>
        public static readonly string StrCountyDefaultLabel = Catalog.GetString("County/St&ate:");

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ALabelText"></param>
        /// <param name="AName"></param>
        public static void GetLocStrCounty(out String ALabelText, out String AName)
        {
            String LocalisedCountyLabel;

            LocalisedCountyLabel = TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_LOCALISEDCOUNTYLABEL, String.Empty);

            if (LocalisedCountyLabel.Trim() != String.Empty)
            {
                if (!LocalisedCountyLabel.EndsWith(":"))
                {
                    LocalisedCountyLabel = LocalisedCountyLabel + ':';
                }

                ALabelText = LocalisedCountyLabel;
            }
            else
            {
                ALabelText = StrCountyDefaultLabel;
            }

            // Remove & and : from the LabelText to get the 'Name' of the field
            AName = ALabelText.Replace("&", "");
            AName = AName.Replace(":", "");
        }
    }
}