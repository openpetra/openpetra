-- updates for Postgresql database for OpenPetra
ALTER TABLE a_gift_detail ADD COLUMN a_modified_detail_key_c varchar(48);
ALTER TABLE a_ep_statement ADD COLUMN a_start_balance_n numeric(24, 10);
ALTER TABLE a_batch ADD COLUMN  a_gift_batch_number_i integer;
ALTER TABLE s_user ADD COLUMN s_email_address_c varchar(100);

alter table pm_past_experience alter column pm_start_date_d DROP NOT NULL;
alter table pm_past_experience alter column pm_end_date_d DROP NOT NULL;

alter table a_ap_document add column a_currency_code_c varchar(16) NOT NULL;
alter table a_ap_payment add column a_currency_code_c varchar(16) NOT NULL;
