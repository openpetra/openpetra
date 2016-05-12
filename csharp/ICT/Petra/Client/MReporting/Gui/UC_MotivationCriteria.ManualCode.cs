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

            if (AParameters.Get("param_motivation_group").ToString() == "All")
            {
                clbMotivationGroup.SetCheckedStringList(clbMotivationGroup.GetAllStringList());
            }
            else
            {
                clbMotivationGroup.SetCheckedStringList(AParameters.Get("param_motivation_group").ToString());
            }

            FCheckedMotGroupStringList = clbMotivationGroup.GetCheckedStringList();

            // then initialize motivation detail list (depending on selected motivation groups) and then set values
            InitializeMotivationDetailList();

            if (AParameters.Get("param_motivation_detail").ToString() == "All")
            {
                clbMotivationDetail.SetCheckedStringList(clbMotivationDetail.GetAllStringList());
            }
            else
            {
                clbMotivationDetail.SetCheckedStringList(AParameters.Get("param_motivation_detail").ToString());
            }

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

            string MotivationGroups = string.Empty;

            // are all motivation groups selected?
            if (clbMotivationGroup.GetAllStringList() == clbMotivationGroup.GetCheckedStringList())
            {
                ACalc.AddParameter("param_all_motivation_groups", true);

                MotivationGroups = "All";
            }
            else
            {
                ACalc.AddParameter("param_all_motivation_groups", false);

                // we need these list items enclosed with single quotes for SQL
                MotivationGroups = clbMotivationGroup.GetCheckedStringList(true);
                MotivationGroups = MotivationGroups.Replace("\"", "'");
            }

            ACalc.AddParameter("param_motivation_group_quotes", MotivationGroups);

            string Group_Detail_Pairs = string.Empty;
            string Group_Detail_Individual = string.Empty;

            // are all motivation details selected?
            if (clbMotivationDetail.GetAllStringList() == clbMotivationDetail.GetCheckedStringList())
            {
                ACalc.AddParameter("param_all_motivation_details", true);

                Group_Detail_Pairs = "All";
                Group_Detail_Individual = "All";
            }
            else
            {
                ACalc.AddParameter("param_all_motivation_details", false);

                // Motivation Group and Detail Code in Pairs. First value is group code, second is detail code.
                List <String>param_motivation_detail = new List <String>(ACalc.GetParameters().Get("param_motivation_detail").ToString().Split(','));

                int Index = 0;

                foreach (String KeyPart in param_motivation_detail)
                {
                    if (Index % 2 == 0)
                    {
                        if (Group_Detail_Pairs.Length > 0)
                        {
                            Group_Detail_Pairs += ",";
                            Group_Detail_Individual += ",";
                        }

                        // even Index: Group Code
                        Group_Detail_Pairs += "('" + KeyPart + "','";
                    }
                    else
                    {
                        // odd Index: Detail Code
                        Group_Detail_Pairs += KeyPart + "')";
                        Group_Detail_Individual += KeyPart;
                    }

                    // increase Index for next element
                    Index += 1;
                }
            }

            ACalc.AddParameter("param_motivation_group_detail_pairs", Group_Detail_Pairs);
            ACalc.AddParameter("param_motivation_details_only", Group_Detail_Individual);

            ACalc.AddParameter("param_number_of_mot_details", clbMotivationDetail.CheckedItemsCount);

            // create a header description for the motivation groups and details used
            if (ACalc.GetParameters().Get("param_all_motivation_groups").ToBool()
                && ACalc.GetParameters().Get("param_all_motivation_details").ToBool())
            {
                ACalc.AddParameter("param_group_detail_desc", "All");
            }
            else
            {
                string GroupsAndDetails = ACalc.GetParameters().Get("param_motivation_group_detail_pairs").ToString().Replace("\'", "");

                if (GroupsAndDetails.Length > 750)
                {
                    GroupsAndDetails = GroupsAndDetails.Substring(0, 67) + "...";
                }

                ACalc.AddParameter("param_group_detail_desc", GroupsAndDetails);
            }
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