//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System.Text;
using System.Data;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// Import all data of a partner
    /// </summary>
    public class TPartnerFileImport : TImportExportTextFile
    {
        private PartnerImportExportTDS FMainDS = null;
        private List <Int64>FRequiredOfficeKeys = new List <long>();
        private List <Int64>FRequiredOptionKeys = new List <long>();
        private List <Int64>FPartnerAlreadyLoaded = new List <long>();
        private TVerificationResultCollection FResultList = new TVerificationResultCollection();
        private string FLimitToOption = string.Empty;
        private bool FDoNotOverwrite = false;
        private int FCountLocationKeys = -1;
        private Int64 FPartnerKey = -1;
        private bool FIgnorePartner = false;
        private bool FIgnoreApplication = false;
        private static String ImportContext;

        private void AddVerificationResult(String AResultText, TResultSeverity ASeverity)
        {
            if (ASeverity != TResultSeverity.Resv_Status)
            {
                TLogging.Log(AResultText);
            }

            FResultList.Add(new TVerificationResult(ImportContext, AResultText, ASeverity));
        }

        private void AddVerificationResult(String AResultText)
        {
            AddVerificationResult(AResultText, TResultSeverity.Resv_Noncritical);
        }

        private void AddRequiredOffice(Int64 AOfficeKey)
        {
            if ((AOfficeKey != 0) && !FRequiredOfficeKeys.Contains(AOfficeKey))
            {
                FRequiredOfficeKeys.Add(AOfficeKey);
            }
        }

        private void AddUnitOption(Int64 AOptionKey)
        {
            if ((AOptionKey != 0) && !FRequiredOptionKeys.Contains(AOptionKey))
            {
                FRequiredOptionKeys.Add(AOptionKey);
            }
        }

        /// <summary>
        /// This replaces AddRequiredUnits, below - actually I prefer not to add any units,
        /// because no information can be known about them. Instead I'll go back and erase
        /// any references that are not valid.
        /// </summary>
        private void CheckRequiredUnits(TDBTransaction ATransaction)
        {
            // First the option keys, which are all out of ShortTermApplications:
            foreach (Int64 OptionCode in FRequiredOptionKeys)
            {
                if (!PUnitAccess.Exists(OptionCode, ATransaction))
                {
                    foreach (PmShortTermApplicationRow Row in FMainDS.PmShortTermApplication.Rows)
                    {
                        if (!Row.IsStConfirmedOptionNull() && (Row.StConfirmedOption == OptionCode))
                        {
                            AddVerificationResult("Unknown StConfirmedOption in ShortTermApplicationRow: " + OptionCode);
                            Row.SetStConfirmedOptionNull();
                        }

/* StOption1 and StOption2 are removed.
 *
 *                      if (!Row.IsStOption1Null() && Row.StOption1 == OptionCode)
 *                      {
 *                          AddVerificationResult("Unknown StOption1 in ShortTermApplicationRow: " + OptionCode);
 *                          Row.SetStOption1Null();
 *                      }
 *                      if (!Row.IsStOption2Null() && Row.StOption2 == OptionCode)
 *                      {
 *                          AddVerificationResult("Unknown StOption2 in ShortTermApplicationRow: " + OptionCode);
 *                          Row.SetStOption2Null();
 *                      }
 */
                    }
                }
            }

            // then these required offices, which came from a few different places:
            foreach (Int64 OfficeCode in FRequiredOfficeKeys)
            {
                if (!PUnitAccess.Exists(OfficeCode, ATransaction))
                {
                    foreach (PFamilyRow Row in FMainDS.PFamily.Rows)
                    {
                        if (!Row.IsFieldKeyNull() && (Row.FieldKey == OfficeCode))
                        {
                            AddVerificationResult("Unknown FieldKey in FamilyRow: " + OfficeCode);
                            Row.SetFieldKeyNull();
                        }
                    }

                    foreach (PPersonRow Row in FMainDS.PPerson.Rows)
                    {
                        if (!Row.IsFieldKeyNull() && (Row.FieldKey == OfficeCode))
                        {
                            AddVerificationResult("Unknown FieldKey in PersonRow: " + OfficeCode);
                            Row.SetFieldKeyNull();
                        }
                    }

/*
 *                  // I can't do this because RegistrationOffice is part of GeneralApplicationRow's primary Key
 *                  // So I mustn't change it after calling AddOrModifyRecord.
 *
 *                  foreach (PmGeneralApplicationRow Row in FMainDS.PmGeneralApplication.Rows)
 *                  {
 *                      if (!Row.IsRegistrationOfficeNull() && Row.RegistrationOffice == OfficeCode)
 *                      {
 *                          AddVerificationResult("Unknown RegistrationOffice in GeneralApplicationRow: " + OfficeCode);
 *                          Row.RegistrationOffice = DomainManager.GSiteKey;
 *                      }
 *                  }
 */
                    foreach (PmShortTermApplicationRow Row in FMainDS.PmShortTermApplication.Rows)
                    {
                        if (!Row.IsStCurrentFieldNull() && (Row.StCurrentField == OfficeCode))
                        {
                            AddVerificationResult("Unknown CurrentField in ShortTermApplicationRow: " + OfficeCode);
                            Row.SetStCurrentFieldNull();
                        }

/*
 *                  // I can't do this because RegistrationOffice is part of PmShortTermApplication's primary Key
 *                  // so I mustn't change it after calling AddOrModifyRecord.
 *
 *                      if (!Row.IsRegistrationOfficeNull() && Row.RegistrationOffice == OfficeCode)
 *                      {
 *                          AddVerificationResult("Unknown RegistrationOffice in ShortTermApplicationRow: " + OfficeCode);
 *                          Row.RegistrationOffice = DomainManager.GSiteKey;
 *                      }
 */
                        if (!Row.IsStFieldChargedNull() && (Row.StFieldCharged == OfficeCode))
                        {
                            AddVerificationResult("Unknown StFieldCharged in ShortTermApplicationRow: " + OfficeCode);
                            Row.SetStFieldChargedNull();
                        }
                    }

                    foreach (PmStaffDataRow Row in FMainDS.PmStaffData.Rows)
                    {
                        if (!Row.IsHomeOfficeNull() && (Row.HomeOffice == OfficeCode))
                        {
                            AddVerificationResult("Unknown HomeOffice in StaffDataRow: " + OfficeCode);
                            // TODO: I really want to put NULL in here.
                            Row.HomeOffice = DomainManager.GSiteKey;
                        }

                        if (!Row.IsReceivingFieldNull() && (Row.ReceivingField == OfficeCode))
                        {
                            AddVerificationResult("Unknown ReceivingField in StaffDataRow: " + OfficeCode);
                            // TODO: I really want to put NULL in here.
                            Row.ReceivingField = DomainManager.GSiteKey;
                        }

                        if (!Row.IsOfficeRecruitedByNull() && (Row.OfficeRecruitedBy == OfficeCode))
                        {
                            AddVerificationResult("Unknown OfficeRecruitedBy in StaffDataRow: " + OfficeCode);
                            // TODO: I really want to put NULL in here.
                            Row.OfficeRecruitedBy = DomainManager.GSiteKey;
                        }

                        if (!Row.IsReceivingFieldOfficeNull() && (Row.ReceivingFieldOffice == OfficeCode))
                        {
                            AddVerificationResult("Unknown ReceivingFieldOffice in StaffDataRow: " + OfficeCode);
                            Row.SetReceivingFieldOfficeNull();
                        }
                    }

                    foreach (UmJobRow Row in FMainDS.UmJob.Rows)
                    {
                        if (!Row.IsUnitKeyNull() && (Row.UnitKey == OfficeCode))
                        {
                            AddVerificationResult("Unknown UnitKey in JobRow: " + OfficeCode);
                            // TODO: I really want to put NULL in here.
                            Row.UnitKey = DomainManager.GSiteKey;
                        }
                    }

                    foreach (PmJobAssignmentRow Row in FMainDS.PmJobAssignment.Rows)
                    {
                        if (!Row.IsUnitKeyNull() && (Row.UnitKey == OfficeCode))
                        {
                            AddVerificationResult("Unknown UnitKey in JobAssignmentRow: " + OfficeCode);
                            // TODO: I really want to put NULL in here.
                            Row.UnitKey = DomainManager.GSiteKey;
                        }
                    }

                    foreach (PPartnerInterestRow Row in FMainDS.PPartnerInterest.Rows)
                    {
                        if (!Row.IsFieldKeyNull() && (Row.FieldKey == OfficeCode))
                        {
                            AddVerificationResult("Unknown FieldKey in PartnerInterestRow: " + OfficeCode);
                            Row.SetFieldKeyNull();
                        }
                    }
                }
            }
        }

/*
 *      /// <summary>
 *      /// Need to add referenced offices if they don't exist yet
 *      /// </summary>
 *      private void AddRequiredUnits(List <Int64>AUnitKeys, string AUnitType, Int64 AUnitParent, string AUnitNamePrefix, TDBTransaction ATransaction)
 *      {
 *          foreach (Int64 NewUnitKey in AUnitKeys)
 *          {
 *              if (!PUnitAccess.Exists(NewUnitKey, ATransaction))
 *              {
 *                  PUnitRow UnitRow = FMainDS.PUnit.NewRowTyped();
 *                  UnitRow.PartnerKey = NewUnitKey;
 *                  UnitRow.UnitName = AUnitNamePrefix + " " + NewUnitKey.ToString();
 *                  UnitRow.UnitTypeCode = AUnitType;
 *                  FMainDS.PUnit.Rows.Add(UnitRow);
 *
 *                  PPartnerRow partnerRow = FMainDS.PPartner.NewRowTyped();
 *                  partnerRow.PartnerKey = UnitRow.PartnerKey;
 *                  partnerRow.PartnerShortName = UnitRow.UnitName;
 *                  partnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;
 *                  partnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_UNIT;
 *                  FMainDS.PPartner.Rows.Add(partnerRow);
 *
 *                  UmUnitStructureRow UnitStructureRow = FMainDS.UmUnitStructure.NewRowTyped();
 *                  UnitStructureRow.ParentUnitKey = AUnitParent;
 *                  UnitStructureRow.ChildUnitKey = UnitRow.PartnerKey;
 *                  FMainDS.UmUnitStructure.Rows.Add(UnitStructureRow);
 *
 *                  // TODO: should we add an empty location or not?
 *                  // this currently causes problem with the generated code, with the sequence for the
 *                  PLocationRow locationRow = FMainDS.PLocation.NewRowTyped();
 *                  locationRow.SiteKey = UnitRow.PartnerKey;
 *                  locationRow.LocationKey = 0;
 *                  locationRow.StreetName = Catalog.GetString("No valid address on file");
 *                  FMainDS.PLocation.Rows.Add(locationRow);
 *
 *                  PPartnerLocationRow partnerLocationRow = FMainDS.PPartnerLocation.NewRowTyped();
 *                  partnerLocationRow.SiteKey = UnitRow.PartnerKey;
 *                  partnerLocationRow.PartnerKey = UnitRow.PartnerKey;
 *                  partnerLocationRow.LocationKey = 0;
 *                  FMainDS.PPartnerLocation.Rows.Add(partnerLocationRow);
 *              }
 *          }
 *      }
 */
        /// <summary>
        /// If I'm importing an unknown country code, use "99" instead
        /// </summary>
        /// <param name="ACountryCode"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private String CheckCountryCode(String ACountryCode, TDBTransaction ATransaction)
        {
            if ((ACountryCode == "") || (PCountryAccess.Exists(ACountryCode, ATransaction)))
            {
                return ACountryCode;
            }
            else
            {
                AddVerificationResult("Unknown Country code: " + ACountryCode);
                return "99";  // "Bad country" code
            }
        }

        private String CheckCurrencyCode(String ACurrencyCode, TDBTransaction ATransaction)
        {
            if ((ACurrencyCode == "") || (ACurrencyAccess.Exists(ACurrencyCode, ATransaction)))
            {
                return ACurrencyCode;
            }
            else
            {
                AddVerificationResult("Unknown Currency code: " + ACurrencyCode);
                return "";
            }
        }

        private String CheckJobAssignmentTypeCode(String AAssignmentTypeCode, TDBTransaction ATransaction)
        {
            if ((AAssignmentTypeCode == "") || (PtAssignmentTypeAccess.Exists(AAssignmentTypeCode, ATransaction)))
            {
                return AAssignmentTypeCode;
            }
            else
            {
                AddVerificationResult("Unknown Job Assignment Type Code: " + AAssignmentTypeCode);
                return "";
            }
        }

        /// <summary>
        /// We do not want to add new congress codes,
        /// but limit the registration offices to the existing congress codes (roles)
        /// </summary>
        /// <param name="ACongressCode"></param>
        /// <param name="ATransaction"></param>
        private String CheckCongressCode(string ACongressCode, TDBTransaction ATransaction)
        {
            if (FIgnoreApplication || (ACongressCode.Length == 0))
            {
                return "";
            }

            if (PtCongressCodeAccess.Exists(ACongressCode, ATransaction))
            {
                return ACongressCode;
            }

            {
                AddVerificationResult("Unknown Congress Code " + ACongressCode);
                return "";
            }
        }

        /// <summary>
        /// If I'm importing an unknown Language level, use 99 instead
        /// </summary>
        /// <param name="ALanguageLevel"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private int CheckLanguageLevel(int ALanguageLevel, TDBTransaction ATransaction)
        {
            if (PtLanguageLevelAccess.Exists(ALanguageLevel, ATransaction))
            {
                return ALanguageLevel;
            }
            else
            {
                AddVerificationResult("Unknown Language Level " + ALanguageLevel);
                return 99;  // "Unknown" code
            }
        }

        private PPartnerRow ImportPartner(TDBTransaction ATransaction)
        {
            FPartnerKey = ReadInt64();
            PPartnerRow PartnerRow = FMainDS.PPartner.NewRowTyped();
            PartnerRow.PartnerKey = FPartnerKey;

            if (!PPartnerAccess.Exists(FPartnerKey, ATransaction))
            {
                // look for partners that have the same original key.
                // this can happen when partners are exported from the online registration, and then imported again into the online registration
                PmGeneralApplicationRow LocalPartnerKeyRow = FMainDS.PmGeneralApplication.NewRowTyped(false);
                LocalPartnerKeyRow.LocalPartnerKey = FPartnerKey;
                PmGeneralApplicationTable ExistingApplication = PmGeneralApplicationAccess.LoadUsingTemplate(LocalPartnerKeyRow, ATransaction);

                if (ExistingApplication.Count > 0)
                {
                    FPartnerKey = ExistingApplication[0].PartnerKey;
                }
            }

            ImportContext = String.Format("Importing partner {0}", FPartnerKey);

            if (PPartnerAccess.Exists(FPartnerKey, ATransaction))
            {
                FMainDS.Merge(PPartnerAccess.LoadByPrimaryKey(FPartnerKey, ATransaction));

                FMainDS.PPartner.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                    PPartnerTable.GetPartnerKeyDBName(),
                    FPartnerKey);
                PartnerRow = (PPartnerRow)FMainDS.PPartner.DefaultView[0].Row;
            }

            PartnerRow.PartnerClass = ReadString();
            PartnerRow.PartnerShortName = ReadString();
            PartnerRow.AcquisitionCode = ReadString();
            PartnerRow.StatusCode = ReadString();
            PartnerRow.PreviousName = ReadString();
            PartnerRow.LanguageCode = ReadString();
            PartnerRow.AddresseeTypeCode = ReadString().ToUpper();
            PartnerRow.ChildIndicator = ReadBoolean();
            PartnerRow.ReceiptEachGift = ReadBoolean();
            PartnerRow.ReceiptLetterFrequency = ReadString();

            // These values are not in the ext file
            //  PartnerRow.NoSolicitations = ReadBoolean();
            //  PartnerRow.AnonymousDonor = ReadBoolean();

            // Check if this partner was already imported
            FIgnorePartner = FPartnerAlreadyLoaded.Contains(FPartnerKey);

            if (!FIgnorePartner && !FMainDS.PPartner.Rows.Contains(FPartnerKey))
            {
                PPartnerAccess.AddOrModifyRecord(PartnerRow.PartnerKey, FMainDS.PPartner, PartnerRow, FDoNotOverwrite, ATransaction);
                FPartnerAlreadyLoaded.Add(FPartnerKey);
            }

            return PartnerRow;
        }

        private void ImportPartnerClassSpecific(string APartnerClass, TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            if (APartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                PChurchRow ChurchRow = FMainDS.PChurch.NewRowTyped();
                ChurchRow.PartnerKey = FPartnerKey;
                ChurchRow.ChurchName = ReadString();
                ChurchRow.DenominationCode = ReadString();
                ChurchRow.Accomodation = ReadBoolean();
                ChurchRow.AccomodationSize = ReadInt32();
                ChurchRow.AccomodationType = ReadString();
                ChurchRow.ApproximateSize = ReadInt32();

                if (!FIgnorePartner)
                {
                    PChurchAccess.AddOrModifyRecord(ChurchRow.PartnerKey, FMainDS.PChurch, ChurchRow, FDoNotOverwrite, ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                PFamilyRow FamilyRow = FMainDS.PFamily.NewRowTyped();
                FamilyRow.PartnerKey = FPartnerKey;
                FamilyRow.FamilyName = ReadString();
                FamilyRow.FirstName = ReadString();
                FamilyRow.Title = ReadString();
                try
                {
                    FamilyRow.FieldKey = ReadInt64();

                    if (FamilyRow.FieldKey == 0)
                    {
                        FamilyRow.SetFieldKeyNull();
                    }
                    else
                    {
                        AddRequiredOffice(FamilyRow.FieldKey);
                    }
                }
                catch (Exception)
                {
                    FamilyRow.SetFieldKeyNull();
                }

                FamilyRow.MaritalStatus = ReadString();

                FamilyRow.MaritalStatusSince = ReadNullableDate();
                FamilyRow.MaritalStatusComment = ReadString();

                //
                // The EXT file can have the same family record more than once (where several Partners share a family)
                // I need to check I've not added it already.
                //
                if (!FIgnorePartner)
                {
                    PFamilyAccess.AddOrModifyRecord(FamilyRow.PartnerKey, FMainDS.PFamily, FamilyRow, FDoNotOverwrite, ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                PPersonRow PersonRow = FMainDS.PPerson.NewRowTyped();
                PersonRow.PartnerKey = FPartnerKey;
                PersonRow.FamilyName = ReadString();
                PersonRow.FirstName = ReadString();
                PersonRow.MiddleName1 = ReadString();
                PersonRow.Title = ReadString();
                PersonRow.Decorations = ReadString();
                PersonRow.PreferedName = ReadString();
                PersonRow.DateOfBirth = ReadNullableDate();
                PersonRow.Gender = ReadString();
                PersonRow.MaritalStatus = ReadString();
                PersonRow.MaritalStatusSince = ReadNullableDate();
                PersonRow.MaritalStatusComment = ReadString();

                if (APetraVersion.FileMajorPart < 3)
                {
                    // used to be BelieverSinceYear and BelieverSinceComment before 3.0
                    ReadNullableInt32();
                    ReadString();
                }

                PersonRow.OccupationCode = ReadString();
                Int64? FieldKey = ReadNullableInt64();

                if (FieldKey.HasValue && (FieldKey.Value != 0))
                {
                    PersonRow.FieldKey = FieldKey.Value;
                    AddRequiredOffice(PersonRow.FieldKey);
                }

                PersonRow.FamilyKey = ReadInt64();
                PersonRow.FamilyId = ReadInt32();

                if (TAppSettingsManager.GetValue("AllowCreationPersonRecords", "true", false).ToLower() != "true")
                {
                    throw new Exception(
                        "Import of PERSON records is currently not supported. " +
                        "Add configuration parameter AllowCreationPersonRecords with value true to use PERSON records.");
                }

                if (!FIgnorePartner)
                {
                    PPersonAccess.AddOrModifyRecord(PersonRow.PartnerKey, FMainDS.PPerson, PersonRow, FDoNotOverwrite, ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                POrganisationRow OrganisationRow = FMainDS.POrganisation.NewRowTyped();
                OrganisationRow.PartnerKey = FPartnerKey;
                OrganisationRow.OrganisationName = ReadString();
                OrganisationRow.BusinessCode = ReadString();
                OrganisationRow.Religious = ReadBoolean();
                OrganisationRow.Foundation = ReadBoolean();

                if (!FIgnorePartner)
                {
                    POrganisationAccess.AddOrModifyRecord(OrganisationRow.PartnerKey,
                        FMainDS.POrganisation,
                        OrganisationRow,
                        FDoNotOverwrite,
                        ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                PUnitRow UnitRow = FMainDS.PUnit.NewRowTyped();
                UnitRow.PartnerKey = FPartnerKey;
                UnitRow.UnitName = ReadString();
                ReadString(); // was omss code
                UnitRow.OutreachCode = ReadString();
                UnitRow.Description = ReadString();
                ReadInt32(); // was um_default_entry_conf_key_n
                UnitRow.UnitTypeCode = ReadString();
                UnitRow.CountryCode = CheckCountryCode(ReadString(), ATransaction);
                UnitRow.OutreachCost = ReadDecimal();
                UnitRow.OutreachCostCurrencyCode = CheckCurrencyCode(ReadString(), ATransaction);
                UnitRow.PrimaryOffice = ReadInt64();

                if (!FIgnorePartner)
                {
                    PUnitAccess.AddOrModifyRecord(UnitRow.PartnerKey, FMainDS.PUnit, UnitRow, FDoNotOverwrite, ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                PVenueRow VenueRow = FMainDS.PVenue.NewRowTyped();
                VenueRow.PartnerKey = FPartnerKey;
                VenueRow.VenueName = ReadString();
                VenueRow.VenueCode = ReadString();
                VenueRow.CurrencyCode = CheckCurrencyCode(ReadString(), ATransaction);
                VenueRow.ContactPartnerKey = ReadInt64();

                if (!FIgnorePartner)
                {
                    PVenueAccess.AddOrModifyRecord(VenueRow.PartnerKey, FMainDS.PVenue, VenueRow, FDoNotOverwrite, ATransaction);
                }
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                PBankRow BankRow = FMainDS.PBank.NewRowTyped();
                BankRow.PartnerKey = FPartnerKey;

                if (!FIgnorePartner)
                {
                    PBankAccess.AddOrModifyRecord(BankRow.PartnerKey, FMainDS.PBank, BankRow, FDoNotOverwrite, ATransaction);
                }
            }
        }

        private void ImportLocation(TDBTransaction ATransaction)
        {
            PLocationRow LocationRow = FMainDS.PLocation.NewRowTyped();


            LocationRow.SiteKey = ReadInt64();
            LocationRow.Locality = ReadString();
            LocationRow.StreetName = ReadString();
            LocationRow.Address3 = ReadString();
            LocationRow.City = ReadString();
            LocationRow.County = ReadString();
            LocationRow.PostalCode = ReadString();
            LocationRow.CountryCode = CheckCountryCode(ReadString(), ATransaction);

            // get all locations and partnerlocations of this partner
            LocationRow.LocationKey = 0;
            PLocationTable ExistingLocation = PLocationAccess.LoadViaPPartner(FPartnerKey, ATransaction);

            foreach (PLocationRow ExistingRow in ExistingLocation.Rows)  // If the same address is already stored, use that key.
            {
                if ((ExistingRow.Locality == LocationRow.Locality)
                    && (ExistingRow.StreetName == LocationRow.StreetName)
                    && (ExistingRow.PostalCode == LocationRow.PostalCode))
                {
                    LocationRow.LocationKey = ExistingRow.LocationKey;
                    break;
                }
            }

            if (LocationRow.LocationKey == 0)
            {
                LocationRow.LocationKey = FCountLocationKeys--;
            }

            PPartnerLocationRow PartnerLocationRow = FMainDS.PPartnerLocation.NewRowTyped();

            PartnerLocationRow.PartnerKey = FPartnerKey;
            PartnerLocationRow.SiteKey = LocationRow.SiteKey;
            PartnerLocationRow.LocationKey = LocationRow.LocationKey;
            PartnerLocationRow.DateEffective = ReadNullableDate();
            PartnerLocationRow.DateGoodUntil = ReadNullableDate();
            PartnerLocationRow.LocationType = ReadString();
            PartnerLocationRow.SendMail = ReadBoolean();
            PartnerLocationRow.EmailAddress = ReadString();
            PartnerLocationRow.TelephoneNumber = ReadString();
            PartnerLocationRow.Extension = ReadInt32();
            PartnerLocationRow.FaxNumber = ReadString();
            PartnerLocationRow.FaxExtension = ReadInt32();

            if (!FIgnorePartner)
            {
                if (ExistingLocation.DefaultView.Count > 0)
                {
                    PLocationAccess.AddOrModifyRecord(LocationRow.SiteKey,
                        LocationRow.LocationKey,
                        FMainDS.PLocation,
                        LocationRow,
                        FDoNotOverwrite,
                        ATransaction);
                    PPartnerLocationAccess.AddOrModifyRecord(PartnerLocationRow.PartnerKey,
                        PartnerLocationRow.SiteKey,
                        PartnerLocationRow.LocationKey,
                        FMainDS.PPartnerLocation,
                        PartnerLocationRow,
                        FDoNotOverwrite,
                        ATransaction);
                }
                else
                {
                    FMainDS.PLocation.Rows.Add(LocationRow);
                    FMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
                }
            }
        }

        private void ImportAbility(TDBTransaction ATransaction)
        {
            PmPersonAbilityRow PersonAbilityRow = FMainDS.PmPersonAbility.NewRowTyped();

            PersonAbilityRow.PartnerKey = FPartnerKey;

            PersonAbilityRow.AbilityAreaName = ReadString();
            PersonAbilityRow.AbilityLevel = ReadInt32();
            PersonAbilityRow.YearsOfExperience = ReadInt32();
            PersonAbilityRow.BringingInstrument = ReadBoolean();
            PersonAbilityRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonAbilityRow.Comment = ReadString();

            if (!FIgnorePartner)
            {
                PmPersonAbilityAccess.AddOrModifyRecord(PersonAbilityRow.PartnerKey,
                    PersonAbilityRow.AbilityAreaName,
                    FMainDS.PmPersonAbility,
                    PersonAbilityRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private PtArrivalPointTable FArrivalPointTable = null;

        private void ReadShortApplicationForm(TFileVersionInfo APetraVersion,
            PmGeneralApplicationRow AGeneralApplicationRow,
            TDBTransaction ATransaction)
        {
            PmShortTermApplicationRow ShortTermApplicationRow = FMainDS.PmShortTermApplication.NewRowTyped();

            ShortTermApplicationRow.PartnerKey = FPartnerKey;
            ShortTermApplicationRow.ApplicationKey = AGeneralApplicationRow.ApplicationKey;
            ShortTermApplicationRow.RegistrationOffice = AGeneralApplicationRow.RegistrationOffice;
            ShortTermApplicationRow.StAppDate = AGeneralApplicationRow.GenAppDate;
            ShortTermApplicationRow.StApplicationType = AGeneralApplicationRow.AppTypeName;
            ShortTermApplicationRow.StBasicOutreachId = AGeneralApplicationRow.OldLink;
            ShortTermApplicationRow.ConfirmedOptionCode = ReadString();

            if ((FLimitToOption.Length > 0) && (ShortTermApplicationRow.ConfirmedOptionCode != FLimitToOption))
            {
                FIgnoreApplication = true;
            }

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();     // field removed: ShortTermApplicationRow.Option1Code
                ReadString();     // field removed: ShortTermApplicationRow.Option2Code
            }

            ShortTermApplicationRow.FromCongTravelInfo = ReadString();

            ShortTermApplicationRow.Arrival = ReadNullableDate();
            ShortTermApplicationRow.ArrivalHour = ReadInt32();
            ShortTermApplicationRow.ArrivalMinute = ReadInt32();
            ShortTermApplicationRow.Departure = ReadNullableDate();
            ShortTermApplicationRow.DepartureHour = ReadInt32();
            ShortTermApplicationRow.DepartureMinute = ReadInt32();

            ShortTermApplicationRow.StApplicationHoldReason = ReadString();
            ShortTermApplicationRow.StApplicationOnHold = ReadBoolean();
            ShortTermApplicationRow.StBasicDeleteFlag = ReadBoolean();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadBoolean();      // field removed: ShortTermApplicationRow.StBookingFeeReceived
            }

            ShortTermApplicationRow.StOutreachOnlyFlag = ReadBoolean();
            ShortTermApplicationRow.StOutreachSpecialCost = ReadInt32();
            ShortTermApplicationRow.StCngrssSpecialCost = ReadInt32();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();      // field removed: ShortTermApplicationRow.StComment
            }

            Int64 Option = ReadInt64();

            if (Option > 0)
            {
                ShortTermApplicationRow.StConfirmedOption = Option;

                if (!FIgnoreApplication)
                {
                    AddUnitOption(Option);
                }
            }

            ShortTermApplicationRow.StCongressCode = CheckCongressCode(ReadString(), ATransaction);
            ShortTermApplicationRow.StCongressLanguage = ReadString();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();     // field removed: ShortTermApplicationRow.StCountryPref
            }

            Int64? StCurrentField = ReadNullableInt64();

            if (!FIgnoreApplication && StCurrentField.HasValue && (StCurrentField.Value != 0))
            {
                ShortTermApplicationRow.StCurrentField = StCurrentField.Value;
                AddRequiredOffice(ShortTermApplicationRow.StCurrentField);
            }

            ShortTermApplicationRow.OutreachRole = CheckCongressCode(ReadString(), ATransaction);
            ShortTermApplicationRow.StFgCode = ReadString();
            ShortTermApplicationRow.StFgLeader = ReadBoolean();
            ShortTermApplicationRow.StFieldCharged = ReadInt64();

            if (!FIgnoreApplication)
            {
                AddRequiredOffice(ShortTermApplicationRow.StFieldCharged);
            }

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString(); // field removed: ShortTermApplicationRow.StLeadershipRating

                ReadNullableInt64(); // field removed: StOption1
                ReadNullableInt64(); // field removed: StOption2
                ReadInt64(); // field removed: ShortTermApplicationRow.StPartyContact
                ReadString(); // field removed: ShortTermApplicationRow.StPartyTogether
            }

            ShortTermApplicationRow.StPreCongressCode = CheckCongressCode(ReadString(), ATransaction);

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadBoolean(); // field removed: ShortTermApplicationRow.StProgramFeeReceived
                ReadString(); // field removed: ShortTermApplicationRow.StRecruitEfforts
                ReadDecimal(); // field removed: ShortTermApplicationRow.StScholarshipAmount
                ReadString(); // field removed: ShortTermApplicationRow.StScholarshipApprovedBy
                ReadString(); // field removed: ShortTermApplicationRow.StScholarshipPeriod
                ReadNullableDate(); // field removed: ShortTermApplicationRow.StScholarshipReviewDate
            }

            ShortTermApplicationRow.StSpecialApplicant = ReadString();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();     // field removed: ShortTermApplicationRow.StActivityPref
            }

            ShortTermApplicationRow.ToCongTravelInfo = ReadString();
            ShortTermApplicationRow.ArrivalPointCode = ReadString();

            ShortTermApplicationRow.DeparturePointCode = ReadString();
            ShortTermApplicationRow.TravelTypeFromCongCode = ReadString();
            ShortTermApplicationRow.TravelTypeToCongCode = ReadString();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();     // field removed: ShortTermApplicationRow.ContactNumber
            }

            ShortTermApplicationRow.ArrivalDetailsStatus = ReadString();
            ShortTermApplicationRow.ArrivalTransportNeeded = ReadBoolean();

            if (FArrivalPointTable == null)
            {
                FArrivalPointTable = PtArrivalPointAccess.LoadAll(StringHelper.StrSplit(PtArrivalPointTable.GetCodeDBName(), ","), ATransaction);
            }

            // clear unknown arrival points
            if (FArrivalPointTable.Rows.Find(ShortTermApplicationRow.ArrivalPointCode) == null)
            {
                ShortTermApplicationRow.SetArrivalPointCodeNull();
            }

            if (FArrivalPointTable.Rows.Find(ShortTermApplicationRow.DeparturePointCode) == null)
            {
                ShortTermApplicationRow.SetDeparturePointCodeNull();
            }

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadNullableDate(); // field removed: ShortTermApplicationRow.ArrivalExp
                ReadInt32(); // field removed: ShortTermApplicationRow.ArrivalExpHour
                ReadInt32(); // field removed: ShortTermApplicationRow.ArrivalExpMinute
            }

            ShortTermApplicationRow.ArrivalComments = ReadString();
            ShortTermApplicationRow.TransportInterest = ReadBoolean();

            ShortTermApplicationRow.DepartureDetailsStatus = ReadString();
            ShortTermApplicationRow.DepartureTransportNeeded = ReadBoolean();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadNullableDate(); // field removed: ShortTermApplicationRow.DepartureExp
                ReadInt32(); // field removed: ShortTermApplicationRow.DepartureExpHour
                ReadInt32(); // field removed: ShortTermApplicationRow.DepartureExpMinute
            }

            ShortTermApplicationRow.DepartureComments = ReadString();

            /*
             *  I don't think that this should be done - In "old Petra" we could have 0 in StFieldCharged,
             *  but in OpenPetra that's not a legal value.
             *  (Tim Ingham, Oct 2011)
             */
            if (ShortTermApplicationRow.StFieldCharged == 0)
            {
                // We cannot import a partner that has no field charged - this would be an invalid application.
                // we assume that the registration office will be charged.
                ShortTermApplicationRow.StFieldCharged = ShortTermApplicationRow.RegistrationOffice;
            }

            if (!FIgnoreApplication)
            {
                PmShortTermApplicationAccess.AddOrModifyRecord(
                    ShortTermApplicationRow.PartnerKey,
                    ShortTermApplicationRow.ApplicationKey,
                    ShortTermApplicationRow.RegistrationOffice,
                    FMainDS.PmShortTermApplication, ShortTermApplicationRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ReadLongApplicationForm(TFileVersionInfo APetraVersion,
            PmGeneralApplicationRow AGeneralApplicationRow,
            TDBTransaction ATransaction)
        {
            PmYearProgramApplicationRow YearProgramApplicationRow = FMainDS.PmYearProgramApplication.NewRowTyped();

            YearProgramApplicationRow.PartnerKey = FPartnerKey;
            YearProgramApplicationRow.ApplicationKey = AGeneralApplicationRow.ApplicationKey;
            YearProgramApplicationRow.YpAppDate = AGeneralApplicationRow.GenAppDate;
            YearProgramApplicationRow.YpBasicAppType = AGeneralApplicationRow.AppTypeName;
            YearProgramApplicationRow.RegistrationOffice = AGeneralApplicationRow.RegistrationOffice;

            YearProgramApplicationRow.HoOrientConfBookingKey = ReadString();
            YearProgramApplicationRow.YpAgreedJoiningCharge = ReadDecimal();
            YearProgramApplicationRow.YpAgreedSupportFigure = ReadDecimal();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadBoolean(); // Field removed: YearProgramApplicationRow.YpAppFeeReceived
            }

            YearProgramApplicationRow.YpBasicDeleteFlag = ReadBoolean();
            YearProgramApplicationRow.YpJoiningConf = ReadInt32();
            YearProgramApplicationRow.StartOfCommitment = ReadNullableDate();
            YearProgramApplicationRow.EndOfCommitment = ReadNullableDate();
            YearProgramApplicationRow.IntendedComLengthMonths = ReadInt32();
            YearProgramApplicationRow.PositionName = ReadString();
            YearProgramApplicationRow.PositionScope = ReadString();
            YearProgramApplicationRow.AssistantTo = ReadBoolean();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString(); // Field removed: YearProgramApplicationRow.YpScholarshipAthrizedBy
                ReadNullableDate(); // Field removed: YearProgramApplicationRow.YpScholarshipBeginDate
                ReadNullableDate(); // Field removed: YearProgramApplicationRow.YpScholarshipEndDate
                ReadDecimal(); // Field removed: YearProgramApplicationRow.YpScholarship
                ReadString(); // Field removed: YearProgramApplicationRow.YpScholarshipPeriod
                ReadNullableDate(); // Field removed: YearProgramApplicationRow.YpScholarshipReviewDate
            }

            YearProgramApplicationRow.YpSupportPeriod = ReadString();

            if (!FIgnoreApplication)
            {
                PmYearProgramApplicationAccess.AddOrModifyRecord(
                    YearProgramApplicationRow.PartnerKey,
                    YearProgramApplicationRow.ApplicationKey,
                    YearProgramApplicationRow.RegistrationOffice,
                    FMainDS.PmYearProgramApplication, YearProgramApplicationRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ReadApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow, TDBTransaction ATransaction)
        {
            // only needed in case of file form version < 3.0.0
            ReadString(); // field removed: ApplicationFormRow.FormName

            ReadBoolean(); // field removed: ApplicationFormRow.FormDeleteFlag
            ReadBoolean(); // field removed: ApplicationFormRow.FormEdited
            ReadNullableDate(); // field removed: ApplicationFormRow.FormReceivedDate
            ReadBoolean(); // field removed: ApplicationFormRow.FormReceived
            ReadNullableDate(); // field removed: ApplicationFormRow.FormSentDate
            ReadBoolean(); // field removed: ApplicationFormRow.FormSent

            ReadInt64(); // field removed: ApplicationFormRow.ReferencePartnerKey
            ReadString(); // field removed: ApplicationFormRow.Comment
        }

        private void ImportApplication(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            FIgnoreApplication = FIgnorePartner;

            PtApplicationTypeRow ApplicationTypeRow = FMainDS.PtApplicationType.NewRowTyped();

            ApplicationTypeRow.AppFormType = ReadString();
            ApplicationTypeRow.AppTypeName = ReadString();
            ApplicationTypeRow.AppTypeDescr = ReadString();

            PmGeneralApplicationRow GeneralApplicationRow = FMainDS.PmGeneralApplication.NewRowTyped();

            GeneralApplicationRow.PartnerKey = FPartnerKey;

            GeneralApplicationRow.AppTypeName = ApplicationTypeRow.AppTypeName;
            GeneralApplicationRow.GenAppDate = ReadDate();
            GeneralApplicationRow.OldLink = ReadString();
            GeneralApplicationRow.GenApplicantType = ReadString();
            GeneralApplicationRow.GenApplicationHoldReason = ReadString();
            GeneralApplicationRow.GenApplicationOnHold = ReadBoolean();
            GeneralApplicationRow.GenApplicationStatus = ReadString();
            GeneralApplicationRow.GenAppCancelled = ReadNullableDate();
            GeneralApplicationRow.GenAppCancelReason = ReadString();
            GeneralApplicationRow.GenAppDeleteFlag = ReadBoolean();
            GeneralApplicationRow.Closed = ReadBoolean();
            GeneralApplicationRow.ClosedBy = ReadString();
            GeneralApplicationRow.DateClosed = ReadNullableDate();

            Int64? GenAppPossSrvUnitKey = ReadNullableInt64();

            if (GenAppPossSrvUnitKey.HasValue && (GenAppPossSrvUnitKey.Value != 0))
            {
                GeneralApplicationRow.GenAppPossSrvUnitKey = GenAppPossSrvUnitKey.Value;
            }

            GeneralApplicationRow.GenAppRecvgFldAccept = ReadNullableDate();
            GeneralApplicationRow.GenAppSrvFldAccept = ReadBoolean();
            GeneralApplicationRow.GenAppSendFldAcceptDate = ReadNullableDate();
            GeneralApplicationRow.GenAppSendFldAccept = ReadBoolean();
            GeneralApplicationRow.GenAppCurrencyCode = CheckCurrencyCode(ReadString(), ATransaction);

            Int64? PlacementPartnerKey = ReadNullableInt64();

            if (PlacementPartnerKey.HasValue && (PlacementPartnerKey.Value != 0))
            {
                GeneralApplicationRow.PlacementPartnerKey = PlacementPartnerKey.Value;
            }

            GeneralApplicationRow.GenAppUpdate = ReadNullableDate();
            GeneralApplicationRow.GenCancelledApp = ReadBoolean();
            GeneralApplicationRow.GenContact1 = ReadString();
            GeneralApplicationRow.GenContact2 = ReadString();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString();     // field removed: GeneralApplicationRow.GenYearProgram
            }

            GeneralApplicationRow.ApplicationKey = ReadInt32();
            GeneralApplicationRow.RegistrationOffice = ReadInt64();

            if (!PUnitAccess.Exists(GeneralApplicationRow.RegistrationOffice, ATransaction))
            {
                AddVerificationResult(String.Format("Unknown Registration Office {0}.\n{1} substituted in Application form.",
                        GeneralApplicationRow.RegistrationOffice, DomainManager.GSiteKey));
                GeneralApplicationRow.RegistrationOffice = DomainManager.GSiteKey;

                // I can't do this because this is part of GeneralApplicationRow's primary Key
                // So I mustn't change it after calling AddOrModifyRecord, below.
                //        AddRequiredOffice(GeneralApplicationRow.RegistrationOffice);            }
            }

            GeneralApplicationRow.Comment = ReadMultiLine();

            if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
            {
                ReadShortApplicationForm(APetraVersion, GeneralApplicationRow, ATransaction);
            }
            else if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_LONGFORM)
            {
                ReadLongApplicationForm(APetraVersion, GeneralApplicationRow, ATransaction);
            }

            if (!FIgnoreApplication)
            {
                PmGeneralApplicationAccess.AddOrModifyRecord(
                    GeneralApplicationRow.PartnerKey,
                    GeneralApplicationRow.ApplicationKey,
                    GeneralApplicationRow.RegistrationOffice,
                    FMainDS.PmGeneralApplication, GeneralApplicationRow, FDoNotOverwrite, ATransaction);
            }

            if (APetraVersion.FileMajorPart < 3)
            {
                string KeyWord = ReadString();

                // needs to be kept in to support versions < 3.0.0
                while (KeyWord == "APPL-FORM")
                {
                    ReadApplicationForm(GeneralApplicationRow, ATransaction);

                    KeyWord = ReadString();
                }

                if (KeyWord == "END")
                {
                    CheckForKeyword("FORMS");
                }
            }
        }

        private void ImportComment(PPartnerRow APartnerRow)
        {
            APartnerRow.Comment = ReadString();
        }

        private void ImportCommentSeq()
        {
            PPartnerCommentRow PartnerCommentRow = FMainDS.PPartnerComment.NewRowTyped();

            PartnerCommentRow.PartnerKey = FPartnerKey;

            PartnerCommentRow.Sequence = ReadInt32();
            PartnerCommentRow.Comment = ReadString();

            if (!FIgnorePartner)
            {
                FMainDS.PPartnerComment.Rows.Add(PartnerCommentRow);
            }
        }

        private void ImportStaffData(TDBTransaction ATransaction)
        {
            PmStaffDataRow StaffDataRow = FMainDS.PmStaffData.NewRowTyped();

            StaffDataRow.PartnerKey = FPartnerKey;

            StaffDataRow.SiteKey = ReadInt64();
            StaffDataRow.Key = ReadInt64();
            StaffDataRow.StartOfCommitment = ReadDate();
            StaffDataRow.StartDateApprox = ReadBoolean();
            StaffDataRow.EndOfCommitment = ReadNullableDate();
            StaffDataRow.StatusCode = ReadString();
            StaffDataRow.ReceivingField = ReadInt64();
            StaffDataRow.HomeOffice = ReadInt64();
            StaffDataRow.OfficeRecruitedBy = ReadInt64();

            Int64? ReceivingFieldOffice = ReadNullableInt64();

            if (ReceivingFieldOffice.HasValue && (ReceivingFieldOffice.Value != 0))
            {
                StaffDataRow.ReceivingFieldOffice = ReceivingFieldOffice.Value;
            }

            StaffDataRow.JobTitle = ReadString();
            StaffDataRow.StaffDataComments = ReadString();

            if (StaffDataRow.ReceivingField == 0)
            {
                // We cannot import a partner that has a receiving field 0. This would break referential integrity.
                AddVerificationResult("Error - PmStaffData.ReceivingField for partner " + FPartnerKey.ToString() + " is 0.");
                return;
            }

            if (!FIgnorePartner)
            {
                PmStaffDataAccess.AddOrModifyRecord(StaffDataRow.SiteKey,
                    StaffDataRow.Key,
                    FMainDS.PmStaffData,
                    StaffDataRow,
                    FDoNotOverwrite,
                    ATransaction);

                AddRequiredOffice(StaffDataRow.HomeOffice);
                AddRequiredOffice(StaffDataRow.ReceivingField);
                AddRequiredOffice(StaffDataRow.OfficeRecruitedBy);

                if (!StaffDataRow.IsReceivingFieldOfficeNull())
                {
                    AddRequiredOffice(StaffDataRow.ReceivingFieldOffice);
                }
            }
        }

        private void ImportLanguage(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmPersonLanguageRow PersonLanguageRow = FMainDS.PmPersonLanguage.NewRowTyped();

            PersonLanguageRow.PartnerKey = FPartnerKey;

            PersonLanguageRow.LanguageCode = ReadString();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadBoolean(); // field removed: PersonLanguageRow.WillingToTranslate
                ReadBoolean(); // field removed: PersonLanguageRow.TranslateInto
                ReadBoolean(); // field removed: PersonLanguageRow.TranslateOutOf
            }

            PersonLanguageRow.YearsOfExperience = ReadInt32();
            PersonLanguageRow.LanguageLevel = CheckLanguageLevel(ReadInt32(), ATransaction);
            PersonLanguageRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonLanguageRow.Comment = ReadString();

            if (!FIgnorePartner)
            {
                PmPersonLanguageAccess.AddOrModifyRecord(PersonLanguageRow.PartnerKey,
                    PersonLanguageRow.LanguageCode,
                    FMainDS.PmPersonLanguage,
                    PersonLanguageRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportPreviousExperience(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmPastExperienceRow PastExperienceRow = FMainDS.PmPastExperience.NewRowTyped();

            PastExperienceRow.PartnerKey = FPartnerKey;

            PastExperienceRow.SiteKey = ReadInt64();
            PastExperienceRow.Key = ReadInt64();
            PastExperienceRow.PrevLocation = ReadString();
            PastExperienceRow.StartDate = ReadNullableDate();
            PastExperienceRow.EndDate = ReadNullableDate();
            PastExperienceRow.PrevWorkHere = ReadBoolean();
            PastExperienceRow.PrevWork = ReadBoolean();
            PastExperienceRow.OtherOrganisation = ReadString();
            PastExperienceRow.PrevRole = ReadString();

            if (APetraVersion.Compare(new TFileVersionInfo("2.3.3")) >= 0)
            {
                PastExperienceRow.Category = ReadString();
            }

            PastExperienceRow.PastExpComments = ReadString();

            if (!FIgnorePartner)
            {
                PmPastExperienceAccess.AddOrModifyRecord(PastExperienceRow.SiteKey,
                    PastExperienceRow.Key,
                    FMainDS.PmPastExperience,
                    PastExperienceRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportPassport(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmPassportDetailsRow PassportDetailsRow = FMainDS.PmPassportDetails.NewRowTyped();

            PassportDetailsRow.PartnerKey = FPartnerKey;

            PassportDetailsRow.PassportNumber = ReadString();

            if (APetraVersion.Compare(new TFileVersionInfo("2.3.3")) >= 0)
            {
                PassportDetailsRow.MainPassport = ReadBoolean();
            }

            PassportDetailsRow.CountryOfIssue = CheckCountryCode(ReadString(), ATransaction);
            PassportDetailsRow.DateOfExpiration = ReadNullableDate();
            PassportDetailsRow.DateOfIssue = ReadNullableDate();
            PassportDetailsRow.FullPassportName = ReadString();
            PassportDetailsRow.PassportNationalityCode = ReadString();
            PassportDetailsRow.PassportDetailsType = ReadString();
            PassportDetailsRow.PassportDob = ReadNullableDate();
            PassportDetailsRow.PlaceOfBirth = ReadString();
            PassportDetailsRow.PlaceOfIssue = ReadString();

            if (!FIgnorePartner)
            {
                PmPassportDetailsAccess.AddOrModifyRecord(PassportDetailsRow.PartnerKey,
                    PassportDetailsRow.PassportNumber,
                    FMainDS.PmPassportDetails,
                    PassportDetailsRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportPersonalDocument(TDBTransaction ATransaction)
        {
            PartnerImportExportTDSPmDocumentRow DocumentRow = FMainDS.PmDocument.NewRowTyped();

            DocumentRow.PartnerKey = FPartnerKey;

            DocumentRow.SiteKey = ReadInt64();
            DocumentRow.DocumentKey = ReadInt64();
            DocumentRow.DocCode = ReadString();
            DocumentRow.DocCategory = ReadString(); // DocCategory is a "custom field" - I'll need to add it to PmDocumentCategory later if it's not present.
            DocumentRow.DocumentId = ReadString();
            DocumentRow.PlaceOfIssue = ReadString();
            DocumentRow.DateOfIssue = ReadNullableDate();
            DocumentRow.DateOfStart = ReadNullableDate();
            DocumentRow.DateOfExpiration = ReadNullableDate();
            DocumentRow.AssocDocId = ReadString();
            DocumentRow.ContactPartnerKey = ReadInt64();
            DocumentRow.DocComment = ReadString();

            if (!FIgnorePartner)
            {
                PmDocumentAccess.AddOrModifyRecord(DocumentRow.SiteKey,
                    DocumentRow.DocumentKey,
                    FMainDS.PmDocument,
                    DocumentRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportPersonalData(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmPersonalDataRow PersonalDataRow = FMainDS.PmPersonalData.NewRowTyped();

            PersonalDataRow.PartnerKey = FPartnerKey;

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadString(); // Field removed: PersonalDataRow.DriverStatus
                ReadBoolean(); // Field removed: PersonalDataRow.GenDriverLicense
                ReadString(); // Field removed: PersonalDataRow.DrivingLicenseNumber
                ReadBoolean(); // Field removed: PersonalDataRow.InternalDriverLicense

                if (((APetraVersion.FileMinorPart == 2)
                     && (APetraVersion.FileBuildPart >= 20))
                    || ((APetraVersion.FileMinorPart == 3)
                        && (APetraVersion.FileBuildPart >= 6)))
                {
                    // blood type added with release 2.2.20 and 2.3.6
                    PersonalDataRow.BloodType = ReadString();
                }
            }
            else
            {
                Int32? BelieverSinceYear = ReadNullableInt32();

                if (BelieverSinceYear.HasValue)
                {
                    PersonalDataRow.BelieverSinceYear = BelieverSinceYear.Value;
                }

                PersonalDataRow.BelieverSinceComment = ReadString();
                PersonalDataRow.BloodType = ReadString();
            }

            if (!FIgnorePartner)
            {
                PmPersonalDataAccess.AddOrModifyRecord(FPartnerKey, FMainDS.PmPersonalData, PersonalDataRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ImportProfessionalData(TDBTransaction ATransaction)
        {
            PmPersonQualificationRow PersonQualificationRow = FMainDS.PmPersonQualification.NewRowTyped();

            PersonQualificationRow.PartnerKey = FPartnerKey;

            PersonQualificationRow.QualificationAreaName = ReadString();
            PersonQualificationRow.QualificationLevel = ReadInt32();
            PersonQualificationRow.InformalFlag = ReadBoolean();
            PersonQualificationRow.YearsOfExperience = ReadInt32();
            PersonQualificationRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonQualificationRow.Comment = ReadString();
            PersonQualificationRow.QualificationDate = ReadNullableDate();
            PersonQualificationRow.QualificationExpiry = ReadNullableDate();

            if (!FIgnorePartner)
            {
                PmPersonQualificationAccess.AddOrModifyRecord(PersonQualificationRow.PartnerKey,
                    PersonQualificationRow.QualificationAreaName,
                    FMainDS.PmPersonQualification,
                    PersonQualificationRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportPersonEvaluation(TDBTransaction ATransaction)
        {
            PmPersonEvaluationRow PersonEvaluationRow = FMainDS.PmPersonEvaluation.NewRowTyped();

            PersonEvaluationRow.PartnerKey = FPartnerKey;

            PersonEvaluationRow.EvaluationDate = ReadDate();
            PersonEvaluationRow.Evaluator = ReadString();
            PersonEvaluationRow.EvaluationType = ReadString();
            PersonEvaluationRow.NextEvaluationDate = ReadNullableDate();
            PersonEvaluationRow.EvaluationComments = ReadString();
            PersonEvaluationRow.PersonEvalAction = ReadString();

            if (!FIgnorePartner)
            {
                PmPersonEvaluationAccess.AddOrModifyRecord(PersonEvaluationRow.PartnerKey,
                    PersonEvaluationRow.EvaluationDate,
                    PersonEvaluationRow.Evaluator,
                    FMainDS.PmPersonEvaluation,
                    PersonEvaluationRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportSpecialNeeds(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmSpecialNeedRow SpecialNeedRow = FMainDS.PmSpecialNeed.NewRowTyped();

            SpecialNeedRow.PartnerKey = FPartnerKey;

            SpecialNeedRow.DateCreated = ReadNullableDate();

            if (APetraVersion.FileMajorPart < 3)
            {
                ReadBoolean(); // Field removed: SpecialNeedRow.ContactHomeOffice
            }

            SpecialNeedRow.VegetarianFlag = ReadBoolean();
            SpecialNeedRow.DietaryComment = ReadString();
            SpecialNeedRow.MedicalComment = ReadString();
            SpecialNeedRow.OtherSpecialNeed = ReadString();

            if (!FIgnorePartner)
            {
                PmSpecialNeedAccess.AddOrModifyRecord(SpecialNeedRow.PartnerKey, FMainDS.PmSpecialNeed, SpecialNeedRow, FDoNotOverwrite, ATransaction);
            }
        }

        private PTypeTable FTypeTable = null;

        private void ImportPartnerType(TDBTransaction ATransaction)
        {
            PPartnerTypeRow PartnerTypeRow = FMainDS.PPartnerType.NewRowTyped();

            PartnerTypeRow.PartnerKey = FPartnerKey;

            PartnerTypeRow.TypeCode = ReadString();
            PartnerTypeRow.ValidFrom = ReadNullableDate();
            PartnerTypeRow.ValidUntil = ReadNullableDate();

            if (FTypeTable == null)
            {
                FTypeTable = PTypeAccess.LoadAll(StringHelper.StrSplit(PTypeTable.GetTypeCodeDBName(), ","), ATransaction);
            }

            // ignore types that are not in the database. avoid constraint violation
            if (!FIgnorePartner && (FTypeTable.Rows.Find(PartnerTypeRow.TypeCode) != null))
            {
                PPartnerTypeAccess.AddOrModifyRecord(PartnerTypeRow.PartnerKey,
                    PartnerTypeRow.TypeCode,
                    FMainDS.PPartnerType,
                    PartnerTypeRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportInterest(TDBTransaction ATransaction)
        {
            PartnerImportExportTDSPPartnerInterestRow PartnerInterestRow = FMainDS.PPartnerInterest.NewRowTyped();

            PartnerInterestRow.PartnerKey = FPartnerKey;
            PartnerInterestRow.InterestNumber = ReadInt32();
            Int64 FieldKey = ReadInt64();

            if (FieldKey != 0)
            {
                PartnerInterestRow.FieldKey = FieldKey; // If I've imported 0, leave the field key null.
                AddRequiredOffice(FieldKey);
            }

            PartnerInterestRow.Country = CheckCountryCode(ReadString(), ATransaction);
            PartnerInterestRow.Interest = ReadString();
            PartnerInterestRow.Category = ReadString(); // Category
            PartnerInterestRow.Level = ReadInt32();
            PartnerInterestRow.Comment = ReadString();

            if (!FIgnorePartner)
            {
                PPartnerInterestAccess.AddOrModifyRecord(PartnerInterestRow.PartnerKey,
                    PartnerInterestRow.InterestNumber,
                    FMainDS.PPartnerInterest,
                    PartnerInterestRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportUnitAbility(TDBTransaction ATransaction)
        {
            /* AbilityAreaName */
            ReadString();

            /* AbilityLevel */ ReadInt32();

            /* YearsOfExperience */ ReadInt32();
        }

        private void ImportUnitCosts(TDBTransaction ATransaction)
        {
            /* ValidFromDate */
            ReadDate();

            /* ChargePeriod */ ReadString();

            /* CoupleJoiningChargeIntl */ ReadDecimal();

            /* AdultJoiningChargeIntl */ ReadDecimal();

            /* ChildJoiningChargeIntl */ ReadDecimal();

            /* CoupleCostsPeriodIntl */ ReadDecimal();

            /* SingleCostsPeriodIntl */ ReadDecimal();

            /* Child1CostsPeriodIntl */ ReadDecimal();

            /* Child2CostsPeriodIntl */ ReadDecimal();

            /* Child3CostsPeriodIntl */ ReadDecimal();
        }

        private void ImportJob(TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            PmJobAssignmentRow JobAssignmentRow = FMainDS.PmJobAssignment.NewRowTyped();

            JobAssignmentRow.PartnerKey = FPartnerKey;
            JobAssignmentRow.FromDate = ReadDate();
            JobAssignmentRow.ToDate = ReadNullableDate();
            JobAssignmentRow.PositionName = ReadString();
            JobAssignmentRow.PositionScope = ReadString();
            JobAssignmentRow.AssistantTo = ReadBoolean();

            ReadInt64(); // JobAssignmentRow.JobKey: not to be imported
            ReadInt64(); // JobAssignmentRow.JobAssignmentKey: not to be imported

            JobAssignmentRow.UnitKey = ReadInt64();
            JobAssignmentRow.AssignmentTypeCode = CheckJobAssignmentTypeCode(ReadString(), ATransaction);

            if (APetraVersion.Compare(new TFileVersionInfo("3.0.0")) < 0)
            {
                ReadString();       // used to be JobAssignmentRow.LeavingCode
                ReadNullableDate(); // used to be JobAssignmentRow.LeavingCodeUpdatedDate
            }

            if (!FIgnorePartner)
            {
                // find job assignment (ignoring job key and job assignment key)
                PmJobAssignmentRow TmpJobAssignmentRow = FMainDS.PmJobAssignment.NewRowTyped(false);
                TmpJobAssignmentRow.PartnerKey = FPartnerKey;
                TmpJobAssignmentRow.UnitKey = JobAssignmentRow.UnitKey;
                TmpJobAssignmentRow.PositionName = JobAssignmentRow.PositionName;
                TmpJobAssignmentRow.PositionScope = JobAssignmentRow.PositionScope;

                PmJobAssignmentTable ExistingJobAssignmentTable = PmJobAssignmentAccess.LoadUsingTemplate(TmpJobAssignmentRow, null, ATransaction);

                if (ExistingJobAssignmentTable.Count == 0)
                {
                    // if job assignment does not exist: find job
                    UmJobRow TmpJobRow = FMainDS.UmJob.NewRowTyped(false);
                    TmpJobRow.UnitKey = JobAssignmentRow.UnitKey;
                    TmpJobRow.PositionName = JobAssignmentRow.PositionName;
                    TmpJobRow.PositionScope = JobAssignmentRow.PositionScope;

                    UmJobTable ExistingJobTable = UmJobAccess.LoadUsingTemplate(TmpJobRow, null, ATransaction);

                    if (ExistingJobTable.Count == 0)
                    {
                        // if job does not exist: create job with default values
                        UmJobRow JobRow = FMainDS.UmJob.NewRowTyped(true);
                        JobRow.UnitKey = TmpJobRow.UnitKey;
                        JobRow.PositionName = TmpJobRow.PositionName;
                        JobRow.PositionScope = TmpJobRow.PositionScope;
                        JobRow.JobKey = (Int32)MCommon.WebConnectors.TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_job);
                        JobRow.JobType = "Long Term";
                        JobRow.FromDate = JobAssignmentRow.FromDate;
                        JobRow.ToDate = JobAssignmentRow.ToDate;
                        JobRow.CommitmentPeriod = "None";
                        JobRow.TrainingPeriod = "None";
                        JobRow.Present = 1;

                        UmJobAccess.AddOrModifyRecord(JobRow.UnitKey,
                            JobRow.PositionName,
                            JobRow.PositionScope,
                            JobRow.JobKey,
                            FMainDS.UmJob,
                            JobRow,
                            FDoNotOverwrite,
                            ATransaction);

                        JobAssignmentRow.JobKey = JobRow.JobKey;
                    }
                    else
                    {
                        JobAssignmentRow.JobKey = ((UmJobRow)ExistingJobTable.Rows[0]).JobKey;
                    }

                    JobAssignmentRow.JobAssignmentKey = (Int32)MCommon.WebConnectors.TSequenceWebConnector.GetNextSequence(
                        TSequenceNames.seq_job_assignment);
                }
                else
                {
                    // job assignment already exists: update record in database
                    JobAssignmentRow.JobKey = ((PmJobAssignmentRow)ExistingJobAssignmentTable.Rows[0]).JobKey;
                    JobAssignmentRow.JobAssignmentKey = ((PmJobAssignmentRow)ExistingJobAssignmentTable.Rows[0]).JobAssignmentKey;
                }

                // now add or modify job assignment record
                PmJobAssignmentAccess.AddOrModifyRecord(JobAssignmentRow.PartnerKey,
                    JobAssignmentRow.UnitKey,
                    JobAssignmentRow.PositionName,
                    JobAssignmentRow.PositionScope,
                    JobAssignmentRow.JobKey,
                    JobAssignmentRow.JobAssignmentKey,
                    FMainDS.PmJobAssignment,
                    JobAssignmentRow,
                    FDoNotOverwrite,
                    ATransaction);
            }
        }

        private void ImportUnitJob()
        {
            // Jobs are not being imported into the database anymore from 2.1.0 onwards.
            ReadString();
            ReadString();
            ReadInt32();
            ReadNullableDate();
            ReadNullableDate();
            ReadString();
            ReadString();
            ReadString();
            ReadBoolean();
            ReadBoolean();
            ReadInt32();
            ReadBoolean();
            ReadInt32();
            ReadInt32();
            ReadInt32();
            ReadInt32();
        }

        private void ImportUnitJobAbility()
        {
            // Jobs are not being imported into the database anymore from 2.1.0 onwards.
            ReadString();
            ReadString();
            ReadInt32();
            ReadString();
            ReadInt32();
            ReadInt32();
        }

        private void ImportUnitJobLanguage()
        {
            // Jobs are not being imported into the database anymore from 2.1.0 onwards.
            ReadString();
            ReadString();
            ReadInt32();
            ReadString();
            ReadInt32();
            ReadInt32();
        }

        private void ImportUnitJobQualification()
        {
            // Jobs are not being imported into the database anymore from 2.1.0 onwards.
            ReadString();
            ReadString();
            ReadInt32();
            ReadString();
            ReadInt32();
            ReadInt32();
        }

        private void ImportUnitJobVision()
        {
            // Jobs are not being imported into the database anymore from 2.1.0 onwards.
            // Table um_job_vision dropped in OpenPetra as no longer needed
            ReadString();
            ReadString();
            ReadInt32();
            ReadString();
            ReadInt32();
        }

        private void ImportUnitLanguage(TDBTransaction ATransaction)
        {
            /* LanguageCode */
            ReadString();

            /* LanguageLevel */ ReadInt32();

            /* YearsOfExperience */ ReadInt32();

            /* UnitLanguageReq */ ReadString();

            /* UnitLangComment */ ReadString();
        }

        private void ImportUnitStructure(TDBTransaction ATransaction)
        {
            UmUnitStructureRow UnitStructureRow = FMainDS.UmUnitStructure.NewRowTyped();

            UnitStructureRow.ChildUnitKey = FPartnerKey;
            UnitStructureRow.ParentUnitKey = ReadInt64();

            if (!FIgnorePartner)
            {
                UmUnitStructureAccess.AddOrModifyRecord(UnitStructureRow.ParentUnitKey,
                    UnitStructureRow.ChildUnitKey,
                    FMainDS.UmUnitStructure, UnitStructureRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ImportUnitVision(TDBTransaction ATransaction)
        {
            // Table um_unit_vision dropped in OpenPetra as no longer needed

            /* VisionAreaName */
            ReadString();

            /* VisionLevel */ ReadInt32();
        }

        private void ImportBuilding(TDBTransaction ATransaction)
        {
            PcBuildingRow BuildingRow = FMainDS.PcBuilding.NewRowTyped();

            BuildingRow.VenueKey = FPartnerKey;

            BuildingRow.BuildingCode = ReadString();
            BuildingRow.BuildingDesc = ReadString();

            if (!FIgnorePartner)
            {
                PcBuildingAccess.AddOrModifyRecord(BuildingRow.VenueKey,
                    BuildingRow.BuildingCode,
                    FMainDS.PcBuilding, BuildingRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ImportRoom(TDBTransaction ATransaction)
        {
            PcRoomRow RoomRow = FMainDS.PcRoom.NewRowTyped();

            RoomRow.VenueKey = FPartnerKey;

            RoomRow.BuildingCode = ReadString();
            RoomRow.RoomNumber = ReadString();
            RoomRow.Beds = ReadInt32();
            RoomRow.BedCharge = ReadDecimal();
            RoomRow.BedCost = ReadDecimal();
            RoomRow.MaxOccupancy = ReadInt32();
            RoomRow.GenderPreference = ReadString();

            if (!FIgnorePartner)
            {
                PcRoomAccess.AddOrModifyRecord(RoomRow.VenueKey,
                    RoomRow.BuildingCode,
                    RoomRow.RoomNumber,
                    FMainDS.PcRoom, RoomRow, FDoNotOverwrite, ATransaction);
            }
        }

        private void ImportVision(TDBTransaction ATransaction)
        {
            // Table pm_person_vision dropped in OpenPetra as no longer needed

            ReadString(); /* Vision Area */

            ReadInt32(); /* Vision Level */
            ReadString(); /* Vision Comment */
        }

        private void ImportOptionalDetails(PPartnerRow APartnerRow, TFileVersionInfo APetraVersion, TDBTransaction ATransaction)
        {
            string KeyWord = ReadString();

            while (KeyWord != "END")
            {
                if (KeyWord == "ABILITY")
                {
                    ImportAbility(ATransaction);
                }
                else if (KeyWord == "ADDRESS")
                {
                    ImportLocation(ATransaction);
                }
                else if (KeyWord == "APPLCTN")
                {
                    ImportApplication(APetraVersion, ATransaction);
                }
                else if (KeyWord == "COMMENT")
                {
                    ImportComment(APartnerRow);
                }
                else if (KeyWord == "COMMENTSEQ")
                {
                    ImportCommentSeq();
                }
                else if (KeyWord == "COMMIT")
                {
                    ImportStaffData(ATransaction);
                }
                else if (KeyWord == "JOB")
                {
                    ImportJob(APetraVersion, ATransaction);
                }
                else if (KeyWord == "LANGUAGE")
                {
                    ImportLanguage(APetraVersion, ATransaction);
                }
                else if (KeyWord == "PREVEXP")
                {
                    ImportPreviousExperience(APetraVersion, ATransaction);
                }
                else if (KeyWord == "PASSPORT")
                {
                    ImportPassport(APetraVersion, ATransaction);
                }
                else if (KeyWord == "PERSDOCUMENT")
                {
                    ImportPersonalDocument(ATransaction);
                }
                else if (KeyWord == "PERSONAL")
                {
                    ImportPersonalData(APetraVersion, ATransaction);
                }
                else if (KeyWord == "PROFESN")
                {
                    ImportProfessionalData(ATransaction);
                }
                else if (KeyWord == "PROGREP")
                {
                    ImportPersonEvaluation(ATransaction);
                }
                else if (KeyWord == "SPECNEED")
                {
                    ImportSpecialNeeds(APetraVersion, ATransaction);
                }
                else if (KeyWord == "TYPE")
                {
                    ImportPartnerType(ATransaction);
                }
                else if (KeyWord == "INTEREST")
                {
                    ImportInterest(ATransaction);
                }
                else if (KeyWord == "U-ABILITY")
                {
                    ImportUnitAbility(ATransaction);
                }
                else if (KeyWord == "U-COSTS")
                {
                    ImportUnitCosts(ATransaction);
                }
                else if (KeyWord == "U-JOB")
                {
                    ImportUnitJob();
                }
                else if (KeyWord == "UJ-ABIL")
                {
                    ImportUnitJobAbility();
                }
                else if (KeyWord == "UJ-LANG")
                {
                    ImportUnitJobLanguage();
                }
                else if (KeyWord == "UJ-QUAL")
                {
                    ImportUnitJobQualification();
                }
                else if (KeyWord == "UJ-VISION")
                {
                    ImportUnitJobVision();
                }
                else if (KeyWord == "U-LANG")
                {
                    ImportUnitLanguage(ATransaction);
                }
                else if (KeyWord == "U-STRUCT")
                {
                    ImportUnitStructure(ATransaction);
                }
                else if (KeyWord == "U-VISION")
                {
                    ImportUnitVision(ATransaction);
                }
                else if (KeyWord == "V-BUILDING")
                {
                    ImportBuilding(ATransaction);
                }
                else if (KeyWord == "V-ROOM")
                {
                    ImportRoom(ATransaction);
                }
                else if (KeyWord == "VISION")
                {
                    ImportVision(ATransaction);
                }
                else
                {
                    throw new Exception("Found unknown option " + KeyWord);
                }

                KeyWord = ReadString();
            }
        }

        /// <summary>
        /// Import all data of a partner from a text file, using a format used by Petra 2.x.
        /// containing: partner, person/family/church/etc record, valid locations, special types,
        ///             interests, personnel data, commitments, applications
        /// for units there is more specific data, used eg. for the events file
        /// </summary>
        /// <param name="ALinesToImport"></param>
        /// <param name="ALimitToOption">if this is not an empty string, only the applications for this conference will be imported, historic applications will be ignored</param>
        /// <param name="ADoNotOverwrite">do not modify records that already exist in the database</param>
        /// <param name="AResultList">verification results. can contain critical errors and messages for the user</param>
        /// <returns>nothing - an empty TDS</returns>
        public PartnerImportExportTDS ImportAllData(string[] ALinesToImport,
            string ALimitToOption,
            bool ADoNotOverwrite,
            out TVerificationResultCollection AResultList)
        {
            FResultList = new TVerificationResultCollection();
            FCountLocationKeys = -1;
            FLimitToOption = ALimitToOption;
            FDoNotOverwrite = ADoNotOverwrite;
            FMainDS = new PartnerImportExportTDS();
            TDBTransaction Transaction;
            Boolean NewTransaction;

            InitReading(ALinesToImport);

            TFileVersionInfo PetraVersion = new TFileVersionInfo(ReadString());
            ReadInt64(); // SiteKey
            ReadInt32(); // SubVersion

            if ((PetraVersion.FileMajorPart < 3) && (PetraVersion.FileMinorPart < 2))
            {
                AddVerificationResult(
                    "EXT import not supported from old file type: " + PetraVersion.ToString(), TResultSeverity.Resv_Critical);
            }
            else
            {
                try
                {
                    Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

                    while (CheckForKeyword("PARTNER"))
                    {
                        PPartnerRow PartnerRow = ImportPartner(Transaction);

                        ImportPartnerClassSpecific(PartnerRow.PartnerClass, PetraVersion, Transaction);

                        ImportLocation(Transaction);

                        ImportOptionalDetails(PartnerRow, PetraVersion, Transaction);
                    }

                    ImportContext = "Checking data references";

                    CheckRequiredUnits(Transaction);
                    //                AddRequiredUnits(FRequiredOfficeKeys, "F", 1000000, "Office", Transaction);
                    //                AddRequiredUnits(FRequiredOptionKeys, "CONF", 1000000, "Conference", Transaction);
                }
                catch (Exception e)
                {
                    TLogging.Log(e.GetType().ToString() + ": " + e.Message + " in line " + (CurrentLineCounter + 1).ToString());
                    TLogging.Log(CurrentLine);
                    TLogging.Log(e.StackTrace);
                    throw;
                }
                finally
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            AResultList = FResultList;

            if (AResultList.HasCriticalErrors)
            {
                return new PartnerImportExportTDS();
            }

            return FMainDS;
        }
    }
}