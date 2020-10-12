//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, dougm
//
// Copyright 2004-2020 by OM International
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
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.Gift
{
    /// <summary>
    /// provides methods for exporting a batch
    /// </summary>
    public class TGiftExporting
    {
        private const String quote = "\"";
        private const String summarizedData = "Summarised Gift Data";
        private const String sGift = "Gift";
        private const String sConfidential = "Confidential";
        TDBTransaction FTransaction = new TDBTransaction();
        StringWriter FStringWriter;
        String FDelimiter;
        String FDateFormatString;
        CultureInfo FCultureInfo;
        Boolean FTransactionsOnly;
        Boolean FExtraColumns;
        DateTime? FDateForSummary;
        Boolean FUseBaseCurrency;
        Int32 FLedgerNumber;
        String FCurrencyCode = "";
        GiftBatchTDS FMainDS;
        StringHelper myStringHelper = new StringHelper();

        TVerificationResultCollection FMessages = new TVerificationResultCollection();

        /// <summary>
        /// export all the Data of the batches matching the parameters to an Excel file
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumberStart"></param>
        /// <param name="ABatchNumberEnd"></param>
        /// <param name="ABatchDateFrom"></param>
        /// <param name="ABatchDateTo"></param>
        /// <param name="ADateFormatString"></param>
        /// <param name="ASummary"></param>
        /// <param name="AUseBaseCurrency"></param>
        /// <param name="ADateForSummary"></param>
        /// <param name="ANumberFormat">American or European</param>
        /// <param name="ATransactionsOnly"></param>
        /// <param name="AExtraColumns"></param>
        /// <param name="ARecipientNumber"></param>
        /// <param name="AFieldNumber"></param>
        /// <param name="AIncludeUnposted"></param>
        /// <param name="AExportExcel">the export file as Excel file</param>
        /// <param name="AVerificationMessages">Additional messages to display in a messagebox</param>
        /// <returns>number of exported batches, -1 if cancelled, -2 if error</returns>
        public Int32 ExportAllGiftBatchData(
            Int32 ALedgerNumber,
            Int32 ABatchNumberStart,
            Int32 ABatchNumberEnd,
            DateTime? ABatchDateFrom,
            DateTime? ABatchDateTo,
            string ADateFormatString,
            bool ASummary,
            bool AUseBaseCurrency,
            DateTime? ADateForSummary,
            string ANumberFormat,
            bool ATransactionsOnly,
            bool AExtraColumns,
            Int64 ARecipientNumber,
            Int64 AFieldNumber,
            bool AIncludeUnposted,
            out String AExportExcel,
            out TVerificationResultCollection AVerificationMessages)
        {
            int ReturnGiftBatchCount = 0;
            AExportExcel = string.Empty;

            FStringWriter = new StringWriter();
            FMainDS = new GiftBatchTDS();
            FDelimiter = ",";
            FLedgerNumber = ALedgerNumber;
            FDateFormatString = ADateFormatString;
            Boolean Summary = ASummary;
            FUseBaseCurrency = AUseBaseCurrency;
            FDateForSummary = ADateForSummary;
            FCultureInfo = new CultureInfo(ANumberFormat.Equals("American") ? "en-US" : "de-DE");
            FTransactionsOnly = ATransactionsOnly;
            FExtraColumns = AExtraColumns;
            String RecipientFilter = (ARecipientNumber != 0) ? " AND PUB_a_gift_detail.p_recipient_key_n = " + ARecipientNumber.ToString() : "";
            String FieldFilter = (AFieldNumber != 0) ? " AND PUB_a_gift_detail.a_recipient_ledger_number_n = " + AFieldNumber.ToString() : "";

            String StatusFilter =
                (AIncludeUnposted) ? " AND (PUB_a_gift_batch.a_batch_status_c = 'Posted' OR PUB_a_gift_batch.a_batch_status_c = 'Unposted')"
                : " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'";

            if (ABatchNumberStart == ABatchNumberEnd)
            {
                StatusFilter = "";
            }

            TDataBase db = DBAccess.Connect("ExportAllGiftBatchData");

            try
            {
                db.ReadTransaction(
                    ref FTransaction,
                    delegate
                    {
                        try
                        {
                            myStringHelper.CurrencyFormatTable = db.SelectDT("SELECT * FROM PUB_a_currency", "a_currency", FTransaction);

                            ALedgerAccess.LoadByPrimaryKey(FMainDS, FLedgerNumber, FTransaction);
                            String BatchRangeFilter = (ABatchNumberStart > -1) ?
                                                      " AND (PUB_a_gift_batch.a_batch_number_i >= " + ABatchNumberStart.ToString() +
                                                      " AND PUB_a_gift_batch.a_batch_number_i <= " + ABatchNumberEnd.ToString() +
                                                      ")" : "";

                            // If I've specified a BatchRange, I can't also have a DateRange:
                            String DateRangeFilter = (BatchRangeFilter == "") ?
                                                     " AND (PUB_a_gift_batch.a_gl_effective_date_d >= '" +
                                                     ABatchDateFrom.Value.ToString("yyyy-MM-dd") +
                                                     "' AND PUB_a_gift_batch.a_gl_effective_date_d <= '" +
                                                     ABatchDateTo.Value.ToString("yyyy-MM-dd") +
                                                     "')" : "";

                            string StatementCore =
                                " FROM PUB_a_gift_batch, PUB_a_gift, PUB_a_gift_detail" +
                                " WHERE PUB_a_gift_batch.a_ledger_number_i = " + FLedgerNumber +
                                RecipientFilter +
                                FieldFilter +
                                DateRangeFilter +
                                BatchRangeFilter +
                                StatusFilter +
                                " AND PUB_a_gift.a_ledger_number_i =  PUB_a_gift_batch.a_ledger_number_i" +
                                " AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i" +
                                " AND PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i" +
                                " AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i" +
                                " AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i";

                            string sqlStatement = "SELECT DISTINCT PUB_a_gift_batch.* " +
                                                  StatementCore +
                                                  " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName();

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Retrieving gift batch records"),
                                5);

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                                throw new ApplicationException(Catalog.GetString("Export of Batches was cancelled by user"));
                            }

                            db.Select(FMainDS,
                                sqlStatement,
                                FMainDS.AGiftBatch.TableName,
                                FTransaction
                                );


                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Retrieving gift records"),
                                10);

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                                throw new ApplicationException(Catalog.GetString("Export of Batches was cancelled by user"));
                            }

                            sqlStatement = "SELECT DISTINCT PUB_a_gift.* " +
                                           StatementCore +
                                           " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName() +
                                           ", " +
                                           AGiftTable.GetGiftTransactionNumberDBName();

                            db.Select(FMainDS,
                                sqlStatement,
                                FMainDS.AGift.TableName,
                                FTransaction);


                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Retrieving gift detail records"),
                                15);

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                                throw new ApplicationException(Catalog.GetString("Export of Batches was cancelled by user"));
                            }

                            sqlStatement = "SELECT DISTINCT PUB_a_gift_detail.* " + StatementCore;

                            db.Select(FMainDS,
                                sqlStatement,
                                FMainDS.AGiftDetail.TableName,
                                FTransaction);
                        }
                        catch (Exception ex)
                        {
                            TLogging.LogException(ex, Utilities.GetMethodSignature());
                            throw;
                        }
                    });

                TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Exporting Gift Batches"), 100);

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Retrieving records"),
                    5);

                string BaseCurrency = FMainDS.ALedger[0].BaseCurrency;
                FCurrencyCode = BaseCurrency; // Depending on FUseBaseCurrency, this will be overwritten for each gift.

                SortedDictionary <String, AGiftSummaryRow>sdSummary = new SortedDictionary <String, AGiftSummaryRow>();

                UInt32 counter = 0;

                // TProgressTracker Variables
                UInt32 GiftCounter = 0;

                AGiftSummaryRow giftSummary = null;

                FMainDS.AGiftDetail.DefaultView.Sort =
                    AGiftDetailTable.GetLedgerNumberDBName() + "," +
                    AGiftDetailTable.GetBatchNumberDBName() + "," +
                    AGiftDetailTable.GetGiftTransactionNumberDBName();

                foreach (AGiftBatchRow giftBatch in FMainDS.AGiftBatch.Rows)
                {
                    if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                    {
                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                        throw new ApplicationException(Catalog.GetString("Export of Batches was cancelled by user"));
                    }

                    ReturnGiftBatchCount++;

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        string.Format(Catalog.GetString("Batch {0}"), giftBatch.BatchNumber),
                        20);
                    GiftCounter = 0;

                    if (!FTransactionsOnly & !Summary)
                    {
                        WriteGiftBatchLine(giftBatch);
                    }

                    foreach (AGiftRow gift in FMainDS.AGift.Rows)
                    {
                        if (gift.BatchNumber.Equals(giftBatch.BatchNumber) && gift.LedgerNumber.Equals(giftBatch.LedgerNumber))
                        {
                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                                throw new ApplicationException(Catalog.GetString("Export of Batches was cancelled by user"));
                            }

                            // Update progress tracker every 25 records
                            if (++GiftCounter % 25 == 0)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    string.Format(Catalog.GetString("Batch {0} - Exporting gifts"), giftBatch.BatchNumber),
                                    (GiftCounter / 25 + 4) * 5 > 90 ? 90 : (GiftCounter / 25 + 4) * 5);
                            }

                            DataRowView[] selectedRowViews = FMainDS.AGiftDetail.DefaultView.FindRows(
                                new object[] { gift.LedgerNumber, gift.BatchNumber, gift.GiftTransactionNumber });

                            foreach (DataRowView rv in selectedRowViews)
                            {
                                AGiftDetailRow giftDetail = (AGiftDetailRow)rv.Row;

                                if (Summary)
                                {
                                    FCurrencyCode = FUseBaseCurrency ? BaseCurrency : giftBatch.CurrencyCode;
                                    decimal mapExchangeRateToBase = FUseBaseCurrency ? 1 : giftBatch.ExchangeRateToBase;


                                    counter++;
                                    String DictionaryKey = FCurrencyCode + ";" + giftBatch.BankCostCentre + ";" + giftBatch.BankAccountCode + ";" +
                                                           giftDetail.RecipientKey + ";" + giftDetail.MotivationGroupCode + ";" +
                                                           giftDetail.MotivationDetailCode;

                                    if (sdSummary.TryGetValue(DictionaryKey, out giftSummary))
                                    {
                                        giftSummary.GiftTransactionAmount += giftDetail.GiftTransactionAmount;
                                        giftSummary.GiftAmount += giftDetail.GiftAmount;
                                    }
                                    else
                                    {
                                        giftSummary = new AGiftSummaryRow();

                                        /*
                                         * summary_data.a_transaction_currency_c = lv_stored_currency_c
                                         * summary_data.a_bank_cost_centre_c = a_gift_batch.a_bank_cost_centre_c
                                         * summary_data.a_bank_account_code_c = a_gift_batch.a_bank_account_code_c
                                         * summary_data.a_recipient_key_n = a_gift_detail.p_recipient_key_n
                                         * summary_data.a_motivation_group_code_c = a_gift_detail.a_motivation_group_code_c
                                         * summary_data.a_motivation_detail_code_c = a_gift_detail.a_motivation_detail_code_c
                                         * summary_data.a_exchange_rate_to_base_n = lv_exchange_rate_n
                                         * summary_data.a_gift_type_c = a_gift_batch.a_gift_type_c */
                                        giftSummary.CurrencyCode = FCurrencyCode;
                                        giftSummary.BankCostCentre = giftBatch.BankCostCentre;
                                        giftSummary.BankAccountCode = giftBatch.BankAccountCode;
                                        giftSummary.RecipientKey = giftDetail.RecipientKey;
                                        giftSummary.MotivationGroupCode = giftDetail.MotivationGroupCode;
                                        giftSummary.MotivationDetailCode = giftDetail.MotivationDetailCode;
                                        giftSummary.GiftTransactionAmount = giftDetail.GiftTransactionAmount;
                                        giftSummary.GiftAmount = giftDetail.GiftAmount;

                                        sdSummary.Add(DictionaryKey, giftSummary);
                                    }

                                    //overwrite always because we want to have the last
                                    giftSummary.ExchangeRateToBase = mapExchangeRateToBase;
                                }
                                else  // not summary
                                {
                                    WriteGiftLine(gift, giftDetail);
                                }
                            }
                        }
                    }
                }

                if (Summary)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Export Summary"),
                        95);

                    bool first = true;

                    foreach (KeyValuePair <string, AGiftSummaryRow>kvp in sdSummary)
                    {
                        if (!FTransactionsOnly && first)
                        {
                            WriteGiftBatchSummaryLine(kvp.Value);
                            first = false;
                        }

                        WriteGiftSummaryLine(kvp.Value);
                    }
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Gift batch export successful"),
                    100);

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }
            catch (ApplicationException)
            {
                //Show cancel condition
                ReturnGiftBatchCount = -1;
                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());

                //Show error condition
                ReturnGiftBatchCount = -2;

                FMessages.Add(new TVerificationResult(
                        "Exporting Gift Batches Terminated Unexpectedly",
                        ex.Message,
                        "An unexpected error occurred during the export of gift batches",
                        string.Empty,
                        TResultSeverity.Resv_Critical,
                        Guid.Empty));

                TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
            }

            if (ReturnGiftBatchCount > 0)
            {
                MemoryStream mstream = new MemoryStream();
                if (TCsv2Xml.CSV2ExcelStream(FStringWriter.ToString(), mstream, FDelimiter, "giftbatch"))
                {
                    AExportExcel = Convert.ToBase64String(mstream.ToArray());
                }
            }

            AVerificationMessages = FMessages;

            return ReturnGiftBatchCount;
        } // ExportAllGiftBatchData

        private String PartnerShortName(Int64 partnerKey)
        {
            if (partnerKey > 0)
            {
                // Get Partner ShortName
                PPartnerTable pt = null;

                TDBTransaction Transaction = new TDBTransaction();
                TDataBase db = DBAccess.Connect("PartnerShortName");
                db.ReadTransaction(
                    ref Transaction,
                    delegate
                    {
                        pt =
                            PPartnerAccess.LoadByPrimaryKey(partnerKey,
                                StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }),
                                Transaction, null, 0, 0);
                    });

                if (pt.Rows.Count == 1)
                {
                    return pt[0].PartnerShortName;
                }
            }

            return "";
        }

        void WriteGiftBatchSummaryLine(AGiftSummaryRow giftSummary)
        {
            WriteStringQuoted("B");
            WriteStringQuoted(summarizedData);
            WriteStringQuoted(giftSummary.BankAccountCode);
            WriteCurrency(0);
            WriteDate(FDateForSummary.Value);

            if (FUseBaseCurrency)
            {
                WriteStringQuoted(FMainDS.ALedger[0].BaseCurrency);
            }
            else
            {
                WriteStringQuoted(giftSummary.CurrencyCode);
            }

            WriteGeneralNumber(giftSummary.ExchangeRateToBase);
            WriteStringQuoted(giftSummary.BankCostCentre);
            WriteStringQuoted(giftSummary.GiftType, true);
        }

        void WriteGiftBatchLine(AGiftBatchRow giftBatch)
        {
            WriteStringQuoted("B");
            WriteStringQuoted(giftBatch.BatchDescription);
            WriteStringQuoted(giftBatch.BankAccountCode);

            if (FUseBaseCurrency)
            {
                WriteCurrency(giftBatch.HashTotal * giftBatch.ExchangeRateToBase);
            }
            else
            {
                WriteCurrency(giftBatch.HashTotal);
            }

            WriteDate(giftBatch.GlEffectiveDate);

            if (FUseBaseCurrency)
            {
                WriteStringQuoted(FMainDS.ALedger[0].BaseCurrency);
            }
            else
            {
                WriteStringQuoted(giftBatch.CurrencyCode);
            }

            WriteGeneralNumber(giftBatch.ExchangeRateToBase);
            WriteStringQuoted(giftBatch.BankCostCentre);
            WriteStringQuoted(giftBatch.GiftType, true);
        }

        void WriteGiftLine(AGiftRow gift, AGiftDetailRow giftDetails)
        {
            if (!FTransactionsOnly)
            {
                WriteStringQuoted("T");
            }

            if (TGift.GiftRestricted(gift, FTransaction))
            {
                WriteGeneralNumber(0);
                WriteStringQuoted("Confidential");
                ProcessConfidentialMessage();
            }
            else
            {
                WriteGeneralNumber(gift.DonorKey);
                WriteStringQuoted(PartnerShortName(gift.DonorKey));
            }

            WriteStringQuoted(gift.MethodOfGivingCode);
            WriteStringQuoted(gift.MethodOfPaymentCode);
            WriteStringQuoted(gift.Reference);
            WriteStringQuoted(gift.ReceiptLetterCode);

            if (FExtraColumns)
            {
                WriteGeneralNumber(gift.ReceiptNumber);
                WriteBoolean(gift.FirstTimeGift);
                WriteBoolean(gift.ReceiptPrinted);
            }

            WriteGeneralNumber(giftDetails.RecipientKey);
            WriteStringQuoted(PartnerShortName(giftDetails.RecipientKey));

            if (FExtraColumns)
            {
                WriteGeneralNumber(giftDetails.RecipientLedgerNumber);
            }

            if (FUseBaseCurrency)
            {
                WriteCurrency(giftDetails.GiftAmount);
            }
            else
            {
                WriteCurrency(giftDetails.GiftTransactionAmount);
            }

            if (FExtraColumns)
            {
                WriteCurrency(giftDetails.GiftAmountIntl);
            }

            WriteBoolean(giftDetails.ConfidentialGiftFlag);
            WriteStringQuoted(giftDetails.MotivationGroupCode);
            WriteStringQuoted(giftDetails.MotivationDetailCode);

            //
            // "In Petra Cost Centre is always inferred from recipient field and motivation detail so is not needed in the import."
            if (FExtraColumns)
            {
                WriteStringQuoted(giftDetails.CostCentreCode);
            }

            WriteStringQuoted(giftDetails.GiftCommentOne);
            WriteStringQuoted(giftDetails.CommentOneType);

            if (giftDetails.MailingCode.Equals("?"))
            {
                WriteStringQuoted("");
            }
            else
            {
                WriteStringQuoted(giftDetails.MailingCode);
            }

            WriteStringQuoted(giftDetails.GiftCommentTwo);
            WriteStringQuoted(giftDetails.CommentTwoType);
            WriteStringQuoted(giftDetails.GiftCommentThree);
            WriteStringQuoted(giftDetails.CommentThreeType);

            WriteBoolean(giftDetails.IsTaxDeductibleNull() ? false : giftDetails.TaxDeductible, true);

//          WriteLineDate(gift.DateEntered);  //Don't write this - it can't be used by import.
        }

        private Boolean confidentialMessageGiven = false;
        void ProcessConfidentialMessage()
        {
            if (!confidentialMessageGiven)
            {
                FMessages.Add(new TVerificationResult(
                        Catalog.GetString("Gift Details"), Catalog.GetString("Please note that some gifts in this report are restricted.\n" +
                            "To include full details of these gifts, the export\n" +
                            "must be run by someone with a higher level of access."),
                        TResultSeverity.Resv_Noncritical));
                confidentialMessageGiven = true;
            }
        }

        void WriteGiftSummaryLine(AGiftSummaryRow giftSummary)
        {
            if (!FTransactionsOnly)
            {
                WriteStringQuoted("T");
            }

            Int64 tempKey = FLedgerNumber * 1000000;
            WriteGeneralNumber(tempKey);               //is this the right Ledger Number?
            WriteStringQuoted(PartnerShortName(tempKey));
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteGeneralNumber(giftSummary.RecipientKey);
            WriteStringQuoted(PartnerShortName(giftSummary.RecipientKey));

            if (FUseBaseCurrency)
            {
                WriteCurrency(giftSummary.GiftAmount, true);
            }
            else
            {
                WriteCurrency(giftSummary.GiftTransactionAmount, true);
            }
        }

        void WriteDelimiter(bool bLineEnd = false)
        {
            if (bLineEnd)
            {
                FStringWriter.WriteLine();
            }
            else
            {
                FStringWriter.Write(FDelimiter);
            }
        }

        void WriteStringQuoted(String theString, bool bLineEnd = false)
        {
            if (theString != null)
            {
                theString = theString.Replace(quote, quote + quote);
            }

            FStringWriter.Write(quote);
            FStringWriter.Write(theString);
            FStringWriter.Write(quote);
            WriteDelimiter(bLineEnd);
        }

        void WriteCurrency(decimal currencyField, bool bLineEnd = false)
        {
            Int64 integerNumber = Convert.ToInt64(currencyField);

            if (Convert.ToDecimal(integerNumber) == currencyField)
            {
                FStringWriter.Write(String.Format("{0:d}", integerNumber));
            }
            else
            {
                FStringWriter.Write(quote);
                FStringWriter.Write(myStringHelper.FormatUsingCurrencyCode(currencyField, FCurrencyCode));
                FStringWriter.Write(quote);
            }

            WriteDelimiter(bLineEnd);
        }

        void WriteGeneralNumber(decimal generalNumberField, bool bLineEnd = false)
        {
            FStringWriter.Write(String.Format(FCultureInfo, "{0:g}", generalNumberField));
            WriteDelimiter(bLineEnd);
        }

        void WriteBoolean(bool aBool, bool bLineEnd = false)
        {
            FStringWriter.Write(aBool ? "yes" : "no");
            WriteDelimiter(bLineEnd);
        }

        void WriteDate(DateTime dateField, bool bLineEnd = false)
        {
            FStringWriter.Write(dateField.ToString(FDateFormatString));
            WriteDelimiter(bLineEnd);
        }
    }

    /// <summary>
    /// provides the (outer and inner) structure for summarizing gifts
    /// </summary>
    public class AGiftSummaryRow
    {
        private String currencyCode;

        private Int64 recipientKey;

        /// <summary>
        /// Recipient Key
        /// </summary>
        public long RecipientKey {
            get
            {
                return recipientKey;
            }
            set
            {
                recipientKey = value;
            }
        }
        private String motivationGroupCode;

        /// <summary>
        /// Motivation Group Code
        /// </summary>
        public string MotivationGroupCode {
            get
            {
                return motivationGroupCode;
            }
            set
            {
                motivationGroupCode = value;
            }
        }
        private String motivationDetailCode;

        /// <summary>
        /// Motivation Detail Code
        /// </summary>
        public string MotivationDetailCode {
            get
            {
                return motivationDetailCode;
            }
            set
            {
                motivationDetailCode = value;
            }
        }
        private decimal exchangeRateToBase;


        /// <summary>
        /// Exchange Rate
        /// </summary>
        public decimal ExchangeRateToBase {
            get
            {
                return exchangeRateToBase;
            }
            set
            {
                exchangeRateToBase = value;
            }
        }

        /// <summary>
        /// Transaction Currency
        /// </summary>
        public string CurrencyCode {
            get
            {
                return currencyCode;
            }
            set
            {
                currencyCode = value;
            }
        }

        private String bankCostCentre;

        /// <summary>
        /// Cost Centre
        /// </summary>
        public string BankCostCentre {
            get
            {
                return bankCostCentre;
            }
            set
            {
                bankCostCentre = value;
            }
        }
        private String bankAccountCode;

        /// <summary>
        /// Account Code
        /// </summary>
        public string BankAccountCode {
            get
            {
                return bankAccountCode;
            }
            set
            {
                bankAccountCode = value;
            }
        }


        private decimal giftTransactionAmount;

        /// <summary>
        /// >Gift Transaction Amount
        /// </summary>
        public decimal GiftTransactionAmount {
            get
            {
                return giftTransactionAmount;
            }
            set
            {
                giftTransactionAmount = value;
            }
        }
        private decimal giftAmount;

        /// <summary>
        /// Amount in Base currency
        /// </summary>
        public decimal GiftAmount {
            get
            {
                return giftAmount;
            }
            set
            {
                giftAmount = value;
            }
        }

        private String giftType;

        /// <summary>
        /// Amount in Base currency (may be negative!!!)
        /// </summary>
        public String GiftType {
            get
            {
                return giftType;
            }
            set
            {
                giftType = value;
            }
        }
    }
}
