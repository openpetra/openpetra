//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2016 by OM International
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
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Gui.ICH;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TPeriodEnd
    {
        TVerificationResultCollection FverificationResult = new TVerificationResultCollection();
        private Int32 FLedgerNumber;

        /// <summary>
        /// Made public because I want to access this from a callback
        /// </summary>
        public Boolean FOperationResult = false;
        private List <Int32>FglBatchNumbers;
        private Boolean FMonthEndStewardship = false;

        /// <summary>
        /// Sets the ledger number and initializes the gui ...
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                string msg;

                FOperationResult = RunPeriodEnd(true);
                msg = FverificationResult.BuildVerificationResultString();

                if (FOperationResult)
                {
                    msg = Catalog.GetString("Process cannot be performed:") +
                          "\r\n\r\n" + msg + "\r\n";
                }

                if (TVerificationHelper.IsNullOrOnlyNonCritical(FverificationResult))
                {
                    msg += (blnIsInMonthMode) ?
                           Catalog.GetString("Press the button below to close the current period.")
                           : Catalog.GetString("Press the button below to close the current year.");
                }

                tbxMessage.Text = msg;
                btnPeriodEnd.Enabled = TVerificationHelper.IsNullOrOnlyNonCritical(FverificationResult);
                this.OnResizeEnd(new EventArgs());
            }
        }

        private void CancelButtonClick(object btn, EventArgs e)
        {
            if (btnCancel.Text != Catalog.GetString("Done")) // The Cancel button becomes "Done" after AsyncOpEnd, below.
            {
                TRemote.MFinance.GL.WebConnectors.CancelPeriodOperation();
            }

            this.Close();
        }

        private void PeriodEndButtonClick(object btn, EventArgs e)
        {
            tbxMessage.Text = "Running Period End operation - please wait.";
            UseWaitCursor = true;
            Thread periodEndThread = new Thread(() => periodEndThreadOp());
            periodEndThread.SetApartmentState(ApartmentState.STA);
            periodEndThread.Start();
        }

        private void periodEndThreadOp()
        {
            RunPeriodEnd(false);
            PeriodEndTidyup();
        }

        delegate void CrossThreadUpdate ();

        /// <summary>Called after the operation</summary>
        /// <remarks>Uses an "invoke" to update screen controls.</remarks>
        ///
        public void PeriodEndTidyup()
        {
            if (InvokeRequired)
            {
                Invoke(new CrossThreadUpdate(PeriodEndTidyup));
                return;
            }

            UseWaitCursor = false;

            if (FOperationResult)
            {
                tbxMessage.Text = Catalog.GetString("Operation did not complete:") +
                                  "\r\n\r\n" +
                                  FverificationResult.BuildVerificationResultString();
            }
            else
            {
                tbxMessage.Text = Catalog.GetString("Operation completed successfully.") +
                                  "\r\n\r\n" +
                                  FverificationResult.BuildVerificationResultString();
            }

            btnPeriodEnd.Visible = false;

            if (!FOperationResult)
            {
                TFrmGLPeriodEndReporting PrintDlg = new TFrmGLPeriodEndReporting(this);
                PrintDlg.glBatchNumbers = FglBatchNumbers;
                PrintDlg.FLedgerNumber = FLedgerNumber;
                PrintDlg.FMonthMode = blnIsInMonthMode;
                PrintDlg.ShowDialog();

                if (FMonthEndStewardship)
                {
                    //
                    // Show Stewardship Reports screen
                    TFrmStewardshipReports stewardshipForm = new TFrmStewardshipReports(null);
                    stewardshipForm.LedgerNumber = FLedgerNumber;
                    stewardshipForm.Show();
                }
            }

            btnCancel.Text = Catalog.GetString("Done");
        }

        private bool RunPeriodEnd(bool AInInfoMode)
        {
            if (blnIsInMonthMode)
            {
                FOperationResult = TRemote.MFinance.GL.WebConnectors.PeriodMonthEnd(
                    FLedgerNumber, AInInfoMode,
                    out FglBatchNumbers,
                    out FMonthEndStewardship,
                    out FverificationResult);
            }
            else
            {
                FOperationResult = TRemote.MFinance.GL.WebConnectors.PeriodYearEnd(
                    FLedgerNumber, AInInfoMode,
                    out FglBatchNumbers,
                    out FverificationResult);
            }

            TLogging.Log("RunPeriodEnd: " + FOperationResult);
            return FOperationResult;
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