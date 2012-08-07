SELECT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_p_partner, pub_p_partner_location, pub_p_location
WHERE pub_p_partner_location.p_partner_key_n = pub_p_partner.p_partner_key_n
    AND pub_p_partner_location.p_location_key_i = pub_p_location.p_location_key_i
    AND pub_p_location.p_city_c LIKE ?
    AND pub_p_partner.p_deleted_partner_l = false
    AND pub_p_partner.p_status_code_c = "ACTIVE"
    AND pub_p_partner_location.p_send_mail_l = true
    AND ? >= pub_p_partner_location.p_date_effective_d 
    AND (? <= pub_p_partner_location.p_date_good_until_d OR pub_p_partner_location.p_date_good_until_d IS NULL)
ORDER BY pub_p_partner.p_partner_short_name_c