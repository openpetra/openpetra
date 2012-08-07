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
using System;
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Ict.Testing.NUnitPetraServer;
using Ict.Testing.NUnitTools;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MConference.Applications;
using Ict.Common.Data;
using Ict.Petra.Server.MPartner.Import;

namespace Tests.MConference.OnlineRegistration
{
    /// This will test the business logic directly on the server
    [TestFixture]
    public class TConferenceOnlineRegistrationTest
    {
        static Int64 EventPartnerKey;
        static string EventCode;

        /// <summary>
        /// open database connection or prepare other things for this test
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");

            EventPartnerKey = TAppSettingsManager.GetInt64("ConferenceTool.EventPartnerKey", 1110198);
            EventCode = TAppSettingsManager.GetValue("ConferenceTool.EventCode", "SC001CNGRSS08");
        }

        /// <summary>
        /// cleaning up everything that was set up for this test
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraServerConnector.Disconnect();
        }

        /// <summary>
        /// process a new application
        /// </summary>
        [Test]
        public void TestProcessApplication()
        {
            string JSONFormData = "{'RegistrationOffice':'#REGISTRATIONOFFICE_VALUE'," +
                                  "'EventIdentifier':'#EVENTCODE','EventPartnerKey':'#EVENTPARTNERKEY'," +
                                  "'RegistrationCountryCode':'#CULTURECODE','EventPartnerKey':'#EVENTPARTNERKEY'," +
                                  "'Role':'#ROLE','FirstName':'#FIRSTNAME','LastName':'#LASTNAME'," +
                                  "'Street':'#STREET','Postcode':'#POSTCODE','City':'#CITY','Country':'#COUNTRY_VALUE'," +
                                  "'Phone':'#PHONE','Email':'#EMAIL','DateOfBirth':'#DATEOFBIRTH'," +
                                  "'Gender':'#GENDER','Vegetarian':'#VEGETARIAN','MedicalNeeds':'#MEDICALNEEDS','PaymentInfo':'#PAYMENTINFO'}";

            StringBuilder json = new StringBuilder(JSONFormData);

            Dictionary <string, string>values = new Dictionary <string, string>();

            values.Add("EventCode", EventCode);
            values.Add("EventPartnerKey", EventPartnerKey.ToString());

            string cultureCode = "de-DE";
            values.Add("RegistrationCountryCode", cultureCode);

            Int64 RegistrationOffice = 43000000;
            string CountryIsoCode = "DE";

            DateTime DateOfBirth = new DateTime(1990, 12, 20);
            string role = "TS-SERVE";

            values.Add("RegistrationOffice_Value", RegistrationOffice.ToString());
            values.Add("Role", role);
            values.Add("FormsId", "\"");
            values.Add("culturecode", cultureCode);

            values.Add("FirstName", "Hans");
            values.Add("LastName", "Bambel");
            values.Add("Gender", "Male");
            values.Add("Vegetarian", "No");

            string EmailAddress = "hans.bambel@sample.openpetra.org";

            values.Add("Email", EmailAddress);
            values.Add("Street", "Bahnhofstr. 1");
            values.Add("Postcode", "12345");
            values.Add("City", "Berlin");
            values.Add("Country_VALUE", CountryIsoCode);
            values.Add("MedicalNeeds", "test with \"quote\" in text");
            values.Add("PaymentInfo", "NONE");

            Catalog.Init("en-GB", cultureCode);
            values.Add("DateOfBirth", DateOfBirth.ToShortDateString()); // in the culture of the country code

            foreach (string key in values.Keys)
            {
                string value = values[key].ToString().Trim();

                json.Replace("#" + key.ToUpper(), value);
            }

            string result = TImportPartnerForm.DataImportFromForm("RegisterPerson", json.ToString(), false);

            Assert.AreEqual("{\"failure\":true, \"data\":{\"result\":\"We were not able to send the email to " + EmailAddress + "\"}}",
                result,
                "everything but sending the email should work");
        }
    }
}