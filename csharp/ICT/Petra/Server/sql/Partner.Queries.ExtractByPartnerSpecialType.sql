SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner_type, pub_p_partner ##address_filter_tables##
WHERE pub_p_partner_type.p_type_code_c IN (?)
    AND (NOT ? OR pub_p_partner_type.s_date_created_d > ?)
    AND pub_p_partner.p_partner_key_n = pub_p_partner_type.p_partner_key_n
    AND (NOT ? OR pub_p_partner.p_status_code_c = "ACTIVE")
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE "FAMILY%")
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c, pub_p_partner.p_partner_key_n
##address_filter_order_by_clause##