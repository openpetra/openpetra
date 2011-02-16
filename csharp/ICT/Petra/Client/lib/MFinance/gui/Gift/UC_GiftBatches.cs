// auto generated with nant generateWinforms from UC_GiftBatches.yaml and template controlMaintainTable
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
  public partial class TUC_GiftBatches: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS FMainDS;

    /// constructor
    public TUC_GiftBatches() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedgerNumber.Text = Catalog.GetString("Ledger:");
      this.rbtPosted.Text = Catalog.GetString("Posted");
      this.rbtEditing.Text = Catalog.GetString("Editing");
      this.rbtAll.Text = Catalog.GetString("All");
      this.rgrShowBatches.Text = Catalog.GetString("Show batches");
      this.btnNew.Text = Catalog.GetString("&Add");
      this.btnDelete.Text = Catalog.GetString("&Cancel");
      this.btnPostBatch.Text = Catalog.GetString("&Post Batch");
      this.lblDetailBatchDescription.Text = Catalog.GetString("Batch Description:");
      this.lblDetailBankCostCentre.Text = Catalog.GetString("Cost Centre:");
      this.lblDetailBankAccountCode.Text = Catalog.GetString("Bank Account:");
      this.lblDetailGlEffectiveDate.Text = Catalog.GetString("GL Effective Date:");
      this.lblValidDateRange.Text = Catalog.GetString("Valid Date Range:");
      this.lblDetailBatchHashTotal.Text = Catalog.GetString("Hash Total:");
      this.lblDetailCurrencyCode.Text = Catalog.GetString("Currency Code:");
      this.lblDetailExchangeRateToBase.Text = Catalog.GetString("Exchange Rate To Base:");
      this.lblDetailMethodOfPaymentCode.Text = Catalog.GetString("Method of Payment:");
      this.rbtGift.Text = Catalog.GetString("Gift");
      this.rbtGiftInKind.Text = Catalog.GetString("Gift In Kind");
      this.rbtOther.Text = Catalog.GetString("Other");
      this.rgrDetailGiftType.Text = Catalog.GetString("Gift Type");
      this.tbbPostBatch.Text = Catalog.GetString("&Post Batch");
      this.tbbExportBatches.Text = Catalog.GetString("&Export Batches");
      this.tbbImportBatches.Text = Catalog.GetString("&Import Batches");
      this.mniPost.Text = Catalog.GetString("&Post Batch");
      this.mniBatch.Text = Catalog.GetString("&Batch");
      #endregion

      this.txtLedgerNumber.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailBatchDescription.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailBatchHashTotal.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailExchangeRateToBase.Font = TAppSettingsManager.GetDefaultBoldFont();
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
      FPetraUtilsObject.SetStatusBarText(txtDetailBatchDescription, Catalog.GetString("Enter a description for the gift batch."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailBankCostCentre, Catalog.GetString("Enter a cost centre code"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailBankAccountCode, Catalog.GetString("Enter the bank account which this batch is for."));
      FPetraUtilsObject.SetStatusBarText(dtpDetailGlEffectiveDate, Catalog.GetString("Effective date to be used when posted to the general ledger."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCurrencyCode, Catalog.GetString("Select a currency code to use for the journal transactions."));
      cmbDetailCurrencyCode.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(txtDetailExchangeRateToBase, Catalog.GetString("Enter the exchange rate from the transaction currency to base."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailMethodOfPaymentCode, Catalog.GetString("Enter the method of payment"));
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Batch Number", FMainDS.AGiftBatch.ColumnBatchNumber);
      grdDetails.AddTextColumn("Batch Status", FMainDS.AGiftBatch.ColumnBatchStatus);
      grdDetails.AddDateColumn("GL Effective Date", FMainDS.AGiftBatch.ColumnGlEffectiveDate);
      grdDetails.AddCurrencyColumn("Hash Total", FMainDS.AGiftBatch.ColumnHashTotal);
      grdDetails.AddTextColumn("Batch description", FMainDS.AGiftBatch.ColumnBatchDescription);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AGiftBatch.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      ShowData();
    }

    /// automatically generated, create a new record of AGiftBatch and display on the edit screen
    public bool CreateNewAGiftBatch()
    {
        FMainDS.Merge(TRemote.MFinance.Gift.WebConnectors.CreateAGiftBatch(FLedgerNumber, FDateEffective));

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AGiftBatch.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AGiftBatch.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AGiftBatch.PrimaryKey)
            {
                string value1 = FMainDS.AGiftBatch.Rows[ARowNumberInTable][myColumn].ToString();
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
    public AGiftBatchRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (AGiftBatchRow)SelectedGridRow[0].Row;
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
        if (FMainDS.AGiftBatch != null)
        {
            DataView myDataView = FMainDS.AGiftBatch.DefaultView;
            myDataView.Sort = "a_batch_number_i DESC";
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

    private void ShowDetails(AGiftBatchRow ARow)
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
            if (ARow.IsBatchDescriptionNull())
            {
                txtDetailBatchDescription.Text = String.Empty;
            }
            else
            {
                txtDetailBatchDescription.Text = ARow.BatchDescription;
            }
            cmbDetailBankCostCentre.SetSelectedString(ARow.BankCostCentre);
            cmbDetailBankAccountCode.SetSelectedString(ARow.BankAccountCode);
            dtpDetailGlEffectiveDate.Date = ARow.GlEffectiveDate;
            cmbDetailCurrencyCode.SetSelectedString(ARow.CurrencyCode);
            txtDetailExchangeRateToBase.Text = ARow.ExchangeRateToBase.ToString();
            if (ARow.IsMethodOfPaymentCodeNull())
            {
                cmbDetailMethodOfPaymentCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailMethodOfPaymentCode.SetSelectedString(ARow.MethodOfPaymentCode);
            }
            ShowDetailsManual(ARow);
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private AGiftBatchRow FPreviouslySelectedDetailRow = null;
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

    private void GetDetailsFromControls(AGiftBatchRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            if (txtDetailBatchDescription.Text.Length == 0)
            {
                ARow.SetBatchDescriptionNull();
            }
            else
            {
                ARow.BatchDescription = txtDetailBatchDescription.Text;
            }
            ARow.BankCostCentre = cmbDetailBankCostCentre.GetSelectedString();
            ARow.BankAccountCode = cmbDetailBankAccountCode.GetSelectedString();
            ARow.GlEffectiveDate = dtpDetailGlEffectiveDate.Date.Value;
            ARow.CurrencyCode = cmbDetailCurrencyCode.GetSelectedString();
            ARow.ExchangeRateToBase = Convert.ToDecimal(txtDetailExchangeRateToBase.Text);
            if (cmbDetailMethodOfPaymentCode.SelectedIndex == -1)
            {
                ARow.SetMethodOfPaymentCodeNull();
            }
            else
            {
                ARow.MethodOfPaymentCode = cmbDetailMethodOfPaymentCode.GetSelectedString();
            }
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
        if (e.ActionName == "actNew")
        {
            btnNew.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDelete")
        {
            btnDelete.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPostBatch")
        {
            btnPostBatch.Enabled = e.Enabled;
            tbbPostBatch.Enabled = e.Enabled;
            mniPost.Enabled = e.Enabled;
        }
        if (e.ActionName == "actExportBatches")
        {
            tbbExportBatches.Enabled = e.Enabled;
        }
        if (e.ActionName == "actImportBatches")
        {
            tbbImportBatches.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
