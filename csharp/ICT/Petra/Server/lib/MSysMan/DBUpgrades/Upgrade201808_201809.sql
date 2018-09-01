ALTER TABLE s_user
  ADD COLUMN s_password_reset_token_c varchar(64) DEFAULT NULL AFTER p_partner_key_n;
ALTER TABLE s_user
  ADD COLUMN s_password_reset_valid_until_d date DEFAULT NULL AFTER s_password_reset_token_c;
