//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2013 by OM International
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
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// manual methods for the generated window
    public partial class TFrmBatchDateDialog
    {
        private DateTime FBatchDate;
        private DateTime FStartDateCurrentPeriod;
        private DateTime FEndDateLastForwardingPeriod;

        /// <summary>
        /// The date the user has chosen to reverse the batch
        /// </summary>
        public DateTime BatchDate
        {
            get
            {
                return FBatchDate;
            }
        }

        private void InitializeManualCode()
        {
            txtMessage.Font = new Font(txtMessage.Font, FontStyle.Regular);
        }

        /// <summary></summary>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        /// <param name="ABatchNumber"></param>
        public void SetParameters(DateTime AStartDateCurrentPeriod, DateTime AEndDateLastForwardingPeriod, int ABatchNumber)
        {
            FStartDateCurrentPeriod = AStartDateCurrentPeriod;
            FEndDateLastForwardingPeriod = AEndDateLastForwardingPeriod;

            txtMessage.Text = string.Format(Catalog.GetString("Enter date for Reversal of Batch {0}. (Between {1} and {2}.)"),
                ABatchNumber, AStartDateCurrentPeriod.ToShortDateString(), AEndDateLastForwardingPeriod.ToShortDateString());
            dtpReversalDate.Date = AStartDateCurrentPeriod;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (ValidateManual())
            {
                FBatchDate = (DateTime)dtpReversalDate.Date;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ValidateManual()
        {
            TVerificationResultCollection VerificationResultCollection = new TVerificationResultCollection();

            TSharedFinanceValidation_GL.ValidateGLBatchDateManual(dtpReversalDate.Date, dtpReversalDate.Description,
                ref VerificationResultCollection, FStartDateCurrentPeriod, FEndDateLastForwardingPeriod, dtpReversalDate);

            return TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResultCollection, this.GetType(), dtpReversalDate.GetType());
        }
    }
}