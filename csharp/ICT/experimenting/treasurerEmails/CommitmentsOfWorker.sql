SELECT PUB_pm_staff_data.pm_status_code_c AS CommitmentStatus
FROM PUB_pm_staff_data, PUB_p_person
WHERE PUB_p_person.p_family_key_n = ?
   AND PUB_pm_staff_data.p_partner_key_n = PUB_p_person.p_partner_key_n
   AND PUB_pm_staff_data.pm_home_office_n = ?
   AND (? BETWEEN PUB_pm_staff_data.pm_start_of_commitment_d AND PUB_pm_staff_data.pm_end_of_commitment_d
     OR (PUB_pm_staff_data.pm_start_of_commitment_d <= ? AND PUB_pm_staff_data.pm_end_of_commitment_d IS NULL))
