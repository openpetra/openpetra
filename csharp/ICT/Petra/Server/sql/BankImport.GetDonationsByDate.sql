SELECT PUB_a_gift_detail.*, PUB_a_gift.p_donor_key_n AS DonorKey, PUB_p_partner.p_partner_short_name_c AS DonorShortName, RecipientPartner.p_partner_short_name_c AS RecipientDescription, false AS AlreadyMatched, PUB_a_gift_batch.a_batch_status_c AS BatchStatus
FROM PUB_a_gift_batch, PUB_a_gift, PUB_a_gift_detail, PUB_p_partner, PUB_p_partner AS RecipientPartner
WHERE PUB_a_gift_batch.a_ledger_number_i = ?
AND PUB_a_gift_batch.a_batch_status_c <> 'Cancelled'
AND PUB_a_gift_batch.a_gl_effective_date_d = ?
AND PUB_a_gift.a_ledger_number_i = PUB_a_gift_batch.a_ledger_number_i
AND PUB_a_gift.a_batch_number_i = PUB_a_gift_batch.a_batch_number_i
AND PUB_a_gift_detail.a_ledger_number_i = PUB_a_gift.a_ledger_number_i
AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i
AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i
AND NOT PUB_a_gift_detail.a_modified_detail_l = true
AND PUB_p_partner.p_partner_key_n = PUB_a_gift.p_donor_key_n
AND RecipientPartner.p_partner_key_n = PUB_a_gift_detail.p_recipient_key_n
