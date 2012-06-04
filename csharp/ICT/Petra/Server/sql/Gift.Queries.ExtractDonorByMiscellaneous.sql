SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_a_gift_batch, pub_a_gift, pub_a_gift_detail, a_motivation_detail, pub_p_partner ##address_filter_tables##
WHERE
        (? OR a_gift_detail.a_ledger_number_i = ?)
    AND (? OR pub_a_gift_detail.p_recipient_key_n = ?)
    AND (? OR pub_a_gift_detail.p_mailing_code_c ##equals_or_like_mailing_code## ?)

    AND pub_a_motivation_detail.a_ledger_number_i = pub_a_gift_detail.a_ledger_number_i
    AND pub_a_motivation_detail.a_motivation_group_code_c = pub_a_gift_detail.a_motivation_group_code_c
    AND pub_a_motivation_detail.a_motivation_detail_code_c = pub_a_gift_detail.a_motivation_detail_code_c
    AND (NOT ? OR pub_a_motivation_detail.a_receipt_l)
    
    AND pub_a_gift.a_ledger_number_i = pub_a_gift_detail.a_ledger_number_i
    AND pub_a_gift.a_batch_number_i = pub_a_gift_detail.a_batch_number_i
    AND pub_a_gift.a_gift_transaction_number_i = pub_a_gift_detail.a_gift_transaction_number_i
    AND (? OR pub_a_gift.a_date_entered_d >= ?)
    AND (? OR pub_a_gift.a_date_entered_d <= ?)
    AND (NOT ? OR pub_a_gift.a_first_time_gift_l)
    AND (? OR pub_a_gift.a_method_of_giving_code_c = ?)
    AND (? OR pub_a_gift.a_method_of_payment_code_c = ?)
    AND (? OR pub_a_gift.a_reference_c = ?)
    ##receipt_letter_code_snippet##

    AND pub_a_gift_batch.a_ledger_number_i = pub_a_gift.a_ledger_number_i
    AND pub_a_gift_batch.a_batch_number_i  = pub_a_gift.a_batch_number_i
    AND (? OR pub_a_gift_batch.a_gift_type_c = ?)
    
    AND pub_p_partner.p_partner_key_n = pub_a_gift.p_donor_key_n
    AND (NOT ? OR pub_p_partner.p_receipt_each_gift_l)
    AND (NOT ? OR pub_p_partner.p_status_code_c = "ACTIVE")
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE "FAMILY%")
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
    AND (NOT ? OR pub_p_partner.p_receipt_letter_frequency_c LIKE ?)
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##
