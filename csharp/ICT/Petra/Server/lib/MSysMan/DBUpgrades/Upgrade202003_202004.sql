INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('BOARDING_SCHOOL','Child at school');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('CHILDREN_HOME,Child at Orphanage');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('HOME_BASED','Child at home');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('PREVIOUS_CHILD','Previous child');
INSERT INTO p_type(p_type_code_c, p_type_description_c) VALUES('CHILD_DIED','Child passed away');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('PARTNERVIEW','View and select Partners');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('SPONSORADMIN','Manage Sponsorships');
INSERT INTO s_module(s_module_id_c,s_module_name_c) VALUES('SPONSORVIEW','View Sponsorships');

ALTER TABLE p_partner_reminder ADD COLUMN p_partner_reminder_id_i integer NOT NULL BEFORE p_partner_key_n;
ALTER TABLE p_partner_reminder MODIFY COLUMN p_contact_id_i bigint;
ALTER TABLE p_partner_reminder MODIFY CONSTRAINT p_partner_reminder_pk PRIMARY KEY (p_partner_reminder_id_i);
ALTER TABLE p_partner_reminder ADD CONSTRAINT p_partner_reminder_uk UNIQUE (p_partner_key_n,p_contact_id_i,p_reminder_id_i);
CREATE TABLE seq_partner_reminder (sequence INTEGER AUTO_INCREMENT, dummy INTEGER, PRIMARY KEY(sequence));

ALTER TABLE s_group_partner_reminder ADD COLUMN p_partner_reminder_id_i integer NOT NULL AFTER s_group_unit_key_n;
ALTER TABLE s_group_partner_reminder MODIFY CONSTRAINT s_group_partner_reminder_pk PRIMARY KEY (s_group_id_c, s_group_unit_key_n, p_partner_reminder_id_i);
ALTER TABLE s_group_partner_reminder MODIFY CONSTRAINT s_group_partner_reminder_fk2 FOREIGN KEY p_partner_reminder (p_partner_reminder_id_i);
ALTER TABLE s_group_partner_reminder DROP COLUMN p_partner_key_n;
ALTER TABLE s_group_partner_reminder DROP COLUMN p_contact_id_i;
ALTER TABLE s_group_partner_reminder DROP COLUMN p_reminder_id_i;
