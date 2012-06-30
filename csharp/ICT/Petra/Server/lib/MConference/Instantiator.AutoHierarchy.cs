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

using Ict.Petra.Shared.Interfaces.MConference;
using Ict.Petra.Shared.Interfaces.MConference.Cacheable;
using Ict.Petra.Shared.Interfaces.MConference.WebConnectors;
using Ict.Petra.Server.MConference.Instantiator.Cacheable;
using Ict.Petra.Server.MConference.Instantiator.WebConnectors;
using Ict.Petra.Server.MConference.Cacheable;
using Ict.Petra.Server.MConference.WebConnectors;

#region ManualCode
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MConference;
#endregion ManualCode
namespace Ict.Petra.Server.MConference.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMConferenceNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMConference FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMConference
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
                Console.WriteLine("TMConferenceNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMConference();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMConferenceNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMConference GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MConference Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMConference : TConfigurableMBRObject, IMConferenceNamespace
    {
        private TCacheableNamespaceRemote FCacheableSubNamespace;
        private TWebConnectorsNamespaceRemote FWebConnectorsSubNamespace;

        /// <summary>Constructor</summary>
        public TMConference()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMConference object exists until this AppDomain is unloaded!
        }

        /// <summary>serializable, which means that this object is executed on the client side</summary>
        [Serializable]
        public class TCacheableNamespaceRemote: ICacheableNamespace
        {
            private ICacheableNamespace RemoteObject = null;
            private string FObjectURI;

            /// <summary>constructor. get remote object</summary>
            public TCacheableNamespaceRemote(string AObjectURI)
            {
                FObjectURI = AObjectURI;
            }

            private void InitRemoteObject()
            {
                RemoteObject = (ICacheableNamespace)TConnector.TheConnector.GetRemoteObject(FObjectURI, typeof(ICacheableNamespace));
            }

            /// generated method from interface
            public System.Data.DataTable GetCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
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
            public System.Data.DataTable GetCacheableTablea(TCacheableConferenceTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetCacheableTablea(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableConferenceTablesEnum ACacheableTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable);
            }
            /// generated method from interface
            public void RefreshCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
                                              out System.Data.DataTable ADataTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                RemoteObject.RefreshCacheableTable(ACacheableTable,out ADataTable);
            }
            /// generated method from interface
            public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
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

        /// <summary>The 'Cacheable' subnamespace contains further subnamespaces.</summary>
        public ICacheableNamespace Cacheable
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MConference.Cacheable' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MConference.Cacheable' sub-namespace
                //

                // accessing TCacheableNamespace the first time? > instantiate the object
                if (FCacheableSubNamespace == null)
                {
                    // need to calculate the URI for this object and pass it to the new namespace object
                    string ObjectURI = TConfigurableMBRObject.BuildRandomURI("TCacheableNamespace");
                    TCacheableNamespace ObjectToRemote = new TCacheableNamespace();

                    // we need to add the service in the main domain
                    DomainManagerBase.UClientManagerCallForwarderRef.AddCrossDomainService(
                        DomainManagerBase.GClientID.ToString(), ObjectURI, ObjectToRemote);

                    FCacheableSubNamespace = new TCacheableNamespaceRemote(ObjectURI);
                }

                return FCacheableSubNamespace;
            }

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
            public PUnitTable GetOutreachOptions(Int64 AUnitKey)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetOutreachOptions(AUnitKey);
            }
            /// generated method from interface
            public SelectConferenceTDS GetConferences(String AConferenceName,
                                                      String APrefix)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetConferences(AConferenceName,APrefix);
            }
            /// generated method from interface
            public System.Boolean GetEarliestAndLatestDate(Int64 AConferenceKey,
                                                           out DateTime AEarliestArrivalDate,
                                                           out DateTime ALatestDepartureDate,
                                                           out DateTime AStartDate,
                                                           out DateTime AEndDate)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetEarliestAndLatestDate(AConferenceKey,out AEarliestArrivalDate,out ALatestDepartureDate,out AStartDate,out AEndDate);
            }
            /// generated method from interface
            public System.Boolean GetOutreachOptions(System.Int64 AUnitKey,
                                                     out System.Data.DataTable AConferenceTable)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetOutreachOptions(AUnitKey,out AConferenceTable);
            }
            /// generated method from interface
            public System.Boolean GetFieldUnits(Int64 AConferenceKey,
                                                TUnitTypeEnum AFieldTypes,
                                                out DataTable AFieldsTable,
                                                out String AConferencePrefix)
            {
                if (RemoteObject == null)
                {
                    InitRemoteObject();
                }

                return RemoteObject.GetFieldUnits(AConferenceKey,AFieldTypes,out AFieldsTable,out AConferencePrefix);
            }
        }

        /// <summary>The 'WebConnectors' subnamespace contains further subnamespaces.</summary>
        public IWebConnectorsNamespace WebConnectors
        {
            get
            {
                //
                // Creates or passes a reference to an instantiator of sub-namespaces that
                // reside in the 'MConference.WebConnectors' sub-namespace.
                // A call to this function is done everytime a Client uses an object of this
                // sub-namespace - this is fully transparent to the Client.
                //
                // @return A reference to an instantiator of sub-namespaces that reside in
                //         the 'MConference.WebConnectors' sub-namespace
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
    }
}

namespace Ict.Petra.Server.MConference.Instantiator.Cacheable
{
    /// <summary>
    /// REMOTEABLE CLASS. Cacheable Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TCacheableNamespace : TConfigurableMBRObject, ICacheableNamespace
    {
        #region ManualCode
        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private TCacheable FCachePopulator;
        #endregion ManualCode

        /// <summary>Constructor</summary>
        public TCacheableNamespace()
        {
            #region ManualCode
            FCachePopulator = new Ict.Petra.Server.MConference.Cacheable.TCacheable();
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
        private DataTable GetCacheableTableInternal(TCacheableConferenceTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableConferenceTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableConferenceTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public System.Data.DataTable GetCacheableTablea(TCacheableConferenceTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            return GetCacheableTable(ACacheableTable, "", out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableConferenceTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableConferenceTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                                    
        }
    }
}

namespace Ict.Petra.Server.MConference.Instantiator.WebConnectors
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
        public PUnitTable GetOutreachOptions(Int64 AUnitKey)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions), "GetOutreachOptions", ";LONG;");
            return Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions.GetOutreachOptions(AUnitKey);
        }

        /// generated method from connector
        public SelectConferenceTDS GetConferences(String AConferenceName,
                                                  String APrefix)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions), "GetConferences", ";STRING;STRING;");
            return Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions.GetConferences(AConferenceName, APrefix);
        }

        /// generated method from connector
        public System.Boolean GetEarliestAndLatestDate(Int64 AConferenceKey,
                                                       out DateTime AEarliestArrivalDate,
                                                       out DateTime ALatestDepartureDate,
                                                       out DateTime AStartDate,
                                                       out DateTime AEndDate)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions), "GetEarliestAndLatestDate", ";LONG;DATETIME;DATETIME;DATETIME;DATETIME;");
            return Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions.GetEarliestAndLatestDate(AConferenceKey, out AEarliestArrivalDate, out ALatestDepartureDate, out AStartDate, out AEndDate);
        }

        /// generated method from connector
        public System.Boolean GetOutreachOptions(System.Int64 AUnitKey,
                                                 out System.Data.DataTable AConferenceTable)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions), "GetOutreachOptions", ";LONG;DATATABLE;");
            return Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions.GetOutreachOptions(AUnitKey, out AConferenceTable);
        }

        /// generated method from connector
        public System.Boolean GetFieldUnits(Int64 AConferenceKey,
                                            TUnitTypeEnum AFieldTypes,
                                            out DataTable AFieldsTable,
                                            out String AConferencePrefix)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions), "GetFieldUnits", ";LONG;TUNITTYPEENUM;DATATABLE;STRING;");
            return Ict.Petra.Server.MConference.WebConnectors.TConferenceOptions.GetFieldUnits(AConferenceKey, AFieldTypes, out AFieldsTable, out AConferencePrefix);
        }
    }
}

