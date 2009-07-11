/* auto generated with nant generateWinforms from APEditDocument.yaml
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

  /// auto generated: AP Document Edit
  public partial class TFrmAccountsPayableEditDocument: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    /// constructor
    public TFrmAccountsPayableEditDocument(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblSupplierName.Text = Catalog.GetString("Current Supplier:");
      this.lblSupplierCurrency.Text = Catalog.GetString("Currency:");
      this.lblInvoiceNumber.Text = Catalog.GetString("Invoice &Number:");
      this.lblDocumentType.Text = Catalog.GetString("T&ype:");
      this.lblReference.Text = Catalog.GetString("&Reference:");
      this.lblDateIssued.Text = Catalog.GetString("&Date Issued:");
      this.lblDateDue.Text = Catalog.GetString("Date D&ue:");
      this.lblCreditTerms.Text = Catalog.GetString("Credit &Terms:");
      this.lblAmount.Text = Catalog.GetString("&Amount:");
      this.lblExchangeRate.Text = Catalog.GetString("E&xchange Rate:");
      this.btnEarlyPaymentDiscount.Text = Catalog.GetString("Early Pyt Discount");
      this.grpDocumentInfo.Text = Catalog.GetString("Document Information");
      this.lblNarrative.Text = Catalog.GetString("Narrati&ve:");
      this.btnAddDetail.Text = Catalog.GetString("Add De&tail");
      this.lblDetailReference.Text = Catalog.GetString("Detail &Ref:");
      this.btnRemoveDetail.Text = Catalog.GetString("&Remove Detail");
      this.lblDetailAmount.Text = Catalog.GetString("A&mount:");
      this.lblCostCentre.Text = Catalog.GetString("C&ost Centre:");
      this.btnAnalysisAttributes.Text = Catalog.GetString("Analysis Attri&b.");
      this.lblBaseAmount.Text = Catalog.GetString("Base:");
      this.lblAccount.Text = Catalog.GetString("Accou&nt:");
      this.btnApproveDetail.Text = Catalog.GetString("A&pprove Detail");
      this.lblDateApproved.Text = Catalog.GetString("Approved On:");
      this.btnUseTaxAccountCostCentre.Text = Catalog.GetString("Use Ta&x Acct+CC");
      this.grpDetails.Text = Catalog.GetString("Details");
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
      this.Text = Catalog.GetString("AP Document Edit");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(nudCreditTerms, Catalog.GetString("Credit terms allowed for this invoice."));
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

    private void ShowData()
    {
        if (FMainDS.AApDocument[0].IsCreditTermsNull())
        {
            nudCreditTerms.Value = 0;
        }
        else
        {
            nudCreditTerms.Value = FMainDS.AApDocument[0].CreditTerms;
        }
        ShowDataManual();
    }

    private void GetDataFromControls()
    {
        FMainDS.AApDocument[0].CreditTerms = (Int32)nudCreditTerms.Value;
        GetDataFromControlsManual();
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
