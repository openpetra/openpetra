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
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MConference.Data;

namespace Ict.Petra.Shared.MConference.Validation
{
    /// <summary>
    /// Contains functions for the validation of Cacheable DataTables.
    /// </summary>
    public static partial class TSharedValidation_CacheableDataTables
    {
        /// <summary>
        /// Validates the MPartner Marital Status screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        public static void ValidateConferenceCostType(object AContext, PcCostTypeRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection)
        {
            DataColumn ValidationColumn;
            TVerificationResult VerificationResult = null;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // 'UnassignableDate' must not be empty if the flag is set
            ValidationColumn = ARow.Table.Columns[PcCostTypeTable.ColumnUnassignableDateId];

            if (true)
            {
                if (ARow.UnassignableFlag)
                {
                    VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.UnassignableDate,
                        String.Empty, AVerificationResultCollection, true,
                        AContext, ValidationColumn);
                }

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
            }
        }
    }
}
