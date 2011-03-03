// auto generated with nant generateORM
// Do not modify this file manually!
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

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically Partner submodule
    /// </summary>
    public enum TCacheablePartnerTablesEnum
    {
        /// <summary>
        /// Ex. Fam - Family, SM - Single Male, etc.
        /// </summary>
        AddresseeTypeList,

        /// <summary>
        /// how did we get to know this partner
        /// </summary>
        AcquisitionCodeList,

        /// <summary>
        /// List of businesses with codes
        /// </summary>
        BusinessCodeList,

        /// <summary>
        /// Unit of money for various countries.
        /// </summary>
        CurrencyCodeList,

        /// <summary>
        /// This table is used to define data labels for individual use in each office.
        /// </summary>
        DataLabelList,

        /// <summary>
        /// This table defines where a data label is used and the order the labels appear in.
        /// </summary>
        DataLabelUseList,

        /// <summary>
        /// This table holds the categories that can be used for data label values.
        /// </summary>
        DataLabelLookupCategoryList,

        /// <summary>
        /// This table holds all lookup values that can be used for data label values.
        /// </summary>
        DataLabelLookupList,

        /// <summary>
        /// List of denomination codes for churches
        /// </summary>
        DenominationList,

        /// <summary>
        /// Area of Interest
        /// </summary>
        InterestList,

        /// <summary>
        /// Categories for Area of Interest
        /// </summary>
        InterestCategoryList,

        /// <summary>
        /// Types of address e.g. home, business
        /// </summary>
        LocationTypeList,

        /// <summary>
        /// This table contains the codes indicating someones marital status.
        /// </summary>
        MaritalStatusList,

        /// <summary>
        /// How contacts are made
        /// </summary>
        MethodOfContactList,

        /// <summary>
        /// List of occupations with codes
        /// </summary>
        OccupationList,

        /// <summary>
        /// List of statuses for partners
        /// </summary>
        PartnerStatusList,

        /// <summary>
        /// List of all possible special types for a partner.
        /// </summary>
        PartnerTypeList,

        /// <summary>
        /// Foundation proposal status codes and descriptions
        /// </summary>
        ProposalStatusList,

        /// <summary>
        /// Submission type for foundation proposals e.g. EMAIL, LETTER.
        /// </summary>
        ProposalSubmissionTypeList,

        /// <summary>
        /// List of relationships between partners.  Relations occur in one direction only.   The relation code is used in the p_partner_relationship record.
        /// </summary>
        RelationList,

        /// <summary>
        /// This table contains the codes that indicate the categories of relations (grouping).
        /// </summary>
        RelationCategoryList,

        /// <summary>
        /// General information about the unit such as unit type and entry conference.
        /// </summary>
        UnitTypeList,
        /// <summary>
        /// list of counties
        /// </summary>
        CountyList,

        /// <summary>
        /// list of users that are associated with foundations
        /// </summary>
        FoundationOwnerList,

        /// <summary>
        /// list of installed sites
        /// </summary>
        InstalledSitesList,

        /// <summary>
        /// list of countries that are actually used in the database (smaller than the full country list)
        /// </summary>
        CountryListFromExistingLocations
    };
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically Mailing submodule
    /// </summary>
    public enum TCacheableMailingTablesEnum
    {
        /// <summary>
        /// Possible attributes for partner contacts.  Gives the description of each attribute code.  An attribute is a type of contact that was made or which occurred with a partner.
        /// </summary>
        ContactAttributeList,

        /// <summary>
        /// Possible attribute details for each contact attribute.  Breaks down the attribute into more specifice information that applies to a contact with a partner.
        /// </summary>
        ContactAttributeDetailList,

        /// <summary>
        /// How contacts are made
        /// </summary>
        MethodOfContactList,

        /// <summary>
        /// Master record for Mail Merge output creation
        /// </summary>
        MergeFormList,

        /// <summary>
        /// Fields within a Mail Merge Form
        /// </summary>
        MergeFieldList,

        /// <summary>
        /// Lists mailings that are being tracked.   When entering gifts, the mailing that motivated the gift can be indicated.
        /// </summary>
        MailingList,
        /// <summary>
        /// todoComment
        /// </summary>
        PostCodeRegionList
    };
    /// <summary>
    /// Enums holding the possible cacheable tables for the Petra Partner Module, specifically Subscriptions submodule
    /// </summary>
    public enum TCacheableSubscriptionsTablesEnum
    {
        /// <summary>
        /// Cost of a publication
        /// </summary>
        PublicationCostList,

        /// <summary>
        /// available publications
        /// </summary>
        PublicationList,

        /// <summary>
        /// List of reasons for giving a subscription
        /// </summary>
        ReasonSubscriptionGivenList,

        /// <summary>
        /// List of reasons for cancelling a subscription
        /// </summary>
        ReasonSubscriptionCancelledList
    };
}
