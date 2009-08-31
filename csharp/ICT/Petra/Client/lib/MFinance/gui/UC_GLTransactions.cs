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

namespace Ict.Petra.Client.MFinance.Gui.GL
{

  /// auto generated user control
  public partial class TUC_GLTransactions: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;

	private Int32 FCurrentDetailIndex = -1;

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
      this.lblBaseCurrency.Text = Catalog.GetString("TODOBaseCurrency:");
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
      grdDetails.AddTextColumn("Transaction Posted Status", FMainDS.ATransaction.ColumnTransactionStatus);
      grdDetails.AddTextColumn("Transaction Date", FMainDS.ATransaction.ColumnTransactionDate);
      grdDetails.AddTextColumn("Debit/Credit Indicator", FMainDS.ATransaction.ColumnDebitCreditIndicator);
      grdDetails.AddTextColumn("Cost Centre Code", FMainDS.ATransaction.ColumnCostCentreCode);
      grdDetails.AddTextColumn("Account Code", FMainDS.ATransaction.ColumnAccountCode);
      grdDetails.AddTextColumn("Transaction Amount", FMainDS.ATransaction.ColumnTransactionAmount);
      grdDetails.AddTextColumn("Reference", FMainDS.ATransaction.ColumnReference);
      grdDetails.AddTextColumn("Narrative", FMainDS.ATransaction.ColumnNarrative);
      grdDetails.AddTextColumn("Date Entered", FMainDS.ATransaction.ColumnDateEntered);
      grdDetails.AddTextColumn("Amount in Base Currency", FMainDS.ATransaction.ColumnAmountInBaseCurrency);
      grdDetails.AddTextColumn("Amount in International Currency", FMainDS.ATransaction.ColumnAmountInIntlCurrency);
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
        Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDSATransactionRow NewRow = FMainDS.ATransaction.NewRowTyped(true);
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
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).mDataView[Counter][myColumn.Ordinal].ToString();
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

    /// return the index in the detail datatable of the selected row, not the index in the datagrid
    private Int32 GetSelectedDetailDataTableIndex()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            // this would return the index in the grid: return grdDetails.DataSource.IndexOf(SelectedGridRow[0]);
            // we could keep track of the order in the datatable ourselves: return Convert.ToInt32(SelectedGridRow[0][ORIGINALINDEX]);
            // does not seem to work: return grdDetails.DataSourceRowToIndex2(SelectedGridRow[0]);

            for (int Counter = 0; Counter < FMainDS.ATransaction.Rows.Count; Counter++)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.ATransaction.PrimaryKey)
                {
                    if (FMainDS.ATransaction.Rows[Counter][myColumn].ToString() !=
                        SelectedGridRow[0][myColumn.Ordinal].ToString())
                    {
                        found = false;
                    }

                }
                if (found)
                {
                    return Counter;
                }
            }
        }

        return -1;
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
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
                ShowDetails(GetSelectedDetailDataTableIndex());
                pnlDetails.Enabled = true;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(Int32 ACurrentDetailIndex)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ACurrentDetailIndex == -1)
        {
            pnlDetails.Enabled = false;
        }
        else
        {
            pnlDetails.Enabled = true;
            FCurrentDetailIndex = ACurrentDetailIndex;
            cmbDetailCostCentreCode.SetSelectedString(FMainDS.ATransaction[ACurrentDetailIndex].CostCentreCode);
            cmbDetailAccountCode.SetSelectedString(FMainDS.ATransaction[ACurrentDetailIndex].AccountCode);
            if (FMainDS.ATransaction[ACurrentDetailIndex].IsNarrativeNull())
            {
                txtDetailNarrative.Text = String.Empty;
            }
            else
            {
                txtDetailNarrative.Text = FMainDS.ATransaction[ACurrentDetailIndex].Narrative;
            }
            if (FMainDS.ATransaction[ACurrentDetailIndex].IsReferenceNull())
            {
                txtDetailReference.Text = String.Empty;
            }
            else
            {
                txtDetailReference.Text = FMainDS.ATransaction[ACurrentDetailIndex].Reference;
            }
            dtpDetailTransactionDate.Value = FMainDS.ATransaction[ACurrentDetailIndex].TransactionDate;
            if (FMainDS.ATransaction[ACurrentDetailIndex].IsKeyMinistryKeyNull())
            {
                cmbDetailKeyMinistryKey.SelectedIndex = -1;
            }
            else
            {
                cmbDetailKeyMinistryKey.SetSelectedInt64(FMainDS.ATransaction[ACurrentDetailIndex].KeyMinistryKey);
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // get the details from the previously selected row
        if (FCurrentDetailIndex != -1)
        {
            GetDetailsFromControls(FCurrentDetailIndex);
        }
        // display the details of the currently selected row
        ShowDetails(GetSelectedDetailDataTableIndex());
    }

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(FCurrentDetailIndex);
    }

    private void GetDetailsFromControls(Int32 ACurrentDetailIndex)
    {
        if (ACurrentDetailIndex != -1)
        {
            FMainDS.ATransaction.Rows[ACurrentDetailIndex].BeginEdit();
            FMainDS.ATransaction[ACurrentDetailIndex].CostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
            FMainDS.ATransaction[ACurrentDetailIndex].AccountCode = cmbDetailAccountCode.GetSelectedString();
            if (txtDetailNarrative.Text.Length == 0)
            {
                FMainDS.ATransaction[ACurrentDetailIndex].SetNarrativeNull();
            }
            else
            {
                FMainDS.ATransaction[ACurrentDetailIndex].Narrative = txtDetailNarrative.Text;
            }
            if (txtDetailReference.Text.Length == 0)
            {
                FMainDS.ATransaction[ACurrentDetailIndex].SetReferenceNull();
            }
            else
            {
                FMainDS.ATransaction[ACurrentDetailIndex].Reference = txtDetailReference.Text;
            }
            FMainDS.ATransaction[ACurrentDetailIndex].TransactionDate = dtpDetailTransactionDate.Value;
            if (cmbDetailKeyMinistryKey.SelectedIndex == -1)
            {
                FMainDS.ATransaction[ACurrentDetailIndex].SetKeyMinistryKeyNull();
            }
            else
            {
                FMainDS.ATransaction[ACurrentDetailIndex].KeyMinistryKey = cmbDetailKeyMinistryKey.GetSelectedInt64();
            }
            FMainDS.ATransaction.Rows[ACurrentDetailIndex].EndEdit();
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
