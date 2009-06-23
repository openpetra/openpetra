/* auto generated with nant generateWinforms from APSupplierTransactions.yaml
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

  /// auto generated: Supplier Transactions
  public partial class TFrmAccountsPayableSupplierTransactions: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmAccountsPayableSupplierTransactions(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblCurrentSupplierName.Text = Catalog.GetString("Current Supplier:");
      this.lblCurrentSupplierCurrency.Text = Catalog.GetString("Currency:");
      this.lblType.Text = Catalog.GetString("&Type:");
      this.lblDate.Text = Catalog.GetString("&Date:");
      this.lblDateField.Text = Catalog.GetString("Date &Field:");
      this.lblStatus.Text = Catalog.GetString("&Status:");
      this.chkHideAgedTransactions.Text = Catalog.GetString("Hide &Aged Transactions");
      this.btnTagApprovable.Text = Catalog.GetString("Tag all Appro&vable");
      this.btnTagPostable.Text = Catalog.GetString("Tag all P&ostable");
      this.btnTagPayable.Text = Catalog.GetString("Tag all Paya&ble");
      this.btnUntagAll.Text = Catalog.GetString("&Untag All");
      this.lblSumOfTagged.Text = Catalog.GetString("Sum of Tagged:");
      this.lblDisplayedBalance.Text = Catalog.GetString("Displayed Balance:");
      this.tbbNewInvoice.Text = Catalog.GetString("&Invoice");
      this.tbbNewCreditNote.Text = Catalog.GetString("&Credit Note");
      this.tbbOpenSelected.Text = Catalog.GetString("&Open Selected");
      this.tbbReverseSelected.Text = Catalog.GetString("Re&verse Selected");
      this.tbbApproveTagged.Text = Catalog.GetString("&Approve Tagged");
      this.tbbPostTagged.Text = Catalog.GetString("&Post Tagged");
      this.tbbAddTaggedToPayment.Text = Catalog.GetString("Add Tagged to Pa&yment");
      this.mniReprintRemittanceAdvice.Text = Catalog.GetString("Reprint Re&mittance Advice");
      this.mniReprintCheque.Text = Catalog.GetString("Reprint &Cheque");
      this.mniReprintPaymentReport.Text = Catalog.GetString("Reprint Pa&yment Report");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniNewInvoice.Text = Catalog.GetString("&Invoice");
      this.mniNewCreditNote.Text = Catalog.GetString("&Credit Note");
      this.mniActionNew.Text = Catalog.GetString("&New...");
      this.mniOpenSelected.Text = Catalog.GetString("&Open Selected");
      this.mniReverseTransaction.Text = Catalog.GetString("Re&verse Selected");
      this.mniApproveTagged.Text = Catalog.GetString("&Approve Tagged");
      this.mniPostTagged.Text = Catalog.GetString("&Post Tagged");
      this.mniAddTaggedToPayment.Text = Catalog.GetString("Add Tagged to Pa&yment");
      this.mniAction.Text = Catalog.GetString("&Action");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Supplier Transactions");
      #endregion

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);

      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    private void tbbNewInvoiceClick(object sender, EventArgs e)
    {
        actNewInvoice(sender, e);
    }

    private void tbbNewCreditNoteClick(object sender, EventArgs e)
    {
        actNewCreditNote(sender, e);
    }

    private void tbbOpenSelectedClick(object sender, EventArgs e)
    {
        actOpenSelected(sender, e);
    }

    private void tbbReverseSelectedClick(object sender, EventArgs e)
    {
        actReverseSelected(sender, e);
    }

    private void tbbApproveTaggedClick(object sender, EventArgs e)
    {
        actApproveTagged(sender, e);
    }

    private void tbbPostTaggedClick(object sender, EventArgs e)
    {
        actPostTagged(sender, e);
    }

    private void tbbAddTaggedToPaymentClick(object sender, EventArgs e)
    {
        actAddTaggedToPayment(sender, e);
    }

    private void mniCloseClick(object sender, EventArgs e)
    {
        actClose(sender, e);
    }

    private void mniNewInvoiceClick(object sender, EventArgs e)
    {
        actNewInvoice(sender, e);
    }

    private void mniNewCreditNoteClick(object sender, EventArgs e)
    {
        actNewCreditNote(sender, e);
    }

    private void mniOpenSelectedClick(object sender, EventArgs e)
    {
        actOpenSelected(sender, e);
    }

    private void mniReverseTransactionClick(object sender, EventArgs e)
    {
        actReverseSelected(sender, e);
    }

    private void mniApproveTaggedClick(object sender, EventArgs e)
    {
        actApproveTagged(sender, e);
    }

    private void mniPostTaggedClick(object sender, EventArgs e)
    {
        actPostTagged(sender, e);
    }

    private void mniAddTaggedToPaymentClick(object sender, EventArgs e)
    {
        actAddTaggedToPayment(sender, e);
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
        if (e.ActionName == "actNewInvoice")
        {
            tbbNewInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewCreditNote")
        {
            tbbNewCreditNote.Enabled = e.Enabled;
        }
        if (e.ActionName == "actOpenSelected")
        {
            tbbOpenSelected.Enabled = e.Enabled;
        }
        if (e.ActionName == "actReverseSelected")
        {
            tbbReverseSelected.Enabled = e.Enabled;
        }
        if (e.ActionName == "actApproveTagged")
        {
            tbbApproveTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostTagged")
        {
            tbbPostTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actAddTaggedToPayment")
        {
            tbbAddTaggedToPayment.Enabled = e.Enabled;
        }
        mniReprintRemittanceAdvice.Enabled = false;
        mniReprintCheque.Enabled = false;
        mniReprintPaymentReport.Enabled = false;
        mniSeparator0.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewInvoice")
        {
            mniNewInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewCreditNote")
        {
            mniNewCreditNote.Enabled = e.Enabled;
        }
        if (e.ActionName == "actOpenSelected")
        {
            mniOpenSelected.Enabled = e.Enabled;
        }
        mniSeparator1.Enabled = false;
        if (e.ActionName == "actReverseSelected")
        {
            mniReverseTransaction.Enabled = e.Enabled;
        }
        if (e.ActionName == "actApproveTagged")
        {
            mniApproveTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostTagged")
        {
            mniPostTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actAddTaggedToPayment")
        {
            mniAddTaggedToPayment.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniSeparator2.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator3.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actNewInvoice(object sender, EventArgs e)
    {
        // TODO action actNewInvoice
    }

    /// auto generated
    protected void actNewCreditNote(object sender, EventArgs e)
    {
        // TODO action actNewCreditNote
    }

    /// auto generated
    protected void actOpenSelected(object sender, EventArgs e)
    {
        // TODO action actOpenSelected
    }

    /// auto generated
    protected void actReverseSelected(object sender, EventArgs e)
    {
        // TODO action actReverseSelected
    }

    /// auto generated
    protected void actApproveTagged(object sender, EventArgs e)
    {
        // TODO action actApproveTagged
    }

    /// auto generated
    protected void actPostTagged(object sender, EventArgs e)
    {
        // TODO action actPostTagged
    }

    /// auto generated
    protected void actAddTaggedToPayment(object sender, EventArgs e)
    {
        // TODO action actAddTaggedToPayment
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
