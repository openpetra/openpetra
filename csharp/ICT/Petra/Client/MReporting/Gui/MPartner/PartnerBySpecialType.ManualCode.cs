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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerBySpecialType
    {
        private void InitializeManualCode()
        {
            string CheckedMember = "CHECKED";
            string DisplayMember = PTypeTable.GetTypeDescriptionDBName();
            string ValueMember = PTypeTable.GetTypeCodeDBName();

            DataTable Table = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.PartnerTypeList);
            DataView view = new DataView(Table);

            // TODO view.RowFilter = only active types?
            view.Sort = ValueMember;

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbIncludeSpecialTypes.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            //clbIncludeSpecialTypes.SelectionMode = SourceGrid.GridSelectionMode.Row;
            clbIncludeSpecialTypes.Columns.Clear();
            clbIncludeSpecialTypes.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbIncludeSpecialTypes.AddTextColumn(Catalog.GetString("Type Code"), NewTable.Columns[ValueMember], 100);
            clbIncludeSpecialTypes.AddTextColumn(Catalog.GetString("Type Description"), NewTable.Columns[DisplayMember], 320);
            clbIncludeSpecialTypes.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
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
                tabReportSettings.Controls.Remove(tpgReportSorting);
            }
            else
            {
                ucoChkFilter.ShowMailingAddressesOnly(false);
            }

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbIncludeSpecialTypes.AutoFindColumn = ((Int16)(1));
            this.clbIncludeSpecialTypes.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbIncludeSpecialTypes.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one special type"),
                    Catalog.GetString("Please select at least one special type, to avoid listing the whole database!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }
    }
}