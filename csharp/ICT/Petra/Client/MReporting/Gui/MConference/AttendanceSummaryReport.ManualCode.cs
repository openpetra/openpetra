//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2011 by OM International
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmAttendanceSummaryReport.ManualCode.
    /// </summary>
    public partial class TFrmAttendanceSummaryReport
    {
        private void InitUserControlsManually()
        {
            ucoConferenceSelection.AddConfernceKeyChangedEventHandler(this.ConferenceKeyChanged);
            ucoConferenceSelection.AddConferenceSelectionChangedEventHandler(this.ConferenceSelectionChanged);

            dtpConferenceStartDate.Enabled = false;
            dtpConferenceEndDate.Enabled = false;
            dtpEarliestArrivalDate.Enabled = false;
            dtpLatestDepartureDate.Enabled = false;
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int ColumnCounter = 0;
            bool DatesAreValid = (dtpFromDate.ValidDate(false) && dtpToDate.ValidDate(false));

            // Add the columns to the report
            ACalc.AddParameter("param_calculation", "Date", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", (float)2.5, ColumnCounter);
            ColumnCounter++;
            ACalc.AddParameter("param_calculation", "Total", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "3", ColumnCounter);
            ColumnCounter++;
            ACalc.AddParameter("param_calculation", "Actual", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "2", ColumnCounter);
            ColumnCounter++;
            ACalc.AddParameter("param_calculation", "Expected", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "2", ColumnCounter);
            ColumnCounter++;
            ACalc.AddParameter("param_calculation", "Children", ColumnCounter);
            ACalc.AddParameter("ColumnWidth", "2", ColumnCounter);
            ColumnCounter++;

            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);

            if ((AReportAction == TReportActionEnum.raGenerate) && DatesAreValid)
            {
                if (dtpFromDate.Date > dtpToDate.Date)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Change From-Date or To-Date"),
                        Catalog.GetString("From-Date must be earlier than To-Date"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            if (!DatesAreValid)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Enter valid From-Date and To-Date."),
                    Catalog.GetString("Dates must be valid"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void SetDateRange()
        {
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;
            DateTime EarliestArrivalDate = DateTime.Today;
            DateTime LatestDepartureDate = DateTime.Today;
            bool IsSuccessful;

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                IsSuccessful = TRemote.MConference.WebConnectors.GetEarliestAndLatestDate(-1,
                    out EarliestArrivalDate, out LatestDepartureDate, out StartDate, out EndDate);
            }
            else
            {
                long ConferenceKey = Convert.ToInt64(ucoConferenceSelection.ConferenceKey);

                IsSuccessful = TRemote.MConference.WebConnectors.GetEarliestAndLatestDate(ConferenceKey,
                    out EarliestArrivalDate, out LatestDepartureDate, out StartDate, out EndDate);
            }

            if (IsSuccessful)
            {
                dtpConferenceStartDate.Date = StartDate;
                dtpConferenceEndDate.Date = EndDate;
                dtpEarliestArrivalDate.Date = EarliestArrivalDate;
                dtpLatestDepartureDate.Date = LatestDepartureDate;
                dtpFromDate.Date = EarliestArrivalDate;
                dtpToDate.Date = LatestDepartureDate;
            }
        }

        /// <summary>
        /// Event called when the text of "select conference button" has changed.
        /// Updates the Dates of the conference.
        /// </summary>
        /// <param name="AConferenceKey">Unit key of the conference</param>
        /// <param name="AConferenceName">Name of the conference</param>
        /// <param name="AValidConference">True if we have a valid conference. Otherwise false.</param>
        public void ConferenceKeyChanged(Int64 AConferenceKey, String AConferenceName, bool AValidConference)
        {
            if (AValidConference)
            {
                SetDateRange();
            }
        }

        /// <summary>
        /// Event called when the conference selection has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConferenceSelectionChanged(System.Object sender, EventArgs e)
        {
            SetDateRange();
        }
    }
}