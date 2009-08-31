/* auto generated with nant generateWinforms from UC_GLJournals.yaml and template controlMaintainTable
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
  public partial class TUC_GLJournals: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS FMainDS;

	private Int32 FCurrentDetailIndex = -1;

    /// constructor
    public TUC_GLJournals() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblLedgerNumber.Text = Catalog.GetString("Ledger:");
      this.lblBatchNumber.Text = Catalog.GetString("Batch:");
      this.btnAdd.Text = Catalog.GetString("&Add");
      this.btnRemove.Text = Catalog.GetString("Remove");
      this.lblDetailJournalDescription.Text = Catalog.GetString("Journal Description:");
      this.lblDetailSubSystemCode.Text = Catalog.GetString("Sub System:");
      this.lblDetailTransactionTypeCode.Text = Catalog.GetString("Transaction Type:");
      this.lblDetailTransactionCurrency.Text = Catalog.GetString("Currency:");
      this.lblDetailDateEffective.Text = Catalog.GetString("Effective Date:");
      this.lblDetailExchangeRateToBase.Text = Catalog.GetString("Exchange Rate to Base:");
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
      FPetraUtilsObject.SetStatusBarText(txtDetailJournalDescription, Catalog.GetString("Enter a description for this general ledger journal."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailSubSystemCode, Catalog.GetString("The subsystem from which this journal came."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailTransactionTypeCode, Catalog.GetString("Select the type of journal."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailTransactionCurrency, Catalog.GetString("Select a currency code to use for the journal transactions."));
      cmbDetailTransactionCurrency.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(dtpDetailDateEffective, Catalog.GetString("Enter the date for the journal to come into effect."));
      FPetraUtilsObject.SetStatusBarText(txtDetailExchangeRateToBase, Catalog.GetString("Enter the exchange rate from the transaction currency to base."));
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Journal Number", FMainDS.AJournal.ColumnJournalNumber);
      grdDetails.AddTextColumn("Journal Status", FMainDS.AJournal.ColumnJournalStatus);
      grdDetails.AddTextColumn("Journal Debit Total", FMainDS.AJournal.ColumnJournalDebitTotal);
      grdDetails.AddTextColumn("Journal Credit Total", FMainDS.AJournal.ColumnJournalCreditTotal);
      grdDetails.AddTextColumn("Transaction Currency", FMainDS.AJournal.ColumnTransactionCurrency);
      grdDetails.AddTextColumn("Journal Description", FMainDS.AJournal.ColumnJournalDescription);
      grdDetails.AddTextColumn("Sub System", FMainDS.AJournal.ColumnSubSystemCode);
      grdDetails.AddTextColumn("Transaction Type", FMainDS.AJournal.ColumnTransactionTypeCode);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AJournal.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

      ShowData();
    }

    /// automatically generated, create a new record of AJournal and display on the edit screen
    public bool CreateNewAJournal()
    {
        // we create the table locally, no dataset
        Ict.Petra.Shared.MFinance.Account.Data.AJournalRow NewRow = FMainDS.AJournal.NewRowTyped(true);
        NewRowManual(ref NewRow);
        FMainDS.AJournal.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AJournal.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AJournal.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AJournal.PrimaryKey)
            {
                string value1 = FMainDS.AJournal.Rows[ARowNumberInTable][myColumn].ToString();
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

            for (int Counter = 0; Counter < FMainDS.AJournal.Rows.Count; Counter++)
            {
                bool found = true;
                foreach (DataColumn myColumn in FMainDS.AJournal.PrimaryKey)
                {
                    if (FMainDS.AJournal.Rows[Counter][myColumn].ToString() !=
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
        ShowDataManual();
        pnlDetails.Enabled = false;
        if (FMainDS.AJournal != null)
        {
            DataView myDataView = FMainDS.AJournal.DefaultView;
            myDataView.Sort = "a_journal_number_i ASC";
            myDataView.RowFilter = "a_batch_number_i = " + FBatchNumber.ToString();
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
            ShowDetailsManual(ACurrentDetailIndex);
        }
        else
        {
            pnlDetails.Enabled = true;
            FCurrentDetailIndex = ACurrentDetailIndex;
            BeforeShowDetailsManual(FMainDS.AJournal[ACurrentDetailIndex]);
            txtDetailJournalDescription.Text = FMainDS.AJournal[ACurrentDetailIndex].JournalDescription;
            cmbDetailSubSystemCode.SetSelectedString(FMainDS.AJournal[ACurrentDetailIndex].SubSystemCode);
            if (FMainDS.AJournal[ACurrentDetailIndex].IsTransactionTypeCodeNull())
            {
                cmbDetailTransactionTypeCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailTransactionTypeCode.SetSelectedString(FMainDS.AJournal[ACurrentDetailIndex].TransactionTypeCode);
            }
            cmbDetailTransactionCurrency.SetSelectedString(FMainDS.AJournal[ACurrentDetailIndex].TransactionCurrency);
            dtpDetailDateEffective.Value = FMainDS.AJournal[ACurrentDetailIndex].DateEffective;
            txtDetailExchangeRateToBase.Text = FMainDS.AJournal[ACurrentDetailIndex].ExchangeRateToBase.ToString();
            ShowDetailsManual(ACurrentDetailIndex);
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
            FMainDS.AJournal.Rows[ACurrentDetailIndex].BeginEdit();
            FMainDS.AJournal[ACurrentDetailIndex].JournalDescription = txtDetailJournalDescription.Text;
            if (cmbDetailTransactionTypeCode.SelectedIndex == -1)
            {
                FMainDS.AJournal[ACurrentDetailIndex].SetTransactionTypeCodeNull();
            }
            else
            {
                FMainDS.AJournal[ACurrentDetailIndex].TransactionTypeCode = cmbDetailTransactionTypeCode.GetSelectedString();
            }
            FMainDS.AJournal[ACurrentDetailIndex].TransactionCurrency = cmbDetailTransactionCurrency.GetSelectedString();
            FMainDS.AJournal[ACurrentDetailIndex].ExchangeRateToBase = Convert.ToDouble(txtDetailExchangeRateToBase.Text);
            FMainDS.AJournal.Rows[ACurrentDetailIndex].EndEdit();
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
            btnAdd.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
