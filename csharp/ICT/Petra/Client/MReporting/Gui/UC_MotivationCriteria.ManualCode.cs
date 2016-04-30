//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2010 by OM International
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

        private String FCheckedMotGroupStringList = "";
        private String FCheckedMotDetailStringList = "";

        private Int32 FLedgerNumber = -1;

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
        /// MoivationGroup control
        /// </summary>
        public Ict.Common.Controls.TClbVersatile ClbMotivationGroup
        {
            get
            {
                return clbMotivationGroup;
            }
        }

        /// <summary>
        /// MotivationDetail control
        /// </summary>
        public Ict.Common.Controls.TClbVersatile ClbMotivationDetail
        {
            get
            {
                return clbMotivationDetail;
            }
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // enable autofind in lists for first character (so the user can press character to find list entry)
            this.clbMotivationGroup.AutoFindColumn = ((Int16)(1));
            this.clbMotivationGroup.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            this.clbMotivationDetail.AutoFindColumn = ((Int16)(1));
            this.clbMotivationDetail.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
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

        private void InitializeMotivationGroupList()
        {
            // don't do anything unless screen is activated or settings are loaded
            if (FDuringLoadSettings)
            {
                return;
            }

            string CheckedMemberMotGroup = "CHECKED";
            string ValueMemberMotGroup = AMotivationGroupTable.GetMotivationGroupCodeDBName();
            string DisplayMemberMotGroup = AMotivationGroupTable.GetMotivationGroupDescriptionDBName();

            DataTable MotGroupTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationGroupList,
                FLedgerNumber);
            DataView MotGroupView = new DataView(MotGroupTable);

            MotGroupView.Sort = ValueMemberMotGroup;

            DataTable NewMotGroupTable = MotGroupView.ToTable(true, new string[] { ValueMemberMotGroup, DisplayMemberMotGroup });
            NewMotGroupTable.Columns.Add(new DataColumn(CheckedMemberMotGroup, typeof(bool)));

            clbMotivationGroup.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            clbMotivationGroup.Columns.Clear();
            clbMotivationGroup.AddCheckBoxColumn("", NewMotGroupTable.Columns[CheckedMemberMotGroup], 17, false);
            clbMotivationGroup.AddTextColumn(Catalog.GetString("Group Code"), NewMotGroupTable.Columns[ValueMemberMotGroup]);
            clbMotivationGroup.AddTextColumn(Catalog.GetString("Description"), NewMotGroupTable.Columns[DisplayMemberMotGroup]);
            clbMotivationGroup.DataBindGrid(NewMotGroupTable,
                ValueMemberMotGroup,
                CheckedMemberMotGroup,
                ValueMemberMotGroup,
                false,
                true,
                false);

            clbMotivationGroup.AutoResizeGrid();
            clbMotivationGroup.AutoStretchColumnsToFitWidth = true;

            // set flag as otherwise InitializeMotivationDetailList will be processed unnecessarily
            FDuringLoadSettings = true;
            clbMotivationGroup.SetCheckedStringList("");
            FDuringLoadSettings = false;
        }

        private void InitializeMotivationDetailList()
        {
            // don't do anything unless screen is activated or settings are loaded
            if (FDuringLoadSettings)
            {
                return;
            }

            // remember checked records
            FCheckedMotDetailStringList = clbMotivationDetail.GetCheckedStringList();

            string CheckedMemberMotDetail = "CHECKED";
            string Value1MemberMotDetail = AMotivationDetailTable.GetMotivationGroupCodeDBName();
            string Value2MemberMotDetail = AMotivationDetailTable.GetMotivationDetailCodeDBName();
            string DisplayMemberMotDetail = AMotivationDetailTable.GetMotivationDetailDescDBName();
            List <String>KeyColumnList = new List <String>();
            KeyColumnList.Add(Value1MemberMotDetail);
            KeyColumnList.Add(Value2MemberMotDetail);
            string CheckedMotivationGroups = clbMotivationGroup.GetCheckedStringList();
            string Filter = "";

            DataTable MotDetailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, FLedgerNumber);
            DataView MotDetailView = new DataView(MotDetailTable);

            MotDetailView.Sort = Value1MemberMotDetail + "," + Value2MemberMotDetail;

            if (CheckedMotivationGroups.Length == 0)
            {
                Filter = Value1MemberMotDetail + " = ''";
            }
            else
            {
                while (CheckedMotivationGroups.Length > 0)
                {
                    if (Filter.Length > 0)
                    {
                        Filter = Filter + " OR ";
                    }

                    Filter = Filter + " " + Value1MemberMotDetail + " = '" + StringHelper.GetNextCSV(ref CheckedMotivationGroups) + "'";
                }
            }

            MotDetailView.RowFilter = Filter;

            DataTable NewMotDetailTable = MotDetailView.ToTable(true,
                new string[] { Value2MemberMotDetail, Value1MemberMotDetail, DisplayMemberMotDetail });
            NewMotDetailTable.Columns.Add(new DataColumn(CheckedMemberMotDetail, typeof(bool)));

            clbMotivationDetail.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            clbMotivationDetail.Columns.Clear();
            clbMotivationDetail.AddCheckBoxColumn("", NewMotDetailTable.Columns[CheckedMemberMotDetail], 17, false);
            clbMotivationDetail.AddTextColumn(Catalog.GetString("Detail Code"), NewMotDetailTable.Columns[Value2MemberMotDetail]);
            clbMotivationDetail.AddTextColumn(Catalog.GetString("Group Code"), NewMotDetailTable.Columns[Value1MemberMotDetail]);
            clbMotivationDetail.AddTextColumn(Catalog.GetString("Description"), NewMotDetailTable.Columns[DisplayMemberMotDetail]);
            clbMotivationDetail.DataBindGrid(NewMotDetailTable, "", CheckedMemberMotDetail, KeyColumnList, false, true, false);

            // reset checked records
            clbMotivationDetail.SetCheckedStringList(FCheckedMotDetailStringList);

            clbMotivationDetail.AutoResizeGrid();
        }

        private void LoadSettingsStarting()
        {
            /* Make sure that event handler does not cause server calls to constantly reload
             * motivation detail list while motivation group list is filled */
            FDuringLoadSettings = true;
            clbMotivationGroup.ValueChanged -= new EventHandler(MotivationGroupColumnChanged);
        }

        private void LoadSettingsFinished(TParameterList AParameters)
        {
            // currently relies on fact that always when this screen is opened some settings are loaded
            FDuringLoadSettings = false;

            // first initialize motivation group list (depending on ledger) and then set values
            InitializeMotivationGroupList();
            clbMotivationGroup.SetCheckedStringList(AParameters.Get("param_motivation_group").ToString());
            FCheckedMotGroupStringList = clbMotivationGroup.GetCheckedStringList();

            // then initialize motivation detail list (depending on selected motivation groups) and then set values
            InitializeMotivationDetailList();
            clbMotivationDetail.SetCheckedStringList(AParameters.Get("param_motivation_detail").ToString());

            // restart normal user interface processing when user ticks/unticks motivation group
            clbMotivationGroup.ValueChanged += new EventHandler(MotivationGroupColumnChanged);
        }

        /// <summary>
        /// Reads the selected values from the controls, and stores them into the parameter system of FCalculator
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddStringParameter("param_motivation_group", this.clbMotivationGroup.GetCheckedStringList());
            ACalc.AddStringParameter("param_motivation_detail", this.clbMotivationDetail.GetCheckedStringList());
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            clbMotivationGroup.SetCheckedStringList(AParameters.Get("param_motivation_group").ToString());
            clbMotivationDetail.SetCheckedStringList(AParameters.Get("param_motivation_detail").ToString());
        }

        /// <summary>
        /// This will add functions to the list of available functions
        /// </summary>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            LedgerNumber = TLstTasks.CurrentLedger;
        }

        private void SelectAllMotivationGroup(System.Object sender, EventArgs e)
        {
            clbMotivationGroup.ValueChanged -= new EventHandler(MotivationGroupColumnChanged);
            clbMotivationGroup.SelectAll();
            MotivationGroupColumnChanged(this, null);
            clbMotivationGroup.ValueChanged += new EventHandler(MotivationGroupColumnChanged);
        }

        private void DeselectAllMotivationGroup(System.Object sender, EventArgs e)
        {
            clbMotivationGroup.ValueChanged -= new EventHandler(MotivationGroupColumnChanged);
            clbMotivationGroup.ClearSelected();
            MotivationGroupColumnChanged(this, null);
            clbMotivationGroup.ValueChanged += new EventHandler(MotivationGroupColumnChanged);
        }

        private void SelectAllMotivationDetail(System.Object sender, EventArgs e)
        {
            clbMotivationDetail.SelectAll();
        }

        private void DeselectAllMotivationDetail(System.Object sender, EventArgs e)
        {
            clbMotivationDetail.ClearSelected();
        }

        private void MotivationGroupColumnChanged(System.Object sender, EventArgs e)
        {
            //List<String> AddedMotivationGroups;

            if (!FDuringLoadSettings)
            {
                // update list of checked motivation groups for next time
                FCheckedMotGroupStringList = clbMotivationGroup.GetCheckedStringList();

                // this call will add/remove details to the list depending on groups selected
                InitializeMotivationDetailList();
            }
        }
    }
}