SELECT p2.p_partner_key_n,
       p2.p_partner_short_name_c
FROM pub_p_partner JOIN pub_p_partner_relationship 
ON pub_p_partner.p_partner_key_n = pub_p_partner_relationship.p_partner_key_n
JOIN pub_p_partner p2 ON p2.p_partner_key_n = pub_p_partner_relationship.p_relation_key_n
WHERE 
	pub_p_partner_relationship.p_partner_key_n in (
		SELECT pub_p_partner.p_partner_key_n
		FROM pub_p_partner 
			JOIN m_extract ON pub_p_partner.p_partner_key_n = m_extract.p_partner_key_n
			JOIN m_extract_master on m_extract.m_extract_id_i = m_extract_master.m_extract_id_i
		WHERE m_extract_master.m_extract_name_c = ?	
		)
	AND pub_p_partner_relationship.p_relation_name_c IN (?)
    AND pub_p_partner.p_deleted_partner_l = false
    AND pub_p_partner.p_status_code_c = 'ACTIVE'
    --AND pub_p_partner_location.p_send_mail_l = true
	--AND (NOT ? OR pub_p_partner.p_status_code_c = 'ACTIVE')
    --AND (NOT ? OR NOT pub_p_partner.p_no_solicitations_l )
UNION -- Reciprocal relationships
SELECT p2.p_partner_key_n,
       p2.p_partner_short_name_c
FROM pub_p_partner JOIN pub_p_partner_relationship 
ON pub_p_partner.p_partner_key_n = pub_p_partner_relationship.p_relation_key_n
JOIN pub_p_partner p2 ON p2.p_partner_key_n = pub_p_partner_relationship.p_partner_key_n
WHERE 
	pub_p_partner_relationship.p_relation_key_n in (
		SELECT pub_p_partner.p_partner_key_n
		FROM pub_p_partner 
			JOIN m_extract ON pub_p_partner.p_partner_key_n = m_extract.p_partner_key_n
			JOIN m_extract_master on m_extract.m_extract_id_i = m_extract_master.m_extract_id_i
		WHERE m_extract_master.m_extract_name_c = ?	
		)
	AND pub_p_partner_relationship.p_relation_name_c IN (?)
    AND pub_p_partner.p_deleted_partner_l = false
    AND pub_p_partner.p_status_code_c = 'ACTIVE'
    --AND pub_p_partner_location.p_send_mail_l = true

ORDER BY p_partner_short_name_c