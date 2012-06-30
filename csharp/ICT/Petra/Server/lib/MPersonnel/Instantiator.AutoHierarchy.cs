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
        private TWebConnectorsNamespaceRemote FWebConnectorsSubNamespace;
        private TPersonNamespaceRemote FPersonSubNamespace;
        private TTableMaintenanceNamespaceRemote FTableMaintenanceSubNamespace;
        private TUnitsNamespaceRemote FUnitsSubNamespace;

        /// <summary>Constructor</summary>
        public TMPersonnel()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMPersonnel object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TWebConnectorsNamespaceRemote: IWebConnectorsNamespace
        {
            private IWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IWebConnectorsNamespace));
            }

            /// generated method from interface
            public TSubmitChangesResult SavePersonnelTDS(ref PersonnelTDS AInspectDS,
                                                         out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SavePersonnelTDS(ref AInspectDS,out AVerificationResult);
            }
            /// generated method from interface
            public PersonnelTDS LoadPersonellStaffData(Int64 APartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.LoadPersonellStaffData(APartnerKey);
            }
            /// generated method from interface
            public System.Boolean HasCurrentCommitmentRecord(Int64 APartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.HasCurrentCommitmentRecord(APartnerKey);
            }
            /// generated method from interface
            public System.Int32 GetOrCreateUmJobKey(Int64 AUnitKey,
                                                    System.String APositionName,
                                                    System.String APositionScope)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetOrCreateUmJobKey(AUnitKey,APositionName,APositionScope);
            }
        }

        /// <summary>The 'WebConnectors' subnamespace contains further subnamespaces.</summary>
        public IWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPersonnel.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPersonnel.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TWebConnectorsNamespace");
                    TWebConnectorsNamespace ObjectToRemote = new TWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FWebConnectorsSubNamespace = new TWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FWebConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonNamespaceRemote: IPersonNamespace
        {
            private IPersonNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonNamespace));
            }

            /// property forwarder
            public IPersonDataElementsNamespace DataElements
            {
                get { return RemoteObject.DataElements; }
            }
            /// property forwarder
            public IPersonShepherdsNamespace Shepherds
            {
                get { return RemoteObject.Shepherds; }
            }
        }

        /// <summary>The 'Person' subnamespace contains further subnamespaces.</summary>
        public IPersonNamespace Person
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPersonnel.Person' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPersonnel.Person' sub-namespace
                //

                // accessing TPersonNamespace the first time? > instantiate the object
                if (FPersonSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonNamespace");
                    TPersonNamespace ObjectToRemote = new TPersonNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonSubNamespace = new TPersonNamespaceRemote(ObjectURI);
                }

                return FPersonSubNamespace;
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
                get { return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'TableMaintenance' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceNamespace TableMaintenance
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPersonnel.TableMaintenance' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPersonnel.TableMaintenance' sub-namespace
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
        public class TUnitsNamespaceRemote: IUnitsNamespace
        {
            private IUnitsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TUnitsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IUnitsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IUnitsNamespace));
            }

            /// property forwarder
            public IUnitsDataElementsNamespace DataElements
            {
                get { return RemoteObject.DataElements; }
            }
        }

        /// <summary>The 'Units' subnamespace contains further subnamespaces.</summary>
        public IUnitsNamespace Units
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPersonnel.Units' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPersonnel.Units' sub-namespace
                //

                // accessing TUnitsNamespace the first time? > instantiate the object
                if (FUnitsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TUnitsNamespace");
                    TUnitsNamespace ObjectToRemote = new TUnitsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FUnitsSubNamespace = new TUnitsNamespaceRemote(ObjectURI);
                }

                return FUnitsSubNamespace;
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
        private TPersonDataElementsNamespaceRemote FPersonDataElementsSubNamespace;
        private TPersonShepherdsNamespaceRemote FPersonShepherdsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsNamespaceRemote: IPersonDataElementsNamespace
        {
            private IPersonDataElementsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsNamespace));
            }

            /// property forwarder
            public IPersonDataElementsApplicationsNamespace Applications
            {
                get { return RemoteObject.Applications; }
            }
            /// property forwarder
            public IPersonDataElementsCacheableNamespace Cacheable
            {
                get { return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public IPersonDataElementsUIConnectorsNamespace UIConnectors
            {
                get { return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IPersonDataElementsWebConnectorsNamespace WebConnectors
            {
                get { return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'PersonDataElements' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsNamespace DataElements
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Person.DataElements' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Person.DataElements' sub-namespace
                //

                // accessing TDataElementsNamespace the first time? > instantiate the object
                if (FPersonDataElementsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsNamespace");
                    TPersonDataElementsNamespace ObjectToRemote = new TPersonDataElementsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsSubNamespace = new TPersonDataElementsNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonShepherdsNamespaceRemote: IPersonShepherdsNamespace
        {
            private IPersonShepherdsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonShepherdsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonShepherdsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonShepherdsNamespace));
            }

            /// property forwarder
            public IPersonShepherdsUIConnectorsNamespace UIConnectors
            {
                get { return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'PersonShepherds' subnamespace contains further subnamespaces.</summary>
        public IPersonShepherdsNamespace Shepherds
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Person.Shepherds' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Person.Shepherds' sub-namespace
                //

                // accessing TShepherdsNamespace the first time? > instantiate the object
                if (FPersonShepherdsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonShepherdsNamespace");
                    TPersonShepherdsNamespace ObjectToRemote = new TPersonShepherdsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonShepherdsSubNamespace = new TPersonShepherdsNamespaceRemote(ObjectURI);
                }

                return FPersonShepherdsSubNamespace;
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
        private TPersonDataElementsApplicationsNamespaceRemote FPersonDataElementsApplicationsSubNamespace;
        private TPersonDataElementsCacheableNamespaceRemote FPersonDataElementsCacheableSubNamespace;
        private TPersonDataElementsUIConnectorsNamespaceRemote FPersonDataElementsUIConnectorsSubNamespace;
        private TPersonDataElementsWebConnectorsNamespaceRemote FPersonDataElementsWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonDataElementsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsApplicationsNamespaceRemote: IPersonDataElementsApplicationsNamespace
        {
            private IPersonDataElementsApplicationsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsApplicationsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsApplicationsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsApplicationsNamespace));
            }

            /// property forwarder
            public IPersonDataElementsApplicationsCacheableNamespace Cacheable
            {
                get { return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public IPersonDataElementsApplicationsUIConnectorsNamespace UIConnectors
            {
                get { return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'PersonDataElementsApplications' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsNamespace Applications
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElements.Applications' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElements.Applications' sub-namespace
                //

                // accessing TApplicationsNamespace the first time? > instantiate the object
                if (FPersonDataElementsApplicationsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsApplicationsNamespace");
                    TPersonDataElementsApplicationsNamespace ObjectToRemote = new TPersonDataElementsApplicationsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsApplicationsSubNamespace = new TPersonDataElementsApplicationsNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsApplicationsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsCacheableNamespaceRemote: IPersonDataElementsCacheableNamespace
        {
            private IPersonDataElementsCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
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
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePersonTablesEnum ACacheableTable,
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

        /// <summary>The 'PersonDataElementsCacheable' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElements.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElements.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FPersonDataElementsCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsCacheableNamespace");
                    TPersonDataElementsCacheableNamespace ObjectToRemote = new TPersonDataElementsCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsCacheableSubNamespace = new TPersonDataElementsCacheableNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsUIConnectorsNamespaceRemote: IPersonDataElementsUIConnectorsNamespace
        {
            private IPersonDataElementsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsUIConnectorsNamespace));
            }

            /// generated method from interface
            public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                               Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                               out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.OfficeSpecificDataLabels(APartnerKey,AOfficeSpecificDataLabelUse,out AOfficeSpecificDataLabelsDataSet);
            }
        }

        /// <summary>The 'PersonDataElementsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElements.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElements.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPersonDataElementsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsUIConnectorsNamespace");
                    TPersonDataElementsUIConnectorsNamespace ObjectToRemote = new TPersonDataElementsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsUIConnectorsSubNamespace = new TPersonDataElementsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsWebConnectorsNamespaceRemote: IPersonDataElementsWebConnectorsNamespace
        {
            private IPersonDataElementsWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsWebConnectorsNamespace));
            }

            /// generated method from interface
            public IndividualDataTDS GetData(Int64 APartnerKey,
                                             TIndividualDataItemEnum AIndivDataItem)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetData(APartnerKey,AIndivDataItem);
            }
            /// generated method from interface
            public System.Boolean GetSummaryData(Int64 APartnerKey,
                                                 ref IndividualDataTDS AIndividualDataDS)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetSummaryData(APartnerKey,ref AIndividualDataDS);
            }
        }

        /// <summary>The 'PersonDataElementsWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElements.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElements.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FPersonDataElementsWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsWebConnectorsNamespace");
                    TPersonDataElementsWebConnectorsNamespace ObjectToRemote = new TPersonDataElementsWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsWebConnectorsSubNamespace = new TPersonDataElementsWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsWebConnectorsSubNamespace;
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
        private TPersonDataElementsApplicationsCacheableNamespaceRemote FPersonDataElementsApplicationsCacheableSubNamespace;
        private TPersonDataElementsApplicationsUIConnectorsNamespaceRemote FPersonDataElementsApplicationsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsApplicationsCacheableNamespaceRemote: IPersonDataElementsApplicationsCacheableNamespace
        {
            private IPersonDataElementsApplicationsCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsApplicationsCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsApplicationsCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsApplicationsCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheablePersonTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetCacheableTable(ACacheableTable);
            }
        }

        /// <summary>The 'PersonDataElementsApplicationsCacheable' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElementsApplications.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElementsApplications.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FPersonDataElementsApplicationsCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsApplicationsCacheableNamespace");
                    TPersonDataElementsApplicationsCacheableNamespace ObjectToRemote = new TPersonDataElementsApplicationsCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsApplicationsCacheableSubNamespace = new TPersonDataElementsApplicationsCacheableNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsApplicationsCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonDataElementsApplicationsUIConnectorsNamespaceRemote: IPersonDataElementsApplicationsUIConnectorsNamespace
        {
            private IPersonDataElementsApplicationsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonDataElementsApplicationsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonDataElementsApplicationsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonDataElementsApplicationsUIConnectorsNamespace));
            }

            /// generated method from interface
            public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                               System.Int32 AApplicationKey,
                                                                                                                                               System.Int64 ARegistrationOffice,
                                                                                                                                               Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                               out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.OfficeSpecificDataLabels(APartnerKey,AApplicationKey,ARegistrationOffice,AOfficeSpecificDataLabelUse,out AOfficeSpecificDataLabelsDataSet);
            }
        }

        /// <summary>The 'PersonDataElementsApplicationsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonDataElementsApplicationsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonDataElementsApplications.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonDataElementsApplications.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPersonDataElementsApplicationsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonDataElementsApplicationsUIConnectorsNamespace");
                    TPersonDataElementsApplicationsUIConnectorsNamespace ObjectToRemote = new TPersonDataElementsApplicationsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonDataElementsApplicationsUIConnectorsSubNamespace = new TPersonDataElementsApplicationsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPersonDataElementsApplicationsUIConnectorsSubNamespace;
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
        private TPersonShepherdsUIConnectorsNamespaceRemote FPersonShepherdsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonShepherdsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonShepherdsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPersonShepherdsUIConnectorsNamespaceRemote: IPersonShepherdsUIConnectorsNamespace
        {
            private IPersonShepherdsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPersonShepherdsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPersonShepherdsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPersonShepherdsUIConnectorsNamespace));
            }

        }

        /// <summary>The 'PersonShepherdsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPersonShepherdsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PersonShepherds.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PersonShepherds.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPersonShepherdsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPersonShepherdsUIConnectorsNamespace");
                    TPersonShepherdsUIConnectorsNamespace ObjectToRemote = new TPersonShepherdsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPersonShepherdsUIConnectorsSubNamespace = new TPersonShepherdsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPersonShepherdsUIConnectorsSubNamespace;
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
        private TUnitsDataElementsNamespaceRemote FUnitsDataElementsSubNamespace;

        /// <summary>Constructor</summary>
        public TUnitsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TUnitsDataElementsNamespaceRemote: IUnitsDataElementsNamespace
        {
            private IUnitsDataElementsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TUnitsDataElementsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IUnitsDataElementsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IUnitsDataElementsNamespace));
            }

            /// property forwarder
            public IUnitsDataElementsCacheableNamespace Cacheable
            {
                get { return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public IUnitsDataElementsUIConnectorsNamespace UIConnectors
            {
                get { return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'UnitsDataElements' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsNamespace DataElements
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Units.DataElements' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Units.DataElements' sub-namespace
                //

                // accessing TDataElementsNamespace the first time? > instantiate the object
                if (FUnitsDataElementsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TUnitsDataElementsNamespace");
                    TUnitsDataElementsNamespace ObjectToRemote = new TUnitsDataElementsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FUnitsDataElementsSubNamespace = new TUnitsDataElementsNamespaceRemote(ObjectURI);
                }

                return FUnitsDataElementsSubNamespace;
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
        private TUnitsDataElementsCacheableNamespaceRemote FUnitsDataElementsCacheableSubNamespace;
        private TUnitsDataElementsUIConnectorsNamespaceRemote FUnitsDataElementsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TUnitsDataElementsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TUnitsDataElementsCacheableNamespaceRemote: IUnitsDataElementsCacheableNamespace
        {
            private IUnitsDataElementsCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TUnitsDataElementsCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IUnitsDataElementsCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IUnitsDataElementsCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheableUnitTablesEnum ACacheableTable,
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
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableUnitTablesEnum ACacheableTable,
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

        /// <summary>The 'UnitsDataElementsCacheable' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'UnitsDataElements.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'UnitsDataElements.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FUnitsDataElementsCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TUnitsDataElementsCacheableNamespace");
                    TUnitsDataElementsCacheableNamespace ObjectToRemote = new TUnitsDataElementsCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FUnitsDataElementsCacheableSubNamespace = new TUnitsDataElementsCacheableNamespaceRemote(ObjectURI);
                }

                return FUnitsDataElementsCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TUnitsDataElementsUIConnectorsNamespaceRemote: IUnitsDataElementsUIConnectorsNamespace
        {
            private IUnitsDataElementsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TUnitsDataElementsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IUnitsDataElementsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IUnitsDataElementsUIConnectorsNamespace));
            }

        }

        /// <summary>The 'UnitsDataElementsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IUnitsDataElementsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'UnitsDataElements.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'UnitsDataElements.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FUnitsDataElementsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TUnitsDataElementsUIConnectorsNamespace");
                    TUnitsDataElementsUIConnectorsNamespace ObjectToRemote = new TUnitsDataElementsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FUnitsDataElementsUIConnectorsSubNamespace = new TUnitsDataElementsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FUnitsDataElementsUIConnectorsSubNamespace;
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

