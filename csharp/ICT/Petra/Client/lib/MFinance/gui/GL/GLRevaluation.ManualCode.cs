//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Data;

using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;

using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.CommonControls;

using Ict.Petra.Shared.MFinance;



namespace Ict.Petra.Client.MFinance.Gui.GL
{
	/// <summary>
	/// Description of GLRevaluation_ManualCode.
	/// </summary>
	public partial class TGLRevaluation
	{
		
		private const string REVALUATIONCOSTCENTRE = "REVALUATIONCOSTCENTRE";


		private Int32 FLedgerNumber;
        
        private DateTime DefaultDate;
        private DateTime StartDateCurrentPeriod;
        private DateTime EndDateLastForwardingPeriod;
        
        TFrmSetupDailyExchangeRate tFrmSetupDailyExchangeRate;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                
                Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAccountList;
                cmbAccountList = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
                

                GetListOfRevaluationCurrencies();
                

                TFinanceControls.InitialiseCostCentreList(ref cmbCostCenter, FLedgerNumber,
                                                   true, false, true, false);
			
                LoadUserDefaults();

                TLedgerSelection.GetCurrentPostingRangeDates(FLedgerNumber,
                                                             out StartDateCurrentPeriod,
                                                             out EndDateLastForwardingPeriod,
                                                             out DefaultDate);
                this.lblAccountText.Text = "Account Number:";
                this.lblAccountValue.Text = FLedgerNumber.ToString();
                
                lblDateStart.Text = "Start Date:";
                lblDateStartValue.Text = StartDateCurrentPeriod.ToLongDateString();
                lblDateEnd.Text = "End Date:";
                lblDateEndValue.Text = EndDateLastForwardingPeriod.ToLongDateString();
                
//                tFrmSetupDailyExchangeRate = new TFrmSetupDailyExchangeRate(this);
//                tFrmSetupDailyExchangeRate.LedgerNumber = FLedgerNumber;
//                // tFrmSetupDailyExchangeRate.SetDataFilters(
                
                
            }
        }
        
        private void GetListOfRevaluationCurrencies()
        {
                DataTable table = TDataCache.TMFinance.GetCacheableFinanceTable(
                	TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);
                
                int ic = 0;

                foreach (DataRow row in table.Rows)
                {
                	bool blnIsLedger = (FLedgerNumber == (int)row["a_ledger_number_i"]);
                	bool blnAccountActive = (bool)row["a_account_active_flag_l"];
                	bool blnAccountForeign = (bool)row["a_foreign_currency_flag_l"];
                	bool blnAccountHasPostings = (bool)row["a_posting_status_l"];
                	
                	if (blnIsLedger && blnAccountActive && 
                	    blnAccountForeign && blnAccountHasPostings)
                	{
                		++ic;
                		// accountTable.Rows.Add(row);
                	}
                }
                
                System.Diagnostics.Debug.WriteLine(ic.ToString());
        }
	
		
        private void SaveUserDefaults()
        {
        	TUserDefaults.SetDefault(REVALUATIONCOSTCENTRE, cmbCostCenter.GetSelectedString());
        }
        
        private void LoadUserDefaults()
        {
        	try {
        		cmbCostCenter.SetSelectedString(
        			TUserDefaults.GetStringDefault(REVALUATIONCOSTCENTRE));
        		} catch (Exception) {}        	
        }
		
        private void CancelRevaluation(object btn, EventArgs e)
		{
			this.Close();
		}
		
		private void RunRevaluation(object btn, EventArgs e)
		{
			SaveUserDefaults();
			this.Close();
		}

	}
}
