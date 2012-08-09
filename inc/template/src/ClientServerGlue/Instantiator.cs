{##HEADER}
//
// Contains a remotable class that instantiates an Object which gives access to
// the M{#TOPLEVELMODULE} Namespace (from the Client's perspective).
//
// The purpose of the remotable class is to present other classes which are
// Instantiators for sub-namespaces to the Client. The instantiation of the
// sub-namespace objects is completely transparent to the Client!
// The remotable class itself gets instantiated and dynamically remoted by the
// loader class, which in turn gets called when the Client Domain is set up.
//

using System;
using System.Runtime.Remoting;
using System.Threading;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;

{##TOPLEVELNAMESPACE}
namespace Ict.Petra.Server.M{#TOPLEVELMODULE}.Instantiator
{
    {#LOADERCLASS}

    {#REMOTABLECLASS}

    {#SUBNAMESPACES}
}

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

{##NAMESPACE}
{#REMOTABLECLASS}

{#SUBNAMESPACES1}

{##REMOTABLECLASS}
/// <summary>
/// REMOTEABLE CLASS. {#NAMESPACE} Namespace.
/// </summary>
/// <summary>auto generated class </summary>
public class {#LOCALCLASSNAME} : TConfigurableMBRObject, I{#NAMESPACE}Namespace
{
    /// <summary>Constructor</summary>
    public {#LOCALCLASSNAME}()
    {
    }

    /// make sure that the {#LOCALCLASSNAME} object exists until this AppDomain is unloaded!
    public override object InitializeLifetimeService()
    {
        return null;
    }

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
                typeof(T{#NAMESPACENAME}NamespaceRemote),
                new T{#NAMESPACENAME}Namespace());
    }
}