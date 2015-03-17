//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// This routine reads the line of a_ledger defined by the ledger number
    /// </summary>
    public class TLedgerInfo
    {
        int ledgerNumber;

        //
        // Several utilities may each have their own TLedgerInfo object, so several objects can be created for the same ledger.
        // This static DataTable attempts to ensure that they all see the same view of the world.

        static private ALedgerTable FLedgerTbl = null;
        DataView MyView = null;
        ALedgerRow FLedgerRow;


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
            GetDataRow();
        }

        private void GetDataRow()
        {
            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadUncommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    FLedgerTbl = ALedgerAccess.LoadAll(Transaction); // FLedgerTbl is static - this refreshes *any and all* TLedgerInfo objects.
                    MyView = new DataView(FLedgerTbl);
                    MyView.RowFilter = "a_ledger_number_i = " + ledgerNumber;
                    FLedgerRow = (ALedgerRow)MyView[0].Row; // More than one TLedgerInfo object may point to this same row.
                });
        }

        private void CommitLedgerChange()
        {
            TDBTransaction transaction = null;
            Boolean SubmissionOk = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref transaction, ref SubmissionOk,
                delegate
                {
                    ALedgerAccess.SubmitChanges(FLedgerTbl, transaction);
                });
            FLedgerTbl.AcceptChanges();
            GetDataRow();
        }

        /// <summary>
        /// Get the name for this Ledger
        /// </summary>
        public static string GetLedgerName(int ledgernumber)
        {
            String ReturnValue = "";
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    String strSql = "SELECT p_partner_short_name_c FROM PUB_a_ledger, PUB_p_partner WHERE a_ledger_number_i=" +
                                    ledgernumber + " AND PUB_a_ledger.p_partner_key_n = PUB_p_partner.p_partner_key_n";
                    DataTable tab = DBAccess.GDBAccessObj.SelectDT(strSql, "GetLedgerName_TempTable", ReadTransaction);

                    if (tab.Rows.Count > 0)
                    {
                        ReturnValue = Convert.ToString(tab.Rows[0]["p_partner_short_name_c"]);
                    }
                });

            return ReturnValue;
        }

        /// <summary>
        /// Property to read the value of the Revaluation account
        /// </summary>
        public string RevaluationAccount
        {
            get
            {
                return FLedgerRow.ForexGainsLossesAccount;
            }
        }

        /// <summary>
        /// Property to read the value of the base currency
        /// </summary>
        public string BaseCurrency
        {
            get
            {
                return FLedgerRow.BaseCurrency;
            }
        }

        /// <summary>
        /// Property to read the value of the International currency
        /// </summary>
        public string InternationalCurrency
        {
            get
            {
                return FLedgerRow.IntlCurrency;
            }
        }

        /// <summary>
        /// Read or write the ProvisionalYearEndFlag
        /// </summary>
        public bool ProvisionalYearEndFlag
        {
            get
            {
                GetDataRow();
                return FLedgerRow.ProvisionalYearEndFlag;
            }
            set
            {
                GetDataRow();
                FLedgerRow.ProvisionalYearEndFlag = value;
                CommitLedgerChange();
            }
        }

        /// <summary>
        /// Read or write the YearEndFlag
        /// </summary>
        public bool YearEndFlag
        {
            get
            {
                GetDataRow();
                return FLedgerRow.YearEndFlag;
            }
            set
            {
                GetDataRow();
                FLedgerRow.YearEndFlag = value;
                CommitLedgerChange();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int CurrentPeriod
        {
            get
            {
                GetDataRow();
                return FLedgerRow.CurrentPeriod;
            }
            set
            {
                GetDataRow();
                FLedgerRow.CurrentPeriod = value;
                CommitLedgerChange();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberOfAccountingPeriods
        {
            get
            {
                GetDataRow();
                return FLedgerRow.NumberOfAccountingPeriods;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int NumberFwdPostingPeriods
        {
            get
            {
                GetDataRow();
                return FLedgerRow.NumberFwdPostingPeriods;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int CurrentFinancialYear
        {
            get
            {
                GetDataRow();
                return FLedgerRow.CurrentFinancialYear;
            }
            set
            {
                GetDataRow();
                FLedgerRow.CurrentFinancialYear = value;
                CommitLedgerChange();
            }
        }


        /// <summary>
        ///
        /// </summary>
        public int LedgerNumber
        {
            get
            {
                return FLedgerRow.LedgerNumber;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int YearEndProcessStatus
        {
            get
            {
                return FLedgerRow.YearEndProcessStatus;
            }
            set
            {
                GetDataRow();
                FLedgerRow.YearEndProcessStatus = value;
                CommitLedgerChange();
            }
        }


        /// <summary>
        ///
        /// </summary>
        public bool IltAccountFlag
        {
            get
            {
                return FLedgerRow.IltAccountFlag;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool BranchProcessing
        {
            get
            {
                return FLedgerRow.BranchProcessing;
            }
        }


        /// <summary>
        ///
        /// </summary>
        public bool IltProcessingCentre
        {
            get
            {
                return FLedgerRow.IltProcessingCentre;
            }
        }

        /// <summary>
        /// return standard cost centre to be used for given ledger number
        /// </summary>
        public string GetStandardCostCentre()
        {
            return GetStandardCostCentre(LedgerNumber);
        }

        /// <summary>
        /// return standard cost centre to be used for given ledger number
        /// </summary>
        public static string GetStandardCostCentre(int ALedgerNumber)
        {
            return String.Format("{0:##00}00", ALedgerNumber);
        }

        /// <summary>
        /// get the default bank account for this ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public static string GetDefaultBankAccount(int ALedgerNumber)
        {
            string BankAccountCode = TSystemDefaultsCache.GSystemDefaultsCache.GetStringDefault(
                SharedConstants.SYSDEFAULT_GIFTBANKACCOUNT + ALedgerNumber.ToString());

            if (BankAccountCode.Length == 0)
            {
                TDBTransaction Transaction = null;

                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        // use the first bank account
                        AAccountPropertyTable accountProperties = AAccountPropertyAccess.LoadViaALedger(ALedgerNumber, Transaction);
                        accountProperties.DefaultView.RowFilter = AAccountPropertyTable.GetPropertyCodeDBName() + " = '" +
                                                                  MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT + "' and " +
                                                                  AAccountPropertyTable.GetPropertyValueDBName() + " = 'true'";

                        if (accountProperties.DefaultView.Count > 0)
                        {
                            BankAccountCode = ((AAccountPropertyRow)accountProperties.DefaultView[0].Row).AccountCode;
                        }
                        else
                        {
                            string SQLQuery = "SELECT a_gift_batch.a_bank_account_code_c " +
                                              "FROM a_gift_batch " +
                                              "WHERE a_gift_batch.a_ledger_number_i = " + ALedgerNumber +
                                              " AND a_gift_batch.a_batch_number_i = (" +
                                              "SELECT max(a_gift_batch.a_batch_number_i) " +
                                              "FROM a_gift_batch " +
                                              "WHERE a_gift_batch.a_ledger_number_i = " + ALedgerNumber +
                                              " AND a_gift_batch.a_gift_type_c = '" + MFinanceConstants.GIFT_TYPE_GIFT + "')";

                            DataTable LatestAccountCode = DBAccess.GDBAccessObj.SelectDT(SQLQuery, "LatestAccountCode", Transaction);

                            // use the Bank Account of the previous Gift Batch
                            if ((LatestAccountCode != null) && (LatestAccountCode.Rows.Count > 0))
                            {
                                BankAccountCode = LatestAccountCode.Rows[0]["a_bank_account_code_c"].ToString();
                            }
                            // if this is the first ever gift batch (this should happen only once!) then use the first appropriate Account Code in the database
                            else
                            {
                                AAccountTable AccountTable = AAccountAccess.LoadViaALedger(ALedgerNumber, Transaction);

                                DataView dv = AccountTable.DefaultView;
                                dv.Sort = "a_account_code_c asc";
                                dv.RowFilter = "a_account_active_flag_l = true AND a_posting_status_l = true";
                                DataTable sortedDT = dv.ToTable();

                                TLedgerInfo ledgerInfo = new TLedgerInfo(ALedgerNumber);
                                TGetAccountHierarchyDetailInfo accountHierarchyTools = new TGetAccountHierarchyDetailInfo(ledgerInfo);
                                List <string>children = accountHierarchyTools.GetChildren(MFinanceConstants.CASH_ACCT);

                                foreach (DataRow account in sortedDT.Rows)
                                {
                                    // check if this account reports to the CASH account
                                    if (children.Contains(account["a_account_code_c"].ToString()))
                                    {
                                        BankAccountCode = account["a_account_code_c"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    });
            }

            return BankAccountCode;
        }
    }

    /// <summary>
    /// This is the list of valid Ledger-Init-Flags
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
        ///
        /// </summary>
        DatabaseAllocation
    }

    /// <summary>
    /// LedgerInitFlag is a table wich holds a small set of "boolean" properties for each
    /// Ledger related to the actual month.
    /// One example is the value that a Revaluation has been done in the actual month.
    /// </summary>
    public class TLedgerInitFlagHandler
    {
        private int FLedgerNumber;
        private string strFlagName;
        private string strFlagNameHelp;

        /// <summary>
        /// This Constructor only takes and stores the initial parameters.
        /// No Database request is done by this routine.
        /// </summary>
        /// <param name="ALedgerNumber">A valid ledger number</param>
        /// <param name="AFlagEnum">A valid LegerInitFlag entry</param>
        public TLedgerInitFlagHandler(int ALedgerNumber, TLedgerInitFlagEnum AFlagEnum)
        {
            FLedgerNumber = ALedgerNumber;
            strFlagName = String.Empty;

            if (AFlagEnum.Equals(TLedgerInitFlagEnum.Revaluation))
            {
                strFlagName = "REVALUATION-RUN";
            }
            else
            {
                strFlagName = AFlagEnum.ToString();
            }

            strFlagNameHelp = strFlagName;
        }

        /// <summary>
        ///
        /// </summary>
        public void AddMarker(string AMarker)
        {
            strFlagName = strFlagNameHelp + ":" + AMarker;
        }

        /// <summary>
        /// The Flag property controls all database requests.
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


        /// <summary>
        ///
        /// </summary>
        public void SetFlagAndName(string AName)
        {
            TDBTransaction transaction = null;
            Boolean SubmissionOk = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref transaction, ref SubmissionOk,
                delegate
                {
                    ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                        FLedgerNumber, strFlagName, transaction);

                    ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();

                    aLedgerInitFlagRow.LedgerNumber = FLedgerNumber;
                    aLedgerInitFlagRow.InitOptionName = strFlagName;

                    aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);

                    ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, transaction);
                });
        }

        private bool FindRecord()
        {
            bool boolValue;
            bool NewTransaction;
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                    FLedgerNumber, strFlagName, ReadTransaction);

                boolValue = (aLedgerInitFlagTable.Rows.Count == 1);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured in FindRecord:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }

            return boolValue;
        }

        private void CreateRecord()
        {
            bool NewTransaction;
            TDBTransaction ReadWriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out NewTransaction);

            try
            {
                ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                    FLedgerNumber, strFlagName, ReadWriteTransaction);

                ALedgerInitFlagRow aLedgerInitFlagRow = (ALedgerInitFlagRow)aLedgerInitFlagTable.NewRow();
                aLedgerInitFlagRow.LedgerNumber = FLedgerNumber;
                aLedgerInitFlagRow.InitOptionName = strFlagName;
                aLedgerInitFlagTable.Rows.Add(aLedgerInitFlagRow);

                ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, ReadWriteTransaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured in CreateRecord:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        private void DeleteRecord()
        {
            TDBTransaction transaction = null;
            Boolean SubmissionOK = true;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum,
                ref transaction, ref SubmissionOK,
                delegate
                {
                    ALedgerInitFlagTable aLedgerInitFlagTable = ALedgerInitFlagAccess.LoadByPrimaryKey(
                        FLedgerNumber, strFlagName, transaction);

                    if (aLedgerInitFlagTable.Rows.Count == 1)
                    {
                        ((ALedgerInitFlagRow)aLedgerInitFlagTable.Rows[0]).Delete();

                        ALedgerInitFlagAccess.SubmitChanges(aLedgerInitFlagTable, transaction);
                    }
                });
        }
    }
}