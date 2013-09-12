Download the source for MantisConnect from http://mantisconnect.svn.sourceforge.net/viewvc/mantisconnect/mantisconnect/trunk/clients/dotnet/
http://mantisconnect.svn.sourceforge.net/viewvc/mantisconnect/mantisconnect/trunk/clients/dotnet/?view=tar


modified dotnet\mantisconnect\Request.cs to return subprojects as well:

add at top:
        using Futureware.MantisConnect.MantisConnectWebservice;

add after method UserGetAccessibleProjects:

        /// <summary>
        /// Get projects accessible to the currently logged in user.
        /// </summary>
        /// <remarks>
        /// This returns a table ("Projects") which includes two columns ("project_id", "name").
        /// </remarks>
        /// <returns>An array of projects.</returns>
        public ProjectData[] UserGetDetailedAccessibleProjects()
        {
            return mc.mc_projects_get_user_accessible( session.Username, session.Password );
        }

There seems to be a bug on Windows:
it seems to work fine on Linux/mono. see the test.php for debugging purposes.

System.InvalidOperationException: Fehler im XML-Dokument (2,728). ---> System.InvalidCastException: Ein Objekt des Typ S
ystem.Xml.XmlNode[] kann nicht einem Objekt des Typs System.DateTime zugewiesen werden.
   bei Microsoft.Xml.Serialization.GeneratedAssembly.XmlSerializationReaderMantisConnect.Read10_ProjectVersionData()
   bei System.Xml.Serialization.XmlSerializationReader.ReadReferencingElement(String name, String ns, Boolean elementCan
BeType, String& fixupReference)
   bei System.Xml.Serialization.XmlSerializationReader.ReadArray(String typeName, String typeNs)
   bei System.Xml.Serialization.XmlSerializationReader.ReadReferencingElement(String name, String ns, Boolean elementCan
BeType, String& fixupReference)
   bei Microsoft.Xml.Serialization.GeneratedAssembly.XmlSerializationReaderMantisConnect.Read35_Item()
   bei Microsoft.Xml.Serialization.GeneratedAssembly.ArrayOfObjectSerializer49.Deserialize(XmlSerializationReader reader
)
   bei System.Xml.Serialization.XmlSerializer.Deserialize(XmlReader xmlReader, String encodingStyle, XmlDeserializationE
vents events)
   --- Ende der internen Ausnahmestapelüberwachung ---
   bei System.Xml.Serialization.XmlSerializer.Deserialize(XmlReader xmlReader, String encodingStyle, XmlDeserializationE
vents events)
   bei System.Xml.Serialization.XmlSerializer.Deserialize(XmlReader xmlReader, String encodingStyle)
   bei System.Web.Services.Protocols.SoapHttpClientProtocol.ReadResponse(SoapClientMessage message, WebResponse response
, Stream responseStream, Boolean asyncCall)
   bei System.Web.Services.Protocols.SoapHttpClientProtocol.Invoke(String methodName, Object[] parameters)
   bei Futureware.MantisConnect.MantisConnectWebservice.MantisConnect.mc_project_get_versions(String username, String pa
ssword, String project_id)
   bei Futureware.MantisConnect.Request.ProjectGetVersions(Int32 projectId)
   bei Ict.Tools.Mantis.UpdateVersion.UpdateMantisVersion.UpdateVersionsOfProject(Session ASession, Int32 AProjectID, St
ring AVersionReleased, String AVersionDev, String AVersionNext) in c:\Users\tpokorra\Documents\openpetra\trunk\setup\too
ls\MantisUpdateVersions\updateMantisVersion.cs:Zeile 68.
   bei Ict.Tools.Mantis.UpdateVersion.UpdateMantisVersion.Main(String[] args) in c:\Users\tpokorra\Documents\openpetra\t
runk\setup\tools\MantisUpdateVersions\updateMantisVersion.cs:Zeile 172.
System.InvalidCastException: Ein Objekt des Typ System.Xml.XmlNode[] kann nicht einem Objekt des Typs System.DateTime zu
gewiesen werden.
   bei Microsoft.Xml.Serialization.GeneratedAssembly.XmlSerializationReaderMantisConnect.Read10_ProjectVersionData()
   bei System.Xml.Serialization.XmlSerializationReader.ReadReferencingElement(String name, String ns, Boolean elementCan
BeType, String& fixupReference)
   bei System.Xml.Serialization.XmlSerializationReader.ReadArray(String typeName, String typeNs)
   bei System.Xml.Serialization.XmlSerializationReader.ReadReferencingElement(String name, String ns, Boolean elementCan
BeType, String& fixupReference)
   bei Microsoft.Xml.Serialization.GeneratedAssembly.XmlSerializationReaderMantisConnect.Read35_Item()
   bei Microsoft.Xml.Serialization.GeneratedAssembly.ArrayOfObjectSerializer49.Deserialize(XmlSerializationReader reader
)
   bei System.Xml.Serialization.XmlSerializer.Deserialize(XmlReader xmlReader, String encodingStyle, XmlDeserializationE
vents events)
