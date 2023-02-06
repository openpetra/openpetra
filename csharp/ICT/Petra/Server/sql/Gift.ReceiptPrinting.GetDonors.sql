SELECT DISTINCT p.p_partner_key_n, p.p_partner_short_name_c, p.p_status_code_c, pp.p_type_code_c
FROM a_gift, a_gift_batch, p_partner AS p
LEFT OUTER JOIN p_partner_type AS pp ON pp.p_partner_key_n = p.p_partner_key_n AND pp.p_type_code_c = ?

{#IFDEF BYEXTRACT}
, m_extract, m_extract_master
{#ENDIF BYEXTRACT}

WHERE a_gift_batch.a_ledger_number_i = ?
AND a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
AND a_gift_batch.a_batch_status_c = 'Posted'
AND a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
AND p.p_partner_key_n = a_gift.p_donor_key_n
AND (? OR upper(p.p_receipt_letter_frequency_c) = ?)

{#IFDEF VIAEMAIL}
AND EXISTS(SELECT *
    FROM p_subscription
    WHERE p_subscription.p_partner_key_n = a_gift.p_donor_key_n
    AND p_publication_code_c = ?
    AND (p_start_date_d IS NULL OR p_start_date_d <= NOW())
    AND (p_expiry_date_d IS NULL OR p_expiry_date_d <= NOW()))
{#ENDIF VIAEMAIL}

{#IFDEF VIAPRINT}
AND NOT EXISTS(SELECT *
    FROM p_subscription
    WHERE p_subscription.p_partner_key_n = a_gift.p_donor_key_n
    AND p_publication_code_c = ?
    AND (p_start_date_d IS NULL OR p_start_date_d <= NOW())
    AND (p_expiry_date_d IS NULL OR p_expiry_date_d <= NOW()))
{#ENDIF VIAPRINT}

{#IFDEF BYEXTRACT}
AND a_gift.p_donor_key_n = m_extract.p_partner_key_n
AND m_extract.m_extract_id_i = m_extract_master.m_extract_id_i
AND m_extract_master.m_extract_name_c = ?
{#ENDIF BYEXTRACT}

ORDER BY p.p_partner_short_name_c