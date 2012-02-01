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
		/// <param name="ACurrency"></param>
		/// <param name="AFileName"></param>
		/// <param name="AEmail"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns></returns>
    	public void GenerateStewardshipFile(int ALedgerNumber,
            int APeriodNumber,
            int AICHNumber,
            int ACurrency,
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
            	if (ACurrency == 1)
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
                		
                		if (ACurrency == 1)
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

		
   }
}