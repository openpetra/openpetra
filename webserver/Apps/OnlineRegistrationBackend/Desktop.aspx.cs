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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;

using Ext.Net;
using Jayrock.Json.Conversion;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using PetraWebService;
using Ict.Petra.Server.MConference.Applications;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Person.Cacheable;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.WebServer.MConference
{
    public partial class TPageOnlineApplication : System.Web.UI.Page
    {
        protected string EventCode = String.Empty;
        protected Int64 EventPartnerKey = -1;

        protected Ext.Net.ComboBox FilterStatus;
        protected Ext.Net.ComboBox FilterRole;
        protected Ext.Net.ComboBox FilterRegistrationOffice;
        protected Ext.Net.FormPanel FormPanel1;
        protected Ext.Net.Store Store1;
        protected Ext.Net.Store StoreRole;
        protected Ext.Net.Store StoreApplicationStatus;
        protected Ext.Net.Store StoreRegistrationOffice;
        protected Ext.Net.Store StoreServiceTeamJob;
        protected Ext.Net.Image Image1;
        protected Ext.Net.FileUploadField FileUploadField1;
        protected Ext.Net.FileUploadField FileUploadField2;
        protected Ext.Net.FileUploadField FileUploadField3;
        protected Ext.Net.ComboBox FileUploadCodePage3;
        protected Ext.Net.TextField txtSearchApplicant;
        protected Ext.Net.GridPanel GridPanel1;
        protected Ext.Net.GridFilters GridFilters1;
        protected Ext.Net.Panel TabRawApplicationData;
        protected Ext.Net.Panel TabApplicantDetails;
        protected Ext.Net.Panel TabFinance;
        protected Ext.Net.Panel TabMedicalLog;
        protected Ext.Net.Panel TabMedicalInfo;
        protected Ext.Net.Panel TabRebukes;
        protected Ext.Net.Panel TabGroups;
        protected Ext.Net.Panel TabPetra;
        protected Ext.Net.Panel TabManualRegistration;
        protected Ext.Net.Panel TabTopFinance;
        protected Ext.Net.Panel TabBoundaries;
        protected Ext.Net.Panel TabMedical;
        protected Ext.Net.Panel TabBadges;
        protected Ext.Net.Panel TabExport;
        protected Ext.Net.Panel TabTopGroups;
        protected Ext.Net.Panel TabTODO;
        protected Ext.Net.TabPanel TabPanelApplication;
        protected Ext.Net.ComboBox JobWish1;
        protected Ext.Net.ComboBox JobWish2;
        protected Ext.Net.ComboBox JobAssigned;
        protected Ext.Net.ComboBox StFieldCharged;
        protected Ext.Net.Checkbox SecondSibling;
        protected Ext.Net.Checkbox CancelledByFinanceOffice;
        protected Ext.Net.TextArea Comment;
        protected Ext.Net.TextArea CommentRegistrationOfficeReadOnly;
        protected Ext.Net.Panel TabServiceTeam;
        protected Ext.Net.Button btnJSONApplication;
        protected Ext.Net.Button btnCreateGiftBatch;
        protected Ext.Net.Button btnLoadRefreshApplicants;
        protected Ext.Net.Button btnTestPrintBadges;
        protected Ext.Net.Button btnPrintBadges;
        protected Ext.Net.Button btnReprintBadges;
        protected Ext.Net.Button btnReprintPDF;
        protected Ext.Net.Button btnReprintBadge;
        protected Ext.Net.Button btnPrintMedicalReport;
        protected Ext.Net.Button btnExportTShirtNumbers;
        protected Ext.Net.Button btnImportPrintedBadges;
        protected Ext.Net.Button btnExcelArrivalRegistration;
        protected Ext.Net.Button btnExcelRolesPerCountry;
        protected Ext.Net.Button btnFixArrivalDepartureDates;
        protected Ext.Net.Button btnManualRegistration;
        protected Ext.Net.Button btnPrintArrivalRegistration;
        protected Ext.Net.Store StoreRebukes;
        protected Ext.Net.Button btnNewRebuke;
        protected Ext.Net.DateField dtpRebukesReportForDate;
        protected Ext.Net.DateField dtpMedicalReportForDate;
        protected Ext.Net.TabPanel MedicalPanel;
        protected Ext.Net.TextField MaxGroupMembers;
        protected Ext.Net.TextArea GroupMembers;
        protected Ext.Net.TextField StFgCode;
        protected Ext.Net.TextField GroupWish;

        protected bool ConferenceOrganisingOffice = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check for valid user
            TOpenPetraOrg myServer = new TOpenPetraOrg();

            if (!myServer.IsUserLoggedIn())
            {
                this.Response.Redirect("Default.aspx");
                return;
            }

            if (TLogging.DebugLevel > 1)
            {
                TLogging.Log("desktop constructor userid " + UserInfo.GUserInfo.UserID);
            }

            EventCode = TAppSettingsManager.GetValue("ConferenceTool.EventCode");
            EventPartnerKey = TAppSettingsManager.GetInt64("ConferenceTool.EventPartnerKey");

            if (!X.IsAjaxRequest)
            {
                Session["CURRENTROW"] = null;
                RoleData_Refresh(null, null);
                ApplicationStatus_Refresh(null, null);
                RegistrationOffice_Refresh(null, null);
                ServiceTeamJobs_Refresh(null, null);

                if (ConferenceOrganisingOffice)
                {
                    // for the organising office, only show the accepted applicants by default.
                    FilterStatus.SelectedItem.Value = "accepted";
                }

                btnJSONApplication.Visible = ConferenceOrganisingOffice;
                btnLoadRefreshApplicants.Visible = false;
                btnPrintBadges.Visible = ConferenceOrganisingOffice;
                btnReprintBadges.Visible = ConferenceOrganisingOffice;
                btnExportTShirtNumbers.Visible = ConferenceOrganisingOffice;
                btnImportPrintedBadges.Visible = ConferenceOrganisingOffice;
                btnExcelArrivalRegistration.Visible = ConferenceOrganisingOffice;
                btnPrintArrivalRegistration.Visible = ConferenceOrganisingOffice;
                btnExcelRolesPerCountry.Visible = ConferenceOrganisingOffice;
                btnFixArrivalDepartureDates.Visible = ConferenceOrganisingOffice;
                // btnManualRegistration.Visible = ConferenceOrganisingOffice;

                // for the moment, do not confuse all offices with this button
                btnCreateGiftBatch.Visible = ConferenceOrganisingOffice;

                if (!UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    TabMedicalLog.Enabled = false;
                    TabMedicalLog.Visible = false;
                    TabMedicalInfo.Enabled = false;
                    TabMedicalInfo.Visible = false;
                    TabMedical.Visible = false;
                    btnPrintMedicalReport.Visible = false;
                }

                if (UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    TabRawApplicationData.Visible = false;
                    TabFinance.Visible = false;
                    TabServiceTeam.Visible = false;
                    TabApplicantDetails.Visible = false;
                    TabGroups.Visible = false;

                    TabPetra.Visible = false;
                    TabManualRegistration.Visible = false;
                    TabTopFinance.Visible = false;
                    TabBoundaries.Visible = false;
                    TabBadges.Visible = false;
                    TabExport.Visible = false;
                    TabTopGroups.Visible = false;
                    TabTODO.Visible = false;

                    btnReprintBadge.Visible = false;
                    btnReprintPDF.Visible = false;
                }

                if (UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                    TabRawApplicationData.Visible = false;
                    TabFinance.Visible = false;
                    TabServiceTeam.Visible = false;
                    TabApplicantDetails.Visible = false;
                    TabGroups.Visible = false;

                    TabPetra.Visible = false;
                    TabManualRegistration.Visible = false;
                    TabTopFinance.Visible = false;
                    TabMedical.Visible = false;
                    TabBadges.Visible = false;
                    TabExport.Visible = false;
                    TabTopGroups.Visible = false;
                    TabTODO.Visible = false;

                    btnReprintBadge.Visible = false;
                    btnReprintPDF.Visible = false;
                }

                if (!UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                    TabRebukes.Enabled = false;
                    TabRebukes.Visible = false;
                    btnNewRebuke.Visible = false;
                }

                DateTime PrintDate = DateTime.Today;

                if (PrintDate.Hour < 8)
                {
                    // if rebukes are printed in the early morning, print of last day
                    PrintDate.AddDays(-1);
                }

                // users in group HEADSET will only see the headset window
                if (UserInfo.GUserInfo.IsInModule("HEADSET"))
                {
                    X.Js.Call("#{winHeadsets}.show()");
                }
                else
                {
                    X.Js.Call("#{winApplications}.show()");
                }

                dtpRebukesReportForDate.Value = PrintDate;
                dtpMedicalReportForDate.Value = PrintDate;

                MyData_Refresh(null, null);
            }
        }

        private object[] DataTableToArray(DataTable ATable, object AEmptyValue, string AEmptyString)
        {
            ArrayList Result = new ArrayList();

            if (AEmptyString != null)
            {
                object[] NewRow = new object[ATable.Columns.Count];
                NewRow[0] = AEmptyValue;
                NewRow[1] = AEmptyString;
                Result.Add(NewRow);
            }

            foreach (DataRowView row in ATable.DefaultView)
            {
                object[] NewRow = new object[ATable.Columns.Count];

                for (int count = 0; count < ATable.Columns.Count; count++)
                {
                    NewRow[count] = row[count];
                }

                Result.Add(NewRow);
            }

            return Result.ToArray();
        }

        private object[] DataTableToArray(DataTable ATable)
        {
            return DataTableToArray(ATable, null, null);
        }

        /// returns -1 if no office is selected
        private Int64 GetSelectedRegistrationOffice()
        {
            Int64 RegistrationOffice = -1;

            try
            {
                RegistrationOffice = Convert.ToInt64(this.FilterRegistrationOffice.SelectedItem.Value);

                if (RegistrationOffice == 0)
                {
                    RegistrationOffice = -1;
                }
            }
            catch (Exception)
            {
            }

            return RegistrationOffice;
        }

        /// returns null if no role is selected
        private String GetSelectedRole()
        {
            String Role = null;

            try
            {
                Role = this.FilterRole.SelectedItem.Value.ToString();

                if (this.FilterRole.SelectedIndex == 0)
                {
                    Role = null;
                }
            }
            catch (Exception)
            {
            }

            return Role;
        }

        /// load data from the database
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            // TODO get the current sitekey of the user
            // TODO offer all available conferences???
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            if ((CurrentApplicants == null) || (sender != null) || (Session["CURRENTROW"] == null))
            {
                try
                {
                    CurrentApplicants = new ConferenceApplicationTDS();
                    TApplicationManagement.GetApplications(
                        ref CurrentApplicants,
                        EventPartnerKey,
                        EventCode,
                        this.FilterStatus.SelectedItem.Value,
                        GetSelectedRegistrationOffice(),
                        GetSelectedRole(),
                        true);
                }
                catch (Exception exc)
                {
                    TLogging.Log(exc.ToString());
                }

                Session["CURRENTAPPLICANTS"] = CurrentApplicants;
                this.FormPanel1.SetValues(new { });
                this.FormPanel1.Disabled = true;
            }

            try
            {
                this.Store1.DataSource = DataTableToArray(CurrentApplicants.ApplicationGrid);
                this.Store1.DataBind();
            }
            catch (Exception ex)
            {
                TLogging.Log("Exception in MyData_Refresh");
                TLogging.Log(ex.Message);
                TLogging.Log(ex.StackTrace);
            }
        }

        protected void RoleData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            TPersonnelCacheable cache = new TPersonnelCacheable();
            Type dummy;
            PtCongressCodeTable roleTable = (PtCongressCodeTable)cache.GetCacheableTable(TCacheablePersonTablesEnum.EventRoleList,
                String.Empty, true, out dummy);

            roleTable.DefaultView.Sort = PtCongressCodeTable.GetCodeDBName();

            if (roleTable.DefaultView.Find("TS-TEEN-A") != -1)
            {
                PtCongressCodeRow AllTeenagers = roleTable.NewRowTyped();

                AllTeenagers.Code = "TS-TEEN";
                AllTeenagers.Description = "all teenagers";
                roleTable.Rows.Add(AllTeenagers);
            }

            this.StoreRole.DataSource = DataTableToArray(roleTable, "All", "All");

            this.StoreRole.DataBind();
        }

        protected void ChangeFilter(object sender, DirectEventArgs e)
        {
            Session["CURRENTROW"] = null;
            MyData_Refresh(null, null);
        }

        protected void SearchApplicant(object sender, DirectEventArgs e)
        {
            NumericFilter nfRegistrationKey = (NumericFilter)GridFilters1.Filters[0];
            NumericFilter nfPersonKey = (NumericFilter)GridFilters1.Filters[1];
            StringFilter sfFamilyName = (StringFilter)GridFilters1.Filters[2];
            StringFilter sfFirstName = (StringFilter)GridFilters1.Filters[3];
            StringFilter sfFGroupName = (StringFilter)GridFilters1.Filters[4];

            if (txtSearchApplicant.Text.Length == 0)
            {
                nfRegistrationKey.SetActive(false);
                nfPersonKey.SetActive(false);
                sfFamilyName.SetActive(false);
                sfFirstName.SetActive(false);
                sfFGroupName.SetActive(false);
                return;
            }

            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            try
            {
                Int64 Partnerkey = Convert.ToInt64(txtSearchApplicant.Text);

                if (Partnerkey < 1000000)
                {
                    // TODO: use sitekey?
                    Partnerkey += 4000000;
                }

                if (Math.Abs((Int64)(float)Partnerkey - Partnerkey) >= 1)
                {
                    // for some reason, the conversion to float changes the value by one
                    nfRegistrationKey.SetValue(Partnerkey - 2, Partnerkey + 2);
                }
                else
                {
                    nfRegistrationKey.SetValue(Partnerkey);
                }

                sfFamilyName.SetActive(false);
                sfFirstName.SetActive(false);
                nfPersonKey.SetActive(false);
                nfRegistrationKey.SetActive(true);
                sfFGroupName.SetActive(false);

                CurrentApplicants.ApplicationGrid.DefaultView.RowFilter =
                    ConferenceApplicationTDSApplicationGridTable.GetPartnerKeyDBName() + " = " + Partnerkey.ToString();

                if (CurrentApplicants.ApplicationGrid.DefaultView.Count == 0)
                {
                    // search for the person key from the home office
                    if (Math.Abs((Int64)(float)Partnerkey - Partnerkey) >= 1)
                    {
                        // for some reason, the conversion to float changes the value by one
                        nfPersonKey.SetValue(Partnerkey - 2, Partnerkey + 2);
                    }
                    else
                    {
                        nfPersonKey.SetValue(Partnerkey);
                    }

                    sfFamilyName.SetActive(false);
                    sfFirstName.SetActive(false);
                    nfRegistrationKey.SetActive(false);
                    nfPersonKey.SetActive(true);
                    sfFGroupName.SetActive(false);
                }
            }
            catch
            {
                bool found = false;

                // search for a family name starting with this text
                CurrentApplicants.ApplicationGrid.DefaultView.RowFilter =
                    ConferenceApplicationTDSApplicationGridTable.GetFamilyNameDBName() + " LIKE '*" + txtSearchApplicant.Text + "*'";

                if (CurrentApplicants.ApplicationGrid.DefaultView.Count != 0)
                {
                    found = true;
                    sfFamilyName.SetValue(txtSearchApplicant.Text);
                    sfFamilyName.SetActive(true);
                    sfFirstName.SetActive(false);
                    nfPersonKey.SetActive(false);
                    nfRegistrationKey.SetActive(false);
                    sfFGroupName.SetActive(false);
                }

                if (!found)
                {
                    // search for a group name with this text
                    CurrentApplicants.ApplicationGrid.DefaultView.RowFilter =
                        ConferenceApplicationTDSApplicationGridTable.GetStFgCodeDBName() + " LIKE '*" + txtSearchApplicant.Text + "*'";

                    if (CurrentApplicants.ApplicationGrid.DefaultView.Count != 0)
                    {
                        found = true;
                        sfFGroupName.SetValue(txtSearchApplicant.Text);
                        sfFamilyName.SetActive(false);
                        sfFirstName.SetActive(false);
                        nfPersonKey.SetActive(false);
                        nfRegistrationKey.SetActive(false);
                        sfFGroupName.SetActive(true);
                    }
                }

                if (!found)
                {
                    // search for first name
                    CurrentApplicants.ApplicationGrid.DefaultView.RowFilter =
                        ConferenceApplicationTDSApplicationGridTable.GetFirstNameDBName() + " LIKE '*" + txtSearchApplicant.Text + "*'";

                    if (CurrentApplicants.ApplicationGrid.DefaultView.Count != 0)
                    {
                        found = true;
                        sfFirstName.SetValue(txtSearchApplicant.Text);
                        sfFamilyName.SetActive(false);
                        sfFirstName.SetActive(true);
                        nfPersonKey.SetActive(false);
                        nfRegistrationKey.SetActive(false);
                        sfFGroupName.SetActive(false);
                    }
                }
            }

            CurrentApplicants.ApplicationGrid.DefaultView.RowFilter = "";
        }

        protected void ServiceTeamJobs_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            string[] availableJobs = new string[] {
                "Kitchen", "Kiosk", "StaffCafe", "FunFood", "FruitStand", "Cocktail Lounge", "CoffeeBar", "Information", "TS Office",
                "Technical Support", "CleanStreet", "Security", "MainHall Security", "OmniVision", "Medical Team", "Sports Team", "CyberCafe",
                "KidsStreet", "InBetweens", "ToddlerStreet", "BookShop", "Communications", "Headset Team", "Outreach", "RAG", "Dayvisitors",
                "ArtZone",
                "Others"
            };

            List <object[]>datasource = new List <object[]>();

            foreach (string job in availableJobs)
            {
                datasource.Add(new object[] { job });
            }

            StoreServiceTeamJob.DataSource = datasource.ToArray();

            StoreServiceTeamJob.DataBind();
        }

        protected void ApplicationStatus_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            TPersonnelCacheable cache = new TPersonnelCacheable();
            Type dummy;
            PtApplicantStatusTable statusTable = (PtApplicantStatusTable)cache.GetCacheableTable(TCacheablePersonTablesEnum.ApplicantStatusList,
                String.Empty, true, out dummy);

            this.StoreApplicationStatus.DataSource = DataTableToArray(statusTable);

            this.StoreApplicationStatus.DataBind();
        }

        protected void RegistrationOffice_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();

            offices.DefaultView.Sort = PPartnerTable.GetPartnerShortNameDBName();

            ConferenceOrganisingOffice = TApplicationManagement.IsConferenceOrganisingOffice();

            Session["CONFERENCEORGANISINGOFFICE"] = ConferenceOrganisingOffice;

            if (offices.Count > 0)
            {
                this.StoreRegistrationOffice.DataSource = DataTableToArray(offices, -1, "All");
            }
            else
            {
                this.StoreRegistrationOffice.DataSource = DataTableToArray(offices);
            }

            this.StoreRegistrationOffice.DataBind();
        }

        /// to avoid the error on the ext.js client: Status Text: BADRESPONSE: Parse Error
        private object ReplaceQuotes(object value)
        {
            if (value.GetType() == typeof(string))
            {
                return value.ToString().Replace("&quot;", "\\\"");
            }
            else
            {
                return value;
            }
        }

        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                Int64 PartnerKey = Convert.ToInt64(e.ExtraParams["PartnerKey"]);
                Int32 ApplicationKey = Convert.ToInt32(e.ExtraParams["ApplicationKey"]);
                ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];
                ConferenceOrganisingOffice = Convert.ToBoolean(Session["CONFERENCEORGANISINGOFFICE"]);

                System.Data.DataView ApplicationView = CurrentApplicants.ApplicationGrid.DefaultView;

                ConferenceApplicationTDSApplicationGridRow row =
                    (ConferenceApplicationTDSApplicationGridRow)ApplicationView[ApplicationView.Find(new object[] { PartnerKey,
                                                                                                                    ApplicationKey })].Row;
                Session["CURRENTROW"] = row;

                this.FormPanel1.Disabled = false;

                string RawData = TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice);
                Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(RawData);

                if (UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    LoadDataForMedicalTeam(row, rawDataObject);
                }
                else if (UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                }
                else
                {
                    TabRawApplicationData.Html = TJsonTools.DataToHTMLTable(RawData);

                    Ext.Net.Panel panel = this.X().Panel()
                                          .ID("TabMoreDetails")
                                          .Title("Edit more details")
                                          .Padding(5)
                                          .AutoScroll(true);
                    panel.Render("TabPanelApplication", 4, RenderMode.InsertTo);

                    Ext.Net.Label label = this.X().Label()
                                          .ID("lblWarningEdit")
                                          .Html(
                        "<b>Please be very careful</b>, only edit data if you are sure. Drop Down boxes or Dates might not work anymore.<br/><br/>");

                    label.Render("TabMoreDetails", RenderMode.AddTo);
                }

                var dictionary = new Dictionary <string, object>();
                dictionary.Add("PartnerKey", row.PartnerKey);
                dictionary.Add("PersonKey", row.IsPersonKeyNull() ? "" : row.PersonKey.ToString());
                dictionary.Add("FirstName", ReplaceQuotes(row.FirstName));
                dictionary.Add("FamilyName", ReplaceQuotes(row.FamilyName));
                dictionary.Add("Gender", row.Gender);
                dictionary.Add("DateOfBirth", row.DateOfBirth);
                dictionary.Add("DateOfArrival", row.DateOfArrival);
                dictionary.Add("DateOfDeparture", row.DateOfDeparture);
                dictionary.Add("GenAppDate", row.GenAppDate);
                dictionary.Add("GenApplicationStatus", row.GenApplicationStatus);
                dictionary.Add("StCongressCode", row.StCongressCode);
                dictionary.Add("Comment", ReplaceQuotes(row.Comment));
                dictionary.Add("StFgLeader", row.StFgLeader);
                dictionary.Add("StFgCode", row.StFgCode);
                dictionary.Add("StFieldCharged", row.StFieldCharged);
                dictionary.Add("MedicalLog", row.MedicalNotes);

                List <string>FieldsOnFirstTab = new List <string>(new string[] {
                                                                      "TShirtStyle", "TShirtSize", "JobWish1", "JobWish2", "JobAssigned",
                                                                      "SecondSibling", "CancelledByFinanceOffice", "GroupWish", "DateOfArrival",
                                                                      "DateOfDeparture"
                                                                  });

                foreach (string key in rawDataObject.Names)
                {
                    if (!dictionary.ContainsKey(key)
                        && FieldsOnFirstTab.Contains(key))
                    {
                        dictionary.Add(key, ReplaceQuotes(rawDataObject[key]));

                        if (((key == "SecondSibling") || (key == "CancelledByFinanceOffice"))
                            && (rawDataObject[key].ToString().ToLower() == "true"))
                        {
                            // CheckBox: need to set the name of the checkbox as value
                            dictionary[key] = key;
                        }
                    }
                }

                List <string>FieldsNotToBeEdited = new List <string>(new string[] {
                                                                         "Role", "FormsId", "EventIdentifier", "RegistrationOffice", "LastName",
                                                                         "RegistrationCountryCode", "ImageID",
                                                                         "CLS", "Dresscode", "LegalImprint"
                                                                     });

                if (UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                    RefreshRebukesStore(row.RebukeNotes);
                }
                else if (!UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    foreach (string key in rawDataObject.Names)
                    {
                        if (!dictionary.ContainsKey(key)
                            && !FieldsNotToBeEdited.Contains(key)
                            && !FieldsOnFirstTab.Contains(key)
                            && !key.EndsWith("_SelIndex")
                            && !key.EndsWith("_Value")
                            && !key.EndsWith("_ActiveTab"))
                        {
                            dictionary.Add(key, ReplaceQuotes(rawDataObject[key]));

                            if (rawDataObject[key].ToString().Length > 40)
                            {
                                TextArea text = this.X().TextArea()
                                                .ID(key)
                                                .LabelWidth(200)
                                                .Width(700)
                                                .Height(150)
                                                .FieldLabel(key);

                                text.Render("TabMoreDetails", RenderMode.AddTo);
                            }
                            else
                            {
                                TextField text = this.X().TextField()
                                                 .ID(key)
                                                 .LabelWidth(200)
                                                 .Width(500)
                                                 .FieldLabel(key);

                                text.Render("TabMoreDetails", RenderMode.AddTo);
                            }
                        }
                    }
                }

                JobWish1.ClearValue();
                JobWish2.ClearValue();
                JobAssigned.ClearValue();
                SecondSibling.Clear();
                CancelledByFinanceOffice.Clear();

                if (rawDataObject.Contains("RegistrationCountryCode") && (rawDataObject["RegistrationCountryCode"].ToString() == "sv-SE"))
                {
                    X.Js.Call("SetDateFormat", "Y-m-d");
                }
                else
                {
                    X.Js.Call("SetDateFormat", "d-m-Y");
                }

                // SetValues: new {}; anonymous type: http://msdn.microsoft.com/en-us/library/bb397696.aspx
                // instead a Dictionary can be used as well
                // See also StackOverflow ObjectExtensions::ToDictionary for converting an anonymous type to a dictionary
                this.FormPanel1.SetValues(dictionary);

                CommentRegistrationOfficeReadOnly.Text = row.Comment;

                // only allow the logistics office to assign jobs
                JobAssigned.ReadOnly = !ConferenceOrganisingOffice;
                StFieldCharged.ReadOnly = !ConferenceOrganisingOffice;

                // ReadOnly does not seem to work to make the checkbox readonly. use disabled instead.
                CancelledByFinanceOffice.Disabled = !UserInfo.GUserInfo.IsInModule("FINANCE-3");

                // only show service team panel for TS-STAFF and TS-SERVE
                if ((row.StCongressCode == "TS-STAFF") || (row.StCongressCode == "TS-SERVE"))
                {
                    X.Js.Call("ShowTabPanel", "TabServiceTeam");
                }
                else
                {
                    X.Js.Call("HideTabPanel", "TabServiceTeam");
                }

                Random rand = new Random();
                Image1.ImageUrl = "photos.aspx?id=" + PartnerKey.ToString() + ".jpg&" + rand.Next(1, 10000).ToString();

                GroupMembers.Text = string.Empty;
            }
            catch (Exception ex)
            {
                TLogging.Log("Exception in RowSelect: " + ex.ToString() + " " + ex.Message);
                TLogging.Log(ex.StackTrace);
            }
        }

        protected void ShowRawApplicationData(object sender, DirectEventArgs e)
        {
            // the data is already displayed in RowSelect
        }

        protected void SaveApplication(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];

            //Console.WriteLine(e.ExtraParams["Values"]);

            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);
            string RebukeValues = string.Empty;

            try
            {
                RebukeValues = e.ExtraParams["RebukeValues"].ToString();
            }
            catch (Exception)
            {
            }

            try
            {
                if (UserInfo.GUserInfo.IsInModule("MEDICAL"))
                {
                    row.MedicalNotes = GetMedicalLogsFromScreen(values);
                }
                else if (UserInfo.GUserInfo.IsInModule("BOUNDARIES"))
                {
                    row.RebukeNotes = RebukeValues;
                }
                else
                {
                    string RawData = TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice);
                    Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(RawData);

                    if (!rawDataObject.Contains("RegistrationCountryCode"))
                    {
                        // some of the late registrations do not have a country code
                        rawDataObject.Put("RegistrationCountryCode", "en-GB");
                    }

                    CultureInfo OrigCulture = Catalog.SetCulture(rawDataObject["RegistrationCountryCode"].ToString());

                    row.FamilyName = values["FamilyName"];
                    row.FirstName = values["FirstName"];
                    row.Gender = values["Gender"];

                    if (!values.ContainsKey("DateOfBirth") || (values["DateOfBirth"].Length == 0))
                    {
                        row.DateOfBirth = new Nullable <DateTime>();
                    }
                    else
                    {
                        // avoid problems with different formatting of dates, could cause parsing errors later, into the typed class
                        // Problem: Mär und Mrz. caused by javascript, ext.js?
                        values["DateOfBirth"] = values["DateOfBirth"].ToString().Replace("Mär", "Mrz");
                        values["DateOfBirth"] = Convert.ToDateTime(values["DateOfBirth"]).ToShortDateString();
                        row.DateOfBirth = Convert.ToDateTime(values["DateOfBirth"]);
                    }

                    if (!values.ContainsKey("DateOfArrival") || (values["DateOfArrival"].Length == 0))
                    {
                        row.SetDateOfArrivalNull();
                    }
                    else
                    {
                        values["DateOfArrival"] = Convert.ToDateTime(values["DateOfArrival"]).ToShortDateString();
                        row.DateOfArrival = Convert.ToDateTime(values["DateOfArrival"]);
                    }

                    if (!values.ContainsKey("DateOfDeparture") || (values["DateOfDeparture"].Length == 0))
                    {
                        row.SetDateOfDepartureNull();
                    }
                    else
                    {
                        values["DateOfDeparture"] = Convert.ToDateTime(values["DateOfDeparture"]).ToShortDateString();
                        row.DateOfDeparture = Convert.ToDateTime(values["DateOfDeparture"]);
                    }

                    row.GenAppDate = Convert.ToDateTime(values["GenAppDate"]);
                    row.StCongressCode = values["StCongressCode_Value"];
                    row.GenApplicationStatus = values["GenApplicationStatus_Value"];
                    row.StFgLeader = values.ContainsKey("StFgLeader");
                    row.StFgCode = values.ContainsKey("StFgCode") ? values["StFgCode"] : string.Empty;

                    if (values.ContainsKey("StFieldCharged_Value"))
                    {
                        row.StFieldCharged = Convert.ToInt64(values["StFieldCharged_Value"]);
                    }

                    row.Comment = values["Comment"];

                    bool SecondSibling = false;
                    bool CancelledByFinanceOffice = false;

                    foreach (string key in values.Keys)
                    {
                        // TODO: typecast dates?
                        object value = values[key];

                        if (rawDataObject.Contains(key))
                        {
                            rawDataObject[key] = value;
                        }
                        else if (key.EndsWith("_ActiveTab")
                                 || key.EndsWith("_SelIndex")
                                 || key.EndsWith("_Value"))
                        {
                            // do not add all values, eg. _Value or _SelIndex for comboboxes or _ActiveTab, cause trouble otherwise
                        }
                        else if (key == "JobAssigned")
                        {
                            rawDataObject.Put(key, value);
                        }
                        else if (key == "SecondSibling")
                        {
                            SecondSibling = true;
                            rawDataObject.Put(key, true);
                        }
                        else if (key == "CancelledByFinanceOffice")
                        {
                            CancelledByFinanceOffice = true;
                            rawDataObject.Put(key, true);
                        }
                    }

                    if (!SecondSibling)
                    {
                        rawDataObject.Put("SecondSibling", false);
                    }

                    if (!CancelledByFinanceOffice)
                    {
                        rawDataObject.Put("CancelledByFinanceOffice", false);
                    }

                    rawDataObject["Role"] = values["StCongressCode_Value"];
                    rawDataObject["SecondSibling"] = values.ContainsKey("SecondSibling");
                    rawDataObject["CancelledByFinanceOffice"] = values.ContainsKey("CancelledByFinanceOffice");

                    row.JSONData = TJsonTools.ToJsonString(rawDataObject);

                    Catalog.SetCulture(OrigCulture);
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                X.Msg.Alert("Error", "Saving did not work").Show();
                return;
            }

            TVerificationResultCollection VerificationResult;

            if (TApplicationManagement.SaveApplication(EventCode, row, out VerificationResult) != TSubmitChangesResult.scrOK)
            {
                X.Msg.Alert("Error", "Saving did not work: " + VerificationResult.BuildVerificationResultString()).Show();
            }

            // save some time? user can click refresh himself.
            // MyData_Refresh(null, null);
        }

        protected void ReprintPDF(object sender, DirectEventArgs e)
        {
            try
            {
                ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];
                string RawData = TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice);

                TApplicationFormData data = (TApplicationFormData)TJsonTools.ImportIntoTypedStructure(
                    typeof(TApplicationFormData),
                    RawData);
                data.RawData = RawData;

                string pdfIdentifier;
                string pdfFilename = TImportPartnerForm.GeneratePDF(row.PartnerKey, data.registrationcountrycode, data, out pdfIdentifier);
                try
                {
                    // TImportPartnerForm.SendEmail(row.PartnerKey, data.registrationcountrycode, data, pdfFilename);
                }
                catch (Exception ex)
                {
                    TLogging.Log(ex.Message);
                    TLogging.Log(ex.StackTrace);
                }
                TLogging.Log(pdfFilename);
                this.Response.Clear();
                this.Response.ContentType = "application/pdf";
                this.Response.AddHeader("Content-Type", "application/pdf");
                this.Response.AddHeader("Content-Length", (new FileInfo(pdfFilename)).Length.ToString());
                this.Response.AddHeader("Content-Disposition", "attachment; filename=" + row.PartnerKey.ToString() + ".pdf");
                this.Response.WriteFile(pdfFilename);
                this.Response.Flush();
                // this.Response.End(); avoid System.Threading.ThreadAbortException
            }
            catch (Exception ex2)
            {
                TLogging.Log(ex2.Message);
                TLogging.Log(ex2.StackTrace);
                throw;
            }
        }

        protected void ReprintBadge(object sender, DirectEventArgs e)
        {
            try
            {
                ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];

                string PDFPath = TConferenceBadges.ReprintBadge(EventPartnerKey,
                    EventCode,
                    row.PartnerKey);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=Badge_" +
                        (row.FirstName + "_" + row.FamilyName).Replace(".", "_").Replace(" ", string.Empty) + ".pdf");
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void AcceptManyApplicants(object sender, DirectEventArgs e)
        {
            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);
            string[] RegistrationKeysString = values["RegistrationsKeys"].ToString().Split(new char[] { '\n', '\r' });

            List <Int64>RegistrationKeys = new List <Int64>();

            foreach (string key in RegistrationKeysString)
            {
                if (key.Length > 0)
                {
                    try
                    {
                        Int64 registrationKey = Convert.ToInt64(key);

                        if (registrationKey < 1000000)
                        {
                            registrationKey += 4000000;
                        }

                        RegistrationKeys.Add(registrationKey);
                    }
                    catch (Exception ex)
                    {
                        TLogging.Log(ex.Message);
                        X.Msg.Alert("Error", "Problem with key " + key).Show();
                        return;
                    }
                }
            }

            ConferenceApplicationTDS CurrentApplicants = new ConferenceApplicationTDS();
            TApplicationManagement.GetApplications(
                ref CurrentApplicants,
                EventPartnerKey,
                EventCode,
                "all", -1, null, true);

            // first do a test run to test the keys
            List <Int64>RegistrationKeysBackup = new List <Int64>();
            RegistrationKeysBackup.AddRange(RegistrationKeys);

            foreach (ConferenceApplicationTDSApplicationGridRow row in CurrentApplicants.ApplicationGrid.Rows)
            {
                if (RegistrationKeysBackup.Contains(row.PartnerKey))
                {
                    RegistrationKeysBackup.Remove(row.PartnerKey);
                }
            }

            if (RegistrationKeysBackup.Count > 0)
            {
                string list = String.Empty;

                foreach (Int64 key in RegistrationKeysBackup)
                {
                    list += key.ToString() + Environment.NewLine;
                }

                X.Msg.Alert("Error", "Some keys are wrong, nothing was changed: " + Environment.NewLine + list).Show();
                return;
            }

            foreach (ConferenceApplicationTDSApplicationGridRow row in CurrentApplicants.ApplicationGrid.Rows)
            {
                if (RegistrationKeys.Contains(row.PartnerKey))
                {
                    row.GenApplicationStatus = "A";
                }
            }

            if (TApplicationManagement.SaveApplications(ref CurrentApplicants) != TSubmitChangesResult.scrOK)
            {
                X.Msg.Alert("Error", "Saving did not work").Show();
            }

            MyData_Refresh(null, null);
        }

        protected void DownloadPetra(object sender, StoreSubmitDataEventArgs e)
        {
            string csvLines = TApplicationManagement.DownloadApplications(EventPartnerKey, EventCode, GetSelectedRegistrationOffice(), true);

            this.Response.Clear();
            // TODO: this is a problem with old Petra 2.x, importing ANSI only
            this.Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");

            // Some browsers (Safari on Mac?) process the file and confuse the separators
            //this.Response.ContentType = "application/csv";

            this.Response.ContentType = "text/plain";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=petra-import.csv");
            this.Response.Write(csvLines);
            this.Response.End();
        }

        protected void DownloadExcel(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            this.Response.Clear();
            this.Response.ContentType = "application/xlsx";
            this.Response.AddHeader("Content-Type", "application/xlsx");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=Applicants.xlsx");
            MemoryStream m = new MemoryStream();
            TApplicationManagement.DownloadApplications(EventPartnerKey, EventCode, ref CurrentApplicants, m);
            m.WriteTo(this.Response.OutputStream);
            m.Close();
            this.Response.End();
        }

        protected void LoadRefreshApplicants(object sender, DirectEventArgs e)
        {
            TAttendeeManagement.RefreshAttendees(EventPartnerKey, EventCode);
        }

        protected void PrintBadges(bool AReprintPrinted, bool ADoNotReprint)
        {
            string OutputName = "badges.pdf";

            try
            {
                OutputName = (this.FilterRegistrationOffice.SelectedItem.Text + "_" + this.FilterRole.SelectedItem.Text).Replace(" ", "_") + ".pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TConferenceBadges.PrintBadges(EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice(),
                    GetSelectedRole(),
                    AReprintPrinted,
                    ADoNotReprint);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception e)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = e.Message
                    });
            }
        }

        protected void PrintBadges(object sender, DirectEventArgs e)
        {
            PrintBadges(false, true);
        }

        protected void TestPrintBadges(object sender, DirectEventArgs e)
        {
            PrintBadges(false, false);
        }

        protected void ReprintBadges(object sender, DirectEventArgs e)
        {
            PrintBadges(true, false);
        }

        protected void PrintFinanceReport(object sender, DirectEventArgs e)
        {
            string OutputName = "FinanceReport.pdf";

            try
            {
                OutputName = "FinanceReport_" + this.FilterRegistrationOffice.SelectedItem.Text.Replace(" ", "_") + ".pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TConferenceFinanceReports.PrintFinanceReport(EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice());

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void ClearFilterByRebukes(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            this.Store1.DataSource = DataTableToArray(CurrentApplicants.ApplicationGrid);
            this.Store1.DataBind();
        }

        protected void FilterByRebukes(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            CurrentApplicants.ApplicationGrid.DefaultView.RowFilter =
                ConferenceApplicationTDSApplicationGridTable.GetRebukeNotesDBName() + " <> '' AND " +
                ConferenceApplicationTDSApplicationGridTable.GetRebukeNotesDBName() + " IS NOT NULL";

            if (CurrentApplicants.ApplicationGrid.DefaultView.Count > 0)
            {
                this.Store1.DataSource = DataTableToArray(CurrentApplicants.ApplicationGrid);
                this.Store1.DataBind();
            }

            CurrentApplicants.ApplicationGrid.DefaultView.RowFilter = string.Empty;
        }

        protected void PrintRebukesReport(object sender, DirectEventArgs e)
        {
            DateTime PrintDate = (DateTime)dtpRebukesReportForDate.Value;

            string OutputName = "RebukesReport_" + PrintDate.ToString("yyyy-MM-dd") + ".pdf";

            try
            {
                OutputName = "RebukesReport_" + (this.FilterRegistrationOffice.SelectedItem.Text + "_" + PrintDate.ToString("yyyy-MM-dd")).Replace(
                    ".",
                    "_").Replace(" ", "_") + ".pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TRebukeReport.PrintRebukeReport(EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice(),
                    PrintDate);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void PrintMedicalReportForParticipant(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];

            string OutputName = ("MedicalReport_" + row.FamilyName + "_" + row.FirstName + ".pdf").Replace(" ", "_");

            try
            {
                string PDFPath = TMedicalReport.PrintReport(
                    EventCode,
                    row.PartnerKey);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void PrintMedicalReport(object sender, DirectEventArgs e)
        {
            DateTime PrintDate = (DateTime)dtpMedicalReportForDate.Value;

            string OutputName = "MedicalReport_" + PrintDate.ToString("yyyy-MM-dd") + ".pdf";

            try
            {
                OutputName = "MedicalReport_" + PrintDate.ToString("yyyy-MM-dd").Replace(
                    ".",
                    "_").Replace(" ", "_") + ".pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TMedicalReport.PrintReport(EventPartnerKey,
                    EventCode,
                    PrintDate);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void PrintBarcodeLabels(object sender, DirectEventArgs e)
        {
            string OutputName = "badgeLabels.pdf";

            try
            {
                OutputName =
                    this.FilterRegistrationOffice.SelectedItem.Text.Replace(" ", "_") + "_" + this.FilterRole.SelectedItem.Text + "_Labels.pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TConferenceBadges.PrintBadgeLabels(EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice(),
                    GetSelectedRole(),
                    "BadgeLabel", false);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void PrintArrivalRegistration(object sender, DirectEventArgs e)
        {
            string OutputName = "arrivalsList.pdf";

            try
            {
                OutputName =
                    this.FilterRegistrationOffice.SelectedItem.Text.Replace(" ", "_") + "_" + this.FilterRole.SelectedItem.Text + "_arrivals.pdf";
            }
            catch (Exception)
            {
            }

            try
            {
                string PDFPath = TConferenceBadges.PrintBadgeLabels(EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice(),
                    GetSelectedRole(),
                    "ArrivalsList", true);

                if (File.Exists(PDFPath))
                {
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-Type", "application/pdf");
                    this.Response.AddHeader("Content-Length", (new FileInfo(PDFPath)).Length.ToString());
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + OutputName);
                    this.Response.WriteFile(PDFPath);
                    this.Response.Flush();
                    File.Delete(PDFPath);
                    // this.Response.End(); avoid System.Threading.ThreadAbortException
                }
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = ex.Message
                    });
            }
        }

        protected void FixArrivalDepartureDates(object sender, DirectEventArgs e)
        {
            TConferenceToolsRawData.FixArrivalDepartureDates(EventPartnerKey, EventCode);
        }

        protected void ExportTShirtNumbers(object sender, DirectEventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "application/xlsx";
            this.Response.AddHeader("Content-Type", "application/xlsx");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=TShirtNumbers.xlsx");
            MemoryStream m = new MemoryStream();
            TConferenceFreeTShirt.DownloadTShirtNumbers(EventPartnerKey, EventCode, m);
            m.WriteTo(this.Response.OutputStream);
            m.Close();
            this.Response.End();
        }

        protected void ExportArrivalRegistrationList(object sender, DirectEventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "application/xlsx";
            this.Response.AddHeader("Content-Type", "application/xlsx");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=ArrivalRegistration.xlsx");
            MemoryStream m = new MemoryStream();
            TConferenceExcelReports.DownloadArrivalRegistration(EventPartnerKey, EventCode, m);
            m.WriteTo(this.Response.OutputStream);
            m.Close();
            this.Response.End();
        }

        protected void ExportRolesPerCountry(object sender, DirectEventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "application/xlsx";
            this.Response.AddHeader("Content-Type", "application/xlsx");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=RolesPerCountry.xlsx");
            MemoryStream m = new MemoryStream();
            TConferenceExcelReports.GetNumbersOfRolesPerCountry(EventPartnerKey, EventCode, m);
            m.WriteTo(this.Response.OutputStream);
            m.Close();
            this.Response.End();
        }

        protected void Logout_Click(object sender, DirectEventArgs e)
        {
            // Logout from Authenticated Session
            TOpenPetraOrg myServer = new TOpenPetraOrg();

            myServer.Logout();
            this.Response.Redirect("Default.aspx");
        }

        protected void UploadClick(object sender, DirectEventArgs e)
        {
            try
            {
                Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);

                Int64 PartnerKey = Convert.ToInt64(values["PartnerKey"]);

                if (this.FileUploadField1.HasFile)
                {
                    // TODO: use a generic upload function for images, with max size and dimension parameters, same as in upload.aspx for the participants
                    int FileLen = this.FileUploadField1.PostedFile.ContentLength;

                    if (FileLen > 10 * 1024 * 1024)
                    {
                        X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Fail",
                                Message = "We do not support files greater than 10 MB " + FileLen.ToString()
                            });
                        return;
                    }

                    // TODO: convert to jpg
                    if ((Path.GetExtension(this.FileUploadField1.PostedFile.FileName).ToLower() != ".jpg")
                        && (Path.GetExtension(this.FileUploadField1.PostedFile.FileName).ToLower() != ".jpeg"))
                    {
                        X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Fail",
                                Message = "we only support jpg files"
                            });
                        return;
                    }

                    // TODO: scale image
                    // TODO: rotate image
                    // TODO: allow editing of image, select the photo from a square image etc

                    this.FileUploadField1.PostedFile.SaveAs(TAppSettingsManager.GetValue("Server.PathData") +
                        Path.DirectorySeparatorChar + "photos" + Path.DirectorySeparatorChar + PartnerKey.ToString() + ".jpg");
                    Random rand = new Random();
                    Image1.ImageUrl = "photos.aspx?id=" + PartnerKey.ToString() + ".jpg&" + rand.Next(1, 10000).ToString();

                    // hide wait message, uploading
                    X.Msg.Hide();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Fail",
                            Message = "No file uploaded"
                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void UploadPetraImportClick(object sender, DirectEventArgs e)
        {
            try
            {
                if (this.FileUploadField2.HasFile)
                {
                    int FileLen = this.FileUploadField2.PostedFile.ContentLength;

                    if (Path.GetExtension(this.FileUploadField2.PostedFile.FileName).ToLower() != ".csv")
                    {
                        X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Fail",
                                Message = "we only support the csv files that are written by Petra 2.x"
                            });
                        return;
                    }

                    Random rand = new Random();
                    string filename = string.Empty;

                    do
                    {
                        filename = TAppSettingsManager.GetValue("Server.PathData") +
                                   Path.DirectorySeparatorChar + "petraimports" + Path.DirectorySeparatorChar +
                                   rand.Next(1, 1000000).ToString() + ".csv";
                    } while (File.Exists(filename));

                    this.FileUploadField2.PostedFile.SaveAs(filename);

                    if (!TApplicationManagement.UploadPetraImportResult(filename))
                    {
                        throw new Exception("Problems during Import");
                    }

                    MyData_Refresh(null, null);

                    // hide wait message, uploading
                    X.Msg.Hide();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Fail",
                            Message = "No file uploaded"
                        });
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.Message);

                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Failure",
                        Message = "Problem with upload, no file uploaded"
                    });
            }
        }

        protected void UploadPetraExtractClick(object sender, DirectEventArgs e)
        {
            try
            {
                if (this.FileUploadField3.HasFile)
                {
                    int FileLen = this.FileUploadField3.PostedFile.ContentLength;

                    if (Path.GetExtension(this.FileUploadField3.PostedFile.FileName).ToLower() != ".ext")
                    {
                        X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Fail",
                                Message = "we only support the extract files (.ext) that are written by Petra 2.x"
                            });
                        return;
                    }

                    Random rand = new Random();
                    string filename = string.Empty;

                    do
                    {
                        filename = TAppSettingsManager.GetValue("Server.PathData") +
                                   Path.DirectorySeparatorChar + "petraimports" + Path.DirectorySeparatorChar +
                                   rand.Next(1, 1000000).ToString() + ".ext";
                    } while (File.Exists(filename));

                    TLogging.Log(filename);
                    this.FileUploadField3.PostedFile.SaveAs(filename);
                    TTextFile.ConvertToUnicode(filename, FileUploadCodePage3.SelectedItem.Value);

                    TVerificationResultCollection VerificationResult;

                    if (!TApplicationManagement.UploadPetraExtract(filename, EventCode, out VerificationResult))
                    {
                        X.Msg.Show(new MessageBoxConfig
                            {
                                Buttons = MessageBox.Button.OK,
                                Icon = MessageBox.Icon.ERROR,
                                Title = "Import Failure",
                                Message = VerificationResult.BuildVerificationResultString().Replace("\n", "<br/>")
                            });
                        return;
                    }

                    MyData_Refresh(null, null);

                    // hide wait message, uploading
                    X.Msg.Hide();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Fail",
                            Message = "No file uploaded"
                        });
                }
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.Message);
                TLogging.Log(ex.StackTrace);
                X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Fail",
                        Message = "There has been a problem in the .ext file."
                    });
            }
        }

        /// this is used for applications made by mistake on the demo website, to import into the live application website
        protected void ImportJSONApplication(object sender, DirectEventArgs e)
        {
            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);

            string AJSONFormData = values["JSONData"].ToString();
            string RequiredCulture = string.Empty;
            AJSONFormData = TJsonTools.RemoveContainerControls(AJSONFormData, ref RequiredCulture);

            AJSONFormData = AJSONFormData.Replace("\"txt", "\"").
                            Replace("\"chk", "\"").
                            Replace("\"rbt", "\"").
                            Replace("\"cmb", "\"").
                            Replace("\"hid", "\"").
                            Replace("\"dtp", "\"").
                            Replace("\n", " ").Replace("\r", "");

            TLogging.Log(AJSONFormData);

            try
            {
                // should not be able to create a PDF since the picture is missing, and not send an email
                TImportPartnerForm.DataImportFromForm("RegisterPerson", AJSONFormData);
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.Message);
                TLogging.Log(ex.StackTrace);
            }
        }

        private static int NewRebukeId = 1;

        private void RefreshRebukesStore(string AData)
        {
            List <TRebuke>store = new List <TRebuke>();

            if (AData.Length > 0)
            {
                //string test = "[{\"ID\":1,\"When\":\"2011-07-21T16:55:04\",\"What\":\"\",\"Consequence\":\"TBD\"}]";

                Object obj = Jayrock.Json.Conversion.JsonConvert.Import(AData);

                if (obj is Jayrock.Json.JsonArray)
                {
                    Jayrock.Json.JsonArray list = (Jayrock.Json.JsonArray)obj;

                    foreach (Jayrock.Json.JsonObject element in list)
                    {
                        string time = string.Empty;

                        try
                        {
                            time = element["Time"].ToString();
                        }
                        catch (Exception)
                        {
                        }

                        if (Convert.ToInt32(element["ID"]) >= NewRebukeId)
                        {
                            NewRebukeId = Convert.ToInt32(element["ID"]) + 1;
                        }

                        store.Add(new TRebuke(Convert.ToInt32(element["ID"]),
                                Convert.ToDateTime(element["When"]),
                                time,
                                element["What"].ToString(),
                                element["Consequence"].ToString()));
                    }
                }
            }

            this.StoreRebukes.DataSource = store;
            this.StoreRebukes.DataBind();
        }

        protected void AddNewRebuke(Object sender, DirectEventArgs e)
        {
            NewRebukeId++;
            this.StoreRebukes.AddRecord(new TRebuke(NewRebukeId));
            this.StoreRebukes.CommitChanges();
        }

        protected void AddMedicalIncident(Object sender, DirectEventArgs e)
        {
            Int32 NewIncidentID = (Int32)Session["NewMedicalId"];

            Session["NewMedicalId"] = NewIncidentID + 1;

            AddMedicalIncidentData(new TMedicalIncident(NewIncidentID));
        }

        protected void AddMedicalIncidentData(TMedicalIncident ARow)
        {
            string TabId = "TabMedicalIncident" + ARow.ID.ToString();

            Ext.Net.Panel panel = this.X().Panel()
                                  .ID(TabId)
                                  .Title("incident " + ARow.ID.ToString())
                                  .Padding(5)
                                  .AutoScroll(true);

            Ext.Net.TableLayout tblMedicalIncident = this.X().TableLayout()
                                                     .ID("tblMedicalIncident")
                                                     .Columns(3);

            Ext.Net.DateField dtpDate = this.X().DateField()
                                        .ID("dtpDate" + ARow.ID.ToString())
                                        .Width(300)
                                        .Value(ARow.Date)
                                        .Format("dd-MM-yyyy")
                                        .FieldLabel("Date");

            Ext.Net.Cell cDate = new Cell();
            cDate.ColSpan = 2;
            cDate.Items.Add(dtpDate);
            tblMedicalIncident.Cells.Add(cDate);

            Ext.Net.TextField txtExaminer = this.X().TextField()
                                            .ID("txtExaminer" + ARow.ID.ToString())
                                            .Value(ARow.Examiner)
                                            .FieldLabel("Examiner");

            Ext.Net.Cell cExaminer = new Cell();
            cExaminer.ColSpan = 1;
            cExaminer.Items.Add(txtExaminer);
            tblMedicalIncident.Cells.Add(cExaminer);

            Ext.Net.TextField txtPulse = this.X().TextField()
                                         .ID("txtPulse" + ARow.ID.ToString())
                                         .Value(ARow.Pulse)
                                         .FieldLabel("Pulse");

            Ext.Net.Cell cPulse = new Cell();
            cPulse.Items.Add(txtPulse);
            tblMedicalIncident.Cells.Add(cPulse);

            Ext.Net.TextField txtBloodPressure = this.X().TextField()
                                                 .ID("txtBloodPressure" + ARow.ID.ToString())
                                                 .Value(ARow.BloodPressure)
                                                 .FieldLabel("Blood Pressure");

            Ext.Net.Cell cBloodPressure = new Cell();
            cBloodPressure.Items.Add(txtBloodPressure);
            tblMedicalIncident.Cells.Add(cBloodPressure);

            Ext.Net.TextField txtTemperature = this.X().TextField()
                                               .ID("txtTemperature" + ARow.ID.ToString())
                                               .Value(ARow.Temperature)
                                               .FieldLabel("Temperature");

            Ext.Net.Cell cTemperature = new Cell();
            cTemperature.Items.Add(txtTemperature);
            tblMedicalIncident.Cells.Add(cTemperature);

            Ext.Net.TextArea txtDiagnosis = this.X().TextArea()
                                            .ID("txtDiagnosis" + ARow.ID.ToString())
                                            .Height(100)
                                            .Width(500)
                                            .Value(ARow.Diagnosis)
                                            .FieldLabel("Diagnosis");

            Ext.Net.Cell cDiagnosis = new Cell();
            cDiagnosis.ColSpan = 3;
            cDiagnosis.Items.Add(txtDiagnosis);
            tblMedicalIncident.Cells.Add(cDiagnosis);

            Ext.Net.TextArea txtTherapy = this.X().TextArea()
                                          .ID("txtTherapy" + ARow.ID.ToString())
                                          .Height(100)
                                          .Width(500)
                                          .Value(ARow.Therapy)
                                          .FieldLabel("Therapy");

            Ext.Net.Cell cTherapy = new Cell();
            cTherapy.ColSpan = 3;
            cTherapy.Items.Add(txtTherapy);
            tblMedicalIncident.Cells.Add(cTherapy);

            Ext.Net.TextField txtKeywords = this.X().TextField()
                                            .ID("txtKeywords" + ARow.ID.ToString())
                                            .Width(500)
                                            .EmptyText("for statistics, separated by comma")
                                            .Value(ARow.Keywords)
                                            .FieldLabel("Keywords");

            Ext.Net.Cell cKeywords = new Cell();
            cKeywords.ColSpan = 3;
            cKeywords.Items.Add(txtKeywords);
            tblMedicalIncident.Cells.Add(cKeywords);

            Ext.Net.Button btnDeleteIncident = this.X().Button()
                                               .ID("btnDeleteIncident" + ARow.ID.ToString())
                                               .Text("Delete Incident")
                                               .OnClientClick("DeleteMedicalIncident(" + ARow.ID.ToString() + ")");

            Ext.Net.Cell cDelete = new Cell();
            cDelete.ColSpan = 3;
            cDelete.Items.Add(btnDeleteIncident);
            tblMedicalIncident.Cells.Add(cDelete);

            panel.ContentControls.Add(tblMedicalIncident);
            panel.Render("MedicalPanel", RenderMode.AddTo);
            X.Js.Call("SetActiveMedicalIncident", ARow.ID - 1);
        }

        /// <summary>
        /// values to JSON string
        /// </summary>
        /// <param name="AValues"></param>
        /// <returns></returns>
        private string GetMedicalLogsFromScreen(Dictionary <string, string>AValues)
        {
            string Result = "[";

            Int32 NewIncidentID = (Int32)Session["NewMedicalId"];

            for (Int32 Counter = 1; Counter < NewIncidentID; Counter++)
            {
                if (AValues.ContainsKey("dtpDate" + Counter.ToString()))
                {
                    if (Result.EndsWith("}"))
                    {
                        Result += ",";
                    }

                    Result += "{\"ID\":\"" + Counter.ToString() + "\"";

                    string[] fields = new string[] {
                        "dtpDate", "txtExaminer", "txtPulse", "txtBloodPressure", "txtTemperature", "txtDiagnosis", "txtTherapy", "txtKeywords"
                    };

                    foreach (string name in fields)
                    {
                        string value = AValues[name + Counter.ToString()];

                        if (name == "dtpDate")
                        {
                            // TODO: assume European Date format on the client
                            value = value.Substring(6, 4) + "-" +
                                    value.Substring(3, 2) + "-" +
                                    value.Substring(0, 2);
                        }

                        if (value == "for statistics, separated by comma")
                        {
                            value = string.Empty;
                        }

                        Result += ",\"" + name + "\":\"" + value + "\"";
                    }

                    Result += "}";
                }
            }

            Result += "]";
            Result = Result.Replace(Environment.NewLine, "<br/");

            return Result;
        }

        private void LoadMedicalLogs(string AData)
        {
            Int32 NewMedicalId = 1;

            MedicalPanel.RemoveAll();

            if (AData.Length > 0)
            {
                Jayrock.Json.JsonArray list = (Jayrock.Json.JsonArray)Jayrock.Json.Conversion.JsonConvert.Import(AData);

                foreach (Jayrock.Json.JsonObject element in list)
                {
                    if (Convert.ToInt32(element["ID"]) >= NewMedicalId)
                    {
                        NewMedicalId = Convert.ToInt32(element["ID"]) + 1;
                    }

                    DateTime date = DateTime.Today;
                    try
                    {
                        string dateText = element["dtpDate"].ToString();

                        // TODO: assume European Date format on the client
                        date = new DateTime(
                            Convert.ToInt32(dateText.Substring(0, 4)),
                            Convert.ToInt32(dateText.Substring(5, 2)),
                            Convert.ToInt32(dateText.Substring(8, 2)));
                    }
                    catch (Exception)
                    {
                    }

                    AddMedicalIncidentData(new TMedicalIncident(Convert.ToInt32(element["ID"]),
                            date,
                            element["txtExaminer"].ToString(),
                            element["txtPulse"].ToString(),
                            element["txtBloodPressure"].ToString(),
                            element["txtTemperature"].ToString(),
                            element["txtDiagnosis"].ToString(),
                            element["txtTherapy"].ToString(),
                            element["txtKeywords"].ToString()));
                }
            }

            Session["NewMedicalId"] = NewMedicalId;
        }

        private void LoadDataForMedicalTeam(ConferenceApplicationTDSApplicationGridRow ARow, Jayrock.Json.JsonObject ARawDataObject)
        {
            // fill MedicalLog TextArea with Info about the participant
            string MedicalInfo = "<table>";

            MedicalInfo += "<tr><th>Name</th><td>" + ARow.FamilyName + ", " + ARow.FirstName + "</td></tr>";

            if (ARow.DateOfBirth.HasValue)
            {
                MedicalInfo += "<tr><th>Date of Birth</th><td>" + ARow.DateOfBirth.Value.ToString("dd-MMM-yyyy") + "</td></tr>";
            }

            MedicalInfo += "<tr><th>Country</th><td>" + ARawDataObject["Country"].ToString() + "</td></tr>";

            PPartnerTable offices = TApplicationManagement.GetRegistrationOffices();
            offices.DefaultView.Sort = PPartnerTable.GetPartnerKeyDBName();
            PPartnerRow office =
                (PPartnerRow)offices.DefaultView[offices.DefaultView.Find(Convert.ToInt64(ARawDataObject["RegistrationOffice"]))].Row;
            MedicalInfo += "<tr><th>Registration Office</th><td>" + office.PartnerShortName + "</td></tr>";

            MedicalInfo += "<tr><th>Fellowship Group</th><td>" + ARow.StFgCode + "</td></tr>";

            if (ARawDataObject.Contains("Phone"))
            {
                MedicalInfo += "<tr><th>Phone</th><td>" + ARawDataObject["Phone"].ToString() + "</td></tr>";
            }

            if (ARawDataObject.Contains("Mobile"))
            {
                MedicalInfo += "<tr><th>Mobile</th><td>" + ARawDataObject["Mobile"].ToString() + "</td></tr>";
            }

            foreach (string key in ARawDataObject.Names)
            {
                if (key.ToLower().Contains("health")
                    || key.ToLower().Contains("emergency")
                    || key.ToLower().Contains("medical")
                    || key.ToLower().Contains("vegetarian"))
                {
                    MedicalInfo += "<tr><th>" + key + "</th><td>" + ARawDataObject[key].ToString() + "</td></tr>";
                }
            }

#if disabled
            foreach (string key in ARawDataObject.Names)
            {
                // show the answers to these personal questions, since the applicants might write about past health issues
                if (key.ToLower().Contains("why")
                    || key.ToLower().Contains("daily")
                    || key.ToLower().Contains("what"))
                {
                    MedicalInfo += "<tr><th>" + key + "</th><td>" + ARawDataObject[key].ToString() + "</td></tr>";
                }
            }
#endif

            MedicalInfo += "</table>";
            TabMedicalInfo.Html = MedicalInfo.Replace("&quot;", "\\\"");

            LoadMedicalLogs(ARow.MedicalNotes);
        }

        protected void CalculateFellowshipGroups(Object sender, DirectEventArgs e)
        {
            try
            {
                Int32 MaxGroupMembersInt = Convert.ToInt32(MaxGroupMembers);

                TFellowshipGroups.CalculateFellowshipGroups(
                    EventPartnerKey,
                    EventCode,
                    GetSelectedRegistrationOffice(),
                    GetSelectedRole(),
                    MaxGroupMembersInt);
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
            }
        }

        protected void RefreshGroupMembers(Object sender, DirectEventArgs e)
        {
            GroupMembers.Text = TFellowshipGroups.GetGroupMembers(EventCode, StFgCode.Text);
        }
    }
}