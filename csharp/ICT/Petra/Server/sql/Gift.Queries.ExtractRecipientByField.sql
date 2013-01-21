SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_a_gift, pub_a_gift_detail, pub_p_partner
WHERE
    ((? AND a_gift_detail.a_recipient_ledger_number_n <> 0) OR a_gift_detail.a_recipient_ledger_number_n IN (?))
    AND pub_a_gift.a_ledger_number_i = pub_a_gift_detail.a_ledger_number_i
    AND pub_a_gift.a_batch_number_i = pub_a_gift_detail.a_batch_number_i
    AND pub_a_gift.a_gift_transaction_number_i = pub_a_gift_detail.a_gift_transaction_number_i
    AND (? OR a_gift.a_date_entered_d >= ?)
    AND (? OR a_gift.a_date_entered_d <= ?)
    AND pub_p_partner.p_partner_key_n = pub_a_gift_detail.p_recipient_key_n
ORDER BY pub_p_partner.p_partner_short_name_c
