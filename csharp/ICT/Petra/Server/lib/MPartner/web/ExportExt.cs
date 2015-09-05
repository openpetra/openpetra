//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Shared.MHospitality.Data;
using System.Globalization;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// Export all data of a partner
    /// </summary>
    public class TPartnerFileExport : TImportExportTextFile
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns>A string that will form the first two lines of a .ext file</returns>
        public string ExtFileHeader()
        {
            String DateFormat = "dmy";
            String UserDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower();

            if (UserDatePattern.IndexOf("m") < UserDatePattern.IndexOf("d"))
            {
                DateFormat = "mdy";
            }

            String PetraVersion = "3.0.0";  // TODO: I should be able to get this out of the system somewhere!
            long MySiteKey = DomainManager.GSiteKey;
            String SubVersion = "0";

            String Header = string.Format("{0} \"{1}\" {2}{4}{3}{4}", DateFormat, PetraVersion, MySiteKey, SubVersion, Environment.NewLine);

            if (DateFormat == "dmy")
            {
                DATEFORMAT = "dd/MM/yyyy";
            }
            else
            {
                DATEFORMAT = "MM/dd/yyyy";
            }

            return Header;
        }

        private void WriteLocation(PLocationRow ALocationRow, PPartnerLocationRow APartnerLocationRow,
            TLocationPK ABestAddressPK)
        {
            string PhoneNumber;
            string EmailAddress;
            string FaxNumber;

            Write(ALocationRow.IsSiteKeyNull() ? 0 : ALocationRow.SiteKey);
            Write(ALocationRow.IsLocalityNull() ? "" : ALocationRow.Locality);
            Write(ALocationRow.IsStreetNameNull() ? "" : ALocationRow.StreetName);
            Write(ALocationRow.IsAddress3Null() ? "" : ALocationRow.Address3);
            WriteLine();
            Write(ALocationRow.IsCityNull() ? "" : ALocationRow.City);
            Write(ALocationRow.IsCountyNull() ? "" : ALocationRow.County);
            Write(ALocationRow.IsPostalCodeNull() ? "" : ALocationRow.PostalCode);
            Write(ALocationRow.IsCountryCodeNull() ? "" : ALocationRow.CountryCode);
            WriteLine();

            Write(APartnerLocationRow.IsDateEffectiveNull() ? "?" : APartnerLocationRow.DateEffective.Value.ToString(DATEFORMAT));
            Write(APartnerLocationRow.IsDateGoodUntilNull() ? "?" : APartnerLocationRow.DateGoodUntil.Value.ToString(DATEFORMAT));
            Write(APartnerLocationRow.IsLocationTypeNull() ? "" : APartnerLocationRow.LocationType);
            Write(APartnerLocationRow.IsSendMailNull() ? false : APartnerLocationRow.SendMail);

            if ((APartnerLocationRow.LocationKey == ABestAddressPK.LocationKey)
                && (APartnerLocationRow.SiteKey == ABestAddressPK.SiteKey))
            {
                // For the Location that is the 'Best Address' of the Partner we export 'Primary Phone Number',
                // 'Primary E-mail Address' and the 'Fax Number'.
                // They are exported for backwards compatibility as part of the 'Location' information as that is the only
                // place where the data was/is stored (and was/is seen and was/is maintained by the user) in Petra 2.x!
                TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhoneAndFax(APartnerLocationRow.PartnerKey,
                    out PhoneNumber, out EmailAddress, out FaxNumber);

                Write(EmailAddress ?? String.Empty);
                Write(PhoneNumber ?? String.Empty);
                Write(0);  // Phone Extensions are no longer kept in the Contact Details scheme so we can't export them...
                Write(FaxNumber ?? String.Empty);
                Write(0);  // Fax Extensions are no longer kept in the Contact Details scheme so we can't export them...
            }
            else
            {
                // For any Location that isn't the 'Best Address' of the Partner: Export empty data for EmailAddress,
                // PhoneNumber, (Phone) Extension, Fax and Fax Extension.
                Write(String.Empty);
                Write(String.Empty);
                Write(0);
                Write(String.Empty);
                Write(0);
            }

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
                Write(ShortTermApplicationRow.IsConfirmedOptionCodeNull() ? "" : ShortTermApplicationRow.ConfirmedOptionCode);
                //Write(ShortTermApplicationRow.IsOption1CodeNull() ? "" : ShortTermApplicationRow.Option1Code);
                //Write(ShortTermApplicationRow.IsOption2CodeNull() ? "" : ShortTermApplicationRow.Option2Code);
                Write(ShortTermApplicationRow.IsFromCongTravelInfoNull() ? "" : ShortTermApplicationRow.FromCongTravelInfo);
                WriteLine();
                Write(ShortTermApplicationRow.IsArrivalNull() ? "?" : ShortTermApplicationRow.Arrival.Value.ToString(DATEFORMAT));
                Write(ShortTermApplicationRow.IsArrivalHourNull() ? 0 : ShortTermApplicationRow.ArrivalHour);
                Write(ShortTermApplicationRow.IsArrivalMinuteNull() ? 0 : ShortTermApplicationRow.ArrivalMinute);
                Write(ShortTermApplicationRow.IsDepartureNull() ? "?" : ShortTermApplicationRow.Departure.Value.ToString(DATEFORMAT));
                Write(ShortTermApplicationRow.IsDepartureHourNull() ? 0 : ShortTermApplicationRow.DepartureHour);
                Write(ShortTermApplicationRow.IsDepartureMinuteNull() ? 0 : ShortTermApplicationRow.DepartureMinute);
                WriteLine();
                Write(ShortTermApplicationRow.IsStApplicationHoldReasonNull() ? "" : ShortTermApplicationRow.StApplicationHoldReason);
                Write(ShortTermApplicationRow.IsStApplicationOnHoldNull() ? false : ShortTermApplicationRow.StApplicationOnHold);
                Write(ShortTermApplicationRow.IsStBasicDeleteFlagNull() ? false : ShortTermApplicationRow.StBasicDeleteFlag);
                //Write(ShortTermApplicationRow.IsStBookingFeeReceivedNull() ? false : ShortTermApplicationRow.StBookingFeeReceived);
                Write(ShortTermApplicationRow.IsStOutreachOnlyFlagNull() ? false : ShortTermApplicationRow.StOutreachOnlyFlag);
                Write(ShortTermApplicationRow.IsStOutreachSpecialCostNull() ? 0 : ShortTermApplicationRow.StOutreachSpecialCost);
                Write(ShortTermApplicationRow.IsStCngrssSpecialCostNull() ? 0 : ShortTermApplicationRow.StCngrssSpecialCost);
                WriteLine();
                //Write(ShortTermApplicationRow.IsStCommentNull() ? "" : ShortTermApplicationRow.StComment);
                WriteLine();
                Write(ShortTermApplicationRow.IsStConfirmedOptionNull() ? 0 : ShortTermApplicationRow.StConfirmedOption);
                Write(ShortTermApplicationRow.IsStCongressCodeNull() ? "" : ShortTermApplicationRow.StCongressCode);
                Write(ShortTermApplicationRow.IsStCongressLanguageNull() ? "" : ShortTermApplicationRow.StCongressLanguage);
                //Write(ShortTermApplicationRow.IsStCountryPrefNull() ? "" : ShortTermApplicationRow.StCountryPref);
                Write(ShortTermApplicationRow.IsStCurrentFieldNull() ? 0 : ShortTermApplicationRow.StCurrentField);
                Write(ShortTermApplicationRow.IsOutreachRoleNull() ? "" : ShortTermApplicationRow.OutreachRole);
                WriteLine();
                Write(ShortTermApplicationRow.IsStFgCodeNull() ? "" : ShortTermApplicationRow.StFgCode);
                Write(ShortTermApplicationRow.IsStFgLeaderNull() ? false : ShortTermApplicationRow.StFgLeader);
                Write(ShortTermApplicationRow.IsStFieldChargedNull() ? 0 : ShortTermApplicationRow.StFieldCharged);
                // Write(ShortTermApplicationRow.IsStLeadershipRatingNull()? "" : ShortTermApplicationRow.StLeadershipRating); // fields removed
                // Write(ShortTermApplicationRow.IsStOption1Null()? 0 : ShortTermApplicationRow.StOption1);
                // Write(ShortTermApplicationRow.IsStOption2Null()? 0 : ShortTermApplicationRow.StOption2);
                WriteLine();
                // Write(ShortTermApplicationRow.IsStPartyContactNull()? 0 : ShortTermApplicationRow.StPartyContact);
                // Write(ShortTermApplicationRow.IsStPartyTogetherNull()? "" :  ShortTermApplicationRow.StPartyTogether);
                Write(ShortTermApplicationRow.IsStPreCongressCodeNull() ? "" : ShortTermApplicationRow.StPreCongressCode);
                // Write(ShortTermApplicationRow.IsStProgramFeeReceivedNull()? false : ShortTermApplicationRow.StProgramFeeReceived);
                // Write(ShortTermApplicationRow.IsStRecruitEffortsNull()? "" : ShortTermApplicationRow.StRecruitEfforts);
                // Write(ShortTermApplicationRow.IsStScholarshipAmountNull()? 0 : ShortTermApplicationRow.StScholarshipAmount);
                // Write(ShortTermApplicationRow.IsStScholarshipApprovedByNull()? "" : ShortTermApplicationRow.StScholarshipApprovedBy);
                // Write(ShortTermApplicationRow.IsStScholarshipPeriodNull()? "" : ShortTermApplicationRow.StScholarshipPeriod);
                // Write(ShortTermApplicationRow.IsStScholarshipReviewDateNull()? "?" : ShortTermApplicationRow.StScholarshipReviewDate.Value.ToString(DATEFORMAT));
                WriteLine();
                Write(ShortTermApplicationRow.IsStSpecialApplicantNull() ? "" : ShortTermApplicationRow.StSpecialApplicant);
                //Write(ShortTermApplicationRow.IsStActivityPrefNull() ? "" : ShortTermApplicationRow.StActivityPref);
                Write(ShortTermApplicationRow.IsToCongTravelInfoNull() ? "" : ShortTermApplicationRow.ToCongTravelInfo);
                Write(ShortTermApplicationRow.IsArrivalPointCodeNull() ? "" : ShortTermApplicationRow.ArrivalPointCode);
                Write(ShortTermApplicationRow.IsDeparturePointCodeNull() ? "" : ShortTermApplicationRow.DeparturePointCode);
                Write(ShortTermApplicationRow.IsTravelTypeFromCongCodeNull() ? "" : ShortTermApplicationRow.TravelTypeFromCongCode);
                Write(ShortTermApplicationRow.IsTravelTypeToCongCodeNull() ? "" : ShortTermApplicationRow.TravelTypeToCongCode);
                WriteLine();
                //Write(ShortTermApplicationRow.IsContactNumberNull() ? "" : ShortTermApplicationRow.ContactNumber);
                Write(ShortTermApplicationRow.IsArrivalDetailsStatusNull() ? "" : ShortTermApplicationRow.ArrivalDetailsStatus);
                Write(ShortTermApplicationRow.IsArrivalTransportNeededNull() ? false : ShortTermApplicationRow.ArrivalTransportNeeded);
                // Write(ShortTermApplicationRow.IsArrivalExpNull()? "?" : ShortTermApplicationRow.ArrivalExp.Value.ToString(DATEFORMAT));
                // Write(ShortTermApplicationRow.IsArrivalExpHourNull()? 0 : ShortTermApplicationRow.ArrivalExpHour);
                // Write(ShortTermApplicationRow.IsArrivalMinuteNull()? 0 : ShortTermApplicationRow.ArrivalExpMinute);
                Write(ShortTermApplicationRow.IsArrivalCommentsNull() ? "" : ShortTermApplicationRow.ArrivalComments);
                Write(ShortTermApplicationRow.IsTransportInterestNull() ? false : ShortTermApplicationRow.TransportInterest);
                WriteLine();
                Write(ShortTermApplicationRow.IsDepartureDetailsStatusNull() ? "" : ShortTermApplicationRow.DepartureDetailsStatus);
                Write(ShortTermApplicationRow.IsDepartureTransportNeededNull() ? false : ShortTermApplicationRow.DepartureTransportNeeded);
                // Write(ShortTermApplicationRow.IsDepartureExpNull()? "?" : ShortTermApplicationRow.DepartureExp.Value.ToString(DATEFORMAT));
                // Write(ShortTermApplicationRow.IsDepartureHourNull()? 0 : ShortTermApplicationRow.DepartureExpHour);
                // Write(ShortTermApplicationRow.IsDepartureMinuteNull()? 0 : ShortTermApplicationRow.DepartureExpMinute);
                Write(ShortTermApplicationRow.IsDepartureCommentsNull() ? "" : ShortTermApplicationRow.DepartureComments);
                WriteLine();
            }
            else
            {
                Write("");                 // ShortTermApplicationRow.ConfirmedOptionCode
                //Write("");                 // ShortTermApplicationRow.Option1Code
                //Write("");                 // ShortTermApplicationRow.Option2Code
                Write("");                 // ShortTermApplicationRow.FromCongTravelInfo
                WriteLine();
                Write("?");                // ShortTermApplicationRow.Arrival
                Write(0);                  // ShortTermApplicationRow.ArrivalHour
                Write(0);                  // ShortTermApplicationRow.ArrivalMinute
                Write("?");                // ShortTermApplicationRow.Departure
                Write(0);                  // ShortTermApplicationRow.DepartureHour
                Write(0);                  // ShortTermApplicationRow.DepartureMinute
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StApplicationHoldReason
                Write(true);               // ShortTermApplicationRow.StApplicationOnHold
                Write(false);              // ShortTermApplicationRow.StBasicDeleteFlag
                Write(false);              // ShortTermApplicationRow.StBookingFeeReceived
                Write(false);              // ShortTermApplicationRow.StOutreachOnlyFlag
                Write(0);                  // ShortTermApplicationRow.StOutreachSpecialCost
                Write(0);                  // ShortTermApplicationRow.StCngrssSpecialCost
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StComment
                WriteLine();
                Write(0);                  // ShortTermApplicationRow.StConfirmedOption
                Write("");                 // ShortTermApplicationRow.StCongressCode
                Write("");                 // ShortTermApplicationRow.StCongressLanguage
                Write("");                 // ShortTermApplicationRow.StCountryPref
                Write(0);                  // ShortTermApplicationRow.StCurrentField
                Write("");                 // ShortTermApplicationRow.OutreachRole
                WriteLine();
                Write("");                 // ShortTermApplicationRow.StFgCode
                Write(false);              // ShortTermApplicationRow.StFgLeader
                Write(0);                  // ShortTermApplicationRow.StFieldCharged
                // Write("");              // ShortTermApplicationRow.StLeadershipRating removed
                // Write(0);               // ShortTermApplicationRow.StOption1
                // Write(0);               // ShortTermApplicationRow.StOption2
                WriteLine();
                // Write(0);               // ShortTermApplicationRow.StPartyContact
                // Write("");              // ShortTermApplicationRow.StPartyTogether
                Write("");                 // ShortTermApplicationRow.StPreCongressCode
                // Write(false);           // ShortTermApplicationRow.StProgramFeeReceived
                // Write("");              // ShortTermApplicationRow.StRecruitEfforts
                // Write(0);               // ShortTermApplicationRow.StScholarshipAmount
                // Write("");              // ShortTermApplicationRow.StScholarshipApprovedBy
                // Write("");              // ShortTermApplicationRow.StScholarshipPeriod
                // Write("?");             // ShortTermApplicationRow.StScholarshipReviewDate
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
                Write(false);              // ShortTermApplicationRow.ArrivalTransportNeeded
                // Write("?");             // ShortTermApplicationRow.ArrivalExp
                // Write(0);               // ShortTermApplicationRow.ArrivalExpHour
                // Write(0);               // ShortTermApplicationRow.ArrivalExpMinute
                Write("");                 // ShortTermApplicationRow.ArrivalComments
                Write(false);              // ShortTermApplicationRow.TransportInterest
                WriteLine();
                Write("");                 // ShortTermApplicationRow.DepartureDetailsStatus
                Write(false);              // ShortTermApplicationRow.DepartureTransportNeeded
                // Write("?");             // ShortTermApplicationRow.DepartureExp
                // Write(0);               // ShortTermApplicationRow.DepartureExpHour
                // Write(0);               // ShortTermApplicationRow.DepartureExpMinute
                Write("");                 // ShortTermApplicationRow.DepartureComments
                WriteLine();
            }
        }

        private void WriteLongApplicationForm(PartnerImportExportTDS AMainDS, PmGeneralApplicationRow AGeneralApplicationRow)
        {
            // TODO: test that the filter works with the date
            AMainDS.PmYearProgramApplication.DefaultView.RowFilter =
                String.Format("{0}={1} and {2}='{3}' and {4}=#{5}#",
                    PmYearProgramApplicationTable.GetPartnerKeyDBName(),
                    AGeneralApplicationRow.PartnerKey,
                    PmYearProgramApplicationTable.GetYpBasicAppTypeDBName(),
                    AGeneralApplicationRow.AppTypeName,
                    PmYearProgramApplicationTable.GetYpAppDateDBName(),
                    AGeneralApplicationRow.GenAppDate.Date.ToString("yyyy-MM-dd"));

            // The RowFilter above will be applied when the Count property is accessed.
            //
            if (AMainDS.PmYearProgramApplication.DefaultView.Count > 0)
            {
                PmYearProgramApplicationRow Row =
                    (PmYearProgramApplicationRow)AMainDS.PmYearProgramApplication.DefaultView[0].Row;
                Write(Row.IsHoOrientConfBookingKeyNull() ? "" : Row.HoOrientConfBookingKey);
                Write(Row.IsYpAgreedJoiningChargeNull() ? 0 : Row.YpAgreedJoiningCharge);
                Write(Row.IsYpAgreedSupportFigureNull() ? 0 : Row.YpAgreedSupportFigure);
                // Write(Row.IsYpAppFeeReceivedNull()? false : Row.YpAppFeeReceived);  // Field removed
                Write(Row.IsYpBasicDeleteFlagNull() ? false : Row.YpBasicDeleteFlag);
                Write(Row.IsYpJoiningConfNull() ? 0 : Row.YpJoiningConf);
                Write(Row.IsStartOfCommitmentNull() ? "?" : Row.StartOfCommitment.Value.ToString(DATEFORMAT));
                Write(Row.IsEndOfCommitmentNull() ? "?" : Row.EndOfCommitment.Value.ToString(DATEFORMAT));
                Write(Row.IsIntendedComLengthMonthsNull() ? 0 : Row.IntendedComLengthMonths);
                Write(Row.IsPositionNameNull() ? "" : Row.PositionName);
                Write(Row.IsPositionScopeNull() ? "" : Row.PositionScope);
                Write(Row.IsAssistantToNull() ? false : Row.AssistantTo);
                WriteLine();
                // Write(Row.IsYpScholarshipAthrizedByNull()? "" : Row.YpScholarshipAthrizedBy);
                // Write(Row.IsYpScholarshipBeginDateNull()? "?" : Row.YpScholarshipBeginDate.Value.ToString(DATEFORMAT));
                // Write(Row.IsYpScholarshipEndDateNull()? "?" : Row.YpScholarshipEndDate.Value.ToString(DATEFORMAT));
                // Write(Row.IsYpScholarshipNull()? 0 : Row.YpScholarship);
                // Write(Row.IsYpScholarshipPeriodNull()? "" : Row.YpScholarshipPeriod);
                // Write(Row.IsYpScholarshipReviewDateNull()? "?" : Row.YpScholarshipReviewDate.Value.ToString(DATEFORMAT));
                Write(Row.IsYpSupportPeriodNull() ? "" : Row.YpSupportPeriod);
                WriteLine();
            }
            else
            {
                // PmYearProgramApplicationRow YearProgramApplicationRow =
                //    (PmYearProgramApplicationRow)AMainDS.PmYearProgramApplication.DefaultView[0].Row;
                Write("");                 // YearProgramApplicationRow.HoOrientConfBookingKey
                Write(0);                  // YearProgramApplicationRow.YpAgreedJoiningCharge
                Write(0);                  // YearProgramApplicationRow.YpAgreedSupportFigure
                // Write(false);           // YearProgramApplicationRow.YpAppFeeReceived
                Write(false);              // YearProgramApplicationRow.YpBasicDeleteFlag
                Write(0);                  // YearProgramApplicationRow.YpJoiningConf
                Write("?");                // YearProgramApplicationRow.StartOfCommitment
                Write("?");                // YearProgramApplicationRow.EndOfCommitment
                Write(0);                  // YearProgramApplicationRow.IntendedComLengthMonths
                Write("");                 // YearProgramApplicationRow.PositionName
                Write("");                 // YearProgramApplicationRow.PositionScope
                Write(false);              // YearProgramApplicationRow.AssistantTo
                WriteLine();
                // Write("");              // YearProgramApplicationRow.YpScholarshipAthrizedBy
                // Write("?");             // YearProgramApplicationRow.YpScholarshipBeginDate
                // Write("?");             // YearProgramApplicationRow.YpScholarshipEndDate
                // Write(0);               // YearProgramApplicationRow.YpScholarship
                // Write("");              // YearProgramApplicationRow.YpScholarshipPeriod
                // Write("?");             // YearProgramApplicationRow.YpScholarshipReviewDate
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
                    Write(ApplicationTypeRow.IsAppFormTypeNull() ? "" : ApplicationTypeRow.AppFormType);
                    Write(ApplicationTypeRow.IsAppTypeNameNull() ? "" : ApplicationTypeRow.AppTypeName);
                    Write(ApplicationTypeRow.IsAppTypeDescrNull() ? "" : ApplicationTypeRow.AppTypeDescr);
                    WriteLine();
                    Write(GeneralApplicationRow.IsGenAppDateNull() ? "?" : GeneralApplicationRow.GenAppDate.ToString(DATEFORMAT));
                    Write(GeneralApplicationRow.IsOldLinkNull() ? "" : GeneralApplicationRow.OldLink);
                    WriteLine();
                    Write(GeneralApplicationRow.IsGenApplicantTypeNull() ? "" : GeneralApplicationRow.GenApplicantType);
                    Write(GeneralApplicationRow.IsGenApplicationHoldReasonNull() ? "" : GeneralApplicationRow.GenApplicationHoldReason);
                    Write(GeneralApplicationRow.IsGenApplicationOnHoldNull() ? false : GeneralApplicationRow.GenApplicationOnHold);
                    Write(GeneralApplicationRow.IsGenApplicationStatusNull() ? "" : GeneralApplicationRow.GenApplicationStatus);
                    Write(GeneralApplicationRow.IsGenAppCancelledNull() ? "?" : GeneralApplicationRow.GenAppCancelled.Value.ToString(DATEFORMAT));
                    Write(GeneralApplicationRow.IsGenAppCancelReasonNull() ? "" : GeneralApplicationRow.GenAppCancelReason);
                    Write(GeneralApplicationRow.IsGenAppDeleteFlagNull() ? false : GeneralApplicationRow.GenAppDeleteFlag);
                    Write(GeneralApplicationRow.IsClosedNull() ? false : GeneralApplicationRow.Closed);
                    Write(GeneralApplicationRow.IsClosedByNull() ? "" : GeneralApplicationRow.ClosedBy);
                    Write(GeneralApplicationRow.IsDateClosedNull() ? "?" : GeneralApplicationRow.DateClosed.Value.ToString(DATEFORMAT));
                    WriteLine();
                    Write(GeneralApplicationRow.IsGenAppPossSrvUnitKeyNull() ? 0 : GeneralApplicationRow.GenAppPossSrvUnitKey);
                    Write(GeneralApplicationRow.IsGenAppRecvgFldAcceptNull() ? "?" : GeneralApplicationRow.GenAppRecvgFldAccept.Value.ToString(
                            DATEFORMAT));
                    Write(GeneralApplicationRow.IsGenAppSrvFldAcceptNull() ? false : GeneralApplicationRow.GenAppSrvFldAccept);
                    Write(GeneralApplicationRow.IsGenAppSendFldAcceptDateNull() ? "?" : GeneralApplicationRow.GenAppSendFldAcceptDate.Value.ToString(
                            DATEFORMAT));
                    Write(GeneralApplicationRow.IsGenAppSendFldAcceptNull() ? false : GeneralApplicationRow.GenAppSendFldAccept);
                    Write(GeneralApplicationRow.IsGenAppCurrencyCodeNull() ? "" : GeneralApplicationRow.GenAppCurrencyCode);
                    Write(GeneralApplicationRow.IsPlacementPartnerKeyNull() ? 0 : GeneralApplicationRow.PlacementPartnerKey);
                    WriteLine();
                    Write(GeneralApplicationRow.IsGenAppUpdateNull() ? "?" : GeneralApplicationRow.GenAppUpdate.Value.ToString(DATEFORMAT));
                    Write(GeneralApplicationRow.IsGenCancelledAppNull() ? false : GeneralApplicationRow.GenCancelledApp);
                    Write(GeneralApplicationRow.IsGenContact1Null() ? "" : GeneralApplicationRow.GenContact1);
                    Write(GeneralApplicationRow.IsGenContact2Null() ? "" : GeneralApplicationRow.GenContact2);
                    //Write(GeneralApplicationRow.IsGenYearProgramNull() ? "" : GeneralApplicationRow.GenYearProgram);
                    Write(GeneralApplicationRow.IsApplicationKeyNull() ? 0 : GeneralApplicationRow.ApplicationKey);
                    Write(GeneralApplicationRow.IsRegistrationOfficeNull() ? 0 : GeneralApplicationRow.RegistrationOffice);
                    WriteMultiLine(GeneralApplicationRow.IsCommentNull() ? "" : GeneralApplicationRow.Comment);
                    WriteLine();

                    if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_SHORTFORM)
                    {
                        WriteShortApplicationForm(AMainDS, GeneralApplicationRow);
                    }
                    else if (ApplicationTypeRow.AppFormType == MPersonnelConstants.APPLICATIONFORMTYPE_LONGFORM)
                    {
                        WriteLongApplicationForm(AMainDS, GeneralApplicationRow);
                    }
                }
            }
        }

        private void WritePersonnelData(PartnerImportExportTDS AMainDS)
        {
            foreach (PmPersonalDataRow PersonalDataRow in AMainDS.PmPersonalData.Rows)
            {
                Write("PERSONAL");
                WriteLine();
                Write(PersonalDataRow.IsBelieverSinceYearNull() ? 0 : PersonalDataRow.BelieverSinceYear);
                Write(PersonalDataRow.IsBelieverSinceCommentNull() ? "" : PersonalDataRow.BelieverSinceComment);
                Write(PersonalDataRow.IsBloodTypeNull() ? "" : PersonalDataRow.BloodType);
                WriteLine();
                // Write(PersonalDataRow.IsDriverStatusNull()? "" : PersonalDataRow.DriverStatus);
                // Write(PersonalDataRow.IsGenDriverLicenseNull()? false : PersonalDataRow.GenDriverLicense);
                // Write(PersonalDataRow.IsDrivingLicenseNumberNull()? "" : PersonalDataRow.DrivingLicenseNumber);
                // Write(PersonalDataRow.IsInternalDriverLicenseNull()? false : PersonalDataRow.InternalDriverLicense);
                // WriteLine();
            }

            foreach (PmPassportDetailsRow PassportDetailsRow in AMainDS.PmPassportDetails.Rows)
            {
                if (PassportDetailsRow.IsDateOfExpirationNull() || (PassportDetailsRow.DateOfExpiration < DateTime.Today))
                {
                    Write("PASSPORT");
                    WriteLine();
                    Write(PassportDetailsRow.IsPassportNumberNull() ? "" : PassportDetailsRow.PassportNumber);
                    WriteLine();
                    Write(PassportDetailsRow.IsMainPassportNull() ? false : PassportDetailsRow.MainPassport);
                    Write(PassportDetailsRow.IsCountryOfIssueNull() ? "" : PassportDetailsRow.CountryOfIssue);
                    Write(PassportDetailsRow.IsDateOfExpirationNull() ? "?" : PassportDetailsRow.DateOfExpiration.Value.ToString(DATEFORMAT));
                    Write(PassportDetailsRow.IsDateOfIssueNull() ? "?" : PassportDetailsRow.DateOfIssue.Value.ToString(DATEFORMAT));
                    Write(PassportDetailsRow.IsFullPassportNameNull() ? "" : PassportDetailsRow.FullPassportName);
                    Write(PassportDetailsRow.IsPassportNationalityCodeNull() ? "" : PassportDetailsRow.PassportNationalityCode);
                    Write(PassportDetailsRow.IsPassportDetailsTypeNull() ? "" : PassportDetailsRow.PassportDetailsType);
                    Write(PassportDetailsRow.IsPassportDobNull() ? "?" : PassportDetailsRow.PassportDob.Value.ToString(DATEFORMAT));
                    Write(PassportDetailsRow.IsPlaceOfBirthNull() ? "" : PassportDetailsRow.PlaceOfBirth);
                    Write(PassportDetailsRow.IsPlaceOfIssueNull() ? "" : PassportDetailsRow.PlaceOfIssue);
                    WriteLine();
                }
            }

            foreach (PmDocumentRow DocumentRow in AMainDS.PmDocument.Rows)
            {
                AMainDS.PmDocumentType.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PmDocumentTypeTable.GetDocCodeDBName(),
                    DocumentRow.DocCode);
                PmDocumentTypeRow TypeRow = (PmDocumentTypeRow)AMainDS.PmDocumentType.DefaultView[0].Row;

                Write("PERSDOCUMENT");
                WriteLine();
                Write(DocumentRow.IsSiteKeyNull() ? 0 : DocumentRow.SiteKey);
                Write(DocumentRow.IsDocumentKeyNull() ? 0 : DocumentRow.DocumentKey);
                WriteLine();
                Write(DocumentRow.IsDocCodeNull() ? "" : DocumentRow.DocCode);
                Write(TypeRow.IsDocCategoryNull() ? "" : TypeRow.DocCategory);
                Write(DocumentRow.IsDocumentIdNull() ? "" : DocumentRow.DocumentId);
                Write(DocumentRow.IsPlaceOfIssueNull() ? "" : DocumentRow.PlaceOfIssue);
                Write(DocumentRow.IsDateOfIssueNull() ? "?" : DocumentRow.DateOfIssue.Value.ToString(DATEFORMAT));
                Write(DocumentRow.IsDateOfStartNull() ? "?" : DocumentRow.DateOfStart.Value.ToString(DATEFORMAT));
                Write(DocumentRow.IsDateOfExpirationNull() ? "?" : DocumentRow.DateOfExpiration.Value.ToString(DATEFORMAT));
                Write(DocumentRow.IsAssocDocIdNull() ? "" : DocumentRow.AssocDocId);
                Write(DocumentRow.IsContactPartnerKeyNull() ? 0 : DocumentRow.ContactPartnerKey);
                WriteLine();
                Write(DocumentRow.IsDocCommentNull() ? "" : DocumentRow.DocComment);
                WriteLine();
            }

            foreach (PmPersonQualificationRow Row in AMainDS.PmPersonQualification.Rows)
            {
                Write("PROFESN");
                WriteLine();
                Write(Row.IsQualificationAreaNameNull() ? "" : Row.QualificationAreaName);
                WriteLine();
                Write(Row.IsQualificationLevelNull() ? 0 : Row.QualificationLevel);
                Write(Row.IsInformalFlagNull() ? false : Row.InformalFlag);
                Write(Row.IsYearsOfExperienceNull() ? 0 : Row.YearsOfExperience);
                Write(Row.IsYearsOfExperienceAsOfNull() ? "?" : Row.YearsOfExperienceAsOf.Value.ToString(DATEFORMAT));
                Write(Row.IsCommentNull() ? "" : Row.Comment);
                Write(Row.IsQualificationDateNull() ? "?" : Row.QualificationDate.Value.ToString(DATEFORMAT));
                Write(Row.IsQualificationExpiryNull() ? "?" : Row.QualificationExpiry.Value.ToString(DATEFORMAT));
                WriteLine();
            }

            foreach (PmSpecialNeedRow SpecialNeedRow in AMainDS.PmSpecialNeed.Rows)
            {
                Write("SPECNEED");
                WriteLine();
                Write(SpecialNeedRow.IsDateCreatedNull() ? "?" : SpecialNeedRow.DateCreated.Value.ToString(DATEFORMAT));
                // Write(false); // ContactHomeOffice is removed
                Write(SpecialNeedRow.IsVegetarianFlagNull() ? false : SpecialNeedRow.VegetarianFlag);
                WriteLine();
                Write(SpecialNeedRow.IsDietaryCommentNull() ? "" : SpecialNeedRow.DietaryComment);
                WriteLine();
                Write(SpecialNeedRow.IsMedicalCommentNull() ? "" : SpecialNeedRow.MedicalComment);
                WriteLine();
                Write(SpecialNeedRow.IsOtherSpecialNeedNull() ? "" : SpecialNeedRow.OtherSpecialNeed);
                WriteLine();
            }

            foreach (PmPastExperienceRow PastExperienceRow in AMainDS.PmPastExperience.Rows)
            {
                Write("PREVEXP");
                WriteLine();
                Write(PastExperienceRow.IsSiteKeyNull() ? 0 : PastExperienceRow.SiteKey);
                Write(PastExperienceRow.IsKeyNull() ? 0 : PastExperienceRow.Key);
                Write(PastExperienceRow.IsPrevLocationNull() ? "" : PastExperienceRow.PrevLocation);
                Write(PastExperienceRow.IsStartDateNull() ? "?" : PastExperienceRow.StartDate.Value.ToString(DATEFORMAT));
                Write(PastExperienceRow.IsEndDateNull() ? "?" : PastExperienceRow.EndDate.Value.ToString(DATEFORMAT));
                WriteLine();
                Write(PastExperienceRow.IsPrevWorkHereNull() ? false : PastExperienceRow.PrevWorkHere);
                Write(PastExperienceRow.IsPrevWorkNull() ? false : PastExperienceRow.PrevWork);
                Write(PastExperienceRow.IsOtherOrganisationNull() ? "" : PastExperienceRow.OtherOrganisation);
                Write(PastExperienceRow.IsPrevRoleNull() ? "" : PastExperienceRow.PrevRole);
                Write(PastExperienceRow.IsCategoryNull() ? "" : PastExperienceRow.Category);
                WriteLine();
                Write(PastExperienceRow.IsPastExpCommentsNull() ? "" : PastExperienceRow.PastExpComments);
                WriteLine();
            }

            foreach (PmPersonLanguageRow PersonLanguageRow in AMainDS.PmPersonLanguage.Rows)
            {
                Write("LANGUAGE");
                WriteLine();
                Write(PersonLanguageRow.IsLanguageCodeNull() ? "" : PersonLanguageRow.LanguageCode);
                WriteLine();
                // Write(PersonLanguageRow.IsWillingToTranslateNull()? false : PersonLanguageRow.WillingToTranslate);
                // Write(PersonLanguageRow.IsTranslateIntoNull()? false : PersonLanguageRow.TranslateInto);
                // Write(PersonLanguageRow.IsTranslateOutOfNull()? false : PersonLanguageRow.TranslateOutOf);
                Write(PersonLanguageRow.IsYearsOfExperienceNull() ? 0 : PersonLanguageRow.YearsOfExperience);
                Write(PersonLanguageRow.IsLanguageLevelNull() ? 0 : PersonLanguageRow.LanguageLevel);
                Write(PersonLanguageRow.IsYearsOfExperienceAsOfNull() ? "?" : PersonLanguageRow.YearsOfExperienceAsOf.Value.ToString(DATEFORMAT));
                Write(PersonLanguageRow.IsCommentNull() ? "" : PersonLanguageRow.Comment);
                WriteLine();
            }

            foreach (PmPersonAbilityRow PersonAbilityRow in AMainDS.PmPersonAbility.Rows)
            {
                Write("ABILITY");
                WriteLine();
                Write(PersonAbilityRow.IsAbilityAreaNameNull() ? "" : PersonAbilityRow.AbilityAreaName);
                Write(PersonAbilityRow.IsAbilityLevelNull() ? 0 : PersonAbilityRow.AbilityLevel);
                WriteLine();
                Write(PersonAbilityRow.IsYearsOfExperienceNull() ? 0 : PersonAbilityRow.YearsOfExperience);
                Write(PersonAbilityRow.IsBringingInstrumentNull() ? false : PersonAbilityRow.BringingInstrument);
                Write(PersonAbilityRow.IsYearsOfExperienceAsOfNull() ? "?" : PersonAbilityRow.YearsOfExperienceAsOf.Value.ToString(DATEFORMAT));
                Write(PersonAbilityRow.IsCommentNull() ? "" : PersonAbilityRow.Comment);
                WriteLine();
            }

            foreach (PmStaffDataRow StaffDataRow in AMainDS.PmStaffData.Rows)
            {
                Write("COMMIT");
                WriteLine();
                Write(StaffDataRow.IsSiteKeyNull() ? 0 : StaffDataRow.SiteKey);
                Write(StaffDataRow.IsKeyNull() ? 0 : StaffDataRow.Key);
                Write(StaffDataRow.IsStartOfCommitmentNull() ? "?" : StaffDataRow.StartOfCommitment.ToString(DATEFORMAT));
                Write(StaffDataRow.IsStartDateApproxNull() ? false : StaffDataRow.StartDateApprox);
                Write(StaffDataRow.IsEndOfCommitmentNull() ? "?" : StaffDataRow.EndOfCommitment.Value.ToString(DATEFORMAT));
                WriteLine();
                Write(StaffDataRow.IsStatusCodeNull() ? "" : StaffDataRow.StatusCode);
                Write(StaffDataRow.IsReceivingFieldNull() ? 0 : StaffDataRow.ReceivingField);
                Write(StaffDataRow.IsHomeOfficeNull() ? 0 : StaffDataRow.HomeOffice);
                Write(StaffDataRow.IsOfficeRecruitedByNull() ? 0 : StaffDataRow.OfficeRecruitedBy);
                Write(StaffDataRow.IsReceivingFieldOfficeNull() ? 0 : StaffDataRow.ReceivingFieldOffice);
                Write(StaffDataRow.IsJobTitleNull() ? "" : StaffDataRow.JobTitle);
                WriteLine();
                Write(StaffDataRow.IsStaffDataCommentsNull() ? "" : StaffDataRow.StaffDataComments);
                WriteLine();
            }

            foreach (PmJobAssignmentRow JobAssignmentRow in AMainDS.PmJobAssignment.Rows)
            {
                Write("JOB");
                WriteLine();
                Write(JobAssignmentRow.IsFromDateNull() ? "?" : JobAssignmentRow.FromDate.ToString(DATEFORMAT));
                Write(JobAssignmentRow.IsToDateNull() ? "?" : JobAssignmentRow.ToDate.Value.ToString(DATEFORMAT));
                Write(JobAssignmentRow.IsPositionNameNull() ? "" : JobAssignmentRow.PositionName);
                Write(JobAssignmentRow.IsPositionScopeNull() ? "" : JobAssignmentRow.PositionScope);
                Write(JobAssignmentRow.IsAssistantToNull() ? false : JobAssignmentRow.AssistantTo);
                Write(JobAssignmentRow.IsJobKeyNull() ? 0 : JobAssignmentRow.JobKey);
                Write(JobAssignmentRow.IsJobAssignmentKeyNull() ? 0 : JobAssignmentRow.JobAssignmentKey);
                WriteLine();
                Write(JobAssignmentRow.IsUnitKeyNull() ? 0 : JobAssignmentRow.UnitKey);
                Write(JobAssignmentRow.IsAssignmentTypeCodeNull() ? "" : JobAssignmentRow.AssignmentTypeCode);
                // Write(JobAssignmentRow.IsLeavingCodeNull()? "" : JobAssignmentRow.LeavingCode);
                // Write(JobAssignmentRow.IsLeavingCodeUpdatedDateNull()? "?" : JobAssignmentRow.LeavingCodeUpdatedDate.Value.ToString(DATEFORMAT));
                WriteLine();
            }

            foreach (PmPersonEvaluationRow Row in AMainDS.PmPersonEvaluation.Rows)
            {
                Write("PROGREP");
                WriteLine();
                Write(Row.IsEvaluationDateNull() ? "?" : Row.EvaluationDate.ToString(DATEFORMAT));
                Write(Row.IsEvaluatorNull() ? "" : Row.Evaluator);
                WriteLine();
                Write(Row.IsEvaluationTypeNull() ? "" : Row.EvaluationType);
                Write(Row.IsNextEvaluationDateNull() ? "?" : Row.NextEvaluationDate.Value.ToString(DATEFORMAT));
                WriteLine();
                Write(Row.IsEvaluationCommentsNull() ? "" : Row.EvaluationComments);
                WriteLine();
                Write(Row.IsPersonEvalActionNull() ? "" : Row.PersonEvalAction);
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
                Write(UnitStructureRow.IsParentUnitKeyNull() ? 0 : UnitStructureRow.ParentUnitKey);
                WriteLine();
            }

/*
 *  There's a clutch of tables here we're not exporting anymore:
 *
 *          foreach (UmUnitAbilityRow UnitAbilityRow in AMainDS.UmUnitAbility.Rows)
 *          {
 *              Write("U-ABILITY");
 *              WriteLine();
 *              Write(UnitAbilityRow.IsAbilityAreaNameNull()? "" : UnitAbilityRow.AbilityAreaName);
 *              Write(UnitAbilityRow.IsAbilityLevelNull()? 0 : UnitAbilityRow.AbilityLevel);
 *              Write(UnitAbilityRow.IsYearsOfExperienceNull()? 0 : UnitAbilityRow.YearsOfExperience);
 *              WriteLine();
 *          }
 *
 *          foreach (UmUnitLanguageRow UnitLanguageRow in AMainDS.UmUnitLanguage.Rows)
 *          {
 *              Write("U-LANG");
 *              WriteLine();
 *              Write(UnitLanguageRow.IsLanguageCodeNull()? "" : UnitLanguageRow.LanguageCode);
 *              Write(UnitLanguageRow.IsLanguageLevelNull()? 0 : UnitLanguageRow.LanguageLevel);
 *              Write(UnitLanguageRow.IsYearsOfExperienceNull()? 0 : UnitLanguageRow.YearsOfExperience);
 *              Write(UnitLanguageRow.IsUnitLanguageReqNull()? "" : UnitLanguageRow.UnitLanguageReq);
 *              WriteLine();
 *              Write(UnitLanguageRow.IsUnitLangCommentNull()? "" : UnitLanguageRow.UnitLangComment);
 *              WriteLine();
 *          }
 *
 *          // table dropped in OpenPetra
 *          foreach (UmUnitVisionRow UnitVisionRow in AMainDS.UmUnitVision.Rows)
 *          {
 *              Write("U-VISION");
 *              WriteLine();
 *              Write(UnitVisionRow.IsVisionAreaNameNull()? "" : UnitVisionRow.VisionAreaName);
 *              Write(UnitVisionRow.IsVisionLevelNull()? 0 : UnitVisionRow.VisionLevel);
 *              WriteLine();
 *          }
 *
 *          AMainDS.UmUnitCost.DefaultView.Sort = UmUnitCostTable.GetValidFromDateDBName() + " desc";
 *          // TODO: A filter could be applied here, to get only current and future costs.
 *
 *          // AMainDS.UmUnitCost.DefaultView.RowFilter = String.Format ("{0} >= #{1}#",
 *          //                                                         UmUnitCostTable.GetValidFromDateDBName(),
 *          //                                                         DateTime.Today);
 *
 *          foreach (DataRowView v in AMainDS.UmUnitCost.DefaultView)
 *          {
 *              UmUnitCostRow UnitCostRow = (UmUnitCostRow)v.Row;
 *
 *              // only export current and future costs
 *              if (UnitCostRow.ValidFromDate >= DateTime.Today)
 *              {
 *                  Write("U-COSTS");
 *                  WriteLine();
 *                  Write(UnitCostRow.IsValidFromDateNull()? "?" : UnitCostRow.ValidFromDate.ToString(DATEFORMAT));
 *                  Write(UnitCostRow.IsChargePeriodNull()? "" : UnitCostRow.ChargePeriod);
 *                  WriteLine();
 *                  // only export values in international currency
 *                  Write(UnitCostRow.IsCoupleJoiningChargeIntlNull()? 0 : UnitCostRow.CoupleJoiningChargeIntl);
 *                  Write(UnitCostRow.IsAdultJoiningChargeIntlNull()? 0 : UnitCostRow.AdultJoiningChargeIntl);
 *                  Write(UnitCostRow.IsChildJoiningChargeIntlNull()? 0 : UnitCostRow.ChildJoiningChargeIntl);
 *                  Write(UnitCostRow.IsCoupleCostsPeriodIntlNull()? 0 : UnitCostRow.CoupleCostsPeriodIntl);
 *                  Write(UnitCostRow.IsSingleCostsPeriodIntlNull()? 0 : UnitCostRow.SingleCostsPeriodIntl);
 *                  Write(UnitCostRow.IsChild1CostsPeriodIntlNull()? 0 : UnitCostRow.Child1CostsPeriodIntl);
 *                  Write(UnitCostRow.IsChild2CostsPeriodIntlNull()? 0 : UnitCostRow.Child2CostsPeriodIntl);
 *                  Write(UnitCostRow.IsChild3CostsPeriodIntlNull()? 0 : UnitCostRow.Child3CostsPeriodIntl);
 *                  WriteLine();
 *              }
 *          }
 *
 *          AMainDS.UmJob.DefaultView.Sort = UmJobTable.GetToDateDBName() + " desc";
 *         // AMainDS.UmJob.DefaultView.RowFilter = String.Format ("{0} >= #{1}#",
 *         //                                                          UmJobTable.GetToDateDBName(),
 *         //                                                          DateTime.Today);
 *
 *          foreach (DataRowView v in AMainDS.UmJob.DefaultView)
 *          {
 *              UmJobRow JobRow = (UmJobRow)v.Row;
 *
 *              // only export current and future jobs
 *              if (JobRow.ToDate >= DateTime.Today)
 *              {
 *                  Write("U-JOB");
 *                  WriteLine();
 *                  Write(JobRow.IsPositionNameNull()? "" : JobRow.PositionName);
 *                  Write(JobRow.IsPositionScopeNull()? "" : JobRow.PositionScope);
 *                  Write(JobRow.IsJobKeyNull()? 0 : JobRow.JobKey);
 *                  WriteLine();
 *                  Write(JobRow.IsFromDateNull()? "?" : JobRow.FromDate.Value.ToString(DATEFORMAT));
 *                  Write(JobRow.IsToDateNull()? "?" : JobRow.ToDate.Value.ToString(DATEFORMAT));
 *                  Write(JobRow.IsJobTypeNull()? "" : JobRow.JobType);
 *                  Write(JobRow.IsCommitmentPeriodNull()? "" : JobRow.CommitmentPeriod);
 *                  Write(JobRow.IsTrainingPeriodNull()? "" : JobRow.TrainingPeriod);
 *                  Write(JobRow.IsPartTimeFlagNull()? false : JobRow.PartTimeFlag);
 *                  Write(JobRow.IsPreviousInternalExpReqNull()? false : JobRow.PreviousInternalExpReq);
 *                  Write(JobRow.IsJobPublicityNull()? 0 : JobRow.JobPublicity);
 *                  Write(JobRow.IsPublicFlagNull()? false : JobRow.PublicFlag);
 *                  Write(JobRow.IsMaximumNull()? 0 : JobRow.Maximum);
 *                  Write(JobRow.IsMinimumNull()? 0 : JobRow.Minimum);
 *                  Write(JobRow.IsPresentNull()? 0 : JobRow.Present);
 *                  Write(JobRow.IsPartTimersNull()? 0 : JobRow.PartTimers);
 *                  WriteLine();
 *
 *                  foreach (UmJobRequirementRow Row in AMainDS.UmJobRequirement.Rows)
 *                  {
 *                      if ((Row.UnitKey == JobRow.UnitKey)
 *                          && (Row.PositionName == JobRow.PositionName))
 *                      {
 *                          Write("UJ-ABIL");
 *                          WriteLine();
 *                          Write(Row.IsPositionNameNull()? "" : Row.PositionName);
 *                          Write(Row.IsPositionScopeNull()? "" : Row.PositionScope);
 *                          Write(Row.IsJobKeyNull()? 0 : Row.JobKey);
 *                          Write(Row.IsAbilityAreaNameNull()? "" : Row.AbilityAreaName);
 *                          Write(Row.IsAbilityLevelNull()? 0 : Row.AbilityLevel);
 *                          Write(Row.IsYearsOfExperienceNull()? 0 : Row.YearsOfExperience);
 *                          WriteLine();
 *                      }
 *                  }
 *
 *                  foreach (UmJobLanguageRow JobLanguageRow in AMainDS.UmJobLanguage.Rows)
 *                  {
 *                      if ((JobLanguageRow.UnitKey == JobRow.UnitKey)
 *                          && (JobLanguageRow.PositionName == JobRow.PositionName))
 *                      {
 *                          Write("UJ-LANG");
 *                          WriteLine();
 *                          Write(JobLanguageRow.IsPositionNameNull()? "" : JobLanguageRow.PositionName);
 *                          Write(JobLanguageRow.IsPositionScopeNull()? "" : JobLanguageRow.PositionScope);
 *                          Write(JobLanguageRow.IsJobKeyNull()? 0 : JobLanguageRow.JobKey);
 *                          Write(JobLanguageRow.IsLanguageCodeNull()? "" : JobLanguageRow.LanguageCode);
 *                          Write(JobLanguageRow.IsLanguageLevelNull()? 0 : JobLanguageRow.LanguageLevel);
 *                          Write(JobLanguageRow.IsYearsOfExperienceNull()? 0 : JobLanguageRow.YearsOfExperience);
 *                          WriteLine();
 *                      }
 *                  }
 *
 *                  foreach (UmJobQualificationRow Row in AMainDS.UmJobQualification.Rows)
 *                  {
 *                      if ((Row.UnitKey == JobRow.UnitKey)
 *                          && (Row.PositionName == JobRow.PositionName))
 *                      {
 *                          Write("UJ-QUAL");
 *                          WriteLine();
 *                          Write(Row.IsPositionNameNull()? "" : Row.PositionName);
 *                          Write(Row.IsPositionScopeNull()? "" : Row.PositionScope);
 *                          Write(Row.IsJobKeyNull()? 0 : Row.JobKey);
 *                          Write(Row.IsQualificationAreaNameNull()? "" : Row.QualificationAreaName);
 *                          Write(Row.IsQualificationLevelNull()? 0 : Row.QualificationLevel);
 *                          Write(Row.IsYearsOfExperienceNull()? 0 : Row.YearsOfExperience);
 *                          WriteLine();
 *                      }
 *                  }
 *
 *                  // table dropped in OpenPetra
 *                  foreach (UmJobVisionRow JobVisionRow in AMainDS.UmJobVision.Rows)
 *                  {
 *                      if ((JobVisionRow.UnitKey == JobRow.UnitKey)
 *                          && (JobVisionRow.PositionName == JobRow.PositionName))
 *                      {
 *                          Write("UJ-VISION");
 *                          WriteLine();
 *                          Write(JobVisionRow.IsPositionNameNull()? "" : JobVisionRow.PositionName);
 *                          Write(JobVisionRow.IsPositionScopeNull()? "" : JobVisionRow.PositionScope);
 *                          Write(JobVisionRow.IsJobKeyNull()? 0 : JobVisionRow.JobKey);
 *                          Write(JobVisionRow.IsVisionAreaNameNull()? "" : JobVisionRow.VisionAreaName);
 *                          Write(JobVisionRow.IsVisionLevelNull()? 0 : JobVisionRow.VisionLevel);
 *                          WriteLine();
 *                      }
 *                  }
 *              }
 *          }
 */
        }

        private void WriteVenueData(PartnerImportExportTDS AMainDS, StringCollection ASpecificBuildingInfo)
        {
            foreach (PcBuildingRow BuildingRow in AMainDS.PcBuilding.Rows)
            {
                if ((ASpecificBuildingInfo == null) || (ASpecificBuildingInfo.Count == 0) || ASpecificBuildingInfo.Contains(BuildingRow.BuildingCode))
                {
                    Write("V-BUILDING");
                    WriteLine();
                    Write(BuildingRow.IsBuildingCodeNull() ? "" : BuildingRow.BuildingCode);
                    Write(BuildingRow.IsBuildingDescNull() ? "" : BuildingRow.BuildingDesc);
                    WriteLine();

                    foreach (PcRoomRow RoomRow in AMainDS.PcRoom.Rows)
                    {
                        if (RoomRow.BuildingCode == BuildingRow.BuildingCode)
                        {
                            Write("V-ROOM");
                            WriteLine();
                            Write(RoomRow.IsBuildingCodeNull() ? "" : RoomRow.BuildingCode);
                            Write(RoomRow.IsRoomNumberNull() ? "" : RoomRow.RoomNumber);
                            Write(RoomRow.IsBedsNull() ? 0 : RoomRow.Beds);
                            Write(RoomRow.IsBedChargeNull() ? 0 : RoomRow.BedCharge);
                            Write(RoomRow.IsBedCostNull() ? 0 : RoomRow.BedCost);
                            Write(RoomRow.IsMaxOccupancyNull() ? 0 : RoomRow.MaxOccupancy);
                            Write(RoomRow.IsGenderPreferenceNull() ? "" : RoomRow.GenderPreference);
                            WriteLine();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Export all data of a partner in a long string with newlines, using a format used by Petra 2.x.
        /// containing: partner, person/family/church/etc record, valid locations, special types,
        ///             interests, personnel data, commitments, applications
        /// For units there is more specific data, used eg. for the events file
        /// </summary>
        public string ExportPartnerExt(PartnerImportExportTDS AMainDS, Int64 ASiteKey, Int32 ALocationKey, StringCollection ASpecificBuildingInfo)
        {
            PPartnerRow PartnerRow = AMainDS.PPartner[0];
            TLocationPK BestAddressPK;

            StartWriting();

            Write("PARTNER");
            WriteLine();

            Write(PartnerRow.PartnerKey);
            Write(PartnerRow.IsPartnerClassNull() ? "" : PartnerRow.PartnerClass);
            Write(PartnerRow.IsPartnerShortNameNull() ? "" : PartnerRow.PartnerShortName);
            Write(PartnerRow.IsAcquisitionCodeNull() ? "" : PartnerRow.AcquisitionCode);
            Write(PartnerRow.IsStatusCodeNull() ? "" : PartnerRow.StatusCode);
            Write(PartnerRow.IsPreviousNameNull() ? "" : PartnerRow.PreviousName);
            WriteLine();

            Write(PartnerRow.IsLanguageCodeNull() ? "" : PartnerRow.LanguageCode);
            Write(PartnerRow.IsAddresseeTypeCodeNull() ? "" : PartnerRow.AddresseeTypeCode);
            Write(PartnerRow.IsChildIndicatorNull() ? false : PartnerRow.ChildIndicator);
            Write(PartnerRow.IsReceiptEachGiftNull() ? false : PartnerRow.ReceiptEachGift);
            Write(PartnerRow.IsReceiptLetterFrequencyNull() ? "" : PartnerRow.ReceiptLetterFrequency);
            WriteLine();

            if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_CHURCH)
            {
                PChurchRow ChurchRow = AMainDS.PChurch[0];
                Write(ChurchRow.IsChurchNameNull() ? "" : ChurchRow.ChurchName);
                Write(ChurchRow.IsDenominationCodeNull() ? "" : ChurchRow.DenominationCode);
                Write(ChurchRow.IsAccomodationNull() ? false : ChurchRow.Accomodation);
                Write(ChurchRow.IsAccomodationSizeNull() ? 0 : ChurchRow.AccomodationSize);
                Write(ChurchRow.IsAccomodationTypeNull() ? "" : ChurchRow.AccomodationType);
                Write(ChurchRow.IsApproximateSizeNull() ? 0 : ChurchRow.ApproximateSize);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                PFamilyRow FamilyRow = AMainDS.PFamily[0];
                Write(FamilyRow.IsFamilyNameNull() ? "" : FamilyRow.FamilyName);
                Write(FamilyRow.IsFirstNameNull() ? "" : FamilyRow.FirstName);
                Write(FamilyRow.IsTitleNull() ? "" : FamilyRow.Title);
                Write(FamilyRow.IsMaritalStatusNull() ? "" : FamilyRow.MaritalStatus);
                Write(FamilyRow.IsMaritalStatusSinceNull() ? "?" : FamilyRow.MaritalStatusSince.Value.ToString(DATEFORMAT));
                Write(FamilyRow.IsMaritalStatusCommentNull() ? "" : FamilyRow.MaritalStatusComment);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                PPersonRow PersonRow = AMainDS.PPerson[0];
                Write(PersonRow.IsFamilyNameNull() ? "" : PersonRow.FamilyName);
                Write(PersonRow.IsFirstNameNull() ? "" : PersonRow.FirstName);
                Write(PersonRow.IsMiddleName1Null() ? "" : PersonRow.MiddleName1);
                Write(PersonRow.IsTitleNull() ? "" : PersonRow.Title);
                WriteLine();  // Added Sept 2011 as per spec, and example

                Write(PersonRow.IsDecorationsNull() ? "" : PersonRow.Decorations);
                Write(PersonRow.IsPreferedNameNull() ? "" : PersonRow.PreferedName);
                Write(PersonRow.IsDateOfBirthNull() ? "?" : PersonRow.DateOfBirth.Value.ToString(DATEFORMAT));
                Write(PersonRow.IsGenderNull() ? "" : PersonRow.Gender);
                Write(PersonRow.IsMaritalStatusNull() ? "" : PersonRow.MaritalStatus);
                Write(PersonRow.IsMaritalStatusSinceNull() ? "?" : PersonRow.MaritalStatusSince.Value.ToString(DATEFORMAT));
                Write(PersonRow.IsMaritalStatusCommentNull() ? "" : PersonRow.MaritalStatusComment);
                Write(PersonRow.IsOccupationCodeNull() ? "" : PersonRow.OccupationCode);
                Write(PersonRow.IsFamilyKeyNull() ? 0 : PersonRow.FamilyKey);
                Write(PersonRow.IsFamilyIdNull() ? 0 : PersonRow.FamilyId);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                POrganisationRow OrganisationRow = AMainDS.POrganisation[0];
                Write(OrganisationRow.IsOrganisationNameNull() ? "" : OrganisationRow.OrganisationName);
                Write(OrganisationRow.IsBusinessCodeNull() ? "" : OrganisationRow.BusinessCode);
                Write(OrganisationRow.IsReligiousNull() ? false : OrganisationRow.Religious);
                Write(OrganisationRow.IsFoundationNull() ? false : OrganisationRow.Foundation);
                // spec says TODO: export/import table p_foundation if p_organisation.p_foundation_l is TRUE
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                PUnitRow UnitRow = AMainDS.PUnit[0];
                Write(UnitRow.IsUnitNameNull() ? "" : UnitRow.UnitName);
                Write("");                 // was omss code
                Write(UnitRow.IsOutreachCodeNull() ? "" : UnitRow.OutreachCode);
                Write(UnitRow.IsDescriptionNull() ? "" : UnitRow.Description);
                Write(0);                 // was um_default_entry_conf_key_n
                Write(UnitRow.IsUnitTypeCodeNull() ? "" : UnitRow.UnitTypeCode);
                Write(UnitRow.IsCountryCodeNull() ? "" : UnitRow.CountryCode);
                Write(UnitRow.IsOutreachCostNull() ? 0 : UnitRow.OutreachCost);
                Write(UnitRow.IsOutreachCostCurrencyCodeNull() ? "" : UnitRow.OutreachCostCurrencyCode);
                Write(UnitRow.IsPrimaryOfficeNull() ? 0 : UnitRow.PrimaryOffice);
                WriteLine();
            }
            else if (PartnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_VENUE)
            {
                PVenueRow VenueRow = AMainDS.PVenue[0];
                Write(VenueRow.IsVenueNameNull() ? "" : VenueRow.VenueName);
                Write(VenueRow.IsVenueCodeNull() ? "" : VenueRow.VenueCode);
                Write(VenueRow.IsCurrencyCodeNull() ? "" : VenueRow.CurrencyCode);
                Write(VenueRow.IsContactPartnerKeyNull() ? 0 : VenueRow.ContactPartnerKey);
                WriteLine();
            }

            BestAddressPK = Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(AMainDS);

            //
            // If I have not been given a locationKey, I can pull one out of the LocationTable now,
            // so that the code below works.
            if (ALocationKey == 0)
            {
                ALocationKey = ((PPartnerLocationRow)AMainDS.PPartnerLocation.DefaultView[0].Row).LocationKey;
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

            Boolean FirstAddressWritten = false;

            if (AMainDS.PLocation.DefaultView.Count > 0)
            {
                WriteLocation((PLocationRow)AMainDS.PLocation.DefaultView[0].Row,
                    (PPartnerLocationRow)AMainDS.PPartnerLocation.DefaultView[0].Row, BestAddressPK);
                FirstAddressWritten = true;
            }

            AMainDS.PPartnerLocation.DefaultView.RowFilter = String.Empty;

            foreach (PPartnerLocationRow PartnerLocationRow in AMainDS.PPartnerLocation.Rows)
            {
                if (!((PartnerLocationRow.LocationKey == ALocationKey)
                      && (PartnerLocationRow.SiteKey == ASiteKey))
                    && (PartnerLocationRow.IsDateGoodUntilNull() || (PartnerLocationRow.DateGoodUntil >= DateTime.Today)))
                {
                    if (FirstAddressWritten)
                    {
                        Write("ADDRESS");
                        WriteLine();
                    }

                    AMainDS.PLocation.DefaultView.RowFilter =
                        String.Format("{0}={1} and {2}={3}",
                            PLocationTable.GetSiteKeyDBName(),
                            PartnerLocationRow.SiteKey,
                            PLocationTable.GetLocationKeyDBName(),
                            PartnerLocationRow.LocationKey);

                    WriteLocation((PLocationRow)AMainDS.PLocation.DefaultView[0].Row, PartnerLocationRow, BestAddressPK);
                    FirstAddressWritten = true;
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
                Write(PartnerCommentRow.IsSequenceNull() ? 0 : PartnerCommentRow.Sequence);
                Write(PartnerCommentRow.IsCommentNull() ? "" : PartnerCommentRow.Comment);
                WriteLine();
            }

            foreach (PPartnerTypeRow PartnerTypeRow in AMainDS.PPartnerType.Rows)
            {
                Write("TYPE");
                WriteLine();
                Write(PartnerTypeRow.IsTypeCodeNull() ? "" : PartnerTypeRow.TypeCode);
                Write(PartnerTypeRow.IsValidFromNull() ? "?" : PartnerTypeRow.ValidFrom.Value.ToString(DATEFORMAT));
                Write(PartnerTypeRow.IsValidUntilNull() ? "?" : PartnerTypeRow.ValidUntil.Value.ToString(DATEFORMAT));
                WriteLine();
            }

            foreach (PPartnerAttributeRow PartnerAttributeRow in AMainDS.PPartnerAttribute.Rows)
            {
                Write("PARTNERATTRIBUTE");
                WriteLine();
                Write(PartnerAttributeRow.IsAttributeTypeNull() ? "" : PartnerAttributeRow.AttributeType);
                Write(PartnerAttributeRow.IsIndexNull() ? 0 : PartnerAttributeRow.Index);
                Write(PartnerAttributeRow.IsValueNull() ? "" : PartnerAttributeRow.Value);
                Write(PartnerAttributeRow.IsCommentNull() ? "" : PartnerAttributeRow.Comment);
                Write(PartnerAttributeRow.IsPrimaryNull() ? false : PartnerAttributeRow.Primary);
                Write(PartnerAttributeRow.IsWithinOrganisationNull() ? false : PartnerAttributeRow.WithinOrganisation);
                Write(PartnerAttributeRow.IsSpecialisedNull() ? false : PartnerAttributeRow.Specialised);
                Write(PartnerAttributeRow.IsConfidentialNull() ? false : PartnerAttributeRow.Confidential);
                Write(PartnerAttributeRow.IsCurrentNull() ? false : PartnerAttributeRow.Current);
                Write(PartnerAttributeRow.IsNoLongerCurrentFromNull() ? "?" : PartnerAttributeRow.NoLongerCurrentFrom.Value.ToString(DATEFORMAT));
                WriteLine();
            }

            foreach (PPartnerInterestRow PartnerInterestRow in AMainDS.PPartnerInterest.Rows)
            {
                AMainDS.PInterest.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    PInterestTable.GetInterestDBName(),
                    PartnerInterestRow.Interest);
                PInterestRow tempRow = (PInterestRow)AMainDS.PInterest.DefaultView[0].Row;

                Write("INTEREST");
                WriteLine();
                Write(PartnerInterestRow.IsInterestNumberNull() ? 0 : PartnerInterestRow.InterestNumber);
                Write(PartnerInterestRow.IsFieldKeyNull() ? 0 : PartnerInterestRow.FieldKey);
                Write(PartnerInterestRow.IsCountryNull() ? "" : PartnerInterestRow.Country);
                Write(PartnerInterestRow.IsInterestNull() ? "" : PartnerInterestRow.Interest);
                Write(tempRow.IsCategoryNull() ? "" : tempRow.Category);
                Write(PartnerInterestRow.IsLevelNull() ? 0 : PartnerInterestRow.Level);
                Write(PartnerInterestRow.IsCommentNull() ? "" : PartnerInterestRow.Comment);
                WriteLine();
            }

            foreach (PPartnerGiftDestinationRow GiftDestinationRow in AMainDS.PPartnerGiftDestination.Rows)
            {
                Write("GIFTDESTINATION");
                WriteLine();
                Write(GiftDestinationRow.IsFieldKeyNull() ? 0 : GiftDestinationRow.FieldKey);
                Write(GiftDestinationRow.IsDateEffectiveNull() ? "?" : GiftDestinationRow.DateEffective.ToString(DATEFORMAT));
                Write(GiftDestinationRow.IsDateExpiresNull() ? "?" : GiftDestinationRow.DateExpires.Value.ToString(DATEFORMAT));
                Write(GiftDestinationRow.IsActiveNull() ? false : GiftDestinationRow.Active);
                Write(GiftDestinationRow.IsDefaultGiftDestinationNull() ? false : GiftDestinationRow.DefaultGiftDestination);
                Write(GiftDestinationRow.IsPartnerClassNull() ? "" : GiftDestinationRow.PartnerClass);
                Write(GiftDestinationRow.IsCommentNull() ? "" : GiftDestinationRow.Comment);
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