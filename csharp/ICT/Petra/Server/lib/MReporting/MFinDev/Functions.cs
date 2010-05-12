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
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MReporting;
using Ict.Common;
using Ict.Petra.Server.MReporting.MFinance;
using System.Data.Odbc;
using System.Data;

namespace Ict.Petra.Server.MReporting.MFinDev
{
    /// <summary>
    /// user functions for Financial Development module
    /// </summary>
    public class TRptUserFunctionsFinDev : TRptUserFunctions
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsFinDev() : base()
        {
        }

        /// <summary>
        /// functions need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            if (StringHelper.IsSame(f, "getGiftStatistics"))
            {
                value = new TVariant();
                GetGiftStatistics(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString(), ops[4].ToInt(), ops[5].ToInt(), ops[6].ToInt());
                return true;
            }

            value = new TVariant();
            return false;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ledgerNumber"></param>
        /// <param name="donorKey"></param>
        /// <param name="recipientKey">if this is empty, all donations are scanned, otherwise the result is limited to gifts to that recipient</param>
        /// <param name="startPeriod">The period where to start; can be a negative number to go back to previous years</param>
        /// <param name="endPeriod">The last period to be considered; should be not the current period, because of unentered gifts</param>
        /// <param name="year"></param>
        /// <returns>The function will put the following variables into the memory:
        /// numberOfGifts, minAmount, maxAmount, averagePerGift, averagePerPeriod, totalAmount, percentageRegular
        /// returns empty string
        /// </returns>
        private void GetGiftStatistics(int ledgerNumber, string donorKey, string recipientKey, int startPeriod, int endPeriod, int year)
        {
            TRptUserFunctionsDate rptDateFunctions;
            DateTime startDate;
            DateTime endDate;
            string strSql;
            DataTable tab;
            int numberOfGifts;
            int numberOfPeriods;
            double giftAmount;
            double minAmount;
            double maxAmount;
            double averagePerGift;
            double averagePerPeriod;
            double totalAmount;
            int percentageRegular;

            rptDateFunctions = new TRptUserFunctionsDate(situation);
            startDate = rptDateFunctions.GetStartDateOfPeriod(ledgerNumber, year, startPeriod);
            endDate = rptDateFunctions.GetEndDateOfPeriod(ledgerNumber, year, endPeriod);
            numberOfPeriods = rptDateFunctions.GetMonthDiff(startDate, endDate);
            rptDateFunctions = null;
            strSql = "SELECT a_gift_amount_n, a_confidential_gift_flag_l";

            if (recipientKey == "ALL")
            {
                strSql = strSql + ", p_recipient_key_n";
            }

            strSql = strSql + " FROM PUB_a_gift, PUB_a_gift_detail " + "WHERE PUB_a_gift.a_ledger_number_i = PUB_a_gift_detail.a_ledger_number_i " +
                     "AND PUB_a_gift.a_batch_number_i = PUB_a_gift_detail.a_batch_number_i " +
                     "AND PUB_a_gift.a_gift_transaction_number_i = PUB_a_gift_detail.a_gift_transaction_number_i " +
                     "AND PUB_a_gift.a_ledger_number_i = ? " +
                     "AND PUB_a_gift.p_donor_key_n = " + donorKey + ' ' + "AND PUB_a_gift.a_date_entered_d BETWEEN ? AND ? ";

            if (recipientKey != "ALL")
            {
                strSql = strSql + "AND PUB_a_gift_detail.p_recipient_key_n = " + recipientKey;
            }

            tab = situation.GetDatabaseConnection().SelectDT(strSql, "table", situation.GetDatabaseConnection().Transaction,
                new OdbcParameter[] {
                    new OdbcParameter("ledgernumber", (object)ledgerNumber),
                    new OdbcParameter("startDate", startDate),
                    new OdbcParameter("endDate", endDate)
                });
            numberOfGifts = 0;
            minAmount = 0;
            maxAmount = 0;
            totalAmount = 0;

            foreach (DataRow row in tab.Rows)
            {
                numberOfGifts++;
                giftAmount = Convert.ToDouble(row["a_gift_amount_n"]);

                if (((minAmount == 0) || (giftAmount < minAmount)) && (giftAmount > 0))
                {
                    minAmount = giftAmount;
                }

                if ((maxAmount == 0) || (giftAmount > maxAmount))
                {
                    maxAmount = giftAmount;
                }

                totalAmount = totalAmount + giftAmount;
            }

            averagePerGift = (double)totalAmount / (double)numberOfGifts;
            averagePerPeriod = (double)totalAmount / (double)numberOfPeriods;
            percentageRegular = 0;

            // has given same amount, one gift for a period => full regular donor
            if ((maxAmount == minAmount) && (numberOfGifts == numberOfPeriods))
            {
                percentageRegular = 100;
            }
            // has given one gift for each period => 80 % regular donor
            else if (numberOfGifts == numberOfPeriods)
            {
                percentageRegular = 80;
            }
            // has given several gifts (at least number of periods divided by 3, starting with 3)
            else if ((numberOfGifts >= numberOfPeriods / 3) && (numberOfGifts > 2))
            {
                // e.g. someone gives sometimes every 2nd month 200, but sometimes for 2 months each 100
                if (minAmount == averagePerPeriod)
                {
                    percentageRegular = 60;
                }
                // has given more than 2 gifts, and the amount was always the same
                else if (maxAmount == minAmount)
                {
                    percentageRegular = 30;
                }
                else if (numberOfGifts > 3)
                {
                    percentageRegular = numberOfGifts;
                }
            }

            situation.GetParameters().Add("numberOfGifts", numberOfGifts, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("minAmount", new TVariant(minAmount, "currency"), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("maxAmount", new TVariant(maxAmount, "currency"), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("totalAmount", new TVariant(totalAmount,
                    "currency"), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("averagePerGift", new TVariant(averagePerGift,
                    "currency"), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("averagePerPeriod", new TVariant(averagePerPeriod,
                    "currency"), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            situation.GetParameters().Add("percentageRegular", percentageRegular, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
        }
    }
}