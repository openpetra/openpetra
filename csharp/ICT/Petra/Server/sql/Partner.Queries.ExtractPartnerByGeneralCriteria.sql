SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner ##partner_specific_tables## ##address_filter_tables##
WHERE (? OR pub_p_partner.p_partner_class_c = ?)
    AND (? OR pub_p_partner.p_language_code_c = ?)
    AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
    AND (? OR pub_p_partner.s_created_by_c = ?)
    AND (? OR pub_p_partner.s_modified_by_c = ?)
    AND (? OR pub_p_partner.s_date_created_d >= ?)
    AND (? OR pub_p_partner.s_date_created_d <= ?)
    AND (? OR pub_p_partner.s_date_modified_d >= ?)
    AND (? OR pub_p_partner.s_date_modified_d <= ?)
##partner_specific_where_clause##    
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c, pub_p_partner.p_partner_key_n
##address_filter_order_by_clause##