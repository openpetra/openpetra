//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
        private static SortedList <string, TProgressState>FProgressStates = new SortedList <string, TProgressState>();
        private static Object ProgressLockObject = new Object();

        /// <summary>
        /// add or reuse a tracker for the given clientID
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="ACaption"></param>
        /// <param name="AAbsoluteOverallAmount"></param>
        /// <returns></returns>
        static public TProgressState InitProgressTracker(string AClientID, string ACaption, decimal AAbsoluteOverallAmount = 100.0m)
        {
            lock (ProgressLockObject)
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
        }

        /// <summary>
        /// get the current state, by clientID
        /// </summary>
        /// <param name="AClientID"></param>
        /// <returns></returns>
        static public TProgressState GetCurrentState(string AClientID)
        {
            lock (ProgressLockObject)
            {
                if ((AClientID != null) && FProgressStates.ContainsKey(AClientID))
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
        }

        static private int DEBUG_PROGRESS = 1;

        /// <summary>
        /// set the current state
        /// </summary>
        /// <param name="AClientID"></param>
        /// <param name="AStatusMessage"></param>
        /// <param name="ACurrentAbsoluteAmount"></param>
        static public void SetCurrentState(string AClientID, string AStatusMessage, Decimal ACurrentAbsoluteAmount)
        {
            lock (ProgressLockObject)
            {
                if (FProgressStates.ContainsKey(AClientID))
                {
                    TProgressState state = FProgressStates[AClientID];

                    if (AStatusMessage.Length > 0)
                    {
                        state.StatusMessage = AStatusMessage;
                    }

                    state.PercentageDone = Convert.ToInt32((ACurrentAbsoluteAmount / state.AbsoluteOverallAmount) * 100.0m);

                    if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                    {
                        // avoid recursive calls, especially during report calculation
                        Console.WriteLine(state.PercentageDone.ToString() + "%: " + state.StatusMessage);
                    }
                }
            }
        }

        /// <summary>
        /// the client can cancel the job
        /// </summary>
        /// <param name="AClientID"></param>
        static public bool CancelJob(string AClientID)
        {
            lock (ProgressLockObject)
            {
                if (FProgressStates.ContainsKey(AClientID))
                {
                    TProgressState state = FProgressStates[AClientID];

                    if (state.JobFinished == true)
                    {
                        if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                        {
                            TLogging.Log("Cannot cancel the job for " + AClientID + " because the job has already finished");
                        }
                    }
                    else
                    {
                        state.CancelJob = true;

                        if (TLogging.DebugLevel >= DEBUG_PROGRESS)
                        {
                            TLogging.Log("Cancelled the job for " + AClientID);
                        }

                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// the server will set the job to finished
        /// </summary>
        static public bool FinishJob(string AClientID)
        {
            lock (ProgressLockObject)
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
}
