//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Reporting.WebConnectors;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Provides back-end methods to support the FastReports front end.
    /// </summary>
    public class TReportingWebConnector
    {
        /// <summary>Set this in a server utility to set the status</summary>
        public static String ServerStatus = "";
        private static TReportingDbAdapter FDbAdapter = null;

        /// <summary>Call this from the client to display the status:</summary>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static String GetServerStatus()
        {
            return ServerStatus;
        }

        /// <summary>
        /// TLogging StatusBar calls come to here.
        /// They are returned to the client by a thread that calls regularly to GetServerStatus, above.
        /// </summary>
        /// <returns>void</returns>
        private static void WriteToStatusBar(String s)
        {
            ServerStatus = s;
        }

        /// <summary>Cancel the operation that's getting a Dataset for me.</summary>
        [RequireModulePermission("none")]
        public static void CancelDataTableGeneration()
        {
            if (FDbAdapter != null)
            {
                FDbAdapter.CancelQuery();
            }
        }

        /// <summary>If the client wants to find out that the operation was cancelled</summary>
        [RequireModulePermission("none")]
        public static Boolean DataTableGenerationWasCancelled()
        {
            return (FDbAdapter != null) ? FDbAdapter.IsCancelled : false;
        }

        /// <summary>Prepare a DataTable for this kind of report, using these parameters.
        /// The process runs in a new thread, but this thread will stop here and wait
        /// until the result comes back, or the request is cancelled.
        /// </summary>
        [RequireModulePermission("none")]
        public static DataTable GetReportDataTable(String AReportType, Dictionary <String, TVariant>AParameters)
        {
            TLogging.SetStatusBarProcedure(WriteToStatusBar);
            DataTable ResultTbl = null;

            FDbAdapter = new TReportingDbAdapter(true);   // Uses a separate DB Connection.

            switch (AReportType)
            {
                /* GL Reports */

                case "BalanceSheet":

                    ResultTbl = TFinanceReportingWebConnector.BalanceSheetTable(AParameters, FDbAdapter);
                    break;

                case "FieldGifts":

                    ResultTbl = TFinanceReportingWebConnector.KeyMinGiftsTable(AParameters, FDbAdapter);
                    break;

                case "HOSA":

                    ResultTbl = TFinanceReportingWebConnector.HosaGiftsTable(AParameters, FDbAdapter);
                    break;

                case "Stewardship":

                    ResultTbl = TFinanceReportingWebConnector.StewardshipTable(AParameters, FDbAdapter);
                    break;

                case "Fees":

                    ResultTbl = TFinanceReportingWebConnector.FeesTable(AParameters, FDbAdapter);
                    break;

                case "StewardshipForPeriod":

                    ResultTbl = TFinanceReportingWebConnector.StewardshipForPeriodTable(AParameters, FDbAdapter);
                    break;

                case "IncomeExpense":

                    ResultTbl = TFinanceReportingWebConnector.IncomeExpenseTable(AParameters, FDbAdapter);
                    break;

                case "AFO":

                    ResultTbl = TFinanceReportingWebConnector.AFOTable(AParameters, FDbAdapter);
                    break;

                case "Executive Summary":

                    ResultTbl = TFinanceReportingWebConnector.ExecutiveSummaryTable(AParameters, FDbAdapter);
                    break;

                case "TrialBalance":

                    ResultTbl = TFinanceReportingWebConnector.TrialBalanceTable(AParameters, FDbAdapter);
                    break;

                case "SurplusDeficit":

                    ResultTbl = TFinanceReportingWebConnector.SurplusDeficitTable(AParameters, FDbAdapter);
                    break;

                /* Gift Reports */

                case "GiftBatchDetail":

                    ResultTbl = TFinanceReportingWebConnector.GiftBatchDetailTable(AParameters, FDbAdapter);
                    break;

                case "RecipientTaxDeductPct":

                    ResultTbl = TFinanceReportingWebConnector.RecipientTaxDeductPctTable(AParameters, FDbAdapter);
                    break;

                case "FieldLeaderGiftSummary":

                    ResultTbl = TFinanceReportingWebConnector.FieldLeaderGiftSummary(AParameters, FDbAdapter);
                    break;

                case "TotalGiftsThroughField":

                    ResultTbl = TFinanceReportingWebConnector.TotalGiftsThroughField(AParameters, FDbAdapter);
                    break;

                case "MotivationResponse":

                    ResultTbl = TFinanceReportingWebConnector.MotivationResponse(AParameters, FDbAdapter);
                    break;

                case "DonorGiftsToField":

                    ResultTbl = TFinanceReportingWebConnector.DonorGiftsToField(AParameters, FDbAdapter);
                    break;

                case "GiftDestination":

                    ResultTbl = TFinanceReportingWebConnector.GiftDestination(AParameters, FDbAdapter);
                    break;

                case "TotalForRecipients":
                    ResultTbl = TFinanceReportingWebConnector.TotalForRecipients(AParameters, FDbAdapter);
                    break;

                /* Financial Development */

                case "SYBUNT":

                    ResultTbl = TFinanceReportingWebConnector.SYBUNTTable(AParameters, FDbAdapter);
                    break;

                default:
                    TLogging.Log("GetDatatableThread unknown ReportType: " + AReportType);
                    break;
            }

            FDbAdapter.CloseConnection();

            if (FDbAdapter.IsCancelled)
            {
                ResultTbl = null;
            }

            return ResultTbl;
        }

        /// <summary>Prepare a DataTable for this kind of report, using these parameters.
        /// The process runs in a new thread, but this thread will stop here and wait
        /// until the result comes back, or the request is cancelled.
        /// </summary>
        [RequireModulePermission("none")]
        public static DataSet GetReportDataSet(String AReportType, Dictionary <String, TVariant>AParameters)
        {
            TLogging.SetStatusBarProcedure(WriteToStatusBar);
            DataSet ResultSet = null;

            FDbAdapter = new TReportingDbAdapter(true);   // Uses a separate DB Connection.

            switch (AReportType)
            {
                /* Finance */
                case "RecipientGiftStatement":
                    ResultSet = GetRecipientGiftStatementDataSet(AParameters, FDbAdapter);
                    break;

                case "DonorGiftStatement":
                    ResultSet = GetDonorGiftStatementDataSet(AParameters, FDbAdapter);
                    break;

                /* Financial Development */

                case "GiftsOverMinimum":

                    ResultSet = TFinanceReportingWebConnector.GiftsOverMinimum(AParameters, FDbAdapter);
                    break;

                default:
                    TLogging.Log("GetDataSetThread unknown ReportType: " + AReportType);
                    break;
            }

            FDbAdapter.CloseConnection();

            if (FDbAdapter.IsCancelled)
            {
                ResultSet = null;
            }

            return ResultSet;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// The CSV can get several tables and add them to the Dataset.
        /// Each CSV entry is divided using a slash / like this:
        /// TableName/Query.
        /// </summary>
        [RequireModulePermission("none")]
        public static GLReportingTDS GetReportingDataSet(String ADataSetFilterCsv)
        {
            TDBTransaction Transaction = null;
            GLReportingTDS MainDs = new GLReportingTDS();

            try
            {
                FDbAdapter = new TReportingDbAdapter(true);
                Transaction = FDbAdapter.FPrivateDatabaseObj.BeginTransaction(
ATransactionName: "FastReports Report GetReportingDataSet DB Transaction");

                while (!FDbAdapter.IsCancelled && ADataSetFilterCsv != "")
                {
                    String Tbl = StringHelper.GetNextCSV(ref ADataSetFilterCsv, ",", "");
                    String[] part = Tbl.Split('/');
                    DataTable NewTbl = FDbAdapter.RunQuery(part[1], part[0], Transaction);
                    MainDs.Merge(NewTbl);
                }

                if (FDbAdapter.IsCancelled)
                {
                    return null;
                }
            }
            catch (Exception Exc)
            {
                MainDs = null;

                TLogging.Log("TReportingWebConnector.GetReportingDataSet encountered an Exception: " + Exc.ToString());

                throw;
            }
            finally
            {
                FDbAdapter.FPrivateDatabaseObj.RollbackTransaction();
                FDbAdapter.CloseConnection();
            }

            return MainDs;
        }

        private static void GetDonorHistoricTotals(Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            Int64 APartnerKey,
            DataTable Totals)
        {
            TDBTransaction Transaction = null;
            int ledgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            int currentYear = DateTime.Now.Year;

            string amountField = AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n";

            ADbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    for (Int32 month = 1; month <= 12; month++)
                    {
                        String currentMonthStart = string.Format("-{0:D2}-01'", month);
                        String nextMonthStart = string.Format("-{0:D2}-01'", month + 1);
                        Int32 nextMonthYear = currentYear;

                        if (month == 12)
                        {
                            nextMonthYear++;
                            nextMonthStart = "-01-01'";
                        }

                        string Query = "SELECT " +
                                       APartnerKey + " AS DonorKey, " +
                                       month + " AS Month, " +
                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + currentYear + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + nextMonthYear + nextMonthStart +
                                       " THEN GiftDetail." + amountField +
                                       " ELSE 0" +
                                       " END) AS Year0Total," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + currentYear + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + nextMonthYear + nextMonthStart +
                                       " THEN 1" +
                                       " ELSE 0" +
                                       " END) AS Year0Count," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 1) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 1) + nextMonthStart +
                                       " THEN GiftDetail." + amountField +
                                       " ELSE 0" +
                                       " END) AS Year1Total," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 1) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 1) + nextMonthStart +
                                       " THEN 1" +
                                       " ELSE 0" +
                                       " END) AS Year1Count," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 2) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 2) + nextMonthStart +
                                       " THEN GiftDetail." + amountField +
                                       " ELSE 0" +
                                       " END) AS Year2Total," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 2) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 2) + nextMonthStart +
                                       " THEN 1" +
                                       " ELSE 0" +
                                       " END) AS Year2Count," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 3) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 3) + nextMonthStart +
                                       " THEN GiftDetail." + amountField +
                                       " ELSE 0" +
                                       " END) AS Year3Total," +

                                       " SUM (" +
                                       " CASE WHEN" +
                                       " Gift.a_date_entered_d >= '" + (currentYear - 3) + currentMonthStart +
                                       " AND Gift.a_date_entered_d < '" + (nextMonthYear - 3) + nextMonthStart +
                                       " THEN 1 " +
                                       " ELSE 0" +
                                       " END) AS Year3Count" +

                                       " FROM" +
                                       " PUB_a_gift AS Gift, " +
                                       " PUB_a_gift_detail AS GiftDetail," +
                                       " PUB_a_gift_batch AS GiftBatch" +

                                       " WHERE" +

                                       " GiftDetail.a_ledger_number_i = " + ledgerNumber +
                                       " AND Gift.p_donor_key_n = " + APartnerKey +
                                       " AND Gift.a_ledger_number_i = " + ledgerNumber +
                                       " AND Gift.a_batch_number_i = GiftDetail.a_batch_number_i" +
                                       " AND Gift.a_gift_transaction_number_i = GiftDetail.a_gift_transaction_number_i" +
                                       " AND Gift.a_date_entered_d >= '" + (currentYear - 3) + "-01-01'" +
                                       " AND GiftBatch.a_ledger_number_i = " + ledgerNumber +
                                       " AND GiftBatch.a_batch_number_i = Gift.a_batch_number_i" +
                                       " AND GiftBatch.a_batch_status_c = 'Posted'";

                        DataTable Results = ADbAdapter.RunQuery(Query, "RecipientTotals", Transaction);

                        if (Results == null)
                        {
                            break;
                        }
                        else
                        {
                            Totals.Merge(Results);
                        }
                    } // for month

                }); // Get NewOrExisting AutoRead Transaction
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        private static DataSet GetDonorGiftStatementDataSet(Dictionary <String, TVariant>AParameters, TReportingDbAdapter ADbAdapter)
        {
            String reportType = AParameters["param_report_type"].ToString();
            String donorSelect = AParameters["param_donor"].ToString();
            Int64 donorKey = (donorSelect == "All Donors") ?
                             -1
                             :
                             AParameters["param_donorkey"].ToInt64();

            TLogging.SetStatusBarProcedure(WriteToStatusBar);

            // get donors
            DataTable Donors = TFinanceReportingWebConnector.GiftStatementDonorTable(AParameters, ADbAdapter, donorKey, -1, "DONOR");

            if (ADbAdapter.IsCancelled || (Donors == null))
            {
                return null;
            }

            DataTable Recipients = new DataTable("Recipients");
            DataTable Totals = new DataTable("Totals");

            DataView View = new DataView(Donors);
            View.Sort = "DonorKey";

            DataTable DistinctDonors = new DataTable();
            DataTable tempTable;

            if (View.Count > 0)
            {
                DistinctDonors = View.ToTable(true, "DonorKey");
            }

            foreach (DataRow Row in DistinctDonors.Rows)
            {
                Int64 DonorKey = (Int64)Row["DonorKey"];

                // get historical totals for donor
                if (reportType == "Totals")
                {
                    GetDonorHistoricTotals(AParameters, ADbAdapter, DonorKey, Totals);
                }
                else
                {
                    Decimal thisYearTotal = 0;
                    Decimal previousYearTotal = 0;
                    TFinanceReportingWebConnector.GetGiftStatementTotals(AParameters,
                        ADbAdapter,
                        DonorKey,
                        out thisYearTotal,
                        out previousYearTotal,
                        true);
                    DataRowView[] WriteBackRVs = View.FindRows(DonorKey);

                    foreach (DataRowView rv in WriteBackRVs)
                    {
                        DataRow DonorsRow = rv.Row;
                        DonorsRow["thisYearTotal"] = thisYearTotal;
                        DonorsRow["previousYearTotal"] = previousYearTotal;
                    }

                    // Get recipient information for each donor
                    tempTable = TFinanceReportingWebConnector.GiftStatementRecipientTable(AParameters, ADbAdapter, DonorKey);

                    if (tempTable != null)
                    {
                        Recipients.Merge(tempTable);
                    }
                }

                if (ADbAdapter.IsCancelled)
                {
                    return null;
                }
            } // foreach

            if (reportType == "Totals")
            {
                Totals.DefaultView.Sort = "DonorKey, Month";
                Totals = Totals.DefaultView.ToTable();

                Recipients.Columns.Add("RecipientKey", typeof(Int32));
                Recipients.Columns.Add("RecipientName", typeof(Int32));
                Recipients.Columns.Add("RecipientClass", typeof(Int32));
                Recipients.Columns.Add("FieldName", typeof(Int32));
                Recipients.Columns.Add("FieldKey", typeof(Int32));
                Recipients.Columns.Add("thisYearTotal", typeof(Int32));
                Recipients.Columns.Add("previousYearTotal", typeof(Int32));
            }
            else
            {
                Totals.Columns.Add("DonorKey", typeof(Int64));
                Totals.Columns.Add("Month", typeof(Int32));
                Totals.Columns.Add("Year0Total", typeof(Decimal));
                Totals.Columns.Add("Year0Count", typeof(Int32));
                Totals.Columns.Add("Year1Total", typeof(Decimal));
                Totals.Columns.Add("Year1Count", typeof(Int32));
                Totals.Columns.Add("Year2Total", typeof(Decimal));
                Totals.Columns.Add("Year2Count", typeof(Int32));
                Totals.Columns.Add("Year3Total", typeof(Decimal));
                Totals.Columns.Add("Year3Count", typeof(Int32));
            }

            DataTable DonorAddresses = new DataTable("DonorAddresses");

            foreach (DataRow Row in DistinctDonors.Rows)
            {
                // get best address for donor
                Int64 DonorKey = (Int64)Row["DonorKey"];
                tempTable = TFinanceReportingWebConnector.GiftStatementDonorAddressesTable(ADbAdapter, DonorKey);

                if (tempTable != null)
                {
                    DonorAddresses.Merge(tempTable);
                }

                if (ADbAdapter.IsCancelled)
                {
                    return null;
                }
            }

            if (ADbAdapter.IsCancelled)
            {
                return null;
            }

            DataSet ReturnDataSet = new DataSet();
            ReturnDataSet.Tables.Add(Recipients);
            ReturnDataSet.Tables.Add(Donors);
            ReturnDataSet.Tables.Add(DonorAddresses);
            ReturnDataSet.Tables.Add(Totals);
            return ReturnDataSet;
        } // Get DonorGiftStatement DataSet

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        private static DataSet GetRecipientGiftStatementDataSet(Dictionary <String, TVariant>AParameters, TReportingDbAdapter ADbAdapter)
        {
            string ReportType = AParameters["param_report_type"].ToString();

            TLogging.SetStatusBarProcedure(WriteToStatusBar);

            // get recipients
            DataTable Recipients = TFinanceReportingWebConnector.GiftStatementRecipientTable(AParameters, ADbAdapter);
            Recipients.DefaultView.Sort = "RecipientKey";

            if (ADbAdapter.IsCancelled || (Recipients == null))
            {
                return null;
            }

            DataTable Donors = new DataTable("Donors");

            foreach (DataRow Row in Recipients.Rows)
            {
                Int64 recipientKey = (Int64)Row["RecipientKey"];

                if (ReportType == "Complete")
                {
                    // get historical totals for recipient
                    Decimal thisYearTotal = 0;
                    Decimal previousYearTotal = 0;
                    TFinanceReportingWebConnector.GetGiftStatementTotals(AParameters,
                        ADbAdapter,
                        recipientKey,
                        out thisYearTotal,
                        out previousYearTotal,
                        false);

                    DataRowView[] WriteBackRVs = Recipients.DefaultView.FindRows(recipientKey);

                    foreach (DataRowView rv in WriteBackRVs)
                    {
                        DataRow RecipientsRow = rv.Row;
                        RecipientsRow["thisYearTotal"] = thisYearTotal;
                        RecipientsRow["previousYearTotal"] = previousYearTotal;
                    }
                }

                // get donor information for each recipient
                DataTable DonorTemp = TFinanceReportingWebConnector.GiftStatementDonorTable(AParameters, ADbAdapter, -1, recipientKey);

                if (DonorTemp != null)
                {
                    Donors.Merge(DonorTemp);
                }

                if (ADbAdapter.IsCancelled)
                {
                    return null;
                }
            }

            DataView View = new DataView(Donors);
            DataTable DistinctDonors = new DataTable();

            if (View.Count > 0)
            {
                DistinctDonors = View.ToTable(true, "DonorKey");
            }

            DataTable DonorAddresses = new DataTable("DonorAddresses");

            if ((ReportType == "Complete") || (ReportType == "Donors Only"))
            {
                foreach (DataRow Row in DistinctDonors.Rows)
                {
                    // get best address for each distinct donor
                    DataTable DonorTemp = TFinanceReportingWebConnector.GiftStatementDonorAddressesTable(ADbAdapter, Convert.ToInt64(Row["DonorKey"]));

                    if (DonorTemp != null)
                    {
                        DonorAddresses.Merge(DonorTemp);
                    }

                    if (ADbAdapter.IsCancelled)
                    {
                        return null;
                    }
                }
            }

            if (ReportType == "Donors Only")
            {
                if (View.Count > 0)
                {
                    DistinctDonors = View.ToTable(true, "DonorKey", "DonorName", "RecipientKey");
                    Donors.Clear();
                    Donors.Merge(DistinctDonors);
                }
                else // I should return an empty table with just columns, to keep the client happy:
                {
                    DistinctDonors = new DataTable();
                    DistinctDonors.Columns.Add("DonorKey", typeof(Int64));
                    DistinctDonors.Columns.Add("DonorName", typeof(String));
                    DistinctDonors.Columns.Add("RecipientKey", typeof(Int64));
                }
            }

            if (ADbAdapter.IsCancelled)
            {
                return null;
            }

            DataSet ReturnDataSet = new DataSet();
            ReturnDataSet.Tables.Add(Recipients);
            ReturnDataSet.Tables.Add(Donors);
            ReturnDataSet.Tables.Add(DonorAddresses);

            return ReturnDataSet;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [RequireModulePermission("none")]
        public static DataSet GetOneYearMonthGivingDataSet(Dictionary <String, TVariant>AParameters)
        {
            FDbAdapter = new TReportingDbAdapter(true);
            TLogging.SetStatusBarProcedure(WriteToStatusBar);
            DataSet ReturnDataSet = new DataSet();

            // get recipients
            DataTable Recipients = TFinanceReportingWebConnector.GiftStatementRecipientTable(AParameters, FDbAdapter);

            if (FDbAdapter.IsCancelled || (Recipients == null))
            {
                FDbAdapter.CloseConnection();
                return null;
            }

            DataTable Donors = new DataTable("Donors");

            foreach (DataRow Row in Recipients.Rows)
            {
                // get donor information for each recipient
                Donors.Merge(TFinanceReportingWebConnector.OneYearMonthGivingDonorTable(AParameters, (Int64)Row["RecipientKey"], FDbAdapter));

                if (FDbAdapter.IsCancelled)
                {
                    FDbAdapter.CloseConnection();
                    return null;
                }
            }

            ReturnDataSet.Tables.Add(Recipients);
            ReturnDataSet.Tables.Add(Donors);

            FDbAdapter.CloseConnection();

            return (FDbAdapter.IsCancelled) ? null : ReturnDataSet;
        }

        /// <summary>
        /// Uses the ClientTask mechanism to ask the client to request a report with the given params
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="Params">a CSV list of param_name=value</param>
        /// <returns></returns>
        [RequireModulePermission("none")]
        public static Int32 GenerateReportOnClient(String ReportName, String Params)
        {
            // Standalone does not support ClientTasks yet
            if (DomainManager.CurrentClient == null)
            {
                return -1;
            }

            return DomainManager.CurrentClient.FTasksManager.ClientTaskAdd(SharedConstants.CLIENTTASKGROUP_REPORT, ReportName, Params,
                null, null, null, 1);
        }
    }
}
