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
                checkFCacheInitialised(); return FCacheDS;
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
            if (FBatchNumber != -1)
            {
                GetDataFromControls();
            }

            FLedgerNumber = ALedgerNumber;
            FBatchNumber = ABatchNumber;
            FJournalNumber = AJournalNumber;
            FTransactionNumber = ATransactionNumber;
            FPreviouslySelectedDetailRow = null;

            DataView view = new DataView(FMainDS.ATransAnalAttrib);
            view.RowStateFilter = DataViewRowState.CurrentRows | DataViewRowState.Deleted;

            // only load from server if there are no attributes loaded yet for this journal
            // otherwise we would overwrite attributes that have already been modified
            view.Sort = StringHelper.StrMerge(TTypedDataTable.GetPrimaryKeyColumnStringList(ATransactionTable.TableId), ',');

            if (view.Find(new object[] { FLedgerNumber, FBatchNumber, FJournalNumber, FTransactionNumber }) == -1)
            {
                FMainDS.Merge(TRemote.MFinance.GL.WebConnectors.LoadATransAnalAttrib(ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber));
            }

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            // bool ActiveOnly = this.Enabled;
            // TFinanceControls.InitialiseValuesList(ref cmbDetailAccountCode, FLedgerNumber, true, false, ActiveOnly, false);

            // do no longer call ShowData but instead use the code below
            //ShowData();

            // the following code represents what usually happens inside ShowData. Need manual code here since
            // the view does not show all records but only filtered one (filter by transaction)
            FPetraUtilsObject.DisableDataChangedEvent();
            pnlDetails.Enabled = false;
            ShowDataManual();

            if (FMainDS.ATransAnalAttrib != null)
            {
                // set a row filter to make sure only records belonging to this transaction are shown
                view.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}={7}",
                    ATransAnalAttribTable.GetLedgerNumberDBName(), FLedgerNumber,
                    ATransAnalAttribTable.GetBatchNumberDBName(), FBatchNumber,
                    ATransAnalAttribTable.GetJournalNumberDBName(), FJournalNumber,
                    ATransAnalAttribTable.GetTransactionNumberDBName(), FTransactionNumber);
                view.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(view);

                if (view.Count > 0)
                {
                    SelectRowInGrid(1);
                    pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
                }
                else
                {
//                    show
                }
            }

            FPetraUtilsObject.EnableDataChangedEvent();
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

            checkFCacheInitialised();
        }

        void checkFCacheInitialised()
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
                    if (AFRow.Active || ARow.AnalysisAttributeValue == AFRow.AnalysisValue)
                    {
                        cmbDetailAnalysisAttributeValue.Items.Add(AFRow.AnalysisValue);
                    }
                }
            }

            txtReadonlyAnalysisTypeCode.Text = ARow.AnalysisTypeCode;
            AAnalysisTypeRow ATRow = (AAnalysisTypeRow)FCacheDS.AAnalysisType.Rows.Find(new Object[] { ARow.AnalysisTypeCode });
            txtReadonlyDescription.Text = ATRow.AnalysisTypeDescription;

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
        /// check if the necessary rows for the given account are there, automatically add/delete rows, update account in my table
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <param name="AAccount">Account Number for AnalysisTable lookup</param>
        public void CheckAnalysisAttributes(int ALedgerNumber, int ABatchNumber, int AJournalNumber,
            int ATransactionNumber, String AAccount)
        {
            //grdDetails
            checkFCacheInitialised();

            if (FCacheDS == null)
            {
                return;
            }

            if (FPetraUtilsObject.SuppressChangeDetection)
            {
                return;
            }

            DataView ANView = FCacheDS.AAnalysisAttribute.DefaultView;
            ANView.RowFilter = String.Format("{0} = '{1}'",
                AAnalysisAttributeTable.GetAccountCodeDBName(),
                AAccount);
            int i = 0;

            while (i < ANView.Count)
            {
                AAnalysisAttributeRow myRow = (AAnalysisAttributeRow)ANView[i].Row;
                String TypeCode = myRow.AnalysisTypeCode;

                ATransAnalAttribRow myTableRow =
                    (ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber,
                                                                                           ATransactionNumber,
                                                                                           TypeCode });

                if (myTableRow == null)
                {
                    //Create a new TypeCode for this account
                    ATransAnalAttribRow newRow = FMainDS.ATransAnalAttrib.NewRowTyped(true);
                    newRow.LedgerNumber = ALedgerNumber;
                    newRow.BatchNumber = ABatchNumber;
                    newRow.JournalNumber = AJournalNumber;
                    newRow.TransactionNumber = ATransactionNumber;
                    newRow.AnalysisTypeCode = TypeCode;
                    newRow.AccountCode = AAccount;

                    FMainDS.ATransAnalAttrib.Rows.InsertAt(newRow, FMainDS.ATransAnalAttrib.Rows.Count);
                    //Debug.WriteLine("Produced TransAnalAttrib with tn:"+FTransactionNumber+" and Ac:"+ AAccount);
                    FPetraUtilsObject.SetChangedFlag();
                    //grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ATransAnalAttrib.DefaultView);
                    grdDetails.Refresh();

                    SelectDetailRowByDataTableIndex(FMainDS.ATransAnalAttrib.Rows.Count - 1);
                }
                else
                {
                    myTableRow.AccountCode = AAccount;
                }

                i++;
            }

            // search for unnecessary elements
            i = FMainDS.ATransAnalAttrib.Rows.Count - 1;

            while (i >= 0)
            {
                ATransAnalAttribRow checkRow = (ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows[i];

                if ((checkRow.RowState != DataRowState.Deleted)
                    && (checkRow.TransactionNumber == FTransactionNumber)
                    && (checkRow.BatchNumber == FBatchNumber)
                    && (checkRow.LedgerNumber == FLedgerNumber))
                {
                    AAnalysisAttributeRow attRow =
                        (AAnalysisAttributeRow)FCacheDS.AAnalysisAttribute.Rows.Find(new Object[] { checkRow.LedgerNumber, checkRow.AnalysisTypeCode,
                                                                                                    AAccount });

                    if (!checkRow.AccountCode.Equals(AAccount) || (attRow == null))
                    {
                        int rowIndex = grdDetails.Selection.GetSelectionRegion().GetRowsIndex()[0];

                        if (checkRow.RowState == DataRowState.Added)
                        {
                            //Debug.WriteLine("Removed row at#"+i+" with tn:"+FTransactionNumber+" and Ac:"+ AAccount);
                            FMainDS.ATransAnalAttrib.Rows.RemoveAt(i);
                        }
                        else
                        {
                            //Debug.WriteLine("Deleted row at#"+i+" with tn:"+FTransactionNumber+" and Ac:"+ AAccount);
                            checkRow.Delete();
                        }

                        if (rowIndex == grdDetails.Rows.Count)
                        {
                            rowIndex--;
                        }

                        if (grdDetails.Rows.Count > 1)
                        {
                            grdDetails.Selection.SelectRow(rowIndex, true);
                            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                            ShowDetails(FPreviouslySelectedDetailRow);
                        }
                        else
                        {
                            FPreviouslySelectedDetailRow = null;
                        }
                    }
                }

                i--;
            }
        }

        /// <summary>
        /// remove all attributes if the transaction is deleted (needed?))
        /// </summary>
        /// <param name="trans">Row of the transaction where the attributes are deleted</param>

        public void DeleteTransactionAttributes(GLBatchTDSATransactionRow trans)
        {
            if (trans == null)
            {
                return;
            }

            int NumRows = FMainDS.ATransAnalAttrib.Rows.Count;

            for (int RowIndex = NumRows - 1; RowIndex >= 0; RowIndex -= 1)
            {
                ATransAnalAttribRow row = (ATransAnalAttribRow)FMainDS.ATransAnalAttrib.Rows[RowIndex];

                if (
                    row.TransactionNumber.Equals(trans.TransactionNumber)
                    && row.JournalNumber.Equals(trans.JournalNumber)
                    && row.BatchNumber.Equals(trans.BatchNumber)
                    && row.LedgerNumber.Equals(trans.LedgerNumber)

                    )
                {
                    row.Delete();
                }
            }

            FPreviouslySelectedDetailRow = null;

            FPetraUtilsObject.SetChangedFlag();
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
            Object sio = cmbDetailAnalysisAttributeValue.SelectedItem;
            int si = cmbDetailAnalysisAttributeValue.SelectedIndex;

            if ((si < 0) || (sio == null))
            {
                return;
            }

            AFreeformAnalysisRow afaRow = null;
            String v = sio.ToString();
            bool AccessOk = false;
            try
            {
                Object[] PrimaryKey = new Object[] {
                    FLedgerNumber, txtReadonlyAnalysisTypeCode.Text, v
                };
                afaRow = (AFreeformAnalysisRow)FCacheDS.AFreeformAnalysis.Rows.Find(PrimaryKey);
                AccessOk = true;
            }
            catch (Exception)
            {
                cmbDetailAnalysisAttributeValue.ForeColor = System.Drawing.Color.Red;
            }

            if (AccessOk)
            {
                if (afaRow.Active)
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