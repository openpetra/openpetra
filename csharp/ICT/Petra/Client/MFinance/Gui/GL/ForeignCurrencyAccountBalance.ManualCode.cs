//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TFrmForeignCurrencyAccountBalance
    {
        private Int32 FLedgerNumber;
        private DataTable FForeignCurrencyAccountsDT = new DataTable();

        /// <summary>
        /// The applicable Ledger number
        /// </summary>
        public Int32 LedgerNumber
        {
            get
            {
                return FLedgerNumber;
            }

            set
            {
                FLedgerNumber = value;
                FFilter = FLedgerNumber;

                LoadDataAndFinishScreenSetup();

                // set up combobox with accounts available for set up as suspense accounts
                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);
            }
        }

        private void InitializeManualCode()
        {
            // don't show the toolstrip with the save button as this screen can never be saved
            tbrMain.Visible = false;
        }

        private void RunOnceOnActivationManual()
        {
            GetData();
            SetupGrid();

            SelectRowInGrid(1);
            UpdateRecordNumberDisplay();
        }

        private void GetData()
        {
            FMainDS.AAccount.DefaultView.RowFilter = "a_foreign_currency_flag_l = 'true'";

            FForeignCurrencyAccountsDT.Clear();
            FForeignCurrencyAccountsDT = FMainDS.AAccount.Clone();
            FForeignCurrencyAccountsDT.Merge(FMainDS.AAccount.DefaultView.ToTable());

            Int32 Year = (Int32)TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber).Rows[0][ALedgerTable.GetCurrentFinancialYearDBName()];

            // get additional columns from database
            TRemote.MFinance.Common.ServerLookups.WebConnectors.GetForeignCurrencyAccountActuals(ref FForeignCurrencyAccountsDT, FLedgerNumber, Year);

            DataView myDataView = FForeignCurrencyAccountsDT.DefaultView;
            myDataView.AllowNew = false;
            myDataView.RowFilter = "a_foreign_currency_flag_l = 'true'";
            myDataView.Sort = AAccountTable.GetForeignCurrencyCodeDBName() + " ASC, " + AAccountTable.GetAccountCodeDBName() + " ASC";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
        }

        private void SetupGrid()
        {
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn(Catalog.GetString("Currency Code"), FMainDS.AAccount.ColumnForeignCurrencyCode);
            grdDetails.AddTextColumn(Catalog.GetString("Account Code"), FMainDS.AAccount.ColumnAccountCode);
            grdDetails.AddTextColumn(Catalog.GetString("Description"), FMainDS.AAccount.ColumnAccountCodeShortDesc);
            grdDetails.AddCurrencyColumn(Catalog.GetString("YTD Total"),
                FForeignCurrencyAccountsDT.Columns[AGeneralLedgerMasterTable.GetYtdActualForeignDBName()]);
            grdDetails.AddCurrencyColumn(Catalog.GetString("YTD Total (BASE)"),
                FForeignCurrencyAccountsDT.Columns[AGeneralLedgerMasterTable.GetYtdActualBaseDBName()]);
        }

        private void EditAccount(System.Object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                TFrmGLAccountHierarchy GLAccountHierarchy = new TFrmGLAccountHierarchy(this);
                GLAccountHierarchy.LedgerNumber = FLedgerNumber;
                GLAccountHierarchy.SetSelectedAccountCode(FPreviouslySelectedDetailRow.AccountCode);
                GLAccountHierarchy.Show();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // select either the given account code or (if it is no longer in the grid) the first row in the grid
        private void SelectAccountInGrid(string AAccountCode)
        {
            FPreviouslySelectedDetailRow = null;

            foreach (DataRowView RowView in FForeignCurrencyAccountsDT.DefaultView)
            {
                if (RowView[AAccountTable.GetAccountCodeDBName()].ToString() == AAccountCode)
                {
                    grdDetails.SelectRowInGrid(grdDetails.Rows.DataSourceRowToIndex(RowView) + 1);
                    return;
                }
            }

            SelectRowInGrid(1);
        }

        #region Forms Messaging Interface Implementation

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>This screen 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>Returns True if the Form reacted on the specific Forms Message,
        /// otherwise false.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcAccountsChanged)
            {
                string CurrentlySelectedAccountCode = FPreviouslySelectedDetailRow.AccountCode;
                Type DataTableType;

                // Load Data
                DataTable CacheDT = TDataCache.GetSpecificallyFilteredCacheableDataTableFromCache("AccountList", "Ledger", FFilter, out DataTableType);
                FMainDS.AAccount.Merge(CacheDT);

                GetData();

                // reapply filter
                FFilterAndFindObject.ApplyFilter();

                // reselect the last selected account code
                if (FPreviouslySelectedDetailRow != null)
                {
                    SelectAccountInGrid(CurrentlySelectedAccountCode);
                }

                UpdateRecordNumberDisplay();

                MessageProcessed = true;
            }

            return MessageProcessed;
        }

        #endregion
    }
}