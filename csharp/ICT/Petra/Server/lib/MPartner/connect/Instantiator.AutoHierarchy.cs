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
        /// <summary>holds reference to the TMPartner object</summary>
        private ObjRef FRemotedObject;

        /// <summary>Constructor</summary>
        public TMPartnerNamespaceLoader()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
            }

#endif
        }

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
            TMPartner RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;
            String RandomString;
            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte rndbytespos;
            Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMPartnerNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

#endif

            RandomString = "";
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rnd.GetBytes(rndbytes);

            for (rndbytespos = 1; rndbytespos <= 4; rndbytespos += 1)
            {
                RandomString = RandomString + rndbytes[rndbytespos].ToString();
            }

            RemotingTime = DateTime.Now;
            RemotedObject = new TMPartner();
            RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IMPartnerNamespace));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("TMPartner.URI: " + FRemotedObject.URI);
            }

#endif

            return FRemotingURL;
        }

    }

    /// <summary>
    /// REMOTEABLE CLASS. MPartner Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMPartner : MarshalByRefObject, IMPartnerNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TExtractsNamespace FExtractsSubNamespace;
        private TImportExportNamespace FImportExportSubNamespace;
        private TMailingNamespace FMailingSubNamespace;
        private TPartnerNamespace FPartnerSubNamespace;
        private TPartnerMergeNamespace FPartnerMergeSubNamespace;
        private TSubscriptionsNamespace FSubscriptionsSubNamespace;
        private TTableMaintenanceNamespace FTableMaintenanceSubNamespace;

        /// <summary>Constructor</summary>
        public TMPartner()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMPartner()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMPartner object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TExtractsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.Extracts') should be automatically contructable.
                    FExtractsSubNamespace = new TExtractsNamespace();
                }

                return FExtractsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.ImportExport') should be automatically contructable.
                    FImportExportSubNamespace = new TImportExportNamespace();
                }

                return FImportExportSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TMailingNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.Mailing') should be automatically contructable.
                    FMailingSubNamespace = new TMailingNamespace();
                }

                return FMailingSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.Partner') should be automatically contructable.
                    FPartnerSubNamespace = new TPartnerNamespace();
                }

                return FPartnerSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerMergeNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.PartnerMerge') should be automatically contructable.
                    FPartnerMergeSubNamespace = new TPartnerMergeNamespace();
                }

                return FPartnerMergeSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSubscriptionsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.Subscriptions') should be automatically contructable.
                    FSubscriptionsSubNamespace = new TSubscriptionsNamespace();
                }

                return FSubscriptionsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TTableMaintenanceNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPartner.Instantiator.TableMaintenance') should be automatically contructable.
                    FTableMaintenanceSubNamespace = new TTableMaintenanceNamespace();
                }

                return FTableMaintenanceSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Extracts
{
    /// <summary>auto generated class </summary>
    public class TExtractsNamespace : MarshalByRefObject, IExtractsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TExtractsUIConnectorsNamespace FExtractsUIConnectorsSubNamespace;
        private TExtractsWebConnectorsNamespace FExtractsWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TExtractsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TExtractsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TExtractsNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TExtractsUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Extracts.Instantiator.UIConnectors') should be automatically contructable.
                    FExtractsUIConnectorsSubNamespace = new TExtractsUIConnectorsNamespace();
                }

                return FExtractsUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TExtractsWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Extracts.Instantiator.WebConnectors') should be automatically contructable.
                    FExtractsWebConnectorsSubNamespace = new TExtractsWebConnectorsNamespace();
                }

                return FExtractsWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Extracts.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TExtractsUIConnectorsNamespace : MarshalByRefObject, IExtractsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TExtractsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TExtractsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
    /// <summary>auto generated class </summary>
    public class TExtractsWebConnectorsNamespace : MarshalByRefObject, IExtractsWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TExtractsWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TExtractsWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TExtractsWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport
{
    /// <summary>auto generated class </summary>
    public class TImportExportNamespace : MarshalByRefObject, IImportExportNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TImportExportUIConnectorsNamespace FImportExportUIConnectorsSubNamespace;
        private TImportExportWebConnectorsNamespace FImportExportWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TImportExportNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TImportExportNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.ImportExport.Instantiator.UIConnectors') should be automatically contructable.
                    FImportExportUIConnectorsSubNamespace = new TImportExportUIConnectorsNamespace();
                }

                return FImportExportUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.ImportExport.Instantiator.WebConnectors') should be automatically contructable.
                    FImportExportWebConnectorsSubNamespace = new TImportExportWebConnectorsNamespace();
                }

                return FImportExportWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TImportExportUIConnectorsNamespace : MarshalByRefObject, IImportExportUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TImportExportUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TImportExportUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.ImportExport.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TImportExportWebConnectorsNamespace : MarshalByRefObject, IImportExportWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TImportExportWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TImportExportWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
    /// <summary>auto generated class </summary>
    public class TMailingNamespace : MarshalByRefObject, IMailingNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TMailingCacheableNamespace FMailingCacheableSubNamespace;
        private TMailingUIConnectorsNamespace FMailingUIConnectorsSubNamespace;
        private TMailingWebConnectorsNamespace FMailingWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMailingNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMailingNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TMailingCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Mailing.Instantiator.Cacheable') should be automatically contructable.
                    FMailingCacheableSubNamespace = new TMailingCacheableNamespace();
                }

                return FMailingCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TMailingUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Mailing.Instantiator.UIConnectors') should be automatically contructable.
                    FMailingUIConnectorsSubNamespace = new TMailingUIConnectorsNamespace();
                }

                return FMailingUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TMailingWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Mailing.Instantiator.WebConnectors') should be automatically contructable.
                    FMailingWebConnectorsSubNamespace = new TMailingWebConnectorsNamespace();
                }

                return FMailingWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable
{
    /// <summary>auto generated class </summary>
    public class TMailingCacheableNamespace : MarshalByRefObject, IMailingCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TMailingCacheableNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMailingCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TMailingUIConnectorsNamespace : MarshalByRefObject, IMailingUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TMailingUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMailingUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMailingUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Mailing.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TMailingWebConnectorsNamespace : MarshalByRefObject, IMailingWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TMailingWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMailingWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
    /// <summary>auto generated class </summary>
    public class TPartnerNamespace : MarshalByRefObject, IPartnerNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPartnerCacheableNamespace FPartnerCacheableSubNamespace;
        private TPartnerDataElementsNamespace FPartnerDataElementsSubNamespace;
        private TPartnerServerLookupsNamespace FPartnerServerLookupsSubNamespace;
        private TPartnerUIConnectorsNamespace FPartnerUIConnectorsSubNamespace;
        private TPartnerWebConnectorsNamespace FPartnerWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Partner.Instantiator.Cacheable') should be automatically contructable.
                    FPartnerCacheableSubNamespace = new TPartnerCacheableNamespace();
                }

                return FPartnerCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerDataElementsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Partner.Instantiator.DataElements') should be automatically contructable.
                    FPartnerDataElementsSubNamespace = new TPartnerDataElementsNamespace();
                }

                return FPartnerDataElementsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerServerLookupsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Partner.Instantiator.ServerLookups') should be automatically contructable.
                    FPartnerServerLookupsSubNamespace = new TPartnerServerLookupsNamespace();
                }

                return FPartnerServerLookupsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Partner.Instantiator.UIConnectors') should be automatically contructable.
                    FPartnerUIConnectorsSubNamespace = new TPartnerUIConnectorsNamespace();
                }

                return FPartnerUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Partner.Instantiator.WebConnectors') should be automatically contructable.
                    FPartnerWebConnectorsSubNamespace = new TPartnerWebConnectorsNamespace();
                }

                return FPartnerWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable
{
    /// <summary>auto generated class </summary>
    public class TPartnerCacheableNamespace : MarshalByRefObject, IPartnerCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TPartnerCacheableNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable,
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
    /// <summary>auto generated class </summary>
    public class TPartnerDataElementsNamespace : MarshalByRefObject, IPartnerDataElementsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPartnerDataElementsUIConnectorsNamespace FPartnerDataElementsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerDataElementsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerDataElementsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerDataElementsNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerDataElementsUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PartnerDataElements.Instantiator.UIConnectors') should be automatically contructable.
                    FPartnerDataElementsUIConnectorsSubNamespace = new TPartnerDataElementsUIConnectorsNamespace();
                }

                return FPartnerDataElementsUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TPartnerDataElementsUIConnectorsNamespace : MarshalByRefObject, IPartnerDataElementsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPartnerDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TOfficeSpecificDataLabelsUIConnector...");
            }

#endif
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AOfficeSpecificDataLabelUse);

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData...");
            }

#endif
            AOfficeSpecificDataLabelsDataSet = ReturnValue.GetData();
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData finished.");
            }

#endif
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups
{
    /// <summary>auto generated class </summary>
    public class TPartnerServerLookupsNamespace : MarshalByRefObject, IPartnerServerLookupsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPartnerServerLookupsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerServerLookupsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
                                     out String APartnerShortName,
                                     out TPartnerClass APartnerClass,
                                     out Boolean AIsMergedPartner)
        {
            #region ManualCode
            return TPartnerServerLookups.VerifyPartner(APartnerKey,
                AValidPartnerClasses,
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
    /// <summary>auto generated class </summary>
    public class TPartnerUIConnectorsNamespace : MarshalByRefObject, IPartnerUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPartnerUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TPartnerEditUIConnector...");
            }

#endif
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector(APartnerKey);

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData...");
            }

#endif
            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData finished.");
            }

#endif
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
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TPartnerEditUIConnector...");
            }

#endif
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector(APartnerKey, ASiteKey, ALocationKey);

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData...");
            }

#endif
            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData finished.");
            }

#endif
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
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TPartnerEditUIConnector...");
            }

#endif
            TPartnerEditUIConnector ReturnValue = new TPartnerEditUIConnector();

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData...");
            }

#endif
            ADataSet = ReturnValue.GetData(ADelayedDataLoading, ATabPage);
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TPartnerEditUIConnector.GetData finished.");
            }

#endif
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
    /// <summary>auto generated class </summary>
    public class TPartnerWebConnectorsNamespace : MarshalByRefObject, IPartnerWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPartnerWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector), "AddContact", ";LIST;DATETIME;STRING;STRING;STRING;STRING;TVERIFICATIONRESULTCOLLECTION;");
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
    /// <summary>auto generated class </summary>
    public class TPartnerMergeNamespace : MarshalByRefObject, IPartnerMergeNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPartnerMergeUIConnectorsNamespace FPartnerMergeUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPartnerMergeNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerMergeNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerMergeNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPartnerMergeUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PartnerMerge.Instantiator.UIConnectors') should be automatically contructable.
                    FPartnerMergeUIConnectorsSubNamespace = new TPartnerMergeUIConnectorsNamespace();
                }

                return FPartnerMergeUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.PartnerMerge.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TPartnerMergeUIConnectorsNamespace : MarshalByRefObject, IPartnerMergeUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPartnerMergeUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPartnerMergeUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPartnerMergeUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Subscriptions
{
    /// <summary>auto generated class </summary>
    public class TSubscriptionsNamespace : MarshalByRefObject, ISubscriptionsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TSubscriptionsCacheableNamespace FSubscriptionsCacheableSubNamespace;
        private TSubscriptionsUIConnectorsNamespace FSubscriptionsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TSubscriptionsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSubscriptionsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSubscriptionsNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSubscriptionsCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Subscriptions.Instantiator.Cacheable') should be automatically contructable.
                    FSubscriptionsCacheableSubNamespace = new TSubscriptionsCacheableNamespace();
                }

                return FSubscriptionsCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSubscriptionsUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Subscriptions.Instantiator.UIConnectors') should be automatically contructable.
                    FSubscriptionsUIConnectorsSubNamespace = new TSubscriptionsUIConnectorsNamespace();
                }

                return FSubscriptionsUIConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable
{
    /// <summary>auto generated class </summary>
    public class TSubscriptionsCacheableNamespace : MarshalByRefObject, ISubscriptionsCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode

        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable FCachePopulator;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TSubscriptionsCacheableNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable();
            #endregion ManualCode
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSubscriptionsCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable,
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
    /// <summary>auto generated class </summary>
    public class TSubscriptionsUIConnectorsNamespace : MarshalByRefObject, ISubscriptionsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TSubscriptionsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TSubscriptionsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSubscriptionsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance
{
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceNamespace : MarshalByRefObject, ITableMaintenanceNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TTableMaintenanceUIConnectorsNamespace FTableMaintenanceUIConnectorsSubNamespace;
        private TTableMaintenanceWebConnectorsNamespace FTableMaintenanceWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TTableMaintenanceNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TTableMaintenanceNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)

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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TTableMaintenanceUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.TableMaintenance.Instantiator.UIConnectors') should be automatically contructable.
                    FTableMaintenanceUIConnectorsSubNamespace = new TTableMaintenanceUIConnectorsNamespace();
                }

                return FTableMaintenanceUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TTableMaintenanceWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.TableMaintenance.Instantiator.WebConnectors') should be automatically contructable.
                    FTableMaintenanceWebConnectorsSubNamespace = new TTableMaintenanceWebConnectorsNamespace();
                }

                return FTableMaintenanceWebConnectorsSubNamespace;
            }

        }
    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.UIConnectors
{
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceUIConnectorsNamespace : MarshalByRefObject, ITableMaintenanceUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TTableMaintenanceUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TTableMaintenanceUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

    }
}

namespace Ict.Petra.Server.MPartner.Instantiator.TableMaintenance.WebConnectors
{
    /// <summary>auto generated class </summary>
    public class TTableMaintenanceWebConnectorsNamespace : MarshalByRefObject, ITableMaintenanceWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TTableMaintenanceWebConnectorsNamespace()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }

        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TTableMaintenanceWebConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif

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

