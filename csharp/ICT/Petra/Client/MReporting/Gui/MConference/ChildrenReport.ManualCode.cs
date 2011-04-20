//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmChildrenReport.ManualCode.
    /// </summary>
    public partial class TFrmChildrenReport
    {
        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int MinAge = Convert.ToInt32(txtFromYears.Text);     // 0
            int MaxAge = Convert.ToInt32(txtToYears.Text);     // 17
            DateTime EarliestBirthday;
            DateTime LatestBirthday;

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
                EarliestBirthday = StartDate.AddYears(-MaxAge - 1);
                LatestBirthday = EndDate.AddYears(-MinAge);

                ACalc.AddParameter("param_earliest_birthday", EarliestBirthday.Date);
                ACalc.AddParameter("param_latest_birthday", LatestBirthday.Date);
            }
        }
    }
}