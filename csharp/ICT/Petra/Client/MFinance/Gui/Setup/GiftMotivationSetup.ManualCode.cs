//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup.Gift
{
    public partial class TFrmGiftMotivationSetup
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// maintain motivation details for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FFilter = value;
                FLedgerNumber = value;

                LoadDataAndFinishScreenSetup();
//                FMainDS = TRemote.MFinance.Gift.WebConnectors.LoadMotivationDetails(FLedgerNumber);
//
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);
//
//                DataView myDataView = FMainDS.AMotivationDetail.DefaultView;
//                myDataView.AllowNew = false;
//                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
//                grdDetails.AutoSizeCells();
            }
        }

        private object GetFilterCriteria()
        {
            return FLedgerNumber;
        }

        private void NewRowManual(ref AMotivationDetailRow ARow)
        {
            // TODO support more motivation groups; only supporting one motivation group at the moment
            ARow.LedgerNumber = FLedgerNumber;
            ARow.MotivationGroupCode = FMainDS.AMotivationDetail[0].MotivationGroupCode;

            string newName = Catalog.GetString("NEWDETAIL");
            Int32 countNewDetail = 0;

            if (FMainDS.AMotivationDetail.Rows.Find(new object[] { FLedgerNumber, ARow.MotivationGroupCode, newName }) != null)
            {
                while (FMainDS.AMotivationDetail.Rows.Find(new object[] { FLedgerNumber, ARow.MotivationGroupCode, newName +
                                                                          countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.MotivationDetailCode = newName;
        }

        private TSubmitChangesResult StoreManualCode(ref GiftBatchTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            return TRemote.MFinance.Gift.WebConnectors.SaveMotivationDetails(ref ASubmitChanges, out AVerificationResult);
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewAMotivationDetail();
        }
    }
}