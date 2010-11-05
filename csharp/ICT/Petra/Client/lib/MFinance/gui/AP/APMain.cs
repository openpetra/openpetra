// auto generated with nant generateWinforms from APMain.yaml and template windowFind
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

namespace Ict.Petra.Client.MFinance.Gui.AP
{

  /// auto generated: Accounts Payable
  public partial class TFrmAPMain: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmAPMain(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblSupplierCode.Text = Catalog.GetString("S&earch Supplier:");
      this.chkDueToday.Text = Catalog.GetString("Due &Today");
      this.chkOverdue.Text = Catalog.GetString("&Overdue");
      this.chkDueFuture.Text = Catalog.GetString("Due &Within Future");
      this.cmbTimeUnit.Text = Catalog.GetString("Days");
      this.btnSearch.Text = Catalog.GetString("&Search");
      this.btnReset.Text = Catalog.GetString("&Reset Criteria");
      this.chkShowOutstandingAmounts.Text = Catalog.GetString("Show Outstanding &Amounts");
      this.chkHideInactiveSuppliers.Text = Catalog.GetString("Hide &Inactive Suppliers");
      this.lblSupplierCurrency.Text = Catalog.GetString("C&urrency:");
      this.tpgSuppliers.Text = Catalog.GetString("Suppliers");
      this.btnTagAllApprovable.Text = Catalog.GetString("Tag all Appro&vable");
      this.btnTagAllPostable.Text = Catalog.GetString("Tag a&ll Postable");
      this.btnTagAllPayable.Text = Catalog.GetString("Tag all Paya&ble");
      this.btnUntagAll.Text = Catalog.GetString("&Untag all");
      this.lblSumTagged.Text = Catalog.GetString("Sum of Tagged:");
      this.tpgOutstandingInvoices.Text = Catalog.GetString("Outstanding Invoices");
      this.tbbTransactions.ToolTipText = Catalog.GetString("Open the transactions of the supplier");
      this.tbbTransactions.Text = Catalog.GetString("Open Transactions");
      this.tbbEditSupplier.ToolTipText = Catalog.GetString("Change the details and settings of an existing supplier");
      this.tbbEditSupplier.Text = Catalog.GetString("&Edit Supplier");
      this.tbbNewSupplier.ToolTipText = Catalog.GetString("Create a new supplier");
      this.tbbNewSupplier.Text = Catalog.GetString("&New Supplier");
      this.tbbCreateInvoice.Text = Catalog.GetString("Create &Invoice");
      this.tbbCreateCreditNote.Text = Catalog.GetString("Create C&redit Note");
      this.mniReports.Text = Catalog.GetString("&Reports");
      this.mniReprintPaymentReport.Text = Catalog.GetString("Reprint Pa&yment Report");
      this.mniImport.Text = Catalog.GetString("&Import");
      this.mniExport.Text = Catalog.GetString("&Export");
      this.mniDefaults.Text = Catalog.GetString("AP &Defaults");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniNewSupplier.ToolTipText = Catalog.GetString("Create a new supplier");
      this.mniNewSupplier.Text = Catalog.GetString("&New Supplier");
      this.mniTransactions.ToolTipText = Catalog.GetString("Open the transactions of the supplier");
      this.mniTransactions.Text = Catalog.GetString("Open Transactions");
      this.mniEditSupplier.ToolTipText = Catalog.GetString("Change the details and settings of an existing supplier");
      this.mniEditSupplier.Text = Catalog.GetString("&Edit Supplier");
      this.mniCreateInvoice.Text = Catalog.GetString("Create &Invoice");
      this.mniCreateCreditNote.Text = Catalog.GetString("Create C&redit Note");
      this.mniSupplier.Text = Catalog.GetString("Supplier");
      this.mniFindInvoice.Text = Catalog.GetString("&Find Invoice...");
      this.mniFind.Text = Catalog.GetString("Find");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Accounts Payable");
      #endregion

      this.txtSumTagged.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(cmbSupplierCode, Catalog.GetString("Search by supplier name or partner key"));
      this.cmbSupplierCode.AcceptNewEntries += new TAcceptNewEntryEventHandler(FPetraUtilsObject.AddComboBoxHistory);
      FPetraUtilsObject.LoadComboBoxHistory(cmbSupplierCode);
      this.AcceptButton = btnSearch;
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      chkDueFutureCheckedChanged(null, null);
      ActionEnabledEvent(null, new ActionEventArgs("cndSelectedSupplier", false));

    }

    void chkDueFutureCheckedChanged(object sender, System.EventArgs e)
    {
      nudNumberTimeUnits.Enabled = chkDueFuture.Checked;
      cmbTimeUnit.Enabled = chkDueFuture.Checked;
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
        if (e.ActionName == "actSearch")
        {
            btnSearch.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSupplierTransactions")
        {
            tbbTransactions.Enabled = e.Enabled;
            mniTransactions.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditSupplier")
        {
            tbbEditSupplier.Enabled = e.Enabled;
            mniEditSupplier.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewSupplier")
        {
            tbbNewSupplier.Enabled = e.Enabled;
            mniNewSupplier.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateInvoice")
        {
            tbbCreateInvoice.Enabled = e.Enabled;
            mniCreateInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateCreditNote")
        {
            tbbCreateCreditNote.Enabled = e.Enabled;
            mniCreateCreditNote.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFindInvoice")
        {
            mniFindInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "cndSelectedSupplier")
        {
            FPetraUtilsObject.EnableAction("actSupplierTransactions", e.Enabled);
            FPetraUtilsObject.EnableAction("actEditSupplier", e.Enabled);
            FPetraUtilsObject.EnableAction("actCreateInvoice", e.Enabled);
            FPetraUtilsObject.EnableAction("actCreateCreditNote", e.Enabled);
        }
        mniReports.Enabled = false;
        mniReprintPaymentReport.Enabled = false;
        mniImport.Enabled = false;
        mniExport.Enabled = false;
        mniDefaults.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
