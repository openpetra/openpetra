/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;

namespace Ict.Petra.Server.MFinance.ImportExport.WebConnectors
{
    /// <summary>
    /// import a bank statement from a CSV file
    /// </summary>
    public class TBankImportWebConnector
    {
        /// <summary>
        /// upload new bank statement so that it can be used for matching etc.
        /// </summary>
        /// <param name="AStmtTable"></param>
        /// <param name="ATransTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        static public TSubmitChangesResult StoreNewBankStatement(AEpStatementTable AStmtTable,
            AEpTransactionTable ATransTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            // TODO: check for existing statement with same filename? to avoid duplicate statements? delete older statement?

            AVerificationResult = new TVerificationResultCollection();
            SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                if (AEpStatementAccess.SubmitChanges(AStmtTable, SubmitChangesTransaction, out AVerificationResult))
                {
                    // update statement key reference
                    // supports committing several bank statements at once
                    foreach (AEpTransactionRow row in ATransTable.Rows)
                    {
                        if (row.StatementKey < 0)
                        {
                            row.StatementKey = AStmtTable[(row.StatementKey + 1) * -1].StatementKey;
                        }
                    }

                    if (AEpTransactionAccess.SubmitChanges(ATransTable, SubmitChangesTransaction, out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                TLogging.Log("after submitchanges: exception " + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }

        /// <summary>
        /// returns the bank statements that are from or newer than the given date
        /// </summary>
        /// <param name="AStartDate"></param>
        /// <returns></returns>
        static public AEpStatementTable GetImportedBankStatements(DateTime AStartDate)
        {
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AEpStatementTable localTable = new AEpStatementTable();
            AEpStatementRow row = localTable.NewRowTyped(false);

            row.Date = AStartDate;

            StringCollection operators = new StringCollection();
            operators.Add(">=");

            localTable = AEpStatementAccess.LoadUsingTemplate(row, operators, null, ReadTransaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return localTable;
        }

        /// <summary>
        /// match text should uniquely identify a gift from a certain donor with a certain purpose;
        /// use account name, description, and amount;
        /// remove umlaut and spaces, because the banks sometimes play around with them
        /// </summary>
        private static string CalculateMatchText(AEpTransactionRow tr)
        {
            string matchtext = tr.AccountName + tr.Description;

            matchtext += tr.TransactionAmount;

            matchtext = matchtext.Replace(",", "").Replace("/", "").Replace("-", "").Replace(";", "").Replace(".", "");

            string oldMatchText = String.Empty;

            while (oldMatchText != matchtext)
            {
                oldMatchText = matchtext;
                matchtext = matchtext.ToUpper();
                matchtext = matchtext.Replace("UE", "");
                matchtext = matchtext.Replace("AE", "");
                matchtext = matchtext.Replace("OE", "");
                matchtext = matchtext.Replace("SS", "");
                matchtext = matchtext.Replace("Ü", "");
                matchtext = matchtext.Replace("Ä", "");
                matchtext = matchtext.Replace("Ö", "");
                matchtext = matchtext.Replace("ß", "");
                matchtext = matchtext.Replace(" ", "");
            }

            return matchtext;
        }

        /// <summary>
        /// returns the transactions of the bank statement, and the matches if they exist;
        /// tries to find matches too
        /// </summary>
        static public BankImportTDS GetBankStatementTransactionsAndMatches(Int32 AStatementKey, Int32 ALedgerNumber)
        {
            TVerificationResultCollection VerificationResult;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            BankImportTDS ResultDataset = new BankImportTDS();

            try
            {
                ACostCentreAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                // TODO load Motivation Groups as well
                AMotivationDetailAccess.LoadViaALedger(ResultDataset, ALedgerNumber, Transaction);

                AEpTransactionAccess.LoadViaAEpStatement(ResultDataset, AStatementKey, Transaction);

                // load the matches or create new matches
                foreach (AEpTransactionRow row in ResultDataset.AEpTransaction.Rows)
                {
                    if (!row.IsEpMatchKeyNull())
                    {
                        // load existing match
                        AEpMatchTable tempTable = AEpMatchAccess.LoadByPrimaryKey(row.EpMatchKey, Transaction);

                        ResultDataset.AEpMatch.Merge(tempTable);
                    }
                    else
                    {
                        // find a match with the same match text, or create a new one
                        string matchText = CalculateMatchText(row);

                        AEpMatchTable tempTable = new AEpMatchTable();
                        AEpMatchRow tempRow = tempTable.NewRowTyped(false);
                        tempRow.MatchText = matchText;
                        tempTable = AEpMatchAccess.LoadUsingTemplate(tempRow, Transaction);

                        if (tempTable.Count > 0)
                        {
                            // update the recent date
                            bool update = false;

                            foreach (AEpMatchRow tempRow2 in tempTable.Rows)
                            {
                                if (tempRow2.RecentMatch < row.DateEffective)
                                {
                                    tempRow2.RecentMatch = row.DateEffective;
                                    update = true;
                                }
                            }

                            if (update)
                            {
                                AEpMatchAccess.SubmitChanges(tempTable, Transaction, out VerificationResult);
                            }

                            row.EpMatchKey = tempTable[0].EpMatchKey;

                            ResultDataset.AEpMatch.Merge(tempTable);
                        }
                        else
                        {
                            // create new match
                            tempRow = tempTable.NewRowTyped(true);
                            tempRow.EpMatchKey = -1;
                            tempRow.Detail = 0;
                            tempRow.MatchText = matchText;
                            tempRow.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                            tempTable.Rows.Add(tempRow);
                            AEpMatchAccess.SubmitChanges(tempTable, Transaction, out VerificationResult);
                            row.EpMatchKey = tempTable[0].EpMatchKey;

                            ResultDataset.AEpMatch.Merge(tempTable);
                        }
                    }
                }

                AEpTransactionAccess.SubmitChanges(ResultDataset.AEpTransaction, Transaction, out VerificationResult);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                TLogging.Log("Exception in BankImport, GetBankStatementTransactionsAndMatches; " + e.Message);
                TLogging.Log(e.StackTrace);
                throw e;
            }

            return ResultDataset;
        }

        /// <summary>
        /// commit matches into a_ep_match
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <returns></returns>
        static public bool CommitMatches(BankImportTDS AMainDS)
        {
            // TODO: store into match table

            return false;
        }

        /// <summary>
        /// create a gift batch for the matched gifts
        /// </summary>
        /// <returns>the gift batch number</returns>
        static public Int32 CreateGiftBatch(BankImportTDS AMainDS, Int32 ALedgerNumber, Int32 AGiftBatchNumber)
        {
            // TODO: create a gift batch and return the gift batch number
            // or use the preselected gift batch
            return -1;
        }

        /// <summary>
        /// create a gl batch for the matched gl transactions
        /// </summary>
        /// <returns>the batch number</returns>
        static public Int32 CreateGLBatch(BankImportTDS AMainDS, Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            // TODO: create a batch and return the batch number
            // or use the preselected batch
            return -1;
        }
    }
}