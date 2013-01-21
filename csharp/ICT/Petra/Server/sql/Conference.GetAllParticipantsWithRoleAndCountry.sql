SELECT PUB_pm_short_term_application.p_partner_key_n AS PartnerKey, 
       PUB_p_partner.p_partner_short_name_c AS ShortName,
       PUB_pm_short_term_application.pm_st_fg_code_c AS FellowshipGroup,
       PUB_pm_short_term_application.pm_st_congress_code_c AS Role,
       PUB_p_unit.p_unit_name_c AS Country
FROM PUB_pm_short_term_application, PUB_pm_general_application, PUB_p_unit, PUB_p_partner
WHERE PUB_pm_short_term_application.pm_confirmed_option_code_c = ?
AND PUB_pm_general_application.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n
AND PUB_pm_general_application.pm_application_key_i = PUB_pm_short_term_application.pm_application_key_i
AND PUB_pm_general_application.pm_registration_office_n = PUB_pm_short_term_application.pm_registration_office_n
AND PUB_pm_general_application.pm_gen_application_status_c = 'A'
AND PUB_p_unit.p_partner_key_n = PUB_pm_short_term_application.pm_registration_office_n
AND PUB_p_partner.p_partner_key_n = PUB_pm_short_term_application.p_partner_key_n
