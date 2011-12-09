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

namespace Ict.Petra.Server.MFinance.AP.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Accounts Payable screens
    ///</summary>
    public class TTransactionWebConnector
    {
        private static void LoadAnalysisAttributes (AccountsPayableTDS AMainDS, Int32 ALedgerNumber, TDBTransaction ATransaction)
        
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
        /// Passes data as a Typed DataSet to the Transaction Edit Screen
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS LoadAApDocument(Int32 ALedgerNumber, Int32 AAPNumber)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AApDocumentAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, AAPNumber, Transaction);
            AApDocumentDetailAccess.LoadViaAApDocument(MainDS, ALedgerNumber, AAPNumber, Transaction);
            AApSupplierAccess.LoadByPrimaryKey(MainDS, MainDS.AApDocument[0].PartnerKey, Transaction);
            
            // Load via template...
            {
	            AApAnalAttribRow TemplateRow = MainDS.AApAnalAttrib.NewRowTyped(false);
	            TemplateRow.LedgerNumber = ALedgerNumber;
	            TemplateRow.ApNumber = AAPNumber;
	            AApAnalAttribAccess.LoadUsingTemplate(MainDS, TemplateRow, Transaction);
            }

            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();
            
            // I also need a full list of analysis attributes that could apply to this document
            // (although if it's already been posted I don't need to get this...)
            
            LoadAnalysisAttributes (MainDS, ALedgerNumber, Transaction);

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

            NewDocumentRow.ApNumber = -1; // ap number will be set in SubmitChanges
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
            
            LoadAnalysisAttributes (MainDS, ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

        /// <summary>
        /// store the AP Document (and document details)
        ///
        /// All DataTables contained in the Typed DataSet are inspected for added,
        /// changed or deleted rows by submitting them to the DataStore.
        ///
        /// </summary>
        /// <param name="AInspectDS">Typed DataSet that needs to contain known DataTables</param>
        /// <param name="AVerificationResult">Null if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions)</param>
        /// <returns>true if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed
        /// </returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveAApDocument(ref AccountsPayableTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            AVerificationResult = null;

            if (AInspectDS != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    // set AP Number if it has not been set yet
                    if ((AInspectDS.AApDocument != null) && (AInspectDS.AApDocument[0].ApNumber == -1))
                    {
                        StringCollection fieldlist = new StringCollection();
                        ALedgerTable myLedgerTable;
                        fieldlist.Add(ALedgerTable.GetLastApInvNumberDBName());
                        myLedgerTable = ALedgerAccess.LoadByPrimaryKey(
                            AInspectDS.AApDocument[0].LedgerNumber,
                            fieldlist,
                            SubmitChangesTransaction);
                        myLedgerTable[0].LastApInvNumber++;
                        AInspectDS.AApDocument[0].ApNumber = myLedgerTable[0].LastApInvNumber;

                        if (AInspectDS.AApDocumentDetail != null)
                        {
                            foreach (AApDocumentDetailRow detailrow in AInspectDS.AApDocumentDetail.Rows)
                            {
                                detailrow.ApNumber = AInspectDS.AApDocument[0].ApNumber;
                            }
                        }

                        ALedgerAccess.SubmitChanges(myLedgerTable, SubmitChangesTransaction, out AVerificationResult);
                    }

                    if ((AInspectDS.AApDocument == null) || AApDocumentAccess.SubmitChanges(AInspectDS.AApDocument, SubmitChangesTransaction,
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
        /// <param name="AApNumber"></param>
        /// <param name="AApSupplier_DefaultExpAccount"></param>
        /// <param name="AApSupplier_DefaultCostCentre"></param>
        /// <param name="AAmount">the amount that is still missing from the total amount of the invoice</param>
        /// <param name="ALastDetailNumber">AApDocument.LastDetailNumber</param>
        /// <returns>the new AApDocumentDetail row</returns>
        [RequireModulePermission("FINANCE-1")]
        public static AccountsPayableTDS CreateAApDocumentDetail(Int32 ALedgerNumber,
            Int32 AApNumber,
            string AApSupplier_DefaultExpAccount,
            string AApSupplier_DefaultCostCentre,
            decimal AAmount,
            Int32 ALastDetailNumber)
        {
            // create the DataSet that will later be passed to the Client
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AApDocumentDetailRow NewRow = MainDS.AApDocumentDetail.NewRowTyped();

            NewRow.ApNumber = AApNumber;
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

            // TODO: filters for documents
            // TODO: filter by ledger number
            AApDocumentAccess.LoadViaAApSupplier(MainDS, ASupplierKey, null);

            return MainDS;
        }

        private static bool DocumentBalanceOK(AccountsPayableTDS AMainDS, Int32 ALedgerNumber, int AApNumber, TDBTransaction ATransaction)
        {
            AApDocumentAccess.LoadByPrimaryKey(AMainDS, ALedgerNumber, AApNumber, ATransaction);
            AApDocumentRow DocumentRow = AMainDS.AApDocument[0];
        	decimal DocumentBalance = DocumentRow.TotalAmount;
        	
        	AMainDS.AApDocumentDetail.DefaultView.RowFilter 
        	    = String.Format("{0}={1} AND {2}={3}",
        	       AApDocumentTable.GetLedgerNumberDBName(), ALedgerNumber,
        	       AApDocumentTable.GetApNumberDBName(),AApNumber);
        	foreach (DataRowView rv in AMainDS.AApDocumentDetail.DefaultView)
        	{
        	    AApDocumentDetailRow Row = (AApDocumentDetailRow)rv.Row;
        		DocumentBalance -= Row.Amount;
        	}
        	return (DocumentBalance == 0.0m);
        }
        
        /// <summary>
        /// Load the Analysis Attributes for this document
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AApNumber"></param>
        /// <param name="ATransaction"></param>
        /// <returns>true if all required attributes are present</returns>
        private static bool AttributesAllOK(AccountsPayableTDS AMainDS, Int32 ALedgerNumber, int AApNumber, TDBTransaction ATransaction)
        {
            AMainDS.AApDocumentDetail.DefaultView.RowFilter = 
                String.Format("{0}={1}", AApDocumentDetailTable.GetApNumberDBName(),AApNumber);
            
            LoadAnalysisAttributes(AMainDS, ALedgerNumber, ATransaction);
            
    		// Load the Analysis Attributes defined for this document, using a template...
            {
	            AApAnalAttribRow TemplateRow = AMainDS.AApAnalAttrib.NewRowTyped(false);
	            TemplateRow.LedgerNumber = ALedgerNumber;
	            TemplateRow.ApNumber = AApNumber;
	            AApAnalAttribAccess.LoadUsingTemplate(AMainDS, TemplateRow, ATransaction);
            }
            
            foreach (DataRowView rv in AMainDS.AApDocumentDetail.DefaultView)
            {
                AApDocumentDetailRow DetailRow = (AApDocumentDetailRow)rv.Row;
        		AMainDS.AAnalysisAttribute.DefaultView.RowFilter = 
        			String.Format("{0}={1}", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode); // Do I need Cost Centre in here too?
        		
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
        /// <param name="AAPDocumentNumbers"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns> The TDS for posting</returns>
        private static AccountsPayableTDS LoadDocumentsAndCheck(Int32 ALedgerNumber,
            List <Int32>AAPDocumentNumbers,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = new AccountsPayableTDS();

            AVerifications = new TVerificationResultCollection();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            // collect the AP documents from the database
            foreach (Int32 APDocumentNumber in AAPDocumentNumbers)
            {
                AApDocumentAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, APDocumentNumber, Transaction);
                AApDocumentDetailAccess.LoadViaAApDocument(MainDS, ALedgerNumber, APDocumentNumber, Transaction);
            }

            // do some checks on state of AP documents
            foreach (AApDocumentRow row in MainDS.AApDocument.Rows)
            {
                if (row.DocumentStatus != MFinanceConstants.AP_DOCUMENT_APPROVED)
                {
                    AVerifications.Add(new TVerificationResult(
                            Catalog.GetString("error during posting of AP document"),
                            String.Format(Catalog.GetString("Document with Number {0} cannot be posted since the status is {1}."),
                                row.ApNumber, row.DocumentStatus), TResultSeverity.Resv_Critical));
                }

                // TODO: also check if details are filled, and they each have a costcentre and account?

                // TODO: check for document.apaccount, if not set, get the default apaccount from the supplier, and save the ap document
                
                // Check that the amount of the document equals the totals of details
                if (!DocumentBalanceOK(MainDS, ALedgerNumber, row.ApNumber, Transaction))
                {
                	AVerifications.Add(new TVerificationResult(
                            String.Format(Catalog.GetString("Cannot post the AP document {0} in Ledger {1}"), row.ApNumber, ALedgerNumber),
                            String.Format(Catalog.GetString("The value does not match the sum of the details.")),
                            TResultSeverity.Resv_Critical));
                }
                // Load Analysis Attributes and check they're all present.
                if (!AttributesAllOK(MainDS, ALedgerNumber, row.ApNumber, Transaction))
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

                foreach (AApDocumentRow document in DocumentsByCurrency[CurrencyCode])
                {
                    ATransactionRow transaction = null;
                    DataView DocumentDetails = APDataset.AApDocumentDetail.DefaultView;
                    DocumentDetails.RowFilter = AApDocumentDetailTable.GetApNumberDBName() + " = " + document.ApNumber.ToString();

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
                        
                        APDataset.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1}",
                                      AApAnalAttribTable.GetDetailNumberDBName(), documentDetail.DetailNumber);
                        foreach (DataRowView rv in APDataset.AApAnalAttrib.DefaultView)
                        {
                            AApAnalAttribRow RowSource = (AApAnalAttribRow) rv.Row;
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

                    // TODO: support foreign currencies
                    transaction.AmountInBaseCurrency = document.TotalAmount;

                    if (document.CreditNoteFlag)
                    {
                        transaction.AmountInBaseCurrency *= -1;
                    }

                    transaction.DebitCreditIndicator = (transaction.AmountInBaseCurrency > 0);

                    if (transaction.AmountInBaseCurrency < 0)
                    {
                        transaction.AmountInBaseCurrency *= -1;
                    }

                    transaction.DebitCreditIndicator = false;

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
        /// creates GL transactions for the selected AP documents,
        /// and posts those GL transactions,
        /// and marks the AP documents as Posted
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAPDocumentNumbers"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPDocuments(Int32 ALedgerNumber,
            List <Int32>AAPDocumentNumbers,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AccountsPayableTDS MainDS = LoadDocumentsAndCheck(ALedgerNumber, AAPDocumentNumbers, APostingDate, out AVerifications);

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
                APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApNumberDBName() + " = " +
                                                              ((AApDocumentPaymentRow)APDataset.AApDocumentPayment.DefaultView[0].Row).ApNumber.
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
                        APDataset.AApDocument.DefaultView.RowFilter = AApDocumentTable.GetApNumberDBName() + " = " +
                                                                      documentPayment.ApNumber.ToString();
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
                                                         documentPayment.ApNumber.ToString();
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

        /// <summary>
        /// store payments in the database, and post the payment to GL
        /// </summary>
        /// <param name="APayments"></param>
        /// <param name="ADocumentPayments"></param>
        /// <param name="APostingDate"></param>
        /// <param name="AVerifications"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool PostAPPayments(
            AccountsPayableTDSAApPaymentTable APayments,
            AccountsPayableTDSAApDocumentPaymentTable ADocumentPayments,
            DateTime APostingDate,
            out TVerificationResultCollection AVerifications)
        {
            AVerifications = null;
            bool ResultValue = false;

            AccountsPayableTDS MainDS = new AccountsPayableTDS();
            MainDS.AApPayment.Merge(APayments);
            MainDS.AApDocumentPayment.Merge(ADocumentPayments);

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            foreach (AccountsPayableTDSAApDocumentPaymentRow row in MainDS.AApDocumentPayment.Rows)
            {
                AApDocumentAccess.LoadByPrimaryKey(MainDS, row.LedgerNumber, row.ApNumber, ReadTransaction);

                // TODO: once partial payments are implemented, check for total amount
                // TODO: check that documents have not been paid already

                // TODO: modify the ap documents and mark as paid or partially paid
                MainDS.AApDocument.DefaultView.Sort = AApDocumentTable.GetApNumberDBName();
                AApDocumentRow documentRow = (AApDocumentRow)MainDS.AApDocument.DefaultView[
                    MainDS.AApDocument.DefaultView.Find(row.ApNumber)].Row;
                documentRow.DocumentStatus = MFinanceConstants.AP_DOCUMENT_PAID;
            }

            // get max payment number for this ledger
            // PROBLEM: what if two payments are happening at the same time? do we need locking?
            // see also http://sourceforge.net/apps/mantisbt/openpetraorg/view.php?id=50
            object maxPaymentCanBeNull = DBAccess.GDBAccessObj.ExecuteScalar(
                "SELECT MAX(PUB_a_ap_payment.a_payment_number_i) FROM PUB_a_ap_payment WHERE PUB_a_ap_payment.a_ledger_number_i = " +
                MainDS.AApDocumentPayment[0].LedgerNumber.ToString(),
                ReadTransaction);
            Int32 maxPaymentNumberInLedger = (maxPaymentCanBeNull == System.DBNull.Value ? 1 : Convert.ToInt32(maxPaymentCanBeNull));

            DBAccess.GDBAccessObj.CommitTransaction();

            foreach (AccountsPayableTDSAApPaymentRow paymentRow in MainDS.AApPayment.Rows)
            {
                paymentRow.PaymentDate = APostingDate;

                paymentRow.Amount = 0.0M;

                foreach (AccountsPayableTDSAApDocumentPaymentRow docPaymentRow in MainDS.AApDocumentPayment.Rows)
                {
                    if (docPaymentRow.PaymentNumber == paymentRow.PaymentNumber)
                    {
                        paymentRow.Amount += docPaymentRow.Amount;
                        docPaymentRow.PaymentNumber = maxPaymentNumberInLedger + (-1 * paymentRow.PaymentNumber);
                    }
                }

                paymentRow.PaymentNumber = maxPaymentNumberInLedger + (-1 * paymentRow.PaymentNumber);
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

                TLogging.Log("after submitchanges: exception " + e.Message);

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
    }
}