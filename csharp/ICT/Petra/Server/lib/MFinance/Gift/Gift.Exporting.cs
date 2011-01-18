//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash
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
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;

//using Ict.Petra.Server.MFinance.Account.Data.Access;
//using Ict.Petra.Shared.MFinance.Account.Data;


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
        StringWriter FStringWriter;
        String FDelimiter;
        Int32 FLedgerNumber;
        String FDateFormatString;
        CultureInfo FCultureInfo;
        bool FSummary;
        bool FUseBaseCurrency;
        String FBaseCurrency;
        DateTime FDateForSummary;
        bool FTransactionsOnly;
        bool FExtraColumns;
        TDBTransaction FTransaction;
        Int64 FRecipientNumber;
        Int64 FFieldNumber;
        GiftBatchTDS FMainDS;
        GLSetupTDS FSetupTDS;
        TVerificationResultCollection FMessages = new TVerificationResultCollection();


        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches">Arraylist containing the batch numbers of the gift batches to export</param>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="exportString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if not completed</returns>
        public bool ExportAllGiftBatchData(ref ArrayList batches,
            Hashtable requestParams,
            out String exportString,
            out TVerificationResultCollection AMessages)
        {
            FStringWriter = new StringWriter();
            StringBuilder line = new StringBuilder();
            FMainDS = new GiftBatchTDS();
            FSetupTDS = new GLSetupTDS();
            FDelimiter = (String)requestParams["Delimiter"];
            FLedgerNumber = (Int32)requestParams["ALedgerNumber"];
            FDateFormatString = (String)requestParams["DateFormatString"];
            FSummary = (bool)requestParams["Summary"];
            FUseBaseCurrency = (bool)requestParams["bUseBaseCurrency"];
            FBaseCurrency = (String)requestParams["BaseCurrency"];
            FDateForSummary = (DateTime)requestParams["DateForSummary"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            FCultureInfo = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FTransactionsOnly = (bool)requestParams["TransactionsOnly"];
            FRecipientNumber = (Int64)requestParams["RecipientNumber"];
            FFieldNumber = (Int64)requestParams["FieldNumber"];
            FExtraColumns = (bool)requestParams["ExtraColumns"];


            SortedDictionary <String, AGiftSummaryRow>sdSummary = new SortedDictionary <String, AGiftSummaryRow>();

            FTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            while (batches.Count > 0)
            {
                Int32 ABatchNumber = (Int32)batches[0];
                AGiftBatchAccess.LoadByPrimaryKey(FMainDS, FLedgerNumber, ABatchNumber, FTransaction);
                AGiftAccess.LoadViaAGiftBatch(FMainDS, FLedgerNumber, ABatchNumber, FTransaction);

                foreach (AGiftRow gift in FMainDS.AGift.Rows)
                {
                    if (gift.BatchNumber.Equals(ABatchNumber) && gift.LedgerNumber.Equals(FLedgerNumber))
                    {
                        AGiftDetailAccess.LoadViaAGift(FMainDS, gift.LedgerNumber,
                            gift.BatchNumber,
                            gift.GiftTransactionNumber,
                            FTransaction);
                    }
                }

                batches.RemoveAt(0);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            UInt32 counter = 0;
            AGiftSummaryRow giftSummary = null;

            foreach (AGiftBatchRow giftBatch in FMainDS.AGiftBatch.Rows)
            {
                if (!FTransactionsOnly & !FSummary)
                {
                    WriteGiftBatchLine(giftBatch);
                }

                //foreach (AGiftRow gift in giftDS.AGift.Rows)
                foreach (AGiftRow gift in FMainDS.AGift.Rows)
                {
                    String mapCurrency;

                    if (gift.BatchNumber.Equals(giftBatch.BatchNumber) && gift.LedgerNumber.Equals(giftBatch.LedgerNumber))
                    {
//                        else
//                        {
//                            if (!FTransactionsOnly)
//                            {
////                                WriteGiftLine(gift);
//                            }
//                        }

                        FMainDS.AGiftDetail.DefaultView.Sort = AGiftDetailTable.GetDetailNumberDBName();
                        FMainDS.AGiftDetail.DefaultView.RowFilter =
                            String.Format("{0}={1} and {2}={3} and {4}={5}",
                                AGiftDetailTable.GetLedgerNumberDBName(),
                                gift.LedgerNumber,
                                AGiftDetailTable.GetBatchNumberDBName(),
                                gift.BatchNumber,
                                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                                gift.GiftTransactionNumber);

                        foreach (DataRowView dv in FMainDS.AGiftDetail.DefaultView)
                        {
                            AGiftDetailRow giftDetail = (AGiftDetailRow)dv.Row;
                            Ict.Petra.Server.MPartner.Partner.Data.Access.PPartnerAccess.LoadByPrimaryKey(FSetupTDS,
                                giftDetail.RecipientKey,
                                FTransaction);

                            if (FSummary)
                            {
                                mapCurrency = FUseBaseCurrency ? FBaseCurrency : giftBatch.CurrencyCode;
                                decimal mapExchangeRateToBase = FUseBaseCurrency ? 1 : giftBatch.ExchangeRateToBase;


                                counter++;
                                String DictionaryKey = mapCurrency + ";" + giftBatch.BankCostCentre + ";" + giftBatch.BankAccountCode + ";" +
                                                       giftDetail.RecipientKey + ";" + giftDetail.MotivationGroupCode + ";" +
                                                       giftDetail.MotivationGroupCode;

                                if (sdSummary.TryGetValue(DictionaryKey, out giftSummary))
                                {
                                    giftSummary.GiftTransactionAmount += giftDetail.GiftTransactionAmount;
                                    giftSummary.GiftTransactionAmount += giftDetail.GiftAmount;
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
                                    giftSummary.CurrencyCode = mapCurrency;
                                    giftSummary.BankCostCentre = giftBatch.BankCostCentre;
                                    giftSummary.BankAccountCode = giftBatch.BankAccountCode;
                                    giftSummary.GiftTransactionAmount = giftDetail.GiftTransactionAmount;
                                    giftSummary.GiftAmount = giftDetail.GiftAmount;


                                    sdSummary.Add(DictionaryKey, giftSummary);
                                }

                                //overwrite always because we want to have the last
                                giftSummary.ExchangeRateToBase = mapExchangeRateToBase;
                            }
                            else
                            {
                                WriteGiftLine(gift, giftDetail);
                            }
                        }
                    }
                }
            }

            if (FSummary)
            {
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

            exportString = FStringWriter.ToString();
            AMessages = FMessages;
            return true; //true=complete TODO (if needed) find reasonable limit for FStringWriter in main memory, interrupt
        }

        private bool GiftRestricted(AGiftRow GiftDR)
        {
            SGroupGiftTable GroupGiftDT;
            SUserGroupTable UserGroupDT;
            Int16 Counter;

            DataRow[] FoundUserGroups;

            if (GiftDR.Restricted)
            {
                GroupGiftDT = SGroupGiftAccess.LoadViaAGift(
                    GiftDR.LedgerNumber,
                    GiftDR.BatchNumber,
                    GiftDR.GiftTransactionNumber,
                    FTransaction);
                UserGroupDT = SUserGroupAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, FTransaction);

                // Loop over all rows of GroupGiftDT
                for (Counter = 0; Counter <= GroupGiftDT.Rows.Count - 1; Counter += 1)
                {
                    // To be able to view a Gift, ReadAccess must be granted
                    if (GroupGiftDT[Counter].ReadAccess)
                    {
                        // Find out whether the user has a row in s_user_group with the
                        // GroupID of the GroupGift row
                        FoundUserGroups = UserGroupDT.Select(SUserGroupTable.GetGroupIdDBName() + " = '" + GroupGiftDT[Counter].GroupId + "'");

                        if (FoundUserGroups.Length != 0)
                        {
                            // The gift is not restricted because there is a read access for the group
                            return false;
                            // don't evaluate further GroupGiftDT rows
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private String PartnerShortName(Int64 partnerKey)
        {
            if (partnerKey > 0)
            {
                // Get Partner ShortName
                PPartnerTable pt = PPartnerAccess.LoadByPrimaryKey(partnerKey,
                    StringHelper.InitStrArr(new String[] { PPartnerTable.GetPartnerShortNameDBName() }), FTransaction, null, 0, 0);

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
            WriteLineStringQuoted(giftSummary.GiftType);
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
            WriteLineStringQuoted(giftBatch.GiftType);
        }

        void WriteGiftLine(AGiftRow gift, AGiftDetailRow giftDetails)
        {
            if (!FTransactionsOnly)
            {
                WriteStringQuoted("T");
            }

            if (GiftRestricted(gift))
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
            WriteStringQuoted(giftDetails.CostCentreCode);
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
            WriteLineBoolean(giftDetails.TaxDeductable);
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
            WriteGeneralNumber(tempKey);                        //is this the right Ledger Number?
            WriteStringQuoted(PartnerShortName(tempKey));
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteStringQuoted("");
            WriteGeneralNumber(giftSummary.RecipientKey);
            WriteStringQuoted(PartnerShortName(giftSummary.RecipientKey));                        //TODO: Replaces this by p_partner.short_name

            if (FUseBaseCurrency)
            {
                WriteCurrency(giftSummary.GiftAmount);
            }
            else
            {
                WriteCurrency(giftSummary.GiftTransactionAmount);
            }
        }

        void WriteDelimiter(bool bLineEnd)
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

        void WriteStringQuoted(String theString, bool bLineEnd)
        {
            FStringWriter.Write(quote);
            FStringWriter.Write(theString);
            FStringWriter.Write(quote);
            WriteDelimiter(bLineEnd);
        }

        void WriteCurrency(decimal currencyField, bool bLineEnd)
        {
            Int64 integerNumber = Convert.ToInt64(currencyField);

            if (Convert.ToDecimal(integerNumber) == currencyField)
            {
                FStringWriter.Write(String.Format("{0:d}", integerNumber));
            }
            else
            {
                FStringWriter.Write(String.Format(FCultureInfo, "{0:###########0.00}", currencyField));
            }

            WriteDelimiter(bLineEnd);
        }

        void WriteGeneralNumber(decimal generalNumberField, bool bLineEnd)
        {
            Int64 integerNumber = Convert.ToInt64(generalNumberField);


            FStringWriter.Write(String.Format(FCultureInfo, "{0:g}", generalNumberField));


            WriteDelimiter(bLineEnd);
        }

        void WriteBoolean(bool aBool, bool bLineEnd)
        {
            FStringWriter.Write(aBool ? "yes" : "no");
            WriteDelimiter(bLineEnd);
        }

        void WriteDate(DateTime dateField, bool bLineEnd)
        {
            FStringWriter.Write(String.Format(FDateFormatString, dateField));
            WriteDelimiter(bLineEnd);
        }

        void WriteStringQuoted(String theString)
        {
            WriteStringQuoted(theString, false);
        }

        void WriteCurrency(decimal currencyField)
        {
            WriteGeneralNumber(currencyField, false);
        }

        void WriteGeneralNumber(decimal generalNumberField)
        {
            WriteGeneralNumber(generalNumberField, false);
        }

        void WriteBoolean(bool aBool)
        {
            WriteBoolean(aBool, false);
        }

        void WriteDate(DateTime dateField)
        {
            WriteDate(dateField, false);
        }

        void WriteLineStringQuoted(String theString)
        {
            WriteStringQuoted(theString, true);
        }

        void WriteLineCurrency(decimal currencyField)
        {
            WriteGeneralNumber(currencyField, true);
        }

        void WriteLineGeneralNumber(decimal generalNumberField)
        {
            WriteGeneralNumber(generalNumberField, true);
        }

        void WriteLineBoolean(bool aBool)
        {
            WriteBoolean(aBool, true);
        }

        void WriteLineDate(DateTime dateField)
        {
            WriteDate(dateField, true);
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