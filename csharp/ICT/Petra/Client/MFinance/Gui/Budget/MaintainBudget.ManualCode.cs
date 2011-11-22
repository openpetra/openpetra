//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
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
using Ict.Petra.Shared.Interfaces;
using Ict.Petra.Shared.Interfaces.MFinance.Budget.WebConnectors;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Budget
{
	
    public partial class TFrmMaintainBudget
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// AP is opened in this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                
                FMainDS = TRemote.MFinance.Budget.WebConnectors.LoadBudget(FLedgerNumber);
                
                                // to get an empty ABudgetFee table, instead of null reference
                FMainDS.Merge(new BudgetTDS());

                TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, FLedgerNumber, true, false, false, false);

                // Do not include summary cost centres: we want to use one cost centre for each Motivation Details
                TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, FLedgerNumber, true, false, false, true);

                DataView myDataView = FMainDS.ABudget.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdDetails.AutoSizeCells();

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";
            }
        }
        
        private void NewRowManual(ref ABudgetRow ARow)
        {
            int newSequence = -1;

            if (FMainDS.ABudget.Rows.Find(new object[] { newSequence }) != null)
            {
                while (FMainDS.ABudget.Rows.Find(new object[] { newSequence }) != null)
                {
                    newSequence--;
                }
            }

            ARow.BudgetSequence = newSequence;
            ARow.LedgerNumber = FLedgerNumber;
            //TODO replace line below
            ARow.Revision = Math.Abs(newSequence);
            //ARow.Year = 2010
        }


        private TSubmitChangesResult StoreManualCode(ref BudgetTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult TSCR = TRemote.MFinance.Budget.WebConnectors.SaveBudget(ref ASubmitChanges, out AVerificationResult);

            return TSCR;
        }
        
        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewABudget();
        }


        private void DeleteRow(System.Object sender, EventArgs e)
        {
            if (FPreviouslySelectedDetailRow == null)
            {
                return;
            }

            //TODO need to create CheckDeleteABudgetSequence
            int num = 0; //TRemote.MFinance.Budget.WebConnectors.CheckDeleteABudgetSequence(FPreviouslySelectedDetailRow.BudgetSequence);

            if (num > 0)
            {
                MessageBox.Show(Catalog.GetString("This budget is already referenced and cannot be deleted."));
                return;
            }

            bool isBudgetUsed = false;
            
            for (int i = 0; i < 13; i++)
            {
	            if (FMainDS.ABudgetPeriod.Rows.Find(new object[] { FPreviouslySelectedDetailRow.BudgetSequence, i }) != null)
	            {
	            	isBudgetUsed = true;
	            	break;
	            }
            }
            
			if (isBudgetUsed) 
			{
                MessageBox.Show(Catalog.GetString("Budget figures exist for last year and this year. The budget cannot be deleted. Do you wish to cancel the budget figures for next year?"));
                return;
			}            
            
            if ((FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                || (MessageBox.Show(String.Format(Catalog.GetString(
                                "You have chosen to delete this budget (Cost Centre: {0}, Account: {1}, Type: {2}, Revision: {3}).\n\nDo you really want to delete it?"),
                            FPreviouslySelectedDetailRow.CostCentreCode,
                            FPreviouslySelectedDetailRow.AccountCode,
                            FPreviouslySelectedDetailRow.BudgetTypeCode,
                            FPreviouslySelectedDetailRow.Revision), Catalog.GetString("Confirm Delete"),
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                int rowIndex = CurrentRowIndex();
                FPreviouslySelectedDetailRow.Delete();
                FPetraUtilsObject.SetChangedFlag();
                SelectByIndex(rowIndex);
            }

        }


        private int CurrentRowIndex()
        {
            int rowIndex = -1;

            SourceGrid.RangeRegion selectedRegion = grdDetails.Selection.GetSelectionRegion();

            if ((selectedRegion != null) && (selectedRegion.GetRowsIndex().Length > 0))
            {
                rowIndex = selectedRegion.GetRowsIndex()[0];
            }

            return rowIndex;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }

        private void BudgetTypeChanged(System.Object sender, EventArgs e)
        {
        	
        }
        
        private void TotalSplitAmountChanged(System.Object sender, EventArgs e)
        {
        	decimal annualAmount = 0;
        	decimal perPeriodAmount = 0;
        	decimal remainder = 0;
        	
        	TTxtNumericTextBox txt = (TTxtNumericTextBox)sender;
        	txt.CurrencySymbol = "";
        	//string strAmount = txt.Text.Substring(0,
        	if(Decimal.TryParse(txt.Text, out annualAmount))
        	{
        		perPeriodAmount = Math.Truncate(annualAmount / 12);
        		remainder = annualAmount - perPeriodAmount * 12;
        	} 
        	else
        	{
        		MessageBox.Show("Not numeric");
        	}
        	
        	txtPerPeriodAmount.Text = perPeriodAmount.ToString();
        	txtPeriod12Amount.Text = (perPeriodAmount + remainder).ToString();
        }

        private void ShowDetailsManual(ABudgetRow ARow)
        {
	            if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_SPLIT)
	            {
		            rbtSplit.Checked = true;
	            }
	            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_ADHOC)
	            {
		            rbtAdHoc.Checked = true;
	            }
	            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_SAME)
	            {
		            rbtSame.Checked = true;
	            }
	            else if (ARow.BudgetTypeCode == MFinanceConstants.BUDGET_INFLATE_BASE)
	            {
		            rbtInflateBase.Checked = true;
	            }
	            else  //ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_N
	            {
		            rbtInflateN.Checked = true;
	            }
	            
//	            if (!grpBudgetDetails.AutoSize)
//	            {
//		            grpBudgetDetails.AutoSize = true;	
//	            }
	            
                pnlBudgetTypeAdhoc.Visible = rbtAdHoc.Checked;
	            pnlBudgetTypeSame.Visible = rbtSame.Checked;
	            pnlBudgetTypeSplit.Visible = rbtSplit.Checked;
	            pnlBudgetTypeInflateN.Visible = rbtInflateN.Checked;
	            pnlBudgetTypeInflateBase.Visible = rbtInflateBase.Checked;	
        }
        
        private bool GetDetailDataFromControlsManual(ABudgetRow ARow)
	    {
	        if (ARow != null)
	        {
	            ARow.BeginEdit();
	            if (rbtSplit.Checked)
	            {
		            ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SPLIT;
	            }
	            else if (rbtAdHoc.Checked)
	            {
		            ARow.BudgetTypeCode = MFinanceConstants.BUDGET_ADHOC;
	            }
	            else if (rbtSame.Checked)
	            {
		            ARow.BudgetTypeCode = MFinanceConstants.BUDGET_SAME;
	            }
	            else if (rbtInflateBase.Checked)
	            {
		            ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_BASE;
	            }
	            else  //Inf. n
	            {
		            ARow.BudgetTypeCode = MFinanceConstants.BUDGET_INFLATE_N;
	            }
	            
	            //TODO switch to using Ledger financial year
	            ARow.Year = 2012;
	            ARow.Revision = CreateBudgetRevisionRow(FLedgerNumber, ARow.Year);
	            ARow.EndEdit();
	        }
	
	        return true;
	    }
        
        
        private int CreateBudgetRevisionRow(int ALedgerNumber, int AYear)
        {
        	
        	int newRevision = 0;

            if (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, newRevision }) == null)
            {
	        	ABudgetRevisionRow BudgetRevisionRow = FMainDS.ABudgetRevision.NewRowTyped();
	        	
	        	BudgetRevisionRow.LedgerNumber = ALedgerNumber;
	        	BudgetRevisionRow.Year = AYear;
	
	        	BudgetRevisionRow.Revision = newRevision;
	        	FMainDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
	
	        	//TODO check with Rob about budget versioning
	        	//        	while (FMainDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, AYear, newRevision }) != null)
				//                {
				//                    newRevision++;
				//                }
            }
        	
        	return newRevision;
        }
        
        
    }
}