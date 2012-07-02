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

using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts;
using Ict.Petra.Shared.Interfaces.MPartner.ImportExport;
using Ict.Petra.Shared.Interfaces.MPartner.Mailing;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Shared.Interfaces.MPartner.PartnerMerge;
using Ict.Petra.Shared.Interfaces.MPartner.Subscriptions;
using Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Extracts.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.ImportExport.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.ImportExport.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Mailing.Cacheable;
using Ict.Petra.Shared.Interfaces.MPartner.Mailing.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Mailing.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.Cacheable;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.DataElements;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.DataElements.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.ServerLookups;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Partner.WebConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.PartnerMerge.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.Subscriptions.Cacheable;
using Ict.Petra.Shared.Interfaces.MPartner.Subscriptions.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance.UIConnectors;
using Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance.WebConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Extracts;
using Ict.Petra.Server.MPartner.Instantiator.ImportExport;
using Ict.Petra.Server.MPartner.Instantiator.Mailing;
using Ict.Petra.Server.MPartner.Instantiator.Partner;
using Ict.Petra.Server.MPartner.Instantiator.PartnerMerge;
using Ict.Petra.Server.MPartner.Instantiator.Subscriptions;
using Ict.Petra.Server.MPartner.Instantiator.TableMaintenance;
using Ict.Petra.Server.MPartner.Instantiator.Extracts.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Extracts.WebConnectors;
using Ict.Petra.Server.MPartner.Instantiator.ImportExport.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.ImportExport.WebConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable;
using Ict.Petra.Server.MPartner.Instantiator.Mailing.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Mailing.WebConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable;
using Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements;
using Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups;
using Ict.Petra.Server.MPartner.Instantiator.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Partner.WebConnectors;
using Ict.Petra.Server.MPartner.Instantiator.PartnerMerge.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable;
using Ict.Petra.Server.MPartner.Instantiator.Subscriptions.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.WebConnectors;
using Ict.Petra.Server.MPartner.Extracts;
//using Ict.Petra.Server.MPartner.ImportExport;
// using Ict.Petra.Server.MPartner.Mailing;
using Ict.Petra.Server.MPartner.Partner;
//using Ict.Petra.Server.MPartner.PartnerMerge;
//using Ict.Petra.Server.MPartner.Subscriptions;
//using Ict.Petra.Server.MPartner.TableMaintenance;
using Ict.Petra.Server.MPartner.Extracts.UIConnectors;
//using Ict.Petra.Server.MPartner.Extracts.WebConnectors;
//using Ict.Petra.Server.MPartner.ImportExport.UIConnectors;
using Ict.Petra.Server.MPartner.ImportExport.WebConnectors;
using Ict.Petra.Server.MPartner.Mailing.Cacheable;
//using Ict.Petra.Server.MPartner.Mailing.UIConnectors;
using Ict.Petra.Server.MPartner.Mailing.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
//using Ict.Petra.Server.MPartner.Partner.DataElements;
//using Ict.Petra.Server.MPartner.Partner.DataElements.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.ServerLookups;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
//using Ict.Petra.Server.MPartner.PartnerMerge.UIConnectors;
using Ict.Petra.Server.MPartner.Subscriptions.Cacheable;
//using Ict.Petra.Server.MPartner.Subscriptions.UIConnectors;
//using Ict.Petra.Server.MPartner.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors;

#region ManualCode
using System.Collections.Specialized;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MCommon.UIConnectors;
#endregion ManualCode
namespace Ict.Petra.Server.MPartner.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMPartnerNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMPartner FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMPartner
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
                Console.WriteLine("TMPartnerNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMPartner();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMPartnerNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMPartner GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MPartner Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMPartner : TConfigurableMBRObject, IMPartnerNamespace
    {
        private TExtractsNamespaceRemote FExtractsSubNamespace;
        private TImportExportNamespaceRemote FImportExportSubNamespace;
        private TMailingNamespaceRemote FMailingSubNamespace;
        private TPartnerNamespaceRemote FPartnerSubNamespace;
        private TPartnerMergeNamespaceRemote FPartnerMergeSubNamespace;
        private TSubscriptionsNamespaceRemote FSubscriptionsSubNamespace;
        private TTableMaintenanceNamespaceRemote FTableMaintenanceSubNamespace;

        /// <summary>Constructor</summary>
        public TMPartner()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMPartner object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TExtractsNamespaceRemote: IExtractsNamespace
        {
            private IExtractsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TExtractsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IExtractsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IExtractsNamespace));
            }

            /// property forwarder
            public IExtractsUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IExtractsWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'Extracts' subnamespace contains further subnamespaces.</summary>
        public IExtractsNamespace Extracts
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.Extracts' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.Extracts' sub-namespace
                //

                // accessing TExtractsNamespace the first time? > instantiate the object
                if (FExtractsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TExtractsNamespace");
                    TExtractsNamespace ObjectToRemote = new TExtractsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FExtractsSubNamespace = new TExtractsNamespaceRemote(ObjectURI);
                }

                return FExtractsSubNamespace;
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
            public IImportExportUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
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
                // reside in the 'MPartner.ImportExport' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.ImportExport' sub-namespace
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
        public class TMailingNamespaceRemote: IMailingNamespace
        {
            private IMailingNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMailingNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMailingNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMailingNamespace));
            }

            /// property forwarder
            public IMailingCacheableNamespace Cacheable
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public IMailingUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IMailingWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'Mailing' subnamespace contains further subnamespaces.</summary>
        public IMailingNamespace Mailing
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.Mailing' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.Mailing' sub-namespace
                //

                // accessing TMailingNamespace the first time? > instantiate the object
                if (FMailingSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMailingNamespace");
                    TMailingNamespace ObjectToRemote = new TMailingNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMailingSubNamespace = new TMailingNamespaceRemote(ObjectURI);
                }

                return FMailingSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerNamespaceRemote: IPartnerNamespace
        {
            private IPartnerNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerNamespace));
            }

            /// property forwarder
            public IPartnerCacheableNamespace Cacheable
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public IPartnerDataElementsNamespace DataElements
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.DataElements; }
            }
            /// property forwarder
            public IPartnerServerLookupsNamespace ServerLookups
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.ServerLookups; }
            }
            /// property forwarder
            public IPartnerUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
            /// property forwarder
            public IPartnerWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'Partner' subnamespace contains further subnamespaces.</summary>
        public IPartnerNamespace Partner
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.Partner' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.Partner' sub-namespace
                //

                // accessing TPartnerNamespace the first time? > instantiate the object
                if (FPartnerSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerNamespace");
                    TPartnerNamespace ObjectToRemote = new TPartnerNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerSubNamespace = new TPartnerNamespaceRemote(ObjectURI);
                }

                return FPartnerSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerMergeNamespaceRemote: IPartnerMergeNamespace
        {
            private IPartnerMergeNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerMergeNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerMergeNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerMergeNamespace));
            }

            /// property forwarder
            public IPartnerMergeUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'PartnerMerge' subnamespace contains further subnamespaces.</summary>
        public IPartnerMergeNamespace PartnerMerge
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.PartnerMerge' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.PartnerMerge' sub-namespace
                //

                // accessing TPartnerMergeNamespace the first time? > instantiate the object
                if (FPartnerMergeSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerMergeNamespace");
                    TPartnerMergeNamespace ObjectToRemote = new TPartnerMergeNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerMergeSubNamespace = new TPartnerMergeNamespaceRemote(ObjectURI);
                }

                return FPartnerMergeSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSubscriptionsNamespaceRemote: ISubscriptionsNamespace
        {
            private ISubscriptionsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSubscriptionsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISubscriptionsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISubscriptionsNamespace));
            }

            /// property forwarder
            public ISubscriptionsCacheableNamespace Cacheable
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.Cacheable; }
            }
            /// property forwarder
            public ISubscriptionsUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'Subscriptions' subnamespace contains further subnamespaces.</summary>
        public ISubscriptionsNamespace Subscriptions
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.Subscriptions' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.Subscriptions' sub-namespace
                //

                // accessing TSubscriptionsNamespace the first time? > instantiate the object
                if (FSubscriptionsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSubscriptionsNamespace");
                    TSubscriptionsNamespace ObjectToRemote = new TSubscriptionsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSubscriptionsSubNamespace = new TSubscriptionsNamespaceRemote(ObjectURI);
                }

                return FSubscriptionsSubNamespace;
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
            /// property forwarder
            public ITableMaintenanceWebConnectorsNamespace WebConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.WebConnectors; }
            }
        }

        /// <summary>The 'TableMaintenance' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceNamespace TableMaintenance
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MPartner.TableMaintenance' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MPartner.TableMaintenance' sub-namespace
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
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Extracts
{
    /// <summary>
    /// REMOTEABLE CLASS. Extracts Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TExtractsNamespace : TConfigurableMBRObject, IExtractsNamespace
    {
        private TExtractsUIConnectorsNamespaceRemote FExtractsUIConnectorsSubNamespace;
        private TExtractsWebConnectorsNamespaceRemote FExtractsWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TExtractsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TExtractsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TExtractsUIConnectorsNamespaceRemote: IExtractsUIConnectorsNamespace
        {
            private IExtractsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TExtractsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IExtractsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IExtractsUIConnectorsNamespace));
            }

            /// generated method from interface
            public IPartnerUIConnectorsExtractsAddSubscriptions ExtractsAddSubscriptions(System.Int32 AExtractID)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExtractsAddSubscriptions(AExtractID);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerNewExtract PartnerNewExtract()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerNewExtract();
            }
        }

        /// <summary>The 'ExtractsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IExtractsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Extracts.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Extracts.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FExtractsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TExtractsUIConnectorsNamespace");
                    TExtractsUIConnectorsNamespace ObjectToRemote = new TExtractsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FExtractsUIConnectorsSubNamespace = new TExtractsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FExtractsUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TExtractsWebConnectorsNamespaceRemote: IExtractsWebConnectorsNamespace
        {
            private IExtractsWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TExtractsWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IExtractsWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IExtractsWebConnectorsNamespace));
            }

        }

        /// <summary>The 'ExtractsWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IExtractsWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Extracts.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Extracts.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FExtractsWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TExtractsWebConnectorsNamespace");
                    TExtractsWebConnectorsNamespace ObjectToRemote = new TExtractsWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FExtractsWebConnectorsSubNamespace = new TExtractsWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FExtractsWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Extracts.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. ExtractsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TExtractsUIConnectorsNamespace : TConfigurableMBRObject, IExtractsUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TExtractsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TExtractsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IPartnerUIConnectorsExtractsAddSubscriptions ExtractsAddSubscriptions(System.Int32 AExtractID)
        {
            return new TExtractsAddSubscriptionsUIConnector(AExtractID);
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerNewExtract PartnerNewExtract()
        {
            return new TPartnerNewExtractUIConnector();
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Extracts.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. ExtractsWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TExtractsWebConnectorsNamespace : TConfigurableMBRObject, IExtractsWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TExtractsWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TExtractsWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport
{
    /// <summary>
    /// REMOTEABLE CLASS. ImportExport Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TImportExportNamespace : TConfigurableMBRObject, IImportExportNamespace
    {
        private TImportExportUIConnectorsNamespaceRemote FImportExportUIConnectorsSubNamespace;
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
        public class TImportExportUIConnectorsNamespaceRemote: IImportExportUIConnectorsNamespace
        {
            private IImportExportUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TImportExportUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IImportExportUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IImportExportUIConnectorsNamespace));
            }

        }

        /// <summary>The 'ImportExportUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IImportExportUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'ImportExport.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'ImportExport.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FImportExportUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TImportExportUIConnectorsNamespace");
                    TImportExportUIConnectorsNamespace ObjectToRemote = new TImportExportUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FImportExportUIConnectorsSubNamespace = new TImportExportUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FImportExportUIConnectorsSubNamespace;
            }

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
            public PartnerImportExportTDS ImportPartnersFromYml(System.String AXmlPartnerData,
                                                                out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ImportPartnersFromYml(AXmlPartnerData,out AVerificationResult);
            }
            /// generated method from interface
            public PartnerImportExportTDS ImportFromCSVFile(System.String AXmlPartnerData,
                                                            out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ImportFromCSVFile(AXmlPartnerData,out AVerificationResult);
            }
            /// generated method from interface
            public PartnerImportExportTDS ImportFromPartnerExtract(System.String[] ATextFileLines,
                                                                   out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ImportFromPartnerExtract(ATextFileLines,out AVerificationResult);
            }
            /// generated method from interface
            public Boolean CommitChanges(PartnerImportExportTDS MainDS,
                                         out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.CommitChanges(MainDS,out AVerificationResult);
            }
            /// generated method from interface
            public System.String ExportPartners()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExportPartners();
            }
            /// generated method from interface
            public System.String GetExtFileHeader()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetExtFileHeader();
            }
            /// generated method from interface
            public System.String GetExtFileFooter()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetExtFileFooter();
            }
            /// generated method from interface
            public System.String ExportPartnerExt(Int64 APartnerKey,
                                                  Int32 ASiteKey,
                                                  Int32 ALocationKey,
                                                  Boolean ANoFamily,
                                                  StringCollection ASpecificBuildingInfo)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExportPartnerExt(APartnerKey,ASiteKey,ALocationKey,ANoFamily,ASpecificBuildingInfo);
            }
            /// generated method from interface
            public String ExportAllPartnersExt()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExportAllPartnersExt();
            }
            /// generated method from interface
            public Boolean ImportDataExt(System.String[] ALinesToImport,
                                         System.String ALimitToOption,
                                         System.Boolean ADoNotOverwrite,
                                         out TVerificationResultCollection AResultList)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ImportDataExt(ALinesToImport,ALimitToOption,ADoNotOverwrite,out AResultList);
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

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. ImportExportUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TImportExportUIConnectorsNamespace : TConfigurableMBRObject, IImportExportUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TImportExportUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport.WebConnectors
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
        public PartnerImportExportTDS ImportPartnersFromYml(System.String AXmlPartnerData,
                                                            out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ImportPartnersFromYml", ";STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ImportPartnersFromYml(AXmlPartnerData, out AVerificationResult);
        }

        /// generated method from connector
        public PartnerImportExportTDS ImportFromCSVFile(System.String AXmlPartnerData,
                                                        out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ImportFromCSVFile", ";STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ImportFromCSVFile(AXmlPartnerData, out AVerificationResult);
        }

        /// generated method from connector
        public PartnerImportExportTDS ImportFromPartnerExtract(System.String[] ATextFileLines,
                                                               out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ImportFromPartnerExtract", ";STRING.ARRAY;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ImportFromPartnerExtract(ATextFileLines, out AVerificationResult);
        }

        /// generated method from connector
        public Boolean CommitChanges(PartnerImportExportTDS MainDS,
                                     out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "CommitChanges", ";PARTNERIMPORTEXPORTTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.CommitChanges(MainDS, out AVerificationResult);
        }

        /// generated method from connector
        public System.String ExportPartners()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ExportPartners", ";");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ExportPartners();
        }

        /// generated method from connector
        public System.String GetExtFileHeader()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "GetExtFileHeader", ";");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.GetExtFileHeader();
        }

        /// generated method from connector
        public System.String GetExtFileFooter()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "GetExtFileFooter", ";");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.GetExtFileFooter();
        }

        /// generated method from connector
        public System.String ExportPartnerExt(Int64 APartnerKey,
                                              Int32 ASiteKey,
                                              Int32 ALocationKey,
                                              Boolean ANoFamily,
                                              StringCollection ASpecificBuildingInfo)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ExportPartnerExt", ";LONG;INT;INT;BOOL;STRINGCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ExportPartnerExt(APartnerKey, ASiteKey, ALocationKey, ANoFamily, ASpecificBuildingInfo);
        }

        /// generated method from connector
        public String ExportAllPartnersExt()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ExportAllPartnersExt", ";");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ExportAllPartnersExt();
        }

        /// generated method from connector
        public Boolean ImportDataExt(System.String[] ALinesToImport,
                                     System.String ALimitToOption,
                                     System.Boolean ADoNotOverwrite,
                                     out TVerificationResultCollection AResultList)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector), "ImportDataExt", ";STRING.ARRAY;STRING;BOOL;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector.ImportDataExt(ALinesToImport, ALimitToOption, ADoNotOverwrite, out AResultList);
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing
{
    /// <summary>
    /// REMOTEABLE CLASS. Mailing Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMailingNamespace : TConfigurableMBRObject, IMailingNamespace
    {
        private TMailingCacheableNamespaceRemote FMailingCacheableSubNamespace;
        private TMailingUIConnectorsNamespaceRemote FMailingUIConnectorsSubNamespace;
        private TMailingWebConnectorsNamespaceRemote FMailingWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMailingNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMailingCacheableNamespaceRemote: IMailingCacheableNamespace
        {
            private IMailingCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMailingCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMailingCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMailingCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
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
            public void RefreshCacheableTable(TCacheableMailingTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
                                              out System.Data.DataTable ADataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable,out ADataTable);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
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

        /// <summary>The 'MailingCacheable' subnamespace contains further subnamespaces.</summary>
        public IMailingCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Mailing.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Mailing.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FMailingCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMailingCacheableNamespace");
                    TMailingCacheableNamespace ObjectToRemote = new TMailingCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMailingCacheableSubNamespace = new TMailingCacheableNamespaceRemote(ObjectURI);
                }

                return FMailingCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMailingUIConnectorsNamespaceRemote: IMailingUIConnectorsNamespace
        {
            private IMailingUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMailingUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMailingUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMailingUIConnectorsNamespace));
            }

        }

        /// <summary>The 'MailingUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IMailingUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Mailing.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Mailing.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FMailingUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMailingUIConnectorsNamespace");
                    TMailingUIConnectorsNamespace ObjectToRemote = new TMailingUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMailingUIConnectorsSubNamespace = new TMailingUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FMailingUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TMailingWebConnectorsNamespaceRemote: IMailingWebConnectorsNamespace
        {
            private IMailingWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TMailingWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IMailingWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IMailingWebConnectorsNamespace));
            }

            /// generated method from interface
            public System.Boolean GetBestAddress(Int64 APartnerKey,
                                                 out PLocationTable AAddress,
                                                 out PPartnerLocationTable APartnerLocation,
                                                 out System.String ACountryNameLocal,
                                                 out System.String AEmailAddress)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetBestAddress(APartnerKey,out AAddress,out APartnerLocation,out ACountryNameLocal,out AEmailAddress);
            }
            /// generated method from interface
            public BestAddressTDSLocationTable AddPostalAddress(ref DataTable APartnerTable,
                                                                DataColumn APartnerKeyColumn,
                                                                System.Boolean AIgnoreForeignAddresses)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.AddPostalAddress(ref APartnerTable,APartnerKeyColumn,AIgnoreForeignAddresses);
            }
            /// generated method from interface
            public System.Boolean CreateExtractFromBestAddressTable(String AExtractName,
                                                                    String AExtractDescription,
                                                                    out Int32 ANewExtractId,
                                                                    out Boolean AExtractAlreadyExists,
                                                                    out TVerificationResultCollection AVerificationResults,
                                                                    BestAddressTDSLocationTable ABestAddressTable,
                                                                    System.Boolean AIncludeNonValidAddresses)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.CreateExtractFromBestAddressTable(AExtractName,AExtractDescription,out ANewExtractId,out AExtractAlreadyExists,out AVerificationResults,ABestAddressTable,AIncludeNonValidAddresses);
            }
        }

        /// <summary>The 'MailingWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IMailingWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Mailing.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Mailing.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FMailingWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TMailingWebConnectorsNamespace");
                    TMailingWebConnectorsNamespace ObjectToRemote = new TMailingWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FMailingWebConnectorsSubNamespace = new TMailingWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FMailingWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. MailingCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMailingCacheableNamespace : TConfigurableMBRObject, IMailingCacheableNamespace
    {

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TMailingCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingCacheableNamespace object exists until this AppDomain is unloaded!
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
        private DataTable GetCacheableTableInternal(TCacheableMailingTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableMailingTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableMailingTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableMailingTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableMailingTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                                    
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. MailingUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMailingUIConnectorsNamespace : TConfigurableMBRObject, IMailingUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TMailingUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. MailingWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMailingWebConnectorsNamespace : TConfigurableMBRObject, IMailingWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TMailingWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.Boolean GetBestAddress(Int64 APartnerKey,
                                             out PLocationTable AAddress,
                                             out PPartnerLocationTable APartnerLocation,
                                             out System.String ACountryNameLocal,
                                             out System.String AEmailAddress)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector), "GetBestAddress", ";LONG;PLOCATIONTABLE;PPARTNERLOCATIONTABLE;STRING;STRING;");
            return Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector.GetBestAddress(APartnerKey, out AAddress, out APartnerLocation, out ACountryNameLocal, out AEmailAddress);
        }

        /// generated method from connector
        public BestAddressTDSLocationTable AddPostalAddress(ref DataTable APartnerTable,
                                                            DataColumn APartnerKeyColumn,
                                                            System.Boolean AIgnoreForeignAddresses)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector), "AddPostalAddress", ";DATATABLE;DATACOLUMN;BOOL;");
            return Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector.AddPostalAddress(ref APartnerTable, APartnerKeyColumn, AIgnoreForeignAddresses);
        }

        /// generated method from connector
        public System.Boolean CreateExtractFromBestAddressTable(String AExtractName,
                                                                String AExtractDescription,
                                                                out Int32 ANewExtractId,
                                                                out Boolean AExtractAlreadyExists,
                                                                out TVerificationResultCollection AVerificationResults,
                                                                BestAddressTDSLocationTable ABestAddressTable,
                                                                System.Boolean AIncludeNonValidAddresses)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector), "CreateExtractFromBestAddressTable", ";STRING;STRING;INT;BOOL;TVERIFICATIONRESULTCOLLECTION;BESTADDRESSTDSLOCATIONTABLE;BOOL;");
            return Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector.CreateExtractFromBestAddressTable(AExtractName, AExtractDescription, out ANewExtractId, out AExtractAlreadyExists, out AVerificationResults, ABestAddressTable, AIncludeNonValidAddresses);
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner
{
    /// <summary>
    /// REMOTEABLE CLASS. Partner Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerNamespace : TConfigurableMBRObject, IPartnerNamespace
    {
        private TPartnerCacheableNamespaceRemote FPartnerCacheableSubNamespace;
        private TPartnerDataElementsNamespaceRemote FPartnerDataElementsSubNamespace;
        private TPartnerServerLookupsNamespaceRemote FPartnerServerLookupsSubNamespace;
        private TPartnerUIConnectorsNamespaceRemote FPartnerUIConnectorsSubNamespace;
        private TPartnerWebConnectorsNamespaceRemote FPartnerWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerCacheableNamespaceRemote: IPartnerCacheableNamespace
        {
            private IPartnerCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
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
            public void RefreshCacheableTable(TCacheablePartnerTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
                                              out System.Data.DataTable ADataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable,out ADataTable);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
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

        /// <summary>The 'PartnerCacheable' subnamespace contains further subnamespaces.</summary>
        public IPartnerCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Partner.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Partner.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FPartnerCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerCacheableNamespace");
                    TPartnerCacheableNamespace ObjectToRemote = new TPartnerCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerCacheableSubNamespace = new TPartnerCacheableNamespaceRemote(ObjectURI);
                }

                return FPartnerCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerDataElementsNamespaceRemote: IPartnerDataElementsNamespace
        {
            private IPartnerDataElementsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerDataElementsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerDataElementsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerDataElementsNamespace));
            }

            /// property forwarder
            public IPartnerDataElementsUIConnectorsNamespace UIConnectors
            {
                get { if (RemoteObject == null) { InitRemoteObject(); } return RemoteObject.UIConnectors; }
            }
        }

        /// <summary>The 'PartnerDataElements' subnamespace contains further subnamespaces.</summary>
        public IPartnerDataElementsNamespace DataElements
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Partner.DataElements' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Partner.DataElements' sub-namespace
                //

                // accessing TDataElementsNamespace the first time? > instantiate the object
                if (FPartnerDataElementsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerDataElementsNamespace");
                    TPartnerDataElementsNamespace ObjectToRemote = new TPartnerDataElementsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerDataElementsSubNamespace = new TPartnerDataElementsNamespaceRemote(ObjectURI);
                }

                return FPartnerDataElementsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerServerLookupsNamespaceRemote: IPartnerServerLookupsNamespace
        {
            private IPartnerServerLookupsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerServerLookupsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerServerLookupsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerServerLookupsNamespace));
            }

            /// generated method from interface
            public Boolean GetPartnerShortName(Int64 APartnerKey,
                                               out String APartnerShortName,
                                               out TPartnerClass APartnerClass,
                                               Boolean AMergedPartners)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetPartnerShortName(APartnerKey,out APartnerShortName,out APartnerClass,AMergedPartners);
            }
            /// generated method from interface
            public Boolean GetPartnerShortName(Int64 APartnerKey,
                                               out String APartnerShortName,
                                               out TPartnerClass APartnerClass)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetPartnerShortName(APartnerKey,out APartnerShortName,out APartnerClass);
            }
            /// generated method from interface
            public Boolean VerifyPartner(Int64 APartnerKey,
                                         TPartnerClass[] AValidPartnerClasses,
                                         out System.Boolean APartnerExists,
                                         out String APartnerShortName,
                                         out TPartnerClass APartnerClass,
                                         out Boolean AIsMergedPartner)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.VerifyPartner(APartnerKey,AValidPartnerClasses,out APartnerExists,out APartnerShortName,out APartnerClass,out AIsMergedPartner);
            }
            /// generated method from interface
            public Boolean VerifyPartner(Int64 APartnerKey,
                                         out String APartnerShortName,
                                         out TPartnerClass APartnerClass,
                                         out Boolean AIsMergedPartner,
                                         out Boolean AUserCanAccessPartner)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.VerifyPartner(APartnerKey,out APartnerShortName,out APartnerClass,out AIsMergedPartner,out AUserCanAccessPartner);
            }
            /// generated method from interface
            public Boolean VerifyPartner(Int64 APartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.VerifyPartner(APartnerKey);
            }
            /// generated method from interface
            public Boolean VerifyPartnerAtLocation(Int64 APartnerKey,
                                                   TLocationPK ALocationKey,
                                                   out Boolean AAddressNeitherCurrentNorMailing)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.VerifyPartnerAtLocation(APartnerKey,ALocationKey,out AAddressNeitherCurrentNorMailing);
            }
            /// generated method from interface
            public Boolean MergedPartnerDetails(Int64 AMergedPartnerPartnerKey,
                                                out String AMergedPartnerPartnerShortName,
                                                out TPartnerClass AMergedPartnerPartnerClass,
                                                out Int64 AMergedIntoPartnerKey,
                                                out String AMergedIntoPartnerShortName,
                                                out TPartnerClass AMergedIntoPartnerClass,
                                                out String AMergedBy,
                                                out DateTime AMergeDate)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.MergedPartnerDetails(AMergedPartnerPartnerKey,out AMergedPartnerPartnerShortName,out AMergedPartnerPartnerClass,out AMergedIntoPartnerKey,out AMergedIntoPartnerShortName,out AMergedIntoPartnerClass,out AMergedBy,out AMergeDate);
            }
            /// generated method from interface
            public Boolean PartnerInfo(Int64 APartnerKey,
                                       TPartnerInfoScopeEnum APartnerInfoScope,
                                       out PartnerInfoTDS APartnerInfoDS)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerInfo(APartnerKey,APartnerInfoScope,out APartnerInfoDS);
            }
            /// generated method from interface
            public Boolean PartnerInfo(Int64 APartnerKey,
                                       TLocationPK ALocationKey,
                                       TPartnerInfoScopeEnum APartnerInfoScope,
                                       out PartnerInfoTDS APartnerInfoDS)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerInfo(APartnerKey,ALocationKey,APartnerInfoScope,out APartnerInfoDS);
            }
            /// generated method from interface
            public Boolean GetExtractDescription(String AExtractName,
                                                 out String AExtractDescription)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetExtractDescription(AExtractName,out AExtractDescription);
            }
            /// generated method from interface
            public Boolean GetPartnerFoundationStatus(Int64 APartnerKey,
                                                      out Boolean AIsFoundation)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetPartnerFoundationStatus(APartnerKey,out AIsFoundation);
            }
            /// generated method from interface
            public Boolean GetRecentlyUsedPartners(System.Int32 AMaxPartnersCount,
                                                   ArrayList APartnerClasses,
                                                   out Dictionary<System.Int64, System.String>ARecentlyUsedPartners)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetRecentlyUsedPartners(AMaxPartnersCount,APartnerClasses,out ARecentlyUsedPartners);
            }
            /// generated method from interface
            public Int64 GetFamilyKeyForPerson(Int64 APersonKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetFamilyKeyForPerson(APersonKey);
            }
        }

        /// <summary>The 'PartnerServerLookups' subnamespace contains further subnamespaces.</summary>
        public IPartnerServerLookupsNamespace ServerLookups
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Partner.ServerLookups' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Partner.ServerLookups' sub-namespace
                //

                // accessing TServerLookupsNamespace the first time? > instantiate the object
                if (FPartnerServerLookupsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerServerLookupsNamespace");
                    TPartnerServerLookupsNamespace ObjectToRemote = new TPartnerServerLookupsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerServerLookupsSubNamespace = new TPartnerServerLookupsNamespaceRemote(ObjectURI);
                }

                return FPartnerServerLookupsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerUIConnectorsNamespaceRemote: IPartnerUIConnectorsNamespace
        {
            private IPartnerUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerUIConnectorsNamespace));
            }

            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit(APartnerKey);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey,
                                                               ref PartnerEditTDS ADataSet,
                                                               Boolean ADelayedDataLoading,
                                                               TPartnerEditTabPageEnum ATabPage)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit(APartnerKey,ref ADataSet,ADelayedDataLoading,ATabPage);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                               Int64 ASiteKey,
                                                               Int32 ALocationKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit(APartnerKey,ASiteKey,ALocationKey);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                               Int64 ASiteKey,
                                                               Int32 ALocationKey,
                                                               ref PartnerEditTDS ADataSet,
                                                               Boolean ADelayedDataLoading,
                                                               TPartnerEditTabPageEnum ATabPage)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit(APartnerKey,ASiteKey,ALocationKey,ref ADataSet,ADelayedDataLoading,ATabPage);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit();
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerEdit PartnerEdit(ref PartnerEditTDS ADataSet,
                                                               Boolean ADelayedDataLoading,
                                                               TPartnerEditTabPageEnum ATabPage)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerEdit(ref ADataSet,ADelayedDataLoading,ATabPage);
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerFind PartnerFind()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerFind();
            }
            /// generated method from interface
            public IPartnerUIConnectorsPartnerLocationFind PartnerLocationFind(DataTable ACriteriaData)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PartnerLocationFind(ACriteriaData);
            }
        }

        /// <summary>The 'PartnerUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPartnerUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Partner.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Partner.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPartnerUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerUIConnectorsNamespace");
                    TPartnerUIConnectorsNamespace ObjectToRemote = new TPartnerUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerUIConnectorsSubNamespace = new TPartnerUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPartnerUIConnectorsSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerWebConnectorsNamespaceRemote: IPartnerWebConnectorsNamespace
        {
            private IPartnerWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerWebConnectorsNamespace));
            }

            /// generated method from interface
            public System.Boolean AddContact(List<Int64>APartnerKeys,
                                             DateTime AContactDate,
                                             System.String AMethodOfContact,
                                             System.String AComment,
                                             System.String AModuleID,
                                             System.String AMailingCode,
                                             out TVerificationResultCollection AVerificationResults)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.AddContact(APartnerKeys,AContactDate,AMethodOfContact,AComment,AModuleID,AMailingCode,out AVerificationResults);
            }
            /// generated method from interface
            public PPartnerContactTable FindContacts(System.String AContactor,
                                                     System.Nullable<DateTime>AContactDate,
                                                     System.String ACommentContains,
                                                     System.String AMethodOfContact,
                                                     System.String AModuleID,
                                                     System.String AMailingCode)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.FindContacts(AContactor,AContactDate,ACommentContains,AMethodOfContact,AModuleID,AMailingCode);
            }
            /// generated method from interface
            public System.Boolean DeleteContacts(PPartnerContactTable APartnerContacts,
                                                 out TVerificationResultCollection AVerificationResults)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.DeleteContacts(APartnerContacts,out AVerificationResults);
            }
            /// generated method from interface
            public MExtractMasterTable GetAllExtractHeaders()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetAllExtractHeaders();
            }
            /// generated method from interface
            public MExtractMasterTable GetAllExtractHeaders(String AExtractNameFilter,
                                                            Boolean AAllUsers,
                                                            String AUserCreated,
                                                            String AUserModified)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetAllExtractHeaders(AExtractNameFilter,AAllUsers,AUserCreated,AUserModified);
            }
            /// generated method from interface
            public Boolean ExtractExists(String AExtractName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ExtractExists(AExtractName);
            }
            /// generated method from interface
            public Boolean CreateEmptyExtract(ref System.Int32 AExtractId,
                                              String AExtractName,
                                              String AExtractDescription)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.CreateEmptyExtract(ref AExtractId,AExtractName,AExtractDescription);
            }
            /// generated method from interface
            public Boolean PurgeExtracts(System.Int32 ANumberOfDays,
                                         Boolean AAllUsers,
                                         String AUserName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.PurgeExtracts(ANumberOfDays,AAllUsers,AUserName);
            }
            /// generated method from interface
            public ExtractTDSMExtractTable GetExtractRowsWithPartnerData(System.Int32 AExtractId)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetExtractRowsWithPartnerData(AExtractId);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveExtractMaster(ref MExtractMasterTable AExtractMasterTable,
                                                          out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveExtractMaster(ref AExtractMasterTable,out AVerificationResult);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveExtract(System.Int32 AExtractId,
                                                    ref MExtractTable AExtractTable,
                                                    out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SaveExtract(AExtractId,ref AExtractTable,out AVerificationResult);
            }
            /// generated method from interface
            public System.Boolean AddRecentlyUsedPartner(Int64 APartnerKey,
                                                         TPartnerClass APartnerClass,
                                                         Boolean ANewPartner,
                                                         TLastPartnerUse ALastPartnerUse)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.AddRecentlyUsedPartner(APartnerKey,APartnerClass,ANewPartner,ALastPartnerUse);
            }
            /// generated method from interface
            public TLocationPK DetermineBestAddress(Int64 APartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.DetermineBestAddress(APartnerKey);
            }
            /// generated method from interface
            public System.Boolean ChangeFamily(Int64 APersonKey,
                                               Int64 AOldFamilyKey,
                                               Int64 ANewFamilyKey,
                                               out String AProblemMessage,
                                               out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.ChangeFamily(APersonKey,AOldFamilyKey,ANewFamilyKey,out AProblemMessage,out AVerificationResult);
            }
            /// generated method from interface
            public Int64 GetBankBySortCode(System.String ABranchCode)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetBankBySortCode(ABranchCode);
            }
            /// generated method from interface
            public PUnitTable GetConferenceUnits(System.String AConferenceName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetConferenceUnits(AConferenceName);
            }
            /// generated method from interface
            public PUnitTable GetOutreachUnits(System.String AOutreachName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetOutreachUnits(AOutreachName);
            }
            /// generated method from interface
            public PUnitTable GetActiveFieldUnits(System.String AFieldName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetActiveFieldUnits(AFieldName);
            }
            /// generated method from interface
            public PUnitTable GetLedgerUnits(System.String ALedgerName)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetLedgerUnits(ALedgerName);
            }
            /// generated method from interface
            public Int64 NewPartnerKey(Int64 AFieldPartnerKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.NewPartnerKey(AFieldPartnerKey);
            }
            /// generated method from interface
            public PartnerEditTDS GetPartnerDetails(Int64 APartnerKey,
                                                    System.Boolean AWithAddressDetails,
                                                    System.Boolean AWithSubscriptions,
                                                    System.Boolean AWithRelationships)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetPartnerDetails(APartnerKey,AWithAddressDetails,AWithSubscriptions,AWithRelationships);
            }
            /// generated method from interface
            public System.Boolean SavePartner(PartnerEditTDS AMainDS,
                                              out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SavePartner(AMainDS,out AVerificationResult);
            }
            /// generated method from interface
            public PartnerFindTDS FindPartners(System.String AFirstName,
                                               System.String AFamilyNameOrOrganisation,
                                               System.String ACity,
                                               StringCollection APartnerClasses)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.FindPartners(AFirstName,AFamilyNameOrOrganisation,ACity,APartnerClasses);
            }
        }

        /// <summary>The 'PartnerWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IPartnerWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Partner.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Partner.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FPartnerWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerWebConnectorsNamespace");
                    TPartnerWebConnectorsNamespace ObjectToRemote = new TPartnerWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerWebConnectorsSubNamespace = new TPartnerWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FPartnerWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerCacheableNamespace : TConfigurableMBRObject, IPartnerCacheableNamespace
    {
        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode

        /// <summary>Constructor</summary>
        public TPartnerCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerCacheableNamespace object exists until this AppDomain is unloaded!
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
        private DataTable GetCacheableTableInternal(TCacheablePartnerTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheablePartnerTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheablePartnerTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheablePartnerTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerDataElements Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerDataElementsNamespace : TConfigurableMBRObject, IPartnerDataElementsNamespace
    {
        private TPartnerDataElementsUIConnectorsNamespaceRemote FPartnerDataElementsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerDataElementsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerDataElementsUIConnectorsNamespaceRemote: IPartnerDataElementsUIConnectorsNamespace
        {
            private IPartnerDataElementsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerDataElementsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerDataElementsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerDataElementsUIConnectorsNamespace));
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

        /// <summary>The 'PartnerDataElementsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPartnerDataElementsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PartnerDataElements.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PartnerDataElements.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPartnerDataElementsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerDataElementsUIConnectorsNamespace");
                    TPartnerDataElementsUIConnectorsNamespace ObjectToRemote = new TPartnerDataElementsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerDataElementsUIConnectorsSubNamespace = new TPartnerDataElementsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPartnerDataElementsUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerDataElementsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerDataElementsUIConnectorsNamespace : TConfigurableMBRObject, IPartnerDataElementsUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TPartnerDataElementsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerDataElementsUIConnectorsNamespace object exists until this AppDomain is unloaded!
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

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerServerLookups Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerServerLookupsNamespace : TConfigurableMBRObject, IPartnerServerLookupsNamespace
    {

        /// <summary>Constructor</summary>
        public TPartnerServerLookupsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerServerLookupsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public Boolean GetPartnerShortName(Int64 APartnerKey,
                                           out String APartnerShortName,
                                           out TPartnerClass APartnerClass,
                                           Boolean AMergedPartners)
        {
            #region ManualCode
            return TPartnerServerLookups.GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, AMergedPartners);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean GetPartnerShortName(Int64 APartnerKey,
                                           out String APartnerShortName,
                                           out TPartnerClass APartnerClass)
        {
            #region ManualCode
            return GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, true);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean VerifyPartner(Int64 APartnerKey,
                                     TPartnerClass[] AValidPartnerClasses,
                                     out System.Boolean APartnerExists,
                                     out String APartnerShortName,
                                     out TPartnerClass APartnerClass,
                                     out Boolean AIsMergedPartner)
        {
            #region ManualCode
            return TPartnerServerLookups.VerifyPartner(APartnerKey,
                AValidPartnerClasses,
                out APartnerExists,
                out APartnerShortName,
                out APartnerClass,
                out AIsMergedPartner);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean VerifyPartner(Int64 APartnerKey,
                                     out String APartnerShortName,
                                     out TPartnerClass APartnerClass,
                                     out Boolean AIsMergedPartner,
                                     out Boolean AUserCanAccessPartner)
        {
            #region ManualCode
            return TPartnerServerLookups.VerifyPartner(APartnerKey,
                out APartnerShortName,
                out APartnerClass,
                out AIsMergedPartner,
                out AUserCanAccessPartner);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean VerifyPartner(Int64 APartnerKey)
        {
            #region ManualCode
            return TPartnerServerLookups.VerifyPartner(APartnerKey);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean VerifyPartnerAtLocation(Int64 APartnerKey,
                                               TLocationPK ALocationKey,
                                               out Boolean AAddressNeitherCurrentNorMailing)
        {
            #region ManualCode
            return TPartnerServerLookups.VerifyPartnerAtLocation(APartnerKey,
                ALocationKey, out AAddressNeitherCurrentNorMailing);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean MergedPartnerDetails(Int64 AMergedPartnerPartnerKey,
                                            out String AMergedPartnerPartnerShortName,
                                            out TPartnerClass AMergedPartnerPartnerClass,
                                            out Int64 AMergedIntoPartnerKey,
                                            out String AMergedIntoPartnerShortName,
                                            out TPartnerClass AMergedIntoPartnerClass,
                                            out String AMergedBy,
                                            out DateTime AMergeDate)
        {
            #region ManualCode
            return TPartnerServerLookups.MergedPartnerDetails(AMergedPartnerPartnerKey,
                out AMergedPartnerPartnerShortName,
                out AMergedPartnerPartnerClass,
                out AMergedIntoPartnerKey,
                out AMergedIntoPartnerShortName,
                out AMergedIntoPartnerClass,
                out AMergedBy,
                out AMergeDate);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean PartnerInfo(Int64 APartnerKey,
                                   TPartnerInfoScopeEnum APartnerInfoScope,
                                   out PartnerInfoTDS APartnerInfoDS)
        {
            #region ManualCode
            return TPartnerServerLookups.PartnerInfo(APartnerKey,
                APartnerInfoScope,
                out APartnerInfoDS);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean PartnerInfo(Int64 APartnerKey,
                                   TLocationPK ALocationKey,
                                   TPartnerInfoScopeEnum APartnerInfoScope,
                                   out PartnerInfoTDS APartnerInfoDS)
        {
            #region ManualCode
            return TPartnerServerLookups.PartnerInfo(APartnerKey,
                ALocationKey,
                APartnerInfoScope,
                out APartnerInfoDS);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean GetExtractDescription(String AExtractName,
                                             out String AExtractDescription)
        {
            #region ManualCode
            return TPartnerServerLookups.GetExtractDescription(AExtractName, out AExtractDescription);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean GetPartnerFoundationStatus(Int64 APartnerKey,
                                                  out Boolean AIsFoundation)
        {
            #region ManualCode
            return TPartnerServerLookups.GetPartnerFoundationStatus(APartnerKey, out AIsFoundation);
            #endregion ManualCode
        }

        /// generated method from interface
        public Boolean GetRecentlyUsedPartners(System.Int32 AMaxPartnersCount,
                                               ArrayList APartnerClasses,
                                               out Dictionary<System.Int64, System.String>ARecentlyUsedPartners)
        {
            #region ManualCode
            return TPartnerServerLookups.GetRecentlyUsedPartners(AMaxPartnersCount, APartnerClasses, out ARecentlyUsedPartners);
            #endregion ManualCode
        }

        /// generated method from interface
        public Int64 GetFamilyKeyForPerson(Int64 APersonKey)
        {
            #region ManualCode
            return TPartnerServerLookups.GetFamilyKeyForPerson(APersonKey);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerUIConnectorsNamespace : TConfigurableMBRObject, IPartnerUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TPartnerUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey)
        {
            return new TPartnerEditUIConnector(APartnerKey);
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey,
                                                           ref PartnerEditTDS ADataSet,
                                                           Boolean ADelayedDataLoading,
                                                           TPartnerEditTabPageEnum ATabPage)
        {
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector(APartnerKey);

            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
            return ReturnValue;
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                           Int64 ASiteKey,
                                                           Int32 ALocationKey)
        {
            return new TPartnerEditUIConnector(APartnerKey, ASiteKey, ALocationKey);
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                           Int64 ASiteKey,
                                                           Int32 ALocationKey,
                                                           ref PartnerEditTDS ADataSet,
                                                           Boolean ADelayedDataLoading,
                                                           TPartnerEditTabPageEnum ATabPage)
        {
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector(APartnerKey, ASiteKey, ALocationKey);

            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
            return ReturnValue;
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit()
        {
            return new TPartnerEditUIConnector();
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerEdit PartnerEdit(ref PartnerEditTDS ADataSet,
                                                           Boolean ADelayedDataLoading,
                                                           TPartnerEditTabPageEnum ATabPage)
        {
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector();

            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
            return ReturnValue;
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerFind PartnerFind()
        {
            return new TPartnerFindUIConnector();
        }

        /// generated method from interface
        public IPartnerUIConnectorsPartnerLocationFind PartnerLocationFind(DataTable ACriteriaData)
        {
            return new TPartnerLocationFindUIConnector(ACriteriaData);
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerWebConnectorsNamespace : TConfigurableMBRObject, IPartnerWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TPartnerWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public System.Boolean AddContact(List<Int64>APartnerKeys,
                                         DateTime AContactDate,
                                         System.String AMethodOfContact,
                                         System.String AComment,
                                         System.String AModuleID,
                                         System.String AMailingCode,
                                         out TVerificationResultCollection AVerificationResults)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector), "AddContact", ";LONG?;DATETIME;STRING;STRING;STRING;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector.AddContact(APartnerKeys, AContactDate, AMethodOfContact, AComment, AModuleID, AMailingCode, out AVerificationResults);
        }

        /// generated method from connector
        public PPartnerContactTable FindContacts(System.String AContactor,
                                                 System.Nullable<DateTime>AContactDate,
                                                 System.String ACommentContains,
                                                 System.String AMethodOfContact,
                                                 System.String AModuleID,
                                                 System.String AMailingCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector), "FindContacts", ";STRING;NULLABLE;STRING;STRING;STRING;STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector.FindContacts(AContactor, AContactDate, ACommentContains, AMethodOfContact, AModuleID, AMailingCode);
        }

        /// generated method from connector
        public System.Boolean DeleteContacts(PPartnerContactTable APartnerContacts,
                                             out TVerificationResultCollection AVerificationResults)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector), "DeleteContacts", ";PPARTNERCONTACTTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector.DeleteContacts(APartnerContacts, out AVerificationResults);
        }

        /// generated method from connector
        public MExtractMasterTable GetAllExtractHeaders()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "GetAllExtractHeaders", ";");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.GetAllExtractHeaders();
        }

        /// generated method from connector
        public MExtractMasterTable GetAllExtractHeaders(String AExtractNameFilter,
                                                        Boolean AAllUsers,
                                                        String AUserCreated,
                                                        String AUserModified)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "GetAllExtractHeaders", ";STRING;BOOL;STRING;STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.GetAllExtractHeaders(AExtractNameFilter, AAllUsers, AUserCreated, AUserModified);
        }

        /// generated method from connector
        public Boolean ExtractExists(String AExtractName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "ExtractExists", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.ExtractExists(AExtractName);
        }

        /// generated method from connector
        public Boolean CreateEmptyExtract(ref System.Int32 AExtractId,
                                          String AExtractName,
                                          String AExtractDescription)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "CreateEmptyExtract", ";INT;STRING;STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.CreateEmptyExtract(ref AExtractId, AExtractName, AExtractDescription);
        }

        /// generated method from connector
        public Boolean PurgeExtracts(System.Int32 ANumberOfDays,
                                     Boolean AAllUsers,
                                     String AUserName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "PurgeExtracts", ";INT;BOOL;STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.PurgeExtracts(ANumberOfDays, AAllUsers, AUserName);
        }

        /// generated method from connector
        public ExtractTDSMExtractTable GetExtractRowsWithPartnerData(System.Int32 AExtractId)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "GetExtractRowsWithPartnerData", ";INT;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.GetExtractRowsWithPartnerData(AExtractId);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveExtractMaster(ref MExtractMasterTable AExtractMasterTable,
                                                      out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "SaveExtractMaster", ";MEXTRACTMASTERTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.SaveExtractMaster(ref AExtractMasterTable, out AVerificationResult);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveExtract(System.Int32 AExtractId,
                                                ref MExtractTable AExtractTable,
                                                out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector), "SaveExtract", ";INT;MEXTRACTTABLE;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TExtractMasterWebConnector.SaveExtract(AExtractId, ref AExtractTable, out AVerificationResult);
        }

        /// generated method from connector
        public System.Boolean AddRecentlyUsedPartner(Int64 APartnerKey,
                                                     TPartnerClass APartnerClass,
                                                     Boolean ANewPartner,
                                                     TLastPartnerUse ALastPartnerUse)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector), "AddRecentlyUsedPartner", ";LONG;TPARTNERCLASS;BOOL;TLASTPARTNERUSE;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector.AddRecentlyUsedPartner(APartnerKey, APartnerClass, ANewPartner, ALastPartnerUse);
        }

        /// generated method from connector
        public TLocationPK DetermineBestAddress(Int64 APartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector), "DetermineBestAddress", ";LONG;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector.DetermineBestAddress(APartnerKey);
        }

        /// generated method from connector
        public System.Boolean ChangeFamily(Int64 APersonKey,
                                           Int64 AOldFamilyKey,
                                           Int64 ANewFamilyKey,
                                           out String AProblemMessage,
                                           out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector), "ChangeFamily", ";LONG;LONG;LONG;STRING;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector.ChangeFamily(APersonKey, AOldFamilyKey, ANewFamilyKey, out AProblemMessage, out AVerificationResult);
        }

        /// generated method from connector
        public Int64 GetBankBySortCode(System.String ABranchCode)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector), "GetBankBySortCode", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector.GetBankBySortCode(ABranchCode);
        }

        /// generated method from connector
        public PUnitTable GetConferenceUnits(System.String AConferenceName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader), "GetConferenceUnits", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader.GetConferenceUnits(AConferenceName);
        }

        /// generated method from connector
        public PUnitTable GetOutreachUnits(System.String AOutreachName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader), "GetOutreachUnits", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader.GetOutreachUnits(AOutreachName);
        }

        /// generated method from connector
        public PUnitTable GetActiveFieldUnits(System.String AFieldName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader), "GetActiveFieldUnits", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader.GetActiveFieldUnits(AFieldName);
        }

        /// generated method from connector
        public PUnitTable GetLedgerUnits(System.String ALedgerName)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader), "GetLedgerUnits", ";STRING;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerDataReader.GetLedgerUnits(ALedgerName);
        }

        /// generated method from connector
        public Int64 NewPartnerKey(Int64 AFieldPartnerKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector), "NewPartnerKey", ";LONG;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector.NewPartnerKey(AFieldPartnerKey);
        }

        /// generated method from connector
        public PartnerEditTDS GetPartnerDetails(Int64 APartnerKey,
                                                System.Boolean AWithAddressDetails,
                                                System.Boolean AWithSubscriptions,
                                                System.Boolean AWithRelationships)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector), "GetPartnerDetails", ";LONG;BOOL;BOOL;BOOL;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector.GetPartnerDetails(APartnerKey, AWithAddressDetails, AWithSubscriptions, AWithRelationships);
        }

        /// generated method from connector
        public System.Boolean SavePartner(PartnerEditTDS AMainDS,
                                          out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector), "SavePartner", ";PARTNEREDITTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector.SavePartner(AMainDS, out AVerificationResult);
        }

        /// generated method from connector
        public PartnerFindTDS FindPartners(System.String AFirstName,
                                           System.String AFamilyNameOrOrganisation,
                                           System.String ACity,
                                           StringCollection APartnerClasses)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerFindWebConnector), "FindPartners", ";STRING;STRING;STRING;STRINGCOLLECTION;");
            return Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerFindWebConnector.FindPartners(AFirstName, AFamilyNameOrOrganisation, ACity, APartnerClasses);
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.PartnerMerge
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerMerge Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerMergeNamespace : TConfigurableMBRObject, IPartnerMergeNamespace
    {
        private TPartnerMergeUIConnectorsNamespaceRemote FPartnerMergeUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerMergeNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerMergeNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TPartnerMergeUIConnectorsNamespaceRemote: IPartnerMergeUIConnectorsNamespace
        {
            private IPartnerMergeUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TPartnerMergeUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (IPartnerMergeUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(IPartnerMergeUIConnectorsNamespace));
            }

        }

        /// <summary>The 'PartnerMergeUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPartnerMergeUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'PartnerMerge.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'PartnerMerge.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FPartnerMergeUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TPartnerMergeUIConnectorsNamespace");
                    TPartnerMergeUIConnectorsNamespace ObjectToRemote = new TPartnerMergeUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FPartnerMergeUIConnectorsSubNamespace = new TPartnerMergeUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FPartnerMergeUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.PartnerMerge.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. PartnerMergeUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TPartnerMergeUIConnectorsNamespace : TConfigurableMBRObject, IPartnerMergeUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TPartnerMergeUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerMergeUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Subscriptions
{
    /// <summary>
    /// REMOTEABLE CLASS. Subscriptions Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSubscriptionsNamespace : TConfigurableMBRObject, ISubscriptionsNamespace
    {
        private TSubscriptionsCacheableNamespaceRemote FSubscriptionsCacheableSubNamespace;
        private TSubscriptionsUIConnectorsNamespaceRemote FSubscriptionsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TSubscriptionsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSubscriptionsNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSubscriptionsCacheableNamespaceRemote: ISubscriptionsCacheableNamespace
        {
            private ISubscriptionsCacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSubscriptionsCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISubscriptionsCacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISubscriptionsCacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
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
            public void RefreshCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
                                              out System.Data.DataTable ADataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable,out ADataTable);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
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

        /// <summary>The 'SubscriptionsCacheable' subnamespace contains further subnamespaces.</summary>
        public ISubscriptionsCacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Subscriptions.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Subscriptions.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FSubscriptionsCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSubscriptionsCacheableNamespace");
                    TSubscriptionsCacheableNamespace ObjectToRemote = new TSubscriptionsCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSubscriptionsCacheableSubNamespace = new TSubscriptionsCacheableNamespaceRemote(ObjectURI);
                }

                return FSubscriptionsCacheableSubNamespace;
            }

        }
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TSubscriptionsUIConnectorsNamespaceRemote: ISubscriptionsUIConnectorsNamespace
        {
            private ISubscriptionsUIConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TSubscriptionsUIConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ISubscriptionsUIConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ISubscriptionsUIConnectorsNamespace));
            }

        }

        /// <summary>The 'SubscriptionsUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ISubscriptionsUIConnectorsNamespace UIConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'Subscriptions.UIConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'Subscriptions.UIConnectors' sub-namespace
                //

                // accessing TUIConnectorsNamespace the first time? > instantiate the object
                if (FSubscriptionsUIConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TSubscriptionsUIConnectorsNamespace");
                    TSubscriptionsUIConnectorsNamespace ObjectToRemote = new TSubscriptionsUIConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FSubscriptionsUIConnectorsSubNamespace = new TSubscriptionsUIConnectorsNamespaceRemote(ObjectURI);
                }

                return FSubscriptionsUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. SubscriptionsCacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSubscriptionsCacheableNamespace : TConfigurableMBRObject, ISubscriptionsCacheableNamespace
    {

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TSubscriptionsCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSubscriptionsCacheableNamespace object exists until this AppDomain is unloaded!
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
        private DataTable GetCacheableTableInternal(TCacheableSubscriptionsTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableSubscriptionsTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableSubscriptionsTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                        
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Subscriptions.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. SubscriptionsUIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TSubscriptionsUIConnectorsNamespace : TConfigurableMBRObject, ISubscriptionsUIConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TSubscriptionsUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSubscriptionsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance
{
    /// <summary>
    /// REMOTEABLE CLASS. TableMaintenance Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceNamespace : TConfigurableMBRObject, ITableMaintenanceNamespace
    {
        private TTableMaintenanceUIConnectorsNamespaceRemote FTableMaintenanceUIConnectorsSubNamespace;
        private TTableMaintenanceWebConnectorsNamespaceRemote FTableMaintenanceWebConnectorsSubNamespace;

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
        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TTableMaintenanceWebConnectorsNamespaceRemote: ITableMaintenanceWebConnectorsNamespace
        {
            private ITableMaintenanceWebConnectorsNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TTableMaintenanceWebConnectorsNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ITableMaintenanceWebConnectorsNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ITableMaintenanceWebConnectorsNamespace));
            }

            /// generated method from interface
            public PartnerSetupTDS LoadPartnerTypes()
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.LoadPartnerTypes();
            }
            /// generated method from interface
            public TSubmitChangesResult SavePartnerMaintenanceTables(ref PartnerSetupTDS AInspectDS,
                                                                     out TVerificationResultCollection AVerificationResult)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.SavePartnerMaintenanceTables(ref AInspectDS,out AVerificationResult);
            }
        }

        /// <summary>The 'TableMaintenanceWebConnectors' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'TableMaintenance.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'TableMaintenance.WebConnectors' sub-namespace
                //

                // accessing TWebConnectorsNamespace the first time? > instantiate the object
                if (FTableMaintenanceWebConnectorsSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TTableMaintenanceWebConnectorsNamespace");
                    TTableMaintenanceWebConnectorsNamespace ObjectToRemote = new TTableMaintenanceWebConnectorsNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FTableMaintenanceWebConnectorsSubNamespace = new TTableMaintenanceWebConnectorsNamespaceRemote(ObjectURI);
                }

                return FTableMaintenanceWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.UIConnectors
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

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.WebConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. TableMaintenanceWebConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceWebConnectorsNamespace : TConfigurableMBRObject, ITableMaintenanceWebConnectorsNamespace
    {

        /// <summary>Constructor</summary>
        public TTableMaintenanceWebConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from connector
        public PartnerSetupTDS LoadPartnerTypes()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector), "LoadPartnerTypes", ";");
            return Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector.LoadPartnerTypes();
        }

        /// generated method from connector
        public TSubmitChangesResult SavePartnerMaintenanceTables(ref PartnerSetupTDS AInspectDS,
                                                                 out TVerificationResultCollection AVerificationResult)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector), "SavePartnerMaintenanceTables", ";PARTNERSETUPTDS;TVERIFICATIONRESULTCOLLECTION;");
            return Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector.SavePartnerMaintenanceTables(ref AInspectDS, out AVerificationResult);
        }
    }
}

