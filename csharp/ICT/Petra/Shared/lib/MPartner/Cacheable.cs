/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically partner submodule
    /// </summary>
    public enum TCacheablePartnerTablesEnum
    {
        /// <summary>
        /// type of address
        /// </summary>
        AddresseeTypeList,

        /// <summary>
        /// how did we get to know this partner
        /// </summary>
        AcquisitionCodeList,

        /// <summary>
        /// todoComment
        /// </summary>
        BusinessCodeList,

        /// <summary>
        /// list of countries
        /// </summary>
        CountryList,

        /// <summary>
        /// list of currencies
        /// </summary>
        CurrencyCodeList,

        /// <summary>
        /// list of data labels
        /// </summary>
        DataLabelList,

        /// <summary>
        /// list of how a data label is used
        /// </summary>
        DataLabelUseList,

        /// <summary>
        /// categories for data label lookup
        /// </summary>
        DataLabelLookupCategoryList,

        /// <summary>
        /// list for data label lookup
        /// </summary>
        DataLabelLookupList,

        /// <summary>
        /// list of denominations
        /// </summary>
        DenominationList,

        /// <summary>
        /// list of installed sites
        /// </summary>
        InstalledSitesList,

        /// <summary>
        /// lists of interests
        /// </summary>
        InterestList,

        /// <summary>
        /// categories of interest values
        /// </summary>
        InterestCategoryList,

        /// <summary>
        /// list of languages
        /// </summary>
        LanguageCodeList,

        /// <summary>
        /// type of location
        /// </summary>
        LocationTypeList,

        /// <summary>
        /// list of marital status values
        /// </summary>
        MaritalStatusList,

        /// <summary>
        /// occupations
        /// </summary>
        OccupationList,

        /// <summary>
        /// possible status of a partner
        /// </summary>
        PartnerStatusList,

        /// <summary>
        /// types of partners
        /// </summary>
        PartnerTypeList,

        /// <summary>
        /// types of units
        /// </summary>
        UnitTypeList,

        /// <summary>
        /// list of users that are associated with foundations
        /// </summary>
        FoundationOwnerList,

        /// <summary>
        /// list of stati for proposal
        /// </summary>
        ProposalStatusList,

        /// <summary>
        /// list of submission types for proposals
        /// </summary>
        ProposalSubmissionTypeList,

        /// <summary>
        /// list of counties
        /// </summary>
        CountyList,

        /// <summary>
        /// list of countries that are actually used in the database (smaller than the full country list)
        /// </summary>
        CountryListFromExistingLocations
    };

    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically mailing submodule
    /// </summary>
    public enum TCacheableMailingTablesEnum
    {
        /// <summary>
        /// todoComment
        /// </summary>
        MergeFormList,

        /// <summary>
        /// todoComment
        /// </summary>
        MergeFieldList,

        /// <summary>
        /// list of Post code regions
        /// </summary>
        PostCodeRegionList
    };

    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically for subscriptions
    /// </summary>
    public enum TCacheableSubscriptionsTablesEnum
    {
        /// <summary>
        /// publication costs
        /// </summary>
        PublicationCost,

        /// <summary>
        /// available publications
        /// </summary>
        PublicationList,

        /// <summary>
        /// reasons why someone gets a subscription
        /// </summary>
        ReasonSubscriptionGivenList,

        /// <summary>
        /// reasons why someone would cancel a subscription
        /// </summary>
        ReasonSubscriptionCancelledList
    };
}