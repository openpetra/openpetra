//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Generic;

namespace Ict.Tools.DBXML
{
    /// <summary>
    /// this class knows all the differences
    /// between the current database structure and the previous version
    /// This should perhaps be implemented in XML rather than hardcoded here
    /// </summary>
    public class DataDefinitionDiff
    {
        /// <summary>
        /// the current version
        /// </summary>
        public static string newVersion = "3.0";

        /// <summary>
        /// test if the name of the table has been renamed
        /// </summary>
        /// <param name="oldname">the name of the table in question</param>
        /// <returns>returns the same name or the new name of the table</returns>
        public static string GetNewTableName(String oldname)
        {
            string newname = oldname;

            if (oldname == "pt_om_contact")
            {
                return "pt_contact";
            }
            else if (oldname == "pt_camp_preference_level")
            {
                return "pt_outreach_preference_level";
            }

            return newname;
        }

        /// <summary>
        /// test if the table has been renamed, and return the old name or the current
        /// </summary>
        /// <param name="newname">the table to be tested</param>
        /// <returns>the old or the current name</returns>
        public static string GetOldTableName(String newname)
        {
            string oldname = newname;

            if (newname == "pt_contact")
            {
                return "pt_om_contact";
            }
            else if (newname == "pt_outreach_preference_level")
            {
                return "pt_camp_preference_level";
            }

            return oldname;
        }

        static List <TRenamedField>NewFieldNames = null;

        /// <summary>
        /// see if the column has been renamed
        /// </summary>
        /// <param name="tablename">the table (if table name also has been renamed, need entries for both below)</param>
        /// <param name="oldname">the old field name (returns the old name)</param>
        /// <param name="newname">the new name (returns the new name)</param>
        /// <returns>true if the names are different, the field has been renamed</returns>
        public static Boolean GetNewFieldName(String tablename, ref String oldname, ref String newname)
        {
            if (oldname.Length == 0)
            {
                oldname = newname;
            }
            else
            {
                newname = oldname;
            }

            if (NewFieldNames == null)
            {
                // for 2.1 to 2.2:
                if (newVersion.Equals("2.2"))
                {
                    // for example:
                    // NewFieldNames = new TRenamedField[1];
                    // NewFieldNames[0] = new TRenamedField("a_budget_period", "a_budget_this_year_n", "a_budget_base_n");
                    // if the table name has been renamed as well, need a TRenamedField for both old and new table
                }
                else if (newVersion.Equals("3.0"))
                {
                    NewFieldNames = new List <TRenamedField>();
                    NewFieldNames.Add(new TRenamedField("p_acquisition", "p_recruiting_mission_l", "p_recruiting_effort_l"));
                    NewFieldNames.Add(new TRenamedField("a_ap_anal_attrib", "a_ap_number_i", "a_ap_document_id_i"));
                    NewFieldNames.Add(new TRenamedField("a_ap_document_detail", "a_ap_number_i", "a_ap_document_id_i"));
                    NewFieldNames.Add(new TRenamedField("a_ap_document_payment", "a_ap_number_i", "a_ap_document_id_i"));
                    NewFieldNames.Add(new TRenamedField("p_partner", "p_caleb_id_c", "p_intranet_id_c"));
                    NewFieldNames.Add(new TRenamedField("p_unit", "p_campaign_code_c", "p_outreach_code_c"));
                    NewFieldNames.Add(new TRenamedField("p_unit", "p_campaign_cost_n", "p_outreach_cost_n"));
                    NewFieldNames.Add(new TRenamedField("p_unit", "p_campaign_cost_currency_code_c", "p_outreach_cost_currency_code_c"));
                    NewFieldNames.Add(new TRenamedField("p_family", "p_om_field_key_n", "p_field_key_n"));
                    NewFieldNames.Add(new TRenamedField("p_person", "p_old_omss_family_id_i", "p_family_id_i"));
                    NewFieldNames.Add(new TRenamedField("p_person", "p_om_field_key_n", "p_field_key_n"));
                    NewFieldNames.Add(new TRenamedField("p_church", "p_prayer_cell_l", "p_prayer_group_l"));
                    NewFieldNames.Add(new TRenamedField("a_motivation_detail", "a_export_to_caleb_l", "a_export_to_intranet_l"));
                    NewFieldNames.Add(new TRenamedField("pm_general_application", "pm_gen_om_contact1_c", "pm_gen_contact1_c"));
                    NewFieldNames.Add(new TRenamedField("pm_general_application", "pm_gen_om_contact2_c", "pm_gen_contact2_c"));
                    NewFieldNames.Add(new TRenamedField("pt_congress_code", "pt_campaign_l", "pt_outreach_l"));
                    NewFieldNames.Add(new TRenamedField("pm_short_term_application", "pm_st_campaign_only_flag_l", "pm_st_outreach_only_flag_l"));
                    NewFieldNames.Add(new TRenamedField("pm_short_term_application", "pm_st_recruit_missions_c", "pm_st_recruit_efforts_c"));
                    NewFieldNames.Add(new TRenamedField("pm_short_term_application", "pm_campaign_role_c", "pm_outreach_role_c"));
                    NewFieldNames.Add(new TRenamedField("pm_short_term_application", "pm_st_target_pref_c", "pm_st_activity_pref_c"));
                    NewFieldNames.Add(new TRenamedField("pm_past_experience", "pm_other_mission_org_c", "pm_other_organisation_c"));
                    NewFieldNames.Add(new TRenamedField("pm_past_experience", "pm_prev_om_work_l", "pm_prev_work_here_l"));
                    NewFieldNames.Add(new TRenamedField("pm_personal_data", "pm_om_driver_license_l", "pm_internal_driver_license_l"));
                    NewFieldNames.Add(new TRenamedField("pm_staff_data", "pm_target_field_n", "pm_receiving_field_n"));
                    NewFieldNames.Add(new TRenamedField("pm_staff_data", "pm_target_field_office_n", "pm_receiving_field_office_n"));
                    NewFieldNames.Add(new TRenamedField("pm_commitment_status", "pm_caleb_access_l", "pm_intranet_access_l"));
                    NewFieldNames.Add(new TRenamedField("um_job", "um_previous_om_exp_req_l", "um_previous_internal_exp_req_l"));
                    NewFieldNames.Add(new TRenamedField("pc_conference", "pc_campaign_prefix_c", "pc_outreach_prefix_c"));
                    NewFieldNames.Add(new TRenamedField("pc_attendee", "pc_campaign_type_c", "pc_outreach_type_c"));
                    NewFieldNames.Add(new TRenamedField("pc_supplement", "pc_campaign_type_c", "pc_outreach_type_c"));
                    NewFieldNames.Add(new TRenamedField("pt_om_contact", "pt_om_contact_name_c", "pt_contact_name_c"));
                    NewFieldNames.Add(new TRenamedField("pt_om_contact", "pt_om_contact_descr_c", "pt_contact_descr_c"));
                    NewFieldNames.Add(new TRenamedField("pt_contact", "pt_om_contact_name_c", "pt_contact_name_c"));
                    NewFieldNames.Add(new TRenamedField("pt_contact", "pt_om_contact_descr_c", "pt_contact_descr_c"));
                }
            }

            if (NewFieldNames != null)
            {
                foreach (TRenamedField row in NewFieldNames)
                {
                    if (row.TableName == tablename)
                    {
                        if (newname == row.NewFieldName)
                        {
                            oldname = row.OldFieldName;
                        }
                        else if (oldname == row.OldFieldName)
                        {
                            newname = row.NewFieldName;
                        }
                    }
                }
            }

            return newname != oldname;
        }
    }

    /// <summary>
    /// easy structure for tracking renamed column names
    /// </summary>
    public class TRenamedField
    {
        /// <summary>
        /// the table that the column belongs to
        /// </summary>
        public string TableName;

        /// <summary>
        /// the old name of the column
        /// </summary>
        public String OldFieldName;

        /// <summary>
        /// the new name of the column
        /// </summary>
        public String NewFieldName;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATableName">the table that the column belongs to</param>
        /// <param name="AOldFieldName">the old name of the column</param>
        /// <param name="ANewFieldName">the new name of the column</param>
        public TRenamedField(string ATableName, string AOldFieldName, string ANewFieldName)
        {
            TableName = ATableName;
            OldFieldName = AOldFieldName;
            NewFieldName = ANewFieldName;
        }
    }
}