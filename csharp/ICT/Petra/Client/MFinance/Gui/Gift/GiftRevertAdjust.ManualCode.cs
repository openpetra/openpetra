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
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// Description of GiftRevertAdjust_ManualCode.
    /// </summary>
    public partial class TFrmGiftRevertAdjust
    {
        //private FMainDS GiftBatchTDS=new GiftBatchTDS();
        private void RevertAdjust(System.Object sender, System.EventArgs e)
        {
            if (chkSelect.Checked && (FPreviouslySelectedDetailRow == null))
            {
                // nothing seleted
                MessageBox.Show(Catalog.GetString("Please select a Batch!."));
                return;
            }

            MessageBox.Show(Catalog.GetString("Your batch has been sucessfully reverted"));
            return;
        }

        private void InitializeManualCode()
        {
            FMainDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                AGiftBatchTable.GetBatchStatusDBName(),
                MFinanceConstants.BATCH_UNPOSTED);
        }

        private void SelectBatchChanged(System.Object sender, EventArgs e)
        {
            grdDetails.Enabled = chkSelect.Checked;
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }
    }
}