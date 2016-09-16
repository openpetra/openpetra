//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//       ChristianK
//
// Copyright 2004-2014 by OM International
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
using Ict.Petra.Shared.MPartner.Conversion;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.App.Core;
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
        private static readonly string StrPrimaryContactDetailChangedToImported =
            Catalog.GetString("'{0}' changed to the one contained in the import file: '{1}'");

        private static readonly string StrPrimaryContactDetailAttrTypeChanged =
            Catalog.GetString("Prior to the importing of this Partner, the '{0}' record " +
                "of this Partner had the same {1}, but it was recorded with a different " +
                "Contact Type, '{2}'. That existing record was not removed; please maintain the Contact Details " +
                "of this Partner as required!");

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
        /// read data from db for partner with given key according to csv columns needed
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerImportExportTDS ReadPartnerDataForCSV(Int64 APartnerKey, List <String>ACSVColumns)
        {
            TDBTransaction ReadTransaction = null;

            PartnerImportExportTDS MainDS = new PartnerImportExportTDS();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.RepeatableRead, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    // read PPartner record
                    MainDS.Merge(PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));

                    // read Partner specific record
                    if ((MainDS.PPartner != null)
                        && (MainDS.PPartner.Count > 0))
                    {
                        switch (MainDS.PPartner[0].PartnerClass)
                        {
                            case MPartnerConstants.PARTNERCLASS_FAMILY:
                                MainDS.Merge(PFamilyAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_PERSON:
                                MainDS.Merge(PPersonAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_UNIT:
                                MainDS.Merge(PUnitAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_ORGANISATION:
                                MainDS.Merge(POrganisationAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_CHURCH:
                                MainDS.Merge(PChurchAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_BANK:
                                MainDS.Merge(PBankAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            case MPartnerConstants.PARTNERCLASS_VENUE:
                                MainDS.Merge(PVenueAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction));
                                break;

                            default:
                                // this should not happen
                                break;
                        }
                    }

                    // read addresses
                    MainDS.Merge(PLocationAccess.LoadViaPPartner(APartnerKey, ReadTransaction));
                    MainDS.Merge(PPartnerLocationAccess.LoadViaPPartner(APartnerKey, ReadTransaction));

                    // read contact details
                    if (ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_EMAIL)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PHONE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_MOBILEPHONE))
                    {
                        MainDS.Merge(PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, ReadTransaction));
                    }

                    // for FAMILY ("SpecialTypeFamily_x") and other Partner Classes ("SpecialType_x"):
                    // read special types in any case (checking for criteria would take too much runtime)
                    MainDS.Merge(PPartnerTypeAccess.LoadViaPPartner(APartnerKey, ReadTransaction));

                    // read special needs (for PERSON)
                    if (ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_VEGETARIAN)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_MEDICALNEEDS))
                    {
                        MainDS.Merge(PmSpecialNeedAccess.LoadViaPPerson(APartnerKey, ReadTransaction));
                    }

                    // read application (for PERSON)
                    if (ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_EVENTKEY)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ARRIVALDATE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_ARRIVALTIME)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DEPARTUREDATE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_DEPARTURETIME)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_EVENTROLE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPDATE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPSTATUS)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPTYPE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CHARGEDFIELD)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_APPCOMMENTS))
                    {
                        MainDS.Merge(PmGeneralApplicationAccess.LoadViaPPersonPartnerKey(APartnerKey, ReadTransaction));
                        MainDS.Merge(PmShortTermApplicationAccess.LoadViaPPerson(APartnerKey, ReadTransaction));
                    }

                    // read contact logs
                    if (ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTCODE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTDATE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTTIME)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTOR)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTNOTES)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTATTR)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_CONTACTDETAIL))
                    {
                        MainDS.Merge(PContactLogAccess.LoadViaPPartnerPPartnerContact(APartnerKey, ReadTransaction));
                    }

                    // read passports (for PERSON)
                    if (ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNUMBER)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNAME)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTTYPE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFBIRTH)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTNATIONALITY)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTPLACEOFISSUE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFISSUE)
                        || ACSVColumns.Contains(MPartnerConstants.PARTNERIMPORT_PASSPORTDATEOFEXPIRATION))
                    {
                        MainDS.Merge(PmPassportDetailsAccess.LoadViaPPerson(APartnerKey, ReadTransaction));
                    }
                });

            MainDS.AcceptChanges();

            return MainDS;
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePartnerTablesEnum.LocationTypeList.ToString());
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheablePartnerTablesEnum.BusinessCodeList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheableCommonTablesEnum.LanguageCodeList.ToString());
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
                TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                    TCacheablePartnerTablesEnum.AcquisitionCodeList.ToString());
            }
        }

        /// <summary>
        /// If the address I'm importing is already on file, I don't want to create another row.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="AImportFileFormat"></param>
        /// <param name="PartnerRow"></param>
        /// <param name="AReplaceAddress"></param>
        /// <param name="ASiteKeyToBeReplaced"></param>
        /// <param name="ALocationKeyToBeReplaced"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        private static void CheckAddresses(PartnerImportExportTDS MainDS,
            TImportFileFormat AImportFileFormat,
            PPartnerRow PartnerRow,
            Boolean AReplaceAddress,
            Int64 ASiteKeyToBeReplaced,
            Int32 ALocationKeyToBeReplaced,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            Boolean AddressIsReplaced = false;

            if (AReplaceAddress)
            {
                // this case is for csv files only and we should only have one import address

                PLocationTable LocationTable = PLocationAccess.LoadByPrimaryKey(ASiteKeyToBeReplaced, ALocationKeyToBeReplaced, Transaction);

                if ((LocationTable.Rows.Count > 0)
                    && (MainDS.PPartnerLocation.Rows.Count == 1)
                    && (MainDS.PLocation.Rows.Count == 1))
                {
                    String ExistingAddress = Calculations.DetermineLocationString(LocationTable[0],
                        Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                    String NewAddress = Calculations.DetermineLocationString(MainDS.PLocation[0],
                        Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);

                    // now we need to find out if this address is also used by other partners
                    // (if used by other partners we need to create a new one, if not then we can just modify this one)
                    PPartnerLocationTable PartnerLocationTable = PPartnerLocationAccess.LoadViaPLocation(LocationTable[0].SiteKey,
                        LocationTable[0].LocationKey,
                        Transaction);

                    if (PartnerLocationTable.Count == 1)
                    {
                        // save imported values to temporary location row
                        PLocationRow ImportedLocationRow = MainDS.PLocation[0];
                        PLocationRow ImportedLocationRowCopy = MainDS.PLocation.NewRowTyped(false);
                        ImportedLocationRowCopy.ItemArray = (object[])ImportedLocationRow.ItemArray.Clone();

                        // now initialize imported row with values from db and reset RowState (with AcceptChanges) so that is it not Added
                        ImportedLocationRow.ItemArray = (object[])LocationTable[0].ItemArray.Clone();
                        ImportedLocationRow.AcceptChanges();

                        // copy imported values into row (does not affect RowState), but keep location key from db
                        ImportedLocationRow.ItemArray = (object[])ImportedLocationRowCopy.ItemArray.Clone();
                        ImportedLocationRow.SiteKey = ASiteKeyToBeReplaced;
                        ImportedLocationRow.LocationKey = ALocationKeyToBeReplaced;

                        // remove partner location as the one in db already points to the right location
                        MainDS.PPartnerLocation.Rows.RemoveAt(0);

                        AddressIsReplaced = true;
                        AddVerificationResult(ref ReferenceResults,
                            "Existing address (" + ExistingAddress + ") replaced with: " + NewAddress,
                            TResultSeverity.Resv_Status);
                    }
                    else if (PartnerLocationTable.Count > 1)
                    {
                        // remove partner location for this partner and add the address as a new one (further down in the code of this method)
                        PPartnerLocationAccess.DeleteByPrimaryKey(PartnerRow.PartnerKey,
                            LocationTable[0].SiteKey,
                            LocationTable[0].LocationKey,
                            Transaction);
                    }
                }
            }

            // if address should not be replaced or if replacing did not work continue here anyways so we can definitely import some address data
            if (!AddressIsReplaced)
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
                                && (DbLocationRow.Address3 == ImportLocationRow.Address3)
                                && (DbLocationRow.County == ImportLocationRow.County)
                                && (DbLocationRow.City == ImportLocationRow.City)
                                && (DbLocationRow.CountryCode == ImportLocationRow.CountryCode)
                                && (DbLocationRow.PostalCode == ImportLocationRow.PostalCode)
                                )
                            {
                                String ExistingAddress = Calculations.DetermineLocationString(DbLocationRow,
                                    Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);

                                MainDS.PLocation.Rows.Remove(ImportLocationRow);

                                //TODOWBxxx
                                // check if partner with this location key already exists in database
                                if (PPartnerLocationAccess.Exists(PartnerRow.PartnerKey, 0, DbLocationRow.LocationKey, Transaction))
                                {
                                    // in case of .ext files all data is imported, so the PartnerLocation record may contain important
                                    // information like start and end date of an address, whereas with .csv this information is not imported
                                    if (AImportFileFormat != TImportFileFormat.ext)
                                    {
                                        MainDS.PPartnerLocation.Rows.RemoveAt(ImportPartnerLocationRowIdx);
                                        RowRemoved = true;
                                    }
                                }
                                else
                                {
                                    ((PPartnerLocationRow)MainDS.PPartnerLocation.Rows[ImportPartnerLocationRowIdx]).LocationKey =
                                        DbLocationRow.LocationKey;
                                }

                                AddVerificationResult(ref ReferenceResults, "Existing address used: " + ExistingAddress, TResultSeverity.Resv_Status);
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
                TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheablePartnerTablesEnum.AddresseeTypeList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.AbilityAreaList.ToString());
                    }
                }

                if (!PtAbilityLevelAccess.Exists(rv.AbilityLevel, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Removing unknown Ability level " + rv.AbilityLevel);
                    rv.AbilityLevel = 99; // If I don't know what this AbilityLevel means, I can only say, "unknown".
                }
            }
        }

        private static void CheckSkillCategoryAndLevel(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction Transaction)
        {
            // Skill Category: if there's any skill, they must only be in known categories!
            // Skill Level: only import known level identifiers
            foreach (PmPersonSkillRow rv in MainDS.PmPersonSkill.Rows)
            {
                if ((rv.SkillCategoryCode != "") && (!PtSkillCategoryAccess.Exists(rv.SkillCategoryCode, Transaction)))
                {
                    MainDS.PtSkillCategory.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtSkillCategoryTable.GetCodeDBName(), rv.SkillCategoryCode);

                    if (MainDS.PtSkillCategory.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Skill Category " + rv.SkillCategoryCode);
                        PtSkillCategoryRow Row = MainDS.PtSkillCategory.NewRowTyped();
                        Row.Code = rv.SkillCategoryCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtSkillCategory.Rows.Add(Row);
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.SkillCategoryList.ToString());
                    }
                }

                if (!PtSkillLevelAccess.Exists(rv.SkillLevel, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Removing unknown Skill level " + rv.SkillLevel);
                    rv.SkillLevel = 99; // If I don't know what this Skill Level means, I can only say, "unknown".
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheablePartnerTablesEnum.InterestList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePartnerTablesEnum.InterestCategoryList.ToString());
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheablePartnerTablesEnum.PartnerTypeList.ToString());
                }
            }
        }

        private static void CheckPartnerAttribute(PartnerImportExportTDS MainDS, Int64 APartnerKey,
            ref TVerificationResultCollection ReferenceResults, TDBTransaction Transaction)
        {
            PPartnerAttributeTable PartnersPartnerAttributesInDBDT;
            PPartnerAttributeTable PartnersPartnerAttributesInDBNeedingUpdatingDT;
            PPartnerAttributeRow FoundPartnerAttribDR;
            PPartnerAttributeRow ExistingEmailPartnerAttribDR;
            string ImportedPrimaryPhoneNumber;
            string ImportedPrimaryEmailAddress;
            DataView ExistingEmailAddressDV;
            bool ExistingPrimaryRecordHasAnEmailAttributeType;
            bool SameValueButAttributeTypeMismatch;

            Calculations.DeterminePartnerContactDetailAttributes(MainDS.PPartnerAttribute);

            // Prevent duplicate 'Primary' E-Mail and/or Phone in case the imported Partner Attributes contain at least one
            // 'Primary' Contact Detail and this/they is/are set on (a) different Contact Detail(s) than in the DB!
            if (Calculations.GetPrimaryEmailAndPrimaryPhone(MainDS.PPartnerAttribute, out ImportedPrimaryPhoneNumber,
                    out ImportedPrimaryEmailAddress))
            {
                // Load all Partner Attributes of this Partner that exist in the DB
                PartnersPartnerAttributesInDBDT = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, Transaction);
                Calculations.DeterminePartnerContactDetailAttributes(PartnersPartnerAttributesInDBDT);
                PartnersPartnerAttributesInDBDT.AcceptChanges();
                ExistingEmailAddressDV = Calculations.DeterminePartnerEmailAddresses(PartnersPartnerAttributesInDBDT, false);

                foreach (PPartnerAttributeRow ExistingPartnerAttribDV in PartnersPartnerAttributesInDBDT.Rows)
                {
                    SameValueButAttributeTypeMismatch = false;

                    // Check each Partner Contact Detail...
                    if ((ExistingPartnerAttribDV[Ict.Petra.Shared.MPartner.Calculations.PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN]
                         != System.DBNull.Value)
                        && ((bool)ExistingPartnerAttribDV[Ict.Petra.Shared.MPartner.Calculations.PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] ==
                            true))
                    {
                        // ... that is 'Primary' in the DB ...
                        if (ExistingPartnerAttribDV.Primary)
                        {
                            // ... whether it has the same Value than a to-be-imported 'Primary' Partner Contact Attribute
                            if ((ExistingPartnerAttribDV.Value == ImportedPrimaryPhoneNumber)
                                || (ExistingPartnerAttribDV.Value == ImportedPrimaryEmailAddress))
                            {
                                // ... and whether it is of the same AttributeType than a to-be-imported 'Primary' Partner Contact Attribute
                                foreach (PPartnerAttributeRow ImportedPartnerAttribDV in MainDS.PPartnerAttribute.Rows)
                                {
                                    if (ExistingPartnerAttribDV.Value == ImportedPartnerAttribDV.Value)
                                    {
                                        if (ExistingPartnerAttribDV.AttributeType == ImportedPartnerAttribDV.AttributeType)
                                        {
                                            // Yes: No duplicate 'Primary' record could potentially get created as it IS the same record
                                            continue;
                                        }
                                        else
                                        {
                                            SameValueButAttributeTypeMismatch = true;
                                        }
                                    }
                                }
                            }

                            // No: A duplicate 'Primary' record could potentially get created -> peform further checks:

                            // Determine if the existing 'Primary' record has got an Attribute Type that designates an E-Mail
                            // Address

                            if (ExistingEmailAddressDV.Count == 0)
                            {
                                if (ImportedPrimaryPhoneNumber != null)
                                {
                                    // Primary Contact Attribute in the DB must be for the 'Primary Phone Number' as there are no
                                    // existing 'Primary' records that have got an Attribute Type that designates an E-Mail Address

                                    if (ExistingPartnerAttribDV.Value != ImportedPrimaryPhoneNumber)
                                    {
                                        // Make the DataRow in the DB no longer 'Primary' as this would become another 'Primary'
                                        // Phone Number --- in addition to the to-be-imported 'Primary' Phone Number!
                                        ExistingPartnerAttribDV.Primary = false;

                                        AddVerificationResult(ref ReferenceResults,
                                            String.Format(StrPrimaryContactDetailChangedToImported,
                                                Catalog.GetString("Primary Phone"), ImportedPrimaryPhoneNumber),
                                            TResultSeverity.Resv_Status);

                                        if (SameValueButAttributeTypeMismatch)
                                        {
                                            AddVerificationResult(ref ReferenceResults,
                                                String.Format(StrPrimaryContactDetailAttrTypeChanged,
                                                    Catalog.GetString("Primary Phone"), Catalog.GetString("Phone Number"),
                                                    ExistingPartnerAttribDV.AttributeType),
                                                TResultSeverity.Resv_Status);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ExistingPrimaryRecordHasAnEmailAttributeType = false;

                                // Check all existing 'Primary' records that have got an Attribute Type that designates an E-Mail Address
                                foreach (DataRowView DataViewElement in ExistingEmailAddressDV)
                                {
                                    ExistingEmailPartnerAttribDR = (PPartnerAttributeRow)DataViewElement.Row;

                                    if (ExistingPartnerAttribDV.AttributeType == ExistingEmailPartnerAttribDR.AttributeType)
                                    {
                                        ExistingPrimaryRecordHasAnEmailAttributeType = true;

                                        if (ImportedPrimaryEmailAddress != null)
                                        {
                                            // Primary Contact Attribute in the DB is for the 'Primary E-Mail Address'
                                            if (ExistingPartnerAttribDV.Value != ImportedPrimaryEmailAddress)
                                            {
                                                // Make the DataRow in the DB no longer 'Primary' as this would become another 'Primary'
                                                // E-Mail Address --- in addition to the to-be-imported 'Primary' E-Mail Address!
                                                ExistingPartnerAttribDV.Primary = false;

                                                AddVerificationResult(ref ReferenceResults,
                                                    String.Format(StrPrimaryContactDetailChangedToImported,
                                                        Catalog.GetString("Primary E-Mail"), ImportedPrimaryEmailAddress),
                                                    TResultSeverity.Resv_Status);

                                                break;
                                            }
                                            else
                                            {
                                                if (SameValueButAttributeTypeMismatch)
                                                {
                                                    // Make the DataRow in the DB no longer 'Primary' as this would become another 'Primary'
                                                    // E-Mail Address --- in addition to the to-be-imported 'Primary' E-Mail Address!
                                                    ExistingPartnerAttribDV.Primary = false;

                                                    AddVerificationResult(ref ReferenceResults,
                                                        String.Format(StrPrimaryContactDetailChangedToImported,
                                                            Catalog.GetString("Primary E-Mail"), ImportedPrimaryEmailAddress),
                                                        TResultSeverity.Resv_Status);
                                                    AddVerificationResult(ref ReferenceResults,
                                                        String.Format(StrPrimaryContactDetailAttrTypeChanged,
                                                            Catalog.GetString("Primary E-Mail"), Catalog.GetString("E-Mail Address"),
                                                            ExistingPartnerAttribDV.AttributeType),
                                                        TResultSeverity.Resv_Status);

                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (!ExistingPrimaryRecordHasAnEmailAttributeType)
                                {
                                    if (ImportedPrimaryPhoneNumber != null)
                                    {
                                        // Primary Contact Attribute in the DB must be for the 'Primary Phone Number'
                                        if (ExistingPartnerAttribDV.Value != ImportedPrimaryPhoneNumber)
                                        {
                                            // Make the DataRow in the DB no longer 'Primary' as this would become another 'Primary'
                                            // Phone Number --- in addition to the to-be-imported 'Primary' Phone Number!
                                            ExistingPartnerAttribDV.Primary = false;

                                            AddVerificationResult(ref ReferenceResults,
                                                String.Format(StrPrimaryContactDetailChangedToImported,
                                                    Catalog.GetString("Primary Phone"), ImportedPrimaryPhoneNumber),
                                                TResultSeverity.Resv_Status);
                                        }
                                        else
                                        {
                                            if (SameValueButAttributeTypeMismatch)
                                            {
                                                // Make the DataRow in the DB no longer 'Primary' as this would become another 'Primary'
                                                // Phone Number --- in addition to the to-be-imported 'Primary' Phone Number!
                                                ExistingPartnerAttribDV.Primary = false;

                                                AddVerificationResult(ref ReferenceResults,
                                                    String.Format(StrPrimaryContactDetailChangedToImported,
                                                        Catalog.GetString("Primary Phone"), ImportedPrimaryPhoneNumber),
                                                    TResultSeverity.Resv_Status);
                                                AddVerificationResult(ref ReferenceResults,
                                                    String.Format(StrPrimaryContactDetailAttrTypeChanged,
                                                        Catalog.GetString("Primary Phone"), Catalog.GetString("Phone Number"),
                                                        ExistingPartnerAttribDV.AttributeType),
                                                    TResultSeverity.Resv_Status);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                PartnersPartnerAttributesInDBNeedingUpdatingDT = PartnersPartnerAttributesInDBDT.GetChangesTyped();

                if (PartnersPartnerAttributesInDBNeedingUpdatingDT != null)
                {
                    // Save any modified existing 'Primary' Contact Detail DataRows to prevent duplicate 'Primary' E-Mail and/or Phone
                    PPartnerAttributeAccess.SubmitChanges(PartnersPartnerAttributesInDBNeedingUpdatingDT, Transaction);
                }
            }

            TPartnerContactDetails_LocationConversionHelper.PartnerAttributeLoadUsingTemplate =
                PPartnerAttributeAccess.LoadUsingTemplate;

            for (int Counter = 0; Counter < MainDS.PPartnerAttribute.Rows.Count; Counter++)
            {
                // Check the to-be-imported p_partner_attribute records whether matching p_partner_attribute records exists
                // in the DB. This is to prevent duplication of p_partner_attribute records in the DB as a result
                // of the Import operation!
                if (TPartnerContactDetails_LocationConversionHelper.ExistingPartnerAttributes(
                        MainDS.PPartnerAttribute[Counter], out FoundPartnerAttribDR, Transaction))
                {
                    TPartnerContactDetails_LocationConversionHelper.TakeExistingPartnerAttributeRecordAndModifyIt(
                        MainDS.PPartnerAttribute[Counter], FoundPartnerAttribDR);
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheablePartnerTablesEnum.DenominationList.ToString());
                }
            }
        }

        private static void CheckContactRefs(PartnerImportExportTDS MainDS,
            ref TVerificationResultCollection ReferenceResults,
            TDBTransaction ATransaction)
        {
            //int ContactId = 0;

            //foreach (PartnerImportExportTDSPPartnerContactRow Row in MainDS.PPartnerContact.Rows)
            //{
            //    PPartnerContactTable Tbl = PPartnerContactAccess.LoadByUniqueKey(Row.PartnerKey, Row.ContactDate, Row.ContactTime, ATransaction);
            //    bool HereAlready = false;

            //    if (Tbl.Rows.Count > 0)         // I've already imported this..
            //    {
            //        Row.AcceptChanges();             // This should make the DB update instead of Add
            //        Row.ContactId = Tbl[0].ContactId;
            //        Row.ModificationId = Tbl[0].ModificationId;
            //        HereAlready = true;
            //    }

            //    if (Row.ContactId == 0)
            //    {
            //        Row.ContactId = --ContactId;
            //    }

            //    // The row has custom Attr and Detail fields, which I need to put into the right tables..
            //    if (!HereAlready && (Row.ContactAttr != ""))
            //    {
            //        AddVerificationResult(ref ReferenceResults, "Adding new contact attribute: " + Row.ContactAttr, TResultSeverity.Resv_Status);
            //        PContactAttributeDetailRow PcadRow = MainDS.PContactAttributeDetail.NewRowTyped();
            //        PcadRow.ContactAttributeCode = Row.ContactAttr;
            //        PcadRow.ContactAttrDetailCode = Row.ContactDetail;
            //        PcadRow.ContactAttrDetailDescr = FNewRowDescription;
            //        PContactAttributeDetailAccess.AddOrModifyRecord(
            //            PcadRow.ContactAttributeCode,
            //            PcadRow.ContactAttrDetailCode,
            //            MainDS.PContactAttributeDetail,
            //            PcadRow, false, ATransaction);

            //        PPartnerContactAttributeRow PcaRow = MainDS.PPartnerContactAttribute.NewRowTyped();
            //        PcaRow.ContactId = Row.ContactId;
            //        PcaRow.ContactAttributeCode = Row.ContactAttr;
            //        PcaRow.ContactAttrDetailCode = Row.ContactDetail;
            //        PPartnerContactAttributeAccess.AddOrModifyRecord(
            //            PcaRow.ContactId,
            //            PcaRow.ContactAttributeCode,
            //            PcaRow.ContactAttrDetailCode,
            //            MainDS.PPartnerContactAttribute,
            //            PcaRow, false, ATransaction);

            //        PContactAttributeRow CaRow = MainDS.PContactAttribute.NewRowTyped();
            //        CaRow.ContactAttributeDescr = FNewRowDescription;
            //        CaRow.ContactAttributeCode = Row.ContactAttr;
            //        PContactAttributeAccess.AddOrModifyRecord(
            //            CaRow.ContactAttributeCode,
            //            MainDS.PContactAttribute,
            //            CaRow,
            //            false,
            //            ATransaction);
            //    }

            //    if (!PMethodOfContactAccess.Exists(Row.ContactCode, ATransaction))
            //    {
            //        AddVerificationResult(ref ReferenceResults, "Adding new method of contact: " + Row.ContactCode, TResultSeverity.Resv_Status);
            //        PMethodOfContactRow MocRow = MainDS.PMethodOfContact.NewRowTyped();
            //        MocRow.MethodOfContactCode = Row.ContactCode;
            //        MocRow.Description = FNewRowDescription;
            //        MocRow.ValidMethod = true;
            //        MainDS.PMethodOfContact.Rows.Add(MocRow);
            //    }
            //}
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
                //TODOWBxxx: probably not needed any longer
                //if (PmGeneralApplicationAccess.Exists(GenAppRow.PartnerKey, GenAppRow.GenAppDate, GenAppRow.AppTypeName, GenAppRow.OldLink,
                //        Transaction))
                //{
                //    // If it does, I need to update and not add.
                //    PmGeneralApplicationTable Tbl = PmGeneralApplicationAccess.LoadByUniqueKey(
                //        GenAppRow.PartnerKey, GenAppRow.GenAppDate, GenAppRow.AppTypeName, GenAppRow.OldLink, Transaction);
                //    PmGeneralApplicationRow ExistingRow = Tbl[0];
                //
                //    GenAppRow.AcceptChanges();
                //    GenAppRow.ModificationId = ExistingRow.ModificationId;
                //
                //    AddVerificationResult(ref ReferenceResults, "Existing Application record updated.");
                //}

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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ApplicantStatusList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ApplicationTypeList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ContactList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ContactList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ArrivalDeparturePointList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.ArrivalDeparturePointList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.TransportTypeList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.TransportTypeList.ToString());
                    }
                }

                if ((rv.StCongressCode != "") && (!PtCongressCodeAccess.Exists(rv.StCongressCode, Transaction)))
                {
                    MainDS.PtCongressCode.DefaultView.RowFilter = String.Format("{0}='{1}'", PtCongressCodeTable.GetCodeDBName(), (rv.StCongressCode));

                    if (MainDS.PtCongressCode.DefaultView.Count == 0) // Check I've not just added this a moment ago..
                    {
                        AddVerificationResult(ref ReferenceResults, "Adding new Congress Code " + rv.StCongressCode);
                        PtCongressCodeRow Row = MainDS.PtCongressCode.NewRowTyped();
                        Row.Code = rv.StCongressCode;
                        Row.Description = FNewRowDescription;
                        MainDS.PtCongressCode.Rows.Add(Row);
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.EventRoleList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheableUnitTablesEnum.PositionList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.QualificationAreaList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.QualificationLevelList.ToString());
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
                TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheablePartnerTablesEnum.MaritalStatusList.ToString());
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheablePartnerTablesEnum.OccupationList.ToString());
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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheablePartnerTablesEnum.UnitTypeList.ToString());
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
                        //TODOWBxxx ImportPassport.AcceptChanges(); // This should cause the passport to be updated rather then added.
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.PassportTypeList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.DocumentTypeCategoryList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                            TCacheablePersonTablesEnum.DocumentTypeList.ToString());
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
                //TODOWBxxx: probably not needed any longer
                //PSubscriptionTable Tbl = PSubscriptionAccess.LoadByPrimaryKey(SubsRow.PublicationCode, SubsRow.PartnerKey, Transaction); // If the record is present, I need to update rather than add.
                //
                //if (Tbl.Rows.Count > 0)
                //{
                //    PSubscriptionRow DbRow = Tbl[0];
                //    SubsRow.AcceptChanges();                        // This removes the "Added" attribute.
                //    SubsRow.PartnerKey = SubsRow.PartnerKey;        // this looks pointless, but it makes the row "Modified".
                //
                //    SubsRow.ModificationId = DbRow.ModificationId;  // I'll copy this to keep the ORM happy,
                //    SubsRow.CreatedBy = DbRow.CreatedBy;            // and these two in case anyone might refer to them.
                //    SubsRow.DateCreated = DbRow.DateCreated;
                //}

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
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableSubscriptionsTablesEnum.ReasonSubscriptionGivenList.ToString());
                }

                if ((SubsRow.ReasonSubsCancelledCode != "")
                    && (!PReasonSubscriptionCancelledAccess.Exists(SubsRow.ReasonSubsCancelledCode, Transaction)))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Subscription reason: " + SubsRow.ReasonSubsCancelledCode);
                    PReasonSubscriptionCancelledRow NewRow = MainDS.PReasonSubscriptionCancelled.NewRowTyped();
                    NewRow.Code = SubsRow.ReasonSubsCancelledCode;
                    NewRow.Description = FNewRowDescription;
                    MainDS.PReasonSubscriptionCancelled.Rows.Add(NewRow);
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableSubscriptionsTablesEnum.ReasonSubscriptionCancelledList.ToString());
                }

                if ((SubsRow.PublicationCode.Length > 0) && !PPublicationAccess.Exists(SubsRow.PublicationCode, Transaction))
                {
                    AddVerificationResult(ref ReferenceResults, "Adding Publication Code: " + SubsRow.PublicationCode);
                    PPublicationRow NewRow = MainDS.PPublication.NewRowTyped();
                    NewRow.PublicationCode = SubsRow.PublicationCode;
                    NewRow.PublicationDescription = FNewRowDescription;
                    NewRow.FrequencyCode = "Daily"; // I can't leave this blank, so I need to make something up...
                    MainDS.PPublication.Rows.Add(NewRow);
                    TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(
                        TCacheableSubscriptionsTablesEnum.PublicationList.ToString());
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
                        TCacheableTablesManager.GCacheableTablesManager.MarkCachedTableNeedsRefreshing(TCacheableUnitTablesEnum.PositionList.ToString());
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
        /// <param name="AImportFileFormat"></param>
        /// <param name="AReplaceAddress"></param>
        /// <param name="ASiteKeyToBeReplaced"></param>
        /// <param name="ALocationKeyToBeReplaced"></param>
        /// <param name="ReferenceResults"></param>
        /// <param name="Transaction"></param>
        /// <returns>false if this data can't be imported.</returns>
        private static bool CheckReferencedTables(PartnerImportExportTDS MainDS,
            TImportFileFormat AImportFileFormat,
            Boolean AReplaceAddress,
            Int64 ASiteKeyToBeReplaced,
            Int32 ALocationKeyToBeReplaced,
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
            CheckAddresses(MainDS,
                AImportFileFormat,
                PartnerRow,
                AReplaceAddress,
                ASiteKeyToBeReplaced,
                ALocationKeyToBeReplaced,
                ref ReferenceResults,
                Transaction);
            CheckAddresseeTypeCode(MainDS, PartnerRow, ref ReferenceResults, Transaction);
            CheckAbilityArea(MainDS, ref ReferenceResults, Transaction);
            CheckPartnerInterest(MainDS, ref ReferenceResults, Transaction);
            CheckPartnerType(MainDS, ref ReferenceResults, Transaction);
            CheckPartnerAttribute(MainDS, PartnerRow.PartnerKey, ref ReferenceResults, Transaction);
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
            CheckSkillCategoryAndLevel(MainDS, ref ReferenceResults, Transaction);
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
        /// <param name="AImportFileFormat"></param>
        /// <param name="AReplaceAddress"></param>
        /// <param name="ASiteKeyToBeReplaced"></param>
        /// <param name="ALocationKeyToBeReplaced"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if no error</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CommitChanges(PartnerImportExportTDS MainDS,
            TImportFileFormat AImportFileFormat,
            Boolean AReplaceAddress,
            Int64 ASiteKeyToBeReplaced,
            Int32 ALocationKeyToBeReplaced,
            out TVerificationResultCollection AVerificationResult)
        {
            TVerificationResultCollection ReferenceResults = new TVerificationResultCollection();

            bool CanImport = false;
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    CanImport = CheckModificationId(MainDS, ref ReferenceResults, Transaction);

                    if (CanImport)
                    {
                        CanImport =
                            CheckReferencedTables(MainDS, AImportFileFormat, AReplaceAddress, ASiteKeyToBeReplaced, ALocationKeyToBeReplaced,
                                ref ReferenceResults,
                                Transaction);
                    }

                    SubmissionOK = true;
                });

            TSubmitChangesResult Res = TSubmitChangesResult.scrError;

            if (CanImport)
            {
                try
                {
                    PartnerImportExportTDSAccess.SubmitChanges(MainDS);
                    Res = TSubmitChangesResult.scrOK;
                }
                catch (Exception ex)
                {
                    TLogging.LogException(ex, "ImportExport.WebConnectors_CommitChanges");
                    TLogging.LogStackTrace(TLoggingType.ToLogfile);
                }
            }

            if (CanImport && (Res == TSubmitChangesResult.scrError))
            {
                // We got an exception!
                string msg = Catalog.GetString("A server error occurred during import of {0} {1}.  ");
                msg += Catalog.GetString("More information is available in the server log file.  No data was imported for this row.");

                AddVerificationResult(
                    ref ReferenceResults,
                    String.Format(msg,
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerClass,
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerShortName),
                    TResultSeverity.Resv_Critical);
            }

            if (((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerClass == "")
            {
                AddVerificationResult(ref ReferenceResults, "Partner has no CLASS!", TResultSeverity.Resv_Critical);
                Res = TSubmitChangesResult.scrInfoNeeded;
            }
            else
            {
                AddVerificationResult(ref ReferenceResults, String.Format("Import of {0} {1} was {2}.",
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerClass,
                        ((PPartnerRow)MainDS.PPartner.Rows[0]).PartnerShortName,
                        Res == TSubmitChangesResult.scrOK ? "successful" : "unsuccessful"
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
        /// <returns>A string that will form the final line of a .ext file</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetExtFileFooter()
        {
            return "0  \"FINISH\"" + Environment.NewLine;
        }

        /// <summary>
        /// Format a partner as ext (Petra 2.x format)
        /// If I've been asked to export a PERSON, I can also export the FAMILY record first.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="ASiteKey">Partner's site key</param>
        /// <param name="ALocationKey">Partner's primary location key</param>
        /// <param name="AIncludeFamilyWithPerson">Set this flag to false for a PERSON, to prevent the FAMILY being exported too.</param>
        /// <param name="AIncludePersonWithFamily">Set this flag to false for a FAMILY, to prevent the PERSONs being exported too.</param>
        /// <param name="ASpecificBuildingInfo">Only include these buildings (null for all)</param>
        /// <param name="AOldPetraFormat">Set this flag if export to be done in old format</param>
        /// <returns>One partner in EXT format</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string ExportPartnerExt(Int64 APartnerKey,
            Int64 ASiteKey,
            Int32 ALocationKey,
            Boolean AIncludeFamilyWithPerson,
            Boolean AIncludePersonWithFamily,
            StringCollection ASpecificBuildingInfo,
            Boolean AOldPetraFormat)
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
                PartnerAccessOk = TPartnerServerLookups.VerifyPartnerAndGetDetails(APartnerKey,
                    out ShortName, out PartnerClass,
                    out IsMergedPartner, out UserCanAccessPartner);
            }

            if (!PartnerAccessOk || !UserCanAccessPartner)
            {
                return extRecord;  // This is empty - TODO: I'm not returning any error code here.
            }

            TPartnerFileExport Exporter = new TPartnerFileExport();
            PartnerImportExportTDS AMainDS = TExportAllPartnerData.ExportPartner(APartnerKey);

            if (AMainDS.PPartner.Rows.Count == 0)
            {
                return extRecord;  // This is empty - TODO: I'm not returning any error code here.
            }

            PPartnerRow PartnerRow = AMainDS.PPartner[0];

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                if (AIncludeFamilyWithPerson)  // I'll check whether there's a FAMILY to go with this Partner.
                {
                    PPersonRow PersonRow = AMainDS.PPerson[0];
                    long FamilyKey = PersonRow.FamilyKey;
                    PartnerAccessOk = TPartnerServerLookups.VerifyPartnerAndGetDetails(FamilyKey,
                        out ShortName, out PartnerClass,
                        out IsMergedPartner, out UserCanAccessPartner);

                    if ((FamilyKey > 0) && PartnerAccessOk && UserCanAccessPartner)
                    {
                        PartnerImportExportTDS FamilyDS = TExportAllPartnerData.ExportPartner(FamilyKey);
                        extRecord += Exporter.ExportPartnerExt(FamilyDS, ASiteKey, ALocationKey, ASpecificBuildingInfo, AOldPetraFormat);
                    }

                    // TODO: If I couldn't access the FAMILY for a PERSON, I should perhaps stop exporting?
                }
            }

            extRecord += Exporter.ExportPartnerExt(AMainDS, ASiteKey, ALocationKey, ASpecificBuildingInfo, AOldPetraFormat);

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                if (AIncludePersonWithFamily)
                {
                    // We need to export the Persons in this Family as well
                    TDBTransaction ReadTransaction = null;
                    DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                        ref ReadTransaction,
                        delegate
                        {
                            PPersonTable Persons = PPersonAccess.LoadViaPFamily(APartnerKey, ReadTransaction);

                            foreach (PPersonRow Row in Persons.Rows)
                            {
                                long personKey = Row.PartnerKey;
                                PartnerAccessOk = TPartnerServerLookups.VerifyPartnerAndGetDetails(personKey,
                                    out ShortName, out PartnerClass,
                                    out IsMergedPartner, out UserCanAccessPartner);

                                if ((personKey > 0) && PartnerAccessOk && UserCanAccessPartner)
                                {
                                    PartnerImportExportTDS PersonDS = TExportAllPartnerData.ExportPartner(personKey, TPartnerClass.PERSON);

                                    // if member has same address as family
                                    if (PPartnerLocationAccess.Exists(personKey, ASiteKey, ALocationKey, ReadTransaction))
                                    {
                                        // export the person member with the same address as family partner
                                        extRecord += Exporter.ExportPartnerExt(PersonDS, ASiteKey, ALocationKey, null, AOldPetraFormat);
                                    }
                                    else
                                    {
                                        PLocationTable LocationTable;
                                        string CountryName;

                                        TAddressTools.GetBestAddress(personKey, out LocationTable, out CountryName, ReadTransaction);

                                        if ((LocationTable != null) && (LocationTable.Rows.Count > 0))
                                        {
                                            // export the person member with the person partner's best address
                                            //LocationKey = LocationTable[0].LocationKey;
                                            extRecord +=
                                                Exporter.ExportPartnerExt(PersonDS, ASiteKey, LocationTable[0].LocationKey, null, AOldPetraFormat);
                                        }
                                    }
                                }
                            }
                        });
                }
            }

            return extRecord;
        }

        /// <summary>
        /// Checks to see if an extract contains at least one family partner
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool CheckExtractContainsFamily(int AExtractId)
        {
            TDBTransaction ReadTransaction = null;
            bool ReturnValue = false;
            string Result = string.Empty;

            string Query = "SELECT CASE WHEN EXISTS (SELECT * FROM m_extract, p_partner" +
                           " WHERE m_extract.m_extract_id_i = " + AExtractId +
                           " AND p_partner.p_partner_key_n = m_extract.p_partner_key_n" +
                           " AND p_partner.p_partner_class_c = '" + MPartnerConstants.PARTNERCLASS_FAMILY + "')" +
                           " THEN 'true' ELSE 'false' END";

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    Result = DBAccess.GDBAccessObj.ExecuteScalar(Query, ReadTransaction).ToString();
                });

            if (Result == "true")
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Export all partners of an extract into an EXT file.
        /// </summary>
        /// <param name="AExtractId">Extract Id</param>
        /// <param name="AIncludeFamilyMembers">True if also family members are to be exported</param>
        /// <param name="AOldPetraFormat">True if to export in old format</param>
        /// <returns>Exported Text</returns>
        [RequireModulePermission("PTNRUSER")]
        public static String ExportExtractPartnersExt(int AExtractId, Boolean AIncludeFamilyMembers, Boolean AOldPetraFormat)
        {
            TDBTransaction ReadTransaction = null;
            TPartnerFileExport Exporter = new TPartnerFileExport();
            String ExtText = Exporter.ExtFileHeader(AOldPetraFormat);
            PartnerImportExportTDS MainDS;
            DataTable ExtractPartners = new MExtractTable();

            ExtractPartners.Columns.Add("p_partner_class_c", typeof(string));

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Exporting Extract"));

            string Query = "SELECT m_extract.*, p_partner.p_partner_class_c" +
                           " FROM m_extract, p_partner" +
                           " WHERE m_extract.m_extract_id_i = " + AExtractId +
                           " AND p_partner.p_partner_key_n = m_extract.p_partner_key_n";

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    ExtractPartners = DBAccess.GDBAccessObj.SelectDT(Query, ExtractPartners.TableName, ReadTransaction);
                    int i = 0;
                    int Total = ExtractPartners.Rows.Count;
                    decimal PercentCompleted = 0;

                    foreach (DataRow ExtractPartner in ExtractPartners.Rows)
                    {
                        // stop if user cancels operation
                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                        {
                            ExtText = null;
                            break;
                        }

                        if (i > 0)
                        {
                            PercentCompleted = decimal.Divide(i, Total) * 100;
                        }

                        i++;

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            string.Format(Catalog.GetString("Exporting partner {0} out of {1}."), i, Total),
                            PercentCompleted);

                        // get keys
                        Int64 PartnerKey = (Int64)ExtractPartner[MExtractTable.GetPartnerKeyDBName()];
                        Int64 SiteKey = (Int64)ExtractPartner[MExtractTable.GetSiteKeyDBName()];
                        int LocationKey = 0;

                        if ((ExtractPartner[MExtractTable.GetLocationKeyDBName()] != System.DBNull.Value)
                            && (ExtractPartner[MExtractTable.GetLocationKeyDBName()] != null))
                        {
                            LocationKey = (Int32)ExtractPartner[MExtractTable.GetLocationKeyDBName()];
                        }

                        if (PartnerKey != 0)
                        {
                            // if row is a family partner and the user wants to also export the family's person members
                            if (AIncludeFamilyMembers && (ExtractPartner["p_partner_class_c"].ToString() == MPartnerConstants.PARTNERCLASS_FAMILY))
                            {
                                // export the family partner in the extract
                                MainDS = TExportAllPartnerData.ExportPartner(PartnerKey, TPartnerClass.FAMILY);
                                ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);

                                PPersonTable Persons = PPersonAccess.LoadViaPFamily(PartnerKey, ReadTransaction);

                                foreach (PPersonRow Row in Persons.Rows)
                                {
                                    // do not export if already in extract
                                    if (ExtractPartners.Select(MExtractTable.GetPartnerKeyDBName() + " = " + Row.PartnerKey).Length > 0)
                                    {
                                        continue;
                                    }

                                    MainDS = TExportAllPartnerData.ExportPartner(Row.PartnerKey, TPartnerClass.PERSON);

                                    // if member has same address as family
                                    if (PPartnerLocationAccess.Exists(Row.PartnerKey, SiteKey, LocationKey, ReadTransaction))
                                    {
                                        // export the person member with the same address as family partner
                                        ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);
                                    }
                                    else
                                    {
                                        PLocationTable LocationTable;
                                        string CountryName;

                                        TAddressTools.GetBestAddress(Row.PartnerKey, out LocationTable, out CountryName, ReadTransaction);

                                        if ((LocationTable != null) && (LocationTable.Rows.Count > 0))
                                        {
                                            // export the person member with the person partner's best address
                                            //LocationKey = LocationTable[0].LocationKey;
                                            ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationTable[0].LocationKey, null, AOldPetraFormat);
                                        }
                                    }
                                }
                            }
                            else if (ExtractPartner["p_partner_class_c"].ToString() == MPartnerConstants.PARTNERCLASS_PERSON) // if person partner
                            {
                                PPersonRow PersonRecord = PPersonAccess.LoadByPrimaryKey(PartnerKey, ReadTransaction)[0];

                                // do not export family if already in extract
                                if (ExtractPartners.Select(MExtractTable.GetPartnerKeyDBName() + " = " + PersonRecord.FamilyKey).Length == 0)
                                {
                                    // export the family record first.
                                    MainDS = TExportAllPartnerData.ExportPartner(PersonRecord.FamilyKey, TPartnerClass.FAMILY);

                                    // if family has same address as person member
                                    if (PPartnerLocationAccess.Exists(PersonRecord.FamilyKey, SiteKey, LocationKey, ReadTransaction))
                                    {
                                        // export the family member with the same address as person member
                                        ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);
                                    }
                                    else
                                    {
                                        PLocationTable LocationTable;
                                        string CountryName;

                                        TAddressTools.GetBestAddress(PersonRecord.FamilyKey, out LocationTable, out CountryName, ReadTransaction);

                                        if ((LocationTable != null) && (LocationTable.Rows.Count > 0))
                                        {
                                            // export the family with the family partner's best address
                                            LocationKey = LocationTable[0].LocationKey;
                                            ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);
                                        }
                                    }
                                }

                                // export the person partner in the extract
                                MainDS = TExportAllPartnerData.ExportPartner(PartnerKey, TPartnerClass.PERSON);
                                ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);
                            }
                            else
                            {
                                // export the partner in the extract
                                MainDS =
                                    TExportAllPartnerData.ExportPartner(PartnerKey,
                                        SharedTypes.PartnerClassStringToEnum(ExtractPartner["p_partner_class_c"].ToString()));
                                ExtText += Exporter.ExportPartnerExt(MainDS, SiteKey, LocationKey, null, AOldPetraFormat);
                            }
                        }
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            if (ExtText != null) // if operation has been cancelled
            {
                ExtText += GetExtFileFooter();
            }

            return ExtText;
        }

        /// <summary>
        /// Export a single partner into an EXT file.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="AIncludeFamilyMembers">True if also family members are to be exported</param>
        /// <param name="AOldPetraFormat">True if to export in old format</param>
        /// <returns>Exported Text</returns>
        [RequireModulePermission("PTNRUSER")]
        public static String ExportSinglePartnerExt(Int64 APartnerKey, Boolean AIncludeFamilyMembers, Boolean AOldPetraFormat)
        {
            TDBTransaction ReadTransaction = null;
            TPartnerFileExport Exporter = new TPartnerFileExport();
            String ExtText = Exporter.ExtFileHeader(AOldPetraFormat);
            PartnerImportExportTDS MainDS;
            TPartnerClass PartnerClass;
            String PartnerShortName;

            if (APartnerKey == 0)
            {
                return "";
            }

            if (!TPartnerServerLookups.GetPartnerShortName(APartnerKey, out PartnerShortName, out PartnerClass))
            {
                // partner not found in db
                return "";
            }

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    // if row is a family partner and the user wants to also export the family's person members
                    if (AIncludeFamilyMembers && (PartnerClass == TPartnerClass.FAMILY))
                    {
                        // export the family with the given key
                        MainDS = TExportAllPartnerData.ExportPartner(APartnerKey);
                        ExtText += Exporter.ExportPartnerExt(MainDS, 0, 0, null, AOldPetraFormat);

                        PPersonTable Persons = PPersonAccess.LoadViaPFamily(APartnerKey, ReadTransaction);

                        foreach (PPersonRow Row in Persons.Rows)
                        {
                            MainDS = TExportAllPartnerData.ExportPartner(Row.PartnerKey, TPartnerClass.PERSON);
                            ExtText += Exporter.ExportPartnerExt(MainDS, 0, 0, null, AOldPetraFormat);
                        }
                    }
                    else if (PartnerClass == TPartnerClass.PERSON) // if person partner --> also export family
                    {
                        PPersonRow PersonRecord = PPersonAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction)[0];

                        // export the family record first.
                        MainDS = TExportAllPartnerData.ExportPartner(PersonRecord.FamilyKey, TPartnerClass.FAMILY);
                        ExtText += Exporter.ExportPartnerExt(MainDS, 0, 0, null, AOldPetraFormat);

                        // and now export the person
                        MainDS = TExportAllPartnerData.ExportPartner(APartnerKey, TPartnerClass.PERSON);
                        ExtText += Exporter.ExportPartnerExt(MainDS, 0, 0, null, AOldPetraFormat);
                    }
                    else
                    {
                        // export the partner with the given key
                        MainDS = TExportAllPartnerData.ExportPartner(APartnerKey);
                        ExtText += Exporter.ExportPartnerExt(MainDS, 0, 0, null, AOldPetraFormat);
                    }
                });

            // add footer
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
            TDBTransaction ReadTransaction = null;
            TPartnerFileExport Exporter = new TPartnerFileExport();
            String ExtText = Exporter.ExtFileHeader(false);
            PartnerImportExportTDS MainDS;
            PPartnerTable Partners = new PPartnerTable();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    Partners = PPartnerAccess.LoadAll(ReadTransaction);
                });

            foreach (PPartnerRow Partner in Partners.Rows)
            {
                if ((Partner.PartnerKey != 0) && (Partner.PartnerKey != 1000000)) // skip organization root and 0 when exporting
                {
                    MainDS = TExportAllPartnerData.ExportPartner(Partner.PartnerKey);
                    ExtText += Exporter.ExportPartnerExt(MainDS, /*ASiteKey*/ 0, /*ALocationKey*/ 0, null, false);
                }
            }

            ExtText += GetExtFileFooter();
            return ExtText;
        }
    }
}
