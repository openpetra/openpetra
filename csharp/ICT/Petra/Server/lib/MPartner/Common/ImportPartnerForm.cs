//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Data;
using System.Drawing.Printing;
using System.Net.Mail;
using System.Collections;
using System.Text.RegularExpressions;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Ict.Common.Printing;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

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
        /// <summary>
        /// gender
        /// </summary>
        public string gender;
        /// <summary>
        /// Date of Birth of the person
        /// </summary>
        public DateTime dateofbirth;
        /// <summary>
        /// for the children
        /// </summary>
        public string parentname;
        /// <summary>
        /// partner key of registration office
        /// </summary>
        public Int64 registrationoffice;
        /// <summary>
        /// country code of registration office
        /// </summary>
        public string registrationcountrycode;
        /// <summary>
        /// identifies the event
        /// </summary>
        public string eventidentifier;
        /// <summary>
        /// identifies the event with the partner key
        /// </summary>
        public string eventpartnerkey;
        /// <summary>
        /// each applicant is given a role at the event (participant, volunteer, etc)
        /// </summary>
        public string role;
        /// <summary>
        /// relationship to the person which should be called if there are problems
        /// </summary>
        public string emergencyrelationship;
        /// <summary>
        /// used for de-CH, bus details
        /// </summary>
        public string busfrom;
        /// used for fr-CH, bus details
        public string travel;
        /// avoid default string when nothing gets entered
        public string groupwish;
        /// first job wish
        public string jobwish1;
        /// second job wish
        public string jobwish2;
        /// job wish for prayer or counselling team
        public string jobwishpray;
        /// parse date of arrival
        public DateTime? dateofarrival;
        /// parse date of departure
        public DateTime? dateofdeparture;
        /// has the applicant been here before?
        public string numberprevconfparticipant;
        /// has the applicant been here before?
        public string numberprevconfadult;
        /// has the applicant been here before?
        public string numberprevconfleader;
        /// has the applicant been here before?
        public string numberprevconfhelper;
        /// has the applicant been here before?
        public string numberprevconf;
        /// <summary>
        /// last year's partner key
        /// </summary>
        public string existingpartnerkey;
        /// <summary>
        /// the temp filename of the photo of the participant, which has been uploaded by upload.aspx
        /// </summary>
        public string imageid;
        /// <summary>
        /// the form that should be used, for example helper or teenager application form
        /// </summary>
        public string formsid;
        /// the raw data in json format
        public string RawData;
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

        private static Int64 CreatePerson(ref PartnerEditTDS AMainDS, Int64 AFamilyKey, TApplicationFormData APartnerData)
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

            PPersonRow newPerson = AMainDS.PPerson.NewRowTyped();
            newPerson.PartnerKey = newPartner.PartnerKey;
            newPerson.FamilyKey = AFamilyKey;
            newPerson.FirstName = APartnerData.firstname;
            newPerson.FamilyName = APartnerData.lastname;
            newPerson.Gender = APartnerData.gender;
            newPerson.DateOfBirth = APartnerData.dateofbirth;

            newPerson.Title = APartnerData.title;

            AMainDS.PPerson.Rows.Add(newPerson);

            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            newPartner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

            newPartner.PartnerShortName =
                Calculations.DeterminePartnerShortName(newPerson.FamilyName, newPerson.Title, newPerson.FirstName);
            return newPartnerKey;
        }

        private static void CreateAddress(ref PartnerEditTDS AMainDS, TApplicationFormData APartnerData, Int64 ANewPartnerKey)
        {
            // the webform prevents adding empty addresses

            // for children and staff, we do not require to enter an address
            if (APartnerData.street == null)
            {
                PPartnerLocationRow emptyPartnerLocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                emptyPartnerLocation.SiteKey = 0;
                emptyPartnerLocation.LocationKey = 0;
                emptyPartnerLocation.PartnerKey = ANewPartnerKey;
                emptyPartnerLocation.SendMail = false;
                emptyPartnerLocation.DateEffective = DateTime.Now;
                emptyPartnerLocation.LocationType = "HOME";
                emptyPartnerLocation.EmailAddress = APartnerData.email;
                AMainDS.PPartnerLocation.Rows.Add(emptyPartnerLocation);
                return;
            }

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

        /// create PDF
        public static string GeneratePDF(Int64 APartnerKey, string ACountryCode, TApplicationFormData AData, out string ADownloadIdentifier)
        {
            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                "ApplicationPDF",
                AData.registrationcountrycode,
                AData.formsid,
                "html");

            string HTMLText = string.Empty;

            if (!File.Exists(FileName))
            {
                HTMLText = "<html><body>" + String.Format("Cannot find file {0}", FileName) + "</body></html>";
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                HTMLText = r.ReadToEnd();
                r.Close();
            }

            if ((AData.existingpartnerkey != null) && AData.existingpartnerkey.StartsWith("If you cannot find it"))
            {
                AData.RawData = AData.RawData.Replace(AData.existingpartnerkey, "N/A");
                AData.existingpartnerkey = "";
            }

            if (AData.groupwish == null)
            {
                Regex regex = new Regex(@"^.*#GROUPWISH.*$", RegexOptions.Multiline);
                HTMLText = regex.Replace(HTMLText, "");
            }

            HTMLText = TJsonTools.ReplaceKeywordsWithData(AData.RawData, HTMLText);

            HTMLText = HTMLText.Replace("#DATE", StringHelper.DateToLocalizedString(DateTime.Today));
            HTMLText = HTMLText.Replace("#FORMLETTERPATH", TAppSettingsManager.GetValue("Formletters.Path"));
            HTMLText = HTMLText.Replace("#REGISTRATIONID", StringHelper.FormatStrToPartnerKeyString(APartnerKey.ToString()));
            HTMLText = HTMLText.Replace("#PHOTOPARTICIPANT", TAppSettingsManager.GetValue("Server.PathData") +
                Path.DirectorySeparatorChar + "photos" +
                Path.DirectorySeparatorChar + APartnerKey.ToString() + ".jpg");

            HTMLText = HTMLText.Replace("#HTMLRAWDATA", TJsonTools.DataToHTMLTable(AData.RawData));

            PrintDocument doc = new PrintDocument();

            TPdfPrinter pdfPrinter = new TPdfPrinter(doc, TGfxPrinter.ePrinterBehaviour.eFormLetter);
            TPrinterHtml htmlPrinter = new TPrinterHtml(HTMLText,
                String.Empty,
                pdfPrinter);

            pdfPrinter.Init(eOrientation.ePortrait, htmlPrinter, eMarginType.ePrintableArea);

            string pdfPath = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                             "pdfs";

            if (!Directory.Exists(pdfPath))
            {
                Directory.CreateDirectory(pdfPath);
            }

            string pdfFilename = pdfPath + Path.DirectorySeparatorChar + APartnerKey.ToString() + ".pdf";

            pdfPrinter.SavePDF(pdfFilename);

            string downloadPath = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                                  "downloads";

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            // Create a link file for this PDF
            ADownloadIdentifier = TPatchTools.GetMd5Sum(pdfFilename);
            StreamWriter sw = new StreamWriter(downloadPath + Path.DirectorySeparatorChar + ADownloadIdentifier + ".txt");
            sw.WriteLine("pdfs");
            sw.WriteLine(Path.GetFileName(pdfFilename));
            sw.Close();

            return pdfFilename;
        }

        /// send an email to the applicant and the registration office
        public static bool SendEmail(Int64 APartnerKey, string ACountryCode, TApplicationFormData AData, string APDFFilename)
        {
            string FileName = TFormLettersTools.GetRoleSpecificFile(TAppSettingsManager.GetValue("Formletters.Path"),
                "ApplicationReceivedEmail",
                AData.registrationcountrycode,
                AData.formsid,
                "html");

            string HTMLText = string.Empty;
            string SenderAddress = string.Empty;
            string BCCAddress = string.Empty;
            string EmailSubject = string.Empty;

            if (!File.Exists(FileName))
            {
                HTMLText = "<html><body>" + String.Format("Cannot find file {0}", FileName) + "</body></html>";
            }
            else
            {
                StreamReader r = new StreamReader(FileName);
                SenderAddress = r.ReadLine();
                BCCAddress = r.ReadLine();
                EmailSubject = r.ReadLine();
                HTMLText = r.ReadToEnd();
                r.Close();
            }

            if (!SenderAddress.StartsWith("From: "))
            {
                throw new Exception("missing From: line in the Email template " + FileName);
            }

            if (!BCCAddress.StartsWith("BCC: "))
            {
                throw new Exception("missing BCC: line in the Email template " + FileName);
            }

            if (!EmailSubject.StartsWith("Subject: "))
            {
                throw new Exception("missing Subject: line in the Email template " + FileName);
            }

            SenderAddress = SenderAddress.Substring("From: ".Length);
            BCCAddress = BCCAddress.Substring("BCC: ".Length);
            EmailSubject = EmailSubject.Substring("Subject: ".Length);

            HTMLText = TJsonTools.ReplaceKeywordsWithData(AData.RawData, HTMLText);
            HTMLText = HTMLText.Replace("#HTMLRAWDATA", TJsonTools.DataToHTMLTable(AData.RawData));

            // load the language file for the specific country
            Catalog.Init(ACountryCode, ACountryCode);

            // send email
            TSmtpSender emailSender = new TSmtpSender();

            MailMessage msg = new MailMessage(SenderAddress,
                AData.email,
                EmailSubject,
                HTMLText);

            msg.Attachments.Add(new Attachment(APDFFilename, System.Net.Mime.MediaTypeNames.Application.Octet));
            msg.Bcc.Add(BCCAddress);

            if (!emailSender.SendMessage(msg))
            {
                TLogging.Log("There has been a problem sending the email to " + AData.email);
                return false;
            }

            return true;
        }

        /// <summary>
        /// method for importing data entered on the web form
        /// </summary>
        /// <returns></returns>
        public static string DataImportFromForm(string AFormID, string AJSONFormData)
        {
            return DataImportFromForm(AFormID, AJSONFormData, true);
        }

        /// <summary>
        /// method for importing data entered on the web form
        /// </summary>
        /// <returns></returns>
        public static string DataImportFromForm(string AFormID, string AJSONFormData, bool ASendApplicationReceivedEmail)
        {
            if (AFormID == "TestPrintingEmail")
            {
                // This is a test for printing to PDF and sending an email, no partner is created in the database.
                // make sure you have a photo with name data\photos\815.jpg for the photo to appear in the pdf
                TApplicationFormData data = (TApplicationFormData)TJsonTools.ImportIntoTypedStructure(typeof(TApplicationFormData),
                    AJSONFormData);
                data.RawData = AJSONFormData;

                string pdfIdentifier;
                string pdfFilename = GeneratePDF(0815, data.registrationcountrycode, data, out pdfIdentifier);
                try
                {
                    if (SendEmail(0815, data.registrationcountrycode, data, pdfFilename))
                    {
                        // return id of the PDF pdfIdentifier
                        string result = "{\"success\":true,\"data\":{\"pdfPath\":\"downloadPDF.aspx?pdf-id=" + pdfIdentifier + "\"}}";
                        return result;
                    }
                    else
                    {
                        string message = String.Format(Catalog.GetString("We were not able to send the email to {0}"), data.email);
                        TLogging.Log("returning: " + "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}");
                        return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                }
            }

            if (AFormID == "RegisterPerson")
            {
                TApplicationFormData data = (TApplicationFormData)TJsonTools.ImportIntoTypedStructure(typeof(TApplicationFormData),
                    AJSONFormData);
                data.RawData = AJSONFormData;

                Int64 NewPersonPartnerKey = -1;
                string imageTmpPath = String.Empty;

                try
                {
                    PartnerEditTDS MainDS = new PartnerEditTDS();

                    // TODO: check that email is unique. do not allow email to be associated with 2 records. this would cause trouble with authentication
                    // TODO: create a user for this partner

                    Int64 NewFamilyPartnerKey = CreateFamily(ref MainDS, data);
                    NewPersonPartnerKey = CreatePerson(ref MainDS, NewFamilyPartnerKey, data);
                    CreateAddress(ref MainDS, data, NewFamilyPartnerKey);

                    TVerificationResultCollection VerificationResult;
                    PartnerEditTDSAccess.SubmitChanges(MainDS, out VerificationResult);

                    if (VerificationResult.HasCriticalErrors)
                    {
                        TLogging.Log(VerificationResult.BuildVerificationResultString());
                        string message = "There is some critical error when saving to the database";
                        return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                    }

                    // add a record for the application
                    ConferenceApplicationTDS ConfDS = new ConferenceApplicationTDS();
                    PmGeneralApplicationRow GeneralApplicationRow = ConfDS.PmGeneralApplication.NewRowTyped();
                    GeneralApplicationRow.RawApplicationData = AJSONFormData;
                    GeneralApplicationRow.PartnerKey = NewPersonPartnerKey;
                    GeneralApplicationRow.ApplicationKey = -1;
                    GeneralApplicationRow.RegistrationOffice = data.registrationoffice;
                    GeneralApplicationRow.GenAppDate = DateTime.Today;
                    GeneralApplicationRow.AppTypeName = MConferenceConstants.APPTYPE_CONFERENCE;

                    // TODO pm_st_basic_camp_identifier_c is quite strange. will there be an overflow soon?
                    // see ticket https://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=161
                    GeneralApplicationRow.OldLink = "";
                    GeneralApplicationRow.GenApplicantType = "";
                    GeneralApplicationRow.GenApplicationStatus = MConferenceConstants.APPSTATUS_ONHOLD;
                    ConfDS.PmGeneralApplication.Rows.Add(GeneralApplicationRow);

                    PmShortTermApplicationRow ShortTermApplicationRow = ConfDS.PmShortTermApplication.NewRowTyped();
                    ShortTermApplicationRow.PartnerKey = NewPersonPartnerKey;
                    ShortTermApplicationRow.ApplicationKey = -1;
                    ShortTermApplicationRow.RegistrationOffice = data.registrationoffice;
                    ShortTermApplicationRow.StAppDate = DateTime.Today;
                    ShortTermApplicationRow.StApplicationType = MConferenceConstants.APPTYPE_CONFERENCE;
                    ShortTermApplicationRow.StBasicOutreachId = GeneralApplicationRow.OldLink;
                    ShortTermApplicationRow.StCongressCode = data.role;
                    ShortTermApplicationRow.ConfirmedOptionCode = data.eventidentifier;
                    ShortTermApplicationRow.StConfirmedOption = Convert.ToInt64(data.eventpartnerkey);
                    ShortTermApplicationRow.StFieldCharged = data.registrationoffice;
                    ShortTermApplicationRow.Arrival = data.dateofarrival;
                    ShortTermApplicationRow.Departure = data.dateofdeparture;

                    ConfDS.PmShortTermApplication.Rows.Add(ShortTermApplicationRow);

                    // TODO ApplicationForms

                    ConferenceApplicationTDSAccess.SubmitChanges(ConfDS, out VerificationResult);

                    if (VerificationResult.HasCriticalErrors)
                    {
                        TLogging.Log(VerificationResult.BuildVerificationResultString());
                        string message = "There is some critical error when saving to the database";
                        return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                    }

                    // process Photo
                    imageTmpPath = TAppSettingsManager.GetValue("Server.PathTemp") +
                                   Path.DirectorySeparatorChar +
                                   Path.GetFileName(data.imageid);

                    if (File.Exists(imageTmpPath))
                    {
                        string photosPath = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar +
                                            "photos";

                        if (!Directory.Exists(photosPath))
                        {
                            Directory.CreateDirectory(photosPath);
                        }

                        File.Copy(imageTmpPath,
                            photosPath +
                            Path.DirectorySeparatorChar +
                            NewPersonPartnerKey +
                            Path.GetExtension(imageTmpPath).ToLower(), true);
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log(e.Message);
                    TLogging.Log(e.StackTrace);
                    string message = "There is some critical error when saving to the database";
                    return "{\"failure\":true, \"data\":{\"result\":\"" + message + "\"}}";
                }

                if (ASendApplicationReceivedEmail)
                {
                    string pdfIdentifier;
                    string pdfFilename = GeneratePDF(NewPersonPartnerKey, data.registrationcountrycode, data, out pdfIdentifier);
                    try
                    {
                        if (SendEmail(NewPersonPartnerKey, data.registrationcountrycode, data, pdfFilename))
                        {
                            if (File.Exists(imageTmpPath))
                            {
                                // only delete the temp image after successful application. otherwise we have a problem with resending the application, because the tmp image is gone
                                File.Delete(imageTmpPath);
                            }

                            // return id of the PDF pdfIdentifier
                            string result = "{\"success\":true,\"data\":{\"pdfPath\":\"downloadPDF.aspx?pdf-id=" + pdfIdentifier + "\"}}";
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log(e.Message);
                        TLogging.Log(e.StackTrace);
                    }
                }

                string message2 = String.Format(Catalog.GetString("We were not able to send the email to {0}"), data.email);
                string result2 = "{\"failure\":true, \"data\":{\"result\":\"" + message2 + "\"}}";

                if (ASendApplicationReceivedEmail)
                {
                    TLogging.Log(result2);
                }

                return result2;
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