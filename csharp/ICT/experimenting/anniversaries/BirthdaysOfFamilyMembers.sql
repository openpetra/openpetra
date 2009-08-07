SELECT person.p_partner_key_n AS PartnerKey,
    DAYOFYEAR(person.p_date_of_birth_d) AS DOBThisYear,
    person.p_date_of_birth_d AS DOB,
    person.p_family_name_c AS Surname,
    person.p_first_name_c AS Firstname
FROM PUB_p_person AS person,
    PUB_p_partner AS partner
WHERE person.p_partner_key_n = partner.p_partner_key_n
    AND partner.p_status_code_c = 'ACTIVE'
    AND person.p_family_key_n = ?