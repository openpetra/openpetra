//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, peters
//
// Copyright 2004-2013 by OM International
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
using System.Reflection;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Controls;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MReporting.Gui.MPersonnel;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MConference.Gui
{
    /// <summary>
    /// Description of TFrmConferenceFindForm.ManualCode.
    /// </summary>
    public partial class TFrmConferenceFindForm
    {
        private String FSelectedConferenceName;
        private Int64 FSelectedConferenceKey;
        private bool FIsModal;

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

        /// <summary>
        /// true if screen has been opened modally
        /// </summary>
        public bool IsModal
        {
            set
            {
                FIsModal = value;
            }
        }

        /// <remarks>
        /// For NUnit tests that just try to open the Conference Find screen but which don't instantiate a Main Form
        /// we need to work around the fact that there is no Main Window!
        /// </remarks>
        private void InitGridManually()
        {
            MethodInfo Method = null;

            LoadDataGrid(true);

            grdConferences.DoubleClickCell += new TDoubleClickCellEventHandler(grdConferences_DoubleClickCell);

            // Attempt to obtain conference key from parent form or parent's parent form and use this to focus the currently selected
            // conference in the grid. If no conference key is found then the first conference in the grid will be focused.
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // Main Window will not be available if run from within NUnit Test without Main Form instance...
            if (MainWindow != null)
            {
                Method = MainWindow.GetType().GetMethod("GetSelectedConferenceKey");
            }

            if (Method == null)
            {
                // Main Window will not be available if run from within NUnit Test without Main Form instance...
                if (MainWindow != null)
                {
                    Method = MainWindow.GetType().GetMethod("GetPetraUtilsObject");
                }

                if (Method != null)
                {
                    TFrmPetraUtils ParentPetraUtilsObject = (TFrmPetraUtils)Method.Invoke(MainWindow, null);
                    MainWindow = ParentPetraUtilsObject.GetCallerForm();
                    Method = MainWindow.GetType().GetMethod("GetSelectedConferenceKey");
                }
            }

            if (Method != null)
            {
                FSelectedConferenceKey = Convert.ToInt64(Method.Invoke(MainWindow, null));
                int RowPos = 1;

                foreach (DataRowView rowView in FMainDS.PcConference.DefaultView)
                {
                    PcConferenceRow Row = (PcConferenceRow)rowView.Row;

                    if (Row.ConferenceKey == FSelectedConferenceKey)
                    {
                        break;
                    }

                    RowPos++;
                }

                // automatically select the current conference
                grdConferences.SelectRowInGrid(RowPos, true);
            }
        }

        void grdConferences_DoubleClickCell(object Sender, SourceGrid.CellContextEventArgs e)
        {
            Accept(e, null);
        }

        /// <summary>
        /// Create a new conference
        /// </summary>
        public void NewConference(System.Object sender, EventArgs e)
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            System.Int64 PartnerKey = 0;
            string PartnerShortName;
            String OutreachCode;

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenEventFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenEventFindScreen.Invoke
                        ("",
                        out PartnerKey,
                        out PartnerShortName,
                        out OutreachCode,
                        MainWindow);

                    // check if a conference already exists for chosen event
                    Boolean ConferenceExists = TRemote.MConference.Conference.WebConnectors.ConferenceExists(PartnerKey);

                    if ((PartnerKey != -1) && !ConferenceExists)
                    {
                        TRemote.MConference.Conference.WebConnectors.CreateNewConference(PartnerKey);

                        FSelectedConferenceKey = PartnerKey;
                        FSelectedConferenceName = PartnerShortName;

                        ReloadNavigation();
                    }
                    else if ((PartnerKey != -1) && ConferenceExists)
                    {
                        MessageBox.Show(Catalog.GetString("This conference already exists"), Catalog.GetString(
                                "New Conference"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        NewConference(sender, e);
                    }
                }
                catch (Exception exp)
                {
                    throw new EOPAppException("Exception occured while calling OpenEventFindScreen Delegate!", exp);
                }
            }
        }

        private void Accept(System.Object sender, EventArgs e)
        {
            if (grdConferences.SelectedDataRows.Length == 1)
            {
                FSelectedConferenceKey = (Int64)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PcConferenceTable.GetConferenceKeyDBName()];
                FSelectedConferenceName = (String)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()];

                if (!FIsModal)
                {
                    ReloadNavigation();
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        // reload navigation
        private void ReloadNavigation()
        {
            // update user defaults table
            TUserDefaults.SetDefault(TUserDefaults.CONFERENCE_LASTCONFERENCEWORKEDWITH, FSelectedConferenceKey);

            Form MainWindow = FPetraUtilsObject.GetCallerForm();
            MethodInfo method = MainWindow.GetType().GetMethod("LoadNavigationUI");

            if (method != null)
            {
                method.Invoke(MainWindow, new object[] { true });
            }

            method = MainWindow.GetType().GetMethod("SelectConferenceFolder");

            if (method != null)
            {
                method.Invoke(MainWindow, null);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Cancel(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Filter(System.Object sender, EventArgs e)
        {
            DataView MyDataView = FMainDS.PcConference.DefaultView;

            String Filter = "";

            if (!string.IsNullOrEmpty(txtConference.Text))
            {
                Filter += FMainDS.PcConference.Columns[PPartnerTable.GetPartnerShortNameDBName()] + " LIKE '" + "%" + txtConference.Text + "%'";
            }

            if (!string.IsNullOrEmpty(txtPrefix.Text))
            {
                if (Filter != "")
                {
                    Filter += " AND ";
                }

                Filter += FMainDS.PcConference.Columns[PcConferenceTable.GetOutreachPrefixDBName()] + " LIKE '" + txtPrefix.Text + "%'";
            }

            MyDataView.RowFilter = Filter;
            grdConferences.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            UpdateRecordNumberDisplay();
        }

        private void LoadDataGrid(bool AFirstTime)
        {
            FMainDS.PcConference.Clear();
            FMainDS.PPartner.Clear();

            FMainDS.Merge(TRemote.MConference.WebConnectors.GetConferences("", ""));

            if (FMainDS.PcConference.Rows.Count == FMainDS.PPartner.Rows.Count)
            {
                if (AFirstTime)
                {
                    FMainDS.PcConference.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), Type.GetType("System.String"));
                    FMainDS.PcConference.DefaultView.AllowNew = false;
                    FinishButtonPanelSetup();
                }

                for (int Counter = 0; Counter < FMainDS.PcConference.Rows.Count; ++Counter)
                {
                    FMainDS.PcConference.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()] =
                        FMainDS.PPartner.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()];
                }
            }

            // sort order for grid
            DataView MyDataView = FMainDS.PcConference.DefaultView;
            MyDataView.Sort = "p_partner_short_name_c ASC";
            grdConferences.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            UpdateRecordNumberDisplay();
        }

        // Deletes the conference
        private void RemoveRecord(System.Object sender, EventArgs e)
        {
            FSelectedConferenceKey = (Int64)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PcConferenceTable.GetConferenceKeyDBName()];
            FSelectedConferenceName = (String)((DataRowView)grdConferences.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()];

            TDeleteConference.DeleteConference(FPetraUtilsObject.GetCallerForm(), FSelectedConferenceKey, FSelectedConferenceName);

            LoadDataGrid(false);
        }

        private void FinishButtonPanelSetup()
        {
            // Further set up certain Controls Properties that can't be set directly in the WinForms Generator...
            lblRecordCounter.AutoSize = true;
            lblRecordCounter.Padding = new Padding(4, 3, 0, 0);
            lblRecordCounter.ForeColor = System.Drawing.Color.SlateGray;

            pnlButtonsRecordCounter.AutoSize = true;
        }

        // update the record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdConferences.DataSource != null)
            {
                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdConferences.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
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

            AConferenceKey = 0;
            AConferenceName = String.Empty;

            TFrmConferenceFindForm FindConference = new TFrmConferenceFindForm(AParentForm);
            FindConference.IsModal = true;

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