INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('BOARDING_SCHOOL','Child at school');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('CHILDREN_HOME','Child at Orphanage');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('HOME_BASED','Child at home');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('PREVIOUS_CHILD','Previous child');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('CHILD_DIED','Child passed away');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('PARTNERVIEW','View and select Partners');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('SPONSORADMIN','Manage Sponsorships');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('SPONSORVIEW','View Sponsorships');

ALTER TABLE p_partner_reminder ADD COLUMN p_partner_reminder_id_i integer NOT NULL FIRST;
ALTER TABLE p_partner_reminder MODIFY COLUMN p_first_reminder_date_d date DEFAULT NULL;
ALTER TABLE p_partner_reminder DROP PRIMARY KEY;
ALTER TABLE p_partner_reminder ADD CONSTRAINT p_partner_reminder_pk PRIMARY KEY (p_partner_reminder_id_i);
ALTER TABLE p_partner_reminder MODIFY COLUMN `p_event_date_d` date NOT NULL;
ALTER TABLE p_partner_reminder ADD CONSTRAINT p_partner_reminder_uk UNIQUE (p_partner_key_n,p_contact_id_i,p_reminder_id_i);
ALTER TABLE p_partner_reminder MODIFY COLUMN p_contact_id_i bigint DEFAULT NULL;
CREATE TABLE seq_partner_reminder (sequence INTEGER AUTO_INCREMENT, dummy INTEGER, PRIMARY KEY(sequence));

ALTER TABLE s_group_partner_reminder ADD COLUMN p_partner_reminder_id_i integer NOT NULL AFTER s_group_unit_key_n;
ALTER TABLE s_group_partner_reminder DROP PRIMARY KEY;
ALTER TABLE s_group_partner_reminder ADD CONSTRAINT s_group_partner_reminder_pk PRIMARY KEY (s_group_id_c, s_group_unit_key_n, p_partner_reminder_id_i);
ALTER TABLE s_group_partner_reminder DROP FOREIGN KEY s_group_partner_reminder_fk2;
ALTER TABLE s_group_partner_reminder ADD FOREIGN KEY s_group_partner_reminder_fk2(p_partner_reminder_id_i) REFERENCES p_partner_reminder (p_partner_reminder_id_i);
ALTER TABLE s_group_partner_reminder DROP COLUMN p_partner_key_n;
ALTER TABLE s_group_partner_reminder DROP COLUMN p_contact_id_i;
ALTER TABLE s_group_partner_reminder DROP COLUMN p_reminder_id_i;

ALTER TABLE p_family ADD COLUMN `p_date_of_birth_d` date DEFAULT NULL AFTER p_marital_status_comment_c;
ALTER TABLE p_family ADD COLUMN `p_gender_c` varchar(16) DEFAULT 'Unknown' AFTER p_date_of_birth_d;
ALTER TABLE p_family ADD COLUMN `p_photo_b` longtext AFTER p_gender_c;

ALTER TABLE p_contact_log MODIFY COLUMN `p_contact_comment_c` longtext;
ALTER TABLE pm_general_application MODIFY COLUMN `pm_raw_application_data_c` longtext;
ALTER TABLE p_partner MODIFY COLUMN `p_comment_c` longtext;
ALTER TABLE p_partner_comment MODIFY COLUMN `p_comment_c` longtext;
