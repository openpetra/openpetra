//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using Ict.Common;

namespace Ict.Common.Remoting.Shared
{
    /// <summary>
    /// IAsynchronousExecutionProgress is an interface that is used by the Client
    /// to make calls to TAsynchronousExecutionProgress objects.
    /// </summary>
    public interface IAsynchronousExecutionProgress : IInterface
    {
        /// <summary>
        /// some text information about current progress
        /// </summary>
        String ProgressInformation
        {
            get;
            set;
        }

        /// <summary>
        /// progress percentage
        /// </summary>
        Int16 ProgressPercentage
        {
            get;
            set;
        }

        /// <summary>
        /// progress state
        /// </summary>
        TAsyncExecProgressState ProgressState
        {
            get;
            set;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        object Result
        {
            get;
            set;
        }

        /// <summary>
        /// get all 3 properties at once
        /// </summary>
        /// <param name="ProgressState"></param>
        /// <param name="ProgressPercentage"></param>
        /// <param name="ProgressInformation"></param>
        void ProgressCombinedInfo(out TAsyncExecProgressState ProgressState, out Int16 ProgressPercentage, out String ProgressInformation);

        /// <summary>
        /// cancel the operation that is monitored
        /// </summary>
        void Cancel();
    }
}