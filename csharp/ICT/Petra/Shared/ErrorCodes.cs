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

namespace Ict.Petra.Shared
{
    /// <summary>
    /// some error codes for Petra
    /// it is useful to have error codes in case the error messages are translated into local language
    /// </summary>
    public class ErrorCodes
    {
        /// <summary>Value is no longer assignable</summary>
        public const String PETRAERRORCODE_VALUEUNASSIGNABLE = "X_0035";

        /// <summary>UnitName change undone</summary>
        public const String PETRAERRORCODE_UNITNAMECHANGEUNDONE = "X_0044";

        /// <summary>Partner Status MERGED change undone</summary>
        public const String PETRAERRORCODE_PARTNERSTATUSMERGEDCHANGEUNDONE = "X_0045";

        /// <summary>Bank Bic/Swift Code invalid</summary>
        public const String PETRAERRORCODE_BANKBICSWIFTCODEINVALID = "X_0046";

        /// <summary>email Address invalid</summary>
        public const String PETRAERRORCODE_EMAILADDRESSINVALID = "X_0047";

        /// <summary>No permission to access DB Table</summary>
        public const String PETRAERRORCODE_NOPERMISSIONTOACCESSTABLE = "SM0002";

        /// <summary>No permission to access Petra Module</summary>
        public const String PETRAERRORCODE_NOPERMISSIONTOACCESSMODULE = "SM0013";

        /// <summary>No permission to access Petra Group</summary>
        public const String PETRAERRORCODE_NOPERMISSIONTOACCESSGROUP = "SM0014";

        /// <summary>Concurrent changes to data happened</summary>
        public const String PETRAERRORCODE_CONCURRENTCHANGES = "SM0036";
    }
}