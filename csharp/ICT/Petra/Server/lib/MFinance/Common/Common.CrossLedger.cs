//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
        public static ExchangeRateTDS LoadDailyExchangeRateData()
        {
            ExchangeRateTDS WorkingDS = new ExchangeRateTDS();

            WorkingDS.EnforceConstraints = false;
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    // Load the table so we can read bits of it into our dataset
                    ADailyExchangeRateTable exchangeRates = ADailyExchangeRateAccess.LoadAll(Transaction);

                    // Populate the ExchangeRateTDSADailyExchangeRate table
                    //-- This is the complete query for the DAILYEXCHANGERATE TABLE
                    //-- It returns all the rows from the DailyExchangeRate table
                    //-- PLUS all rows from the Journal and Gift Batch tables that are NOT referenced by the Daily Exchange Rate table.
                    string strSQL = "SELECT * FROM ( ";

                    // -- This part of the query returns all the rows from the ExchangeRate table
                    // -- All rows in Journal and Gift that DO match an entry are reported in the usage count
                    // -- It will always return exactly the same number of rows that are in the Daily Exchange Rate table itself.
                    // --  In the development database case it returns 312 rows.
                    // --  It includes 86 Journal entries and 1 Gift Batch entry
                    strSQL += "SELECT der.a_from_currency_code_c, ";
                    strSQL += "  der.a_to_currency_code_c, ";
                    strSQL += "  der.a_rate_of_exchange_n, ";
                    strSQL += "  der.a_date_effective_from_d, ";
                    strSQL += "  der.a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  count(j.a_journal_number_i) AS {0}, count(gb.a_batch_number_i) AS {1}, 'DEX' as {2} ",
                        ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName());
                    strSQL += "FROM PUB_a_daily_exchange_rate AS der ";
                    strSQL += "LEFT OUTER JOIN PUB_a_journal j ";
                    strSQL += "  ON j.a_transaction_currency_c = der.a_from_currency_code_c ";
                    strSQL += "  AND j.a_base_currency_c = der.a_to_currency_code_c ";
                    strSQL += "  AND j.a_exchange_rate_to_base_n = der.a_rate_of_exchange_n ";
                    strSQL += "  AND j.a_date_effective_d = der.a_date_effective_from_d ";
                    strSQL += "  AND a_exchange_rate_time_i = der.a_time_effective_from_i ";
                    strSQL += "LEFT OUTER JOIN PUB_a_gift_batch AS gb ";
                    strSQL += "  ON gb.a_currency_code_c = der.a_from_currency_code_c ";
                    strSQL += "  AND gb.a_exchange_rate_to_base_n = der.a_rate_of_exchange_n ";
                    strSQL += "  AND gb.a_gl_effective_date_d = der.a_date_effective_from_d ";
                    strSQL += "LEFT OUTER JOIN PUB_a_ledger AS ldg ";
                    strSQL += "  ON ldg.a_ledger_number_i = gb.a_ledger_number_i ";
                    strSQL += "  AND ldg.a_base_currency_c = der.a_to_currency_code_c ";
                    strSQL +=
                        "GROUP BY der.a_from_currency_code_c, der.a_to_currency_code_c, der.a_rate_of_exchange_n, der.a_date_effective_from_d, der.a_time_effective_from_i ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ";
                    strSQL += Environment.NewLine;

                    // -- This part of the query returns all the rows from the journal table that do NOT have an
                    // -- entry in the exchange rate table
                    // -- Using the devlopment database it returns 41 unique rows associated with 53 Journal entries
                    strSQL += "SELECT ";
                    strSQL += "  j.a_transaction_currency_c AS a_from_currency_code_c, ";
                    strSQL += "  j.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  j.a_exchange_rate_to_base_n AS a_rate_of_exchange_n, ";
                    strSQL += "  j.a_date_effective_d AS a_date_effective_from_d, ";
                    strSQL += "  j.a_exchange_rate_time_i AS a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  count(j.a_transaction_currency_c) AS {0},  0 AS {1},  'J' AS {2} ",
                        ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName());
                    strSQL += "FROM a_journal j ";
                    strSQL += "LEFT JOIN a_daily_exchange_rate der ";
                    strSQL += "  ON der.a_from_currency_code_c = j.a_transaction_currency_c ";
                    strSQL += "  AND der.a_to_currency_code_c = j.a_base_currency_c ";
                    strSQL += "  AND der.a_date_effective_from_d = j.a_date_effective_d ";
                    strSQL += "  AND der.a_time_effective_from_i = j.a_exchange_rate_time_i ";
                    strSQL += "  AND der.a_rate_of_exchange_n = j.a_exchange_rate_to_base_n ";
                    strSQL += "WHERE j.a_transaction_currency_c <> j.a_base_currency_c ";
                    strSQL += "  AND der.a_from_currency_code_c IS NULL ";
                    strSQL +=
                        "GROUP BY j.a_transaction_currency_c, j.a_base_currency_c, j.a_exchange_rate_to_base_n, j.a_date_effective_d, j.a_exchange_rate_time_i ";

                    strSQL += Environment.NewLine;
                    strSQL += "UNION ";
                    strSQL += Environment.NewLine;

                    // -- This part of the query returns all the rows in the gift batch table that do NOT have an
                    // -- entry in the exchange rate table
                    // -- Using the devlopment database it returns 0 rows, because the one row in the gift batch table has already been included
                    // --   in the first query (on the Daily Exchange rate table)
                    strSQL += "SELECT ";
                    strSQL += "  gb.a_currency_code_c AS a_from_currency_code_c, ";
                    strSQL += "  ldg.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  gb.a_exchange_rate_to_base_n AS a_rate_of_exchange_n, ";
                    strSQL += "  gb.a_gl_effective_date_d AS a_date_effective_from_d, ";
                    strSQL += "  0 AS a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  0 AS {0}, count(gb.a_currency_code_c) AS {1}, 'GB' AS {2} ",
                        ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName(),
                        ExchangeRateTDSADailyExchangeRateTable.GetTableSourceDBName());
                    strSQL += "FROM a_gift_batch gb ";
                    strSQL += "LEFT JOIN a_ledger ldg ";
                    strSQL += "  ON ldg.a_ledger_number_i = gb.a_ledger_number_i ";
                    strSQL += "LEFT JOIN a_daily_exchange_rate der ";
                    strSQL += "  ON der.a_from_currency_code_c = gb.a_currency_code_c ";
                    strSQL += "  AND der.a_to_currency_code_c = ldg.a_base_currency_c ";
                    strSQL += "  AND der.a_date_effective_from_d = gb.a_gl_effective_date_d ";
                    strSQL += "  AND der.a_rate_of_exchange_n = gb.a_exchange_rate_to_base_n ";
                    strSQL += "WHERE gb.a_currency_code_c <> ldg.a_base_currency_c ";
                    strSQL += "  AND der.a_from_currency_code_c IS NULL ";
                    strSQL += "GROUP BY gb.a_currency_code_c, ldg.a_base_currency_c, gb.a_exchange_rate_to_base_n, gb.a_gl_effective_date_d ";

                    strSQL += ") AS allrates ";
                    strSQL += "ORDER BY allrates.a_date_effective_from_d DESC, allrates.a_time_effective_from_i DESC ";

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
                    strSQL += "  j.a_base_currency_c AS a_to_currency_code_c, ";
                    strSQL += "  j.a_exchange_rate_to_base_n AS a_rate_of_exchange_n, ";
                    strSQL += "  j.a_date_effective_d AS a_date_effective_from_d, ";
                    strSQL += "  j.a_exchange_rate_time_i AS a_time_effective_from_i, ";
                    strSQL += String.Format(
                        "  j.a_ledger_number_i AS {0}, j.a_batch_number_i AS {1}, j.a_journal_number_i AS {2}, b.a_batch_status_c AS {3}, b.a_batch_year_i AS {4}, b.a_batch_period_i AS {5}, 'J' AS {6} ",
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchStatusDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchYearDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchPeriodDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetTableSourceDBName());
                    strSQL += "FROM a_journal j ";
                    strSQL += "JOIN a_batch b ";
                    strSQL += "  ON b.a_batch_number_i = j.a_batch_number_i ";
                    strSQL += "  AND b.a_ledger_number_i = j.a_ledger_number_i ";
                    strSQL += "WHERE j.a_transaction_currency_c <> j.a_base_currency_c ";

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
                        "  gb.a_ledger_number_i AS {0}, gb.a_batch_number_i AS {1}, 0 AS {2}, gb.a_batch_status_c AS {3}, gb.a_batch_year_i AS {4}, gb.a_batch_period_i AS {5}, 'GB' AS {6} ",
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName(),
                        ExchangeRateTDSADailyExchangeRateUsageTable.GetBatchStatusDBName(),
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

                    // Start by creating a data view on the whole result set.  The oredering is important because we are going to step through the set row by row.
                    // We order on: From, To, Date, Time, JournalUsage, GiftBatchUsage
                    // We need to deal with the following possibilities:
                    //   From  To   Date         Time   Rate  Journals  Gifts  TableSource
                    //   EUR   GBP  2014-01-01   1234   2.115  0        0      DEX
                    //   EUR   GBP  2014-01-01   1234   2.11   3        0      J
                    //   EUR   GBP  2014-01-01   1234   2.22   1        0      J
                    //   EUR   GBP  2014-01-01   1234   3.11   0        1      GB
                    //
                    // In the first row we have an entry from the DEX table that is not used anywhere, but a (slightly) different rate is actually used
                    //   in a Journal.  So we actually don't show the DEX row.
                    // In the other rows we have 3 different rates - all used somewhere.  We need to adjust the times so they are different.

                    DataView dv = new DataView(WorkingDS.ADailyExchangeRate, "",
                        String.Format("{0}, {1}, {2} DESC, {3} DESC, {4}, {5}",
                            ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                            ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                            ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                            ADailyExchangeRateTable.GetTimeEffectiveFromDBName(),
                            ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                            ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName()), DataViewRowState.CurrentRows);

                    for (int i = 0; i < dv.Count - 1; i++)
                    {
                        // Get the 'current' row and the 'next' one...
                        ExchangeRateTDSADailyExchangeRateRow drThis = (ExchangeRateTDSADailyExchangeRateRow)dv[i].Row;
                        ExchangeRateTDSADailyExchangeRateRow drNext = (ExchangeRateTDSADailyExchangeRateRow)dv[i + 1].Row;

                        if ((drThis.JournalUsage == 0) && (drThis.GiftBatchUsage == 0))
                        {
                            // This will be a row that the client can edit/delete, so we need to add the modification info
                            ADailyExchangeRateRow foundRow = (ADailyExchangeRateRow)exchangeRates.Rows.Find(new object[] {
                                    drThis.FromCurrencyCode, drThis.ToCurrencyCode, drThis.DateEffectiveFrom, drThis.TimeEffectiveFrom
                                });

                            if (foundRow != null)
                            {
                                // it should always be non-null
                                drThis.BeginEdit();
                                drThis.ModificationId = foundRow.ModificationId;
                                drThis.DateModified = foundRow.DateModified;
                                drThis.ModifiedBy = foundRow.ModifiedBy;
                                drThis.DateCreated = foundRow.DateCreated;
                                drThis.CreatedBy = foundRow.CreatedBy;
                                drThis.EndEdit();
                            }
                        }

                        if (!drThis.FromCurrencyCode.Equals(drNext.FromCurrencyCode)
                            || !drThis.ToCurrencyCode.Equals(drNext.ToCurrencyCode)
                            || !drThis.DateEffectiveFrom.Equals(drNext.DateEffectiveFrom)
                            || !drThis.TimeEffectiveFrom.Equals(drNext.TimeEffectiveFrom))
                        {
                            // Something is different so our primary key will be ok for the current row
                            continue;
                        }

                        // We have got two (or more) rows with the same potential primary key and different rates.
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

                        // Modify all the rows in the usage table that refer to the previous time
                        OnModifyEffectiveTime(WorkingDS.ADailyExchangeRateUsage, drNext.FromCurrencyCode, drNext.ToCurrencyCode,
                            drNext.DateEffectiveFrom,
                            prevTimeEffectiveFrom, drNext.TimeEffectiveFrom, drNext.RateOfExchange);

                        // Now look ahead even further than the 'next' row and modify those times too, adding 1 more minute to each
                        for (int k = i + 2;; k++)
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
                    ARateOfExchange), "", DataViewRowState.CurrentRows);

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