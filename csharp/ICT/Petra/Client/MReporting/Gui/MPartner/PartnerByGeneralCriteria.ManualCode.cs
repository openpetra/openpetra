//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Common;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByGeneralCriteria
    {
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

            ucoChkFilter.ShowFamiliesOnly(false);
            ucoAddress.ShowCountyStateField(true);
            ucoAddress.ShowAddressDateFields(true);

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbLocationType.AutoFindColumn = ((Int16)(1));
            this.clbLocationType.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbLocationType.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadListData();

            // make sure date fields are not initialized with today's date but later on with default settings
            dtpCreatedFrom.Text = "";
            dtpCreatedTo.Text = "";
            dtpModifiedFrom.Text = "";
            dtpModifiedTo.Text = "";

            FPetraUtilsObject.LoadDefaultSettings();
        }

        private void LoadListData()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PLocationTypeTable.GetCodeDBName();
            PLocationTypeTable Table;

            Table = (PLocationTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.LocationTypeList);

            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbLocationType.Columns.Clear();
            clbLocationType.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbLocationType.AddTextColumn("", NewTable.Columns[ValueMember], 130);

            clbLocationType.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, ValueMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbLocationType.SetCheckedStringList("");
        }

        private void OnPartnerClassChanged(object sender, EventArgs e)
        {
            if (cmbPartnerClass.GetSelectedString() == "ORGANISATION")
            {
                lblDenomination.Visible = false;
                cmbDenomination.Visible = false;
                cmbDenomination.SetSelectedString("", -1);

                lblBusiness.Visible = true;
                cmbBusiness.Visible = true;
            }
            else if (cmbPartnerClass.GetSelectedString() == "CHURCH")
            {
                lblBusiness.Visible = false;
                cmbBusiness.Visible = false;
                cmbBusiness.SetSelectedString("", -1);

                lblDenomination.Visible = true;
                cmbDenomination.Visible = true;
            }
            else
            {
                lblBusiness.Visible = false;
                cmbBusiness.Visible = false;
                cmbBusiness.SetSelectedString("", -1);

                lblDenomination.Visible = false;
                cmbDenomination.Visible = false;
                cmbDenomination.SetSelectedString("", -1);
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // add verification here if needed
        }
    }
}