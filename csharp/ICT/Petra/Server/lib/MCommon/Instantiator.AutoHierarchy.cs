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

using Ict.Petra.Shared.Interfaces.MCommon;
using Ict.Petra.Shared.Interfaces.MCommon.Cacheable;
using Ict.Petra.Shared.Interfaces.MCommon.UIConnectors;
using Ict.Petra.Shared.Interfaces.MCommon.WebConnectors;
using Ict.Petra.Shared.Interfaces.MCommon.DataReader;
using Ict.Petra.Server.MCommon.Instantiator.Cacheable;
using Ict.Petra.Server.MCommon.Instantiator.UIConnectors;
using Ict.Petra.Server.MCommon.Instantiator.WebConnectors;
using Ict.Petra.Server.MCommon.Instantiator.DataReader;
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.MCommon.UIConnectors;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MCommon.DataReader;

#region ManualCode
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon;
#endregion ManualCode
namespace Ict.Petra.Server.MCommon.Instantiator
{
    /// <summary>
    /// LOADER CLASS. Creates and dynamically exposes an instance of the remoteable
    /// class to make it callable remotely from the Client.
    /// </summary>
    public class TMCommonNamespaceLoader : TConfigurableMBRObject
    {
        /// <summary>URL at which the remoted object can be reached</summary>
        private String FRemotingURL;
        /// <summary>the remoted object</summary>
        private TMCommon FRemotedObject;

        /// <summary>
        /// Creates and dynamically exposes an instance of the remoteable TMCommon
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
                Console.WriteLine("TMCommonNamespaceLoader.GetRemotingURL in AppDomain: " + Thread.GetDomain().FriendlyName);
            }

            FRemotedObject = new TMCommon();
            FRemotingURL = TConfigurableMBRObject.BuildRandomURI("TMCommonNamespaceLoader");

            return FRemotingURL;
        }

        /// <summary>
        /// get the object to be remoted
        /// </summary>
        public TMCommon GetRemotedObject()
        {
            return FRemotedObject;
        }
    }

    /// <summary>
    /// REMOTEABLE CLASS. MCommon Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TMCommon : TConfigurableMBRObject, IMCommonNamespace
    {
        /// <summary>Constructor</summary>
        public TMCommon()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TMCommon object exists until this AppDomain is unloaded!
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
        /// <summary>The 'UIConnectors' subnamespace contains further subnamespaces.</summary>
        public IUIConnectorsNamespace UIConnectors
        {
            get
            {
                return (IUIConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IUIConnectorsNamespace),
                        new TUIConnectorsNamespace());
            }
        }
        /// <summary>The 'WebConnectors' subnamespace contains further subnamespaces.</summary>
        public IWebConnectorsNamespace WebConnectors
        {
            get
            {
                return (IWebConnectorsNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IWebConnectorsNamespace),
                        new TWebConnectorsNamespace());
            }
        }
        /// <summary>The 'DataReader' subnamespace contains further subnamespaces.</summary>
        public IDataReaderNamespace DataReader
        {
            get
            {
                return (IDataReaderNamespace) TCreateRemotableObject.CreateRemotableObject(
                        typeof(IDataReaderNamespace),
                        new TDataReaderNamespace());
            }
        }
    }
}

namespace Ict.Petra.Server.MCommon.Instantiator.Cacheable
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
            FCachePopulator = new Ict.Petra.Server.MCommon.Cacheable.TCacheable();
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
        private DataTable GetCacheableTableInternal(TCacheableCommonTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            DataTable ReturnValue = FCachePopulator.GetCacheableTable(ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheableCommonTablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheableCommonTablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

        #endregion ManualCode
        /// generated method from interface
        public System.Data.DataTable GetCacheableTable(TCacheableCommonTablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            #region ManualCode
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableCommonTablesEnum ACacheableTable)
        {
            #region ManualCode
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public void RefreshCacheableTable(TCacheableCommonTablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            #region ManualCode
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
            #endregion ManualCode
        }

        /// generated method from interface
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableCommonTablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode                                    
        }
    }
}

namespace Ict.Petra.Server.MCommon.Instantiator.UIConnectors
{
    /// <summary>
    /// REMOTEABLE CLASS. UIConnectors Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TUIConnectorsNamespace : TConfigurableMBRObject, IUIConnectorsNamespace
    {
        /// <summary>Constructor</summary>
        public TUIConnectorsNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TUIConnectorsNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                          TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            return (IDataElementsUIConnectorsOfficeSpecificDataLabels) TCreateRemotableObject.CreateRemotableObject(
                    typeof(IDataElementsUIConnectorsOfficeSpecificDataLabels),
                    new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AOfficeSpecificDataLabelUse));
        }

        /// generated method from interface
        public IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                          TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                          ref OfficeSpecificDataLabelsTDS ADataSet,
                                                                                          TDBTransaction AReadTransaction)
        {
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AOfficeSpecificDataLabelUse);

            ADataSet = ReturnValue.GetData(AReadTransaction);
            return ReturnValue;
        }

        /// generated method from interface
        public IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                          Int32 AApplicationKey,
                                                                                          Int64 ARegistrationOffice,
                                                                                          TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse)
        {
            return (IDataElementsUIConnectorsOfficeSpecificDataLabels) TCreateRemotableObject.CreateRemotableObject(
                    typeof(IDataElementsUIConnectorsOfficeSpecificDataLabels),
                    new TOfficeSpecificDataLabelsUIConnector(APartnerKey, AApplicationKey, ARegistrationOffice, AOfficeSpecificDataLabelUse));
        }

        /// generated method from interface
        public IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(Int64 APartnerKey,
                                                                                          Int32 AApplicationKey,
                                                                                          Int64 ARegistrationOffice,
                                                                                          TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                          ref OfficeSpecificDataLabelsTDS ADataSet,
                                                                                          TDBTransaction AReadTransaction)
        {
            TOfficeSpecificDataLabelsUIConnector ReturnValue = new TOfficeSpecificDataLabelsUIConnector(APartnerKey,
               AApplicationKey,
               ARegistrationOffice,
               AOfficeSpecificDataLabelUse);

            ADataSet = ReturnValue.GetData(AReadTransaction);
            return ReturnValue;
        }

        /// generated method from interface
        public IPartnerUIConnectorsFieldOfService FieldOfService(Int64 APartnerKey)
        {
            return (IPartnerUIConnectorsFieldOfService) TCreateRemotableObject.CreateRemotableObject(
                    typeof(IPartnerUIConnectorsFieldOfService),
                    new TFieldOfServiceUIConnector(APartnerKey));
        }

        /// generated method from interface
        public IPartnerUIConnectorsFieldOfService FieldOfService(Int64 APartnerKey,
                                                                 ref FieldOfServiceTDS ADataSet)
        {
            TFieldOfServiceUIConnector ReturnValue = new TFieldOfServiceUIConnector(APartnerKey);

            ADataSet = ReturnValue.GetData();
            return ReturnValue;
        }
    }
}

namespace Ict.Petra.Server.MCommon.Instantiator.WebConnectors
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
        public System.Boolean GetCurrentState(out System.String ACaption,
                                              out System.String AStatusMessage,
                                              out System.Int32 APercentageDone,
                                              out System.Boolean AJobFinished)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MCommon.WebConnectors.TProgressTrackerWebConnector), "GetCurrentState", ";STRING;STRING;INT;BOOL;");
            return Ict.Petra.Server.MCommon.WebConnectors.TProgressTrackerWebConnector.GetCurrentState(out ACaption, out AStatusMessage, out APercentageDone, out AJobFinished);
        }

        /// generated method from connector
        public System.Boolean CancelJob()
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MCommon.WebConnectors.TProgressTrackerWebConnector), "CancelJob", ";");
            return Ict.Petra.Server.MCommon.WebConnectors.TProgressTrackerWebConnector.CancelJob();
        }

        /// generated method from connector
        public Int64 GetNextSequence(TSequenceNames ASequence)
        {
            TModuleAccessManager.CheckUserPermissionsForMethod(typeof(Ict.Petra.Server.MCommon.WebConnectors.TSequenceWebConnector), "GetNextSequence", ";TSEQUENCENAMES;");
            return Ict.Petra.Server.MCommon.WebConnectors.TSequenceWebConnector.GetNextSequence(ASequence);
        }
    }
}

namespace Ict.Petra.Server.MCommon.Instantiator.DataReader
{
    /// <summary>
    /// REMOTEABLE CLASS. DataReader Namespace (highest level).
    /// </summary>
    /// <summary>auto generated class </summary>
    public class TDataReaderNamespace : TConfigurableMBRObject, IDataReaderNamespace
    {
        /// <summary>Constructor</summary>
        public TDataReaderNamespace()
        {
        }

        /// NOTE AutoGeneration: This function is all-important!!!
        public override object InitializeLifetimeService()
        {
            return null; // make sure that the TDataReaderNamespace object exists until this AppDomain is unloaded!
        }

        /// generated method from interface
        public System.Boolean GetData(System.String ATablename,
                                      TSearchCriteria[] ASearchCriteria,
                                      out Ict.Common.Data.TTypedDataTable AResultTable)
        {
            #region ManualCode
            return TCommonDataReader.GetData(ATablename, ASearchCriteria, out AResultTable);
            #endregion ManualCode            
        }

        /// generated method from interface
        public TSubmitChangesResult SaveData(System.String ATablename,
                                             ref TTypedDataTable ASubmitTable,
                                             out TVerificationResultCollection AVerificationResult)
        {
            #region ManualCode
            return TCommonDataReader.SaveData(ATablename, ref ASubmitTable, out AVerificationResult);
            #endregion ManualCode
        }
    }
}

