//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, dougm, alanP
//
// Copyright 2004-2014 by OM International
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
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Validation;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;


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
        GiftBatchTDS FMainDS;
        CultureInfo FCultureInfoNumberFormat;
        CultureInfo FCultureInfoDate;

        private String FImportLine;
        private String FNewLine;
        private string FLedgerBaseCurrency = String.Empty;
        private string FLedgerIntlCurrency = String.Empty;

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

        private bool UpdateDailyExchangeRateTable(ADailyExchangeRateTable DailyExchangeTable, string AFromCurrencyCode, string AToCurrencyCode,
            decimal AExchangeRate, DateTime AEffectiveDate)
        {
            string SortByTimeDescending = ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";

            string filter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=#{5}#",
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName(), AFromCurrencyCode,
                ADailyExchangeRateTable.GetToCurrencyCodeDBName(), AToCurrencyCode,
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                AEffectiveDate.ToString("d", CultureInfo.InvariantCulture));

            DataView dvTo = new DataView(DailyExchangeTable, filter, SortByTimeDescending, DataViewRowState.CurrentRows);
            bool foundMatch = false;

            for (int i = 0; i < dvTo.Count; i++)
            {
                if (((ADailyExchangeRateRow)dvTo[i].Row).RateOfExchange == AExchangeRate)
                {
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                int newTime = 3600;

                if (dvTo.Count > 0)
                {
                    newTime = ((ADailyExchangeRateRow)dvTo[0].Row).TimeEffectiveFrom + 600;
                }

                ADailyExchangeRateRow newRow = DailyExchangeTable.NewRowTyped();
                newRow.DateEffectiveFrom = AEffectiveDate;
                newRow.FromCurrencyCode = AFromCurrencyCode;
                newRow.ToCurrencyCode = AToCurrencyCode;
                newRow.TimeEffectiveFrom = newTime;
                newRow.RateOfExchange = AExchangeRate;
                DailyExchangeTable.Rows.Add(newRow);

                return true;
            }

            return false;
        }

        private void SetCommentTypeCase(ref string ACommentType)
        {
            if (ACommentType == null)
            {
                return;
            }

            if (String.Compare(ACommentType, MFinanceConstants.GIFT_COMMENT_TYPE_DONOR, false) == 0)
            {
                ACommentType = MFinanceConstants.GIFT_COMMENT_TYPE_DONOR;
            }

            if (String.Compare(ACommentType, MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT, false) == 0)
            {
                ACommentType = MFinanceConstants.GIFT_COMMENT_TYPE_RECIPIENT;
            }

            if (String.Compare(ACommentType, MFinanceConstants.GIFT_COMMENT_TYPE_BOTH, false) == 0)
            {
                ACommentType = MFinanceConstants.GIFT_COMMENT_TYPE_BOTH;
            }

            if (String.Compare(ACommentType, MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE, false) == 0)
            {
                ACommentType = MFinanceConstants.GIFT_COMMENT_TYPE_OFFICE;
            }
        }

        /// <summary>
        /// Import Gift batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="ARequestParams">Hashtable containing the given params </param>
        /// <param name="AImportString">Big parts of the export file as a simple String</param>
        /// <param name="ANeedRecipientLedgerNumber">Gifts in this table are responsible for failing the
        /// import becuase their Family recipients do not have an active Gift Destination</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        public bool ImportGiftBatches(
            Hashtable ARequestParams,
            String AImportString,
            out GiftBatchTDSAGiftDetailTable ANeedRecipientLedgerNumber,
            out TVerificationResultCollection AMessages
            )
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing Gift Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                0);

            TVerificationResultCollection Messages = new TVerificationResultCollection();

            // fix for Mono issue with out parameter: https://bugzilla.xamarin.com/show_bug.cgi?id=28196
            AMessages = Messages;

            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();

            FMainDS = new GiftBatchTDS();
            StringReader sr = new StringReader(AImportString);

            // Parse the supplied parameters
            FDelimiter = (String)ARequestParams["Delimiter"];
            FLedgerNumber = (Int32)ARequestParams["ALedgerNumber"];
            FDateFormatString = (String)ARequestParams["DateFormatString"];
            String NumberFormat = (String)ARequestParams["NumberFormat"];
            FNewLine = (String)ARequestParams["NewLine"];

            // Set culture from parameters
            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            bool TaxDeductiblePercentageEnabled = Convert.ToBoolean(
                TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));

            // Initialise our working variables
            AGiftBatchRow giftBatch = null;
            decimal totalBatchAmount = 0;
            Int32 RowNumber = 0;
            Int32 InitialTextLength = AImportString.Length;
            Int32 TextProcessedLength = 0;
            Int32 PercentDone = 10;
            Int32 PreviousPercentDone = 0;
            Boolean CancelledByUser = false;

            string ImportMessage = Catalog.GetString("Initialising");

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictBatch = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictGift = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictGiftDetail = new TValidationControlsDict();

            // This needs to be initialised because we will be calling the method
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;
            TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriodDelegate = @TAccountingPeriodsWebConnector.GetFirstDayOfAccountingPeriod;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        // Load supplementary tables that we are going to need for validation
                        ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, Transaction);
                        AAccountTable AccountTable = AAccountAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        ACostCentreTable CostCentreTable = ACostCentreAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMotivationDetailTable MotivationDetailTable = AMotivationDetailAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AAccountPropertyTable AccountPropertyTable = AAccountPropertyAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMethodOfGivingTable MethodOfGivingTable = AMethodOfGivingAccess.LoadAll(Transaction);
                        AMethodOfPaymentTable MethodOfPaymentTable = AMethodOfPaymentAccess.LoadAll(Transaction);
                        ACurrencyTable CurrencyTable = ACurrencyAccess.LoadAll(Transaction);
                        PMailingTable MailingTable = PMailingAccess.LoadAll(Transaction);

                        if (LedgerTable.Rows.Count == 0)
                        {
                            throw new Exception(String.Format(Catalog.GetString("Ledger {0} doesn't exist."), FLedgerNumber));
                        }

                        FLedgerBaseCurrency = ((ALedgerRow)LedgerTable.Rows[0]).BaseCurrency;
                        FLedgerIntlCurrency = ((ALedgerRow)LedgerTable.Rows[0]).IntlCurrency;

                        ACorporateExchangeRateTable CorporateExchangeToLedgerTable = ACorporateExchangeRateAccess.LoadViaACurrencyFromCurrencyCode(
                            FLedgerBaseCurrency,
                            Transaction);
                        ADailyExchangeRateTable DailyExchangeToLedgerTable =
                            ADailyExchangeRateAccess.LoadViaACurrencyToCurrencyCode(FLedgerBaseCurrency,
                                Transaction);
                        ADailyExchangeRateTable DailyExchangeToIntlTable =
                            ADailyExchangeRateAccess.LoadViaACurrencyToCurrencyCode(FLedgerIntlCurrency,
                                Transaction);

                        ImportMessage = Catalog.GetString("Parsing first line");
                        AGiftRow previousGift = null;

                        // Go round a loop reading the file line by line
                        FImportLine = sr.ReadLine();

                        while (FImportLine != null)
                        {
                            RowNumber++;

                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            PercentDone = 10 + ((TextProcessedLength * 90) / InitialTextLength);

                            // skip empty lines and commented lines
                            if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                            {
                                int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count;

                                // Read the row analysisType - there is no 'validation' on this so we can make the call with null parameters
                                string RowType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("row type"), null, null);

                                if (RowType == "B")
                                {
                                    ImportMessage = Catalog.GetString("Parsing a batch row");

                                    // It is a Batch row
                                    if (numberOfElements < 8)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString(
                                                    "Wrong number of batch columns.  The correct number is either 8 columns (in which case the gift type is assumed to be 'Gift') or 9 columns, which allows for alternative gift types."),
                                                TResultSeverity.Resv_Critical));

                                        FImportLine = sr.ReadLine();

                                        if (FImportLine != null)
                                        {
                                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                        }

                                        continue;
                                    }

                                    //Check if this is the start of a new batch (i.e. not the first batch)
                                    if ((previousGift != null) && (giftBatch != null))
                                    {
                                        //New batch so set total amount of Batch for previous batch
                                        giftBatch.BatchTotal = totalBatchAmount;

                                        if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                        {
                                            ImportMessage = Catalog.GetString("Saving batch");
                                            AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, Transaction);
                                            FMainDS.AGiftBatch.AcceptChanges();

                                            ImportMessage = Catalog.GetString("Saving gift");
                                            AGiftAccess.SubmitChanges(FMainDS.AGift, Transaction);
                                            FMainDS.AGift.AcceptChanges();

                                            ImportMessage = Catalog.GetString("Saving giftdetails");
                                            AGiftDetailAccess.SubmitChanges(FMainDS.AGiftDetail, Transaction);
                                            FMainDS.AGiftDetail.AcceptChanges();
                                        }

                                        previousGift = null;
                                    }

                                    ImportMessage = Catalog.GetString("Starting new batch");
                                    totalBatchAmount = 0;

                                    // Parse the complete line and validate it
                                    ParseBatchLine(ref giftBatch, ref Transaction, ref LedgerTable, ref ImportMessage, RowNumber, Messages,
                                        ValidationControlsDictBatch, AccountTable, AccountPropertyTable, AccountingPeriodTable, CostCentreTable,
                                        CorporateExchangeToLedgerTable, CurrencyTable);

                                    if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                    {
                                        // This row passes validation so we can do final actions if the batch is not in the ledger currency
                                        if (giftBatch.CurrencyCode != FLedgerBaseCurrency)
                                        {
                                            ImportMessage = Catalog.GetString("Updating foreign exchange data");

                                            // Validation will have ensured that we have a corporate rate for the effective date
                                            // We need to know what that rate is...
                                            DateTime firstOfMonth = new DateTime(giftBatch.GlEffectiveDate.Year,
                                                giftBatch.GlEffectiveDate.Month,
                                                1);
                                            ACorporateExchangeRateRow corporateRateRow =
                                                (ACorporateExchangeRateRow)CorporateExchangeToLedgerTable.Rows.Find(
                                                    new object[] { giftBatch.CurrencyCode, FLedgerBaseCurrency, firstOfMonth });
                                            decimal corporateRate = corporateRateRow.RateOfExchange;

                                            if (Math.Abs((giftBatch.ExchangeRateToBase - corporateRate) / corporateRate) > 0.20m)
                                            {
                                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.
                                                            StrImportValidationWarningInLine,
                                                            RowNumber),
                                                        String.Format(Catalog.GetString(
                                                                "The exchange rate of {0} differs from the Corporate Rate of {1} for the month commencing {2} by more than 20 percent."),
                                                            giftBatch.ExchangeRateToBase, corporateRate,
                                                            StringHelper.DateToLocalizedString(firstOfMonth)),
                                                        TResultSeverity.Resv_Noncritical));
                                            }

                                            // we need to create a daily exchange rate pair for the transaction date
                                            // start with To Ledger currency
                                            if (UpdateDailyExchangeRateTable(DailyExchangeToLedgerTable, giftBatch.CurrencyCode, FLedgerBaseCurrency,
                                                    giftBatch.ExchangeRateToBase, giftBatch.GlEffectiveDate))
                                            {
                                                ADailyExchangeRateAccess.SubmitChanges(DailyExchangeToLedgerTable, Transaction);
                                                DailyExchangeToLedgerTable.AcceptChanges();

                                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportInformationForLine,
                                                            RowNumber),
                                                        String.Format(Catalog.GetString(
                                                                "Added exchange rate of {0} to Daily Exchange Rate table for {1}"),
                                                            giftBatch.ExchangeRateToBase,
                                                            StringHelper.DateToLocalizedString(giftBatch.GlEffectiveDate)),
                                                        TResultSeverity.Resv_Info));
                                            }

                                            // Now the inverse for From Ledger currency
                                            ADailyExchangeRateTable DailyExchangeFromTable =
                                                ADailyExchangeRateAccess.LoadViaACurrencyFromCurrencyCode(giftBatch.CurrencyCode, Transaction);
                                            decimal inverseRate = Math.Round(1 / giftBatch.ExchangeRateToBase, 10);

                                            if (UpdateDailyExchangeRateTable(DailyExchangeFromTable, FLedgerBaseCurrency, giftBatch.CurrencyCode,
                                                    inverseRate, giftBatch.GlEffectiveDate))
                                            {
                                                ADailyExchangeRateAccess.SubmitChanges(DailyExchangeFromTable, Transaction);
                                            }
                                        }
                                    }
                                }
                                else if (RowType == "T")
                                {
                                    ImportMessage = Catalog.GetString("Parsing a transaction row");

                                    // It is a Transaction row
                                    if (numberOfElements < 13) // Perhaps this CSV file is a summary, and can't be imported?
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString(
                                                    "Wrong number of gift columns. Expected at least 13 columns. (This may be a summary?)"),
                                                TResultSeverity.Resv_Critical));
                                        FImportLine = sr.ReadLine();

                                        if (FImportLine != null)
                                        {
                                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                        }

                                        continue;
                                    }

                                    if (giftBatch == null)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString(
                                                    "Expected a GiftBatch line, but found a Gift Transaction. Will create a dummy working batch for the current period."),
                                                TResultSeverity.Resv_Critical));

                                        // in order to carry on we will make a dummy batch and force the date to fit
                                        giftBatch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref FMainDS,
                                            ref Transaction,
                                            ref LedgerTable,
                                            FLedgerNumber,
                                            DateTime.Today);
                                    }

                                    // Parse the line into a new row
                                    AGiftRow gift = FMainDS.AGift.NewRowTyped(true);
                                    AGiftDetailRow giftDetails;
                                    ParseTransactionLine(gift,
                                        giftBatch,
                                        ref previousGift,
                                        numberOfElements,
                                        ref totalBatchAmount,
                                        ref ImportMessage,
                                        RowNumber,
                                        Messages,
                                        ValidationControlsDictGift,
                                        ValidationControlsDictGiftDetail,
                                        CostCentreTable,
                                        AccountTable,
                                        MotivationGroupTable,
                                        MotivationDetailTable,
                                        MethodOfGivingTable,
                                        MethodOfPaymentTable,
                                        MailingTable,
                                        ref NeedRecipientLedgerNumber,
                                        out giftDetails);

                                    if (TaxDeductiblePercentageEnabled)
                                    {
                                        // Sets TaxDeductiblePct and uses it to calculate the tax deductibility amounts for a Gift Detail
                                        TGift.SetDefaultTaxDeductibilityData(ref giftDetails, gift.DateEntered, Transaction);
                                    }

                                    if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                    {
                                        if ((FLedgerBaseCurrency != FLedgerIntlCurrency) && (giftDetails.GiftAmountIntl != 0))
                                        {
                                            ImportMessage = Catalog.GetString("Updating international exchange rate data");

                                            // We should add a Daily Exchange Rate row pair
                                            // start with To Ledger currency
                                            decimal fromIntlToBase = GLRoutines.Divide(giftDetails.GiftAmount, giftDetails.GiftAmountIntl);

                                            if (UpdateDailyExchangeRateTable(DailyExchangeToLedgerTable, FLedgerIntlCurrency, FLedgerBaseCurrency,
                                                    fromIntlToBase, giftBatch.GlEffectiveDate))
                                            {
                                                ADailyExchangeRateAccess.SubmitChanges(DailyExchangeToLedgerTable, Transaction);
                                                DailyExchangeToLedgerTable.AcceptChanges();

                                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportInformationForLine,
                                                            RowNumber),
                                                        String.Format(Catalog.GetString(
                                                                "Added exchange rate of {0} to Daily Exchange Rate table for ledger currency / international currency on {1}"),
                                                            fromIntlToBase, StringHelper.DateToLocalizedString(giftBatch.GlEffectiveDate)),
                                                        TResultSeverity.Resv_Info));
                                            }

                                            // Now the inverse for From Ledger currency
                                            decimal inverseRate = GLRoutines.Divide(giftDetails.GiftAmountIntl, giftDetails.GiftAmount);

                                            if (UpdateDailyExchangeRateTable(DailyExchangeToIntlTable, FLedgerBaseCurrency, FLedgerIntlCurrency,
                                                    inverseRate, giftBatch.GlEffectiveDate))
                                            {
                                                ADailyExchangeRateAccess.SubmitChanges(DailyExchangeToIntlTable, Transaction);
                                                DailyExchangeToIntlTable.AcceptChanges();
                                            }
                                        }
                                    }
                                } // If known row analysisType
                                else
                                {
                                    if (giftBatch == null)
                                    {
                                        string msg = Catalog.GetString(
                                            "Expecting a Row Type definition. Valid types are 'B' or 'T'. Maybe you are opening a 'Transactions' file.");
                                        msg +=
                                            Catalog.GetString(
                                                "  You need to be on the 'Transactions' Tab to import transaction-only data into an existing batch.");
                                        msg += Catalog.GetString("  Alternatively you may have selected the wrong Field Delimiter.");
                                        msg += Catalog.GetString("  Choose a delimiter that shows multiple columns in the preview window.");
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                msg,
                                                TResultSeverity.Resv_Critical));
                                        break;
                                    }
                                    else
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                String.Format(Catalog.GetString(
                                                        "'{0}' is not a valid Row Type. Valid types are 'B' or 'T'."),
                                                    RowType), TResultSeverity.Resv_Critical));
                                    }
                                }
                            }  // if the CSV line qualifies

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                CancelledByUser = true;
                                break;
                            }

                            if (Messages.Count > 100)
                            {
                                // This probably means that it is a big file and the user has made the same mistake many times over
                                break;
                            }

                            // Update progress tracker every few percent
                            if ((PercentDone - PreviousPercentDone) > 3)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    String.Format(Catalog.GetString("Importing row {0}"), RowNumber),
                                    (PercentDone > 98) ? 98 : PercentDone);
                                PreviousPercentDone = PercentDone;
                            }

                            // Read the next line
                            FImportLine = sr.ReadLine();

                            if (FImportLine != null)
                            {
                                TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            }
                        }  // while CSV lines

                        if (CancelledByUser)
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    "The import was cancelled by the user.", TResultSeverity.Resv_Info));
                            return;
                        }

                        // Finished reading the file - did we have critical errors?
                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Batch has critical errors"),
                                0);

                            // Record error count
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));

                            if (FImportLine == null)
                            {
                                // We did reach the end of the file
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                                        TResultSeverity.Resv_Info));
                            }
                            else
                            {
                                // We gave up before the end
                                if (Messages.Count > 100)
                                {
                                    Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                            Catalog.GetString(
                                                "Stopped reading the file after generating more than 100 messages.  The file may contian more errors beyond the ones listed here."),
                                            TResultSeverity.Resv_Info));
                                }
                                else
                                {
                                    Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                            Catalog.GetString(
                                                "Stopped reading the file. Please check you are using a Gift Batch Import file and have chosen the correct Field Delimiter."),
                                            TResultSeverity.Resv_Info));
                                }
                            }

                            // we do not want to think about Gift Destination problems if the import has failed for another reason
                            NeedRecipientLedgerNumber.Clear();

                            // Do the 'finally' actions and return false
                            return;
                        }

                        // if the import contains gifts with Motivation Group 'GIFT' and that have a Family recipient with no Gift Destination then the import will fail
                        if (NeedRecipientLedgerNumber.Rows.Count > 0)
                        {
                            return;
                        }

                        // Everything is ok, so we can do our finish actions

                        //Update batch total for the last batch entered.
                        if (giftBatch != null)
                        {
                            giftBatch.BatchTotal = totalBatchAmount;
                        }

                        ImportMessage = Catalog.GetString("Saving all data into the database");

                        //Finally save pending changes (the last number is updated !)
                        ImportMessage = Catalog.GetString("Saving final batch");
                        AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, Transaction);
                        FMainDS.AGiftBatch.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving final gift");
                        AGiftAccess.SubmitChanges(FMainDS.AGift, Transaction);
                        FMainDS.AGift.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving final giftdetails");
                        AGiftDetailAccess.SubmitChanges(FMainDS.AGiftDetail, Transaction);
                        FMainDS.AGiftDetail.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving ledger changes");
                        ALedgerAccess.SubmitChanges(LedgerTable, Transaction);
                        FMainDS.ALedger.AcceptChanges();

                        // Commit the transaction (we know that we got a new one and can control it)
                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        // Parse the exception text for possible references to database foreign keys
                        // Make the message more friendly in that case
                        string friendlyExceptionText = MakeFriendlyFKExceptions(ex);

                        if (RowNumber > 0)
                        {
                            // At least we made a start
                            string msg = ImportMessage;

                            if (friendlyExceptionText.Length > 0)
                            {
                                msg += FNewLine + friendlyExceptionText;
                            }

                            if (ImportMessage.StartsWith(Catalog.GetString("Saving ")))
                            {
                                // Do not display any specific line number because these errors occur outside the parsing loop
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileSavingBatch,
                                            giftBatch.BatchDescription),
                                        msg, TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                        msg, TResultSeverity.Resv_Critical));
                            }
                        }
                        else
                        {
                            // We got an exception before we even started parsing the rows (getting a transaction?)
                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                    friendlyExceptionText, TResultSeverity.Resv_Critical));
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Exception Occurred"),
                            0);

                        SubmissionOK = false;
                    }
                    finally
                    {
                        sr.Close();

                        if (SubmissionOK)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Gift batch import successful"),
                                100);
                        }
                        else
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    Catalog.GetString("None of the data from the import was saved."),
                                    TResultSeverity.Resv_Critical));

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Data could not be saved."),
                                0);
                        }

                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    } // end of 'finally'
                });

            // Set our 'out' parameters
            AMessages = Messages;
            ANeedRecipientLedgerNumber = NeedRecipientLedgerNumber;

            return SubmissionOK;
        }

        /// <summary>
        /// Import Gift Transactions from a file
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="AGiftBatchNumber"></param>
        /// <param name="ANeedRecipientLedgerNumber"></param>
        /// <param name="AMessages"></param>
        /// <returns></returns>
        public bool ImportGiftTransactions(
            Hashtable ARequestParams,
            String AImportString,
            Int32 AGiftBatchNumber,
            out GiftBatchTDSAGiftDetailTable ANeedRecipientLedgerNumber,
            out TVerificationResultCollection AMessages
            )
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing Gift Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            GiftBatchTDSAGiftDetailTable NeedRecipientLedgerNumber = new GiftBatchTDSAGiftDetailTable();
            TVerificationResultCollection Messages = new TVerificationResultCollection();

            // fix for Mono issue with out parameter: https://bugzilla.xamarin.com/show_bug.cgi?id=28196
            AMessages = Messages;

            FMainDS = new GiftBatchTDS();
            StringReader sr = new StringReader(AImportString);

            // Parse the supplied parameters
            FDelimiter = (String)ARequestParams["Delimiter"];
            FLedgerNumber = (Int32)ARequestParams["ALedgerNumber"];
            FDateFormatString = (String)ARequestParams["DateFormatString"];
            String NumberFormat = (String)ARequestParams["NumberFormat"];
            FNewLine = (String)ARequestParams["NewLine"];

            // Set culture from parameters
            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            bool TaxDeductiblePercentageEnabled = Convert.ToBoolean(
                TSystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, "FALSE"));

            // Initialise our working variables
            decimal totalBatchAmount = 0;
            Int32 RowNumber = 0;
            Int32 InitialTextLength = AImportString.Length;
            Int32 TextProcessedLength = 0;
            Int32 PercentDone = 10;
            Int32 PreviousPercentDone = 0;
            Boolean CancelledByUser = false;
            string ImportMessage = Catalog.GetString("Initialising");

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictGift = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictGiftDetail = new TValidationControlsDict();

            // This needs to be initialised because we will be calling the method
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;
            TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriodDelegate = @TAccountingPeriodsWebConnector.GetFirstDayOfAccountingPeriod;

            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    try
                    {
                        // Load supplementary tables that we are going to need for validation
                        ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(FLedgerNumber, Transaction);
                        ACostCentreTable CostCentreTable = ACostCentreAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AAccountTable AccountTable = AAccountAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMotivationGroupTable MotivationGroupTable = AMotivationGroupAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMotivationDetailTable MotivationDetailTable = AMotivationDetailAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        AMethodOfGivingTable MethodOfGivingTable = AMethodOfGivingAccess.LoadAll(Transaction);
                        AMethodOfPaymentTable MethodOfPaymentTable = AMethodOfPaymentAccess.LoadAll(Transaction);
                        PMailingTable MailingTable = PMailingAccess.LoadAll(Transaction);

                        AGiftBatchTable giftBatchTable = AGiftBatchAccess.LoadViaALedger(FLedgerNumber, Transaction);
                        DataView giftBatchDV = new DataView(giftBatchTable, String.Format("{0}={1}",
                                AGiftBatchTable.GetBatchNumberDBName(), AGiftBatchNumber), "", DataViewRowState.CurrentRows);
                        FMainDS.AGiftBatch.ImportRow(giftBatchDV[0].Row);
                        FMainDS.AcceptChanges();
                        AGiftBatchRow giftBatch = (AGiftBatchRow)FMainDS.AGiftBatch.Rows.Find(new object[] { FLedgerNumber, AGiftBatchNumber });

                        if (LedgerTable.Rows.Count == 0)
                        {
                            throw new Exception(String.Format(Catalog.GetString("Ledger {0} doesn't exist."), FLedgerNumber));
                        }

                        ImportMessage = Catalog.GetString("Parsing first line");
                        AGiftRow previousGift = null;

                        // Go round a loop reading the file line by line
                        FImportLine = sr.ReadLine();

                        while (FImportLine != null)
                        {
                            RowNumber++;

                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            PercentDone = 10 + ((TextProcessedLength * 90) / InitialTextLength);

                            // skip empty lines and commented lines
                            if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                            {
                                // number of elements is incremented by 1 as though the line started with 'T'
                                int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count + 1;

                                // It is a Transaction row
                                if (numberOfElements < 13) // Perhaps this CSV file is a summary, and can't be imported?
                                {
                                    Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                            Catalog.GetString("Wrong number of gift columns. Expected at least 13 columns. (This may be a summary?)"),
                                            TResultSeverity.Resv_Critical));
                                    FImportLine = sr.ReadLine();

                                    if (FImportLine != null)
                                    {
                                        TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                    }

                                    continue;
                                }

                                // Parse the line into a new row
                                ImportMessage = Catalog.GetString("Parsing transaction line");
                                AGiftRow gift = FMainDS.AGift.NewRowTyped(true);
                                AGiftDetailRow giftDetails;
                                ParseTransactionLine(gift,
                                    giftBatch,
                                    ref previousGift,
                                    numberOfElements,
                                    ref totalBatchAmount,
                                    ref ImportMessage,
                                    RowNumber,
                                    Messages,
                                    ValidationControlsDictGift,
                                    ValidationControlsDictGiftDetail,
                                    CostCentreTable,
                                    AccountTable,
                                    MotivationGroupTable,
                                    MotivationDetailTable,
                                    MethodOfGivingTable,
                                    MethodOfPaymentTable,
                                    MailingTable,
                                    ref NeedRecipientLedgerNumber,
                                    out giftDetails);

                                if (TaxDeductiblePercentageEnabled)
                                {
                                    // Sets TaxDeductiblePct and uses it to calculate the tax deductibility amounts for a Gift Detail
                                    TGift.SetDefaultTaxDeductibilityData(ref giftDetails, gift.DateEntered, Transaction);
                                }
                            }

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                CancelledByUser = true;
                                break;
                            }

                            if (Messages.Count > 100)
                            {
                                // This probably means that it is a big file and the user has made the same mistake many times over
                                break;
                            }

                            // Update progress tracker every few percent
                            if ((PercentDone - PreviousPercentDone) > 3)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    String.Format(Catalog.GetString("Importing row {0}"), RowNumber),
                                    (PercentDone > 98) ? 98 : PercentDone);
                                PreviousPercentDone = PercentDone;
                            }

                            // Read the next line
                            FImportLine = sr.ReadLine();

                            if (FImportLine != null)
                            {
                                TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            }
                        } // while CSV lines

                        if (CancelledByUser)
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    "The import was cancelled by the user.", TResultSeverity.Resv_Info));
                            return;
                        }

                        // Finished reading the file - did we have critical errors?
                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Batch has critical errors"),
                                100);

                            // Record error count
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));

                            if (FImportLine == null)
                            {
                                // We did reach the end of the file
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                                        TResultSeverity.Resv_Info));
                            }
                            else
                            {
                                // We gave up before the end
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Stopped reading the file after generating more than 100 messages.  The file may contian more errors beyond the ones listed here."),
                                        TResultSeverity.Resv_Info));
                            }

                            // we do not want to think about Gift Destination problems if the import has failed for another reason
                            NeedRecipientLedgerNumber.Clear();

                            // Do the 'finally' actions and return false
                            return;
                        }

                        // if the import contains gifts with Motivation Group 'GIFT' and that have a Family recipient with no Gift Destination then the import will fail
                        if (NeedRecipientLedgerNumber.Rows.Count > 0)
                        {
                            // Do the 'finally' actions and return false
                            return;
                        }

                        // Everything is ok, so we can do our finish actions

                        //Update batch total for the last batch entered.
                        if (giftBatch != null)
                        {
                            giftBatch.BatchTotal = totalBatchAmount;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Saving all data into the database"),
                            100);

                        //Finally save pending changes (the last number is updated !)
                        ImportMessage = Catalog.GetString("Saving gift batch");
                        AGiftBatchAccess.SubmitChanges(FMainDS.AGiftBatch, Transaction);
                        FMainDS.AGiftBatch.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving gifts");
                        AGiftAccess.SubmitChanges(FMainDS.AGift, Transaction);
                        FMainDS.AGift.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving giftdetails");
                        AGiftDetailAccess.SubmitChanges(FMainDS.AGiftDetail, Transaction);
                        FMainDS.AGiftDetail.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving ledger");
                        ALedgerAccess.SubmitChanges(LedgerTable, Transaction);
                        LedgerTable.AcceptChanges();

                        // Commit the transaction (we know that we got a new one and can control it)
                        SubmissionOK = true;
                    }
                    catch (Exception ex)
                    {
                        // Parse the exception text for possible references to database foreign keys
                        // Make the message more friendly in that case
                        string friendlyExceptionText = MakeFriendlyFKExceptions(ex);

                        if (RowNumber > 0)
                        {
                            // At least we made a start
                            string msg = ImportMessage;

                            if (friendlyExceptionText.Length > 0)
                            {
                                msg += FNewLine + friendlyExceptionText;
                            }

                            if (ImportMessage.StartsWith(Catalog.GetString("Saving ")))
                            {
                                // Do not display any specific line number because these errors occur outside the parsing loop
                                Messages.Add(new TVerificationResult(MCommonConstants.StrExceptionWhileSavingTransactions,
                                        msg, TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                        msg, TResultSeverity.Resv_Critical));
                            }
                        }
                        else
                        {
                            // We got an exception before we even started parsing the rows (getting a transaction?)
                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                    friendlyExceptionText, TResultSeverity.Resv_Critical));
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Exception Occurred"),
                            0);

                        SubmissionOK = false;
                    }
                    finally
                    {
                        sr.Close();

                        if (SubmissionOK)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Gift batch import successful"),
                                100);
                        }
                        else
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    Catalog.GetString("None of the data from the import was saved."),
                                    TResultSeverity.Resv_Critical));

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Data could not be saved."),
                                0);
                        }

                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    } // end of 'finally'
                });

            // Set our 'out' parameters
            ANeedRecipientLedgerNumber = NeedRecipientLedgerNumber;
            AMessages = Messages;

            return SubmissionOK;
        }

        private void ParseBatchLine(ref AGiftBatchRow AGiftBatch,
            ref TDBTransaction ATransaction,
            ref ALedgerTable ALedgerTable,
            ref string AImportMessage,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationControlsDictBatch,
            AAccountTable AValidationAccountTable,
            AAccountPropertyTable AValidationAccountPropertyTable,
            AAccountingPeriodTable AValidationAccountingPeriodTable,
            ACostCentreTable AValidationCostCentreTable,
            ACorporateExchangeRateTable AValidationCorporateExchTable,
            ACurrencyTable AValidationCurrencyTable)
        {
            // Start parsing
            int preParseMessageCount = AMessages.Count;

            // There are 8 elements to import (the last of which can be blank)
            string BatchDescription = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Batch description"),
                FMainDS.AGiftBatch.ColumnBatchDescription, AValidationControlsDictBatch);
            string BankAccountCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Bank account code"),
                FMainDS.AGiftBatch.ColumnBankAccountCode, AValidationControlsDictBatch).ToUpper();
            decimal HashTotal = TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Hash total"),
                FMainDS.AGiftBatch.ColumnHashTotal, ARowNumber, AMessages, AValidationControlsDictBatch);
            DateTime GlEffectiveDate = TCommonImport.ImportDate(ref FImportLine, FDelimiter, FCultureInfoDate, Catalog.GetString("Effective Date"),
                FMainDS.AGiftBatch.ColumnGlEffectiveDate, ARowNumber, AMessages, AValidationControlsDictBatch);

            AImportMessage = "Creating new batch";

            // This call sets: BatchNumber, BatchYear, BatchPeriod, GlEffectiveDate, ExchangeRateToBase, BatchDescription, BankAccountCode
            //  BankCostCentre and CurrencyCode.  The effective date will NOT be modified.
            //  The first three are not validated because they should be ok by default
            AGiftBatch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref FMainDS,
                ref ATransaction,
                ref ALedgerTable,
                FLedgerNumber,
                GlEffectiveDate,
                false);

            // Now we modify some of these in the light of the imported data
            AGiftBatch.BatchDescription = BatchDescription;
            AGiftBatch.HashTotal = HashTotal;
            AGiftBatch.CurrencyCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Currency code"),
                FMainDS.AGiftBatch.ColumnCurrencyCode, AValidationControlsDictBatch).ToUpper();
            AGiftBatch.ExchangeRateToBase =
                TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Exchange rate to base"),
                    FMainDS.AGiftBatch.ColumnExchangeRateToBase, ARowNumber, AMessages, AValidationControlsDictBatch);

            string BankCostCentre = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Bank cost centre"),
                FMainDS.AGiftBatch.ColumnBankCostCentre, AValidationControlsDictBatch).ToUpper();
            AGiftBatch.GiftType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Gift type"),
                FMainDS.AGiftBatch.ColumnGiftType, AValidationControlsDictBatch);

            // End of parsing
            int postParseMessageCount = AMessages.Count;

            // This may generate a non-critical error
            TCommonImport.FixAccountCodes(FLedgerNumber, ARowNumber, ref BankAccountCode, AValidationAccountTable,
                ref BankCostCentre, AValidationCostCentreTable, AMessages);

            AGiftBatch.BankAccountCode = BankAccountCode;
            AGiftBatch.BankCostCentre = BankCostCentre;

            // If GiftType was empty, will default to GIFT
            // In all cases we ensure that the case entered by the user is converted to the case of our constants
            if ((AGiftBatch.GiftType == String.Empty) || (String.Compare(AGiftBatch.GiftType, MFinanceConstants.GIFT_TYPE_GIFT, true) == 0))
            {
                AGiftBatch.GiftType = MFinanceConstants.GIFT_TYPE_GIFT;
            }
            else if (String.Compare(AGiftBatch.GiftType, MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND, true) == 0)
            {
                AGiftBatch.GiftType = MFinanceConstants.GIFT_TYPE_GIFT_IN_KIND;
            }
            else if (String.Compare(AGiftBatch.GiftType, MFinanceConstants.GIFT_TYPE_OTHER, true) == 0)
            {
                AGiftBatch.GiftType = MFinanceConstants.GIFT_TYPE_OTHER;
            }

            if (preParseMessageCount == postParseMessageCount)
            {
                // No parsing errors so we can validate (parsing errors will have assumed, probably invalid, values)
                int messageCountBeforeValidate = AMessages.Count;

                // Do our standard gift batch validation checks on this row
                AImportMessage = Catalog.GetString("Validating the gift batch data");
                AGiftBatchValidation.Validate(this, AGiftBatch, ref AMessages, AValidationControlsDictBatch);

                // And do the additional manual ones
                AImportMessage = Catalog.GetString("Additional validation of the gift batch data");
                TSharedFinanceValidation_Gift.ValidateGiftBatchManual(this, AGiftBatch, ref AMessages, AValidationControlsDictBatch,
                    AValidationAccountTable, AValidationCostCentreTable, AValidationAccountPropertyTable, AValidationAccountingPeriodTable,
                    AValidationCorporateExchTable, AValidationCurrencyTable,
                    FLedgerBaseCurrency, FLedgerIntlCurrency);

                // Fix up the messages
                for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                {
                    ((TVerificationResult)AMessages[i]).OverrideResultContext(String.Format(MCommonConstants.StrValidationErrorInLine, ARowNumber));

                    if (AMessages[i] is TScreenVerificationResult)
                    {
                        TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)AMessages[i]);
                        AMessages.RemoveAt(i);
                        AMessages.Insert(i, downgrade);
                    }
                }
            }

            if (AGiftBatch.ExchangeRateToBase > 10000000)  // Huge numbers here indicate that the decimal comma/point is incorrect.
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                        String.Format(Catalog.GetString("A huge exchange rate of {0} suggests a decimal point format problem."),
                            AGiftBatch.ExchangeRateToBase),
                        TResultSeverity.Resv_Noncritical));
            }
        }

        private void ParseTransactionLine(AGiftRow AGift, AGiftBatchRow AGiftBatch, ref AGiftRow APreviousGift, int ANumberOfColumns,
            ref decimal ATotalBatchAmount, ref string AImportMessage, int ARowNumber, TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationControlsDictGift, TValidationControlsDict AValidationControlsDictGiftDetail,
            ACostCentreTable AValidationCostCentreTable, AAccountTable AValidationAccountTable, AMotivationGroupTable AValidationMotivationGroupTable,
            AMotivationDetailTable AValidationMotivationDetailTable, AMethodOfGivingTable AValidationMethodOfGivingTable,
            AMethodOfPaymentTable AValidationMethodOfPaymentTable, PMailingTable AValidationMailingTable,
            ref GiftBatchTDSAGiftDetailTable ANeedRecipientLedgerNumber, out AGiftDetailRow AGiftDetails)
        {
            // Start parsing
            int preParseMessageCount = AMessages.Count;

            // Is this the format with extra columns?
            // Actually if it has the extra columns but does not have the optional final 8 columns we cannot distiguish using this test...
            //   A file without extra columns will have between 13 and 21 columns - depending on whether some of the optional ones at the end are included.
            //   A file with extra columns will be between 19 and 27.
            //  So any count between 19 and 21 is ambiguous.  We will assume that if the file has extra columns it also has
            //   at least enough of the optional ones to exceed 21.
            bool HasExtraColumns = (ANumberOfColumns > 21);

            AImportMessage = Catalog.GetString("Importing the gift data");

            AGift.DonorKey = TCommonImport.ImportInt64(ref FImportLine, FDelimiter, Catalog.GetString("Donor key"),
                FMainDS.AGift.ColumnDonorKey, ARowNumber, AMessages, AValidationControlsDictGift);

            TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("short name of donor (unused)"), null, null); // unused

            // This group is optional and database NULL's are allowed
            AGift.MethodOfGivingCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Method of giving Code"),
                FMainDS.AGift.ColumnMethodOfGivingCode, AValidationControlsDictGift, false);
            AGift.MethodOfPaymentCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Method Of Payment Code"),
                FMainDS.AGift.ColumnMethodOfPaymentCode, AValidationControlsDictGift, false);
            AGift.Reference = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Reference"),
                FMainDS.AGift.ColumnReference, AValidationControlsDictGift, false);
            AGift.ReceiptLetterCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Receipt letter code"),
                FMainDS.AGift.ColumnReceiptLetterCode, AValidationControlsDictGift, false);

            if (AGift.MethodOfGivingCode != null)
            {
                AGift.MethodOfGivingCode = AGift.MethodOfGivingCode.ToUpper();
            }

            if (AGift.MethodOfPaymentCode != null)
            {
                AGift.MethodOfPaymentCode = AGift.MethodOfPaymentCode.ToUpper();
            }

            if (AGift.ReceiptLetterCode != null)
            {
                AGift.ReceiptLetterCode = AGift.ReceiptLetterCode.ToUpper();
            }

            if (HasExtraColumns)
            {
                TCommonImport.ImportInt32(ref FImportLine, FDelimiter, Catalog.GetString("Receipt number"),
                    FMainDS.AGift.ColumnReceiptNumber, ARowNumber, AMessages, AValidationControlsDictGift);
                TCommonImport.ImportBoolean(ref FImportLine, FDelimiter, Catalog.GetString("First time gift"),
                    FMainDS.AGift.ColumnFirstTimeGift, ARowNumber, AMessages, AValidationControlsDictGift);
                TCommonImport.ImportBoolean(ref FImportLine, FDelimiter, Catalog.GetString("Receipt printed"),
                    FMainDS.AGift.ColumnReceiptPrinted, ARowNumber, AMessages, AValidationControlsDictGift);
            }

            AImportMessage = Catalog.GetString("Importing the gift details");

            AGiftDetails = FMainDS.AGiftDetail.NewRowTyped(true);

            if ((APreviousGift != null) && (AGift.DonorKey == APreviousGift.DonorKey)
                && (AGift.MethodOfGivingCode == APreviousGift.MethodOfGivingCode)
                && (AGift.MethodOfPaymentCode == APreviousGift.MethodOfPaymentCode)
                && (AGift.Reference == APreviousGift.Reference)
                && (AGift.ReceiptLetterCode == APreviousGift.ReceiptLetterCode)
                && (AGift.ReceiptNumber == APreviousGift.ReceiptNumber)
                && (AGift.FirstTimeGift == APreviousGift.FirstTimeGift)
                && (AGift.ReceiptPrinted == APreviousGift.ReceiptPrinted))
            {
                // this row is a new detail for the previousGift
                AGift = APreviousGift;
                AGift.LastDetailNumber++;
                AGiftDetails.DetailNumber = AGift.LastDetailNumber;
            }
            else
            {
                APreviousGift = AGift;
                AGift.LedgerNumber = AGiftBatch.LedgerNumber;
                AGift.BatchNumber = AGiftBatch.BatchNumber;
                AGift.GiftTransactionNumber = AGiftBatch.LastGiftNumber + 1;
                AGiftBatch.LastGiftNumber++;
                AGift.LastDetailNumber = 1;
                FMainDS.AGift.Rows.Add(AGift);
                AGiftDetails.DetailNumber = 1;
            }

            AGiftDetails.LedgerNumber = AGift.LedgerNumber;
            AGiftDetails.BatchNumber = AGiftBatch.BatchNumber;
            AGiftDetails.GiftTransactionNumber = AGift.GiftTransactionNumber;
            FMainDS.AGiftDetail.Rows.Add(AGiftDetails);

            AGiftDetails.RecipientKey = TCommonImport.ImportInt64(ref FImportLine, FDelimiter, Catalog.GetString("Recipient key"),
                FMainDS.AGiftDetail.ColumnRecipientKey, ARowNumber, AMessages, AValidationControlsDictGiftDetail);

            TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("short name of recipient (unused)"), null, null); // unused

            if (HasExtraColumns)
            {
                TCommonImport.ImportInt32(ref FImportLine, FDelimiter, Catalog.GetString("Recipient ledger number"),
                    FMainDS.AGiftDetail.ColumnRecipientLedgerNumber, ARowNumber, AMessages, AValidationControlsDictGiftDetail);
            }

            // we always calculate RecipientLedgerNumber
            AGiftDetails.RecipientLedgerNumber = TGiftTransactionWebConnector.GetRecipientFundNumber(
                AGiftDetails.RecipientKey, AGiftBatch.GlEffectiveDate);

            decimal currentGiftAmount =
                TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Gift amount"),
                    FMainDS.AGiftDetail.ColumnGiftTransactionAmount, ARowNumber, AMessages, AValidationControlsDictGiftDetail);
            AGiftDetails.GiftTransactionAmount = currentGiftAmount;     // amount in batch currency
            ATotalBatchAmount += currentGiftAmount;

            AGiftDetails.GiftAmount = GLRoutines.Divide(currentGiftAmount, AGiftBatch.ExchangeRateToBase);      // amount in ledger currency

            if (HasExtraColumns)
            {
                // amount in international currency
                TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Gift amount intl"),
                    FMainDS.AGiftDetail.ColumnGiftAmountIntl, ARowNumber, AMessages, AValidationControlsDictGiftDetail);
            }

            AGiftDetails.ConfidentialGiftFlag = TCommonImport.ImportBoolean(ref FImportLine, FDelimiter, Catalog.GetString("Confidential gift"),
                FMainDS.AGiftDetail.ColumnConfidentialGiftFlag, ARowNumber, AMessages, AValidationControlsDictGiftDetail);
            AGiftDetails.MotivationGroupCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Motivation group code"),
                FMainDS.AGiftDetail.ColumnMotivationGroupCode, AValidationControlsDictGiftDetail).ToUpper();
            AGiftDetails.MotivationDetailCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Motivation detail"),
                FMainDS.AGiftDetail.ColumnMotivationDetailCode, AValidationControlsDictGiftDetail).ToUpper();

            if (HasExtraColumns)
            {
                TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Cost centre code"),
                    FMainDS.AGiftDetail.ColumnCostCentreCode, AValidationControlsDictGiftDetail);
            }

            // "In Petra Cost Centre is always inferred from recipient field and motivation detail so is not needed in the import."
            AGiftDetails.CostCentreCode = InferCostCentre(AGiftDetails);

            // All the remaining columns are optional and can contain database NULL
            AGiftDetails.GiftCommentOne = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Gift comment one"),
                FMainDS.AGiftDetail.ColumnGiftCommentOne, AValidationControlsDictGiftDetail, false);
            string commentOneType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Comment one type"),
                FMainDS.AGiftDetail.ColumnCommentOneType, AValidationControlsDictGiftDetail, false);

            AGiftDetails.MailingCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Mailing code"),
                FMainDS.AGiftDetail.ColumnMailingCode, AValidationControlsDictGiftDetail, false);

            AGiftDetails.GiftCommentTwo = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Gift comment two"),
                FMainDS.AGiftDetail.ColumnGiftCommentTwo, AValidationControlsDictGiftDetail, false);
            string commentTwoType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Comment two type"),
                FMainDS.AGiftDetail.ColumnCommentTwoType, AValidationControlsDictGiftDetail, false);
            AGiftDetails.GiftCommentThree = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Gift comment three"),
                FMainDS.AGiftDetail.ColumnGiftCommentThree, AValidationControlsDictGiftDetail, false);
            string commentThreeType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Comment three type"),
                FMainDS.AGiftDetail.ColumnCommentThreeType, AValidationControlsDictGiftDetail, false);

            SetCommentTypeCase(ref commentOneType);
            AGiftDetails.CommentOneType = commentOneType;

            SetCommentTypeCase(ref commentTwoType);
            AGiftDetails.CommentTwoType = commentTwoType;

            SetCommentTypeCase(ref commentThreeType);
            AGiftDetails.CommentThreeType = commentThreeType;

            if (AGiftDetails.MailingCode != null)
            {
                AGiftDetails.MailingCode = AGiftDetails.MailingCode.ToUpper();
            }

            // Find the default Tax deductabilty from the motivation detail. This ensures that the column can be missing.
            AMotivationDetailRow motivationDetailRow = (AMotivationDetailRow)AValidationMotivationDetailTable.Rows.Find(
                new object[] { FLedgerNumber, AGiftDetails.MotivationGroupCode, AGiftDetails.MotivationDetailCode });
            string defaultTaxDeductible =
                ((motivationDetailRow != null) && !motivationDetailRow.IsTaxDeductibleAccountCodeNull()
                 && motivationDetailRow.TaxDeductible) ? "yes" : "no";

            AGiftDetails.TaxDeductible = TCommonImport.ImportBoolean(ref FImportLine, FDelimiter, Catalog.GetString("Tax deductible"),
                FMainDS.AGiftDetail.ColumnTaxDeductible, ARowNumber, AMessages, AValidationControlsDictGiftDetail, defaultTaxDeductible);

            // Account Codes are always inferred from the motivation detail and so is not needed in the import.
            string NewAccountCode = null;
            string NewTaxDeductibleAccountCode = null;

            // get up-to-date account codes
            if (motivationDetailRow != null)
            {
                NewAccountCode = motivationDetailRow.AccountCode;
                NewTaxDeductibleAccountCode = motivationDetailRow.TaxDeductibleAccountCode;
            }

            AGiftDetails.AccountCode = NewAccountCode;
            AGiftDetails.TaxDeductibleAccountCode = NewTaxDeductibleAccountCode;

            // Date entered cannot be imported although it can be modified in the GUI.
            // This is because it would have to be the last column in the import for compatibility
            // but it belongs with the gift and not the detail so it would need to go in an earlier column.
            // For now the import date entered is the effective date.
            AGift.DateEntered = AGiftBatch.GlEffectiveDate;

            // Enforce the correct case for our GIFT constant
            if (String.Compare(AGiftDetails.MotivationGroupCode, MFinanceConstants.MOTIVATION_GROUP_GIFT, true) == 0)
            {
                AGiftDetails.MotivationGroupCode = MFinanceConstants.MOTIVATION_GROUP_GIFT;
            }

            // End of parsing
            if (AMessages.Count == preParseMessageCount)
            {
                // No parsing errors so we can validate (parsing errors will have assumed, probably invalid, values)
                AImportMessage = Catalog.GetString("Validating the gift data");

                int messageCountBeforeValidate = AMessages.Count;

                TPartnerClass RecipientClass;
                string RecipientDescription;
                TPartnerServerLookups.GetPartnerShortName(AGiftDetails.RecipientKey, out RecipientDescription, out RecipientClass);

                // If the gift has a Family recipient with no Gift Destination then the import will fail. Gift is added to a table and returned to client.
                if ((AGiftDetails.RecipientLedgerNumber == 0) && (AGiftDetails.MotivationGroupCode == MFinanceConstants.MOTIVATION_GROUP_GIFT))
                {
                    if (RecipientClass == TPartnerClass.FAMILY)
                    {
                        ((GiftBatchTDSAGiftDetailRow)AGiftDetails).RecipientDescription = RecipientDescription;
                        ANeedRecipientLedgerNumber.Rows.Add((object[])AGiftDetails.ItemArray.Clone());
                    }
                }

                // Do our standard validation on this gift
                AGiftValidation.Validate(this, AGift, ref AMessages, AValidationControlsDictGift);
                TSharedFinanceValidation_Gift.ValidateGiftManual(this, AGift, AGiftBatch.BatchYear, AGiftBatch.BatchPeriod,
                    null, ref AMessages, AValidationControlsDictGift, AValidationMethodOfGivingTable, AValidationMethodOfPaymentTable);

                AImportMessage = Catalog.GetString("Validating the gift details data");

                AGiftDetailValidation.Validate(this, AGiftDetails, ref AMessages, AValidationControlsDictGiftDetail);
                TSharedFinanceValidation_Gift.ValidateGiftDetailManual(this, (GiftBatchTDSAGiftDetailRow)AGiftDetails,
                    ref AMessages, AValidationControlsDictGiftDetail, RecipientClass, AValidationCostCentreTable, AValidationAccountTable,
                    AValidationMotivationGroupTable, AValidationMotivationDetailTable, AValidationMailingTable, AGiftDetails.RecipientKey);

                // Fix up the messages
                for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                {
                    ((TVerificationResult)AMessages[i]).OverrideResultContext(String.Format(MCommonConstants.StrValidationErrorInLine, ARowNumber));

                    if (AMessages[i] is TScreenVerificationResult)
                    {
                        TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)AMessages[i]);
                        AMessages.RemoveAt(i);
                        AMessages.Insert(i, downgrade);
                    }
                }
            }
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

        private String MakeFriendlyFKExceptions(Exception ex)
        {
            //note that this is only done for "user errors" not for program errors!
            String innerMessage;

            if (ex.InnerException == null)
            {
                innerMessage = ex.Message;
            }
            else
            {
                innerMessage = ex.InnerException.ToString();
            }

            string formatStr = Catalog.GetString("  Do you need to add this code to the '{0}' main setup screen?");
            string createNewPartner = Catalog.GetString("  Do you need to add this partner using the 'Create Partner' main setup screen?");

            if (innerMessage.Contains("a_gift_batch_fk2"))
            {
                return Catalog.GetString("Unknown account code.") + String.Format(formatStr, "Manage Accounts");
            }

            if (innerMessage.Contains("a_gift_batch_fk3"))
            {
                return Catalog.GetString("Unknown cost centre.") + String.Format(formatStr, "Manage Cost Centres");
            }

            if (innerMessage.Contains("a_gift_batch_fk4"))
            {
                return Catalog.GetString("Unknown currency code.") + String.Format(formatStr, "Currencies");
            }

            if (innerMessage.Contains("a_gift_fk2"))
            {
                return Catalog.GetString("Unknown method of giving.") + String.Format(formatStr, "Methods Of Giving");
            }

            if (innerMessage.Contains("a_gift_fk3"))
            {
                return Catalog.GetString("Unknown method of payment.") + String.Format(formatStr, "Methods Of Payment");
            }

            if (innerMessage.Contains("a_gift_fk4"))
            {
                return Catalog.GetString("Unknown donor partner key.") + createNewPartner;
            }

            if (innerMessage.Contains("a_gift_detail_fk2"))
            {
                return Catalog.GetString("Unknown motivation detail.") + String.Format(formatStr, "Motivation Details");
            }

            if (innerMessage.Contains("a_gift_detail_fk3"))
            {
                return Catalog.GetString("Unknown recipient partner key.") + createNewPartner;
            }

            if (innerMessage.Contains("a_gift_detail_fk4"))
            {
                return Catalog.GetString("Unknown mailing code.") + String.Format(formatStr, "Mailings");
            }

            if (innerMessage.Contains("a_gift_detail_fk5"))
            {
                return Catalog.GetString("Unknown recipient ledger number.") + createNewPartner;
            }

            if (innerMessage.Contains("a_gift_detail_fk6"))
            {
                return Catalog.GetString("Unknown cost centre.") + String.Format(formatStr, "Manage Cost Centres");
            }

            TLogging.Log("Importing Gift batch: " + ex.ToString());

            return ex.Message;
        }
    }
}