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
using Ict.Petra.Server.App.Core.Security;
using System.Data;
using System.Collections.Generic;
using Ict.Common;
using System.Threading;
using Ict.Petra.Server.MFinance.Reporting.WebConnectors;

namespace Ict.Petra.Server.MReporting.WebConnectors
{
    /// <summary>
    /// Provides back-end methods to support the FastReports front end.
    /// </summary>
    public class TReportingWebConnector
    {
        /// <summary>Set this in a server utility to set the status</summary>
        public static String ServerStatus ="";
        private static Thread FWorkerThread;
        private enum TWorkerStatusEnum {none,running,finished,aborted};
        private static TWorkerStatusEnum FWorkerStatus = TWorkerStatusEnum.none;
        private static DataTable FResultSet = null;
        private static String FReportType;
        private static Dictionary<String, TVariant> FParameters;

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


        /// <summary>Forget the thread that's getting a Dataset for me.</summary>
        [RequireModulePermission("none")]
        public static void CancelDataTableGeneration()
        {
            FWorkerStatus = TWorkerStatusEnum.aborted;
            FWorkerThread = null;
        }

        private static void GetDatatableThread ()
        {
            FWorkerStatus = TWorkerStatusEnum.running;
            TLogging.SetStatusBarProcedure(new TLogging.TStatusCallbackProcedure(WriteToStatusBar));

            switch (FReportType)
            {
                case "BalanceSheet":
                    FResultSet = TFinanceReportingWebConnector.BalanceSheetTable(FParameters);
                    break;
                case "HOSA":
                    FResultSet = TFinanceReportingWebConnector.HosaGiftsTable(FParameters);
                    break;
                case "IncomeExpense":
                    FResultSet = TFinanceReportingWebConnector.IncomeExpenseTable(FParameters);
                    break;
                default:
                    TLogging.Log("GetDatatableThread unknown ReportType: " + FReportType);
                    break;
            }

            //
            // On finishing, I can only set the status if
            // this thread is the one the client is wating for:
            if (FWorkerThread != null && (Thread.CurrentThread.ManagedThreadId == FWorkerThread.ManagedThreadId))
            {
                FWorkerStatus = TWorkerStatusEnum.finished;
            }
        }

        /// <summary>Prepare a DataTable for this kind of report, using these parameters.
        /// The process runs in a new thread, but this thread will stop here and wait
        /// until the result comes back, or the request is cancelled.
        /// </summary>
        [RequireModulePermission("none")]
        public static DataTable GetReportDataTable(String AReportType, Dictionary<String, TVariant> AParameters)
        {
            FReportType = AReportType;
            FParameters = AParameters;

            FWorkerThread = new Thread(GetDatatableThread);
            FWorkerThread.IsBackground = true;
            FWorkerThread.Start();

            //
            // Now I'm going to sit here and wait to see if the thread finishes,
            // or it gets aborted by a further request from the client.

            do
            {
                Thread.Sleep(500);
            } while (FWorkerStatus == TWorkerStatusEnum.running);

            return (FWorkerStatus == TWorkerStatusEnum.finished)? FResultSet : null;
        }
    }
}