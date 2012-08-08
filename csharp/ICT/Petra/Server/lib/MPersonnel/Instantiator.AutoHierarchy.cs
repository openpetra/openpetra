// Auto generated with nant generateGlue
// based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
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

//
// Contains a remotable class that instantiates an Object which gives access to
// the MPartner Namespace (from the Client's perspective).
//
// The purpose of the remotable class is to present other classes which are
// Instantiators for sub-namespaces to the Client. The instantiation of the
// sub-namespace objects is completely transparent to the Client!
// The remotable class itself gets instantiated and dynamically remoted by the
// loader class, which in turn gets called when the Client Domain is set up.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using Ict.Common;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core.Security;

using Ict.Petra.Shared.Interfaces.MPersonnel;
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
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.DataElements.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds;
using Ict.Petra.Shared.Interfaces.MPersonnel.Person.Shepherds.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.TableMaintenance.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.Cacheable;
using Ict.Petra.Shared.Interfaces.MPersonnel.Units.DataElements.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.WebConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person;
using Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance;
using Ict.Petra.Server.MPersonnel.Instantiator.Units;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.WebConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.UIConnectors;
using Ict.Petra.Server.MPersonnel.WebConnectors;
//using Ict.Petra.Server.MPersonnel.Person;
//using Ict.Petra.Server.MPersonnel.TableMaintenance;
//using Ict.Petra.Server.MPersonnel.Units;
//using Ict.Petra.Server.MPersonnel.Person.DataElements;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications.Cacheable;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Cacheable;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.UIConnectors;
using Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors;
//using Ict.Petra.Server.MPersonnel.Person.Shepherds;
//using Ict.Petra.Server.MPersonnel.Person.Shepherds.UIConnectors;
//using Ict.Petra.Server.MPersonnel.TableMaintenance.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Units.DataElements;
//using Ict.Petra.Server.MPersonnel.Units.DataElements.Cacheable;
//using Ict.Petra.Server.MPersonnel.Units.DataElements.UIConnectors;

#region ManualCode
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.UIConnectors;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
#endregion ManualCode
namespace Ict.Petra.Server.MPersonnel.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMPersonnelNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMPersonnel FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMPersonnel
        /// class to make it callable remotely from the Client.
        ///
        /// @comment This function gets called from TRemoteLoader.LoadPetraModuleAssembly.
        /// This call is done late-bound through .NET Reflection!
        ///
        /// WARNING: If the name of this function or its parameters should change, this
        /// needs to be reflected in the call to this function in
        /// TRemoteLoader.LoadPetraModuleAssembly!!!
        ///
        /// </summary>
        /// <returns>The URL at which the remoted object can be reached.</returns>
        public String GetRemotingURL()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMPersonnelNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMPersonnel();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMPersonnelNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMPersonnel GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MPersonnel Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMPersonnel : TConfigurableMBRObject, IMPersonnelNamespace
    {
        /// <summary>Constructor</summary>
        public TMPersonnel()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMPersonnel object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'WebConnectors' subnamespace contains further subnamespaces.</summary>
        public IWebConnectorsNamespace WebConnectors
        {
            get
            {
                return (IWebConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IWebConnectorsNamespace),
                        new TWebConnectorsNamespace());
            }
        }
        /// <summary>The 'Person' subnamespace contains further subnamespaces.</summary>
        public IPersonNamespace Person
        {
            get
            {
                return (IPersonNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonNamespace),
                        new TPersonNamespace());
            }
        }
        /// <summary>The 'TableMaintenance' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceNamespace TableMaintenance
        {
            get
            {
                return (ITableMaintenanceNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ITableMaintenanceNamespace),
                        new TTableMaintenanceNamespace());
            }
        }
        /// <summary>The 'Units' subnamespace contains further subnamespaces.</summary>
        public IUnitsNamespace Units
        {
            get
            {
                return (IUnitsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IUnitsNamespace),
                        new TUnitsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. WebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TWebConnectorsNamespace : TConfigurableMBRObject, IWebConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public TSubmitChangesResult SavePersonnelTDS(ref PersonnelTDS AInspectDS,
                                                     out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "SavePersonnelTDS", ";PERSONNELTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.SavePersonnelTDS(ref AInspectDS, out AVerificationResult);
        }

        /// generated method from connector
        public PersonnelTDS LoadPersonellStaffData(Int64 APartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "LoadPersonellStaffData", ";LONG;");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.LoadPersonellStaffData(APartnerKey);
        }

        /// generated method from connector
        public System.Boolean HasCurrentCommitmentRecord(Int64 APartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "HasCurrentCommitmentRecord", ";LONG;");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.HasCurrentCommitmentRecord(APartnerKey);
        }

        /// generated method from connector
        public System.Int32 GetOrCreateUmJobKey(Int64 AUnitKey,
                                                System.String APositionName,
                                                System.String APositionScope)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "GetOrCreateUmJobKey", ";LONG;STRING;STRING;");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.GetOrCreateUmJobKey(AUnitKey, APositionName, APositionScope);
        }

        /// generated method from connector
        public ArrayList GetUnitHeirarchy()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "GetUnitHeirarchy", ";");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.GetUnitHeirarchy();
        }

        /// generated method from connector
        public System.Boolean SaveUnitHierarchy(ArrayList Nodes)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector), "SaveUnitHierarchy", ";ARRAYLIST;");
            return Ict.Petra.Server.MPersonnel.WebConnectors.TPersonnelWebConnector.SaveUnitHierarchy(Nodes);
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person
{
    /// <summary>
    /// REMOTEABLE CLASS. Person Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonNamespace : TConfigurableMBRObject, IPersonNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'PersonDataElements' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsNamespace DataElements
        {
            get
            {
                return (IPersonDataElementsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsNamespace),
                        new TPersonDataElementsNamespace());
            }
        }
        /// <summary>The 'PersonShepherds' subnamespace contains further subnamespaces.</summary>
        public IPersonShepherdsNamespace Shepherds
        {
            get
            {
                return (IPersonShepherdsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonShepherdsNamespace),
                        new TPersonShepherdsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElements Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsNamespace : TConfigurableMBRObject, IPersonDataElementsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'PersonDataElementsApplications' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsNamespace Applications
        {
            get
            {
                return (IPersonDataElementsApplicationsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsApplicationsNamespace),
                        new TPersonDataElementsApplicationsNamespace());
            }
        }
        /// <summary>The 'PersonDataElementsCacheable' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsCacheableNamespace Cacheable
        {
            get
            {
                return (IPersonDataElementsCacheableNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsCacheableNamespace),
                        new TPersonDataElementsCacheableNamespace());
            }
        }
        /// <summary>The 'PersonDataElementsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IPersonDataElementsUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsUIConnectorsNamespace),
                        new TPersonDataElementsUIConnectorsNamespace());
            }
        }
        /// <summary>The 'PersonDataElementsWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsWebConnectorsNamespace WebConnectors
        {
            get
            {
                return (IPersonDataElementsWebConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsWebConnectorsNamespace),
                        new TPersonDataElementsWebConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsApplications Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsNamespace : TConfigurableMBRObject, IPersonDataElementsApplicationsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'PersonDataElementsApplicationsCacheable' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsCacheableNamespace Cacheable
        {
            get
            {
                return (IPersonDataElementsApplicationsCacheableNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsApplicationsCacheableNamespace),
                        new TPersonDataElementsApplicationsCacheableNamespace());
            }
        }
        /// <summary>The 'PersonDataElementsApplicationsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IPersonDataElementsApplicationsUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonDataElementsApplicationsUIConnectorsNamespace),
                        new TPersonDataElementsApplicationsUIConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsApplicationsCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsCacheableNamespace : TConfigurableMBRObject, IPersonDataElementsApplicationsCacheableNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsCacheableNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsCacheableNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable)
        {
            #region ManualCode
            return null;
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsApplicationsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsUIConnectorsNamespace : TConfigurableMBRObject, IPersonDataElementsApplicationsUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                           System.Int32 AApplicationKey,
                                                                                                                                           System.Int64 ARegistrationOffice,
                                                                                                                                           Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                           out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
        {
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey,
               AApplicationKey,
               ARegistrationOffice,
               AOfficeSpecificDataLabelUse);

            AOfficeSpecificDataLabelsDataSet = ReturnValue.GetData();
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsCacheableNamespace : TConfigurableMBRObject, IPersonDataElementsCacheableNamespace
    {
        #region ManualCode
        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TPersonDataElementsCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsCacheableNamespace object exists until this AppDomain is unloaded!
        }

        #region ManualCode

        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private DataTable GetCacheableTableInternal(TCacheablePersonTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheablePersonTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheablePersonTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode

            //todo
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode            
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsUIConnectorsNamespace : TConfigurableMBRObject, IPersonDataElementsUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                           Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                           out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
        {
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AOfficeSpecificDataLabelUse);

            AOfficeSpecificDataLabelsDataSet = ReturnValue.GetData();
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonDataElementsWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonDataElementsWebConnectorsNamespace : TConfigurableMBRObject, IPersonDataElementsWebConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonDataElementsWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public IndividualDataTDS GetData(Int64 APartnerKey,
                                         TIndividualDataItemEnum AIndivDataItem)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors.TIndividualDataWebConnector), "GetData", ";LONG;TINDIVIDUALDATAITEMENUM;");
            return Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors.TIndividualDataWebConnector.GetData(APartnerKey, AIndivDataItem);
        }

        /// generated method from connector
        public System.Boolean GetSummaryData(Int64 APartnerKey,
                                             ref IndividualDataTDS AIndividualDataDS)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors.TIndividualDataWebConnector), "GetSummaryData", ";LONG;INDIVIDUALDATATDS;");
            return Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors.TIndividualDataWebConnector.GetSummaryData(APartnerKey, ref AIndividualDataDS);
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonShepherds Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonShepherdsNamespace : TConfigurableMBRObject, IPersonShepherdsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonShepherdsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonShepherdsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'PersonShepherdsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonShepherdsUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IPersonShepherdsUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPersonShepherdsUIConnectorsNamespace),
                        new TPersonShepherdsUIConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PersonShepherdsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPersonShepherdsUIConnectorsNamespace : TConfigurableMBRObject, IPersonShepherdsUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TPersonShepherdsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonShepherdsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance
{
    /// <summary>
    /// REMOTEABLE CLASS. TableMaintenance Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceNamespace : TConfigurableMBRObject, ITableMaintenanceNamespace
    {
        /// <summary>Constructor</summary>
        public TTableMaintenanceNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'TableMaintenanceUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (ITableMaintenanceUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ITableMaintenanceUIConnectorsNamespace),
                        new TTableMaintenanceUIConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. TableMaintenanceUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceUIConnectorsNamespace : TConfigurableMBRObject, ITableMaintenanceUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TTableMaintenanceUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Units
{
    /// <summary>
    /// REMOTEABLE CLASS. Units Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TUnitsNamespace : TConfigurableMBRObject, IUnitsNamespace
    {
        /// <summary>Constructor</summary>
        public TUnitsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'UnitsDataElements' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsNamespace DataElements
        {
            get
            {
                return (IUnitsDataElementsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IUnitsDataElementsNamespace),
                        new TUnitsDataElementsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements
{
    /// <summary>
    /// REMOTEABLE CLASS. UnitsDataElements Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsNamespace : TConfigurableMBRObject, IUnitsDataElementsNamespace
    {
        /// <summary>Constructor</summary>
        public TUnitsDataElementsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'UnitsDataElementsCacheable' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsCacheableNamespace Cacheable
        {
            get
            {
                return (IUnitsDataElementsCacheableNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IUnitsDataElementsCacheableNamespace),
                        new TUnitsDataElementsCacheableNamespace());
            }
        }
        /// <summary>The 'UnitsDataElementsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IUnitsDataElementsUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IUnitsDataElementsUIConnectorsNamespace),
                        new TUnitsDataElementsUIConnectorsNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. UnitsDataElementsCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsCacheableNamespace : TConfigurableMBRObject, IUnitsDataElementsCacheableNamespace
    {
        #region ManualCode
        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TUnitsDataElementsCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsCacheableNamespace object exists until this AppDomain is unloaded!
        }

        #region ManualCode

        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private DataTable GetCacheableTableInternal(TCacheableUnitTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableUnitTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableUnitTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableUnitTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableUnitTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode            
        }
    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. UnitsDataElementsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsUIConnectorsNamespace : TConfigurableMBRObject, IUnitsDataElementsUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TUnitsDataElementsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

