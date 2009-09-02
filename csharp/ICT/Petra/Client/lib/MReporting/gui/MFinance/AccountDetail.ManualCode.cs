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
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmAccountDetail
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                TFinanceControls.InitialiseAccountList(ref cmbAccountStart, FLedgerNumber, false, false, false);
                TFinanceControls.InitialiseAccountList(ref cmbAccountEnd, FLedgerNumber, false, false, false);
                TFinanceControls.InitialiseCostCentreList(ref cmbCostCentreStart, FLedgerNumber, false, false, false, false);
                TFinanceControls.InitialiseCostCentreList(ref cmbCostCentreEnd, FLedgerNumber, false, false, false, false);
                TFinanceControls.InitialiseAccountList(ref clbAccounts, FLedgerNumber, false, false, false);
                TFinanceControls.InitialiseCostCentreList(ref clbCostCentres, FLedgerNumber, false, false, false, false);
                TFinanceControls.InitialiseAccountHierarchyList(ref cmbAccountHierarchy, FLedgerNumber);
                TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbPeriodYear, FLedgerNumber);

                // if there is only one hierarchy, disable the control
                this.cmbAccountHierarchy.Enabled = (this.cmbAccountHierarchy.Count > 1);

                // TODO does not seem to work to select the first and last item; SelectedIndex is broken?
                // or is this not important at all, because of prepared initial settings?
                cmbAccountStart.SelectedIndex = 0;
                cmbAccountEnd.SelectedIndex = cmbAccountEnd.Count - 1;
                cmbCostCentreStart.SelectedIndex = 0;
                cmbCostCentreEnd.SelectedIndex = cmbCostCentreEnd.Count - 1;
                clbAccounts.SetCheckedStringList("");     // TODO: select first account?
                clbCostCentres.SetCheckedStringList(FLedgerNumber + "00S");
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
        }
    }
}