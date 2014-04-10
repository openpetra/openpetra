SELECT DISTINCT pub_p_partner.p_partner_key_n,
       pub_p_partner.p_partner_short_name_c
       ##address_filter_fields##
FROM pub_p_partner, pub_pm_staff_data ##address_filter_tables##
WHERE
    pub_p_partner.p_partner_key_n = pub_pm_staff_data.p_partner_key_n
    AND (? OR pub_pm_staff_data.pm_start_of_commitment_d >= ?) -- Start Date from
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
    AND (? OR pub_p_partner.p_status_code_c = 'ACTIVE') -- Active partners only
    AND (? OR NOT pub_p_partner.p_no_solicitations_l) -- Respect no solicitations
##address_filter_where_clause##    
ORDER BY pub_p_partner.p_partner_short_name_c
##address_filter_order_by_clause##