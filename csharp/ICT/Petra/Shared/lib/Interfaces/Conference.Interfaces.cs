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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.Interfaces.MConference.Cacheable;
using Ict.Petra.Shared.Interfaces.MConference.WebConnectors;
#region ManualCode
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MConference;
#endregion ManualCode
namespace Ict.Petra.Shared.Interfaces.MConference
{
    /// <summary>auto generated</summary>
    public interface IMConferenceNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ICacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MConference.Cacheable
{
    /// <summary>auto generated</summary>
    public interface ICacheableNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MConference.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions)</summary>
        PUnitTable GetOutreachOptions(Int64 AUnitKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions)</summary>
        SelectConferenceTDS GetConferences(String AConferenceName,
                                           String APrefix);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions)</summary>
        System.Boolean GetEarliestAndLatestDate(Int64 AConferenceKey,
                                                out DateTime AEarliestArrivalDate,
                                                out DateTime ALatestDepartureDate,
                                                out DateTime AStartDate,
                                                out DateTime AEndDate);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions)</summary>
        System.Boolean GetOutreachOptions(System.Int64 AUnitKey,
                                          out System.Data.DataTable AConferenceTable);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions)</summary>
        System.Boolean GetFieldUnits(Int64 AConferenceKey,
                                     TUnitTypeEnum AFieldTypes,
                                     out DataTable AFieldsTable,
                                     out String AConferencePrefix);
    }

}

