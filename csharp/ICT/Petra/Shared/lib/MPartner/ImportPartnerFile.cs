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
using System.Text;
using System.Data;
using Ict.Common.IO;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MHospitality.Data;

namespace Ict.Petra.Shared.MPartner.IO
{
    /// <summary>
    /// Import all data of a partner
    /// </summary>
    public class TPartnerFileImport : TImportExportTextFile
    {
        private PartnerImportExportTDS FMainDS = null;
        private int FCountLocationKeys = -1;
        private Int64 FPartnerKey = -1;

        private PPartnerRow ImportPartner()
        {
            PPartnerRow PartnerRow = FMainDS.PPartner.NewRowTyped();

            FMainDS.PPartner.Rows.Add(PartnerRow);

            PartnerRow.PartnerKey = ReadInt64();
            PartnerRow.PartnerClass = ReadString();
            PartnerRow.PartnerShortName = ReadString();
            PartnerRow.AcquisitionCode = ReadString();
            PartnerRow.StatusCode = ReadString();
            PartnerRow.PreviousName = ReadString();
            PartnerRow.LanguageCode = ReadString();
            PartnerRow.AddresseeTypeCode = ReadString();
            PartnerRow.ChildIndicator = ReadBoolean();
            PartnerRow.ReceiptEachGift = ReadBoolean();
            PartnerRow.ReceiptLetterFrequency = ReadString();
            PartnerRow.NoSolicitations = ReadBoolean();
            PartnerRow.AnonymousDonor = ReadBoolean();

            if (PartnerRow.AcquisitionCode.Length == 0)
            {
                PartnerRow.AcquisitionCode = MPartnerConstants.ACQUISITIONCODE_APPLICANT;
            }

            PartnerRow.StatusCode = MPartnerConstants.PARTNERSTATUS_ACTIVE;

            return PartnerRow;
        }

        private void ImportPartnerClassSpecific(string APartnerClass)
        {
            if (APartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                PChurchRow ChurchRow = FMainDS.PChurch.NewRowTyped();
                FMainDS.PChurch.Rows.Add(ChurchRow);
                ChurchRow.PartnerKey = FPartnerKey;
                ChurchRow.ChurchName = ReadString();
                ChurchRow.DenominationCode = ReadString();
                ChurchRow.Accomodation = ReadBoolean();
                ChurchRow.AccomodationSize = ReadInt32();
                ChurchRow.AccomodationType = ReadString();
                ChurchRow.ApproximateSize = ReadInt32();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                PFamilyRow FamilyRow = FMainDS.PFamily.NewRowTyped();
                FMainDS.PFamily.Rows.Add(FamilyRow);
                FamilyRow.PartnerKey = FPartnerKey;
                FamilyRow.FamilyName = ReadString();
                FamilyRow.FirstName = ReadString();
                FamilyRow.Title = ReadString();
                FamilyRow.FieldKey = ReadInt64();
                FamilyRow.MaritalStatus = ReadString();
                FamilyRow.MaritalStatusSince = ReadNullableDate();
                FamilyRow.MaritalStatusComment = ReadString();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                PPersonRow PersonRow = FMainDS.PPerson.NewRowTyped();
                FMainDS.PPerson.Rows.Add(PersonRow);
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
                PersonRow.BelieverSinceYear = ReadInt32();
                PersonRow.BelieverSinceComment = ReadString();
                PersonRow.OccupationCode = ReadString();
                PersonRow.FieldKey = ReadInt64();
                PersonRow.FamilyKey = ReadInt64();
                PersonRow.FamilyId = ReadInt32();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                POrganisationRow OrganisationRow = FMainDS.POrganisation.NewRowTyped();
                FMainDS.POrganisation.Rows.Add(OrganisationRow);
                OrganisationRow.PartnerKey = FPartnerKey;
                OrganisationRow.OrganisationName = ReadString();
                OrganisationRow.BusinessCode = ReadString();
                OrganisationRow.Religious = ReadBoolean();
                OrganisationRow.Foundation = ReadBoolean();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                PUnitRow UnitRow = FMainDS.PUnit.NewRowTyped();
                FMainDS.PUnit.Rows.Add(UnitRow);
                UnitRow.PartnerKey = FPartnerKey;
                UnitRow.UnitName = ReadString();
                ReadString(); // was omss code
                UnitRow.XyzTbdCode = ReadString();
                UnitRow.Description = ReadString();
                ReadInt32(); // was um_default_entry_conf_key_n
                UnitRow.UnitTypeCode = ReadString();
                UnitRow.CountryCode = ReadString();
                UnitRow.XyzTbdCost = ReadDouble();
                UnitRow.XyzTbdCostCurrencyCode = ReadString();
                UnitRow.PrimaryOffice = ReadInt64();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                PVenueRow VenueRow = FMainDS.PVenue.NewRowTyped();
                FMainDS.PVenue.Rows.Add(VenueRow);
                VenueRow.PartnerKey = FPartnerKey;
                VenueRow.VenueName = ReadString();
                VenueRow.VenueCode = ReadString();
                VenueRow.CurrencyCode = ReadString();
                VenueRow.ContactPartnerKey = ReadInt64();
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                PBankRow BankRow = FMainDS.PBank.NewRowTyped();
                FMainDS.PBank.Rows.Add(BankRow);
                BankRow.PartnerKey = FPartnerKey;
            }
        }

        private void ImportLocation()
        {
            PLocationRow LocationRow = FMainDS.PLocation.NewRowTyped();

            FMainDS.PLocation.Rows.Add(LocationRow);

            LocationRow.LocationKey = FCountLocationKeys--;
            LocationRow.SiteKey = ReadInt64();
            LocationRow.Locality = ReadString();
            LocationRow.StreetName = ReadString();
            LocationRow.Address3 = ReadString();
            LocationRow.City = ReadString();
            LocationRow.County = ReadString();
            LocationRow.PostalCode = ReadString();
            LocationRow.CountryCode = ReadString();

            PPartnerLocationRow PartnerLocationRow = FMainDS.PPartnerLocation.NewRowTyped();
            FMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);

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
        }

        private void ImportAbility()
        {
            PmPersonAbilityRow PersonAbilityRow = FMainDS.PmPersonAbility.NewRowTyped();

            FMainDS.PmPersonAbility.Rows.Add(PersonAbilityRow);
            PersonAbilityRow.PartnerKey = FPartnerKey;

            PersonAbilityRow.AbilityAreaName = ReadString();
            PersonAbilityRow.AbilityLevel = ReadInt32();
            PersonAbilityRow.YearsOfExperience = ReadInt32();
            PersonAbilityRow.BringingInstrument = ReadBoolean();
            PersonAbilityRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonAbilityRow.Comment = ReadString();
        }

        private void ReadShortApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmShortTermApplicationRow ShortTermApplicationRow = FMainDS.PmShortTermApplication.NewRowTyped();

            FMainDS.PmShortTermApplication.Rows.Add(ShortTermApplicationRow);
            ShortTermApplicationRow.PartnerKey = FPartnerKey;
            ShortTermApplicationRow.ApplicationKey = AGeneralApplicationRow.ApplicationKey;

            ShortTermApplicationRow.ConfirmedOptionCode = ReadString();
            ShortTermApplicationRow.Option1Code = ReadString();
            ShortTermApplicationRow.Option2Code = ReadString();
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
            ShortTermApplicationRow.StBookingFeeReceived = ReadBoolean();
            ShortTermApplicationRow.StXyzTbdOnlyFlag = ReadBoolean();
            ShortTermApplicationRow.StCmpgnSpecialCost = ReadInt32();
            ShortTermApplicationRow.StCngrssSpecialCost = ReadInt32();

            ShortTermApplicationRow.StComment = ReadString();

            ShortTermApplicationRow.StConfirmedOption = ReadInt64();
            ShortTermApplicationRow.StCongressCode = ReadString();
            ShortTermApplicationRow.StCongressLanguage = ReadString();
            ShortTermApplicationRow.StCountryPref = ReadString();
            ShortTermApplicationRow.StCurrentField = ReadInt64();
            ShortTermApplicationRow.XyzTbdRole = ReadString();

            ShortTermApplicationRow.StFgCode = ReadString();
            ShortTermApplicationRow.StFgLeader = ReadBoolean();
            ShortTermApplicationRow.StFieldCharged = ReadInt64();
            ShortTermApplicationRow.StLeadershipRating = ReadString();
            ShortTermApplicationRow.StOption1 = ReadInt64();
            ShortTermApplicationRow.StOption2 = ReadInt64();

            ShortTermApplicationRow.StPartyContact = ReadInt64();
            ShortTermApplicationRow.StPartyTogether = ReadString();
            ShortTermApplicationRow.StPreCongressCode = ReadString();
            ShortTermApplicationRow.StProgramFeeReceived = ReadBoolean();
            ShortTermApplicationRow.StRecruitEfforts = ReadString();
            ShortTermApplicationRow.StScholarshipAmount = ReadDouble();
            ShortTermApplicationRow.StScholarshipApprovedBy = ReadString();
            ShortTermApplicationRow.StScholarshipPeriod = ReadString();
            ShortTermApplicationRow.StScholarshipReviewDate = ReadNullableDate();

            ShortTermApplicationRow.StSpecialApplicant = ReadString();
            ShortTermApplicationRow.StActivityPref = ReadString();
            ShortTermApplicationRow.ToCongTravelInfo = ReadString();
            ShortTermApplicationRow.ArrivalPointCode = ReadString();
            ShortTermApplicationRow.DeparturePointCode = ReadString();
            ShortTermApplicationRow.TravelTypeFromCongCode = ReadString();
            ShortTermApplicationRow.TravelTypeToCongCode = ReadString();

            ShortTermApplicationRow.ContactNumber = ReadString();
            ShortTermApplicationRow.ArrivalDetailsStatus = ReadString();
            ShortTermApplicationRow.ArrivalTransportNeeded = ReadBoolean();
            ShortTermApplicationRow.ArrivalExp = ReadNullableDate();
            ShortTermApplicationRow.ArrivalExpHour = ReadInt32();
            ShortTermApplicationRow.ArrivalExpMinute = ReadInt32();
            ShortTermApplicationRow.ArrivalComments = ReadString();
            ShortTermApplicationRow.TransportInterest = ReadBoolean();

            ShortTermApplicationRow.DepartureDetailsStatus = ReadString();
            ShortTermApplicationRow.DepartureTransportNeeded = ReadBoolean();
            ShortTermApplicationRow.DepartureExp = ReadNullableDate();
            ShortTermApplicationRow.DepartureExpHour = ReadInt32();
            ShortTermApplicationRow.DepartureExpMinute = ReadInt32();
            ShortTermApplicationRow.DepartureComments = ReadString();
        }

        private void ReadLongApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmYearProgramApplicationRow YearProgramApplicationRow = FMainDS.PmYearProgramApplication.NewRowTyped();

            FMainDS.PmYearProgramApplication.Rows.Add(YearProgramApplicationRow);
            YearProgramApplicationRow.PartnerKey = FPartnerKey;
            YearProgramApplicationRow.ApplicationKey = AGeneralApplicationRow.ApplicationKey;

            YearProgramApplicationRow.HoOrientConfBookingKey = ReadString();
            YearProgramApplicationRow.YpAgreedJoiningCharge = ReadDouble();
            YearProgramApplicationRow.YpAgreedSupportFigure = ReadDouble();
            YearProgramApplicationRow.YpAppFeeReceived = ReadBoolean();
            YearProgramApplicationRow.YpBasicDeleteFlag = ReadBoolean();
            YearProgramApplicationRow.YpJoiningConf = ReadInt32();
            YearProgramApplicationRow.StartOfCommitment = ReadNullableDate();
            YearProgramApplicationRow.EndOfCommitment = ReadNullableDate();
            YearProgramApplicationRow.IntendedComLengthMonths = ReadInt32();
            YearProgramApplicationRow.PositionName = ReadString();
            YearProgramApplicationRow.PositionScope = ReadString();
            YearProgramApplicationRow.AssistantTo = ReadBoolean();

            YearProgramApplicationRow.YpScholarshipAthrizedBy = ReadString();
            YearProgramApplicationRow.YpScholarshipBeginDate = ReadNullableDate();
            YearProgramApplicationRow.YpScholarshipEndDate = ReadNullableDate();
            YearProgramApplicationRow.YpScholarship = ReadDouble();
            YearProgramApplicationRow.YpScholarshipPeriod = ReadString();
            YearProgramApplicationRow.YpScholarshipReviewDate = ReadNullableDate();
            YearProgramApplicationRow.YpSupportPeriod = ReadString();
        }

        private void ReadApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmApplicationFormsRow ApplicationFormRow = FMainDS.PmApplicationForms.NewRowTyped();

            FMainDS.PmApplicationForms.Rows.Add(ApplicationFormRow);
            ApplicationFormRow.PartnerKey = FPartnerKey;
            ApplicationFormRow.ApplicationKey = AGeneralApplicationRow.ApplicationKey;

            ApplicationFormRow.FormName = ReadString();

            ApplicationFormRow.FormDeleteFlag = ReadBoolean();
            ApplicationFormRow.FormEdited = ReadBoolean();
            ApplicationFormRow.FormReceivedDate = ReadNullableDate();
            ApplicationFormRow.FormReceived = ReadBoolean();
            ApplicationFormRow.FormSentDate = ReadNullableDate();
            ApplicationFormRow.FormSent = ReadBoolean();

            ApplicationFormRow.ReferencePartnerKey = ReadInt64();
            ApplicationFormRow.Comment = ReadString();
        }

        private void ImportApplication()
        {
            PtApplicationTypeRow ApplicationTypeRow = FMainDS.PtApplicationType.NewRowTyped();

            FMainDS.PtApplicationType.Rows.Add(ApplicationTypeRow);

            ApplicationTypeRow.AppFormType = ReadString();
            ApplicationTypeRow.AppTypeName = ReadString();
            ApplicationTypeRow.AppTypeDescr = ReadString();

            PmGeneralApplicationRow GeneralApplicationRow = FMainDS.PmGeneralApplication.NewRowTyped();
            FMainDS.PmGeneralApplication.Rows.Add(GeneralApplicationRow);
            GeneralApplicationRow.PartnerKey = FPartnerKey;

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
            GeneralApplicationRow.GenAppPossSrvUnitKey = ReadInt64();
            GeneralApplicationRow.GenAppRecvgFldAccept = ReadNullableDate();
            GeneralApplicationRow.GenAppSrvFldAccept = ReadBoolean();
            GeneralApplicationRow.GenAppSendFldAcceptDate = ReadNullableDate();
            GeneralApplicationRow.GenAppSendFldAccept = ReadBoolean();
            GeneralApplicationRow.GenAppCurrencyCode = ReadString();
            GeneralApplicationRow.PlacementPartnerKey = ReadInt64();
            GeneralApplicationRow.GenAppUpdate = ReadNullableDate();
            GeneralApplicationRow.GenCancelledApp = ReadBoolean();
            GeneralApplicationRow.GenContact1 = ReadString();
            GeneralApplicationRow.GenContact2 = ReadString();
            GeneralApplicationRow.GenYearProgram = ReadString();
            GeneralApplicationRow.ApplicationKey = ReadInt32();
            GeneralApplicationRow.RegistrationOffice = ReadInt64();
            GeneralApplicationRow.Comment = ReadMultiLine();

            if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
            {
                ReadShortApplicationForm(GeneralApplicationRow);
            }
            else if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
            {
                ReadLongApplicationForm(GeneralApplicationRow);
            }

            string KeyWord = ReadString();

            while (KeyWord == "APPL-FORMS")
            {
                ReadApplicationForm(GeneralApplicationRow);

                KeyWord = ReadString();
            }

            if (KeyWord == "END")
            {
                CheckForKeyword("FORMS");
            }
        }

        private void ImportOptionalDetails()
        {
            string KeyWord = ReadString();

            while (KeyWord != "END")
            {
                if (KeyWord == "ABILITY")
                {
                    ImportAbility();
                }
                else if (KeyWord == "ADDRESS")
                {
                    ImportLocation();
                }
                else if (KeyWord == "APPLCTN")
                {
                    ImportApplication();
                }
                else if (KeyWord == "COMMENT")
                {
                    // TODO ImportComment();
                }
                else if (KeyWord == "COMMENTSEQ")
                {
                    // TODO ImportCommentSeq();
                }
                else if (KeyWord == "COMMIT")
                {
                    // TODO ImportCommitment();
                }
                else if (KeyWord == "JOB")
                {
                    // TODO ImportJob();
                }
                else if (KeyWord == "LANGUAGE")
                {
                    // TODO ImportLanguage();
                }
                else if (KeyWord == "PREVEXP")
                {
                    // TODO ImportPreviousExperience();
                }
                else if (KeyWord == "PASSPORT")
                {
                    // TODO ImportPassport();
                }
                else if (KeyWord == "PERSDOCUMENT")
                {
                    // TODO ImportPersonalDocument();
                }
                else if (KeyWord == "PERSONAL")
                {
                    // TODO ImportPersonal();
                }
                else if (KeyWord == "PROFESN")
                {
                    // TODO ImportProfession();
                }
                else if (KeyWord == "PROGREP")
                {
                    // TODO ImportPersonEvaluation();
                }
                else if (KeyWord == "SPECNEED")
                {
                    // TODO ImportSpecialNeeds();
                }
                else if (KeyWord == "TYPE")
                {
                    // TODO ImportPartnerType();
                }
                else if (KeyWord == "INTEREST")
                {
                    // TODO ImportInterest();
                }
                else if (KeyWord == "VISION")
                {
                    // TODO ImportVision();
                }
                else if (KeyWord == "U-ABILITY")
                {
                    // TODO ImportUnitAbility();
                }
                else if (KeyWord == "U-COSTS")
                {
                    // TODO ImportUnitCosts();
                }
                else if (KeyWord == "U-JOB")
                {
                    // TODO ImportUnitJob();
                }
                else if (KeyWord == "UJ-ABIL")
                {
                    // TODO ImportUnitJobAbility();
                }
                else if (KeyWord == "UJ-LANG")
                {
                    // TODO ImportUnitJobLanguage();
                }
                else if (KeyWord == "UJ-QUAL")
                {
                    // TODO ImportUnitJobQualification();
                }
                else if (KeyWord == "UJ-VISION")
                {
                    // TODO ImportUnitJobVision();
                }
                else if (KeyWord == "U-LANG")
                {
                    // TODO ImportUnitLanguage();
                }
                else if (KeyWord == "U-STRUCT")
                {
                    // TODO ImportUnitStructure();
                }
                else if (KeyWord == "U-VISION")
                {
                    // TODO ImportUnitVision();
                }
                else if (KeyWord == "V-BUILDING")
                {
                    // TODO ImportBuilding();
                }
                else if (KeyWord == "V-ROOM")
                {
                    // TODO ImportRoom();
                }
                else
                {
                    throw new Exception("found unknown option " + KeyWord);
                }
            }
        }

        /// <summary>
        /// import all data of a partner from a text file, using a format used by Petra 2.x.
        /// containing: partner, person/family/church/etc record, valid locations, special types,
        ///             interests, personnel data, commitments, applications
        /// for units there is more specific data, used eg. for the events file
        /// </summary>
        public PartnerImportExportTDS ImportAllData(string[] ALinesToImport)
        {
            FCountLocationKeys = -1;
            FMainDS = new PartnerImportExportTDS();

            InitReading(ALinesToImport);

            while (CheckForKeyword("PARTNER"))
            {
                PPartnerRow PartnerRow = ImportPartner();

                FPartnerKey = PartnerRow.PartnerKey;

                ImportPartnerClassSpecific(PartnerRow.PartnerClass);

                ImportLocation();

                ImportOptionalDetails();
            }

            return FMainDS;
        }
    }
}