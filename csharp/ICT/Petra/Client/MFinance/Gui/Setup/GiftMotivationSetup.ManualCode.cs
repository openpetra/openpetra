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
using System.Collections.Specialized;
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
                FLedgerNumber = value;

                FMainDS = TRemote.MFinance.Gift.WebConnectors.LoadMotivationDetails(FLedgerNumber);

                // to get an empty AMotivationDetailFee table, instead of null reference
                FMainDS.Merge(new GiftBatchTDS());

                TFinanceControls.InitialiseMotivationGroupList(ref cmbDetailMotivationGroupCode, FLedgerNumber, false);

                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);

                TFinanceControls.InitialiseFeesReceivableList(ref clbDetailFeesReceivable, FLedgerNumber);
                TFinanceControls.InitialiseFeesPayableList(ref clbDetailFeesPayable, FLedgerNumber);

                DataView myDataView = FMainDS.AMotivationDetail.DefaultView;
                myDataView.AllowNew = false;
                myDataView.Sort = String.Format("{0} ASC, {1} ASC",
                    AMotivationDetailFeeTable.GetMotivationGroupCodeDBName(),
                    AMotivationDetailFeeTable.GetMotivationDetailCodeDBName()
                    );
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdDetails.AutoSizeCells();

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";
            }
        }

        private void NewRowManual(ref AMotivationDetailRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            if (FMainDS.AMotivationGroup == null || FMainDS.AMotivationGroup.Rows.Count == 0)
            {
                MessageBox.Show(Catalog.GetString("You must define at least one Motivation Group."), Catalog.GetString("New Motivation Detail"), 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ARow.MotivationGroupCode = FMainDS.AMotivationGroup[0].MotivationGroupCode;

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

        private void ShowDetailsManual(AMotivationDetailRow ARow)
        {
            string FeesPayable = string.Empty;
            string FeesReceivable = string.Empty;

            if (ARow != null)
            {
                FMainDS.AMotivationDetailFee.DefaultView.RowFilter =
                    String.Format("{0}={1} and {2}='{3}' and {4}='{5}'",
                        AMotivationDetailFeeTable.GetLedgerNumberDBName(),
                        ARow.LedgerNumber,
                        AMotivationDetailFeeTable.GetMotivationGroupCodeDBName(),
                        ARow.MotivationGroupCode,
                        AMotivationDetailFeeTable.GetMotivationDetailCodeDBName(),
                        ARow.MotivationDetailCode);

                foreach (DataRowView rv in FMainDS.AMotivationDetailFee.DefaultView)
                {
                    AMotivationDetailFeeRow detailFeeRow = (AMotivationDetailFeeRow)rv.Row;

                    if (StringHelper.StrSplit(clbDetailFeesPayable.GetAllStringList(), ",").Contains(detailFeeRow.FeeCode))
                    {
                        FeesPayable = StringHelper.AddCSV(FeesPayable, detailFeeRow.FeeCode);
                    }
                    else
                    {
                        FeesReceivable = StringHelper.AddCSV(FeesReceivable, detailFeeRow.FeeCode);
                    }
                }
            }

            clbDetailFeesPayable.SetCheckedStringList(FeesPayable);
            clbDetailFeesReceivable.SetCheckedStringList(FeesReceivable);
        }

        private void GetDetailDataFromControlsManual(AMotivationDetailRow ARow)
        {
            StringCollection TickedFees = StringHelper.StrSplit(
                StringHelper.ConcatCSV(clbDetailFeesPayable.GetCheckedStringList(), clbDetailFeesReceivable.GetCheckedStringList()),
                ",");

            FMainDS.AMotivationDetailFee.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}='{3}' and {4}='{5}'",
                    AMotivationDetailFeeTable.GetLedgerNumberDBName(),
                    ARow.LedgerNumber,
                    AMotivationDetailFeeTable.GetMotivationGroupCodeDBName(),
                    ARow.MotivationGroupCode,
                    AMotivationDetailFeeTable.GetMotivationDetailCodeDBName(),
                    ARow.MotivationDetailCode);

            StringCollection ExistingFees = new StringCollection();

            foreach (DataRowView rv in FMainDS.AMotivationDetailFee.DefaultView)
            {
                AMotivationDetailFeeRow detailFeeRow = (AMotivationDetailFeeRow)rv.Row;

                if (!TickedFees.Contains(detailFeeRow.FeeCode))
                {
                    // delete existing fees that have been unticked
                    detailFeeRow.Delete();
                }
                else
                {
                    ExistingFees.Add(detailFeeRow.FeeCode);
                }
            }

            // add new fees
            foreach (string fee in TickedFees)
            {
                if (!ExistingFees.Contains(fee))
                {
                    AMotivationDetailFeeRow NewRow = FMainDS.AMotivationDetailFee.NewRowTyped();
                    NewRow.LedgerNumber = ARow.LedgerNumber;
                    NewRow.MotivationGroupCode = ARow.MotivationGroupCode;
                    NewRow.MotivationDetailCode = ARow.MotivationDetailCode;
                    NewRow.FeeCode = fee;
                    FMainDS.AMotivationDetailFee.Rows.Add(NewRow);
                }
            }
        }
    }
}