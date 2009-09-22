SELECT PUB_p_partner_relationship.p_relation_key_n AS TreasurerKey, 
       PUB_p_partner.p_partner_short_name_c AS TreasurerName,
       PUB_p_partner_relationship.p_partner_key_n AS RecipientKey
FROM PUB_p_partner_relationship, PUB_p_partner
WHERE PUB_p_partner_relationship.p_relation_name_c = "TREASURER"
AND PUB_p_partner_relationship.p_partner_key_n = ?
AND PUB_p_partner_relationship.p_relation_key_n = PUB_p_partner.p_partner_key_n
ORDER BY PUB_p_partner.p_partner_short_name_c