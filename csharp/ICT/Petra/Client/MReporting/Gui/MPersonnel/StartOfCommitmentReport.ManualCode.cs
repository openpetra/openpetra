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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    /// <summary>
    /// manual code for TFrmStartOfCommitmentReport class
    /// </summary>
    public partial class TFrmStartOfCommitmentReport
    {
        private DataTable FCommitmentStatusTable;

        private void grdStatuses_InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            // Get list of commitment statuses
            FCommitmentStatusTable = TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                TCacheablePersonTablesEnum.CommitmentStatusList);

            FCommitmentStatusTable.Columns.Add("Selection", System.Type.GetType("System.Boolean"));

            grdStatuses.Columns.Clear();

            grdStatuses.AddCheckBoxColumn("", FCommitmentStatusTable.Columns["Selection"], false);
            grdStatuses.AddTextColumn("Type", FCommitmentStatusTable.Columns[PmCommitmentStatusTable.GetCodeDBName()]);
            grdStatuses.AddTextColumn("Description", FCommitmentStatusTable.Columns[PmCommitmentStatusTable.GetDescDBName()]);

            FCommitmentStatusTable.DefaultView.AllowNew = false;
            FCommitmentStatusTable.DefaultView.AllowDelete = false;

            grdStatuses.DataSource = new DevAge.ComponentModel.BoundDataView(FCommitmentStatusTable.DefaultView);
            grdStatuses.AutoSizeCells();

            chkSelectedStatusChanged(null, null);
        }

        private void grdStatuses_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            String DocumentTypeList = "";

            foreach (DataRow Row in FCommitmentStatusTable.Rows)
            {
                if ((Row["Selection"].GetType() == Type.GetType("System.Boolean"))
                    && ((bool)Row["Selection"]))
                {
                    DocumentTypeList = DocumentTypeList + (String)Row[PmCommitmentStatusTable.GetCodeDBName()] + ',';
                }
            }

            if (DocumentTypeList.Length > 0)
            {
                // Remove the last comma
                DocumentTypeList = DocumentTypeList.Remove(DocumentTypeList.Length - 1, 1);
                ACalc.AddParameter("param_commitmentstatuses", DocumentTypeList);
            }
            else if ((AReportAction == TReportActionEnum.raGenerate)
                     && (chkSelectedStatus.Checked)
                     && (!chkNoSelectedStatus.Checked))
            {
                // at least one commitment type must be checked
                TVerificationResult VerificationResult = new TVerificationResult("Select at least one commitment type.",
                    "No commitment type selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        private void grdStatuses_SetControls(TParameterList AParameters)
        {
            String SelectedStatuses = AParameters.Get("param_commitmentstatuses").ToString();

            foreach (DataRow Row in FCommitmentStatusTable.Rows)
            {
                String CurrentStatus = (String)Row[PmCommitmentStatusTable.GetCodeDBName()];

                Row["Selection"] = SelectedStatuses.Contains(CurrentStatus);
            }
        }

        private void chkSelectedStatusChanged(System.Object sender, EventArgs e)
        {
            chkNoSelectedStatus.Enabled = chkSelectedStatus.Checked;
            grdStatuses.Enabled = chkSelectedStatus.Checked;

            for (int Counter = 1; Counter < grdStatuses.Rows.Count; ++Counter)
            {
                if (chkSelectedStatus.Checked)
                {
                    grdStatuses.Rows.ShowRow(Counter);
                }
                else
                {
                    grdStatuses.Rows.HideRow(Counter);
                }
            }
        }

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.FFastReportsPlugin.SetDataGetter(LoadReportData);
        }

        private bool LoadReportData(TRptCalculator ACalc)
        {
            return FPetraUtilsObject.FFastReportsPlugin.LoadReportData("StartOfCommitmentReport",
                false,
                new string[] { "StartOfCommitment" },
                ACalc,
                this,
                true);
        }
    }
}