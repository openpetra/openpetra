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
using Ict.Petra.Shared.Interfaces.MCommon.Cacheable;
using Ict.Petra.Shared.Interfaces.MCommon.UIConnectors;
using Ict.Petra.Shared.Interfaces.MCommon.DataReader;
#region ManualCode
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
#endregion ManualCode
namespace Ict.Petra.Shared.Interfaces.MCommon
{
    /// <summary>auto generated</summary>
    public interface IMCommonNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ICacheableNamespace Cacheable
        {
            get;
        }

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


namespace Ict.Petra.Shared.Interfaces.MCommon.Cacheable
{
    /// <summary>auto generated</summary>
    public interface ICacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.Cacheable.Class)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MCommon.TCacheableCommonTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MCommon.TCacheableCommonTablesEnum ACacheableTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MCommon.TCacheableCommonTablesEnum ACacheableTable,
                                   out System.Data.DataTable ADataTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.Cacheable.Class)</summary>
        TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableCommonTablesEnum ACacheableTable,
                                                               ref TTypedDataTable ASubmitTable,
                                                               out TVerificationResultCollection AVerificationResult);
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
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.DataReader.Class)</summary>
        System.Boolean GetData(System.String ATablename,
                               TSearchCriteria[] ASearchCriteria,
                               out Ict.Common.Data.TTypedDataTable AResultTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MCommon.Instantiator.DataReader.Class)</summary>
        TSubmitChangesResult SaveData(System.String ATablename,
                                      ref TTypedDataTable ASubmitTable,
                                      out TVerificationResultCollection AVerificationResult);
    }

}

