//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2012 by OM International
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

using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;


namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmDonorByField
    {
        private DataTable FFieldTable;

        private void InitializeManualCode()
        {
            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbLedger.AutoFindColumn = ((Int16)(1));
            this.clbLedger.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbLedger.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadListData();

            // allow 'any' selection for receipt frequency
            DataRow emptyRow = cmbReceiptLetterFrequency.Table.NewRow();
            emptyRow[0] = string.Empty;
            emptyRow[1] = Catalog.GetString("Any Frequency");
            cmbReceiptLetterFrequency.Table.Rows.Add(emptyRow);

            // catch enter key when using text box
            txtFilterFields.KeyDown += txtFilterFields_KeyDown;

            // manually fix tab order
            pnlMiddle.TabIndex = 1;
            pnlFilter.TabIndex = 0;
        }

        private void LoadListData()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PUnitTable.GetPartnerKeyDBName();
            string DisplayMember = PUnitTable.GetUnitNameDBName();

            // retrieve data from server
            PUnitTable UnitTable = TRemote.MPartner.Partner.WebConnectors.GetLedgerUnits();

            DataView view = new DataView(UnitTable);

            FFieldTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            FFieldTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbLedger.Columns.Clear();
            clbLedger.AddCheckBoxColumn("", FFieldTable.Columns[CheckedMember], 17, false);
            clbLedger.AddTextColumn(Catalog.GetString("Ledger Name"), FFieldTable.Columns[DisplayMember]);
            clbLedger.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), FFieldTable.Columns[ValueMember]);

            clbLedger.DataBindGrid(FFieldTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            clbLedger.SetCheckedStringList("");
        }

        private void txtFilterFields_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterList(sender, null);
            }
        }

        private void FilterList(System.Object sender, EventArgs e)
        {
            DataView view = new DataView(FFieldTable);

            view.RowFilter = PUnitTable.GetUnitNameDBName() + " LIKE '" + txtFilterFields.Text + "%'";

            clbLedger.DataBindGrid(view.ToTable(), PUnitTable.GetUnitNameDBName(), "CHECKED", PUnitTable.GetPartnerKeyDBName(), false, true, false);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbLedger.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Generate Extract"),
                    Catalog.GetString("Please select at least one Field!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void ChkAllLedgersChanged(System.Object sender, EventArgs e)
        {
            clbLedger.Enabled = !chkAllLedgers.Checked;
            txtFilterFields.Enabled = !chkAllLedgers.Checked;
            btnApplyFilter.Enabled = !chkAllLedgers.Checked;
        }
    }
}