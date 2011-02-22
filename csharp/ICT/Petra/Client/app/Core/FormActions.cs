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

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// EventArgs for Form Actions
    /// </summary>
    public class ActionEventArgs
    {
        /// <summary>todoComment</summary>
        public bool Enabled;

        /// <summary>todoComment</summary>
        public string ActionName;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AActionName"></param>
        /// <param name="AEnabled"></param>
        public ActionEventArgs(string AActionName, bool AEnabled)
        {
            this.ActionName = AActionName;
            this.Enabled = AEnabled;
        }
    }
}