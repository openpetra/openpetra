SELECT DISTINCT
    person.p_family_key_n AS FamilyKey
FROM PUB_p_person AS person

{#IFDEF ONEPARTNER}
WHERE person.p_partner_key_n = ?
{#ENDIF ONEPARTNER}

{#IFDEF BYEXTRACT}
    , PUB_m_extract,
    PUB_m_extract_master
WHERE person.p_partner_key_n = PUB_m_extract.p_partner_key_n
    AND PUB_m_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i
    AND PUB_m_extract_master.m_extract_name_c = ?
{#ENDIF BYEXTRACT}

{#IFDEF BYSTAFF}
    , PUB_pm_staff_data AS staff
WHERE person.p_partner_key_n = staff.p_partner_key_n
    AND staff.pm_start_of_commitment_d <= ?
    AND (staff.pm_end_of_commitment_d >= ?
        OR staff.pm_end_of_commitment_d IS NULL)
{#ENDIF BYSTAFF}

    AND person.p_family_key_n IS NOT NULL
