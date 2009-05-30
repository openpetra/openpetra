DELETE FROM p_partner_ledger;
DELETE FROM p_partner_location;
DELETE FROM p_unit;
DELETE FROM p_location;
DELETE FROM p_location_type;
DELETE FROM p_partner_type; 
DELETE FROM p_type;
DELETE FROM p_family;
DELETE FROM p_recent_partners;
DELETE FROM p_partner; 
DELETE FROM p_partner_status; 
DELETE FROM p_partner_classes;
DELETE FROM pt_marital_status;
DELETE FROM p_addressee_type;
DELETE FROM p_language; 
DELETE FROM s_system_defaults; 
DELETE FROM a_currency; 
DELETE FROM p_country; 
DELETE FROM p_international_postal_type;
DELETE FROM p_acquisition;
DELETE FROM s_system_status; 
DELETE FROM s_user_module_access_permission; 
DELETE FROM s_login;
DELETE FROM s_logon_message;
DELETE FROM s_user_table_access_permission; 
DELETE FROM s_user_defaults; 
DELETE FROM s_module; 
DELETE FROM s_user; 

-- passwords are inserted by OpenPetra.build
INSERT INTO s_user(s_user_id_c, s_password_hash_c, s_password_salt_c, s_password_needs_change_l) VALUES('SYSADMIN', '{#PASSWORDHASHSYSADMIN}', '{#PASSWORDSALTSYSADMIN}', true);
INSERT INTO s_user(s_user_id_c, s_password_hash_c, s_password_salt_c, s_password_needs_change_l) VALUES('DEMO', '{#PASSWORDHASHDEMO}', '{#PASSWORDSALTDEMO}', true);

-- load base data
COPY s_logon_message FROM '{#ABSOLUTEBASEDATAPATH}/s_logon_message.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_location_type FROM '{#ABSOLUTEBASEDATAPATH}/p_location_type.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_international_postal_type FROM '{#ABSOLUTEBASEDATAPATH}/p_international_postal_type.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_country FROM '{#ABSOLUTEBASEDATAPATH}/p_country.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_language FROM '{#ABSOLUTEBASEDATAPATH}/p_language.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_addressee_type FROM '{#ABSOLUTEBASEDATAPATH}/p_addressee_type.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY a_currency FROM '{#ABSOLUTEBASEDATAPATH}/a_currency.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_acquisition FROM '{#ABSOLUTEBASEDATAPATH}/p_acquisition.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY s_module FROM '{#ABSOLUTEBASEDATAPATH}/s_module.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_type FROM '{#ABSOLUTEBASEDATAPATH}/p_type.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_partner_status FROM '{#ABSOLUTEBASEDATAPATH}/p_partner_status.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_partner_classes FROM '{#ABSOLUTEBASEDATAPATH}/p_partner_classes.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY pt_marital_status FROM '{#ABSOLUTEBASEDATAPATH}/pt_marital_status.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';
COPY p_location FROM '{#ABSOLUTEBASEDATAPATH}/p_location.csv' WITH DELIMITER AS ',' NULL AS '?' CSV QUOTE AS '"' ESCAPE AS '"';

INSERT INTO s_system_status(s_user_id_c,s_system_login_status_l) VALUES('SYSADMIN', true);
INSERT INTO p_partner(p_partner_key_n, p_partner_short_name_c) VALUES(0, 'INVALID PARTNER');
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('LocalisedCountyLabel', 'LocalisedCountyLabel', 'County/St&ate');

-- setup initial user permissions
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('SYSADMIN', 'SYSMAN', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'PTNRUSER', true);
INSERT INTO s_user_module_access_permission(s_user_id_c,s_module_id_c,s_can_access_l) VALUES('DEMO', 'FINANCE-1', true);
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_partner');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_partner_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_location');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_church');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_family');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_person');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_unit');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_bank');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_venue');
INSERT INTO s_user_table_access_permission(s_user_id_c,s_table_name_c) VALUES('DEMO', 'p_organisation');

-- setup the sample site 99000000
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('CurrentDatabaseVersion', 'the currently installed release number, set by installer/patchtool', '3.0.0');
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('SiteKey', 'there has to be one site key for the database', '99000000');

INSERT INTO p_partner(p_partner_key_n, p_partner_short_name_c) VALUES(99000000, 'DEMO SITE'); 
INSERT INTO p_partner_type(p_partner_key_n, p_type_code_c) VALUES(99000000, 'LEDGER'); 
INSERT INTO p_unit(p_partner_key_n) VALUES(99000000); 
INSERT INTO p_location(p_site_key_n, p_location_key_i, p_street_name_c, p_country_code_c) VALUES(99000000, 0, 'No valid address on file', '99');
INSERT INTO p_partner_ledger(p_partner_key_n, p_last_partner_id_i) VALUES(99000000, 5000); 
INSERT INTO p_partner_location(p_partner_key_n, p_site_key_n, p_location_key_i) VALUES(99000000, 99000000, 0);
