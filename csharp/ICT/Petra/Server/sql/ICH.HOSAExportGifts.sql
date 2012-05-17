--See gi3200.p ln: 170. Select any gift transactions to export
SELECT
		a_general_ledger_master.a_glm_sequence_i,
		a_general_ledger_master.a_ledger_number_i,
        a_general_ledger_master.a_year_i,
        a_general_ledger_master.a_account_code_c,
        a_general_ledger_master.a_cost_centre_code_c,
        a_account.a_account_type_c
    FROM
        public.a_general_ledger_master,
        public.a_cost_centre,
        public.a_account
    WHERE
        a_general_ledger_master.a_ledger_number_i = a_cost_centre.a_ledger_number_i AND
        a_general_ledger_master.a_cost_centre_code_c = a_cost_centre.a_cost_centre_code_c AND
        a_general_ledger_master.a_ledger_number_i = a_account.a_ledger_number_i AND
        a_general_ledger_master.a_account_code_c = a_account.a_account_code_c AND
        a_account.a_posting_status_l = True AND
        a_cost_centre.a_posting_cost_centre_flag_l = True AND
        a_general_ledger_master.a_ledger_number_i = ? AND
        a_general_ledger_master.a_year_i = ? AND
        a_general_ledger_master.a_cost_centre_code_c = ?
    ORDER BY
        a_general_ledger_master.a_account_code_c ASC;