//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Globalization;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.App.Core;


namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TUC_GiftBatches
    {
        /// <summary>
        /// this supports the batch export files from Petra 2.x.
        /// Each line starts with a type specifier, B for batch, J for journal, T for transaction
        /// </summary>
        private void ImportBatches(System.Object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = Catalog.GetString("Import batches from spreadsheet file");
            dialog.Filter = Catalog.GetString("GL Batches files (*.csv)|*.csv");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TDlgSelectCSVSeparator dlgSeparator = new TDlgSelectCSVSeparator(false);
                dlgSeparator.CSVFileName = dialog.FileName;

                if (dlgSeparator.ShowDialog() == DialogResult.OK)
                {
                    CultureInfo culture = new CultureInfo("en-GB");
                    culture.DateTimeFormat.ShortDatePattern = dlgSeparator.DateFormat;

                    StreamReader sr = new StreamReader(dialog.FileName);
                    AGiftBatchRow giftBatch = null;
                    //AGiftRow gift = null;
                    string Message = Catalog.GetString("Parsing first line");
                    Int32 RowNumber = 0;

                    try
                    {
                        while (!sr.EndOfStream)
                        {
                            string Line = sr.ReadLine();
                            RowNumber++;

                            // skip empty lines and commented lines
                            if ((Line.Trim().Length > 0) && !Line.StartsWith("/*") && !Line.StartsWith("#"))
                            {
                                string RowType = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);

                                if (RowType == "B")
                                {
                                    /*
                                     *  WriteStringQuoted("B");
                                     * WriteStringQuoted(giftBatch.BatchDescription);
                                     * WriteStringQuoted(giftBatch.BankAccountCode);
                                     * WriteCurrency(giftBatch.HashTotal);
                                     * WriteDate(giftBatch.GlEffectiveDate);
                                     * WriteStringQuoted(giftBatch.CurrencyCode);
                                     * WriteGeneralNumber(giftBatch.ExchangeRateToBase);
                                     * WriteStringQuoted(giftBatch.BankCostCentre);
                                     * WriteLineStringQuoted(giftBatch.GiftType);
                                     */
                                    GiftBatchTDS NewGiftBatchDS = TRemote.MFinance.Gift.WebConnectors.CreateAGiftBatch(FLedgerNumber);
                                    Int32 NewBatchNumber = NewGiftBatchDS.AGiftBatch[0].BatchNumber;
                                    FMainDS.Merge(NewGiftBatchDS);

                                    DataView FindView = new DataView(FMainDS.AGiftBatch);
                                    FindView.Sort = AGiftBatchTable.GetLedgerNumberDBName() + "," + AGiftBatchTable.GetBatchNumberDBName();
                                    giftBatch = (AGiftBatchRow)FindView[FindView.Find(new object[] { FLedgerNumber, NewBatchNumber })].Row;
                                    //gift = null;

                                    FPetraUtilsObject.SetChangedFlag();

                                    giftBatch.BatchDescription = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    giftBatch.BankAccountCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the hash value of the batch");
                                    giftBatch.HashTotal = Convert.ToDouble(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator));
                                    string NextString = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    Message = Catalog.GetString("Parsing the date effective of the batch: " + NextString);
                                    giftBatch.GlEffectiveDate = Convert.ToDateTime(NextString, culture);
                                    giftBatch.CurrencyCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    giftBatch.ExchangeRateToBase = Convert.ToDouble(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator));
                                    giftBatch.BankCostCentre = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                    giftBatch.GiftType = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                }
                                else if (RowType == "T")
                                {
                                    /*
                                     *   if (GiftRestricted(gift))
                                     * {
                                     * WriteGeneralNumber(0);
                                     * WriteStringQuoted("Confidential");
                                     * ProcessConfidentialMessage();
                                     * }
                                     * else
                                     * {
                                     * WriteGeneralNumber(gift.DonorKey);
                                     * WriteStringQuoted(PartnerShortName(gift.DonorKey));
                                     * }
                                     *
                                     * WriteStringQuoted(gift.MethodOfGivingCode);
                                     * WriteStringQuoted(gift.MethodOfPaymentCode);
                                     * WriteStringQuoted(gift.Reference);
                                     * WriteStringQuoted(gift.ReceiptLetterCode);
                                     *
                                     * if (FExtraColumns)
                                     * {
                                     * WriteGeneralNumber(gift.ReceiptNumber);
                                     * WriteBoolean(gift.FirstTimeGift);
                                     * WriteBoolean(gift.ReceiptPrinted);
                                     * }
                                     *
                                     * WriteGeneralNumber(giftDetails.RecipientKey);
                                     * WriteStringQuoted(PartnerShortName(giftDetails.RecipientKey));
                                     *
                                     * if (FExtraColumns)
                                     * {
                                     * WriteGeneralNumber(giftDetails.RecipientLedgerNumber);
                                     * }
                                     *
                                     * if (FUseBaseCurrency)
                                     * {
                                     * WriteCurrency(giftDetails.GiftAmount);
                                     * }
                                     * else
                                     * {
                                     * WriteCurrency(giftDetails.GiftTransactionAmount);
                                     * }
                                     *
                                     * if (FExtraColumns)
                                     * {
                                     * WriteCurrency(giftDetails.GiftAmountIntl);
                                     * }
                                     *
                                     * WriteBoolean(giftDetails.ConfidentialGiftFlag);
                                     * WriteStringQuoted(giftDetails.MotivationGroupCode);
                                     * WriteStringQuoted(giftDetails.MotivationDetailCode);
                                     * WriteStringQuoted(giftDetails.CostCentreCode);
                                     * WriteStringQuoted(giftDetails.GiftCommentOne);
                                     * WriteStringQuoted(giftDetails.CommentOneType);
                                     *
                                     * if (giftDetails.MailingCode.Equals("?"))
                                     * {
                                     * WriteStringQuoted("");
                                     * }
                                     * else
                                     * {
                                     * WriteStringQuoted(giftDetails.MailingCode);
                                     * }
                                     *
                                     * WriteStringQuoted(giftDetails.GiftCommentTwo);
                                     * WriteStringQuoted(giftDetails.CommentTwoType);
                                     * WriteStringQuoted(giftDetails.GiftCommentThree);
                                     * WriteStringQuoted(giftDetails.CommentThreeType);
                                     * WriteLineBoolean(giftDetails.TaxDeductable); */

                                    /*
                                     * if (giftBatch == null)
                                     * {
                                     *   Message = Catalog.GetString("Expected a GiftBatch line, but found a Gift");
                                     *   throw new Exception();
                                     * }
                                     *
                                     * gift = FMainDS.AGift.NewRowTyped(true);
                                     * ((TFrmGiftBatch)ParentForm).GetJournalsControl().NewRowManual(ref NewJournal);
                                     * NewJournal.BatchNumber = NewBatch.BatchNumber;
                                     * FMainDS.AJournal.Rows.Add(NewJournal);
                                     *
                                     *
                                     * AGiftRow gift = FMainDS.AGift.NewRowTyped(true);
                                     *
                                     *
                                     * ((TFrmGiftBatch)ParentForm).GetTransactionsControl().NewRowManual(ref gift, NewJournal);
                                     * gift.BatchNumber = giftBatch.BatchNumber;
                                     * gift.JournalNumber = NewJournal.JournalNumber;
                                     * FMainDS.ATransaction.Rows.Add(gift);
                                     *
                                     * Message = Catalog.GetString("Parsing the cost centre of the transaction");
                                     * gift.CostCentreCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * // TODO check if cost centre exists, and is a posting costcentre.
                                     * // TODO check if cost centre is active. ask user if he wants to use an inactive cost centre
                                     * Message = Catalog.GetString("Parsing the account code of the transaction");
                                     * gift.AccountCode = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * // TODO check if account exists, and is a posting account.
                                     * // TODO check if account is active. ask user if he wants to use an inactive account
                                     * Message = Catalog.GetString("Parsing the narrative of the transaction");
                                     * gift.Narrative = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * Message = Catalog.GetString("Parsing the reference of the transaction");
                                     * gift.Reference = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * Message = Catalog.GetString("Parsing the transaction date");
                                     * gift.TransactionDate =
                                     *   Convert.ToDateTime(StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator), culture);
                                     *
                                     * Message = Catalog.GetString("Parsing the debit amount of the transaction");
                                     * string DebitAmountString = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * Double DebitAmount = DebitAmountString.Trim().Length == 0 ? 0.0 : Convert.ToDouble(DebitAmountString);
                                     * Message = Catalog.GetString("Parsing the credit amount of the transaction");
                                     * string CreditAmountString = StringHelper.GetNextCSV(ref Line, dlgSeparator.SelectedSeparator);
                                     * Double CreditAmount = DebitAmountString.Trim().Length == 0 ? 0.0 : Convert.ToDouble(CreditAmountString);
                                     *
                                     * if ((DebitAmount == 0) && (CreditAmount == 0))
                                     * {
                                     *   Message = Catalog.GetString("Either the debit amount or the debit amount must be greater than 0.");
                                     * }
                                     *
                                     * if ((DebitAmount != 0) && (CreditAmount != 0))
                                     * {
                                     *   Message = Catalog.GetString("You can not have a value for both debit and credit amount");
                                     * }
                                     *
                                     * if (DebitAmount != 0)
                                     * {
                                     *   gift.DebitCreditIndicator = true;
                                     *   gift.TransactionAmount = DebitAmount;
                                     *   NewJournal.JournalDebitTotal += DebitAmount;
                                     *   giftBatch.BatchDebitTotal += DebitAmount;
                                     *   //NewBatch.BatchControlTotal += DebitAmount;
                                     *   giftBatch.BatchRunningTotal += DebitAmount;
                                     * }
                                     * else
                                     * {
                                     *   gift.DebitCreditIndicator = false;
                                     *   gift.TransactionAmount = CreditAmount;
                                     *   NewJournal.JournalCreditTotal += CreditAmount;
                                     *   giftBatch.BatchCreditTotal += CreditAmount;
                                     * }
                                     *
                                     */
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        sr.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            String.Format(Catalog.GetString("There is a problem parsing the file in row {0}. "), RowNumber) +
                            Environment.NewLine +
                            Message + " " + ex,
                            Catalog.GetString("Error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sr.Close();
                        return;
                    }
                }
            }
        }
    }
}