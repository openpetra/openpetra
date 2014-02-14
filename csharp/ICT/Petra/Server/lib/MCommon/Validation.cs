//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2014 by OM International
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
using System.Windows.Forms;

using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MCommon.Validation;

namespace Ict.Petra.Server.MCommon.DataReader.WebConnectors
{
    public partial class TCommonDataReader
    {
        //
        // Put Methods for the validation of Common Module WebConnectors and DataReaders in this code file.
        //

        static partial void ValidateInternationalPostalTypeManual(ref TVerificationResultCollection AVerificationResult,
            TTypedDataTable ASubmitTable)
        {
            TValidationControlsDict ValidationControlsDict = new TValidationControlsDict();

            ValidationControlsDict.Add(ASubmitTable.Columns[(short)PInternationalPostalTypeTable.ColumnInternatPostalTypeCodeId],
                new TValidationControlsData(null, PInternationalPostalTypeTable.GetInternatPostalTypeCodeDBName()));
            ValidationControlsDict.Add(ASubmitTable.Columns[(short)PInternationalPostalTypeTable.ColumnDescriptionId],
                new TValidationControlsData(null, PInternationalPostalTypeTable.GetDescriptionDBName()));
            ValidationControlsDict.Add(ASubmitTable.Columns[(short)PInternationalPostalTypeTable.ColumnDeletableId],
                new TValidationControlsData(null, PInternationalPostalTypeTable.GetDeletableDBName()));

            for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
            {
                PInternationalPostalTypeValidation.Validate("TCommonDataReader.ValidateInternationalPostalTypeManual" +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    (PInternationalPostalTypeRow)ASubmitTable.Rows[Counter], ref AVerificationResult,
                    ValidationControlsDict);
            }
        }
    }
}