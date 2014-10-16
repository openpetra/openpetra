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
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

//using Ict.Petra.Server.MFinance.Account.Data.Access;
//using Ict.Petra.Shared.MFinance.Account.Data;


namespace Ict.Petra.Server.MFinance.Gift
{
    /// <summary>
    /// Import a Gift Batch
    /// </summary>
    public class TGiftImporting
    {
        String FDelimiter;
        Int32 FLedgerNumber;
        String FDateFormatString;
        bool FExtraColumns;
        TDBTransaction FTransaction;
        GiftBatchTDS FMainDS;
        CultureInfo FCultureInfoNumberFormat;
        CultureInfo FCultureInfoDate;


        private String FImportMessage;
        private String FImportLine;
        private String FNewLine;

        private String InferCostCentre(AGiftDetailRow AgiftDetails)
        {
            String costCentre = "";

            if (!Common.Common.HasPartnerCostCentreLink(AgiftDetails.RecipientKey, out costCentre))
            {
                // There's no helpful entry in a_valid_ledger_number - I'll see about using the MotivationDetail.
                AMotivationDetailRow mdRow = FMainDS.AMotivationDetail.NewRowTyped(false);
                mdRow.LedgerNumber = AgiftDetails.LedgerNumber;
                mdRow.MotivationGroupCode = AgiftDetails.MotivationGroupCode;
                mdRow.MotivationDetailCode = AgiftDetails.MotivationDetailCode;
                AMotivationDetailTable tempTbl = null;

                TDBTransaction Transaction = null;
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        tempTbl = AMotivationDetailAccess.LoadUsingTemplate(mdRow, Transaction);
                    });

                if (tempTbl.Rows.Count > 0)
                {
                    costCentre = tempTbl[0].CostCentreCode;
                }
            }

            return costCentre;
        }

        /// <summary>
        /// Import Gift batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        public bool ImportGiftBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing Gift Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Parsing first line"),
                0);

            AMessages = new TVerificationResultCollection();
            FMainDS = new GiftBatchTDS();
            StringReader sr = new StringReader(importString);

            FDelimiter = (String)requestParams["Delimiter"];
            FLedgerNumber = (Int32)requestParams["ALedgerNumber"];
            FDateFormatString = (String)requestParams["DateFormatString"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            FNewLine = (String)requestParams["NewLine"];

            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            bool TaxDeductiblePercentageEnabled = Convert.ToBoolean(
                TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));

            //Assume it is a new transaction
            bool NewTransaction = true;

            //TDBTransaction FTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            FTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

            //Set this to true to commit or rollback the calling transaction at this point
            NewTransaction = true;

            AGiftBatchRow giftBatch = null;
            //AGiftRow gift = null;
            decimal totalBatchAmount = 0;
            FImportMessage = Catalog.GetString("Parsing first line");
            Int32 RowNumber = 0;
            Int32 BatchDetailCounter = 0;
            bool ok = false;

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, FTransaction);

                if (LedgerTable.Rows.Count == 0)
                {
                    AMessages.Add(new TVerificationResult(Catalog.GetString("Gift Batch Import"),
                            String.Format(Catalog.GetString("Ledger {0} doesn't exist."),
                                FLedgerNumber),
                            TResultSeverity.Resv_Critical));
                    return false;
                }

                AGiftRow previousGift = null;

                while ((FImportLine = sr.ReadLine()) != null)
                {
                    RowNumber++;

                    // skip empty lines and commented lines
                    if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                    {
                        string RowType = ImportString(Catalog.GetString("row type"));

                        if (RowType == "B")
                        {
                            //Check if
                            if ((previousGift != null) && (giftBatch != null))
                            {
                                //New batch so set total amount of Batch for previous batch
                                giftBatch.BatchTotal = totalBatchAmount;

                                AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, FTransaction);

//                              FMainDS.AGiftBatch.AcceptChanges();  // changed to use a new TDS for each batch.
                                FMainDS = new GiftBatchTDS();

                                previousGift = null;
                            }

                            totalBatchAmount = 0;


                            string BatchDescription = ImportString(Catalog.GetString("batch description"), AGiftBatchTable.GetBatchDescriptionLength());
                            string BankAccountCode = ImportString(Catalog.GetString("bank account code"), AGiftBatchTable.GetBankAccountCodeLength());
                            decimal HashTotal = ImportDecimal(Catalog.GetString("hash total"));
                            DateTime GlEffectiveDate = ImportDate(Catalog.GetString("effective Date"));
                            FImportMessage = "Creating new batch";

                            giftBatch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref FMainDS,
                                ref FTransaction,
                                ref LedgerTable,
                                FLedgerNumber,
                                GlEffectiveDate);

                            giftBatch.BatchDescription = BatchDescription;
                            giftBatch.BankAccountCode = BankAccountCode;
                            giftBatch.HashTotal = HashTotal;
                            giftBatch.CurrencyCode = ImportString(Catalog.GetString("currency code"), AGiftBatchTable.GetCurrencyCodeLength());
                            giftBatch.ExchangeRateToBase = ImportDecimal(Catalog.GetString("exchange rate to base"));

                            if (giftBatch.ExchangeRateToBase > 10000000)  // Huge numbers here indicate that the decimal comma/point is incorrect.
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Gift Batch Import"),
                                        String.Format(Catalog.GetString("huge exchange rate of {0} suggest decimal point format problem."),
                                            giftBatch.ExchangeRateToBase),
                                        TResultSeverity.Resv_Critical));
                                return false;
                            }

                            giftBatch.BankCostCentre = ImportString(Catalog.GetString("bank cost centre"), AGiftBatchTable.GetBankCostCentreLength());
                            giftBatch.GiftType = ImportString(Catalog.GetString("gift type"), AGiftBatchTable.GetGiftTypeLength());
                            FImportMessage = Catalog.GetString("Saving gift batch");

                            AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, FTransaction);

                            FMainDS.AGiftBatch.AcceptChanges();

                            BatchDetailCounter = 0;
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                string.Format(Catalog.GetString("Batch {0}"), giftBatch.BatchNumber),
                                10);
                        }
                        else if (RowType == "T")
                        {
                            int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count;

                            if (numberOfElements < 12) // Perhaps this CSV file is a summary, and can't be imported?
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Gift Batch Import"),
                                        Catalog.GetString("CSV Format problem: Not enough fields in gift row\n(This may be a summary?)"),
                                        TResultSeverity.Resv_Critical));
                                return false;
                            }

                            //this is the format with extra columns
                            FExtraColumns = numberOfElements > 22;

                            if (giftBatch == null)
                            {
                                FImportMessage = Catalog.GetString("Expected a GiftBatch line, but found a Gift");
                                throw new Exception();
                            }

                            AGiftRow gift = FMainDS.AGift.NewRowTyped(true);

                            gift.DonorKey = ImportInt64(Catalog.GetString("donor key"));
                            ImportString(Catalog.GetString("short name of donor (unused)")); // unused

                            gift.MethodOfGivingCode = ImportString(Catalog.GetString("method of giving Code"), AGiftTable.GetMethodOfGivingCodeLength());
                            gift.MethodOfPaymentCode = ImportString(Catalog.GetString(
                                    "method Of Payment Code"), AGiftTable.GetMethodOfPaymentCodeLength());
                            gift.Reference = ImportString(Catalog.GetString("reference"), AGiftTable.GetReferenceLength());
                            gift.ReceiptLetterCode = ImportString(Catalog.GetString("receipt letter code"), AGiftTable.GetReceiptLetterCodeLength());

                            if (FExtraColumns)
                            {
                                gift.ReceiptNumber = ImportInt32(Catalog.GetString("receipt number"));
                                gift.FirstTimeGift = ImportBoolean(Catalog.GetString("first time gift"));
                                gift.ReceiptPrinted = ImportBoolean(Catalog.GetString("receipt printed"));
                            }

                            GiftBatchTDSAGiftDetailRow giftDetails = FMainDS.AGiftDetail.NewRowTyped(true);

                            if ((previousGift != null) && (gift.DonorKey == previousGift.DonorKey)
                                && (gift.MethodOfGivingCode == previousGift.MethodOfGivingCode)
                                && (gift.MethodOfPaymentCode == previousGift.MethodOfPaymentCode)
                                && (gift.Reference == previousGift.Reference)
                                && (gift.ReceiptLetterCode == previousGift.ReceiptLetterCode)
                                && (gift.ReceiptNumber == previousGift.ReceiptNumber)
                                && (gift.FirstTimeGift == previousGift.FirstTimeGift)
                                && (gift.ReceiptPrinted == previousGift.ReceiptPrinted))
                            {
                                // this row is a new detail for the previousGift
                                gift = previousGift;
                                gift.LastDetailNumber++;
                                giftDetails.DetailNumber = gift.LastDetailNumber;
                            }
                            else
                            {
                                previousGift = gift;
                                gift.LedgerNumber = giftBatch.LedgerNumber;
                                gift.BatchNumber = giftBatch.BatchNumber;
                                gift.GiftTransactionNumber = giftBatch.LastGiftNumber + 1;
                                giftBatch.LastGiftNumber++;
                                gift.LastDetailNumber = 1;
                                FMainDS.AGift.Rows.Add(gift);
                                giftDetails.DetailNumber = 1;
                            }

                            giftDetails.LedgerNumber = gift.LedgerNumber;
                            giftDetails.BatchNumber = giftBatch.BatchNumber;
                            giftDetails.GiftTransactionNumber = gift.GiftTransactionNumber;
                            FMainDS.AGiftDetail.Rows.Add(giftDetails);

                            giftDetails.RecipientKey = ImportInt64(Catalog.GetString("recipient key"));
                            ImportString(Catalog.GetString("short name of recipient (unused)")); // unused

                            if (FExtraColumns)
                            {
                                giftDetails.RecipientLedgerNumber = ImportInt32(Catalog.GetString("recipient ledger number"));
                            }

                            decimal currentGiftAmount = ImportDecimal(Catalog.GetString("Gift amount"));
                            giftDetails.GiftAmount = currentGiftAmount;
                            giftDetails.GiftTransactionAmount = currentGiftAmount;
                            totalBatchAmount += currentGiftAmount;
                            // TODO: currency translation

                            if (FExtraColumns)
                            {
                                giftDetails.GiftAmountIntl = ImportDecimal(Catalog.GetString("gift amount intl"));
                            }

                            giftDetails.ConfidentialGiftFlag = ImportBoolean(Catalog.GetString("confidential gift"));
                            giftDetails.MotivationGroupCode = ImportString(Catalog.GetString(
                                    "motivation group code"), AGiftDetailTable.GetMotivationGroupCodeLength());
                            giftDetails.MotivationDetailCode = ImportString(Catalog.GetString(
                                    "motivation detail"), AGiftDetailTable.GetMotivationDetailCodeLength());

                            if (giftDetails.MotivationGroupCode == "") // Perhaps this CSV file is a summary, and can't be imported?
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Gift Batch Import"),
                                        Catalog.GetString("CSV Format problem: No Motivation Group code."),
                                        TResultSeverity.Resv_Critical));
                                return false;
                            }

                            if (FExtraColumns)
                            {
                                giftDetails.CostCentreCode = ImportString(Catalog.GetString(
                                        "cost centre code"), AGiftDetailTable.GetCostCentreCodeLength());
                            }
                            else
                            {
                                //
                                // "In Petra Cost Centre is always inferred from recipient field and motivation detail so is not needed in the import."
                                giftDetails.CostCentreCode = InferCostCentre(giftDetails);
                            }

                            giftDetails.GiftCommentOne = ImportString(Catalog.GetString("comment one"), AGiftDetailTable.GetGiftCommentOneLength());
                            giftDetails.CommentOneType = ImportString(Catalog.GetString("comment one type"), AGiftDetailTable.GetCommentOneTypeLength());

                            giftDetails.MailingCode = ImportString(Catalog.GetString("mailing code"));

                            giftDetails.GiftCommentTwo = ImportString(Catalog.GetString("gift comment two"), AGiftDetailTable.GetGiftCommentTwoLength());
                            giftDetails.CommentTwoType = ImportString(Catalog.GetString("comment two type"), AGiftDetailTable.GetCommentTwoTypeLength());
                            giftDetails.GiftCommentThree = ImportString(Catalog.GetString(
                                    "gift comment three"), AGiftDetailTable.GetGiftCommentThreeLength());
                            giftDetails.CommentThreeType = ImportString(Catalog.GetString(
                                    "comment three type"), AGiftDetailTable.GetCommentThreeTypeLength());
                            giftDetails.TaxDeductible = ImportBoolean(Catalog.GetString("tax deductible"));
                            
                            if (TaxDeductiblePercentageEnabled)
                            {
                            	// Sets TaxDeductiblePct and uses it to calculate the tax deductibility amounts for a Gift Detail
                        		TGift.SetDefaultTaxDeductibilityData(ref giftDetails, gift.DateEntered, FTransaction);
                            }

                            gift.DateEntered = giftBatch.GlEffectiveDate;

                            FImportMessage = Catalog.GetString("Saving gift");

                            AGiftAccess.SubmitChanges(FMainDS.AGift, FTransaction);

                            FMainDS.AGift.AcceptChanges();
                            FImportMessage = Catalog.GetString("Saving giftdetails");

                            AGiftDetailAccess.SubmitChanges(FMainDS.AGiftDetail, FTransaction);

                            FMainDS.AGiftDetail.AcceptChanges();

                            // Update progress tracker every 50 records
                            if (++BatchDetailCounter % 50 == 0)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    string.Format(Catalog.GetString("Batch {0} - Importing gift detail"), giftBatch.BatchNumber),
                                    (BatchDetailCounter / 50 + 2) * 10 > 90 ? 90 : (BatchDetailCounter / 50 + 2) * 10);
                            }
                        } // If known row type
                        else
                        {
                            throw new Exception(Catalog.GetString("Invalid Row Type. Perhaps using wrong CSV separator?"));
                        }
                    }  // if the CSV line qualifies

                }  // while CSV lines

                //Update batch total for the last batch entered.
                if (giftBatch != null)
                {
                    giftBatch.BatchTotal = totalBatchAmount;
                }

                FImportMessage = Catalog.GetString("Saving all data into the database");

                //Finally save pending changes (the last number is updated !)
                AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, FTransaction);

                ALedgerAccess.SubmitChanges(LedgerTable, FTransaction);

                FMainDS.AGiftBatch.AcceptChanges();
                FMainDS.ALedger.AcceptChanges();

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }

                ok = true;
            }
            catch (Exception ex)
            {
                String speakingExceptionText = SpeakingExceptionMessage(ex);

                if (AMessages == null)
                {
                    AMessages = new TVerificationResultCollection();
                }

                AMessages.Add(new TVerificationResult(Catalog.GetString("Import"),
                        String.Format(Catalog.GetString("There is a problem parsing the file in row {0}:"), RowNumber) +
                        FNewLine +
                        Catalog.GetString(FImportMessage) + FNewLine + speakingExceptionText,
                        TResultSeverity.Resv_Critical));

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Exception Occurred"),
                    0);

                return false;
            }
            finally
            {
                try
                {
                    sr.Close();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured while closing the Import File:" + Environment.NewLine + Exc.ToString());

                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult(Catalog.GetString("Import"),
                            Catalog.GetString("A problem was encountered while closing the Import File:"),
                            TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Exception Occurred"),
                        0);

                    TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                    throw;
                }

                if (ok)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Gift batch import successful"),
                        100);
                }
                else
                {
                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult(Catalog.GetString("Import"),
                            Catalog.GetString("Data could not be saved."),
                            TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Data could not be saved."),
                        0);
                }

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            }

            return true;
        }

        /// <summary>
        /// returns the most recently imported gift batch
        /// </summary>
        public Int32 GetLastGiftBatchNumber()
        {
            if ((FMainDS != null) && (FMainDS.AGiftBatch != null) && (FMainDS.AGiftBatch.Count > 0))
            {
                return FMainDS.AGiftBatch[FMainDS.AGiftBatch.Count - 1].BatchNumber;
            }

            return -1;
        }

        private String SpeakingExceptionMessage(Exception ex)
        {
            //note that this is only done for "user errors" not for program errors!
            String theExMessage = ex.Message;

            if (theExMessage.Contains("a_gift_batch_fk2"))
            {
                return Catalog.GetString("Invalid account code");
            }

            if (theExMessage.Contains("a_gift_batch_fk3"))
            {
                return Catalog.GetString("Invalid cost centre");
            }

            if (theExMessage.Contains("a_gift_batch_fk4"))
            {
                return Catalog.GetString("Invalid currency code");
            }

            if (theExMessage.Contains("a_gift_fk2"))
            {
                return Catalog.GetString("Invalid method of giving");
            }

            if (theExMessage.Contains("a_gift_fk3"))
            {
                return Catalog.GetString("Invalid method of payment");
            }

            if (theExMessage.Contains("a_gift_fk4"))
            {
                return Catalog.GetString("Invalid donor partner key");
            }

            if (theExMessage.Contains("a_gift_detail_fk2"))
            {
                return Catalog.GetString("Invalid motivation detail");
            }

            if (theExMessage.Contains("a_gift_detail_fk3"))
            {
                return Catalog.GetString("Invalid recipient partner key");
            }

            if (theExMessage.Contains("a_gift_detail_fk4"))
            {
                return Catalog.GetString("Invalid mailing code");
            }

            if (theExMessage.Contains("a_gift_detail_fk5"))
            {
                return Catalog.GetString("Invalid recipient ledger number");
            }

            if (theExMessage.Contains("a_gift_detail_fk6"))
            {
                return Catalog.GetString("Invalid cost centre");
            }

            TLogging.Log("Importing Gift batch: " + ex.ToString());

            return ex.Message;
        }

        private String ImportString(String message, Int32 AmaximumLength = -1)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);

            if (sReturn.Length == 0)
            {
                return null;
            }

            if ((AmaximumLength > 0) && (sReturn.Length > AmaximumLength))
            {
                throw new Exception(String.Format(Catalog.GetString("Maximum field length ({0}) exceeded."), AmaximumLength));
            }

            return sReturn;
        }

        private Boolean ImportBoolean(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return sReturn.ToLower().Equals("yes");
        }

        private Int64 ImportInt64(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return Convert.ToInt64(sReturn);
        }

        private Int32 ImportInt32(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return Convert.ToInt32(sReturn);
        }

        private decimal ImportDecimal(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            decimal dec = Convert.ToDecimal(sReturn, FCultureInfoNumberFormat);
            return dec;
        }

        private DateTime ImportDate(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sDate = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            DateTime dtReturn;

            try
            {
                dtReturn = Convert.ToDateTime(sDate, FCultureInfoDate);
            }
            catch (Exception)
            {
                TLogging.Log("Problem parsing " + sDate + " with format " + FCultureInfoDate.DateTimeFormat.ShortDatePattern);
                throw;
            }
            return dtReturn;
        }
    }
}