SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_p_partner_type, pub_p_partner ##address_filter_table_names## --, pub_p_location, pub_p_partner_location, pub_p_postcode_region, pub_p_postcode_range
WHERE pub_p_partner_type.p_type_code_c IN (?)
    AND (NOT ? OR pub_p_partner_type.s_date_created_d > ?)
    AND pub_p_partner.p_partner_key_n = pub_p_partner_type.p_partner_key_n
    AND (NOT ? OR pub_p_partner.p_status_code_c = "ACTIVE")
--    AND (NOT ? OR pub_p_partner_location.p_send_mail_l) -- Mailable
    AND (NOT ? OR pub_p_partner.p_partner_class_c LIKE "FAMILY%")
    AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
 ##address_filter_where_clause##    
--    AND pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n
--    AND pub_p_location.p_site_key_n = pub_p_partner_location.p_site_key_n
--    AND pub_p_location.p_location_key_i = pub_p_partner_location.p_site_key_n
--    AND (? OR pub_p_location.p_city_c LIKE ?) -- city: search for all beginning with city name
--    AND (? OR pub_p_location.p_country_code_c = ?) -- country code needs to match
--    AND ((? AND ?) -- post code from/to
--         OR (NOT ? AND ?     AND pub_p_location.p_postal_code LIKE ?)
--         OR (NOT ? AND NOT ? AND pub_p_location.p_postal_code >= ? AND pub_p_location.p_postal_code <= ?)
--    AND (? OR (     (     pub_p_postcode_range.p_from_c <= pub_p_location.p_postal_code_c 
--                      AND pub_p_postcode_range.p_to_c   >= pub_p_location.p_postal_code_c)
--                AND (     pub_p_postcode_region.p_range_c  EQ pub_p_postcode_range.p_range_c
--                      AND pub_p_postcode_region.p_region_c EQ ?))
ORDER BY pub_p_partner.p_partner_short_name_c