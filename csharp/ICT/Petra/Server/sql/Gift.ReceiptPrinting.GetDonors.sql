SELECT DISTINCT p_partner.p_partner_key_n, p_partner.p_partner_short_name_c, p_partner.p_status_code_c
FROM a_gift, a_gift_batch, p_partner

{#IFDEF BYEXTRACT}
, m_extract, m_extract_master
{#ENDIF BYEXTRACT}

WHERE a_gift_batch.a_ledger_number_i = ?
AND a_gift_batch.a_gl_effective_date_d BETWEEN ? AND ?
AND a_gift_batch.a_batch_status_c = 'Posted'
AND a_gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
AND a_gift.a_batch_number_i = a_gift_batch.a_batch_number_i
AND p_partner.p_partner_key_n = a_gift.p_donor_key_n
AND upper(p_partner.p_receipt_letter_frequency_c) = ?

{#IFDEF BYEXTRACT}
AND a_gift.p_donor_key_n = m_extract.p_partner_key_n
AND m_extract.m_extract_id_i = m_extract_master.m_extract_id_i
AND m_extract_master.m_extract_name_c = ?
{#ENDIF BYEXTRACT}

ORDER BY p_partner.p_partner_short_name_c