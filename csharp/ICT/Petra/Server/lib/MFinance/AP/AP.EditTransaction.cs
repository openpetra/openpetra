//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, Tim Ingham
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
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.AP.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Accounts Payable screens
    ///</summary>
    public partial class TAPTransactionWebConnector
    {
        /// <summary>
        /// Retrieve all the information for the current Ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns>ALedgerTable</returns>
        [RequireModulePermission("FINANCE-1")]
        public static ALedgerTable GetLedgerInfo(Int32 ALedgerNumber)
        {
            ALedgerTable Tbl = null;
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref ReadTransaction,
                delegate
                {
                    Tbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, ReadTransaction);
                });

            return Tbl;
        }

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
        /// Loads Supplier and Partner.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        /// <returns>TDS with tables loaded</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS LoadAApSupplier(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            // create the DataSet that will be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    AApSupplierAccess.LoadByPrimaryKey(MainDS, APartnerKey, transaction);
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, transaction);
                });
            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();
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

            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    AccountsPayableTDSAApDocumentRow DocumentRow = (AccountsPayableTDSAApDocumentRow)
                                                                   AApDocumentAccess.LoadByPrimaryKey(MainDS, AApDocumentId, transaction);

                    // If the load didn't work, don't bother with anything else..
                    if (MainDS.AApDocument.Count > 0)
                    {
                        SetOutstandingAmount(DocumentRow, ALedgerNumber, MainDS.AApDocumentPayment);

                        AApDocumentDetailAccess.LoadViaAApDocument(MainDS, AApDocumentId, transaction);
                        AApSupplierAccess.LoadByPrimaryKey(MainDS, DocumentRow.PartnerKey, transaction);

                        AApAnalAttribAccess.LoadViaAApDocument(MainDS, AApDocumentId, transaction);

                        // Accept row changes here so that the Client gets 'unmodified' rows
                        MainDS.AcceptChanges();

                        // I also need a full list of analysis attributes that could apply to this document
                        // (although if it's already been posted I don't need to get this...)

                        LoadAnalysisAttributes(MainDS, ALedgerNumber, transaction);
                    }
                });

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// Create a new AP document (invoice or credit note) and fill with default values from the supplier
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
            NewDocumentRow.LastDetailNumber = 0;

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ALedgerTable LedgerTbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                    // get the supplier defaults
                    AApSupplierRow SupplierRow = AApSupplierAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                    if (SupplierRow != null)
                    {
                        if (!SupplierRow.IsDefaultCreditTermsNull())
                        {
                            NewDocumentRow.CreditTerms = SupplierRow.DefaultCreditTerms;
                        }

                        if (!SupplierRow.IsDefaultDiscountDaysNull())
                        {
                            NewDocumentRow.DiscountDays = SupplierRow.DefaultDiscountDays;
                            NewDocumentRow.DiscountPercentage = 0;
                        }

                        if (!SupplierRow.IsDefaultDiscountPercentageNull())
                        {
                            NewDocumentRow.DiscountPercentage = SupplierRow.DefaultDiscountPercentage;
                        }

                        if (!SupplierRow.IsDefaultApAccountNull())
                        {
                            NewDocumentRow.ApAccount = SupplierRow.DefaultApAccount;
                        }

                        NewDocumentRow.CurrencyCode = SupplierRow.CurrencyCode;
                        NewDocumentRow.ExchangeRateToBase = TExchangeRateTools.GetDailyExchangeRate(NewDocumentRow.CurrencyCode,
                            LedgerTbl[0].BaseCurrency,
                            DateTime.Now);
                    }

                    MainDS.AApDocument.Rows.Add(NewDocumentRow);

                    // I also need a full list of analysis attributes that could apply to this document

                    LoadAnalysisAttributes(MainDS, ALedgerNumber, Transaction);
                });
            return MainDS;
        } // Create AApDocument

        /// <summary>
        /// Get the next available ApNumber for a new document.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        private static Int32 NextApDocumentNumber(Int32 ALedgerNumber,
            TDBTransaction ATransaction)
        {
            Int32 NewApNum = 1;
            Object MaxVal =
                DBAccess.GDBAccessObj.ExecuteScalar(String.Format("SELECT max(a_ap_number_i) from PUB_a_ap_document where a_ledger_number_i={0}",
                        ALedgerNumber), ATransaction);

            if (MaxVal.GetType() != typeof(System.DBNull))
            {
                NewApNum = Convert.ToInt32(MaxVal) + 1;
            }

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
            AccountsPayableTDS InspectDS = AInspectDS;
            TVerificationResultCollection LocalVerificationResults = new TVerificationResultCollection();

            AVerificationResult = LocalVerificationResults;

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            TDBTransaction transaction = null;
            Boolean SubmissionOK = false;
            TSubmitChangesResult result = TSubmitChangesResult.scrError;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref transaction, ref SubmissionOK,
                delegate
                {
                    if ((InspectDS.AApDocument != null) && (InspectDS.AApDocument.Rows.Count > 0))
                    {
                        // I want to check that the Invoice numbers are not blank,
                        // and that none of the documents already exist in the database.

                        foreach (AApDocumentRow NewDocRow in InspectDS.AApDocument.Rows)
                        {
                            if (NewDocRow.DocumentCode.Length == 0)
                            {
                                LocalVerificationResults.Add(new TVerificationResult(Catalog.GetString("Save Document"),
                                        Catalog.GetString("The Document has no Document number."),
                                        TResultSeverity.Resv_Noncritical));
                                result = TSubmitChangesResult.scrInfoNeeded;
                                return;
                            }

                            AApDocumentRow DocTemplateRow = InspectDS.AApDocument.NewRowTyped(false);
                            DocTemplateRow.LedgerNumber = NewDocRow.LedgerNumber;
                            DocTemplateRow.PartnerKey = NewDocRow.PartnerKey;
                            DocTemplateRow.DocumentCode = NewDocRow.DocumentCode;
                            AApDocumentTable MatchingRecords = AApDocumentAccess.LoadUsingTemplate(DocTemplateRow, transaction);

                            foreach (AApDocumentRow MatchingRow in MatchingRecords.Rows) // Generally I expect this table is empty..
                            {
                                if (MatchingRow.ApDocumentId != NewDocRow.ApDocumentId) // This Document Code is in use, and not by me!
                                {
                                    LocalVerificationResults.Add(new TVerificationResult(Catalog.GetString("Save Document"),
                                            String.Format(Catalog.GetString("Document Code {0} already exists."), NewDocRow.DocumentCode),
                                            TResultSeverity.Resv_Noncritical));
                                    result = TSubmitChangesResult.scrInfoNeeded;
                                    return;
                                }
                            }
                        } // foreach (document)
                    } // if {there's actually a document}

                    if (InspectDS.AApDocument != null)
                    {
                        foreach (AccountsPayableTDSAApDocumentRow NewDocRow in InspectDS.AApDocument.Rows)
                        {
                            // Set AP Number if it has not been set yet.
                            if (NewDocRow.ApNumber < 0)
                            {
                                NewDocRow.ApNumber = NextApDocumentNumber(NewDocRow.LedgerNumber, transaction);
                            }

                            SetOutstandingAmount(NewDocRow, NewDocRow.LedgerNumber, InspectDS.AApDocumentPayment);
                        }

                        AApDocumentAccess.SubmitChanges(InspectDS.AApDocument, transaction);
                    }

                    if (InspectDS.AApDocumentDetail != null) // Document detail lines
                    {
                        ValidateApDocumentDetail(ref LocalVerificationResults, InspectDS.AApDocumentDetail);
                        ValidateApDocumentDetailManual(ref LocalVerificationResults, InspectDS.AApDocumentDetail);

                        if (TVerificationHelper.IsNullOrOnlyNonCritical(LocalVerificationResults))
                        {
                            AApDocumentDetailAccess.SubmitChanges(InspectDS.AApDocumentDetail, transaction);
                        }
                    }

                    if (InspectDS.AApAnalAttrib != null) // Analysis attributes
                    {
                        AApAnalAttribAccess.SubmitChanges(InspectDS.AApAnalAttrib, transaction);
                    }

                    result = TSubmitChangesResult.scrOK;
                    SubmissionOK = true;
                });

            // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
            // Serialisation (needed for .NET Remoting).
            TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);

            return result;
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
            NewRow.DetailNumber = ALastDetailNumber + 1;
            NewRow.Amount = AAmount;
            NewRow.CostCentreCode = AApSupplier_DefaultCostCentre;
            NewRow.AccountCode = AApSupplier_DefaultExpAccount;

            MainDS.AApDocumentDetail.Rows.Add(NewRow);

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        private static Boolean LedgerRquiresDocumentApproval(Int32 ALedgerNumber, TDBTransaction AReadTrans)
        {
            ALedgerInitFlagTable Tbl = ALedgerInitFlagAccess.LoadViaALedger(ALedgerNumber, AReadTrans);

            Tbl.DefaultView.RowFilter = "a_init_option_name_c='AP_APPROVE_BLOCK'";
            return Tbl.DefaultView.Count > 0;
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
                Row.OutstandingAmount -= UIConnectors.TFindUIConnector.GetPartPaidAmount(Row.ApDocumentId);
            }
        }

        private static bool DocumentBalanceOK(AccountsPayableTDS AMainDS, int AApDocumentId, TDBTransaction ATransaction)
        {
            AccountsPayableTDSAApDocumentRow DocumentRow = (AccountsPayableTDSAApDocumentRow)
                                                           AApDocumentAccess.LoadByPrimaryKey(AMainDS, AApDocumentId, ATransaction);
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
                    String.Format("{0}='{1}'", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode);

                if (AMainDS.AAnalysisAttribute.DefaultView.Count > 0)
                {
                    foreach (DataRowView aa_rv in AMainDS.AAnalysisAttribute.DefaultView)
                    {
                        AAnalysisAttributeRow AttrRow = (AAnalysisAttributeRow)aa_rv.Row;

                        AMainDS.AApAnalAttrib.DefaultView.RowFilter =
                            String.Format("{0}={1} AND {2}='{3}'",
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
        /// <returns> The TDS for posting</returns>
        private static AccountsPayableTDS LoadDocumentsAndCheck(Int32 ALedgerNumber,
            List <Int32>AAPDocumentIds,
            DateTime APostingDate,
            Boolean AReversal,
            out Boolean AMustBeApproved,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            TVerificationResultCollection Verifications = new TVerificationResultCollection();

            AVerifications = Verifications;
            Boolean MustBeApproved = false;

            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    // collect the AP documents from the database
                    foreach (Int32 APDocumentId in AAPDocumentIds)
                    {
                        AApDocumentAccess.LoadByPrimaryKey(MainDS, APDocumentId, transaction);
                        AApDocumentDetailAccess.LoadViaAApDocument(MainDS, APDocumentId, transaction);
                    }

                    MustBeApproved = LedgerRquiresDocumentApproval(ALedgerNumber, transaction);

                    // do some checks on state of AP documents
                    foreach (AApDocumentRow document in MainDS.AApDocument.Rows)
                    {
                        if (AReversal)
                        {
                            if (document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_POSTED)
                            {
                                Verifications.Add(new TVerificationResult(
                                        Catalog.GetString("Error during reversal of posted AP document"),
                                        String.Format(Catalog.GetString("Document Number {0} cannot be reversed since the status is {1}."),
                                            document.ApNumber, document.DocumentStatus), TResultSeverity.Resv_Critical));
                            }
                        }
                        else
                        {
                            if (
                                (MustBeApproved && (document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_APPROVED))
                                || (!MustBeApproved
                                    && ((document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_OPEN)
                                        && (document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_APPROVED)))
                                )
                            {
                                Verifications.Add(new TVerificationResult(
                                        Catalog.GetString("Error during posting of AP document"),
                                        String.Format(Catalog.GetString("Document Number {0} cannot be posted since the status is {1}."),
                                            document.ApNumber, document.DocumentStatus), TResultSeverity.Resv_Critical));
                            }
                        }

                        // TODO: also check if details are filled, and they each have a costcentre and account?

                        // TODO: check for document.apaccount, if not set, get the default apaccount from the supplier, and save the ap document

                        // Check that the amount of the document equals the totals of details
                        if (!DocumentBalanceOK(MainDS, document.ApDocumentId, transaction))
                        {
                            Verifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), document.ApNumber,
                                        ALedgerNumber),
                                    String.Format(Catalog.GetString("The value does not match the sum of the details.")),
                                    TResultSeverity.Resv_Critical));
                        }

                        // Load Analysis Attributes and check they're all present.
                        if (!AttributesAllOK(MainDS, ALedgerNumber, document.ApDocumentId, transaction))
                        {
                            Verifications.Add(new TVerificationResult(
                                    String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), document.ApNumber,
                                        ALedgerNumber),
                                    String.Format(Catalog.GetString("Analysis Attributes are required.")),
                                    TResultSeverity.Resv_Critical));
                        }
                    }  //foreach

                    // is APostingDate inside the valid posting periods?
                    Int32 DateEffectivePeriodNumber, DateEffectiveYearNumber;

                    if (!TFinancialYear.IsValidPostingPeriod(ALedgerNumber, APostingDate, out DateEffectivePeriodNumber, out DateEffectiveYearNumber,
                            transaction))
                    {
                        Verifications.Add(new TVerificationResult(
                                String.Format(Catalog.GetString("Cannot post the AP documents in Ledger {0}"), ALedgerNumber),
                                String.Format(Catalog.GetString("The Date Effective {0:d-MMM-yyyy} does not fit any open accounting period."),
                                    APostingDate),
                                TResultSeverity.Resv_Critical));
                    }
                }); // Get NewOrExisting AutoReadTransaction
            AMustBeApproved = MustBeApproved;
            return MainDS;
        } // Load Documents And Check

        /// <summary>
        /// creates the GL batch needed for posting the AP Documents
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingDate"></param>
        /// <param name="Reversal"></param>
        /// <param name="APDataset"></param>
        /// <returns>Batch for posting</returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPosting(
            Int32 ALedgerNumber,
            DateTime APostingDate,
            Boolean Reversal,
            ref AccountsPayableTDS APDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = TGLTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            batch.BatchDescription = Catalog.GetString("Accounts Payable");

            if (Reversal)
            {
                batch.BatchDescription = Catalog.GetString("Reversal: ") + batch.BatchDescription;
            }

            batch.DateEffective = APostingDate;
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // since the list of documents can be for several suppliers, the currency might be different; group by currency first
            SortedList <string, List <AApDocumentRow>>DocumentsByCurrency = new SortedList <string, List <AApDocumentRow>>();

            foreach (AApDocumentRow row in APDataset.AApDocument.Rows)
            {
                string CurrencyCode = (row.CurrencyCode + "|" + row.ExchangeRateToBase.ToString());  // If douments with the same currency are using different

                // exchange rates, I'm going to handle them separately.
                if (!DocumentsByCurrency.ContainsKey(CurrencyCode))
                {
                    DocumentsByCurrency.Add(CurrencyCode, new List <AApDocumentRow>());
                }

                DocumentsByCurrency[CurrencyCode].Add(row);
            }

            Int32 CounterJournals = 1;

            // Add journal for each currency / Exchange Rate and the transactions
            foreach (string CurrencyCode in DocumentsByCurrency.Keys)
            {
                AJournalRow journal = GLDataset.AJournal.NewRowTyped();
                journal.LedgerNumber = batch.LedgerNumber;
                journal.BatchNumber = batch.BatchNumber;
                journal.JournalNumber = CounterJournals++;
                journal.DateEffective = batch.DateEffective;
                journal.TransactionCurrency = CurrencyCode.Substring(0, CurrencyCode.IndexOf("|"));
                journal.JournalDescription = "AP";

                int baseCurrencyDecimalPlaces = 0; // This will not be used unless this is a foreign journal.
                int intlCurrencyDecimalPlaces = 0;

                if (journal.TransactionCurrency != GLDataset.ALedger[0].BaseCurrency)
                {
                    baseCurrencyDecimalPlaces = StringHelper.DecimalPlacesForCurrency(GLDataset.ALedger[0].BaseCurrency);
                    intlCurrencyDecimalPlaces = StringHelper.DecimalPlacesForCurrency(GLDataset.ALedger[0].IntlCurrency);
                }

                if (Reversal)
                {
                    journal.JournalDescription = "Reversal: AP";
                }

                journal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
                journal.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                journal.DateOfEntry = DateTime.Now;

                // I'm not using the Daily Exchange Rate, since the exchange rate has been specified by the user in the document.
                // using the exchange rate from the first ap document in this set of documents with same currency and exchange rate
                journal.ExchangeRateToBase = DocumentsByCurrency[CurrencyCode][0].ExchangeRateToBase;
                journal.ExchangeRateTime = 0;
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
                        transaction.TransactionDate = batch.DateEffective;
                        transaction.SystemGenerated = true;

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

                        if (Reversal)
                        {
                            transaction.TransactionAmount *= -1; // this is going to post everything backwards for me.
                        }

                        transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                        if (transaction.TransactionAmount < 0)
                        {
                            transaction.TransactionAmount *= -1;
                        }

                        transaction.AmountInBaseCurrency = GLRoutines.Divide(transaction.TransactionAmount,
                            journal.ExchangeRateToBase, baseCurrencyDecimalPlaces);

                        transaction.AmountInIntlCurrency = GLRoutines.Divide(transaction.AmountInBaseCurrency,
                            TExchangeRateTools.GetDailyExchangeRate(
                                GLDataset.ALedger[0].BaseCurrency,
                                GLDataset.ALedger[0].IntlCurrency,
                                transaction.TransactionDate),
                            intlCurrencyDecimalPlaces);

                        transaction.AccountCode = documentDetail.AccountCode;
                        transaction.CostCentreCode = documentDetail.CostCentreCode;
                        transaction.Narrative = "AP " + document.ApNumber.ToString() + " - " + documentDetail.Narrative + " - " + SupplierShortName;

                        if (Reversal)
                        {
                            transaction.Narrative = "Reversal: " + transaction.Narrative;
                        }

                        transaction.Reference = documentDetail.ItemRef;
//                      transaction.Reference = "AP " + document.ApNumber.ToString() + " - " + document.DocumentCode;

                        transaction.DetailNumber = documentDetail.DetailNumber;

                        GLDataset.ATransaction.Rows.Add(transaction);
                    }

                    // create one transaction for the AP account
                    transaction = GLDataset.ATransaction.NewRowTyped();
                    transaction.LedgerNumber = journal.LedgerNumber;
                    transaction.BatchNumber = journal.BatchNumber;
                    transaction.JournalNumber = journal.JournalNumber;
                    transaction.TransactionNumber = TransactionCounter++;
                    transaction.TransactionAmount = document.TotalAmount;
                    transaction.TransactionDate = batch.DateEffective;
                    transaction.SystemGenerated = true;

                    if (!document.CreditNoteFlag)
                    {
                        transaction.TransactionAmount *= -1;
                    }

                    if (Reversal)
                    {
                        transaction.TransactionAmount *= -1; // this is going to post everything backwards for me.
                    }

                    transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                    if (transaction.TransactionAmount < 0)
                    {
                        transaction.TransactionAmount *= -1;
                    }

                    transaction.AmountInIntlCurrency = GLRoutines.Divide(transaction.TransactionAmount,
                        TExchangeRateTools.GetDailyExchangeRate(
                            journal.TransactionCurrency,
                            GLDataset.ALedger[0].IntlCurrency,
                            transaction.TransactionDate), intlCurrencyDecimalPlaces);

                    transaction.AmountInBaseCurrency = GLRoutines.Divide(transaction.TransactionAmount,
                        journal.ExchangeRateToBase, baseCurrencyDecimalPlaces);

                    transaction.AccountCode = document.ApAccount;
                    transaction.CostCentreCode = TGLTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
                    transaction.Reference = "AP " + document.ApNumber.ToString() + " - " + document.DocumentCode;
                    transaction.Narrative = transaction.Reference + " - " + SupplierShortName;

                    if (Reversal)
                    {
                        transaction.Narrative = "Reversal: " + transaction.Narrative;
                    }

                    transaction.DetailNumber = 0;

                    GLDataset.ATransaction.Rows.Add(transaction);
                }

                journal.LastTransactionNumber = TransactionCounter - 1;
            }

            batch.LastJournal = CounterJournals - 1;

            return GLDataset;
        }

        /// <summary>
        /// Check that the Account codes for an invoice can be used with the cost centres referenced.
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AccountCodesCostCentres">list {Account"|"Cost Centre}</param>
        /// <returns>Empty string if there's no problems</returns>
        [RequireModulePermission("FINANCE-3")]
        public static String CheckAccountsAndCostCentres(Int32 ALedgerNumber, List <String>AccountCodesCostCentres)
        {
            String ReportMsg = "";
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref ReadTransaction,
                delegate
                {
                    foreach (String AccCostCentre in AccountCodesCostCentres)
                    {
                        Int32 BarPos = AccCostCentre.IndexOf("|");
                        String AccountCode = AccCostCentre.Substring(0, BarPos);
                        AAccountTable AccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AccountCode, ReadTransaction);
                        String ValidCcCombo = AccountTbl[0].ValidCcCombo.ToLower();

                        // If this account goes with any cost centre (as is likely),
                        // there's nothing more to do.

                        if (ValidCcCombo != "all")
                        {
                            String CostCentre = AccCostCentre.Substring(BarPos + 1);
                            ACostCentreTable CcTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, CostCentre, ReadTransaction);
                            String CcType = CcTbl[0].CostCentreType.ToLower();

                            if (ValidCcCombo != CcType)
                            {
                                ReportMsg +=
                                    String.Format(Catalog.GetString(
                                            "Error: Account {0} cannot be used with cost centre {1}. Account requires a {2} cost centre."),
                                        AccountCode, CostCentre, ValidCcCombo);
                                ReportMsg += Environment.NewLine;
                            }
                        }
                    }
                }); // End of BeginAutoReadTransaction
            return ReportMsg;
        }

        /// <summary>
        /// Approve documents that have an OPEN status
        /// This is called by a client
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApproveTheseDocs"></param>
        /// <param name="AVerificationResult"></param>
        [RequireModulePermission("FINANCE-1")]
        public static bool ApproveAPDocuments(Int32 ALedgerNumber,
            List <Int32>AApproveTheseDocs,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            bool ResultValue = false;

            AccountsPayableTDS TempDS = new AccountsPayableTDS();

            if (AApproveTheseDocs.Count == 0)
            {
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Approve AP Documents"),
                        Catalog.GetString("Nothing to do - the document list is empty"),
                        TResultSeverity.Resv_Noncritical));
                return false;
            }

            foreach (Int32 ApDocumentId in AApproveTheseDocs)
            {
                TempDS.Merge(LoadAApDocument(ALedgerNumber, ApDocumentId)); // This gives me documents, details, and potentially ap_anal_attrib records.
            }

            foreach (AApDocumentRow ApDocumentRow in TempDS.AApDocument.Rows)
            {
                if (ApDocumentRow.DocumentStatus == MFinanceConstants.AP_DOCUMENT_OPEN)
                {
                    ApDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
                }
                else
                {
                    AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Approve AP Documents"),
                            Catalog.GetString("Only OPEN documents can be approved"),
                            TResultSeverity.Resv_Noncritical));
                    return false;
                }
            }

            TDBTransaction transaction = null;
            Boolean submissionOk = true;
            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref transaction, ref submissionOk,
                delegate
                {
                    AApDocumentAccess.SubmitChanges(TempDS.AApDocument, transaction);
                    ResultValue = true;
                });
            return ResultValue;
        }

        /// <summary>
        /// Documents can only be deleted if they're not posted yet.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADeleteTheseDocs"></param>
        [RequireModulePermission("FINANCE-3")]
        public static void DeleteAPDocuments(Int32 ALedgerNumber, List <Int32>ADeleteTheseDocs)
        {
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

            Boolean submissionOK = true;
            TDBTransaction transaction = null;
            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref transaction, ref submissionOK,
                delegate
                {
                    AApAnalAttribAccess.SubmitChanges(TempDS.AApAnalAttrib, transaction);
                    AApDocumentDetailAccess.SubmitChanges(TempDS.AApDocumentDetail, transaction);
                    AApDocumentAccess.SubmitChanges(TempDS.AApDocument, transaction);
                });
        }

        /// <summary>
        /// creates GL transactions for the selected AP documents,
        /// and posts those GL transactions,
        /// and marks the AP documents as Posted
        /// Also used to "un-post" posted documents.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAPDocumentIds"></param>
        /// <param name="APostingDate"></param>
        /// <param name="Reversal"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPDocuments(Int32 ALedgerNumber,
            List <Int32>AAPDocumentIds,
            DateTime APostingDate,
            Boolean Reversal,
            out TVerificationResultCollection AVerificationResult)
        {
            bool PostingWorkedOk = true;
            ABatchRow batch;
            TVerificationResultCollection ResultsCollection = new TVerificationResultCollection();

            TDBTransaction HighLevelTransaction = null;
            Boolean WillCommit = true;
            Boolean MustBeApproved;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref HighLevelTransaction, ref WillCommit,
                delegate
                {
                    AccountsPayableTDS MainDS = LoadDocumentsAndCheck(ALedgerNumber, AAPDocumentIds, APostingDate, Reversal,
                        out MustBeApproved,
                        out ResultsCollection);

                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(ResultsCollection))
                    {
                        PostingWorkedOk = false;
                        return; // This is returning from the AutoTransaction, not from the whole method.
                    }

                    GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPosting(ALedgerNumber, APostingDate, Reversal, ref MainDS);

                    batch = GLDataset.ABatch[0];

                    // save the batch
                    if (TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                            out ResultsCollection) != TSubmitChangesResult.scrOK)
                    {
                        PostingWorkedOk = false;
                    }

                    // post the batch
                    if (PostingWorkedOk)
                    {
                        PostingWorkedOk = TGLPosting.PostGLBatch(ALedgerNumber, batch.BatchNumber, out ResultsCollection);
                    }

                    if (!PostingWorkedOk)
                    {
                        TVerificationResultCollection MoreResults;

                        TGLPosting.DeleteGLBatch(
                            ALedgerNumber,
                            GLDataset.ABatch[0].BatchNumber,
                            out MoreResults);
                        ResultsCollection.AddCollection(MoreResults);

                        return;
                    }

                    // GL posting is OK - change status of AP documents and save to database
                    foreach (AApDocumentRow row in MainDS.AApDocument.Rows)
                    {
                        if (Reversal)
                        {
                            row.DocumentStatus = MustBeApproved ? MFinanceConstants.AP_DOCUMENT_APPROVED : MFinanceConstants.AP_DOCUMENT_OPEN;
                        }
                        else
                        {
                            row.DocumentStatus = MFinanceConstants.AP_DOCUMENT_POSTED;
                        }
                    }

                    try
                    {
                        AApDocumentAccess.SubmitChanges(MainDS.AApDocument, HighLevelTransaction);
                    }
                    catch (Exception Exc)
                    {
                        // Now I've got GL entries, but "unposted" AP documents!

                        TLogging.Log("An Exception occured during the Posting of an AP Document:" + Environment.NewLine + Exc.ToString());

                        ResultsCollection.Add(new TVerificationResult(Catalog.GetString("Post AP Document"),
                                Catalog.GetString("NOTE THAT A GL ENTRY MAY HAVE BEEN CREATED.") + Environment.NewLine +
                                Exc.Message,
                                TResultSeverity.Resv_Critical));
                        PostingWorkedOk = false;
                        return;
                    }
                });

            AVerificationResult = ResultsCollection;    // The System.Action defined in the delegate below cannot directly access
                                                        // "out" parameters, so this intermediate variable was used.
            return PostingWorkedOk;
        }

        /// <summary>
        /// Creates the GL batch needed for paying the AP Documents
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APostingDate"></param>
        /// <param name="APDataset"></param>
        /// <returns></returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPaying(Int32 ALedgerNumber, DateTime APostingDate, ref AccountsPayableTDS APDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = TGLTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            batch.BatchDescription = Catalog.GetString("Accounts Payable Payment");
            batch.DateEffective = APostingDate;
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // since the list of documents can be for several suppliers, there could be more than one currency; group by currency first
            SortedList <string,
                        List <AccountsPayableTDSAApPaymentRow>>DocumentsByCurrency = new SortedList <string, List <AccountsPayableTDSAApPaymentRow>>();

            foreach (AccountsPayableTDSAApPaymentRow row in APDataset.AApPayment.Rows)
            {
                // Get the currency from the supplier, from the first documentpayment of this payment; we need the currency
                APDataset.AApDocumentPayment.DefaultView.RowFilter = AApDocumentPaymentTable.GetPaymentNumberDBName() + " = " +
                                                                     row.PaymentNumber.ToString();

                if (APDataset.AApDocumentPayment.DefaultView.Count == 0) // I'm not sure if this is allowable, but it's better than crashing...
                {
                    continue;
                }

                APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApDocumentIdDBName() + " = " +
                                                              ((AApDocumentPaymentRow)APDataset.AApDocumentPayment.DefaultView[0].Row).ApDocumentId.
                                                              ToString();

                if (APDataset.AApDocument.DefaultView.Count == 0) // I'm not sure if this is allowable, but it's better than crashing...
                {
                    continue;
                }

                AApDocumentRow documentRow = (AApDocumentRow)APDataset.AApDocument.DefaultView[0].Row;
                row.SupplierKey = documentRow.PartnerKey;

                string CurrencyCode = documentRow.CurrencyCode;

                if (row.IsExchangeRateToBaseNull())
                {
                    CurrencyCode += "|1.0m";
                }
                else
                {
                    CurrencyCode += ("|" + row.ExchangeRateToBase.ToString());  // If documents with the same currency are using different
                                                                                // exchange rates, I'm going to handle them separately.
                }

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

            // Add a journal for each currency/exchangeRate, and the transactions.
            // Most likely only one currency/exchangeRate will be used!

            foreach (string CurrencyCode in DocumentsByCurrency.Keys)
            {
                String StandardCostCentre = TLedgerInfo.GetStandardCostCentre(batch.LedgerNumber);
                Dictionary <String, Decimal>ForexGain = new Dictionary <string, decimal>(); // ForexGain is recorded for each AP account in use.

                AJournalRow journalRow = GLDataset.AJournal.NewRowTyped();
                journalRow.LedgerNumber = batch.LedgerNumber;
                journalRow.BatchNumber = batch.BatchNumber;
                journalRow.JournalNumber = CounterJournals++;
                journalRow.DateEffective = batch.DateEffective;
                journalRow.TransactionCurrency = CurrencyCode.Substring(0, CurrencyCode.IndexOf("|"));
                journalRow.JournalDescription = "AP";
                journalRow.TransactionTypeCode = CommonAccountingTransactionTypesEnum.INV.ToString();
                journalRow.SubSystemCode = CommonAccountingSubSystemsEnum.AP.ToString();
                journalRow.DateOfEntry = DateTime.Now;

                int baseCurrencyDecimalPlaces = 0; // This will not be used unless this is a foreign journal.
                int intlCurrencyDecimalPlaces = 0;

                if (journalRow.TransactionCurrency != GLDataset.ALedger[0].BaseCurrency)
                {
                    baseCurrencyDecimalPlaces = StringHelper.DecimalPlacesForCurrency(GLDataset.ALedger[0].BaseCurrency);
                    intlCurrencyDecimalPlaces = StringHelper.DecimalPlacesForCurrency(GLDataset.ALedger[0].IntlCurrency);
                }

                // I'm not using the Daily Exchange Rate, since the exchange rate has been specified by the user in the payment.
                // using the exchange rate from the first payment in this set of payments with same currency and exchange rate
                journalRow.ExchangeRateTime = 0;

                if (DocumentsByCurrency[CurrencyCode][0].IsExchangeRateToBaseNull())
                {
                    journalRow.ExchangeRateToBase = 1.0m;
                }
                else
                {
                    journalRow.ExchangeRateToBase = DocumentsByCurrency[CurrencyCode][0].ExchangeRateToBase;
                }

                GLDataset.AJournal.Rows.Add(journalRow);

                Int32 TransactionCounter = 1;

                foreach (AccountsPayableTDSAApPaymentRow paymentRow in DocumentsByCurrency[CurrencyCode])
                {
                    DataView DocumentPaymentView = APDataset.AApDocumentPayment.DefaultView;
                    DocumentPaymentView.RowFilter = AApDocumentPaymentTable.GetPaymentNumberDBName() + " = " + paymentRow.PaymentNumber.ToString();

                    foreach (DataRowView rowview in DocumentPaymentView)
                    {
                        AApDocumentPaymentRow documentPaymentRow = (AApDocumentPaymentRow)rowview.Row;
                        APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApDocumentIdDBName() + " = " +
                                                                      documentPaymentRow.ApDocumentId.ToString();
                        AApDocumentRow documentRow = (AApDocumentRow)APDataset.AApDocument.DefaultView[0].Row;

                        ATransactionRow transactionRowBank = null;
                        GLDataset.ATransaction.DefaultView.RowFilter =
                            "a_account_code_c='" + paymentRow.BankAccount +
                            "' AND a_journal_number_i=" + journalRow.JournalNumber.ToString();

                        if (GLDataset.ATransaction.DefaultView.Count > 0)
                        {
                            transactionRowBank = (ATransactionRow)GLDataset.ATransaction.DefaultView[0].Row;
                            transactionRowBank.TransactionAmount += documentPaymentRow.Amount; // This TransactionAmount is unsigned until later.
                            transactionRowBank.Narrative = "AP Payment: Multiple suppliers";
                        }
                        else
                        {
                            transactionRowBank = GLDataset.ATransaction.NewRowTyped();
                            transactionRowBank.LedgerNumber = journalRow.LedgerNumber;
                            transactionRowBank.BatchNumber = journalRow.BatchNumber;
                            transactionRowBank.JournalNumber = journalRow.JournalNumber;
                            transactionRowBank.TransactionNumber = TransactionCounter++;
                            transactionRowBank.TransactionAmount = documentPaymentRow.Amount; // This TransactionAmount is unsigned until later.
                            transactionRowBank.AmountInBaseCurrency = 0; // This will be corrected later, after this nested loop.
                            transactionRowBank.TransactionDate = batch.DateEffective;
                            transactionRowBank.SystemGenerated = true;
                            transactionRowBank.AccountCode = paymentRow.BankAccount;
                            transactionRowBank.CostCentreCode = StandardCostCentre;
                            transactionRowBank.Narrative = "AP Payment: " + paymentRow.PaymentNumber.ToString() + " - " +
                                                           Ict.Petra.Shared.MPartner.Calculations.FormatShortName(paymentRow.SupplierName,
                                eShortNameFormat.eReverseWithoutTitle);
                            transactionRowBank.Reference = paymentRow.Reference;
                            GLDataset.ATransaction.Rows.Add(transactionRowBank);
                        }

                        ATransactionRow transactionRowAp = null;
                        GLDataset.ATransaction.DefaultView.RowFilter =
                            "a_account_code_c='" + documentRow.ApAccount +
                            "' AND a_journal_number_i=" + journalRow.JournalNumber.ToString();

                        if (GLDataset.ATransaction.DefaultView.Count > 0)
                        {
                            transactionRowAp = (ATransactionRow)GLDataset.ATransaction.DefaultView[0].Row;
                            transactionRowAp.TransactionAmount -= documentPaymentRow.Amount; // This TransactionAmount is unsigned until later.
                            transactionRowAp.Narrative += ", " + documentRow.ApNumber.ToString();
                        }
                        else
                        {
                            transactionRowAp = GLDataset.ATransaction.NewRowTyped();
                            transactionRowAp.LedgerNumber = journalRow.LedgerNumber;
                            transactionRowAp.BatchNumber = journalRow.BatchNumber;
                            transactionRowAp.JournalNumber = journalRow.JournalNumber;
                            transactionRowAp.TransactionNumber = TransactionCounter++;
                            transactionRowAp.TransactionAmount = 0 - documentPaymentRow.Amount;  // This TransactionAmount is unsigned until later.
                            transactionRowAp.TransactionDate = batch.DateEffective;
                            transactionRowAp.SystemGenerated = true;
                            transactionRowAp.AccountCode = documentRow.ApAccount;
                            transactionRowAp.CostCentreCode = StandardCostCentre;
                            transactionRowAp.Narrative = "AP Payment:" + paymentRow.PaymentNumber.ToString() + " AP: " +
                                                         documentRow.ApNumber.ToString();
                            transactionRowAp.Reference = paymentRow.Reference;
                            transactionRowAp.AmountInBaseCurrency = 0; // This will be corrected later, after this nested loop.
                            GLDataset.ATransaction.Rows.Add(transactionRowAp);
                        }

                        if (journalRow.TransactionCurrency != GLDataset.ALedger[0].BaseCurrency)
                        {
                            // This invoice is in a non-base currency, and the value of it in my base currency
                            // may have changed since it was first posted. To keep the ledger balanced,
                            // adjusting entries will made to the AP accounts,
                            // and a single balancing entry will made to the ForexGainsLossesAccount account.
                            // (Most often only one AP account per currency will be used.)

                            Decimal BaseAmountAtPosting = GLRoutines.Divide(documentPaymentRow.Amount,
                                documentRow.ExchangeRateToBase,
                                baseCurrencyDecimalPlaces);
                            Decimal BaseAmountNow = GLRoutines.Divide(documentPaymentRow.Amount,
                                paymentRow.ExchangeRateToBase,
                                baseCurrencyDecimalPlaces);
                            Decimal ForexDelta = (BaseAmountNow - BaseAmountAtPosting);

                            if (ForexDelta != 0)  // There's a good chance this will be 0!
                            {
                                if (ForexGain.ContainsKey(documentRow.ApAccount))
                                {
                                    ForexGain[documentRow.ApAccount] += ForexDelta;
                                }
                                else
                                {
                                    ForexGain.Add(documentRow.ApAccount, ForexDelta);
                                }
                            }
                        }
                    } // foreach DocumentPayment
                } // foreach Payment

                journalRow.LastTransactionNumber = TransactionCounter - 1;

                // So now I have one bank transaction per Bank Account credited
                // (Which is likely to be one per currency, but it could be more...)
                // and one AP transaction per AP account used
                // (Probably also one per currency.)
                // I need to set the international fields on the consolidated transaction rows:
                GLDataset.ATransaction.DefaultView.RowFilter = "a_journal_number_i=" + journalRow.JournalNumber.ToString();

                foreach (DataRowView rv in GLDataset.ATransaction.DefaultView)
                {
                    ATransactionRow tempTransRow = (ATransactionRow)rv.Row;

                    if (tempTransRow.AmountInBaseCurrency == 0) // I left this blank earlier as a marker.
                    {                                           // If any other rows have 0 here, (a) that's a fault, and (b) it's OK - it won't do any harm!
                        tempTransRow.DebitCreditIndicator = (tempTransRow.TransactionAmount < 0);

                        if (tempTransRow.TransactionAmount < 0)
                        {
                            tempTransRow.TransactionAmount *= -1;
                        }

                        if (journalRow.TransactionCurrency == GLDataset.ALedger[0].BaseCurrency)
                        {
                            tempTransRow.AmountInBaseCurrency = tempTransRow.TransactionAmount;
                        }
                        else
                        {
                            tempTransRow.AmountInBaseCurrency = GLRoutines.Divide(tempTransRow.TransactionAmount,
                                journalRow.ExchangeRateToBase,
                                baseCurrencyDecimalPlaces);

                            tempTransRow.AmountInIntlCurrency = GLRoutines.Divide(tempTransRow.AmountInBaseCurrency,
                                TExchangeRateTools.GetDailyExchangeRate(
                                    GLDataset.ALedger[0].BaseCurrency,
                                    GLDataset.ALedger[0].IntlCurrency,
                                    tempTransRow.TransactionDate),
                                intlCurrencyDecimalPlaces);
                        }
                    }
                }

                // The'base value' of these invoices may have changed since they were posted -
                // if this is the case I need a 'reval' journal to keep the books straight.
                // NOTE if the payment is already in Base Currency, the ForexGain dictionary will be empty.

                if (ForexGain.Count > 0) // One ForexGain per foreign currency would be normal, but there could be multiple AP accounts.
                {
                    Decimal TotalForexCorrection = 0;
                    // this goes into a separate REVAL journal in Base Currency.

                    AJournalRow RevalJournal = GLDataset.AJournal.NewRowTyped();
                    RevalJournal.LedgerNumber = batch.LedgerNumber;
                    RevalJournal.BatchNumber = batch.BatchNumber;
                    RevalJournal.JournalNumber = CounterJournals++;
                    RevalJournal.DateEffective = batch.DateEffective;
                    RevalJournal.TransactionCurrency = GLDataset.ALedger[0].BaseCurrency;
                    RevalJournal.JournalDescription = "AP Reval";
                    RevalJournal.TransactionTypeCode = CommonAccountingTransactionTypesEnum.REVAL.ToString();
                    RevalJournal.SubSystemCode = CommonAccountingSubSystemsEnum.GL.ToString();
                    RevalJournal.DateOfEntry = DateTime.Now;
                    RevalJournal.ExchangeRateToBase = 1.0m;
                    RevalJournal.ExchangeRateTime = 0;
                    GLDataset.AJournal.Rows.Add(RevalJournal);

                    TransactionCounter = 1;

                    foreach (String ApAccount in ForexGain.Keys)
                    {
                        // One transaction for each AP Account used:
                        ATransactionRow transactionApReval = GLDataset.ATransaction.NewRowTyped();
                        transactionApReval.LedgerNumber = RevalJournal.LedgerNumber;
                        transactionApReval.BatchNumber = RevalJournal.BatchNumber;
                        transactionApReval.JournalNumber = RevalJournal.JournalNumber;
                        transactionApReval.TransactionNumber = TransactionCounter++;
                        transactionApReval.Narrative = "AP expense reval";
                        transactionApReval.Reference = "";
                        transactionApReval.AccountCode = ApAccount;
                        transactionApReval.CostCentreCode = StandardCostCentre;
                        transactionApReval.TransactionAmount = 0; // no real value
                        transactionApReval.AmountInIntlCurrency = 0; // no real value
                        transactionApReval.TransactionDate = batch.DateEffective;
                        transactionApReval.SystemGenerated = true;
                        transactionApReval.DebitCreditIndicator = (ForexGain[ApAccount] < 0);
                        transactionApReval.AmountInBaseCurrency = Math.Abs(ForexGain[ApAccount]);
                        GLDataset.ATransaction.Rows.Add(transactionApReval);

                        TotalForexCorrection += ForexGain[ApAccount];
                    }

                    // A single transaction to the ForexGainsLossesAccount:
                    ATransactionRow transactionReval = GLDataset.ATransaction.NewRowTyped();
                    transactionReval.LedgerNumber = RevalJournal.LedgerNumber;
                    transactionReval.BatchNumber = RevalJournal.BatchNumber;
                    transactionReval.JournalNumber = RevalJournal.JournalNumber;
                    transactionReval.TransactionNumber = TransactionCounter++;
                    transactionReval.Narrative = "AP expense reval";
                    transactionReval.Reference = "";
                    transactionReval.AccountCode = GLDataset.ALedger[0].ForexGainsLossesAccount;
                    transactionReval.CostCentreCode = StandardCostCentre;
                    transactionReval.TransactionDate = batch.DateEffective;
                    transactionReval.SystemGenerated = true;
                    transactionReval.TransactionAmount = 0; // no real value
                    transactionReval.AmountInIntlCurrency = 0; // no real value
                    transactionReval.DebitCreditIndicator = (TotalForexCorrection > 0); // Opposite sign to those used above
                    transactionReval.AmountInBaseCurrency = Math.Abs(TotalForexCorrection);
                    GLDataset.ATransaction.Rows.Add(transactionReval);

                    RevalJournal.LastTransactionNumber = TransactionCounter - 1;
                }
            } // foreach currency

            batch.LastJournal = CounterJournals - 1;

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

            return (AApSupplierRow)Tbl.DefaultView[indexSupplier].Row;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADataset"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ADocumentsToPay"></param>
        /// <returns>There's really nothing to return, but generateGlue doesn't cope properly with <void> methods...</void></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool CreatePaymentTableEntries(ref AccountsPayableTDS ADataset, Int32 ALedgerNumber, List <Int32>ADocumentsToPay)
        {
            ADataset.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();

            foreach (Int32 ApDocId in ADocumentsToPay)
            {
                int indexDocument = ADataset.AApDocument.DefaultView.Find(ApDocId);
                //
                // I might not have the document loaded - if not I'll load it now.

                if (indexDocument == -1)
                {
                    ADataset.Merge(LoadAApDocument(ALedgerNumber, ApDocId));
                    indexDocument = ADataset.AApDocument.DefaultView.Find(ApDocId);
                }

                if (indexDocument != -1)  // If it's not loaded now, something really bad has happened!
                {
                    AccountsPayableTDSAApDocumentRow apDocumentRow =
                        (AccountsPayableTDSAApDocumentRow)ADataset.AApDocument.DefaultView[indexDocument].Row;

                    AApSupplierRow supplierRow = GetSupplier(ADataset.AApSupplier, apDocumentRow.PartnerKey);

                    if (supplierRow == null)
                    {
                        // I need to load the supplier record into the TDS...
                        ADataset.Merge(LoadAApSupplier(apDocumentRow.LedgerNumber, apDocumentRow.PartnerKey));
                        supplierRow = GetSupplier(ADataset.AApSupplier, apDocumentRow.PartnerKey);
                    }

                    if (supplierRow != null)
                    {
                        AccountsPayableTDSAApPaymentRow supplierPaymentsRow = null;

                        // My TDS may already have a AApPayment row for this supplier.
                        ADataset.AApPayment.DefaultView.RowFilter = String.Format("{0}='{1}'", AccountsPayableTDSAApPaymentTable.GetSupplierKeyDBName(
                                ), supplierRow.PartnerKey);

                        if (ADataset.AApPayment.DefaultView.Count > 0)
                        {
                            supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)ADataset.AApPayment.DefaultView[0].Row;

                            if (apDocumentRow.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay -= apDocumentRow.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay += apDocumentRow.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.
                        }
                        else
                        {
                            supplierPaymentsRow = ADataset.AApPayment.NewRowTyped();
                            supplierPaymentsRow.LedgerNumber = ADataset.AApDocument[0].LedgerNumber;
                            supplierPaymentsRow.PaymentNumber = -1 * (ADataset.AApPayment.Count + 1);
                            supplierPaymentsRow.SupplierKey = supplierRow.PartnerKey;
                            supplierPaymentsRow.MethodOfPayment = supplierRow.PaymentType;
                            supplierPaymentsRow.BankAccount = supplierRow.DefaultBankAccount;

                            supplierPaymentsRow.CurrencyCode = apDocumentRow.CurrencyCode;
                            supplierPaymentsRow.ExchangeRateToBase = apDocumentRow.ExchangeRateToBase; // The client may change this.

                            TPartnerClass partnerClass;
                            string partnerShortName;
                            TPartnerServerLookups.GetPartnerShortName(
                                supplierRow.PartnerKey,
                                out partnerShortName,
                                out partnerClass);
                            supplierPaymentsRow.SupplierName = Ict.Petra.Shared.MPartner.Calculations.FormatShortName(partnerShortName,
                                eShortNameFormat.eReverseWithoutTitle);

                            supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";

                            if (apDocumentRow.CreditNoteFlag)
                            {
                                supplierPaymentsRow.TotalAmountToPay = 0 - apDocumentRow.OutstandingAmount;
                            }
                            else
                            {
                                supplierPaymentsRow.TotalAmountToPay = apDocumentRow.OutstandingAmount;
                            }

                            supplierPaymentsRow.Amount = supplierPaymentsRow.TotalAmountToPay; // The user may choose to change the amount paid.

                            ADataset.AApPayment.Rows.Add(supplierPaymentsRow);
                        }

                        AccountsPayableTDSAApDocumentPaymentRow DocumentPaymentRow = ADataset.AApDocumentPayment.NewRowTyped();
                        DocumentPaymentRow.LedgerNumber = supplierPaymentsRow.LedgerNumber;
                        DocumentPaymentRow.PaymentNumber = supplierPaymentsRow.PaymentNumber;
                        DocumentPaymentRow.ApDocumentId = ApDocId;
                        DocumentPaymentRow.Amount = apDocumentRow.TotalAmount;
                        DocumentPaymentRow.InvoiceTotal = apDocumentRow.OutstandingAmount;

                        if (apDocumentRow.CreditNoteFlag)
                        {
                            DocumentPaymentRow.Amount = 0 - DocumentPaymentRow.Amount;
                            DocumentPaymentRow.InvoiceTotal = 0 - DocumentPaymentRow.InvoiceTotal;
                        }

                        DocumentPaymentRow.PayFullInvoice = true;

                        // TODO: discounts
                        DocumentPaymentRow.HasValidDiscount = false;
                        DocumentPaymentRow.DiscountPercentage = 0;
                        DocumentPaymentRow.UseDiscount = false;
                        DocumentPaymentRow.DocumentCode = apDocumentRow.DocumentCode;
                        DocumentPaymentRow.DocType = (apDocumentRow.CreditNoteFlag ? "CREDIT" : "INVOICE");
                        ADataset.AApDocumentPayment.Rows.Add(DocumentPaymentRow);
                    } // supplierRow != null
                } // indexDocument != -1
            }  // foreach document

            ADataset.AApPayment.DefaultView.RowFilter = "";
            return true;
        }

        /// <summary>
        /// Store payments in the database, and post the payment to GL
        /// </summary>
        /// <returns>true if it seemed to work OK</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPPayments(
            ref AccountsPayableTDS AMainDS,
            DateTime APostingDate,
            out TVerificationResultCollection AVerificationResult)
        {
            AccountsPayableTDS MainDS = AMainDS;

            TVerificationResultCollection VerificationResult = new TVerificationResultCollection();

            AVerificationResult = VerificationResult;

            if ((MainDS.AApPayment.Rows.Count < 1) || (MainDS.AApDocumentPayment.Rows.Count < 1))
            {
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult("Post Payment",
                        String.Format("Nothing to do - Payments has {0} rows, Documents has {1} rows.",
                            MainDS.AApPayment.Rows.Count, MainDS.AApDocumentPayment.Rows.Count), TResultSeverity.Resv_Noncritical));
                return false;
            }

            TDBTransaction transaction = null;
            Boolean SubmissionOK = false;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoTransaction(IsolationLevel.Serializable, ref transaction, ref SubmissionOK,
                delegate
                {
                    foreach (AccountsPayableTDSAApDocumentPaymentRow row in MainDS.AApDocumentPayment.Rows)
                    {
                        AccountsPayableTDSAApDocumentRow documentRow =
                            (AccountsPayableTDSAApDocumentRow)MainDS.AApDocument.Rows.Find(row.ApDocumentId);

                        if (documentRow != null)
                        {
                            MainDS.AApDocument.Rows.Remove(documentRow);
                        }

                        documentRow = (AccountsPayableTDSAApDocumentRow)
                                      AApDocumentAccess.LoadByPrimaryKey(MainDS, row.ApDocumentId, transaction);

                        SetOutstandingAmount(documentRow, documentRow.LedgerNumber, MainDS.AApDocumentPayment);

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
                        transaction);
                    Int32 maxPaymentNumberInLedger = (maxPaymentCanBeNull == System.DBNull.Value ? 0 : Convert.ToInt32(maxPaymentCanBeNull));

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

                    // create GL batch
                    GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPaying(MainDS.AApPayment[0].LedgerNumber,
                        APostingDate,
                        ref MainDS);

                    ABatchRow batch = GLDataset.ABatch[0];

                    // save the batch
                    Boolean PostingWorkedOk = (TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                                                   out VerificationResult) == TSubmitChangesResult.scrOK);

                    if (PostingWorkedOk)
                    {
                        // post the batch
                        PostingWorkedOk = TGLPosting.PostGLBatch(MainDS.AApPayment[0].LedgerNumber, batch.BatchNumber,
                            out VerificationResult);
                    }

                    if (!PostingWorkedOk)
                    {
                        TVerificationResultCollection MoreResults;

                        TGLPosting.DeleteGLBatch(
                            MainDS.AApPayment[0].LedgerNumber,
                            batch.BatchNumber,
                            out MoreResults);
                        VerificationResult.AddCollection(MoreResults);

                        return; // return from delegate
                    }

                    // store ApPayment and ApDocumentPayment to database
                    AApPaymentAccess.SubmitChanges(MainDS.AApPayment, transaction);
                    AApDocumentPaymentAccess.SubmitChanges(MainDS.AApDocumentPayment, transaction);

                    // save changed status of AP documents to database
                    AApDocumentAccess.SubmitChanges(MainDS.AApDocument, transaction);

                    SubmissionOK = true;
                }); // Get NewOrExisting Auto Transaction

            return SubmissionOK;
        } // Post AP Payments

        /// <summary>
        /// Load this payment, together with the supplier and all the related documents.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        /// <returns>Fully loaded TDS</returns>
        [RequireModulePermission("FINANCE-3")]
        public static AccountsPayableTDS LoadAPPayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            AccountsPayableTDS MainDs = new AccountsPayableTDS();
            TDBTransaction transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    AccountsPayableTDSAApPaymentRow supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)
                                                                          AApPaymentAccess.LoadByPrimaryKey(MainDs,
                        ALedgerNumber,
                        APaymentNumber,
                        transaction);

                    if (MainDs.AApPayment.Rows.Count > 0) // If I can load the referenced payment, I'll also load related documents.
                    {
                        AApDocumentPaymentAccess.LoadViaAApPayment(MainDs, ALedgerNumber, APaymentNumber, transaction);

                        // There may be a batch of several invoices in this payment,
                        // but they must be to the same supplier, and in the same currency!
                        Int64 PartnerKey = 0;
                        AApDocumentRow DocumentRow = null;

                        foreach (AccountsPayableTDSAApDocumentPaymentRow Row in MainDs.AApDocumentPayment.Rows)
                        {
                            DocumentRow =
                                AApDocumentAccess.LoadByPrimaryKey(MainDs, Row.ApDocumentId, transaction);

                            PartnerKey = DocumentRow.PartnerKey;
                            Row.InvoiceTotal = DocumentRow.TotalAmount;
                            Row.PayFullInvoice = (MainDs.AApDocumentPayment[0].Amount == DocumentRow.TotalAmount);
                            Row.DocumentCode = DocumentRow.DocumentCode;
                            Row.DocType = (DocumentRow.CreditNoteFlag ? "CREDIT" : "INVOICE");

                            AApDocumentDetailAccess.LoadViaAApDocument(MainDs, Row.ApDocumentId, transaction);

                            // Then I also need to get any referenced AnalAttrib records
                            MainDs.AApDocumentDetail.DefaultView.RowFilter = String.Format("{0}={1}",
                                AApDocumentDetailTable.GetApDocumentIdDBName(), Row.ApDocumentId);

                            foreach (DataRowView rv in MainDs.AApDocumentDetail.DefaultView)
                            {
                                AApDocumentDetailRow DetailRow = (AApDocumentDetailRow)rv.Row;
                                AApAnalAttribAccess.LoadViaAApDocumentDetail(MainDs, Row.ApDocumentId, DetailRow.DetailNumber, transaction);
                            }
                        }

                        PPartnerRow PartnerRow =
                            PPartnerAccess.LoadByPrimaryKey(MainDs, PartnerKey, transaction);
                        supplierPaymentsRow.SupplierKey = PartnerKey;
                        supplierPaymentsRow.SupplierName = PartnerRow.PartnerShortName;
                        supplierPaymentsRow.CurrencyCode = DocumentRow.CurrencyCode;
                        supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";
                        PPartnerLocationAccess.LoadViaPPartner(MainDs, PartnerKey, transaction);
                        PLocationAccess.LoadViaPPartner(MainDs, PartnerKey, transaction);
                        AApSupplierAccess.LoadByPrimaryKey(MainDs, PartnerKey, transaction);
                    }
                }); // Get NewOrExisting AutoReadTransaction

            return MainDs;
        } // Load AP Payment

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        /// <returns>true if a matching payment reversal can be found</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool WasThisPaymentReversed(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            // TODO: Look for a matching reverse payment
            return false;
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
        public static bool ReversePayment(Int32 ALedgerNumber,
            Int32 APaymentNumber,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            //
            // I need to create new documents and post them.
            TVerificationResultCollection Verifications = new TVerificationResultCollection();

            AVerifications = Verifications;

            TDBTransaction ReversalTransaction = null;
            Boolean SubmissionOK = false;
            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref ReversalTransaction, ref SubmissionOK,
                delegate
                {
                    // First, a squeaky clean TDS, and also one with the existing payment:
                    AccountsPayableTDS ReverseDS = new AccountsPayableTDS();
                    AccountsPayableTDS TempDS = LoadAPPayment(ALedgerNumber, APaymentNumber);

                    Int32 NewApNum = -1;
                    Boolean MustBeApproved = LedgerRquiresDocumentApproval(ALedgerNumber, ReversalTransaction);

                    List <Int32>PostTheseDocs = new List <Int32>();

                    //
                    // Now produce a reversed copy of each referenced document
                    //
                    TempDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApDocumentIdDBName();
                    TempDS.AApPayment.DefaultView.Sort = AApPaymentTable.GetPaymentNumberDBName();

                    foreach (AApDocumentPaymentRow DocPaymentRow in TempDS.AApDocumentPayment.Rows)
                    {
                        Int32 DocIdx = TempDS.AApDocument.DefaultView.Find(DocPaymentRow.ApDocumentId);
                        AApDocumentRow OldDocumentRow = TempDS.AApDocument[DocIdx];
                        AccountsPayableTDSAApDocumentRow NewDocumentRow = ReverseDS.AApDocument.NewRowTyped();
                        DocIdx = TempDS.AApPayment.DefaultView.Find(DocPaymentRow.PaymentNumber);
                        AApPaymentRow OldPaymentRow = TempDS.AApPayment[DocIdx];
                        DataUtilities.CopyAllColumnValues(OldDocumentRow, NewDocumentRow);
                        NewDocumentRow.ApDocumentId = (Int32)TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_ap_document);

                        PostTheseDocs.Add(NewDocumentRow.ApDocumentId);

                        NewDocumentRow.CreditNoteFlag = !OldDocumentRow.CreditNoteFlag; // Here's the actual reversal!
                        NewDocumentRow.DocumentCode = "Reversal " + OldDocumentRow.DocumentCode;
                        NewDocumentRow.Reference = "Reversal " + OldDocumentRow.Reference;
                        NewDocumentRow.DocumentStatus = MustBeApproved ? MFinanceConstants.AP_DOCUMENT_APPROVED : MFinanceConstants.AP_DOCUMENT_OPEN;

                        NewDocumentRow.DateCreated = DateTime.Now;
                        NewDocumentRow.DateEntered = DateTime.Now;
                        NewDocumentRow.ApNumber = NextApDocumentNumber(ALedgerNumber, ReversalTransaction);
                        NewDocumentRow.ExchangeRateToBase = OldDocumentRow.ExchangeRateToBase;
                        NewDocumentRow.SavedExchangeRate = OldPaymentRow.ExchangeRateToBase;
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
                    if (SaveAApDocument(ref ReverseDS, out Verifications) != TSubmitChangesResult.scrOK)
                    {
                        return;
                    }

                    //
                    // Now I can post these new documents, and pay them:
                    //

                    foreach (AccountsPayableTDSAApDocumentRow DocumentRow in ReverseDS.AApDocument.Rows)
                    {
                        //
                        // For foreign invoices,
                        // I need to ensure that the reverse payment uses the exchange rate that was used
                        // when the original document was paid.
                        //
                        Decimal PaymentExchangeRate = DocumentRow.SavedExchangeRate;
                        DocumentRow.SavedExchangeRate = DocumentRow.ExchangeRateToBase;
                        DocumentRow.ExchangeRateToBase = PaymentExchangeRate;
                    }

                    if (!PostAPDocuments(
                            ALedgerNumber,
                            PostTheseDocs,
                            APostingDate,
                            false,
                            out Verifications))
                    {
                        return;
                    }

                    CreatePaymentTableEntries(ref ReverseDS, ALedgerNumber, PostTheseDocs);
                    //              AccountsPayableTDSAApPaymentTable AApPayment = ReverseDS.AApPayment;
                    //              AccountsPayableTDSAApDocumentPaymentTable AApDocumentPayment = ReverseDS.AApDocumentPayment;

                    //
                    // For foreign invoices,
                    // I need to ensure that the invoice shows the exchange rate that was used
                    // when the original document was posted.
                    //

                    foreach (AccountsPayableTDSAApDocumentRow DocumentRow in ReverseDS.AApDocument.Rows)
                    {
                        //
                        // I'll restore the exchange rates I save above...
                        DocumentRow.ExchangeRateToBase = DocumentRow.SavedExchangeRate; // If this exchange rate is different to the one
                        // used in the payment, a "Forex Reval" transaction will be
                        // created to balance the books.
                    }

                    if (!PostAPPayments(
                            ref ReverseDS,
                            APostingDate,
                            out Verifications))
                    {
                        return;
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
                        NewDocumentRow.DocumentStatus = MustBeApproved ? MFinanceConstants.AP_DOCUMENT_APPROVED : MFinanceConstants.AP_DOCUMENT_OPEN;
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

                    if (SaveAApDocument(ref CreateDs, out Verifications) != TSubmitChangesResult.scrOK)
                    {
                        return;
                    }

                    //
                    // The process of saving those new documents should have given them all shiny new ApNumbers,
                    // So finally I need to make a list of those Document numbers, and post them.
                    PostTheseDocs.Clear();

                    foreach (AApDocumentRow DocumentRow in CreateDs.AApDocument.Rows)
                    {
                        PostTheseDocs.Add(DocumentRow.ApDocumentId);
                    }

                    if (!PostAPDocuments(ALedgerNumber, PostTheseDocs, APostingDate, false, out Verifications))
                    {
                        return;
                    }

                    SubmissionOK = true;
                }); // Begin Auto Transaction
            return SubmissionOK;
        }

        #region Data Validation

        static partial void ValidateApDocumentDetail(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateApDocumentDetailManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}
