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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.GL;

namespace Ict.Petra.Client.MFinance.Gui.AP
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

            FMainDS.Merge(TRemote.MFinance.AP.WebConnectors.FindAApDocument(
                    ALedgerNumber, APartnerKey,
                    "UNPOSTED", // cmbStatus.GetSelectedString(),
                    cmbType.SelectedIndex == 1,
                    chkHideAgedTransactions.Checked));

            // TODO: calculate duedate column? or should that be done on the server?

            foreach (AccountsPayableTDSAApDocumentRow row in FMainDS.AApDocument.Rows)
            {
                row.Tagged = false;
            }

            ShowData(FMainDS.AApSupplier[0]);
        }

        /// <summary>
        /// This will re-draw the form, so that any data changes are shown.
        /// </summary>
        public void Reload()
        {
            LoadSupplier(FLedgerNumber, FPartnerKey);
        }

        /// <summary>
        /// needed for generated code
        /// </summary>
        void ShowDataManual(AApSupplierRow ARow)
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
                TFrmAPEditDocument frm = new TFrmAPEditDocument(this);
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
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

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
            TFrmAPEditDocument frm = new TFrmAPEditDocument(this);

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
        /// Uses static functions from TFrmAPEditDocument
        /// </summary>
        private void PostTaggedDocuments(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();

            foreach (AccountsPayableTDSAApDocumentRow row in FMainDS.AApDocument.Rows)
            {
                if (!row.IsTaggedNull() && row.Tagged)
                {
                    if (TFrmAPEditDocument.ApDocumentCanPost (FMainDS, row))
                    {
                        TaggedDocuments.Add(row.ApNumber);
                    }
                }
            }

            if (TaggedDocuments.Count == 0)
            {
                return;
            }

            // TODO: make sure that there are uptodate exchange rates
            if (TFrmAPEditDocument.PostApDocumentList(FMainDS, FMainDS.AApDocument[0].LedgerNumber, TaggedDocuments))
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP documents have been posted successfully!"));

                // TODO: show posting register of GL Batch?

                // TODO: refresh the grid, to reflect that the transactions have been posted
                // TODO: somehow the row cannot be tagged anymore, readonly?
                FMainDS.AApDocument.Clear();
                LoadSupplier(FLedgerNumber, FPartnerKey);
            }
        }

        /// Add all selected invoices to the payment list and show that list so that the user can make the payment
        private void AddTaggedToPayment(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();

            foreach (AccountsPayableTDSAApDocumentRow row in FMainDS.AApDocument.Rows)
            {
                if (!row.IsTaggedNull() && row.Tagged)
                {
                    // TODO: only use tagged rows that can be paid?
                    TaggedDocuments.Add(row.ApNumber);
                }
            }

            if (TaggedDocuments.Count == 0)
            {
                return;
            }

            TFrmAPPayment frm = new TFrmAPPayment(this);

            frm.AddDocumentsToPayment(FMainDS, TaggedDocuments);

            frm.Show();
        }
    }
}