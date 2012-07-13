SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_m_extract, pub_p_person, pub_p_family, pub_p_partner
WHERE pub_m_extract.m_extract_id_i = ?
    AND pub_p_person.p_partner_key_n = pub_m_extract.p_partner_key_n
    AND pub_p_family.p_partner_key_n = pub_p_person.p_family_key_n
    AND pub_p_partner.p_partner_key_n = pub_p_family.p_partner_key_n
    AND pub_p_partner.p_status_code_c <> "MERGED"
ORDER BY pub_p_partner.p_partner_short_name_c
