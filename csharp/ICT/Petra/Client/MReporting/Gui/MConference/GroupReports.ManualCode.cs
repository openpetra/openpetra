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
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmGroupReports.ManualCode.
    /// </summary>
    public partial class TFrmGroupReports
    {
        private void GroupSelectionChanged(System.Object sender, EventArgs e)
        {
            // Disable radio button "List only attendees whose role is a participant and who have a group assigned to it"
            // when radio button "All Groups" is selected.
            rbtParticipantsIgnoreAll.Enabled = !rbtAllGroups.Checked;

            if (rbtAllGroups.Checked && rbtParticipantsIgnoreAll.Checked)
            {
                rbtParticipantsAll.Checked = true;
            }
        }

        private void tpgGroups_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (rbtAllGroups.Checked && rbtParticipantsIgnoreAll.Checked)
            {
                rbtParticipantsAll.Checked = true;
            }
        }
    }
}