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
    public partial class TFrmConvertApplicationsToPreviousExperience
    {
        private long FEventPartnerKey = 0;
        private string FEventPartnerShortName;
        private String FOutreachCode;
        private String FNoApplication = Catalog.GetString("No event selected");
        private int FRecordCount = 0;

        private void InitializeManualCode()
        {
            // initially all applicants that have no event
            FOutreachCode = "";
            txtEventName.Text = FNoApplication;

            rbtConvert.Checked = true;

            FinishButtonPanelSetup();
            lblRecordCounter.Text = "";

            // open partner edit screen when user double clicks on a row
            this.grdApplications.DoubleClick += new System.EventHandler(this.EditApplication);

            // populate the event year combo box
            int MinYear;
            int MaxYear;

            TRemote.MPersonnel.WebConnectors.GetRangeOfYearsWithEvents(out MinYear, out MaxYear);

            cmbEventYear.AddStringItem("");

            for (int i = MaxYear; i >= MinYear; i--)
            {
                cmbEventYear.AddStringItem(i.ToString());
            }
        }

        private void RunOnceOnActivationManual()
        {
            // This fixes the button position. Can't get this right in YAML file
            btnClear.Location = new System.Drawing.Point(
                pnlCriteriaButtons.Location.X + pnlCriteriaButtons.Size.Width - btnClear.Size.Width - 7, btnClear.Location.Y);
        }

        private void LoadDataGrid(bool ABlankGrid = false)
        {
            this.Cursor = Cursors.WaitCursor;

            FMainDS = new ApplicationTDS();

            if (!ABlankGrid)
            {
                // populate dataset
                TRemote.MPersonnel.WebConnectors.LoadApplicationsForConverting(
                    ref FMainDS, rbtConvert.Checked, FOutreachCode, chkShowAllOutreaches.Checked, rbtAll.Checked, cmbEventYear.GetSelectedString());
            }

            FMainDS.PmShortTermApplication.DefaultView.AllowNew = false;

            // sort order for grid
            DataView MyDataView = FMainDS.PmShortTermApplication.DefaultView;

            if (!ABlankGrid)
            {
                MyDataView.Sort = "p_partner_short_name_c ASC";
            }

            grdApplications.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);

            // change button text depending on what kind of opperation this will be
            if (rbtConvert.Checked)
            {
                btnConvertApplications.Text = string.Format("Convert Applications");
            }
            else
            {
                btnConvertApplications.Text = string.Format("Remove Applications");
            }

            grdApplications.SelectRowInGrid(1);
            UpdateRecordNumberDisplay();

            this.Cursor = Cursors.Default;
        }

        private void Search(System.Object sender, EventArgs e)
        {
            if (rbtSelectEvent.Checked && string.IsNullOrEmpty(FOutreachCode))
            {
                MessageBox.Show(Catalog.GetString("You have not selected an event."),
                    Catalog.GetString("No Event Selected"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            LoadDataGrid();
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
                    }
                }
                catch (Exception exp)
                {
                    throw new ApplicationException("Exception occured while calling OpenEventFindScreen Delegate!", exp);
                }
            }
        }

        private void ConvertApplications(System.Object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // make sure there is data in the dataset (this should never be a problem anyway)
                if ((FMainDS == null) || (FMainDS.PmShortTermApplication.Rows.Count == 0))
                {
                    MessageBox.Show(Catalog.GetString("There are no applications that meet the required criteria."),
                        Catalog.GetString("No Applications"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                // converting to past experience
                if (rbtConvert.Checked
                    && (MessageBox.Show(string.Format(Catalog.GetString(
                                    "This will create a Previous Experience record for every application in the grid.{0}{0}Do you want to continue?"),
                                "\r\n"),
                            Catalog.GetString("Convert Applications To Past Experience"),
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes))
                {
                    if (TRemote.MPersonnel.WebConnectors.ConvertApplicationsToPreviousExperience(FMainDS))
                    {
                        MessageBox.Show(string.Format(Catalog.GetPluralString(
                                    "1 application has been successfully converted into a Previous Experience record.",
                                    "All {0} applications have been successfully converted into Previous Experience records.",
                                    FRecordCount), FRecordCount),
                            Catalog.GetString("Convert Applications To Past Experience"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDataGrid(true);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Conversion failed! No new Past Experience records were created."),
                            Catalog.GetString("Convert Applications To Past Experience"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                // removing from past experience
                else if (MessageBox.Show(string.Format(Catalog.GetString(
                                     "This will delete all Previous Experience records that were created from the applications in the grid.{0}{0}Do you want to continue?"),
                                 "\r\n"),
                             Catalog.GetString("Remove Applications From Past Experience"),
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (TRemote.MPersonnel.WebConnectors.RemoveApplicationsFromPreviousExperience(FMainDS))
                    {
                        MessageBox.Show(string.Format(Catalog.GetPluralString(
                                    "Previous Experience records have successfully been removed from 1 application.",
                                    "Previous Experience records have successfully been removed from all {0} applications.",
                                    FRecordCount), FRecordCount),
                            Catalog.GetString("Remove Applications From Past Experience"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDataGrid(true);
                    }
                    else
                    {
                        MessageBox.Show(Catalog.GetString("Operation failed! No Past Experience records were removed."),
                            Catalog.GetString("Remove Applications From Past Experience"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // clear search criteria and search results
        private void Clear_Click(System.Object sender, EventArgs e)
        {
            FOutreachCode = "";
            txtEventName.Text = FNoApplication;
            cmbEventYear.SelectedIndex = 0;

            LoadDataGrid(true);
        }

        private void EditApplication(System.Object sender, EventArgs e)
        {
            PmShortTermApplicationRow SelectedRow = GetSelectedApplication();

            // Open the selected partner's Partner Edit screen at Personnel Applications
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, SelectedRow.PartnerKey, TPartnerEditTabPageEnum.petpPersonnelApplications);
            frm.Show();
            frm.SelectApplication(SelectedRow.ApplicationKey, SelectedRow.RegistrationOffice);
        }

        private PmShortTermApplicationRow GetSelectedApplication()
        {
            return (PmShortTermApplicationRow)((DataRowView)grdApplications.SelectedDataRows[0]).Row;
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
            FRecordCount = 0;

            if (grdApplications.DataSource != null)
            {
                FRecordCount = ((DevAge.ComponentModel.BoundDataView)grdApplications.DataSource).Count;
            }

            lblRecordCounter.Text = String.Format(
                Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, FRecordCount,
                    true),
                FRecordCount);

            if (FRecordCount > 0)
            {
                btnConvertApplications.Enabled = true;
                btnEditApplication.Enabled = true;
            }
            else
            {
                btnConvertApplications.Enabled = false;
                btnEditApplication.Enabled = false;
            }
        }
    }
}