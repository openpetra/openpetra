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
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MPersonnel.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person;
using Ict.Petra.Shared.Interfaces.MPersonnel.TableMaintenance;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications.Cacheable;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Cacheable;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.TableMaintenance.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.Cacheable;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.UIConnectors;
#region ManualCode
using Ict.Common.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
#endregion ManualCode
namespace Ict.Petra.Shared.Interfaces.MPersonnel
{
    /// <summary>auto generated</summary>
    public interface IMPersonnelNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IWebConnectorsNamespace WebConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPersonNamespace Person
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ITableMaintenanceNamespace TableMaintenance
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IUnitsNamespace Units
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector)</summary>
        TSubmitChangesResult SavePersonnelTDS(ref PersonnelTDS AInspectDS,
                                              out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector)</summary>
        PersonnelTDS LoadPersonellStaffData(Int64 APartnerKey);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person
{
    /// <summary>auto generated</summary>
    public interface IPersonNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPersonDataElementsNamespace DataElements
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPersonShepherdsNamespace Shepherds
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPersonDataElementsApplicationsNamespace Applications
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPersonDataElementsCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPersonDataElementsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsApplicationsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPersonDataElementsApplicationsCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPersonDataElementsApplicationsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications.Cacheable
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsApplicationsCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.Cacheable.TPersonDataElementsApplicationsCacheableNamespace)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonTablesEnum ACacheableTable);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Applications.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsApplicationsUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.UIConnectors.TPersonDataElementsApplicationsUIConnectorsNamespace)</summary>
        Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                    System.Int32 AApplicationKey,
                                                                                                                                    System.Int64 ARegistrationOffice,
                                                                                                                                    Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                    out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.Cacheable
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable.TPersonDataElementsCacheableNamespace)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable.TPersonDataElementsCacheableNamespace)</summary>
        TSubmitChangesResult SaveChangedStandardCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonTablesEnum ACacheableTable,
                                                               ref TTypedDataTable ASubmitTable,
                                                               out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPersonDataElementsUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.UIConnectors.TPersonDataElementsUIConnectorsNamespace)</summary>
        Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                    Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                    out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds
{
    /// <summary>auto generated</summary>
    public interface IPersonShepherdsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPersonShepherdsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPersonShepherdsUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.TableMaintenance
{
    /// <summary>auto generated</summary>
    public interface ITableMaintenanceNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ITableMaintenanceUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.TableMaintenance.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface ITableMaintenanceUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Units
{
    /// <summary>auto generated</summary>
    public interface IUnitsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IUnitsDataElementsNamespace DataElements
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements
{
    /// <summary>auto generated</summary>
    public interface IUnitsDataElementsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IUnitsDataElementsCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IUnitsDataElementsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.Cacheable
{
    /// <summary>auto generated</summary>
    public interface IUnitsDataElementsCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable.TUnitsDataElementsCacheableNamespace)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheableUnitTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable.TUnitsDataElementsCacheableNamespace)</summary>
        TSubmitChangesResult SaveChangedStandardCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheableUnitTablesEnum ACacheableTable,
                                                               ref TTypedDataTable ASubmitTable,
                                                               out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IUnitsDataElementsUIConnectorsNamespace : IInterface
    {
    }

}

