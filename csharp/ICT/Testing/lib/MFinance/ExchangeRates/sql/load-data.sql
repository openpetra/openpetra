-- Insert posted gift batch data
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 15, 'NUnit Batch 1', 0, 'GBP', '5555', '2000-08-01', 0.5155000000, 25000, 'Posted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 16, 'NUnit Batch 2', 0, 'GBP', '5555', '2000-08-08', 0.5155000000, 30000, 'Posted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 17, 'NUnit Batch 3', 0, 'GBP', '5555', '2000-08-28', 0.5155000000, 15000, 'Posted');

-- Insert unposted gift batch data
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 21, 'NUnit Batch 5', 0, 'GBP', '5555', '2000-10-01', 0.5155000000, 25000, 'Unposted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 22, 'NUnit Batch 6', 0, 'GBP', '5555', '2000-10-05', 0.5225000000, 25000, 'Unposted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 23, 'NUnit Batch 7', 0, 'GBP', '5555', '2000-10-09', 0.5225000000, 25000, 'Unposted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 24, 'NUnit Batch 8', 0, 'GBP', '5555', '2000-10-15', 0.5225000000, 25000, 'Unposted');

-- Insert Journal Table entries for the posted gift batches
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 101);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 102);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 103);
   
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 101, 1, 'Gift Batch 1', 'GR', 'GBP', '2000-08-01', 0.5155000000, 'Posted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 102, 1, 'Gift Batch 2', 'GR', 'GBP', '2000-08-08', 0.5155000000, 'Posted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 103, 1, 'Gift Batch 3', 'GR', 'GBP', '2000-08-28', 0.5155000000, 'Posted');

   -- Insert unposted Journal entries
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 111);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 112);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 113);
   
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 111, 1, 'NUnit Journal 1', 'GL', 'GBP', '2000-10-22', 0.5225000000, 'Unposted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 112, 1, 'NUnit Journal 2', 'GL', 'GBP', '2000-10-26', 0.5225000000, 'Unposted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 113, 1, 'NUnit Journal 3', 'GL', 'GBP', '2000-10-30', 0.5225000000, 'Unposted');
