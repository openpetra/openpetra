SELECT a_motivation_group_code_c, a_motivation_group_description_c
FROM PUB_a_motivation_group
WHERE a_ledger_number_i = ?
AND (a_motivation_group_code_c LIKE ?
OR a_motivation_group_description_c LIKE ?)
ORDER BY a_motivation_group_code_c
