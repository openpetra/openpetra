//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2014 by OM International
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
using System.Data;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Server.MCommon.DataReader.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.MConference.Cacheable;
using Ict.Petra.Server.MFinance.Cacheable;
using Ict.Petra.Server.MPartner.Mailing.Cacheable;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
using Ict.Petra.Server.MPartner.Subscriptions.Cacheable;
using Ict.Petra.Server.MPersonnel.Person.Cacheable;
using Ict.Petra.Server.MPersonnel.Unit.Cacheable;
using Ict.Petra.Server.MSysMan.Cacheable;
using Ict.Petra.Server.MPartner;

using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.ICH.WebConnectors;
using Ict.Petra.Server.MReporting.WebConnectors;
using Ict.Petra.Server.MFinance.Common;

namespace Ict.Petra.Server.App.Delegates
{
    /// <summary>
    /// Sets up Delegates that allow arbitrary code to be called in various server-side
    /// DLL's, avoiding 'circular dependencies' between DLL's that need to call Methods in
    /// other DLL's (which would also reference the DLL that the call would originate from).
    /// </summary>
    public class TSetupDelegates
    {
        private static Ict.Petra.Server.MCommon.Cacheable.TCacheable CachePopulatorCommon;
        private static Ict.Petra.Server.MConference.Cacheable.TCacheable CachePopulatorConference;
        private static Ict.Petra.Server.MFinance.Cacheable.TCacheable CachePopulatorFinance;
        private static Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable CachePopulatorMailing;
        private static Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable CachePopulatorPartner;
        private static Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable CachePopulatorSubscriptions;
        private static Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable CachePopulatorPersonnel;
        private static Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable CachePopulatorUnits;
        private static Ict.Petra.Server.MSysMan.Cacheable.TCacheable CachePopulatorSysMan;

        /// <summary>
        /// init the static variables
        /// </summary>
        public static void Init()
        {
            // Set up Error Codes and Data Validation Delegates for a Client's AppDomain.
            // This setting-up makes use of the fact that this Method is called only once,
            // namely directly after the Client logged in successfully.
            ErrorCodeInventory.RegisteredTypes.Add(new Ict.Petra.Shared.PetraErrorCodes().GetType());

            TSharedValidationHelper.SharedGetDataDelegate = @TCommonDataReader.GetData;
            TSharedPartnerValidationHelper.VerifyPartnerDelegate = @TPartnerServerLookups.VerifyPartner;
            TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookups.GetCurrentPostingRangeDates;
            TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriodDelegate = @TAccountingPeriodsWebConnector.GetFirstDayOfAccountingPeriod;
            TMonthEnd.StewardshipCalculationDelegate = @TStewardshipCalculationWebConnector.PerformStewardshipCalculation;
            TGLPosting.PrintReportOnClientDelegate = @TReportingWebConnector.GenerateReportOnClient;
            TIntranetExportWebConnector.GetPrimaryEmailAndPrimaryPhoneDelegate = @TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhone;
            TIntranetExportWebConnector.GetWithinOrganisationEmailDelegate = @TContactDetailsAggregate.GetWithinOrganisationEmailAddress;

            // Set up Delegates for retrieval of cacheable tables when called from Shared directories on server side
            CachePopulatorCommon = new Ict.Petra.Server.MCommon.Cacheable.TCacheable();
            CachePopulatorConference = new Ict.Petra.Server.MConference.Cacheable.TCacheable();
            CachePopulatorFinance = new Ict.Petra.Server.MFinance.Cacheable.TCacheable();
            CachePopulatorMailing = new Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable();
            CachePopulatorPartner = new Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable();
            CachePopulatorSubscriptions = new Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable();
            CachePopulatorPersonnel = new Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable();
            CachePopulatorUnits = new Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable();
            CachePopulatorSysMan = new Ict.Petra.Server.MSysMan.Cacheable.TCacheable();

            TSharedDataCache.TMCommon.GetCacheableCommonTableDelegate = @CachePopulatorCommon.GetCacheableTable;

            TSharedDataCache.TMFinance.GetCacheableFinanceTableDelegate = @CachePopulatorFinance.GetCacheableTable;

            TSharedDataCache.TMPartner.GetCacheablePartnerTableDelegate = @CachePopulatorPartner.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableMailingTableDelegate = @CachePopulatorMailing.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableSubscriptionsTableDelegate = @CachePopulatorSubscriptions.GetCacheableTable;
            TSharedDataCache.TMPartner.GetPartnerCalculationsSystemCategoryAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DetermineSystemCategoryAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsPartnerContactDetailAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerContactDetailAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsEmailPartnerAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DetermineEmailPartnerAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsPhonePartnerAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DeterminePhonePartnerAttributeTypes;

            TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate = @CachePopulatorPersonnel.GetCacheableTable;
            TSharedDataCache.TMPersonnel.GetCacheableUnitsTableDelegate = @CachePopulatorUnits.GetCacheableTable;

            TSharedDataCache.TMConference.GetCacheableConferenceTableDelegate = @CachePopulatorConference.GetCacheableTable;

            TSharedDataCache.TMSysMan.GetCacheableSysManTableDelegate = @CachePopulatorSysMan.GetCacheableTable;
        }
    }
}
