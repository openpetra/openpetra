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
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Common.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Calculations of conference role report.
    /// It calculates the data withing FConferenceTable and transfers it
    /// to reports with "FinishConferenceRoleTable"
    /// </summary>
    public class TConferenceRoleCalculation
    {
        private DataTable FConferenceRoleTable;

        const int COLUMNROLE = 0;
        const int COLUMNGENDER = 1;
        const int COLUMNFAMILYKEY = 2;
        const int COLUMNOPTIONCODE = 3;

        /// <summary>
        /// constructor
        /// </summary>
        public TConferenceRoleCalculation()
        {
            InitConferenceRoleTable();
        }

        /// <summary>
        /// Adds one partner to the report table
        /// </summary>
        /// <param name="AGender">Gender of the current partner</param>
        /// <param name="AFamilyKey">The Family Key the person belongs to</param>
        /// <param name="AConferenceRole">The role the person has at the conference</param>
        /// <param name="AConferenceKey">The key of the current conference to examine</param>
        /// <param name="AOptionCode">The partner key of the confirmed outreach option of the attendee</param>
        /// <returns></returns>
        public bool CalculateSingleConferenceRole(String AGender, long AFamilyKey,
            String AConferenceRole, long AConferenceKey,
            long AOptionCode)
        {
            DataRow NewRow = FConferenceRoleTable.NewRow();

            NewRow[COLUMNROLE] = AConferenceRole;
            NewRow[COLUMNGENDER] = AGender;
            NewRow[COLUMNFAMILYKEY] = AFamilyKey;
            NewRow[COLUMNOPTIONCODE] = AOptionCode;

            FConferenceRoleTable.Rows.Add(NewRow);

            return true;
        }

        /// <summary>
        /// Copies the result of the FConferenceRoleTable to report.
        /// </summary>
        /// <param name="ASelectedOutreachOptions">csv list of the selected outreach options</param>
        /// <param name="ASituation">current report situation</param>
        /// <returns></returns>
        public bool FinishConferenceRoleTable(String ASelectedOutreachOptions, ref TRptSituation ASituation)
        {
            ASituation.GetResults().Clear();

            if (FConferenceRoleTable == null)
            {
                return false;
            }

            DataTable ResultTable = MakeResultTable(ASelectedOutreachOptions);

            int ChildRow = 1;
            int[] Totals = new int[] {
                0, 0, 0, 0, 0, 0
            };

            foreach (DataRow CurrentRow in ResultTable.Rows)
            {
                TVariant[] Header = new TVariant[6];
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns = new TVariant[6];

                for (int Counter = 0; Counter < 6; ++Counter)
                {
                    Columns[Counter] = new TVariant(CurrentRow[Counter].ToString());
                    Header[Counter] = new TVariant();

                    if (Counter > 0)
                    {
                        Totals[Counter] = Totals[Counter] + (int)CurrentRow[Counter];
                    }
                }

                ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                    Header, Description, Columns);
            }

            // Add empty Row
            TVariant[] LastHeader = new TVariant[6];
            TVariant[] LastDescription =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] LastColumns = new TVariant[6];

            for (int Counter = 0; Counter < 6; ++Counter)
            {
                LastColumns[Counter] = new TVariant(" ");
                LastHeader[Counter] = new TVariant();
            }

            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);
            // Add Total Row

            LastColumns[0] = new TVariant("Total");

            for (int Counter = 1; Counter < 6; ++Counter)
            {
                LastColumns[Counter] = new TVariant(Totals[Counter]);
            }

            ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                LastHeader, LastDescription, LastColumns);
            return true;
        }

        private bool InitConferenceRoleTable()
        {
            // Init the Table
            FConferenceRoleTable = new DataTable("Conference Role");

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "Role";
            FConferenceRoleTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "Gender";
            FConferenceRoleTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int64");
            Column.DefaultValue = 0;
            Column.ColumnName = "FamilyKey";
            FConferenceRoleTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int64");
            Column.DefaultValue = 0;
            Column.ColumnName = "OptionCode";
            FConferenceRoleTable.Columns.Add(Column);

            return true;
        }

        /// <summary>
        /// Returns a table that represents the reporting result
        /// </summary>
        /// <param name="ASelectedOutreachOptions">csv list of the selected outreach options. Only persons
        /// with this outreach option will be counted into the report</param>
        /// <returns></returns>
        private DataTable MakeResultTable(String ASelectedOutreachOptions)
        {
            DataTable ResultTable = new DataTable("Result Table");

            DataColumn NewColumn = new DataColumn("Role", Type.GetType("System.String"));

            ResultTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Total", Type.GetType("System.Int32"));
            ResultTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Female", Type.GetType("System.Int32"));
            ResultTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Male", Type.GetType("System.Int32"));
            ResultTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Couple", Type.GetType("System.Int32"));
            ResultTable.Columns.Add(NewColumn);
            NewColumn = new DataColumn("Family", Type.GetType("System.Int32"));
            ResultTable.Columns.Add(NewColumn);

            int ColumnToAdd = 0;

            foreach (DataRow Row in FConferenceRoleTable.Rows)
            {
                switch (GetGroup((long)Row[COLUMNFAMILYKEY], (String)Row[COLUMNGENDER]))
                {
                    case 'F':
                        // Female
                        ColumnToAdd = 2;
                        break;

                    case 'M':
                        // Male
                        ColumnToAdd = 3;
                        break;

                    case 'C':
                        // Couple
                        ColumnToAdd = 4;
                        break;

                    default:
                        // Family
                        ColumnToAdd = 5;
                        break;
                }

                if (!IsOutreachOptionSelected(ASelectedOutreachOptions, Row[COLUMNOPTIONCODE].ToString()))
                {
                    continue;
                }

                int RowIndex = -1;

                for (int Counter = 0; Counter < ResultTable.Rows.Count; ++Counter)
                {
                    if ((String)ResultTable.Rows[Counter][0] == (String)Row[COLUMNROLE])
                    {
                        RowIndex = Counter;
                        break;
                    }
                }

                AddResultRow(ref ResultTable, RowIndex, ColumnToAdd, (String)Row[COLUMNROLE]);
            }

            return ResultTable;
        }

        /// <summary>
        /// Add one row to the result table
        /// </summary>
        /// <param name="AResultTable"></param>
        /// <param name="ARowIndex"></param>
        /// <param name="AColumnToAdd"></param>
        /// <param name="ARole"></param>
        /// <returns></returns>
        private bool AddResultRow(ref DataTable AResultTable, int ARowIndex, int AColumnToAdd, String ARole)
        {
            if (ARowIndex < 0)
            {
                DataRow NewRow = AResultTable.NewRow();

                NewRow[0] = ARole;
                NewRow[1] = 0;
                NewRow[2] = 0;
                NewRow[3] = 0;
                NewRow[4] = 0;
                NewRow[5] = 0;

                AResultTable.Rows.Add(NewRow);

                ARowIndex = AResultTable.Rows.Count - 1;
            }

            AResultTable.Rows[ARowIndex][1] = (int)AResultTable.Rows[ARowIndex][1] + 1;

            AResultTable.Rows[ARowIndex][AColumnToAdd] = (int)AResultTable.Rows[ARowIndex][AColumnToAdd] + 1;

            return true;
        }

        /// <summary>
        /// Count the number of partner in FConferenceTable who have the same FamilyKey
        /// This decides if it is a single person (1), couple (2) or family (more than 2)
        /// </summary>
        /// <param name="AFamilyKey"></param>
        /// <param name="AGender"></param>
        /// <returns></returns>
        private char GetGroup(long AFamilyKey, String AGender)
        {
            int Familymembers = 0;
            char ReturnValue;

            foreach (DataRow Row in FConferenceRoleTable.Rows)
            {
                if ((long)Row[COLUMNFAMILYKEY] == AFamilyKey)
                {
                    Familymembers++;
                }
            }

            switch (Familymembers)
            {
                case 1:

                    if (AGender == "Male")
                    {
                        ReturnValue = 'M';
                    }
                    else
                    {
                        ReturnValue = 'F';
                    }

                    break;

                case 2:
                    ReturnValue = 'C';
                    break;

                default:
                    ReturnValue = 'A';
                    break;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if the current outreach option is in the list of the selected outreach options.
        /// </summary>
        /// <param name="ASelectedOutreachOptions"></param>
        /// <param name="ACurrentOutreachOption"></param>
        /// <returns></returns>
        private bool IsOutreachOptionSelected(String ASelectedOutreachOptions, String ACurrentOutreachOption)
        {
            if (ASelectedOutreachOptions.Length == 0)
            {
                // If we have no selected outreachoptions this means, user selected "all conferences"
                // from the report.
                return true;
            }

            if (ASelectedOutreachOptions.Contains(ACurrentOutreachOption))
            {
                return true;
            }

            return false;
        }
    }
}