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
using System.Data;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MReporting;
using Ict.Common.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Calculations of ages summary report.
    /// It calculates the data withing FAgeTable and transfers it
    /// to reports with "FinishAgeTable"
    /// </summary>
    public class TAgeSummaryCalculation
    {
        private static DataTable FAgeTable;
        private static DateTime FEarliestBirthday;
        private static DateTime FLatestBirthday;

        const int COLUMNAGE = 0;
        const int COLUMNTOTAL = 1;
        const int COLUMNFEMALE = 2;
        const int COLUMNMALE = 3;
        const int COLUMNOTHER = 4;

        /// <summary>
        /// constructor
        /// </summary>
        public TAgeSummaryCalculation()
        {
            FEarliestBirthday = DateTime.MaxValue;
            FLatestBirthday = DateTime.MinValue;

            InitAgeTable();
        }

        /// <summary>
        /// Adds one partner to the report table
        /// </summary>
        /// <param name="ABirthday">The birthday of the current partner</param>
        /// <param name="AGender">Gender of the current partner</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        public bool CalculateSingleAge(DateTime ABirthday, String AGender, ref TRptSituation ASituation)
        {
            bool FoundAge = false;

            int Age = Calculations.CalculateAge(ABirthday);

            for (int Counter = 0; Counter < FAgeTable.Rows.Count; ++Counter)
            {
                if ((int)FAgeTable.Rows[Counter][COLUMNAGE] == Age)
                {
                    FoundAge = true;
                    AddAttendeeToTable(Counter, AGender, Age);
                }
            }

            if (!FoundAge)
            {
                AddAttendeeToTable(-1, AGender, Age);
            }

            if (FEarliestBirthday.CompareTo(ABirthday) > 0)
            {
                FEarliestBirthday = ABirthday;
            }

            if (FLatestBirthday.CompareTo(ABirthday) < 0)
            {
                FLatestBirthday = ABirthday;
            }

            return true;
        }

        /// <summary>
        /// Copies the result of the FAgeTable to report.
        /// </summary>
        /// <returns></returns>
        public bool FinishAgeTable(ref TRptSituation ASituation)
        {
            ASituation.GetResults().Clear();

            if (FAgeTable == null)
            {
                return false;
            }

            int ChildRow = 1;

            foreach (DataRow CurrentRow in FAgeTable.Rows)
            {
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

                ASituation.GetResults().AddRow(0, ChildRow++, true, 2, "", "", false,
                    Header, Description, Columns);
            }

            TVariant[] LastHeader = new TVariant[5];
            TVariant[] LastDescription =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] LastColumns = new TVariant[5];

            for (int Counter = 0; Counter < 5; ++Counter)
            {
                LastColumns[Counter] = new TVariant(" ");
                LastHeader[Counter] = new TVariant();
            }

            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);

            LastColumns[0] = new TVariant("Youngest person was born on: " + FLatestBirthday.ToString("dd-MMM-yyyy"));

            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);

            LastColumns[0] = new TVariant(" ");
            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);

            LastColumns[0] = new TVariant("Oldest person was born on: " + FEarliestBirthday.ToString("dd-MMM-yyyy"));

            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);


            return true;
        }

        private bool InitAgeTable()
        {
            // Init the Table
            FAgeTable = new DataTable("Age Summary");

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Age";
            FAgeTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Total";
            FAgeTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Female";
            FAgeTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Male";
            FAgeTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Other";
            FAgeTable.Columns.Add(Column);

            return true;
        }

        /// <summary>
        /// Adds the result of one partner to the FAgeTable
        /// </summary>
        /// <param name="ARowIndex">Index to which row in FAgeTable the data needs to be added.
        /// if it is less than 0 then a new row will be added</param>
        /// <param name="AGender">Gender of the partner</param>
        /// <param name="AAge">The age of the partner</param>
        /// <returns></returns>
        private bool AddAttendeeToTable(int ARowIndex, String AGender, int AAge)
        {
            if (ARowIndex < 0)
            {
                DataRow NewRow = FAgeTable.NewRow();

                NewRow[COLUMNAGE] = AAge;
                FAgeTable.Rows.Add(NewRow);
                ARowIndex = FAgeTable.Rows.Count - 1;
            }

            FAgeTable.Rows[ARowIndex][COLUMNTOTAL] = (int)
                                                     FAgeTable.Rows[ARowIndex][COLUMNTOTAL] + 1;

            int GenderColumn = COLUMNOTHER;

            if (AGender == "Male")
            {
                GenderColumn = COLUMNMALE;
            }
            else if (AGender == "Female")
            {
                GenderColumn = COLUMNFEMALE;
            }

            FAgeTable.Rows[ARowIndex][GenderColumn] = (int)
                                                      FAgeTable.Rows[ARowIndex][GenderColumn] + 1;

            return true;
        }
    }
}

/*
 * CVS HISTORY
 *
 * $Log: AgeSummaryCalculation.cs,v $
 * Revision 1.1  2010/03/03 13:40:19  berndr
 * Add calculations for generating some summary conference reports.
 *
 *
 *
 */