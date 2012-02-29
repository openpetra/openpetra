//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, TimI
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
using System.Data;
using System.Data.Odbc;
using System.Collections.Specialized;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MPartner.Partner.ServerLookups;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance.AP.WebConnectors;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MCommon.WebConnectors;

namespace Ict.Petra.Server.MFinance.AP.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Accounts Payable screens
    ///</summary>
    public class TTransactionWebConnector
    {
        private static void LoadAnalysisAttributes(AccountsPayableTDS AMainDS, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            {   // Load via template...
                AAnalysisAttributeRow TemplateRow = AMainDS.AAnalysisAttribute.NewRowTyped(false);
                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.Active = true;
                AAnalysisAttributeAccess.LoadUsingTemplate(AMainDS, TemplateRow, ATransaction);
            }

            AFreeformAnalysisAccess.LoadViaALedger(AMainDS, ALedgerNumber, ATransaction);
        }

        /// <summary>
        /// Loads ApDocument row, and also Supplier, DocumentDetail, and AnalAttrib.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        /// <returns>TDS with tables loaded</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS LoadAApSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            // create the DataSet that will be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AApSupplierAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
            PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            DBAccess.GDBAccessObj.RollbackTransaction();

            // Remove any Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// Loads ApDocument row, and also Supplier, DocumentDetail, and AnalAttrib.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApDocumentId"></param>
        /// <returns>TDS with tables loaded</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS LoadAApDocument(Int32 ALedgerNumber, Int32 AApDocumentId)
        {
            // create the DataSet that will be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AApDocumentAccess.LoadByPrimaryKey(MainDS, AApDocumentId, Transaction);

            // If the load didn't work, don't bother with anything else..
            if (MainDS.AApDocument.Count > 0)
            {
                SetOutstandingAmount(MainDS.AApDocument[0], ALedgerNumber, MainDS.AApDocumentPayment);

                AApDocumentDetailAccess.LoadViaAApDocument(MainDS, AApDocumentId, Transaction);
                AApSupplierAccess.LoadByPrimaryKey(MainDS, MainDS.AApDocument[0].PartnerKey, Transaction);

                AApAnalAttribAccess.LoadViaAApDocument(MainDS, AApDocumentId, Transaction);

                // Accept row changes here so that the Client gets 'unmodified' rows
                MainDS.AcceptChanges();

                // I also need a full list of analysis attributes that could apply to this document
                // (although if it's already been posted I don't need to get this...)

                LoadAnalysisAttributes(MainDS, ALedgerNumber, Transaction);
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// create a new AP document (invoice or credit note) and fill with default values from the supplier
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey">the supplier</param>
        /// <param name="ACreditNoteOrInvoice">true: credit note; false: invoice</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS CreateAApDocument(Int32 ALedgerNumber, Int64 APartnerKey, bool ACreditNoteOrInvoice)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentRow NewDocumentRow = MainDS.AApDocument.NewRowTyped();

            NewDocumentRow.ApDocumentId = (Int32)TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_ap_document);
            NewDocumentRow.ApNumber = -1; // This will be assigned later.
            NewDocumentRow.LedgerNumber = ALedgerNumber;
            NewDocumentRow.PartnerKey = APartnerKey;
            NewDocumentRow.CreditNoteFlag = ACreditNoteOrInvoice;
            NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_OPEN;
            NewDocumentRow.LastDetailNumber = -1;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // get the supplier defaults
            AApSupplierTable tempTable;
            tempTable = AApSupplierAccess.LoadByPrimaryKey(APartnerKey, Transaction);

            if (tempTable.Rows.Count == 1)
            {
                MainDS.AApSupplier.Merge(tempTable);

                AApSupplierRow Supplier = MainDS.AApSupplier[0];

                if (!Supplier.IsDefaultCreditTermsNull())
                {
                    NewDocumentRow.CreditTerms = Supplier.DefaultCreditTerms;
                }

                if (!Supplier.IsDefaultDiscountDaysNull())
                {
                    NewDocumentRow.DiscountDays = Supplier.DefaultDiscountDays;
                }

                if (!Supplier.IsDefaultDiscountPercentageNull())
                {
                    NewDocumentRow.DiscountPercentage = Supplier.DefaultDiscountPercentage;
                }

                if (!Supplier.IsDefaultApAccountNull())
                {
                    NewDocumentRow.ApAccount = Supplier.DefaultApAccount;
                }
            }

            MainDS.AApDocument.Rows.Add(NewDocumentRow);

            // I also need a full list of analysis attributes that could apply to this document

            LoadAnalysisAttributes(MainDS, ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// Get the next available ApNumber for a new document.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATransaction"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        private static Int32 NextApDocumentNumber(Int32 ALedgerNumber, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)

        {
            StringCollection fieldlist = new StringCollection();
            ALedgerTable myLedgerTable;
            fieldlist.Add(ALedgerTable.GetLastApInvNumberDBName());
            myLedgerTable = ALedgerAccess.LoadByPrimaryKey(
                ALedgerNumber,
                fieldlist,
                ATransaction);
            myLedgerTable[0].LastApInvNumber++;
            Int32 NewApNum = myLedgerTable[0].LastApInvNumber;

            ALedgerAccess.SubmitChanges(myLedgerTable, ATransaction, out AVerificationResult);
            return NewApNum;

        }

        /// <summary>
        /// Store the AP Documents (and matching document details and analattribs).
        ///
        /// All DataTables contained in the Typed DataSet are inspected for added,
        /// changed or deleted rows by submitting them to the DataStore.
        ///
        /// </summary>
        /// <param name="AInspectDS">Typed DataSet that needs to contain known DataTables</param>
        /// <param name="AVerificationResult">Empty if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions)</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded,
        /// false if any verification or DB call failed
        /// </returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveAApDocument(ref AccountsPayableTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            AVerificationResult = null;

            if ((AInspectDS != null) && (AInspectDS.AApDocument != null) && (AInspectDS.AApDocument.Rows.Count > 0))
            {
                AVerificationResult = new TVerificationResultCollection();

                // I want to check that the Invoice numbers are not blank,
                // and that none of the documents already exist in the database.

                foreach (AApDocumentRow NewDocRow in AInspectDS.AApDocument.Rows)
                {
                    if (NewDocRow.DocumentCode.Length == 0)
                    {
                        AVerificationResult.Add(new TVerificationResult("Save AP Document", "The Document has empty Document Reference.",
                                TResultSeverity.Resv_Noncritical));
                        return TSubmitChangesResult.scrInfoNeeded;
                    }

                    if (NewDocRow.RowState == DataRowState.Added) // Load via Template
                    {
                        AApDocumentRow DocTemplateRow = AInspectDS.AApDocument.NewRowTyped(false);
                        DocTemplateRow.LedgerNumber = NewDocRow.LedgerNumber;
                        DocTemplateRow.PartnerKey = NewDocRow.PartnerKey;
                        DocTemplateRow.DocumentCode = NewDocRow.DocumentCode;
                        AApDocumentTable MatchingRecords = AApDocumentAccess.LoadUsingTemplate(DocTemplateRow, null);

                        if (MatchingRecords.Rows.Count > 0)
                        {
                            AVerificationResult.Add(new TVerificationResult("Save AP Document", "A Document with this Reference already exists.",
                                    TResultSeverity.Resv_Noncritical));
                            return TSubmitChangesResult.scrInfoNeeded;
                        }
                    }
                }

                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    foreach (AccountsPayableTDSAApDocumentRow NewDocRow in AInspectDS.AApDocument.Rows)
                    {
                        // Set AP Number if it has not been set yet.
                        if (NewDocRow.ApNumber < 0)
                        {
                            NewDocRow.ApNumber = NextApDocumentNumber(NewDocRow.LedgerNumber, SubmitChangesTransaction, out AVerificationResult);
                        }

                        SetOutstandingAmount(NewDocRow, NewDocRow.LedgerNumber, AInspectDS.AApDocumentPayment);
                    }

                    if ((AInspectDS.AApDocument != null) && AApDocumentAccess.SubmitChanges(AInspectDS.AApDocument, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        if ((AInspectDS.AApDocumentDetail == null) // Document detail lines
                            || AApDocumentDetailAccess.SubmitChanges(AInspectDS.AApDocumentDetail, SubmitChangesTransaction,
                                out AVerificationResult))
                        {
                            if ((AInspectDS.AApAnalAttrib == null)  // Analysis attributes
                                || AApAnalAttribAccess.SubmitChanges(AInspectDS.AApAnalAttrib, SubmitChangesTransaction,
                                    out AVerificationResult))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            else
                            {
                                SubmissionResult = TSubmitChangesResult.scrError;
                            }
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                        }
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);
                    TLogging.Log(e.StackTrace);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// create a new AP document detail for an existing AP document;
        /// attention: need to modify the LastDetailNumber on the client side!
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApDocumentId"></param>
        /// <param name="AApSupplier_DefaultExpAccount"></param>
        /// <param name="AApSupplier_DefaultCostCentre"></param>
        /// <param name="AAmount">the amount that is still missing from the total amount of the invoice</param>
        /// <param name="ALastDetailNumber">AApDocument.LastDetailNumber</param>
        /// <returns>the new AApDocumentDetail row</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS CreateAApDocumentDetail(Int32 ALedgerNumber,
            Int32 AApDocumentId,
            string AApSupplier_DefaultExpAccount,
            string AApSupplier_DefaultCostCentre,
            decimal AAmount,
            Int32 ALastDetailNumber)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentDetailRow NewRow = MainDS.AApDocumentDetail.NewRowTyped();

            NewRow.ApDocumentId = AApDocumentId;
            NewRow.LedgerNumber = ALedgerNumber;
            NewRow.DetailNumber = ALastDetailNumber;
            NewRow.Amount = AAmount;
            NewRow.CostCentreCode = AApSupplier_DefaultCostCentre;
            NewRow.AccountCode = AApSupplier_DefaultExpAccount;

            MainDS.AApDocumentDetail.Rows.Add(NewRow);

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        private static void SetOutstandingAmount(AccountsPayableTDSAApDocumentRow Row, Int32 ALedgerNumber, AApDocumentPaymentTable DocPaymentTbl)
        {
            if (Row.DocumentStatus == MFinanceConstants.AP_DOCUMENT_PAID)
            {
                Row.OutstandingAmount = 0;
            }
            else
            {
                Row.OutstandingAmount = Row.TotalAmount;
            }

            if (Row.DocumentStatus == MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID)
            {
                // For any invoices that are partly paid, find out how much is outstanding.
                Row.OutstandingAmount -= UIConnectors.TFindUIConnector.GetPartPaidAmount(Row.ApDocumentId, DocPaymentTbl);
            }
        }

        /// <summary>
        /// Find AP Documents
        /// TODO: date
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS FindAApDocument(Int32 ALedgerNumber, Int64 ASupplierKey,
            string ADocumentStatus,
            bool IsCreditNoteNotInvoice,
            bool AHideAgedTransactions)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApSupplierAccess.LoadByPrimaryKey(MainDS, ASupplierKey, null);

            // TODO: filters for document status
            AccountsPayableTDSAApDocumentRow DocumentTemplate = MainDS.AApDocument.NewRowTyped(false);
            DocumentTemplate.LedgerNumber = ALedgerNumber;
            DocumentTemplate.PartnerKey = ASupplierKey;
            AApDocumentAccess.LoadUsingTemplate(MainDS, DocumentTemplate, null);

            foreach (AccountsPayableTDSAApDocumentRow Row in MainDS.AApDocument.Rows)
            {
                SetOutstandingAmount(Row, ALedgerNumber, MainDS.AApDocumentPayment);
            }

            return MainDS;
        }

        private static bool DocumentBalanceOK(AccountsPayableTDS AMainDS, int AApDocumentId, TDBTransaction ATransaction)
        {
            AApDocumentAccess.LoadByPrimaryKey(AMainDS, AApDocumentId, ATransaction);
            AApDocumentRow DocumentRow = AMainDS.AApDocument[0];
            decimal DocumentBalance = DocumentRow.TotalAmount;

            AMainDS.AApDocumentDetail.DefaultView.RowFilter
                = String.Format("{0}={1}",
                AApDocumentDetailTable.GetApDocumentIdDBName(), AApDocumentId);

            foreach (DataRowView rv in AMainDS.AApDocumentDetail.DefaultView)
            {
                AApDocumentDetailRow Row = (AApDocumentDetailRow)rv.Row;
                DocumentBalance -= Row.Amount;
            }

            return DocumentBalance == 0.0m;
        }

        /// <summary>
        /// Load the Analysis Attributes for this document
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApDocumentId"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if all required attributes are present</returns>
        private static bool AttributesAllOK(AccountsPayableTDS AMainDS, Int32 ALedgerNumber, int AApDocumentId, TDBTransaction ATransaction)
        {
            AMainDS.AApDocumentDetail.DefaultView.RowFilter =
                String.Format("{0}={1}", AApDocumentDetailTable.GetApDocumentIdDBName(), AApDocumentId);

            LoadAnalysisAttributes(AMainDS, ALedgerNumber, ATransaction);

            AApAnalAttribAccess.LoadViaAApDocument(AMainDS, AApDocumentId, ATransaction);

            foreach (DataRowView rv in AMainDS.AApDocumentDetail.DefaultView)
            {
                AApDocumentDetailRow DetailRow = (AApDocumentDetailRow)rv.Row;
                AMainDS.AAnalysisAttribute.DefaultView.RowFilter =
                    String.Format("{0}={1}", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode);     // Do I need Cost Centre in here too?

                if (AMainDS.AAnalysisAttribute.DefaultView.Count > 0)
                {
                    foreach (DataRowView aa_rv in AMainDS.AAnalysisAttribute.DefaultView)
                    {
                        AAnalysisAttributeRow AttrRow = (AAnalysisAttributeRow)aa_rv.Row;

                        AMainDS.AApAnalAttrib.DefaultView.RowFilter =
                            String.Format("{0}={1} AND {2}={3}",
                                AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber,
                                AApAnalAttribTable.GetAccountCodeDBName(), AttrRow.AccountCode);

                        if (AMainDS.AApAnalAttrib.DefaultView.Count == 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Load the AP documents and see if they are ready to be posted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAPDocumentIds"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns> The TDS for posting</returns>
        private static AccountsPayableTDS LoadDocumentsAndCheck(Int32 ALedgerNumber,
            List <Int32>AAPDocumentIds,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AVerifications = new TVerificationResultCollection();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // collect the AP documents from the database
            foreach (Int32 APDocumentId in AAPDocumentIds)
            {
                AApDocumentAccess.LoadByPrimaryKey(MainDS, APDocumentId, Transaction);
                AApDocumentDetailAccess.LoadViaAApDocument(MainDS, APDocumentId, Transaction);
            }

            // do some checks on state of AP documents
            foreach (AApDocumentRow row in MainDS.AApDocument.Rows)
            {
                if (row.DocumentStatus != MFinanceConstants.AP_DOCUMENT_APPROVED)
                {
                    AVerifications.Add(new TVerificationResult(
                            Catalog.GetString("Error during posting of AP document"),
                            String.Format(Catalog.GetString("Document with Number {0} cannot be posted since the status is {1}."),
                                row.ApNumber, row.DocumentStatus), TResultSeverity.Resv_Critical));
                }

                // TODO: also check if details are filled, and they each have a costcentre and account?

                // TODO: check for document.apaccount, if not set, get the default apaccount from the supplier, and save the ap document

                // Check that the amount of the document equals the totals of details
                if (!DocumentBalanceOK(MainDS, row.ApDocumentId, Transaction))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), row.ApNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The value does not match the sum of the details.")),
                            TResultSeverity.Resv_Critical));
                }

                // Load Analysis Attributes and check they're all present.
                if (!AttributesAllOK(MainDS, ALedgerNumber, row.ApDocumentId, Transaction))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), row.ApNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("Analysis Attributes are required.")),
                            TResultSeverity.Resv_Critical));
                }
            }

            // is APostingDate inside the valid posting periods?
            Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;

            if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, APostingDate, out DateEffectivePeriodNumber, out DateEffectiveYearNumber,
                    Transaction))
            {
                AVerifications.Add(new TVerificationResult(
                        String.Format(Catalog.GetString("Cannot post the AP documents in Ledger {0}"), ALedgerNumber),
                        String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                            APostingDate),
                        TResultSeverity.Resv_Critical));
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// creates the GL batch needed for posting the AP Documents
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingDate"></param>
        /// <param name="APDataset"></param>
        /// <returns></returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPosting(Int32 ALedgerNumber, DateTime APostingDate, ref AccountsPayableTDS APDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            batch.BatchDescription = Catalog.GetString("Accounts Payable Batch");
            batch.DateEffective = APostingDate;
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // since the AP documents can be for several suppliers, the currency might be different; group by currency first
            SortedList <string, List <AApDocumentRow>>DocumentsByCurrency = new SortedList <string, List <AApDocumentRow>>();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            foreach (AApDocumentRow row in APDataset.AApDocument.Rows)
            {
                DataView findSupplier = APDataset.AApSupplier.DefaultView;
                findSupplier.RowFilter = AApSupplierTable.GetPartnerKeyDBName() + " = " + row.PartnerKey.ToString();

                if (findSupplier.Count == 0)
                {
                    AApSupplierAccess.LoadByPrimaryKey(APDataset, row.PartnerKey, Transaction);
                }

                string CurrencyCode = ((AApSupplierRow)findSupplier[0].Row).CurrencyCode;

                if (!DocumentsByCurrency.ContainsKey(CurrencyCode))
                {
                    DocumentsByCurrency.Add(CurrencyCode, new List <AApDocumentRow>());
                }

                DocumentsByCurrency[CurrencyCode].Add(row);
            }

            Int32 CounterJournals = 1;

            // Add journal for each currency and the transactions
            foreach (string CurrencyCode in DocumentsByCurrency.Keys)
            {
                AJournalRow journal = GLDataset.AJournal.NewRowTyped();
                journal.LedgerNumber = batch.LedgerNumber;
                journal.BatchNumber = batch.BatchNumber;
                journal.JournalNumber = CounterJournals++;
                journal.DateEffective = batch.DateEffective;
                journal.TransactionCurrency = CurrencyCode;
                journal.JournalDescription = "TODO"; // TODO: journal description for posting AP documents
                journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
                journal.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                journal.DateOfEntry = DateTime.Now;

                // TODO journal.ExchangeRateToBase
                journal.ExchangeRateToBase = 1.0M;

                // TODO journal.ExchangeRateTime
                GLDataset.AJournal.Rows.Add(journal);

                Int32 TransactionCounter = 1;

                foreach (AApDocumentRow document in DocumentsByCurrency[CurrencyCode])
                {
                    ATransactionRow transaction = null;
                    DataView DocumentDetails = APDataset.AApDocumentDetail.DefaultView;
                    DocumentDetails.RowFilter = AApDocumentDetailTable.GetApDocumentIdDBName() + " = " + document.ApDocumentId.ToString();

                    string SupplierShortName;
                    TPartnerClass SupplierPartnerClass;
                    TPartnerServerLookups.GetPartnerShortName(document.PartnerKey, out SupplierShortName, out SupplierPartnerClass);

                    foreach (DataRowView rowview in DocumentDetails)
                    {
                        AApDocumentDetailRow documentDetail = (AApDocumentDetailRow)rowview.Row;


                        transaction = GLDataset.ATransaction.NewRowTyped();
                        transaction.LedgerNumber = journal.LedgerNumber;
                        transaction.BatchNumber = journal.BatchNumber;
                        transaction.JournalNumber = journal.JournalNumber;
                        transaction.TransactionNumber = TransactionCounter++;
                        transaction.TransactionAmount = documentDetail.Amount;

                        // Analysis Attributes - Any attributes linked to this row,
                        // I need to create equivalents in the Transaction DS.

                        APDataset.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3}",
                            AApAnalAttribTable.GetDetailNumberDBName(), documentDetail.DetailNumber,
                            AApAnalAttribTable.GetApDocumentIdDBName(), document.ApDocumentId);

                        foreach (DataRowView rv in APDataset.AApAnalAttrib.DefaultView)
                        {
                            AApAnalAttribRow RowSource = (AApAnalAttribRow)rv.Row;
                            ATransAnalAttribRow RowDest = GLDataset.ATransAnalAttrib.NewRowTyped();

                            RowDest.LedgerNumber = RowSource.LedgerNumber;
                            RowDest.BatchNumber = journal.BatchNumber;
                            RowDest.JournalNumber = journal.JournalNumber;
                            RowDest.TransactionNumber = transaction.TransactionNumber;
                            RowDest.AccountCode = RowSource.AccountCode;
                            RowDest.CostCentreCode = documentDetail.CostCentreCode;
                            RowDest.AnalysisTypeCode = RowSource.AnalysisTypeCode;
                            RowDest.AnalysisAttributeValue = RowSource.AnalysisAttributeValue;

                            GLDataset.ATransAnalAttrib.Rows.Add(RowDest);
                        }

                        if (document.CreditNoteFlag)
                        {
                            transaction.TransactionAmount *= -1;
                        }

                        transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                        if (transaction.TransactionAmount < 0)
                        {
                            transaction.TransactionAmount *= -1;
                        }

                        // TODO: support foreign currencies
                        transaction.AmountInBaseCurrency = transaction.TransactionAmount;

                        transaction.AccountCode = documentDetail.AccountCode;
                        transaction.CostCentreCode = documentDetail.CostCentreCode;
                        transaction.Narrative = "AP:" + document.ApNumber.ToString() + " - " + documentDetail.Narrative + " - " + SupplierShortName;
                        transaction.Reference = documentDetail.ItemRef;

                        // TODO transaction.DetailNumber

                        GLDataset.ATransaction.Rows.Add(transaction);
                    }

                    // create one transaction for the AP account
                    transaction = GLDataset.ATransaction.NewRowTyped();
                    transaction.LedgerNumber = journal.LedgerNumber;
                    transaction.BatchNumber = journal.BatchNumber;
                    transaction.JournalNumber = journal.JournalNumber;
                    transaction.TransactionNumber = TransactionCounter++;
                    transaction.TransactionAmount = document.TotalAmount;

                    if (!document.CreditNoteFlag)
                    {
                        transaction.TransactionAmount *= -1;
                    }

                    transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                    if (transaction.TransactionAmount < 0)
                    {
                        transaction.TransactionAmount *= -1;
                    }

                    // TODO: support foreign currencies
                    transaction.AmountInBaseCurrency = transaction.TransactionAmount;

                    // This seems to be wrong here? (Tim, Feb 2012)
                    // transaction.DebitCreditIndicator = false;

                    // TODO: if document.ApAccount is empty, look for supplier default ap account?
                    transaction.AccountCode = document.ApAccount;
                    transaction.CostCentreCode = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(
                        ALedgerNumber);
                    transaction.Narrative = "AP:" + document.ApNumber.ToString() + " - " + document.DocumentCode + " - " + SupplierShortName;
                    transaction.Reference = "AP" + document.ApNumber.ToString();

                    // TODO transaction.DetailNumber

                    GLDataset.ATransaction.Rows.Add(transaction);
                }

                journal.LastTransactionNumber = TransactionCounter - 1;
            }

            batch.LastJournal = CounterJournals - 1;

            DBAccess.GDBAccessObj.CommitTransaction();

            return GLDataset;
        }

        /// <summary>
        /// Documents can only be deleted if they're not posted yet.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADeleteTheseDocs"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool DeleteAPDocuments(Int32 ALedgerNumber, List <Int32>ADeleteTheseDocs, out TVerificationResultCollection AVerifications)
        {
            AVerifications = new TVerificationResultCollection();
            AccountsPayableTDS TempDS = new AccountsPayableTDS();

            foreach (Int32 ApDocumentId in ADeleteTheseDocs)
            {
                TempDS.Merge(LoadAApDocument(ALedgerNumber, ApDocumentId)); // This gives me documents, details, and potentially ap_anal_attrib records.
            }

            foreach (AApAnalAttribRow AnalAttribRow in TempDS.AApAnalAttrib.Rows)
            {
                AnalAttribRow.Delete();
            }

            foreach (AApDocumentDetailRow DetailRow in TempDS.AApDocumentDetail.Rows)
            {
                DetailRow.Delete();
            }

            foreach (AApDocumentRow DocRow in TempDS.AApDocument.Rows)
            {
                DocRow.Delete();
            }

            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            bool DeleteOK = AApAnalAttribAccess.SubmitChanges(TempDS.AApAnalAttrib, SubmitChangesTransaction, out AVerifications);

            if (DeleteOK)
            {
                DeleteOK = AApDocumentDetailAccess.SubmitChanges(TempDS.AApDocumentDetail, SubmitChangesTransaction, out AVerifications);
            }

            if (DeleteOK)
            {
                DeleteOK = AApDocumentAccess.SubmitChanges(TempDS.AApDocument, SubmitChangesTransaction, out AVerifications);
            }

            if (DeleteOK)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return DeleteOK;
        }

        /// <summary>
        /// creates GL transactions for the selected AP documents,
        /// and posts those GL transactions,
        /// and marks the AP documents as Posted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAPDocumentIds"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPDocuments(Int32 ALedgerNumber,
            List <Int32>AAPDocumentIds,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = LoadDocumentsAndCheck(ALedgerNumber, AAPDocumentIds, APostingDate, out AVerifications);

            if (AVerifications.HasCriticalError())
            {
                return false;
            }

            GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPosting(ALedgerNumber, APostingDate, ref MainDS);

            ABatchRow batch = GLDataset.ABatch[0];

            // save the batch
            if (Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                    out AVerifications) != TSubmitChangesResult.scrOK)
            {
                return false;
            }

            // post the batch
            if (!TGLPosting.PostGLBatch(ALedgerNumber, batch.BatchNumber, out AVerifications))
            {
                // TODO: what if posting fails? do we have an orphaned batch lying around? can this be put into one single transaction? probably not
                // TODO: we should cancel that batch
                return false;
            }

            // change status of AP documents and save to database
            foreach (AApDocumentRow row in MainDS.AApDocument.Rows)
            {
                row.DocumentStatus = MFinanceConstants.AP_DOCUMENT_POSTED;
            }

            TDBTransaction SubmitChangesTransaction;

            try
            {
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                if (AApDocumentAccess.SubmitChanges(MainDS.AApDocument, SubmitChangesTransaction,
                        out AVerifications))
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                // we should not get here; how would the database get broken?
                // TODO do we need a bigger transaction around everything?

                TLogging.Log("after submitchanges: exception " + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return true;
        }

        /// <summary>
        /// creates the GL batch needed for paying the AP Documents
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingDate"></param>
        /// <param name="APDataset"></param>
        /// <returns></returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPaying(Int32 ALedgerNumber, DateTime APostingDate, ref AccountsPayableTDS APDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            batch.BatchDescription = Catalog.GetString("Accounts Payable Payment Batch");
            batch.DateEffective = APostingDate;
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // since the AP documents can be for several suppliers, the currency might be different; group by currency first
            SortedList <string,
                        List <AccountsPayableTDSAApPaymentRow>>DocumentsByCurrency = new SortedList <string, List <AccountsPayableTDSAApPaymentRow>>();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            foreach (AccountsPayableTDSAApPaymentRow row in APDataset.AApPayment.Rows)
            {
                // get the currency from the supplier, from the first documentpayment of this payment; we need the currency
                APDataset.AApDocumentPayment.DefaultView.RowFilter = AApDocumentPaymentTable.GetPaymentNumberDBName() + " = " +
                                                                     row.PaymentNumber.ToString();
                APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApDocumentIdDBName() + " = " +
                                                              ((AApDocumentPaymentRow)APDataset.AApDocumentPayment.DefaultView[0].Row).ApDocumentId.
                                                              ToString();
                row.SupplierKey = ((AApDocumentRow)APDataset.AApDocument.DefaultView[0].Row).PartnerKey;
                AApSupplierAccess.LoadByPrimaryKey(APDataset, row.SupplierKey, Transaction);
                APDataset.AApSupplier.DefaultView.RowFilter = AApSupplierTable.GetPartnerKeyDBName() + " = " + row.SupplierKey.ToString();
                string CurrencyCode = ((AApSupplierRow)APDataset.AApSupplier.DefaultView[0].Row).CurrencyCode;

                TPartnerClass SupplierPartnerClass;
                string supplierName;
                TPartnerServerLookups.GetPartnerShortName(row.SupplierKey, out supplierName, out SupplierPartnerClass);
                row.SupplierName = supplierName;

                if (!DocumentsByCurrency.ContainsKey(CurrencyCode))
                {
                    DocumentsByCurrency.Add(CurrencyCode, new List <AccountsPayableTDSAApPaymentRow>());
                }

                DocumentsByCurrency[CurrencyCode].Add(row);
            }

            Int32 CounterJournals = 1;

            // add journal for each currency and the transactions
            foreach (string CurrencyCode in DocumentsByCurrency.Keys)
            {
                AJournalRow journal = GLDataset.AJournal.NewRowTyped();
                journal.LedgerNumber = batch.LedgerNumber;
                journal.BatchNumber = batch.BatchNumber;
                journal.JournalNumber = CounterJournals++;
                journal.DateEffective = batch.DateEffective;
                journal.TransactionCurrency = CurrencyCode;
                journal.JournalDescription = "TODO"; // TODO: journal description for posting AP documents
                journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
                journal.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                journal.DateOfEntry = DateTime.Now;

                // TODO journal.ExchangeRateToBase
                journal.ExchangeRateToBase = 1.0M;

                // TODO journal.ExchangeRateTime
                GLDataset.AJournal.Rows.Add(journal);

                Int32 TransactionCounter = 1;

                foreach (AccountsPayableTDSAApPaymentRow payment in DocumentsByCurrency[CurrencyCode])
                {
                    ATransactionRow transaction = null;
                    DataView DocumentPaymentView = APDataset.AApDocumentPayment.DefaultView;
                    DocumentPaymentView.RowFilter = AApDocumentPaymentTable.GetPaymentNumberDBName() + " = " + payment.PaymentNumber.ToString();

                    // at the moment, we create 2 transactions for each document; no summarising of documents with same AP account etc
                    foreach (DataRowView rowview in DocumentPaymentView)
                    {
                        AApDocumentPaymentRow documentPayment = (AApDocumentPaymentRow)rowview.Row;
                        APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApDocumentIdDBName() + " = " +
                                                                      documentPayment.ApDocumentId.ToString();
                        AApDocumentRow document = (AApDocumentRow)APDataset.AApDocument.DefaultView[0].Row;

                        // TODO: analysis attributes

                        transaction = GLDataset.ATransaction.NewRowTyped();
                        transaction.LedgerNumber = journal.LedgerNumber;
                        transaction.BatchNumber = journal.BatchNumber;
                        transaction.JournalNumber = journal.JournalNumber;
                        transaction.TransactionNumber = TransactionCounter++;
                        transaction.TransactionAmount = documentPayment.Amount;

                        transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                        if (transaction.TransactionAmount < 0)
                        {
                            transaction.TransactionAmount *= -1;
                        }

                        // TODO: support foreign currencies
                        transaction.AmountInBaseCurrency = transaction.TransactionAmount;

                        transaction.AccountCode = payment.BankAccount;
                        transaction.CostCentreCode = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(
                            payment.LedgerNumber);
                        transaction.Narrative = "AP Payment:" + payment.PaymentNumber.ToString() + " - " +
                                                Ict.Petra.Shared.MPartner.Calculations.FormatShortName(payment.SupplierName,
                            eShortNameFormat.eReverseWithoutTitle);
                        transaction.Reference = payment.Reference;

                        // TODO transaction.DetailNumber

                        GLDataset.ATransaction.Rows.Add(transaction);

                        // at the moment: no summarising of documents with same AP account etc
                        // create one transaction for the AP account
                        ATransactionRow transactionAPAccount = GLDataset.ATransaction.NewRowTyped();
                        transactionAPAccount.LedgerNumber = journal.LedgerNumber;
                        transactionAPAccount.BatchNumber = journal.BatchNumber;
                        transactionAPAccount.JournalNumber = journal.JournalNumber;
                        transactionAPAccount.TransactionNumber = TransactionCounter++;
                        transactionAPAccount.DebitCreditIndicator = !transaction.DebitCreditIndicator;
                        transactionAPAccount.TransactionAmount = transaction.TransactionAmount;
                        transactionAPAccount.AmountInBaseCurrency = transaction.AmountInBaseCurrency;
                        transactionAPAccount.AccountCode = document.ApAccount;
                        transactionAPAccount.CostCentreCode =
                            Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(payment.LedgerNumber);
                        transactionAPAccount.Narrative = "AP Payment:" + payment.PaymentNumber.ToString() + " AP: " +
                                                         document.ApNumber.ToString();
                        transactionAPAccount.Reference = payment.Reference;

                        // TODO transactionAPAccount.DetailNumber

                        GLDataset.ATransaction.Rows.Add(transactionAPAccount);

                        // TODO: for other currencies a post to a_ledger.a_forex_gains_losses_account_c (AP REVAL)
                    }
                }

                journal.LastTransactionNumber = TransactionCounter - 1;
            }

            batch.LastJournal = CounterJournals - 1;

            DBAccess.GDBAccessObj.CommitTransaction();

            return GLDataset;
        }

        private static AApSupplierRow GetSupplier(AApSupplierTable Tbl, Int64 APartnerKey)
        {
            Tbl.DefaultView.Sort = AApSupplierTable.GetPartnerKeyDBName();

            int indexSupplier = Tbl.DefaultView.Find(APartnerKey);

            if (indexSupplier == -1)
            {
                return null;
            }

            return Tbl[indexSupplier];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADataset"></param>
        /// <param name="ADocumentsToPay"></param>
        /// <returns>There's really nothing to return, but generateGlue doesn't cope properly with <void> methods...</void></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool CreatePaymentTableEntries(ref AccountsPayableTDS ADataset, List<Int32> ADocumentsToPay)
        {
            ADataset.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();

            foreach (Int32 ApDocId in ADocumentsToPay)
            {
                int indexDocument = ADataset.AApDocument.DefaultView.Find(ApDocId);

                if (indexDocument != -1)
                {
                    AccountsPayableTDSAApDocumentRow apdocument =
                        (AccountsPayableTDSAApDocumentRow)ADataset.AApDocument.DefaultView[indexDocument].Row;

                    AApSupplierRow supplier = GetSupplier(ADataset.AApSupplier, apdocument.PartnerKey);

                    if (supplier == null)
                    {
                        // I need to load the supplier record into the TDS...
                        ADataset.Merge(LoadAApSupplier(apdocument.LedgerNumber, apdocument.PartnerKey));
                        supplier = GetSupplier(ADataset.AApSupplier, apdocument.PartnerKey);
                    }

                    if (supplier != null)
                    {
                        AccountsPayableTDSAApPaymentRow supplierPaymentsRow = null;

                        // My TDS may already have a AApPayment row for this supplier.
                        ADataset.AApPayment.DefaultView.RowFilter = String.Format("{0}='{1}'", AccountsPayableTDSAApPaymentTable.GetSupplierKeyDBName(
                                ), supplier.PartnerKey);

                        if (ADataset.AApPayment.DefaultView.Count > 0)
                        {
                            supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)ADataset.AApPayment.DefaultView[0].Row;

                            if (apdocument.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay -= apdocument.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay += apdocument.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.
                        }
                        else
                        {
                            supplierPaymentsRow = ADataset.AApPayment.NewRowTyped();
                            supplierPaymentsRow.LedgerNumber = ADataset.AApDocument[0].LedgerNumber;
                            supplierPaymentsRow.PaymentNumber = -1 * (ADataset.AApPayment.Count + 1);
                            supplierPaymentsRow.SupplierKey = supplier.PartnerKey;
                            supplierPaymentsRow.MethodOfPayment = supplier.PaymentType;
                            supplierPaymentsRow.BankAccount = supplier.DefaultBankAccount;
                            supplierPaymentsRow.CurrencyCode = supplier.CurrencyCode;

                            // TODO: use uptodate exchange rate?
                            supplierPaymentsRow.ExchangeRateToBase = 1.0M;

                            // TODO: leave empty
                            supplierPaymentsRow.Reference = "TODO";

                            TPartnerClass partnerClass;
                            string partnerShortName;
                            TPartnerServerLookups.GetPartnerShortName(
                                supplier.PartnerKey,
                                out partnerShortName,
                                out partnerClass);
                            supplierPaymentsRow.SupplierName = Ict.Petra.Shared.MPartner.Calculations.FormatShortName(partnerShortName,
                                eShortNameFormat.eReverseWithoutTitle);

                            supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";

                            if (apdocument.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay = 0 - apdocument.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay = apdocument.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.

                            ADataset.AApPayment.Rows.Add(supplierPaymentsRow);
                        }

                        AccountsPayableTDSAApDocumentPaymentRow DocumentPaymentRow = ADataset.AApDocumentPayment.NewRowTyped();
                        DocumentPaymentRow.LedgerNumber = supplierPaymentsRow.LedgerNumber;
                        DocumentPaymentRow.PaymentNumber = supplierPaymentsRow.PaymentNumber;
                        DocumentPaymentRow.ApDocumentId = ApDocId;
                        DocumentPaymentRow.CurrencyCode = supplier.CurrencyCode;
                        DocumentPaymentRow.Amount = apdocument.TotalAmount;
                        DocumentPaymentRow.InvoiceTotal = apdocument.OutstandingAmount;

                        if (apdocument.CreditNoteFlag)
                        {
                            DocumentPaymentRow.Amount = 0 - DocumentPaymentRow.Amount;
                            DocumentPaymentRow.InvoiceTotal = 0 - DocumentPaymentRow.InvoiceTotal;
                        }

                        DocumentPaymentRow.PayFullInvoice = true;

                        // TODO: discounts
                        DocumentPaymentRow.HasValidDiscount = false;
                        DocumentPaymentRow.DiscountPercentage = 0;
                        DocumentPaymentRow.UseDiscount = false;
                        DocumentPaymentRow.DocumentCode = apdocument.DocumentCode;
                        DocumentPaymentRow.DocType = (apdocument.CreditNoteFlag ? "CREDIT" : "INVOICE");
                        ADataset.AApDocumentPayment.Rows.Add(DocumentPaymentRow);
                    }
                }
            }

            ADataset.AApPayment.DefaultView.RowFilter = "";
            return true;
        }

        /// <summary>
        /// Store payments in the database, and post the payment to GL
        /// </summary>
        /// <param name="APayments"></param>
        /// <param name="ADocumentPayments"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPPayments(
            ref AccountsPayableTDSAApPaymentTable APayments,
            AccountsPayableTDSAApDocumentPaymentTable ADocumentPayments,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = null;
            bool ResultValue = false;

            if ((APayments.Rows.Count < 1) || (ADocumentPayments.Rows.Count < 1))
            {
                AVerifications = new TVerificationResultCollection();
                AVerifications.Add(new TVerificationResult("Post Payment",
                        String.Format("Nothing to do - Payments has {0} rows, Documents has {1} rows.",
                            APayments.Rows.Count, ADocumentPayments.Rows.Count), TResultSeverity.Resv_Noncritical));
                return false;
            }

            AccountsPayableTDS MainDS = new AccountsPayableTDS();
            MainDS.AApPayment.Merge(APayments);
            MainDS.AApDocumentPayment.Merge(ADocumentPayments);

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            foreach (AccountsPayableTDSAApDocumentPaymentRow row in MainDS.AApDocumentPayment.Rows)
            {
                AApDocumentAccess.LoadByPrimaryKey(MainDS, row.ApDocumentId, ReadTransaction);

                // Modify the AP documents and mark as paid or partially paid
                MainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();
                Int32 RowIdx = MainDS.AApDocument.DefaultView.Find(row.ApDocumentId);
                AccountsPayableTDSAApDocumentRow documentRow = (AccountsPayableTDSAApDocumentRow)
                    MainDS.AApDocument.DefaultView[RowIdx].Row;

                SetOutstandingAmount(documentRow, documentRow.LedgerNumber, ADocumentPayments);

                //
                // If the amount paid is negative, this is a refund..
                if (row.Amount < 0)
                {
                    if (row.Amount <= documentRow.OutstandingAmount)
                    {
                        documentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_PAID;
                    }
                    else
                    {
                        documentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID;
                    }
                }
                else
                {
                    if ((row.Amount >= documentRow.OutstandingAmount) || (documentRow.OutstandingAmount == 0.0m))
                    {
                        documentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_PAID;
                    }
                    else
                    {
                        documentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID;
                    }
                }
            }

            // Get max payment number for this ledger
            // PROBLEM: what if two payments are happening at the same time? do we need locking?
            // see also http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=50
            object maxPaymentCanBeNull = DBAccess.GDBAccessObj.ExecuteScalar(
                "SELECT MAX(PUB_a_ap_payment.a_payment_number_i) FROM PUB_a_ap_payment WHERE PUB_a_ap_payment.a_ledger_number_i = " +
                MainDS.AApPayment[0].LedgerNumber.ToString(),
                ReadTransaction);
            Int32 maxPaymentNumberInLedger = (maxPaymentCanBeNull == System.DBNull.Value ? 0 : Convert.ToInt32(maxPaymentCanBeNull));

            DBAccess.GDBAccessObj.CommitTransaction();

            foreach (AccountsPayableTDSAApPaymentRow paymentRow in MainDS.AApPayment.Rows)
            {
                paymentRow.PaymentDate = APostingDate;

                paymentRow.Amount = 0.0M;
                Int32 NewPaymentNumber = maxPaymentNumberInLedger + (-1 * paymentRow.PaymentNumber);

                foreach (AccountsPayableTDSAApDocumentPaymentRow docPaymentRow in MainDS.AApDocumentPayment.Rows)
                {
                    if (docPaymentRow.PaymentNumber == paymentRow.PaymentNumber)
                    {
                        paymentRow.Amount += docPaymentRow.Amount;
                        docPaymentRow.PaymentNumber = NewPaymentNumber;
                    }
                }

                paymentRow.PaymentNumber = NewPaymentNumber;
            }

            TDBTransaction SubmitChangesTransaction = null;

            try
            {
                // create GL batch
                GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPaying(MainDS.AApPayment[0].LedgerNumber,
                    APostingDate,
                    ref MainDS);

                ABatchRow batch = GLDataset.ABatch[0];

                // save the batch
                if (Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                        out AVerifications) == TSubmitChangesResult.scrOK)
                {
                    // post the batch
                    if (!TGLPosting.PostGLBatch(MainDS.AApPayment[0].LedgerNumber, batch.BatchNumber,
                            out AVerifications))
                    {
                        // TODO: what if posting fails? do we have an orphaned batch lying around? can this be put into one single transaction? probably not
                        // TODO: we should cancel that batch

                        // TODO: I need to at least report this to the user.
                    }
                    else
                    {
                        SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                        // store ApPayment and ApDocumentPayment to database
                        if (AApPaymentAccess.SubmitChanges(MainDS.AApPayment, SubmitChangesTransaction,
                                out AVerifications))
                        {
                            if (AApDocumentPaymentAccess.SubmitChanges(MainDS.AApDocumentPayment, SubmitChangesTransaction,
                                    out AVerifications))
                            {
                                // save changed status of AP documents to database
                                if (AApDocumentAccess.SubmitChanges(MainDS.AApDocument, SubmitChangesTransaction, out AVerifications))
                                {
                                    ResultValue = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // we should not get here; how would the database get broken?
                // TODO do we need a bigger transaction around everything?

                TLogging.Log("Posting payments: exception " + e.Message);

                if (SubmitChangesTransaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw new Exception(e.ToString() + " " + e.Message);
            }

            if (ResultValue)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// Load this payment, together with the supplier and all the related documents.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static AccountsPayableTDS LoadAPPayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            AccountsPayableTDS MainDs = new AccountsPayableTDS();

            AApPaymentAccess.LoadByPrimaryKey(MainDs, ALedgerNumber, APaymentNumber, ReadTransaction);
            AccountsPayableTDSAApPaymentRow supplierPaymentsRow = MainDs.AApPayment[0];

            if (MainDs.AApPayment.Rows.Count > 0) // If I can load the referenced payment, I'll also load related documents.
            {
                AApDocumentPaymentAccess.LoadViaAApPayment(MainDs, ALedgerNumber, APaymentNumber, ReadTransaction);

                // There may be a batch of several invoices in this payment,
                // but they should all be to the same supplier.
                foreach (AccountsPayableTDSAApDocumentPaymentRow Row in MainDs.AApDocumentPayment.Rows)
                {
                    AApDocumentAccess.LoadByPrimaryKey(MainDs, Row.ApDocumentId, ReadTransaction);
                    //
                    // After loading - I need to find that row I just loaded!
                    MainDs.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();
                    Int32 Idx = MainDs.AApDocument.DefaultView.Find(Row.ApDocumentId);
                    AApDocumentRow DocumentRow = (AApDocumentRow) MainDs.AApDocument.DefaultView[Idx].Row;

                    Row.InvoiceTotal = DocumentRow.TotalAmount;
                    Row.PayFullInvoice = (MainDs.AApDocumentPayment[0].Amount == DocumentRow.TotalAmount);
                    Row.DocumentCode = DocumentRow.DocumentCode;
                    Row.DocType = (DocumentRow.CreditNoteFlag ? "CREDIT" : "INVOICE");

                    AApDocumentDetailAccess.LoadViaAApDocument(MainDs, Row.ApDocumentId, ReadTransaction);

                    // Then I also need to get any referenced AnalAttrib records
                    MainDs.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                        AApDocumentDetailTable.GetApDocumentIdDBName(), Row.ApDocumentId);

                    foreach (DataRowView rv in MainDs.AApDocumentDetail.DefaultView)
                    {
                        AApDocumentDetailRow DetailRow = (AApDocumentDetailRow)rv.Row;
                        AApAnalAttribAccess.LoadViaAApDocumentDetail(MainDs, Row.ApDocumentId, DetailRow.DetailNumber, ReadTransaction);
                    }
                }

                AApSupplierAccess.LoadByPrimaryKey(MainDs, MainDs.AApDocument[0].PartnerKey, ReadTransaction);

                PPartnerAccess.LoadByPrimaryKey(MainDs, MainDs.AApDocument[0].PartnerKey, ReadTransaction);
                supplierPaymentsRow.SupplierKey = MainDs.AApDocument[0].PartnerKey;
                supplierPaymentsRow.SupplierName = MainDs.PPartner[0].PartnerShortName;
                supplierPaymentsRow.CurrencyCode = MainDs.AApSupplier[0].CurrencyCode;
                supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDs;
        }

        /// <summary>
        /// Create "balancing" documents that undo the effect of all the documents in this payment,
        /// then "pay" those documents, create new ones (duplicates of the originals), and post them.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool ReversePayment(Int32 ALedgerNumber, Int32 APaymentNumber, DateTime APostingDate, out TVerificationResultCollection AVerifications)

        {

            //
            // I need to create new documents and post them.

            // First, a squeaky clean TDS, and also one with the existing payment:
            AccountsPayableTDS ReverseDS = new AccountsPayableTDS();
            AccountsPayableTDS TempDS = LoadAPPayment(ALedgerNumber, APaymentNumber);

            Int32 NewApNum = -1;
            AVerifications = new TVerificationResultCollection();

            // This transaction should enclose the entire operation.
            TDBTransaction ReversalTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            List<Int32> PostTheseDocs = new List<Int32>();


            //
            // Now produce a reversed copy of each referenced document
            //
            TempDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApDocumentId);
                AApDocumentRow OldDocumentRow = TempDS.AApDocument[DocIdx];
                AApDocumentRow NewDocumentRow = ReverseDS.AApDocument.NewRowTyped();

                DataUtilities.CopyAllColumnValues(OldDocumentRow, NewDocumentRow);
                NewDocumentRow.ApDocumentId = (Int32)TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_ap_document);

                PostTheseDocs.Add(NewDocumentRow.ApDocumentId);

                NewDocumentRow.CreditNoteFlag = !OldDocumentRow.CreditNoteFlag; // Here's the actual reversal!
                NewDocumentRow.DocumentCode = "Reversal " + OldDocumentRow.DocumentCode;
                NewDocumentRow.Reference = "Reversal " + OldDocumentRow.Reference;
                NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;

                NewDocumentRow.DateCreated = DateTime.Now;
                NewDocumentRow.DateEntered = DateTime.Now;
                NewDocumentRow.ApNumber = NextApDocumentNumber(ALedgerNumber, ReversalTransaction, out AVerifications);
                ReverseDS.AApDocument.Rows.Add(NewDocumentRow);

                TempDS.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentDetailTable.GetApDocumentIdDBName(), OldDocumentRow.ApDocumentId);

                foreach (DataRowView rv in TempDS.AApDocumentDetail.DefaultView)
                {
                    AApDocumentDetailRow OldDetailRow = (AApDocumentDetailRow)rv.Row;
                    AApDocumentDetailRow NewDetailRow = ReverseDS.AApDocumentDetail.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldDetailRow, NewDetailRow);
                    NewDetailRow.ApDocumentId = NewDocumentRow.ApDocumentId;
                    ReverseDS.AApDocumentDetail.Rows.Add(NewDetailRow);
                }

                //
                // if the original invoice has AnalAttrib records attached, I need to copy those over..
                TempDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApAnalAttribTable.GetApDocumentIdDBName(), OldDocumentRow.ApDocumentId);

                foreach (DataRowView rv in TempDS.AApAnalAttrib.DefaultView)
                {
                    AApAnalAttribRow OldAttribRow = (AApAnalAttribRow)rv.Row;
                    AApAnalAttribRow NewAttribRow = ReverseDS.AApAnalAttrib.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAttribRow, NewAttribRow);
                    NewAttribRow.ApDocumentId = NewDocumentRow.ApDocumentId;
                    ReverseDS.AApAnalAttrib.Rows.Add(NewAttribRow);
                }
            }

            //
            // Save these new documents, with their details and analAttribs.
            if (SaveAApDocument(ref ReverseDS, out AVerifications) != TSubmitChangesResult.scrOK)
            {
                //
                // What to do now? I've got nice new documents, but I can't save them.
                //
                return false;
            }

            //
            // Now I can post these new documents, and pay them:
            //

            if (!PostAPDocuments(
                    ALedgerNumber,
                    PostTheseDocs,
                    APostingDate,
                    out AVerifications))
            {
                // What to do now? I've made new "reversal" documents, but I can't post them...
                return false;
            }

            CreatePaymentTableEntries(ref ReverseDS, PostTheseDocs);
            AccountsPayableTDSAApPaymentTable AApPayment = ReverseDS.AApPayment;
            AccountsPayableTDSAApDocumentPaymentTable AApDocumentPayment = ReverseDS.AApDocumentPayment;

            if (!PostAPPayments(
                    ref AApPayment,
                    AApDocumentPayment,
                    APostingDate,
                    out AVerifications))
            {
                //
                // What to do now? I've created these negative documents, and they're posted,
                // but they can't be paid for some reason.
                //
                return false;
            }

            //
            // Now I need to re-create and Post new documents that match the previous ones that were reversed!
            //

            AccountsPayableTDS CreateDs = new AccountsPayableTDS();
            NewApNum = -1;

            foreach (AApDocumentPaymentRow PaymentRow in TempDS.AApDocumentPayment.Rows)
            {
                Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(PaymentRow.ApDocumentId);
                AApDocumentRow OldDocumentRow = TempDS.AApDocument[DocIdx];
                AApDocumentRow NewDocumentRow = CreateDs.AApDocument.NewRowTyped();

                DataUtilities.CopyAllColumnValues(OldDocumentRow, NewDocumentRow);
                NewDocumentRow.ApDocumentId = (Int32)TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_ap_document);
                NewDocumentRow.DocumentCode = "Duplicate " + OldDocumentRow.DocumentCode;
                NewDocumentRow.Reference = "Duplicate " + OldDocumentRow.Reference;
                NewDocumentRow.DateEntered = APostingDate;
                NewDocumentRow.ApNumber = NewApNum;
                NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
                CreateDs.AApDocument.Rows.Add(NewDocumentRow);

                TempDS.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApDocumentDetailTable.GetApDocumentIdDBName(), OldDocumentRow.ApDocumentId);

                foreach (DataRowView rv in TempDS.AApDocumentDetail.DefaultView)
                {
                    AApDocumentDetailRow OldDetailRow = (AApDocumentDetailRow)rv.Row;
                    AApDocumentDetailRow NewDetailRow = CreateDs.AApDocumentDetail.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldDetailRow, NewDetailRow);
                    NewDetailRow.ApDocumentId = NewDocumentRow.ApDocumentId;
                    CreateDs.AApDocumentDetail.Rows.Add(NewDetailRow);
                }

                //
                // if the invoice had AnalAttrib records attached, I need to copy those over..
                TempDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                    AApAnalAttribTable.GetApDocumentIdDBName(), OldDocumentRow.ApDocumentId);

                foreach (DataRowView rv in TempDS.AApAnalAttrib.DefaultView)
                {
                    AApAnalAttribRow OldAttribRow = (AApAnalAttribRow)rv.Row;
                    AApAnalAttribRow NewAttribRow = CreateDs.AApAnalAttrib.NewRowTyped();
                    DataUtilities.CopyAllColumnValues(OldAttribRow, NewAttribRow);
                    NewAttribRow.ApDocumentId = NewDocumentRow.ApDocumentId;
                    CreateDs.AApAnalAttrib.Rows.Add(NewAttribRow);
                }

                NewApNum--; // These negative record numbers should be replaced on posting.
            }

            if (SaveAApDocument(ref CreateDs, out AVerifications) != TSubmitChangesResult.scrOK)
            {
                //
                // What to do now? I've cancelled the previous payment, but I can't re-create it.
                //
                return false;
            }

            //
            // The process of saving those new documents should have given them all shiny new ApNumbers,
            // So finally I need to make a list of those Document numbers, and post them.
            PostTheseDocs.Clear();

            foreach (AApDocumentRow DocumentRow in CreateDs.AApDocument.Rows)
            {
                PostTheseDocs.Add(DocumentRow.ApDocumentId);
            }

            if (!PostAPDocuments(ALedgerNumber, PostTheseDocs, APostingDate, out AVerifications))
            {
                //
                // What to do now? These shiny new documents don't post!
                //
                return false;
            }

            return true;
        }
    }
}