// auto generated with nant generateWinforms from UC_GeneralSettings.yaml and template usercontrolUnbound
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{

  /// auto generated user control
  public partial class TFrmUC_GeneralSettings: UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraReportingUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS FMainDS;

    /// constructor
    public TFrmUC_GeneralSettings() : base()
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
      this.rbtPeriod.Text = Catalog.GetString("Period");
      this.lblStartPeriod.Text = Catalog.GetString("from:");
      this.lblEndPeriod.Text = Catalog.GetString("to:");
      this.lblPeriodYear.Text = Catalog.GetString("Year:");
      this.rbtQuarter.Text = Catalog.GetString("Quarter");
      this.lblQuarter.Text = Catalog.GetString("from:");
      this.lblQuarterYear.Text = Catalog.GetString("Year:");
      this.rbtDate.Text = Catalog.GetString("Date");
      this.lblStartDate.Text = Catalog.GetString("from:");
      this.lblEndDate.Text = Catalog.GetString("to:");
      this.grpPeriodRange.Text = Catalog.GetString("Period Range");
      #endregion

      this.txtLedger.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtStartPeriod.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtEndPeriod.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtQuarter.Font = TAppSettingsManager.GetDefaultBoldFont();
    }

    /// helper object for the whole screen
    public TFrmPetraReportingUtils PetraUtilsObject
    {
        get
        {
            return FPetraUtilsObject;
        }

        set
        {
            FPetraUtilsObject = value;
        }
    }

    /// dataset for the whole screen
    public Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingStarted(object sender, EventArgs e)
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(sender, e);
        }
    }

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingFinished(object sender, EventArgs e)
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(sender, e);
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
    }

    void rbtPeriodCheckedChanged(object sender, System.EventArgs e)
    {
      txtStartPeriod.Enabled = rbtPeriod.Checked;
      txtEndPeriod.Enabled = rbtPeriod.Checked;
      cmbPeriodYear.Enabled = rbtPeriod.Checked;
    }

    void rbtQuarterCheckedChanged(object sender, System.EventArgs e)
    {
      txtQuarter.Enabled = rbtQuarter.Checked;
      cmbQuarterYear.Enabled = rbtQuarter.Checked;
    }

    void rbtDateCheckedChanged(object sender, System.EventArgs e)
    {
      dtpStartDate.Enabled = rbtDate.Checked;
      dtpEndDate.Enabled = rbtDate.Checked;
    }

#region Implement interface functions
    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion
  }
}
