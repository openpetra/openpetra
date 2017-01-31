//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
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
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmLocalDataFieldOrderSetup
    {
        // Instance of a 'Helper Class' for handling the Indexes of the DataRows. (The Grid is sorted by the Index.)
        TSgrdDataGrid.IndexedGridRowsHelper FIndexedGridRowsHelper;

        // This is the extra dataset that we need that gives us the DataLabel information
        private class FExtraDS
        {
            public static PDataLabelTable PDataLabel;
        }

        /// <summary>
        /// The Context in which we have been launched, eg Family, Church, Personnel etc
        /// </summary>
        public string Context {
            get; set;
        }

        private void RunOnceOnActivationManual()
        {
            // Set up the window title
            if (String.Compare(Context, "Bank", true) == 0)
            {
                this.Text += Catalog.GetString(" For Bank");
            }
            else if (String.Compare(Context, "Church", true) == 0)
            {
                this.Text += Catalog.GetString(" For Church");
            }
            else if (String.Compare(Context, "Family", true) == 0)
            {
                this.Text += Catalog.GetString(" For Family");
            }
            else if (String.Compare(Context, "Organisation", true) == 0)
            {
                this.Text += Catalog.GetString(" For Organisation");
            }
            else if (String.Compare(Context, "Person", true) == 0)
            {
                this.Text += Catalog.GetString(" For Person");
            }
            else if (String.Compare(Context, "Unit", true) == 0)
            {
                this.Text += Catalog.GetString(" For Unit");
            }
            else if (String.Compare(Context, "Venue", true) == 0)
            {
                this.Text += Catalog.GetString(" For Venue");
            }
            else if (String.Compare(Context, "LongTermApp", true) == 0)
            {
                this.Text += Catalog.GetString(" For Long Term Applications");
            }
            else if (String.Compare(Context, "ShortTermApp", true) == 0)
            {
                this.Text += Catalog.GetString(" For Short Term Applications");
            }
            else if (String.Compare(Context, "Personnel", true) == 0)
            {
                this.Text += Catalog.GetString(" For Personnel");
            }

            // Initialize 'Helper Class' for handling the Indexes of the DataRows.
            FIndexedGridRowsHelper = new TSgrdDataGrid.IndexedGridRowsHelper(
                grdDetails, PDataLabelUseTable.ColumnIdx1Id, btnDemote, btnPromote,
                delegate { FPetraUtilsObject.SetChangedFlag(); });

            // Load the Extra Data from DataLabel table
            Type DataTableType;
            FExtraDS.PDataLabel = new PDataLabelTable();
            DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("DataLabelList", String.Empty, null, out DataTableType);
            FExtraDS.PDataLabel.Merge(CacheDT);

            // Extend our main DataLabelUse table
            int NameOrdinal = FMainDS.PDataLabelUse.Columns.Add("Name", typeof(String)).Ordinal;
            int GroupOrdinal = FMainDS.PDataLabelUse.Columns.Add("GroupHeading", typeof(String)).Ordinal;
            int DescriptionOrdinal = FMainDS.PDataLabelUse.Columns.Add("Description", typeof(String)).Ordinal;

            // Take each row of our main dataset and populate the new columns with relevant data
            //   from the DataLabelUse table
            foreach (PDataLabelUseRow useRow in FMainDS.PDataLabelUse.Rows)
            {
                PDataLabelRow labelRow = (PDataLabelRow)FExtraDS.PDataLabel.Rows.Find(new object[] { useRow.DataLabelKey });
                useRow[NameOrdinal] = labelRow.Text;
                useRow[GroupOrdinal] = labelRow.Group;
                useRow[DescriptionOrdinal] = labelRow.Description;
            }

            // Add columns to the grid for the label details
            grdDetails.AddTextColumn(Catalog.GetString("Name"), FMainDS.PDataLabelUse.Columns[NameOrdinal]);
            grdDetails.AddTextColumn(Catalog.GetString("Group Heading"), FMainDS.PDataLabelUse.Columns[GroupOrdinal]);
            grdDetails.AddTextColumn(Catalog.GetString("Description"), FMainDS.PDataLabelUse.Columns[DescriptionOrdinal]);
            grdDetails.Selection.SelectionChanged += HandleSelectionChanged;

            // Remove the first column.  We added this in the YAML so that the auto-generator had something to do
            grdDetails.Columns.Remove(0);
            grdDetails.SetHeaderTooltip(0, Catalog.GetString("Name"));
            grdDetails.SetHeaderTooltip(1, Catalog.GetString("Group Heading"));
            grdDetails.SetHeaderTooltip(2, Catalog.GetString("Description"));

            // Create a view that will only show the rows applicable to our currentContext
            DataView contextView = new DataView(FMainDS.PDataLabelUse, "p_use_c='" + Context + "'", "p_idx1_i", DataViewRowState.CurrentRows);
            contextView.AllowNew = false;

            // Bind the view to our grid
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(contextView);
            grdDetails.Refresh();

            grdDetails.AutoResizeGrid();
            SelectRowInGrid(1);
        }

        private PDataLabelUseRow FPreviouslySelectedDetailRow = null;

        void HandleSelectionChanged(object sender, SourceGrid.RangeRegionChangedEventArgs e)
        {
            FIndexedGridRowsHelper.UpdateButtons(grdDetails.Selection.ActivePosition.Row, FPetraUtilsObject.SecurityReadOnly);
        }

        private void DataFieldPromote(System.Object sender, System.EventArgs e)
        {
            FIndexedGridRowsHelper.PromoteRow();
        }

        private void DataFieldDemote(System.Object sender, System.EventArgs e)
        {
            FIndexedGridRowsHelper.DemoteRow();
        }

        // These four methods are not used because we do not have a details panel beneath the grid
        // However, they need to be here to get the auto-gen code to compile
        private void NewRowManual(ref PDataLabelUseRow ARow)
        {
            // Dummy use of FIgnoreFocusRowLeaving to make things compile
            if (FIgnoreFocusRowLeaving)
            {
                ShowDetails();
            }
        }

        private void ShowDetails()
        {
        }

        private void ShowDetails(object o)
        {
        }

        private void GetDetailsFromControls(PDataLabelUseRow ARow, bool AIsNewRow = false)
        {
        }

        private int FPrevRowChangedRow = -1;
        private bool FIgnoreFocusRowLeaving = false;

        #region Security

        private List <string>ApplySecurityManual()
        {
            List <string>ReturnValue = new List <string>();

            switch (FPetraUtilsObject.SecurityScreenContext)
            {
                case "Bank":
                case "Church":
                case "Family":
                case "Person":
                case "Organisation":
                case "Unit":
                case "Venue":
                    FPetraUtilsObject.SecurityScreenContext = "MPartner";

                    break;

                case "Personnel":
                case "LongTermApp":
                case "ShortTermApp":
                    FPetraUtilsObject.SecurityScreenContext = "MPersonnel";

                    break;
            }

            return ReturnValue;
        }

        #endregion

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFormDataKeyDescriptionList recordList = new TFormDataKeyDescriptionList();
            DataView FieldOrderView = new DataView(FMainDS.PDataLabelUse);

            FieldOrderView.RowFilter = String.Format("p_use_c = '{0}'", Context);
            recordList.Title = "Local Data Field Order For " + Context;

            String Key = Catalog.GetString("Name");
            String Descr = Catalog.GetString("GroupHeading");
            String Field3 = Catalog.GetString("Description");

            recordList.KeyTitle = Key;
            recordList.DescriptionTitle = Descr;
            recordList.Field3Title = Field3;

            foreach (DataRowView FieldOrderViewRow in FieldOrderView)
            {
                TFormDataKeyDescription record = new TFormDataKeyDescription();

                record.Key = FieldOrderViewRow[Key].ToString();
                record.Description = FieldOrderViewRow[Descr].ToString();
                record.Field3 = FieldOrderViewRow[Field3].ToString();
                recordList.Add(record);
            }

            TStandardFormPrint.PrintRecordList(recordList,
                3,
                APrintApplication,
                FieldOrderView.Count,
                FieldOrderView,
                "",
                APreviewMode);
        }
    }
}