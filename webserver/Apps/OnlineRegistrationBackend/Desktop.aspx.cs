//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Ext.Net;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using PetraWebService;
using Ict.Petra.Server.MConference.Applications;
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
        protected Ext.Net.Image Image1;
        protected Ext.Net.FileUploadField FileUploadField1;
        protected Ext.Net.FileUploadField FileUploadField2;
        protected Ext.Net.FileUploadField FileUploadField3;
        protected Ext.Net.ComboBox FileUploadCodePage3;
        protected Ext.Net.TextField OldPassword;
        protected Ext.Net.TextField NewPassword1;
        protected Ext.Net.TextField NewPassword2;
        protected Ext.Net.DesktopWindow winChangePassword;
        protected Ext.Net.TextField txtSearchApplicant;
        protected Ext.Net.GridPanel GridPanel1;
        protected Ext.Net.GridFilters GridFilters1;
        protected Ext.Net.Panel TabRawApplicationData;
        protected Ext.Net.TabPanel TabPanelApplication;

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

            EventCode = TAppSettingsManager.GetValue("ConferenceTool.EventCode");
            EventPartnerKey = TAppSettingsManager.GetInt64("ConferenceTool.EventPartnerKey");

            if (!X.IsAjaxRequest)
            {
                Session["CURRENTROW"] = null;
                RoleData_Refresh(null, null);
                ApplicationStatus_Refresh(null, null);
                RegistrationOffice_Refresh(null, null);

                if (ConferenceOrganisingOffice)
                {
                    // for the organising office, only show the accepted applicants by default.
                    FilterStatus.SelectedItem.Value = "accepted";
                }

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

            foreach (DataRow row in ATable.Rows)
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
                CurrentApplicants = TApplicationManagement.GetApplications(EventCode,
                    this.FilterStatus.SelectedItem.Value,
                    GetSelectedRegistrationOffice(),
                    GetSelectedRole());
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
            NumericFilter nf = (NumericFilter)GridFilters1.Filters[0];
            StringFilter sf = (StringFilter)GridFilters1.Filters[1];

            if (txtSearchApplicant.Text.Length == 0)
            {
                nf.SetActive(false);
                sf.SetActive(false);
                return;
            }

            try
            {
                Int64 Partnerkey = Convert.ToInt64(txtSearchApplicant.Text);

                if (Partnerkey < 1000000)
                {
                    // TODO: use sitekey?
                    Partnerkey += 4000000;
                }

                nf.SetValue(Partnerkey);
                sf.SetActive(false);
                nf.SetActive(true);
            }
            catch
            {
                // search for a name starting with this text
                sf.SetValue(txtSearchApplicant.Text);
                sf.SetActive(true);
                nf.SetActive(false);
            }
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

            if (offices.Count > 3)
            {
                ConferenceOrganisingOffice = true;
            }

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

        protected void RowSelect(object sender, DirectEventArgs e)
        {
            Int64 PartnerKey = Convert.ToInt64(e.ExtraParams["PartnerKey"]);

            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            CurrentApplicants.ApplicationGrid.DefaultView.RowFilter = "p_partner_key_n = " + PartnerKey.ToString();

            ConferenceApplicationTDSApplicationGridRow row =
                (ConferenceApplicationTDSApplicationGridRow)CurrentApplicants.ApplicationGrid.DefaultView[0].Row;
            Session["CURRENTROW"] = row;

            this.FormPanel1.Disabled = false;

            string RawData = TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice);
            TabRawApplicationData.Html = TJsonTools.DataToHTMLTable(RawData);

            Ext.Net.Panel panel = this.X().Panel()
                                  .ID("TabMoreDetails")
                                  .Title("Edit more details")
                                  .Padding(5)
                                  .AutoScroll(true);
            panel.Render("TabPanelApplication", 1, RenderMode.InsertTo);

            Ext.Net.Label label = this.X().Label()
                                  .ID("lblWarningEdit")
                                  .Html(
                "<b>Please be very careful</b>, only edit data if you are sure. Drop Down boxes or Dates might not work anymore.<br/><br/>");

            label.Render("TabMoreDetails", RenderMode.AddTo);

            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(RawData);

            var dictionary = new Dictionary <string, object>();
            dictionary.Add("PartnerKey", row.PartnerKey);
            dictionary.Add("PersonKey", row.IsPersonKeyNull() ? "" : row.PersonKey.ToString());
            dictionary.Add("FirstName", row.FirstName);
            dictionary.Add("FamilyName", row.FamilyName);
            dictionary.Add("Gender", row.Gender);
            dictionary.Add("DateOfBirth", row.DateOfBirth);
            dictionary.Add("GenAppDate", row.GenAppDate);
            dictionary.Add("GenApplicationStatus", row.GenApplicationStatus);
            dictionary.Add("StCongressCode", row.StCongressCode);
            dictionary.Add("Comment", row.Comment);
            dictionary.Add("StFgLeader", row.StFgLeader);
            dictionary.Add("StFgCode", row.StFgCode);

            List <string>FieldsOnFirstTab = new List <string>(new string[] {
                                                                  "TShirtStyle", "TShirtSize"
                                                              });

            foreach (string key in rawDataObject.Names)
            {
                if (!dictionary.ContainsKey(key)
                    && FieldsOnFirstTab.Contains(key))
                {
                    dictionary.Add(key, rawDataObject[key]);
                }
            }

            List <string>FieldsNotToBeEdited = new List <string>(new string[] {
                                                                     "Role", "FormsId", "EventIdentifier", "RegistrationOffice", "LastName",
                                                                     "RegistrationCountryCode", "ImageID",
                                                                     "CLS", "Dresscode", "LegalImprint"
                                                                 });

            foreach (string key in rawDataObject.Names)
            {
                if (!dictionary.ContainsKey(key)
                    && !FieldsNotToBeEdited.Contains(key)
                    && !FieldsOnFirstTab.Contains(key)
                    && !key.EndsWith("_SelIndex")
                    && !key.EndsWith("_Value")
                    && !key.EndsWith("_ActiveTab"))
                {
                    dictionary.Add(key, rawDataObject[key]);

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

            // SetValues: new {}; anonymous type: http://msdn.microsoft.com/en-us/library/bb397696.aspx
            // instead a Dictionary can be used as well
            // See also StackOverflow ObjectExtensions::ToDictionary for converting an anonymous type to a dictionary
            this.FormPanel1.SetValues(dictionary);

            Random rand = new Random();
            Image1.ImageUrl = "photos.aspx?id=" + PartnerKey.ToString() + ".jpg&" + rand.Next(1, 10000).ToString();
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

            string RawData = TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice);
            Jayrock.Json.JsonObject rawDataObject = TJsonTools.ParseValues(RawData);

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
            }

            row.JSONData = TJsonTools.ToJsonString(rawDataObject);

            row.FamilyName = values["FamilyName"];
            row.FirstName = values["FirstName"];
            row.Gender = values["Gender"];

            if (values["DateOfBirth"].Length == 0)
            {
                row.SetDateOfBirthNull();
            }
            else
            {
                row.DateOfBirth = Convert.ToDateTime(values["DateOfBirth"]);
            }

            row.GenAppDate = Convert.ToDateTime(values["GenAppDate"]);
            row.StCongressCode = values["StCongressCode_Value"];
            row.GenApplicationStatus = values["GenApplicationStatus_Value"];
            row.StFgLeader = values.ContainsKey("StFgLeader");
            row.StFgCode = values["StFgCode"];
            row.Comment = values["Comment"];

            if (TApplicationManagement.SaveApplication(EventCode, row) != TSubmitChangesResult.scrOK)
            {
                X.Msg.Alert("Error", "Saving did not work").Show();
            }

            // save some time? user can click refresh himself. 
            // MyData_Refresh(null, null);
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

            ConferenceApplicationTDS CurrentApplicants = TApplicationManagement.GetApplications(EventCode, "all", -1, null);

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
            string csvLines = TApplicationManagement.DownloadApplications(EventPartnerKey, EventCode, GetSelectedRegistrationOffice());

            this.Response.Clear();
            // TODO: this is a problem with old Petra 2.x, importing ANSI only
            this.Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");
            this.Response.ContentType = "application/csv";
            this.Response.AddHeader("Content-Disposition", "attachment; filename=petra-import.csv");
            this.Response.Write(csvLines);
            this.Response.End();
        }

        protected void DownloadExcel(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            this.Response.Clear();
            this.Response.ContentType = "application/xls";
            this.Response.AddHeader("Content-Type", "application/xls");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=TeenStreetApplicants.xls");
            MemoryStream m = new MemoryStream();
            TApplicationManagement.DownloadApplications(EventCode, ref CurrentApplicants, m);
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
    }
}