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
        /// <summary>Constructor</summary>
        public TMSysMan()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMSysMan object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'Application' subnamespace contains further subnamespaces.</summary>
        public IApplicationNamespace Application
        {
            get
            {
                return (IApplicationNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IApplicationNamespace),
                        new TApplicationNamespace());
            }
        }
        /// <summary>The 'Maintenance' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceNamespace Maintenance
        {
            get
            {
                return (IMaintenanceNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IMaintenanceNamespace),
                        new TMaintenanceNamespace());
            }
        }
        /// <summary>The 'TableMaintenance' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceNamespace TableMaintenance
        {
            get
            {
                return (ITableMaintenanceNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ITableMaintenanceNamespace),
                        new TTableMaintenanceNamespace());
            }
        }
        /// <summary>The 'ImportExport' subnamespace contains further subnamespaces.</summary>
        public IImportExportNamespace ImportExport
        {
            get
            {
                return (IImportExportNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IImportExportNamespace),
                        new TImportExportNamespace());
            }
        }
        /// <summary>The 'PrintManagement' subnamespace contains further subnamespaces.</summary>
        public IPrintManagementNamespace PrintManagement
        {
            get
            {
                return (IPrintManagementNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPrintManagementNamespace),
                        new TPrintManagementNamespace());
            }
        }
        /// <summary>The 'Security' subnamespace contains further subnamespaces.</summary>
        public ISecurityNamespace Security
        {
            get
            {
                return (ISecurityNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ISecurityNamespace),
                        new TSecurityNamespace());
            }
        }
        /// <summary>The 'Cacheable' subnamespace contains further subnamespaces.</summary>
        public ICacheableNamespace Cacheable
        {
            get
            {
                return (ICacheableNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ICacheableNamespace),
                        new TCacheableNamespace());
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
        /// <summary>Constructor</summary>
        public TApplicationNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TApplicationNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'ApplicationUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IApplicationUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IApplicationUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IApplicationUIConnectorsNamespace),
                        new TApplicationUIConnectorsNamespace());
            }
        }
        /// <summary>The 'ApplicationServerLookups' subnamespace contains further subnamespaces.</summary>
        public IApplicationServerLookupsNamespace ServerLookups
        {
            get
            {
                return (IApplicationServerLookupsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IApplicationServerLookupsNamespace),
                        new TApplicationServerLookupsNamespace());
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
        /// <summary>Constructor</summary>
        public TMaintenanceNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'MaintenanceSystemDefaults' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceSystemDefaultsNamespace SystemDefaults
        {
            get
            {
                return (IMaintenanceSystemDefaultsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IMaintenanceSystemDefaultsNamespace),
                        new TMaintenanceSystemDefaultsNamespace());
            }
        }
        /// <summary>The 'MaintenanceUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IMaintenanceUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IMaintenanceUIConnectorsNamespace),
                        new TMaintenanceUIConnectorsNamespace());
            }
        }
        /// <summary>The 'MaintenanceUserDefaults' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceUserDefaultsNamespace UserDefaults
        {
            get
            {
                return (IMaintenanceUserDefaultsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IMaintenanceUserDefaultsNamespace),
                        new TMaintenanceUserDefaultsNamespace());
            }
        }
        /// <summary>The 'MaintenanceWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IMaintenanceWebConnectorsNamespace WebConnectors
        {
            get
            {
                return (IMaintenanceWebConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IMaintenanceWebConnectorsNamespace),
                        new TMaintenanceWebConnectorsNamespace());
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
        /// <summary>Constructor</summary>
        public TTableMaintenanceNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TTableMaintenanceNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'TableMaintenanceUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ITableMaintenanceUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (ITableMaintenanceUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ITableMaintenanceUIConnectorsNamespace),
                        new TTableMaintenanceUIConnectorsNamespace());
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
            return (ISysManUIConnectorsTableMaintenance) TCreateRemotableObject.CreateRemotableObject(
                    typeof(ISysManUIConnectorsTableMaintenance),
                    new TSysManTableMaintenanceUIConnector());
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
        /// <summary>Constructor</summary>
        public TImportExportNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TImportExportNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'ImportExportWebConnectors' subnamespace contains further subnamespaces.</summary>
        public IImportExportWebConnectorsNamespace WebConnectors
        {
            get
            {
                return (IImportExportWebConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IImportExportWebConnectorsNamespace),
                        new TImportExportWebConnectorsNamespace());
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
        /// <summary>Constructor</summary>
        public TPrintManagementNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TPrintManagementNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'PrintManagementUIConnectors' subnamespace contains further subnamespaces.</summary>
        public IPrintManagementUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IPrintManagementUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IPrintManagementUIConnectorsNamespace),
                        new TPrintManagementUIConnectorsNamespace());
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
        /// <summary>Constructor</summary>
        public TSecurityNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TSecurityNamespace object exists until this AppDomain is unloaded!
        }

        /// <summary>The 'SecurityUIConnectors' subnamespace contains further subnamespaces.</summary>
        public ISecurityUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (ISecurityUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ISecurityUIConnectorsNamespace),
                        new TSecurityUIConnectorsNamespace());
            }
        }
        /// <summary>The 'SecurityUserManager' subnamespace contains further subnamespaces.</summary>
        public ISecurityUserManagerNamespace UserManager
        {
            get
            {
                return (ISecurityUserManagerNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(ISecurityUserManagerNamespace),
                        new TSecurityUserManagerNamespace());
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

