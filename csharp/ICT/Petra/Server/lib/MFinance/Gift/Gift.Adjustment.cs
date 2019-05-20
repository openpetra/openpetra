//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash, peters
//
// Copyright 2004-2019 by OM International
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
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector provides functions to adjust and reverse gifts
    ///</summary>
    public class TAdjustmentWebConnector
    {
        /// <summary>
        /// Get all data that is needed for a reverse or adjust (not Field Adjust)
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params</param>
        /// <param name="AGiftDS">DataSet containing all gift data needed</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetGiftsForReverseAdjust(
            Hashtable requestParams, ref GiftBatchTDS AGiftDS, out TVerificationResultCollection AMessages)
        {
            GiftAdjustmentFunctionEnum Function = (GiftAdjustmentFunctionEnum)requestParams["Function"];
            Int32 LedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Int32 BatchNumber = (Int32)requestParams["BatchNumber"];

            AMessages = new TVerificationResultCollection();
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetGiftsForReverseAdjust");

            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    // get data needed for new gifts
                    if (Function.Equals(GiftAdjustmentFunctionEnum.ReverseGiftBatch))
                    {
                        AGiftAccess.LoadViaAGiftBatch(MainDS, LedgerNumber, BatchNumber, Transaction);

                        foreach (AGiftRow gift in MainDS.AGift.Rows)
                        {
                            AGiftDetailAccess.LoadViaAGift(MainDS, LedgerNumber, BatchNumber, gift.GiftTransactionNumber, Transaction);
                        }
                    }
                    else
                    {
                        Int32 GiftNumber = (Int32)requestParams["GiftNumber"];
                        Int32 GiftDetailNumber = (Int32)requestParams["GiftDetailNumber"];
                        AGiftAccess.LoadByPrimaryKey(MainDS, LedgerNumber, BatchNumber, GiftNumber, Transaction);

                        if (Function.Equals(GiftAdjustmentFunctionEnum.ReverseGiftDetail))
                        {
                            AGiftDetailAccess.LoadByPrimaryKey(MainDS, LedgerNumber, BatchNumber, GiftNumber, GiftDetailNumber, Transaction);
                        }
                        else
                        {
                            AGiftDetailAccess.LoadViaAGift(MainDS, LedgerNumber, BatchNumber, GiftNumber, Transaction);
                        }
                    }
                });

            AGiftDS = MainDS;

            db.CloseDBConnection();

            return CheckGiftsNotPreviouslyReversed(AGiftDS, out AMessages);
        }

        /// <summary>
        /// Find all gifts that need their field adjusted
        /// </summary>
        /// <param name="AGiftDS">Gift Batch containing all the data needed for a Field Change Adjustment</param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ARecipientKey"></param>
        /// <param name="AStartDate">start of period where we want to fix gifts</param>
        /// <param name="AEndDate">end of period where we want to fix gifts</param>
        /// <param name="AOldField">the wrong field</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetGiftsForFieldChangeAdjustment(ref GiftBatchTDS AGiftDS, Int32 ALedgerNumber,
            Int64 ARecipientKey,
            DateTime AStartDate,
            DateTime AEndDate,
            Int64 AOldField,
            out TVerificationResultCollection AMessages)
        {
            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GetGiftsForFieldChangeAdjustment");
            GiftBatchTDS MainDS = new GiftBatchTDS();

            AMessages = new TVerificationResultCollection();

            db.ReadTransaction(
                ref Transaction,
                delegate
                {
                    string SqlStmt = TDataBase.ReadSqlFile("Gift.GetGiftsToAdjustField.sql");

                    List <OdbcParameter>parameters = new List <OdbcParameter>();
                    OdbcParameter param = new OdbcParameter("LedgerNumber", OdbcType.Int);
                    param.Value = ALedgerNumber;
                    parameters.Add(param);
                    param = new OdbcParameter("StartDate", OdbcType.Date);
                    param.Value = AStartDate;
                    parameters.Add(param);
                    param = new OdbcParameter("EndDate", OdbcType.Date);
                    param.Value = AEndDate;
                    parameters.Add(param);
                    param = new OdbcParameter("RecipientKey", OdbcType.BigInt);
                    param.Value = ARecipientKey;
                    parameters.Add(param);
                    param = new OdbcParameter("OldField", OdbcType.BigInt);
                    param.Value = AOldField;
                    parameters.Add(param);

                    db.Select(MainDS, SqlStmt, MainDS.AGiftDetail.TableName, Transaction, parameters.ToArray());

                    // get additional data
                    foreach (GiftBatchTDSAGiftDetailRow Row in MainDS.AGiftDetail.Rows)
                    {
                        AGiftBatchAccess.LoadByPrimaryKey(MainDS, Row.LedgerNumber, Row.BatchNumber, Transaction);
                        AGiftRow GiftRow =
                            AGiftAccess.LoadByPrimaryKey(MainDS, Row.LedgerNumber, Row.BatchNumber, Row.GiftTransactionNumber, Transaction);

                        Row.DateEntered = GiftRow.DateEntered;
                        Row.DonorKey = GiftRow.DonorKey;
                        Row.IchNumber = 0;
                        Row.DonorName = PPartnerAccess.LoadByPrimaryKey(Row.DonorKey, Transaction)[0].PartnerShortName;
                    }
                });

            AGiftDS = MainDS;

            db.CloseDBConnection();

            return CheckGiftsNotPreviouslyReversed(AGiftDS, out AMessages);
        }

        /// <summary>
        /// Check that none of the gifts have been reversed before.
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static bool CheckGiftsNotPreviouslyReversed(GiftBatchTDS AGiftDS, out TVerificationResultCollection AMessages)
        {
            string Message = string.Empty;
            int GiftCount = 0;

            AMessages = new TVerificationResultCollection();

            // sort gifts
            AGiftDS.AGiftDetail.DefaultView.Sort = string.Format("{0}, {1}, {2}",
                AGiftDetailTable.GetBatchNumberDBName(),
                AGiftDetailTable.GetGiftTransactionNumberDBName(),
                AGiftDetailTable.GetDetailNumberDBName());

            foreach (DataRowView RowView in AGiftDS.AGiftDetail.DefaultView)
            {
                AGiftDetailRow GiftDetailRow = (AGiftDetailRow)RowView.Row;

                if (GiftDetailRow.ModifiedDetail)
                {
                    Message += "\n" + String.Format(Catalog.GetString("Gift {0} with Detail {1} in Batch {2}"),
                        GiftDetailRow.GiftTransactionNumber, GiftDetailRow.DetailNumber, GiftDetailRow.BatchNumber);

                    GiftCount++;
                }
            }

            if (GiftCount != 0)
            {
                if (GiftCount > 1)
                {
                    Message = String.Format(Catalog.GetString("Cannot reverse or adjust the following gifts:")) + "\n" + Message +
                              "\n\n" + Catalog.GetString("They have already been adjusted or reversed.");
                }
                else if (GiftCount == 1)
                {
                    Message = String.Format(Catalog.GetString("Cannot reverse or adjust the following gift:")) + "\n" + Message +
                              "\n\n" + Catalog.GetString("It has already been adjusted or reversed.");
                }

                AMessages.Add(new TVerificationResult(null, Message, TResultSeverity.Resv_Critical));

                return false;
            }

            return true;
        }

        /// <summary>
        /// Identify the gift detail that needs to be reset as not reversed
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AModifiedDetailKeys"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void ReversedGiftReset(Int32 ALedgerNumber, List <string>AModifiedDetailKeys)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (AModifiedDetailKeys.Count == 0)
            {
                //Not an error condition, just return.
                return;
            }

            #endregion Validate Arguments

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("ReversedGiftReset");
            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        foreach (string ModifiedDetailKey in AModifiedDetailKeys)
                        {
                            //Sometimes the underlying ModifiedDetailKeys field is set to empty string
                            if (ModifiedDetailKey.Length == 0)
                            {
                                continue;
                            }

                            string[] GiftDetailFields = ModifiedDetailKey.Split('|');

                            int giftBatchNumber = Convert.ToInt32(GiftDetailFields[1]);
                            int giftNumber = Convert.ToInt32(GiftDetailFields[2]);
                            int giftDetailNumber = Convert.ToInt32(GiftDetailFields[3]);

                            AGiftDetailTable GiftDetailTable =
                                AGiftDetailAccess.LoadByPrimaryKey(ALedgerNumber, giftBatchNumber, giftNumber, giftDetailNumber, Transaction);

                            #region Validate Data

                            if ((GiftDetailTable == null) || (GiftDetailTable.Count == 0))
                            {
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - Data for Gift Detail {1}, from Gift {2} in Batch {3} and Ledger {4}, does not exist or could not be accessed!"),
                                        Utilities.GetMethodName(true),
                                        giftDetailNumber,
                                        giftNumber,
                                        giftBatchNumber,
                                        ALedgerNumber));
                            }

                            #endregion Validate Data

                            GiftDetailTable[0].ModifiedDetail = false;

                            AGiftDetailAccess.SubmitChanges(GiftDetailTable, Transaction);
                        }

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            db.CloseDBConnection();            
        }

        /// <summary>
        /// Revert or Adjust a Gift, revert a Gift Detail, revert a gift batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AGiftDetailNumber"></param>
        /// <param name="ABatchSelected"></param>
        /// <param name="ANewBatchNumber"></param>
        /// <param name="ANewGLDateEffective"></param>
        /// <param name="AFunction"></param>
        /// <param name="ANoReceipt"></param>
        /// <param name="ANewPct"></param>
        /// <param name="AAdjustmentBatchNumber">Batch that adjustment transactions have been added to</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GiftRevertAdjust(
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AGiftDetailNumber,
            bool ABatchSelected,
            Int32 ANewBatchNumber,
            DateTime? ANewGLDateEffective,
            GiftAdjustmentFunctionEnum AFunction,
            bool ANoReceipt,
            Decimal ANewPct,
            out int AAdjustmentBatchNumber)
        {
            AAdjustmentBatchNumber = 0;
            int AdjustmentBatchNo = AAdjustmentBatchNumber;

            GiftBatchTDS GiftDS = new GiftBatchTDS();

            decimal batchGiftTotal = 0;
            ANewBatchNumber = ABatchSelected ? ANewBatchNumber : 0;

            TDBTransaction Transaction = new TDBTransaction();
            TDataBase db = DBAccess.Connect("GiftRevertAdjust");
            bool SubmissionOK = false;

            try
            {
                db.WriteTransaction(
                    ref Transaction,
                    ref SubmissionOK,
                    delegate
                    {
                        // load the original gifts and gift details
                        AGiftAccess.LoadViaAGiftBatch(GiftDS, ALedgerNumber, ABatchNumber, Transaction);
                        AGiftDetailAccess.LoadViaAGiftBatch(GiftDS, ALedgerNumber, ABatchNumber, Transaction);

                        ALedgerTable ledgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

                        AGiftBatchRow giftBatch;

                        DateTime DateEffective;

                        if (ANewGLDateEffective.HasValue)
                        {
                            DateEffective = ANewGLDateEffective.Value;
                        }
                        else
                        {
                            AGiftBatchTable OriginalGiftBatch = AGiftBatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, Transaction);
                            DateEffective = OriginalGiftBatch[0].GlEffectiveDate;
                        }

                        // if we need to create a new gift batch
                        if (!ABatchSelected)
                        {
                            giftBatch = CreateNewGiftBatch(
                                ALedgerNumber,
                                ABatchNumber,
                                DateEffective,
                                AFunction,
                                ref GiftDS, ref ledgerTable, Transaction);
                        }
                        else // using an existing gift batch
                        {
                            AGiftBatchAccess.LoadByPrimaryKey(GiftDS, ALedgerNumber, ANewBatchNumber, Transaction);

                            giftBatch = GiftDS.AGiftBatch[0];
                            DateEffective = giftBatch.GlEffectiveDate;
                            //If into an existing batch, then retrieve the existing batch total
                            batchGiftTotal = giftBatch.BatchTotal;
                        }

                        AdjustmentBatchNo = giftBatch.BatchNumber;

                        //assuming new elements are added after these static borders

                        GiftDS.AGift.DefaultView.Sort = string.Format("{0}, {1}",
                            AGiftTable.GetBatchNumberDBName(),
                            AGiftTable.GetGiftTransactionNumberDBName());

                        GiftDS.AGiftDetail.DefaultView.Sort = string.Format("{0}, {1}, {2}",
                            AGiftDetailTable.GetBatchNumberDBName(),
                            AGiftDetailTable.GetGiftTransactionNumberDBName(),
                            AGiftDetailTable.GetDetailNumberDBName());

                        foreach (DataRowView giftRow in GiftDS.AGift.DefaultView)
                        {
                            int cycle = 0;

                            // first cycle creates gift reversal; second cycle creates new adjusted gift (if needed)
                            do
                            {
                                AGiftRow oldGift = (AGiftRow)giftRow.Row;

                                if (oldGift.RowState != DataRowState.Added)
                                {
                                    AGiftRow gift = GiftDS.AGift.NewRowTyped(true);
                                    DataUtilities.CopyAllColumnValuesWithoutPK(oldGift, gift);
                                    gift.LedgerNumber = giftBatch.LedgerNumber;
                                    gift.BatchNumber = giftBatch.BatchNumber;
                                    // keep the same DateEntered as in the original gift if it is in the same period as the batch
                                    if ((gift.DateEntered.Year != DateEffective.Year) || (gift.DateEntered.Month != DateEffective.Month))
                                    {
                                        gift.DateEntered = DateEffective;
                                    }
                                    gift.GiftTransactionNumber = giftBatch.LastGiftNumber + 1;
                                    giftBatch.LastGiftNumber++;
                                    gift.LinkToPreviousGift = (cycle != 0);
                                    gift.LastDetailNumber = 0;
                                    gift.FirstTimeGift = false;

                                    // do not print a receipt for reversed gifts
                                    if (cycle == 0)
                                    {
                                        gift.ReceiptPrinted = true;
                                        gift.PrintReceipt = false;
                                    }
                                    else
                                    {
                                        gift.ReceiptPrinted = false;
                                        gift.PrintReceipt = !ANoReceipt;
                                    }

                                    GiftDS.AGift.Rows.Add(gift);

                                    foreach (DataRowView giftDetailRow in GiftDS.AGiftDetail.DefaultView)
                                    {
                                        AGiftDetailRow oldGiftDetail = (AGiftDetailRow)giftDetailRow.Row;

                                        // if gift detail belongs to gift
                                        if ((oldGiftDetail.GiftTransactionNumber == oldGift.GiftTransactionNumber)
                                            && (oldGiftDetail.BatchNumber == oldGift.BatchNumber)
                                            && (AFunction != GiftAdjustmentFunctionEnum.ReverseGiftDetail)
                                            || (oldGiftDetail.DetailNumber == AGiftDetailNumber))
                                        {
                                            AddDuplicateGiftDetailToGift(ref GiftDS, ref gift, oldGiftDetail, cycle == 0, Transaction,
                                                AFunction,
                                                ANewPct);

                                            batchGiftTotal += ((cycle == 0) ? 0 : oldGiftDetail.GiftTransactionAmount);

                                            // original gift also gets marked as a reversal
                                            oldGiftDetail.ModifiedDetail = true;
                                        }
                                    }
                                }

                                cycle++;
                            } while ((cycle < 2)
                                     && (AFunction.Equals(GiftAdjustmentFunctionEnum.AdjustGift)
                                         || AFunction.Equals(GiftAdjustmentFunctionEnum.FieldAdjust)
                                         || AFunction.Equals(GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust)));
                        }

                        //When reversing into a new or existing batch, set batch total
                        giftBatch.BatchTotal = batchGiftTotal;

                        // save everything at the end
                        AGiftBatchAccess.SubmitChanges(GiftDS.AGiftBatch, Transaction);
                        ALedgerAccess.SubmitChanges(ledgerTable, Transaction);
                        AGiftAccess.SubmitChanges(GiftDS.AGift, Transaction);
                        AGiftDetailAccess.SubmitChanges(GiftDS.AGiftDetail, Transaction);

                        GiftDS.AGiftBatch.AcceptChanges();

                        SubmissionOK = true;
                    });
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw new EOPAppException(Catalog.GetString("Gift Reverse/Adjust failed."), ex);
            }

            AAdjustmentBatchNumber = AdjustmentBatchNo;

            db.CloseDBConnection();

            return SubmissionOK;
        }

        /// create a new gift batch using some of the details of an existing gift batch
        private static AGiftBatchRow CreateNewGiftBatch(
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            DateTime ADateEffective,
            GiftAdjustmentFunctionEnum AFunction,
            ref GiftBatchTDS AMainDS,
            ref ALedgerTable ALedgerTable,
            TDBTransaction ATransaction)
        {
            AGiftBatchRow ReturnValue;

            AGiftBatchAccess.LoadByPrimaryKey(AMainDS, ALedgerNumber, ABatchNumber, ATransaction);

            AGiftBatchRow oldGiftBatch = AMainDS.AGiftBatch[0];
            TGiftBatchFunctions.CreateANewGiftBatchRow(ref AMainDS, ref ATransaction, ref ALedgerTable, ALedgerNumber, ADateEffective);
            ReturnValue = AMainDS.AGiftBatch[1];
            ReturnValue.BankAccountCode = oldGiftBatch.BankAccountCode;
            ReturnValue.BankCostCentre = oldGiftBatch.BankCostCentre;
            ReturnValue.CurrencyCode = oldGiftBatch.CurrencyCode;
            ReturnValue.ExchangeRateToBase = oldGiftBatch.ExchangeRateToBase;
            ReturnValue.MethodOfPaymentCode = oldGiftBatch.MethodOfPaymentCode;
            ReturnValue.HashTotal = 0;

            if (ReturnValue.MethodOfPaymentCode.Length == 0)
            {
                ReturnValue.SetMethodOfPaymentCodeNull();
            }

            ReturnValue.BankCostCentre = oldGiftBatch.BankCostCentre;
            ReturnValue.GiftType = oldGiftBatch.GiftType;

            if (AFunction.Equals(GiftAdjustmentFunctionEnum.AdjustGift))
            {
                ReturnValue.BatchDescription = Catalog.GetString("Gift Adjustment");
            }
            else if (AFunction.Equals(GiftAdjustmentFunctionEnum.FieldAdjust))
            {
                ReturnValue.BatchDescription = Catalog.GetString("Gift Adjustment (Field Change)");
            }
            else if (AFunction.Equals(GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust))
            {
                ReturnValue.BatchDescription = Catalog.GetString("Gift Adjustment (Tax Deductible Pct Change)");
            }
            else
            {
                ReturnValue.BatchDescription = Catalog.GetString("Reverse Gift");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Adds a duplicate Gift Detail (or reversed duplicate GiftDetail) to Gift.
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="AGift"></param>
        /// <param name="AOldGiftDetail"></param>
        /// <param name="AReversal">True for reverse or false for straight duplicate</param>
        /// <param name="ATransaction"></param>
        /// <param name="AFunction"></param>
        /// <param name="ANewPct"></param>
        /// <param name="AAutoCompleteComments"></param>
        /// <param name="AReversalCommentOne"></param>
        /// <param name="AReversalCommentTwo"></param>
        /// <param name="AReversalCommentThree"></param>
        /// <param name="AReversalCommentOneType"></param>
        /// <param name="AReversalCommentTwoType"></param>
        /// <param name="AReversalCommentThreeType"></param>
        /// <param name="AUpdateTaxDeductiblePctRecipients"></param>
        /// <param name="AGeneralFixedGiftDestination"></param>
        /// <param name="AFixedGiftDestination"></param>
        private static void AddDuplicateGiftDetailToGift(ref GiftBatchTDS AMainDS,
            ref AGiftRow AGift,
            AGiftDetailRow AOldGiftDetail,
            bool AReversal,
            TDBTransaction ATransaction,
            GiftAdjustmentFunctionEnum AFunction,
            Decimal ANewPct,
            bool AAutoCompleteComments = false,
            string AReversalCommentOne = "",
            string AReversalCommentTwo = "",
            string AReversalCommentThree = "",
            string AReversalCommentOneType = "",
            string AReversalCommentTwoType = "",
            string AReversalCommentThreeType = "",
            List <string[]> AUpdateTaxDeductiblePctRecipients = null,
            bool AGeneralFixedGiftDestination = false,
            List <string> AFixedGiftDestination = null
            )
        {
            bool TaxDeductiblePercentageEnabled =
                TSystemDefaults.GetBooleanDefault(SharedConstants.SYSDEFAULT_TAXDEDUCTIBLEPERCENTAGE, false);

            AGiftDetailRow giftDetail = AMainDS.AGiftDetail.NewRowTyped(true);

            DataUtilities.CopyAllColumnValuesWithoutPK(AOldGiftDetail, giftDetail);

            giftDetail.DetailNumber = AGift.LastDetailNumber + 1;
            AGift.LastDetailNumber++;

            giftDetail.LedgerNumber = AGift.LedgerNumber;
            giftDetail.BatchNumber = AGift.BatchNumber;
            giftDetail.GiftTransactionNumber = AGift.GiftTransactionNumber;
            giftDetail.IchNumber = 0;

            decimal signum = (AReversal) ? -1 : 1;
            giftDetail.GiftTransactionAmount = signum * AOldGiftDetail.GiftTransactionAmount;
            giftDetail.GiftAmount = signum * AOldGiftDetail.GiftAmount;
            giftDetail.GiftAmountIntl = signum * AOldGiftDetail.GiftAmountIntl;

            if (TaxDeductiblePercentageEnabled)
            {
                if (!AReversal && AFunction.Equals(GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust))
                {
                    giftDetail.TaxDeductiblePct = ANewPct;
                    TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref giftDetail);
                }
                else if (!AReversal)
                {
                    if (AUpdateTaxDeductiblePctRecipients != null)
                    {
                        string[] Result = AUpdateTaxDeductiblePctRecipients.Find(x => x[0] == giftDetail.RecipientKey.ToString());

                        // true if a new percentage is available and the user wants to use it
                        if (Result != null)
                        {
                            giftDetail.TaxDeductiblePct = Convert.ToDecimal(Result[1]);
                            TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref giftDetail);
                        }
                    }
                }
                else
                {
                    giftDetail.TaxDeductibleAmount = signum * AOldGiftDetail.TaxDeductibleAmount;
                    giftDetail.TaxDeductibleAmountBase = signum * AOldGiftDetail.TaxDeductibleAmountBase;
                    giftDetail.TaxDeductibleAmountIntl = signum * AOldGiftDetail.TaxDeductibleAmountIntl;
                    giftDetail.NonDeductibleAmount = signum * AOldGiftDetail.NonDeductibleAmount;
                    giftDetail.NonDeductibleAmountBase = signum * AOldGiftDetail.NonDeductibleAmountBase;
                    giftDetail.NonDeductibleAmountIntl = signum * AOldGiftDetail.NonDeductibleAmountIntl;
                }
            }

            if (AAutoCompleteComments) // only used for tax deductible pct gift adjustments
            {
                AGiftRow OldGiftRow = (AGiftRow)AMainDS.AGift.Rows.Find(
                    new object[] { AOldGiftDetail.LedgerNumber, AOldGiftDetail.BatchNumber, AOldGiftDetail.GiftTransactionNumber });

                giftDetail.GiftCommentThree = Catalog.GetString("Original gift date: " + OldGiftRow.DateEntered.ToString("dd-MMM-yyyy"));
                giftDetail.CommentThreeType = "Both";
            }
            else // user defined
            {
                giftDetail.GiftCommentOne = AReversalCommentOne;
                giftDetail.GiftCommentTwo = AReversalCommentTwo;
                giftDetail.GiftCommentThree = AReversalCommentThree;
                giftDetail.CommentOneType = AReversalCommentOneType;
                giftDetail.CommentTwoType = AReversalCommentTwoType;
                giftDetail.CommentThreeType = AReversalCommentThreeType;
            }

            // If reversal: mark the new gift as a reversal
            if (AReversal)
            {
                giftDetail.ModifiedDetail = true;

                //Identify the reversal source
                giftDetail.ModifiedDetailKey = "|" + AOldGiftDetail.BatchNumber.ToString() + "|" +
                                               AOldGiftDetail.GiftTransactionNumber.ToString() + "|" +
                                               AOldGiftDetail.DetailNumber.ToString();
            }
            else
            {
                giftDetail.ModifiedDetail = false;

                // Make sure the motivation detail is still active. If not then we need a new one.
                AMotivationDetailTable MotivationDetailTable = AMotivationDetailAccess.LoadViaAMotivationGroup(
                    giftDetail.LedgerNumber, giftDetail.MotivationGroupCode, ATransaction);
                DataRow CurrentMotivationDetail = MotivationDetailTable.Rows.Find(
                    new object[] { giftDetail.LedgerNumber, giftDetail.MotivationGroupCode, giftDetail.MotivationDetailCode });

                // Motivation detail has been made inactive (or doesn't exist) then use default
                if (!((MotivationDetailTable != null) && (MotivationDetailTable.Rows.Count > 0) && (CurrentMotivationDetail != null))
                    || !Convert.ToBoolean(CurrentMotivationDetail[AMotivationDetailTable.GetMotivationStatusDBName()]))
                {
                    bool ActiveRowFound = false;

                    // search for first alternative active detail that is part of the same group
                    foreach (AMotivationDetailRow Row in MotivationDetailTable.Rows)
                    {
                        if ((Row.MotivationDetailCode != giftDetail.MotivationDetailCode) && Row.MotivationStatus)
                        {
                            ActiveRowFound = true;
                            giftDetail.MotivationGroupCode = Row.MotivationGroupCode;
                            giftDetail.MotivationDetailCode = Row.MotivationDetailCode;
                            break;
                        }
                    }

                    // if none found then use default group and detail
                    if (!ActiveRowFound)
                    {
                        giftDetail.MotivationGroupCode = MFinanceConstants.MOTIVATION_GROUP_GIFT;
                        giftDetail.MotivationDetailCode = MFinanceConstants.GROUP_DETAIL_SUPPORT;
                    }
                }

                // if the gift destination should be fixed
                if ((AFunction.Equals(GiftAdjustmentFunctionEnum.TaxDeductiblePctAdjust) && AGeneralFixedGiftDestination)
                        || ((AFixedGiftDestination != null) && (AFixedGiftDestination.Exists(x => x == giftDetail.RecipientKey.ToString()))))
                {
                    giftDetail.FixedGiftDestination = true;
                }
                else
                {
                    giftDetail.FixedGiftDestination = false;
                }
            }

            AMainDS.AGiftDetail.Rows.Add(giftDetail);
        }
    }
}
