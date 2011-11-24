{##HEADER}
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
{##TOPLEVELNAMESPACE}
namespace Ict.Petra.Server.M{#TOPLEVELMODULE}.Instantiator
{
    {#LOADERCLASS}

    {#REMOTABLECLASS}
}

{#SUBNAMESPACES}

{##LOADERCLASS}
/// <summary>
/// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
/// class to make it callable remotely from the Client.
/// </summary>
public class TM{#MODULE}NamespaceLoader : TConfigurableMBRObject
{
    /// <summary>URL at which the remoted object can be reached</summary>
    private String FRemotingURL;
    /// <summary>holds reference to the TM{#MODULE} object</summary>
    private ObjRef FRemotedObject;

    /// <summary>Constructor</summary>
    public TM{#MODULE}NamespaceLoader()
    {
#if DEBUGMODE
        if (TLogging.DL >= 9)
        {
            Console.WriteLine(this.GetType().FullName + " created in application domain: " + Thread.GetDomain().FriendlyName);
        }

#endif
    }


    /// <summary>
    /// Creates and dynamically exposes an instance of the remoteable TM{#MODULE}
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
        TM{#MODULE} RemotedObject;
        DateTime RemotingTime;
        String RemoteAtURI;
        String RandomString;
        System.Security.Cryptography.RNGCryptoServiceProvider rnd;
        Byte rndbytespos;
        Byte[] rndbytes = new Byte[5];

#if DEBUGMODE
        if (TLogging.DL >= 9)
        {
            Console.WriteLine("TM{#MODULE}NamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
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
        RemotedObject = new TM{#MODULE}();
        RemoteAtURI = (RemotingTime.Day).ToString() + (RemotingTime.Hour).ToString() + (RemotingTime.Minute).ToString() +
                      (RemotingTime.Second).ToString() + '_' + RandomString.ToString();
        FRemotedObject = RemotingServices.Marshal(RemotedObject, RemoteAtURI, typeof(IM{#MODULE}Namespace));
        FRemotingURL = RemoteAtURI; // FRemotedObject.URI;

#if DEBUGMODE
        if (TLogging.DL >= 9)
        {
            Console.WriteLine("TM{#MODULE}.URI: " + FRemotedObject.URI);
        }

#endif

        return FRemotingURL;
    }


}

{##REMOTABLECLASS}
{#IFDEF HIGHESTLEVEL}
/// <summary>
/// REMOTEABLE CLASS. {#NAMESPACE} Namespace (highest level).
/// </summary>
{#ENDIF HIGHESTLEVEL}
/// <summary>auto generated class </summary>
public class {#LOCALCLASSNAME} : MarshalByRefObject, I{#NAMESPACE}Namespace
{
#if DEBUGMODE
    private DateTime FStartTime;
#endif
    {#SUBNAMESPACEDEFINITIONS}


    /// <summary>Constructor</summary>
    public {#LOCALCLASSNAME}()
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
    ~{#LOCALCLASSNAME}()
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
        return null; // make sure that the {#LOCALCLASSNAME} object exists until this AppDomain is unloaded!
    }

{#IFDEF SUBMODULENAMESPACES}
    // NOTE AutoGeneration: There will be one Property like the following for each of the Petra Modules' Sub-Modules (Sub-Namespaces) (these are second-level ... n-level deep for the each Petra Module)
{#ENDIF SUBMODULENAMESPACES}
    {#SUBNAMESPACESREMOTABLECLASS}
}

{##SUBNAMESPACE}

/// <summary>The '{#NAMESPACENAME}' subnamespace contains further subnamespaces.</summary>
public I{#NAMESPACENAME}Namespace {#OBJECTNAME}
{
    get
    {
        //
        // Creates or passes a reference to an instantiator of sub-namespaces that
        // reside in the '{#NAMESPACE}.{#OBJECTNAME}' sub-namespace.
        // A call to this function is done everytime a Client uses an object of this
        // sub-namespace - this is fully transparent to the Client.
        //
        // @return A reference to an instantiator of sub-namespaces that reside in
        //         the '{#NAMESPACE}.{#OBJECTNAME}' sub-namespace
        //

        // accessing T{#OBJECTNAME}Namespace the first time? > instantiate the object
        if (F{#NAMESPACENAME}SubNamespace == null)
        {
            // NOTE AutoGeneration: * the returned Type will need to be manually coded in ManualEndpoints.cs of this Project!
            //      * for the Generator: the name of this Type ('T{#NAMESPACENAME}Namespace') needs to come out of the XML definition,
            //      * The Namespace where it resides in ('Ict.Petra.Server.{#NAMESPACE}.Instantiator.{#OBJECTNAME}') should be automatically contructable.
            F{#NAMESPACENAME}SubNamespace = new T{#NAMESPACENAME}Namespace();
        }


        return F{#NAMESPACENAME}SubNamespace;
    }

}

{##NAMESPACE}
namespace {#NAMESPACENAME}
{
    {#REMOTABLECLASS}
}

{#SUBNAMESPACES1}

{##INTERFACEMETHODS}
{#METHOD}

{##GENERATEDMETHODFROMINTERFACE}

/// generated method from interface
{#PROCEDUREHEADER}
{
    {#CALLPROCEDURE}
}

{##GENERATEDMETHODFROMCONNECTOR}

/// generated method from connector
{#PROCEDUREHEADER}
{
    {#CHECKUSERMODULEPERMISSIONS}
    {#CALLPROCEDURE}
}

{##CHECKUSERMODULEPERMISSIONS}
TModuleAccessManager.CheckUserPermissionsForMethod(typeof({#CONNECTORWITHNAMESPACE}), "{#METHODNAME}", "{#PARAMETERTYPES}"{#LEDGERNUMBER});

{##CALLPROCEDUREWITHGETDATA}
#if DEBUGMODE
if (TLogging.DL >= 9)
{
    Console.WriteLine(this.GetType().FullName + ": Creating T{#CONNECTORTYPE}...");
}

#endif
{#CALLPROCEDUREINTERNAL}
#if DEBUGMODE
if (TLogging.DL >= 9)
{
    Console.WriteLine(this.GetType().FullName + ": Calling T{#CONNECTORTYPE}.GetData...");
}

#endif
{#GETDATA}
#if DEBUGMODE
if (TLogging.DL >= 9)
{
    Console.WriteLine(this.GetType().FullName + ": Calling T{#CONNECTORTYPE}.GetData finished.");
}

#endif
return ReturnValue;
