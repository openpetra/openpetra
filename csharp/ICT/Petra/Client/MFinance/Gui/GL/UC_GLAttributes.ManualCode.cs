//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Diagnostics;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TUC_GLAttributes
    {
        private Int32 FLedgerNumber = -1;
        /// <summary>
        /// for fast init of the ledger number
        /// </summary>
        public int LedgerNumber {
            get
            {
                return FLedgerNumber;
            }
            set
            {
                FLedgerNumber = value;
            }
        }
        private Int32 FBatchNumber = -1;
        private Int32 FJournalNumber = -1;
        private Int32 FTransactionNumber = -1;
        private GLSetupTDS FCacheDS = null;

        /// <summary>
        /// for use of Analysis tables in other classes
        /// </summary>
        public GLSetupTDS CacheDS {
            get
            {
                CheckFCacheInitialised();
                return FCacheDS;
            }
        }

        private void InitializeManualCode()
        {
            this.cmbDetailAnalysisAttributeValue.DropDown += new System.EventHandler(this.DropDown);
            this.cmbDetailAnalysisAttributeValue.DropDownClosed += new System.EventHandler(this.ValueChanged);
        }

        /// <summary>
        /// load the transactions into the grid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        public void LoadAttributes(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber)
        {
            //Check if the same batch is selected, so no need to apply filter
            if ((FLedgerNumber == ALedgerNumber) && (FBatchNumber == ABatchNumber) && (FJournalNumber == AJournalNumber)
                && (FTransactionNumber == ATransactionNumber) && (FMainDS.ATransAnalAttrib.DefaultView.Count > 0))
            {
                GetDataFromControls();
                return;
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            FTransactionNumber = ATransactionNumber;
            FPreviouslySelectedDetailRow = null;

            if (!cmbDetailAnalysisAttributeValue.Enabled)
            {
                cmbDetailAnalysisAttributeValue.Enabled = true;
            }

            //DataView view = new DataView(FMainDS.ATransAnalAttrib);
            //view.RowStateFilter = DataViewRowState.CurrentRows | DataViewRowState.Deleted;

            // only load from server if there are no attributes loaded yet for this journal
            // otherwise we would overwrite attributes that have already been modified
            FMainDS.ATransAnalAttrib.DefaultView.RowFilter = string.Empty;
            FMainDS.ATransAnalAttrib.DefaultView.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(
                    ATransactionTable.TableId), ',');

            if (FMainDS.ATransAnalAttrib.DefaultView.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber, FTransactionNumber }) == -1)
            {
                FMainDS.ATransAnalAttrib.Clear();
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttrib(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber));
            }

            CheckFCacheInitialised();

            FMainDS.ATransAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                ATransAnalAttribTable.GetBatchNumberDBName(),
                FBatchNumber,
                ATransAnalAttribTable.GetJournalNumberDBName(),
                FJournalNumber,
                ATransAnalAttribTable.GetTransactionNumberDBName(),
                FTransactionNumber);

            FMainDS.ATransAnalAttrib.DefaultView.Sort = String.Format("{0} ASC",
                ATransAnalAttribTable.GetAnalysisTypeCodeDBName()
                );

            grdDetails.DataSource = null;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransAnalAttrib.DefaultView);

            FMainDS.ATransAnalAttrib.DefaultView.AllowNew = false;

            if (FMainDS.ATransAnalAttrib.DefaultView.Count > 0)
            {
                SelectRowInGrid(1);
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
                UpdateLabels();
            }
            else
            {
                ClearControls();
            }
        }

        private void UpdateLabels()
        {
            txtLedgerNumber.Text = FLedgerNumber.ToString();
            txtBatchNumber.Text = FBatchNumber.ToString();
            txtJournalNumber.Text = FJournalNumber.ToString();
            txtTransactionNumber.Text = FTransactionNumber.ToString();
        }

        private void ClearControls()
        {
            FPetraUtilsObject.DisableDataChangedEvent();
            txtLedgerNumber.Clear();
            txtBatchNumber.Clear();
            txtJournalNumber.Clear();
            txtTransactionNumber.Clear();
            txtReadonlyAnalysisTypeCode.Clear();
            txtReadonlyDescription.Clear();
            cmbDetailAnalysisAttributeValue.Enabled = false;
            FPetraUtilsObject.EnableDataChangedEvent();
        }

        /// <summary>
        /// Unload the currently loaded attributes
        /// </summary>
        public void UnloadAttributes()
        {
            if (FMainDS.ATransAnalAttrib.DefaultView.Count > 0)
            {
                FMainDS.ATransAnalAttrib.Clear();
                grdDetails.DataSource = null;
            }

            FPreviouslySelectedDetailRow = null;
            FLedgerNumber = -1;
            FBatchNumber = -1;
            FJournalNumber = -1;
            FTransactionNumber = -1;
            FPetraUtilsObject.HasChanges = false;
        }

        /// <summary>
        /// get the details of the current journal
        /// </summary>
        /// <returns></returns>
        private AJournalRow GetJournalRow()
        {
            return (AJournalRow)FMainDS.AJournal.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber });
        }

        private ABatchRow GetBatchRow()
        {
            return (ABatchRow)FMainDS.ABatch.Rows.Find(new object[] { FLedgerNumber, FBatchNumber });
        }

        private ATransactionRow GetTransactionRow()
        {
            return (ATransactionRow)FMainDS.ATransaction.Rows.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber, FTransactionNumber });
        }

        /// <summary>
        /// Return the FMainDS dataset
        /// </summary>
        public Ict.Petra.Shared.MFinance.GL.Data.GLBatchTDS GetAttributesDataSet()
        {
            return FMainDS;
        }

        /// <summary>
        /// add a new transactions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewRow(System.Object sender, EventArgs e)
        {
            this.CreateNewATransAnalAttrib();
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated
        /// </summary>
        /// <param name="ANewRow">returns the modified new transaction row</param>
        /// <param name="ARefTransactionRow">this can be null; otherwise this is the transaction that the Attribute should belong to</param>
        public void NewRowManual(ref ATransAnalAttribRow ANewRow, ATransactionRow ARefTransactionRow)
        {
            if (ARefTransactionRow == null)
            {
                ARefTransactionRow = GetTransactionRow();
            }

            ANewRow.LedgerNumber = ARefTransactionRow.LedgerNumber;
            ANewRow.BatchNumber = ARefTransactionRow.BatchNumber;
            ANewRow.JournalNumber = ARefTransactionRow.JournalNumber;
            ANewRow.TransactionNumber = ARefTransactionRow.TransactionNumber;
        }

        /// <summary>
        /// make sure the correct transaction number is assigned and the journal.lastTransactionNumber is updated;
        /// will use the currently selected journal
        /// </summary>
        public void NewRowManual(ref ATransAnalAttribRow ANewRow)
        {
            NewRowManual(ref ANewRow, null);
        }

        private void ShowDataManual()
        {
            txtLedgerNumber.Text = TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);
            txtBatchNumber.Text = FBatchNumber.ToString();
            txtJournalNumber.Text = FJournalNumber.ToString();
            txtTransactionNumber.Text = FTransactionNumber.ToString();

            cmbDetailAnalysisAttributeValue.Items.Clear();  // These details controls may be set by
            txtReadonlyAnalysisTypeCode.Text = "";          // ShowDetailsManual, below - or they may not...
            txtReadonlyDescription.Text = "";
            cmbDetailAnalysisAttributeValue.SetSelectedString("");

            CheckFCacheInitialised();
        }

        void CheckFCacheInitialised()
        {
            if ((FCacheDS == null) && (FLedgerNumber >= 0))
            {
                FCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(FLedgerNumber);
            }
        }

        private void ShowDetailsManual(ATransAnalAttribRow ARow)
        {
            cmbDetailAnalysisAttributeValue.Items.Clear();

            if (ARow == null)
            {
                return;
            }

            // The content of the combobox derives from the typecode, the ledgernumber and whether the value is active.

            foreach (AFreeformAnalysisRow AFRow in  FCacheDS.AFreeformAnalysis.Rows)
            {
                if (ARow.AnalysisTypeCode.Equals(AFRow.AnalysisTypeCode) && ARow.LedgerNumber.Equals(AFRow.LedgerNumber))
                {
                    // add value if it is active but also if not active and already set
                    if (AFRow.Active || (ARow.AnalysisAttributeValue == AFRow.AnalysisValue))
                    {
                        cmbDetailAnalysisAttributeValue.Items.Add(AFRow.AnalysisValue);
                    }
                }
            }

            txtReadonlyAnalysisTypeCode.Text = ARow.AnalysisTypeCode;
            AAnalysisTypeRow analysisTypeRow = (AAnalysisTypeRow)FCacheDS.AAnalysisType.Rows.Find(new Object[] { ARow.AnalysisTypeCode });
            txtReadonlyDescription.Text = analysisTypeRow.AnalysisTypeDescription;

            if ((ARow.AnalysisAttributeValue != null) && (ARow.AnalysisAttributeValue.Length > 0))
            {
                cmbDetailAnalysisAttributeValue.SetSelectedString(ARow.AnalysisAttributeValue);
            }
            else
            {
                cmbDetailAnalysisAttributeValue.SetSelectedString("", -1);
            }

            // If the batch has been posted, the Combobox can't be changed.
            Boolean changeable = GetBatchRow() != null
                                 && (GetBatchRow().BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            cmbDetailAnalysisAttributeValue.Enabled = changeable;
        }

        private void GetDetailDataFromControlsManual(ATransAnalAttribRow ARow)
        {
            //needed?
        }

        /// <summary>
        /// check if the necessary rows for the given account are there, automatically add/delete rows, update account
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        public void CheckAnalysisAttributes(int ALedgerNumber, int ABatchNumber, int AJournalNumber)
        {
            GLSetupTDS glSetupCacheDS = TRemote.MFinance.GL.WebConnectors.LoadAAnalysisAttributes(ALedgerNumber);

            //Account Number for AnalysisTable lookup
            int currentTransactionNumber = 0;
            string currentAccountCode = String.Empty;

            if (glSetupCacheDS == null)
            {
                return;
            }

            //Load all
            DataView allTransView = FMainDS.ATransaction.DefaultView;

            allTransView.RowFilter = String.Format("{0}={1} and {2}={3}",
                ATransactionTable.GetBatchNumberDBName(),
                ABatchNumber,
                ATransactionTable.GetJournalNumberDBName(),
                AJournalNumber);

            foreach (DataRowView transRowView in allTransView)
            {
                ATransactionRow tr = (ATransactionRow)transRowView.Row;

                currentAccountCode = tr.AccountCode;

                //Retrieve the analysis attributes for the supplied account
                DataView analAttrView = glSetupCacheDS.AAnalysisAttribute.DefaultView;
                analAttrView.RowFilter = String.Format("{0} = '{1}'",
                    AAnalysisAttributeTable.GetAccountCodeDBName(),
                    currentAccountCode);

                for (int i = 0; i < analAttrView.Count; i++)
                {
                    //Read the Type Code for each attribute row
                    AAnalysisAttributeRow analAttrRow = (AAnalysisAttributeRow)analAttrView[i].Row;
                    string analysisTypeCode = analAttrRow.AnalysisTypeCode;

                    //Check if the attribute type code exists in the Transaction Analysis Attributes table
                    ATransAnalAttribRow transAnalAttrRow =
                        (ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber,
                                                                                               currentTransactionNumber,
                                                                                               analysisTypeCode });

                    if (transAnalAttrRow == null)
                    {
                        //Create a new TypeCode for this account
                        ATransAnalAttribRow newRow = FMainDS.ATransAnalAttrib.NewRowTyped(true);
                        newRow.LedgerNumber = ALedgerNumber;
                        newRow.BatchNumber = ABatchNumber;
                        newRow.JournalNumber = AJournalNumber;
                        newRow.TransactionNumber = currentTransactionNumber;
                        newRow.AnalysisTypeCode = analysisTypeCode;
                        newRow.AccountCode = currentAccountCode;

                        FMainDS.ATransAnalAttrib.Rows.InsertAt(newRow, FMainDS.ATransAnalAttrib.Rows.Count);
                    }
                    else if (transAnalAttrRow.AccountCode != currentAccountCode)
                    {
                        transAnalAttrRow.AccountCode = currentAccountCode;
                    }
                }
            }
        }

        /// <summary>
        /// clear the current selection
        /// </summary>
        public void ClearCurrentSelection()
        {
            this.FPreviouslySelectedDetailRow = null;
        }

        /// <summary>
        /// if the value changes check if the new value is active
        /// </summary>
        private void ValueChanged(object sender, EventArgs e)
        {
            Object selectedItem = cmbDetailAnalysisAttributeValue.SelectedItem;
            int selectedIndex = cmbDetailAnalysisAttributeValue.SelectedIndex;

            if ((selectedIndex < 0) || (selectedItem == null))
            {
                return;
            }

            AFreeformAnalysisRow freeFormAnalRow = null;
            String selectedItemString = selectedItem.ToString();
            bool AccessOk = false;
            try
            {
                Object[] PrimaryKey = new Object[] {
                    FLedgerNumber, txtReadonlyAnalysisTypeCode.Text, selectedItemString
                };
                freeFormAnalRow = (AFreeformAnalysisRow)FCacheDS.AFreeformAnalysis.Rows.Find(PrimaryKey);
                AccessOk = true;
            }
            catch (Exception)
            {
                cmbDetailAnalysisAttributeValue.ForeColor = System.Drawing.Color.Red;
            }

            if (AccessOk)
            {
                if (freeFormAnalRow.Active)
                {
                    cmbDetailAnalysisAttributeValue.ForeColor = System.Drawing.Color.Black;
                    cmbDetailAnalysisAttributeValue.Font = System.Windows.Forms.Control.DefaultFont;
                }
                else
                {
                    cmbDetailAnalysisAttributeValue.ForeColor = System.Drawing.Color.Gray;
                    cmbDetailAnalysisAttributeValue.Font = new System.Drawing.Font(System.Windows.Forms.Control.DefaultFont,
                        System.Drawing.FontStyle.Strikeout);
                }
            }
        }

        /// <summary>
        /// reset the fonts on dropdown
        /// </summary>
        private void DropDown(object sender, EventArgs e)
        {
            cmbDetailAnalysisAttributeValue.ForeColor = System.Drawing.Color.Black;
            cmbDetailAnalysisAttributeValue.Font = System.Windows.Forms.Control.DefaultFont;
        }
    }
}