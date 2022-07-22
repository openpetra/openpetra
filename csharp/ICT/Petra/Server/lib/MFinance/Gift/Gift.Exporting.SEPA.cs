//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2022 by OM International
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
using System.IO;
using System.Text;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Server.MFinance.Gift
{
    /// <summary>
    /// provides methods for exporting a batch to a SEPA Direct Debit file
    /// </summary>
    public class TGiftExportingSEPA
    {
        /// <summary>
        /// export a recurring gift batch to a SEPA Direct Debit file
        /// </summary>
        /// <param name="ALedgerNumber">The current Ledger</param>
        /// <param name="ARecurringBatchNumber">The batch number of the recurring gift batch</param>
        /// <param name="ACollectionDate">The date when the direct debit will be collected</param>
        /// <param name="ASEPAFileContent">The content of the SEPA file</param>
        /// <param name="AVerificationMessages">Additional messages to display in a messagebox</param>
        static public Boolean ExportRecurringGiftBatch(
            Int32 ALedgerNumber,
            Int32 ARecurringBatchNumber,
            DateTime ACollectionDate,
            out String ASEPAFileContent,
            out TVerificationResultCollection AVerificationMessages)
        {
            string SEPAFileContent = String.Empty;
            ASEPAFileContent = SEPAFileContent;
            var Messages = new TVerificationResultCollection();
            AVerificationMessages = Messages;
            bool Success = false;
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = new TDBTransaction();

            TDataBase db = DBAccess.Connect("ExportRecurringGiftBatchSEPA");
            TSystemDefaults SystemDefaults = new TSystemDefaults(db);

            try
            {
                db.ReadTransaction(
                    ref Transaction,
                    delegate
                    {
                        // ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        TSEPAWriterDirectDebit writer = new TSEPAWriterDirectDebit();
                        string InitiatorName = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SEPA_CREDITOR_NAME, String.Empty);
                        string CreditorName = InitiatorName;
                        string CreditorIBAN = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SEPA_CREDITOR_IBAN, String.Empty);;
                        string CreditorBIC = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SEPA_CREDITOR_BIC, String.Empty);
                        string CreditorSchemeID = SystemDefaults.GetSystemDefault(SharedConstants.SYSDEFAULT_SEPA_CREDITOR_SCHEMEID, String.Empty);

                        if (InitiatorName == String.Empty)
                        {
                            Messages.Add(new TVerificationResult(
                                    "Exporting Recurring Gift Batch to SEPA failed",
                                    "Missing the setting " + SharedConstants.SYSDEFAULT_SEPA_CREDITOR_NAME,
                                    String.Empty,
                                    string.Empty,
                                    TResultSeverity.Resv_Critical,
                                    Guid.Empty));
                        }

                        if (CreditorIBAN == String.Empty)
                        {
                            Messages.Add(new TVerificationResult(
                                    "Exporting Recurring Gift Batch to SEPA failed",
                                    "Missing the setting " + SharedConstants.SYSDEFAULT_SEPA_CREDITOR_IBAN,
                                    String.Empty,
                                    string.Empty,
                                    TResultSeverity.Resv_Critical,
                                    Guid.Empty));
                        }

                        if (CreditorBIC == String.Empty)
                        {
                            Messages.Add(new TVerificationResult(
                                    "Exporting Recurring Gift Batch to SEPA failed",
                                    "Missing the setting " + SharedConstants.SYSDEFAULT_SEPA_CREDITOR_BIC,
                                    String.Empty,
                                    string.Empty,
                                    TResultSeverity.Resv_Critical,
                                    Guid.Empty));
                        }

                        if (CreditorSchemeID == String.Empty)
                        {
                            Messages.Add(new TVerificationResult(
                                    "Exporting Recurring Gift Batch to SEPA failed",
                                    "Missing the setting " + SharedConstants.SYSDEFAULT_SEPA_CREDITOR_SCHEMEID,
                                    String.Empty,
                                    string.Empty,
                                    TResultSeverity.Resv_Critical,
                                    Guid.Empty));
                        }

                        if (Messages.Count == 0)
                        {
                            writer.Init(InitiatorName, ACollectionDate, CreditorName, CreditorIBAN, CreditorBIC, CreditorSchemeID);

                            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                            ARecurringGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ARecurringBatchNumber, Transaction);
                            ARecurringGiftAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, ARecurringBatchNumber, Transaction);
                            ARecurringGiftDetailAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, ARecurringBatchNumber, Transaction);

                            // TODO GiftBatchTDS GiftDS = new GiftBatchTDS();
                            // TODO TGiftTransactionWebConnector.LoadGiftDonorRelatedData(GiftDS, true, ALedgerNumber, SponsorshipBatchNumber, Transaction);

                            ProcessPmtInf(ref writer, ref MainDS, ACollectionDate);

                            SEPAFileContent = StringHelper.EncodeStringToBase64(writer.Document.OuterXml);

                            Success = true;
                        }
                    });

            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());

                Success = false;

                Messages.Add(new TVerificationResult(
                        "Exporting Recurring Gift Batch to SEPA Terminated Unexpectedly",
                        ex.Message,
                        "An unexpected error occurred during the export of the recurring gift batch",
                        string.Empty,
                        TResultSeverity.Resv_Critical,
                        Guid.Empty));
            }

            AVerificationMessages = Messages;
            ASEPAFileContent = SEPAFileContent;
            return Success;
        }

        private static string CalculateSequenceType(ref GiftBatchTDS MainDS, DateTime APostingDate)
        {
            // filter on DefaultView has already been set
            string ResultSequenceType = String.Empty;

            foreach (DataRowView r in MainDS.ARecurringGiftDetail.DefaultView)
            {
                ARecurringGiftDetailRow DetailRow = (ARecurringGiftDetailRow)r.Row;

                if (DetailRow.StartDonations == DetailRow.EndDonations )
                {
                    if (ResultSequenceType != "" && ResultSequenceType != "OOFF")
                    {
                        throw new Exception("only one sequence type per gift allowed");
                    }

                    ResultSequenceType = "OOFF";
                }
                else
                {
                    if (ResultSequenceType != "" && ResultSequenceType != "RCUR")
                    {
                        throw new Exception("only one sequence type per gift allowed");
                    }

                    ResultSequenceType = "RCUR";
                }

                // TODO? FRST?
            }

            return ResultSequenceType;
        }

        private static string GenerateReference(ref GiftBatchTDS MainDS, DateTime APostingDate)
        {
            string TotalReference = String.Empty;

            foreach (DataRowView r in MainDS.ARecurringGiftDetail.DefaultView)
            {
                ARecurringGiftDetailRow DetailRow = (ARecurringGiftDetailRow)r.Row;

                if (DetailRow.StartDonations > APostingDate || (!DetailRow.IsEndDonationsNull() && DetailRow.EndDonations < APostingDate))
                {
                    continue;
                }

                // possibilities: Project (motivation detail)
                // TODO: sponsored child
                MainDS.AMotivationDetail.DefaultView.RowFilter = 
                    String.Format("a_motivation_group_code_c='{0}' and a_motivation_detail_code_c='{1}'", DetailRow.MotivationGroupCode, DetailRow.MotivationDetailCode);
                var MotivationDetailRow = (AMotivationDetailRow)MainDS.AMotivationDetail.DefaultView[0].Row;
                string DetailReference = MotivationDetailRow.MotivationDetailDescLocal;

                if (DetailReference.Length == 0)
                {
                    DetailReference = MotivationDetailRow.MotivationDetailDesc;
                }

                if (MainDS.ARecurringGiftDetail.DefaultView.Count > 1)
                { 
                    if (DetailReference.Length > 20)
                    {
                        DetailReference = DetailReference.Substring(0,20);
                    }

                    DetailReference += " " + DetailRow.GiftAmount.ToString();
                }

                if (DetailReference.Length > 0)
                {
                    if (TotalReference.Length > 0)
                    {
                        TotalReference += "; ";
                    }

                    TotalReference += DetailReference;
                }
            }

            if (TotalReference.Length > 140)
            {
                TotalReference = TotalReference.Replace(" ", "");
            }

            if (TotalReference.Length > 140)
            {
                TLogging.Log("Warning: SEPA Reference was shortened to 140 characters");
                TLogging.Log(String.Format("It was: {0}", TotalReference));
                TotalReference = TotalReference.Substring(0, 140);
                TLogging.Log(String.Format("It is now: {0}", TotalReference));
            }

            return TotalReference;
        }

        private static void ProcessPmtInf(ref TSEPAWriterDirectDebit AWriter, ref GiftBatchTDS MainDS, DateTime APostingDate)
        {
            // TODO: make list of bank accounts. make sure we only have one transaction for each account: use split gifts instead.

            foreach (ARecurringGiftRow Row in MainDS.ARecurringGift.Rows)
            {
                MainDS.ARecurringGiftDetail.DefaultView.RowFilter = String.Format("a_gift_transaction_number_i='{0}'", Row.GiftTransactionNumber);

                string SequenceType = CalculateSequenceType(ref MainDS, APostingDate);

                Decimal Amount = 0.0m;

                foreach (DataRowView r in MainDS.ARecurringGiftDetail.DefaultView)
                {
                    ARecurringGiftDetailRow DetailRow = (ARecurringGiftDetailRow)r.Row;

                    if (DetailRow.StartDonations > APostingDate || (!DetailRow.IsEndDonationsNull() && DetailRow.EndDonations < APostingDate))
                    {
                        continue;
                    }

                    Amount += DetailRow.GiftAmount;
                }

                string Description = GenerateReference(ref MainDS, APostingDate);

                string DebtorName = "TODO";
                string DebtorIBAN = "TODO";
                string DebtorBIC = "TODO";
                string MandateID = "TODO";
                DateTime MandateSignatureDate = DateTime.Now; // TODO
                string EndToEndId = "TODO";
                AWriter.AddPaymentToSEPADirectDebitFile(SequenceType, DebtorName, DebtorIBAN, DebtorBIC, MandateID, MandateSignatureDate, Amount, Description, EndToEndId);
            }
        }
    }
}