/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using Ict.Common.DB;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace treasurerEmails
{
public class TGetTreasurerData
{
    const string GIFTSTABLE = "giftsums";
    const string TREASURERTABLE = "treasurer";

    /// <summary>
    /// open the db connection, and retrieve all sums of donations for each treasurer for the last x months
    /// <param name="ADBUsername"></param>
    /// <param name="ADBPassword"></param>
    /// <param name="ALedgerNumber"></param>
    /// <param name="AMotivationGroup"></param>
    /// <param name="AMotivationDetail"></param>
    /// <param name="ALastDonationDate"></param>
    /// <param name="ANumberMonths"></param>
    /// <returns></returns>
    public static DataSet GetTreasurerData(
        string ADBUsername, string ADBPassword,
        Int32 ALedgerNumber,
        string AMotivationGroup,
        string AMotivationDetail,
        DateTime ALastDonationDate, Int16 ANumberMonths)
    {
        // establish connection to database
        TAppSettingsManager settings = new TAppSettingsManager();


        TDataBase db = new TDataBase();

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

        //db.DebugLevel = 10;

        // calculate the first and the last days of the months range
        // if last donation date is not the last of the month, go back to the last day of the previous month
        DateTime EndDate = new DateTime(ALastDonationDate.Year, ALastDonationDate.Month, ALastDonationDate.Day);

        if (DateTime.DaysInMonth(EndDate.Year, EndDate.Month) != EndDate.Day)
        {
            EndDate = EndDate.AddMonths(-1);
            EndDate = new DateTime(EndDate.Year, EndDate.Month, DateTime.DaysInMonth(EndDate.Year, EndDate.Month));
        }

        DateTime StartDate = EndDate.AddMonths(-1 * ANumberMonths);
        StartDate = new DateTime(StartDate.Year, StartDate.Month, 1);

        DataTable GiftsTable = GetAllGiftsForRecipientPerMonthByMotivation(ALedgerNumber, AMotivationGroup, AMotivationDetail, StartDate, EndDate);

        DataSet ResultDataset = new DataSet();
        ResultDataset.Tables.Add(GiftsTable);

        // add the last date of the month to the table
        AddMonthDate(ref GiftsTable, ALedgerNumber);

        // get the treasurer(s) for each recipient; get their name and partner key
        AddTreasurer(ref ResultDataset);
        DataTable TreasurerTable = ResultDataset.Tables[TREASURERTABLE];

        // get the name of each recipient
        AddRecipientName(ref TreasurerTable);

        // use GetBestAddress to get the email address of the treasurer
        AddTreasurerEmail(ref TreasurerTable);

        return ResultDataset;
    }

    private static string ReadSqlFile(string ASqlFilename)
    {
        StreamReader reader = new StreamReader(ASqlFilename);
        string line = null;
        string stmt = "";

        while ((line = reader.ReadLine()) != null)
        {
            if (!line.Trim().StartsWith("--"))
            {
                stmt += line.Trim() + " ";
            }
        }

        reader.Close();
        return stmt;
    }

    /// <summary>
    /// Get the sum of all gifts per recipient per month, by specified motivation and time span
    /// </summary>
    /// <param name="ALedgerNumber"></param>
    /// <param name="AMotivationGroup"></param>
    /// <param name="AMotivationDetail"></param>
    /// <param name="AStartDate"></param>
    /// <param name="AEndDate"></param>
    /// <returns> returns a table with columns:
    ///    RecipientKey, MonthAmount, FinancialYear, FinancialPeriod</returns>
    private static DataTable GetAllGiftsForRecipientPerMonthByMotivation(
        Int32 ALedgerNumber,
        string AMotivationGroup,
        string AMotivationDetail,
        DateTime AStartDate,
        DateTime AEndDate)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        string stmt = ReadSqlFile("GetAllGiftsForRecipientPerMonthByMotivation.sql");

        OdbcParameter[] parameters = new OdbcParameter[5];
        parameters[0] = new OdbcParameter("Ledger", ALedgerNumber);
        parameters[1] = new OdbcParameter("MotivationGroup", AMotivationGroup);
        parameters[2] = new OdbcParameter("MotivationDetail", AMotivationDetail);
        parameters[3] = new OdbcParameter("StartDate", AStartDate);
        parameters[4] = new OdbcParameter("EndDate", AEndDate);
        DataTable ResultTable = DBAccess.GDBAccessObj.SelectDT(stmt, GIFTSTABLE, transaction,
            parameters);


        DBAccess.GDBAccessObj.RollbackTransaction();

        return ResultTable;
    }

    /// <summary>
    /// The previous query only retrieves the financial year number and period number,
    /// but we want the last date of that period to be added to the result table
    /// </summary>
    /// <param name="ResultTable"></param>
    /// <param name="ALedgerNumber"></param>
    private static void AddMonthDate(ref DataTable ResultTable, Int32 ALedgerNumber)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        // get the accounting period table to know the month
        AAccountingPeriodTable periods = AAccountingPeriodAccess.LoadAll(transaction);

        OdbcParameter[] parameters = new OdbcParameter[1];
        parameters[0] = new OdbcParameter("Ledger", ALedgerNumber);
        Int32 currentFinancialYear =
            Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT a_current_financial_year_i FROM PUB_a_ledger WHERE a_ledger_number_i = ?",
                    transaction, parameters));

        ResultTable.Columns.Add("MonthDate", typeof(DateTime));

        foreach (DataRow row in ResultTable.Rows)
        {
            Int32 yearNr = Convert.ToInt32(row[ResultTable.Columns["FinancialYear"].Ordinal]);
            Int32 periodNr = Convert.ToInt32(row[ResultTable.Columns["FinancialPeriod"].Ordinal]);
            DateTime monthDate = DateTime.MinValue;

            foreach (AAccountingPeriodRow period in periods.Rows)
            {
                if ((period.LedgerNumber == ALedgerNumber) && (period.AccountingPeriodNumber == periodNr))
                {
                    monthDate = period.PeriodEndDate;
                }
            }

            if (yearNr != currentFinancialYear)
            {
                // substract the years to get the right date
                TLogging.Log("substract year " + yearNr.ToString() + " " + currentFinancialYear.ToString() + " " + monthDate.Year.ToString() + " " +
                    row[ResultTable.Columns["RecipientKey"].Ordinal].ToString());
                monthDate = monthDate.AddYears(-1 * (currentFinancialYear - yearNr));
            }

            row[ResultTable.Columns["MonthDate"].Ordinal] = monthDate;
        }

        DBAccess.GDBAccessObj.RollbackTransaction();
    }

    /// <summary>
    /// get the name of the recipient and add to the table
    /// </summary>
    /// <param name="ResultTable"></param>
    private static void AddRecipientName(ref DataTable ResultTable)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        ResultTable.Columns.Add("RecipientName", typeof(string));

        foreach (DataRow row in ResultTable.Rows)
        {
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("PartnerKey", row[ResultTable.Columns["RecipientKey"].Ordinal]);
            string shortname = DBAccess.GDBAccessObj.ExecuteScalar(
                "SELECT p_partner_short_name_c FROM PUB_p_partner WHERE p_partner_key_n = ?",
                transaction, parameters).ToString();

            row[ResultTable.Columns["RecipientName"].Ordinal] = shortname;
        }

        DBAccess.GDBAccessObj.RollbackTransaction();
    }

    /// <summary>
    /// get the treasurer(s) for each recipient;
    /// get their name and partner key
    /// </summary>
    /// <param name="Result"></param>
    private static void AddTreasurer(ref DataSet Result)
    {
        TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        // to check if the recipient has been processed already
        SortedList <Int64, Int64>recipients = new SortedList <Int64, Int64>();

        DataTable GiftSumsTable = Result.Tables[GIFTSTABLE];

        foreach (DataRow row in GiftSumsTable.Rows)
        {
            Int64 recipientKey = Convert.ToInt64(row[GiftSumsTable.Columns["RecipientKey"].Ordinal]);

            if (recipients.ContainsKey(recipientKey))
            {
                continue;
            }

            recipients.Add(recipientKey, recipientKey);
            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("PartnerKey", recipientKey);

            string stmt = ReadSqlFile("TreasurerOfWorker.sql");

            DataTable TreasurerTable = DBAccess.GDBAccessObj.SelectDT(stmt,
                TREASURERTABLE, transaction,
                parameters);

            if (TreasurerTable.Rows.Count > 1)
            {
                throw new Exception("more than one treasurer for " + row[GiftSumsTable.Columns["RecipientKey"].Ordinal].ToString());
            }

            if (TreasurerTable.Rows.Count == 1)
            {
                if (!Result.Tables.Contains(TREASURERTABLE))
                {
                    Result.Tables.Add(TreasurerTable);
                }
                else
                {
                    Result.Tables[TREASURERTABLE].Merge(TreasurerTable);
                }
            }
            else
            {
                // cannot find treasurer
                TLogging.Log("cannot find treasurer for partner " + row[GiftSumsTable.Columns["RecipientKey"].Ordinal].ToString());
            }
        }

        DBAccess.GDBAccessObj.RollbackTransaction();
    }

    /// <summary>
    /// get the email address of the treasurer and add to the table
    /// </summary>
    /// <param name="ResultTable"></param>
    private static void AddTreasurerEmail(ref DataTable ResultTable)
    {
        ResultTable.Columns.Add("TreasurerEmail", typeof(string));

        foreach (DataRow row in ResultTable.Rows)
        {
            if (row[ResultTable.Columns["TreasurerKey"].Ordinal] != DBNull.Value)
            {
                string emailAddress = GetBestEmailAddress(Convert.ToInt64(row[ResultTable.Columns["TreasurerKey"].Ordinal]));
                row[ResultTable.Columns["TreasurerEmail"].Ordinal] = emailAddress;
            }
        }
    }

    private static string GetBestEmailAddress(Int64 APartnerKey)
    {
        string EmailAddress = "";
        TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        DataSet PartnerLocationsDS = new DataSet();

        PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
        DataTable PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];

        // add special column BestAddress and Icon
        PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
        PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

        // find all locations of the partner, put it into a dataset
        PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, Transaction);

        Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
        Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(PartnerLocationsDS);

        foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
        {
            // find the row with BestAddress = 1
            if (Convert.ToInt32(row["BestAddress"]) == 1)
            {
                EmailAddress = row.EmailAddress;

                // just if wanted the post address, we would need to load the p_location table:
                // PLocationAccess.LoadByPrimaryKey(out LocationTable, row.SiteKey, row.LocationKey, Transaction);
            }
        }

        DBAccess.GDBAccessObj.RollbackTransaction();
        return EmailAddress;
    }

    public static List <MailMessage>GenerateEmails(DataSet ATreasurerData, string ASenderEmailAddress)
    {
        List <MailMessage>emails = new List <MailMessage>();

        foreach (DataRow row in ATreasurerData.Tables[TREASURERTABLE].Rows)
        {
            string treasurerEmail = row[ATreasurerData.Tables[TREASURERTABLE].Columns["TreasurerEmail"].Ordinal].ToString();

            if (treasurerEmail.Length == 0)
            {
                // TODO: exclude emails for treasurers without email address
                treasurerEmail = "todo@example.com";
            }

            string treasurerName = row[ATreasurerData.Tables[TREASURERTABLE].Columns["TreasurerName"].Ordinal].ToString();
            Int64 recipientKey = Convert.ToInt64(row[ATreasurerData.Tables[TREASURERTABLE].Columns["RecipientKey"].Ordinal]);

            // TODO: message body from HTML template; recognise detail lines automatically; drop title tag, because it is the subject
            string msg = String.Format(
                "<html><body>Hello {0}, <br/> This is a test. <br/> Donations so far: <br/>",
                treasurerName);
            msg += "<table>";

            DataRow[] rows = ATreasurerData.Tables[GIFTSTABLE].Select("RecipientKey = " + recipientKey.ToString(), "MonthDate");

            foreach (DataRow rowGifts in rows)
            {
                DateTime month = Convert.ToDateTime(rowGifts["MonthDate"]);
                msg += "<tr><td>" + month.ToString("MMMM yyyy") + "</td>";
                msg += "<td>" + String.Format("{0:C}", Convert.ToDouble(rowGifts["MonthAmount"])) + "</td></tr>";
            }

            msg += "</table><br/>All the best, </body></html>";

            // TODO: subject also from HTML template, title tag
            MailMessage mail = new MailMessage(ASenderEmailAddress,
                treasurerEmail,
                "Spendeneingang für " + row["RecipientName"], msg);
            emails.Add(mail);
        }

        return emails;
    }
}
}