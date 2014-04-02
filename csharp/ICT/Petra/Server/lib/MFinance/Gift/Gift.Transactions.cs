//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christophert
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
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Gift screens
    ///</summary>
    public partial class TGiftTransactionWebConnector
    {
        /// <summary>
        /// Create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber, DateTime ADateEffective, string ABatchDescription)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction ReadWriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ReadWriteTransaction);

                TGiftBatchFunctions.CreateANewGiftBatchRow(ref MainDS, ref ReadWriteTransaction, ref LedgerTable, ALedgerNumber, ADateEffective);
                MainDS.AGiftBatch[0].BatchDescription = ABatchDescription;

                AGiftBatchAccess.SubmitChanges(MainDS.AGiftBatch, ReadWriteTransaction);

                ALedgerAccess.SubmitChanges(LedgerTable, ReadWriteTransaction);

                MainDS.AGiftBatch.AcceptChanges();

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the creation of a Gift Batch record:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber)
        {
            return CreateAGiftBatch(ALedgerNumber, DateTime.Today, Catalog.GetString("Please enter batch description"));
        }

        /// <summary>
        /// create a new recurring batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS CreateARecurringGiftBatch(Int32 ALedgerNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction ReadWriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ReadWriteTransaction);

                TGiftBatchFunctions.CreateANewRecurringGiftBatchRow(ref MainDS, ref ReadWriteTransaction, ref LedgerTable, ALedgerNumber);

                ARecurringGiftBatchAccess.SubmitChanges(MainDS.ARecurringGiftBatch, ReadWriteTransaction);

                ALedgerAccess.SubmitChanges(LedgerTable, ReadWriteTransaction);

                MainDS.ARecurringGiftBatch.AcceptChanges();

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the creation of a Recurring Gift Batch record:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }

            return MainDS;
        }

        /// <summary>
        /// create a gift batch from a recurring gift batch
        /// including gift and gift detail
        /// </summary>
        /// <param name="requestParams">HashTable with many parameters</param>
        [RequireModulePermission("FINANCE-1")]
        public static void SubmitRecurringGiftBatch(Hashtable requestParams)
        {
            GiftBatchTDS GMainDS = new GiftBatchTDS();
            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Int32 ABatchNumber = (Int32)requestParams["ABatchNumber"];
            DateTime AEffectiveDate = (DateTime)requestParams["AEffectiveDate"];
            Decimal AExchangeRateToBase = (Decimal)requestParams["AExchangeRateToBase"];

            GiftBatchTDS RMainDS = LoadRecurringTransactions(ALedgerNumber, ABatchNumber);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                ARecurringGiftBatchAccess.LoadByPrimaryKey(RMainDS, ALedgerNumber, ABatchNumber, Transaction);

                // Assuming all relevant data is loaded in FMainDS
                foreach (ARecurringGiftBatchRow recBatch  in RMainDS.ARecurringGiftBatch.Rows)
                {
                    if ((recBatch.BatchNumber == ABatchNumber) && (recBatch.LedgerNumber == ALedgerNumber))
                    {
                        Decimal batchTotal = 0;
                        AGiftBatchRow batch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref GMainDS,
                            ref Transaction,
                            ref LedgerTable,
                            ALedgerNumber,
                            AEffectiveDate);

                        batch.BatchDescription = recBatch.BatchDescription;
                        batch.BankCostCentre = recBatch.BankCostCentre;
                        batch.BankAccountCode = recBatch.BankAccountCode;
                        batch.ExchangeRateToBase = AExchangeRateToBase;
                        batch.MethodOfPaymentCode = recBatch.MethodOfPaymentCode;
                        batch.GiftType = recBatch.GiftType;
                        //batch.HashTotal = recBatch.HashTotal; // Does this  make sense? Active Gifts are not
                        batch.CurrencyCode = recBatch.CurrencyCode;

                        foreach (ARecurringGiftRow recGift in RMainDS.ARecurringGift.Rows)
                        {
                            if ((recGift.BatchNumber == ABatchNumber) && (recGift.LedgerNumber == ALedgerNumber) && recGift.Active)
                            {
                                //Look if there is a detail which is in the donation period (else continue)
                                bool foundDetail = false;

                                foreach (ARecurringGiftDetailRow recGiftDetail in RMainDS.ARecurringGiftDetail.Rows)
                                {
                                    if ((recGiftDetail.GiftTransactionNumber == recGift.GiftTransactionNumber)
                                        && (recGiftDetail.BatchNumber == ABatchNumber) && (recGiftDetail.LedgerNumber == ALedgerNumber)
                                        && ((recGiftDetail.StartDonations == null) || (recGiftDetail.StartDonations <= DateTime.Today))
                                        && ((recGiftDetail.EndDonations == null) || (recGiftDetail.EndDonations >= DateTime.Today))
                                        )
                                    {
                                        foundDetail = true;
                                        break;
                                    }
                                }

                                if (!foundDetail)
                                {
                                    continue;
                                }

                                // make the gift from recGift
                                AGiftRow gift = GMainDS.AGift.NewRowTyped();
                                gift.LedgerNumber = batch.LedgerNumber;
                                gift.BatchNumber = batch.BatchNumber;
                                gift.GiftTransactionNumber = batch.LastGiftNumber + 1;
                                gift.DonorKey = recGift.DonorKey;
                                gift.MethodOfGivingCode = recGift.MethodOfGivingCode;

                                if (gift.MethodOfGivingCode.Length == 0)
                                {
                                    gift.SetMethodOfGivingCodeNull();
                                }

                                gift.MethodOfPaymentCode = recGift.MethodOfPaymentCode;

                                if (gift.MethodOfPaymentCode.Length == 0)
                                {
                                    gift.SetMethodOfPaymentCodeNull();
                                }

                                gift.Reference = recGift.Reference;
                                gift.ReceiptLetterCode = recGift.ReceiptLetterCode;


                                GMainDS.AGift.Rows.Add(gift);
                                batch.LastGiftNumber++;
                                //TODO (not here, but in the client or while posting) Check for Ex-OM Partner
                                //TODO (not here, but in the client or while posting) Check for expired key ministry (while Posting)

                                foreach (ARecurringGiftDetailRow recGiftDetail in RMainDS.ARecurringGiftDetail.Rows)
                                {
                                    if ((recGiftDetail.GiftTransactionNumber == recGift.GiftTransactionNumber)
                                        && (recGiftDetail.BatchNumber == ABatchNumber) && (recGiftDetail.LedgerNumber == ALedgerNumber)
                                        && ((recGiftDetail.StartDonations == null) || (recGiftDetail.StartDonations <= DateTime.Today))
                                        && ((recGiftDetail.EndDonations == null) || (recGiftDetail.EndDonations >= DateTime.Today))
                                        )
                                    {
                                        AGiftDetailRow detail = GMainDS.AGiftDetail.NewRowTyped();
                                        detail.LedgerNumber = gift.LedgerNumber;
                                        detail.BatchNumber = gift.BatchNumber;
                                        detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                                        detail.DetailNumber = gift.LastDetailNumber + 1;
                                        gift.LastDetailNumber++;

                                        detail.GiftTransactionAmount = recGiftDetail.GiftAmount;
                                        batchTotal += recGiftDetail.GiftAmount;
                                        detail.RecipientKey = recGiftDetail.RecipientKey;
                                        //maybe that this is unused
                                        detail.RecipientLedgerNumber = recGiftDetail.RecipientLedgerNumber;
                                        detail.ChargeFlag = recGiftDetail.ChargeFlag;
                                        detail.ConfidentialGiftFlag = recGiftDetail.ConfidentialGiftFlag;
                                        detail.TaxDeductible = recGiftDetail.TaxDeductible;
                                        detail.MailingCode = recGiftDetail.MailingCode;

                                        if (detail.MailingCode.Length == 0)
                                        {
                                            detail.SetMailingCodeNull();
                                        }

                                        // TODO convert with exchange rate to get the amount in base currency
                                        // detail.GiftAmount=

                                        detail.MotivationGroupCode = recGiftDetail.MotivationGroupCode;
                                        detail.MotivationDetailCode = recGiftDetail.MotivationDetailCode;
                                        detail.GiftCommentOne = recGiftDetail.GiftCommentOne;
                                        detail.CommentOneType = recGiftDetail.CommentOneType;
                                        detail.GiftCommentTwo = recGiftDetail.GiftCommentTwo;
                                        detail.CommentTwoType = recGiftDetail.CommentTwoType;
                                        detail.GiftCommentThree = recGiftDetail.GiftCommentThree;
                                        detail.CommentThreeType = recGiftDetail.CommentThreeType;


                                        GMainDS.AGiftDetail.Rows.Add(detail);
                                    }
                                }

                                batch.BatchTotal = batchTotal;
                            }
                        }
                    }
                }

                AGiftBatchAccess.SubmitChanges(GMainDS.AGiftBatch, Transaction);

                ALedgerAccess.SubmitChanges(LedgerTable, Transaction);

                AGiftAccess.SubmitChanges(GMainDS.AGift, Transaction);

                AGiftDetailAccess.SubmitChanges(GMainDS.AGiftDetail, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();

                GMainDS.AcceptChanges();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the submission of a Recurring Gift Batch:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                GMainDS.RejectChanges();

                throw;
            }
        }

        /// <summary>
        /// Loads all available years with gift data into a table
        /// To be used by a combobox to select the financial year
        ///
        /// </summary>
        /// <returns>DataTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static DataTable GetAvailableGiftYears(Int32 ALedgerNumber, out String ADisplayMember, out String AValueMember)
        {
            ADisplayMember = "YearDate";
            AValueMember = "YearNumber";
            DataTable tab = new DataTable();
            tab.Columns.Add(AValueMember, typeof(System.Int32));
            tab.Columns.Add(ADisplayMember, typeof(String));
            tab.PrimaryKey = new DataColumn[] {
                tab.Columns[0]
            };

            System.Type typeofTable = null;
            TCacheable CachePopulator = new TCacheable();
            ALedgerTable LedgerTable = (ALedgerTable)CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.LedgerDetails,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            AAccountingPeriodTable AccountingPeriods = (AAccountingPeriodTable)CachePopulator.GetCacheableTable(
                TCacheableFinanceTablesEnum.AccountingPeriodList,
                "",
                false,
                ALedgerNumber,
                out typeofTable);

            AAccountingPeriodRow currentYearEndPeriod =
                (AAccountingPeriodRow)AccountingPeriods.Rows.Find(new object[] { ALedgerNumber, LedgerTable[0].NumberOfAccountingPeriods });
            DateTime currentYearEnd = currentYearEndPeriod.PeriodEndDate;

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction();
            try
            {
                // add the years, which are retrieved by reading from the gift batch tables
                string sql =
                    String.Format("SELECT DISTINCT {0} AS availYear " + " FROM PUB_{1} " + " WHERE {2} = " +
                        ALedgerNumber.ToString() + " ORDER BY 1 DESC",
                        AGiftBatchTable.GetBatchYearDBName(),
                        AGiftBatchTable.GetTableDBName(),
                        AGiftBatchTable.GetLedgerNumberDBName());

                DataTable BatchYearTable = DBAccess.GDBAccessObj.SelectDT(sql, "BatchYearTable", ReadTransaction);

                foreach (DataRow row in BatchYearTable.Rows)
                {
                    DataRow resultRow = tab.NewRow();
                    resultRow[0] = row[0];
                    resultRow[1] = currentYearEnd.AddYears(-1 * (LedgerTable[0].CurrentFinancialYear - Convert.ToInt32(row[0]))).ToString("yyyy");
                    tab.Rows.Add(resultRow);
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // we should also check if the current year has been added, in case there are no gift batches yet
            if (null == tab.Rows.Find(LedgerTable[0].CurrentFinancialYear))
            {
                DataRow resultRow = tab.NewRow();
                resultRow[0] = LedgerTable[0].CurrentFinancialYear;
                resultRow[1] = currentYearEnd.ToString("yyyy");
                tab.Rows.InsertAt(resultRow, 0);
            }

            return tab;
        }

        /// <summary>
        /// loads a list of batches for the given ledger
        /// also get the ledger for the base currency etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchStatus"></param>
        /// <param name="AYear">if -1, the year will be ignored</param>
        /// <param name="APeriod">if AYear is -1 or period is -1, the period will be ignored.
        /// if APeriod is 0 and the current year is selected, then the current and the forwarding periods are used</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadAGiftBatch(Int32 ALedgerNumber, string ABatchStatus, Int32 AYear, Int32 APeriod)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            StringCollection templateOperators = new StringCollection();

            AGiftBatchRow templateRow = MainDS.AGiftBatch.NewRowTyped(false);
            templateRow.LedgerNumber = ALedgerNumber;

            if ((ABatchStatus != null) && (ABatchStatus.Length > 0))
            {
                templateRow.BatchStatus = ABatchStatus;
                templateOperators.Add("=");
            }

            if (AYear != -1)
            {
                templateRow.BatchYear = AYear;
                templateOperators.Add("=");

                if ((APeriod == 0) && (AYear == MainDS.ALedger[0].CurrentFinancialYear))
                {
                    templateRow.BatchPeriod = MainDS.ALedger[0].CurrentPeriod;
                    templateOperators.Add(">=");
                }
                else if (APeriod != -1)
                {
                    templateRow.BatchPeriod = APeriod;
                    templateOperators.Add("=");
                }
            }

            AGiftBatchAccess.LoadUsingTemplate(MainDS, templateRow, templateOperators, null, Transaction, null, 0, 0);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring batches for the given ledger
        /// also get the ledger for the base currency etc
        /// TODO: limit to period, limit to batch status, etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadARecurringGiftBatch(Int32 ALedgerNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
            ARecurringGiftBatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of gift transactions and details for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool BatchStatusUnposted;
            bool NewTransaction = false;
            string FailedUpdates = string.Empty;

            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            //Load Ledger & Motivation Data to allow updating of CostCentreCode
            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            MainDS.Merge(LoadGiftBatchData(ALedgerNumber, ABatchNumber));

            //Find the batch status
            BatchStatusUnposted = (MainDS.AGiftBatch[0].BatchStatus == MFinanceConstants.BATCH_UNPOSTED);

            if (BatchStatusUnposted)
            {
                if (!UpdateCostCentreCodeForRecipients(ref MainDS, out FailedUpdates, ABatchNumber))
                {
                    TLogging.Log(String.Format("Updating Cost Centre Codes For Recipients in Ledger {0} and Batch {1} failed:{2}  {3}",
                            ALedgerNumber,
                            ABatchNumber,
                            Environment.NewLine,
                            FailedUpdates));
                }
            }

            // drop all tables apart from AGift and AGiftDetail
            foreach (DataTable table in MainDS.Tables)
            {
                if ((table.TableName != MainDS.AGift.TableName) && (table.TableName != MainDS.AGiftDetail.TableName))
                {
                    table.Clear();
                }
            }

            //Add a temp table
            if (FailedUpdates.Length > 0)
            {
                DataTable table = new DataTable();
                table.TableName = "AUpdateErrors";
                table.Columns.Add("UpdateError", typeof(string));
                table.Rows.Add(FailedUpdates);

                MainDS.Tables.Add(table);
            }

            return MainDS;
        }

        /// <summary>
        /// Update the cost centres for the recipients
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AFailedUpdates"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AGiftTransactionNumber"></param>
        /// <param name="AGiftDetailNumber"></param>
        /// <returns>Return true if no errors occurred else check value of out AFailedUpdates</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool UpdateCostCentreCodeForRecipients(ref GiftBatchTDS AMainDS,
            out string AFailedUpdates,
            Int32 ABatchNumber,
            Int32 AGiftTransactionNumber = 0,
            Int32 AGiftDetailNumber = 0)
        {
            AFailedUpdates = string.Empty;

            if ((AMainDS.AGiftBatch.Count == 0) || (AMainDS.AGift.Count == 0))
            {
                return true;
            }

            int LedgerNumber = AMainDS.ALedger[0].LedgerNumber;
            Int64 LedgerPartnerKey = AMainDS.ALedger[0].PartnerKey;

            string CurrentCostCentreCode = string.Empty;
            string NewCostCentreCode = string.Empty;

            string MotivationGroup = string.Empty;
            string MotivationDetail = string.Empty;

            Int64 PartnerKey = 0;
            Int64 RecipientLedgerNumber = 0;

            bool KeyMinIsActive = false;
            bool KeyMinExists = false;

            string ValidLedgerNumberCostCentreCode = string.Empty;
            //bool ValidLedgerNumberExists = false;

            string ErrMsg = string.Empty;

            string RowFilterForGifts = string.Empty;

            if (AGiftTransactionNumber > 0)
            {
                RowFilterForGifts = String.Format("{0}={1} And {2}={3} And {4}={5}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber,
                    AGiftDetailTable.GetGiftTransactionNumberDBName(),
                    AGiftTransactionNumber,
                    AGiftDetailTable.GetDetailNumberDBName(),
                    AGiftDetailNumber);
            }
            else
            {
                RowFilterForGifts = String.Format("{0}={1}",
                    AGiftDetailTable.GetBatchNumberDBName(),
                    ABatchNumber);
            }

            DataView giftRowsView = new DataView(AMainDS.AGiftDetail);
            giftRowsView.RowFilter = RowFilterForGifts;

            foreach (DataRowView dvRows in giftRowsView)
            {
                AGiftDetailRow giftRow = (AGiftDetailRow)dvRows.Row;

                CurrentCostCentreCode = giftRow.CostCentreCode;
                NewCostCentreCode = CurrentCostCentreCode;

                MotivationGroup = giftRow.MotivationGroupCode;
                MotivationDetail = giftRow.MotivationDetailCode;

                PartnerKey = giftRow.RecipientKey;
                RecipientLedgerNumber = giftRow.RecipientLedgerNumber;

                KeyMinIsActive = false;
                KeyMinExists = KeyMinistryExists(PartnerKey, out KeyMinIsActive);

                //ValidLedgerNumberExists = CheckCostCentreLinkForRecipient(LedgerNumber,
                //    PartnerKey,
                //    out ValidLedgerNumberCostCentreCode);

                if (CheckCostCentreLinkForRecipient(LedgerNumber, PartnerKey,
                        out ValidLedgerNumberCostCentreCode)
                    || CheckCostCentreLinkForRecipient(LedgerNumber, RecipientLedgerNumber,
                        out ValidLedgerNumberCostCentreCode))
                {
                    NewCostCentreCode = ValidLedgerNumberCostCentreCode;
                }
                else if ((RecipientLedgerNumber != LedgerPartnerKey) && ((MotivationGroup == "GIFT") || KeyMinExists))
                {
                    ErrMsg = String.Format(
                        "Error in extracting Cost Centre Code for Recipient: {0} in Ledger: {1}.{2}{2}(Recipient Ledger Number: {3}, Ledger Partner Key: {4})",
                        PartnerKey,
                        LedgerNumber,
                        Environment.NewLine,
                        RecipientLedgerNumber,
                        LedgerPartnerKey);

                    TLogging.Log("Cost Centre Code Error: " + ErrMsg);
                }
                else
                {
                    AMotivationDetailRow motivationDetail = (AMotivationDetailRow)AMainDS.AMotivationDetail.Rows.Find(
                        new object[] { LedgerNumber, MotivationGroup, MotivationDetail });

                    if (motivationDetail != null)
                    {
                        NewCostCentreCode = motivationDetail.CostCentreCode.ToString();
                    }
                    else
                    {
                        ErrMsg = String.Format(
                            "Error in extracting Cost Centre Code for Motivation Group: {0} and Motivation Detail: {1} in Ledger: {2}.",
                            MotivationGroup,
                            MotivationDetail,
                            LedgerNumber);

                        TLogging.Log("Cost Centre Code Error: " + ErrMsg);
                    }
                }

                if (CurrentCostCentreCode != NewCostCentreCode)
                {
                    giftRow.CostCentreCode = NewCostCentreCode;
                }

                if (ErrMsg.Length > 0)
                {
                    if (AFailedUpdates.Length > 0)
                    {
                        AFailedUpdates += (Environment.NewLine + Environment.NewLine);
                    }

                    AFailedUpdates += ErrMsg;
                    ErrMsg = string.Empty;
                }
            }

            return AFailedUpdates.Length == 0;
        }

        /// <summary>
        /// loads a list of recurring gift transactions and details for the given ledger and recurring batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadRecurringTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GiftBatchTDS MainDS = LoadRecurringGiftBatchData(ALedgerNumber, ABatchNumber);

            // drop all tables apart from ARecurringGift and ARecurringGiftDetail
            foreach (DataTable table in MainDS.Tables)
            {
                if ((table.TableName != MainDS.ARecurringGift.TableName) && (table.TableName != MainDS.ARecurringGiftDetail.TableName))
                {
                    table.Clear();
                }
            }

            return MainDS;
        }

        /// <summary>
        /// loads a list of gift transactions and details for the given ledger and batch
        /// </summary>
        /// <param name="requestParams"></param>
        /// <param name="AMessages"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadDonorRecipientHistory(Hashtable requestParams,
            out TVerificationResultCollection AMessages)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = null;

            AMessages = new TVerificationResultCollection();

            string tempTableName = (string)requestParams["TempTable"];
            Int32 ledgerNumber = (Int32)requestParams["Ledger"];
            long recipientKey = (Int64)requestParams["Recipient"];
            long donorKey = (Int64)requestParams["Donor"];

            string motivationGroup = (string)requestParams["MotivationGroup"];
            string motivationDetail = (string)requestParams["MotivationDetail"];

            string dateFrom = (string)requestParams["DateFrom"];
            string dateTo = (string)requestParams["DateTo"];
            DateTime startDate;
            DateTime endDate;

            bool noDates = (dateFrom.Length == 0 && dateTo.Length == 0);

            string sqlStmt = string.Empty;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                sqlStmt = TDataBase.ReadSqlFile("Gift.GetDonationsOfDonorAndOrRecipientTemplate.sql");

                OdbcParameter param;

                List <OdbcParameter>parameters = new List <OdbcParameter>();

                param = new OdbcParameter("LedgerNumber", OdbcType.Int);
                param.Value = ledgerNumber;
                parameters.Add(param);
                param = new OdbcParameter("DonorAny", OdbcType.Bit);
                param.Value = (donorKey == 0);
                parameters.Add(param);
                param = new OdbcParameter("DonorKey", OdbcType.BigInt);
                param.Value = donorKey;
                parameters.Add(param);
                param = new OdbcParameter("RecipientAny", OdbcType.Bit);
                param.Value = (recipientKey == 0);
                parameters.Add(param);
                param = new OdbcParameter("RecipientKey", OdbcType.BigInt);
                param.Value = recipientKey;
                parameters.Add(param);

                noDates = (dateFrom.Length == 0 && dateTo.Length == 0);
                param = new OdbcParameter("DateAny", OdbcType.Bit);
                param.Value = noDates;
                parameters.Add(param);

                if (noDates)
                {
                    //These values don't matter because of the value of noDate
                    startDate = new DateTime(2000, 1, 1);
                    endDate = new DateTime(2000, 1, 1);
                }
                else if ((dateFrom.Length > 0) && (dateTo.Length > 0))
                {
                    startDate = Convert.ToDateTime(dateFrom);     //, new CultureInfo("en-US"));
                    endDate = Convert.ToDateTime(dateTo);     //, new CultureInfo("en-US"));
                }
                else if (dateFrom.Length > 0)
                {
                    startDate = Convert.ToDateTime(dateFrom);
                    endDate = new DateTime(2050, 1, 1);
                }
                else
                {
                    startDate = new DateTime(1965, 1, 1);
                    endDate = Convert.ToDateTime(dateTo);
                }

                param = new OdbcParameter("DateFrom", OdbcType.Date);
                param.Value = startDate;
                parameters.Add(param);
                param = new OdbcParameter("DateTo", OdbcType.Date);
                param.Value = endDate;
                parameters.Add(param);
                param = new OdbcParameter("MotivationGroupAny", OdbcType.Bit);
                param.Value = (motivationGroup.Length == 0);
                parameters.Add(param);
                param = new OdbcParameter("MotivationGroupCode", OdbcType.VarChar);
                param.Value = motivationGroup;
                parameters.Add(param);
                param = new OdbcParameter("MotivationDetailAny", OdbcType.Bit);
                param.Value = (motivationDetail.Length == 0);
                parameters.Add(param);
                param = new OdbcParameter("MotivationDetailCode", OdbcType.VarChar);
                param.Value = motivationDetail;
                parameters.Add(param);

                //Load Ledger Table
                ALedgerAccess.LoadByPrimaryKey(MainDS, ledgerNumber, Transaction);

                //Can do this if needed: MainDS.DisableConstraints();
                DBAccess.GDBAccessObj.SelectToTempTable(MainDS, sqlStmt, tempTableName, Transaction, parameters.ToArray(), 0, 0);

                MainDS.Tables[tempTableName].Columns.Add("DonorDescription");

                foreach (DataRow Row in MainDS.Tables[tempTableName].Rows)
                {
                    PPartnerTable Tbl = PPartnerAccess.LoadByPrimaryKey(Convert.ToInt64(Row["DonorKey"]), Transaction);
                    Row["DonorDescription"] = Tbl[0].PartnerShortName;
                }

                MainDS.AcceptChanges();
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return MainDS;
        }

        /// <summary>
        /// this will store all new and modified batches, gift transactions and details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveGiftBatchTDS(ref GiftBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            AVerificationResult = new TVerificationResultCollection();

            // make sure that empty tables are removed !! This can return NULL!
            AInspectDS = AInspectDS.GetChangesTyped(true);

            if (AInspectDS == null)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Save Gift Batch"),
                        Catalog.GetString("No changes - nothing to do"),
                        TResultSeverity.Resv_Info));
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            bool AllValidationsOK = true;

            bool giftBatchTableInDataSet = (AInspectDS.AGiftBatch != null);
            bool giftTableInDataSet = (AInspectDS.AGift != null);
            bool giftDetailTableInDataSet = (AInspectDS.AGiftDetail != null);
            bool recurrGiftBatchTableInDataSet = (AInspectDS.ARecurringGiftBatch != null);
            bool recurrGiftTableInDataSet = (AInspectDS.ARecurringGift != null);
            bool recurrGiftDetailTableInDataSet = (AInspectDS.ARecurringGiftDetail != null);

            if (recurrGiftBatchTableInDataSet || recurrGiftTableInDataSet || recurrGiftDetailTableInDataSet)
            {
                if (giftBatchTableInDataSet || giftTableInDataSet || giftDetailTableInDataSet)
                {
                    throw new Exception(
                        "SaveGiftBatchTDS: need to call GetChangesTyped before saving, otherwise confusion about recurring or normal gl batch");
                }

                return SaveRecurringGiftBatchTDS(ref AInspectDS, ref AVerificationResult);
            }

            if (giftBatchTableInDataSet)
            {
                ValidateGiftBatch(ref AVerificationResult, AInspectDS.AGiftBatch);
                ValidateGiftBatchManual(ref AVerificationResult, AInspectDS.AGiftBatch);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    AllValidationsOK = false;
                }
            }

            if (giftDetailTableInDataSet)
            {
                ValidateGiftDetail(ref AVerificationResult, AInspectDS.AGiftDetail);
                ValidateGiftDetailManual(ref AVerificationResult, AInspectDS.AGiftDetail);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    AllValidationsOK = false;
                }
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            if (AllValidationsOK)
            {
                GiftBatchTDSAccess.SubmitChanges(AInspectDS);

                SubmissionResult = TSubmitChangesResult.scrOK;

                if (giftTableInDataSet)
                {
                    if (giftDetailTableInDataSet)
                    {
                        AInspectDS.AGiftDetail.AcceptChanges();
                    }

                    AInspectDS.AGift.AcceptChanges();

                    if (AInspectDS.AGift.Count > 0)
                    {
                        AGiftRow tranR = (AGiftRow)AInspectDS.AGift.Rows[0];

                        Int32 currentLedger = tranR.LedgerNumber;
                        Int32 currentBatch = tranR.BatchNumber;
                        Int32 giftToDelete = 0;

                        try
                        {
                            DataRow[] foundGiftsForDeletion = AInspectDS.AGift.Select(String.Format("{0} = '{1}'",
                                    AGiftTable.GetGiftStatusDBName(),
                                    MFinanceConstants.MARKED_FOR_DELETION));

                            if (foundGiftsForDeletion.Length > 0)
                            {
                                AGiftRow giftRowClient = null;

                                for (int i = 0; i < foundGiftsForDeletion.Length; i++)
                                {
                                    //A gift has been deleted
                                    giftRowClient = (AGiftRow)foundGiftsForDeletion[i];

                                    giftToDelete = giftRowClient.GiftTransactionNumber;
                                    TLogging.Log(String.Format("Gift to Delete: {0} from Batch: {1}",
                                            giftToDelete,
                                            currentBatch));

                                    giftRowClient.Delete();
                                }
                            }

                            GiftBatchTDSAccess.SubmitChanges(AInspectDS);

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        catch (Exception ex)
                        {
                            TLogging.Log("Saving DataSet: " + ex.Message);

                            TLogging.Log(String.Format("Error trying to save changes: {0} in Batch: {1}",
                                    giftToDelete,
                                    currentBatch
                                    ));

                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                }
            }

            return SubmissionResult;
        }

        private bool CheckGiftNumbersAreConsecutive()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// this will store all new and modified recurring batches, recurring gift transactions and recurring details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        private static TSubmitChangesResult SaveRecurringGiftBatchTDS(ref GiftBatchTDS AInspectDS,
            ref TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            bool AllValidationsOK = true;

            bool recurrGiftBatchTableInDataSet = (AInspectDS.ARecurringGiftBatch != null);
            bool recurrGiftTableInDataSet = (AInspectDS.ARecurringGift != null);
            bool recurrGiftDetailTableInDataSet = (AInspectDS.ARecurringGiftDetail != null);

            if (recurrGiftBatchTableInDataSet)
            {
                ValidateRecurringGiftBatch(ref AVerificationResult, AInspectDS.ARecurringGiftBatch);
                ValidateRecurringGiftBatchManual(ref AVerificationResult, AInspectDS.ARecurringGiftBatch);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    AllValidationsOK = false;
                }
            }

            if (recurrGiftDetailTableInDataSet)
            {
                ValidateRecurringGiftDetail(ref AVerificationResult, AInspectDS.ARecurringGiftDetail);
                ValidateRecurringGiftDetailManual(ref AVerificationResult, AInspectDS.ARecurringGiftDetail);

                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                {
                    AllValidationsOK = false;
                }
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            if (AllValidationsOK)
            {
                GiftBatchTDSAccess.SubmitChanges(AInspectDS);

                SubmissionResult = TSubmitChangesResult.scrOK;

                if (recurrGiftTableInDataSet && (AInspectDS.ARecurringGift.Count > 0))
                {
                    if (recurrGiftBatchTableInDataSet)
                    {
                        AInspectDS.ARecurringGiftBatch.AcceptChanges();
                    }

                    if (recurrGiftDetailTableInDataSet)
                    {
                        AInspectDS.ARecurringGiftDetail.AcceptChanges();
                    }

                    AInspectDS.ARecurringGift.AcceptChanges();

                    if (AInspectDS.ARecurringGift.Count > 0)
                    {
                        ARecurringGiftRow tranR = (ARecurringGiftRow)AInspectDS.ARecurringGift.Rows[0];

                        Int32 currentLedger = tranR.LedgerNumber;
                        Int32 currentBatch = tranR.BatchNumber;
                        Int32 giftToDelete = 0;

                        try
                        {
                            DataRow[] foundGiftsForDeletion = AInspectDS.ARecurringGift.Select(String.Format("{0} = '{1}'",
                                    ARecurringGiftTable.GetChargeStatusDBName(),
                                    MFinanceConstants.MARKED_FOR_DELETION));

                            if (foundGiftsForDeletion.Length > 0)
                            {
                                ARecurringGiftRow giftRowClient = null;

                                for (int i = 0; i < foundGiftsForDeletion.Length; i++)
                                {
                                    //A gift has been deleted
                                    giftRowClient = (ARecurringGiftRow)foundGiftsForDeletion[i];

                                    giftToDelete = giftRowClient.GiftTransactionNumber;
                                    TLogging.Log(String.Format("Gift to Delete: {0} from Batch: {1}",
                                            giftToDelete,
                                            currentBatch));

                                    giftRowClient.Delete();
                                }
                            }

                            GiftBatchTDSAccess.SubmitChanges(AInspectDS);

                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        catch (Exception ex)
                        {
                            TLogging.Log("Saving DataSet: " + ex.Message);

                            TLogging.Log(String.Format("Error trying to save changes: {0} in Batch: {1}",
                                    giftToDelete,
                                    currentBatch
                                    ));

                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// creates the GL batch needed for posting the gift batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGiftDataset"></param>
        /// <returns></returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPostingGifts(Int32 ALedgerNumber, ref GiftBatchTDS AGiftDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = TGLTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            AGiftBatchRow giftBatch = AGiftDataset.AGiftBatch[0];

            batch.BatchDescription = Catalog.GetString("Gift Batch " + giftBatch.BatchNumber.ToString());
            batch.DateEffective = giftBatch.GlEffectiveDate;
            batch.GiftBatchNumber = giftBatch.BatchNumber;

            // TODO batchperiod depending on date effective; or fix that when posting?
            // batch.BatchPeriod =
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // one gift batch only has one currency, create only one journal
            AJournalRow journal = GLDataset.AJournal.NewRowTyped();
            journal.LedgerNumber = batch.LedgerNumber;
            journal.BatchNumber = batch.BatchNumber;
            journal.JournalNumber = 1;
            journal.DateEffective = batch.DateEffective;
            journal.JournalPeriod = giftBatch.BatchPeriod;
            journal.TransactionCurrency = giftBatch.CurrencyCode;
            journal.JournalDescription = batch.BatchDescription;
            journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.GR.ToString();
            journal.SubSystemCode = CommonAccountingSubSystemsEnum.GR.ToString();
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;

            // TODO journal.ExchangeRateToBase and journal.ExchangeRateTime
            journal.ExchangeRateToBase = 1.0M;
            GLDataset.AJournal.Rows.Add(journal);

            foreach (GiftBatchTDSAGiftDetailRow giftdetail in AGiftDataset.AGiftDetail.Rows)
            {
                ATransactionRow transaction = null;

                // do we have already a transaction for this costcentre&account?
                GLDataset.ATransaction.DefaultView.RowFilter = String.Format("{0}='{1}' and {2}='{3}'",
                    ATransactionTable.GetAccountCodeDBName(),
                    giftdetail.AccountCode,
                    ATransactionTable.GetCostCentreCodeDBName(),
                    giftdetail.CostCentreCode);

                if (GLDataset.ATransaction.DefaultView.Count == 0)
                {
                    transaction = GLDataset.ATransaction.NewRowTyped();
                    transaction.LedgerNumber = journal.LedgerNumber;
                    transaction.BatchNumber = journal.BatchNumber;
                    transaction.JournalNumber = journal.JournalNumber;
                    transaction.TransactionNumber = ++journal.LastTransactionNumber;
                    transaction.AccountCode = giftdetail.AccountCode;
                    transaction.CostCentreCode = giftdetail.CostCentreCode;
                    transaction.Narrative = "GB - Gift Batch " + giftBatch.BatchNumber.ToString();
                    transaction.Reference = "GB" + giftBatch.BatchNumber.ToString();
                    transaction.DebitCreditIndicator = false;
                    transaction.TransactionAmount = 0;
                    transaction.AmountInBaseCurrency = 0;
                    transaction.SystemGenerated = true;
                    transaction.TransactionDate = giftBatch.GlEffectiveDate;

                    GLDataset.ATransaction.Rows.Add(transaction);
                }
                else
                {
                    transaction = (ATransactionRow)GLDataset.ATransaction.DefaultView[0].Row;
                }

                transaction.TransactionAmount += giftdetail.GiftTransactionAmount;
                transaction.AmountInBaseCurrency += giftdetail.GiftAmount;

                // TODO: for other currencies a post to a_ledger.a_forex_gains_losses_account_c ???

                // TODO: do the fee calculation, a_fees_payable, a_fees_receivable
            }

            ATransactionRow transactionForTotals = GLDataset.ATransaction.NewRowTyped();
            transactionForTotals.LedgerNumber = journal.LedgerNumber;
            transactionForTotals.BatchNumber = journal.BatchNumber;
            transactionForTotals.JournalNumber = journal.JournalNumber;
            transactionForTotals.TransactionNumber = ++journal.LastTransactionNumber;
            transactionForTotals.TransactionAmount = 0;
            transactionForTotals.TransactionDate = giftBatch.GlEffectiveDate;
            transactionForTotals.SystemGenerated = true;

            foreach (GiftBatchTDSAGiftDetailRow giftdetail in AGiftDataset.AGiftDetail.Rows)
            {
                transactionForTotals.TransactionAmount += giftdetail.GiftTransactionAmount;
            }

            transactionForTotals.DebitCreditIndicator = true;

            // TODO: support foreign currencies
            transactionForTotals.AmountInBaseCurrency = transactionForTotals.TransactionAmount;

            // TODO: account and costcentre based on linked costcentre, current commitment, and Motivation detail
            // if motivation cost centre is a summary cost centre, make sure the transaction costcentre is reporting to that summary cost centre
            // Careful: modify gift cost centre and account and recipient field only when the amount is positive.
            // adjustments and reversals must remain on the original value
            transactionForTotals.AccountCode = giftBatch.BankAccountCode;
            transactionForTotals.CostCentreCode = TLedgerInfo.GetStandardCostCentre(ALedgerNumber);
            transactionForTotals.Narrative = "Deposit from receipts - Gift Batch " + giftBatch.BatchNumber.ToString();
            transactionForTotals.Reference = "GB" + giftBatch.BatchNumber.ToString();

            GLDataset.ATransaction.Rows.Add(transactionForTotals);

            GLDataset.ATransaction.DefaultView.RowFilter = string.Empty;

            foreach (ATransactionRow transaction in GLDataset.ATransaction.Rows)
            {
                if (transaction.DebitCreditIndicator)
                {
                    journal.JournalDebitTotal += transaction.TransactionAmount;
                    batch.BatchDebitTotal += transaction.TransactionAmount;
                }
                else
                {
                    journal.JournalCreditTotal += transaction.TransactionAmount;
                    batch.BatchCreditTotal += transaction.TransactionAmount;
                }
            }

            batch.LastJournal = 1;

            return GLDataset;
        }

        private static void LoadGiftRelatedData(GiftBatchTDS AGiftDS, bool ARecurring,
            Int32 ALedgerNumber, Int32 ABatchNumber,
            TDBTransaction ATransaction)
        {
            // load all donor shortnames in one go
            string getDonorSQL =
                "SELECT DISTINCT dp.p_partner_key_n, dp.p_partner_short_name_c, dp.p_status_code_c FROM PUB_p_partner dp, PUB_a_gift g " +
                "WHERE g.a_ledger_number_i = ? AND g.a_batch_number_i = ? AND g.p_donor_key_n = dp.p_partner_key_n";

            if (ARecurring)
            {
                getDonorSQL = getDonorSQL.Replace("PUB_a_gift", "PUB_a_recurring_gift");
            }

            List <OdbcParameter>parameters = new List <OdbcParameter>();
            OdbcParameter param = new OdbcParameter("ledger", OdbcType.Int);
            param.Value = ALedgerNumber;
            parameters.Add(param);
            param = new OdbcParameter("batch", OdbcType.Int);
            param.Value = ABatchNumber;
            parameters.Add(param);

            DBAccess.GDBAccessObj.Select(AGiftDS, getDonorSQL, AGiftDS.DonorPartners.TableName,
                ATransaction,
                parameters.ToArray(), 0, 0);

            // load all recipient partners and fields related to this gift batch in one go
            string getRecipientSQL =
                "SELECT DISTINCT rp.* FROM PUB_p_partner rp, PUB_a_gift_detail gd " +
                "WHERE gd.a_ledger_number_i = ? AND gd.a_batch_number_i = ? AND gd.p_recipient_key_n = rp.p_partner_key_n";

            if (ARecurring)
            {
                getRecipientSQL = getRecipientSQL.Replace("PUB_a_gift", "PUB_a_recurring_gift");
            }

            DBAccess.GDBAccessObj.Select(AGiftDS, getRecipientSQL, AGiftDS.RecipientPartners.TableName,
                ATransaction,
                parameters.ToArray(), 0, 0);

            string getRecipientFamilySQL =
                "SELECT DISTINCT pf.* FROM PUB_p_family pf, PUB_a_gift_detail gd " +
                "WHERE gd.a_ledger_number_i = ? AND gd.a_batch_number_i = ? AND gd.p_recipient_key_n = pf.p_partner_key_n";

            if (ARecurring)
            {
                getRecipientFamilySQL = getRecipientFamilySQL.Replace("PUB_a_gift", "PUB_a_recurring_gift");
            }

            DBAccess.GDBAccessObj.Select(AGiftDS, getRecipientFamilySQL, AGiftDS.RecipientFamily.TableName,
                ATransaction,
                parameters.ToArray(), 0, 0);

            string getRecipientPersonSQL =
                "SELECT DISTINCT pf.* FROM PUB_p_Person pf, PUB_a_gift_detail gd " +
                "WHERE gd.a_ledger_number_i = ? AND gd.a_batch_number_i = ? AND gd.p_recipient_key_n = pf.p_partner_key_n";

            if (ARecurring)
            {
                getRecipientPersonSQL = getRecipientPersonSQL.Replace("PUB_a_gift", "PUB_a_recurring_gift");
            }

            DBAccess.GDBAccessObj.Select(AGiftDS, getRecipientPersonSQL, AGiftDS.RecipientPerson.TableName,
                ATransaction,
                parameters.ToArray(), 0, 0);

            string getRecipientUnitSQL =
                "SELECT DISTINCT pf.* FROM PUB_p_Unit pf, PUB_a_gift_detail gd " +
                "WHERE gd.a_ledger_number_i = ? AND gd.a_batch_number_i = ? AND gd.p_recipient_key_n = pf.p_partner_key_n";

            if (ARecurring)
            {
                getRecipientUnitSQL = getRecipientUnitSQL.Replace("PUB_a_gift", "PUB_a_recurring_gift");
            }

            DBAccess.GDBAccessObj.Select(AGiftDS, getRecipientUnitSQL, AGiftDS.RecipientUnit.TableName,
                ATransaction,
                parameters.ToArray(), 0, 0);

            UmUnitStructureAccess.LoadAll(AGiftDS, ATransaction);
            AGiftDS.UmUnitStructure.DefaultView.Sort = UmUnitStructureTable.GetChildUnitKeyDBName();
        }

        /// <summary>
        /// Check if an entry exists in ValidLedgerNumber for the specified ledger number and partner key
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="ACostCentreCode"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool CheckCostCentreLinkForRecipient(Int32 ALedgerNumber, Int64 APartnerKey, out string ACostCentreCode)
        {
            bool CostCentreExists = false;

            ACostCentreCode = string.Empty;

            bool NewTransaction;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            string ValidLedgerNumberTable = "ValidLedgerNumber";

            string GetPartnerValidLedgerNumberSQL = String.Format("SELECT DISTINCT vln.a_cost_centre_code_c FROM PUB_a_valid_ledger_number vln " +
                "WHERE vln.a_ledger_number_i = {0} AND vln.p_partner_key_n = {1}",
                ALedgerNumber,
                APartnerKey);

            DataSet tempDataSet = new DataSet();

            try
            {
                DBAccess.GDBAccessObj.Select(tempDataSet, GetPartnerValidLedgerNumberSQL, ValidLedgerNumberTable,
                    Transaction,
                    0, 0);

                if (tempDataSet.Tables[ValidLedgerNumberTable] != null)
                {
                    if (tempDataSet.Tables[ValidLedgerNumberTable].Rows.Count > 0)
                    {
                        DataRow row = tempDataSet.Tables[ValidLedgerNumberTable].Rows[0];
                        ACostCentreCode = row[0].ToString();
                        CostCentreExists = true;
                    }

                    tempDataSet.Clear();
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return CostCentreExists;
        }

        /// <summary>
        /// create GiftBatchTDS with the gift batch to post, and all gift transactions and details, and motivation details
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadGiftBatchData(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GiftBatchTDS MainDS = new GiftBatchTDS();

            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

            AGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            AGiftAccess.LoadViaAGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            AGiftDetailAccess.LoadViaAGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            LoadGiftRelatedData(MainDS, false, ALedgerNumber, ABatchNumber, Transaction);

            DataView giftView = new DataView(MainDS.AGift);
            giftView.Sort = AGiftTable.GetGiftTransactionNumberDBName();

            // fill the columns in the modified GiftDetail Table to show donorkey, dateentered etc in the grid
            foreach (GiftBatchTDSAGiftDetailRow giftDetail in MainDS.AGiftDetail.Rows)
            {
                // get the gift
                AGiftRow giftRow = (AGiftRow)giftView.FindRows(giftDetail.GiftTransactionNumber)[0].Row;

                PPartnerRow DonorRow = (PPartnerRow)MainDS.DonorPartners.Rows.Find(giftRow.DonorKey);

                giftDetail.DonorKey = giftRow.DonorKey;
                giftDetail.DonorName = DonorRow.PartnerShortName;
                giftDetail.DonorClass = DonorRow.PartnerClass;
                giftDetail.MethodOfGivingCode = giftRow.MethodOfGivingCode;
                giftDetail.MethodOfPaymentCode = giftRow.MethodOfPaymentCode;
                giftDetail.ReceiptNumber = giftRow.ReceiptNumber;
                giftDetail.ReceiptPrinted = giftRow.ReceiptPrinted;

                //do the same for the Recipient
                if (giftDetail.RecipientKey > 0)
                {
                    giftDetail.RecipientField = GetRecipientFundNumber(MainDS, giftDetail.RecipientKey);

                    PPartnerRow RecipientRow = (PPartnerRow)MainDS.RecipientPartners.Rows.Find(giftDetail.RecipientKey);
                    giftDetail.RecipientDescription = RecipientRow.PartnerShortName;

                    PUnitRow RecipientUnitRow = (PUnitRow)MainDS.RecipientUnit.Rows.Find(giftDetail.RecipientKey);

                    if ((RecipientUnitRow != null) && (RecipientUnitRow.UnitTypeCode == MFinanceConstants.UNIT_TYPE_CODE_KEY_MIN))
                    {
                        giftDetail.RecipientKeyMinistry = RecipientUnitRow.UnitName;
                    }
                    else
                    {
                        giftDetail.SetRecipientKeyMinistryNull();
                    }
                }
                else
                {
                    giftDetail.SetRecipientFieldNull();
                    giftDetail.RecipientDescription = "INVALID";
                    giftDetail.SetRecipientKeyMinistryNull();
                }

                //And account code
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)MainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, giftDetail.MotivationGroupCode, giftDetail.MotivationDetailCode });

                if (motivationDetail != null)
                {
                    giftDetail.AccountCode = motivationDetail.AccountCode;
                }
                else
                {
                    giftDetail.SetAccountCodeNull();
                }

                giftDetail.DateEntered = giftRow.DateEntered;
                giftDetail.Reference = giftRow.Reference;
            }

            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            MainDS.LedgerPartnerTypes.Merge(PPartnerTypeAccess.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null));

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// create GiftBatchTDS with the recurring gift batch, and all gift transactions and details, and motivation details
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadRecurringGiftBatchData(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GiftBatchTDS MainDS = new GiftBatchTDS();

            ARecurringGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            ARecurringGiftAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            ARecurringGiftDetailAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            LoadGiftRelatedData(MainDS, true, ALedgerNumber, ABatchNumber, Transaction);

            DataView giftView = new DataView(MainDS.ARecurringGift);
            giftView.Sort = ARecurringGiftTable.GetGiftTransactionNumberDBName();

            // fill the columns in the modified GiftDetail Table to show donorkey, dateentered etc in the grid
            foreach (GiftBatchTDSARecurringGiftDetailRow giftDetail in MainDS.ARecurringGiftDetail.Rows)
            {
                // get the gift
                ARecurringGiftRow giftRow = (ARecurringGiftRow)giftView.FindRows(giftDetail.GiftTransactionNumber)[0].Row;

                PPartnerRow DonorRow = (PPartnerRow)MainDS.DonorPartners.Rows.Find(giftRow.DonorKey);

                giftDetail.DonorKey = giftRow.DonorKey;
                giftDetail.DonorName = DonorRow.PartnerShortName;
                giftDetail.DonorClass = DonorRow.PartnerClass;
                giftDetail.MethodOfGivingCode = giftRow.MethodOfGivingCode;
                giftDetail.MethodOfPaymentCode = giftRow.MethodOfPaymentCode;
                //giftDetail.ReceiptNumber = giftRow.ReceiptNumber;
                //giftDetail.ReceiptPrinted = giftRow.ReceiptPrinted;

                //do the same for the Recipient
                if (giftDetail.RecipientKey > 0)
                {
                    giftDetail.RecipientField = GetRecipientFundNumber(MainDS, giftDetail.RecipientKey);
                    PPartnerRow RecipientRow = (PPartnerRow)MainDS.RecipientPartners.Rows.Find(giftDetail.RecipientKey);
                    giftDetail.RecipientDescription = RecipientRow.PartnerShortName;
                }
                else
                {
                    giftDetail.SetRecipientFieldNull();
                    giftDetail.RecipientDescription = "INVALID";
                }

                //And account code
                AMotivationDetailRow motivationDetail = (AMotivationDetailRow)MainDS.AMotivationDetail.Rows.Find(
                    new object[] { ALedgerNumber, giftDetail.MotivationGroupCode, giftDetail.MotivationDetailCode });

                if (motivationDetail != null)
                {
                    giftDetail.AccountCode = motivationDetail.AccountCode.ToString();
                }
                else
                {
                    giftDetail.SetAccountCodeNull();
                }
            }

            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

        /// <summary>
        /// calculate the admin fee for a given amount.
        /// public so that it can be tested by NUnit tests.
        /// </summary>
        /// <param name="MainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AFeeCode"></param>
        /// <param name="AGiftAmount"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal CalculateAdminFee(GiftBatchTDS MainDS,
            Int32 ALedgerNumber,
            string AFeeCode,
            decimal AGiftAmount,
            out TVerificationResultCollection AVerificationResult
            )
        {
            //Amount to return
            decimal FeeAmount = 0;

            decimal GiftPercentageAmount;
            decimal ChargeAmount;
            string ChargeOption;

            //Error handling
            string ErrorContext = String.Empty;
            string ErrorMessage = String.Empty;
            //Set default type as non-critical
            TResultSeverity ErrorType = TResultSeverity.Resv_Noncritical;

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                AFeesPayableRow feePayableRow = (AFeesPayableRow)MainDS.AFeesPayable.Rows.Find(new object[] { ALedgerNumber, AFeeCode });

                if (feePayableRow == null)
                {
                    AFeesReceivableRow feeReceivableRow = (AFeesReceivableRow)MainDS.AFeesReceivable.Rows.Find(new object[] { ALedgerNumber, AFeeCode });

                    if (feeReceivableRow == null)
                    {
                        ErrorContext = "Calculate Admin Fee";
                        ErrorMessage = String.Format(Catalog.GetString("The Ledger no.: {0} or Fee Code: {1} does not exist."),
                            ALedgerNumber,
                            AFeeCode
                            );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.ArgumentException(ErrorMessage);
                    }
                    else
                    {
                        GiftPercentageAmount = feeReceivableRow.ChargePercentage * AGiftAmount / 100;
                        ChargeOption = feeReceivableRow.ChargeOption.ToUpper();
                        ChargeAmount = feeReceivableRow.ChargeAmount;
                    }
                }
                else
                {
                    GiftPercentageAmount = feePayableRow.ChargePercentage * AGiftAmount / 100;
                    ChargeOption = feePayableRow.ChargeOption.ToUpper();
                    ChargeAmount = feePayableRow.ChargeAmount;
                }

                switch (ChargeOption)
                {
                    case MFinanceConstants.ADMIN_CHARGE_OPTION_FIXED:

                        if (AGiftAmount >= 0)
                        {
                            FeeAmount = ChargeAmount;
                        }
                        else
                        {
                            FeeAmount = -ChargeAmount;
                        }

                        break;

                    case MFinanceConstants.ADMIN_CHARGE_OPTION_MIN:

                        if (AGiftAmount >= 0)
                        {
                            if (ChargeAmount >= GiftPercentageAmount)
                            {
                                FeeAmount = ChargeAmount;
                            }
                            else
                            {
                                FeeAmount = GiftPercentageAmount;
                            }
                        }
                        else
                        {
                            if (-ChargeAmount <= GiftPercentageAmount)
                            {
                                FeeAmount = -ChargeAmount;
                            }
                            else
                            {
                                FeeAmount = GiftPercentageAmount;
                            }
                        }

                        break;

                    case MFinanceConstants.ADMIN_CHARGE_OPTION_MAX:

                        if (AGiftAmount >= 0)
                        {
                            if (ChargeAmount <= GiftPercentageAmount)
                            {
                                FeeAmount = ChargeAmount;
                            }
                            else
                            {
                                FeeAmount = GiftPercentageAmount;
                            }
                        }
                        else
                        {
                            if (-ChargeAmount >= GiftPercentageAmount)
                            {
                                FeeAmount = -ChargeAmount;
                            }
                            else
                            {
                                FeeAmount = GiftPercentageAmount;
                            }
                        }

                        break;

                    case MFinanceConstants.ADMIN_CHARGE_OPTION_PERCENT:
                        FeeAmount = GiftPercentageAmount;
                        break;

                    default:
                        ErrorContext = "Calculate Admin Fee";
                        ErrorMessage =
                            String.Format(Catalog.GetString("Unexpected Fee Payable/Receivable Charge Option in Ledger: {0} and Fee Code: '{1}'."),
                                ALedgerNumber,
                                AFeeCode
                                );
                        ErrorType = TResultSeverity.Resv_Noncritical;
                        throw new System.InvalidOperationException(ErrorMessage);
                }
            }
            catch (ArgumentException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
            }
            catch (InvalidOperationException ex)
            {
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ex.Message, ErrorType));
            }
            catch (Exception ex)
            {
                ErrorContext = "Calculate Admin Fee";
                ErrorMessage = String.Format(Catalog.GetString("Unknown error while calculating admin fee for Ledger: {0} and Fee Code: {1}" +
                        Environment.NewLine + Environment.NewLine + ex.ToString()),
                    ALedgerNumber,
                    AFeeCode
                    );
                ErrorType = TResultSeverity.Resv_Critical;
                AVerificationResult.Add(new TVerificationResult(ErrorContext, ErrorMessage, ErrorType));
            }

            // calculate the admin fee for the specific amount and admin fee. see gl4391.p

            return FeeAmount;
        }

        private static void AddToFeeTotals(GiftBatchTDS AMainDS,
            AGiftDetailRow AGiftDetailRow,
            string AFeeCode,
            decimal AFeeAmount,
            int APostingPeriod)
        {
            // TODO CT
            // see Add_To_Fee_Totals in gr1210.p

            try
            {
                /* Get the record for the totals of the processed fees. */
                AProcessedFeeTable ProcessedFeeDataTable = AMainDS.AProcessedFee;
                AProcessedFeeRow ProcessedFeeRow =
                    (AProcessedFeeRow)ProcessedFeeDataTable.Rows.Find(new object[] { AGiftDetailRow.LedgerNumber,
                                                                                     AGiftDetailRow.BatchNumber,
                                                                                     AGiftDetailRow.GiftTransactionNumber,
                                                                                     AGiftDetailRow.DetailNumber,
                                                                                     AFeeCode });

                if (ProcessedFeeRow == null)
                {
                    ProcessedFeeRow = (AProcessedFeeRow)ProcessedFeeDataTable.NewRowTyped(false);
                    ProcessedFeeRow.LedgerNumber = AGiftDetailRow.LedgerNumber;
                    ProcessedFeeRow.BatchNumber = AGiftDetailRow.BatchNumber;
                    ProcessedFeeRow.GiftTransactionNumber = AGiftDetailRow.GiftTransactionNumber;
                    ProcessedFeeRow.DetailNumber = AGiftDetailRow.DetailNumber;
                    ProcessedFeeRow.FeeCode = AFeeCode;
                    ProcessedFeeRow.PeriodicAmount = 0;

                    ProcessedFeeDataTable.Rows.Add(ProcessedFeeRow);
                }

                ProcessedFeeRow.CostCentreCode = AGiftDetailRow.CostCentreCode;
                ProcessedFeeRow.PeriodNumber = APostingPeriod;

                /* Add the amount to the existing total. */
                ProcessedFeeRow.PeriodicAmount += AFeeAmount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static GiftBatchTDS PrepareGiftBatchForPosting(Int32 ALedgerNumber,
            Int32 ABatchNumber,
            out TVerificationResultCollection AVerifications)
        {
            bool NewTransaction = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            GiftBatchTDS MainDS = LoadGiftBatchData(ALedgerNumber, ABatchNumber);

            AVerifications = new TVerificationResultCollection();

            if (MainDS.AGiftBatch.Rows.Count < 1)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting Gift Batch",
                        String.Format("Unable to Load GiftBatchData ({0}, {1})",
                            ALedgerNumber,
                            ABatchNumber),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            AGiftBatchRow GiftBatchRow = MainDS.AGiftBatch[0];
            // for calculation of admin fees
            AMotivationDetailFeeAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            AFeesPayableAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            AFeesReceivableAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            AProcessedFeeAccess.LoadViaAGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            // check that the Gift Batch BatchPeriod matches the date effective
            int DateEffectivePeriod, DateEffectiveYear;
            TFinancialYear.IsValidPostingPeriod(GiftBatchRow.LedgerNumber,
                GiftBatchRow.GlEffectiveDate,
                out DateEffectivePeriod,
                out DateEffectiveYear,
                null);

            if (GiftBatchRow.BatchPeriod != DateEffectivePeriod)
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting Gift Batch",
                        String.Format("Invalid gift batch period {0} for date {1:dd-MMM-yyyy}",
                            GiftBatchRow.BatchPeriod,
                            GiftBatchRow.GlEffectiveDate),
                        TResultSeverity.Resv_Critical));
                return null;
            }
            else if ((GiftBatchRow.HashTotal != 0) && (GiftBatchRow.BatchTotal != GiftBatchRow.HashTotal))
            {
                AVerifications.Add(
                    new TVerificationResult(
                        "Posting Gift Batch",
                        String.Format("The gift batch total ({0}) does not equal the hash total ({1}).",
                            StringHelper.FormatUsingCurrencyCode(GiftBatchRow.BatchTotal, GiftBatchRow.CurrencyCode),
                            StringHelper.FormatUsingCurrencyCode(GiftBatchRow.HashTotal, GiftBatchRow.CurrencyCode)),
                        TResultSeverity.Resv_Critical));
                return null;
            }

            foreach (GiftBatchTDSAGiftDetailRow giftDetail in MainDS.AGiftDetail.Rows)
            {
                // find motivation detail
                AMotivationDetailRow motivationRow =
                    (AMotivationDetailRow)MainDS.AMotivationDetail.Rows.Find(new object[] { ALedgerNumber,
                                                                                            giftDetail.MotivationGroupCode,
                                                                                            giftDetail.MotivationDetailCode });

                if (motivationRow == null)
                {
                    AVerifications.Add(
                        new TVerificationResult(
                            "Posting Gift Batch",
                            String.Format("Invalid motivation detail {0}/{1} in gift {2}",
                                giftDetail.MotivationGroupCode,
                                giftDetail.MotivationDetailCode,
                                giftDetail.GiftTransactionNumber),
                            TResultSeverity.Resv_Critical));
                    return null;
                }

                PPartnerRow RecipientPartner = (PPartnerRow)MainDS.RecipientPartners.Rows.Find(giftDetail.RecipientKey);

                giftDetail.RecipientLedgerNumber = 0;

                // make sure the correct costcentres and accounts are used
                if (RecipientPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
                {
                    // get the field that the key ministry belongs to. or it might be a field itself
                    giftDetail.RecipientLedgerNumber = GetRecipientFundNumber(MainDS, giftDetail.RecipientKey);
                }
                else if (RecipientPartner.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                {
                    // TODO make sure the correct costcentres and accounts are used, recipient ledger number
                    giftDetail.RecipientLedgerNumber = GetRecipientFundNumber(MainDS, giftDetail.RecipientKey);
                }

                if (giftDetail.RecipientLedgerNumber != 0)
                {
                    giftDetail.CostCentreCode = IdentifyPartnerCostCentre(giftDetail.LedgerNumber, giftDetail.RecipientLedgerNumber);
                }
                else
                {
                    giftDetail.CostCentreCode = motivationRow.CostCentreCode;
                }

                // set column giftdetail.AccountCode motivation
                giftDetail.AccountCode = motivationRow.AccountCode;

                // TODO deal with different currencies; at the moment assuming base currency
                //giftDetail.GiftAmount = giftDetail.GiftTransactionAmount;
                giftDetail.GiftAmount = giftDetail.GiftTransactionAmount * GiftBatchRow.ExchangeRateToBase;

                // get all motivation detail fees for this gift
                foreach (AMotivationDetailFeeRow motivationFeeRow in MainDS.AMotivationDetailFee.Rows)
                {
                    if ((motivationFeeRow.MotivationDetailCode == motivationRow.MotivationDetailCode)
                        && (motivationFeeRow.MotivationGroupCode == motivationRow.MotivationGroupCode))
                    {
                        TVerificationResultCollection Verifications2;

                        decimal FeeAmount = CalculateAdminFee(MainDS,
                            ALedgerNumber,
                            motivationFeeRow.FeeCode,
                            giftDetail.GiftAmount,
                            out Verifications2);

                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(Verifications2))
                        {
                            AVerifications.AddCollection(Verifications2);

                            return null;
                        }

                        AddToFeeTotals(MainDS, giftDetail, motivationFeeRow.FeeCode, FeeAmount, GiftBatchRow.BatchPeriod);
                    }
                }
            }

            // TODO if already posted, fail
            MainDS.AGiftBatch[0].BatchStatus = MFinanceConstants.BATCH_POSTED;

            return MainDS;
        }

        /// <summary>
        /// post a Gift Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        [RequireModulePermission("FINANCE-2")]
        public static bool PostGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            List <Int32>GiftBatches = new List <int>();
            GiftBatches.Add(ABatchNumber);

            return PostGiftBatches(ALedgerNumber, GiftBatches, out AVerifications);
        }

        /// <summary>
        /// post several gift batches at once
        /// </summary>
        [RequireModulePermission("FINANCE-2")]
        public static bool PostGiftBatches(Int32 ALedgerNumber, List <Int32>ABatchNumbers, out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            TVerificationResultCollection SingleVerificationResultCollection;

            bool NewTransaction;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Posting gift batches"),
                ABatchNumbers.Count * 3 + 1);

            List <Int32>GLBatchNumbers = new List <int>();

            try
            {
                // first prepare all the gift batches, mark them as posted, and create the GL batches
                foreach (Int32 BatchNumber in ABatchNumbers)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Posting gift batches"),
                        ABatchNumbers.IndexOf(BatchNumber) * 3);

                    GiftBatchTDS MainDS = PrepareGiftBatchForPosting(ALedgerNumber, BatchNumber, out SingleVerificationResultCollection);

                    AVerifications.AddCollection(SingleVerificationResultCollection);

                    if (MainDS == null)
                    {
                        return false;
                    }

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Posting gift batches"),
                        ABatchNumbers.IndexOf(BatchNumber) * 3 + 1);

                    // create GL batch
                    GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPostingGifts(ALedgerNumber, ref MainDS);

                    ABatchRow batch = GLDataset.ABatch[0];

                    // save the batch
                    if (TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                            out SingleVerificationResultCollection) == TSubmitChangesResult.scrOK)
                    {
                        AVerifications.AddCollection(SingleVerificationResultCollection);

                        GLBatchNumbers.Add(batch.BatchNumber);

                        //
                        //                     Assign ReceiptNumbers to Gifts
                        //
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
                        Int32 LastReceiptNumber = MainDS.ALedger[0].LastHeaderRNumber;

                        foreach (AGiftRow GiftRow in MainDS.AGift.Rows)
                        {
                            LastReceiptNumber++;
                            GiftRow.ReceiptNumber = LastReceiptNumber;
                        }

                        MainDS.ALedger[0].LastHeaderRNumber = LastReceiptNumber;


                        MainDS.ThrowAwayAfterSubmitChanges = true;

                        GiftBatchTDSAccess.SubmitChanges(MainDS);
                    }
                    else
                    {
                        AVerifications.AddCollection(SingleVerificationResultCollection);
                        return false;
                    }
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Posting gift batches"),
                    ABatchNumbers.Count * 3 - 1);

                // now post the GL batches
                if (!TGLPosting.PostGLBatches(ALedgerNumber, GLBatchNumbers,
                        out SingleVerificationResultCollection))
                {
                    AVerifications.AddCollection(SingleVerificationResultCollection);
                    // Transaction will be rolled back, no open GL batch flying around
                    return false;
                }
                else
                {
                    AVerifications.AddCollection(SingleVerificationResultCollection);

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        NewTransaction = false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log("In posting Gift batches: exception " + e.Message);

                throw;
            }
            finally
            {
                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// export all the Data of the batches matching the parameters to a String
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="exportString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>number of exported batches</returns>
        [RequireModulePermission("FINANCE-1")]
        static public Int32 ExportAllGiftBatchData(
            Hashtable requestParams,
            out String exportString,
            out TVerificationResultCollection AMessages)
        {
            TGiftExporting exporting = new TGiftExporting();

            return exporting.ExportAllGiftBatchData(requestParams, out exportString, out AMessages);
        }

        /// <summary>
        /// Import Gift batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediatelya
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">The import file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportGiftBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            TGiftImporting importing = new TGiftImporting();

            return importing.ImportGiftBatches(requestParams, importString, out AMessages);
        }

        /// <summary>
        /// Load Partner Data
        /// </summary>
        /// <param name="PartnerKey">Partner Key </param>
        /// <returns>Partnertable for the partner Key</returns>
        [RequireModulePermission("FINANCE-1")]
        public static PPartnerTable LoadPartnerData(long PartnerKey)
        {
            return PPartnerAccess.LoadByPrimaryKey(PartnerKey, null);
        }

        /// <summary>
        /// Find the cost centre associated with the partner
        /// </summary>
        /// <returns>Cost Centre code</returns>
        [RequireModulePermission("FINANCE-1")]
        public static string IdentifyPartnerCostCentre(Int32 ALedgerNumber, Int64 AFieldNumber)
        {
            TCacheable CachePopulator = new TCacheable();
            Type typeOfTable;
            AValidLedgerNumberTable ValidLedgerNumbers = (AValidLedgerNumberTable)
                                                         CachePopulator.GetCacheableTable(TCacheableFinanceTablesEnum.ValidLedgerNumberList,
                "",
                false,
                out typeOfTable);

            AValidLedgerNumberRow ValidLedgerNumberRow = (AValidLedgerNumberRow)
                                                         ValidLedgerNumbers.Rows.Find(new object[] { ALedgerNumber, AFieldNumber });

            if (ValidLedgerNumberRow != null)
            {
                return ValidLedgerNumberRow.CostCentreCode;
            }
            else
            {
                return TGLTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
            }
        }

        /// <summary>
        /// get the recipient ledger partner for a unit
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static Int64 GetRecipientFundNumber(Int64 partnerKey)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            MainDS.LedgerPartnerTypes.Merge(PPartnerTypeAccess.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null));
            MainDS.RecipientPartners.Merge(PPartnerAccess.LoadByPrimaryKey(partnerKey, null));
            MainDS.RecipientFamily.Merge(PFamilyAccess.LoadByPrimaryKey(partnerKey, null));
            MainDS.RecipientPerson.Merge(PPersonAccess.LoadByPrimaryKey(partnerKey, null));
            MainDS.RecipientUnit.Merge(PUnitAccess.LoadByPrimaryKey(partnerKey, null));
            //MainDS.LedgerPartnerTypes.Merge(PPartnerTypeAccess.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null));

            UmUnitStructureAccess.LoadAll(MainDS, null);
            MainDS.UmUnitStructure.DefaultView.Sort = UmUnitStructureTable.GetChildUnitKeyDBName();

            return GetRecipientFundNumber(MainDS, partnerKey);
        }

        private static Int64 GetRecipientFundNumber(GiftBatchTDS AMainDS, Int64 APartnerKey)
        {
            if (APartnerKey == 0)
            {
                return 0;
            }

            // TODO check pm_staff_data for commitments

            PFamilyRow familyRow;
            PPersonRow personRow;

            if ((familyRow = (PFamilyRow)AMainDS.RecipientFamily.Rows.Find(APartnerKey)) != null)
            {
                if (familyRow.IsFieldKeyNull())
                {
                    return 0;
                }
                else
                {
                    return familyRow.FieldKey;
                }
            }

            if ((personRow = (PPersonRow)AMainDS.RecipientPerson.Rows.Find(APartnerKey)) != null)
            {
                if (personRow.IsFieldKeyNull())
                {
                    return 0;
                }
                else
                {
                    return personRow.FieldKey;
                }
            }

            //Check that LedgerPartnertypes are already loaded
            if (AMainDS.LedgerPartnerTypes.Count == 0)
            {
                AMainDS.LedgerPartnerTypes.Merge(PPartnerTypeAccess.LoadViaPType(MPartnerConstants.PARTNERTYPE_LEDGER, null));
            }

            if (AMainDS.LedgerPartnerTypes.Rows.Find(new object[] { APartnerKey, MPartnerConstants.PARTNERTYPE_LEDGER }) != null)
            {
                //TODO Warning on inactive Fund
                return APartnerKey;
            }

            //This was taken from old Petra - perhaps we should better search for unit type = F in PUnit

            DataRowView[] rows = AMainDS.UmUnitStructure.DefaultView.FindRows(APartnerKey);

            if (rows.Length > 0)
            {
                UmUnitStructureRow structureRow = (UmUnitStructureRow)rows[0].Row;

                if (structureRow.ParentUnitKey == structureRow.ChildUnitKey)
                {
                    // should not get here
                    return 0;
                }

                // recursive call until we find a partner that has partnertype LEDGER
                return GetRecipientFundNumber(AMainDS, structureRow.ParentUnitKey);
            }
            else
            {
                TLogging.Log("cannot find Recipient LedgerNumber for partner " + APartnerKey.ToString());
                return APartnerKey;
            }
        }

        /// <summary>
        /// Check if Key Ministry exists
        /// </summary>
        /// <param name="APartnerKey">Partner Key </param>
        /// <param name="AIsActive">return true if Key Ministry is active </param>
        /// <returns>return true if APartnerKey identifies a Key Ministry</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean KeyMinistryExists(Int64 APartnerKey, out Boolean AIsActive)
        {
            Boolean KeyMinistryExists = false;
            TDBTransaction Transaction = null;

            AIsActive = false;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                PUnitTable UnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, Transaction);

                if (UnitTable.Rows.Count == 1)
                {
                    // this partner is indeed a unit
                    PUnitRow UnitRow = UnitTable[0];

                    if (UnitRow.UnitTypeCode.Equals(MPartnerConstants.UNIT_TYPE_KEYMIN))
                    {
                        KeyMinistryExists = true;

                        PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, Transaction);
                        PPartnerRow PartnerRow = PartnerTable[0];

                        if (SharedTypes.StdPartnerStatusCodeStringToEnum(PartnerRow.StatusCode) == TStdPartnerStatusCode.spscACTIVE)
                        {
                            AIsActive = true;
                        }
                    }
                }
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return KeyMinistryExists;
        }

        /// <summary>
        /// Load key Ministry
        /// </summary>
        /// <param name="partnerKey">Partner Key </param>
        /// <param name="fieldNumber">Field Number </param>
        /// <returns>ArrayList for loading the key ministry combobox</returns>
        [RequireModulePermission("FINANCE-1")]
        public static PUnitTable LoadKeyMinistry(Int64 partnerKey, out Int64 fieldNumber)
        {
            TDBTransaction Transaction = null;

            PUnitTable unitTable = null;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                unitTable = LoadKeyMinistries(partnerKey, Transaction);
                fieldNumber = GetRecipientFundNumber(partnerKey);
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return unitTable;
        }

        /// <summary>
        /// get the key ministries. If Recipient is a field, get the key ministries of that field.
        /// If Recipient is a key ministry itself, get all key ministries of the same field
        /// </summary>
        private static PUnitTable LoadKeyMinistries(Int64 ARecipientPartnerKey, TDBTransaction ATransaction)
        {
            PUnitTable UnitTable = PUnitAccess.LoadByPrimaryKey(ARecipientPartnerKey, ATransaction);

            if (UnitTable.Rows.Count == 1)
            {
                // this partner is indeed a unit
                PUnitRow unitRow = UnitTable[0];

                switch (unitRow.UnitTypeCode)
                {
                    case MPartnerConstants.UNIT_TYPE_KEYMIN:
                        Int64 fieldNumber = GetRecipientFundNumber(ARecipientPartnerKey);
                        UnitTable = LoadKeyMinistriesOfField(fieldNumber, ATransaction);
                        break;

                    case MPartnerConstants.UNIT_TYPE_FIELD:
                        UnitTable = LoadKeyMinistriesOfField(ARecipientPartnerKey, ATransaction);
                        break;
                }
            }

            return UnitTable;
        }

        private static PUnitTable LoadKeyMinistriesOfField(Int64 partnerKey, TDBTransaction ATransaction)
        {
            string sqlLoadKeyMinistriesOfField =
                "SELECT unit.* FROM PUB_um_unit_structure us, PUB_p_unit unit, PUB_p_partner partner " +
                "WHERE us.um_parent_unit_key_n = " + partnerKey.ToString() + " " +
                "AND unit.p_partner_key_n = us.um_child_unit_key_n " +
                "AND unit.u_unit_type_code_c = '" + MPartnerConstants.UNIT_TYPE_KEYMIN + "' " +
                "AND partner.p_partner_key_n = unit.p_partner_key_n " +
                "AND partner.p_status_code_c = '" + MPartnerConstants.PARTNERSTATUS_ACTIVE + "'";

            PUnitTable UnitTable = new PUnitTable();

            DBAccess.GDBAccessObj.SelectDT(UnitTable, sqlLoadKeyMinistriesOfField, ATransaction, new OdbcParameter[0], 0, 0);

            return UnitTable;
        }

        #region Data Validation

        static partial void ValidateGiftBatch(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateGiftBatchManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateGiftDetail(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateGiftDetailManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        static partial void ValidateRecurringGiftBatch(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateRecurringGiftBatchManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateRecurringGiftDetail(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateRecurringGiftDetailManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}