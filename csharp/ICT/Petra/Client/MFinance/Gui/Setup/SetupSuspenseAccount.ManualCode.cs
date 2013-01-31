//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupSuspenseAccount
    {
        private Int32 FLedgerNumber;
        private DataColumn FDescriptionColumn;

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

                // now merge account table into dataset as we need descriptions for account codes
                FMainDS.AAccount.Merge(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList, FLedgerNumber));

                LoadDataAndFinishScreenSetup();
                
                // set up combobox with accounts available for set up as suspense accounts
                TFinanceControls.InitialiseAccountList(ref cmbDetailSuspenseAccountCode, FLedgerNumber, true, false, false, false);
            }
        }

        private void InitializeManualCode()
        {
            // Hook up DataSavingStarted Event to be able to run code before SaveChanges is doing anything
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(this.DataSavingStarted);
            
            // Hook up DataSaved Event to be able to run code after SaveChanges is finished
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(this.OnDataSaved);

            // add column for account description
            AddSpecialColumns();
            
            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Account Code", FMainDS.ASuspenseAccount.ColumnSuspenseAccountCode);
            grdDetails.AddTextColumn("Description", 
                FMainDS.ASuspenseAccount.Columns["Parent_" + AAccountTable.GetAccountCodeShortDescDBName()]);
        }

        /// <summary>
        /// Add columns that were created and are not part of the normal ASuspenseAccount
        /// </summary>
        private void AddSpecialColumns()
        {
            if (FDescriptionColumn == null)
            {
                FDescriptionColumn = new DataColumn();
                FDescriptionColumn.DataType = System.Type.GetType("System.String");
                FDescriptionColumn.ColumnName = "Parent_" + AAccountTable.GetAccountCodeShortDescDBName();
                FDescriptionColumn.Expression = "Parent." + AAccountTable.GetAccountCodeShortDescDBName();
            }

            if (!FMainDS.ASuspenseAccount.Columns.Contains(FDescriptionColumn.ColumnName))
            {
                FMainDS.ASuspenseAccount.Columns.Add(FDescriptionColumn);
            }
        }

        /// <summary>
        /// Remove columns that were created and are not part of the normal ASuspenseAccount.
        /// This is needed e.g. when table contents are to be saved
        /// </summary>
        private void RemoveSpecialColumns()
        {
            if ((FDescriptionColumn != null)
                && FMainDS.ASuspenseAccount.Columns.Contains(FDescriptionColumn.ColumnName))
            {
                FMainDS.ASuspenseAccount.Columns.Remove(FDescriptionColumn);
            }
        }
        
        private void NewRecord(System.Object sender, EventArgs e)
        {
            CreateNewASuspenseAccount();
        }

        private void NewRowManual(ref ASuspenseAccountRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            ARow.SuspenseAccountCode = "";
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            DeleteASuspenseAccount();
        }
        
        
        private void DataSavingStarted(System.Object sender, System.EventArgs e)
        {
            // saving fails if extra columns exist
            RemoveSpecialColumns();
        }

        private void OnDataSaved(System.Object sender, TDataSavedEventArgs e)
        {
            // need to add columns again once it is saved
            AddSpecialColumns();
        }
        
    }
}