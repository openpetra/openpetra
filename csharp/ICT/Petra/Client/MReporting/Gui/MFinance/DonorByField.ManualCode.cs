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
            this.LoadListData("");

            // allow 'any' selection for receipt frequency
            DataRow emptyRow = cmbReceiptLetterFrequency.Table.NewRow();
            emptyRow[0] = string.Empty;
            emptyRow[1] = Catalog.GetString("Any Frequency");
            cmbReceiptLetterFrequency.Table.Rows.Add(emptyRow);
        }

        private void LoadListData(string AFilter)
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PUnitTable.GetPartnerKeyDBName();
            string DisplayMember = PUnitTable.GetUnitNameDBName();
            PUnitTable Table;

            // retrieve data from server
            Table = TRemote.MPartner.Partner.WebConnectors.GetLedgerUnits(AFilter);

            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbLedger.Columns.Clear();
            clbLedger.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbLedger.AddTextColumn(Catalog.GetString("Ledger Name"), NewTable.Columns[DisplayMember], 240);
            clbLedger.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 100);

            clbLedger.DataBindGrid(NewTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            clbLedger.SetCheckedStringList("");
        }

        private void FilterList(System.Object sender, EventArgs e)
        {
            LoadListData(txtFilter.Text);
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbLedger.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one Field"),
                    Catalog.GetString("Please select at least one Field, to avoid listing the whole database!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }
    }
}