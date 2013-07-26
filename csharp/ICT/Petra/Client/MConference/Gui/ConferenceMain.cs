//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
using System.Runtime.Serialization;
using System;
using Ict.Common;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Client.MConference.Gui.Setup;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MConference.Gui
{
    /// <summary>
    /// Elements
    /// </summary>
    public class TConferenceMain
    {
        /// <summary>
        /// opens Early Late Registration screen for pre selected conference
        /// </summary>
        public static void EarlyLateRegistrationsForSelectedConference(Form AParentForm)
        {
            long ConferenceKey = Convert.ToInt64(AParentForm.GetType().GetMethod("GetSelectedConferenceKey").Invoke(AParentForm, null));

            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PcEarlyLateTable.GetConferenceKeyDBName(), ConferenceKey);
            TFrmEarlyLateRegistrationSetup frm = new TFrmEarlyLateRegistrationSetup(AParentForm, Search, ConferenceKey);

            frm.Show();
        }

        /// <summary>
        /// opens Standard Cost screen for pre selected conference
        /// </summary>
        public static void StandardCostsForSelectedConference(Form AParentForm)
        {
            long ConferenceKey = Convert.ToInt64(AParentForm.GetType().GetMethod("GetSelectedConferenceKey").Invoke(AParentForm, null));

            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PcConferenceCostTable.GetConferenceKeyDBName(), ConferenceKey);
            TFrmConferenceStandardCostSetup frm = new TFrmConferenceStandardCostSetup(AParentForm, Search, ConferenceKey);

            frm.Show();
        }

        /// <summary>
        /// opens Child Discount screen for pre selected conference
        /// </summary>
        public static void ChildDiscountsForSelectedConference(Form AParentForm)
        {
            long ConferenceKey = Convert.ToInt64(AParentForm.GetType().GetMethod("GetSelectedConferenceKey").Invoke(AParentForm, null));

            TSearchCriteria[] Search = new TSearchCriteria[2];
            Search[0] = new TSearchCriteria(PcDiscountTable.GetConferenceKeyDBName(), ConferenceKey);
            Search[1] = new TSearchCriteria(PcDiscountTable.GetDiscountCriteriaCodeDBName(), "CHILD");
            TFrmChildDiscountSetup frm = new TFrmChildDiscountSetup(AParentForm, Search, ConferenceKey);

            frm.Show();
        }

        /// <summary>
        /// opens Event Find Screen screen and uses Conference Find Form (not displayed) to create a new conference
        /// </summary>
        public static void NewConference(Form AParentForm)
        {
            TFrmConferenceFindForm frm = new TFrmConferenceFindForm(AParentForm);

            frm.NewConference(new object(), new EventArgs());
        }
    }
}