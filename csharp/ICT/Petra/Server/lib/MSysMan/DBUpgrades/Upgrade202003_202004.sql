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

CREATE TABLE `s_module_table_access_permission` (
  `s_module_id_c` varchar(20) NOT NULL,
  `s_table_name_c` varchar(64) NOT NULL,
  `s_can_create_l` tinyint(1) DEFAULT '1',
  `s_can_modify_l` tinyint(1) DEFAULT '1',
  `s_can_delete_l` tinyint(1) DEFAULT '1',
  `s_can_inquire_l` tinyint(1) DEFAULT '1',
  `s_date_created_d` date DEFAULT NULL,
  `s_created_by_c` varchar(20) DEFAULT NULL,
  `s_date_modified_d` date DEFAULT NULL,
  `s_modified_by_c` varchar(20) DEFAULT NULL,
  `s_modification_id_t` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`s_module_id_c`,`s_table_name_c`),
  UNIQUE KEY `inx_s_module_table_acc_perm_pk0` (`s_module_id_c`,`s_table_name_c`),
  KEY `inx_odule_table_acc_perm_fk1_key1` (`s_module_id_c`),
  KEY `inx_e_access_permission_fkcr_ke32` (`s_created_by_c`),
  KEY `inx_e_access_permission_fkmd_ke33` (`s_modified_by_c`),
  KEY `gtap_OnTableName0` (`s_table_name_c`),
  CONSTRAINT `s_module_table_acc_perm_fk1` FOREIGN KEY (`s_module_id_c`) REFERENCES `s_module` (`s_module_id_c`),
  CONSTRAINT `s_module_table_access_permission_fkcr` FOREIGN KEY (`s_created_by_c`) REFERENCES `s_user` (`s_user_id_c`),
  CONSTRAINT `s_module_table_access_permission_fkmd` FOREIGN KEY (`s_modified_by_c`) REFERENCES `s_user` (`s_user_id_c`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `s_module_table_access_permission`
VALUES ('FINANCE-1','a_analysis_type',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_ap_document',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_ap_supplier',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_corporate_exchange_rate',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_currency',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_currency_language',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_daily_exchange_rate',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_fees_payable',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_fees_receivable',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_gift_batch',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_journal',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_ledger',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56'),
('FINANCE-1','a_transaction_type',1,1,1,1,NULL,NULL,NULL,NULL,'2020-04-28 04:35:56');


