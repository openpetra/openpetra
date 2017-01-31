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
            bool Extract = false;

            if ((AParameters["param_extract"].ToString() != "") && (AParameters["param_selection"].ToString() == "an extract"))
            {
                Extract = true;
            }

            //Determine Selection
            String Selection = "";

            if (Extract)
            {
                Selection =
                    " IN (SELECT p_partner_key_n FROM m_extract  WHERE m_extract_id_i = (SELECT m_extract_id_i FROM m_extract_master WHERE m_extract_name_c = '"
                    +
                    AParameters["param_extract"] + "'))";
            }
            else if (AParameters["param_selection"].ToString() == "all current staff")
            {
                DataTable Staff = new DataTable();
                String StaffAsString = "";

                DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    ref Transaction,
                    delegate
                    {
                        string date = AParameters["param_currentstaffdate"].ToDate().ToString("yyyy-MM-dd");
                        string staffQuery = "SELECT p_partner_key_n FROM pm_staff_data WHERE pm_start_of_commitment_d <= '" + date +
                                            "' AND (pm_end_of_commitment_d >= '" + date + "' OR pm_end_of_commitment_d IS NULL)";

                        Staff = DbAdapter.RunQuery(staffQuery, "Staff", Transaction);
                    });

                List <String>StaffList = new List <string>();

                foreach (DataRow dr in Staff.Rows)
                {
                    StaffList.Add(dr[0].ToString());
                }

                StaffAsString = String.Join(",", StaffList);

                Selection = " IN (" + StaffAsString + ") ";
            }
            else
            {
                Selection = " IN (" + AParameters["param_partnerkey"] + ") ";
            }

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
                        Selection;
                    PersonnelData = DbAdapter.RunQuery(Query,
                        "PersonnelData",
                        Transaction);

                    //Get Family Table
                    Query =
                        "SELECT p_person.*, p_partner.* FROM p_person JOIN p_partner ON p_person.p_partner_key_n = p_partner.p_partner_key_n WHERE p_family_key_n IN "
                        +
                        "(SELECT p_family_key_n FROM p_person WHERE p_partner_key_n " + Selection + ") AND p_person.p_partner_key_n " + Selection;

                    if (!AParameters["param_chkFamilyMembers"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    Family = DbAdapter.RunQuery(Query,
                        "Family",
                        Transaction);

                    //Get Passport
                    Query =
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

                        WHERE p_partner_key_n"
                        +
                        Selection;

                    if (!AParameters["param_chkPassport"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    Passports = DbAdapter.RunQuery(Query,
                        "Passports",
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
                        Selection;

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
                        Selection;

                    if (!AParameters["param_chkProofQuestion"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    ProofOfLife = DbAdapter.RunQuery(Query,
                        "ProofOfLife",
                        Transaction);

                    //Get Special Needs
                    Query =
                        @"SELECT
                            p_partner_key_n,
                            pm_medical_comment_c,
                            pm_dietary_comment_c,
                            pm_other_special_need_c,
                            pm_vegetarian_flag_l

                            FROM pm_special_need

                            WHERE p_partner_key_n "
                        +
                        Selection;

                    if (!AParameters["param_chkSpecialNeeds"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    SpecialNeeds = DbAdapter.RunQuery(Query,
                        "SpecialNeeds",
                        Transaction);

                    //Get Skills
                    Query =
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

                            WHERE p_partner_key_n "
                        +
                        Selection;

                    if (!AParameters["param_chkSkills"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    Skills = DbAdapter.RunQuery(Query,
                        "Skills",
                        Transaction);

                    //Get language
                    Query =
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
                            p_partner_key_n "
                        +
                        Selection;

                    if (!AParameters["param_chkLanguages"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    Languages = DbAdapter.RunQuery(Query,
                        "Languages",
                        Transaction);

                    //Get Personal Documents
                    Query =
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


                            FROM pm_document WHERE p_partner_key_n "
                        +
                        Selection;

                    if (!AParameters["param_chkPersonalDocuments"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    PersonalDocuments = DbAdapter.RunQuery(Query,
                        "PersonalDocuments",
                        Transaction);

                    //Get Emergengcy Contacts
                    Query =
                        @"SELECT
                            p_relation_key_n AS partnerKey,
                            p_partner_relationship.p_partner_key_n AS EmergencyContactPartnerKey,
                            p_relation_name_c,
                            p_partner_relationship.p_comment_c,
                            p_partner_class_c AS ECPartnerClass,
                            p_partner_short_name_c AS ECShortName

                            FROM p_partner_relationship
                            JOIN p_partner ON p_partner.p_partner_key_n = p_partner_relationship.p_partner_key_n
                            WHERE p_relation_name_c LIKE 'EMER%' AND p_relation_key_n "
                        +
                        Selection;

                    if (!AParameters["param_chkEmergencyContacts"].ToBool())
                    {
                        Query += " LIMIT 0";
                    }

                    EmergencyContacts = DbAdapter.RunQuery(Query, "EmergencyContacts", Transaction);

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
    }
}