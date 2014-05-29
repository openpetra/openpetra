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
            ALedgerTable Tbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, null);

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

            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            AApSupplierAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
            PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

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

            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            AccountsPayableTDSAApDocumentRow DocumentRow = (AccountsPayableTDSAApDocumentRow)
                                                           AApDocumentAccess.LoadByPrimaryKey(MainDS, AApDocumentId, Transaction);

            // If the load didn't work, don't bother with anything else..
            if (MainDS.AApDocument.Count > 0)
            {
                SetOutstandingAmount(DocumentRow, ALedgerNumber, MainDS.AApDocumentPayment);

                AApDocumentDetailAccess.LoadViaAApDocument(MainDS, AApDocumentId, Transaction);
                AApSupplierAccess.LoadByPrimaryKey(MainDS, DocumentRow.PartnerKey, Transaction);

                AApAnalAttribAccess.LoadViaAApDocument(MainDS, AApDocumentId, Transaction);

                // Accept row changes here so that the Client gets 'unmodified' rows
                MainDS.AcceptChanges();

                // I also need a full list of analysis attributes that could apply to this document
                // (although if it's already been posted I don't need to get this...)

                LoadAnalysisAttributes(MainDS, ALedgerNumber, Transaction);
            }

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

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

            bool IsMyOwnTransaction = false; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = null;
            try
            {
                Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                  (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

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
            } // try
            finally
            {
                if ((Transaction != null) && IsMyOwnTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return MainDS;
        }

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
            bool NewTransaction;

            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS == null)
            {
                return TSubmitChangesResult.scrNothingToBeSaved;
            }

            if ((AInspectDS.AApDocument != null) && (AInspectDS.AApDocument.Rows.Count > 0))
            {
                // I want to check that the Invoice numbers are not blank,
                // and that none of the documents already exist in the database.

                foreach (AApDocumentRow NewDocRow in AInspectDS.AApDocument.Rows)
                {
                    if (NewDocRow.DocumentCode.Length == 0)
                    {
                        AVerificationResult.Add(new TVerificationResult("Save Document", "The Document has no Document number.",
                                TResultSeverity.Resv_Noncritical));
                        return TSubmitChangesResult.scrInfoNeeded;
                    }

                    AApDocumentRow DocTemplateRow = AInspectDS.AApDocument.NewRowTyped(false);
                    DocTemplateRow.LedgerNumber = NewDocRow.LedgerNumber;
                    DocTemplateRow.PartnerKey = NewDocRow.PartnerKey;
                    DocTemplateRow.DocumentCode = NewDocRow.DocumentCode;
                    AApDocumentTable MatchingRecords = AApDocumentAccess.LoadUsingTemplate(DocTemplateRow, null);

                    foreach (AApDocumentRow MatchingRow in MatchingRecords.Rows) // Generally I expect this table is empty..
                    {
                        if (MatchingRow.ApDocumentId != NewDocRow.ApDocumentId) // This Document Code is in use, and not by me!
                        {
                            AVerificationResult.Add(new TVerificationResult("Save Document",
                                    String.Format("Document Code {0} already exists.", NewDocRow.DocumentCode),
                                    TResultSeverity.Resv_Noncritical));
                            return TSubmitChangesResult.scrInfoNeeded;
                        }
                    }
                } // foreach (document)

            } // if {there's actually a document}

            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                out NewTransaction);

            try
            {
                if (AInspectDS.AApDocument != null)
                {
                    foreach (AccountsPayableTDSAApDocumentRow NewDocRow in AInspectDS.AApDocument.Rows)
                    {
                        // Set AP Number if it has not been set yet.
                        if (NewDocRow.ApNumber < 0)
                        {
                            NewDocRow.ApNumber = NextApDocumentNumber(NewDocRow.LedgerNumber, SubmitChangesTransaction);
                        }

                        SetOutstandingAmount(NewDocRow, NewDocRow.LedgerNumber, AInspectDS.AApDocumentPayment);
                    }

                    AApDocumentAccess.SubmitChanges(AInspectDS.AApDocument, SubmitChangesTransaction);
                }

                if (AInspectDS.AApDocumentDetail != null) // Document detail lines
                {
                    ValidateApDocumentDetail(ref AVerificationResult, AInspectDS.AApDocumentDetail);
                    ValidateApDocumentDetailManual(ref AVerificationResult, AInspectDS.AApDocumentDetail);

                    if (TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
                    {
                        AApDocumentDetailAccess.SubmitChanges(AInspectDS.AApDocumentDetail, SubmitChangesTransaction);
                    }
                }

                if (AInspectDS.AApAnalAttrib != null) // Analysis attributes
                {
                    AApAnalAttribAccess.SubmitChanges(AInspectDS.AApAnalAttrib, SubmitChangesTransaction);
                }

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured while saving an AP Document:" + Environment.NewLine + Exc.ToString());

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                if (AVerificationResult == null)
                {
                    AVerificationResult = new TVerificationResultCollection();
                }

                AVerificationResult.Add(new TVerificationResult("Save AP Document", Exc.Message,
                        TResultSeverity.Resv_Critical));

                throw;
            }

            if ((AVerificationResult != null) && (AVerificationResult.Count > 0))
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return TSubmitChangesResult.scrOK;
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

        /* Method is not used. (Compiles OK, but no-one seems to want it!)
         *
         * /// <summary>
         * ///
         * /// </summary>
         * /// <param name="ALedgerNumber"></param>
         * /// <param name="ASupplierKey"></param>
         * /// <param name="ADocumentStatus"></param>
         * /// <param name="IsCreditNoteNotInvoice"></param>
         * /// <param name="AHideAgedTransactions"></param>
         * /// <returns></returns>
         * [RequireModulePermission("FINANCE-1")]
         * public static AccountsPayableTDS FindAApDocument(Int32 ALedgerNumber, Int64 ASupplierKey,
         *  string ADocumentStatus,
         *  bool IsCreditNoteNotInvoice,
         *  bool AHideAgedTransactions)
         * {
         *  // create the DataSet that will later be passed to the Client
         *  AccountsPayableTDS MainDS = new AccountsPayableTDS();
         *
         *  AApSupplierAccess.LoadByPrimaryKey(MainDS, ASupplierKey, null);
         *
         *  // TODO: filters for document status
         *  AccountsPayableTDSAApDocumentRow DocumentTemplate = MainDS.AApDocument.NewRowTyped(false);
         *  DocumentTemplate.LedgerNumber = ALedgerNumber;
         *  DocumentTemplate.PartnerKey = ASupplierKey;
         *  AApDocumentAccess.LoadUsingTemplate(MainDS, DocumentTemplate, null);
         *
         *  foreach (AccountsPayableTDSAApDocumentRow Row in MainDS.AApDocument.Rows)
         *  {
         *      SetOutstandingAmount(Row, ALedgerNumber, MainDS.AApDocumentPayment);
         *  }
         *
         *  return MainDS;
         * }
         */

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
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAPDocumentIds"></param>
        /// <param name="APostingDate"></param>
        /// <param name="Reversal"></param>
        /// <param name="AVerifications"></param>
        /// <returns> The TDS for posting</returns>
        private static AccountsPayableTDS LoadDocumentsAndCheck(Int32 ALedgerNumber,
            List <Int32>AAPDocumentIds,
            DateTime APostingDate,
            Boolean Reversal,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AVerifications = new TVerificationResultCollection();
            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction Transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                             (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);

            // collect the AP documents from the database
            foreach (Int32 APDocumentId in AAPDocumentIds)
            {
                AApDocumentAccess.LoadByPrimaryKey(MainDS, APDocumentId, Transaction);
                AApDocumentDetailAccess.LoadViaAApDocument(MainDS, APDocumentId, Transaction);
            }

            // do some checks on state of AP documents
            foreach (AApDocumentRow document in MainDS.AApDocument.Rows)
            {
                if (Reversal)
                {
                    if (document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_POSTED)
                    {
                        AVerifications.Add(new TVerificationResult(
                                Catalog.GetString("Error during reversal of posted AP document"),
                                String.Format(Catalog.GetString("Document Number {0} cannot be reversed since the status is {1}."),
                                    document.ApNumber, document.DocumentStatus), TResultSeverity.Resv_Critical));
                    }
                }
                else
                {
                    if (document.DocumentStatus != MFinanceConstants.AP_DOCUMENT_APPROVED)
                    {
                        AVerifications.Add(new TVerificationResult(
                                Catalog.GetString("Error during posting of AP document"),
                                String.Format(Catalog.GetString("Document Number {0} cannot be posted since the status is {1}."),
                                    document.ApNumber, document.DocumentStatus), TResultSeverity.Resv_Critical));
                    }
                }

                // TODO: also check if details are filled, and they each have a costcentre and account?

                // TODO: check for document.apaccount, if not set, get the default apaccount from the supplier, and save the ap document

                // Check that the amount of the document equals the totals of details
                if (!DocumentBalanceOK(MainDS, document.ApDocumentId, Transaction))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), document.ApNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The value does not match the sum of the details.")),
                            TResultSeverity.Resv_Critical));
                }

                // Load Analysis Attributes and check they're all present.
                if (!AttributesAllOK(MainDS, ALedgerNumber, document.ApDocumentId, Transaction))
                {
                    AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), document.ApNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("Analysis Attributes are required.")),
                            TResultSeverity.Resv_Critical));
                }
            }  //foreach

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

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }

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

            foreach (String AccCostCentre in AccountCodesCostCentres)
            {
                Int32 BarPos = AccCostCentre.IndexOf("|");
                String AccountCode = AccCostCentre.Substring(0, BarPos);
                AAccountTable AccountTbl = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, AccountCode, null);
                String ValidCcCombo = AccountTbl[0].ValidCcCombo.ToLower();

                // If this account goes with any cost centre (as is likely),
                // there's nothing more to do.

                if (ValidCcCombo != "all")
                {
                    String CostCentre = AccCostCentre.Substring(BarPos + 1);
                    ACostCentreTable CcTbl = ACostCentreAccess.LoadByPrimaryKey(ALedgerNumber, CostCentre, null);
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
                AVerificationResult.Add(new TVerificationResult("Approve AP Document", "Nothing to do - the document list is empty",
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
                    AVerificationResult.Add(new TVerificationResult("Approve AP Document", "Only OPEN documents can be approved",
                            TResultSeverity.Resv_Noncritical));
                    return false;
                }
            }

            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                AApDocumentAccess.SubmitChanges(TempDS.AApDocument, SubmitChangesTransaction);
                DBAccess.GDBAccessObj.CommitTransaction();
                ResultValue = true;
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the approval of AP Documents:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                AVerificationResult.Add(new TVerificationResult("Approve AP Documents", Exc.Message, TResultSeverity.Resv_Critical));

                throw;
            }

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

            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                AApAnalAttribAccess.SubmitChanges(TempDS.AApAnalAttrib, SubmitChangesTransaction);

                AApDocumentDetailAccess.SubmitChanges(TempDS.AApDocumentDetail, SubmitChangesTransaction);

                AApDocumentAccess.SubmitChanges(TempDS.AApDocument, SubmitChangesTransaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception Exc)
            {
                TLogging.Log("An Exception occured during the deletion of AP Documents:" + Environment.NewLine + Exc.ToString());

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw;
            }
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
            AVerificationResult = ResultsCollection;    // The System.Action defined in the delegate below cannot directly access 
                                                        // "out" parameters, so this intermediate variable is used.
            TDBTransaction HighLevelTransaction = null;

            DBAccess.GDBAccessObj.BeginAutoTransaction(ref HighLevelTransaction, true,
                delegate
                {

                    AccountsPayableTDS MainDS = LoadDocumentsAndCheck(ALedgerNumber, AAPDocumentIds, APostingDate, Reversal, out ResultsCollection);

                    if (!TVerificationHelper.IsNullOrOnlyNonCritical(ResultsCollection))
                    {
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
                            row.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
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

                        ResultsCollection.Add(new TVerificationResult("Post AP Document", 
                            "NOTE THAT A GL ENTRY MAY HAVE BEEN CREATED." + Environment.NewLine
                            + Exc.Message,
                            TResultSeverity.Resv_Critical));
                        PostingWorkedOk = false;
                        return;
                    }
                });
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
                APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApDocumentIdDBName() + " = " +
                                                              ((AApDocumentPaymentRow)APDataset.AApDocumentPayment.DefaultView[0].Row).ApDocumentId.
                                                              ToString();
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
        /// <param name="MainDS"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>true if it seemed to work OK</returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPPayments(
            ref AccountsPayableTDS MainDS,
            DateTime APostingDate,
            out TVerificationResultCollection AVerificationResult)
        {
            bool NewTransaction;
            TDBTransaction ReadTransaction;

            AVerificationResult = new TVerificationResultCollection();
            bool ResultValue = false;

            if ((MainDS.AApPayment.Rows.Count < 1) || (MainDS.AApDocumentPayment.Rows.Count < 1))
            {
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult("Post Payment",
                        String.Format("Nothing to do - Payments has {0} rows, Documents has {1} rows.",
                            MainDS.AApPayment.Rows.Count, MainDS.AApDocumentPayment.Rows.Count), TResultSeverity.Resv_Noncritical));
                return false;
            }

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            foreach (AccountsPayableTDSAApDocumentPaymentRow row in MainDS.AApDocumentPayment.Rows)
            {
                AccountsPayableTDSAApDocumentRow documentRow = (AccountsPayableTDSAApDocumentRow)MainDS.AApDocument.Rows.Find(row.ApDocumentId);

                if (documentRow != null)
                {
                    MainDS.AApDocument.Rows.Remove(documentRow);
                }

                documentRow = (AccountsPayableTDSAApDocumentRow)
                              AApDocumentAccess.LoadByPrimaryKey(MainDS, row.ApDocumentId, ReadTransaction);

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
                ReadTransaction);
            Int32 maxPaymentNumberInLedger = (maxPaymentCanBeNull == System.DBNull.Value ? 0 : Convert.ToInt32(maxPaymentCanBeNull));

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

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
                Boolean PostingWorkedOk = (TGLTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                                               out AVerificationResult) == TSubmitChangesResult.scrOK);

                if (PostingWorkedOk)
                {
                    // post the batch
                    PostingWorkedOk = TGLPosting.PostGLBatch(MainDS.AApPayment[0].LedgerNumber, batch.BatchNumber,
                        out AVerificationResult);
                }

                if (!PostingWorkedOk)
                {
                    TVerificationResultCollection MoreResults;

                    TGLPosting.DeleteGLBatch(
                        MainDS.AApPayment[0].LedgerNumber,
                        batch.BatchNumber,
                        out MoreResults);
                    AVerificationResult.AddCollection(MoreResults);

                    return false;
                }

                SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                               (IsolationLevel.Serializable, TEnforceIsolationLevel.eilMinimum, out NewTransaction);

                // store ApPayment and ApDocumentPayment to database
                AApPaymentAccess.SubmitChanges(MainDS.AApPayment, SubmitChangesTransaction);
                AApDocumentPaymentAccess.SubmitChanges(MainDS.AApDocumentPayment, SubmitChangesTransaction);

                // save changed status of AP documents to database
                AApDocumentAccess.SubmitChanges(MainDS.AApDocument, SubmitChangesTransaction);

                ResultValue = true;
            }
            catch (Exception e)
            {
                // Now I've got payment entries in the GL, and "unposted" payment records.

                TLogging.Log("Posting AP payments: exception " + e.Message);

                if ((SubmitChangesTransaction != null) && NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                AVerificationResult.Add(new TVerificationResult("Post AP Payment",
                        e.Message, TResultSeverity.Resv_Critical));

                throw;
            }

            if ((SubmitChangesTransaction != null) && NewTransaction)
            {
                if (ResultValue)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return ResultValue;
        }

        /// <summary>
        /// Load this payment, together with the supplier and all the related documents.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APaymentNumber"></param>
        /// <returns>Fully loaded TDS</returns>
        [RequireModulePermission("FINANCE-3")]
        public static AccountsPayableTDS LoadAPPayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            bool IsMyOwnTransaction; // If I create a transaction here, then I need to rollback when I'm done.
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction
                                                 (IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum, out IsMyOwnTransaction);
            AccountsPayableTDS MainDs = new AccountsPayableTDS();

            AccountsPayableTDSAApPaymentRow supplierPaymentsRow = (AccountsPayableTDSAApPaymentRow)
                                                                  AApPaymentAccess.LoadByPrimaryKey(MainDs,
                ALedgerNumber,
                APaymentNumber,
                ReadTransaction);

            if (MainDs.AApPayment.Rows.Count > 0) // If I can load the referenced payment, I'll also load related documents.
            {
                AApDocumentPaymentAccess.LoadViaAApPayment(MainDs, ALedgerNumber, APaymentNumber, ReadTransaction);

                // There may be a batch of several invoices in this payment,
                // but they must be to the same supplier, and in the same currency!
                Int64 PartnerKey = 0;
                AApDocumentRow DocumentRow = null;

                foreach (AccountsPayableTDSAApDocumentPaymentRow Row in MainDs.AApDocumentPayment.Rows)
                {
                    DocumentRow =
                        AApDocumentAccess.LoadByPrimaryKey(MainDs, Row.ApDocumentId, ReadTransaction);

                    PartnerKey = DocumentRow.PartnerKey;
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

                PPartnerRow PartnerRow =
                    PPartnerAccess.LoadByPrimaryKey(MainDs, PartnerKey, ReadTransaction);
                supplierPaymentsRow.SupplierKey = PartnerKey;
                supplierPaymentsRow.SupplierName = PartnerRow.PartnerShortName;
                supplierPaymentsRow.CurrencyCode = DocumentRow.CurrencyCode;
                supplierPaymentsRow.ListLabel = supplierPaymentsRow.SupplierName + " (" + supplierPaymentsRow.MethodOfPayment + ")";
                PPartnerLocationAccess.LoadViaPPartner(MainDs, PartnerKey, ReadTransaction);
                PLocationAccess.LoadViaPPartner(MainDs, PartnerKey, ReadTransaction);
                AApSupplierAccess.LoadByPrimaryKey(MainDs, PartnerKey, ReadTransaction);
            }

            if (IsMyOwnTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDs;
        }

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

            // First, a squeaky clean TDS, and also one with the existing payment:
            AccountsPayableTDS ReverseDS = new AccountsPayableTDS();
            AccountsPayableTDS TempDS = LoadAPPayment(ALedgerNumber, APaymentNumber);

            Int32 NewApNum = -1;

            AVerifications = new TVerificationResultCollection();

            // This transaction encloses the entire operation.
            // I can call lower-level functions, so long as they use
            // GetNewOrExistingTransaction.

            TDBTransaction ReversalTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            List <Int32>PostTheseDocs = new List <Int32>();
            try
            {
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
                    NewDocumentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;

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
                if (SaveAApDocument(ref ReverseDS, out AVerifications) != TSubmitChangesResult.scrOK)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
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
                        out AVerifications))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
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
                        out AVerifications))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
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
                    DBAccess.GDBAccessObj.RollbackTransaction();
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

                if (!PostAPDocuments(ALedgerNumber, PostTheseDocs, APostingDate, false, out AVerifications))
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }

                DBAccess.GDBAccessObj.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                DBAccess.GDBAccessObj.RollbackTransaction(); // throw away all that...
                AVerifications = new TVerificationResultCollection();
                TLogging.Log("In ReversePayment: exception " + e.Message);
                TLogging.Log(e.StackTrace);

                TVerificationResult Res = new TVerificationResult("Exception", e.Message + "\r\n" + e.StackTrace, TResultSeverity.Resv_Critical);
                AVerifications.Add(Res);
                return false;
            }
        }

        #region Data Validation

        static partial void ValidateApDocumentDetail(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);
        static partial void ValidateApDocumentDetailManual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);

        #endregion Data Validation
    }
}