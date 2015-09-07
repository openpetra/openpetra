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


namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    public partial class TFrmPartnerByField
    {
        private DataTable FFieldTable;

        private void InitializeManualCode()
        {
            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbField.AutoFindColumn = ((Int16)(1));
            this.clbField.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbField.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadListData();

            // TODO: At the moment this group box is not needed as we currently only consider commitments. "Worker Field" does not exist
            // any longer. If in the future "Gift Destination" (as replacement for "Worker Field") for this extract becomes important
            // then this group box needs to be modified and displayed accordingly.
            rbtCommitmentsOnly.Checked = true;
            rgrCommitmentsOnly.Visible = false;

            // catch enter key when using text box
            txtFilterFields.KeyDown += txtFilterFields_KeyDown;

            // manually fix tab order
            pnlMiddle.TabIndex = 1;
            pnlFilter.TabIndex = 0;
            this.ActiveControl = txtFilterFields;
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // no columns tab needed if called from extracts
            if (CalledFromExtracts)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
            }
        }

        private void LoadListData()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PUnitTable.GetPartnerKeyDBName();
            string DisplayMember = PUnitTable.GetUnitNameDBName();
            PUnitTable Table;

            Table = TRemote.MPartner.Partner.WebConnectors.GetActiveFieldUnits();

            DataView view = new DataView(Table);
            FFieldTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            FFieldTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbField.Columns.Clear();
            clbField.AddCheckBoxColumn("", FFieldTable.Columns[CheckedMember], 17, false);
            clbField.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns[DisplayMember]);
            clbField.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), FFieldTable.Columns[ValueMember]);

            clbField.DataBindGrid(FFieldTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbField.SetCheckedStringList("");
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

            clbField.DataBindGrid(view.ToTable(), PUnitTable.GetUnitNameDBName(), "CHECKED", PUnitTable.GetPartnerKeyDBName(), false, true, false);
        }

        private void FieldDatesSelectionChanged(System.Object sender, EventArgs e)
        {
            // when date range is selected then only commitments can be considered
            if (rbtDateRange.Checked)
            {
                rbtCommitmentsOnly.Checked = true;
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (rbtNow.Checked)
            {
                ACalc.AddParameter("param_from_date", DateTime.Today.Date);
                ACalc.AddParameter("param_until_date", DateTime.Today.Date);
            }

            if (clbField.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Generate Extract"),
                    Catalog.GetString("Please select at least one Field!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }
    }
}