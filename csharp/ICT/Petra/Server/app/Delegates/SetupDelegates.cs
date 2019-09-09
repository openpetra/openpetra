//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Server.MSysMan.Application.WebConnectors;
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

using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Server.MFinance.ICH.WebConnectors;
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
        private static Ict.Petra.Server.MCommon.Cacheable.TCacheable CachePopulatorCommon; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MConference.Cacheable.TCacheable CachePopulatorConference; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MFinance.Cacheable.TCacheable CachePopulatorFinance; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable CachePopulatorMailing; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable CachePopulatorPartner; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable CachePopulatorSubscriptions; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable CachePopulatorPersonnel; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable CachePopulatorUnits; // STATIC_OK: will be set for each request
        private static Ict.Petra.Server.MSysMan.Cacheable.TCacheable CachePopulatorSysMan; // STATIC_OK: will be set for each request

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
            TSharedPartnerValidationHelper.PartnerHasActiveStatusDelegate = @TPartnerServerLookups.PartnerHasActiveStatus;
            TSharedPartnerValidationHelper.PartnerIsLinkedToCCDelegate = @TPartnerServerLookups.PartnerIsLinkedToCC;
            TSharedPartnerValidationHelper.PartnerOfTypeCCIsLinkedDelegate = @TPartnerServerLookups.PartnerOfTypeCCIsLinked;
            TSharedPartnerValidationHelper.PartnerHasCurrentGiftDestinationDelegate = @TPartnerServerLookups.PartnerHasCurrentGiftDestination;
            TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookupWebConnector.GetCurrentPostingRangeDates;
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;
            TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriodDelegate = @TAccountingPeriodsWebConnector.GetFirstDayOfAccountingPeriod;
            TMonthEnd.StewardshipCalculationDelegate = @TStewardshipCalculationWebConnector.PerformStewardshipCalculation;

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

            Ict.Petra.Server.MCommon.Cacheable.TCacheable.Init();
            Ict.Petra.Server.MConference.Cacheable.TCacheable.Init();
            Ict.Petra.Server.MFinance.Cacheable.TCacheable.Init();
            Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable.Init();
            Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable.Init();
            Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable.Init();
            Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable.Init();
            Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable.Init();
            Ict.Petra.Server.MSysMan.Cacheable.TCacheable.Init();

            TSharedDataCache.TMCommon.GetCacheableCommonTableDelegate = @CachePopulatorCommon.GetCacheableTable;
            TSharedDataCache.TMFinance.GetCacheableFinanceTableDelegate = @CachePopulatorFinance.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheablePartnerTableDelegate = @CachePopulatorPartner.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableMailingTableDelegate = @CachePopulatorMailing.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableSubscriptionsTableDelegate = @CachePopulatorSubscriptions.GetCacheableTable;
            TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate = @CachePopulatorPersonnel.GetCacheableTable;
            TSharedDataCache.TMPersonnel.GetCacheableUnitsTableDelegate = @CachePopulatorUnits.GetCacheableTable;
            TSharedDataCache.TMConference.GetCacheableConferenceTableDelegate = @CachePopulatorConference.GetCacheableTable;
            TSharedDataCache.TMSysMan.GetCacheableSysManTableDelegate = @CachePopulatorSysMan.GetCacheableTable;

            TSharedDataCache.TMPartner.GetPartnerCalculationsSystemCategoryAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DetermineSystemCategoryAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsPartnerContactDetailAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerContactDetailAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsEmailPartnerAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DetermineEmailPartnerAttributeTypes;
            TSharedDataCache.TMPartner.GetPartnerCalculationsPhonePartnerAttributeTypesDelegate =
                @Ict.Petra.Shared.MPartner.Calculations.DeterminePhonePartnerAttributeTypes;

            TSmtpSender.GetSmtpSettings = @TSmtpSender.GetSmtpSettingsFromAppSettings;
        }
    }
}
