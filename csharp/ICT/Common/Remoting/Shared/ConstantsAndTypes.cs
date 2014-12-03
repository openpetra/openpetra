//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.Data;
using Ict.Common;

namespace Ict.Common.Remoting.Shared
{
    /// <summary>
    /// Surrogate Base Interface
    /// All Interfaces can safely derive from this Interface. The reason why we have this is:
    /// that all Types that implement any Interfaces that themselves derives from IInterface
    /// can be cast to IInterface (and passed as Function Argument: eg. IInterface AnObject).
    /// This is important for .NET Remoting scenarios.
    ///
    /// </summary>
    public interface IInterface
    {
    }

    /// <summary>
    /// some constants that are useful for Remoting
    /// </summary>
    public class RemotingConstants
    {
        /// <summary>Used as a 'separator character' for TClientTasksManager when there is a need to concatenate values in a String.</summary>
        public const String GCLIENTTASKPARAMETER_SEPARATOR = "?";

        /// <summary>Remoting URL Identifiers</summary>
        public const String REMOTINGURL_IDENTIFIER_POLLCLIENTTASKS = "PollClientTasks";

        /// <summary>ClientTask TaskGroups</summary>
        public const String CLIENTTASKGROUP_DISCONNECT = "DISCONNECT";
    }

    /// an interface for cacheable tables manager
    public interface ICacheableTablesManager
    {
    }
}