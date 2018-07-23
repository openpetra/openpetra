SELECT a_cost_centre_code_c, a_cost_centre_name_c
FROM PUB_a_cost_centre
WHERE a_ledger_number_i = ?
AND (a_cost_centre_code_c LIKE ? OR a_cost_centre_name_c LIKE ?)
AND (NOT ? OR a_posting_cost_centre_flag_l)
ORDER BY a_cost_centre_code_c
