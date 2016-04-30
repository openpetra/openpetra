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
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    public partial class TFrmMotivationResponse
    {
        private Int32 FLedgerNumber;

        /// Indicator if settings are currently loaded. Initially set to true until after initial settings are loaded.
        private bool FDuringLoadSettings = true;

        private String FCheckedMotGroupStringList = "";
        private String FCheckedMotDetailStringList = "";

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                ReportTypeChanged(this, null);

                FPetraUtilsObject.LoadDefaultSettings();
                FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
            }
        }

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // if fast reports isn't working then close the screen
            if ((FPetraUtilsObject.GetCallerForm() != null) && !FPetraUtilsObject.FFastReportsPlugin.LoadedOK)
            {
                MessageBox.Show("No alternative reporting engine is available for this report. This screen will now be closed.", "Reporting engine");
                this.Close();
            }

            // enable autofind in lists for first character (so the user can press character to find list entry)
            this.clbMotivationGroup.AutoFindColumn = ((Int16)(1));
            this.clbMotivationGroup.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            this.clbMotivationDetail.AutoFindColumn = ((Int16)(1));
            this.clbMotivationDetail.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
        }

        private void InitializeManualCode()
        {
            // set the delegate that is needed before and after LoadSettings has been run
            FPetraUtilsObject.DelegateLoadSettingsStarting = @LoadSettingsStarting;
            FPetraUtilsObject.DelegateLoadSettingsFinished = @LoadSettingsFinished;

            cmbMailingCode.cmbCombobox.AllowBlankValue = true;
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

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);

            if (dtpToDate.Date.HasValue)
            {
                Int32 ToDateYear = dtpToDate.Date.Value.Year;
                //TODO: Calendar vs Financial Date Handling - Confirm that these should not be ledger dates, i.e. allowing for >12 periods and non-calendar period boundaries
                DateTime FromDateThisYear = new DateTime(ToDateYear, 1, 1);
                DateTime ToDatePreviousYear = new DateTime(ToDateYear - 1, 12, 31);
                DateTime FromDatePreviousYear = new DateTime(ToDateYear - 1, 1, 1);

                ACalc.AddParameter("param_from_date_this_year", FromDateThisYear);
                ACalc.AddParameter("param_to_date_previous_year", ToDatePreviousYear);
                ACalc.AddParameter("param_from_date_previous_year", FromDatePreviousYear);
            }

            int MaxColumns = ACalc.GetParameters().Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter <= MaxColumns; ++Counter)
            {
                String ColumnName = ACalc.GetParameters().Get("param_calculation", Counter, 0).ToString();

                if (ColumnName == "Gift Amount")
                {
                    ACalc.AddParameter("param_gift_amount_column", Counter);
                }
            }
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            DateTime dtpFromDateDate = AParameters.Get("param_from_date").ToDate();

            if ((dtpFromDateDate <= DateTime.MinValue)
                || (dtpFromDateDate >= DateTime.MaxValue))
            {
                dtpFromDate.Date = new DateTime(DateTime.Today.Year, 1, 1);
            }
        }

        private Boolean LoadReportData(TRptCalculator ACalc)
        {
            Shared.MReporting.TParameterList pm = ACalc.GetParameters();

            // are all motivation groups selected?
            if (clbMotivationGroup.GetAllStringList() == clbMotivationGroup.GetCheckedStringList())
            {
                pm.Add("param_all_motivation_groups", true);
            }
            else
            {
                pm.Add("param_all_motivation_groups", false);

                // we need these list items enclosed with single quotes for SQL
                string MotivationGroups = clbMotivationGroup.GetCheckedStringList(true);
                MotivationGroups = MotivationGroups.Replace("\"", "'");
                pm.Add("param_motivation_group_quotes", MotivationGroups);
            }

            // are all motivation details selected?
            if (clbMotivationDetail.GetAllStringList() == clbMotivationDetail.GetCheckedStringList())
            {
                pm.Add("param_all_motivation_details", true);
            }
            else
            {
                pm.Add("param_all_motivation_details", false);

                // Motivation Group and Detail Code in Pairs. First value is group code, second is detail code.
                List <String>param_motivation_detail = new List <String>(pm.Get("param_motivation_detail").ToString().Split(','));

                int Index = 0;
                String Group_Detail_Pairs = "";
                string Group_Detail_Individual = "";

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

                pm.Add("param_motivation_group_detail_pairs", Group_Detail_Pairs);
                pm.Add("param_motivation_details_only", Group_Detail_Individual);
            }

            pm.Add("param_number_of_mot_details", clbMotivationDetail.CheckedItemsCount);

            ArrayList reportParam = ACalc.GetParameters().Elems;
            Dictionary <String, TVariant>paramsDictionary = new Dictionary <string, TVariant>();

            foreach (Shared.MReporting.TParameter p in reportParam)
            {
                if (p.name.StartsWith("param") && (p.name != "param_calculation"))
                {
                    paramsDictionary.Add(p.name, p.value);
                }
            }

            DataTable ReportTable = TRemote.MReporting.WebConnectors.GetReportDataTable("MotivationResponse", paramsDictionary);

            if (ReportTable == null)
            {
                FPetraUtilsObject.WriteToStatusBar("Report Cancelled.");
                return false;
            }

            FPetraUtilsObject.FFastReportsPlugin.RegisterData(ReportTable, "MotivationResponse");
            //
            // My report doesn't need a ledger row - only the name of the ledger. And I need the currency formatter..
            DataTable LedgerNameTable = TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerNameList);
            DataView LedgerView = new DataView(LedgerNameTable);
            LedgerView.RowFilter = "LedgerNumber=" + FLedgerNumber;
            String LedgerName = "";

            if (LedgerView.Count > 0)
            {
                LedgerName = LedgerView[0].Row["LedgerName"].ToString();
            }

            ACalc.AddStringParameter("param_ledger_name", LedgerName);
            ACalc.AddStringParameter("param_currency_formatter", "0,0.000");

            Boolean HasData = ReportTable.Rows.Count > 0;

            if (!HasData)
            {
                MessageBox.Show(Catalog.GetString("No Motivation Response data found."), "Motivation Response");
            }

            return HasData;
        }

        #region Event Handlers

        private void ReportTypeChanged(object ASender, System.EventArgs AEventArgs)
        {
            if (cmbReportType.GetSelectedString() == "Totals")
            {
                chkSuppressDetail.Enabled = false;
            }
            else
            {
                chkSuppressDetail.Enabled = true;
            }
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

        #endregion
    }
}