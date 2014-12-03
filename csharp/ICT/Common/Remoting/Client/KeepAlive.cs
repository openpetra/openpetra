//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Runtime.Remoting;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Remoting.Shared;

namespace Ict.Common.Remoting.Client
{
    /// <summary>
    /// Provides liftime-handling of UIConnector Objects.
    /// </summary>
    public static class TUIConnectorLifetimeHandling
    {
        /// <summary>
        /// 'Releases' an instantiated UIConnector Object on the server side so it can get Garbage Collected there.
        /// </summary>
        /// <param name="ARemotedObject">UIConnector object. Must be casted to <see cref="IDisposable" />.</param>
        public static void ReleaseUIConnector(IDisposable ARemotedObject)
        {
            ARemotedObject.Dispose();
            TLogging.LogAtLevel(4, "TUIConnectorLifetimeHandling.ReleaseUIConnector: " + ARemotedObject.GetType().ToString() + ": Dispose() got called on the 'client-glue' Object.");
        }
    }
}