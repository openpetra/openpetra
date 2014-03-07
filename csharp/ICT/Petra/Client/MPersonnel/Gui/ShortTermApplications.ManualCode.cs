//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.Interfaces.MPersonnel;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPersonnel.Gui
{
    public partial class TFrmShortTermApplications
    {
        private long FEventPartnerKey = 0;
        private string FEventPartnerShortName;
        private String FOutreachCode;

        // option to show applicants for all outreaches of a conference
        private bool FShowAllOutreaches = false;

        // The user's default filter from SUserDefaults (if it exists)
        private string FUserDefaultFilter;

        private void InitializeManualCode()
        {
            // initially load all applicants that have no event
            FOutreachCode = "";
            txtEventName.Text = Catalog.GetString("Applications with no event filled in");

            // open partner edit screen when user double clicks on a row
            this.grdApplications.DoubleClick += new System.EventHandler(this.EditApplication);

            this.Closed += new System.EventHandler(this.TFrmPetra_ClosedManual);

            SetupFilter();
        }

        // read user default filter from SUserDefaults (if it exists) and apply it to form
        private void SetupFilter()
        {
            FUserDefaultFilter = TUserDefaults.GetStringDefault(TUserDefaults.PERSONNEL_APPLICATION_STATUS);
            string[] FilterArray = FUserDefaultFilter.Split(',');

            // No filter
            if (FUserDefaultFilter == "")
            {
                return;
            }
            // General filter
            else if (FUserDefaultFilter.Substring(0, 1) == ",")
            {
                rbtGeneral.Checked = true;

                foreach (string Filter in FilterArray)
                {
                    if (Filter == "A")
                    {
                        chkAccepted.Checked = true;
                    }
                    else if (Filter == "C")
                    {
                        chkCancelled.Checked = true;
                    }
                    else if (Filter == "H")
                    {
                        chkHold.Checked = true;
                    }
                    else if (Filter == "E")
                    {
                        chkEnquiry.Checked = true;
                    }
                    else if (Filter == "R")
                    {
                        chkRejected.Checked = true;
                    }
                }
            }
            // Detailed filter
            else
            {
                rbtDetailed.Checked = true;
                txtDetailedStatuses.Text = FUserDefaultFilter;
            }
        }

        private void InitGridManually()
        {
            LoadDataGrid();

            // if default filter exists then apply this to the grid
            if (FUserDefaultFilter != "")
            {
                FilterChange(this, null);
            }
            else
            {
                UpdateRecordNumberDisplay();
            }
        }

        private void LoadDataGrid()
        {
            FMainDS = new ApplicationTDS();

            // populate dataset
            TRemote.MPersonnel.WebConnectors.LoadShortTermApplications(ref FMainDS, FOutreachCode);

            FMainDS.PmShortTermApplication.DefaultView.AllowNew = false;

            // sort order for grid
            DataView MyDataView = FMainDS.PmShortTermApplication.DefaultView;
            MyDataView.Sort = "p_partner_short_name_c ASC";

            if (!chkShowAllOutreaches.Checked && chkShowAllOutreaches.Enabled)
            {
                // filter rows so only showing applicants for selected outreach rather than the entire conference
                MyDataView.RowFilter = PmShortTermApplicationTable.GetConfirmedOptionCodeDBName() + " = " + "'" + FOutreachCode + "'";
            }

            grdApplications.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        // Run when screen is closed to save filter as user default
        private void TFrmPetra_ClosedManual(object sender, EventArgs e)
        {
            string DefaultFilter = "";

            if (rbtGeneral.Checked)
            {
                DefaultFilter = ",";

                if (chkAccepted.Checked)
                {
                    DefaultFilter += "A,";
                }

                if (chkCancelled.Checked)
                {
                    DefaultFilter += "C,";
                }

                if (chkHold.Checked)
                {
                    DefaultFilter += "H,";
                }

                if (chkEnquiry.Checked)
                {
                    DefaultFilter += "E,";
                }

                if (chkRejected.Checked)
                {
                    DefaultFilter += "R";
                }
            }
            else if (rbtDetailed.Checked)
            {
                DefaultFilter = txtDetailedStatuses.Text;
            }

            TUserDefaults.SetDefault(TUserDefaults.PERSONNEL_APPLICATION_STATUS, DefaultFilter);
        }

        private void SelectEvent_Click(System.Object sender, EventArgs e)
        {
            Form MainWindow = FPetraUtilsObject.GetCallerForm();

            // If the delegate is defined, the host form will launch a Modal Partner Find screen for us
            if (TCommonScreensForwarding.OpenEventFindScreen != null)
            {
                // delegate IS defined
                try
                {
                    TCommonScreensForwarding.OpenEventFindScreen.Invoke
                        ("",
                        out FEventPartnerKey,
                        out FEventPartnerShortName,
                        out FOutreachCode,
                        MainWindow);

                    if (FEventPartnerKey != -1)
                    {
                        txtEventName.Text = FEventPartnerShortName;
                        chkShowAllOutreaches.Enabled = true;

                        InitGridManually();
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling OpenEventFindScreen Delegate!", exp);
                }
            }
        }

        private void SelectStatuses_Click(System.Object sender, EventArgs e)
        {
            // open dialog to select detailed statuses
            string ApplicationStatusList;
            DialogResult DlgResult = TFrmApplicationStatusDialog.OpenApplicationStatusDialog(
                txtDetailedStatuses.Text, FPetraUtilsObject.GetForm(), out ApplicationStatusList);

            if ((DlgResult == DialogResult.OK) && (txtDetailedStatuses.Text != ApplicationStatusList))
            {
                txtDetailedStatuses.Text = ApplicationStatusList;
            }

            // update the grid with the new filter
            FilterChange(sender, e);
        }

        private void ShowAllOutreaches_CheckBox(System.Object sender, EventArgs e)
        {
            FShowAllOutreaches = !FShowAllOutreaches;

            DataView MyDataView = FMainDS.PmShortTermApplication.DefaultView;

            if (!chkShowAllOutreaches.Checked)
            {
                // filter rows so only showing applicants for selected outreach rather than the entire conference
                MyDataView.RowFilter = PmShortTermApplicationTable.GetConfirmedOptionCodeDBName() + " = " + "'" + FOutreachCode + "'";
            }
            else
            {
                MyDataView.RowFilter = null;
            }

            grdApplications.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            UpdateRecordNumberDisplay();
        }

        private void EditApplication(System.Object sender, EventArgs e)
        {
            // Open the selected partner's Partner Edit screen at Personnel Applications
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, GetPartnerKeySelected(), TPartnerEditTabPageEnum.petpPersonnelApplications);
            frm.Show();
        }

        // update the grid once the filter is changed
        private void FilterChange(System.Object sender, EventArgs e)
        {
            List <string>Filters = new List <string>();
            string FiltersString = "";

            DataView MyDataView = FMainDS.PmShortTermApplication.DefaultView;
            MyDataView.RowFilter = null;

            if (chkShowAllOutreaches.Enabled && !chkShowAllOutreaches.Checked)
            {
                // filter rows so only showing applicants for selected outreach rather than the entire conference
                FiltersString = PmShortTermApplicationTable.GetConfirmedOptionCodeDBName() + " = '" + FOutreachCode + "'";
            }

            if (rbtGeneral.Checked)
            {
                if (!chkAccepted.Checked)
                {
                    Filters.Add("SUBSTRING(" + PmGeneralApplicationTable.GetGenApplicationStatusDBName() + ",1,1) <> 'A'");
                }

                if (!chkCancelled.Checked)
                {
                    Filters.Add("SUBSTRING(" + PmGeneralApplicationTable.GetGenApplicationStatusDBName() + ",1,1) <> 'C'");
                }

                if (!chkEnquiry.Checked)
                {
                    Filters.Add("SUBSTRING(" + PmGeneralApplicationTable.GetGenApplicationStatusDBName() + ",1,1) <> 'E'");
                }

                if (!chkHold.Checked)
                {
                    Filters.Add("SUBSTRING(" + PmGeneralApplicationTable.GetGenApplicationStatusDBName() + ",1,1) <> 'H'");
                }

                if (!chkRejected.Checked)
                {
                    Filters.Add("SUBSTRING(" + PmGeneralApplicationTable.GetGenApplicationStatusDBName() + ",1,1) <> 'R'");
                }

                for (int i = 0; i < Filters.Count; i++)
                {
                    if ((i != 0) || (FiltersString.Length > 0))
                    {
                        FiltersString += " AND " + Filters[i];
                    }
                    else
                    {
                        FiltersString += Filters[i];
                    }
                }
            }
            else if (rbtDetailed.Checked)
            {
                string[] DetailedStatuses = txtDetailedStatuses.Text.Split(',');

                foreach (string Status in DetailedStatuses)
                {
                    Filters.Add(PmGeneralApplicationTable.GetGenApplicationStatusDBName() + " = '" + Status + "'");
                }

                for (int i = 0; i < Filters.Count; i++)
                {
                    if ((i == 0) && (FiltersString.Length > 0))
                    {
                        FiltersString += " AND (" + Filters[i];
                    }
                    else if ((i == 0) && (FiltersString.Length == 0))
                    {
                        FiltersString += "(" + Filters[i];
                    }
                    else
                    {
                        FiltersString += " OR " + Filters[i];
                    }

                    if (i == Filters.Count - 1)
                    {
                        FiltersString += ")";
                    }
                }
            }

            MyDataView.RowFilter = FiltersString;

            grdApplications.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            UpdateRecordNumberDisplay();
        }

        private void GridRowSelected(System.Object sender, EventArgs e)
        {
            // Only enable the button if a row is selected.
            btnEditApplication.Enabled = true;
        }

        private long GetPartnerKeySelected()
        {
            return Convert.ToInt64(((DataRowView)grdApplications.SelectedDataRows[0]).Row[PmShortTermApplicationTable.GetPartnerKeyDBName()]);
        }

        // update the record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdApplications.DataSource != null)
            {
                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdApplications.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
        }
    }
}