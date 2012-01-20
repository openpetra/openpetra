//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       binki
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


using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MReporting;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPartner
{
    public partial class TFrmPartnerByCommitmentExtract
    {
        /// <summary>
        ///   For managing grdCommitmentStatusChoices.
        /// </summary>
        private DataTable FCommitmentStatusTable;

        private void InitializeManualCode()
        {
            SetControls(new TParameterList());
        }

        private void grdCommitmentStatusChoices_InitialiseData(TFrmPetraReportingUtils FPetraUtilsObject)
        {
            #region Copied from csharp/ICT/Petra/Client/MReporting/Gui/MPersonnel/StartOfCommitmentReport.ManualCode.cs

            /**
             * \todo
             *   The following block of code should be generalized
             *   along with the similar code from
             *   StartOfCommitmentReport.ManualCode.cs.
             */
            FCommitmentStatusTable = TDataCache.TMPersonnel.GetCacheablePersonnelTable(
                TCacheablePersonTablesEnum.CommitmentStatusList);

            FCommitmentStatusTable.Columns.Add("Selection", System.Type.GetType("System.Boolean"));

            grdCommitmentStatusChoices.Columns.Clear();

            grdCommitmentStatusChoices.AddCheckBoxColumn("", FCommitmentStatusTable.Columns["Selection"], false);
            grdCommitmentStatusChoices.AddTextColumn("Status", FCommitmentStatusTable.Columns[PmCommitmentStatusTable.GetCodeDBName()]);
            grdCommitmentStatusChoices.AddTextColumn("Description", FCommitmentStatusTable.Columns[PmCommitmentStatusTable.GetDescDBName()]);

            FCommitmentStatusTable.DefaultView.AllowNew = false;
            FCommitmentStatusTable.DefaultView.AllowDelete = false;

            grdCommitmentStatusChoices.DataSource = new DevAge.ComponentModel.BoundDataView(FCommitmentStatusTable.DefaultView);
            grdCommitmentStatusChoices.AutoSizeCells();
            #endregion
        }

        private void grdCommitmentStatusChoices_ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            /**
             * \todo
             *   The reason that the status choices listbox is not
             *   filled in with saved data is that Parameter.Save()
             *   and Parameter.Load() do not use
             *   TVariant.EncodeToString() and
             *   TVariant.DecodeFromString() when storing a saved set
             *   of parameters. Composites survive fine over the
             *   remoting interface, though. I (binki) am allowed to
             *   change the behavior of TParameterList.Save() and
             *   TParameterList.Load() _if_ I change the
             *   XmlReports/Settings/.../standard.xml files to support
             *   DecodeFromString().
             */
            TVariant param_grdCommitmentStatusChoices = new TVariant();

            foreach (DataRow ARow in FCommitmentStatusTable.Rows)
            {
                if ((bool)ARow["Selection"])
                {
                    param_grdCommitmentStatusChoices.Add(new TVariant((String)ARow[PmCommitmentStatusTable.GetCodeDBName()]), "", false);
                }
            }

            ACalc.AddParameter("param_grdCommitmentStatusChoices", param_grdCommitmentStatusChoices);
        }

        private void grdCommitmentStatusChoices_SetControls(TParameterList AParameters)
        {
            HashSet <String>AValues = new HashSet <String>();

            foreach (TVariant choice in AParameters.Get("param_grdCommitmentStatusChoices").ToComposite())
            {
                AValues.Add(choice.ToString());
            }

            foreach (DataRow ARow in FCommitmentStatusTable.Rows)
            {
                ARow["Selection"] = AValues.Contains((String)ARow[PmCommitmentStatusTable.GetCodeDBName()]);
            }
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_txtFieldSending", txtFieldSending.Text);
            ACalc.AddParameter("param_txtFieldReceiving", txtFieldReceiving.Text);
        }

        /// <summary>
        /// Set the given DateTime control to be blank instead of
        /// DateTime.Now if the value is not specified.
        /// </summary>
        private void SetdtpControlManual(Ict.Petra.Client.CommonControls.TtxtPetraDate AControl,
            TVariant AValue)
        {
            if (AValue.IsZeroOrNull())
            {
                AControl.Date = null;
            }
        }

        private void SetControlsManual(TParameterList AProperties)
        {
            SetdtpControlManual(dtpStartDateFrom, AProperties.Get("param_dtpStartDateFrom"));
            SetdtpControlManual(dtpStartDateTo, AProperties.Get("param_dtpStartDateTo"));
            SetdtpControlManual(dtpEndDateFrom, AProperties.Get("param_dtpEndDateFrom"));
            SetdtpControlManual(dtpEndDateTo, AProperties.Get("param_dtpEndDateTo"));
            SetdtpControlManual(dtpDateValidOn, AProperties.Get("param_dtpDateValidOn"));

            /**
             * \todo
             *   The following code will be obsolete once Andrew
             *   Webster's UC_ExtractChkFilter / ucoExtractChkFilter
             *   is finished.
             */
            if (AProperties.GetParameter("param_chkPartnerActive") == null)
            {
                chkPartnerActive.Checked = true;
            }

            if (AProperties.GetParameter("param_chkMailable") == null)
            {
                chkMailable.Checked = true;
            }

            if (AProperties.GetParameter("param_chkRespectNoSolicitors") == null)
            {
                chkRespectNoSolicitors.Checked = true;
            }

            /*
             * For some reason, when setting a textbox to have the
             * class PartnerKey we lose the automatic SetControls()
             * and ReadControls() stuff for them.
             */
            if (AProperties.GetParameter("param_txtFieldReceiving") != null)
            {
                txtFieldReceiving.Text = AProperties.Get("param_txtFieldReceiving").ToString();
            }

            if (AProperties.GetParameter("param_txtFieldSending") != null)
            {
                txtFieldSending.Text = AProperties.Get("param_txtFieldSending").ToString();
            }

            chkCommitmentStatus_Changed(chkCommitmentStatus, null);
        }

        /// <summary>
        /// Disable or enable the commitments selection group depending
        /// on if the user wants this group to affect his query.
        /// </summary>
        private void chkCommitmentStatus_Changed(Object sender, EventArgs e)
        {
            grpCommitmentStatuses.Enabled = ((System.Windows.Forms.CheckBox)sender).Checked;
        }
    }
}