SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
FROM pub_p_partner
--    LEFT JOIN pub_p_partner_location ON pub_p_partner.p_partner_key_n = pub_p_partner_location.p_partner_key_n
    LEFT JOIN pub_pm_staff_data ON pub_p_partner.p_partner_key_n = pub_pm_staff_data.p_partner_key_n
WHERE
    (? OR pub_pm_staff_data.pm_start_of_commitment_d >= ?) -- Start Date from
    AND (? OR pub_pm_staff_data.pm_start_of_commitment_d <= ?) -- Start Date to
    AND (? OR pub_pm_staff_data.pm_end_of_commitment_d >= ?) -- End Date from
    AND (? OR pub_pm_staff_data.pm_end_of_commitment_d <= ?) -- End Date to
    AND (? OR pub_pm_staff_data.pm_start_of_commitment_d <= ? AND pm_staff_data.pm_end_of_commitment_d >= ?) -- Commitment Valid on
    AND (? OR pub_pm_staff_data.pm_home_office_n = ?) -- Sending Field office
    AND (? OR pub_pm_staff_data.pm_receiving_field_n = ?) -- Receiving Field office
    AND (? OR ( -- Include only persons with selected commmitment stati
        pub_pm_staff_data.pm_status_code_c IN (?) -- Commitment Stati
        OR ? AND pub_pm_staff_data.pm_status_code_c IS NULL -- Include people with no commitment set
        ) )
    AND (? OR pub_p_partner.p_status_code_c = "ACTIVE") -- Active partners only
--    AND (? OR pub_p_partner_location.p_send_mail_l) -- Mailable
    AND (? OR NOT pub_p_partner.p_no_solicitations_l) -- Respect no solicitations
ORDER BY pub_pub_p_partner.p_partner_short_name_c
