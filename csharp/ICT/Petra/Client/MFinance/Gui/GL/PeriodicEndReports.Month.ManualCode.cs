//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLRevaluation.
    /// </summary>
    public partial class TPeriodEndMonthly
    {
        TVerificationResultCollection verificationResult;
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                bool blnErrorStatus = TRemote.MFinance.GL.WebConnectors.TPeriodMonthEndInfo(
                    value, out verificationResult);
                tbxMessage.Text = verificationResult.BuildVerificationResultString();
                btnMonthEnd.Enabled = !blnErrorStatus;
                this.OnResizeEnd(new EventArgs());
            }
        }

        private void CancelMonthEnd(object btn, EventArgs e)
        {
            this.Close();
        }

        private void RunMonthEnd(object btn, EventArgs e)
        {
            bool blnErrorStatus = TRemote.MFinance.GL.WebConnectors.TPeriodMonthEndInfo(
                FLedgerNumber, out verificationResult);

            tbxMessage.Text = verificationResult.BuildVerificationResultString();
            btnMonthEnd.Visible = false;
            btnCancel.Text = Catalog.GetString("Done");
        }

        private void ResizeForm(object from, EventArgs e)
        {
            tbxMessage.Size = new Size(this.Width - 30, this.Height - 100);
            this.btnMonthEnd.Location =
                new System.Drawing.Point(this.Width - 400, this.Height - 70);
            this.btnCancel.Location =
                new System.Drawing.Point(this.Width - 200, this.Height - 70);
        }
    }
}