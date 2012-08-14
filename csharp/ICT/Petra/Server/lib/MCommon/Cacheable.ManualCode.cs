//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;

using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MCommon.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MCommon sub-namespace
    /// that can be cached on the Client side.
    /// </summary>
    public partial class TCacheable
    {
        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Wrapper for other GetCacheableTable method
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <returns>DataTable</returns>
        public DataTable GetCacheableTable(TCacheableCommonTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
        }

        /// <summary>
        /// Overload of <see cref="M:GetCacheableTable(TCacheableCommonTablesEnum, string, bool, out System.Type)" />. See description there.
        /// </summary>
        /// <remarks>Can be used with Delegate TGetCacheableDataTableFromCache.</remarks>
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>The specified Cacheable DataTable is returned if the string matches a Cacheable DataTable,
        /// otherwise <see cref="String.Empty" />.</returns>
        public DataTable GetCacheableTable(string ACacheableTable, out System.Type AType)
        {
            return GetCacheableTable((TCacheableCommonTablesEnum)Enum.Parse(typeof(TCacheableCommonTablesEnum), ACacheableTable),
                String.Empty, false, out AType);
        }
    }
}