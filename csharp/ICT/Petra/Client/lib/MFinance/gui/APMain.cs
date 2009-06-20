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
      this.rbtPartnerKey.Text = Catalog.GetString("&Partner Key");
      this.rbtName.Text = Catalog.GetString("N&ame");
      this.rgrPartnerKeyOrName.Text = Catalog.GetString("PartnerKeyOrName");
      this.chkDueToday.Text = Catalog.GetString("Due &Today");
      this.chkOverdue.Text = Catalog.GetString("&Overdue");
      this.chkDueFuture.Text = Catalog.GetString("Due &Within Future");
      this.btnSearch.Text = Catalog.GetString("&Search");
      this.btnReset.Text = Catalog.GetString("&Reset Criteria");
      this.btnTagAllApprovable.Text = Catalog.GetString("Tag all Appro&vable");
      this.btnTagAllPostable.Text = Catalog.GetString("Tag a&ll Postable");
      this.btnTagAllPayable.Text = Catalog.GetString("Tag all Paya&ble");
      this.btnUntagAll.Text = Catalog.GetString("&Untag all");
      this.lblSumTagged.Text = Catalog.GetString("Sum of Tagged:");
      this.tpgOutstandingInvoices.Text = Catalog.GetString("OutstandingInvoices");
      this.chkShowOutstandingAmounts.Text = Catalog.GetString("Show Outstanding &Amounts");
      this.chkHideInactiveSuppliers.Text = Catalog.GetString("Hide &Inactive Suppliers");
      this.lblSupplierCurrency.Text = Catalog.GetString("C&urrency:");
      this.tpgSuppliers.Text = Catalog.GetString("Suppliers");
      this.tbbTransactions.Text = Catalog.GetString("Open Transactions");
      this.tbbEditSupplier.Text = Catalog.GetString("Edit Supplier");
      this.tbbNewSupplier.Text = Catalog.GetString("New Supplier");
      this.tbbCreateInvoice.Text = Catalog.GetString("Create Invoice");
      this.tbbCreateCreditNote.Text = Catalog.GetString("Create Credit Note");
      this.mniTodo1.Text = Catalog.GetString("todo");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniTodo2.Text = Catalog.GetString("todo");
      this.mniSupplier.Text = Catalog.GetString("Supplier");
      this.mniTodo3.Text = Catalog.GetString("todo");
      this.mniFind.Text = Catalog.GetString("Find");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Accounts Payable");
      #endregion

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);

      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    void chkDueFutureCheckedChanged(object sender, System.EventArgs e)
    {
      nudNumberTimeUnits.Enabled = chkDueFuture.Checked;
      cmbTimeUnit.Enabled = chkDueFuture.Checked;
    }

    private void mniCloseClick(object sender, EventArgs e)
    {
        actClose(sender, e);
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
    public Ict.Petra.Client.CommonForms.TFrmPetraUtils GetUtilObject()
    {
        return (Ict.Petra.Client.CommonForms.TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        mniTodo1.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniTodo2.Enabled = false;
        mniTodo3.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniSeparator0.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator1.Enabled = false;
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
