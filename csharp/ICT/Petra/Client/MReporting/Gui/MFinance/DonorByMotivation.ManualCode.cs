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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmDonorByMotivation
    {
        /// Indicator if settings are currently loaded. Initially set to true until after initial settings are loaded.
        private bool FDuringLoadSettings = true;

        private String FCheckedMotGroupStringList = "";
        private String FCheckedMotDetailStringList = "";

        private void InitializeManualCode()
        {
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
                GetSelectedLedger());
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

            //clbMotivationGroup.SelectionMode = SourceGrid.GridSelectionMode.Row;
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

            DataTable MotDetailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, GetSelectedLedger());
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

            //clbMotivationGroup.SelectionMode = SourceGrid.GridSelectionMode.Row;
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

        private void CmbLedgerChanged(System.Object sender, EventArgs e)
        {
            this.CmbLedgerChanged();
        }

        private void CmbLedgerChanged()
        {
            InitializeMotivationGroupList();
            InitializeMotivationDetailList();
        }

        private Int32 GetSelectedLedger()
        {
            Int32 LedgerNumber;

            // motivation group and detail depend on ledger
            if (cmbLedgerNumber.GetSelectedString().Trim().Length > 0)
            {
                LedgerNumber = Convert.ToInt32(cmbLedgerNumber.GetSelectedString());
            }
            else
            {
                LedgerNumber = 0;
            }

            return LedgerNumber;
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // add event handler so list boxes are also updated when user changes text in combobox with keyboard
            this.cmbLedgerNumber.TextChanged += new System.EventHandler(this.CmbLedgerChanged);

            // enable autofind in lists for first character (so the user can press character to find list entry)
            // from Sep 2015 this is handled automatically by the code generator
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
            List <String>AddedMotivationGroups;

            if (!FDuringLoadSettings)
            {
                // if a new motivation group has been selected then tick all details of that group by default
                AddedMotivationGroups = FindAddedMotivationGroups(FCheckedMotGroupStringList);

                // update list of checked motivation groups for next time
                FCheckedMotGroupStringList = clbMotivationGroup.GetCheckedStringList();

                // this call will add/remove details to the list depending on groups selected
                InitializeMotivationDetailList();

                if (AddedMotivationGroups.Count > 0)
                {
                    List <String>AllMotivationDetails = new List <String>(clbMotivationDetail.GetAllStringList(false).Split(','));

                    /* Key contains two columns: Group and Detail code. Keys at odd positions are Group Code
                     * and at even positions Detail Code. Find added groups and add pair of group and detail code */

                    int Index = 0;
                    bool AddNextDetail = false;

                    foreach (String KeyPart in AllMotivationDetails)
                    {
                        if (Index % 2 == 0)
                        {
                            // even Index: Group Code
                            if (AddedMotivationGroups.Contains(KeyPart))
                            {
                                // add group code now and indicate to add following detail in next loop
                                if (FCheckedMotDetailStringList.Length > 0)
                                {
                                    FCheckedMotDetailStringList += ",";
                                }

                                FCheckedMotDetailStringList += KeyPart;

                                AddNextDetail = true;
                            }
                        }
                        else
                        {
                            // odd Index: Detail Code
                            if (AddNextDetail)
                            {
                                if (FCheckedMotDetailStringList.Length > 0)
                                {
                                    FCheckedMotDetailStringList += ",";
                                }

                                FCheckedMotDetailStringList += KeyPart;

                                AddNextDetail = false;
                            }
                        }

                        // increase Index for next element
                        Index += 1;
                    }

                    // reset checked records
                    clbMotivationDetail.SetCheckedStringList(FCheckedMotDetailStringList);
                }
            }
        }

        private List <String>FindAddedMotivationGroups(String APreviouslyCheckedMotGroupStringList)
        {
            List <String>PreviousMotivationGroups = new List <String>(APreviouslyCheckedMotGroupStringList.Split(','));
            List <String>CurrentMotivationGroups = new List <String>(clbMotivationGroup.GetCheckedStringList().Split(','));
            List <String>AddedMotivationGroups = new List <String>();

            foreach (String MotivationGroup in CurrentMotivationGroups)
            {
                if (!PreviousMotivationGroups.Contains(MotivationGroup))
                {
                    AddedMotivationGroups.Add(MotivationGroup);
                }
            }

            return AddedMotivationGroups;
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

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }
    }
}