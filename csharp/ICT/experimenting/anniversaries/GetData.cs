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
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using Ict.Common.DB;
using Ict.Common;
using Ict.Petra.Plugins.SQL;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

namespace anniversaries
{
public class TDataAnniversaries
{
    public const string BIRTHDAYTABLE = "birthday";
    public const string ANNIVERSARYTABLE = "anniversaries";
    private static TDataBase db;

    /// open the db connection
    public static bool InitDBConnection(
        string ADBUsername, string ADBPassword)
    {
        // establish connection to database
        TAppSettingsManager settings = new TAppSettingsManager(false);

        db = new TDataBase();

        new TLogging("debug.log");
        db.DebugLevel = settings.GetInt16("DebugLevel", 0);

        TDBType dbtype = CommonTypes.ParseDBType(settings.GetValue("Server.RDBMSType"));

        if (dbtype != TDBType.ProgressODBC)
        {
            throw new Exception("at the moment only Progress ODBC db is supported");
        }

        db.EstablishDBConnection(dbtype,
            settings.GetValue("Server.ODBC_DSN"),
            "",
            ADBUsername,
            ADBPassword,
            "");
        DBAccess.GDBAccessObj = db;

        return true;
    }

    /// retrieve all special birthdays of staff in the given time
    public static DataSet GetBirthdays(
        Int64 AFieldPartnerKey,
        DateTime AStartDate, DateTime AEndDate,
        int[] ASpecialBirthdays)
    {
        List <Int64>StaffPartnerKeys = GetFamiliesOfAllCurrentStaff(AStartDate, AFieldPartnerKey);

        DataTable BirthdayTable = GetBirthdays(StaffPartnerKeys);

        DataTable FilteredBirthdays = FilterBirthdays(BirthdayTable, AStartDate, AEndDate, ASpecialBirthdays);

        DataSet ResultDataset = new DataSet();
        ResultDataset.Tables.Add(FilteredBirthdays);

        TSQLTools.GetBestAddress(ResultDataset, FilteredBirthdays.TableName, FilteredBirthdays.Columns[0].ColumnName);

        return ResultDataset;
    }

    private static bool IsOpenEndedCommitment(DateTime ACommitmentEndDate)
    {
        return ACommitmentEndDate == DateTime.MaxValue;
    }

    public static int TotalMonths(TimeSpan value)
    {
        DateTime BaseDate = new DateTime(1, 1, 1);
        DateTime endDate = BaseDate.AddTicks(Math.Abs(value.Ticks));

        if (endDate.AddDays(1).Month != endDate.Month)
        {
            // add this full month
            return ((endDate.Year - 1) * 12) + endDate.Month;
        }

        // don't include this month
        return ((endDate.Year - 1) * 12) + endDate.Month - 1;
    }

    public static int TotalYears(TimeSpan value)
    {
        DateTime BaseDate = new DateTime(1, 1, 1);
        DateTime endDate = BaseDate.AddTicks(Math.Abs(value.Ticks));

        return endDate.Year - 1;
    }

    /// <summary>
    /// return the date of a full year before or on the given current end date
    /// </summary>
    public static DateTime AnniversaryDate(int monthsServed, DateTime ACurrentEndDate)
    {
        return ACurrentEndDate.AddMonths(-1 * monthsServed % 12);
    }

    /// retrieve all special anniversaries of staff in the given time
    public static DataSet GetAnniversaries(
        Int64 AFieldPartnerKey,
        DateTime AReportStartDate, DateTime AReportEndDate,
        int[] ASpecialAnniversaries)
    {
        List <Int64>StaffPartnerKeys = GetAllCurrentStaff(AReportStartDate, AReportEndDate, AFieldPartnerKey);

        DataTable Anniversaries = new DataTable(ANNIVERSARYTABLE);
        Anniversaries.Columns.Add(new DataColumn("PartnerKey", typeof(Int64)));
        Anniversaries.Columns.Add(new DataColumn("TotalYears", typeof(Int16)));
        Anniversaries.Columns.Add(new DataColumn("AnniversaryDay", typeof(DateTime)));
        Anniversaries.Columns.Add(new DataColumn("Surname", typeof(string)));
        Anniversaries.Columns.Add(new DataColumn("Firstname", typeof(string)));

        foreach (Int64 WorkerPartnerKey in StaffPartnerKeys)
        {
            // get all commitment records for each worker
            PmStaffDataTable commitments = GetAllCommitmentRecordsOfWorker(WorkerPartnerKey);

            // commitments are sorted by start date
            // we know that all those workers are working during the report period (AReportStartDate, AReportEndDate)

            // add up time, ignore overlaps
            DateTime previousStartDate = commitments[0].StartOfCommitment;
            DateTime previousEndDate = DateTime.MaxValue; // MaxValue: open ended commitment

            // we need the overall time that someone has worked with us
            // only count full months
            int monthsServed = 0;

            previousEndDate = commitments[0].IsEndOfCommitmentNull() ? DateTime.MaxValue : commitments[0].EndOfCommitment;

            int Counter = 1;

            while (Counter < commitments.Count && !IsOpenEndedCommitment(previousEndDate))
            {
                PmStaffDataRow row = commitments[Counter];

                if (row.StartOfCommitment > AReportEndDate)
                {
                    // outside of current period for this report
                    break;
                }

                // overlapping commitment?
                if (row.StartOfCommitment <= previousEndDate)
                {
                    if (!row.IsEndOfCommitmentNull() && (row.EndOfCommitment <= previousEndDate))
                    {
                        // ignore this commitment, since it is already covered by the previous commitment
                    }
                    else
                    {
                        // modify previous commitment to be longer
                        previousEndDate = row.IsEndOfCommitmentNull() ? DateTime.MaxValue : row.EndOfCommitment;
                    }
                }
                else
                {
                    // store previous commitment time, and start a new commitment
                    monthsServed += TotalMonths(previousEndDate - previousStartDate);
                    previousStartDate = row.StartOfCommitment;
                    previousEndDate = row.IsEndOfCommitmentNull() ? DateTime.MaxValue : row.EndOfCommitment;
                }

                Counter++;
            }

            DateTime ThisWorkerEndDate;

            if (IsOpenEndedCommitment(previousEndDate))
            {
                monthsServed += TotalMonths(AReportEndDate - previousStartDate);
                ThisWorkerEndDate = AReportEndDate;
            }
            else
            {
                if (AReportEndDate < previousEndDate)
                {
                    monthsServed += TotalMonths(AReportEndDate - previousStartDate);
                    ThisWorkerEndDate = AReportEndDate;
                }
                else
                {
                    monthsServed += TotalMonths(previousEndDate - previousStartDate);
                    ThisWorkerEndDate = previousEndDate;
                }
            }

            // we need the date where an anniversary happens, in the report time
            int TotalYears = Convert.ToInt32(monthsServed / 12);
            DateTime anniversaryDate = AnniversaryDate(monthsServed, ThisWorkerEndDate);

            foreach (int specialanniversary in ASpecialAnniversaries)
            {
                if ((TotalYears == specialanniversary)
                    && (anniversaryDate >= AReportStartDate))
                {
                    // this is a special anniversary
                    // store the number of years, and the anniversary date
                    DataRow newAnniversary = Anniversaries.NewRow();
                    newAnniversary["PartnerKey"] = WorkerPartnerKey;
                    newAnniversary["TotalYears"] = TotalYears;
                    newAnniversary["AnniversaryDay"] = anniversaryDate;

                    PPersonTable PersonTable;
                    TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);
                    PersonTable = PPersonAccess.LoadByPrimaryKey(WorkerPartnerKey, transaction);
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    newAnniversary["Surname"] = PersonTable[0].FamilyName;
                    newAnniversary["Firstname"] = PersonTable[0].FirstName;
                    Anniversaries.Rows.Add(newAnniversary);
                    break;
                }
            }
        }

        DataSet ResultDataset = new DataSet();
        ResultDataset.Tables.Add(Anniversaries);

        TSQLTools.GetBestAddress(ResultDataset, Anniversaries.TableName, Anniversaries.Columns[0].ColumnName);

        return ResultDataset;
    }

    /// <summary>
    /// return a list of family keys of current workers
    /// </summary>
    /// <param name="AStaffCurrentDate"></param>
    /// <param name="AFieldPartnerKey"></param>
    /// <returns></returns>
    private static List <Int64>GetFamiliesOfAllCurrentStaff(DateTime AStaffCurrentDate, Int64 AFieldPartnerKey)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        string stmt = TDataBase.ReadSqlFile("CurrentWorkersFamilies.sql");

        OdbcParameter[] parameters = new OdbcParameter[5];
        parameters[0] = new OdbcParameter("StaffCurrentDate", OdbcType.Date);
        parameters[0].Value = AStaffCurrentDate;
        parameters[1] = new OdbcParameter("StaffCurrentDate", OdbcType.Date);
        parameters[1].Value = AStaffCurrentDate;
        parameters[2] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[2].Value = AFieldPartnerKey;
        parameters[3] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[3].Value = AFieldPartnerKey;
        parameters[4] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[4].Value = AFieldPartnerKey;
        DataTable ResultTable = DBAccess.GDBAccessObj.SelectDT(stmt, "worker", transaction,
            parameters);

        DBAccess.GDBAccessObj.RollbackTransaction();

        List <Int64>ResultList = new List <Int64>();

        foreach (DataRow row in ResultTable.Rows)
        {
            ResultList.Add(Convert.ToInt64(row["FamilyKey"]));
        }

        return ResultList;
    }

    /// <summary>
    /// return a list of person keys of current workers
    /// </summary>
    /// <param name="AReportStartDate"></param>
    /// <param name="AReportEndDate"></param>
    /// <param name="AFieldPartnerKey"></param>
    /// <returns></returns>
    private static List <Int64>GetAllCurrentStaff(DateTime AReportStartDate, DateTime AReportEndDate, Int64 AFieldPartnerKey)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        string stmt = TDataBase.ReadSqlFile("CurrentWorkers.sql");

        OdbcParameter[] parameters = new OdbcParameter[10];
        parameters[0] = new OdbcParameter("ReportStartDate", OdbcType.Date);
        parameters[0].Value = AReportStartDate;
        parameters[1] = new OdbcParameter("ReportStartDate", OdbcType.Date);
        parameters[1].Value = AReportEndDate;
        parameters[2] = new OdbcParameter("ReportEndDate", OdbcType.Date);
        parameters[2].Value = AReportEndDate;
        parameters[3] = new OdbcParameter("ReportStartDate", OdbcType.Date);
        parameters[3].Value = AReportEndDate;
        parameters[4] = new OdbcParameter("ReportEndDate", OdbcType.Date);
        parameters[4].Value = AReportEndDate;
        parameters[5] = new OdbcParameter("ReportStartDate", OdbcType.Date);
        parameters[5].Value = AReportEndDate;
        parameters[6] = new OdbcParameter("ReportEndDate", OdbcType.Date);
        parameters[6].Value = AReportEndDate;
        parameters[7] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[7].Value = AFieldPartnerKey;
        parameters[8] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[8].Value = AFieldPartnerKey;
        parameters[9] = new OdbcParameter("FieldPartnerKey", OdbcType.Decimal, 10);
        parameters[9].Value = AFieldPartnerKey;
        DataTable ResultTable = DBAccess.GDBAccessObj.SelectDT(stmt, "worker", transaction,
            parameters);

        DBAccess.GDBAccessObj.RollbackTransaction();

        List <Int64>ResultList = new List <Int64>();

        foreach (DataRow row in ResultTable.Rows)
        {
            ResultList.Add(Convert.ToInt64(row["PartnerKey"]));
        }

        return ResultList;
    }

    /// <summary>
    /// get all commitment records of the worker
    /// </summary>
    /// <param name="APartnerKey"></param>
    /// <returns></returns>
    private static PmStaffDataTable GetAllCommitmentRecordsOfWorker(Int64 APartnerKey)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        PmStaffDataTable stafftable;

        stafftable = PmStaffDataAccess.LoadViaPPerson(APartnerKey, null, transaction,
            StringHelper.StrSplit("ORDER BY pm_start_of_commitment_d ASC", ","), 0, 0);

        DBAccess.GDBAccessObj.RollbackTransaction();

        return stafftable;
    }

    /// <summary>
    /// get the birthdays of staff and all direct family members
    /// </summary>
    /// <param name="AWorkers"></param>
    /// <returns></returns>
    private static DataTable GetBirthdays(List <Int64>AWorkers)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        string stmt = TDataBase.ReadSqlFile("BirthdaysOfFamilyMembers.sql");

        OdbcParameter[] parameters = new OdbcParameter[1];

        DataTable ResultTable = null;

        foreach (Int64 workerPartnerKey in AWorkers)
        {
            parameters[0] = new OdbcParameter("WorkerPartnerKey", workerPartnerKey);
            DataTable tempTable = DBAccess.GDBAccessObj.SelectDT(stmt, BIRTHDAYTABLE, transaction,
                parameters);

            if (ResultTable == null)
            {
                ResultTable = tempTable;
            }
            else
            {
                ResultTable.Merge(tempTable);
            }
        }

        DBAccess.GDBAccessObj.RollbackTransaction();

        return ResultTable;
    }

    /// <summary>
    /// get all people with special birthdays in the given time period
    /// </summary>
    /// <param name="AWorkersAndFamilyMembersBirthdays"></param>
    /// <param name="AStartPeriod"></param>
    /// <param name="AEndPeriod"></param>
    /// <param name="ASpecialAge"></param>
    /// <returns></returns>
    private static DataTable FilterBirthdays(DataTable AWorkersAndFamilyMembersBirthdays,
        DateTime AStartPeriod, DateTime AEndPeriod,
        int[] ASpecialAge)
    {
        int Counter = 0;

        while (Counter < AWorkersAndFamilyMembersBirthdays.Rows.Count)
        {
            bool HasSpecialBirthday = false;
            DataRow row = AWorkersAndFamilyMembersBirthdays.Rows[Counter++];

            if ((row["DOBThisYear"] != System.DBNull.Value)
                && (Convert.ToInt32(row["DOBThisYear"]) >= AStartPeriod.DayOfYear)
                && (Convert.ToInt32(row["DOBThisYear"]) <= AEndPeriod.DayOfYear))
            {
                int age = AEndPeriod.Year - Convert.ToDateTime(row["DOB"]).Year;

                foreach (int specialbirthday in ASpecialAge)
                {
                    if (age == specialbirthday)
                    {
                        HasSpecialBirthday = true;
                    }
                }
            }

            if (!HasSpecialBirthday)
            {
                AWorkersAndFamilyMembersBirthdays.Rows.Remove(row);
                Counter--;
            }
        }

        return AWorkersAndFamilyMembersBirthdays;
    }
}
}

/*
 * Get the birthdays of workers; include family members
 *   what about previous workers?
 * Get the anniversaries of joining the organisation
 *   what about anniversaries of wifes?
 *   what about leaving and rejoining? Only count the time that people have been with the organisation
 *   what about diceased workers and their families?
 *
 *
 * run by cronjob once a month:
 *
 * birthdays:
 * First get all workers at given date
 * Then get the birthdays of all members of those families (get family key first?)
 * If birthday is in list of special birthdays and happens in the given time period, add to list
 *
 * anniversaries:
 * get all workers
 * get the earliest commitment on record for each worker
 * if the anniversary is in the list and in the given period of time, add to list
 *
 * get contact details for each on the list
 *
 * send email to personnel department
 *
 */