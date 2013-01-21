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
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.DB; // Implicit reference
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Server.MHospitality.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MHospitality.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Generates the table for the accommodation reports
    /// </summary>
    public class TAccommodationReportCalculation
    {
        private const String NO_ACCOMMODATION = "No Accomodation";
        // Data table which contains the results of the accomodation reports
        private DataTable FAccommodationTable;
        // Data table which contains the result of the accommodation report for each person
        private DataTable FAccommodationDetailTable;
        // List of partners which don't have an accommodation
        private ArrayList FNoAccommodationList;

        /// <summary>
        /// constructor
        /// </summary>
        public TAccommodationReportCalculation()
        {
        }

        /// <summary>
        /// Initialises the static properties for the accommodation report
        /// </summary>
        /// <remarks>
        /// The table which holds the result contains two fields for each date.
        /// The first field contains the number of people in the room.
        /// The second field contains the character indicating the gender of the persons in that room.
        /// </remarks>
        /// <param name="AFromDate">start date of the report</param>
        /// <param name="AToDate">end date of the report</param>
        /// <returns></returns>
        public bool InitAccomTable(DateTime AFromDate, DateTime AToDate)
        {
            // Init the Table
            FAccommodationTable = new DataTable("Accomodation Report");
            FAccommodationDetailTable = new DataTable("Accommodation Detail Table");
            FNoAccommodationList = new ArrayList();

            TimeSpan CheckLength = AToDate.Subtract(AFromDate);
            int DaysNumber = CheckLength.Days;

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "RoomName";
            FAccommodationTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Char");
            Column.DefaultValue = ' ';
            FAccommodationTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "PartnerName";
            FAccommodationDetailTable.Columns.Add(Column);

            for (int Counter = 0; Counter <= DaysNumber; ++Counter)
            {
                Column = new DataColumn();
                Column.DataType = Type.GetType("System.Int32");
                Column.ColumnName = AFromDate.AddDays(Counter).ToString("MMM-dd");
                Column.DefaultValue = 0;
                FAccommodationTable.Columns.Add(Column);
                // Add to each column a char as gender indicator
                Column = new DataColumn();
                Column.DataType = Type.GetType("System.Char");
                Column.DefaultValue = ' ';
                FAccommodationTable.Columns.Add(Column);

                Column = new DataColumn();
                Column.DataType = Type.GetType("System.String");
                Column.ColumnName = AFromDate.AddDays(Counter).ToString("MMM-dd");
                Column.DefaultValue = "";
                FAccommodationDetailTable.Columns.Add(Column);
            }

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Decimal");
            Column.ColumnName = "Total Cost";
            Column.DefaultValue = 0.0;
            FAccommodationTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Char");
            Column.DefaultValue = ' ';
            FAccommodationTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.ColumnName = "Venue";
            Column.DefaultValue = "";
            FAccommodationTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            FAccommodationDetailTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "RoomName";
            FAccommodationDetailTable.Columns.Add(Column);

            DataRow TmpRow = FAccommodationTable.NewRow();

            TmpRow[0] = NO_ACCOMMODATION;
            FAccommodationTable.Rows.Add(TmpRow);

            return true;
        }

        /// <summary>
        /// Main entry point for calculating the accommodation report.
        /// This must be called for each partner in each conference.
        /// </summary>
        /// <param name="AConferenceKey">Conference Key of the current conference to examine</param>
        /// <param name="AStartDate">Start date of the conference</param>
        /// <param name="AEndDate">End date of the conference</param>
        /// <param name="AFromDate">Start date of the report</param>
        /// <param name="AToDate">End date of the report</param>
        /// <param name="APartnerKey">Partner Key of the current partner to examine</param>
        /// <param name="AReportDetail">Indicator of the details of the report. Possible options: Brief, Full, Detail</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        public bool CalculatePartnerAccom(long AConferenceKey,
            DateTime AStartDate, DateTime AEndDate,
            DateTime AFromDate, DateTime AToDate,
            long APartnerKey, string AReportDetail,
            ref TRptSituation ASituation)
        {
            PcAttendeeTable AttendeeTable;
            PcAttendeeRow AttendeeRow = null;
            PmShortTermApplicationTable ShortTermTable;
            PmShortTermApplicationTable TmpTable = new PmShortTermApplicationTable();
            PmShortTermApplicationRow TemplateRow;


            TemplateRow = TmpTable.NewRowTyped(false);
            TemplateRow.PartnerKey = APartnerKey;
            TemplateRow.StConfirmedOption = AConferenceKey;

            if (FAccommodationTable == null)
            {
                InitAccomTable(AFromDate, AToDate);
            }

            AttendeeTable = PcAttendeeAccess.LoadByPrimaryKey(AConferenceKey, APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            if (AttendeeTable.Rows.Count > 0)
            {
                AttendeeRow = (PcAttendeeRow)AttendeeTable.Rows[0];
            }

            ShortTermTable = PmShortTermApplicationAccess.LoadUsingTemplate(TemplateRow, ASituation.GetDatabaseConnection().Transaction);

            foreach (DataRow Row in ShortTermTable.Rows)
            {
                AddPartnerToAccom((PmShortTermApplicationRow)Row, AttendeeRow, AStartDate, AEndDate,
                    AFromDate, AToDate, AReportDetail, ref ASituation);
            }

            return true;
        }

        /// <summary>
        /// Adds the dates of one partner to the accommodation report
        /// </summary>
        /// <param name="AShortTermRow">The short term application row of the current partner</param>
        /// <param name="AAttendeeRow">The attendee row of the current partner</param>
        /// <param name="AStartDate">Start date of the conference</param>
        /// <param name="AEndDate">End date of the conference</param>
        /// <param name="AFromDate">Start date of the report</param>
        /// <param name="AToDate">End date of the report</param>
        /// <param name="AReportDetail">Indicator of the details of the report. Possible options: Brief, Full, Detail</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        private bool AddPartnerToAccom(PmShortTermApplicationRow AShortTermRow,
            PcAttendeeRow AAttendeeRow,
            DateTime AStartDate, DateTime AEndDate,
            DateTime AFromDate, DateTime AToDate,
            string AReportDetail, ref TRptSituation ASituation)
        {
            // if we have an actual arrival and departure date from the attendee row use it.
            if (AAttendeeRow != null)
            {
                if (!AAttendeeRow.IsActualArrNull())
                {
                    AShortTermRow.Arrival = AAttendeeRow.ActualArr;
                }

                if (!AAttendeeRow.IsActualDepNull())
                {
                    AShortTermRow.Departure = AAttendeeRow.ActualDep;
                }
            }

            if (AShortTermRow.IsArrivalNull())
            {
                AShortTermRow.Arrival = AStartDate;
            }

            if (AShortTermRow.IsDepartureNull())
            {
                AShortTermRow.Departure = AEndDate;
            }

            if ((AShortTermRow.Arrival <= AToDate)
                && (AShortTermRow.Departure >= AFromDate))
            {
                // this short term application covers the dates we examine

                PcRoomAllocTable TempTable = new PcRoomAllocTable();
                PcRoomAllocTable RoomAllocTable;
                PcRoomAllocRow TemplateRow = TempTable.NewRowTyped(false);
                TemplateRow.PartnerKey = AShortTermRow.PartnerKey;
                TemplateRow.ConferenceKey = AShortTermRow.StConfirmedOption;

                RoomAllocTable = PcRoomAllocAccess.LoadUsingTemplate(TemplateRow, ASituation.GetDatabaseConnection().Transaction);

                char Gender;
                int Age;
                string PartnerName;
                GetGenderAndAge(AShortTermRow.PartnerKey, AStartDate, out Gender, out Age, ref ASituation);
                PartnerName = TAccommodationReportCalculation.GetPartnerShortName(AShortTermRow.PartnerKey, ref ASituation);

                foreach (DataRow Row in RoomAllocTable.Rows)
                {
                    CheckRoomAllocation((PcRoomAllocRow)Row, AShortTermRow, AFromDate, AToDate,
                        AReportDetail, Gender, Age, PartnerName, ref ASituation);
                }

                if (RoomAllocTable.Rows.Count == 0)
                {
                    AddNoRoomBooking(AShortTermRow, AFromDate, AToDate, Gender, PartnerName);
                }
            }

            return true;
        }

        /// <summary>
        /// Adds one partner to the result which has no room booking at all.
        /// </summary>
        /// <param name="AShortTermRow">The short term application row of the current partner</param>
        /// <param name="AFromDate">Start date of the report</param>
        /// <param name="AToDate">End date of the report</param>
        /// <param name="AGender">Gender of the current person to examine</param>
        /// <param name="APartnerName">Name of the current person</param>
        /// <returns></returns>
        private bool AddNoRoomBooking(PmShortTermApplicationRow AShortTermRow,
            DateTime AFromDate, DateTime AToDate, char AGender,
            string APartnerName)
        {
            int RoomRow = 0;
            bool FirstDayOfNoAccom = true;

            TimeSpan CheckLength = AToDate.Subtract(AFromDate);
            int DaysNumber = CheckLength.Days;
            int MaxCollumns = FAccommodationTable.Columns.Count - 2;

            DataRow DetailedRow = FAccommodationDetailTable.NewRow();

            DetailedRow["RoomName"] = NO_ACCOMMODATION;
            DetailedRow["PartnerName"] = "   " + APartnerName;

            // Check each day if the partner is at the conference
            for (int Counter = 0; Counter <= DaysNumber; ++Counter)
            {
                DateTime CurrentDay = AFromDate.AddDays(Counter);

                if ((CurrentDay >= AShortTermRow.Arrival)
                    && (CurrentDay < AShortTermRow.Departure))
                {
                    // the partner is here at this date but has no booking
                    if (Counter < MaxCollumns)
                    {
                        FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2] =
                            (int)FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2] + 1;
                        FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2 + 1] =
                            GetGenderOfBooking(AGender, (char)FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2 + 1]);

                        DetailedRow[Counter + 1] = "**";

                        if (FirstDayOfNoAccom)
                        {
                            AddToNoAccomList(AShortTermRow.PartnerKey, APartnerName, CurrentDay);
                            FirstDayOfNoAccom = false;
                        }
                    }
                }
            }

            FAccommodationDetailTable.Rows.Add(DetailedRow);

            return true;
        }

        /// <summary>
        /// Adds the room allocation of one partner to the result.
        /// </summary>
        /// <param name="ARoomAllocRow">The room allocation row of the current partner</param>
        /// <param name="AShortTermRow">The short term application row of the current partner</param>
        /// <param name="AFromDate">Start date of the report</param>
        /// <param name="AToDate">End date of the report</param>
        /// <param name="AReportDetail">Indicator of the details of the report. Possible options: Brief, Full, Detail</param>
        /// <param name="AGender">Gender of the current person to examine</param>
        /// <param name="AAge">Age of the person to examine</param>
        /// <param name="APartnerName">Name of the current person</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        private bool CheckRoomAllocation(PcRoomAllocRow ARoomAllocRow, PmShortTermApplicationRow AShortTermRow,
            DateTime AFromDate, DateTime AToDate, string AReportDetail, char AGender,
            int AAge, string APartnerName, ref TRptSituation ASituation)
        {
            int RoomRow = GetRowIndexForRoom(ARoomAllocRow, AReportDetail, ref ASituation);

            TimeSpan CheckLength = AToDate.Subtract(AFromDate);
            int DaysNumber = CheckLength.Days;
            int MaxCollumns = FAccommodationTable.Columns.Count - 2;
            int NumberOfBookedDays = 0;
            bool FirstDayOfNoAccom = true;

            DataRow DetailRow = FAccommodationDetailTable.NewRow();

            DetailRow["RoomName"] = FAccommodationTable.Rows[RoomRow]["RoomName"];
            DetailRow["PartnerName"] = "   " + APartnerName;
            DataRow DetailRowNoBooking = FAccommodationDetailTable.NewRow();
            DetailRowNoBooking["RoomName"] = NO_ACCOMMODATION;
            DetailRowNoBooking["PartnerName"] = "   " + APartnerName;

            // Check each day for the booking
            for (int Counter = 0; Counter <= DaysNumber; ++Counter)
            {
                DateTime CurrentDay = AFromDate.AddDays(Counter);

                if ((CurrentDay >= ARoomAllocRow.In)
                    && (CurrentDay < ARoomAllocRow.Out))
                {
                    // there is a room booking for this person for this day
                    if (Counter < MaxCollumns)
                    {
                        FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2] =
                            (int)FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2] + 1;
                        FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2 + 1] =
                            GetGenderOfBooking(AGender, (char)FAccommodationTable.Rows[RoomRow][(Counter + 1) * 2 + 1]);
                        NumberOfBookedDays++;

                        DetailRow[Counter + 1] = "**";
                    }
                }
                else
                {
                    // there is no room booking
                    if ((Counter < MaxCollumns)
                        && (CurrentDay >= AShortTermRow.Arrival)
                        && (CurrentDay < AShortTermRow.Departure))
                    {
                        FAccommodationTable.Rows[0][(Counter + 1) * 2] =
                            (int)FAccommodationTable.Rows[0][(Counter + 1) * 2] + 1;
                        FAccommodationTable.Rows[0][(Counter + 1) * 2 + 1] =
                            GetGenderOfBooking(AGender, (char)FAccommodationTable.Rows[0][(Counter + 1) * 2 + 1]);

                        DetailRowNoBooking[Counter + 1] = "**";

                        if (FirstDayOfNoAccom)
                        {
                            AddToNoAccomList(AShortTermRow.PartnerKey, APartnerName, CurrentDay);
                            FirstDayOfNoAccom = false;
                        }
                    }
                }
            }

            CalculateRoomCost(ARoomAllocRow, NumberOfBookedDays, RoomRow, AAge, AShortTermRow.StConfirmedOption, ref ASituation);

            FAccommodationDetailTable.Rows.Add(DetailRow);

            if (!FirstDayOfNoAccom)
            {
                // Add this partner to the "no accomodation" list there is one day without.
                FAccommodationDetailTable.Rows.Add(DetailRowNoBooking);
            }

            return true;
        }

        /// <summary>
        /// Retrieves the gender and age of one person.
        /// </summary>
        /// <param name="APartnerKey">Partner key of the person to examine</param>
        /// <param name="AStartDate">Start date of the conference. The age is calculated at the start date</param>
        /// <param name="AGender">Gender of the current person</param>
        /// <param name="AAge">Age of the current person</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        private bool GetGenderAndAge(long APartnerKey, DateTime AStartDate, out char AGender, out int AAge,
            ref TRptSituation ASituation)
        {
            bool ReturnValue = false;

            AGender = ' ';
            AAge = 0;

            PPersonTable PersonTable;
            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            if (PersonTable.Rows.Count > 0)
            {
                PPersonRow PersonRow = (PPersonRow)PersonTable.Rows[0];
                AGender = PersonRow.Gender.ToCharArray()[0];

                if (!PersonRow.IsDateOfBirthNull())
                {
                    AAge = Calculations.CalculateAge(PersonRow.DateOfBirth.Value, AStartDate);
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the character of the gender sign of a room.
        /// </summary>
        /// <param name="APersonGender">the gender of the person will be added to the room</param>
        /// <param name="ARoomBookingGender">the gender of the persons that are already in the room</param>
        /// <returns></returns>
        private char GetGenderOfBooking(char APersonGender, char ARoomBookingGender)
        {
            char ReturnValue = APersonGender;

            if (ARoomBookingGender == ' ')
            {
                ReturnValue = APersonGender;
            }
            else if (ARoomBookingGender != APersonGender)
            {
                ReturnValue = 'X';
            }

            return ReturnValue;
        }

        /// <summary>
        /// Calculates the cost of the room for one person for the given number of days.
        /// </summary>
        /// <param name="ARow">Room allocation row for the room booking</param>
        /// <param name="ANumberOfBookedDays">number of booked days</param>
        /// <param name="ARoomRow">the row index on the result table to which this calculation refers</param>
        /// <param name="AAge">age of the person who is in the room</param>
        /// <param name="AConferenceKey">conference key of the current conference</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        private bool CalculateRoomCost(PcRoomAllocRow ARow, int ANumberOfBookedDays, int ARoomRow,
            int AAge, long AConferenceKey, ref TRptSituation ASituation)
        {
            PcRoomTable RoomTable;

            RoomTable = PcRoomAccess.LoadByPrimaryKey(ARow.VenueKey, ARow.BuildingCode, ARow.RoomNumber,
                ASituation.GetDatabaseConnection().Transaction);

            if (RoomTable.Rows.Count > 0)
            {
                PcRoomRow RoomRow = (PcRoomRow)RoomTable.Rows[0];

                decimal cost = RoomRow.BedCost * ANumberOfBookedDays;
                decimal ChildDiscount;
                bool InPercent;

                if (TAccommodationReportCalculation.GetChildDiscount(AAge, AConferenceKey, "ACCOMMODATION", out ChildDiscount, out InPercent,
                        ref ASituation))
                {
                    if (InPercent)
                    {
                        cost = cost * (100 - ChildDiscount) / 100;
                    }
                    else
                    {
                        // At the moment we ignore if the child discount is not set up as percent
                        cost = RoomRow.BedCost * ANumberOfBookedDays;
                    }
                }

                FAccommodationTable.Rows[ARoomRow]["Total Cost"] =
                    (decimal)FAccommodationTable.Rows[ARoomRow]["Total Cost"] + cost;
            }

            return true;
        }

        /// <summary>
        /// Gets the row index for the result table to which this room booking refers.
        /// </summary>
        /// <param name="ARow">room booking row</param>
        /// <param name="AReportDetail">Indicator of the details of the report. Possible options: Brief, Full, Detail</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        private int GetRowIndexForRoom(PcRoomAllocRow ARow, string AReportDetail, ref TRptSituation ASituation)
        {
            // Check if this room is already in the table

            String RoomName = ARow.BuildingCode;

            if (AReportDetail != "Brief")
            {
                RoomName = ARow.BuildingCode + " / " + ARow.RoomNumber;
            }

            int RoomRow = -1;

            for (int Counter = 0; Counter < FAccommodationTable.Rows.Count; ++Counter)
            {
                if (FAccommodationTable.Rows[Counter][0].ToString() == RoomName)
                {
                    RoomRow = Counter;
                    break;
                }
            }

            if (RoomRow == -1)
            {
                DataRow NewRow = FAccommodationTable.NewRow();
                NewRow[0] = RoomName;
                NewRow["Venue"] = TAccommodationReportCalculation.GetPartnerShortName(ARow.VenueKey, ref ASituation);

                FAccommodationTable.Rows.Add(NewRow);
                RoomRow = FAccommodationTable.Rows.Count - 1;
            }

            return RoomRow;
        }

        /// <summary>
        /// Retrieves the short name of a partner.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        public static String GetPartnerShortName(Int64 APartnerKey, ref TRptSituation ASituation)
        {
            String ReturnValue;
            PPartnerTable table;
            StringCollection fields;

            ReturnValue = "N/A";
            fields = new StringCollection();
            fields.Add(PPartnerTable.GetPartnerShortNameDBName());
            table = PPartnerAccess.LoadByPrimaryKey(APartnerKey, fields, ASituation.GetDatabaseConnection().Transaction);

            if (table.Rows.Count > 0)
            {
                ReturnValue = table[0].PartnerShortName;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the child discount for
        /// </summary>
        /// <param name="AAge">age of the person</param>
        /// <param name="AConferenceKey">conference key</param>
        /// <param name="ACostType">Defines the type of discount (e.g. ACCOMMODATION or CONFERENCE)</param>
        /// <param name="ADiscount">discount the person gets</param>
        /// <param name="AInPercent">Type of discaount: True - discount is in percent. False - discount is the amount</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        public static bool GetChildDiscount(int AAge, long AConferenceKey, String ACostType,
            out decimal ADiscount, out bool AInPercent, ref TRptSituation ASituation)
        {
            ADiscount = 0.0M;
            AInPercent = false;

            PcDiscountTable DiscountTable;

            PcDiscountTable TmpTable = new PcDiscountTable();
            PcDiscountRow TemplateRow = TmpTable.NewRowTyped(false);

            TemplateRow.ConferenceKey = AConferenceKey;
            TemplateRow.CostTypeCode = ACostType;
            TemplateRow.DiscountCriteriaCode = "CHILD";
            TemplateRow.Validity = "ALWAYS";

            StringCollection OrderList = new StringCollection();
            OrderList.Add(" ORDER BY " + PcDiscountTable.GetUpToAgeDBName() + " ASC");

            DiscountTable = PcDiscountAccess.LoadUsingTemplate(TemplateRow, null, null,
                ASituation.GetDatabaseConnection().Transaction, OrderList, 0, 0);

            foreach (PcDiscountRow DiscountRow in DiscountTable.Rows)
            {
                if ((!DiscountRow.IsUpToAgeNull())
                    && (DiscountRow.UpToAge >= AAge))
                {
                    ADiscount = DiscountRow.Discount;
                    AInPercent = DiscountRow.Percentage;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add the details of the partner to the no room allocation list
        /// </summary>
        /// <param name="APartnerKey">PartnerKey</param>
        /// <param name="APartnerName">Name of the current person</param>
        /// <param name="ADate">First date where the partner has no room allocation</param>
        /// <returns>true</returns>
        private bool AddToNoAccomList(long APartnerKey, string APartnerName, DateTime ADate)
        {
            FNoAccommodationList.Add(APartnerKey.ToString() + "   " + APartnerName + "   (beginning) " +
                ADate.ToString("dd-MMM-yyyy"));
            return true;
        }

        /// <summary>
        /// Transfers the result of the accommodation table to the report results
        /// </summary>
        /// <param name="ADetailLevel">Indicator if we have a brief, full or detail accommodation report</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns>true</returns>
        public bool FinishAccomTable(String ADetailLevel, ref TRptSituation ASituation)
        {
            ASituation.GetResults().Clear();

            if (FAccommodationTable == null)
            {
                return false;
            }

            int ChildRow = 1;
            int NumColumns = FAccommodationTable.Columns.Count / 2;

            if (ADetailLevel == "Detail")
            {
                // Don't show the cost column if we have a detailed report
                NumColumns--;
            }

            String PreviousVenueName = "";

            DataRow[] SortedRows = FAccommodationTable.Select("", "Venue DESC");

            foreach (DataRow CurrentRow in SortedRows)
            {
                if (CurrentRow[0].ToString() == NO_ACCOMMODATION)
                {
                    // ignore the row with no accomodation here
                    continue;
                }

                if (CurrentRow["Venue"].ToString() != PreviousVenueName)
                {
                    PreviousVenueName = (String)CurrentRow["Venue"];

                    if (ChildRow > 1)
                    {
                        InsertEmptyRow(NumColumns, ChildRow++, "", ref ASituation);
                    }

                    InsertEmptyRow(NumColumns, ChildRow++, PreviousVenueName, ref ASituation);
                }

                InsertDataRow(NumColumns, ChildRow++, CurrentRow, ref ASituation);

                if (ADetailLevel == "Detail")
                {
                    InsertDetailDataRow(NumColumns, ref ChildRow, CurrentRow["RoomName"].ToString(), ref ASituation);
                }
            }

            InsertEmptyRow(NumColumns, ChildRow++, "", ref ASituation);

            foreach (DataRow CurrentRow in FAccommodationTable.Rows)
            {
                if (CurrentRow[0].ToString() != NO_ACCOMMODATION)
                {
                    continue;
                }

                InsertDataRow(NumColumns, ChildRow++, CurrentRow, ref ASituation);
                break;
            }

            if (ADetailLevel == "Full")
            {
                InsertEmptyRow(NumColumns, ChildRow++, "", ref ASituation);

                TVariant[] Header = new TVariant[NumColumns];
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns = new TVariant[NumColumns];

                for (int Counter = 0; Counter < NumColumns; ++Counter)
                {
                    Columns[Counter] = new TVariant();
                    Header[Counter] = new TVariant();
                }

                Columns[0] = new TVariant("");
                Columns[1] = new TVariant("People with accommodation not allocated for their actual time at the conference:");
                ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                    Header, Description, Columns);

                foreach (String NoAccom in FNoAccommodationList)
                {
                    Header = new TVariant[NumColumns];
                    Description[0] = new TVariant();
                    Description[1] = new TVariant();
                    Columns = new TVariant[NumColumns];

                    for (int Counter = 0; Counter < NumColumns; ++Counter)
                    {
                        Columns[Counter] = new TVariant();
                        Header[Counter] = new TVariant();
                    }

                    Columns[0] = new TVariant("");
                    Columns[1] = new TVariant(NoAccom);
                    ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                        Header, Description, Columns);
                }
            }
            else if (ADetailLevel == "Detail")
            {
                InsertDetailDataRow(NumColumns, ref ChildRow, NO_ACCOMMODATION, ref ASituation);
            }

            return true;
        }

        /// <summary>
        /// Insert an empty row into the report results
        /// </summary>
        /// <param name="ANumColumns">Number of columns the report has</param>
        /// <param name="AChildRow">The child row index</param>
        /// <param name="AVenueName">The first column entry</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns>true</returns>
        private bool InsertEmptyRow(int ANumColumns, int AChildRow, String AVenueName, ref TRptSituation ASituation)
        {
            TVariant[] Header = new TVariant[ANumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[ANumColumns];

            for (int Counter = 0; Counter < ANumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(" ");
                Header[Counter] = new TVariant();
            }

            Columns[0] = new TVariant(AVenueName);

            ASituation.GetResults().AddRow(0, AChildRow, true, 2, "", "", false,
                Header, Description, Columns);
            return true;
        }

        /// <summary>
        /// Insert the values of a data row into the report results.
        /// The data row contains for each room / venue the bookings.
        /// </summary>
        /// <param name="ANumColumns">Number of columns the report has</param>
        /// <param name="AChildRow">Index of the child row</param>
        /// <param name="ADataRow">The data row which contains the values</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns>true</returns>
        private bool InsertDataRow(int ANumColumns, int AChildRow, DataRow ADataRow, ref TRptSituation ASituation)
        {
            TVariant[] Header = new TVariant[ANumColumns];
            TVariant[] Description =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] Columns = new TVariant[ANumColumns];

            for (int Counter = 0; Counter < ANumColumns; ++Counter)
            {
                Columns[Counter] = new TVariant(ADataRow[Counter * 2].ToString() + ADataRow[Counter * 2 + 1].ToString());
                Header[Counter] = new TVariant();
            }

            ASituation.GetResults().AddRow(0, AChildRow, true, 2, "", "", false,
                Header, Description, Columns);
            return true;
        }

        /// <summary>
        /// Insert the values of the detail data row if we have "Detail" as report level.
        /// The detail row contains for each partner the room bookings.
        /// </summary>
        /// <param name="ANumColumns">Number of columns the report has</param>
        /// <param name="AChildRow">Index of the child row</param>
        /// <param name="ARoomName">The room name of which to add the details to the table</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns>true</returns>
        private bool InsertDetailDataRow(int ANumColumns, ref int AChildRow, string ARoomName, ref TRptSituation ASituation)
        {
            foreach (DataRow DetailRow in FAccommodationDetailTable.Rows)
            {
                if (DetailRow["RoomName"].ToString() != ARoomName)
                {
                    continue;
                }

                TVariant[] Header = new TVariant[ANumColumns];
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns = new TVariant[ANumColumns];

                for (int Counter = 0; Counter < ANumColumns; ++Counter)
                {
                    Columns[Counter] = new TVariant(DetailRow[Counter].ToString());
                    Header[Counter] = new TVariant();
                }

                ASituation.GetResults().AddRow(0, AChildRow++, true, 2, "", "", false,
                    Header, Description, Columns);
            }

            return true;
        }
    }
}