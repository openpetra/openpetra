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
   -- avoid all workers sent by other fields; we are not the home office for them, so there is no treasurer
   AND EXISTS(SELECT * FROM PUB_pm_staff_data, PUB_p_person WHERE PUB_pm_staff_data.p_partner_key_n = PUB_p_person.p_partner_key_n AND PUB_p_person.p_family_key_n = PUB_a_gift_detail.p_recipient_key_n AND PUB_pm_staff_data.pm_home_office_n = ?)
-- for debugging: only one recipient
   AND (PUB_a_gift_detail.p_recipient_key_n = 27061298 OR PUB_a_gift_detail.p_recipient_key_n < 27007000)
-- YEAR(PUB_a_gift_batch.a_gl_effective_date_d), MONTH(PUB_a_gift_batch.a_gl_effective_date_d) does not work for Progress, you cannot use it in a GROUP BY clause   
GROUP BY PUB_a_gift_batch.a_batch_year_i, PUB_a_gift_batch.a_batch_period_i, PUB_a_gift_detail.p_recipient_key_n
ORDER BY PUB_a_gift_detail.p_recipient_key_n