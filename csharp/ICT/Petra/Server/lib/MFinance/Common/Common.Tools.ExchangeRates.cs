//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, christophert, timop
//
// Copyright 2004-2013 by OM International
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

using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.CrossLedger.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common.WebConnectors;
using Ict.Petra.Server.MCommon.Data.Access;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Base on the idea to reduce the number of database request to it's minimum, this object reads
    /// the complete a_currency table. Two currency slots are provided, a base currency slot and a foreigen
    /// currency slot. The the base currency slot can only set in one of the constructors one time and the
    /// foreign currency slot can easily be switched to an other currency by using the
    /// ForeignCurrencyCode property without and any more database request.
    ///
    /// The petra table a_currency contains a "hidden information" about the number of digits and this
    /// value will be used either to calculate the number of value dependet rounding digits.
    /// Furthermore there are some servic routines base on this information.
    /// </summary>

    public class TCurrencyInfo
    {
        private ACurrencyTable currencyTable = null;
        private ACurrencyRow baseCurrencyRow = null;
        private ACurrencyRow foreignCurrencyRow = null;
        private int intBaseCurrencyDigits;
        private int intForeignCurrencyDigits;
        private const int DIGIT_INIT_VALUE = -1;

        /// <summary>
        /// Constructor which automatically loads the table and sets the value of the
        /// currency table.
        /// </summary>
        /// <param name="ACurrencyCode">Three digit description to define the
        /// base currency.</param>
        public TCurrencyInfo(string ACurrencyCode)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ACurrencyCode);
        }

        /// <summary>
        /// Constructor which automatically loads the table and sets the value of
        /// the base currency table and foreign currency table.
        /// </summary>
        /// <param name="ABaseCurrencyCode">Base currency code</param>
        /// <param name="AForeignCurrencyCode">foreign Currency Code</param>
        public TCurrencyInfo(string ABaseCurrencyCode, string AForeignCurrencyCode)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ABaseCurrencyCode);
            foreignCurrencyRow = SetRowToCode(AForeignCurrencyCode);
        }

        private void LoadDatabase()
        {
            intBaseCurrencyDigits = DIGIT_INIT_VALUE;
            intForeignCurrencyDigits = DIGIT_INIT_VALUE;

            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    currencyTable = ACurrencyAccess.LoadAll(transaction);
                });

            if (currencyTable.Rows.Count == 0)
            {
                EVerificationException terminate = new EVerificationException(
                    Catalog.GetString("The table a_currency is empty!"));
                terminate.Context = "Common Accounting";
                terminate.ErrorCode = "TCurrencyInfo01";
                throw terminate;
            }
        }

        private ACurrencyRow SetRowToCode(string ACurrencyCode)
        {
            if (ACurrencyCode.Equals(String.Empty))
            {
                return null;
            }

            for (int i = 0; i < currencyTable.Rows.Count; ++i)
            {
                if (ACurrencyCode.Equals(((ACurrencyRow)currencyTable[i]).CurrencyCode))
                {
                    return (ACurrencyRow)currencyTable[i];
                }
            }

            EVerificationException terminate = new EVerificationException(
                String.Format(Catalog.GetString(
                        "No Data for currency {0} found"), ACurrencyCode));
            terminate.Context = "Common Accounting";
            terminate.ErrorCode = "TCurrencyInfo02";
            throw terminate;
        }

        /// <summary>
        /// Property to read the base currency code value.
        /// </summary>
        public string CurrencyCode
        {
            get
            {
                return baseCurrencyRow.CurrencyCode;
            }
        }

        /// <summary>
        /// Property to read and to set the foreign currency code value.
        /// </summary>
        public string ForeignCurrencyCode
        {
            get
            {
                return foreignCurrencyRow.CurrencyCode;
            }
            set
            {
                intForeignCurrencyDigits = DIGIT_INIT_VALUE;
                foreignCurrencyRow = SetRowToCode(value);
            }
        }

        /// <summary>
        /// This rotine handles a correct roundig by using the number of digits in a_currency
        /// </summary>
        /// <param name="AValueToRound"></param>
        /// <returns></returns>
        public decimal RoundBaseCurrencyValue(decimal AValueToRound)
        {
            return Math.Round(AValueToRound, digits);
        }

        /// <summary>
        /// This rotine handles a correct roundig by using the number of digits in a_currency
        /// </summary>
        /// <param name="AValueToRound"></param>
        /// <returns></returns>
        public decimal RoundForeignCurrencyValue(decimal AValueToRound)
        {
            return Math.Round(AValueToRound, foreignDigits);
        }

        /// <summary>
        /// Here you can calculate the base value amount by defining the parameters below.
        /// Because of the rounding the ToBaseValue(ToForeignValue(someValue)) != someValue
        /// </summary>
        /// <param name="AForeignValue"></param>
        /// <param name="AExchangeRate"></param>
        /// <returns></returns>
        public decimal ToBaseValue(decimal AForeignValue, decimal AExchangeRate)
        {
            return RoundBaseCurrencyValue(AForeignValue * AExchangeRate);
        }

        /// <summary>
        /// Here you can calculate the base value amount by defining the parameters below.
        /// Because of the rounding the ToBaseValue(ToForeignValue(someValue)) != someValue
        /// </summary>
        /// <param name="ABaseValue"></param>
        /// <param name="AExchangeRate"></param>
        /// <returns></returns>
        public decimal ToForeignValue(decimal ABaseValue, decimal AExchangeRate)
        {
            return RoundForeignCurrencyValue(ABaseValue / AExchangeRate);
        }

        /// <summary>
        /// Calculates the number of digits by reading the row.DisplayFormat
        /// Entry of the currency table and convert the old petra string to an
        /// integer response.
        /// </summary>
        public int digits
        {
            get
            {
                if (intBaseCurrencyDigits == DIGIT_INIT_VALUE)
                {
                    intBaseCurrencyDigits = new FormatConverter(baseCurrencyRow.DisplayFormat).digits;
                }

                return intBaseCurrencyDigits;
            }
        }

        /// <summary>
        /// Calculates the number of digits for the foreign currency.
        /// </summary>
        public int foreignDigits
        {
            get
            {
                if (intForeignCurrencyDigits == DIGIT_INIT_VALUE)
                {
                    intForeignCurrencyDigits = new FormatConverter(foreignCurrencyRow.DisplayFormat).digits;
                }

                return intForeignCurrencyDigits;
            }
        }
    }

    /// <summary>
    /// This class is a local Format converter <br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.99").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9.9").digits.ToString());<br />
    ///  Console.WriteLine(new FormatConverter("->>>,>>>,>>>,>>9").digits.ToString());<br />
    /// The result is 2,1 and 0 digits ..
    /// </summary>
    public class FormatConverter
    {
        string sRegex;
        Regex reg;
        MatchCollection matchCollection;
        int intDigits;

        /// <summary>
        ///
        /// </summary>
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                EVerificationException terminate = new EVerificationException(
                    String.Format(Catalog.GetString("The regular expression {0} does not fit for a match in {1}"),
                        sRegex, strFormat));

                terminate.Context = "Common Accountig";
                terminate.ErrorCode = "TCurrencyInfo03";
                throw terminate;
            }

            intDigits = (matchCollection[0].Value).Length - 3;

            if (intDigits == -1)
            {
                intDigits = 0;
            }

            if (intDigits < -1)
            {
                intDigits = 2;
            }
        }

        /// <summary>
        /// Property to report the number of digits
        /// </summary>
        public int digits
        {
            get
            {
                return intDigits;
            }
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TExchangeRate
    {
        /// <summary>todoComment</summary>
        public int ledger_number_i;

        /// <summary>todoComment</summary>
        public int period_i;

        /// <summary>todoComment</summary>
        public int year_i;

        /// <summary>todoComment</summary>
        public decimal rate_n;
    }

    /// <summary>
    /// a cache for corporate exchange rates. mainly used by the reporting tool
    /// </summary>
    public class TCorporateExchangeRateCache
    {
        private List <TExchangeRate>exchangeRates;

        /// <summary>
        /// constructor
        /// </summary>
        public TCorporateExchangeRateCache() : base()
        {
            exchangeRates = new List <TExchangeRate>();
        }

        private decimal GetCorporateExchangeRateFromDB(TDataBase databaseConnection,
            int pv_ledger_number_i,
            int pv_year_i,
            int pv_period_i,
            int currentFinancialYear)
        {
            ALedgerTable ledgerTable = ALedgerAccess.LoadByPrimaryKey(pv_ledger_number_i, databaseConnection.Transaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(pv_ledger_number_i,
                pv_period_i,
                databaseConnection.Transaction);

            if (AccountingPeriodTable.Rows.Count < 1)
            {
                return -1; // This is poor (because the caller can blindly use it!)
                           // I wonder whether an exception would be better. (Tim Ingham, Oct 2013)
            }

            if (currentFinancialYear < 0)
            {
                currentFinancialYear = ledgerTable[0].CurrentFinancialYear;
            }

            DateTime startOfPeriod = AccountingPeriodTable[0].PeriodStartDate;
            DateTime endOfPeriod = AccountingPeriodTable[0].PeriodEndDate;

            startOfPeriod = new DateTime(startOfPeriod.Year - (currentFinancialYear - pv_year_i), startOfPeriod.Month, startOfPeriod.Day);

            if ((endOfPeriod.Month == 2) && (endOfPeriod.Day == 29)
                && (((currentFinancialYear - pv_year_i)) % 4 != 0))
            {
                endOfPeriod = endOfPeriod.AddDays(-1);
            }

            endOfPeriod = new DateTime(endOfPeriod.Year - (currentFinancialYear - pv_year_i), endOfPeriod.Month, endOfPeriod.Day);

            // get the corporate exchange rate between base and intl currency for the period
            return TExchangeRateTools.GetCorporateExchangeRate(ledgerTable[0].IntlCurrency,
                ledgerTable[0].BaseCurrency,
                startOfPeriod,
                endOfPeriod);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="databaseConnection">The database connection.</param>
        /// <param name="pv_ledger_number_i">The pv_ledger_number_i.</param>
        /// <param name="pv_year_i">The pv_year_i.</param>
        /// <param name="pv_period_i">The pv_period_i.</param>
        /// <param name="currentFinancialYear">The current financial year.</param>
        /// <returns></returns>
        public decimal GetCorporateExchangeRate(TDataBase databaseConnection,
            int pv_ledger_number_i,
            int pv_year_i,
            int pv_period_i,
            int currentFinancialYear)
        {
            if (pv_period_i == 0) // I sometimes get asked for this. There's no period 0.
            {
                pv_period_i = 12; // Perhaps I should look up this value from number of periods?
                pv_year_i--;
            }

            foreach (TExchangeRate exchangeRateElement in exchangeRates)
            {
                if ((exchangeRateElement.ledger_number_i == pv_ledger_number_i) && (exchangeRateElement.year_i == pv_year_i)
                    && (exchangeRateElement.period_i == pv_period_i))
                {
                    return exchangeRateElement.rate_n;
                }
            }

            decimal ReturnValue = GetCorporateExchangeRateFromDB(databaseConnection, pv_ledger_number_i, pv_year_i, pv_period_i, currentFinancialYear);
            //
            // Cache this for the next time I'm asked...
            TExchangeRate exchangeRateElement2 = new TExchangeRate();
            exchangeRateElement2.ledger_number_i = pv_ledger_number_i;
            exchangeRateElement2.year_i = pv_year_i;
            exchangeRateElement2.period_i = pv_period_i;
            exchangeRateElement2.rate_n = ReturnValue;
            exchangeRates.Add(exchangeRateElement2);
            return ReturnValue;
        }
    }

    /// <summary>
    /// several static functions to get the exchange rates from the database
    /// </summary>
    public class TExchangeRateTools
    {
        /// <summary>
        /// Gets daily exchange rate for the given currencies and date.  There is no limit on how 'old' the rate can be.
        /// If more than one rate exists on or before the specified date the latest one is returned.  This might be old or it might
        /// be one of several on the same day.
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <returns>Zero if no exchange rate found</returns>
        public static decimal GetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective)
        {
            return GetDailyExchangeRate(ACurrencyFrom, ACurrencyTo, ADateEffective, -1, false);
        }

        /// <summary>
        /// Gets daily exchange rate for the given currencies and date. The APriorDaysAllwed parameter limits how 'old' the rate can be.
        /// The unique rate parameter can ensure that a rate is only returned if there is only one to choose from.
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <param name="APriorDaysAllowed">Sets a limit on how many days prior to ADateEffective to search.  Use -1 for no limit,
        /// 0 to imply that the rate must match for the specified date, 1 for the date and the day before and so on.</param>
        /// <param name="AEnforceUniqueRate">If true the method will only return a value if there is one unique rate in the date range.
        /// Otherwise it returns the latest rate.</param>
        /// <returns>Zero if no exchange rate found</returns>
        public static decimal GetDailyExchangeRate(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime ADateEffective,
            int APriorDaysAllowed,
            Boolean AEnforceUniqueRate)
        {
            // TODO: collect exchange rate from the web; save to db
            // see Mantis tracker case #87

            // The rule is that we don't enforce finding a unique rate over the whole date range as that doesn't really make sense
            if (AEnforceUniqueRate && (APriorDaysAllowed == -1))
            {
                throw new ArgumentException(
                    "The GetDailyExchangeRate method does not allow 'AEnforceUniqueRate' to be true when 'APriorDaysAllowed' is -1. Unique rates should only be requested over a limited date range.");
            }

            if (ACurrencyFrom == ACurrencyTo)
            {
                return 1.0M;
            }

            // Define our earliest date, if set
            DateTime earliestDate = DateTime.MinValue;

            if (APriorDaysAllowed >= 0)
            {
                earliestDate = ADateEffective.AddDays(-APriorDaysAllowed);
            }

            // Query the database...
            TDBTransaction transaction = null;
            bool oppositeRate = false;
            ADailyExchangeRateRow fittingRate = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    ExchangeRateTDS allRates = TCrossLedger.LoadDailyExchangeRateData(false, earliestDate, ADateEffective);
                    ADailyExchangeRateTable rates = allRates.ADailyExchangeRate;
                    ADailyExchangeRateRow uniqueFittingRow = null;

                    // sort rates by date, look for rate just before the date we are looking for
                    string rowFilter = String.Format("{0}='{1}' AND {2}='{3}'",
                        ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                        ACurrencyFrom,
                        ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                        ACurrencyTo);

                    rates.DefaultView.RowFilter = rowFilter;
                    rates.DefaultView.Sort = String.Format("{0} DESC, {1} DESC",
                        ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                        ADailyExchangeRateTable.GetTimeEffectiveFromDBName());

                    // If we are after a unique row we need to remember the one from this view, if it exists
                    if (AEnforceUniqueRate && (rates.DefaultView.Count == 1))
                    {
                        uniqueFittingRow = (ADailyExchangeRateRow)rates.DefaultView[0].Row;
                    }

                    if ((rates.DefaultView.Count == 0) || AEnforceUniqueRate)
                    {
                        // try other way round
                        rowFilter = String.Format("{0}='{1}' AND {2}='{3}'",
                            ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                            ACurrencyFrom,
                            ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                            ACurrencyTo);

                        rates.DefaultView.RowFilter = rowFilter;
                        oppositeRate = true;

                        if (AEnforceUniqueRate)
                        {
                            if (rates.DefaultView.Count > 1)
                            {
                                // This way round does not have a unique rate
                                uniqueFittingRow = null;
                            }
                            else if ((rates.DefaultView.Count == 1) && (uniqueFittingRow == null))
                            {
                                // we will use this as our unique rate
                                uniqueFittingRow = (ADailyExchangeRateRow)rates.DefaultView[0].Row;
                            }
                            else
                            {
                                // put this variable back as it was
                                oppositeRate = false;
                            }
                        }
                    }

                    if (AEnforceUniqueRate)
                    {
                        // Did we get a unique rate?
                        if (uniqueFittingRow != null)
                        {
                            fittingRate = uniqueFittingRow;
                        }
                    }
                    else if (rates.DefaultView.Count > 0)
                    {
                        // Just return the first in the list
                        fittingRate = (ADailyExchangeRateRow)rates.DefaultView[0].Row;
                    }
                });

            if (fittingRate != null)
            {
                if (oppositeRate)
                {
                    return GLRoutines.Divide(1.0m, fittingRate.RateOfExchange);
                }

                return fittingRate.RateOfExchange;
            }

            TLogging.Log("Cannot find daily exchange rate for " + ACurrencyFrom + " " + ACurrencyTo + " for " + ADateEffective.ToString("yyyy-MM-dd"));

            //Returning 0 causes a validation error to force the user to select an exchange rate:
            return 0M;
        }

        /// <summary>
        /// get corporate exchange rate for the given currencies and date;
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <returns></returns>
        public static decimal GetCorporateExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime AStartDate, DateTime AEndDate)
        {
            decimal ExchangeRate = 1.0M;

            if (ACurrencyFrom == ACurrencyTo)
            {
                return ExchangeRate;
            }

            if (!GetCorporateExchangeRate(ACurrencyFrom, ACurrencyTo, AStartDate, AEndDate, out ExchangeRate))
            {
                //ExchangeRate = 1.0M;
                //Instead return 0 to make it easy to catch error
                ExchangeRate = 0M;
                TLogging.Log("Cannot find corporate exchange rate for " + ACurrencyFrom + " " + ACurrencyTo);
            }

            return ExchangeRate;
        }

        /// <summary>
        /// get corporate exchange rate for the given currencies and date;
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <param name="AExchangeRateToFind"></param>
        /// <returns>true if a exchange rate was found for the date. Otherwise false</returns>
        public static bool GetCorporateExchangeRate(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime AStartDate,
            DateTime AEndDate,
            out decimal AExchangeRateToFind)
        {
            AExchangeRateToFind = decimal.MinValue;
            decimal ExchangeRateToFind = AExchangeRateToFind;

            TDBTransaction Transaction = null;

            ACorporateExchangeRateTable tempTable = new ACorporateExchangeRateTable();
            ACorporateExchangeRateRow templateRow = tempTable.NewRowTyped(false);

            templateRow.FromCurrencyCode = ACurrencyFrom;
            templateRow.ToCurrencyCode = ACurrencyTo;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    try
                    {
                        ACorporateExchangeRateTable ExchangeRates = ACorporateExchangeRateAccess.LoadUsingTemplate(templateRow, Transaction);

                        if (ExchangeRates.Count > 0)
                        {
                            // sort rates by date, look for rate just before the date we are looking for
                            ExchangeRates.DefaultView.Sort = ACorporateExchangeRateTable.GetDateEffectiveFromDBName();
                            ExchangeRates.DefaultView.RowFilter = ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + ">= #" +
                                                                  AStartDate.ToString("yyyy-MM-dd") + "# AND " +
                                                                  ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + "<= #" +
                                                                  AEndDate.ToString("yyyy-MM-dd") + "#";

                            if (ExchangeRates.DefaultView.Count > 0)
                            {
                                ExchangeRateToFind = ((ACorporateExchangeRateRow)ExchangeRates.DefaultView[0].Row).RateOfExchange;
                            }
                        }

                        if (ExchangeRateToFind == decimal.MinValue)
                        {
                            // try other way round
                            templateRow.FromCurrencyCode = ACurrencyTo;
                            templateRow.ToCurrencyCode = ACurrencyFrom;

                            ExchangeRates = ACorporateExchangeRateAccess.LoadUsingTemplate(templateRow, Transaction);

                            if (ExchangeRates.Count > 0)
                            {
                                // sort rates by date, look for rate just before the date we are looking for
                                ExchangeRates.DefaultView.Sort = ACorporateExchangeRateTable.GetDateEffectiveFromDBName();
                                ExchangeRates.DefaultView.RowFilter = ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + ">= #" +
                                                                      AStartDate.ToString("yyyy-MM-dd") + "# AND " +
                                                                      ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + "<= #" +
                                                                      AEndDate.ToString("yyyy-MM-dd") + "#";

                                if (ExchangeRates.DefaultView.Count > 0)
                                {
                                    ExchangeRateToFind = 1 / ((ACorporateExchangeRateRow)ExchangeRates.DefaultView[0].Row).RateOfExchange;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TLogging.Log("Error in GetCorporateExchangeRate: " + e.Message);
                    }
                });

            AExchangeRateToFind = ExchangeRateToFind;

            return AExchangeRateToFind != decimal.MinValue;
        }

        /// <summary>
        /// Checks whether or not a given currency exists in the Currency table
        /// </summary>
        /// <param name="ACurrencyCode">The currency code to look for</param>
        /// <param name="ADBTransaction">The current transaction</param>
        /// <returns>True if exists, else false</returns>
        public static bool CheckCurrencyExists(string ACurrencyCode, TDBTransaction ADBTransaction)
        {
            return ACurrencyAccess.Exists(ACurrencyCode, ADBTransaction);
        }
    }
}