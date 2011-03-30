//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.GL
{
	
	/// <summary>
	/// This Tool creates a batch enables to add a journal and to add transactions to a yournal
	/// All internal "pointers" and control data are set internal and the structure is "read to post".
	/// </summary>
    public partial class CommonAccountingTool
    {
    	private int intLedgerNumber;
    	private int intAccountingPeriod;
        private GLBatchTDS aBatchTable = null;
        private ABatchRow aBatchRow;
        private AJournalRow journal;
	
        private int intJournalCount;
        

        /// <summary>
        /// The constructor creates a base batch and defines the batch parameters. There is only
        /// one batch to account. Use a new object to post another batch.
        /// </summary>
        /// <param name="ALedgerNumber">the ledger number</param>
        /// <param name="AAccountingPeriod">the accounting period</param>
        /// <param name="ABatchDescription">a batch description text</param>
        /// <param name="ABatchDateEffective">and the "effective" date of batch and 
        /// corresponding journals</param>
    	public CommonAccountingTool(int ALedgerNumber, 
                                    int AAccountingPeriod,
                                    string ABatchDescription,
                                    DateTime ABatchDateEffective)
    	{
    		intAccountingPeriod = AAccountingPeriod;
    		intLedgerNumber = ALedgerNumber;
    		aBatchTable = TTransactionWebConnector.CreateABatch(intLedgerNumber);
    		aBatchRow = aBatchTable.ABatch[0];
            aBatchRow.BatchDescription = ABatchDescription;
            aBatchRow.DateEffective = ABatchDateEffective;
            aBatchRow.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;
            intJournalCount = 0;
    	}
    	

        /// <summary>
        /// A journal is added to a batch. This routine can be called multiple times. 
        /// </summary>
        /// <param name="ACurrencyCode"></param>
        /// <param name="AAccountingDate"></param>
        /// <param name="ATransactionTypeCode"></param>
        /// <param name="ASubSystemCode"></param>
        /// <param name="AExchangeRateToBase"></param>
        public void AddJournalInForeignCurrency(string ACurrencyCode,
                                DateTime AAccountingDate,
                                string ATransactionTypeCode, 
                                string ASubSystemCode, 
                                decimal AExchangeRateToBase)
        {
        	AddAJournal(ACurrencyCode, AAccountingDate, ATransactionTypeCode, 
        	            ASubSystemCode, AExchangeRateToBase);
        }
        public void AddJournalInBaseCurrency(string ACurrencyCode,
                                DateTime AAccountingDate,
                                string ATransactionTypeCode, 
                                string ASubSystemCode)
        {
        	AddAJournal(ACurrencyCode, AAccountingDate, ATransactionTypeCode, 
        	            ASubSystemCode, 1.0m);
        }
        
        private void AddAJournal(string ACurrencyCode,
                                DateTime AAccountingDate,
                                string ATransactionTypeCode, 
                                string ASubSystemCode, 
                                decimal AExchangeRateToBase)
    	{
    		++intJournalCount;
    		journal = aBatchTable.AJournal.NewRowTyped();
            journal.LedgerNumber = aBatchRow.LedgerNumber;
            journal.BatchNumber = aBatchRow.BatchNumber;
            journal.JournalNumber = intJournalCount;
            journal.DateEffective = aBatchRow.DateEffective;
            journal.JournalPeriod = intAccountingPeriod;
            journal.TransactionCurrency = ACurrencyCode;
            journal.JournalDescription = aBatchRow.BatchDescription;
            journal.TransactionTypeCode = ATransactionTypeCode;
            journal.SubSystemCode = ASubSystemCode;
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = AAccountingDate;
            journal.ExchangeRateToBase = AExchangeRateToBase;
    		aBatchTable.AJournal.Rows.Add(journal);
    	}
    	
        /// <summary>
        /// A Transaction is added only to the last added batch. This routine can be called
        /// multiple times too even inside of one journal.
        /// </summary>
        /// <param name="AAccount"></param>
        /// <param name="ACostCenter"></param>
        /// <param name="ANarrativeMessage"></param>
        /// <param name="AReferenceMessage"></param>
        /// <param name="AIsDebit"></param>
        /// <param name="AAmountBaseCurrency"></param>
    	public void AddATransactionInBaseCurrency(string AAccount, 
                                    string ACostCenter,
                                    string ANarrativeMessage, 
                                    string AReferenceMessage, 
                                    bool AIsDebit,
                                    decimal AAmountBaseCurrency)
        {
        	AddATransaction(AAccount, ACostCenter, ANarrativeMessage, 
        	                AReferenceMessage, AIsDebit, AAmountBaseCurrency, 0);
        }
        
    	public void AddATransactionInForeignCurrency(string AAccount, 
                                    string ACostCenter,
                                    string ANarrativeMessage, 
                                    string AReferenceMessage, 
                                    bool AIsDebit,
                                    decimal AAmountForeignCurrency)
        {
        	AddATransaction(AAccount, ACostCenter, ANarrativeMessage, 
        	                AReferenceMessage, AIsDebit, 0, AAmountForeignCurrency);
        }
    	private void AddATransaction(string AAccount, 
                                    string ACostCenter,
                                    string ANarrativeMessage, 
                                    string AReferenceMessage, 
                                    bool AIsDebit,
                                    decimal AAmountBaseCurrency,
                                    decimal AAmountForeignCurrency)
    	{
            ATransactionRow transaction = null;

            transaction = aBatchTable.ATransaction.NewRowTyped();
            transaction.LedgerNumber = journal.LedgerNumber;
            transaction.BatchNumber = journal.BatchNumber;
            transaction.JournalNumber = journal.JournalNumber;
            transaction.TransactionNumber = ++journal.LastTransactionNumber;
            transaction.AccountCode = AAccount;
            transaction.CostCentreCode = ACostCenter;
            transaction.Narrative = ANarrativeMessage;
            transaction.Reference = AReferenceMessage;
            transaction.DebitCreditIndicator = AIsDebit;
            transaction.AmountInBaseCurrency = AAmountBaseCurrency;
            transaction.TransactionAmount = 2;
            transaction.TransactionDate = aBatchRow.DateEffective;
            aBatchTable.ATransaction.Rows.Add(transaction);
    	}
    		

        /// <summary>
        /// Here you can close save and post the batch, the included journal(s) and the 
        /// transaction(s). 
        /// </summary>
        /// <param name="AVerifications">A TVerificationResultCollection can defined to
        /// accept the error messages and warnings - if necessary.</param>
        /// <returns>The routine writes back the batch number and so you can access to the
        /// batch directly (if necessary)</returns>
    	public int CloseSaveAndPost(TVerificationResultCollection AVerifications)
    	{
    		return CloseSaveAndPost_(AVerifications);
    	}
    	
    	/// <summary>
    	/// The net-syntax checker reqires a clause "using Ict.Common.Verification;" in the routine which
    	/// calls CloseSaveAndPost(null). The only way to avoid this is the use of CloseSaveAndPost().
    	/// </summary>
    	/// <returns></returns>
    	public int CloseSaveAndPost()
    	{
    		return CloseSaveAndPost_(null);
    	}
    	
    	private int CloseSaveAndPost_(TVerificationResultCollection AVerifications)
    	{
    		bool blnReturnValue =
    			(TTransactionWebConnector.SaveGLBatchTDS(
    				ref aBatchTable, out AVerifications) == TSubmitChangesResult.scrOK);
    		blnReturnValue = (GL.WebConnectors.TTransactionWebConnector.PostGLBatch(
    			aBatchRow.LedgerNumber, aBatchRow.BatchNumber, out AVerifications));    		 
    		int returnValue = aBatchRow.BatchNumber;
    		aBatchTable = null;
    		aBatchRow = null;
    		journal = null;
    		return returnValue;
       }
    }
}