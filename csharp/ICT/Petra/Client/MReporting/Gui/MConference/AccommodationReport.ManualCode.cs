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
using System.Drawing;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.MReporting;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// manual code for TFrmAccommodationReport class
    /// </summary>
    public partial class TFrmAccommodationReport
    {
        private void InitUserControlsManual()
        {
            ucoConferenceSelection.AddConfernceKeyChangedEventHandler(this.ConferenceChanged);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int ColumnCounter = 0;
            bool DatesAreValid = (dtpFromDate.ValidDate(false) && dtpToDate.ValidDate(false));

            ACalc.AddParameter("param_calculation", "Room", ColumnCounter++);

            if (DatesAreValid)
            {
                TimeSpan CheckLength = dtpToDate.Date.Value.Subtract(dtpFromDate.Date.Value);

                // Add the days as columns
                for (int Counter = 0; Counter <= CheckLength.Days; ++Counter)
                {
                    DateTime currentDate = dtpFromDate.Date.Value.AddDays(Counter);

                    ACalc.AddParameter("param_calculation", "CheckDate", ColumnCounter);
                    ACalc.AddParameter("ColumnWidth", (float)1.3, ColumnCounter);
                    ACalc.AddParameter("ColumnCaption", currentDate.ToString("MMM-d"), ColumnCounter);

                    ColumnCounter++;
                }
            }

            if (!rbtDetailReport.Checked)
            {
                // Don't show the cost column if we have a detailed report
                ACalc.AddParameter("param_calculation", "Cost", ColumnCounter);
                ACalc.AddParameter("ColumnWidth", "2.0", ColumnCounter++);
            }

            ACalc.AddParameter("MaxDisplayColumns", ColumnCounter);

            if (rbtBriefReport.Checked)
            {
                ACalc.AddParameter("param_report_detail", "Brief");
            }
            else if (rbtFullReport.Checked)
            {
                ACalc.AddParameter("param_report_detail", "Full");
            }
            else
            {
                ACalc.AddParameter("param_report_detail", "Detail");
            }

            if ((AReportAction == TReportActionEnum.raGenerate) && DatesAreValid)
            {
                if (dtpFromDate.Date > dtpToDate.Date)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        "Change From-Date or To-Date",
                        "From Date must be earlier than To Date",
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }

            if (!DatesAreValid)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    "Enter valid From-Date and To-Date.",
                    "Dates must be valid",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            SetDateRange();

            rbtDetailReport.Checked = (AParameters.Get("param_report_detail").ToString() == "Detail");
            rbtFullReport.Checked = (AParameters.Get("param_report_detail").ToString() == "Full");
            rbtBriefReport.Checked = (AParameters.Get("param_report_detail").ToString() == "Brief");
        }

        private void SetDateRange()
        {
            SelectConferenceTDS SelectedConferences = TRemote.MConference.WebConnectors.GetConferences("", "");

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                SetDateRangeForAllConferences(SelectedConferences);
            }
            else if (ucoConferenceSelection.OneConferenceSelected)
            {
                foreach (PcConferenceRow Row in SelectedConferences.PcConference.Rows)
                {
                    String PartnerKey = Row.ConferenceKey.ToString();
                    int Diff = 10 - PartnerKey.Length;

                    if (Diff > 0)
                    {
                        PartnerKey = new String('0', Diff) + PartnerKey;
                    }

                    if (PartnerKey == ucoConferenceSelection.ConferenceKey)
                    {
                        if (!Row.IsStartNull())
                        {
                            dtpStartDate.Date = Row.Start.Value;
                        }

                        if (!Row.IsEndNull())
                        {
                            dtpEndDate.Date = Row.End.Value;
                        }

                        break;
                    }
                }
            }
        }

        private void SetDateRangeForAllConferences(SelectConferenceTDS ASelectedConferences)
        {
            DateTime EarliestDate = DateTime.MaxValue;
            DateTime LatestDate = DateTime.MinValue;

            foreach (PcConferenceRow Row in ASelectedConferences.PcConference.Rows)
            {
                if (Row.Start.Value.CompareTo(EarliestDate) < 0)
                {
                    EarliestDate = Row.Start.Value;
                }

                if (Row.End.Value.CompareTo(LatestDate) > 0)
                {
                    LatestDate = Row.End.Value;
                }
            }

            dtpStartDate.Date = EarliestDate;

            dtpEndDate.Date = LatestDate;
        }

        /// <summary>
        /// Event called when the text of "select conference button" has changed.
        /// Updates the Dates of the conference.
        /// </summary>
        /// <param name="AConferenceKey">Unit key of the conference</param>
        /// <param name="AConferenceName">Name of the conference</param>
        /// <param name="AValidConference">True if we have a valid conference. Otherwise false.</param>
        public void ConferenceChanged(Int64 AConferenceKey, String AConferenceName, bool AValidConference)
        {
            DateTime EarliestArrivalDate = DateTime.Today;
            DateTime LatestDepartureDate = DateTime.Today;
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            if (AValidConference)
            {
                TRemote.MConference.WebConnectors.GetEarliestAndLatestDate(AConferenceKey, out EarliestArrivalDate, out LatestDepartureDate,
                    out StartDate, out EndDate);
                SetDateRange();
            }

            dtpEarliestArrival.Date = EarliestArrivalDate;
            dtpLatestDeparture.Date = LatestDepartureDate;
            dtpFromDate.Date = EarliestArrivalDate;
            dtpToDate.Date = LatestDepartureDate;
            dtpStartDate.Date = StartDate;
            dtpEndDate.Date = EndDate;
        }
    }
}