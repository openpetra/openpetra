//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.MFinance.Gui.Setup.Gift
{
    public partial class TFrmMotivationGroupSetup
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// maintain motivation details for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                FMainDS = TRemote.MFinance.Gift.WebConnectors.LoadMotivationDetails(FLedgerNumber);

                FMainDS.Merge(new GiftBatchTDS());

                DataView myDataView = FMainDS.AMotivationGroup.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdDetails.AutoSizeCells();

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";

                SelectRowInGrid(1);
                UpdateRecordNumberDisplay();
            }
        }

        private void NewRowManual(ref AMotivationGroupRow ARow)
        {
            // Deal with primary key. MotivationGroupCode is unique and is 8 characters.
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.AMotivationGroup.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AMotivationGroup.Rows.Find(new object[] { FLedgerNumber, newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.MotivationGroupCode = newName;
            ARow.LedgerNumber = FLedgerNumber;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewAMotivationGroup();
        }

        private TSubmitChangesResult StoreManualCode(ref GiftBatchTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            
            TSubmitChangesResult result = TRemote.MFinance.Gift.WebConnectors.SaveMotivationDetails(ref ASubmitChanges);

            if (result == TSubmitChangesResult.scrOK)
            {
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationGroupList, FLedgerNumber);
            }

            return result;
        }
    }
}