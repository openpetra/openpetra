-- All possible channels to get data changes
-- GROUP: partner
CREATE TABLE p_consent_channel (

    -- Unique key for channel
  p_channel_code_c varchar(40) NOT NULL,
    -- internatinal name for i18n translation
  p_name_c varchar(128) DEFAULT '0' NOT NULL,
    -- comment for a channel
  p_comment_c varchar(512),
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_consent_channel_pk
    PRIMARY KEY (p_channel_code_c),
  CONSTRAINT p_consent_channel_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c),
  CONSTRAINT p_consent_channel_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c),
KEY inx_p_data_history_fk2_ref0 
   (p_channel_code_c),
UNIQUE KEY inx_p_consent_channel_pk1 
   (p_channel_code_c),
KEY inx_p_consent_channel_fkcr_key2 
   (s_created_by_c),
KEY inx_p_consent_channel_fkmd_key3 
   (s_modified_by_c)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

-- Keeps track of all data changes for GDPR
-- GROUP: partner
CREATE TABLE p_data_history (

    -- Incremental key for entrys
  p_entry_id_i bigint DEFAULT 0 NOT NULL,
    -- Key for partner
  p_partner_key_n bigint DEFAULT 0 NOT NULL,
    -- Type for Data saved
  p_type_c varchar(128) NOT NULL,
    -- Value for type key
  p_value_c varchar(1024),
    -- Code of channel
  p_channel_code_c varchar(40) NOT NULL,
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_data_history_pk
    PRIMARY KEY (p_entry_id_i),
  CONSTRAINT p_data_history_fk1
    FOREIGN KEY (p_partner_key_n)
    REFERENCES p_partner(p_partner_key_n),
  CONSTRAINT p_data_history_fk2
    FOREIGN KEY (p_channel_code_c)
    REFERENCES p_consent_channel(p_channel_code_c),
  CONSTRAINT p_data_history_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c),
  CONSTRAINT p_data_history_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c),
UNIQUE KEY inx_p_data_history_pk0 
   (p_entry_id_i),
KEY inx_p_data_history_fk2_key1 
   (p_channel_code_c),
KEY inx_p_data_history_fkcr_key2 
   (s_created_by_c),
KEY inx_p_data_history_fkmd_key3 
   (s_modified_by_c),
KEY dp_data_history_k0 
   (p_partner_key_n)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

-- All possible use cases for data
-- GROUP: partner
CREATE TABLE p_purpose (

    -- Unique key for purpose
  p_purpose_code_c varchar(40) NOT NULL,
    -- internatinal name for i18n translation
  p_name_c varchar(128) DEFAULT '0' NOT NULL,
    -- comment for a purpose
  p_comment_c varchar(512),
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_purpose_pk
    PRIMARY KEY (p_purpose_code_c),
  CONSTRAINT p_purpose_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c),
  CONSTRAINT p_purpose_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c),
UNIQUE KEY inx_p_purpose_pk0 
   (p_purpose_code_c),
KEY inx_p_purpose_fkcr_key1 
   (s_created_by_c),
KEY inx_p_purpose_fkmd_key2 
   (s_modified_by_c)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

-- Contains for what data can be uses
-- GROUP: partner
CREATE TABLE p_data_history_permission (

    -- ID for entrys
  p_data_history_entry_i bigint DEFAULT 0 NOT NULL,
    -- Code for purpose
  p_purpose_code_c varchar(40) NOT NULL,
    -- The date the record was created.
  s_date_created_d date,
    -- User ID of who created this record.
  s_created_by_c varchar(20),
    -- The date the record was modified.
  s_date_modified_d date,
    -- User ID of who last modified this record.
  s_modified_by_c varchar(20),
    -- This identifies the current version of the record.
  s_modification_id_t timestamp,
  CONSTRAINT p_data_history_permission_pk
    PRIMARY KEY (p_data_history_entry_i,p_purpose_code_c),
  CONSTRAINT p_data_history_permission_fk1
    FOREIGN KEY (p_data_history_entry_i)
    REFERENCES p_data_history(p_entry_id_i),
  CONSTRAINT p_data_history_permission_fk2
    FOREIGN KEY (p_purpose_code_c)
    REFERENCES p_purpose(p_purpose_code_c),
  CONSTRAINT p_data_history_permission_fkcr
    FOREIGN KEY (s_created_by_c)
    REFERENCES s_user(s_user_id_c),
  CONSTRAINT p_data_history_permission_fkmd
    FOREIGN KEY (s_modified_by_c)
    REFERENCES s_user(s_user_id_c),
KEY inx_a_history_permission_fk1_key0 
   (p_data_history_entry_i),
KEY inx_a_history_permission_fk2_key1 
   (p_purpose_code_c),
KEY inx__history_permission_fkcr_key2 
   (s_created_by_c),
KEY inx__history_permission_fkmd_key3 
   (s_modified_by_c),
UNIQUE KEY p_data_history_permission_k0 
   (p_data_history_entry_i,p_purpose_code_c)
) ENGINE=InnoDB
 CHARACTER SET UTF8
;

CREATE TABLE seq_data_entry_id (sequence INTEGER AUTO_INCREMENT, dummy INTEGER, PRIMARY KEY(sequence));
INSERT INTO seq_data_entry_id VALUES(NULL, -1);

INSERT INTO p_consent_channel(p_channel_code_c, p_name_c, p_comment_c) VALUES ('PHONE','phone','Via phone');
INSERT INTO p_consent_channel(p_channel_code_c, p_name_c, p_comment_c) VALUES ('LETTER','letter','Via a letter');
INSERT INTO p_consent_channel(p_channel_code_c, p_name_c, p_comment_c) VALUES ('EMAIL','email','Via E-Mail');
INSERT INTO p_consent_channel(p_channel_code_c, p_name_c, p_comment_c) VALUES ('CONVERSATION','conversation','Via direct conversation');

INSERT INTO p_purpose(p_purpose_code_c, p_name_c, p_comment_c) VALUES ('GR','gift receipting','Can be used for donation processing');
INSERT INTO p_purpose(p_purpose_code_c, p_name_c, p_comment_c) VALUES ('PR','public relations','Can be used for public relations');
INSERT INTO p_purpose(p_purpose_code_c, p_name_c, p_comment_c) VALUES ('NEWSLETTER','newsletter','Can be used to send newsletter');


INSERT INTO s_module_table_access_permission VALUES ('FINANCE-1','p_country',1,1,1,1,NULL,NULL,NULL,NULL,NOW());

ALTER TABLE p_type ADD COLUMN p_system_type_l boolean DEFAULT 0 AFTER p_valid_type_l;
UPDATE p_type SET p_system_type_l = 1 WHERE p_type_code_c IN ('BOARDING_SCHOOL','CHILDREN_HOME','HOME_BASED','PREVIOUS_CHILD','CHILD_DIED','LEDGER');
