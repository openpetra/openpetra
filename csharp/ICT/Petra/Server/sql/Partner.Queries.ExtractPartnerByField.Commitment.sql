SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner, pub_pm_staff_data ##person_table## ##address_filter_tables##
WHERE pub_pm_staff_data.##sending_or_receiving_field## IN (?)
  AND (? OR pub_pm_staff_data.pm_end_of_commitment_d >= ? OR pub_pm_staff_data.pm_end_of_commitment_d = null)
  AND (? OR pub_pm_staff_data.pm_start_of_commitment_d <= ? OR pub_pm_staff_data.pm_start_of_commitment_d = null)
  ##join_for_person_or_family##
  AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
  AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
 ##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##
