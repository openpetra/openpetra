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
using Ict.Common.Remoting.Client;
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
    /// <summary>the remoted object</summary>
    private TM{#MODULE} FRemotedObject;

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
        if (TLogging.DL >= 9)
        {
            Console.WriteLine("TM{#MODULE}NamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
        }

        FRemotedObject = new TM{#MODULE}();
        FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TM{#MODULE}NamespaceLoader");

        return FRemotingURL;
    }

    /// <summary>
    /// get the object to be remoted
    /// </summary>
    public TM{#MODULE} GetRemotedObject()
    {
        return FRemotedObject;
    }
}

{##REMOTABLECLASS}
{#IFDEF HIGHESTLEVEL}
/// <summary>
/// REMOTEABLE CLASS. {#NAMESPACE} Namespace (highest level).
/// </summary>
/// <summary>auto generated class </summary>
public class {#LOCALCLASSNAME} : TConfigurableMBRObject, I{#NAMESPACE}Namespace
{#ENDIF HIGHESTLEVEL}
{#IFNDEF HIGHESTLEVEL}
/// <summary>auto generated class </summary>
[Serializable]
public class {#LOCALCLASSNAME} : I{#NAMESPACE}Namespace
{#ENDIFN HIGHESTLEVEL}
{
    /// <summary>Constructor</summary>
    public {#LOCALCLASSNAME}()
    {
    }

{#IFDEF HIGHESTLEVEL}
    /// NOTE AutoGeneration: This function is all-important!!!
    public override object InitializeLifetimeService()
    {
        return null; // make sure that the {#LOCALCLASSNAME} object exists until this AppDomain is unloaded!
    }
{#ENDIF HIGHESTLEVEL}

    {#SUBNAMESPACESREMOTABLECLASS}
}

{##SUBNAMESPACE}
/// <summary>The '{#NAMESPACENAME}' subnamespace contains further subnamespaces.</summary>
public I{#NAMESPACENAME}Namespace {#OBJECTNAME}
{
    get
    {
        return (I{#NAMESPACENAME}Namespace) TCreateRemotableObject.CreateRemotableObject(
                typeof(I{#NAMESPACENAME}Namespace), 
                new T{#NAMESPACENAME}Namespace());
    }
}

{##NAMESPACE}
namespace {#NAMESPACENAME}
{
    {#REMOTABLECLASS}
}

{#SUBNAMESPACES1}

{##CALLFORWARDINGMETHOD}
/// generated method from interface
{#METHODHEAD}
{
    if (RemoteObject == null)
    {
        InitRemoteObject();
    }

    {#METHODCALL}
}


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
{#CALLPROCEDUREINTERNAL}
{#GETDATA}
return ReturnValue;

{##GETREMOTEABLEUICONNECTOROBJECT}
return ({#INTERFACENAME}) TCreateRemotableObject.CreateRemotableObject(
        typeof({#INTERFACENAME}), 
        new T{#UICONNECTORCLASS}({#PARAMETERS}));
