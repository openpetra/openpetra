//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Xml.XPath;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Server.MPersonnel.queries
{
    /// <summary>
    /// some methods for the length of commitment report
    /// </summary>
    public class QueryLengthOfCommitmentReport
    {
        private static Int32 ElapsedDays(TimeSpan value)
        {
            return Convert.ToInt32(value.TotalDays);

            /*
             * DateTime BaseDate = new DateTime(1, 1, 1);
             * DateTime endDate = BaseDate.AddTicks(Math.Abs(value.Ticks));
             *
             * if (endDate.AddDays(1).Month != endDate.Month)
             * {
             *  // add this full month
             *  return ((endDate.Year - 1) * 12) + endDate.Month;
             * }
             *
             * // don't include this month
             * return ((endDate.Year - 1) * 12) + endDate.Month - 1;
             */
        }

        /// <summary>
        /// return the date of a full year before or on the given current end date
        /// </summary>
        private static DateTime AnniversaryDate(int daysServed, DateTime ACurrentEndDate)
        {
            return ACurrentEndDate.AddDays(-1 * daysServed % 365);
        }

        private static void StoreCommitmentTotal(LengthOfCommitmentReportTDSPmStaffDataRow PreviousRow,
            Int32 daysServed,
            DateTime AReportStartDate,
            DateTime AReportEndDate,
            DateTime previousStartDate,
            DateTime? previousEndDate,
            LengthOfCommitmentReportTDSPmStaffDataTable Anniversaries,
            List <Int32>ASpecialAnniversaries)
        {
            DateTime ThisWorkerEndDate;

            if (!previousEndDate.HasValue)
            {
                daysServed += ElapsedDays(AReportEndDate - previousStartDate);

                ThisWorkerEndDate = AReportEndDate;
            }
            else
            {
                if (AReportEndDate < previousEndDate)
                {
                    daysServed += ElapsedDays(AReportEndDate - previousStartDate);
                    ThisWorkerEndDate = AReportEndDate;
                }
                else
                {
                    daysServed += ElapsedDays(previousEndDate.Value - previousStartDate);
                    ThisWorkerEndDate = previousEndDate.Value;
                }
            }

            // we need the date where an anniversary happens, in the report time
            int TotalYears = Convert.ToInt32(daysServed / 365);

            DateTime anniversaryDate = AnniversaryDate(daysServed, ThisWorkerEndDate);

            bool doStore = (
                (anniversaryDate.DayOfYear >= AReportStartDate.DayOfYear)
                && (anniversaryDate.DayOfYear <= AReportEndDate.DayOfYear));

            if (ASpecialAnniversaries.Count > 0)
            {
                if (!ASpecialAnniversaries.Contains(TotalYears))
                {
                    doStore = false;
                }
            }

            if (doStore)
            {
                // this is a special anniversary
                // store the number of years, and the anniversary date
                LengthOfCommitmentReportTDSPmStaffDataRow newAnniversary = Anniversaries.NewRowTyped();
                newAnniversary.Key = PreviousRow.Key;
                newAnniversary.SiteKey = PreviousRow.SiteKey;
                newAnniversary.PartnerKey = PreviousRow.PartnerKey;
                newAnniversary.TotalYears = TotalYears;
                newAnniversary.AnniversaryDay = anniversaryDate;
                newAnniversary.PartnerName = PreviousRow.PartnerName;
                newAnniversary.FirstName = PreviousRow.FirstName;
                newAnniversary.Surname = PreviousRow.Surname;
                newAnniversary.Gender = PreviousRow.Gender;
                Anniversaries.Rows.Add(newAnniversary);
            }
        }

        private static DataTable CalculateLengthOfCommitment(
            LengthOfCommitmentReportTDSPmStaffDataTable ACommitmentTable,
            DateTime AReportStartDate,
            DateTime AReportEndDate,
            List <Int32>ASpecialAnniversaries)
        {
            LengthOfCommitmentReportTDSPmStaffDataTable Anniversaries = new LengthOfCommitmentReportTDSPmStaffDataTable();

            // commitments are sorted by start date
            ACommitmentTable.DefaultView.Sort = LengthOfCommitmentReportTDSPmStaffDataTable.GetPartnerKeyDBName() + "," +
                                                LengthOfCommitmentReportTDSPmStaffDataTable.GetStartOfCommitmentDBName();

            LengthOfCommitmentReportTDSPmStaffDataRow PreviousRow = null;
            Int64 CurrentPartnerKey = -1;
            Int64 PreviousPartnerKey = -1;

            // add up time, ignore overlaps
            DateTime previousStartDate = DateTime.MinValue;
            DateTime? previousEndDate = new Nullable <DateTime>(); // null value: open ended commitment

            // we need the overall time that someone has worked with us
            // only count full months
            int monthsServed = 0;

            foreach (DataRowView rv in ACommitmentTable.DefaultView)
            {
                LengthOfCommitmentReportTDSPmStaffDataRow row = (LengthOfCommitmentReportTDSPmStaffDataRow)rv.Row;
                CurrentPartnerKey = row.PartnerKey;

                if (CurrentPartnerKey != PreviousPartnerKey)
                {
                    if (PreviousPartnerKey != -1)
                    {
                        StoreCommitmentTotal(
                            PreviousRow,
                            monthsServed,
                            AReportStartDate,
                            AReportEndDate,
                            previousStartDate,
                            previousEndDate,
                            Anniversaries,
                            ASpecialAnniversaries);
                    }

                    monthsServed = 0;
                    PreviousPartnerKey = CurrentPartnerKey;
                    PreviousRow = row;
                    previousStartDate = row.StartOfCommitment;
                    previousEndDate = row.IsEndOfCommitmentNull() ? new Nullable <DateTime>() : row.EndOfCommitment;
                }
                else
                {
                    // another commitment for the current person
                    if (!previousEndDate.HasValue)
                    {
                        // previous commitment was open ended
                        continue;
                    }

                    if (row.StartOfCommitment > AReportEndDate)
                    {
                        // outside of current period for this report
                        continue;
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
                            previousEndDate = row.IsEndOfCommitmentNull() ? new Nullable <DateTime>() : row.EndOfCommitment;
                        }
                    }
                    else
                    {
                        // store previous commitment time, and start a new commitment
                        monthsServed += ElapsedDays(previousEndDate.Value - previousStartDate);
                        previousStartDate = row.StartOfCommitment;
                        previousEndDate = row.IsEndOfCommitmentNull() ? new Nullable <DateTime>() : row.EndOfCommitment;
                    }
                }
            }

            if (PreviousRow != null)
            {
                StoreCommitmentTotal(
                    PreviousRow,
                    monthsServed,
                    AReportStartDate,
                    AReportEndDate,
                    previousStartDate,
                    previousEndDate,
                    Anniversaries,
                    ASpecialAnniversaries);
            }

            return Anniversaries;
        }

        /// <summary>
        /// get all partners and their commitment details
        /// </summary>
        public static DataTable GetLengthOfCommitment(TParameterList AParameters, TResultList AResults)
        {
            SortedList <string, string>Defines = new SortedList <string, string>();
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
                SqlParameterList.Add(new OdbcParameter("staffdate", OdbcType.Date)
                    {
                        Value = AParameters.Get("param_dtpCurrentStaff").ToDate()
                    });
                SqlParameterList.Add(new OdbcParameter("staffdate2", OdbcType.Date)
                    {
                        Value = AParameters.Get("param_dtpCurrentStaff").ToDate()
                    });
            }
            catch (Exception e)
            {
                TLogging.Log("problem while preparing sql statement for length of commitment report: " + e.ToString());
                return null;
            }

            string SqlStmt = TDataBase.ReadSqlFile("Personnel.Reports.AllCommitments.sql", Defines);
            Boolean NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                // now run the database query
                TLogging.Log("Getting the data from the database...", TLoggingType.ToStatusBar);

                LengthOfCommitmentReportTDSPmStaffDataTable CommitmentTable = new LengthOfCommitmentReportTDSPmStaffDataTable();
                DBAccess.GDBAccessObj.SelectDT(CommitmentTable, SqlStmt, Transaction,
                    SqlParameterList.ToArray(), 0, 0);

                // if this is taking a long time, every now and again update the TLogging statusbar, and check for the cancel button
                if (AParameters.Get("CancelReportCalculation").ToBool() == true)
                {
                    return null;
                }

                List <Int32>SpecialAnniversaries = new List <int>();

                if (AParameters.Get("param_chkAnniversaries").ToBool() == true)
                {
                    string[] Anniversaries = AParameters.Get("param_txtAnniversaries").ToString().Split(new char[] { ',', ';' });

                    foreach (string s in Anniversaries)
                    {
                        SpecialAnniversaries.Add(Convert.ToInt32(s.Trim()));
                    }
                }

                return CalculateLengthOfCommitment(
                    CommitmentTable,
                    AParameters.Get("param_dtpFromDate").ToDate(),
                    AParameters.Get("param_dtpToDate").ToDate(),
                    SpecialAnniversaries);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                return null;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }
    }
}