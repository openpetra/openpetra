SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner ##person_or_family_table##, pub_p_partner_type ##address_filter_tables##
WHERE ##person_or_family_table_name##.p_field_key_n IN (?)
  ##join_for_person_or_family##
  ##exclude_familiy_members_existing_in_extract##
  AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
  AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
  AND pub_p_partner_type.p_type_code_c LIKE '##worker_type##'
  ##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##
