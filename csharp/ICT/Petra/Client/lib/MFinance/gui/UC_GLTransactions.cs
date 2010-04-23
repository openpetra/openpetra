/* auto generated with nant generateWinforms from UC_GLTransactions.yaml and template controlMaintainTable
 *
 * DO NOT edit manually, DO NOT edit with the designer
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
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.GL
{

  /// auto generated user control
  public partial class TUC_GLTransactions: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;

    /// constructor
    public TUC_GLTransactions() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedgerNumber.Text = Catalog.GetString("Ledger:");
      this.lblBatchNumber.Text = Catalog.GetString("Batch:");
      this.lblJournalNumber.Text = Catalog.GetString("Journal:");
      this.btnNew.Text = Catalog.GetString("&Add");
      this.btnRemove.Text = Catalog.GetString("Remove");
      this.lblDetailCostCentreCode.Text = Catalog.GetString("Cost Centre Code:");
      this.lblDetailAccountCode.Text = Catalog.GetString("Account Code:");
      this.lblDetailNarrative.Text = Catalog.GetString("Narrative:");
      this.lblDetailReference.Text = Catalog.GetString("Reference:");
      this.lblDetailTransactionDate.Text = Catalog.GetString("Transaction Date:");
      this.lblDetailKeyMinistryKey.Text = Catalog.GetString("Key Ministry:");
      this.lblTransactionCurrency.Text = Catalog.GetString("TODOTransactionCurrency:");
      this.lblBaseCurrency.Text = Catalog.GetString("TODOBase Currency:");
      this.lblDebitAmount.Text = Catalog.GetString("Dr Amount:");
      this.lblDebitAmountBase.Text = Catalog.GetString("Dr Amount:");
      this.lblCreditAmount.Text = Catalog.GetString("Cr Amount:");
      this.lblCreditAmountBase.Text = Catalog.GetString("Cr Amount:");
      this.lblDebitTotalAmount.Text = Catalog.GetString("Debit Total:");
      this.lblDebitTotalAmountBase.Text = Catalog.GetString("Debit Total:");
      this.lblCreditTotalAmount.Text = Catalog.GetString("Credit Total:");
      this.lblCreditTotalAmountBase.Text = Catalog.GetString("Credit Total:");
      #endregion

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
    public Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      FPetraUtilsObject.SetStatusBarText(cmbDetailCostCentreCode, Catalog.GetString("Enter a cost centre code (department or fund)."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountCode, Catalog.GetString("Enter an account code."));
      FPetraUtilsObject.SetStatusBarText(txtDetailNarrative, Catalog.GetString("Enter a description of the transaction."));
      FPetraUtilsObject.SetStatusBarText(txtDetailReference, Catalog.GetString("(Optional) Enter a reference code."));
      FPetraUtilsObject.SetStatusBarText(dtpDetailTransactionDate, Catalog.GetString("The date the transaction is to take effect (same as journal)."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailKeyMinistryKey, Catalog.GetString("Key ministry to which this transaction applies (just for fund transfers)"));
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Transaction Number", FMainDS.ATransaction.ColumnTransactionNumber);
      grdDetails.AddDateColumn("Transaction Date", FMainDS.ATransaction.ColumnTransactionDate);
      grdDetails.AddTextColumn("Cost Centre Code", FMainDS.ATransaction.ColumnCostCentreCode);
      grdDetails.AddTextColumn("Account Code", FMainDS.ATransaction.ColumnAccountCode);
      grdDetails.AddCurrencyColumn("Transaction Amount", FMainDS.ATransaction.ColumnTransactionAmount);
      grdDetails.AddTextColumn("Credit/Debit", FMainDS.ATransaction.ColumnDebitCreditIndicator);
      grdDetails.AddTextColumn("Reference", FMainDS.ATransaction.ColumnReference);
      grdDetails.AddTextColumn("Narrative", FMainDS.ATransaction.ColumnNarrative);
      grdDetails.AddCurrencyColumn("Amount in Base Currency", FMainDS.ATransaction.ColumnAmountInBaseCurrency);
      grdDetails.AddCurrencyColumn("Amount in International Currency", FMainDS.ATransaction.ColumnAmountInIntlCurrency);
      grdDetails.AddTextColumn("Analysis Attributes", FMainDS.ATransaction.ColumnAnalysisAttributes);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.ATransaction.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

      ShowData();
    }

    /// automatically generated, create a new record of ATransaction and display on the edit screen
    public bool CreateNewATransaction()
    {
        // we create the table locally, no dataset
        GLBatchTDSATransactionRow NewRow = FMainDS.ATransaction.NewRowTyped(true);
        NewRowManual(ref NewRow);
        FMainDS.ATransaction.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransaction.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.ATransaction.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.ATransaction.PrimaryKey)
            {
                string value1 = FMainDS.ATransaction.Rows[ARowNumberInTable][myColumn].ToString();
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
    private GLBatchTDSATransactionRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (GLBatchTDSATransactionRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        ShowDataManual();
        pnlDetails.Enabled = false;
        if (FMainDS.ATransaction != null)
        {
            DataView myDataView = FMainDS.ATransaction.DefaultView;
            myDataView.Sort = "a_transaction_number_i ASC";
            myDataView.RowFilter = "a_batch_number_i = " + FBatchNumber.ToString() + " and a_journal_number_i = " + FJournalNumber.ToString();
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdDetails.AutoSizeCells();
            if (myDataView.Count > 0)
            {
                grdDetails.Selection.ResetSelection(false);
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = true;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(GLBatchTDSATransactionRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            ShowDetailsManual(ARow);
        }
        else
        {
            pnlDetails.Enabled = true;
            FPreviouslySelectedDetailRow = ARow;
            cmbDetailCostCentreCode.SetSelectedString(ARow.CostCentreCode);
            cmbDetailAccountCode.SetSelectedString(ARow.AccountCode);
            if (ARow.IsNarrativeNull())
            {
                txtDetailNarrative.Text = String.Empty;
            }
            else
            {
                txtDetailNarrative.Text = ARow.Narrative;
            }
            if (ARow.IsReferenceNull())
            {
                txtDetailReference.Text = String.Empty;
            }
            else
            {
                txtDetailReference.Text = ARow.Reference;
            }
            dtpDetailTransactionDate.Date = ARow.TransactionDate;
            if (ARow.IsKeyMinistryKeyNull())
            {
                cmbDetailKeyMinistryKey.SelectedIndex = -1;
            }
            else
            {
                cmbDetailKeyMinistryKey.SetSelectedInt64(ARow.KeyMinistryKey);
            }
            ShowDetailsManual(ARow);
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private GLBatchTDSATransactionRow FPreviouslySelectedDetailRow = null;
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

    private void GetDetailsFromControls(GLBatchTDSATransactionRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            ARow.CostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
            ARow.AccountCode = cmbDetailAccountCode.GetSelectedString();
            if (txtDetailNarrative.Text.Length == 0)
            {
                ARow.SetNarrativeNull();
            }
            else
            {
                ARow.Narrative = txtDetailNarrative.Text;
            }
            if (txtDetailReference.Text.Length == 0)
            {
                ARow.SetReferenceNull();
            }
            else
            {
                ARow.Reference = txtDetailReference.Text;
            }
            ARow.TransactionDate = dtpDetailTransactionDate.Date.Value;
            if (cmbDetailKeyMinistryKey.SelectedIndex == -1)
            {
                ARow.SetKeyMinistryKeyNull();
            }
            else
            {
                ARow.KeyMinistryKey = cmbDetailKeyMinistryKey.GetSelectedInt64();
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
        if (e.ActionName == "actNew")
        {
            btnNew.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
