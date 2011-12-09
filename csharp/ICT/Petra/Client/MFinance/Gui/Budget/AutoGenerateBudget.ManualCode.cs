//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
//
// Copyright 2004-2011 by OM International
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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance.Budget.WebConnectors;
using Ict.Petra.Shared.MFinance;
//using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
    public partial class TFrmAutoGenerateBudget
    {
        private Int32 FLedgerNumber;
        

        private Ict.Petra.Shared.MFinance.GL.Data.BudgetTDS FMainDS;
        

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

		        FMainDS = TRemote.MFinance.Budget.WebConnectors.LoadBudget(FLedgerNumber);
		        
		        InitialiseBudgetList(FMainDS.ABudget);
		        
	            //TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbSelectBudgetYear, FLedgerNumber, true);

	            //TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                //TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);
                

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";
            }
        }

        private void InitialiseBudgetList(ABudgetTable ABdgTable)
        {

            string CheckedMember = "CHECKED";
            string AccountDBN = ABudgetTable.GetAccountCodeDBName();
            string CostCentreDBN = ABudgetTable.GetCostCentreCodeDBName();

            // add empty row so that SetSelectedString for invalid string will not result in undefined behaviour (selecting the first cost centre etc)
            ABudgetRow emptyRow = (ABudgetRow)ABdgTable.NewRow();

//            emptyRow[ABudgetTable.ColumnBudgetSequenceId] = -1000;
//            emptyRow[ABudgetTable.ColumnYearId] = 2010;
//            emptyRow[ABudgetTable.ColumnLedgerNumberId] = FLedgerNumber;
//            emptyRow[ABudgetTable.ColumnCostCentreCodeId] = string.Empty;
//            emptyRow[ABudgetTable.ColumnAccountCodeId] = string.Empty; //Catalog.GetString("Select a valid cost centre/account combination");
//            ABdgTable.Rows.Add(emptyRow);

            DataView view = new DataView(ABdgTable);
            DataTable ABdgTable2 = view.ToTable(true, new string[] { AccountDBN, CostCentreDBN });
            ABdgTable2.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbCostCentreAccountCodes.Columns.Clear();
            clbCostCentreAccountCodes.AddCheckBoxColumn("", ABdgTable2.Columns[CheckedMember], 17, false);
            clbCostCentreAccountCodes.AddTextColumn(Catalog.GetString("Cost Centre"), ABdgTable2.Columns[CostCentreDBN], 75);
            clbCostCentreAccountCodes.AddTextColumn(Catalog.GetString("Account"), ABdgTable2.Columns[AccountDBN], 100);
            clbCostCentreAccountCodes.DataBindGrid(ABdgTable2, CostCentreDBN, CheckedMember, CostCentreDBN, AccountDBN, false, true, false);

            clbCostCentreAccountCodes.SetCheckedStringList("");
        }
        
        private void GenerateBudget(Object sender, EventArgs e)
        {

        }

        private void NewBudgetScope(Object sender, EventArgs e)
        {
        	if(rbtAllBudgets.Checked)
        	{
        		clbCostCentreAccountCodes.Enabled = false;
        	}
        	else
        	{
        		clbCostCentreAccountCodes.Enabled = true;
        	}
        	
        }

        private void NewRemainingPeriod(Object sender, EventArgs e)
        {

        }

        private void CloseForm(System.Object sender, EventArgs e)
        {
        	Close();
        }

        private void UnselectAllBudgets(System.Object sender, EventArgs e)
        {
        	clbCostCentreAccountCodes.ClearSelected();
        }
        
    }
}