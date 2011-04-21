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
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>Event Handlers</summary>
    public delegate void THookupPartnerEditDataChangeEventHandler(System.Object Sender, THookupPartnerEditDataChangeEventArgs e);

    /// <summary>
    /// Event Arguments
    /// </summary>
    public class THookupPartnerEditDataChangeEventArgs : System.EventArgs
    {
        private TPartnerEditTabPageEnum FTabPage;

        /// <summary>
        /// todoComment
        /// </summary>
        public TPartnerEditTabPageEnum TabPage
        {
            get
            {
                return FTabPage;
            }

            set
            {
                FTabPage = value;
            }
        }


        /// <summary>
        /// constructor
        /// </summary>
        /// <returns>void</returns>
        public THookupPartnerEditDataChangeEventArgs() : base()
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATabPage"></param>
        public THookupPartnerEditDataChangeEventArgs(TPartnerEditTabPageEnum ATabPage) : base()
        {
            FTabPage = ATabPage;
        }
    }
}