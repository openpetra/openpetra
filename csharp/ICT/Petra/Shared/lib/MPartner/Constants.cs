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
using Ict.Common;

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

        /// <summary>FamilyPartnerKey</summary>
        public const String PARTNERIMPORT_FAMILYPARTNERKEY = "FamilyPartnerKey";

        /// <summary>PersonPartnerKey</summary>
        public const String PARTNERIMPORT_PERSONPARTNERKEY = "PersonPartnerKey";

        /// <summary>FamilyName</summary>
        public const String PARTNERIMPORT_FAMILYNAME = "FamilyName";

        /// <summary>MaritalStatus</summary>
        public const String PARTNERIMPORT_MARITALSTATUS = "MaritalStatus";

        /// <summary>Address1</summary>
        public const String PARTNERIMPORT_ADDRESS1 = "Address1";

        /// <summary>Street</summary>
        public const String PARTNERIMPORT_STREET = "Street";

        /// <summary>Address3</summary>
        public const String PARTNERIMPORT_ADDRESS3 = "Address3";

        /// <summary>PostCode</summary>
        public const String PARTNERIMPORT_POSTCODE = "PostCode";

        /// <summary>City</summary>
        public const String PARTNERIMPORT_CITY = "City";

        /// <summary>County</summary>
        public const String PARTNERIMPORT_COUNTY = "County_State";

        /// <summary>CountryCode</summary>
        public const String PARTNERIMPORT_COUNTRYCODE = "Country";

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

        /// <summary>Date of birth</summary>
        public const String PARTNERIMPORT_DATEOFBIRTH = "DateOfBirth";

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
        public const String PARTNERIMPORT_APPDATE = "ApplicationDate";

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

        /// <summary>PassportName</summary>
        public const String PARTNERIMPORT_PASSPORTNAME = "PassportName";

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

        /// <summary>RecordImported</summary>
        public const String PARTNERIMPORT_RECORDIMPORTED = "RecordImported";


        /// <summary>Default Aquisition Code to use for Partner Import</summary>
        public const String PARTNERIMPORT_AQUISITION_DEFAULT = "IMPORT";

        /// Default values
        /// <summary>used eg. for PPerson.OccupationCode</summary>
        public const String DEFAULT_CODE_UNKNOWN = "UNKNOWN";

        /// <summary> Acquisition code for partner </summary>
        public const String ACQUISITIONCODE_APPLICANT = "APL";

        /// <summary> Cannot merge these classes </summary>
        public const string PARTNERMERGE_SELECTED_CLASSES_CANNOT_BE_MERGED = "Selected Partner Classes cannot be merged!";

        #region readonly Fields   (Used for 'constants' whose value can be translated so that they are meaningful to the users in their language)

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_DESTINATION = Catalog.GetString("{0} change made to Gift Destinations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_DESTINATION_PLURAL = Catalog.GetString("{0} changes made to Gift Destinations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_INFO = Catalog.GetString("{0} change made to Gift Donor and/or Recipient");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_INFO_PLURAL = Catalog.GetString("{0} changes made to Gift Donor and/or Recipient");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_ACCOUNTS_PAYABLE = Catalog.GetString("{0} change made to Accounts Payable details");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_ACCOUNTS_PAYABLE_PLURAL = Catalog.GetString("{0} changes made to Accounts Payable details");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_MOTIVATIONS = Catalog.GetString("{0} change made to Gift Motivations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GIFT_MOTIVATIONS_PLURAL = Catalog.GetString("{0} changes made to Gift Motivations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_EXTRACTS = Catalog.GetString("{0} change made to Extracts");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_EXTRACTS_PLURAL = Catalog.GetString("{0} changes made to Extracts");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GREETINGS = Catalog.GetString("{0} change made to Greetings");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GREETINGS_PLURAL = Catalog.GetString("{0} changes made to Greetings");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_CONTACT_LOG_AND_REMINDERS = Catalog.GetString("{0} change made to Contact Log and Reminders");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_CONTACT_LOG_AND_REMINDERS_PLURAL = Catalog.GetString(
            "{0} changes made to Contact Log and Reminders");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_INTERESTS = Catalog.GetString("{0} change made to Interests");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_INTERESTS_PLURAL = Catalog.GetString("{0} changes made to Interests");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_ADDRESSES = Catalog.GetString("{0} change made to Addresses");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_ADDRESSES_PLURAL = Catalog.GetString("{0} changes made to Addresses");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PARTNER_TYPES = Catalog.GetString("{0} change made to Partner Types");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PARTNER_TYPES_PLURAL = Catalog.GetString("{0} changes made to Partner Types");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_SUBSCRIPTIONS = Catalog.GetString("{0} change made to Subscriptions");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_SUBSCRIPTIONS_PLURAL = Catalog.GetString("{0} changes made to Subscriptions");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_APPLICATIONS = Catalog.GetString("{0} change made to Applications");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_APPLICATIONS_PLURAL = Catalog.GetString("{0} changes made to Applications");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PERSONNEL_DATA = Catalog.GetString("{0} change made to Personnel Data");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PERSONNEL_DATA_PLURAL = Catalog.GetString("{0} changes made to Personnel Data");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_JOBS = Catalog.GetString("{0} change made to Jobs");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_JOBS_PLURAL = Catalog.GetString("{0} changes made to Jobs");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PARTNER_CLASS = Catalog.GetString("{0} change made to Partner Class ");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_PARTNER_CLASS_PLURAL = Catalog.GetString("{0} changes made to Partner Class ");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_RELATIONSHIPS = Catalog.GetString("{0} change made to Relationships");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_RELATIONSHIPS_PLURAL = Catalog.GetString("{0} changes made to Relationships");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_BASIC_PARTNER_INFO = Catalog.GetString("{0} change made to Basic Partner Info");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_BASIC_PARTNER_INFO_PLURAL = Catalog.GetString("{0} changes made to Basic Partner Info");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_VENUE = Catalog.GetString("{0} change made to Venue - Buildings, Rooms and Allocations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_VENUE_PLURAL = Catalog.GetString("{0} changes made to Venue - Buildings, Rooms and Allocations");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_BANK_ACCOUNTS = Catalog.GetString("{0} change made to Bank Accounts");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_BANK_ACCOUNTS_PLURAL = Catalog.GetString("{0} changes made to Bank Accounts");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_TAX_PERCENTAGE = Catalog.GetString("{0} change made to Tax Deductibility Percentage");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_TAX_PERCENTAGE_PLURAL = Catalog.GetString("{0} changes made to Tax Deductibility Percentage");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_LINK_TO_COST_CENTRE = Catalog.GetString("{0} change made to Link to Cost Centre");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_LINK_TO_COST_CENTRE_PLURAL = Catalog.GetString("{0} changes made to Link to Cost Centre");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GRAPHICS = Catalog.GetString("{0} change made to Graphics");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_GRAPHICS_PLURAL = Catalog.GetString("{0} changes made to Graphics");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_CONTACT_DETAILS = Catalog.GetString("{0} change made to Partner Contact Details");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_CONTACT_DETAILS_PLURAL = Catalog.GetString("{0} changes made to Partner Contact Details");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_MERGE_SUCCESSFUL = Catalog.GetString("Merge Partners completed successfully.");

        /// <summary>Partner Merge Topic</summary>
        public static readonly string PARTNERMERGE_MERGE_CANCELLED = Catalog.GetString("Cancelled by user");


        #endregion
    }
}