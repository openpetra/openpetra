//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using Ict.Petra.Client.MReporting.Logic;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Shared.MReporting;

using Ict.Petra.Client.MSysMan.Gui;
using Ict.Common.IO;
using System.Text;


namespace Ict.Petra.Client.MFinance.Gui.ICH
{
    /// manual methods for the generated window
    public partial class TFrmStewardshipReports : System.Windows.Forms.Form
    {
        Int32 FLedgerNumber = 0;
        ALedgerRow FLedgerRow = null;
        FastReportsWrapper MyFastReportsPlugin;
        List <String>FStatusMsg = new List <String>();

        const string STEWARDSHIP_EMAIL_ADDRESS = "ICHEMAIL";

        /// <summary>
        /// Write-only Ledger number property
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                //
                // I've been getting some grief from cached tables, so I'll mark them as being "dirty" before I do anything else:

                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.ICHStewardshipList, FLedgerNumber);
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber);


                FLedgerRow =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                TFinanceControls.InitialiseAvailableFinancialYearsListHOSA(
                    ref cmbYearEnding,
                    FLedgerNumber);

                chkHOSA.CheckedChanged += RefreshReportingOptions;
                chkStewardship.CheckedChanged += RefreshReportingOptions;
                chkFees.CheckedChanged += RefreshReportingOptions;
                //              chkRecipient.CheckedChanged += RefreshReportingOptions;
                cmbReportPeriod.SelectedValueChanged += RefreshReportingOptions;
                rbtEmailHosa.CheckedChanged += RefreshReportingOptions;
                rbtReprintHosa.CheckedChanged += RefreshReportingOptions;
                RefreshReportingOptions(null, null);
                FPetraUtilsObject.DelegateGenerateReportOverride = GenerateAllSelectedReports;
                FPetraUtilsObject.DelegateViewReportOverride = ViewReportTemplate;
                uco_SelectedFees.LedgerNumber = FLedgerNumber;

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        //
        // called on Year change.
        private void RefreshReportPeriodList(object sender, EventArgs e)
        {
            if (cmbYearEnding.SelectedIndex > -1)
            {
                TFinanceControls.InitialiseAvailableFinancialPeriodsList(
                    ref cmbReportPeriod,
                    FLedgerNumber,
                    cmbYearEnding.GetSelectedInt32(),
                    FLedgerRow.CurrentPeriod - 1,
                    false,
                    false);
            }
        }

        //
        // Called on period change.
        private void RefreshICHStewardshipNumberList(object sender, EventArgs e)
        {
            if ((cmbReportPeriod.SelectedIndex > -1) && (cmbYearEnding.SelectedIndex > -1))
            {
                Int32 accountingYear = cmbYearEnding.GetSelectedInt32();
                TFinanceControls.InitialiseICHStewardshipList(
                    cmbICHNumber, FLedgerNumber,
                    cmbYearEnding.GetSelectedInt32(),
                    cmbReportPeriod.GetSelectedInt32());

                cmbICHNumber.SelectedIndex = 0;
            }
        }

        /// <summary>Called from generated code</summary>
        public void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        //
        // Called on any report checkbox changed
        private void RefreshReportingOptions(Object Sender, EventArgs e)
        {
            if (cmbYearEnding.SelectedIndex != 0)
            {
                chkHOSA.Enabled = chkHOSA.Checked = false;
                chkFees.Enabled = chkFees.Checked = false;
            }
            else
            {
                chkHOSA.Enabled = true;
                chkFees.Enabled = true;
            }

            rbtEmailHosa.Enabled =
                rbtReprintHosa.Enabled = chkHOSA.Enabled && chkHOSA.Checked;

            if (rbtEmailHosa.Enabled && !rbtEmailHosa.Checked && !rbtReprintHosa.Checked)
            {
                rbtEmailHosa.Checked = true;
            }

            chkRecipient.Enabled =
                rbtReprintHosa.Enabled & rbtReprintHosa.Checked;

            rbtEmailStewardship.Enabled =
                rbtReprintStewardship.Enabled = chkStewardship.Enabled && chkStewardship.Checked;

            if (rbtEmailStewardship.Enabled && !rbtEmailStewardship.Checked && !rbtReprintStewardship.Checked)
            {
                rbtEmailStewardship.Checked = true;
            }

            rbtFull.Enabled =
                rbtSummary.Enabled = chkFees.Enabled && chkFees.Checked;
        }

        private void ViewReportTemplate(TRptCalculator ACalc)
        {
            String ReportName = "";

            if (chkFees.Enabled && chkFees.Checked)
            {
                ReportName = "Fees";
            }

            if (chkStewardship.Enabled && chkStewardship.Checked)
            {
                ReportName = "Stewardship";
            }

            if (chkHOSA.Enabled && chkHOSA.Checked)
            {
                ReportName = "HOSA";
            }

            if (ReportName == "")
            {
                return;
            }

            MyFastReportsPlugin = new FastReportsWrapper(ReportName);

            if (chkFees.Enabled && chkFees.Checked)
            {
                MyFastReportsPlugin.SetDataGetter(LoadFeesReportData);
            }

            if (chkStewardship.Enabled && chkStewardship.Checked)
            {
                MyFastReportsPlugin.SetDataGetter(LoadStewardshipReportData);
            }

            if (chkHOSA.Enabled && chkHOSA.Checked)
            {
                MyFastReportsPlugin.SetDataGetter(LoadHosaReportData);
            }

            MyFastReportsPlugin.DesignReport(ACalc);
        }

        //
        // New methods using the Fast-reports DLL:
        // This form generates a clutch of different reports.

        // In this method, a new FastReportsWrapper is used for each selected report type.

        private void GenerateAllSelectedReports(TRptCalculator ACalc)
        {
            FStatusMsg.Clear();

            if (chkHOSA.Enabled && chkHOSA.Checked)
            {
                FStatusMsg.Add(Catalog.GetString("All HOSAs:"));
                MyFastReportsPlugin = new FastReportsWrapper("HOSA");
                MyFastReportsPlugin.SetDataGetter(LoadHosaReportData);
                MyFastReportsPlugin.GenerateReport(ACalc);
            }

            if (chkStewardship.Enabled && chkStewardship.Checked)
            {
                FStatusMsg.Add(Catalog.GetString("Stewardship Report:"));
                MyFastReportsPlugin = new FastReportsWrapper("Stewardship");
                MyFastReportsPlugin.SetDataGetter(LoadStewardshipReportData);
                MyFastReportsPlugin.GenerateReport(ACalc);
            }

            if (chkFees.Enabled && chkFees.Checked)
            {
                FStatusMsg.Add(Catalog.GetString("Fees Report:"));
                MyFastReportsPlugin = new FastReportsWrapper("Fees");
                MyFastReportsPlugin.SetDataGetter(LoadFeesReportData);
                MyFastReportsPlugin.GenerateReport(ACalc);
            }

            // complex way of stepping around the Windows non-thread-safe controls problem!
            FStatusMsg.Add("\n" + Catalog.GetString("Report generation complete."));
            this.Invoke(new CrossThreadUpdate(ShowReportStatus));
        }

        delegate void CrossThreadUpdate();

        private void ShowReportStatus()
        {
            MessageBox.Show(String.Join("\n", FStatusMsg), Catalog.GetString("Stewardship Reports"));
        }

        private Dictionary <String, TVariant>InitialiseDictionary(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();
            pm.Add("param_ledger_number_i", FLedgerNumber);
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddParameter("param_currency_name", FLedgerRow.BaseCurrency); // Stewardship reports are always in Base Currency.

            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");
            ACalc.AddParameter("param_ich_number", pm.Get("param_cmbICHNumber").ToInt32());
            ACalc.AddParameter("param_period", true);

            Int32 period = pm.Get("param_cmbReportPeriod").ToInt32();
            Int32 PeriodStart = Math.Max(1, period);
            Int32 PeriodEnd = period;

            if (PeriodEnd == 0)
            {
                PeriodEnd = TFinanceControls.GetLedgerNumPeriods(FLedgerNumber);
            }

            Int32 Year = pm.Get("param_cmbYearEnding").ToInt32();

            ACalc.AddParameter("param_start_period_i", PeriodStart);
            ACalc.AddParameter("param_end_period_i", PeriodEnd);
            DateTime StartDate = TRemote.MFinance.GL.WebConnectors.GetPeriodStartDate(FLedgerNumber, Year, 0, PeriodStart);
            DateTime EndDate = TRemote.MFinance.GL.WebConnectors.GetPeriodEndDate(FLedgerNumber, Year, 0, PeriodEnd);
            ACalc.AddParameter("param_real_year", StartDate.Year);
            ACalc.AddParameter("param_start_date", StartDate);
            ACalc.AddParameter("param_end_date", EndDate);
            ACalc.AddParameter("param_current_financial_year", FLedgerRow.CurrentFinancialYear == Year);
            Boolean IsClosed = (Year < FLedgerRow.CurrentFinancialYear) || (PeriodEnd < FLedgerRow.CurrentPeriod);
            ACalc.AddParameter("param_period_closed", IsClosed);
            Boolean IsCurrent = (Year == FLedgerRow.CurrentFinancialYear) && (PeriodEnd == FLedgerRow.CurrentPeriod);
            ACalc.AddParameter("param_period_current", IsCurrent);
            ACalc.AddParameter("param_year_i", Year);
            ArrayList reportParam = pm.Elems;

            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            return paramsDictionary;
        } // Initialise Dictionary

        private Boolean LoadHosaReportData(TRptCalculator ACalc)
        {
            InitialiseDictionary(ACalc);
            ACalc.AddStringParameter("param_cost_centre_codes", "ALL");
            ACalc.AddStringParameter("param_filter_cost_centres", "");
            ACalc.AddStringParameter("param_linked_partner_cc", ""); // Used for auto-emailing HOSAs, this is usually blank.
            ACalc.AddParameter("param_include_rgs", !chkRecipient.Enabled || chkRecipient.Checked);
            Boolean DataOk = TFrmHOSA.LoadReportDataStaticInner(this, FPetraUtilsObject, MyFastReportsPlugin, ACalc);

            if ((!ACalc.GetParameters().Get("param_design_template").ToBool())
                && (rbtEmailHosa.Checked))
            {
                ACalc.AddStringParameter("param_currency", "Base"); // Always email HOSAs in Base Currency
                FStatusMsg.AddRange(FastReportsWrapper.AutoEmailReports(FPetraUtilsObject, MyFastReportsPlugin, ACalc, FLedgerNumber, "Foreign"));
                return false;
            }

            return DataOk;
        }  // Load Hosa Report Data

        private Boolean LoadStewardshipReportData(TRptCalculator ACalc)
        {
            Dictionary <String, TVariant>paramsDictionary = InitialiseDictionary(ACalc);
            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("Stewardship", paramsDictionary);
            TSmtpSender EmailSender;

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            MyFastReportsPlugin.RegisterData(ReportTable, "Stewardship");

            Boolean HasData = (ReportTable.Rows.Count > 0);

            if (!HasData)
            {
                FStatusMsg.Add(Catalog.GetString("No Stewardship entries found for selected Run Number."));
            }

            TParameterList Params = ACalc.GetParameters();

            if ((!Params.Get("param_design_template").ToBool())
                && (rbtEmailStewardship.Checked))
            {
                try
                {
                    EmailSender = new TSmtpSender();
                    EmailSender.SetSender(TUserDefaults.GetStringDefault("SmtpFromAccount"), TUserDefaults.GetStringDefault("SmtpDisplayName"));
                    EmailSender.CcEverythingTo = TUserDefaults.GetStringDefault("SmtpCcTo");
                    EmailSender.ReplyTo = TUserDefaults.GetStringDefault("SmtpReplyTo");
                }
                catch (ESmtpSenderInitializeException e)
                {
                    if (e.InnerException != null)
                    {
                        // I'd write the full exception to the log file, but it still gets transferred to the client window status bar and is _really_ ugly.
                        //TLogging.Log("Stewardship Email: " + e.InnerException.ToString());
                        TLogging.Log("Stewardship Email: " + e.InnerException.Message);
                    }

                    FStatusMsg.Add(e.Message);

                    if (e.ErrorClass == TSmtpErrorClassEnum.secClient)
                    {
                        FStatusMsg.Add("See the Email tab in User Settings >> Preferences.");
                    }

                    return false;
                }

                String MyCostCentreCode = String.Format("{0:##00}00", FLedgerNumber);
                DateTime paramEndDate = Params.Get("param_end_date").ToDate();
                String PeriodEnd = paramEndDate.ToString("dd/MM/yyyy");
                Int32 RunNumber = Params.Get("param_cmbICHNumber").ToInt32();
                String CsvAttachment = String.Format("ICH-CSV,{0},{1},{2},{3},{4},{5},{6:F0}\n", // ICH-CSV,<ledger number>,<calendar year>,<calendar month number>,<run number>,<currency code>,<date of report>,<time of report in number of seconds>

                    FLedgerRow.LedgerNumber,                    // <Ledger Number>
                    paramEndDate.Year,                          // <calendar year>
                    paramEndDate.Month,                         // <calendar month number>
                    RunNumber,                                  // <run number>
                    FLedgerRow.BaseCurrency,                    // Stewardship Report CSV always in Base Currency
                    DateTime.Now.ToString("dd/MM/yyyy"),        // <date of report>
                    DateTime.Now.TimeOfDay.TotalSeconds         // <time of report in number of seconds>
                    );

                foreach (DataRow Row in ReportTable.Rows) // <cost centre>,<income amount>,<expense amount>,<transfer amount>
                {
                    CsvAttachment += String.Format("{0},{1},{2},{3}\n",
                        Row["CostCentreCode"].ToString(),
                        Convert.ToDecimal(Row["Income"]).ToString("0.00", CultureInfo.InvariantCulture),  // Stewardship Report CSV always in Base Currency
                        Convert.ToDecimal(Row["Expense"]).ToString("0.00", CultureInfo.InvariantCulture),
                        Convert.ToDecimal(Row["Xfer"]).ToString("0.00", CultureInfo.InvariantCulture)
                        );
                }

                //
                // This System Default must be present to send ICH emails

                if (!TSystemDefaults.IsSystemDefaultDefined(STEWARDSHIP_EMAIL_ADDRESS))
                {
                    FStatusMsg.Add(Catalog.GetString("Stewardship email address not configured in System Defaults."));
                    return false;
                }

                String EmailRecipient = TSystemDefaults.GetStringDefault(STEWARDSHIP_EMAIL_ADDRESS);

                String EmailBody = TUserDefaults.GetStringDefault("SmtpEmailBody");
                EmailSender.AttachFromStream(new MemoryStream(Encoding.ASCII.GetBytes(CsvAttachment)), "Stewardship_" + MyCostCentreCode + ".csv");
                Boolean SentOk = EmailSender.SendEmail(
                    EmailRecipient,
                    "Stewardship Report [" + MyCostCentreCode + "] Period end: " + PeriodEnd + " Run#: " + RunNumber,
                    EmailBody);

                if (SentOk)
                {
                    FStatusMsg.Add(Catalog.GetString("Stewardship report emailed to ICH."));
                }
                else
                {
                    FStatusMsg.Add(Catalog.GetString("Failed to send Stewardship email to ICH:"));
                    FStatusMsg.Add(EmailSender.ErrorStatus);
                }

                EmailSender.Dispose();
                return false;
            }

            return HasData;
        } // Load Stewardship Report Data

        private Boolean LoadFeesReportData(TRptCalculator ACalc)
        {
            Dictionary <String, TVariant>paramsDictionary = InitialiseDictionary(ACalc);
            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("Fees", paramsDictionary);

            if (this.IsDisposed)
            {
                return false;
            }

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            String[] SelectedFees = paramsDictionary["param_fee_codes"].ToString().Split(',');
            Int32 FeeCols = SelectedFees.Length;
            ACalc.AddParameter("param_fee_columns", FeeCols);

            DataTable FeeNames = new DataTable();

            for (Int32 Idx = 0; Idx < uco_SelectedFees.MAX_FEE_COUNT; Idx++)
            {
                FeeNames.Columns.Add();
            }

            DataRow Row = FeeNames.NewRow();
            FeeNames.Rows.Add(Row);

            for (Int32 Idx = 0; Idx < FeeCols; Idx++)
            {
                Row[Idx] = SelectedFees[Idx];
            }

            ACalc.AddParameter("param_full_report", rbtFull.Checked);

            MyFastReportsPlugin.RegisterData(FeeNames, "FeeNames");
            MyFastReportsPlugin.RegisterData(ReportTable, "Fees");
            return true;
        } // Load Fees Report Data

        /*
         *      private Boolean LoadRecipientReportData(TRptCalculator ACalc)
         *      {
         *          InitialiseDictionary(ACalc);
         *          return false;
         *      }  // Load Recipient Report Data
         */
    }
}
