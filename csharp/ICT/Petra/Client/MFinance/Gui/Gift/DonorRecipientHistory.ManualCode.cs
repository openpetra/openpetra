//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
using System.Collections;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of DonorRecipientHistory_ManualCode.
    /// </summary>
    public partial class TFrmDonorRecipientHistory
    {
        private long FDonor = 0;
    	/// the Donor
        public long Donor
        {
            set
            {
                FDonor = value;
                txtDonor.Text = String.Format("{0:0000000000}", value);
            }             //injected
        }

        private long FRecipient = 0;
	    /// the recipient
        public long Recipient
        {
            set
            {
                FRecipient = value;
                txtRecipient.Text = String.Format("{0:0000000000}", value);
            }             //injected
        }


        private void InitializeManualCode()
        {
            // needed?
        }

        /// <summary>
        /// Browse: (Re)LoadTableContents, called after injection of parameters or manual via button
        /// </summary>
        public void Browse()
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

            requestParams.Add("Donor", donor);
            requestParams.Add("Recipient", recipient);
          	
            requestParams.Add("From", dtpDateFrom.Text);
            requestParams.Add("To", dtpDateTo.Text);
			
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
                DataView myDataView = FMainDS.AGiftDetail.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            }
                
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            Browse();
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            // TODO Pop up a "normal" gift batch/gift/Giftdetail window where the gift shown this table and selected is selected
            // all the details are disabled (readonly)
        }
        /// <summary>
        /// static method for opening the window from partner module
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PartnerKey"></param>
        /// <param name="theHandle"></param>
        public static void OpenWindowDonorRecipientHistory(String Name, Int64 PartnerKey, IntPtr theHandle)
        {
            if (PartnerKey == -1)
            {
                MessageBox.Show(Catalog.GetString("No current partner seleted"));
                return;
            }

            //this.Cursor = Cursors.WaitCursor;
            Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory frmDRH = new  Ict.Petra.Client.MFinance.Gui.Gift.TFrmDonorRecipientHistory(
                theHandle);

            if (Name.Equals("mniMaintainDonorHistory"))
            {
                frmDRH.Donor = PartnerKey;
            }
            else
            {
                frmDRH.Recipient = PartnerKey;
            }

            frmDRH.Browse();
            frmDRH.Show();
            //this.Cursor = Cursors.Default;
        }
    }
}