//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
#region usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;

#endregion usings

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftTransactions
    {
        #region load donor

        private PPartnerRow RetrieveDonorRow(long APartnerKey)
        {
            if (APartnerKey == 0)
            {
                return null;
            }

            // find PPartnerRow from dataset
            PPartnerRow DonorRow = (PPartnerRow)FMainDS.DonorPartners.Rows.Find(new object[] { APartnerKey });

            // if PPartnerRow cannot be found, load it from db
            if ((DonorRow == null) || (DonorRow[PPartnerTable.GetReceiptEachGiftDBName()] == DBNull.Value))
            {
                PPartnerTable PartnerTable = TRemote.MFinance.Gift.WebConnectors.LoadPartnerData(APartnerKey);

                if ((PartnerTable == null) || (PartnerTable.Rows.Count == 0))
                {
                    // invalid partner
                    return null;
                }
                else
                {
                    FMainDS.DonorPartners.Merge(PartnerTable);

                    if (TSystemDefaults.GetBooleanDefault("GovIdEnabled", false))
                    {
                        PTaxTable taxTbl = TRemote.MFinance.Gift.WebConnectors.LoadPartnerPtax(APartnerKey);

                        if ((taxTbl != null) && (taxTbl.Rows.Count > 0))
                        {
                            FMainDS.PTax.Merge(taxTbl);
                        }
                    }
                }

                DonorRow = PartnerTable[0];
            }

            return DonorRow;
        }

        /// <summary>
        /// Update all donor names in gift details table
        /// </summary>
        /// <param name="ABatchNumber"></param>
        private void UpdateAllDonorNames(Int32 ABatchNumber)
        {
            Dictionary <Int32, Int64>GiftsDict = new Dictionary <Int32, Int64>();
            Dictionary <Int64, string>DonorsDict = new Dictionary <Int64, string>();

            DataView GiftDV = new DataView(FMainDS.AGift);

            GiftDV.RowFilter = string.Format("{0}={1}",
                AGiftTable.GetBatchNumberDBName(),
                ABatchNumber);

            GiftDV.Sort = string.Format("{0} ASC", AGiftTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView drv in GiftDV)
            {
                AGiftRow gr = (AGiftRow)drv.Row;

                Int64 donorKey = gr.DonorKey;

                GiftsDict.Add(gr.GiftTransactionNumber, donorKey);

                if (!DonorsDict.ContainsKey(donorKey))
                {
                    if (donorKey != 0)
                    {
                        PPartnerRow pr = RetrieveDonorRow(donorKey);

                        if (pr != null)
                        {
                            DonorsDict.Add(donorKey, pr.PartnerShortName);
                        }
                    }
                    else
                    {
                        DonorsDict.Add(0, "");
                    }
                }
            }

            //Add donor info to gift details
            DataView GiftDetailDV = new DataView(FMainDS.AGiftDetail);

            GiftDetailDV.RowFilter = string.Format("{0}={1}",
                AGiftDetailTable.GetBatchNumberDBName(),
                ABatchNumber);

            GiftDetailDV.Sort = string.Format("{0} ASC", AGiftDetailTable.GetGiftTransactionNumberDBName());

            foreach (DataRowView drv in GiftDetailDV)
            {
                GiftBatchTDSAGiftDetailRow giftDetail = (GiftBatchTDSAGiftDetailRow)drv.Row;

                Int64 donorKey = GiftsDict[giftDetail.GiftTransactionNumber];

                giftDetail.DonorKey = donorKey;
                giftDetail.DonorName = DonorsDict[donorKey];
            }
        }

        private bool DonorIsAlreadyLoaded(Int64 ADonorKey, Int32 AGiftNumber)
        {
            //Check for Donor in loaded gift batches
            DataView giftDV = new DataView(FMainDS.AGift);

            giftDV.RowFilter = string.Format("{0}={1} And Not ({2}={3} And {4}={5})",
                AGiftTable.GetDonorKeyDBName(),
                ADonorKey,
                AGiftTable.GetBatchNumberDBName(),
                FBatchNumber,
                AGiftTable.GetGiftTransactionNumberDBName(),
                AGiftNumber);

            return giftDV != null && giftDV.Count > 0;
        }

        #endregion load donor

        #region donor changed

        private void DonorKeyChanged(Int64 APartnerKey,
            String APartnerShortName,
            bool AValidSelection)
        {
            if (!FAutoPopulatingGiftInProcess)
            {
                //This gets set to false when autopopulating gift
                txtDetailDonorKey.FocusTextBoxPartAfterFindScreenCloses = true;
            }

            if (FAutoPopulatingGiftInProcess)
            {
                return;
            }
            else if (!AValidSelection && (APartnerKey != 0))
            {
                //An invalid donor number can stop deletion of a new row, so need to stop invalid entries
                MessageBox.Show(String.Format(Catalog.GetString("Donor number {0} could not be found!"),
                        APartnerKey));
                txtDetailDonorKey.Text = String.Format("{0:0000000000}", 0);
                return;
            }
            // At the moment this event is thrown twice
            // We want to deal only on manual entered changes, i.e. not on selections changes, and on non-zero keys
            else if (FPetraUtilsObject.SuppressChangeDetection || (APartnerKey == 0))
            {
                // FLastDonor should be the last donor key that has been entered for a gift (not 0)
                if (APartnerKey != 0)
                {
                    FLastDonor = APartnerKey;
                    mniDonorHistory.Enabled = true;
                }
                else
                {
                    mniDonorHistory.Enabled = false;
                    txtDonorInfo.Text = string.Empty;

                    if (FNewGiftInProcess)
                    {
                        FLastDonor = 0;
                    }
                }
            }
            else
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    // this is a different donor
                    if (APartnerKey != FLastDonor)
                    {
                        PPartnerRow pr = RetrieveDonorRow(APartnerKey);

                        if (pr == null)
                        {
                            string errMsg = String.Format(Catalog.GetString("Partner Key:'{0} - {1}' cannot be found in the Partner table!"),
                                APartnerKey,
                                APartnerShortName);
                            MessageBox.Show(errMsg, Catalog.GetString("Donor Changed"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //An invalid donor number can stop deletion of a new row, so need to stop invalid entries
                            txtDetailDonorKey.Text = String.Format("{0:0000000000}", 0);
                            return;
                        }

                        chkDetailConfidentialGiftFlag.Checked = pr.AnonymousDonor;

                        Int32 giftTransactionNo = FPreviouslySelectedDetailRow.GiftTransactionNumber;

                        DataView giftDetailDV = new DataView(FMainDS.AGiftDetail);

                        giftDetailDV.RowFilter = string.Format("{0}={1} And {2}={3}",
                            AGiftDetailTable.GetBatchNumberDBName(),
                            FBatchNumber,
                            AGiftDetailTable.GetGiftTransactionNumberDBName(),
                            giftTransactionNo);

                        foreach (DataRowView drv in giftDetailDV)
                        {
                            GiftBatchTDSAGiftDetailRow giftDetail = (GiftBatchTDSAGiftDetailRow)drv.Row;

                            giftDetail.DonorKey = APartnerKey;
                            giftDetail.DonorName = APartnerShortName;
                            giftDetail.DonorClass = pr.PartnerClass;
                        }

                        //Point to current gift row and specify as not a new donor
                        FGift = GetGiftRow(giftTransactionNo);
                        FGift.FirstTimeGift = false;

                        //Only autopopulate if this is a donor selection on a clean gift,
                        //  i.e. determine this is not a donor change where other changes have been made
                        //Sometimes you want to just change the donor without changing what already has been entered
                        //  e.g. when you realise you have entered the wrong donor after entering the correct recipient data
                        if (!DonorIsAlreadyLoaded(APartnerKey, giftTransactionNo)
                            && !TRemote.MFinance.Gift.WebConnectors.DonorHasGiven(FLedgerNumber, APartnerKey))
                        {
                            FGift.FirstTimeGift = true;

                            // add donor key to list so that new donor warning can be shown
                            if (!FNewDonorsList.Contains(APartnerKey))
                            {
                                FNewDonorsList.Add(APartnerKey);
                            }
                        }
                        else if ((giftDetailDV.Count == 1)
                                 && (Convert.ToInt64(txtDetailRecipientKey.Text) == 0)
                                 && (txtDetailGiftTransactionAmount.NumberValueDecimal.Value == 0))
                        {
                            AutoPopulateGiftDetail(APartnerKey, APartnerShortName, giftTransactionNo);
                        }

                        mniDonorHistory.Enabled = true;
                    }

                    ShowDonorInfo(APartnerKey);

                    FLastDonor = APartnerKey;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        #endregion donor changed
    }
}