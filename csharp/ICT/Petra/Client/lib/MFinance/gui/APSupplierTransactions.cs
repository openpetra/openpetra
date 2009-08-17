/* auto generated with nant generateWinforms from APSupplierTransactions.yaml and template windowFind
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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{

  /// auto generated: Supplier Transactions
  public partial class TFrmAPSupplierTransactions: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS FMainDS;

    /// constructor
    public TFrmAPSupplierTransactions(IntPtr AParentFormHandle) : base()
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
      this.tbbNewInvoice.Text = Catalog.GetString("New &Invoice");
      this.tbbNewCreditNote.Text = Catalog.GetString("New &Credit Note");
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
      this.mniNewInvoice.Text = Catalog.GetString("New &Invoice");
      this.mniNewCreditNote.Text = Catalog.GetString("New &Credit Note");
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
      FMainDS = new Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS();
      FPetraUtilsObject.SetStatusBarText(txtCurrentSupplierCurrency, Catalog.GetString("The currency code to use for this supplier."));
      grdResult.Columns.Clear();
      grdResult.AddTextColumn("AP Number", FMainDS.AApDocument.ColumnApNumber);
      grdResult.AddTextColumn("Invoice Number", FMainDS.AApDocument.ColumnDocumentCode);
      grdResult.AddTextColumn("Date Issued", FMainDS.AApDocument.ColumnDateIssued);
      grdResult.AddTextColumn("Total Amount", FMainDS.AApDocument.ColumnTotalAmount);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

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

    private void ShowData()
    {
        TPartnerClass partnerClass;
        string partnerShortName;
        TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
            FMainDS.AApSupplier[0].PartnerKey,
            out partnerShortName,
            out partnerClass);
        txtCurrentSupplierName.Text = partnerShortName;
        txtCurrentSupplierCurrency.Text = FMainDS.AApSupplier[0].CurrencyCode;
        ShowDataManual();
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
        if (e.ActionName == "actNewInvoice")
        {
            tbbNewInvoice.Enabled = e.Enabled;
            mniNewInvoice.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewCreditNote")
        {
            tbbNewCreditNote.Enabled = e.Enabled;
            mniNewCreditNote.Enabled = e.Enabled;
        }
        if (e.ActionName == "actOpenSelected")
        {
            tbbOpenSelected.Enabled = e.Enabled;
            mniOpenSelected.Enabled = e.Enabled;
        }
        if (e.ActionName == "actReverseSelected")
        {
            tbbReverseSelected.Enabled = e.Enabled;
            mniReverseTransaction.Enabled = e.Enabled;
        }
        if (e.ActionName == "actApproveTagged")
        {
            tbbApproveTagged.Enabled = e.Enabled;
            mniApproveTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostTagged")
        {
            tbbPostTagged.Enabled = e.Enabled;
            mniPostTagged.Enabled = e.Enabled;
        }
        if (e.ActionName == "actAddTaggedToPayment")
        {
            tbbAddTaggedToPayment.Enabled = e.Enabled;
            mniAddTaggedToPayment.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniReprintRemittanceAdvice.Enabled = false;
        mniReprintCheque.Enabled = false;
        mniReprintPaymentReport.Enabled = false;
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
