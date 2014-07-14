-- Set all user accounts to be needing a password change
UPDATE s_user SET s_password_needs_change_l = true WHERE s_user_id_c <> 'ANONYMOUS';

-- Specify the usage of XML Reports
INSERT INTO s_system_defaults(s_default_code_c, s_default_description_c, s_default_value_c) VALUES ('USEXMLREPORTS', 'Use OpenPetra XML Report engine instead of external reporting engine', 'Yes');
