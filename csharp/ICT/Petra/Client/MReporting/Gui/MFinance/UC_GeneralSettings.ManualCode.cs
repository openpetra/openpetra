//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, timop
//
// Copyright 2004-2013 by OM International
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;
using SourceGrid.Selection;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// Description of TFrmUC_GeneralSettings.
    /// </summary>
    public partial class TFrmUC_GeneralSettings
    {
        private int FLedgerNumber = -1;
        private ALedgerRow FLedgerRow = null;
        Boolean OnlyEndPeriodShown = false;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            rbtPeriod.Checked = true;
            dtpStartDate.Enabled = false;
            dtpEndDate.Enabled = false;
            rbtDate.Enabled = false;
            txtQuarter.Enabled = false;
            cmbQuarterYear.Enabled = false;
            cmbBreakdownYear.Enabled = false;
            EnableBreakdownByPeriod(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseLedger(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
            FLedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

            txtLedger.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

            TFinanceControls.InitialiseAvailableEndOfYearsList(ref cmbPeriodYear, FLedgerNumber);
            cmbPeriodYear.SelectedIndex = 0;

            TFinanceControls.InitialiseAvailableEndOfYearsList(ref cmbQuarterYear, FLedgerNumber);
            cmbQuarterYear.SelectedIndex = 0;

            TFinanceControls.InitialiseAvailableEndOfYearsList(ref cmbBreakdownYear, FLedgerNumber);
            cmbBreakdownYear.SelectedIndex = 0;

            TFinanceControls.InitialiseAccountHierarchyList(ref cmbAccountHierarchy, FLedgerNumber);
            cmbAccountHierarchy.SelectedIndex = 0;

            // if there is only one hierarchy, disable the control
//          cmbAccountHierarchy.Enabled = (cmbAccountHierarchy.Count > 1);

            /* select the latest year TODO ??? */
//            if (this.CbB_AvailableYears.Items.Count > 0)
//            {
//                this.CbB_AvailableYears.SelectedIndex = 0; /// first item is the most current year
//            }
        }

        #region Parameter/Settings Handling

        /// <summary>
        /// Sets the available functions (fields) that can be used for this report.
        /// </summary>
        /// <param name="AAvailableFunctions">List of TColumnFunction</param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// Reads the selected values from the controls,
        /// and stores them into the parameter system of FCalculator
        ///
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        /// <returns>void</returns>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            int Year = 0;
            DateTime EndDate = DateTime.Now;

            // TODO
            int DiffPeriod = 0;             //(System.Int32)CbB_YearEndsOn.SelectedItem;

            //            DiffPeriod = DiffPeriod - 12;
            ACalculator.AddParameter("param_diff_period_i", DiffPeriod);
            ACalculator.AddParameter("param_start_period_i", 0);
            ACalculator.AddParameter("param_end_period_i", 0);

            ACalculator.AddParameter("param_account_hierarchy_c", this.cmbAccountHierarchy.GetSelectedString());

            String CurrencySelection = this.cmbCurrency.GetSelectedString();
            ACalculator.AddParameter("param_currency", CurrencySelection);

            String CurrencyName = (CurrencySelection == "Base") ? FLedgerRow.BaseCurrency : FLedgerRow.IntlCurrency;
            ACalculator.AddParameter("param_currency_name", CurrencyName);

            ACalculator.AddParameter("param_period", rbtPeriod.Checked);
            ACalculator.AddParameter("param_period_breakdown", rbtBreakdown.Checked && rbtBreakdown.Visible);

            ACalculator.AddParameter("param_period_checked", rbtPeriod.Checked);
            ACalculator.AddParameter("param_date_checked", rbtDate.Checked);
            ACalculator.AddParameter("param_quarter_checked", rbtQuarter.Checked);

            if (rbtQuarter.Checked)
            {
                Year = cmbQuarterYear.GetSelectedInt32();
                ACalculator.AddParameter("param_year_i", Year);
                ACalculator.AddParameter("param_real_year", cmbQuarterYear.GetSelectedString(1));
                ACalculator.AddParameter("param_real_year_ending", cmbPeriodYear.GetSelectedString(2));

                int Quarter = (Int32)StringHelper.TryStrToInt(txtQuarter.Text, 1);
                ACalculator.AddParameter("param_quarter", (System.Object)(Quarter));
                ACalculator.AddParameter("param_start_period_i", (System.Object)(Quarter * 3 - 2));
                ACalculator.AddParameter("param_end_period_i", (System.Object)(Quarter * 3));
                ACalculator.AddParameter("param_start_date",
                    TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, DiffPeriod, Quarter * 3 - 2));
                EndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, DiffPeriod, Quarter * 3);
                ACalculator.AddParameter("param_end_date", EndDate);

                //VerificationResult = TFinancialPeriodChecks.ValidQuarter(DiffPeriod, Year, Quarter, "Quarter");
                if (AReportAction == TReportActionEnum.raGenerate)
                {
                    CheckQuarter(Year, Quarter);
                }

                dtpStartDate.Date = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, DiffPeriod, (Quarter * 3 - 2));
                dtpEndDate.Date = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, DiffPeriod, Quarter * 3);
            }
            else if (rbtPeriod.Checked)
            {
                Year = cmbPeriodYear.GetSelectedInt32();
                ACalculator.AddParameter("param_year_i", Year);
                ACalculator.AddParameter("param_real_year", cmbPeriodYear.GetSelectedString(1));
                ACalculator.AddParameter("param_real_year_ending", cmbPeriodYear.GetSelectedString(2));

                int EndPeriod = (Int32)StringHelper.TryStrToInt(txtEndPeriod.Text, 1);
                ACalculator.AddParameter("param_end_period_i", EndPeriod);
                EndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, DiffPeriod, EndPeriod);
                ACalculator.AddParameter("param_end_date", EndDate);

                CheckPeriod(Year, EndPeriod);
                dtpEndDate.Date = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, DiffPeriod, EndPeriod);

                if (!OnlyEndPeriodShown)
                {
                    int StartPeriod = (Int32)StringHelper.TryStrToInt(txtStartPeriod.Text, 1);
                    ACalculator.AddParameter("param_start_period_i", StartPeriod);
                    ACalculator.AddParameter("param_start_date",
                        TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, DiffPeriod, StartPeriod));

                    if (AReportAction == TReportActionEnum.raGenerate)
                    {
                        CheckPeriod(Year, StartPeriod);

                        if (StartPeriod > EndPeriod)
                        {
                            FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                                    Catalog.GetString("Start Period must not be bigger than End Period."),
                                    Catalog.GetString("Invalid Data entered."),
                                    TResultSeverity.Resv_Critical));
                        }
                    }

                    dtpStartDate.Date = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, DiffPeriod, StartPeriod);
                }
            }
            else if (rbtDate.Checked)
            {
                if (dtpStartDate.Date.HasValue && dtpEndDate.Date.HasValue)
                {
                    if (dtpStartDate.Date.Value > dtpEndDate.Date.Value)
                    {
                        FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                                Catalog.GetString("Start Date must not be later than End Date"),
                                Catalog.GetString("Invalid Data entered."),
                                TResultSeverity.Resv_Critical));
                    }
                    else
                    {
                        ACalculator.AddParameter("param_start_date", dtpStartDate.Date.Value);
                        EndDate = dtpEndDate.Date.Value;
                        ACalculator.AddParameter("param_end_date", EndDate);
                        ACalculator.AddParameter("param_real_year", dtpEndDate.Date.Value.Year);
                    }
                }
            }
            else if (rbtBreakdown.Checked)
            {
                Year = cmbBreakdownYear.GetSelectedInt32();
                ACalculator.AddParameter("param_real_year", cmbBreakdownYear.GetSelectedString(1));
                ACalculator.AddParameter("param_year_i", Year);
                ACalculator.AddParameter("param_start_date", TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, DiffPeriod, 1));
                EndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, DiffPeriod, 12);
                ACalculator.AddParameter("param_end_date", EndDate);
            }

            //
            // I may need to check whether the selected currency has an exchange rate I can use!

            if (CurrencyName == FLedgerRow.IntlCurrency)
            {
                Decimal IntlRate = TExchangeRateCache.GetDailyExchangeRate(FLedgerRow.BaseCurrency, FLedgerRow.IntlCurrency, EndDate);

                if (IntlRate == 0) // No exchange rate has been specified
                {
                    FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                            Catalog.GetString("International Exchange Rate"),
                            Catalog.GetString("No Exchange Rate is available for the requested period."),
                            TResultSeverity.Resv_Critical));
                }
            }

            if (Year < 0)
            {
                FPetraUtilsObject.AddVerificationResult(new TVerificationResult(
                        Catalog.GetString("Accounting Year was not specified"),
                        Catalog.GetString("Invalid Data entered."),
                        TResultSeverity.Resv_Critical));
            }
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns>void</returns>
        public void SetControls(TParameterList AParameters)
        {
            if (FLedgerNumber == -1)
            {
                // we will wait until the ledger number has been set
                return;
            }

            // TODO
            //          int DiffPeriod = 0;//(System.Int32)CbB_YearEndsOn.SelectedItem;
            //            DiffPeriod = DiffPeriod - 12;
            //            ACalculator.AddParameter("param_diff_period_i", DiffPeriod);

            cmbAccountHierarchy.SetSelectedString(AParameters.Get("param_account_hierarchy_c").ToString());
            cmbCurrency.SetSelectedString(AParameters.Get("param_currency").ToString());
            cmbPeriodYear.SetSelectedInt32(AParameters.Get("param_year_i").ToInt());
            cmbQuarterYear.SetSelectedInt32(AParameters.Get("param_year_i").ToInt());
            cmbBreakdownYear.SetSelectedInt32(AParameters.Get("param_year_i").ToInt());

            rbtQuarter.Checked = AParameters.Get("param_quarter").ToBool();
            rbtDate.Checked = AParameters.Get("param_date_checked").ToBool();
            rbtPeriod.Checked = AParameters.Get("param_period").ToBool();
            rbtBreakdown.Checked = AParameters.Get("param_period_breakdown").ToBool();

            if (!rbtPeriod.Checked && !rbtDate.Checked && !rbtQuarter.Checked && !rbtBreakdown.Checked)
            {
                rbtPeriod.Checked = true;
            }

            txtQuarter.Text = (AParameters.Get("param_end_period_i").ToInt() / 3).ToString();
            txtStartPeriod.Text = AParameters.Get("param_start_period_i").ToString();
            txtEndPeriod.Text = AParameters.Get("param_end_period_i").ToString();

            if (txtStartPeriod.Text.Length == 0)
            {
                txtStartPeriod.Text = FLedgerRow.CurrentPeriod.ToString();
            }

            if (txtEndPeriod.Text.Length == 0)
            {
                txtEndPeriod.Text = FLedgerRow.CurrentPeriod.ToString();
            }

            if (cmbPeriodYear.SelectedIndex == -1)
            {
                cmbPeriodYear.SetSelectedInt32(FLedgerRow.CurrentFinancialYear);
            }

            if (cmbQuarterYear.SelectedIndex == -1)
            {
                cmbQuarterYear.SetSelectedInt32(FLedgerRow.CurrentFinancialYear);
            }

            if (cmbBreakdownYear.SelectedIndex == -1)
            {
                cmbBreakdownYear.SetSelectedInt32(FLedgerRow.CurrentFinancialYear);
            }

            if (AParameters.Exists("param_start_date"))
            {
                dtpStartDate.Date = AParameters.Get("param_start_date").ToDate();
                dtpEndDate.Date = AParameters.Get("param_end_date").ToDate();
            }
        }

        #endregion

        /// <summary>
        /// Hide all Period Range selection elements except of the year selection
        /// </summary>
        public void ShowOnlyYearSelection()
        {
            rbtPeriod.Visible = false;
            rbtQuarter.Visible = false;
            rbtDate.Visible = false;
            lblStartPeriod.Visible = false;
            lblEndPeriod.Visible = false;
            lblQuarter.Visible = false;
            lblQuarterYear.Visible = false;
            lblStartDate.Visible = false;
            lblEndDate.Visible = false;
            txtStartPeriod.Visible = false;
            txtEndPeriod.Visible = false;
            txtQuarter.Visible = false;
            cmbQuarterYear.Visible = false;
            dtpStartDate.Visible = false;
            dtpEndDate.Visible = false;
            cmbPeriodYear.Enabled = true;
            cmbPeriodYear.Visible = true;
        }

        /// <summary>
        /// For BalanceSheet, quarters and periods are not appropriate -
        /// I just want a single date.
        /// </summary>
        public void ShowOnlyEndPeriod()
        {
            lblStartPeriod.Visible = false;
            txtStartPeriod.Visible = false;

            rbtQuarter.Visible = false;
            lblQuarter.Visible = false;
            txtQuarter.Visible = false;
            lblQuarterYear.Visible = false;
            cmbQuarterYear.Visible = false;

            rbtDate.Visible = false;
            lblStartDate.Visible = false;
            dtpStartDate.Visible = false;
            lblEndDate.Visible = false;
            dtpEndDate.Visible = false;

            rbtPeriod.Visible = true;
            rbtPeriod.Checked = true;
            lblEndPeriod.Visible = true;
            lblEndPeriod.Location = new System.Drawing.Point(50, 12);
            lblEndPeriod.Size = new System.Drawing.Size(68, 17);
            lblEndPeriod.Text = "At end of:";
            txtEndPeriod.Visible = true;
            cmbPeriodYear.Visible = true;

            OnlyEndPeriodShown = true;
        }

        /// <summary>
        /// Show / Hide the account hierarchy combo box (e.g. we hide it in Financial development reports)
        /// </summary>
        /// <param name="AValue">false to hide the account hierarchy combo box</param>
        public void ShowAccountHierarchy(bool AValue)
        {
            lblAccountHierarchy.Visible = AValue;
            cmbAccountHierarchy.Visible = AValue;
        }

        /// <summary>
        /// Show / Hide the Currency selection group. (we hide it in Financial development reports)
        /// </summary>
        /// <param name="AValue">false to hide the currency selection group</param>
        public void ShowCurrencySelection(bool AValue)
        {
            cmbCurrency.Visible = AValue;
            lblCurrency.Visible = AValue;
            grpCurrency.Visible = AValue;
        }

        /// <summary>
        /// Show these currency choices (Instead of the usual list)
        /// </summary>
        /// <param name="items"></param>
        public void CurrencyOptions(object[] items)
        {
            this.cmbCurrency.Items.Clear();
            this.cmbCurrency.Items.AddRange(items);
        }

        /// <summary>
        /// Enable / Disable the radio button date
        /// </summary>
        /// <param name="AValue">true to enable the radio button</param>
        public void EnableDateSelection(bool AValue)
        {
            rbtDate.Enabled = AValue;
        }

        /// <summary>
        /// Allow Breakdown By Period (in Income Expense Statement only)
        /// </summary>
        /// <param name="AValue"></param>
        public void EnableBreakdownByPeriod(bool AValue)
        {
            rbtBreakdown.Visible = AValue;
            lblYear.Visible = AValue;
            cmbBreakdownYear.Visible = AValue;
            grpPeriodRange.Height = (AValue) ? 240 : 164;
        }

        private void UnselectAll(System.Object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Checks whether the period is inside a valid financial year
        /// </summary>
        /// <param name="AYear">which year is the period in</param>
        /// <param name="APeriodNr">period</param>
        /// <returns></returns>
        private void CheckPeriod(int AYear, int APeriodNr)
        {
            String ValidRange;

            System.Int32 RealPeriod;
            System.Int32 RealYear;

            TRemote.MFinance.GL.WebConnectors.GetRealPeriod(FLedgerNumber, 0, AYear, APeriodNr, out RealPeriod, out RealYear);

            if (RealYear == FLedgerRow.CurrentFinancialYear)
            {
                ValidRange = Catalog.GetString("1 and ") + Convert.ToString(FLedgerRow.CurrentPeriod + FLedgerRow.NumberFwdPostingPeriods);
            }
            else
            {
                ValidRange = Catalog.GetString("1 and ") + FLedgerRow.NumberOfAccountingPeriods.ToString();
            }

            if ((APeriodNr <= 0)
                || ((RealYear == FLedgerRow.CurrentFinancialYear) && (RealPeriod > FLedgerRow.CurrentPeriod + FLedgerRow.NumberFwdPostingPeriods))
                || ((RealYear < FLedgerRow.CurrentFinancialYear) && (APeriodNr > FLedgerRow.NumberOfAccountingPeriods)))
            {
                FPetraUtilsObject.AddVerificationResult(new TVerificationResult("",
                        Catalog.GetString("Invalid Period entered.") + Environment.NewLine + Catalog.GetString("Period must be between ") +
                        ValidRange + '.',
                        Catalog.GetString("Invalid Data entered"),
                        "X_0041",
                        TResultSeverity.Resv_Critical));
            }
        }

        /// <summary>
        /// Checks whether the quarter is inside a valid financial year
        /// </summary>
        /// <param name="AYear">which year is the period in</param>
        /// <param name="AQuarter">quarter</param>
        /// <returns></returns>
        private void CheckQuarter(int AYear, int AQuarter)
        {
            String ValidRange;

            System.Int32 RealPeriod;
            System.Int32 RealYear;

            TRemote.MFinance.GL.WebConnectors.GetRealPeriod(FLedgerNumber, 0, AYear, AQuarter * 3 - 2, out RealPeriod, out RealYear);

            if (RealYear == FLedgerRow.CurrentFinancialYear)
            {
                ValidRange = Catalog.GetString("1 and ") + Convert.ToString(
                    ((FLedgerRow.CurrentPeriod + FLedgerRow.NumberFwdPostingPeriods) - 1) / 3 + 1);
            }
            else
            {
                ValidRange = Catalog.GetString("1 and ") + Convert.ToString((FLedgerRow.NumberOfAccountingPeriods - 1) / 3 + 1);
            }

            if ((AQuarter <= 0)
                || ((RealYear == FLedgerRow.CurrentFinancialYear) && (RealPeriod > FLedgerRow.CurrentPeriod + FLedgerRow.NumberFwdPostingPeriods))
                || ((RealYear < FLedgerRow.CurrentFinancialYear) && (AQuarter > 4)))
            {
                FPetraUtilsObject.AddVerificationResult(new TVerificationResult("",
                        Catalog.GetString("Invalid Quarter entered.") + Environment.NewLine + Catalog.GetString("Quarter must be between ") +
                        ValidRange + '.',
                        Catalog.GetString("Invalid Data entered"),
                        "X_0041",
                        TResultSeverity.Resv_Critical));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void DisableToPeriod()
        {
            txtEndPeriod.Enabled = false;
            txtStartPeriod.TextChanged += new EventHandler(txtStartPeriod_TextChanged);
            rbtPeriod.CheckedChanged += new EventHandler(rbtPeriod_CheckedChanged);
        }

        //
        // This handler disables the "to" period txt box,
        // iff DisableToPeriod, above, has been called.
        void rbtPeriod_CheckedChanged(object sender, EventArgs e)
        {
            txtEndPeriod.Enabled = false;
        }

        //
        // This handler copies the "from" period to the "to" period,
        // iff DisableToPeriod, above, has been called.
        void txtStartPeriod_TextChanged(object sender, EventArgs e)
        {
            txtEndPeriod.Text = txtStartPeriod.Text;
        }
    }
}