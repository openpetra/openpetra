SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_a_gift, pub_a_gift_detail, pub_p_partner ##address_filter_tables##
WHERE
    ((? AND a_gift_detail.a_recipient_ledger_number_n <> 0) OR a_gift_detail.a_recipient_ledger_number_n IN (?))
    AND pub_a_gift.a_ledger_number_i = pub_a_gift_detail.a_ledger_number_i
    AND pub_a_gift.a_batch_number_i = pub_a_gift_detail.a_batch_number_i
    AND pub_a_gift.a_gift_transaction_number_i = pub_a_gift_detail.a_gift_transaction_number_i
    AND (? OR pub_a_gift.a_date_entered_d >= ?)
    AND (? OR pub_a_gift.a_date_entered_d <= ?)
    AND (NOT ? OR pub_a_gift.a_first_time_gift_l)
    AND pub_p_partner.p_partner_key_n = pub_a_gift.p_donor_key_n
    AND (NOT ? OR pub_p_partner.p_receipt_each_gift_l)
    AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE 'FAMILY%')
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
    AND (NOT ? OR pub_p_partner.p_receipt_letter_frequency_c = ?)
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##
