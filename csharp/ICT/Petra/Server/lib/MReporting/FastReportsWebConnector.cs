//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using System.Threading;
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
            FDbAdapter = new TReportingDbAdapter();
            TLogging.SetStatusBarProcedure(WriteToStatusBar);
            DataTable ResultTbl = null;

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

                default:
                    TLogging.Log("GetDatatableThread unknown ReportType: " + AReportType);
                    break;
            }

            return (FDbAdapter.IsCancelled) ? null : ResultTbl;
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
                Transaction = DBAccess.GDBAccessObj.BeginTransaction();

                FDbAdapter = new TReportingDbAdapter();

                while (!FDbAdapter.IsCancelled && ADataSetFilterCsv != "")
                {
                    String Tbl = StringHelper.GetNextCSV(ref ADataSetFilterCsv, ",", "");
                    String[] part = Tbl.Split('/');
                    DataTable NewTbl = FDbAdapter.RunQuery(part[1], part[0], Transaction);
                    //              MainDs.Tables[part[0]].Merge(FDbAdapter.RunQuery(part[1], part[0], Transaction));

                    MainDs.Merge(NewTbl);
                }

                if (FDbAdapter.IsCancelled)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                MainDs = null;
                throw;
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDs;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [RequireModulePermission("none")]
        public static DataSet GetRecipientGiftStatementDataSet(Dictionary <String, TVariant>AParameters)
        {
            string ReportType = AParameters["param_report_type"].ToString();

            FDbAdapter = new TReportingDbAdapter();
            DataSet ReturnDataSet = new DataSet();

            // get recipients
            DataTable Recipients = TFinanceReportingWebConnector.RecipientGiftStatementRecipientTable(AParameters);

            if (FDbAdapter.IsCancelled || (Recipients == null))
            {
                return null;
            }

            DataTable RecipientTotals = new DataTable("RecipientTotals");
            RecipientTotals.Columns.Add("PreviousYearTotal", typeof(decimal));
            RecipientTotals.Columns.Add("CurrentYearTotal", typeof(decimal));
            DataTable Donors = new DataTable("Donors");

            foreach (DataRow Row in Recipients.Rows)
            {
                if (ReportType == "Complete")
                {
                    // get year totals for recipient
                    RecipientTotals.Merge(TFinanceReportingWebConnector.RecipientGiftStatementTotalsTable(AParameters, (Int64)Row["RecipientKey"]));
                }

                // get donor information for each recipient
                Donors.Merge(TFinanceReportingWebConnector.RecipientGiftStatementDonorTable(AParameters, (Int64)Row["RecipientKey"]));
            }

            DataView View = new DataView(Donors);
            DataTable DistinctDonors = View.ToTable(true, "DonorKey");

            DataTable DonorAddresses = new DataTable("DonorAddresses");

            if ((ReportType == "Complete") || (ReportType == "Donors Only"))
            {
                foreach (DataRow Row in DistinctDonors.Rows)
                {
                    // get best address for each distinct donor
                    DonorAddresses.Merge(TFinanceReportingWebConnector.RecipientGiftStatementDonorAddressesTable(Convert.ToInt64(Row["DonorKey"])));
                }
            }
            else
            {
                DonorAddresses.Merge(DistinctDonors);
            }

            // We only want distinct donors for this report (i.e. no more than one per recipient)
            if (ReportType == "Donors Only")
            {
                DistinctDonors = View.ToTable(true, "DonorKey", "DonorName", "RecipientKey");
                Donors.Clear();
                Donors.Merge(DistinctDonors);
            }

            ReturnDataSet.Tables.Add(Recipients);
            ReturnDataSet.Tables.Add(RecipientTotals);
            ReturnDataSet.Tables.Add(Donors);
            ReturnDataSet.Tables.Add(DonorAddresses);

            return ReturnDataSet;
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
            return DomainManager.CurrentClient.FTasksManager.ClientTaskAdd(SharedConstants.CLIENTTASKGROUP_REPORT, ReportName, Params,
                null, null, null, 1);
        }
    }
}