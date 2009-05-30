/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using Ict.Common;
using Ict.Common.DB;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// run a function for each result
    /// </summary>
    public delegate void TProcessResult(TRptSituation situation, TResult element);

    /// <summary>
    /// situation describes the environment where the calculator currently is;
    /// this is eg the depth in the report, the column, etc;
    /// this makes it possible to have eg. a different ledger per column, etc
    /// </summary>
    public class TRptSituation
    {
        /// <summary>
        /// current depth in the report level hierarchy
        /// </summary>
        public int Depth;

        /// <summary>
        /// current column
        /// </summary>
        protected int column;

        /// <summary>
        /// identification of the current row
        /// </summary>
        protected int LineId;

        /// <summary>
        /// identification of the parent row of the current row
        /// </summary>
        protected int ParentRowId;

        /// <summary>
        /// current set of parameters (ie environment variables)
        /// </summary>
        protected TParameterList Parameters;

        /// <summary>
        /// the result of the report, this can be used to run functions on already calcualated results
        /// </summary>
        protected TResultList Results;

        /// <summary>
        /// the report definition
        /// </summary>
        protected TReportStore ReportStore;

        /// <summary>
        /// reference to the current report definition
        /// </summary>
        protected TRptReport CurrentReport;

        /// <summary>
        /// connection to the database for SQL queries
        /// </summary>
        protected TDataBase DatabaseConnection;

        /// <summary>
        /// the runningCode is used to establish the master/child relation
        /// of the records in the precalculated table
        ///
        /// </summary>
        protected static int RunningCode;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="results"></param>
        /// <param name="reportStore"></param>
        /// <param name="report"></param>
        /// <param name="dataDB"></param>
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptSituation(TParameterList parameters,
            TResultList results,
            TReportStore reportStore,
            TRptReport report,
            TDataBase dataDB,
            int depth,
            int column,
            int lineId,
            int parentRowId)
        {
            TRptSituation.RunningCode = 0;
            this.Parameters = parameters;
            this.Results = results;
            this.ReportStore = reportStore;
            this.CurrentReport = report;
            this.DatabaseConnection = dataDB;
            this.Depth = depth;
            this.column = column;
            this.LineId = lineId;
            this.ParentRowId = parentRowId;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptSituation(TRptSituation situation)
        {
            this.Parameters = situation.Parameters;
            this.Results = situation.Results;
            this.ReportStore = situation.ReportStore;
            this.CurrentReport = situation.CurrentReport;
            this.DatabaseConnection = situation.DatabaseConnection;
            this.Depth = situation.Depth;
            this.column = situation.column;
            this.LineId = situation.LineId;
            this.ParentRowId = situation.ParentRowId;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptSituation(TRptSituation situation, int depth, int column, int lineId, int parentRowId)
        {
            this.Parameters = situation.Parameters;
            this.Results = situation.Results;
            this.ReportStore = situation.ReportStore;
            this.CurrentReport = situation.CurrentReport;
            this.DatabaseConnection = situation.DatabaseConnection;
            this.Depth = depth;
            this.column = column;
            this.LineId = lineId;
            this.ParentRowId = parentRowId;
        }

        /// <summary>
        /// the runningCode is used to establish the master/child relation
        /// of the records in the precalculated table
        /// </summary>
        /// <returns></returns>
        public static int GetNextRunningCode()
        {
            int ReturnValue;

            ReturnValue = RunningCode;
            RunningCode++;
            return ReturnValue;
        }

        /// <summary>
        /// property accessor to the current environment of the calculation
        /// </summary>
        /// <returns></returns>
        public TParameterList GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// property accessor for result of report
        /// </summary>
        /// <returns></returns>
        public TResultList GetResults()
        {
            return Results;
        }

        /// <summary>
        /// property accessor for the report definitions
        /// </summary>
        /// <returns></returns>
        public TReportStore GetReportStore()
        {
            return ReportStore;
        }

        /// <summary>
        /// property accessor for the current report
        /// </summary>
        /// <returns></returns>
        public TRptReport GetCurrentReport()
        {
            return CurrentReport;
        }

        /// <summary>
        /// get the current column number
        /// </summary>
        /// <returns></returns>
        public int GetColumn()
        {
            return column;
        }

        /// <summary>
        /// get the current depth in the level hierarchy
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            return Depth;
        }

        /// <summary>
        /// get the database connection
        /// </summary>
        /// <returns></returns>
        public TDataBase GetDatabaseConnection()
        {
            return DatabaseConnection;
        }

        /// <summary>
        /// run a specified function for all rows
        /// </summary>
        /// <param name="processRow"></param>
        public void ProcessAllRows(TProcessResult processRow)
        {
            foreach (TResult element in Results.GetResults())
            {
                processRow(this, element);
            }
        }
    }
}