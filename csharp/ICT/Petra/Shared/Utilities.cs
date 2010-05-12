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

namespace Ict.Petra.Shared
{
    /// Contains Utility functions for ADO.NET Data operations that are Petra specific.
    public class SharedDataUtilities
    {
        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <param name="AMaxColumn"></param>
        /// <param name="AExcludeLocation0"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT, Int32 AMaxColumn, Boolean AExcludeLocation0)
        {
            Int32 ReturnValue;
            Int32 MaxColumn;
            Int16 Counter;
            Int16 Counter2;
            Int16 ChangedDRColumns;

            #region Process Arguments

            if (AInspectDT == null)
            {
                throw new ArgumentException("AInspectDT must not be nil");
            }

            if (AMaxColumn != -1)
            {
                MaxColumn = AMaxColumn;
            }
            else
            {
                MaxColumn = AInspectDT.Columns.Count;
            }

            #endregion
            ReturnValue = 0;

            for (Counter = 0; Counter <= AInspectDT.Rows.Count - 1; Counter += 1)
            {
                ChangedDRColumns = 0;

                if (((AInspectDT.Rows[Counter].RowState == DataRowState.Modified)
                     || (AInspectDT.Rows[Counter].RowState == DataRowState.Added))
                    && ((AExcludeLocation0) && (Convert.ToInt32(AInspectDT.Rows[Counter]["p_location_key_i"]) != 0)))
                {
                    for (Counter2 = 0; Counter2 <= MaxColumn - 1; Counter2 += 1)
                    {
                        if ((AInspectDT.Rows[Counter].RowState == DataRowState.Added)
                            || (AInspectDT.Rows[Counter][Counter2,
                                                         DataRowVersion.Original] != AInspectDT.Rows[Counter][Counter2, DataRowVersion.Current]))
                        {
                            ChangedDRColumns++;
                            ReturnValue++;
                        }
                    }

                    if (ChangedDRColumns == 0)
                    {
                        // Make DataRow unchanged since the Original values don't differ
                        // from the Current values for any of the DataColumns!
                        // MessageBox.Show('Calling AcceptChanges on Row ' + Counter.ToString + '; Contents of first 2 columns: ' +
                        // AInspectDT.Rows[Counter][0].ToString + '; ' + AInspectDT.Rows[Counter][1].ToString);
                        AInspectDT.Rows[Counter].AcceptChanges();
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <param name="AMaxColumn"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT, Int32 AMaxColumn)
        {
            return AcceptChangesForUnmodifiedRows(AInspectDT, AMaxColumn, false);
        }

        /// <summary>
        /// make sure that unmodified rows are marked as accepted
        /// </summary>
        /// <param name="AInspectDT"></param>
        /// <returns></returns>
        public static Int32 AcceptChangesForUnmodifiedRows(DataTable AInspectDT)
        {
            return AcceptChangesForUnmodifiedRows(AInspectDT, -1, false);
        }
    }
}