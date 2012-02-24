SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner, pub_p_subscription ##address_filter_tables##
WHERE pub_p_subscription.p_publication_code_c IN (?)
    AND pub_p_partner.p_partner_key_n = pub_p_subscription.p_partner_key_n
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##