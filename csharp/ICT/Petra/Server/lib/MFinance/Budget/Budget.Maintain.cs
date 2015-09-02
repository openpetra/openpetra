//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//		 cthomas
//
// Copyright 2004-2013 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

using GNU.Gettext;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.IO;
using Ict.Common.Verification;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.GL.Data.Access;
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
        public static BudgetTDS LoadAllBudgets(Int32 ALedgerNumber)
        {
            BudgetTDS MainDS = new BudgetTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        //Load all by Ledger
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
                        ABudgetAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
                        ABudgetRevisionAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

                        ABudgetTable BudgetTable = new ABudgetTable();
                        ABudgetRow TemplateRow = (ABudgetRow)BudgetTable.NewRow();
                        TemplateRow.LedgerNumber = ALedgerNumber;

                        ABudgetPeriodAccess.LoadViaABudgetTemplate(MainDS, TemplateRow, Transaction);
                    });

                // Accept row changes here so that the Client gets 'unmodified' rows
                MainDS.AcceptChanges();

                // Remove all Tables that were not filled with data before remoting them.
                MainDS.RemoveEmptyTables();
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return MainDS;
        }

        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABudgetYear"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadBudgetsForYear(Int32 ALedgerNumber, Int32 ABudgetYear)
        {
            BudgetTDS MainDS = new BudgetTDS();
            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);

                        //Load all by Year
                        ABudgetTable BudgetTable = new ABudgetTable();
                        ABudgetRow TemplateRow = (ABudgetRow)BudgetTable.NewRow();
                        TemplateRow.Year = ABudgetYear;

                        ABudgetAccess.LoadUsingTemplate(MainDS, TemplateRow, Transaction);
                        ABudgetPeriodAccess.LoadViaABudgetTemplate(MainDS, TemplateRow, Transaction);
                        //TODO: add Budget Revision capability when decision made to add it to OP.
                        //  Assume Revision=0 for now until implemented
                        ABudgetRevisionAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABudgetYear, 0, Transaction);
                    });

                // Accept row changes here so that the Client gets 'unmodified' rows
                MainDS.AcceptChanges();

                // Remove all Tables that were not filled with data before remoting them.
                MainDS.RemoveEmptyTables();
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return MainDS;
        }

        /// <summary>
        /// save modified budgets
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-3")]
        public static TSubmitChangesResult SaveBudget(ref BudgetTDS AInspectDS)
        {
            if (AInspectDS != null)
            {
                BudgetTDSAccess.SubmitChanges(AInspectDS);

                return TSubmitChangesResult.scrOK;
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
        /// <param name="ARecordsUpdated"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>Total number of records imported and number of which updated as the fractional part</returns>
        [RequireModulePermission("FINANCE-3")]
        public static Int32 ImportBudgets(Int32 ALedgerNumber,
            Int32 ACurrentBudgetYear,
            string ACSVFileName,
            string[] AFdlgSeparator,
            ref BudgetTDS AImportDS,
            out Int32 ARecordsUpdated,
            out TVerificationResultCollection AVerificationResult)
        {
            ARecordsUpdated = 0;
            AVerificationResult = new TVerificationResultCollection();

            if (AImportDS != null)
            {
                int retVal = ImportBudgetFromCSV(ALedgerNumber,
                    ACurrentBudgetYear,
                    ACSVFileName,
                    AFdlgSeparator,
                    ref AImportDS,
                    ref ARecordsUpdated,
                    ref AVerificationResult);

                return retVal;
            }

            return 0;
        }

        private static CultureInfo FCultureInfoNumberFormat;

        /// <summary>
        /// Import the budget from a CSV file
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACurrentBudgetYear"></param>
        /// <param name="ACSVFileName"></param>
        /// <param name="AFdlgSeparator"></param>
        /// <param name="AImportDS"></param>
        /// <param name="ARecordsUpdated"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>Total number of records imported and number of which updated as the fractional part</returns>
        private static Int32 ImportBudgetFromCSV(Int32 ALedgerNumber,
            Int32 ACurrentBudgetYear,
            string ACSVFileName,
            string[] AFdlgSeparator,
            ref BudgetTDS AImportDS,
            ref Int32 ARecordsUpdated,
            ref TVerificationResultCollection AVerificationResult)
        {
            StreamReader DataFile = new StreamReader(ACSVFileName, System.Text.Encoding.Default);

            string Separator = AFdlgSeparator[0];
            string DateFormat = AFdlgSeparator[1];
            string NumberFormat = AFdlgSeparator[2];

            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            CultureInfo MyCultureInfoDate = new CultureInfo("en-GB");
            MyCultureInfoDate.DateTimeFormat.ShortDatePattern = DateFormat;

            // To store the From and To currencies
            // Use an array to store these to make for easy
            //   inverting of the two currencies when calculating
            //   the inverse value.

            //string currentBudgetVal = string.Empty;
            //string mess = string.Empty;
            ACostCentreTable CostCentreTable = null;
            AAccountTable AccountTable = null;

            string CostCentre = string.Empty;
            string Account = string.Empty;
            string BudgetType = string.Empty;
            string BudgetYearString = string.Empty;
            int BudgetYearNumber = 0;
            int BdgRevision = 0;  //not currently implementing versioning so always zero

            int NumPeriods = TAccountingPeriodsWebConnector.GetNumberOfPeriods(ALedgerNumber);
            decimal[] BudgetPeriods = new decimal[NumPeriods];

            int RowNumber = 0;

            ABudgetTable BudgetTableExistingAndImported = new ABudgetTable();

            while (!DataFile.EndOfStream)
            {
                decimal totalBudgetRowAmount = 0;

                try
                {
                    //Increment row number
                    RowNumber++;

                    string Line = DataFile.ReadLine();

                    CostCentre = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();

                    //Check if header row exists
                    if (CostCentre == "Cost Centre")
                    {
                        //Read the next line
                        Line = DataFile.ReadLine();
                        CostCentre = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();
                    }

                    //Read the values for the current line
                    //Account
                    Account = StringHelper.GetNextCSV(ref Line, Separator, false).ToString();
                    //BudgetType
                    BudgetType = StringHelper.GetNextCSV(ref Line, Separator, false).ToString().ToUpper();
                    BudgetType = BudgetType.Replace(" ", ""); //Ad hoc will become ADHOC

                    //Allow for variations on Inf.Base and Inf.N
                    if (BudgetType.Contains("INF"))
                    {
                        if (BudgetType.Contains("BASE"))
                        {
                            if (BudgetType != MFinanceConstants.BUDGET_INFLATE_BASE)
                            {
                                BudgetType = MFinanceConstants.BUDGET_INFLATE_BASE;
                            }
                        }
                        else if (BudgetType != MFinanceConstants.BUDGET_INFLATE_N)
                        {
                            BudgetType = MFinanceConstants.BUDGET_INFLATE_N;
                        }
                    }

                    //BudgetYear
                    BudgetYearString = (StringHelper.GetNextCSV(ref Line, Separator, false)).ToUpper();

                    //Check validity of CSV file line values
                    if (!ValidateKeyBudgetFields(ALedgerNumber,
                            RowNumber,
                            ref CostCentreTable,
                            ref AccountTable,
                            CostCentre,
                            Account,
                            BudgetType,
                            BudgetYearString,
                            ref AVerificationResult))
                    {
                        continue;
                    }

                    //Read the budgetperiod values to check if valid according to type
                    Array.Clear(BudgetPeriods, 0, NumPeriods);

                    if (!ProcessBudgetTypeImportDetails(ref Line, Separator, BudgetType, ref BudgetPeriods))
                    {
                        AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + RowNumber.ToString("0000")),
                                String.Format(Catalog.GetString(
                                        " The values in this budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') do not match the Budget Type: '{3}'!"),
                                    BudgetYearString,
                                    CostCentre,
                                    Account,
                                    BudgetType),
                                TResultSeverity.Resv_Noncritical));

                        continue;
                    }

                    //Calculate the budget Year
                    BudgetYearNumber = GetBudgetYearNumber(ALedgerNumber, BudgetYearString);

                    //Add budget revision record if there's not one already.
                    if (AImportDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, BudgetYearNumber, BdgRevision }) == null)
                    {
                        ABudgetRevisionRow BudgetRevisionRow = (ABudgetRevisionRow)AImportDS.ABudgetRevision.NewRowTyped();
                        BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                        BudgetRevisionRow.Year = BudgetYearNumber;
                        BudgetRevisionRow.Revision = BdgRevision;
                        BudgetRevisionRow.Description = "Budget Import from: " + ACSVFileName;
                        AImportDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
                    }

                    for (int i = 0; i < NumPeriods; i++)
                    {
                        totalBudgetRowAmount += BudgetPeriods[i];
                    }

                    BudgetTDS mainDS = new BudgetTDS();
                    TDBTransaction transaction = null;

                    DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        ref transaction,
                        delegate
                        {
                            //TODO: need to filter on ABudgetPeriod using LoadViaBudget or LoadViaUniqueKey
                            ABudgetAccess.LoadByUniqueKey(mainDS, ALedgerNumber, BudgetYearNumber, BdgRevision, CostCentre, Account, transaction);

                            #region Validate Data

                            if ((mainDS.ABudget != null) && (mainDS.ABudget.Count > 1))
                            {
                                //TODO: update when budget revisioning is added
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - Duplicates unique keys exist in the Budget table for Ledger: {1} Year: '{2}' ({3}), Cost Centre: '{4}' & Account: '{5}'!"),
                                        Utilities.GetMethodName(true),
                                        ALedgerNumber,
                                        BudgetYearString,
                                        BudgetYearNumber,
                                        CostCentre,
                                        Account));
                            }

                            #endregion Validate Data
                        });

                    //Check to see if the budget combination already exists:
                    if (mainDS.ABudget.Count > 0)
                    {
                        //Will only be one row
                        ABudgetRow br2 = (ABudgetRow)mainDS.ABudget.Rows[0];

                        //Check if exists in AImportDS
                        int bdgSeq = br2.BudgetSequence;

                        //Add to duplicates-checking table
                        //If not in saved budget table, check if already been imported earlier in the file
                        DataRow duplicateBudgetRow = BudgetTableExistingAndImported.Rows.Find(new object[] { bdgSeq });

                        if (duplicateBudgetRow != null)
                        {
                            //TODO: update when budget revisioning is added
                            AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + RowNumber.ToString("0000")),
                                    String.Format(Catalog.GetString(
                                            " This budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') is repeated in the import file!"),
                                        BudgetYearString,
                                        CostCentre,
                                        Account),
                                    TResultSeverity.Resv_Noncritical));

                            continue;
                        }

                        BudgetTableExistingAndImported.ImportRow(br2);

                        ABudgetRow bdgRow = (ABudgetRow)AImportDS.ABudget.Rows.Find(new object[] { bdgSeq });

                        if (bdgRow != null)
                        {
                            bool rowUpdated = false;

                            if (bdgRow.BudgetTypeCode != BudgetType)
                            {
                                rowUpdated = true;
                                bdgRow.BudgetTypeCode = BudgetType;
                            }

                            ABudgetPeriodRow BPRow = null;

                            for (int i = 0; i < NumPeriods; i++)
                            {
                                BPRow = (ABudgetPeriodRow)AImportDS.ABudgetPeriod.Rows.Find(new object[] { bdgSeq, i + 1 });

                                if ((BPRow != null) && (BPRow.BudgetBase != BudgetPeriods[i]))
                                {
                                    rowUpdated = true;
                                    BPRow.BudgetBase = BudgetPeriods[i];
                                }

                                BPRow = null;
                            }

                            if (rowUpdated)
                            {
                                ARecordsUpdated++;
                            }
                        }
                    }
                    else
                    {
                        //If not in saved budget table, check if already been imported earlier in the file
                        DataRow[] duplicateBudgetRows =
                            BudgetTableExistingAndImported.Select(String.Format("{0}={1} And {2}={3} And {4}={5} And {6}='{7}' And {8}='{9}'",
                                    ABudgetTable.GetLedgerNumberDBName(),
                                    ALedgerNumber,
                                    ABudgetTable.GetYearDBName(),
                                    BudgetYearNumber,
                                    ABudgetTable.GetRevisionDBName(),
                                    BdgRevision,
                                    ABudgetTable.GetCostCentreCodeDBName(),
                                    CostCentre,
                                    ABudgetTable.GetAccountCodeDBName(),
                                    Account));

                        if ((duplicateBudgetRows != null) && (duplicateBudgetRows.Length > 0))
                        {
                            //TODO: update when budget revisioning is added
                            AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + RowNumber.ToString("0000")),
                                    String.Format(Catalog.GetString(
                                            "This budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') is repeated in the import file!"),
                                        BudgetYearString,
                                        CostCentre,
                                        Account),
                                    TResultSeverity.Resv_Noncritical));

                            continue;
                        }

                        //Add the new budget row
                        ABudgetRow BudgetRow = (ABudgetRow)AImportDS.ABudget.NewRowTyped();
                        int newSequence = Convert.ToInt32(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_budget)); // -1 * (AImportDS.ABudget.Rows.Count + 1);

                        BudgetRow.BudgetSequence = newSequence;
                        BudgetRow.LedgerNumber = ALedgerNumber;
                        BudgetRow.Year = BudgetYearNumber;
                        BudgetRow.Revision = BdgRevision;
                        BudgetRow.CostCentreCode = CostCentre;
                        BudgetRow.AccountCode = Account;
                        BudgetRow.BudgetTypeCode = BudgetType;
                        AImportDS.ABudget.Rows.Add(BudgetRow);

                        //Add to import table to check for later duplicates
                        BudgetTableExistingAndImported.ImportRow(BudgetRow);

                        //Add the budget periods
                        for (int i = 0; i < NumPeriods; i++)
                        {
                            ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)AImportDS.ABudgetPeriod.NewRowTyped();
                            BudgetPeriodRow.BudgetSequence = newSequence;
                            BudgetPeriodRow.PeriodNumber = i + 1;
                            BudgetPeriodRow.BudgetBase = BudgetPeriods[i];
                            AImportDS.ABudgetPeriod.Rows.Add(BudgetPeriodRow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                            Utilities.GetMethodSignature(),
                            Environment.NewLine,
                            ex.Message));
                    throw ex;
                }
            }

            DataFile.Close();

            return RowNumber;
        }

        private static bool ValidateKeyBudgetFields(int ALedgerNumber,
            int ARowNumber,
            ref ACostCentreTable ACostCentreTbl,
            ref AAccountTable AAccountTbl,
            string ACostCentre,
            string AAccount,
            string ABudgetType,
            string ABudgetYearString,
            ref TVerificationResultCollection AVerificationResult)
        {
            string VerifMessage = string.Empty;

            ACostCentreTable CCTable = ACostCentreTbl;
            AAccountTable AccTable = AAccountTbl;

            ACostCentreRow CostCentreRow = null;
            AAccountRow AccountRow = null;

            if (CCTable == null)
            {
                try
                {
                    TDBTransaction Transaction = null;

                    DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        ref Transaction,
                        delegate
                        {
                            CCTable = ACostCentreAccess.LoadViaALedger(ALedgerNumber, Transaction);
                            AccTable = AAccountAccess.LoadViaALedger(ALedgerNumber, Transaction);

                            #region Validate Data

                            if ((CCTable == null) || (CCTable.Count == 0))
                            {
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - Cost Centre data for Ledger number {1} does not exist or could not be accessed!"),
                                        Utilities.GetMethodName(true),
                                        ALedgerNumber));
                            }
                            else if ((AccTable == null) || (AccTable.Count == 0))
                            {
                                throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                            "Function:{0} - Account data for Ledger number {1} does not exist or could not be accessed!"),
                                        Utilities.GetMethodName(true),
                                        ALedgerNumber));
                            }

                            #endregion Validate Data
                        });

                    ACostCentreTbl = CCTable;
                    AAccountTbl = AccTable;
                }
                catch (Exception ex)
                {
                    TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                            Utilities.GetMethodSignature(),
                            Environment.NewLine,
                            ex.Message));
                    throw ex;
                }
            }

            //Check for missing or inactive cost centre
            CostCentreRow = (ACostCentreRow)ACostCentreTbl.Rows.Find(new object[] { ALedgerNumber, ACostCentre });

            if (CostCentreRow == null)
            {
                VerifMessage += String.Format(Catalog.GetString(" Cost Centre: '{0}' does not exist."), ACostCentre);
            }
            else if (!CostCentreRow.CostCentreActiveFlag)
            {
                VerifMessage += String.Format(Catalog.GetString(" Cost Centre: '{0}' is currently inactive."), ACostCentre);
            }

            //Check for missing or inactive account code
            AccountRow = (AAccountRow)AAccountTbl.Rows.Find(new object[] { ALedgerNumber, AAccount });

            if (AccountRow == null)
            {
                VerifMessage += String.Format(Catalog.GetString(" Account: '{0}' does not exist."), AAccount);
            }
            else if (!AccountRow.AccountActiveFlag)
            {
                VerifMessage += String.Format(Catalog.GetString(" Account: '{0}' is currently inactive."), AAccount);
            }

            //Check Budget Type
            if ((ABudgetType != MFinanceConstants.BUDGET_ADHOC)
                && (ABudgetType != MFinanceConstants.BUDGET_SAME)
                && (ABudgetType != MFinanceConstants.BUDGET_INFLATE_N)
                && (ABudgetType != MFinanceConstants.BUDGET_SPLIT)
                && (ABudgetType != MFinanceConstants.BUDGET_INFLATE_BASE))
            {
                VerifMessage += String.Format(Catalog.GetString(" Budget Type: '{0}' does not exist."), ABudgetType);
            }

            //Check Budget Year
            if ((ABudgetYearString != "THIS") && (ABudgetYearString != "NEXT"))
            {
                VerifMessage += String.Format(Catalog.GetString(" Budget Year: '{0}' should be 'THIS' or 'NEXT'."), ABudgetYearString);
            }

            //Check if errors have occurred
            if (VerifMessage.Length > 0)
            {
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + ARowNumber.ToString("0000")),
                        VerifMessage,
                        TResultSeverity.Resv_Noncritical));

                return false;
            }
            else
            {
                return true;
            }
        }

        private static Int32 GetBudgetYearNumber(int ALedgerNumber, string ABudgetYearName)
        {
            int RetVal = 0;
            int BudgetYear = 0;

            ALedgerTable LedgerTable = null;
            AAccountingPeriodTable AccPeriodTable = null;

            TDBTransaction Transaction = null;

            try
            {
                DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                        AccPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, 1, Transaction);
                    });

                #region Validate Data

                if ((LedgerTable == null) || (LedgerTable.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }
                else if ((AccPeriodTable == null) || (AccPeriodTable.Count == 0))
                {
                    throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                "Function:{0} - Accounting Period data for Ledger number {1} does not exist or could not be accessed!"),
                            Utilities.GetMethodName(true),
                            ALedgerNumber));
                }

                #endregion Validate Data

                ALedgerRow ledgerRow = (ALedgerRow)LedgerTable.Rows[0];
                AAccountingPeriodRow accPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];

                BudgetYear = accPeriodRow.PeriodStartDate.Year;

                if (ABudgetYearName.ToUpper() != "THIS")
                {
                    BudgetYear++;
                }

                DateTime currentYearEnd = TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber,
                    ledgerRow.CurrentFinancialYear,
                    0,
                    ledgerRow.NumberOfAccountingPeriods);

                RetVal = (BudgetYear - currentYearEnd.Year + ledgerRow.CurrentFinancialYear);
            }
            catch (Exception ex)
            {
                TLogging.Log(String.Format("Method:{0} - Unexpected error!{1}{1}{2}",
                        Utilities.GetMethodSignature(),
                        Environment.NewLine,
                        ex.Message));
                throw ex;
            }

            return RetVal;
        }

        private static string BudgetRevisionYearName(int ALedgerNumber, int ABudgetRevisionYear)
        {
            int budgetYear = 0;
            ALedgerTable LedgerTable = null;
            AAccountingPeriodTable accPeriodTable = null;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                    accPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, 1, Transaction);
                });

            ALedgerRow ledgerRow = (ALedgerRow)LedgerTable.Rows[0];
            AAccountingPeriodRow accPeriodRow = (AAccountingPeriodRow)accPeriodTable.Rows[0];

            DateTime CurrentYearEnd = TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber,
                ledgerRow.CurrentFinancialYear,
                0,
                ledgerRow.NumberOfAccountingPeriods);

            budgetYear = ABudgetRevisionYear + CurrentYearEnd.Year - ledgerRow.CurrentFinancialYear;

            if (budgetYear == accPeriodRow.PeriodStartDate.Year)
            {
                return "This";
            }
            else
            {
                return "Next";
            }
        }

        private static bool ProcessBudgetTypeImportDetails(ref string Line, string Separator, string ABudgetType, ref decimal[] ABudgetPeriods)
        {
            bool RetVal = true;

            int NumPeriods = ABudgetPeriods.Length;

            try
            {
                switch (ABudgetType)
                {
                    case MFinanceConstants.BUDGET_SAME:

                        string periodAmountString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal periodAmount = Convert.ToDecimal(periodAmountString, FCultureInfoNumberFormat);

                        for (int i = 0; i < NumPeriods; i++)
                        {
                            ABudgetPeriods[i] = periodAmount;
                        }

                        break;

                    case MFinanceConstants.BUDGET_SPLIT:

                        string totalAmountString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal totalAmount = Convert.ToDecimal(totalAmountString, FCultureInfoNumberFormat);
                        decimal perPeriodAmount = Math.Truncate(totalAmount / NumPeriods);
                        decimal lastPeriodAmount = totalAmount - perPeriodAmount * (NumPeriods - 1);

                        //Write to Budget rows
                        for (int i = 0; i < NumPeriods; i++)
                        {
                            if (i < (NumPeriods - 1))
                            {
                                ABudgetPeriods[i] = perPeriodAmount;
                            }
                            else
                            {
                                ABudgetPeriods[i] = lastPeriodAmount;
                            }
                        }

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_BASE:

                        string period1AmountString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal period1Amount = Convert.ToDecimal(period1AmountString, FCultureInfoNumberFormat);
                        string periodNPercentString = string.Empty;

                        ABudgetPeriods[0] = period1Amount;

                        for (int i = 1; i < NumPeriods; i++)
                        {
                            periodNPercentString = StringHelper.GetNextCSV(ref Line, Separator, false);
                            ABudgetPeriods[i] = ABudgetPeriods[i - 1] * (1 + (Convert.ToDecimal(periodNPercentString, FCultureInfoNumberFormat) / 100));
                        }

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_N:

                        string periodStartAmountString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal periodStartAmount = Convert.ToDecimal(periodStartAmountString, FCultureInfoNumberFormat);

                        string inflateAfterPeriodString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal inflateAfterPeriod = Convert.ToDecimal(inflateAfterPeriodString, FCultureInfoNumberFormat);

                        string inflationRateString = StringHelper.GetNextCSV(ref Line, Separator, false);
                        decimal inflationRate = (Convert.ToDecimal(inflationRateString, FCultureInfoNumberFormat)) / 100;

                        decimal subsequentPeriodsAmount = periodStartAmount * (1 + inflationRate);

                        //Control the inflate after period number
                        if (inflateAfterPeriod < 0)
                        {
                            inflateAfterPeriod = 0;
                        }
                        else if (inflateAfterPeriod >= NumPeriods)
                        {
                            inflateAfterPeriod = (NumPeriods - 1);
                        }

                        //Write the period values
                        for (int i = 0; i < NumPeriods; i++)
                        {
                            if (i <= (inflateAfterPeriod - 1))
                            {
                                ABudgetPeriods[i] = periodStartAmount;
                            }
                            else
                            {
                                ABudgetPeriods[i] = subsequentPeriodsAmount;
                            }
                        }

                        break;

                    default:                              //MFinanceConstants.BUDGET_ADHOC:

                        string periodNAmount = string.Empty;

                        for (int i = 0; i < NumPeriods; i++)
                        {
                            periodNAmount = StringHelper.GetNextCSV(ref Line, Separator, false);
                            ABudgetPeriods[i] = Convert.ToDecimal(periodNAmount, FCultureInfoNumberFormat);
                        }

                        break;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Exports budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACSVFileName"></param>
        /// <param name="AFdlgSeparator"></param>
        /// <param name="AFileContents"></param>
        /// <param name="AExportDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>Total number of records exported</returns>
        [RequireModulePermission("FINANCE-3")]
        public static int ExportBudgets(Int32 ALedgerNumber,
            string ACSVFileName,
            string[] AFdlgSeparator,
            ref string AFileContents,
            ref BudgetTDS AExportDS,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            if (AExportDS != null)
            {
                int retVal = ExportBudgetToCSV(ALedgerNumber,
                    ACSVFileName,
                    AFdlgSeparator,
                    ref AFileContents,
                    ref AExportDS,
                    ref AVerificationResult);
                return retVal;
            }

            return 0;
        }

        private static Int32 ExportBudgetToCSV(Int32 ALedgerNumber,
            string ACSVFileName,
            string[] AFdlgSeparator,
            ref string AFileContents,
            ref BudgetTDS AExportDS,
            ref TVerificationResultCollection AVerificationResult)
        {
            Int32 numBudgetsExported = 0;

            ALedgerRow lr = (ALedgerRow)AExportDS.ALedger.Rows[0];

            ABudgetPeriodTable budgetPeriod = (ABudgetPeriodTable)AExportDS.ABudgetPeriod;

            Int32 numPeriods = lr.NumberOfAccountingPeriods;

            char separator = AFdlgSeparator[0].Substring(0, 1).ToCharArray()[0];

            TLogging.Log("Writing file: " + ACSVFileName);

            StringBuilder sb = new StringBuilder();
            string budgetAmounts = string.Empty;

            foreach (ABudgetRow row in AExportDS.ABudget.Rows)
            {
                switch (row.BudgetTypeCode)
                {
                    case MFinanceConstants.BUDGET_SAME:
                        StringBudgetTypeSameAmounts(row.BudgetSequence, ref budgetPeriod, out budgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_SPLIT:
                        StringBudgetTypeSplitAmounts(row.BudgetSequence, numPeriods, ref budgetPeriod, separator, out budgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_BASE:
                        StringBudgetTypeInflateBaseAmounts(row.BudgetSequence, numPeriods, ref budgetPeriod, separator, out budgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_N:
                        StringBudgetTypeInflateNAmounts(row.BudgetSequence, numPeriods, ref budgetPeriod, separator, out budgetAmounts);

                        break;

                    default:                              //MFinanceConstants.BUDGET_ADHOC:
                        StringBudgetTypeAdhocAmounts(row.BudgetSequence, numPeriods, ref budgetPeriod, separator, out budgetAmounts);

                        break;
                }

                sb.Append(StringHelper.StrMerge(
                        new string[] {
                            Encase(row.CostCentreCode),
                            Encase(row.AccountCode),
                            Encase(row.BudgetTypeCode),
                            Encase(BudgetRevisionYearName(ALedgerNumber, row.Year))
                        }, separator));

                sb.Append(separator.ToString());
                sb.Append(budgetAmounts);
                sb.Append(Environment.NewLine);

                numBudgetsExported++;
            }

            AFileContents = sb.ToString();

            return numBudgetsExported;
        }

        private static string Encase(string AStringToEncase)
        {
            return "\"" + AStringToEncase + "\"";
        }

        private static void StringBudgetTypeSplitAmounts(int ABudgetSequence,
            int ANumPeriods,
            ref ABudgetPeriodTable ABudgetPeriod,
            char ASeparator,
            out String ASb)
        {
            ABudgetPeriodRow budgetPeriodRow;

            ASb = string.Empty;
            decimal perPeriodAmount = 0;
            decimal endPeriodAmount = 0;

            //Find periods 1-(total periods-1) amount
            budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, 1 });

            if (budgetPeriodRow != null)
            {
                perPeriodAmount = budgetPeriodRow.BudgetBase;
                budgetPeriodRow = null;

                //Find period FNumberOfPeriods amount
                budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence,
                                                                                           ANumPeriods });

                if (budgetPeriodRow != null)
                {
                    endPeriodAmount = budgetPeriodRow.BudgetBase;
                }
            }

            //Calculate the total amount
            ASb += (perPeriodAmount * (ANumPeriods - 1) + endPeriodAmount).ToString();
        }

        private static void StringBudgetTypeAdhocAmounts(int ABudgetSequence,
            int ANumPeriods,
            ref ABudgetPeriodTable ABudgetPeriod,
            char ASeparator,
            out String ASb)
        {
            ABudgetPeriodRow budgetPeriodRow;

            ASb = string.Empty;

            for (int i = 1; i <= ANumPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    ASb += budgetPeriodRow.BudgetBase.ToString();

                    if (i < ANumPeriods)
                    {
                        ASb += ASeparator.ToString();
                    }
                }

                budgetPeriodRow = null;
            }
        }

        private static void StringBudgetTypeSameAmounts(int ABudgetSequence, ref ABudgetPeriodTable ABudgetPeriod, out String ASb)
        {
            ABudgetPeriodRow budgetPeriodRow;

            ASb = string.Empty;

            budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, 1 });

            if (budgetPeriodRow != null)
            {
                ASb += budgetPeriodRow.BudgetBase.ToString();
            }
        }

        private static void StringBudgetTypeInflateBaseAmounts(int ABudgetSequence,
            int ANumPeriods,
            ref ABudgetPeriodTable ABudgetPeriod,
            char ASeparator,
            out String ASb)
        {
            ABudgetPeriodRow budgetPeriodRow;

            ASb = string.Empty;

            decimal priorPeriodAmount = 0;
            decimal currentPeriodAmount = 0;

            for (int i = 1; i <= ANumPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    currentPeriodAmount = budgetPeriodRow.BudgetBase;

                    if (i == 1)
                    {
                        ASb += currentPeriodAmount.ToString();
                    }
                    else
                    {
                        ASb += ((currentPeriodAmount - priorPeriodAmount) / priorPeriodAmount * 100).ToString();
                    }

                    if (i < ANumPeriods)
                    {
                        ASb += ASeparator.ToString();
                    }

                    priorPeriodAmount = currentPeriodAmount;
                }

                budgetPeriodRow = null;
            }
        }

        private static void StringBudgetTypeInflateNAmounts(int ABudgetSequence,
            int ANumPeriods,
            ref ABudgetPeriodTable ABudgetPeriod,
            char ASeparator,
            out String ASb)
        {
            ABudgetPeriodRow budgetPeriodRow;

            ASb = string.Empty;

            decimal firstPeriodAmount = 0;
            decimal currentPeriodAmount;

            for (int i = 1; i <= ANumPeriods; i++)
            {
                budgetPeriodRow = (ABudgetPeriodRow)ABudgetPeriod.Rows.Find(new object[] { ABudgetSequence, i });

                if (budgetPeriodRow != null)
                {
                    currentPeriodAmount = budgetPeriodRow.BudgetBase;

                    if (i == 1)
                    {
                        firstPeriodAmount = currentPeriodAmount;
                        ASb += currentPeriodAmount.ToString();
                        ASb += ASeparator.ToString();
                    }
                    else
                    {
                        if (currentPeriodAmount != firstPeriodAmount)
                        {
                            ASb += (i - 1).ToString();
                            ASb += ASeparator.ToString();
                            ASb += ((currentPeriodAmount - firstPeriodAmount) / firstPeriodAmount * 100).ToString();
                            break;
                        }
                        else if (i == ANumPeriods)     // and by implication CurrentPeriodAmount == FirstPeriodAmount
                        {
                            //This is an odd case that the user should never implement, but still needs to be covered.
                            //  It is equivalent to using BUDGET TYPE: SAME
                            ASb += "0";
                            ASb += ASeparator.ToString();
                            ASb += "0";
                        }
                    }
                }

                budgetPeriodRow = null;
            }
        }

        private static bool IsZero(decimal d)
        {
            return d == 0;
        }

        /// <summary>
        /// Validate Budget Type: Same
        /// </summary>
        /// <param name="APeriodValues"></param>
        /// <param name="ANumberOfPeriods"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeSame(Int32 ANumberOfPeriods, decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;

            decimal Period1Amount = APeriodValues[0];

            for (int i = 1; i < ANumberOfPeriods; i++)
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
        /// <param name="ANumberOfPeriods"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeSplit(Int32 ANumberOfPeriods, decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;

            decimal Period1Amount = APeriodValues[0];

            for (int i = 1; i < (ANumberOfPeriods - 1); i++)
            {
                if (Period1Amount != APeriodValues[i])
                {
                    PeriodValuesOK = false;
                    break;
                }
            }

            if (PeriodValuesOK)
            {
                if ((APeriodValues[ANumberOfPeriods - 1] < Period1Amount)
                    || ((APeriodValues[ANumberOfPeriods - 1] - Period1Amount) >= ANumberOfPeriods))
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
        /// <param name="ANumberOfPeriods"></param>
        /// <returns></returns>
        private static bool ValidateBudgetTypeInflateN(Int32 ANumberOfPeriods, decimal[] APeriodValues)
        {
            bool PeriodValuesOK = true;
            bool PeriodAmountHasChanged = false;

            decimal Period1Amount = APeriodValues[0];

            if (Period1Amount == 0)
            {
                PeriodValuesOK = false;
                return PeriodValuesOK;
            }

            for (int i = 1; i < ANumberOfPeriods; i++)
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
    }
}