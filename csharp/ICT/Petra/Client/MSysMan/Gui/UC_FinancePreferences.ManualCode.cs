//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Reflection;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_FinancePreferences
    {
        private int FCurrentLedger;
        private bool FNewDonorWarning = true;

        private void InitializeManualCode()
        {
            int DefaultLedger = TUserDefaults.GetInt32Default(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER);

            FCurrentLedger = TLstTasks.CurrentLedger;

            if (DefaultLedger > 0)
            {
                cmbDefaultLedger.SetSelectedInt32(DefaultLedger);
            }
            else
            {
                cmbDefaultLedger.SetSelectedInt32(FCurrentLedger);
            }

            FNewDonorWarning = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_NEW_DONOR_WARNING, true);
            chkNewDonorWarning.Checked = FNewDonorWarning;
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
        }

        /// <summary>
        /// Saves any changed preferences to s_user_defaults
        /// </summary>
        /// <returns>void</returns>
        public bool SaveFinanceTab()
        {
            int NewLedger = cmbDefaultLedger.GetSelectedInt32();

            if (FCurrentLedger != NewLedger)
            {
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER, NewLedger);

                // change the current ledger in the main window
                Form MainWindow = FPetraUtilsObject.GetCallerForm();
                PropertyInfo CurrentLedgerProperty = MainWindow.GetType().GetProperty("CurrentLedger");
                CurrentLedgerProperty.SetValue(MainWindow, NewLedger, null);

                return true;
            }

            if (FNewDonorWarning != chkNewDonorWarning.Checked)
            {
                FNewDonorWarning = chkNewDonorWarning.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_NEW_DONOR_WARNING, FNewDonorWarning);
            }

            return false;
        }
    }
}