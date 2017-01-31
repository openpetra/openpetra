//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2015 by OM International
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
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MReporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TFrmSelectMotivationCriteriaDialog
    {

        /// Indicator if settings are currently loaded. Initially set to true until after initial settings are loaded.
        private bool FDuringLoadSettings = true;

        private String FCheckedMotGroupStringList = "";
        private String FCheckedMotDetailStringList = "";

        private bool FUndoLastEdit = false;
        private Int32 FLedgerNumber = -1;

        private DataTable FMotGroupTable;
        private DataTable FMotDetailTable;

        /// <summary>
        /// ParameterList
        /// </summary>
        public TParameterList FParameters { set; get; }

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
            this.Resize += new System.EventHandler(ResizeLeftAndRight);
            // enable autofind in lists for first character (so the user can press character to find list entry)
            this.clbMotivationGroup.AutoFindColumn = ((Int16)(1));
            this.clbMotivationGroup.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            this.clbMotivationDetail.AutoFindColumn = ((Int16)(1));
            this.clbMotivationDetail.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            btnCancel.Click += new System.EventHandler(BtnCancel_Click);
            
            this.Shown += new System.EventHandler(DialogShown);
        }

        private void ResizeLeftAndRight(System.Object sender, EventArgs e)
        {
            pnlLeft.Width = (int) this.Width / 2;
            pnlRight.Width = (int) this.Width / 2;
            clbMotivationDetail.Width = (int)this.Width / 2 - 20;
            clbMotivationGroup.Width = (int)this.Width / 2 - 20;
        }

        /// <summary>
        /// initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;
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

            if(FMotGroupTable == null)
            {
                SetGroupTable();
            }

            DataView MotGroupView = new DataView(FMotGroupTable);

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
            List<String> KeyColumnList = new List<String>();
            KeyColumnList.Add(Value1MemberMotDetail);
            KeyColumnList.Add(Value2MemberMotDetail);
            string CheckedMotivationGroups = clbMotivationGroup.GetCheckedStringList();
            string Filter = "";

            if(FMotDetailTable == null)
            {
                SetDetailTable();
            }

            DataView MotDetailView = new DataView(FMotDetailTable);

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

        /// <summary>
        /// Loads Information for the Dialog
        /// </summary>
        /// <param name="AParameters"></param>
        public void LoadSettingsFinished(TParameterList AParameters)
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

        private void SetGroupTable()
        {
            FMotGroupTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationGroupList, FLedgerNumber);
        }

        private void SetDetailTable()
        {
            FMotDetailTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationList, FLedgerNumber);
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
            string Group_Detail_Individual_quotes = string.Empty;

            // are all motivation details selected?
            if (clbMotivationDetail.GetAllStringList() == clbMotivationDetail.GetCheckedStringList() && clbMotivationGroup.GetAllStringList() == clbMotivationGroup.GetCheckedStringList())
            {
                ACalc.AddParameter("param_all_motivation_details", true);

                Group_Detail_Pairs = "All";
                Group_Detail_Individual = "All";
                Group_Detail_Individual_quotes = "All";
            }
            else
            {
                ACalc.AddParameter("param_all_motivation_details", false);

                // Motivation Group and Detail Code in Pairs. First value is group code, second is detail code.
                List<String> param_motivation_detail = new List<String>(ACalc.GetParameters().Get("param_motivation_detail").ToString().Split(','));

                int Index = 0;

                foreach (String KeyPart in param_motivation_detail)
                {
                    if (Index % 2 == 0)
                    {
                        if (Group_Detail_Pairs.Length > 0)
                        {
                            Group_Detail_Pairs += ",";
                            Group_Detail_Individual += ",";
                            Group_Detail_Individual_quotes += ",";
                        }

                        // even Index: Group Code
                        Group_Detail_Pairs += "('" + KeyPart + "','";
                    }
                    else
                    {
                        // odd Index: Detail Code
                        Group_Detail_Pairs += KeyPart + "')";
                        Group_Detail_Individual += KeyPart;
                        Group_Detail_Individual_quotes += ("'" + KeyPart + "'");
                    }

                    // increase Index for next element
                    Index += 1;
                }
            }

            ACalc.AddParameter("param_motivation_group_detail_pairs", Group_Detail_Pairs);
            ACalc.AddParameter("param_motivation_details", Group_Detail_Individual);
            ACalc.AddParameter("param_motivation_details_quotes", Group_Detail_Individual_quotes);

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
                // this call will add/remove details to the list depending on groups selected
                InitializeMotivationDetailList();
            }
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            if (clbMotivationDetail.CheckedItemsCount == 0)
            {
                MessageBox.Show(Catalog.GetString("Please select at least one Motivation Detail!"), Catalog.GetString("Validate Data"));
            }
            else
            {
                FCheckedMotGroupStringList = clbMotivationGroup.GetCheckedStringList();
                FCheckedMotDetailStringList = clbMotivationDetail.GetCheckedStringList();
                FUndoLastEdit = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnCancel_Click(Object Sender, EventArgs e)
        {
            FUndoLastEdit = true;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DialogShown(Object Sender, EventArgs e)
        {
            if (FUndoLastEdit)
            {
                FDuringLoadSettings = true;
                clbMotivationGroup.SetCheckedStringList(FCheckedMotGroupStringList);
                FDuringLoadSettings = false;
                clbMotivationDetail.SetCheckedStringList(FCheckedMotDetailStringList);
                
            }
            
        }

        /// <summary>
        /// Returns the Number of Checked Items
        /// </summary>
        /// <returns></returns>
        public int GetTotalDetailsCount()
        {
            return FMotDetailTable.Rows.Count;
        }

        /// <summary>
        /// Returns the Checked String from clbMotivationDetails
        /// </summary>
        /// <returns></returns>
        public String GetDetailsCheckedString()
        {
            return clbMotivationDetail.GetCheckedStringList();
        }

        /// <summary>
        /// Returns the Checked String from clbMotivationGroup
        /// </summary>
        /// <returns></returns>
        public String GetGroupsCheckedString()
        {
            return clbMotivationGroup.GetCheckedStringList();
        }
    }
}