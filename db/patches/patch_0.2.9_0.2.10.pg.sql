ALTER TABLE p_unit RENAME COLUMN p_xyz_tbd_code_c TO p_outreach_code_c;
ALTER TABLE p_unit RENAME COLUMN p_xyz_tbd_cost_n TO p_outreach_cost_n;
ALTER TABLE p_unit RENAME COLUMN p_xyz_tbd_cost_currency_code_c TO p_outreach_cost_currency_code_c;

ALTER TABLE pt_xyz_tbd_preference_level RENAME TO pt_outreach_preference_level;

ALTER TABLE pt_congress_code RENAME COLUMN pt_xyz_tbd_l TO pt_outreach_l;

ALTER TABLE pm_short_term_application RENAME COLUMN pm_st_basic_xyz_tbd_identifier_c TO pm_st_basic_outreach_id_c;
ALTER TABLE pm_short_term_application RENAME COLUMN pm_st_xyz_tbd_only_flag_l TO pm_st_outreach_only_flag_l;
ALTER TABLE pm_short_term_application RENAME COLUMN pm_xyz_tbd_role_c TO pm_outreach_role_c;
ALTER TABLE pm_short_term_application RENAME COLUMN pm_st_cmpgn_special_cost_i TO pm_st_outreach_special_cost_i;

ALTER TABLE pc_conference RENAME COLUMN pc_xyz_tbd_prefix_c TO pc_outreach_prefix_c;

ALTER TABLE pc_attendee RENAME COLUMN pc_xyz_tbd_type_c TO pc_outreach_type_c;

ALTER TABLE pc_supplement RENAME COLUMN pc_xyz_tbd_type_c TO pc_outreach_type_c;

