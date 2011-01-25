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
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmArrivalListingReport.ManualCode.
    /// </summary>
    public partial class TFrmArrivalListingReport
    {
        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (dtpReportDate.ValidDate(true))
            {
                ACalc.AddParameter("param_dtpReportDate", this.dtpReportDate.Date);
            }

            if (rbtArrival.Checked)
            {
                ACalc.AddParameter("param_reportday", "Arrival");
            }
            else if (rbtDeparture.Checked)
            {
                ACalc.AddParameter("param_reportday", "Departure");
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            // param_reportday defines if the report is run on arrival or departuer days.
            if (AParameters.Get("param_reportday").ToString() == "Arrival")
            {
                rbtArrival.Checked = true;
            }
            else if (AParameters.Get("param_reportday").ToString() == "Departure")
            {
                rbtDeparture.Checked = true;
            }
        }
    }
}