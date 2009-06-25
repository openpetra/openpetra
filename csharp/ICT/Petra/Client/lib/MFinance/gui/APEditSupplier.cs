/* auto generated with nant generateWinforms from APEditSupplier.yaml
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

  /// auto generated: AP Supplier Edit
  public partial class TFrmAccountsPayableEditSupplier: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    /// constructor
    public TFrmAccountsPayableEditSupplier(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblPartnerKey.Text = Catalog.GetString("Partner Key:");
      this.btnEditPartner.Text = Catalog.GetString("&Edit Partner info of Supplier");
      this.lblPartnerName.Text = Catalog.GetString("Partner Name:");
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

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);

      InitializeManualCode();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
    }

    private void tbbSaveClick(object sender, EventArgs e)
    {
        actSave(sender, e);
    }

    private void mniFileSaveClick(object sender, EventArgs e)
    {
        actSave(sender, e);
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
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSave")
        {
            mniFileSave.Enabled = e.Enabled;
        }
        mniSeparator0.Enabled = false;
        mniFilePrint.Enabled = false;
        mniSeparator1.Enabled = false;
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniSeparator2.Enabled = false;
        mniEditFind.Enabled = false;
        mniHelpPetraHelp.Enabled = false;
        mniSeparator3.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniSeparator4.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actSave(object sender, EventArgs e)
    {
        FileSave(sender, e);
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
