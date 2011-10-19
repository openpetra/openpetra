SELECT PUB_a_gift.*, 
       PUB_p_partner.p_partner_short_name_c AS DonorShortName, 
       RecipientPartner.p_partner_short_name_c AS RecipientDescription,
       PUB_a_gift_batch.a_gl_effective_date_d AS DateOfFirstGift,
       PUB_a_gift_detail.a_motivation_group_code_c AS MotivationGroupCode,
       PUB_a_gift_detail.a_motivation_detail_code_c AS MotivationDetailCode
FROM PUB_p_partner, PUB_p_partner AS RecipientPartner, PUB_a_gift_batch, PUB_a_gift, PUB_a_gift_detail
WHERE RecipientPartner.p_partner_key_n = ?
AND PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n

-- only donors of class FAMILY
AND PUB_p_partner.p_partner_class_c = "FAMILY"

-- we need to get all donations, and select the last donation. they might have donated earlier, and paused their connection with us
-- AND PUB_a_gift.a_first_time_gift_l = 1
AND PUB_a_gift_batch.a_ledger_number_i = ?
AND PUB_a_gift_batch.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_batch.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i
-- only use the first gift detail
AND PUB_a_gift_detail.a_detail_number_i = 1
AND PUB_a_gift_detail.p_recipient_key_n = RecipientPartner.p_partner_key_n
AND PUB_a_gift_batch.a_batch_status_c <> 'Cancelled'
ORDER BY PUB_p_partner.p_partner_short_name_c, PUB_a_gift_batch.a_gl_effective_date_d ASC