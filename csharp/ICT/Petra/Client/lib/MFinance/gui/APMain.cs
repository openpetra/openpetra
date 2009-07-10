/* auto generated with nant generateWinforms from APMain.yaml
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
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui
{

  /// auto generated: Accounts Payable
  public partial class TFrmAccountsPayableMain: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmAccountsPayableMain(IntPtr AParentFormHandle) : base()
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
      this.tpgOutstandingInvoices.Text = Catalog.GetString("OutstandingInvoices");
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

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      this.cmbSupplierCode.AcceptNewEntries += new TAcceptNewEntryEventHandler(FPetraUtilsObject.AddComboBoxHistory);
      FPetraUtilsObject.LoadComboBoxHistory(cmbSupplierCode);
      this.AcceptButton = btnSearch;
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      chkDueFutureCheckedChanged(null, null);
      ActivateSelectedSupplier(false);
    }

    void chkDueFutureCheckedChanged(object sender, System.EventArgs e)
    {
      nudNumberTimeUnits.Enabled = chkDueFuture.Checked;
      cmbTimeUnit.Enabled = chkDueFuture.Checked;
    }

    private void btnSearchClick(object sender, EventArgs e)
    {
        actSearch(sender, e);
    }

    private void grdSupplierResultDoubleClick(object sender, EventArgs e)
    {
        actSupplierTransactions(sender, e);
    }

    private void tbbTransactionsClick(object sender, EventArgs e)
    {
        actSupplierTransactions(sender, e);
    }

    private void tbbEditSupplierClick(object sender, EventArgs e)
    {
        actEditSupplier(sender, e);
    }

    private void tbbNewSupplierClick(object sender, EventArgs e)
    {
        actNewSupplier(sender, e);
    }

    private void tbbCreateInvoiceClick(object sender, EventArgs e)
    {
        actCreateInvoice(sender, e);
    }

    private void tbbCreateCreditNoteClick(object sender, EventArgs e)
    {
        actCreateCreditNote(sender, e);
    }

    private void mniCloseClick(object sender, EventArgs e)
    {
        actClose(sender, e);
    }

    private void mniNewSupplierClick(object sender, EventArgs e)
    {
        actNewSupplier(sender, e);
    }

    private void mniTransactionsClick(object sender, EventArgs e)
    {
        actSupplierTransactions(sender, e);
    }

    private void mniEditSupplierClick(object sender, EventArgs e)
    {
        actEditSupplier(sender, e);
    }

    private void mniCreateInvoiceClick(object sender, EventArgs e)
    {
        actCreateInvoice(sender, e);
    }

    private void mniCreateCreditNoteClick(object sender, EventArgs e)
    {
        actCreateCreditNote(sender, e);
    }

    private void mniFindInvoiceClick(object sender, EventArgs e)
    {
        actFindInvoice(sender, e);
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
        }
        if (e.ActionName == "actEditSupplier")
        {
            tbbEditSupplier.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewSupplier")
        {
            tbbNewSupplier.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateInvoice")
        {
            tbbCreateInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateCreditNote")
        {
            tbbCreateCreditNote.Enabled = e.Enabled;
        }
        mniReports.Enabled = false;
        mniReprintPaymentReport.Enabled = false;
        mniSeparator0.Enabled = false;
        mniImport.Enabled = false;
        mniExport.Enabled = false;
        mniSeparator1.Enabled = false;
        mniDefaults.Enabled = false;
        mniSeparator2.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewSupplier")
        {
            mniNewSupplier.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSupplierTransactions")
        {
            mniTransactions.Enabled = e.Enabled;
        }
        if (e.ActionName == "actEditSupplier")
        {
            mniEditSupplier.Enabled = e.Enabled;
        }
        mniSeparator3.Enabled = false;
        if (e.ActionName == "actCreateInvoice")
        {
            mniCreateInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actCreateCreditNote")
        {
            mniCreateCreditNote.Enabled = e.Enabled;
        }
        if (e.ActionName == "actFindInvoice")
        {
            mniFindInvoice.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniSeparator4.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator5.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void ActivateSelectedSupplier(bool AEnabled)
    {
        FPetraUtilsObject.EnableAction("actSupplierTransactions", AEnabled);
        FPetraUtilsObject.EnableAction("actEditSupplier", AEnabled);
        FPetraUtilsObject.EnableAction("actCreateInvoice", AEnabled);
        FPetraUtilsObject.EnableAction("actCreateCreditNote", AEnabled);
    }

    /// auto generated
    protected void actSearch(object sender, EventArgs e)
    {
        SearchForSupplier(sender, e);
    }

    /// auto generated
    protected void actSupplierTransactions(object sender, EventArgs e)
    {
        SupplierTransactions(sender, e);
    }

    /// auto generated
    protected void actNewSupplier(object sender, EventArgs e)
    {
        NewSupplier(sender, e);
    }

    /// auto generated
    protected void actEditSupplier(object sender, EventArgs e)
    {
        EditSupplier(sender, e);
    }

    /// auto generated
    protected void actCreateInvoice(object sender, EventArgs e)
    {
        // TODO action actCreateInvoice
    }

    /// auto generated
    protected void actCreateCreditNote(object sender, EventArgs e)
    {
        // TODO action actCreateCreditNote
    }

    /// auto generated
    protected void actFindInvoice(object sender, EventArgs e)
    {
        // TODO action actFindInvoice
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
