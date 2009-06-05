/* Auto generated with nant generateGlue
 * based on u:\OpenPetra\csharp\ICT\Petra\Definitions\NamespaceHierarchy.xml
 */
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
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPersonnel;
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
using Ict.Petra.Server.MPersonnel.Instantiator.Person;
using Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance;
using Ict.Petra.Server.MPersonnel.Instantiator.Units;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds;
using Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance.UIConnectors;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable;
using Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Person;
//using Ict.Petra.Server.MPersonnel.TableMaintenance;
//using Ict.Petra.Server.MPersonnel.Units;
//using Ict.Petra.Server.MPersonnel.Person.DataElements;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications.Cacheable;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Applications.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.Cacheable;
//using Ict.Petra.Server.MPersonnel.Person.DataElements.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Person.Shepherds;
//using Ict.Petra.Server.MPersonnel.Person.Shepherds.UIConnectors;
//using Ict.Petra.Server.MPersonnel.TableMaintenance.UIConnectors;
//using Ict.Petra.Server.MPersonnel.Units.DataElements;
//using Ict.Petra.Server.MPersonnel.Units.DataElements.Cacheable;
//using Ict.Petra.Server.MPersonnel.Units.DataElements.UIConnectors;


#region ManualCode
using Ict.Petra.Server.MCommon.UIConnectors;
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
        /// <summary>holds reference to the TMPersonnel object</summary>
        private ObjRef FRemotedObject;

        /// <summary>Constructor</summary>
        public TMPersonnelNamespaceLoader()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
            }

#endif
        }


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
            TMPersonnel RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;
            String RandomString;
            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte rndbytespos;
            Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMPersonnelNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
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
            RemotedObject = new TMPersonnel();
            RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IMPersonnelNamespace));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMPersonnel.URI: " + FRemotedObject.URI);
            }

#endif

            return FRemotingURL;
        }


    }


    /// <summary>
    /// REMOTEABLE CLASS. MPersonnel Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMPersonnel : MarshalByRefObject, IMPersonnelNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPersonNamespace FPersonSubNamespace;
        private TTableMaintenanceNamespace FTableMaintenanceSubNamespace;
        private TUnitsNamespace FUnitsSubNamespace;

        /// <summary>Constructor</summary>
        public TMPersonnel()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMPersonnel()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMPersonnel object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPersonNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPersonnel.Instantiator.Person') should be automatically contructable.
                    FPersonSubNamespace = new TPersonNamespace();
                }


                return FPersonSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TTableMaintenanceNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance') should be automatically contructable.
                    FTableMaintenanceSubNamespace = new TTableMaintenanceNamespace();
                }


                return FTableMaintenanceSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUnitsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MPersonnel.Instantiator.Units') should be automatically contructable.
                    FUnitsSubNamespace = new TUnitsNamespace();
                }


                return FUnitsSubNamespace;
            }

        }


    }
}

namespace Ict.Petra.Server.MPersonnel.Instantiator.Person
{

    /// <summary>auto generated class </summary>
    public class TPersonNamespace : MarshalByRefObject, IPersonNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPersonDataElementsNamespace FPersonDataElementsSubNamespace;
        private TPersonShepherdsNamespace FPersonShepherdsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TDataElementsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Person.Instantiator.DataElements') should be automatically contructable.
                    FPersonDataElementsSubNamespace = new TPersonDataElementsNamespace();
                }


                return (IPersonDataElementsNamespace)FPersonDataElementsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TShepherdsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Person.Instantiator.Shepherds') should be automatically contructable.
                    FPersonShepherdsSubNamespace = new TPersonShepherdsNamespace();
                }


                return (IPersonShepherdsNamespace)FPersonShepherdsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsNamespace : MarshalByRefObject, IPersonDataElementsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPersonDataElementsApplicationsNamespace FPersonDataElementsApplicationsSubNamespace;
        private TPersonDataElementsCacheableNamespace FPersonDataElementsCacheableSubNamespace;
        private TPersonDataElementsUIConnectorsNamespace FPersonDataElementsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonDataElementsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TApplicationsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonDataElements.Instantiator.Applications') should be automatically contructable.
                    FPersonDataElementsApplicationsSubNamespace = new TPersonDataElementsApplicationsNamespace();
                }


                return (IPersonDataElementsApplicationsNamespace)FPersonDataElementsApplicationsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonDataElements.Instantiator.Cacheable') should be automatically contructable.
                    FPersonDataElementsCacheableSubNamespace = new TPersonDataElementsCacheableNamespace();
                }


                return (IPersonDataElementsCacheableNamespace)FPersonDataElementsCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonDataElements.Instantiator.UIConnectors') should be automatically contructable.
                    FPersonDataElementsUIConnectorsSubNamespace = new TPersonDataElementsUIConnectorsNamespace();
                }


                return (IPersonDataElementsUIConnectorsNamespace)FPersonDataElementsUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsNamespace : MarshalByRefObject, IPersonDataElementsApplicationsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPersonDataElementsApplicationsCacheableNamespace FPersonDataElementsApplicationsCacheableSubNamespace;
        private TPersonDataElementsApplicationsUIConnectorsNamespace FPersonDataElementsApplicationsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsApplicationsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonDataElementsApplications.Instantiator.Cacheable') should be automatically contructable.
                    FPersonDataElementsApplicationsCacheableSubNamespace = new TPersonDataElementsApplicationsCacheableNamespace();
                }


                return (IPersonDataElementsApplicationsCacheableNamespace)FPersonDataElementsApplicationsCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonDataElementsApplications.Instantiator.UIConnectors') should be automatically contructable.
                    FPersonDataElementsApplicationsUIConnectorsSubNamespace = new TPersonDataElementsApplicationsUIConnectorsNamespace();
                }


                return (IPersonDataElementsApplicationsUIConnectorsNamespace)FPersonDataElementsApplicationsUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.Cacheable
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsCacheableNamespace : MarshalByRefObject, IPersonDataElementsApplicationsCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsCacheableNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsApplicationsCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsCacheableNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonDataElementsTablesEnum ACacheableTable)
        {
            #region ManualCode

            //todo
            return null;
            #endregion ManualCode
        }

    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Applications.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsApplicationsUIConnectorsNamespace : MarshalByRefObject, IPersonDataElementsApplicationsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPersonDataElementsApplicationsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsApplicationsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsApplicationsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(
                                                                                                                                           System.Int64 APartnerKey,
                                                                                                                                           System.Int32 AApplicationKey,
                                                                                                                                           System.Int64 ARegistrationOffice,
                                                                                                                                           Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                           out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TOfficeSpecificDataLabelsUIConnector...");
            }

#endif
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey, 
               AApplicationKey, 
               ARegistrationOffice, 
               AOfficeSpecificDataLabelUse);
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData...");
            }

#endif
            AOfficeSpecificDataLabelsDataSet = ReturnValue.GetData();
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData finished.");
            }

#endif
            return ReturnValue;
        }

    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.Cacheable
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsCacheableNamespace : MarshalByRefObject, IPersonDataElementsCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPersonDataElementsCacheableNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsCacheableNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheablePersonDataElementsTablesEnum ACacheableTable)
        {
            #region ManualCode

            //todo
            return null;
            #endregion ManualCode
        }

    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.DataElements.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TPersonDataElementsUIConnectorsNamespace : MarshalByRefObject, IPersonDataElementsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPersonDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonDataElementsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(
                                                                                                                                           System.Int64 APartnerKey,
                                                                                                                                           Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                           out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet)
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TOfficeSpecificDataLabelsUIConnector...");
            }

#endif
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AOfficeSpecificDataLabelUse);
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData...");
            }

#endif
            AOfficeSpecificDataLabelsDataSet = ReturnValue.GetData();
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TOfficeSpecificDataLabelsUIConnector.GetData finished.");
            }

#endif
            return ReturnValue;
        }

    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds
{

    /// <summary>auto generated class </summary>
    public class TPersonShepherdsNamespace : MarshalByRefObject, IPersonShepherdsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPersonShepherdsUIConnectorsNamespace FPersonShepherdsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPersonShepherdsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonShepherdsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonShepherdsNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PersonShepherds.Instantiator.UIConnectors') should be automatically contructable.
                    FPersonShepherdsUIConnectorsSubNamespace = new TPersonShepherdsUIConnectorsNamespace();
                }


                return (IPersonShepherdsUIConnectorsNamespace)FPersonShepherdsUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Person.Shepherds.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TPersonShepherdsUIConnectorsNamespace : MarshalByRefObject, IPersonShepherdsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPersonShepherdsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TPersonShepherdsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPersonShepherdsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance
{

    /// <summary>auto generated class </summary>
    public class TTableMaintenanceNamespace : MarshalByRefObject, ITableMaintenanceNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TTableMaintenanceUIConnectorsNamespace FTableMaintenanceUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TTableMaintenanceNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
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
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
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
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.TableMaintenance.Instantiator.UIConnectors') should be automatically contructable.
                    FTableMaintenanceUIConnectorsSubNamespace = new TTableMaintenanceUIConnectorsNamespace();
                }


                return (ITableMaintenanceUIConnectorsNamespace)FTableMaintenanceUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.TableMaintenance.UIConnectors
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
            if (TSrvSetting.DL >= 9)
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
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
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


namespace Ict.Petra.Server.MPersonnel.Instantiator.Units
{

    /// <summary>auto generated class </summary>
    public class TUnitsNamespace : MarshalByRefObject, IUnitsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TUnitsDataElementsNamespace FUnitsDataElementsSubNamespace;

        /// <summary>Constructor</summary>
        public TUnitsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TUnitsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TDataElementsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Units.Instantiator.DataElements') should be automatically contructable.
                    FUnitsDataElementsSubNamespace = new TUnitsDataElementsNamespace();
                }


                return (IUnitsDataElementsNamespace)FUnitsDataElementsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements
{

    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsNamespace : MarshalByRefObject, IUnitsDataElementsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TUnitsDataElementsCacheableNamespace FUnitsDataElementsCacheableSubNamespace;
        private TUnitsDataElementsUIConnectorsNamespace FUnitsDataElementsUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TUnitsDataElementsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TUnitsDataElementsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TCacheableNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.UnitsDataElements.Instantiator.Cacheable') should be automatically contructable.
                    FUnitsDataElementsCacheableSubNamespace = new TUnitsDataElementsCacheableNamespace();
                }


                return (IUnitsDataElementsCacheableNamespace)FUnitsDataElementsCacheableSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.UnitsDataElements.Instantiator.UIConnectors') should be automatically contructable.
                    FUnitsDataElementsUIConnectorsSubNamespace = new TUnitsDataElementsUIConnectorsNamespace();
                }


                return (IUnitsDataElementsUIConnectorsNamespace)FUnitsDataElementsUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.Cacheable
{

    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsCacheableNamespace : MarshalByRefObject, IUnitsDataElementsCacheableNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TUnitsDataElementsCacheableNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TUnitsDataElementsCacheableNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsCacheableNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPersonnel.TCacheableUnitsDataElementsTablesEnum ACacheableTable)
        {
            #region ManualCode

            //todo
            return null;
            #endregion ManualCode
        }

    }
}


namespace Ict.Petra.Server.MPersonnel.Instantiator.Units.DataElements.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TUnitsDataElementsUIConnectorsNamespace : MarshalByRefObject, IUnitsDataElementsUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TUnitsDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TUnitsDataElementsUIConnectorsNamespace()
        {
#if DEBUGMODELONGRUNNINGFINALIZERS
            const Int32 MAX_ITERATIONS = 100000;
            System.Int32 LoopCounter;
            object MyObject;
            object MyObject2;
#endif
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }

#if DEBUGMODELONGRUNNINGFINALIZERS
            MyObject = new object();
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Now performing some longer-running stuff...");
            }

            for (LoopCounter = 0; LoopCounter <= MAX_ITERATIONS; LoopCounter += 1)
            {
                MyObject2 = new object();
                GC.KeepAlive(MyObject);
            }

            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": FINALIZER has run.");
            }

#endif
        }

#endif



        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUnitsDataElementsUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}

