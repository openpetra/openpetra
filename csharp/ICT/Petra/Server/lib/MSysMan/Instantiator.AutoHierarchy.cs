/* Auto generated with nant generateGlue
 * based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.xml
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
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.Interfaces.MSysMan.Application;
using Ict.Petra.Shared.Interfaces.MSysMan.Maintenance;
using Ict.Petra.Shared.Interfaces.MSysMan.TableMaintenance;
using Ict.Petra.Shared.Interfaces.MSysMan.ImportExport;
using Ict.Petra.Shared.Interfaces.MSysMan.PrintManagement;
using Ict.Petra.Shared.Interfaces.MSysMan.Security;
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
using Ict.Petra.Server.MSysMan.Application;
using Ict.Petra.Server.MSysMan.Maintenance;
//using Ict.Petra.Server.MSysMan.TableMaintenance;
using Ict.Petra.Server.MSysMan.ImportExport;
//using Ict.Petra.Server.MSysMan.PrintManagement;
using Ict.Petra.Server.MSysMan.Security;
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
using Ict.Petra.Shared.MSysMan.Data;
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
        /// <summary>holds reference to the TMSysMan object</summary>
        private ObjRef FRemotedObject;

        /// <summary>Constructor</summary>
        public TMSysManNamespaceLoader()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
            }

#endif
        }


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
            TMSysMan RemotedObject;
            DateTime RemotingTime;
            String RemoteAtURI;
            String RandomString;
            System.Security.Cryptography.RNGCryptoServiceProvider rnd;
            Byte rndbytespos;
            Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMSysManNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
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
            RemotedObject = new TMSysMan();
            RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                          (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
            FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IMSysManNamespace));
            FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine("TMSysMan.URI: " + FRemotedObject.URI);
            }

#endif

            return FRemotingURL;
        }


    }


    /// <summary>
    /// REMOTEABLE CLASS. MSysMan Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMSysMan : MarshalByRefObject, IMSysManNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TApplicationNamespace FApplicationSubNamespace;
        private TMaintenanceNamespace FMaintenanceSubNamespace;
        private TTableMaintenanceNamespace FTableMaintenanceSubNamespace;
        private TImportExportNamespace FImportExportSubNamespace;
        private TPrintManagementNamespace FPrintManagementSubNamespace;
        private TSecurityNamespace FSecuritySubNamespace;

        /// <summary>Constructor</summary>
        public TMSysMan()
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
        ~TMSysMan()
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
            return null; // make sure that the TMSysMan object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TApplicationNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.Application') should be automatically contructable.
                    FApplicationSubNamespace = new TApplicationNamespace();
                }


                return FApplicationSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TMaintenanceNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.Maintenance') should be automatically contructable.
                    FMaintenanceSubNamespace = new TMaintenanceNamespace();
                }


                return FMaintenanceSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TTableMaintenanceNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance') should be automatically contructable.
                    FTableMaintenanceSubNamespace = new TTableMaintenanceNamespace();
                }


                return FTableMaintenanceSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TImportExportNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.ImportExport') should be automatically contructable.
                    FImportExportSubNamespace = new TImportExportNamespace();
                }


                return FImportExportSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TPrintManagementNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.PrintManagement') should be automatically contructable.
                    FPrintManagementSubNamespace = new TPrintManagementNamespace();
                }


                return FPrintManagementSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSecurityNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.MSysMan.Instantiator.Security') should be automatically contructable.
                    FSecuritySubNamespace = new TSecurityNamespace();
                }


                return FSecuritySubNamespace;
            }

        }


    }
}

namespace Ict.Petra.Server.MSysMan.Instantiator.Application
{

    /// <summary>auto generated class </summary>
    public class TApplicationNamespace : MarshalByRefObject, IApplicationNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TApplicationUIConnectorsNamespace FApplicationUIConnectorsSubNamespace;
        private TApplicationServerLookupsNamespace FApplicationServerLookupsSubNamespace;

        /// <summary>Constructor</summary>
        public TApplicationNamespace()
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
        ~TApplicationNamespace()
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
            return null; // make sure that the TApplicationNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Application.Instantiator.UIConnectors') should be automatically contructable.
                    FApplicationUIConnectorsSubNamespace = new TApplicationUIConnectorsNamespace();
                }


                return (IApplicationUIConnectorsNamespace)FApplicationUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TServerLookupsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Application.Instantiator.ServerLookups') should be automatically contructable.
                    FApplicationServerLookupsSubNamespace = new TApplicationServerLookupsNamespace();
                }


                return (IApplicationServerLookupsNamespace)FApplicationServerLookupsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Application.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TApplicationUIConnectorsNamespace : MarshalByRefObject, IApplicationUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TApplicationUIConnectorsNamespace()
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
        ~TApplicationUIConnectorsNamespace()
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
            return null; // make sure that the TApplicationUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Application.ServerLookups
{

    /// <summary>auto generated class </summary>
    public class TApplicationServerLookupsNamespace : MarshalByRefObject, IApplicationServerLookupsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TApplicationServerLookupsNamespace()
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
        ~TApplicationServerLookupsNamespace()
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
            return null; // make sure that the TApplicationServerLookupsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public System.Boolean GetDBVersion(out System.String APetraDBVersion)
        {
            #region ManualCode
            return TSysManServerLookups.GetDBVersion(out APetraDBVersion);
            #endregion
        }

        /// generated method from interface
        public System.Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
        {
            #region ManualCode
            return TSysManServerLookups.GetInstalledPatches(out APatchLogDT);
            #endregion
        }

    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance
{

    /// <summary>auto generated class </summary>
    public class TMaintenanceNamespace : MarshalByRefObject, IMaintenanceNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TMaintenanceSystemDefaultsNamespace FMaintenanceSystemDefaultsSubNamespace;
        private TMaintenanceUIConnectorsNamespace FMaintenanceUIConnectorsSubNamespace;
        private TMaintenanceUserDefaultsNamespace FMaintenanceUserDefaultsSubNamespace;
        private TMaintenanceWebConnectorsNamespace FMaintenanceWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMaintenanceNamespace()
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
        ~TMaintenanceNamespace()
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
            return null; // make sure that the TMaintenanceNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TSystemDefaultsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Maintenance.Instantiator.SystemDefaults') should be automatically contructable.
                    FMaintenanceSystemDefaultsSubNamespace = new TMaintenanceSystemDefaultsNamespace();
                }


                return (IMaintenanceSystemDefaultsNamespace)FMaintenanceSystemDefaultsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Maintenance.Instantiator.UIConnectors') should be automatically contructable.
                    FMaintenanceUIConnectorsSubNamespace = new TMaintenanceUIConnectorsNamespace();
                }


                return (IMaintenanceUIConnectorsNamespace)FMaintenanceUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUserDefaultsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Maintenance.Instantiator.UserDefaults') should be automatically contructable.
                    FMaintenanceUserDefaultsSubNamespace = new TMaintenanceUserDefaultsNamespace();
                }


                return (IMaintenanceUserDefaultsNamespace)FMaintenanceUserDefaultsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Maintenance.Instantiator.WebConnectors') should be automatically contructable.
                    FMaintenanceWebConnectorsSubNamespace = new TMaintenanceWebConnectorsNamespace();
                }


                return (IMaintenanceWebConnectorsNamespace)FMaintenanceWebConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.SystemDefaults
{

    /// <summary>auto generated class </summary>
    public class TMaintenanceSystemDefaultsNamespace : MarshalByRefObject, IMaintenanceSystemDefaultsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        #region ManualCode
        private TMaintenanceSystemDefaults FSystemDefaultsManager;
        #endregion ManualCode
        /// <summary>Constructor</summary>
        public TMaintenanceSystemDefaultsNamespace()
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }

            FStartTime = DateTime.Now;
#endif
            #region ManualCode
            FSystemDefaultsManager = new TMaintenanceSystemDefaults();
            #endregion ManualCode
        }


        // NOTE AutoGeneration: This destructor is only needed for debugging...
#if DEBUGMODE
        /// <summary>Destructor</summary>
        ~TMaintenanceSystemDefaultsNamespace()
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
            return null; // make sure that the TMaintenanceSystemDefaultsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public Ict.Petra.Shared.MSysMan.Data.SSystemDefaultsTable GetSystemDefaults()
        {
            #region ManualCode
            return FSystemDefaultsManager.GetSystemDefaults();
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

    /// <summary>auto generated class </summary>
    public class TMaintenanceUIConnectorsNamespace : MarshalByRefObject, IMaintenanceUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TMaintenanceUIConnectorsNamespace()
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
        ~TMaintenanceUIConnectorsNamespace()
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
            return null; // make sure that the TMaintenanceUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.UserDefaults
{

    /// <summary>auto generated class </summary>
    public class TMaintenanceUserDefaultsNamespace : MarshalByRefObject, IMaintenanceUserDefaultsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TMaintenanceUserDefaultsNamespace()
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
        ~TMaintenanceUserDefaultsNamespace()
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
            return null; // make sure that the TMaintenanceUserDefaultsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public void GetUserDefaults(System.String AUserName,
                                    out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
        {
            #region ManualCode
            TMaintenanceUserDefaults.GetUserDefaults(AUserName, out AUserDefaultsDataTable);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Boolean SaveUserDefaults(System.String AUserName,
                                               ref Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable,
                                               out Ict.Common.Verification.TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return TMaintenanceUserDefaults.SaveUserDefaultsFromClientSide(AUserName, ref AUserDefaultsDataTable, out AVerificationResult);
            #endregion ManualCode
        }

        /// generated method from interface
        public void ReloadUserDefaults(System.String AUserName,
                                       out Ict.Petra.Shared.MSysMan.Data.SUserDefaultsTable AUserDefaultsDataTable)
        {
            #region ManualCode
            TMaintenanceUserDefaults.ReloadUserDefaults(AUserName, true, out AUserDefaultsDataTable);
            #endregion ManualCode
        }

    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Maintenance.WebConnectors
{

    /// <summary>auto generated class </summary>
    public class TMaintenanceWebConnectorsNamespace : MarshalByRefObject, IMaintenanceWebConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TMaintenanceWebConnectorsNamespace()
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
        ~TMaintenanceWebConnectorsNamespace()
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
            return null; // make sure that the TMaintenanceWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from connector
        public bool SetUserPassword(string AUsername,
                                    string APassword)
        {
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.SetUserPassword(AUsername, APassword);
        }

        /// generated method from connector
        public bool CreateUser(string AUsername,
                               string APassword,
                               string AModulePermissions)
        {
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.CreateUser(AUsername, APassword, AModulePermissions);
        }

        /// generated method from connector
        public TSubmitChangesResult SaveSUser(ref MaintainUsersTDS ASubmitDS,
                                              out TVerificationResultCollection AVerificationResult)
        {
            return Ict.Petra.Server.MSysMan.Maintenance.WebConnectors.TMaintenanceWebConnector.SaveSUser(ref ASubmitDS, out AVerificationResult);
        }

    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance
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


namespace Ict.Petra.Server.MSysMan.Instantiator.TableMaintenance.UIConnectors
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


        /// generated method from interface
        public ISysManUIConnectorsTableMaintenance SysManTableMaintenance()
        {
            return new TSysManTableMaintenanceUIConnector();
        }

        /// generated method from interface
        public ISysManUIConnectorsTableMaintenance SysManTableMaintenance(ref DataTable ADataSet,
                                                                          string ATableName)
        {
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Creating TSysManTableMaintenanceUIConnector...");
            }

#endif
            TSysManTableMaintenanceUIConnector ReturnValue = new TSysManTableMaintenanceUIConnector();
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TSysManTableMaintenanceUIConnector.GetData...");
            }

#endif
            ADataSet = ReturnValue.GetData(ATableName);
#if DEBUGMODE
            if (TSrvSetting.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Calling TSysManTableMaintenanceUIConnector.GetData finished.");
            }

#endif
            return ReturnValue;
        }

    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.ImportExport
{

    /// <summary>auto generated class </summary>
    public class TImportExportNamespace : MarshalByRefObject, IImportExportNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TImportExportWebConnectorsNamespace FImportExportWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TImportExportNamespace()
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
        ~TImportExportNamespace()
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
            return null; // make sure that the TImportExportNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    //      * for the Generator: the name of this Type ('TWebConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.ImportExport.Instantiator.WebConnectors') should be automatically contructable.
                    FImportExportWebConnectorsSubNamespace = new TImportExportWebConnectorsNamespace();
                }


                return (IImportExportWebConnectorsNamespace)FImportExportWebConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.ImportExport.WebConnectors
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
        ~TImportExportWebConnectorsNamespace()
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
            return null; // make sure that the TImportExportWebConnectorsNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from connector
        public string ExportAllTables()
        {
            return Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector.ExportAllTables();
        }

        /// generated method from connector
        public bool ResetDatabase(string ANewDatabaseData)
        {
            return Ict.Petra.Server.MSysMan.ImportExport.WebConnectors.TImportExportWebConnector.ResetDatabase(ANewDatabaseData);
        }

    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.PrintManagement
{

    /// <summary>auto generated class </summary>
    public class TPrintManagementNamespace : MarshalByRefObject, IPrintManagementNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TPrintManagementUIConnectorsNamespace FPrintManagementUIConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TPrintManagementNamespace()
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
        ~TPrintManagementNamespace()
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
            return null; // make sure that the TPrintManagementNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.PrintManagement.Instantiator.UIConnectors') should be automatically contructable.
                    FPrintManagementUIConnectorsSubNamespace = new TPrintManagementUIConnectorsNamespace();
                }


                return (IPrintManagementUIConnectorsNamespace)FPrintManagementUIConnectorsSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.PrintManagement.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TPrintManagementUIConnectorsNamespace : MarshalByRefObject, IPrintManagementUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TPrintManagementUIConnectorsNamespace()
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
        ~TPrintManagementUIConnectorsNamespace()
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
            return null; // make sure that the TPrintManagementUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Security
{

    /// <summary>auto generated class </summary>
    public class TSecurityNamespace : MarshalByRefObject, ISecurityNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif
        private TSecurityUIConnectorsNamespace FSecurityUIConnectorsSubNamespace;
        private TSecurityUserManagerNamespace FSecurityUserManagerSubNamespace;

        /// <summary>Constructor</summary>
        public TSecurityNamespace()
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
        ~TSecurityNamespace()
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
            return null; // make sure that the TSecurityNamespace object exists until this AppDomain is unloaded!
        }


        // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUIConnectorsNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Security.Instantiator.UIConnectors') should be automatically contructable.
                    FSecurityUIConnectorsSubNamespace = new TSecurityUIConnectorsNamespace();
                }


                return (ISecurityUIConnectorsNamespace)FSecurityUIConnectorsSubNamespace;
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
                    // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
                    //      * for the Generator: the name of this Type ('TUserManagerNamespace') needs to come out of the XML definition,
                    //      * The Namespace where it resides in ('Ict.Petra.Server.Security.Instantiator.UserManager') should be automatically contructable.
                    FSecurityUserManagerSubNamespace = new TSecurityUserManagerNamespace();
                }


                return (ISecurityUserManagerNamespace)FSecurityUserManagerSubNamespace;
            }

        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Security.UIConnectors
{

    /// <summary>auto generated class </summary>
    public class TSecurityUIConnectorsNamespace : MarshalByRefObject, ISecurityUIConnectorsNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TSecurityUIConnectorsNamespace()
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
        ~TSecurityUIConnectorsNamespace()
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
            return null; // make sure that the TSecurityUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }


    }
}


namespace Ict.Petra.Server.MSysMan.Instantiator.Security.UserManager
{

    /// <summary>auto generated class </summary>
    public class TSecurityUserManagerNamespace : MarshalByRefObject, ISecurityUserManagerNamespace
    {
#if DEBUGMODE
        private DateTime FStartTime;
#endif

        /// <summary>Constructor</summary>
        public TSecurityUserManagerNamespace()
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
        ~TSecurityUserManagerNamespace()
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
            return null; // make sure that the TSecurityUserManagerNamespace object exists until this AppDomain is unloaded!
        }


        /// generated method from interface
        public Ict.Petra.Shared.Security.TPetraPrincipal ReloadCachedUserInfo()
        {
            #region ManualCode
            return TUserManager.ReloadCachedUserInfo();
            #endregion ManualCode
        }

        /// generated method from interface
        public void SignalReloadCachedUserInfo(System.String AUserID)
        {
            #region ManualCode
            TUserManager.SignalReloadCachedUserInfo(AUserID);
            #endregion ManualCode
        }

    }
}

