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
using Ict.Common.DB;
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
using Ict.Petra.Server.MFinance.Account.Data.Access;

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
		        
		        ALedgerTable LedgerTable = FMainDS.ALedger;
		        ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows.Find(new object[] {FLedgerNumber});
		        int ForecastEndPeriod = LedgerRow.CurrentPeriod - 1;
		        
		        txtForecast.Text = ForecastEndPeriod.ToString();
					
	            //TFinanceControls.InitialiseAvailableFinancialYearsList(ref cmbSelectBudgetYear, FLedgerNumber, true);

	            //TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                //TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);
                

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";
            }
        }

        
        private int CostCentrePadding = 0;
        private string CurrentCheckedList = string.Empty;
        
        private void InitialiseBudgetList(ABudgetTable ABdgTable)
        {

            string CheckedMember = "CHECKED";
            string AccountDBN = ABudgetTable.GetAccountCodeDBName();
            string CostCentreDBN = ABudgetTable.GetCostCentreCodeDBName();
            string BudgetSeqDBN = ABudgetTable.GetBudgetSequenceDBName();
            string CCAccKey = "CostCentreAccountKey";
            string CCAccDesc = "CostCentreAccountDescription";
            string BudgetSeqKey = "BudgetSequenceKey";

            //Calculate the longest Cost Centre to calculate padding amount
            ABudgetRow BudgetRow;
            int CostCentreCodeLength = 0;
            for (int i = 0; i < ABdgTable.Count; i++)
            {
            	BudgetRow = (ABudgetRow)ABdgTable.Rows[i];
            	CostCentreCodeLength = BudgetRow.CostCentreCode.Length;
            	if (CostCentreCodeLength > CostCentrePadding)
            	{
            		CostCentrePadding = CostCentreCodeLength;
            	}
            }
            BudgetRow = null;
            
            // add empty row so that SetSelectedString for invalid string will not result in undefined behaviour (selecting the first cost centre etc)
            ABudgetRow emptyRow = (ABudgetRow)ABdgTable.NewRow();

//            emptyRow[ABudgetTable.ColumnBudgetSequenceId] = -1000;
//            emptyRow[ABudgetTable.ColumnYearId] = 2010;
//            emptyRow[ABudgetTable.ColumnLedgerNumberId] = FLedgerNumber;
//            emptyRow[ABudgetTable.ColumnCostCentreCodeId] = string.Empty;
//            emptyRow[ABudgetTable.ColumnAccountCodeId] = string.Empty; //Catalog.GetString("Select a valid cost centre/account combination");
//            ABdgTable.Rows.Add(emptyRow);

            DataView view = new DataView(ABdgTable);
            DataTable ABdgTable2 = view.ToTable(true, new string[] { BudgetSeqDBN, AccountDBN, CostCentreDBN });
            ABdgTable2.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
            /*ABdgTable2.Columns.Add(new DataColumn(CCAccKey, typeof(string), CostCentreDBN + " + '-' + " + AccountDBN));*/
            ABdgTable2.Columns.Add(new DataColumn(BudgetSeqKey, typeof(string), BudgetSeqDBN));
            ABdgTable2.Columns.Add(new DataColumn(CCAccDesc, typeof(string), CostCentreDBN.PadRight(CostCentrePadding + 2 ,' ') + " + '-' + " + AccountDBN));

            clbCostCentreAccountCodes.Columns.Clear();
            clbCostCentreAccountCodes.AddCheckBoxColumn("", ABdgTable2.Columns[CheckedMember], 17, false);
            clbCostCentreAccountCodes.AddTextColumn("Key", ABdgTable2.Columns[BudgetSeqKey], 0);
            clbCostCentreAccountCodes.AddTextColumn("Cost Centre-Account", ABdgTable2.Columns[CCAccDesc], 200);
            clbCostCentreAccountCodes.DataBindGrid(ABdgTable2, BudgetSeqKey, CheckedMember, BudgetSeqKey, CCAccDesc, false, true, false);

			/*clbCostCentreAccountCodes.Columns.Clear();
            clbCostCentreAccountCodes.AddCheckBoxColumn("", ABdgTable2.Columns[CheckedMember], 17, false);
            clbCostCentreAccountCodes.AddTextColumn("Key", ABdgTable2.Columns[CCAccKey], 0);
            clbCostCentreAccountCodes.AddTextColumn("Cost Centre-Account", ABdgTable2.Columns[CCAccDesc], 200);
            clbCostCentreAccountCodes.DataBindGrid(ABdgTable2, CCAccKey, CheckedMember, CCAccKey, CCAccDesc, false, true, false);*/
            //clbCostCentreAccountCodes.AddTextColumn(Catalog.GetString("Cost Centre/Account"), ABdgTable2.Columns[CostCentreDBN], 150);
            //clbCostCentreAccountCodes.AddTextColumn(Catalog.GetString("Account"), ABdgTable2.Columns[AccountDBN], 100);
            //clbCostCentreAccountCodes.DataBindGrid(ABdgTable2, CCAccKey, CheckedMember, CCAccKey, AccountDBN, false, true, false);

            clbCostCentreAccountCodes.SetCheckedStringList("");
        }
        
        private void GenerateBudget(Object sender, EventArgs e)
        {
			int lv_counter_i = 0;
        	
        	string msg = string.Empty;
			
			msg = "You can either consolidate all of your budgets";
			msg += " or just those that have changed since the last consolidation." + "\n\r\n\r";
			msg += "Do you want to consolidate all of your budgets?";
				
			bool ConsolidateAll = (MessageBox.Show(msg,"Consolidate Budgets",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly,false) == DialogResult.Yes);

	        //TODO: call code on the server. To be completed with Timo.
	        //TODO: Don't forget to examine code in lb_budget.p for the contents of functions ClearBudgets, StartConsolidation etc...
        	//ConsolidateBudgets();
       		string CheckItemsList = clbCostCentreAccountCodes.GetCheckedStringList();
       		string[] CheckedItems = CheckItemsList.Split(',');

       		if (rbtSelectedBudgets.Checked && CheckItemsList.Length > 0)
        	{
        		lv_counter_i = 1;
        		foreach (string BudgetItem in CheckedItems)
        		{
		            /* Generate report. Parameters are recid of the budget and the forecast type.
		            RUN gb4000.p (RECID(a_budget), rad_forecast_type_c:SCREEN-VALUE).*/
		            int BudgetItemNo = Convert.ToInt32(BudgetItem)
		            GenBudgetForNextYear(BudgetItemNo);
        		}
        	}
			else
			{

			}
			
        }

        
        private void GenBudgetForNextYear(int ABudgetSeq)
        {
        	int lv_glm_sequence_this_year_i = 0;
			int lv_glm_sequence_last_year_i = 0;

			
        }
        
        private int GetGLMSequence(int ALedgerNumber, string AAccountCode, string CostCentreCode, int Year)
        {
        	
        }
        
        
        
		//This flag is needed to stop the event occuring twice for each
		//change of the option
        private bool AllBudgetsWasLastSelected = false;
        private void NewBudgetScope(Object sender, EventArgs e)
        {
        	if (rbtAllBudgets.Checked && !AllBudgetsWasLastSelected)
        	{
        		AllBudgetsWasLastSelected = true;
        		CurrentCheckedList = clbCostCentreAccountCodes.GetCheckedStringList();
				
        		SelectAll();

        		btnSelectAllBudgets.Enabled = false;
        		btnUnselectAllBudgets.Enabled = false;
        		clbCostCentreAccountCodes.Enabled = false;
        	}
        	else if (!rbtAllBudgets.Checked && AllBudgetsWasLastSelected)
        	{
        		AllBudgetsWasLastSelected = false;
        		btnSelectAllBudgets.Enabled = true;
        		btnUnselectAllBudgets.Enabled = true;
        		clbCostCentreAccountCodes.Enabled = true;
        		clbCostCentreAccountCodes.SetCheckedStringList(CurrentCheckedList);
				clbCostCentreAccountCodes.SelectRowInGrid(1);
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
        
        private void SelectAllBudgets(System.Object sender, EventArgs e)
        {
        	SelectAll();
        }
        
        private void SelectAll()
        {
    		ABudgetTable BudgetTable = FMainDS.ABudget;
			ABudgetRow BudgetRow;
    		string CheckedList = string.Empty;
    		for (int i = 0; i < BudgetTable.Count; i++)
    		{
    			BudgetRow = (ABudgetRow)BudgetTable.Rows[i];
    			//CheckedList += BudgetRow.CostCentreCode + '-' + BudgetRow.AccountCode + ",";
    			CheckedList += BudgetRow.BudgetSequence.ToString() + ",";
    		}
    		if (CheckedList.Length > 0)
    		{
    			//MessageBox.Show(CheckedList);
    			CheckedList = CheckedList.Substring(0, CheckedList.Length - 1);
        		clbCostCentreAccountCodes.SetCheckedStringList(CheckedList);
				clbCostCentreAccountCodes.SelectRowInGrid(1);
    		}
        	
        }

    }
}