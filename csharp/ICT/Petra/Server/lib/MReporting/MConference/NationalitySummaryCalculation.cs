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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Ict.Common;
using Ict.Petra.Server.MReporting;
using Ict.Common.Data;
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MReporting.MPersonnel;

namespace Ict.Petra.Server.MReporting.MConference
{
    /// <summary>
    /// Calculations of nationality summary report.
    /// It calculates the data withing FNationalityTable and transfers it
    /// to reports with "FinishNationalityTable"
    /// </summary>
    public class TNationalitySummaryCalculation
    {
        private static DataTable FNationalityTable;
        private static DataTable FLanguagesTable;

        const int COLUMNNATIONALITY = 0;
        const int COLUMNTOTALNATIONALITIES = 1;
        const int COLUMNFEMALE = 2;
        const int COLUMNMALE = 3;
        const int COLUMNOTHER = 4;
        const int COLUMNLANGUAGES = 5;
        const int COLUMNNATIONALITYCODE = 6;

        /// <summary>
        /// constructor
        /// </summary>
        public TNationalitySummaryCalculation()
        {
            InitNationalityTable();
        }

        /// <summary>
        /// Adds one partner to the report table
        /// </summary>
        /// <param name="APartnerKey">The partner key of the current partner</param>
        /// <param name="AGender">Gender of the current partner</param>
        /// <param name="ALanguageCode">The mother language of the current partner</param>
        /// <param name="ASituation">The current report situation</param>
        /// <returns></returns>
        public bool CalculateNationalities(long APartnerKey, String AGender, String ALanguageCode, ref TRptSituation ASituation)
        {
            TCacheable CommonCacheable = new TCacheable();

            StringCollection PassportColumns = StringHelper.StrSplit(
                PmPassportDetailsTable.GetDateOfIssueDBName() + "," +
                PmPassportDetailsTable.GetDateOfExpirationDBName() + "," +
                PmPassportDetailsTable.GetPassportNationalityCodeDBName() + "," +
                PmPassportDetailsTable.GetMainPassportDBName(), ",");

            PmPassportDetailsTable PassportDetailsDT = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey,
                PassportColumns, ASituation.GetDatabaseConnection().Transaction, null, 0, 0);

            string Nationalities = Ict.Petra.Shared.MPersonnel.Calculations.DeterminePersonsNationalities(
                @CommonCacheable.GetCacheableTable, PassportDetailsDT);

            string[] NationalitiesArray = Nationalities.Split(',');

            bool FoundNationality = false;
            List <string>PreviousNationalities = new List <string>();

            foreach (string Nationality in NationalitiesArray)
            {
                string SingleNationality = Nationality;

                // remove all characters other than the country name
                SingleNationality = Nationality.TrimStart(' ');

                if (SingleNationality.EndsWith(Ict.Petra.Shared.MPersonnel.Calculations.PASSPORT_EXPIRED))
                {
                    SingleNationality = SingleNationality.Remove(SingleNationality.Length -
                        Ict.Petra.Shared.MPersonnel.Calculations.PASSPORT_EXPIRED.Length);
                }
                else if (SingleNationality.EndsWith(Ict.Petra.Shared.MPersonnel.Calculations.PASSPORTMAIN_EXPIRED))
                {
                    SingleNationality = SingleNationality.Remove(SingleNationality.Length -
                        Ict.Petra.Shared.MPersonnel.Calculations.PASSPORTMAIN_EXPIRED.Length);
                }

                if (SingleNationality == "")
                {
                    SingleNationality = Catalog.GetString("UNKNOWN");
                }

                // make sure that this nationality has not already been processed for this partner
                if (!PreviousNationalities.Contains(SingleNationality))
                {
                    for (int Counter = 0; Counter < FNationalityTable.Rows.Count; ++Counter)
                    {
                        if (((String)FNationalityTable.Rows[Counter][COLUMNNATIONALITYCODE] == SingleNationality))
                        {
                            FoundNationality = true;
                            AddAttendeeToTable(Counter, AGender, SingleNationality, ALanguageCode);
                        }
                    }

                    if (!FoundNationality)
                    {
                        AddAttendeeToTable(-1, AGender, SingleNationality, ALanguageCode);
                    }

                    PreviousNationalities.Add(SingleNationality);
                }
            }

            return true;
        }

        /// <summary>
        /// Copies the result of the FNationalityTable to report.
        /// </summary>
        /// <returns></returns>
        public bool FinishNationalityTable(ref TRptSituation ASituation)
        {
            ASituation.GetResults().Clear();

            if (FNationalityTable == null)
            {
                return false;
            }

            int ChildRow = 1;

            foreach (DataRow CurrentRow in FNationalityTable.Rows)
            {
                String NationalityCode = (String)CurrentRow[COLUMNNATIONALITYCODE];
                CurrentRow[COLUMNNATIONALITY] = NationalityCode; // + "  " + GetCountryName(NationalityCode, ref ASituation);

                TVariant[] Header = new TVariant[6];
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns = new TVariant[6];

                for (int Counter = 0; Counter < 5; ++Counter)
                {
                    Columns[Counter] = new TVariant(CurrentRow[Counter].ToString());
                    Header[Counter] = new TVariant();
                }

                Columns[5] = new TVariant(GetLanguagesSummary(NationalityCode));
                Header[5] = new TVariant();

                ASituation.GetResults().AddRow(0, ChildRow++, true, 1, "", "", false,
                    Header, Description, Columns);
            }

            return true;
        }

        private bool InitNationalityTable()
        {
            // Init the Table
            FNationalityTable = new DataTable("Nationality Report");

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "Nationality";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Total";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Female";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Male";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Other";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "Languages";
            FNationalityTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "NationalityCode";
            FNationalityTable.Columns.Add(Column);

            // Init the language table
            // first column defines the nationality
            // second column defines the language spoken by the partner who has this nationality
            // third column counts the number of these languages
            FLanguagesTable = new DataTable();

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "NationalityCode";
            FLanguagesTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.DefaultValue = "";
            Column.ColumnName = "LanguageCode";
            FLanguagesTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.DefaultValue = 0;
            Column.ColumnName = "Languages";
            FLanguagesTable.Columns.Add(Column);

            return true;
        }

        /// <summary>
        /// Adds the result of one partner to the FNationalityTable
        /// </summary>
        /// <param name="ARowIndex">Index to which row in FNationalityTable the data needs to be added.
        /// if it is less than 0 then a new row will be added</param>
        /// <param name="AGender">Gender of the partner</param>
        /// <param name="ANationalityCode">The nationality code from the passport of the partner</param>
        /// <param name="ALanguageCode">The mother language of the partner</param>
        /// <returns></returns>
        private bool AddAttendeeToTable(int ARowIndex, String AGender,
            String ANationalityCode, String ALanguageCode)
        {
            if (ARowIndex < 0)
            {
                DataRow NewRow = FNationalityTable.NewRow();

                NewRow[COLUMNNATIONALITYCODE] = ANationalityCode;
                FNationalityTable.Rows.Add(NewRow);
                ARowIndex = FNationalityTable.Rows.Count - 1;
            }

            FNationalityTable.Rows[ARowIndex][COLUMNTOTALNATIONALITIES] = (int)
                                                                          FNationalityTable.Rows[ARowIndex][COLUMNTOTALNATIONALITIES] + 1;

            int GenderColumn = COLUMNOTHER;

            if (AGender == "Male")
            {
                GenderColumn = COLUMNMALE;
            }
            else if (AGender == "Female")
            {
                GenderColumn = COLUMNFEMALE;
            }

            FNationalityTable.Rows[ARowIndex][GenderColumn] = (int)
                                                              FNationalityTable.Rows[ARowIndex][GenderColumn] + 1;

            AddAttendeeToLanguageTable(ANationalityCode, ALanguageCode);

            return true;
        }

        private bool AddAttendeeToLanguageTable(String ANationalityCode, String ALanguageCode)
        {
            int RowIndex = -1;

            for (int Counter = 0; Counter < FLanguagesTable.Rows.Count; ++Counter)
            {
                if (((String)FLanguagesTable.Rows[Counter][0] == ANationalityCode)
                    && ((String)FLanguagesTable.Rows[Counter][1] == ALanguageCode))
                {
                    RowIndex = Counter;
                    break;
                }
            }

            if (RowIndex == -1)
            {
                DataRow NewRow = FLanguagesTable.NewRow();

                NewRow[0] = ANationalityCode;
                NewRow[1] = ALanguageCode;
                NewRow[2] = 1;

                FLanguagesTable.Rows.Add(NewRow);
            }
            else
            {
                FLanguagesTable.Rows[RowIndex][2] = (int)FLanguagesTable.Rows[RowIndex][2] + 1;
            }

            return true;
        }

        private String GetCountryName(String ACountryCode, ref TRptSituation ASituation)
        {
            PCountryTable CountryTable;

            CountryTable = PCountryAccess.LoadByPrimaryKey(ACountryCode, ASituation.GetDatabaseConnection().Transaction);

            if (CountryTable.Rows.Count > 0)
            {
                return (String)CountryTable.Rows[0][PCountryTable.GetCountryNameDBName()];
            }

            return "Unknown";
        }

        /// <summary>
        /// Get all the languages and numbers from the language table which have the nationality code
        /// </summary>
        /// <param name="ANationalityCode"></param>
        /// <returns></returns>
        private String GetLanguagesSummary(String ANationalityCode)
        {
            String ReturnValue = "";

            foreach (DataRow Row in FLanguagesTable.Rows)
            {
                if ((String)Row[0] == ANationalityCode)
                {
                    ReturnValue = ReturnValue + (String)Row[1] + " " + Row[2].ToString() + "   ";
                }
            }

            return ReturnValue;
        }
    }
}