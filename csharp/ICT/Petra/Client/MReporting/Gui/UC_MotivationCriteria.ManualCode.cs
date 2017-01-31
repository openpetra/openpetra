//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2016 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of TFrmUC_AddressFilter
    /// </summary>
    public partial class TFrmUC_MotivationCriteria
    {
        /// Indicator if settings are currently loaded. Initially set to true until after initial settings are loaded.
        private bool FDuringLoadSettings = true;

        /*private String FCheckedMotGroupStringList = "";
         * private String FCheckedMotDetailStringList = "";*/

        private Int32 FLedgerNumber = -1;

        private TParameterList FParameters;
        private DataTable FMotDetailTable;

        private TFrmSelectMotivationCriteriaDialog FSelectMotDialog;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
            }
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void InitializeManualCode()
        {
        }

        /// <summary>
        /// Return true if any motivation detail code is selected
        /// </summary>
        public bool IsAnyMotivationDetailSelected()
        {
            return true;
            //return clbMotivationDetail.CheckedItemsCount > 0;
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            FParameters = AParameters;

            /*
             * clbMotivationGroup.SetCheckedStringList(AParameters.Get("param_motivation_group").ToString());
             * clbMotivationDetail.SetCheckedStringList(AParameters.Get("param_motivation_detail").ToString());
             */
        }

        /// <summary>
        /// This will add functions to the list of available functions
        /// </summary>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            LedgerNumber = TLstTasks.CurrentLedger;
        }

        /// <summary>
        /// initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            // set the delegate that is needed before and after LoadSettings has been run
            FPetraUtilsObject.DelegateLoadSettingsStarting = @LoadSettingsStarting;
            FPetraUtilsObject.DelegateLoadSettingsFinished = @LoadSettingsFinished;
        }

        private void LoadSettingsStarting()
        {
            /* Make sure that event handler does not cause server calls to constantly reload
             * motivation detail list while motivation group list is filled */
            FDuringLoadSettings = true;
        }

        private void LoadSettingsFinished(TParameterList AParameters)
        {
            FParameters = AParameters;
            // currently relies on fact that always when this screen is opened some settings are loaded
            FDuringLoadSettings = false;

            FSelectMotDialog = new TFrmSelectMotivationCriteriaDialog(this.ParentForm);
            //FSelectMotDialog.FParameters = FParameters;
            FSelectMotDialog.LedgerNumber = FLedgerNumber;
            FSelectMotDialog.LoadSettingsFinished(FParameters);

            FMotDetailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, FLedgerNumber);
            RefreshMotivationDetailList();

            SetGroupBoxLabel();
        }

        /// <summary>
        /// Shows the chkShowDetailedMotivationInformation checkbox
        /// </summary>
        public void ShowchkShowDetailedMotivationInformation()
        {
            chkShowDetailedMotivationInformation.Visible = true;
        }

        private void RefreshMotivationDetailList()
        {
            // don't do anything unless screen is activated or settings are loaded
            if (FDuringLoadSettings)
            {
                return;
            }

            string Value1MemberMotDetail = AMotivationDetailTable.GetMotivationDetailCodeDBName();
            string Value2MemberMotDetail = AMotivationDetailTable.GetMotivationGroupCodeDBName();
            string DisplayMemberMotDetail = AMotivationDetailTable.GetMotivationDetailDescDBName();
            //List<String> KeyColumnList = new List<String>();
            //KeyColumnList.Add(Value1MemberMotDetail);
            //KeyColumnList.Add(Value2MemberMotDetail);
            string CheckedMotivationDetails = FSelectMotDialog.GetDetailsCheckedString();
            string CheckedMotivationGroups = FSelectMotDialog.GetGroupsCheckedString();
            string Filter = "";


            DataView MotDetailView = new DataView(FMotDetailTable);

            MotDetailView.Sort = Value2MemberMotDetail + "," + Value1MemberMotDetail;

            if (CheckedMotivationDetails.Length == 0)
            {
                Filter = Value1MemberMotDetail + " = ''";
            }
            else
            {
                while (CheckedMotivationDetails.Length > 0)
                {
                    if (Filter.Length > 0)
                    {
                        Filter = Filter + " OR ";
                    }

                    Filter += " (" + Value2MemberMotDetail + " = '" + StringHelper.GetNextCSV(ref CheckedMotivationDetails) + "'";
                    Filter += " AND " + Value1MemberMotDetail + " = '" + StringHelper.GetNextCSV(ref CheckedMotivationDetails) + "') ";
                }
            }

            MotDetailView.RowFilter = Filter;

            DataTable NewMotDetailTable = MotDetailView.ToTable(true,
                new string[] {  Value2MemberMotDetail, Value1MemberMotDetail, DisplayMemberMotDetail });

            grdMotivationSelection.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            grdMotivationSelection.Columns.Clear();
            grdMotivationSelection.AddTextColumn(Catalog.GetString("Group Code"), NewMotDetailTable.Columns[Value2MemberMotDetail]);
            grdMotivationSelection.AddTextColumn(Catalog.GetString("Detail Code"), NewMotDetailTable.Columns[Value1MemberMotDetail]);
            grdMotivationSelection.AddTextColumn(Catalog.GetString("Description"), NewMotDetailTable.Columns[DisplayMemberMotDetail]);
            DataView tempDataView = NewMotDetailTable.DefaultView;
            tempDataView.AllowNew = false;
            grdMotivationSelection.DataSource = new DevAge.ComponentModel.BoundDataView(tempDataView);

            grdMotivationSelection.AutoResizeGrid();
        }

        /// <summary>
        /// Reads the selected values from the controls, and stores them into the parameter system of FCalculator
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (chkShowDetailedMotivationInformation.Checked)
            {
                ACalc.AddParameter("param_chkShowDetailedMotivationInformation", true);
            }
            else
            {
                ACalc.AddParameter("param_chkShowDetailedMotivationInformation", false);
            }

            FSelectMotDialog.ReadControls(ACalc, AReportAction);
        }

        private void OpenDialog(System.Object sender, EventArgs e)
        {
            FSelectMotDialog.ShowDialog(this);

            if (FSelectMotDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                RefreshMotivationDetailList();
                SetGroupBoxLabel();
            }
        }

        private void SetGroupBoxLabel()
        {
            string sum = "0";

            if (grdMotivationSelection.Rows.Count > FSelectMotDialog.GetTotalDetailsCount())
            {
                sum = "all";
            }
            else
            {
                sum = (grdMotivationSelection.Rows.Count - 1).ToString();
            }

            grpMotivationCriteria.Text = Catalog.GetString("Motivation Criteria ") + String.Format("[{0}]", sum);
        }
    }
}