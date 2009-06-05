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
namespace Ict.Petra.Shared.Interfaces.MPersonnel
{
    /// <summary>auto generated</summary>
    public interface IMPersonnelNamespace : IInterface
    {
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
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonDataElementsTablesEnum ACacheableTable);
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
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonDataElementsTablesEnum ACacheableTable);
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
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheableUnitsDataElementsTablesEnum ACacheableTable);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IUnitsDataElementsUIConnectorsNamespace : IInterface
    {
    }

}

