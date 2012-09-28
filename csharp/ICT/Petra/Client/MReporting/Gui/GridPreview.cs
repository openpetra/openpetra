//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Ict.Common.Remoting.Client;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using System.Data;
using System.Collections;
using Ict.Petra.Shared.MPartner;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>todoComment</summary>
    public delegate void TPreviewDelegate(TRptCalculator ACalculator);

    /// <summary>
    /// This has some functions to fill the Grid on the report Print Preview screen.
    /// It allows to open detail reports
    /// </summary>
    public class TGridPreview
    {
        private TResultList results;
        private TParameterList parameters;

        /// <summary>is reused for setting parameters</summary>
        private TParameterList FOrigParameters;

        /// <summary>the parameters for the new detail report</summary>
        private TParameterList FDetailParameters;
        private TSgrdDataGrid FGridView;
        private Form FPreviewForm;
        private TPreviewDelegate FPreviewDelegate;
        private Thread FGenerateReportThread;
        private TRptCalculator FCalculator;
        private TFrmPetraUtils FPetraUtilsObject;

        /// <summary>
        /// constructor
        /// </summary>
        public TGridPreview(Form APreviewForm,
            TFrmPetraUtils APetraUtilsObject,
            TPreviewDelegate APreviewDelegate,
            TResultList AResultList,
            TParameterList AParameters)
        {
            results = AResultList.ConvertToFormattedStrings(AParameters);
            FOrigParameters = AParameters;
            parameters = AParameters.ConvertToFormattedStrings();
            FPreviewForm = APreviewForm;
            FPetraUtilsObject = APetraUtilsObject;
            FPreviewDelegate = APreviewDelegate;
            FGenerateReportThread = null;
        }

        /// <summary>
        /// Try to populate the grid with the current result.
        /// </summary>
        /// <returns>s false if no detail report is available
        /// </returns>
        public Boolean PopulateResultGrid(TSgrdDataGrid ASgGridView)
        {
            Boolean ReturnValue;
            DataTable t;
            DataRow row;
            Int32 i;
            Int32 counter;
            Int32 columnCounter;
            string caption;
            ArrayList sortedList;
            bool display;

            ReturnValue = true;
            FGridView = ASgGridView;

            // only do this if there are detail reports available
            // this is to prevent bugs that are still happening (same column caption etc)
            if (!parameters.Exists("param_detail_report_0"))
            {
                return false;
            }

            results.SortChildren();
            sortedList = new ArrayList();
            results.CreateSortedListByMaster(sortedList, 0);

            // create columns
            // todo: header left
            // todo: indented columns
            t = new DataTable();
            columnCounter = 0;
            t.Columns.Add("id");

            for (i = 0; i <= results.GetMaxDisplayColumns() - 1; i += 1)
            {
                if ((!parameters.Get("ColumnCaption", i).IsNil()))
                {
                    caption =
                        (parameters.Get("ColumnCaption",
                             i).ToString() + ' ' +
                         parameters.Get("ColumnCaption2", i).ToString(false) + ' ' + parameters.Get("ColumnCaption3", i).ToString(false)).Trim();

                    // todo: add i for preventing same name columns (finance reports, long captions)
                    if (t.Columns.Contains(caption))
                    {
                        caption = caption + i.ToString();
                    }
                }
                else
                {
                    caption = "Column" + i.ToString();
                }

                /* if useIndented then
                 * begin
                 * columnCounter := ColumnCounter + 1;
                 * t.Columns.Add(caption + 'Indented');
                 * end;
                 */
                t.Columns.Add(caption);
                columnCounter = columnCounter + 1;
            }

            foreach (TResult element in sortedList)
            {
                if (element.display)
                {
                    row = t.NewRow();
                    display = false;
                    row[0] = element.code;

                    for (i = 0; i <= results.GetMaxDisplayColumns() - 1; i += 1)
                    {
                        if ((element.column[i] != null) && (!element.column[i].IsNil()))
                        {
                            display = true;
                            row[i + 1] = element.column[i].ToString();
                        }
                    }

                    if (display)
                    {
                        t.Rows.Add(row);
                    }
                }
            }

            FGridView.Columns.Clear();
            FGridView.AddTextColumn(t.Columns[0].ColumnName, t.Columns[0], 0);

            for (counter = 0; counter <= parameters.Get("MaxDisplayColumns").ToInt() - 1; counter += 1)
            {
                FGridView.AddTextColumn(t.Columns[counter + 1].ColumnName, t.Columns[counter + 1]);
            }

            FGridView.DataSource = new DevAge.ComponentModel.BoundDataView(new DataView(t));
            ((DevAge.ComponentModel.BoundDataView)FGridView.DataSource).AllowEdit = false;
            ((DevAge.ComponentModel.BoundDataView)FGridView.DataSource).AllowNew = false;
            ((DevAge.ComponentModel.BoundDataView)FGridView.DataSource).AllowDelete = false;
            FGridView.AutoSizeCells();

            // FGridView.Width := 576;   it is necessary to reassign the width because the columns don't take up the maximum width
            return ReturnValue;
        }

        /// <summary>
        /// populate the popup menu
        /// </summary>
        /// <param name="AContextMenu"></param>
        public void PopulateGridContextMenu(ContextMenu AContextMenu)
        {
            AContextMenu.MenuItems.Clear();
            Int32 Counter = 0;

            while (parameters.Exists("param_detail_report_" + Counter.ToString()) == true)
            {
                String detailReportCSV = parameters.Get("param_detail_report_" + Counter.ToString()).ToString();

                MenuItem mnuItem = new MenuItem();
                mnuItem.Tag = detailReportCSV;
                mnuItem.Text = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                mnuItem.OwnerDraw = true;
                mnuItem.Click += new EventHandler(this.MenuItemClick);
                AContextMenu.MenuItems.Add(mnuItem);
                Counter = Counter + 1;
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MenuItemClick(System.Object sender, System.EventArgs e)
        {
            Int32 columnCounter;
            String detailReportCSV;
            String action;
            String query;
            String LineCode;

            DataRowView[] TheDataRowViewArray;
            TResult SelectedResult;
            String paramName;
            String paramValue;
            String SettingsDirectory;
            MenuItem ClickedMenuItem = (MenuItem)sender;
            TheDataRowViewArray = FGridView.SelectedDataRowsAsDataRowView;

            if (TheDataRowViewArray.Length <= 0)
            {
                // no row is selected
                return;
            }

            LineCode = TheDataRowViewArray[0][0].ToString();
            SelectedResult = null;

            foreach (TResult r in results.GetResults())
            {
                if (r.code == LineCode)
                {
                    SelectedResult = r;
                }
            }

            detailReportCSV = (String)ClickedMenuItem.Tag;
            StringHelper.GetNextCSV(ref detailReportCSV, ",");

            // get rid of the name
            action = StringHelper.GetNextCSV(ref detailReportCSV, ",");

            if (action == "PartnerEditScreen")
            {
#if TODO
                // get the partner key
                Int64 PartnerKey = Convert.ToInt64(SelectedResult.column[Convert.ToInt32(detailReportCSV)].ToString());
                // TODO: open Partner Edit screen with the given partner key
#endif
            }
            else if (action.IndexOf(".xml") != -1)
            {
                query = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                FDetailParameters = new TParameterList(FOrigParameters);
                FDetailParameters.Add("param_whereSQL", query);

                // get the parameter names and values
                while (detailReportCSV.Length > 0)
                {
                    paramName = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    paramValue = StringHelper.GetNextCSV(ref detailReportCSV, ",");
                    FDetailParameters.Add(paramName, new TVariant(paramValue));
                }

                // add the values of the selected column (in this example the first one)
                for (columnCounter = 0; columnCounter <= FDetailParameters.Get("MaxDisplayColumns").ToInt() - 1; columnCounter += 1)
                {
                    FDetailParameters.Add("param_" +
                        FDetailParameters.Get("param_calculation", columnCounter).ToString(), SelectedResult.column[columnCounter]);
                }

                // action is a link to a settings file; it contains e.g. xmlfiles, currentReport, and column settings
                // TParameterList.Load adds the new parameters to the existing parameters
                SettingsDirectory = TClientSettings.ReportingPathReportSettings;
                FDetailParameters.Load(SettingsDirectory + '/' + action);
                GenerateReportInThread();
            }
        }

        private void GenerateReportInThread()
        {
            if ((FGenerateReportThread == null) || (!FGenerateReportThread.IsAlive))
            {
                FGenerateReportThread = new Thread(GenerateReport);
                FGenerateReportThread.IsBackground = true;
                FGenerateReportThread.Start();
            }
        }

        private void GenerateReport()
        {
            try
            {
                FPreviewForm.Cursor = Cursors.WaitCursor;
                TLogging.SetStatusBarProcedure(FPetraUtilsObject.WriteToStatusBar);

                // calculate the report
                FCalculator = new TRptCalculator();
                FCalculator.GetParameters().LoadFromDataTable(FDetailParameters.ToDataTable());

                if (FCalculator.GenerateResultRemoteClient())
                {
                    if (TClientSettings.DebugLevel >= TClientSettings.DEBUGLEVEL_REPORTINGDATA)
                    {
                        FCalculator.GetParameters().Save(TClientSettings.PathLog + Path.DirectorySeparatorChar + "debugParameterReturn.xml", true);
                        FCalculator.GetResults().WriteCSV(
                            FCalculator.GetParameters(), TClientSettings.PathLog + Path.DirectorySeparatorChar + "debugResultReturn.csv");
                    }

                    FPreviewForm.Cursor = Cursors.Default;
                    object[] Args = new object[1];
                    Args[0] = FCalculator;
                    FPreviewForm.Invoke((System.Delegate) new TPreviewDelegate(FPreviewDelegate), Args);
                }
                else
                {
                    // if generateResult failed
                    FPreviewForm.Cursor = Cursors.Default;

                    // EnableDisableToolbar(true);
                }
            }
            catch (Exception e)
            {
                TLogging.Log("Exception in GenerateReport: " + e.ToString());
                // EnableDisableToolbar(true);
            }
        }
    }
}