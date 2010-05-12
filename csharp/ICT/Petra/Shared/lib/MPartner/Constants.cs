//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
    /// some constants used in the partner module
    /// </summary>
    public class MPartnerConstants
    {
        /// <summary>Addressmanagement</summary>
        public const String ADDRESSMGMT_SIMILARLOCATIONTABLE = "SimilarLocationTable";

        /// <summary>Addressmanagement</summary>
        public const String PARTNERADDRESSAGGREGATERESPONSE_DATASET = "PartnerAddressAggregateResponse";

        /// <summary>Addressmanagement</summary>
        public const String EXISTINGLOCATIONPARAMETERS_TABLENAME = "SimilarLocationParameters";

        /// <summary>Addressmanagement</summary>
        public const String ADDRESSADDEDORCHANGEDPROMOTION_TABLENAME = "AddressAddedOrChangedPromotion";

        /// <summary>Addressmanagement</summary>
        public const String ADDRESSCHANGEPROMOTIONPARAMETERS_TABLENAME = "ChangePromotionParameters";

        /// <summary>Subscription Statuses</summary>
        public const String SUBSCRIPTIONS_STATUS_PERMANENT = "PERMANENT";

        /// <summary>Subscription Statuses</summary>
        public const String SUBSCRIPTIONS_STATUS_PROVISIONAL = "PROVISIONAL";

        /// <summary>Subscription Statuses</summary>
        public const String SUBSCRIPTIONS_STATUS_GIFT = "GIFT";

        /// <summary>Subscription Statuses</summary>
        public const String SUBSCRIPTIONS_STATUS_CANCELLED = "CANCELLED";

        /// <summary>Subscription Statuses</summary>
        public const String SUBSCRIPTIONS_STATUS_EXPIRED = "EXPIRED";

        /// <summary>Subscription Reasons Ended</summary>
        public const String SUBSCRIPTIONS_REASON_ENDED_BADADDR = "BAD-ADDR";

        /// <summary>Partner Types (Special Types)</summary>
        public const String PARTNERTYPE_EX_WORKER = "EX-WORKER";

        /// <summary>Partner Types</summary>
        public const String PARTNERTYPE_LEDGER = "LEDGER";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_FAMILY = "FAMILY";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_PERSON = "PERSON";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_ORGANISATION = "ORGANISATION";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_CHURCH = "CHURCH";

        /// <summary>Partner status</summary>
        public const String PARTNERSTATUS_ACTIVE = "ACTIVE";

        /// <summary>Partner status</summary>
        public const String PARTNERSTATUS_INACTIVE = "INACTIVE";

        /// <summary>Gender</summary>
        public const String GENDER_FEMALE = "Female";

        /// <summary>Gender</summary>
        public const String GENDER_MALE = "Male";

        /// <summary>Gender</summary>
        public const String GENDER_UNKNOWN = "Unknown";

        /// <summary>Addresseetype</summary>
        public const String ADDRESSEETYPE_MALE = "1-MALE";

        /// <summary>Addresseetype</summary>
        public const String ADDRESSEETYPE_FEMALE = "1-FEMALE";

        /// <summary>Addresseetype</summary>
        public const String ADDRESSEETYPE_DEFAULT = "DEFAULT";

        /// <summary>Marital Status</summary>
        public const String MARITALSTATUS_UNDEFINED = "U";

        /// <summary>Marital Status</summary>
        public const String MARITALSTATUS_SINGLE = "N";

        /// <summary>Marital Status</summary>
        public const String MARITALSTATUS_ENGAGED = "E";

        /// <summary>Marital Status</summary>
        public const String MARITALSTATUS_MARRIED = "M";

        /// <summary>Marital Status</summary>
        public const String MARITALSTATUS_DIVORCED = "D";

        /// <summary>Location Type</summary>
        public const String LOCATIONTYPE_HOME = "HOME";

        /// Default values
        /// <summary>used eg. for PPerson.OccupationCode</summary>
        public const String DEFAULT_CODE_UNKNOWN = "UNKNOWN";
    }
}