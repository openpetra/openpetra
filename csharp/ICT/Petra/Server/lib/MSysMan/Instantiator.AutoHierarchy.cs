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

using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.Interfaces.MSysMan.Application;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance;
using Ict.Petra.Shared.Interfaces.MSysMan.TableMaintenance;
using Ict.Petra.Shared.Interfaces.MSysMan.ImportExport;
using Ict.Petra.Shared.Interfaces.MSysMan.PrintManagement;
using Ict.Petra.Shared.Interfaces.MSysMan.Security;
using Ict.Petra.Shared.Interfaces.MSysMan.Cacheable;
using Ict.Petra.Shared.Interfaces.MSysMan.Application.UIConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.Application.ServerLookups;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance.SystemDefaults;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance.UIConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance.UserDefaults;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.TableMaintenance.UIConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.PrintManagement.UIConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.Security.UIConnectors;
using Ict.Petra.Shared.Interfaces.MSysMan.Security.UserManager;
using Ict.Petra.Server.MSysMan.Instantiator.Application;
using Ict.Petra.Server.MSysMan.Instantiator.Maintenance;
using Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance;
using Ict.Petra.Server.MSysMan.Instantiator.ImportExport;
using Ict.Petra.Server.MSysMan.Instantiator.PrintManagement;
using Ict.Petra.Server.MSysMan.Instantiator.Security;
using Ict.Petra.Server.MSysMan.Instantiator.Cacheable;
using Ict.Petra.Server.MSysMan.Instantiator.Application.UIConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.Application.ServerLookups;
using Ict.Petra.Server.MSysMan.Instantiator.Maintenance.SystemDefaults;
using Ict.Petra.Server.MSysMan.Instantiator.Maintenance.UIConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.Maintenance.UserDefaults;
using Ict.Petra.Server.MSysMan.Instantiator.Maintenance.WebConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.ImportExport.WebConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.PrintManagement.UIConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.Security.UIConnectors;
using Ict.Petra.Server.MSysMan.Instantiator.Security.UserManager;
//using Ict.Petra.Server.MSysMan.Application;
using Ict.Petra.Server.MSysMan.Maintenance;
//using Ict.Petra.Server.MSysMan.TableMaintenance;
//using Ict.Petra.Server.MSysMan.ImportExport;
//using Ict.Petra.Server.MSysMan.PrintManagement;
using Ict.Petra.Server.MSysMan.Security;
using Ict.Petra.Server.MSysMan.Cacheable;
//using Ict.Petra.Server.MSysMan.Application.UIConnectors;
using Ict.Petra.Server.MSysMan.Application.ServerLookups;
//using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults;
//using Ict.Petra.Server.MSysMan.Maintenance.UIConnectors;
//using Ict.Petra.Server.MSysMan.Maintenance.UserDefaults;
using Ict.Petra.Server.MSysMan.Maintenance.WebConnectors;
using Ict.Petra.Server.MSysMan.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;
//using Ict.Petra.Server.MSysMan.PrintManagement.UIConnectors;
//using Ict.Petra.Server.MSysMan.Security.UIConnectors;
//using Ict.Petra.Server.MSysMan.Security.UserManager;

#region ManualCode
using Ict.Common.Verification;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MSysMan;
using Ict.Common.Data;
#endregion ManualCode
namespace Ict.Petra.Server.MSysMan.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMSysManNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMSysMan FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMSysMan
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
                Console.WriteLine("TMSysManNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMSysMan();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMSysManNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMSysMan GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MSysMan Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMSysMan : TConfigurableMBRObject, IMSysManNamespace
    {
        private TApplicationNamespaceRemote FApplicationSubNamespace;
        private TMaintenanceNamespaceRemote FMaintenanceSubNamespace;
        private TTableMaintenanceNamespaceRemote FTableMaintenanceSubNamespace;
        private TImportExportNamespaceRemote FImportExportSubNamespace;
        private TPrintManagementNamespaceRemote FPrintManagementSubNamespace;
        private TSecurityNamespaceRemote FSecuritySubNamespace;
        private TCacheableNamespaceRemote FCacheableSubNamespace;

        /// <summary>Constructor</summary>
        public TMSysMan()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMSysMan object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TApplicationNamespaceRemote: IApplicationNamespace
        {
            private IApplicationNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TApplicationNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IApplicationNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IApplicationNamespace));
            }

            /// property forwarder
            public IApplicationUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IApplicationServerLookupsNamespace ServerLookups
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.ServerLookups; }
            }
        }

        /// <summary>The 'Application' subnamespace contains further subnamespaces.</summary>
        public IApplicationNamespace Application
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.Application' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.Application' sub-namespace
                //

                // accessing TApplicationNamespace the first time? > instantiate the object
                if (FApplicationSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TApplicationNamespace");
                    TApplicationNamespace ObjectToRemote = new TApplicationNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FApplicationSubNamespace = new TApplicationNamespaceRemote(ObjectURI);
                }

                return FApplicationSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMaintenanceNamespaceRemote: IMaintenanceNamespace
        {
            private IMaintenanceNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMaintenanceNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMaintenanceNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMaintenanceNamespace));
            }

            /// property forwarder
            public IMaintenanceSystemDefaultsNamespace SystemDefaults
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.SystemDefaults; }
            }
            /// property forwarder
            public IMaintenanceUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IMaintenanceUserDefaultsNamespace UserDefaults
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UserDefaults; }
            }
            /// property forwarder
            public IMaintenanceWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'Maintenance' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceNamespace Maintenance
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.Maintenance' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.Maintenance' sub-namespace
                //

                // accessing TMaintenanceNamespace the first time? > instantiate the object
                if (FMaintenanceSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMaintenanceNamespace");
                    TMaintenanceNamespace ObjectToRemote = new TMaintenanceNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMaintenanceSubNamespace = new TMaintenanceNamespaceRemote(ObjectURI);
                }

                return FMaintenanceSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TTableMaintenanceNamespaceRemote: ITableMaintenanceNamespace
        {
            private ITableMaintenanceNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TTableMaintenanceNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ITableMaintenanceNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ITableMaintenanceNamespace));
            }

            /// property forwarder
            public ITableMaintenanceUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'TableMaintenance' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceNamespace TableMaintenance
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.TableMaintenance' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.TableMaintenance' sub-namespace
                //

                // accessing TTableMaintenanceNamespace the first time? > instantiate the object
                if (FTableMaintenanceSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TTableMaintenanceNamespace");
                    TTableMaintenanceNamespace ObjectToRemote = new TTableMaintenanceNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FTableMaintenanceSubNamespace = new TTableMaintenanceNamespaceRemote(ObjectURI);
                }

                return FTableMaintenanceSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TImportExportNamespaceRemote: IImportExportNamespace
        {
            private IImportExportNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TImportExportNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IImportExportNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IImportExportNamespace));
            }

            /// property forwarder
            public IImportExportWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'ImportExport' subnamespace contains further subnamespaces.</summary>
        public IImportExportNamespace ImportExport
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.ImportExport' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.ImportExport' sub-namespace
                //

                // accessing TImportExportNamespace the first time? > instantiate the object
                if (FImportExportSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TImportExportNamespace");
                    TImportExportNamespace ObjectToRemote = new TImportExportNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FImportExportSubNamespace = new TImportExportNamespaceRemote(ObjectURI);
                }

                return FImportExportSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPrintManagementNamespaceRemote: IPrintManagementNamespace
        {
            private IPrintManagementNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPrintManagementNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPrintManagementNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPrintManagementNamespace));
            }

            /// property forwarder
            public IPrintManagementUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'PrintManagement' subnamespace contains further subnamespaces.</summary>
        public IPrintManagementNamespace PrintManagement
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.PrintManagement' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.PrintManagement' sub-namespace
                //

                // accessing TPrintManagementNamespace the first time? > instantiate the object
                if (FPrintManagementSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPrintManagementNamespace");
                    TPrintManagementNamespace ObjectToRemote = new TPrintManagementNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPrintManagementSubNamespace = new TPrintManagementNamespaceRemote(ObjectURI);
                }

                return FPrintManagementSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSecurityNamespaceRemote: ISecurityNamespace
        {
            private ISecurityNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSecurityNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISecurityNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISecurityNamespace));
            }

            /// property forwarder
            public ISecurityUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public ISecurityUserManagerNamespace UserManager
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UserManager; }
            }
        }

        /// <summary>The 'Security' subnamespace contains further subnamespaces.</summary>
        public ISecurityNamespace Security
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.Security' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.Security' sub-namespace
                //

                // accessing TSecurityNamespace the first time? > instantiate the object
                if (FSecuritySubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSecurityNamespace");
                    TSecurityNamespace ObjectToRemote = new TSecurityNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSecuritySubNamespace = new TSecurityNamespaceRemote(ObjectURI);
                }

                return FSecuritySubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TCacheableNamespaceRemote: ICacheableNamespace
        {
            private ICacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ICacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ICacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                                           System.String AHashCode,
                                                           out System.Type AType)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetCacheableTable(ACacheableTable,AHashCode,out AType);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableSysManTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                              out System.Data.DataTable ADataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable,out ADataTable);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                                                          ref TTypedDataTable ASubmitTable,
                                                                          out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveChangedStandardCacheableTable(ACacheableTable,ref ASubmitTable,out AVerificationResult);
            }
        }

        /// <summary>The 'Cacheable' subnamespace contains further subnamespaces.</summary>
        public ICacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MSysMan.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MSysMan.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TCacheableNamespace");
                    TCacheableNamespace ObjectToRemote = new TCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FCacheableSubNamespace = new TCacheableNamespaceRemote(ObjectURI);
                }

                return FCacheableSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Application
{
    /// <summary>
    /// REMOTEABLE CLASS. Application Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TApplicationNamespace : TConfigurableMBRObject, IApplicationNamespace
    {
        private TApplicationUIConnectorsNamespaceRemote FApplicationUIConnectorsSubNamespace;
        private TApplicationServerLookupsNamespaceRemote FApplicationServerLookupsSubNamespace;

        /// <summary>Constructor</summary>
        public TApplicationNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TApplicationNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TApplicationUIConnectorsNamespaceRemote: IApplicationUIConnectorsNamespace
        {
            private IApplicationUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TApplicationUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IApplicationUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IApplicationUIConnectorsNamespace));
            }

        }

        /// <summary>The 'ApplicationUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IApplicationUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Application.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Application.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FApplicationUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TApplicationUIConnectorsNamespace");
                    TApplicationUIConnectorsNamespace ObjectToRemote = new TApplicationUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FApplicationUIConnectorsSubNamespace = new TApplicationUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FApplicationUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TApplicationServerLookupsNamespaceRemote: IApplicationServerLookupsNamespace
        {
            private IApplicationServerLookupsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TApplicationServerLookupsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IApplicationServerLookupsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IApplicationServerLookupsNamespace));
            }

            /// generated method from interface
            public System.Boolean GetDBVersion(out System.String APetraDBVersion)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetDBVersion(out APetraDBVersion);
            }
            /// generated method from interface
            public System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetInstalledPatches(out APatchLogDT);
            }
        }

        /// <summary>The 'ApplicationServerLookups' subnamespace contains further subnamespaces.</summary>
        public IApplicationServerLookupsNamespace ServerLookups
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Application.ServerLookups' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Application.ServerLookups' sub-namespace
                //

                // accessing TServerLookupsNamespace the first time? > instantiate the object
                if (FApplicationServerLookupsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TApplicationServerLookupsNamespace");
                    TApplicationServerLookupsNamespace ObjectToRemote = new TApplicationServerLookupsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FApplicationServerLookupsSubNamespace = new TApplicationServerLookupsNamespaceRemote(ObjectURI);
                }

                return FApplicationServerLookupsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Application.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. ApplicationUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TApplicationUIConnectorsNamespace : TConfigurableMBRObject, IApplicationUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TApplicationUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TApplicationUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Application.ServerLookups
{
    /// <summary>
    /// REMOTEABLE CLASS. ApplicationServerLookups Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TApplicationServerLookupsNamespace : TConfigurableMBRObject, IApplicationServerLookupsNamespace
    {

        /// <summary>Constructor</summary>
        public TApplicationServerLookupsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TApplicationServerLookupsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public System.Boolean GetDBVersion(out System.String APetraDBVersion)
        {
            #region ManualCode
            return TSysManServerLookups.GetDBVersion(out APetraDBVersion);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
        {
            #region ManualCode
            return TSysManServerLookups.GetInstalledPatches(out APatchLogDT);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance
{
    /// <summary>
    /// REMOTEABLE CLASS. Maintenance Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMaintenanceNamespace : TConfigurableMBRObject, IMaintenanceNamespace
    {
        private TMaintenanceSystemDefaultsNamespaceRemote FMaintenanceSystemDefaultsSubNamespace;
        private TMaintenanceUIConnectorsNamespaceRemote FMaintenanceUIConnectorsSubNamespace;
        private TMaintenanceUserDefaultsNamespaceRemote FMaintenanceUserDefaultsSubNamespace;
        private TMaintenanceWebConnectorsNamespaceRemote FMaintenanceWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMaintenanceNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMaintenanceSystemDefaultsNamespaceRemote: IMaintenanceSystemDefaultsNamespace
        {
            private IMaintenanceSystemDefaultsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMaintenanceSystemDefaultsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMaintenanceSystemDefaultsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMaintenanceSystemDefaultsNamespace));
            }

            /// generated method from interface
            public Ict.Petra.Shared.MSysMan.Data.SSystemDefaultsTable GetSystemDefaults()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetSystemDefaults();
            }
            /// generated method from interface
            public System.Boolean SaveSystemDefaults(Ict.Petra.Shared.MSysMan.Data.SSystemDefaultsTable ASystemDefaultsDataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveSystemDefaults(ASystemDefaultsDataTable);
            }
            /// generated method from interface
            public void ReloadSystemDefaultsTable()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.ReloadSystemDefaultsTable();
            }
        }

        /// <summary>The 'MaintenanceSystemDefaults' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceSystemDefaultsNamespace SystemDefaults
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Maintenance.SystemDefaults' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Maintenance.SystemDefaults' sub-namespace
                //

                // accessing TSystemDefaultsNamespace the first time? > instantiate the object
                if (FMaintenanceSystemDefaultsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMaintenanceSystemDefaultsNamespace");
                    TMaintenanceSystemDefaultsNamespace ObjectToRemote = new TMaintenanceSystemDefaultsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMaintenanceSystemDefaultsSubNamespace = new TMaintenanceSystemDefaultsNamespaceRemote(ObjectURI);
                }

                return FMaintenanceSystemDefaultsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMaintenanceUIConnectorsNamespaceRemote: IMaintenanceUIConnectorsNamespace
        {
            private IMaintenanceUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMaintenanceUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMaintenanceUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMaintenanceUIConnectorsNamespace));
            }

        }

        /// <summary>The 'MaintenanceUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Maintenance.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Maintenance.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FMaintenanceUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMaintenanceUIConnectorsNamespace");
                    TMaintenanceUIConnectorsNamespace ObjectToRemote = new TMaintenanceUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMaintenanceUIConnectorsSubNamespace = new TMaintenanceUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FMaintenanceUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMaintenanceUserDefaultsNamespaceRemote: IMaintenanceUserDefaultsNamespace
        {
            private IMaintenanceUserDefaultsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMaintenanceUserDefaultsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMaintenanceUserDefaultsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMaintenanceUserDefaultsNamespace));
            }

            /// generated method from interface
            public void GetUserDefaults(System.String AUserName,
                                        out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.GetUserDefaults(AUserName,out AUserDefaultsDataTable);
            }
            /// generated method from interface
            public System.Boolean SaveUserDefaults(System.String AUserName,
                                                   ref Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable,
                                                   out Ict.Common.Verification.TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveUserDefaults(AUserName,ref AUserDefaultsDataTable,out AVerificationResult);
            }
            /// generated method from interface
            public void ReloadUserDefaults(System.String AUserName,
                                           out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.ReloadUserDefaults(AUserName,out AUserDefaultsDataTable);
            }
        }

        /// <summary>The 'MaintenanceUserDefaults' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceUserDefaultsNamespace UserDefaults
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Maintenance.UserDefaults' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Maintenance.UserDefaults' sub-namespace
                //

                // accessing TUserDefaultsNamespace the first time? > instantiate the object
                if (FMaintenanceUserDefaultsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMaintenanceUserDefaultsNamespace");
                    TMaintenanceUserDefaultsNamespace ObjectToRemote = new TMaintenanceUserDefaultsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMaintenanceUserDefaultsSubNamespace = new TMaintenanceUserDefaultsNamespaceRemote(ObjectURI);
                }

                return FMaintenanceUserDefaultsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMaintenanceWebConnectorsNamespaceRemote: IMaintenanceWebConnectorsNamespace
        {
            private IMaintenanceWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMaintenanceWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMaintenanceWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMaintenanceWebConnectorsNamespace));
            }

            /// generated method from interface
            public System.Boolean SetLanguageAndCulture(System.String ALanguageCode,
                                                        System.String ACultureCode)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SetLanguageAndCulture(ALanguageCode,ACultureCode);
            }
            /// generated method from interface
            public System.Boolean LoadLanguageAndCultureFromUserDefaults()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.LoadLanguageAndCultureFromUserDefaults();
            }
            /// generated method from interface
            public System.Boolean GetLanguageAndCulture(ref System.String ALanguageCode,
                                                        ref System.String ACultureCode)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetLanguageAndCulture(ref ALanguageCode,ref ACultureCode);
            }
            /// generated method from interface
            public System.Boolean SetUserPassword(System.String AUsername,
                                                  System.String APassword)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SetUserPassword(AUsername,APassword);
            }
            /// generated method from interface
            public System.Boolean CheckPasswordQuality(System.String APassword,
                                                       out TVerificationResultCollection AVerification)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.CheckPasswordQuality(APassword,out AVerification);
            }
            /// generated method from interface
            public System.Boolean SetUserPassword(System.String AUsername,
                                                  System.String APassword,
                                                  System.String AOldPassword,
                                                  out TVerificationResultCollection AVerification)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SetUserPassword(AUsername,APassword,AOldPassword,out AVerification);
            }
            /// generated method from interface
            public System.Boolean CreateUser(System.String AUsername,
                                             System.String APassword,
                                             System.String AModulePermissions)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.CreateUser(AUsername,APassword,AModulePermissions);
            }
            /// generated method from interface
            public System.Boolean GetAuthenticationFunctionality(out System.Boolean ACanCreateUser,
                                                                 out System.Boolean ACanChangePassword,
                                                                 out System.Boolean ACanChangePermissions)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetAuthenticationFunctionality(out ACanCreateUser,out ACanChangePassword,out ACanChangePermissions);
            }
            /// generated method from interface
            public MaintainUsersTDS LoadUsersAndModulePermissions()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.LoadUsersAndModulePermissions();
            }
            /// generated method from interface
            public TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS,
                                                  out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveSUser(ref ASubmitDS,out AVerificationResult);
            }
        }

        /// <summary>The 'MaintenanceWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Maintenance.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Maintenance.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FMaintenanceWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMaintenanceWebConnectorsNamespace");
                    TMaintenanceWebConnectorsNamespace ObjectToRemote = new TMaintenanceWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMaintenanceWebConnectorsSubNamespace = new TMaintenanceWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FMaintenanceWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.SystemDefaults
{
    /// <summary>
    /// REMOTEABLE CLASS. MaintenanceSystemDefaults Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMaintenanceSystemDefaultsNamespace : TConfigurableMBRObject, IMaintenanceSystemDefaultsNamespace
    {

        #region ManualCode
        private TSystemDefaults FSystemDefaultsManager;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TMaintenanceSystemDefaultsNamespace()
        {
            #region ManualCode
            FSystemDefaultsManager = new TSystemDefaults();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceSystemDefaultsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public Ict.Petra.Shared.MSysMan.Data.SSystemDefaultsTable GetSystemDefaults()
        {
            #region ManualCode
            return TSystemDefaults.GetSystemDefaults();
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Boolean SaveSystemDefaults(Ict.Petra.Shared.MSysMan.Data.SSystemDefaultsTable ASystemDefaultsDataTable)
        {
            #region ManualCode
            return FSystemDefaultsManager.SaveSystemDefaults(ASystemDefaultsDataTable);
            #endregion ManualCode
        }

        /// generated method from interface
        public void ReloadSystemDefaultsTable()
        {
            #region ManualCode
            FSystemDefaultsManager.ReloadSystemDefaultsTable();
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. MaintenanceUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMaintenanceUIConnectorsNamespace : TConfigurableMBRObject, IMaintenanceUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TMaintenanceUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.UserDefaults
{
    /// <summary>
    /// REMOTEABLE CLASS. MaintenanceUserDefaults Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMaintenanceUserDefaultsNamespace : TConfigurableMBRObject, IMaintenanceUserDefaultsNamespace
    {

        /// <summary>Constructor</summary>
        public TMaintenanceUserDefaultsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceUserDefaultsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public void GetUserDefaults(System.String AUserName,
                                    out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
        {
            #region ManualCode
            TUserDefaults.GetUserDefaults(AUserName, out AUserDefaultsDataTable);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Boolean SaveUserDefaults(System.String AUserName,
                                               ref Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable,
                                               out Ict.Common.Verification.TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return TUserDefaults.SaveUserDefaultsFromClientSide(AUserName, ref AUserDefaultsDataTable, out AVerificationResult);
            #endregion ManualCode
        }

        /// generated method from interface
        public void ReloadUserDefaults(System.String AUserName,
                                       out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
        {
            #region ManualCode
            TUserDefaults.ReloadUserDefaults(AUserName, true, out AUserDefaultsDataTable);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. MaintenanceWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMaintenanceWebConnectorsNamespace : TConfigurableMBRObject, IMaintenanceWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TMaintenanceWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.Boolean SetLanguageAndCulture(System.String ALanguageCode,
                                                    System.String ACultureCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector), "SetLanguageAndCulture", ";STRING;STRING;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector.SetLanguageAndCulture(ALanguageCode, ACultureCode);
        }

        /// generated method from connector
        public System.Boolean LoadLanguageAndCultureFromUserDefaults()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector), "LoadLanguageAndCultureFromUserDefaults", ";");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector.LoadLanguageAndCultureFromUserDefaults();
        }

        /// generated method from connector
        public System.Boolean GetLanguageAndCulture(ref System.String ALanguageCode,
                                                    ref System.String ACultureCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector), "GetLanguageAndCulture", ";STRING;STRING;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintainLanguageSettingsWebConnector.GetLanguageAndCulture(ref ALanguageCode, ref ACultureCode);
        }

        /// generated method from connector
        public System.Boolean SetUserPassword(System.String AUsername,
                                              System.String APassword)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "SetUserPassword", ";STRING;STRING;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.SetUserPassword(AUsername, APassword);
        }

        /// generated method from connector
        public System.Boolean CheckPasswordQuality(System.String APassword,
                                                   out TVerificationResultCollection AVerification)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "CheckPasswordQuality", ";STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.CheckPasswordQuality(APassword, out AVerification);
        }

        /// generated method from connector
        public System.Boolean SetUserPassword(System.String AUsername,
                                              System.String APassword,
                                              System.String AOldPassword,
                                              out TVerificationResultCollection AVerification)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "SetUserPassword", ";STRING;STRING;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.SetUserPassword(AUsername, APassword, AOldPassword, out AVerification);
        }

        /// generated method from connector
        public System.Boolean CreateUser(System.String AUsername,
                                         System.String APassword,
                                         System.String AModulePermissions)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "CreateUser", ";STRING;STRING;STRING;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.CreateUser(AUsername, APassword, AModulePermissions);
        }

        /// generated method from connector
        public System.Boolean GetAuthenticationFunctionality(out System.Boolean ACanCreateUser,
                                                             out System.Boolean ACanChangePassword,
                                                             out System.Boolean ACanChangePermissions)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "GetAuthenticationFunctionality", ";BOOL;BOOL;BOOL;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.GetAuthenticationFunctionality(out ACanCreateUser, out ACanChangePassword, out ACanChangePermissions);
        }

        /// generated method from connector
        public MaintainUsersTDS LoadUsersAndModulePermissions()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "LoadUsersAndModulePermissions", ";");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.LoadUsersAndModulePermissions();
        }

        /// generated method from connector
        public TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS,
                                              out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector), "SaveSUser", ";MAINTAINUSERSTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.SaveSUser(ref ASubmitDS, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance
{
    /// <summary>
    /// REMOTEABLE CLASS. TableMaintenance Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceNamespace : TConfigurableMBRObject, ITableMaintenanceNamespace
    {
        private TTableMaintenanceUIConnectorsNamespaceRemote FTableMaintenanceUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TTableMaintenanceNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TTableMaintenanceUIConnectorsNamespaceRemote: ITableMaintenanceUIConnectorsNamespace
        {
            private ITableMaintenanceUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TTableMaintenanceUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ITableMaintenanceUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ITableMaintenanceUIConnectorsNamespace));
            }

            /// generated method from interface
            public ISysManUIConnectorsTableMaintenance SysManTableMaintenance()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SysManTableMaintenance();
            }
            /// generated method from interface
            public ISysManUIConnectorsTableMaintenance SysManTableMaintenance(ref DataTable ADataSet,
                                                                              System.String ATableName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SysManTableMaintenance(ref ADataSet,ATableName);
            }
        }

        /// <summary>The 'TableMaintenanceUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'TableMaintenance.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'TableMaintenance.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FTableMaintenanceUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TTableMaintenanceUIConnectorsNamespace");
                    TTableMaintenanceUIConnectorsNamespace ObjectToRemote = new TTableMaintenanceUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FTableMaintenanceUIConnectorsSubNamespace = new TTableMaintenanceUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FTableMaintenanceUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance.UIConnectors
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

        /// generated method from interface
        public ISysManUIConnectorsTableMaintenance SysManTableMaintenance()
        {
            return new TSysManTableMaintenanceUIConnector();
        }

        /// generated method from interface
        public ISysManUIConnectorsTableMaintenance SysManTableMaintenance(ref DataTable ADataSet,
                                                                          System.String ATableName)
        {
            TSysManTableMaintenanceUIConnector ReturnValue = new TSysManTableMaintenanceUIConnector();

            ADataSet = ReturnValue.GetData(ATableName);
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.ImportExport
{
    /// <summary>
    /// REMOTEABLE CLASS. ImportExport Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TImportExportNamespace : TConfigurableMBRObject, IImportExportNamespace
    {
        private TImportExportWebConnectorsNamespaceRemote FImportExportWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TImportExportNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TImportExportWebConnectorsNamespaceRemote: IImportExportWebConnectorsNamespace
        {
            private IImportExportWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TImportExportWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IImportExportWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IImportExportWebConnectorsNamespace));
            }

            /// generated method from interface
            public System.String ExportAllTables()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExportAllTables();
            }
            /// generated method from interface
            public System.Boolean ResetDatabase(System.String AZippedNewDatabaseData)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ResetDatabase(AZippedNewDatabaseData);
            }
            /// generated method from interface
            public System.Boolean SaveTDS(SampleDataConstructorTDS dataTDS,
                                          out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveTDS(dataTDS,out AVerificationResult);
            }
        }

        /// <summary>The 'ImportExportWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IImportExportWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'ImportExport.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'ImportExport.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FImportExportWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TImportExportWebConnectorsNamespace");
                    TImportExportWebConnectorsNamespace ObjectToRemote = new TImportExportWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FImportExportWebConnectorsSubNamespace = new TImportExportWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FImportExportWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.ImportExport.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. ImportExportWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TImportExportWebConnectorsNamespace : TConfigurableMBRObject, IImportExportWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TImportExportWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.String ExportAllTables()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector), "ExportAllTables", ";");
            return Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector.ExportAllTables();
        }

        /// generated method from connector
        public System.Boolean ResetDatabase(System.String AZippedNewDatabaseData)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector), "ResetDatabase", ";STRING;");
            return Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector.ResetDatabase(AZippedNewDatabaseData);
        }

        /// generated method from connector
        public System.Boolean SaveTDS(SampleDataConstructorTDS dataTDS,
                                      out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector), "SaveTDS", ";SAMPLEDATACONSTRUCTORTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector.SaveTDS(dataTDS, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.PrintManagement
{
    /// <summary>
    /// REMOTEABLE CLASS. PrintManagement Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPrintManagementNamespace : TConfigurableMBRObject, IPrintManagementNamespace
    {
        private TPrintManagementUIConnectorsNamespaceRemote FPrintManagementUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPrintManagementNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPrintManagementNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPrintManagementUIConnectorsNamespaceRemote: IPrintManagementUIConnectorsNamespace
        {
            private IPrintManagementUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPrintManagementUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPrintManagementUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPrintManagementUIConnectorsNamespace));
            }

        }

        /// <summary>The 'PrintManagementUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPrintManagementUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PrintManagement.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PrintManagement.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPrintManagementUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPrintManagementUIConnectorsNamespace");
                    TPrintManagementUIConnectorsNamespace ObjectToRemote = new TPrintManagementUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPrintManagementUIConnectorsSubNamespace = new TPrintManagementUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPrintManagementUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.PrintManagement.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PrintManagementUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPrintManagementUIConnectorsNamespace : TConfigurableMBRObject, IPrintManagementUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TPrintManagementUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPrintManagementUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Security
{
    /// <summary>
    /// REMOTEABLE CLASS. Security Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSecurityNamespace : TConfigurableMBRObject, ISecurityNamespace
    {
        private TSecurityUIConnectorsNamespaceRemote FSecurityUIConnectorsSubNamespace;
        private TSecurityUserManagerNamespaceRemote FSecurityUserManagerSubNamespace;

        /// <summary>Constructor</summary>
        public TSecurityNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSecurityNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSecurityUIConnectorsNamespaceRemote: ISecurityUIConnectorsNamespace
        {
            private ISecurityUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSecurityUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISecurityUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISecurityUIConnectorsNamespace));
            }

        }

        /// <summary>The 'SecurityUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ISecurityUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Security.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Security.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FSecurityUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSecurityUIConnectorsNamespace");
                    TSecurityUIConnectorsNamespace ObjectToRemote = new TSecurityUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSecurityUIConnectorsSubNamespace = new TSecurityUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FSecurityUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSecurityUserManagerNamespaceRemote: ISecurityUserManagerNamespace
        {
            private ISecurityUserManagerNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSecurityUserManagerNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISecurityUserManagerNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISecurityUserManagerNamespace));
            }

            /// generated method from interface
            public Ict.Petra.Shared.Security.TPetraPrincipal ReloadCachedUserInfo()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ReloadCachedUserInfo();
            }
            /// generated method from interface
            public void SignalReloadCachedUserInfo(System.String AUserID)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.SignalReloadCachedUserInfo(AUserID);
            }
        }

        /// <summary>The 'SecurityUserManager' subnamespace contains further subnamespaces.</summary>
        public ISecurityUserManagerNamespace UserManager
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Security.UserManager' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Security.UserManager' sub-namespace
                //

                // accessing TUserManagerNamespace the first time? > instantiate the object
                if (FSecurityUserManagerSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSecurityUserManagerNamespace");
                    TSecurityUserManagerNamespace ObjectToRemote = new TSecurityUserManagerNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSecurityUserManagerSubNamespace = new TSecurityUserManagerNamespaceRemote(ObjectURI);
                }

                return FSecurityUserManagerSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Security.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. SecurityUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSecurityUIConnectorsNamespace : TConfigurableMBRObject, ISecurityUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TSecurityUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSecurityUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Security.UserManager
{
    /// <summary>
    /// REMOTEABLE CLASS. SecurityUserManager Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSecurityUserManagerNamespace : TConfigurableMBRObject, ISecurityUserManagerNamespace
    {

        /// <summary>Constructor</summary>
        public TSecurityUserManagerNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSecurityUserManagerNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public Ict.Petra.Shared.Security.TPetraPrincipal ReloadCachedUserInfo()
        {
            #region ManualCode
            return Ict.Petra.Server.MSysMan.Security.TUserManager.ReloadCachedUserInfo();
            #endregion ManualCode
        }

        /// generated method from interface
        public void SignalReloadCachedUserInfo(System.String AUserID)
        {
            #region ManualCode
            Ict.Petra.Server.MSysMan.Security.TUserManager.SignalReloadCachedUserInfo(AUserID);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. Cacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TCacheableNamespace : TConfigurableMBRObject, ICacheableNamespace
    {

		#region ManualCode
        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MSysMan.Cacheable.TCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TCacheableNamespace()
        {
			#region ManualCode
            FCachePopulator = new Ict.Petra.Server.MSysMan.Cacheable.TCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TCacheableNamespace object exists until this AppDomain is unloaded!
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
        private DataTable GetCacheableTableInternal(TCacheableSysManTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue;

            switch (ACacheableTable)
            {
                case TCacheableSysManTablesEnum.UserList:
                case TCacheableSysManTablesEnum.LanguageSpecificList:
                    ReturnValue = FCachePopulator.GetCacheableTable(
            			ACacheableTable, AHashCode, ARefreshFromDB, out AType);
            		
                    // Ict.Petra.Shared.MPartner.Cacheable.AddresseeTypeList:
                    // begin
                    // Result := FCachePopulator.ReasonSubscriptionGivenList(Enum(ACacheableTable).ToString("G"));
                    // end;
                    // Ict.Petra.Shared.MPartner.Cacheable.AcquisitionCodeList:
                    // begin
                    // Result := FCachePopulator.ReasonSubscriptionCancelledList(Enum(ACacheableTable).ToString("G"));
                    // end;
                    break;

                default:
                    throw new ECachedDataTableNotImplementedException(
                    "Requested Cacheable DataTable '" + ACacheableTable.ToString() + "' is not (yet) implemented in the PetraServer");

                    //break;
            }

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableSysManTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableSysManTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableSysManTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                                    
        }
    }
}

