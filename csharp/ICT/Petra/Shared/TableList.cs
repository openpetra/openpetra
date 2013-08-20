// Auto generated with nant generateORMTables
// Do not modify this file manually!
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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

using System.Collections.Generic;

namespace Ict.Petra.Shared
{
    /// <summary>
    /// this returns a list of all database tables, ordered by the most referenced tables,
    /// which need to be created first and can be deleted last
    /// </summary>
    public class TTableList
    {
        /// <summary>
        /// get the names of the tables, ordered by constraint dependancy.
        /// first the tables that other depend upon
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDBNames()
        {
            List<string> list = new List<string>();
            #region DBTableNames
            list.Add("s_user");
            list.Add("p_language");
            list.Add("a_frequency");
            list.Add("p_international_postal_type");
            list.Add("p_country");
            list.Add("a_currency");
            list.Add("s_form");
            list.Add("s_group");
            list.Add("s_user_group");
            list.Add("s_module");
            list.Add("s_valid_output_form");
            list.Add("s_group_module_access_permission");
            list.Add("s_group_table_access_permission");
            list.Add("s_user_module_access_permission");
            list.Add("s_user_table_access_permission");
            list.Add("s_language_specific");
            list.Add("s_login");
            list.Add("s_logon_message");
            list.Add("s_patch_log");
            list.Add("s_reports_to_archive");
            list.Add("s_system_status");
            list.Add("s_user_defaults");
            list.Add("s_system_defaults");
            list.Add("s_error_log");
            list.Add("p_partner_status");
            list.Add("p_acquisition");
            list.Add("p_addressee_type");
            list.Add("p_title");
            list.Add("p_partner_classes");
            list.Add("p_location");
            list.Add("p_location_type");
            list.Add("p_partner_attribute_type");
            list.Add("u_unit_type");
            list.Add("pt_marital_status");
            list.Add("p_occupation");
            list.Add("p_denomination");
            list.Add("p_business");
            list.Add("p_banking_type");
            list.Add("p_banking_details_usage_type");
            list.Add("p_type_category");
            list.Add("p_type");
            list.Add("p_relation_category");
            list.Add("p_relation");
            list.Add("m_extract_type");
            list.Add("m_extract_master");
            list.Add("m_extract_parameter");
            list.Add("p_mailing");
            list.Add("p_address_layout_code");
            list.Add("p_address_layout");
            list.Add("p_address_element");
            list.Add("p_address_line");
            list.Add("p_addressee_title_override");
            list.Add("p_formality");
            list.Add("p_label");
            list.Add("p_merge_form");
            list.Add("p_merge_field");
            list.Add("p_postcode_range");
            list.Add("p_postcode_region");
            list.Add("p_postcode_region_range");
            list.Add("p_publication");
            list.Add("p_publication_cost");
            list.Add("p_reason_subscription_given");
            list.Add("p_reason_subscription_cancelled");
            list.Add("p_contact_attribute");
            list.Add("p_contact_attribute_detail");
            list.Add("p_method_of_contact");
            list.Add("a_sub_system");
            list.Add("a_tax_type");
            list.Add("a_ledger");
            list.Add("a_tax_table");
            list.Add("a_ledger_init_flag");
            list.Add("a_budget_type");
            list.Add("a_account_property_code");
            list.Add("a_cost_centre_types");
            list.Add("a_budget_revision");
            list.Add("a_accounting_period");
            list.Add("a_accounting_system_parameter");
            list.Add("a_analysis_store_table");
            list.Add("a_analysis_type");
            list.Add("a_corporate_exchange_rate");
            list.Add("a_daily_exchange_rate");
            list.Add("p_email");
            list.Add("a_form");
            list.Add("a_form_element_type");
            list.Add("a_form_element");
            list.Add("a_freeform_analysis");
            list.Add("a_method_of_giving");
            list.Add("a_method_of_payment");
            list.Add("a_motivation_group");
            list.Add("a_recurring_batch");
            list.Add("a_batch");
            list.Add("a_special_trans_type");
            list.Add("a_system_interface");
            list.Add("a_currency_language");
            list.Add("a_ar_category");
            list.Add("a_ar_article");
            list.Add("a_ar_article_price");
            list.Add("a_ar_discount");
            list.Add("a_ar_discount_per_category");
            list.Add("a_ar_default_discount");
            list.Add("pt_applicant_status");
            list.Add("pt_application_type");
            list.Add("pt_contact");
            list.Add("pt_special_applicant");
            list.Add("pt_leadership_rating");
            list.Add("pt_arrival_point");
            list.Add("pt_outreach_preference_level");
            list.Add("pt_congress_code");
            list.Add("pt_travel_type");
            list.Add("pm_document_category");
            list.Add("pm_document_type");
            list.Add("pt_passport_type");
            list.Add("pt_language_level");
            list.Add("pt_ability_area");
            list.Add("pt_ability_level");
            list.Add("pt_qualification_area");
            list.Add("pt_qualification_level");
            list.Add("pt_skill_category");
            list.Add("pt_skill_level");
            list.Add("pt_driver_status");
            list.Add("p_data_label_lookup_category");
            list.Add("p_data_label");
            list.Add("p_data_label_use");
            list.Add("p_data_label_lookup");
            list.Add("pm_commitment_status");
            list.Add("pt_position");
            list.Add("pt_assignment_type");
            list.Add("pc_cost_type");
            list.Add("pc_conference_option_type");
            list.Add("pc_discount_criteria");
            list.Add("pc_room_attribute_type");
            list.Add("p_interest_category");
            list.Add("p_interest");
            list.Add("p_reminder_category");
            list.Add("p_process");
            list.Add("p_state");
            list.Add("p_action");
            list.Add("p_first_contact");
            list.Add("p_partner");
            list.Add("p_recent_partners");
            list.Add("p_partner_location");
            list.Add("p_partner_attribute");
            list.Add("p_unit");
            list.Add("um_unit_structure");
            list.Add("p_family");
            list.Add("p_person");
            list.Add("p_church");
            list.Add("p_organisation");
            list.Add("p_bank");
            list.Add("p_venue");
            list.Add("p_banking_details");
            list.Add("p_partner_banking_details");
            list.Add("p_banking_details_usage");
            list.Add("p_partner_tax_deductible_pct");
            list.Add("p_partner_type");
            list.Add("p_partner_relationship");
            list.Add("p_partner_ledger");
            list.Add("m_extract");
            list.Add("p_customised_greeting");
            list.Add("p_subscription");
            list.Add("p_partner_contact");
            list.Add("p_partner_contact_attribute");
            list.Add("a_account");
            list.Add("a_ep_statement");
            list.Add("a_account_property");
            list.Add("a_account_hierarchy");
            list.Add("a_account_hierarchy_detail");
            list.Add("a_email_destination");
            list.Add("a_transaction_type");
            list.Add("a_recurring_journal");
            list.Add("a_journal");
            list.Add("a_suspense_account");
            list.Add("a_ap_supplier");
            list.Add("a_ap_document");
            list.Add("a_crdt_note_invoice_link");
            list.Add("a_ap_payment");
            list.Add("a_ap_document_payment");
            list.Add("a_ep_payment");
            list.Add("a_ep_document_payment");
            list.Add("a_ar_invoice");
            list.Add("a_ar_invoice_detail");
            list.Add("a_ar_invoice_discount");
            list.Add("a_ar_invoice_detail_discount");
            list.Add("pm_general_application");
            list.Add("pm_application_status_history");
            list.Add("pm_short_term_application");
            list.Add("pm_year_program_application");
            list.Add("pm_document");
            list.Add("pm_passport_details");
            list.Add("pm_person_language");
            list.Add("pm_past_experience");
            list.Add("pm_person_ability");
            list.Add("pm_person_qualification");
            list.Add("pm_person_skill");
            list.Add("pm_formal_education");
            list.Add("pm_personal_data");
            list.Add("p_data_label_value_partner");
            list.Add("p_data_label_value_application");
            list.Add("pm_person_evaluation");
            list.Add("pm_person_absence");
            list.Add("pm_special_need");
            list.Add("pm_staff_data");
            list.Add("pm_person_commitment_status");
            list.Add("um_job");
            list.Add("um_job_requirement");
            list.Add("um_job_language");
            list.Add("um_job_qualification");
            list.Add("pm_job_assignment");
            list.Add("um_unit_ability");
            list.Add("um_unit_language");
            list.Add("um_unit_cost");
            list.Add("um_unit_evaluation");
            list.Add("pc_conference");
            list.Add("pc_conference_option");
            list.Add("pc_discount");
            list.Add("pc_attendee");
            list.Add("pc_conference_cost");
            list.Add("pc_extra_cost");
            list.Add("pc_early_late");
            list.Add("pc_group");
            list.Add("pc_supplement");
            list.Add("pc_building");
            list.Add("pc_room");
            list.Add("pc_room_alloc");
            list.Add("pc_room_attribute");
            list.Add("pc_conference_venue");
            list.Add("ph_booking");
            list.Add("ph_room_booking");
            list.Add("p_tax");
            list.Add("p_partner_interest");
            list.Add("p_partner_merge");
            list.Add("p_partner_reminder");
            list.Add("p_partner_field_of_service");
            list.Add("p_partner_short_code");
            list.Add("p_partner_state");
            list.Add("p_partner_action");
            list.Add("a_key_focus_area");
            list.Add("a_cost_centre");
            list.Add("a_valid_ledger_number");
            list.Add("a_budget");
            list.Add("a_budget_period");
            list.Add("a_analysis_attribute");
            list.Add("a_fees_payable");
            list.Add("a_fees_receivable");
            list.Add("a_general_ledger_master");
            list.Add("a_general_ledger_master_period");
            list.Add("a_ich_stewardship");
            list.Add("a_motivation_detail");
            list.Add("a_ep_account");
            list.Add("a_ep_match");
            list.Add("a_ep_transaction");
            list.Add("a_motivation_detail_fee");
            list.Add("a_recurring_transaction");
            list.Add("a_recurring_trans_anal_attrib");
            list.Add("a_recurring_gift_batch");
            list.Add("a_recurring_gift");
            list.Add("a_recurring_gift_detail");
            list.Add("a_gift_batch");
            list.Add("a_gift");
            list.Add("a_gift_detail");
            list.Add("a_processed_fee");
            list.Add("a_transaction");
            list.Add("a_trans_anal_attrib");
            list.Add("a_ap_document_detail");
            list.Add("a_ap_anal_attrib");
            list.Add("s_function");
            list.Add("s_group_function");
            list.Add("s_job_group");
            list.Add("p_partner_set");
            list.Add("s_group_partner_set");
            list.Add("p_partner_set_partner");
            list.Add("s_group_gift");
            list.Add("s_group_motivation");
            list.Add("s_group_partner_contact");
            list.Add("s_group_partner_reminder");
            list.Add("s_group_location");
            list.Add("s_group_partner_location");
            list.Add("s_group_data_label");
            list.Add("s_group_ledger");
            list.Add("s_group_cost_centre");
            list.Add("s_group_extract");
            list.Add("s_change_event");
            list.Add("s_label");
            list.Add("p_partner_comment");
            list.Add("p_proposal_submission_type");
            list.Add("p_foundation");
            list.Add("p_foundation_proposal_status");
            list.Add("p_foundation_proposal");
            list.Add("p_foundation_proposal_detail");
            list.Add("p_foundation_deadline");
            list.Add("s_workflow_definition");
            list.Add("s_workflow_user");
            list.Add("s_workflow_group");
            list.Add("s_workflow_step");
            list.Add("s_function_relationship");
            list.Add("s_workflow_instance");
            list.Add("s_workflow_instance_step");
            list.Add("s_volume");
            list.Add("p_file_info");
            list.Add("p_partner_graphic");
            list.Add("p_partner_file");
            list.Add("pm_person_file");
            list.Add("p_partner_contact_file");
            list.Add("pm_document_file");
            list.Add("pm_application_file");
            list.Add("s_volume_partner_group");
            list.Add("s_default_file_volume");
            list.Add("s_volume_partner_group_partner");
            list.Add("s_group_file_info");

            #endregion
            return list;
        }

        /// <summary>
        /// get the names of the sequences of the whole database
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDBSequenceNames()
        {
            List<string> list = new List<string>();
            #region DBSequenceNames
            list.Add("seq_application");
            list.Add("seq_ap_document");
            list.Add("seq_contact");
            list.Add("seq_extract_number");
            list.Add("seq_location_number");
            list.Add("seq_pe_evaluation_number");
            list.Add("seq_report_number");
            list.Add("seq_general_ledger_master");
            list.Add("seq_budget");
            list.Add("seq_bank_details");
            list.Add("seq_document");
            list.Add("seq_past_experience");
            list.Add("seq_staff_data");
            list.Add("seq_job");
            list.Add("seq_job_assignment");
            list.Add("seq_data_label");
            list.Add("seq_foundation_proposal");
            list.Add("seq_proposal_detail");
            list.Add("seq_form_letter_insert");
            list.Add("seq_workflow");
            list.Add("seq_file_info");
            list.Add("seq_person_skill");
            list.Add("seq_booking");
            list.Add("seq_room_alloc");
            list.Add("seq_ar_invoice");
            list.Add("seq_match_number");
            list.Add("seq_statement_number");
            list.Add("seq_login_process_id");

            #endregion
            return list;
        }
    }
}
