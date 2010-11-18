// auto generated with nant generateWinforms from GiftBatchExport.yaml
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
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{

  /// auto generated: Export Gift Batches
  public partial class TFrmGiftBatchExport: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmGiftBatchExport(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.rbtDetail.Text = Catalog.GetString("Detail");
      this.rbtSummary.Text = Catalog.GetString("Summary");
      this.lblDateSummary.Text = Catalog.GetString("Date for summary:");
      this.lblDontSummarize.Text = Catalog.GetString("Don't summarize with:");
      this.lblDontSummarizeAccount.Text = Catalog.GetString("Account:");
      this.rgrDetailSummary.Text = Catalog.GetString("Detail or Summary");
      this.rbtBaseCurrency.Text = Catalog.GetString("Base Currency");
      this.rbtOriginalTransactionCurrency.Text = Catalog.GetString("Original Transaction Currency");
      this.rgrCurrency.Text = Catalog.GetString("Currency");
      this.lblDateFrom.Text = Catalog.GetString("Date from:");
      this.lblDateTo.Text = Catalog.GetString("To:");
      this.lblBatchNumberStart.Text = Catalog.GetString("Batch Number from:");
      this.lblBatchNumberEnd.Text = Catalog.GetString("To:");
      this.rgrDateOrBatchRange.Text = Catalog.GetString("Date Or Batch Range");
      this.lblIncludeUnposted.Text = Catalog.GetString("Include Unposted Batches:");
      this.lblTransactionsOnly.Text = Catalog.GetString("Transactions Only:");
      this.lblFilename.Text = Catalog.GetString("Filename:");
      this.btnBrowseFilename.Text = Catalog.GetString("Browse Filename");
      this.lblDelimiter.Text = Catalog.GetString("Delimiter:");
      this.lblDateFormat.Text = Catalog.GetString("Date Format:");
      this.lblNumberFormat.Text = Catalog.GetString("Number Format:");
      this.btnHelp.Text = Catalog.GetString("&Help");
      this.btnOK.Text = Catalog.GetString("&Start");
      this.btnClose.Text = Catalog.GetString("&Close");
      this.tbbExportBatches.Text = Catalog.GetString("&Start");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Export Gift Batches");
      #endregion

      this.txtFilename.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      rbtSummaryCheckedChanged(null, null);
      rbtDateRangeCheckedChanged(null, null);
      rbtBatchNumberSelectionCheckedChanged(null, null);

    }

    void rbtSummaryCheckedChanged(object sender, System.EventArgs e)
    {
      dtpDateSummary.Enabled = rbtSummary.Checked;
      chkDontSummarize.Enabled = rbtSummary.Checked;
      cmbDontSummarizeAccount.Enabled = rbtSummary.Checked;
    }

    void rbtDateRangeCheckedChanged(object sender, System.EventArgs e)
    {
      dtpDateFrom.Enabled = rbtDateRange.Checked;
      dtpDateTo.Enabled = rbtDateRange.Checked;
    }

    void rbtBatchNumberSelectionCheckedChanged(object sender, System.EventArgs e)
    {
      txtBatchNumberStart.Enabled = rbtBatchNumberSelection.Checked;
      txtBatchNumberEnd.Enabled = rbtBatchNumberSelection.Checked;
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
        // TODO? Save Window position
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

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actHelp")
        {
            btnHelp.Enabled = e.Enabled;
            mniHelp.Enabled = e.Enabled;
        }
        if (e.ActionName == "actExportBatches")
        {
            btnOK.Enabled = e.Enabled;
            tbbExportBatches.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            btnClose.Enabled = e.Enabled;
            mniClose.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

#endregion
  }
}
