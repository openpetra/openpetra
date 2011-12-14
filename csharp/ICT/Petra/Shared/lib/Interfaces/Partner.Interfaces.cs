// Auto generated with nant generateGlue, based on csharp\ICT\Petra\Definitions\NamespaceHierarchy.yml
// and the Server c# files (eg. UIConnector implementations)
// Do not change this file manually.
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ict.Common;
using Ict.Common.Verification;
#region ManualCode
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using System.Collections.Specialized;
#endregion ManualCode
using Ict.Common.Remoting.Shared;
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
namespace Ict.Petra.Shared.Interfaces.MPartner
{
    /// <summary>auto generated</summary>
    public interface IMPartnerNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IExtractsNamespace Extracts
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IImportExportNamespace ImportExport
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IMailingNamespace Mailing
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerNamespace Partner
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerMergeNamespace PartnerMerge
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ISubscriptionsNamespace Subscriptions
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ITableMaintenanceNamespace TableMaintenance
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Extracts
{
    /// <summary>auto generated</summary>
    public interface IExtractsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IExtractsUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IExtractsWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Extracts.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IExtractsUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Extracts.UIConnectors.TExtractsAddSubscriptionsUIConnector)</summary>
        IPartnerUIConnectorsExtractsAddSubscriptions ExtractsAddSubscriptions(System.Int32 AExtractID);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Extracts.UIConnectors.TPartnerNewExtractUIConnector)</summary>
        IPartnerUIConnectorsPartnerNewExtract PartnerNewExtract();
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsExtractsAddSubscriptions : IInterface
    {
        /// <summary>auto generated from Connector property (Ict.Petra.Server.MPartner.Extracts.UIConnectors.TExtractsAddSubscriptionsUIConnector)</summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Extracts.UIConnectors.TExtractsAddSubscriptionsUIConnector)</summary>
        void SubmitChangesAsync(PSubscriptionTable AInspectDT);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Extracts.UIConnectors.TExtractsAddSubscriptionsUIConnector)</summary>
        void SubmitChangesAsyncResult(out DataTable AResponseDT,
                                      out TVerificationResultCollection AVerificationResult,
                                      out TSubmitChangesResult ASubmitChangesResult);
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsPartnerNewExtract : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Extracts.UIConnectors.TPartnerNewExtractUIConnector)</summary>
        System.Boolean CreateNewExtract(String AExtractName,
                                        String AExtractDescription,
                                        out Int32 AExtractID,
                                        out Boolean AExtractAlreadyExists,
                                        out TVerificationResultCollection AVerificationResults);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Extracts.UIConnectors.TPartnerNewExtractUIConnector)</summary>
        void DeleteExtractAgain();
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Extracts.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IExtractsWebConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.ImportExport
{
    /// <summary>auto generated</summary>
    public interface IImportExportNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IImportExportUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IImportExportWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.ImportExport.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IImportExportUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.ImportExport.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IImportExportWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        PartnerImportExportTDS ImportPartnersFromYml(System.String AXmlPartnerData,
                                                     out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        PartnerImportExportTDS ImportFromCSVFile(System.String AXmlPartnerData,
                                                 out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        PartnerImportExportTDS ImportFromPartnerExtract(System.String[] ATextFileLines,
                                                        out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        Boolean CommitChanges(PartnerImportExportTDS MainDS,
                              out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        System.String ExportPartners();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        System.String GetExtFileHeader();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        System.String GetExtFileFooter();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        System.String ExportPartnerExt(Int64 APartnerKey,
                                       Int32 ASiteKey,
                                       Int32 ALocationKey,
                                       Boolean ANoFamily,
                                       StringCollection ASpecificBuildingInfo);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.ImportExport.WebConnectors.TImportExportWebConnector)</summary>
        Boolean ImportDataExt(System.String[] ALinesToImport,
                              System.String ALimitToOption,
                              System.Boolean ADoNotOverwrite,
                              out TVerificationResultCollection AResultList);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Mailing
{
    /// <summary>auto generated</summary>
    public interface IMailingNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IMailingCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IMailingUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IMailingWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Mailing.Cacheable
{
    /// <summary>auto generated</summary>
    public interface IMailingCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable.Class)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Mailing.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableMailingTablesEnum ACacheableTable,
                                   out System.Data.DataTable ADataTable);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Mailing.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IMailingUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Mailing.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IMailingWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector)</summary>
        System.Boolean GetBestAddress(Int64 APartnerKey,
                                      out PLocationTable AAddress,
                                      out PPartnerLocationTable APartnerLocation,
                                      out System.String ACountryNameLocal,
                                      out System.String AEmailAddress);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector)</summary>
        BestAddressTDSLocationTable AddPostalAddress(ref DataTable APartnerTable,
                                                     DataColumn APartnerKeyColumn,
                                                     System.Boolean AIgnoreForeignAddresses);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Mailing.WebConnectors.TAddressWebConnector)</summary>
        System.Boolean CreateExtractFromBestAddressTable(String AExtractName,
                                                         String AExtractDescription,
                                                         out Int32 ANewExtractId,
                                                         out Boolean AExtractAlreadyExists,
                                                         out TVerificationResultCollection AVerificationResults,
                                                         BestAddressTDSLocationTable ABestAddressTable,
                                                         System.Boolean AIncludeNonValidAddresses);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner
{
    /// <summary>auto generated</summary>
    public interface IPartnerNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPartnerCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerDataElementsNamespace DataElements
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerServerLookupsNamespace ServerLookups
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        IPartnerWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.Cacheable
{
    /// <summary>auto generated</summary>
    public interface IPartnerCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable.Class)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheablePartnerTablesEnum ACacheableTable,
                                   out System.Data.DataTable ADataTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.Cacheable.Class)</summary>
        TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheablePartnerTablesEnum ACacheableTable,
                                                               ref TTypedDataTable ASubmitTable,
                                                               out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.DataElements
{
    /// <summary>auto generated</summary>
    public interface IPartnerDataElementsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPartnerDataElementsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.DataElements.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPartnerDataElementsUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.DataElements.UIConnectors.Class)</summary>
        Ict.Petra.Shared.Interfaces.MCommon.UIConnectors.IDataElementsUIConnectorsOfficeSpecificDataLabels OfficeSpecificDataLabels(System.Int64 APartnerKey,
                                                                                                                                    Ict.Petra.Shared.MCommon.TOfficeSpecificDataLabelUseEnum AOfficeSpecificDataLabelUse,
                                                                                                                                    out Ict.Petra.Shared.MCommon.Data.OfficeSpecificDataLabelsTDS AOfficeSpecificDataLabelsDataSet);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.ServerLookups
{
    /// <summary>auto generated</summary>
    public interface IPartnerServerLookupsNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean GetPartnerShortName(Int64 APartnerKey,
                                    out String APartnerShortName,
                                    out TPartnerClass APartnerClass,
                                    Boolean AMergedPartners);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean GetPartnerShortName(Int64 APartnerKey,
                                    out String APartnerShortName,
                                    out TPartnerClass APartnerClass);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean VerifyPartner(Int64 APartnerKey,
                              TPartnerClass[] AValidPartnerClasses,
                              out String APartnerShortName,
                              out TPartnerClass APartnerClass,
                              out Boolean AIsMergedPartner);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean VerifyPartner(Int64 APartnerKey,
                              out String APartnerShortName,
                              out TPartnerClass APartnerClass,
                              out Boolean AIsMergedPartner,
                              out Boolean AUserCanAccessPartner);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean MergedPartnerDetails(Int64 AMergedPartnerPartnerKey,
                                     out String AMergedPartnerPartnerShortName,
                                     out TPartnerClass AMergedPartnerPartnerClass,
                                     out Int64 AMergedIntoPartnerKey,
                                     out String AMergedIntoPartnerShortName,
                                     out TPartnerClass AMergedIntoPartnerClass,
                                     out String AMergedBy,
                                     out DateTime AMergeDate);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean PartnerInfo(Int64 APartnerKey,
                            TPartnerInfoScopeEnum APartnerInfoScope,
                            out PartnerInfoTDS APartnerInfoDS);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean PartnerInfo(Int64 APartnerKey,
                            TLocationPK ALocationKey,
                            TPartnerInfoScopeEnum APartnerInfoScope,
                            out PartnerInfoTDS APartnerInfoDS);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean GetExtractDescription(String AExtractName,
                                      out String AExtractDescription);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean GetPartnerFoundationStatus(Int64 APartnerKey,
                                           out Boolean AIsFoundation);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Boolean GetRecentlyUsedPartners(System.Int32 AMaxPartnersCount,
                                        ArrayList APartnerClasses,
                                        out Dictionary<System.Int64, System.String>ARecentlyUsedPartners);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Partner.ServerLookups.Class)</summary>
        Int64 GetFamilyKeyForPerson(Int64 APersonKey);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsNamespace : IInterface
    {
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit(System.Int64 APartnerKey,
                                                    ref PartnerEditTDS ADataSet,
                                                    Boolean ADelayedDataLoading,
                                                    TPartnerEditTabPageEnum ATabPage);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                    Int64 ASiteKey,
                                                    Int32 ALocationKey);
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit(Int64 APartnerKey,
                                                    Int64 ASiteKey,
                                                    Int32 ALocationKey,
                                                    ref PartnerEditTDS ADataSet,
                                                    Boolean ADelayedDataLoading,
                                                    TPartnerEditTabPageEnum ATabPage);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit();
        /// <summary>auto generated from Connector constructor and GetData (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IPartnerUIConnectorsPartnerEdit PartnerEdit(ref PartnerEditTDS ADataSet,
                                                    Boolean ADelayedDataLoading,
                                                    TPartnerEditTabPageEnum ATabPage);
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        IPartnerUIConnectorsPartnerFind PartnerFind();
        /// <summary>auto generated from Connector constructor (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerLocationFindUIConnector)</summary>
        IPartnerUIConnectorsPartnerLocationFind PartnerLocationFind(DataTable ACriteriaData);
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsPartnerEdit : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetBankingDetails(System.Int64 APartnerKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetData(Boolean ADelayedDataLoading,
                               TPartnerEditTabPageEnum ATabPage);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetData(Boolean ADelayedDataLoading);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetData();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetDataAddresses();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetDataFoundation(Boolean ABaseTableOnly);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PPartnerInterestTable GetDataPartnerInterests();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PInterestTable GetDataInterests();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PLocationTable GetDataLocation(Int64 ASiteKey,
                                       Int32 ALocationKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDS GetDataNewPartner(System.Int64 ASiteKey,
                                         System.Int64 APartnerKey,
                                         TPartnerClass APartnerClass,
                                         String ADesiredCountryCode,
                                         String AAcquisitionCode,
                                         Boolean APrivatePartner,
                                         Int64 AFamilyPartnerKey,
                                         Int64 AFamilySiteKey,
                                         Int32 AFamilyLocationKey,
                                         out String ASiteCountryCode);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        IndividualDataTDS GetDataPersonnelIndividualData(TIndividualDataItemEnum AIndividualDataItem);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PPartnerTypeTable GetDataPartnerTypes();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PSubscriptionTable GetDataSubscriptions();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDSPPartnerRelationshipTable GetDataPartnerRelationships();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PDataLabelValuePartnerTable GetDataLocalPartnerDataValues();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        PartnerEditTDSFamilyMembersTable GetDataFamilyMembers(Int64 AFamilyPartnerKey,
                                                              String AWorkWithSpecialType);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        TSubmitChangesResult SubmitChanges(ref PartnerEditTDS AInspectDS,
                                           ref DataSet AResponseDS,
                                           out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        TSubmitChangesResult SubmitChangesContinue(out PartnerEditTDS AInspectDS,
                                                   ref DataSet AResponseDS,
                                                   out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        Int64 GetPartnerKeyForNewPartner(System.Int64 AFieldPartnerKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        Boolean GetPartnerShortName(Int64 APartnerKey,
                                    out String APartnerShortName,
                                    out TPartnerClass APartnerClass);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        Boolean HasPartnerCostCentreLink(out String ACostCentreCode);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        Boolean HasPartnerCostCentreLink(System.Int64 APartnerKey,
                                         out String ACostCentreCode);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        Boolean HasPartnerLocationOtherPartnerReferences(Int64 ASiteKey,
                                                         Int32 ALocationKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerEditUIConnector)</summary>
        System.Boolean SubmitPartnerKeyForNewPartner(System.Int64 AFieldPartnerKey,
                                                     System.Int64 AOriginalDefaultKey,
                                                     ref System.Int64 ANewPartnerKey);
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsPartnerFind : IInterface
    {
        /// <summary>auto generated from Connector property (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        void PerformSearch(DataTable ACriteriaData,
                           System.Boolean ADetailedResults);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        DataTable GetDataPagedResult(System.Int16 APage,
                                     System.Int16 APageSize,
                                     out System.Int32 ATotalRecords,
                                     out System.Int16 ATotalPages);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        void StopSearch(System.Object ASender,
                        EventArgs AArgs);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector)</summary>
        Int32 AddAllFoundPartnersToExtract(System.Int32 AExtractID,
                                           out TVerificationResultCollection AVerificationResult);
    }

    /// <summary>auto generated</summary>
    public interface IPartnerUIConnectorsPartnerLocationFind : IInterface
    {
        /// <summary>auto generated from Connector property (Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerLocationFindUIConnector)</summary>
        IAsynchronousExecutionProgress AsyncExecProgress
        {
            get;
        }

        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerLocationFindUIConnector)</summary>
        DataTable GetDataPagedResult(System.Int16 APage,
                                     System.Int16 APageSize,
                                     out System.Int32 ATotalRecords,
                                     out System.Int16 ATotalPages);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Partner.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface IPartnerWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector)</summary>
        System.Boolean AddContact(List<Int64> APartnerKeys,
                                  DateTime AContactDate,
                                  System.String AMethodOfContact,
                                  System.String AComment,
                                  System.String AModuleID,
                                  System.String AMailingCode,
                                  out TVerificationResultCollection AVerificationResults);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector)</summary>
        PPartnerContactTable FindContacts(System.String AContactor,
                                          System.Nullable<DateTime> AContactDate,
                                          System.String ACommentContains,
                                          System.String AMethodOfContact,
                                          System.String AModuleID,
                                          System.String AMailingCode);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TContactsWebConnector)</summary>
        System.Boolean DeleteContacts(PPartnerContactTable APartnerContacts,
                                      out TVerificationResultCollection AVerificationResults);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TPartnerWebConnector)</summary>
        System.Boolean ChangeFamily(Int64 APersonKey,
                                    Int64 AOldFamilyKey,
                                    Int64 ANewFamilyKey,
                                    out String AProblemMessage,
                                    out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector)</summary>
        Int64 NewPartnerKey(Int64 AFieldPartnerKey);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector)</summary>
        PartnerEditTDS GetPartnerDetails(Int64 APartnerKey,
                                         System.Boolean AWithAddressDetails,
                                         System.Boolean AWithSubscriptions,
                                         System.Boolean AWithRelationships);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerEditWebConnector)</summary>
        System.Boolean SavePartner(PartnerEditTDS AMainDS,
                                   out TVerificationResultCollection AVerificationResult);
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.Partner.WebConnectors.TSimplePartnerFindWebConnector)</summary>
        PartnerFindTDS FindPartners(System.String AFirstName,
                                    System.String AFamilyNameOrOrganisation,
                                    System.String ACity,
                                    StringCollection APartnerClasses);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.PartnerMerge
{
    /// <summary>auto generated</summary>
    public interface IPartnerMergeNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        IPartnerMergeUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.PartnerMerge.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface IPartnerMergeUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Subscriptions
{
    /// <summary>auto generated</summary>
    public interface ISubscriptionsNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ISubscriptionsCacheableNamespace Cacheable
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ISubscriptionsUIConnectorsNamespace UIConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Subscriptions.Cacheable
{
    /// <summary>auto generated</summary>
    public interface ISubscriptionsCacheableNamespace : IInterface
    {
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable.Class)</summary>
        System.Data.DataTable GetCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable,
                                                System.String AHashCode,
                                                out System.Type AType);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable.Class)</summary>
        void RefreshCacheableTable(Ict.Petra.Shared.MPartner.TCacheableSubscriptionsTablesEnum ACacheableTable,
                                   out System.Data.DataTable ADataTable);
        /// <summary>auto generated from Instantiator (Ict.Petra.Server.MPartner.Instantiator.Subscriptions.Cacheable.Class)</summary>
        TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableSubscriptionsTablesEnum ACacheableTable,
                                                               ref TTypedDataTable ASubmitTable,
                                                               out TVerificationResultCollection AVerificationResult);
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.Subscriptions.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface ISubscriptionsUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance
{
    /// <summary>auto generated</summary>
    public interface ITableMaintenanceNamespace : IInterface
    {
        /// <summary>access to sub namespace</summary>
        ITableMaintenanceUIConnectorsNamespace UIConnectors
        {
            get;
        }

        /// <summary>access to sub namespace</summary>
        ITableMaintenanceWebConnectorsNamespace WebConnectors
        {
            get;
        }

    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance.UIConnectors
{
    /// <summary>auto generated</summary>
    public interface ITableMaintenanceUIConnectorsNamespace : IInterface
    {
    }

}


namespace Ict.Petra.Shared.Interfaces.MPartner.TableMaintenance.WebConnectors
{
    /// <summary>auto generated</summary>
    public interface ITableMaintenanceWebConnectorsNamespace : IInterface
    {
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector)</summary>
        PartnerSetupTDS LoadPartnerTypes();
        /// <summary> auto generated from Connector method(Ict.Petra.Server.MPartner.TableMaintenance.WebConnectors.TPartnerSetupWebConnector)</summary>
        TSubmitChangesResult SavePartnerMaintenanceTables(ref PartnerSetupTDS AInspectDS,
                                                          out TVerificationResultCollection AVerificationResult);
    }

}

