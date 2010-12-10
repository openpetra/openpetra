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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// import and export partner data from yml files
    /// </summary>
    public class TImportExportYml
    {
        static Int64 NewPartnerKey = -1;

        private static void ParsePartners(ref PartnerImportExportTDS AMainDS, XmlNode ACurNode, ref TVerificationResultCollection AVerificationResult)
        {
            XmlNode LocalNode = ACurNode;

            while (LocalNode != null)
            {
                if (LocalNode.Name.StartsWith("PartnerGroup"))
                {
                    ParsePartners(ref AMainDS, LocalNode.FirstChild, ref AVerificationResult);
                }
                else if (LocalNode.Name.StartsWith("Partner"))
                {
                    PPartnerRow newPartner = AMainDS.PPartner.NewRowTyped();

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "SiteKey"))
                    {
                        throw new Exception(Catalog.GetString("Missing SiteKey Attribute"));
                    }

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "status"))
                    {
                        throw new Exception(Catalog.GetString("Missing status Attribute"));
                    }

                    // get a new partner key
                    Int64 SiteKey = Convert.ToInt64(TYml2Xml.GetAttributeRecursive(LocalNode, "SiteKey"));

                    if (TYml2Xml.HasAttribute(LocalNode, "PartnerKey"))
                    {
                        newPartner.PartnerKey = Convert.ToInt64(TYml2Xml.GetAttribute(LocalNode, "PartnerKey"));
                    }
                    else
                    {
                        newPartner.PartnerKey = TImportExportYml.NewPartnerKey;
                        TImportExportYml.NewPartnerKey--;
                    }

                    if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        PFamilyRow newFamily = AMainDS.PFamily.NewRowTyped();
                        newFamily.PartnerKey = newPartner.PartnerKey;
                        newFamily.FamilyName = TYml2Xml.GetAttributeRecursive(LocalNode, "LastName");
                        newFamily.FirstName = TYml2Xml.GetAttribute(LocalNode, "FirstName");
                        newFamily.Title = TYml2Xml.GetAttribute(LocalNode, "Title");

                        if (TYml2Xml.HasAttribute(LocalNode, "CreatedAt"))
                        {
                            newFamily.DateCreated = Convert.ToDateTime(TYml2Xml.GetAttribute(LocalNode, "CreatedAt"));
                        }

                        AMainDS.PFamily.Rows.Add(newFamily);

                        newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                        newPartner.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

                        newPartner.PartnerShortName =
                            Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);
                    }

                    if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        throw new Exception(
                            "We are currently not supporting import of PERSON records, until we have resolved the issues with household/family");
                        // TODO
                    }
                    else if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        POrganisationRow OrganisationRow = AMainDS.POrganisation.NewRowTyped();
                        OrganisationRow.PartnerKey = newPartner.PartnerKey;
                        OrganisationRow.OrganisationName = TYml2Xml.GetAttributeRecursive(LocalNode, "Name");
                        AMainDS.POrganisation.Rows.Add(OrganisationRow);

                        newPartner.PartnerShortName = OrganisationRow.OrganisationName;
                        newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_ORGANISATION;
                    }
                    else if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_UNIT)
                    {
                        PUnitRow UnitRow = AMainDS.PUnit.NewRowTyped();
                        UnitRow.PartnerKey = newPartner.PartnerKey;
                        UnitRow.UnitTypeCode = TYml2Xml.GetAttributeRecursive(LocalNode, "UnitTypeCode");
                        UnitRow.UnitName = TYml2Xml.GetAttributeRecursive(LocalNode, "Name");
                        AMainDS.PUnit.Rows.Add(UnitRow);

                        if (newPartner.PartnerKey < -1)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Importing Unit {0}"), UnitRow.UnitName),
                                    Catalog.GetString("You need to provide a partner key for the unit"),
                                    TResultSeverity.Resv_Critical));
                        }

                        UmUnitStructureRow UnitStructureRow = AMainDS.UmUnitStructure.NewRowTyped();

                        UnitStructureRow.ParentUnitKey = Convert.ToInt64(TYml2Xml.GetAttributeRecursive(LocalNode, "ParentUnitKey"));
                        UnitStructureRow.ChildUnitKey = newPartner.PartnerKey;

                        AMainDS.UmUnitStructure.Rows.Add(UnitStructureRow);

                        newPartner.PartnerShortName = UnitRow.UnitName;
                        newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
                    }
                    else
                    {
                        // TODO AVerificationResult add failing problem: unknown partner class
                    }

                    newPartner.StatusCode = TYml2Xml.GetAttributeRecursive(LocalNode, "status");
                    AMainDS.PPartner.Rows.Add(newPartner);

                    // import special types
                    StringCollection SpecialTypes = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "SpecialTypes"), ",");

                    foreach (string SpecialType in SpecialTypes)
                    {
                        PPartnerTypeRow partnertype = AMainDS.PPartnerType.NewRowTyped();
                        partnertype.PartnerKey = newPartner.PartnerKey;
                        partnertype.TypeCode = SpecialType.Trim();
                        AMainDS.PPartnerType.Rows.Add(partnertype);

                        // TODO: check if special type does not exist yet, and create it
                    }

                    // import subscriptions
                    StringCollection Subscriptions = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "Subscriptions"), ",");

                    foreach (string publicationCode in Subscriptions)
                    {
                        PSubscriptionRow subscription = AMainDS.PSubscription.NewRowTyped();
                        subscription.PartnerKey = newPartner.PartnerKey;
                        subscription.PublicationCode = publicationCode.Trim();
                        subscription.ReasonSubsGivenCode = "FREE";
                        AMainDS.PSubscription.Rows.Add(subscription);
                    }

                    // import address
                    XmlNode addressNode = TYml2Xml.GetChild(LocalNode, "Address");

                    if ((addressNode == null) || (TYml2Xml.GetAttributeRecursive(addressNode, "Street").Length == 0))
                    {
                        // add the empty location
                        PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                        partnerlocation.SiteKey = 0;
                        partnerlocation.PartnerKey = newPartner.PartnerKey;
                        partnerlocation.DateEffective = DateTime.Now;
                        partnerlocation.LocationType = "HOME";
                        partnerlocation.SendMail = false;
                        partnerlocation.EmailAddress = TYml2Xml.GetAttributeRecursive(addressNode, "Email");
                        partnerlocation.TelephoneNumber = TYml2Xml.GetAttributeRecursive(addressNode, "Phone");
                        partnerlocation.MobileNumber = TYml2Xml.GetAttributeRecursive(addressNode, "MobilePhone");
                        AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                    }
                    else
                    {
                        // TODO: avoid duplicate addresses, reuse existing locations
                        PLocationRow location = AMainDS.PLocation.NewRowTyped(true);
                        location.LocationKey = (AMainDS.PLocation.Rows.Count + 1) * -1;
                        location.SiteKey = 0;

                        if (!TYml2Xml.HasAttributeRecursive(LocalNode, "Country"))
                        {
                            throw new Exception(Catalog.GetString("Missing Country Attribute"));
                        }

                        location.CountryCode = TYml2Xml.GetAttributeRecursive(addressNode, "Country");
                        location.StreetName = TYml2Xml.GetAttributeRecursive(addressNode, "Street");
                        location.City = TYml2Xml.GetAttributeRecursive(addressNode, "City");
                        location.PostalCode = TYml2Xml.GetAttributeRecursive(addressNode, "PostCode");
                        AMainDS.PLocation.Rows.Add(location);

                        PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                        partnerlocation.SiteKey = 0;
                        partnerlocation.LocationKey = location.LocationKey;
                        partnerlocation.PartnerKey = newPartner.PartnerKey;
                        partnerlocation.SendMail = true;
                        partnerlocation.DateEffective = DateTime.Now;
                        partnerlocation.LocationType = "HOME";
                        partnerlocation.EmailAddress = TYml2Xml.GetAttributeRecursive(addressNode, "Email");
                        partnerlocation.TelephoneNumber = TYml2Xml.GetAttributeRecursive(addressNode, "Phone");
                        partnerlocation.MobileNumber = TYml2Xml.GetAttributeRecursive(addressNode, "MobilePhone");
                        AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                    }
                }

                LocalNode = LocalNode.NextSibling;
            }
        }

        /// <summary>
        /// imports partner data from file
        /// </summary>
        /// <param name="AXmlPartnerData"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static PartnerImportExportTDS ImportPartners(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            PartnerImportExportTDS MainDS = new PartnerImportExportTDS();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlPartnerData);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            // import partner groups
            // advantage: can inherit some common attributes, eg. partner class, etc

            TImportExportYml.NewPartnerKey = -1;
            ParsePartners(ref MainDS, root, ref AVerificationResult);

            return MainDS;
        }

        /// <summary>
        /// return an XmlDocument with all partner info;
        /// the partners are grouped by class, country, status, and sitekey
        /// </summary>
        /// <returns></returns>
        public static string ExportPartners()
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            // load data
            try
            {
                PPartnerAccess.LoadAll(MainDS, Transaction);

                foreach (PPartnerRow partnerRow in MainDS.PPartner.Rows)
                {
                    PLocationAccess.LoadViaPPartner(MainDS, partnerRow.PartnerKey, Transaction);
                    PPartnerLocationAccess.LoadViaPPartner(MainDS, partnerRow.PartnerKey, Transaction);
                    PPartnerTypeAccess.LoadViaPPartner(MainDS, partnerRow.PartnerKey, Transaction);
                    PPersonAccess.LoadViaPPartner(MainDS, partnerRow.PartnerKey, Transaction);
                    PFamilyAccess.LoadViaPPartner(MainDS, partnerRow.PartnerKey, Transaction);
                    POrganisationAccess.LoadViaPPartnerPartnerKey(MainDS, partnerRow.PartnerKey, Transaction);
                }
            }
            catch (Exception e)
            {
                TLogging.Log("ExportPartners: " + e.Message);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            // group partners by class, country, status, and sitekey
            SortedList <string, List <Int64>>PartnerCategories = new SortedList <string, List <long>>();

            foreach (PPartnerRow partnerRow in MainDS.PPartner.Rows)
            {
                string countryCode = "";
                Int64 siteKey = -1;

                // TODO: could determine the best address and use that
                DataView partnerLocationView = MainDS.PPartnerLocation.DefaultView;
                partnerLocationView.RowFilter = PPartnerLocationTable.GetPartnerKeyDBName() + " = " + partnerRow.PartnerKey.ToString();

                if (partnerLocationView.Count > 0)
                {
                    PPartnerLocationRow partnerLocationRow = (PPartnerLocationRow)partnerLocationView[0].Row;

                    DataView locationView = MainDS.PLocation.DefaultView;
                    locationView.RowFilter =
                        PLocationTable.GetSiteKeyDBName() + "=" + partnerLocationRow.SiteKey.ToString() + " AND " +
                        PLocationTable.GetLocationKeyDBName() + "=" + partnerLocationRow.LocationKey.ToString();

                    if (locationView.Count > 0)
                    {
                        PLocationRow locationRow = (PLocationRow)locationView[0].Row;
                        countryCode = locationRow.CountryCode;
                        siteKey = locationRow.SiteKey;
                    }
                }

                string category = partnerRow.PartnerClass + "," + countryCode + "," +
                                  partnerRow.StatusCode + "," + siteKey.ToString();

                if (!PartnerCategories.ContainsKey(category))
                {
                    PartnerCategories.Add(category, new List <long>());
                }

                PartnerCategories[category].Add(partnerRow.PartnerKey);
            }

            // create XML structure for each category
            XmlDocument PartnerData = TYml2Xml.CreateXmlDocument();

            XmlNode rootNode = PartnerData.FirstChild.NextSibling;

            Int32 groupCounter = 0;

            foreach (string category in PartnerCategories.Keys)
            {
                // get category data
                groupCounter++;
                XmlElement groupNode = PartnerData.CreateElement("PartnerGroup" + groupCounter.ToString());
                rootNode.AppendChild(groupNode);

                Int32 partnerCounter = 0;
                string[] categoryDetails = category.Split(new char[] { ',' });

                groupNode.SetAttribute("class", categoryDetails[0]);
                groupNode.SetAttribute("Country", categoryDetails[1]);
                groupNode.SetAttribute("status", categoryDetails[2]);
                groupNode.SetAttribute("SiteKey", categoryDetails[3]);

                List <long>partnerKeys = PartnerCategories[category];

                foreach (Int64 partnerKey in partnerKeys)
                {
                    MainDS.PPartner.DefaultView.RowFilter = PPartnerTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                    PPartnerRow partnerRow = (PPartnerRow)MainDS.PPartner.DefaultView[0].Row;

                    PFamilyRow familyRow = null;

                    if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        MainDS.PFamily.DefaultView.RowFilter = PFamilyTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                        familyRow = (PFamilyRow)MainDS.PFamily.DefaultView[0].Row;
                    }

                    PPersonRow personRow = null;

                    if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        MainDS.PPerson.DefaultView.RowFilter = PPersonTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                        personRow = (PPersonRow)MainDS.PPerson.DefaultView[0].Row;
                    }

                    POrganisationRow organisationRow = null;

                    if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        MainDS.POrganisation.DefaultView.RowFilter = POrganisationTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                        organisationRow = (POrganisationRow)MainDS.POrganisation.DefaultView[0].Row;
                    }

                    partnerCounter++;
                    XmlElement partnerNode = PartnerData.CreateElement("Partner" + partnerCounter.ToString());
                    groupNode.AppendChild(partnerNode);

                    partnerNode.SetAttribute("PartnerKey", partnerRow.PartnerKey.ToString());

                    //groupNode.SetAttribute("ShortName", partnerRow.PartnerShortName.ToString());

                    if (personRow != null)
                    {
                        partnerNode.SetAttribute("FirstName", personRow.FirstName.ToString());
                        partnerNode.SetAttribute("LastName", personRow.FamilyName.ToString());
                        partnerNode.SetAttribute("Title", personRow.Title.ToString());
                    }
                    else if (familyRow != null)
                    {
                        partnerNode.SetAttribute("FirstName", familyRow.FirstName.ToString());
                        partnerNode.SetAttribute("LastName", familyRow.FamilyName.ToString());
                        partnerNode.SetAttribute("Title", familyRow.Title.ToString());
                    }
                    else if (organisationRow != null)
                    {
                        partnerNode.SetAttribute("Name", organisationRow.OrganisationName.ToString());
                    }

                    partnerNode.SetAttribute("CreatedAt", partnerRow.DateCreated.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                    // special types
                    string specialTypes = "";
                    MainDS.PPartnerType.DefaultView.RowFilter = PPartnerTypeTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();

                    foreach (DataRowView rv in MainDS.PPartnerType.DefaultView)
                    {
                        if (specialTypes.Length > 0)
                        {
                            specialTypes += ", ";
                        }

                        specialTypes += ((PPartnerTypeRow)rv.Row).TypeCode;
                    }

                    if (specialTypes.Length > 0)
                    {
                        partnerNode.SetAttribute("SpecialTypes", specialTypes);
                    }

                    // addresses
                    DataView partnerLocationView = MainDS.PPartnerLocation.DefaultView;
                    partnerLocationView.RowFilter = PPartnerLocationTable.GetPartnerKeyDBName() + " = " + partnerRow.PartnerKey.ToString();
                    Int32 addressCounter = 0;

                    foreach (DataRowView rv in partnerLocationView)
                    {
                        XmlElement addressNode = PartnerData.CreateElement("Address" + (addressCounter > 0 ? addressCounter.ToString() : ""));
                        addressCounter++;
                        partnerNode.AppendChild(addressNode);

                        PPartnerLocationRow partnerLocationRow = (PPartnerLocationRow)rv.Row;

                        DataView locationView = MainDS.PLocation.DefaultView;
                        locationView.RowFilter =
                            PLocationTable.GetSiteKeyDBName() + "=" + partnerLocationRow.SiteKey.ToString() + " AND " +
                            PLocationTable.GetLocationKeyDBName() + "=" + partnerLocationRow.LocationKey.ToString();

                        if (locationView.Count > 0)
                        {
                            PLocationRow locationRow = (PLocationRow)locationView[0].Row;

                            addressNode.SetAttribute("Street", locationRow.StreetName);
                            addressNode.SetAttribute("City", locationRow.City);
                            addressNode.SetAttribute("PostCode", locationRow.PostalCode);
                        }

                        addressNode.SetAttribute("Email", partnerLocationRow.EmailAddress);
                        addressNode.SetAttribute("Phone", partnerLocationRow.TelephoneNumber);
                        addressNode.SetAttribute("MobilePhone", partnerLocationRow.MobileNumber);
                    }

                    // TODO: notes
                }
            }

            return TXMLParser.XmlToString(PartnerData);
        }
    }
}