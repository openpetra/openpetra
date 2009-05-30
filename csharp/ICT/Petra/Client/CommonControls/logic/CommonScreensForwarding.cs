/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;

namespace Ict.Petra.Client.CommonControls.Logic
{
    /// <summary>
    /// Allows calls to Screens that are defined in various Assemblies
    /// from within various Assemblies. This resolves the problem of
    /// circular references between Assemblies that would come up with
    /// conventional calls to Screens.
    /// </summary>
    public static class TCommonScreensForwarding
    {
        static TDelegateOpenPartnerFindScreen FOpenPartnerFindScreen;

        /// <summary>
        /// This property is used to provide a function which opens a modal Partner Find screen.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TDelegateOpenPartnerFindScreen OpenPartnerFindScreen
        {
            get
            {
                return FOpenPartnerFindScreen;
            }

            set
            {
                FOpenPartnerFindScreen = value;
            }
        }
    }
}