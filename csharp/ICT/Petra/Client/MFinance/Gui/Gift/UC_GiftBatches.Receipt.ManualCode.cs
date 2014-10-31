//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
//
// Copyright 2004-2014 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Printing;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles Gift Receipting
    /// </summary>
    public class TUC_GiftBatches_Receipt
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_Receipt()
        {
        }

        #endregion

        #region Main Public Methods

        /// <summary>
        /// Print a receipt for each gift (one page for each donor) in the batch
        /// </summary>
        /// <param name="AGiftTDS"></param>
        public void PrintGiftBatchReceipts(GiftBatchTDS AGiftTDS)
        {
            AGiftBatchRow GiftBatchRow = AGiftTDS.AGiftBatch[0];

            DataView GiftView = new DataView(AGiftTDS.AGift);

            //AGiftTDS.AGift.DefaultView.RowFilter
            GiftView.RowFilter = String.Format("{0}={1} and {2}={3}",
                AGiftTable.GetLedgerNumberDBName(), GiftBatchRow.LedgerNumber,
                AGiftTable.GetBatchNumberDBName(), GiftBatchRow.BatchNumber);
            String ReceiptedDonorsList = "";
            List <Int32>ReceiptedGiftTransactions = new List <Int32>();
            SortedList <Int64, AGiftTable>GiftsPerDonor = new SortedList <Int64, AGiftTable>();

            foreach (DataRowView rv in GiftView)
            {
                AGiftRow GiftRow = (AGiftRow)rv.Row;
                bool ReceiptEachGift;
                String ReceiptLetterFrequency;
                bool EmailGiftStatement;
                bool AnonymousDonor;

                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerReceiptingInfo(
                    GiftRow.DonorKey,
                    out ReceiptEachGift,
                    out ReceiptLetterFrequency,
                    out EmailGiftStatement,
                    out AnonymousDonor);

                if (ReceiptEachGift)
                {
                    // I want to print a receipt for this gift,
                    // but if there's already one queued for this donor,
                    // I'll add this gift onto the existing receipt.

                    if (!GiftsPerDonor.ContainsKey(GiftRow.DonorKey))
                    {
                        GiftsPerDonor.Add(GiftRow.DonorKey, new AGiftTable());
                    }

                    AGiftRow NewRow = GiftsPerDonor[GiftRow.DonorKey].NewRowTyped();
                    DataUtilities.CopyAllColumnValues(GiftRow, NewRow);
                    GiftsPerDonor[GiftRow.DonorKey].Rows.Add(NewRow);
                }  // if receipt required

            } // foreach gift

            String HtmlDoc = "";

            foreach (Int64 DonorKey in GiftsPerDonor.Keys)
            {
                String DonorShortName;
                TPartnerClass DonorClass;
                TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(DonorKey, out DonorShortName, out DonorClass);
                DonorShortName = Calculations.FormatShortName(DonorShortName, eShortNameFormat.eReverseShortname);

                string HtmlPage = TRemote.MFinance.Gift.WebConnectors.PrintGiftReceipt(
                    GiftBatchRow.CurrencyCode,
                    DonorShortName,
                    DonorKey,
                    DonorClass,
                    GiftsPerDonor[DonorKey]
                    );

                TFormLettersTools.AttachNextPage(ref HtmlDoc, HtmlPage);
                ReceiptedDonorsList += (DonorShortName + "\r\n");

                foreach (AGiftRow GiftRow in GiftsPerDonor[DonorKey].Rows)
                {
                    ReceiptedGiftTransactions.Add(GiftRow.GiftTransactionNumber);
                }
            }

            TFormLettersTools.CloseDocument(ref HtmlDoc);

            if (ReceiptedGiftTransactions.Count > 0)
            {
                TFrmReceiptControl.PreviewOrPrint(HtmlDoc);

                if (MessageBox.Show(
                        Catalog.GetString(
                            "Press OK if receipts to these recipients were printed correctly.\r\nThe gifts will be marked as receipted.\r\n") +
                        ReceiptedDonorsList,

                        Catalog.GetString("Receipt Printing"),
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (Int32 Trans in ReceiptedGiftTransactions)
                    {
                        TRemote.MFinance.Gift.WebConnectors.MarkReceiptsPrinted(
                            GiftBatchRow.LedgerNumber,
                            GiftBatchRow.BatchNumber,
                            Trans);
                    }
                }
            }
        }

        #endregion
    }
}