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
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MConference.Validation;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MConference.Gui
{
    public partial class TFrmAttendeeList
    {
        /// PartnerKey for selected conference to be set from outside
        public static Int64 FPartnerKey {
            private get; set;
        }

        private void InitializeManualCode()
        {
            string ConferenceName;
            TPartnerClass PartnerClass;

            // display the conference name in the title bar and in a text box at the top of the screen
            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(FPartnerKey, out ConferenceName, out PartnerClass);
            this.Text = this.Text + " [" + ConferenceName + "]";
            txtConferenceName.Text = ConferenceName;

            // open partner edit screen when user double clicks on a row
            this.grdAttendees.DoubleClick += new System.EventHandler(this.EditPerson);
        }

        private void InitGridManually()
        {
            LoadDataGrid();
            UpdateRecordNumberDisplay();
        }

        private void LoadDataGrid()
        {
            List <long>HomeOfficeKeyList = new List <long>();

            FMainDS.PcAttendee.Clear();
            FMainDS.PmGeneralApplication.Clear();
            FMainDS.PmShortTermApplication.Clear();
            FMainDS.PPartner.Clear();

            // populate dataset
            TRemote.MConference.Conference.WebConnectors.GetConferenceApplications(ref FMainDS, FPartnerKey);

            // Add all homeofficekeys to list as this column will be removed from dataset
            foreach (PcAttendeeRow Row in FMainDS.PcAttendee.Rows)
            {
                if (Row.IsHomeOfficeKeyNull() || (Row.HomeOfficeKey == 0))
                {
                    HomeOfficeKeyList.Add(((int)Row.PartnerKey / 1000000) * 1000000);
                }
                else
                {
                    HomeOfficeKeyList.Add(Row.HomeOfficeKey);
                }
            }

            FMainDS.PcAttendee.Columns.Add(PPartnerTable.GetPartnerShortNameDBName(), Type.GetType("System.String"));
            FMainDS.PcAttendee.Columns.Add(PmShortTermApplicationTable.GetStFieldChargedDBName(), Type.GetType("System.String"));
            FMainDS.PcAttendee.Columns.Add(PmShortTermApplicationTable.GetStCurrentFieldDBName(), Type.GetType("System.String"));

            // This column is removed and readded with a different type. This allows the partner short name to be displayed rather than the partner key.
            FMainDS.PcAttendee.Columns.Remove(PcAttendeeTable.GetHomeOfficeKeyDBName());
            FMainDS.PcAttendee.Columns.Add(PcAttendeeTable.GetHomeOfficeKeyDBName(), Type.GetType("System.String"));

            FMainDS.PcAttendee.DefaultView.AllowNew = false;

            for (int Counter = 0; Counter < FMainDS.PcAttendee.Rows.Count; ++Counter)
            {
                long PartnerKey = ((PcAttendeeRow)FMainDS.PcAttendee.Rows[Counter]).PartnerKey;
                long HomeOfficeKey = HomeOfficeKeyList[Counter];
                long StFieldCharged = 0;
                long StConfirmedOption = 0;

                foreach (PmShortTermApplicationRow Row in FMainDS.PmShortTermApplication.Rows)
                {
                    if (Row.PartnerKey == PartnerKey)
                    {
                        if (Row.IsStFieldChargedNull() || (Row.StFieldCharged == 0))
                        {
                            if (Row.IsStCurrentFieldNull() || (Row.StCurrentField == 0))
                            {
                                StFieldCharged = HomeOfficeKey;
                            }
                            else
                            {
                                StFieldCharged = Row.StCurrentField;
                            }
                        }
                        else
                        {
                            StFieldCharged = Row.StFieldCharged;
                        }

                        if (Row.IsStCurrentFieldNull() || (Row.StConfirmedOption == 0))
                        {
                            FMainDS.PcAttendee.Rows[Counter][PmShortTermApplicationTable.GetStCurrentFieldDBName()] =
                                ((PcAttendeeRow)FMainDS.PcAttendee.Rows[Counter]).OutreachType;
                        }
                        else
                        {
                            StConfirmedOption = Row.StConfirmedOption;
                        }

                        break;
                    }
                }

                int PartnersFound = 0;

                foreach (PPartnerRow Row in FMainDS.PPartner.Rows)
                {
                    if (Row.PartnerKey == PartnerKey)
                    {
                        FMainDS.PcAttendee.Rows[Counter][PPartnerTable.GetPartnerShortNameDBName()] = Row.PartnerShortName;

                        if (PartnersFound == 3)
                        {
                            break;
                        }
                        else
                        {
                            PartnersFound++;
                        }
                    }

                    // display partner short name rather than the partner key
                    if ((StFieldCharged != 0) && (Row.PartnerKey == StFieldCharged))
                    {
                        FMainDS.PcAttendee.Rows[Counter][PmShortTermApplicationTable.GetStFieldChargedDBName()] = Row.PartnerShortName;

                        if (PartnersFound == 3)
                        {
                            break;
                        }
                        else
                        {
                            PartnersFound++;
                        }
                    }

                    // display partner short name rather than the partner key
                    if ((StConfirmedOption != 0) && (Row.PartnerKey == StConfirmedOption))
                    {
                        FMainDS.PcAttendee.Rows[Counter][PmShortTermApplicationTable.GetStCurrentFieldDBName()] = Row.PartnerShortName;

                        if (PartnersFound == 3)
                        {
                            break;
                        }
                        else
                        {
                            PartnersFound++;
                        }
                    }

                    // display partner short name rather than the partner key
                    if ((HomeOfficeKey != 0) && (Row.PartnerKey == HomeOfficeKey))
                    {
                        FMainDS.PcAttendee.Rows[Counter][PcAttendeeTable.GetHomeOfficeKeyDBName()] = Row.PartnerShortName;

                        if (!(Row.PartnerShortName.Length > 0))
                        {
                        }

                        if (PartnersFound == 3)
                        {
                            break;
                        }
                        else
                        {
                            PartnersFound++;
                        }
                    }
                }
            }

            // sort order for grid
            DataView MyDataView = FMainDS.PcAttendee.DefaultView;
            MyDataView.Sort = "p_partner_short_name_c ASC";
            grdAttendees.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        private void RefreshAttendees(Object sender, EventArgs e)
        {
            // refresh attendees
            TRemote.MConference.Conference.WebConnectors.RefreshAttendees(FPartnerKey);

            // reload dataset and grid
            FMainDS = new Ict.Petra.Shared.MConference.Data.ConferenceApplicationTDS();
            InitGridManually();
        }

        private void EditPerson(System.Object sender, EventArgs e)
        {
            TFrmPartnerEdit frm = new TFrmPartnerEdit(FPetraUtilsObject.GetForm());

            frm.SetParameters(TScreenMode.smEdit, GetPartnerKeySelected(), TPartnerEditTabPageEnum.petpPersonnelApplications);
            frm.Show();
        }

        private void GridRowSelected(System.Object sender, EventArgs e)
        {
            btnEditPerson.Enabled = true;
        }

        private long GetPartnerKeySelected()
        {
            return Convert.ToInt64(((DataRowView)grdAttendees.SelectedDataRows[0]).Row[PcAttendeeTable.GetPartnerKeyDBName()]);
        }

        // update the record counter
        private void UpdateRecordNumberDisplay()
        {
            int RecordCount;

            if (grdAttendees.DataSource != null)
            {
                RecordCount = ((DevAge.ComponentModel.BoundDataView)grdAttendees.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
        }
    }
}