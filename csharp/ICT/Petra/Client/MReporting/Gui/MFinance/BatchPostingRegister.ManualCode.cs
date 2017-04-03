//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2017 by OM International
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
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmBatchPostingRegister
    {
        private Int32 FLedgerNumber;
        private Int32 FBatchNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 BatchNumber
        {
            set
            {
                if (value != -1)
                {
                    FBatchNumber = value;
                    txtBatchNumber.Text = FBatchNumber.ToString();
                }
            }
        }

        //
        // Returns True if the data apparently loaded OK and the report should be printed.
        private bool LoadReportData(TRptCalculator ACalc)
        {
            GLBatchTDS BatchTDS = null;

            try
            {
                BatchTDS = TRemote.MFinance.GL.WebConnectors.LoadABatchAndRelatedTablesUsingPrivateDb(FLedgerNumber, FBatchNumber);
            }
            catch
            {
            }         // Ignore this error and instead detect the empty batch, below:

            if ((BatchTDS == null) || (BatchTDS.ABatch == null) || (BatchTDS.ABatch.Rows.Count < 1))
            {
                return false;
            }

            //Call RegisterData to give the data to the template
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(BatchTDS.ABatch, "ABatch");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(BatchTDS.AJournal, "AJournal");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(BatchTDS.ATransaction, "ATransaction");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountList,
                    FLedgerNumber), "AAccount");
            FPetraUtilsObject.FFastReportsPlugin.RegisterData(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.
                    CostCentreList,
                    FLedgerNumber), "ACostCentre");

            ACalc.AddParameter("param_batch_number_i", FBatchNumber);
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ALedgerTable LedgerTable = (ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails,
                FLedgerNumber);

            ACalc.AddStringParameter("param_linked_partner_cc", ""); // I may want to use this for auto_email, but usually it's unused.
            ACalc.AddParameter("param_currency_name", LedgerTable[0].BaseCurrency);

            if (this.IsDisposed) // If the form is closed, exit immediately!
            {
                return false;
            }

            return true;
        }

        /// <summary>Automatically print this Posting Register without displaying the UI</summary>
        /// <param name="AledgerNumber"></param>
        /// <param name="AbatchNumber"></param>
        public void PrintReportNoUi(Int32 AledgerNumber, Int32 AbatchNumber)
        {
            FLedgerNumber = AledgerNumber;
            FBatchNumber = AbatchNumber;
            FPetraUtilsObject.LoadDefaultSettings();
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();
            TRptCalculator Calc = new TRptCalculator();
            Calc.AddStringParameter("param_sortby", "Transaction");     // always by transaction number with no UI

            FPetraUtilsObject.FFastReportsPlugin.GenerateReport(Calc);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            Int32.TryParse(txtBatchNumber.Text, out FBatchNumber);
        }

        private void RunOnceOnActivationManual()
        {
            // if fast reports isn't working then close the screen
            if ((FPetraUtilsObject.GetCallerForm() != null) && !FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                MessageBox.Show("No alternative reporting engine is available for this report. This screen will now be closed.", "Reporting engine");
                this.Close();
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
        }
    }
}