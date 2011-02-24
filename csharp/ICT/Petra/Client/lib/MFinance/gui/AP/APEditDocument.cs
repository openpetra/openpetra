// auto generated with nant generateWinforms from APEditDocument.yaml and template windowEditWebConnectorMasterDetail
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.AP.Data;

namespace Ict.Petra.Client.MFinance.Gui.AP
{

  /// auto generated: AP Document Edit
  public partial class TFrmAPEditDocument: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS FMainDS;

    /// constructor
    public TFrmAPEditDocument(IntPtr AParentFormHandle) : base()
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
      this.btnAddDetail.Text = Catalog.GetString("Add De&tail");
      this.btnRemoveDetail.Text = Catalog.GetString("&Remove Detail");
      this.btnAnalysisAttributes.Text = Catalog.GetString("Analysis Attri&b.");
      this.lblDetailNarrative.Text = Catalog.GetString("Narrati&ve:");
      this.lblDetailItemRef.Text = Catalog.GetString("Detail &Ref:");
      this.lblDetailAmount.Text = Catalog.GetString("A&mount:");
      this.lblDetailCostCentreCode.Text = Catalog.GetString("C&ost Centre:");
      this.btnUseTaxAccountCostCentre.Text = Catalog.GetString("Use Ta&x Acct+CC");
      this.lblDetailBaseAmount.Text = Catalog.GetString("Base:");
      this.lblDetailAccountCode.Text = Catalog.GetString("Accou&nt:");
      this.grpDetails.Text = Catalog.GetString("Details");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbPostDocument.Text = Catalog.GetString("Post Document");
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

      this.txtSupplierName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtSupplierCurrency.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDocumentCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtReference.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDiscountPercentage.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtTotalAmount.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtExchangeRateToBase.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailNarrative.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailItemRef.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailAmount.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailBaseAmount.Font = TAppSettingsManager.GetDefaultBoldFont();

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
      FPetraUtilsObject.SetStatusBarText(txtDetailNarrative, Catalog.GetString("A narrative about what this is."));
      FPetraUtilsObject.SetStatusBarText(txtDetailItemRef, Catalog.GetString("Some other reference to the item."));
      FPetraUtilsObject.SetStatusBarText(txtDetailAmount, Catalog.GetString("The amount of money this detail is worth."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCostCentreCode, Catalog.GetString("Reference to the cost centre to use for this detail."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountCode, Catalog.GetString("Reference to the account to use for this detail"));
      FMainDS = new Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS();
      grdDetails.Columns.Clear();
      grdDetails.AddCurrencyColumn("Amount", FMainDS.AApDocumentDetail.ColumnAmount);
      grdDetails.AddTextColumn("Narrative", FMainDS.AApDocumentDetail.ColumnNarrative);
      grdDetails.AddTextColumn("Reference", FMainDS.AApDocumentDetail.ColumnItemRef);
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
    public bool CreateAApDocument(Int32 ALedgerNumber, Int64 APartnerKey, bool ACreditNoteOrInvoice)
    {
        FMainDS = TRemote.MFinance.AP.WebConnectors.CreateAApDocument(ALedgerNumber, APartnerKey, ACreditNoteOrInvoice);

        FPetraUtilsObject.SetChangedFlag();

        ShowData(FMainDS.AApDocument[0]);

        return true;
    }

    /// automatically generated, create a new record of AApDocumentDetail and display on the edit screen
    public bool CreateAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, string AApSupplier_DefaultExpAccount, string AApSupplier_DefaultCostCentre, decimal AAmount, Int32 ALastDetailNumber)
    {
        FMainDS.Merge(TRemote.MFinance.AP.WebConnectors.CreateAApDocumentDetail(ALedgerNumber, AApNumber, AApSupplier_DefaultExpAccount, AApSupplier_DefaultCostCentre, AAmount, ALastDetailNumber));
        FMainDS.InitVars();
        FMainDS.AApDocumentDetail.InitVars();

        FPetraUtilsObject.SetChangedFlag();

        DataView myDataView = FMainDS.AApDocumentDetail.DefaultView;
        myDataView.AllowNew = false;
        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AApDocumentDetail.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AApDocumentDetail.PrimaryKey)
            {
                string value1 = FMainDS.AApDocumentDetail.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
            }
        }

        grdDetails.SelectRowInGrid(RowNumberGrid);
    }

    /// return the selected row
    private AApDocumentDetailRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (AApDocumentDetailRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    /// automatically generated function from webconnector
    public bool LoadAApDocument(Int32 ALedgerNumber, Int32 AAPNumber)
    {
        FMainDS.Merge(TRemote.MFinance.AP.WebConnectors.LoadAApDocument(ALedgerNumber, AAPNumber));

        ShowData(FMainDS.AApDocument[0]);

        return true;
    }

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
    }

    private void ShowData(AccountsPayableTDSAApDocumentRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        TPartnerClass partnerClass;
        string partnerShortName;
        TRemote.MPartner.Partner.ServerLookups.GetPartnerShortName(
            ARow.PartnerKey,
            out partnerShortName,
            out partnerClass);
        txtSupplierName.Text = partnerShortName;
        if (FMainDS.AApSupplier != null)
        {
            txtSupplierCurrency.Text = FMainDS.AApSupplier[0].CurrencyCode;
        }
        if (ARow.IsDocumentCodeNull())
        {
            txtDocumentCode.Text = String.Empty;
        }
        else
        {
            txtDocumentCode.Text = ARow.DocumentCode;
        }
        cmbDocumentType.SelectedIndex = (ARow.CreditNoteFlag?1:0);
        if (ARow.IsReferenceNull())
        {
            txtReference.Text = String.Empty;
        }
        else
        {
            txtReference.Text = ARow.Reference;
        }
        dtpDateIssued.Date = ARow.DateIssued;
        dtpDateDue.Date = ARow.DateDue;
        if (ARow.IsCreditTermsNull())
        {
            nudCreditTerms.Value = 0;
        }
        else
        {
            nudCreditTerms.Value = ARow.CreditTerms;
        }
        if (ARow.IsDiscountDaysNull())
        {
            nudDiscountDays.Value = 0;
        }
        else
        {
            nudDiscountDays.Value = ARow.DiscountDays;
        }
        if (ARow.IsDiscountPercentageNull())
        {
            txtDiscountPercentage.Text = String.Empty;
        }
        else
        {
            txtDiscountPercentage.Text = ARow.DiscountPercentage.ToString();
        }
        txtTotalAmount.Text = ARow.TotalAmount.ToString();
        if (ARow.IsExchangeRateToBaseNull())
        {
            txtExchangeRateToBase.Text = String.Empty;
        }
        else
        {
            txtExchangeRateToBase.Text = ARow.ExchangeRateToBase.ToString();
        }
        pnlDetails.Enabled = false;
        if (FMainDS.AApDocumentDetail != null)
        {
            DataView myDataView = FMainDS.AApDocumentDetail.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (FMainDS.AApDocumentDetail.Rows.Count > 0)
            {
                grdDetails.SelectRowInGrid(1);
                pnlDetails.Enabled = true;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(AApDocumentDetailRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        BeforeShowDetailsManual(ARow);
        if (ARow.IsNarrativeNull())
        {
            txtDetailNarrative.Text = String.Empty;
        }
        else
        {
            txtDetailNarrative.Text = ARow.Narrative;
        }
        if (ARow.IsItemRefNull())
        {
            txtDetailItemRef.Text = String.Empty;
        }
        else
        {
            txtDetailItemRef.Text = ARow.ItemRef;
        }
        if (ARow.IsAmountNull())
        {
            txtDetailAmount.Text = String.Empty;
        }
        else
        {
            txtDetailAmount.Text = ARow.Amount.ToString();
        }
        if (ARow.IsCostCentreCodeNull())
        {
            cmbDetailCostCentreCode.SelectedIndex = -1;
        }
        else
        {
            cmbDetailCostCentreCode.SetSelectedString(ARow.CostCentreCode);
        }
        if (ARow.IsAccountCodeNull())
        {
            cmbDetailAccountCode.SelectedIndex = -1;
        }
        else
        {
            cmbDetailAccountCode.SetSelectedString(ARow.AccountCode);
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private AApDocumentDetailRow FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
        // display the details of the currently selected row
        FPreviouslySelectedDetailRow = GetSelectedDetailRow();
        ShowDetails(FPreviouslySelectedDetailRow);
        pnlDetails.Enabled = true;
    }

    private void GetDataFromControls(AccountsPayableTDSAApDocumentRow ARow)
    {
        if (txtDocumentCode.Text.Length == 0)
        {
            ARow.SetDocumentCodeNull();
        }
        else
        {
            ARow.DocumentCode = txtDocumentCode.Text;
        }
        ARow.CreditNoteFlag = cmbDocumentType.SelectedIndex == 1;
        if (txtReference.Text.Length == 0)
        {
            ARow.SetReferenceNull();
        }
        else
        {
            ARow.Reference = txtReference.Text;
        }
        ARow.DateIssued = dtpDateIssued.Date.Value;
        ARow.DateDue = dtpDateDue.Date.Value;
        ARow.CreditTerms = (Int32)nudCreditTerms.Value;
        ARow.DiscountDays = (Int32)nudDiscountDays.Value;
        if (txtDiscountPercentage.Text.Length == 0)
        {
            ARow.SetDiscountPercentageNull();
        }
        else
        {
            ARow.DiscountPercentage = Convert.ToDecimal(txtDiscountPercentage.Text);
        }
        ARow.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
        if (txtExchangeRateToBase.Text.Length == 0)
        {
            ARow.SetExchangeRateToBaseNull();
        }
        else
        {
            ARow.ExchangeRateToBase = Convert.ToDecimal(txtExchangeRateToBase.Text);
        }
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
    }

    private void GetDetailsFromControls(AApDocumentDetailRow ARow)
    {
        if (ARow != null)
        {
            if (txtDetailNarrative.Text.Length == 0)
            {
                ARow.SetNarrativeNull();
            }
            else
            {
                ARow.Narrative = txtDetailNarrative.Text;
            }
            if (txtDetailItemRef.Text.Length == 0)
            {
                ARow.SetItemRefNull();
            }
            else
            {
                ARow.ItemRef = txtDetailItemRef.Text;
            }
            if (txtDetailAmount.Text.Length == 0)
            {
                ARow.SetAmountNull();
            }
            else
            {
                ARow.Amount = Convert.ToDecimal(txtDetailAmount.Text);
            }
            if (cmbDetailCostCentreCode.SelectedIndex == -1)
            {
                ARow.SetCostCentreCodeNull();
            }
            else
            {
                ARow.CostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
            }
            if (cmbDetailAccountCode.SelectedIndex == -1)
            {
                ARow.SetAccountCodeNull();
            }
            else
            {
                ARow.AccountCode = cmbDetailAccountCode.GetSelectedString();
            }
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

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls(FMainDS.AApDocument[0]);

        // TODO: verification

        if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
        {
            foreach (DataTable InspectDT in FMainDS.Tables)
            {
                foreach (DataRow InspectDR in InspectDT.Rows)
                {
                    InspectDR.EndEdit();
                }
            }

            if (!FPetraUtilsObject.HasChanges)
            {
                return true;
            }
            else
            {
                FPetraUtilsObject.WriteToStatusBar("Saving data...");
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Petra.Shared.MFinance.AP.Data.AccountsPayableTDS SubmitDS = FMainDS.GetChangesTyped(true);

                if (SubmitDS == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(Catalog.GetString("There is nothing to be saved."));
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TRemote.MFinance.AP.WebConnectors.SaveAApDocument(ref SubmitDS, out VerificationResult);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                        "No Server response",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
/* TODO ESecurityDBTableAccessDeniedException
*                  catch (ESecurityDBTableAccessDeniedException Exp)
*                  {
*                      FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
*                      this.Cursor = Cursors.Default;
*                      // TODO TMessages.MsgSecurityException(Exp, this.GetType());
*                      bool ReturnValue = false;
*                      // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
*                      return ReturnValue;
*                  }
*/
                catch (EDBConcurrencyException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;

                    // TODO TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception exp)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    TLogging.Log(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                        TLoggingType.ToLogfile);
                    MessageBox.Show(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                        "For details see the log file: " + TLogging.GetLogFileName(),
                        "Server connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return false;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:

                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.Merge(SubmitDS, false);

                        // need to accept the new modification ID
                        FMainDS.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                        this.Cursor = Cursors.Default;

                        // TODO EnableSave(false);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return true;

                    case TSubmitChangesResult.scrError:

                        // TODO scrError
                        this.Cursor = Cursors.Default;
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        // TODO scrNothingToBeSaved
                        this.Cursor = Cursors.Default;
                        return true;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
        }

        return false;
    }

#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actNewDetail")
        {
            btnAddDetail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
            mniFileSave.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostDocument")
        {
            tbbPostDocument.Enabled = e.Enabled;
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
