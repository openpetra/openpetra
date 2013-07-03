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
using Ict.Common;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Client.MConference.Gui.Setup;

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
            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PcEarlyLateTable.GetConferenceKeyDBName(), 1110198);
            TFrmEarlyLateRegistrationSetup frm = new TFrmEarlyLateRegistrationSetup(AParentForm, Search);
            
            frm.Show();
        }
                
        /// <summary>
        /// opens Standard Cost screen for pre selected conference
        /// </summary>
        public static void StandardCostsForSelectedConference(Form AParentForm)
        {
            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PcConferenceCostTable.GetConferenceKeyDBName(), 1110198);
            TFrmConferenceStandardCostSetup frm = new TFrmConferenceStandardCostSetup(AParentForm, Search);
            
            frm.Show();
        }
        
        /// <summary>
        /// opens Child Discount screen for pre selected conference
        /// </summary>
        public static void ChildDiscountsForSelectedConference(Form AParentForm)
        {
            TSearchCriteria[] Search = new TSearchCriteria[1];
            Search[0] = new TSearchCriteria(PcDiscountTable.GetConferenceKeyDBName(), 1110198);
            TFrmChildDiscountSetup frm = new TFrmChildDiscountSetup(AParentForm, Search);
            
            frm.Show();
        }
    }
}