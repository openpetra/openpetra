//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Security.Principal;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.Security;

namespace Ict.Petra.Server.App.Core
{
    /// <summary>
    /// tracks the progress for long running jobs
    /// </summary>
    public class TProgressTracker
    {
        /// <summary>
        /// current state of the long-running procedure
        /// </summary>
        public class TProgressState
        {
            /// percentage done
            public int PercentageDone = -1;
            /// overall amount
            public decimal AbsoluteOverallAmount = 100.0m;
            /// status message, which changes during the procedure
            public string StatusMessage = string.Empty;
            /// caption, overall description of job
            public string Caption = string.Empty;
            /// the client can ask the procedure to stop
            public bool CancelJob = false;
            /// if the job has finished, this is set to true. note: sometimes percentage might be inaccurate, or not present at all
            public bool JobFinished = false;
        }

        private static SortedList <string, TProgressState>FProgressStates = new SortedList <string, TProgressState>();

        /// <summary>
        /// add or reuse a tracker for the given clientID
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ACaption"></param>
        /// <param name="AAbsoluteOverallAmount"></param>
        /// <returns></returns>
        static public TProgressState InitProgressTracker(string AClientID, string ACaption, decimal AAbsoluteOverallAmount = 100.0m)
        {
            TProgressState state = new TProgressState();

            state.AbsoluteOverallAmount = AAbsoluteOverallAmount;
            state.Caption = ACaption;

            if (FProgressStates.ContainsKey(AClientID))
            {
                FProgressStates[AClientID] = state;
            }
            else
            {
                FProgressStates.Add(AClientID, state);
            }

            return state;
        }

        /// <summary>
        /// get the current state, by clientID
        /// </summary>
        /// <param name="AClientID"></param>
        /// <returns></returns>
        static public TProgressState GetCurrentState(string AClientID)
        {
            if (FProgressStates.ContainsKey(AClientID))
            {
                if (FProgressStates[AClientID].PercentageDone > 100)
                {
                    TLogging.Log("invalid percentage: " + FProgressStates[AClientID].PercentageDone.ToString());
                    FProgressStates[AClientID].PercentageDone = 99;
                }

                return FProgressStates[AClientID];
            }

            return new TProgressState();
        }

        static private int DEBUG_PROGRESS = 1;

        /// <summary>
        /// set the current state
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="AStatusMessage"></param>
        /// <param name="ACurrentAbsolutAmount"></param>
        static public void SetCurrentState(string AClientID, string AStatusMessage, Decimal ACurrentAbsolutAmount)
        {
            if (FProgressStates.ContainsKey(AClientID))
            {
                TProgressState state = FProgressStates[AClientID];
                state.StatusMessage = AStatusMessage;
                state.PercentageDone = Convert.ToInt32((ACurrentAbsolutAmount / state.AbsoluteOverallAmount) * 100.0m);

                if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                {
                    TLogging.Log(state.PercentageDone.ToString() + "%: " + state.StatusMessage);
                }
            }
        }

        /// <summary>
        /// the client can cancel the job
        /// </summary>
        /// <param name="AClientID"></param>
        static public bool CancelJob(string AClientID)
        {
            if (FProgressStates.ContainsKey(AClientID))
            {
                TProgressState state = FProgressStates[AClientID];
                state.CancelJob = true;

                if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                {
                    TLogging.Log("Cancelled the job for " + AClientID);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// the server will set the job to finished
        /// </summary>
        static public bool FinishJob(string AClientID)
        {
            if (FProgressStates.ContainsKey(AClientID))
            {
                TProgressState state = FProgressStates[AClientID];
                state.JobFinished = true;

                if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                {
                    TLogging.Log("Finished the job for " + AClientID);
                }

                return true;
            }

            return false;
        }
    }
}