//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Jayrock.Json;

namespace Ict.Petra.Server.MPartner.Import
{
    /// <summary>
    /// collection of data that is entered on the web form
    /// </summary>
    public class TApplicationFormData
    {
        /// <summary>
        /// title for the partner
        /// </summary>
        public string title;
        /// <summary>
        /// first name of the partner
        /// </summary>
        public string firstname;
        /// <summary>
        /// last name of the partner
        /// </summary>
        public string lastname;
        /// <summary>
        /// street name and house number
        /// </summary>
        public string street;
        /// <summary>
        /// post code of the city
        /// </summary>
        public string postcode;
        /// <summary>
        /// name of the city
        /// </summary>
        public string city;
        /// <summary>
        /// county/state
        /// </summary>
        public string county;
        /// <summary>
        /// country
        /// </summary>
        public string country;
        /// <summary>
        /// land line phone number
        /// </summary>
        public string phone;
        /// <summary>
        /// mobile phone number
        /// </summary>
        public string mobilephone;
        /// <summary>
        /// email address
        /// </summary>
        public string email;
    }

    /// <summary>
    /// this class can be used for partners that want to insert or update their own data.
    /// this is time effective and helps the staff in the office.
    /// </summary>
    public class TImportPartnerForm
    {
        private static Int64 CreateFamily(ref PartnerEditTDS AMainDS, TApplicationFormData APartnerData)
        {
            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();
            Int64 SiteKey = DomainManager.GSiteKey;

            // get a new partner key
            Int64 newPartnerKey = -1;

            do
            {
                newPartnerKey = TNewPartnerKey.GetNewPartnerKey(SiteKey);
                TNewPartnerKey.SubmitNewPartnerKey(SiteKey, newPartnerKey, ref newPartnerKey);
                newPartner.PartnerKey = newPartnerKey;
            } while (newPartnerKey == -1);

            // TODO: new status UNAPPROVED?
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
            AMainDS.PPartner.Rows.Add(newPartner);

            PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
            newFamily.PartnerKey = newPartner.PartnerKey;
            newFamily.FamilyName = APartnerData.lastname;
            newFamily.FirstName = APartnerData.firstname;
            newFamily.Title = APartnerData.title;
            AMainDS.PFamily.Rows.Add(newFamily);

            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            newPartner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

            newPartner.PartnerShortName =
                Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);
            return newPartnerKey;
        }

        private static void CreateAddress(ref PartnerEditTDS AMainDS, TApplicationFormData APartnerData, Int64 ANewPartnerKey)
        {
            // the webform prevents adding empty addresses

            // TODO: avoid duplicate addresses, reuse existing locations
            PLocationRow location = AMainDS.PLocation.NewRowTyped(true);

            location.LocationKey = (AMainDS.PLocation.Rows.Count + 1) * -1;
            location.SiteKey = 0;

            location.CountryCode = APartnerData.country;
            location.County = APartnerData.county;
            location.StreetName = APartnerData.street;
            location.City = APartnerData.city;
            location.PostalCode = APartnerData.postcode;
            AMainDS.PLocation.Rows.Add(location);

            PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
            partnerlocation.SiteKey = 0;
            partnerlocation.LocationKey = location.LocationKey;
            partnerlocation.PartnerKey = ANewPartnerKey;
            partnerlocation.SendMail = true;
            partnerlocation.DateEffective = DateTime.Now;
            partnerlocation.LocationType = "HOME";
            partnerlocation.EmailAddress = APartnerData.email;
            partnerlocation.TelephoneNumber = APartnerData.phone;
            partnerlocation.MobileNumber = APartnerData.mobilephone;
            AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
        }

        /// <summary>
        /// method for importing data entered on the web form
        /// </summary>
        /// <param name="AFormID"></param>
        /// <param name="AJSONFormData"></param>
        /// <returns></returns>
        public static string DataImportFromForm(string AFormID, string AJSONFormData)
        {
            if (AFormID == "RegisterPerson")
            {
                try
                {
                    TApplicationFormData data = (TApplicationFormData)Jayrock.Json.Conversion.JsonConvert.Import(typeof(TApplicationFormData),
                        AJSONFormData);

                    PartnerEditTDS MainDS = new PartnerEditTDS();

                    // TODO: check that email is unique. do not allow email to be associated with 2 records. this would cause trouble with authentication
                    // TODO: create a user for this partner

                    Int64 NewPartnerKey = CreateFamily(ref MainDS, data);
                    CreateAddress(ref MainDS, data, NewPartnerKey);

                    TVerificationResultCollection VerificationResult;
                    PartnerEditTDSAccess.SubmitChanges(MainDS, out VerificationResult);

                    if (VerificationResult.HasCriticalError())
                    {
                        TLogging.Log(VerificationResult.BuildVerificationResultString());
                        string message = "There is some critical error when saving to the database";
                        return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                    }

                    // TODO create PDF, send email
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                }

                return "{\"success\":true}";
            }
            else
            {
                string message = "The server does not know about a form called " + AFormID;
                TLogging.Log(message);
                return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
            }
        }
    }
}