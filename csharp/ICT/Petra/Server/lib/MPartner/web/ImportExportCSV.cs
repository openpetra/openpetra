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
using System.Xml;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Server.MSysMan.Maintenance;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// This will create a new partner and new relationships, match addresses etc
    /// </summary>
    public class TPartnerImportCSV
    {
        private static Int32 FLocationKey = -1;

        /// <summary>
        /// Import data from a CSV file
        /// </summary>
        /// <param name="AChildNode"></param>
        /// <returns></returns>
        public static PartnerImportExportTDS ImportData(XmlNode AChildNode)
        {
            PartnerImportExportTDS ResultDS = new PartnerImportExportTDS();

            TPartnerImportCSV.FLocationKey = -1;

            while (AChildNode != null)
            {
                CreateNewFamily(AChildNode, ref ResultDS);

                AChildNode = AChildNode.NextSibling;
            }

            return ResultDS;
        }

        /// <summary>
        /// create a new family record with the address and return the data
        /// </summary>
        private static void CreateNewFamily(XmlNode ACurrentPartnerNode, ref PartnerImportExportTDS AMainDS)
        {
            PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

            AMainDS.PPartner.Rows.Add(newPartner);

            newPartner.PartnerKey = (AMainDS.PPartner.Rows.Count + 1) * -1;
            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            if (TXMLParser.HasAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_AQUISITION))
            {
                newPartner.AcquisitionCode = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_AQUISITION);
            }
            else
            {
                newPartner.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
            }

            newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_DEFAULT;

            if (TXMLParser.HasAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE))
            {
                newPartner.AddresseeTypeCode = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE);
            }
            else
            {
                string gender = GetGenderCode(ACurrentPartnerNode);

                if (gender == MPartnerConstants.GENDER_MALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_MALE;
                }
                else if (gender == MPartnerConstants.GENDER_FEMALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FEMALE;
                }
            }

            if (TXMLParser.HasAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                newPartner.LanguageCode = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            }
            else if (TUserDefaults.HasDefault(MSysManConstants.PARTNER_LANGUAGECODE))
            {
                newPartner.LanguageCode = TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGECODE);
            }

            PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
            AMainDS.PFamily.Rows.Add(newFamily);

            newFamily.PartnerKey = newPartner.PartnerKey;
            newFamily.FirstName = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FIRSTNAME);
            newFamily.FamilyName = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
            newFamily.Title = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE);
            newPartner.PartnerShortName = Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);

            string[] giftReceiptingDefaults = TSystemDefaults.GetSystemDefault("GiftReceiptingDefaults", ",no").Split(new char[] { ',' });
            newPartner.ReceiptLetterFrequency = giftReceiptingDefaults[0];
            newPartner.ReceiptEachGift = giftReceiptingDefaults[1] == "YES" || giftReceiptingDefaults[1] == "TRUE";

            newFamily.MaritalStatus = GetMaritalStatusCode(ACurrentPartnerNode);

            PLocationRow newLocation = AMainDS.PLocation.NewRowTyped(true);
            AMainDS.PLocation.Rows.Add(newLocation);
            newLocation.LocationKey = TPartnerImportCSV.FLocationKey;
            newLocation.Locality = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LOCALITY);
            newLocation.StreetName = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_STREETNAME);
            newLocation.Address3 = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESS);
            newLocation.PostalCode = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_POSTALCODE);
            newLocation.City = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_CITY);
            newLocation.County = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_COUNTY);
            newLocation.CountryCode = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);

            PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
            partnerlocation.LocationKey = TPartnerImportCSV.FLocationKey;
            partnerlocation.SiteKey = 0;
            partnerlocation.PartnerKey = newPartner.PartnerKey;
            partnerlocation.DateEffective = DateTime.Now;
            partnerlocation.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            partnerlocation.SendMail = true;
            partnerlocation.EmailAddress = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_EMAIL);
            partnerlocation.TelephoneNumber = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_PHONE);
            partnerlocation.MobileNumber = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);
            AMainDS.PPartnerLocation.Rows.Add(partnerlocation);

            TPartnerImportCSV.FLocationKey--;

            // import special types
            if (TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_SPECIALTYPES).Length != 0)
            {
                string specialTypes = TXMLParser.GetAttribute(ACurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_SPECIALTYPES);

                while (specialTypes.Length > 0)
                {
                    PPartnerTypeRow partnerType = AMainDS.PPartnerType.NewRowTyped(true);
                    partnerType.PartnerKey = newPartner.PartnerKey;
                    partnerType.TypeCode = StringHelper.GetNextCSV(ref specialTypes, ",").Trim().ToUpper();
                    AMainDS.PPartnerType.Rows.Add(partnerType);
                }
            }
        }

        /// <summary>
        /// returns the gender of the currently selected partner
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
                // or should this be Ms?
                return Catalog.GetString("Mrs");
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