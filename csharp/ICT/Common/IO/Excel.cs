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
using System.Reflection;
using Ict.Common;

namespace Ict.Common.IO
{
    /// <summary>todoComment</summary>
    public enum XlChartType
    {
        /// <summary>todoComment</summary>
        xl3DArea = -4098,

        /// <summary>todoComment</summary>
        xl3DAreaStacked = 78,

        /// <summary>todoComment</summary>
        xl3DAreaStacked100 = 79,

        /// <summary>todoComment</summary>
        xl3DBarClustered = 60,

        /// <summary>todoComment</summary>
        xl3DBarStacked = 61,

        /// <summary>todoComment</summary>
        xl3DBarStacked100 = 62,

        /// <summary>todoComment</summary>
        xl3DColumn = -4100,

        /// <summary>todoComment</summary>
        xl3DColumnClustered = 54,

        /// <summary>todoComment</summary>
        xl3DColumnStacked = 55,

        /// <summary>todoComment</summary>
        xl3DColumnStacked100 = 56,

        /// <summary>todoComment</summary>
        xl3DLine = -4101,

        /// <summary>todoComment</summary>
        xl3DPie = -4102,

        /// <summary>todoComment</summary>
        xl3DPieExploded = 70,

        /// <summary>todoComment</summary>
        xlArea = 1,

        /// <summary>todoComment</summary>
        xlAreaStacked = 76,

        /// <summary>todoComment</summary>
        xlAreaStacked100 = 77,

        /// <summary>todoComment</summary>
        xlBarClustered = 57,

        /// <summary>todoComment</summary>
        xlBarOfPie = 71,

        /// <summary>todoComment</summary>
        xlBarStacked = 58,

        /// <summary>todoComment</summary>
        xlBarStacked100 = 59,

        /// <summary>todoComment</summary>
        xlBubble = 15,

        /// <summary>todoComment</summary>
        xlBubble3DEffect = 87,

        /// <summary>todoComment</summary>
        xlColumnClustered = 51,

        /// <summary>todoComment</summary>
        xlColumnStacked = 52,

        /// <summary>todoComment</summary>
        xlColumnStacked100 = 53,

        /// <summary>todoComment</summary>
        xlCloneBarClustered = 102,

        /// <summary>todoComment</summary>
        xlCloneBarStacked = 103,

        /// <summary>todoComment</summary>
        xlCloneBarStacked100 = 104,

        /// <summary>todoComment</summary>
        xlCloneCol = 105,

        /// <summary>todoComment</summary>
        xlCloneColClustered = 99,

        /// <summary>todoComment</summary>
        xlCloneColStacked = 100,

        /// <summary>todoComment</summary>
        xlCloneColStacked100 = 101,

        /// <summary>todoComment</summary>
        xlCylinderBarClustered = 95,

        /// <summary>todoComment</summary>
        xlCylinderBarStacked = 96,

        /// <summary>todoComment</summary>
        xlCylinderBarStacked100 = 97,

        /// <summary>todoComment</summary>
        xlCylinderCol = 98,

        /// <summary>todoComment</summary>
        xlCylinderColClustered = 92,

        /// <summary>todoComment</summary>
        xlCylinderColStacked = 93,

        /// <summary>todoComment</summary>
        xlCylinderColStacked100 = 94,

        /// <summary>todoComment</summary>
        xlDoughnut = 4120,

        /// <summary>todoComment</summary>
        xlDoughnutExploded = 80,

        /// <summary>todoComment</summary>
        xlLine = 4,

        /// <summary>todoComment</summary>
        xlLineMarkers = 65,

        /// <summary>todoComment</summary>
        xlLineMarkersStacked = 66,

        /// <summary>todoComment</summary>
        xlLineMarkersStacked100 = 67,

        /// <summary>todoComment</summary>
        xlLineStacked = 63,

        /// <summary>todoComment</summary>
        xlLineStacked100 = 64,

        /// <summary>todoComment</summary>
        xlPie = 5,

        /// <summary>todoComment</summary>
        xlPieExploded = 69,

        /// <summary>todoComment</summary>
        xlPieOfPie = 68,

        /// <summary>todoComment</summary>
        xlPyramidBarClustered = 109,

        /// <summary>todoComment</summary>
        xlPyramidBarStacked = 110,

        /// <summary>todoComment</summary>
        xlPyramidBarStacked100 = 111,

        /// <summary>todoComment</summary>
        xlPyramidCol = 112,

        /// <summary>todoComment</summary>
        xlPyramidColClustered = 106,

        /// <summary>todoComment</summary>
        xlPyramidColStacked = 107,

        /// <summary>todoComment</summary>
        xlPyramidColStacked100 = 108,

        /// <summary>todoComment</summary>
        xlRadar = -4151,

        /// <summary>todoComment</summary>
        xlRadarFilled = 82,

        /// <summary>todoComment</summary>
        xlRadarMarkers = 81,

        /// <summary>todoComment</summary>
        xlStockHLC = 88,

        /// <summary>todoComment</summary>
        xlStockOHLC = 89,

        /// <summary>todoComment</summary>
        xlStockVHLC = 90,

        /// <summary>todoComment</summary>
        xlStockVOHLC = 91,

        /// <summary>todoComment</summary>
        xlSurface = 83,

        /// <summary>todoComment</summary>
        xlSurfaceTopView = 85,

        /// <summary>todoComment</summary>
        xlSurfaceTopViewWireframe = 86,

        /// <summary>todoComment</summary>
        xlSurfaceWireframe = 84,

        /// <summary>todoComment</summary>
        xlXYScatter = -4169,

        /// <summary>todoComment</summary>
        xlXYScatterLines = 74,

        /// <summary>todoComment</summary>
        xlXYScatterLinesNoMarkers = 75,

        /// <summary>todoComment</summary>
        xlXYScatterSmooth = 72,

        /// <summary>todoComment</summary>
        xlXYScatterSmoothNoMarkers = 73
    };

    /// <summary>todoComment</summary>
    public enum XlChartLocation
    {
        /// <summary>todoComment</summary>
        xlLocationAsNewSheet = 1,

        /// <summary>todoComment</summary>
        xlLocationAsObject = 2,

        /// <summary>todoComment</summary>
        xlLocationAutomatic = 3
    };

    /// <summary>todoComment</summary>
    public enum XlDataLabelsType
    {
        /// <summary>todoComment</summary>
        xlDataLabelsShowNone = -4142,

        /// <summary>todoComment</summary>
        xlDataLabelsShowValue = 2,

        /// <summary>todoComment</summary>
        xlDataLabelsShowPercent = 3,

        /// <summary>todoComment</summary>
        xlDataLabelsShowLabel = 4,

        /// <summary>todoComment</summary>
        xlDataLabelsShowLabelAndPercent = 5,

        /// <summary>todoComment</summary>
        xlDataLabelsShowBubbleSizes = 6
    };

    /// <summary>
    /// This contains some functions to write into Excel via remote calls
    /// constants from http:www.p6c.com/CommonTypelibs/O2000_EXCEL9.CSV
    /// </summary>
    public class TExcel
    {
        private System.Type MSExcelType;
        private System.Object MSExcel;
        private System.Object workbooks;
        private System.Object workbook;
        private System.Object sheet;
        private System.Int32 countFixedSheets;

        /// <summary>
        /// Instantiate Microsoft Excel
        /// </summary>
        /// <returns>void</returns>
        public TExcel() : base()
        {
            // eg HKEY_CLASSES_ROOT\Excel.Application.11 'Excel.Application.11'
            MSExcelType = System.Type.GetTypeFromProgID("Excel.Application", true);

            if (MSExcelType == null)
            {
                throw new Exception("no Excel available");
            }

            MSExcel = Activator.CreateInstance(MSExcelType);

            // Get a new workbook.
            // oWB = (Excel._Workbook)(oXL.Workbooks.Add( Missing.Value ));
            workbooks = MSExcelType.InvokeMember("Workbooks", BindingFlags.GetProperty, null, MSExcel, null);
            workbook = MSExcelType.InvokeMember("Add", BindingFlags.InvokeMethod, null, workbooks, null);
            countFixedSheets = 0;
        }

        private String FixSheetName(String sheetName)
        {
            sheetName = sheetName.Replace('[', ' ');
            sheetName = sheetName.Replace(']', ' ');

            if (sheetName.Length >= 32)
            {
                countFixedSheets = countFixedSheets + 1;
                sheetName = sheetName.Substring(1, 29) + countFixedSheets.ToString();
            }

            // TODO : fix name of sheet
            // countFixedSheets := countFixedSheets + 1;
            // sheetName := 'MySheet ' + countFixedSheets.tostring();
            return sheetName;

            // TLogging.Log('sheetname: ' + result);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public System.Object NewSheet(String sheetName)
        {
            System.Object sheets;

            // thisWorkbook.Sheets.Add(Missing.Value, Missing.Value, 1, Missing.Value);
            sheets = MSExcelType.InvokeMember("Sheets", BindingFlags.GetProperty, null, workbook, null);

            object[] parameters = new object[4];
            parameters[0] = Missing.Value;
            parameters[1] = Missing.Value;
            parameters[2] = (Object)(1);
            parameters[3] = Missing.Value;
            MSExcelType.InvokeMember("Add", BindingFlags.GetProperty, null, sheets, parameters);

            // oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            sheet = MSExcelType.InvokeMember("ActiveSheet", BindingFlags.GetProperty, null, workbook, null);
            sheetName = FixSheetName(sheetName);

            parameters = new object[1];
            parameters[0] = sheetName;
            MSExcelType.InvokeMember("Name", BindingFlags.SetProperty, null, sheet, parameters);
            return sheet;
        }

        /// <summary>
        /// Get a range object that contains cell (column, row).
        /// </summary>
        /// <returns>void</returns>
        public System.Object GetRange(System.Int32 column, System.Int32 row)
        {
            object[] parameters = new object[2];
            parameters[0] = Convert.ToChar(column + Convert.ToInt16('A') - 1) + row.ToString();
            parameters[1] = Missing.Value;
            return MSExcelType.InvokeMember("Range", BindingFlags.GetProperty, null, sheet, parameters);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="col2"></param>
        /// <param name="row2"></param>
        /// <returns></returns>
        public System.Object GetRange(System.Int32 column, System.Int32 row, System.Int32 col2, System.Int32 row2)
        {
            object[] parameters = new object[2];
            parameters[0] = Convert.ToChar(column + Convert.ToInt16('A') - 1) + row.ToString();
            parameters[1] = Convert.ToChar(col2 + Convert.ToInt16('A') - 1) + row2.ToString();
            return MSExcelType.InvokeMember("Range", BindingFlags.GetProperty, null, sheet, parameters);
        }

        /// <summary>
        /// Write value in cell.
        /// </summary>
        /// <returns>void</returns>
        public void SetValue(System.Object range, System.Object value)
        {
            // Write value in cell.
            object[] parameters = new object[1];
            parameters[0] = value;
            MSExcelType.InvokeMember("Value", BindingFlags.SetProperty, null, range, parameters);
        }

        /// <summary>
        /// Write formula in cell.
        /// </summary>
        /// <returns>void</returns>
        public void SetFormula(System.Object range, String formula)
        {
            object[] parameters = new object[1];
            parameters[0] = formula;

            // rng.Formula = "=@Sum(B5..B8)";
            MSExcelType.InvokeMember("Formula", BindingFlags.SetProperty, null, range, parameters);

            // rng.Calculate();
            MSExcelType.InvokeMember("Calculate", BindingFlags.InvokeMethod, null, range, null);
        }

        /// <summary>
        /// add a chart with dimensions and referencing range
        /// </summary>
        /// <returns>void</returns>
        public void AddChart(String chartSheetName,
            String title,
            System.Object valueRange,
            XlChartType typeOfChart)
        {
            System.Object charts;
            System.Object chartObj;
            System.Object chart;
            System.Object chartTitle;
            System.Object characters;
            System.Object seriesCollection;
            System.Object series;
            System.Int32 x1;
            System.Int32 y1;
            System.Int32 x2;
            System.Int32 y2;
            x1 = 150;
            y1 = 20;
            x2 = 500;
            y2 = 300;
            object[] parameters = new object[4];
            parameters[0] = (Object)(x1);
            parameters[1] = (Object)(y1);
            parameters[2] = (Object)(x2);
            parameters[3] = (Object)(y2);

            // office.ChartObjects charts = (office.ChartObjects)ws.ChartObjects(missing);
            // office.ChartObjectchartObj = charts.Add(150, 20, 500, 300);
            charts = MSExcelType.InvokeMember("ChartObjects", BindingFlags.GetProperty, null, sheet, null);
            chartObj = MSExcelType.InvokeMember("Add", BindingFlags.InvokeMethod, null, charts, parameters);
            chart = MSExcelType.InvokeMember("Chart", BindingFlags.GetProperty, null, chartObj, null);

            // chartObj.Chart.ChartType = office.XlChartType.xlLine;

            parameters = new object[1];
            parameters[0] = (Object)(typeOfChart);
            MSExcelType.InvokeMember("ChartType", BindingFlags.SetProperty, null, chart, parameters);

            // chartObj.Chart.SetSourceData(chartRange, missing);
            parameters = new object[2];
            parameters[0] = valueRange;
            parameters[1] = Missing.Value;
            MSExcelType.InvokeMember("SetSourceData", BindingFlags.InvokeMethod, null, chart, parameters);
            seriesCollection = MSExcelType.InvokeMember("SeriesCollection", BindingFlags.GetProperty, null, chart, null);
            parameters = new object[1];
            parameters[0] = (Object)(1);
            series = MSExcelType.InvokeMember("Item", BindingFlags.GetProperty, null, seriesCollection, parameters);

            /*
             * // for Office 2003:
             * setLength(parameters, 10);
             * parameters[0] := Missing.Value; // System.Object(XlDataLabelsType.xlDataLabelsShowLabelAndPercent ); // Type, XlDataLabelsType
             * parameters[1] := System.Object(false); // Legend key
             * parameters[2] := System.Object(false); // AutoText
             * parameters[3] := System.Object(true); // has Leader Lines
             * parameters[4] := System.Object(false); // show series name
             * parameters[5] := System.Object(withCategoryName); // show category name
             * parameters[6] := System.Object(withValue); // show value
             * parameters[7] := System.Object(withPercentage); // show percentage
             * parameters[8] := System.Object(false); // show bubble size
             * parameters[9] := Missing.Value; // separator
             */

            // for Office 2000:
            parameters = new object[1];

            // Type, XlDataLabelsType
            parameters[0] = (Object)XlDataLabelsType.xlDataLabelsShowLabelAndPercent;
            MSExcelType.InvokeMember("ApplyDataLabels", BindingFlags.InvokeMethod, null, series, parameters);

            /* ActiveChart.SeriesCollection(1).Name = "=Sheet1!R1C1"
             * seriesCollection := MSExcelType.InvokeMember(
             * 'SeriesCollection', BindingFlags.GetProperty, nil, chart, nil);
             * SetLength(parameters, 1);
             * parameters[0] := System.Object(1);
             * series := chart.GetType().InvokeMember(
             * 'Item', BindingFlags.InvokeMethod, nil, seriesCollection, parameters);
             *
             * SetLength(parameters, 1);
             * parameters[0] := '=Sheet1!A1A1';
             * series.GetType().InvokeMember(
             * 'Name', BindingFlags.SetProperty, nil, series, parameters);
             */

            // chartObj.Chart.HasLegend := true;
            parameters = new object[1];
            parameters[0] = (Object)(true);
            chart.GetType().InvokeMember("HasLegend", BindingFlags.SetProperty, null, chart, parameters);

            // chartObj.Chart.HasTitle := true;
            parameters = new object[1];
            parameters[0] = (Object)(true);
            chart.GetType().InvokeMember("HasTitle", BindingFlags.SetProperty, null, chart, parameters);

            // chartObj.Chart.ChartTitle.Characters.Text := title;
            chartTitle = chart.GetType().InvokeMember("ChartTitle", BindingFlags.GetProperty, null, chart, null);
            parameters = new object[2];
            parameters[0] = Missing.Value;
            parameters[1] = Missing.Value;
            characters = chart.GetType().InvokeMember("Characters", BindingFlags.GetProperty, null, chartTitle, parameters);
            parameters = new object[1];
            parameters[0] = title;
            characters.GetType().InvokeMember("Text", BindingFlags.SetProperty, null, characters, parameters);

            // move into its own sheet, must be called last (!!!)
            // chart.Location((Excel.XlChartLocation)xlLocationAsNewSheet, "test");
            parameters = new object[2];
            chartSheetName = FixSheetName(chartSheetName);
            parameters[0] = (Object)XlChartLocation.xlLocationAsNewSheet;
            parameters[1] = (String)(chartSheetName);
            MSExcelType.InvokeMember("Location", BindingFlags.InvokeMethod, null, chart, parameters);
        }

        /// <summary>
        /// overload
        /// </summary>
        public void Show()
        {
            MSExcelType.InvokeMember("Visible", BindingFlags.SetProperty, null, MSExcel, new object[] { true });
        }

        /// <summary>
        /// leave Excel to the user, disconnect this program of it
        /// </summary>
        /// <returns>void</returns>
        public void GiveUserControl()
        {
            // Make sure Excel is visible and give the user control
            // of Microsoft Excel's lifetime.
            Show();
            MSExcelType.InvokeMember("UserControl", BindingFlags.SetProperty, null, MSExcel, new object[] { true });
            MSExcel = null;
            MSExcelType = null;
        }
    }
}