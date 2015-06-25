//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
//
using System;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Logic
{
    /// <summary>
    /// Common logic for the two Admin Grants Setup (Receivable and Payable) screens.
    /// </summary>
    public static class TSetupAdminGrants
    {
        /// <summary>
        /// Populates the Cost Centre, Account Code and DR Account Code ComboBoxes.
        /// </summary>
        /// <param name="ACostCentreComboBox">Cost Centre ComboBox Control</param>
        /// <param name="AAccountCodeComboBox">Account Code ComboBox Control</param>
        /// <param name="ADrAccountCodeComboBox">DR Account Code ComboBox Control</param>
        /// <param name="ALedgerNumber">Ledger Number</param>
        /// <param name="ACalledByReceivableScreen">Set this to true if the 'Admin Grants Receivable' screen is calling this Method.</param>
        public static void PopulateComboBoxes(TCmbAutoPopulated ACostCentreComboBox,
            TCmbAutoPopulated AAccountCodeComboBox,
            TCmbAutoPopulated ADrAccountCodeComboBox,
            Int32 ALedgerNumber,
            bool ACalledByReceivableScreen)
        {
            string filter = String.Empty;

            DataTable CostCentreListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList, ALedgerNumber);

            CostCentreListTable.DefaultView.Sort = ACostCentreTable.GetCostCentreNameDBName() + " ASC";

            if (ACalledByReceivableScreen)
            {
                filter = ACostCentreTable.GetPostingCostCentreFlagDBName() + " = true AND " +
                         ACostCentreTable.GetCostCentreTypeDBName() + " = 'Local'";
            }
            else
            {
                filter = ACostCentreTable.GetPostingCostCentreFlagDBName() + " = true";
            }

            ACostCentreComboBox.InitialiseUserControl(CostCentreListTable,
                ACostCentreTable.GetCostCentreCodeDBName(), ACostCentreTable.GetCostCentreNameDBName(), null);
            ACostCentreComboBox.AppearanceSetup(new int[] { -1, 300 }, 20);
            ACostCentreComboBox.Filter = filter;

            DataTable AccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, ALedgerNumber);
            AccountListTable.DefaultView.Sort = AAccountTable.GetAccountCodeDBName() + " ASC";
            filter = AAccountTable.GetPostingStatusDBName() + " = true AND " +
                     AAccountTable.GetAccountTypeDBName().ToUpper() + " = 'INCOME'";
            AAccountCodeComboBox.InitialiseUserControl(AccountListTable,
                AAccountTable.GetAccountCodeDBName(), AAccountTable.GetAccountCodeShortDescDBName(), null);
            AAccountCodeComboBox.AppearanceSetup(new int[] { -1, 300 }, 20);
            AAccountCodeComboBox.Filter = filter;

            DataTable DrAccountListTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, ALedgerNumber);
            DrAccountListTable.DefaultView.Sort = AAccountTable.GetAccountCodeDBName() + " ASC";
            filter = AAccountTable.GetPostingStatusDBName() + " = true AND " +
                     AAccountTable.GetAccountTypeDBName().ToUpper() + " = 'EXPENSE'";
            ADrAccountCodeComboBox.InitialiseUserControl(DrAccountListTable,
                AAccountTable.GetAccountCodeDBName(), AAccountTable.GetAccountCodeShortDescDBName(), null);
            ADrAccountCodeComboBox.AppearanceSetup(new int[] { -1, 300 }, 20);
            ADrAccountCodeComboBox.Filter = filter;
        }

        /// <summary>
        /// Performs actions that need to happen once the user changes the value of the 'Charge Option' ComboBox.
        /// </summary>
        /// <param name="AAChargeOptionComboBox">'Charge Option' ComboBox Control</param>
        /// <param name="ADetailChargeAmountLabel">'Charge Amount' Label Control</param>
        /// <param name="ADetailChargeAmountTextBox">'Charge Amount' TextBox Control</param>
        /// <param name="ADetailChargePercentageTextBox">'Charge Percentage' TextBox Control</param>
        public static void ChargeOptionComboChanged(TCmbAutoComplete AAChargeOptionComboBox, Label ADetailChargeAmountLabel,
            TTxtCurrencyTextBox ADetailChargeAmountTextBox, TTxtNumericTextBox ADetailChargePercentageTextBox)
        {
            ADetailChargeAmountLabel.Text = AAChargeOptionComboBox.GetSelectedString() + Catalog.GetString(" Amount:");
            ADetailChargeAmountTextBox.Enabled = true;
            ADetailChargePercentageTextBox.Enabled = true;

            switch (AAChargeOptionComboBox.SelectedIndex)
            {
                case 2:
                    ADetailChargePercentageTextBox.Enabled = false;
                    ADetailChargePercentageTextBox.NumberValueDecimal = (decimal)0.0;

                    break;

                case 3:
                    ADetailChargeAmountLabel.Text = Catalog.GetString("Amount:");      // overwrite what was assigned earlier on
                    ADetailChargeAmountTextBox.Enabled = false;                         // overwrite what was assigned earlier on
                    ADetailChargeAmountTextBox.NumberValueDecimal = (decimal)0.0;

                    break;
            }
        }
    }
}