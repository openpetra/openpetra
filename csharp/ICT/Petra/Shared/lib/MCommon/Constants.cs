//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// this class defines some data types that can be used for Office Specific Data Labels
    /// </summary>
    public class MCommonConstants
    {
        /// <summary>string</summary>
        public const String OFFICESPECIFIC_DATATYPE_CHAR = "char";

        /// <summary>decimal numbers</summary>
        public const String OFFICESPECIFIC_DATATYPE_FLOAT = "float";

        /// <summary>date</summary>
        public const String OFFICESPECIFIC_DATATYPE_DATE = "date";

        /// <summary>integer</summary>
        public const String OFFICESPECIFIC_DATATYPE_INTEGER = "integer";

        /// <summary>currency values</summary>
        public const String OFFICESPECIFIC_DATATYPE_CURRENCY = "currency";

        /// <summary>logical</summary>
        public const String OFFICESPECIFIC_DATATYPE_BOOLEAN = "boolean";

        /// <summary>partner key</summary>
        public const String OFFICESPECIFIC_DATATYPE_PARTNERKEY = "partnerkey";

        /// <summary>lookup, refering to another table</summary>
        public const String OFFICESPECIFIC_DATATYPE_LOOKUP = "lookup";
    }
}