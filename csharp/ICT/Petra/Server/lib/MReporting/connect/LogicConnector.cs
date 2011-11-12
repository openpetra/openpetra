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
using System.Data;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MReporting.LogicConnectors;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Threading;
using Ict.Common;
using Ict.Common.DB;

namespace Ict.Petra.Server.MReporting.LogicConnectors
{
    /// <summary>
    /// the connector for the report generation
    /// </summary>
    public class TReportGeneratorLogicConnector : TConfigurableMBRObject, IReportGeneratorLogicConnector
    {
        private TAsynchronousExecutionProgress FAsyncExecProgress;
        private TRptDataCalculator FDatacalculator;
        private TResultList FResultList;
        private TParameterList FParameterList;
        private String FErrorMessage;
        private Boolean FSuccess;

        /// constructor needed for the interface
        public TReportGeneratorLogicConnector()
        {
        }

        /// <summary>
        /// to show the progress of the report calculation;
        /// prints the current id of the row that is being calculated
        /// </summary>
        public IAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                return FAsyncExecProgress;
            }
        }


        /// <summary>
        /// Calculates the report, which is specified in the parameters table
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Start(System.Data.DataTable AParameters)
        {
            Thread TheThread;
            String PathStandardReports;
            String PathCustomReports;

            this.FAsyncExecProgress = new TAsynchronousExecutionProgress();
            this.FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Executing;
            FParameterList = new TParameterList();
            FParameterList.LoadFromDataTable(AParameters);
            FSuccess = false;
            PathStandardReports = TAppSettingsManager.GetValue("Reporting.PathStandardReports");
            PathCustomReports = TAppSettingsManager.GetValue("Reporting.PathCustomReports");
            FDatacalculator = new TRptDataCalculator(DBAccess.GDBAccessObj, PathStandardReports, PathCustomReports);

            // setup the logging to go to the FAsyncExecProgress.ProgressInformation
            TLogging.SetStatusBarProcedure(new TLogging.TStatusCallbackProcedure(WriteToStatusBar));
            TheThread = new Thread(new ThreadStart(Run));
            TheThread.Start();
        }

        /// <summary>
        /// cancel the report calculation
        /// </summary>
        public void Cancel()
        {
            // This variable will be picked up regularly during generation, in TRptDataCalcLevel.calculate in Ict.Petra.Server.MReporting.Calculation
            FParameterList.Add("CancelReportCalculation", new TVariant(true));
        }

        /// <summary>
        /// run the report
        /// </summary>
        private void Run()
        {
            try
            {
                if (FParameterList.Get("IsolationLevel").ToString().ToLower() == "readuncommitted")
                {
                    // for long reports, that should not take out locks;
                    // the data does not need to be consistent or will most likely not be changed during the generation of the report
                    DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);
                }
                else if (FParameterList.Get("IsolationLevel").ToString().ToLower() == "repeatableread")
                {
                    // for financial reports: it is important to have consistent data; e.g. for totals
                    DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.RepeatableRead);
                }
                else
                {
                    // default behaviour for normal reports
                    DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
                }

                FSuccess = false;

                if (FDatacalculator.GenerateResult(ref FParameterList, ref FResultList, ref FErrorMessage))
                {
                    FSuccess = true;
                }
                else
                {
                    TLogging.Log(FErrorMessage);
                }
            }
            catch (Exception e)
            {
                TLogging.Log("problem when calculating report: " + e.Message);
                TLogging.Log(e.StackTrace, TLoggingType.ToLogfile);
            }
            DBAccess.GDBAccessObj.RollbackTransaction();
            FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Finished;
        }

        /// <summary>
        /// get the result of the report calculation
        /// </summary>
        public DataTable Result
        {
            get
            {
                return FResultList.ToDataTable(FParameterList);
            }
        }

        /// <summary>
        /// get the environment variables after report calculation
        /// </summary>
        public DataTable Parameter
        {
            get
            {
                return FParameterList.ToDataTable();
            }
        }

        /// <summary>
        /// see if the report calculation finished successfully
        /// </summary>
        public Boolean Success
        {
            get
            {
                return FSuccess;
            }
        }

        /// <summary>
        /// error message that happened during report calculation
        /// </summary>
        public String ErrorMessage
        {
            get
            {
                return FErrorMessage;
            }
        }

        /// <summary>
        /// for displaying the progress
        /// </summary>
        /// <returns>void</returns>
        private void WriteToStatusBar(String s)
        {
            FAsyncExecProgress.ProgressInformation = s;
        }
    }
}