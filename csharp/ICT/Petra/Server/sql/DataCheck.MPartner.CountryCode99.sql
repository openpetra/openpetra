-- get all locations that have the mail flag set, but the country code is invalid
SELECT pl.p_partner_key_n AS PartnerKey, 
       pl.p_location_key_i AS LocationKey,
      'invalid country code' AS errormessage,
      p.p_partner_short_name_c AS PartnerName,
      l.p_street_name_c AS streetname,
      l.p_city_c AS city

FROM PUB_p_partner_location AS pl, 
     PUB_p_location AS l,
     PUB_p_partner AS p

WHERE pl.p_location_key_i = l.p_location_key_i
  AND p.p_partner_key_n = pl.p_partner_key_n
  AND pl.p_send_mail_l = true
  AND l.p_location_key_i <> 0
  AND l.p_country_code_c = '99'