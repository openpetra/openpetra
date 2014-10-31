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
using System.Data;

using Ict.Common;

namespace Ict.Petra.Server.MCommon
{
    /// <summary>
    /// Contains constants that are Petra Server specific and are shared between
    /// several Petra modules.
    /// </summary>
    public class MCommonConstants
    {
        /// <summary>Cacheable DataTables: Isolation Level used when reading them into memory</summary>
        public const System.Data.IsolationLevel CACHEABLEDT_ISOLATIONLEVEL = IsolationLevel.ReadCommitted;

        #region Importing

        /// <summary>'Import Information'</summary>
        public static readonly string StrImportInformation = Catalog.GetString("Import Information");

        /// <summary>'An exception occurred while parsing line {0}'</summary>
        public static readonly string StrExceptionWhileParsingLine = Catalog.GetString("An exception occurred while parsing line {0}");

        /// <summary>'Parsing error in Line {0}'</summary>
        public static readonly string StrParsingErrorInLine = Catalog.GetString("Parsing error in Line {0}");

        /// <summary>'Parsing error in line {0} - column '{1}''</summary>
        public static readonly string StrParsingErrorInLineColumn = Catalog.GetString("Parsing error in line {0} - column '{1}'");

        /// <summary>'Import information for Line {0}'</summary>
        public static readonly string StrImportInformationForLine = Catalog.GetString("Import information for Line {0}");

        /// <summary>'Import validation warning in Line {0}'</summary>
        public static readonly string StrImportValidationWarningInLine = Catalog.GetString("Import validation warning in Line {0}");

        /// <summary>'Import validation error in Line {0}'</summary>
        public static readonly string StrImportValidationErrorInLine = Catalog.GetString("Import validation error in Line {0}");

        /// <summary>'Validation error in line {0}'</summary>
        public static readonly string StrValidationErrorInLine = Catalog.GetString("Validation error in line {0}");

        #endregion
    }
}