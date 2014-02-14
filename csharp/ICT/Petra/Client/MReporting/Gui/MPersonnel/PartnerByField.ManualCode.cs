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
            this.LoadListData("");
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

        private void LoadListData(string AFilter)
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PUnitTable.GetPartnerKeyDBName();
            string DisplayMember = PUnitTable.GetUnitNameDBName();
            PUnitTable Table;

            Table = TRemote.MPartner.Partner.WebConnectors.GetActiveFieldUnits(AFilter);

            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbField.Columns.Clear();
            clbField.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbField.AddTextColumn(Catalog.GetString("Field Name"), NewTable.Columns[DisplayMember], 240);
            clbField.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 100);

            clbField.DataBindGrid(NewTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbField.SetCheckedStringList("");
        }

        private void FilterList(System.Object sender, EventArgs e)
        {
            LoadListData(txtFilter.Text);
        }

        private void FieldDatesSelectionChanged(System.Object sender, EventArgs e)
        {
            // when date range is selected then only commitments can be considered
            if (rbtDateRange.Checked)
            {
                rbtCommitmentsOnly.Checked = true;
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbField.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one Field"),
                    Catalog.GetString("Please select at least one Field as otherwise the Extract cannot be created!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (rbtNow.Checked)
            {
                ACalc.AddParameter("param_from_date", DateTime.Today.Date);
                ACalc.AddParameter("param_until_date", DateTime.Today.Date);
            }
        }
    }
}