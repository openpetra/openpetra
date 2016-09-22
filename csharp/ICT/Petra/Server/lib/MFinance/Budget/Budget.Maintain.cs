//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       cthomas
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
        private static CultureInfo FCultureInfoNumberFormat;

        /// <summary>
        /// load budgets
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static BudgetTDS LoadAllBudgetsForExport(Int32 ALedgerNumber)
        {
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }

            #endregion Validate Arguments

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

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        int CurrentFinancialYear = MainDS.ALedger[0].CurrentFinancialYear;
                        int NextFinancialYear = CurrentFinancialYear + 1;

                        //Load all by Ledger but none may exist
                        ABudgetTable BudgetTable = new ABudgetTable();
                        ABudgetRow TemplateRow = (ABudgetRow)BudgetTable.NewRow();

                        TemplateRow.LedgerNumber = ALedgerNumber;
                        TemplateRow.Year = CurrentFinancialYear;

                        StringCollection Operators = StringHelper.InitStrArr(new string[] { "=", "<=" });
                        StringCollection OrderList = new StringCollection();

                        OrderList.Add("ORDER BY");
                        OrderList.Add(ABudgetTable.GetCostCentreCodeDBName() + " ASC");
                        OrderList.Add(ABudgetTable.GetAccountCodeDBName() + " ASC");
                        OrderList.Add(ABudgetTable.GetYearDBName() + " DESC");

                        ABudgetAccess.LoadUsingTemplate(MainDS, TemplateRow, Operators, null, Transaction, OrderList, 0, 0);
                        ABudgetPeriodAccess.LoadViaABudgetTemplate(MainDS, TemplateRow, Operators, null, Transaction, OrderList, 0, 0);

                        ABudgetRevisionAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, CurrentFinancialYear, 0, Transaction);
                        ABudgetRevisionAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, NextFinancialYear, 0, Transaction);
                    });

                // Accept row changes here so that the Client gets 'unmodified' rows
                MainDS.AcceptChanges();

                // Remove all Tables that were not filled with data before remoting them.
                MainDS.RemoveEmptyTables();
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ABudgetYear < 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The Budget Year number cannot be negative!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

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

                        #region Validate Data

                        if ((MainDS.ALedger == null) || (MainDS.ALedger.Count == 0))
                        {
                            throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                        "Function:{0} - Ledger data for Ledger number {1} does not exist or could not be accessed!"),
                                    Utilities.GetMethodName(true),
                                    ALedgerNumber));
                        }

                        #endregion Validate Data

                        int numPeriods = MainDS.ALedger[0].NumberOfAccountingPeriods;

                        //Load all by Year
                        LoadABudgetByYearWithCustomColumns(MainDS, ALedgerNumber, ABudgetYear, numPeriods, Transaction);

                        //Load budget period data
                        ABudgetTable BudgetTable = new ABudgetTable();
                        ABudgetRow TemplateRow = (ABudgetRow)BudgetTable.NewRow();
                        TemplateRow.Year = ABudgetYear;

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
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return MainDS;
        }

        private static void LoadABudgetByYearWithCustomColumns(BudgetTDS AMainDS,
            Int32 ALedgerNumber,
            Int32 AYear,
            Int32 ANumberOfPeriods,
            TDBTransaction ATransaction)
        {
            //Parameters for SQL as strings
            string prmLedgerNumber = ALedgerNumber.ToString();
            string prmYear = AYear.ToString();

            //Tables with alias
            string BudgetTableAlias = "b";
            string bBudgetTable = ABudgetTable.GetTableDBName() + " " + BudgetTableAlias;
            string BudgetPeriodTableAlias = "bp";
            string bpBudgetPeriodTable = ABudgetPeriodTable.GetTableDBName() + " " + BudgetPeriodTableAlias;

            //Table: ABudgetTable and fields
            string bLedgerNumber = BudgetTableAlias + "." + ABudgetTable.GetLedgerNumberDBName();
            string bBudgetSequence = BudgetTableAlias + "." + ABudgetTable.GetBudgetSequenceDBName();
            string bYear = BudgetTableAlias + "." + ABudgetTable.GetYearDBName();
            string bRevision = BudgetTableAlias + "." + ABudgetTable.GetRevisionDBName();
            string bCostCentreCode = BudgetTableAlias + "." + ABudgetTable.GetCostCentreCodeDBName();
            string bAccountCode = BudgetTableAlias + "." + ABudgetTable.GetAccountCodeDBName();
            string bBudgetTypeCode = BudgetTableAlias + "." + ABudgetTable.GetBudgetTypeCodeDBName();
            string bBudgetStatus = BudgetTableAlias + "." + ABudgetTable.GetBudgetStatusDBName();
            string bComment = BudgetTableAlias + "." + ABudgetTable.GetCommentDBName();
            string bDateCreated = BudgetTableAlias + "." + ABudgetTable.GetDateCreatedDBName();
            string bCreatedBy = BudgetTableAlias + "." + ABudgetTable.GetCreatedByDBName();
            string bDateModified = BudgetTableAlias + "." + ABudgetTable.GetDateModifiedDBName();
            string bModifiedBy = BudgetTableAlias + "." + ABudgetTable.GetModifiedByDBName();
            string bModificationId = BudgetTableAlias + "." + ABudgetTable.GetModificationIdDBName();

            //Table: ABudgetPeriodTable and fields
            string bpPeriodNumber = BudgetPeriodTableAlias + "." + ABudgetPeriodTable.GetPeriodNumberDBName();
            string bpBudgetBase = BudgetPeriodTableAlias + "." + ABudgetPeriodTable.GetBudgetBaseDBName();
            string bpBudgetSequence = BudgetPeriodTableAlias + "." + ABudgetPeriodTable.GetBudgetSequenceDBName();

            string SQLStatement =
                "select " +
                bBudgetSequence + ", " + bLedgerNumber + ", " + bYear + ", " +
                bRevision + ", " + bCostCentreCode + ", " + bAccountCode + ", " + bBudgetTypeCode + ", " +
                bBudgetStatus + ", " + bComment + ", " + bDateCreated + ", " + bCreatedBy + ", " +
                bDateModified + ", " + bModifiedBy + ", " + bModificationId + ", " +
                "min(case when " + bpPeriodNumber + " = 1 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period01Amount, " +
                "min(case when " + bpPeriodNumber + " = 2 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period02Amount, " +
                "min(case when " + bpPeriodNumber + " = 3 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period03Amount, " +
                "min(case when " + bpPeriodNumber + " = 4 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period04Amount, " +
                "min(case when " + bpPeriodNumber + " = 5 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period05Amount, " +
                "min(case when " + bpPeriodNumber + " = 6 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period06Amount, " +
                "min(case when " + bpPeriodNumber + " = 7 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period07Amount, " +
                "min(case when " + bpPeriodNumber + " = 8 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period08Amount, " +
                "min(case when " + bpPeriodNumber + " = 9 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period09Amount, " +
                "min(case when " + bpPeriodNumber + " = 10 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period10Amount, " +
                "min(case when " + bpPeriodNumber + " = 11 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period11Amount, " +
                "min(case when " + bpPeriodNumber + " = 12 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period12Amount";

            if (ANumberOfPeriods > 12)
            {
                SQLStatement += ", min(case when " + bpPeriodNumber + " = 13 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period13Amount";
            }
            else
            {
                SQLStatement += ", CAST(0 AS NUMERIC(24,10)) Period13Amount";
            }

            if (ANumberOfPeriods > 13)
            {
                SQLStatement += ", min(case when " + bpPeriodNumber + " = 14 then CAST(" + bpBudgetBase + " AS NUMERIC(24,10)) end) Period14Amount";
            }
            else
            {
                SQLStatement += ", CAST(0 AS NUMERIC(24,10)) Period14Amount";
            }

            SQLStatement +=
                " from " + bBudgetTable + " join " + bpBudgetPeriodTable +
                "   on " + bpBudgetSequence + " = " + bBudgetSequence +
                " where " + bLedgerNumber + " = " + prmLedgerNumber +
                "  and " + bYear + " = " + prmYear +
                " group by " +
                bBudgetSequence + ", " + bLedgerNumber + ", " + bYear + ", " +
                bRevision + ", " + bCostCentreCode + ", " + bAccountCode + ", " + bBudgetTypeCode + ", " +
                bBudgetStatus + ", " + bComment + ", " + bDateCreated + ", " + bCreatedBy + ", " +
                bDateModified + ", " + bModifiedBy + ", " + bModificationId + ";";

            DBAccess.GDBAccessObj.Select(AMainDS, SQLStatement, AMainDS.ABudget.TableName, ATransaction);
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
        /// Import the budget from a CSV file
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AImportString"></param>
        /// <param name="ACSVFileName"></param>
        /// <param name="AFdlgSeparator"></param>
        /// <param name="AImportDS"></param>
        /// <param name="ABudgetsAdded"></param>
        /// <param name="ABudgetsUpdated"></param>
        /// <param name="ABudgetsFailed"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>Total number of records imported. Also shows number updated or failed.</returns>
        [RequireModulePermission("FINANCE-3")]
        public static Int32 ImportBudgets(Int32 ALedgerNumber,
            string AImportString,
            string ACSVFileName,
            string[] AFdlgSeparator,
            ref BudgetTDS AImportDS,
            out Int32 ABudgetsAdded,
            out Int32 ABudgetsUpdated,
            out Int32 ABudgetsFailed,
            out TVerificationResultCollection AVerificationResult)
        {
            ABudgetsAdded = 0;
            ABudgetsUpdated = 0;
            ABudgetsFailed = 0;
            TVerificationResultCollection VerificationResult = new TVerificationResultCollection();
            AVerificationResult = VerificationResult;

            if (AImportDS == null)
            {
                return 0;
            }

            BudgetTDS ImportDS = AImportDS;
            StringReader sr = new StringReader(AImportString);

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
            ACostCentreTable CostCentreTable = new ACostCentreTable();
            AAccountTable AccountTable = new AAccountTable();

            int BudgetYearNumber = 0;
            int BdgRevision = 0;  //not currently implementing versioning so always zero

            Int32 BudgetsAdded = 0;
            Int32 BudgetsUpdated = 0;
            Int32 BudgetsFailed = 0;

            int NumPeriods = TAccountingPeriodsWebConnector.GetNumberOfPeriods(ALedgerNumber);
            decimal[] BudgetPeriods = new decimal[NumPeriods];

            int RowNumber = 0;
            Boolean hasAccountCodeProblem = false;

            ABudgetTable BudgetTableExistingAndImported = new ABudgetTable();

            TDBTransaction transaction = null;

            // Go round a loop reading the file line by line
            string ImportLine = sr.ReadLine();

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                ref transaction,
                delegate
                {
                    while (ImportLine != null)
                    {
                        decimal totalBudgetRowAmount = 0;

                        try
                        {
                            String CostCentre = StringHelper.GetNextCSV(ref ImportLine, Separator, false).ToString();

                            //Check if header row exists
                            if ((CostCentre == "Cost Centre") || string.IsNullOrEmpty(ImportLine))
                            {
                                continue;
                            }

                            //Increment row number
                            RowNumber++;

                            //Read the values for the current line
                            //Account
                            String Account = StringHelper.GetNextCSV(ref ImportLine, Separator, false).ToString();
                            //BudgetType
                            String BudgetTypeUpper = StringHelper.GetNextCSV(ref ImportLine, Separator, false).ToString().ToUpper();
                            BudgetTypeUpper = BudgetTypeUpper.Replace(" ", ""); //Ad hoc will become ADHOC

                            //Allow for variations on Inf.Base and Inf.N
                            if (BudgetTypeUpper.Contains("INF"))
                            {
                                if (BudgetTypeUpper.Contains("BASE"))
                                {
                                    if (BudgetTypeUpper != MFinanceConstants.BUDGET_INFLATE_BASE)
                                    {
                                        BudgetTypeUpper = MFinanceConstants.BUDGET_INFLATE_BASE;
                                    }
                                }
                                else if (BudgetTypeUpper != MFinanceConstants.BUDGET_INFLATE_N)
                                {
                                    BudgetTypeUpper = MFinanceConstants.BUDGET_INFLATE_N;
                                }
                            }

                            //BudgetYear
                            String BudgetYearStringUpper = (StringHelper.GetNextCSV(ref ImportLine, Separator, false)).ToUpper();

                            //Check validity of CSV file line values
                            if (!ValidateKeyBudgetFields(transaction,
                                    ALedgerNumber,
                                    RowNumber,
                                    CostCentreTable,
                                    AccountTable,
                                    CostCentre,
                                    Account,
                                    BudgetTypeUpper,
                                    BudgetYearStringUpper,
                                    ref hasAccountCodeProblem,
                                    VerificationResult))
                            {
                                BudgetsFailed++;
                                continue;
                            }

                            //Read the budgetperiod values to check if valid according to type
                            Array.Clear(BudgetPeriods, 0, NumPeriods);

                            if (!ProcessBudgetTypeImportDetails(RowNumber,
                                    ref ImportLine,
                                    Separator,
                                    BudgetTypeUpper,
                                    BudgetPeriods,
                                    BudgetYearStringUpper,
                                    CostCentre,
                                    Account,
                                    VerificationResult))
                            {
                                BudgetsFailed++;
                                continue;
                            }

                            //Calculate the budget Year
                            BudgetYearNumber = GetBudgetYearNumber(ALedgerNumber, BudgetYearStringUpper);

                            //Add budget revision record if there's not one already.
                            if (ImportDS.ABudgetRevision.Rows.Find(new object[] { ALedgerNumber, BudgetYearNumber, BdgRevision }) == null)
                            {
                                ABudgetRevisionRow BudgetRevisionRow = (ABudgetRevisionRow)ImportDS.ABudgetRevision.NewRowTyped();
                                BudgetRevisionRow.LedgerNumber = ALedgerNumber;
                                BudgetRevisionRow.Year = BudgetYearNumber;
                                BudgetRevisionRow.Revision = BdgRevision;
                                BudgetRevisionRow.Description = "Budget Import from: " + ACSVFileName;
                                ImportDS.ABudgetRevision.Rows.Add(BudgetRevisionRow);
                            }

                            for (int i = 0; i < NumPeriods; i++)
                            {
                                totalBudgetRowAmount += BudgetPeriods[i];
                            }

                            BudgetTDS mainDS = new BudgetTDS();

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
                                        BudgetYearStringUpper,
                                        BudgetYearNumber,
                                        CostCentre,
                                        Account));
                            }

                            #endregion Validate Data

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
                                    VerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + RowNumber.ToString("0000")),
                                            String.Format(Catalog.GetString(
                                                    " This budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') is repeated in the import file!"),
                                                BudgetYearStringUpper,
                                                CostCentre,
                                                Account),
                                            TResultSeverity.Resv_Noncritical));

                                    BudgetsFailed++;
                                    continue;
                                }

                                BudgetTableExistingAndImported.ImportRow(br2);

                                ABudgetRow bdgRow = (ABudgetRow)ImportDS.ABudget.Rows.Find(new object[] { bdgSeq });

                                if (bdgRow != null)
                                {
                                    bool rowUpdated = false;

                                    if (bdgRow.BudgetTypeCode != BudgetTypeUpper)
                                    {
                                        rowUpdated = true;
                                        bdgRow.BudgetTypeCode = BudgetTypeUpper;
                                    }

                                    ABudgetPeriodRow bpRow = null;

                                    for (int i = 0; i < NumPeriods; i++)
                                    {
                                        bpRow = (ABudgetPeriodRow)ImportDS.ABudgetPeriod.Rows.Find(new object[] { bdgSeq, i + 1 });

                                        if ((bpRow != null) && (bpRow.BudgetBase != BudgetPeriods[i]))
                                        {
                                            rowUpdated = true;
                                            bpRow.BudgetBase = BudgetPeriods[i];
                                        }

                                        bpRow = null;
                                    }

                                    if (rowUpdated)
                                    {
                                        bdgRow.Comment = "Updated";
                                        BudgetsUpdated++;
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
                                    VerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + RowNumber.ToString("0000")),
                                            String.Format(Catalog.GetString(
                                                    "This budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') is repeated in the import file!"),
                                                BudgetYearStringUpper,
                                                CostCentre,
                                                Account),
                                            TResultSeverity.Resv_Noncritical));

                                    BudgetsFailed++;
                                    continue;
                                }

                                //Add the new budget row
                                ABudgetRow BudgetRow = (ABudgetRow)ImportDS.ABudget.NewRowTyped();
                                int newSequence = Convert.ToInt32(TSequenceWebConnector.GetNextSequence(TSequenceNames.seq_budget));

                                BudgetRow.BudgetSequence = newSequence;
                                BudgetRow.LedgerNumber = ALedgerNumber;
                                BudgetRow.Year = BudgetYearNumber;
                                BudgetRow.Revision = BdgRevision;
                                BudgetRow.CostCentreCode = CostCentre;
                                BudgetRow.AccountCode = Account;
                                BudgetRow.BudgetTypeCode = BudgetTypeUpper;
                                BudgetRow.Comment = "Added";
                                ImportDS.ABudget.Rows.Add(BudgetRow);

                                //Add to import table to check for later duplicates
                                BudgetTableExistingAndImported.ImportRow(BudgetRow);

                                //Add the budget periods
                                for (int i = 0; i < NumPeriods; i++)
                                {
                                    ABudgetPeriodRow BudgetPeriodRow = (ABudgetPeriodRow)ImportDS.ABudgetPeriod.NewRowTyped();
                                    BudgetPeriodRow.BudgetSequence = newSequence;
                                    BudgetPeriodRow.PeriodNumber = i + 1;
                                    BudgetPeriodRow.BudgetBase = BudgetPeriods[i];
                                    ImportDS.ABudgetPeriod.Rows.Add(BudgetPeriodRow);
                                }

                                BudgetsAdded++;
                            }
                        }
                        catch (Exception ex)
                        {
                            TLogging.LogException(ex, Utilities.GetMethodSignature());
                            throw;
                        }
                        finally
                        {
                            // Read the next line
                            ImportLine = sr.ReadLine();
                        }
                    }
                }); // NewOrExisting AutoReadTransaction

            ABudgetsAdded = BudgetsAdded;
            ABudgetsUpdated = BudgetsUpdated;
            ABudgetsFailed = BudgetsFailed;

            if (hasAccountCodeProblem)
            {
                AVerificationResult.Add(new TVerificationResult(
                        Catalog.GetString("Account Code / Code Centre problem"),
                        Catalog.GetString(
                            "(To prevent the Excel program removing leading zeros, you can set the account column in the Excel spreadsheet to type 'Text')"),
                        TResultSeverity.Resv_Info));
            }

            return RowNumber;
        } // Import Budgets

        private static bool ValidateKeyBudgetFields(TDBTransaction Atransaction,
            int ALedgerNumber,
            int ARowNumber,
            ACostCentreTable ACostCentreTbl,
            AAccountTable AAccountTbl,
            string ACostCentre,
            string AAccount,
            string ABudgetType,
            string ABudgetYearStringUpper,
            ref Boolean AHasAccountCodeProblem,
            TVerificationResultCollection AVerificationResult)
        {
            string VerificationMessage = string.Empty;

            if (ACostCentreTbl.Rows.Count == 0)
            {
                try
                {
                    ACostCentreTbl = ACostCentreAccess.LoadViaALedger(ALedgerNumber, Atransaction);
                    AAccountTbl = AAccountAccess.LoadViaALedger(ALedgerNumber, Atransaction);

                    #region Validate Data

                    if ((ACostCentreTbl == null) || (ACostCentreTbl.Count == 0))
                    {
                        throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                    "Function:{0} - Cost Centre data for Ledger number {1} does not exist or could not be accessed!"),
                                Utilities.GetMethodName(true),
                                ALedgerNumber));
                    }
                    else if ((AAccountTbl == null) || (AAccountTbl.Count == 0))
                    {
                        throw new EFinanceSystemDataTableReturnedNoDataException(String.Format(Catalog.GetString(
                                    "Function:{0} - Account data for Ledger number {1} does not exist or could not be accessed!"),
                                Utilities.GetMethodName(true),
                                ALedgerNumber));
                    }

                    #endregion Validate Data
                }
                catch (Exception ex)
                {
                    TLogging.LogException(ex, Utilities.GetMethodSignature());
                    throw;
                }
            }

            //Check for missing or inactive cost centre
            ACostCentreRow CostCentreRow = (ACostCentreRow)ACostCentreTbl.Rows.Find(new object[] { ALedgerNumber, ACostCentre });

            if (CostCentreRow == null)
            {
                VerificationMessage += String.Format(Catalog.GetString(" Cost Centre: '{0}' does not exist."), ACostCentre);
                AHasAccountCodeProblem = true;
            }
            else if (!CostCentreRow.CostCentreActiveFlag)
            {
                VerificationMessage += String.Format(Catalog.GetString(" Cost Centre: '{0}' is currently inactive."), ACostCentre);
            }

            //Check for missing or inactive account code
            AAccountRow AccountRow = (AAccountRow)AAccountTbl.Rows.Find(new object[] { ALedgerNumber, AAccount });

            if (AccountRow == null)
            {
                VerificationMessage += String.Format(Catalog.GetString(" Account: '{0}' does not exist."), AAccount);
                AHasAccountCodeProblem = true;
            }
            else if (!AccountRow.AccountActiveFlag)
            {
                VerificationMessage += String.Format(Catalog.GetString(" Account: '{0}' is currently inactive."), AAccount);
            }

            //Check Budget Type
            if ((ABudgetType != MFinanceConstants.BUDGET_ADHOC)
                && (ABudgetType != MFinanceConstants.BUDGET_SAME)
                && (ABudgetType != MFinanceConstants.BUDGET_INFLATE_N)
                && (ABudgetType != MFinanceConstants.BUDGET_SPLIT)
                && (ABudgetType != MFinanceConstants.BUDGET_INFLATE_BASE))
            {
                VerificationMessage += String.Format(Catalog.GetString(" Budget Type: '{0}' does not exist."), ABudgetType);
            }

            //Check Budget Year
            ABudgetYearStringUpper = ABudgetYearStringUpper.Trim();

            if ((ABudgetYearStringUpper != MFinanceConstants.BUDGET_YEAR_THIS.ToUpper())
                && (ABudgetYearStringUpper != MFinanceConstants.BUDGET_YEAR_NEXT.ToUpper()))
            {
                VerificationMessage += String.Format(Catalog.GetString(" Budget Year: '{0}' must be exactly '{1}' or '{2}'."),
                    ABudgetYearStringUpper,
                    MFinanceConstants.BUDGET_YEAR_THIS,
                    MFinanceConstants.BUDGET_YEAR_NEXT);
            }

            //Check if errors have occurred
            if (VerificationMessage.Length > 0)
            {
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + ARowNumber.ToString("0000")),
                        VerificationMessage,
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
                    });

                ALedgerRow ledgerRow = (ALedgerRow)LedgerTable.Rows[0];
                AAccountingPeriodRow accPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];

                BudgetYear = accPeriodRow.PeriodStartDate.Year;

                if (ABudgetYearName.ToUpper() != MFinanceConstants.BUDGET_YEAR_THIS.ToUpper())
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
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
        }

        private static string BudgetRevisionYearName(int ALedgerNumber, int ABudgetRevisionYear)
        {
            string RetVal = string.Empty;

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
                    });

                ALedgerRow ledgerRow = (ALedgerRow)LedgerTable.Rows[0];
                AAccountingPeriodRow accPeriodRow = (AAccountingPeriodRow)AccPeriodTable.Rows[0];

                DateTime CurrentYearEnd = TAccountingPeriodsWebConnector.GetPeriodEndDate(ALedgerNumber,
                    ledgerRow.CurrentFinancialYear,
                    0,
                    ledgerRow.NumberOfAccountingPeriods);

                BudgetYear = ABudgetRevisionYear + CurrentYearEnd.Year - ledgerRow.CurrentFinancialYear;

                if (BudgetYear == accPeriodRow.PeriodStartDate.Year)
                {
                    RetVal = MFinanceConstants.BUDGET_YEAR_THIS;
                }
                else
                {
                    RetVal = MFinanceConstants.BUDGET_YEAR_NEXT;
                }
            }
            catch (Exception ex)
            {
                TLogging.LogException(ex, Utilities.GetMethodSignature());
                throw;
            }

            return RetVal;
        }

        private static bool ProcessBudgetTypeImportDetails(int ARowNumber,
            ref string Line,
            string Separator,
            string ABudgetType,
            decimal[] ABudgetPeriods,
            string ABudgetYearString,
            string ACostCentre,
            string AAccount,
            TVerificationResultCollection AVerificationResult)
        {
            //Assume return true
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
                AVerificationResult.Add(new TVerificationResult(Catalog.GetString("Row: " + ARowNumber.ToString("0000")),
                        String.Format(Catalog.GetString(
                                " The values in this budget import row (Year: '{0}', Cost Centre: '{1}', Account: '{2}') do not match the Budget Type: '{3}' or number of periods: '{4}'!"),
                            ABudgetYearString,
                            ACostCentre,
                            AAccount,
                            ABudgetType,
                            ABudgetPeriods.Length),
                        TResultSeverity.Resv_Noncritical));
            }

            return RetVal;
        } // Process BudgetType Import Details

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
            #region Validate Arguments

            if (ALedgerNumber <= 0)
            {
                throw new EFinanceSystemInvalidLedgerNumberException(String.Format(Catalog.GetString(
                            "Function:{0} - The Ledger number must be greater than 0!"),
                        Utilities.GetMethodName(true)), ALedgerNumber);
            }
            else if (ACSVFileName.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - The CSV File Name is missing!"),
                        Utilities.GetMethodName(true)));
            }
            else if (AFdlgSeparator.Length == 0)
            {
                throw new ArgumentException(String.Format(Catalog.GetString("Function:{0} - Information on how to parse the CSV file is missing!"),
                        Utilities.GetMethodName(true)));
            }

            #endregion Validate Arguments

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
            Int32 NumBudgetsExported = 0;

            ALedgerRow LedgerRow = (ALedgerRow)AExportDS.ALedger.Rows[0];

            ABudgetPeriodTable BudgetPeriod = (ABudgetPeriodTable)AExportDS.ABudgetPeriod;

            Int32 NumPeriods = LedgerRow.NumberOfAccountingPeriods;

            char Separator = AFdlgSeparator[0].Substring(0, 1).ToCharArray()[0];

            TLogging.Log("Writing file: " + ACSVFileName);

            StringBuilder StrBuilder = new StringBuilder();
            string BudgetAmounts = string.Empty;

            foreach (ABudgetRow row in AExportDS.ABudget.Rows)
            {
                switch (row.BudgetTypeCode)
                {
                    case MFinanceConstants.BUDGET_SAME:
                        StringBudgetTypeSameAmounts(row.BudgetSequence, ref BudgetPeriod, out BudgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_SPLIT:
                        StringBudgetTypeSplitAmounts(row.BudgetSequence, NumPeriods, ref BudgetPeriod, Separator, out BudgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_BASE:
                        StringBudgetTypeInflateBaseAmounts(row.BudgetSequence, NumPeriods, ref BudgetPeriod, Separator, out BudgetAmounts);

                        break;

                    case MFinanceConstants.BUDGET_INFLATE_N:
                        StringBudgetTypeInflateNAmounts(row.BudgetSequence, NumPeriods, ref BudgetPeriod, Separator, out BudgetAmounts);

                        break;

                    default:                              //MFinanceConstants.BUDGET_ADHOC:
                        StringBudgetTypeAdhocAmounts(row.BudgetSequence, NumPeriods, ref BudgetPeriod, Separator, out BudgetAmounts);

                        break;
                }

                StrBuilder.Append(StringHelper.StrMerge(
                        new string[] {
                            Encase(row.CostCentreCode),
                            Encase(row.AccountCode),
                            Encase(row.BudgetTypeCode),
                            Encase(BudgetRevisionYearName(ALedgerNumber, row.Year))
                        }, Separator));

                StrBuilder.Append(Separator.ToString());
                StrBuilder.Append(BudgetAmounts);
                StrBuilder.Append(Environment.NewLine);

                NumBudgetsExported++;
            }

            AFileContents = StrBuilder.ToString();

            return NumBudgetsExported;
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
            ASb += (perPeriodAmount * (ANumPeriods - 1) + endPeriodAmount).ToString("F2", CultureInfo.InvariantCulture);
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
                    ASb += budgetPeriodRow.BudgetBase.ToString("F2", CultureInfo.InvariantCulture);

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
                ASb += budgetPeriodRow.BudgetBase.ToString("F2", CultureInfo.InvariantCulture);
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
                        ASb += currentPeriodAmount.ToString("F2", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        ASb += ((currentPeriodAmount - priorPeriodAmount) / priorPeriodAmount * 100).ToString("F2", CultureInfo.InvariantCulture);
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
                        ASb += currentPeriodAmount.ToString("F2", CultureInfo.InvariantCulture);
                        ASb += ASeparator.ToString();
                    }
                    else
                    {
                        if (currentPeriodAmount != firstPeriodAmount)
                        {
                            ASb += (i - 1).ToString();
                            ASb += ASeparator.ToString();
                            ASb += ((currentPeriodAmount - firstPeriodAmount) / firstPeriodAmount * 100).ToString("F2", CultureInfo.InvariantCulture);
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