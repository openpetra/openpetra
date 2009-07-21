/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{
    public partial class TFrmAccountsPayableEditDocument
    {
        private void NewDetail(Object sender, EventArgs e)
        {
            // get the entered amounts, so that we can calculate the missing amount for the new detail
            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            double DetailAmount = FMainDS.AApDocument[0].TotalAmount;

            foreach (AApDocumentDetailRow detailRow in FMainDS.AApDocumentDetail.Rows)
            {
                DetailAmount -= detailRow.Amount;
            }

            if (DetailAmount < 0)
            {
                DetailAmount = 0;
            }

            CreateNewAApDocumentDetail(
                FMainDS.AApDocument[0].LedgerNumber,
                FMainDS.AApDocument[0].ApNumber,
                FMainDS.AApSupplier[0].DefaultExpAccount,
                FMainDS.AApSupplier[0].DefaultCostCentre,
                DetailAmount,
                FMainDS.AApDocument[0].LastDetailNumber);
            FMainDS.AApDocument[0].LastDetailNumber++;
        }

        private void UpdateCreditTerms(object sender, EventArgs e)
        {
            if (sender == dtpDateDue)
            {
                int diffDays = (dtpDateDue.Value - dtpDateIssued.Value).Days;

                if (diffDays < 0)
                {
                    diffDays = 0;
                    dtpDateDue.Value = dtpDateIssued.Value;
                }

                nudCreditTerms.Value = diffDays;
            }
            else if ((sender == dtpDateIssued) || (sender == nudCreditTerms))
            {
                dtpDateDue.Value = dtpDateIssued.Value.AddDays((double)nudCreditTerms.Value);
            }
        }
    }
}