//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
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
using System.Data;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MFinance.Logic;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;


namespace Ict.Petra.Client.MFinance.Gui.ICH
{
    /// <summary>
    /// Enums holding the possible reporting period selection modes
    /// </summary>
    public enum TICHReportingPeriodSelectionModeEnum
    {
        /// <summary>
        /// ICH Statement reporting period selection mode
        /// </summary>
        rpsmICHStatement,
        /// <summary>
        /// ICH Stewardship Calculation reporting period selection mode
        /// </summary>
        rpsmICHStewardshipCalc
    }


    /// manual methods for the generated window
    public partial class TFrmReportingPeriodSelectionDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// Field to store the reporting period selection mode
        /// </summary>
        public TICHReportingPeriodSelectionModeEnum FReportingPeriodSelectionMode = TICHReportingPeriodSelectionModeEnum.rpsmICHStewardshipCalc;
        /// <summary>
        /// Field to store the relevant Ledger number
        /// </summary>
        public Int32 FLedgerNumber = 0;

        private void GenerateHOSAFiles(Object Sender, EventArgs e)
        {
            int Currency = (this.rbtBase.Checked ? 0 : 1); //0 = base 1 = intl
            string FileName = string.Empty;
          	string CostCentreCode = string.Empty;
          	bool HOSASuccess = false;
            
            TVerificationResultCollection VerificationResults;

            string msg = string.Empty;
			string SuccessfullCostCentres = string.Empty;
			string FailedCostCentres = string.Empty;
            

            if (!ValidReportPeriod(true))
            {
                return;
            }

            int SelectedReportPeriod = cmbReportPeriod.GetSelectedInt32();
            int SelectedICHNumber = cmbICHNumber.GetSelectedInt32();
            
            try
            {
            	Cursor = Cursors.WaitCursor;

            	DataTable ICHNumbers = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.ICHStewardshipList, FLedgerNumber);
	
				//Filter for current period
	            ICHNumbers.DefaultView.RowFilter = String.Format("{0}={1} And {2}={3}",
	                                                             AIchStewardshipTable.GetPeriodNumberDBName(),
	                                                             SelectedReportPeriod,
	                                                             AIchStewardshipTable.GetIchNumberDBName(),
	                                                             SelectedICHNumber);
				
				ICHNumbers.DefaultView.Sort = AIchStewardshipTable.GetCostCentreCodeDBName();
			
            	foreach (DataRowView tmpRow in ICHNumbers.DefaultView)
            	{
            		AIchStewardshipRow ichRow = (AIchStewardshipRow)tmpRow.Row;
            		
            		CostCentreCode = (string)ichRow[AIchStewardshipTable.ColumnCostCentreCodeId];
            		
            		FileName = TClientSettings.PathTemp + Path.DirectorySeparatorChar + "TestGenHOSAFileFor" + CostCentreCode + ".csv";

					HOSASuccess = TRemote.MFinance.ICH.WebConnectors.GenerateHOSAFiles(FLedgerNumber, cmbReportPeriod.GetSelectedInt32(),
	                        			cmbICHNumber.GetSelectedInt32(), CostCentreCode, Currency, FileName, out VerificationResults);
            			
	                if (HOSASuccess)
	                {
	                	if (SuccessfullCostCentres.Length == 0)
	                	{
	                		SuccessfullCostCentres = CostCentreCode;
	                	}
	                	else
	                	{
	                		SuccessfullCostCentres += ", " + CostCentreCode;
	                	}
	                }
	                else
	                {
	                	if (FailedCostCentres.Length == 0)
	                	{
	                		FailedCostCentres = CostCentreCode;
	                	}
	                	else
	                	{
	                		FailedCostCentres += ", " + CostCentreCode;
	                	}
	                }
	                	
            	}

                Cursor = Cursors.Default;

                if (SuccessfullCostCentres.Length > 0)
            	{
                	msg = String.Format(Catalog.GetString("HOSA file generated successfully for Cost Centre(s):{0}{0}{1}{0}{0}"),
	                	                    Environment.NewLine,
	                	                    SuccessfullCostCentres);
            	}

            	if (FailedCostCentres.Length > 0)
            	{
                	msg += String.Format(Catalog.GetString("HOSA generation FAILED for Cost Centre(s):{0}{0}{1}"),
	                	                    Environment.NewLine,
	                	                    FailedCostCentres);
            	}

            	if (msg.Length == 0)
            	{
            		msg = String.Format(Catalog.GetString("No Cost Centres to process in Ledger {0} for report period: {1} and ICH No.: {2}."),
            		                    FLedgerNumber,
            		                    SelectedReportPeriod,
            		                    SelectedICHNumber);
            	}
            	
            	MessageBox.Show(msg, Catalog.GetString("Generate HOSA Files"));

                btnCancel.Text = "Close";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidReportPeriod(bool AIsHOSA = false)
        {
        	if ((cmbReportPeriod.SelectedIndex > -1) && !AIsHOSA)
            {
                return true;
            }
            else if (cmbReportPeriod.SelectedIndex == -1)
            {
                MessageBox.Show(Catalog.GetString("Please select a valid reporting period first."));
                cmbReportPeriod.Focus();
                return false;
            }
            else if (cmbReportPeriod.SelectedIndex > -1)
            {
            	return true;
            }
            else
            {
                return false;
            }
        }

        private void StewardshipCalculation(Object Sender, EventArgs e)
        {
            if (!ValidReportPeriod())
            {
                return;
            }

            bool retVal = false;

            TVerificationResultCollection VerificationResult = null;

            try
            {
                switch (this.ReportingPeriodSelectionMode)
                {
                    case TICHReportingPeriodSelectionModeEnum.rpsmICHStewardshipCalc:

                        Cursor = Cursors.WaitCursor;

                        retVal = TRemote.MFinance.ICH.WebConnectors.PerformStewardshipCalculation(FLedgerNumber, cmbReportPeriod.GetSelectedInt32(),
                        out VerificationResult);

                        Cursor = Cursors.Default;
                        String ResultMsg =
                            (retVal ? Catalog.GetString("Stewardship Calculation Completed Successfully") : Catalog.GetString(
                                 "UNSUCCESSFUL Stewardship Calculation!"));
                        MessageBox.Show(Messages.BuildMessageFromVerificationResult(ResultMsg, VerificationResult));
                        break;

                    case TICHReportingPeriodSelectionModeEnum.rpsmICHStatement:

                        throw new NotImplementedException(Catalog.GetString("ICH Statement functionality is not yet implemented!"));
                }

                btnCancel.Text = "Close";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
        }

        /// <summary>
        /// Gets or sets the ICH reporting period selection mode
        /// </summary>
        public TICHReportingPeriodSelectionModeEnum ReportingPeriodSelectionMode
        {
            get
            {
                return FReportingPeriodSelectionMode;
            }

            set
            {
                FReportingPeriodSelectionMode = value;

                if (FReportingPeriodSelectionMode == TICHReportingPeriodSelectionModeEnum.rpsmICHStatement)
                {
                    chkEmailHOSAReport.Enabled = true;
                }
                else
                {
                    chkEmailHOSAReport.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Write-only Ledger number property
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                btnOK.Visible = false;

                TFinanceControls.InitialiseOpenFinancialPeriodsList(
                    ref cmbReportPeriod,
                    FLedgerNumber);

                cmbReportPeriod.SelectedIndex = 0;

                //this.grpGenerateHosaFiles.Text = Catalog.GetString("Generate HOSA Files");
            }
        }

        private void RefreshICHStewardshipNumberList(object sender, EventArgs e)
        {
        	if ((cmbReportPeriod.SelectedIndex > -1))
            {
                TFinanceControls.InitialiseICHStewardshipList(ref cmbICHNumber, FLedgerNumber,
                    cmbReportPeriod.GetSelectedInt32());
        		
        		cmbICHNumber.SelectedIndex = 0;
            }
        }
    }
}