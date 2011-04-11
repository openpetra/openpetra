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
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;

namespace Ict.Petra.WebServer.MConference
{
    public partial class TPageOnlineApplication : System.Web.UI.Page
    {
        protected string EventCode = "TODOEVENTCODE";
        protected Int64 EventPartnerKey = 08151234;
        protected Ext.Net.ComboBox FilterStatus;
        protected Ext.Net.FormPanel FormPanel1;
        protected Ext.Net.Store Store1;
        protected Ext.Net.Store StoreRole;
        protected Ext.Net.Store StoreApplicationStatus;
        protected Ext.Net.Image Image1;
        protected Ext.Net.FileUploadField FileUploadField1;
        protected Ext.Net.FileUploadField FileUploadField2;
        protected Ext.Net.TextField OldPassword;
        protected Ext.Net.TextField NewPassword1;
        protected Ext.Net.TextField NewPassword2;
        protected Ext.Net.DesktopWindow winChangePassword;
        protected Ext.Net.TextField txtSearchApplicant;
        protected Ext.Net.GridPanel GridPanel1;
        protected Ext.Net.GridFilters GridFilters1;
        protected Ext.Net.Panel TabRawApplicationData;
        protected Ext.Net.TabPanel TabPanelApplication;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check for valid user
            TOpenPetraOrg myServer = new TOpenPetraOrg();

            if (!myServer.IsUserLoggedIn())
            {
                this.Response.Redirect("Default.aspx");
                return;
            }

            if (!X.IsAjaxRequest)
            {
                Session["CURRENTROW"] = null;
                MyData_Refresh(null, null);
                RoleData_Refresh(null, null);
                ApplicationStatus_Refresh(null, null);
            }
        }

        private object[] DataTableToArray(DataTable ATable)
        {
            ArrayList Result = new ArrayList();

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

        /// load data from the database
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            // TODO get the current sitekey of the user
            // TODO offer all available conferences???
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            if ((CurrentApplicants == null) || (sender != null) || (Session["CURRENTROW"] == null))
            {
                CurrentApplicants = TApplicationManagement.GetApplications(EventCode, this.FilterStatus.SelectedItem.Value);
                Session["CURRENTAPPLICANTS"] = CurrentApplicants;
                this.FormPanel1.SetValues(new { });
                this.FormPanel1.Disabled = true;
            }

            this.Store1.DataSource = DataTableToArray(CurrentApplicants.ApplicationGrid);
            this.Store1.DataBind();
        }

        protected void RoleData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            TPersonnelCacheable cache = new TPersonnelCacheable();
            Type dummy;
            PtCongressCodeTable roleTable = (PtCongressCodeTable)cache.GetCacheableTable(TCacheablePersonTablesEnum.EventRoleList,
                String.Empty, true, out dummy);

            this.StoreRole.DataSource = DataTableToArray(roleTable);

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

        protected void RowSelect(object sender, DirectEventArgs e)
        {
            Int64 PartnerKey = Convert.ToInt64(e.ExtraParams["PartnerKey"]);

            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            CurrentApplicants.ApplicationGrid.DefaultView.RowFilter = "p_partner_key_n = " + PartnerKey.ToString();

            ConferenceApplicationTDSApplicationGridRow row =
                (ConferenceApplicationTDSApplicationGridRow)CurrentApplicants.ApplicationGrid.DefaultView[0].Row;
            Session["CURRENTROW"] = row;

            this.FormPanel1.Disabled = false;

            this.FormPanel1.SetValues(new {
                    row.PartnerKey,
                    row.FirstName,
                    row.FamilyName,
                    row.Gender,
                    row.DateOfBirth,
                    row.GenAppDate,
                    row.GenApplicationStatus,
                    row.StCongressCode,
                    row.Comment,
                    row.StFgLeader,
                    row.StFgCode
                });

            if (TabPanelApplication.ActiveTabIndex == 1)
            {
                // update the second tab, if it is active
                ShowRawApplicationData(null, null);
            }

            Random rand = new Random();
            Image1.ImageUrl = "photos.aspx?id=" + PartnerKey.ToString() + ".jpg&" + rand.Next(1, 10000).ToString();
        }

        protected void ShowRawApplicationData(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];

            TabRawApplicationData.Html =
                TJsonTools.DataToHTMLTable(TApplicationManagement.GetRawApplicationData(row.PartnerKey, row.ApplicationKey, row.RegistrationOffice));
        }

        protected void SaveApplication(object sender, DirectEventArgs e)
        {
            ConferenceApplicationTDSApplicationGridRow row = (ConferenceApplicationTDSApplicationGridRow)Session["CURRENTROW"];

            //Console.WriteLine(e.ExtraParams["Values"]);

            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);

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

            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];

            if (TApplicationManagement.SaveApplications(ref CurrentApplicants) != TSubmitChangesResult.scrOK)
            {
                X.Msg.Alert("Error", "Saving did not work").Show();
            }

            MyData_Refresh(null, null);
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

            ConferenceApplicationTDS CurrentApplicants = TApplicationManagement.GetApplications(EventCode, "all");

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
            ConferenceApplicationTDS CurrentApplicants = (ConferenceApplicationTDS)Session["CURRENTAPPLICANTS"];
            string csvLines = TApplicationManagement.DownloadApplications(EventPartnerKey, EventCode, ref CurrentApplicants);

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

        protected void ChangePassword(Object sender, DirectEventArgs e)
        {
            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);
            string oldPassword = values["OldPassword"].ToString().Trim();
            string newPassword = values["NewPassword1"].ToString().Trim();
            string newPassword2 = values["NewPassword2"].ToString().Trim();

            if (newPassword != newPassword2)
            {
                X.Msg.Alert("Error", "Your password has NOT been changed. <br/>You have entered two different new passwords.").Show();
                return;
            }

            TVerificationResultCollection verification;

            if (!TMaintenanceWebConnector.CheckPasswordQuality(newPassword, out verification))
            {
                X.Msg.Alert(
                    "Error",
                    "Your Password has NOT been changed. <br/>Your password is not strong enough. <br/><br/>" +
                    verification[0].ResultText)
                .Show();
                return;
            }

            if (TMaintenanceWebConnector.SetUserPassword(UserInfo.GUserInfo.UserID, newPassword, oldPassword) == true)
            {
                X.Msg.Alert("Success", "Your Password has been changed!").Show();
                OldPassword.Text = "";
                NewPassword1.Text = "";
                NewPassword2.Text = "";
                winChangePassword.Hide();
            }
            else
            {
                X.Msg.Alert("Error", "Your password has NOT been changed. <br/>You have probably entered the wrong old password").Show();
            }
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

                    this.FileUploadField1.PostedFile.SaveAs(TAppSettingsManager.GetValueStatic("Server.PathData") +
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
                        filename = TAppSettingsManager.GetValueStatic("Server.PathData") +
                                   Path.DirectorySeparatorChar + "petraimports" + Path.DirectorySeparatorChar +
                                   rand.Next(1, 1000000).ToString() + ".csv";
                    } while (File.Exists(filename));

                    this.FileUploadField2.PostedFile.SaveAs(filename);

                    TApplicationManagement.UploadPetraImportResult(filename);

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
                Console.WriteLine(ex.Message);
            }
        }
    }
}