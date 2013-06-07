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
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TPeriodEnd
    {
        const bool INFORMATION_MODE = true;
        const bool CALCULATION_MODE = false;

        TVerificationResultCollection FverificationResult;
        private Int32 FLedgerNumber;

        /// <summary>
        /// Sets the ledger number and initializes the gui ...
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                string msg;

                bool ErrorStatus = RunPeriodEnd(INFORMATION_MODE);
                msg = FverificationResult.BuildVerificationResultString();

                if (ErrorStatus)
                {
                    msg = Catalog.GetString("The server returned this message:") +
                          "\r\n\r\n" + msg + "\r\n";
                }

                if (!FverificationResult.HasCriticalErrors)
                {
                    msg += Catalog.GetString("Press the button below to close the current period.");
                }
                tbxMessage.Text = msg;
                btnPeriodEnd.Enabled = (!FverificationResult.HasCriticalErrors);
                this.OnResizeEnd(new EventArgs());
            }
        }

        private void CancelButtonClick(object btn, EventArgs e)
        {
            this.Close();
        }

        private void PeriodEndButtonClick(object btn, EventArgs e)
        {
            RunPeriodEnd(CALCULATION_MODE);
            tbxMessage.Text = Catalog.GetString("The server returned this message:") +
                              "\r\n\r\n" +
                              FverificationResult.BuildVerificationResultString();
            btnPeriodEnd.Visible = false;
            btnCancel.Text = Catalog.GetString("Done");

            // reset valid dates as they may have changed: next time this object is called values are refreshed from server
            TLedgerSelection.ResetValidDates(FLedgerNumber);
        }

        private bool RunPeriodEnd(bool AInInfoMode)
        {
            bool blnErrorStatus;

            if (blnIsInMonthMode)
            {
                blnErrorStatus = TRemote.MFinance.GL.WebConnectors.TPeriodMonthEnd(
                    FLedgerNumber, AInInfoMode, out FverificationResult);
            }
            else
            {
                blnErrorStatus = TRemote.MFinance.GL.WebConnectors.TPeriodYearEnd(
                    FLedgerNumber, AInInfoMode, out FverificationResult);
            }

            return blnErrorStatus;
        }

        private void ResizeForm(object from, EventArgs e)
        {
            tbxMessage.Size = new Size(this.Width - 30, this.Height - 100);
            this.btnPeriodEnd.Location =
                new System.Drawing.Point(this.Width - 400, this.Height - 70);
            this.btnCancel.Location =
                new System.Drawing.Point(this.Width - 200, this.Height - 70);
        }
    }
}