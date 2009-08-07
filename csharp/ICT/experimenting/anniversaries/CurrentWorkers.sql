SELECT DISTINCT PUB_p_person.p_partner_key_n AS PartnerKey
FROM PUB_pm_staff_data AS staff, PUB_p_person AS person, PUB_p_partner AS partner
WHERE person.p_partner_key_n = staff.p_partner_key_n
        AND person.p_partner_key_n = partner.p_partner_key_n
        -- commitment is in given date range
        -- case 1: start of commitment before date range, open ended
        AND ((staff.pm_start_of_commitment_d <= ? AND staff.pm_end_of_commitment_d IS NULL)
        -- case 2: start of commitment before date range, end date after date range
         OR (staff.pm_start_of_commitment_d <= ? AND staff.pm_end_of_commitment_d >= ?)
        -- case 3: start of commitment in date range
         OR (staff.pm_start_of_commitment_d BETWEEN ? AND ?)
        -- case 4: end of commitment in date range
         OR (staff.pm_end_of_commitment_d BETWEEN ? AND ?))
-- 3.0: AND (staff.pm_home_office_n = ? OR staff.pm_receiving_field_n = ? OR staff.pm_receiving_field_office_n = ?)
        AND (staff.pm_home_office_n = ? OR staff.pm_target_field_n = ? OR staff.pm_target_field_office_n = ?)
        AND partner.p_status_code_c = 'ACTIVE'