//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, christophert, timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Xml;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.MFinance.ICH
{
    /// <summary>
    /// Class for the performance of the Stewardship Calculation
    /// </summary>
    public class TGenFilesReports
    {
		/// <summary>
        /// Performs the ICH code to generate Stewardship Calculation.
        ///  Relates to gi3100.p
		/// </summary>
		/// <param name="ALedgerNumber"></param>
		/// <param name="APeriodNumber"></param>
		/// <param name="AICHNumber"></param>
		/// <param name="ACurrencyType"></param>
		/// <param name="AFileName"></param>
		/// <param name="AEmail"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns></returns>
    	public void GenerateStewardshipFile(int ALedgerNumber,
            int APeriodNumber,
            int AICHNumber,
            int ACurrencyType,
            string AFileName,
            bool AEmail,
            out TVerificationResultCollection AVerificationResult
            )
        {
			string CostCentre;
			decimal IncomeAmount = 0;
			decimal ExpenseAmount = 0;
			decimal XferAmount = 0;
			string Currency;

			string LedgerName;
			
            AVerificationResult = new TVerificationResultCollection();

            //Begin the transaction
            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {

				//Find the LedgerRow
	            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, DBTransaction);
	            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];
	
	            //Find the Partner Short Name
	            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(LedgerRow.PartnerKey, DBTransaction);
	            PPartnerRow PartnerRow = (PPartnerRow)PartnerTable.Rows[0];
	            
				LedgerName = PartnerRow.PartnerShortName;

				//Specify currency type
            	if (ACurrencyType == MFinanceConstants.CURRENCY_BASE_NUM)
				{
					Currency = LedgerRow.BaseCurrency;
				}				
				else
				{
					Currency = LedgerRow.IntlCurrency;
				}

						
				//Create table for conversion to XML and export to CSV
				AAccountingPeriodTable AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, APeriodNumber, null);
				AAccountingPeriodRow AccountingPeriodRow = (AAccountingPeriodRow)AccountingPeriodTable.Rows[0];
				
				DateTime PeriodEndDate = AccountingPeriodRow.PeriodEndDate;
				string StandardCostCentre = ALedgerNumber.ToString() + "00";
	            DateTime DateToday = DateTime.Today;
				
				//First four fields are constant for each row
				DataTable TableForExport = new DataTable();

	            TableForExport.Columns.Add("PeriodEndDate", typeof(DateTime));
	            TableForExport.Columns.Add("StandardCostCentre", typeof(string));
	            TableForExport.Columns.Add("DateToday", typeof(DateTime));
	            TableForExport.Columns.Add("Currency", typeof(string));
	            TableForExport.Columns.Add("CostCentre", typeof(string));
	            TableForExport.Columns.Add("Income", typeof(decimal));
	            TableForExport.Columns.Add("Expense", typeof(decimal));
	            TableForExport.Columns.Add("DirectTransfer", typeof(decimal));
				
				CostCentre = string.Empty;

				AIchStewardshipTable IchStewTable = new AIchStewardshipTable();
                AIchStewardshipRow TemplateRow = (AIchStewardshipRow)IchStewTable.NewRowTyped(false);

                TemplateRow.LedgerNumber = ALedgerNumber;
                TemplateRow.PeriodNumber = APeriodNumber;
                
                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });

                AIchStewardshipTable IchStewardshipTable = AIchStewardshipAccess.LoadUsingTemplate(TemplateRow, operators, null, DBTransaction);
                AIchStewardshipRow IchStewardshipRow = null;
                
                for (int i = 0; i < IchStewardshipTable.Count; i++)
                {
                	IchStewardshipRow = (AIchStewardshipRow)IchStewardshipTable.Rows[i];
                	
                	if (AICHNumber == 0
                	    || IchStewardshipRow.IchNumber == AICHNumber)
                	{
                		if (CostCentre != string.Empty
                		   && CostCentre != IchStewardshipRow.CostCentreCode)
                		{
                			if (IncomeAmount != 0 || ExpenseAmount != 0 || XferAmount != 0)
                			{
			                    DataRow DR = (DataRow)TableForExport.NewRow();

			                    DR.ItemArray[0] = PeriodEndDate;
			                    DR.ItemArray[1] = StandardCostCentre;
			                    DR.ItemArray[2] = DateToday;
			                    DR.ItemArray[3] = Currency;
			                    DR.ItemArray[4] = CostCentre;
			                    DR.ItemArray[5] = IncomeAmount;
			                    DR.ItemArray[6] = ExpenseAmount;
			                    DR.ItemArray[7] = XferAmount;
			
			                    TableForExport.Rows.Add(DR);
                			}
                			
                			IncomeAmount = 0;
			            	ExpenseAmount = 0;
			            	XferAmount = 0;
                			
                		}
                		
                		if (ACurrencyType == MFinanceConstants.CURRENCY_BASE_NUM)
                		{
        		            IncomeAmount += IchStewardshipRow.IncomeAmount;
				            ExpenseAmount += IchStewardshipRow.ExpenseAmount;
				            XferAmount += IchStewardshipRow.DirectXferAmount;
                		}
						else
						{
        		            IncomeAmount += IchStewardshipRow.IncomeAmountIntl;
				            ExpenseAmount += IchStewardshipRow.ExpenseAmountIntl;
				            XferAmount += IchStewardshipRow.DirectXferAmountIntl;
						}

					    CostCentre = IchStewardshipRow.CostCentreCode;
                	}
                }
                
                if (CostCentre != string.Empty && (IncomeAmount != 0 || ExpenseAmount != 0 || XferAmount != 0))
				{
                    DataRow DR = (DataRow)TableForExport.NewRow();

                    DR.ItemArray[0] = PeriodEndDate;
                    DR.ItemArray[1] = StandardCostCentre;
                    DR.ItemArray[2] = DateToday;
                    DR.ItemArray[3] = Currency;
                    DR.ItemArray[4] = CostCentre;
                    DR.ItemArray[5] = IncomeAmount;
                    DR.ItemArray[6] = ExpenseAmount;
                    DR.ItemArray[7] = XferAmount;

                    TableForExport.Rows.Add(DR);
				}

				//Create the XMLDoc ready for export to CSV
	            XmlDocument doc = TDataBase.DataTableToXml(TableForExport);
	            
	            TCsv2Xml.Xml2Csv(doc, AFileName);
	            
	            if (AEmail)
				{
	            	AEmailDestinationTable AEmailDestTable = new AEmailDestinationTable();
	            	AEmailDestinationRow TemplateRow2 = (AEmailDestinationRow)AEmailDestTable.NewRowTyped(false);
	            	
	                TemplateRow2.FileCode = MFinanceConstants.EMAIL_FILE_CODE_STEWARDSHIP;
                
	                StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=" });

                	AEmailDestinationTable AEmailDestinationTable = AEmailDestinationAccess.LoadUsingTemplate(TemplateRow2, operators2, null, DBTransaction);
                	AEmailDestinationRow EmailDestinationRow = (AEmailDestinationRow)AEmailDestinationTable.Rows[0];
            	
                	string SenderAddress = "";
                	string EmailAddress = EmailDestinationRow.EmailAddress;
                	string EmailSubject = "Stewardship File from " + LedgerName;
                	string HTMLText = string.Empty;
	                
					//Read the file title
                	FileInfo FileDetails = new FileInfo(AFileName);
					string FileTitle = FileDetails.Name;

                	if (!File.Exists(AFileName))
            		{
		                HTMLText = "<html><body>" + String.Format("Cannot find file {0}", AFileName) + "</body></html>";
            		}
		            else
    		        {
		                HTMLText = "<html><body>" + EmailSubject + ": " + FileTitle + " is attached.</body></html>";
    		        }

                	TSmtpSender SendMail = new TSmtpSender();
                	
       	            MailMessage msg = new MailMessage(SenderAddress,
										                EmailAddress,
										                EmailSubject,
										                HTMLText);
                	
                	msg.Attachments.Add(new Attachment(AFileName));
            		//msg.Bcc.Add(BCCAddress);

                	SendMail.SendMessage(ref msg);
				}

                DBAccess.GDBAccessObj.RollbackTransaction();

            }
            catch (Exception Exp)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();

                TLogging.Log(Exp.Message);
                TLogging.Log(Exp.StackTrace);

                throw Exp;
            }
        }


		/// <summary>
		/// Searches all transaction tables to see if there is a stewardship
        ///   batch that already exists which appears to match the one
        ///   currently being processed.
		/// </summary>
		/// <param name="AICHLedgerNumber">ICH Ledger number</param>
		/// <param name="ABatchRef">Reference of current stewardship batch</param>
		/// <param name="AYear">Year to which current stewardship batch applies</param>
		/// <param name="AMonth">Month to which current stewardship batch applies</param>
		/// <param name="ACurrentPeriod">Current period of ICH ledger</param>
		/// <param name="ACurrentYear">Current year of ICH ledger</param>
		/// <returns>The batch number of the matching batch or 0 if there is no match.</returns>
		public int FindMatchingStewardshipBatchInICH(int AICHLedgerNumber, string ABatchRef, int AYear, int AMonth, int ACurrentPeriod, int ACurrentYear)
		{
			int BatchNumber = 0;
			
			bool MatchFound = false;
			
			/*cOldStyleDescription = REPLACE(REPLACE(pcBatchDescription, STRING(piYear) + " ", ""), " (Run " + STRING(piRunNumber) + ")", "").*/

			/* Old reference format (prior to this program being available) was xxyy where xx was the fund number
			   and yy was the period number. Therefore we need to strip off the final 3 characters to convert
			   from the new style reference to the old style reference (those 3 characters are the run number).
			   We need to do this because some of the batches we will search through will be in old format. */
			string cOldStyleReference = ABatchRef.Substring(0, ABatchRef.Length - 3);
			int cOldStyleReferenceLen = cOldStyleReference.Length;
			
			/* Note: In the queries below we need to check the length of the reference as well because of the
		         3 digit fund numbers (eg. Central America Period 12 would have reference 2012 while NAA Period 2 would be
		         20112) */
			
			TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
			
			/* look in current period */
            ATransactionTable TransTable = new ATransactionTable();
			ATransactionRow TemplateRow = (ATransactionRow)TransTable.NewRowTyped(false);
			
			TemplateRow.LedgerNumber = AICHLedgerNumber;
			TemplateRow.TransactionStatus = true;
			
            StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });

            ATransactionTable TransactionTable = ATransactionAccess.LoadUsingTemplate(TemplateRow, operators, null, DBTransaction);
            ATransactionRow TransactionRow = null;
			
            string TransRef;
            int TransRefLen;
            
            for (int i = 0; i < TransactionTable.Count; i++)
            {
            	TransactionRow = (ATransactionRow)TransactionTable.Rows[i];
            	
            	TransRef = TransactionRow.Reference;
            	TransRefLen = TransRef.Length;
            	
            	if (TransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference
            	    && (TransRefLen == cOldStyleReferenceLen || TransRefLen == (cOldStyleReferenceLen + 3)))
            	{
            		BatchNumber = TransactionRow.BatchNumber;	
            		MatchFound = true;
					break;
            	}
            }
			

            if (!MatchFound)
			{
	            /* look in previous periods (need to compare date of transactions to the month of the stewardship
				   because there may be a batch present which was for last years stewardship - eg. Dec 2007
				   Stewardship for US may have been processed in Jan 2008) */
	            AThisYearOldTransactionTable YearOldTransTable = new AThisYearOldTransactionTable();
				AThisYearOldTransactionRow TemplateRow2 = (AThisYearOldTransactionRow)YearOldTransTable.NewRowTyped(false);
				
				TemplateRow2.LedgerNumber = AICHLedgerNumber;
				TemplateRow2.TransactionStatus = true;
				
	            StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=" });
	
	            AThisYearOldTransactionTable ThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow2, operators2, null, DBTransaction);
	            AThisYearOldTransactionRow ThisYearOldTransactionRow = null;
				
	            string OldTransRef;
	            int OldTransRefLen;
	            
	            for (int i = 0; i < ThisYearOldTransactionTable.Count; i++)
	            {
	            	ThisYearOldTransactionRow = (AThisYearOldTransactionRow)ThisYearOldTransactionTable.Rows[i];
	            	
	            	OldTransRef = ThisYearOldTransactionRow.Reference;
	            	OldTransRefLen = OldTransRef.Length;
	            	
	            	if (OldTransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference
	            	    && (OldTransRefLen == cOldStyleReferenceLen || OldTransRefLen == (cOldStyleReferenceLen + 3))
	            	    && (AMonth <= ThisYearOldTransactionRow.TransactionDate.Month || AMonth > ACurrentPeriod))
	            	{
	            		BatchNumber = ThisYearOldTransactionRow.BatchNumber;	
	            		MatchFound = true;
						break;
	            	}
	            }
	
				/* look in previous years (need to make sure that you only match batches that are stewardships
				   for the same year as the one being processed). */
	           if (!MatchFound && AYear < ACurrentYear)
	           {
	                APreviousYearTransactionTable YearPreviousTransTable = new APreviousYearTransactionTable();
	                APreviousYearTransactionRow TemplateRow3 = (APreviousYearTransactionRow)YearPreviousTransTable.NewRowTyped(false);
	                
	                TemplateRow3.LedgerNumber = AICHLedgerNumber;
	                TemplateRow3.TransactionStatus = true;
	                
	                StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=" });
	    
	                APreviousYearTransactionTable PreviousYearTransactionTable = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow3, operators3, null, DBTransaction);
	                APreviousYearTransactionRow PreviousYearTransactionRow = null;
	                
	                string PreviousTransRef;
	                int PreviousTransRefLen;
	                
	                for (int i = 0; i < PreviousYearTransactionTable.Count; i++)
	                {
	                    PreviousYearTransactionRow = (APreviousYearTransactionRow)PreviousYearTransactionTable.Rows[i];
	                    
	                    PreviousTransRef = PreviousYearTransactionRow.Reference;
	                    PreviousTransRefLen = PreviousTransRef.Length;
	                    
	                    if (PreviousTransRef.Substring(0, cOldStyleReferenceLen) == cOldStyleReference
	                        && (PreviousTransRefLen == cOldStyleReferenceLen || PreviousTransRefLen == (cOldStyleReferenceLen + 3))
	                        && PreviousYearTransactionRow.TransactionDate.Month >= AMonth
	                        && AMonth > ACurrentPeriod
	                        && PreviousYearTransactionRow.TransactionDate.Year == AYear)
	                    {
	                        BatchNumber = PreviousYearTransactionRow.BatchNumber;
							break;
	                    }
	                }
	            } 
			}
            
            return BatchNumber;
		}
		
		
		/// <summary>
		/// Checks the specified stewardship batch to see whether the summary transactions
        ///    match the amounts specified.
		/// </summary>
		/// <param name="AICHLedgerNumber">ICH Ledger number</param>
		/// <param name="ABatchNumber">The batch number of the batch to check</param>
		/// <param name="ACostCentre">Fund to which stewardship batch applies</param>
		/// <param name="AIncomeAmount">Income total to check against</param>
		/// <param name="AExpenseAmount">Expense total to check against</param>
		/// <param name="ATransferAmount">Transfer total to check against</param>
		/// <returns>True if transasctions match, false otherwise</returns>
		public bool SummaryTransactionsMatch(int AICHLedgerNumber, int ABatchNumber, string ACostCentre, decimal AIncomeAmount, decimal AExpenseAmount, decimal ATransferAmount)
		{
			decimal dExistingIncExpTotal;

			TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

			
			ABatchTable BatchTable = ABatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, DBTransaction);
			ABatchRow BatchRow = (ABatchRow)BatchTable.Rows[0];
			
			if (BatchRow != null)
			{
	            /* look for summary transaction for transfers */
				ATransactionTable TransTable = new ATransactionTable();
				ATransactionRow TemplateRow = (ATransactionRow)TransTable.NewRowTyped(false);
				
				TemplateRow.LedgerNumber = AICHLedgerNumber;
				TemplateRow.BatchNumber = ABatchNumber;
				TemplateRow.CostCentreCode = ACostCentre;
				TemplateRow.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH; //i.e. 8540
				
	            StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
	
	            ATransactionTable TransactionTable = ATransactionAccess.LoadUsingTemplate(TemplateRow, operators, null, DBTransaction);
	            ATransactionRow TransactionRow = (ATransactionRow)TransactionTable.Rows[0];
				
				if (TransactionRow != null)
				{
					if (TransactionRow.TransactionAmount != ATransferAmount)
					{
						return false;
					}
				}
				
				/* find the summary transactions for income and expense and sum them */
				dExistingIncExpTotal = 0;

				ATransactionTable TransTable2 = new ATransactionTable();
				ATransactionRow TemplateRow2 = (ATransactionRow)TransTable2.NewRowTyped(false);
				
				TemplateRow2.LedgerNumber = AICHLedgerNumber;
				TemplateRow2.BatchNumber = ABatchNumber;
				TemplateRow2.CostCentreCode = ACostCentre;
				TemplateRow2.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER; //i.e. 8520
				
	            StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
	
	            ATransactionTable TransactionTable2 = ATransactionAccess.LoadUsingTemplate(TemplateRow2, operators2, null, DBTransaction);
	            ATransactionRow TransactionRow2 = null;
	            
	            for (int i = 0; i < TransactionTable2.Count; i++)
	            {
	            	TransactionRow2 = (ATransactionRow)TransactionTable2.Rows[i];
	            	
	            	dExistingIncExpTotal += TransactionRow2.TransactionAmount;
	            }
				
	            return (dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount));
			}
			
			/* now check previous periods if batch wasn't in current period */
			AThisYearOldBatchTable ThisYearOldBatchTable = AThisYearOldBatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, DBTransaction);
			AThisYearOldBatchRow ThisYearOldBatchRow = (AThisYearOldBatchRow)ThisYearOldBatchTable.Rows[0];
			
			if (ThisYearOldBatchRow != null)
			{
	            /* look for summary transaction for transfers */
				AThisYearOldTransactionTable ThisYearOldTransTable = new AThisYearOldTransactionTable();
				AThisYearOldTransactionRow TemplateRow3 = (AThisYearOldTransactionRow)ThisYearOldTransTable.NewRowTyped(false);
				
				TemplateRow3.LedgerNumber = AICHLedgerNumber;
				TemplateRow3.BatchNumber = ABatchNumber;
				TemplateRow3.CostCentreCode = ACostCentre;
				TemplateRow3.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH; //i.e. 8540
				
	            StringCollection operators3 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
	
	            AThisYearOldTransactionTable ThisYearOldTransactionTable = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow3, operators3, null, DBTransaction);
	            AThisYearOldTransactionRow ThisYearOldTransactionRow = (AThisYearOldTransactionRow)ThisYearOldTransactionTable.Rows[0];
				
				if (ThisYearOldTransactionRow != null)
				{
					if (ThisYearOldTransactionRow.TransactionAmount != ATransferAmount)
					{
						return false;
					}
				}
				
				/* find the summary transactions for income and expense and sum them */
				dExistingIncExpTotal = 0;

				AThisYearOldTransactionTable ThisYearOldTransTable4 = new AThisYearOldTransactionTable();
				AThisYearOldTransactionRow TemplateRow4 = (AThisYearOldTransactionRow)ThisYearOldTransTable4.NewRowTyped(false);
				
				TemplateRow4.LedgerNumber = AICHLedgerNumber;
				TemplateRow4.BatchNumber = ABatchNumber;
				TemplateRow4.CostCentreCode = ACostCentre;
				TemplateRow4.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER; //i.e. 8520
				
	            StringCollection operators4 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
	
	            AThisYearOldTransactionTable ThisYearOldTransactionTable2 = AThisYearOldTransactionAccess.LoadUsingTemplate(TemplateRow4, operators4, null, DBTransaction);
	            AThisYearOldTransactionRow ThisYearOldTransactionRow2 = null;
	            
	            for (int i = 0; i < ThisYearOldTransactionTable2.Count; i++)
	            {
	            	ThisYearOldTransactionRow2 = (AThisYearOldTransactionRow)ThisYearOldTransactionTable2.Rows[i];
	            	
	            	dExistingIncExpTotal += ThisYearOldTransactionRow2.TransactionAmount;
	            }
				
	            return (dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount));
			}
			
			/* now check previous years if batch wasn't in current year */
            APreviousYearBatchTable PreviousYearBatchTable = APreviousYearBatchAccess.LoadByPrimaryKey(AICHLedgerNumber, ABatchNumber, DBTransaction);
            APreviousYearBatchRow PreviousYearBatchRow = (APreviousYearBatchRow)PreviousYearBatchTable.Rows[0];
            
            if (PreviousYearBatchRow != null)
            {
                /* look for summary transaction for transfers */
                APreviousYearTransactionTable PreviousYearTransTable = new APreviousYearTransactionTable();
                APreviousYearTransactionRow TemplateRow5 = (APreviousYearTransactionRow)PreviousYearTransTable.NewRowTyped(false);
                
                TemplateRow5.LedgerNumber = AICHLedgerNumber;
                TemplateRow5.BatchNumber = ABatchNumber;
                TemplateRow5.CostCentreCode = ACostCentre;
                TemplateRow5.AccountCode = MFinanceConstants.ICH_ACCT_DEPOSITS_WITH_ICH; //i.e. 8540
                
                StringCollection operators5 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
    
                APreviousYearTransactionTable PreviousYearTransactionTable = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow5, operators5, null, DBTransaction);
                APreviousYearTransactionRow PreviousYearTransactionRow = (APreviousYearTransactionRow)PreviousYearTransactionTable.Rows[0];
                
                if (PreviousYearTransactionRow != null)
                {
                    if (PreviousYearTransactionRow.TransactionAmount != ATransferAmount)
                    {
                        return false;
                    }
                }
                
                /* find the summary transactions for income and expense and sum them */
                dExistingIncExpTotal = 0;

                APreviousYearTransactionTable PreviousYearTransTable4 = new APreviousYearTransactionTable();
                APreviousYearTransactionRow TemplateRow6 = (APreviousYearTransactionRow)PreviousYearTransTable4.NewRowTyped(false);
                
                TemplateRow6.LedgerNumber = AICHLedgerNumber;
                TemplateRow6.BatchNumber = ABatchNumber;
                TemplateRow6.CostCentreCode = ACostCentre;
                TemplateRow6.AccountCode = MFinanceConstants.ICH_ACCT_LOCAL_LEDGER; //i.e. 8520
                
                StringCollection operators6 = StringHelper.InitStrArr(new string[] { "=", "=", "=", "=" });
    
                APreviousYearTransactionTable PreviousYearTransactionTable2 = APreviousYearTransactionAccess.LoadUsingTemplate(TemplateRow6, operators6, null, DBTransaction);
                APreviousYearTransactionRow PreviousYearTransactionRow2 = null;
                
                for (int i = 0; i < PreviousYearTransactionTable2.Count; i++)
                {
                    PreviousYearTransactionRow2 = (APreviousYearTransactionRow)PreviousYearTransactionTable2.Rows[i];
                    
                    dExistingIncExpTotal += PreviousYearTransactionRow2.TransactionAmount;
                }
                
                return (dExistingIncExpTotal == (AIncomeAmount + AExpenseAmount));
            }
			
			return false;	
		}
		
		/// <summary>
		/// Imports all available stewardships (reading from a specific directory)
		///   into the current period.
		/// </summary>
		public void ImportAllAvailableStewardshipReports(int ALedgerNumber, string AICHFolder)
		{
			string cLogFile;
			string cICHDir;
			string cPendingDir;
			string cNewDir;
			string cCurrentFile;
			string cInputLine; 
			string cPeriod; 
			string cNewFileName; 
			string cUnsuccessfulFileList; 
			string cMessage; 
			string cTime;   
			string cPreviousFileName; 
			int iHours;   
			int iCount;   
			int iMins;   
			int iPreviousLedger;   
			int iPreviousRunNumber;   
			int iPreviousPeriod;   
			bool lMethodOk;
			bool lSuccessful;   
			bool lFormatOK = true;
			DateTime dtPeriodDate;
			bool lDateLineValid;

			/* temp table to store basic details about each stewardship file */
			DataTable ttFileList = new DataTable();

            ttFileList.Columns.Add("cFileName", typeof(string));
            ttFileList.Columns.Add("dtReportDate", typeof(DateTime));
            ttFileList.Columns.Add("iReportTimeInMins", typeof(int));
            ttFileList.Columns.Add("iLedger", typeof(int));
            ttFileList.Columns.Add("iPeriod", typeof(int));
            ttFileList.Columns.Add("iYear", typeof(int));
            ttFileList.Columns.Add("iRunNumber", typeof(int));

            cPendingDir = AICHFolder + @"\pending" ;

            /* Check that previous period has been closed so that these stewardships go into the right period.
				   If it hasn't been closed then don't go any further 
			FIND FIRST a_accounting_period WHERE a_accounting_period.a_ledger_number_i = pv_ledger_number_i
                                 AND CAN-FIND(FIRST a_ledger WHERE a_ledger.a_ledger_number_i = pv_ledger_number_i
                                                               AND a_ledger.a_current_period_i = a_accounting_period.a_accounting_period_number_i) NO-LOCK.
			IF TODAY > a_accounting_period.a_period_start_date_d + 60 THEN DO:
			    MESSAGE "It looks like you need to close the current period before processing the current batch of Stewardships"
			        VIEW-AS ALERT-BOX INFO BUTTONS OK.
			    RETURN.
			END.
             */

            
		}
		
		
		
   }
}