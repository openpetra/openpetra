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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


namespace Ict.Petra.Server.MFinance.GL
{
    public class THandleGlmpInfo
    {
        AGeneralLedgerMasterPeriodTable aGLMp;
        AGeneralLedgerMasterPeriodRow aGLMpRow;

        public THandleGlmpInfo()
        {
            LoadAll();
            aGLMpRow = null;
        }

        public THandleGlmpInfo(int ASequence, int APeriod)
        {
            LoadAll();
            SetToRow(ASequence, APeriod);
        }

        private void LoadAll()
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                aGLMp = AGeneralLedgerMasterPeriodAccess.LoadAll(transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        public bool SetToRow(int ASequence, int APeriod)
        {
            if (aGLMp.Rows.Count > 0)
            {
                for (int i = 0; i < aGLMp.Rows.Count; ++i)
                {
                    aGLMpRow = aGLMp[i];

                    if (aGLMpRow.GlmSequence == ASequence)
                    {
                        if (aGLMpRow.PeriodNumber == APeriod)
                        {
                            return true;
                        }
                    }
                }
            }

            aGLMpRow = null;
            return false;
        }

        public decimal ActualBase
        {
            get
            {
                return aGLMpRow.ActualBase;
            }
        }
    }


    /// <summary>
    /// Object to handle the read only glm-infos ...
    /// </summary>
    public class TGet_GLM_Info
    {
        DataTable aGLM;
        int iPtr = 0;

        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, int ACurrentFinancialYear)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[1].Value = AAccountCode;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ACurrentFinancialYear;

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetYearDBName() + " = ? ";
            aGLM = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, string ACostCentreCode)
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
                    return (decimal)aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetYtdActualBaseDBName()];
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
                    return (decimal)aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetYtdActualForeignDBName()];
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
        public int Sequence
        {
            get
            {
                return (int)aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetGlmSequenceDBName()];
            }
        }
        public string CostCentreCode
        {
            get
            {
                return (string)aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetCostCentreCodeDBName()];
            }
        }
    }

    public class THandleGlmInfo
    {
        DataTable glmTable;
        DataRow glmRow;
        int iPtr;

        public THandleGlmInfo(int ALedgerNumber, int AYear, string AAccountCode)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = AYear;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = AAccountCode;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
                strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
                strSQL += "AND " + AGeneralLedgerMasterTable.GetYearDBName() + " = ? ";
                strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
                glmTable = DBAccess.GDBAccessObj.SelectDT(
                    strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        public void Reset()
        {
            iPtr = -1;
        }

        public bool MoveNext()
        {
            ++iPtr;
            try
            {
                glmRow = glmTable.Rows[iPtr];
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public string AccountCode
        {
            get
            {
                return (string)glmRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()];
            }
        }
        public string CostCentreCode
        {
            get
            {
                return (string)glmRow[AGeneralLedgerMasterTable.GetCostCentreCodeDBName()];
            }
        }

        public int GlmSequence
        {
            get
            {
                return (int)glmRow[AGeneralLedgerMasterTable.GetGlmSequenceDBName()];
            }
        }
        public decimal YtdActualBase
        {
            get
            {
                return (decimal)glmRow[AGeneralLedgerMasterTable.GetYtdActualBaseDBName()];
            }
        }
    }


    /// <summary>
    /// Object to handle the cost center table ...
    /// </summary>
    public class THandleCostCenterInfo
    {
        ACostCentreTable costCentreTable;
        ACostCentreRow costCentreRow;
        bool blnCostCenterValid;

        /// <summary>
        /// Constructor to select ledger and cost center directly
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACostCenterCode"></param>
        public THandleCostCenterInfo(int ALedgerNumber, string ACostCenterCode)
        {
            THandleCostCenterInfo_(ALedgerNumber);
            blnCostCenterValid = SetCostCenterRow_(ACostCenterCode);
        }

        /// <summary>
        /// This constructor only loads a cc list. The row remains invalid
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public THandleCostCenterInfo(int ALedgerNumber)
        {
            THandleCostCenterInfo_(ALedgerNumber);
            blnCostCenterValid = false;
        }

        private void THandleCostCenterInfo_(int ALedgerNumber)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();

            costCentreTable = ACostCentreAccess.LoadViaALedger(ALedgerNumber, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// Select an other data row ...
        /// </summary>
        /// <param name="ACostCenterCode"></param>
        public void SetCostCenterRow(string ACostCenterCode)
        {
            blnCostCenterValid = SetCostCenterRow_(ACostCenterCode);
        }

        private bool SetCostCenterRow_(string ACostCenterCode)
        {
            System.Diagnostics.Debug.WriteLine("CCTR" + costCentreTable.Rows.Count);

            if (costCentreTable.Rows.Count > 0)
            {
                for (int i = 0; i < costCentreTable.Rows.Count; ++i)
                {
                    costCentreRow = (ACostCentreRow)costCentreTable[i];

                    if (costCentreRow.CostCentreCode.Equals(ACostCenterCode))
                    {
                        System.Diagnostics.Debug.WriteLine("found");
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True means that the row the object holds a valid data row.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return blnCostCenterValid;
            }
        }

        public bool PostingCostCentreFlag
        {
            get
            {
                return costCentreRow.PostingCostCentreFlag;
            }
        }
        public string CostCentreCode
        {
            get
            {
                return costCentreRow.CostCentreCode;
            }
        }
    }

    /// <summary>
    /// This object handles the table AccountHierarchyDetailInfo and provides some standard
    /// procedures.
    /// </summary>
    public class TGetAccountHierarchyDetailInfo
    {
        /// <summary>
        /// A AChildLevel value which defines to serach the childs and all subchilds of a
        /// given parent.
        /// </summary>
        public const int GET_ALL_LEVELS = -1;

        private const string STANDARD = "STANDARD";
        AAccountHierarchyDetailTable accountTable;
        AAccountHierarchyDetailRow accountRow = null;
        TLedgerInfo ledgerInfo;

        int iPtr;

        public TGetAccountHierarchyDetailInfo(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
            accountTable = AAccountHierarchyDetailAccess.LoadViaALedger(
                ledgerInfo.LedgerNumber, transaction);
            DBAccess.GDBAccessObj.CommitTransaction();
        }

        /// <summary>
        /// The idea of this routine is given by x_clist.i
        /// of course the data are read only one time (Constructor) and the results are put
        /// into a list for a more specific use.
        /// All childs and sub childs are listed ...
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public IList <String>ChildList(string AAccountCode)
        {
            IList <String>help = new List <String>();
            ChildListIntern(help, AAccountCode, GET_ALL_LEVELS);
            return help;
        }

        /// <summary>
        /// See ChildList definition without AChildLevel, here you can define your own
        /// ChildLevel.
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <param name="AChildLevel">Level counting starts with 1</param>
        /// <returns></returns>
        public IList <String>ChildList(string AAccountCode, int AChildLevel)
        {
            IList <String>help = new List <String>();
            ChildListIntern(help, AAccountCode, --AChildLevel);
            return help;
        }

        private void ChildListIntern(IList <String>help, string AAccountCode, int AChildLevel)
        {
            if (accountTable.Rows.Count > 0)
            {
                for (int i = 0; i < accountTable.Rows.Count; ++i)
                {
                    accountRow = (AAccountHierarchyDetailRow)accountTable.Rows[i];

                    if (accountRow.AccountCodeToReportTo.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            help.Add(accountRow.ReportingAccountCode);

                            if (AChildLevel != 0)
                            {
                                ChildListIntern(help, accountRow.ReportingAccountCode, --AChildLevel);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If you only want to know, that the account code has no childs, you can avoid the
        /// handling of the list.
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public bool HasNoChilds(string AAccountCode)
        {
            if (accountTable.Rows.Count > 0)
            {
                for (int i = 0; i < accountTable.Rows.Count; ++i)
                {
                    accountRow = (AAccountHierarchyDetailRow)accountTable.Rows[i];

                    if (accountRow.AccountCodeToReportTo.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the parent account of an inserted account ...
        /// </summary>
        /// <param name="AAccountCode"></param>
        /// <returns></returns>
        public string GetParentAccount(string AAccountCode)
        {
            if (accountTable.Rows.Count > 0)
            {
                for (int i = 0; i < accountTable.Rows.Count; ++i)
                {
                    accountRow = (AAccountHierarchyDetailRow)accountTable.Rows[i];

                    if (accountRow.ReportingAccountCode.Equals(AAccountCode))
                    {
                        if (accountRow.AccountHierarchyCode.Equals(STANDARD))
                        {
                            return accountRow.AccountCodeToReportTo;
                        }
                    }
                }
            }

            return String.Empty;
        }
    }

    /// <summary>
    /// A Enum-list of some special account codes ...
    /// </summary>
    public enum TAccountPropertyEnum
    {
        GIFT_HEADING,               // GIFT
        INTER_LEDGER_HEADING,       // ILT
        BANK_HEADING,               // CASH
        BALANCE_SHEET_HEADING,      // BAL

        /// <summary>
        /// See: https://sourceforge.net/apps/phpbb/openpetraorg/viewtopic.php?f=14&t=117&start=0
        /// And
        /// https://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Data_Conversion_from_Petra_to_Openpetra
        /// </summary>
        //PROFIT_AND_LOSS_HEADING,    // PL
        //INCOME_HEADING,             // INC
        //EXPENSE_HEADING,            // EXP
        DEBTOR_HEADING,             // DRS
        CREDITOR_HEADING,           // CRS
        TOTAL_ASSET_HEADING,        // ASSETS
        TOTAL_LIABILITY_HEADING,    // LIABS
        EQUITY_HEADING,             // RET EARN

        EARNINGS_BF_ACCT,           // 9700
        DIRECT_XFER_ACCT,           // 5501
        ICH_SETTLEMENT_ACCT,        // 5601
        ICH_ACCT,                   // 8500
        INTERNAL_XFER_ACCT,         // 9800
        ADMIN_FEE_INCOME_ACCT,      // 3400
        ADMIN_FEE_EXPENSE_ACCT,     // 4900
        FUND_TRANSFER_INCOME_ACCT,  // 3300
        FUND_TRANSFER_EXPENSE_ACCT, // 4800
    }

    /// <summary>
    /// A handler to the special accounts in TAccountPropertyEnum
    /// </summary>
    public class THandleAccountPropertyInfo
    {
        AAccountPropertyTable propertyCodeTable;
        /// <summary>
        /// The constructor needs a lederinfo (for the ledger number)
        /// </summary>
        /// <param name="ledgerInfo"></param>
        public THandleAccountPropertyInfo(TLedgerInfo ledgerInfo)
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                propertyCodeTable = AAccountPropertyAccess.LoadViaALedger(
                    ledgerInfo.LedgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        /// <summary>
        /// Get access on a special account ...
        /// </summary>
        /// <param name="AEnum"></param>
        /// <returns></returns>
        public string GetAccountCode(TAccountPropertyEnum AEnum)
        {
            try
            {
                AAccountPropertyRow row;

                for (int i = 0; i < propertyCodeTable.Rows.Count; ++i)
                {
                    row = (AAccountPropertyRow)propertyCodeTable[i];

                    if (row.PropertyCode.Equals("Is_Special_Account"))
                    {
                        if (row.PropertyValue.Equals(AEnum.ToString()))
                        {
                            return row.AccountCode;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }

        public string GetAccountCode(string APropertyCode)
        {
            try
            {
                AAccountPropertyRow row;

                for (int i = 0; i < propertyCodeTable.Rows.Count; ++i)
                {
                    row = (AAccountPropertyRow)propertyCodeTable[i];

                    if (row.PropertyCode.Equals(APropertyCode))
                    {
                        return row.PropertyValue;
                    }
                }
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }
    }


    /// <summary>
    /// THandleAccountInfo uses a TLedgerInfo a primilary references the LedgerNumber.
    /// All Accounts are load in both contructors. You can define an inital account code in the
    /// second constructor or you can set the value later (or change) by using SetAccountRowTo.
    /// Then you can read the values for the selected Account.
    /// </summary>
    public class THandleAccountInfo
    {
        AAccountTable accountTable;
        AAccountRow accountRow = null;
        TLedgerInfo ledgerInfo;
        THandleAccountPropertyInfo tAccountPropertyHandler = null;

        int iPtr;

        /// <summary>
        /// This mininmal constructor defines the result collection for the error messages and
        /// Ledger Info to select the ledger ...
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        public THandleAccountInfo(TLedgerInfo ALedgerInfo)
        {
            ledgerInfo = ALedgerInfo;
            LoadData();
        }

        /// <summary>
        /// The Constructor defines a first value of a specific accounting code too.
        /// </summary>
        /// <param name="ALedgerInfo"></param>
        /// <param name="AAccountCode"></param>
        public THandleAccountInfo(TLedgerInfo ALedgerInfo, string AAccountCode)
        {
            ledgerInfo = ALedgerInfo;
            LoadData();
            AccountCode = AAccountCode;
        }

        private void LoadData()
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                accountTable = AAccountAccess.LoadViaALedger(
                    ledgerInfo.LedgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
            accountRow = null;
        }

        public void SetSpecialAccountCode(TAccountPropertyEnum AENum)
        {
            accountRow = null;

            if (tAccountPropertyHandler == null)
            {
                tAccountPropertyHandler = new THandleAccountPropertyInfo(ledgerInfo);
            }

            string account = tAccountPropertyHandler.GetAccountCode(AENum);

            if (!account.Equals(string.Empty))
            {
                AccountCode = account;
            }
        }

        public string SetCarryForwardAccount()
        {
            accountRow = null;

            if (tAccountPropertyHandler == null)
            {
                tAccountPropertyHandler = new THandleAccountPropertyInfo(ledgerInfo);
            }

            string result = tAccountPropertyHandler.GetAccountCode("CARRYFORWARDCC");

            if (!result.Equals(string.Empty))
            {
                string[] arrStrHelp = result.Split(new Char[] { ',' });
                AccountCode = arrStrHelp[0];
                return arrStrHelp[1];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// The Account code can be read and the result is the account code of the row
        /// which was selected before. </br>
        /// The Account can be written and this will change the selected row without any
        /// database request. The result may be invalid.
        /// </summary>
        public string AccountCode
        {
            get
            {
                return accountRow.AccountCode;
            }
            set
            {
                if (value.Equals(String.Empty))
                {
                    accountRow = null;
                }
                else
                {
                    accountRow = null;

                    for (int i = 0; i < accountTable.Rows.Count; ++i)
                    {
                        if (value.Equals(((AAccountRow)accountTable[i]).AccountCode))
                        {
                            accountRow = (AAccountRow)accountTable[i];
                        }
                    }
                }
            }
        }

        public string AccountType
        {
            get
            {
                return accountRow.AccountType;
            }
        }

        public void Reset()
        {
            iPtr = -1;
        }

        public bool MoveNext()
        {
            ++iPtr;

            if (iPtr < accountTable.Rows.Count)
            {
                accountRow = (AAccountRow)accountTable[iPtr];
                return true;
            }
            else
            {
                accountRow = null;
                return false;
            }
        }

        /// <summary>
        /// Informs that a row selection is valid and the row properties will not produce
        /// an exception.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !(accountRow == null);
            }
        }

        /// <summary>
        /// Standard property of an AAccountRow
        /// </summary>
        public bool ForeignCurrencyFlag
        {
            get
            {
                return accountRow.ForeignCurrencyFlag;
            }
        }

        /// <summary>
        /// Standard property of an AAccountRow
        /// </summary>
        public string ForeignCurrencyCode
        {
            get
            {
                return accountRow.ForeignCurrencyCode;
            }
        }

        public bool PostingStatus
        {
            get
            {
                return accountRow.PostingStatus;
            }
        }
        public bool DebitCreditIndicator
        {
            get
            {
                return accountRow.DebitCreditIndicator;
            }
        }
    }

    /// <summary>
    /// Gets the specific date informations of an accounting intervall. This routine is either used by
    /// GL.PeriodEnd.Month and GL.Revaluation but in different senses. On time the dataset holds exact
    /// one row (Contructor with two parameters) and on time it holds a set of rows (Constructor with
    /// one parameter.
    /// </summary>
    public class TGetAccountingPeriodInfo
    {
        private AAccountingPeriodTable periodTable = null;

        /// <summary>
        /// Constructor needs a valid ledger number.
        /// </summary>
        /// <param name="ALedgerNumber">Ledger number</param>
        public TGetAccountingPeriodInfo(int ALedgerNumber)
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

        public TGetAccountingPeriodInfo(int ALedgerNumber, int ACurrentPeriod)
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
        /// Reads the value of the first and hopefully only row.
        /// </summary>
        public DateTime PeriodEndDate
        {
            get
            {
                AAccountingPeriodRow periodRow = (AAccountingPeriodRow)periodTable[0];
                return periodRow.PeriodEndDate;
            }
        }


        /// <summary>
        /// Reads the end date of the period
        /// </summary>
        /// <param name="APeriodNum">The number of the period. DateTime.MinValue is an
        /// error value.</param>
        /// <returns></returns>
        public DateTime GetPeriodEndDate(int APeriodNum)
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
        public DateTime GetPeriodStartDate(int APeriodNum)
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

    public enum TAccountTypeEnum
    {
        Income,
        Expense,
        Asset,
        Equity,
        Liability
    }

    /// <summary>
    /// This routine reads the line of a_ledger defined by the ledger number
    /// </summary>
    public class TLedgerInfo
    {
        int ledgerNumber;
        private ALedgerTable ledger = null;
        ALedgerRow row;

        /// <summary>
        /// Constructor to address the correct table line (relevant ledger number). The
        /// constructor only will run the database accesses including a CommitTransaction
        /// and so this object may be used to "store" the data and use the database connection
        /// for something else.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TLedgerInfo(int ALedgerNumber)
        {
            ledgerNumber = ALedgerNumber;
            LoadInfoLine();
        }

        private void LoadInfoLine()
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                ledger = ALedgerAccess.LoadByPrimaryKey(ledgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
                row = (ALedgerRow)ledger[0];
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
        }

        /// <summary>
        /// Property to read the value of the Revaluation account
        /// </summary>
        public string RevaluationAccount
        {
            get
            {
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
                return row.ProvisionalYearEndFlag;
            }
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
                LoadInfoLine();
            }
        }


        public int CurrentPeriod
        {
            get
            {
                return row.CurrentPeriod;
            }
        }
        public int NumberOfAccountingPeriods
        {
            get
            {
                return row.NumberOfAccountingPeriods;
            }
        }
        public int NumberFwdPostingPeriods
        {
            get
            {
                return row.NumberFwdPostingPeriods;
            }
        }
        public int CurrentFinancialYear
        {
            get
            {
                return row.CurrentFinancialYear;
            }
        }

        public int LedgerNumber
        {
            get
            {
                return row.LedgerNumber;
            }
        }

        public bool YearEndFlag
        {
            get
            {
                return row.YearEndFlag;
            }
        }
        public int TYearEndProcessStatus
        {
            get
            {
                return row.TYearEndProcessStatus;
            }
            set
            {
                OdbcParameter[] ParametersArray;
                ParametersArray = new OdbcParameter[2];
                ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[0].Value = value;
                ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
                ParametersArray[1].Value = ledgerNumber;

                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                string strSQL = "UPDATE PUB_" + ALedgerTable.GetTableDBName() + " ";
                strSQL += "SET " + ALedgerTable.GetYearEndProcessStatusDBName() + " = ? ";
                strSQL += "WHERE " + ALedgerTable.GetLedgerNumberDBName() + " = ? ";
                DBAccess.GDBAccessObj.ExecuteNonQuery(
                    strSQL, transaction, ParametersArray);
                DBAccess.GDBAccessObj.CommitTransaction();
                LoadInfoLine();
            }
        }

        public bool IltAccountFlag
        {
            get
            {
                return row.IltAccountFlag;
            }
        }
        public bool BranchProcessing
        {
            get
            {
                return row.BranchProcessing;
            }
        }

        public bool IltProcessingCentre
        {
            get
            {
                return row.IltProcessingCentre;
            }
        }
    }

    public class TLedgerLock
    {
        int intLegerNumber;
        private bool blnResult;
        private Object synchRoot = new Object();
        public TLedgerLock(int ALedgerNum)
        {
            intLegerNumber = ALedgerNum;
            TVerificationResultCollection tvr;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                intLegerNumber, TLedgerInitFlagEnum.LedgerLock.ToString(), Transaction);
            ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();
            aLedgerInitFlagRow.LedgerNumber = intLegerNumber;
            aLedgerInitFlagRow.InitOptionName = TLedgerInitFlagEnum.LedgerLock.ToString();
            lock (synchRoot) {
                try
                {
                    aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);
                    ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, Transaction, out tvr);
                    blnResult = true;
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                catch (System.Data.ConstraintException)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    blnResult = false;
                }
            }
        }

        public bool IsLocked
        {
            get
            {
                return blnResult;
            }
        }

        public void UnLock()
        {
            if (blnResult)
            {
                TLedgerInitFlagHandler tifh =
                    new TLedgerInitFlagHandler(intLegerNumber, TLedgerInitFlagEnum.LedgerLock);
                tifh.Flag = false;
            }
        }

        public string LockInfo()
        {
            if (!blnResult)
            {
                try
                {
                    TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
                    ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                        intLegerNumber, TLedgerInitFlagEnum.LedgerLock.ToString(), Transaction);
                    ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0];
                    string strAnswer = aLedgerInitFlagRow.CreatedBy + " - " +
                                       DateTime.Parse(aLedgerInitFlagRow.DateCreated.ToString()).ToLongDateString();
                    DBAccess.GDBAccessObj.CommitTransaction();
                    return strAnswer;
                }
                catch (Exception)
                {
                    return Catalog.GetString("Free Again");
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }


    // -----------------------------------------------------------------------------

    /// <summary>
    /// This is the list of vaild Ledger-Init-Flags
    /// (the TLedgerInitFlagHandler has an internal information of the flag)
    /// </summary>
    public enum TLedgerInitFlagEnum
    {
        /// <summary>
        /// Revaluation is a process which has to be done once each month. So the value
        /// a) is set to true if a revaluation is done,
        /// b) set to false if the month end process was done successful.
        /// c) is used bevor the month end process to remember the user for the outstanding
        /// revaluation.
        /// </summary>
        Revaluation,

        /// <summary>
        /// Property to lock a ledger ...
        /// Not implemented in petra
        /// </summary>
        LedgerLock,

        DatabaseAllocation
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
        /// <param name="AFlagEnum">A valid LegerInitFlag entry</param>
        public TLedgerInitFlagHandler(int ALedgerNumber, TLedgerInitFlagEnum AFlagEnum)
        {
            intLedgerNumber = ALedgerNumber;
            strFlagName = String.Empty;

            if (AFlagEnum.Equals(TLedgerInitFlagEnum.Revaluation))
            {
                strFlagName = "REVALUATION-RUN";
            }
            else
            {
                strFlagName = AFlagEnum.ToString();
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

        public void SetFlagAndName(string AName)
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

    /// <summary>
    /// This exception transports the error message and if the reason was an other exception
    /// to the end of the routine. ResultCollection unpackst this data into a
    /// TVerificationResultCollection object, so that the user gets this message on the
    /// "normal" message box.
    /// </summary>
    public class TerminateException : SystemException
    {
        /// <summary>
        /// Constructor with inner exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public TerminateException(Exception innerException, string message)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner exception
        /// </summary>
        /// <param name="message"></param>
        public TerminateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Property to handle (transport) the error code
        /// </summary>
        public string ErrorCode = String.Empty;

        /// <summary>
        /// Property to handle (transport) the error context
        /// </summary>
        public string Context = String.Empty;

        /// <summary>
        /// A Method to transform the exception message(s) into a
        /// TVerificationResultCollection
        /// </summary>
        /// <returns></returns>
        public TVerificationResultCollection ResultCollection()
        {
            TVerificationResultCollection collection =
                new TVerificationResultCollection();
            TVerificationResult avrEntry;

            avrEntry = new TVerificationResult(this.Context,
                this.Message, "",
                this.ErrorCode,
                TResultSeverity.Resv_Critical);
            collection.Add(avrEntry);
            avrEntry = new TVerificationResult(Catalog.GetString("Exception has been thrown"),
                this.ToString(), "",
                this.ErrorCode,
                TResultSeverity.Resv_Critical);
            collection.Add(avrEntry);

            if (this.InnerException != null)
            {
                avrEntry = new TVerificationResult(Catalog.GetString("Inner Exception"),
                    this.InnerException.ToString(),
                    TResultSeverity.Resv_Critical);
                collection.Add(avrEntry);
            }

            return collection;
        }
    }
    
    /// <summary>
    /// Handling of the standard data base transactions
    /// </summary>
    public class TTransactionFunctions
    {
    	TDBTransaction transaction;
    	bool blnTransactionDefined;
    	
    	/// <summary>
    	/// A Constructor
    	/// </summary>
    	/// <param name="AMasterTransaction">The value my be null which means: Create an onw 
    	/// Transaction</param>
    	public TTransactionFunctions(TDBTransaction AMasterTransaction)
    	{
    		if (AMasterTransaction == null)
    		{
    			blnTransactionDefined = false;
    			transaction = DBAccess.GDBAccessObj.BeginTransaction();
    		} else 
    		{
    			blnTransactionDefined = true;
    			transaction = AMasterTransaction;
    		}    		
    	}
    	
    	/// <summary>
    	/// The result is a valid transaction 
    	/// </summary>
    	public TDBTransaction TransactionValue
    	{
    		get 
    		{
    			return transaction;
    		}
    	}
    	
    	/// <summary>
    	/// Runs a very specific "Commit"  <br />
    	/// a) if an internaly created transaction is used, a commit will be done  <br />
    	/// b) otherwise the commint will be done in the very last end ...
    	/// </summary>
    	public void CommitOrRollback()
    	{
        	if (!blnTransactionDefined) 
        	{
        		try 
        		{
        			DBAccess.GDBAccessObj.CommitTransaction();
        		} catch (Exception exception)
        		{
        			DBAccess.GDBAccessObj.RollbackTransaction();
        			throw exception;
        		}
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
            LoadDatabase(null);
            baseCurrencyRow = SetRowToCode(ACurrencyCode);
        }

        public TCurrencyInfo(string ACurrencyCode, TDBTransaction AMasterTransaction)
        {
            LoadDatabase(null);
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
            LoadDatabase(null);
            baseCurrencyRow = SetRowToCode(ABaseCurrencyCode);
            foreignCurrencyRow = SetRowToCode(AForeignCurrencyCode);
        }

        private void LoadDatabase(TDBTransaction AMasterTransaction)
        {
            intBaseCurrencyDigits = DIGIT_INIT_VALUE;
            intForeignCurrencyDigits = DIGIT_INIT_VALUE;

            TTransactionFunctions masterTransaction = new TTransactionFunctions(AMasterTransaction);
        	currencyTable = ACurrencyAccess.LoadAll(masterTransaction.TransactionValue);
        	masterTransaction.CommitOrRollback();

            if (currencyTable.Rows.Count == 0)
            {
                TerminateException terminate = new TerminateException(
                    Catalog.GetString("The table a_currency is empty!"));
                terminate.Context = "Common Accountig";
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

            TerminateException terminate = new TerminateException(
                Catalog.GetString(String.Format(
                        "No Data for curency {0} found", ACurrencyCode)));
            terminate.Context = "Common Accountig";
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
        public FormatConverter(string strFormat)
        {
            sRegex = ">9.(9)+|>9$";
            reg = new Regex(sRegex);
            matchCollection = reg.Matches(strFormat);

            if (matchCollection.Count != 1)
            {
                TerminateException terminate = new TerminateException(
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
    /// The THandleBudgetInfo was primilary written for the year end calculation(s).
    /// </summary>
    public class THandleBudgetInfo
    {
        TLedgerInfo tHandleLedgerInfo;
        ABudgetTable aBudgetTable;
        ABudgetRow aBudgetRow;

        List <THandleBudgetPeriodInfo>budgetPeriodInfoList;

        /// <summary>
        /// The constructor internally reads in all a_budget-Table entries which belong to the
        /// ledger
        /// </summary>
        /// <param name="ATLedgerInfo">For LedgerNumber only</param>
        public THandleBudgetInfo(TLedgerInfo ATLedgerInfo)
        {
            tHandleLedgerInfo = ATLedgerInfo;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                aBudgetTable = ABudgetAccess.LoadViaALedger(tHandleLedgerInfo.LedgerNumber, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }
            budgetPeriodInfoList = new List <THandleBudgetPeriodInfo>();
        }

        /// <summary>
        /// Preparation of the Close-Budget-Operation(s). The relevant THandleBudgetPeriodInfo
        /// data sets are sorted out and added to a list.
        /// </summary>
        public void ReadCloseBudgetListYearEnd()
        {
            if (aBudgetTable.Rows.Count > 0)
            {
                for (int iBgt = 0; iBgt < aBudgetTable.Rows.Count; ++iBgt)        // FOR EACH a_budget
                {
                    aBudgetRow = aBudgetTable[iBgt];
                    int iHelp = tHandleLedgerInfo.NumberOfAccountingPeriods +
                                tHandleLedgerInfo.NumberFwdPostingPeriods;

                    // iBgtPrd start with 1 because the first period of a year has the
                    // number 1
                    for (int iBgtPrd = 1; iBgtPrd < iHelp; ++iBgtPrd)
                    {
                        THandleBudgetPeriodInfo tHandleBudgetPeriodInfo =
                            new THandleBudgetPeriodInfo(aBudgetRow.BudgetSequence, iBgtPrd);

                        if (tHandleBudgetPeriodInfo.IsValid)
                        {
                            budgetPeriodInfoList.Add(tHandleBudgetPeriodInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Running the close procedure by using a Transation which is defined outside
        /// of the object. The transaction is required in order to create a
        /// complete task transaction for the complete year end.
        /// </summary>
        /// <param name="ATransaction"></param>
        public void CloseBudgetListYearEnd(TDBTransaction ATransaction)
        {
            if (budgetPeriodInfoList.Count > 0)
            {
                for (int i = 0; i < budgetPeriodInfoList.Count; ++i)
                {
                    budgetPeriodInfoList[i].ClosePeriodYearEnd(ATransaction);
                }
            }
        }
    }

    /// <summary>
    /// An object which mainly shall be used by THandleBudgetInfo.
    /// </summary>
    public class THandleBudgetPeriodInfo
    {
        ABudgetPeriodTable aBudgetPeriodTable;
        ABudgetPeriodRow aBudgetPeriodRow;

        /// <summary>
        /// One cudget period info record will be loaded - if exists
        /// </summary>
        /// <param name="ABudgetSequence">1st. Primary key parameter</param>
        /// <param name="ABudgetPeriod">2nd. Primary key parameter</param>
        public THandleBudgetPeriodInfo(int ABudgetSequence, int ABudgetPeriod)
        {
            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction();
                aBudgetPeriodTable = ABudgetPeriodAccess.LoadByPrimaryKey(
                    ABudgetSequence, ABudgetPeriod, transaction);
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw exception;
            }

            if (aBudgetPeriodTable.Rows.Count > 0)
            {
                aBudgetPeriodRow = aBudgetPeriodTable[0];
            }
        }

        /// <summary>
        /// Returns true if a record has been found
        /// </summary>
        public bool IsValid
        {
            get
            {
                return aBudgetPeriodRow != null;
            }
        }

        /// <summary>
        /// Runs a year end closing on the budget record
        /// </summary>
        /// <param name="ATransaction">A required transaction to synchronize with all
        /// other year end operations.</param>
        public void ClosePeriodYearEnd(TDBTransaction ATransaction)
        {
            aBudgetPeriodRow.BudgetLastYear = aBudgetPeriodRow.BudgetThisYear;
            aBudgetPeriodRow.BudgetThisYear = aBudgetPeriodRow.BudgetNextYear;
            aBudgetPeriodRow.BudgetNextYear = 0;

            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[5];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[0].Value = aBudgetPeriodRow.BudgetThisYear;;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[1].Value = aBudgetPeriodRow.BudgetNextYear;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Decimal);
            ParametersArray[2].Value = 0;
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = aBudgetPeriodRow.BudgetSequence;
            ParametersArray[4] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[4].Value = aBudgetPeriodRow.PeriodNumber;

            string strSQL = "UPDATE PUB_" + ABudgetPeriodTable.GetTableDBName() + " ";
            strSQL += "SET " + ABudgetPeriodTable.GetBudgetLastYearDBName() + " = ? ";
            strSQL += ", " + ABudgetPeriodTable.GetBudgetThisYearDBName() + " = ? ";
            strSQL += ", " + ABudgetPeriodTable.GetBudgetNextYearDBName() + " = ? ";
            strSQL += "WHERE " + ABudgetPeriodTable.GetBudgetSequenceDBName() + " = ? ";
            strSQL += "AND " + ABudgetPeriodTable.GetPeriodNumberDBName() + " = ? ";
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                strSQL, ATransaction, ParametersArray);
        }
    }    
}