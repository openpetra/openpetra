// auto generated with nant generateWinforms from UC_GiftTransactions.yaml and template controlMaintainTable
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
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{

  /// auto generated user control
  public partial class TUC_GiftTransactions: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS FMainDS;

    /// constructor
    public TUC_GiftTransactions() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedgerNumber.Text = Catalog.GetString("Ledger:");
      this.lblBatchTotal.Text = Catalog.GetString("Batch Total:");
      this.lblBatchNumber.Text = Catalog.GetString("Gift Batch:");
      this.lblHashTotal.Text = Catalog.GetString("Hash Total:");
      this.btnNewGift.Text = Catalog.GetString("&Add Gift");
      this.btnNewDetail.Text = Catalog.GetString("Add Detai&l");
      this.btnDeleteDetail.Text = Catalog.GetString("&Delete Detail");
      this.txtDetailDonorKey.ButtonText = Catalog.GetString("Find");
      this.lblDetailDonorKey.Text = Catalog.GetString("Donor:");
      this.lblDetailMethodOfGivingCode.Text = Catalog.GetString("Method of Giving:");
      this.lblDetailMethodOfPaymentCode.Text = Catalog.GetString("Method of Payment:");
      this.lblDetailReference.Text = Catalog.GetString("Reference:");
      this.lblDetailReceiptLetterCode.Text = Catalog.GetString("Letter Code:");
      this.lblDateEntered.Text = Catalog.GetString("Gift Date:");
      this.lblGiftTotal.Text = Catalog.GetString("Total:");
      this.lblDetailGiftTransactionAmount.Text = Catalog.GetString("Amount:");
      this.lblDetailConfidentialGiftFlag.Text = Catalog.GetString("Confidential?:");
      this.txtDetailRecipientKey.ButtonText = Catalog.GetString("Find");
      this.lblDetailRecipientKey.Text = Catalog.GetString("Recipient:");
      this.lblField.Text = Catalog.GetString("Field:");
      this.lblDetailChargeFlag.Text = Catalog.GetString("Admin Grants?:");
      this.lblMinistry.Text = Catalog.GetString("Key Ministry:");
      this.lblDetailMailingCode.Text = Catalog.GetString("Mailing:");
      this.lblDetailTaxDeductable.Text = Catalog.GetString("Tax deductable?:");
      this.lblDetailMotivationGroupCode.Text = Catalog.GetString("Motivation Group:");
      this.lblDetailMotivationDetailCode.Text = Catalog.GetString("Motivation Detail:");
      this.lblDetailCostCentreCode.Text = Catalog.GetString("Cost Centre:");
      this.lblDetailAccountCode.Text = Catalog.GetString("Account:");
      this.lblDetailGiftCommentOne.Text = Catalog.GetString("Comment 1:");
      this.lblDetailCommentOneType.Text = Catalog.GetString("for:");
      this.lblDetailGiftCommentTwo.Text = Catalog.GetString("Comment 2:");
      this.lblDetailCommentTwoType.Text = Catalog.GetString("for:");
      this.lblDetailGiftCommentThree.Text = Catalog.GetString("Comment 3:");
      this.lblDetailCommentThreeType.Text = Catalog.GetString("for:");
      #endregion

      this.txtLedgerNumber.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtBatchNumber.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtBatchStatus.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailReference.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailCostCentreCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailAccountCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailGiftCommentOne.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailGiftCommentTwo.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailGiftCommentThree.Font = TAppSettingsManager.GetDefaultBoldFont();
    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }

    /// dataset for the whole screen
    public Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      FPetraUtilsObject.SetStatusBarText(txtDetailGiftTransactionAmount, Catalog.GetString("Enter Your Currency Amount"));
      FPetraUtilsObject.SetStatusBarText(chkDetailConfidentialGiftFlag, Catalog.GetString("Make a selection"));
      FPetraUtilsObject.SetStatusBarText(txtDetailRecipientKey, Catalog.GetString("Enter the partner key"));
      FPetraUtilsObject.SetStatusBarText(chkDetailChargeFlag, Catalog.GetString("To determine whether an admin fee on the transaction should be overwritten if it normally has a charge associated with it. Used for both local and ilt transaction."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailMailingCode, Catalog.GetString("The mailing code if the gift was given in response to a mailing"));
      FPetraUtilsObject.SetStatusBarText(chkDetailTaxDeductable, Catalog.GetString("Is this gift tax deductable?"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailMotivationGroupCode, Catalog.GetString("Enter a motivation group code"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailMotivationDetailCode, Catalog.GetString("Enter a motivation detail code"));
      FPetraUtilsObject.SetStatusBarText(txtDetailCostCentreCode, Catalog.GetString("Enter a cost centre code"));
      FPetraUtilsObject.SetStatusBarText(txtDetailGiftCommentOne, Catalog.GetString("Enter a comment"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCommentOneType, Catalog.GetString("Make a selection"));
      FPetraUtilsObject.SetStatusBarText(txtDetailGiftCommentTwo, Catalog.GetString("Enter a comment"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCommentTwoType, Catalog.GetString("Make a selection"));
      FPetraUtilsObject.SetStatusBarText(txtDetailGiftCommentThree, Catalog.GetString("Enter a comment"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCommentThreeType, Catalog.GetString("Make a selection"));
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Gift Transaction Number", FMainDS.AGiftDetail.ColumnGiftTransactionNumber);
      grdDetails.AddTextColumn("Gift Number", FMainDS.AGiftDetail.ColumnDetailNumber);
      grdDetails.AddTextColumn("Donor Name", FMainDS.AGiftDetail.ColumnDonorName);
      grdDetails.AddTextColumn("Class", FMainDS.AGiftDetail.ColumnDonorClass);
      grdDetails.AddCheckBoxColumn("Confidential Gift", FMainDS.AGiftDetail.ColumnConfidentialGiftFlag);
      grdDetails.AddCurrencyColumn("Transaction Gift Amount", FMainDS.AGiftDetail.ColumnGiftTransactionAmount);
      grdDetails.AddTextColumn("Recipient", FMainDS.AGiftDetail.ColumnRecipientDescription);
      grdDetails.AddTextColumn("Field", FMainDS.AGiftDetail.ColumnRecipientField);
      grdDetails.AddTextColumn("Motivation Group", FMainDS.AGiftDetail.ColumnMotivationGroupCode);
      grdDetails.AddTextColumn("Motivation Detail", FMainDS.AGiftDetail.ColumnMotivationDetailCode);
      grdDetails.AddTextColumn("Receipt", FMainDS.AGiftDetail.ColumnReceiptNumber);
      grdDetails.AddCheckBoxColumn("Receipt Printed", FMainDS.AGiftDetail.ColumnReceiptPrinted);
      grdDetails.AddTextColumn("Method of Giving", FMainDS.AGiftDetail.ColumnMethodOfGivingCode);
      grdDetails.AddTextColumn("Method of Payment", FMainDS.AGiftDetail.ColumnMethodOfPaymentCode);
      grdDetails.AddTextColumn("Mailing Code", FMainDS.AGiftDetail.ColumnMailingCode);
      grdDetails.AddTextColumn("Gift Amount (Base)", FMainDS.AGiftDetail.ColumnGiftAmount);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AGiftDetail.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      ShowData();
    }

    /// automatically generated, create a new record of AGiftDetail and display on the edit screen
    public bool CreateNewAGiftDetail()
    {
        // we create the table locally, no dataset
        GiftBatchTDSAGiftDetailRow NewRow = FMainDS.AGiftDetail.NewRowTyped(true);
        FMainDS.AGiftDetail.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftDetail.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AGiftDetail.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AGiftDetail.PrimaryKey)
            {
                string value1 = FMainDS.AGiftDetail.Rows[ARowNumberInTable][myColumn].ToString();
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
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(RowNumberGrid, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(RowNumberGrid, 0), true);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the selected row
    public GiftBatchTDSAGiftDetailRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (GiftBatchTDSAGiftDetailRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        ShowDataManual();
        pnlDetails.Enabled = false;
        if (FMainDS.AGiftDetail != null)
        {
            DataView myDataView = FMainDS.AGiftDetail.DefaultView;
            myDataView.Sort = "a_gift_transaction_number_i ASC";
            myDataView.RowFilter = "a_batch_number_i = " + FBatchNumber.ToString();
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (myDataView.Count > 0)
            {
                grdDetails.Selection.ResetSelection(false);
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(GiftBatchTDSAGiftDetailRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            ShowDetailsManual(ARow);
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            if (ARow.IsMethodOfGivingCodeNull())
            {
                cmbDetailMethodOfGivingCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfGivingCode.SetSelectedString(ARow.MethodOfGivingCode);
            }
            if (ARow.IsMethodOfPaymentCodeNull())
            {
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(ARow.MethodOfPaymentCode);
            }
            txtDetailGiftTransactionAmount.NumberValueDecimal = Convert.ToDecimal(ARow.GiftTransactionAmount);
            chkDetailConfidentialGiftFlag.Checked = ARow.ConfidentialGiftFlag;
            txtDetailRecipientKey.Text = String.Format("{0:0000000000}", ARow.RecipientKey);
            if (ARow.IsChargeFlagNull())
            {
                chkDetailChargeFlag.Checked = false;
            }
            else
            {
                chkDetailChargeFlag.Checked = ARow.ChargeFlag;
            }
            if (ARow.IsMailingCodeNull())
            {
                cmbDetailMailingCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMailingCode.SetSelectedString(ARow.MailingCode);
            }
            if (ARow.IsTaxDeductableNull())
            {
                chkDetailTaxDeductable.Checked = false;
            }
            else
            {
                chkDetailTaxDeductable.Checked = ARow.TaxDeductable;
            }
            cmbDetailMotivationGroupCode.SetSelectedString(ARow.MotivationGroupCode);
            cmbDetailMotivationDetailCode.SetSelectedString(ARow.MotivationDetailCode);
            if (ARow.IsCostCentreCodeNull())
            {
                txtDetailCostCentreCode.Text = String.Empty;
            }
            else
            {
                txtDetailCostCentreCode.Text = ARow.CostCentreCode;
            }
            if (ARow.IsAccountCodeNull())
            {
                txtDetailAccountCode.Text = String.Empty;
            }
            else
            {
                txtDetailAccountCode.Text = ARow.AccountCode;
            }
            if (ARow.IsGiftCommentOneNull())
            {
                txtDetailGiftCommentOne.Text = String.Empty;
            }
            else
            {
                txtDetailGiftCommentOne.Text = ARow.GiftCommentOne;
            }
            if (ARow.IsCommentOneTypeNull())
            {
                cmbDetailCommentOneType.SelectedIndex = -1;
            }
            else
            {
                cmbDetailCommentOneType.SetSelectedString(ARow.CommentOneType);
            }
            if (ARow.IsGiftCommentTwoNull())
            {
                txtDetailGiftCommentTwo.Text = String.Empty;
            }
            else
            {
                txtDetailGiftCommentTwo.Text = ARow.GiftCommentTwo;
            }
            if (ARow.IsCommentTwoTypeNull())
            {
                cmbDetailCommentTwoType.SelectedIndex = -1;
            }
            else
            {
                cmbDetailCommentTwoType.SetSelectedString(ARow.CommentTwoType);
            }
            if (ARow.IsGiftCommentThreeNull())
            {
                txtDetailGiftCommentThree.Text = String.Empty;
            }
            else
            {
                txtDetailGiftCommentThree.Text = ARow.GiftCommentThree;
            }
            if (ARow.IsCommentThreeTypeNull())
            {
                cmbDetailCommentThreeType.SelectedIndex = -1;
            }
            else
            {
                cmbDetailCommentThreeType.SetSelectedString(ARow.CommentThreeType);
            }
            ShowDetailsManual(ARow);
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private GiftBatchTDSAGiftDetailRow FPreviouslySelectedDetailRow = null;
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
    }

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
    }

    private void GetDetailsFromControls(GiftBatchTDSAGiftDetailRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            if (cmbDetailMethodOfGivingCode.SelectedIndex == -1)
            {
                ARow.SetMethodOfGivingCodeNull();
            }
            else
            {
                ARow.MethodOfGivingCode = cmbDetailMethodOfGivingCode.GetSelectedString();
            }
            if (cmbDetailMethodOfPaymentCode.SelectedIndex == -1)
            {
                ARow.SetMethodOfPaymentCodeNull();
            }
            else
            {
                ARow.MethodOfPaymentCode = cmbDetailMethodOfPaymentCode.GetSelectedString();
            }
            ARow.GiftTransactionAmount = Convert.ToDecimal(txtDetailGiftTransactionAmount.NumberValueDecimal);
            ARow.ConfidentialGiftFlag = chkDetailConfidentialGiftFlag.Checked;
            ARow.RecipientKey = Convert.ToInt64(txtDetailRecipientKey.Text);
            ARow.ChargeFlag = chkDetailChargeFlag.Checked;
            if (cmbDetailMailingCode.SelectedIndex == -1)
            {
                ARow.SetMailingCodeNull();
            }
            else
            {
                ARow.MailingCode = cmbDetailMailingCode.GetSelectedString();
            }
            ARow.TaxDeductable = chkDetailTaxDeductable.Checked;
            ARow.MotivationGroupCode = cmbDetailMotivationGroupCode.GetSelectedString();
            ARow.MotivationDetailCode = cmbDetailMotivationDetailCode.GetSelectedString();
            if (txtDetailGiftCommentOne.Text.Length == 0)
            {
                ARow.SetGiftCommentOneNull();
            }
            else
            {
                ARow.GiftCommentOne = txtDetailGiftCommentOne.Text;
            }
            if (cmbDetailCommentOneType.SelectedIndex == -1)
            {
                ARow.SetCommentOneTypeNull();
            }
            else
            {
                ARow.CommentOneType = cmbDetailCommentOneType.GetSelectedString();
            }
            if (txtDetailGiftCommentTwo.Text.Length == 0)
            {
                ARow.SetGiftCommentTwoNull();
            }
            else
            {
                ARow.GiftCommentTwo = txtDetailGiftCommentTwo.Text;
            }
            if (cmbDetailCommentTwoType.SelectedIndex == -1)
            {
                ARow.SetCommentTwoTypeNull();
            }
            else
            {
                ARow.CommentTwoType = cmbDetailCommentTwoType.GetSelectedString();
            }
            if (txtDetailGiftCommentThree.Text.Length == 0)
            {
                ARow.SetGiftCommentThreeNull();
            }
            else
            {
                ARow.GiftCommentThree = txtDetailGiftCommentThree.Text;
            }
            if (cmbDetailCommentThreeType.SelectedIndex == -1)
            {
                ARow.SetCommentThreeTypeNull();
            }
            else
            {
                ARow.CommentThreeType = cmbDetailCommentThreeType.GetSelectedString();
            }
            GetDetailDataFromControlsManual(ARow);
            ARow.EndEdit();
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
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actNewGift")
        {
            btnNewGift.Enabled = e.Enabled;
        }
        if (e.ActionName == "actNewDetail")
        {
            btnNewDetail.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDeleteDetail")
        {
            btnDeleteDetail.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
