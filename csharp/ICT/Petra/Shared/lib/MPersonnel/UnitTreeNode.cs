//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Shared.MPersonnel
{
    /// <summary>
    /// This object supplies fields to / from a TreeNode, for the UnitHierarchy methods.
    /// </summary>
    [Serializable]
    public class UnitHierarchyNode
    {
        /// <summary>The Key for this Unit</summary>
        public Int64 MyUnitKey;
        /// <summary>This Unit's parent</summary>
        public Int64 ParentUnitKey;
        /// <summary>From PUnit.Name</summary>
        public String Description;
        /// <summary>From PUnit.TypeCode=>Description</summary>
        public string TypeCode;
        /// <summary>From PUnitType Attribute LIKE "%sticky%"</summary>
        public Boolean UnitIsSticky;
    }
}