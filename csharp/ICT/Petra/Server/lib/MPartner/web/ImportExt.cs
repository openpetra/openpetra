//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Text;
using System.Data;
using Ict.Common.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

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
        private bool FIgnorePartner = false;

        private PPartnerRow ImportPartner()
        {
            PPartnerRow PartnerRow = FMainDS.PPartner.NewRowTyped();

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

            // it seems, these values are not part of the ext files that I have seen
            //PartnerRow.NoSolicitations = ReadBoolean();
            //PartnerRow.AnonymousDonor = ReadBoolean();

            if (PartnerRow.AcquisitionCode.Length == 0)
            {
                PartnerRow.AcquisitionCode = MPartnerConstants.ACQUISITIONCODE_APPLICANT;
            }

            // check if such a partner (most likely family partner has already been loaded)
            FMainDS.PPartner.DefaultView.RowFilter = String.Format("{0} = '{1}'", PPartnerTable.GetPartnerKeyDBName(), PartnerRow.PartnerKey);
            FIgnorePartner = FMainDS.PPartner.DefaultView.Count != 0;
            FMainDS.PPartner.DefaultView.RowFilter = String.Empty;

            if (!FIgnorePartner)
            {
                FMainDS.PPartner.Rows.Add(PartnerRow);
            }

            return PartnerRow;
        }

        private void ImportPartnerClassSpecific(string APartnerClass)
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
                FMainDS.PChurch.Rows.Add(ChurchRow);
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
                }
                catch (Exception)
                {
                    FamilyRow.SetFieldKeyNull();
                }

                FamilyRow.MaritalStatus = ReadString();

                FamilyRow.MaritalStatusSince = ReadNullableDate();
                FamilyRow.MaritalStatusComment = ReadString();

                if (!FIgnorePartner)
                {
                    FMainDS.PFamily.Rows.Add(FamilyRow);
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
                Int32? BelieverSinceYear = ReadNullableInt32();

                if (BelieverSinceYear.HasValue)
                {
                    PersonRow.BelieverSinceYear = BelieverSinceYear.Value;
                }

                PersonRow.BelieverSinceComment = ReadString();
                PersonRow.OccupationCode = ReadString();
                Int64? FieldKey = ReadNullableInt64();

                if (FieldKey.HasValue)
                {
                    PersonRow.FieldKey = FieldKey.Value;
                }

                PersonRow.FamilyKey = ReadInt64();
                PersonRow.FamilyId = ReadInt32();
                FMainDS.PPerson.Rows.Add(PersonRow);

                if (TAppSettingsManager.GetValue("AllowCreationPersonRecords", "false", false).ToLower() != "true")
                {
                    throw new Exception(
                        "We are currently not supporting import of PERSON records, until we have resolved the issues with household/family. " +
                        "Please add configuration parameter AllowCreationPersonRecords with value true if you want to use PERSON records");
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
                FMainDS.POrganisation.Rows.Add(OrganisationRow);
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                PUnitRow UnitRow = FMainDS.PUnit.NewRowTyped();
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
                FMainDS.PUnit.Rows.Add(UnitRow);
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                PVenueRow VenueRow = FMainDS.PVenue.NewRowTyped();
                VenueRow.PartnerKey = FPartnerKey;
                VenueRow.VenueName = ReadString();
                VenueRow.VenueCode = ReadString();
                VenueRow.CurrencyCode = ReadString();
                VenueRow.ContactPartnerKey = ReadInt64();
                FMainDS.PVenue.Rows.Add(VenueRow);
            }
            else if (APartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                PBankRow BankRow = FMainDS.PBank.NewRowTyped();
                BankRow.PartnerKey = FPartnerKey;
                FMainDS.PBank.Rows.Add(BankRow);
            }
        }

        private void ImportLocation()
        {
            PLocationRow LocationRow = FMainDS.PLocation.NewRowTyped();

            LocationRow.LocationKey = FCountLocationKeys--;
            LocationRow.SiteKey = ReadInt64();
            LocationRow.Locality = ReadString();
            LocationRow.StreetName = ReadString();
            LocationRow.Address3 = ReadString();
            LocationRow.City = ReadString();
            LocationRow.County = ReadString();
            LocationRow.PostalCode = ReadString();
            LocationRow.CountryCode = ReadString();

            FMainDS.PLocation.Rows.Add(LocationRow);

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

            FMainDS.PPartnerLocation.Rows.Add(PartnerLocationRow);
        }

        private void ImportAbility()
        {
            PmPersonAbilityRow PersonAbilityRow = FMainDS.PmPersonAbility.NewRowTyped();

            PersonAbilityRow.PartnerKey = FPartnerKey;

            PersonAbilityRow.AbilityAreaName = ReadString();
            PersonAbilityRow.AbilityLevel = ReadInt32();
            PersonAbilityRow.YearsOfExperience = ReadInt32();
            PersonAbilityRow.BringingInstrument = ReadBoolean();
            PersonAbilityRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonAbilityRow.Comment = ReadString();

            FMainDS.PmPersonAbility.Rows.Add(PersonAbilityRow);
        }

        private void ReadShortApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmShortTermApplicationRow ShortTermApplicationRow = FMainDS.PmShortTermApplication.NewRowTyped();

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

            FMainDS.PmShortTermApplication.Rows.Add(ShortTermApplicationRow);
        }

        private void ReadLongApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmYearProgramApplicationRow YearProgramApplicationRow = FMainDS.PmYearProgramApplication.NewRowTyped();

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

            FMainDS.PmYearProgramApplication.Rows.Add(YearProgramApplicationRow);
        }

        private void ReadApplicationForm(PmGeneralApplicationRow AGeneralApplicationRow)
        {
            PmApplicationFormsRow ApplicationFormRow = FMainDS.PmApplicationForms.NewRowTyped();

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

            FMainDS.PmApplicationForms.Rows.Add(ApplicationFormRow);
        }

        private void ImportApplication()
        {
            PtApplicationTypeRow ApplicationTypeRow = FMainDS.PtApplicationType.NewRowTyped();

            ApplicationTypeRow.AppFormType = ReadString();
            ApplicationTypeRow.AppTypeName = ReadString();
            ApplicationTypeRow.AppTypeDescr = ReadString();

            FMainDS.PtApplicationType.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                PtApplicationTypeTable.GetAppTypeNameDBName(), ApplicationTypeRow.AppTypeName);

            if (FMainDS.PtApplicationType.DefaultView.Count == 0)
            {
                FMainDS.PtApplicationType.Rows.Add(ApplicationTypeRow);
            }

            FMainDS.PtApplicationType.DefaultView.RowFilter = String.Empty;


            PmGeneralApplicationRow GeneralApplicationRow = FMainDS.PmGeneralApplication.NewRowTyped();

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

            Int64? GenAppPossSrvUnitKey = ReadNullableInt64();

            if (GenAppPossSrvUnitKey.HasValue)
            {
                GeneralApplicationRow.GenAppPossSrvUnitKey = GenAppPossSrvUnitKey.Value;
            }

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

            FMainDS.PmGeneralApplication.Rows.Add(GeneralApplicationRow);

            if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
            {
                ReadShortApplicationForm(GeneralApplicationRow);
            }
            else if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
            {
                ReadLongApplicationForm(GeneralApplicationRow);
            }

            string KeyWord = ReadString();

            while (KeyWord == "APPL-FORM")
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

            PartnerCommentRow.PartnerKey = FPartnerKey;

            PartnerCommentRow.Sequence = ReadInt32();
            PartnerCommentRow.Comment = ReadString();

            FMainDS.PPartnerComment.Rows.Add(PartnerCommentRow);
        }

        private void ImportCommitment()
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
            StaffDataRow.ReceivingFieldOffice = ReadInt64();
            StaffDataRow.JobTitle = ReadString();
            StaffDataRow.StaffDataComments = ReadString();

            FMainDS.PmStaffData.Rows.Add(StaffDataRow);
        }

        private void ImportLanguage()
        {
            PmPersonLanguageRow PersonLanguageRow = FMainDS.PmPersonLanguage.NewRowTyped();

            PersonLanguageRow.PartnerKey = FPartnerKey;

            PersonLanguageRow.LanguageCode = ReadString();
            PersonLanguageRow.WillingToTranslate = ReadBoolean();
            PersonLanguageRow.TranslateInto = ReadBoolean();
            PersonLanguageRow.TranslateOutOf = ReadBoolean();
            PersonLanguageRow.YearsOfExperience = ReadInt32();
            PersonLanguageRow.LanguageLevel = ReadInt32();
            PersonLanguageRow.YearsOfExperienceAsOf = ReadNullableDate();
            PersonLanguageRow.Comment = ReadString();

            FMainDS.PmPersonLanguage.Rows.Add(PersonLanguageRow);
        }

        private void ImportPreviousExperience(TFileVersionInfo APetraVersion)
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

            FMainDS.PmPastExperience.Rows.Add(PastExperienceRow);
        }

        private void ImportPassport()
        {
            PmPassportDetailsRow PassportDetailsRow = FMainDS.PmPassportDetails.NewRowTyped();

            PassportDetailsRow.PartnerKey = FPartnerKey;

            PassportDetailsRow.PassportNumber = ReadString();
            PassportDetailsRow.CountryOfIssue = ReadString();
            PassportDetailsRow.DateOfExpiration = ReadNullableDate();
            PassportDetailsRow.DateOfIssue = ReadNullableDate();
            PassportDetailsRow.FullPassportName = ReadString();
            PassportDetailsRow.PassportNationalityCode = ReadString();
            PassportDetailsRow.PassportDetailsType = ReadString();
            PassportDetailsRow.PassportDob = ReadNullableDate();
            PassportDetailsRow.PlaceOfBirth = ReadString();
            PassportDetailsRow.PlaceOfIssue = ReadString();

            FMainDS.PmPassportDetails.Rows.Add(PassportDetailsRow);
        }

        private void ImportPersonalDocument()
        {
            PmDocumentRow DocumentRow = FMainDS.PmDocument.NewRowTyped();

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

            FMainDS.PmDocument.Rows.Add(DocumentRow);

            // TODO: PmDocumentType, PmDocumentCategory
        }

        private void ImportPersonalData()
        {
            PmPersonalDataRow PersonalDataRow = FMainDS.PmPersonalData.NewRowTyped();

            PersonalDataRow.PartnerKey = FPartnerKey;

            PersonalDataRow.DriverStatus = ReadString();
            PersonalDataRow.GenDriverLicense = ReadBoolean();
            PersonalDataRow.DrivingLicenseNumber = ReadString();
            PersonalDataRow.InternalDriverLicense = ReadBoolean();

            FMainDS.PmPersonalData.Rows.Add(PersonalDataRow);

            // TODO: PtDriverStatus
        }

        private void ImportProfessionalData()
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

            FMainDS.PmPersonQualification.Rows.Add(PersonQualificationRow);
        }

        private void ImportPersonEvaluation()
        {
            PmPersonEvaluationRow PersonEvaluationRow = FMainDS.PmPersonEvaluation.NewRowTyped();

            PersonEvaluationRow.PartnerKey = FPartnerKey;

            PersonEvaluationRow.EvaluationDate = ReadDate();
            PersonEvaluationRow.Evaluator = ReadString();
            PersonEvaluationRow.EvaluationType = ReadString();
            PersonEvaluationRow.NextEvaluationDate = ReadNullableDate();
            PersonEvaluationRow.EvaluationComments = ReadString();
            PersonEvaluationRow.PersonEvalAction = ReadString();

            FMainDS.PmPersonEvaluation.Rows.Add(PersonEvaluationRow);
        }

        private void ImportSpecialNeeds()
        {
            PmSpecialNeedRow SpecialNeedRow = FMainDS.PmSpecialNeed.NewRowTyped();

            SpecialNeedRow.PartnerKey = FPartnerKey;

            SpecialNeedRow.DateCreated = ReadNullableDate();
            SpecialNeedRow.ContactHomeOffice = ReadBoolean();
            SpecialNeedRow.VegetarianFlag = ReadBoolean();
            SpecialNeedRow.DietaryComment = ReadString();
            SpecialNeedRow.MedicalComment = ReadString();
            SpecialNeedRow.OtherSpecialNeed = ReadString();

            FMainDS.PmSpecialNeed.Rows.Add(SpecialNeedRow);
        }

        private void ImportPartnerType()
        {
            PPartnerTypeRow PartnerTypeRow = FMainDS.PPartnerType.NewRowTyped();

            PartnerTypeRow.PartnerKey = FPartnerKey;

            string s = ReadString();

            PartnerTypeRow.TypeCode = s;
            PartnerTypeRow.ValidFrom = ReadNullableDate();
            PartnerTypeRow.ValidUntil = ReadNullableDate();

            if (!FIgnorePartner)
            {
                FMainDS.PPartnerType.Rows.Add(PartnerTypeRow);
            }
        }

        private void ImportInterest()
        {
            PPartnerInterestRow PartnerInterestRow = FMainDS.PPartnerInterest.NewRowTyped();

            PartnerInterestRow.PartnerKey = FPartnerKey;

            PartnerInterestRow.InterestNumber = ReadInt32();
            PartnerInterestRow.FieldKey = ReadInt64();
            PartnerInterestRow.Country = ReadString();
            PartnerInterestRow.Interest = ReadString();
            string Category = ReadString();
            PartnerInterestRow.Level = ReadInt32();
            PartnerInterestRow.Comment = ReadString();

            FMainDS.PPartnerInterest.Rows.Add(PartnerInterestRow);

            // TODO: PInterest, PInterestCategory
        }

        private void ImportVision()
        {
            PmPersonVisionRow PersonVisionRow = FMainDS.PmPersonVision.NewRowTyped();

            PersonVisionRow.PartnerKey = FPartnerKey;

            PersonVisionRow.VisionAreaName = ReadString();
            PersonVisionRow.VisionLevel = ReadInt32();
            PersonVisionRow.VisionComment = ReadString();

            FMainDS.PmPersonVision.Rows.Add(PersonVisionRow);
        }

        private void ImportUnitAbility()
        {
            UmUnitAbilityRow UnitAbilityRow = FMainDS.UmUnitAbility.NewRowTyped();

            UnitAbilityRow.PartnerKey = FPartnerKey;

            UnitAbilityRow.AbilityAreaName = ReadString();
            UnitAbilityRow.AbilityLevel = ReadInt32();
            UnitAbilityRow.YearsOfExperience = ReadInt32();

            FMainDS.UmUnitAbility.Rows.Add(UnitAbilityRow);
        }

        private void ImportUnitCosts()
        {
            UmUnitCostRow UnitCostRow = FMainDS.UmUnitCost.NewRowTyped();

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

            FMainDS.UmUnitCost.Rows.Add(UnitCostRow);
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
            ReadInt64();
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

            UnitLanguageRow.PartnerKey = FPartnerKey;

            UnitLanguageRow.LanguageCode = ReadString();
            UnitLanguageRow.LanguageLevel = ReadInt32();
            UnitLanguageRow.YearsOfExperience = ReadInt32();
            UnitLanguageRow.UnitLanguageReq = ReadString();
            UnitLanguageRow.UnitLangComment = ReadString();

            FMainDS.UmUnitLanguage.Rows.Add(UnitLanguageRow);

            // TODO p_language
            // TODO pt_language_level
        }

        private void ImportUnitStructure()
        {
            UmUnitStructureRow UnitStructureRow = FMainDS.UmUnitStructure.NewRowTyped();

            UnitStructureRow.ChildUnitKey = FPartnerKey;
            UnitStructureRow.ParentUnitKey = ReadInt64();

            FMainDS.UmUnitStructure.Rows.Add(UnitStructureRow);
        }

        private void ImportUnitVision()
        {
            UmUnitVisionRow UnitVisionRow = FMainDS.UmUnitVision.NewRowTyped();

            UnitVisionRow.PartnerKey = FPartnerKey;

            UnitVisionRow.VisionAreaName = ReadString();
            UnitVisionRow.VisionLevel = ReadInt32();

            FMainDS.UmUnitVision.Rows.Add(UnitVisionRow);

            // TODO pt_vision_area
            // TODO pt_vision_level
        }

        private void ImportBuilding()
        {
            PcBuildingRow BuildingRow = FMainDS.PcBuilding.NewRowTyped();

            BuildingRow.VenueKey = FPartnerKey;

            BuildingRow.BuildingCode = ReadString();
            BuildingRow.BuildingDesc = ReadString();

            FMainDS.PcBuilding.Rows.Add(BuildingRow);
        }

        private void ImportRoom()
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

            FMainDS.PcRoom.Rows.Add(RoomRow);
        }

        private void ImportOptionalDetails(PPartnerRow APartnerRow, TFileVersionInfo APetraVersion)
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
                    ImportPreviousExperience(APetraVersion);
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

                KeyWord = ReadString();
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

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            PtApplicationTypeAccess.LoadAll(FMainDS, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();

            InitReading(ALinesToImport);

            TFileVersionInfo PetraVersion = new TFileVersionInfo(ReadString());
            Int64 SiteKey = ReadInt64();
            Int32 SubVersion = ReadInt32();

            try
            {
                while (CheckForKeyword("PARTNER"))
                {
                    PPartnerRow PartnerRow = ImportPartner();

                    FPartnerKey = PartnerRow.PartnerKey;

                    ImportPartnerClassSpecific(PartnerRow.PartnerClass);

                    ImportLocation();

                    ImportOptionalDetails(PartnerRow, PetraVersion);
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message + " in line " + (CurrentLineCounter + 1).ToString());
                TLogging.Log(CurrentLine);
                throw;
            }

            return FMainDS;
        }
    }
}