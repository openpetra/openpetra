// auto generated with nant generateWinforms from DonorReportShort.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
//
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

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{

  /// <summary>
  /// auto generated class for report
  /// </summary>
  public partial class TFrmDonorReportShort: System.Windows.Forms.Form, IFrmReporting
  {
    private TFrmPetraReportingUtils FPetraUtilsObject;

    /// <summary>
    /// constructor
    /// </summary>
    public TFrmDonorReportShort(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedger.Text = Catalog.GetString("Ledger:");
      this.rbtAllPartners.Text = Catalog.GetString("All Partner");
      this.rbtExtract.Text = Catalog.GetString("From Extract");
      this.txtExtract.ButtonText = Catalog.GetString("Find");
      this.rgrPartnerSelection.Text = Catalog.GetString("Select Partner");
      this.lblStartDate.Text = Catalog.GetString("From:");
      this.lblEndDate.Text = Catalog.GetString("To:");
      this.grpDateSelection.Text = Catalog.GetString("Select Date");
      this.lblCurrency.Text = Catalog.GetString("Currency:");
      this.grpCurrencySelection.Text = Catalog.GetString("Select Currency");
      this.tpgGeneralSettings.Text = Catalog.GetString("GeneralSettings");
      this.tpgColumns.Text = Catalog.GetString("Columns");
      this.tpgSorting.Text = Catalog.GetString("Sorting");
      this.rbtComplete.Text = Catalog.GetString("Complete");
      this.rbtWithoutDecimals.Text = Catalog.GetString("Without Decimals");
      this.rbtOnlyThousands.Text = Catalog.GetString("Only Thousands");
      this.rgrFormatCurrency.Text = Catalog.GetString("Format currency numbers:");
      this.tpgAdditionalSettings.Text = Catalog.GetString("Additional Settings");
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
      this.mniWrapColumn.Text = Catalog.GetString("&Wrap Columns");
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
      this.Text = Catalog.GetString("Donors Report - Short");
      #endregion

      FPetraUtilsObject = new TFrmPetraReportingUtils(AParentFormHandle, this, stbMain);

      FPetraUtilsObject.FXMLFiles = "FinancialDevelopment\\\\donorreportshort.xml,common.xml";
      FPetraUtilsObject.FReportName = "DonorReportShort";
      FPetraUtilsObject.FCurrentReport = "DonorReportShort";
      FPetraUtilsObject.FSettingsDirectory = "FinancialDevelopment";

      // Hook up Event that is fired by ucoReportColumns
      // ucoReportColumns.FillColumnGridEventHandler += new TFillColumnGridEventHandler(FPetraUtilsObject.FillColumnGrid);
      FPetraUtilsObject.InitialiseData("");
      // FPetraUtilsObject.InitialiseSettingsGui(ucoReportColumns, mniLoadSettings, /*ConMnuLoadSettings*/null,
      //                                 mniSaveSettings, mniSaveSettingsAs, mniLoadSettingsDialog, mniMaintainSettings);
      this.SetAvailableFunctions();

      rbtExtractCheckedChanged(null, null);
      ucoReportColumns.InitialiseData(FPetraUtilsObject);
      ucoReportSorting.InitialiseData(FPetraUtilsObject);

      ucoReportColumns.PetraUtilsObject = FPetraUtilsObject;
      ucoReportColumns.InitUserControl();
      ucoReportSorting.PetraUtilsObject = FPetraUtilsObject;
      ucoReportSorting.InitUserControl();

      FPetraUtilsObject.LoadDefaultSettings();
    }

    void rbtExtractCheckedChanged(object sender, System.EventArgs e)
    {
      txtExtract.Enabled = rbtExtract.Checked;
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

      ReadControlsVerify(ACalc, AReportAction);

      if (rbtAllPartners.Checked)
      {
          ACalc.AddParameter("param_partner_source", "AllPartner");
      }
      if (rbtExtract.Checked)
      {
          ACalc.AddParameter("param_partner_source", "Extract");
      }
      ACalc.AddParameter("param_start_date", this.dtpStartDate.Date);
      ACalc.AddParameter("param_end_date", this.dtpEndDate.Date);
      if (this.cmbCurrency.SelectedItem != null)
      {
          ACalc.AddParameter("param_currency", this.cmbCurrency.SelectedItem.ToString());
      }
      else
      {
          ACalc.AddParameter("param_currency", "");
      }
      ucoReportColumns.ReadControls(ACalc, AReportAction);
      ucoReportSorting.ReadControls(ACalc, AReportAction);
      if (rbtComplete.Checked)
      {
          ACalc.AddParameter("param_currency_format", "CurrencyComplete");
      }
      if (rbtWithoutDecimals.Checked)
      {
          ACalc.AddParameter("param_currency_format", "CurrencyWithoutDecimals");
      }
      if (rbtOnlyThousands.Checked)
      {
          ACalc.AddParameter("param_currency_format", "CurrencyThousands");
      }
      ReadControlsManual(ACalc, AReportAction);

    }

    /**
       Sets the selected values in the controls, using the parameters loaded from a file

    */
    public void SetControls(TParameterList AParameters)
    {

      rbtAllPartners.Checked = AParameters.Get("param_partner_source").ToString() == "AllPartner";
      rbtExtract.Checked = AParameters.Get("param_partner_source").ToString() == "Extract";
      DateTime dtpStartDateDate = AParameters.Get("param_start_date").ToDate();
      if ((dtpStartDateDate <= DateTime.MinValue)
          || (dtpStartDateDate >= DateTime.MaxValue))
      {
          dtpStartDateDate = DateTime.Now;
      }
      dtpStartDate.Date = dtpStartDateDate;
      DateTime dtpEndDateDate = AParameters.Get("param_end_date").ToDate();
      if ((dtpEndDateDate <= DateTime.MinValue)
          || (dtpEndDateDate >= DateTime.MaxValue))
      {
          dtpEndDateDate = DateTime.Now;
      }
      dtpEndDate.Date = dtpEndDateDate;
      cmbCurrency.SelectedValue = AParameters.Get("param_currency").ToString();
      ucoReportColumns.SetControls(AParameters);
      ucoReportSorting.SetControls(AParameters);
      rbtComplete.Checked = AParameters.Get("param_currency_format").ToString() == "CurrencyComplete";
      rbtWithoutDecimals.Checked = AParameters.Get("param_currency_format").ToString() == "CurrencyWithoutDecimals";
      rbtOnlyThousands.Checked = AParameters.Get("param_currency_format").ToString() == "CurrencyThousands";
      SetControlsManual(AParameters);
    }
#endregion

#region Column Functions and Calculations
    /**
       This will add functions to the list of available functions

    */
    public void SetAvailableFunctions()
    {
      //ArrayList availableFunctions = FPetraUtilsObject.InitAvailableFunctions();

      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("DonorKey", 2.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("First Name", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Surname", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Address line 1", 2.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Address line 3", 2.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Street", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Post Code", 1.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("City", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Country", 2.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("County", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Telephone Number", 2.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Telephone Extension", 1.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Alternate Phone", 2.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Fax Number", 2.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Fax Extension", 1.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Mobile Number", 2.5));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("E-Mail", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Partner Name", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Total Given", 3.0));
      FPetraUtilsObject.AddAvailableFunction(new TPartnerColumnFunction("Partner Class", 2.0));

      ucoReportColumns.SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());
      ucoReportSorting.SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());

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
    /// <param name="AReportParameter">Initialisation values needed for some reports</param>
    public void InitialiseData(String AReportParameter)
    {
        FPetraUtilsObject.InitialiseData(AReportParameter);
    }

    /// <summary>
    /// Checks / Unchecks the menu item "Wrap Columns"
    /// </summary>
    /// <param name="ACheck">True if menu item is to be checked. Otherwise false</param>
    public void CheckWrapColumnMenuItem(bool ACheck)
    {
        this.mniWrapColumn.Checked = ACheck;
    }

    /// <summary>
    /// activate and deactivate toolbar buttons and menu items depending on ongoing report calculation
    /// </summary>
    /// <param name="ABusy">True if a report is generated and the close button should be disabled.</param>
    public void EnableBusy(bool ABusy)
    {
        mniClose.Enabled = !ABusy;

        if (ABusy == false)
        {
            mniGenerateReport.Text = Catalog.GetString("&Generate Report...");
            tbbGenerateReport.Text = Catalog.GetString("Generate");
            tbbGenerateReport.ToolTipText = Catalog.GetString("Generate a report and display the preview");
        }
        else
        {
            mniGenerateReport.Text = Catalog.GetString("&Cancel Report");
            tbbGenerateReport.Text = Catalog.GetString("Cancel");
            tbbGenerateReport.ToolTipText = Catalog.GetString("Cancel the calculation of the report (after cancelling it might still take a while)");
        }
    }

#endregion

    /// <summary>
    /// allow to store and load settings
    /// </summary>
    /// <param name="AEnabled">True if the store and load settings are to be enabled.</param>
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
    /// this is used for writing the captions of the menu items and toolbar buttons for recently used report settings
    /// </summary>
    /// <returns>false if an item with that index does not exist</returns>
    /// <param name="AIndex"></param>
    /// <param name="mniItem"></param>
    /// <param name="tbbItem"></param>
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

    /// auto generated
    protected void actWrapColumn(object sender, EventArgs e)
    {
        FPetraUtilsObject.MI_WrapColumn_Click(sender, e);
    }

#endregion
  }
}
