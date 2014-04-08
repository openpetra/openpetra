SELECT DISTINCT p_partner.p_partner_key_n, p_partner.p_partner_short_name_c 
FROM a_gift, a_gift_batch, p_partner
WHERE a_gift_batch.a_ledger_number_i = ?
AND a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
AND a_gift_batch.a_batch_status_c = 'Posted'
AND a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
AND p_partner.p_partner_key_n = a_gift.p_donor_key_n
AND p_partner.p_receipt_letter_frequency_c = 'Annual'
ORDER BY p_partner.p_partner_short_name_c