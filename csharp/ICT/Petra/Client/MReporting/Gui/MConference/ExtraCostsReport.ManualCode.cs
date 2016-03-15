//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
    /// Description of TFrmExtraCostsReport.ManualCode.
    /// </summary>
    public partial class TFrmExtraCostsReport
    {
        private DataTable FFieldTable;
        private long FLastConferenceKey;

        private void grdChargedFields_InitialiseData(TFrmPetraReportingUtils FPetraUtilsObject)
        {
            FLastConferenceKey = long.MinValue;
            ucoConferenceSelection.AddConfernceKeyChangedEventHandler(this.ConferenceKeyChanged);
            ucoConferenceSelection.AddConferenceSelectionChangedEventHandler(this.ConferenceSelectionChanged);
        }

        private void grdChargedFields_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            String SelectedFieldKeys = "";

            if ((FFieldTable != null)
                && rbtSelectedFields.Checked)
            {
                foreach (DataRow Row in FFieldTable.Rows)
                {
                    if (((bool)Row["Selection"])
                        && (ucoConferenceSelection.AllConferenceSelected
                            || (!ucoConferenceSelection.AllConferenceSelected && (bool)Row["Used_in_Conference"])))
                    {
                        SelectedFieldKeys = SelectedFieldKeys + Row["Unit_Key"].ToString() + ',';
                    }
                }
            }

            if (SelectedFieldKeys.Length > 0)
            {
                // Remove the last comma
                SelectedFieldKeys = SelectedFieldKeys.Remove(SelectedFieldKeys.Length - 1);
            }

            ACalc.AddStringParameter("param_selectedfieldkeys", SelectedFieldKeys);

            if (rbtSelectedFields.Checked)
            {
                ACalc.AddParameter("param_chargedfields", "Selected Fields");

                if ((SelectedFieldKeys.Length == 0)
                    && (AReportAction == TReportActionEnum.raGenerate))
                {
                    TVerificationResult VerificationResult = new TVerificationResult(
                        Catalog.GetString("Select at least one field to calculate the extra costs."),
                        Catalog.GetString("No field was selected!"),
                        TResultSeverity.Resv_Critical);
                    FPetraUtilsObject.AddVerificationResult(VerificationResult);
                }
            }
            else
            {
                ACalc.AddParameter("param_chargedfields", "All Fields");
            }
        }

        private void grdChargedFields_SetControls(TParameterList AParameters)
        {
            rbtSelectedFields.Checked = AParameters.Get("param_chargedfields").ToString() == "Selected Fields";
            rbtAllFields.Checked = AParameters.Get("param_chargedfields").ToString() == "All Fields";

            UpdateFieldData();

            String SelectedFieldKeys = AParameters.Get("param_selectedfieldkeys").ToString();

            SelectedFieldKeys = AddMissingZeros(SelectedFieldKeys);

            foreach (DataRow Row in FFieldTable.Rows)
            {
                String CurrentKey = Row["Unit_Key"].ToString();

                if (SelectedFieldKeys.Contains(CurrentKey))
                {
                    Row["Selection"] = true;
                }
            }
        }

        private void FieldSelectionChanged(System.Object sender, EventArgs e)
        {
            grdChargedFields.Enabled = rbtSelectedFields.Checked;
        }

        /// <summary>
        /// Add zeros at the end of the last field key in the string. The string might be "1030002,299"
        /// but should be "1030002,29900000"
        /// </summary>
        /// <param name="AFieldKeys">The comma separated list with partner keys</param>
        /// <returns></returns>
        private String AddMissingZeros(String AFieldKeys)
        {
            if (AFieldKeys.Length == 0)
            {
                return AFieldKeys;
            }

            int StartIndex = AFieldKeys.LastIndexOf(',') + 1;

            String LastKey = AFieldKeys.Substring(StartIndex, AFieldKeys.Length - StartIndex);

            int NumberMissingZeros = 10 - LastKey.Length;

            String MissingZeros = new String('0', NumberMissingZeros);

            return AFieldKeys + MissingZeros;
        }

        private void UpdateFieldData()
        {
            String ConferencePrefix;
            long ConferenceKey = Convert.ToInt64(ucoConferenceSelection.ConferenceKey);

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                // if all conferences are seleced we get the relevant fields of all conferences
                // The conference key must be set to -1
                ConferenceKey = -1;
            }

            if ((FLastConferenceKey == long.MinValue)
                || (FLastConferenceKey != ConferenceKey))
            {
                FLastConferenceKey = ConferenceKey;

                TRemote.MConference.WebConnectors.GetFieldUnits(ConferenceKey, TUnitTypeEnum.utChargedFields, out FFieldTable, out ConferencePrefix);

                if (grdChargedFields.Columns.Count == 0)
                {
                    grdChargedFields.AddCheckBoxColumn("", FFieldTable.Columns["Selection"]);
                    grdChargedFields.AddTextColumn(Catalog.GetString("Field Key"), FFieldTable.Columns["Unit_Key"]);
                    grdChargedFields.AddTextColumn(Catalog.GetString("Field Name"), FFieldTable.Columns["Unit_Name"]);
                }

                FFieldTable.DefaultView.AllowNew = false;
                FFieldTable.DefaultView.AllowEdit = true;
                FFieldTable.DefaultView.AllowDelete = false;

                grdChargedFields.DataSource = new DevAge.ComponentModel.BoundDataView(FFieldTable.DefaultView);

                grdChargedFields.AutoSizeCells();
            }

            if (ucoConferenceSelection.AllConferenceSelected)
            {
                FFieldTable.DefaultView.RowFilter = "";
            }
            else
            {
                FFieldTable.DefaultView.RowFilter = "Used_in_Conference = true";
            }
        }

        /// <summary>
        /// Event called when the text of "select conference button" has changed.
        /// Updates the Dates of the conference.
        /// </summary>
        /// <param name="AConferenceKey">Unit key of the conference</param>
        /// <param name="AConferenceName">Name of the conference</param>
        /// <param name="AValidConference">True if we have a valid conference. Otherwise false.</param>
        public void ConferenceKeyChanged(Int64 AConferenceKey, String AConferenceName, bool AValidConference)
        {
            if (AValidConference)
            {
                UpdateFieldData();
            }
        }

        /// <summary>
        /// Event called when the conference selection has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConferenceSelectionChanged(System.Object sender, EventArgs e)
        {
            UpdateFieldData();
        }
    }
}