/* auto generated with nant generateWinforms from AccountDetail.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Mono.Unix;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{

  /// <summary>
  /// auto generated class for report
  /// </summary>
  public partial class TFrmAccountDetail: System.Windows.Forms.Form, IFrmReporting
  {
    private TFrmPetraReportingUtils FPetraUtilsObject;

    /// <summary>
    /// constructor
    /// </summary>
    public TFrmAccountDetail(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedger.Text = Catalog.GetString("Ledger:");
      this.lblAccountHierarchy.Text = Catalog.GetString("Account Hierarchy:");
      this.grpLedger.Text = Catalog.GetString("Ledger Details");
      this.lblCurrency.Text = Catalog.GetString("Currency:");
      this.grpCurrency.Text = Catalog.GetString("Currency");
      this.rbtPeriodRange.Text = Catalog.GetString("Period Range");
      this.lblStartPeriod.Text = Catalog.GetString("from:");
      this.lblEndPeriod.Text = Catalog.GetString("to:");
      this.lblPeriodYear.Text = Catalog.GetString("Year:");
      this.rbtDateRange.Text = Catalog.GetString("Date Range");
      this.lblDateEnd.Text = Catalog.GetString("to:");
      this.rgrPeriod.Text = Catalog.GetString("Period");
      this.rbtSortByAccount.Text = Catalog.GetString("Sort by Account");
      this.rbtSortByCostCentre.Text = Catalog.GetString("Sort by Cost Centre");
      this.rbtSortByReference.Text = Catalog.GetString("Sort By Reference");
      this.lblReferenceFrom.Text = Catalog.GetString("from:");
      this.lblReferenceTo.Text = Catalog.GetString("to:");
      this.rbtSortByAnalysisType.Text = Catalog.GetString("Sort By Analysis Type");
      this.lblAnalysisTypeFrom.Text = Catalog.GetString("from:");
      this.lblAnalysisTypeTo.Text = Catalog.GetString("to:");
      this.rgrSorting.Text = Catalog.GetString("Sorting");
      this.tpgReportSpecific.Text = Catalog.GetString("General Settings");
      this.rbtAccountRange.Text = Catalog.GetString("Account Range");
      this.lblAccountStart.Text = Catalog.GetString("From:");
      this.lblAccountEnd.Text = Catalog.GetString("To:");
      this.rbtAccountList.Text = Catalog.GetString("Account List");
      this.btnUnselectAllAccounts.Text = Catalog.GetString("Unselect All");
      this.rgrAccounts.Text = Catalog.GetString("Accounts");
      this.rbtCostCentreRange.Text = Catalog.GetString("Cost Centre Range");
      this.lblCostCentreStart.Text = Catalog.GetString("From:");
      this.lblCostCentreEnd.Text = Catalog.GetString("To:");
      this.rbtCostCentreList.Text = Catalog.GetString("Cost Centre List");
      this.btnUnselectAllCostCentres.Text = Catalog.GetString("Unselect All");
      this.rgrCostCentres.Text = Catalog.GetString("Cost Centres");
      this.tpgCCAccount.Text = Catalog.GetString("Account/CostCentre Settings");
      this.tbbGenerateReport.ToolTipText = Catalog.GetString("Generate the report");
      this.tbbGenerateReport.Text = Catalog.GetString("&Generate");
      this.tbbSaveSettings.Text = Catalog.GetString("&Save Settings");
      this.tbbSaveSettingsAs.Text = Catalog.GetString("Save Settings &As...");
      this.tbbLoadSettingsDialog.Text = Catalog.GetString("&Open...");
      this.mniLoadSettingsDialog.Text = Catalog.GetString("&Open...");
      this.mniLoadSettings1.Text = Catalog.GetString("RecentSettings");
      this.mniLoadSettings2.Text = Catalog.GetString("RecentSettings");
      this.mniLoadSettings3.Text = Catalog.GetString("RecentSettings");
      this.mniLoadSettings4.Text = Catalog.GetString("RecentSettings");
      this.mniLoadSettings5.Text = Catalog.GetString("RecentSettings");
      this.mniLoadSettings.Text = Catalog.GetString("&Load Settings");
      this.mniSaveSettings.Text = Catalog.GetString("&Save Settings");
      this.mniSaveSettingsAs.Text = Catalog.GetString("Save Settings &As...");
      this.mniMaintainSettings.Text = Catalog.GetString("&Maintain Settings...");
      this.mniGenerateReport.ToolTipText = Catalog.GetString("Generate the report");
      this.mniGenerateReport.Text = Catalog.GetString("&Generate");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Account Detail");
      #endregion

      this.txtLedger.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtStartPeriod.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtEndPeriod.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtReferenceFrom.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtReferenceTo.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtAnalysisTypeFrom.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtAnalysisTypeTo.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraReportingUtils(AParentFormHandle, this, stbMain);

      FPetraUtilsObject.FXMLFiles = "Finance\\\\accountdetail.xml,Finance\\\\accountdetailcommon.xml,Finance\\\\finance.xml,common.xml";
      FPetraUtilsObject.FReportName = "Account Detail";
      FPetraUtilsObject.FCurrentReport = "Account Detail";
	  FPetraUtilsObject.FSettingsDirectory = "Finance";

      // Hook up Event that is fired by ucoReportColumns
      // ucoReportColumns.FillColumnGridEventHandler += new TFillColumnGridEventHandler(FPetraUtilsObject.FillColumnGrid);
      FPetraUtilsObject.InitialiseData("");
      // FPetraUtilsObject.InitialiseSettingsGui(ucoReportColumns, mniLoadSettings, /*ConMnuLoadSettings*/null,
      //                                 mniSaveSettings, mniSaveSettingsAs, mniLoadSettingsDialog, mniMaintainSettings);
      this.SetAvailableFunctions();

      rbtPeriodRangeCheckedChanged(null, null);
      rbtDateRangeCheckedChanged(null, null);
      rbtSortByReferenceCheckedChanged(null, null);
      rbtSortByAnalysisTypeCheckedChanged(null, null);
      rbtAccountRangeCheckedChanged(null, null);
      rbtAccountListCheckedChanged(null, null);
      rbtCostCentreRangeCheckedChanged(null, null);
      rbtCostCentreListCheckedChanged(null, null);
	
	  FPetraUtilsObject.LoadDefaultSettings();
    }

    void rbtPeriodRangeCheckedChanged(object sender, System.EventArgs e)
    {
      txtStartPeriod.Enabled = rbtPeriodRange.Checked;
      txtEndPeriod.Enabled = rbtPeriodRange.Checked;
      cmbPeriodYear.Enabled = rbtPeriodRange.Checked;
    }

    void rbtDateRangeCheckedChanged(object sender, System.EventArgs e)
    {
      dtpDateStart.Enabled = rbtDateRange.Checked;
      dtpDateEnd.Enabled = rbtDateRange.Checked;
    }

    void rbtSortByReferenceCheckedChanged(object sender, System.EventArgs e)
    {
      txtReferenceFrom.Enabled = rbtSortByReference.Checked;
      txtReferenceTo.Enabled = rbtSortByReference.Checked;
    }

    void rbtSortByAnalysisTypeCheckedChanged(object sender, System.EventArgs e)
    {
      txtAnalysisTypeFrom.Enabled = rbtSortByAnalysisType.Checked;
      txtAnalysisTypeTo.Enabled = rbtSortByAnalysisType.Checked;
    }

    void rbtAccountRangeCheckedChanged(object sender, System.EventArgs e)
    {
      cmbAccountStart.Enabled = rbtAccountRange.Checked;
      cmbAccountEnd.Enabled = rbtAccountRange.Checked;
    }

    void rbtAccountListCheckedChanged(object sender, System.EventArgs e)
    {
      clbAccounts.Enabled = rbtAccountList.Checked;
      btnUnselectAllAccounts.Enabled = rbtAccountList.Checked;
    }

    void rbtCostCentreRangeCheckedChanged(object sender, System.EventArgs e)
    {
      cmbCostCentreStart.Enabled = rbtCostCentreRange.Checked;
      cmbCostCentreEnd.Enabled = rbtCostCentreRange.Checked;
    }

    void rbtCostCentreListCheckedChanged(object sender, System.EventArgs e)
    {
      clbCostCentres.Enabled = rbtCostCentreList.Checked;
      btnUnselectAllCostCentres.Enabled = rbtCostCentreList.Checked;
    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Load(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Load(sender, e);
    }

    private void TFrmPetra_Closing(object sender, CancelEventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Closing(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
    }
#region Parameter/Settings Handling
    /**
       Reads the selected values from the controls, and stores them into the parameter system of FCalculator

    */
    public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
    {
      ACalc.SetMaxDisplayColumns(FPetraUtilsObject.FMaxDisplayColumns);

      ACalc.AddParameter("param_account_hierarchy_c", this.cmbAccountHierarchy.GetSelectedString());
      if (this.cmbCurrency.SelectedItem != null)
      {
        ACalc.AddParameter("param_currency", this.cmbCurrency.SelectedItem.ToString());
      }
      else
      {
        ACalc.AddParameter("param_currency", "");
      }
      ACalc.AddParameter("param_start_period_i", this.txtStartPeriod.Text);
      ACalc.AddParameter("param_end_period_i", this.txtEndPeriod.Text);
      ACalc.AddParameter("param_year_i", this.cmbPeriodYear.GetSelectedString());
      ACalc.AddParameter("param_start_date", this.dtpDateStart.Date);
      ACalc.AddParameter("param_end_date", this.dtpDateEnd.Date);
      if (rbtPeriodRange.Checked)
      {
        ACalc.AddParameter("param_rgrPeriod", "PeriodRange");
      }
      if (rbtDateRange.Checked)
      {
        ACalc.AddParameter("param_rgrPeriod", "DateRange");
      }
      ACalc.AddParameter("param_reference_start", this.txtReferenceFrom.Text);
      ACalc.AddParameter("param_reference_end", this.txtReferenceTo.Text);
      ACalc.AddParameter("param_analyis_type_start", this.txtAnalysisTypeFrom.Text);
      ACalc.AddParameter("param_analyis_type_end", this.txtAnalysisTypeTo.Text);
      if (rbtSortByAccount.Checked)
      {
        ACalc.AddParameter("param_sortby", "Account");
      }
      if (rbtSortByCostCentre.Checked)
      {
        ACalc.AddParameter("param_sortby", "Cost Centre");
      }
      if (rbtSortByReference.Checked)
      {
        ACalc.AddParameter("param_sortby", "Reference");
      }
      if (rbtSortByAnalysisType.Checked)
      {
        ACalc.AddParameter("param_sortby", "Analysis Type");
      }
      ACalc.AddParameter("param_account_code_start", this.cmbAccountStart.GetSelectedString());
      ACalc.AddParameter("param_account_code_end", this.cmbAccountEnd.GetSelectedString());
      ACalc.AddParameter("param_account_codes", this.clbAccounts.GetCheckedStringList());
      if (rbtAccountRange.Checked)
      {
        ACalc.AddParameter("param_rgrAccounts", "AccountRange");
      }
      if (rbtAccountList.Checked)
      {
        ACalc.AddParameter("param_rgrAccounts", "AccountList");
      }
      ACalc.AddParameter("param_cost_centre_code_start", this.cmbCostCentreStart.GetSelectedString());
      ACalc.AddParameter("param_cost_centre_code_end", this.cmbCostCentreEnd.GetSelectedString());
      ACalc.AddParameter("param_cost_centre_codes", this.clbCostCentres.GetCheckedStringList());
      if (rbtCostCentreRange.Checked)
      {
        ACalc.AddParameter("param_rgrCostCentres", "CostCentreRange");
      }
      if (rbtCostCentreList.Checked)
      {
        ACalc.AddParameter("param_rgrCostCentres", "CostCentreList");
      }
      ReadControlsManual(ACalc);

    }

    /**
       Sets the selected values in the controls, using the parameters loaded from a file

    */
    public void SetControls(TParameterList AParameters)
    {

      cmbAccountHierarchy.SetSelectedString(AParameters.Get("param_account_hierarchy_c").ToString());
      cmbCurrency.SelectedValue = AParameters.Get("param_currency").ToString();
      txtStartPeriod.Text = AParameters.Get("param_start_period_i").ToString();
      txtEndPeriod.Text = AParameters.Get("param_end_period_i").ToString();
      cmbPeriodYear.SetSelectedString(AParameters.Get("param_year_i").ToString());
      DateTime dtpDateStartDate = AParameters.Get("param_start_date").ToDate();
      if ((dtpDateStartDate <= DateTime.MinValue)
          || (dtpDateStartDate >= DateTime.MaxValue))
      {
          dtpDateStartDate = DateTime.Now;
      }
      dtpDateStart.Date = dtpDateStartDate;
      DateTime dtpDateEndDate = AParameters.Get("param_end_date").ToDate();
      if ((dtpDateEndDate <= DateTime.MinValue)
          || (dtpDateEndDate >= DateTime.MaxValue))
      {
          dtpDateEndDate = DateTime.Now;
      }
      dtpDateEnd.Date = dtpDateEndDate;
      rbtPeriodRange.Checked = AParameters.Get("param_rgrPeriod").ToString() == "PeriodRange";
      rbtDateRange.Checked = AParameters.Get("param_rgrPeriod").ToString() == "DateRange";
      txtReferenceFrom.Text = AParameters.Get("param_reference_start").ToString();
      txtReferenceTo.Text = AParameters.Get("param_reference_end").ToString();
      txtAnalysisTypeFrom.Text = AParameters.Get("param_analyis_type_start").ToString();
      txtAnalysisTypeTo.Text = AParameters.Get("param_analyis_type_end").ToString();
      rbtSortByAccount.Checked = AParameters.Get("param_sortby").ToString() == "Account";
      rbtSortByCostCentre.Checked = AParameters.Get("param_sortby").ToString() == "Cost Centre";
      rbtSortByReference.Checked = AParameters.Get("param_sortby").ToString() == "Reference";
      rbtSortByAnalysisType.Checked = AParameters.Get("param_sortby").ToString() == "Analysis Type";
      cmbAccountStart.SetSelectedString(AParameters.Get("param_account_code_start").ToString());
      cmbAccountEnd.SetSelectedString(AParameters.Get("param_account_code_end").ToString());
      clbAccounts.SetCheckedStringList(AParameters.Get("param_account_codes").ToString());
      rbtAccountRange.Checked = AParameters.Get("param_rgrAccounts").ToString() == "AccountRange";
      rbtAccountList.Checked = AParameters.Get("param_rgrAccounts").ToString() == "AccountList";
      cmbCostCentreStart.SetSelectedString(AParameters.Get("param_cost_centre_code_start").ToString());
      cmbCostCentreEnd.SetSelectedString(AParameters.Get("param_cost_centre_code_end").ToString());
      clbCostCentres.SetCheckedStringList(AParameters.Get("param_cost_centre_codes").ToString());
      rbtCostCentreRange.Checked = AParameters.Get("param_rgrCostCentres").ToString() == "CostCentreRange";
      rbtCostCentreList.Checked = AParameters.Get("param_rgrCostCentres").ToString() == "CostCentreList";
    }
#endregion

#region Column Functions and Calculations
    /**
       This will add functions to the list of available functions

    */
    public void SetAvailableFunctions()
    {
      //ArrayList availableFunctions = FPetraUtilsObject.InitAvailableFunctions();
	
	

    }
#endregion

#region Implement interface functions

    /// <summary>
    /// only run this code once during activation
    /// </summary>
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    /// <returns>void</returns>
    public void HookupAllControls()
    {
    }

    /// <summary>
    /// check if report window can be closed
    /// </summary>
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// <summary>
    /// access to the utility object
    /// </summary>
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }

    /// <summary>
    /// initialisation
    /// </summary>
    public void InitialiseData(String AReportParameter)
    {
        FPetraUtilsObject.InitialiseData(AReportParameter);
    }
#endregion

    /// <summary>
    /// allow to store and load settings
    /// </summary>
    public void EnableSettings(bool AEnabled)
    {
        foreach (ToolStripItem item in mniLoadSettings.DropDownItems)
        {
            item.Enabled = AEnabled;
        }
        mniLoadSettings.Enabled = AEnabled;
        mniSaveSettings.Enabled = AEnabled;
        mniSaveSettingsAs.Enabled = AEnabled;
        mniMaintainSettings.Enabled = AEnabled;
        //tbbLoadSettings.Enabled = AEnabled;
        tbbSaveSettings.Enabled = AEnabled;
        tbbSaveSettingsAs.Enabled = AEnabled;
    }

    /// <summary>
    /// activate and deactivate toolbar buttons and menu items depending on ongoing report calculation
    /// </summary>
    public void EnableBusy(bool ABusy)
    {
        mniClose.Enabled = !ABusy;

        if (ABusy == false)
        {
            mniGenerateReport.Text = "&Generate Report...";
            tbbGenerateReport.Text = "Generate";
            tbbGenerateReport.ToolTipText = "Generate a report and display the preview";
        }
        else
        {
            mniGenerateReport.Text = "&Cancel Report";
            tbbGenerateReport.Text = "Cancel";
            tbbGenerateReport.ToolTipText = "Cancel the calculation of the report (after cancelling it might still take a while)";
        }
    }

    /// <summary>
    /// this is used for writing the captions of the menu items and toolbar buttons for recently used report settings
    /// </summary>
    /// <returns>false if an item with that index does not exist</returns>
    public bool GetRecentSettingsItems(int AIndex, out ToolStripItem mniItem, out ToolStripItem tbbItem)
    {
        if (AIndex < 0 || AIndex >= mniLoadSettings.DropDownItems.Count - 2)
        {
            mniItem = null;
            tbbItem = null;
            return false;
        }
        mniItem = mniLoadSettings.DropDownItems[AIndex + 2];
        // TODO
        tbbItem = null;
        return true;
    }

#region Action Handling

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

    /// auto generated
    protected void actGenerateReport(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_GenerateReport_Click(sender, e);
    }

    /// auto generated
    protected void actSaveSettingsAs(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_SaveSettingsAs_Click(sender, e);
    }

    /// auto generated
    protected void actSaveSettings(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_SaveSettings_Click(sender, e);
    }

    /// auto generated
    protected void actLoadSettingsDialog(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_LoadSettingsDialog_Click(sender, e);
    }

    /// auto generated
    protected void actLoadSettings(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_LoadSettings_Click(sender, e);
    }

    /// auto generated
    protected void actMaintainSettings(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_MaintainSettings_Click(sender, e);
    }

#endregion
  }
}
