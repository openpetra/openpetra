//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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

namespace Ict.Petra.Shared.MCommon.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of data.
    /// </summary>
    public static class TSharedValidationHelper
    {
        /// <summary>
        /// Delegate for invoking the simple data reader.
        /// </summary>
        public delegate bool TSharedGetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable);

        /// <summary>
        /// Reference to the Delegate for invoking the simple data reader.
        /// </summary>
        private static TSharedGetData FDelegateSharedGetData;

        /// <summary>
        /// This property is used to provide a function which invokes the simple data reader.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TSharedGetData SharedGetDataDelegate
        {
            get
            {
                return FDelegateSharedGetData;
            }

            set
            {
                FDelegateSharedGetData = value;
            }
        }

        /// <summary>
        /// simple data reader;
        /// checks for permissions of the current user;
        /// </summary>
        /// <param name="ATablename"></param>
        /// <param name="ASearchCriteria">a set of search criteria</param>
        /// <param name="AResultTable">returns typed datatable</param>
        /// <returns></returns>
        public static bool GetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable)
        {
            if (FDelegateSharedGetData != null)
            {
                return FDelegateSharedGetData(ATablename, ASearchCriteria, out AResultTable);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TSharedGetData' must be initialised before calling this Method");
            }
        }

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
