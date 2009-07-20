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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.AccountsPayable
{

  /// auto generated: AP Document Edit
  public partial class TFrmAccountsPayableEditDocument: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS FMainDS;

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
      this.lblDocumentCode.Text = Catalog.GetString("Invoice &Number:");
      this.cmbDocumentType.Text = Catalog.GetString("Invoice");
      this.lblDocumentType.Text = Catalog.GetString("T&ype:");
      this.lblReference.Text = Catalog.GetString("&Reference:");
      this.lblDateIssued.Text = Catalog.GetString("&Date Issued:");
      this.lblDateDue.Text = Catalog.GetString("Date D&ue:");
      this.lblCreditTerms.Text = Catalog.GetString("Credit &Terms:");
      this.lblDiscountDays.Text = Catalog.GetString("Discount &Days:");
      this.lblDiscountPercentage.Text = Catalog.GetString("Discount &Value (%):");
      this.lblTotalAmount.Text = Catalog.GetString("&Amount:");
      this.lblExchangeRateToBase.Text = Catalog.GetString("E&xchange Rate:");
      this.grpDocumentInfo.Text = Catalog.GetString("Document Information");
      this.lblDetailNarrative.Text = Catalog.GetString("Narrati&ve:");
      this.btnAddDetail.Text = Catalog.GetString("Add De&tail");
      this.lblDetailItemRef.Text = Catalog.GetString("Detail &Ref:");
      this.btnRemoveDetail.Text = Catalog.GetString("&Remove Detail");
      this.lblDetailAmount.Text = Catalog.GetString("A&mount:");
      this.lblDetailCostCentreCode.Text = Catalog.GetString("C&ost Centre:");
      this.btnAnalysisAttributes.Text = Catalog.GetString("Analysis Attri&b.");
      this.lblDetailBaseAmount.Text = Catalog.GetString("Base:");
      this.lblDetailAccountCode.Text = Catalog.GetString("Accou&nt:");
      this.btnApproveDetail.Text = Catalog.GetString("A&pprove Detail");
      this.lblDetailApprovalDate.Text = Catalog.GetString("Approved On:");
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
      FPetraUtilsObject.SetStatusBarText(txtSupplierCurrency, Catalog.GetString("The currency code to use for this supplier."));
      FPetraUtilsObject.SetStatusBarText(txtDocumentCode, Catalog.GetString("The code given on the document itself (be it invoice or credit note). This will have to be unique for each supplier."));
      FPetraUtilsObject.SetStatusBarText(cmbDocumentType, Catalog.GetString("A flag to indicate if this document is an invoice or a credit note."));
      FPetraUtilsObject.SetStatusBarText(txtReference, Catalog.GetString("Some kind of other reference needed."));
      FPetraUtilsObject.SetStatusBarText(dtpDateIssued, Catalog.GetString("The date when this document was issued."));
      FPetraUtilsObject.SetStatusBarText(dtpDateDue, Catalog.GetString("Credit Terms is the number of days between date issued and due date"));
      FPetraUtilsObject.SetStatusBarText(nudCreditTerms, Catalog.GetString("Credit terms allowed for this invoice."));
      FPetraUtilsObject.SetStatusBarText(nudDiscountDays, Catalog.GetString("The number of days that the discount is valid for (0 for none)."));
      FPetraUtilsObject.SetStatusBarText(txtDiscountPercentage, Catalog.GetString("The percentage discount you get for early payment of this document in the case that it is an invoice."));
      FPetraUtilsObject.SetStatusBarText(txtTotalAmount, Catalog.GetString("The total amount of money that this document is worth."));
      FPetraUtilsObject.SetStatusBarText(txtExchangeRateToBase, Catalog.GetString("The exchange rate to the base currency at the time that the document was issued."));
      FMainDS = new Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS();
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Amount", FMainDS.AApDocumentDetail.ColumnAmount);
      grdDetails.AddTextColumn("Reference", FMainDS.AApDocumentDetail.ColumnItemRef);
      grdDetails.AddTextColumn("Narrative", FMainDS.AApDocumentDetail.ColumnNarrative);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      ActionEnabledEvent(null, new ActionEventArgs("cndDiscountEnabled", false));

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

    }

    /// automatically generated function from webconnector
    public bool CreateNewAApDocument(Int32 ALedgerNumber, Int64 APartnerKey, bool ACreditNoteOrInvoice)
    {
        FMainDS = TRemote.MFinance.AccountsPayable.WebConnectors.CreateNewAApDocument(ALedgerNumber, APartnerKey, ACreditNoteOrInvoice);

        FPetraUtilsObject.SetChangedFlag();

        ShowData();

        return true;
    }

    /// automatically generated, create a new record of AApDocumentDetail and display on the edit screen
    public bool CreateNewAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, string AApSupplier_DefaultExpAccount, string AApSupplier_DefaultCostCentre, double AAmount, Int32 ATempNumberOfDetails)
    {
        FMainDS.Merge(TRemote.MFinance.AccountsPayable.WebConnectors.CreateNewAApDocumentDetail(ALedgerNumber, AApNumber, AApSupplier_DefaultExpAccount, AApSupplier_DefaultCostCentre, AAmount, ATempNumberOfDetails));

        FPetraUtilsObject.SetChangedFlag();

        ShowData();
        SelectDetailRow(grdDetails.Rows.Count - 1);

        return true;
    }
    private void SelectDetailRow(Int32 ARowNumber)
    {
        ShowData();
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(ARowNumber, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(ARowNumber, 0), true);

        // TODO? DataGrid_FocusRowEntered(this, new RowEventArgs(ARowNumber));
    }

    /// automatically generated function from webconnector
    public bool LoadAApDocument(Int32 ALedgerNumber, Int32 AAPNumber)
    {
        FMainDS.Merge(TRemote.MFinance.AccountsPayable.WebConnectors.LoadAApDocument(ALedgerNumber, AAPNumber));

        ShowData();

        return true;
    }

    private void ShowData()
    {
        TPartnerClass partnerClass;
        string partnerShortName;
        TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
            FMainDS.AApDocument[0].PartnerKey,
            out partnerShortName,
            out partnerClass);
        txtSupplierName.Text = partnerShortName;
        txtSupplierCurrency.Text = FMainDS.AApSupplier[0].CurrencyCode;
        if (FMainDS.AApDocument[0].IsDocumentCodeNull())
        {
            txtDocumentCode.Text = String.Empty;
        }
        else
        {
            txtDocumentCode.Text = FMainDS.AApDocument[0].DocumentCode;
        }
        cmbDocumentType.SelectedIndex = (FMainDS.AApDocument[0].CreditNoteFlag?1:0);
        if (FMainDS.AApDocument[0].IsReferenceNull())
        {
            txtReference.Text = String.Empty;
        }
        else
        {
            txtReference.Text = FMainDS.AApDocument[0].Reference;
        }
        dtpDateIssued.Value = FMainDS.AApDocument[0].DateIssued;
        if (FMainDS.AApDocument[0].IsCreditTermsNull())
        {
            nudCreditTerms.Value = 0;
        }
        else
        {
            nudCreditTerms.Value = FMainDS.AApDocument[0].CreditTerms;
        }
        if (FMainDS.AApDocument[0].IsDiscountDaysNull())
        {
            nudDiscountDays.Value = 0;
        }
        else
        {
            nudDiscountDays.Value = FMainDS.AApDocument[0].DiscountDays;
        }
        if (FMainDS.AApDocument[0].IsDiscountPercentageNull())
        {
            txtDiscountPercentage.Text = String.Empty;
        }
        else
        {
            txtDiscountPercentage.Text = FMainDS.AApDocument[0].DiscountPercentage.ToString();
        }
        if (FMainDS.AApDocument[0].IsTotalAmountNull())
        {
            txtTotalAmount.Text = String.Empty;
        }
        else
        {
            txtTotalAmount.Text = FMainDS.AApDocument[0].TotalAmount.ToString();
        }
        if (FMainDS.AApDocument[0].IsExchangeRateToBaseNull())
        {
            txtExchangeRateToBase.Text = String.Empty;
        }
        else
        {
            txtExchangeRateToBase.Text = FMainDS.AApDocument[0].ExchangeRateToBase.ToString();
        }
        if (FMainDS.AApDocumentDetail != null)
        {
            DataView myDataView = FMainDS.AApDocumentDetail.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdDetails.AutoSizeCells();
        }
    }

    private void GetDataFromControls()
    {
        if (txtDocumentCode.Text.Length == 0)
        {
            FMainDS.AApDocument[0].SetDocumentCodeNull();
        }
        else
        {
            FMainDS.AApDocument[0].DocumentCode = txtDocumentCode.Text;
        }
        FMainDS.AApDocument[0].CreditNoteFlag = cmbDocumentType.SelectedIndex == 1;
        if (txtReference.Text.Length == 0)
        {
            FMainDS.AApDocument[0].SetReferenceNull();
        }
        else
        {
            FMainDS.AApDocument[0].Reference = txtReference.Text;
        }
        FMainDS.AApDocument[0].DateIssued = dtpDateIssued.Value;
        FMainDS.AApDocument[0].CreditTerms = (Int32)nudCreditTerms.Value;
        FMainDS.AApDocument[0].DiscountDays = (Int32)nudDiscountDays.Value;
        if (txtDiscountPercentage.Text.Length == 0)
        {
            FMainDS.AApDocument[0].SetDiscountPercentageNull();
        }
        else
        {
            FMainDS.AApDocument[0].DiscountPercentage = Convert.ToDouble(txtDiscountPercentage.Text);
        }
        if (txtTotalAmount.Text.Length == 0)
        {
            FMainDS.AApDocument[0].SetTotalAmountNull();
        }
        else
        {
            FMainDS.AApDocument[0].TotalAmount = Convert.ToDouble(txtTotalAmount.Text);
        }
        if (txtExchangeRateToBase.Text.Length == 0)
        {
            FMainDS.AApDocument[0].SetExchangeRateToBaseNull();
        }
        else
        {
            FMainDS.AApDocument[0].ExchangeRateToBase = Convert.ToDouble(txtExchangeRateToBase.Text);
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
        SaveChanges(FMainDS);
    }

    /// auto generated
    public bool SaveChanges()
    {
        return SaveChanges(FMainDS);
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
            txtDiscountPercentage.Enabled = e.Enabled;
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
