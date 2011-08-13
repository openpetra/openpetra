//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash
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
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance GL screens
    ///</summary>
    public class TTransactionWebConnector
    {
        /// <summary>
        /// retrieve the start and end dates of the current period of the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetCurrentPeriodDates(Int32 ALedgerNumber, out DateTime AStartDate, out DateTime AEndDate)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod,
                Transaction);

            AStartDate = AccountingPeriodTable[0].PeriodStartDate;
            AEndDate = AccountingPeriodTable[0].PeriodEndDate;

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// Get the valid dates for posting;
        /// based on current period and number of forwarding periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod,
                Transaction);

            AStartDateCurrentPeriod = AccountingPeriodTable[0].PeriodStartDate;

            AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods,
                Transaction);
            AEndDateLastForwardingPeriod = AccountingPeriodTable[0].PeriodEndDate;

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// Get the start date and end date
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYearNumber"></param>
        /// <param name="ADiffPeriod"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="AStartDatePeriod"></param>
        /// <param name="AEndDatePeriod"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetPeriodDates(Int32 ALedgerNumber,
            Int32 AYearNumber,
            Int32 ADiffPeriod,
            Int32 APeriodNumber,
            out DateTime AStartDatePeriod,
            out DateTime AEndDatePeriod)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, Transaction);

            // TODO: ADiffPeriod for support of different financial years

            AStartDatePeriod = AccountingPeriodTable[0].PeriodStartDate;
            AEndDatePeriod = AccountingPeriodTable[0].PeriodEndDate;

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AStartDatePeriod = AStartDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));
            AEndDatePeriod = AEndDatePeriod.AddMonths(-12 * (LedgerTable[0].CurrentFinancialYear - AYearNumber));

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            return TGLPosting.CreateABatch(ALedgerNumber);
        }

        /// <summary>
        /// loads a list of batches for the given ledger
        /// also get the ledger for the base currency etc
        /// TODO: limit to period, limit to batch status, etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFilterBatchStatus"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadABatch(Int32 ALedgerNumber, TFinanceBatchFilterEnum AFilterBatchStatus)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            if (AFilterBatchStatus == TFinanceBatchFilterEnum.fbfAll)
            {
                ABatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            }

            if ((AFilterBatchStatus & TFinanceBatchFilterEnum.fbfEditing) != 0)
            {
                ABatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadAJournal(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch and journal
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of attributes for the given transaction (identified by ledger,batch,Journal and Transactionnumber)
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="ATransactionNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLBatchTDS LoadATransAnalAttrib(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber, Int32 ATransactionNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransAnalAttribAccess.LoadViaATransaction(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, ATransactionNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads some necessary analysis attributes tables for the given ledger number
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns>GLSetupTDS</returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadAAnalysisAttributes(Int32 ALedgerNumber)
        {
            GLSetupTDS CacheDS = new GLSetupTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AAnalysisTypeAccess.LoadAll(CacheDS, Transaction);
            AFreeformAnalysisAccess.LoadViaALedger(CacheDS, ALedgerNumber, Transaction);
            AAnalysisAttributeAccess.LoadViaALedger(CacheDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return CacheDS;
        }

        /// <summary>
        /// this will store all new and modified batches, journals, transactions
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveGLBatchTDS(ref GLBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            // TODO: calculate debit and credit sums for journal and batch?

            TSubmitChangesResult SubmissionResult = GLBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

            return SubmissionResult;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            return TGLPosting.PostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// <summary>
        /// return a string that shows the balances of the accounts involved, if the GL Batch was posted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
//        [RequireModulePermission("FINANCE-1")]
        public static List <TVariant>TestPostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            GLBatchTDS MainDS;
            bool success = TGLPosting.PostGLBatchInternal(ALedgerNumber, ABatchNumber, out AVerifications, out MainDS);

            List <TVariant>Result = new List <TVariant>();

            if (success)
            {
                MainDS.AGeneralLedgerMaster.DefaultView.RowFilter = string.Empty;
                MainDS.AAccount.DefaultView.RowFilter = string.Empty;
                MainDS.ACostCentre.DefaultView.RowFilter = string.Empty;
                MainDS.AGeneralLedgerMaster.DefaultView.Sort = AGeneralLedgerMasterTable.GetGlmSequenceDBName();
                MainDS.ACostCentre.DefaultView.Sort = ACostCentreTable.GetCostCentreCodeDBName();
                MainDS.AAccount.DefaultView.Sort = AAccountTable.GetAccountCodeDBName();

                foreach (AGeneralLedgerMasterPeriodRow glmRow in MainDS.AGeneralLedgerMasterPeriod.Rows)
                {
                    if ((glmRow.PeriodNumber == MainDS.ABatch[0].BatchPeriod) && (glmRow.RowState != DataRowState.Unchanged))
                    {
                        Int32 masterIndex = MainDS.AGeneralLedgerMaster.DefaultView.Find(glmRow.GlmSequence);

                        if (masterIndex == -1)
                        {
                            // not exactly sure why those records are missing, perhaps they would be zero?
                            // TLogging.Log("cannot find glm sequence " + glmRow.GlmSequence.ToString() + " in glm");
                            continue;
                        }

                        AGeneralLedgerMasterRow masterRow =
                            (AGeneralLedgerMasterRow)
                            MainDS.AGeneralLedgerMaster.DefaultView[masterIndex].Row;

                        ACostCentreRow ccRow =
                            (ACostCentreRow)
                            MainDS.ACostCentre.DefaultView[MainDS.ACostCentre.DefaultView.Find(masterRow.CostCentreCode)].Row;

                        // only consider the top level cost centre
                        if (ccRow.IsCostCentreToReportToNull())
                        {
                            AAccountRow accRow =
                                (AAccountRow)
                                MainDS.AAccount.DefaultView[MainDS.AAccount.DefaultView.Find(masterRow.AccountCode)].Row;

                            // only modified accounts have been loaded to the dataset, therefore report on all accounts available
                            if (accRow.PostingStatus)
                            {
                                decimal CurrentValue = 0.0m;

                                if (glmRow.RowState == DataRowState.Modified)
                                {
                                    CurrentValue = (decimal)glmRow[AGeneralLedgerMasterPeriodTable.ColumnActualBaseId, DataRowVersion.Original];
                                }

                                decimal DebitCredit = 1.0m;

                                if (accRow.DebitCreditIndicator && (accRow.AccountType != MFinanceConstants.ACCOUNT_TYPE_ASSET))
                                {
                                    DebitCredit = -1.0m;
                                }

                                // only return values, the client compiles the message, with Catalog.GetString
                                TVariant values = new TVariant(accRow.AccountCode);
                                values.Add(new TVariant(accRow.AccountCodeShortDesc), "", false);
                                values.Add(new TVariant(CurrentValue * DebitCredit), "", false);
                                values.Add(new TVariant(glmRow.ActualBase * DebitCredit), "", false);

                                Result.Add(values);
                            }
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// return the name of the standard costcentre for the given ledger;
        /// this supports up to 4 digit ledgers
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static string GetStandardCostCentre(Int32 ALedgerNumber)
        {
            return String.Format("{0:##00}00", ALedgerNumber);
        }

        /// <summary>
        /// get daily exchange rate for the given currencies and date;
        /// TODO: might even collect the latest exchange rate from the web
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="ADateEffective"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static decimal GetDailyExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime ADateEffective)
        {
            if (ACurrencyFrom == ACurrencyTo)
            {
                return 1.0M;
            }

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

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
                    rates.DefaultView.RowFilter = ADailyExchangeRateTable.GetDateEffectiveFromDBName() + "<= '" +
                                                  ADateEffective.ToString("dd/MM/yyyy") + "'";

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

            TLogging.Log("cannot find rate for " + ACurrencyFrom + " " + ACurrencyTo);

            return 1.0M;
        }

        /// <summary>
        /// get corporate exchange rate for the given currencies and date;
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <returns></returns>
        [RequireModulePermission("NONE")]
        public static decimal GetCorporateExchangeRate(string ACurrencyFrom, string ACurrencyTo, DateTime AStartDate, DateTime AEndDate)
        {
            decimal ExchangeRate = 1.0M;

            if (ACurrencyFrom == ACurrencyTo)
            {
                return ExchangeRate;
            }

            if (!GetCorporateExchangeRate(ACurrencyFrom, ACurrencyTo, AStartDate, AEndDate, out ExchangeRate))
            {
                if (!GetCorporateExchangeRateFromPreviousYears(ACurrencyFrom, ACurrencyTo, AStartDate, AEndDate, out ExchangeRate))
                {
                    ExchangeRate = 1.0M;
                    TLogging.Log("cannot find rate for " + ACurrencyFrom + " " + ACurrencyTo);
                }
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
        private static bool GetCorporateExchangeRate(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime AStartDate,
            DateTime AEndDate,
            out decimal AExchangeRate)
        {
            AExchangeRate = decimal.MinValue;

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            ACorporateExchangeRateTable tempTable = new ACorporateExchangeRateTable();
            ACorporateExchangeRateRow templateRow = tempTable.NewRowTyped(false);

            templateRow.FromCurrencyCode = ACurrencyFrom;
            templateRow.ToCurrencyCode = ACurrencyTo;

            ACorporateExchangeRateTable ExchangeRates = ACorporateExchangeRateAccess.LoadUsingTemplate(templateRow, Transaction);

            if (ExchangeRates.Count > 0)
            {
                // sort rates by date, look for rate just before the date we are looking for
                ExchangeRates.DefaultView.Sort = ACorporateExchangeRateTable.GetDateEffectiveFromDBName();
                ExchangeRates.DefaultView.RowFilter = ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + ">= '" +
                                                      AStartDate.ToString("dd/MM/yyyy") + "' AND " +
                                                      ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + "<= '" +
                                                      AEndDate.ToString("dd/MM/yyyy") + "'";

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
                    ExchangeRates.DefaultView.RowFilter = ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + ">= '" +
                                                          AStartDate.ToString("dd/MM/yyyy") + "' AND " +
                                                          ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + "<= '" +
                                                          AEndDate.ToString("dd/MM/yyyy") + "'";

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
        /// get corporate exchange rate from the previous years for the given currencies and date;
        /// </summary>
        /// <param name="ACurrencyFrom"></param>
        /// <param name="ACurrencyTo"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        /// <param name="AExchangeRate"></param>
        /// <returns>true if a exchange rate was found for the date. Otherwise false</returns>
        private static bool GetCorporateExchangeRateFromPreviousYears(string ACurrencyFrom,
            string ACurrencyTo,
            DateTime AStartDate,
            DateTime AEndDate,
            out decimal AExchangeRate)
        {
            AExchangeRate = decimal.MinValue;

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            APrevYearCorpExRateTable PrevTempTable = new APrevYearCorpExRateTable();
            APrevYearCorpExRateRow PrevTemplateRow = PrevTempTable.NewRowTyped(false);

            PrevTemplateRow.FromCurrencyCode = ACurrencyFrom;
            PrevTemplateRow.ToCurrencyCode = ACurrencyTo;

            APrevYearCorpExRateTable PrevExchangeRates = APrevYearCorpExRateAccess.LoadUsingTemplate(PrevTemplateRow, Transaction);

            if (PrevExchangeRates.Count > 0)
            {
                // sort rates by date, look for rate just before the date we are looking for
                PrevExchangeRates.DefaultView.Sort = APrevYearCorpExRateTable.GetDateEffectiveFromDBName();
                PrevExchangeRates.DefaultView.RowFilter = APrevYearCorpExRateTable.GetDateEffectiveFromDBName() + ">= '" +
                                                          AStartDate.ToString("dd/MM/yyyy") + "' AND " +
                                                          APrevYearCorpExRateTable.GetDateEffectiveFromDBName() + "<= '" +
                                                          AEndDate.ToString("dd/MM/yyyy") + "'";

                if (PrevExchangeRates.DefaultView.Count > 0)
                {
                    AExchangeRate = ((APrevYearCorpExRateRow)PrevExchangeRates.DefaultView[0].Row).RateOfExchange;
                }
            }

            if (AExchangeRate == decimal.MinValue)
            {
                // try other way round
                PrevTemplateRow.FromCurrencyCode = ACurrencyTo;
                PrevTemplateRow.ToCurrencyCode = ACurrencyFrom;

                PrevExchangeRates = APrevYearCorpExRateAccess.LoadUsingTemplate(PrevTemplateRow, Transaction);

                if (PrevExchangeRates.Count > 0)
                {
                    // sort rates by date, look for rate just before the date we are looking for
                    PrevExchangeRates.DefaultView.Sort = APrevYearCorpExRateTable.GetDateEffectiveFromDBName();
                    PrevExchangeRates.DefaultView.RowFilter = APrevYearCorpExRateTable.GetDateEffectiveFromDBName() + ">= '" +
                                                              AStartDate.ToString("dd/MM/yyyy") + "' AND " +
                                                              APrevYearCorpExRateTable.GetDateEffectiveFromDBName() + "<= '" +
                                                              AEndDate.ToString("dd/MM/yyyy") + "'";

                    if (PrevExchangeRates.DefaultView.Count > 0)
                    {
                        AExchangeRate = 1 / ((APrevYearCorpExRateRow)PrevExchangeRates.DefaultView[0].Row).RateOfExchange;
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
        /// cancel a GL Batch
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-3")]
        public static bool CancelGLBatch(out GLBatchTDS MainDS,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            return TGLPosting.CancelGLBatch(out MainDS, ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches"></param>
        /// <param name="requestParams"></param>
        /// <param name="exportString"></param>
        /// <returns>false if batch does not exist at all</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ExportAllGLBatchData(ref ArrayList batches, Hashtable requestParams, out String exportString)
        {
            TGLExporting exporting = new TGLExporting();

            return exporting.ExportAllGLBatchData(ref batches, requestParams, out exportString);
        }

        /// <summary>
        /// Import GL batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">Big parts of the import file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportGLBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            TGLImporting importing = new TGLImporting();

            return importing.ImportGLBatches(requestParams, importString, out AMessages);
        }
    }
}