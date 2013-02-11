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
            grdDetails.DoubleClick += new EventHandler(grdDetails_DoubleClick);
        }

        void grdDetails_DoubleClick(object sender, EventArgs e)
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
                    //gb.ShowDetailsOfOneBatch(FLedgerNumber, FPreviouslySelectedDetailRow.BatchNumber);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
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

        private void SetupGrid()
        {
			grdDetails.Columns.Clear();
			grdDetails.AddDateColumn("Date Entered", FMainDS.Tables["Table"].Columns["DateEntered"]);
			grdDetails.AddTextColumn("Group", FMainDS.Tables["Table"].Columns["MotivationGroupCode"]);
			grdDetails.AddTextColumn("Detail", FMainDS.Tables["Table"].Columns["MotivationDetailCode"]);
			grdDetails.AddTextColumn("Receipt", FMainDS.Tables["Table"].Columns["ReceiptNumber"]);
			grdDetails.AddCurrencyColumn("Amount (Base)", FMainDS.Tables["Table"].Columns["GiftAmount"]);
			grdDetails.AddCurrencyColumn("Amount (Intl)", FMainDS.Tables["Table"].Columns["GiftAmountIntl"]);
			grdDetails.AddCheckBoxColumn("C", FMainDS.Tables["Table"].Columns["ConfidentialGiftFlag"]);
			grdDetails.AddTextColumn("Batch", FMainDS.Tables["Table"].Columns["BatchNumber"]);
			grdDetails.AddTextColumn("Trans", FMainDS.Tables["Table"].Columns["GiftTransactionNumber"]);
			grdDetails.AddTextColumn("Recipient", FMainDS.Tables["Table"].Columns["RecipientDescription"]);
			grdDetails.AddTextColumn("Reference", FMainDS.Tables["Table"].Columns["Reference"]);
			grdDetails.AddTextColumn("Comment One", FMainDS.Tables["Table"].Columns["GiftCommentOne"]);
			grdDetails.AddTextColumn("Comment Type", FMainDS.Tables["Table"].Columns["CommentOneType"]);
			grdDetails.AddTextColumn("Recipient Ledger", FMainDS.Tables["Table"].Columns["RecipientLedgerNumber"]);
			grdDetails.AddTextColumn("Recipient", FMainDS.Tables["Table"].Columns["RecipientKey"]);
			grdDetails.AddCheckBoxColumn("Charge Fee", FMainDS.Tables["Table"].Columns["ChargeFlag"]);
			grdDetails.AddTextColumn("Method of Payment", FMainDS.Tables["Table"].Columns["MethodOfPaymentCode"]);
			grdDetails.AddTextColumn("Method of Giving", FMainDS.Tables["Table"].Columns["MethodOfGivingCode"]);
			grdDetails.AddTextColumn("Cost Centre Code", FMainDS.Tables["Table"].Columns["CostCentreCode"]);
			grdDetails.AddTextColumn("Comment Two", FMainDS.Tables["Table"].Columns["GiftCommentTwo"]);
			grdDetails.AddTextColumn("Comment Three", FMainDS.Tables["Table"].Columns["GiftCommentThree"]);
			grdDetails.AddTextColumn("Mailing Code", FMainDS.Tables["Table"].Columns["MailingCode"]);
        }
        
        
        /// <summary>
        /// Browse: (Re)LoadTableContents, called after injection of parameters or manual via button
        /// </summary>
        public void Search(bool ALoading)
        {
            TVerificationResultCollection AMessages;
            Hashtable requestParams = new Hashtable();

            Int64 donor = Convert.ToInt64(txtDonor.Text);
            Int64 recipient = Convert.ToInt64(txtRecipient.Text);

            string motivationGroup = txtMotivationGroup.Text.Trim();
            string motivationDetail = txtMotivationDetail.Text.Trim();

            string dateFrom = dtpDateFrom.Date.HasValue ? dtpDateFrom.Date.Value.ToShortDateString(): String.Empty;
            string dateTo = dtpDateTo.Date.HasValue ? dtpDateTo.Date.Value.ToShortDateString(): String.Empty;
            
            if ((donor == 0) && (recipient == 0))
            {
                MessageBox.Show(Catalog.GetString("You have to restrict the search via donor and/or via recipient"), Catalog.GetString("Donor/Recipient Error"));
                txtDonor.Focus();
                return;
            }

            if (FLedgerNumber < 0)
            {
                MessageBox.Show(Catalog.GetString("Select Ledger then press 'Browse'."), Catalog.GetString("Ledger Error"));
                return;
            }

			if (dateFrom.Length == 0 && dtpDateFrom.Text.Length > 0)
			{
                MessageBox.Show(Catalog.GetString("Please enter a valid Date From."), Catalog.GetString("Date From Error"));
				dtpDateFrom.Focus();
				dtpDateFrom.SelectAll();
                return;
			}

			if (dateTo.Length == 0 && dtpDateTo.Text.Length > 0)
			{
                MessageBox.Show(Catalog.GetString("Please enter a valid Date To."), Catalog.GetString("Date To Error"));
				dtpDateTo.Focus();
				dtpDateTo.SelectAll();
                return;
			}
			
			txtMotivationGroup.Text = txtMotivationGroup.Text.ToUpper();
			txtMotivationDetail.Text = txtMotivationDetail.Text.ToUpper();

			requestParams.Add("Ledger", FLedgerNumber);
            requestParams.Add("Donor", donor);
            requestParams.Add("Recipient", recipient);
            requestParams.Add("MotivationGroup", motivationGroup);
            requestParams.Add("MotivationDetail", motivationDetail);
            requestParams.Add("DateFrom", dateFrom);
            requestParams.Add("DateTo", dateTo);

			try
			{
	            Cursor = Cursors.WaitCursor;
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
	            	if (FMainDS.Tables.Contains("Table"))
	            	{
	            		FMainDS.Tables.Remove("Table");
	            	}
	            	FMainDS = newTDS;
	            }
	
	            if (FMainDS != null)
	            {
	                if (ALoading)
	                {
	                	if (FMainDS.Tables["Table"].Rows.Count > 0)
	                    {
	                		//If this form is loaded from elsewhere, set DateFrom to be lowest date in returned Table
	                		DataRow gdr = (DataRow)FMainDS.Tables["Table"].Rows[FMainDS.Tables["Table"].Rows.Count - 1];
	                		dtpDateFrom.Date = Convert.ToDateTime(gdr["DateEntered"]);
	                    }
	                    else
	                    {
	                        dtpDateFrom.Date = null;
	                        dtpDateTo.Date = null;
	                    }
	                }
	
					if (grdDetails.Columns.Count < 2)
					{
						SetupGrid();
					}
					
	                DataView myDataView = FMainDS.Tables["Table"].DefaultView;
					myDataView.AllowNew = false;
					grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);                
	                
	                SelectByIndex(0);
	                txtNumberOfGifts.Text = (grdDetails.Rows.Count - 1).ToString();
	                grdDetails.Columns[0].Width = 90; // Date
	                grdDetails.Columns[2].Width = 80; // Motiv. Detail
	                grdDetails.Columns[4].Width = 60; // Amount
	                grdDetails.Columns[8].Width = 160; // Recipient
	            }
 
	            UpdateTotals();
			}
			finally
			{
				Cursor = Cursors.Default;
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

        private void BtnSearchClick(object sender, EventArgs e)
        {
        	Search(false);
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
                frmDRH.Search(true);
                frmDRH.Show();
            }
            finally
            {
                frmDRH.Cursor = Cursors.Default;
            }
        }

        private void UpdateTotals()
        {
            decimal sum = 0;
            decimal sumIntl = 0;

            if (grdDetails.Rows.Count > 1)
            {
            	foreach (DataRow rv in FMainDS.Tables["Table"].Rows)
                {
                    DataRow gdr = (DataRow)rv;
                    sum += (decimal)gdr["GiftAmount"];
                    sumIntl += (decimal)gdr["GiftAmountIntl"];
                }
            }

           	txtGiftTotal.NumberValueDecimal = sum;
          	txtGiftTotal.CurrencySymbol = FMainDS.ALedger[0].BaseCurrency;
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
            }
            else
            {
                grdDetails.Selection.ResetSelection(false);
                FPreviouslySelectedDetailRow = null;
            }
        }
    }
}