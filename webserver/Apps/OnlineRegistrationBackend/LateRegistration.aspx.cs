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
using Ict.Petra.Shared;
using Ict.Petra.Server.MConference.Applications;
using Ict.Petra.Server.MPartner.Import;

namespace Ict.Petra.WebServer.MConference
{
    public partial class TLateRegistrationUI : System.Web.UI.Page
    {
        protected Ext.Net.TextArea RegistrationsKeys;
        protected Ext.Net.FormPanel FellowshipForm;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check for valid user
            TOpenPetraOrg myServer = new TOpenPetraOrg();

            if (!myServer.IsUserLoggedIn())
            {
                this.Response.Redirect("Default.aspx");
                return;
            }
        }

        protected void SubmitLateRegistration(Object sender, DirectEventArgs e)
        {
            string JSONFormData = "{'RegistrationOffice':'#REGISTRATIONOFFICE'," +
                                  "'EventIdentifier':'#EVENTCODE','EventPartnerKey':'#EVENTPARTNERKEY'," +
                                  "'Role':'#ROLE','FirstName':'#FIRSTNAME','LastName':'#LASTNAME'," +
                                  "'Street':'#STREET','Postcode':'#POSTCODE','City':'#CITY','Country':'#COUNTRY'," +
                                  "'Phone':'#PHONE','Email':'#EMAIL','DateOfBirth':'#DATEOFBIRTH','DateOfArrival':'#DATEOFARRIVAL'," +
                                  "'Gender':'#GENDER','Vegetarian':'#VEGETARIAN','MedicalNeeds':'#MEDICALNEEDS'}";

            JSONFormData = JSONFormData.Replace("'", "\"");

            Dictionary <string, string>values = JSON.Deserialize <Dictionary <string, string>>(e.ExtraParams["Values"]);

            JSONFormData = JSONFormData.Replace("#EVENTCODE", TAppSettingsManager.GetValue("ConferenceTool.EventCode"));
            JSONFormData = JSONFormData.Replace("#EVENTPARTNERKEY", TAppSettingsManager.GetValue("ConferenceTool.EventPartnerKey"));

            foreach (string key in values.Keys)
            {
                string value = values[key].ToString().Trim();

                JSONFormData = JSONFormData.Replace("#" + key.ToUpper(), value);
            }

            try
            {
                // should not be able to create a PDF since the picture is missing, and not send an email
                TImportPartnerForm.DataImportFromForm("RegisterPerson", JSONFormData);
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.Message);
                TLogging.Log(ex.StackTrace);
            }
        }
    }
}