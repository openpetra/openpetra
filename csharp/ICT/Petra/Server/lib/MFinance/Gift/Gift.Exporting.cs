//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, dougm
//
// Copyright 2004-2012 by OM International
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
using Ict.Common.Verification;
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
        TDBTransaction FTransaction;
        StringWriter FStringWriter;
        String FDelimiter;
        String FDateFormatString;
        CultureInfo FCultureInfo;
        bool FTransactionsOnly;
        bool FExtraColumns;
        DateTime FDateForSummary;
        bool FUseBaseCurrency;
        Int32 FLedgerNumber;
        String FCurrencyCode = "";

        TVerificationResultCollection FMessages = new TVerificationResultCollection();

        /// <summary>
        /// export all the Data of the batches matching the parameters to a String
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="exportString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>number of exported batches</returns>
        public Int32 ExportAllGiftBatchData(
            Hashtable requestParams,
            out String exportString,
            out TVerificationResultCollection AMessages)
        {
            FStringWriter = new StringWriter();
            GiftBatchTDS MainDS = new GiftBatchTDS();
            FDelimiter = (String)requestParams["Delimiter"];
            FLedgerNumber = (Int32)requestParams["ALedgerNumber"];
            FDateFormatString = (String)requestParams["DateFormatString"];
            bool Summary = (bool)requestParams["Summary"];

            FUseBaseCurrency = (bool)requestParams["bUseBaseCurrency"];
            FDateForSummary = (DateTime)requestParams["DateForSummary"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            FCultureInfo = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FTransactionsOnly = (bool)requestParams["TransactionsOnly"];
            FExtraColumns = (bool)requestParams["ExtraColumns"];

            FTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Exporting Gift Batches"), 100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Retrieving records"),
                5);

            try
            {
                ALedgerAccess.LoadByPrimaryKey(MainDS, FLedgerNumber, FTransaction);

                List <OdbcParameter>parameters = new List <OdbcParameter>();

                SortedList <String, String>SQLCommandDefines = new SortedList <string, string>();

                if ((bool)requestParams["IncludeUnposted"])
                {
                    SQLCommandDefines.Add("INCLUDEUNPOSTED", string.Empty);
                }

                OdbcParameter param = new OdbcParameter("LedgerNumber", OdbcType.Int);
                param.Value = FLedgerNumber;
                parameters.Add(param);

                Int64 recipientNumber = (Int64)requestParams["RecipientNumber"];
                Int64 fieldNumber = (Int64)requestParams["FieldNumber"];

                if (recipientNumber != 0)
                {
                    SQLCommandDefines.Add("BYRECIPIENT", string.Empty);
                    param = new OdbcParameter("RecipientNumber", OdbcType.Int);
                    param.Value = recipientNumber;
                    parameters.Add(param);
                }

                if (fieldNumber != 0)
                {
                    SQLCommandDefines.Add("BYFIELD", string.Empty);
                    param = new OdbcParameter("fieldNumber", OdbcType.Int);
                    param.Value = fieldNumber;
                    parameters.Add(param);
                }

                if (requestParams.ContainsKey("BatchNumberStart"))
                {
                    SQLCommandDefines.Add("BYBATCHNUMBER", string.Empty);
                    param = new OdbcParameter("BatchNumberStart", OdbcType.Int);
                    param.Value = (Int32)requestParams["BatchNumberStart"];
                    parameters.Add(param);
                    param = new OdbcParameter("BatchNumberEnd", OdbcType.Int);
                    param.Value = (Int32)requestParams["BatchNumberEnd"];
                    parameters.Add(param);
                }
                else
                {
                    SQLCommandDefines.Add("BYDATERANGE", string.Empty);
                    param = new OdbcParameter("BatchDateFrom", OdbcType.DateTime);
                    param.Value = (DateTime)requestParams["BatchDateFrom"];
                    parameters.Add(param);
                    param = new OdbcParameter("BatchDateTo", OdbcType.DateTime);
                    param.Value = (DateTime)requestParams["BatchDateTo"];
                    parameters.Add(param);
                }

                string sqlStatement = TDataBase.ReadSqlFile("Gift.GetGiftsToExport.sql", SQLCommandDefines);

                DBAccess.GDBAccessObj.Select(MainDS,
                    "SELECT DISTINCT PUB_a_gift_batch.* " + sqlStatement + " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName(),
                    MainDS.AGiftBatch.TableName,
                    FTransaction,
                    parameters.ToArray());

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Retrieving gift records"),
                    10);

                DBAccess.GDBAccessObj.Select(MainDS,
                    "SELECT DISTINCT PUB_a_gift.* " + sqlStatement + " ORDER BY " + AGiftBatchTable.GetBatchNumberDBName() + ", " +
                    AGiftTable.GetGiftTransactionNumberDBName(),
                    MainDS.AGift.TableName,
                    FTransaction,
                    parameters.ToArray());

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Retrieving gift detail records"),
                    15);

                DBAccess.GDBAccessObj.Select(MainDS,
                    "SELECT DISTINCT PUB_a_gift_detail.* " + sqlStatement,
                    MainDS.AGiftDetail.TableName,
                    FTransaction,
                    parameters.ToArray());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            string BaseCurrency = MainDS.ALedger[0].BaseCurrency;
            FCurrencyCode = BaseCurrency; // Depending on FUseBaseCurrency, this will be overwritten for each gift.

            SortedDictionary <String, AGiftSummaryRow>sdSummary = new SortedDictionary <String, AGiftSummaryRow>();

            UInt32 counter = 0;

            // TProgressTracker Variables
            UInt32 GiftCounter = 0;

            AGiftSummaryRow giftSummary = null;

            MainDS.AGiftDetail.DefaultView.Sort =
                AGiftDetailTable.GetLedgerNumberDBName() + "," +
                AGiftDetailTable.GetBatchNumberDBName() + "," +
                AGiftDetailTable.GetGiftTransactionNumberDBName();

            foreach (AGiftBatchRow giftBatch in MainDS.AGiftBatch.Rows)
            {
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    string.Format(Catalog.GetString("Batch {0}"), giftBatch.BatchNumber),
                    20);
                GiftCounter = 0;

                if (!FTransactionsOnly & !Summary)
                {
                    WriteGiftBatchLine(giftBatch);
                }

                foreach (AGiftRow gift in MainDS.AGift.Rows)
                {
                    if (gift.BatchNumber.Equals(giftBatch.BatchNumber) && gift.LedgerNumber.Equals(giftBatch.LedgerNumber))
                    {
                        // Update progress tracker every 25 records
                        if (++GiftCounter % 25 == 0)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                string.Format(Catalog.GetString("Batch {0} - Exporting gifts"), giftBatch.BatchNumber),
                                (GiftCounter / 25 + 4) * 5 > 90 ? 90 : (GiftCounter / 25 + 4) * 5);
                        }

                        DataRowView[] selectedRowViews = MainDS.AGiftDetail.DefaultView.FindRows(
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

            exportString = FStringWriter.ToString();
            AMessages = FMessages;
            return MainDS.AGiftBatch.Count;
        }

        private String PartnerShortName(Int64 partnerKey)
        {
            if (partnerKey > 0)
            {
                // Get Partner ShortName
                PPartnerTable pt = PPartnerAccess.LoadByPrimaryKey(partnerKey,
                    StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), null, null, 0, 0);

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
            WriteDate(FDateForSummary);
            WriteStringQuoted(giftSummary.CurrencyCode);
            WriteGeneralNumber(giftSummary.ExchangeRateToBase);
            WriteStringQuoted(giftSummary.BankCostCentre);
            WriteStringQuoted(giftSummary.GiftType, true);
        }

        void WriteGiftBatchLine(AGiftBatchRow giftBatch)
        {
            WriteStringQuoted("B");
            WriteStringQuoted(giftBatch.BatchDescription);
            WriteStringQuoted(giftBatch.BankAccountCode);
            WriteCurrency(giftBatch.HashTotal);
            WriteDate(giftBatch.GlEffectiveDate);
            WriteStringQuoted(giftBatch.CurrencyCode);
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
            WriteBoolean(giftDetails.TaxDeductable, true);
//            WriteLineDate(gift.DateEntered);  //Don't write this - it can't be used by import.
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
                FStringWriter.Write(StringHelper.FormatUsingCurrencyCode(currencyField, FCurrencyCode));
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