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
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// This contains specific functions for the Conference module,
    /// that are needed for report generation.
    /// </summary>
    public class TRptUserFunctionsConference : TRptUserFunctions
    {
        private static TAccommodationReportCalculation FAccommodationReportCalculation;
        private static TNationalitySummaryCalculation FNationalitySummaryCalculation;
        private static TAttendanceSummaryCalculation FAttendanceSummaryCalculation;
        private static TAgeSummaryCalculation FAgeSummaryCalculation;
        private static TConferenceRoleCalculation FConferenceRoleCalculation;
        private static TConferenceFieldCalculation FConferenceFieldCalculation;

        /// <summary>
        /// all functions for reports in the conference module need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            if (StringHelper.IsSame(f, "GetConferenceRoom"))
            {
                value = new TVariant(GetConferenceRoom(ops[1].ToInt64(), ops[2].ToInt64(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "CalculatePartnerAccom"))
            {
                bool ResultValue = false;

                if (FAccommodationReportCalculation != null)
                {
                    ResultValue = FAccommodationReportCalculation.CalculatePartnerAccom(
                        ops[1].ToInt64(), ops[2].ToDate(), ops[3].ToDate(),
                        ops[4].ToDate(), ops[5].ToDate(), ops[6].ToInt64(),
                        ops[7].ToString(), ref situation);
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearAccomTable"))
            {
                value = new TVariant(true);

                if (FAccommodationReportCalculation != null)
                {
                    FAccommodationReportCalculation = null;
                }

                FAccommodationReportCalculation = new TAccommodationReportCalculation();

                return true;
            }

            if (StringHelper.IsSame(f, "FinishAccomTable"))
            {
                bool ResultValue = false;

                if (FAccommodationReportCalculation != null)
                {
                    ResultValue = FAccommodationReportCalculation.FinishAccomTable(ops[1].ToString(),
                        ref situation);
                    // now we don't use FAccommodationReportCalculation any more.
                    FAccommodationReportCalculation = null;
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateSingleAttendance"))
            {
                bool ResultValue = false;

                if (FAttendanceSummaryCalculation != null)
                {
                    ResultValue = FAttendanceSummaryCalculation.CalculateSingleAttendance(
                        ops[1].ToInt64(), ops[2].ToInt64(), ops[3].ToDate(),
                        ops[4].ToDate(), ops[5].ToDate(),
                        ops[6].ToDate(), ops[7].ToDate(),
                        ops[8].ToDate(), ops[9].ToDate(), ref situation);
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearAttendanceTable"))
            {
                FAttendanceSummaryCalculation = new TAttendanceSummaryCalculation(ops[1].ToDate(), ops[2].ToDate());
                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "FinishAttendanceTable"))
            {
                bool ResultValue = false;

                if (FAttendanceSummaryCalculation != null)
                {
                    ResultValue = FAttendanceSummaryCalculation.FinishAttendanceTable(ref situation);
                    FAttendanceSummaryCalculation = null;
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateNationalities"))
            {
                bool ResultValue = false;

                if (FNationalitySummaryCalculation != null)
                {
                    ResultValue = FNationalitySummaryCalculation.CalculateNationalities(ops[1].ToInt64(),
                        ops[2].ToString(), ops[3].ToString(), ref situation);
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearNationalityTable"))
            {
                FNationalitySummaryCalculation = new TNationalitySummaryCalculation();

                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "FinishNationalityTable"))
            {
                bool ResultValue = false;

                if (FNationalitySummaryCalculation != null)
                {
                    ResultValue = FNationalitySummaryCalculation.FinishNationalityTable(ref situation);
                    FNationalitySummaryCalculation = null;
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateSingleAge"))
            {
                bool ResultValue = false;

                if (FAgeSummaryCalculation != null)
                {
                    ResultValue = FAgeSummaryCalculation.CalculateSingleAge(
                        ops[1].ToDate(), ops[2].ToString(), ref situation);
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearAgeTable"))
            {
                FAgeSummaryCalculation = new TAgeSummaryCalculation();

                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "FinishAgeTable"))
            {
                bool ResultValue = false;

                if (FAgeSummaryCalculation != null)
                {
                    ResultValue = FAgeSummaryCalculation.FinishAgeTable(ref situation);
                    FAgeSummaryCalculation = null;
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateSingleConferenceRole"))
            {
                bool ResultValue = false;

                if (FConferenceRoleCalculation != null)
                {
                    ResultValue = FConferenceRoleCalculation.CalculateSingleConferenceRole(
                        ops[1].ToString(), ops[2].ToInt64(), ops[3].ToString(), ops[4].ToInt64(),
                        ops[5].ToInt64());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearConferenceRoleTable"))
            {
                FConferenceRoleCalculation = new TConferenceRoleCalculation();

                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "FinishConferenceRoleTable"))
            {
                bool ResultValue = false;

                if (FConferenceRoleCalculation != null)
                {
                    ResultValue = FConferenceRoleCalculation.FinishConferenceRoleTable(
                        ops[1].ToString(), ref situation);
                    FConferenceRoleCalculation = null;
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "InitFieldCostsCalculation"))
            {
                if (FConferenceFieldCalculation == null)
                {
                    FConferenceFieldCalculation = new TConferenceFieldCalculation(ref situation,
                        ops[1].ToInt64());
                }

                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "ClearFieldCostsCalculation"))
            {
                if (FConferenceFieldCalculation != null)
                {
                    FConferenceFieldCalculation = null;
                }

                value = new TVariant(true);
                return true;
            }

            if (StringHelper.IsSame(f, "PrintFieldFinancialCosts"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.PrintFieldFinancialCosts(ref situation, ops[1].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "PrintFinancialSignOffLines"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.PrintFinancialSignOffLines(ref situation, ops[1].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "PrintAttendanceSignOffLines"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.PrintAttendanceSignOffLines(ref situation, ops[1].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "PrintEmptyLineInFieldReport"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.PrintEmptyLineInFieldReport(ref situation);
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateOneAttendeeFieldCost"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    String FinanceDetails;
                    String Accommodation;

                    ResultValue = FConferenceFieldCalculation.CalculateOneAttendeeFieldCost(ref situation,
                        ops[1].ToInt32(), ops[2].ToInt64(), ops[3].ToInt32(), ops[4].ToInt64(),
                        ops[5].ToInt64(),
                        ops[6].ToString(), ops[7].ToDate(), out FinanceDetails, out Accommodation);

                    situation.GetParameters().Add("AccommodationCost", new TVariant(Accommodation));
                    situation.GetParameters().Add("FinanceDetails", new TVariant(FinanceDetails));
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "GetExtraCosts"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.GetExtraCosts(ref situation, ops[1].ToInt64(),
                        ops[2].ToInt64());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "HasReportSendingField"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.HasReportSendingField(ops[1].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "HasAttendeeReceivingField"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.HasAttendeeReceivingField(ref situation, ops[1].ToInt64(),
                        ops[2].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            if (StringHelper.IsSame(f, "HasFieldReportDiscount"))
            {
                bool ResultValue = false;

                if (FConferenceFieldCalculation != null)
                {
                    ResultValue = FConferenceFieldCalculation.HasFieldReportDiscount(ops[1].ToString());
                }

                value = new TVariant(ResultValue);
                return true;
            }

            /*
             * if (isSame(f, 'doSomething')) then
             * begin
             * value := new TVariant();
             * doSomething(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString());
             * exit;
             * end;
             */
            value = new TVariant();
            return false;
        }

        /// <summary>
        /// Gets the Room for a given partner during a confernce.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the person</param>
        /// <param name="AConferenceKey">PartnerKey of the confernce</param>
        /// <param name="ADefaultValue">Default value if no room was found</param>
        /// <returns></returns>
        private String GetConferenceRoom(Int64 APartnerKey, Int64 AConferenceKey, String ADefaultValue)
        {
            String ReturnValue = ADefaultValue;
            PcRoomAllocTable RoomAllocTable;

            RoomAllocTable = PcRoomAllocAccess.LoadViaPcAttendee(AConferenceKey, APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (RoomAllocTable.Rows.Count > 0)
            {
                ReturnValue = ((PcRoomAllocRow)RoomAllocTable.Rows[0]).BuildingCode + " " +
                              ((PcRoomAllocRow)RoomAllocTable.Rows[0]).RoomNumber;
            }

            return ReturnValue;
        }
    }
}