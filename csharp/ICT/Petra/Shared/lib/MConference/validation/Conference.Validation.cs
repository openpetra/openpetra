//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MConference;

namespace Ict.Petra.Shared.MConference.Validation
{
    /// <summary>
    /// Contains functions for the validation of Conference DataTables.
    /// </summary>
    public static partial class TSharedConferenceValidation_Conference
    {
        /// <summary>
        /// Validates the MConference Standard Cost Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AGridData">A <see cref="TValidationControlsDict" />Contains all rows that are included in the grid</param>
        public static void ValidateConferenceStandardCost(object AContext, PcConferenceCostRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            DataRowCollection AGridData)
        {
            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            // Check the row being validated is consistent with the rest of the data in the table
            PcConferenceCostRow ARowCompare = null;
            Boolean StandardCostInconsistency = false;
            string[] InconsistentRows = new string[2];  // used for the error message
            int i = 0;

            while (i < AGridData.Count)
            {
                ARowCompare = (PcConferenceCostRow)AGridData[i];

                if ((ARowCompare.RowState != DataRowState.Deleted) && (ARowCompare.OptionDays > ARow.OptionDays) && (ARowCompare.Charge < ARow.Charge))
                {
                    StandardCostInconsistency = true;
                    InconsistentRows[0] = ARow.OptionDays.ToString();
                    InconsistentRows[1] = ARowCompare.OptionDays.ToString();
                    break;
                }
                else if ((ARowCompare.RowState != DataRowState.Deleted) && (ARowCompare.OptionDays < ARow.OptionDays)
                         && (ARowCompare.Charge > ARow.Charge))
                {
                    StandardCostInconsistency = true;
                    InconsistentRows[0] = ARowCompare.OptionDays.ToString();
                    InconsistentRows[1] = ARow.OptionDays.ToString();
                    break;
                }

                i++;
            }

            // if an inconsistency is found
            if (StandardCostInconsistency == true)
            {
                TValidationControlsData ValidationControlsData;
                TScreenVerificationResult VerificationResult = null;
                DataColumn ValidationColumn = ARow.Table.Columns[PcConferenceCostTable.ColumnChargeId];

                // displays a warning message (non-critical error)
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_STANDARD_COST_INCONSISTENCY, InconsistentRows)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Validates the MConference Standard Cost Setup screen data.
        /// </summary>
        /// <param name="AContext">Context that describes where the data validation failed.</param>
        /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
        /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
        /// data validation errors occur.</param>
        /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
        /// display data that is about to be validated.</param>
        /// <param name="AGridData">A <see cref="TValidationControlsDict" />Contains all rows that are included in the grid</param>
        /// <param name="AEndDate">The End date for the selected conference</param>
        public static void ValidateEarlyLateRegistration(object AContext, PcEarlyLateRow ARow,
            ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict,
            DataRowCollection AGridData, DateTime AEndDate)
        {
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult = null;
            DataColumn ValidationColumn;

            // Don't validate deleted DataRows
            if (ARow.RowState == DataRowState.Deleted)
            {
                return;
            }

            if (ARow.Applicable > AEndDate)
            {
                ValidationColumn = ARow.Table.Columns[PcEarlyLateTable.ColumnApplicableId];

                // displays an error message
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_APPLICABLE_DATE_AFTER_CONFERENCE_END_DATE)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }

            // Check the row being validated is consistent with the rest of the data in the table
            PcEarlyLateRow ARowCompare = null;
            Boolean ApplicableDateTooEarly = false;
            Boolean ApplicableDateTooLate = false;
            string[] ErrorMessageData = new string[3];  // used for the error message
            int i = 0;

            while (i < AGridData.Count)
            {
                ARowCompare = (PcEarlyLateRow)AGridData[i];

                if ((ARowCompare.RowState != DataRowState.Deleted) && (ARowCompare.Type == true)
                    && (ARow.Type == false) && (ARowCompare.Applicable > ARow.Applicable))
                {
                    ApplicableDateTooEarly = true;
                    break;
                }
                else if ((ARowCompare.RowState != DataRowState.Deleted) && (ARowCompare.Type == false)
                         && (ARow.Type == true) && (ARowCompare.Applicable < ARow.Applicable))
                {
                    ApplicableDateTooLate = true;
                    break;
                }

                i++;
            }

            // if an inconsistency is found
            if (ApplicableDateTooEarly == true)
            {
                ValidationColumn = ARow.Table.Columns[PcEarlyLateTable.ColumnApplicableId];

                // displays a warning message (non-critical error)
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_EARLY_APPLICABLE_DATE_LATER_THAN_LATE_APPLICABLE_DATE)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
            else if (ApplicableDateTooLate == true)
            {
                ValidationColumn = ARow.Table.Columns[PcEarlyLateTable.ColumnApplicableId];

                // displays a warning message (non-critical error)
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_LATE_APPLICABLE_DATE_EARLIER_THAN_EARLY_APPLICABLE_DATE)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
            }
        }
    }
}