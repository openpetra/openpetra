// auto generated with nant generateWinforms from APEditSupplier.yaml and template windowEditUIConnector
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
using Mono.Unix;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AP
{

  /// auto generated: AP Supplier Edit
  public partial class TFrmAPEditSupplier: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors.IAPUIConnectorsSupplierEdit FUIConnector = null;

    /// constructor
    public TFrmAPEditSupplier(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblPartnerKey.Text = Catalog.GetString("Partner Key:");
      this.btnEditPartner.Text = Catalog.GetString("&Edit Partner info of Supplier");
      this.lblCurrency.Text = Catalog.GetString("&Currency:");
      this.lblSupplierType.Text = Catalog.GetString("Supplier &Type:");
      this.grpGeneralInformation.Text = Catalog.GetString("General Information");
      this.lblInvoiceAging.Text = Catalog.GetString("Invoice A&ging (in months):");
      this.lblCreditTerms.Text = Catalog.GetString("C&redit Terms (in days):");
      this.lblDefaultPaymentType.Text = Catalog.GetString("Default &Payment Type:");
      this.lblDiscountDays.Text = Catalog.GetString("Number of Days for &Discount (0 for none):");
      this.lblDiscountValue.Text = Catalog.GetString("Discount &Value (%):");
      this.grpMiscDefaults.Text = Catalog.GetString("Misc Defaults");
      this.lblAPAccount.Text = Catalog.GetString("&AP Account:");
      this.lblDefaultBankAccount.Text = Catalog.GetString("Default &Bank Account:");
      this.lblCostCentre.Text = Catalog.GetString("Default C&ost Centre:");
      this.lblExpenseAccount.Text = Catalog.GetString("Default &Expense Account:");
      this.grpAccountInformation.Text = Catalog.GetString("Account Information");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.mniFileSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.mniFileSave.Text = Catalog.GetString("&Save");
      this.mniFilePrint.Text = Catalog.GetString("&Print...");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditUndoCurrentField.Text = Catalog.GetString("Undo &Current Field");
      this.mniEditUndoScreen.Text = Catalog.GetString("&Undo Screen");
      this.mniEditFind.Text = Catalog.GetString("&Find...");
      this.mniEdit.Text = Catalog.GetString("&Edit");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("AP Supplier Edit");
      #endregion

      this.txtDiscountValue.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(txtPartnerKey, Catalog.GetString("Reference to the partner key for this supplier"));
      FPetraUtilsObject.SetStatusBarText(cmbCurrency, Catalog.GetString("The currency code to use for this supplier."));
      cmbCurrency.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(cmbSupplierType, Catalog.GetString("What type of supplier this is - normal, credit card, maybe something else."));
      FPetraUtilsObject.SetStatusBarText(nudInvoiceAging, Catalog.GetString("Number of months to display invoices and credit notes"));
      FPetraUtilsObject.SetStatusBarText(nudCreditTerms, Catalog.GetString("Default credit terms to use for invoices from this supplier."));
      FPetraUtilsObject.SetStatusBarText(cmbDefaultPaymentType, Catalog.GetString("The default type of payment to use when paying this supplier."));
      FPetraUtilsObject.SetStatusBarText(nudDiscountDays, Catalog.GetString("Default number of days in which the discount percentage has effect."));
      FPetraUtilsObject.SetStatusBarText(txtDiscountValue, Catalog.GetString("Default percentage discount to receive for early payments."));
      FPetraUtilsObject.SetStatusBarText(cmbAPAccount, Catalog.GetString("The default AP Account to use when paying this supplier."));
      FPetraUtilsObject.SetStatusBarText(cmbDefaultBankAccount, Catalog.GetString("Reference to default bank account to use to pay supplier with."));
      FPetraUtilsObject.SetStatusBarText(cmbCostCentre, Catalog.GetString("Reference to the default cost centre to use for invoice details."));
      FPetraUtilsObject.SetStatusBarText(cmbExpenseAccount, Catalog.GetString("Reference to the default expense Account to use for invoice details."));
      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      ActionEnabledEvent(null, new ActionEventArgs("cndDiscountEnabled", false));

      FUIConnector = TRemote.MFinance.AP.UIConnectors.SupplierEdit();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
    }

    private void nudDiscountDaysValueChanged(object sender, EventArgs e)
    {
        ActionEnabledEvent(null, new ActionEventArgs("cndDiscountEnabled", nudDiscountDays.Value > 0));
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

        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
        }
    }

    private void ShowData(AApSupplierRow ARow)
    {
        txtPartnerKey.Text = String.Format("{0:0000000000}", ARow.PartnerKey);
        txtPartnerKey.ReadOnly = (ARow.RowState != DataRowState.Added);
        cmbCurrency.SetSelectedString(ARow.CurrencyCode);
        if (ARow.IsSupplierTypeNull())
        {
            cmbSupplierType.SelectedIndex = -1;
        }
        else
        {
            cmbSupplierType.SetSelectedString(ARow.SupplierType);
        }
        if (ARow.IsPreferredScreenDisplayNull())
        {
            nudInvoiceAging.Value = 0;
        }
        else
        {
            nudInvoiceAging.Value = ARow.PreferredScreenDisplay;
        }
        if (ARow.IsDefaultCreditTermsNull())
        {
            nudCreditTerms.Value = 0;
        }
        else
        {
            nudCreditTerms.Value = ARow.DefaultCreditTerms;
        }
        if (ARow.IsPaymentTypeNull())
        {
            cmbDefaultPaymentType.SelectedIndex = -1;
        }
        else
        {
            cmbDefaultPaymentType.SetSelectedString(ARow.PaymentType);
        }
        if (ARow.IsDefaultDiscountDaysNull())
        {
            nudDiscountDays.Value = 0;
        }
        else
        {
            nudDiscountDays.Value = ARow.DefaultDiscountDays;
        }
        if (ARow.IsDefaultDiscountPercentageNull())
        {
            txtDiscountValue.Text = String.Empty;
        }
        else
        {
            txtDiscountValue.Text = ARow.DefaultDiscountPercentage.ToString();
        }
        if (ARow.IsDefaultApAccountNull())
        {
            cmbAPAccount.SelectedIndex = -1;
        }
        else
        {
            cmbAPAccount.SetSelectedString(ARow.DefaultApAccount);
        }
        if (ARow.IsDefaultBankAccountNull())
        {
            cmbDefaultBankAccount.SelectedIndex = -1;
        }
        else
        {
            cmbDefaultBankAccount.SetSelectedString(ARow.DefaultBankAccount);
        }
        if (ARow.IsDefaultCostCentreNull())
        {
            cmbCostCentre.SelectedIndex = -1;
        }
        else
        {
            cmbCostCentre.SetSelectedString(ARow.DefaultCostCentre);
        }
        if (ARow.IsDefaultExpAccountNull())
        {
            cmbExpenseAccount.SelectedIndex = -1;
        }
        else
        {
            cmbExpenseAccount.SetSelectedString(ARow.DefaultExpAccount);
        }
    }

    private void GetDataFromControls(AApSupplierRow ARow)
    {
        ARow.CurrencyCode = cmbCurrency.GetSelectedString();
        if (cmbSupplierType.SelectedIndex == -1)
        {
            ARow.SetSupplierTypeNull();
        }
        else
        {
            ARow.SupplierType = cmbSupplierType.GetSelectedString();
        }
        ARow.PreferredScreenDisplay = (Int32)nudInvoiceAging.Value;
        ARow.DefaultCreditTerms = (Int32)nudCreditTerms.Value;
        if (cmbDefaultPaymentType.SelectedIndex == -1)
        {
            ARow.SetPaymentTypeNull();
        }
        else
        {
            ARow.PaymentType = cmbDefaultPaymentType.GetSelectedString();
        }
        ARow.DefaultDiscountDays = (Int32)nudDiscountDays.Value;
        if (txtDiscountValue.Text.Length == 0)
        {
            ARow.SetDefaultDiscountPercentageNull();
        }
        else
        {
            ARow.DefaultDiscountPercentage = Convert.ToDouble(txtDiscountValue.Text);
        }
        if (cmbAPAccount.SelectedIndex == -1)
        {
            ARow.SetDefaultApAccountNull();
        }
        else
        {
            ARow.DefaultApAccount = cmbAPAccount.GetSelectedString();
        }
        if (cmbDefaultBankAccount.SelectedIndex == -1)
        {
            ARow.SetDefaultBankAccountNull();
        }
        else
        {
            ARow.DefaultBankAccount = cmbDefaultBankAccount.GetSelectedString();
        }
        if (cmbCostCentre.SelectedIndex == -1)
        {
            ARow.SetDefaultCostCentreNull();
        }
        else
        {
            ARow.DefaultCostCentre = cmbCostCentre.GetSelectedString();
        }
        if (cmbExpenseAccount.SelectedIndex == -1)
        {
            ARow.SetDefaultExpAccountNull();
        }
        else
        {
            ARow.DefaultExpAccount = cmbExpenseAccount.GetSelectedString();
        }
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

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
        SaveChanges();
    }

#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
            mniFileSave.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        if (e.ActionName == "cndDiscountEnabled")
        {
            txtDiscountValue.Enabled = e.Enabled;
        }
        mniFilePrint.Enabled = false;
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniEditFind.Enabled = false;
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
