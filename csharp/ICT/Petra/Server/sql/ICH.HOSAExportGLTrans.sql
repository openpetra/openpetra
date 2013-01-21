-- gi3200.i ln:33. See if there are any GL transactions to export.
SELECT
        Trans.a_ledger_number_i,
        Trans.a_batch_number_i,
        Trans.a_journal_number_i,
        Trans.a_transaction_number_i,
        Trans.a_account_code_c,
        Trans.a_cost_centre_code_c,
        Trans.a_transaction_date_d,
        Trans.a_transaction_amount_n,
        Trans.a_amount_in_base_currency_n,
        Trans.a_amount_in_intl_currency_n,
        Trans.a_ich_number_i,
        Trans.a_system_generated_l,
        Trans.a_narrative_c,
        Trans.a_debit_credit_indicator_l
    FROM
        public.a_transaction AS Trans,
        public.a_journal AS Journal
    WHERE
        Trans.a_ledger_number_i = Journal.a_ledger_number_i
		AND Trans.a_batch_number_i = Journal.a_batch_number_i
        AND Trans.a_journal_number_i = Journal.a_journal_number_i
        AND Trans.a_ledger_number_i = ? 
        AND Trans.a_account_code_c = ? 
        AND Trans.a_cost_centre_code_c = ? 
        AND Trans.a_transaction_status_l = true 
		AND NOT (LEFT(Trans.a_narrative_c, 22) = ? --a_narrative_c BEGINS "Year end re-allocation"
			     AND Trans.a_system_generated_l = true) 
		AND ((Trans.a_ich_number_i + ?) = Trans.a_ich_number_i
		     OR Trans.a_ich_number_i = ?) 
        AND Journal.a_journal_period_i = ?;