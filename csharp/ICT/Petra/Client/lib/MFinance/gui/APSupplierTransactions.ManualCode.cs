/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Common.Data;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    public partial class TFrmAPSupplierTransactions
    {
        Int32 FLedgerNumber = -1;
        Int64 FPartnerKey = -1;

        /// <summary>
        /// load the supplier, do the first search with the default search parameters
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        public void LoadSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            FLedgerNumber = ALedgerNumber;
            FPartnerKey = APartnerKey;

            FMainDS.Merge(TRemote.MFinance.AccountsPayable.WebConnectors.FindAApDocument(
                    ALedgerNumber, APartnerKey,

                    // cmbStatus.GetSelectedString(),
                    "UNPOSTED",
                    cmbType.SelectedIndex == 1,
                    chkHideAgedTransactions.Checked));

            // TODO: calculate duedate column? or should that be done on the server?

            ShowData();
        }

        /// <summary>
        /// needed for generated code
        /// </summary>
        void ShowDataManual()
        {
            DataView myDataView = FMainDS.AApDocument.DefaultView;

            myDataView.AllowNew = false;
            grdResult.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdResult.AutoSizeCells();
        }

        private void OpenSelectedApDocument(System.Object sender, EventArgs args)
        {
            DataRowView[] SelectedGridRow = grdResult.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this.Handle);
                frm.LoadAApDocument(FLedgerNumber, Convert.ToInt32(SelectedGridRow[0][FMainDS.AApDocument.ColumnApNumber.ColumnName]));
                frm.Show();
            }
        }

        /// <summary>
        /// create a new invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInvoice(object sender, EventArgs e)
        {
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this.Handle);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, false);
            frm.Show();
        }

        /// <summary>
        /// create a new credit note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCreditNote(object sender, EventArgs e)
        {
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this.Handle);

            frm.CreateAApDocument(FLedgerNumber, FPartnerKey, true);
            frm.Show();
        }

        /// <summary>
        /// untag all documents
        /// </summary>
        private void UntagAll(object sender, EventArgs e)
        {
            foreach (AccountsPayableTDSAApDocumentRow row in FMainDS.AApDocument.Rows)
            {
                row.Tagged = false;
            }
        }

        /// <summary>
        /// Post all tagged documents in one GL Batch
        /// </summary>
        private void PostTaggedDocuments(object sender, EventArgs e)
        {
            string msg = "";

            foreach (AccountsPayableTDSAApDocumentRow row in FMainDS.AApDocument.Rows)
            {
                if (!row.IsTaggedNull() && row.Tagged)
                {
                    msg += row.ApNumber.ToString() + " ";
                }
            }

            MessageBox.Show("tagged: " + msg);

            // TODO: transmit ledgernumber and ap numbers to server
            // TODO: update view to reflect posted documents
        }
    }
}