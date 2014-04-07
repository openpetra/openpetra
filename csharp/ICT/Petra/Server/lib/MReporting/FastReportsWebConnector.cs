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
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Reporting.WebConnectors;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Common.DB;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Provides back-end methods to support the FastReports front end.
    /// </summary>
    public class TReportingWebConnector
    {
        /// <summary>Set this in a server utility to set the status</summary>
        public static String ServerStatus ="";
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
                FDbAdapter.Cancelled = true;
            }
        }

        /// <summary>If the client wants to find out that the operation was cancelled</summary>
        [RequireModulePermission("none")]
        public static Boolean DataTableGenerationWasCancelled()
        {
            return (FDbAdapter != null) ? FDbAdapter.Cancelled : false;
        }

        /// <summary>Prepare a DataTable for this kind of report, using these parameters.
        /// The process runs in a new thread, but this thread will stop here and wait
        /// until the result comes back, or the request is cancelled.
        /// </summary>
        [RequireModulePermission("none")]
        public static DataTable GetReportDataTable(String AReportType, Dictionary<String, TVariant> AParameters)
        {
            FDbAdapter = new TReportingDbAdapter();
            DataTable ResultTbl = null;
            switch (AReportType)
            {
                case "BalanceSheet":
                    ResultTbl = TFinanceReportingWebConnector.BalanceSheetTable(AParameters, FDbAdapter);
                    break;
                case "HOSA":
                    ResultTbl = TFinanceReportingWebConnector.HosaGiftsTable(AParameters, FDbAdapter);
                    break;
                case "IncomeExpense":
                    ResultTbl = TFinanceReportingWebConnector.IncomeExpenseTable(AParameters, FDbAdapter);
                    break;
                default:
                    TLogging.Log("GetDatatableThread unknown ReportType: " + AReportType);
                    break;
            }
            return (FDbAdapter.Cancelled) ? null : ResultTbl;
        }

        /// <summary>
        /// Returns a DataSet to the client for use in client-side reporting
        /// </summary>
        [RequireModulePermission("none")]
        public static GLReportingTDS GetReportingDataSet(String ADataSetFilterCsv)
        {
            GLReportingTDS MainDs = new GLReportingTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();
            FDbAdapter = new TReportingDbAdapter();

            while (!FDbAdapter.Cancelled && ADataSetFilterCsv != "")
            {
                String Tbl = StringHelper.GetNextCSV(ref ADataSetFilterCsv, ",", "");
                String[] part = Tbl.Split('/');
                String OrderBy = "";

                if (part.Length > 4)
                {
                    OrderBy = part[4];
                }

                String Query = "SELECT " + part[1] + " FROM " + part[2] + " WHERE " + part[3] + OrderBy;
                MainDs.Tables[part[0]].Merge(FDbAdapter.RunQuery(Query, part[0], Transaction));
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDs;
        }


    }
}