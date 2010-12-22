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

using Ict.Petra.Shared;

namespace Ict.Petra.Client.App.Gui
{
    /// <summary>
    /// Operation Mode of a Screen.
    /// </summary>
    public enum TScreenMode
    {
        /// <summary>Entering of new record(s).</summary>
        smNew,

        /// <summary>Editing of an existing record.</summary>
        smEdit,

        /// <summary>Editing of an existing record, no navigation to other records allowed.</summary>
        smEditCurrent,

        /// <summary>Read-only viewing of an existing record, no navigation to other records allowed.</summary>
        smInquireCurrent,

        /// <summary>Read-only viewing of existing records.</summary>
        smInquireAll,

        /// <summary>Read-only viewing of existing records and entering of new record(s).</summary>
        smNewInquireAll
    };

    /// <summary>Event Handler for a generic Data Change Event.</summary>
    public delegate void THookupDataChangeEventHandler(System.Object Sender, System.EventArgs e);


    /// <summary>
    /// Contains Types which can be used anywhere in the Petra Client. They are not
    /// specific for a certain Petra Module.
    /// </summary>
    public class Types
    {
        /// <summary>
        /// Data structure for Partner Key Data.
        /// </summary>
        public class TPartnerKeyData
        {
            private Int64 FPartnerKey;
            private String FPartnerShortName;
            private TPartnerClass FPartnerClass;

            /// <summary>PartnerKey of a Partner.</summary>
            public long PartnerKey
            {
                get
                {
                    return FPartnerKey;
                }
            }

            /// <summary>Short Name of a Partner.</summary>
            public string PartnerShortName
            {
                get
                {
                    return FPartnerShortName;
                }
            }

            /// <summary>Partner Class of a Partner.</summary>
            public TPartnerClass PartnerClass
            {
                get
                {
                    return FPartnerClass;
                }
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of a Partner.</param>
            /// <param name="APartnerShortName">ShortName of a Partner.</param>
            /// <param name="APartnerClass">Partner Class of a Partner.</param>
            public TPartnerKeyData(Int64 APartnerKey, string APartnerShortName, TPartnerClass APartnerClass)
            {
                FPartnerKey = APartnerKey;
                FPartnerShortName = APartnerShortName;
                FPartnerClass = APartnerClass;
            }
        }
    }
}