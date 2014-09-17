//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2012 by OM International
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
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using System.Data;
using Ict.Common.Verification;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TFrmIntranetExportMotivationsDialog
    {
        private GiftBatchTDS FMainDS;
        private Int32 FLedgerNumber;
        private Boolean FExportSettingsChanged;
        private ListBox lstDoExport;
        private ListBox lstDontExport;

        private class RowItemDescr
        {
            private string FDescr;
            private Int32 FRowIdx;
            public RowItemDescr(Int32 RowIdx, string Descr)
            {
                FRowIdx = RowIdx;
                FDescr = Descr;
            }

            public String Descr
            {
                get
                {
                    return FDescr;
                }
            }
            public Int32 RowIdx
            {
                get
                {
                    return FRowIdx;
                }
            }
        }
        private void RunOnceOnActivationManual()
        {
            FExportSettingsChanged = false;

            /* load available ledgers into listbox */
            DataTable LedgerTable;

            TRemote.MFinance.Cacheable.WebConnectors.RefreshCacheableTable(TCacheableFinanceTablesEnum.LedgerNameList, out LedgerTable);

            foreach (DataRow Row in LedgerTable.Rows)
            {
                //
                // I need to establish whether the user is allowed to see this Ledger.
                Int32 LedgerNumber = Convert.ToInt32(Row["LedgerNumber"]);

                if (UserInfo.GUserInfo.IsInLedger(LedgerNumber))
                {
                    String LedgerDescr = String.Format("{0}: {1}", LedgerNumber, Row["LedgerName"]);
                    cmbLedger.Items.Add(LedgerDescr);
                }
            }

            cmbLedger.SelectedIndex = 0;
            EnableAddRemoveButtons(null, null);
        }

        private void InitializeManualCode()
        {
            lstDoExport = new ListBox();
            lstDoExport.Location = new System.Drawing.Point(0, 0);
            lstDoExport.Size = pnlDoExport.Size;
            lstDoExport.SelectionMode = SelectionMode.MultiExtended;
            lstDoExport.SelectedIndexChanged += new System.EventHandler(EnableAddRemoveButtons);
            lstDoExport.DisplayMember = "Descr";
            pnlDoExport.Controls.Add(lstDoExport);

            lstDontExport = new ListBox();
            lstDontExport.Location = new System.Drawing.Point(0, 0);
            lstDontExport.Size = pnlDontExport.Size;
            lstDontExport.SelectionMode = SelectionMode.MultiExtended;
            lstDontExport.SelectedIndexChanged += new System.EventHandler(EnableAddRemoveButtons);
            lstDontExport.DisplayMember = "Descr";
            pnlDontExport.Controls.Add(lstDontExport);
        }

        private void SaveAllChangedRows()
        {
            if (FExportSettingsChanged)
            {
//              TSubmitChangesResult SubmitOK =
                TRemote.MFinance.Gift.WebConnectors.SaveMotivationDetails(ref FMainDS);

                FExportSettingsChanged = false;
            }
        }

        private void OnLedgerChange(Object Sender, EventArgs e)
        {
            String LedgerDescr = (string)cmbLedger.Items[cmbLedger.SelectedIndex];

            FLedgerNumber = Convert.ToInt32(LedgerDescr.Substring(0, LedgerDescr.IndexOf(':')));

            //
            // If I've got changes in the current list, I need to commit that first..
            //
            SaveAllChangedRows();
            FMainDS = TRemote.MFinance.Gift.WebConnectors.LoadMotivationDetails(FLedgerNumber);
            LoadLists();
        }

        private void LoadLists()
        {
            Int32 YPosDo = lstDoExport.TopIndex;
            Int32 YPosDont = lstDontExport.TopIndex;

            lstDoExport.Items.Clear();
            lstDontExport.Items.Clear();
            lstDoExport.BeginUpdate();
            lstDontExport.BeginUpdate();

            for (Int32 RowIdx = 0; RowIdx < FMainDS.AMotivationDetail.Rows.Count; RowIdx++)
            {
                AMotivationDetailRow Row = FMainDS.AMotivationDetail[RowIdx];
                RowItemDescr RowDescr = new RowItemDescr(RowIdx, String.Format("[{0}] {1}", Row.MotivationDetailCode, Row.MotivationDetailDesc));

                if (Row.ExportToIntranet)
                {
                    lstDoExport.Items.Add(RowDescr);
                }
                else
                {
                    lstDontExport.Items.Add(RowDescr);
                }
            }

            lstDoExport.TopIndex = YPosDo;
            lstDontExport.TopIndex = YPosDont;
            lstDoExport.EndUpdate();
            lstDontExport.EndUpdate();
            EnableAddRemoveButtons(null, null);
        }

        private void EnableAddRemoveButtons(Object Sender, EventArgs e)
        {
            btnAdd.Enabled = (lstDontExport.SelectedIndices.Count > 0);
            btnRemove.Enabled = (lstDoExport.SelectedIndices.Count > 0);
        }

        private void AddSelected(Object Sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection Selections = lstDontExport.SelectedIndices;

            foreach (Int32 SelNum in Selections)
            {
                Int32 RowNum = ((RowItemDescr)(lstDontExport.Items[SelNum])).RowIdx;
                FMainDS.AMotivationDetail[RowNum].ExportToIntranet = true;
            }

            LoadLists();
            FExportSettingsChanged = true;
        }

        private void RemoveSelected(Object Sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection Selections = lstDoExport.SelectedIndices;

            foreach (Int32 SelNum in Selections)
            {
                Int32 RowNum = ((RowItemDescr)(lstDoExport.Items[SelNum])).RowIdx;
                FMainDS.AMotivationDetail[RowNum].ExportToIntranet = false;
            }

            LoadLists();
            FExportSettingsChanged = true;
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            SaveAllChangedRows();
            Close();
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            Close();
        }
    }
}