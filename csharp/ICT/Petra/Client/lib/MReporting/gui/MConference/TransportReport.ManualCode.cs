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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmTransportReport.ManualCode.
    /// </summary>
    public partial class TFrmTransportReport
    {
        private void rbtArrivalDepartureChanged(System.Object sender, EventArgs e)
        {
            if (rbtArrivals.Checked)
            {
                chkOnlyTravelDay.Text = Catalog.GetString("Only list people arriving on this day");
                chkNeedTransport.Text = Catalog.GetString("Only list people that need transport from their arrival point");
                chkIncompleteDetails.Text = Catalog.GetString("Only list people with incomplete arrival details");
            }

            if (rbtDepartures.Checked)
            {
                chkOnlyTravelDay.Text = Catalog.GetString("Only list people departing on this day");
                chkNeedTransport.Text = Catalog.GetString("Only list people that need transport to their departing point");
                chkIncompleteDetails.Text = Catalog.GetString("Only list people with incomplete departure details");
            }
        }

        private void chkTravelDayChanged(System.Object sender, EventArgs e)
        {
            dtpTravelDay.Enabled = chkOnlyTravelDay.Checked;
        }
    }
}