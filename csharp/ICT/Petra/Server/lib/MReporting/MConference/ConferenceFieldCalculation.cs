//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Ict.Common;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Description of ConferenceFieldCalculation.
    /// </summary>
    public class TConferenceFieldCalculation
    {
        private enum TConferenceCostTypeEnum
        {
            cctPerDay, cctPerNight, cctPerOutreach
        };

        #region member variables
        private long FConferenceKey;
        private TConferenceCostTypeEnum FConferenceCostType;

        // Values for the different discount types
        private decimal FParticipantDiscountAccommodationPre;
        private decimal FParticipantDiscountConferencePre;
        private decimal FVolunteerDiscountAccommodationPre;
        private decimal FVolunteerDiscountAccommodationConference;
        private decimal FVolunteerDiscountConferencePre;
        private decimal FVolunteerDiscountConferenceConference;
        private decimal FRoleDiscountAccommodationPre;
        private decimal FRoleDiscountAccommodationConference;
        private decimal FRoleDiscountConferencePre;
        private decimal FRoleDiscountConferenceConference;
        private decimal FRoleDiscountConferencePost;

        // this does not seem to be used anywhere, see Mantis #412
        // private decimal FRoleDiscountAccommodationPost;

        private decimal FConferenceDayRate;
        private decimal FConferenceRate;

        private String FConferenceCurrency;

        private int FConferenceDays;
        private int FAttendeeDays;

        private DateTime FConferenceStartDate;
        private DateTime FConferenceEndDate;
        private DateTime FAttendeeStartDate;
        private DateTime FAttendeeEndDate;

        private decimal FSupportCost;
        private decimal FExtraCost;

        private decimal FCongressCosts;
        private decimal FOutreachCosts;
        private decimal FAccommodationCosts;
        private decimal FPreAccommodationCosts;
        private decimal FPreConferenceCosts;
        private decimal FPostAccommodationCosts;
        private decimal FPostConferenceCosts;

        // These values define where to add the result
        private bool FIsCongressVolunteer;
        private bool FIsCongressRole;
        private bool FIsCongressOnly;
        private bool FIsOutreachOnly;

        // store in this string the flags like 'P' PreConference, 'T' PostConference, 'C' Child, 'O' Omer, 'E' Early, 'L' Late
        private String FConferenceFlags;
        private String FUsedConferenceFlags;
        private DataTable FResultDataTable;

        private bool FExtraCostLinePrinted;
        private int FNumExtraCostLines;
        private int FInvisibleRowCounter;
        private int FMaxNumColumns;

        // store in this member the index of the master row where the results of on field needs to go
        private int FMasterRowIndex;
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AConferenceKey"></param>
        public TConferenceFieldCalculation(ref TRptSituation ASituation, long AConferenceKey)
        {
            FConferenceKey = AConferenceKey;
            FInvisibleRowCounter = 0;

            int NumColumns = ASituation.GetParameters().GetParameter("MaxDisplayColumns").value.ToInt();

            FMaxNumColumns = 15;

            if (FMaxNumColumns < NumColumns)
            {
                FMaxNumColumns = NumColumns;
            }

            InitResultDataTable(ref ASituation, AConferenceKey);

            DetermineConferenceCostCharges(ref ASituation, AConferenceKey);
            DetermineConferenceCostType(ref ASituation, AConferenceKey);
            DetermineConferenceDate(ref ASituation, AConferenceKey);
            DetermineConferenceDiscounts(ref ASituation, AConferenceKey);

            FExtraCostLinePrinted = false;
        }

        #region public methods
        /// <summary>
        /// Calculates the financial details of one attendee and stores the value in FResultTable
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AAge"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AApplicationKey"></param>
        /// <param name="ARegistrationOfficeKey"></param>
        /// <param name="AHomeOfficeKey"></param>
        /// <param name="AOutreachType"></param>
        /// <param name="ARegistrationDate"></param>
        /// <param name="AFinanceDetails">Returns the conference costs</param>
        /// <param name="AAccommodation">Returns the accommodation costs</param>
        /// <returns></returns>
        public bool CalculateOneAttendeeFieldCost(ref TRptSituation ASituation, int AAge, long APartnerKey,
            int AApplicationKey, long ARegistrationOfficeKey,
            long AHomeOfficeKey,
            String AOutreachType, DateTime ARegistrationDate,
            out String AFinanceDetails, out String AAccommodation)
        {
            FCongressCosts = 0;
            FExtraCost = 0;
            FSupportCost = 0;
            FPreConferenceCosts = 0;
            FPostConferenceCosts = 0;
            FAccommodationCosts = 0;
            FPreAccommodationCosts = 0;
            FPostAccommodationCosts = 0;

            FIsCongressVolunteer = false;
            FIsCongressRole = false;
            FIsCongressOnly = false;
            FIsOutreachOnly = false;

            decimal ChildDiscount;
            decimal ChildDiscountAccommodation;

            bool InPercent;
            bool GeneralDiscountApplied = false;

            AFinanceDetails = "0.00";
            AAccommodation = "0.00";
            FConferenceFlags = " ";

            if (AAge < 0)
            {
                AAge = 0;
            }

            CheckPartnerTypeCode(APartnerKey, ref ASituation);

            PmShortTermApplicationTable ShortTermerTable;
            ShortTermerTable = PmShortTermApplicationAccess.LoadByPrimaryKey(APartnerKey, AApplicationKey,
                ARegistrationOfficeKey, ASituation.GetDatabaseConnection().Transaction);

            if (ShortTermerTable.Rows.Count < 1)
            {
                return false;
            }

            PmShortTermApplicationRow ShortTermerRow = (PmShortTermApplicationRow)ShortTermerTable.Rows[0];

            if (ShortTermerRow.StPreCongressCode == "")
            {
                ShortTermerRow.StPreCongressCode = ShortTermerRow.StCongressCode;
            }

            DateTime ArrivalDate = FConferenceStartDate;
            DateTime DepartureDate = FConferenceEndDate;

            if (!ShortTermerRow.IsArrivalNull())
            {
                ArrivalDate = ShortTermerRow.Arrival.Value;
            }

            if (!ShortTermerRow.IsDepartureNull())
            {
                DepartureDate = ShortTermerRow.Departure.Value;
            }

            FAttendeeDays = DepartureDate.Subtract(ArrivalDate).Days + 1;

            TAccommodationReportCalculation.GetChildDiscount(AAge, FConferenceKey, "CONFERENCE", out ChildDiscount, out InPercent, ref ASituation);

            if (!InPercent)
            {
                ChildDiscount = 0;
            }

            TAccommodationReportCalculation.GetChildDiscount(AAge,
                FConferenceKey,
                "ACCOMMODATION",
                out ChildDiscountAccommodation,
                out InPercent,
                ref ASituation);

            if (!InPercent)
            {
                ChildDiscountAccommodation = 0;
            }

            if ((ChildDiscountAccommodation > 0)
                || (ChildDiscount > 0))
            {
                FConferenceFlags = FConferenceFlags + "C";
            }

            decimal UsedConferenceDiscountPre = FParticipantDiscountConferencePre;
            decimal UsedAccommodationDiscountPre = FParticipantDiscountAccommodationPre;

            if (IsAttendeeARole(ref ASituation, ShortTermerRow.StCongressCode))
            {
                UsedConferenceDiscountPre = FRoleDiscountConferencePre;
                UsedAccommodationDiscountPre = FRoleDiscountAccommodationPre;
                FIsCongressRole = true;

                if (FRoleDiscountConferencePre != 0)
                {
                    GeneralDiscountApplied = true;
                }
            }

            DetermineConferenceBasicCharges(ref ASituation, ref ShortTermerRow, AOutreachType);

            DetermineOutreachSupplements(ref ASituation, AOutreachType, ShortTermerRow.StCongressCode, ChildDiscount);

            DetermineExtraCosts(ref ASituation, ShortTermerRow.PartnerKey);

            DetermineAccommodationCosts(ref ASituation, ShortTermerRow.PartnerKey);

            // ************************************************************
            // ************************************************************
            // * Now do the calculations
            // ************************************************************
            // ************************************************************

            if (ShortTermerRow.IsArrivalNull())
            {
                ShortTermerRow.Arrival = FConferenceStartDate;
            }

            if (ShortTermerRow.IsDepartureNull())
            {
                ShortTermerRow.Departure = FConferenceEndDate;
            }

            int CalculationDays = 0;

            if (FConferenceCostType != TConferenceCostTypeEnum.cctPerOutreach)
            {
                /*
                 * Calculations when charge by the day for all
                 * Note: volunteers and short stays override this calculation
                 * Note: this does not include pre and post congress stays
                 */
                CalculationDays = GetDaysAtCongress(ref ASituation, ShortTermerRow,
                    ShortTermerRow.Arrival.Value,
                    ShortTermerRow.Departure.Value);

                if (FConferenceCostType == TConferenceCostTypeEnum.cctPerDay)
                {
                    CalculationDays++;
                }

                FCongressCosts = CalculationDays * FConferenceDayRate * (100 - ChildDiscount) / 100;

                if (FIsCongressRole)
                {
                    FCongressCosts = FCongressCosts * (100 - FRoleDiscountConferenceConference) / 100;

                    if (FRoleDiscountConferenceConference != 0)
                    {
                        GeneralDiscountApplied = true;
                    }
                }

                FOutreachCosts = 0;
            }
            else
            {
                /*
                 * Calculations when charging by outreach length
                 * Note: volunteers and short stays override this calculation
                 * Note: this does not include pre and post congress stays
                 */
                decimal TmpCost = GetOutreachCost(ref ASituation);

                FCongressCosts = FConferenceRate * (100 - ChildDiscount) / 100;

                if (FIsCongressRole)
                {
                    FCongressCosts = FConferenceRate *
                                     (100 - FRoleDiscountConferenceConference) / 100;

                    if (FRoleDiscountConferenceConference != 0)
                    {
                        GeneralDiscountApplied = true;
                    }

                    // No child discount for outreach
                    FOutreachCosts = (TmpCost - FConferenceRate) *
                                     (100 - FRoleDiscountConferencePost) / 100;

                    if (FRoleDiscountConferencePost != 0)
                    {
                        GeneralDiscountApplied = true;
                    }
                }
            }

            if (ShortTermerRow.StOutreachOnlyFlag)
            {
                // Reset when outreach only
                FCongressCosts = 0;
            }

            if (ShortTermerRow.StCongressCode == "VOL")
            {
                /*
                 * Volunteers during the congress
                 * NOTE: will override an above calculation for the congress charge
                 * Note: this does not include pre and post congress stays
                 */
                FIsCongressVolunteer = true;
                int Nights = GetDaysAtCongress(ref ASituation, ShortTermerRow, ShortTermerRow.Arrival.Value,
                    ShortTermerRow.Departure.Value);

                if (FConferenceCostType == TConferenceCostTypeEnum.cctPerDay)
                {
                    Nights++;
                }

                FCongressCosts = Nights * FConferenceDayRate *
                                 (100 - FVolunteerDiscountConferenceConference) / 100 *
                                 (100 - ChildDiscount) / 100;

                FAccommodationCosts = FAccommodationCosts *
                                      (100 - FVolunteerDiscountAccommodationConference) / 100 *
                                      (100 - ChildDiscountAccommodation) / 100;
            }
            else
            {
                FAccommodationCosts = FAccommodationCosts * (100 - ChildDiscountAccommodation) / 100;

                if (FIsCongressRole)
                {
                    FAccommodationCosts = FAccommodationCosts * (100 - FRoleDiscountAccommodationConference) / 100;
                }
            }

            if (ShortTermerRow.Arrival.Value.CompareTo(FAttendeeStartDate) < 0)
            {
                /*
                 * Pre-conference charging, which is always by the day *
                 * Note: short stays override this calculation
                 */

                CalculationDays = GetRoomAllocatedNightsBetween(ref ASituation, ShortTermerRow.PartnerKey,
                    ShortTermerRow.Arrival.Value, FAttendeeStartDate);

                if ((FConferenceCostType == TConferenceCostTypeEnum.cctPerDay)
                    && (ShortTermerRow.Departure.Value.CompareTo(FAttendeeStartDate) < 0))
                {
                    /* a day only needs to be added if the person leaves before the conference
                     * starts, otherwise the daily charge is taken into consideration with the
                     * conference days
                     */
                    CalculationDays++;
                }

                if (ShortTermerRow.StPreCongressCode == "VOL")
                {
                    // Volunteers potentially have a different dscount rate
                    FPreConferenceCosts = CalculationDays * FConferenceDayRate *
                                          (100 - ChildDiscount) / 100 *
                                          (100 - FVolunteerDiscountConferencePre) / 100;

                    FPreAccommodationCosts = FPreAccommodationCosts *
                                             (100 - ChildDiscountAccommodation) / 100 *
                                             (100 - FVolunteerDiscountAccommodationPre) / 100;
                }
                else
                {
                    // everyone else
                    FPreConferenceCosts = CalculationDays * FConferenceDayRate *
                                          (100 - ChildDiscount) / 100 *
                                          (100 - UsedConferenceDiscountPre) / 100;

                    FPreAccommodationCosts = FPreAccommodationCosts *
                                             (100 - ChildDiscountAccommodation) / 100 *
                                             (100 - UsedAccommodationDiscountPre) / 100;
                }

                FConferenceFlags = FConferenceFlags + "P";
            }             // End of pre conference calculation

            if (ShortTermerRow.Departure.Value.CompareTo(FConferenceEndDate) > 0)
            {
                /* Post-conference charging, which is always by the day
                 * For Conference costs only relevent for Congress only people
                 * Note: short stays override this calculation
                 */

                if (FIsCongressOnly)
                {
                    CalculationDays = GetRoomAllocatedNightsBetween(ref ASituation, ShortTermerRow.PartnerKey,
                        FConferenceEndDate, ShortTermerRow.Departure.Value);

                    if ((FConferenceCostType == TConferenceCostTypeEnum.cctPerDay)
                        && (ShortTermerRow.Arrival.Value.CompareTo(FConferenceEndDate) > 0))
                    {
                        /* a day only needs to be added if the person arrives after the conference
                         * ends, otherwise the daily charge is taken into consideration with the
                         * conference days
                         */
                        CalculationDays++;
                    }

                    if (ShortTermerRow.StCongressCode == "VOL")
                    {
                        FPostConferenceCosts = CalculationDays * FConferenceDayRate *
                                               (100 - ChildDiscount) / 100 *
                                               (100 - FVolunteerDiscountConferenceConference) / 100;
                    }
                    else
                    {
                        FPostConferenceCosts = CalculationDays * FConferenceDayRate * (100 - ChildDiscount) / 100;

                        if (FIsCongressRole)
                        {
                            FPostConferenceCosts = FPostConferenceCosts * (100 - FRoleDiscountConferenceConference) / 100;

                            if (FRoleDiscountConferenceConference != 0)
                            {
                                GeneralDiscountApplied = true;
                            }
                        }
                    }
                }

                if (ShortTermerRow.StCongressCode == "VOL")
                {
                    FPostAccommodationCosts = FPostAccommodationCosts *
                                              (100 - ChildDiscount) / 100 *
                                              (100 - FVolunteerDiscountAccommodationConference) / 100;
                }
                else
                {
                    FPostAccommodationCosts = FPostAccommodationCosts * (100 - ChildDiscount) / 100;

                    if (FIsCongressRole)
                    {
                        FPostAccommodationCosts = FPostAccommodationCosts * (100 - FRoleDiscountAccommodationConference) / 100;
                    }
                }
            }

            /*
             * Short stay.   NOTE: overrides all the above charges
             * NOTE: will override an above calculation for the congress charge
             */
            if (FConferenceCostType == TConferenceCostTypeEnum.cctPerOutreach)
            {
                // consider short stay only if it is not on a daily basis
                if (((ShortTermerRow.Departure.Value.Subtract(ShortTermerRow.Arrival.Value).Days * 2) <= FConferenceDays)
                    && (ShortTermerRow.StCongressCode != "VOL")
                    && (!ShortTermerRow.StOutreachOnlyFlag))
                {
                    FCongressCosts = ShortTermerRow.Departure.Value.Subtract(ShortTermerRow.Arrival.Value).Days * FConferenceDayRate *
                                     (100 - ChildDiscount) / 100;

                    if (FIsCongressRole)
                    {
                        FCongressCosts = FCongressCosts * (100 - FRoleDiscountConferenceConference) / 100;

                        if (FRoleDiscountConferenceConference != 0)
                        {
                            GeneralDiscountApplied = true;
                        }
                    }
                }
            }

            // Early and late booking charges
            decimal EarlyAmount, LateAmount;
            int EarlyPercent, LatePercent;
            GetEarlyLateCharges(ref ASituation, ARegistrationDate, ShortTermerRow, out EarlyPercent, out EarlyAmount,
                out LatePercent, out LateAmount);

            FCongressCosts = FCongressCosts - (EarlyAmount * (100 - ChildDiscount) / 100);
            FCongressCosts = FCongressCosts * (100 - EarlyPercent) / 100;
            FCongressCosts = FCongressCosts - (LateAmount * (100 - ChildDiscount) / 100);
            FCongressCosts = FCongressCosts * (100 - LatePercent) / 100;

            // Clear any negative values
            if (FCongressCosts < 0)
            {
                FCongressCosts = 0;
            }

            if (FOutreachCosts < 0)
            {
                FOutreachCosts = 0;
            }

            AFinanceDetails = StringHelper.FormatUsingCurrencyCode(
                (decimal)(FOutreachCosts + FCongressCosts + FExtraCost + FSupportCost + FPreConferenceCosts + FPostConferenceCosts),
                FConferenceCurrency);
            AAccommodation = StringHelper.FormatUsingCurrencyCode(
                (decimal)(FAccommodationCosts + FPreAccommodationCosts + FPostAccommodationCosts),
                FConferenceCurrency);

            AFinanceDetails = AFinanceDetails + FConferenceFlags;
            FUsedConferenceFlags = FUsedConferenceFlags + FConferenceFlags;

            if (GeneralDiscountApplied)
            {
                AFinanceDetails = AFinanceDetails + "*";
                FUsedConferenceFlags = FUsedConferenceFlags + "*";
            }

            String FieldName = TAccommodationReportCalculation.GetPartnerShortName(AHomeOfficeKey, ref ASituation);

            if ((AHomeOfficeKey == 0)
                || (FieldName == ""))
            {
                FieldName = "PARTNERS WITH NO FIELD";
            }

            AddCalculationsToResultTable(FieldName, AAge, ShortTermerRow, ref ASituation);

            AddCalculationsToResultTable("REPORT SUMMARY", AAge, ShortTermerRow, ref ASituation);

            return true;
        }

        /// <summary>
        /// Generates the financial lines in the report for each field
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        public bool PrintFieldFinancialCosts(ref TRptSituation ASituation, String AFieldName)
        {
            bool HeaderAdded = false;

            TExtentedSituation ExtentedSituation = new TExtentedSituation(ASituation);
            int ChildRowIndex = ExtentedSituation.GetRunningCode();

            int NumberOfEntriesPerField = GetNumberOfEntriesPerField(AFieldName) + FInvisibleRowCounter;

            if (FExtraCostLinePrinted)
            {
                FMasterRowIndex = ChildRowIndex - NumberOfEntriesPerField * 2 - FNumExtraCostLines - 1;
            }
            else
            {
                FMasterRowIndex = ChildRowIndex - NumberOfEntriesPerField - 1;
            }

            if (AFieldName == "REPORT SUMMARY")
            {
                FMasterRowIndex = ChildRowIndex - 1;
            }

            foreach (DataRow ResultRow in FResultDataTable.Rows)
            {
                if (AFieldName == (String)ResultRow["Field"])
                {
                    if (!HeaderAdded)
                    {
                        AddNewFieldNameToResults(AFieldName, FMasterRowIndex, 2, ChildRowIndex, ref ASituation);

                        ChildRowIndex = ChildRowIndex + 3;
                        HeaderAdded = true;
                    }

                    AddFinancialRowToResults(ResultRow, FMasterRowIndex, 2, ChildRowIndex++, ref ASituation);
                }
            }

            AddEmptyRowToResults(ref ASituation, FMasterRowIndex, 2, ChildRowIndex++);

            ExtentedSituation.SetRunningCode(ChildRowIndex);

            FNumExtraCostLines = 0;

            return true;
        }

        /// <summary>
        /// Generates the financial sign off lines for each field in the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        public bool PrintFinancialSignOffLines(ref TRptSituation ASituation, String AFieldName)
        {
            TExtentedSituation ExtentedSituation = new TExtentedSituation(ASituation);
            int ChildRowIndex = ExtentedSituation.GetRunningCode();

            AddFinancialSignOffLines(ref ASituation, FMasterRowIndex, 1, ChildRowIndex, AFieldName);

            // 21 rows were added
            ExtentedSituation.SetRunningCode(ChildRowIndex + 21);
            return true;
        }

        /// <summary>
        /// Generates the attendee sign off lines for each field in the report.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        public bool PrintAttendanceSignOffLines(ref TRptSituation ASituation, String AFieldName)
        {
            TExtentedSituation ExtentedSituation = new TExtentedSituation(ASituation);
            int ChildRowIndex = ExtentedSituation.GetRunningCode();

            AddAttendanceSignOffLines(ref ASituation, FMasterRowIndex, 1, ChildRowIndex, AFieldName);

            // 21 rows were added
            ExtentedSituation.SetRunningCode(ChildRowIndex + 21);
            return true;
        }

        /// <summary>
        /// Prints an empty line to the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        public bool PrintEmptyLineInFieldReport(ref TRptSituation ASituation)
        {
            TExtentedSituation ExtentedSituation = new TExtentedSituation(ASituation);
            int ChildRowIndex = ExtentedSituation.GetRunningCode();

            //AddEmptyRowToResults(ref ASituation, FMasterRowIndex, 2, ChildRowIndex++);
            AddEmptyRowToResults(ref ASituation, ChildRowIndex - 1, 1, ChildRowIndex);

            ++ChildRowIndex;

            ExtentedSituation.SetRunningCode(ChildRowIndex);

            return true;
        }

        /// <summary>
        /// Generates the extra cost lines for the attendees in the report.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AConferenceKey"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public bool GetExtraCosts(ref TRptSituation ASituation, long AConferenceKey, long APartnerKey)
        {
            TExtentedSituation ExtentedSituation = new TExtentedSituation(ASituation);
            int ChildRowIndex = ExtentedSituation.GetRunningCode();

            int numLines = AddExtraCostLines(ref ASituation, AConferenceKey, APartnerKey,
                ChildRowIndex - 2, 3, ChildRowIndex);

            ExtentedSituation.SetRunningCode(ChildRowIndex + numLines);

            if (numLines > 0)
            {
                FNumExtraCostLines = FNumExtraCostLines + numLines;
            }

            FExtraCostLinePrinted = true;
            return true;
        }

        /// <summary>
        /// Checks if there is any entry with this field
        /// </summary>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        public bool HasReportSendingField(String AFieldName)
        {
            foreach (DataRow Row in FResultDataTable.Rows)
            {
                if ((String)Row["Field"] == AFieldName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Evaluates if a partner has a receiving field assigned.
        /// If there is a receiving field the FInvisibleRowCounter is increased because in the Receiving
        /// Field Report this partner is not displayed in the "NO FIELD" list.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ATargetFieldList"></param>
        /// <returns>True if there is a receiving field. Otherwise false.</returns>
        public bool HasAttendeeReceivingField(ref TRptSituation ASituation, long APartnerKey,
            String ATargetFieldList)
        {
            PPersonTable PersonTable;
            bool ReturnValue = false;
            PmStaffDataTable StaffDataTable = new PmStaffDataTable();
            PmStaffDataRow TemplateRow = (PmStaffDataRow)StaffDataTable.NewRow();

            TemplateRow.PartnerKey = APartnerKey;

            // 1. check the staff data if this partner has the same target / receiving field
            StaffDataTable = PmStaffDataAccess.LoadUsingTemplate(TemplateRow, ASituation.GetDatabaseConnection().Transaction);

            foreach (PmStaffDataRow StaffDataRow in StaffDataTable.Rows)
            {
                if ((StaffDataRow.StartOfCommitment < DateTime.Today)
                    && (StaffDataRow.IsEndOfCommitmentNull()
                        || (!StaffDataRow.IsEndOfCommitmentNull()
                            && (StaffDataRow.EndOfCommitment > DateTime.Today)))
                    && !StaffDataRow.IsReceivingFieldNull())
                {
                    if (ATargetFieldList.Contains(StaffDataRow.ReceivingField.ToString()))
                    {
                        ReturnValue = true;
                    }
                }
            }

            // 2. check the person gift destination
            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            if ((ReturnValue == false)
                && (PersonTable.Rows.Count > 0))
            {
                PPersonRow PersonRow = (PPersonRow)PersonTable.Rows[0];
                
                PPartnerGiftDestinationTable GiftDestinationTable = 
                	PPartnerGiftDestinationAccess.LoadViaPPartner(PersonRow.FamilyKey, ASituation.GetDatabaseConnection().Transaction);
                
                if (GiftDestinationTable.Rows.Count > 0)
                {
	                foreach (PPartnerGiftDestinationRow Row in GiftDestinationTable.Rows)
	                {
	                	// check if the gift destination is currently active
	                	if (Row.DateEffective <= DateTime.Today
	                	    && (Row.IsDateExpiresNull() || (Row.DateExpires >= DateTime.Today && Row.DateExpires != Row.DateEffective)))
	                	{
	                		ReturnValue = true;
	                	}
	                }
                }
            }

            if (ReturnValue == true)
            {
                // Hide the line from the output
                FInvisibleRowCounter++;
                ASituation.GetParameters().Add("DONTDISPLAYROW", new TVariant(true), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }
            else
            {
                ASituation.GetParameters().Add("DONTDISPLAYROW", new TVariant(false), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if a special discount type is used in the field report.
        /// </summary>
        /// <param name="ADiscountType">One character which represents the discount type</param>
        /// <returns></returns>
        public bool HasFieldReportDiscount(String ADiscountType)
        {
            if (FUsedConferenceFlags != null)
            {
                return FUsedConferenceFlags.Contains(ADiscountType);
            }

            return false;
        }

        #endregion

        # region Get basic conference data

        /// <summary>
        /// Determine the conference cost charges. Either per single day or per conference
        /// </summary>
        /// <param name="ASituation">Current report situation. Used to get a database transacion</param>
        /// <param name="AConferenceKey">Unique partner key of the conference</param>
        /// <returns>true</returns>
        private bool DetermineConferenceCostCharges(ref TRptSituation ASituation, long AConferenceKey)
        {
            PcConferenceCostTable ConferenceCostTable;

            ConferenceCostTable = PcConferenceCostAccess.LoadByPrimaryKey(AConferenceKey, 1, ASituation.GetDatabaseConnection().Transaction);

            if (ConferenceCostTable.Rows.Count > 0)
            {
                FConferenceDayRate = (decimal)ConferenceCostTable.Rows[0][PcConferenceCostTable.GetChargeDBName()];
            }
            else
            {
                FConferenceDayRate = 0;
            }

            return true;
        }

        /// <summary>
        /// Determine the conference cost type. Either per day, per night, by outreach.
        /// </summary>
        /// <param name="ASituation">Current report situation. Used to get a database transacion</param>
        /// <param name="AConferenceKey">Unique partner key of the conference</param>
        /// <returns>true</returns>
        private bool DetermineConferenceCostType(ref TRptSituation ASituation, long AConferenceKey)
        {
            PcConferenceOptionTable ConferenceOptionTable;

            ConferenceOptionTable = PcConferenceOptionAccess.LoadViaPcConference(AConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            FConferenceCostType = TConferenceCostTypeEnum.cctPerOutreach;

            foreach (DataRow Row in ConferenceOptionTable.Rows)
            {
                if ((String)Row[PcConferenceOptionTable.GetOptionTypeCodeDBName()] == "COST_PER_DAY")
                {
                    FConferenceCostType = TConferenceCostTypeEnum.cctPerDay;
                    break;
                }
                else if ((String)Row[PcConferenceOptionTable.GetOptionTypeCodeDBName()] == "COST_PER_NIGHT")
                {
                    FConferenceCostType = TConferenceCostTypeEnum.cctPerNight;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the start and end date of the conference.
        /// </summary>
        /// <param name="ASituation">Current report situation. Used to get a database transacion</param>
        /// <param name="AConferenceKey">Unique partner key of the conference</param>
        /// <returns>true if succussful</returns>
        private bool DetermineConferenceDate(ref TRptSituation ASituation, long AConferenceKey)
        {
            PcConferenceTable ConferenceTable;

            ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(AConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            if (ConferenceTable.Rows.Count > 0)
            {
                PcConferenceRow Row = (PcConferenceRow)ConferenceTable.Rows[0];

                if (Row.IsStartNull() || Row.IsEndNull())
                {
                    TLogging.Log("Can't get start or end date of conference: " + AConferenceKey.ToString());
                    return false;
                }

                FConferenceStartDate = Row.Start.Value;
                FConferenceEndDate = Row.End.Value;
                FConferenceDays = FConferenceEndDate.Subtract(FConferenceStartDate).Days + 1;

                FConferenceCurrency = Row.CurrencyCode;
            }

            return true;
        }

        /// <summary>
        /// Gets the different discounts for this conference
        /// (Volunteer, Omer, Pre, Post)
        /// </summary>
        /// <param name="ASituation">Current report situation. Used to get a database transacion</param>
        /// <param name="AConferenceKey">Unique partner key of the conference</param>
        /// <returns>true if succussful</returns>
        private bool DetermineConferenceDiscounts(ref TRptSituation ASituation, long AConferenceKey)
        {
            PcDiscountTable DiscountTable;

            FParticipantDiscountAccommodationPre = 0;
            FParticipantDiscountConferencePre = 0;
            FRoleDiscountAccommodationConference = 0;
            FRoleDiscountAccommodationPre = 0;
            FRoleDiscountConferenceConference = 0;
            FRoleDiscountConferencePost = 0;
            FRoleDiscountConferencePre = 0;
            FVolunteerDiscountAccommodationConference = 0;
            FVolunteerDiscountAccommodationPre = 0;
            FVolunteerDiscountConferenceConference = 0;
            FVolunteerDiscountConferencePre = 0;

            // this does not seem to be used anywhere, see Mantis #412
            // FRoleDiscountAccommodationPost = 0;

            DiscountTable = PcDiscountAccess.LoadViaPcConference(AConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcDiscountRow Row in DiscountTable.Rows)
            {
                if (Row.DiscountCriteriaCode == "ROLE")
                {
                    if (Row.CostTypeCode == "CONFERENCE")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FRoleDiscountConferencePre = Row.Discount;
                        }
                        else if (Row.Validity == "CONF")
                        {
                            FRoleDiscountConferenceConference = Row.Discount;
                        }
                        else if (Row.Validity == "POST")
                        {
                            FRoleDiscountConferencePost = Row.Discount;
                        }
                    }
                    else if (Row.CostTypeCode == "ACCOMMODATION")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FRoleDiscountAccommodationPre = Row.Discount;
                        }
                        else if (Row.Validity == "CONF")
                        {
                            FRoleDiscountAccommodationConference = Row.Discount;
                        }
                        else if (Row.Validity == "POST")
                        {
                            // this does not seem to be used anywhere, see Mantis #412
                            // FRoleDiscountAccommodationPost = Row.Discount;
                        }
                    }
                }
                else if (Row.DiscountCriteriaCode == "VOL")
                {
                    if (Row.CostTypeCode == "CONFERENCE")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FVolunteerDiscountConferencePre = Row.Discount;
                        }
                        else if (Row.Validity == "CONF")
                        {
                            FVolunteerDiscountConferenceConference = Row.Discount;
                        }
                    }
                    else if (Row.CostTypeCode == "ACCOMMODATION")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FVolunteerDiscountAccommodationPre = Row.Discount;
                        }
                        else if (Row.Validity == "CONF")
                        {
                            FVolunteerDiscountAccommodationConference = Row.Discount;
                        }
                    }
                }
                else if (Row.DiscountCriteriaCode == "OTHER")
                {
                    if (Row.CostTypeCode == "CONFERENCE")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FParticipantDiscountConferencePre = Row.Discount;
                        }
                    }
                    else if (Row.CostTypeCode == "ACCOMMODATION")
                    {
                        if (Row.Validity == "PRE")
                        {
                            FParticipantDiscountAccommodationPre = Row.Discount;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AShortTermerRow">The row of the attendee from the personnel short termer table</param>
        /// <param name="AOutreachType"></param>
        /// <returns></returns>
        private bool DetermineConferenceBasicCharges(ref TRptSituation ASituation, ref PmShortTermApplicationRow AShortTermerRow,
            String AOutreachType)
        {
            FAttendeeStartDate = FConferenceStartDate;
            FAttendeeEndDate = FConferenceEndDate;

            PcConferenceCostTable ConferenceCostTable;

            ConferenceCostTable = PcConferenceCostAccess.LoadByPrimaryKey(FConferenceKey, FAttendeeDays,
                ASituation.GetDatabaseConnection().Transaction);

            if (ConferenceCostTable.Rows.Count > 0)
            {
                FConferenceRate = ((PcConferenceCostRow)ConferenceCostTable.Rows[0]).Charge;
            }
            else
            {
                FConferenceRate = 0;
            }

            //Find the outreach length for this individual
            // use the confirmed option code (the last two characters should give the number of days
            FAttendeeDays = 0;
            long OutreachOption = 0;
            FIsCongressOnly = false;

            FAttendeeDays = GetConferenceLengthFromConferenceCode(AShortTermerRow.ConfirmedOptionCode);

            if (FAttendeeDays > 0)
            {
                OutreachOption = AShortTermerRow.StConfirmedOption;
            }

            if (FAttendeeDays == 0)
            {
                // None of the options has given us a valid number of days so start with the conference length
                FAttendeeDays = FConferenceDays;
                OutreachOption = FConferenceKey;
            }

            if (FIsCongressOnly
                && (AOutreachType != "CNGRSS"))
            {
                GetCongressOptionRates(ref ASituation, OutreachOption, ref AShortTermerRow);
            }

            return true;
        }

        # endregion

        #region Get Applicant specific conference data

        /// <summary>
        /// Returns the length of the conference based on the outreach code
        /// </summary>
        /// <param name="AOutreachCode"></param>
        /// <returns></returns>
        private int GetConferenceLengthFromConferenceCode(String AOutreachCode)
        {
            int NumberOfDays = 0;

            FIsCongressOnly = false;

            if (AOutreachCode.Length >= 13)
            {
                try
                {
                    NumberOfDays = Convert.ToInt32(AOutreachCode.Substring(11, 2));
                }
                catch
                {
                    NumberOfDays = 0;
                }
            }

            if ((AOutreachCode.Length >= 8)
                && (AOutreachCode.Substring(5, 3) == "CNG"))
            {
                FIsCongressOnly = true;
            }

            return NumberOfDays;
        }

        /// <summary>
        /// Congress option within a main congress which is charged at fixed rate
        /// Particularly applicable to De Bron when Staff and Ladies occur
        /// The conference rate and arrival/departure need to be adjusted
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AOutreachOption">The unit key of the outreach</param>
        /// <param name="AShortTermerRow">The row of the attendee from the personnel short termer table</param>
        /// <returns></returns>
        private bool GetCongressOptionRates(ref TRptSituation ASituation, long AOutreachOption,
            ref PmShortTermApplicationRow AShortTermerRow)
        {
            PPartnerLocationTable PartnerLocationTable;

            PartnerLocationTable = PPartnerLocationAccess.LoadViaPPartner(AOutreachOption, ASituation.GetDatabaseConnection().Transaction);

            if (PartnerLocationTable.Rows.Count < 1)
            {
                return false;
            }

            PPartnerLocationRow PartnerLocationRow = (PPartnerLocationRow)PartnerLocationTable.Rows[0];

            FAttendeeStartDate = PartnerLocationRow.DateEffective.Value;
            FAttendeeEndDate = PartnerLocationRow.DateGoodUntil.Value;

            // Update arrival and departure dates if none have been entered yet,
            // either in the application travel details or the actual arrival at the conference
            if (AShortTermerRow.IsArrivalNull())
            {
                AShortTermerRow.Arrival = FAttendeeStartDate;
            }

            if (AShortTermerRow.IsDepartureNull())
            {
                AShortTermerRow.Departure = FAttendeeEndDate;
            }

            PcConferenceCostTable ConferenceCostTable;

            ConferenceCostTable = PcConferenceCostAccess.LoadByPrimaryKey(AOutreachOption,
                FAttendeeDays, ASituation.GetDatabaseConnection().Transaction);

            if (ConferenceCostTable.Rows.Count > 0)
            {
                FConferenceRate = (decimal)ConferenceCostTable.Rows[0][PcConferenceCostTable.GetChargeDBName()];
            }

            return true;
        }

        /// <summary>
        /// Find any applicable outreach supplement and puts them into FSupportCost member
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AOutreachType"></param>
        /// <param name="ACongressCode"></param>
        /// <param name="AChildDiscount"></param>
        /// <returns></returns>
        private bool DetermineOutreachSupplements(ref TRptSituation ASituation, String AOutreachType,
            String ACongressCode, decimal AChildDiscount)
        {
            PcSupplementTable SupplementTable;

            FSupportCost = 0;

            SupplementTable = PcSupplementAccess.LoadByPrimaryKey(FConferenceKey, AOutreachType, ASituation.GetDatabaseConnection().Transaction);

            if (SupplementTable.Rows.Count > 0)
            {
                PcSupplementRow Row = (PcSupplementRow)SupplementTable.Rows[0];

                FSupportCost = Row.Supplement;

                if (Row.ApplyDiscounts)
                {
                    FSupportCost = FSupportCost * (100 - AChildDiscount) / 100;

                    if (FIsCongressRole)
                    {
                        FSupportCost = FSupportCost * (100 - FRoleDiscountConferenceConference) / 100;
                    }

                    if (ACongressCode == "VOL")
                    {
                        FSupportCost = FSupportCost * (100 - FVolunteerDiscountConferenceConference) / 100;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Find any extra costs for this attendee
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        private bool DetermineExtraCosts(ref TRptSituation ASituation, long APartnerKey)
        {
            FExtraCost = 0;

            PcExtraCostTable ExtraCostTable;

            ExtraCostTable = PcExtraCostAccess.LoadViaPcAttendee(FConferenceKey, APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcExtraCostRow Row in ExtraCostTable.Rows)
            {
                FExtraCost = FExtraCost + Row.CostAmount;
            }

            return true;
        }

        /// <summary>
        /// Find out about the accommodation
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        private bool DetermineAccommodationCosts(ref TRptSituation ASituation, long APartnerKey)
        {
            PcRoomAllocTable RoomAllocTable;
            PcRoomTable RoomTable;

            FPreAccommodationCosts = 0;
            FAccommodationCosts = 0;
            FPostAccommodationCosts = 0;

            String AccommodationString = "";

            RoomAllocTable = PcRoomAllocAccess.LoadViaPcAttendee(FConferenceKey, APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcRoomAllocRow RoomAllocRow in RoomAllocTable.Rows)
            {
                RoomTable = PcRoomAccess.LoadByPrimaryKey(RoomAllocRow.VenueKey, RoomAllocRow.BuildingCode,
                    RoomAllocRow.RoomNumber, ASituation.GetDatabaseConnection().Transaction);

                if (RoomTable.Rows.Count < 1)
                {
                    continue;
                }

                AccommodationString = AccommodationString + "(" + RoomAllocRow.In.Subtract(
                    RoomAllocRow.Out.Value).Days.ToString() + "), ";

                int PreNights = 0;
                int ConferenceNights = 0;
                int PostNights = 0;

                if (RoomAllocRow.In.CompareTo(FAttendeeStartDate) < 0)
                {
                    // calculate Pre conference nights
                    if (RoomAllocRow.Out.Value.CompareTo(FAttendeeStartDate) < 1)
                    {
                        PreNights = RoomAllocRow.Out.Value.Subtract(RoomAllocRow.In).Days;
                    }
                    else
                    {
                        PreNights = FAttendeeStartDate.Subtract(RoomAllocRow.In).Days;
                    }
                }

                if ((RoomAllocRow.In.CompareTo(FAttendeeEndDate) < 0)
                    || (RoomAllocRow.Out.Value.CompareTo(FAttendeeStartDate) > 0))
                {
                    // calculate conference nights
                    ConferenceNights = FAttendeeEndDate.Subtract(FAttendeeStartDate).Days;

                    // subtract if there are days left out at beginning of conference
                    if (RoomAllocRow.In.CompareTo(FAttendeeStartDate) > 0)
                    {
                        ConferenceNights = ConferenceNights - RoomAllocRow.In.Subtract(FAttendeeStartDate).Days;
                    }

                    //subtract if there are days left out at end of conference
                    if (RoomAllocRow.Out.Value.CompareTo(FAttendeeEndDate) < 0)
                    {
                        ConferenceNights = ConferenceNights - FAttendeeEndDate.Subtract(RoomAllocRow.Out.Value).Days;
                    }

                    if (RoomAllocRow.Out.Value.CompareTo(FAttendeeEndDate) > 0)
                    {
                        // calculate post conference nights
                        if (RoomAllocRow.In.CompareTo(FAttendeeEndDate) > 0)
                        {
                            PostNights = RoomAllocRow.Out.Value.Subtract(RoomAllocRow.In).Days;
                        }
                        else
                        {
                            PostNights = RoomAllocRow.Out.Value.Subtract(FAttendeeEndDate).Days;
                        }

                        FConferenceFlags = FConferenceFlags + "T";
                    }

                    decimal BedCharge = (decimal)RoomTable.Rows[0][PcRoomTable.GetBedChargeDBName()];

                    FPreAccommodationCosts = FPreAccommodationCosts + PreNights * BedCharge;
                    FAccommodationCosts = FAccommodationCosts + ConferenceNights * BedCharge;
                    FPostAccommodationCosts = FPostAccommodationCosts + PostNights * BedCharge;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the number of days the attendee is on the conference.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AShortTermerRow">The row of the attendee from the personnel short termer table</param>
        /// <param name="AArrivalDate"></param>
        /// <param name="ADepartureDate"></param>
        /// <returns></returns>
        private int GetDaysAtCongress(ref TRptSituation ASituation, PmShortTermApplicationRow AShortTermerRow,
            DateTime AArrivalDate, DateTime ADepartureDate)
        {
            int Nights = 0;

            if (AArrivalDate.CompareTo(FAttendeeStartDate) >= 0)
            {
                if (ADepartureDate.CompareTo(FConferenceEndDate) <= 0)
                {
                    Nights = GetRoomAllocatedNightsBetween(ref ASituation, AShortTermerRow.PartnerKey, AArrivalDate, ADepartureDate);
                }
                else
                {
                    Nights = GetRoomAllocatedNightsBetween(ref ASituation, AShortTermerRow.PartnerKey, AArrivalDate, FAttendeeEndDate);
                }
            }
            else
            {
                if (ADepartureDate.CompareTo(FAttendeeEndDate) <= 0)
                {
                    Nights = GetRoomAllocatedNightsBetween(ref ASituation, AShortTermerRow.PartnerKey, FAttendeeStartDate, ADepartureDate);
                }
                else
                {
                    Nights = GetRoomAllocatedNightsBetween(ref ASituation, AShortTermerRow.PartnerKey, FAttendeeStartDate, FAttendeeEndDate);
                }
            }

            return Nights;
        }

        /// <summary>
        /// Returns the room allocation for this attendee.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AFromDate"></param>
        /// <param name="AToDate"></param>
        /// <returns></returns>
        private int GetRoomAllocatedNightsBetween(ref TRptSituation ASituation, long APartnerKey,
            DateTime AFromDate, DateTime AToDate)
        {
            PcRoomAllocTable RoomAllocTable = new PcRoomAllocTable();
            PcRoomAllocRow TemplateRow = (PcRoomAllocRow)RoomAllocTable.NewRow();

            TemplateRow.PartnerKey = APartnerKey;

            int Nights = 0;

            RoomAllocTable = PcRoomAllocAccess.LoadUsingTemplate(TemplateRow, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcRoomAllocRow RoomAllocRow in RoomAllocTable.Rows)
            {
                if ((RoomAllocRow.Out.Value.CompareTo(AFromDate) < 1)
                    || (RoomAllocRow.In.CompareTo(AToDate) > 0))
                {
                    continue;
                }

                if (RoomAllocRow.In.CompareTo(AFromDate) < 0)
                {
                    RoomAllocRow.In = AFromDate;
                }

                if (RoomAllocRow.Out.Value.CompareTo(AToDate) > 0)
                {
                    RoomAllocRow.Out = AToDate;
                }

                Nights = Nights + RoomAllocRow.Out.Value.Subtract(RoomAllocRow.In).Days;
            }

            if (Nights <= 0)
            {
                //If no accommodation allocated, charge for full period
                Nights = AToDate.Subtract(AFromDate).Days;
            }

            return Nights;
        }

        /// <summary>
        /// Returns the cost for the outreach. There might be different charges of the outreach
        /// depending how long the attendee takes part.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private decimal GetOutreachCost(ref TRptSituation ASituation)
        {
            PcConferenceCostTable ConferenceCostTable;

            ConferenceCostTable = PcConferenceCostAccess.LoadViaPcConference(FConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcConferenceCostRow Row in
                     ConferenceCostTable.Select("", PcConferenceCostTable.GetOptionDaysDBName() + " ASC"))
            {
                if (Row.OptionDays >= FAttendeeDays)
                {
                    return Row.Charge;
                }
            }

            return 0;
        }

        /// <summary>
        /// Determines if the attendee has a special role on the conference
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="ACongressCode"></param>
        /// <returns></returns>
        private bool IsAttendeeARole(ref TRptSituation ASituation, String ACongressCode)
        {
            bool ReturnValue = false;

            PtCongressCodeTable CongressCodeTable;

            CongressCodeTable = PtCongressCodeAccess.LoadByPrimaryKey(ACongressCode, ASituation.GetDatabaseConnection().Transaction);

            if (CongressCodeTable.Rows.Count > 0)
            {
                ReturnValue = (bool)CongressCodeTable.Rows[0][PtCongressCodeTable.GetDiscountedDBName()];
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the early or late charges of the attendee
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="ARegistrationDate"></param>
        /// <param name="AShortTermerRow">The row of the attendee from the personnel short termer table</param>
        /// <param name="EarlyPercent"></param>
        /// <param name="EarlyAmount"></param>
        /// <param name="LatePercent"></param>
        /// <param name="LateAmount"></param>
        private void GetEarlyLateCharges(ref TRptSituation ASituation, DateTime ARegistrationDate,
            PmShortTermApplicationRow AShortTermerRow, out int EarlyPercent,
            out decimal EarlyAmount, out int LatePercent, out decimal LateAmount)
        {
            EarlyAmount = 0;
            EarlyPercent = 0;
            LateAmount = 0;
            LatePercent = 0;

            PcEarlyLateTable EarlyLateTable;

            EarlyLateTable = PcEarlyLateAccess.LoadViaPcConference(FConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PcEarlyLateRow Row in EarlyLateTable.Rows)
            {
                if ((Row.Type)
                    && (ARegistrationDate.CompareTo(Row.Applicable) <= 0)
                    && ((AShortTermerRow.Departure.Value.Subtract(AShortTermerRow.Arrival.Value).Days * 2) > FConferenceDays))
                {
                    // Early
                    if (Row.AmountPercent)
                    {
                        EarlyAmount = Row.Amount;
                    }
                    else
                    {
                        EarlyPercent = Row.Percent;
                    }

                    // One record only
                    FConferenceFlags = FConferenceFlags + "E";
                    break;
                }

                if ((!Row.Type)
                    && (ARegistrationDate.CompareTo(Row.Applicable) >= 0))
                {
                    // Late
                    if (Row.AmountPercent)
                    {
                        LateAmount = Row.Amount;
                    }
                    else
                    {
                        LatePercent = Row.Percent;
                    }

                    // One record only
                    FConferenceFlags = FConferenceFlags + "L";
                    break;
                }
            }
        }

        /// <summary>
        /// Determines if the attendee is an omer.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ASituation"></param>
        private void CheckPartnerTypeCode(long APartnerKey, ref TRptSituation ASituation)
        {
            PPartnerTypeTable PartnerTypeTable;

            PartnerTypeTable = PPartnerTypeAccess.LoadViaPPartner(APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (PPartnerTypeRow Row in PartnerTypeTable.Rows)
            {
                // TODO ORGANIZATION SPECIFIC TypeCode
                if (Row.TypeCode.StartsWith("OMER"))
                {
                    FConferenceFlags = FConferenceFlags + "O";
                    return;
                }
            }
        }

        # endregion

        # region Initialisation

        /// <summary>
        /// Initialises the FResult data table
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AConferenceKey"></param>
        private void InitResultDataTable(ref TRptSituation ASituation, long AConferenceKey)
        {
            FResultDataTable = new DataTable();

            DataColumn newColumn = new DataColumn("Field", Type.GetType("System.String"));
            newColumn.DefaultValue = "";
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("RowType", Type.GetType("System.Char"));
            newColumn.DefaultValue = ' ';              // should be either 'C' (Child) 'A' (Aduld) 'T' (Total) or 'S' (Cost)
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Age From", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Age To", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Conference Discount", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Accommodation Discount", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Congress Volunteer", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Congress Special Role", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Congress Only", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Total", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Outreach Only", Type.GetType("System.Int32"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Conference Fees", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Outreach Fees", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Supplement", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Extra Costs", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Accommodation", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
            newColumn = new DataColumn("Total Fees", Type.GetType("System.Decimal"));
            newColumn.DefaultValue = 0;
            FResultDataTable.Columns.Add(newColumn);
        }

        /// <summary>
        /// Puts the calculated results of one attendee to the FResult data table
        /// </summary>
        /// <param name="AField"></param>
        /// <param name="AAge"></param>
        /// <param name="AShortTermerRow">The row of the attendee from the personnel short termer table</param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool AddCalculationsToResultTable(String AField, int AAge, PmShortTermApplicationRow AShortTermerRow,
            ref TRptSituation ASituation)
        {
            int ResultTableRowIndex = -1;
            Char RowType = '?';

            ResultTableRowIndex = GetResultTableRowIndex(AField, ref RowType, AAge, ref ASituation);

            if (ResultTableRowIndex >= FResultDataTable.Rows.Count)
            {
                TLogging.Log("Can't add calculations to the result table");
                return false;
            }

            DataRow CurrentRow = FResultDataTable.Rows[ResultTableRowIndex];

            decimal TotalFees = 0;
            // Add the costs
            CurrentRow["Conference Fees"] = (decimal)CurrentRow["Conference Fees"] + FCongressCosts + FPreConferenceCosts + FPostConferenceCosts;
            CurrentRow["Outreach Fees"] = (decimal)CurrentRow["Outreach Fees"] + FOutreachCosts;
            CurrentRow["Supplement"] = (decimal)CurrentRow["Supplement"] + FSupportCost;
            CurrentRow["Extra Costs"] = (decimal)CurrentRow["Extra Costs"] + FExtraCost;
            CurrentRow["Accommodation"] = (decimal)CurrentRow["Accommodation"] + FAccommodationCosts + FPreAccommodationCosts +
                                          FPostAccommodationCosts;
            TotalFees = FCongressCosts + FPreConferenceCosts + FPostConferenceCosts +
                        FOutreachCosts + FSupportCost + FExtraCost + FAccommodationCosts + FPreAccommodationCosts + FPostAccommodationCosts;
            CurrentRow["Total Fees"] = (decimal)CurrentRow["Total Fees"] + TotalFees;

            // Add the counts of the attendees
            CurrentRow["Total"] = (Int32)CurrentRow["Total"] + 1;

            if (FIsCongressVolunteer)
            {
                CurrentRow["Congress Volunteer"] = (Int32)CurrentRow["Congress Volunteer"] + 1;
            }
            else if (FIsCongressOnly)
            {
                CurrentRow["Congress Only"] = (Int32)CurrentRow["Congress Only"] + 1;
            }
            else if (FIsOutreachOnly)
            {
                CurrentRow["Outreach Only"] = (Int32)CurrentRow["Outreach Only"] + 1;
            }
            else if (FIsCongressRole)
            {
                CurrentRow["Congress Special Role"] = (Int32)CurrentRow["Congress Special Role"] + 1;
            }

            AddCalculationsToTotalRow(AField, TotalFees, ref ASituation);
            return true;
        }

        /// <summary>
        /// Adds the calculated result of one attendee to the "total" row of the FResult data table.
        /// </summary>
        /// <param name="AField"></param>
        /// <param name="ATotalFees"></param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool AddCalculationsToTotalRow(String AField, decimal ATotalFees, ref TRptSituation ASituation)
        {
            int ResultTableRowIndex = -1;
            Char RowType = 'T';

            ResultTableRowIndex = GetResultTableRowIndex(AField, ref RowType, 0, ref ASituation);

            DataRow TotalRow = FResultDataTable.Rows[ResultTableRowIndex];

            TotalRow["Conference Fees"] = (decimal)TotalRow["Conference Fees"] + FCongressCosts + FPreConferenceCosts + FPostConferenceCosts;
            TotalRow["Outreach Fees"] = (decimal)TotalRow["Outreach Fees"] + FOutreachCosts;
            TotalRow["Supplement"] = (decimal)TotalRow["Supplement"] + FSupportCost;
            TotalRow["Extra Costs"] = (decimal)TotalRow["Extra Costs"] + FExtraCost;
            TotalRow["Accommodation"] = (decimal)TotalRow["Accommodation"] + FAccommodationCosts + FPreAccommodationCosts + FPostAccommodationCosts;
            TotalRow["Total Fees"] = (decimal)TotalRow["Total Fees"] + ATotalFees;

            TotalRow["Total"] = (Int32)TotalRow["Total"] + 1;

            if (FIsCongressVolunteer)
            {
                TotalRow["Congress Volunteer"] = (Int32)TotalRow["Congress Volunteer"] + 1;
            }
            else if (FIsCongressOnly)
            {
                TotalRow["Congress Only"] = (Int32)TotalRow["Congress Only"] + 1;
            }
            else if (FIsOutreachOnly)
            {
                TotalRow["Outreach Only"] = (Int32)TotalRow["Outreach Only"] + 1;
            }
            else if (FIsCongressRole)
            {
                TotalRow["Congress Special Role"] = (Int32)TotalRow["Congress Special Role"] + 1;
            }

            return true;
        }

        /// <summary>
        /// Gets the row index from the FResultDataTable, based on the field name and age.
        /// The row depends on the field and age.
        /// </summary>
        /// <param name="AField"></param>
        /// <param name="ARowType">Defines the type of row. Either 'C' (Child), 'A' (Adult)
        /// 'T' (Total) or 'S' (Cost)
        /// If it is '?' then it will be changed to either 'C' or 'A', depending on the age and
        /// the available discounts.</param>
        /// <param name="AAge"></param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private int GetResultTableRowIndex(String AField, ref Char ARowType, int AAge, ref TRptSituation ASituation)
        {
            bool FieldFound = false;
            int AdultRowIndex = -1;

            for (int Counter = 0; Counter < FResultDataTable.Rows.Count; ++Counter)
            {
                if ((String)FResultDataTable.Rows[Counter]["Field"] == AField)
                {
                    FieldFound = true;

                    if (ARowType == '?')
                    {
                        if ((Char)FResultDataTable.Rows[Counter]["RowType"] == 'A')
                        {
                            AdultRowIndex = Counter;
                        }
                        else if (((Char)FResultDataTable.Rows[Counter]["RowType"] == 'C')
                                 && ((Int32)FResultDataTable.Rows[Counter]["Age From"] <= AAge)
                                 && ((Int32)FResultDataTable.Rows[Counter]["Age To"] >= AAge))
                        {
                            return Counter;
                        }
                    }
                    else if (ARowType == (Char)FResultDataTable.Rows[Counter]["RowType"])
                    {
                        return Counter;
                    }
                }
            }

            if (FieldFound)
            {
                // We found the field but it was not a child
                return AdultRowIndex;
            }
            else
            {
                AddFieldToResultTable(AField, ref ASituation);

                return GetResultTableRowIndex(AField, ref ARowType, AAge, ref ASituation);
            }
        }

        /// <summary>
        /// Adds a new field (unit) with all the rows to the FResult data table
        /// </summary>
        /// <param name="AField">The name of the field</param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool AddFieldToResultTable(String AField, ref TRptSituation ASituation)
        {
            List <int>AgeList;

            GetChildDiscountList(out AgeList, ref ASituation);

            int PreviousAge = 0;
            decimal ChildDiscount;
            decimal ChildAccommodationDiscount;
            Boolean InPercent;

            // Add Cost to the table
            DataRow NewRow = FResultDataTable.NewRow();
            NewRow["RowType"] = 'S';
            NewRow["Field"] = AField;
            NewRow["Congress Volunteer"] = (Int32)FConferenceDayRate * (100 - FVolunteerDiscountConferenceConference) / 100;
            NewRow["Congress Special Role"] = (Int32)FConferenceDayRate * (100 - FRoleDiscountConferenceConference) / 100;
            NewRow["Congress Only"] = (Int32)FConferenceDayRate;
            FResultDataTable.Rows.Add(NewRow);

            foreach (int CurrentAge in AgeList)
            {
                // Add the child discounts to the table
                NewRow = FResultDataTable.NewRow();

                TAccommodationReportCalculation.GetChildDiscount(CurrentAge,
                    FConferenceKey,
                    "CONFERENCE",
                    out ChildDiscount,
                    out InPercent,
                    ref ASituation);
                TAccommodationReportCalculation.GetChildDiscount(CurrentAge,
                    FConferenceKey,
                    "ACCOMMODATION",
                    out ChildAccommodationDiscount,
                    out InPercent,
                    ref ASituation);

                NewRow["Age From"] = PreviousAge;
                NewRow["Age To"] = CurrentAge;
                NewRow["RowType"] = 'C';
                NewRow["Field"] = AField;

                NewRow["Conference Discount"] = ChildDiscount;
                NewRow["Accommodation Discount"] = ChildAccommodationDiscount;

                PreviousAge = CurrentAge + 1;

                FResultDataTable.Rows.Add(NewRow);
            }

            // Add Adult and Total to the table
            NewRow = FResultDataTable.NewRow();
            NewRow["RowType"] = 'A';
            NewRow["Field"] = AField;
            FResultDataTable.Rows.Add(NewRow);
            NewRow = FResultDataTable.NewRow();
            NewRow["RowType"] = 'T';
            NewRow["Field"] = AField;
            FResultDataTable.Rows.Add(NewRow);

            return true;
        }

        /// <summary>
        /// Get a list of all the different child discounts available for this conference.
        /// </summary>
        /// <param name="AgeList"></param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool GetChildDiscountList(out List <int>AgeList, ref TRptSituation ASituation)
        {
            AgeList = new List <int>();

            PcDiscountTable DiscountTable = new PcDiscountTable();
            PcDiscountRow TemplateRow = DiscountTable.NewRowTyped(false);

            TemplateRow.ConferenceKey = FConferenceKey;
            TemplateRow.DiscountCriteriaCode = "CHILD";
            TemplateRow.Validity = "ALWAYS";

            StringCollection OrderList = new StringCollection();
            OrderList.Add(" ORDER BY " + PcDiscountTable.GetUpToAgeDBName() + " ASC");

            DiscountTable = PcDiscountAccess.LoadUsingTemplate(TemplateRow, null, null,
                ASituation.GetDatabaseConnection().Transaction, OrderList, 0, 0);

            int PreviousAge = -1;

            foreach (PcDiscountRow DiscountRow in DiscountTable.Rows)
            {
                // we might get the same age two times (for accommodation discount and for conference discount)
                // But we need the age only once.
                if (PreviousAge != DiscountRow.UpToAge)
                {
                    AgeList.Add(DiscountRow.UpToAge);
                    PreviousAge = DiscountRow.UpToAge;
                }
            }

            return true;
        }

        #endregion

        #region Transfer Results to output

        /// <summary>
        /// Add one line with the field name to the report.
        /// </summary>
        /// <param name="AFieldName"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool AddNewFieldNameToResults(String AFieldName, int AMasterRow,
            int ALevel, int AChildRow, ref TRptSituation ASituation)
        {
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Columns[0] = new TVariant(AFieldName);

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            AddFinancialHeaderRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            return true;
        }

        /// <summary>
        /// Adds an empty row to the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <returns></returns>
        private bool AddEmptyRowToResults(ref TRptSituation ASituation, int AMasterRow, int ALevel, int AChildRow)
        {
            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow, Columns);
            return true;
        }

        /// <summary>
        /// Adds the header row of the financial lines to the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <returns></returns>
        private bool AddFinancialHeaderRowToResults(ref TRptSituation ASituation, int AMasterRow, int ALevel, int AChildRow)
        {
            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Columns[1] = new TVariant("Conf. Disc.");
            Columns[2] = new TVariant("Accom. Disc.");
            Columns[3] = new TVariant("Cong. Vol.");
            Columns[4] = new TVariant("Cong. Sp.Role");
            Columns[5] = new TVariant("Cong. Only");
            Columns[6] = new TVariant("Total");
            Columns[7] = new TVariant("Campgn Only");
            Columns[8] = new TVariant("Conf. Fees");
            Columns[9] = new TVariant("Outreach Fees");
            Columns[10] = new TVariant("Supplement");
            Columns[11] = new TVariant("Extra Costs");
            Columns[12] = new TVariant("Accommodation");
            Columns[13] = new TVariant("Total Fees");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow, Columns);

            return true;
        }

        /// <summary>
        /// Adds one row for the financial lines from the FResult data table to the report
        /// </summary>
        /// <param name="AResultRow"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <param name="ASituation"></param>
        /// <returns></returns>
        private bool AddFinancialRowToResults(DataRow AResultRow, int AMasterRow, int ALevel,
            int AChildRow, ref TRptSituation ASituation)
        {
            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Columns[0] = new TVariant(MakeFirstColumnEntry(AResultRow));
            Columns[1] = new TVariant(AResultRow["Conference Discount"]);
            Columns[2] = new TVariant(AResultRow["Accommodation Discount"]);
            Columns[3] = new TVariant(AResultRow["Congress Volunteer"]);
            Columns[4] = new TVariant(AResultRow["Congress Special Role"]);
            Columns[5] = new TVariant(AResultRow["Congress Only"]);
            Columns[6] = new TVariant(AResultRow["Total"]);
            Columns[7] = new TVariant(AResultRow["Outreach Only"]);
            Columns[8] = new TVariant(AResultRow["Conference Fees"]);
            Columns[9] = new TVariant(AResultRow["Outreach Fees"]);
            Columns[10] = new TVariant(AResultRow["Supplement"]);
            Columns[11] = new TVariant(AResultRow["Extra Costs"]);
            Columns[12] = new TVariant(AResultRow["Accommodation"]);
            Columns[13] = new TVariant(AResultRow["Total Fees"]);

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow, Columns);

            return true;
        }

        /// <summary>
        /// Get the text of the first column for the financial lines
        /// </summary>
        /// <param name="ADataRow"></param>
        /// <returns></returns>
        private String MakeFirstColumnEntry(DataRow ADataRow)
        {
            String ReturnValue = "";

            switch ((Char)ADataRow["RowType"])
            {
                case 'S':
                    ReturnValue = "Cost:";
                    break;

                case 'C':
                    ReturnValue = ADataRow["Age From"].ToString() + " to " + ADataRow["Age To"].ToString() + ":";
                    break;

                case 'A':
                    ReturnValue = "Adult:";
                    break;

                case 'T':
                    ReturnValue = "Total:";
                    break;
            }

            return ReturnValue;
        }

        #endregion

        #region Sign of part

        /// <summary>
        /// Prints the text of the financial sign off lines to the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        private bool AddFinancialSignOffLines(ref TRptSituation ASituation, int AMasterRow, int ALevel, int AChildRow, String AFieldName)
        {
            // 1. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            // 2. line
            // Get the total amount
            Char RowType = 'T';
            int TotalRowIndex = GetResultTableRowIndex(AFieldName, ref RowType, 0, ref ASituation);
            decimal TotalFees = (decimal)FResultDataTable.Rows[TotalRowIndex]["Total Fees"];

            Columns[0] = new TVariant("I agree that the above attendances are accurate. Any differences over the amount");
            Columns[4] = new TVariant("Total amount due from Field:");
            Columns[5] = new TVariant(FConferenceCurrency + "....." + StringHelper.FormatUsingCurrencyCode(TotalFees, FConferenceCurrency));


            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 3. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("payable/receivable are to be discussed with the Conference Office.");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 4. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 5. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Registrar");
            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");
            Columns[3] = new TVariant(" ");
            Columns[4] = new TVariant("1)........................................................");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 6. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 7. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Field/Office Representative");
            Columns[1] = new TVariant("(name)__________________");
            Columns[4] = new TVariant("2)........................................................");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 8. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 9. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");
            Columns[4] = new TVariant("3)........................................................");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 10. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 11. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Finance Manager");
            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");
            Columns[4] = new TVariant("4)........................................................");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 12. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 13. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Field/Office Representative");
            Columns[1] = new TVariant("(name)__________________");
            Columns[2] = new TVariant("Date:____________________");
            Columns[4] = new TVariant("5)........................................................");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 14. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 15. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");
            Columns[4] = new TVariant("Less cash already paid by Field:");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 16. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 17. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("NOTE: If this sheet is not signed and returned before leaving the conference");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 18. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("       the Conference computer records will be deemed as correct.");
            Columns[4] = new TVariant("Balance due from Field:");
            Columns[5] = new TVariant(FConferenceCurrency + "..........................");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 19. 20. 21. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            return true;
        }

        /// <summary>
        /// Prints the text for the attendee sign off line to the report.
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        private bool AddAttendanceSignOffLines(ref TRptSituation ASituation, int AMasterRow, int ALevel, int AChildRow, String FieldName)
        {
            // 1. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            // 2. line
            Columns[0] = new TVariant("This report is to enable us to establish the correct attendance at the conference");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 3. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();
            Columns[0] = new TVariant("A separate bill with full financial information will be given to the");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 4. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();
            Columns[0] = new TVariant("fields responsible for pqyment of conference fees.");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 5. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 6. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Registrar");
            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 7. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 8. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("Field/Office Representative");
            Columns[1] = new TVariant("(name)__________________");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 9. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 10. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[1] = new TVariant("(signed)_________________");
            Columns[2] = new TVariant("Date:____________________");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 11. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            // 12. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("NOTE: If this sheet is not signed and returned before leaving the conference");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 13. line
            for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Description[0] = new TVariant();
            Description[1] = new TVariant();

            Columns[0] = new TVariant("       the Conference computer records will be deemed as correct.");

            ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                Header, Description, Columns);
            ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

            // 14. 15. 16. line
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);
            AddEmptyRowToResults(ref ASituation, AMasterRow, ALevel, AChildRow++);

            return true;
        }

        #endregion

        #region Extra Costs
        /// <summary>
        /// Add the extra cost lines for the attendees to the report
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="AConferenceKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="AMasterRow"></param>
        /// <param name="ALevel"></param>
        /// <param name="AChildRow"></param>
        /// <returns></returns>
        private int AddExtraCostLines(ref TRptSituation ASituation, long AConferenceKey, long APartnerKey,
            int AMasterRow, int ALevel, int AChildRow)
        {
            int NumLinesAdded = 0;

            PcExtraCostTable ExtraCostTable;

            ExtraCostTable = PcExtraCostAccess.LoadViaPcAttendee(AConferenceKey, APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            TVariant[] Header = new TVariant[FMaxNumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[FMaxNumColumns];

            foreach (PcExtraCostRow ExtraCostRow in ExtraCostTable.Rows)
            {
                for (int Counter = 0; Counter < FMaxNumColumns; ++Counter)
                {
                    Columns[Counter] = new TVariant(" ");
                    Header[Counter] = new TVariant();
                }

                Description[0] = new TVariant();
                Description[1] = new TVariant();

                if (NumLinesAdded == 0)
                {
                    Columns[0] = new TVariant("Extra Cost:");
                }

                Columns[1] = new TVariant(ExtraCostRow.CostTypeCode);
                // we need one space after the amount. Otherwise it's not correctly printed in the report
                Columns[2] = new TVariant(StringHelper.FormatUsingCurrencyCode(ExtraCostRow.CostAmount, FConferenceCurrency));
                Columns[3] = new TVariant(ExtraCostRow.Comment);

                ASituation.GetResults().AddRow(AMasterRow, AChildRow, true, ALevel, "", "", false,
                    Header, Description, Columns);
                ASituation.GetResults().UpdateRow(AMasterRow, AChildRow++, Columns);

                ++NumLinesAdded;
            }

            return NumLinesAdded;
        }

        #endregion
        /// <summary>
        /// Returns the number of attendees from this field.
        /// </summary>
        /// <param name="AFieldName"></param>
        /// <returns></returns>
        private int GetNumberOfEntriesPerField(String AFieldName)
        {
            foreach (DataRow Row in FResultDataTable.Rows)
            {
                if (((String)Row["Field"] == AFieldName)
                    && ((Char)Row["RowType"] == 'T'))
                {
                    return (Int32)Row["Total"];
                }
            }

            return 0;
        }
    }

    /// <summary>
    /// Extends class TRptSituation to expose the RunningCode member
    /// </summary>
    public class TExtentedSituation : TRptSituation
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ASituation"></param>
        public TExtentedSituation(TRptSituation ASituation) : base(ASituation)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int GetRunningCode()
        {
            return RunningCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AValue"></param>
        public void SetRunningCode(int AValue)
        {
            RunningCode = AValue;
        }
    }
}