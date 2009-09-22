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
        // last donation date covers the full month
        DateTime EndDate =
            new DateTime(ALastDonationDate.Year, ALastDonationDate.Month, DateTime.DaysInMonth(ALastDonationDate.Year, ALastDonationDate.Month));

        DateTime StartDate = EndDate.AddMonths(-1 * (ANumberMonths - 1));
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
        AddTreasurerEmailOrPostalAddress(ref TreasurerTable);

        return ResultDataset;
    }

    private static string ReadSqlFile(string ASqlFilename)
    {
        string path = TAppSettingsManager.GetValueStatic("SqlFiles.Path", ".");

        StreamReader reader = new StreamReader(path + Path.DirectorySeparatorChar + ASqlFilename);
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

            if (TreasurerTable.Rows.Count >= 1)
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
    /// get the email address or the postal address of the treasurer and add to the table
    /// </summary>
    /// <param name="ResultTable"></param>
    private static void AddTreasurerEmailOrPostalAddress(ref DataTable ResultTable)
    {
        ResultTable.Columns.Add("TreasurerEmail", typeof(string));
        ResultTable.Columns.Add("TreasurerLocality", typeof(string));
        ResultTable.Columns.Add("TreasurerStreetName", typeof(string));
        ResultTable.Columns.Add("TreasurerBuilding1", typeof(string));
        ResultTable.Columns.Add("TreasurerBuilding2", typeof(string));
        ResultTable.Columns.Add("TreasurerAddress3", typeof(string));
        ResultTable.Columns.Add("TreasurerCountryCode", typeof(string));
        ResultTable.Columns.Add("TreasurerPostalCode", typeof(string));
        ResultTable.Columns.Add("TreasurerCity", typeof(string));

        foreach (DataRow row in ResultTable.Rows)
        {
            if (row[ResultTable.Columns["TreasurerKey"].Ordinal] != DBNull.Value)
            {
                PLocationTable Address;
                string emailAddress = GetBestEmailAddress(Convert.ToInt64(row[ResultTable.Columns["TreasurerKey"].Ordinal]), out Address);

                if (emailAddress.Length > 0)
                {
                    row[ResultTable.Columns["TreasurerEmail"].Ordinal] = emailAddress;
                }
                else
                {
                    if (!Address[0].IsLocalityNull())
                    {
                        row[ResultTable.Columns["TreasurerLocality"].Ordinal] = Address[0].Locality;
                    }

                    if (!Address[0].IsStreetNameNull())
                    {
                        row[ResultTable.Columns["TreasurerStreetName"].Ordinal] = Address[0].StreetName;
                    }

                    if (!Address[0].IsBuilding1Null())
                    {
                        row[ResultTable.Columns["TreasurerBuilding1"].Ordinal] = Address[0].Building1;
                    }

                    if (!Address[0].IsBuilding2Null())
                    {
                        row[ResultTable.Columns["TreasurerBuilding2"].Ordinal] = Address[0].Building2;
                    }

                    if (!Address[0].IsAddress3Null())
                    {
                        row[ResultTable.Columns["TreasurerAddress3"].Ordinal] = Address[0].Address3;
                    }

                    if (!Address[0].IsCountryCodeNull())
                    {
                        row[ResultTable.Columns["TreasurerCountryCode"].Ordinal] = Address[0].CountryCode;
                    }

                    if (!Address[0].IsPostalCodeNull())
                    {
                        row[ResultTable.Columns["TreasurerPostalCode"].Ordinal] = Address[0].PostalCode;
                    }

                    if (!Address[0].IsCityNull())
                    {
                        row[ResultTable.Columns["TreasurerCity"].Ordinal] = Address[0].City;
                    }
                }
            }
        }
    }

    private static string GetBestEmailAddress(Int64 APartnerKey, out PLocationTable AAddress)
    {
        string EmailAddress = "";
        TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

        AAddress = new PLocationTable();

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
                if (!row.IsEmailAddressNull())
                {
                    EmailAddress = row.EmailAddress;
                }

                // we also want the post address, need to load the p_location table:
                AAddress = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, Transaction);
            }
        }

        DBAccess.GDBAccessObj.RollbackTransaction();
        return EmailAddress;
    }

    public static List <MailMessage>GenerateEmails(DataSet ATreasurerData, string ASenderEmailAddress, bool AForceLetters)
    {
        List <MailMessage>emails = new List <MailMessage>();

        foreach (DataRow row in ATreasurerData.Tables[TREASURERTABLE].Rows)
        {
            if (!AForceLetters && (row[ATreasurerData.Tables[TREASURERTABLE].Columns["TreasurerEmail"].Ordinal] != System.DBNull.Value))
            {
                string treasurerEmail = row[ATreasurerData.Tables[TREASURERTABLE].Columns["TreasurerEmail"].Ordinal].ToString();

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
                    msg += "<td align=\"right\">" + String.Format("{0:C}", Convert.ToDouble(rowGifts["MonthAmount"])) + "</td>";
                    msg += "<td>" + String.Format("  {0}", Convert.ToDouble(rowGifts["MonthCount"])) + "</td>";
                    msg += "</tr>";
                }

                msg += "</table><br/>All the best, </body></html>";

                // TODO: subject also from HTML template, title tag
                MailMessage mail = new MailMessage(ASenderEmailAddress,
                    treasurerEmail,
                    "Spendeneingang für " + row["RecipientName"], msg);
                emails.Add(mail);
            }
        }

        return emails;
    }

    /// <summary>
    /// generate the letters to be printed and to be sent to postal addresses
    /// </summary>
    /// <param name="ATreasurerData"></param>
    /// <returns></returns>
    public static List <LetterMessage>GenerateLetters(DataSet ATreasurerData, bool AForceLetters)
    {
        List <LetterMessage>letters = new List <LetterMessage>();

        foreach (DataRow row in ATreasurerData.Tables[TREASURERTABLE].Rows)
        {
            if (AForceLetters || (row[ATreasurerData.Tables[TREASURERTABLE].Columns["TreasurerEmail"].Ordinal] == System.DBNull.Value))
            {
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
                    msg += "<td align=\"right\">" + String.Format("{0:C}", Convert.ToDouble(rowGifts["MonthAmount"])) + "</td>";
                    msg += "<td>" + String.Format("  {0}", Convert.ToDouble(rowGifts["MonthCount"])) + "</td>";
                    msg += "</tr>";
                }

                msg += "</table><br/>All the best, </body></html>";

                LetterMessage letter = new LetterMessage(
                    treasurerName,
                    "Spendeneingang für " + row["RecipientName"], msg);
                letters.Add(letter);
            }
        }

        return letters;
    }
}

public class LetterMessage
{
    public string RecipientShortName;
    public string Subject;
    public string HtmlMessage;
    public LetterMessage(string ARecipientShortName, string ASubject, string AHtmlMessage)
    {
        RecipientShortName = ARecipientShortName;
        Subject = ASubject;
        HtmlMessage = AHtmlMessage;
    }
}
}