//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2021 by OM International
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
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;

namespace Ict.Petra.Server.MCommon.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of data.
    /// </summary>
    public static class TValidationHelper
    {
        /// <summary>
        /// checks if row has status "Added" or if field value is modified compared to original value
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AFieldDbName">db name of field to be checked</param>
        /// <returns></returns>
        public static bool IsRowAddedOrFieldModified(DataRow ARow, string AFieldDbName)
        {
            if (ARow.RowState == DataRowState.Added)
            {
                return true;
            }

            if ((ARow.RowState == DataRowState.Modified)
                && ARow.HasVersion(DataRowVersion.Original)
                && ((TTypedDataAccess.GetSafeValue(ARow, AFieldDbName, DataRowVersion.Original)).ToString()
                    != (TTypedDataAccess.GetSafeValue(ARow, AFieldDbName, DataRowVersion.Current)).ToString()))
            {
                return true;
            }

            return false;
        }
    }
}
