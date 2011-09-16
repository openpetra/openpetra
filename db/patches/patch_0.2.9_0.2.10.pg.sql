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

CREATE SEQUENCE "seq_login_process_id"
  INCREMENT BY 1
  MINVALUE 0
  MAXVALUE 999999999
  START WITH 2000;
GRANT SELECT,UPDATE,USAGE ON seq_login_process_id TO petraserver;

alter table s_login drop constraint s_login_pk;
alter table s_login add constraint s_login_uk UNIQUE (s_user_id_c, s_login_date_d, s_login_time_i);
alter table s_login add primary key (s_login_process_id_r); 

update s_system_defaults set s_default_value_c = '0.2.10-0' where s_default_code_c = 'CurrentDatabaseVersion';