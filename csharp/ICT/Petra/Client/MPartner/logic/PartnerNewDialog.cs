//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MPartner
{
    /// <summary>
    /// Contains logic for the Partner New Dialog.
    /// </summary>
    public class TPartnerNewDialogScreenLogic
    {
        private Int64 FSiteKey;

        /// <summary>todoComment</summary>
        public Int64 SiteKey
        {
            get
            {
                return FSiteKey;
            }

            set
            {
                FSiteKey = value;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <param name="ASourceTable"></param>
        public void CreateColumns(TSgrdDataGrid AGrid, DataTable ASourceTable)
        {
            AGrid.AddPartnerKeyColumn("Site Key", ASourceTable.Columns[PPartnerLedgerTable.GetPartnerKeyDBName()], 75);
            AGrid.AddTextColumn("Site Name", ASourceTable.Columns[PPartnerTable.GetPartnerShortNameDBName()], 353);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGrid"></param>
        /// <returns></returns>
        public Int64 DetermineCurrentSitePartnerKey(TSgrdDataGrid AGrid)
        {
            DataRowView[] TheDataRowViewArray;
            Int64 SitePartnerKey;

            // MessageBox.Show(ARow.ToString);
            TheDataRowViewArray = AGrid.SelectedDataRowsAsDataRowView;

            // get PartnerKey of current DataRow
            try
            {
                SitePartnerKey = Convert.ToInt64(TheDataRowViewArray[0].Row[PPartnerLedgerTable.GetPartnerKeyDBName()]);
            }
            catch (Exception)
            {
                throw;
            }

            // MessageBox.Show(FPartnerKey.ToString);
            FSiteKey = SitePartnerKey;
            return SitePartnerKey;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AGridDataView"></param>
        /// <param name="APreselectedSiteKey"></param>
        /// <param name="ARowNumber"></param>
        /// <param name="ASiteKey"></param>
        public void DetermineInitiallySelectedSite(DataView AGridDataView, Int64 APreselectedSiteKey, out Int32 ARowNumber, out Int64 ASiteKey)
        {
            System.Int16 CurrentRow;

            if ((APreselectedSiteKey == 0) || (APreselectedSiteKey == -1))
            {
                ASiteKey = TSystemDefaults.GetSiteKeyDefault();
            }
            else
            {
                ASiteKey = APreselectedSiteKey;
            }

            FSiteKey = ASiteKey;
            ARowNumber = 0;

            for (CurrentRow = 0; CurrentRow <= AGridDataView.Count - 1; CurrentRow += 1)
            {
                ARowNumber = ARowNumber + 1;

                if (Convert.ToInt64(AGridDataView[CurrentRow].Row[PPartnerLedgerTable.GetPartnerKeyDBName()]) == ASiteKey)
                {
                    break;
                }
            }
        }
    }
}