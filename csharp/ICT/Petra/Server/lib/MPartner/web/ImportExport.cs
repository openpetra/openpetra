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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Import;
using Ict.Petra.Server.MPartner.Partner;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.MPartner.ImportExport.WebConnectors
{
    /// <summary>
    /// import and export partner data
    /// </summary>
    public class TImportExportWebConnector
    {
        static Int64 NewPartnerKey = -1;

        private static void ParsePartners(ref PartnerImportExportTDS AMainDS, XmlNode ACurNode)
        {
            XmlNode LocalNode = ACurNode;

            while (LocalNode != null)
            {
                if (LocalNode.Name.StartsWith("PartnerGroup"))
                {
                    ParsePartners(ref AMainDS, LocalNode.FirstChild);
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
                    newPartner.PartnerKey = TImportExportWebConnector.NewPartnerKey;
                    TImportExportWebConnector.NewPartnerKey--;

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
                        if (TAppSettingsManager.GetValue("AllowCreationPersonRecords", "true", false).ToLower() != "true")
                        {
                            throw new Exception(
                                "We are currently not supporting import of PERSON records, until we have resolved the issues with household/family. "
                                +
                                "Please add configuration parameter AllowCreationPersonRecords with value true if you want to use PERSON records");
                        }

                        // TODO
                    }
                    else if (TYml2Xml.GetAttributeRecursive(LocalNode, "class") == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        // TODO
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
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportPartnersFromYml(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            return TImportExportYml.ImportPartners(AXmlPartnerData, out AVerificationResult);
        }

        /// <summary>
        /// This imports partners from a CSV file
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportFromCSVFile(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlPartnerData);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            PartnerImportExportTDS MainDS = TPartnerImportCSV.ImportData(root, ref AVerificationResult);

            return MainDS;
        }

        /// <summary>
        /// This imports partners from a partner extract which is a text file format used already with Petra 2.x
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ImportFromPartnerExtract(string[] ATextFileLines, out TVerificationResultCollection AVerificationResult)
        {
            TPartnerFileImport Importer = new TPartnerFileImport();
            PartnerImportExportTDS MainDS = Importer.ImportAllData(ATextFileLines, string.Empty, false, out AVerificationResult);

            return MainDS;
        }

        private static String FNewRowDescription = "Auto-generated by Partner Import";
        private static String FImportContext;

        private static void AddVerificationResult(ref TVerificationResultCollection ReferenceResults, String AResultText, TResultSeverity Severity)
        {
            if (Severity != TResultSeverity.Resv_Status)
            {
                TLogging.Log(AResultText);
            }

            ReferenceResults.Add(new TVerificationResult(FImportContext, AResultText, Severity));
        }

        private static void AddVerificationResult(ref TVerificationResultCollection ReferenceResults, String AResultText)
        {
            AddVerificationResult(ref ReferenceResults, AResultText, TResultSeverity.Resv_Noncritical);
        }

        /// <summary>
        /// Check that I seem to have the right partner tables for this PartnerClass.
        ///
        /// If the child records (PPerson, PFamily, etc) have no ModificationID,
        /// (because these are new records - not originally from the database)
        /// I need to get the current one from the server to prevent it sulking.
        /// </summary>
        /// <param name="PartnerRow"></param>
        /// <param name="MainDS"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        /// <returns></returns>
        private static bool CheckPartnerClass(PPartnerRow PartnerRow,
            PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                if (MainDS.PFamily.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults, "Internal Error - No Family row for FAMILY", TResultSeverity.Resv_Critical);
                    return false;
                }

                PFamilyTable Table = PFamilyAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.PFamily[0].DateCreated = Table[0].DateCreated;
                    MainDS.PFamily[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.PFamily[0].ModificationId = Table[0].ModificationId;
                }
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                if (MainDS.PPerson.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults, "Internal Error - No Person row for PERSON", TResultSeverity.Resv_Critical);
                    return false;
                }

                PPersonTable Table = PPersonAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.PPerson[0].DateCreated = Table[0].DateCreated;
                    MainDS.PPerson[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.PPerson[0].ModificationId = Table[0].ModificationId;
                }

                if ((MainDS.PPerson[0].IsFamilyIdNull()) || (MainDS.PPerson[0].FamilyId == 0)) // Every Person must have a FamilyId..
                {
                    bool FamilyIdOk = false;

                    if (MainDS.PPerson[0].PartnerKey > 0) // If I've got a real key, then my existing FamilyId might also be real
                    {
                        PPersonTable PersonTable = PPersonAccess.LoadByPrimaryKey(MainDS.PPerson[0].PartnerKey, Transaction);

                        if (PersonTable.Rows.Count != 0)
                        {
                            MainDS.PPerson[0].FamilyId = PersonTable[0].FamilyId;
                            FamilyIdOk = true;
                        }
                    }

                    if (!FamilyIdOk)  // Otherwise I'll just grab one that's available.
                    {
                        TPartnerFamilyIDHandling IdFactory = new TPartnerFamilyIDHandling();
                        TFamilyIDSuccessEnum FamIdRes = TFamilyIDSuccessEnum.fiSuccess;
                        int NewFamilyId;
                        String NewFamilyMsg;

                        FamIdRes = IdFactory.GetNewFamilyID(MainDS.PPerson[0].FamilyKey, out NewFamilyId, out NewFamilyMsg);

                        if (FamIdRes != TFamilyIDSuccessEnum.fiError)
                        {
                            MainDS.PPerson[0].FamilyId = NewFamilyId;
                        }
                        else
                        {
                            AddVerificationResult(ref ReferenceResults,
                                "Problem generating family id: " + NewFamilyMsg,
                                TResultSeverity.Resv_Critical);
                            return false;
                        }
                    }
                }
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                if (MainDS.PUnit.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults, "Internal Error - No Unit row for UNIT", TResultSeverity.Resv_Critical);
                    return false;
                }

                PUnitTable Table = PUnitAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.PUnit[0].DateCreated = Table[0].DateCreated;
                    MainDS.PUnit[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.PUnit[0].ModificationId = Table[0].ModificationId;
                }
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                if (MainDS.PChurch.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults, "Internal Error - No Church row for CHURCH", TResultSeverity.Resv_Critical);
                    return false;
                }

                PChurchTable Table = PChurchAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.PChurch[0].DateCreated = Table[0].DateCreated;
                    MainDS.PChurch[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.PChurch[0].ModificationId = Table[0].ModificationId;
                }
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                if (MainDS.POrganisation.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults,
                        "Internal Error - No Organisation row for ORGANISATION",
                        TResultSeverity.Resv_Critical);
                    return false;
                }

                POrganisationTable Table = POrganisationAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.POrganisation[0].DateCreated = Table[0].DateCreated;
                    MainDS.POrganisation[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.POrganisation[0].ModificationId = Table[0].ModificationId;
                }
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                if (MainDS.PBank.Rows.Count < 1)
                {
                    AddVerificationResult(ref ReferenceResults, "Internal Error - No Bank row for BANK", TResultSeverity.Resv_Critical);
                    return false;
                }

                PBankTable Table = PBankAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, Transaction);

                if (Table.Rows.Count > 0)
                {
                    MainDS.PBank[0].DateCreated = Table[0].DateCreated;
                    MainDS.PBank[0].CreatedBy = Table[0].CreatedBy;
                    MainDS.PBank[0].ModificationId = Table[0].ModificationId;
                }
            }

            return true;
        }

        private static void CheckLocationType(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // LocationType in PartnerLocation row
            foreach (PPartnerLocationRow rv in MainDS.PPartnerLocation.Rows)
            {
                if ((rv.LocationType != "") && (!PLocationTypeAccess.Exists(rv.LocationType, Transaction)))
                {
                    MainDS.PLocationType.DefaultView.RowFilter = String.Format("{0}='{1}'", PLocationTypeTable.GetCodeDBName(), rv.LocationType);

                    if (MainDS.PLocationType.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new location type " + rv.LocationType);
                        PLocationTypeRow Row = MainDS.PLocationType.NewRowTyped();
                        Row.Code = rv.LocationType;
                        Row.Description = FNewRowDescription;
                        MainDS.PLocationType.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckBusiness(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // PBusiness: If I'm importing any organisations, they can only do business that I'm expecting.
            foreach (POrganisationRow rv in MainDS.POrganisation.Rows)
            {
                if ((rv.BusinessCode != "") && (!PBusinessAccess.Exists(rv.BusinessCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding new Business Code " + rv.BusinessCode);
                    PBusinessRow Row = MainDS.PBusiness.NewRowTyped();
                    Row.BusinessCode = rv.BusinessCode;
                    Row.BusinessDescription = FNewRowDescription;
                    MainDS.PBusiness.Rows.Add(Row);
                }
            }
        }

        private static void CheckLanguage(PartnerImportExportTDS MainDS,
            PPartnerRow PartnerRow,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Language: we can only speak languages that we've heard of
            StringCollection RequiredLanguages = new StringCollection();

            if (!PLanguageAccess.Exists(PartnerRow.LanguageCode, Transaction))
            {
                RequiredLanguages.Add(PartnerRow.LanguageCode);
            }

            foreach (PmPersonLanguageRow rv in MainDS.PmPersonLanguage.Rows)
            {
                if ((!RequiredLanguages.Contains(rv.LanguageCode)) && (!PLanguageAccess.Exists(rv.LanguageCode, Transaction)))
                {
                    RequiredLanguages.Add(rv.LanguageCode);
                }
            }

            foreach (PmShortTermApplicationRow rv in MainDS.PmShortTermApplication.Rows)
            {
                if ((!RequiredLanguages.Contains(rv.StCongressLanguage)) && (!PLanguageAccess.Exists(rv.StCongressLanguage, Transaction)))
                {
                    RequiredLanguages.Add(rv.StCongressLanguage);
                }
            }

            foreach (String NewLanguage in RequiredLanguages)
            {
                if (NewLanguage != "")
                {
                    MainDS.PLanguage.DefaultView.RowFilter = String.Format("{0}='{1}'", PLanguageTable.GetLanguageCodeDBName(), NewLanguage);

                    if (MainDS.PLanguage.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Language Code " + NewLanguage);
                        PLanguageRow Row = MainDS.PLanguage.NewRowTyped();
                        Row.LanguageCode = NewLanguage;
                        Row.LanguageDescription = FNewRowDescription;
                        MainDS.PLanguage.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckAcquisitionCode(PartnerImportExportTDS MainDS,
            PPartnerRow PartnerRow,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Acquisition: Check that partner's acquisition code exists in database
            if ((PartnerRow.AcquisitionCode != "") && (!PAcquisitionAccess.Exists(PartnerRow.AcquisitionCode, Transaction)))
            {
                AddVerificationResult(ref ReferenceResults, "Adding new Acquisition Code " + PartnerRow.AcquisitionCode);
                PAcquisitionRow Row = MainDS.PAcquisition.NewRowTyped();
                Row.AcquisitionCode = PartnerRow.AcquisitionCode;
                Row.AcquisitionDescription = FNewRowDescription;
                MainDS.PAcquisition.Rows.Add(Row);
            }
        }

        /// <summary>
        /// If the address I'm importing is already on file, I don't want to create another row.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="PartnerRow"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        private static void CheckAddresses(PartnerImportExportTDS MainDS,
            PPartnerRow PartnerRow,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            PLocationTable LocationTable = PLocationAccess.LoadViaPPartner(PartnerRow.PartnerKey, Transaction);

            if (LocationTable.Rows.Count == 0)
            {
                return;
            }

            for (int ImportPartnerLocationRowIdx = 0; ImportPartnerLocationRowIdx < MainDS.PPartnerLocation.Rows.Count; )
            {
                PPartnerLocationRow ImportPartnerLocationRow = (PPartnerLocationRow)MainDS.PPartnerLocation[ImportPartnerLocationRowIdx];
                MainDS.PLocation.DefaultView.RowFilter = String.Format("{0}={1}",
                    PLocationTable.GetLocationKeyDBName(), ImportPartnerLocationRow.LocationKey);
                bool RowRemoved = false;

                if (MainDS.PLocation.DefaultView.Count > 0)
                {
                    PLocationRow ImportLocationRow = (PLocationRow)MainDS.PLocation.DefaultView[0].Row;

                    // Now I want to find out whether this row exists in my database.
                    foreach (PLocationRow DbLocationRow in LocationTable.Rows)
                    {
                        if (
                            (DbLocationRow.StreetName == ImportLocationRow.StreetName)
                            && (DbLocationRow.Locality == ImportLocationRow.Locality)
                            && (DbLocationRow.City == ImportLocationRow.City)
                            && (DbLocationRow.PostalCode == ImportLocationRow.PostalCode)
                            )
                        {
                            String ExistingAddress = Calculations.DetermineLocationString(DbLocationRow,
                                Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);

                            MainDS.PLocation.Rows.Remove(ImportLocationRow);
                            MainDS.PPartnerLocation.Rows.RemoveAt(ImportPartnerLocationRowIdx);

                            AddVerificationResult(ref ReferenceResults, "Existing address used: " + ExistingAddress, TResultSeverity.Resv_Status);
                            RowRemoved = true;
                            break;  // If there's already a duplicate in the database, I can't fix that here...
                        }
                    }
                }

                if (!RowRemoved)
                {
                    ImportPartnerLocationRowIdx++; // There is no auto-increment on the "for" loop.
                }
            }
        }

        private static void CheckAddresseeTypeCode(PartnerImportExportTDS MainDS,
            PPartnerRow PartnerRow,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Addresssee type: Check that we know how to address this partner:
            if ((PartnerRow.AddresseeTypeCode != "") && (!PAddresseeTypeAccess.Exists(PartnerRow.AddresseeTypeCode, Transaction)))
            {
                AddVerificationResult(ref ReferenceResults, "Adding new Addressee Type Code " + PartnerRow.AddresseeTypeCode);
                PAddresseeTypeRow Row = MainDS.PAddresseeType.NewRowTyped();
                Row.AddresseeTypeCode = PartnerRow.AddresseeTypeCode;
                Row.Description = FNewRowDescription;
                MainDS.PAddresseeType.Rows.Add(Row);
            }
        }

        private static void CheckAbilityArea(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Ability Area: if there's any abilities, they must only be in known areas!
            // Ability Level: only import known level identifiers
            foreach (PmPersonAbilityRow rv in MainDS.PmPersonAbility.Rows)
            {
                if ((rv.AbilityAreaName != "") && (!PtAbilityAreaAccess.Exists(rv.AbilityAreaName, Transaction)))
                {
                    MainDS.PtAbilityArea.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtAbilityAreaTable.GetAbilityAreaNameDBName(), rv.AbilityAreaName);

                    if (MainDS.PtAbilityArea.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Ability Area " + rv.AbilityAreaName);
                        PtAbilityAreaRow Row = MainDS.PtAbilityArea.NewRowTyped();
                        Row.AbilityAreaName = rv.AbilityAreaName;
                        Row.AbilityAreaDescr = FNewRowDescription;
                        MainDS.PtAbilityArea.Rows.Add(Row);
                    }
                }

                if (!PtAbilityLevelAccess.Exists(rv.AbilityLevel, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Removing unknown Ability level " + rv.AbilityLevel);
                    rv.AbilityLevel = 99; // If I don't know what this AbilityLevel means, I can only say, "unknown".
                }
            }
        }

        private static void CheckPartnerInterest(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Interest: A PartnerInterest entry must have a cossesponding entry in PInterest
            // (The PPartnerInterest table has the category as a custom field)

            foreach (PartnerImportExportTDSPPartnerInterestRow rv in MainDS.PPartnerInterest.Rows)
            {
                if ((rv.Interest != "") && (!PInterestAccess.Exists(rv.Interest, Transaction)))
                {
                    PInterestRow Row = MainDS.PInterest.NewRowTyped();
                    AddVerificationResult(ref ReferenceResults, "Adding new Interest " + rv.Interest);
                    Row.Interest = rv.Interest;
                    Row.Category = rv.Category;
                    Row.Description = FNewRowDescription;

                    MainDS.PInterest.Rows.Add(Row);
                }

                if ((rv.Category != "") && (!PInterestCategoryAccess.Exists(rv.Category, Transaction)))
                {
                    MainDS.PInterestCategory.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PInterestCategoryTable.GetCategoryDBName(), rv.Category);

                    if (MainDS.PInterestCategory.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Interest Category " + rv.Category);
                        PInterestCategoryRow Row = MainDS.PInterestCategory.NewRowTyped();
                        Row.Category = rv.Category;
                        Row.Description = FNewRowDescription;
                        MainDS.PInterestCategory.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckPartnerType(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // PType: In the previous version, unknown types were not imported.
            foreach (PPartnerTypeRow rv in MainDS.PPartnerType.Rows)
            {
                if ((rv.TypeCode != "") && (!PTypeAccess.Exists(rv.TypeCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding new Partner Type " + rv.TypeCode);
                    PTypeRow Row = MainDS.PType.NewRowTyped();
                    Row.TypeCode = rv.TypeCode;
                    Row.TypeDescription = FNewRowDescription;
                    MainDS.PType.Rows.Add(Row);
                }
            }
        }

        private static void CheckChurchDenomination(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Denomination: If a church belongs to one, we need to know about it.
            foreach (PChurchRow rv in MainDS.PChurch.Rows)
            {
                if ((rv.DenominationCode != "") & (!PDenominationAccess.Exists(rv.DenominationCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding new Denomination " + rv.DenominationCode);
                    PDenominationRow Row = MainDS.PDenomination.NewRowTyped();
                    Row.DenominationCode = rv.DenominationCode;
                    Row.DenominationName = FNewRowDescription;
                    MainDS.PDenomination.Rows.Add(Row);
                }
            }
        }

        private static void CheckContactRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction ATransaction)
        {
            int ContactId = 0;

            foreach (PartnerImportExportTDSPPartnerContactRow Row in MainDS.PPartnerContact.Rows)
            {
                PPartnerContactTable Tbl = PPartnerContactAccess.LoadByUniqueKey(Row.PartnerKey, Row.ContactDate, Row.ContactTime, ATransaction);
                bool HereAlready = false;

                if (Tbl.Rows.Count > 0)         // I've already imported this..
                {
                    Row.AcceptChanges();             // This should make the DB update instead of Add
                    Row.ContactId = Tbl[0].ContactId;
                    Row.ModificationId = Tbl[0].ModificationId;
                    HereAlready = true;
                }

                if (Row.ContactId == 0)
                {
                    Row.ContactId = --ContactId;
                }

                // The row has custom Attr and Detail fields, which I need to put into the right tables..
                if (!HereAlready && (Row.ContactAttr != ""))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding new contact attribute: " + Row.ContactAttr, TResultSeverity.Resv_Status);
                    PContactAttributeDetailRow PcadRow = MainDS.PContactAttributeDetail.NewRowTyped();
                    PcadRow.ContactAttributeCode = Row.ContactAttr;
                    PcadRow.ContactAttrDetailCode = Row.ContactDetail;
                    PcadRow.ContactAttrDetailDescr = FNewRowDescription;
                    PContactAttributeDetailAccess.AddOrModifyRecord(
                        PcadRow.ContactAttributeCode,
                        PcadRow.ContactAttrDetailCode,
                        MainDS.PContactAttributeDetail,
                        PcadRow, false, ATransaction);

                    PPartnerContactAttributeRow PcaRow = MainDS.PPartnerContactAttribute.NewRowTyped();
                    PcaRow.ContactId = Row.ContactId;
                    PcaRow.ContactAttributeCode = Row.ContactAttr;
                    PcaRow.ContactAttrDetailCode = Row.ContactDetail;
                    PPartnerContactAttributeAccess.AddOrModifyRecord(
                        PcaRow.ContactId,
                        PcaRow.ContactAttributeCode,
                        PcaRow.ContactAttrDetailCode,
                        MainDS.PPartnerContactAttribute,
                        PcaRow, false, ATransaction);

                    PContactAttributeRow CaRow = MainDS.PContactAttribute.NewRowTyped();
                    CaRow.ContactAttributeDescr = FNewRowDescription;
                    CaRow.ContactAttributeCode = Row.ContactAttr;
                    PContactAttributeAccess.AddOrModifyRecord(
                        CaRow.ContactAttributeCode,
                        MainDS.PContactAttribute,
                        CaRow,
                        false,
                        ATransaction);
                }

                if (!PMethodOfContactAccess.Exists(Row.ContactCode, ATransaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding new method of contact: " + Row.ContactCode, TResultSeverity.Resv_Status);
                    PMethodOfContactRow MocRow = MainDS.PMethodOfContact.NewRowTyped();
                    MocRow.MethodOfContactCode = Row.ContactCode;
                    MocRow.Description = FNewRowDescription;
                    MocRow.ValidMethod = true;
                    MainDS.PMethodOfContact.Rows.Add(MocRow);
                }
            }
        }

        private static void CheckApplication(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // The Application must be unique - if it's present already, I can update the existing record.
            // ApplicationStatus: Application Status must be listed in PtApplicantStatus
            // ApplicantType: applicants must be of known types
            // ApplicationType: applications must be of known types

            for (int RowIdx = 0; RowIdx < MainDS.PmGeneralApplication.Rows.Count; RowIdx++)
            {
                PmGeneralApplicationRow GenAppRow = MainDS.PmGeneralApplication[RowIdx];

                // Check if this row already exists, using the unique key
                if (PmGeneralApplicationAccess.Exists(GenAppRow.PartnerKey, GenAppRow.GenAppDate, GenAppRow.AppTypeName, GenAppRow.OldLink,
                        Transaction))
                {
                    // If it does, I need to update and not add.
                    PmGeneralApplicationTable Tbl = PmGeneralApplicationAccess.LoadByUniqueKey(
                        GenAppRow.PartnerKey, GenAppRow.GenAppDate, GenAppRow.AppTypeName, GenAppRow.OldLink, Transaction);
                    PmGeneralApplicationRow ExistingRow = Tbl[0];

                    GenAppRow.AcceptChanges();
                    GenAppRow.ModificationId = ExistingRow.ModificationId;

                    AddVerificationResult(ref ReferenceResults, "Existing Application record updated.");
                }

                if ((GenAppRow.GenApplicationStatus != "") && (!PtApplicantStatusAccess.Exists(GenAppRow.GenApplicationStatus, Transaction)))
                {
                    MainDS.PtApplicantStatus.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtApplicantStatusTable.GetCodeDBName(), GenAppRow.GenApplicationStatus);

                    if (MainDS.PtApplicantStatus.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Applicant Status " + GenAppRow.GenApplicationStatus);
                        PtApplicantStatusRow Row = MainDS.PtApplicantStatus.NewRowTyped();
                        Row.Code = GenAppRow.GenApplicationStatus;
                        Row.Description = FNewRowDescription;
                        MainDS.PtApplicantStatus.Rows.Add(Row);
                    }
                }

                if (GenAppRow.IsGenApplicantTypeNull())
                {
                    GenAppRow.GenApplicantType = "Gen App";  // This field is scheduled for deletion, but for now it's NOT NULL.
                }

                if (GenAppRow.AppTypeName == "")
                {
                    GenAppRow.AppTypeName = "CONFERENCE";
                }

                if ((GenAppRow.AppTypeName != "") && (!PtApplicationTypeAccess.Exists(GenAppRow.AppTypeName, Transaction)))
                {
                    MainDS.PtApplicationType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtApplicationTypeTable.GetAppTypeNameDBName(), GenAppRow.AppTypeName);

                    if (MainDS.PtApplicationType.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Application Type " + GenAppRow.AppTypeName);
                        PtApplicationTypeRow Row = MainDS.PtApplicationType.NewRowTyped();
                        Row.AppTypeName = GenAppRow.AppTypeName;
                        Row.AppTypeDescr = FNewRowDescription;
                        MainDS.PtApplicationType.Rows.Add(Row);
                    }
                }

                if ((GenAppRow.GenContact1 != "") && (!PtContactAccess.Exists(GenAppRow.GenContact1, Transaction)))
                {
                    MainDS.PtContact.DefaultView.RowFilter = String.Format("{0}='{1}'", PtContactTable.GetContactNameDBName(), GenAppRow.GenContact1);

                    if (MainDS.PtContact.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Contact Name " + GenAppRow.GenContact1);
                        PtContactRow Row = MainDS.PtContact.NewRowTyped();
                        Row.ContactName = GenAppRow.GenContact1;
                        Row.ContactDescr = FNewRowDescription;
                        MainDS.PtContact.Rows.Add(Row);
                    }
                }

                if ((GenAppRow.GenContact2 != "") && (!PtContactAccess.Exists(GenAppRow.GenContact2, Transaction)))
                {
                    MainDS.PtContact.DefaultView.RowFilter = String.Format("{0}='{1}'", PtContactTable.GetContactNameDBName(), GenAppRow.GenContact2);

                    if (MainDS.PtContact.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Contact Name " + GenAppRow.GenContact2);
                        PtContactRow Row = MainDS.PtContact.NewRowTyped();
                        Row.ContactName = GenAppRow.GenContact2;
                        Row.ContactDescr = FNewRowDescription;
                        MainDS.PtContact.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckCommitmentStatus(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Commitment Status Code: I'm not going to add this, but I should inform the user...
            foreach (PmStaffDataRow rv in MainDS.PmStaffData.Rows)
            {
                if ((rv.StatusCode != "") && (!PmCommitmentStatusAccess.Exists(rv.StatusCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Removing unknown Commitment Status " + rv.StatusCode);
                    rv.StatusCode = "";

/*
 *                  TLogging.Log("Adding new commitment status code " + rv.StatusCode);
 *                  PmCommitmentStatusRow commitmentStatusRow = MainDS.PmCommitmentStatus.NewRowTyped();
 *                  commitmentStatusRow.Code = rv.StatusCode;
 *                  commitmentStatusRow.Desc = NewRowDescription;
 *                  MainDS.PmCommitmentStatus.Rows.Add(commitmentStatusRow);
 */
                }
            }
        }

        private static void CheckSTApplicationRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Special Applicant: keep a list of them!
            // Arrival Point:
            // Transport Type:
            // Leadership Rating:
            // PartyType:
            // PmOutreachRole: All from Short Term Application Rows

            foreach (PmShortTermApplicationRow rv in MainDS.PmShortTermApplication.Rows)
            {
                if ((rv.StSpecialApplicant != "") && (!PtSpecialApplicantAccess.Exists(rv.StSpecialApplicant, Transaction)))
                {
                    MainDS.PtSpecialApplicant.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtSpecialApplicantTable.GetCodeDBName(), rv.StSpecialApplicant);

                    if (MainDS.PtSpecialApplicant.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Special Applicant " + rv.StSpecialApplicant);
                        PtSpecialApplicantRow Row = MainDS.PtSpecialApplicant.NewRowTyped();
                        Row.Code = rv.StSpecialApplicant;
                        Row.Description = FNewRowDescription;
                        MainDS.PtSpecialApplicant.Rows.Add(Row);
                    }
                }

                if ((rv.ArrivalPointCode != "") && (!PtArrivalPointAccess.Exists(rv.ArrivalPointCode, Transaction)))
                {
                    MainDS.PtArrivalPoint.DefaultView.RowFilter = String.Format("{0}='{1}'", PtArrivalPointTable.GetCodeDBName(), rv.ArrivalPointCode);

                    if (MainDS.PtArrivalPoint.DefaultView.Count == 0)  // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new arrival point code '" + rv.ArrivalPointCode + "'");
                        PtArrivalPointRow Row = MainDS.PtArrivalPoint.NewRowTyped();
                        Row.Code = rv.ArrivalPointCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtArrivalPoint.Rows.Add(Row);
                    }
                }

                if ((rv.DeparturePointCode != "") && (!PtArrivalPointAccess.Exists(rv.DeparturePointCode, Transaction)))
                {
                    MainDS.PtArrivalPoint.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtArrivalPointTable.GetCodeDBName(), rv.DeparturePointCode);

                    if (MainDS.PtArrivalPoint.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new arrival point code '" + rv.DeparturePointCode + "'");
                        PtArrivalPointRow Row = MainDS.PtArrivalPoint.NewRowTyped();
                        Row.Code = rv.DeparturePointCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtArrivalPoint.Rows.Add(Row);
                    }
                }

                if ((rv.TravelTypeToCongCode != "") && (!PtTravelTypeAccess.Exists(rv.TravelTypeToCongCode, Transaction)))
                {
                    MainDS.PtTravelType.DefaultView.RowFilter = String.Format("{0}='{1}'", PtTravelTypeTable.GetCodeDBName(), rv.TravelTypeToCongCode);

                    if (MainDS.PtTravelType.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Travel Type " + rv.TravelTypeToCongCode);
                        PtTravelTypeRow Row = MainDS.PtTravelType.NewRowTyped();
                        Row.Code = rv.TravelTypeToCongCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtTravelType.Rows.Add(Row);
                    }
                }

                if ((rv.TravelTypeFromCongCode != "") && (!PtTravelTypeAccess.Exists(rv.TravelTypeFromCongCode, Transaction)))
                {
                    MainDS.PtTravelType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtTravelTypeTable.GetCodeDBName(), rv.TravelTypeFromCongCode);

                    if (MainDS.PtTravelType.DefaultView.Count == 0) // I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Travel Type " + rv.TravelTypeFromCongCode);
                        PtTravelTypeRow Row = MainDS.PtTravelType.NewRowTyped();
                        Row.Code = rv.TravelTypeFromCongCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtTravelType.Rows.Add(Row);
                    }
                }

                if ((rv.OutreachRole != "") && (!PtCongressCodeAccess.Exists(rv.OutreachRole, Transaction)))
                {
                    MainDS.PtCongressCode.DefaultView.RowFilter = String.Format("{0}='{1}'", PtCongressCodeTable.GetCodeDBName(), (rv.OutreachRole));

                    if (MainDS.PtCongressCode.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Congress Code " + rv.OutreachRole);
                        PtCongressCodeRow Row = MainDS.PtCongressCode.NewRowTyped();
                        Row.Code = rv.OutreachRole;
                        Row.Description = FNewRowDescription;
                        MainDS.PtCongressCode.Rows.Add(Row);
                    }
                }
            }
        }

/*
 *      private static void CheckVisionRefs(PartnerImportExportTDS MainDS, ref TVerificationResultCollection ReferenceResults, TDBTransaction Transaction)
 *     {
 *          // Vision: update _area and _level tables
 *          foreach (PmPersonVisionRow rv in MainDS.PmPersonVision.Rows)
 *          {
 *              if ((rv.VisionAreaName != "") && (!PtVisionAreaAccess.Exists(rv.VisionAreaName, Transaction)))
 *              {
 *                  MainDS.PtVisionArea.DefaultView.RowFilter = String.Format("{0}='{1}'", PtVisionAreaTable.GetVisionAreaNameDBName(), rv.VisionAreaName);
 *                  if (MainDS.PtVisionArea.DefaultView.Count == 0) // Check I've not just added this a moment ago..
 *                  {
 *                      AddVerificationResult(ref ReferenceResults, "Adding new Vision Area " + rv.VisionAreaName);
 *                      PtVisionAreaRow Row = MainDS.PtVisionArea.NewRowTyped();
 *                      Row.VisionAreaName = rv.VisionAreaName;
 *                      Row.VisionAreaDescr = NewRowDescription;
 *                      MainDS.PtVisionArea.Rows.Add(Row);
 *                  }
 *              }
 *
 *              if (!PtVisionLevelAccess.Exists(rv.VisionLevel, Transaction))
 *              {
 *                  AddVerificationResult(ref ReferenceResults, "Adding new Vision Level " + rv.VisionLevel);
 *                  PtVisionLevelRow Row = MainDS.PtVisionLevel.NewRowTyped();
 *                  Row.VisionLevel = rv.VisionLevel;
 *                  Row.VisionLevelDescr = NewRowDescription;
 *                  MainDS.PtVisionLevel.Rows.Add(Row);
 *              }
 *          }
 *      }
 */

        private static void CheckYearProgramRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Position: We can't apply for positions we've not heard of
            foreach (PmYearProgramApplicationRow rv in MainDS.PmYearProgramApplication.Rows)
            {
                if ((rv.PositionName != "") && (!PtPositionAccess.Exists(rv.PositionName, rv.PositionScope, Transaction)))
                {
                    MainDS.PtPosition.DefaultView.RowFilter = String.Format("{0}='{1}'", PtPositionTable.GetPositionNameDBName(), rv.PositionName);

                    if (MainDS.PtPosition.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Position " + rv.PositionName);
                        PtPositionRow Row = MainDS.PtPosition.NewRowTyped();
                        Row.PositionName = rv.PositionName;
                        Row.PositionScope = rv.PositionScope;
                        Row.PositionDescr = FNewRowDescription;
                        MainDS.PtPosition.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckQualificationRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Qualification Area and Qualification Level - Add them if they're not present.
            foreach (PmPersonQualificationRow rv in MainDS.PmPersonQualification.Rows)
            {
                if ((rv.QualificationAreaName != "") && (!PtQualificationAreaAccess.Exists(rv.QualificationAreaName, Transaction)))
                {
                    MainDS.PtQualificationArea.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtQualificationAreaTable.GetQualificationAreaNameDBName(), rv.QualificationAreaName);

                    if (MainDS.PtQualificationArea.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Qualification Area " + rv.QualificationAreaName);
                        PtQualificationAreaRow Row = MainDS.PtQualificationArea.NewRowTyped();
                        Row.QualificationAreaName = rv.QualificationAreaName;
                        Row.QualificationAreaDescr = FNewRowDescription;
                        MainDS.PtQualificationArea.Rows.Add(Row);
                    }
                }

                if (!PtQualificationLevelAccess.Exists(rv.QualificationLevel, Transaction))
                {
                    MainDS.PtQualificationLevel.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtQualificationLevelTable.GetQualificationLevelDBName(), rv.QualificationLevel);

                    if (MainDS.PtQualificationLevel.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Qualification Level " + rv.QualificationLevel);
                        PtQualificationLevelRow Row = MainDS.PtQualificationLevel.NewRowTyped();
                        Row.QualificationLevel = rv.QualificationLevel;
                        Row.QualificationLevelDescr = FNewRowDescription;
                        MainDS.PtQualificationLevel.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckMaritalStatus(PartnerImportExportTDS MainDS,
            PPartnerRow PartnerRow,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Marital Status: The Marital status code used must be present in PTMaritalStatus
            String RequiredMaritalStatus = "";

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                RequiredMaritalStatus = ((PFamilyRow)MainDS.PFamily.Rows[0]).MaritalStatus;
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                RequiredMaritalStatus = ((PPersonRow)MainDS.PPerson.Rows[0]).MaritalStatus;
            }

            if ((RequiredMaritalStatus != "") && (!PtMaritalStatusAccess.Exists(RequiredMaritalStatus, Transaction)))
            {
                AddVerificationResult(ref ReferenceResults, "Adding Marital Status " + RequiredMaritalStatus);
                PtMaritalStatusRow Row = MainDS.PtMaritalStatus.NewRowTyped();
                Row.Code = RequiredMaritalStatus;
                Row.Description = FNewRowDescription;
                MainDS.PtMaritalStatus.Rows.Add(Row);
            }
        }

        private static void CheckOccupation(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // POccupation: If there's a person, and they have an occupation, I need to have it listed.
            if (MainDS.PPerson.Rows.Count > 0)
            {
                String RequiredOccupation = ((PPersonRow)MainDS.PPerson.Rows[0]).OccupationCode;

                if ((RequiredOccupation != "") && (!POccupationAccess.Exists(RequiredOccupation, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Occupation Code " + RequiredOccupation);
                    POccupationRow Row = MainDS.POccupation.NewRowTyped();
                    Row.OccupationCode = RequiredOccupation;
                    Row.OccupationDescription = FNewRowDescription;
                    MainDS.POccupation.Rows.Add(Row);
                }
            }
        }

        private static void CheckUnitType(PartnerImportExportTDS AMainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction ATransaction)
        {
            // UUnitType: If there's a unit, I need to have its type listed.
            if (AMainDS.PUnit.Rows.Count > 0)
            {
                String RequiredUnitType = ((PUnitRow)AMainDS.PUnit.Rows[0]).UnitTypeCode;

                if ((RequiredUnitType != "") && (!UUnitTypeAccess.Exists(RequiredUnitType, ATransaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Unit Type " + RequiredUnitType);
                    UUnitTypeRow Row = AMainDS.UUnitType.NewRowTyped();
                    Row.UnitTypeCode = RequiredUnitType;
                    Row.UnitTypeName = FNewRowDescription;
                    AMainDS.UUnitType.Rows.Add(Row);
                }
            }
        }

        //
        // If my unit's parent is not found, I can't import it.
        // If it's OK, I need to check whether this UmUnitStructure record is new or not.
        private static bool CheckUnitParent(PartnerImportExportTDS AMainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction ATransaction)
        {
            bool EveryoneHasAParent = true;

            // UmUnitStructure: the ParentUnit must exist
            foreach (UmUnitStructureRow Row in AMainDS.UmUnitStructure.Rows)
            {
                AMainDS.PPartner.DefaultView.Sort = PPartnerTable.GetPartnerKeyDBName();
                Int32 RowIdx = AMainDS.PPartner.DefaultView.Find(Row.ParentUnitKey);

                if (RowIdx < 0)
                {
                    if (!PPartnerAccess.Exists(Row.ParentUnitKey, ATransaction))
                    {
                        AddVerificationResult(ref ReferenceResults,
                            "Required Parent UNIT not Found: " + Row.ParentUnitKey.ToString(), TResultSeverity.Resv_Critical);
                        EveryoneHasAParent = false;
                        break;
                    }
                }

                if (UmUnitStructureAccess.Exists(Row.ParentUnitKey, Row.ChildUnitKey, ATransaction))
                {
                    Row.AcceptChanges();
                }
            }

            return EveryoneHasAParent;
        }

        /// <summary>
        /// If this person already has details of this passport on file,
        /// I'll overwrite those details, rather than creating a new row.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        private static void CheckPassport(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            if (MainDS.PmPassportDetails.Rows.Count < 1)
            {
                return;
            }

            PmPassportDetailsTable DbPassportTbl = PmPassportDetailsAccess.LoadViaPPerson(MainDS.PPartner[0].PartnerKey, Transaction);

            if (DbPassportTbl.Rows.Count < 1)
            {
                return;
            }

            for (int ImportPassprtRowIdx = 0; ImportPassprtRowIdx < MainDS.PmPassportDetails.Rows.Count; ImportPassprtRowIdx++)
            {
                PmPassportDetailsRow ImportPassport = (PmPassportDetailsRow)MainDS.PmPassportDetails[ImportPassprtRowIdx];

                foreach (PmPassportDetailsRow DbPassport in DbPassportTbl.Rows)
                {
                    if (DbPassport.PassportNumber == ImportPassport.PassportNumber) // This simple match ought to be unique
                    {   // These rows are the same passport. I want my imported data to overwrite the data in the database.
                        ImportPassport.AcceptChanges(); // This should cause the passport to be updated rather then added.
                        ImportPassport.DateCreated = DbPassport.DateCreated;
                        ImportPassport.CreatedBy = DbPassport.CreatedBy;
                        ImportPassport.ModificationId = DbPassport.ModificationId; // The trick only works if I have this magic password.

                        // No break - if this passport appears more than once in the DB (because of a fault), I'll overwrite all occurences.
                    }
                }
            }
        }

        private static void CheckPassportType(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // PassportDetails: If there's a passport, I need to have its type listed.
            foreach (PmPassportDetailsRow rv in MainDS.PmPassportDetails.Rows)
            {
                if ((rv.PassportDetailsType != "") && (!PtPassportTypeAccess.Exists(rv.PassportDetailsType, Transaction)))
                {
                    MainDS.PtPassportType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtPassportTypeTable.GetCodeDBName(), rv.PassportDetailsType);

                    if (MainDS.PtPassportType.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Passport Type " + rv.PassportDetailsType);
                        PtPassportTypeRow Row = MainDS.PtPassportType.NewRowTyped();
                        Row.Code = rv.PassportDetailsType;
                        Row.Description = FNewRowDescription;
                        MainDS.PtPassportType.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckDocumentRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            //
            // PMDocumentType and PmDocumentCategory
            // If the Code isn't currently in PmDocumentType, I can add it now
            //   ..and if even the Category isn't known, I can add that too in PmDocumentCategory.
            //
            foreach (PartnerImportExportTDSPmDocumentRow rv in MainDS.PmDocument.Rows)
            {
                if ((rv.DocCategory != "") && (!PmDocumentCategoryAccess.Exists(rv.DocCategory, Transaction)))
                {
                    MainDS.PmDocumentCategory.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PmDocumentCategoryTable.GetCodeDBName(), rv.DocCategory);

                    if (MainDS.PmDocumentCategory.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Document Category " + rv.DocCategory);
                        PmDocumentCategoryRow Row = MainDS.PmDocumentCategory.NewRowTyped();
                        Row.Code = rv.DocCategory;
                        Row.Description = FNewRowDescription;
                        MainDS.PmDocumentCategory.Rows.Add(Row);
                    }
                }

                if ((rv.DocCode != "") && (!PmDocumentTypeAccess.Exists(rv.DocCode, Transaction)))
                {
                    MainDS.PmDocumentType.DefaultView.RowFilter = String.Format("{0}='{1}'", PmDocumentTypeTable.GetDocCodeDBName(), rv.DocCode);

                    if (MainDS.PmDocumentType.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding Document Code " + rv.DocCode);
                        PmDocumentTypeRow Row = MainDS.PmDocumentType.NewRowTyped();
                        Row.DocCode = rv.DocCode;
                        Row.DocCategory = rv.DocCategory;
                        Row.Description = FNewRowDescription;
                        MainDS.PmDocumentType.Rows.Add(Row);
                    }
                }
            }
        }

        private static void CheckSubscriptions(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            foreach (PSubscriptionRow SubsRow in MainDS.PSubscription.Rows)
            {
                PSubscriptionTable Tbl = PSubscriptionAccess.LoadByPrimaryKey(SubsRow.PublicationCode, SubsRow.PartnerKey, Transaction); // If the record is present, I need to update rather than add.

                if (Tbl.Rows.Count > 0)
                {
                    PSubscriptionRow DbRow = Tbl[0];
                    SubsRow.AcceptChanges();                        // This removes the "Added" attribute.
                    SubsRow.PartnerKey = SubsRow.PartnerKey;        // this looks pointless, but it makes the row "Modified".

                    SubsRow.ModificationId = DbRow.ModificationId;  // I'll copy this to keep the ORM happy,
                    SubsRow.CreatedBy = DbRow.CreatedBy;            // and these two in case anyone might refer to them.
                    SubsRow.DateCreated = DbRow.DateCreated;
                }

                if (SubsRow.ReasonSubsGivenCode == "")
                {
                    SubsRow.ReasonSubsGivenCode = "FREE";
                }

                if (!PReasonSubscriptionGivenAccess.Exists(SubsRow.ReasonSubsGivenCode, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Subscription reason: " + SubsRow.ReasonSubsGivenCode);
                    PReasonSubscriptionGivenRow NewRow = MainDS.PReasonSubscriptionGiven.NewRowTyped();
                    NewRow.Code = SubsRow.ReasonSubsGivenCode;
                    NewRow.Description = FNewRowDescription;
                    MainDS.PReasonSubscriptionGiven.Rows.Add(NewRow);
                }

                if ((SubsRow.ReasonSubsCancelledCode != "")
                    && (!PReasonSubscriptionCancelledAccess.Exists(SubsRow.ReasonSubsCancelledCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Subscription reason: " + SubsRow.ReasonSubsCancelledCode);
                    PReasonSubscriptionCancelledRow NewRow = MainDS.PReasonSubscriptionCancelled.NewRowTyped();
                    NewRow.Code = SubsRow.ReasonSubsCancelledCode;
                    NewRow.Description = FNewRowDescription;
                    MainDS.PReasonSubscriptionCancelled.Rows.Add(NewRow);
                }

                if ((SubsRow.PublicationCode.Length > 0) && !PPublicationAccess.Exists(SubsRow.PublicationCode, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Publication Code: " + SubsRow.PublicationCode);
                    PPublicationRow NewRow = MainDS.PPublication.NewRowTyped();
                    NewRow.PublicationCode = SubsRow.PublicationCode;
                    NewRow.PublicationDescription = FNewRowDescription;
                    NewRow.FrequencyCode = "Daily"; // I can't leave this blank, so I need to make something up...
                    MainDS.PPublication.Rows.Add(NewRow);
                }
            }
        }

        private static void CheckJobRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Position: We can't have jobs for positions we've not heard of
            foreach (UmJobRow rv in MainDS.UmJob.Rows)
            {
                if ((rv.PositionName != "") && (!PtPositionAccess.Exists(rv.PositionName, rv.PositionScope, Transaction)))
                {
                    MainDS.PtPosition.DefaultView.RowFilter = String.Format("{0}='{1}'", PtPositionTable.GetPositionNameDBName(), rv.PositionName);

                    if (MainDS.PtPosition.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Position " + rv.PositionName);
                        PtPositionRow Row = MainDS.PtPosition.NewRowTyped();
                        Row.PositionName = rv.PositionName;
                        Row.PositionScope = rv.PositionScope;
                        Row.PositionDescr = FNewRowDescription;
                        MainDS.PtPosition.Rows.Add(Row);
                    }
                }
            }
        }

        /// <summary>
        /// If the PPartner record on the server has changed since the start of the import process,
        /// I need to abort, with a note to the user.
        ///
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        /// <returns>false if this data can't be imported.</returns>
        private static bool CheckModificationId(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            Int64 PartnerKey = MainDS.PPartner[0].PartnerKey;
            DateTime MyModifId = MainDS.PPartner[0].ModificationId; // This ModificationId was read at start of import.

            PPartnerTable Table = PPartnerAccess.LoadByPrimaryKey(PartnerKey, Transaction);

            if (Table.Rows.Count == 0)
            {
                return true; // I don't have this Partner on the database, but that's OK..
            }

            DateTime OrignModified = Table[0].ModificationId;

            if (MyModifId != OrignModified)
            {
                AddVerificationResult(ref ReferenceResults,
                    "PPartner record in database has been updated during import process.",
                    TResultSeverity.Resv_Critical);
                return false;
            }

            return true;
        }

        /// <summary>
        /// I need to check all the various tables that are linked to the main tables,
        /// so that all the references will be OK. If any are missing I need to make something up,
        /// or in some cases substitute a known value in the reference.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        /// <returns>false if this data can't be imported.</returns>
        private static bool CheckReferencedTables(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            PPartnerRow PartnerRow = (PPartnerRow)MainDS.PPartner.Rows[0];

            FImportContext = String.Format("While importing partner [{0}]", PartnerRow.PartnerKey);

            if (!CheckPartnerClass(PartnerRow, MainDS, ref ReferenceResults, Transaction))
            {
                return false;
            }

            if (!CheckUnitParent(MainDS, ref ReferenceResults, Transaction))
            {
                return false;
            }

            CheckLocationType(MainDS, ref ReferenceResults, Transaction);
            CheckBusiness(MainDS, ref ReferenceResults, Transaction);
            CheckLanguage(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckAcquisitionCode(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckAddresses(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckAddresseeTypeCode(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckAbilityArea(MainDS, ref ReferenceResults, Transaction);
            CheckPartnerInterest(MainDS, ref ReferenceResults, Transaction);
            CheckPartnerType(MainDS, ref ReferenceResults, Transaction);
            CheckChurchDenomination(MainDS, ref ReferenceResults, Transaction);
            CheckContactRefs(MainDS, ref ReferenceResults, Transaction);
            CheckApplication(MainDS, ref ReferenceResults, Transaction);
            CheckCommitmentStatus(MainDS, ref ReferenceResults, Transaction);
            CheckSTApplicationRefs(MainDS, ref ReferenceResults, Transaction);
//          CheckVisionRefs(MainDS, ref ReferenceResults, Transaction);
            CheckYearProgramRefs(MainDS, ref ReferenceResults, Transaction);
            CheckQualificationRefs(MainDS, ref ReferenceResults, Transaction);
            CheckMaritalStatus(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckOccupation(MainDS, ref ReferenceResults, Transaction);
            CheckUnitType(MainDS, ref ReferenceResults, Transaction);
            CheckPassport(MainDS, ref ReferenceResults, Transaction);
            CheckPassportType(MainDS, ref ReferenceResults, Transaction);
            CheckDocumentRefs(MainDS, ref ReferenceResults, Transaction);
            CheckSubscriptions(MainDS, ref ReferenceResults, Transaction);
            CheckJobRefs(MainDS, ref ReferenceResults, Transaction);
            return true;
        }

        /// <summary>
        /// Web connector for commit changes after importing a partner
        /// Before calling SubmitChanges on the dataset, this does a load of checks
        /// and supllies values to index tables.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if no error</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CommitChanges(PartnerImportExportTDS MainDS, out TVerificationResultCollection AVerificationResult)
        {
            TVerificationResultCollection ReferenceResults = new TVerificationResultCollection();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            bool CanImport = CheckModificationId(MainDS, ref ReferenceResults, Transaction);

            if (CanImport)
            {
                CanImport = CheckReferencedTables(MainDS, ref ReferenceResults, Transaction);
            }

            DBAccess.GDBAccessObj.CommitTransaction();

            TSubmitChangesResult Res = TSubmitChangesResult.scrError;

            if (CanImport)
            {
                PartnerImportExportTDSAccess.SubmitChanges(MainDS);
                
                Res = TSubmitChangesResult.scrOK;
            }

            if (((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerClass == "")
            {
                AddVerificationResult(ref ReferenceResults, "Partner has no CLASS!", TResultSeverity.Resv_Critical);
                Res = TSubmitChangesResult.scrInfoNeeded;
            }
            else
            {
                AddVerificationResult(ref ReferenceResults, String.Format("Import of {0} {1}\r\n {2}",
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerClass,
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerShortName,
                        Res == TSubmitChangesResult.scrOK ? "Successful" : "Error"
                       ),
                    TResultSeverity.Resv_Status);
            }

            AVerificationResult = ReferenceResults;

            return TSubmitChangesResult.scrOK == Res;
        }

        /// <summary>
        /// Return an XmlDocument with all partner info;
        /// the partners are grouped by class, country, status, and sitekey
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static string ExportPartners()
        {
            return TImportExportYml.ExportPartners();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A string that will form the first two lines of a .ext file</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetExtFileHeader()
        {
            TPartnerFileExport Exporter = new TPartnerFileExport();

            return Exporter.ExtFileHeader();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A string that will form the final line of a .ext file</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetExtFileFooter()
        {
            return "0  \"FINISH\"\n";
        }

        /// <summary>
        /// Format a partner as ext (Petra 2.x format)
        /// If I've been asked to export a PERSON, I can also export the FAMILY record first.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="ASiteKey">Partner's site key</param>
        /// <param name="ALocationKey">Partner's primary location key</param>
        /// <param name="ANoFamily">Set this flag for a PERSON, to prevent the FAMILY being exported too.</param>
        /// <param name="ASpecificBuildingInfo">Only include these buildings (null for all)</param>
        /// <returns>One partner in EXT format</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string ExportPartnerExt(Int64 APartnerKey,
            Int64 ASiteKey,
            Int32 ALocationKey,
            Boolean ANoFamily,
            StringCollection ASpecificBuildingInfo)
        {
            String extRecord = "";
            //
            // First I'm going to check that I can access this partner OK..
            Boolean PartnerAccessOk = false;
            String ShortName;
            TPartnerClass PartnerClass;
            Boolean IsMergedPartner = false;
            Boolean UserCanAccessPartner = false;

            if (APartnerKey != 0)
            {
                PartnerAccessOk = TPartnerServerLookups.VerifyPartner(APartnerKey,
                    out ShortName, out PartnerClass,
                    out IsMergedPartner, out UserCanAccessPartner);
            }

            if (!PartnerAccessOk || !UserCanAccessPartner)
            {
                return extRecord;  // This is empty - TODO: I'm not returning any error code here.
            }

            TPartnerFileExport Exporter = new TPartnerFileExport();
            PartnerImportExportTDS AMainDS = TExportAllPartnerData.ExportPartner(APartnerKey);

            if (!ANoFamily)  // I'll check whether there's a FAMILY to go with this Partner.
            {
                PPartnerRow PartnerRow = AMainDS.PPartner[0];

                if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                {
                    PPersonRow PersonRow = AMainDS.PPerson[0];
                    long FamilyKey = PersonRow.FamilyKey;
                    PartnerAccessOk = TPartnerServerLookups.VerifyPartner(FamilyKey,
                        out ShortName, out PartnerClass,
                        out IsMergedPartner, out UserCanAccessPartner);

                    if ((FamilyKey > 0) && PartnerAccessOk && UserCanAccessPartner)
                    {
                        PartnerImportExportTDS FamilyDS = TExportAllPartnerData.ExportPartner(FamilyKey);
                        extRecord += Exporter.ExportPartnerExt(FamilyDS, ASiteKey, ALocationKey, ASpecificBuildingInfo);
                    }

                    // TODO: If I couldn't access the FAMILY for a PERSON, I should perhaps stop exporting?
                }
            }

            extRecord += Exporter.ExportPartnerExt(AMainDS, ASiteKey, ALocationKey, ASpecificBuildingInfo);
            return extRecord;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Export all partners of an extract into an EXT file.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static String ExportExtractPartnersExt(int AExtractId, Boolean AIncludeFamilyMembers)
        {
            String ExtText = GetExtFileHeader();
            MExtractTable ExtractPartners = MExtractAccess.LoadViaMExtractMaster(AExtractId, null);
            TPartnerFileExport Exporter = new TPartnerFileExport();
            PartnerImportExportTDS MainDS;

            foreach (MExtractRow ExtractPartner in ExtractPartners.Rows)
            {
                if (ExtractPartner.PartnerKey != 0)
                {
                    MainDS = TExportAllPartnerData.ExportPartner(ExtractPartner.PartnerKey);
                    ExtText += Exporter.ExportPartnerExt(MainDS, /*ASiteKey*/ 0, /*ALocationKey*/ 0, null);
                }
            }

            ExtText += GetExtFileFooter();
            return ExtText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>All the text to write into an EXT file.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static String ExportAllPartnersExt()
        {
            String ExtText = GetExtFileHeader();
            PPartnerTable Partners = PPartnerAccess.LoadAll(null);
            TPartnerFileExport Exporter = new TPartnerFileExport();
            PartnerImportExportTDS MainDS;

            foreach (PPartnerRow Partner in Partners.Rows)
            {
                if ((Partner.PartnerKey != 0) && (Partner.PartnerKey != 1000000)) // skip organization root and 0 when exporting
                {
                    MainDS = TExportAllPartnerData.ExportPartner(Partner.PartnerKey);
                    ExtText += Exporter.ExportPartnerExt(MainDS, /*ASiteKey*/ 0, /*ALocationKey*/ 0, null);
                }
            }

            ExtText += GetExtFileFooter();
            return ExtText;
        }

        /// <summary>
        /// Unpack this (ext formatted) string into the database
        /// </summary>
        /// <param name="ALinesToImport"></param>
        /// <param name="ALimitToOption"></param>
        /// <param name="ADoNotOverwrite"></param>
        /// <param name="AResultList"></param>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ImportDataExt(string[] ALinesToImport,
            string ALimitToOption,
            bool ADoNotOverwrite,
            out TVerificationResultCollection AResultList)
        {
            TPartnerFileImport Importer = new TPartnerFileImport();

            Importer.ImportAllData(ALinesToImport,
                ALimitToOption, ADoNotOverwrite, out AResultList);
            return true;
        }
    }
}