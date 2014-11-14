//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert, alanP
//
// Copyright 2004-2014 by OM International
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

using Ict.Common.Data;

using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    /// <summary>
    /// A business logic class that handles the account and cost centre code
    /// </summary>
    public class TUC_GiftBatches_AccountAndCostCentre
    {
        private Int32 FLedgerNumber = 0;
        private GiftBatchTDS FMainDS = null;
        private TCmbAutoPopulated FCmbBankAccountCode = null;
        private TCmbAutoPopulated FCmbCostCentreCode = null;

        private AAccountTable FAccountTable = null;
        private ACostCentreTable FCostCentreTable = null;

        #region Properties

        /// <summary>
        /// Gets the Account table
        /// </summary>
        public AAccountTable AccountTable
        {
            get
            {
                return FAccountTable;
            }
        }

        /// <summary>
        /// Gets the Cost Centre table
        /// </summary>
        public ACostCentreTable CostCentreTable
        {
            get
            {
                return FCostCentreTable;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TUC_GiftBatches_AccountAndCostCentre(Int32 ALedgerNumber,
            GiftBatchTDS AMainDS,
            TCmbAutoPopulated ACmbBankAccountCode,
            TCmbAutoPopulated ACmbCostCentreCode)
        {
            FLedgerNumber = ALedgerNumber;
            FMainDS = AMainDS;

            FCmbBankAccountCode = ACmbBankAccountCode;
            FCmbCostCentreCode = ACmbCostCentreCode;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns true if the specified account code is active
        /// </summary>
        /// <param name="AAccountCode">The account code string to test.  If not supplied the code is taken from the current combo box item</param>
        /// <returns>Returns true if the specified account code is active</returns>
        public bool AccountIsActive(string AAccountCode = "")
        {
            bool retVal = true;

            AAccountRow currentAccountRow = null;

            //If empty, read value from combo
            if (AAccountCode == string.Empty)
            {
                if ((FAccountTable != null) && (FCmbBankAccountCode.SelectedIndex != -1) && (FCmbBankAccountCode.Count > 0)
                    && (FCmbBankAccountCode.GetSelectedString() != null))
                {
                    AAccountCode = FCmbBankAccountCode.GetSelectedString();
                }
            }

            if (FAccountTable != null)
            {
                currentAccountRow = (AAccountRow)FAccountTable.Rows.Find(new object[] { FLedgerNumber, AAccountCode });
            }

            if (currentAccountRow != null)
            {
                retVal = currentAccountRow.AccountActiveFlag;
            }

            return retVal;
        }

        /// <summary>
        /// Returns true if the specified cost centre code is active
        /// </summary>
        /// <param name="ACostCentreCode">The account code string to test.  If not supplied the code is taken from the current combo box item</param>
        /// <returns>Returns true if the specified cost centre code is active</returns>
        public bool CostCentreIsActive(string ACostCentreCode = "")
        {
            bool retVal = true;

            ACostCentreRow currentCostCentreRow = null;

            //If empty, read value from combo
            if (ACostCentreCode == string.Empty)
            {
                if ((FCostCentreTable != null) && (FCmbCostCentreCode.SelectedIndex != -1) && (FCmbCostCentreCode.Count > 0)
                    && (FCmbCostCentreCode.GetSelectedString() != null))
                {
                    ACostCentreCode = FCmbCostCentreCode.GetSelectedString();
                }
            }

            if (FCostCentreTable != null)
            {
                currentCostCentreRow = (ACostCentreRow)FCostCentreTable.Rows.Find(new object[] { FLedgerNumber, ACostCentreCode });
            }

            if (currentCostCentreRow != null)
            {
                retVal = currentCostCentreRow.CostCentreActiveFlag;
            }

            return retVal;
        }

        /// <summary>
        /// Call this to initialse the 'Lists' (tables) for the ComboBoxes
        /// </summary>
        /// <param name="ALoadAndFilterLogicObject">Supply a reference to the Filter Logic object because it needs a reference to the same Lists</param>
        public void RefreshBankAccountAndCostCentreData(TUC_GiftBatches_LoadAndFilter ALoadAndFilterLogicObject)
        {
            //Populate CostCentreList variable
            DataTable costCentreList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.CostCentreList,
                FLedgerNumber);

            ACostCentreTable tmpCostCentreTable = new ACostCentreTable();

            FMainDS.Tables.Add(tmpCostCentreTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref costCentreList, FMainDS.Tables[tmpCostCentreTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpCostCentreTable.TableName);

            FCostCentreTable = (ACostCentreTable)costCentreList;
            ALoadAndFilterLogicObject.CostCentreTable = FCostCentreTable;

            //Populate AccountList variable
            DataTable accountList = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            AAccountTable tmpAccountTable = new AAccountTable();
            FMainDS.Tables.Add(tmpAccountTable);
            DataUtilities.ChangeDataTableToTypedDataTable(ref accountList, FMainDS.Tables[tmpAccountTable.TableName].GetType(), "");
            FMainDS.RemoveTable(tmpAccountTable.TableName);

            FAccountTable = (AAccountTable)accountList;
            ALoadAndFilterLogicObject.AccountTable = FAccountTable;
        }

        /// <summary>
        /// Call this to do initial set up the Bank account and cost centre combo boxes
        /// </summary>
        /// <param name="AActiveOnly"></param>
        /// <param name="ARow"></param>
        public void SetupAccountAndCostCentreCombos(bool AActiveOnly, AGiftBatchRow ARow)
        {
            FCmbCostCentreCode.Clear();
            FCmbBankAccountCode.Clear();
            TFinanceControls.InitialiseAccountList(ref FCmbBankAccountCode, FLedgerNumber, true, false, AActiveOnly, true, true, FAccountTable);
            TFinanceControls.InitialiseCostCentreList(ref FCmbCostCentreCode, FLedgerNumber, true, false, AActiveOnly, true, true, FCostCentreTable);

            if (ARow != null)
            {
                FCmbCostCentreCode.SetSelectedString(ARow.BankCostCentre, -1);
                FCmbBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);
            }
        }

        /// <summary>
        /// Refreshes the filters on the combo boxes
        /// </summary>
        /// <param name="AActiveOnly"></param>
        /// <param name="ARow"></param>
        public void RefreshBankAccountAndCostCentreFilters(bool AActiveOnly, AGiftBatchRow ARow)
        {
            FCmbBankAccountCode.Filter = TFinanceControls.PrepareAccountFilter(true, false, AActiveOnly, true, "");
            FCmbCostCentreCode.Filter = TFinanceControls.PrepareCostCentreFilter(true, false, AActiveOnly, true);

            if (ARow != null)
            {
                FCmbCostCentreCode.SetSelectedString(ARow.BankCostCentre, -1);
                FCmbBankAccountCode.SetSelectedString(ARow.BankAccountCode, -1);
            }
        }

        #endregion
    }
}