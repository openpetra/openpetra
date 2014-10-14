//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System;
using System.Xml;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults.WebConnectors;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// This will create a new partner and new relationships, match addresses etc
    /// </summary>
    public class TPartnerImportCSV
    {
        private static Int32 FLocationKey = -1;
        private static TVerificationResultCollection ResultsCol;
        private static String ResultsContext;

        private static void AddVerificationResult(String AResultText, TResultSeverity ASeverity)
        {
            if (ASeverity != TResultSeverity.Resv_Status)
            {
                TLogging.Log(AResultText);
            }

            ResultsCol.Add(new TVerificationResult(ResultsContext, AResultText, ASeverity));
        }

        private static void AddVerificationResult(String AResultText)
        {
            AddVerificationResult(AResultText, TResultSeverity.Resv_Noncritical);
        }

        /// <summary>
        /// Import data from a CSV file
        /// </summary>
        /// <param name="ANode"></param>
        /// <param name="AReferenceResults"></param>
        /// <returns></returns>
        public static PartnerImportExportTDS ImportData(XmlNode ANode, ref TVerificationResultCollection AReferenceResults)
        {
            PartnerImportExportTDS ResultDS = new PartnerImportExportTDS();

            TPartnerImportCSV.FLocationKey = -1;
            ResultsCol = AReferenceResults;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            while (ANode != null)
            {
                ResultsContext = "CSV Import";
                String PartnerClass = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PARTNERCLASS).ToUpper();
                Int64 PartnerKey = 0;
                int LocationKey = 0;

                if (PartnerClass.Length == 0)
                {
                    PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                }

                if ((PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY) || (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON))
                {
                    ResultsContext = "CSV Import Family";
                    PartnerKey = CreateNewFamily(ANode, out LocationKey, ref ResultDS);
                    CreateSpecialTypes(ANode, PartnerKey, "SpecialTypeFamily_", ref ResultDS);
                }

                if (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    ResultsContext = "CSV Import person";
                    Int64 PersonKey = CreateNewPerson(PartnerKey, LocationKey, ANode, ref ResultDS);
                    CreateShortTermApplication(ANode, PersonKey, ref ResultDS, Transaction);
                    CreateSpecialTypes(ANode, PersonKey, ref ResultDS);
                    CreateSubscriptions(ANode, PersonKey, ref ResultDS);
                    CreateContacts(ANode, PersonKey, ref ResultDS, "_1");
                    CreateContacts(ANode, PersonKey, ref ResultDS, "_2");
                    CreatePassport(ANode, PersonKey, ref ResultDS);
                }

                ANode = ANode.NextSibling;
            }

            DBAccess.GDBAccessObj.CommitTransaction();

            return ResultDS;
        }

        /// <summary>
        /// Create new partner, family, location and PartnerLocation records in MainDS
        /// </summary>
        private static Int64 CreateNewFamily(XmlNode ANode, out int ALocationKey, ref PartnerImportExportTDS AMainDS)
        {
            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

            AMainDS.PPartner.Rows.Add(newPartner);

            newPartner.PartnerKey = (AMainDS.PPartner.Rows.Count + 1) * -1;
            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
            newPartner.Comment = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_NOTESFAMILY);

            String AcquisitionCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_AQUISITION);
            newPartner.AcquisitionCode = (AcquisitionCode.Length > 0) ? AcquisitionCode : MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;

            newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_DEFAULT;

            if (TXMLParser.HasAttribute(ANode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE))
            {
                newPartner.AddresseeTypeCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE);
            }
            else
            {
                string gender = GetGenderCode(ANode);

                if (gender == MPartnerConstants.GENDER_MALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_MALE;
                }
                else if (gender == MPartnerConstants.GENDER_FEMALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FEMALE;
                }
            }

            if (TXMLParser.HasAttribute(ANode, MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                newPartner.LanguageCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            }
            else if (TUserDefaults.HasDefault(MSysManConstants.PARTNER_LANGUAGECODE))
            {
                newPartner.LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGECODE);
            }

            string[] giftReceiptingDefaults = TSystemDefaults.GetSystemDefault("GiftReceiptingDefaults", ",no").Split(new char[] { ',' });
            newPartner.ReceiptLetterFrequency = giftReceiptingDefaults[0];
            newPartner.ReceiptEachGift = giftReceiptingDefaults[1] == "YES" || giftReceiptingDefaults[1] == "TRUE";


            PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
            AMainDS.PFamily.Rows.Add(newFamily);

            newFamily.PartnerKey = newPartner.PartnerKey;
            newFamily.FirstName = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_FIRSTNAME);
            newFamily.FamilyName = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
            newFamily.MaritalStatus = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS);
            newFamily.Title = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_TITLE);
            newFamily.MaritalStatus = GetMaritalStatusCode(ANode);

            newPartner.PartnerShortName = Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);
            PLocationRow newLocation = AMainDS.PLocation.NewRowTyped(true);
            AMainDS.PLocation.Rows.Add(newLocation);
            newLocation.LocationKey = TPartnerImportCSV.FLocationKey;
            newLocation.Locality = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_LOCALITY);
            newLocation.StreetName = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_STREETNAME);
            newLocation.Address3 = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_ADDRESS);
            newLocation.PostalCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_POSTALCODE);
            newLocation.City = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CITY);
            newLocation.County = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_COUNTY);
            newLocation.CountryCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);

            PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
            partnerlocation.LocationKey = TPartnerImportCSV.FLocationKey;
            partnerlocation.SiteKey = 0;
            partnerlocation.PartnerKey = newPartner.PartnerKey;
            partnerlocation.DateEffective = DateTime.Now;
            partnerlocation.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            partnerlocation.SendMail = true;
            partnerlocation.EmailAddress = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_EMAIL);
            partnerlocation.TelephoneNumber = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PHONE);
            partnerlocation.MobileNumber = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);
            AMainDS.PPartnerLocation.Rows.Add(partnerlocation);

            ALocationKey = TPartnerImportCSV.FLocationKey;
            TPartnerImportCSV.FLocationKey--;
            return newPartner.PartnerKey;
        }

        /// <summary>
        /// Create a Partner and a Person having this FamilyKey, living at this address.
        /// </summary>
        /// <param name="AFamilyKey"></param>
        /// <param name="ALocationKey"></param>
        /// <param name="ANode"></param>
        /// <param name="AMainDS"></param>
        private static Int64 CreateNewPerson(Int64 AFamilyKey, int ALocationKey, XmlNode ANode, ref PartnerImportExportTDS AMainDS)
        {
            AMainDS.PFamily.DefaultView.RowFilter = String.Format("{0}={1}", PFamilyTable.GetPartnerKeyDBName(), AFamilyKey);
            PFamilyRow FamilyRow = (PFamilyRow)AMainDS.PFamily.DefaultView[0].Row;

            AMainDS.PPartner.DefaultView.RowFilter = String.Format("{0}={1}", PPartnerTable.GetPartnerKeyDBName(), AFamilyKey);
            PPartnerRow PartnerRow = (PPartnerRow)AMainDS.PPartner.DefaultView[0].Row;

            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();
            AMainDS.PPartner.Rows.Add(newPartner);

            newPartner.PartnerKey = (AMainDS.PPartner.Rows.Count + 1) * -1;
            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_PERSON;
            newPartner.AddresseeTypeCode = PartnerRow.AddresseeTypeCode;
            newPartner.PartnerShortName = PartnerRow.PartnerShortName;
            newPartner.LanguageCode = PartnerRow.LanguageCode;
            newPartner.Comment = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_NOTES);
            newPartner.AcquisitionCode = PartnerRow.AcquisitionCode;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            PPersonRow newPerson = AMainDS.PPerson.NewRowTyped();
            AMainDS.PPerson.Rows.Add(newPerson);

            newPerson.PartnerKey = newPartner.PartnerKey;
            newPerson.FamilyKey = AFamilyKey;
            // When this record is imported, newPerson.FamilyId must be unique for this family!
            newPerson.FirstName = FamilyRow.FirstName;
            newPerson.FamilyName = FamilyRow.FamilyName;
            newPerson.Title = FamilyRow.Title;
            newPerson.Gender = GetGenderCode(ANode);
            newPerson.MaritalStatus = FamilyRow.MaritalStatus;

            PPartnerLocationRow newPartnerLocation = AMainDS.PPartnerLocation.NewRowTyped();
            AMainDS.PPartnerLocation.Rows.Add(newPartnerLocation);

            newPartnerLocation.LocationKey = ALocationKey; // This person lives at the same address as the family.
            newPartnerLocation.SiteKey = 0;
            newPartnerLocation.PartnerKey = newPartner.PartnerKey;
            newPartnerLocation.DateEffective = DateTime.Now;
            newPartnerLocation.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            newPartnerLocation.SendMail = true;
            newPartnerLocation.EmailAddress = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_EMAIL);
            newPartnerLocation.TelephoneNumber = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PHONE);
            newPartnerLocation.MobileNumber = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);
            AddVerificationResult("Person Record Created.", TResultSeverity.Resv_Status);
            return newPerson.PartnerKey;
        }

        private static void CreateSpecialTypes(XmlNode ANode, Int64 APartnerKey, String ACSVKey, ref PartnerImportExportTDS AMainDS)
        {
            for (int Idx = 1; Idx < 6; Idx++)
            {
                String SpecialType = TXMLParser.GetAttribute(ANode, ACSVKey + Idx.ToString());

                if (SpecialType.Length > 0)
                {
                    PPartnerTypeRow partnerType = AMainDS.PPartnerType.NewRowTyped(true);
                    partnerType.PartnerKey = APartnerKey;
                    partnerType.TypeCode = SpecialType;
                    AMainDS.PPartnerType.Rows.Add(partnerType);
                }
            }
        }

        private static void CreateSpecialTypes(XmlNode ANode, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS)
        {
            // This previous code requires a format that doesn't conform to the documented standard:

/*
 *          if (TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_SPECIALTYPES).Length != 0)
 *          {
 *              string specialTypes = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_SPECIALTYPES);
 *
 *              while (specialTypes.Length > 0)
 *              {
 *                  PPartnerTypeRow partnerType = AMainDS.PPartnerType.NewRowTyped(true);
 *                  partnerType.PartnerKey = PartnerKey;
 *                  partnerType.TypeCode = StringHelper.GetNextCSV(ref specialTypes, ",").Trim().ToUpper();
 *                  AMainDS.PPartnerType.Rows.Add(partnerType);
 *              }
 *          }
 */
            CreateSpecialTypes(ANode, APartnerKey, "SpecialType_", ref AMainDS);
        }

        private static void CreateShortTermApplication(XmlNode ANode,
            Int64 APartnerKey,
            ref PartnerImportExportTDS AMainDS,
            TDBTransaction ATransaction)
        {
            String strEventKey = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_EVENTKEY);
            long EventKey = -1;

            if (strEventKey.Length > 0)
            {
                try
                {
                    EventKey = long.Parse(strEventKey);
                }
                catch (System.FormatException)
                {
                    AddVerificationResult("Bad number format in EventKey: " + strEventKey);
                }

                if (!PUnitAccess.Exists(EventKey, ATransaction))
                {
                    AddVerificationResult("EventKey not known - application cannot be imported: " + EventKey);
                    return;
                }

                PmGeneralApplicationRow GenAppRow = AMainDS.PmGeneralApplication.NewRowTyped();

                GenAppRow.PartnerKey = APartnerKey;
                GenAppRow.ApplicationKey = (int)DBAccess.GDBAccessObj.GetNextSequenceValue("seq_application", ATransaction);

                GenAppRow.OldLink =
                    TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SITEKEY, "") + ";" + GenAppRow.ApplicationKey.ToString();
                GenAppRow.RegistrationOffice = DomainManager.GSiteKey; // When this is imported, RegistrationOffice can't be null.

                GenAppRow.GenAppDate = DateTime.Now;
                GenAppRow.AppTypeName = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_APPTYPE);
                GenAppRow.GenApplicationStatus = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_APPSTATUS);
                GenAppRow.Comment = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_APPCOMMENTS);

                PmShortTermApplicationRow ShortTermRow = AMainDS.PmShortTermApplication.NewRowTyped();
                ShortTermRow.PartnerKey = APartnerKey;
                ShortTermRow.ApplicationKey = GenAppRow.ApplicationKey;
                ShortTermRow.RegistrationOffice = GenAppRow.RegistrationOffice; // When this is imported, RegistrationOffice can't be null.
                ShortTermRow.StBasicOutreachId = "Unused field"; // This field is scheduled for deletion, but NOT NULL now.
                ShortTermRow.StAppDate = DateTime.Now;
                ShortTermRow.StApplicationType = GenAppRow.AppTypeName;
                ShortTermRow.StConfirmedOption = EventKey;
                String TimeString = "";

                try
                {
                    TimeString = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_ARRIVALDATE);

                    if (TimeString.Length > 0)
                    {
                        ShortTermRow.Arrival = DateTime.Parse(TimeString);
                    }

                    TimeString = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_DEPARTUREDATE);

                    if (TimeString.Length > 0)
                    {
                        ShortTermRow.Departure = DateTime.Parse(TimeString);
                    }
                }
                catch (System.FormatException)
                {
                    AddVerificationResult("Bad date format in Application: " + TimeString);
                }

                DateTime TempTime;

                TimeString = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_ARRIVALTIME);

                if (TimeString.Length > 0)
                {
                    try
                    {
                        TempTime = DateTime.Parse(TimeString);
                        ShortTermRow.ArrivalHour = TempTime.Hour;
                        ShortTermRow.ArrivalMinute = TempTime.Minute;
                    }
                    catch (System.FormatException)
                    {
                        AddVerificationResult("Bad time format in Application: " + TimeString);
                    }
                }

                TimeString = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_DEPARTURETIME);

                if (TimeString.Length > 0)
                {
                    try
                    {
                        TempTime = DateTime.Parse(TimeString);
                        ShortTermRow.DepartureHour = TempTime.Hour;
                        ShortTermRow.DepartureMinute = TempTime.Minute;
                    }
                    catch (System.FormatException)
                    {
                        AddVerificationResult("Bad time format in Application: " + TimeString);
                    }
                }

                ShortTermRow.OutreachRole = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_EVENTROLE);
                String ChargedField = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CHARGEDFIELD);

                if (ChargedField.Length > 0)
                {
                    try
                    {
                        ShortTermRow.StFieldCharged = long.Parse(ChargedField);
                    }
                    catch
                    {
                        AddVerificationResult("Bad number format in ChargedField: " + ChargedField);
                    }
                }

                AMainDS.PmGeneralApplication.Rows.Add(GenAppRow);
                AMainDS.PmShortTermApplication.Rows.Add(ShortTermRow);
                AddVerificationResult("Application Record Created.", TResultSeverity.Resv_Status);
            }
        }

        private static void CreatePassport(XmlNode ANode, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS)
        {
            string PassportNum = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTNUMBER);

            if (PassportNum.Length > 0)
            {
                PmPassportDetailsRow NewRow = AMainDS.PmPassportDetails.NewRowTyped();
                NewRow.PassportNumber = PassportNum;
                NewRow.PassportDetailsType = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTTYPE);
                NewRow.PlaceOfBirth = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFBIRTH);
                NewRow.PassportNationalityCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTNATIONALITY);
                NewRow.PlaceOfIssue = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFISSUE);
                NewRow.CountryOfIssue = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE);
                NewRow.DateOfIssue = DateTime.Parse(TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFISSUE));
                NewRow.DateOfExpiration = DateTime.Parse(TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFEXPIRATION));
                AMainDS.PmPassportDetails.Rows.Add(NewRow);
                AddVerificationResult("Passport Record Created.", TResultSeverity.Resv_Status);
            }
        }

        private static void CreateSubscriptions(XmlNode ANode, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS)
        {
            int SubsCount = 0;

            foreach (XmlAttribute Attr in ANode.Attributes)
            {
                if ((Attr.Name.ToLower().IndexOf("subscribe_") == 0) && (Attr.Value.ToLower() == "yes"))
                {
                    PSubscriptionRow NewRow = AMainDS.PSubscription.NewRowTyped();
                    NewRow.PartnerKey = APartnerKey;
                    NewRow.PublicationCode = Attr.Name.Substring(10).ToUpper();
                    AMainDS.PSubscription.Rows.Add(NewRow);
                    SubsCount++;
                }
            }

            if (SubsCount > 0)
            {
                AddVerificationResult("Subscriptions Created.", TResultSeverity.Resv_Status);
            }
        }

        private static void CreateContacts(XmlNode ANode, Int64 APartnerKey, ref PartnerImportExportTDS AMainDS, string Suffix)
        {
            string ContactCode = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTCODE + Suffix);

            if (ContactCode.Length > 0)
            {
                PartnerImportExportTDSPContactLogRow ContactLogRow = AMainDS.PContactLog.NewRowTyped();
                ContactLogRow.ContactCode = ContactCode;
                ContactLogRow.ContactDate = DateTime.Parse(TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTDATE + Suffix));
                DateTime ContactTime = DateTime.Parse(TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTTIME + Suffix));
                //ContactLogRow.ContactTime = ((ContactTime.Hour * 60) + ContactTime.Minute * 60) + ContactTime.Second;
                ContactLogRow.Contactor = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTOR + Suffix);
                ContactLogRow.ContactComment = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTNOTES + Suffix);
                ContactLogRow.ContactAttr = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTATTR + Suffix);
                ContactLogRow.ContactDetail = TXMLParser.GetAttribute(ANode, MPartnerConstants.PARTNERIMPORT_CONTACTDETAIL + Suffix);
                AMainDS.PContactLog.Rows.Add(ContactLogRow);
                AddVerificationResult("Contact Record Created.", TResultSeverity.Resv_Status);
                
                var PartnerContactRow = AMainDS.PPartnerContact.NewRowTyped();
                PartnerContactRow.PartnerKey = APartnerKey;
                PartnerContactRow.ContactLogId = ContactLogRow.ContactLogId;
                AMainDS.PPartnerContact.Rows.Add(PartnerContactRow);          
            }
        }

        /// <summary>
        /// Returns the gender of the currently selected partner
        /// </summary>
        /// <returns></returns>
        private static string GetGenderCode(XmlNode ACurrentPartnerNode)
        {
            string gender = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_GENDER);

            if ((gender.ToLower() == Catalog.GetString("Female").ToLower()) || (gender.ToLower() == "female"))
            {
                return MPartnerConstants.GENDER_FEMALE;
            }
            else if ((gender.ToLower() == Catalog.GetString("Male").ToLower()) || (gender.ToLower() == "male"))
            {
                return MPartnerConstants.GENDER_MALE;
            }

            return MPartnerConstants.GENDER_UNKNOWN;
        }

        private static string GetTitle(XmlNode ACurrentPartnerNode)
        {
            if (TXMLParser.HasAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE))
            {
                return TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE);
            }

            string genderCode = GetGenderCode(ACurrentPartnerNode);

            if (genderCode == MPartnerConstants.GENDER_MALE)
            {
                return Catalog.GetString("Mr");
            }
            else if (genderCode == MPartnerConstants.GENDER_FEMALE)
            {
                return Catalog.GetString("Ms");
            }

            return "";
        }

        private static string GetMaritalStatusCode(XmlNode ACurrentPartnerNode)
        {
            if (TXMLParser.HasAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
            {
                string maritalStatus = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS);

                if (maritalStatus.ToLower() == Catalog.GetString("married").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_MARRIED;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("engaged").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_ENGAGED;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("single").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_SINGLE;
                }
                else if (maritalStatus.ToLower() == Catalog.GetString("divorced").ToLower())
                {
                    return MPartnerConstants.MARITALSTATUS_DIVORCED;
                }
            }

            return MPartnerConstants.MARITALSTATUS_UNDEFINED;
        }
    }
}