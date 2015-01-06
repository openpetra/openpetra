//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//       timop
//
// Copyright 2004-2015 by OM International
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

        /// <summary>Publication Valid</summary>
        public const String PUBLICATION_VALID_TEXT_COLUMNNAME = "PublicationValidText";

        /// <summary>Banking Type</summary>
        public const Int16 BANKINGTYPE_BANKACCOUNT = 0;

        /// <summary>Banking Type</summary>
        public const Int16 BANKINGTYPE_SAVINGSACCOUNT = 2;

        /// <summary>Banking Details Usage Type</summary>
        public const String BANKINGUSAGETYPE_MAIN = "MAIN";

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

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_EMAIL = "EMAIL";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_FAX = "FAX";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_FORMLETTER = "FLETTER";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_LETTER = "LETTER";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_MAILING = "MAILING";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_PERSON = "PERSON";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_PHONE = "PHONE";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_RESPONSE = "RESPONSE";

        /// <summary>Method of Contact</summary>
        public const String METHOD_CONTACT_VISIT = "VISIT";

        /// <summary>Partner Types (Special Types)</summary>
        public const String PARTNERTYPE_EX_WORKER = "EX-WORKER";

        /// <summary>Partner Types (Special Types)</summary>
        public const String PARTNERTYPE_WORKER = "WORKER";

        /// <summary>Partner Types</summary>
        public const String PARTNERTYPE_LEDGER = "LEDGER";

        /// <summary>Partner Types</summary>
        public const String PARTNERTYPE_COSTCENTRE = "COSTCENTRE";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_FAMILY = "FAMILY";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_PERSON = "PERSON";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_ORGANISATION = "ORGANISATION";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_CHURCH = "CHURCH";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_VENUE = "VENUE";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_UNIT = "UNIT";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_AREA = "A";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_COUNTRY = "C";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_CONFERENCE = "CONF";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_FUND = "D";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_FIELD = "F";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_KEYMIN = "KEY-MIN";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_OTHER = "O";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_ROOT = "R";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_TEAM = "T";

        /// <summary>Unit-Type</summary>
        public const String UNIT_TYPE_WORKING_GROUP = "W";

        /// <summary>Partner class</summary>
        public const String PARTNERCLASS_BANK = "BANK";

        /// <summary>Partner status</summary>
        public const String PARTNERSTATUS_ACTIVE = "ACTIVE";

        /// <summary>Partner status</summary>
        public const String PARTNERSTATUS_INACTIVE = "INACTIVE";

        /// <summary>Partner status</summary>
        public const String PARTNERSTATUS_MERGED = "MERGED";

        /// <summary>Gender</summary>
        public const String GENDER_FEMALE = "Female";

        /// <summary>Gender</summary>
        public const String GENDER_MALE = "Male";

        /// <summary>Unknown</summary>
        public const String GENDER_UNKNOWN = "Unknown";

        /// <summary>1-MALE</summary>
        public const String ADDRESSEETYPE_MALE = "1-MALE";

        /// <summary>1-FEMALE</summary>
        public const String ADDRESSEETYPE_FEMALE = "1-FEMALE";

        /// <summary>addressee type</summary>
        public const String ADDRESSEETYPE_COUPLE = "COUPLE";

        /// <summary>addressee type</summary>
        public const String ADDRESSEETYPE_FAMILY = "FAMILY";

        /// <summary>addressee type</summary>
        public const String ADDRESSEETYPE_ORGANISATION = "ORGANISA";

        /// <summary>DEFAULT</summary>
        public const String ADDRESSEETYPE_DEFAULT = "DEFAULT";

        /// <summary>U</summary>
        public const String MARITALSTATUS_UNDEFINED = "U";

        /// <summary>N</summary>
        public const String MARITALSTATUS_SINGLE = "N";

        /// <summary>E</summary>
        public const String MARITALSTATUS_ENGAGED = "E";

        /// <summary>M</summary>
        public const String MARITALSTATUS_MARRIED = "M";

        /// <summary>D</summary>
        public const String MARITALSTATUS_DIVORCED = "D";

        /// <summary>HOME</summary>
        public const String LOCATIONTYPE_HOME = "HOME";

        /// <summary>Location type</summary>
        public const String LOCATIONTYPE_BUSINESS = "BUSINESS";

        /// <summary>partner attribute</summary>
        public const String ATTR_TYPE_PHONE = "Phone";

        /// <summary>partner attribute</summary>
        public const String ATTR_TYPE_FAX = "Fax";

        /// <summary>partner attribute</summary>
        public const String ATTR_TYPE_MOBILE_PHONE = "Mobile Phone";

        /// <summary>partner attribute</summary>
        public const String ATTR_TYPE_EMAIL = "E-Mail";

        /// <summary>partner attribute</summary>
        public const String ATTR_TYPE_WEBSITE = "Web Site";

        /// <summary>
        /// Partner Attribute Type that denotes the 'Primary Contact Method'.
        /// </summary>
        public const string ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD = "PARTNERS_PRIMARY_CONTACT_METHOD";

        /// <summary>PartnerClass</summary>
        public const String PARTNERIMPORT_PARTNERCLASS = "PartnerClass";

        /// <summary>PartnerKey</summary>
        public const String PARTNERIMPORT_PARTNERKEY = "PartnerKey";

        /// <summary>FamilyName</summary>
        public const String PARTNERIMPORT_FAMILYNAME = "FamilyName";

        /// <summary>MaritalStatus</summary>
        public const String PARTNERIMPORT_MARITALSTATUS = "MaritalStatus";

        /// <summary>StreetName</summary>
        public const String PARTNERIMPORT_STREETNAME = "StreetName";

        /// <summary>Locality</summary>
        public const String PARTNERIMPORT_LOCALITY = "Locality";

        /// <summary>Address3</summary>
        public const String PARTNERIMPORT_ADDRESS = "Address3";

        /// <summary>PostalCode</summary>
        public const String PARTNERIMPORT_POSTALCODE = "PostalCode";

        /// <summary>City</summary>
        public const String PARTNERIMPORT_CITY = "City";

        /// <summary>County</summary>
        public const String PARTNERIMPORT_COUNTY = "County";

        /// <summary>CountryCode</summary>
        public const String PARTNERIMPORT_COUNTRYCODE = "CountryCode";

        /// <summary>Aquisition</summary>
        public const String PARTNERIMPORT_AQUISITION = "Aquisition";

        /// <summary>Gender</summary>
        public const String PARTNERIMPORT_GENDER = "Gender";

        /// <summary>AddresseeType</summary>
        public const String PARTNERIMPORT_ADDRESSEE_TYPE = "AddresseeType";

        /// <summary>Language</summary>
        public const String PARTNERIMPORT_LANGUAGE = "Language";

        /// <summary>OMerField</summary>
        public const String PARTNERIMPORT_OMERFIELD = "OMerField";

        /// <summary>FirstName</summary>
        public const String PARTNERIMPORT_FIRSTNAME = "FirstName";

        /// <summary>Email</summary>
        public const String PARTNERIMPORT_EMAIL = "Email";

        /// <summary>Phone</summary>
        public const String PARTNERIMPORT_PHONE = "Phone";

        /// <summary>MobilePhone</summary>
        public const String PARTNERIMPORT_MOBILEPHONE = "MobilePhone";

        /// <summary>Title</summary>
        public const String PARTNERIMPORT_TITLE = "Title";

        /// <summary>SpecialTypes</summary>
        public const String PARTNERIMPORT_SPECIALTYPES = "SpecialTypes";

        /// <summary>Vegetarian</summary>
        public const String PARTNERIMPORT_VEGETARIAN = "Vegetarian";

        /// <summary>MedicalNeeds</summary>
        public const String PARTNERIMPORT_MEDICALNEEDS = "MedicalNeeds";

        /// <summary>EventPartnerKey</summary>
        public const String PARTNERIMPORT_EVENTKEY = "EventPartnerKey";

        /// <summary>ArrivalDate</summary>
        public const String PARTNERIMPORT_ARRIVALDATE = "ArrivalDate";

        /// <summary>ArrivalTime</summary>
        public const String PARTNERIMPORT_ARRIVALTIME = "ArrivalTime";

        /// <summary>DepartureDate</summary>
        public const String PARTNERIMPORT_DEPARTUREDATE = "DepartureDate";

        /// <summary>DepartureTime</summary>
        public const String PARTNERIMPORT_DEPARTURETIME = "DepartureTime";

        /// <summary>EventRole</summary>
        public const String PARTNERIMPORT_EVENTROLE = "EventRole";

        /// <summary>AppStatus</summary>
        public const String PARTNERIMPORT_APPSTATUS = "AppStatus";

        /// <summary>AppType</summary>
        public const String PARTNERIMPORT_APPTYPE = "AppType";

        /// <summary>PreviousAttendance</summary>
        public const String PARTNERIMPORT_PREVIOUSATTENDANCE = "PreviousAttendance";

        /// <summary>ChargedField</summary>
        public const String PARTNERIMPORT_CHARGEDFIELD = "ChargedField";

        /// <summary>AppComments</summary>
        public const String PARTNERIMPORT_APPCOMMENTS = "AppComments";

        /// <summary>Notes</summary>
        public const String PARTNERIMPORT_NOTES = "Notes";

        /// <summary>NotesFamily</summary>
        public const String PARTNERIMPORT_NOTESFAMILY = "NotesFamily";

        /// <summary>ContactCode</summary>
        public const String PARTNERIMPORT_CONTACTCODE = "ContactCode";
        /// <summary>ContactDate</summary>
        public const String PARTNERIMPORT_CONTACTDATE = "ContactDate";
        /// <summary>ContactTime</summary>
        public const String PARTNERIMPORT_CONTACTTIME = "ContactTime";
        /// <summary>Contactor</summary>
        public const String PARTNERIMPORT_CONTACTOR = "Contactor";
        /// <summary>ContactNotes</summary>
        public const String PARTNERIMPORT_CONTACTNOTES = "ContactNotes";
        /// <summary>ContactAttr</summary>
        public const String PARTNERIMPORT_CONTACTATTR = "ContactAttr";
        /// <summary>ContactDetail</summary>
        public const String PARTNERIMPORT_CONTACTDETAIL = "ContactDetail";

        /// <summary>PassportNumber</summary>
        public const String PARTNERIMPORT_PASSPORTNUMBER = "PassportNumber";

        /// <summary>PassportType</summary>
        public const String PARTNERIMPORT_PASSPORTTYPE = "PassportType";

        /// <summary>PassportPlaceOfBirth</summary>
        public const String PARTNERIMPORT_PASSPORTPLACEOFBIRTH = "PassportPlaceOfBirth";

        /// <summary>PassportNationality</summary>
        public const String PARTNERIMPORT_PASSPORTNATIONALITY = "PassportNationality";

        /// <summary>PassportPlaceOfIssue</summary>
        public const String PARTNERIMPORT_PASSPORTPLACEOFISSUE = "PassportPlaceOfIssue";

        /// <summary>PassportCountryOfIssue</summary>
        public const String PARTNERIMPORT_PASSPORTCOUNTRYOFISSUE = "PassportCountryOfIssue";

        /// <summary>PassportDateOfIssue</summary>
        public const String PARTNERIMPORT_PASSPORTDATEOFISSUE = "PassportDateOfIssue";

        /// <summary>PassportDateOfExpiration</summary>
        public const String PARTNERIMPORT_PASSPORTDATEOFEXPIRATION = "PassportDateOfExpiration";

        /// <summary>FamilyPartnerKey</summary>
        public const String PARTNERIMPORT_FAMILYPARTNERKEY = "FamilyPartnerKey";

        /// <summary>RecordImported</summary>
        public const String PARTNERIMPORT_RECORDIMPORTED = "RecordImported";


        /// <summary>Default Aquisition Code to use for Partner Import</summary>
        public const String PARTNERIMPORT_AQUISITION_DEFAULT = "IMPORT";

        /// Default values
        /// <summary>used eg. for PPerson.OccupationCode</summary>
        public const String DEFAULT_CODE_UNKNOWN = "UNKNOWN";

        /// <summary> Acquisition code for partner </summary>
        public const String ACQUISITIONCODE_APPLICANT = "APL";
    }
}