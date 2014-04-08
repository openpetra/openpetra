SELECT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c,
       pub_a_gift_detail.a_ledger_number_i,
       pub_a_gift_detail.a_batch_number_i,
       pub_a_gift.a_gift_transaction_number_i,
       pub_a_gift.a_first_time_gift_l,
       pub_a_gift_detail.a_detail_number_i,
       pub_a_gift_detail.a_gift_amount_n,
       pub_a_gift_detail.a_gift_amount_intl_n
       ##address_filter_fields##
FROM pub_a_gift_batch, pub_a_gift, pub_a_gift_detail, pub_p_partner ##address_filter_tables##
WHERE
    pub_a_gift_batch.a_batch_status_c LIKE 'Posted'
    AND pub_a_gift_batch.a_ledger_number_i = pub_a_gift.a_ledger_number_i
    AND pub_a_gift_batch.a_batch_number_i  = pub_a_gift.a_batch_number_i
    AND (? OR pub_a_gift.a_date_entered_d >= ?)
    AND (? OR pub_a_gift.a_date_entered_d <= ?)
    AND (NOT ? OR pub_a_gift.a_first_time_gift_l)
    AND pub_a_gift.a_ledger_number_i = pub_a_gift_detail.a_ledger_number_i
    AND pub_a_gift.a_batch_number_i = pub_a_gift_detail.a_batch_number_i
    AND pub_a_gift.a_gift_transaction_number_i = pub_a_gift_detail.a_gift_transaction_number_i
    AND pub_p_partner.p_partner_key_n = pub_a_gift.p_donor_key_n
    AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE 'FAMILY%')
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
##address_filter_where_clause##  
ORDER BY pub_a_gift.p_donor_key_n, pub_a_gift.a_ledger_number_i, pub_a_gift.a_batch_number_i, pub_a_gift.a_gift_transaction_number_i, pub_a_gift_detail.a_detail_number_i