//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, timop
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
using System.Drawing;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for birthday report
    /// </summary>
    public partial class TFrmBirthdayListReport
    {
        private PTypeTable FTypesTable;

        private void grdTypes_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            // Get list of commitment statuses
            FTypesTable = (PTypeTable)TDataCache.TMPartner.GetCacheablePartnerTable(
                TCacheablePartnerTablesEnum.PartnerTypeList);

            grdTypes.Columns.Clear();

            grdTypes.AddTextColumn("Type", FTypesTable.Columns[PTypeTable.GetTypeCodeDBName()]);

            FTypesTable.DefaultView.AllowNew = false;
            FTypesTable.DefaultView.AllowDelete = false;

            for (int Counter = FTypesTable.Rows.Count - 1; Counter >= 0; --Counter)
            {
                String TypeCode = (String)FTypesTable.Rows[Counter][PTypeTable.GetTypeCodeDBName()];

                // allow ORGANIZATION SPECIFIC TypeCode
                String[] DefaultWorkerTypes =
                    TAppSettingsManager.GetValue("DEFAULTWORKERTYPES_STARTINGWITH", "WORKER;EX-WORKER;ASSOC", false).Split(new char[] { ';' });

                bool typeInList = false;

                foreach (string allowedType in DefaultWorkerTypes)
                {
                    if (TypeCode.StartsWith(allowedType))
                    {
                        typeInList = true;
                    }
                }

                if (!typeInList)
                {
                    FTypesTable.Rows.RemoveAt(Counter);
                }
            }

            grdTypes.DataSource = new DevAge.ComponentModel.BoundDataView(FTypesTable.DefaultView);
            grdTypes.AutoSizeCells();
            grdTypes.Selection.EnableMultiSelection = true;
        }

        private void grdTypes_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            //param_partnertypelist
            if (chkSelectTypes.Checked)
            {
                String SelectedTypes = "";

                for (int Counter = 0; Counter < grdTypes.SelectedDataRows.Length; ++Counter)
                {
                    SelectedTypes = SelectedTypes +
                                    (String)((DataRowView)grdTypes.SelectedDataRows[Counter]).Row[FTypesTable.Columns[PTypeTable.GetTypeCodeDBName()]
                                    ] + ",";
                }

                // remove the last ,
                if (SelectedTypes.Length > 0)
                {
                    SelectedTypes = SelectedTypes.Substring(0, SelectedTypes.Length - 1);
                }

                ACalc.AddParameter("param_typecode", SelectedTypes);
            }
            else
            {
                ACalc.AddParameter("param_partnertypelist", "false");
            }

            if (chkIncludeFamily.Checked)
            {
                // Allow sorting of multiple levels
                // if the option "Family members" is used, then the report will
                // generate a result with multiple levels
                ACalc.AddParameter("param_sort_multiple_levels", "true");
            }

            String SortColumns = ACalc.GetParameters().Get("param_sortby_readable").ToString();

            if (SortColumns.Contains("Date of Birth"))
            {
                // Replace sorting of "Date of Birth" with "Date Of Birth This Year"
                String SortingColumns = ACalc.GetParameters().Get("param_sortby_columns").ToString();

                StringCollection SortingColumn = StringHelper.StrSplit(SortingColumns, ",");

                foreach (String StrColumn in SortingColumn)
                {
                    int currentColumn = Convert.ToInt32(StrColumn);

                    String ColumnName = ACalc.GetParameters().Get("param_calculation", currentColumn, -1, eParameterFit.eExact).ToString();

                    if (ColumnName == "Date of Birth")
                    {
                        ACalc.AddParameter("param_calculation", "Date Of Birth This Year", currentColumn);
                        break;
                    }
                }
            }
        }

        private void grdTypes_SetControls(TParameterList AParameters)
        {
            String SelectedTypes = AParameters.Get("param_typecode").ToString() + ",";

            grdTypes.Selection.ResetSelection(false);

            for (int Counter = 0; Counter <= FTypesTable.Rows.Count; ++Counter)
            {
                DataRowView Row = (DataRowView)grdTypes.Rows.IndexToDataSourceRow(Counter);

                if (Row != null)
                {
                    String CurrentType = (String)Row[0];

                    grdTypes.Selection.SelectRow(Counter, SelectedTypes.Contains((CurrentType + ",")));
                }
            }
        }

        private void SelectTypesChanged(System.Object sender, EventArgs e)
        {
            grdTypes.Enabled = chkSelectTypes.Checked;

            for (int Counter = 1; Counter < grdTypes.Rows.Count; ++Counter)
            {
                if (chkSelectTypes.Checked)
                {
                    grdTypes.Rows.ShowRow(Counter);
                }
                else
                {
                    grdTypes.Rows.HideRow(Counter);
                }
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // validation: date range not longer than a year.
            if (chkUseDate.Checked)
            {
                if (!dtpFromDate.Date.HasValue || !dtpToDate.Date.HasValue || !dtpFromDate.ValidDate(false) || !dtpToDate.ValidDate(false))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("We need a valid start and end date."),
                        Catalog.GetString("Please select a valid start date and a valid end date."),
                        TResultSeverity.Resv_Critical);

                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
                else if (dtpFromDate.Date.Value.AddYears(1).CompareTo(dtpToDate.Date) <= 0)
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Not more than a year between start and end date"),
                        Catalog.GetString("Please specify the end date not longer than one year from the start date."),
                        TResultSeverity.Resv_Critical);

                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }
        }
    }
}