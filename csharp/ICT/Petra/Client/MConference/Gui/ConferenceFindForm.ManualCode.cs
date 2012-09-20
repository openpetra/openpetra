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
using Ict.Common.Controls;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common.Remoting.Client;
//using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MConference.Gui
{
    /// <summary>
    /// Description of TFrmConferenceFindForm.ManualCode.
    /// </summary>
    public partial class TFrmConferenceFindForm
    {
        private String FSelectedConferenceName;
        private Int64 FSelectedConferenceKey;

        /// <summary>
        /// publish the selected conference name
        /// </summary>
        public String SelectedConferenceName
        {
            get
            {
                return FSelectedConferenceName;
            }
        }

        /// <summary>
        /// publish the selected conference key
        /// </summary>
        public Int64 SelectedConferenceKey
        {
            get
            {
                return FSelectedConferenceKey;
            }
        }

        private void InitGridManually()
        {
            LoadDataGrid(true);

            grdConferences.DoubleClickCell += new TDoubleClickCellEventHandler(grdConferences_DoubleClickCell);
        }

        void grdConferences_DoubleClickCell(object Sender, SourceGrid.CellContextEventArgs e)
        {
            Accept(e, null);
        }

        private void Accept(System.Object sender, EventArgs e)
        {
            // int[] SelectedRowIndex = grdConferences.Selection.GetSelectionRegion().GetRowsIndex();

            if (grdConferences.SelectedDataRows.Length == 1)
            {
                FSelectedConferenceKey = (Int64)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PcConferenceTable.GetConferenceKeyDBName()];
                FSelectedConferenceName = (String)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()];
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Search(System.Object sender, EventArgs e)
        {
            LoadDataGrid(false);
        }

        private void LoadDataGrid(bool AFirstTime)
        {
            FMainDS.PcConference.Clear();
            FMainDS.PPartner.Clear();

            FMainDS.Merge(TRemote.MConference.WebConnectors.GetConferences(txtConference.Text, txtPrefix.Text));

            if (FMainDS.PcConference.Rows.Count == FMainDS.PPartner.Rows.Count)
            {
                if (AFirstTime)
                {
                    FMainDS.PcConference.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), Type.GetType("System.String"));
                    FMainDS.PcConference.DefaultView.AllowNew = false;
                }

                for (int Counter = 0; Counter < FMainDS.PcConference.Rows.Count; ++Counter)
                {
                    FMainDS.PcConference.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()] =
                        FMainDS.PPartner.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()];
                }
            }

            grdConferences.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PcConference.DefaultView);
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Partner Find Screen.
    /// </summary>
    public static class TConferenceFindScreenManager
    {
        /// <summary>
        /// Opens a Modal instance of the Conference Find screen.
        /// </summary>
        /// <param name="AConferenceNamePattern">Mathcing pattern for the conference name</param>
        /// <param name="AOutreachCodePattern">Matching patterns for the outreach code</param>
        /// <param name="AConferenceKey">Conference key of the found conference</param>
        /// <param name="AConferenceName">Partner ShortName name of the found conference</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if a conference was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String AConferenceNamePattern,
            String AOutreachCodePattern,
            out Int64 AConferenceKey,
            out String AConferenceName,
            Form AParentForm)
        {
            DialogResult dlgResult;

            AConferenceKey = -1;
            AConferenceName = String.Empty;

            TFrmConferenceFindForm FindConference = new TFrmConferenceFindForm(AParentForm);

            dlgResult = FindConference.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                AConferenceKey = FindConference.SelectedConferenceKey;
                AConferenceName = FindConference.SelectedConferenceName;

                return true;
            }

            return false;
        }
    }
}