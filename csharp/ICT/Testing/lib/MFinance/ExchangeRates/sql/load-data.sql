-- This set of data exercises the following scenarios:
--  Data that is only in the DER table and nowhere else
--  Data that is in the DER table and is used elsewhere in both gift and journal
--  Data that is in the DER table that is a matching rate but the wrong time
--  Data that is in the DER table that has the same rate at two times (one matching and one not)

--  Data that appears nowhere in the DER table as follows:
--    Identical rates that appear in both gift and journal at the same time and date
--    Identical rates that appear in both gift and journal at different times on the same date
--    Identical rates that appear in Journal only at different times on the same date
--    Identical rates that appear in gift only on the same date
--    Multiple rates that appear on the same date at the same or different times

-- Insert into Daily Exchange rate
   -- The next two are only in the DER table and are far into the future
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2999-07-01', 7200, 0.53);
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2999-06-01', 7200, 0.52);
   
   -- the next is a rate/date that matches a gift batch (the time is irrelevant)
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2000-10-05', 7200, 0.5225000000);
   
   -- the next is a duplicate rate/date but different time that matches the same gift batch (so appears twice)
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2000-10-05', 14400, 0.5225000000);
   
   -- the next is the right rate/date/time for a journal
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2000-10-22', 0, 0.5225000000);
   
   -- The next is the right rate/date but the wrong time to match a journal
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '2000-10-22', 7200, 0.5225000000);

   -- The next two are only DER rows and are very OLD
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '1900-07-01', 7200, 0.51);
INSERT INTO a_daily_exchange_rate (a_from_currency_code_c, a_to_currency_code_c, a_date_effective_from_d, a_time_effective_from_i, a_rate_of_exchange_n)
   VALUES ('GBP', 'BEF', '1900-06-01', 7200, 0.50);

-- Insert posted gift batch data.
   -- The next two are dates/rates that match a Journal
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 15, 'NUnit Batch 15', 0, 'GBP', '5555', '2000-08-01', 0.5155000000, 25000, 'Posted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 16, 'NUnit Batch 16', 0, 'GBP', '5555', '2000-08-08', 0.5155000000, 30000, 'Posted');
   
   -- Next two are same date/rate so will result in '2 usages' for gift, and also same date/rate/time as a Journal
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 17, 'NUnit Batch 17', 0, 'GBP', '5555', '2000-08-28', 0.5155000000, 15000, 'Posted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 18, 'NUnit Batch 18', 0, 'GBP', '5555', '2000-08-28', 0.5155000000, 15000, 'Posted');
   
   -- The next two are same date but different rates.  No journal
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 19, 'NUnit Batch 19', 0, 'GBP', '5555', '2000-08-30', 0.5185000000, 15000, 'Posted');
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 20, 'NUnit Batch 20', 0, 'GBP', '5555', '2000-08-30', 0.5195000000, 15000, 'Posted');


-- Insert unposted gift batch data
   -- Standalone gift batch using the rate we have used before but a different date
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 21, 'NUnit Batch 21', 0, 'GBP', '5555', '2000-10-01', 0.5155000000, 25000, 'Unposted');

   -- This matches an entry in DER
INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
   VALUES (9997, '0100', 22, 'NUnit Batch 22', 0, 'GBP', '5555', '2000-10-05', 0.5225000000, 25000, 'Unposted');
   
--INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
--   VALUES (9997, '0100', 23, 'NUnit Batch 23', 0, 'GBP', '5555', '2000-10-09', 0.5225000000, 25000, 'Unposted');
--INSERT INTO a_gift_batch (a_ledger_number_i, a_bank_account_code_c, a_batch_number_i, a_batch_description_c, a_batch_year_i, a_currency_code_c, a_bank_cost_centre_c, a_gl_effective_date_d, a_exchange_rate_to_Base_n, a_batch_total_n, a_batch_status_c)
--   VALUES (9997, '0100', 24, 'NUnit Batch 24', 0, 'GBP', '5555', '2000-10-15', 0.5225000000, 25000, 'Unposted');

-- Insert Journal Table entries for the posted gift batches
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 101);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 102);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 103);
   
   -- The next is the same date/rate as used in a Gift
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 101, 1, 'GL Batch 101 + J1', 'GR', 'GBP', 'BEF', '2000-08-01', 0.5155000000, 'Posted');

   -- The next two are stand-alone journal entries with same rate but different dates
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 102, 1, 'GL Batch 102 + J1 has a much longer description', 'GR', 'GBP', 'BEF', '2000-08-08', 0.5155000000, 'Posted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 103, 1, 'GL Batch 103 + J1', 'GR', 'GBP', 'BEF', '2000-08-28', 0.5155000000, 'Posted');

   -- Insert unposted Journal entries
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 111);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 112);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 113);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 114);
INSERT INTO a_batch (a_ledger_number_i, a_batch_number_i)
   VALUES (9997, 115);

   -- This matches a row in DER
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 111, 1, 'GL Batch 111 + J1', 'GL', 'GBP', 'BEF', '2000-10-22', 0.5225000000, 'Unposted');
   
   -- standalone entry using same rate but different date
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 112, 1, 'GL Batch 112 + J1', 'GL', 'GBP', 'BEF', '2000-10-26', 0.5225000000, 'Unposted');

   -- Same rate/date, different journals and same time
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 113, 1, 'GL Batch 113 + J1', 'GL', 'GBP', 'BEF', '2000-10-27', 0.5225000000, 'Unposted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 113, 2, 'GL Batch 113 + J2', 'GL', 'GBP', 'BEF', '2000-10-27', 0.5225000000, 'Unposted');
   
   -- These two are interesting: same rate/date but different journals and different times.  These should get amalgamated to same time
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_time_i, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 114, 1, 'GL Batch 114 + J1', 'GL', 'GBP', 'BEF', '2000-10-28', 7200, 0.5225000000, 'Unposted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_time_i, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 114, 2, 'GL Batch 114 + J2', 'GL', 'GBP', 'BEF', '2000-10-28', 14400, 0.5225000000, 'Unposted');

   -- Two different rates on the same date and same time
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 115, 1, 'GL Batch 115 + J1', 'GL', 'GBP', 'BEF', '2000-10-30', 0.5245000000, 'Unposted');
INSERT INTO a_journal (a_ledger_number_i, a_batch_number_i, a_journal_number_i, a_journal_description_c, a_sub_system_code_c, a_transaction_currency_c, a_base_currency_c, a_date_effective_d, a_exchange_rate_to_base_n, a_journal_status_c)
   VALUES (9997, 115, 2, 'GL Batch 115 + J2', 'GL', 'GBP', 'BEF', '2000-10-30', 0.5275000000, 'Unposted');
