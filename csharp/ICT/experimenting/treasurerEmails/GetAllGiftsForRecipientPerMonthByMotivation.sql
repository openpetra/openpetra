-- TODO: what about negative gifts, adjustments? across months?
SELECT PUB_a_gift_detail.p_recipient_key_n AS RecipientKey, 
       SUM(PUB_a_gift_detail.a_gift_transaction_amount_n) AS MonthAmount, 
       COUNT(PUB_a_gift_detail.a_gift_transaction_amount_n) AS MonthCount, 
       PUB_a_gift_batch.a_batch_year_i AS FinancialYear, 
       PUB_a_gift_batch.a_batch_period_i AS FinancialPeriod
FROM PUB_a_gift_detail, PUB_a_gift_batch
WHERE PUB_a_gift_batch.a_ledger_number_i = PUB_a_gift_detail.a_ledger_number_i
   AND PUB_a_gift_batch.a_batch_number_i = PUB_a_gift_detail.a_batch_number_i
   AND PUB_a_gift_batch.a_batch_status_c = "POSTED"
   AND PUB_a_gift_detail.a_ledger_number_i = ?
   AND PUB_a_gift_detail.a_motivation_group_code_c = ?
   AND PUB_a_gift_detail.a_motivation_detail_code_c = ?
   AND PUB_a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
-- make sure that only valid gifts will be processed; it seems there are some gifts without proper a_batch_period_i   
   AND PUB_a_gift_batch.a_batch_period_i > 0
-- for debugging: only one recipient
   AND (PUB_a_gift_detail.p_recipient_key_n = 27061298 OR PUB_a_gift_detail.p_recipient_key_n < 27007000)
-- YEAR(PUB_a_gift_batch.a_gl_effective_date_d), MONTH(PUB_a_gift_batch.a_gl_effective_date_d) does not work for Progress, you cannot use it in a GROUP BY clause   
GROUP BY PUB_a_gift_batch.a_batch_year_i, PUB_a_gift_batch.a_batch_period_i, PUB_a_gift_detail.p_recipient_key_n
ORDER BY PUB_a_gift_detail.p_recipient_key_n