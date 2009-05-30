/* Auto generated with nant generateGlue, based on u:\OpenPetra\csharp\ICT\Petra\Definitions\NamespaceHierarchy.xml
 * and the Server c# files (eg. UIConnector implementations)
 * Do not change this file manually.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
#region ManualCode
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;
#endregion ManualCode
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

