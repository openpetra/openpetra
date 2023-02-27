//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2004-2023 by OM International
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
using System.Data.Odbc;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Session;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MFinance.BankImport.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.BankImport.Logic;

namespace Ict.Petra.Server.MFinance.BankImport.WebConnectors
{
    /// <summary>
    /// import a bank statement from a CSV file
    /// </summary>
    public class TBankImportWebConnector
    {
        /// store settings in s_system_defaults
        [RequireModulePermission("FINANCE-1")]
        public static bool SaveSettings(
            string ASeparator,
            string AFileEncoding,
            string ADateFormat,
            string ANumberFormat,
            string AColumnMeaning,
            string AStartAfterLine)
        {
            TDataBase db = DBAccess.Connect("BankImportSaveSettings");
            TSystemDefaults defaults = new TSystemDefaults(db);

            defaults.SetSystemDefault("BANKIMPORT_SEPARATOR", ASeparator, db);
            defaults.SetSystemDefault("BANKIMPORT_ENCODING", AFileEncoding, db);
            defaults.SetSystemDefault("BANKIMPORT_DATEFORMAT", ADateFormat, db);
            defaults.SetSystemDefault("BANKIMPORT_NUMBERFORMAT", ANumberFormat, db);
            defaults.SetSystemDefault("BANKIMPORT_COLUMNMEANING", AColumnMeaning, db);
            defaults.SetSystemDefault("BANKIMPORT_STARTAFTERLINE", AStartAfterLine, db);

            db.CloseDBConnection();

            return true;
        }

        /// retrieve settings from s_system_defaults
        [RequireModulePermission("FINANCE-1")]
        public static bool ReadSettings(
            out string ABankAccountCode,
            out string ASeparator,
            out string AFileEncoding,
            out string ADateFormat,
            out string ANumberFormat,
            out string AColumnMeaning,
            out string AStartAfterLine)
        {
            TDataBase db = DBAccess.Connect("BankImportReadSettings");
            TSystemDefaults defaults = new TSystemDefaults(db);

            ABankAccountCode = defaults.GetStringDefault("BANKIMPORT_BANKACCOUNTCODE", "6200");
            ASeparator = defaults.GetStringDefault("BANKIMPORT_SEPARATOR", ";");
            AFileEncoding = defaults.GetStringDefault("BANKIMPORT_ENCODING", "UTF-8");
            ADateFormat = defaults.GetStringDefault("BANKIMPORT_DATEFORMAT", "DMY");
            ANumberFormat = defaults.GetStringDefault("BANKIMPORT_NUMBERFORMAT", "European");
            AColumnMeaning = defaults.GetStringDefault("BANKIMPORT_COLUMNMEANING", "DateEffective,unused,Description,Amount,Currency");
            AStartAfterLine = defaults.GetStringDefault("BANKIMPORT_STARTAFTERLINE", "\"Buchungstag\";\"Wertstellungstag\";\"Verwendungszweck\";\"Umsatz\";\"Währung\"");

            db.CloseDBConnection();

            return true;
        }


        /// <summary>
        /// import the data of a CSV file
        /// </summary>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <param name="ABankStatementFilename"></param>
        /// <param name="ACSVContent"></param>
        /// <param name="ASeparator"></param>
        /// <param name="ADateFormat">DMY or MDY</param>
        /// <param name="ANumberFormat">European or American</param>
        /// <param name="AColumnMeaning"></param>
        /// <param name="AStartAfterLine">can be empty, otherwise only the lines after the line matching AStartAfterLine will be parsed</param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportFromCSVFile(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            string ABankStatementFilename,
            string ACSVContent,
            string ASeparator,
            string ADateFormat,
            string ANumberFormat,
            string AColumnMeaning,
            string AStartAfterLine,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            new TSystemDefaults().SetSystemDefault("BANKIMPORT_BANKACCOUNTCODE", ABankAccountCode);

            return TBankStatementImportCSV.ImportBankStatement(
                ALedgerNumber,
                ABankAccountCode,
                ABankStatementFilename,
                ACSVContent,
                ASeparator,
                ADateFormat,
                ANumberFormat,
                String.Empty,
                AColumnMeaning,
                AStartAfterLine,
                out AStatementKey,
                out AVerificationResult);
        }

        /// <summary>
        /// import the data of a MT940 file
        /// </summary>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <param name="ABankStatementFilename"></param>
        /// <param name="AMT940Content"></param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportFromMT940File(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            string ABankStatementFilename,
            string AMT940Content,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            new TSystemDefaults().SetSystemDefault("BANKIMPORT_BANKACCOUNTCODE", ABankAccountCode);

            return TBankStatementImportMT940.ImportFromFile(
                ALedgerNumber,
                ABankAccountCode,
                ABankStatementFilename,
                AMT940Content,
                false,
                out AStatementKey,
                out AVerificationResult);
        }

        /// <summary>
        /// import the data of a CAMT file
        /// </summary>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <param name="ABankStatementFilename"></param>
        /// <param name="ACAMTContent"></param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportFromCAMTFile(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            string ABankStatementFilename,
            string ACAMTContent,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            new TSystemDefaults().SetSystemDefault("BANKIMPORT_BANKACCOUNTCODE", ABankAccountCode);

            return TBankStatementImportCAMT.ImportFromFile(
                ALedgerNumber,
                ABankAccountCode,
                ABankStatementFilename,
                ACAMTContent,
                false,
                out AStatementKey,
                out AVerificationResult);
        }

        /// <summary>
        /// import the data of a zip file containing CAMT files
        /// </summary>
        /// <param name="ALedgerNumber">the current ledger number</param>
        /// <param name="ABankAccountCode">the bank account against which the statement should be stored</param>
        /// <param name="AZipFileContent"></param>
        /// <param name="AStatementKey">this returns the first key of a statement that was imported. depending on the implementation, several statements can be created from one file</param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportFromCAMTZIPFile(
            Int32 ALedgerNumber,
            string ABankAccountCode,
            byte[] AZipFileContent,
            out Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult)
        {
            new TSystemDefaults().SetSystemDefault("BANKIMPORT_BANKACCOUNTCODE", ABankAccountCode);

            return TBankStatementImportCAMT.ImportFromZipFile(
                ALedgerNumber,
                ABankAccountCode,
                AZipFileContent,
                false,
                out AStatementKey,
                out AVerificationResult);
        }

        /// <summary>
        /// train an existing bank statement, where the gift batch has been posted already
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool TrainBankStatement(Int32 ALedgerNumber, DateTime ADateOfStatement, string ABankAccountCode)
        {
            // get the statement keys
            TDataBase db = DBAccess.Connect("TrainBankStatement");
            TDBTransaction ReadTransaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            AEpStatementTable Statements = new AEpStatementTable();
            AEpStatementRow row = Statements.NewRowTyped(false);

            row.LedgerNumber = ALedgerNumber;
            row.Date = ADateOfStatement;
            row.BankAccountCode = ABankAccountCode;

            Statements = AEpStatementAccess.LoadUsingTemplate(row, ReadTransaction);

            ReadTransaction.Rollback();

            db.CloseDBConnection();

            if (Statements.Rows.Count == 0)
            {
                return false;
            }

            // search for already posted gift batches, and do the matching for these imported statements
            TBankImportMatching.Train(Statements);

            return true;
        }

        /// <summary>
        /// train all bank statements of last month, in this ledger
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool TrainBankStatementsLastMonth(Int32 ALedgerNumber, DateTime AToday)
        {
            string sessionID = TSession.GetSessionID();
            string configfilename = TAppSettingsManager.ConfigFileName;
            Int32 clientID = DomainManager.GClientID;
            Thread t = new Thread(() => TrainBankStatementsLastMonthThread(
                    configfilename,
                    sessionID,
                    clientID,
                    ALedgerNumber, AToday));

            t.Name = Guid.NewGuid().ToString();

            t.Start();
            return true;
        }

        private static bool TrainBankStatementsLastMonthThread(String AConfigFileName, String ASessionID, Int32 AClientID, Int32 ALedgerNumber, DateTime AToday)
        {
            TSession.InitThread("TrainBankStatementsLastMonthThread", AConfigFileName, ASessionID);

            DomainManager.GClientID = AClientID;
            string MyClientID = DomainManager.GClientID.ToString();

            TProgressTracker.InitProgressTracker(MyClientID,
                Catalog.GetString("Training last months bank statements"),
                30);

            DateTime startDateThisMonth = new DateTime(AToday.Year, AToday.Month, 1);
            DateTime endDateLastMonth = startDateThisMonth.AddDays(-1);
            DateTime startDateLastMonth = new DateTime(endDateLastMonth.Year, endDateLastMonth.Month, 1);

            // for debugging the training:
            //startDateLastMonth = new DateTime(2015,10,5);
            //startDateThisMonth = startDateLastMonth.AddDays(1);

            // get all bank accounts
            TCacheable CachePopulator = new TCacheable();
            Type typeofTable;
            GLSetupTDSAAccountTable accounts = (GLSetupTDSAAccountTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.AccountList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            foreach (GLSetupTDSAAccountRow account in accounts.Rows)
            {
                // at OM Germany we don't have the bank account flags set
                if ((!account.IsBankAccountFlagNull() && (account.BankAccountFlag == true))
                    || (!account.IsCashAccountFlagNull() && (account.CashAccountFlag == true))
                    )
                {
                    string BankAccountCode = account.AccountCode;

                    DateTime counter = startDateLastMonth;

                    while (!counter.Equals(startDateThisMonth))
                    {
                        TProgressTracker.SetCurrentState(
                            MyClientID,
                            String.Format(Catalog.GetString("Training {0} {1}"), BankAccountCode, counter.ToShortDateString()),
                            counter.Day);

                        // TODO: train only one bank statement per date and bank account?
                        TrainBankStatement(ALedgerNumber, counter, BankAccountCode);
                        counter = counter.AddDays(1);
                    }
                }
            }

            TProgressTracker.FinishJob(MyClientID);

            return true;
        }

        /// <summary>
        /// returns the bank statements that are from or newer than the given date
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AEpStatementTable GetImportedBankStatements(Int32 ALedgerNumber, short ALimit)
        {
            TDataBase db = DBAccess.Connect("GetImportedBankStatements");
            TDBTransaction ReadTransaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            string SqlStmt = "SELECT * FROM PUB.a_ep_statement " +
                "WHERE a_ledger_number_i = " + ALedgerNumber.ToString() + " " +
                "ORDER BY a_statement_key_i DESC ";

            if (ALimit > 0)
            {
                SqlStmt += "LIMIT " + ALimit.ToString();
            }

            AEpStatementTable result = new AEpStatementTable();
            db.SelectDT(result, SqlStmt, ReadTransaction);

            ReadTransaction.Rollback();

            db.CloseDBConnection();

            return result;
        }

        /// <summary>
        /// drop a bank statement and all its transactions
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool DropBankStatement(Int32 ALedgerNumber, Int32 AStatementKey, out TVerificationResultCollection AVerificationResult, TDataBase ADataBase = null)
        {
            AVerificationResult = new TVerificationResultCollection();
            TDataBase db = DBAccess.Connect("DropBankStatement", ADataBase);
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            BankImportTDS MainDS = new BankImportTDS();

            AEpStatementAccess.LoadByPrimaryKey(MainDS, AStatementKey, Transaction);
            AEpTransactionAccess.LoadViaAEpStatement(MainDS, AStatementKey, Transaction);

            Transaction.Rollback();

            if (MainDS.AEpStatement.Rows.Count == 0)
            {
                AVerificationResult.Add(new TVerificationResult("error", "cannot find statement", TResultSeverity.Resv_Critical));
                db.CloseDBConnection();
                return false;
            }

            foreach (AEpStatementRow stmtRow in MainDS.AEpStatement.Rows)
            {
                if (stmtRow.LedgerNumber != ALedgerNumber)
                {
                    AVerificationResult.Add(new TVerificationResult("error", "Statement is not linked to current ledger", TResultSeverity.Resv_Critical));
                    return false;
                }

                stmtRow.Delete();
            }

            foreach (AEpTransactionRow transactionRow in MainDS.AEpTransaction.Rows)
            {
                transactionRow.Delete();
            }

            MainDS.ThrowAwayAfterSubmitChanges = true;
            try
            {
                BankImportTDSAccess.SubmitChanges(MainDS, db);
                return true;
            }
            catch (Exception)
            {
                AVerificationResult.Add(new TVerificationResult("error", "cannot delete statement", TResultSeverity.Resv_Critical));
                return false;
            }
            finally
            {
                if (ADataBase == null)
                {
                    db.CloseDBConnection();
                }
            }
        }

        /// <summary>
        /// drop multiple bank statements by criteria, eg. OlderThan1Year, All
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool DropBankStatements(Int32 ALedgerNumber, String ACriteria, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            TDataBase db = DBAccess.Connect("DropOldBankStatements");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.ReadCommitted);
            bool Result = false;
            BankImportTDS MainDS = new BankImportTDS();
            AEpStatementAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            Transaction.Rollback();

            if (MainDS.AEpStatement.Rows.Count == 0)
            {
                AVerificationResult.Add(new TVerificationResult("error", "cannot find statement", TResultSeverity.Resv_Critical));
                db.CloseDBConnection();
                return false;
            }

            if (ACriteria == "All")
            {
                foreach (AEpStatementRow stmtRow in MainDS.AEpStatement.Rows)
                {
                    Result = DropBankStatement(ALedgerNumber, stmtRow.StatementKey, out AVerificationResult, db);
                    if (!Result)
                    {
                        break;
                    }
                }
            }
            else if (ACriteria == "OlderThan1Year")
            {
                foreach (AEpStatementRow stmtRow in MainDS.AEpStatement.Rows)
                {
                    if (stmtRow.Date < DateTime.Now.AddYears(-1))
                    {
                        Result = DropBankStatement(ALedgerNumber, stmtRow.StatementKey, out AVerificationResult, db);
                        if (!Result)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                AVerificationResult.Add(new TVerificationResult("error", "Criteria is not recognised", TResultSeverity.Resv_Critical));
            }

            db.CloseDBConnection();
            return Result;
        }

        private static bool FindDonorByIBAN(
            DataView APartnerByBankAccount,
            string AIBAN,
            out Int64 ADonorKey)
        {
            ADonorKey = -1;

            DataRowView[] rows = APartnerByBankAccount.FindRows(new object[] { AIBAN });

            if (rows.Length == 1)
            {
                ADonorKey = Convert.ToInt64(rows[0].Row["PartnerKey"]);
                return true;
            }

            return false;
        }

        private static string FindDonorName(
            DataView APartnerByDonorKey,
            Int64 ADonorKey)
        {
            if (ADonorKey == 0)
            {
                return String.Empty;
            }

            DataRowView[] rows = APartnerByDonorKey.FindRows(new object[] { ADonorKey });

            if (rows.Length == 1)
            {
                return rows[0].Row["ShortName"].ToString();
            }

            return String.Empty;
        }

        private struct MatchUpdate
        {
            public MatchUpdate(ref AEpMatchRow AOldValue, string ADBAction)
            {
                actualRow = AOldValue;
                newValue = ((AEpMatchTable)AOldValue.Table).NewRowTyped();
                newValue.ItemArray = AOldValue.ItemArray.Clone() as object[];
                dbAction = ADBAction;
            }

            public AEpMatchRow actualRow;
            public AEpMatchRow newValue;
            public string dbAction;

            public bool UpdateActualRow()
            {
                bool changed = false;
                if (actualRow.Action != newValue.Action)
                {
                    actualRow.Action = newValue.Action;
                    changed = true;
                }
                if (actualRow.RecentMatch != newValue.RecentMatch)
                {
                    actualRow.RecentMatch = newValue.RecentMatch;
                    changed = true;
                }
                if (actualRow.DonorKey != newValue.DonorKey)
                {
                    actualRow.DonorKey = newValue.DonorKey;
                    changed = true;
                }
                if (actualRow.MotivationGroupCode != newValue.MotivationGroupCode)
                {
                    actualRow.MotivationGroupCode = newValue.MotivationGroupCode;
                    changed = true;
                }
                if (actualRow.MotivationDetailCode != newValue.MotivationDetailCode)
                {
                    actualRow.MotivationDetailCode = newValue.MotivationDetailCode;
                    changed = true;
                }
                return changed;
            }
        }

        private static bool AddUpdateMatches(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow, string ADBAction)
        {
            if (!AUpdates.ContainsKey(ARow.MatchText + "::" + ARow.Detail.ToString()))
            {
                AUpdates.Add(ARow.MatchText + "::" + ARow.Detail.ToString(), new MatchUpdate(ref ARow, ADBAction));
            }

            return true;
        }

        private static void UpdateMatchesInsertNewRow(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow)
        {
            AddUpdateMatches(ref AUpdates, ref ARow, "INSERT");
        }

        private static void UpdateMatchesRecentMatch(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow, DateTime ANewDateEffective)
        {
            AddUpdateMatches(ref AUpdates, ref ARow, "UPDATE");
            AUpdates[ARow.MatchText + "::" + ARow.Detail.ToString()].newValue.RecentMatch = ANewDateEffective;
        }

        private static void UpdateMatchesMotivation(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow, string MotivationGroup, string MotivationDetail)
        {
            AddUpdateMatches(ref AUpdates, ref ARow, "UPDATE");
            AUpdates[ARow.MatchText + "::" + ARow.Detail.ToString()].newValue.MotivationGroupCode = MotivationGroup;
            AUpdates[ARow.MatchText + "::" + ARow.Detail.ToString()].newValue.MotivationDetailCode = MotivationDetail;
        }

        private static void UpdateMatchesDonorKey(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow, Int64 ADonorKey)
        {
            AddUpdateMatches(ref AUpdates, ref ARow, "UPDATE");
            AUpdates[ARow.MatchText + "::" + ARow.Detail.ToString()].newValue.DonorKey = ADonorKey;
        }

        private static void UpdateMatchesUnmatch(ref SortedList <string, MatchUpdate>AUpdates, ref AEpMatchRow ARow)
        {
            AddUpdateMatches(ref AUpdates, ref ARow, "UPDATE");
            AUpdates[ARow.MatchText + "::" + ARow.Detail.ToString()].newValue.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
        }

        /// <summary>
        /// returns the transactions of the bank statement, and the matches if they exist;
        /// tries to find matches too
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static BankImportTDS GetBankStatementTransactionsAndMatches(Int32 AStatementKey, Int32 ALedgerNumber, bool AInitialLoad = false, bool AClearTables = true, TDataBase ADataBase = null)
        {
            TDataBase db = DBAccess.Connect("GetBankStatementTransactionsAndMatches", ADataBase);
            bool NewTransaction;
            TDBTransaction Transaction = db.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            BankImportTDS ResultDataset = new BankImportTDS();
            string MyClientID = DomainManager.GClientID.ToString();

            TProgressTracker.InitProgressTracker(MyClientID,
                Catalog.GetString("Load Bank Statement"),
                100.0m);

            TProgressTracker.SetCurrentState(MyClientID,
                Catalog.GetString("loading statement"),
                0);

            DataTable PartnerByDonorKey;

            try
            {
                AEpStatementAccess.LoadByPrimaryKey(ResultDataset, AStatementKey, Transaction);

                if (ResultDataset.AEpStatement[0].BankAccountCode.Length == 0)
                {
                    throw new Exception("Loading Bank Statement: Bank Account must not be empty");
                }

                ACostCentreAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AMotivationDetailAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AEpTransactionAccess.LoadViaAEpStatement(ResultDataset, AStatementKey, Transaction);
                AEpMatchAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                BankImportTDS UpdateDS = new BankImportTDS();
                AEpTransactionAccess.LoadViaAEpStatement(UpdateDS, AStatementKey, Transaction);
                AEpMatchAccess.LoadViaALedger(UpdateDS, ALedgerNumber, Transaction);

                // load all bankingdetails and partner shortnames related to this statement
                string sqlLoadPartnerByBankAccount =
                    "SELECT DISTINCT p.p_partner_key_n AS PartnerKey, " +
                    "p.p_partner_short_name_c AS ShortName, " +
                    "t.a_iban_c AS IBAN " +
                    "FROM PUB_a_ep_transaction t, PUB_p_banking_details bd, PUB_p_bank b, PUB_p_partner_banking_details pbd, PUB_p_partner p " +
                    "WHERE t.a_statement_key_i = " + AStatementKey.ToString() + " " +
                    "AND ((bd.p_iban_c = t.a_iban_c) or " +
                    "   (bd.p_bank_account_number_c = t.a_bank_account_number_c " +
                    "      AND b.p_partner_key_n = bd.p_bank_key_n " +
                    "      AND b.p_branch_code_c = t.p_branch_code_c) " +
                    " ) " +
                    "AND pbd.p_banking_details_key_i = bd.p_banking_details_key_i " +
                    "AND p.p_partner_key_n = pbd.p_partner_key_n";

                DataTable PartnerByBankAccount = db.SelectDT(sqlLoadPartnerByBankAccount, "partnerByBankAccount", Transaction);
                PartnerByBankAccount.DefaultView.Sort = "IBAN";

                // load all partner short names of matches
                string sqlLoadPartnerName =
                    "SELECT DISTINCT p.p_partner_key_n AS PartnerKey, " +
                    "p.p_partner_short_name_c AS ShortName " +
                    "FROM PUB_a_ep_transaction t, PUB_a_ep_match m, PUB_p_partner p " +
                    "WHERE t.a_statement_key_i = " + AStatementKey.ToString() + " " +
                    "AND t.a_match_text_c = m.a_match_text_c " +
                    "AND (m.p_donor_key_n = p.p_partner_key_n OR m.p_recipient_key_n = p.p_partner_key_n)";

                PartnerByDonorKey = db.SelectDT(sqlLoadPartnerName, "partnerByDonorKey", Transaction);
                PartnerByDonorKey.DefaultView.Sort = "PartnerKey";

                // get all recipients that have been merged
                string sqlGetMergedRecipients =
                    string.Format(
                        "SELECT DISTINCT p.p_partner_key_n AS PartnerKey, p.p_status_code_c AS StatusCode FROM PUB_a_ep_match m, PUB_p_partner p " +
                        "WHERE (m.p_recipient_key_n = p.p_partner_key_n OR m.p_donor_key_n = p.p_partner_key_n) AND p.p_status_code_c = '{0}'",
                        MPartnerConstants.PARTNERSTATUS_MERGED);
                DataTable MergedPartners = db.SelectDT(sqlGetMergedRecipients, "mergedPartners", Transaction);
                MergedPartners.DefaultView.Sort = "PartnerKey";

                // get all recipients that are not active anymore
                string sqlGetNotActiveRecipients =
                    string.Format(
                        "SELECT DISTINCT p.p_partner_key_n AS PartnerKey, p.p_status_code_c AS StatusCode FROM PUB_a_ep_match m, PUB_p_partner p " +
                        "WHERE m.p_recipient_key_n = p.p_partner_key_n AND p.p_status_code_c <> '{0}'",
                        MPartnerConstants.PARTNERSTATUS_ACTIVE);
                DataTable NotActiveRecipients = db.SelectDT(sqlGetNotActiveRecipients, "notActiveRecipients", Transaction);
                NotActiveRecipients.DefaultView.Sort = "PartnerKey";

                string BankAccountCode = ResultDataset.AEpStatement[0].BankAccountCode;

                UpdateDS.AEpMatch.DefaultView.Sort = AEpMatchTable.GetMatchTextDBName();

                SortedList <string, MatchUpdate>MatchesUpdates = new SortedList <string, MatchUpdate>();

                int count = 0;

                // load the matches or create new matches
                foreach (BankImportTDSAEpTransactionRow TrRow in ResultDataset.AEpTransaction.Rows)
                {
                    TProgressTracker.SetCurrentState(MyClientID,
                        Catalog.GetString("finding matches") +
                        " " + count + "/" + ResultDataset.AEpTransaction.Rows.Count.ToString(),
                        10.0m + (count * 80.0m / ResultDataset.AEpTransaction.Rows.Count));
                    count++;

                    // find the same row from the UpdateDS
                    BankImportTDSAEpTransactionRow UpdateTrRow =
                        (BankImportTDSAEpTransactionRow)UpdateDS.AEpTransaction.Rows.Find(
                            new object[] {
                                TrRow.StatementKey,
                                TrRow.Order,
                                TrRow.DetailKey
                            });

                    // find a match with the same match text, or create a new one
                    if (TrRow.IsMatchTextNull() || (TrRow.MatchText.Length == 0) || !TrRow.MatchText.StartsWith(BankAccountCode))
                    {
                        TrRow.MatchText = TBankImportMatching.CalculateMatchText(BankAccountCode, TrRow);

                        UpdateTrRow.MatchText = TrRow.MatchText;
                    }

                    DataRowView[] matches = UpdateDS.AEpMatch.DefaultView.FindRows(TrRow.MatchText);

                    if (matches.Length == 0)
                    {
                        // create new match
                        AEpMatchRow newRow = UpdateDS.AEpMatch.NewRowTyped(true);
                        newRow.EpMatchKey = (UpdateDS.AEpMatch.Count + MatchesUpdates.Count + 1) * -1;
                        newRow.Detail = 0;
                        newRow.MatchText = TrRow.MatchText;
                        newRow.LedgerNumber = ALedgerNumber;
                        newRow.GiftTransactionAmount = TrRow.TransactionAmount;
                        newRow.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                        UpdateMatchesInsertNewRow(ref MatchesUpdates, ref newRow);

                        TrRow.MatchAction = newRow.Action;

                        // filter matches again
                        matches = UpdateDS.AEpMatch.DefaultView.FindRows(TrRow.MatchText);
                    }

                    if (matches.Length > 0)
                    {
                        Decimal sum = 0.0m;

                        // update the recent date
                        foreach (DataRowView rv in matches)
                        {
                            AEpMatchRow r = (AEpMatchRow)rv.Row;

                            sum += r.GiftTransactionAmount;
                            string action = r.Action;

                            // check if the recipient key is still valid. could be that they have married, and merged into another family record
                            if ((r.RecipientKey != 0)
                                && (MergedPartners.DefaultView.FindRows(r.RecipientKey).Length > 0))
                            {
                                TLogging.LogAtLevel(1, "partner has been merged: " + r.RecipientKey.ToString());
                                r.RecipientKey = 0;
                                action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            }

                            // check if the donor key is still valid. could be that they have married, and merged into another family record
                            if ((r.DonorKey != 0)
                                && (MergedPartners.DefaultView.FindRows(r.DonorKey).Length > 0))
                            {
                                TLogging.LogAtLevel(1, "partner has been merged: " + r.DonorKey.ToString());
                                r.DonorKey = 0;
                                action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            }

                            // check if the recipient is still active
                            if ((r.RecipientKey != 0)
                                && (NotActiveRecipients.DefaultView.FindRows(r.RecipientKey).Length > 0))
                            {
                                r.RecipientKey = 0;
                                action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            }

                            // check if the costcentre is still active
                            ACostCentreRow costcentre = (ACostCentreRow)ResultDataset.ACostCentre.Rows.Find(new object[] { ALedgerNumber,
                                                                                                                           r.CostCentreCode });

                            if ((costcentre != null) && !costcentre.CostCentreActiveFlag)
                            {
                                TLogging.LogAtLevel(1, "costcentre " + r.CostCentreCode + " is not active anymore; donor: " + r.DonorKey.ToString());
                                action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            }

                            if (r.RecentMatch < TrRow.DateEffective)
                            {
                                // do not modify RecentMatch here for speed reasons
                                // r.RecentMatch = row.DateEffective;
                                UpdateMatchesRecentMatch(ref MatchesUpdates, ref r, TrRow.DateEffective);
                            }

                            if (action == MFinanceConstants.BANK_STMT_STATUS_UNMATCHED && r.Action != action)
                            {
                                UpdateMatchesUnmatch(ref MatchesUpdates, ref r);
                            }

                            TrRow.MatchAction = action;

                            if (r.IsDonorKeyNull() || (r.DonorKey <= 0))
                            {
                                Int64 DonorKey;

                                if (FindDonorByIBAN(PartnerByBankAccount.DefaultView, TrRow.Iban, out DonorKey))
                                {
                                    UpdateMatchesDonorKey(ref MatchesUpdates, ref r, DonorKey);
                                }
                            }

                            if (r.MotivationGroupCode == String.Empty && r.MotivationDetailCode == String.Empty)
                            {
                                // if there is a default motivation detail in the database, then default to that motivation detail
                                string DefaultMotivation = new TSystemDefaults().GetSystemDefault("DEFAULTMOTIVATION" + ALedgerNumber.ToString());
                                if (DefaultMotivation != SharedConstants.SYSDEFAULT_NOT_FOUND)
                                {
                                    UpdateMatchesMotivation(ref MatchesUpdates, ref r,
                                        DefaultMotivation.Substring(0, DefaultMotivation.IndexOf("::")),
                                        DefaultMotivation.Substring(DefaultMotivation.IndexOf("::") + 2));
                                }
                            }
                        }

                        if (sum != TrRow.TransactionAmount && AInitialLoad)
                        {
                            TLogging.Log(
                                "we should drop this match since the total is wrong: " + TrRow.Description + " " + sum.ToString() + " " +
                                TrRow.TransactionAmount.ToString());
                            TrRow.MatchAction = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;

                            foreach (DataRowView rv in matches)
                            {
                                AEpMatchRow r = (AEpMatchRow)rv.Row;
                                UpdateMatchesUnmatch(ref MatchesUpdates, ref r);
                            }
                        }
                    }
                }

                // for speed reasons, add the new rows after clearing the sort on the view
                UpdateDS.AEpMatch.DefaultView.Sort = string.Empty;

                foreach (MatchUpdate update in MatchesUpdates.Values)
                {
                    update.UpdateActualRow();

                    if (update.dbAction == "INSERT")
                    {
                        UpdateDS.AEpMatch.Rows.Add(update.actualRow);
                    }
                }

                TProgressTracker.SetCurrentState(MyClientID,
                    Catalog.GetString("save matches"),
                    90.0m);

                UpdateDS.ThrowAwayAfterSubmitChanges = true;
                // only store a_ep_transactions and a_ep_matches, but without additional typed fields (ie MatchAction)
                BankImportTDSAccess.SubmitChanges(UpdateDS.GetChangesTyped(true), db);
            }
            catch (Exception e)
            {
                TLogging.Log(e.GetType().ToString() + " in BankImport, GetBankStatementTransactionsAndMatches; " + e.Message);
                TLogging.Log(e.StackTrace);

                if (NewTransaction)
                {
                    Transaction.Rollback();
                }

                throw;
            }

            // drop all matches that do not occur on this statement
            ResultDataset.AEpMatch.Clear();

            // reloading is faster than deleting all matches that are not needed
            string sqlLoadMatchesOfStatement =
                "SELECT DISTINCT m.* FROM PUB_a_ep_match m, PUB_a_ep_transaction t WHERE t.a_statement_key_i = ? AND m.a_ledger_number_i = ? AND m.a_match_text_c = t.a_match_text_c";

            OdbcParameter param = new OdbcParameter("statementkey", OdbcType.Int);
            param.Value = AStatementKey;
            OdbcParameter paramLedgerNumber = new OdbcParameter("ledgerNumber", OdbcType.Int);
            paramLedgerNumber.Value = ALedgerNumber;

            db.SelectDT(ResultDataset.AEpMatch,
                sqlLoadMatchesOfStatement,
                Transaction,
                new OdbcParameter[] { param, paramLedgerNumber }, -1, -1);

            // update the custom field for cost centre name for each match
            foreach (BankImportTDSAEpMatchRow row in ResultDataset.AEpMatch.Rows)
            {
                ACostCentreRow ccRow = (ACostCentreRow)ResultDataset.ACostCentre.Rows.Find(new object[] { row.LedgerNumber, row.CostCentreCode });

                if (ccRow != null)
                {
                    row.CostCentreName = ccRow.CostCentreName;
                }

                row.DonorShortName = FindDonorName(PartnerByDonorKey.DefaultView, row.DonorKey);
                row.RecipientShortName = FindDonorName(PartnerByDonorKey.DefaultView, row.RecipientKey);
            }

            // remove all rows that we do not need on the client side
            if (AClearTables)
            {
                ResultDataset.AGiftDetail.Clear();
                ResultDataset.AMotivationDetail.Clear();
                ResultDataset.ACostCentre.Clear();
            }

            ResultDataset.AcceptChanges();

            if (TLogging.DebugLevel > 0)
            {
                int CountMatched = 0;

                foreach (BankImportTDSAEpTransactionRow transaction in ResultDataset.AEpTransaction.Rows)
                {
                    if (!transaction.IsMatchActionNull() && (transaction.MatchAction != MFinanceConstants.BANK_STMT_STATUS_UNMATCHED))
                    {
                        CountMatched++;
                    }
                }

                TLogging.Log(
                    "Loading bank statement: matched: " + CountMatched.ToString() + " of " + ResultDataset.AEpTransaction.Rows.Count.ToString());
            }

            Transaction.Commit();

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }

            TProgressTracker.FinishJob(MyClientID);

            return ResultDataset;
        }

        /// <summary>
        /// returns the transactions of the bank statement.
        /// returns only the transactions, not the details. no matching.
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AEpTransactionTable GetTransactions(
            Int32 AStatementKey,
            Int32 ALedgerNumber,
            String AMatchAction,
            out decimal ATotalDebit,
            out decimal ATotalCredit,
            out String ACurrencyCode
            )
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber, true);
            ATotalCredit = 0;
            ATotalDebit = 0;

            foreach (BankImportTDSAEpTransactionRow row in MainDS.AEpTransaction.Rows)
            {
                if (row.MatchAction != AMatchAction && AMatchAction != String.Empty )
                {
                    row.Delete();
                }
                else
                {
                    if (row.TransactionAmount > 0)
                    {
                        ATotalCredit += row.TransactionAmount;
                    }
                    else
                    {
                        ATotalDebit += row.TransactionAmount*(-1.0m);
                    }
                }
            }

            ACurrencyCode = TFinanceServerLookupWebConnector.GetLedgerBaseCurrency(ALedgerNumber);

            MainDS.AcceptChanges();

            return MainDS.AEpTransaction;
        }

        /// <summary>
        /// returns the transaction details of a transaction of the bank statement, and the matches if they exist;
        /// tries to find matches too.
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadTransactionAndDetails(Int32 AStatementKey, Int32 ALedgerNumber, Int32 AOrderNumber,
            out BankImportTDSAEpTransactionTable ATransactions,
            out BankImportTDSTransactionDetailTable ADetails)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber);

            ATransactions = new BankImportTDSAEpTransactionTable();

            foreach (BankImportTDSAEpTransactionRow row in MainDS.AEpTransaction.Rows)
            {
                if (row.Order == AOrderNumber)
                {
                    BankImportTDSAEpTransactionRow newRow = ATransactions.NewRowTyped(false);
                    DataUtilities.CopyAllColumnValues(row, newRow);
                    ATransactions.Rows.Add(newRow);
                }
            }

            ATransactions.Constraints.Clear();
            ATransactions.Columns.Remove("a_detail_key_i");
            ATransactions.Columns.Remove("GiftLedgerNumber");
            ATransactions.Columns.Remove("GiftBatchNumber");
            ATransactions.Columns.Remove("GiftTransactionNumber");
            ATransactions.Columns.Remove("GiftDetailNumbers");

            ATransactions.AcceptChanges();

            ADetails = new BankImportTDSTransactionDetailTable();

            foreach (BankImportTDSAEpMatchRow row in MainDS.AEpMatch.Rows)
            {
                if (row.MatchText == ATransactions[0].MatchText)
                {
                    BankImportTDSTransactionDetailRow newRow = ADetails.NewRowTyped(false);
                    DataUtilities.CopyAllColumnValues(row, newRow);
                    newRow.LedgerNumber = ALedgerNumber;
                    newRow.StatementKey = AStatementKey;
                    newRow.Order = AOrderNumber;
                    ADetails.Rows.Add(newRow);
                }
            }

            ATransactions[0].DonorKey = ADetails[0].DonorKey;
            ATransactions[0].DonorName = ADetails[0].DonorShortName;
            ADetails.Columns.Remove("p_donor_key_n");

            ADetails.AcceptChanges();

            return true;
        }

        /// <summary>
        /// returns the specified transaction detail of a transaction of the bank statement
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool LoadTransactionDetail(Int32 ALedgerNumber, Int32 AStatementKey, Int32 AOrder,
            Int32 ADetail,
            out BankImportTDSTransactionDetailTable TransactionDetail)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber, false, false);

            TransactionDetail = new BankImportTDSTransactionDetailTable();

            foreach (BankImportTDSAEpTransactionRow row in MainDS.AEpTransaction.Rows)
            {
                if (row.Order == AOrder)
                {
                    foreach (BankImportTDSAEpMatchRow match in MainDS.AEpMatch.Rows)
                    {
                        if ((match.MatchText == row.MatchText) && (match.Detail == ADetail))
                        {
                            MainDS.AMotivationDetail.DefaultView.RowFilter =
                                String.Format("{0}='{1}' and {2}='{3}'",
                                    AMotivationDetailTable.GetMotivationGroupCodeDBName(),
                                    match.MotivationGroupCode,
                                    AMotivationDetailTable.GetMotivationDetailCodeDBName(),
                                    match.MotivationDetailCode);

                            if (MainDS.AMotivationDetail.DefaultView.Count != 1)
                            {
                                throw new Exception("LoadTransactionDetail: cannot find Motivation Detail: " + MainDS.AMotivationDetail.DefaultView.RowFilter);
                            }

                            AMotivationDetailRow MotivationDetail = (AMotivationDetailRow)MainDS.AMotivationDetail.DefaultView[0].Row;

                            BankImportTDSTransactionDetailRow newRow = TransactionDetail.NewRowTyped(false);
                            DataUtilities.CopyAllColumnValues(match, newRow);
                            newRow.LedgerNumber = ALedgerNumber;
                            newRow.StatementKey = AStatementKey;
                            newRow.Order = AOrder;
                            newRow.Membership = MotivationDetail.Membership;
                            newRow.Sponsorship = MotivationDetail.Sponsorship;
                            newRow.WorkerSupport = MotivationDetail.WorkerSupport;
                            TransactionDetail.Rows.Add(newRow);

                            MainDS.AMotivationDetail.DefaultView.RowFilter = "";
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// commit matches into a_ep_match
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CommitMatches(BankImportTDS AMainDS)
        {
            if (AMainDS == null)
            {
                return false;
            }

            bool NewTransaction;
            TDataBase db = DBAccess.Connect("CommitMatches");
            TDBTransaction Transaction = db.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            try
            {
                for (int RowCount = 0; (RowCount != AMainDS.AEpMatch.Rows.Count); RowCount++)
                {
                    DataRow TheRow = AMainDS.AEpMatch.Rows[RowCount];

                    if (TheRow.RowState == DataRowState.Deleted)
                    {
                        string sql = "UPDATE " + AEpTransactionTable.GetTableDBName() +
                            " SET " + AEpTransactionTable.GetEpMatchKeyDBName() + " = NULL" +
                            " WHERE " + AEpTransactionTable.GetEpMatchKeyDBName() + " = " +
                            TheRow[AEpMatchTable.GetEpMatchKeyDBName(), DataRowVersion.Original];
                        Transaction.DataBaseObj.ExecuteNonQuery(sql, Transaction);
                    }
                }

                AMainDS.ThrowAwayAfterSubmitChanges = true;

                BankImportTDSAccess.SubmitChanges(AMainDS, Transaction.DataBaseObj);

                if (NewTransaction)
                {
                    Transaction.Commit();
                    db.CloseDBConnection();
                }

                return true;
            }
            catch (Exception e)
            {
                TLogging.Log("Bankimport, CommitMatches: " + e.ToString());

                if (NewTransaction)
                {
                    Transaction.Rollback();
                }

                db.CloseDBConnection();

                return false;
            }
        }

        /// <summary>
        /// commit transaction
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool MaintainTransaction(
            string action,
            int ALedgerNumber,
            int AStatementKey, int AOrder,
            string MatchAction,
            Int64 ADonorKey,
            out TVerificationResultCollection AVerificationResult)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber);
            AVerificationResult = new TVerificationResultCollection();
            if (MainDS == null)
            {
                return false;
            }

            // find the right transaction
            BankImportTDSAEpTransactionRow transaction = null;

            foreach (BankImportTDSAEpTransactionRow row in MainDS.AEpTransaction.Rows)
            {
                if (row.Order == AOrder)
                {
                    transaction = row;
                }
            }

            if (action == "update")
            {
                foreach (BankImportTDSAEpMatchRow row in MainDS.AEpMatch.Rows)
                {
                    if (row.MatchText == transaction.MatchText)
                    {
                        row.Action = MatchAction;
                        row.DonorKey = ADonorKey;
                    }
                }
            }
            else
            {
                return false;
            }

            try
            {
                BankImportTDSAccess.SubmitChanges(MainDS);
                return true;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                AVerificationResult.Add(new TVerificationResult("error", e.ToString(), TResultSeverity.Resv_Critical));
                return false;
            }
        }

        /// <summary>
        /// commit matches into a_ep_match
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool MaintainTransactionDetail(
            string action,
            int ALedgerNumber,
            int AStatementKey, int AOrder, int ADetail,
            string AMatchAction,
            Int64 ADonorKey, string AMotivationGroupCode, string AMotivationDetailCode,
            string AAccountCode, string ACostCentreCode,
            Int64 ARecipientKey,
            Decimal AGiftTransactionAmount,
            out TVerificationResultCollection AVerificationResult)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber);
            AVerificationResult = new TVerificationResultCollection();

            if (MainDS == null)
            {
                return false;
            }

            // find the right transaction
            BankImportTDSAEpTransactionRow transaction = null;

            foreach (BankImportTDSAEpTransactionRow row in MainDS.AEpTransaction.Rows)
            {
                if (row.Order == AOrder)
                {
                    transaction = row;
                }
            }

            if (action == "delete")
            {
                foreach (BankImportTDSAEpMatchRow row in MainDS.AEpMatch.Rows)
                {
                    if (row.MatchText == transaction.MatchText)
                    {
                        if(row.Detail == ADetail)
                        {
                            row.Delete();
                        }
                        else if (row.Detail > ADetail)
                        {
                            row.Detail--;
                        }
                    }
                }
            }
            else if (action == "create")
            {
                foreach (BankImportTDSAEpMatchRow rowOther in MainDS.AEpMatch.Rows)
                {
                    if (rowOther.MatchText == transaction.MatchText)
                    {
                        rowOther.Action = AMatchAction;
                        rowOther.DonorKey = ADonorKey;
                    }
                }

                BankImportTDSAEpMatchRow row = MainDS.AEpMatch.NewRowTyped(true);
                row.EpMatchKey = -1;
                row.MatchText = transaction.MatchText;
                row.Detail = ADetail;
                row.LedgerNumber = ALedgerNumber;
                row.Action = AMatchAction;
                row.DonorKey = ADonorKey;
                row.RecipientKey = ARecipientKey;
                row.MotivationGroupCode = AMotivationGroupCode;
                row.MotivationDetailCode = AMotivationDetailCode;
                row.AccountCode = AAccountCode;
                row.CostCentreCode = ACostCentreCode;
                row.GiftTransactionAmount = AGiftTransactionAmount;
                MainDS.AEpMatch.Rows.Add(row);
            }
            else if (action == "update")
            {
                foreach (BankImportTDSAEpMatchRow row in MainDS.AEpMatch.Rows)
                {
                    if (row.MatchText == transaction.MatchText)
                    {
                        row.Action = AMatchAction;
                        row.DonorKey = ADonorKey;
                        row.RecipientKey = ARecipientKey;
                        if (row.Detail == ADetail)
                        {
                            row.MotivationGroupCode = AMotivationGroupCode;
                            row.MotivationDetailCode = AMotivationDetailCode;
                            row.AccountCode = AAccountCode;
                            row.CostCentreCode = ACostCentreCode;
                            row.GiftTransactionAmount = AGiftTransactionAmount;
                        }
                    }
                }
            }
            else
            {
                AVerificationResult.Add(new TVerificationResult("unknown", "unknown action: '"+action+"'", TResultSeverity.Resv_Critical));
                return false;
            }

            try
            {
                BankImportTDSAccess.SubmitChanges(MainDS);
                return true;
            }
            catch (Exception e)
            {
                AVerificationResult.Add(new TVerificationResult("error", e.ToString(), TResultSeverity.Resv_Critical));
                return false;
            }
        }

        /// <summary>
        /// create a gift batch for the matched gifts, and return the gift batch number
        /// </summary>
        /// <returns>the gift batch number</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CreateGiftBatch(
            Int32 ALedgerNumber,
            Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult,
            out Int32 ABatchNumber,
            TDataBase ADataBase = null)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber, true, true, ADataBase);
            ABatchNumber = -1;
            string MyClientID = DomainManager.GClientID.ToString();

            AVerificationResult = new TVerificationResultCollection();

            MainDS.AEpTransaction.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpTransactionTable.GetStatementKeyDBName(),
                    AStatementKey);
            MainDS.AEpStatement.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    AStatementKey);
            AEpStatementRow stmt = (AEpStatementRow)MainDS.AEpStatement.DefaultView[0].Row;

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            DateTime BatchDateEffective = stmt.Date;
            TDataBase db = DBAccess.Connect("CreateGiftBatch", ADataBase);
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.Serializable);

            TProgressTracker.InitProgressTracker(MyClientID,
                Catalog.GetString("Creating gift batch"),
                MainDS.AEpTransaction.DefaultView.Count + 10);

            if (!TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref BatchDateEffective, out DateEffectiveYearNumber,
                    out DateEffectivePeriodNumber,
                    Transaction, true))
            {
                // just use the latest possible date
                string msg =
                    String.Format(Catalog.GetString("Date {0} is not in an open period of the ledger, using date {1} instead for the gift batch."),
                        stmt.Date.ToShortDateString(),
                        BatchDateEffective.ToShortDateString());
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Gift Batch"), msg, TResultSeverity.Resv_Info));
            }

            ACostCentreAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

            MainDS.AEpMatch.DefaultView.Sort =
                AEpMatchTable.GetActionDBName() + ", " +
                AEpMatchTable.GetMatchTextDBName();

            if (MainDS.AEpTransaction.DefaultView.Count == 0)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Creating Gift Batch"),
                        String.Format(Catalog.GetString("There are no transactions for statement #{0}."), AStatementKey),
                        TResultSeverity.Resv_Info));
                TProgressTracker.FinishJob(MyClientID);
                return false;
            }

            foreach (DataRowView dv in MainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataRowView[] matches = MainDS.AEpMatch.DefaultView.FindRows(new object[] {
                        MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT,
                        transactionRow.MatchText
                    });

                if (matches.Length > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)matches[0].Row;

                    if (match.IsDonorKeyNull() || (match.DonorKey == 0))
                    {
                        string msg =
                            String.Format(Catalog.GetString("Cannot create a gift for transaction {0} since there is no valid donor."),
                                transactionRow.Description);
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating Gift Batch"), msg, TResultSeverity.Resv_Critical));
                        Transaction.Rollback();
                        TProgressTracker.FinishJob(MyClientID);
                        if (ADataBase == null)
                        {
                            db.CloseDBConnection();
                        }
                        return false;
                    }
                }
            }

            string MatchedGiftReference = stmt.Filename;

            if (!stmt.IsBankAccountKeyNull())
            {
                string sqlGetBankSortCode =
                    "SELECT bank.p_branch_code_c " +
                    "FROM PUB_p_banking_details details, PUB_p_bank bank " +
                    "WHERE details.p_banking_details_key_i = ?" +
                    "AND details.p_bank_key_n = bank.p_partner_key_n";
                OdbcParameter param = new OdbcParameter("detailkey", OdbcType.Int);
                param.Value = stmt.BankAccountKey;

                PBankTable bankTable = new PBankTable();
                db.SelectDT(bankTable, sqlGetBankSortCode, Transaction, new OdbcParameter[] { param }, 0, 0);

                MatchedGiftReference = bankTable[0].BranchCode + " " + stmt.Date.Day.ToString();
            }

            Transaction.Rollback();

            GiftBatchTDS GiftDS = TGiftTransactionWebConnector.CreateAGiftBatch(
                ALedgerNumber,
                BatchDateEffective,
                String.Format(Catalog.GetString("bank import for date {0}"), stmt.Date.ToShortDateString()), db);

            AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];
            giftbatchRow.BankAccountCode = stmt.BankAccountCode;

            decimal HashTotal = 0.0M;

            MainDS.AEpTransaction.DefaultView.Sort =
                AEpTransactionTable.GetNumberOnPaperStatementDBName();

            MainDS.AEpMatch.DefaultView.RowFilter = String.Empty;
            MainDS.AEpMatch.DefaultView.Sort =
                AEpMatchTable.GetActionDBName() + ", " +
                AEpMatchTable.GetMatchTextDBName();

            int counter = 5;

            foreach (DataRowView dv in MainDS.AEpTransaction.DefaultView)
            {
                TProgressTracker.SetCurrentState(MyClientID,
                    Catalog.GetString("Preparing the gifts"),
                    counter++);

                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataRowView[] matches = MainDS.AEpMatch.DefaultView.FindRows(new object[] {
                        MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT,
                        transactionRow.MatchText
                    });

                if (matches.Length > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)matches[0].Row;

                    AGiftRow gift = GiftDS.AGift.NewRowTyped();
                    gift.LedgerNumber = giftbatchRow.LedgerNumber;
                    gift.BatchNumber = giftbatchRow.BatchNumber;
                    gift.GiftTransactionNumber = giftbatchRow.LastGiftNumber + 1;
                    gift.DonorKey = match.DonorKey;
                    gift.DateEntered = transactionRow.DateEffective;
                    gift.Reference = MatchedGiftReference;

                    if (gift.Reference.Length > AGiftTable.GetReferenceLength())
                    {
                        gift.Reference = gift.Reference.Substring(0, AGiftTable.GetReferenceLength());
                    }

                    GiftDS.AGift.Rows.Add(gift);
                    giftbatchRow.LastGiftNumber++;

                    foreach (DataRowView r in matches)
                    {
                        match = (AEpMatchRow)r.Row;

                        AGiftDetailRow detail = GiftDS.AGiftDetail.NewRowTyped();
                        detail.LedgerNumber = gift.LedgerNumber;
                        detail.BatchNumber = gift.BatchNumber;
                        detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                        detail.DetailNumber = gift.LastDetailNumber + 1;
                        gift.LastDetailNumber++;

                        detail.GiftTransactionAmount = match.GiftTransactionAmount;
                        detail.GiftAmount = match.GiftTransactionAmount;
                        HashTotal += match.GiftTransactionAmount;
                        detail.MotivationGroupCode = match.MotivationGroupCode;
                        detail.MotivationDetailCode = match.MotivationDetailCode;

                        // do not use the description in comment one, because that could show up on the gift receipt???
                        // detail.GiftCommentOne = transactionRow.Description;

                        detail.CommentOneType = MFinanceConstants.GIFT_COMMENT_TYPE_BOTH;
                        detail.CostCentreCode = match.CostCentreCode;
                        detail.RecipientKey = match.RecipientKey;
                        detail.RecipientLedgerNumber = match.RecipientLedgerNumber;

                        AMotivationDetailRow motivation = (AMotivationDetailRow)MainDS.AMotivationDetail.Rows.Find(
                            new object[] { ALedgerNumber, detail.MotivationGroupCode, detail.MotivationDetailCode });

                        if (motivation == null)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("creating gift for match {0}"), transactionRow.Description),
                                    String.Format(Catalog.GetString("Cannot find motivation group '{0}' and motivation detail '{1}' in line {2} with description {3}"),
                                        detail.MotivationGroupCode, detail.MotivationDetailCode, transactionRow.Order, transactionRow.Description),
                                    TResultSeverity.Resv_Critical));
                        }

                        // try to retrieve the current costcentre for this recipient
                        if (detail.RecipientKey != 0)
                        {
                            detail.RecipientLedgerNumber = TGiftTransactionWebConnector.GetRecipientFundNumber(detail.RecipientKey);

                            detail.CostCentreCode = TGiftTransactionWebConnector.IdentifyPartnerCostCentre(detail.LedgerNumber,
                                detail.RecipientLedgerNumber);
                        }
                        else
                        {
                            if (motivation != null)
                            {
                                detail.CostCentreCode = motivation.CostCentreCode;
                            }
                        }

                        // check for active cost centre
                        ACostCentreRow costcentre = (ACostCentreRow)MainDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, detail.CostCentreCode });

                        if ((costcentre == null) || !costcentre.CostCentreActiveFlag)
                        {
                            AVerificationResult.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("creating gift for match {0}"), transactionRow.Description),
                                    Catalog.GetString("Invalid or inactive cost centre"),
                                    TResultSeverity.Resv_Critical));
                        }

                        GiftDS.AGiftDetail.Rows.Add(detail);
                    }
                }
            }

            TProgressTracker.SetCurrentState(MyClientID,
                Catalog.GetString("Submit to database"),
                counter++);

            if (AVerificationResult.HasCriticalErrors)
            {
                TProgressTracker.FinishJob(MyClientID);
                if (ADataBase == null)
                {
                    db.CloseDBConnection();
                }
                return false;
            }

            giftbatchRow.HashTotal = HashTotal;
            giftbatchRow.BatchTotal = HashTotal;

            // do not overwrite the parameter, because there might be the hint for a different gift batch date
            TVerificationResultCollection VerificationResultSubmitChanges;

            TSubmitChangesResult result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS,
                out VerificationResultSubmitChanges);

            TProgressTracker.FinishJob(MyClientID);

            if (result == TSubmitChangesResult.scrOK)
            {
                ABatchNumber = giftbatchRow.BatchNumber;
                if (ADataBase == null)
                {
                    db.CloseDBConnection();
                }
                return true;
            }
            else
            {
                TLogging.Log("Bankimport: Problem with creating gift batch:");
                TLogging.Log(VerificationResultSubmitChanges.BuildVerificationResultString());
            }

            if (ADataBase == null)
            {
                db.CloseDBConnection();
            }
            return false;
        }

        /// <summary>
        /// create a GL batch for the matched GL Transactions, and return the GL batch number
        /// </summary>
        /// <returns>the GL batch number</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CreateGLBatch(
            Int32 ALedgerNumber,
            Int32 AStatementKey,
            out TVerificationResultCollection AVerificationResult,
            out Int32 ABatchNumber)
        {
            BankImportTDS MainDS = GetBankStatementTransactionsAndMatches(AStatementKey, ALedgerNumber, true);
            ABatchNumber = -1;

            MainDS.AEpTransaction.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpTransactionTable.GetStatementKeyDBName(),
                    AStatementKey);
            MainDS.AEpStatement.DefaultView.RowFilter =
                String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    AStatementKey);
            AEpStatementRow stmt = (AEpStatementRow)MainDS.AEpStatement.DefaultView[0].Row;

            AVerificationResult = null;

            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;
            TDataBase db = DBAccess.Connect("CreateGLBatch");
            TDBTransaction Transaction = db.BeginTransaction(IsolationLevel.ReadCommitted);

            if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, stmt.Date, out DateEffectivePeriodNumber, out DateEffectiveYearNumber,
                    Transaction))
            {
                string msg = String.Format(Catalog.GetString("Cannot create a GL batch for date {0} since it is not in an open period of the ledger."),
                    stmt.Date.ToShortDateString());
                TLogging.Log(msg);
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Creating GL Batch"), msg, TResultSeverity.Resv_Critical));

                Transaction.Rollback();
                db.CloseDBConnection();
                return false;
            }

            Int32 BatchYear, BatchPeriod;

            // if DateEffective is outside the range of open periods, use the most fitting date
            DateTime DateEffective = stmt.Date;
            TFinancialYear.GetLedgerDatePostingPeriod(ALedgerNumber, ref DateEffective, out BatchYear, out BatchPeriod, Transaction, true);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            Transaction.Rollback();
            Transaction = db.BeginTransaction(IsolationLevel.Serializable);

            GLBatchTDS GLDS = TGLPosting.CreateABatch(ALedgerNumber, db, true);

            ABatchRow glbatchRow = GLDS.ABatch[0];
            glbatchRow.BatchPeriod = BatchPeriod;
            glbatchRow.DateEffective = DateEffective;
            glbatchRow.BatchDescription = String.Format(Catalog.GetString("bank import for date {0}"), stmt.Date.ToShortDateString());

            decimal HashTotal = 0.0M;
            decimal DebitTotal = 0.0M;
            decimal CreditTotal = 0.0M;

            // TODO: support several journals
            // TODO: support several currencies, support other currencies than the base currency
            AJournalRow gljournalRow = GLDS.AJournal.NewRowTyped();
            gljournalRow.LedgerNumber = glbatchRow.LedgerNumber;
            gljournalRow.BatchNumber = glbatchRow.BatchNumber;
            gljournalRow.JournalNumber = glbatchRow.LastJournal + 1;
            gljournalRow.TransactionCurrency = LedgerTable[0].BaseCurrency;
            glbatchRow.LastJournal++;
            gljournalRow.JournalPeriod = glbatchRow.BatchPeriod;
            gljournalRow.DateEffective = glbatchRow.DateEffective;
            gljournalRow.JournalDescription = glbatchRow.BatchDescription;
            gljournalRow.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
            gljournalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.STD.ToString();
            gljournalRow.ExchangeRateToBase = 1.0m;
            GLDS.AJournal.Rows.Add(gljournalRow);

            ATransactionRow trans;

            foreach (DataRowView dv in MainDS.AEpTransaction.DefaultView)
            {
                AEpTransactionRow transactionRow = (AEpTransactionRow)dv.Row;

                DataView v = MainDS.AEpMatch.DefaultView;
                v.RowFilter = AEpMatchTable.GetActionDBName() + " = '" + MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL + "' and " +
                              AEpMatchTable.GetMatchTextDBName() + " = '" + transactionRow.MatchText + "'";

                if (v.Count > 0)
                {
                    AEpMatchRow match = (AEpMatchRow)v[0].Row;
                    trans = GLDS.ATransaction.NewRowTyped();
                    trans.LedgerNumber = glbatchRow.LedgerNumber;
                    trans.BatchNumber = glbatchRow.BatchNumber;
                    trans.JournalNumber = gljournalRow.JournalNumber;
                    trans.TransactionNumber = gljournalRow.LastTransactionNumber + 1;
                    trans.AccountCode = match.AccountCode;
                    trans.CostCentreCode = match.CostCentreCode;
                    trans.Reference = match.Reference;
                    trans.Narrative = match.Narrative;
                    trans.TransactionDate = transactionRow.DateEffective;

                    if (transactionRow.TransactionAmount < 0)
                    {
                        trans.AmountInBaseCurrency = -1 * transactionRow.TransactionAmount;
                        trans.TransactionAmount = -1 * transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = true;
                        DebitTotal += trans.AmountInBaseCurrency;
                    }
                    else
                    {
                        trans.AmountInBaseCurrency = transactionRow.TransactionAmount;
                        trans.TransactionAmount = transactionRow.TransactionAmount;
                        trans.DebitCreditIndicator = false;
                        CreditTotal += trans.AmountInBaseCurrency;
                    }

                    GLDS.ATransaction.Rows.Add(trans);
                    gljournalRow.LastTransactionNumber++;
                }
            }

            // add one transaction for the bank account as well
            trans = GLDS.ATransaction.NewRowTyped();
            trans.LedgerNumber = glbatchRow.LedgerNumber;
            trans.BatchNumber = glbatchRow.BatchNumber;
            trans.JournalNumber = gljournalRow.JournalNumber;
            trans.TransactionNumber = gljournalRow.LastTransactionNumber + 1;
            trans.AccountCode = stmt.BankAccountCode;
            trans.CostCentreCode = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
            trans.Reference = String.Empty;
            trans.Narrative = gljournalRow.JournalDescription;
            trans.TransactionDate = glbatchRow.DateEffective;

            if (CreditTotal > DebitTotal)
            {
                trans.AmountInBaseCurrency = CreditTotal - DebitTotal;
                trans.TransactionAmount = CreditTotal - DebitTotal;
                trans.DebitCreditIndicator = true;
                DebitTotal += (CreditTotal - DebitTotal);
            }
            else
            {
                trans.AmountInBaseCurrency = DebitTotal - CreditTotal;
                trans.TransactionAmount = DebitTotal - CreditTotal;
                trans.DebitCreditIndicator = false;
                CreditTotal += (DebitTotal - CreditTotal);
            }

            GLDS.ATransaction.Rows.Add(trans);
            gljournalRow.LastTransactionNumber++;

            gljournalRow.JournalDebitTotal = DebitTotal;
            gljournalRow.JournalCreditTotal = CreditTotal;
            glbatchRow.BatchDebitTotal = DebitTotal;
            glbatchRow.BatchCreditTotal = CreditTotal;
            glbatchRow.BatchControlTotal = HashTotal;

            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDS,
                out VerificationResult, db);

            if (result == TSubmitChangesResult.scrOK)
            {
                ABatchNumber = glbatchRow.BatchNumber;
                db.CloseDBConnection();
                return true;
            }

            TLogging.Log("Problems storing GL Batch");
            db.CloseDBConnection();
            return false;
        }
    }
}
