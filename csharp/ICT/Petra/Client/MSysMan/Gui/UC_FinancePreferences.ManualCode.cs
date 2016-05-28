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
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_FinancePreferences
    {
        //private int FCurrentLedger;
        private int FInitiallySelectedLedger;
        private bool FInactiveValuesWarningOnGLPosting = false;
        private bool FShowMoneyAsCurrency = true;
        private bool FShowDecimalsAsCurrency = true;
        private bool FShowThousands = true;

        private void InitializeManualCode()
        {
            FInitiallySelectedLedger = TLstTasks.InitiallySelectedLedger;

            cmbDefaultLedger.SetSelectedInt32(FInitiallySelectedLedger);

            FShowMoneyAsCurrency = TUserDefaults.GetBooleanDefault(StringHelper.FINANCE_CURRENCY_FORMAT_AS_CURRENCY, true);
            chkMoneyFormat.Checked = FShowMoneyAsCurrency;

            FShowDecimalsAsCurrency = TUserDefaults.GetBooleanDefault(StringHelper.FINANCE_DECIMAL_FORMAT_AS_CURRENCY, true);
            chkDecimalFormat.Checked = FShowDecimalsAsCurrency;

            FShowThousands = TUserDefaults.GetBooleanDefault(StringHelper.FINANCE_CURRENCY_SHOW_THOUSANDS, true);
            chkShowThousands.Checked = FShowThousands;

            // Examples
            txtMoneyExample.Context = ".MFinance";
            txtMoneyExample.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Currency;
            txtMoneyExample.CurrencyCode = "USD";
            txtMoneyExample.NumberValueDecimal = 12345.67m;

            txtExchangeRateExample.Context = ".MFinance";
            txtExchangeRateExample.ControlMode = TTxtNumericTextBox.TNumericTextBoxMode.Decimal;
            txtExchangeRateExample.NumberValueDecimal = 6.789m;
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

            if (FInitiallySelectedLedger != NewLedger)
            {
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER, NewLedger);

                // change the current ledger in the main window
                Form MainWindow = FPetraUtilsObject.GetCallerForm();
                PropertyInfo CurrentLedgerProperty = MainWindow.GetType().GetProperty("CurrentLedger");
                CurrentLedgerProperty.SetValue(MainWindow, NewLedger, null);

                return true;
            }

            if (FInactiveValuesWarningOnGLPosting != chkInactiveValuesWarningOnGLPosting.Checked)
            {
                FInactiveValuesWarningOnGLPosting = chkInactiveValuesWarningOnGLPosting.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GL_WARN_OF_INACTIVE_VALUES_ON_POSTING, FInactiveValuesWarningOnGLPosting);
            }

            if (FShowMoneyAsCurrency != chkMoneyFormat.Checked)
            {
                FShowMoneyAsCurrency = chkMoneyFormat.Checked;
                TUserDefaults.SetDefault(StringHelper.FINANCE_CURRENCY_FORMAT_AS_CURRENCY, FShowMoneyAsCurrency);
            }

            if (FShowDecimalsAsCurrency != chkDecimalFormat.Checked)
            {
                FShowDecimalsAsCurrency = chkDecimalFormat.Checked;
                TUserDefaults.SetDefault(StringHelper.FINANCE_DECIMAL_FORMAT_AS_CURRENCY, FShowDecimalsAsCurrency);
            }

            if (FShowThousands != chkShowThousands.Checked)
            {
                FShowThousands = chkShowThousands.Checked;
                TUserDefaults.SetDefault(StringHelper.FINANCE_CURRENCY_SHOW_THOUSANDS, FShowThousands);
            }

            return false;
        }

        private void ExampleCheckChanged(object sender, EventArgs e)
        {
            txtMoneyExample.OverrideNormalFormatting(!chkMoneyFormat.Checked, chkShowThousands.Checked);
            txtExchangeRateExample.OverrideNormalFormatting(!chkDecimalFormat.Checked, chkShowThousands.Checked);
        }
    }
}