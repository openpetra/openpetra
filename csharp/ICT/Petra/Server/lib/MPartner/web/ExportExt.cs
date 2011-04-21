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
    /// Export all data of a partner
    /// </summary>
    public class TPartnerFileExport : TImportExportTextFile
    {
        private void WriteLocation(PLocationRow ALocationRow, PPartnerLocationRow APartnerLocationRow)
        {
            Write(ALocationRow.SiteKey);
            Write(ALocationRow.Locality);
            Write(ALocationRow.StreetName);
            Write(ALocationRow.Address3);
            WriteLine();
            Write(ALocationRow.City);
            Write(ALocationRow.County);
            Write(ALocationRow.PostalCode);
            Write(ALocationRow.CountryCode);
            WriteLine();

            Write(APartnerLocationRow.DateEffective);
            Write(APartnerLocationRow.DateGoodUntil);
            Write(APartnerLocationRow.LocationType);
            Write(APartnerLocationRow.SendMail);
            Write(APartnerLocationRow.EmailAddress);
            Write(APartnerLocationRow.TelephoneNumber);
            Write(APartnerLocationRow.Extension);
            Write(APartnerLocationRow.FaxNumber);
            Write(APartnerLocationRow.FaxExtension);
            WriteLine();
        }

        private void WriteShortApplicationForm(PartnerImportExportTDS AMainDS, PmGeneralApplicationRow AGeneralApplicationRow)
        {
            AMainDS.PmShortTermApplication.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}={3} and {4}={5}",
                    PmShortTermApplicationTable.GetPartnerKeyDBName(),
                    AGeneralApplicationRow.PartnerKey,
                    PmShortTermApplicationTable.GetApplicationKeyDBName(),
                    AGeneralApplicationRow.ApplicationKey,
                    PmShortTermApplicationTable.GetRegistrationOfficeDBName(),
                    AGeneralApplicationRow.RegistrationOffice);

            if (AMainDS.PmShortTermApplication.DefaultView.Count > 0)
            {
                PmShortTermApplicationRow ShortTermApplicationRow = (PmShortTermApplicationRow)AMainDS.PmShortTermApplication.DefaultView[0].Row;
                Write(ShortTermApplicationRow.ConfirmedOptionCode);
                Write(ShortTermApplicationRow.Option1Code);
                Write(ShortTermApplicationRow.Option2Code);
                Write(ShortTermApplicationRow.FromCongTravelInfo);
                WriteLine();
                Write(ShortTermApplicationRow.Arrival);
                Write(ShortTermApplicationRow.ArrivalHour);
                Write(ShortTermApplicationRow.ArrivalMinute);
                Write(ShortTermApplicationRow.Departure);
                Write(ShortTermApplicationRow.DepartureHour);
                Write(ShortTermApplicationRow.DepartureMinute);
                WriteLine();
                Write(ShortTermApplicationRow.StApplicationHoldReason);
                Write(ShortTermApplicationRow.StApplicationOnHold);
                Write(ShortTermApplicationRow.StBasicDeleteFlag);
                Write(ShortTermApplicationRow.StBookingFeeReceived);
                Write(ShortTermApplicationRow.StXyzTbdOnlyFlag);
                Write(ShortTermApplicationRow.StCmpgnSpecialCost);
                Write(ShortTermApplicationRow.StCngrssSpecialCost);
                WriteLine();
                Write(ShortTermApplicationRow.StComment);
                WriteLine();
                Write(ShortTermApplicationRow.StConfirmedOption);
                Write(ShortTermApplicationRow.StCongressCode);
                Write(ShortTermApplicationRow.StCongressLanguage);
                Write(ShortTermApplicationRow.StCountryPref);
                Write(ShortTermApplicationRow.StCurrentField);
                Write(ShortTermApplicationRow.XyzTbdRole);
                WriteLine();
                Write(ShortTermApplicationRow.StFgCode);
                Write(ShortTermApplicationRow.StFgLeader);
                Write(ShortTermApplicationRow.StFieldCharged);
                Write(ShortTermApplicationRow.StLeadershipRating);
                Write(ShortTermApplicationRow.StOption1);
                Write(ShortTermApplicationRow.StOption2);
                WriteLine();
                Write(ShortTermApplicationRow.StPartyContact);
                Write(ShortTermApplicationRow.StPartyTogether);
                Write(ShortTermApplicationRow.StPreCongressCode);
                Write(ShortTermApplicationRow.StProgramFeeReceived);
                Write(ShortTermApplicationRow.StRecruitEfforts);
                Write(ShortTermApplicationRow.StScholarshipAmount);
                Write(ShortTermApplicationRow.StScholarshipApprovedBy);
                Write(ShortTermApplicationRow.StScholarshipPeriod);
                Write(ShortTermApplicationRow.StScholarshipReviewDate);
                WriteLine();
                Write(ShortTermApplicationRow.StSpecialApplicant);
                Write(ShortTermApplicationRow.StActivityPref);
                Write(ShortTermApplicationRow.ToCongTravelInfo);
                Write(ShortTermApplicationRow.ArrivalPointCode);
                Write(ShortTermApplicationRow.DeparturePointCode);
                Write(ShortTermApplicationRow.TravelTypeFromCongCode);
                Write(ShortTermApplicationRow.TravelTypeToCongCode);
                WriteLine();
                Write(ShortTermApplicationRow.ContactNumber);
                Write(ShortTermApplicationRow.ArrivalDetailsStatus);
                Write(ShortTermApplicationRow.ArrivalTransportNeeded);
                Write(ShortTermApplicationRow.ArrivalExp);
                Write(ShortTermApplicationRow.ArrivalExpHour);
                Write(ShortTermApplicationRow.ArrivalExpMinute);
                Write(ShortTermApplicationRow.ArrivalComments);
                Write(ShortTermApplicationRow.TransportInterest);
                WriteLine();
                Write(ShortTermApplicationRow.DepartureDetailsStatus);
                Write(ShortTermApplicationRow.DepartureTransportNeeded);
                Write(ShortTermApplicationRow.DepartureExp);
                Write(ShortTermApplicationRow.DepartureExpHour);
                Write(ShortTermApplicationRow.DepartureExpMinute);
                Write(ShortTermApplicationRow.DepartureComments);
                WriteLine();
            }
            else
            {
                Write("");                 // ShortTermApplicationRow.ConfirmedOptionCode
                Write("");                 // ShortTermApplicationRow.Option1Code
                Write("");                 // ShortTermApplicationRow.Option2Code
                Write("");                 // ShortTermApplicationRow.FromCongTravelInfo
                WriteLine();
                Write("?");                 // ShortTermApplicationRow.Arrival
                Write("0");                 // ShortTermApplicationRow.ArrivalHour
                Write("0");                 // ShortTermApplicationRow.ArrivalMinute
                Write("?");                 // ShortTermApplicationRow.Departure
                Write("0");                 // ShortTermApplicationRow.DepartureHour
                Write("0");                 // ShortTermApplicationRow.DepartureMinute
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StApplicationHoldReason
                Write(true);                 // ShortTermApplicationRow.StApplicationOnHold
                Write(false);                 // ShortTermApplicationRow.StBasicDeleteFlag
                Write(false);                 // ShortTermApplicationRow.StBookingFeeReceived
                Write(false);                 // ShortTermApplicationRow.StXyzTbdOnlyFlag
                Write(0);                 // ShortTermApplicationRow.StCmpgnSpecialCost
                Write(0);                 // ShortTermApplicationRow.StCngrssSpecialCost
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StComment
                WriteLine();
                Write(0);                 // ShortTermApplicationRow.StConfirmedOption
                Write("");                 // ShortTermApplicationRow.StCongressCode
                Write("");                 // ShortTermApplicationRow.StCongressLanguage
                Write("");                 // ShortTermApplicationRow.StCountryPref
                Write(0);                 // ShortTermApplicationRow.StCurrentField
                Write("");                 // ShortTermApplicationRow.XyzTbdRole
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StFgCode
                Write(false);                 // ShortTermApplicationRow.StFgLeader
                Write(0);                 // ShortTermApplicationRow.StFieldCharged
                Write("");                 // ShortTermApplicationRow.StLeadershipRating
                Write(0);                 // ShortTermApplicationRow.StOption1
                Write(0);                 // ShortTermApplicationRow.StOption2
                WriteLine();
                Write(0);                 // ShortTermApplicationRow.StPartyContact
                Write("");                 // ShortTermApplicationRow.StPartyTogether
                Write("");                 // ShortTermApplicationRow.StPreCongressCode
                Write(false);                 // ShortTermApplicationRow.StProgramFeeReceived
                Write("");                 // ShortTermApplicationRow.StRecruitEfforts
                Write(0);                 // ShortTermApplicationRow.StScholarshipAmount
                Write("");                 // ShortTermApplicationRow.StScholarshipApprovedBy
                Write("");                 // ShortTermApplicationRow.StScholarshipPeriod
                Write("?");                 // ShortTermApplicationRow.StScholarshipReviewDate
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StSpecialApplicant
                Write("");                 // ShortTermApplicationRow.StActivityPref
                Write("");                 // ShortTermApplicationRow.ToCongTravelInfo
                Write("");                 // ShortTermApplicationRow.ArrivalPointCode
                Write("");                 // ShortTermApplicationRow.DeparturePointCode
                Write("");                 // ShortTermApplicationRow.TravelTypeFromCongCode
                Write("");                 // ShortTermApplicationRow.TravelTypeToCongCode
                WriteLine();
                Write("");                 // ShortTermApplicationRow.ContactNumber
                Write("");                 // ShortTermApplicationRow.ArrivalDetailsStatus
                Write(false);                 // ShortTermApplicationRow.ArrivalTransportNeeded
                Write("?");                 // ShortTermApplicationRow.ArrivalExp
                Write(0);                 // ShortTermApplicationRow.ArrivalExpHour
                Write(0);                 // ShortTermApplicationRow.ArrivalExpMinute
                Write("");                 // ShortTermApplicationRow.ArrivalComments
                Write(false);                 // ShortTermApplicationRow.TransportInterest
                WriteLine();
                Write("");                 // ShortTermApplicationRow.DepartureDetailsStatus
                Write(false);                 // ShortTermApplicationRow.DepartureTransportNeeded
                Write("?");                 // ShortTermApplicationRow.DepartureExp
                Write(0);                 // ShortTermApplicationRow.DepartureExpHour
                Write(0);                 // ShortTermApplicationRow.DepartureExpMinute
                Write("");                 // ShortTermApplicationRow.DepartureComments
                WriteLine();
            }
        }

        private void WriteLongApplicationForm(PartnerImportExportTDS AMainDS, PmGeneralApplicationRow AGeneralApplicationRow)
        {
            // TODO: does filter work for the date?
            AMainDS.PmYearProgramApplication.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}='{3}' and {4}={5}",
                    PmYearProgramApplicationTable.GetPartnerKeyDBName(),
                    AGeneralApplicationRow.PartnerKey,
                    PmYearProgramApplicationTable.GetYpBasicAppTypeDBName(),
                    AGeneralApplicationRow.AppTypeName,
                    PmYearProgramApplicationTable.GetYpAppDateDBName(),
                    AGeneralApplicationRow.GenAppDate);

            if (AMainDS.PmYearProgramApplication.DefaultView.Count > 0)
            {
                PmYearProgramApplicationRow YearProgramApplicationRow =
                    (PmYearProgramApplicationRow)AMainDS.PmYearProgramApplication.DefaultView[0].Row;
                Write(YearProgramApplicationRow.HoOrientConfBookingKey);
                Write(YearProgramApplicationRow.YpAgreedJoiningCharge);
                Write(YearProgramApplicationRow.YpAgreedSupportFigure);
                Write(YearProgramApplicationRow.YpAppFeeReceived);
                Write(YearProgramApplicationRow.YpBasicDeleteFlag);
                Write(YearProgramApplicationRow.YpJoiningConf);
                Write(YearProgramApplicationRow.StartOfCommitment);
                Write(YearProgramApplicationRow.EndOfCommitment);
                Write(YearProgramApplicationRow.IntendedComLengthMonths);
                Write(YearProgramApplicationRow.PositionName);
                Write(YearProgramApplicationRow.PositionScope);
                Write(YearProgramApplicationRow.AssistantTo);
                WriteLine();
                Write(YearProgramApplicationRow.YpScholarshipAthrizedBy);
                Write(YearProgramApplicationRow.YpScholarshipBeginDate);
                Write(YearProgramApplicationRow.YpScholarshipEndDate);
                Write(YearProgramApplicationRow.YpScholarship);
                Write(YearProgramApplicationRow.YpScholarshipPeriod);
                Write(YearProgramApplicationRow.YpScholarshipReviewDate);
                Write(YearProgramApplicationRow.YpSupportPeriod);
                WriteLine();
            }
            else
            {
                PmYearProgramApplicationRow YearProgramApplicationRow =
                    (PmYearProgramApplicationRow)AMainDS.PmYearProgramApplication.DefaultView[0].Row;
                Write("");                 // YearProgramApplicationRow.HoOrientConfBookingKey
                Write(0);                 // YearProgramApplicationRow.YpAgreedJoiningCharge
                Write(0);                 // YearProgramApplicationRow.YpAgreedSupportFigure
                Write(false);                 // YearProgramApplicationRow.YpAppFeeReceived
                Write(false);                 // YearProgramApplicationRow.YpBasicDeleteFlag
                Write(0);                 // YearProgramApplicationRow.YpJoiningConf
                Write("?");                 // YearProgramApplicationRow.StartOfCommitment
                Write("?");                 // YearProgramApplicationRow.EndOfCommitment
                Write(0);                 // YearProgramApplicationRow.IntendedComLengthMonths
                Write("");                 // YearProgramApplicationRow.PositionName
                Write("");                 // YearProgramApplicationRow.PositionScope
                Write(false);                 // YearProgramApplicationRow.AssistantTo
                WriteLine();
                Write("");                 // YearProgramApplicationRow.YpScholarshipAthrizedBy
                Write("?");                 // YearProgramApplicationRow.YpScholarshipBeginDate
                Write("?");                 // YearProgramApplicationRow.YpScholarshipEndDate
                Write(0);                 // YearProgramApplicationRow.YpScholarship
                Write("");                 // YearProgramApplicationRow.YpScholarshipPeriod
                Write("?");                 // YearProgramApplicationRow.YpScholarshipReviewDate
                Write("");                 // YearProgramApplicationRow.YpSupportPeriod
                WriteLine();
            }
        }

        private void WriteApplications(PartnerImportExportTDS AMainDS)
        {
            foreach (PmGeneralApplicationRow GeneralApplicationRow in AMainDS.PmGeneralApplication.Rows)
            {
                if (!GeneralApplicationRow.GenAppDeleteFlag)
                {
                    AMainDS.PtApplicationType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                        PtApplicationTypeTable.GetAppTypeNameDBName(),
                        GeneralApplicationRow.AppTypeName);

                    PtApplicationTypeRow ApplicationTypeRow = (PtApplicationTypeRow)AMainDS.PtApplicationType.DefaultView[0].Row;

                    Write("APPLCTN");
                    WriteLine();
                    Write(ApplicationTypeRow.AppFormType);
                    Write(ApplicationTypeRow.AppTypeName);
                    Write(ApplicationTypeRow.AppTypeDescr);
                    WriteLine();
                    Write(GeneralApplicationRow.GenAppDate);
                    Write(GeneralApplicationRow.OldLink);
                    WriteLine();
                    Write(GeneralApplicationRow.GenApplicantType);
                    Write(GeneralApplicationRow.GenApplicationHoldReason);
                    Write(GeneralApplicationRow.GenApplicationOnHold);
                    Write(GeneralApplicationRow.GenApplicationStatus);
                    Write(GeneralApplicationRow.GenAppCancelled);
                    Write(GeneralApplicationRow.GenAppCancelReason);
                    Write(GeneralApplicationRow.GenAppDeleteFlag);
                    Write(GeneralApplicationRow.Closed);
                    Write(GeneralApplicationRow.ClosedBy);
                    Write(GeneralApplicationRow.DateClosed);
                    WriteLine();
                    Write(GeneralApplicationRow.GenAppPossSrvUnitKey);
                    Write(GeneralApplicationRow.GenAppRecvgFldAccept);
                    Write(GeneralApplicationRow.GenAppSrvFldAccept);
                    Write(GeneralApplicationRow.GenAppSendFldAcceptDate);
                    Write(GeneralApplicationRow.GenAppSendFldAccept);
                    Write(GeneralApplicationRow.GenAppCurrencyCode);
                    Write(GeneralApplicationRow.PlacementPartnerKey);
                    WriteLine();
                    Write(GeneralApplicationRow.GenAppUpdate);
                    Write(GeneralApplicationRow.GenCancelledApp);
                    Write(GeneralApplicationRow.GenContact1);
                    Write(GeneralApplicationRow.GenContact2);
                    Write(GeneralApplicationRow.GenYearProgram);
                    Write(GeneralApplicationRow.ApplicationKey);
                    Write(GeneralApplicationRow.RegistrationOffice);
                    WriteMultiLine(GeneralApplicationRow.Comment);
                    WriteLine();

                    if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
                    {
                        WriteShortApplicationForm(AMainDS, GeneralApplicationRow);
                    }
                    else if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
                    {
                        WriteLongApplicationForm(AMainDS, GeneralApplicationRow);
                    }

                    AMainDS.PmApplicationForms.DefaultView.RowFilter =
                        String.Format("{0}={1} and {2}={3} and {4}={5}",
                            PmApplicationFormsTable.GetPartnerKeyDBName(),
                            GeneralApplicationRow.PartnerKey,
                            PmApplicationFormsTable.GetApplicationKeyDBName(),
                            GeneralApplicationRow.ApplicationKey,
                            PmApplicationFormsTable.GetRegistrationOfficeDBName(),
                            GeneralApplicationRow.RegistrationOffice);

                    foreach (DataRowView v in AMainDS.PmApplicationForms.DefaultView)
                    {
                        PmApplicationFormsRow ApplicationFormRow = (PmApplicationFormsRow)v.Row;
                        Write("APPL-FORM");
                        Write(ApplicationFormRow.FormName);
                        WriteLine();
                        Write(ApplicationFormRow.FormDeleteFlag);
                        Write(ApplicationFormRow.FormEdited);
                        Write(ApplicationFormRow.FormReceivedDate);
                        Write(ApplicationFormRow.FormReceived);
                        Write(ApplicationFormRow.FormSentDate);
                        Write(ApplicationFormRow.FormSent);
                        WriteLine();
                        Write(ApplicationFormRow.ReferencePartnerKey);
                        Write(ApplicationFormRow.Comment);
                        WriteLine();
                    }

                    Write("END");
                    Write("FORMS");
                    WriteLine();
                }
            }
        }

        private void WritePersonnelData(PartnerImportExportTDS AMainDS)
        {
            foreach (PmPersonalDataRow PersonalDataRow in AMainDS.PmPersonalData.Rows)
            {
                Write("PERSONAL");
                WriteLine();
                Write(PersonalDataRow.DriverStatus);
                Write(PersonalDataRow.GenDriverLicense);
                Write(PersonalDataRow.DrivingLicenseNumber);
                Write(PersonalDataRow.InternalDriverLicense);
                WriteLine();
            }

            foreach (PmPassportDetailsRow PassportDetailsRow in AMainDS.PmPassportDetails.Rows)
            {
                if (PassportDetailsRow.IsDateOfExpirationNull() || (PassportDetailsRow.DateOfExpiration < DateTime.Today))
                {
                    Write("PASSPORT");
                    WriteLine();
                    Write(PassportDetailsRow.PassportNumber);
                    WriteLine();
                    Write(PassportDetailsRow.MainPassport);
                    Write(PassportDetailsRow.CountryOfIssue);
                    Write(PassportDetailsRow.DateOfExpiration);
                    Write(PassportDetailsRow.DateOfIssue);
                    Write(PassportDetailsRow.FullPassportName);
                    Write(PassportDetailsRow.PassportNationalityCode);
                    Write(PassportDetailsRow.PassportDetailsType);
                    Write(PassportDetailsRow.PassportDob);
                    Write(PassportDetailsRow.PlaceOfBirth);
                    Write(PassportDetailsRow.PlaceOfIssue);
                    WriteLine();
                }
            }

            foreach (PmDocumentRow DocumentRow in AMainDS.PmDocument.Rows)
            {
                AMainDS.PmDocumentType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PmDocumentTypeTable.GetDocCodeDBName(),
                    DocumentRow.DocCode);
                Write("PERSDOCUMENT");
                WriteLine();
                Write(DocumentRow.SiteKey);
                Write(DocumentRow.DocumentKey);
                WriteLine();
                Write(DocumentRow.DocCode);
                Write(((PmDocumentTypeRow)AMainDS.PmDocumentType.DefaultView[0].Row).DocCategory);
                Write(DocumentRow.DocumentId);
                Write(DocumentRow.PlaceOfIssue);
                Write(DocumentRow.DateOfIssue);
                Write(DocumentRow.DateOfStart);
                Write(DocumentRow.DateOfExpiration);
                Write(DocumentRow.AssocDocId);
                Write(DocumentRow.ContactPartnerKey);
                WriteLine();
                Write(DocumentRow.DocComment);
                WriteLine();
            }

            foreach (PmPersonQualificationRow PersonQualificationRow in AMainDS.PmPersonQualification.Rows)
            {
                Write("PROFESN");
                WriteLine();
                Write(PersonQualificationRow.QualificationAreaName);
                WriteLine();
                Write(PersonQualificationRow.QualificationLevel);
                Write(PersonQualificationRow.InformalFlag);
                Write(PersonQualificationRow.YearsOfExperience);
                Write(PersonQualificationRow.YearsOfExperienceAsOf);
                Write(PersonQualificationRow.Comment);
                Write(PersonQualificationRow.QualificationDate);
                Write(PersonQualificationRow.QualificationExpiry);
                WriteLine();
            }

            foreach (PmSpecialNeedRow SpecialNeedRow in AMainDS.PmSpecialNeed.Rows)
            {
                Write("SPECNEED");
                WriteLine();
                Write(SpecialNeedRow.DateCreated);
                Write(SpecialNeedRow.ContactHomeOffice);
                Write(SpecialNeedRow.VegetarianFlag);
                WriteLine();
                Write(SpecialNeedRow.DietaryComment);
                WriteLine();
                Write(SpecialNeedRow.MedicalComment);
                WriteLine();
                Write(SpecialNeedRow.OtherSpecialNeed);
                WriteLine();
            }

            foreach (PmPastExperienceRow PastExperienceRow in AMainDS.PmPastExperience.Rows)
            {
                Write("PREVEXP");
                WriteLine();
                Write(PastExperienceRow.SiteKey);
                Write(PastExperienceRow.Key);
                Write(PastExperienceRow.PrevLocation);
                Write(PastExperienceRow.StartDate);
                Write(PastExperienceRow.EndDate);
                WriteLine();
                Write(PastExperienceRow.PrevWorkHere);
                Write(PastExperienceRow.PrevWork);
                Write(PastExperienceRow.OtherOrganisation);
                Write(PastExperienceRow.PrevRole);
                Write(PastExperienceRow.Category);
                WriteLine();
                Write(PastExperienceRow.PastExpComments);
                WriteLine();
            }

            foreach (PmPersonLanguageRow PersonLanguageRow in AMainDS.PmPersonLanguage.Rows)
            {
                Write("LANGUAGE");
                WriteLine();
                Write(PersonLanguageRow.LanguageCode);
                WriteLine();
                Write(PersonLanguageRow.WillingToTranslate);
                Write(PersonLanguageRow.TranslateInto);
                Write(PersonLanguageRow.TranslateOutOf);
                Write(PersonLanguageRow.YearsOfExperience);
                Write(PersonLanguageRow.LanguageLevel);
                Write(PersonLanguageRow.YearsOfExperienceAsOf);
                Write(PersonLanguageRow.Comment);
                WriteLine();
            }

            foreach (PmPersonAbilityRow PersonAbilityRow in AMainDS.PmPersonAbility.Rows)
            {
                Write("ABILITY");
                WriteLine();
                Write(PersonAbilityRow.AbilityAreaName);
                Write(PersonAbilityRow.AbilityLevel);
                WriteLine();
                Write(PersonAbilityRow.YearsOfExperience);
                Write(PersonAbilityRow.BringingInstrument);
                Write(PersonAbilityRow.YearsOfExperienceAsOf);
                Write(PersonAbilityRow.Comment);
                WriteLine();
            }

            foreach (PmPersonVisionRow PersonVisionRow in AMainDS.PmPersonVision.Rows)
            {
                Write("VISION");
                WriteLine();
                Write(PersonVisionRow.VisionAreaName);
                WriteLine();
                Write(PersonVisionRow.VisionLevel);
                Write(PersonVisionRow.VisionComment);
                WriteLine();
            }

            foreach (PmStaffDataRow StaffDataRow in AMainDS.PmStaffData.Rows)
            {
                Write("COMMIT");
                WriteLine();
                Write(StaffDataRow.SiteKey);
                Write(StaffDataRow.Key);
                Write(StaffDataRow.StartOfCommitment);
                Write(StaffDataRow.StartDateApprox);
                Write(StaffDataRow.EndOfCommitment);
                WriteLine();
                Write(StaffDataRow.StatusCode);
                Write(StaffDataRow.ReceivingField);
                Write(StaffDataRow.HomeOffice);
                Write(StaffDataRow.OfficeRecruitedBy);
                Write(StaffDataRow.ReceivingFieldOffice);
                Write(StaffDataRow.JobTitle);
                WriteLine();
                Write(StaffDataRow.StaffDataComments);
                WriteLine();
            }

            foreach (PmJobAssignmentRow JobAssignmentRow in AMainDS.PmJobAssignment.Rows)
            {
                Write("JOB");
                WriteLine();
                Write(JobAssignmentRow.FromDate);
                Write(JobAssignmentRow.ToDate);
                Write(JobAssignmentRow.PositionName);
                Write(JobAssignmentRow.PositionScope);
                Write(JobAssignmentRow.AssistantTo);
                Write(JobAssignmentRow.JobKey);
                Write(JobAssignmentRow.JobAssignmentKey);
                WriteLine();
                Write(JobAssignmentRow.UnitKey);
                Write(JobAssignmentRow.AssignmentTypeCode);
                Write(JobAssignmentRow.LeavingCode);
                Write(JobAssignmentRow.LeavingCodeUpdatedDate);
                WriteLine();
            }

            foreach (PmPersonEvaluationRow PersonEvaluationRow in AMainDS.PmPersonEvaluation.Rows)
            {
                Write("PROGREP");
                WriteLine();
                Write(PersonEvaluationRow.EvaluationDate);
                Write(PersonEvaluationRow.Evaluator);
                WriteLine();
                Write(PersonEvaluationRow.EvaluationType);
                Write(PersonEvaluationRow.NextEvaluationDate);
                WriteLine();
                Write(PersonEvaluationRow.EvaluationComments);
                WriteLine();
                Write(PersonEvaluationRow.PersonEvalAction);
                WriteLine();
            }

            WriteApplications(AMainDS);
        }

        private void WriteUnitData(PartnerImportExportTDS AMainDS)
        {
            foreach (UmUnitStructureRow UnitStructureRow in AMainDS.UmUnitStructure.Rows)
            {
                Write("U-STRUCT");
                WriteLine();
                Write(UnitStructureRow.ParentUnitKey);
                WriteLine();
            }

            foreach (UmUnitAbilityRow UnitAbilityRow in AMainDS.UmUnitAbility.Rows)
            {
                Write("U-ABILITY");
                WriteLine();
                Write(UnitAbilityRow.AbilityAreaName);
                Write(UnitAbilityRow.AbilityLevel);
                Write(UnitAbilityRow.YearsOfExperience);
                WriteLine();
            }

            foreach (UmUnitLanguageRow UnitLanguageRow in AMainDS.UmUnitLanguage.Rows)
            {
                Write("U-LANG");
                WriteLine();
                Write(UnitLanguageRow.LanguageCode);
                Write(UnitLanguageRow.LanguageLevel);
                Write(UnitLanguageRow.YearsOfExperience);
                Write(UnitLanguageRow.UnitLanguageReq);
                WriteLine();
                Write(UnitLanguageRow.UnitLangComment);
                WriteLine();
            }

            foreach (UmUnitVisionRow UnitVisionRow in AMainDS.UmUnitVision.Rows)
            {
                Write("U-VISION");
                WriteLine();
                Write(UnitVisionRow.VisionAreaName);
                Write(UnitVisionRow.VisionLevel);
                WriteLine();
            }

            AMainDS.UmUnitCost.DefaultView.Sort = UmUnitCostTable.GetValidFromDateDBName() + " desc";

            foreach (DataRowView v in AMainDS.UmUnitCost.DefaultView)
            {
                UmUnitCostRow UnitCostRow = (UmUnitCostRow)v.Row;

                // only export current and future costs
                if (UnitCostRow.ValidFromDate >= DateTime.Today)
                {
                    Write("U-COSTS");
                    WriteLine();
                    Write(UnitCostRow.ValidFromDate);
                    Write(UnitCostRow.ChargePeriod);
                    WriteLine();
                    // only export values in international currency
                    Write(UnitCostRow.CoupleJoiningChargeIntl);
                    Write(UnitCostRow.AdultJoiningChargeIntl);
                    Write(UnitCostRow.ChildJoiningChargeIntl);
                    Write(UnitCostRow.CoupleCostsPeriodIntl);
                    Write(UnitCostRow.SingleCostsPeriodIntl);
                    Write(UnitCostRow.Child1CostsPeriodIntl);
                    Write(UnitCostRow.Child2CostsPeriodIntl);
                    Write(UnitCostRow.Child3CostsPeriodIntl);
                    WriteLine();
                }
            }

            AMainDS.UmJob.DefaultView.Sort = UmJobTable.GetToDateDBName() + " desc";

            foreach (DataRowView v in AMainDS.UmJob.DefaultView)
            {
                UmJobRow JobRow = (UmJobRow)v.Row;

                // only export current and future jobs
                if (JobRow.ToDate >= DateTime.Today)
                {
                    Write("U-JOB");
                    WriteLine();
                    Write(JobRow.PositionName);
                    Write(JobRow.PositionScope);
                    Write(JobRow.JobKey);
                    WriteLine();
                    Write(JobRow.FromDate);
                    Write(JobRow.ToDate);
                    Write(JobRow.JobType);
                    Write(JobRow.CommitmentPeriod);
                    Write(JobRow.TrainingPeriod);
                    Write(JobRow.PartTimeFlag);
                    Write(JobRow.PreviousInternalExpReq);
                    Write(JobRow.JobPublicity);
                    Write(JobRow.PublicFlag);
                    Write(JobRow.Maximum);
                    Write(JobRow.Minimum);
                    Write(JobRow.Present);
                    Write(JobRow.PartTimers);
                    WriteLine();

                    foreach (UmJobRequirementRow JobRequirementRow in AMainDS.UmJobRequirement.Rows)
                    {
                        if ((JobRequirementRow.UnitKey == JobRow.UnitKey)
                            && (JobRequirementRow.PositionName == JobRow.PositionName))
                        {
                            Write("UJ-ABIL");
                            WriteLine();
                            Write(JobRequirementRow.PositionName);
                            Write(JobRequirementRow.PositionScope);
                            Write(JobRequirementRow.JobKey);
                            Write(JobRequirementRow.AbilityAreaName);
                            Write(JobRequirementRow.AbilityLevel);
                            Write(JobRequirementRow.YearsOfExperience);
                            WriteLine();
                        }
                    }

                    foreach (UmJobLanguageRow JobLanguageRow in AMainDS.UmJobLanguage.Rows)
                    {
                        if ((JobLanguageRow.UnitKey == JobRow.UnitKey)
                            && (JobLanguageRow.PositionName == JobRow.PositionName))
                        {
                            Write("UJ-LANG");
                            WriteLine();
                            Write(JobLanguageRow.PositionName);
                            Write(JobLanguageRow.PositionScope);
                            Write(JobLanguageRow.JobKey);
                            Write(JobLanguageRow.LanguageCode);
                            Write(JobLanguageRow.LanguageLevel);
                            Write(JobLanguageRow.YearsOfExperience);
                            WriteLine();
                        }
                    }

                    foreach (UmJobQualificationRow JobQualificationRow in AMainDS.UmJobQualification.Rows)
                    {
                        if ((JobQualificationRow.UnitKey == JobRow.UnitKey)
                            && (JobQualificationRow.PositionName == JobRow.PositionName))
                        {
                            Write("UJ-QUAL");
                            WriteLine();
                            Write(JobQualificationRow.PositionName);
                            Write(JobQualificationRow.PositionScope);
                            Write(JobQualificationRow.JobKey);
                            Write(JobQualificationRow.QualificationAreaName);
                            Write(JobQualificationRow.QualificationLevel);
                            Write(JobQualificationRow.YearsOfExperience);
                            WriteLine();
                        }
                    }

                    foreach (UmJobVisionRow JobVisionRow in AMainDS.UmJobVision.Rows)
                    {
                        if ((JobVisionRow.UnitKey == JobRow.UnitKey)
                            && (JobVisionRow.PositionName == JobRow.PositionName))
                        {
                            Write("UJ-VISION");
                            WriteLine();
                            Write(JobVisionRow.PositionName);
                            Write(JobVisionRow.PositionScope);
                            Write(JobVisionRow.JobKey);
                            Write(JobVisionRow.VisionAreaName);
                            Write(JobVisionRow.VisionLevel);
                            WriteLine();
                        }
                    }
                }
            }
        }

        private void WriteVenueData(PartnerImportExportTDS AMainDS, StringCollection ASpecificBuildingInfo)
        {
            foreach (PcBuildingRow BuildingRow in AMainDS.PcBuilding.Rows)
            {
                if ((ASpecificBuildingInfo == null) || (ASpecificBuildingInfo.Count == 0) || ASpecificBuildingInfo.Contains(BuildingRow.BuildingCode))
                {
                    Write("V-BUILDING");
                    WriteLine();
                    Write(BuildingRow.BuildingCode);
                    Write(BuildingRow.BuildingDesc);
                    WriteLine();

                    foreach (PcRoomRow RoomRow in AMainDS.PcRoom.Rows)
                    {
                        if (RoomRow.BuildingCode == BuildingRow.BuildingCode)
                        {
                            Write("V-ROOM");
                            WriteLine();
                            Write(RoomRow.BuildingCode);
                            Write(RoomRow.RoomNumber);
                            Write(RoomRow.Beds);
                            Write(RoomRow.BedCharge);
                            Write(RoomRow.BedCost);
                            Write(RoomRow.MaxOccupancy);
                            Write(RoomRow.GenderPreference);
                            WriteLine();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// export all data of a partner in a long string with newlines, using a format used by Petra 2.x.
        /// containing: partner, person/family/church/etc record, valid locations, special types,
        ///             interests, personnel data, commitments, applications
        /// for units there is more specific data, used eg. for the events file
        /// </summary>
        public string ExportAllData(PartnerImportExportTDS AMainDS, Int32 ASiteKey, Int32 ALocationKey, StringCollection ASpecificBuildingInfo)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner[0];

            StartWriting();

            Write("PARTNER");
            WriteLine();

            Write(PartnerRow.PartnerKey);
            Write(PartnerRow.PartnerClass);
            Write(PartnerRow.PartnerShortName);
            Write(PartnerRow.AcquisitionCode);
            Write(PartnerRow.StatusCode);
            Write(PartnerRow.PreviousName);
            WriteLine();

            Write(PartnerRow.LanguageCode);
            Write(PartnerRow.AddresseeTypeCode);
            Write(PartnerRow.ChildIndicator);
            Write(PartnerRow.ReceiptEachGift);
            Write(PartnerRow.ReceiptLetterFrequency);
            WriteLine();

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                PChurchRow ChurchRow = AMainDS.PChurch[0];
                Write(ChurchRow.ChurchName);
                Write(ChurchRow.DenominationCode);
                Write(ChurchRow.Accomodation);
                Write(ChurchRow.AccomodationSize);
                Write(ChurchRow.AccomodationType);
                Write(ChurchRow.ApproximateSize);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                PFamilyRow FamilyRow = AMainDS.PFamily[0];
                Write(FamilyRow.FamilyName);
                Write(FamilyRow.FirstName);
                Write(FamilyRow.Title);
                Write(FamilyRow.FieldKey);
                Write(FamilyRow.MaritalStatus);
                Write(FamilyRow.MaritalStatusSince);
                Write(FamilyRow.MaritalStatusComment);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                PPersonRow PersonRow = AMainDS.PPerson[0];
                Write(PersonRow.FamilyName);
                Write(PersonRow.FirstName);
                Write(PersonRow.MiddleName1);
                Write(PersonRow.Title);
                Write(PersonRow.Decorations);
                Write(PersonRow.PreferedName);
                Write(PersonRow.DateOfBirth);
                Write(PersonRow.Gender);
                Write(PersonRow.MaritalStatus);
                Write(PersonRow.MaritalStatusSince);
                Write(PersonRow.MaritalStatusComment);
                Write(PersonRow.BelieverSinceYear);
                Write(PersonRow.BelieverSinceComment);
                Write(PersonRow.OccupationCode);
                Write(PersonRow.FieldKey);
                Write(PersonRow.FamilyKey);
                Write(PersonRow.FamilyId);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                POrganisationRow OrganisationRow = AMainDS.POrganisation[0];
                Write(OrganisationRow.OrganisationName);
                Write(OrganisationRow.BusinessCode);
                Write(OrganisationRow.Religious);
                Write(OrganisationRow.Foundation);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                PUnitRow UnitRow = AMainDS.PUnit[0];
                Write(UnitRow.UnitName);
                Write("");                 // was omss code
                Write(UnitRow.XyzTbdCode);
                Write(UnitRow.Description);
                Write(0);                 // was um_default_entry_conf_key_n
                Write(UnitRow.UnitTypeCode);
                Write(UnitRow.CountryCode);
                Write(UnitRow.XyzTbdCost);
                Write(UnitRow.XyzTbdCostCurrencyCode);
                Write(UnitRow.PrimaryOffice);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                PVenueRow VenueRow = AMainDS.PVenue[0];
                Write(VenueRow.VenueName);
                Write(VenueRow.VenueCode);
                Write(VenueRow.CurrencyCode);
                Write(VenueRow.ContactPartnerKey);
                WriteLine();
            }

            AMainDS.PLocation.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}={3}",
                    PLocationTable.GetSiteKeyDBName(),
                    ASiteKey,
                    PLocationTable.GetLocationKeyDBName(),
                    ALocationKey);
            AMainDS.PPartnerLocation.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}={3}",
                    PPartnerLocationTable.GetSiteKeyDBName(),
                    ASiteKey,
                    PPartnerLocationTable.GetLocationKeyDBName(),
                    ALocationKey);

            WriteLocation((PLocationRow)AMainDS.PLocation.DefaultView[0].Row,
                (PPartnerLocationRow)AMainDS.PPartnerLocation.DefaultView[0].Row);

            AMainDS.PPartnerLocation.DefaultView.RowFilter = String.Empty;

            foreach (PPartnerLocationRow PartnerLocationRow in AMainDS.PPartnerLocation.Rows)
            {
                if (!((PartnerLocationRow.LocationKey == ALocationKey)
                      && (PartnerLocationRow.SiteKey == ASiteKey))
                    && (PartnerLocationRow.IsDateGoodUntilNull() || (PartnerLocationRow.DateGoodUntil >= DateTime.Today)))
                {
                    Write("ADDRESS");
                    WriteLine();

                    AMainDS.PLocation.DefaultView.RowFilter =
                        String.Format("{0}={1} and {2}={3}",
                            PLocationTable.GetSiteKeyDBName(),
                            PartnerLocationRow.SiteKey,
                            PLocationTable.GetLocationKeyDBName(),
                            PartnerLocationRow.LocationKey);

                    WriteLocation((PLocationRow)AMainDS.PLocation.DefaultView[0].Row, PartnerLocationRow);
                }
            }

            AMainDS.PLocation.DefaultView.RowFilter = String.Empty;

            if (!PartnerRow.IsCommentNull() && (PartnerRow.Comment.Length > 0))
            {
                Write("COMMENT");
                WriteLine();
                Write(PartnerRow.Comment);
                WriteLine();
            }

            AMainDS.PPartnerComment.DefaultView.Sort = PPartnerCommentTable.GetSequenceDBName();

            foreach (DataRowView v in AMainDS.PPartnerComment.DefaultView)
            {
                PPartnerCommentRow PartnerCommentRow = (PPartnerCommentRow)v.Row;

                Write("COMMENTSEQ");
                WriteLine();
                Write(PartnerCommentRow.Sequence);
                Write(PartnerCommentRow.Comment);
                WriteLine();
            }

            foreach (PPartnerTypeRow PartnerTypeRow in AMainDS.PPartnerType.Rows)
            {
                Write("TYPE");
                WriteLine();
                Write(PartnerTypeRow.TypeCode);
                Write(PartnerTypeRow.ValidFrom);
                Write(PartnerTypeRow.ValidUntil);
                WriteLine();
            }

            foreach (PPartnerInterestRow PartnerInterestRow in AMainDS.PPartnerInterest.Rows)
            {
                AMainDS.PInterest.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PInterestTable.GetInterestDBName(),
                    PartnerInterestRow.Interest);
                Write("INTEREST");
                WriteLine();
                Write(PartnerInterestRow.InterestNumber);
                Write(PartnerInterestRow.FieldKey);
                Write(PartnerInterestRow.Country);
                Write(PartnerInterestRow.Interest);
                Write(((PInterestRow)AMainDS.PInterest.DefaultView[0].Row).Category);
                Write(PartnerInterestRow.Level);
                Write(PartnerInterestRow.Comment);
                WriteLine();
            }

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                WritePersonnelData(AMainDS);
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                WriteUnitData(AMainDS);
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                WriteVenueData(AMainDS, ASpecificBuildingInfo);
            }

            Write("END");
            WriteLine();

            return FinishWriting();
        }
    }
}