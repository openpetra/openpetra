//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


namespace Ict.Petra.Server.MFinance.GL
{
    public class Get_GLM_Info
    {
        DataTable aGLM;
        public Get_GLM_Info(int ALedgerNumber, string AAccountCode, string ACostCentreCode)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[1].Value = AAccountCode;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = ACostCentreCode;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetCostCentreCodeDBName() + " = ? ";
            aGLM = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        public decimal YtdActual
        {
            get
            {
                try
                {
                    return (decimal)aGLM.Rows[0][AGeneralLedgerMasterTable.GetYtdActualBaseDBName()];
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
            }
        }

        public decimal YtdForeign
        {
            get
            {
                try
                {
                    return (decimal)aGLM.Rows[0][AGeneralLedgerMasterTable.GetYtdActualForeignDBName()];
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
                catch (InvalidCastException)
                {
                    return 0;
                }
            }
        }
    }

    public class GetCostCenterInfo
    {
        ACostCentreTable costCentreTable;
        ACostCentreRow costCentreRow;
        public GetCostCenterInfo(int ALedgerNumber, string ACostCenterCode)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            costCentreTable = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, ACostCenterCode, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
            try
            {
                costCentreRow = (ACostCentreRow)costCentreTable.Rows[0];
            }
            catch (Exception)
            {
            }
        }

        public bool IsValid
        {
            get
            {
                return costCentreTable.Rows.Count == 1;
            }
        }
    }

    public class GetAccountInfo
    {
        AAccountTable accountTable;
        AAccountRow accountRow;

        public GetAccountInfo(int ALedgerNumber, string AAccountCode)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            accountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AAccountCode, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
            try
            {
                accountRow = (AAccountRow)accountTable.Rows[0];
            }
            catch (Exception)
            {
            }
        }

        public bool IsValid
        {
            get
            {
                return accountTable.Rows.Count == 1;
            }
        }

        public bool ForeignCurrencyFlag
        {
            get
            {
                return accountRow.ForeignCurrencyFlag;
            }
        }
        public string ForeignCurrencyCode
        {
            get
            {
                return accountRow.ForeignCurrencyCode;
            }
        }
    }

    /// <summary>
    /// Gets the specific date informations of an accounting intervall. This routine is either used by
    /// GL.PeriodEnd.Month and GL.Revaluation but in different senses. On time the dataset holds exact
    /// one row (Contructor with two parameters) and on time it holds a set of rows (Constructor with
    /// one parameter.
    /// </summary>
    public class GetAccountingPeriodInfo
    {
        private AAccountingPeriodTable periodTable = null;

        /// <summary>
        /// Constructor needs a valid ledger number.
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        public GetAccountingPeriodInfo(int ALedgerNumber)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            periodTable = AAccountingPeriodAccess.LoadViaALedger(ALedgerNumber, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// Constructor to adress a record by its primary key
        /// </summary>
        /// <param name="ALedgerNumber">the ledger number</param>
        /// <param name="ACurrentPeriod">the current accounting period</param>

        public GetAccountingPeriodInfo(int ALedgerNumber, int ACurrentPeriod)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            periodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, ACurrentPeriod, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// Selects to correct AAccountingPeriodRow or - in case of an error -
        /// it sets to null
        /// </summary>
        /// <param name="APeriodNum">Number of the requested period</param>
        /// <returns></returns>
        private AAccountingPeriodRow GetRowOfPeriod(int APeriodNum)
        {
            if (periodTable != null)
            {
                if (periodTable.Rows.Count != 0)
                {
                    for (int i = 0; i < periodTable.Rows.Count; ++i)
                    {
                        AAccountingPeriodRow periodRow =
                            (AAccountingPeriodRow)periodTable[i];

                        if (periodRow.AccountingPeriodNumber == APeriodNum)
                        {
                            return periodRow;
                        }
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the effective date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetEffectiveDateOfPeriod(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.EffectiveDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Reads the value of the first and hopefully only row.
        /// </summary>
        public DateTime EffectiveDate
        {
            get
            {
                AAccountingPeriodRow periodRow = (AAccountingPeriodRow)periodTable[0];
                return periodRow.EffectiveDate;
            }
        }


        /// <summary>
        /// Reads the end date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetDatePeriodEnd(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.PeriodEndDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Reads the start date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetDatePeriodStart(int APeriodNum)
        {
            AAccountingPeriodRow periodRow = GetRowOfPeriod(APeriodNum);

            if (periodRow != null)
            {
                return periodRow.PeriodStartDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public int Rows
        {
            get
            {
                return periodTable.Rows.Count;
            }
        }
    }

    public class SetLedgerParameter
    {
        int ledgerNumber;
        public SetLedgerParameter(int ALedgerNumber)
        {
            ledgerNumber = ALedgerNumber;
        }

        public bool ProvisionalYearEndFlag
        {
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Bit);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetProvisionalYearEndFlagDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }
    }

    /// <summary>
    /// This routine reads the line of a_ledger defined by the ledger number
    /// </summary>
    public class GetLedgerInfo
    {
        int ledgerNumber;
        private ALedgerTable ledger = null;

        /// <summary>
        /// Constructor to address the correct table line (relevant ledger number). The
        /// constructor only will run the database accesses including a CommitTransaction
        /// and so this object may be used to "store" the data and use the database connection
        /// for something else.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public GetLedgerInfo(int ALedgerNumber)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

//            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
//              IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ledgerNumber = ALedgerNumber;
            ledger = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// Property to read the value of the Revaluation account
        /// </summary>
        public string RevaluationAccount
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.ForexGainsLossesAccount;
            }
        }

        /// <summary>
        /// Property to read the value of the base currency
        /// </summary>
        public string BaseCurrency
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.BaseCurrency;
            }
        }

        /// <summary>
        /// Property to read the value of the ProvisionalYearEndFlag
        /// </summary>
        public bool ProvisionalYearEndFlag
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.ProvisionalYearEndFlag;
            }
        }


        public int CurrentPeriod
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.CurrentPeriod;
            }
        }
        public int NumberOfAccountingPeriods
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.NumberOfAccountingPeriods;
            }
        }
        public int CurrentFinancialYear
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.CurrentFinancialYear;
            }
        }

        public int LedgerNumber
        {
            get
            {
                ALedgerRow row = (ALedgerRow)ledger[0];
                return row.LedgerNumber;
            }
        }
    }

    // -----------------------------------------------------------------------------

    /// <summary>
    /// This is the list of vaild Ledger-Init-Flags
    /// (the TLedgerInitFlagHandler has an internal information of the flag)
    /// </summary>
    public enum LegerInitFlag
    {
        /// <summary>
        /// Revaluation is a process which has to be done once each month. So the value
        /// a) is set to true if a revaluation is done,
        /// b) set to false if the month end process was done successful.
        /// c) is used bevor the month end process to remember the user for the outstanding
        /// revaluation.
        /// </summary>
        Revaluation
    }

    /// <summary>
    /// LedgerInitFlag is a table wich holds a small set of "boolean" properties for each
    /// Ledger refered to the actual month.
    /// One example is the value that a Revaluation has been done in the actual month. Some other
    /// values will be added soon.
    /// </summary>
    public class TLedgerInitFlagHandler
    {
        private TVerificationResultCollection VerificationResult = null;
        private int intLedgerNumber;
        private string strFlagName;

        /// <summary>
        /// This Constructor only takes and stores the initial parameters. No
        /// Database request is done by this routine.
        /// </summary>
        /// <param name="ALedgerNumber">A valid ledger number</param>
        /// <param name="AFlagNum">A valid LegerInitFlag entry</param>
        public TLedgerInitFlagHandler(int ALedgerNumber, LegerInitFlag AFlagNum)
        {
            intLedgerNumber = ALedgerNumber;
            strFlagName = String.Empty;

            if (AFlagNum == LegerInitFlag.Revaluation)
            {
                strFlagName = "REVAL";
            }

            if (strFlagName.Equals(String.Empty))
            {
                throw new ApplicationException("Please define a value for the selected enum");
            }
        }

        /// <summary>
        /// The flag property controls all databse requests.
        /// </summary>
        public bool Flag
        {
            get
            {
                return FindRecord();
            }
            set
            {
                if (FindRecord())
                {
                    if (!value)
                    {
                        DeleteRecord();
                    }
                }
                else
                {
                    if (value)
                    {
                        CreateRecord();
                    }
                }
            }
        }

        private bool FindRecord()
        {
            bool boolValue;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);

            boolValue = (aLedgerInitFlagTable.Rows.Count == 1);
            DBAccess.GDBAccessObj.CommitTransaction();
            HandleVerificationResuls();
            return boolValue;
        }

        private void CreateRecord()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);
            ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();

            aLedgerInitFlagRow.LedgerNumber = intLedgerNumber;
            aLedgerInitFlagRow.InitOptionName = strFlagName;
            aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
            ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);
            DBAccess.GDBAccessObj.CommitTransaction();
            HandleVerificationResuls();
        }

        private void DeleteRecord()
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLedgerNumber, strFlagName, Transaction);

            if (aLedgerInitFlagTable.Rows.Count == 1)
            {
                ((ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0]).Delete();
                ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out VerificationResult);
            }

            DBAccess.GDBAccessObj.CommitTransaction();
            HandleVerificationResuls();
        }

        private void HandleVerificationResuls()
        {
            if (VerificationResult != null)
            {
                if (VerificationResult.HasCriticalError())
                {
                    throw new ApplicationException("TLedgerInitFlagHandler does not work");
                }
            }
        }
    }

    public class TerminateException : SystemException
    {
        public TerminateException() : base()
        {
        }
    }

    public class InternalExceptionStati
    {
        public static string INTERNALS = "INTERNALS";
    }
    /// <summary>
    /// This exception shall handle the internal errors of type critcal.
    /// </summary>
    public class InternalException : SystemException
    {
        string strErrorCode;

        public InternalException(Exception innerException, string message)
            : base(string.Empty, innerException)
        {
            strErrorCode = InternalExceptionStati.INTERNALS;
        }

        public InternalException(Exception innerException, string errorCode, string message)
            : base(message, innerException)
        {
            strErrorCode = errorCode;
        }

        public InternalException(string errorCode, string message)
            : base(message)
        {
            strErrorCode = errorCode;
        }

        public string ErrorCode
        {
            get
            {
                return strErrorCode;
            }
        }
    }


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

    public class GetCurrencyInfo
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
        public GetCurrencyInfo(string ACurrencyCode)
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
        public GetCurrencyInfo(string ABaseCurrencyCode, string AForeignCurrencyCode)
        {
            LoadDatabase();
            baseCurrencyRow = SetRowToCode(ABaseCurrencyCode);
            foreignCurrencyRow = SetRowToCode(AForeignCurrencyCode);
        }

        private void LoadDatabase()
        {
            intBaseCurrencyDigits = DIGIT_INIT_VALUE;
            intForeignCurrencyDigits = DIGIT_INIT_VALUE;
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            currencyTable = ACurrencyAccess.LoadAll(transaction);
            DBAccess.GDBAccessObj.CommitTransaction();

            if (currencyTable.Rows.Count == 0)
            {
                throw new InternalException("GetCurrencyInfo.01",
                    Catalog.GetString(
                        "The table a_currency is empty!"));
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

            throw new InternalException("GetCurrencyInfo.02",
                Catalog.GetString(String.Format(
                        "No Data for curency {0} found", ACurrencyCode)));
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
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                throw new InternalException("GetCurrencyInfo.03",
                    String.Format("The regular expression {0} does not fit for a match in {1}",
                        sRegex, strFormat));
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
}