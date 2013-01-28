-- get all locations that have the mail flag set, but the country code is invalid
SELECT pl.p_partner_key_n AS PartnerKey, 
       pl.p_location_key_i AS LocationKey, 
      "invalid country code" AS errormessage

FROM PUB_p_partner_location AS pl, 
     PUB_p_location AS l 

WHERE pl.p_location_key_i = l.p_location_key_i
  AND pl.p_send_mail_l = true
  AND l.p_country_code_c = "99"