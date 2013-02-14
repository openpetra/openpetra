-- updates for Postgresql database for OpenPetra
alter table pm_past_experience alter column pm_start_date_d DROP NOT NULL;
alter table pm_past_experience alter column pm_end_date_d DROP NOT NULL;

alter table a_ap_document add column a_currency_code_c varchar(16) NOT NULL;
alter table a_ap_payment add column a_currency_code_c varchar(16) NOT NULL;
