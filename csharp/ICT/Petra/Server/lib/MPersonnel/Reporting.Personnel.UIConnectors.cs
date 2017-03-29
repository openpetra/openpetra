//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Jakob Englert
//
// Copyright 2004-2016 by OM International
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
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Shared.MReporting;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ict.Petra.Server.MPersonnel.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the Personnel reporting screens
    ///</summary>
    public partial class TPersonnelReportingWebConnector
    {
        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet EmergencyDataReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            String Selection = TPartnerReportTools.GetPartnerKeysAsString(AParameters, DbAdapter);
            String INSelection = " IN (" + Selection + ") ";

            // create new datatables and dataset
            DataTable PersonnelData = new DataTable();
            DataTable Family = new DataTable();
            //DataTable FamilyLink = new DataTable();
            DataTable Passports = new DataTable();
            DataTable Skills = new DataTable();
            DataTable Languages = new DataTable();
            DataTable PersonalDocuments = new DataTable();
            DataTable PartnerAddress = new DataTable();
            DataTable EmergencyContacts = new DataTable();
            DataTable ECAddresses = new DataTable();
            DataTable ECContactDetails = new DataTable();
            DataTable OtherEmergData = new DataTable();
            DataTable ProofOfLife = new DataTable();
            DataTable SpecialNeeds = new DataTable();

            DataSet ReturnSet = new DataSet();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    //Get Personnel Data
                    string Query =
                        @"SELECT
                        --Partner--

                        partner.p_partner_key_n,
	                    partner.p_partner_class_c,
	                    partner.p_addressee_type_code_c,
	                    partner.p_partner_short_name_c,

	                    --Person--

                        p_title_c,
	                    p_first_name_c,
	                    p_prefered_name_c,
	                    p_middle_name_1_c,
	                    p_middle_name_2_c,
	                    p_middle_name_3_c,
	                    p_family_name_c,
	                    p_date_of_birth_d,
	                    p_gender_c,
	                    p_academic_title_c,
	                    p_marital_status_c,
                        p_family_key_n

	
	
                    FROM p_partner AS partner
                    JOIN p_person AS person ON person.p_partner_key_n = partner.p_partner_key_n
                    WHERE partner.p_partner_key_n "
                        +
                        INSelection;
                    PersonnelData = DbAdapter.RunQuery(Query,
                        "PersonnelData",
                        Transaction);

                    //Get Family Table
                    Query =
                        "SELECT p_person.*, p_partner.* FROM p_person JOIN p_partner ON p_person.p_partner_key_n = p_partner.p_partner_key_n WHERE p_family_key_n IN "
                        +
                        "(SELECT p_family_key_n FROM p_person WHERE p_partner_key_n " + INSelection + ") AND p_person.p_partner_key_n " + INSelection;

                    if (!AParameters["param_chkFamilyMembers"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    Family = DbAdapter.RunQuery(Query,
                        "Family",
                        Transaction);

                    //Get Passport
                    Passports = GetPassportTable(Selection,
                        AParameters,
                        DbAdapter,
                        Transaction);

                    //Get Other Emergency Data
                    Query =
                        @"SELECT
                            p_partner_key_n,
                            pm_height_cm_i,
                            pm_weight_kg_n,
                            pm_eye_colour_c,
                            pm_hair_colour_c,
                            pm_facial_hair_c,
                            pm_physical_desc_c,
                            pm_blood_type_c,
                            pm_ethnic_origin_c

                            FROM pm_personal_data

                            WHERE
                            NOT (
                            pm_height_cm_i = 0
                            AND pm_weight_kg_n = 0
                            AND pm_eye_colour_c = NULL
                            AND pm_hair_colour_c = NULL
                            AND pm_facial_hair_c = NULL
                            AND pm_physical_desc_c = NULL
                            AND pm_blood_type_c = NULL
                            AND pm_ethnic_origin_c = NULL)
                            AND p_partner_key_n "
                        +
                        INSelection;

                    if (!AParameters["param_chkOtherEmergencyData"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    OtherEmergData = DbAdapter.RunQuery(Query,
                        "OtherEmergData",
                        Transaction);


                    //Get Proof of Life Questions
                    Query =
                        @"SELECT
                            p_partner_key_n,
                            pm_life_question_1_c,
                            pm_life_answer_1_c,
                            pm_life_question_2_c,
                            pm_life_answer_2_c,
                            pm_life_question_3_c,
                            pm_life_answer_3_c,
                            pm_life_question_4_c,
                            pm_life_answer_4_c

                            FROM pm_personal_data

                            WHERE
                            NOT (
                            pm_life_question_1_c = ''
                            AND pm_life_answer_1_c = ''
                            AND pm_life_question_2_c = ''
                            AND pm_life_answer_2_c = ''
                            AND pm_life_question_3_c = ''
                            AND pm_life_answer_3_c = ''
                            AND pm_life_question_4_c = ''
                            AND pm_life_answer_4_c = '')
                            AND p_partner_key_n "
                        +
                        INSelection;

                    if (!AParameters["param_chkProofQuestion"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    ProofOfLife = DbAdapter.RunQuery(Query,
                        "ProofOfLife",
                        Transaction);

                    //Get Special Needs
                    SpecialNeeds = GetSpecialNeedsTable(Selection, AParameters, DbAdapter, Transaction);

                    //Get Skills
                    Skills = GetSkillsTable(Selection, AParameters, DbAdapter, Transaction);

                    //Get language
                    Languages = GetLanguagesTable(Selection, AParameters, DbAdapter, Transaction);

                    //Get Personal Documents
                    PersonalDocuments = GetPersonalDocumentsTable(Selection, AParameters, DbAdapter, Transaction);

                    //Get Emergengcy Contacts
                    EmergencyContacts = GetEmergencyContactsTable(Selection, AParameters, DbAdapter, Transaction);

                    //Get Addresses for Partner
                    if (AParameters["param_chkAddress"].ToBool())
                    {
                        PartnerAddress = TAddressTools.GetBestAddressForPartners(PersonnelData, 0, Transaction);
                    }

                    //Get Addresses for Emergency Contact
                    ECAddresses = TAddressTools.GetBestAddressForPartners(EmergencyContacts, 1, Transaction);
                    ECAddresses.TableName = "ECAddresses";

                    //Get Contact Details for Emergency Contact
                    ECContactDetails.TableName = "ECContactDetails";
                    ECContactDetails.Columns.Add("ECPartnerKey", typeof(long));
                    ECContactDetails.Columns.Add("PrimaryPhone");
                    ECContactDetails.Columns.Add("PrimaryEmailAddress");
                    ECContactDetails.Columns.Add("FaxNumber");

                    foreach (DataRow dr in ECAddresses.Rows) //ECAddresses because the Emergency Contact is unique here
                    {
                        long ECPartnerKey = long.Parse(dr[0].ToString());
                        string APrimaryPhone = "";
                        string APrimaryEmailAddress = "";
                        string AFaxNumber = "";
                        TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhoneAndFax(ECPartnerKey,
                            out APrimaryPhone, out APrimaryEmailAddress, out AFaxNumber);
                        object[] newContactDetails = { ECPartnerKey, APrimaryPhone, APrimaryEmailAddress, AFaxNumber };
                        ECContactDetails.Rows.Add(newContactDetails);
                    }
                });

            //PartnerAddresses should not be empty or OP will crash
            if (PartnerAddress.Columns.Count == 0)
            {
                String[] Columns =
                {
                    "p_partner_key_n", "p_date_effective_d", "p_date_good_until_d", "p_location_type_c", "p_send_mail_l", "p_site_key_n",
                    "p_location_key_i", "p_building_1_c",
                    "p_building_2_c", "p_street_name_c", "p_locality_c", "p_suburb_c", "p_city_c", "p_county_c", "p_postal_code_c",
                    "p_country_code_c", "p_address_3_c", "p_geo_latitude_n",
                    "p_geo_longitude_n", "p_geo_km_x_i", "p_geo_km_y_i", "p_geo_accuracy_i", "p_restricted_l", "s_date_created_d", "s_created_by_c",
                    "s_date_modified_d", "s_modified_by_c",
                    "s_modification_id_t"
                };
                DataColumn[] DataColumns = new DataColumn[Columns.Length];

                for (int i = 0; i < Columns.Length; i++)
                {
                    DataColumns[i] = new DataColumn(Columns[i]);
                }

                PartnerAddress.Columns.AddRange(DataColumns);
            }

            PartnerAddress.TableName = "PartnerAddress";

            //ECAddresses should not be empty or OP will crash
            if (ECAddresses.Columns.Count == 0)
            {
                String[] Columns =
                {
                    "p_partner_key_n", "p_date_effective_d", "p_date_good_until_d", "p_location_type_c", "p_send_mail_l", "p_site_key_n",
                    "p_location_key_i", "p_building_1_c",
                    "p_building_2_c", "p_street_name_c", "p_locality_c", "p_suburb_c", "p_city_c", "p_county_c", "p_postal_code_c",
                    "p_country_code_c", "p_address_3_c", "p_geo_latitude_n",
                    "p_geo_longitude_n", "p_geo_km_x_i", "p_geo_km_y_i", "p_geo_accuracy_i", "p_restricted_l", "s_date_created_d", "s_created_by_c",
                    "s_date_modified_d", "s_modified_by_c",
                    "s_modification_id_t"
                };
                DataColumn[] DataColumns = new DataColumn[Columns.Length];

                for (int i = 0; i < Columns.Length; i++)
                {
                    DataColumns[i] = new DataColumn(Columns[i]);
                }

                ECAddresses.Columns.AddRange(DataColumns);
            }

            ReturnSet.Tables.Add(PersonnelData);
            ReturnSet.Tables.Add(Family);
            //ReturnSet.Tables.Add(FamilyLink);
            ReturnSet.Tables.Add(Passports);
            ReturnSet.Tables.Add(Skills);
            ReturnSet.Tables.Add(Languages);
            ReturnSet.Tables.Add(PersonalDocuments);
            ReturnSet.Tables.Add(PartnerAddress);
            ReturnSet.Tables.Add(EmergencyContacts);
            ReturnSet.Tables.Add(ECAddresses);
            ReturnSet.Tables.Add(ECContactDetails);
            ReturnSet.Tables.Add(OtherEmergData);
            ReturnSet.Tables.Add(ProofOfLife);
            ReturnSet.Tables.Add(SpecialNeeds);

            return ReturnSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet PersonalDataReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;
            DataTable Person = new DataTable();
            DataTable LocalPartnerData = new DataTable();
            DataTable LocalPersonnelData = new DataTable();
            DataTable JobAssignments = new DataTable();
            DataTable Commitments = new DataTable();
            DataTable Passports = new DataTable();
            DataTable PersonalDocuments = new DataTable();
            DataTable SpecialNeeds = new DataTable();
            DataTable PersonalBudget = new DataTable();
            DataTable Skills = new DataTable();
            DataTable Languages = new DataTable();
            DataTable PreviousExperiences = new DataTable();

            String Selection = TPartnerReportTools.GetPartnerKeysAsString(AParameters, DbAdapter);

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query;

                    Query =
                        @"SELECT p_person.*, p_partner.p_partner_short_name_c FROM p_person JOIN p_partner ON p_partner.p_partner_key_n = p_person.p_partner_key_n
                              WHERE p_person.p_partner_key_n IN ("
                        +
                        Selection + ")";

                    Person = DbAdapter.RunQuery(Query,
                        "Person",
                        Transaction);

                    Query =
                        @"SELECT DISTINCT
                                p_data_label_value_partner.p_partner_key_n,
                                p_data_label.p_text_c,
                                p_data_label.p_data_type_c,
                                p_data_label_value_partner.p_value_char_c,
                                p_data_label_value_partner.p_value_num_n,
                                p_data_label_value_partner.p_value_currency_n,
                                p_data_label_value_partner.p_value_int_i,
                                p_data_label_value_partner.p_value_bool_l,
                                p_data_label_value_partner.p_value_date_d,
                                p_data_label_value_partner.p_value_time_i,
                                p_data_label_value_partner.p_value_partner_key_n,
                                p_data_label_value_partner.p_value_lookup_c
                              FROM
                                p_data_label_use, p_data_label, p_data_label_value_partner
                              WHERE
                                  p_data_label_value_partner.p_partner_key_n IN ( "
                        +
                        Selection +
                        @")
                                AND p_data_label_value_partner.p_data_label_key_i = p_data_label.p_key_i
                                AND p_data_label_use.p_use_c <> 'Personnel'
                                AND p_data_label_use.p_data_label_key_i = p_data_label.p_key_i"                                                                                                                                                                                                                    ;

                    LocalPartnerData = DbAdapter.RunQuery(Query,
                        "LocalPartnerData",
                        Transaction);


                    Query =
                        @"SELECT DISTINCT
                                p_data_label_value_partner.p_partner_key_n,
                                p_data_label.p_text_c,
                                p_data_label.p_data_type_c,
                                p_data_label_value_partner.p_value_char_c,
                                p_data_label_value_partner.p_value_num_n,
                                p_data_label_value_partner.p_value_currency_n,
                                p_data_label_value_partner.p_value_int_i,
                                p_data_label_value_partner.p_value_bool_l,
                                p_data_label_value_partner.p_value_date_d,
                                p_data_label_value_partner.p_value_time_i,
                                p_data_label_value_partner.p_value_partner_key_n,
                                p_data_label_value_partner.p_value_lookup_c
                              FROM
                                p_data_label_use, p_data_label, p_data_label_value_partner
                              WHERE
                                  p_data_label_value_partner.p_partner_key_n IN("
                        +
                        Selection +
                        @")
                                AND p_data_label_value_partner.p_data_label_key_i = p_data_label.p_key_i
                                AND p_data_label_use.p_use_c = 'Personnel'
                                AND p_data_label_use.p_data_label_key_i = p_data_label.p_key_i"                                                                                                                                                                                                                   ;

                    LocalPersonnelData = DbAdapter.RunQuery(Query, "LocalPersonnelData", Transaction);


                    JobAssignments = GetJobAssignmentsTable(Selection,
                        AParameters,
                        DbAdapter,
                        Transaction);


                    Query =
                        @"SELECT DISTINCT
                                pm_staff_data.p_partner_key_n,
					            pm_staff_data.pm_start_of_commitment_d,
					            pm_staff_data.pm_end_of_commitment_d,
					            pm_staff_data.pm_status_code_c,
					            receiving.p_partner_short_name_c AS receiving_field,
					            sending.p_partner_short_name_c AS office_recruited_by,
					            pm_staff_data.pm_home_office_n,
					            pm_staff_data.pm_staff_data_comments_c
				              FROM
					            pm_staff_data
					            LEFT JOIN p_partner AS receiving ON pm_staff_data.pm_receiving_field_n = receiving.p_partner_key_n
					            LEFT JOIN p_partner AS sending ON pm_staff_data.pm_office_recruited_by_n = sending.p_partner_key_n
				             WHERE
					            pm_staff_data.p_partner_key_n IN("
                        +
                        Selection + ")";

                    Commitments = DbAdapter.RunQuery(Query, "Commitments", Transaction);

                    Passports = GetPassportTable(Selection, AParameters, DbAdapter, Transaction);

                    PersonalDocuments = GetPersonalDocumentsTable(Selection, AParameters, DbAdapter, Transaction);

                    SpecialNeeds = GetSpecialNeedsTable(Selection, AParameters, DbAdapter, Transaction);

                    Skills = GetSkillsTable(Selection, AParameters, DbAdapter, Transaction);

                    Languages = GetLanguagesTable(Selection, AParameters, DbAdapter, Transaction);

                    PreviousExperiences = PreviousExperience(AParameters, DbAdapter, Selection);
                });


            DataSet ReturnSet = new DataSet();
            ReturnSet.Tables.Add(Person);
            ReturnSet.Tables.Add(LocalPartnerData);
            ReturnSet.Tables.Add(LocalPersonnelData);
            ReturnSet.Tables.Add(JobAssignments);
            ReturnSet.Tables.Add(Commitments);
            ReturnSet.Tables.Add(Passports);
            ReturnSet.Tables.Add(PersonalDocuments);
            ReturnSet.Tables.Add(SpecialNeeds);
            ReturnSet.Tables.Add(PersonalBudget);
            ReturnSet.Tables.Add(Skills);
            ReturnSet.Tables.Add(Languages);
            ReturnSet.Tables.Add(PreviousExperiences);

            TPartnerReportTools.ConvertDbFieldNamesToReadable(ReturnSet);
            return ReturnSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable PreviousExperience(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            return PreviousExperience(AParameters, DbAdapter, TPartnerReportTools.GetPartnerKeysAsString(AParameters, DbAdapter), true);
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable PreviousExperience(Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter DbAdapter,
            String ASelection,
            bool AUseOrderBy = false)
        {
            string date = "";

            if (AParameters["param_currentstaffdate"].ToString() != String.Empty)
            {
                date = AParameters["param_currentstaffdate"].ToDate().ToString("yyyy-MM-dd");
            }

            TDBTransaction Transaction = null;

            DataTable PreviousExperience = new DataTable();
            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT DISTINCT
                                            p_partner.p_partner_key_n AS PartnerKey,
	                                        p_partner.p_partner_short_name_c AS PartnerName,
	                                        pm_past_experience.pm_start_date_d AS StartDate,
	                                        pm_past_experience.pm_end_date_d AS EndDate,
	                                        pm_past_experience.pm_prev_location_c AS Location,
	                                        pm_past_experience.pm_prev_role_c AS Role,
	                                        pm_past_experience.pm_other_organisation_c AS Organisation,
	                                        pm_past_experience.pm_prev_work_here_l AS PrevWorkHere,
                                            p_type_code_c AS Type,
                                            pm_past_exp_comments_c AS Comment
                                        FROM
                                            p_partner
                                            LEFT JOIN pm_past_experience ON p_partner.p_partner_key_n = pm_past_experience.p_partner_key_n
                                            LEFT JOIN (SELECT MAX(p_type_code_c) p_type_code_c, p_partner_key_n FROM p_partner_type
                                            WHERE p_type_code_c LIKE 'OMER%' OR p_type_code_c LIKE 'EX-OMER%' GROUP BY p_partner_key_n) AS type
                                            ON p_partner.p_partner_key_n = type.p_partner_key_n
                                        WHERE
                                            p_partner.p_partner_key_n IN("
                        +
                        ASelection + ") ";

                    if (AUseOrderBy)
                    {
                        Query += " ORDER BY " + AParameters["param_sortby_readable"].ToString().Replace(" ", "");
                    }

                    PreviousExperience = DbAdapter.RunQuery(Query, "PreviousExperience", Transaction);
                });

            return PreviousExperience;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable PassportExpiryReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable PassportExpiryReport = new DataTable();

            TDBTransaction Transaction = null;

            DataTable PreviousExperience = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    Dictionary <string, string>ColumnNames = new Dictionary <string, string>();
                    ColumnNames.Add("p_partner.p_partner_key_n", "PartnerKey");
                    ColumnNames.Add("p_partner.p_partner_short_name_c", "PartnerName");
                    ColumnNames.Add("p_type_code_c", "PartnerType");
                    ColumnNames.Add("pm_passport_details.pm_date_of_expiration_d", "PassportExpiryDate");
                    ColumnNames.Add("pm_passport_details.pm_passport_number_c", "PassportNumber");
                    ColumnNames.Add("pm_passport_details.pm_passport_details_type_c", "PassportType");
                    ColumnNames.Add("pm_passport_details.p_passport_nationality_code_c", "PassportNationality");
                    ColumnNames.Add("pm_passport_details.p_country_of_issue_c", "CountryofIssue");
                    ColumnNames.Add("pm_passport_details.pm_passport_dob_d", "PassportDateofBirth");
                    ColumnNames.Add("pm_passport_details.pm_date_of_issue_d", "PassportDateofIssue");
                    ColumnNames.Add("pm_passport_details.pm_full_passport_name_c", "PassportName");
                    ColumnNames.Add("pm_passport_details.pm_place_of_birth_c", "PassportPlaceofBirth");
                    ColumnNames.Add("pm_passport_details.pm_place_of_issue_c", "PassportPlaceofIssue");
                    ColumnNames.Add("p_person.p_first_name_c", "PersonFirstName");
                    ColumnNames.Add("p_person.p_prefered_name_c", "PersonPreferedName");
                    ColumnNames.Add("p_person.p_middle_name_1_c", "PersonMiddleName");
                    ColumnNames.Add("p_person.p_family_name_c", "PersonLastName");
                    ColumnNames.Add("p_person.p_gender_c", "Gender");
                    ColumnNames.Add("p_person.p_date_of_birth_d", "PersonDateofBirth");
                    ColumnNames.Add("p_person.p_occupation_code_c", "Occupation");
                    ColumnNames.Add("p_occupation.p_occupation_description_c", "OccupationDescription");
                    ColumnNames.Add("p_country.p_country_name_c", "CountryName");

                    string Query = "SELECT DISTINCT " + TPartnerReportTools.ReplaceColumnWithNullWhenUnused(AParameters,
                        ColumnNames) +
                                   @"

                                    FROM
                                        p_partner,
	                                    p_person,
	                                    pm_passport_details,
                                        pm_staff_data,
	                                    p_country,
                                        p_occupation
                                        LEFT JOIN (SELECT MAX(p_type_code_c) p_type_code_c, p_partner_key_n FROM p_partner_type
                                            WHERE p_type_code_c LIKE 'OMER%' OR p_type_code_c LIKE 'EX-OMER%' GROUP BY p_partner_key_n) AS type
                                            ON TRUE
                                      WHERE

                                        pm_passport_details.p_partner_key_n = p_partner.p_partner_key_n

                                        AND p_occupation.p_occupation_code_c = p_person.p_occupation_code_c

                                        AND p_partner.p_partner_key_n = type.p_partner_key_n

                                        AND p_person.p_partner_key_n = p_partner.p_partner_key_n

                                        AND(p_country.p_country_code_c = pm_passport_details.p_passport_nationality_code_c

                                            OR(pm_passport_details.p_passport_nationality_code_c IS NULL

                                                AND p_country.p_country_code_c = '99'))

                                        AND p_partner.p_partner_key_n IN("
                                   +
                                   TPartnerReportTools.GetPartnerKeysAsString(AParameters,
                        DbAdapter) + ") ORDER BY " + AParameters["param_sortby_readable"].ToString().Replace(" ", "");;

                    PassportExpiryReport = DbAdapter.RunQuery(Query, "PassportExpiryReport", Transaction);

                    TPartnerReportTools.AddFieldNameToTable(PassportExpiryReport, 0, AParameters, "param_currentstaffdate", DbAdapter);
                });

            return PassportExpiryReport;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable ProgressReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable ProgressReport = new DataTable();

            TDBTransaction Transaction = null;

            DataTable PreviousExperience = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query =
                        @"SELECT DISTINCT
	                                    person.p_partner_key_n AS PartnerKey,
	                                    partner.p_partner_short_name_c AS PartnerName,
	                                    evaluation.pm_evaluation_date_d AS ReportDate,
	                                    evaluation.pm_evaluation_type_c AS ReportType,
	                                    evaluation.pm_evaluator_c AS Reporter,
	                                    evaluation.pm_next_evaluation_date_d AS NextReportDate,
	                                    evaluation.pm_evaluation_comments_c AS ReportComment
                                    FROM p_person AS person,
	                                    p_partner AS partner,
	                                    pm_person_evaluation AS evaluation
                                    WHERE
	                                    person.p_partner_key_n = partner.p_partner_key_n
	                                    AND person.p_partner_key_n = evaluation.p_partner_key_n
	                                    AND partner.p_status_code_c = 'ACTIVE' AND person.p_partner_key_n
                                    IN("
                        +
                        TPartnerReportTools.GetPartnerKeysAsString(AParameters,
                            DbAdapter) + ") ORDER BY " + AParameters["param_sortby_readable"].ToString().Replace(" ", "");;

                    ProgressReport = DbAdapter.RunQuery(Query, "ProgressReport", Transaction);
                });

            return ProgressReport;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable EndOfCommitmentReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable EndofCommitment = new DataTable();

            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query =
                        @"SELECT DISTINCT person.p_partner_key_n AS PartnerKey,
                                       partner.p_partner_short_name_c AS PartnerName,
                                       staff.pm_end_of_commitment_d AS EndDate,
                                       staff.pm_start_of_commitment_d AS StartDate,
                                       staff.pm_status_code_c AS CommitmentStatus,
                                       unit.p_unit_name_c AS FieldName,
                                       string_agg(p_type_code_c, ' / ') AS PartnerType
                                    FROM p_person AS person,
	                                    p_partner AS partner,
	                                    pm_staff_data AS staff
					                LEFT JOIN p_unit AS unit
						                ON staff.pm_receiving_field_n = unit.p_partner_key_n
						            LEFT JOIN p_partner_type AS pptype ON TRUE
	                                WHERE
	                                    person.p_partner_key_n = partner.p_partner_key_n
	                                    AND pptype.p_partner_key_n = person.p_partner_key_n
							            AND  ( pptype.p_type_code_c LIKE 'EX-OMER%' OR
								            pptype.p_type_code_c LIKE 'OMER%' OR
								            pptype.p_type_code_c LIKE 'ASSOC%' )
	                                    AND person.p_partner_key_n = staff.p_partner_key_n
	                                    AND (staff.pm_end_of_commitment_d >= '"
                        +
                        AParameters["param_today"].ToDate().ToString(
                            "yyyy-MM-dd") +
                        @"'
		                                    OR staff.pm_end_of_commitment_d IS NULL)
		                                AND person.p_partner_key_n IN("
                        +
                        TPartnerReportTools.GetPartnerKeysAsString(AParameters,
                            DbAdapter) +
                        @")
                                    GROUP BY person.p_partner_key_n,
                                            staff.pm_end_of_commitment_d,
                                            staff.pm_start_of_commitment_d,
                                            staff.pm_status_code_c,
                                            partner.p_partner_short_name_c,
                                            unit.p_unit_name_c
                                    ORDER BY "
                        +
                        AParameters["param_sortby_readable"].ToString().Replace(" ", "");


                    EndofCommitment = DbAdapter.RunQuery(Query, "EndofCommitment", Transaction);
                    EndofCommitment = TAddressTools.GetBestAddressForPartnersAsJoinedTable(EndofCommitment, 0, Transaction);
                    TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(EndofCommitment, 0, DbAdapter);
                });

            return EndofCommitment;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet EmergencyContactReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable Person = new DataTable();
            DataTable EmergencyContacts = new DataTable();

            string Selection = TPartnerReportTools.GetPartnerKeysAsString(AParameters, DbAdapter);
            TDBTransaction Transaction = null;

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query = "SELECT p_partner_key_n, p_partner_short_name_c FROM p_partner WHERE p_partner_key_n IN(" + Selection + ")";
                    Person = DbAdapter.RunQuery(Query, "Person", Transaction);
                    EmergencyContacts = GetEmergencyContactsTable(Selection, AParameters, DbAdapter, Transaction);
                    TAddressTools.AddBestAddressForPartner(ref EmergencyContacts, 1, Transaction);
                    TAddressTools.AddBestAddressForPartner(ref Person, 0, Transaction);
                });
            TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(EmergencyContacts, 1, DbAdapter, true, true);
            DataSet ReturnSet = new DataSet();
            ReturnSet.Tables.Add(Person);
            ReturnSet.Tables.Add(EmergencyContacts);
            TPartnerReportTools.ConvertDbFieldNamesToReadable(ReturnSet);
            return ReturnSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable JobAssignmentReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            DataTable JobAssignments = new DataTable();

            String Selection = TPartnerReportTools.GetPartnerKeysAsString(AParameters, DbAdapter);

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    JobAssignments = GetJobAssignmentsTable(Selection, AParameters, DbAdapter, Transaction);
                    TAddressTools.AddBestAddressForPartner(ref JobAssignments, 0, Transaction);
                });
            TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(JobAssignments, 0, DbAdapter);
            TPartnerReportTools.ConvertDbFieldNamesToReadable(JobAssignments);

            DataView dv = JobAssignments.DefaultView;

            Dictionary <string, string>Mapping = new Dictionary <string, string>();
            Mapping.Add("PartnerName", "PartnerShortName");
            Mapping.Add("RoleKey", "PositionName");
            Mapping.Add("Assistant", "AssistantTo");
            Mapping.Add("StartDate", "FromDate");
            Mapping.Add("AddressLine1", "Locality");
            Mapping.Add("AddressStreet", "StreetName");
            Mapping.Add("AddressLine3", "Address3");
            Mapping.Add("AddressPostCode", "PostalCode");
            Mapping.Add("AddressCity", "City");
            Mapping.Add("Address State/County/Province", "County");
            Mapping.Add("AddressCountry", "Country");
            dv.Sort = TPartnerReportTools.ColumnMapping(AParameters["param_sortby_readable"].ToString(), JobAssignments.Columns, Mapping);
            JobAssignments = dv.ToTable();

            return JobAssignments;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable StartOfCommitmentReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            DataTable StartOfCommitment = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT DISTINCT
	                                    person.p_partner_key_n,
	                                    p_partner.p_partner_class_c,
	                                    staff.pm_start_of_commitment_d,
	                                    staff.pm_end_of_commitment_d,
	                                    staff.pm_status_code_c,
	                                    partner.p_partner_short_name_c,
                                        unit.p_unit_name_c AS FieldName
                                    FROM
	                                    p_person AS person,
	                                    p_partner AS partner,
	                                    pm_staff_data AS staff,
                                        p_unit AS unit,
	                                    p_partner
                                    WHERE
	                                    person.p_partner_key_n = partner.p_partner_key_n
	                                    AND person.p_partner_key_n = staff.p_partner_key_n
                                        AND staff.pm_receiving_field_n = unit.p_partner_key_n
	                                    AND partner.p_status_code_c = 'ACTIVE'
	                                    AND staff.pm_start_of_commitment_d >= '"
                        +
                        AParameters["param_dtpStartDate"].ToDate().ToString(
                            "yyyy-MM-dd") + @"'
	                                    AND staff.pm_start_of_commitment_d <= '"                                           +
                        AParameters["param_dtpEndDate"].ToDate().ToString(
                            "yyyy-MM-dd") + @"'
	                                    AND p_partner.p_partner_key_n = person.p_partner_key_n"                                          ;

                    if (AParameters["param_chkSelectedStatus"].ToBool())
                    {
                        String status = "";

                        if (AParameters.ContainsKey("param_commitmentstatuses"))
                        {
                            status = AParameters["param_commitmentstatuses"].ToString();
                        }

                        Query += " AND (staff.pm_status_code_c IN('" + status.Replace(",", "','") + "') ";
                    }

                    if (AParameters["param_chkNoSelectedStatus"].ToBool())
                    {
                        if (AParameters["param_chkSelectedStatus"].ToBool())
                        {
                            Query += " OR ";
                        }
                        else
                        {
                            Query += " AND ";
                        }

                        Query += " (staff.pm_status_code_c = '' OR staff.pm_status_code_c IS NULL)";
                    }

                    if (AParameters["param_chkSelectedStatus"].ToBool())
                    {
                        Query += " ) ";
                    }

                    StartOfCommitment = DbAdapter.RunQuery(Query, "StartOfCommitment", Transaction);
                    TAddressTools.AddBestAddressForPartner(ref StartOfCommitment, 0, Transaction, false, true);
                });

            TPartnerReportTools.AddPrimaryPhoneEmailFaxToTable(StartOfCommitment, 0, DbAdapter, true, true, true);

            TPartnerReportTools.ConvertDbFieldNamesToReadable(StartOfCommitment);

            DataView dv = StartOfCommitment.DefaultView;
            Dictionary <string, string>Mapping = new Dictionary <string, string>();
            Mapping.Add("PartnerName", "PartnerShortName");
            Mapping.Add("FaxNumber", "Fax");
            Mapping.Add("FieldName", "fieldname");
            Mapping.Add("CommitmentType", "StatusCode");
            Mapping.Add("PartnerAddress", "FullAddress");
            Mapping.Add("StartDate", "StartOfCommitment");
            Mapping.Add("EndDate", "EndOfCommitment");
            dv.Sort = TPartnerReportTools.ColumnMapping(AParameters["param_sortby_readable"].ToString(), StartOfCommitment.Columns, Mapping);
            StartOfCommitment = dv.ToTable();


            return StartOfCommitment;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable UnitHierarchyReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            DataTable UnitHierarchy = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query =
                        @"WITH RECURSIVE breadth_first_traversal
                                       (
                                         um_child_unit_key_n,
                                         um_parent_unit_key_n
                                          )
                                    AS ( SELECT
                                                um_unit_structure.um_child_unit_key_n           AS um_child_unit_key_n,
                                                um_unit_structure.um_parent_unit_key_n            AS um_parent_unit_key_n
                                           FROM um_unit_structure
                                          WHERE
                                          um_unit_structure.um_child_unit_key_n = "
                        +
                        AParameters["param_txtUnitCode"].ToInt32() +
                        @"
                                       UNION ALL
                                         SELECT
                                                um_unit_structure.um_child_unit_key_n,
                                                um_unit_structure.um_parent_unit_key_n
                                           FROM breadth_first_traversal
                                                INNER JOIN um_unit_structure
                                                        ON um_unit_structure.um_parent_unit_key_n = breadth_first_traversal.um_child_unit_key_n
                                          WHERE um_unit_structure.um_parent_unit_key_n <> um_unit_structure.um_child_unit_key_n )
                                    SELECT um_parent_unit_key_n,
	                                    um_child_unit_key_n,
	                                    p_unit_name_c,
                                        p_unit.u_unit_type_code_c,
                                        u_unit_type_name_c
                                      FROM breadth_first_traversal
                                           JOIN p_unit ON p_partner_key_n = um_child_unit_key_n
                                           LEFT JOIN u_unit_type ON p_unit.u_unit_type_code_c = u_unit_type.u_unit_type_code_c "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 ;

                    if (!AParameters["param_chkInclude"].ToBool())
                    {
                        Query +=
                            @" WHERE p_unit.u_unit_type_code_c NOT LIKE 'GA%'
                                    AND p_unit.u_unit_type_code_c NOT LIKE 'GC%'
                                    AND p_unit.u_unit_type_code_c NOT LIKE 'TN%'
                                    AND p_unit.u_unit_type_code_c NOT LIKE 'TS%'"                                                                                                                                                                                                                                                   ;
                    }

                    UnitHierarchy = DbAdapter.RunQuery(Query, "UnitHierarchy", Transaction);
                });
            TPartnerReportTools.ConvertDbFieldNamesToReadable(UnitHierarchy);

            return UnitHierarchy;
        }

        private static DataTable GetSkillsTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            //Get Skills
            String Query =
                @"SELECT
                            p_partner_key_n,
                            pm_skill_category_code_c,
                            pm_description_english_c,
                            pm_description_local_c,
                            pm_description_language_c,
                            pm_skill_level_i,
                            pm_years_of_experience_i,
                            pm_years_of_experience_as_of_d,
                            pm_professional_skill_l,
                            pm_current_occupation_l,
                            pm_degree_c,
                            pm_year_of_degree_i,
                            pm_comment_c,
                            pt_description_c

                            FROM pm_person_skill
                            JOIN pt_skill_level ON pm_person_skill.pm_skill_level_i = pt_skill_level.pt_level_i

                            WHERE p_partner_key_n IN("
                +
                ASelection + ") ";

            if (!AParameters["param_chkSkills"].ToBool())
            {
                Query += " LIMIT 0";
            }

            return ADbAdapter.RunQuery(Query, "Skills", ATransaction);
        }

        private static DataTable GetPassportTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT
                            p_partner_key_n,
	                        pm_passport_number_c,
	                        pm_main_passport_l,
	                        pm_active_flag_c,
	                        pm_full_passport_name_c,
	                        pm_passport_dob_d,
	                        pm_place_of_birth_c,
	                        p_passport_nationality_code_c,
	                        pm_date_of_expiration_d,
	                        pm_place_of_issue_c,
	                        p_country_of_issue_c,
	                        pm_date_of_issue_d,
	                        pm_passport_details_type_c

                        FROM pm_passport_details

                        WHERE p_partner_key_n IN("
                +
                ASelection + ") ";

            if (!AParameters["param_chkPassport"].ToBool())
            {
                Query += " LIMIT 0";
            }

            return ADbAdapter.RunQuery(Query, "Passports", ATransaction);
        }

        private static DataTable GetPersonalDocumentsTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT
                            p_partner_key_n,
                            pm_doc_code_c,
                            pm_document_id_c,
                            pm_place_of_issue_c,
                            pm_date_of_issue_d,
                            pm_date_of_start_d,
                            pm_date_of_expiration_d,
                            pm_doc_comment_c,
                            pm_assoc_doc_id_c


                            FROM pm_document WHERE p_partner_key_n IN("
                +
                ASelection + ") ";

            if (!AParameters["param_chkPersonalDocuments"].ToBool())
            {
                Query += " LIMIT 0";
            }

            return ADbAdapter.RunQuery(Query, "PersonalDocuments", ATransaction);
        }

        private static DataTable GetSpecialNeedsTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT
                            p_partner_key_n,
                            pm_medical_comment_c,
                            pm_dietary_comment_c,
                            pm_other_special_need_c,
                            pm_vegetarian_flag_l

                            FROM pm_special_need

                            WHERE p_partner_key_n IN( "
                +
                ASelection + ") ";

            if (!AParameters["param_chkSpecialNeeds"].ToBool())
            {
                Query += " LIMIT 0";
            }

            return ADbAdapter.RunQuery(Query, "SpecialNeeds", ATransaction);
        }

        private static DataTable GetLanguagesTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT
                            p_partner_key_n,
                            pm_person_language.p_language_code_c,
                            p_language_description_c,
                            pm_person_language.pm_years_of_experience_i,
                            pm_person_language.pm_years_of_experience_as_of_d,
                            pm_person_language.pt_language_level_i,
                            pm_comment_c,
                            pt_language_level_descr_c,
                            pt_language_comment_c

                            FROM pm_person_language
                            JOIN pt_language_level ON pt_language_level.pt_language_level_i = pm_person_language.pt_language_level_i
                            JOIN p_language ON p_language.p_language_code_c = pm_person_language.p_language_code_c
                            WHERE
                            p_partner_key_n IN ("
                +
                ASelection + ") ";

            if (!AParameters["param_chkLanguages"].ToBool())
            {
                Query += " LIMIT 0";
            }

            return ADbAdapter.RunQuery(Query, "Languages", ATransaction);
        }

        private static DataTable GetEmergencyContactsTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT
                            p_relation_key_n AS partnerKey,
                            p_partner_relationship.p_partner_key_n AS EmergencyContactPartnerKey,
                            p_relation_name_c,
                            p_partner_relationship.p_comment_c,
                            p_partner_class_c AS ECPartnerClass,
                            p_partner_short_name_c AS ECShortName

                            FROM p_partner_relationship
                            JOIN p_partner ON p_partner.p_partner_key_n = p_partner_relationship.p_partner_key_n
                            WHERE p_relation_name_c LIKE 'EMER%' AND p_relation_key_n IN("
                +
                ASelection + ") ";

            if (AParameters.ContainsKey("param_chkEmergencyContacts"))
            {
                if (!AParameters["param_chkEmergencyContacts"].ToBool())
                {
                    Query += " LIMIT 0";
                }
            }

            return ADbAdapter.RunQuery(Query, "EmergencyContacts", ATransaction);
        }

        private static DataTable GetJobAssignmentsTable(String ASelection,
            Dictionary <String, TVariant>AParameters,
            TReportingDbAdapter ADbAdapter,
            TDBTransaction ATransaction)
        {
            String Query =
                @"SELECT DISTINCT
	                            pm_job_assignment.p_partner_key_n,
	                            partner.p_partner_short_name_c,
	                            pm_job_assignment.pt_position_name_c,
	                            pm_job_assignment.pm_from_date_d,
	                            pm_job_assignment.pm_to_date_d,
	                            pm_job_assignment.pt_assistant_to_l,
	                            pt_assignment_type.pt_assignment_code_descr_c,
	                            unit.p_partner_short_name_c AS Field_Name,
	                            position.pt_position_descr_c AS Role_Name
                            FROM
	                            pm_job_assignment
	                            LEFT JOIN pt_position AS position ON position.pt_position_name_c = pm_job_assignment.pt_position_name_c
	                            LEFT JOIN pt_assignment_type ON pt_assignment_type.pt_assignment_type_code_c = pm_job_assignment.pt_assignment_type_code_c
	                            LEFT JOIN p_partner AS unit ON unit.p_partner_key_n = pm_job_assignment.pm_unit_key_n
                                LEFT JOIN p_partner AS partner ON pm_job_assignment.p_partner_key_n = partner.p_partner_key_n
                              WHERE
                                pm_job_assignment.p_partner_key_n IN("
                +
                ASelection + @")";

            return ADbAdapter.RunQuery(Query, "JobAssignments", ATransaction);
        }
    }
}