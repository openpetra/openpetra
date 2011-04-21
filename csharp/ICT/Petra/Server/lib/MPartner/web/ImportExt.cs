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
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MHospitality.Data;

namespace Ict.Petra.Server.MPartner.ImportExport
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
                try
                {
                    FamilyRow.FieldKey = ReadInt64();
                }
                catch (Exception)
                {
                    FamilyRow.SetFieldKeyNull();
                }

                FamilyRow.MaritalStatus = ReadString();

                // TODO it seems the NULL value for field key confuses the next values,
                // so date cannot be parsed, because some fields have been jumped?

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

                throw new Exception(
                    "We are currently not supporting import of PERSON records, until we have resolved the issues with household/family");
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
                UnitRow.XyzTbdCost = ReadDecimal();
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
            ShortTermApplicationRow.StScholarshipAmount = ReadDecimal();
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
            YearProgramApplicationRow.YpAgreedJoiningCharge = ReadDecimal();
            YearProgramApplicationRow.YpAgreedSupportFigure = ReadDecimal();
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
            YearProgramApplicationRow.YpScholarship = ReadDecimal();
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

        private void ImportComment(PPartnerRow APartnerRow)
        {
            APartnerRow.Comment = ReadString();
        }

        private void ImportCommentSeq()
        {
            PPartnerCommentRow PartnerCommentRow = FMainDS.PPartnerComment.NewRowTyped();

            FMainDS.PPartnerComment.Rows.Add(PartnerCommentRow);
            PartnerCommentRow.PartnerKey = FPartnerKey;

            PartnerCommentRow.Sequence = ReadInt32();
            PartnerCommentRow.Comment = ReadString();
        }

        private void ImportCommitment()
        {
            PmStaffDataRow StaffDataRow = FMainDS.PmStaffData.NewRowTyped();

            FMainDS.PmStaffData.Rows.Add(StaffDataRow);
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
            StaffDataRow.ReceivingFieldOffice = ReadInt64();
            StaffDataRow.JobTitle = ReadString();
            StaffDataRow.StaffDataComments = ReadString();
        }

        private void ImportLanguage()
        {
            PmPersonLanguageRow PersonLanguageRow = FMainDS.PmPersonLanguage.NewRowTyped();

            FMainDS.PmPersonLanguage.Rows.Add(PersonLanguageRow);
            PersonLanguageRow.PartnerKey = FPartnerKey;

            PersonLanguageRow.LanguageCode = ReadString();
            PersonLanguageRow.WillingToTranslate = ReadBoolean();
            PersonLanguageRow.TranslateInto = ReadBoolean();
            PersonLanguageRow.TranslateOutOf = ReadBoolean();
            PersonLanguageRow.YearsOfExperience = ReadInt32();
            PersonLanguageRow.LanguageLevel = ReadInt32();
            PersonLanguageRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonLanguageRow.Comment = ReadString();
        }

        private void ImportPreviousExperience()
        {
            PmPastExperienceRow PastExperienceRow = FMainDS.PmPastExperience.NewRowTyped();

            FMainDS.PmPastExperience.Rows.Add(PastExperienceRow);
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
            PastExperienceRow.Category = ReadString();
            PastExperienceRow.PastExpComments = ReadString();
        }

        private void ImportPassport()
        {
            PmPassportDetailsRow PassportDetailsRow = FMainDS.PmPassportDetails.NewRowTyped();

            FMainDS.PmPassportDetails.Rows.Add(PassportDetailsRow);
            PassportDetailsRow.PartnerKey = FPartnerKey;

            PassportDetailsRow.PassportNumber = ReadString();
            PassportDetailsRow.MainPassport = ReadBoolean();
            PassportDetailsRow.CountryOfIssue = ReadString();
            PassportDetailsRow.DateOfExpiration = ReadNullableDate();
            PassportDetailsRow.DateOfIssue = ReadNullableDate();
            PassportDetailsRow.FullPassportName = ReadString();
            PassportDetailsRow.PassportNationalityCode = ReadString();
            PassportDetailsRow.PassportDetailsType = ReadString();
            PassportDetailsRow.PassportDob = ReadNullableDate();
            PassportDetailsRow.PlaceOfBirth = ReadString();
            PassportDetailsRow.PlaceOfIssue = ReadString();
        }

        private void ImportPersonalDocument()
        {
            PmDocumentRow DocumentRow = FMainDS.PmDocument.NewRowTyped();

            FMainDS.PmDocument.Rows.Add(DocumentRow);
            DocumentRow.PartnerKey = FPartnerKey;

            DocumentRow.SiteKey = ReadInt64();
            DocumentRow.DocumentKey = ReadInt64();
            DocumentRow.DocCode = ReadString();
            string DocCategory = ReadString();
            DocumentRow.DocumentId = ReadString();
            DocumentRow.PlaceOfIssue = ReadString();
            DocumentRow.DateOfIssue = ReadNullableDate();
            DocumentRow.DateOfStart = ReadNullableDate();
            DocumentRow.DateOfExpiration = ReadNullableDate();
            DocumentRow.AssocDocId = ReadString();
            DocumentRow.ContactPartnerKey = ReadInt64();
            DocumentRow.DocComment = ReadString();

            // TODO: PmDocumentType, PmDocumentCategory
        }

        private void ImportPersonalData()
        {
            PmPersonalDataRow PersonalDataRow = FMainDS.PmPersonalData.NewRowTyped();

            FMainDS.PmPersonalData.Rows.Add(PersonalDataRow);
            PersonalDataRow.PartnerKey = FPartnerKey;

            PersonalDataRow.DriverStatus = ReadString();
            PersonalDataRow.GenDriverLicense = ReadBoolean();
            PersonalDataRow.DrivingLicenseNumber = ReadString();
            PersonalDataRow.InternalDriverLicense = ReadBoolean();

            // TODO: PtDriverStatus
        }

        private void ImportProfessionalData()
        {
            PmPersonQualificationRow PersonQualificationRow = FMainDS.PmPersonQualification.NewRowTyped();

            FMainDS.PmPersonQualification.Rows.Add(PersonQualificationRow);
            PersonQualificationRow.PartnerKey = FPartnerKey;

            PersonQualificationRow.QualificationAreaName = ReadString();
            PersonQualificationRow.QualificationLevel = ReadInt32();
            PersonQualificationRow.InformalFlag = ReadBoolean();
            PersonQualificationRow.YearsOfExperience = ReadInt32();
            PersonQualificationRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonQualificationRow.Comment = ReadString();
            PersonQualificationRow.QualificationDate = ReadNullableDate();
            PersonQualificationRow.QualificationExpiry = ReadNullableDate();
        }

        private void ImportPersonEvaluation()
        {
            PmPersonEvaluationRow PersonEvaluationRow = FMainDS.PmPersonEvaluation.NewRowTyped();

            FMainDS.PmPersonEvaluation.Rows.Add(PersonEvaluationRow);
            PersonEvaluationRow.PartnerKey = FPartnerKey;

            PersonEvaluationRow.EvaluationDate = ReadDate();
            PersonEvaluationRow.Evaluator = ReadString();
            PersonEvaluationRow.EvaluationType = ReadString();
            PersonEvaluationRow.NextEvaluationDate = ReadNullableDate();
            PersonEvaluationRow.EvaluationComments = ReadString();
            PersonEvaluationRow.PersonEvalAction = ReadString();
        }

        private void ImportSpecialNeeds()
        {
            PmSpecialNeedRow SpecialNeedRow = FMainDS.PmSpecialNeed.NewRowTyped();

            FMainDS.PmSpecialNeed.Rows.Add(SpecialNeedRow);
            SpecialNeedRow.PartnerKey = FPartnerKey;

            SpecialNeedRow.DateCreated = ReadNullableDate();
            SpecialNeedRow.ContactHomeOffice = ReadBoolean();
            SpecialNeedRow.VegetarianFlag = ReadBoolean();
            SpecialNeedRow.DietaryComment = ReadString();
            SpecialNeedRow.MedicalComment = ReadString();
            SpecialNeedRow.OtherSpecialNeed = ReadString();
        }

        private void ImportPartnerType()
        {
            PPartnerTypeRow PartnerTypeRow = FMainDS.PPartnerType.NewRowTyped();

            FMainDS.PPartnerType.Rows.Add(PartnerTypeRow);
            PartnerTypeRow.PartnerKey = FPartnerKey;

            PartnerTypeRow.TypeCode = ReadString();
            PartnerTypeRow.ValidFrom = ReadNullableDate();
            PartnerTypeRow.ValidUntil = ReadNullableDate();
        }

        private void ImportInterest()
        {
            PPartnerInterestRow PartnerInterestRow = FMainDS.PPartnerInterest.NewRowTyped();

            FMainDS.PPartnerInterest.Rows.Add(PartnerInterestRow);
            PartnerInterestRow.PartnerKey = FPartnerKey;

            PartnerInterestRow.InterestNumber = ReadInt32();
            PartnerInterestRow.FieldKey = ReadInt64();
            PartnerInterestRow.Country = ReadString();
            PartnerInterestRow.Interest = ReadString();
            string Category = ReadString();
            PartnerInterestRow.Level = ReadInt32();
            PartnerInterestRow.Comment = ReadString();

            // TODO: PInterest, PInterestCategory
        }

        private void ImportVision()
        {
            PmPersonVisionRow PersonVisionRow = FMainDS.PmPersonVision.NewRowTyped();

            FMainDS.PmPersonVision.Rows.Add(PersonVisionRow);
            PersonVisionRow.PartnerKey = FPartnerKey;

            PersonVisionRow.VisionAreaName = ReadString();
            PersonVisionRow.VisionLevel = ReadInt32();
            PersonVisionRow.VisionComment = ReadString();
        }

        private void ImportUnitAbility()
        {
            UmUnitAbilityRow UnitAbilityRow = FMainDS.UmUnitAbility.NewRowTyped();

            FMainDS.UmUnitAbility.Rows.Add(UnitAbilityRow);
            UnitAbilityRow.PartnerKey = FPartnerKey;

            UnitAbilityRow.AbilityAreaName = ReadString();
            UnitAbilityRow.AbilityLevel = ReadInt32();
            UnitAbilityRow.YearsOfExperience = ReadInt32();
        }

        private void ImportUnitCosts()
        {
            UmUnitCostRow UnitCostRow = FMainDS.UmUnitCost.NewRowTyped();

            FMainDS.UmUnitCost.Rows.Add(UnitCostRow);
            UnitCostRow.PartnerKey = FPartnerKey;

            UnitCostRow.ValidFromDate = ReadDate();
            UnitCostRow.ChargePeriod = ReadString();
            UnitCostRow.CoupleJoiningChargeIntl = ReadDecimal();
            UnitCostRow.AdultJoiningChargeIntl = ReadDecimal();
            UnitCostRow.ChildJoiningChargeIntl = ReadDecimal();
            UnitCostRow.CoupleCostsPeriodIntl = ReadDecimal();
            UnitCostRow.SingleCostsPeriodIntl = ReadDecimal();
            UnitCostRow.Child1CostsPeriodIntl = ReadDecimal();
            UnitCostRow.Child2CostsPeriodIntl = ReadDecimal();
            UnitCostRow.Child3CostsPeriodIntl = ReadDecimal();
        }

        private void ImportJob()
        {
            // Job Assignments are not being imported into the database anymore from 2.1.0 onwards.

            ReadNullableDate();
            ReadNullableDate();
            ReadString();
            ReadString();
            ReadBoolean();
            ReadInt64();
            ReadString();
            ReadString();
            ReadNullableDate();
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
            ReadString();
            ReadString();
            ReadInt32();
            ReadString();
            ReadInt32();
        }

        private void ImportUnitLanguage()
        {
            UmUnitLanguageRow UnitLanguageRow = FMainDS.UmUnitLanguage.NewRowTyped();

            FMainDS.UmUnitLanguage.Rows.Add(UnitLanguageRow);
            UnitLanguageRow.PartnerKey = FPartnerKey;

            UnitLanguageRow.LanguageCode = ReadString();
            UnitLanguageRow.LanguageLevel = ReadInt32();
            UnitLanguageRow.YearsOfExperience = ReadInt32();
            UnitLanguageRow.UnitLanguageReq = ReadString();
            UnitLanguageRow.UnitLangComment = ReadString();

            // TODO p_language
            // TODO pt_language_level
        }

        private void ImportUnitStructure()
        {
            UmUnitStructureRow UnitStructureRow = FMainDS.UmUnitStructure.NewRowTyped();

            FMainDS.UmUnitStructure.Rows.Add(UnitStructureRow);
            UnitStructureRow.ChildUnitKey = FPartnerKey;
            UnitStructureRow.ParentUnitKey = ReadInt64();
        }

        private void ImportUnitVision()
        {
            UmUnitVisionRow UnitVisionRow = FMainDS.UmUnitVision.NewRowTyped();

            FMainDS.UmUnitVision.Rows.Add(UnitVisionRow);
            UnitVisionRow.PartnerKey = FPartnerKey;

            UnitVisionRow.VisionAreaName = ReadString();
            UnitVisionRow.VisionLevel = ReadInt32();

            // TODO pt_vision_area
            // TODO pt_vision_level
        }

        private void ImportBuilding()
        {
            PcBuildingRow BuildingRow = FMainDS.PcBuilding.NewRowTyped();

            FMainDS.PcBuilding.Rows.Add(BuildingRow);
            BuildingRow.VenueKey = FPartnerKey;

            BuildingRow.BuildingCode = ReadString();
            BuildingRow.BuildingDesc = ReadString();
        }

        private void ImportRoom()
        {
            PcRoomRow RoomRow = FMainDS.PcRoom.NewRowTyped();

            FMainDS.PcRoom.Rows.Add(RoomRow);
            RoomRow.VenueKey = FPartnerKey;

            RoomRow.BuildingCode = ReadString();
            RoomRow.RoomNumber = ReadString();
            RoomRow.Beds = ReadInt32();
            RoomRow.BedCharge = ReadDecimal();
            RoomRow.BedCost = ReadDecimal();
            RoomRow.MaxOccupancy = ReadInt32();
            RoomRow.GenderPreference = ReadString();
        }

        private void ImportOptionalDetails(PPartnerRow APartnerRow)
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
                    ImportComment(APartnerRow);
                }
                else if (KeyWord == "COMMENTSEQ")
                {
                    ImportCommentSeq();
                }
                else if (KeyWord == "COMMIT")
                {
                    ImportCommitment();
                }
                else if (KeyWord == "JOB")
                {
                    ImportJob();
                }
                else if (KeyWord == "LANGUAGE")
                {
                    ImportLanguage();
                }
                else if (KeyWord == "PREVEXP")
                {
                    ImportPreviousExperience();
                }
                else if (KeyWord == "PASSPORT")
                {
                    ImportPassport();
                }
                else if (KeyWord == "PERSDOCUMENT")
                {
                    ImportPersonalDocument();
                }
                else if (KeyWord == "PERSONAL")
                {
                    ImportPersonalData();
                }
                else if (KeyWord == "PROFESN")
                {
                    ImportProfessionalData();
                }
                else if (KeyWord == "PROGREP")
                {
                    ImportPersonEvaluation();
                }
                else if (KeyWord == "SPECNEED")
                {
                    ImportSpecialNeeds();
                }
                else if (KeyWord == "TYPE")
                {
                    ImportPartnerType();
                }
                else if (KeyWord == "INTEREST")
                {
                    ImportInterest();
                }
                else if (KeyWord == "VISION")
                {
                    ImportVision();
                }
                else if (KeyWord == "U-ABILITY")
                {
                    ImportUnitAbility();
                }
                else if (KeyWord == "U-COSTS")
                {
                    ImportUnitCosts();
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
                    ImportUnitLanguage();
                }
                else if (KeyWord == "U-STRUCT")
                {
                    ImportUnitStructure();
                }
                else if (KeyWord == "U-VISION")
                {
                    ImportUnitVision();
                }
                else if (KeyWord == "V-BUILDING")
                {
                    ImportBuilding();
                }
                else if (KeyWord == "V-ROOM")
                {
                    ImportRoom();
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

            string PetraVersion = ReadString();
            Int64 SiteKey = ReadInt64();
            Int32 SubVersion = ReadInt32();

            while (CheckForKeyword("PARTNER"))
            {
                PPartnerRow PartnerRow = ImportPartner();

                FPartnerKey = PartnerRow.PartnerKey;

                ImportPartnerClassSpecific(PartnerRow.PartnerClass);

                ImportLocation();

                ImportOptionalDetails(PartnerRow);
            }

            return FMainDS;
        }
    }
}