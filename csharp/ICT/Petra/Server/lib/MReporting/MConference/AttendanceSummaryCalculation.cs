//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Data;
using Ict.Common;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Description of AttendanceSummaryCalculation.
    /// </summary>
    public class TAttendanceSummaryCalculation
    {
        private static DataTable FAttendanceTable;
        private static DateTime FFromDate;
        private static DateTime FToDate;
        private static long FLastConferenceKey;
        private static int FChildDiscountAge;

        const int COLUMNDATE = 0;
        const int COLUMNTOTAL = 1;
        const int COLUMNACTUAL = 2;
        const int COLUMNEXPECTED = 3;
        const int COLUMNCHILDREN = 4;
        const int COLUMNISUSED = 5;

        /// <summary>
        ///
        /// </summary>
        /// <param name="AFromDate"></param>
        /// <param name="AToDate"></param>
        public TAttendanceSummaryCalculation(DateTime AFromDate, DateTime AToDate)
        {
            InitAttendanceTable(AFromDate, AToDate);
        }

        /// <summary>
        /// Calculate the data of one partner attending the conference
        /// </summary>
        /// <param name="AConferenceKey"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ABirthDay"></param>
        /// <param name="AArrivalDate"></param>
        /// <param name="ADepartureDate"></param>
        /// <param name="AActualArrivalDate"></param>
        /// <param name="AActualDepartureDate"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <param name="ASituation">Current report situation</param>
        /// <returns></returns>
        public bool CalculateSingleAttendance(long AConferenceKey, long APartnerKey, DateTime ABirthDay,
            DateTime AArrivalDate, DateTime ADepartureDate,
            DateTime AActualArrivalDate, DateTime AActualDepartureDate,
            DateTime AStartDate, DateTime AEndDate,
            ref TRptSituation ASituation)
        {
            DateTime ArrivalDate = AActualArrivalDate;
            DateTime DepartureDate = AActualDepartureDate;

            if (FLastConferenceKey != AConferenceKey)
            {
                FChildDiscountAge = GetChildDiscountAge(AConferenceKey, ref ASituation);
                FLastConferenceKey = AConferenceKey;
            }

            int Age = Calculations.CalculateAge(ABirthDay, AStartDate);

            if ((AArrivalDate > ADepartureDate)
                || (AActualArrivalDate > AActualDepartureDate))
            {
                return false;
            }

            if (DepartureDate == DateTime.MinValue)
            {
                DepartureDate = ADepartureDate;

                if (DepartureDate == DateTime.MinValue)
                {
                    DepartureDate = AEndDate;
                }
            }

            if (ArrivalDate != DateTime.MinValue)
            {
                AddAttendeeToList(ArrivalDate, DepartureDate, Age, true);
            }
            else
            {
                ArrivalDate = AArrivalDate;

                if (ArrivalDate == DateTime.MinValue)
                {
                    ArrivalDate = AStartDate;
                }

                AddAttendeeToList(ArrivalDate, DepartureDate, Age, false);
            }

            return true;
        }

        /// <summary>
        /// Copies the result of the FAttendanceTable to report.
        /// </summary>
        /// <returns></returns>
        public bool FinishAttendanceTable(ref TRptSituation ASituation)
        {
            ASituation.GetResults().Clear();

            if (FAttendanceTable == null)
            {
                return false;
            }

            int ChildRow = 1;

            foreach (DataRow CurrentRow in FAttendanceTable.Rows)
            {
                if (!(bool)CurrentRow[5])
                {
                    // skip dates where there is no entry.
                    continue;
                }

                TVariant[] Header = new TVariant[5];
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns = new TVariant[5];

                for (int Counter = 0; Counter < 5; ++Counter)
                {
                    Columns[Counter] = new TVariant(CurrentRow[Counter].ToString());
                    Header[Counter] = new TVariant();
                }

                ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                    Header, Description, Columns);
            }

            return true;
        }

        private bool InitAttendanceTable(DateTime AFromDate, DateTime AToDate)
        {
            FFromDate = AFromDate;
            FToDate = AToDate;

            // Init the Table
            FAttendanceTable = new DataTable("Accomodation Report");

            // TimeSpan CheckLength = AToDate.Subtract(AFromDate);
            // int DaysNumber = CheckLength.Days;

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "Date";
            FAttendanceTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Total";
            FAttendanceTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Actual";
            FAttendanceTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Expected";
            FAttendanceTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Children";
            FAttendanceTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Boolean");
            Column.DefaultValue = false;
            Column.ColumnName = "IsUsed";
            FAttendanceTable.Columns.Add(Column);

            int NumDays = FToDate.Subtract(FFromDate).Days;

            for (int Counter = 0; Counter <= NumDays; ++Counter)
            {
                DataRow NewRow = FAttendanceTable.NewRow();

                DateTime CurrentDate = FFromDate.AddDays(Counter);
                // TODO add year???
                NewRow[0] = CurrentDate.ToString("MMM-dd-yyyy");

                FAttendanceTable.Rows.Add(NewRow);
            }

            return true;
        }

        private bool AddAttendeeToList(DateTime AArrivalDate, DateTime ADepartureDate, int AAge, bool AIsActual)
        {
            int NumDays = FToDate.Subtract(FFromDate).Days;

            for (int Counter = 0; Counter <= NumDays; ++Counter)
            {
                DateTime CurrentDate = FFromDate.AddDays(Counter);

                if ((CurrentDate >= AArrivalDate)
                    && (CurrentDate <= ADepartureDate))
                {
                    if (AIsActual)
                    {
                        FAttendanceTable.Rows[Counter][COLUMNACTUAL] = (int)FAttendanceTable.Rows[Counter][COLUMNACTUAL] + 1;
                    }
                    else
                    {
                        FAttendanceTable.Rows[Counter][COLUMNEXPECTED] = (int)FAttendanceTable.Rows[Counter][COLUMNEXPECTED] + 1;
                    }

                    FAttendanceTable.Rows[Counter][COLUMNTOTAL] = (int)FAttendanceTable.Rows[Counter][COLUMNTOTAL] + 1;

                    if (AAge <= FChildDiscountAge)
                    {
                        FAttendanceTable.Rows[Counter][COLUMNCHILDREN] = (int)FAttendanceTable.Rows[Counter][COLUMNCHILDREN] + 1;
                    }

                    FAttendanceTable.Rows[Counter][COLUMNISUSED] = true;
                }
            }

            return true;
        }

        private int GetChildDiscountAge(long AConferenceKey, ref TRptSituation ASituation)
        {
            int DiscountAge = 0;

            PcDiscountTable DiscountTable;

            DiscountTable = PcDiscountAccess.LoadViaPcConference(AConferenceKey, ASituation.GetDatabaseConnection().Transaction);

            foreach (DataRow CurrentRow in DiscountTable.Rows)
            {
                if ((string)CurrentRow[PcDiscountTable.GetDiscountCriteriaCodeDBName()] == "CHILD")
                {
                    DiscountAge = (int)CurrentRow[PcDiscountTable.GetUpToAgeDBName()];
                    return DiscountAge;
                }
            }

            return DiscountAge;
        }
    }
}