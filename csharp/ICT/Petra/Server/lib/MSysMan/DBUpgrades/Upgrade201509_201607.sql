--#################### a_motivation_detail ###########################
-- AFTER a_receipt_l
ALTER TABLE a_motivation_detail ADD COLUMN a_dont_report_l boolean DEFAULT '0';

--#################### p_partner_attribute ###########################
-- AFTER p_value_c
ALTER TABLE p_partner_attribute ADD COLUMN p_value_country_c varchar(400);

--#################### a_ich_stewardship ###########################
-- AFTER a_ledger_number_i
ALTER TABLE a_ich_stewardship ADD COLUMN a_year_i integer DEFAULT 0 NOT NULL;

--#################### a_ledger_init_flag ###########################
-- AFTER a_init_option_name_c
ALTER TABLE a_ledger_init_flag ADD COLUMN a_value_c varchar(128) DEFAULT 'IsSet' NOT NULL;

--#################### s_system_defaults_gui ###########################
CREATE TABLE s_system_defaults_gui (
  s_default_code_c varchar(100) NOT NULL,
  s_control_id_i integer NOT NULL,
  s_control_label_c varchar(100) NOT NULL,
  s_control_type_c varchar(20) NOT NULL,
  s_control_optional_values_c varchar(200),
  s_control_attributes_c varchar(500),
  s_date_created_d date DEFAULT CURRENT_DATE,
  s_created_by_c varchar(20),
  s_date_modified_d date,
  s_modified_by_c varchar(20),
  s_modification_id_t timestamp,
  CONSTRAINT s_system_defaults_gui_pk
    PRIMARY KEY (s_default_code_c,s_control_id_i)
);

ALTER TABLE s_system_defaults_gui
  ADD CONSTRAINT s_system_defaults_gui_fk1
    FOREIGN KEY (s_default_code_c)
    REFERENCES s_system_defaults(s_default_code_c);
ALTER TABLE s_system_defaults_gui
  ADD CONSTRAINT s_system_defaults_gui_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c);
ALTER TABLE s_system_defaults_gui
  ADD CONSTRAINT s_system_defaults_gui_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c);

CREATE UNIQUE INDEX inx_s_system_defaults_gui_pk0
   ON s_system_defaults_gui
   (s_default_code_c,s_control_id_i);
CREATE INDEX inx__system_defaults_gui_fk1_key1
   ON s_system_defaults_gui
   (s_default_code_c);
CREATE INDEX inx_system_defaults_gui_fkcr_key2
   ON s_system_defaults_gui
   (s_created_by_c);
CREATE INDEX inx_system_defaults_gui_fkmd_key3
   ON s_system_defaults_gui
   (s_modified_by_c);

--#################### s_system_defaults ###########################
ALTER TABLE s_system_defaults ALTER COLUMN s_default_description_c TYPE varchar(500);
ALTER TABLE s_system_defaults ADD COLUMN s_default_code_local_c varchar(100);
ALTER TABLE s_system_defaults ADD COLUMN s_default_code_intl_c varchar(100);
ALTER TABLE s_system_defaults ADD COLUMN s_category_c varchar(100);
ALTER TABLE s_system_defaults ADD COLUMN s_read_only_l boolean DEFAULT '1' NOT NULL;
