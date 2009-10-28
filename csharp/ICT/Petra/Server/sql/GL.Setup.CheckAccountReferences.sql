SELECT COUNT(*)
FROM PUB_a_account 
WHERE a_ledger_number_i = ? 
AND a_account_code_c = ?
AND (
-- a_system_account_flag_l = true OR 
a_posting_status_l = false
OR EXISTS (SELECT a_account_code_c FROM PUB_a_transaction WHERE PUB_a_transaction.a_account_code_c = PUB_a_account.a_account_code_c))
-- TODO: check for other references to account as well, eg. bank account of supplier, etc