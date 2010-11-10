-- updates for standalone SQLite database for OpenPetra
ALTER TABLE a_ep_match ADD COLUMN a_narrative_c VARCHAR(120);
ALTER TABLE a_ep_statement ADD COLUMN a_ledger_number_i INTEGER DEFAULT 0 NOT NULL;
ALTER TABLE a_ep_statement ADD COLUMN a_bank_account_code_c VARCHAR(8);