SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_p_partner, pub_p_subscription
WHERE pub_p_subscription.p_publication_code_c IN (?)
    AND pub_p_partner.p_partner_key_n = pub_p_subscription.p_partner_key_n
ORDER BY pub_p_partner.p_partner_short_name_c
