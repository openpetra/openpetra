/* Auto generated with nant generateGlue, based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.xml
 * and the Server c# files (eg. UIConnector implementations)
 * Do not change this file manually.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MCommon.UIConnectors;
using Ict.Petra.Shared.Interfaces.MCommon.DataReader;
#region ManualCode
using Ict.Common.DB;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
#endregion ManualCode
namespace Ict.Petra.Shared.Interfaces.MCommon
{
    /// <summary>auto generated</summary>
    public interface IMCommonNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IDataReaderNamespace DataReader
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MCommon.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                   TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                   TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                   ref OfficeSpecificDataLabelsTDS ADataSet,
                                                                                   TDBTransaction AReadTransaction);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                   Int32 AApplicationKey,
                                                                                   Int64 ARegistrationOffice,
                                                                                   TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                   Int32 AApplicationKey,
                                                                                   Int64 ARegistrationOffice,
                                                                                   TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                   ref OfficeSpecificDataLabelsTDS ADataSet,
                                                                                   TDBTransaction AReadTransaction);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MCommon.UIConnectors.TFieldOfServiceUIConnector)</summary>
        IPartnerUIConnectorsFieldOfService FieldOfService(Int64 APartnerKey);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MCommon.UIConnectors.TFieldOfServiceUIConnector)</summary>
        IPartnerUIConnectorsFieldOfService FieldOfService(Int64 APartnerKey,
                                                          ref FieldOfServiceTDS ADataSet);
    }

    /// <summary>auto generated</summary>
    public interface IDataElementsUIConnectorsOfficeSpecificDataLabels : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        OfficeSpecificDataLabelsTDS GetData();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        TSubmitChangesResult SubmitChanges(ref OfficeSpecificDataLabelsTDS AInspectDS,
                                           out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MCommon.UIConnectors.TOfficeSpecificDataLabelsUIConnector)</summary>
        Boolean GetPartnerShortName(Int64 APartnerKey,
                                    out String APartnerShortName,
                                    out TPartnerClass APartnerClass);
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsFieldOfService : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MCommon.UIConnectors.TFieldOfServiceUIConnector)</summary>
        FieldOfServiceTDS GetData();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MCommon.UIConnectors.TFieldOfServiceUIConnector)</summary>
        TSubmitChangesResult SubmitChanges(ref FieldOfServiceTDS AInspectDS,
                                           out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MCommon.DataReader
{
    /// <summary>auto generated</summary>
    public interface IDataReaderNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.DataReader.TDataReaderNamespace)</summary>
        bool GetData(string ATablename,
                     Ict.Common.Data.TTypedDataTable AKeys,
                     out Ict.Common.Data.TTypedDataTable AResultTable);
    }

}

