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
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Shared.MCommon.Validation
{
    /// <summary>
    /// Contains functions for the validation of MCommon DataTables.
    /// </summary>
    public static class TSharedCommonValidation
    {
        /// <summary>
        /// Checks whether an International Postal Type is valid. Null values are accepted.
        /// </summary>
        /// <param name="AInternatPostalTypeCode">The International Postal Type to check.</param>
        /// <param name="ADescription">Description what the value is about (for the
        /// error message).</param>
        /// <param name="AResultContext">Context of verification (can be null).</param>
        /// <param name="AResultColumn">Which <see cref="System.Data.DataColumn" /> failed (can be null).</param>
        /// <returns>Null if <paramref name="AInternatPostalTypeCode" /> is null,
        /// otherwise a <see cref="TVerificationResult" /> is returned that
        /// contains details about the problem, with a message that uses <paramref name="ADescription" />.</returns>
        public static TVerificationResult IsValidInternationalPostalCode(string AInternatPostalTypeCode,
            string ADescription = "", object AResultContext = null, System.Data.DataColumn AResultColumn = null)
        {
            TVerificationResult ReturnValue = null;

            Ict.Common.Data.TTypedDataTable IntPostalDT;

            if (AInternatPostalTypeCode != null)
            {
                if (AInternatPostalTypeCode != String.Empty)
                {
                    TSharedValidationHelper.GetData(PInternationalPostalTypeTable.GetTableDBName(), null, out IntPostalDT);

                    if (IntPostalDT.Rows.Find(new object[] { AInternatPostalTypeCode }) == null)
                    {
                        ReturnValue = new TVerificationResult(AResultContext,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALIDINTERNATIONALPOSTALCODE));

                        if (AResultColumn != null)
                        {
                            ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn);
                        }
                    }
                }
                else
                {
                    ReturnValue = new TVerificationResult(AResultContext,
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_INVALIDINTERNATIONALPOSTALCODE));

                    if (AResultColumn != null)
                    {
                        ReturnValue = new TScreenVerificationResult(ReturnValue, AResultColumn);
                    }
                }
            }

            return ReturnValue;
        }
    }
}
