//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
//       Tim Ingham
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
using Ict.Petra.Client.MReporting.Logic;
using System.Collections;
using System.Collections.Generic;
using Ict.Petra.Client.MReporting.Gui;


namespace Ict.Petra.Client.MFinance.Gui.ICH
{
    /// <summary>
    /// Enums holding the possible reporting period selection modes
    /// </summary>
    public enum ICHModeEnum
    {
        /// <summary>
        /// ICH Statement reporting period selection mode
        /// </summary>
        Statement,
        /// <summary>
        /// ICH Stewardship Calculation reporting period selection mode
        /// </summary>
        StewardshipCalc
    }


    /// manual methods for the generated window
    public partial class TFrmStewardshipReports : System.Windows.Forms.Form
    {
        Int32 FLedgerNumber = 0;
        ALedgerRow FLedgerRow = null;
        FastReportsWrapper MyFastReportsPlugin;

/*
 *      ICHModeEnum FReportingPeriodSelectionMode = ICHModeEnum.StewardshipCalc;
 *
 *      /// <summary>
 *      /// Gets or sets the ICH reporting period selection mode
 *      /// </summary>
 *      public ICHModeEnum ReportingPeriodSelectionMode
 *      {
 *          get
 *          {
 *              return FReportingPeriodSelectionMode;
 *          }
 *
 *          set
 *          {
 *              FReportingPeriodSelectionMode = value;
 *
 *              if (FReportingPeriodSelectionMode == ICHModeEnum.Statement)
 *              {
 *                  chkEmailHOSAReport.Enabled = true;
 *              }
 *              else
 *              {
 *                  chkEmailHOSAReport.Enabled = false;
 *              }
 *          }
 *      }
 */
        /// <summary>
        /// Write-only Ledger number property
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                FLedgerRow =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];

                TFinanceControls.InitialiseAvailableFinancialYearsListHOSA(
                    ref cmbYearEnding,
                    FLedgerNumber);

                /*
                 * //Resize and move label
                 * cmbYearEnding.ComboBoxWidth += 18;
                 * cmbYearEnding.AttachedLabel.Left += 18;
                 */
                chkHOSA.CheckedChanged += RefreshReportingOptions;
                chkStewardship.CheckedChanged += RefreshReportingOptions;
                chkFees.CheckedChanged += RefreshReportingOptions;
                chkRecipient.CheckedChanged += RefreshReportingOptions;
                RefreshReportingOptions(null, null);
                FPetraUtilsObject.DelegateGenerateReportOverride = GenerateAllSelectedReports;
                FPetraUtilsObject.DelegateViewReportOverride = ViewReportTemplate;
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
                    FLedgerRow.CurrentPeriod,
                    false);
            }
        }

        //
        // Called on period change.
        private void RefreshICHStewardshipNumberList(object sender, EventArgs e)
        {
            if ((cmbReportPeriod.SelectedIndex > -1) && (cmbYearEnding.SelectedIndex > -1))
            {
                DateTime YearEnding;

                if (DateTime.TryParse(cmbYearEnding.GetSelectedDescription(), out YearEnding))
                {
                    DateTime YearStart = TRemote.MFinance.GL.WebConnectors.DecrementYear(YearEnding).AddDays(1);

                    TFinanceControls.InitialiseICHStewardshipList(ref cmbICHNumber, FLedgerNumber,
                        cmbReportPeriod.GetSelectedInt32(),
                        YearStart.ToShortDateString(),
                        YearEnding.ToShortDateString());
                }
                else
                {
                    TFinanceControls.InitialiseICHStewardshipList(ref cmbICHNumber, FLedgerNumber,
                        cmbReportPeriod.GetSelectedInt32(),
                        null,
                        null);
                }

                cmbICHNumber.SelectedIndex = 0;
            }
        }

        //
        // Called on any report checkbox changed
        private void RefreshReportingOptions(Object Sender, EventArgs e)
        {
            rbtEmailHosa.Enabled = chkHOSA.Checked;
            rbtReprintHosa.Enabled = chkHOSA.Checked;

            rbtEmailStewardship.Enabled =
                rbtReprintStewardship.Enabled = chkStewardship.Checked;

            rbtEmailFees.Enabled = chkFees.Checked;
            rbtReprintFees.Enabled = chkFees.Checked;

            rbtEmailRecipient.Enabled = chkRecipient.Checked;
            rbtReprintRecipient.Enabled = chkRecipient.Checked;
        }

/*
 *      private void GenerateReports(Object Sender, EventArgs e)
 *      {
 *          String CurrencySelect = (this.rbtBase.Checked ? MFinanceConstants.CURRENCY_BASE : MFinanceConstants.CURRENCY_INTERNATIONAL);
 *          bool DoGenerateHOSAReports = chkHOSAReport.Checked;
 *          bool DoEmailHOSAReports = chkEmailHOSAReport.Checked;
 *          bool DoGenerateHOSAFiles = chkHOSAFile.Checked;
 *          bool DoEmailHOSAFiles = chkEmailHOSAFile.Checked;
 *
 *          TVerificationResultCollection VerificationResults;
 *
 *          string msg = string.Empty;
 *          string SuccessfullCostCentres = string.Empty;
 *          string FailedCostCentres = string.Empty;
 *
 *          int SelectedReportPeriod = cmbReportPeriod.GetSelectedInt32();
 *          int SelectedICHNumber = cmbICHNumber.GetSelectedInt32();
 *
 *          if (!ValidReportPeriod())
 *          {
 *              return;
 *          }
 *
 *          String HOSAFilePrefix = txtHOSAPrefix.Text;
 *
 *          if (HOSAFilePrefix.Length == 0)
 *          {
 *              HOSAFilePrefix = Catalog.GetString("HOSAFilesExportFor");
 *          }
 *          else
 *          {
 *              Int32 IndexOfInvalidFilenameCharacter = HOSAFilePrefix.IndexOfAny(Path.GetInvalidFileNameChars());
 *
 *              if (IndexOfInvalidFilenameCharacter >= 0)
 *              {
 *                  msg = String.Format("The HOSA File Prefix: '{0}', contains characters not valid in a filename: {1}{2}{2}Please remove and retry.",
 *                      HOSAFilePrefix,
 *                      String.Join(", ", Path.GetInvalidFileNameChars()),
 *                      Environment.NewLine);
 *
 *                  MessageBox.Show(msg, "Generate HOSA Reports and Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
 *
 *                  txtHOSAPrefix.Focus();
 *                  txtHOSAPrefix.Select(IndexOfInvalidFilenameCharacter, 1);
 *                  return;
 *              }
 *          }
 *
 *          try
 *          {
 *              Cursor = Cursors.WaitCursor;
 *
 *              DataTable ICHNumbers = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.ICHStewardshipList, FLedgerNumber);
 *
 *              //Filter for current period
 *              if (SelectedICHNumber != 0)
 *              {
 *                  ICHNumbers.DefaultView.RowFilter = String.Format("{0}={1} And {2}={3}",
 *                      AIchStewardshipTable.GetPeriodNumberDBName(),
 *                      SelectedReportPeriod,
 *                      AIchStewardshipTable.GetIchNumberDBName(),
 *                      SelectedICHNumber);
 *              }
 *              else
 *              {
 *                  ICHNumbers.DefaultView.RowFilter = String.Format("{0}={1}",
 *                      AIchStewardshipTable.GetPeriodNumberDBName(),
 *                      SelectedReportPeriod);
 *              }
 *
 *              ICHNumbers.DefaultView.Sort = AIchStewardshipTable.GetCostCentreCodeDBName();
 *
 *              foreach (DataRowView tmpRow in ICHNumbers.DefaultView)
 *              {
 *                  AIchStewardshipRow ichRow = (AIchStewardshipRow)tmpRow.Row;
 *                  bool HOSASuccess = false;
 *
 *                  String CostCentreCode = ichRow.CostCentreCode;
 *
 *                  if (DoGenerateHOSAReports)
 *                  {
 *                      //TODO code to generate the HOSA reports
 *                      TRemote.MFinance.ICH.WebConnectors.GenerateHOSAReports(FLedgerNumber,
 *                          cmbReportPeriod.GetSelectedInt32(),
 *                          cmbICHNumber.GetSelectedInt32(),
 *                          CurrencySelect,
 *                          out VerificationResults);
 *                      HOSASuccess = !VerificationResults.HasCriticalErrors;
 *                  }
 *                  else if (DoGenerateHOSAFiles)
 *                  {
 *                      String FileName = TClientSettings.PathTemp + Path.DirectorySeparatorChar + HOSAFilePrefix + CostCentreCode + ".csv";
 *                      HOSASuccess = TRemote.MFinance.ICH.WebConnectors.GenerateHOSAFiles(FLedgerNumber, cmbReportPeriod.GetSelectedInt32(),
 *                          cmbICHNumber.GetSelectedInt32(), CostCentreCode, CurrencySelect, FileName, out VerificationResults);
 *                  }
 *
 *                  if (HOSASuccess)
 *                  {
 *                      if (SuccessfullCostCentres.Length == 0)
 *                      {
 *                          SuccessfullCostCentres = CostCentreCode;
 *                      }
 *                      else
 *                      {
 *                          SuccessfullCostCentres += ", " + CostCentreCode;
 *                      }
 *                  }
 *                  else
 *                  {
 *                      if (FailedCostCentres.Length == 0)
 *                      {
 *                          FailedCostCentres = CostCentreCode;
 *                      }
 *                      else
 *                      {
 *                          FailedCostCentres += ", " + CostCentreCode;
 *                      }
 *                  }
 *              }
 *
 *              Cursor = Cursors.Default;
 *
 *              if (SuccessfullCostCentres.Length > 0)
 *              {
 *                  msg = String.Format(Catalog.GetString("HOSA file generated successfully for Cost Centre(s):{0}{0}{1}{0}{0}"),
 *                      Environment.NewLine,
 *                      SuccessfullCostCentres);
 *              }
 *
 *              if (FailedCostCentres.Length > 0)
 *              {
 *                  msg += String.Format(Catalog.GetString("HOSA generation FAILED for Cost Centre(s):{0}{0}{1}"),
 *                      Environment.NewLine,
 *                      FailedCostCentres);
 *              }
 *
 *              if (msg.Length == 0)
 *              {
 *                  msg = Catalog.GetString("Stewardship Calculations haven't been run or no transactions to process.");
 *              }
 *
 *              MessageBox.Show(msg, Catalog.GetString("Generate Reports"));
 *          }
 *          finally
 *          {
 *              Cursor = Cursors.Default;
 *          }
 *      }
 *      private bool ValidReportPeriod()
 *      {
 *          if (cmbReportPeriod.SelectedIndex > -1)
 *          {
 *              return true;
 *          }
 *          else if (cmbReportPeriod.Count > 0)
 *          {
 *              MessageBox.Show(Catalog.GetString("Please select a valid reporting period first."));
 *              cmbReportPeriod.Focus();
 *          }
 *
 *          return false;
 *      }
 */
        private void ViewReportTemplate(TRptCalculator ACalc)
        {
            String ReportName = "";

            if (chkRecipient.Checked)
            {
                ReportName = "Recipient Gift Statement";
            }

            if (chkFees.Checked)
            {
                ReportName = "Fees";
            }

            if (chkStewardship.Checked)
            {
                ReportName = "Stewardship";
            }

            if (chkHOSA.Checked)
            {
                ReportName = "HOSA";
            }

            if (ReportName == "")
            {
                return;
            }

            MyFastReportsPlugin = new FastReportsWrapper(ReportName);
            MyFastReportsPlugin.DesignReport(ACalc);
        }

        //
        // New methods using the Fast-reports DLL:
        // This form generates a clutch of different reports.

        // In this method, a new FastReportsWrapper is used for each selected report type.

        private void GenerateAllSelectedReports(TRptCalculator ACalc)
        {
            // "Stewardship";
            MyFastReportsPlugin = new FastReportsWrapper("Stewardship");
            MyFastReportsPlugin.SetDataGetter(LoadStewardshipReportData);
            MyFastReportsPlugin.GenerateReport(ACalc);
        }

        private Boolean LoadStewardshipReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();
            pm.Add("param_ledger_number_i", FLedgerNumber);

            ArrayList reportParam = pm.Elems;
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

//          pm.Add("param_current_period", uco_GeneralSettings.GetCurrentPeiod());

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("Stewardship", paramsDictionary);

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
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);
            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = (ReportTable.Rows.Count > 0);

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Stewardship entries found for selected Run Number."), "Stewardship");
            }

            return HasData;
        }
    }
}