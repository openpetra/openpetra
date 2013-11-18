//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for Partner Subscriptions.
    /// </summary>
    public class TUCPartnerSubscriptionsLogic
    {
        /// <summary>
        /// Checks if the Publication that is selected in a ComboBox is valid. If it isn't valid,
        /// a warning is shown to the user, asking the user as to whether the user still wants to use
        /// this Publication for the Subscription that the user is working with.
        /// </summary>
        /// <param name="APublicationComboBox">ComboBox containing Publications.</param>
        public static void CheckPublicationComboValidValue(TCmbAutoPopulated APublicationComboBox)
        {
            DataTable DataCachePublicationListTable;
            PPublicationRow TmpRow;
            string SelectedPublication;

            if (APublicationComboBox == null) 
            {
                throw new ArgumentNullException("APublicationComboBox must not be null");
            }
            
            SelectedPublication = APublicationComboBox.GetSelectedString();            

            if (!String.IsNullOrWhiteSpace(SelectedPublication))
            {
                DataCachePublicationListTable = TDataCache.TMPartner.GetCacheableSubscriptionsTable(TCacheableSubscriptionsTablesEnum.PublicationList);
                TmpRow = (PPublicationRow)DataCachePublicationListTable.Rows.Find(
                    new Object[] { SelectedPublication });
    
                if (TmpRow != null) 
                {
                    if (!TmpRow.ValidPublication)
                    {
                        if (MessageBox.Show(
                                Catalog.GetString(String.Format("Please note that Publication\r\n\r\n    {0}\r\n\r\n" +
                                        "is no longer available." + "\r\n\r\n" +
                                        "Would you still like to use it for a Subscription?",
                                        APublicationComboBox.GetSelectedString())),
                                Catalog.GetString("Publication Selection"),
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            APublicationComboBox.cmbCombobox.SelectedIndex = -1;
                        }
                    }                    
                }
            }
        }
    }
}