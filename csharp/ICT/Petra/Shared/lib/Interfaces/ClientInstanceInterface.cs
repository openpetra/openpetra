//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using Ict.Common;
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;

namespace Ict.Petra.Shared.Interfaces
{
    /**
     * Used by the Client to poll for ClientTasks.
     *
     */
    public interface IPollClientTasksInterface : IInterface
    {
        /// <summary>
        /// check which tasks are available
        /// </summary>
        /// <returns></returns>
        DataTable PollClientTasks();
    }

    /** Only for experimenting purposes!
     */
    public interface IClientInstanceInterface : IInterface
    {
        /// <summary>
        /// for experimenting
        /// </summary>
        /// <param name="HelloString"></param>
        void Hello(out String HelloString);
    }

    /** Only for experimenting purposes!
     */
    public interface IClientInstanceInterface2 : IInterface
    {
        /// <summary>
        /// only for experimenting
        /// </summary>
        /// <param name="HelloString"></param>
        void Hello2(out String HelloString);
    }

    /** Only for experimenting purposes!
     */
    public interface IClientAsyncProgressDemoInterface : IInterface
    {
        /// <summary>
        /// get the progress
        /// </summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary>
        /// start the operation
        /// </summary>
        void Start();
    }

    /** Only for experimenting purposes!
     */
    public interface IRemoteFactory : IInterface
    {
    }
}