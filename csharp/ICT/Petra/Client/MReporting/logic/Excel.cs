//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.IO;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using Ict.Common;

namespace Ict.Petra.Client.MReporting.Logic
{
    /// <summary>
    /// special logic for exporting report to Excel for graphs etc
    /// </summary>
    public class TReportExcel : TExcel
    {
        private TResultList results;
        private TParameterList parameters;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AResultList"></param>
        /// <param name="AParameters"></param>
        public TReportExcel(TResultList AResultList, TParameterList AParameters)
        {
            results = AResultList.ConvertToFormattedStrings(AParameters, "CSV");
            parameters = AParameters.ConvertToFormattedStrings("CSV");
        }

        private String GetRowDescr(TResult row)
        {
            String ReturnValue;

            ReturnValue = "";

            if ((row.header != null) && (row.header[0].ToString().Length > 0))
            {
                if (ReturnValue.Length > 0)
                {
                    ReturnValue = ReturnValue + " ";
                }

                ReturnValue = ReturnValue + row.header[0].ToString();
            }

            if ((row.header != null) && (row.header[1].ToString().Length > 0))
            {
                if (ReturnValue.Length > 0)
                {
                    ReturnValue = ReturnValue + " ";
                }

                ReturnValue = ReturnValue + row.header[1].ToString();
            }

            if ((row.descr != null) && (row.descr[0].ToString().Length > 0))
            {
                if (ReturnValue.Length > 0)
                {
                    ReturnValue = ReturnValue + " ";
                }

                ReturnValue = ReturnValue + row.descr[0].ToString();
            }

            if ((row.descr != null) && (row.descr[1].ToString().Length > 0))
            {
                if (ReturnValue.Length > 0)
                {
                    ReturnValue = ReturnValue + " ";
                }

                ReturnValue = ReturnValue + row.descr[1].ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Draw one chart in form of a pie on the current sheet, with the grand child lines of the given line as the parts of the pie
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DrawChartPieAccountBreakdown(System.Int16 columnNr, String masterLineCode, String chartSheetName)
        {
            TResult masterRow;
            ArrayList rows;
            Int32 rowCounter;
            String title;
            String ColumnCaption;
            TVariant value;

            masterRow = results.GetRow(masterLineCode);

            if (masterRow == null)
            {
                TLogging.Log("There is no row with line code " + masterLineCode);
                return;
            }

            rows = new ArrayList();
            title = masterLineCode;
            title = title + " " + parameters.Get("ControlSource", ReportingConsts.HEADERTITLE2).ToString(); // ledger name
            title = title + " " + masterRow.header[1].ToString(); // Main Account Description
            title = title + "\r\n" + parameters.Get("ControlSource", ReportingConsts.HEADERPERIOD).ToString(); // period range
            ColumnCaption = parameters.Get("ColumnCaption", columnNr).ToString();

            if (parameters.Exists("ColumnCaption2", columnNr))
            {
                ColumnCaption = ColumnCaption + " " + parameters.Get("ColumnCaption2", columnNr).ToString();
            }

            if (parameters.Exists("ColumnCaption3", columnNr))
            {
                ColumnCaption = ColumnCaption + " " + parameters.Get("ColumnCaption3", columnNr).ToString();
            }

            title = title + "\r\n" + ColumnCaption;
            title = title + "\r\n" + parameters.Get("ControlSource", ReportingConsts.HEADERDESCR2).ToString(); // currency and number format

            if (parameters.Get("param_depth").ToString() == "summary")
            {
                results.GetChildRows(masterRow.childRow, ref rows);
            }
            else
            {
                results.GetGrandChildRows(masterRow.childRow, ref rows);
            }

            if (rows.Count == 0)
            {
                return;
            }

            rowCounter = 0;

            foreach (TResult row in rows)
            {
                rowCounter = rowCounter + 1;
                SetValue(GetRange(1, rowCounter), GetRowDescr(row));
                value = new TVariant(row.column[columnNr].ToObject());

                if (parameters.Exists("param_currency_format"))
                {
                    value = new TVariant(StringHelper.FormatCurrency(value, parameters.Get("param_currency_format").ToString()));
                }

                SetValue(GetRange(2, rowCounter), value.ToString());
            }

            AddChart(chartSheetName, title, GetRange(1, 1, 2, rowCounter), XlChartType.xlPie);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="accountCode"></param>
        /// <param name="costCentreCodeList"></param>
        /// <param name="chartSheetName"></param>
        public void DrawChartPieCostCentreBreakdown(String accountCode, System.Collections.ArrayList costCentreCodeList, String chartSheetName)
        {
        }

        /// <summary>
        /// Draw one chart in form of a line diagram, each line for one of the given account codes
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DrawChartLineCompareAccounts(String costCentreCode, System.Collections.ArrayList accountCodeList, String chartSheetName)
        {
            TResult row;

            System.Int32 rowCounter;
            System.Int32 month;
            String title;
            TResult masterRow;
            DateTime monthDate;

            if (accountCodeList.Count < 1)
            {
                return;
            }

            masterRow = results.GetRow(costCentreCode);

            if (masterRow == null)
            {
                TLogging.Log("There is no row with line code " + costCentreCode);
                return;
            }

            title = parameters.Get("ControlSource", ReportingConsts.HEADERTITLE2).ToString(); // ledger name
            title = title + " " + masterRow.header[1].ToString(); // CostCentre Description
            title = title + "\r\n" + parameters.Get("ControlSource", ReportingConsts.HEADERPERIOD).ToString(); // period range
            rowCounter = 1;

            for (month = 1; month <= 12; month += 1)
            {
                monthDate = new DateTime(1970, month, 1);
                SetValue(GetRange(month + 1, rowCounter), monthDate.ToString("MMMM"));
            }

            rowCounter = rowCounter + 1;

            foreach (String accountCode in accountCodeList)
            {
                row = results.GetRow(costCentreCode + '/' + accountCode);
                SetValue(GetRange(1, rowCounter), accountCode);

                if (row != null)
                {
                    for (month = 1; month <= 12; month += 1)
                    {
                        SetValue(GetRange(month + 1, rowCounter), row.column[month - 1].ToObject());
                    }

                    rowCounter = rowCounter + 1;
                }
            }

            AddChart(chartSheetName, title, GetRange(1, 1, 13, rowCounter - 1), XlChartType.xlLine);
        }

        /// <summary>
        /// Draw one chart in form of a line diagram, each line for one of the given costcentre codes
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DrawChartLineCompareCostCentres(String accountCode, System.Collections.ArrayList costCentreCodeList, String chartSheetName)
        {
            String linecode;
            TResult row;

            System.Int32 rowCounter;
            System.Int32 month;
            String title;
            TResult masterRow;
            DateTime monthDate;

            if (costCentreCodeList.Count < 1)
            {
                return;
            }

            linecode = ((String)costCentreCodeList[0]);
            linecode = linecode + '/' + accountCode;
            masterRow = results.GetRow(linecode);

            if (masterRow == null)
            {
                TLogging.Log("There is no row with line code " + linecode);
                return;
            }

            title = parameters.Get("ControlSource", ReportingConsts.HEADERTITLE2).ToString(); // ledger name
            title = title + " " + masterRow.header[1].ToString(); // Account Description
            title = title + "\r\n" + parameters.Get("ControlSource", ReportingConsts.HEADERPERIOD).ToString(); // period range
            rowCounter = 1;

            for (month = 1; month <= 12; month += 1)
            {
                monthDate = new DateTime(1970, month, 1);
                SetValue(GetRange(month + 1, rowCounter), monthDate.ToString("MMMM"));
            }

            rowCounter = rowCounter + 1;

            foreach (String costCentreCode in costCentreCodeList)
            {
                row = results.GetRow(costCentreCode + '/' + accountCode);
                SetValue(GetRange(1, rowCounter), costCentreCode);

                if (row != null)
                {
                    for (month = 1; month <= 12; month += 1)
                    {
                        SetValue(GetRange(month + 1, rowCounter), row.column[month - 1].ToObject());
                    }

                    rowCounter = rowCounter + 1;
                }
            }

            AddChart(chartSheetName, title, GetRange(1, 1, 13, rowCounter - 1), XlChartType.xlLine);
        }

        /// <summary>
        /// Export the full result to Excel
        /// it is a modification of TResult.writeCSV
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ExportResult()
        {
            int i;
            ArrayList sortedList;
            bool display;

            System.Int32 columnCounter;
            System.Int32 rowCounter;
            bool useIndented;

            // write headings
            rowCounter = 1;
            columnCounter = 1;
            SetValue(GetRange(columnCounter, rowCounter), "id");
            columnCounter = columnCounter + 1;

            if (parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT1))
            {
                SetValue(GetRange(columnCounter, rowCounter), parameters.Get("ControlSource", ReportingConsts.HEADERPAGELEFT1).ToString());
                columnCounter = columnCounter + 1;
            }

            if (parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT2))
            {
                SetValue(GetRange(columnCounter, rowCounter), parameters.Get("ControlSource", ReportingConsts.HEADERPAGELEFT2).ToString());
                columnCounter = columnCounter + 1;
            }

            if (parameters.Exists("ControlSource", ReportingConsts.HEADERCOLUMN))
            {
                SetValue(GetRange(columnCounter, rowCounter), "header 1");
                columnCounter = columnCounter + 1;
                SetValue(GetRange(columnCounter, rowCounter), "header 0");
                columnCounter = columnCounter + 1;
            }

            parameters.Add("CurrentSubReport", 0);

            // otherwise 'indented' cannot be read
            useIndented = false;

            for (i = 0; i <= parameters.Get("lowestLevel").ToInt(); i += 1)
            {
                if (parameters.Exists("indented", ReportingConsts.ALLCOLUMNS, i))
                {
                    useIndented = true;
                }
            }

            for (i = 0; i <= -1; i += 1)
            {
                if ((!parameters.Get("ColumnCaption", i).IsNil()))
                {
                    SetValue(GetRange(columnCounter,
                            rowCounter),
                        (parameters.Get("ColumnCaption",
                             i).ToString() + ' ' +
                         parameters.Get("ColumnCaption2", i).ToString(false) + ' ' + parameters.Get("ColumnCaption3", i).ToString(false)).Trim());

                    if (useIndented)
                    {
                        columnCounter++;
                    }

                    columnCounter++;
                }
            }

            rowCounter = rowCounter + 1;
            results.SortChildren();
            sortedList = new ArrayList();
            results.CreateSortedListByMaster(sortedList, 0);

            // write each row to CSV file
            foreach (TResult element in sortedList)
            {
                if (element.display)
                {
                    columnCounter = 1;
                    SetValue(GetRange(columnCounter, rowCounter), element.code);
                    columnCounter++;

                    if (parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT1))
                    {
                        SetValue(GetRange(columnCounter, rowCounter), element.descr[0].ToString());
                        columnCounter++;
                    }

                    if (parameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT2))
                    {
                        SetValue(GetRange(columnCounter, rowCounter), element.descr[1].ToString());
                        columnCounter++;
                    }

                    if (parameters.Exists("ControlSource", ReportingConsts.HEADERCOLUMN))
                    {
                        SetValue(GetRange(columnCounter, rowCounter), element.header[1].ToString());
                        columnCounter++;
                        SetValue(GetRange(columnCounter, rowCounter), element.header[0].ToString());
                        columnCounter++;
                    }

                    display = false;

                    for (i = 0; i <= results.GetMaxDisplayColumns() - 1; i += 1)
                    {
                        if (parameters.Get("indented", i, element.depth, eParameterFit.eAllColumnFit).ToBool() == true)
                        {
                            columnCounter++;
                        }

                        if ((element.column != null) && (!element.column[i].IsNil()))
                        {
                            display = true;
                            SetValue(GetRange(columnCounter, rowCounter), element.column[i].ToString());
                        }
                        else
                        {
                            SetValue(GetRange(columnCounter, rowCounter), "");
                        }

                        if ((parameters.Get("indented", i, element.depth, eParameterFit.eAllColumnFit).ToBool() != true) && useIndented)
                        {
                            columnCounter++;
                        }

                        columnCounter++;
                    }

                    if (display)
                    {
                        rowCounter++;
                    }
                }
            }

            // todo: autofit all columns after the export to Excel
            // VBA:
            // Cells.Select
            // Cells.EntireColumn.AutoFit
        }
    }
}