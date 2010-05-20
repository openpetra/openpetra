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
using System.Data;
using System.Windows.Forms;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmPartnerImport
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public bool SaveChanges()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void FileSave(System.Object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void InitializeManualCode()
        {
            pnlImportRecord.Enabled = false;
        }

        XmlNode FCurrentPartnerNode = null;
        private void OpenFile(System.Object sender, EventArgs e)
        {
            if (!FPetraUtilsObject.IsEnabled("actStartImport"))
            {
                MessageBox.Show(Catalog.GetString("Please cancel the current import before selecting a different file"));
                return;
            }

            string filename;

            XmlDocument doc = TImportExportDialogs.ImportWithDialog(Catalog.GetString("Select the file for importing partners"), out filename);

            if (doc != null)
            {
                txtFilename.Text = filename;

                XmlNode root = doc.FirstChild.NextSibling;
                FCurrentPartnerNode = root.FirstChild;
            }
        }

        private void StartImport(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode == null)
            {
                OpenFile(null, null);
            }

            if (FCurrentPartnerNode == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a text file containing the partners first"),
                    Catalog.GetString("Need a file to import"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            this.FPetraUtilsObject.EnableAction("actStartImport", false);

            FCurrentPartnerNode = SkipImportedPartners(FCurrentPartnerNode);

            pnlImportRecord.Enabled = true;

            DisplayCurrentRecord();
        }

        private void DisplayCurrentRecord()
        {
            if (FCurrentPartnerNode == null)
            {
                grdParsedValues.DataSource = null;
                grdMatchingRecords.DataSource = null;
                pnlImportRecord.Enabled = false;
                return;
            }

            DataTable ValuePairs = new DataTable();

            ValuePairs.Columns.Add(new DataColumn("Attribute", typeof(string)));
            ValuePairs.Columns.Add(new DataColumn("Value", typeof(string)));

            if (FCurrentPartnerNode != null)
            {
                foreach (XmlAttribute attr in FCurrentPartnerNode.Attributes)
                {
                    DataRow valuePair = ValuePairs.NewRow();
                    valuePair["Attribute"] = attr.Name;
                    valuePair["Value"] = attr.Value;
                    ValuePairs.Rows.Add(valuePair);
                }
            }

            grdParsedValues.Columns.Clear();
            grdParsedValues.AddTextColumn(Catalog.GetString("Attribute"), ValuePairs.Columns["Attribute"], 150);
            grdParsedValues.AddTextColumn(Catalog.GetString("Value"), ValuePairs.Columns["Value"], 150);
            ValuePairs.DefaultView.AllowNew = false;
            grdParsedValues.DataSource = new DevAge.ComponentModel.BoundDataView(ValuePairs.DefaultView);

            // get all partners with same surname in that city
            PartnerFindTDS result = TRemote.MPartner.Partner.WebConnectors.FindPartners("",
                TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FAMILYNAME),
                TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_CITY),
                new StringCollection());

            grdMatchingRecords.Columns.Clear();
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Class"), result.SearchResult.ColumnPartnerClass, 50);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Name"), result.SearchResult.ColumnPartnerShortName, 200);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("Address"), result.SearchResult.ColumnStreetName, 200);
            grdMatchingRecords.AddTextColumn(Catalog.GetString("City"), result.SearchResult.ColumnCity, 150);
            result.SearchResult.DefaultView.AllowNew = false;
            grdMatchingRecords.DataSource = new DevAge.ComponentModel.BoundDataView(result.SearchResult.DefaultView);
        }

        private void CancelImport(Object sender, EventArgs e)
        {
            FCurrentPartnerNode = null;
            this.FPetraUtilsObject.EnableAction("actStartImport", true);
        }

        private XmlNode SkipImportedPartners(XmlNode ACurrentNode)
        {
            // TODO check for import settings, which partners to skip etc

            return ACurrentNode;
        }

        private void SkipRecord(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode != null)
            {
                FCurrentPartnerNode = FCurrentPartnerNode.NextSibling;

                FCurrentPartnerNode = SkipImportedPartners(FCurrentPartnerNode);
            }

            DisplayCurrentRecord();
        }

        /// <summary>
        /// returns the gender of the currently selected partner
        /// </summary>
        /// <returns></returns>
        private string GetGenderCode()
        {
            string gender = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_GENDER);

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

        private string GetTitle()
        {
            if (TXMLParser.HasAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE))
            {
                return TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE);
            }

            string genderCode = GetGenderCode();

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

        private string GetMaritalStatusCode()
        {
            if (TXMLParser.HasAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS))
            {
                string maritalStatus = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MARITALSTATUS);

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

        private void CreateNewFamilyAndPerson(Object sender, EventArgs e)
        {
            if (FCurrentPartnerNode == null)
            {
                return;
            }

            if (TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_PARTNERKEY).Length > 0)
            {
                // it would not make any sense to create a partner if there is already a partner key
                return;
            }

            PartnerEditTDS MainDS = new PartnerEditTDS();

            PPartnerRow newPartner = MainDS.PPartner.NewRowTyped();
            MainDS.PPartner.Rows.Add(newPartner);

            newPartner.PartnerKey = TRemote.MPartner.Partner.WebConnectors.NewPartnerKey(-1);
            newPartner.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
            newPartner.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            FCurrentPartnerNode.Attributes[MPartnerConstants.PARTNERIMPORT_PARTNERKEY].Value = newPartner.PartnerKey.ToString();

            if (TXMLParser.HasAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_AQUISITION))
            {
                newPartner.AcquisitionCode = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_AQUISITION);
            }
            else
            {
                newPartner.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
            }

            newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_DEFAULT;

            if (TXMLParser.HasAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE))
            {
                newPartner.AddresseeTypeCode = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESSEE_TYPE);
            }
            else
            {
                string gender = GetGenderCode();

                if (gender == MPartnerConstants.GENDER_MALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_MALE;
                }
                else if (gender == MPartnerConstants.GENDER_FEMALE)
                {
                    newPartner.AddresseeTypeCode = MPartnerConstants.ADDRESSEETYPE_FEMALE;
                }
            }

            if (TXMLParser.HasAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LANGUAGE))
            {
                newPartner.LanguageCode = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LANGUAGE);
            }
            else if (TUserDefaults.HasDefault(TUserDefaults.PARTNER_LANGUAGECODE))
            {
                newPartner.LanguageCode = TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_LANGUAGECODE);
            }

            PFamilyRow newFamily = MainDS.PFamily.NewRowTyped();
            MainDS.PFamily.Rows.Add(newFamily);

            newFamily.PartnerKey = newPartner.PartnerKey;
            newFamily.FirstName = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FIRSTNAME);
            newFamily.FamilyName = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_FAMILYNAME);
            newFamily.Title = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_TITLE);
            newPartner.PartnerShortName = Calculations.DeterminePartnerShortName(newFamily.FamilyName, newFamily.Title, newFamily.FirstName);

            string[] giftReceiptingDefaults = TSystemDefaults.GetSystemDefault("GiftReceiptingDefaults", ",no").Split(new char[] { ',' });
            newPartner.ReceiptLetterFrequency = giftReceiptingDefaults[0];
            newPartner.ReceiptEachGift = giftReceiptingDefaults[1] == "YES" || giftReceiptingDefaults[1] == "TRUE";

            newFamily.MaritalStatus = GetMaritalStatusCode();

            // TODO check for existing location
            PLocationRow newLocation = MainDS.PLocation.NewRowTyped(true);
            MainDS.PLocation.Rows.Add(newLocation);
            newLocation.LocationKey = -1;
            newLocation.Locality = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_LOCALITY);
            newLocation.StreetName = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_STREETNAME);
            newLocation.Address3 = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_ADDRESS);
            newLocation.PostalCode = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_POSTALCODE);
            newLocation.City = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_CITY);
            newLocation.County = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_COUNTY);
            newLocation.CountryCode = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_COUNTRYCODE);

            PPartnerLocationRow partnerlocation = MainDS.PPartnerLocation.NewRowTyped(true);
            partnerlocation.LocationKey = -1;
            partnerlocation.SiteKey = 0;
            partnerlocation.PartnerKey = newPartner.PartnerKey;
            partnerlocation.DateEffective = DateTime.Now;
            partnerlocation.LocationType = MPartnerConstants.LOCATIONTYPE_HOME;
            partnerlocation.SendMail = false;
            partnerlocation.EmailAddress = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_EMAIL);
            partnerlocation.TelephoneNumber = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_PHONE);
            partnerlocation.MobileNumber = TXMLParser.GetAttribute(FCurrentPartnerNode, MPartnerConstants.PARTNERIMPORT_MOBILEPHONE);
            MainDS.PPartnerLocation.Rows.Add(partnerlocation);

            TVerificationResultCollection VerificationResult;

            if (!TRemote.MPartner.Partner.WebConnectors.SavePartner(MainDS, out VerificationResult))
            {
                MessageBox.Show(VerificationResult.BuildVerificationResultString(), Catalog.GetString("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            // Reload the partner find with the new data, and also the grid with the partner key etc
            DisplayCurrentRecord();
        }
    }
}