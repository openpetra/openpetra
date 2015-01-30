//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2015 by OM International
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
using System.Globalization;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.CrossLedger.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.Common.WebConnectors
{
    ///<summary>
    /// This connector provides methods for working with the Cross-Ledger data
    ///</summary>
    public static class TCrossLedger
    {
        // Two static variables that are used in our code to automatically clean the Daily Exchange Rate table
        private static DateTime PreviousDailyExchangeRateCleanTime = DateTime.UtcNow.AddDays(-30);
        private static DateTime PreviousDailyExchangeRateAccessTime = DateTime.UtcNow.AddHours(-24);

        /// <summary>
        /// This method is called when clients access the Daily Exchange Rate data.
        /// The Daily Exchange Rate table is unusual in that it doesn't really need to hold any data because the DataSet that the client receives
        /// contains all the used rates from the GL/Gift tables whether or not those rates are in the DER table itself.  Any rates in the DER table
        /// that are NOT used are also returned, but, of course, because they are not used anywhere they are not very inetresting!
        /// Additionally, because the GL/Gift tables do not necessarily hold a time or a time that matches the same rate in the DER table, it is possible
        /// for the DER table to have a rate that is used on the date but at a different time.  As a result the client sometimes does not see all rows
        /// from the DER table - and so has no way of deleting them.
        ///
        /// That is the reason why we need to automatically clean the table.
        ///
        /// But there is some value in having some 'unused' rows that are work-in-progress.  So we delete everything in the DER table that
        /// applies to dates older than 30 days.  In the future this might become a configurable server option.
        /// </summary>
        private static void DoDailyExchangeRateClean()
        {
            if ((DateTime.UtcNow - PreviousDailyExchangeRateAccessTime).TotalHours > 8)
            {
                // Nobody has opened a DailyExchangeRate screen for 8 hours
                if ((DateTime.UtcNow - PreviousDailyExchangeRateCleanTime).TotalHours > 24)
                {
                    // It is more than 24 hours since our last clean
                    TDBTransaction t = null;
                    bool bSubmissionOk = false;
                    DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref t, ref bSubmissionOk,
                        delegate
                        {
                            string logMsg = String.Empty;
                            int affectedRowCount = 0;

                            // Standard is that we delete rows applicable to dates more than 60 days old
                            string criticalDate = DateTime.Now.AddDays(-60).ToString("yyyy-MM-dd");

                            try
                            {
                                // Our deletion rule is to delete rows where
                                //  either the effective date is too old and we have no info about creation or modification
                                //  or     the creation date is too old and we have no info about any modification
                                //  or     the modification date is too old
                                // These rules ensure that if rates are added to a DB that is past its last accounting period (like SA-DB)
                                //  we can still continue to use the DER screen to add unused rates because they will have create/modify times
                                //  that can be long past the final accounting period because we will keep
                                //         any row that has been modified recently, whatever the effective date or creation date
                                //         any row that was created recently but not subsequently modified, whatever the effective date
                                //         any row where we don't have info about create/modify but where the effective date is recent
                                string sql = String.Format(
                                    "DELETE FROM PUB_{0} WHERE (({1}<'{2}') and {3} is NULL and {4} is NULL) or (({3}<'{2}') and {4} is NULL) or ({4}<'{2}')",
                                    ADailyExchangeRateTable.GetTableDBName(),
                                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                                    criticalDate,
                                    ADailyExchangeRateTable.GetDateCreatedDBName(),
                                    ADailyExchangeRateTable.GetDateModifiedDBName());
                                affectedRowCount = DBAccess.GDBAccessObj.ExecuteNonQuery(sql, t);
                                bSubmissionOk = true;
                                PreviousDailyExchangeRateCleanTime = DateTime.UtcNow;
                            }
                            catch (Exception ex)
                            {
                                logMsg = "An error occurred while trying to purge the Daily Exchange Rate table of 'aged' rows.";
                                logMsg += String.Format("  The exception message was: {0}", ex.Message);
                            }

                            if ((affectedRowCount > 0) && (logMsg == String.Empty))
                            {
                                logMsg =
                                    String.Format("The Daily Exchange Rate table was purged of {0} entries applicable prior to ",
                                        affectedRowCount) + criticalDate;
                            }

                            if (logMsg != String.Empty)
                            {
                                TLogging.Log(logMsg);
                            }
                        });
                }
            }

            PreviousDailyExchangeRateAccessTime = DateTime.UtcNow;
        }

        /// <summary>
        /// This is the main method to load all the data required by the Daily Exchange rate Setup screen.
        /// It slices and dices the exchange rate data in two ways:
        ///   1. It populates a table with all the rows from the daily exchange rate table itself, plus the data from the Journal and Gift
        ///      tables as well that are not referenced by any of the daily rate table rows.  Additionally this table contains two columns
        ///      that show how many times the specified rate has been used.  This table has one row for every defined rate.  The rate may be used
        ///      in more than one place.  The primary key for this table (like the Daily Exchange Rate table itself) is From/To/Date/Time
        ///   2. It populates another table with rate and date information in the same Daily Exchange Rate table format but this table is extended
        ///      such that the row specifies the ledger/batch/journal details where this rate can be found.  There is one row per place where a rate
        ///      is used, so the same from/to/date/time may occur more than once, each with a different ledger/batch/journal.  Typically this table has
        ///      fewer rows than (1) because it does not contain any unused rows and it does not contain any inverse currency rows.  The primary key
        ///      is From/To/Date/Time/Ledger/Batch/Journal.
        /// The third table is the Corporate Exchange Rate table, which contains standard content
        /// </summary>
        /// <returns>A complete typed data set containing three tables.</returns>
        [RequireModulePermission("FINANCE-1")]
        public static ExchangeRateTDS LoadDailyExchangeRateData(bool ADeleteAgedExchangeRatesFirst)
        {
            // If relevant, we do a clean of the data table first, purging 'aged' data
            if (ADeleteAgedExchangeRatesFirst)
            {
                DoDailyExchangeRateClean();
            }

            ExchangeRateTDS WorkingDS = new ExchangeRateTDS();
            WorkingDS.EnforceConstraints = false;
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    // Populate the ExchangeRateTDSADailyExchangeRate table
                    //-- This is the complete query for the DAILYEXCHANGERATE TABLE
                    //-- It returns all rows from the Journal and Gift Batch tables
                    //-- PLUS all the rows from the DailyExchangeRate table that are NOT referenced by the Journal and Gift Batch tables.
                    string strSQL = "SELECT * FROM ";
                    strSQL += "( ";

                    // This returns all the rows in Daily Exchange rate that do NOT match any journal or gift
                    strSQL += "SELECT ";
                    strSQL += String.Format(
                        "  0 AS {0}, 0 AS {1}, 'DER' AS {2}, ",
                        ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName());
                    strSQL += "  der.* ";
                    strSQL += "FROM PUB_a_daily_exchange_rate AS der ";
                    // By doing a left join and only selecting the NULL rows we get the rows from DER that are NOT used
                    strSQL += "LEFT JOIN ";
                    strSQL += "( ";
                    // This SELECT returns all the used rows (372 rows in the case of SA-DB)
                    strSQL += "SELECT ";
                    strSQL += "  j.a_batch_number_i AS a_batch_number_i, ";
                    strSQL += "  j.a_transaction_currency_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  j.a_date_effective_d AS a_date_effective_from_d, ";
                    strSQL += "  j.a_exchange_rate_to_base_n AS a_rate_of_exchange_n ";
                    strSQL += "FROM PUB_a_journal AS j ";
                    strSQL += "JOIN PUB_a_ledger AS ldg ON ";
                    strSQL += "  ldg.a_ledger_number_i = j.a_ledger_number_i ";
                    strSQL += "WHERE ";
                    strSQL += "  j.a_transaction_currency_c <> ldg.a_base_currency_c ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ALL ";
                    strSQL += Environment.NewLine;

                    strSQL += "SELECT ";
                    strSQL += "  gb.a_batch_number_i AS a_batch_number_i, ";
                    strSQL += "  gb.a_currency_code_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  gb.a_gl_effective_date_d AS a_date_effective_from_d, ";
                    strSQL += "  gb.a_exchange_rate_to_base_n AS a_rate_of_exchange_n ";
                    strSQL += "FROM PUB_a_gift_batch AS gb ";
                    strSQL += "JOIN PUB_a_ledger AS ldg ON ";
                    strSQL += "  ldg.a_ledger_number_i = gb.a_ledger_number_i ";
                    strSQL += "WHERE ";
                    strSQL += "  gb.a_currency_code_c <> ldg.a_base_currency_c ";
                    strSQL += ") AS j_and_gb ";
                    strSQL += "ON ";
                    strSQL += "  der.a_from_currency_code_c = j_and_gb.a_from_currency_code_c ";
                    strSQL += "  AND der.a_to_currency_code_c = j_and_gb.a_to_currency_code_c ";
                    strSQL += "  AND der.a_date_effective_from_d = j_and_gb.a_date_effective_from_d ";
                    strSQL += "  AND der.a_rate_of_exchange_n = j_and_gb.a_rate_of_exchange_n ";
                    strSQL += "WHERE ";
                    strSQL += "  a_batch_number_i IS NULL ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ALL ";
                    strSQL += Environment.NewLine;

                    // The second half of the UNION returns all the Forex rows from journal and gift
                    //  They are aggregated by from/to/date/rate and the time is the min time.
                    //  We also get the usage count as well as whether the row originated in the DER table or one of gift or batch
                    strSQL += "SELECT ";
                    strSQL += String.Format(
                        "  sum(journalUsage) AS {0}, sum(giftBatchUsage) AS {1}, 'GBJ' AS {2}, ",
                        ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName());
                    strSQL += "  a_from_currency_code_c, ";
                    strSQL += "  a_to_currency_code_c, ";
                    strSQL += "  a_rate_of_exchange_n, ";
                    strSQL += "  a_date_effective_from_d, ";
                    strSQL += "  min(a_time_effective_from_i), ";
                    strSQL += "  NULL AS s_date_created_d, ";
                    strSQL += "  NULL AS s_created_by_c, ";
                    strSQL += "  NULL AS s_date_modified_d, ";
                    strSQL += "  NULL AS s_modified_by_c, ";
                    strSQL += "  NULL AS s_modification_id_t ";
                    strSQL += "FROM ";
                    strSQL += "( ";
                    // These are all the used rows again (same as part of the query above) but this time we can count the usages from the two tables
                    strSQL += "SELECT ";
                    strSQL += "  1 AS journalUsage, ";
                    strSQL += "  0 AS giftBatchUsage, ";
                    strSQL += "  j.a_transaction_currency_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  j.a_date_effective_d AS a_date_effective_from_d, ";
                    strSQL += "  j.a_exchange_rate_time_i AS a_time_effective_from_i, ";
                    strSQL += "  j.a_exchange_rate_to_base_n AS a_rate_of_exchange_n ";
                    strSQL += "FROM PUB_a_journal AS j ";
                    strSQL += "JOIN PUB_a_ledger AS ldg ON ";
                    strSQL += "  ldg.a_ledger_number_i = j.a_ledger_number_i ";
                    strSQL += "WHERE ";
                    strSQL += "  j.a_transaction_currency_c <> ldg.a_base_currency_c ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ALL ";
                    strSQL += Environment.NewLine;

                    strSQL += "SELECT ";
                    strSQL += "  0 AS journalUsage, ";
                    strSQL += "  1 AS giftBatchUsage, ";
                    strSQL += "  gb.a_currency_code_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  gb.a_gl_effective_date_d AS a_date_effective_from_d, ";
                    strSQL += "  0 AS a_time_effective_from_i, ";
                    strSQL += "  gb.a_exchange_rate_to_base_n AS a_rate_of_exchange_n ";
                    strSQL += "FROM PUB_a_gift_batch AS gb ";
                    strSQL += "JOIN PUB_a_ledger AS ldg ON ";
                    strSQL += "  ldg.a_ledger_number_i = gb.a_ledger_number_i ";
                    strSQL += "WHERE ";
                    strSQL += "  gb.a_currency_code_c <> ldg.a_base_currency_c ";
                    strSQL += ") AS j_and_gb ";

                    // GROUP the second half of the query (the UNION of used rates)
                    strSQL += "GROUP BY ";
                    strSQL += "  a_from_currency_code_c, ";
                    strSQL += "  a_to_currency_code_c, ";
                    strSQL += "  a_date_effective_from_d, ";
                    strSQL += "  a_rate_of_exchange_n ";
                    strSQL += ") AS all_rates ";

                    // ORDER of the outermost SELECT
                    strSQL += "ORDER BY ";
                    strSQL += "  a_to_currency_code_c, ";
                    strSQL += "  a_from_currency_code_c, ";
                    strSQL += "  a_date_effective_from_d DESC, ";
                    strSQL += "  a_time_effective_from_i DESC ";

                    DBAccess.GDBAccessObj.Select(WorkingDS, strSQL, WorkingDS.ADailyExchangeRate.TableName, Transaction);


                    // Now populate the ExchangeRateTDSADailyExchangerateUsage table
                    //-- COMPLETE QUERY TO RETURN ADailyExchangeRateUsage
                    //-- Query to return the Daily Exchange Rate Usage details
                    //--  Only returns rows that are in a foreign currency
                    //-- Querying this table by from/to/date/time will return one row per use case
                    //-- If the Journal is 0 the batch refers to a gift batch, otherwise it is a GL batch
                    strSQL = "SELECT * FROM ( ";

                    //-- This part of the query returns the use cases from the Journal table
                    strSQL += "SELECT ";
                    strSQL += "  j.a_transaction_currency_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  j.a_exchange_rate_to_base_n AS a_rate_of_exchange_n, ";
                    strSQL += "  j.a_date_effective_d AS a_date_effective_from_d, ";
                    strSQL += "  j.a_exchange_rate_time_i AS a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  j.a_ledger_number_i AS {0}, j.a_batch_number_i AS {1}, j.a_journal_number_i AS {2}, b.a_batch_status_c AS {3}, j.a_journal_description_c AS {4}, b.a_batch_year_i AS {5}, b.a_batch_period_i AS {6}, 'J' AS {7} ",
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchStatusDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetDescriptionDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchYearDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchPeriodDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetTableSourceDBName());
                    strSQL += "FROM a_journal j ";
                    strSQL += "JOIN a_ledger ldg ";
                    strSQL += "  ON ldg.a_ledger_number_i = j.a_ledger_number_i ";
                    strSQL += "JOIN a_batch b ";
                    strSQL += "  ON b.a_batch_number_i = j.a_batch_number_i ";
                    strSQL += "  AND b.a_ledger_number_i = j.a_ledger_number_i ";
                    strSQL += "WHERE j.a_transaction_currency_c <> ldg.a_base_currency_c ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ";
                    strSQL += Environment.NewLine;

                    //-- This part of the query returns the use cases from the Gift Batch table
                    strSQL += "SELECT ";
                    strSQL += "  gb.a_currency_code_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  gb.a_exchange_rate_to_base_n AS a_rate_of_exchange_n, ";
                    strSQL += "  gb.a_gl_effective_date_d AS a_date_effective_from_d, ";
                    strSQL += "  0 AS a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  gb.a_ledger_number_i AS {0}, gb.a_batch_number_i AS {1}, 0 AS {2}, gb.a_batch_status_c AS {3}, gb.a_batch_description_c AS {4}, gb.a_batch_year_i AS {5}, gb.a_batch_period_i AS {6}, 'GB' AS {7} ",
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchStatusDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetDescriptionDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchYearDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchPeriodDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetTableSourceDBName());
                    strSQL += "FROM a_gift_batch gb ";
                    strSQL += "JOIN a_ledger ldg ";
                    strSQL += "  ON ldg.a_ledger_number_i = gb.a_ledger_number_i ";
                    strSQL += "WHERE gb.a_currency_code_c <> ldg.a_base_currency_c ";

                    strSQL += ") AS usage ";
                    strSQL += "ORDER BY usage.a_date_effective_from_d DESC, usage.a_time_effective_from_i DESC ";

                    DBAccess.GDBAccessObj.Select(WorkingDS, strSQL, WorkingDS.ADailyExchangeRateUsage.TableName, Transaction);

                    // Now we start a tricky bit to resolve potential primary key conflicts when the constraints are turned on.
                    // By combining the Journal and Gift Batch data that is not referenced in the exchange rate table we can easily
                    //  have introduced conflicts where more than one rate has been used for a given currency pair and effective date/time.
                    // This is because there is no constraint that enforces the batch/journal tables to use a time from the exch rate table.
                    // So we have to go through all the rows in our data table and potentially change the time to make it possible to get our primary key.

                    // Start by creating a data view on the whole result set.  The ordering is important because we are going to step through the set row by row.
                    // Within one group of from/to/date it is essential that the first 'source' is the DER table because we don't change the time on that one -
                    //   and of course that must stay the same because the user can modify that one.
                    // We need to deal with the following possibilities:
                    //   From  To   Date         Time  Source   Rate
                    //   EUR   GBP  2014-01-01   1234   DER     2.11
                    //   EUR   GBP  2014-01-01   1234   GBJ     2.115
                    //   EUR   GBP  2014-01-01   1234   GBJ     2.22
                    //   EUR   GBP  2014-01-01   1234   GBJ     3.11
                    //
                    // In the first row we have an entry from the DER table that is not used anywhere, but a (slightly) different rate is actually used
                    //   in a Journal.
                    // In the other rows we have 3 different rates - all used somewhere.  We need to adjust the times so they are different.

                    DataView dv = new DataView(WorkingDS.ADailyExchangeRate, "",
                        String.Format("{0}, {1}, {2} DESC, {3} DESC, {4}, {5}",
                            ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                            ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                            ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                            ADailyExchangeRateTable.GetTimeEffectiveFromDBName(),
                            ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName(),
                            ADailyExchangeRateTable.GetRateOfExchangeDBName()), DataViewRowState.CurrentRows);

                    for (int i = 0; i < dv.Count - 1; i++)
                    {
                        // Get the 'current' row and the 'next' one...
                        ExchangeRateTDSADailyExchangeRateRow drThis = (ExchangeRateTDSADailyExchangeRateRow)dv[i].Row;
                        ExchangeRateTDSADailyExchangeRateRow drNext = (ExchangeRateTDSADailyExchangeRateRow)dv[i + 1].Row;

                        if (!drThis.FromCurrencyCode.Equals(drNext.FromCurrencyCode)
                            || !drThis.ToCurrencyCode.Equals(drNext.ToCurrencyCode)
                            || !drThis.DateEffectiveFrom.Equals(drNext.DateEffectiveFrom)
                            || !drThis.TimeEffectiveFrom.Equals(drNext.TimeEffectiveFrom))
                        {
                            // Something is different so our primary key will be ok for the current row
                            continue;
                        }

                        // We have got two (or more) rows with the same potential primary key and different rates/usages.
                        // We need to work out how many rows ahead also have the same time and adjust them all
                        bool moveForwards = (drThis.TimeEffectiveFrom < 43200);
                        int timeOffset = 60;        // 1 minute

                        // Start by adjusting our 'next' row we are already working with
                        drNext.BeginEdit();
                        int prevTimeEffectiveFrom = drNext.TimeEffectiveFrom;
                        drNext.TimeEffectiveFrom = (moveForwards) ? prevTimeEffectiveFrom + timeOffset : prevTimeEffectiveFrom - timeOffset;
                        timeOffset = (moveForwards) ? timeOffset + 60 : timeOffset - 60;
                        drNext.EndEdit();
                        i++;            // we can increment our main loop counter now that we have dealt with our 'next' row.
                        TLogging.LogAtLevel(2, String.Format("Modifying {0} row: From {1}, To {2}, Date {3}, Time {4}, new Time {5}",
                                drThis.TableSource, drThis.FromCurrencyCode, drThis.ToCurrencyCode, drThis.DateEffectiveFrom.ToString("yyyy-MM-dd"),
                                prevTimeEffectiveFrom, drNext.TimeEffectiveFrom), TLoggingType.ToLogfile);

                        // Modify all the rows in the usage table that refer to the previous time
                        OnModifyEffectiveTime(WorkingDS.ADailyExchangeRateUsage, drNext.FromCurrencyCode, drNext.ToCurrencyCode,
                            drNext.DateEffectiveFrom,
                            prevTimeEffectiveFrom, drNext.TimeEffectiveFrom, drNext.RateOfExchange);

                        // Now look ahead even further than the 'next' row and modify those times too, adding 1 more minute to each
                        for (int k = i + 1;; k++)
                        {
                            ExchangeRateTDSADailyExchangeRateRow drLookAhead = (ExchangeRateTDSADailyExchangeRateRow)dv[k].Row;

                            if (!drThis.FromCurrencyCode.Equals(drLookAhead.FromCurrencyCode)
                                || !drThis.ToCurrencyCode.Equals(drLookAhead.ToCurrencyCode)
                                || !drThis.DateEffectiveFrom.Equals(drLookAhead.DateEffectiveFrom)
                                || !drThis.TimeEffectiveFrom.Equals(drLookAhead.TimeEffectiveFrom))
                            {
                                // No more rows match our potential primary key conflict on the 'current' row.
                                break;
                            }

                            // Do exactly the same to this row as we did to the 'next' row above
                            drLookAhead.BeginEdit();
                            prevTimeEffectiveFrom = drLookAhead.TimeEffectiveFrom;
                            drLookAhead.TimeEffectiveFrom = (moveForwards) ? prevTimeEffectiveFrom + timeOffset : prevTimeEffectiveFrom - timeOffset;
                            timeOffset = (moveForwards) ? timeOffset + 60 : timeOffset - 60;
                            drLookAhead.EndEdit();
                            i++;
                            TLogging.LogAtLevel(2, String.Format("Modifying additional {0} row: From {1}, To {2}, Date {3}, Time {4}, new Time {5}",
                                    drThis.TableSource, drThis.FromCurrencyCode, drThis.ToCurrencyCode,
                                    drThis.DateEffectiveFrom.ToString("yyyy-MM-dd"),
                                    prevTimeEffectiveFrom, drLookAhead.TimeEffectiveFrom), TLoggingType.ToLogfile);

                            OnModifyEffectiveTime(WorkingDS.ADailyExchangeRateUsage, drLookAhead.FromCurrencyCode, drLookAhead.ToCurrencyCode,
                                drLookAhead.DateEffectiveFrom, prevTimeEffectiveFrom, drLookAhead.TimeEffectiveFrom, drLookAhead.RateOfExchange);
                        }
                    }       // check the next row in the table so that it becomes the 'current' row.

                    WorkingDS.EnforceConstraints = true;

                    // Load the Corporate exchange rate table using the usual method
                    ACorporateExchangeRateAccess.LoadAll(WorkingDS, Transaction);
                });

            // Accept row changes here so that the Client gets 'unmodified' rows
            WorkingDS.AcceptChanges();

            return WorkingDS;
        }

        private static void OnModifyEffectiveTime(ExchangeRateTDSADailyExchangeRateUsageTable AUsageTable,
            String AFromCurrencyCode,
            String AToCurrencyCode,
            DateTime AEffectiveDate,
            int AEffectiveTime,
            int ANewEffectiveTime,
            decimal ARateOfExchange)
        {
            DataView dv = new DataView(AUsageTable, String.Format("{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND {6}={7} AND {8}={9}",
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    AFromCurrencyCode,
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    AToCurrencyCode,
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    AEffectiveDate.ToString("d", CultureInfo.InvariantCulture),
                    ADailyExchangeRateTable.GetTimeEffectiveFromDBName(),
                    AEffectiveTime,
                    ADailyExchangeRateTable.GetRateOfExchangeDBName(),
                    ARateOfExchange.ToString(CultureInfo.InvariantCulture)), "", DataViewRowState.CurrentRows);

            foreach (DataRowView drv in dv)
            {
                ExchangeRateTDSADailyExchangeRateUsageRow row = (ExchangeRateTDSADailyExchangeRateUsageRow)drv.Row;
                row.BeginEdit();
                row.TimeEffectiveFrom = ANewEffectiveTime;
                row.EndEdit();
            }
        }
    }
}