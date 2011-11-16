//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    /// <summary>
    /// Description of TFrmFieldReports.ManualCode.
    /// </summary>
    public partial class TFrmFieldReports
    {
        private TUnitTypeEnum FUnitType;

        private DataTable FUnitTableList;

        #region accessor
        /// <summary>
        /// Accessor for the derived classes (Charged Field Report, Sending Field Report, ...)
        /// </summary>
        protected GroupBox BaseGrpChargedFields
        {
            get
            {
                return grpChargedFields;
            }
        }

        /// <summary>
        /// Sets the xml reporting parameters in FPetraUtilsObject
        /// This is needed for the derived classes (Charged Field Report, Sending Field Report, ...)
        /// </summary>
        protected void SetReportParameters(String AXmlfile, String AReportName)
        {
            FPetraUtilsObject.FXMLFiles = AXmlfile;
            FPetraUtilsObject.FReportName = AReportName;
            FPetraUtilsObject.FCurrentReport = AReportName;
            FPetraUtilsObject.InitialiseData("");
            FPetraUtilsObject.LoadDefaultSettings();
        }

        #endregion

        private void grdFields_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FUnitType = TUnitTypeEnum.utUnknown;

            this.ucoConferenceSelection.AddConfernceKeyChangedEventHandler(this.ConferenceKeyChanged);
            this.ucoConferenceSelection.AddConferenceSelectionChangedEventHandler(this.ConferenceSelectionChanged);

            rbtFull.Checked = true;
            cmbSignOffLines.SelectedIndex = 0;
            cmbChargedFields.SelectedIndex = 0;

            ucoConferenceSelection.DisableRadioButtonAllConferences(true);
            ucoConferenceSelection.FShowSelectOutreachOptionsDialog = false;
        }

        /// <summary>
        /// Initialises the Field grid.
        /// </summary>
        /// <param name="AUnitType">Indicator which types of units should be shown</param>
        protected void grdFields_InitialiseData(TUnitTypeEnum AUnitType)
        {
            FUnitType = AUnitType;

            if (ucoConferenceSelection.ConferenceKey == grdFields.Text)
            {
                // don't reload the grid if the confernece key didn't change
                return;
            }

            grdFields.Text = ucoConferenceSelection.ConferenceKey;

            String ConferencePrefix;
            long ConferenceKey = Convert.ToInt64(ucoConferenceSelection.ConferenceKey);

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                // if all conferences are seleced we get the relevant fields of all conferences
                // The conference key must be set to -1
                ConferenceKey = -1;
            }

            TRemote.MConference.WebConnectors.GetFieldUnits(ConferenceKey, FUnitType, out FUnitTableList, out ConferencePrefix);

            if (grdFields.Columns.Count == 0)
            {
                // Add the columns only once.
                // Don't add them again when the conference key changed
                grdFields.AddCheckBoxColumn("", FUnitTableList.Columns["Selection"]);

                if (FUnitType == TUnitTypeEnum.utOutreachOptions)
                {
                    grdFields.AddTextColumn(Catalog.GetString("Outreach Code"), FUnitTableList.Columns["Outreach Code"]);
                }
                else
                {
                    grdFields.AddTextColumn(Catalog.GetString("Unit Key"), FUnitTableList.Columns["Unit Key"]);
                }

                grdFields.AddTextColumn(Catalog.GetString("Unit Name"), FUnitTableList.Columns["Unit Name"]);
            }

            FUnitTableList.DefaultView.AllowNew = false;
            FUnitTableList.DefaultView.AllowEdit = true;
            FUnitTableList.DefaultView.AllowDelete = false;

            grdFields.DataSource = new DevAge.ComponentModel.BoundDataView(FUnitTableList.DefaultView);

            grdFields.Text = ConferenceKey.ToString();
        }

        private void grdFields_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            int ColumnCounter = 0;

            DataTable ParaTable = ACalc.GetParameters().ToDataTable();

            // We need at least 14 columns (for the details)
            foreach (DataRow Row in ParaTable.Rows)
            {
                if ((String)Row[0] == "param_calculation")
                {
                    ColumnCounter++;
                }
            }

            if (ColumnCounter < 15)
            {
                for (int Counter = ColumnCounter; Counter < 15; ++Counter)
                {
                    ACalc.AddParameter("param_calculation", "DummyValue" + Counter.ToString(), Counter);
                }

                ACalc.AddParameter("MaxDisplayColumns", 15);
            }

            if (rbtFull.Checked)
            {
                ACalc.AddParameter("param_report_detail", "Full");
            }
            else if (rbtSummaries.Checked)
            {
                ACalc.AddParameter("param_report_detail", "Summary");
            }

            String SelectedFields = GetSelectedFieldKeysCSV();

            ACalc.AddStringParameter("param_selected_keys_csv", SelectedFields);

            if ((SelectedFields.Length < 1)
                && (AReportAction == TReportActionEnum.raGenerate))
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one Unit Key from the Additional Settings tab."),
                    Catalog.GetString("No Field for the report selected"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            ACalc.AddParameter("param_today", DateTime.Today);
        }

        private void grdFields_SetControls(TParameterList AParameters)
        {
            String SelectedFields = AParameters.Get("param_selected_keys_csv").ToString();

            UpdateUnitGrid(SelectedFields);

            rbtFull.Checked = (AParameters.Get("param_report_detail").ToString() == "Full");
            rbtSummaries.Checked = (AParameters.Get("param_report_detail").ToString() == "Summary");
        }

        private void grdFieldDoubleClick(System.Object sender, EventArgs e)
        {
        }

        private void SelectAll(System.Object sender, EventArgs e)
        {
            foreach (DataRow Row in FUnitTableList.Rows)
            {
                Row["Selection"] = true;
            }
        }

        private void DeselectAll(System.Object sender, EventArgs e)
        {
            foreach (DataRow Row in FUnitTableList.Rows)
            {
                Row["Selection"] = false;
            }
        }

        /// <summary>
        /// Show all units when "All Conferences" is chosen.
        /// Show only relevant units when "Selected Conference" is chosen.
        /// The third column in grdFields indicates if the unit is used in the current conference.
        /// </summary>
        /// <param name="ASelectedFields">csv list with the selected field keys</param>
        private void UpdateUnitGrid(String ASelectedFields)
        {
            if (FUnitTableList == null)
            {
                return;
            }

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                FUnitTableList.DefaultView.RowFilter = "Used_in_Conference = true";
            }
            else
            {
                FUnitTableList.DefaultView.RowFilter = "Used_in_Conference = true";
            }
        }

        /// <summary>
        /// Hide all units form grdFields
        /// </summary>
        /// <param name="AShowEntries">True to show all entries; false to hide them.</param>
        private void ShowAllFieldGridEntries(bool AShowEntries)
        {
            for (int Counter = grdFields.Rows.Count - 1; Counter > 0; --Counter)
            {
                if (AShowEntries)
                {
                    grdFields.Rows.ShowRow(Counter);
                }
                else
                {
                    grdFields.Rows.HideRow(Counter);
                }
            }
        }

        /// <summary>
        /// Stores the conference name in a member field.
        /// </summary>
        /// <param name="AConferenceKey">The partner key of the current conference</param>
        /// <param name="AConferenceName">The name of the conference</param>
        /// <param name="AValidConference">True if conference key and conference name are from a valid conference</param>
        private void ConferenceKeyChanged(long AConferenceKey, String AConferenceName, bool AValidConference)
        {
            if (AValidConference)
            {
                if (ucoConferenceSelection.ConferenceKey == grdFields.Text)
                {
                    // don't reload the grid if the confernece key didn't change
                    return;
                }

                grdFields_InitialiseData(FUnitType);
                UpdateUnitGrid("");
            }
            else
            {
                ShowAllFieldGridEntries(false);
            }
        }

        private void ConferenceSelectionChanged(System.Object sender, EventArgs e)
        {
            grdFields_InitialiseData(FUnitType);
        }

        /// <summary>
        /// Returns a csv list of unit keys of the selected units from grdFields
        /// </summary>
        /// <returns></returns>
        private String GetSelectedFieldKeysCSV()
        {
            String ReturnValue = "";

            foreach (DataRow Row in FUnitTableList.Rows)
            {
                if (((bool)Row["Selection"])
                    && (ucoConferenceSelection.AllConferenceSelected
                        || (!ucoConferenceSelection.AllConferenceSelected && (bool)Row["Used_in_Conference"])))
                {
                    if (FUnitType == TUnitTypeEnum.utOutreachOptions)
                    {
                        ReturnValue = ReturnValue + Row["Outreach Code"].ToString() + ',';
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Row["Unit Key"].ToString() + ',';
                    }
                }
            }

            if (ReturnValue.Length > 0)
            {
                // Remove the last comma
                ReturnValue = ReturnValue.Remove(ReturnValue.Length - 1);
            }

            return ReturnValue;
        }
    }
}