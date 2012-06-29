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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Collections;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Tests.IctCommonRemoting.Interface;

namespace Tests.IctCommonRemoting.Client
{
    /// <summary>
    /// The TConnector class is responsible for opening a connection to the
    /// PetraServer's ClientManager and to retrieve Remoting Proxy objects for
    /// the Server-side .NET Remoting Sponsor and several other remoted objects from
    /// the PetraServer.
    /// </summary>
    public class TConnector : TConnectorBase
    {
    }
}