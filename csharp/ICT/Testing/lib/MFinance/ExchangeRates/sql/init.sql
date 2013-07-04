INSERT INTO a_ledger (a_ledger_number_i, a_ledger_name_c, a_ledger_status_l, a_base_currency_c, a_forex_gains_losses_account_c, p_partner_key_n)
   VALUES (9997, 'NUnit Test Ledger', 'TRUE', 'BEF', 'Trash', 99970000);
INSERT INTO a_account (a_ledger_number_i, a_account_code_c, a_account_type_c, a_budget_type_code_c) 
   VALUES (9997, '0100', 'NUnit', 'ADHOC');
INSERT INTO a_cost_centre_types (a_ledger_number_i, a_cost_centre_type_c) VALUES (9997, 'Local');
INSERT INTO a_cost_centre (a_ledger_number_i, a_cost_centre_code_c, a_cost_centre_name_c)
   VALUES (9997, '5555', 'NUnit');

