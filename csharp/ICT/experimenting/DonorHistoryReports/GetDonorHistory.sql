-- Get donors by the amount and number of gifts
SELECT PUB_p_partner.p_partner_key_n AS DonorKey,
       PUB_p_partner.p_partner_short_name_c AS DonorShortName, 
--       RecipientPartner.p_partner_short_name_c AS RecipientDescription,
       PUB_a_gift_detail.a_motivation_group_code_c AS MotivationGroupCode,
       PUB_a_gift_detail.a_motivation_detail_code_c AS MotivationDetailCode,
       PUB_a_gift_detail.a_gift_amount_n AS GiftAmount,
       PUB_a_gift_batch.a_gl_effective_date_d AS DateOfGift       
FROM PUB_a_gift_detail, PUB_a_gift, PUB_a_gift_batch, PUB_p_partner
--, PUB_p_partner AS RecipientPartner
WHERE PUB_a_gift_batch.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_batch.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i
-- AND PUB_a_gift_detail.p_recipient_key_n = RecipientPartner.p_partner_key_n
AND PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n

AND PUB_a_gift_batch.a_batch_status_c <> 'Cancelled'

AND PUB_a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
-- what about split gifts?
AND PUB_a_gift_detail.a_gift_amount_n BETWEEN ? AND ?

-- todo: partner class more flexible
AND PUB_p_partner.p_partner_class_c = "FAMILY"
AND PUB_p_partner.p_status_code_c = "ACTIVE"
-- todo: make selection of motivations more flexible
-- AND (NOT (PUB_a_gift_detail.a_motivation_group_code_c = "GIFT" AND PUB_a_gift_detail.a_motivation_detail_code_c = "SUPPORT"))
AND (PUB_a_gift_detail.a_motivation_group_code_c = "PRO" OR (PUB_a_gift_detail.a_motivation_group_code_c = "GIFT" AND ( PUB_a_gift_detail.a_motivation_detail_code_c = "AUSLAND" OR PUB_a_gift_detail.a_motivation_detail_code_c = "FIELD")))

-- todo: filter out non-gifts?
-- AND PUB_a_motivation_detail.a_tax_deductable_l = 1