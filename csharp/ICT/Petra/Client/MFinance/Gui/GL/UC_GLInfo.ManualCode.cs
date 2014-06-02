//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Manual code for the GL Info user control
    /// </summary>
    public partial class TUC_GLInfo
    {
        /// <summary>
        /// Initialize values
        /// </summary>
        public void InitializeManualCode()
        {
            cmbBaseCurrency.Enabled = false;
            cmbIntlCurrency.Enabled = false;
            txtAccountingPeriods.Enabled = false;
            txtCalendarMode.Enabled = false;
            txtCurrencyRevaluation.Enabled = false;
            txtCurrentPeriod.Enabled = false;
            txtForwardPeriods.Enabled = false;
            dtpPeriodStartDate.Enabled = false;
            dtpPeriodEndDate.Enabled = false;
            dtpPostingAllowedUntil.Enabled = false;
            chkSuspenseAccounts.Enabled = false;
            chkBudgetControl.Enabled = false;

            FMainDS = new GLSetupTDS();
        }

        private Int32 FLedgerNumber;

        /// the ledger that the user is currently working with
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadLedgerInfo(FLedgerNumber));
                ShowData(FMainDS);
            }
        }

        /// <summary>
        /// implement dummy functions so that we can use this control on a yaml form
        /// </summary>
        public void GetDataFromControls()
        {
            // not implemented
        }

        /// <summary>
        /// Show ledger info data from given data set
        /// </summary>
        /// <param name="ADataSet">The data set for which details will be shown</param>
        private void ShowData(GLSetupTDS ADataSet)
        {
            ALedgerRow LedgerRow;
            AAccountingPeriodRow AccountingPeriodRow;
            DataRow TempRow;

            if ((ADataSet == null)
                || (ADataSet.ALedger.Count == 0))
            {
                return;
            }

            LedgerRow = (ALedgerRow)ADataSet.ALedger.Rows[0];

            cmbBaseCurrency.SetSelectedString(LedgerRow.BaseCurrency, -1);
            cmbIntlCurrency.SetSelectedString(LedgerRow.IntlCurrency, -1);

            txtAccountingPeriods.NumberValueInt = LedgerRow.NumberOfAccountingPeriods;

            if (LedgerRow.CalendarMode)
            {
                txtCalendarMode.Text = Catalog.GetString("Monthly");
            }
            else
            {
                txtCalendarMode.Text = Catalog.GetString("Non-Monthly");
            }

            txtCurrencyRevaluation.Text = LedgerRow.ForexGainsLossesAccount;
            txtCurrentPeriod.NumberValueInt = LedgerRow.CurrentPeriod;
            txtForwardPeriods.NumberValueInt = LedgerRow.NumberFwdPostingPeriods;

            TempRow = ADataSet.AAccountingPeriod.Rows.Find(new object[] { LedgerRow.LedgerNumber,
                                                                          LedgerRow.CurrentPeriod });

            if (TempRow != null)
            {
                AccountingPeriodRow = (AAccountingPeriodRow)TempRow;
                dtpPeriodStartDate.Date = AccountingPeriodRow.PeriodStartDate.Date;
                dtpPeriodEndDate.Date = AccountingPeriodRow.PeriodEndDate.Date;
            }

            TempRow = ADataSet.AAccountingPeriod.Rows.Find(new object[] { LedgerRow.LedgerNumber,
                                                                          LedgerRow.CurrentPeriod + LedgerRow.NumberFwdPostingPeriods });

            if (TempRow != null)
            {
                AccountingPeriodRow = (AAccountingPeriodRow)TempRow;
                dtpPostingAllowedUntil.Date = AccountingPeriodRow.PeriodEndDate.Date;
            }

            chkSuspenseAccounts.Checked = LedgerRow.SuspenseAccountFlag;
            chkBudgetControl.Checked = LedgerRow.BudgetControlFlag;
        }
    }
}