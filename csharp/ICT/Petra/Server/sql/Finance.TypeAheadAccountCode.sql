SELECT a_account_code_c, a_account_code_short_desc_c
FROM PUB_a_account
WHERE a_ledger_number_i = ?
AND (a_account_code_c LIKE ? OR a_account_code_short_desc_c LIKE ? OR a_account_code_long_desc_c LIKE ?)
AND (NOT ? OR a_posting_status_l)
ORDER BY a_account_code_c
