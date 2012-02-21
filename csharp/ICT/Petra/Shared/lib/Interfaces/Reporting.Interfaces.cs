// Auto generated with nant generateGlue, based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
// and the Server c# files (eg. UIConnector implementations)
// Do not change this file manually.
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MReporting.LogicConnectors;
namespace Ict.Petra.Shared.Interfaces.MReporting
{
    /// <summary>auto generated</summary>
    public interface IMReportingNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ILogicConnectorsNamespace LogicConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MReporting.LogicConnectors
{
    /// <summary>auto generated</summary>
    public interface ILogicConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        IReportGeneratorLogicConnector ReportGenerator();
    }

    /// <summary>auto generated</summary>
    public interface IReportGeneratorLogicConnector : IInterface
    {
        /// <summary>auto generated from Connector property (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary>auto generated from Connector property (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        DataTable Result
        {
            get;
        }

        /// <summary>auto generated from Connector property (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        DataTable Parameter
        {
            get;
        }

        /// <summary>auto generated from Connector property (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        Boolean Success
        {
            get;
        }

        /// <summary>auto generated from Connector property (Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        String ErrorMessage
        {
            get;
        }

        /// <summary> auto generated from Connector method(Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        void Start(System.Data.DataTable AParameters);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MReporting.LogicConnectors.TReportGeneratorLogicConnector)</summary>
        void Cancel();
    }

}

