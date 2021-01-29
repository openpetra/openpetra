//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2021 by OM International
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
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;
using Ict.Petra.Server.MSysMan.Application.WebConnectors;
using Ict.Petra.Shared;
using Ict.Petra.Server.MCommon.Validation;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Validation;
using Ict.Petra.Server.MFinance.Validation;
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
using Ict.Petra.Server.MCommon.Cacheable.WebConnectors;
using Ict.Petra.Server.MConference.Cacheable.WebConnectors;
using Ict.Petra.Server.MFinance.Cacheable.WebConnectors;
using Ict.Petra.Server.MPartner.Mailing.Cacheable.WebConnectors;
using Ict.Petra.Server.MPartner.Partner.Cacheable.WebConnectors;
using Ict.Petra.Server.MPartner.Subscriptions.Cacheable.WebConnectors;
using Ict.Petra.Server.MPersonnel.Person.Cacheable.WebConnectors;
using Ict.Petra.Server.MPersonnel.Unit.Cacheable.WebConnectors;
using Ict.Petra.Server.MSysMan.Cacheable.WebConnectors;

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
        [ThreadStatic]
        private static Ict.Petra.Server.MCommon.Cacheable.TCacheable CachePopulatorCommon;
        [ThreadStatic]
        private static Ict.Petra.Server.MConference.Cacheable.TCacheable CachePopulatorConference;
        [ThreadStatic]
        private static Ict.Petra.Server.MFinance.Cacheable.TCacheable CachePopulatorFinance;
        [ThreadStatic]
        private static Ict.Petra.Server.MPartner.Mailing.Cacheable.TPartnerCacheable CachePopulatorMailing;
        [ThreadStatic]
        private static Ict.Petra.Server.MPartner.Partner.Cacheable.TPartnerCacheable CachePopulatorPartner;
        [ThreadStatic]
        private static Ict.Petra.Server.MPartner.Subscriptions.Cacheable.TPartnerCacheable CachePopulatorSubscriptions;
        [ThreadStatic]
        private static Ict.Petra.Server.MPersonnel.Person.Cacheable.TPersonnelCacheable CachePopulatorPersonnel;
        [ThreadStatic]
        private static Ict.Petra.Server.MPersonnel.Unit.Cacheable.TPersonnelCacheable CachePopulatorUnits;
        [ThreadStatic]
        private static Ict.Petra.Server.MSysMan.Cacheable.TCacheable CachePopulatorSysMan;

        /// <summary>
        /// Initialize the static variables.
        /// Set up Error Codes and Data Validation Delegates for a Web Request.
        /// This setting-up makes use of the fact that this Method is called only once,
        /// at the start of each Web Request in TOpenPetraOrgSessionManager.Init()
        /// </summary>
        public static void Init()
        {
            TValidationHelper.SharedGetDataDelegate = @TCommonDataReader.GetData;
            TPartnerValidationHelper.VerifyPartnerDelegate = @TPartnerServerLookups.VerifyPartner;
            TPartnerValidationHelper.PartnerHasActiveStatusDelegate = @TPartnerServerLookups.PartnerHasActiveStatus;
            TPartnerValidationHelper.PartnerIsLinkedToCCDelegate = @TPartnerServerLookups.PartnerIsLinkedToCC;
            TPartnerValidationHelper.PartnerOfTypeCCIsLinkedDelegate = @TPartnerServerLookups.PartnerOfTypeCCIsLinked;
            TPartnerValidationHelper.PartnerHasCurrentGiftDestinationDelegate = @TPartnerServerLookups.PartnerHasCurrentGiftDestination;
            TFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookupWebConnector.GetCurrentPostingRangeDates;
            TFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;
            TFinanceValidationHelper.GetFirstDayOfAccountingPeriodDelegate = @TAccountingPeriodsWebConnector.GetFirstDayOfAccountingPeriod;
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

            Ict.Petra.Server.MCommon.Cacheable.WebConnectors.TCommonCacheableWebConnector.Init();
            Ict.Petra.Server.MConference.Cacheable.WebConnectors.TConferenceCacheableWebConnector.Init();
            Ict.Petra.Server.MFinance.Cacheable.WebConnectors.TFinanceCacheableWebConnector.Init();
            Ict.Petra.Server.MPartner.Mailing.Cacheable.WebConnectors.TMailingCacheableWebConnector.Init();
            Ict.Petra.Server.MPartner.Partner.Cacheable.WebConnectors.TPartnerCacheableWebConnector.Init();
            Ict.Petra.Server.MPartner.Subscriptions.Cacheable.WebConnectors.TSubscriptionsCacheableWebConnector.Init();
            Ict.Petra.Server.MPersonnel.Person.Cacheable.WebConnectors.TPersonCacheableWebConnector.Init();
            Ict.Petra.Server.MPersonnel.Unit.Cacheable.WebConnectors.TUnitCacheableWebConnector.Init();
            Ict.Petra.Server.MSysMan.Cacheable.WebConnectors.TSysManCacheableWebConnector.Init();

            TSharedDataCache.TMCommon.GetCacheableCommonTableDelegate = @CachePopulatorCommon.GetCacheableTable;
            TSharedDataCache.TMFinance.GetCacheableFinanceTableDelegate = @CachePopulatorFinance.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheablePartnerTableDelegate = @CachePopulatorPartner.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableMailingTableDelegate = @CachePopulatorMailing.GetCacheableTable;
            TSharedDataCache.TMPartner.GetCacheableSubscriptionsTableDelegate = @CachePopulatorSubscriptions.GetCacheableTable;
            TSharedDataCache.TMPersonnel.GetCacheablePersonnelTableDelegate = @CachePopulatorPersonnel.GetCacheableTable;
            TSharedDataCache.TMPersonnel.GetCacheableUnitsTableDelegate = @CachePopulatorUnits.GetCacheableTable;
            TSharedDataCache.TMConference.GetCacheableConferenceTableDelegate = @CachePopulatorConference.GetCacheableTable;
            TSharedDataCache.TMSysMan.GetCacheableSysManTableDelegate = @CachePopulatorSysMan.GetCacheableTable;

            TCacheableTablesManager.Init();
            TCacheableTablesManager.GCacheableTablesManager = new TCacheableTablesManager(new TDelegateSendClientTask(TClientManager.QueueClientTask));

            TSmtpSender.GetSmtpSettings = @TSmtpSender.GetSmtpSettingsFromAppSettings;
        }
    }
}
