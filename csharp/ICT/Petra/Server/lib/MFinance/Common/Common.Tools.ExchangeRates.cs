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
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
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

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            currencyTable = ACurrencyAccess.LoadAll(transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (currencyTable.Rows.Count == 0)
            {
                TVerificationException terminate = new TVerificationException(
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

            TVerificationException terminate = new TVerificationException(
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
                TVerificationException terminate = new TVerificationException(
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
                return -1;
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
        /// <param name="databaseConnection"></param>
        /// <param name="pv_ledger_number_i"></param>
        /// <param name="pv_year_i"></param>
        /// <param name="pv_period_i"></param>
        /// <param name="currentFinancialYear"></param>
        /// <returns></returns>
        public decimal GetCorporateExchangeRate(TDataBase databaseConnection,
            int pv_ledger_number_i,
            int pv_year_i,
            int pv_period_i,
            int currentFinancialYear)
        {
            decimal ReturnValue;

            foreach (TExchangeRate exchangeRateElement in exchangeRates)
            {
                if ((exchangeRateElement.ledger_number_i == pv_ledger_number_i) && (exchangeRateElement.year_i == pv_year_i)
                    && (exchangeRateElement.period_i == pv_period_i))
                {
                    return exchangeRateElement.rate_n;
                }
            }

            ReturnValue = GetCorporateExchangeRateFromDB(databaseConnection, pv_ledger_number_i, pv_year_i, pv_period_i, currentFinancialYear);
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
        /// get daily exchange rate for the given currencies and date;
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <returns>Zero if no exchange rate found</returns>
        public static decimal GetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective)
        {
            if (ACurrencyFrom == ACurrencyTo)
            {
                return 1.0M;
            }

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ADailyExchangeRateRow fittingRate = null;

            // TODO: get the most recent exchange rate for the given date and currencies
            bool oppositeRate = false;
            ADailyExchangeRateTable rates = ADailyExchangeRateAccess.LoadByPrimaryKey(ACurrencyFrom, ACurrencyTo, ADateEffective, 0, Transaction);

            if (rates.Count == 0)
            {
                // try other way round
                rates = ADailyExchangeRateAccess.LoadByPrimaryKey(ACurrencyTo, ACurrencyFrom, ADateEffective, 0, Transaction);
                oppositeRate = true;
            }

            if (rates.Count == 1)
            {
                fittingRate = rates[0];
            }
            else if (rates.Count == 0)
            {
                // TODO: collect exchange rate from the web; save to db
                // see tracker http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=87

                // Or look for most recent exchange rate???
                ADailyExchangeRateTable tempTable = new ADailyExchangeRateTable();
                ADailyExchangeRateRow templateRow = tempTable.NewRowTyped(false);
                templateRow.FromCurrencyCode = ACurrencyFrom;
                templateRow.ToCurrencyCode = ACurrencyTo;
                oppositeRate = false;
                rates = ADailyExchangeRateAccess.LoadUsingTemplate(templateRow, Transaction);

                if (rates.Count == 0)
                {
                    templateRow.FromCurrencyCode = ACurrencyTo;
                    templateRow.ToCurrencyCode = ACurrencyFrom;
                    oppositeRate = true;
                    rates = ADailyExchangeRateAccess.LoadUsingTemplate(templateRow, Transaction);
                }

                if (rates.Count > 0)
                {
                    // sort rates by date, look for rate just before the date we are looking for
                    rates.DefaultView.Sort = ADailyExchangeRateTable.GetDateEffectiveFromDBName();
                    rates.DefaultView.RowFilter = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + "<= #" +
                                                  ADateEffective.ToString("yyyy-MM-dd") + "#";

                    if (rates.DefaultView.Count > 0)
                    {
                        fittingRate = (ADailyExchangeRateRow)rates.DefaultView[rates.DefaultView.Count - 1].Row;
                    }
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (fittingRate != null)
            {
                if (oppositeRate)
                {
                    return 1.0M / fittingRate.RateOfExchange;
                }

                return fittingRate.RateOfExchange;
            }

            TLogging.Log("Cannot find exchange rate for " + ACurrencyFrom + " " + ACurrencyTo);

            //return 1.0M;
            //Instead, cause a validation error to force the user to select an exchange rate
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
                ExchangeRate = 1.0M;
                TLogging.Log("cannot find rate for " + ACurrencyFrom + " " + ACurrencyTo);
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
        /// <param name="AExchangeRate"></param>
        /// <returns>true if a exchange rate was found for the date. Otherwise false</returns>
        public static bool GetCorporateExchangeRate(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime AStartDate,
            DateTime AEndDate,
            out decimal AExchangeRate)
        {
            AExchangeRate = decimal.MinValue;

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ACorporateExchangeRateTable tempTable = new ACorporateExchangeRateTable();
            ACorporateExchangeRateRow templateRow = tempTable.NewRowTyped(false);

            templateRow.FromCurrencyCode = ACurrencyFrom;
            templateRow.ToCurrencyCode = ACurrencyTo;

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
                    AExchangeRate = ((ACorporateExchangeRateRow)ExchangeRates.DefaultView[0].Row).RateOfExchange;
                }
            }

            if (AExchangeRate == decimal.MinValue)
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
                        AExchangeRate = 1 / ((ACorporateExchangeRateRow)ExchangeRates.DefaultView[0].Row).RateOfExchange;
                    }
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (AExchangeRate == decimal.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the latest Corporate exchange rate
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AIntlExchangeRate"></param>
        /// <returns></returns>
        public static bool GetLatestIntlCorpExchangeRate(int ALedgerNumber, out decimal AIntlExchangeRate)
        {
            bool retVal = true;
            string CurrencyFrom;
            string CurrencyTo;
            DateTime StartDate;
            DateTime EndDate;

            AIntlExchangeRate = decimal.MinValue;

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            string IntlCurrency = LedgerRow.IntlCurrency.Trim();
            string BaseCurrency = LedgerRow.BaseCurrency;
            int CurrentPeriod = LedgerRow.CurrentPeriod;

            if (IntlCurrency != string.Empty)
            {
                //ACurrencyTable CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(IntlCurrency, null);
                AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, CurrentPeriod, Transaction);
                AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];

                if (BaseCurrency == IntlCurrency)
                {
                    AIntlExchangeRate = 1;
                }
                else
                {
                    CurrencyFrom = BaseCurrency;
                    CurrencyTo = IntlCurrency;
                    StartDate = AccountingPeriodRow.PeriodStartDate;
                    EndDate = AccountingPeriodRow.PeriodEndDate;
                    retVal = GetCorporateExchangeRate(CurrencyFrom, CurrencyTo, StartDate, EndDate, out AIntlExchangeRate);
                }
            }

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            if (AIntlExchangeRate == decimal.MinValue)
            {
                retVal = false;
            }

            return retVal;
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