//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL.Data.Access;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

namespace Ict.Petra.Server.MFinance.Budget.WebConnectors
{
	/// <summary>
    /// maintain the budget
    /// </summary>
    public class TBudgetMaintainWebConnector
    {

    	/// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadBudget(Int32 ALedgerNumber)
        {
            BudgetTDS MainDS = new BudgetTDS();

            //TODO: need to filter on Year
            ABudgetAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            ABudgetRevisionAccess.LoadViaALedger(MainDS, ALedgerNumber, null);
            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
            ABudgetPeriodAccess.LoadAll(MainDS, null);
            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, null);

//            ABudgetPeriodTable BudgetPeriodTable = new ABudgetPeriodTable();
//            ABudgetPeriodRow TemplateRow = (ABudgetPeriodRow)BudgetPeriodTable.NewRow(false);
//
//            TemplateRow.BudgetSequence;
//            ABudgetPeriodAccess.LoadViaABudgetTemplate(MainDS, TemplateRow, null);


            // Accept row changes here so that the Client gets 'unmodified' rows
            MainDS.AcceptChanges();

            // Remove all Tables that were not filled with data before remoting them.
            MainDS.RemoveEmptyTables();

            return MainDS;
        }

		

        /// <summary>
        /// save modified budgets
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveBudget(ref BudgetTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            if (AInspectDS != null)
            {
                return BudgetTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);
            }

            return TSubmitChangesResult.scrError;
        }

		/// <summary>
		/// Imports budgets
		/// </summary>
		/// <param name="ALedgerNumber"></param>
		/// <param name="ACurrentBudgetYear"></param>
		/// <param name="ACSVFileName"></param>
		/// <param name="AFdlgSeparator"></param>
		/// <param name="AImportDS"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static int ImportBudgets(Int32 ALedgerNumber, Int32 ACurrentBudgetYear, string ACSVFileName, string[] AFdlgSeparator, ref BudgetTDS AImportDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            if (AImportDS != null)
            {
                int retVal = ImportBudgetFromCSV(ALedgerNumber, ACurrentBudgetYear, ACSVFileName, AFdlgSeparator, ref AImportDS, ref AVerificationResult);
                return retVal;
            }

            return 0;
        }

        
		/// <summary>
        /// Import the budget from a CSV file
		/// </summary>
		/// <param name="ALedgerNumber"></param>
		/// <param name="ACurrentBudgetYear"></param>
		/// <param name="ACSVFileName"></param>
		/// <param name="AFdlgSeparator"></param>
		/// <param name="AImportDS"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns>Total number of records imported</returns>
        private static int ImportBudgetFromCSV(Int32 ALedgerNumber, Int32 ACurrentBudgetYear, string ACSVFileName, string[] AFdlgSeparator, ref BudgetTDS AImportDS,
            ref TVerificationResultCollection AVerificationResult)
        {
            StreamReader DataFile = new StreamReader(ACSVFileName, System.Text.Encoding.Default);

            string Separator = AFdlgSeparator[0];
            string DateFormat = AFdlgSeparator[1];
            string NumberFormat = AFdlgSeparator[2] == "0" ? "American" : "European";

            //CultureInfo MyCultureInfoDate = new CultureInfo("en-GB");
            //MyCultureInfoDate.DateTimeFormat.ShortDatePattern = DateFormat;

            // To store the From and To currencies
            // Use an array to store these to make for easy
            //   inverting of the two currencies when calculating
            //   the inverse value.

            string currentBudgetVal = string.Empty;
            string mess = string.Empty;
            string CostCentre = string.Empty;
            string Account = string.Empty;
            string BudgetType = string.Empty;
            int YearFromCSV = 0;

            decimal[] BudgetPeriods = new decimal[12];
            int YearForBudgetRevision = 0;
            int BdgRevision = 0;  //not currently implementing versioning so always zero
            bool HasRunOnce = false;

            int newSequence = -1;

            //Find the next budget sequence
            if (AImportDS.ABudget.Rows.Find(new object[] { newSequence }) != null)
            {
            	newSequence = newSequence * (AImportDS.ABudget.Rows.Count);
            }

            int rowNumber = 0;

            while (!DataFile.EndOfStream)
            {
                if (HasRunOnce)
                {
                    newSequence--;
                }

                decimal totalBudgetRowAmount = 0;

                try
                {
                    string Line = DataFile.ReadLine();

                    CostCentre = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();

                    if (CostCentre == "Cost Centre")
                    {
                        //Read the next line
                        Line = DataFile.ReadLine();
                        CostCentre = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();
                        //newSequence--;
                    }

                    //Increment row number
                    rowNumber++;

                    //Convert separator to a char
                    char Sep = Separator[0];
                    //Turn current line into string array of column values
                    string[] CsvColumns = Line.Split(Sep);

                    int NumCols = CsvColumns.Length;

                    //If number of columns is not 4 then import csv file is wrongly formed.
//                if (NumCols != 24)
//                {
//                    AVerificationResult. MessageBox.Show(Catalog.GetString("Failed to import the CSV budget file:\r\n\r\n" +
//                            "   " + ADataFilename + "\r\n\r\n" +
//                            "It contains " + NumCols.ToString() + " columns. " +
//                            ), AImportMode + " Exchange Rates Import Error");
//                    return;
//                }

                    //Read the values for the current line
                    Account = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();
                    BudgetType = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();

                    if ((BudgetType != MFinanceConstants.BUDGET_ADHOC)
                        && (BudgetType != MFinanceConstants.BUDGET_SAME)
                        && (BudgetType != MFinanceConstants.BUDGET_INFLATE_N)
                        && (BudgetType != MFinanceConstants.BUDGET_SPLIT)
                        && (BudgetType != MFinanceConstants.BUDGET_INFLATE_BASE)
                        )
                    {
                        throw new InvalidOperationException("Budget Type: " + BudgetType + " in row: " + rowNumber.ToString() + " does not exist.");
                    }

                    //Calculate the budget Year
                    YearFromCSV = Convert.ToInt32(StringHelper.GetNextCSV(ref Line, Separator, false));

                    ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, null);
                    ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

                    DateTime CurrentYearEnd = TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber,
                        LedgerRow.CurrentFinancialYear,
                        0,
                        LedgerRow.NumberOfAccountingPeriods);

                    YearForBudgetRevision = YearFromCSV - CurrentYearEnd.Year + LedgerRow.CurrentFinancialYear;

                    //Add the budget revision sequence. Only need to do once
                    if (!HasRunOnce)
                    {
                        HasRunOnce = true;

	                    //Check if in correct year
	                    if (ACurrentBudgetYear != YearForBudgetRevision)
	                    {
	                    	return -1;
	                    }

	                    if (AImportDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, YearForBudgetRevision, BdgRevision }) == null)
                        {
                            ABudgetRevisionRow BudgetRevisionRow = (ABudgetRevisionRow)AImportDS.ABudgetRevision.NewRowTyped();
                            BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                            BudgetRevisionRow.Year = YearForBudgetRevision;
                            BudgetRevisionRow.Revision = BdgRevision;
                            BudgetRevisionRow.Description = "Budget Import from: " + ACSVFileName;
                            AImportDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
                        }
                    }

                    //Read the budgetperiod values to check if valid according to type
                    Array.Clear(BudgetPeriods, 0, 12);

                    for (int i = 0; i < 12; i++)
                    {
                        currentBudgetVal = StringHelper.GetNextCSV(ref Line, Separator, false);
                        BudgetPeriods[i] = Convert.ToDecimal(currentBudgetVal);
                        totalBudgetRowAmount += BudgetPeriods[i];
                    }

                    bool ErrorInPeriodValues = false;

                    switch (BudgetType)
                    {
                        case MFinanceConstants.BUDGET_SAME:

                            if (Array.TrueForAll(BudgetPeriods, IsZero)
                                || !ValidateBudgetTypeSame(BudgetPeriods))
                            {
                                ErrorInPeriodValues = true;
                            }

                            break;

                        case MFinanceConstants.BUDGET_SPLIT:

                            if (Array.TrueForAll(BudgetPeriods, IsZero)
                                || !ValidateBudgetTypeSplit(BudgetPeriods))
                            {
                                ErrorInPeriodValues = true;
                            }

                            break;

                        case MFinanceConstants.BUDGET_INFLATE_BASE:

                            if (Array.TrueForAll(BudgetPeriods, IsZero)
                                || !ValidateBudgetTypeInflateBase(BudgetPeriods))
                            {
                                ErrorInPeriodValues = true;
                            }

                            break;

                        case MFinanceConstants.BUDGET_INFLATE_N:

                            if (Array.TrueForAll(BudgetPeriods, IsZero)
                                || !ValidateBudgetTypeInflateN(BudgetPeriods))
                            {
                                ErrorInPeriodValues = true;
                            }

                            break;

                        case MFinanceConstants.BUDGET_ADHOC:

                            if (Array.TrueForAll(BudgetPeriods, IsZero))
                            {
                                ErrorInPeriodValues = true;
                            }

                            break;

                        default:                          //Unknown budget type
                            throw new InvalidOperationException(String.Format(
                                "The budget in row {0} for Ledger: {1}, Year: {2}, Cost Centre: {3} and Account: {4}, has the unrecognised Budget Type: {5}.",
                                rowNumber,
                                ALedgerNumber,
                                YearFromCSV,
                                CostCentre,
                                Account,
                                BudgetType));

                            //break;
                    }

                    if (ErrorInPeriodValues)
                    {
                        throw new InvalidOperationException(String.Format(
                                "The budget in row {0} for Ledger: {1}, Year: {2}, Cost Centre: {3} and Account: {4}, does not have values consistent with Budget Type: {5}.",
                                rowNumber,
                                ALedgerNumber,
                                YearFromCSV,
                                CostCentre,
                                Account,
                                BudgetType));
                    }

                    BudgetTDS MainDS = new BudgetTDS();

		            ABudgetAccess.LoadByUniqueKey(MainDS, ALedgerNumber, YearForBudgetRevision, BdgRevision, CostCentre, Account,  null);
		            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
		            
					//Check to see if the budget combination already exists:
                    
                    if (MainDS.ABudget.Count > 0)
                    {
                    	ABudgetRow BR2 = (ABudgetRow)MainDS.ABudget.Rows[0];
                    	
                    	int BTSeq = BR2.BudgetSequence;

                    	ABudgetRow BdgTRow = (ABudgetRow)AImportDS.ABudget.Rows.Find(new object[] { BTSeq });
                    	
                    	if (BdgTRow != null)
                    	{
                    		BdgTRow.BeginEdit();
		                    //Edit the new budget row
		                    BdgTRow.BudgetTypeCode = BudgetType;
                    		BdgTRow.EndEdit();
                    		
                    		ABudgetPeriodRow BPRow = null;
				
				            for (int i = 0; i < 12; i++)
				            {
				                BPRow = (ABudgetPeriodRow)AImportDS.ABudgetPeriod.Rows.Find(new object[] { BTSeq, i + 1 });
				
				                if (BPRow != null)
				                {
				                	BPRow.BeginEdit();
				                	BPRow.BudgetBase = BudgetPeriods[i];
				                	BPRow.EndEdit();
				                }
				
				                BPRow = null;
				            }
                    	}
                    }
                    else
                    {
	                    //Add the new budget row
	                    ABudgetRow BudgetRow = (ABudgetRow)AImportDS.ABudget.NewRowTyped();
	                    BudgetRow.BudgetSequence = newSequence;
	                    BudgetRow.LedgerNumber = ALedgerNumber;
	                    BudgetRow.Year = YearForBudgetRevision;
	                    BudgetRow.Revision = BdgRevision;
	                    BudgetRow.CostCentreCode = CostCentre;
	                    BudgetRow.AccountCode = Account;
	                    BudgetRow.BudgetTypeCode = BudgetType;
	                    AImportDS.ABudget.Rows.Add(BudgetRow);
	
	                    //Add the budget periods
	                    for (int i = 0; i < 12; i++)
	                    {
	                        ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)AImportDS.ABudgetPeriod.NewRowTyped();
	                        BudgetPeriodRow.BudgetSequence = newSequence;
	                        BudgetPeriodRow.PeriodNumber = i + 1;
	                        BudgetPeriodRow.BudgetBase = BudgetPeriods[i];
	                        AImportDS.ABudgetPeriod.Rows.Add(BudgetPeriodRow);
	                    }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }

            DataFile.Close();

            return rowNumber;
        }


        /// <summary>
        /// GetGLMSequence
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        /// <param name="AYear"></param>
        /// <returns>GLM Sequence no</returns>
        [RequireModulePermission("FINANCE-3")]
        public static int GetGLMSequenceForBudget(int ALedgerNumber, string AAccountCode, string ACostCentreCode, int AYear)
        {
            int retVal;

            bool NewTransaction = false;
            TDBTransaction dbtrans = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);

            try
            {
                AGeneralLedgerMasterTable GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByUniqueKey(ALedgerNumber,
                    AYear,
                    AAccountCode,
                    ACostCentreCode,
                    dbtrans);

                if (GeneralLedgerMasterTable.Count > 0)
                {
                    retVal = (int)GeneralLedgerMasterTable.Rows[0].ItemArray[0];
                }
                else
                {
                    retVal = -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return retVal;
        }

        /// <summary>
        /// ------------------------------------------------------------------------------
        /// Description: GetActual retrieves the actuals value of the given period, no matter if it is in a forwarding period.
        ///  GetActual is similar to GetBudget. The main difference is, that forwarding periods are saved in the current year.
        ///  You still need the sequence_next_year, because this_year can be older than current_financial_year of the ledger.
        ///  So you need to give number_accounting_periods and current_financial_year of the ledger.
        ///  You also need to give the number of the year from which you want the data.
        ///  Currency_select is either "B" for base or "I" for international currency or "T" for transaction currency
        ///  You want e.g. the actual data of period 13 in year 2, the current financial year is 3.
        ///  The call would look like: GetActual(sequence_year_2, sequence_year_3, 13, 12, 3, 2, false, "B");
        ///  That means, the function has to return the difference between year 3 period 1 and the start balance of year 3.
        /// ------------------------------------------------------------------------------
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetActual(int ALedgerNumber,
            int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYTD,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            retVal = GetActualInternal(ALedgerNumber,
                AGLMSeqThisYear,
                AGLMSeqNextYear,
                APeriodNumber,
                ANumberAccountingPeriods,
                ACurrentFinancialYear,
                AThisYear,
                AYTD,
                false,
                ACurrencySelect);

            return retVal;
        }

        /// <summary>
        /// Get the actual amount
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AThisYear"></param>
        /// <param name="AYTD"></param>
        /// <param name="ABalSheetForwardPeriods"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        private static decimal GetActualInternal(int ALedgerNumber,
            int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            int ACurrentFinancialYear,
            int AThisYear,
            bool AYTD,
            bool ABalSheetForwardPeriods,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            decimal CurrencyAmount = 0;
            bool IncExpAccountFwdPeriod = false;

            //DEFINE BUFFER a_glm_period FOR a_general_ledger_master_period.
            //DEFINE BUFFER a_glm FOR a_general_ledger_master.
            //DEFINE BUFFER buf_account FOR a_account.

            if (AGLMSeqThisYear == -1)
            {
                return retVal;
            }

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction();

            AGeneralLedgerMasterTable GeneralLedgerMasterTable = null;
            AGeneralLedgerMasterRow GeneralLedgerMasterRow = null;

            AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

            AAccountTable AccountTable = null;
            AAccountRow AccountRow = null;

            try
            {
                if (APeriodNumber == 0)             /* start balance */
                {
                    GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGLMSeqThisYear, DBTransaction);
                    GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];

                    switch (ACurrencySelect)
                    {
                        case MFinanceConstants.CURRENCY_BASE:
                            CurrencyAmount = GeneralLedgerMasterRow.StartBalanceBase;
                            break;

                        case MFinanceConstants.CURRENCY_INTERNATIONAL:
                            CurrencyAmount = GeneralLedgerMasterRow.StartBalanceIntl;
                            break;

                        default:
                            CurrencyAmount = GeneralLedgerMasterRow.StartBalanceForeign;
                            break;
                    }
                }
                else if (APeriodNumber > ANumberAccountingPeriods)             /* forwarding periods only exist for the current financial year */
                {
                    if (ACurrentFinancialYear == AThisYear)
                    {
                        GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqThisYear,
                            APeriodNumber,
                            DBTransaction);
                        GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];
                    }
                    else
                    {
                        GeneralLedgerMasterPeriodTable =
                            AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqNextYear,
                                (APeriodNumber - ANumberAccountingPeriods),
                                DBTransaction);
                        GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];
                    }
                }
                else             /* normal period */
                {
                    GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeqThisYear, APeriodNumber, DBTransaction);
                    GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];
                }

                if (GeneralLedgerMasterPeriodRow != null)
                {
                    switch (ACurrencySelect)
                    {
                        case MFinanceConstants.CURRENCY_BASE:
                            CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualBase;
                            break;

                        case MFinanceConstants.CURRENCY_INTERNATIONAL:
                            CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualIntl;
                            break;

                        default:
                            CurrencyAmount = GeneralLedgerMasterPeriodRow.ActualForeign;
                            break;
                    }
                }

                if ((APeriodNumber > ANumberAccountingPeriods) && (ACurrentFinancialYear == AThisYear))
                {
                    GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadByPrimaryKey(AGLMSeqThisYear, DBTransaction);
                    GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[0];

                    AccountTable = AAccountAccess.LoadByPrimaryKey(ALedgerNumber, GeneralLedgerMasterRow.AccountCode, DBTransaction);
                    AccountRow = (AAccountRow)AccountTable.Rows[0];

                    if ((AccountRow.AccountCode.ToUpper() == MFinanceConstants.ACCOUNT_TYPE_INCOME.ToUpper())
                        || (AccountRow.AccountCode.ToUpper() == MFinanceConstants.ACCOUNT_TYPE_EXPENSE.ToUpper())
                        && !ABalSheetForwardPeriods)
                    {
                        IncExpAccountFwdPeriod = true;
                        CurrencyAmount -= GetActualInternal(ALedgerNumber,
                            AGLMSeqThisYear,
                            AGLMSeqNextYear,
                            ANumberAccountingPeriods,
                            ANumberAccountingPeriods,
                            ACurrentFinancialYear,
                            AThisYear,
                            true,
                            ABalSheetForwardPeriods,
                            ACurrencySelect);
                    }
                }

                if (!AYTD)
                {
                    if (!((APeriodNumber == (ANumberAccountingPeriods + 1)) && IncExpAccountFwdPeriod)
                        && !((APeriodNumber == (ANumberAccountingPeriods + 1)) && (ACurrentFinancialYear > AThisYear)))
                    {
                        /* if it is an income expense acount, and we are in a forward period, nothing needs to be subtracted,
                         * because that was done in correcting the amount in the block above;
                         * if we are in a previous year, in a forward period, don't worry about subtracting.
                         */
                        CurrencyAmount -= GetActualInternal(ALedgerNumber,
                            AGLMSeqThisYear,
                            AGLMSeqNextYear,
                            (APeriodNumber - 1),
                            ANumberAccountingPeriods,
                            ACurrentFinancialYear,
                            AThisYear,
                            true,
                            ABalSheetForwardPeriods,
                            ACurrencySelect);
                    }
                }

                retVal = CurrencyAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return retVal;
        }

        
        /// <summary>
        /// Retrieves a budget value
        /// </summary>
        /// <param name="AGLMSeqThisYear"></param>
        /// <param name="AGLMSeqNextYear"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="ANumberAccountingPeriods"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetBudget(int AGLMSeqThisYear,
            int AGLMSeqNextYear,
            int APeriodNumber,
            int ANumberAccountingPeriods,
            bool AYTD,
            string ACurrencySelect)
        {
            decimal retVal = 0;

            if (APeriodNumber > ANumberAccountingPeriods)
            {
                retVal = CalculateBudget(AGLMSeqNextYear, 1, (APeriodNumber - ANumberAccountingPeriods), AYTD, ACurrencySelect);
            }
            else
            {
                retVal = CalculateBudget(AGLMSeqThisYear, 1, APeriodNumber, AYTD, ACurrencySelect);
            }

            return retVal;
        }
        
        
        /// <summary>
        ///Description: CalculateBudget is only used internally as a helper function for GetBudget.
        ///Returns the budget for the given period of time,
        ///  if ytd is set, this period is from start_period to end_period,
        ///  otherwise it is only the value of the end_period.
        ///  currency_select is either "B" for base or "I" for international currency
        /// </summary>
        /// <param name="AGLMSeq"></param>
        /// <param name="AStartPeriod"></param>
        /// <param name="AEndPeriod"></param>
        /// <param name="AYTD"></param>
        /// <param name="ACurrencySelect"></param>
        /// <returns></returns>
        private static decimal CalculateBudget(int AGLMSeq, int AStartPeriod, int AEndPeriod, bool AYTD, string ACurrencySelect)
        {
            decimal retVal = 0;

            decimal lv_currency_amount_n = 0;
            int lv_ytd_period_i;

            TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction();

            AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = null;
            AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;

            try
            {
                if (AGLMSeq == -1)
                {
                    return retVal;
                }

                if (!AYTD)
                {
                    AStartPeriod = AEndPeriod;
                }

                for (lv_ytd_period_i = AStartPeriod; lv_ytd_period_i <= AEndPeriod; lv_ytd_period_i++)
                {
                    GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSeq, lv_ytd_period_i, DBTransaction);
                    GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

                    if (GeneralLedgerMasterPeriodRow != null)
                    {
                        if (ACurrencySelect == MFinanceConstants.CURRENCY_BASE)
                        {
                            lv_currency_amount_n += GeneralLedgerMasterPeriodRow.BudgetBase;
                        }
                        else if (ACurrencySelect == MFinanceConstants.CURRENCY_INTERNATIONAL)
                        {
                            lv_currency_amount_n += GeneralLedgerMasterPeriodRow.BudgetIntl;
                        }
                    }
                }

                retVal = lv_currency_amount_n;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return retVal;
        }


//        private static bool ExportBudgetToCSV(Int32 ALedgerNumber, string ACSVFileName, string[] AFdlgSeparator, ref BudgetTDS AImportDS,
//            out TVerificationResultCollection AVerificationResult)
//        {
//
//        }

        private static bool IsZero(decimal d)
        {
            return d == 0;
        }

		/// <summary>
		/// Validate Budget Type: Same
		/// </summary>
		/// <param name="APeriodValues"></param>
		/// <returns></returns>
        private static bool ValidateBudgetTypeSame(decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;

            decimal Period1Amount = APeriodValues[0];

            for (int i = 1; i < 12; i++)
            {
                if (Period1Amount != APeriodValues[i])
                {
                    PeriodValuesOK = false;
                    break;
                }
            }

            return PeriodValuesOK;
        }

        /// <summary>
        /// Validate Budget Type: Split
        /// </summary>
        /// <param name="APeriodValues"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeSplit(decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;

            decimal Period1Amount = APeriodValues[0];

            for (int i = 1; i < 11; i++)
            {
                if (Period1Amount != APeriodValues[i])
                {
                    PeriodValuesOK = false;
                    break;
                }
            }

            if (PeriodValuesOK)
            {
                if ((APeriodValues[11] < Period1Amount)
                    || ((APeriodValues[11] - Period1Amount) >= 12))
                {
                    PeriodValuesOK = false;
                }
            }

            return PeriodValuesOK;
        }

        /// <summary>
        /// Validate Budget Type: Base
        /// </summary>
        /// <param name="APeriodValues"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeInflateBase(decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;

            if (APeriodValues[0] == 0)
            {
                PeriodValuesOK = false;
            }

            return PeriodValuesOK;
        }

        /// <summary>
        /// Validate Budget Type: Inflate n
        /// </summary>
        /// <param name="APeriodValues"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeInflateN(decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;
            bool PeriodAmountHasChanged = false;

            decimal Period1Amount = APeriodValues[0];

            if (Period1Amount == 0)
            {
                PeriodValuesOK = false;
                return PeriodValuesOK;
            }

            for (int i = 1; i < 12; i++)
            {
                if (Period1Amount != APeriodValues[i])
                {
                    if (PeriodAmountHasChanged)
                    {
                        PeriodValuesOK = false;
                        break;
                    }
                    else
                    {
                        Period1Amount = APeriodValues[i];
                        PeriodAmountHasChanged = true;
                    }
                }
            }

            return PeriodValuesOK;
        }


		/// <summary>
		/// Consolidate Budgets.
		/// </summary>
		/// <param name="ALedgerNumber"></param>
		/// <param name="AConsolidateAll"></param>
		/// <param name="ABudgetTDS"></param>
		/// <param name="AVerificationResult"></param>
		/// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static bool ConsolidateBudgets(Int32 ALedgerNumber, bool AConsolidateAll, ref BudgetTDS ABudgetTDS,
            out TVerificationResultCollection AVerificationResult)
        {
            bool retVal = false;
            
            int Year;
            int Period;

            string PreviousAccount = string.Empty;
            
            string CurrentGLMAccountCode;
            int CurrentGLMSequence;

            AVerificationResult = null;
            
            //Create the temp table
            // Same as the wtPeriodData table in 4GL
    		DataTable PeriodDataTempTable = CreateTempTable();
            
            ABudgetTable BudgetTable = ABudgetTDS.ABudget;
            ABudgetRow BudgetRow = null;

            ALedgerTable LedgerTable = ABudgetTDS.ALedger;
            ALedgerRow LedgerRow = (ALedgerRow)LedgerTable.Rows[0];

            if (AConsolidateAll)
            {
                for (int i = 0; i < BudgetTable.Count; i++)
                {
                    BudgetRow = (ABudgetRow)BudgetTable.Rows[i];
                    BudgetRow.BeginEdit();
                    BudgetRow.BudgetStatus = false;
                    BudgetRow.EndEdit();
                }

                for (int i = 0; i <= 1; i++)
                {
                	Year = LedgerRow.CurrentFinancialYear + i;

                	AGeneralLedgerMasterTable GenLedgerMasterTable = new AGeneralLedgerMasterTable();
                    AGeneralLedgerMasterRow TemplateRow = (AGeneralLedgerMasterRow)GenLedgerMasterTable.NewRowTyped(false);

                    TemplateRow.LedgerNumber = ALedgerNumber;
                    TemplateRow.Year = Year;

                    StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "=" });
                    StringCollection OrderList = new StringCollection();

                    OrderList.Add("ORDER BY");
                    OrderList.Add(AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " ASC");
                    OrderList.Add(AGeneralLedgerMasterTable.GetYearDBName() + " ASC");

                    AGeneralLedgerMasterTable GeneralLedgerMasterTable = AGeneralLedgerMasterAccess.LoadUsingTemplate(TemplateRow,
                        operators,
                        null,
                        null,
                        OrderList,
                        0,
                        0);
                    
                    AGeneralLedgerMasterRow GeneralLedgerMasterRow = null;

                    for (int j = 0; j < GeneralLedgerMasterTable.Count; j++)
                    {
                        GeneralLedgerMasterRow = (AGeneralLedgerMasterRow)GeneralLedgerMasterTable.Rows[j];

                        CurrentGLMAccountCode = GeneralLedgerMasterRow.AccountCode;
                        CurrentGLMSequence = GeneralLedgerMasterRow.GlmSequence;
                        
                        if (PreviousAccount != CurrentGLMAccountCode)
                        {
                        	PreviousAccount = CurrentGLMAccountCode;
                        }

                      	for (Period = 1; Period <= LedgerRow.NumberOfAccountingPeriods; Period++)
                        {
                      		ClearAllBudgetValues(ref PeriodDataTempTable, CurrentGLMSequence, Period);
                        }
                    }
                }
            }
            
            for (int k = 0; k < BudgetTable.Count; k++)
            {
            	BudgetRow = (ABudgetRow)BudgetTable.Rows[k];
            	if (!BudgetRow.BudgetStatus || AConsolidateAll)
            	{
            		if (!AConsolidateAll)
            		{
            			UnPostBudget(ref PeriodDataTempTable, ref ABudgetTDS, ref BudgetRow, ALedgerNumber);
            		}	
            		
            		PostBudget(ref PeriodDataTempTable, ref ABudgetTDS, ref BudgetRow, ALedgerNumber);
            	}
            }

			FinishConsolidateBudget(ALedgerNumber, ref PeriodDataTempTable, ref BudgetTable);
            
            return retVal;
        }
        
        
        /// <summary>
        /// Complete the Budget consolidation process
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetTable"></param>
        private static void FinishConsolidateBudget(int ALedgerNumber, ref DataTable APeriodDataTable, ref ABudgetTable ABudgetTable)
        {
        	decimal IntlExchangeRate;
    		int PreviousSequence = 0;
    		int CurrentSequence;
    		
    		if (TExchangeRateTools.GetLatestIntlCorpExchangeRate(ALedgerNumber, out IntlExchangeRate))
    		{
    			/*Consolidate_Budget*/

    			AGeneralLedgerMasterPeriodTable GLMPTable = null;
    			AGeneralLedgerMasterPeriodRow GLMPRow = null;
    			DataRow DR = null;
    			
    			for (int i = 0; i < APeriodDataTable.Rows.Count; i++)
    			{
    				DR = (DataRow)APeriodDataTable.Rows[i];
    				CurrentSequence = Convert.ToInt32(DR.ItemArray[0]);
    				
    				if (PreviousSequence != CurrentSequence)
    				{
    					PreviousSequence = CurrentSequence;
    				}
    				
    				GLMPTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(PreviousSequence, Convert.ToInt32(DR.ItemArray[1]), null);
					GLMPRow = (AGeneralLedgerMasterPeriodRow)GLMPTable.Rows[0];
					
					GLMPRow.BeginEdit();
					GLMPRow.BudgetBase = Convert.ToDecimal(DR.ItemArray[2]);
					GLMPRow.BudgetIntl = Math.Round(Convert.ToDecimal(DR.ItemArray[2]) / IntlExchangeRate);
					GLMPRow.EndEdit();
    			}
    			
    			ABudgetRow BudgetRow = null;
    			
    			for (int i = 0; i < ABudgetTable.Count; i++)
    			{
    				BudgetRow = (ABudgetRow)ABudgetTable.Rows[i];
    				
    				BudgetRow.BeginEdit();
    				BudgetRow.BudgetStatus = true;
    				BudgetRow.EndEdit();
    			}
    			
    		}
	
        }
        
        
		/// <summary>
        /// Return the budget amount from the temp table wtPeriodData. 
		///   if the record is not already in the temp table, it is fetched
		/// </summary>
		/// <param name="APeriodDataTable"></param>
		/// <param name="AGLMSequence"></param>
		/// <param name="APeriodNumber"></param>
		/// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static decimal GetBudgetValue(ref DataTable APeriodDataTable, int AGLMSequence, int APeriodNumber)
        {
        	decimal GetBudgetValue = 0;

            DataRow TempRow = (DataRow)APeriodDataTable.Rows.Find(new object[] {AGLMSequence, APeriodNumber});
            
        	if (TempRow == null)
        	{
 				AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence, APeriodNumber, null);  
				AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;
 				
				if (GeneralLedgerMasterPeriodTable.Count > 0)
				{
					GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

					DataRow DR = (DataRow)APeriodDataTable.NewRow();
					DR.ItemArray[0] = AGLMSequence;
					DR.ItemArray[1] = APeriodNumber;
					DR.ItemArray[2] = GeneralLedgerMasterPeriodRow.BudgetBase;
					
					APeriodDataTable.Rows.Add(DR);
				}			
        	}
        	else
        	{
        		//Set to budget base
        		GetBudgetValue = Convert.ToDecimal(TempRow.ItemArray[2]);
        	}
            
        	return GetBudgetValue;
        }

        
        /// <summary>
        /// Unpost a budget
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetTDS"></param>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        private static bool UnPostBudget(ref DataTable APeriodDataTable, ref BudgetTDS ABudgetTDS, ref ABudgetRow ABudgetRow, int ALedgerNumber)
        {
        	decimal[] TempAmountsThisYear = new decimal[19];             // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
        	decimal[] TempAmountsNextYear = new decimal[19];             // AS DECIMAL EXTENT {&MAX-PERIODS} NO-UNDO. Max-Periods = 20
            //bool lv_answer_l = false;
            int GLMSequenceThisYear;
            int GLMSequenceNextYear;

            ALedgerRow LedgerRow = (ALedgerRow)ABudgetTDS.ALedger.Rows[0];
            
            GLMSequenceThisYear = GetGLMSequenceForBudget(ALedgerNumber, ABudgetRow.AccountCode, ABudgetRow.CostCentreCode, LedgerRow.CurrentFinancialYear);
			GLMSequenceNextYear = GetGLMSequenceForBudget(ALedgerNumber, ABudgetRow.AccountCode, ABudgetRow.CostCentreCode, LedgerRow.CurrentFinancialYear + 1);

			if (GLMSequenceThisYear != -1 || GLMSequenceNextYear != -1)
			{
            	AGeneralLedgerMasterPeriodTable GenLedgerMasterPeriodTable = new AGeneralLedgerMasterPeriodTable();
                AGeneralLedgerMasterPeriodRow TemplateRow = (AGeneralLedgerMasterPeriodRow)GenLedgerMasterPeriodTable.NewRowTyped(false);

                TemplateRow.GlmSequence = GLMSequenceThisYear;
                TemplateRow.BudgetBase = 0;

                StringCollection operators = StringHelper.InitStrArr(new string[] { "=", "<>" });

                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadUsingTemplate(TemplateRow,
                    operators,
                    null,
                    null,
                    null,
                    0,
                    0);
				

				bool BudgetValueExists = false;
                if (GeneralLedgerMasterPeriodTable.Count > 0)
                {
                	BudgetValueExists = true;
                }
                else
                {
	            	AGeneralLedgerMasterPeriodTable GenLedgerMasterPeriodTable2 = new AGeneralLedgerMasterPeriodTable();
	                AGeneralLedgerMasterPeriodRow TemplateRow2 = (AGeneralLedgerMasterPeriodRow)GenLedgerMasterPeriodTable2.NewRowTyped(false);
	
	                TemplateRow2.GlmSequence = GLMSequenceNextYear;
	                TemplateRow2.BudgetBase = 0;
	
	                StringCollection operators2 = StringHelper.InitStrArr(new string[] { "=", "<>" });
	
	                AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable2 = AGeneralLedgerMasterPeriodAccess.LoadUsingTemplate(TemplateRow,
	                    operators2,
	                    null,
	                    null,
	                    null,
	                    0,
	                    0);
	                if (GeneralLedgerMasterPeriodTable2.Count > 0)
	                {
	                	BudgetValueExists = true;
	                }
                }

				if (BudgetValueExists)
				{
					ABudgetPeriodTable BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);	

					for (int i = 0; i < BPT.Count; i++)
					{
						ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];
						
						TempAmountsThisYear[BPR.PeriodNumber] = BPR.BudgetThisYear;
						TempAmountsNextYear[BPR.PeriodNumber] = BPR.BudgetNextYear;
						BPR.BeginEdit();
						BPR.BudgetThisYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceThisYear, BPR.PeriodNumber);
						BPR.BudgetNextYear = -1 * GetBudgetValue(ref APeriodDataTable, GLMSequenceNextYear, BPR.PeriodNumber);
						BPR.EndEdit();
					}					
					
					/* post the negative budget, which will result in an empty a_glm_period.budget */
					PostBudget(ref APeriodDataTable, ref ABudgetTDS, ref ABudgetRow, ALedgerNumber);
					
					BPT = null;
					BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetRow.BudgetSequence, null);	

					for (int i = 0; i < BPT.Count; i++)
					{
						ABudgetPeriodRow BPR = (ABudgetPeriodRow)BPT.Rows[i];
						
						BPR.BeginEdit();
						BPR.BudgetThisYear = TempAmountsThisYear[BPR.PeriodNumber];
						BPR.BudgetNextYear = TempAmountsNextYear[BPR.PeriodNumber];
						BPR.EndEdit();
					}					
				}

				ABudgetRow.BudgetStatus = false; //i.e. unposted
				return true;
			}
			
			return false;
			
        }
        

        /// <summary>
        /// Post a budget
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ABudgetTDS"></param>
        /// <param name="ABudgetRow"></param>
        /// <param name="ALedgerNumber"></param>
        private static void PostBudget(ref DataTable APeriodDataTable, ref BudgetTDS ABudgetTDS, ref ABudgetRow ABudgetRow, int ALedgerNumber)
        {
        	/* post the negative budget, which will result in an empty a_glm_period.budget */	
        	//gb5300.p
			string AccountCode = ABudgetRow.AccountCode;

			string CostCentreList = ABudgetRow.CostCentreCode;  /* posting CC and parents */

        	//Populate list of affected Cost Centres
			CostCentreParentsList(ALedgerNumber, ref CostCentreList);
			
			bool NewTransaction = false;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
        	
        	//Locate the row for the current account
        	GLBatchTDS GLBatchDS = new GLBatchTDS();
        	
        	AAccountAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, transaction);	
        	
        	AAccountRow AccountRow = (AAccountRow)GLBatchDS.AAccount.Rows.Find(new object[] {ALedgerNumber, ABudgetRow.AccountCode});

			try
			{
				/* calculate values for budgets and store them in a temp table; uses lb_budget */
				ProcessAccountParent(ref APeriodDataTable, ALedgerNumber, ABudgetRow.AccountCode, AccountRow.DebitCreditIndicator, CostCentreList, ABudgetRow.BudgetSequence);
			}
        	finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
			
        }
        
        /// <summary>
        /// Process the account code parent codes
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="CurrAccountCode"></param>
        /// <param name="ADebitCreditIndicator"></param>
        /// <param name="ACostCentreList"></param>
        /// <param name="ABudgetSequence"></param>
        private static void ProcessAccountParent(ref DataTable APeriodDataTable, int ALedgerNumber, string CurrAccountCode, bool ADebitCreditIndicator, string ACostCentreList, int ABudgetSequence)
        {
			int GLMThisYear;
			int GLMNextYear;

			int DebitCreditMultiply; /* needed if the debit credit indicator is not the same */
			string CostCentreCode;

			AAccountTable a_current_account_b = null;    /* Current acct record */
			AAccountTable a_parent_account_b = null;     /* Parent acct record */
			AAccountHierarchyDetailTable AccountHierarchyDetailTable = null;
			AAccountHierarchyDetailRow AccountHierarchyDetailRow = null;
			
            bool NewTransaction = false;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
        	
        	//Locate the row for the current account
        	GLBatchTDS GLBatchDS = new GLBatchTDS();
        	
        	AAccountAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, transaction);	

        	ALedgerAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, transaction);
        	ALedgerRow LedgerRow = (ALedgerRow)GLBatchDS.ALedger.Rows[0];
        	
        	a_current_account_b = GLBatchDS.AAccount;
        	AAccountRow AccountRow = (AAccountRow)a_current_account_b.Rows.Find(new object[] {ALedgerNumber, CurrAccountCode});
        	
        	try
            {
	        	AccountHierarchyDetailTable = GLBatchDS.AAccountHierarchyDetail;
	        	AccountHierarchyDetailRow = (AAccountHierarchyDetailRow)AccountHierarchyDetailTable.Rows.Find(new object[] {ALedgerNumber, MFinanceConstants.ACCOUNT_HIERARCHY_STANDARD, CurrAccountCode});
	        	
	        	if (AccountHierarchyDetailRow != null)
	        	{
		        	string AccountCodeToReportTo = AccountHierarchyDetailRow.AccountCodeToReportTo;
		        	
		        	if (AccountCodeToReportTo != null && AccountCodeToReportTo != string.Empty)
		        	{
						a_parent_account_b = GLBatchDS.AAccount;
						AAccountRow AccountRowP = (AAccountRow)a_parent_account_b.Rows.Find(new object[] {ALedgerNumber, AccountCodeToReportTo});
		        		
					    /* Recursively call this procedure. */
					    ProcessAccountParent(ref APeriodDataTable, ALedgerNumber, AccountCodeToReportTo, ADebitCreditIndicator, ACostCentreList, ABudgetSequence);
		        	}
	        	}
	        	
	        	/* If the account has the same db/cr indicator as the original
				    account for which the budget was created, add the budget amount.
		   		   Otherwise, subtract. */
				if (AccountRow.DebitCreditIndicator = ADebitCreditIndicator)
				{
					DebitCreditMultiply = 1;
				}
				else
				{
					DebitCreditMultiply = -1;
				}
				
				string[] CostCentres = ACostCentreList.Split(':');
				string AccCode = AccountRow.AccountCode;
				int CurrYear = LedgerRow.CurrentFinancialYear;
				
				/* For each associated Cost Centre, update the General Ledger Master. */
				for (int i = 1; i < CostCentres.Length; i++)
				{
					CostCentreCode = CostCentres[i];
					
					GLMThisYear = GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear);
					GLMNextYear = GetGLMSequenceForBudget(ALedgerNumber, AccCode, CostCentreCode, CurrYear + 1);
				
				    /* If the posting CC/AC combination doesn't exist create it. */
				    if (GLMThisYear == -1)
				    {
						GLMThisYear = TGLPosting.CreateGLMYear(ref GLBatchDS, ALedgerNumber, CurrYear, AccCode, CostCentreCode);
				    }

				    if (GLMNextYear == -1)
				    {
						GLMNextYear = TGLPosting.CreateGLMYear(ref GLBatchDS, ALedgerNumber, CurrYear + 1, AccCode, CostCentreCode);
				    }
	
					/* Update totals for the General Ledger Master record. */
					ABudgetPeriodTable BPT = ABudgetPeriodAccess.LoadViaABudget(ABudgetSequence, null);
					ABudgetPeriodRow BPR = null;
					
					for (int j = 0; j < BPT.Count; j++)
					{
						BPR = (ABudgetPeriodRow)BPT.Rows[j];
						AddBudgetValue(ref APeriodDataTable, GLMThisYear, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetThisYear);
        				AddBudgetValue(ref APeriodDataTable, GLMNextYear, BPR.PeriodNumber, DebitCreditMultiply * BPR.BudgetNextYear);
					}
				}
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

        }
        
        /// <summary>
        /// Return the list of parent cost centre codes
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACurrentCostCentreList"></param>
        private static void CostCentreParentsList(int ALedgerNumber, ref string ACurrentCostCentreList)
        {
        	string ParentCostCentre;
			string CostCentreList = ACurrentCostCentreList;

 			TDBTransaction DBTransaction = DBAccess.GDBAccessObj.BeginTransaction();
        	
        	GLBatchTDS GLBatchDS = new GLBatchTDS();
        	
        	ACostCentreAccess.LoadViaALedger(GLBatchDS, ALedgerNumber, DBTransaction);
        	
        	//ACostCentreAccess.LoadByPrimaryKey(GLBatchDS, ALedgerNumber, ACurrentCostCentre, null);
        	ACostCentreRow CostCentreRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] {ALedgerNumber, ACurrentCostCentreList});
        	
			ParentCostCentre = CostCentreRow.CostCentreToReportTo;

			while (ParentCostCentre != string.Empty)
			{
				ACostCentreRow CCRow = (ACostCentreRow)GLBatchDS.ACostCentre.Rows.Find(new object[] {ALedgerNumber, ParentCostCentre});
				
				CostCentreList += ":" + CCRow.CostCentreCode;
    			ParentCostCentre = CCRow.CostCentreToReportTo;
			}			
        	
        	DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// Create the temp table that stores the GLMSequence and budget data
        /// </summary>
        /// <returns></returns>
        private static DataTable CreateTempTable()
        {
    		DataTable wtPeriodData = new DataTable();
			wtPeriodData.Columns.Add("GLMSequence", typeof(int));
			wtPeriodData.Columns.Add("PeriodNumber", typeof(int));
			wtPeriodData.Columns.Add("BudgetBase", typeof(decimal));
			wtPeriodData.PrimaryKey = new DataColumn[2] {wtPeriodData.Columns[0], wtPeriodData.Columns[1]};
			
			return wtPeriodData;
        }

        /// <summary>
        /// Write a budget value to the temporary table
        /// </summary>
        /// <param name="APeriodDataTable"></param>
        /// <param name="AGLMSequence"></param>
        /// <param name="APeriodNumber"></param>
        /// <param name="APeriodAmount"></param>
        private static void AddBudgetValue(ref DataTable APeriodDataTable, int AGLMSequence, int APeriodNumber, decimal APeriodAmount)
        {
        	/*  add a budget amount to the temp table wtPeriodData. 
			    if the record is not already in the temp table, it is fetched */
        	DataRow TempRow = (DataRow)APeriodDataTable.Rows.Find(new object[] {AGLMSequence, APeriodNumber});
        	
        	if (TempRow == null)
        	{
 				AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence, APeriodNumber, null);  
				AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;
 				
				if (GeneralLedgerMasterPeriodTable.Count > 0)
				{
					GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

					/* only create records for periods which have a value. try to keep the number of records low,
			           to make the lock count in the write transaction smaller */
					if (GeneralLedgerMasterPeriodRow.BudgetBase != 0)
					{
						DataRow DR = (DataRow)APeriodDataTable.NewRow();
						DR.ItemArray[0] = AGLMSequence;
						DR.ItemArray[1] = APeriodNumber;
						DR.ItemArray[2] = GeneralLedgerMasterPeriodRow.BudgetBase;
						
						APeriodDataTable.Rows.Add(DR);
					}
				}			
        	}
        	else
        	{
        		TempRow.BeginEdit();
        		TempRow.ItemArray[2] = Convert.ToDecimal(TempRow.ItemArray[2]) + APeriodAmount;
        		TempRow.EndEdit();
        	}

        }

		/// <summary>
        /// Reset the budget amount in the temp table wtPeriodData.
    	///   if the record is not already in the temp table, it is created empty
		/// </summary>
		/// <param name="ATempTable"></param>
		/// <param name="AGLMSequence"></param>
		/// <param name="APeriodNumber"></param>
        private static void ClearAllBudgetValues(ref DataTable ATempTable, int AGLMSequence, int APeriodNumber)
        {
        	DataRow TempRow = (DataRow)ATempTable.Rows.Find(new object[] {AGLMSequence, APeriodNumber});
        	
        	if (TempRow == null)
        	{
 				AGeneralLedgerMasterPeriodTable GeneralLedgerMasterPeriodTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(AGLMSequence, APeriodNumber, null);  
				AGeneralLedgerMasterPeriodRow GeneralLedgerMasterPeriodRow = null;
 				
				if (GeneralLedgerMasterPeriodTable.Count > 0)
				{
					GeneralLedgerMasterPeriodRow = (AGeneralLedgerMasterPeriodRow)GeneralLedgerMasterPeriodTable.Rows[0];

					/* only create records for periods which have a value. try to keep the number of records low,
			           to make the lock count in the write transaction smaller */
					if (GeneralLedgerMasterPeriodRow.BudgetBase != 0)
					{
						DataRow DR = (DataRow)ATempTable.NewRow();
						DR.ItemArray[0] = AGLMSequence;
						DR.ItemArray[1] = APeriodNumber;
						DR.ItemArray[2] = 0;
						
						ATempTable.Rows.Add(DR);
					}
				}			
        	}
        	else
        	{
        		TempRow.BeginEdit();
        		TempRow.ItemArray[2] = 0;
        		TempRow.EndEdit();
        	}

        }

        
    }
    
}