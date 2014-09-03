// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//   awebster
//
// Copyright 2004-2013 by OM International
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
// generateNamespaceMap-Link-Extra-DLL System.Data.DataSetExtensions;
using System.Linq;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Owf.Controls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Common.Verification;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByRelationship
    {
        private DataTable relationshipTable;
        private string CheckedMember = "CHECKED";
        private string DisplayMember = PRelationTable.GetRelationDescriptionDBName();
        private string ReciprocalDisplayMember = PRelationTable.GetReciprocalDescriptionDBName();
        private string RelationshipCategory = PRelationTable.GetRelationCategoryDBName();
        private string ValueMember = PRelationTable.GetRelationNameDBName();
        
        private void InitializeManualCode()
        {
            DataTable table = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.RelationList);
            DataView view = new DataView(table);

            view.Sort = ValueMember;

            relationshipTable = view.ToTable(true, new string[] { ValueMember, ReciprocalDisplayMember, DisplayMember, RelationshipCategory });
            relationshipTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
            
            // Relationship selection
            clbIncludeRelationships.SpecialKeys = (SourceGrid.GridSpecialKeys)(
                SourceGrid.GridSpecialKeys.Arrows |
                SourceGrid.GridSpecialKeys.PageDownUp |
                SourceGrid.GridSpecialKeys.Enter |
                SourceGrid.GridSpecialKeys.Escape |
                SourceGrid.GridSpecialKeys.Control | 
                SourceGrid.GridSpecialKeys.Shift);
            clbIncludeRelationships.Columns.Clear();
            clbIncludeRelationships.AddCheckBoxColumn("", relationshipTable.Columns[CheckedMember], 17, false);
            clbIncludeRelationships.AddTextColumn(Catalog.GetString("Relation Name"), relationshipTable.Columns[ValueMember], 100);
            clbIncludeRelationships.AddTextColumn(Catalog.GetString("Description"), relationshipTable.Columns[DisplayMember], 320);
            clbIncludeRelationships.DataBindGrid(relationshipTable, ValueMember, CheckedMember, ValueMember, false, true, false);

            // Reciprocal Relationship selection
            clbIncludeReciprocalRelationships.SpecialKeys = (SourceGrid.GridSpecialKeys)(
                SourceGrid.GridSpecialKeys.Arrows |
                SourceGrid.GridSpecialKeys.PageDownUp |
                SourceGrid.GridSpecialKeys.Enter |
                SourceGrid.GridSpecialKeys.Escape |
                SourceGrid.GridSpecialKeys.Control |
                SourceGrid.GridSpecialKeys.Shift);
            clbIncludeReciprocalRelationships.Columns.Clear();
            clbIncludeReciprocalRelationships.AddCheckBoxColumn("", relationshipTable.Columns[CheckedMember], 17, false);
            clbIncludeReciprocalRelationships.AddTextColumn(Catalog.GetString("Relation Name"), relationshipTable.Columns[ValueMember], 100);
            clbIncludeReciprocalRelationships.AddTextColumn(Catalog.GetString("Reciprocal Description"), relationshipTable.Columns[ReciprocalDisplayMember], 320);
            clbIncludeReciprocalRelationships.DataBindGrid(relationshipTable, ValueMember, CheckedMember, ValueMember, false, true, false);

            // Hide unwanted controls
            ucoChkFilter.ShowFamiliesOnly(false);
            ucoPartnerSelection.ShowAllStaffOption(false);
            ucoPartnerSelection.ShowCurrentStaffOption(false);
        }

        private void OnRelationshipFilterChanged(object sender, EventArgs e)
        {
            
            if (relationshipTable != null)
            {
                var newTable = relationshipTable.AsEnumerable()
                    .Where(r => r[PRelationTable.GetRelationCategoryDBName()].ToString() == cmbRelationshipType.GetSelectedString());
                if (newTable.Any())
                {
                    clbIncludeRelationships.DataBindGrid(newTable.CopyToDataTable(), ValueMember, CheckedMember, ValueMember, false, true, false);
                    clbIncludeReciprocalRelationships.DataBindGrid(newTable.CopyToDataTable(), ValueMember, CheckedMember, ValueMember, false, true, false);
                }
                else // if the filter would produce 0 results, show everything instead
                { 
                    clbIncludeRelationships.DataBindGrid(relationshipTable, ValueMember, CheckedMember, ValueMember, false, true, false);
                    clbIncludeReciprocalRelationships.DataBindGrid(relationshipTable, ValueMember, CheckedMember, ValueMember, false, true, false);
                }
            }
        }


        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbIncludeRelationships.AutoFindColumn = ((Int16)(1));
            this.clbIncludeRelationships.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.clbIncludeReciprocalRelationships.AutoFindColumn = ((Int16)(1));
            this.clbIncludeReciprocalRelationships.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbIncludeRelationships.GetCheckedStringList().Length == 0 && clbIncludeReciprocalRelationships.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one relationship type"),
                    Catalog.GetString("Please select at least one relationship type."),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

    }
}