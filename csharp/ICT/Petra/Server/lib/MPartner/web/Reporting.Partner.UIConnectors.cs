//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2017 by OM International
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.DataAggregates;
using System.Linq;

namespace Ict.Petra.Server.MPartner.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the Partner reporting screens
    ///</summary>
    public partial class TPartnerReportingWebConnector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="DbAdapter"></param>
        /// <returns></returns>
        [NoRemoting]
        public static DataSet BriefAddressReport(Dictionary <string, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataSet ReturnDataSet = new DataSet();

            TDBTransaction Transaction = null;
            DataTable Partners = new DataTable("Partners");
            DataTable Locations = new DataTable("Locations");

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String PartnerSelection = TPartnerReportTools.GetPartnerKeysAsString(AParameters,
                        DbAdapter);

                    String PartnersQuery =
                        @"SELECT
	                                            p_partner_key_n AS PartnerKey,
	                                            p_partner_class_c AS PartnerClass,
	                                            p_partner_short_name_c AS PartnerShortName,
	                                            p_status_code_c AS StatusCode

                                            FROM p_partner
                                            WHERE p_partner_key_n IN ("
                        +
                        PartnerSelection + ")";

                    Partners = DbAdapter.RunQuery(PartnersQuery,
                        "Partners",
                        Transaction);

                    if ((AParameters["param_addressdetail"]).ToString() == "GetBestAddressForPartner")
                    {
                        // Get best Addresses for every partner
                        Locations = TAddressTools.GetBestAddressForPartners(Partners,
                            0,
                            Transaction);
                    }
                    else
                    {
                        // in this case we want to get all addresses (including expired ones)
                        String LocationsQuery =
                            @"SELECT
                                               p_partner_location.*,
                                               p_location.*,
                                               p_country.p_address_order_i
                                           FROM
                                               p_partner_location, p_location, p_country
                                           WHERE
                                               p_partner_location.p_partner_key_n IN("
                            +

                            PartnerSelection +
                            @")

                                               AND p_location.p_location_key_i = p_partner_location.p_location_key_i

                                               AND p_country.p_country_code_c = p_location.p_country_code_c"                                                                                                                                                           ;

                        Locations = DbAdapter.RunQuery(LocationsQuery,
                            "Locations",
                            Transaction);
                    }

                    Locations.TableName = "Locations";

                    //Add fields and retrieve data for contact details
                    TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(Partners, 0, DbAdapter, true, true, true);

                    //Retrieve "field" information
                    TPartnerReportTools.AddFieldNameToTable(Partners, 0, AParameters, "param_currentstaffdate", DbAdapter);

                    //Make the Column Names great again.
                    TPartnerReportTools.ConvertDbFieldNamesToReadable(Partners);
                    TPartnerReportTools.ConvertDbFieldNamesToReadable(Locations);

                    //Sort and filter
                    Locations.CaseSensitive = false;
                    DataView dv = Locations.DefaultView;

                    Dictionary <string, string>Mapping = new Dictionary <string, string>();
                    Mapping.Add("PartnerName", "PartnerShortName");
                    Mapping.Add("AddressType", "LocationType");
                    Mapping.Add("Addressvalidfrom", "DateEffective");
                    Mapping.Add("Addressvalidto", "DateGoodUntil");
                    Mapping.Add("FirstAddressLine", "Locality");
                    Mapping.Add("PostCode", "PostalCode");
                    Mapping.Add("ThirdAddressLine", "Address3");
                    Mapping.Add("Country", "CountryCode");
                    dv.Sort =
                        TPartnerReportTools.ColumnMapping(AParameters["param_sortby_readable"].ToString(), Locations.Columns, Mapping,
                            "Brief Address Report");
                    Locations = dv.ToTable();
                });

            ReturnDataSet.Tables.Add(Partners);
            ReturnDataSet.Tables.Add(Locations);

            return ReturnDataSet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="DbAdapter"></param>
        /// <returns></returns>
        [NoRemoting]
        public static DataSet PrintPartner(Dictionary <string, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataSet ReturnDataSet = new DataSet();

            TDBTransaction Transaction = null;
            DataTable Partners = new DataTable("Partners");
            DataTable ClassPerson = new DataTable("ClassPerson");
            DataTable ClassFamily = new DataTable("ClassFamily");
            DataTable ClassOrganisation = new DataTable("ClassOrganisation");
            DataTable ClassBank = new DataTable("ClassBank");
            DataTable ClassChurch = new DataTable("ClassChurch");
            DataTable ClassUnit = new DataTable("ClassUnit");
            DataTable ClassVenue = new DataTable("ClassVenue");
            DataTable Subscriptions = new DataTable("Subscriptions");
            DataTable Relationships = new DataTable("Relationships");
            DataTable Locations = new DataTable("Locations");
            DataTable ContactDetails = new DataTable("ContactDetails");
            DataTable FinanceDetails = new DataTable("FinanceDetails");
            DataTable PartnerInterests = new DataTable("PartnerInterests");
            DataTable PartnerContacts = new DataTable("PartnerContacts");
            DataTable Reminders = new DataTable("Reminders");
            DataTable SpecialTypes = new DataTable("SpecialTypes");

            DataTable PartnerSelectionTable = new DataTable("DataSelection");


            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    #region Get Partner Selection
                    String PartnerSelection = "";

                    if (AParameters["param_selection"].ToString() == "one partner")
                    {
                        PartnerSelection = AParameters["param_partnerkey"].ToInt32().ToString();
                    }
                    else
                    {
                        String SelectionQuery =
                            "SELECT p_partner_key_n FROM m_extract JOIN m_extract_master ON m_extract.m_extract_id_i = m_extract_master.m_extract_id_i WHERE m_extract_name_c = '"
                            +
                            AParameters["param_extract"] + "'";

                        PartnerSelectionTable = DbAdapter.RunQuery(SelectionQuery, "PartnerSelection", Transaction);
                        List <String>PartnerList = new List <string>();

                        foreach (DataRow row in PartnerSelectionTable.Rows)
                        {
                            PartnerList.Add(row[0].ToString());
                        }

                        PartnerSelection = String.Join(",",
                            PartnerList);
                    }

                    #endregion

                    #region Partners
                    String PartnersQuery =
                        @"SELECT
	                                            p_partner_key_n,
	                                            p_partner_class_c AS PartnerClass,
	                                            p_partner_short_name_c AS PartnerShortName,
	                                            p_language_code_c AS LanguageCode,
	                                            p_acquisition_code_c AS AcquisitionCode,
	                                            p_status_code_c AS StatusCode,

	                                            CASE
		                                            WHEN p_restricted_i=0 THEN 'No'
		                                            WHEN p_restricted_i=1 THEN p_user_id_c
		                                            WHEN p_restricted_i=2 THEN p_group_id_c
	                                            END AS p_restricted,
                                                p_partner.p_receipt_letter_frequency_c  AS ReceiptLetterFrequency,
					                            p_partner.p_receipt_each_gift_l  AS ReceiptEachGift,
					                            p_partner.p_anonymous_donor_l  AS Anonymous,
					                            p_partner.p_email_gift_statement_l  AS EmailGiftStatement,
					                            p_partner.p_finance_comment_c  AS FinanceComment

                                            FROM p_partner
                                            WHERE p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    Partners = DbAdapter.RunQuery(PartnersQuery,
                        "Partners",
                        Transaction);
                    #endregion

                    #region ClassPerson
                    String ClassPersonQuery =
                        @"SELECT
                                                    p_partner_key_n,
                                                    p_person.p_title_c AS Title,
	                                                p_person.p_first_name_c AS FirstName,
	                                                p_person.p_middle_name_1_c AS MiddleName,
	                                                p_person.p_family_name_c AS FamilyName,
	                                                p_person.p_family_key_n AS FamilyKey,
	                                                p_person.p_date_of_birth_d AS DOB,
	                                                p_person.p_gender_c AS Gender,
	                                                p_person.p_marital_status_c AS MaritalStatus,
	                                                p_person.p_occupation_code_c AS OccupationCode,
	                                                p_person.p_prefered_name_c AS PreferedName,
	                                                p_person.p_decorations_c AS Decorations
                                                FROM

                                                    p_person
                                                WHERE

                                                    p_person.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkPartnerClassData"].ToBool())
                    {
                        ClassPersonQuery += " LIMIT 0";
                    }

                    ClassPerson = DbAdapter.RunQuery(ClassPersonQuery,
                        "ClassPerson",
                        Transaction);
                    #endregion

                    #region ClassFamily
                    String ClassFamilyQuery =
                        @"SELECT
                                                    p_partner.p_partner_key_n,
                                                    p_partner.p_partner_class_c,
                                                    p_title_c,
                                                    p_first_name_c,
                                                    p_family_name_c,
                                                    p_marital_status_c,
                                                    p_marital_status_since_d,
                                                    p_marital_status_comment_c,
                                                    p_partner_gift_destination.p_field_key_n

                                                FROM p_partner
                                                INNER JOIN p_family ON p_family.p_partner_key_n = p_partner.p_partner_key_n
                                                LEFT JOIN p_partner_gift_destination
                                                                ON(p_family.p_partner_key_n = p_partner_gift_destination.p_partner_key_n
                                                                AND(p_partner_gift_destination.p_date_effective_d <= '"
                        +
                        DateTime.Today.ToString(
                            "yyyy-MM-dd") +
                        @"' AND p_partner_gift_destination.p_date_expires_d IS NULL
                                                                OR p_partner_gift_destination.p_date_expires_d >= '"
                        +
                        DateTime.Today.ToString(
                            "yyyy-MM-dd") +
                        @"' AND p_partner_gift_destination.p_date_effective_d != p_partner_gift_destination.p_date_expires_d))
                                                WHERE p_family.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkPartnerClassData"].ToBool())
                    {
                        ClassFamilyQuery += " LIMIT 0";
                    }

                    ClassFamily = DbAdapter.RunQuery(ClassFamilyQuery,
                        "ClassFamily",
                        Transaction);
                    #endregion

                    #region ClassOrganisation
                    String ClassOrganisationQuery =
                        @"SELECT
                                                            p_organisation.p_partner_key_n,
	                                                        p_organisation.p_organisation_name_c AS OrganisationName,
	                                                        p_organisation.p_business_code_c AS BusinessCode,
	                                                        p_organisation.p_religious_l AS Religious
                                                        FROM

                                                            p_organisation
                                                        WHERE

                                                            p_organisation.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkPartnerClassData"].ToBool())
                    {
                        ClassOrganisationQuery += " LIMIT 0";
                    }

                    ClassOrganisation = DbAdapter.RunQuery(ClassOrganisationQuery,
                        "ClassOrganisation",
                        Transaction);
                    #endregion

                    #region ClassBank

                    /*String ClassBankQuery = "";
                     *
                     * if (!AParameters["param_chkPartnerClassData"].ToBool())
                     * {
                     *  ClassBankQuery += " LIMIT 0";
                     * }
                     *
                     * ClassBank = DbAdapter.RunQuery(ClassBankQuery, "ClassBank", Transaction);*/
                    #endregion

                    #region ClassChurch
                    String ClassChurchQuery =
                        @"SELECT
                                                    p_church.p_partner_key_n,
	                                                p_church.p_church_name_c AS ChurchName,
	                                                p_church.p_denomination_code_c AS Denomination,
	                                                p_church.p_approximate_size_i AS Size,
	                                                p_church.p_accomodation_l AS Accommodation,
	                                                p_church.p_accomodation_type_c AS AccommodationType,
	                                                p_church.p_accomodation_size_i AS AccommodationSize,
	                                                p_church.p_prayer_group_l AS PrayerGroup,
	                                                p_church.p_map_on_file_l AS Map
                                                FROM
                                                    p_church
                                                WHERE
                                                    p_church.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkPartnerClassData"].ToBool())
                    {
                        ClassChurchQuery += " LIMIT 0";
                    }

                    ClassChurch = DbAdapter.RunQuery(ClassChurchQuery,
                        "ClassChurch",
                        Transaction);
                    #endregion

                    #region ClassUnit
                    String ClassUnitQuery =
                        @"SELECT
	                                            p_unit.p_partner_key_n,
	                                            p_unit.p_unit_name_c AS UnitName,
	                                            p_unit.u_unit_type_code_c AS UnitType,
	                                            p_unit.p_outreach_code_c AS OutreachCode,
	                                            p_unit.um_present_i AS PresentStaff,
	                                            p_unit.um_minimum_i AS MinimumStaff,
	                                            p_unit.um_maximum_i AS MaximumStaff,
	                                            p_unit.um_part_timers_i AS PartTimers,
	                                            p_unit.p_description_c AS Description
                                            FROM
	                                            p_unit
                                            WHERE
	                                            p_unit.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkPartnerClassData"].ToBool())
                    {
                        ClassUnitQuery += " LIMIT 0";
                    }

                    ClassUnit = DbAdapter.RunQuery(ClassUnitQuery,
                        "ClassUnit",
                        Transaction);
                    #endregion

                    #region ClassVenue

                    /*String ClassVenueQuery = "";
                     *
                     * if (!AParameters["param_chkPartnerClassData"].ToBool())
                     * {
                     *  ClassVenueQuery += " LIMIT 0";
                     * }
                     *
                     * ClassVenue = DbAdapter.RunQuery(ClassVenueQuery, "ClassVenue", Transaction);*/
                    #endregion

                    #region Subsciptions
                    String SubscriptionsQuery =
                        @"SELECT
                                                    p_subscription.p_partner_key_n,
	                                                p_subscription.p_publication_code_c AS PublicationCode,
	                                                p_subscription.p_subscription_status_c AS SubscriptionStatus,
	                                                p_subscription.p_gratis_subscription_l AS Gratis,
	                                                p_subscription.p_start_date_d AS StartDate,
	                                                p_subscription.p_subscription_renewal_date_d AS RenewalDate,
	                                                p_subscription.p_date_notice_sent_d AS DateNoticeSent,
	                                                p_subscription.p_expiry_date_d AS ExpiryDate,
	                                                p_subscription.p_date_cancelled_d AS DateCancelled,
	                                                p_subscription.p_number_complimentary_i AS CN,
	                                                p_subscription.p_number_issues_received_i AS IR,
	                                                p_subscription.p_publication_copies_i AS PC,
	                                                p_subscription.p_reason_subs_given_code_c AS Given,
	                                                p_subscription.p_reason_subs_cancelled_code_c AS Cancelled
                                                FROM
                                                    p_subscription
                                                WHERE
                                                    p_subscription.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkSubscriptions"].ToBool())
                    {
                        SubscriptionsQuery += " LIMIT 0";
                    }

                    Subscriptions = DbAdapter.RunQuery(SubscriptionsQuery,
                        "Subscriptions",
                        Transaction);
                    #endregion

                    #region Relationships
                    String RelationshipsQuery =
                        @"SELECT
	                                                p_partner_relationship.p_partner_key_n,
	                                                p_partner_relationship.p_relation_key_n  AS RelationKey,
	                                                p_relation.p_relation_description_c AS RelationDescription,
	                                                p_partner.p_partner_short_name_c AS RelationName
                                                FROM
	                                                p_partner_relationship, p_relation, p_partner
                                                WHERE
	                                                p_partner_relationship.p_partner_key_n IN("
                        +
                        PartnerSelection +
                        @")
	                                                AND p_relation.p_relation_name_c = p_partner_relationship.p_relation_name_c
	                                                AND p_partner.p_partner_key_n = p_partner_relationship.p_relation_key_n"                                                                                                                                                    ;

                    if (!AParameters["param_chkRelationships"].ToBool())
                    {
                        RelationshipsQuery += " LIMIT 0";
                    }

                    Relationships = DbAdapter.RunQuery(RelationshipsQuery,
                        "Relationships",
                        Transaction);
                    #endregion

                    #region ContactDetails
                    String ContactDetailsQuery =
                        @"SELECT
                                                    p_partner_key_n,
                                                       CASE WHEN p_specialised_l = false

                                                       THEN p_partner_attribute.p_attribute_type_c
                                                       ELSE p_partner_attribute_type.p_special_label_c
                                                       END AS CONTACTTYPE,
                                                       CASE WHEN p_confidential_l = false THEN
                                                       CASE WHEN(p_value_country_c <> '')

                                                           THEN '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = p_value_country_c) || ' ' || p_value_c

                                                           ELSE p_value_c

                                                       END
                                                       ELSE

                                                          CASE WHEN (SELECT COUNT(*)

                                                                 FROM s_user_group

                                                                 WHERE s_user_id_c = '"
                        +
                        UserInfo.GUserInfo.UserID +
                        @"'

                                                                   AND s_group_id_c = 'ADDRESSCAN') = 0

                                                           THEN '** restricted **'

                                                           ELSE
                                                               CASE WHEN(p_value_country_c <> '')

                                                               THEN '+' || (SELECT p_internat_telephone_code_i FROM p_country WHERE p_country_code_c = p_value_country_c) || ' ' || p_value_c

                                                               ELSE p_value_c

                                                               END
                                                          END
                                                       END AS VALUE,
                                                       p_comment_c AS COMMENT,
                                                       p_current_l AS CURRENT,
                                                       p_no_longer_current_from_d AS NOLONGERCURRENTFROM,
                                                       p_confidential_l AS CONFIDENTIAL,
                                                       p_partner_attribute_category.p_index_i AS PCIndex,
                                                       p_partner_attribute_type.p_index_i AS PTIndex,
                                                       p_partner_attribute.p_index_i AS PAIndex,
                                                       p_primary_l
                                                    FROM

                                                       p_partner_attribute, p_partner_attribute_type, p_partner_attribute_category
                                                    WHERE

                                                    p_partner_key_n IN("
                        +
                        PartnerSelection +
                        @") AND

                                                    p_partner_attribute_type.p_attribute_type_c = p_partner_attribute.p_attribute_type_c AND

                                                    p_partner_attribute_category.p_category_code_c = p_partner_attribute_type.p_category_code_c AND

                                                    p_partner_attribute_category.p_partner_contact_category_l = true
                                                    ORDER BY

                                                    p_partner_key_n,
	                                                PCIndex ASC, PTIndex ASC, PAIndex ASC"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               ;

                    if (!AParameters["param_chkContactDetails"].ToBool())
                    {
                        ContactDetailsQuery += " LIMIT 0";
                    }

                    ContactDetails = DbAdapter.RunQuery(ContactDetailsQuery,
                        "ContactDetails",
                        Transaction);
                    #endregion

                    #region FinanceDetails
                    String FinanceDetailsQuery =
                        @"SELECT
                                                        p_partner_banking_details.p_partner_key_n,
                                                        p_banking_details.p_bank_account_number_c AS AccountNumber,
	                                                    p_banking_details.p_account_name_c AS AccountName,
	                                                    p_bank.p_branch_code_c AS BranchCode,
	                                                    p_bank.p_branch_name_c AS BranchName,
	                                                    p_bank.p_bic_c AS BIC,
	                                                    p_banking_details.p_iban_c AS IBAN,
	                                                    p_banking_details.p_comment_c AS BankingComment
                                                    FROM
                                                        p_banking_details, p_bank, p_partner_banking_details
                                                    WHERE

                                                        p_partner_banking_details.p_partner_key_n IN("
                        +
                        PartnerSelection +
                        @")

                                                        AND p_banking_details.p_banking_details_key_i = p_partner_banking_details.p_banking_details_key_i

                                                        AND p_banking_details.p_banking_type_i != '1'

                                                        AND p_bank.p_partner_key_n = p_banking_details.p_bank_key_n"                                                                                                                                                                                                                                                                                                     ;

                    if (!AParameters["param_chkFinanceDetails"].ToBool())
                    {
                        FinanceDetailsQuery += " LIMIT 0";
                    }

                    FinanceDetails = DbAdapter.RunQuery(FinanceDetailsQuery,
                        "FinanceDetails",
                        Transaction);
                    #endregion

                    #region PartnerInterests
                    String PartnerInterestsQuery =
                        @"SELECT
                                                        p_partner_interest.p_partner_key_n,
                                                        p_interest.p_category_c AS InterestCategory,
	                                                    p_partner_interest.p_interest_c AS Interest,
	                                                    p_partner_interest.p_country_c AS Country,
	                                                    p_partner_interest.p_field_key_n AS FieldKey,
	                                                    p_partner_interest.p_level_i AS InterestLevel,
	                                                    p_partner_interest.p_comment_c AS InterestComment
                                                    FROM

                                                        p_interest, p_partner_interest
                                                    WHERE

                                                        p_partner_interest.p_partner_key_n IN("
                        +
                        PartnerSelection +
                        @")

                                                        AND p_interest.p_interest_c = p_partner_interest.p_interest_c"                               ;

                    if (!AParameters["param_chkInterests"].ToBool())
                    {
                        PartnerInterestsQuery += " LIMIT 0";
                    }

                    PartnerInterests = DbAdapter.RunQuery(PartnerInterestsQuery,
                        "PartnerInterests",
                        Transaction);
                    #endregion

                    #region PartnerContacts
                    String PartnerContactsQuery =
                        @"SELECT
                                                        p_partner_contact.p_partner_key_n,
                                                        p_contact_log.p_contactor_c AS Contactor,
	                                                    p_contact_log.p_contact_code_c AS ContactCode,
	                                                    p_contact_log.s_contact_date_d AS ContactDate,
	                                                    p_contact_log.p_contact_comment_c AS ContactComment
                                                    FROM

                                                        p_partner_contact
                                                    JOIN p_contact_log ON p_partner_contact.p_contact_log_id_i = p_contact_log.p_contact_log_id_i
                                                    WHERE

                                                        p_partner_contact.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkContacts"].ToBool())
                    {
                        PartnerContactsQuery += " LIMIT 0";
                    }

                    PartnerContacts = DbAdapter.RunQuery(PartnerContactsQuery,
                        "PartnerContacts",
                        Transaction);
                    #endregion

                    #region Reminders
                    String RemindersQuery =
                        @"SELECT
                                                p_partner_reminder.p_partner_key_n,
                                                p_partner_reminder.p_partner_key_n AS  ReminderPartnerKey,
	                                            p_partner_reminder.p_next_reminder_date_d AS  Next,
	                                            p_partner_reminder.p_contact_id_i AS  ContactID,
	                                            p_partner_reminder.p_action_type_c AS ActionType,
	                                            p_partner_reminder.p_event_date_d AS Event,
	                                            p_partner_reminder.p_reminder_frequency_i AS ReminderFrequency,
	                                            p_partner_reminder.p_last_reminder_sent_d AS LastReminder,
	                                            p_partner_reminder.p_first_reminder_date_d AS FirstReminder,
	                                            p_partner_reminder.p_reminder_active_l AS ReminderActive,
	                                            p_partner_reminder.p_reminder_reason_c AS ReminderReason,
	                                            p_partner_reminder.p_email_address_c AS ReminderEmail,
	                                            p_partner_reminder.p_comment_c AS ReminderComment
                                            FROM

                                                p_partner_reminder
                                            WHERE

                                                p_partner_reminder.p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkReminders"].ToBool())
                    {
                        RemindersQuery += " LIMIT 0";
                    }

                    Reminders = DbAdapter.RunQuery(RemindersQuery,
                        "Reminders",
                        Transaction);
                    #endregion

                    #region Locations
                    String LocationsQuery =
                        @"SELECT
                                                p_partner_location.p_partner_key_n AS  LocationPartnerKey,
	                                            p_partner_location.p_location_key_i AS LocationKey,
	                                            p_partner_location.p_location_type_c AS LocationType,
	                                            p_partner_location.p_date_effective_d AS DateEffective,
	                                            p_partner_location.p_date_good_until_d AS GoodUntil,
	                                            p_partner_location.p_send_mail_l AS Mailable,
	                                            p_partner_location.p_location_detail_comment_c AS LocationComment,
	                                            p_location.p_locality_c AS Locality,
	                                            p_location.p_street_name_c AS StreetName,
	                                            p_location.p_address_3_c AS Address3,
	                                            p_location.p_city_c AS City,
	                                            p_location.p_postal_code_c AS PostalCode,
	                                            p_location.p_county_c AS County,
	                                            p_location.p_country_code_c AS CountryCode,
	                                            p_country.p_address_order_i AS AddressOrder
                                            FROM
                                                p_partner_location, p_location, p_country
                                            WHERE
                                                p_partner_location.p_partner_key_n IN("
                        +
                        PartnerSelection +
                        @")

                                                AND p_location.p_location_key_i = p_partner_location.p_location_key_i

                                                AND p_country.p_country_code_c = p_location.p_country_code_c"                                                                                                                                                        ;

                    if (!AParameters["param_chkLocations"].ToBool())
                    {
                        LocationsQuery += " LIMIT 0";
                    }

                    Locations = DbAdapter.RunQuery(LocationsQuery,
                        "Locations",
                        Transaction);
                    #endregion

                    #region Special Types
                    String SpecialTypesString =
                        @"SELECT p_partner_key_n, p_partner_type.p_type_code_c, p_type_description_c, p_valid_from_d, p_valid_until_d
                                                FROM p_partner_type
                                                JOIN p_type ON p_partner_type.p_type_code_c=p_type.p_type_code_c
                                                WHERE p_partner_key_n IN("
                        +
                        PartnerSelection + ")";

                    if (!AParameters["param_chkSpecialTypes"].ToBool())
                    {
                        SpecialTypesString += " LIMIT 0";
                    }

                    SpecialTypes = DbAdapter.RunQuery(SpecialTypesString, "SpecialTypes", Transaction);
                    #endregion
                });

            ReturnDataSet.Tables.Add(Partners);
            ReturnDataSet.Tables.Add(ClassPerson);
            ReturnDataSet.Tables.Add(ClassFamily);
            ReturnDataSet.Tables.Add(ClassOrganisation);
            ReturnDataSet.Tables.Add(ClassBank);
            ReturnDataSet.Tables.Add(ClassChurch);
            ReturnDataSet.Tables.Add(ClassUnit);
            ReturnDataSet.Tables.Add(ClassVenue);
            ReturnDataSet.Tables.Add(Subscriptions);
            ReturnDataSet.Tables.Add(Relationships);
            ReturnDataSet.Tables.Add(Locations);
            ReturnDataSet.Tables.Add(ContactDetails);
            ReturnDataSet.Tables.Add(FinanceDetails);
            ReturnDataSet.Tables.Add(PartnerInterests);
            ReturnDataSet.Tables.Add(PartnerContacts);
            ReturnDataSet.Tables.Add(Reminders);
            ReturnDataSet.Tables.Add(SpecialTypes);
            return ReturnDataSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet PartnerByRelationship(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataSet ResultSet = new DataSet();

            DataTable relationship = new DataTable();
            DataTable church = new DataTable();
            DataTable address = new DataTable();
            DataTable organisation = new DataTable();
            DataTable ContactInformation = new DataTable();
            DataTable PersonInformation = new DataTable();
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Boolean useRecipropcal = AParameters["param_use_reciprocal_relationship"].ToBool();
                    String byPartnerField = (useRecipropcal) ?
                                            "PUB_p_partner_relationship.p_relation_key_n"
                                            :
                                            "PUB_p_partner_relationship.p_partner_key_n";
                    String DescrField = (useRecipropcal) ?
                                        "PUB_p_relation.p_reciprocal_description_c"
                                        :
                                        "PUB_p_relation.p_relation_description_c";
                    String paramPartnerSelection = AParameters["param_selection"].ToString();
                    String extraTables = "";
                    String partnerSelection = "";

                    switch (paramPartnerSelection)
                    {
                        case "one partner":
                            String paramPartnerKey = AParameters["param_partnerkey"].ToString();
                            partnerSelection = " AND " + byPartnerField + " = " + paramPartnerKey;
                            break;

                        case "an extract":
                            extraTables = ", PUB_m_extract, PUB_m_extract_master";
                            String paramExtractName = AParameters["param_extract"].ToString();
                            partnerSelection =
                                " AND PUB_m_extract.p_partner_key_n = " + byPartnerField +
                                " AND PUB_m_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i" +
                                " AND PUB_m_extract_master.m_extract_name_c = '" + paramExtractName + "' ";
                            break;

                        case "all current staff": // There's currently no UI for this option, so it can't get called!
                            extraTables = ", PUB_pm_staff_data";
                            String paramCurrentStaffDate = AParameters["param_currentstaffdate"].ToDate().ToString("yyyy-MM-dd");
                            partnerSelection =
                                " AND PUB_pm_staff_data.p_partner_key_n = " + byPartnerField +
                                " AND PUB_pm_staff_data.pm_start_of_commitment_d <= '" + paramCurrentStaffDate + "'" +
                                " AND(PUB_pm_staff_data.pm_end_of_commitment_d >= '" + paramCurrentStaffDate + "'" +
                                " OR PUB.pm_staff_data.pm_end_of_commitment_d IS NULL )";
                            break;
                    }

                    String paramRelationshipTypes = AParameters["param_relationship_types"].ToString();
                    String relationshipSelection = " AND PUB_p_partner_relationship.p_relation_name_c IN ('" +
                                                   paramRelationshipTypes.Replace(",", "','") + "')";

                    Boolean paramActive = AParameters["param_active"].ToBool();
                    String excludeNotActive = paramActive ? " AND Rel1.p_status_code_c = 'ACTIVE'" : "";

                    Boolean paramExcludeNoSolicitations = AParameters["param_exclude_no_solicitations"].ToBool();
                    String excludeNoSolicitations = paramExcludeNoSolicitations ? " AND Rel1.p_no_solicitations_l = 0" : "";

                    String orderBy = " ORDER BY " + byPartnerField;
                    String Query =
                        @"SELECT DISTINCT
                        PUB_p_partner_relationship.p_relation_name_c AS Relationship, "                                            +
                        DescrField +
                        @" AS RelationshipDescr,
                        PUB_p_partner_relationship.p_partner_key_n AS Rel1PartnerKey,
                        PUB_p_partner_relationship.p_relation_key_n AS Rel2PartnerKey,
                        Rel1.p_partner_short_name_c AS Rel1PartnerName,
                        Rel2.p_partner_short_name_c AS Rel2PartnerName,
                        Rel1.p_partner_class_c AS Rel1PartnerClass,
                        Rel2.p_partner_class_c AS Rel2PartnerClass
                        FROM
                        PUB_p_partner_relationship,
                        PUB_p_relation,
                        PUB_p_partner AS Rel1,
                        PUB_p_partner AS Rel2
                        "
                        +
                        extraTables +
                        " WHERE" +
                        " PUB_p_partner_relationship.p_relation_name_c = PUB_p_relation.p_relation_name_c" +
                        " AND Rel1.p_partner_key_n = PUB_p_partner_relationship.p_partner_key_n" +
                        " AND Rel2.p_partner_key_n = PUB_p_partner_relationship.p_relation_key_n" +
                        partnerSelection +
                        relationshipSelection +
                        excludeNotActive +
                        excludeNoSolicitations +
                        orderBy;
                    relationship = DbAdapter.RunQuery(Query, "Relationship", Transaction);

                    if (relationship.Rows.Count == 0)
                    {
                        return; // Returns out of the delegate, to the enclosing method.
                    }

                    //
                    // Get supporting tables, that the client can link via relations.

                    StringBuilder partnerKeysBuilder = new StringBuilder();

                    foreach (DataRow row in relationship.Rows)
                    {
                        partnerKeysBuilder.Append(row["Rel1PartnerKey"].ToString() + "," + row["Rel2PartnerKey"].ToString() + ",");
                    }

                    String partnerKeys = partnerKeysBuilder.ToString().TrimEnd(new char[] { ',' });

                    if (partnerKeys == String.Empty)
                    {
                        partnerKeys = "-1";
                    }

                    Query = "SELECT p_church.p_partner_key_n AS PartnerKey, " +
                            " p_church.p_church_name_c AS ChurchName, " +
                            " p_partner.p_partner_short_name_c AS ChurchContactPersonName, " +
                            " p_church.p_contact_partner_key_n AS ChurchContactPersonKey" +
                            " FROM p_church, p_partner" +
                            " WHERE p_church.p_partner_key_n in (" + partnerKeys + ")" +
                            " AND p_partner.p_partner_key_n = p_church.p_contact_partner_key_n";
                    church = DbAdapter.RunQuery(Query,
                        "Church",
                        Transaction);

                    Query =
                        "SELECT p_partner_key_n, p_title_c, p_first_name_c, p_prefered_name_c, p_family_name_c, p_date_of_birth_d FROM p_person WHERE p_partner_key_n IN ("
                        + partnerKeys + ")";
                    PersonInformation = DbAdapter.RunQuery(Query, "PersonInformation", Transaction);
                    TPartnerReportTools.ConvertDbFieldNamesToReadable(PersonInformation);

                    ContactInformation = TPartnerReportTools.GetPrimaryPhoneFax(partnerKeys.Split(',').ToList <string>(), DbAdapter, true, true);

                    address = TAddressTools.GetBestAddressForPartners(partnerKeys, Transaction, false, true);

                    Query = "SELECT p_organisation.p_partner_key_n AS PartnerKey, " +
                            " p_organisation.p_organisation_name_c AS OrganisationName, " +
                            " p_partner.p_partner_short_name_c AS OrganisationContactPersonName, " +
                            " p_organisation.p_contact_partner_key_n AS OrganisationContactPersonKey" +
                            " FROM p_organisation, p_partner" +
                            " WHERE p_organisation.p_partner_key_n in (" + partnerKeys + ")" +
                            " AND p_partner.p_partner_key_n = p_organisation.p_contact_partner_key_n";
                    organisation = DbAdapter.RunQuery(Query, "Organisation", Transaction);
                });

            /*
             * if (relationship.Rows.Count == 0)
             * {
             *  return null;
             * }
             */

            ResultSet.Merge(ContactInformation);
            ResultSet.Merge(relationship);
            ResultSet.Merge(church);
            ResultSet.Merge(address);
            ResultSet.Merge(organisation);
            ResultSet.Merge(PersonInformation);
            return ResultSet;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="DbAdapter"></param>
        /// <returns></returns>
        [NoRemoting]
        public static DataTable PartnerBySpecialType(Dictionary <string, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable ReturnTable = new DataTable();
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query =
                        @"SELECT DISTINCT
                                        p_partner.p_partner_key_n,
	                                    p_partner.p_partner_short_name_c,
	                                    p_partner.p_partner_class_c
                                        FROM   p_partner, p_partner_location, p_location, p_partner_type as ptype
                                    WHERE
                                        ptype.p_partner_key_n = p_partner.p_partner_key_n
                                        AND NOT p_partner.p_partner_key_n = 0
                                        AND p_partner_location.p_partner_key_n = p_partner.p_partner_key_n
                                        AND p_partner_location.p_location_key_i = p_location.p_location_key_i"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ;

                    Query += " AND ptype.p_type_code_c IN(" + AParameters["param_explicit_specialtypes"].ToString().Replace(",", "','").Insert(0,
                        "'").Insert(AParameters["param_explicit_specialtypes"].ToString().Replace(",", "','").Insert(0, "'").Length, "'") + ") ";


                    Query += TPartnerReportTools.UCExtractChkFilterSQLConditions(AParameters);

                    ReturnTable = DbAdapter.RunQuery(Query, "PartnerBySpecialType", Transaction);

                    //Add Contact Information, Address, Field
                    TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(ReturnTable, 0, DbAdapter);
                    ReturnTable = TAddressTools.GetBestAddressForPartnersAsJoinedTable(ReturnTable, 0, Transaction, false);
                    TPartnerReportTools.AddFieldNameToTable(ReturnTable, 0, AParameters, "param_address_date_valid_on", DbAdapter);

                    //Make the Column Names great again.
                    TPartnerReportTools.ConvertDbFieldNamesToReadable(ReturnTable);

                    //Sort and filter
                    ReturnTable.CaseSensitive = false;
                    DataView dv = ReturnTable.DefaultView;
                    dv.RowFilter = TPartnerReportTools.UCAddressFilterDataViewRowFilter(AParameters);

                    Dictionary <string, string>Mapping = new Dictionary <string, string>();
                    Mapping.Add("PartnerName", "PartnerShortName");
                    Mapping.Add("AddressType", "LocationType");
                    Mapping.Add("Addressvalidfrom", "DateEffective");
                    Mapping.Add("Addressvalidto", "DateGoodUntil");
                    Mapping.Add("FirstAddressLine", "Locality");
                    Mapping.Add("PostCode", "PostalCode");
                    Mapping.Add("ThirdAddressLine", "Address3");
                    Mapping.Add("Country", "CountryCode");
                    dv.Sort =
                        TPartnerReportTools.ColumnMapping(AParameters["param_sortby_readable"].ToString(), ReturnTable.Columns, Mapping,
                            "Partner By Special Type Report");
                    ReturnTable = dv.ToTable();
                });

            return ReturnTable;
        }
    }
}