// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2014 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using GNU.Gettext;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_CostCentreList
    {
        private TFrmGLCostCentreHierarchy FParentForm = null;

        // The CostCentre selected in the parent form
        CostCentreNodeDetails FSelectedCostCentre;
        Int32 FLedgerNumber;
        DataView FDataView = null;

        /// <summary>
        /// I don't want this, but the auto-generated code references it:
        /// </summary>
        public GLSetupTDS MainDS;

        /// <summary>
        /// The CostCentre may have been selected in the tree view, and copied here.
        /// </summary>
        public CostCentreNodeDetails SelectedCostCentre
        {
            set
            {
                FSelectedCostCentre = value;

                if (FDataView != null)
                {
                    Int32 RowIdx = -1;

                    if (FSelectedCostCentre != null)
                    {
                        RowIdx = FDataView.Find(FSelectedCostCentre.CostCentreRow.CostCentreCode) + 1;
                    }

                    FParentForm.FIAmUpdating++;
                    grdCostCentres.SelectRowInGrid(RowIdx);
                    FParentForm.FIAmUpdating--;
                }
            }
        }

        /// <summary>
        /// Perform initialisation
        /// (Actually called earlier than the parent RunOnceOnActivationManual)
        /// </summary>
        public void RunOnceOnActivationManual(TFrmGLCostCentreHierarchy ParentForm)
        {
            FParentForm = ParentForm;
            grdCostCentres.Selection.SelectionChanged += Selection_SelectionChanged;
        }

        void Selection_SelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            if (FParentForm.FIAmUpdating == 0)
            {
                Int32 RowIdx = grdCostCentres.Selection.ActivePosition.Row;
                DataRowView rowView = (DataRowView)grdCostCentres.Rows.IndexToDataSourceRow(RowIdx);
                String SelectedCostCentreCode = ((ACostCentreRow)rowView.Row).CostCentreCode;
                FParentForm.SetSelectedCostCentreCode(SelectedCostCentreCode);
            }
        }

        /// <summary>
        /// Show all the data (CostCentre Code and description)
        /// </summary>
        public void PopulateListView(GLSetupTDS MainDS, Int32 LedgerNumber)
        {
            FLedgerNumber = LedgerNumber;

            FDataView = new DataView(MainDS.ACostCentre);
            FDataView.Sort = "a_cost_centre_code_c";
            FDataView.AllowNew = false;
            grdCostCentres.DataSource = new DevAge.ComponentModel.BoundDataView(FDataView);
            grdCostCentres.Columns.Clear();
            grdCostCentres.AddTextColumn("Code", MainDS.ACostCentre.ColumnCostCentreCode);
            grdCostCentres.AddTextColumn("Descr", MainDS.ACostCentre.ColumnCostCentreName);
        }
    }
}