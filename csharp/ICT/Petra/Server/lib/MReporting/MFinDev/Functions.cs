//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data.Odbc;
using System.Data;
using System.Text;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB; // Implicit reference
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Server.MReporting.MFinance;
using Ict.Petra.Server.MReporting.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MFinance.Gift.Data;

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

            if (StringHelper.IsSame(f, "IsLapsedDonor"))
            {
                value =
                    new TVariant(IsLapsedDonor(ops[1].ToInt64(), ops[2].ToInt64(), ops[3].ToDate(), ops[4].ToDate(), ops[5].ToString(), ops[6].ToInt(),
                            ops[7].ToInt(), ops[8].ToString(), ops[9].ToString(), ops[10].ToBool()));
                return true;
            }

            if (StringHelper.IsSame(f, "SelectLastGift"))
            {
                value = new TVariant(SelectLastGift(ops[1].ToInt64(), ops[2].ToInt64(), ops[3].ToDate(), ops[4].ToDate(), ops[5].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "IsTopDonor"))
            {
                value = new TVariant(IsTopDonor(ops[1].ToDecimal(), ops[2].ToDecimal(), ops[3].ToDecimal()));
                return true;
            }

            if (StringHelper.IsSame(f, "MakeTopDonor"))
            {
                value = new TVariant(MakeTopDonor(ops[1].ToDecimal(), ops[2].ToDecimal(), ops[3].ToDecimal(),
                        ops[4].ToBool(), ops[5].ToString(), ops[6].ToDate(), ops[7].ToDate(),
                        ops[8].ToInt64(), ops[9].ToString(), ops[10].ToString()));
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
            decimal giftAmount;
            decimal minAmount;
            decimal maxAmount;
            decimal averagePerGift;
            decimal averagePerPeriod;
            decimal totalAmount;
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
                giftAmount = Convert.ToDecimal(row["a_gift_amount_n"]);

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

            averagePerGift = (decimal)totalAmount / (decimal)numberOfGifts;
            averagePerPeriod = (decimal)totalAmount / (decimal)numberOfPeriods;
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

        /// <summary>
        /// Checks if the donor is a lapsed donor according to the parameters supplied
        /// </summary>
        /// <param name="ADonorKey">Partner Key of the donor</param>
        /// <param name="ARecipientKey">Partner Key of the gift recipient</param>
        /// <param name="AStartDate">Date of the first gift</param>
        /// <param name="AEndDate">Date until the gift must occure regularly</param>
        /// <param name="AFrequency">How often the gift must come</param>
        /// <param name="ATolerance">How much tolerance (in days) the gift can vary</param>
        /// <param name="ALedgerNumber">The ledger number</param>
        /// <param name="AMotivationGroup">A matching string for the gift motivation group.</param>
        /// <param name="AMotivationDetail">A matching string for the gift motivation detail</param>
        /// <param name="AIgnoreBetween">True: If this donor gave a gift in between the frequency pattern, then return false</param>
        /// <returns>True if donor is still active</returns>
        private bool IsLapsedDonor(Int64 ADonorKey, Int64 ARecipientKey, DateTime AStartDate, DateTime AEndDate, String AFrequency,
            int ATolerance, int ALedgerNumber,
            String AMotivationGroup, String AMotivationDetail, bool AIgnoreBetween)
        {
            bool ReturnValue = false;
            bool FirstTime = true;

            DateTime StartDate = AEndDate.AddDays(-ATolerance);
            DateTime EndDate = AEndDate.AddDays(ATolerance);

            int YearFrequency;
            int MonthFrequency;
            int DayFrequency;

            GetTimeFrequency(AFrequency, out YearFrequency, out MonthFrequency, out DayFrequency);

            DataTable Table;

            String StrSql = "SELECT " + AGiftTable.GetDateEnteredDBName() +
                            " FROM " + AGiftTable.GetTableDBName() +
                            " , " + AGiftDetailTable.GetTableDBName() +
                            " WHERE " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                            " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetLedgerNumberDBName() + " = " +
                            ALedgerNumber.ToString() +
                            " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDonorKeyDBName() + " = " + ADonorKey.ToString() +
                            " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetBatchNumberDBName() + " = " +
                            AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetBatchNumberDBName() +
                            " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetGiftTransactionNumberDBName() + " = " +
                            AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetGiftTransactionNumberDBName();

            if (ARecipientKey != 0)
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetRecipientKeyDBName() + " = " +
                         ARecipientKey.ToString();
            }

            if (AMotivationDetail != "%")
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationDetailCodeDBName() + " LIKE '" +
                         AMotivationDetail + "'";
            }

            if (AMotivationGroup != "%")
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationGroupCodeDBName() + " LIKE '" +
                         AMotivationGroup + "'";
            }

            StrSql = StrSql +
                     " ORDER BY " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName() + " DESC ";

            Table = situation.GetDatabaseConnection().SelectDT(StrSql, "table", situation.GetDatabaseConnection().Transaction, new OdbcParameter[] { });

            foreach (DataRow Row in Table.Rows)
            {
                DateTime DateEntered = (DateTime)Row[AGiftTable.GetDateEnteredDBName()];

                // Ok they gave a gift during this period but check that it was the last gift.
                if (FirstTime)
                {
                    if (DateEntered > EndDate)
                    {
                        break;
                    }

                    FirstTime = false;
                }

                // If the date is not within the date range then
                // check the flag to see if gifts are allowed between gifts.
                // otherwise ignore.
                if (DateEntered > EndDate)
                {
                    if (AIgnoreBetween)
                    {
                        break;
                    }

                    continue;
                }

                // If we are passed the start date and have not found a gift then this
                // donor does not have the right pattern of giving */
                if (DateEntered < StartDate)
                {
                    break;
                }

                // we have found a gift and it is within the dates we are interested in.
                // now go back to the next set of dates.
                StartDate = StartDate.AddDays(-DayFrequency);
                StartDate = StartDate.AddMonths(-MonthFrequency);
                StartDate = StartDate.AddYears(-YearFrequency);
                EndDate = EndDate.AddDays(-DayFrequency);
                EndDate = EndDate.AddMonths(-MonthFrequency);
                EndDate = EndDate.AddYears(-YearFrequency);

                // we are now beyond the first gift date so stop.
                if (EndDate < AStartDate)
                {
                    ReturnValue = true;
                    break;
                }
            }

            if (!ReturnValue)
            {
                // clear this row, we don't want to display it
                // set all parameters of this row to NULL
                situation.GetParameters().Add("DONTDISPLAYROW", new TVariant(true));
            }
            else
            {
                // show this row
                situation.GetParameters().Add("DONTDISPLAYROW", new TVariant(false), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);

                String Currency = situation.GetParameters().Get("param_currency").ToString();
                bool BaseCurrency = false;

                if (Currency == "Base")
                {
                    BaseCurrency = true;
                }

                CalculatePrviousYearsGift(AEndDate, ADonorKey, ALedgerNumber, ARecipientKey, AMotivationDetail, AMotivationGroup, BaseCurrency);
            }

            return ReturnValue;
        }

        void GetTimeFrequency(String AFrequency, out int AYears, out int AMonths, out int ADays)
        {
            AYears = 0;
            AMonths = 0;
            ADays = 0;

            if (AFrequency == "Annual - Every Year")
            {
                AYears = 1;
            }
            else if (AFrequency == "Semi-Annualy - Every Six Months")
            {
                AMonths = 6;
            }
            else if (AFrequency == "Quarterly - Every Three Months")
            {
                AMonths = 3;
            }
            else if (AFrequency == "Bi-Monthly - Every Two Months")
            {
                AMonths = 2;
            }
            else if (AFrequency == "Monthly - Every Month")
            {
                AMonths = 1;
            }
            else if (AFrequency == " Weekly - Every Week")
            {
                ADays = 7;
            }
            else if (AFrequency == "Daily - Every Day")
            {
                ADays = 7;
            }
        }

        /// <summary>
        /// Calculate the gift amount for this year and the previous two years given from this donor to this recipient.
        /// </summary>
        /// <param name="ALastGiftDate">defines the last year of the calculation</param>
        /// <param name="ADonorKey">Partner Key of the donor</param>
        /// <param name="ALedgerNumber">The ledger number</param>
        /// <param name="ARecipientKey">Partner Key of the gift recipient</param>
        /// <param name="AMotivationDetail">A matching string for the gift motivation detail</param>
        /// <param name="AMotivationGroup">A matching string for the gift motivation group.</param>
        /// <param name="ABaseCurrency">Defines if we sum up the base currency or international currency</param>
        private void CalculatePrviousYearsGift(DateTime ALastGiftDate,
            Int64 ADonorKey,
            int ALedgerNumber,
            Int64 ARecipientKey,
            String AMotivationDetail,
            String AMotivationGroup,
            bool ABaseCurrency)
        {
            DateTime SelectionEndDate = new DateTime(ALastGiftDate.Year, 12, 31);
            DateTime SelectionStartDate = new DateTime(ALastGiftDate.Year - 2, 1, 1);

            String StrSql = "SELECT " + AGiftTable.GetDateEnteredDBName();

            if (ABaseCurrency)
            {
                StrSql = StrSql + ", " + AGiftDetailTable.GetGiftAmountDBName() + " AS CurrentAmount";
            }
            else
            {
                StrSql = StrSql + ", " + AGiftDetailTable.GetGiftAmountIntlDBName() + " AS CurrentAmount";
            }

            StrSql = StrSql +
                     " FROM " + AGiftTable.GetTableDBName() +
                     " , " + AGiftDetailTable.GetTableDBName() +
                     " WHERE " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                     " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetLedgerNumberDBName() + " = " +
                     ALedgerNumber.ToString() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDonorKeyDBName() + " = " + ADonorKey.ToString() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetBatchNumberDBName() + " = " + AGiftDetailTable.GetTableDBName() +
                     "." + AGiftDetailTable.GetBatchNumberDBName() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetGiftTransactionNumberDBName() + " = " +
                     AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetGiftTransactionNumberDBName() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName() + " BETWEEN '" + SelectionStartDate.ToString(
                "yyyy-MM-dd") + "' AND '" + SelectionEndDate.ToString("yyyy-MM-dd") + "'";

            if (ARecipientKey != 0)
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetRecipientKeyDBName() + " = " +
                         ARecipientKey.ToString();
            }

            if (AMotivationDetail != "%")
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationDetailCodeDBName() + " LIKE '" +
                         AMotivationDetail + "'";
            }

            if (AMotivationGroup != "%")
            {
                StrSql = StrSql +
                         " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationGroupCodeDBName() + " LIKE '" +
                         AMotivationGroup + "'";
            }

            DataTable Table = situation.GetDatabaseConnection().SelectDT(StrSql, "table",
                situation.GetDatabaseConnection().Transaction, new OdbcParameter[] { });

            decimal TotalYear_0 = 0.0M;
            decimal TotalYear_1 = 0.0M;
            decimal TotalYear_2 = 0.0M;

            foreach (DataRow Row in Table.Rows)
            {
                DateTime DateEntered = (DateTime)Row[AGiftTable.GetDateEnteredDBName()];
                decimal CurrentAmount = Convert.ToDecimal(Row["CurrentAmount"]);

                if (DateEntered.Year == ALastGiftDate.Year)
                {
                    TotalYear_0 += CurrentAmount;
                }
                else if (DateEntered.Year == ALastGiftDate.Year - 1)
                {
                    TotalYear_1 += CurrentAmount;
                }
                else if (DateEntered.Year == ALastGiftDate.Year - 2)
                {
                    TotalYear_2 += CurrentAmount;
                }
            }

            situation.GetParameters().Add("TotalYear_0", new TVariant(TotalYear_0));
            situation.GetParameters().Add("TotalYear_1", new TVariant(TotalYear_1));
            situation.GetParameters().Add("TotalYear_2", new TVariant(TotalYear_2));
        }

        /// <summary>
        /// Find the latest gift that was given by a donor
        /// </summary>
        /// <param name="AParameters">report parameter list</param>
        /// <param name="AResults">result list</param>
        /// <returns>DataTable with row for last gift by donor</returns>
        public static DataTable SelectLatestGiftRow(TParameterList AParameters, TResultList AResults)
        {
            Int64 DonorKey = AParameters.Get("PartnerKey").ToInt64();

            String StrSql = "SELECT DISTINCT " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetLedgerNumberDBName() + ", " +
                AGiftTable.GetTableDBName() + "." + AGiftTable.GetBatchNumberDBName() + ", " +
                AGiftTable.GetTableDBName() + "." + AGiftTable.GetGiftTransactionNumberDBName() + ", " +
                AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName();

            StrSql = StrSql +
                     " FROM " + AGiftTable.GetTableDBName() +
                     " WHERE " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDonorKeyDBName() + " = " + DonorKey.ToString() +
                     " ORDER BY " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName() + " DESC";

            TDBTransaction Transaction = null;
            DataTable tempTbl = new DataTable();
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    tempTbl = DBAccess.GDBAccessObj.SelectDT(StrSql, "result", Transaction);
                });

            DataTable resultTbl = tempTbl.Clone();
            resultTbl.Clear();

            if (tempTbl.Rows.Count > 0)
            {
                resultTbl.Rows.Add((object[])tempTbl.Rows[0].ItemArray.Clone());
            }

            return resultTbl;

        }

        /// <summary>
        /// Select the last Gift and motivation details of the gifts that were given within the time period from one partner.
        /// </summary>
        /// <param name="ADonorKey">Partner key of the donor</param>
        /// <param name="ALedgerNumber">Ledger number</param>
        /// <param name="AStartDate">Start date of the period</param>
        /// <param name="AEndDate">End date of the period</param>
        /// <param name="ACurrency">Currency: Base or International</param>
        /// <returns>True if a gift was found; otherwise false</returns>
        private bool SelectLastGift(Int64 ADonorKey, Int64 ALedgerNumber, DateTime AStartDate, DateTime AEndDate, String ACurrency)
        {
            String StrSql = "SELECT " + AGiftTable.GetDateEnteredDBName() + ", " +
                            AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationGroupCodeDBName() + ", " +
                            AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetMotivationDetailCodeDBName();

            if (ACurrency == "Base")
            {
                StrSql = StrSql + ", " + AGiftDetailTable.GetGiftAmountDBName() + " AS CurrentAmount";
            }
            else
            {
                StrSql = StrSql + ", " + AGiftDetailTable.GetGiftAmountIntlDBName() + " AS CurrentAmount";
            }

            StrSql = StrSql +
                     " FROM " + AGiftTable.GetTableDBName() +
                     " , " + AGiftDetailTable.GetTableDBName() +
                     ", " + AGiftBatchTable.GetTableDBName() +

                     " WHERE " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                     " AND " + AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetLedgerNumberDBName() + " = " +
                     ALedgerNumber.ToString() +
                     " AND " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDonorKeyDBName() + " = " + ADonorKey.ToString() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetBatchNumberDBName() + " = " + AGiftDetailTable.GetTableDBName() +
                     "." + AGiftDetailTable.GetBatchNumberDBName() +
                     " AND " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetBatchNumberDBName() + " = " +
                     AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetBatchNumberDBName() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetGiftTransactionNumberDBName() + " = " +
                     AGiftDetailTable.GetTableDBName() + "." + AGiftDetailTable.GetGiftTransactionNumberDBName() +
                     " AND " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName() + " BETWEEN '" + AStartDate.ToString(
                "yyyy-MM-dd") + "' AND '" + AEndDate.ToString("yyyy-MM-dd") + "'" +
                     " AND " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetBatchStatusDBName() + " = 'Posted'" +
                     " ORDER BY " + AGiftTable.GetTableDBName() + "." + AGiftTable.GetDateEnteredDBName() + " DESC LIMIT 1";

            DataTable Table = situation.GetDatabaseConnection().SelectDT(StrSql, "table",
                situation.GetDatabaseConnection().Transaction, new OdbcParameter[] { });

            if (Table.Rows.Count > 0)
            {
                DateTime DateEntered = (DateTime)Table.Rows[0][AGiftTable.GetDateEnteredDBName()];
                decimal CurrentAmount = Convert.ToDecimal(Table.Rows[0]["CurrentAmount"]);
                String MotivationDetail = (String)Table.Rows[0][AGiftDetailTable.GetMotivationDetailCodeDBName()];
                String MotivationGroup = (String)Table.Rows[0][AGiftDetailTable.GetMotivationGroupCodeDBName()];

                situation.GetParameters().Add("LastGiftDate", new TVariant(DateEntered));
                situation.GetParameters().Add("LastGiftAmount", new TVariant(CurrentAmount));
                situation.GetParameters().Add("MotivationDetail", new TVariant(MotivationDetail));
                situation.GetParameters().Add("MotivationGroup", new TVariant(MotivationGroup));

                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ATopXAmount">the Maximum Amount</param>
        /// <param name="ABottomXAmount">the Minimum Amount</param>
        /// <param name="ACummulativeAmount">The accummalated amount of all donors</param>
        /// <returns></returns>
        private bool IsTopDonor(decimal ATopXAmount, decimal ABottomXAmount, decimal ACummulativeAmount)
        {
            if ((ACummulativeAmount <= ATopXAmount)
                && (ACummulativeAmount >= ABottomXAmount))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// This report considers gifts given between the two specified dates, and can include all gifts or, if
        /// selected, those to a particular motivation, motivation detail or recipient. For the defined set of gifts
        /// and its total value, the donors are sorted into a list, starting with those who gave most, and showing
        /// the percentage that their gifts contributed to the total received (for this motivation or recipient, if
        /// specified) and the cumulative percentage, moving down the list starting with the top donor.
        /// </summary>
        /// <param name="ATotalAmount">Pre calculated value of the total gifts given with these parameters</param>
        /// <param name="ATopXPercent">Upper limit of the percentage to show in the report</param>
        /// <param name="ABottomXPercent">Lower limit of the percentage to show in the report</param>
        /// <param name="AExtract">true to use only partners from an extract</param>
        /// <param name="AExtractName">extract name</param>
        /// <param name="AStartDate">Start date of the gifts given</param>
        /// <param name="AEndDate">End date of the gifts given</param>
        /// <param name="ARecipientKey">Partner key of a specific recipient. If 0 then use all recipients</param>
        /// <param name="AMotivationGroup">Limit gifts to this motivation group. If % use all motivation groups</param>
        /// <param name="AMotivationDetail">Limit gifts to this motivation detail. If % use all motivation details</param>
        /// <returns></returns>
        private bool MakeTopDonor(decimal ATotalAmount, decimal ATopXPercent, decimal ABottomXPercent,
            bool AExtract, String AExtractName, DateTime AStartDate, DateTime AEndDate,
            Int64 ARecipientKey, String AMotivationGroup, String AMotivationDetail)
        {
            Int64 LedgerNumber = situation.GetParameters().Get("param_ledger_number_i").ToInt64();
            String CurrencyType = situation.GetParameters().Get("param_currency").ToString();
            StringBuilder SqlString = new StringBuilder();

            SqlString.Append("SELECT DISTINCT ");
            SqlString.Append("gift.p_donor_key_n AS DonorKey, ");
            SqlString.Append(PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + " AS ShortName, ");
            SqlString.Append(PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerClassDBName() + " AS PartnerClass, ");

            if (CurrencyType == "Base")
            {
                SqlString.Append("SUM(detail." + AGiftDetailTable.GetGiftAmountDBName() + ") AS Amount ");
            }
            else
            {
                SqlString.Append("SUM(detail." + AGiftDetailTable.GetGiftAmountIntlDBName() + ") AS Amount ");
            }

            SqlString.Append(
                " FROM " + AGiftTable.GetTableDBName() + " as gift, " + AGiftDetailTable.GetTableDBName() + " as detail, " +
                PPartnerTable.GetTableDBName() + ", " + AGiftBatchTable.GetTableDBName() + " ");

            if (AExtract)
            {
                SqlString.Append(", " + MExtractTable.GetTableDBName() + ", " + MExtractMasterTable.GetTableDBName());
                SqlString.Append(
                    " WHERE gift." + AGiftTable.GetDonorKeyDBName() + " = " + MExtractTable.GetTableDBName() + "." +
                    MExtractTable.GetPartnerKeyDBName());
                SqlString.Append(
                    " AND " + MExtractTable.GetTableDBName() + "." + MExtractTable.GetExtractIdDBName() + " = " +
                    MExtractMasterTable.GetTableDBName() +
                    "." + MExtractMasterTable.GetExtractIdDBName());
                SqlString.Append(" AND " + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetExtractNameDBName() + " = '");
                SqlString.Append(AExtractName);
                SqlString.Append("' AND ");
            }
            else
            {
                SqlString.Append(" WHERE ");
            }

            SqlString.Append(" detail." + AGiftDetailTable.GetLedgerNumberDBName() + " = gift." + AGiftTable.GetLedgerNumberDBName());
            SqlString.Append(" AND detail." + AGiftDetailTable.GetBatchNumberDBName() + " = gift." + AGiftTable.GetBatchNumberDBName());
            SqlString.Append(
                " AND detail." + AGiftDetailTable.GetGiftTransactionNumberDBName() + " = gift." + AGiftTable.GetGiftTransactionNumberDBName());
            SqlString.Append(" AND gift." + AGiftTable.GetDateEnteredDBName() + " BETWEEN '");
            SqlString.Append(AStartDate.ToString("yyyy-MM-dd"));
            SqlString.Append("' AND '");
            SqlString.Append(AEndDate.ToString("yyyy-MM-dd"));
            SqlString.Append("' AND gift." + AGiftTable.GetLedgerNumberDBName() + " = ");
            SqlString.Append(LedgerNumber.ToString());
            SqlString.Append(" AND " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetLedgerNumberDBName() + " = ");
            SqlString.Append(LedgerNumber.ToString());
            SqlString.Append(
                " AND " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetBatchNumberDBName() + " = gift." +
                AGiftTable.GetBatchNumberDBName());
            SqlString.Append(" AND ( " + AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetBatchStatusDBName() + " = 'Posted' OR ");
            SqlString.Append(AGiftBatchTable.GetTableDBName() + "." + AGiftBatchTable.GetBatchStatusDBName() + " = 'posted' ) ");
            SqlString.Append(
                " AND " + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName() + " = gift." + AGiftTable.GetDonorKeyDBName());

            if (ARecipientKey != 0)
            {
                SqlString.Append(" AND detail." + AGiftDetailTable.GetRecipientKeyDBName() + " = ");
                SqlString.Append(ARecipientKey.ToString());
            }

            if (AMotivationGroup != "%")
            {
                SqlString.Append(" AND  detail." + AGiftDetailTable.GetMotivationGroupCodeDBName() + " LIKE '");
                SqlString.Append(AMotivationGroup);
                SqlString.Append("' ");
            }

            if (AMotivationDetail != "%")
            {
                SqlString.Append(" AND  detail." + AGiftDetailTable.GetMotivationDetailCodeDBName() + " LIKE '");
                SqlString.Append(AMotivationDetail);
                SqlString.Append("' ");
            }

            SqlString.Append(" GROUP BY gift." + AGiftTable.GetDonorKeyDBName() + ", ");
            SqlString.Append(PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + ", ");
            SqlString.Append(PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerClassDBName());
            SqlString.Append(" ORDER BY Amount DESC");

            DataTable Table = situation.GetDatabaseConnection().SelectDT(SqlString.ToString(), "table",
                situation.GetDatabaseConnection().Transaction, new OdbcParameter[] { });

            decimal CummulativeAmount = 0;
            decimal TopAmount = ATotalAmount * ATopXPercent / 100;
            decimal BottomAmount = ATotalAmount * ABottomXPercent / 100;

            int NumColumns = 7;
            int ChildRow = 1;
            situation.GetResults().Clear();

            for (int Counter = 0; Counter < Table.Rows.Count; ++Counter)
            {
                decimal CurrentAmount = Convert.ToDecimal(Table.Rows[Counter]["Amount"]);

                if (CurrentAmount < 0)
                {
                    continue;
                }

                if ((CummulativeAmount <= TopAmount)
                    && (CummulativeAmount >= BottomAmount))
                {
                    Int64 DonorKey = Convert.ToInt64(Table.Rows[Counter]["DonorKey"]);
                    String ShortName = (String)Table.Rows[Counter]["ShortName"];
                    String PartnerClass = (String)Table.Rows[Counter]["PartnerClass"];

                    CummulativeAmount += CurrentAmount;

                    // Transfer to results
                    TVariant[] Header = new TVariant[NumColumns];
                    TVariant[] Description =
                    {
                        new TVariant(), new TVariant()
                    };
                    TVariant[] Columns = new TVariant[NumColumns];

                    for (int Counter2 = 0; Counter2 < NumColumns; ++Counter2)
                    {
                        Header[Counter2] = new TVariant();
                        Columns[Counter2] = new TVariant();
                    }

                    StringBuilder PartnerAddress = new StringBuilder();
                    PPartnerLocationRow AddressRow;

                    if (Ict.Petra.Server.MReporting.MPartner.TRptUserFunctionsPartner.GetPartnerBestAddressRow(DonorKey, situation, out AddressRow))
                    {
                        PLocationTable LocationTable = PLocationAccess.LoadByPrimaryKey(AddressRow.SiteKey,
                            AddressRow.LocationKey, situation.GetDatabaseConnection().Transaction);

                        if (LocationTable.Rows.Count > 0)
                        {
                            PLocationRow LocationRow = (PLocationRow)LocationTable.Rows[0];

                            PartnerAddress.Append(LocationRow.Locality);

                            if (LocationRow.Locality.Length > 0)
                            {
                                PartnerAddress.Append(", ");
                            }

                            PartnerAddress.Append(LocationRow.StreetName);

                            if (PartnerAddress.Length > 0)
                            {
                                PartnerAddress.Append(", ");
                            }

                            PartnerAddress.Append(LocationRow.Address3);

                            if (PartnerAddress.Length > 0)
                            {
                                PartnerAddress.Append(", ");
                            }

                            PartnerAddress.Append(LocationRow.PostalCode);
                            PartnerAddress.Append(" ");
                            PartnerAddress.Append(LocationRow.City);

                            if (LocationRow.County.Length > 0)
                            {
                                PartnerAddress.Append(", ");
                                PartnerAddress.Append(LocationRow.County);
                            }

                            PartnerAddress.Append(", ");
                            PartnerAddress.Append(LocationRow.CountryCode);
                        }
                    }

                    Columns[0] = new TVariant(DonorKey.ToString("0000000000"));
                    Columns[1] = new TVariant(PartnerClass);
                    Columns[2] = new TVariant(ShortName);
                    Columns[3] = new TVariant(CurrentAmount, "-#,##0.00;#,##0.00");
                    Columns[4] = new TVariant((CurrentAmount * 100 / ATotalAmount), "-#,##0.00;#,##0.00");
                    Columns[5] = new TVariant((CummulativeAmount * 100 / ATotalAmount), "-#,##0.00;#,##0.00");
                    Columns[6] = new TVariant(PartnerAddress.ToString());

                    situation.GetResults().AddRow(0, ChildRow++, true, 2, "", "", false,
                        Header, Description, Columns);
                }
                else
                {
                    CummulativeAmount += CurrentAmount;
                }
            }

            return true;
        }
    }
}