SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner,
    pub_p_contact_log LEFT JOIN
    pub_p_partner_contact_attribute
      ON pub_p_contact_log.p_contact_log_id_i = pub_p_partner_contact_attribute.p_contact_id_i,
    pub_p_partner_contact
    ##address_filter_tables##
WHERE pub_p_partner.p_partner_key_n = pub_p_partner_contact.p_partner_key_n
    AND pub_p_partner_contact.p_contact_log_id_i = pub_p_contact_log.p_contact_log_id_i

    AND (NOT ? OR pub_p_contact_log.p_contactor_c = ?)
    AND (NOT ? OR pub_p_contact_log.p_contact_code_c = ?)
    AND (NOT ? OR pub_p_contact_log.p_mailing_code_c = ?)

    AND (NOT ? OR pub_p_partner_contact_attribute.p_contact_attr_detail_code_c IN (?))

    AND (NOT ? OR pub_p_contact_log.s_contact_date_d >= ?) -- After start date
    AND (NOT ? OR pub_p_contact_log.s_contact_date_d <= ?) -- Before end date

    AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE 'FAMILY%')
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l)
##address_filter_where_clause##
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##
