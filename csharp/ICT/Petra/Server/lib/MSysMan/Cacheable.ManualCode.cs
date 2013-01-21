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
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared.MSysMan.Validation;

namespace Ict.Petra.Server.MSysMan.Cacheable
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the MSysMan sub-namespace
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
        public DataTable GetCacheableTable(TCacheableSysManTablesEnum ACacheableTable)
        {
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", false, out TmpType);
        }

        private DataTable GetUserListTable(TDBTransaction AReadTransaction, string ATableName)
        {
            DataColumn LastAndFirstNameColumn;
            DataTable TmpTable = SUserAccess.LoadAll(AReadTransaction);

            // add column to display a combination of last and first name
            LastAndFirstNameColumn = new DataColumn();
            LastAndFirstNameColumn.DataType = System.Type.GetType("System.String");
            LastAndFirstNameColumn.ColumnName = MSysManConstants.USER_LAST_AND_FIRST_NAME_COLUMNNAME;
            LastAndFirstNameColumn.Expression = string.Format("{0} + ' ' + {1}", SUserTable.GetFirstNameDBName(), SUserTable.GetLastNameDBName());
            TmpTable.Columns.Add(LastAndFirstNameColumn);

            return TmpTable;
        }
    }
}