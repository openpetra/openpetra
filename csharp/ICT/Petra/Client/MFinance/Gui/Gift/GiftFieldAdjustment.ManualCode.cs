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
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftFieldAdjustment
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        private void FieldChangeAdjustment(System.Object sender, EventArgs e)
        {
            if ((!dtpStartDate.Date.HasValue) || (!dtpEndDate.Date.HasValue) || (!dtpDateEffective.Date.HasValue))
            {
                MessageBox.Show(Catalog.GetString("Please supply valid Start, End and Effective dates."));
                return;
            }

            Int32 AdjustmentGiftBatch =
                TRemote.MFinance.Gift.WebConnectors.FieldChangeAdjustment(FLedgerNumber,
                    Convert.ToInt64(txtRecipientKey.Text),
                    dtpStartDate.Date.Value,
                    dtpEndDate.Date.Value,
                    Convert.ToInt64(txtOldFieldKey.Text),
                    dtpDateEffective.Date.Value,
                    !chkNoReceipt.Checked);

            if (AdjustmentGiftBatch != -1)
            {
                // TODO: display the new gift batch?
                MessageBox.Show(String.Format(Catalog.GetString("Please check and post gift batch {0}"), AdjustmentGiftBatch),
                    Catalog.GetString("Success"));
            }
            else
            {
                MessageBox.Show(Catalog.GetString("There was a problem creating the adjustment batch"),
                    Catalog.GetString("Failure"));
            }
        }
    }
}