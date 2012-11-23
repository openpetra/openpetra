//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of DonorRecipientHistory_ManualCode.
    /// </summary>
    public partial class TFrmDonorRecipientHistory
    {
        private DataView FFilteredDataView = null;
        private Int32 FLedgerNumber = -1;

        /// the Donor
        public long Donor
        {
            set
            {
                txtDonor.Text = String.Format("{0:0000000000}", value);
            }
        }

        /// the recipient
        public long Recipient
        {
            set
            {
                txtRecipient.Text = String.Format("{0:0000000000}", value);
            }
        }

        /// <summary>
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                cmbLedger.SetSelectedInt32(FLedgerNumber);
            }
        }

        private void InitializeManualCode()
        {
//            txtLedger.Text = "" + Ict.Petra.Client.MFinance.Logic.TLedgerSelection.DetermineDefaultLedger();
            btnView.Enabled = false;
        }

        private void OnCmbLedgerChange(System.Object sender, EventArgs e)
        {
            FLedgerNumber = cmbLedger.GetSelectedInt32();
        }

        private void EnalbeLedgerDropdown()
        {
            cmbLedger.Enabled = true;
            Int16 DefaultLedger = TUserDefaults.GetInt16Default(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER, -1);

            if (DefaultLedger > 0)
            {
                cmbLedger.SetSelectedInt32(DefaultLedger);
            }

            if (cmbLedger.Count == 1)
            {
                cmbLedger.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Browse: (Re)LoadTableContents, called after injection of parameters or manual via button
        /// </summary>
        public void Browse(bool loading)
        {
            TVerificationResultCollection AMessages;
            Hashtable requestParams = new Hashtable();
            Int64 donor = Convert.ToInt64(txtDonor.Text);
            Int64 recipient = Convert.ToInt64(txtRecipient.Text);

            if ((donor == 0) && (recipient == 0))
            {
                MessageBox.Show(Catalog.GetString("You have to restrict via donor or via recipient"));
                return;
            }

            if (FLedgerNumber < 0)
            {
                MessageBox.Show(Catalog.GetString("Select Ledger then press 'Browse'."));
                return;
            }

            requestParams.Add("Donor", donor);
            requestParams.Add("Recipient", recipient);


            GiftBatchTDS newTDS = TRemote.MFinance.Gift.WebConnectors.LoadDonorRecipientHistory(
                requestParams,
                out AMessages);

            if ((AMessages != null) && (AMessages.Count > 0))
            {
                MessageBox.Show(Messages.BuildMessageFromVerificationResult(Catalog.GetString("Error calling Donor/Recipient history"), AMessages));
                return;
            }
            else
            {
                FMainDS = newTDS;
            }

            if (FMainDS != null)
            {
                FFilteredDataView = FMainDS.AGiftDetail.DefaultView;
                FFilteredDataView.Sort = "DateEntered DESC";
                FFilteredDataView.AllowNew = false;
                RowFilter = "";

                if (txtMotivationDetail.Text.Length > 0)
                {
                    AddRowFilter(AGiftDetailTable.GetMotivationDetailCodeDBName() + " LIKE '" + txtMotivationDetail.Text + "%'");
                }

                if (txtMotivationGroup.Text.Length > 0)
                {
                    AddRowFilter(AGiftDetailTable.GetMotivationGroupCodeDBName() + " LIKE '" + txtMotivationGroup.Text + "%'");
                }

                if (loading)
                {
                    if (FMainDS.AGiftDetail.Count > 0)
                    {
                        GiftBatchTDSAGiftDetailRow gdr = (GiftBatchTDSAGiftDetailRow)FFilteredDataView[FFilteredDataView.Count - 1].Row;
                        dtpDateFrom.Date = gdr.DateEntered;
                    }
                    else
                    {
                        dtpDateFrom.Date = null;
                        dtpDateTo.Date = null;
                    }
                }

                if (dtpDateFrom.Date.HasValue)
                {
                    AddRowFilter(String.Format(CultureInfo.InvariantCulture.DateTimeFormat,
                            "DateEntered  >= #{0}#", dtpDateFrom.Date));
                }

                if (dtpDateTo.Date.HasValue)
                {
                    AddRowFilter(String.Format(CultureInfo.InvariantCulture.DateTimeFormat,
                            "DateEntered <= #{0}#", dtpDateTo.Date));
                }

                FFilteredDataView.RowFilter = RowFilter;

                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FFilteredDataView);
                SelectByIndex(0);
                txtNumberOfGifts.Text = Convert.ToString(FFilteredDataView.Count);
                UpdateTotals();
            }
        }

        private String RowFilter = "";
        private void AddRowFilter(String filterCondition)
        {
            if (RowFilter.Length > 0)
            {
                RowFilter = RowFilter + " AND ";
            }

            RowFilter += filterCondition;
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            Browse(false);
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            if ((FPreviouslySelectedDetailRow != null) && (FMainDS != null))
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    TFrmGiftBatch gb = new TFrmGiftBatch(this);
                    gb.ViewMode = true;
                    gb.ViewModeTDS = FMainDS;
                    // When I call Gift Batch, it will want one row in a LedgerTable!
                    gb.ViewModeTDS.ALedger.Merge(TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber));
                    gb.ShowDetailsOfOneBatch(FLedgerNumber, FPreviouslySelectedDetailRow.BatchNumber);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// static method for opening the window from partner module
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PartnerKey"></param>
        /// <param name="theParentForm"></param>
        public static void OpenWindowDonorRecipientHistory(String Name, Int64 PartnerKey, Form theParentForm)
        {
            if (PartnerKey == -1)
            {
                MessageBox.Show(Catalog.GetString("No current partner selected"));
                return;
            }

            Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory frmDRH = new  Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory(
                theParentForm);
            try
            {
                frmDRH.Cursor = Cursors.WaitCursor;

                if (Name.Equals("mniMaintainDonorHistory"))
                {
                    frmDRH.Donor = PartnerKey;
                }
                else
                {
                    frmDRH.Recipient = PartnerKey;
                }

                frmDRH.EnalbeLedgerDropdown();
                frmDRH.Browse(true);
                frmDRH.Show();
            }
            finally
            {
                frmDRH.Cursor = Cursors.Default;
            }
        }

        private void UpdateTotals()
        {
            Decimal sum = 0;

            if (FFilteredDataView != null)
            {
                foreach (DataRowView rv in FFilteredDataView)
                {
                    AGiftDetailRow gdr = (AGiftDetailRow)rv.Row;
                    sum += gdr.GiftTransactionAmount;
                    //TODO Convert currencies
                }

                txtGiftTotal.NumberValueDecimal = sum;

                if (FMainDS.AGiftBatch.Count > 0)
                {
                    txtGiftTotal.CurrencySymbol = FMainDS.AGiftBatch[0].CurrencyCode;
                }
            }

            txtGiftTotal.ReadOnly = true;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                btnView.Enabled = true;
            }
            else
            {
                grdDetails.Selection.ResetSelection(false);
                FPreviouslySelectedDetailRow = null;
                btnView.Enabled = false;
            }
        }
    }
}