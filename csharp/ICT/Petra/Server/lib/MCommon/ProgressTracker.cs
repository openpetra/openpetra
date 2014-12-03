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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using System.Data;

namespace Ict.Petra.Server.MCommon.WebConnectors
{
    /// <summary>
    /// this connector allows tracking of long-running procedures, using a webconnector.
    /// currently only supports one tracker per client
    /// </summary>
    public class TProgressTrackerWebConnector
    {
        /// <summary>
        /// reset the tracker
        /// </summary>
        [RequireModulePermission("NONE")]
        public static bool Reset()
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), string.Empty, 100.0m);

            return true;
        }

        /// <summary>
        /// get the current state
        /// </summary>
        [RequireModulePermission("NONE")]
        public static bool GetCurrentState(out string ACaption, out string AStatusMessage, out int APercentageDone, out bool AJobFinished)
        {
            TProgressState state = TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString());

            ACaption = state.Caption;
            AStatusMessage = state.StatusMessage;
            APercentageDone = state.PercentageDone;
            AJobFinished = state.JobFinished;

            return state.PercentageDone != -1 || state.StatusMessage != string.Empty;
        }

        /// <summary>
        /// cancel the currently running job
        /// </summary>
        [RequireModulePermission("NONE")]
        public static bool CancelJob()
        {
            return TProgressTracker.CancelJob(DomainManager.GClientID.ToString());
        }
    }
}